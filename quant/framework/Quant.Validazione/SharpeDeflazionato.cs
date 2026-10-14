namespace Quant.Validazione;

/// <summary>
/// Lo Sharpe corretto per quante varianti si sono provate (Bailey e Lopez de Prado, "The Deflated
/// Sharpe Ratio", 2014). Provando cento varianti, la migliore ha uno Sharpe alto anche se nessuna
/// ha un vantaggio: il massimo di cento estrazioni casuali non e' zero. Lo Sharpe deflazionato e'
/// la probabilita' che lo Sharpe vero superi quel massimo atteso dal caso.
///
/// Tutto in unita' giornaliere: Sharpe per giornata, T giornate.
/// </summary>
public static class SharpeDeflazionato
{
    private const double EuleroMascheroni = 0.5772156649015329;

    /// <summary>
    /// Probabilistic Sharpe Ratio: probabilita' che lo Sharpe vero superi <paramref name="riferimento"/>,
    /// tenendo conto di quante osservazioni ci sono e di asimmetria e curtosi (non in eccesso).
    /// </summary>
    public static double Probabilistico(double sharpe, int t, double asimmetria, double curtosi, double riferimento = 0)
    {
        if (t < 2) return double.NaN;
        var denominatore = 1 - asimmetria * sharpe + (curtosi - 1) / 4 * sharpe * sharpe;
        if (denominatore <= 0) return double.NaN;
        return Normale.Cdf((sharpe - riferimento) * Math.Sqrt(t - 1) / Math.Sqrt(denominatore));
    }

    /// <summary>
    /// Lo Sharpe massimo atteso dal caso dopo <paramref name="prove"/> varianti indipendenti, con
    /// varianza degli Sharpe fra le varianti <paramref name="varianzaSharpe"/>.
    /// </summary>
    public static double MassimoAtteso(int prove, double varianzaSharpe)
    {
        if (prove < 2) return 0;
        var n = (double)prove;
        return Math.Sqrt(varianzaSharpe) * ((1 - EuleroMascheroni) * Normale.Inversa(1 - 1 / n)
                                            + EuleroMascheroni * Normale.Inversa(1 - 1 / (n * Math.E)));
    }

    /// <summary>
    /// Lo Sharpe deflazionato. Se la varianza fra le varianti non e' nota (meno di due prove nel
    /// registro), si usa quella dello stimatore sotto l'ipotesi nulla, 1/T: una scelta dichiarata.
    /// </summary>
    public static double Calcola(double sharpe, int t, double asimmetria, double curtosi, int prove, double? varianzaSharpe)
    {
        var v = varianzaSharpe is > 0 ? varianzaSharpe.Value : 1.0 / Math.Max(2, t);
        return Probabilistico(sharpe, t, asimmetria, curtosi, MassimoAtteso(prove, v));
    }
}
