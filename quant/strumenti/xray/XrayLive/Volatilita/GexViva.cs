using System.Globalization;
using XrayLive.Server;

namespace XrayLive.Volatilita;

/// <summary>
/// La GEX viva di NQ ed ES dalla catena CME che ATAS riceve (2 novembre 2026, richiesta dell'utente: "riguardo
/// NQ si'" alla GEX ricalcolata ogni minuto, come QuantData).
///
/// IL MODELLO e' quello di gex.py (quant/metodo/le-opzioni-in-atas.md, "La GEX Calcolata Da Noi"), riscritto qui
/// per tenerlo in memoria:
/// - la volatilita' di ogni strike da Black-76 sull'opzione fuori dal denaro (la call sopra il future, la put
///   sotto), dal medio bid/ask, o dal settlement se il book manca; vale per call e put;
/// - GEX di uno strike = gamma x open interest x moltiplicatore (20 $ NQ, 50 $ ES) x F^2 x 1%, in milioni di $;
///   convenzione A: call - put;
/// - zero gamma sticky strike su +-8% a passi dello 0,1%; call wall e put wall gli strike con la GEX di call e
///   di put piu' grande.
///
/// IL RITMO, perche' l'open interest e' di ieri e cambia una volta al giorno:
/// - **lo 0DTE** (la scadenza di oggi) si rilegge **ogni minuto**: le sue quotazioni cambiano di continuo;
/// - **le altre scadenze** (fino a 70 giorni) a rotazione, quella letta da piu' tempo a ogni minuto: open
///   interest e volatilita' di ogni strike restano in memoria. Una lettura costa 7-35 secondi, e ES ha circa
///   30 scadenze: lette tutte insieme ogni ora (la prima versione) fermavano lo 0DTE per dieci minuti;
/// - **la GEX** si ricalcola ogni minuto al prezzo corrente del future.
/// Al primo giro (o al cambio di giorno) si leggono tutte, una volta.
///
/// IL TETTO: 120 strike per lato, come gex.py (480 sottoscrizioni, sulle 500 del chart). Lo 0DTE e la scadenza a
/// rotazione si richiudono a vicenda, quindi ogni giro le riapre: 10-40 secondi. Provato il 2 novembre 2026 con 60
/// strike per lato, per tenerle aperte insieme: i giri scendevano a 10 secondi, ma lo zero gamma di NQ passava da
/// 29.044 a 29.906 e il put wall da 28.000 a 28.500, perche' si perdevano le put lontane. La precisione vale piu'
/// di 30 secondi.
/// </summary>
internal sealed class GexViva
{
    public static readonly Dictionary<string, double> Moltiplicatore = new() { ["NQ"] = 20, ["ES"] = 50 };
    public const int GiorniMassimi = 70;

    /// <summary>Un'opzione in memoria: la volatilita' e l'open interest dell'ultima lettura della sua scadenza.</summary>
    public sealed record Riga(string Scadenza, DateTime ScadeUtc, double Strike, double OiCall, double OiPut, double Vol);

    public sealed record Profilo(double Totale, double? Zero, double? CallWall, double? PutWall,
        List<(double strike, double call, double put)> Strike);

    public sealed class Stato
    {
        public required string Radice { get; init; }
        public string? Strumento;
        public DateTime? CatenaIntera;             // la lettura piu' vecchia fra le scadenze in memoria
        public Dictionary<string, DateTime> Letta = new();             // quando si e' letta ogni scadenza
        public DateTime? Zero;                     // l'ultima lettura dello 0DTE
        public string? GiornoZero;                 // la scadenza letta come 0DTE
        public double? Future;
        public DateTime? Calcolo;
        public Dictionary<string, List<Riga>> Righe = new();          // per scadenza
        public Dictionary<string, (double giorni, double vol)> Atm = new();
        public Profilo? Tutte;
        public Profilo? Oggi;
        /// <summary>Durante la lettura della catena intera: quante scadenze sono lette (per la pagina Fonti).</summary>
        public string? Avanzamento;
    }

    private readonly CatenaAtas _catena;
    public readonly Dictionary<string, Stato> Stati = Raccolta.Radici.ToDictionary(r => r, r => new Stato { Radice = r });

    public GexViva(CatenaAtas catena) => _catena = catena;

    /// <summary>Un giro per radice: la catena intera se serve, lo 0DTE sempre, poi il calcolo.</summary>
    public Stato Giro(string radice, DateTime adesso)
    {
        var s = Stati[radice];
        var charts = _catena.Charts();
        var chart = charts.FirstOrDefault(c => Contratto(c, radice) && c.TimeFrame == "M30")
                    ?? charts.FirstOrDefault(c => Contratto(c, radice))
                    ?? throw new InvalidOperationException($"nessun chart di {radice} sul bridge");
        s.Strumento = chart.Strumento;
        var serie = _catena.SerieDi(chart.Id).Where(x => x.ScadeUtc > adesso && (x.ScadeUtc - adesso).TotalDays <= GiorniMassimi).ToList();
        if (serie.Count == 0) throw new InvalidOperationException($"nessuna scadenza di {radice} entro {GiorniMassimi} giorni");
        string oggiNY = Raccolta.GiornoNY(adesso);
        var zero = serie[0];

        // al primo giro, o se il giorno e' cambiato, tutte; poi lo 0DTE e le due lette da piu' tempo
        bool intera = s.CatenaIntera is null || Raccolta.GiornoNY(s.CatenaIntera.Value) != oggiNY;
        var daLeggere = intera
            ? serie
            : new[] { zero }.Concat(serie.Skip(1).OrderBy(x => s.Letta.GetValueOrDefault(x.Giorno)).Take(1)).ToList();
        int lette = 0;
        foreach (var x in daLeggere)
        {
            if (intera) s.Avanzamento = $"primo giro: {lette} scadenze lette su {daLeggere.Count}";
            lette++;
            const int strike = 120;
            var sc = _catena.Leggi(chart.Id, x, strike, giri: intera ? 6 : 3);
            if (sc.Sottostante is { } f && f > 0) s.Future = f;
            var righe = Righe(sc, adesso, s.Future);
            if (righe.Count > 0) s.Righe[x.Giorno] = righe;
            s.Letta[x.Giorno] = adesso;
            if (s.Future is { } fw && Atm(righe, fw) is { } a) s.Atm[x.Giorno] = ((x.ScadeUtc - adesso).TotalDays, a);
        }
        // via le scadenze passate
        foreach (var k in s.Righe.Keys.Where(k => s.Righe[k].Count == 0 || s.Righe[k][0].ScadeUtc <= adesso).ToList())
        {
            s.Righe.Remove(k);
            s.Atm.Remove(k);
            s.Letta.Remove(k);
        }
        s.Avanzamento = null;
        s.CatenaIntera = s.Letta.Count > 0 ? s.Letta.Values.Min() : adesso;
        s.Zero = adesso;
        s.GiornoZero = zero.Giorno;
        if (s.Future is not { } fut) throw new InvalidOperationException("nessun prezzo del future");
        double molt = Moltiplicatore[radice];
        var tutte = s.Righe.Values.SelectMany(r => r).ToList();
        s.Tutte = Calcola(tutte, fut, adesso, molt);
        s.Oggi = zero.Giorno == oggiNY && s.Righe.TryGetValue(zero.Giorno, out var r0) ? Calcola(r0, fut, adesso, molt) : null;
        s.Calcolo = adesso;
        return s;
    }

    private static bool Contratto(CatenaAtas.Chart c, string radice) =>
        c.Strumento.StartsWith(radice, StringComparison.Ordinal) && c.Strumento.Length > radice.Length;

    /// <summary>Le righe di una scadenza: per strike l'open interest di call e put e la volatilita' dell'opzione fuori dal denaro.</summary>
    public static List<Riga> Righe(Scadenza sc, DateTime adesso, double? future)
    {
        var out_ = new List<Riga>();
        double t = (sc.ScadeUtc - adesso).TotalDays / 365;
        double? f = sc.Sottostante ?? future;
        if (t <= 0 || f is not { } fw || fw <= 0) return out_;
        double sconto = Math.Exp(Formula.Tasso * t);
        foreach (var g in sc.Opzioni.GroupBy(q => q.Strike))
        {
            var call = g.FirstOrDefault(q => q.Call);
            var put = g.FirstOrDefault(q => !q.Call);
            double oiC = call?.OpenInterest ?? 0, oiP = put?.OpenInterest ?? 0;
            if (oiC <= 0 && oiP <= 0) continue;
            var fuori = g.Key >= fw ? call : put;
            double? prezzo = fuori is null ? null : fuori.Medio ?? (fuori.Settlement is > 0 ? fuori.Settlement : null);
            if (prezzo is not { } p) continue;
            if (Formula.Implicita(p * sconto, fw, g.Key, t, g.Key >= fw) is not { } v) continue;
            out_.Add(new Riga(sc.Giorno, sc.ScadeUtc, g.Key, oiC, oiP, v));
        }
        return out_;
    }

    /// <summary>La volatilita' al prezzo di una scadenza: interpolata fra i due strike attorno al future.</summary>
    public static double? Atm(List<Riga> righe, double f)
    {
        var sotto = righe.Where(r => r.Strike <= f).MaxBy(r => r.Strike);
        var sopra = righe.Where(r => r.Strike > f).MinBy(r => r.Strike);
        if (sotto is null || sopra is null) return (sotto ?? sopra)?.Vol;
        double w = (f - sotto.Strike) / (sopra.Strike - sotto.Strike);
        return sotto.Vol + w * (sopra.Vol - sotto.Vol);
    }

    /// <summary>La GEX (convenzione A) al prezzo f, con le volatilita' ferme (sticky strike), come gex.py.</summary>
    public static Profilo Calcola(IReadOnlyList<Riga> righe, double f, DateTime adesso, double molt)
    {
        double unita(double s) => molt * s * s * 0.01 / 1e6;
        var perK = new SortedDictionary<double, (double c, double p)>();
        foreach (var r in righe)
        {
            double t = (r.ScadeUtc - adesso).TotalDays / 365;
            if (t <= 0) continue;
            double g = Formula.Gamma(f, r.Strike, t, r.Vol) * unita(f);
            var x = perK.GetValueOrDefault(r.Strike);
            perK[r.Strike] = (x.c + g * r.OiCall, x.p + g * r.OiPut);
        }
        double totaleA(double s) => righe.Sum(r =>
        {
            double t = (r.ScadeUtc - adesso).TotalDays / 365;
            return t <= 0 ? 0 : Formula.Gamma(s, r.Strike, t, r.Vol) * unita(s) * (r.OiCall - r.OiPut);
        });
        double passo = f > 10000 ? 5 : 1;
        var griglia = Enumerable.Range(-80, 161).Select(i => Math.Round(f * (1 + i / 1000.0) / passo) * passo).Distinct().OrderBy(x => x)
            .Select(s => (s, g: totaleA(s))).ToList();
        var zeri = new List<double>();
        for (int i = 1; i < griglia.Count; i++)
        {
            var (s1, g1) = griglia[i - 1];
            var (s2, g2) = griglia[i];
            if (g1 == 0 || (g1 < 0) != (g2 < 0)) zeri.Add(g2 != g1 ? s1 + (s2 - s1) * (0 - g1) / (g2 - g1) : s1);
        }
        double? zero = zeri.Count > 0 ? Math.Round(zeri.MinBy(z => Math.Abs(z - f)), 2) : null;
        double? cw = perK.Count > 0 ? perK.MaxBy(kv => kv.Value.c).Key : null;
        double? pw = perK.Count > 0 ? perK.MaxBy(kv => kv.Value.p).Key : null;
        var strike = perK.Where(kv => Math.Abs(kv.Key / f - 1) <= 0.06)
            .Select(kv => (kv.Key, Math.Round(kv.Value.c, 2), Math.Round(-kv.Value.p, 2))).ToList();
        return new Profilo(Math.Round(perK.Values.Sum(x => x.c - x.p), 1), zero, cw, pw, strike);
    }

    /// <summary>La riga di un minuto per l'archivio (gex-NQ.jsonl): i numeri che servono a vederli nel tempo.</summary>
    public static object RigaArchivio(Stato s) => new
    {
        ts = new DateTimeOffset(s.Calcolo!.Value).ToUnixTimeMilliseconds(),
        future = s.Future,
        totale = s.Tutte?.Totale, zero = s.Tutte?.Zero, callWall = s.Tutte?.CallWall, putWall = s.Tutte?.PutWall,
        totale0 = s.Oggi?.Totale, zero0 = s.Oggi?.Zero, callWall0 = s.Oggi?.CallWall, putWall0 = s.Oggi?.PutWall,
    };

    public static string Descrivi(Stato s) => string.Create(CultureInfo.InvariantCulture,
        $"{s.Strumento} {s.Righe.Count} scadenze, future {s.Future}, GEX {s.Tutte?.Totale}, zero {s.Tutte?.Zero}");
}
