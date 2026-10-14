namespace Quant.Validazione.Contratti;

/// <summary>
/// La forma chiusa di Tomas Fernandez (2026), "Valuing Proprietary Trading Firm Evaluation Contracts",
/// ricontrollata formula per formula sul PDF. Serve a due cose: a verificare il simulatore
/// (criteri sigillati C1, C2, C4) e a valutare un contratto senza simulare.
///
/// Unita': dollari e sessioni. d e' la deviazione standard del P&amp;L di una sessione, mu la deriva
/// per sessione; theta = 2 mu / d^2 (TF §2.3).
/// </summary>
public static class FormaChiusa
{
    /// <summary>beta = -zeta(1/2) / sqrt(2 pi), la correzione di Broadie, Glasserman e Kou (TF eq. 5).</summary>
    public const double Beta = 0.5826;

    /// <summary>Deviazione standard del saldo fra due controlli: s = d / sqrt(m) (TF eq. 4, n gia' dentro d).</summary>
    public static double S(double d, double controlliPerSessione) => d / Math.Sqrt(controlliPerSessione);

    /// <summary>
    /// Probabilita' di passare una fase (TF eq. 3 corretta con eq. 5).
    /// </summary>
    /// <param name="kappa">Livello di blocco del pavimento; null = il pavimento sale per tutta la fase.</param>
    /// <param name="sM">Deviazione standard fra due aggiornamenti del pavimento (0 = continuo).</param>
    /// <param name="sB">Deviazione standard fra due controlli della violazione (0 = continuo).</param>
    public static double Passaggio(double T, double D, double? kappa, double mu, double d, double sM, double sB)
    {
        var k = kappa ?? T; // un blocco oltre T - D equivale a nessun blocco
        var L = Math.Min(D + k, T);
        var R = T - D - k;
        var d1 = D + Beta * Math.Max(sM, sB);
        var d2 = D + Beta * sB;
        var theta = d > 0 ? 2 * mu / (d * d) : 0;

        double p1, p2 = 1;
        if (Math.Abs(theta) < 1e-12)
        {
            p1 = Math.Exp(-L / d1);
            if (R > 0) p2 = d2 / (d2 + R);
        }
        else
        {
            p1 = Math.Exp(-theta * L / Expm1(theta * d1));
            if (R > 0) p2 = Expm1(theta * d2) / (Math.Exp(theta * d2) - Math.Exp(-theta * R));
        }
        return L <= 0 ? p2 : p1 * p2;
    }

    /// <summary>Sessioni attese per risolvere una fase senza deriva (TF eq. 8): N = Nbar D^2 / d^2.</summary>
    public static double Durata(double T, double D, double? kappa, double d)
    {
        var k = kappa ?? T;
        var lambda = Math.Min(D + k, T) / D;
        var R = T - D - k;
        var nbar = -Expm1(-lambda) + (R > 0 ? Math.Exp(-lambda) * R / D : 0);
        return nbar * D * D / (d * d);
    }

    /// <summary>Costo atteso per ottenere un conto finanziato, a prezzo per valutazione (TF eq. 9).</summary>
    public static double Costo(double p, double ingresso, double reset, double attivazione = 0) =>
        ingresso + (1 / p - 1) * reset + attivazione;

    /// <summary>
    /// Il tasso di conversione di pareggio, PA = c_r / (c_r + F - c_e) (TF §3.6 e §5.3): sotto questa
    /// probabilita' di passare la valutazione non vale il prezzo, ed e' anche il punto in cui la firma
    /// smette di guadagnare.
    /// </summary>
    public static double Pareggio(double ingresso, double reset, double valoreConto) =>
        reset / (reset + valoreConto - ingresso);

    /// <summary>La deriva per sessione che da' la probabilita' <paramref name="p"/> (TF §5.6), per bisezione.</summary>
    public static double DerivaImplicita(double p, double T, double D, double? kappa, double d, double sM, double sB)
    {
        double basso = -2 * d, alto = 2 * d;
        for (var i = 0; i < 200; i++)
        {
            var mezzo = 0.5 * (basso + alto);
            if (Passaggio(T, D, kappa, mezzo, d, sM, sB) < p) basso = mezzo; else alto = mezzo;
        }
        return 0.5 * (basso + alto);
    }

    /// <summary>e^x - 1 senza perdere cifre per x piccolo.</summary>
    private static double Expm1(double x) => Math.Abs(x) < 1e-5 ? x + 0.5 * x * x + x * x * x / 6 : Math.Exp(x) - 1;
}
