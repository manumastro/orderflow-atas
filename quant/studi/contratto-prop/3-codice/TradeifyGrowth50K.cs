namespace Quant.Studi;

/// <summary>Le regole: quant/studi/contratto-prop/2-regole/tradeify-growth-50k.md.</summary>
public static class TradeifyGrowth50K
{
    /// <summary>
    /// Tradeify Growth 50K, regole lette sul centro assistenza e sul sito l'11 ottobre 2026
    /// (quant/studi/contratto-prop/2-regole/tradeify-growth-50k.md).
    ///
    /// Le due pagine ufficiali non concordano sul blocco del pavimento in valutazione: "Rules:
    /// Trailing Max Drawdowns" dice che in valutazione non si blocca, "Essential Trading Rules
    /// Overview" che si blocca a 50.100 $ come nel finanziato. Le due varianti sono due contratti.
    /// </summary>
    public static Contratto Regole(bool bloccoInValutazione) => new()
    {
        Nome = bloccoInValutazione ? "tradeify-growth-50k-blocco" : "tradeify-growth-50k",
        Valutazione =
        [
            new Fase
            {
                Nome = "valutazione",
                Target = 3_000,
                Drawdown = 2_000,
                Blocco = bloccoInValutazione ? 100 : null,
                PavimentoAllaChiusura = true,
                ViolazioneAllaChiusura = false,
                LimiteGiornaliero = 1_250,
                LimiteRigido = false,
            },
        ],
        Finanziata = new Fase
        {
            Nome = "finanziata",
            Drawdown = 2_000,
            Blocco = 100,
            PavimentoAllaChiusura = true,
            ViolazioneAllaChiusura = false,
            LimiteGiornaliero = 1_250,
            LimiteRigido = false,
            LimiteGiornalieroAlzato = 2_000,
            SogliaLimiteAlzato = 3_000,
            Prelievi = new Prelievi
            {
                SaldoMinimo = 3_000,
                GiorniMinimi = 5,
                ProfittoGiorno = 150,
                Consistenza = 0.35,
                Massimi = [1_500, 2_000, 2_500, 3_000],
                Minimo = 500,
                BloccaPavimento = true,
                Quota = 0.90,
            },
        },
        Prezzi = new Prezzi
        {
            Ingresso = 145,
            Reset = 95,
            Listino = 145,
            Nota = "listino 145 $, reset 95 $; con il codice GEMS (fino al 15 ottobre 2026) 102 $",
        },
        Fonte = "help.tradeify.co e tradeify.co, lette l'11 ottobre 2026",
        LetteIl = new DateOnly(2026, 10, 11),
    };
}
