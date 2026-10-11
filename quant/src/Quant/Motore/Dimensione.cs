using Quant.Dati;

namespace Quant.Motore;

/// <summary>
/// Il terzo riquadro di ogni strategia: quanti contratti. Si decide all'eseguito, quando lo stop e'
/// noto in tick. In sviluppo si usa un contratto fisso; dal vivo si dimensiona sulla volatilita',
/// perche' con un contratto fisso i giorni larghi decidono il risultato da soli
/// (Moreira e Muir, "Volatility-Managed Portfolios"; Harvey e altri, "The Impact of Volatility Targeting").
/// </summary>
public abstract class Dimensione
{
    public abstract string Descrizione { get; }

    /// <param name="serie">Per le giornate passate: si leggono solo quelle prima di <paramref name="giorno"/>.</param>
    /// <param name="giorno">Indice della giornata dell'ingresso.</param>
    /// <param name="stopTick">Distanza dello stop dall'eseguito, in tick, se lo stop c'e'.</param>
    public abstract int Contratti(Serie serie, int giorno, int? stopTick);

    /// <summary>Dai parametri: dimensione=fissa|rischio|volatilita'.</summary>
    public static Dimensione Da(Parametri p)
    {
        var tipo = p.Testo("dimensione", "fissa");
        var max = p.Intero("maxContratti", 50);
        return tipo switch
        {
            "fissa" => new DimensioneFissa(p.Intero("contratti", 1)),
            "rischio" => new RischioFisso(p.Reale("rischio", 1000), max),
            "volatilita" => new VolatilitaTarget(p.Reale("rischio", 1000), p.Intero("giorniVolatilita", 20), max),
            _ => throw new ArgumentException($"dimensione={tipo}: attesa fissa, rischio o volatilita"),
        };
    }
}

public sealed class DimensioneFissa(int contratti) : Dimensione
{
    public override string Descrizione => $"fissa, {contratti} contratt{(contratti == 1 ? "o" : "i")}";
    public override int Contratti(Serie serie, int giorno, int? stopTick) => contratti;
}

/// <summary>Stesso rischio in dollari a ogni operazione: i contratti sono il rischio diviso la distanza dello stop.</summary>
public sealed class RischioFisso(double dollari, int max) : Dimensione
{
    public override string Descrizione => $"rischio fisso {dollari:N0} $ sullo stop, massimo {max} contratti";

    public override int Contratti(Serie serie, int giorno, int? stopTick)
    {
        if (stopTick is not > 0) throw new InvalidOperationException("dimensione=rischio richiede uno stop su ogni ingresso");
        return Math.Min(max, (int)Math.Floor(dollari / (stopTick.Value * serie.Strumento.ValoreTick)));
    }
}

/// <summary>
/// Volatilita' obiettivo: i contratti sono il budget diviso l'escursione media giornaliera delle
/// ultime N giornate chiuse, in dollari. Giorni larghi, meno contratti.
/// </summary>
public sealed class VolatilitaTarget(double dollari, int giorni, int max) : Dimensione
{
    public override string Descrizione => $"volatilita' obiettivo {dollari:N0} $ per escursione media di {giorni} giornate, massimo {max} contratti";

    public override int Contratti(Serie serie, int giorno, int? stopTick)
    {
        if (giorno < giorni) return 0;
        double somma = 0;
        for (var d = giorno - giorni; d < giorno; d++)
            somma += serie.Riassunti[d].Massimo - serie.Riassunti[d].Minimo;
        var mediaTick = somma / giorni;
        return Math.Min(max, (int)Math.Floor(dollari / (mediaTick * serie.Strumento.ValoreTick)));
    }
}
