using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using XrayLive.Server;

namespace XrayLive.Volatilita;

/// <summary>
/// La catena pubblica del Cboe per QQQ e NDX (2 novembre 2026, su richiesta dell'utente: "inseriamo anche
/// cio' che possiamo riguardo NQ e QQQ"). E' una fonte fuori da ATAS, gratuita e ritardata di 15 minuti:
/// https://cdn.cboe.com/api/global/delayed_quotes/options/QQQ.json (e _NDX). Ogni opzione ha bid, ask,
/// volatilita' implicita, greche e open interest. E' contesto, come la macro: mai in un backtest.
///
/// Da qui si calcola, con il nostro modello e non con le greche arrotondate del file (la gamma di NDX ha 4
/// decimali, e allo strike vale 0,0002): la GEX per strike (convenzione A, come gex.py), lo zero gamma, i
/// muri, la volatilita' a 30 giorni con la formula del Cboe, la struttura a termine e il sorriso.
/// </summary>
internal static partial class CboePubblico
{
    public const string Url = "https://cdn.cboe.com/api/global/delayed_quotes/options/{0}.json";
    public const double Moltiplicatore = 100;
    public const int GiorniGex = 70;

    [GeneratedRegex(@"^([A-Z]+)(\d{6})([CP])(\d{8})$")]
    private static partial Regex Simbolo();

    public sealed record Catena(string Simbolo, DateTime Quando, double Prezzo, DateTime PrezzoQuando, double? Iv30,
        List<Scadenza> Scadenze);

    public static Catena Leggi(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var r = doc.RootElement;
        var d = r.GetProperty("data");
        // "timestamp" e' in UTC (il 2/11/2026 alle 10:51 UTC il file di NDX diceva 10:50:24)
        var quando = DateTime.SpecifyKind(DateTime.ParseExact(r.GetProperty("timestamp").GetString()!, "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture), DateTimeKind.Utc);
        // last_trade_time e' in ora di New York ("2026-10-30T15:59:59")
        var ultimo = d.TryGetProperty("last_trade_time", out var lt) && lt.ValueKind == JsonValueKind.String
            ? TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(DateTime.Parse(lt.GetString()!, CultureInfo.InvariantCulture),
                DateTimeKind.Unspecified), Cassa.NewYork)
            : quando;
        string simbolo = d.GetProperty("symbol").GetString()!.TrimStart('^');
        var perScadenza = new Dictionary<string, (DateTime scade, List<Quotazione> q)>();
        foreach (var o in d.GetProperty("options").EnumerateArray())
        {
            var m = Simbolo().Match(o.GetProperty("option").GetString() ?? "");
            if (!m.Success) continue;
            string radice = m.Groups[1].Value;
            var giorno = DateTime.ParseExact(m.Groups[2].Value, "yyMMdd", CultureInfo.InvariantCulture);
            // NDX (mensili) si regolano all'apertura; NDXP, QQQ e il resto alle 16:00 di New York
            var scade = TimeZoneInfo.ConvertTimeToUtc(giorno.AddHours(radice == "NDX" ? 9.5 : 16), Cassa.NewYork);
            string chiave = giorno.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + (radice == "NDX" ? "am" : "");
            if (!perScadenza.TryGetValue(chiave, out var s)) perScadenza[chiave] = s = (scade, new List<Quotazione>());
            s.q.Add(new Quotazione(double.Parse(m.Groups[4].Value, CultureInfo.InvariantCulture) / 1000, m.Groups[3].Value == "C",
                // fuori dalla cassa il Cboe azzera bid e ask delle opzioni su azioni (QQQ): resta il prezzo
                // teorico del suo modello, che si usa al posto del settlement (2 novembre 2026)
                Num(o, "bid"), Num(o, "ask"), Num(o, "theo"), Num(o, "open_interest") ?? 0, Num(o, "iv") is > 0 and var iv ? iv : null));
        }
        var scadenze = perScadenza.Select(kv => new Scadenza(kv.Key[..10], kv.Value.scade, kv.Value.q))
            .OrderBy(s => s.ScadeUtc).ToList();
        return new Catena(simbolo, quando, d.GetProperty("current_price").GetDouble(), ultimo,
            Num(d, "iv30"), scadenze);
    }

    private static double? Num(JsonElement e, string nome) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : null;

    // ---------------------------------------------------------------- la GEX

    public sealed record ProfiloGex(double Totale, double? ZeroGamma, double? CallWall, double? PutWall,
        List<(double strike, double call, double put)> Strike);

    /// <summary>
    /// La GEX per strike, convenzione A (dealer lunghi call, corti put: call - put), in milioni di $ per 1%:
    /// gamma x open interest x 100 x S^2 x 1%. La gamma e' di Black-Scholes con la volatilita' del Cboe e lo
    /// spot al posto del forward (fino a 70 giorni la differenza e' sotto l'1% della gamma). Lo zero gamma
    /// come in gex.py: il prezzo dove la GEX totale cambia segno, con le volatilita' ferme (sticky strike),
    /// su +-8% a passi dello 0,1%.
    /// </summary>
    public static ProfiloGex Gex(IEnumerable<Scadenza> scadenze, double spot, DateTime adesso, double banda = 0.10)
    {
        var righe = new List<(double k, double t, double v, double segnoOi, bool call, double oi)>();
        foreach (var s in scadenze)
        {
            double t = (s.ScadeUtc - adesso).TotalDays / 365;
            if (t <= 0) continue;
            foreach (var q in s.Opzioni)
                if (q.OpenInterest > 0 && q.Iv is > 0)
                    righe.Add((q.Strike, t, q.Iv.Value, q.Call ? q.OpenInterest : -q.OpenInterest, q.Call, q.OpenInterest));
        }
        double unita(double s) => Moltiplicatore * s * s * 0.01 / 1e6;
        var perK = new SortedDictionary<double, (double c, double p)>();
        foreach (var r in righe)
        {
            double g = Formula.Gamma(spot, r.k, r.t, r.v) * unita(spot) * r.oi;
            var x = perK.GetValueOrDefault(r.k);
            perK[r.k] = r.call ? (x.c + g, x.p) : (x.c, x.p + g);
        }
        double totaleA(double s) => righe.Sum(r => Formula.Gamma(s, r.k, r.t, r.v) * unita(s) * r.segnoOi);
        var valori = Enumerable.Range(-80, 161).Select(i => spot * (1 + i / 1000.0)).Select(s => (s, g: totaleA(s))).ToList();
        var zeri = new List<double>();
        for (int i = 1; i < valori.Count; i++)
        {
            var (s1, g1) = valori[i - 1];
            var (s2, g2) = valori[i];
            if (g1 == 0 || (g1 < 0) != (g2 < 0)) zeri.Add(g2 != g1 ? s1 + (s2 - s1) * (0 - g1) / (g2 - g1) : s1);
        }
        double? zero = zeri.Count > 0 ? zeri.MinBy(z => Math.Abs(z - spot)) : null;
        double? callWall = perK.Count > 0 ? perK.MaxBy(kv => kv.Value.c).Key : null;
        double? putWall = perK.Count > 0 ? perK.MaxBy(kv => kv.Value.p).Key : null;
        var strike = perK.Where(kv => Math.Abs(kv.Key / spot - 1) <= banda)
            .Select(kv => (kv.Key, Math.Round(kv.Value.c, 3), Math.Round(-kv.Value.p, 3))).ToList();
        return new ProfiloGex(Math.Round(perK.Values.Sum(x => x.c - x.p), 2), zero, callWall, putWall, strike);
    }

    /// <summary>Le scadenze entro `giorni`, e quella di oggi (0DTE).</summary>
    public static (List<Scadenza> tutte, List<Scadenza> oggi) PerGex(Catena c, DateTime adesso)
    {
        var tutte = c.Scadenze.Where(s => s.ScadeUtc > adesso && (s.ScadeUtc - adesso).TotalDays <= GiorniGex).ToList();
        var oggiNY = TimeZoneInfo.ConvertTimeFromUtc(adesso, Cassa.NewYork).Date;
        var oggi = tutte.Where(s => TimeZoneInfo.ConvertTimeFromUtc(s.ScadeUtc, Cassa.NewYork).Date == oggiNY).ToList();
        return (tutte, oggi);
    }
}
