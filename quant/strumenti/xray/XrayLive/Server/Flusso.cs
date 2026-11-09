namespace XrayLive.Server;

/// <summary>
/// Il flusso del delta per gruppo, letto contro lo storico (9 novembre 2026, richiesta dell'utente: il riquadro
/// "Chi ha comprato delta oggi" piu' utile a una decisione, sulla logica statistica e sui paper). La base e' la
/// ricerca quant/paper/ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md:
/// - nessun paper trova un gruppo che anticipi il verso di un indice (su SPX ne' al giorno ne' alla settimana);
/// - sulle nostre 35 sedute il flusso del retail e' quasi lo specchio del prezzo (correlazione mediana -0,85 dentro
///   la seduta), quello dei clienti professionali va con il prezzo (+0,87); tolta la parte spiegata dal prezzo, il
///   flusso della mattina non predice il pomeriggio per nessun gruppo.
/// Per questo il numero grezzo dice poco, e il server calcola tre letture che hanno un fondamento:
/// 1. il <b>percentile</b> del flusso di oggi fra le sedute passate alla stessa ora (dall'apertura della cassa);
/// 2. la <b>sorpresa</b>: il flusso meno quello che il movimento del prezzo dall'apertura farebbe attendere (una
///    regressione lineare fra le sedute passate, alla stessa ora), con il suo percentile e quanto il prezzo spiega (R²);
/// 3. lo <b>sbilancio</b> fra -1 e +1: il flusso netto diviso il lordo (la somma dei |delta| di ogni minuto e strike).
/// Lo storico sono solo le sedute prima del giorno letto: guardando un giorno passato non entra il suo futuro.
/// Nessuna lettura e' provata come previsione del prezzo: sono descrizioni.
/// </summary>
internal sealed class Flusso
{
    /// <summary>Sotto queste sedute il confronto non si fa.</summary>
    public const int MinSedute = 10;
    /// <summary>Le sedute passate al massimo: circa tre mesi.</summary>
    public const int MaxSedute = 60;
    /// <summary>Il passo in minuti delle bande dei piccoli grafici.</summary>
    public const int PassoBande = 5;
    /// <summary>I minuti della cassa, 09:30-16:00 di New York.</summary>
    public const int MinutiCassa = 390;
    /// <summary>Le soglie di "nella norma": una convenzione nostra, non provata.</summary>
    public const double SogliaBassa = 20, SogliaAlta = 80;

    public static readonly string[] Ambiti = { "0dte", "all" };

    private readonly Magazzino _m;
    private readonly Misure _misure;
    private readonly Dictionary<string, Seduta> _chiuse = new(StringComparer.Ordinal);
    private readonly object _lock = new();

    public Flusso(Magazzino m, Misure misure)
    {
        _m = m;
        _misure = misure;
    }

    /// <summary>
    /// Una seduta minuto per minuto dall'apertura della cassa: l'indice k e' il minuto dopo le 09:30 (0..390).
    /// `Prezzo[k]` e' lo spot di SPX meno quello del primo minuto; `Cum[gruppo_ambito][k]` il delta accumulato dalle
    /// colonne con ts in (09:30, 09:30 + k]; `Lordo` la somma dei |delta| di quelle colonne.
    /// </summary>
    public sealed record Seduta(string Giorno, double?[] Prezzo, Dictionary<string, double?[]> Cum, Dictionary<string, double?[]> Lordo);

    public static string Chiave(string gruppo, string ambito) => $"{gruppo}_{ambito}";

    /// <summary>
    /// La versione del riassunto su disco: si cambia quando cambia il modo di calcolare una seduta, e i file vecchi si
    /// ricalcolano da soli.
    /// </summary>
    public const int Versione = 1;

    /// <summary>
    /// La seduta: quelle chiuse (non l'ultima dell'archivio) si calcolano una volta sola, e il riassunto (pochi KB) si
    /// scrive in `flusso/<giorno>.json` accanto all'archivio. Senza, ogni avvio rileggeva decine di sedute di DEX per
    /// strike: 1 minuto e 45 secondi per la prima pagina (9 novembre 2026).
    /// </summary>
    public Seduta SedutaDi(string giorno, bool chiusa)
    {
        if (!chiusa) return Costruisci(giorno);
        lock (_lock)
            if (_chiuse.TryGetValue(giorno, out var s)) return s;
        var nuova = DalDisco(giorno) ?? SulDisco(Costruisci(giorno));
        lock (_lock) _chiuse[giorno] = nuova;
        return nuova;
    }

    private string FileDi(string giorno) => Path.Combine(_m.Root, "flusso", giorno + ".json");

    private sealed record SuDisco(int Versione, string Giorno, double?[] Prezzo, Dictionary<string, double?[]> Cum, Dictionary<string, double?[]> Lordo);

    private Seduta? DalDisco(string giorno)
    {
        try
        {
            var f = FileDi(giorno);
            if (!File.Exists(f)) return null;
            var d = System.Text.Json.JsonSerializer.Deserialize<SuDisco>(File.ReadAllText(f));
            return d is { Versione: Versione } && d.Giorno == giorno ? new Seduta(d.Giorno, d.Prezzo, d.Cum, d.Lordo) : null;
        }
        catch (Exception ex) when (ex is IOException or System.Text.Json.JsonException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    private Seduta SulDisco(Seduta s)
    {
        try
        {
            var f = FileDi(s.Giorno);
            Directory.CreateDirectory(Path.GetDirectoryName(f)!);
            var tmp = f + ".tmp";
            File.WriteAllText(tmp, System.Text.Json.JsonSerializer.Serialize(new SuDisco(Versione, s.Giorno, s.Prezzo, s.Cum, s.Lordo)));
            File.Move(tmp, f, overwrite: true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // senza disco si ricalcola al prossimo avvio: non e' un errore della risposta
        }
        return s;
    }

    private Seduta Costruisci(string giorno)
    {
        var (apertura, _) = Cassa.Di(giorno);
        var spot = _m.Strike("gex__mm_0dte", giorno);
        var prezzo = new double?[MinutiCassa + 1];
        double? base_ = null;
        int j = 0;
        double? ultimo = null;
        for (int k = 0; k <= MinutiCassa; k++)
        {
            long fino = apertura + k * 60_000L;
            while (j < spot.Count && spot[j].Ts <= fino) { if (double.IsFinite(spot[j].Spot) && spot[j].Spot > 0) ultimo = spot[j].Spot; j++; }
            // lo spot del minuto 0 e' ancora la chiusura di ieri: la base e' il primo minuto di cassa
            if (k == 1) base_ = ultimo;
            prezzo[k] = k >= 1 && base_ is { } b && ultimo is { } u && j > 0 ? u - b : (k == 0 ? 0 : null);
        }

        var cum = new Dictionary<string, double?[]>(StringComparer.Ordinal);
        var lordo = new Dictionary<string, double?[]>(StringComparer.Ordinal);
        foreach (var g in Misure.Gruppi)
            foreach (var a in Ambiti)
            {
                var cs = _m.Strike($"dex__{g}_{a}", giorno);
                var c = new double?[MinutiCassa + 1];
                var l = new double?[MinutiCassa + 1];
                double somma = 0, assoluti = 0;
                int i = 0;
                while (i < cs.Count && cs[i].Ts <= apertura) i++;
                bool visto = false;
                for (int k = 0; k <= MinutiCassa; k++)
                {
                    long fino = apertura + k * 60_000L;
                    while (i < cs.Count && cs[i].Ts <= fino)
                    {
                        var col = cs[i];
                        for (int s = 0; s < col.Strike.Length; s++)
                        {
                            somma += col.Call[s] + col.Put[s];
                            assoluti += Math.Abs(col.Call[s]) + Math.Abs(col.Put[s]);
                        }
                        visto = true;
                        i++;
                    }
                    // dopo l'ultima colonna del giorno il valore non si allunga: e' un'ora che non c'e' ancora
                    bool oltre = i >= cs.Count && (cs.Count == 0 || fino > cs[^1].Ts + 60_000L);
                    c[k] = k == 0 ? 0 : visto && !oltre ? somma : null;
                    l[k] = k == 0 ? 0 : visto && !oltre ? assoluti : null;
                }
                cum[Chiave(g, a)] = c;
                lordo[Chiave(g, a)] = l;
            }
        return new Seduta(giorno, prezzo, cum, lordo);
    }

    // ---------------------------------------------------------------- la risposta

    public object Di(string giorno, long? ts)
    {
        var giorni = _misure.Giorni();                         // dal piu' recente
        string ultimo = giorni.FirstOrDefault() ?? giorno;
        var tempi = _misure.Tempi(giorno);
        long quando = ts ?? (tempi.Count > 0 ? tempi[^1] : Cassa.Di(giorno).apertura);
        var (apertura, chiusura) = Cassa.Di(giorno);
        int k = (int)Math.Floor((Math.Min(quando, chiusura) - apertura) / 60_000.0);
        bool cassa = k >= 1;
        k = Math.Clamp(k, 0, MinutiCassa);

        var oggi = SedutaDi(giorno, chiusa: giorno != ultimo);
        var passate = giorni.Where(g => string.CompareOrdinal(g, giorno) < 0).Take(MaxSedute)
            .Select(g => SedutaDi(g, chiusa: true))
            .Where(s => s.Cum.Values.Any(v => v[MinutiCassa / 2] is not null || v[30] is not null))
            .ToList();

        var gruppi = new List<object>();
        var bande = new Dictionary<string, object>(StringComparer.Ordinal);
        foreach (var g in Misure.Gruppi)
            foreach (var a in Ambiti)
            {
                string key = Chiave(g, a);
                gruppi.Add(Lettura(g, a, oggi, passate, k, cassa));
                bande[key] = Bande(oggi, passate, key, k);
            }

        return new
        {
            giorno,
            ts = quando,
            minuto = k,
            cassa,
            apertura,
            prezzoDallApertura = cassa ? Misure.R(oggi.Prezzo[k] ?? double.NaN, 2) : null,
            sedute = passate.Count,
            dal = passate.Count > 0 ? passate[^1].Giorno : null,
            al = passate.Count > 0 ? passate[0].Giorno : null,
            minimo = MinSedute,
            soglie = new { bassa = SogliaBassa, alta = SogliaAlta },
            gruppi,
            bande,
            nota = "dall'apertura della cassa; lo storico sono le sedute prima di questo giorno; nessuna lettura e' provata come previsione",
        };
    }

    private static object Lettura(string gruppo, string ambito, Seduta oggi, List<Seduta> passate, int k, bool cassa)
    {
        string key = Chiave(gruppo, ambito);
        double? f = cassa ? oggi.Cum[key][k] : null;
        double? f30 = cassa && k >= 30 && oggi.Cum[key][k - 30] is { } p30 && f is { } fv ? fv - p30 : (cassa ? f : null);
        double? lordo = cassa ? oggi.Lordo[key][k] : null;
        double? sbilancio = f is { } a && lordo is { } l && l > 0 ? a / l : null;

        // lo storico alla stessa ora: flusso e prezzo dall'apertura
        var x = new List<double>();
        var y = new List<double>();
        var soloFlusso = new List<double>();
        foreach (var s in passate)
        {
            if (s.Cum[key][k] is not { } fy) continue;
            soloFlusso.Add(fy);
            if (s.Prezzo[k] is { } px) { x.Add(px); y.Add(fy); }
        }
        double? percentile = f is { } v && soloFlusso.Count >= MinSedute ? Percentile(soloFlusso, v) : null;

        double? atteso = null, sorpresa = null, percSorpresa = null, r2 = null, pendenza = null;
        if (f is { } fo && oggi.Prezzo[k] is { } po && x.Count >= MinSedute && Regressione(x, y) is { } reg)
        {
            r2 = reg.r2;
            pendenza = reg.b;
            atteso = reg.a + reg.b * po;
            sorpresa = fo - atteso;
            var residui = x.Select((xi, i) => y[i] - (reg.a + reg.b * xi)).ToList();
            percSorpresa = Percentile(residui, sorpresa.Value);
        }

        return new
        {
            gruppo,
            ambito,
            oggi = Misure.R(f ?? double.NaN, 2),
            trentaMinuti = Misure.R(f30 ?? double.NaN, 2),
            sbilancio = Misure.R(sbilancio ?? double.NaN, 3),
            percentile = Misure.R(percentile ?? double.NaN, 1),
            confronti = soloFlusso.Count,
            atteso = Misure.R(atteso ?? double.NaN, 2),
            sorpresa = Misure.R(sorpresa ?? double.NaN, 2),
            percentileSorpresa = Misure.R(percSorpresa ?? double.NaN, 1),
            // quanto il prezzo dall'apertura spiega il flusso, fra le sedute passate a quest'ora, e in che verso
            spiegaPrezzo = Misure.R(r2 ?? double.NaN, 3),
            versoPrezzo = pendenza is { } bp ? (bp < 0 ? "contro" : "con") : null,
        };
    }

    private static object Bande(Seduta oggi, List<Seduta> passate, string key, int k)
    {
        // ogni 5 minuti, piu' il minuto di adesso se cade fra due passi: la linea di oggi arriva fino ad adesso, e
        // anche li' la banda c'e' (senza, l'area si interrompeva)
        var minuti = Enumerable.Range(0, MinutiCassa / PassoBande + 1).Select(i => i * PassoBande).ToList();
        if (k > 0 && k % PassoBande != 0) { minuti.Add(k); minuti.Sort(); }
        var p10 = new List<double?>();
        var p50 = new List<double?>();
        var p90 = new List<double?>();
        var dioggi = new List<double?>();
        foreach (int m in minuti)
        {
            var v = passate.Select(s => s.Cum[key][m]).Where(z => z is not null).Select(z => z!.Value).OrderBy(z => z).ToList();
            bool basta = v.Count >= MinSedute;
            p10.Add(basta ? Misure.R(Quantile(v, 0.10), 2) : null);
            p50.Add(basta ? Misure.R(Quantile(v, 0.50), 2) : null);
            p90.Add(basta ? Misure.R(Quantile(v, 0.90), 2) : null);
            dioggi.Add(m <= k && oggi.Cum[key][m] is { } o ? Misure.R(o, 2) : null);
        }
        return new { minuti, p10, p50, p90, oggi = dioggi };
    }

    // ---------------------------------------------------------------- la statistica

    /// <summary>La quota (0-100) dei valori sotto v, con meta' dei pari: 50 = a meta' dello storico.</summary>
    public static double Percentile(IReadOnlyCollection<double> storico, double v)
    {
        if (storico.Count == 0) return double.NaN;
        double sotto = storico.Count(z => z < v), pari = storico.Count(z => z == v);
        return 100 * (sotto + pari / 2) / storico.Count;
    }

    /// <summary>Il quantile q (0-1) di una lista gia' ordinata, interpolando fra i due vicini.</summary>
    public static double Quantile(IReadOnlyList<double> ordinati, double q)
    {
        if (ordinati.Count == 0) return double.NaN;
        double pos = q * (ordinati.Count - 1);
        int i = (int)Math.Floor(pos);
        if (i >= ordinati.Count - 1) return ordinati[^1];
        return ordinati[i] + (pos - i) * (ordinati[i + 1] - ordinati[i]);
    }

    /// <summary>La retta dei minimi quadrati y = a + b x, con l'R²; null se x non varia.</summary>
    public static (double a, double b, double r2)? Regressione(IReadOnlyList<double> x, IReadOnlyList<double> y)
    {
        int n = x.Count;
        if (n < 2) return null;
        double mx = x.Average(), my = y.Average();
        double sxx = 0, sxy = 0, syy = 0;
        for (int i = 0; i < n; i++)
        {
            sxx += (x[i] - mx) * (x[i] - mx);
            sxy += (x[i] - mx) * (y[i] - my);
            syy += (y[i] - my) * (y[i] - my);
        }
        if (sxx <= 0) return null;
        double b = sxy / sxx;
        // y costante: il prezzo non spiega niente (la retta e' piatta)
        double r2 = syy > 0 ? sxy * sxy / (sxx * syy) : 0;
        return (my - b * mx, b, r2);
    }
}
