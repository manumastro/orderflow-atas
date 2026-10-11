using Quant.Dati;
using Quant.Motore;

namespace Quant.Test;

/// <summary>Serie scritte a mano: ogni test dichiara le barre che gli servono, in punti NQ.</summary>
internal static class Costruttore
{
    /// <summary>Una giornata di borsa con barre M1 dalle 9:30 ET di quella data, una per riga (o, h, l, c in punti).</summary>
    public static Serie Giornata(DateOnly data, params (double O, double H, double L, double C)[] barre)
    {
        var s = Strumento.NQ;
        var t0 = Calendario.Utc(data, Calendario.AperturaCassa);
        var arr = barre.Select((b, i) => new Barra
        {
            Inizio = t0 + i * Tempo.Minuto, Fine = t0 + (i + 1) * Tempo.Minuto - 1,
            Apertura = s.InTick(b.O), Massimo = s.InTick(b.H), Minimo = s.InTick(b.L), Chiusura = s.InTick(b.C),
            Volume = 100, VolumeAsk = 50, VolumeBid = 50,
        }).ToArray();
        return new Serie(s, arr, "test");
    }

    public static readonly DateOnly Lunedi = new(2026, 3, 2);
}

/// <summary>Una strategia di prova: invia un ordine dato alla barra indicata, e niente altro.</summary>
internal sealed class Comandata(int barra, Func<Contesto, Ordine> ordine, bool chiude = true) : Strategia
{
    public override string Nome => "comandata";
    public override string Regole => "test";
    public override bool ChiudeAFineGiornata => chiude;
    public override void Prepara(Parametri p) { }
    public Action<Contesto>? Ogni { get; init; }

    public override void SuBarra(Contesto c)
    {
        Ogni?.Invoke(c);
        if (c.BarraDiOggi == barra) c.Invia(ordine(c));
    }
}
