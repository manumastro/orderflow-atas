namespace Quant.Regole.Contratti;

/// <summary>
/// Come il compratore opera dentro una fase: quanto grande rispetto alle sessioni della sorgente, e un
/// bracket sul P&amp;L della giornata (Youngblood §2.2). Stop e target si eseguono esattamente al livello;
/// toccato uno dei due, la giornata finisce li'.
/// </summary>
public sealed record Condotta
{
    /// <summary>Moltiplica il P&amp;L della sessione: deviazione, deriva e costi crescono insieme.</summary>
    public double Scala { get; init; } = 1;

    /// <summary>Si chiude quando il P&amp;L della giornata tocca -StopGiorno (sui minimi del passo).</summary>
    public double? StopGiorno { get; init; }

    /// <summary>Si chiude quando il P&amp;L della giornata tocca +TargetGiorno (sulle chiusure del passo).</summary>
    public double? TargetGiorno { get; init; }

    public static Condotta Neutra { get; } = new();

    public bool EUnaNeutra => Scala == 1 && StopGiorno is null && TargetGiorno is null;

    public override string ToString() =>
        $"scala {Scala:0.##}" + (StopGiorno is { } s ? $", stop {s:N0} $" : "") + (TargetGiorno is { } t ? $", target {t:N0} $" : "");

    /// <summary>Applica la condotta a una sessione, in place.</summary>
    public void Applica(Span<double> chiusura, Span<double> minimo)
    {
        if (EUnaNeutra) return;
        double? fuori = null;
        for (var k = 0; k < chiusura.Length; k++)
        {
            if (fuori is { } v) { chiusura[k] = v; minimo[k] = v; continue; }
            var c = chiusura[k] * Scala;
            var m = minimo[k] * Scala;
            if (StopGiorno is { } stop && m <= -stop) { fuori = -stop; chiusura[k] = -stop; minimo[k] = -stop; continue; }
            if (TargetGiorno is { } target && c >= target) { fuori = target; c = target; }
            chiusura[k] = c;
            minimo[k] = m;
        }
    }
}

/// <summary>La politica del compratore: una condotta in valutazione e una nel conto finanziato.</summary>
public sealed record Politica(string Nome, Condotta Valutazione, Condotta Finanziata)
{
    public static Politica Neutra { get; } = new("neutra", Condotta.Neutra, Condotta.Neutra);

    public override string ToString() => $"{Nome}: valutazione {Valutazione}; finanziato {Finanziata}";
}
