using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Tools;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// I muri: i prezzi dove qualcuno di <b>passivo</b> ha retto, e il mercato e' tornato indietro.
///
/// <para><b>Perche' li trova l'indicatore da solo.</b> Un muro non e' una scelta dell'analisi
/// come lo e' "guardo il POC della notte": e' un <b>fatto</b> che o c'e' o non c'e', e cambia
/// ogni pochi minuti. Depositarlo come regola vorrebbe dire dichiarare a mano una finestra e
/// aspettare che qualcuno la riscriva — cioe' il difetto del livello fermo, che tutto questo
/// lavoro esiste per togliere. Qui il calcolo gira a ogni barra e le righe si spostano da sole.</para>
///
/// <para><b>Le quattro prove, e perche' sono quattro.</b> Un muro e' <i>sforzo alto, risultato
/// nullo</i>: tanti contratti scambiati a un prezzo e il prezzo che non si muove. Ma quella
/// coppia da sola descrive anche il <b>POC</b>, che e' il posto piu' scambiato e piu' in pareggio
/// del grafico e non e' affatto un muro. La differenza e' che al POC il prezzo <b>passa sopra</b>,
/// al muro <b>torna indietro</b> — e torna indietro da <b>un lato solo</b>.</para>
///
/// <list type="number">
/// <item><b>sforzo</b> — il volume li' batte di N volte quello del prezzo mediano della finestra;</item>
/// <item><b>pareggio</b> — <c>|delta| / volume</c> sotto soglia: nessuno dei due lati ha vinto;</item>
/// <item><b>tenuta</b> — quel prezzo e' stato il minimo (o il massimo) di almeno N barre;</item>
/// <item><b>asimmetria</b> — e lo e' stato molto piu' spesso da un lato che dall'altro.</item>
/// </list>
///
/// <para><b>La quarta e' quella che tiene in piedi le altre tre</b>, e l'ha dimostrato il mercato
/// il 22 settembre 2026: sulla finestra della notte il prezzo piu' scambiato era il POC
/// <c>30.880</c>, che passava sforzo (3,0x), pareggio (0,13) e dodici ritorni. Asimmetria
/// <b>1,5</b>: girava dodici volte da sotto e otto da sopra. Un prezzo che respinge in tutte e
/// due le direzioni non e' un muro, <b>e' un centro</b>. La stessa mattina, a <c>30.767</c>:
/// dodici minimi contro due massimi, asimmetria <b>6,0</b>.</para>
///
/// <para><b>Quando non c'e' un muro, non si disegna niente.</b> E' meta' del valore di questo
/// calcolo: un muro inventato fa tenere una posizione contro un prezzo che non difende nessuno,
/// ed e' peggio di nessun muro. Il motivo per cui nessun prezzo ha passato le prove finisce nel
/// log e su <c>/muri</c>, col numero mancante accanto.</para>
/// </summary>
public sealed partial class DataBridge
{
    // ------------------------------------------------------------------ le impostazioni

    [Display(Name = "Show walls", GroupName = "Muri",
        Description = "Disegna i muri che l'indicatore trova da solo, senza regole depositate.")]
    public bool MostraMuri { get; set; } = true;

    [Display(Name = "Window from session open", GroupName = "Muri",
        Description = "Dopo l'apertura misura solo la cassa; prima, solo la notte e la mattina.")]
    public bool MuriDallApertura { get; set; } = true;

    [Display(Name = "Session open (UTC)", GroupName = "Muri")]
    public string MuriApertura { get; set; } = "13:30Z";

    [Display(Name = "Session close (UTC)", GroupName = "Muri")]
    public string MuriChiusura { get; set; } = "20:00Z";

    [Display(Name = "Min lots in window", GroupName = "Muri",
        Description = "Sotto questo volume il tratto e' troppo giovane: nessun muro, e lo dice.")]
    [Range(0, 1000000)]
    public decimal MuriLottiMinimi { get; set; } = 15000m;

    [Display(Name = "Window (bars, if not from open)", GroupName = "Muri",
        Description = "Usata solo con 'Window from session open' spento. 360 = sei ore su M1.")]
    [Range(30, 3000)]
    public int MuriFinestra { get; set; } = 360;

    [Display(Name = "Grid (points)", GroupName = "Muri",
        Description = "La griglia su cui si cerca. Sul tick nudo il volume si spalma e non si trova niente.")]
    [Range(0.25, 25)]
    public decimal MuriGrana { get; set; } = 1m;

    [Display(Name = "1 - effort (x median)", GroupName = "Muri",
        Description = "Prova 1: quante volte il volume deve battere quello del prezzo mediano.")]
    [Range(1, 50)]
    public decimal MuriSforzo { get; set; } = 3m;

    [Display(Name = "2 - max |delta|/volume", GroupName = "Muri",
        Description = "Prova 2: sopra questo valore qualcuno ha vinto, ed e' aggressione, non muro.")]
    [Range(0.01, 1)]
    public decimal MuriPareggio { get; set; } = 0.15m;

    [Display(Name = "3 - min rejections", GroupName = "Muri",
        Description = "Prova 3: quante barre devono aver girato su quel prezzo.")]
    [Range(1, 50)]
    public int MuriRespinte { get; set; } = 3;

    [Display(Name = "4 - min asymmetry", GroupName = "Muri",
        Description = "Prova 4: ritorni dal lato giusto / dal lato opposto. Senza questa, il muro esce sul POC.")]
    [Range(1, 20)]
    public decimal MuriAsimmetria { get; set; } = 2m;

    [Display(Name = "Max walls per side", GroupName = "Muri")]
    [Range(1, 10)]
    public int MuriMassimi { get; set; } = 2;

    [Display(Name = "Min separation (points)", GroupName = "Muri",
        Description = "Due muri piu' vicini di cosi' sono lo stesso muro: si tiene il piu' scambiato.")]
    [Range(0, 200)]
    public decimal MuriStacco { get; set; } = 8m;

    [Display(Name = "Font size", GroupName = "Muri")]
    [Range(6, 30)]
    public int MuriFontSize { get; set; } = 11;

    // ------------------------------------------------------------------ il dato

    /// <summary>Un muro trovato, con tutti i numeri che lo giustificano.</summary>
    private readonly record struct Muro(
        decimal Prezzo,
        bool Sotto,
        decimal Volume,
        decimal Sforzo,
        decimal Pareggio,
        int Ritorni,
        int Contrari,
        decimal Asimmetria);

    private Muro[] _muri = Array.Empty<Muro>();
    private string? _muriMotivo;
    private int _muriBarra = -1;
    private DateTime? _muriDa;
    private int _muriBarre;
    private decimal _muriLotti;

    private static readonly Color MuroVerde = Color.FromArgb(255, 102, 187, 106);
    private static readonly Color MuroRosso = Color.FromArgb(255, 239, 83, 80);

    // ------------------------------------------------------------------ il calcolo

    /// <summary>
    /// Ricalcola i muri se la barra e' cambiata. La barra in formazione non entra mai: una
    /// misura presa su una barra aperta si sposta da sola fra un tick e il successivo, e le
    /// righe ballerebbero senza che il mercato abbia fatto niente.
    /// </summary>
    private void AggiornaIMuri()
    {
        var ultimo = CurrentBar - 2;
        if (ultimo < 1 || _muriBarra == ultimo)
        {
            return;
        }

        _muriBarra = ultimo;
        var grana = InPunti(MuriGrana);

        // IL TRATTO, NON LE ULTIME N BARRE. Una finestra scorrevole trascina dentro la mattina
        // europea per ore dopo l'apertura, e i muri restano fermi dove il prezzo non e' piu':
        // il 22 settembre, mezz'ora dopo l'apertura, erano ancora a 165 punti dal prezzo, cioe'
        // inutili proprio nel momento in cui servivano. Dopo l'apertura si guarda **solo la
        // cassa**; prima dell'apertura, **solo cio' che viene prima** — dalla chiusura precedente.
        var primo = Math.Max(0, ultimo - MuriFinestra + 1);
        _muriDa = null;
        if (MuriDallApertura && GetCandle(ultimo) is { } ultimaBarra)
        {
            var apertura = Momento(MuriApertura, ultimaBarra.Time);
            var inizio = ultimaBarra.Time >= apertura
                ? apertura
                : Momento(MuriChiusura, ultimaBarra.Time).AddDays(-1);

            _muriDa = inizio;
            primo = ultimo;
            while (primo > 0 && GetCandle(primo - 1) is { } precedente && precedente.Time >= inizio)
            {
                primo--;
            }
        }

        var volume = new Dictionary<decimal, decimal>();
        var delta = new Dictionary<decimal, decimal>();
        var minimi = new Dictionary<decimal, int>();
        var massimi = new Dictionary<decimal, int>();

        for (var bar = primo; bar <= ultimo; bar++)
        {
            var candle = GetCandle(bar);
            if (candle is null)
            {
                continue;
            }

            var basso = Math.Floor(candle.Low / grana) * grana;
            var alto = Math.Floor(candle.High / grana) * grana;
            minimi[basso] = minimi.GetValueOrDefault(basso) + 1;
            massimi[alto] = massimi.GetValueOrDefault(alto) + 1;

            foreach (var livello in candle.GetAllPriceLevels())
            {
                var prezzo = Math.Floor(livello.Price / grana) * grana;
                volume[prezzo] = volume.GetValueOrDefault(prezzo) + livello.Volume;
                delta[prezzo] = delta.GetValueOrDefault(prezzo) + (livello.Ask - livello.Bid);
            }
        }

        _muriBarre = ultimo - primo + 1;
        _muriLotti = volume.Values.Sum();

        if (volume.Count == 0)
        {
            _muri = Array.Empty<Muro>();
            _muriMotivo = "finestra senza volume";
            return;
        }

        // Nei primi minuti del tratto tutto il volume sta su pochi prezzi, e il "piu' scambiato"
        // lo e' perche' non c'e' altro. Non e' una misura prematura: e' una misura falsa.
        if (_muriLotti < MuriLottiMinimi)
        {
            _muri = Array.Empty<Muro>();
            _muriMotivo = $"ancora presto: {Lotti(_muriLotti)} lotti nel tratto "
                          + $"su {Lotti(MuriLottiMinimi)} richiesti";
            return;
        }

        var ordinati = volume.Values.OrderBy(v => v).ToList();
        var mediana = ordinati[ordinati.Count / 2];
        var richiesto = mediana * MuriSforzo;

        var trovati = new List<Muro>();
        foreach (var sotto in new[] { true, false })
        {
            var giri = sotto ? minimi : massimi;
            var opposti = sotto ? massimi : minimi;
            var tenuti = new List<Muro>();

            foreach (var (prezzo, vol) in volume.OrderByDescending(kv => kv.Value))
            {
                if (vol < richiesto)
                {
                    // La lista e' ordinata per volume: sotto la soglia non c'e' piu' niente.
                    break;
                }

                var pareggio = Math.Abs(delta.GetValueOrDefault(prezzo)) / Math.Max(vol, 1m);
                if (pareggio > MuriPareggio)
                {
                    continue;
                }

                var ritorni = giri.GetValueOrDefault(prezzo);
                if (ritorni < MuriRespinte)
                {
                    continue;
                }

                var contrari = opposti.GetValueOrDefault(prezzo);
                // Senza ritorni contrari non c'e' niente da dividere: l'asimmetria e' piena.
                var asimmetria = contrari > 0 ? (decimal)ritorni / contrari : ritorni;
                if (asimmetria < MuriAsimmetria)
                {
                    continue;
                }

                // Due prezzi a un tick di distanza sono lo stesso muro visto due volte: si tiene
                // il piu' scambiato, che e' il primo perche' la lista e' ordinata.
                if (tenuti.Any(m => Math.Abs(m.Prezzo - prezzo) < InPunti(MuriStacco)))
                {
                    continue;
                }

                tenuti.Add(new Muro(prezzo, sotto, vol, mediana > 0 ? vol / mediana : 0m,
                                    pareggio, ritorni, contrari, asimmetria));
                if (tenuti.Count >= MuriMassimi)
                {
                    break;
                }
            }

            trovati.AddRange(tenuti);
        }

        _muri = trovati.ToArray();

        if (_muri.Length == 0)
        {
            // Il candidato piu' vicino a passare, per poter dire PERCHE' non c'e' un muro.
            // "Nessun muro" e "non lo so" non devono somigliarsi.
            var (prezzo, vol) = volume.OrderByDescending(kv => kv.Value).First();
            var pareggio = Math.Abs(delta.GetValueOrDefault(prezzo)) / Math.Max(vol, 1m);
            var b = minimi.GetValueOrDefault(prezzo);
            var a = massimi.GetValueOrDefault(prezzo);
            _muriMotivo =
                $"nessun muro: il piu' scambiato e' {Prezzo(prezzo)}, "
                + $"sforzo {(mediana > 0 ? vol / mediana : 0m).ToString("0.0", Italiano)}x "
                + $"(ne servono {MuriSforzo.ToString("0.0", Italiano)}), "
                + $"pareggio {pareggio.ToString("0.00", Italiano)} "
                + $"(max {MuriPareggio.ToString("0.00", Italiano)}), "
                + $"{b} minimi contro {a} massimi "
                + $"(ne servono {MuriRespinte} dal lato giusto e "
                + $"asimmetria {MuriAsimmetria.ToString("0.0", Italiano)})";
        }
        else
        {
            _muriMotivo = null;
        }
    }

    // ------------------------------------------------------------------ il disegno

    /// <summary>
    /// Una riga per muro, <b>dalla barra di adesso in avanti</b>, non per tutto l'asse.
    ///
    /// <para>Il muro e' misurato su una finestra che <b>finisce adesso</b>: tirare la riga
    /// indietro su tutta la seduta la farebbe passare sopra ore in cui quel prezzo non aveva
    /// ancora respinto niente. Una riga disegnata dove la misura non vale e' una affermazione
    /// falsa, e a occhio non si distingue da una vera.</para>
    ///
    /// <para>Sulla riga c'e' <b>solo il nome e il prezzo</b>. I numeri che l'hanno fatta nascere
    /// stanno nel tooltip, come per i livelli delle regole: tre muri con la spiegazione intera
    /// scritta addosso coprono le candele proprio nella zona dove il prezzo sta lavorando.</para>
    /// </summary>
    private (string Text, Color Color, Point At)? DisegnaIMuri(
        RenderContext context, Rectangle area, int lineLeft, int dataRight, Point? mouse)
    {
        if (!MostraMuri || _muri.Length == 0 || ChartInfo is null)
        {
            return null;
        }

        var font = new RenderFont("Arial", MuriFontSize);
        (string Text, Color Color, Point At)? tooltip = null;

        foreach (var muro in _muri)
        {
            var y = ChartInfo.GetYByPrice(muro.Prezzo, false);
            if (y < area.Top || y > area.Bottom)
            {
                continue;
            }

            var colore = muro.Sotto ? MuroVerde : MuroRosso;
            var sinistra = Math.Clamp(lineLeft, area.Left, dataRight);
            context.DrawLine(new RenderPen(colore, 2), sinistra, y, dataRight, y);

            var verso = muro.Sotto ? "MURO SOTTO" : "MURO SOPRA";
            var difesa = muro.Sotto ? "il ribasso" : "il rialzo";
            var chi = muro.Sotto ? "un compratore fermo" : "un venditore fermo";
            var breve = $"{verso} {Prezzo(muro.Prezzo)}";
            var intero =
                $"{breve} · "
                + $"{muro.Ritorni} ritorni contro {muro.Contrari} "
                + $"(asimmetria {muro.Asimmetria.ToString("0.0", Italiano)}) · "
                + $"{Lotti(muro.Volume)} lotti, {muro.Sforzo.ToString("0.0", Italiano)}x il mediano · "
                + $"delta pari ({muro.Pareggio.ToString("0.00", Italiano)}) · "
                + $"qui {difesa} ha trovato {chi}";

            var size = context.MeasureString(breve, font);
            var x = Math.Max(area.Left + 6, sinistra + 6);
            // Fondo pieno: sopra un footprint denso il testo nudo non si legge.
            var riquadro = new Rectangle(x - 3, y - size.Height - 2, size.Width + 6, size.Height + 2);
            context.FillRectangle(Color.FromArgb(205, 0, 0, 0), riquadro);
            context.DrawString(breve, font, colore, x, y - size.Height - 1);

            // Il perche' si legge passando sopra, non prima: e' la stessa convenzione dei livelli.
            if (mouse is { } m)
            {
                var banda = new Rectangle(sinistra, y - 4, Math.Max(1, dataRight - sinistra), 8);
                if (riquadro.Contains(m) || banda.Contains(m))
                {
                    tooltip = (intero, colore, m);
                }
            }
        }

        return tooltip;
    }

    // ------------------------------------------------------------------ l'endpoint

    private object Muri()
    {
        AggiornaIMuri();
        return new
        {
            schema = "fof-data-bridge-v1",
            instrument = InstrumentInfo?.Instrument,
            barra = _muriBarra,
            tratto = _muriDa is null
                ? $"ultime {MuriFinestra} barre"
                : (_muriDa.Value.TimeOfDay == Momento(MuriApertura, _muriDa.Value).TimeOfDay
                    ? "dalla apertura di cassa"
                    : "dalla chiusura precedente"),
            da = _muriDa,
            barre = _muriBarre,
            lotti = _muriLotti,
            soglie = new
            {
                sforzo = MuriSforzo,
                pareggio = MuriPareggio,
                respinte = MuriRespinte,
                asimmetria = MuriAsimmetria,
            },
            motivo = _muriMotivo,
            muri = _muri.Select(m => new
            {
                prezzo = m.Prezzo,
                lato = m.Sotto ? "sotto" : "sopra",
                lotti = m.Volume,
                sforzo = Math.Round(m.Sforzo, 2),
                pareggio = Math.Round(m.Pareggio, 3),
                ritorni = m.Ritorni,
                contrari = m.Contrari,
                asimmetria = Math.Round(m.Asimmetria, 2),
            }).ToArray(),
        };
    }
}
