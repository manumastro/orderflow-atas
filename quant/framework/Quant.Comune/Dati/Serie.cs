namespace Quant.Dati;

/// <summary>Una giornata di borsa: le sue barre stanno fra Primo e Ultimo, entrambi inclusi.</summary>
public readonly record struct Giornata(DateOnly Data, int Primo, int Ultimo)
{
    public int Barre => Ultimo - Primo + 1;
}

/// <summary>Il riassunto di una giornata gia' chiusa, per le regole che guardano i giorni passati.</summary>
public readonly record struct RiassuntoGiorno(DateOnly Data, int Apertura, int Massimo, int Minimo, int Chiusura, long Volume);

/// <summary>
/// Un cambio di contratto nella serie continua. Scarto = prezzo del nuovo meno prezzo del vecchio,
/// in tick, allo stesso istante. Metodo: come e' stato ottenuto lo scarto (misurato, salto, carry:
/// vedi <see cref="ScartiDiRollover"/>); vuoto nei file scritti prima del 15 ottobre 2026.
/// </summary>
public readonly record struct Rollover(long Tempo, string Da, string A, int Scarto, string Metodo = "");

/// <summary>
/// Le barre di uno strumento con il calendario gia' calcolato: per ogni barra il minuto di New York
/// e la giornata di borsa. Si calcola una volta, poi ogni prova la legge senza convertire nulla.
/// </summary>
public sealed class Serie
{
    public Strumento Strumento { get; }
    public string Descrizione { get; }
    public Barra[] Barre { get; }
    /// <summary>Minuto del giorno, ora di New York, all'apertura della barra.</summary>
    public short[] MinutoEt { get; }
    /// <summary>Indice in <see cref="Giorni"/> della giornata della barra.</summary>
    public int[] IndiceGiorno { get; }
    public Giornata[] Giorni { get; }
    public RiassuntoGiorno[] Riassunti { get; }
    public Rollover[] Rollover { get; }
    public bool Rettificata { get; }

    public Serie(Strumento strumento, Barra[] barre, string descrizione, Rollover[]? rollover = null, bool rettificaIndietro = true)
    {
        Strumento = strumento;
        Descrizione = descrizione;
        Rollover = rollover ?? [];
        Barre = barre;
        for (var i = 1; i < barre.Length; i++)
            if (barre[i].Inizio <= barre[i - 1].Inizio)
                throw new InvalidDataException($"barre non in ordine o duplicate all'indice {i}: {barre[i]}");

        if (rettificaIndietro && Rollover.Length > 0)
        {
            Rettifica(barre, Rollover);
            Rettificata = true;
        }

        MinutoEt = new short[barre.Length];
        IndiceGiorno = new int[barre.Length];
        var giorni = new List<Giornata>();
        var conv = new Calendario.Convertitore();
        var primo = 0;
        DateOnly corrente = default;
        for (var i = 0; i < barre.Length; i++)
        {
            var et = conv.Et(barre[i].Inizio);
            MinutoEt[i] = (short)(et.Hour * 60 + et.Minute);
            var g = Calendario.GiornataDi(et);
            if (i == 0) corrente = g;
            else if (g != corrente)
            {
                giorni.Add(new Giornata(corrente, primo, i - 1));
                corrente = g;
                primo = i;
            }
            IndiceGiorno[i] = giorni.Count;
        }
        if (barre.Length > 0) giorni.Add(new Giornata(corrente, primo, barre.Length - 1));
        Giorni = [.. giorni];

        Riassunti = new RiassuntoGiorno[Giorni.Length];
        for (var d = 0; d < Giorni.Length; d++)
        {
            var g = Giorni[d];
            int hi = int.MinValue, lo = int.MaxValue;
            long vol = 0;
            for (var i = g.Primo; i <= g.Ultimo; i++)
            {
                hi = Math.Max(hi, barre[i].Massimo);
                lo = Math.Min(lo, barre[i].Minimo);
                vol += barre[i].Volume;
            }
            Riassunti[d] = new RiassuntoGiorno(g.Data, barre[g.Primo].Apertura, hi, lo, barre[g.Ultimo].Chiusura, vol);
        }
    }

    /// <summary>
    /// Rettifica all'indietro (Panama): ogni barra prima di un rollover si sposta dello scarto, cosi'
    /// che una posizione tenuta a cavallo del cambio non veda un salto che sul conto non c'e'.
    /// I prezzi assoluti del passato non sono piu' quelli quotati; le distanze si'.
    /// </summary>
    private static void Rettifica(Barra[] barre, Rollover[] rollover)
    {
        foreach (var r in rollover)
        {
            for (var i = 0; i < barre.Length && barre[i].Inizio < r.Tempo; i++)
            {
                barre[i].Apertura += r.Scarto;
                barre[i].Massimo += r.Scarto;
                barre[i].Minimo += r.Scarto;
                barre[i].Chiusura += r.Scarto;
            }
        }
    }

    /// <summary>L'indice della prima giornata con data maggiore o uguale a quella data.</summary>
    public int GiornoDa(DateOnly data)
    {
        var lo = 0;
        var hi = Giorni.Length;
        while (lo < hi)
        {
            var m = (lo + hi) / 2;
            if (Giorni[m].Data < data) lo = m + 1; else hi = m;
        }
        return lo;
    }

    public override string ToString() =>
        Giorni.Length == 0 ? $"{Descrizione}: vuota"
            : $"{Descrizione}: {Barre.Length:N0} barre, {Giorni.Length} giornate, {Giorni[0].Data:yyyy-MM-dd} -> {Giorni[^1].Data:yyyy-MM-dd}";
}
