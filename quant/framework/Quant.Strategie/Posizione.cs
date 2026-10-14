namespace Quant.Strategie;

/// <summary>La posizione aperta. Una sola per strategia: il motore non fa piramidi.</summary>
public sealed class Posizione
{
    public Lato Lato { get; internal set; }
    public int Contratti { get; internal set; }
    public int PrezzoIngresso { get; internal set; }
    public long TempoIngresso { get; internal set; }
    public int IndiceIngresso { get; internal set; }
    public int? Stop { get; internal set; }
    public int? Target { get; internal set; }
    public long? UscitaAlle { get; internal set; }
    public string Motivo { get; internal set; } = "";
    /// <summary>Massima escursione avversa e favorevole, in tick, misurata sugli estremi delle barre.</summary>
    public int Mae { get; internal set; }
    public int Mfe { get; internal set; }
    internal int StopIniziale { get; set; } = int.MinValue;
    internal int TargetIniziale { get; set; } = int.MinValue;
    internal bool UscitaRichiesta { get; set; }
}
