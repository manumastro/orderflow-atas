namespace Quant.Validazione;

/// <summary>
/// Generatore pseudo-casuale xoshiro256**: veloce, buono, e soprattutto riproducibile. Ogni
/// simulazione ha il suo seme derivato (seme, indice), quindi il risultato non dipende da quanti
/// core la eseguono ne' in che ordine.
/// </summary>
public struct Caso
{
    private ulong _s0, _s1, _s2, _s3;

    public Caso(ulong seme, ulong flusso = 0)
    {
        var x = seme ^ (flusso * 0x9E3779B97F4A7C15UL + 0xD1B54A32D192ED03UL);
        _s0 = SplitMix(ref x);
        _s1 = SplitMix(ref x);
        _s2 = SplitMix(ref x);
        _s3 = SplitMix(ref x);
    }

    private static ulong SplitMix(ref ulong x)
    {
        var z = x += 0x9E3779B97F4A7C15UL;
        z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
        z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
        return z ^ (z >> 31);
    }

    public ulong Prossimo()
    {
        var r = ulong.RotateLeft(_s1 * 5, 7) * 9;
        var t = _s1 << 17;
        _s2 ^= _s0;
        _s3 ^= _s1;
        _s1 ^= _s2;
        _s0 ^= _s3;
        _s2 ^= t;
        _s3 = ulong.RotateLeft(_s3, 45);
        return r;
    }

    /// <summary>Intero uniforme in [0, n), metodo di Lemire.</summary>
    public int Sotto(int n) => (int)(((Prossimo() >> 32) * (ulong)n) >> 32);

    /// <summary>Reale uniforme in [0, 1).</summary>
    public double Uniforme() => (Prossimo() >> 11) * (1.0 / (1UL << 53));

    /// <summary>Normale standard, Box-Muller.</summary>
    public double Normale()
    {
        var u1 = 1.0 - Uniforme();
        var u2 = Uniforme();
        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
    }

    public void Mescola<T>(Span<T> x)
    {
        for (var i = x.Length - 1; i > 0; i--)
        {
            var j = Sotto(i + 1);
            (x[i], x[j]) = (x[j], x[i]);
        }
    }
}

/// <summary>La normale: distribuzione e inversa, senza librerie.</summary>
public static class Normale
{
    public static double Cdf(double x) => 0.5 * Erfc(-x / Math.Sqrt(2));

    /// <summary>erfc con l'approssimazione di Numerical Recipes (errore relativo &lt; 1.2e-7).</summary>
    private static double Erfc(double x)
    {
        var z = Math.Abs(x);
        var t = 1.0 / (1.0 + 0.5 * z);
        var r = t * Math.Exp(-z * z - 1.26551223 + t * (1.00002368 + t * (0.37409196 + t * (0.09678418 + t * (-0.18628806 +
                t * (0.27886807 + t * (-1.13520398 + t * (1.48851587 + t * (-0.82215223 + t * 0.17087277)))))))));
        return x >= 0 ? r : 2 - r;
    }

    /// <summary>Inversa della normale, algoritmo di Acklam (errore relativo &lt; 1.2e-9).</summary>
    public static double Inversa(double p)
    {
        if (p <= 0) return double.NegativeInfinity;
        if (p >= 1) return double.PositiveInfinity;
        double[] a = [-3.969683028665376e+01, 2.209460984245205e+02, -2.759285104469687e+02, 1.383577518672690e+02, -3.066479806614716e+01, 2.506628277459239e+00];
        double[] b = [-5.447609879822406e+01, 1.615858368580409e+02, -1.556989798598866e+02, 6.680131188771972e+01, -1.328068155288572e+01];
        double[] c = [-7.784894002430293e-03, -3.223964580411365e-01, -2.400758277161838e+00, -2.549732539343734e+00, 4.374664141464968e+00, 2.938163982698783e+00];
        double[] d = [7.784695709041462e-03, 3.224671290700398e-01, 2.445134137142996e+00, 3.754408661907416e+00];
        const double basso = 0.02425;
        if (p < basso)
        {
            var q = Math.Sqrt(-2 * Math.Log(p));
            return (((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) / ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
        }
        if (p > 1 - basso)
        {
            var q = Math.Sqrt(-2 * Math.Log(1 - p));
            return -(((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) / ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
        }
        var u = p - 0.5;
        var r = u * u;
        return (((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * u / (((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1);
    }
}

public static class Percentili
{
    /// <summary>Percentile con interpolazione lineare; l'array viene ordinato.</summary>
    public static double Di(double[] x, double p)
    {
        if (x.Length == 0) return double.NaN;
        Array.Sort(x);
        return Ordinato(x, p);
    }

    public static double Ordinato(double[] x, double p)
    {
        var pos = p * (x.Length - 1);
        var lo = (int)Math.Floor(pos);
        var hi = Math.Min(lo + 1, x.Length - 1);
        return x[lo] + (x[hi] - x[lo]) * (pos - lo);
    }
}
