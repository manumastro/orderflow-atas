namespace Quant.Dati;

/// <summary>
/// Lo scarto di ogni rollover della serie continua di ATAS, che passa al contratto principale senza
/// rettificare il prezzo (quant/metodo/i-dati-da-atas.md). Tre metodi, nell'ordine:
///
/// 1. **misurato**: un valore dato da fuori, misurato sui minuti in cui i due contratti stampano
///    insieme (per esempio NQU6 -> NQZ6, +294,5 punti, 11 settembre 2026);
/// 2. **salto**: se fra l'ultima barra del contratto vecchio e la prima del nuovo passano poche ore
///    (la pausa di Globex), il salto fra le due barre e' lo scarto, piu' il movimento di quell'ora;
/// 3. **carry**: se in mezzo c'e' il fine settimana, il salto contiene il movimento del mercato e non
///    serve. Lo scarto si stima come prezzo x tasso x giorni fino al rollover successivo, con il tasso
///    annuo (tasso d'interesse meno dividendi) interpolato nel tempo fra quelli dei rollover con lo
///    scarto noto (1 e 2); fuori dall'intervallo vale il piu' vicino.
/// </summary>
public static class ScartiDiRollover
{
    /// <summary>Un cambio di contratto: quando, da quale a quale, e quando avviene il cambio dopo.</summary>
    public sealed record Cambio(long Tempo, string Da, string A, long TempoSuccessivo);

    private const double MsAnno = 365.0 * 86_400_000;

    public static List<Rollover> Calcola(Barra[] barre, Strumento strumento, IReadOnlyList<Cambio> cambi,
        IReadOnlyDictionary<string, double>? misurati = null, double pausaMassimaOre = 3, double tassoSeNessunoNoto = 0.0374)
    {
        var righe = new List<(Cambio C, double Prezzo, int Salto, double PausaOre)>();
        foreach (var c in cambi.OrderBy(x => x.Tempo))
        {
            var dopo = PrimaDa(barre, c.Tempo);
            if (dopo <= 0 || dopo >= barre.Length) continue;
            righe.Add((c, strumento.InPrezzo(barre[dopo - 1].Chiusura), barre[dopo].Apertura - barre[dopo - 1].Chiusura,
                (barre[dopo].Inizio - barre[dopo - 1].Inizio) / 3_600_000.0));
        }

        // i tassi impliciti dei rollover con lo scarto noto
        var noti = new List<(long Tempo, double Tasso)>();
        var scarti = new Dictionary<Cambio, (int Scarto, string Metodo)>();
        foreach (var (c, prezzo, salto, pausa) in righe)
        {
            (int, string)? s = misurati is not null && misurati.TryGetValue(c.Da, out var punti) ? (strumento.InTick(punti), "misurato")
                : pausa <= pausaMassimaOre ? (salto, "salto") : null;
            if (s is not { } x) continue;
            scarti[c] = x;
            var anni = (c.TempoSuccessivo - c.Tempo) / MsAnno;
            if (anni > 0 && prezzo > 0) noti.Add((c.Tempo, strumento.InPrezzo(x.Item1) / prezzo / anni));
        }

        var esito = new List<Rollover>();
        foreach (var (c, prezzo, _, _) in righe)
        {
            if (!scarti.TryGetValue(c, out var s))
            {
                var tasso = Interpola(noti, c.Tempo) ?? tassoSeNessunoNoto;
                s = (strumento.InTick(prezzo * tasso * (c.TempoSuccessivo - c.Tempo) / MsAnno), "carry");
            }
            esito.Add(new Rollover(c.Tempo, c.Da, c.A, s.Scarto, s.Metodo));
        }
        return esito;
    }

    /// <summary>Il tasso al tempo t: lineare fra i due noti piu' vicini, il piu' vicino fuori dall'intervallo.</summary>
    internal static double? Interpola(List<(long Tempo, double Tasso)> noti, long t)
    {
        if (noti.Count == 0) return null;
        var prima = noti.LastOrDefault(n => n.Tempo <= t);
        var dopo = noti.FirstOrDefault(n => n.Tempo >= t);
        if (prima == default) return dopo.Tasso;
        if (dopo == default || dopo.Tempo == prima.Tempo) return prima.Tasso;
        var w = (double)(t - prima.Tempo) / (dopo.Tempo - prima.Tempo);
        return prima.Tasso + w * (dopo.Tasso - prima.Tasso);
    }

    private static int PrimaDa(Barra[] barre, long t)
    {
        int lo = 0, hi = barre.Length;
        while (lo < hi)
        {
            var m = (lo + hi) / 2;
            if (barre[m].Inizio < t) lo = m + 1; else hi = m;
        }
        return lo;
    }
}
