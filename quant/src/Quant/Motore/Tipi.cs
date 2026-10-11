using Quant.Dati;

namespace Quant.Motore;

public enum Lato { Long = 1, Short = -1 }

public enum TipoOrdine { Mercato, Stop, Limite }

public enum MotivoUscita { Stop, Target, Tempo, FineGiornata, Strategia, FineDati }

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

/// <summary>Un'operazione chiusa: cio' che il backtest consegna alle statistiche e alla validazione.</summary>
public sealed record Operazione
{
    public required Lato Lato { get; init; }
    public required int Contratti { get; init; }
    public required int IndiceIngresso { get; init; }
    public required int IndiceUscita { get; init; }
    public required long TempoIngresso { get; init; }
    public required long TempoUscita { get; init; }
    public required int PrezzoIngresso { get; init; }
    public required int PrezzoUscita { get; init; }
    public required MotivoUscita MotivoUscita { get; init; }
    /// <summary>La giornata di borsa dell'ingresso: le statistiche si raggruppano per questa.</summary>
    public required DateOnly Giorno { get; init; }
    public required int IndiceGiorno { get; init; }
    public required int MinutoEtIngresso { get; init; }
    /// <summary>Distanza dello stop iniziale in tick, se c'era: serve al modello nullo.</summary>
    public int? StopDistanza { get; init; }
    public int? TargetDistanza { get; init; }
    public long? UscitaAlle { get; init; }
    public int Mae { get; init; }
    public int Mfe { get; init; }
    /// <summary>Tick per contratto, gia' al netto dello slittamento.</summary>
    public required int Tick { get; init; }
    public required double Lordo { get; init; }
    public required double Costi { get; init; }
    public double Netto => Lordo - Costi;
    public string Motivo { get; init; } = "";
    public int BarreTenute => IndiceUscita - IndiceIngresso + 1;
    /// <summary>Il risultato in multipli del rischio iniziale, se lo stop c'era.</summary>
    public double? R => StopDistanza is > 0 ? (double)Tick / StopDistanza.Value : null;
}

/// <summary>
/// Le convenzioni di esecuzione. Ognuna e' una scelta con un costo, e il rapporto le stampa
/// accanto al risultato che producono.
/// </summary>
public sealed record Esecuzione
{
    /// <summary>Dollari per contratto per lato, commissioni e tasse di borsa.</summary>
    public double CommissioneLato { get; init; } = 2.50;
    /// <summary>Tick persi su ogni esecuzione a mercato o a stop, in ingresso e in uscita.</summary>
    public int SlittamentoTick { get; init; } = 1;
    /// <summary>Un limite si esegue solo se il prezzo lo attraversa di almeno un tick: toccarlo non basta.</summary>
    public bool LimiteAttraversato { get; init; } = true;
    /// <summary>Stop e target toccati nella stessa barra: senza tick non si sa chi e' venuto prima, si sceglie il peggio.</summary>
    public bool AmbiguitaAlPeggio { get; init; } = true;

    public static readonly Esecuzione Standard = new();
    public static readonly Esecuzione SenzaCosti = new() { CommissioneLato = 0, SlittamentoTick = 0 };

    public override string ToString() =>
        $"commissione {CommissioneLato:0.00} $/lato, slittamento {SlittamentoTick} tick su mercato e stop, " +
        $"limite {(LimiteAttraversato ? "solo se attraversato" : "al tocco")}, ambiguita' {(AmbiguitaAlPeggio ? "al peggio" : "al meglio")}";
}
