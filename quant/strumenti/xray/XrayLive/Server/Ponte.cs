using System.Globalization;
using System.Text.Json;

namespace XrayLive.Server;

/// <summary>La cassa di New York per data: gli orari si scrivono nel fuso del mercato, mai in UTC fisso.</summary>
internal static class Cassa
{
    public static readonly TimeZoneInfo NewYork = Zona("America/New_York", "Eastern Standard Time");

    /// <summary>Apertura e chiusura della cassa (09:30-16:00 di New York) del giorno, in ms UTC.</summary>
    public static (long apertura, long chiusura) Di(string giorno)
    {
        if (!DateTime.TryParseExact(giorno, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            return (0, 0);
        return (Ms(d.Date.AddHours(9.5)), Ms(d.Date.AddHours(16)));
    }

    /// <summary>Vero se l'istante cade nella cassa di un giorno feriale: solo li' lo spot di SPX si muove.</summary>
    public static bool Dentro(long ts)
    {
        var ny = OraNY(ts);
        if (ny.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) return false;
        var t = ny.TimeOfDay;
        return t >= TimeSpan.FromHours(9.5) && t < TimeSpan.FromHours(16);
    }

    public static DateTime OraNY(long ts) =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime, NewYork);

    private static long Ms(DateTime oraNY) =>
        new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(oraNY, DateTimeKind.Unspecified), NewYork))
            .ToUnixTimeMilliseconds();

    private static TimeZoneInfo Zona(params string[] ids)
    {
        foreach (var id in ids)
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
            catch (TimeZoneNotFoundException) { }
        }
        return TimeZoneInfo.Utc;
    }
}

/// <summary>
/// Il prezzo di ES dal bridge di ATAS (FabioDataBridge, porta 8787), per due cose: la base ES-SPX
/// e lo SPX implicito fuori dalla cassa, quando lo spot di X-Ray resta fermo all'ultima chiusura.
/// Senza bridge tutto resta in SPX e lo si dice.
/// </summary>
internal sealed class Ponte
{
    private readonly HttpClient _http;
    private readonly object _lock = new();
    private string? _chart;
    private DateTime _chartQuando = DateTime.MinValue;
    private readonly Dictionary<string, (DateTime quando, SortedList<long, double> chiusure)> _giorni = new();
    private readonly Dictionary<string, (DateTime quando, Base b)> _basi = new();
    public string? UltimoErrore { get; private set; }

    public Ponte(string indirizzo)
    {
        Indirizzo = indirizzo.TrimEnd('/');
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
    }

    public string Indirizzo { get; }

    public sealed record Base(double? Valore, int Campioni, string? Giorno, string Fonte);

    /// <summary>Il chart ES a 1 minuto registrato sul bridge (l'id cambia a ogni ricarica: si chiede).</summary>
    private string? Chart()
    {
        if (_chart is not null && DateTime.UtcNow - _chartQuando < TimeSpan.FromMinutes(2)) return _chart;
        try
        {
            using var doc = JsonDocument.Parse(_http.GetStringAsync(Indirizzo + "/charts").GetAwaiter().GetResult());
            _chart = null;
            foreach (var c in doc.RootElement.GetProperty("charts").EnumerateArray())
            {
                string strumento = c.GetProperty("instrument").GetString() ?? "";
                string tf = c.GetProperty("timeFrame").GetString() ?? "";
                // il contratto (ESZ6), non il continuo ES: sono gli stessi prezzi e il contratto e' quello di X-Ray
                if (strumento.StartsWith("ES", StringComparison.Ordinal) && strumento.Length > 2 && tf == "M1")
                {
                    _chart = c.GetProperty("id").GetString();
                    break;
                }
            }
            _chartQuando = DateTime.UtcNow;
            UltimoErrore = _chart is null ? "nessun chart ES a 1 minuto sul bridge" : null;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException)
        {
            _chart = null;
            _chartQuando = DateTime.UtcNow;
            UltimoErrore = "bridge non raggiungibile su " + Indirizzo;
        }
        return _chart;
    }

    /// <summary>Le chiusure di ES a 1 minuto del giorno (dalle 18:00 di New York del giorno prima alle 18:00).</summary>
    public SortedList<long, double> Chiusure(string giorno)
    {
        lock (_lock)
        {
            var (apertura, _) = Cassa.Di(giorno);
            if (apertura == 0) return new();
            bool oggi = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - apertura < 30 * 3_600_000L;
            if (_giorni.TryGetValue(giorno, out var c)
                && (!oggi || DateTime.UtcNow - c.quando < TimeSpan.FromSeconds(20))
                && (c.chiusure.Count > 0 || DateTime.UtcNow - c.quando < TimeSpan.FromMinutes(2)))
                return c.chiusure;

            var chiusure = new SortedList<long, double>();
            string? chart = Chart();
            if (chart is not null)
            {
                long da = apertura - (long)(15.5 * 3_600_000), a = apertura + (long)(8.5 * 3_600_000);
                string url = $"{Indirizzo}/candles?chart={chart}&from={Iso(da)}&to={Iso(a)}";
                try
                {
                    using var doc = JsonDocument.Parse(_http.GetStringAsync(url).GetAwaiter().GetResult());
                    foreach (var k in doc.RootElement.GetProperty("candles").EnumerateArray())
                    {
                        var t = DateTimeOffset.Parse(k.GetProperty("time").GetString()!, CultureInfo.InvariantCulture);
                        // la chiusura della barra che inizia a t vale all'istante t + 1 minuto
                        chiusure[t.ToUnixTimeMilliseconds() + 60_000] = k.GetProperty("close").GetDouble();
                    }
                }
                catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException
                                               or KeyNotFoundException or InvalidOperationException or FormatException)
                {
                    UltimoErrore = "candele ES non lette: " + ex.Message;
                }
            }
            // un bridge lento o occupato non deve cancellare l'ultimo dato buono: si tiene quello
            // (il 2 novembre 2026 un timeout faceva ricadere il prezzo sullo spot fermo di X-Ray)
            if (chiusure.Count == 0 && c.chiusure is { Count: > 0 } vecchie)
            {
                _giorni[giorno] = (DateTime.UtcNow - TimeSpan.FromSeconds(15), vecchie);
                return vecchie;
            }
            _giorni[giorno] = (DateTime.UtcNow, chiusure);
            return chiusure;
        }
    }

    /// <summary>L'ultima chiusura di ES all'istante ts o prima (al massimo 10 minuti prima).</summary>
    public (long ts, double prezzo)? EsAl(string giorno, long ts)
    {
        var c = Chiusure(giorno);
        int i = UltimoIndice(c.Keys, ts);
        if (i < 0 || ts - c.Keys[i] > 10 * 60_000) return null;
        return (c.Keys[i], c.Values[i]);
    }

    /// <summary>
    /// La base ES-SPX di un giorno: la mediana di ES - spot SPX sui minuti di cassa (lo spot si
    /// muove solo li'). Il 30 ottobre 2026 stava fra 27 e 37 punti, minuto per minuto: la mediana
    /// toglie il rumore dello sfasamento fra l'indice e il future.
    /// </summary>
    public Base BaseDi(string giorno, IReadOnlyList<(long ts, double spot)> spot)
    {
        lock (_lock)
        {
            bool oggi = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Cassa.Di(giorno).apertura < 30 * 3_600_000L;
            if (_basi.TryGetValue(giorno, out var b)
                && (!oggi || DateTime.UtcNow - b.quando < TimeSpan.FromMinutes(2))
                && (b.b.Valore is not null || DateTime.UtcNow - b.quando < TimeSpan.FromMinutes(2)))
                return b.b;
        }
        var chiusure = Chiusure(giorno);
        var differenze = new List<double>();
        foreach (var (ts, s) in spot)
        {
            if (!Cassa.Dentro(ts) || !double.IsFinite(s) || s <= 0) continue;
            if (chiusure.TryGetValue(ts, out var es)) differenze.Add(es - s);
        }
        Base risultato = differenze.Count >= 10
            ? new Base(Math.Round(Mediana(differenze), 2), differenze.Count, giorno, "mediana di ES - SPX sui minuti di cassa")
            : new Base(null, differenze.Count, null, UltimoErrore ?? "pochi minuti di cassa con ES e SPX insieme");
        lock (_lock) _basi[giorno] = (DateTime.UtcNow, risultato);
        return risultato;
    }

    public static int UltimoIndice(IList<long> chiavi, long ts)
    {
        int lo = 0, hi = chiavi.Count - 1, r = -1;
        while (lo <= hi)
        {
            int m = (lo + hi) / 2;
            if (chiavi[m] <= ts) { r = m; lo = m + 1; }
            else hi = m - 1;
        }
        return r;
    }

    public static double Mediana(List<double> v)
    {
        var s = v.OrderBy(x => x).ToList();
        int n = s.Count;
        return n % 2 == 1 ? s[n / 2] : (s[n / 2 - 1] + s[n / 2]) / 2;
    }

    private static string Iso(long ms) =>
        DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
}
