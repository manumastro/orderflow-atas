namespace Quant.Motore;

/// <summary>
/// Le statistiche di una prova. Due famiglie, e la distinzione e' la lezione piu' cara della fase 40R:
/// quelle per operazione descrivono, quelle per giornata misurano. Le operazioni della stessa
/// giornata non sono indipendenti, quindi ogni test di significativita' si fa sul conto giornaliero.
/// </summary>
public sealed record Metriche
{
    public int Operazioni { get; init; }
    public int Giorni { get; init; }
    public int GiorniConOperazioni { get; init; }
    public double Netto { get; init; }
    public double Lordo { get; init; }
    public double Costi { get; init; }
    public double PercVincenti { get; init; }
    public double MediaVincente { get; init; }
    public double MediaPerdente { get; init; }
    public double Payoff { get; init; }
    public double ProfitFactor { get; init; }
    /// <summary>Netto medio per operazione, in dollari.</summary>
    public double Aspettativa { get; init; }
    /// <summary>Netto medio per operazione e per contratto, in tick: confrontabile fra dimensioni diverse.</summary>
    public double TickMediPerContratto { get; init; }
    public double RMedio { get; init; }
    public double MaxDrawdown { get; init; }
    public int DurataMaxDrawdownGiorni { get; init; }
    public int MassimaSerieDiPerdite { get; init; }
    public double SharpeAnnuo { get; init; }
    public double SortinoAnnuo { get; init; }
    /// <summary>Sharpe giornaliero non annualizzato: entra nello Sharpe deflazionato.</summary>
    public double SharpeGiornaliero { get; init; }
    /// <summary>t del netto medio giornaliero: il test onesto, una osservazione per giornata.</summary>
    public double TGiornaliero { get; init; }
    /// <summary>t del netto medio per operazione, come se fossero indipendenti: si stampa per confronto, non si usa.</summary>
    public double TIngenuo { get; init; }
    public double Asimmetria { get; init; }
    /// <summary>Curtosi (non in eccesso: la normale vale 3).</summary>
    public double Curtosi { get; init; }
    public double MediaGiorno { get; init; }
    public double DevGiorno { get; init; }

    public static Metriche Da(Risultato r) => Da(r.Operazioni, r.NettoGiorno);

    public static Metriche Da(IReadOnlyList<Operazione> ops, double[] nettoGiorno)
    {
        var n = ops.Count;
        double vinte = 0, perse = 0, sommaV = 0, sommaP = 0, lordo = 0, costi = 0, tickC = 0, rTot = 0;
        var nR = 0;
        int serie = 0, serieMax = 0;
        foreach (var o in ops)
        {
            lordo += o.Lordo;
            costi += o.Costi;
            tickC += o.Tick;
            if (o.R is { } r) { rTot += r; nR++; }
            if (o.Netto > 0) { vinte++; sommaV += o.Netto; serie = 0; }
            else { perse++; sommaP += o.Netto; serie++; serieMax = Math.Max(serieMax, serie); }
        }
        var netti = ops.Select(o => o.Netto).ToArray();
        var (md, dd) = MediaDev(nettoGiorno);
        var (mo, dO) = MediaDev(netti);
        var negative = nettoGiorno.Where(x => x < 0).ToArray();
        var devGiu = negative.Length > 0 ? Math.Sqrt(nettoGiorno.Sum(x => x < 0 ? x * x : 0) / nettoGiorno.Length) : 0;
        var (ddMax, ddDurata) = Drawdown(nettoGiorno);
        var mediaV = vinte > 0 ? sommaV / vinte : 0;
        var mediaP = perse > 0 ? sommaP / perse : 0;
        var sharpeG = dd > 0 ? md / dd : 0;
        return new Metriche
        {
            Operazioni = n,
            Giorni = nettoGiorno.Length,
            GiorniConOperazioni = ops.Select(o => o.IndiceGiorno).Distinct().Count(),
            Netto = lordo - costi,
            Lordo = lordo,
            Costi = costi,
            PercVincenti = n > 0 ? vinte / n : 0,
            MediaVincente = mediaV,
            MediaPerdente = mediaP,
            Payoff = mediaP < 0 ? mediaV / -mediaP : double.PositiveInfinity,
            ProfitFactor = sommaP < 0 ? sommaV / -sommaP : double.PositiveInfinity,
            Aspettativa = n > 0 ? (lordo - costi) / n : 0,
            TickMediPerContratto = n > 0 ? tickC / n : 0,
            RMedio = nR > 0 ? rTot / nR : double.NaN,
            MaxDrawdown = ddMax,
            DurataMaxDrawdownGiorni = ddDurata,
            MassimaSerieDiPerdite = serieMax,
            SharpeGiornaliero = sharpeG,
            SharpeAnnuo = sharpeG * Math.Sqrt(252),
            SortinoAnnuo = devGiu > 0 ? md / devGiu * Math.Sqrt(252) : 0,
            TGiornaliero = dd > 0 ? md / (dd / Math.Sqrt(nettoGiorno.Length)) : 0,
            TIngenuo = dO > 0 ? mo / (dO / Math.Sqrt(n)) : 0,
            Asimmetria = Momento(nettoGiorno, md, dd, 3),
            Curtosi = Momento(nettoGiorno, md, dd, 4),
            MediaGiorno = md,
            DevGiorno = dd,
        };
    }

    public static (double Media, double Dev) MediaDev(ReadOnlySpan<double> x)
    {
        if (x.Length == 0) return (0, 0);
        double s = 0;
        foreach (var v in x) s += v;
        var m = s / x.Length;
        if (x.Length < 2) return (m, 0);
        double q = 0;
        foreach (var v in x) q += (v - m) * (v - m);
        return (m, Math.Sqrt(q / (x.Length - 1)));
    }

    private static double Momento(double[] x, double m, double d, int k)
    {
        if (x.Length < 3 || d == 0) return k == 4 ? 3 : 0;
        double s = 0;
        foreach (var v in x) s += Math.Pow((v - m) / d, k);
        return s / x.Length;
    }

    /// <summary>Massimo drawdown della curva cumulata (dollari, positivo) e la sua durata in giornate.</summary>
    public static (double Massimo, int Durata) Drawdown(ReadOnlySpan<double> netto)
    {
        double cum = 0, picco = 0, max = 0;
        int dalPicco = 0, durata = 0;
        foreach (var x in netto)
        {
            cum += x;
            if (cum >= picco) { picco = cum; dalPicco = 0; }
            else { dalPicco++; durata = Math.Max(durata, dalPicco); }
            max = Math.Max(max, picco - cum);
        }
        return (max, durata);
    }
}
