using Quant.Motore;

namespace Quant.Strategie;

/// <summary>Le strategie per nome. Una strategia nuova si aggiunge qui e ha la sua scheda in quant/idee/.</summary>
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
