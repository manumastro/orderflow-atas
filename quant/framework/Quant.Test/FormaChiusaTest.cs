namespace Quant.Test;

/// <summary>La forma chiusa riproduce i numeri stampati in Tomas Fernandez (2026).</summary>
public class FormaChiusaTest
{
    [Theory]
    [InlineData(-2000.0, 0.400)] // fisso
    [InlineData(0.0, 0.245)]     // si ferma al saldo iniziale
    [InlineData(100.0, 0.241)]   // si ferma a +100
    public void I_regimi_del_pavimento_di_TF_2_3(double kappa, double atteso) =>
        Assert.Equal(atteso, FormaChiusa.Passaggio(3000, 2000, kappa, 0, 850, 0, 0), 3);

    [Fact]
    public void Il_pavimento_che_sale_sempre_TF_2_3() =>
        Assert.Equal(0.223, FormaChiusa.Passaggio(3000, 2000, null, 0, 850, 0, 0), 3);

    [Theory]
    [InlineData(96, 96, 0.2543)]   // pavimento intraday, violazione intraday
    [InlineData(850, 96, 0.3018)]  // alla chiusura, intraday
    [InlineData(850, 850, 0.3203)] // alla chiusura, alla chiusura
    [InlineData(96, 850, 0.3203)]  // intraday, alla chiusura
    [InlineData(321, 96, 0.2696)]  // oraria, intraday
    public void Il_monitoraggio_discreto_di_TF_2_4(double sM, double sB, double atteso) =>
        Assert.Equal(atteso, FormaChiusa.Passaggio(3000, 2000, 0, 0, 850, sM, sB), 3);

    [Fact]
    public void Tradeify_Growth_secondo_TF_4_2() =>
        Assert.Equal(0.300, FormaChiusa.Passaggio(3000, 2000, 100, 0, 850, 850, 96), 3);

    [Fact]
    public void Il_costo_e_il_pareggio_del_caso_di_studio_TF_8_5()
    {
        Assert.Equal(338, FormaChiusa.Costo(0.300, 107, 99), 0);
        Assert.Equal(0.166, FormaChiusa.Pareggio(107, 99, 606), 3);
    }

    [Fact]
    public void Senza_deriva_le_due_forme_coincidono_al_limite()
    {
        var zero = FormaChiusa.Passaggio(3000, 2000, 100, 0, 850, 850, 96);
        var quasi = FormaChiusa.Passaggio(3000, 2000, 100, 1e-6, 850, 850, 96);
        Assert.Equal(zero, quasi, 6);
    }

    [Fact]
    public void La_durata_con_pavimento_fisso_e_T_su_D() =>
        // pavimento fisso: E[N] = D T / d^2, cioe' Nbar = T / D
        Assert.Equal(2000.0 * 3000 / (850 * 850), FormaChiusa.Durata(3000, 2000, -2000, 850), 6);
}
