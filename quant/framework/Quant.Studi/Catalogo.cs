namespace Quant.Studi;

/// <summary>Le strategie per nome. Una strategia nuova si aggiunge qui e ha il suo studio in quant/studi/<nome>/: la scheda in 1-idea.md, il codice in 3-codice/.</summary>
public static class Catalogo
{
    private static readonly Dictionary<string, Func<Strategia>> Fabbriche = new(StringComparer.OrdinalIgnoreCase)
    {
        ["momento-intraday"] = () => new MomentoIntraday(),
        ["vwap-tendenza"] = () => new VwapTendenza(),
        ["anomalia-notte"] = () => new AnomaliaNotte(),
    };

    public static IEnumerable<string> Nomi => Fabbriche.Keys.Order();

    public static Func<Strategia> Fabbrica(string nome) =>
        Fabbriche.TryGetValue(nome, out var f) ? f
            : throw new ArgumentException($"strategia sconosciuta: {nome}. Disponibili: {string.Join(", ", Nomi)}");
}
