using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;

namespace XrayGui;

// Lo stato che XrayLive pubblica (status.json, journal.jsonl, latest/): quello che serve alla
// finestra di gestione. Le misure le calcola il server della pagina (XrayLive/Server).

internal sealed record Stato(
    bool Esiste, bool Vivo, double SecondiFa, long Chunk, long Errori, string? UltimoErrore,
    string UltimoChunk, double? UltimoChunkSecondiFa, double RamMB, double ArchivioMB, int Giornate,
    long Scritte, long Doppie, long Revisioni, int Pid);

internal static class Dati
{
    public static readonly TimeZoneInfo NewYork = FindZone("America/New_York", "Eastern Standard Time");

    // ---------------------------------------------------------------- file

    public static string? Testo(string path)
    {
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        catch
        {
            return null;   // un file in scrittura o appena rinominato: si rilegge al prossimo giro
        }
    }

    public static JsonObject? Json(string path)
    {
        string? text = Testo(path);
        if (text is null) return null;
        try { return JsonNode.Parse(text) as JsonObject; }
        catch { return null; }
    }

    public static JsonObject? Latest(string root, string famiglia) =>
        Json(Path.Combine(root, "latest", famiglia + ".json"));

    public static JsonObject? Ultima(JsonObject? latest)
    {
        if (latest is null) return null;
        if (latest["last"] is JsonObject last) return last;
        return latest["columns"] is JsonArray cols && cols.Count > 0 ? cols[^1] as JsonObject : null;
    }

    public static List<string> Giorni(string root, string famiglia, string ticker = "SPX")
    {
        string dir = Path.Combine(root, "series", ticker, famiglia);
        if (!Directory.Exists(dir)) return new();
        return Directory.EnumerateFiles(dir, "*.jsonl*")
            .Select(Path.GetFileName)
            .Select(n => n!.Length >= 10 ? n[..10] : n!)
            .Distinct()
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToList();
    }

    // ---------------------------------------------------------------- misure

    public static Stato LeggiStato(string root)
    {
        var s = Json(Path.Combine(root, "status.json"));
        if (s is null)
            return new Stato(false, false, double.NaN, 0, 0, null, "-", null, 0, 0, 0, 0, 0, 0, 0);
        double fa = SecondiDa(Str(s["lastSweepUtc"]));
        double? chunkFa = Str(s["lastChunkUtc"]) is { Length: > 0 } lc ? SecondiDa(lc) : null;
        return new Stato(true, fa < 120, fa, Lungo(s["chunks"]), Lungo(s["errors"]),
            s["lastError"] is JsonValue e && e.TryGetValue<string>(out var err) ? err : null,
            Str(s["lastChunk"]), chunkFa, Num(s["workingSetMB"]), Num(s["archiveMB"]), (int)Num(s["archiveDays"]),
            Lungo(s["columnsWritten"]), Lungo(s["columnsDuplicate"]), Lungo(s["columnsRevised"]), (int)Num(s["pid"]));
    }

    /// <summary>Quando e' arrivato l'ultimo chunk di ogni famiglia (dalla coda del registro).</summary>
    public static Dictionary<string, DateTime> Ricevute(string root)
    {
        var out_ = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        string path = Path.Combine(root, "journal.jsonl");
        if (!File.Exists(path)) return out_;
        string text;
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            long start = Math.Max(0, stream.Length - 2_000_000);
            stream.Seek(start, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            text = reader.ReadToEnd();
        }
        catch
        {
            return out_;
        }
        foreach (var line in text.Split('\n'))
        {
            if (line.Length < 2 || line[0] != '{') continue;
            JsonObject? r;
            try { r = JsonNode.Parse(line) as JsonObject; }
            catch { continue; }
            if (r is null) continue;
            string fam = Str(r["family"]);
            if (fam.Length == 0)
            {
                string variant = Str(r["variant"]).Replace('/', '_');
                fam = Str(r["metric"]) + (variant.Length > 0 ? "__" + variant : "");
            }
            if (DateTime.TryParse(Str(r["seenUtc"]), CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var when))
                out_[fam] = when;
        }
        return out_;
    }

    // ---------------------------------------------------------------- formati

    public static string OraNY(long ms) => ms <= 0 ? "-"
        : TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime, NewYork).ToString("dd/MM HH:mm");

    public static string OraItalia(long ms) => ms <= 0 ? "-"
        : DateTimeOffset.FromUnixTimeMilliseconds(ms).LocalDateTime.ToString("HH:mm");

    public static string Eta(double secondi)
    {
        if (!double.IsFinite(secondi)) return "?";
        if (secondi < 90) return $"{secondi:N0} s";
        if (secondi < 5400) return $"{secondi / 60:N0} min";
        if (secondi < 172800) return $"{secondi / 3600:N1} h";
        return $"{secondi / 86400:N1} g";
    }

    public static string N(double v, int dec = 1) => double.IsFinite(v) ? v.ToString("N" + dec, CultureInfo.GetCultureInfo("it-IT")) : "-";

    public static double[] Numeri(JsonNode? node)
    {
        if (node is not JsonArray a) return Array.Empty<double>();
        var r = new double[a.Count];
        for (int i = 0; i < a.Count; i++) r[i] = a[i] is null ? double.NaN : Num(a[i]);
        return r;
    }

    public static double Num(JsonNode? node)
    {
        if (node is JsonValue v)
        {
            if (v.TryGetValue<double>(out var d)) return d;
            if (v.TryGetValue<long>(out var l)) return l;
        }
        return double.NaN;
    }

    public static long Lungo(JsonNode? node) => Num(node) is var d && double.IsFinite(d) ? (long)d : 0;

    public static string Str(JsonNode? n) => n is JsonValue v && v.TryGetValue<string>(out var s) ? s : "";

    private static double SecondiDa(string iso) =>
        DateTime.TryParse(iso, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var when)
            ? (DateTime.UtcNow - when).TotalSeconds
            : double.NaN;

    private static TimeZoneInfo FindZone(params string[] ids)
    {
        foreach (var id in ids)
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
            catch { }
        }
        return TimeZoneInfo.Utc;
    }
}
