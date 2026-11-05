using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using XrayLive.Server;

namespace XrayLive.Test;

// Le prove del server della pagina su un archivio sintetico, scritto nella cartella temporanea:
// la lettura (incrementale, revisioni, .gz + .jsonl, riga a meta'), le misure (DEX come flusso
// cumulato, interpolazione, la cassa di New York nei due orari) e l'API vera, avviata su una porta
// libera con un bridge che non risponde (tutto deve restare in SPX e dirlo).

public sealed class ArchivioDiProva : IDisposable
{
    public const string Giorno = "2026-10-30";          // ora legale a New York: cassa 13:30Z-20:00Z
    public readonly string Root = Path.Combine(Path.GetTempPath(), "xray-prova-" + Guid.NewGuid().ToString("N")[..8]);

    // 10:00 di New York del 30 ottobre 2026 = 14:00Z
    public static readonly long Dieci = new DateTimeOffset(2026, 10, 30, 14, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

    public ArchivioDiProva() => Directory.CreateDirectory(Root);

    public string File(string famiglia) =>
        Path.Combine(Directory.CreateDirectory(Path.Combine(Root, "series", "SPX", famiglia)).FullName, Giorno + ".jsonl");

    public void Riga(string famiglia, object testa, params object[] colonne)
    {
        var riga = JsonSerializer.SerializeToNode(testa)!.AsObject();
        riga["columns"] = JsonSerializer.SerializeToNode(colonne);
        System.IO.File.AppendAllText(File(famiglia), riga.ToJsonString() + "\n");
    }

    public static object Strike(long ts, double spot, double[] strike, double[] call, double[] put, long from = 0) =>
        new { ts, spot, strikes = strike, call, put, dte = 0, mixed = false, expiry = ts + 3_600_000, from };

    public void Dispose()
    {
        try { Directory.Delete(Root, true); } catch (IOException) { }
    }
}

public class LetturaTest
{
    [Fact]
    public void LeggeSoloLeRigheComplete_ePoiLeNuove()
    {
        using var a = new ArchivioDiProva();
        var testa = new { type = "strikes", metric = "gex" };
        a.Riga("gex__mm_0dte", testa, ArchivioDiProva.Strike(ArchivioDiProva.Dieci, 7600, [7600], [1], [2]));
        // una riga che il lettore sta ancora scrivendo: senza a capo
        System.IO.File.AppendAllText(a.File("gex__mm_0dte"), "{\"type\":\"strikes\",\"columns\":[{\"ts\":");

        var m = new Magazzino(a.Root);
        var prima = m.Strike("gex__mm_0dte", ArchivioDiProva.Giorno);
        Assert.Single(prima);

        // la riga si completa: la seconda colonna arriva al giro dopo, la prima non si rilegge
        System.IO.File.AppendAllText(a.File("gex__mm_0dte"),
            $"{ArchivioDiProva.Dieci + 60_000},\"spot\":7601,\"strikes\":[7600],\"call\":[3],\"put\":[4],\"dte\":0,\"expiry\":{ArchivioDiProva.Dieci + 3_600_000}}}]}}\n");
        var dopo = m.Strike("gex__mm_0dte", ArchivioDiProva.Giorno);
        Assert.Equal(2, dopo.Count);
        Assert.Equal(7, dopo[1].Netto(0));
    }

    [Fact]
    public void UnaRevisioneSostituisce_eLeColonneRestanoInOrdine()
    {
        using var a = new ArchivioDiProva();
        var testa = new { type = "strikes", metric = "gex" };
        a.Riga("gex__mm_0dte", testa, ArchivioDiProva.Strike(ArchivioDiProva.Dieci + 60_000, 7600, [7600], [1], [1]));
        a.Riga("gex__mm_0dte", testa, ArchivioDiProva.Strike(ArchivioDiProva.Dieci, 7600, [7600], [5], [5]));
        a.Riga("gex__mm_0dte", testa, ArchivioDiProva.Strike(ArchivioDiProva.Dieci + 60_000, 7600, [7600], [9], [9]));   // revisione

        var cs = new Magazzino(a.Root).Strike("gex__mm_0dte", ArchivioDiProva.Giorno);
        Assert.Equal(2, cs.Count);
        Assert.Equal(ArchivioDiProva.Dieci, cs[0].Ts);
        Assert.Equal(18, cs[1].Netto(0));
    }

    [Fact]
    public void LeggePrimaIlGz_poiIlJsonl()
    {
        using var a = new ArchivioDiProva();
        string percorso = a.File("gex__mm_0dte");
        var riga1 = "{\"type\":\"strikes\",\"columns\":[" + JsonSerializer.Serialize(ArchivioDiProva.Strike(ArchivioDiProva.Dieci, 7600, [7600], [1], [0])) + "]}\n";
        using (var gz = new GZipStream(System.IO.File.Create(percorso + ".gz"), CompressionLevel.Fastest))
            gz.Write(Encoding.UTF8.GetBytes(riga1));
        a.Riga("gex__mm_0dte", new { type = "strikes" }, ArchivioDiProva.Strike(ArchivioDiProva.Dieci + 60_000, 7600, [7600], [2], [0]));

        var cs = new Magazzino(a.Root).Strike("gex__mm_0dte", ArchivioDiProva.Giorno);
        Assert.Equal([1d, 2d], cs.Select(c => c.Netto(0)));
    }
}

public class MisureTest
{
    [Fact]
    public void IlDexSiSommaDallInizioDellaGiornata()
    {
        using var a = new ArchivioDiProva();
        var testa = new { type = "strikes", metric = "dex" };
        // tre minuti di flusso del retail sugli 0DTE: ogni colonna e' una finestra di 60 s
        for (int i = 0; i < 3; i++)
        {
            long ts = ArchivioDiProva.Dieci + i * 60_000;
            a.Riga("dex__nonprocust_0dte", testa, ArchivioDiProva.Strike(ts, 7600, [7590, 7600], [i + 1, 0], [0, -10], ts - 60_000));
        }
        var m = new Misure(new Magazzino(a.Root), new Ponte("http://127.0.0.1:1"));
        var (col, totale, trenta) = m.DexCumulato(ArchivioDiProva.Giorno, "dex__nonprocust_0dte", ArchivioDiProva.Dieci + 120_000);
        Assert.NotNull(col);
        Assert.Equal([7590d, 7600d], col!.Strike);
        Assert.Equal(6, col.Call[0]);       // 1 + 2 + 3
        Assert.Equal(-30, col.Put[1]);      // tre minuti da -10
        Assert.Equal(-24, totale);
        Assert.Equal(-24, trenta);          // tutto dentro gli ultimi 30 minuti

        var (_, aMeta, _) = m.DexCumulato(ArchivioDiProva.Giorno, "dex__nonprocust_0dte", ArchivioDiProva.Dieci + 60_000);
        Assert.Equal(-17, aMeta);           // (1 - 10) + (2 - 10)
    }

    [Fact]
    public void InterpolaFraDuePrezzi_eFuoriDallaGrigliaNiente()
    {
        double[] p = [7600, 7602.5, 7605];
        float[] v = [-10, 10, 30];
        Assert.Equal(0, Misure.Interpola(p, v, 7601.25), 6);
        Assert.Equal(20, Misure.Interpola(p, v, 7603.75), 6);
        Assert.True(double.IsNaN(Misure.Interpola(p, v, 7700)));
        Assert.Equal("freno", Misure.Regime(1));
        Assert.Equal("acceleratore", Misure.Regime(-1));
    }

    [Fact]
    public void LaCassaSiConvertePerData_ancheDopoIlCambioDOra()
    {
        // 30 ottobre: ora legale a New York, la cassa apre alle 13:30Z
        var (a1, c1) = Cassa.Di("2026-10-30");
        Assert.Equal(new DateTimeOffset(2026, 10, 30, 13, 30, 0, TimeSpan.Zero).ToUnixTimeMilliseconds(), a1);
        Assert.Equal(new DateTimeOffset(2026, 10, 30, 20, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds(), c1);
        // 2 novembre: ora solare, apre alle 14:30Z (un'ora dopo, in UTC)
        var (a2, _) = Cassa.Di("2026-11-02");
        Assert.Equal(new DateTimeOffset(2026, 11, 2, 14, 30, 0, TimeSpan.Zero).ToUnixTimeMilliseconds(), a2);
        Assert.True(Cassa.Dentro(ArchivioDiProva.Dieci));
        Assert.False(Cassa.Dentro(a1 - 60_000));
        Assert.False(Cassa.Dentro(new DateTimeOffset(2026, 10, 31, 15, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds()));  // sabato
    }

    [Fact]
    public void MedianaEUltimoIndice()
    {
        Assert.Equal(30, Ponte.Mediana([27, 30, 37]));
        Assert.Equal(31, Ponte.Mediana([27, 30, 32, 37]));
        long[] k = [10, 20, 30];
        Assert.Equal(-1, Ponte.UltimoIndice(k, 5));
        Assert.Equal(1, Ponte.UltimoIndice(k, 25));
        Assert.Equal(2, Ponte.UltimoIndice(k, 99));
    }
}

public class ServitoreTest
{
    private static int PortaLibera()
    {
        var l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        int p = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return p;
    }

    [Fact]
    public async Task LApiRispondeSuUnArchivioSintetico()
    {
        using var a = new ArchivioDiProva();
        var gex = new { type = "strikes", metric = "gex" };
        var dex = new { type = "strikes", metric = "dex" };
        for (int i = 0; i < 5; i++)
        {
            long ts = ArchivioDiProva.Dieci + i * 60_000;
            a.Riga("gex__mm_0dte", gex, ArchivioDiProva.Strike(ts, 7600 + i, [7590, 7600, 7610], [5, -20, 1], [3, 0, 0]));
            a.Riga("dex__nonprocust_0dte", dex, ArchivioDiProva.Strike(ts, 7600 + i, [7600], [-2], [0], ts - 60_000));
        }
        a.Riga("observed__gamma", new { type = "plane", metric = "observed" },
            new { ts = ArchivioDiProva.Dieci + 240_000, prices = new[] { 7595.0, 7600, 7605, 7610 }, values = new[] { -5.0, -1, 3, 8 } });
        a.Riga("levels__gamma", new { type = "levels", metric = "levels" },
            new
            {
                surface = "observed", ts = ArchivioDiProva.Dieci, until = ArchivioDiProva.Dieci + 3_600_000, spanMs = 60_000,
                prices = new[] { 7595.0, 7600, 7605, 7610 },
                tracks = new object[] { new { kind = 0, first = 0, price = new[] { 7602.0, 7602.5, 7603, 7603.5, 7604 } } },
            });

        int porta = PortaLibera();
        var s = new Servitore(a.Root, Path.Combine(a.Root, "nessuna-pagina"), porta, "http://127.0.0.1:1") { Raccogli = false };
        s.Avvia();
        try
        {
            using var http = new HttpClient { BaseAddress = new Uri($"http://127.0.0.1:{porta}/") };
            async Task<JsonElement> Get(string u) => JsonDocument.Parse(await http.GetStringAsync(u)).RootElement;

            var giorni = await Get("api/giorni");
            Assert.Equal(ArchivioDiProva.Giorno, giorni.GetProperty("giorni")[0].GetString());

            var adesso = await Get("api/adesso");
            var rif = adesso.GetProperty("riferimento");
            Assert.Equal(7604, rif.GetProperty("prezzo").GetDouble());           // spot vivo: 10:04 e' in cassa
            Assert.True(rif.GetProperty("vivo").GetBoolean());
            Assert.Equal(JsonValueKind.Null, adesso.GetProperty("base").GetProperty("valore").ValueKind);  // niente bridge, niente base
            Assert.Equal("freno", adesso.GetProperty("gamma").GetProperty("regime").GetString());       // 7604 fra 3 e 8
            Assert.Equal(7604, adesso.GetProperty("flip")[0].GetDouble());
            var g0 = adesso.GetProperty("gex")[0];
            Assert.Equal(-11, g0.GetProperty("netto").GetDouble());
            Assert.Equal(7590, g0.GetProperty("muroFreno").GetProperty("strike").GetDouble());
            Assert.Equal(7600, g0.GetProperty("muroAcceleratore").GetProperty("strike").GetDouble());
            var retail = adesso.GetProperty("dex").EnumerateArray().First(d => d.GetProperty("gruppo").GetString() == "nonprocust");
            Assert.Equal(-10, retail.GetProperty("zeroDte").GetDouble());       // cinque minuti da -2

            var strike = await Get("api/strike?metrica=dex&gruppo=nonprocust&scadenza=0dte");
            Assert.Equal(-10, strike.GetProperty("netto")[0].GetDouble());

            var deriva = await Get("api/deriva?passo=1");
            var cum = deriva.GetProperty("dexZeroDte").GetProperty("nonprocust").EnumerateArray().Select(x => x.GetDouble()).ToList();
            Assert.Equal([-2d, -4, -6, -8, -10], cum);

            var heat = await Get("api/heatmap?metrica=gex");
            Assert.Equal(4, heat.GetProperty("colonne").GetArrayLength());         // 3 scadenze + totale
            Assert.Equal(7610, heat.GetProperty("strike")[0].GetDouble());          // dall'alto in basso

            var risposta = await http.GetAsync("api/adesso?giorno=2026-13-01");
            Assert.Equal(HttpStatusCode.NotFound, risposta.StatusCode);
        }
        finally
        {
            s.Ferma();
        }
    }
}
