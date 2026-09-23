using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Tools;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Sforzo, delta, esito e big trades <b>su ogni livello</b>, disegnati a sinistra sulla sua riga.
///
/// <para><b>Perche' non stanno piu' nel pannello.</b> Fino al 23 settembre 2026 il pannello
/// misurava un solo livello, quello "in gioco" entro pochi punti dal prezzo. Tutti gli altri
/// livelli arrivavano sul chart muti: si vedeva dove fossero, non cosa ci fosse successo. E il
/// livello su cui si decide e' spesso proprio quello a venti punti, verso cui il prezzo sta
/// andando — non quello su cui e' gia' seduto.</para>
///
/// <para><b>Stessa misura, stessa finestra, per tutti.</b> Una fascia di due tick attorno al
/// prezzo del livello, sulle ultime <c>PanelLookback</c> barre: e' la misura che il pannello
/// usava per il livello in gioco, e resta identica, cosi' due livelli si confrontano fra loro.
/// La barra in formazione e' dentro, come nel pannello.</para>
/// </summary>
public sealed partial class DataBridge
{
    [Display(Name = "Show stats on every level", GroupName = "Levels",
        Description = "A sinistra, su ogni livello: lotti, compratori/venditori, tiene/passa, big trades.")]
    public bool ShowLevelStats { get; set; } = true;

    /// <summary>Cio' che e' successo a un prezzo, con tutti i numeri che servono a giudicarlo.</summary>
    private readonly record struct MisuraLivello(
        decimal Volume,
        decimal Delta,
        int Tocchi,
        int Respinti,
        int Passati,
        int Big,
        decimal BigNetto,
        bool Copre,
        decimal Fascia);

    private readonly Dictionary<decimal, MisuraLivello> _misure = new();
    private int _misureDa = -1;

    /// <summary>
    /// La prima barra del tratto di seduta in corso: dopo l'apertura la cassa, prima tutto cio'
    /// che viene dalla chiusura precedente. E' la finestra delle etichette a sinistra.
    ///
    /// <para><b>Non le ultime dieci barre.</b> Il primo giro, il 23 settembre, usava la finestra
    /// del pannello: su nove livelli sette dicevano "0 lotti, 0 tocchi", perche' in dieci minuti il
    /// prezzo ne aveva toccati due. Un livello a trenta punti non e' muto perche' non conta: e' muto
    /// perche' la finestra era sbagliata. Sul tratto si vede quante volte la mattina ci e' arrivata
    /// e cosa ha fatto, che e' la domanda.</para>
    /// </summary>
    private int InizioDelTratto(int ultimo)
    {
        var ultimaBarra = GetCandle(ultimo);
        var apertura = Momento(Apertura, ultimaBarra.Time);
        var inizio = ultimaBarra.Time >= apertura
            ? apertura
            : Momento(Chiusura, ultimaBarra.Time).AddDays(-1);
        var primo = ultimo;
        while (primo > 0 && GetCandle(primo - 1) is { } precedente && precedente.Time >= inizio)
        {
            primo--;
        }
        return primo;
    }
    private long _misureBarra = -1;
    private object? _misureLivelli;

    /// <summary>
    /// La misura del livello: quanto si e' scambiato li', chi ha attaccato, quante volte il prezzo
    /// ci e' arrivato e cosa ha fatto, e i big trades a quel prezzo.
    /// </summary>
    private MisuraLivello MisuraAlLivello(decimal livello, int da, int ultimo, DateTime tapeA)
    {
        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        var fascia = tick * 2;
        da = Math.Clamp(da, 0, ultimo);
        decimal vol = 0m;
        decimal delta = 0m;
        var tocchi = 0;
        var respinti = 0;
        var passati = 0;

        for (var bar = da; bar <= ultimo; bar++)
        {
            var c = GetCandle(bar);
            for (var pz = livello - fascia; pz <= livello + fascia; pz += tick)
            {
                var info = c.GetPriceVolumeInfo(pz);
                if (info is null)
                {
                    continue;
                }

                vol += info.Volume;
                delta += info.Ask - info.Bid;
            }

            if (c.Low - fascia <= livello && livello <= c.High + fascia)
            {
                tocchi++;
                if (bar > 0)
                {
                    var lato = GetCandle(bar - 1).Close >= livello;
                    if ((c.Close >= livello) == lato) { respinti++; } else { passati++; }
                }
            }
        }

        var (big, bigNetto, _, copre) = BigTradesAlLivello(
            livello, fascia, GetCandle(da)?.Time ?? DateTime.MinValue, tapeA);
        return new MisuraLivello(vol, delta, tocchi, respinti, passati, big, bigNetto, copre, fascia);
    }

    /// <summary>
    /// Le misure di tutti i livelli, ricalcolate una volta per barra. <c>OnRender</c> gira a ogni
    /// movimento del mouse, e il conto non cambia finche' non cambia la barra o la lista.
    /// </summary>
    private IReadOnlyDictionary<decimal, MisuraLivello> MisureDeiLivelli()
    {
        var ultimo = CurrentBar - 1;
        var livelli = _levels;
        if (ultimo < 1)
        {
            return _misure;
        }

        // La barra in formazione cambia a ogni tick: si ricalcola quando cambia il suo volume,
        // non solo quando nasce una barra nuova.
        var chiave = ultimo * 1_000_000L + (long)GetCandle(ultimo).Volume;
        if (chiave == _misureBarra && ReferenceEquals(livelli, _misureLivelli))
        {
            return _misure;
        }

        _misure.Clear();
        var (_, tapeA) = FinestraDelTape(ultimo);
        _misureDa = InizioDelTratto(ultimo);
        foreach (var l in livelli)
        {
            _misure[l.Price] = MisuraAlLivello(l.Price, _misureDa, ultimo, tapeA);
        }

        _misureBarra = chiave;
        _misureLivelli = livelli;
        return _misure;
    }

    /// <summary>
    /// Un'etichetta a sinistra su ogni livello:
    /// <code>
    ///   [▮▮▮▮▯▯] 140 lotti  Δ -16   8 tocchi · tiene 2 · passa 6   big —
    /// </code>
    /// La barretta e' la quota di acquisti aggressivi (verde) contro vendite aggressive (rosso)
    /// a quel prezzo: si legge prima dei numeri, che e' il motivo per cui c'e'.
    /// </summary>
    private void DisegnaLeMisure(RenderContext context, Rectangle area, BridgeLevel[] livelli)
    {
        if (!ShowLevelStats || ChartInfo is null || livelli.Length == 0)
        {
            return;
        }

        var misure = MisureDeiLivelli();
        var font = new RenderFont("Arial", Math.Max(7, LevelFontSize - 1));
        var grigio = Color.FromArgb(255, 170, 170, 170);
        var verde = Color.FromArgb(255, 102, 187, 106);
        var rosso = Color.FromArgb(255, 239, 83, 80);
        var ambra = Color.FromArgb(255, 255, 183, 77);
        const int larghezzaBarra = 44;

        foreach (var livello in livelli)
        {
            if (!misure.TryGetValue(livello.Price, out var m))
            {
                continue;
            }

            var y = ChartInfo.GetYByPrice(livello.Price, false);
            if (y < area.Top + 10 || y > area.Bottom - 4)
            {
                continue;
            }

            var pezzi = new List<(string Testo, Color Colore)>();
            if (m.Volume <= 0)
            {
                pezzi.Add(("mai toccato in questo tratto", grigio));
            }
            else
            {
                pezzi.Add(($"{Lotti(m.Volume)} lotti", grigio));
                pezzi.Add(($"  Δ {Segnato(m.Delta)}", m.Delta > 0 ? verde : m.Delta < 0 ? rosso : grigio));
            }

            if (m.Tocchi > 0)
            {
                pezzi.Add(($"   {m.Tocchi} tocchi", grigio));
                pezzi.Add(($" · tiene {m.Respinti}", m.Respinti > m.Passati ? verde : grigio));
                pezzi.Add(($" · passa {m.Passati}", m.Passati > m.Respinti ? ambra : grigio));
            }

            // Il registro dei big trades copre solo gli ultimi novanta minuti. Se non copre il
            // tratto non si scrive niente: "big —" direbbe zero, e zero non e' "non lo so".
            if (m.Copre && m.Volume > 0)
            {
                pezzi.Add(m.Big == 0
                    ? ("   big —", grigio)
                    : ($"   big {m.Big} {Segnato(m.BigNetto)}", m.BigNetto > 0 ? verde : m.BigNetto < 0 ? rosso : grigio));
            }

            var alto = context.MeasureString("Hg", font).Height;
            var larghezzaTesto = pezzi.Sum(p => context.MeasureString(p.Testo, font).Width);
            var x = area.Left + 6;
            var top = y - alto - 3;
            var conBarra = m.Volume > 0;
            var larghezza = (conBarra ? larghezzaBarra + 6 : 0) + larghezzaTesto + 10;

            context.FillRectangle(Color.FromArgb(200, 12, 12, 16),
                new Rectangle(x - 4, top - 1, larghezza, alto + 3));
            // Un filo del colore del livello sul bordo sinistro: dice di quale riga e' la misura
            // anche quando due livelli sono vicini.
            context.FillRectangle(ParseColor(livello.Color), new Rectangle(x - 4, top - 1, 2, alto + 3));

            var cx = x + 1;
            if (conBarra)
            {
                var comprati = Math.Clamp((m.Volume + m.Delta) / 2m, 0m, m.Volume);
                var quota = m.Volume > 0 ? (double)(comprati / m.Volume) : 0.5;
                var verdi = (int)Math.Round(larghezzaBarra * quota);
                var hBarra = Math.Max(4, alto - 6);
                var yBarra = top + (alto - hBarra) / 2 + 1;
                context.FillRectangle(Color.FromArgb(230, verde), new Rectangle(cx, yBarra, verdi, hBarra));
                context.FillRectangle(Color.FromArgb(230, rosso),
                    new Rectangle(cx + verdi, yBarra, larghezzaBarra - verdi, hBarra));
                cx += larghezzaBarra + 6;
            }

            foreach (var (testo, colore) in pezzi)
            {
                context.DrawString(testo, font, colore, cx, top);
                cx += context.MeasureString(testo, font).Width;
            }
        }
    }

    /// <summary>Le stesse misure in JSON, perche' l'agente le legga come le vede chi guarda.</summary>
    private object[] MisureInJson()
    {
        var misure = MisureDeiLivelli();
        return _levels
            .Where(l => misure.ContainsKey(l.Price))
            .Select(l =>
            {
                var m = misure[l.Price];
                return (object)new
                {
                    nome = l.Nome,
                    prezzo = l.Price,
                    fascia = m.Fascia,
                    da = _misureDa >= 0 ? Iso(GetCandle(_misureDa).Time) : null,
                    lotti = m.Volume,
                    delta = m.Delta,
                    tocchi = m.Tocchi,
                    tiene = m.Respinti,
                    passa = m.Passati,
                    big = m.Copre ? m.Big : (int?)null,
                    bigNetto = m.Copre ? m.BigNetto : (decimal?)null,
                };
            })
            .ToArray();
    }
}
