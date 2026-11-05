using XrayLive.Volatilita;

namespace XrayLive.Test;

// Le prove della matematica della volatilita' (Volatilita/Formula.cs, CboePubblico.cs, Raccolta.cs) su catene
// sintetiche costruite con Black-76 a volatilita' nota: la formula del Cboe, l'interpolazione a 30 giorni,
// l'inversione della volatilita', il sorriso, la volatilita' da vanna/gamma (X-Ray), la GEX del Cboe e gli orari.

public sealed class VolatilitaTest
{
    private static readonly DateTime Adesso = new(2026, 11, 2, 15, 0, 0, DateTimeKind.Utc);

    /// <summary>Una catena sintetica: strike ogni `passo` da F*(1-ampiezza) a F*(1+ampiezza), prezzi di Black-76
    /// scontati, volatilita' data per moneyness, bid e ask a +-0,5% del prezzo.</summary>
    private static Scadenza Catena(double f, double giorni, Func<double, double> vol, double passo = 5, double ampiezza = 0.45,
        double tasso = Formula.Tasso)
    {
        double t = giorni / 365;
        var q = new List<Quotazione>();
        for (double k = Math.Round(f * (1 - ampiezza) / passo) * passo; k <= f * (1 + ampiezza); k += passo)
        {
            double s = vol(k / f - 1);
            foreach (bool call in new[] { true, false })
            {
                double p = Formula.Black(f, k, t, s, call) * Math.Exp(-tasso * t);
                q.Add(p < 0.01
                    ? new Quotazione(k, call, 0, 0.05, null)
                    : new Quotazione(k, call, p * 0.995, p * 1.005, p, 10));
            }
        }
        return new Scadenza($"g{giorni}", Adesso.AddDays(giorni), q);
    }

    [Fact]
    public void LaFormulaDelCboeRitrovaUnaVolatilitaPiatta()
    {
        var v = Formula.VarianzaCboe(Catena(7650, 30, _ => 0.20), Adesso, Formula.Prezzi.Medio);
        Assert.NotNull(v);
        Assert.InRange(v!.Vol, 19.7, 20.3);
        Assert.InRange(v.Forward, 7649, 7651);
        Assert.True(v.K0 <= v.Forward);
    }

    [Fact]
    public void ConLoSkewIlVixStaSopraLaVolatilitaAlPrezzo()
    {
        // put piu' care (skew negativo): la varianza pesa le ali, quindi supera la volatilita' allo strike
        Func<double, double> skew = m => 0.18 - 0.8 * m + 2 * m * m;
        var s = Catena(7650, 30, skew);
        var v = Formula.VarianzaCboe(s, Adesso, Formula.Prezzi.Medio)!;
        var atm = Formula.AlPrezzo(Formula.Sorriso(s, Adesso, v.Forward, Formula.Prezzi.Medio))!.Value;
        Assert.InRange(atm, 17.5, 18.5);
        Assert.True(v.Vol > atm + 1, $"vix {v.Vol} atm {atm}");
    }

    [Fact]
    public void UnoStrikeSenzaPrezzoSiSaltaDuePrezziNulliFermanoLAla()
    {
        var s = Catena(7650, 30, _ => 0.20);
        var buchi = s.Opzioni.Select(q => q.Strike is 7300 or 7305 && !q.Call ? q with { Bid = null, Ask = null } : q).ToList();
        var v = Formula.VarianzaCboe(s with { Opzioni = buchi }, Adesso, Formula.Prezzi.Medio)!;
        Assert.InRange(v.Vol, 19.6, 20.4);
        // il buco a 7300-7305 non ferma l'ala: si ferma dove le put valgono meno del tick (circa 6.300)
        Assert.True(v.StrikeMin < 6500, $"l'ala delle put si ferma a {v.StrikeMin}");
    }

    [Fact]
    public void ATrentaGiorniLInterpolazioneDelCboe()
    {
        var a = new Varianza("a", 25 / 365.0, 100, 100, 0.04, 50, 50, 150);
        var b = new Varianza("b", 35 / 365.0, 100, 100, 0.04, 50, 50, 150);
        Assert.Equal(20, Formula.A30Giorni(a, b)!.Value, 6);
        var c = new Varianza("c", 35 / 365.0, 100, 100, 0.09, 50, 50, 150);
        double v = Formula.A30Giorni(a, c)!.Value;
        Assert.InRange(v, 20, 30);
        Assert.Null(Formula.A30Giorni(c, a));
    }

    [Fact]
    public void LeScadenzeAttornoAiTrentaGiorni()
    {
        var giorni = new[] { 3.0, 11, 25, 32, 46 };
        var c = Formula.Attorno(giorni, g => Adesso.AddDays(g), Adesso);
        Assert.Equal((25.0, 32.0), c);
        Assert.Null(Formula.Attorno(new[] { 3.0, 40 }, g => Adesso.AddDays(g), Adesso));
    }

    [Fact]
    public void LaVolatilitaImplicitaInverteBlack()
    {
        foreach (var (k, call) in new[] { (7000.0, false), (7650.0, true), (8200.0, true) })
        {
            double p = Formula.Black(7650, k, 0.08, 0.23, call);
            Assert.Equal(0.23, Formula.Implicita(p, 7650, k, 0.08, call)!.Value, 4);
        }
        Assert.Null(Formula.Implicita(0, 7650, 7000, 0.08, true));
    }

    [Fact]
    public void IlSettlementSiUsaQuandoIlBookManca()
    {
        var s = Catena(7650, 30, _ => 0.20);
        var senzaBook = s with { Opzioni = s.Opzioni.Select(q => q with { Bid = null, Ask = null }).ToList() };
        Assert.Null(Formula.VarianzaCboe(senzaBook, Adesso, Formula.Prezzi.Medio));
        Assert.InRange(Formula.VarianzaCboe(senzaBook, Adesso, Formula.Prezzi.Settlement)!.Vol, 19.7, 20.3);
    }

    [Fact]
    public void LaVolatilitaDiXRayDaVannaEGamma()
    {
        // GEX = gamma x posizione, VEX = vanna x posizione: il rapporto non dipende dalla posizione
        double s0 = 7600, f = 7600, t = 18 / 365.0;
        Func<double, double> vol = m => 0.16 - 0.5 * m;
        var strike = new List<double>();
        var gc = new List<double>(); var gp = new List<double>(); var vc = new List<double>(); var vp = new List<double>();
        var pos = new Random(7);
        for (double k = 7200; k <= 8000; k += 25)
        {
            double sg = vol(k / f - 1), v = sg * Math.Sqrt(t);
            double d1 = (Math.Log(f / k) + v * v / 2) / v, d2 = d1 - v;
            double gamma = Formula.Phi(d1) / (s0 * v);
            double vanna = -Formula.Phi(d1) * d2 / sg;
            double pc = pos.Next(-500, 500), pp = pos.Next(-500, 500);
            strike.Add(k);
            gc.Add(gamma * pc * 1000); gp.Add(gamma * pp * 1000);
            vc.Add(vanna * pc * 1000); vp.Add(vanna * pp * 1000);
        }
        var r = Formula.DaVannaGamma(s0, strike, gc, gp, vc, vp);
        Assert.NotNull(r);
        // F: dove d2 = 0, cioe' F * e^(-sigma^2 T / 2), qualche punto sotto 7600
        Assert.InRange(r!.Value.forward, 7590, 7601);
        foreach (var (k, m, v) in r.Value.sorriso.Where(x => Math.Abs(x.moneyness) > 1))
            Assert.InRange(v, vol(k / f - 1) * 100 - 0.6, vol(k / f - 1) * 100 + 0.6);
    }

    [Fact]
    public void IlFileDelCboeSiLeggeELaGexHaIlSegnoDellaConvenzioneA()
    {
        const string json = """
        {"timestamp":"2026-11-02 15:00:00","data":{"symbol":"QQQ","current_price":700.0,"last_trade_time":"2026-11-02T09:59:00","iv30":24.1,
         "options":[
          {"option":"QQQ261204C00700000","bid":14.0,"ask":14.2,"iv":0.22,"open_interest":1000,"theo":14.1},
          {"option":"QQQ261204P00700000","bid":13.0,"ask":13.2,"iv":0.24,"open_interest":0,"theo":13.1},
          {"option":"QQQ261204P00650000","bid":3.0,"ask":3.1,"iv":0.30,"open_interest":5000,"theo":3.05},
          {"option":"QQQ261204C00750000","bid":2.0,"ask":2.1,"iv":0.19,"open_interest":0,"theo":2.05}]}}
        """;
        var c = CboePubblico.Leggi(json);
        Assert.Equal("QQQ", c.Simbolo);
        Assert.Equal(new DateTime(2026, 11, 2, 15, 0, 0, DateTimeKind.Utc), c.Quando);
        Assert.Equal(new DateTime(2026, 11, 2, 14, 59, 0, DateTimeKind.Utc), c.PrezzoQuando);   // 09:59 di New York in ora solare
        var s = Assert.Single(c.Scadenze);
        Assert.Equal("2026-12-04", s.Giorno);
        Assert.Equal(new DateTime(2026, 12, 4, 21, 0, 0, DateTimeKind.Utc), s.ScadeUtc);
        Assert.Equal(14.1, s.Opzioni.First(q => q.Call && q.Strike == 700).Settlement);

        var g = CboePubblico.Gex(c.Scadenze, 700, c.Quando);
        var k700 = g.Strike.Single(x => x.strike == 700);
        var k650 = g.Strike.Single(x => x.strike == 650);
        Assert.True(k700.call > 0 && k700.put == 0);
        Assert.True(k650.put < 0 && k650.call == 0);
        Assert.Equal(700, g.CallWall);
        Assert.Equal(650, g.PutWall);
        Assert.Equal(Math.Round(k700.call + k650.put, 1), Math.Round(g.Totale, 1));
    }

    [Fact]
    public void GliOrariDelCmeNeiDueFusi()
    {
        // lunedi' 2 novembre 2026, ora solare a New York (UTC-5)
        Assert.True(Raccolta.CmeAperto(new DateTime(2026, 11, 2, 15, 0, 0, DateTimeKind.Utc)));    // 10:00 NY
        Assert.False(Raccolta.CmeAperto(new DateTime(2026, 11, 2, 22, 30, 0, DateTimeKind.Utc)));  // 17:30 NY, la pausa
        Assert.False(Raccolta.CmeAperto(new DateTime(2026, 10, 31, 15, 0, 0, DateTimeKind.Utc)));  // sabato
        Assert.True(Raccolta.CmeAperto(new DateTime(2026, 11, 1, 23, 30, 0, DateTimeKind.Utc)));   // domenica 18:30 NY
        Assert.False(Raccolta.CmeAperto(new DateTime(2026, 10, 30, 21, 30, 0, DateTimeKind.Utc))); // venerdi' 17:30 NY (ora legale)
        // l'ultima chiusura: lunedi' mattina e' quella di venerdi' 16:00 NY (ora legale: 20:00Z)
        Assert.Equal(new DateTime(2026, 10, 30, 20, 0, 0, DateTimeKind.Utc), Raccolta.UltimaChiusura(new DateTime(2026, 11, 2, 12, 0, 0, DateTimeKind.Utc)));
        // lunedi' sera, dopo le 18:00 NY: quella di lunedi' 16:00 NY (ora solare: 21:00Z)
        Assert.Equal(new DateTime(2026, 11, 2, 21, 0, 0, DateTimeKind.Utc), Raccolta.UltimaChiusura(new DateTime(2026, 11, 2, 23, 30, 0, DateTimeKind.Utc)));
        Assert.Equal("2026-11-02", Raccolta.GiornoNY(new DateTime(2026, 11, 3, 3, 0, 0, DateTimeKind.Utc)));
    }

    [Fact]
    public void LeScadenzeDellaCatenaAtas()
    {
        Assert.Equal(new DateTime(2026, 11, 27, 21, 0, 0, DateTimeKind.Utc), CatenaAtas.Scade("2026-11-27", "Weekly"));
        Assert.Equal(new DateTime(2026, 12, 18, 14, 30, 0, DateTimeKind.Utc), CatenaAtas.Scade("2026-12-18", "Regular"));
        var charts = new List<CatenaAtas.Chart>
        {
            new("a", "ES", "Daily"), new("b", "ESZ6", "M30"), new("c", "ESZ6", "M1"), new("d", "NQZ6", "M1"),
        };
        var (v, l) = CatenaAtas.ChartPer(charts, "ES");
        Assert.Equal("c", v!.Id);
        Assert.Equal("a", l!.Id);
        var (vn, ln) = CatenaAtas.ChartPer(charts, "NQ");
        Assert.Equal("d", vn!.Id);
        Assert.Equal("d", ln!.Id);   // senza il Daily, tutte e due sullo stesso chart
    }

    [Fact]
    public void LaGexVivaComeGexPy()
    {
        // una scadenza a 10 giorni, vol piatta 20%: call pesanti sopra (7800), put pesanti sotto (7500)
        double f = 7650, t = 10;
        var q = new List<Quotazione>();
        for (double k = 7300; k <= 8000; k += 50)
        {
            double oiC = k == 7800 ? 5000 : 100, oiP = k == 7500 ? 6000 : 100;
            double pc = Formula.Black(f, k, t / 365, 0.20, true) * Math.Exp(-Formula.Tasso * t / 365);
            double pp = Formula.Black(f, k, t / 365, 0.20, false) * Math.Exp(-Formula.Tasso * t / 365);
            q.Add(new Quotazione(k, true, pc * 0.999, pc * 1.001, pc, oiC));
            q.Add(new Quotazione(k, false, pp * 0.999, pp * 1.001, pp, oiP));
        }
        var sc = new Scadenza("s", Adesso.AddDays(t), q, f);
        var righe = GexViva.Righe(sc, Adesso, f);
        Assert.All(righe, r => Assert.InRange(r.Vol, 0.199, 0.201));
        Assert.Equal(0.20, GexViva.Atm(righe, f)!.Value, 3);
        var p = GexViva.Calcola(righe, f, Adesso, 50);
        Assert.Equal(7800, p.CallWall);
        Assert.Equal(7500, p.PutWall);
        // il totale e' la somma per strike (call positive, put negative)
        Assert.Equal(p.Strike.Sum(x => x.call + x.put), p.Totale, 0);
        // la GEX di uno strike: gamma x OI x 50 x F^2 x 1% / 1e6
        double atteso = Formula.Gamma(f, 7800, t / 365, 0.20) * 5000 * 50 * f * f * 0.01 / 1e6;
        Assert.Equal(atteso, p.Strike.Single(x => x.strike == 7800).call, 1);
        // lo zero gamma sta fra il put wall e il call wall
        Assert.InRange(p.Zero!.Value, 7500, 7800);
    }
}
