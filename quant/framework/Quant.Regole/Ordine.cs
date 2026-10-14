namespace Quant.Regole;

public enum Lato { Long = 1, Short = -1 }

public enum TipoOrdine { Mercato, Stop, Limite }

/// <summary>
/// Un ordine di ingresso, con le sue uscite gia' dichiarate: i tre riquadri di ogni strategia
/// (ingresso, uscita, dimensione) arrivano al motore insieme.
///
/// Stop e target si danno in assoluto (tick) o relativi al prezzo d'eseguito; il target anche in
/// multipli del rischio (TargetR). L'uscita a tempo e' un istante UTC.
/// </summary>
public sealed class Ordine
{
    public required Lato Lato { get; init; }
    public TipoOrdine Tipo { get; init; } = TipoOrdine.Mercato;
    /// <summary>Prezzo d'innesco per Stop e Limite, in tick.</summary>
    public int Prezzo { get; init; }
    /// <summary>L'ordine non eseguito decade a quest'istante (UTC ms).</summary>
    public long Scadenza { get; init; } = long.MaxValue;

    public int? Stop { get; init; }
    public int? StopDistanza { get; init; }
    public int? Target { get; init; }
    public int? TargetDistanza { get; init; }
    public double? TargetR { get; init; }
    public long? UscitaAlle { get; init; }

    /// <summary>Contratti fissati dalla strategia; se manca decide la regola di dimensione.</summary>
    public int? Contratti { get; init; }
    public string Motivo { get; init; } = "";
}
