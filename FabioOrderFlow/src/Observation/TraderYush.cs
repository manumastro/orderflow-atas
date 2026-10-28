using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Tools;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// I livelli di TraderYush: i prezzi che il trader dell'intervista di Chart Fanatics si segna
/// prima dell'apertura di New York (<c>fabio_course/other_traders/traderyush.txt</c>; documento
/// <c>docs/research/metodo/il-metodo-di-traderyush.md</c>). Le righe citate sono quelle del
/// transcript piegato a 200 caratteri (<c>fold -s -w 200</c>).
///
/// <para><b>Cosa disegna, e solo questo.</b> Una retta orizzontale per livello, con
/// un'etichetta esplicita <c>NOME (SIGLA): PREZZO</c>:</para>
/// <list type="bullet">
/// <item>ieri, solo la cassa: PDH, PDL (righe 21-22), PDC (righe 282-284);</item>
/// <item>il valore di ieri: PDVAH (riga 452), PDVAL (righe 197-198, i bordi del valore come
/// rette);</item>
/// <item>la notte dalle 21:00 di New York alle 9:29: ONH, ONL (righe 22-23);</item>
/// <item>la cassa di oggi: l'apertura RTHO (riga 260) e i primi 30 minuti ORH, ORL (righe 23,
/// 366, 473: "orb", "minimo di apertura range").</item>
/// </list>
///
/// <para><b>Cosa non fa, ed e' deliberato</b> (richiesta del 27 ottobre 2026): niente zone di
/// confluenza, niente conteggio dei criteri, niente big trades, niente picchi di delta, niente
/// migrazione del valore, niente pannello. Sono descritti nel documento come sviluppi possibili.
/// Non dice LONG o SHORT: il trader stesso dice che i livelli <i>"di per se', non significano
/// nulla"</i> (righe 25-26).</para>
///
/// <para><b>Gli orari sono di New York, convertiti per data</b>: la cassa 09:30-16:00 e la notte
/// si scrivono nel fuso di New York (<c>09:30NY</c>) e si convertono in UTC giorno per giorno con
/// <see cref="LeSedute"/>, la classe comune delle sedute, quindi il cambio d'ora (2 novembre 2026)
/// e' gestito.</para>
/// </summary>
[DisplayName("TraderYush")]
public sealed class TraderYush : Indicator
{
    // ------------------------------------------------------------------ le impostazioni: orari

    [Display(Name = "Notte da (ET)", GroupName = "1. Orari New York", Order = 10,
        Description = "Inizio della notte per ONH e ONL, nel fuso di New York (21:00NY). Il transcript dice 21:00 EST (righe 22-23); il globex apre alle 18:00NY.")]
    public string NotteDa
    {
        get => _notteDa;
        set { _notteDa = value; RecalculateValues(); }
    }

    [Display(Name = "Cassa da (ET)", GroupName = "1. Orari New York", Order = 20,
        Description = "RTH 09:30-16:00 EST (riga 28). Sintassi di LeSedute: 09:30NY.")]
    public string CassaDa
    {
        get => _cassaDa;
        set { _cassaDa = value; RecalculateValues(); }
    }

    [Display(Name = "Cassa a (ET)", GroupName = "1. Orari New York", Order = 30)]
    public string CassaA
    {
        get => _cassaA;
        set { _cassaA = value; RecalculateValues(); }
    }

    [Display(Name = "Primi minuti (ORH, ORL)", GroupName = "1. Orari New York", Order = 40,
        Description = "L'opening range: il transcript dice \"orb 302\" (riga 23) e \"orbita bassa, 30\" (riga 473): 30 minuti, trascrizione incerta.")]
    [Range(1, 120)]
    public int OrbMinuti
    {
        get => _orbMinuti;
        set { _orbMinuti = value; RecalculateValues(); }
    }

    // ------------------------------------------------------------------ le impostazioni: famiglie

    [Display(Name = "Ieri: massimo, minimo, chiusura", GroupName = "2. Ieri (PDH, PDL, PDC)", Order = 10)]
    public bool MostraIeri { get; set; } = true;

    [Display(Name = "Colore", GroupName = "2. Ieri (PDH, PDL, PDC)", Order = 20)]
    public Color ColoreIeri { get; set; } = Color.FromArgb(255, 79, 195, 247);

    [Display(Name = "Spessore", GroupName = "2. Ieri (PDH, PDL, PDC)", Order = 30)]
    [Range(1, 5)]
    public int SpessoreIeri { get; set; } = 2;

    [Display(Name = "Valore di ieri: VAH e VAL", GroupName = "3. Valore di ieri (PDVAH, PDVAL)", Order = 10,
        Description = "I bordi dell'area del valore (70%) della cassa di ieri: il VAH di ieri e' un livello alla riga 452.")]
    public bool MostraValoreIeri { get; set; } = true;

    [Display(Name = "Colore", GroupName = "3. Valore di ieri (PDVAH, PDVAL)", Order = 20,
        Description = "Rosa: il colore delle linee del valore nei grafici del trader (righe 197-198).")]
    public Color ColoreValoreIeri { get; set; } = Color.FromArgb(255, 240, 98, 146);

    [Display(Name = "Spessore", GroupName = "3. Valore di ieri (PDVAH, PDVAL)", Order = 30)]
    [Range(1, 5)]
    public int SpessoreValoreIeri { get; set; } = 1;

    [Display(Name = "Valore di oggi: VAH e VAL", GroupName = "3b. Valore di oggi (VAH, VAL)", Order = 10,
        Description = "I bordi del valore della cassa di oggi, che si sposta durante la seduta: \"vedi come si forma il valore e puoi posizionarti\" (righe 126-128, 399-402). Solo RTH (righe 120-121).")]
    public bool MostraValoreOggi { get; set; } = true;

    [Display(Name = "Colore", GroupName = "3b. Valore di oggi (VAH, VAL)", Order = 20,
        Description = "Rosa: le linee del valore nei grafici del trader (righe 84-85, 197).")]
    public Color ColoreValoreOggi { get; set; } = Color.FromArgb(255, 255, 64, 160);

    [Display(Name = "Spessore", GroupName = "3b. Valore di oggi (VAH, VAL)", Order = 30)]
    [Range(1, 5)]
    public int SpessoreValoreOggi { get; set; } = 2;

    [Display(Name = "Notte: massimo e minimo", GroupName = "4. Notte (ONH, ONL)", Order = 10)]
    public bool MostraNotte { get; set; } = true;

    [Display(Name = "Colore", GroupName = "4. Notte (ONH, ONL)", Order = 20,
        Description = "Viola: il minimo della notte e' \"la linea viola tratteggiata\" (riga 199).")]
    public Color ColoreNotte { get; set; } = Color.FromArgb(255, 186, 104, 200);

    [Display(Name = "Spessore", GroupName = "4. Notte (ONH, ONL)", Order = 30)]
    [Range(1, 5)]
    public int SpessoreNotte { get; set; } = 2;

    [Display(Name = "Primi minuti: massimo e minimo", GroupName = "5. Cassa di oggi (RTHO, ORH, ORL)", Order = 10,
        Description = "Si disegnano solo a range chiuso: un range in formazione e' un estremo qualunque.")]
    public bool MostraPrimiMinuti { get; set; } = true;

    [Display(Name = "Apertura della cassa", GroupName = "5. Cassa di oggi (RTHO, ORH, ORL)", Order = 20,
        Description = "Il prezzo di apertura delle 9:30 (riga 260).")]
    public bool MostraApertura { get; set; } = true;

    [Display(Name = "Colore", GroupName = "5. Cassa di oggi (RTHO, ORH, ORL)", Order = 30)]
    public Color ColorePrimiMinuti { get; set; } = Color.FromArgb(255, 255, 183, 77);

    [Display(Name = "Spessore", GroupName = "5. Cassa di oggi (RTHO, ORH, ORL)", Order = 40)]
    [Range(1, 5)]
    public int SpessorePrimiMinuti { get; set; } = 2;

    [Display(Name = "Font", GroupName = "6. Disegno", Order = 10)]
    [Range(6, 16)]
    public int FontSize { get; set; } = 9;

    [Display(Name = "Distanza dalla scala dei prezzi, px", GroupName = "6. Disegno", Order = 20)]
    [Range(0, 200)]
    public int MargineDestro { get; set; } = 10;

    private string _notteDa = "21:00NY";
    private string _cassaDa = LeSedute.AperturaCassa;
    private string _cassaA = LeSedute.ChiusuraCassa;
    private int _orbMinuti = 30;

    public TraderYush()
        : base(true)
    {
        Name = "TraderYush";
        DenyToChangePanel = true;
        EnableCustomDrawing = true;
        SubscribeToDrawingEvents(DrawingLayouts.Final);
        // La serie di default di ATAS non serve: tutto si disegna in OnRender.
        if (DataSeries.Count > 0 && DataSeries[0] is ValueDataSeries serie)
        {
            serie.VisualType = VisualMode.Hide;
        }
    }

    // ------------------------------------------------------------------ cosa si calcola

    private enum Famiglia
    {
        Ieri,
        ValoreIeri,
        ValoreOggi,
        Notte,
        PrimiMinuti,
        Apertura,
    }

    /// <summary>Un livello: il nome per esteso, la sigla, il prezzo, e la barra da cui e' noto (la retta parte da li').</summary>
    private sealed record Livello(Famiglia Famiglia, string Nome, string Sigla, decimal Prezzo, int Barra);

    private volatile Livello[] _livelli = Array.Empty<Livello>();
    private int _ultimaBarra = -1;

    private static readonly CultureInfo Italiano = CultureInfo.GetCultureInfo("it-IT");

    protected override void OnRecalculate()
    {
        _ultimaBarra = -1;
        _livelli = Array.Empty<Livello>();
        base.OnRecalculate();
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        // Come il Data Bridge: si calcola una volta per barra nuova, e solo sulle barre CHIUSE.
        // Senza il filtro, il caricamento storico ricalcolerebbe migliaia di volte.
        if (bar < CurrentBar - 1 || bar <= _ultimaBarra || bar < 2)
        {
            return;
        }

        _ultimaBarra = bar;
        try
        {
            Ricalcola(bar - 1);
        }
        catch (Exception errore)
        {
            this.LogError("TraderYush: livelli non calcolati", errore);
        }
    }

    private void Ricalcola(int chiusa)
    {
        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        var adesso = GetCandle(chiusa).Time;
        var notteOrario = LeSedute.Orario(NotteDa, "21:00NY");
        var apreOrario = LeSedute.Orario(CassaDa, LeSedute.AperturaCassa);
        var chiudeOrario = LeSedute.Orario(CassaA, LeSedute.ChiusuraCassa);

        // IL GIORNO DI SESSIONE: dall'inizio della notte si lavora gia' per la cassa di domani.
        // Ogni orario si converte con le regole dell'ora legale DELLA SUA DATA (LeSedute.Momento).
        var oggi = LeSedute.GiornoDiMercato(adesso).Date;
        var giorno = adesso >= LeSedute.Momento(notteOrario, oggi) ? oggi.AddDays(1) : oggi;
        if (adesso < LeSedute.Momento(notteOrario, giorno.AddDays(-1)))
        {
            giorno = giorno.AddDays(-1);
        }

        DateTime Utc(DateTime data, string orario) => LeSedute.Momento(orario, data);

        var notteDa = Utc(giorno.AddDays(-1), notteOrario);
        var cassaDa = Utc(giorno, apreOrario);
        var orbA = cassaDa.AddMinutes(OrbMinuti);

        // Le barre, dalla piu' vecchia: sei giorni bastano per trovare la cassa precedente anche dopo un ponte.
        var limite = notteDa.AddDays(-6);
        var inizio = chiusa;
        while (inizio > 0 && GetCandle(inizio - 1).Time >= limite)
        {
            inizio--;
        }

        int[] Barre(DateTime da, DateTime a)
        {
            var barre = new List<int>();
            for (var i = inizio; i <= chiusa; i++)
            {
                var t = GetCandle(i).Time;
                if (t >= da && t < a)
                {
                    barre.Add(i);
                }
            }

            return barre.ToArray();
        }

        int BarraDi(DateTime t)
        {
            for (var i = inizio; i <= chiusa; i++)
            {
                if (GetCandle(i).Time >= t)
                {
                    return i;
                }
            }

            return chiusa;
        }

        var livelli = new List<Livello>();

        // IERI, solo la cassa (righe 21-22: "questo e' solo RTH"); la chiusura alle righe 282-284.
        // Le rette partono dall'inizio della notte, quando la cassa di ieri e' chiusa.
        var ieri = Array.Empty<int>();
        for (var d = 1; d <= 6 && ieri.Length == 0; d++)
        {
            ieri = Barre(Utc(giorno.AddDays(-d), apreOrario), Utc(giorno.AddDays(-d), chiudeOrario));
        }

        var barraNotte = BarraDi(notteDa);
        if (ieri.Length > 0)
        {
            livelli.Add(new(Famiglia.Ieri, "Massimo di ieri", "PDH", ieri.Max(i => GetCandle(i).High), barraNotte));
            livelli.Add(new(Famiglia.Ieri, "Minimo di ieri", "PDL", ieri.Min(i => GetCandle(i).Low), barraNotte));
            livelli.Add(new(Famiglia.Ieri, "Chiusura di ieri", "PDC", GetCandle(ieri[^1]).Close, barraNotte));

            // IL VALORE DI IERI: i bordi del 70% del volume della cassa di ieri (riga 452, il VAH di ieri).
            if (ValoreDi(ieri, tick) is { } valore)
            {
                livelli.Add(new(Famiglia.ValoreIeri, "Massimo del valore di ieri", "PDVAH", valore.Vah, barraNotte));
                livelli.Add(new(Famiglia.ValoreIeri, "Minimo del valore di ieri", "PDVAL", valore.Val, barraNotte));
            }
        }

        // LA NOTTE, dalle 21:00 alle 9:29 di New York (righe 22-23). Prima dell'apertura si muove ancora.
        var notteBarre = Barre(notteDa, cassaDa);
        if (notteBarre.Length > 0)
        {
            var hi = notteBarre.MaxBy(i => GetCandle(i).High);
            var lo = notteBarre.MinBy(i => GetCandle(i).Low);
            livelli.Add(new(Famiglia.Notte, "Massimo della notte", "ONH", GetCandle(hi).High, notteBarre[0]));
            livelli.Add(new(Famiglia.Notte, "Minimo della notte", "ONL", GetCandle(lo).Low, notteBarre[0]));
        }

        // L'APERTURA DELLA CASSA (riga 260: "formando valore sopra quel prezzo di apertura").
        var cassaBarre = Barre(cassaDa, Utc(giorno, chiudeOrario));
        if (cassaBarre.Length > 0)
        {
            livelli.Add(new(Famiglia.Apertura, "Apertura della cassa", "RTHO", GetCandle(cassaBarre[0]).Open, cassaBarre[0]));
        }

        // I PRIMI 30 MINUTI (righe 23, 366, 473), solo a range chiuso.
        var orbBarre = Barre(cassaDa, orbA);
        if (adesso >= orbA && orbBarre.Length > 0)
        {
            var b = BarraDi(orbA);
            livelli.Add(new(Famiglia.PrimiMinuti, $"Massimo dei primi {OrbMinuti} minuti", "ORH", orbBarre.Max(i => GetCandle(i).High), b));
            livelli.Add(new(Famiglia.PrimiMinuti, $"Minimo dei primi {OrbMinuti} minuti", "ORL", orbBarre.Min(i => GetCandle(i).Low), b));
        }

        // IL VALORE DI OGGI, in formazione: il trader lo guarda spostarsi durante la seduta (righe 126-128,
        // 399-402) e ci opera ai bordi (modello uno, righe 80-97). Solo la cassa (righe 120-121).
        if (cassaBarre.Length > 0 && ValoreDi(cassaBarre, tick) is { } oggiValore)
        {
            livelli.Add(new(Famiglia.ValoreOggi, "Massimo del valore di oggi", "VAH", oggiValore.Vah, cassaBarre[0]));
            livelli.Add(new(Famiglia.ValoreOggi, "Minimo del valore di oggi", "VAL", oggiValore.Val, cassaBarre[0]));
        }

        _livelli = livelli.ToArray();
        RedrawChart();
    }

    /// <summary>L'area del valore (70% del volume attorno al POC) delle barre, prezzo per prezzo sul tick.</summary>
    private (decimal Val, decimal Vah)? ValoreDi(int[] barre, decimal tick)
    {
        var fasce = new SortedDictionary<decimal, decimal>();
        foreach (var i in barre)
        {
            foreach (var l in GetCandle(i).GetAllPriceLevels())
            {
                fasce[l.Price] = fasce.GetValueOrDefault(l.Price) + l.Volume;
            }
        }

        var totale = fasce.Values.Sum();
        if (totale <= 0)
        {
            return null;
        }

        var chiavi = fasce.Keys.ToArray();
        var poc = Array.IndexOf(chiavi, fasce.MaxBy(x => x.Value).Key);
        int basso = poc, alto = poc;
        var dentro = fasce[chiavi[poc]];
        while (dentro < totale * 0.70m && (basso > 0 || alto < chiavi.Length - 1))
        {
            var su = alto < chiavi.Length - 1 ? fasce[chiavi[alto + 1]] : -1m;
            var giu = basso > 0 ? fasce[chiavi[basso - 1]] : -1m;
            if (su >= giu)
            {
                alto++;
                dentro += su;
            }
            else
            {
                basso--;
                dentro += giu;
            }
        }

        return (Arrotonda(chiavi[basso], tick), Arrotonda(chiavi[alto], tick));
    }

    // ------------------------------------------------------------------ il disegno

    protected override void OnRender(RenderContext context, DrawingLayouts layout)
    {
        if (ChartInfo is null)
        {
            return;
        }

        try
        {
            var area = ChartArea;
            var destra = DataAreaRight(context, area);
            var font = new RenderFont("Arial", FontSize);
            var tick = InstrumentInfo?.TickSize ?? 0.25m;

            // Prima le rette, poi le etichette: le etichette si spostano per non sovrapporsi.
            var etichette = new List<(string Testo, Color Colore, int Y)>();
            foreach (var l in _livelli)
            {
                if (!Acceso(l.Famiglia))
                {
                    continue;
                }

                var y = ChartInfo.GetYByPrice(l.Prezzo, false);
                if (y < area.Top || y > area.Bottom)
                {
                    continue;
                }

                var (colore, spessore) = Stile(l.Famiglia);
                var x = Math.Max(area.Left, ChartInfo.GetXByBar(l.Barra, false));
                if (x >= destra)
                {
                    continue;
                }

                // CA1416: DashStyle vive in System.Drawing.Common; qui e' solo un enum passato a RenderPen di ATAS.
#pragma warning disable CA1416
                var penna = new RenderPen(colore, spessore, l.Famiglia is Famiglia.ValoreIeri or Famiglia.ValoreOggi or Famiglia.Notte ? DashStyle.Dash : DashStyle.Solid);
#pragma warning restore CA1416
                context.DrawLine(penna, x, y, destra, y);
                etichette.Add(($"{l.Nome} ({l.Sigla}): {Prezzo(l.Prezzo, tick)}", colore, y));
            }

            // LE ETICHETTE: a destra, sopra la retta, con un fondo pieno (sopra un footprint denso il
            // testo nudo e' illeggibile); se due livelli sono vicini, quella di sotto scende.
            var alto = context.MeasureString("Hg", font).Height;
            var occupato = int.MinValue;
            foreach (var (testo, colore, y) in etichette.OrderBy(e => e.Y))
            {
                var sz = context.MeasureString(testo, font);
                var yt = Math.Max(y - alto - 1, occupato);
                var xt = Math.Max(area.Left + 4, destra - sz.Width - 8);
                context.FillRectangle(Color.FromArgb(190, 0, 0, 0), new Rectangle(xt - 3, yt - 1, sz.Width + 6, alto + 2));
                context.DrawString(testo, font, colore, xt, yt);
                occupato = yt + alto + 2;
            }
        }
        catch (Exception errore)
        {
            this.LogError("TraderYush: livelli non disegnati", errore);
        }
    }

    private bool Acceso(Famiglia f) => f switch
    {
        Famiglia.Ieri => MostraIeri,
        Famiglia.ValoreIeri => MostraValoreIeri,
        Famiglia.ValoreOggi => MostraValoreOggi,
        Famiglia.Notte => MostraNotte,
        Famiglia.PrimiMinuti => MostraPrimiMinuti,
        Famiglia.Apertura => MostraApertura,
        _ => false,
    };

    private (Color Colore, int Spessore) Stile(Famiglia f) => f switch
    {
        Famiglia.Ieri => (ColoreIeri, SpessoreIeri),
        Famiglia.ValoreIeri => (ColoreValoreIeri, SpessoreValoreIeri),
        Famiglia.ValoreOggi => (ColoreValoreOggi, SpessoreValoreOggi),
        Famiglia.Notte => (ColoreNotte, SpessoreNotte),
        _ => (ColorePrimiMinuti, SpessorePrimiMinuti),
    };

    /// <summary>
    /// Il bordo destro dei dati, prima della scala dei prezzi: <c>ChartArea</c> la include. Come nel
    /// Data Bridge (<c>DataAreaRight</c>), si misura il prezzo piu' largo col font dell'asse.
    /// </summary>
    private int DataAreaRight(RenderContext context, Rectangle area)
    {
        try
        {
            if (ChartInfo is { HidePriceAxis: false } chart)
            {
                var widest = chart.PriceChartContainer?.High ?? 0m;
                var text = widest.ToString(
                    string.IsNullOrWhiteSpace(chart.StringFormat) ? "0.##" : chart.StringFormat,
                    CultureInfo.InvariantCulture);
                var right = area.Right - (context.MeasureString(text, chart.PriceAxisFont).Width + MargineDestro);
                if (right > area.Left)
                {
                    return right;
                }
            }
        }
        catch
        {
            // font o container non pronti: si resta sul bordo dell'area
        }

        return area.Right;
    }

    // ------------------------------------------------------------------ orari e numeri

    private static decimal Arrotonda(decimal p, decimal tick)
        => tick > 0 ? Math.Round(p / tick, MidpointRounding.AwayFromZero) * tick : p;

    /// <summary>Il prezzo sul tick dello strumento, con i decimali del tick: 7.712,50 per NQ (tick 0,25).</summary>
    private static string Prezzo(decimal p, decimal tick)
    {
        var testo = tick.ToString(CultureInfo.InvariantCulture).TrimEnd('0');
        var punto = testo.IndexOf('.');
        var decimali = punto < 0 ? 0 : testo.Length - punto - 1;
        return Arrotonda(p, tick).ToString("N" + Math.Max(2, decimali), Italiano);
    }
}
