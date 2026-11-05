using System.Buffers.Binary;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace XrayLive;

// La cache del plugin, dal file cifrato al chunk in JSON.
//
// Formato del file: "ODC1" | IV(16) | AES-CBC/PKCS7, chiave
// HKDF-SHA256(MachineGuid + sep + UserName, salt/info risolti dal plugin); il nome del file e'
// lo SHA-256 del contenuto in chiaro. Dentro: gzip, poi MessagePack, con i valori per strike in
// float32 planarizzato (prima tutti i byte bassi, poi i secondi...). Gli schemi dei chunk
// (Plane/Strikes/Levels/Pressure) sono in docs/ATASX_XRAY_CACHE.md del repo ATAS_Tool.
internal sealed class Chunk
{
    public string Kind = "raw";
    public string Ticker = "";
    public string Metric = "raw";
    public string Variant = "";
    public string Date = "";
    public string Session = "";
    public double AsOf;
    /// <summary>La testa (type, ticker, metric...) e le colonne, in "columns".</summary>
    public JsonObject Day = new();

    public string Famiglia => Metric + (Variant.Length > 0 ? "__" + Variant : "");
}

internal static class Cache
{
    private static readonly byte[] Magic = "ODC1"u8.ToArray();
    private static readonly byte[] Salt = Encoding.UTF8.GetBytes("ATAS.OptionsDataCache");
    private static readonly byte[] Info = Encoding.UTF8.GetBytes("chunk-cache-v1");

    // ---------------------------------------------------------------- chiave

    public static byte[] DeriveKey(string pluginPath, string atasDir)
    {
        AssemblyLoadContext.Default.Resolving += (ctx, name) =>
        {
            foreach (var dir in new[] { atasDir, Path.GetDirectoryName(pluginPath)! })
            {
                var candidate = Path.Combine(dir, name.Name + ".dll");
                if (File.Exists(candidate))
                    return ctx.LoadFromAssemblyPath(candidate);
            }
            return null;
        };

        var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(pluginPath));
        var resolver = SafeTypes(asm)
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.ReturnType == typeof(string)
                            && m.GetParameters().Length == 1
                            && m.GetParameters()[0].ParameterType == typeof(int)))
            .FirstOrDefault() ?? throw new InvalidOperationException("decifratore di stringhe non trovato");

        string Str(int id) => (string)resolver.Invoke(null, new object[] { id })!;

        string sep = Str(-1050272067);
        string regKey = Str(-1050273489);
        string regValue = Str(-1050273535);

        string machineId = ReadHklm(regKey, regValue) ?? Environment.MachineName;
        return HKDF.DeriveKey(HashAlgorithmName.SHA256,
            Encoding.UTF8.GetBytes(machineId + sep + Environment.UserName), 32, Salt, Info);
    }

    public static string? FindPlugin(string appData)
    {
        string dir = Path.Combine(appData, "Indicators");
        if (!Directory.Exists(dir)) return null;
        foreach (var dll in Directory.EnumerateFiles(dir, "*.dll"))
        {
            try
            {
                using var stream = File.OpenRead(dll);
                var buffer = new byte[Math.Min(stream.Length, 8 * 1024 * 1024)];
                int read = stream.Read(buffer);
                if (buffer.AsSpan(0, read).IndexOf("ODC1"u8) >= 0)
                    return dll;
            }
            catch
            {
                // un dll bloccato non e' il nostro
            }
        }
        return null;
    }

    // ---------------------------------------------------------------- un file

    /// <summary>Decifra, verifica lo SHA-256, scompatta e decodifica un file della cache.</summary>
    public static (Chunk chunk, int rawBytes, int bodyBytes) Read(string file, byte[] key)
    {
        byte[] raw = File.ReadAllBytes(file);
        if (raw.Length <= 20 || !raw.AsSpan(0, Magic.Length).SequenceEqual(Magic))
            throw new InvalidDataException("non ODC1");

        byte[] plain;
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            plain = aes.DecryptCbc(raw.AsSpan(Magic.Length + 16), raw.AsSpan(Magic.Length, 16));
        }

        string digest = Convert.ToHexString(SHA256.HashData(plain)).ToLowerInvariant();
        if (!string.Equals(digest, Path.GetFileName(file), StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("hash diverso dal nome (chiave sbagliata?)");

        byte[] body = plain.Length > 2 && plain[0] == 0x1F && plain[1] == 0x8B ? Gunzip(plain) : plain;
        int cursor = 0;
        object? msg = MsgPack.Read(body, ref cursor);
        return (Classify(msg), raw.Length, body.Length);
    }

    // ---------------------------------------------------------------- classificazione

    public static Chunk Classify(object? msg)
    {
        var c = new Chunk();
        if (msg is not List<object?> v || v.Count < 6)
        {
            c.Day["type"] = "raw";
            c.Day["value"] = ToJson(msg);
            return c;
        }

        c.Ticker = v[1] as string ?? "";
        c.Session = v[2] as string ?? "";
        c.Date = c.Session.Length >= 10 ? c.Session[..10] : c.Session;

        // PlaneChunk: schema, ticker, session, kind, greek, prices[], simUnixMs[], asOf, packing, values[]
        if (v.Count >= 10 && v[9] is byte[] values && v[5] is List<object?> prices && v[6] is List<object?> times)
        {
            c.Kind = "plane";
            c.Metric = v[3] as string ?? "plane";
            c.Variant = v[4] as string ?? "";
            c.AsOf = Num(v[7]);
            int np = prices.Count, nt = times.Count;
            var cols = new JsonArray();
            long n3 = (long)np * nt;

            if (values.Length == n3 * 4 && Num(v[8]) == 1)
            {
                for (int j = 0; j < nt; j++)
                {
                    var vals = new JsonArray();
                    for (int k = 0; k < np; k++)
                    {
                        long m = (long)j * np + k;
                        int bits = values[m]
                                   | values[n3 + m] << 8
                                   | values[2 * n3 + m] << 16
                                   | values[3 * n3 + m] << 24;
                        float f = BitConverter.Int32BitsToSingle(bits);
                        vals.Add(float.IsFinite(f) ? JsonValue.Create((double)f) : null);
                    }
                    cols.Add(new JsonObject
                    {
                        ["ts"] = Num(times[j]),
                        ["prices"] = ToJson(prices),
                        ["values"] = vals,
                    });
                }
            }
            else
            {
                cols.Add(new JsonObject
                {
                    ["ts"] = Num(v[7]),
                    ["prices"] = ToJson(prices),
                    ["values"] = new JsonObject
                    {
                        ["error"] = $"packing {Num(v[8])}, {values.Length} byte per {n3} valori",
                        ["hex"] = Convert.ToHexString(values.AsSpan(0, Math.Min(values.Length, 64))),
                    },
                });
            }

            Head(c, "plane");
            c.Day["greek"] = c.Variant;
            c.Day["asOf"] = c.AsOf;
            c.Day["columns"] = cols;
            return c;
        }

        // StrikesChunk: schema, ticker, session, metric, cohort, scope, minutes[]
        // minute = schema, asOf, spot, strikes[], call[], put[], dte, mixed, expiry, from
        if (v.Count == 7 && v[6] is List<object?> rows && rows.Count > 0
            && rows[0] is List<object?> r0 && r0.Count == 10)
        {
            c.Kind = "strikes";
            c.Metric = v[3] as string ?? "strikes";
            c.Variant = string.Join("_", new[] { v[4] as string, v[5] as string }.Where(s => !string.IsNullOrEmpty(s)));
            var cols = new JsonArray();
            foreach (var row in rows)
            {
                if (row is not List<object?> r || r.Count < 10) continue;
                cols.Add(new JsonObject
                {
                    ["ts"] = Num(r[1]),
                    ["spot"] = Num(r[2]),
                    ["strikes"] = ToJson(r[3]),
                    ["call"] = ToJson(r[4]),
                    ["put"] = ToJson(r[5]),
                    ["dte"] = Num(r[6]),
                    ["mixed"] = r[7] is bool b && b,
                    ["expiry"] = Num(r[8]),
                    ["from"] = Num(r[9]),
                });
            }

            Head(c, "strikes");
            c.Day["cohort"] = v[4] as string ?? "";
            c.Day["scope"] = v[5] as string ?? "";
            c.Day["columns"] = cols;
            return c;
        }

        // LevelsChunk: schema, ticker, session, greek, tracer, observed(surface), outlook(surface)
        if (v.Count == 7
            && (v[5] is null || v[5] is List<object?> s5 && s5.Count == 5)
            && (v[6] is null || v[6] is List<object?> s6 && s6.Count == 5))
        {
            c.Kind = "levels";
            c.Metric = "levels";
            c.Variant = v[3] as string ?? "";
            var cols = new JsonArray();
            if (Surface(v[5], "observed") is { } observed) cols.Add(observed);
            if (Surface(v[6], "outlook") is { } outlook) cols.Add(outlook);

            Head(c, "levels");
            c.Day["greek"] = c.Variant;
            c.Day["tracer"] = ToJson(v[4]);
            c.Day["columns"] = cols;
            return c;
        }

        // PressureChunk (Hedge Pressure): schema, ticker, session, scope, asOfUnixMs[], spot[], delta[], trades[], closeUnixMs
        if (v.Count == 9 && v[4] is List<object?> pTs && v[5] is List<object?> pSpot
            && v[6] is List<object?> pDelta && v[7] is List<object?> pTrades && pTs.Count == pSpot.Count)
        {
            c.Kind = "pressure";
            c.Metric = "pressure";
            c.Variant = v[3] as string ?? "";
            var cols = new JsonArray();
            for (int k = 0; k < pTs.Count; k++)
            {
                cols.Add(new JsonObject
                {
                    ["ts"] = Num(pTs[k]),
                    ["spot"] = k < pSpot.Count ? Num(pSpot[k]) : 0,
                    ["delta"] = k < pDelta.Count ? Num(pDelta[k]) : 0,
                    ["trades"] = k < pTrades.Count ? Num(pTrades[k]) : 0,
                });
            }

            Head(c, "pressure");
            c.Day["scope"] = c.Variant;
            c.Day["closeUnixMs"] = Num(v[8]);
            c.Day["columns"] = cols;
            return c;
        }

        // seriesmap: schema, ticker, session, metric, prices[], entries[[from, to, values[], ...]]
        if (v.Count == 6 && v[4] is List<object?> px && v[5] is List<object?> entries
            && entries.Count > 0 && entries[0] is List<object?> e0 && e0.Count >= 3)
        {
            c.Kind = "seriesmap";
            c.Metric = v[3] as string ?? "seriesmap";
            var cols = new JsonArray();
            foreach (var entry in entries)
            {
                if (entry is not List<object?> e || e.Count < 3) continue;
                cols.Add(new JsonObject
                {
                    ["ts"] = Num(e[0]),
                    ["to"] = Num(e[1]),
                    ["prices"] = ToJson(px),
                    ["values"] = ToJson(e[2]),
                });
            }

            Head(c, "seriesmap");
            c.Day["columns"] = cols;
            return c;
        }

        c.Kind = "raw";
        c.Metric = (v[3] as string) ?? "raw";
        Head(c, "raw");
        c.Day["value"] = ToJson(v);
        return c;
    }

    private static void Head(Chunk c, string kind)
    {
        c.Day["type"] = kind;
        c.Day["ticker"] = c.Ticker;
        c.Day["metric"] = c.Metric;
        if (c.Variant.Length > 0) c.Day["variant"] = c.Variant;
        if (c.Session.Length > 0) c.Day["session"] = c.Session;
    }

    private static JsonObject? Surface(object? node, string name)
    {
        if (node is not List<object?> s || s.Count < 5) return null;
        var tracks = new JsonArray();
        if (s[4] is List<object?> tl)
        {
            foreach (var track in tl)
            {
                if (track is not List<object?> t || t.Count < 5) continue;
                tracks.Add(new JsonObject
                {
                    ["kind"] = Num(t[0]),
                    ["first"] = Num(t[1]),
                    ["price"] = ToJson(t[2]),
                    ["row"] = ToJson(t[3]),
                    ["lower"] = ToJson(t[4]),
                    ["upper"] = t.Count > 5 ? ToJson(t[5]) : new JsonArray(),
                    ["weight"] = t.Count > 6 ? ToJson(t[6]) : new JsonArray(),
                });
            }
        }

        var times = s[1] as List<object?> ?? new List<object?>();
        return new JsonObject
        {
            ["surface"] = name,
            ["ts"] = times.Count > 0 ? Num(times[0]) : 0,
            ["until"] = Num(s[2]),
            ["spanMs"] = Num(s[3]),
            ["prices"] = ToJson(s[0]),
            ["tracks"] = tracks,
        };
    }

    // ---------------------------------------------------------------- utilita'

    public static JsonNode? ToJson(object? value) => value switch
    {
        null => null,
        string s => JsonValue.Create(s),
        bool b => JsonValue.Create(b),
        double d => double.IsFinite(d) ? JsonValue.Create(d) : null,
        long l => JsonValue.Create(l),
        byte[] bytes => JsonValue.Create(Convert.ToHexString(bytes)),
        List<object?> list => new JsonArray(list.Select(ToJson).ToArray()),
        Dictionary<string, object?> map => new JsonObject(
            map.ToDictionary(kv => kv.Key, kv => ToJson(kv.Value))),
        _ => JsonValue.Create(value.ToString()),
    };

    public static double Num(object? value) => value switch
    {
        double d => d,
        float f => f,
        long l => l,
        int i => i,
        short s => s,
        byte b => b,
        _ => 0,
    };

    private static byte[] Gunzip(byte[] input)
    {
        using var source = new MemoryStream(input);
        using var gzip = new GZipStream(source, CompressionMode.Decompress);
        using var target = new MemoryStream();
        gzip.CopyTo(target);
        return target.ToArray();
    }

    private static string? ReadHklm(string keyPath, string valueName)
    {
        try
        {
            var psi = new ProcessStartInfo("reg")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            psi.ArgumentList.Add("query");
            psi.ArgumentList.Add("HKLM\\" + keyPath);
            psi.ArgumentList.Add("/v");
            psi.ArgumentList.Add(valueName);

            using var p = System.Diagnostics.Process.Start(psi)!;
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            if (p.ExitCode != 0) return null;

            foreach (var line in output.Split('\n'))
            {
                var trimmed = line.Trim();
                if (!trimmed.StartsWith(valueName, StringComparison.OrdinalIgnoreCase)) continue;
                var fields = trimmed.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
                if (fields.Length >= 2) return fields[^1];
            }
        }
        catch
        {
            // si ripiega sul nome macchina
        }
        return null;
    }

    private static IEnumerable<Type> SafeTypes(Assembly a)
    {
        try { return a.GetTypes(); }
        catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t is not null)!; }
    }

    // ---------------------------------------------------------------- msgpack

    private static class MsgPack
    {
        public static object? Read(byte[] b, ref int i)
        {
            byte c = b[i++];
            if (c <= 0x7F) return (long)c;
            if (c >= 0xE0) return (long)(sbyte)c;
            if (c <= 0x8F) return ReadMap(b, ref i, c & 0x0F);
            if (c <= 0x9F) return ReadArray(b, ref i, c & 0x0F);
            if (c <= 0xBF) return Str(b, ref i, c & 0x1F);

            switch (c)
            {
                case 0xC0: return null;
                case 0xC2: return false;
                case 0xC3: return true;
                case 0xC4: return Bin(b, ref i, (int)U(b, ref i, 1));
                case 0xC5: return Bin(b, ref i, (int)U(b, ref i, 2));
                case 0xC6: return Bin(b, ref i, (int)U(b, ref i, 4));
                case 0xCA:
                {
                    float f = BinaryPrimitives.ReadSingleBigEndian(b.AsSpan(i, 4));
                    i += 4;
                    return (double)f;
                }
                case 0xCB:
                {
                    double d = BinaryPrimitives.ReadDoubleBigEndian(b.AsSpan(i, 8));
                    i += 8;
                    return d;
                }
                case 0xCC: return (long)U(b, ref i, 1);
                case 0xCD: return (long)U(b, ref i, 2);
                case 0xCE: return (long)U(b, ref i, 4);
                case 0xCF: return (long)U(b, ref i, 8);
                case 0xD0: return (long)(sbyte)U(b, ref i, 1);
                case 0xD1: return (long)(short)U(b, ref i, 2);
                case 0xD2: return (long)(int)U(b, ref i, 4);
                case 0xD3: return (long)U(b, ref i, 8);
                case 0xD9: return Str(b, ref i, (int)U(b, ref i, 1));
                case 0xDA: return Str(b, ref i, (int)U(b, ref i, 2));
                case 0xDB: return Str(b, ref i, (int)U(b, ref i, 4));
                case 0xDC: return ReadArray(b, ref i, (int)U(b, ref i, 2));
                case 0xDD: return ReadArray(b, ref i, (int)U(b, ref i, 4));
                case 0xDE: return ReadMap(b, ref i, (int)U(b, ref i, 2));
                case 0xDF: return ReadMap(b, ref i, (int)U(b, ref i, 4));
                default:
                    // ext (0xD4-0xD8, 0xC7-0xC9) e timestamp: letti come null
                    if (c is >= 0xD4 and <= 0xD8) { i += 1 + (1 << (c - 0xD4)); return null; }
                    if (c == 0xC7) { int n = (int)U(b, ref i, 1); i += 1 + n; return null; }
                    if (c == 0xC8) { int n = (int)U(b, ref i, 2); i += 1 + n; return null; }
                    if (c == 0xC9) { int n = (int)U(b, ref i, 4); i += 1 + n; return null; }
                    throw new InvalidDataException($"msgpack 0x{c:X2} a {i - 1}");
            }
        }

        private static ulong U(byte[] b, ref int i, int n)
        {
            ulong v = 0;
            for (int k = 0; k < n; k++) v = (v << 8) | b[i++];
            return v;
        }

        private static string Str(byte[] b, ref int i, int n)
        {
            var s = Encoding.UTF8.GetString(b, i, n);
            i += n;
            return s;
        }

        private static byte[] Bin(byte[] b, ref int i, int n)
        {
            var slice = new byte[n];
            Array.Copy(b, i, slice, 0, n);
            i += n;
            return slice;
        }

        private static List<object?> ReadArray(byte[] b, ref int i, int n)
        {
            var list = new List<object?>(n);
            for (int k = 0; k < n; k++) list.Add(Read(b, ref i));
            return list;
        }

        private static Dictionary<string, object?> ReadMap(byte[] b, ref int i, int n)
        {
            var map = new Dictionary<string, object?>(n);
            for (int k = 0; k < n; k++)
            {
                object? key = Read(b, ref i);
                map[key?.ToString() ?? ""] = Read(b, ref i);
            }
            return map;
        }
    }
}
