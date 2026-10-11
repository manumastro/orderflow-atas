using Quant.Dati;

namespace Quant.Motore;

/// <summary>
/// Una strategia, cioe' regole abbastanza precise da darle a una macchina. Il motore le chiama
/// <see cref="SuBarra"/> a barra chiusa; quello che la strategia decide si esegue dalla barra dopo.
///
/// La strategia non vede la serie: vede il <see cref="Contesto"/>, che le mostra solo il passato.
/// Il look-ahead che ha falsato la fase 40R qui non si puo' scrivere.
/// </summary>
public abstract class Strategia
{
    public abstract string Nome { get; }

    /// <summary>Si alza ogni volta che cambia la logica: una versione nuova e' una variante nuova nel registro.</summary>
    public virtual int Versione => 1;

    /// <summary>Le regole in parole: ingresso, uscita, dimensione, e da dove viene l'idea.</summary>
    public abstract string Regole { get; }

    /// <summary>La scheda dell'idea in quant/idee/, se c'e'.</summary>
    public virtual string? Idea => null;

    /// <summary>Se vero, il motore chiude la posizione e annulla gli ordini sull'ultima barra della giornata di borsa.</summary>
    public virtual bool ChiudeAFineGiornata => true;

    /// <summary>
    /// La finestra oraria (minuti ET) in cui la strategia puo' entrare. La usa il modello nullo che
    /// entra a caso: deve pescare dalla stessa finestra, o il confronto non e' alla pari.
    /// </summary>
    public virtual (int Da, int A) FinestraIngresso => (0, 24 * 60);

    /// <summary>Legge i parametri. Si chiama una volta, prima della prova.</summary>
    public abstract void Prepara(Parametri p);

    /// <summary>Prima barra di una nuova giornata, prima di <see cref="SuBarra"/>.</summary>
    public virtual void NuovaGiornata(Contesto c) { }

    /// <summary>La barra <c>c.Indice</c> e' chiusa: decidere qui, eseguire dalla prossima.</summary>
    public abstract void SuBarra(Contesto c);
}

/// <summary>
/// Cio' che la strategia vede alla chiusura di una barra: quella barra e le precedenti, la
/// giornata, le giornate passate gia' chiuse, la posizione, gli ordini.
/// </summary>
public sealed class Contesto
{
    private readonly Serie _serie;
    internal int I;
    internal Ordine? InAttesa;
    internal Posizione? Pos;
    internal bool RichiestaUscita;

    internal Contesto(Serie serie) => _serie = serie;

    public Strumento Strumento => _serie.Strumento;
    public int Indice => I;
    public Barra Barra => _serie.Barre[I];

    /// <summary>La barra k passi indietro: 0 e' quella appena chiusa. Il futuro non c'e'.</summary>
    public Barra Indietro(int k)
    {
        if (k < 0) throw new InvalidOperationException("look-ahead: una strategia non puo' leggere barre future");
        return _serie.Barre[I - k];
    }

    /// <summary>Tutte le barre fino a quella appena chiusa, inclusa.</summary>
    public ReadOnlySpan<Barra> Storia => _serie.Barre.AsSpan(0, I + 1);

    /// <summary>Le barre della giornata corrente fino a quella appena chiusa.</summary>
    public ReadOnlySpan<Barra> Oggi => _serie.Barre.AsSpan(PrimaDiOggi, I - PrimaDiOggi + 1);

    public int MinutoEt => _serie.MinutoEt[I];
    public DateOnly Giorno => _serie.Giorni[_serie.IndiceGiorno[I]].Data;
    public int IndiceGiorno => _serie.IndiceGiorno[I];
    public int PrimaDiOggi => _serie.Giorni[_serie.IndiceGiorno[I]].Primo;
    public int BarraDiOggi => I - PrimaDiOggi;

    /// <summary>Il riassunto della k-esima giornata chiusa prima di oggi (1 = ieri).</summary>
    public RiassuntoGiorno? GiornoPrecedente(int k)
    {
        if (k < 1) throw new InvalidOperationException("look-ahead: la giornata corrente non e' chiusa");
        var d = IndiceGiorno - k;
        return d >= 0 ? _serie.Riassunti[d] : null;
    }

    /// <summary>L'istante UTC di un orario di New York in una data di calendario.</summary>
    public long Utc(DateOnly data, int minutoEt) => Calendario.Utc(data, minutoEt);

    /// <summary>L'istante UTC di un orario di New York nella data di calendario di oggi (giornata di borsa).</summary>
    public long OggiAlle(int minutoEt) => Calendario.Utc(Giorno, minutoEt);

    public Posizione? Posizione => Pos;
    public bool Piatto => Pos is null;
    public bool HaOrdine => InAttesa is not null;

    public void Invia(Ordine o)
    {
        if (o.Tipo != TipoOrdine.Mercato && o.Prezzo == 0) throw new ArgumentException("ordine stop o limite senza prezzo");
        InAttesa = o;
    }

    public void Annulla() => InAttesa = null;

    /// <summary>Esce a mercato all'apertura della prossima barra.</summary>
    public void Esci()
    {
        if (Pos is not null) RichiestaUscita = true;
    }

    public void SpostaStop(int stop)
    {
        if (Pos is not null) Pos.Stop = stop;
    }
}
