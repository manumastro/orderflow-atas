using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Tools;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Le zone del valore: la value area di New York di ieri, quella di New York di oggi, e la
/// London finche' New York non apre. Sul chart ci sono solo queste, come bande colorate.
///
/// <para><b>Perche' le calcola l'indicatore e non le regole.</b> Fino al 25 settembre 2026 la banda si
/// ricomponeva dai livelli VAL/VAH/POC depositati dalle regole, e il motore unisce i livelli a meno
/// di otto punti: quel giorno il VAH e il POC di ieri erano finiti dentro quelli della notte, e la
/// value area di ieri non aveva piu' una banda. Una zona non puo' dipendere da quanto e' vicina a
/// un'altra.</para>
///
/// <para><b>I nomi sono quelli di Fabio.</b> Nel live Q1 si dice "London session" e "New York
/// session" (<c>fabio_3.txt</c>, 22:12 e 23:00), mai "Europa", "Asia" o "overnight". La London si
/// vede solo prima dell'apertura di New York: dopo non serve piu', per richiesta dell'utente.</para>
/// </summary>
public sealed partial class DataBridge
{
    [Display(Name = "London da (UTC)", GroupName = "Levels",
        Description = "Da quando si misura la value area della London. Si vede solo prima di New York.")]
    public string LondonDa { get; set; } = "07:00Z";

    private sealed record Zona(string Nome, decimal Val, decimal Vah, decimal Poc, Color Colore, bool Oggi);

    private Zona[] _zone = Array.Empty<Zona>();
    private int _zoneAllaBarra = -1;

    /// <summary>Il delta del tratto a ogni bordo e POC delle zone, calcolato con le zone.</summary>
    private Dictionary<decimal, decimal> _zoneDelta = new();

    /// <summary>Le zone, ricalcolate una volta per barra: OnRender gira a ogni tick.</summary>
    private Zona[] LeZone()
    {
        var ultimo = CurrentBar - 1;
        if (ultimo < 1 || ultimo == _zoneAllaBarra)
        {
            return _zone;
        }

        var zone = new List<Zona>();
        try
        {
            var adesso = GetCandle(ultimo).Time;
            var giorno = GiornoDiMercato(adesso);
            var apertura = Momento(Apertura, giorno);
            var grana = InPunti(1m);
            var passo = InPunti(5m);

            // NEW YORK IERI: la cassa precedente con barre, saltando weekend e festivi.
            for (var indietro = 1; indietro <= 10; indietro++)
            {
                var g = giorno.AddDays(-indietro);
                var p = Profilo(Momento(Apertura, g), Momento(Chiusura, g), passo, grana);
                if (p is not null)
                {
                    zone.Add(new Zona("NY ieri", p.Val, p.Vah, p.Poc, ParseColor(Palette[Seduta.Ieri].Hex), false));
                    break;
                }
            }

            if (adesso >= apertura)
            {
                var p = Profilo(apertura, Momento(Chiusura, giorno), passo, grana);
                if (p is not null)
                {
                    zone.Add(new Zona("NY oggi", p.Val, p.Vah, p.Poc, ParseColor(Palette[Seduta.Oggi].Hex), true));
                }
            }
            else
            {
                var da = Momento(LondonDa, giorno);
                var p = adesso >= da ? Profilo(da, apertura, passo, grana) : null;
                if (p is not null)
                {
                    zone.Add(new Zona("London", p.Val, p.Vah, p.Poc, ParseColor(Palette[Seduta.Europa].Hex), true));
                }
            }
        }
        catch (Exception errore)
        {
            this.LogError("zone del valore non calcolate", errore);
        }

        var delta = new Dictionary<decimal, decimal>();
        try
        {
            var da = InizioDelTratto(ultimo);
            var (_, tapeA) = FinestraDelTape(ultimo);
            foreach (var prezzo in zone.SelectMany(z => new[] { z.Vah, z.Poc, z.Val }).Distinct())
            {
                var m = MisuraAlLivello(prezzo, da, ultimo, tapeA);
                if (m.Volume > 0)
                {
                    delta[prezzo] = m.Delta;
                }
            }
        }
        catch (Exception errore)
        {
            this.LogError("delta delle zone non calcolato", errore);
        }

        _zone = zone.ToArray();
        _zoneDelta = delta;
        _zoneAllaBarra = ultimo;
        return _zone;
    }

    /// <summary>
    /// La banda di ogni zona, col suo nome dentro a sinistra, e il delta ai bordi e al POC.
    /// Niente righe e niente scritte a destra.
    /// </summary>
    private void DisegnaLeZone(RenderContext context, Rectangle area, int dataRight)
    {
        if (!ShowValueBand || ChartInfo is null)
        {
            return;
        }

        var font = new RenderFont("Arial", Math.Max(7, LevelFontSize - 1));
        var alto = context.MeasureString("Hg", font).Height;
        var verde = Color.FromArgb(255, 102, 187, 106);
        var rosso = Color.FromArgb(255, 239, 83, 80);
        var grigio = Color.FromArgb(255, 170, 170, 170);

        foreach (var z in LeZone())
        {
            var yAlto = ChartInfo.GetYByPrice(z.Vah, false);
            var yBasso = ChartInfo.GetYByPrice(z.Val, false);
            var top = Math.Max(area.Top, Math.Min(yAlto, yBasso));
            var bottom = Math.Min(area.Bottom, Math.Max(yAlto, yBasso));
            if (bottom <= top)
            {
                continue;
            }

            context.FillRectangle(Color.FromArgb(ValueBandOpacity, z.Colore),
                new Rectangle(area.Left, top, dataRight - area.Left, bottom - top));

            // Il nome della zona, dentro la banda in alto a sinistra: senza, due bande vicine
            // non si distinguono ed era proprio la lamentela.
            var nome = $"VA {z.Nome}";
            var xNome = area.Left + 70;
            if (bottom - top > alto)
            {
                context.DrawString(nome, font, z.Colore, xNome, top + 1);
            }

            if (!ShowLevelStats)
            {
                continue;
            }

            // Il delta scambiato a quel prezzo nel tratto di seduta, e nient'altro.
            foreach (var prezzo in new[] { z.Vah, z.Poc, z.Val })
            {
                var y = ChartInfo.GetYByPrice(prezzo, false);
                if (y < area.Top + alto || y > area.Bottom - 2)
                {
                    continue;
                }
                if (!_zoneDelta.TryGetValue(prezzo, out var d))
                {
                    continue;
                }
                var testo = $"Δ {Segnato(d)}";
                var size = context.MeasureString(testo, font);
                context.FillRectangle(Color.FromArgb(190, 12, 12, 16),
                    new Rectangle(area.Left + 2, y - size.Height / 2 - 1, size.Width + 8, size.Height + 2));
                context.FillRectangle(z.Colore, new Rectangle(area.Left + 2, y - size.Height / 2 - 1, 2, size.Height + 2));
                context.DrawString(testo, font, d > 0 ? verde : d < 0 ? rosso : grigio,
                    area.Left + 7, y - size.Height / 2);
            }
        }
    }
}
