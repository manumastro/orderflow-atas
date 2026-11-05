using System.Globalization;

namespace Quant.Studi;

/// <summary>
/// La scala di una candela rispetto alla precedente (one time framing, dal video del 5 novembre 2026 sul
/// bias dalla price action): +1 scalino su, -1 scalino giu', 0 pausa. Scalino su = massimo piu' alto,
/// minimo piu' alto E chiusura sopra il massimo della precedente; giu' e' lo stesso rovesciato.
/// Le candele di settimana e mese si fanno dalle giornate di borsa CME (dalle 18:00 ET), raggruppate per
/// la data della giornata: settimana ISO, mese di calendario.
/// </summary>
public static class LaScala
{
    public readonly record struct Candela(int Apertura, int Massimo, int Minimo, int Chiusura);

    public static int Scalino(Candela prima, Candela dopo) =>
        dopo.Massimo > prima.Massimo && dopo.Minimo > prima.Minimo && dopo.Chiusura > prima.Massimo ? 1
        : dopo.Massimo < prima.Massimo && dopo.Minimo < prima.Minimo && dopo.Chiusura < prima.Minimo ? -1
        : 0;

    /// <summary>La chiave di un tempo: g giorno, s settimana ISO, m mese.</summary>
    public static int Chiave(char tempo, DateOnly d) => tempo switch
    {
        'g' => d.DayNumber,
        's' => ISOWeek.GetYear(d.ToDateTime(TimeOnly.MinValue)) * 100 + ISOWeek.GetWeekOfYear(d.ToDateTime(TimeOnly.MinValue)),
        'm' => d.Year * 100 + d.Month,
        _ => throw new ArgumentException($"tempo {tempo}: atteso g, s o m"),
    };

    /// <summary>Le candele CHIUSE di un tempo, dalle giornate chiuse; si esclude quella che contiene `oggi`, ancora aperta.</summary>
    public static List<Candela> Candele(IReadOnlyList<RiassuntoGiorno> giorni, char tempo, DateOnly oggi)
    {
        var aperta = Chiave(tempo, oggi);
        var out_ = new List<Candela>();
        int? chiave = null;
        Candela cur = default;
        foreach (var g in giorni)
        {
            var k = Chiave(tempo, g.Data);
            if (k == aperta) break;
            if (k != chiave)
            {
                if (chiave is not null) out_.Add(cur);
                chiave = k;
                cur = new Candela(g.Apertura, g.Massimo, g.Minimo, g.Chiusura);
            }
            else cur = cur with { Massimo = Math.Max(cur.Massimo, g.Massimo), Minimo = Math.Min(cur.Minimo, g.Minimo), Chiusura = g.Chiusura };
        }
        if (chiave is not null) out_.Add(cur);
        return out_;
    }

    /// <summary>
    /// Lo stato di un tempo. memoria "candela": la scala dell'ultima candela chiusa (0 se pausa).
    /// memoria "scalino": il verso dell'ultimo scalino, che una pausa non cancella. null se le candele non bastano.
    /// La prima candela della serie e' spesso parziale (i dati cominciano a meta'): non si usa.
    /// </summary>
    public static int? Stato(List<Candela> c, bool memoriaScalino)
    {
        if (c.Count < 3) return null;
        if (!memoriaScalino) return Scalino(c[^2], c[^1]);
        for (var i = c.Count - 1; i >= 2; i--)
        {
            var s = Scalino(c[i - 1], c[i]);
            if (s != 0) return s;
        }
        return 0;
    }

    /// <summary>Il verso comune dei tempi chiesti (+1, -1), 0 se non sono tutti nello stesso verso, null se mancano dati.</summary>
    public static int? Allineati(IReadOnlyList<RiassuntoGiorno> giorni, DateOnly oggi, string tempi, bool memoriaScalino)
    {
        int? comune = null;
        foreach (var t in tempi)
        {
            var s = Stato(Candele(giorni, t, oggi), memoriaScalino);
            if (s is null) return null;
            if (s == 0) return 0;
            if (comune is null) comune = s;
            else if (comune != s) return 0;
        }
        return comune ?? 0;
    }
}

/// <summary>
/// La scala dei tre tempi: se mese, settimana e giorno (o i tempi scelti) hanno fatto tutti uno scalino
/// nello stesso verso, si entra in quel verso all'apertura della cassa e si esce alla chiusura. Con i
/// tempi misti non si opera. Dal video del 5 novembre 2026 ("all three stepping up, I'm a buyer only"):
/// scheda in quant/studi/scala-dei-tre-tempi/1-idea.md.
/// </summary>
public sealed class ScalaDeiTreTempi : Strategia
{
    public override string Nome => "scala-dei-tre-tempi";
    public override string? Idea => "quant/studi/scala-dei-tre-tempi/1-idea.md";

    private readonly List<RiassuntoGiorno> _giorni = [];
    private string _tempi = "msg";
    private bool _memoriaScalino;
    private int _entra, _esce;
    private int? _verso;
    private bool _fatto;

    public override string Regole =>
        $"Tempi {_tempi} (m mese, s settimana, g giorno), memoria {(_memoriaScalino ? "scalino (una pausa non cancella l'ultimo scalino)" : "candela (si giudica l'ultima candela chiusa)")}. " +
        "Scalino su = massimo piu' alto, minimo piu' alto e chiusura sopra il massimo precedente; giu' rovesciato. " +
        $"Ingresso: se tutti i tempi sono nello stesso verso, a mercato alle {Calendario.Orario(_entra)} ET in quel verso; con tempi misti niente. " +
        $"Uscita: alle {Calendario.Orario(_esce)} ET. Nessuno stop. Dimensione: dai parametri.";

    public override (int Da, int A) FinestraIngresso => (_entra, _entra + 1);

    public override void Prepara(Parametri p)
    {
        _tempi = p.Testo("tempi", "msg");
        if (_tempi.Length == 0 || _tempi.Any(t => t is not ('m' or 's' or 'g')))
            throw new ArgumentException($"tempi={_tempi}: attese le lettere m, s, g");
        _memoriaScalino = p.Testo("memoria", "candela") switch
        {
            "candela" => false,
            "scalino" => true,
            var x => throw new ArgumentException($"memoria={x}: attesa candela o scalino"),
        };
        _entra = p.Orario("entra", "09:30");
        _esce = p.Orario("esce", "16:00");
    }

    public override void NuovaGiornata(Contesto c)
    {
        _fatto = false;
        // la giornata appena chiusa entra nello storico; solo il passato
        if (c.GiornoPrecedente(1) is { } ieri && (_giorni.Count == 0 || ieri.Data > _giorni[^1].Data)) _giorni.Add(ieri);
        _verso = LaScala.Allineati(_giorni, c.Giorno, _tempi, _memoriaScalino);
    }

    public override void SuBarra(Contesto c)
    {
        if (_fatto || _verso is not (1 or -1) || !c.Piatto || c.HaOrdine) return;
        var m = c.MinutoEt;
        // si decide alla chiusura della barra che finisce all'ora d'ingresso: l'ordine si esegue all'ora esatta
        if (m < _entra - 1 || m >= _entra + 30) return;
        var lato = _verso == 1 ? Lato.Long : Lato.Short;
        c.Invia(new Ordine { Lato = lato, UscitaAlle = c.OggiAlle(_esce), Motivo = lato == Lato.Long ? "tempi allineati su" : "tempi allineati giu'" });
        _fatto = true;
    }
}
