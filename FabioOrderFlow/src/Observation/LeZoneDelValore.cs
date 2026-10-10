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
    private sealed record Zona(string Nome, string Sigla, decimal Val, decimal Vah, decimal Poc, Color Colore, bool Oggi,
        DateTime? Giorno = null);

    private Zona[] _zone = Array.Empty<Zona>();

    /// <summary>Larghezza della colonna delle scritte a destra, e fin dove arrivano quelle agganciate in
    /// alto: il pannello si mette fuori da tutte e due.</summary>
    private int _colonnaDestra;

    private int _agganciateSopra;

    /// <summary>Il profilo della zona in formazione, per la gaussiana sul bordo sinistro.</summary>
    private ProfiloFinestra? _profiloOggi;
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
        ProfiloFinestra? profiloOggi = null;
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
                zone.Add(new Zona($"{sigla} CASH", sigla, p.Val, p.Vah, p.Poc, colore, false, g));
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
                            trovata.P.Poc, ParseColor(Palette[Seduta.Framing].Hex), false, trovata.Giorno));
                    }
                }
            }

            if (adesso >= apertura)
            {
                var p = Profilo(apertura, Momento(Chiusura, giorno), passo, grana);
                if (p is not null)
                {
                    zone.Add(new Zona("NY oggi", "NY", p.Val, p.Vah, p.Poc, ParseColor(Palette[Seduta.Oggi].Hex), true));
                    profiloOggi = p;
                }
            }
            else
            {
                var da = Momento(LondonDa, giorno);
                var p = adesso >= da ? Profilo(da, apertura, passo, grana) : null;
                if (p is not null)
                {
                    zone.Add(new Zona("London", "LONDON", p.Val, p.Vah, p.Poc, ParseColor(Palette[Seduta.Europa].Hex), true));
                    profiloOggi = p;
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
        _profiloOggi = profiloOggi;
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
        var livelli = LeZone().Where(z => !z.Oggi)
            .SelectMany(z => new[] { (P: z.Vah, S: "VAH", Z: z), (P: z.Poc, S: "POC", Z: z), (P: z.Val, S: "VAL", Z: z) })
            .Select(x => (x.P, Y: ChartInfo.GetYByPrice(x.P, false), Testo: $"{x.Z.Sigla} {x.S} CASH", x.Z.Colore))
            .ToList();
        var chiuse = livelli.Where(x => x.Y >= area.Top + alto && x.Y <= area.Bottom).OrderBy(x => x.Y).ToList();
        var ultimoAlto = int.MinValue;
        foreach (var (_, y, testo, colore) in chiuse)
        {
            var sz = context.MeasureString(testo, font);
            // La scritta sta sopra la sua lineetta; se tocca quella prima scende, la lineetta no.
            var yTesto = Math.Max(y - sz.Height - 1, ultimoAlto + 1);
            ultimoAlto = yTesto + sz.Height;
            var x = dataRight - MargineLvn - sz.Width - 6;
            context.FillRectangle(Color.FromArgb(170, 0, 0, 0), new Rectangle(x - 2, yTesto, sz.Width + 6, sz.Height));
            context.DrawString(testo, font, colore, x, yTesto);
            context.DrawLine(new RenderPen(Color.FromArgb(230, colore), 2), x - 2, y, dataRight - MargineLvn, y);
        }

        // FUORI SCHERMO NON VUOL DIRE SPARITO. Il 28 settembre 2026 venerdi' stava 200 punti sopra
        // il prezzo e, zoomato sulla mattina, non si vedeva: "non vedo quello di venerdi". Le
        // scritte si agganciano al bordo, con la freccia e la distanza dal prezzo.
        var prezzoOra = GetCandle(CurrentBar - 1).Close;
        var sopra = livelli.Where(x => x.Y < area.Top + alto).OrderBy(x => x.P).ToList();
        var sotto = livelli.Where(x => x.Y > area.Bottom).OrderByDescending(x => x.P).ToList();
        var colonna = 0;
        foreach (var (_, _, testo, _) in chiuse)
        {
            colonna = Math.Max(colonna, context.MeasureString(testo, font).Width + 8);
        }

        var yBordo = area.Top + 2;
        foreach (var (p, _, testo, colore) in sopra)
        {
            var t = $"▲ {testo} {Prezzo(p)} (+{Prezzo(p - prezzoOra)})";
            var sz = context.MeasureString(t, font);
            var x = dataRight - MargineLvn - sz.Width - 6;
            context.FillRectangle(Color.FromArgb(170, 0, 0, 0), new Rectangle(x - 2, yBordo, sz.Width + 6, sz.Height));
            context.DrawString(t, font, colore, x, yBordo);
            yBordo += sz.Height + 1;
            colonna = Math.Max(colonna, sz.Width + 8);
        }

        _agganciateSopra = sopra.Count > 0 ? yBordo : area.Top;
        yBordo = area.Bottom - 2;
        foreach (var (p, _, testo, colore) in sotto)
        {
            var t = $"▼ {testo} {Prezzo(p)} (-{Prezzo(prezzoOra - p)})";
            var sz = context.MeasureString(t, font);
            yBordo -= sz.Height + 1;
            colonna = Math.Max(colonna, sz.Width + 8);
            var x = dataRight - MargineLvn - sz.Width - 6;
            context.FillRectangle(Color.FromArgb(170, 0, 0, 0), new Rectangle(x - 2, yBordo, sz.Width + 6, sz.Height));
            context.DrawString(t, font, colore, x, yBordo);
        }

        _colonnaDestra = colonna;

        // LA ZONA IN FORMAZIONE, NY oggi o London: niente banda (1 ottobre 2026, richiesta
        // dell'utente), ma VAH, POC e VAL come lineette sul bordo sinistro, specchio delle sedute
        // chiuse a destra: "LONDON VAL 30.792  Δ-35". Il delta e' quello scambiato al prezzo nel
        // tratto di seduta.
        foreach (var z in LeZone().Where(z => z.Oggi))
        {
            // LA GAUSSIANA DELLA SEDUTA IN CORSO, sotto le lineette (9 ottobre 2026): sull'M1 la
            // responsabilita' e' la seduta di adesso, e "low in the curve" (fabio_1.txt 1:24:28) si
            // legge sulla forma, non su tre numeri. Fasce da 5 punti, al massimo 140 pixel.
            if (_profiloOggi is { } po && po.Fasce.Count > 0)
            {
                var massimo = po.Fasce.Values.Max();
                foreach (var (fascia, volume) in po.Fasce)
                {
                    var yAlto = ChartInfo.GetYByPrice(fascia + po.Passo, false);
                    var yBasso = ChartInfo.GetYByPrice(fascia, false);
                    if (massimo <= 0 || yBasso < area.Top || yAlto > area.Bottom)
                    {
                        continue;
                    }

                    var dentro = fascia + po.Passo > z.Val && fascia <= z.Vah;
                    context.FillRectangle(Color.FromArgb(dentro ? 70 : 28, z.Colore),
                        new Rectangle(area.Left, yAlto, Math.Max(1, (int)(volume / massimo * 140)), Math.Max(1, yBasso - yAlto - 1)));
                }
            }

            var bordi = new[] { (P: z.Vah, S: "VAH"), (P: z.Poc, S: "POC"), (P: z.Val, S: "VAL") }
                .Select(x => (x.P, x.S, Y: ChartInfo.GetYByPrice(x.P, false)))
                .Where(x => x.Y >= area.Top + alto && x.Y <= area.Bottom)
                .OrderBy(x => x.Y)
                .ToList();
            var ultimoBasso = int.MinValue;
            foreach (var (p, sigla, y) in bordi)
            {
                var delta = ShowLevelStats && _zoneDelta.TryGetValue(p, out var d) ? $"  Δ {Segnato(d)}" : string.Empty;
                var testo = $"{z.Sigla} {sigla} {Prezzo(p)}{delta}";
                var sz = context.MeasureString(testo, font);
                var yTesto = Math.Max(y - sz.Height - 1, ultimoBasso + 1);
                ultimoBasso = yTesto + sz.Height;
                var x = area.Left + 4;
                context.FillRectangle(Color.FromArgb(170, 0, 0, 0), new Rectangle(x - 2, yTesto, sz.Width + 6, sz.Height));
                context.DrawString(testo, font, z.Colore, x, yTesto);
                context.DrawLine(new RenderPen(Color.FromArgb(230, z.Colore), sigla == "POC" ? 3 : 2),
                    area.Left, y, x + sz.Width + 4, y);
            }
        }
    }
}
