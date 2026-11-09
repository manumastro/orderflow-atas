using System.Text.Json;
using XrayLive.Server;

namespace XrayLive.Test;

// Il flusso per gruppo contro lo storico (9 novembre 2026): su un archivio sintetico di 14 sedute, in cui il flusso
// del retail e' esattamente lo specchio del prezzo nelle 13 passate e oggi vende 5 in piu' al minuto, il server deve
// dire che il prezzo spiega tutto (R2 = 1), che la sorpresa e' -300 e piu' bassa di ogni seduta passata, e che lo
// sbilancio e' -1. Un gruppo senza dati resta vuoto, e il futuro di un giorno passato non entra nel suo storico.
public sealed class FlussoTest : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "xray-flusso-" + Guid.NewGuid().ToString("N")[..8]);
    private static readonly string[] Giorni = Enumerable.Range(1, 14).Select(d => $"2026-10-{d:00}").ToArray();
    private const string Oggi = "2026-10-14";

    public FlussoTest()
    {
        for (int i = 0; i < Giorni.Length; i++)
        {
            string g = Giorni[i];
            double pendenza = g == Oggi ? 0.3 : (i - 6) * 0.1;              // punti di SPX al minuto
            long apertura = Cassa.Di(g).apertura;
            var spot = new List<object>();
            var retail = new List<object>();
            var prof = new List<object>();
            for (int k = 1; k <= 90; k++)
            {
                long ts = apertura + k * 60_000L;
                spot.Add(ArchivioDiProva.Strike(ts, 6000 + pendenza * k, [6000], [1], [0]));
                // il retail vende delta quando il prezzo sale: -10 per punto; oggi 5 in piu' al minuto
                double r = -10 * pendenza - (g == Oggi ? 5 : 0);
                retail.Add(ArchivioDiProva.Strike(ts, 6000, [6000], [r], [0], ts - 60_000));
                prof.Add(ArchivioDiProva.Strike(ts, 6000, [6000], [1], [0], ts - 60_000));
            }
            Scrivi("gex__mm_0dte", g, spot);
            Scrivi("dex__nonprocust_0dte", g, retail);
            Scrivi("dex__procust_0dte", g, prof);
        }
    }

    private void Scrivi(string famiglia, string giorno, List<object> colonne)
    {
        var dir = Directory.CreateDirectory(Path.Combine(_root, "series", "SPX", famiglia)).FullName;
        var riga = JsonSerializer.SerializeToNode(new { type = "strikes", metric = famiglia.Split("__")[0] })!.AsObject();
        riga["columns"] = JsonSerializer.SerializeToNode(colonne);
        File.WriteAllText(Path.Combine(dir, giorno + ".jsonl"), riga.ToJsonString() + "\n");
    }

    private JsonElement Leggi(string giorno, int minuto)
    {
        var mag = new Magazzino(_root);
        var f = new Flusso(mag, new Misure(mag, new Ponte("http://127.0.0.1:1")));
        return JsonSerializer.SerializeToElement(f.Di(giorno, Cassa.Di(giorno).apertura + minuto * 60_000L));
    }

    private static JsonElement Gruppo(JsonElement r, string gruppo, string ambito) =>
        r.GetProperty("gruppi").EnumerateArray().Single(x => x.GetProperty("gruppo").GetString() == gruppo && x.GetProperty("ambito").GetString() == ambito);

    [Fact]
    public void LaSorpresaEQuelloCheIlPrezzoNonSpiega()
    {
        var r = Leggi(Oggi, 60);
        Assert.Equal(60, r.GetProperty("minuto").GetInt32());
        Assert.True(r.GetProperty("cassa").GetBoolean());
        Assert.Equal(13, r.GetProperty("sedute").GetInt32());

        var retail = Gruppo(r, "nonprocust", "0dte");
        Assert.Equal(-480, retail.GetProperty("oggi").GetDouble(), 6);          // 60 minuti da -3 - 5
        Assert.Equal(1, retail.GetProperty("spiegaPrezzo").GetDouble(), 6);    // nelle passate e' lo specchio esatto
        Assert.Equal("contro", retail.GetProperty("versoPrezzo").GetString());
        // il prezzo e' salito di 0,3 x 59 dal primo minuto: l'atteso e' -10 per punto, la sorpresa i 300 in piu' venduti
        Assert.Equal(-300, retail.GetProperty("sorpresa").GetDouble(), 1);
        Assert.Equal(0, retail.GetProperty("percentileSorpresa").GetDouble(), 6);
        Assert.Equal(-1, retail.GetProperty("sbilancio").GetDouble(), 6);
        Assert.Equal(-240, retail.GetProperty("trentaMinuti").GetDouble(), 6); // gli ultimi 30 minuti

        // i professionali comprano 1 al minuto ogni giorno: oggi e' a meta' dello storico, e il prezzo non spiega niente
        var prof = Gruppo(r, "procust", "0dte");
        Assert.Equal(50, prof.GetProperty("percentile").GetDouble(), 6);
        Assert.Equal(0, prof.GetProperty("spiegaPrezzo").GetDouble(), 6);

        // un gruppo senza dati resta vuoto, senza inventare zeri
        var firm = Gruppo(r, "firm", "0dte");
        Assert.Equal(JsonValueKind.Null, firm.GetProperty("oggi").ValueKind);
        Assert.Equal(JsonValueKind.Null, firm.GetProperty("percentile").ValueKind);

        // le bande: ogni 5 minuti, piu' il minuto di adesso; la linea di oggi si ferma ad adesso
        var b = r.GetProperty("bande").GetProperty("nonprocust_0dte");
        var minuti = b.GetProperty("minuti").EnumerateArray().Select(x => x.GetInt32()).ToList();
        Assert.Equal(0, minuti[0]);
        Assert.Contains(60, minuti);
        var oggi = b.GetProperty("oggi").EnumerateArray().ToList();
        Assert.Equal(JsonValueKind.Null, oggi[minuti.IndexOf(65)].ValueKind);
        Assert.True(b.GetProperty("p10")[minuti.IndexOf(60)].GetDouble() <= b.GetProperty("p90")[minuti.IndexOf(60)].GetDouble());
    }

    [Fact]
    public void LoStoricoDiUnGiornoPassatoNonVedeIlSuoFuturo_ePrimaDellaCassaNonSiConfronta()
    {
        var r = Leggi("2026-10-05", 60);
        Assert.Equal(4, r.GetProperty("sedute").GetInt32());                    // solo 1-4 ottobre
        Assert.Equal(JsonValueKind.Null, Gruppo(r, "nonprocust", "0dte").GetProperty("percentile").ValueKind); // meno di 10

        var prima = Leggi(Oggi, -30);
        Assert.False(prima.GetProperty("cassa").GetBoolean());
        Assert.Equal(JsonValueKind.Null, Gruppo(prima, "nonprocust", "0dte").GetProperty("oggi").ValueKind);
    }

    [Fact]
    public void LeSeduteChiuseSiScrivonoSuDisco_eSiRileggonoUguali()
    {
        var primo = Leggi(Oggi, 60);
        var file = Path.Combine(_root, "flusso", "2026-10-13.json");
        Assert.True(File.Exists(file));
        Assert.False(File.Exists(Path.Combine(_root, "flusso", Oggi + ".json")));   // oggi e' ancora aperta
        var secondo = Leggi(Oggi, 60);                                               // un server nuovo: legge dal disco
        Assert.Equal(primo.GetRawText(), secondo.GetRawText());
    }

    [Fact]
    public void PercentileQuantileRegressione()
    {
        Assert.Equal(50, Flusso.Percentile([1, 2, 3, 4], 2.5), 6);
        Assert.Equal(37.5, Flusso.Percentile([1, 2, 3, 4], 2), 6);              // un pari conta mezzo
        Assert.Equal(2.5, Flusso.Quantile([1, 2, 3, 4], 0.5), 6);
        var reg = Flusso.Regressione([0, 1, 2], [1, 3, 5])!.Value;
        Assert.Equal(1, reg.a, 6);
        Assert.Equal(2, reg.b, 6);
        Assert.Equal(1, reg.r2, 6);
        Assert.Null(Flusso.Regressione([1, 1], [1, 2]));
    }

    public void Dispose()
    {
        try { Directory.Delete(_root, true); } catch (IOException) { }
    }
}
