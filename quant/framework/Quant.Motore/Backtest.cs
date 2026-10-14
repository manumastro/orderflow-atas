using System.Diagnostics;

namespace Quant.Motore;

/// <summary>Il risultato di una prova: le operazioni e il conto giornata per giornata.</summary>
public sealed class Risultato
{
    public required string Strategia { get; init; }
    public required int Versione { get; init; }
    public required string Impronta { get; init; }
    public required Parametri Parametri { get; init; }
    public required Esecuzione Esecuzione { get; init; }
    public required string Dimensione { get; init; }
    public required Serie Serie { get; init; }
    public required int GiornoDa { get; init; }
    public required int GiornoA { get; init; }
    public required List<Operazione> Operazioni { get; init; }
    /// <summary>Netto in dollari per ogni giornata della prova, anche quelle senza operazioni.</summary>
    public required double[] NettoGiorno { get; init; }
    /// <summary>Ingressi eseguibili scartati: la dimensione dava zero contratti, o l'eseguito cadeva gia' oltre lo stop.</summary>
    public required int Saltati { get; init; }
    public required TimeSpan Durata { get; init; }
    public int Barre => Serie.Giorni[GiornoA].Ultimo - Serie.Giorni[GiornoDa].Primo + 1;
    public DateOnly Da => Serie.Giorni[GiornoDa].Data;
    public DateOnly A => Serie.Giorni[GiornoA].Data;
    public int Giorni => GiornoA - GiornoDa + 1;
}

/// <summary>
/// Il motore di backtest: rigioca la storia barra per barra, esegue gli ordini con convenzioni
/// dichiarate e conservative, e consegna le operazioni.
///
/// L'ordine degli eventi su ogni barra i:
/// 1. uscita richiesta dalla strategia, all'apertura;
/// 2. ingresso in attesa (deciso alla chiusura di i-1);
/// 3. uscite della posizione: a tempo all'apertura, poi gap, poi stop e target dentro la barra;
/// 4. la strategia vede la barra chiusa e decide;
/// 5. fine giornata: chiusura e annullamento, se la strategia lo chiede.
/// </summary>
public static class Backtest
{
    public static Risultato Esegui(Serie serie, Strategia strategia, Parametri parametri, Esecuzione? esecuzione = null,
                                   int giornoDa = 0, int giornoA = -1)
    {
        var e = esecuzione ?? Esecuzione.Standard;
        if (giornoA < 0) giornoA = serie.Giorni.Length - 1;
        if (serie.Giorni.Length == 0 || giornoDa > giornoA) throw new ArgumentException("nessuna giornata da provare");

        var orologio = Stopwatch.StartNew();
        strategia.Prepara(parametri);
        var dimensione = Dimensione.Da(parametri);
        var nonUsati = parametri.NonUsati.ToList();
        if (nonUsati.Count > 0) throw new ArgumentException($"parametri sconosciuti per {strategia.Nome}: {string.Join(", ", nonUsati)}");

        var barre = serie.Barre;
        var c = new Contesto(serie);
        var operazioni = new List<Operazione>();
        var netto = new double[giornoA - giornoDa + 1];
        var saltati = 0;
        var inizio = serie.Giorni[giornoDa].Primo;
        var fine = serie.Giorni[giornoA].Ultimo;
        var giornoCorrente = -1;
        var vt = serie.Strumento.ValoreTick;

        for (var i = inizio; i <= fine; i++)
        {
            ref readonly var b = ref barre[i];
            c.I = i;

            // 1. uscita chiesta alla chiusura della barra prima
            if (c.Pos is not null && c.RichiestaUscita)
            {
                Chiudi(c.Pos, i, b.Inizio, Avverso(b.Apertura, c.Pos.Lato, e.SlittamentoTick, uscita: true), MotivoUscita.Strategia);
            }
            c.RichiestaUscita = false;

            // 2. ingresso in attesa
            if (c.InAttesa is { } o && c.Pos is null)
            {
                if (b.Inizio >= o.Scadenza) c.InAttesa = null;
                else if (Eseguito(o, b, e) is { } prezzo)
                {
                    c.InAttesa = null;
                    var stop = o.Stop ?? (o.StopDistanza is { } sd ? prezzo - (int)o.Lato * sd : null);
                    var rischio = stop is { } s ? Math.Abs(prezzo - s) : (int?)null;
                    var target = o.Target ?? (o.TargetDistanza is { } td ? prezzo + (int)o.Lato * td
                        : o.TargetR is { } r && rischio is { } rk ? prezzo + (int)o.Lato * (int)Math.Round(r * rk) : null);
                    // eseguito gia' oltre lo stop (un gap): non c'e' rischio misurabile, l'ingresso non si fa e si conta
                    var oltreLoStop = stop is { } st && (int)o.Lato * (prezzo - st) <= 0;
                    var n = oltreLoStop ? 0 : o.Contratti ?? dimensione.Contratti(serie, serie.IndiceGiorno[i], rischio);
                    if (n <= 0) saltati++;
                    else
                    {
                        c.Pos = new Posizione
                        {
                            Lato = o.Lato, Contratti = n, PrezzoIngresso = prezzo, TempoIngresso = b.Inizio, IndiceIngresso = i,
                            Stop = stop, Target = target, UscitaAlle = o.UscitaAlle, Motivo = o.Motivo,
                            StopIniziale = stop ?? int.MinValue, TargetIniziale = target ?? int.MinValue,
                        };
                    }
                }
            }

            // 3. uscite della posizione su questa barra
            if (c.Pos is { } p)
            {
                AggiornaEscursioni(p, b);
                if (ValutaUscita(p.Lato, p.IndiceIngresso, p.Stop, p.Target, p.UscitaAlle, i, b, e) is { } u)
                    Chiudi(p, i, b.Inizio, u.Prezzo, u.Motivo);
            }

            // 4. la strategia
            if (serie.IndiceGiorno[i] != giornoCorrente)
            {
                giornoCorrente = serie.IndiceGiorno[i];
                strategia.NuovaGiornata(c);
            }
            strategia.SuBarra(c);

            // 5. fine giornata
            var ultimaDelGiorno = i == serie.Giorni[serie.IndiceGiorno[i]].Ultimo;
            if (ultimaDelGiorno && strategia.ChiudeAFineGiornata)
            {
                c.InAttesa = null;
                c.RichiestaUscita = false;
                if (c.Pos is { } q) Chiudi(q, i, b.Fine, Avverso(b.Chiusura, q.Lato, e.SlittamentoTick, uscita: true), MotivoUscita.FineGiornata);
            }
        }

        if (c.Pos is { } ultima)
        {
            ref readonly var b = ref barre[fine];
            Chiudi(ultima, fine, b.Fine, Avverso(b.Chiusura, ultima.Lato, e.SlittamentoTick, uscita: true), MotivoUscita.FineDati);
        }

        return new Risultato
        {
            Strategia = strategia.Nome, Versione = strategia.Versione, Impronta = parametri.Impronta(strategia.Nome, strategia.Versione),
            Parametri = parametri, Esecuzione = e, Dimensione = dimensione.Descrizione, Serie = serie,
            GiornoDa = giornoDa, GiornoA = giornoA, Operazioni = operazioni, NettoGiorno = netto, Saltati = saltati,
            Durata = orologio.Elapsed,
        };

        void Chiudi(Posizione p, int i, long tempo, int prezzo, MotivoUscita motivo)
        {
            var tick = (int)p.Lato * (prezzo - p.PrezzoIngresso);
            var gi = serie.IndiceGiorno[p.IndiceIngresso];
            var op = new Operazione
            {
                Lato = p.Lato, Contratti = p.Contratti, IndiceIngresso = p.IndiceIngresso, IndiceUscita = i,
                TempoIngresso = p.TempoIngresso, TempoUscita = tempo, PrezzoIngresso = p.PrezzoIngresso, PrezzoUscita = prezzo,
                MotivoUscita = motivo, Giorno = serie.Giorni[gi].Data, IndiceGiorno = gi,
                MinutoEtIngresso = serie.MinutoEt[p.IndiceIngresso],
                StopDistanza = p.StopIniziale == int.MinValue ? null : Math.Abs(p.PrezzoIngresso - p.StopIniziale),
                TargetDistanza = p.TargetIniziale == int.MinValue ? null : Math.Abs(p.TargetIniziale - p.PrezzoIngresso),
                UscitaAlle = p.UscitaAlle, Mae = p.Mae, Mfe = p.Mfe, Tick = tick,
                Lordo = tick * vt * p.Contratti, Costi = 2 * e.CommissioneLato * p.Contratti, Motivo = p.Motivo,
            };
            operazioni.Add(op);
            if (gi >= giornoDa && gi <= giornoA) netto[gi - giornoDa] += op.Netto;
            c.Pos = null;
        }
    }

    /// <summary>Il prezzo peggiorato dello slittamento: chi compra paga di piu', chi vende incassa di meno.</summary>
    internal static int Avverso(int prezzo, Lato lato, int slittamento, bool uscita)
    {
        var verso = uscita ? -(int)lato : (int)lato;
        return prezzo + verso * slittamento;
    }

    /// <summary>Se e a che prezzo l'ordine si esegue su questa barra.</summary>
    internal static int? Eseguito(Ordine o, in Barra b, Esecuzione e)
    {
        var l = (int)o.Lato;
        switch (o.Tipo)
        {
            case TipoOrdine.Mercato:
                return Avverso(b.Apertura, o.Lato, e.SlittamentoTick, uscita: false);
            case TipoOrdine.Stop:
                // buy stop sopra: si innesca se il massimo lo raggiunge; in gap si esegue all'apertura
                if (l * (b.Apertura - o.Prezzo) >= 0) return Avverso(b.Apertura, o.Lato, e.SlittamentoTick, false);
                if (l > 0 ? b.Massimo >= o.Prezzo : b.Minimo <= o.Prezzo) return Avverso(o.Prezzo, o.Lato, e.SlittamentoTick, false);
                return null;
            case TipoOrdine.Limite:
                // buy limit sotto: in gap si esegue all'apertura, migliore; dentro la barra solo se attraversato
                if (l * (o.Prezzo - b.Apertura) >= 0) return b.Apertura;
                var oltre = e.LimiteAttraversato ? 1 : 0;
                if (l > 0 ? b.Minimo <= o.Prezzo - oltre : b.Massimo >= o.Prezzo + oltre) return o.Prezzo;
                return null;
            default:
                throw new UnreachableException();
        }
    }

    /// <summary>
    /// Le uscite di una posizione su una barra, nell'ordine in cui possono accadere. Si usa uguale
    /// per le operazioni vere e per quelle del modello nullo: stesso metro.
    ///
    /// Sulla barra dell'ingresso conta solo lo stop: non si sa se il target e' stato toccato prima
    /// o dopo l'eseguito, e nel dubbio non si regala.
    /// </summary>
    internal static (int Prezzo, MotivoUscita Motivo)? ValutaUscita(Lato lato, int indiceIngresso, int? stop, int? target,
                                                                   long? uscitaAlle, int i, in Barra b, Esecuzione e)
    {
        var l = (int)lato;
        var stessaBarra = i == indiceIngresso;
        if (!stessaBarra)
        {
            if (uscitaAlle is { } t && b.Inizio >= t)
                return (Avverso(b.Apertura, lato, e.SlittamentoTick, uscita: true), MotivoUscita.Tempo);
            if (stop is { } s0 && l * (b.Apertura - s0) <= 0)
                return (Avverso(b.Apertura, lato, e.SlittamentoTick, uscita: true), MotivoUscita.Stop);
            if (target is { } t0 && l * (b.Apertura - t0) >= 0)
                return (t0, MotivoUscita.Target);
        }
        var stopToccato = stop is { } s && (l > 0 ? b.Minimo <= s : b.Massimo >= s);
        var oltre = e.LimiteAttraversato ? 1 : 0;
        var targetToccato = !stessaBarra && target is { } tg && (l > 0 ? b.Massimo >= tg + oltre : b.Minimo <= tg - oltre);
        if (stopToccato && (!targetToccato || e.AmbiguitaAlPeggio))
            return (Avverso(stop!.Value, lato, e.SlittamentoTick, uscita: true), MotivoUscita.Stop);
        if (targetToccato)
            return (target!.Value, MotivoUscita.Target);
        return null;
    }

    private static void AggiornaEscursioni(Posizione p, in Barra b)
    {
        var favorevole = p.Lato == Lato.Long ? b.Massimo - p.PrezzoIngresso : p.PrezzoIngresso - b.Minimo;
        var avversa = p.Lato == Lato.Long ? p.PrezzoIngresso - b.Minimo : b.Massimo - p.PrezzoIngresso;
        if (favorevole > p.Mfe) p.Mfe = favorevole;
        if (avversa > p.Mae) p.Mae = avversa;
    }
}
