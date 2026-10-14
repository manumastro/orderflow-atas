namespace Quant.Studi;

/// <summary>
/// Le 14 politiche dichiarate nella scheda prima di provarle (quant/studi/politica-per-fase/1-idea.md):
/// 12 politiche per fase, scala in valutazione 1, 2, 4 per quattro condotte nel finanziato, e le tre
/// dimensioni fisse come termine di confronto. La scala e' rispetto alle sessioni della sorgente (d = 850 $ di riferimento).
/// </summary>
public static class PolitichePerFase
{
    private static readonly (string Nome, Condotta Condotta)[] Finanziato =
    [
        ("f0", Condotta.Neutra),
        ("f1", new Condotta { Scala = 0.5 }),
        ("f2", new Condotta { StopGiorno = 400, TargetGiorno = 400 }),
        ("f3", new Condotta { StopGiorno = 600, TargetGiorno = 300 }),
    ];

    public static IEnumerable<Politica> Tutte()
    {
        foreach (var s in new[] { 1, 2, 4 })
            yield return new Politica($"fissa-{s}", new Condotta { Scala = s }, new Condotta { Scala = s });
        foreach (var s in new[] { 1, 2, 4 })
            foreach (var (nome, condotta) in Finanziato)
                yield return new Politica($"v{s}-{nome}", new Condotta { Scala = s }, condotta);
    }
}
