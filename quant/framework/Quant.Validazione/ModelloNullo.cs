namespace Quant.Validazione;

public sealed record EsitoNullo
{
    public required string Metodo { get; init; }
    public required int Repliche { get; init; }
    public required double NettoReale { get; init; }
    public double NullaP05 { get; init; }
    public double NullaP50 { get; init; }
    public double NullaP95 { get; init; }
    /// <summary>Probabilita' che il caso faccia almeno quanto la strategia: (1 + quante volte) / (1 + repliche).</summary>
    public double P { get; init; }
    /// <summary>Quanto la strategia batte la mediana nulla, per operazione.</summary>
    public double VantaggioPerOperazione { get; init; }
}

/// <summary>
/// Il modello nullo: la stessa strategia senza la sua idea. Ogni operazione vera viene rifatta con
/// le stesse uscite (distanza dello stop e del target, durata), gli stessi contratti e gli stessi
/// costi, ma togliendo una cosa sola:
/// <list type="bullet">
/// <item><b>il quando</b> (<see cref="Tempo"/>): giornata e minuto a caso dentro il campione e dentro
/// la finestra della strategia, stesso verso, e l'uscita a tempo spostata della stessa quantita':
/// stessa esposizione. Dice se il segnale sceglie il momento o se basta stare nel mercato con quel verso.
/// E' il nullo che toglie il beta: su un campione salito per caso anche il nullo long guadagna.</item>
/// <item><b>il verso</b> (<see cref="Verso"/>): stesso istante, verso a caso. Dice se il verso scelto
/// batte la moneta. Su una strategia che va solo long misura il beta del campione, non un vantaggio:
/// per questo e' un cancello solo per le strategie che scelgono il verso.</item>
/// </list>
/// Nella fase 40R le condizioni del dossier facevano 49% e il nullo 47%: senza questo confronto
/// quel 49% sembrava un risultato.
/// </summary>
public static class ModelloNullo
{
    /// <summary>
    /// Giornata e minuto a caso, dentro il campione provato e dentro la finestra della strategia;
    /// stesso verso, stessa esposizione. La giornata si pesca a caso anche lei: il verso di
    /// un'operazione vera dipende dal percorso di quella giornata (una VWAP long esiste perche' il
    /// prezzo e' salito), e spostarla prima nella stessa giornata le farebbe catturare il movimento
    /// che l'ha generata. Su una passeggiata casuale quel nullo guadagnava: look-ahead nel nullo.
    /// </summary>
    public static EsitoNullo Tempo(Risultato r, Strategia strategia, int repliche = 1000, ulong seme = 7)
    {
        var serie = r.Serie;
        var (da, a) = strategia.FinestraIngresso;
        var tutti = new List<int>();
        for (var d = r.GiornoDa; d <= r.GiornoA; d++)
        {
            var g = serie.Giorni[d];
            for (var j = g.Primo; j < g.Ultimo; j++)
                if (InFinestra(serie.MinutoEt[j], da, a)) tutti.Add(j);
        }
        var pool = tutti.ToArray();
        return Esegui("giornata e minuto a caso, stesso verso e stessa esposizione", serie, r.Operazioni, strategia, r.Esecuzione, repliche, seme,
            (ref Caso rng, Operazione o) => pool.Length == 0 ? (o.IndiceIngresso, o.Lato) : (pool[rng.Sotto(pool.Length)], o.Lato));
    }

    public static EsitoNullo Verso(Risultato r, Strategia strategia, int repliche = 1000, ulong seme = 11) =>
        Verso(r.Serie, r.Operazioni, strategia, r.Esecuzione, repliche, seme);

    public static EsitoNullo Verso(Serie serie, IReadOnlyList<Operazione> ops, Strategia strategia, Esecuzione e, int repliche = 1000, ulong seme = 11) =>
        Esegui("stesso quando, verso a caso", serie, ops, strategia, e, repliche, seme,
            (ref Caso rng, Operazione o) => (o.IndiceIngresso, rng.Sotto(2) == 0 ? Lato.Long : Lato.Short));

    private delegate (int Indice, Lato Lato) Scelta(ref Caso rng, Operazione o);

    private static EsitoNullo Esegui(string metodo, Serie serie, IReadOnlyList<Operazione> ops, Strategia strategia, Esecuzione e,
                                     int repliche, ulong seme, Scelta scegli)
    {
        var reale = ops.Sum(o => o.Netto);
        if (ops.Count == 0) return new EsitoNullo { Metodo = metodo, Repliche = 0, NettoReale = 0, P = 1 };
        var totali = new double[repliche];
        var chiude = strategia.ChiudeAFineGiornata;
        Parallel.For(0, repliche, r =>
        {
            var rng = new Caso(seme, (ulong)r);
            double somma = 0;
            foreach (var o in ops)
            {
                var (j, lato) = scegli(ref rng, o);
                somma += Simula(serie, o, j, lato, chiude, e);
            }
            totali[r] = somma;
        });
        var oltre = totali.Count(t => t >= reale);
        Array.Sort(totali);
        var mediana = Percentili.Ordinato(totali, 0.5);
        return new EsitoNullo
        {
            Metodo = metodo, Repliche = repliche, NettoReale = reale,
            NullaP05 = Percentili.Ordinato(totali, 0.05), NullaP50 = mediana, NullaP95 = Percentili.Ordinato(totali, 0.95),
            P = (1.0 + oltre) / (1.0 + repliche),
            VantaggioPerOperazione = (reale - mediana) / ops.Count,
        };
    }

    /// <summary>Rifa un'operazione entrando a mercato all'apertura della barra j, con le uscite di quella vera.</summary>
    internal static double Simula(Serie serie, Operazione vera, int j, Lato lato, bool chiudeAFineGiornata, Esecuzione e)
    {
        var barre = serie.Barre;
        var l = (int)lato;
        var ingresso = Backtest.Avverso(barre[j].Apertura, lato, e.SlittamentoTick, uscita: false);
        int? stop = vera.StopDistanza is { } sd ? ingresso - l * sd : null;
        int? target = vera.TargetDistanza is { } td ? ingresso + l * td : null;
        // l'uscita a tempo si sposta con l'ingresso: il nullo resta nel mercato quanto l'operazione vera
        long? uscitaAlle = vera.UscitaAlle is { } u0 ? u0 + (barre[j].Inizio - barre[vera.IndiceIngresso].Inizio) : null;
        var limiteDurata = vera.MotivoUscita == MotivoUscita.Strategia ? j + (vera.IndiceUscita - vera.IndiceIngresso) : int.MaxValue;
        var ultimaDelGiorno = serie.Giorni[serie.IndiceGiorno[j]].Ultimo;
        var uscita = 0;
        var trovata = false;
        for (var k = j; k < barre.Length; k++)
        {
            ref readonly var b = ref barre[k];
            if (k == limiteDurata && k > j)
            {
                uscita = Backtest.Avverso(b.Apertura, lato, e.SlittamentoTick, uscita: true);
                trovata = true;
                break;
            }
            if (Backtest.ValutaUscita(lato, j, stop, target, uscitaAlle, k, b, e) is { } u)
            {
                uscita = u.Prezzo;
                trovata = true;
                break;
            }
            if (chiudeAFineGiornata && k == ultimaDelGiorno)
            {
                uscita = Backtest.Avverso(b.Chiusura, lato, e.SlittamentoTick, uscita: true);
                trovata = true;
                break;
            }
        }
        if (!trovata) uscita = Backtest.Avverso(barre[^1].Chiusura, lato, e.SlittamentoTick, uscita: true);
        var tick = l * (uscita - ingresso);
        return tick * serie.Strumento.ValoreTick * vera.Contratti - 2 * e.CommissioneLato * vera.Contratti;
    }

    internal static bool InFinestra(int minuto, int da, int a) => da <= a ? minuto >= da && minuto < a : minuto >= da || minuto < a;
}
