namespace Quant.Dati;

/// <summary>
/// Le specifiche del contratto. Nel framework ogni prezzo e' un intero in tick: stop, target e
/// confronti non soffrono mai di arrotondamenti in virgola mobile.
/// </summary>
public sealed record Strumento(string Simbolo, double Tick, double ValorePunto)
{
    /// <summary>Dollari per tick e per contratto: per NQ 0,25 x 20 = 5.</summary>
    public double ValoreTick => Tick * ValorePunto;

    public int InTick(double prezzo) => (int)Math.Round(prezzo / Tick, MidpointRounding.AwayFromZero);

    public double InPrezzo(int tick) => tick * Tick;

    public static readonly Strumento NQ = new("NQ", 0.25, 20.0);
    public static readonly Strumento MNQ = new("MNQ", 0.25, 2.0);

    /// <summary>
    /// Il simbolo dell'archivio decide il contratto. SINT e' la serie sintetica: ha le specifiche
    /// di NQ perche' serve a provare il framework su un mercato dove la verita' e' nota.
    /// </summary>
    public static Strumento Da(string simbolo) => simbolo.ToUpperInvariant() switch
    {
        "NQ" or "SINT" => NQ with { Simbolo = simbolo.ToUpperInvariant() },
        "MNQ" => MNQ,
        _ => throw new ArgumentException($"strumento sconosciuto: {simbolo}. Lo scope e' NQ."),
    };
}
