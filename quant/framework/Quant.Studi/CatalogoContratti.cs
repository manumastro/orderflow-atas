namespace Quant.Studi;

/// <summary>
/// I contratti programmati, per nome. Ogni contratto ha il suo file nello studio che lo usa, e le regole
/// di una firma vera hanno la loro pagina, con le fonti ufficiali e la data di lettura:
/// quant/studi/<studio>/2-regole/*.md; il codice del contratto sta in quant/studi/<studio>/3-codice/.
/// </summary>
public static class CatalogoContratti
{
    private static readonly Dictionary<string, Func<Contratto>> Fabbriche = new(StringComparer.OrdinalIgnoreCase)
    {
        ["tradeify-growth-50k"] = () => TradeifyGrowth50K.Regole(bloccoInValutazione: false),
        ["tradeify-growth-50k-blocco"] = () => TradeifyGrowth50K.Regole(bloccoInValutazione: true),
        ["lim-classico"] = () => LimClassico.Regole(scadenza: true),
        ["lim-senza-scadenza"] = () => LimClassico.Regole(scadenza: false),
    };

    public static IEnumerable<string> Nomi => Fabbriche.Keys.Order();

    public static Contratto Da(string nome) =>
        Fabbriche.TryGetValue(nome, out var f) ? f()
            : throw new ArgumentException($"contratto sconosciuto: {nome}. Disponibili: {string.Join(", ", Nomi)}");
}
