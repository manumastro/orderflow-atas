using System.Globalization;
using System.Text.Json;

namespace Quant.Dati;

/// <summary>
/// Il collegamento con ATAS: legge dal Fabio Data Bridge (l'indicatore che espone i dati di ATAS su
/// 127.0.0.1) e scrive in archivio. ATAS e' l'unica fonte di dati del framework.
///
/// Cosa ATAS puo' dare, e fin dove, sta in quant/metodo/i-dati-da-atas.md. Qui c'e' la parte gia'
/// cablata: le barre caricate su un chart, a pagine, e il tape completo, scambio per scambio.
/// </summary>
public sealed class PonteAtas : IDisposable
{
    private readonly HttpClient _http;
    public string Base { get; }

    public PonteAtas(string? baseUrl = null)
    {
        _http = new HttpClient { Timeout = TimeSpan.FromMinutes(6) };
        Base = (baseUrl ?? Scopri()).TrimEnd('/');
    }

    private static string Scopri()
    {
        var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fabio-data-bridge.json");
        if (File.Exists(file))
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(file));
            if (doc.RootElement.TryGetProperty("baseUrl", out var b)) return b.GetString()!;
        }
        return "http://127.0.0.1:8787";
    }

    public async Task<JsonDocument> Chiedi(string percorso, IDictionary<string, string?>? parametri = null)
    {
        var url = Base + percorso;
        if (parametri is { Count: > 0 })
            url += "?" + string.Join("&", parametri.Where(kv => kv.Value is not null)
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));
        using var risposta = await _http.GetAsync(url);
        var corpo = await risposta.Content.ReadAsStringAsync();
        if (!risposta.IsSuccessStatusCode)
            throw new HttpRequestException($"{percorso} -> HTTP {(int)risposta.StatusCode}: {corpo}");
        return JsonDocument.Parse(corpo);
    }

    /// <summary>I chart registrati nel bridge: id, strumento, periodo.</summary>
    public async Task<List<(string Id, string Strumento, string Periodo)>> Chart()
    {
        using var doc = await Chiedi("/charts");
        var lista = new List<(string, string, string)>();
        foreach (var c in doc.RootElement.GetProperty("charts").EnumerateArray())
            lista.Add((c.GetProperty("id").GetString()!, Testo(c, "instrument"), Testo(c, "timeFrame") + " " + Testo(c, "chartType")));
        return lista;
    }

    /// <summary>
    /// Tutte le barre caricate sul chart, a pagine di <paramref name="pagina"/>: il bridge limita le
    /// risposte, e una pagina per indice di barra non perde ne' duplica nulla.
    /// </summary>
    public async Task<(Barra[] Barre, string Strumento, string Periodo)> Barre(string chart, Strumento strumento, int pagina = 10_000, Action<string>? avanzamento = null)
    {
        var tutte = new List<Barra>();
        string nomeStrumento = "", periodo = "";
        var da = 0;
        while (true)
        {
            using var doc = await Chiedi("/candles", new Dictionary<string, string?>
            {
                ["chart"] = chart, ["fromBar"] = da.ToString(CultureInfo.InvariantCulture),
                ["toBar"] = (da + pagina - 1).ToString(CultureInfo.InvariantCulture),
            });
            var r = doc.RootElement;
            nomeStrumento = Testo(r, "instrument");
            periodo = Testo(r, "timeFrame") + " " + Testo(r, "chartType");
            var totale = r.GetProperty("totalBars").GetInt32();
            foreach (var c in r.GetProperty("candles").EnumerateArray())
                tutte.Add(DaJson(c, strumento));
            avanzamento?.Invoke($"  barre {Math.Min(da + pagina, totale):N0} di {totale:N0}");
            da += pagina;
            if (da >= totale) break;
        }
        return ([.. tutte.DistinctBy(b => b.Inizio).OrderBy(b => b.Inizio)], nomeStrumento, periodo);
    }

    /// <summary>
    /// Registra la catena delle opzioni dello strumento del chart, cosi' com'e' (/opzioni del bridge):
    /// per le prossime <paramref name="scadenze"/> serie, i <paramref name="strike"/> strike piu' vicini
    /// al prezzo per lato. Un file JSON per serie in quant/dati/&lt;simbolo&gt;/opzioni/&lt;giorno&gt;/&lt;HHmm&gt;Z-&lt;scadenza&gt;.json:
    /// il dato grezzo, senza calcoli, perche' la GEX si calcola dopo con la convenzione dichiarata.
    /// ATAS non ha la storia delle opzioni: si registra da oggi in avanti.
    /// </summary>
    public async Task<List<string>> RegistraOpzioni(string chart, string simbolo, int scadenze = 3, int strike = 40, Action<string>? avanzamento = null)
    {
        using var elenco = await Chiedi("/opzioni", new Dictionary<string, string?> { ["chart"] = chart });
        var oggi = DateTime.UtcNow.Date;
        var giorni = elenco.RootElement.GetProperty("series").EnumerateArray()
            .Select(s => DateTime.Parse(s.GetProperty("expiration").GetString()!, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).Date)
            .Where(d => d >= oggi).Distinct().OrderBy(d => d).Take(scadenze).ToList();
        var adesso = DateTime.UtcNow;
        var cartella = Path.Combine(Archivio.Radice, simbolo.ToUpperInvariant(), "opzioni", adesso.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        Directory.CreateDirectory(cartella);
        var scritti = new List<string>();
        foreach (var g in giorni)
        {
            var scadenza = g.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            using var doc = await Chiedi("/opzioni", new Dictionary<string, string?>
            {
                ["chart"] = chart, ["expiration"] = scadenza,
                ["strikes"] = strike.ToString(CultureInfo.InvariantCulture), ["wait"] = "30",
            });
            var file = Path.Combine(cartella, $"{adesso:HHmm}Z-{scadenza}.json");
            await File.WriteAllTextAsync(file, doc.RootElement.GetRawText());
            var r = doc.RootElement;
            avanzamento?.Invoke($"  {scadenza}: {r.GetProperty("quoted").GetInt32()} quotate su {r.GetProperty("count").GetInt32()} opzioni -> {Path.GetFileName(file)}");
            scritti.Add(file);
        }
        return scritti;
    }

    /// <summary>
    /// Il tape completo fra <paramref name="da"/> e <paramref name="a"/> (a escluso): ogni scambio
    /// aggregato con i suoi tick, da /cumulative in modalita' Filter senza soglia di volume. Il bridge
    /// include entrambi gli estremi, quindi qui si scarta cio' che cade esattamente su
    /// <paramref name="a"/>: lo prende la finestra dopo. Se il bridge taglia la risposta, la finestra
    /// si divide a meta' finche' non entra.
    /// </summary>
    public async Task Scambi(string chart, Strumento s, DateTime da, DateTime a, List<ScambioAggregato> aggregati, List<Scambio> tick)
    {
        var url = $"{Base}/cumulative?chart={Uri.EscapeDataString(chart)}&mode=Filter&minVolume=0&ticks=1" +
                  $"&from={da:yyyy-MM-ddTHH:mm:ss.fffZ}&to={a:yyyy-MM-ddTHH:mm:ss.fffZ}";
        using var risposta = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        await using var flusso = await risposta.Content.ReadAsStreamAsync();
        if (!risposta.IsSuccessStatusCode)
            throw new HttpRequestException($"/cumulative -> HTTP {(int)risposta.StatusCode}: {await new StreamReader(flusso).ReadToEndAsync()}");
        using var doc = await JsonDocument.ParseAsync(flusso);
        var r = doc.RootElement;
        if (r.GetProperty("truncated").GetBoolean())
        {
            if (a - da <= TimeSpan.FromSeconds(1))
                throw new InvalidOperationException($"/cumulative tagliato anche su un secondo: {da:O}");
            var meta = da + (a - da) / 2;
            await Scambi(chart, s, da, meta, aggregati, tick);
            await Scambi(chart, s, meta, a, aggregati, tick);
            return;
        }
        var fine = Tempo.Ms(a);
        foreach (var t in r.GetProperty("trades").EnumerateArray())
        {
            var tempo = Tempo.Ms(t.GetProperty("time").GetDateTime());
            if (tempo >= fine) continue;
            var singoli = t.GetProperty("ticks");
            var n = 0;
            if (singoli.ValueKind == JsonValueKind.Array)
                foreach (var k in singoli.EnumerateArray())
                {
                    tick.Add(new Scambio
                    {
                        Tempo = Tempo.Ms(k.GetProperty("time").GetDateTime()), Prezzo = s.InTick(k.GetProperty("price").GetDouble()),
                        Volume = (int)k.GetProperty("volume").GetDecimal(), Lato = Lato(k),
                    });
                    n++;
                }
            aggregati.Add(new ScambioAggregato
            {
                Tempo = tempo, Lato = Lato(t), Volume = (int)t.GetProperty("volume").GetDecimal(),
                PrimoPrezzo = s.InTick(t.GetProperty("firstPrice").GetDouble()), UltimoPrezzo = s.InTick(t.GetProperty("lastPrice").GetDouble()),
                BidPrima = Prezzo(t, "previousBid", s), AskPrima = Prezzo(t, "previousAsk", s),
                BidDopo = Prezzo(t, "newBid", s), AskDopo = Prezzo(t, "newAsk", s), Tick = n,
            });
        }
    }

    private static int Lato(JsonElement e) => e.GetProperty("direction").GetString() switch { "Buy" => 1, "Sell" => -1, _ => 0 };

    private static int Prezzo(JsonElement e, string nome, Strumento s) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind == JsonValueKind.Number ? s.InTick(v.GetDouble()) : 0;

    internal static Barra DaJson(JsonElement c, Strumento s)
    {
        long Ms(string nome) => Tempo.Ms(DateTime.Parse(c.GetProperty(nome).GetString()!, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal));
        return new Barra
        {
            Inizio = Ms("time"), Fine = Ms("lastTime"),
            Apertura = s.InTick(c.GetProperty("open").GetDouble()), Massimo = s.InTick(c.GetProperty("high").GetDouble()),
            Minimo = s.InTick(c.GetProperty("low").GetDouble()), Chiusura = s.InTick(c.GetProperty("close").GetDouble()),
            Volume = (int)c.GetProperty("volume").GetDecimal(),
            VolumeBid = (int)c.GetProperty("bid").GetDecimal(), VolumeAsk = (int)c.GetProperty("ask").GetDecimal(),
        };
    }

    private static string Testo(JsonElement e, string nome) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind != JsonValueKind.Null ? v.ToString() : "";

    public void Dispose() => _http.Dispose();
}
