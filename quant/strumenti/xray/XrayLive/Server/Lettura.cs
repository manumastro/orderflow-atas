using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace XrayLive.Server;

// La lettura dell'archivio per il server: le stesse regole di scripts/xray_query.py e di
// Archivio.Chiave (una colonna per chiave, l'ultima versione vince; prima il .gz, poi il .jsonl),
// ma con una cache per giornata che legge solo le righe nuove del file in corso.

/// <summary>Una colonna per strike. <c>From</c> e' l'inizio della finestra della colonna (0 per GEX e VEX, che
/// sono posizioni; ts - 60 s per il DEX, che e' un flusso): serve a riconoscere i minuti anomali della Deriva.</summary>
internal sealed record ColStrike(long Ts, double Spot, long Scadenza, int Giorni,
    double[] Strike, double[] Call, double[] Put, long From = 0)
{
    public double Netto(int i) => Call[i] + Put[i];
}

internal sealed record ColPiano(long Ts, long AsOf, double[] Prezzi, float[] Valori);

internal sealed record ColPressione(long Ts, double Spot, double Delta, double Trades);

/// <summary>Una traccia dei livelli: un valore al minuto, dal minuto First della superficie.</summary>
internal sealed record Traccia(int Tipo, int Primo, double?[] Prezzo, int?[] Riga, int?[] Sotto, int?[] Sopra, double?[] Peso)
{
    public int Lunghezza => Tipo == 0 ? Prezzo.Length : Riga.Length;
}

internal sealed record ColLivelli(string Superficie, long Ts, long Fino, long PassoMs, double[] Prezzi, List<Traccia> Tracce);

/// <summary>Le colonne di una famiglia in una giornata, in ordine di tempo, e da dove riprendere.</summary>
internal abstract class GiornataLetta
{
    public DateTime Usata = DateTime.UtcNow;
}

internal sealed class Giornata<T> : GiornataLetta
{
    public readonly List<T> Colonne = new();
    public readonly Dictionary<string, int> Indice = new(StringComparer.Ordinal);
    public long LunghezzaGz = -1;
    public long Offset;
}

internal static class Lettura
{
    public static string Percorso(string root, string famiglia, string giorno, string ticker = "SPX") =>
        Path.Combine(root, "series", ticker, famiglia, giorno + ".jsonl");

    public static List<string> Giorni(string root, string famiglia, string ticker = "SPX")
    {
        string dir = Path.Combine(root, "series", ticker, famiglia);
        if (!Directory.Exists(dir)) return new();
        return Directory.EnumerateFiles(dir, "*.jsonl*")
            .Select(Path.GetFileName)
            .Select(n => n!.Length >= 10 ? n[..10] : n!)
            .Where(n => n.Length == 10 && n[4] == '-' && n[7] == '-')
            .Distinct()
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// Porta la giornata al passo con i file: il .gz si rilegge solo se cambia lunghezza (una
    /// compattazione), il .jsonl dal punto in cui ci si era fermati, e solo fino all'ultimo a capo
    /// (la riga che il lettore sta scrivendo si prende al giro dopo).
    /// </summary>
    public static void Aggiorna<T>(Giornata<T> g, string percorso,
        Func<JsonElement, JsonElement, (string chiave, T valore)?> leggi, Func<T, long> ts)
    {
        string gz = percorso + ".gz";
        long lunghezzaGz = File.Exists(gz) ? new FileInfo(gz).Length : 0;
        bool nuovo = false;
        if (lunghezzaGz != g.LunghezzaGz)
        {
            g.Colonne.Clear();
            g.Indice.Clear();
            g.Offset = 0;
            g.LunghezzaGz = lunghezzaGz;
            if (lunghezzaGz > 0)
                foreach (var riga in RigheGz(gz))
                    nuovo |= Accogli(g, riga, leggi);
        }

        if (File.Exists(percorso))
        {
            long lunghezza = new FileInfo(percorso).Length;
            if (lunghezza < g.Offset)
            {
                // il .jsonl e' stato accodato al .gz e ricreato: si riparte da capo
                g.LunghezzaGz = -1;
                Aggiorna(g, percorso, leggi, ts);
                return;
            }
            if (lunghezza > g.Offset)
            {
                var (righe, letti) = RigheDa(percorso, g.Offset);
                g.Offset += letti;
                foreach (var riga in righe)
                    nuovo |= Accogli(g, riga, leggi);
            }
        }

        if (nuovo) Riordina(g, ts);
        g.Usata = DateTime.UtcNow;
    }

    private static bool Accogli<T>(Giornata<T> g, string riga,
        Func<JsonElement, JsonElement, (string chiave, T valore)?> leggi)
    {
        if (riga.Length < 2) return false;
        JsonDocument doc;
        try { doc = JsonDocument.Parse(riga); }
        catch (JsonException) { return false; }
        using (doc)
        {
            var testa = doc.RootElement;
            if (testa.ValueKind != JsonValueKind.Object
                || !testa.TryGetProperty("columns", out var colonne)
                || colonne.ValueKind != JsonValueKind.Array) return false;
            bool cambiato = false;
            foreach (var col in colonne.EnumerateArray())
            {
                if (col.ValueKind != JsonValueKind.Object) continue;
                if (leggi(testa, col) is not { } letto) continue;
                if (g.Indice.TryGetValue(letto.chiave, out int i))
                    g.Colonne[i] = letto.valore;   // una revisione: vince l'ultima
                else
                {
                    g.Indice[letto.chiave] = g.Colonne.Count;
                    g.Colonne.Add(letto.valore);
                }
                cambiato = true;
            }
            return cambiato;
        }
    }

    private static void Riordina<T>(Giornata<T> g, Func<T, long> ts)
    {
        var chiavi = g.Indice.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();
        var ordine = Enumerable.Range(0, g.Colonne.Count).OrderBy(i => ts(g.Colonne[i])).ThenBy(i => i).ToList();
        var colonne = ordine.Select(i => g.Colonne[i]).ToList();
        g.Colonne.Clear();
        g.Colonne.AddRange(colonne);
        g.Indice.Clear();
        for (int k = 0; k < ordine.Count; k++) g.Indice[chiavi[ordine[k]]] = k;
    }

    private static IEnumerable<string> RigheGz(string path)
    {
        var righe = new List<string>();
        try
        {
            using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var gz = new GZipStream(file, CompressionMode.Decompress);
            using var reader = new StreamReader(gz, Encoding.UTF8);
            while (reader.ReadLine() is { } line)
                if (line.Length > 0) righe.Add(line);
        }
        catch (IOException)
        {
            // un .gz rinominato mentre lo si legge: si tiene quello che si e' letto
        }
        catch (InvalidDataException)
        {
        }
        return righe;
    }

    /// <summary>Le righe complete dopo offset, e quanti byte coprono (fino all'ultimo a capo).</summary>
    public static (List<string> righe, long letti) RigheDa(string path, long offset)
    {
        byte[] dati;
        try
        {
            using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            if (offset >= file.Length) return (new(), 0);
            file.Seek(offset, SeekOrigin.Begin);
            dati = new byte[file.Length - offset];
            int letto = 0;
            while (letto < dati.Length)
            {
                int n = file.Read(dati, letto, dati.Length - letto);
                if (n == 0) break;
                letto += n;
            }
            if (letto < dati.Length) Array.Resize(ref dati, letto);
        }
        catch (IOException)
        {
            return (new(), 0);
        }
        int fine = Array.LastIndexOf(dati, (byte)'\n');
        if (fine < 0) return (new(), 0);
        var righe = Encoding.UTF8.GetString(dati, 0, fine)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.TrimEnd('\r'))
            .ToList();
        return (righe, fine + 1);
    }

    // ---------------------------------------------------------------- le colonne

    public static (string, ColStrike)? Strike(JsonElement testa, JsonElement col)
    {
        long ts = Lungo(col, "ts");
        var s = Numeri(col, "strikes");
        var c = Numeri(col, "call");
        var p = Numeri(col, "put");
        int n = Math.Min(s.Length, Math.Min(c.Length, p.Length));
        for (int i = 0; i < n; i++)
        {
            if (!double.IsFinite(c[i])) c[i] = 0;
            if (!double.IsFinite(p[i])) p[i] = 0;
        }
        long scadenza = Lungo(col, "expiry");
        int giorni = (int)Lungo(col, "dte");
        return ($"{ts}|{scadenza}|{giorni}",
            new ColStrike(ts, Num(col, "spot"), scadenza, giorni, s[..n], c[..n], p[..n], Lungo(col, "from")));
    }

    public static (string, ColPiano)? Piano(JsonElement testa, JsonElement col)
    {
        long ts = Lungo(col, "ts");
        long asOf = Lungo(testa, "asOf");
        if (asOf == 0) asOf = Lungo(col, "asOf");
        var prezzi = Numeri(col, "prices");
        var valori = Numeri(col, "values");
        int n = Math.Min(prezzi.Length, valori.Length);
        if (n == 0) return null;
        var v = new float[n];
        for (int i = 0; i < n; i++) v[i] = (float)valori[i];
        // la griglia dei prezzi e' la stessa per tutte le colonne di un giorno: una copia sola
        // (30.000 colonne di previsione per giorno, 1,6 KB di prezzi ciascuna)
        prezzi = Griglia(prezzi[..n]);
        string chiave = Str(testa, "metric") == "observed" ? $"{ts}" : $"{asOf}|{ts}";
        return (chiave, new ColPiano(ts, asOf, prezzi, v));
    }

    public static (string, ColPressione)? Pressione(JsonElement testa, JsonElement col)
    {
        long ts = Lungo(col, "ts");
        return ($"{ts}", new ColPressione(ts, Num(col, "spot"), Num(col, "delta"), Num(col, "trades")));
    }

    public static (string, ColLivelli)? Livelli(JsonElement testa, JsonElement col)
    {
        string superficie = Str(col, "surface");
        long ts = Lungo(col, "ts");
        long fino = Lungo(col, "until");
        var tracce = new List<Traccia>();
        if (col.TryGetProperty("tracks", out var tr) && tr.ValueKind == JsonValueKind.Array)
        {
            foreach (var t in tr.EnumerateArray())
            {
                if (t.ValueKind != JsonValueKind.Object) continue;
                tracce.Add(new Traccia((int)Lungo(t, "kind"), (int)Lungo(t, "first"),
                    Opzionali(t, "price"), Interi(t, "row"), Interi(t, "lower"), Interi(t, "upper"), Opzionali(t, "weight")));
            }
        }
        long passo = Lungo(col, "spanMs");
        return ($"{superficie}|{ts}|{fino}",
            new ColLivelli(superficie, ts, fino, passo > 0 ? passo : 60_000, Numeri(col, "prices"), tracce));
    }

    private static readonly object GriglieLock = new();
    private static readonly List<double[]> Griglie = new();

    private static double[] Griglia(double[] prezzi)
    {
        lock (GriglieLock)
        {
            foreach (var g in Griglie)
                if (g.AsSpan().SequenceEqual(prezzi)) return g;
            if (Griglie.Count > 64) Griglie.RemoveAt(0);
            Griglie.Add(prezzi);
            return prezzi;
        }
    }

    // ---------------------------------------------------------------- JSON

    public static double Num(JsonElement e, string nome) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : double.NaN;

    public static long Lungo(JsonElement e, string nome) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind == JsonValueKind.Number
            ? (v.TryGetInt64(out var l) ? l : (long)v.GetDouble())
            : 0;

    public static string Str(JsonElement e, string nome) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";

    public static double[] Numeri(JsonElement e, string nome)
    {
        if (!e.TryGetProperty(nome, out var a) || a.ValueKind != JsonValueKind.Array) return Array.Empty<double>();
        var r = new double[a.GetArrayLength()];
        int i = 0;
        foreach (var x in a.EnumerateArray())
            r[i++] = x.ValueKind == JsonValueKind.Number ? x.GetDouble() : double.NaN;
        return r;
    }

    private static double?[] Opzionali(JsonElement e, string nome) =>
        Numeri(e, nome).Select(x => double.IsFinite(x) ? x : (double?)null).ToArray();

    private static int?[] Interi(JsonElement e, string nome) =>
        Numeri(e, nome).Select(x => double.IsFinite(x) ? (int)x : (int?)null).ToArray();
}
