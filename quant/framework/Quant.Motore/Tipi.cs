namespace Quant.Motore;

public enum MotivoUscita { Stop, Target, Tempo, FineGiornata, Strategia, FineDati }

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
