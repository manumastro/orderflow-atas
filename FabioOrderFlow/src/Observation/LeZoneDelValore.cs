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
/// Le zone del valore: le value area delle ultime due sedute di New York, quella di una seduta
/// piu' indietro quando il profile framing la chiama, quella di New York di oggi, e la London
/// finche' New York non apre. Sul chart ci sono solo queste, come bande colorate.
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

    private static readonly string[] Giorni =
        { "DOMENICA", "LUNEDI", "MARTEDI", "MERCOLEDI", "GIOVEDI", "VENERDI", "SABATO" };

    /// <summary>
    /// Una zona del valore. <see cref="Sigla"/> e' il prefisso delle scritte a destra
    /// ("PD", "GIOVEDI"): "PD VAL CASH" si legge in un colpo d'occhio, "VAL NY ven 25 30.802" no.
    /// </summary>
    private sealed record Zona(string Nome, string Sigla, decimal Val, decimal Vah, decimal Poc, Color Colore, bool Oggi);

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

            // LE SEDUTE DI CASSA PRECEDENTI, dalla piu' recente, saltando weekend e festivi.
            var sedute = new List<(DateTime Giorno, ProfiloFinestra P)>();
            for (var indietro = 1; indietro <= 20 && sedute.Count < 12; indietro++)
            {
                var g = giorno.AddDays(-indietro);
                var p = Profilo(Momento(Apertura, g), Momento(Chiusura, g), passo, grana);
                if (p is not null)
                {
                    sedute.Add((g, p));
                }
            }

            // LE ULTIME DUE, sempre: "below the cash session of the previous two days"
            // (fabio_4.txt 11:57) e' la cornice su cui Fabio decide se un long ha senso.
            for (var i = 0; i < Math.Min(2, sedute.Count); i++)
            {
                var (g, p) = sedute[i];
                var colore = ParseColor(Palette[i == 0 ? Seduta.Ieri : Seduta.DueGiorni].Hex);
                // PD, previous day, per ieri; il nome del giorno per quello prima.
                var sigla = i == 0 ? "PD" : Giorni[(int)g.DayOfWeek];
                zone.Add(new Zona($"{sigla} CASH", sigla, p.Val, p.Vah, p.Poc, colore, false));
            }

            // IL FRAMING, dinamico. Se il prezzo e' fuori dal valore delle ultime due sedute, si
            // torna indietro fino alla prima seduta il cui valore lo contiene: e' li' che il
            // profile framing trova il prossimo riferimento (il 28 settembre 2026, sotto giovedi'
            // e venerdi', il valore di lunedi' 21 e il suo VAL 30.485). Se nessuna lo contiene,
            // la piu' recente col valore oltre il prezzo, dalla parte in cui e' uscito.
            if (sedute.Count > 2)
            {
                // L'ultima barra CHIUSA: con quella in formazione la zona comparirebbe e sparirebbe
                // a ogni tick intorno al bordo.
                var prezzo = GetCandle(ultimo - 1).Close;
                var basso = sedute.Take(2).Min(x => x.P.Val);
                var alto = sedute.Take(2).Max(x => x.P.Vah);
                if (prezzo < basso || prezzo > alto)
                {
                    var prima = sedute.Skip(2).ToList();
                    var trovata = prima.FirstOrDefault(x => x.P.Val <= prezzo && prezzo <= x.P.Vah);
                    if (trovata.P is null)
                    {
                        trovata = prima.FirstOrDefault(x => prezzo < basso ? x.P.Vah < prezzo : x.P.Val > prezzo);
                    }
                    if (trovata.P is not null)
                    {
                        var sigla = Giorni[(int)trovata.Giorno.DayOfWeek];
                        zone.Add(new Zona($"{sigla} CASH (framing)", sigla, trovata.P.Val, trovata.P.Vah,
                            trovata.P.Poc, ParseColor(Palette[Seduta.Framing].Hex), false));
                    }
                }
            }

            if (adesso >= apertura)
            {
                var p = Profilo(apertura, Momento(Chiusura, giorno), passo, grana);
                if (p is not null)
                {
                    zone.Add(new Zona("NY oggi", "NY", p.Val, p.Vah, p.Poc, ParseColor(Palette[Seduta.Oggi].Hex), true));
                }
            }
            else
            {
                var da = Momento(LondonDa, giorno);
                var p = adesso >= da ? Profilo(da, apertura, passo, grana) : null;
                if (p is not null)
                {
                    zone.Add(new Zona("London", "LONDON", p.Val, p.Vah, p.Poc, ParseColor(Palette[Seduta.Europa].Hex), true));
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
    /// Le sedute chiuse come scritte a destra con una lineetta; la zona in formazione come banda,
    /// col suo nome dentro a sinistra, il POC pieno e il delta ai bordi.
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

        // LE SEDUTE CHIUSE: solo scritte a destra, con una lineetta sotto che parte dal bordo
        // destro e segna il prezzo. Dal 28 settembre 2026, per richiesta dell'utente: le righe
        // orizzontali intere di tre sedute, piu' la banda di oggi, riempivano il chart.
        var chiuse = LeZone().Where(z => !z.Oggi)
            .SelectMany(z => new[] { (z.Vah, "VAH", z), (z.Poc, "POC", z), (z.Val, "VAL", z) })
            .Select(x => (Y: ChartInfo.GetYByPrice(x.Item1, false), Testo: $"{x.z.Sigla} {x.Item2} CASH", x.z.Colore))
            .Where(x => x.Y >= area.Top + alto && x.Y <= area.Bottom)
            .OrderBy(x => x.Y)
            .ToList();
        var ultimoAlto = int.MinValue;
        foreach (var (y, testo, colore) in chiuse)
        {
            var sz = context.MeasureString(testo, font);
            // La scritta sta sopra la sua lineetta; se tocca quella prima scende, la lineetta no.
            var yTesto = Math.Max(y - sz.Height - 1, ultimoAlto + 1);
            ultimoAlto = yTesto + sz.Height;
            var x = dataRight - sz.Width - 6;
            context.FillRectangle(Color.FromArgb(170, 0, 0, 0), new Rectangle(x - 2, yTesto, sz.Width + 6, sz.Height));
            context.DrawString(testo, font, colore, x, yTesto);
            context.DrawLine(new RenderPen(Color.FromArgb(230, colore), 2), x - 2, y, dataRight, y);
        }

        // LA ZONA IN FORMAZIONE, NY oggi o London: banda, POC pieno col nome, delta ai bordi.
        foreach (var z in LeZone().Where(z => z.Oggi))
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

            // IL POC, l'unica riga che resta: il bersaglio del mean reverting dentro la zona.
            // Pieno, del colore della zona, con il nome accanto e niente a destra.
            var yPoc = ChartInfo.GetYByPrice(z.Poc, false);
            if (yPoc >= area.Top && yPoc <= area.Bottom)
            {
                context.DrawLine(new RenderPen(Color.FromArgb(220, z.Colore), 2), area.Left, yPoc, dataRight, yPoc);
                var etichetta = $"POC {z.Nome} {Prezzo(z.Poc)}";
                var s = context.MeasureString(etichetta, font);
                context.FillRectangle(Color.FromArgb(190, 0, 0, 0),
                    new Rectangle(area.Left + 67, yPoc - s.Height - 2, s.Width + 6, s.Height + 2));
                context.DrawString(etichetta, font, z.Colore, area.Left + 70, yPoc - s.Height - 1);
            }

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
