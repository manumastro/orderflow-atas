namespace Quant.Motore.Contratti;

/// <summary>
/// Da dove vengono le sessioni che il contratto vede. Una sessione e' una sequenza di passi; per
/// ogni passo, in dollari dall'inizio della sessione: il P&amp;L alla fine del passo (<c>chiusura</c>)
/// e il peggiore toccato dentro il passo (<c>minimo</c>, che e' cio' che il controllo in tempo reale
/// della violazione vede).
/// </summary>
public interface ISorgenteSessioni
{
    string Descrizione { get; }
    int MassimoPassi { get; }

    /// <summary>Un cursore per un ciclo: tiene lo stato (per esempio il blocco del bootstrap).</summary>
    ICursoreSessioni Apri();
}

public interface ICursoreSessioni
{
    /// <summary>Riempie la prossima sessione e restituisce quanti passi ha.</summary>
    int Prossima(ref Caso rng, Span<double> chiusura, Span<double> minimo);
}

/// <summary>
/// Il moto browniano aritmetico di TF e Lim: P&amp;L di sessione con media <c>deriva</c> e deviazione
/// standard <c>d</c>, osservato in <c>passi</c> punti equidistanti. Fra due punti non si guarda: il
/// minimo del passo e' la sua chiusura, ed e' esattamente il monitoraggio discreto di TF §2.4.
/// </summary>
public sealed class SessioniBrowniane(double d, double deriva, int passi) : ISorgenteSessioni
{
    public string Descrizione => $"moto browniano, d = {d:N0} $ per sessione, deriva {deriva:N1} $ per sessione, {passi} controlli per sessione";
    public int MassimoPassi => passi;
    public double D => d;
    public double Deriva => deriva;
    public int Passi => passi;

    public ICursoreSessioni Apri() => new Cursore(d / Math.Sqrt(passi), deriva / passi, passi);

    private sealed class Cursore(double sigma, double mu, int passi) : ICursoreSessioni
    {
        public int Prossima(ref Caso rng, Span<double> chiusura, Span<double> minimo)
        {
            double x = 0;
            for (var k = 0; k < passi; k++)
            {
                x += mu + sigma * rng.Normale();
                chiusura[k] = x;
                minimo[k] = x;
            }
            return passi;
        }
    }
}

/// <summary>
/// Una sessione storica, per un contratto lungo e uno corto, gia' in dollari per un contratto e al
/// lordo dei costi. Long e short sono due percorsi, non uno il negativo dell'altro: con stop e target
/// nella stessa barra vince lo stop, per entrambi.
/// </summary>
public sealed record SessioneStorica(DateOnly Giorno, double[] ChiusuraLong, double[] MinimoLong, double[] ChiusuraShort, double[] MinimoShort);

/// <summary>
/// Sessioni storiche ricampionate a blocchi di sessioni consecutive (TF §8.2: blocchi di cinque), con
/// il verso pescato a caso a ogni sessione: e' l'ipotesi di nessun vantaggio di TF §8.1 e di
/// Youngblood §2.2. Contratti e costi si applicano qui.
/// </summary>
public sealed class SessioniStoriche : ISorgenteSessioni
{
    public SessioniStoriche(IReadOnlyList<SessioneStorica> sessioni, int contratti, double costoPerContratto, int blocco = 5)
    {
        if (sessioni.Count == 0) throw new ArgumentException("nessuna sessione storica");
        Sessioni = sessioni;
        Contratti = contratti;
        CostoPerContratto = costoPerContratto;
        Blocco = blocco;
        MassimoPassi = sessioni.Max(s => s.ChiusuraLong.Length);
    }

    public string Descrizione =>
        $"{Sessioni.Count} sessioni storiche ({Sessioni[0].Giorno:yyyy-MM-dd} - {Sessioni[^1].Giorno:yyyy-MM-dd}), " +
        $"verso a caso, {Contratti} contratti, costi {CostoPerContratto:N2} $ per contratto, blocchi di {Blocco} sessioni";

    public int MassimoPassi { get; }
    public IReadOnlyList<SessioneStorica> Sessioni { get; }
    public int Contratti { get; }
    public double CostoPerContratto { get; }
    public int Blocco { get; }

    public ICursoreSessioni Apri() => new Cursore(this);

    private sealed class Cursore(SessioniStoriche s) : ICursoreSessioni
    {
        private int _indice, _restanti;

        public int Prossima(ref Caso rng, Span<double> chiusura, Span<double> minimo)
        {
            if (_restanti == 0)
            {
                _indice = rng.Sotto(s.Sessioni.Count);
                _restanti = s.Blocco;
            }
            var sess = s.Sessioni[_indice];
            _indice = (_indice + 1) % s.Sessioni.Count;
            _restanti--;

            var lungo = rng.Sotto(2) == 0;
            var c = lungo ? sess.ChiusuraLong : sess.ChiusuraShort;
            var m = lungo ? sess.MinimoLong : sess.MinimoShort;
            var costo = s.Contratti * s.CostoPerContratto;
            for (var k = 0; k < c.Length; k++)
            {
                chiusura[k] = s.Contratti * c[k] - costo;
                minimo[k] = s.Contratti * m[k] - costo;
            }
            return c.Length;
        }
    }
}

/// <summary>Le sessioni storiche di una strategia senza vantaggio, costruite dalle barre di una serie.</summary>
public static class SessioniDaSerie
{
    /// <summary>
    /// Tutta la seduta, come in TF §8.1: ingresso all'apertura della prima barra dalle <paramref name="da"/>,
    /// valutazione a ogni barra, uscita alla chiusura dell'ultima barra prima delle <paramref name="a"/>.
    /// </summary>
    public static List<SessioneStorica> Seduta(Serie serie, double valorePunto, int da = Calendario.AperturaCassa, int a = Calendario.ChiusuraCassa)
    {
        var tick = serie.Strumento.Tick * valorePunto;
        var esito = new List<SessioneStorica>();
        foreach (var g in serie.Giorni)
        {
            var barre = BarreFra(serie, g, da, a);
            if (barre.Count < (a - da) * 9 / 10) continue; // seduta incompleta (festivi, chiusure anticipate)
            var ingresso = serie.Barre[barre[0]].Apertura;
            var n = barre.Count;
            double[] cl = new double[n], ml = new double[n], cs = new double[n], ms = new double[n];
            for (var k = 0; k < n; k++)
            {
                var b = serie.Barre[barre[k]];
                cl[k] = (b.Chiusura - ingresso) * tick;
                ml[k] = (b.Minimo - ingresso) * tick;
                cs[k] = (ingresso - b.Chiusura) * tick;
                ms[k] = (ingresso - b.Massimo) * tick;
            }
            esito.Add(new SessioneStorica(g.Data, cl, ml, cs, ms));
        }
        return esito;
    }

    /// <summary>
    /// La strategia di Youngblood §2.2: ingresso all'apertura della barra delle <paramref name="ingresso"/>,
    /// target e stop simmetrici di <paramref name="distanzaTick"/> tick, uscita a tempo alla chiusura
    /// dell'ultima barra prima delle <paramref name="uscita"/>. Stop e target nella stessa barra: vince
    /// lo stop (Youngblood esclude quei giorni; qui si sceglie il peggio, come nel motore).
    /// </summary>
    public static List<SessioneStorica> Youngblood(Serie serie, double valorePunto, int distanzaTick = 100,
                                                   int ingresso = 10 * 60, int uscita = Calendario.ChiusuraCassa)
    {
        var tick = serie.Strumento.Tick * valorePunto;
        var esito = new List<SessioneStorica>();
        foreach (var g in serie.Giorni)
        {
            var barre = BarreFra(serie, g, ingresso, uscita);
            if (barre.Count < (uscita - ingresso) * 9 / 10 || serie.MinutoEt[barre[0]] != ingresso) continue;
            var prezzo = serie.Barre[barre[0]].Apertura;
            var (cl, ml) = Percorso(serie, barre, prezzo, +1, distanzaTick, tick);
            var (cs, ms) = Percorso(serie, barre, prezzo, -1, distanzaTick, tick);
            esito.Add(new SessioneStorica(g.Data, cl, ml, cs, ms));
        }
        return esito;
    }

    private static (double[] Chiusura, double[] Minimo) Percorso(Serie serie, List<int> barre, int prezzo, int verso, int dist, double tick)
    {
        var n = barre.Count;
        double[] c = new double[n], m = new double[n];
        double? chiuso = null;
        for (var k = 0; k < n; k++)
        {
            if (chiuso is { } v) { c[k] = v; m[k] = v; continue; }
            var b = serie.Barre[barre[k]];
            var peggio = verso > 0 ? b.Minimo - prezzo : prezzo - b.Massimo;
            var meglio = verso > 0 ? b.Massimo - prezzo : prezzo - b.Minimo;
            if (peggio <= -dist) { chiuso = -dist * tick; c[k] = m[k] = chiuso.Value; continue; }
            if (meglio >= dist) { chiuso = dist * tick; c[k] = chiuso.Value; m[k] = peggio * tick; continue; }
            c[k] = verso * (b.Chiusura - prezzo) * tick;
            m[k] = peggio * tick;
        }
        return (c, m);
    }

    private static List<int> BarreFra(Serie serie, Giornata g, int da, int a)
    {
        var barre = new List<int>();
        for (var i = g.Primo; i <= g.Ultimo; i++)
        {
            int m = serie.MinutoEt[i];
            if (m >= da && m < a) barre.Add(i);
        }
        return barre;
    }
}
