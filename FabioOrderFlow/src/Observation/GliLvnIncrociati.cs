using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Tools;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Le zone a basso volume (LVN) di tre profili, il composito di 90 giorni, la settimana e il giorno,
/// messe una accanto all'altra perche' si veda dove coincidono.
///
/// <para><b>Perche'.</b> Il 30 settembre 2026 l'utente ha tracciato a mano una zona a 30.511 sul
/// profilo della settimana, "un po' vuota di volume e gia' rispettata", e il prezzo ci ha fatto il
/// minimo della seduta (30.512,25) prima di salire di 400 punti. Rifatto il conto dopo: 30.511 era il
/// bordo alto del vuoto del composito a 90 giorni (30.070-30.470), dentro il vuoto della settimana
/// (30.355-30.550) e sul fondo di quello della notte (30.510-30.565). Nessuno dei tre profili da solo
/// lo diceva con forza; insieme si'.</para>
///
/// <para><b>Da dove viene.</b> Fabio, <c>fabio_1.txt</c> 46:38-47:08: <i>"if we manage to break this
/// level, we have a gap. And this gap is the first level that the market will aggressively want to
/// rebalance ... because market maker don't like inefficient transaction"</i>. Il composito e' quello
/// del template, <i>"the composite is the last three months, 90 days"</i> (<c>fabio_4.txt</c> 14:18).
/// L'incrocio fra i tre profili e' una richiesta dell'utente, non una regola del live: e' una
/// descrizione, e nessuna soglia qui sotto e' validata.</para>
///
/// <para><b>Come si trova una zona sottile, dichiarato.</b> Su ogni profilo il volume si liscia su
/// qualche fascia; una fascia e' sottile se pesa meno di <c>Soglia</c> volte la fascia piu' pesante
/// entro <c>Ampiezza</c> punti, da una parte o dall'altra. Una fila di fasce sottili lunga almeno
/// <c>Minimo</c> punti e' una zona. Le code del profilo contano: il minimo di un giorno con poco
/// volume sotto e' proprio il tipo di posto che si cerca.</para>
///
/// <para><b>Il composito e il rollover.</b> Il chart continuo di ATAS non e' aggiustato: fino all'11
/// settembre 2026 riporta i prezzi di NQU6, 295-308 punti sotto NQZ6. Senza correzione il vuoto di
/// agosto finisce 300 punti piu' in basso di dove sta. Lo scarto si misura da solo: per ogni giorno
/// presente su entrambi i chart si confronta la chiusura del continuo con quella del contratto, e
/// l'ultimo scarto diverso da zero si applica a tutti i giorni fino a quello (metodo "Panama").</para>
/// </summary>
public sealed partial class DataBridge
{
    [Display(Name = "Show crossed LVN", GroupName = "LVN",
        Description = "Bande sul chart dove almeno due profili su tre (composito 90 giorni, settimana, giorno) sono sottili.")]
    // SPENTO DAL 9 OTTOBRE 2026, richiesta dell'utente: "i lvn per ora li voglio togliere, voglio
    // studiarli meglio dopo". Il calcolo resta, si riaccende qui; lo studio e' in gli-lvn-incrociati.md.
    public bool MostraLvn { get; set; }

    [Display(Name = "Composite days", GroupName = "LVN",
        Description = "Quanti giorni del chart Daily continuo entrano nel composito. Fabio: 90.")]
    [Range(5, 400)]
    public int LvnGiorni { get; set; } = 90;

    /// <summary>
    /// Una scala: il profilo, la sua grana e quanto deve essere sottile una fascia per contare.
    /// Le grandezze sono in punti del NQ e si scalano sullo strumento con <see cref="InPunti"/>.
    /// Un composito di tre mesi ha nodi larghi centinaia di punti, una notte di poche decine: la
    /// stessa soglia sulle due scale troverebbe solo rumore sull'una o niente sull'altra.
    /// </summary>
    private sealed record ScalaLvn(string Nome, decimal Grana, int Liscia, decimal Ampiezza, decimal Soglia,
        decimal Minimo, Color Colore);

    private static readonly ScalaLvn[] LeScale =
    {
        new("90G", 10m, 3, 300m, 0.6m, 30m, Color.FromArgb(255, 206, 147, 216)),
        new("SETT", 5m, 2, 100m, 0.5m, 15m, Color.FromArgb(255, 77, 208, 225)),
        new("OGGI", 5m, 1, 60m, 0.5m, 15m, Color.FromArgb(255, 255, 241, 118)),
    };

    private readonly record struct Sottile(decimal Basso, decimal Alto);

    /// <summary>Un tratto in cui le zone sottili di almeno due scale si sovrappongono.</summary>
    private readonly record struct Incrocio(decimal Basso, decimal Alto, string[] Scale);

    /// <summary>Quanto spazio prendono le strisce sul bordo destro: le scritte dei livelli si spostano.</summary>
    private int MargineLvn => 0;

    private Dictionary<string, Sottile[]> _lvn = new();
    private Incrocio[] _incroci = Array.Empty<Incrocio>();
    private int _lvnBarra = -1;
    private int _lvnPrimaBarra = -1;
    private string _lvnFonte = string.Empty;

    private SortedDictionary<decimal, decimal>? _composito;
    private DateTime _compositoGiorno;
    private string _compositoChi = string.Empty;

    /// <summary>Quale scala si disegna su questo chart: la settimana sull'intraday, il composito sul Daily.</summary>
    private string _lvnSulChart = "SETT";

    private DateTime _lvnQuando = DateTime.MinValue;

    private bool SonoGiornaliero()
        => string.Equals(Convert.ToString(ChartInfo?.TimeFrame), "Daily", StringComparison.OrdinalIgnoreCase);

    private void AggiornaGliLvn()
    {
        // SUL CHART DAILY si disegna il COMPOSITO, sulle barre di quel chart: le stesse del profilo
        // che l'utente ci tiene sopra (1 ottobre 2026, "sul composito voglio i rettangoli del lvn del
        // composito"). Prezzi grezzi del continuo, come quel profilo: non aggiustati al rollover. La
        // versione aggiustata, che vale per il contratto, la calcola l'istanza intraday e va
        // all'agente. Una volta al minuto: la barra di oggi cresce e su un Daily non si chiude.
        if (SonoGiornaliero())
        {
            if (!MostraLvn || (DateTime.UtcNow - _lvnQuando).TotalSeconds < 60)
            {
                return;
            }

            _lvnQuando = DateTime.UtcNow;
            try
            {
                var h = new SortedDictionary<decimal, decimal>();
                var primo = -1;
                for (int bar = CurrentBar - 1, n = 0; bar >= 0 && n < LvnGiorni; bar--, n++)
                {
                    var c = GetCandle(bar);
                    if (c is null)
                    {
                        continue;
                    }

                    primo = bar;
                    foreach (var l in c.GetAllPriceLevels())
                    {
                        h[l.Price] = h.GetValueOrDefault(l.Price) + l.Volume;
                    }
                }

                _lvnPrimaBarra = primo;
                _lvn = new Dictionary<string, Sottile[]> { ["90G"] = LeZoneSottili(h, LeScale[0]) };
                _incroci = Array.Empty<Incrocio>();
                _lvnSulChart = "90G";
            }
            catch (Exception errore)
            {
                this.LogError("LVN del composito non calcolati", errore);
            }

            return;
        }

        var ultimo = CurrentBar - 2; // solo barre chiuse
        if (!MostraLvn || ultimo < 1 || ultimo == _lvnBarra)
        {
            return;
        }

        _lvnBarra = ultimo;
        try
        {
            var adesso = GetCandle(ultimo).Time;
            var giorno = GiornoDiMercato(adesso);
            var lunedi = giorno.AddDays(-(((int)giorno.DayOfWeek + 6) % 7));
            // le 18:00 di New York del giorno prima, convertite per data (22:00Z o 23:00Z): LeSedute
            var inizioSettimana = LeSedute.InizioDelGiorno(lunedi);
            var inizioGiorno = LeSedute.InizioDelGiorno(giorno);
            var fineSettimana = DateTime.MaxValue;

            // LA SETTIMANA VALE DOPO LA PRIMA CASSA CHIUSA. Il lunedi', prima della chiusura di cassa, il profilo
            // della settimana e' lo stesso del giorno: incrociarli contava due volte lo stesso dato
            // (audit del 1 ottobre 2026, osservazione dell'utente). Fino ad allora si usa la
            // settimana prima, intera.
            if (adesso < Momento(Chiusura, lunedi))
            {
                fineSettimana = inizioSettimana;
                inizioSettimana = inizioSettimana.AddDays(-7);
            }

            var settimana = new SortedDictionary<decimal, decimal>();
            var oggi = new SortedDictionary<decimal, decimal>();
            for (var bar = ultimo; bar >= 0; bar--)
            {
                var c = GetCandle(bar);
                if (c is null || c.Time < inizioSettimana)
                {
                    break;
                }

                if (c.Time < fineSettimana)
                {
                    _lvnPrimaBarra = bar;
                }

                foreach (var l in c.GetAllPriceLevels())
                {
                    if (c.Time < fineSettimana)
                    {
                        settimana[l.Price] = settimana.GetValueOrDefault(l.Price) + l.Volume;
                    }

                    if (c.Time >= inizioGiorno)
                    {
                        oggi[l.Price] = oggi.GetValueOrDefault(l.Price) + l.Volume;
                    }
                }
            }

            var composito = IlComposito(giorno);
            _lvn = new Dictionary<string, Sottile[]>
            {
                ["90G"] = LeZoneSottili(composito, LeScale[0]),
                ["SETT"] = LeZoneSottili(settimana, LeScale[1]),
                ["OGGI"] = LeZoneSottili(oggi, LeScale[2]),
            };
            _incroci = GliIncroci(_lvn);
            _lvnFonte = _compositoChi;
        }
        catch (Exception errore)
        {
            this.LogError("LVN non calcolati", errore);
        }
    }

    /// <summary>
    /// Il composito dei giorni chiusi prima di oggi. Si ricalcola una volta al giorno, o quando
    /// compare il chart Daily continuo che prima non c'era.
    /// </summary>
    private SortedDictionary<decimal, decimal> IlComposito(DateTime oggi)
    {
        var giornaliero = IlChartGiornaliero();
        var fonteGiusta = giornaliero is null ? "M1" : giornaliero._id;
        if (_composito is not null && _compositoGiorno == oggi && _compositoChi.StartsWith(fonteGiusta, StringComparison.Ordinal))
        {
            return _composito;
        }

        var h = new SortedDictionary<decimal, decimal>();
        if (giornaliero is null)
        {
            // Senza il chart Daily si usa quello che c'e' sul chart: lo si dice, perche' non sono 90 giorni.
            var primo = DateTime.MaxValue;
            for (var bar = 0; bar < CurrentBar - 1; bar++)
            {
                var c = GetCandle(bar);
                if (c is null || GiornoDiMercato(c.Time) >= oggi)
                {
                    continue;
                }

                primo = primo == DateTime.MaxValue ? GiornoDiMercato(c.Time) : primo;
                foreach (var l in c.GetAllPriceLevels())
                {
                    h[l.Price] = h.GetValueOrDefault(l.Price) + l.Volume;
                }
            }

            _compositoChi = $"M1 — nessun chart Daily continuo aperto: composito dei soli giorni di questo chart"
                            + (primo == DateTime.MaxValue ? string.Empty : $", dal {primo:dd/MM}");
        }
        else
        {
            var (scarto, finoA) = LoScartoDelRollover(giornaliero);
            var barre = new List<IndicatorCandle>();
            for (var bar = giornaliero.CurrentBar - 1; bar >= 0 && barre.Count < LvnGiorni; bar--)
            {
                var c = giornaliero.GetCandle(bar);
                if (c is not null && GiornoDiMercato(c.Time) < oggi)
                {
                    barre.Add(c);
                }
            }

            foreach (var c in barre)
            {
                var sposta = GiornoDiMercato(c.Time) <= finoA ? scarto : 0m;
                foreach (var l in c.GetAllPriceLevels())
                {
                    h[l.Price + sposta] = h.GetValueOrDefault(l.Price + sposta) + l.Volume;
                }
            }

            var dal = barre.Count > 0 ? GiornoDiMercato(barre[^1].Time) : oggi;
            _compositoChi = $"{giornaliero._id} — {giornaliero.InstrumentInfo?.Instrument} Daily, {barre.Count} giorni dal {dal:dd/MM}"
                            + (scarto != 0m
                                ? $", i giorni fino al {finoA:dd/MM} spostati di {Segnato(scarto)} punti (rollover)"
                                : ", nessuno scarto di rollover trovato");
        }

        _composito = h;
        _compositoGiorno = oggi;
        return h;
    }

    /// <summary>
    /// Il chart Daily dello stesso strumento: "NQ" continuo per "NQZ6". La radice si trova togliendo
    /// il codice del mese e l'anno ("Z6"); un chart dello stesso contratto va bene lo stesso.
    /// </summary>
    private DataBridge? IlChartGiornaliero()
    {
        var mia = Radice(InstrumentInfo?.Instrument);
        if (mia.Length == 0)
        {
            return null;
        }

        DataBridge[] tutte;
        lock (HubSync)
        {
            tutte = Instances.Values.ToArray();
        }

        return tutte.FirstOrDefault(i => !ReferenceEquals(i, this)
            && string.Equals(Convert.ToString(i.ChartInfo?.TimeFrame), "Daily", StringComparison.OrdinalIgnoreCase)
            && Radice(i.InstrumentInfo?.Instrument) == mia
            && i.CurrentBar > 1);
    }

    private static string Radice(string? strumento)
        => string.IsNullOrEmpty(strumento) ? string.Empty : Regex.Replace(strumento, "[FGHJKMNQUVXZ][0-9]{1,2}$", string.Empty);

    /// <summary>
    /// Lo scarto fra il contratto di questo chart e il continuo, e fino a quale giorno vale. Per ogni
    /// giorno presente su tutti e due si confrontano le chiusure; l'ultimo giorno con uno scarto
    /// diverso da zero segna il rollover.
    /// </summary>
    private (decimal Scarto, DateTime FinoA) LoScartoDelRollover(DataBridge giornaliero)
    {
        var chiusure = new Dictionary<DateTime, decimal>();
        for (var bar = 0; bar < CurrentBar - 1; bar++)
        {
            var c = GetCandle(bar);
            if (c is not null)
            {
                chiusure[GiornoDiMercato(c.Time)] = c.Close; // l'ultima barra del giorno vince
            }
        }

        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        for (var bar = giornaliero.CurrentBar - 2; bar >= 0; bar--)
        {
            var c = giornaliero.GetCandle(bar);
            if (c is null)
            {
                continue;
            }

            var g = GiornoDiMercato(c.Time);
            if (chiusure.TryGetValue(g, out var mia) && Math.Abs(mia - c.Close) > 2 * tick)
            {
                return (mia - c.Close, g);
            }
        }

        return (0m, DateTime.MinValue);
    }

    /// <summary>Le zone sottili di un profilo, alla grana e con le soglie della sua scala.</summary>
    private Sottile[] LeZoneSottili(SortedDictionary<decimal, decimal> profilo, ScalaLvn s)
    {
        if (profilo.Count < 3)
        {
            return Array.Empty<Sottile>();
        }

        var grana = InPunti(s.Grana);
        var fasce = new SortedDictionary<decimal, decimal>();
        foreach (var (prezzo, volume) in profilo)
        {
            var k = Math.Floor(prezzo / grana) * grana;
            fasce[k] = fasce.GetValueOrDefault(k) + volume;
        }

        // Le fasce senza scambi non esistono nel dizionario: si riempiono a zero.
        var prezzi = new List<decimal>();
        for (var p = fasce.Keys.First(); p <= fasce.Keys.Last(); p += grana)
        {
            prezzi.Add(p);
        }

        var v = prezzi.Select(p => fasce.GetValueOrDefault(p)).ToArray();
        var n = v.Length;
        var liscio = new decimal[n];
        for (var i = 0; i < n; i++)
        {
            int a = Math.Max(0, i - s.Liscia), b = Math.Min(n - 1, i + s.Liscia);
            decimal somma = 0;
            for (var j = a; j <= b; j++) { somma += v[j]; }
            liscio[i] = somma / (b - a + 1);
        }

        var raggio = (int)(InPunti(s.Ampiezza) / grana);
        var lunghezza = (int)Math.Ceiling(InPunti(s.Minimo) / grana);
        var zone = new List<Sottile>();
        var inizio = -1;
        for (var i = 0; i <= n; i++)
        {
            var sottile = false;
            if (i < n)
            {
                decimal massimo = 0;
                for (var j = Math.Max(0, i - raggio); j <= Math.Min(n - 1, i + raggio); j++) { massimo = Math.Max(massimo, liscio[j]); }
                sottile = massimo > 0 && liscio[i] < s.Soglia * massimo;
            }

            if (sottile && inizio < 0) { inizio = i; }
            if (!sottile && inizio >= 0)
            {
                if (i - inizio >= lunghezza)
                {
                    zone.Add(new Sottile(prezzi[inizio], prezzi[i - 1] + grana));
                }
                inizio = -1;
            }
        }

        return zone.ToArray();
    }

    /// <summary>I tratti di prezzo coperti dalle zone sottili di almeno due scale.</summary>
    private static Incrocio[] GliIncroci(Dictionary<string, Sottile[]> lvn)
    {
        var bordi = lvn.Values.SelectMany(z => z).SelectMany(z => new[] { z.Basso, z.Alto }).Distinct().OrderBy(x => x).ToList();
        var tratti = new List<Incrocio>();
        for (var i = 0; i < bordi.Count - 1; i++)
        {
            decimal a = bordi[i], b = bordi[i + 1];
            var chi = lvn.Where(kv => kv.Value.Any(z => z.Basso <= a && z.Alto >= b)).Select(kv => kv.Key).ToArray();
            if (chi.Length < 2)
            {
                continue;
            }

            // Due tratti attaccati con le stesse scale sono un tratto solo.
            if (tratti.Count > 0 && tratti[^1].Alto == a && tratti[^1].Scale.SequenceEqual(chi))
            {
                tratti[^1] = tratti[^1] with { Alto = b };
            }
            else
            {
                tratti.Add(new Incrocio(a, b, chi));
            }
        }

        return tratti.ToArray();
    }

    /// <summary>
    /// Sul chart solo le zone sottili della SETTIMANA, come rettangoli dall'inizio della settimana al
    /// bordo destro: VERDI sotto il prezzo, VIOLA sopra, con i colori esatti dei rettangoli che
    /// l'utente disegna a mano (letti da /disegni il 1 ottobre 2026). Le altre due scale e
    /// l'incrocio restano calcolati e arrivano all'agente nel pannello.
    /// </summary>
    private void DisegnaGliLvn(RenderContext context, Rectangle area, int dataRight)
    {
        AggiornaGliLvn();
        if (!MostraLvn || ChartInfo is null || !_lvn.TryGetValue(_lvnSulChart, out var settimana))
        {
            return;
        }

        var sinistra = area.Left;
        try
        {
            if (_lvnPrimaBarra >= 0)
            {
                sinistra = Math.Clamp(ChartInfo.GetXByBar(_lvnPrimaBarra, false), area.Left, dataRight);
            }
        }
        catch (Exception)
        {
            // Senza la X della prima barra il rettangolo parte dal bordo sinistro.
        }

        var prezzo = GetCandle(CurrentBar - 1).Close;
        var font = new RenderFont("Arial", Math.Max(7, LevelFontSize - 1));
        foreach (var z in settimana)
        {
            var (top, bottom) = Verticale(area, z.Basso, z.Alto);
            if (bottom <= top || dataRight <= sinistra)
            {
                continue;
            }

            // Sotto il prezzo e' un appoggio, sopra un tetto. Se il prezzo ci e' dentro, conta la meta'.
            var sotto = z.Alto < prezzo || (z.Basso <= prezzo && prezzo >= (z.Basso + z.Alto) / 2);
            var (bordo, fondo) = sotto ? (LvnVerdeBordo, LvnVerdeFondo) : (LvnViolaBordo, LvnViolaFondo);
            var r = new Rectangle(sinistra, top, dataRight - sinistra, bottom - top);
            context.FillRectangle(fondo, r);
            context.DrawRectangle(new RenderPen(bordo, 2), r);
            var testo = _lvnSulChart == "90G" ? "LVN 90G" : "LVN SETT";
            var sz = context.MeasureString(testo, font);
            if (bottom - top >= sz.Height + 2)
            {
                context.DrawString(testo, font, bordo, dataRight - sz.Width - 6, top + 2);
            }
        }
    }

    private static readonly Color LvnVerdeBordo = Color.FromArgb(255, 0, 128, 0);
    private static readonly Color LvnVerdeFondo = Color.FromArgb(40, 0, 128, 0);
    private static readonly Color LvnViolaBordo = Color.FromArgb(255, 83, 55, 117);
    private static readonly Color LvnViolaFondo = Color.FromArgb(40, 106, 78, 140);

    private static readonly Color LvnColore = Color.FromArgb(255, 240, 98, 146);

    /// <summary>
    /// I tratti incrociati attaccati diventano una zona sola, che prende il numero piu' alto di
    /// profili d'accordo: 30.960-30.970 x2 e 30.970-31.015 x3 sono la stessa zona.
    /// </summary>
    private List<Incrocio> LeZoneLvn()
    {
        var zone = new List<Incrocio>();
        foreach (var t in _incroci.OrderBy(i => i.Basso))
        {
            if (zone.Count > 0 && t.Basso <= zone[^1].Alto)
            {
                var p = zone[^1];
                zone[^1] = new Incrocio(p.Basso, Math.Max(p.Alto, t.Alto), p.Scale.Union(t.Scale).ToArray());
            }
            else
            {
                zone.Add(t);
            }
        }

        return zone;
    }

    private (int Top, int Bottom) Verticale(Rectangle area, decimal basso, decimal alto)
    {
        var yA = ChartInfo!.GetYByPrice(alto, false);
        var yB = ChartInfo!.GetYByPrice(basso, false);
        return (Math.Max(area.Top, Math.Min(yA, yB)), Math.Min(area.Bottom, Math.Max(yA, yB)));
    }

    /// <summary>Le righe per l'agente: i tratti incrociati piu' vicini sopra e sotto, e la fonte del composito.</summary>
    private List<RigaPannello> RigheDegliLvn(decimal prezzo)
    {
        var righe = new List<RigaPannello>();
        if (!MostraLvn || _lvn.Count == 0)
        {
            return righe;
        }

        righe.Add(new RigaPannello("LVN INCROCIATI (zone sottili di almeno due profili: 90G, SETT, OGGI)", PanelGrigio));
        var unite = LeZoneLvn();
        var sopra = unite.Where(i => i.Basso > prezzo).OrderBy(i => i.Basso).Take(3);
        var dentro = unite.Where(i => i.Basso <= prezzo && prezzo <= i.Alto);
        var sotto = unite.Where(i => i.Alto < prezzo).OrderByDescending(i => i.Alto).Take(3);
        foreach (var (lista, dove) in new[] { (sopra, "sopra "), (dentro, "DENTRO"), (sotto, "sotto ") })
        {
            foreach (var i in lista)
            {
                righe.Add(new RigaPannello(
                    $"  {dove}  {Prezzo(i.Basso)}-{Prezzo(i.Alto)}  x{i.Scale.Length} ({string.Join("+", i.Scale)})",
                    i.Scale.Length >= 3 ? PanelAmbra : PanelGrigio));
            }
        }

        foreach (var s in LeScale)
        {
            var zone = _lvn.GetValueOrDefault(s.Nome) ?? Array.Empty<Sottile>();
            var vicine = zone.OrderBy(z => Math.Min(Math.Abs(z.Basso - prezzo), Math.Abs(z.Alto - prezzo))).Take(4)
                .OrderBy(z => z.Basso).Select(z => $"{Prezzo(z.Basso)}-{Prezzo(z.Alto)}");
            righe.Add(new RigaPannello($"  {s.Nome,-4}  {string.Join("  ", vicine)}", PanelGrigio));
        }

        righe.Add(new RigaPannello($"  composito: {_lvnFonte[(_lvnFonte.IndexOf('—') + 1)..].Trim()}", PanelGrigio));
        return righe;
    }
}
