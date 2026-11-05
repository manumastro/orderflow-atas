using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Nodes;

namespace XrayLive;

// L'archivio delle serie: series/<TICKER>/<famiglia>/<giorno>.jsonl, una riga per chunk.
//
// Tre regole, misurate il 30 ottobre 2026 (3,7 GB in due mesi, 376 MB in due ore e mezza):
//
// 1. Una colonna si scrive una volta. I chunk di X-Ray sono cumulativi (le GEX per strike
//    arrivano a blocchi orari che crescono minuto per minuto), quindi la stessa colonna torna
//    fino a 60 volte, identica. Ogni colonna ha una chiave (Chiave) e un'impronta del contenuto
//    (Impronta): si scrive se la chiave e' nuova o se il contenuto e' cambiato (revisione).
// 2. I numeri si arrotondano a 6 cifre significative (le greche) e a 2 decimali (i prezzi):
//    i valori arrivano in float32, che ne ha 7, e scritti da double erano 20 caratteri di rumore.
// 3. Le giornate chiuse si comprimono in <giorno>.jsonl.gz. Un chunk tardivo di una giornata
//    chiusa va in un <giorno>.jsonl nuovo, che alla chiusura successiva si accoda al .gz come
//    altro membro gzip (chi legge li legge tutti e due: vedi Righe).
internal sealed class Archivio
{
    private readonly string _root;
    private readonly Dictionary<string, Giorno> _aperti = new(StringComparer.OrdinalIgnoreCase);

    public long Scritte, Doppie, Revisioni;

    private sealed class Giorno
    {
        public readonly Dictionary<string, ulong> Chiavi = new(StringComparer.Ordinal);
        public DateTime Usato = DateTime.UtcNow;
    }

    public Archivio(string root) => _root = root;

    public string Serie => Path.Combine(_root, "series");

    /// <summary>Accoda le colonne nuove o cambiate di un chunk al file del giorno; ritorna quante.</summary>
    public int Accoda(string percorso, JsonObject testa, JsonArray colonne)
    {
        var giorno = Apri(percorso);
        giorno.Usato = DateTime.UtcNow;

        var nuove = new JsonArray();
        foreach (var nodo in colonne)
        {
            if (nodo is not JsonObject col) continue;
            Arrotonda(col);
            string chiave = Chiave(testa, col);
            ulong impronta = Impronta(col);
            if (giorno.Chiavi.TryGetValue(chiave, out var vecchia))
            {
                if (vecchia == impronta)
                {
                    Doppie++;
                    continue;
                }
                Revisioni++;
            }
            giorno.Chiavi[chiave] = impronta;
            nuove.Add(col.DeepClone());
        }

        if (nuove.Count == 0) return 0;
        var riga = (JsonObject)testa.DeepClone();
        riga["columns"] = nuove;
        Program.AppendShared(percorso, riga.ToJsonString() + "\n");
        Scritte += nuove.Count;
        return nuove.Count;
    }

    private Giorno Apri(string percorso)
    {
        if (_aperti.TryGetValue(percorso, out var giorno)) return giorno;

        // le chiavi gia' scritte (anche nel .gz di una giornata chiusa) si rileggono una volta
        giorno = new Giorno();
        foreach (var riga in Righe(percorso))
        {
            if (Leggi(riga) is not { } r) continue;
            var (testa, colonne) = r;
            foreach (var nodo in colonne)
                if (nodo is JsonObject col)
                {
                    Arrotonda(col);   // idempotente: l'impronta si calcola sempre sullo stesso testo
                    giorno.Chiavi[Chiave(testa, col)] = Impronta(col);
                }
        }
        _aperti[percorso] = giorno;

        // un giorno non toccato da un'ora esce dalla memoria
        foreach (var vecchio in _aperti.Where(kv => DateTime.UtcNow - kv.Value.Usato > TimeSpan.FromHours(1))
                     .Select(kv => kv.Key).ToList())
            _aperti.Remove(vecchio);
        return giorno;
    }

    // ---------------------------------------------------------------- chiavi e impronte

    /// <summary>Che cosa rende unica una colonna dentro la sua famiglia e la sua giornata.</summary>
    public static string Chiave(JsonObject testa, JsonObject col)
    {
        string tipo = Str(testa["type"]);
        string ts = Lungo(col["ts"] ?? col["k"]);
        return tipo switch
        {
            // la GEX/DEX/VEX per strike: un istante per scadenza
            "strikes" => $"{ts}|{Lungo(col["expiry"])}|{Lungo(col["dte"])}",
            // il profilo osservato e' uno per istante; previsione e giorno dopo cambiano a ogni
            // asOf (ogni chunk e' una previsione nuova degli stessi istanti): la chiave lo porta
            "plane" => Str(testa["metric"]) == "observed" ? ts : $"{Lungo(testa["asOf"])}|{ts}",
            "levels" => $"{Str(col["surface"])}|{ts}|{Lungo(col["until"])}",
            "seriesmap" => $"{ts}|{Lungo(col["to"])}",
            _ => ts,
        };
    }

    /// <summary>FNV-1a a 64 bit del JSON della colonna (gia' arrotondata).</summary>
    public static ulong Impronta(JsonObject col)
    {
        ulong h = 14695981039346656037UL;
        foreach (byte b in Encoding.UTF8.GetBytes(col.ToJsonString()))
        {
            h ^= b;
            h *= 1099511628211UL;
        }
        return h;
    }

    // ---------------------------------------------------------------- arrotondamento

    private static readonly HashSet<string> Greche = new() { "values", "call", "put", "delta", "weight" };
    private static readonly HashSet<string> Prezzi = new() { "price", "spot" };

    /// <summary>Greche a 6 cifre significative, prezzi a 2 decimali; tempi e indici restano interi.</summary>
    public static void Arrotonda(JsonObject col)
    {
        // "k" era una chiave di ordinamento uguale a "ts" (fino al 30 ottobre 2026): non serve
        // piu', e toglierla fa coincidere le impronte delle colonne vecchie e nuove
        col.Remove("k");
        foreach (var (nome, valore) in col.ToList())
        {
            if (Greche.Contains(nome)) col[nome] = Mappa(valore, Sig6);
            else if (Prezzi.Contains(nome)) col[nome] = Mappa(valore, d => Math.Round(d, 2));
            else if (nome == "tracks" && valore is JsonArray tracks)
                foreach (var t in tracks)
                    if (t is JsonObject track) Arrotonda(track);
        }
    }

    private static JsonNode? Mappa(JsonNode? nodo, Func<double, double> f) => nodo switch
    {
        JsonArray a => new JsonArray(a.Select(x => Mappa(x, f)).ToArray()),
        JsonValue v when v.TryGetValue<double>(out var d) => JsonValue.Create(f(d)),
        JsonValue v when v.TryGetValue<long>(out var l) => JsonValue.Create(l),
        null => null,
        _ => nodo.DeepClone(),
    };

    public static double Sig6(double d)
    {
        if (d == 0 || !double.IsFinite(d)) return d;
        return double.Parse(d.ToString("G6", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
    }

    // ---------------------------------------------------------------- lettura

    /// <summary>Le righe di un giorno: prima il .gz (tutti i membri), poi il .jsonl in coda.</summary>
    public static IEnumerable<string> Righe(string percorso)
    {
        string gz = percorso + ".gz";
        if (File.Exists(gz))
        {
            using var file = new FileStream(gz, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var gzip = new GZipStream(file, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip, Encoding.UTF8);
            while (reader.ReadLine() is { } line)
                if (line.Length > 0) yield return line;
        }
        if (File.Exists(percorso))
        {
            using var file = new FileStream(percorso, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var reader = new StreamReader(file, Encoding.UTF8);
            while (reader.ReadLine() is { } line)
                if (line.Length > 0) yield return line;
        }
    }

    /// <summary>Una riga: la testa (senza colonne) e le colonne; null se troncata.</summary>
    public static (JsonObject testa, JsonArray colonne)? Leggi(string riga)
    {
        JsonObject? obj;
        try { obj = JsonNode.Parse(riga) as JsonObject; }
        catch { return null; }   // una riga troncata (lettore ucciso a meta') non ferma niente
        if (obj is null) return null;
        string prop = obj.ContainsKey("columns") ? "columns" : obj.ContainsKey("rows") ? "rows" : "";
        if (prop.Length == 0 || obj[prop] is not JsonArray colonne) return null;
        obj.Remove(prop);
        return (obj, colonne);
    }

    // ---------------------------------------------------------------- giornate chiuse

    /// <summary>Comprime le giornate chiuse: data di sessione prima di ieri e nessuna scrittura da 2 ore.</summary>
    public int ChiudiGiorni()
    {
        if (!Directory.Exists(Serie)) return 0;
        var limite = DateTime.UtcNow.Date.AddDays(-1);
        int chiuse = 0;
        foreach (var plain in Directory.EnumerateFiles(Serie, "*.jsonl", SearchOption.AllDirectories).ToList())
        {
            if (!DateTime.TryParseExact(Path.GetFileNameWithoutExtension(plain), "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var giorno)) continue;
            if (giorno >= limite) continue;
            if (DateTime.UtcNow - File.GetLastWriteTimeUtc(plain) < TimeSpan.FromHours(2)) continue;
            Comprimi(plain);
            _aperti.Remove(plain);
            chiuse++;
        }
        return chiuse;
    }

    /// <summary>Il .jsonl diventa un membro gzip in coda a .jsonl.gz, verificato prima di cancellarlo.</summary>
    public static void Comprimi(string plain)
    {
        string gz = plain + ".gz";
        string tmp = gz + ".tmp";
        int righe = 0;
        using (var output = new FileStream(tmp, FileMode.Create, FileAccess.Write))
        {
            if (File.Exists(gz))
                using (var vecchio = File.OpenRead(gz)) vecchio.CopyTo(output);
            using var gzip = new GZipStream(output, CompressionLevel.SmallestSize);
            using var input = new FileStream(plain, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            input.CopyTo(gzip);
        }
        foreach (var _ in File.ReadLines(plain)) righe++;

        // verifica: il .gz nuovo ha le righe del vecchio piu' quelle del .jsonl
        int attese = righe;
        if (File.Exists(gz)) attese += ContaGz(gz);
        int lette = ContaGz(tmp);
        if (lette != attese)
        {
            File.Delete(tmp);
            throw new InvalidDataException($"{plain}: compresso {lette} righe invece di {attese}");
        }
        File.Move(tmp, gz, overwrite: true);
        File.Delete(plain);
    }

    private static int ContaGz(string gz)
    {
        using var file = File.OpenRead(gz);
        using var gzip = new GZipStream(file, CompressionMode.Decompress);
        using var reader = new StreamReader(gzip);
        int n = 0;
        while (reader.ReadLine() is not null) n++;
        return n;
    }

    // ---------------------------------------------------------------- utilita'

    private static string Str(JsonNode? n) => n is JsonValue v && v.TryGetValue<string>(out var s) ? s : "";

    private static string Lungo(JsonNode? n)
    {
        if (n is JsonValue v)
        {
            if (v.TryGetValue<long>(out var l)) return l.ToString(CultureInfo.InvariantCulture);
            if (v.TryGetValue<double>(out var d)) return ((long)d).ToString(CultureInfo.InvariantCulture);
        }
        return "";
    }
}
