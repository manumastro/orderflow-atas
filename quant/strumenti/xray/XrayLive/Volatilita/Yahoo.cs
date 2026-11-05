using System.Collections.Concurrent;
using System.Text.Json;
using XrayLive.Server;

namespace XrayLive.Volatilita;

/// <summary>
/// Gli indici ufficiali (VIX, VXN) che il Cboe pubblica, via Yahoo: il riferimento con cui si confronta cio'
/// che ricostruiamo. Fonte pubblica fuori da ATAS, come la macro dello studio; solo contesto.
/// A 1 minuto Yahoo tiene 7 giorni; piu' indietro si usano i 5 minuti (60 giorni).
/// </summary>
internal sealed class Yahoo
{
    private readonly HttpClient _http;
    private readonly ConcurrentDictionary<string, (DateTime quando, List<(long ts, double v)> punti)> _cache = new();
    public string? UltimoErrore { get; private set; }
    public DateTime? UltimoOk { get; private set; }

    /// <summary>Lo stato per simbolo (^VIX, ^VXN, QQQ), per la pagina Fonti (5 novembre 2026): un simbolo che non
    /// risponde non deve nascondersi dietro un altro che risponde.</summary>
    public readonly ConcurrentDictionary<string, (DateTime? ok, DateTime? tentato, string? errore)> PerSimbolo = new();

    private void Ok(string simbolo)
    {
        UltimoErrore = null;
        UltimoOk = DateTime.UtcNow;
        PerSimbolo[simbolo] = (DateTime.UtcNow, DateTime.UtcNow, null);
    }

    private void Errore(string simbolo, string messaggio)
    {
        UltimoErrore = $"{simbolo}: {messaggio}";
        var prima = PerSimbolo.GetValueOrDefault(simbolo);
        PerSimbolo[simbolo] = (prima.ok, DateTime.UtcNow, messaggio);
    }

    public Yahoo()
    {
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
    }

    public sealed record Quotazione(double Prezzo, long Quando, string Sessione, double? ChiusuraPrima);

    private readonly ConcurrentDictionary<string, (DateTime letto, Quotazione q)> _prezzi = new();

    /// <summary>
    /// L'ultimo prezzo di un titolo (QQQ) anche fuori dalla cassa: il pre-mercato e il dopo-mercato (5 novembre 2026,
    /// richiesta dell'utente: "il prezzo vero di QQQ", perche' il file del Cboe ha solo i prezzi della cassa, in
    /// ritardo di 15 minuti). Si rilegge al massimo ogni 30 secondi. Sessione: "pre", "cassa", "dopo" o "chiuso".
    /// </summary>
    public Quotazione? Prezzo(string simbolo)
    {
        if (_prezzi.TryGetValue(simbolo, out var c) && DateTime.UtcNow - c.letto < TimeSpan.FromSeconds(30)) return c.q;
        string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{Uri.EscapeDataString(simbolo)}" +
                     "?interval=1m&range=1d&includePrePost=true";
        try
        {
            using var doc = JsonDocument.Parse(_http.GetStringAsync(url).GetAwaiter().GetResult());
            var r = doc.RootElement.GetProperty("chart").GetProperty("result")[0];
            var meta = r.GetProperty("meta");
            double prezzo = meta.GetProperty("regularMarketPrice").GetDouble();
            long quando = meta.GetProperty("regularMarketTime").GetInt64() * 1000;
            // l'ultimo minuto con un prezzo, compreso il pre-mercato e il dopo-mercato
            if (r.TryGetProperty("timestamp", out var tss))
            {
                var chiusure = r.GetProperty("indicators").GetProperty("quote")[0].GetProperty("close");
                int i = 0;
                foreach (var t in tss.EnumerateArray())
                {
                    var v = chiusure[i++];
                    if (v.ValueKind == JsonValueKind.Number && t.GetInt64() * 1000 >= quando)
                    {
                        prezzo = Math.Round(v.GetDouble(), 2);
                        quando = t.GetInt64() * 1000 + 60_000;
                    }
                }
            }
            double? prima = meta.TryGetProperty("chartPreviousClose", out var pc) && pc.ValueKind == JsonValueKind.Number ? pc.GetDouble() : null;
            string sessione = "chiuso";
            if (meta.TryGetProperty("currentTradingPeriod", out var per))
            {
                long ora = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                foreach (var (nome, chiave) in new[] { ("pre", "pre"), ("cassa", "regular"), ("dopo", "post") })
                {
                    var p = per.GetProperty(chiave);
                    if (ora >= p.GetProperty("start").GetInt64() && ora < p.GetProperty("end").GetInt64()) sessione = nome;
                }
            }
            var q = new Quotazione(prezzo, Math.Min(quando, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()), sessione, prima);
            _prezzi[simbolo] = (DateTime.UtcNow, q);
            Ok(simbolo);
            return q;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException or IndexOutOfRangeException)
        {
            Errore(simbolo, ex.Message);
            return c.q;
        }
    }

    /// <summary>I punti di un indice (^VIX, ^VXN) nella giornata di New York; oggi si rilegge ogni 2 minuti.</summary>
    public List<(long ts, double v)> Giorno(string simbolo, string giorno)
    {
        string chiave = simbolo + giorno;
        var (apertura, _) = Cassa.Di(giorno);
        if (apertura == 0) return new();
        // la giornata intera: dalle 18:00 di New York del giorno prima alle 18:00 (VIX e VXN hanno le ore globali)
        long da = apertura - (long)(15.5 * 3_600_000), a = apertura + (long)(8.5 * 3_600_000);
        bool chiusa = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > a + 3_600_000;
        if (_cache.TryGetValue(chiave, out var c) && (chiusa || DateTime.UtcNow - c.quando < TimeSpan.FromMinutes(2)))
            return c.punti;
        bool recente = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - da < 6.5 * 86_400_000;
        string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{Uri.EscapeDataString(simbolo)}" +
                     $"?period1={da / 1000}&period2={a / 1000}&interval={(recente ? "1m" : "5m")}";
        var punti = new List<(long, double)>();
        try
        {
            using var doc = JsonDocument.Parse(_http.GetStringAsync(url).GetAwaiter().GetResult());
            var r = doc.RootElement.GetProperty("chart").GetProperty("result")[0];
            if (r.TryGetProperty("timestamp", out var tss))
            {
                var chiusure = r.GetProperty("indicators").GetProperty("quote")[0].GetProperty("close");
                int i = 0;
                foreach (var t in tss.EnumerateArray())
                {
                    var v = chiusure[i++];
                    // la barra che inizia a t chiude a t + intervallo: il valore vale alla fine
                    if (v.ValueKind == JsonValueKind.Number) punti.Add((t.GetInt64() * 1000 + (recente ? 60_000 : 300_000), Math.Round(v.GetDouble(), 2)));
                }
            }
            Ok(simbolo);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException or IndexOutOfRangeException)
        {
            Errore(simbolo, ex.Message);
            if (c.punti is { Count: > 0 }) return c.punti;
        }
        _cache[chiave] = (DateTime.UtcNow, punti);
        return punti;
    }
}
