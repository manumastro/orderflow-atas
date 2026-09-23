using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow.Observation;

/// <summary>
/// La scala dello strumento: quanto vale "un punto" rispetto al NQ, su cui l'indicatore e' tarato.
///
/// <para><b>Perche' serve.</b> Le soglie in punti — lo stacco fra due livelli (8), il raggio del
/// livello in gioco (12), lo stacco di un'invalidazione (10), la distanza fra due muri (8) — sono
/// state tarate sul NQ, che fa circa 290 punti di range in una cassa. Sull'oro la cassa ne fa 48:
/// otto punti d'oro sono un sesto della giornata, e con lo stacco del NQ due livelli d'oro distanti
/// cinque punti — che sono lontani — diventavano "lo stesso livello". Idem i big trades: 60 lotti di
/// NQ sono una frazione del libro, 60 lotti d'oro sono un evento raro.</para>
///
/// <para><b>Come si misura, dichiarato.</b> Sul NQ la scala e' 1, fissa: e' lo strumento del metodo
/// e le sue soglie non devono muoversi da sole. Su ogni altro strumento si misura sulle sedute di
/// cassa in memoria (la cassa dello strumento, fino a sei):</para>
/// <list type="bullet">
/// <item><b>scala dei punti</b> = mediana del range di cassa / <c>290</c>, la mediana del NQ sulle
/// sedute 15-22 settembre 2026;</item>
/// <item><b>scala dei lotti</b> = mediana del volume di cassa / <c>346.567</c>, la mediana del NQ
/// sulle stesse sedute. Serve alla soglia dei big trades.</item>
/// </list>
/// <para>Misurato il 23 settembre: GCZ6 range 48,2 contro 290,2, <b>scala 0,166</b>; volume 52.263
/// contro 346.567. Entrambe si possono forzare dalle proprieta' dell'istanza.</para>
///
/// <para><b>Uso di studio.</b> Lo strumento operativo resta il NQ (CLAUDE.md, seconda decisione):
/// questa scala esiste perche' l'indicatore dica cose sensate anche su un altro chart, non perche'
/// il metodo si estenda.</para>
/// </summary>
public sealed partial class DataBridge
{
    private const decimal RangeCassaNq = 290m;
    private const decimal VolumeCassaNq = 346_567m;

    [Display(Name = "Point scale (0 = auto)", GroupName = "Instrument",
        Description = "Quanto vale un punto rispetto al NQ. 0 = 1 sul NQ, misurata sugli altri strumenti.")]
    [Range(0, 100)]
    public decimal ScalaPunti { get; set; }

    [Display(Name = "Lot scale (0 = auto)", GroupName = "Instrument",
        Description = "Quanto vale un lotto rispetto al NQ, per la soglia dei big trades. 0 = automatica.")]
    [Range(0, 100)]
    public decimal ScalaLotti { get; set; }

    private DateTime _scalaDelGiorno = DateTime.MinValue;
    private decimal _scalaPuntiMisurata = 1m;
    private decimal _scalaLottiMisurata = 1m;

    private bool EIlNq
    {
        get
        {
            var s = (InstrumentInfo?.Instrument ?? string.Empty).ToUpperInvariant();
            return s.StartsWith("NQ", StringComparison.Ordinal) || s.StartsWith("MNQ", StringComparison.Ordinal);
        }
    }

    private bool EIlCrude
    {
        get
        {
            var s = (InstrumentInfo?.Instrument ?? string.Empty).ToUpperInvariant();
            return s.StartsWith("CL", StringComparison.Ordinal) || s.StartsWith("MCL", StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// L'apertura della cassa per questo strumento. Sul crude il pit NYMEX apre alle 13:00Z e
    /// chiude alle 18:30Z; su NQ e oro la cassa e' 13:30Z-20:00Z (per l'oro scelta sul volume il 18
    /// settembre). Un valore scritto nelle proprieta' vince, **salvo il default del NQ su un chart di
    /// crude**: e' quasi sempre il template ereditato, non una scelta.
    /// </summary>
    private string Apertura => EIlCrude && (string.IsNullOrWhiteSpace(MuriApertura) || MuriApertura == "13:30Z")
        ? "13:00Z"
        : string.IsNullOrWhiteSpace(MuriApertura) ? "13:30Z" : MuriApertura;

    private string Chiusura => EIlCrude && (string.IsNullOrWhiteSpace(MuriChiusura) || MuriChiusura == "20:00Z")
        ? "18:30Z"
        : string.IsNullOrWhiteSpace(MuriChiusura) ? "20:00Z" : MuriChiusura;

    /// <summary>Moltiplicatore dei punti: 1 sul NQ.</summary>
    private decimal Scala
    {
        get
        {
            if (ScalaPunti > 0) { return ScalaPunti; }
            if (EIlNq) { return 1m; }
            MisuraLaScala();
            return _scalaPuntiMisurata;
        }
    }

    /// <summary>Moltiplicatore dei lotti: 1 sul NQ.</summary>
    private decimal ScalaDeiLotti
    {
        get
        {
            if (ScalaLotti > 0) { return ScalaLotti; }
            if (EIlNq) { return 1m; }
            MisuraLaScala();
            return _scalaLottiMisurata;
        }
    }

    /// <summary>Punti del NQ convertiti nei punti di questo strumento, a multipli di tick.</summary>
    private decimal InPunti(decimal puntiNq)
    {
        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        var v = puntiNq * Scala;
        return Math.Max(tick, Math.Round(v / tick, MidpointRounding.AwayFromZero) * tick);
    }

    /// <summary>La soglia dei big trades per questo strumento: 60 lotti sul NQ.</summary>
    private int SogliaBig => EIlNq && ScalaLotti <= 0
        ? SogliaBigTrade
        : Math.Max(3, (int)Math.Round(SogliaBigTrade * ScalaDeiLotti));

    /// <summary>Una volta al giorno di mercato: le sedute di cassa in memoria, fino a sei.</summary>
    private void MisuraLaScala()
    {
        var ultimo = CurrentBar - 1;
        if (ultimo < 1)
        {
            return;
        }

        var oggi = GetCandle(ultimo).Time.Date;
        if (oggi == _scalaDelGiorno)
        {
            return;
        }

        var sedute = new Dictionary<DateTime, (decimal Alto, decimal Basso, decimal Volume)>();
        for (var bar = ultimo; bar >= 0; bar--)
        {
            var c = GetCandle(bar);
            var giorno = c.Time.Date;
            if (giorno < oggi.AddDays(-12))
            {
                break;
            }

            var ora = c.Time.TimeOfDay;
            if (giorno == oggi || ora < Momento(Apertura, giorno).TimeOfDay || ora >= Momento(Chiusura, giorno).TimeOfDay)
            {
                continue;
            }

            sedute[giorno] = sedute.TryGetValue(giorno, out var s)
                ? (Math.Max(s.Alto, c.High), Math.Min(s.Basso, c.Low), s.Volume + c.Volume)
                : (c.High, c.Low, c.Volume);
        }

        var ultime = sedute.OrderByDescending(kv => kv.Key).Take(6).Select(kv => kv.Value).ToList();
        if (ultime.Count > 0)
        {
            var range = Mediana(ultime.Select(s => s.Alto - s.Basso));
            var volume = Mediana(ultime.Select(s => s.Volume));
            _scalaPuntiMisurata = range > 0 ? range / RangeCassaNq : 1m;
            _scalaLottiMisurata = volume > 0 ? volume / VolumeCassaNq : 1m;
        }

        _scalaDelGiorno = oggi;
    }

    private static decimal Mediana(IEnumerable<decimal> valori)
    {
        var v = valori.OrderBy(x => x).ToList();
        if (v.Count == 0) { return 0m; }
        return v.Count % 2 == 1 ? v[v.Count / 2] : (v[v.Count / 2 - 1] + v[v.Count / 2]) / 2m;
    }

    private object ScalaInJson() => new
    {
        strumento = InstrumentInfo?.Instrument,
        cassa = $"{Apertura}-{Chiusura}",
        punti = Math.Round(Scala, 3),
        lotti = Math.Round(ScalaDeiLotti, 3),
        forzata = ScalaPunti > 0 || ScalaLotti > 0,
        riferimento = new { rangeCassaNq = RangeCassaNq, volumeCassaNq = VolumeCassaNq },
        soglie = new
        {
            staccoFraLivelli = InPunti(StaccoMinimoFraLivelli),
            staccoInvalidazione = InPunti(StaccoInvalidazione),
            raggioInGioco = InPunti(InPlayRadius),
            staccoFraMuri = InPunti(MuriStacco),
            grigliaMuri = InPunti(MuriGrana),
            bigTrade = SogliaBig,
        },
    };
}
