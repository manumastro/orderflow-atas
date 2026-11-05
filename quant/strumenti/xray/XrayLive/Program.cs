using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace XrayLive;

// X-Ray OptionsDepth, letto dal vivo.
//
// La cache del plugin (%APPDATA%\ATAS X\OptionsDataCache) e' cifrata (Cache.cs): qui la si
// legge chunk per chunk mentre ATAS la aggiorna, e la si pubblica in JSON (Archivio.cs).
//
//   XrayLive --watch [--interval 15] [--out DIR]   in ascolto
//   XrayLive --once  [--out DIR]                   un giro e basta
//   XrayLive --compatta [--out DIR]                riscrive l'archivio senza doppioni,
//                                                  arrotondato e con le giornate chiuse in .gz
//
//   --serve [--porta 8790] [--pagina DIR] [--bridge URL]
//       con --watch: il lettore e in piu' il server della pagina (Server/), su 127.0.0.1;
//       da solo: solo il server, su un archivio che un altro lettore scrive (o per le prove).
//
// Codici d'uscita: 0 bene, 1 errori di lettura, 3 plugin o cache assenti, 4 un altro lettore
// tiene gia' la cartella, 5 la porta del server e' occupata.
internal static class Program
{
    private static readonly JsonSerializerOptions Pretty = new() { WriteIndented = true };

    private static Mutex? _instance;
    private static byte[] _key = Array.Empty<byte>();
    private static string _plugin = "";
    private static string _cache = "";
    private static string _out = "";
    private static Archivio _archivio = null!;

    private static readonly HashSet<string> Seen = new(StringComparer.OrdinalIgnoreCase);
    private static readonly object Io = new();

    private static long _seq, _chunks, _errors;
    private static string? _lastError;
    private static string _lastChunk = "-";
    private static DateTime _lastChunkUtc = DateTime.MinValue;
    private static readonly DateTime Started = DateTime.UtcNow;
    private static DateTime _lastSweep = DateTime.MinValue;
    private static DateTime _lastClose = DateTime.MinValue;
    private static DateTime _lastMeasure = DateTime.MinValue;
    private static long _lastSweepMs, _archiveBytes;
    private static int _archiveDays;
    private static volatile bool _sweeping;
    private static volatile bool _sweepNow;

    private static int Main(string[] args)
    {
        bool watch = args.Contains("--watch");
        bool compact = args.Contains("--compatta");
        bool serve = args.Contains("--serve");
        int interval = ArgInt(args, "--interval", 15);

        string atas = Arg(args, "--atas") ?? @"C:\ATASX";
        _out = Path.GetFullPath(Arg(args, "--out") ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "orderflow-atas", "quant", "dati", "xray"));
        _archivio = new Archivio(_out);

        if (serve && !watch && !compact && !args.Contains("--once"))
            return SoloServer(args);

        // un solo processo per cartella: due appenderebbero le stesse righe ai file-giorno
        string guard = "Local\\XrayLive-" + Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(_out.ToLowerInvariant())))[..16];
        _instance = new Mutex(true, guard, out bool fresh);
        if (!fresh)
        {
            Console.Error.WriteLine($"XrayLive sta gia' girando su {_out}");
            return 4;
        }

        if (compact) return Compatta.Esegui(_out);

        _plugin = Arg(args, "--plugin") ?? Cache.FindPlugin(AppData()) ?? "";
        _cache = Arg(args, "--cache") ?? Path.Combine(AppData(), "OptionsDataCache");
        if (!File.Exists(_plugin))
        {
            Console.Error.WriteLine($"ERRORE: plugin X-Ray non trovato ({_plugin})");
            return 3;
        }
        if (!Directory.Exists(_cache))
        {
            Console.Error.WriteLine($"ERRORE: cache non trovata ({_cache})");
            return 3;
        }

        Directory.CreateDirectory(Path.Combine(_out, "latest"));
        Directory.CreateDirectory(_archivio.Serie);

        _key = Cache.DeriveKey(_plugin, atas);
        Console.WriteLine($"plugin : {_plugin}");
        Console.WriteLine($"cache  : {_cache}");
        Console.WriteLine($"out    : {_out}");
        Console.WriteLine($"key    : {Convert.ToHexString(SHA256.HashData(_key))[..16]}...");

        LoadLedger();
        Housekeeping(force: true);
        Sweep();

        if (!watch)
        {
            Console.WriteLine($"fatto: {_chunks} chunk letti, {_errors} errori");
            return _errors == 0 ? 0 : 1;
        }

        using var fsw = new FileSystemWatcher(_cache)
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
        };
        FileSystemEventHandler touch = (_, _) => _sweepNow = true;
        fsw.Created += touch;
        fsw.Changed += touch;
        fsw.Renamed += (_, _) => _sweepNow = true;
        fsw.EnableRaisingEvents = true;

        Server.Servitore? servitore = null;
        if (serve && (servitore = AvviaServer(args)) is null) return 5;

        var stop = false;
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; stop = true; };
        Console.WriteLine("in ascolto (Ctrl+C per uscire)");

        while (!stop)
        {
            Thread.Sleep(1000);
            if (!_sweepNow && (DateTime.UtcNow - _lastSweep).TotalSeconds < interval)
                continue;
            _sweepNow = false;
            Sweep();
            Housekeeping(force: false);
        }

        fsw.EnableRaisingEvents = false;
        servitore?.Ferma();
        WriteStatus();
        return 0;
    }

    // ---------------------------------------------------------------- server

    private static Server.Servitore? AvviaServer(string[] args)
    {
        int porta = ArgInt(args, "--porta", Server.Servitore.PortaPredefinita);
        var s = new Server.Servitore(_out, Server.Servitore.TrovaPagina(Arg(args, "--pagina")), porta,
            Arg(args, "--bridge") ?? "http://127.0.0.1:8787")
        {
            // --senza-raccolta: un secondo server (prove, confronti) non deve raccogliere le fonti una seconda volta
            Raccogli = !args.Contains("--senza-raccolta"),
        };
        try
        {
            s.Avvia();
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine($"ERRORE: il server non parte sulla porta {porta}: {ex.Message}");
            return null;
        }
        Console.WriteLine($"pagina : {s.Indirizzo}");
        return s;
    }

    private static int SoloServer(string[] args)
    {
        var s = AvviaServer(args);
        if (s is null) return 5;
        Console.WriteLine($"solo server su {_out} (Ctrl+C per uscire)");
        using var fine = new ManualResetEventSlim();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; fine.Set(); };
        fine.Wait();
        s.Ferma();
        return 0;
    }

    // ---------------------------------------------------------------- giro

    private static void Sweep()
    {
        _lastSweep = DateTime.UtcNow;
        _sweeping = true;
        WriteStatus();       // lo stato e' vivo anche mentre un giro lungo e' in corso
        var files = Directory.EnumerateFiles(_cache, "*", SearchOption.AllDirectories).ToList();
        int done = 0;
        long written = 0;
        foreach (var file in files)
        {
            if (Seen.Contains(file)) continue;
            try
            {
                var (chunk, rawBytes, bodyBytes) = Cache.Read(file, _key);
                written += Publish(chunk, file, rawBytes, bodyBytes);
                Seen.Add(file);
                done++;
                if (done % 200 == 0) WriteStatus();
            }
            catch (FileNotFoundException)
            {
                // il plugin scrive con file temporaneo e rinomina: il nome puo' sparire
                // fra l'elenco della cartella e la lettura. Non e' un errore: si riprovera'.
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (Exception ex)
            {
                _errors++;
                _lastError = $"{Path.GetFileName(file)}: {ex.GetType().Name} {ex.Message}";
                AppendLedger(_seq++, "ERR", $"-\t-\t-\t-\t-\t0\t0\t{file}\t{_lastError}");
                // si riprova al prossimo avvio (LoadLedger salta solo gli "ok"), non a ogni giro:
                // con una chiave sbagliata, segnarli per sempre perderebbe tutta la cache
                Seen.Add(file);
            }
        }

        _sweeping = false;
        _lastSweepMs = (long)(DateTime.UtcNow - _lastSweep).TotalMilliseconds;
        WriteStatus();
        if (done > 0)
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] +{done} chunk, {written} colonne nuove, in {_lastSweepMs} ms " +
                              $"({_chunks} totali, {_errors} errori)");
    }

    /// <summary>Ogni 30 minuti comprime le giornate chiuse; ogni 10 misura l'archivio.</summary>
    private static void Housekeeping(bool force)
    {
        if (force || DateTime.UtcNow - _lastClose > TimeSpan.FromMinutes(30))
        {
            _lastClose = DateTime.UtcNow;
            try
            {
                int closed = _archivio.ChiudiGiorni();
                if (closed > 0) Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {closed} giornate chiuse compresse");
            }
            catch (Exception ex)
            {
                _lastError = "compressione: " + ex.Message;
                Console.Error.WriteLine(_lastError);
            }
        }
        if (force || DateTime.UtcNow - _lastMeasure > TimeSpan.FromMinutes(10))
        {
            _lastMeasure = DateTime.UtcNow;
            try
            {
                var files = Directory.EnumerateFiles(_archivio.Serie, "*", SearchOption.AllDirectories)
                    .Select(f => new FileInfo(f)).ToList();
                _archiveBytes = files.Sum(f => f.Length);
                _archiveDays = files.Select(f => f.Name.Length >= 10 ? f.Name[..10] : "").Distinct().Count();
            }
            catch
            {
                // la misura non deve fermare il lettore
            }
        }
    }

    // ---------------------------------------------------------------- pubblicazione

    private static int Publish(Chunk c, string file, int rawBytes, int bodyBytes)
    {
        string slug = Slug(c.Famiglia);
        string latest = Path.Combine(_out, "latest", slug + ".json");
        string dayPath = Path.Combine(_archivio.Serie, Slug(c.Ticker.Length > 0 ? c.Ticker : "unknown"), slug,
            (c.Date.Length > 0 ? c.Date : "unknown") + ".jsonl");

        int written = 0;
        lock (Io)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dayPath)!);

            // "latest": l'ultimo chunk intero, arrotondato, compatto, con l'ultima colonna a parte
            if (c.Day["columns"] is JsonArray columns)
            {
                foreach (var col in columns)
                    if (col is JsonObject o) Archivio.Arrotonda(o);
                var head = (JsonObject)c.Day.DeepClone();
                head.Remove("columns");
                written = _archivio.Accoda(dayPath, head, columns);
                if (columns.Count > 0) c.Day["last"] = columns[^1]?.DeepClone();
            }
            WriteAtomic(latest, c.Day.ToJsonString());

            AppendLedger(_seq, "ok", $"{c.Kind}\t{c.Metric}\t{c.Ticker}\t{c.Date}\t{c.Variant}\t{rawBytes}\t{bodyBytes}\t{file}");

            var line = new JsonObject
            {
                ["seq"] = _seq,
                ["seenUtc"] = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture),
                ["kind"] = c.Kind,
                ["metric"] = c.Metric,
                ["variant"] = c.Variant,
                ["family"] = slug,
                ["ticker"] = c.Ticker,
                ["date"] = c.Date,
                ["file"] = Path.GetFileName(file),
                ["columns"] = (c.Day["columns"] as JsonArray)?.Count ?? 0,
                ["written"] = written,
            };
            AppendShared(Path.Combine(_out, "journal.jsonl"), line.ToJsonString() + "\n");

            _chunks++;
            _seq++;
            _lastChunk = $"{slug} {c.Ticker} {c.Date}";
            _lastChunkUtc = DateTime.UtcNow;
        }
        return written;
    }

    // ---------------------------------------------------------------- file di contorno

    private static void LoadLedger()
    {
        string index = Path.Combine(_out, "index.tsv");
        if (!File.Exists(index)) return;
        foreach (var line in File.ReadLines(index))
        {
            var parts = line.Split('\t');
            // seq  stato  tipo  metrica  ticker  data  variante  byte  byteBody  file
            if (parts.Length < 10) continue;
            if (!long.TryParse(parts[0], out var seq)) continue;
            _seq = Math.Max(_seq, seq);
            if (parts[1] != "ok") continue;
            _chunks++;
            Seen.Add(parts[9]);
        }
        _seq++;
    }

    private static void AppendLedger(long seq, string status, string rest)
    {
        string index = Path.Combine(_out, "index.tsv");
        if (!File.Exists(index))
            AppendShared(index, "seq\tstatus\tkind\tmetric\tticker\tdate\tvariant\traw\tbody\tfile\n");
        AppendShared(index, $"{seq}\t{status}\t{rest}\n");
    }

    private static void WriteStatus()
    {
        var status = new JsonObject
        {
            ["alive"] = true,
            ["pid"] = Environment.ProcessId,
            ["startedUtc"] = Started.ToString("O", CultureInfo.InvariantCulture),
            ["lastSweepUtc"] = _lastSweep.ToString("O", CultureInfo.InvariantCulture),
            ["sweeping"] = _sweeping,
            ["lastSweepMs"] = _lastSweepMs,
            ["workingSetMB"] = Math.Round(Environment.WorkingSet / 1048576.0),
            ["chunks"] = _chunks,
            ["errors"] = _errors,
            ["lastError"] = _lastError,
            ["lastChunk"] = _lastChunk,
            ["lastChunkUtc"] = _lastChunkUtc == DateTime.MinValue ? null
                : _lastChunkUtc.ToString("O", CultureInfo.InvariantCulture),
            ["columnsWritten"] = _archivio.Scritte,
            ["columnsDuplicate"] = _archivio.Doppie,
            ["columnsRevised"] = _archivio.Revisioni,
            ["archiveMB"] = Math.Round(_archiveBytes / 1048576.0, 1),
            ["archiveDays"] = _archiveDays,
            ["plugin"] = _plugin,
            ["cache"] = _cache,
            ["out"] = _out,
            ["keySha256"] = _key.Length == 0 ? null : Convert.ToHexString(SHA256.HashData(_key))[..16],
        };
        try
        {
            WriteAtomic(Path.Combine(_out, "status.json"), status.ToJsonString(Pretty));
        }
        catch
        {
            // la scrittura dello stato non deve fermare il lettore
        }
    }

    // ---------------------------------------------------------------- utilita'

    /// <summary>Append con condivisione completa e qualche ritentativo: un lettore esterno
    /// (la GUI) non deve far perdere un chunk.</summary>
    public static void AppendShared(string path, string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        for (int attempt = 0; ; attempt++)
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Append, FileAccess.Write,
                    FileShare.ReadWrite | FileShare.Delete);
                stream.Write(bytes, 0, bytes.Length);
                return;
            }
            catch (IOException) when (attempt < 20)
            {
                Thread.Sleep(50);
            }
        }
    }

    /// <summary>Scrive in un file temporaneo e lo rinomina: chi legge non vede mai un JSON a meta'.</summary>
    private static void WriteAtomic(string path, string text)
    {
        string tmp = path + ".tmp";
        File.WriteAllText(tmp, text);
        for (int attempt = 0; ; attempt++)
        {
            try
            {
                File.Move(tmp, path, overwrite: true);
                return;
            }
            catch (IOException) when (attempt < 20)
            {
                Thread.Sleep(50);
            }
            catch (UnauthorizedAccessException) when (attempt < 20)
            {
                Thread.Sleep(50);
            }
        }
    }

    public static string Slug(string name)
    {
        var sb = new StringBuilder(name.Length);
        foreach (var ch in name)
            sb.Append(char.IsLetterOrDigit(ch) || ch is '_' or '-' or '.' ? ch : '_');
        return sb.ToString();
    }

    private static string AppData() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ATAS X");

    private static string? Arg(string[] args, string name)
    {
        int i = Array.IndexOf(args, name);
        return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
    }

    private static int ArgInt(string[] args, string name, int fallback)
        => int.TryParse(Arg(args, name), out var v) ? v : fallback;
}
