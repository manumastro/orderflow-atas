namespace Quant.Validazione.Contratti;

/// <summary>
/// I criteri sigillati di quant/studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop.md (sha256
/// 93721e09...), eseguiti cosi' come sono scritti. Il simulatore si valida qui prima di dire
/// qualcosa su un contratto vero: e' l'obbligo 5 del metodo applicato al contratto.
/// </summary>
public static class VerificaContratto
{
    public sealed record Riga(string Criterio, string Caso, double Atteso, double Misurato, double Scarto, bool Passa);

    private const int Tentativi = 200_000;

    public static List<Riga> Esegui(ulong seme = 20261011)
    {
        var righe = new List<Riga>();
        righe.AddRange(C1(seme));
        righe.AddRange(C2(seme));
        righe.AddRange(C3(seme));
        righe.AddRange(C4());
        return righe;
    }

    /// <summary>C1: le cinque combinazioni di monitoraggio di TF §2.4, 77 passi per sessione.</summary>
    private static IEnumerable<Riga> C1(ulong seme)
    {
        const double T = 3000, D = 2000, d = 850;
        const int m = 77;
        var sPasso = FormaChiusa.S(d, m);
        var sOra = FormaChiusa.S(d, 7);
        var casi = new (string Nome, Fase Fase, double sM, double sB)[]
        {
            ("pavimento intraday, violazione intraday", Fase(T, D, 0) with { PavimentoAllaChiusura = false }, sPasso, sPasso),
            ("pavimento alla chiusura, violazione intraday", Fase(T, D, 0), d, sPasso),
            ("pavimento alla chiusura, violazione alla chiusura", Fase(T, D, 0) with { ViolazioneAllaChiusura = true }, d, d),
            ("pavimento intraday, violazione alla chiusura", Fase(T, D, 0) with { PavimentoAllaChiusura = false, ViolazioneAllaChiusura = true }, sPasso, d),
            ("pavimento orario, violazione intraday", Fase(T, D, 0) with { PavimentoOgniPassi = 11 }, sOra, sPasso),
        };
        foreach (var (nome, fase, sM, sB) in casi)
        {
            var atteso = FormaChiusa.Passaggio(T, D, 0, 0, d, sM, sB);
            var misurato = Probabilita(fase, new SessioniBrowniane(d, 0, m), Tentativi, seme);
            var scarto = (misurato - atteso) / atteso;
            yield return new Riga("C1", nome, atteso, misurato, scarto, Math.Abs(scarto) <= 0.015);
        }
    }

    /// <summary>C2: i quattro regimi di TF §2.3 con monitoraggio fine (390 controlli per sessione).</summary>
    private static IEnumerable<Riga> C2(ulong seme)
    {
        const double T = 3000, D = 2000, d = 850;
        const int m = 390;
        var s = FormaChiusa.S(d, m);
        foreach (var (nome, kappa) in new (string, double?)[] { ("fisso", -D), ("bloccato a 0", 0), ("bloccato a +100", 100), ("sale sempre", null) })
        {
            var fase = Fase(T, D, kappa) with { PavimentoAllaChiusura = false };
            var atteso = FormaChiusa.Passaggio(T, D, kappa, 0, d, s, s);
            var misurato = Probabilita(fase, new SessioniBrowniane(d, 0, m), Tentativi, seme);
            var scarto = (misurato - atteso) / atteso;
            yield return new Riga("C2", nome, atteso, misurato, scarto, Math.Abs(scarto) <= 0.015);
        }
    }

    /// <summary>C3: la tabella 2 di Lim, colonna Sharpe netto 0, volatilita' giornaliera 2%.</summary>
    private static IEnumerable<Riga> C3(ulong seme)
    {
        var baseLim = new Fase
        {
            Nome = "passo 1", Target = 10_000, Drawdown = 10_000, Blocco = -10_000, ViolazioneAllaChiusura = true,
        };
        var giornaliero = baseLim with { LimiteGiornaliero = 5_000, LimiteRigido = true };
        var casi = new (string, Fase, double)[]
        {
            ("solo barriere", baseLim, 0.500),
            ("+ regola giornaliera", giornaliero, 0.453),
            ("+ scadenza 30 sessioni", baseLim with { Scadenza = 30 }, 0.310),
            ("entrambe (contratto base)", giornaliero with { Scadenza = 30 }, 0.298),
            ("regola giornaliera, senza scadenza (tetto 300)", giornaliero with { Scadenza = 300 }, 0.453),
        };
        foreach (var (nome, fase, atteso) in casi)
        {
            var misurato = Probabilita(fase, new SessioniBrowniane(2_000, 0, 1), Tentativi, seme);
            var scarto = misurato - atteso;
            yield return new Riga("C3", nome, atteso, misurato, scarto, Math.Abs(scarto) <= 0.006);
        }
    }

    /// <summary>C4: la deriva implicita di TF §5.6 nel caso di studio.</summary>
    private static IEnumerable<Riga> C4()
    {
        const double d = 850;
        var sB = FormaChiusa.S(d, 78);
        foreach (var (p, atteso) in new[] { (0.14, -127.0), (0.10, -189.0), (0.07, -249.0), (0.05, -303.0) })
        {
            var mu = FormaChiusa.DerivaImplicita(p, 3000, 2000, 100, d, d, sB);
            yield return new Riga("C4", $"P = {p:P0}", atteso, mu, mu - atteso, Math.Abs(mu - atteso) <= 5);
        }
    }

    private static Fase Fase(double T, double D, double? kappa) => new() { Nome = "fase", Target = T, Drawdown = D, Blocco = kappa };

    /// <summary>Probabilita' di passare una fase sola, su tentativi indipendenti.</summary>
    public static double Probabilita(Fase fase, ISorgenteSessioni sorgente, int tentativi, ulong seme)
    {
        var passati = 0;
        Parallel.For(0, tentativi,
            () => (new double[sorgente.MassimoPassi], new double[sorgente.MassimoPassi], 0),
            (i, _, stato) =>
            {
                var rng = new Caso(seme, (ulong)i);
                var (fine, _) = SimulatoreContratto.Valutazione(fase, sorgente.Apri(), ref rng, stato.Item1, stato.Item2);
                return (stato.Item1, stato.Item2, stato.Item3 + (fine == FineFase.Target ? 1 : 0));
            },
            stato => Interlocked.Add(ref passati, stato.Item3));
        return (double)passati / tentativi;
    }
}
