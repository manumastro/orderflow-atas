namespace Quant.Validazione;

/// <summary>Il drawdown che ci si deve aspettare entro le prime N operazioni: il metro per il vivo.</summary>
public readonly record struct Soglia(int Operazioni, double DrawdownP50, double DrawdownP95, double DrawdownP99);

public sealed record EsitoMonteCarlo
{
    public required string Metodo { get; init; }
    public required int Simulazioni { get; init; }
    public required int Lunghezza { get; init; }
    public double FinaleP05 { get; init; }
    public double FinaleP50 { get; init; }
    public double FinaleP95 { get; init; }
    public double ProbPerdita { get; init; }
    public double DrawdownP50 { get; init; }
    public double DrawdownP95 { get; init; }
    public double DrawdownP99 { get; init; }
    public double SerieDiPerditeP50 { get; init; }
    public double SerieDiPerditeP95 { get; init; }
    /// <summary>Il drawdown della sequenza originale, per vedere dove cade nella distribuzione.</summary>
    public double DrawdownOriginale { get; init; }
    /// <summary>Quota delle simulazioni con drawdown peggiore dell'originale: se e' piccola, l'originale e' stato fortunato.</summary>
    public double QuotaPeggioriDellOriginale { get; init; }
    public Soglia[] Soglie { get; init; } = [];
    /// <summary>Alcune curve cumulate, per il grafico a spaghetti.</summary>
    public double[][] Percorsi { get; init; } = [];
}

/// <summary>
/// Il Monte Carlo del metodo, nelle sue tre forme:
/// <list type="bullet">
/// <item><b>rimescolamento</b>: stesse operazioni, ordine diverso. Il finale non cambia; cambiano
/// drawdown e serie di perdite. Dice quanto il risultato dipendeva dall'ordine.</item>
/// <item><b>ricampionamento</b> (bootstrap): si pescano operazioni con reinserimento. Cambia anche
/// il finale: da' la distribuzione degli esiti e la probabilita' di un drawdown oltre una soglia.</item>
/// <item><b>ricampionamento per giornata</b>: si pescano giornate intere, a blocchi. E' l'aggiunta
/// del framework al metodo del transcript: le operazioni della stessa giornata non sono
/// indipendenti, e pescarle una per una sottostima le code.</item>
/// </list>
/// </summary>
public static class MonteCarlo
{
    public static readonly int[] PuntiSoglia = [10, 20, 50, 100];

    public static EsitoMonteCarlo Rimescola(double[] netti, int simulazioni = 10_000, ulong seme = 1) =>
        Esegui("rimescolamento delle operazioni", netti, simulazioni, seme, (ref Caso rng, double[] origine, double[] dest) =>
        {
            origine.CopyTo(dest);
            rng.Mescola(dest.AsSpan());
        });

    public static EsitoMonteCarlo Ricampiona(double[] netti, int simulazioni = 10_000, ulong seme = 1) =>
        Esegui("ricampionamento delle operazioni", netti, simulazioni, seme, (ref Caso rng, double[] origine, double[] dest) =>
        {
            for (var k = 0; k < dest.Length; k++) dest[k] = origine[rng.Sotto(origine.Length)];
        });

    /// <summary>Ricampionamento a blocchi di giornate consecutive (blocco 1 = giornate indipendenti).</summary>
    public static EsitoMonteCarlo RicampionaGiorni(double[] nettoGiorno, int simulazioni = 10_000, ulong seme = 1, int blocco = 5) =>
        Esegui($"ricampionamento per giornata, blocchi di {blocco}", nettoGiorno, simulazioni, seme, (ref Caso rng, double[] origine, double[] dest) =>
        {
            var k = 0;
            while (k < dest.Length)
            {
                var inizio = rng.Sotto(Math.Max(1, origine.Length - blocco + 1));
                for (var j = 0; j < blocco && k < dest.Length; j++) dest[k++] = origine[inizio + j];
            }
        });

    private delegate void Generatore(ref Caso rng, double[] origine, double[] dest);

    private static EsitoMonteCarlo Esegui(string metodo, double[] origine, int simulazioni, ulong seme, Generatore genera)
    {
        var n = origine.Length;
        if (n == 0) return new EsitoMonteCarlo { Metodo = metodo, Simulazioni = 0, Lunghezza = 0 };
        var finali = new double[simulazioni];
        var drawdown = new double[simulazioni];
        var serie = new double[simulazioni];
        var soglie = PuntiSoglia.Where(s => s <= n).ToArray();
        var ddSoglie = soglie.Select(_ => new double[simulazioni]).ToArray();
        const int quantiPercorsi = 60;
        var percorsi = new double[Math.Min(quantiPercorsi, simulazioni)][];

        Parallel.For(0, simulazioni, () => new double[n], (s, _, buf) =>
        {
            var rng = new Caso(seme, (ulong)s);
            genera(ref rng, origine, buf);
            double cum = 0, picco = 0, dd = 0;
            int perdite = 0, maxPerdite = 0, prossimaSoglia = 0;
            double[]? percorso = s < percorsi.Length ? new double[n] : null;
            for (var k = 0; k < n; k++)
            {
                cum += buf[k];
                if (cum > picco) picco = cum;
                if (picco - cum > dd) dd = picco - cum;
                if (buf[k] < 0) { perdite++; if (perdite > maxPerdite) maxPerdite = perdite; } else perdite = 0;
                if (percorso is not null) percorso[k] = cum;
                if (prossimaSoglia < soglie.Length && k + 1 == soglie[prossimaSoglia]) ddSoglie[prossimaSoglia++][s] = dd;
            }
            finali[s] = cum;
            drawdown[s] = dd;
            serie[s] = maxPerdite;
            if (percorso is not null) percorsi[s] = percorso;
            return buf;
        }, _ => { });

        var (ddOrig, _) = Statistiche.Metriche.Drawdown(origine);
        var peggiori = drawdown.Count(d => d >= ddOrig) / (double)simulazioni;
        Array.Sort(finali);
        Array.Sort(drawdown);
        Array.Sort(serie);
        return new EsitoMonteCarlo
        {
            Metodo = metodo, Simulazioni = simulazioni, Lunghezza = n,
            FinaleP05 = Percentili.Ordinato(finali, 0.05), FinaleP50 = Percentili.Ordinato(finali, 0.50), FinaleP95 = Percentili.Ordinato(finali, 0.95),
            ProbPerdita = finali.Count(f => f < 0) / (double)simulazioni,
            DrawdownP50 = Percentili.Ordinato(drawdown, 0.50), DrawdownP95 = Percentili.Ordinato(drawdown, 0.95), DrawdownP99 = Percentili.Ordinato(drawdown, 0.99),
            SerieDiPerditeP50 = Percentili.Ordinato(serie, 0.50), SerieDiPerditeP95 = Percentili.Ordinato(serie, 0.95),
            DrawdownOriginale = ddOrig, QuotaPeggioriDellOriginale = peggiori,
            Soglie = [.. soglie.Select((s, j) =>
            {
                var x = ddSoglie[j];
                Array.Sort(x);
                return new Soglia(s, Percentili.Ordinato(x, 0.50), Percentili.Ordinato(x, 0.95), Percentili.Ordinato(x, 0.99));
            })],
            Percorsi = percorsi,
        };
    }
}
