namespace Quant.Studi;

/// <summary>Il contratto stilizzato di Lim (2026): quant/paper/prop-firm/lim-2026-price-of-a-funded-account/analisi.md.</summary>
public static class LimClassico
{
    /// <summary>
    /// Il contratto stilizzato di Lim (Lim, Tab. 1): conto da 100.000 $, due passi (+10% e +5%),
    /// perdita massima -10% statica, perdita giornaliera -5% rigida, scadenze 30 e 60 sessioni,
    /// controllo alle chiusure giornaliere, prelievo ogni 20 sessioni con azzeramento, 80% al trader,
    /// 500 $ rimborsati al primo prelievo, orizzonte finanziato di 250 sessioni.
    /// </summary>
    public static Contratto Regole(bool scadenza) => new()
    {
        Nome = scadenza ? "lim-classico" : "lim-senza-scadenza",
        Valutazione =
        [
            FaseLim("passo 1", 10_000, scadenza ? 30 : 300),
            FaseLim("passo 2", 5_000, scadenza ? 60 : 300),
        ],
        Finanziata = FaseLim("finanziata", null, null) with
        {
            Prelievi = new Prelievi { Intervallo = 20, Quota = 0.80 },
        },
        Prezzi = new Prezzi { Ingresso = 500, Reset = 500, RimborsoAlPrimoPrelievo = true },
        Fonte = "Lim (2026), Tab. 1: archetipo stilizzato, non una firma",
        LetteIl = new DateOnly(2026, 7, 25),
    };

    private static Fase FaseLim(string nome, double? target, int? scadenza) => new()
    {
        Nome = nome,
        Target = target,
        Drawdown = 10_000,
        Blocco = -10_000,
        ViolazioneAllaChiusura = true,
        LimiteGiornaliero = 5_000,
        LimiteRigido = true,
        Scadenza = scadenza,
    };
}
