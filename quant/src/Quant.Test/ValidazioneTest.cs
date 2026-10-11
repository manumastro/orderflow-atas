using Quant.Statistiche;
using Quant.Validazione;

namespace Quant.Test;

public class ValidazioneTest
{
    [Fact]
    public void IlDrawdownEDalPiccoAllaValle()
    {
        var (dd, durata) = Metriche.Drawdown([100, -50, -80, 60, 200, -10]);
        Assert.Equal(130, dd, 6);
        Assert.Equal(3, durata);
    }

    [Fact]
    public void IlRimescolamentoNonCambiaIlFinale()
    {
        var netti = Enumerable.Range(0, 200).Select(i => i % 3 == 0 ? -100.0 : 80.0).ToArray();
        var mc = MonteCarlo.Rimescola(netti, 2000, 1);
        Assert.Equal(netti.Sum(), mc.FinaleP05, 6);
        Assert.Equal(netti.Sum(), mc.FinaleP95, 6);
        Assert.True(mc.DrawdownP95 >= mc.DrawdownP50);
    }

    [Fact]
    public void IlMonteCarloERiproducibileEIndipendenteDaiCore()
    {
        var netti = Enumerable.Range(0, 300).Select(i => Math.Sin(i) * 100).ToArray();
        var a = MonteCarlo.Ricampiona(netti, 5000, 9);
        var b = MonteCarlo.Ricampiona(netti, 5000, 9);
        Assert.Equal(a.FinaleP50, b.FinaleP50);
        Assert.Equal(a.DrawdownP99, b.DrawdownP99);
        Assert.Equal(a.Soglie, b.Soglie);
    }

    [Fact]
    public void LaNormaleELaSuaInversa()
    {
        Assert.Equal(0.5, Normale.Cdf(0), 7);
        Assert.Equal(0.975, Normale.Cdf(1.959964), 6);
        foreach (var p in new[] { 0.001, 0.05, 0.3, 0.5, 0.9, 0.999 })
            Assert.Equal(p, Normale.Cdf(Normale.Inversa(p)), 6);
    }

    [Fact]
    public void LoSharpeDeflazionatoCalaConLeVarianti()
    {
        // stesso Sharpe, piu' varianti provate: meno credibile
        var una = SharpeDeflazionato.Calcola(0.1, 500, 0, 3, 1, null);
        var cento = SharpeDeflazionato.Calcola(0.1, 500, 0, 3, 100, null);
        Assert.True(una > 0.98);
        Assert.True(cento < una);
        // Sharpe zero con una variante: meta' e meta'
        Assert.Equal(0.5, SharpeDeflazionato.Probabilistico(0, 500, 0, 3), 6);
    }

    [Fact]
    public void IlGeneratoreECorretto()
    {
        var rng = new Caso(1);
        var conteggi = new int[10];
        for (var i = 0; i < 100_000; i++) conteggi[rng.Sotto(10)]++;
        Assert.All(conteggi, c => Assert.InRange(c, 9500, 10500));
        double s = 0, q = 0;
        for (var i = 0; i < 100_000; i++) { var z = rng.Normale(); s += z; q += z * z; }
        Assert.InRange(s / 100_000, -0.02, 0.02);
        Assert.InRange(q / 100_000, 0.97, 1.03);
    }

    [Fact]
    public void IlTPerGiornataEIlTestNonQuelloPerOperazione()
    {
        // dieci operazioni identiche nella stessa giornata non sono dieci prove
        var giorni = new[] { 50.0, -40, 60, -30, 20 };
        var m = Metriche.Da([], giorni);
        Assert.Equal(5, m.Giorni);
        var (media, dev) = Metriche.MediaDev(giorni);
        Assert.Equal(media / (dev / Math.Sqrt(5)), m.TGiornaliero, 9);
    }
}
