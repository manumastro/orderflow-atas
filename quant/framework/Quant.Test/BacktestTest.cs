using static Quant.Test.Costruttore;

namespace Quant.Test;

public class BacktestTest
{
    private static readonly Esecuzione Zero = Esecuzione.SenzaCosti;
    private static int T(double punti) => Strumento.NQ.InTick(punti);

    [Fact]
    public void IlMercatoSiEsegueAllAperturaDellaBarraDopo()
    {
        var serie = Giornata(Lunedi, (100, 101, 99, 100), (102, 103, 101, 102), (103, 104, 102, 103));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long });
        var r = Backtest.Esegui(serie, s, new Parametri(), Zero);
        var o = Assert.Single(r.Operazioni);
        Assert.Equal(T(102), o.PrezzoIngresso);
        Assert.Equal(1, o.IndiceIngresso);
        Assert.Equal(MotivoUscita.FineGiornata, o.MotivoUscita);
        Assert.Equal(T(103), o.PrezzoUscita);
        Assert.Equal(20.0, o.Lordo, 6);
    }

    [Fact]
    public void LoSlittamentoELeCommissioniPeggioranoEntrambiILati()
    {
        var serie = Giornata(Lunedi, (100, 101, 99, 100), (102, 103, 101, 102), (103, 104, 102, 103));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long });
        var r = Backtest.Esegui(serie, s, new Parametri(), new Esecuzione { CommissioneLato = 2.5, SlittamentoTick = 1 });
        var o = Assert.Single(r.Operazioni);
        Assert.Equal(T(102.25), o.PrezzoIngresso);
        Assert.Equal(T(102.75), o.PrezzoUscita);
        Assert.Equal(10.0, o.Lordo, 6);
        Assert.Equal(5.0, o.Costi, 6);
        Assert.Equal(5.0, o.Netto, 6);
    }

    [Fact]
    public void StopETargetNellaStessaBarraVinceLoStop()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (100, 100, 100, 100), (100, 110, 90, 100), (100, 100, 100, 100));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long, StopDistanza = T(5), TargetDistanza = T(5) });
        var r = Backtest.Esegui(serie, s, new Parametri(), Zero);
        var o = Assert.Single(r.Operazioni);
        Assert.Equal(MotivoUscita.Stop, o.MotivoUscita);
        Assert.Equal(T(95), o.PrezzoUscita);
    }

    [Fact]
    public void SullaBarraDellIngressoIlTargetNonConta()
    {
        // il target e' dentro la barra dell'eseguito: non si sa se prima o dopo, non si regala
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (100, 110, 99, 101), (101, 101, 101, 101));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long, StopDistanza = T(5), TargetDistanza = T(5) });
        var r = Backtest.Esegui(serie, s, new Parametri(), Zero);
        Assert.Equal(MotivoUscita.FineGiornata, Assert.Single(r.Operazioni).MotivoUscita);
    }

    [Fact]
    public void IlLimiteToccatoSenzaAttraversareNonSiEsegue()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (100, 100, 98, 99), (99, 100, 97.75, 99), (99, 99, 99, 99));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long, Tipo = TipoOrdine.Limite, Prezzo = T(98) });
        var r = Backtest.Esegui(serie, s, new Parametri(), Zero);
        var o = Assert.Single(r.Operazioni);
        Assert.Equal(2, o.IndiceIngresso); // la barra 1 tocca 98 e basta; la barra 2 lo attraversa
        Assert.Equal(T(98), o.PrezzoIngresso);
    }

    [Fact]
    public void LoStopInGapSiEsegueAllApertura()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (100, 100, 100, 100), (90, 91, 89, 90), (90, 90, 90, 90));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long, StopDistanza = T(5) });
        var r = Backtest.Esegui(serie, s, new Parametri(), Zero);
        var o = Assert.Single(r.Operazioni);
        Assert.Equal(MotivoUscita.Stop, o.MotivoUscita);
        Assert.Equal(T(90), o.PrezzoUscita);
    }

    [Fact]
    public void LUscitaATempoAvvieneAllAperturaDellaPrimaBarraDopoLOra()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (100, 100, 100, 100), (101, 101, 101, 101), (102, 102, 102, 102));
        var s = new Comandata(0, c => new Ordine { Lato = Lato.Long, UscitaAlle = c.OggiAlle(Calendario.AperturaCassa + 2) });
        var r = Backtest.Esegui(serie, s, new Parametri(), Zero);
        var o = Assert.Single(r.Operazioni);
        Assert.Equal(MotivoUscita.Tempo, o.MotivoUscita);
        Assert.Equal(T(101), o.PrezzoUscita);
    }

    [Fact]
    public void LaStrategiaNonPuoLeggereIlFuturo()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (101, 101, 101, 101));
        var s = new Comandata(5, _ => new Ordine { Lato = Lato.Long }) { Ogni = c => c.Indietro(-1) };
        Assert.Throws<InvalidOperationException>(() => Backtest.Esegui(serie, s, new Parametri(), Zero));
        var vista = 0;
        var t = new Comandata(5, _ => new Ordine { Lato = Lato.Long }) { Ogni = c => vista = Math.Max(vista, c.Storia.Length) };
        Backtest.Esegui(serie, t, new Parametri(), Zero);
        Assert.Equal(2, vista);
    }

    [Fact]
    public void IlRischioFissoDimensionaSulloStop()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (100, 100, 100, 100), (100, 100, 100, 100));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long, StopDistanza = T(10) });
        var p = Parametri.Da(["dimensione=rischio", "rischio=1000"]);
        var r = Backtest.Esegui(serie, s, p, Zero);
        Assert.Equal(5, Assert.Single(r.Operazioni).Contratti); // 1000 $ / (10 punti x 20 $)
    }

    [Fact]
    public void UnParametroSconosciutoEUnErrore()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Long });
        Assert.Throws<ArgumentException>(() => Backtest.Esegui(serie, s, Parametri.Da(["refuso=1"]), Zero));
    }

    [Fact]
    public void IlNettoGiornalieroSommaLeOperazioni()
    {
        var serie = Giornata(Lunedi, (100, 100, 100, 100), (100, 100, 100, 100), (105, 105, 105, 105));
        var s = new Comandata(0, _ => new Ordine { Lato = Lato.Short });
        var r = Backtest.Esegui(serie, s, new Parametri(), new Esecuzione { CommissioneLato = 2.5, SlittamentoTick = 0 });
        Assert.Equal(r.Operazioni.Sum(o => o.Netto), r.NettoGiorno.Sum(), 6);
        Assert.Equal(-105.0, r.NettoGiorno[0], 6);
    }
}
