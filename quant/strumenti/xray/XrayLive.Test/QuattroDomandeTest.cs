using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using XrayLive.Server;

namespace XrayLive.Test;

// Le prove delle risposte nuove del server (5 novembre 2026, progetto della pagina in stile QuantData, E.13): le
// quattro domande portate da Python, la scala con le zone fuse, il ping-pong portato da gli_scalini.py, i minuti
// anomali della Deriva, e le rotte nuove sull'archivio sintetico.

public class QuattroDomandeTest
{
    [Fact]
    public void SegnaliOpposti_quandoAlPrezzoE0dteDifferiscono()
    {
        var (l, f) = QuattroDomande.Lettura("acceleratore", "freno");
        Assert.Equal("segnali opposti", l);
        // la frase di Python, parola per parola (con gli accenti veri)
        Assert.Equal("segnali opposti: acceleratore su tutte le scadenze, freno sullo 0DTE; il regime non è netto, e lo 0DTE " +
                     "pesa di più man mano che si avvicina la chiusura", f);
        Assert.Equal("freno", QuattroDomande.Lettura("freno", "freno").lettura);
        Assert.Equal("acceleratore", QuattroDomande.Lettura(null, "acceleratore").lettura);
        Assert.StartsWith("freno: per il video la giornata resta più stretta", QuattroDomande.Lettura("freno", null).frase);
        Assert.Equal((null, null), QuattroDomande.Lettura(null, null));
    }

    [Fact]
    public void IlVersoDiPython_loZeroEFreno()
    {
        Assert.Equal("freno", QuattroDomande.Verso(0));
        Assert.Equal("acceleratore", QuattroDomande.Verso(-0.1));
        Assert.Null(QuattroDomande.Verso(null));
    }

    [Fact]
    public void IViciniSonoLeCoppieEntroLaSoglia_nellOrdineDiPython()
    {
        var muri = new List<(string, double)> { ("freno più forte", 7745), ("flip", 7700) };
        var bordi = new List<Bordo>
        {
            new("VAH cassa mer 04", "VAH", "2026-11-04", 7741.75),
            new("muro alto del ping-pong", "muro alto", "2026-11-02", 7746.25),
        };
        var v = QuattroDomande.Vicini(muri, bordi, 6).Select(x => JsonSerializer.SerializeToElement(x)).ToList();
        Assert.Equal(2, v.Count);
        Assert.Equal("VAH cassa mer 04", v[0].GetProperty("bordo").GetString());
        Assert.Equal(3.25, v[0].GetProperty("distanza").GetDouble());
        Assert.Equal(-1.25, v[1].GetProperty("distanza").GetDouble());
    }

    [Fact]
    public void SenzaBridge_bordiVuotiConLaNota()
    {
        var (bordi, nota) = new Bordi("http://127.0.0.1:1").Di("ES");
        Assert.Empty(bordi);
        Assert.Contains("bridge non raggiungibile", nota);
    }

    [Fact]
    public void IlGiornoBreveComeLeQuattroTab() => Assert.Equal("mer 04", Bordi.Breve("2026-11-04"));
}

public class ScalaTest
{
    private static VoceScala V(double p, string quando = "adesso", string cosa = "flip") =>
        new(cosa, "flip", "xray", null, null, null, p, quando, quando == "adesso" ? null : "2026-11-06");

    [Fact]
    public void FondeLeVociVicine_eOrdinaDallAlto()
    {
        // 7705 entra (5 da 7700), 7710 entra (5 da 7705, l'estremo alto della zona), 7717 no (7 da 7710)
        var righe = Scala.Fondi([V(7700), V(7705), V(7710), V(7717), V(7800), V(7600)], 7750, Scala.SogliaEs);
        Assert.Equal([7800d, 7717d, 7705d, 7600d], righe.Select(r => r.Prezzo));
        var zona = righe[2];
        Assert.True(zona.Zona);
        Assert.Equal(7700, zona.Da);
        Assert.Equal(7710, zona.A);
        Assert.Equal(3, zona.Voci.Count);
        Assert.Equal(-45, zona.Distanza);
        Assert.False(righe[1].Zona);
    }

    [Fact]
    public void LaSogliaDiNqE25_eAdessoNonSiFondeConLaProssimaSeduta()
    {
        Assert.Single(Scala.Fondi([V(28700), V(28725)], null, Scala.SogliaNq));
        Assert.Equal(2, Scala.Fondi([V(28700), V(28726)], null, Scala.SogliaNq).Count);
        var r = Scala.Fondi([V(7700), V(7701, "prossima seduta")], 7700, Scala.SogliaEs);
        Assert.Equal(2, r.Count);
        Assert.All(r, x => Assert.False(x.Zona));
    }
}

public class ScaliniTest
{
    // le stesse candele fatte girare su FabioOrderFlow/tools/gli_scalini.misura il 5 novembre 2026: stessi risultati
    private static readonly (string giorno, double o, double h, double l, double c)[] Barre =
    {
        ("2026-10-04", 100, 110, 90, 105),   // giorno di borsa 10-05
        ("2026-10-05", 106, 115, 95, 112),   // 10-06: scalino su
        ("2026-10-06", 110, 114, 100, 108),  // 10-07: dentro (pausa)
        ("2026-10-07", 108, 118, 99, 110),   // 10-08: ingloba (pausa)
        ("2026-10-08", 110, 116, 101, 104),  // 10-09: pausa
    };

    private static List<Scalini.Candela> Candele(int n) => Barre.Take(n)
        .Select(b => new Scalini.Candela(DateTime.Parse(b.giorno + "T22:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
            b.o, b.h, b.l, b.c)).ToList();

    private static readonly DateTime Sabato = new(2026, 10, 10, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void PingPongDopoLoScalinoSu()
    {
        var p = Scalini.Misura(Candele(5), Sabato);
        Assert.Equal(118, p.MuroAlto);
        Assert.Equal(99, p.MuroBasso);
        Assert.Equal("2026-10-07", p.Dal);
        Assert.Equal(3, p.Giornate);
        Assert.Equal("scalino su", p.DopoLoScalino);
    }

    [Fact]
    public void LUltimaGiornataEUnoScalino_nienteePingPong()
    {
        var p = Scalini.Misura(Candele(2), Sabato);
        Assert.Null(p.MuroAlto);
        Assert.Equal("l'ultima giornata chiusa (2026-10-06) e' uno scalino: niente ping-pong", p.Nota);
    }

    [Fact]
    public void LoScartoDelRolloverCambiaLoScalino_nonIMuri()
    {
        var p = Scalini.Misura(Candele(5), Sabato, 50, "2026-10-06");
        Assert.Equal("scalino giu", p.DopoLoScalino);
        Assert.Equal(118, p.MuroAlto);
    }

    [Fact]
    public void LaGiornataApertaNonConta_finoAlle16DiChicago()
    {
        // venerdi' 9 ottobre alle 15:00 di Chicago (20:00Z): la giornata del 9 non e' chiusa
        var p = Scalini.Misura(Candele(5), new DateTime(2026, 10, 9, 20, 0, 0, DateTimeKind.Utc));
        Assert.Equal(2, p.Giornate);
        Assert.Equal("scalino su", Scalini.Stato(110, 90, 115, 95, 112));
        Assert.Equal("pausa", Scalini.Stato(110, 90, 115, 95, 109));   // visitato, non accettato
    }

    [Fact]
    public void LaMisuraInteraHaLeChiaviDiPython_eLeScaleCorte()
    {
        // cinque giornate: giorno in pausa (dentro... ingloba... pausa), settimana e mese troppo corti
        var (m, _) = Scalini.Calcola(Candele(5), Sabato);
        Assert.Equal(new[] { "aggiustato_al_rollover", "convenzione", "fonte", "scale", "insieme", "ping_pong", "ultima_chiusa" }, m.Keys);
        var scale = (Dictionary<string, object?>)m["scale"]!;
        Assert.Equal(new[] { "mese", "settimana", "giorno" }, scale.Keys);
        var giorno = (Dictionary<string, object?>)scale["giorno"]!;
        Assert.Equal("pausa", giorno["stato"]);
        Assert.Equal("2026-10-09", ((Dictionary<string, object?>)giorno["candela"]!)["al"]);
        Assert.Equal("2026-10-06", ((Dictionary<string, object?>)giorno["ultimo_scalino"]!)["al"]);
        Assert.Null(((Dictionary<string, object?>)scale["settimana"]!)["stato"]);   // una settimana sola, e chiusa
        Assert.Equal("incompleto", m["insieme"]);
        Assert.Equal(118.0, ((Dictionary<string, object?>)m["ping_pong"]!)["muro_alto"]);
        Assert.Equal("2026-10-09", m["ultima_chiusa"]);
    }

    [Fact]
    public void LaSettimanaEIlMeseSiCostruisconoPerGiornoDiBorsa_eIlPrimoMeseCortoSiScarta()
    {
        // 70 giornate feriali da lunedi' 3 agosto 2026, sempre piu' alte: ogni scala e' uno scalino su
        var candele = new List<Scalini.Candela>();
        var d = new DateTime(2026, 8, 2, 22, 0, 0, DateTimeKind.Utc);   // domenica sera: giorno di borsa lunedi' 3
        for (int i = 0; candele.Count < 70; d = d.AddDays(1), i++)
        {
            var g = Scalini.GiornoDiBorsa(d);
            if (g.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
            double b = 100 + candele.Count * 10;
            candele.Add(new Scalini.Candela(d, b, b + 12, b - 2, b + 11));
        }
        var adesso = new DateTime(2026, 11, 7, 12, 0, 0, DateTimeKind.Utc);   // sabato
        var (m, _) = Scalini.Calcola(candele, adesso);
        var scale = (Dictionary<string, object?>)m["scale"]!;
        foreach (var n in new[] { "mese", "settimana", "giorno" })
            Assert.Equal("scalino su", ((Dictionary<string, object?>)scale[n]!)["stato"]);
        Assert.Equal("allineati su", m["insieme"]);
        // agosto (21 giornate) resta, ottobre e' l'ultimo mese chiuso: novembre contiene il giorno di borsa di oggi
        var mese = (Dictionary<string, object?>)scale["mese"]!;
        Assert.Equal("2026-10-01", ((Dictionary<string, object?>)mese["candela"]!)["dal"]);
        Assert.Equal("2026-09-01", ((Dictionary<string, object?>)mese["prima"]!)["dal"]);
        Assert.Equal("l'ultima giornata chiusa (2026-11-06) e' uno scalino: niente ping-pong",
            ((Dictionary<string, object?>)m["ping_pong"]!)["nota"]);
        Assert.True(m.ContainsKey("realizzata_20_giornate"));
    }
}

public class AnomaliTest
{
    private static (List<double?> s, List<ColStrike> c, List<long> t) Serie(double[] flussi, long from = -60_000)
    {
        long t0 = ArchivioDiProva.Dieci;
        var tempi = Enumerable.Range(0, flussi.Length).Select(i => t0 + i * 60_000L).ToList();
        var col = tempi.Select((t, i) => new ColStrike(t, 7600, t + 3_600_000, 0, [7600], [flussi[i]], [0], t + from)).ToList();
        double somma = 0;
        var s = flussi.Select(f => (double?)(somma += f)).ToList();
        return (s, col, tempi);
    }

    [Fact]
    public void UnSaltoDiCentoVolteLaMedianaSiMarca()
    {
        var f = Enumerable.Range(0, 60).Select(i => i % 2 == 0 ? 10.0 : -8).ToArray();
        f[30] = 1000;
        var (s, c, t) = Serie(f);
        var a = Misure.Anomali(s, c, t, "dexTutte.firm").Select(x => JsonSerializer.SerializeToElement(x)).ToList();
        Assert.Single(a);
        Assert.Equal(30, a[0].GetProperty("indice").GetInt32());
        Assert.Equal("salto", a[0].GetProperty("motivo").GetString());
    }

    [Fact]
    public void UnaSerieLisciaNo_eLaPartenzaSiRiconosce()
    {
        var f = Enumerable.Range(0, 60).Select(i => i % 2 == 0 ? 10.0 : -8).ToArray();
        var (s, c, t) = Serie(f);
        Assert.Empty(Misure.Anomali(s, c, t, "x"));
        f[0] = 5000;
        (s, c, t) = Serie(f);
        var a = JsonSerializer.SerializeToElement(Misure.Anomali(s, c, t, "x")[0]);
        Assert.Equal("partenza", a.GetProperty("motivo").GetString());
    }

    [Fact]
    public void UnaFinestraPiuLungaDiUnMinutoEFinestra()
    {
        var f = Enumerable.Range(0, 60).Select(i => i % 2 == 0 ? 10.0 : -8).ToArray();
        f[20] = 2000;
        var (s, c, t) = Serie(f);
        c[20] = c[20] with { From = c[20].Ts - 420_000 };
        var a = JsonSerializer.SerializeToElement(Misure.Anomali(s, c, t, "x").Single());
        Assert.Equal("finestra", a.GetProperty("motivo").GetString());
    }

    [Fact]
    public void LaSedutaDiUnaFinestra()
    {
        // da venerdi' 6 novembre 2026 alle 04:00 di New York a lunedi' 9: la cassa di venerdi'
        long da = new DateTimeOffset(2026, 11, 6, 9, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
        long fino = new DateTimeOffset(2026, 11, 9, 21, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
        Assert.Equal("2026-11-06", Misure.SedutaDi(da, fino));
        // dopo la cassa di venerdi', la prima apertura e' lunedi'
        long sera = new DateTimeOffset(2026, 11, 6, 22, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
        Assert.Equal("2026-11-09", Misure.SedutaDi(sera, fino));
    }
}

public class RotteNuoveTest
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
    public async Task LeRotteNuoveRispondono()
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
                tracks = new object[] { new { kind = 0, first = 0, price = new double?[] { 7601.0, null, null, null, 7603.5, 7604 } } },
            });

        int porta = PortaLibera();
        var s = new Servitore(a.Root, Path.Combine(a.Root, "nessuna-pagina"), porta, "http://127.0.0.1:1") { Raccogli = false };
        s.Avvia();
        try
        {
            using var http = new HttpClient { BaseAddress = new Uri($"http://127.0.0.1:{porta}/"), Timeout = TimeSpan.FromSeconds(60) };
            async Task<JsonElement> Get(string u) => JsonDocument.Parse(await http.GetStringAsync(u)).RootElement;

            var q = await Get("api/quattro-domande?radice=ES");
            Assert.Equal("ES", q.GetProperty("radice").GetString());
            // senza bridge niente base: niente prezzi di ES, e lo si dice; i bordi sono vuoti con la nota
            Assert.Equal(JsonValueKind.Null, q.GetProperty("prezzo").ValueKind);
            Assert.Contains(q.GetProperty("errori").EnumerateArray(), e => e.GetString()!.Contains("base"));
            Assert.Contains("bridge non raggiungibile", q.GetProperty("muri").GetProperty("nota").GetString());
            Assert.Equal("acceleratore", q.GetProperty("regime").GetProperty("zeroDte").GetString());   // la netta 0DTE e' -11
            Assert.Equal("NQ", (await Get("api/quattro-domande?radice=NQ")).GetProperty("radice").GetString());

            var adesso = await Get("api/adesso");
            Assert.True(adesso.TryGetProperty("scala", out var scala));
            Assert.True(scala.GetArrayLength() > 0);
            Assert.Equal(JsonValueKind.Null, adesso.GetProperty("pressione").ValueKind);
            Assert.Equal(7603.5, adesso.GetProperty("flipVicino").GetProperty("prezzo").GetDouble());

            var heat = await Get("api/heatmap?metrica=gex");
            Assert.Equal(4, heat.GetProperty("nette").GetArrayLength());
            Assert.Equal(-11, heat.GetProperty("nette")[0].GetDouble());

            var strike = await Get("api/strike?metrica=gex");
            Assert.Equal(7590, strike.GetProperty("muri").GetProperty("positivo").GetProperty("strike").GetDouble());

            var deriva = await Get("api/deriva?passo=1");
            Assert.Equal(JsonValueKind.Array, deriva.GetProperty("anomali").ValueKind);
            Assert.Equal(-10, deriva.GetProperty("adesso").GetProperty("retail0dte").GetDouble());

            // la traccia del flip con tre minuti vuoti si spezza in due linee
            var livelli = await Get("api/livelli?passo=1");
            Assert.Equal(2, livelli.GetProperty("zeri").GetArrayLength());
        }
        finally
        {
            s.Ferma();
        }
    }
}
