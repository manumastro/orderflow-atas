using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Quant.Dati;

/// <summary>
/// Dove stanno i dati. Un file binario per mese e per tipo:
/// <c>quant/dati/NQ/m1/2026-10.bin</c>. Intestazione di 24 byte, poi i record cosi' come stanno in
/// memoria: leggere un anno di barre a un minuto e' una copia, non un parsing.
///
/// I dati non vanno in git: sono di ATAS, pesano, e si riscaricano.
/// </summary>
public static class Archivio
{
    private static readonly byte[] Magia = "QDAT"u8.ToArray();
    private const int Versione = 1;
    private const int Intestazione = 24;

    public static string Radice
    {
        get
        {
            var env = Environment.GetEnvironmentVariable("QUANT_DATI");
            return string.IsNullOrEmpty(env) ? Path.Combine(Percorsi.Quant, "dati") : env;
        }
    }

    public static string Cartella(string simbolo, string tipo) => Path.Combine(Radice, simbolo.ToUpperInvariant(), tipo);

    /// <summary>
    /// Scrive i record, fondendoli con quelli gia' presenti nei mesi toccati. A parita' di tempo
    /// vince il nuovo: riscaricare un periodo corregge, non duplica.
    /// </summary>
    public static int Scrivi<T>(string simbolo, string tipo, ReadOnlySpan<T> nuovi, Func<T, long> tempo) where T : unmanaged
    {
        if (nuovi.IsEmpty) return 0;
        var cartella = Cartella(simbolo, tipo);
        Directory.CreateDirectory(cartella);
        var perMese = new SortedDictionary<string, List<T>>(StringComparer.Ordinal);
        foreach (var r in nuovi)
        {
            var mese = Tempo.Utc(tempo(r)).ToString("yyyy-MM");
            if (!perMese.TryGetValue(mese, out var lista)) perMese[mese] = lista = [];
            lista.Add(r);
        }
        var scritti = 0;
        foreach (var (mese, lista) in perMese)
        {
            var file = Path.Combine(cartella, mese + ".bin");
            var tutti = new SortedDictionary<long, T>();
            if (File.Exists(file))
                foreach (var r in LeggiFile<T>(file)) tutti[tempo(r)] = r;
            foreach (var r in lista) tutti[tempo(r)] = r;
            var arr = tutti.Values.ToArray();
            ScriviFile(file, arr);
            scritti += lista.Count;
        }
        return scritti;
    }

    /// <summary>
    /// Scrive un file intero, senza fondere: per i tipi dove piu' record condividono lo stesso
    /// tempo (gli scambi nello stesso millisecondo).
    /// </summary>
    public static void ScriviMese<T>(string simbolo, string tipo, string mese, ReadOnlySpan<T> record) where T : unmanaged
    {
        var cartella = Cartella(simbolo, tipo);
        Directory.CreateDirectory(cartella);
        ScriviFile(Path.Combine(cartella, mese + ".bin"), record);
    }

    /// <summary>I record fra da e a (UTC, a escluso), letti solo dai mesi che li contengono.</summary>
    public static T[] Leggi<T>(string simbolo, string tipo, DateTime? da = null, DateTime? a = null, Func<T, long>? tempo = null) where T : unmanaged
    {
        var cartella = Cartella(simbolo, tipo);
        if (!Directory.Exists(cartella)) return [];
        var mesi = Directory.GetFiles(cartella, "*.bin").OrderBy(f => f, StringComparer.Ordinal).ToList();
        var parti = new List<T[]>();
        foreach (var f in mesi)
        {
            var nome = Path.GetFileNameWithoutExtension(f);
            if (!DateTime.TryParseExact(nome + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var inizioMese)) continue;
            if (da is { } d && inizioMese.AddMonths(1) <= d) continue;
            if (a is { } x && inizioMese >= x) continue;
            parti.Add(LeggiFile<T>(f));
        }
        var tutti = parti.SelectMany(p => p);
        if (tempo is not null && (da is not null || a is not null))
        {
            var dms = da is { } d1 ? Tempo.Ms(d1) : long.MinValue;
            var ams = a is { } a1 ? Tempo.Ms(a1) : long.MaxValue;
            tutti = tutti.Where(r => tempo(r) >= dms && tempo(r) < ams);
        }
        return [.. tutti];
    }

    public static Serie Serie(string simbolo, string tipo = "m1", DateTime? da = null, DateTime? a = null, bool rettifica = true)
    {
        var barre = Leggi<Barra>(simbolo, tipo, da, a, b => b.Inizio);
        var roll = LeggiRollover(simbolo);
        if (barre.Length > 0)
            roll = [.. roll.Where(r => r.Tempo > barre[0].Inizio && r.Tempo <= barre[^1].Inizio)];
        return new Serie(Strumento.Da(simbolo), barre, $"{simbolo.ToUpperInvariant()} {tipo}", roll, rettifica);
    }

    public static Rollover[] LeggiRollover(string simbolo)
    {
        var file = Path.Combine(Radice, simbolo.ToUpperInvariant(), "rollover.json");
        return File.Exists(file) ? JsonSerializer.Deserialize<Rollover[]>(File.ReadAllText(file)) ?? [] : [];
    }

    public static void ScriviRollover(string simbolo, IEnumerable<Rollover> rollover)
    {
        var file = Path.Combine(Radice, simbolo.ToUpperInvariant(), "rollover.json");
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, JsonSerializer.Serialize(rollover.OrderBy(r => r.Tempo).ToArray(), new JsonSerializerOptions { WriteIndented = true }));
    }

    /// <summary>Cosa c'e' in archivio: per simbolo e tipo, mesi, record e intervallo.</summary>
    public static IEnumerable<string> Inventario()
    {
        if (!Directory.Exists(Radice)) yield break;
        foreach (var sim in Directory.GetDirectories(Radice).OrderBy(x => x))
        foreach (var tipo in Directory.GetDirectories(sim).OrderBy(x => x))
        {
            var files = Directory.GetFiles(tipo, "*.bin").OrderBy(x => x).ToArray();
            if (files.Length == 0) continue;
            long record = 0, byteTot = 0;
            foreach (var f in files)
            {
                using var s = File.OpenRead(f);
                var (_, n) = LeggiIntestazione(s, f);
                record += n;
                byteTot += s.Length;
            }
            yield return $"{Path.GetFileName(sim),-5} {Path.GetFileName(tipo),-8} {files.Length,4} mesi  {record,14:N0} record  {byteTot / 1048576.0,9:N1} MB  " +
                         $"{Path.GetFileNameWithoutExtension(files[0])} -> {Path.GetFileNameWithoutExtension(files[^1])}";
        }
    }

    private static void ScriviFile<T>(string file, ReadOnlySpan<T> record) where T : unmanaged
    {
        var tmp = file + ".tmp";
        using (var s = File.Create(tmp))
        {
            Span<byte> testa = stackalloc byte[Intestazione];
            Magia.CopyTo(testa);
            BitConverter.TryWriteBytes(testa[4..], Versione);
            BitConverter.TryWriteBytes(testa[8..], Marshal.SizeOf<T>());
            BitConverter.TryWriteBytes(testa[12..], (long)record.Length);
            s.Write(testa);
            s.Write(MemoryMarshal.AsBytes(record));
        }
        File.Move(tmp, file, overwrite: true);
    }

    private static (int dim, long n) LeggiIntestazione(Stream s, string file)
    {
        Span<byte> testa = stackalloc byte[Intestazione];
        s.ReadExactly(testa);
        if (!testa[..4].SequenceEqual(Magia)) throw new InvalidDataException($"{file}: non e' un file dell'archivio");
        var versione = BitConverter.ToInt32(testa[4..]);
        if (versione != Versione) throw new InvalidDataException($"{file}: versione {versione}, attesa {Versione}");
        return (BitConverter.ToInt32(testa[8..]), BitConverter.ToInt64(testa[12..]));
    }

    private static T[] LeggiFile<T>(string file) where T : unmanaged
    {
        using var s = File.OpenRead(file);
        var (dim, n) = LeggiIntestazione(s, file);
        if (dim != Marshal.SizeOf<T>())
            throw new InvalidDataException($"{file}: record da {dim} byte, {typeof(T).Name} ne ha {Marshal.SizeOf<T>()}");
        var arr = new T[n];
        s.ReadExactly(MemoryMarshal.AsBytes(arr.AsSpan()));
        return arr;
    }
}

/// <summary>Le cartelle del framework, trovate risalendo fino a <c>quant/src/Quant.slnx</c>.</summary>
public static class Percorsi
{
    private static string? _quant;

    public static string Quant => _quant ??= Trova();

    /// <summary>QUANT_REGISTRO e QUANT_RISULTATI li spostano: i test scrivono in una cartella temporanea, mai nel registro vero.</summary>
    public static string Registro => Environment.GetEnvironmentVariable("QUANT_REGISTRO") is { Length: > 0 } r ? r : Path.Combine(Quant, "registro");
    public static string Risultati => Environment.GetEnvironmentVariable("QUANT_RISULTATI") is { Length: > 0 } r ? r : Path.Combine(Quant, "risultati");

    private static string Trova()
    {
        foreach (var inizio in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var d = new DirectoryInfo(inizio);
            while (d is not null)
            {
                if (File.Exists(Path.Combine(d.FullName, "quant", "src", "Quant.slnx"))) return Path.Combine(d.FullName, "quant");
                if (File.Exists(Path.Combine(d.FullName, "src", "Quant.slnx"))) return d.FullName;
                d = d.Parent;
            }
        }
        throw new DirectoryNotFoundException("cartella quant/ non trovata: lancia il comando da dentro il repository");
    }
}
