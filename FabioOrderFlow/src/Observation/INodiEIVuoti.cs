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
/// I nodi e i vuoti del profilo di piu' giorni: dove il mercato ha davvero scambiato, e dove ci e'
/// solo passato. Sono i bersagli, sopra e sotto il prezzo.
///
/// <para><b>Perche'.</b> Il 23 settembre il prezzo e' uscito sotto il valore del giorno prima e il
/// chart non aveva niente sotto: le regole guardano oggi, ieri, la notte. Il bersaglio vero stava due
/// giorni indietro — il POC della cassa del 21 e dell'Europa del 22, 30.775-30.783 — e fra il prezzo e
/// quel bersaglio c'era il corridoio lasciato dal pump del 22, fasce da dieci punti con lo 0,1-1% del
/// volume. Il prezzo attraversa i vuoti in fretta e si ferma sui nodi: e' il modo in cui si leggono i
/// bersagli sul profilo, e va fatto su una finestra piu' lunga di una seduta.</para>
///
/// <para><b>Come si trovano, dichiarato.</b> Profilo delle ultime <c>NodiGiorni</c> giornate a fasce
/// di <c>NodiFascia</c> punti (scalati sullo strumento). Un <b>nodo</b> e' una fascia che e' un massimo
/// locale del volume (media su tre fasce) e pesa almeno <c>NodiSoglia</c> volte la fascia mediana; il
/// suo prezzo e' la fascia piu' scambiata del nodo. Un <b>vuoto</b> e' una fila di almeno tre fasce
/// ciascuna sotto <c>VuotoSoglia</c> volte la mediana. Si tengono i piu' vicini al prezzo, sopra e
/// sotto.</para>
/// </summary>
public sealed partial class DataBridge
{
    [Display(Name = "Show nodes and gaps", GroupName = "Nodi",
        Description = "Nodi di volume e vuoti del profilo di piu' giorni, i piu' vicini sopra e sotto il prezzo.")]
    public bool MostraNodi { get; set; } = true;

    [Display(Name = "Days", GroupName = "Nodi")]
    [Range(1, 20)]
    public int NodiGiorni { get; set; } = 5;

    [Display(Name = "Band (NQ points)", GroupName = "Nodi",
        Description = "Larghezza della fascia in punti del NQ; sugli altri strumenti si scala.")]
    [Range(1, 100)]
    public decimal NodiFascia { get; set; } = 5m;

    [Display(Name = "Node: min x median", GroupName = "Nodi")]
    [Range(1, 20)]
    public decimal NodiSoglia { get; set; } = 1.5m;

    [Display(Name = "Gap: max x median", GroupName = "Nodi")]
    [Range(0.01, 1)]
    public decimal VuotoSoglia { get; set; } = 0.25m;

    [Display(Name = "Per side", GroupName = "Nodi")]
    [Range(1, 6)]
    public int NodiPerLato { get; set; } = 3;

    private readonly record struct Nodo(decimal Prezzo, decimal Volume, decimal Peso);

    private readonly record struct Vuoto(decimal Basso, decimal Alto, decimal Peso);

    private Nodo[] _nodi = Array.Empty<Nodo>();
    private Vuoto[] _vuoti = Array.Empty<Vuoto>();
    private int _nodiBarra = -1;
    private decimal _nodiMediana;
    private DateTime _nodiDa;

    private static readonly Color NodoColore = Color.FromArgb(255, 79, 195, 247);
    private static readonly Color VuotoColore = Color.FromArgb(255, 176, 190, 197);

    /// <summary>Ricalcolo a ogni barra chiusa: il profilo di giorni cambia poco da un minuto all'altro.</summary>
    private void AggiornaINodi()
    {
        var ultimo = CurrentBar - 2;
        if (!MostraNodi || ultimo < 1 || ultimo == _nodiBarra)
        {
            return;
        }

        _nodiBarra = ultimo;
        var fascia = InPunti(NodiFascia);
        var inizio = GetCandle(ultimo).Time.AddDays(-NodiGiorni);
        var volume = new SortedDictionary<decimal, decimal>();

        var primo = ultimo;
        for (var bar = ultimo; bar >= 0; bar--)
        {
            var c = GetCandle(bar);
            if (c is null || c.Time < inizio)
            {
                break;
            }

            primo = bar;
            foreach (var l in c.GetAllPriceLevels())
            {
                var k = Math.Floor(l.Price / fascia) * fascia;
                volume[k] = volume.GetValueOrDefault(k) + l.Volume;
            }
        }

        _nodiDa = GetCandle(primo).Time;
        if (volume.Count < 5)
        {
            _nodi = Array.Empty<Nodo>();
            _vuoti = Array.Empty<Vuoto>();
            return;
        }

        // Le fasce vuote in mezzo non esistono nel dizionario: si riempiono a zero, altrimenti un
        // vuoto vero sparisce proprio perche' non ci si e' scambiato niente.
        var prezzi = new List<decimal>();
        for (var p = volume.Keys.First(); p <= volume.Keys.Last(); p += fascia)
        {
            prezzi.Add(p);
        }

        var v = prezzi.Select(p => volume.GetValueOrDefault(p)).ToArray();
        var ordinati = v.Where(x => x > 0).OrderBy(x => x).ToArray();
        var mediana = ordinati.Length > 0 ? ordinati[ordinati.Length / 2] : 0m;
        _nodiMediana = mediana;
        if (mediana <= 0)
        {
            return;
        }

        var liscio = new decimal[v.Length];
        for (var i = 0; i < v.Length; i++)
        {
            var a = Math.Max(0, i - 1);
            var b = Math.Min(v.Length - 1, i + 1);
            decimal s = 0;
            for (var j = a; j <= b; j++) { s += v[j]; }
            liscio[i] = s / (b - a + 1);
        }

        var prezzo = GetCandle(CurrentBar - 1).Close;

        // --- i nodi: massimi locali pesanti; il prezzo e' la fascia piu' scambiata del nodo -----------
        var nodi = new List<Nodo>();
        for (var i = 1; i < v.Length - 1; i++)
        {
            if (liscio[i] >= liscio[i - 1] && liscio[i] > liscio[i + 1] && liscio[i] >= mediana * NodiSoglia)
            {
                var da = Math.Max(0, i - 2);
                var a = Math.Min(v.Length - 1, i + 2);
                var migliore = da;
                for (var j = da; j <= a; j++) { if (v[j] > v[migliore]) { migliore = j; } }
                var centro = prezzi[migliore] + fascia / 2m;
                if (nodi.Count == 0 || Math.Abs(nodi[^1].Prezzo - centro) > fascia * 2)
                {
                    nodi.Add(new Nodo(centro, v[migliore], v[migliore] / mediana));
                }
            }
        }

        _nodi = nodi.Where(n => n.Prezzo > prezzo).OrderBy(n => n.Prezzo).Take(NodiPerLato)
            .Concat(nodi.Where(n => n.Prezzo <= prezzo).OrderByDescending(n => n.Prezzo).Take(NodiPerLato))
            .ToArray();

        // --- i vuoti: file di almeno tre fasce sottili -------------------------------------------
        var vuoti = new List<Vuoto>();
        var inizioVuoto = -1;
        for (var i = 0; i <= v.Length; i++)
        {
            var sottile = i < v.Length && v[i] < mediana * VuotoSoglia;
            if (sottile && inizioVuoto < 0) { inizioVuoto = i; }
            if (!sottile && inizioVuoto >= 0)
            {
                if (i - inizioVuoto >= 3)
                {
                    decimal s = 0;
                    for (var j = inizioVuoto; j < i; j++) { s += v[j]; }
                    vuoti.Add(new Vuoto(prezzi[inizioVuoto], prezzi[i - 1] + fascia, s / (i - inizioVuoto) / mediana));
                }
                inizioVuoto = -1;
            }
        }

        // Solo i vuoti fra il prezzo e l'ultimo nodo tenuto su ciascun lato: sono quelli che si
        // attraversano per arrivare a un bersaglio.
        var tettoSu = _nodi.Where(n => n.Prezzo > prezzo).Select(n => n.Prezzo).DefaultIfEmpty(prezzo).Max();
        var fondoGiu = _nodi.Where(n => n.Prezzo <= prezzo).Select(n => n.Prezzo).DefaultIfEmpty(prezzo).Min();
        _vuoti = vuoti.Where(x => x.Alto > fondoGiu && x.Basso < tettoSu).ToArray();
    }

    /// <summary>Nodi come righe, vuoti come fasce velate: dalla barra di adesso in avanti.</summary>
    private (string Text, Color Color, Point At)? DisegnaINodi(
        RenderContext context, Rectangle area, int lineLeft, int dataRight, Point? mouse)
    {
        if (!MostraNodi || ChartInfo is null || (_nodi.Length == 0 && _vuoti.Length == 0))
        {
            return null;
        }

        var font = new RenderFont("Arial", Math.Max(7, LevelFontSize - 1));
        var sinistra = Math.Clamp(lineLeft, area.Left, dataRight);
        (string Text, Color Color, Point At)? tooltip = null;
        var giorni = $"{NodiGiorni} giorni";

        foreach (var g in _vuoti)
        {
            var yAlto = ChartInfo.GetYByPrice(g.Alto, false);
            var yBasso = ChartInfo.GetYByPrice(g.Basso, false);
            var top = Math.Max(area.Top, Math.Min(yAlto, yBasso));
            var bottom = Math.Min(area.Bottom, Math.Max(yAlto, yBasso));
            if (bottom <= top)
            {
                continue;
            }

            var fascia = new Rectangle(sinistra, top, Math.Max(1, dataRight - sinistra), bottom - top);
            context.FillRectangle(Color.FromArgb(28, VuotoColore), fascia);
            var testo = $"VUOTO {Prezzo(g.Basso)}-{Prezzo(g.Alto)}";
            context.DrawString(testo, font, Color.FromArgb(170, VuotoColore), sinistra + 6, top + 1);
            if (mouse is { } m && fascia.Contains(m))
            {
                tooltip = ($"{testo} · {giorni}, volume per fascia {g.Peso.ToString("0.00", Italiano)}x la mediana "
                           + "· qui il prezzo e' solo passato: se ci rientra, lo attraversa in fretta", VuotoColore, m);
            }
        }

        foreach (var n in _nodi)
        {
            var y = ChartInfo.GetYByPrice(n.Prezzo, false);
            if (y < area.Top || y > area.Bottom)
            {
                continue;
            }

            context.DrawLine(new RenderPen(Color.FromArgb(200, NodoColore), 1, DashOf("dash")),
                sinistra, y, dataRight, y);
            var breve = $"NODO {Prezzo(n.Prezzo)}";
            var size = context.MeasureString(breve, font);
            var x = sinistra + 6;
            var riquadro = new Rectangle(x - 3, y - size.Height - 2, size.Width + 6, size.Height + 2);
            context.FillRectangle(Color.FromArgb(190, 0, 0, 0), riquadro);
            context.DrawString(breve, font, NodoColore, x, y - size.Height - 1);
            if (mouse is { } m)
            {
                var banda = new Rectangle(sinistra, y - 4, Math.Max(1, dataRight - sinistra), 8);
                if (riquadro.Contains(m) || banda.Contains(m))
                {
                    tooltip = ($"{breve} · {giorni}, {Lotti(n.Volume)} lotti nella fascia, "
                               + $"{n.Peso.ToString("0.0", Italiano)}x la mediana · qui il mercato ha costruito valore: "
                               + "e' un bersaglio, e dove il prezzo si ferma", NodoColore, m);
                }
            }
        }

        return tooltip;
    }

    private object Nodi()
    {
        AggiornaINodi();
        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            da = Iso(_nodiDa),
            giorni = NodiGiorni,
            fascia = InPunti(NodiFascia),
            mediana = _nodiMediana,
            nodi = _nodi.OrderByDescending(n => n.Prezzo)
                .Select(n => new { prezzo = n.Prezzo, lotti = n.Volume, peso = Math.Round(n.Peso, 2) }).ToArray(),
            vuoti = _vuoti.OrderByDescending(g => g.Alto)
                .Select(g => new { da = g.Basso, a = g.Alto, peso = Math.Round(g.Peso, 3) }).ToArray(),
        };
    }
}
