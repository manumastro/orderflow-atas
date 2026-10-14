namespace Quant.Validazione.Contratti;

/// <summary>
/// Cio' che si legge dai cicli simulati: le probabilita', i soldi veri e la loro distribuzione, con
/// la precisione dichiarata. Sui dati storici la precisione non la danno i cicli ma le sessioni (TF
/// §8.2): i tentativi indipendenti sono circa sessioni / sessioni per tentativo.
/// </summary>
public sealed record Riepilogo
{
    public required int Cicli { get; init; }
    public required double[] PassaggioPerFase { get; init; }
    /// <summary>Probabilita' di ottenere il conto finanziato a ogni tentativo.</summary>
    public required double Passaggio { get; init; }
    public required double ErrorePassaggio { get; init; }
    public required double SessioniPerTentativo { get; init; }
    public required double TentativiPerConto { get; init; }
    /// <summary>Quanto costa in media un conto finanziato (TF eq. 9, misurato).</summary>
    public required double CostoPerConto { get; init; }
    /// <summary>Quanto rende in media un conto finanziato al compratore: prelievi alla sua quota, piu' l'eventuale rimborso (TF eq. 10).</summary>
    public required double ValoreConto { get; init; }
    public required double ProbabilitaPrelievo { get; init; }
    public required double PrelieviPerConto { get; init; }
    public required double SessioniFinanziate { get; init; }
    public required double NettoMedio { get; init; }
    public required double ErroreNetto { get; init; }
    public required double NettoP5 { get; init; }
    public required double NettoP50 { get; init; }
    public required double NettoP95 { get; init; }
    public required double ProbabilitaPerdita { get; init; }
    /// <summary>Il tasso di conversione di pareggio con il valore del conto misurato (TF §5.3).</summary>
    public required double Pareggio { get; init; }
    public required Dictionary<FineFase, double> FineFinanziata { get; init; }
    public required int CicliSenzaConto { get; init; }

    public static Riepilogo Da(Ciclo[] cicli, Prezzi prezzi)
    {
        var fasi = cicli[0].Passate.Length;
        var perFase = new double[fasi];
        for (var f = 0; f < fasi; f++)
            perFase[f] = (double)cicli.Sum(c => c.Passate[f]) / Math.Max(1, cicli.Sum(c => c.Arrivate[f]));
        var tentativi = cicli.Sum(c => (long)c.Tentativi);
        var finanziati = cicli.Where(c => c.Finanziato).ToArray();
        var p = (double)finanziati.Length / tentativi;
        var netti = cicli.Select(c => c.Netto).ToArray();
        var (media, dev) = MediaDev(netti);
        var ordinati = (double[])netti.Clone();
        Array.Sort(ordinati);
        var valore = finanziati.Length > 0 ? finanziati.Average(c => c.Incassato) : 0;
        return new Riepilogo
        {
            Cicli = cicli.Length,
            PassaggioPerFase = perFase,
            Passaggio = p,
            ErrorePassaggio = Math.Sqrt(p * (1 - p) / tentativi),
            SessioniPerTentativo = (double)cicli.Sum(c => (long)c.SessioniValutazione) / tentativi,
            TentativiPerConto = finanziati.Length > 0 ? (double)tentativi / finanziati.Length : double.PositiveInfinity,
            CostoPerConto = finanziati.Length > 0 ? cicli.Sum(c => c.Costo) / finanziati.Length : double.PositiveInfinity,
            ValoreConto = valore,
            ProbabilitaPrelievo = finanziati.Length > 0 ? finanziati.Count(c => c.NumeroPrelievi > 0) / (double)finanziati.Length : 0,
            PrelieviPerConto = finanziati.Length > 0 ? finanziati.Average(c => c.NumeroPrelievi) : 0,
            SessioniFinanziate = finanziati.Length > 0 ? finanziati.Average(c => c.SessioniFinanziate) : 0,
            NettoMedio = media,
            ErroreNetto = dev / Math.Sqrt(netti.Length),
            NettoP5 = Percentili.Ordinato(ordinati, 0.05),
            NettoP50 = Percentili.Ordinato(ordinati, 0.50),
            NettoP95 = Percentili.Ordinato(ordinati, 0.95),
            ProbabilitaPerdita = netti.Count(x => x < 0) / (double)netti.Length,
            Pareggio = FormaChiusa.Pareggio(prezzi.Ingresso, prezzi.Reset, valore),
            FineFinanziata = finanziati.GroupBy(c => c.FineFinanziata).ToDictionary(g => g.Key, g => g.Count() / (double)finanziati.Length),
            CicliSenzaConto = cicli.Length - finanziati.Length,
        };
    }

    private static (double Media, double Dev) MediaDev(double[] x)
    {
        var m = x.Average();
        var v = x.Sum(v => (v - m) * (v - m)) / Math.Max(1, x.Length - 1);
        return (m, Math.Sqrt(v));
    }

    /// <summary>Le righe da stampare: un numero, cosa vuol dire, con la sua precisione.</summary>
    public IEnumerable<string> Righe(int? sessioniStoriche = null)
    {
        yield return $"cicli simulati                    {Cicli:N0}";
        for (var f = 0; f < PassaggioPerFase.Length; f++)
            yield return $"P fase {f + 1}                          {PassaggioPerFase[f]:0.0000}";
        yield return $"P conto per tentativo             {Passaggio:0.0000}  (errore Monte Carlo {ErrorePassaggio:0.0000})";
        if (sessioniStoriche is { } s)
        {
            var indipendenti = s / Math.Max(1e-9, SessioniPerTentativo);
            var vero = Math.Sqrt(Passaggio * (1 - Passaggio) / Math.Max(1, indipendenti));
            yield return $"  sui dati storici: circa {indipendenti:N0} tentativi indipendenti ({s} sessioni / {SessioniPerTentativo:0.0}), errore vero di P circa {vero:0.000} (TF §8.2)";
        }
        yield return $"sessioni per tentativo            {SessioniPerTentativo:0.00}";
        yield return $"tentativi per conto               {TentativiPerConto:0.00}";
        yield return $"costo per conto finanziato        {CostoPerConto:N0} $";
        yield return $"valore di un conto finanziato     {ValoreConto:N0} $  (prelievi alla quota del trader, piu' l'eventuale rimborso)";
        yield return $"  almeno un prelievo              {ProbabilitaPrelievo:P1}";
        yield return $"  prelievi per conto              {PrelieviPerConto:0.00}";
        yield return $"  sessioni finanziate per conto   {SessioniFinanziate:0.0}";
        yield return $"  come finisce                    {string.Join(", ", FineFinanziata.OrderByDescending(kv => kv.Value).Select(kv => $"{kv.Key} {kv.Value:P1}"))}";
        yield return $"netto per ciclo                   {NettoMedio:N0} $  (errore Monte Carlo {ErroreNetto:N0} $)";
        yield return $"  p5 / mediana / p95              {NettoP5:N0} / {NettoP50:N0} / {NettoP95:N0} $";
        yield return $"  cicli in perdita                {ProbabilitaPerdita:P1}";
        yield return $"tasso di pareggio PA (TF §5.3)    {Pareggio:0.000}  (P sotto questa soglia: il conto non vale il prezzo)";
        if (CicliSenzaConto > 0) yield return $"cicli fermati al tetto dei tentativi: {CicliSenzaConto:N0}";
    }
}
