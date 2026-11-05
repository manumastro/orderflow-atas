using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using XrayLive.Server;

namespace XrayLive.Volatilita;

/// <summary>Lo stato di una fonte, per la pagina Fonti: quando ha dato l'ultimo dato buono e l'ultimo errore.</summary>
internal sealed class StatoFonte
{
    public required string Id { get; init; }
    public required string Fonte { get; init; }
    public required string Nome { get; init; }
    public required string Nota { get; init; }
    public DateTime? Ultimo { get; set; }
    public DateTime? Tentato { get; set; }
    public string? Errore { get; set; }
    public double? Secondi { get; set; }
    /// <summary>Un giro e' cominciato e non e' ancora finito (il primo giro della GEX viva dura minuti).</summary>
    public bool InCorso { get; set; }
    /// <summary>A che punto e' il giro in corso, se la fonte lo sa dire ("7 scadenze su 17").</summary>
    public Func<string?>? Avanzamento { get; set; }
}

/// <summary>
/// La raccolta delle fonti nuove, dentro il processo del server (2 novembre 2026):
/// - **catena CME via ATAS** ogni 5 minuti a CME aperto: VIX (opzioni ES) e VXN (opzioni NQ) con la formula
///   del Cboe, e il sorriso della scadenza vicina ai 30 giorni;
/// - **Cboe pubblico** (QQQ, NDX) ogni 15 minuti in cassa, ogni 3 ore fuori: GEX, zero gamma, muri,
///   volatilita' a 30 giorni, struttura a termine, sorriso, e la conversione in prezzi di NQ;
/// - **gex.py** (la GEX della catena di NQZ6 ed ESZ6) ogni 30 minuti a CME aperto, se la sua cache e'
///   piu' vecchia e nessun altro la sta calcolando (lo stesso lucchetto del giro e dello studio).
/// Tutto si archivia in quant/dati/volatilita/&lt;giorno di New York&gt;/: serie.jsonl (una riga per
/// valore), l'ultima fotografia di ogni fonte, e la storia del Cboe.
/// </summary>
internal sealed class Raccolta
{
    private readonly string _dati;
    private readonly string _repo;
    private readonly CatenaAtas _catena;
    private readonly HttpClient _http;
    private readonly object _scrivi = new();
    public readonly ConcurrentDictionary<string, StatoFonte> Stato = new();

    public static readonly string[] Radici = { "ES", "NQ" };
    public static readonly string[] SimboliCboe = { "QQQ", "_NDX" };

    public Raccolta(string dati, string bridge, string repo)
    {
        _dati = dati;
        _repo = repo;
        _catena = new CatenaAtas(bridge);
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
        Nuova("catena-es", "catena", "Catena ES (CME, via ATAS)", "VIX ricostruito e sorriso, ogni 5 minuti a CME aperto");
        Nuova("catena-nq", "catena", "Catena NQ (CME, via ATAS)", "VXN ricostruito e sorriso, ogni 5 minuti a CME aperto");
        Nuova("gex-nq", "catena", "GEX viva di NQ (catena CME, via ATAS)", "0DTE riletto e GEX ricalcolata ogni minuto, le altre scadenze a rotazione (una al minuto)");
        Nuova("gex-es", "catena", "GEX viva di ES (catena CME, via ATAS)", "0DTE riletto e GEX ricalcolata ogni minuto, le altre scadenze a rotazione (una al minuto)");
        Gex = new GexViva(_catena);
        foreach (var radice in Radici)
            Stato[$"gex-{radice.ToLowerInvariant()}"].Avanzamento = () => Gex.Stati[radice].Avanzamento;
        Nuova("cboe-qqq", "cboe", "QQQ (Cboe pubblico)", "ritardo 15 minuti; l'eta' e' quella dei prezzi: fuori dalla cassa, l'ultima chiusura");
        Nuova("cboe-ndx", "cboe", "NDX (Cboe pubblico)", "ritardo 15 minuti; l'eta' e' quella dei prezzi: fuori dalla cassa, l'ultima chiusura");
    }

    private void Nuova(string id, string fonte, string nome, string nota) =>
        Stato[id] = new StatoFonte { Id = id, Fonte = fonte, Nome = nome, Nota = nota };

    public string Dati => _dati;

    /// <summary>La radice del repository (per il calendario degli eventi).</summary>
    public string Repo => _repo;

    /// <summary>La GEX viva di NQ ed ES (GexViva.cs), in memoria e archiviata ogni minuto.</summary>
    public GexViva Gex { get; }

    public void Avvia(CancellationToken stop)
    {
        _ = Task.Run(() => Ciclo("catena", TimeSpan.FromMinutes(5), CicloCatene, stop));
        _ = Task.Run(() => Ciclo("cboe", TimeSpan.FromMinutes(1), CicloCboe, stop));
        // un ciclo per radice: leggono chart diversi (NQZ6 M30, ESZ6 M30), e il primo giro di ES dura minuti
        foreach (var radice in Radici)
            _ = Task.Run(() => Ciclo("gex", TimeSpan.FromMinutes(1), () => CicloGexViva(radice), stop));
    }

    private static async Task Ciclo(string nome, TimeSpan passo, Action fai, CancellationToken stop)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), stop).ContinueWith(_ => { });
        while (!stop.IsCancellationRequested)
        {
            try
            {
                fai();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[raccolta {nome}] {ex.GetType().Name}: {ex.Message}");
            }
            await Task.Delay(passo, stop).ContinueWith(_ => { });
        }
    }

    // ---------------------------------------------------------------- gli orari

    /// <summary>Il CME delle opzioni su indici: da domenica 18:00 a venerdi' 17:00 di New York, con la pausa 17-18.</summary>
    public static bool CmeAperto(DateTime utc)
    {
        var ny = TimeZoneInfo.ConvertTimeFromUtc(utc, Cassa.NewYork);
        double h = ny.TimeOfDay.TotalHours;
        return ny.DayOfWeek switch
        {
            DayOfWeek.Saturday => false,
            DayOfWeek.Sunday => h >= 18,
            DayOfWeek.Friday => h < 17,
            _ => h < 17 || h >= 18,
        };
    }

    /// <summary>L'ultima chiusura del CME (le 16:00 di New York di un giorno feriale) di almeno due ore fa:
    /// a quell'istante valgono i settlement.</summary>
    public static DateTime UltimaChiusura(DateTime utc)
    {
        var ny = TimeZoneInfo.ConvertTimeFromUtc(utc.AddHours(-2), Cassa.NewYork);
        var d = ny.TimeOfDay.TotalHours >= 16 ? ny.Date : ny.Date.AddDays(-1);
        while (d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) d = d.AddDays(-1);
        return TimeZoneInfo.ConvertTimeToUtc(d.AddHours(16), Cassa.NewYork);
    }

    public static string GiornoNY(DateTime utc) =>
        TimeZoneInfo.ConvertTimeFromUtc(utc, Cassa.NewYork).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    // ---------------------------------------------------------------- la catena CME

    private void CicloCatene()
    {
        if (!CmeAperto(DateTime.UtcNow)) return;
        foreach (var radice in Radici) Fonte($"catena-{radice.ToLowerInvariant()}", () => Catena(radice));
    }

    private void Fonte(string id, Action fai)
    {
        var s = Stato[id];
        var t0 = Stopwatch.StartNew();
        s.Tentato = DateTime.UtcNow;
        s.InCorso = true;
        try
        {
            fai();
            s.Ultimo = DateTime.UtcNow;
            s.Errore = null;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException
                                       or KeyNotFoundException or IOException or FormatException)
        {
            s.Errore = ex.Message;
            // nel log del lettore: un errore che si ripete a ogni giro deve vedersi anche senza la pagina Fonti
            Console.Error.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{id}] {ex.GetType().Name}: {ex.Message}");
        }
        finally
        {
            s.InCorso = false;
        }
        s.Secondi = Math.Round(t0.Elapsed.TotalSeconds, 1);
    }

    public sealed record FotoCatena(long Ts, string Radice, string Indice, double? Valore, string Prezzi,
        object? Vicina, object? Lontana, object? Sorriso, double? Sottostante);

    private void Catena(string radice)
    {
        var charts = _catena.Charts();
        var (a, b) = CatenaAtas.ChartPer(charts, radice);
        if (a is null || b is null) throw new InvalidOperationException($"nessun chart di {radice} sul bridge");
        var adesso = DateTime.UtcNow;
        // i venerdi': le giornaliere vicine hanno strike ogni 5 punti, e 120 per lato tagliano l'ala delle put
        var venerdi = _catena.SerieDi(a.Id).Where(s =>
            DateTime.ParseExact(s.Giorno, "yyyy-MM-dd", CultureInfo.InvariantCulture).DayOfWeek == DayOfWeek.Friday).ToList();
        // le coppie da provare: prima i venerdi' attorno ai 30 giorni (come il Cboe), poi le scadenze vicine di ogni tipo.
        // Il 5 novembre 2026 su NQ la coppia dei venerdi' (4/12, 11/12) non dava varianza ne' dal book ne' dal settlement
        // ("catena incompleta"), e il VXN ricostruito restava fermo da un giorno: ora si passa alla coppia successiva
        var tutte = _catena.SerieDi(a.Id);
        var bersaglio = adesso.AddDays(30);
        var prima = Formula.Attorno(venerdi, s => s.ScadeUtc, adesso);
        var vicine = tutte.Where(s => (s.ScadeUtc - adesso).TotalDays >= 7 && s.ScadeUtc <= bersaglio).OrderByDescending(s => s.ScadeUtc).Take(3);
        var lontane = tutte.Where(s => s.ScadeUtc > bersaglio).OrderBy(s => s.ScadeUtc).Take(3).ToList();
        var coppie = new List<(CatenaAtas.Serie vicina, CatenaAtas.Serie lontana)>();
        if (prima is { } p) coppie.Add(p);
        foreach (var sv in vicine)
            foreach (var sl in lontane)
                if (!coppie.Any(c => c.vicina.Giorno == sv.Giorno && c.lontana.Giorno == sl.Giorno)) coppie.Add((sv, sl));
        if (coppie.Count == 0) throw new InvalidOperationException("nessuna coppia di scadenze attorno ai 30 giorni");

        var lette = new Dictionary<string, Scadenza>();
        Scadenza Letta(string chart, CatenaAtas.Serie s) => lette.TryGetValue(s.Giorno, out var x) ? x : lette[s.Giorno] = _catena.Leggi(chart, s);
        Scadenza vicina = null!, lontana = null!;
        Varianza? v1 = null, v2 = null;
        var prezzi = Formula.Prezzi.Medio;
        var quando = adesso;
        var provate = new List<string>();
        foreach (var (cv, cl) in coppie.Take(4))
        {
            vicina = Letta(a.Id, cv);
            lontana = Letta(b.Id, cl);
            provate.Add($"{cv.Giorno}/{cl.Giorno}");
            // dal vivo il medio bid/ask; se il book non basta (notte, fine settimana) il settlement dell'ultima chiusura
            prezzi = Formula.Prezzi.Medio;
            quando = adesso;
            v1 = Formula.VarianzaCboe(vicina, quando, prezzi);
            v2 = Formula.VarianzaCboe(lontana, quando, prezzi);
            if (v1 is null || v2 is null || v1.Strike < 25 || v2.Strike < 25)
            {
                prezzi = Formula.Prezzi.Settlement;
                quando = UltimaChiusura(adesso);
                v1 = Formula.VarianzaCboe(vicina, quando, prezzi);
                v2 = Formula.VarianzaCboe(lontana, quando, prezzi);
            }
            if (v1 is not null && v2 is not null) break;
        }
        if (v1 is null || v2 is null)
            throw new InvalidOperationException($"catena incompleta: niente varianza (coppie provate {string.Join(", ", provate)})");
        double? valore = Formula.A30Giorni(v1, v2);
        var (sc, v) = Math.Abs(v1.Anni * 365 - 30) <= Math.Abs(v2.Anni * 365 - 30) ? (vicina, v1) : (lontana, v2);
        var sorriso = Formula.Sorriso(sc, quando, v.Forward, prezzi);
        string indice = radice == "ES" ? "SPX" : "NDX";
        string nomeTipo = prezzi == Formula.Prezzi.Medio ? "medio" : "settlement";
        var foto = new FotoCatena(new DateTimeOffset(quando).ToUnixTimeMilliseconds(), radice, indice,
            valore is { } x ? Math.Round(x, 3) : null, nomeTipo, Descrivi(v1), Descrivi(v2),
            new
            {
                scadenza = sc.Giorno,
                giorni = Math.Round(v.Anni * 365, 2),
                forward = Math.Round(v.Forward, 2),
                punti = sorriso.Select(p => new { moneyness = p.moneyness, strike = p.strike, vol = p.vol }),
            },
            vicina.Sottostante);
        string giorno = GiornoNY(quando);
        ScriviFoto(giorno, $"catena-{radice}.json", foto);
        if (valore is { } val)
            Riga(giorno, new { ts = foto.Ts, id = radice == "ES" ? "vix-catena" : "vxn-catena", v = Math.Round(val, 3), prezzi = nomeTipo });
    }

    private static object Descrivi(Varianza v) => new
    {
        scadenza = v.Scadenza,
        giorni = Math.Round(v.Anni * 365, 2),
        vol = Math.Round(v.Vol, 3),
        forward = Math.Round(v.Forward, 2),
        k0 = v.K0,
        strike = v.Strike,
        strikeMin = v.StrikeMin,
        strikeMax = v.StrikeMax,
    };

    // ---------------------------------------------------------------- il Cboe

    private readonly Dictionary<string, DateTime> _cboeFatto = new();

    private void CicloCboe()
    {
        var adesso = DateTime.UtcNow;
        var ny = TimeZoneInfo.ConvertTimeFromUtc(adesso, Cassa.NewYork);
        bool inCassa = ny.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday)
                       && ny.TimeOfDay >= TimeSpan.FromHours(9.5) && ny.TimeOfDay <= TimeSpan.FromHours(16.75);
        var passo = inCassa ? TimeSpan.FromMinutes(15) : TimeSpan.FromHours(3);
        foreach (var simbolo in SimboliCboe)
        {
            if (_cboeFatto.TryGetValue(simbolo, out var f) && adesso - f < passo) continue;
            _cboeFatto[simbolo] = adesso;
            Fonte(simbolo == "QQQ" ? "cboe-qqq" : "cboe-ndx", () => Cboe(simbolo));
        }
    }

    private void Cboe(string simbolo)
    {
        string json = _http.GetStringAsync(string.Format(CultureInfo.InvariantCulture, CboePubblico.Url, simbolo)).GetAwaiter().GetResult();
        var c = CboePubblico.Leggi(json);
        // i prezzi del file sono di 15 minuti prima; fuori dalla cassa valgono all'ultimo scambio
        var dati = c.Quando.AddMinutes(-15);
        if (!Cassa.Dentro(new DateTimeOffset(dati).ToUnixTimeMilliseconds())) dati = c.PrezzoQuando;
        long ts = new DateTimeOffset(dati).ToUnixTimeMilliseconds();
        string nome = c.Simbolo.ToLowerInvariant();

        object? vol30 = null;
        double? v30 = null;
        var coppia = Formula.Attorno(c.Scadenze, s => s.ScadeUtc, dati);
        Scadenza? perSorriso = null;
        Varianza? vSorriso = null;
        // il medio bid/ask; se il book e' vuoto (fuori cassa su QQQ) il prezzo teorico del Cboe
        var prezzi = Formula.Prezzi.Medio;
        if (coppia is { } cp)
        {
            var v1 = Formula.VarianzaCboe(cp.vicina, dati, prezzi);
            var v2 = Formula.VarianzaCboe(cp.lontana, dati, prezzi);
            if (v1 is null || v2 is null || v1.Strike < 25 || v2.Strike < 25)
            {
                prezzi = Formula.Prezzi.Settlement;
                v1 = Formula.VarianzaCboe(cp.vicina, dati, prezzi);
                v2 = Formula.VarianzaCboe(cp.lontana, dati, prezzi);
            }
            if (v1 is not null && v2 is not null)
            {
                v30 = Formula.A30Giorni(v1, v2);
                vol30 = new
                {
                    valore = v30 is { } x ? Math.Round(x, 3) : (double?)null, prezzi = prezzi == Formula.Prezzi.Medio ? "medio" : "teorico Cboe",
                    vicina = Descrivi(v1), lontana = Descrivi(v2),
                };
                (perSorriso, vSorriso) = Math.Abs(v1.Anni * 365 - 30) <= Math.Abs(v2.Anni * 365 - 30) ? (cp.vicina, v1) : (cp.lontana, v2);
            }
        }

        var termine = new List<object>();
        foreach (var s in c.Scadenze.Where(s => (s.ScadeUtc - dati).TotalDays is >= 0.5 and <= 120).Take(30))
        {
            var v = Formula.VarianzaCboe(s, dati, prezzi);
            double fw = v?.Forward ?? c.Prezzo;
            var atm = Formula.AlPrezzo(Formula.Sorriso(s, dati, fw, prezzi, 4));
            if (atm is { } a) termine.Add(new { scadenza = s.Giorno, giorni = Math.Round((s.ScadeUtc - dati).TotalDays, 2), vol = Math.Round(a, 3) });
        }

        object? sorriso = null;
        if (perSorriso is not null && vSorriso is not null)
            sorriso = new
            {
                scadenza = perSorriso.Giorno,
                giorni = Math.Round(vSorriso.Anni * 365, 2),
                forward = Math.Round(vSorriso.Forward, 2),
                punti = Formula.Sorriso(perSorriso, dati, vSorriso.Forward, prezzi)
                    .Select(p => new { moneyness = p.moneyness, strike = p.strike, vol = p.vol }),
            };

        var (tutte, oggi) = CboePubblico.PerGex(c, dati);
        var g = CboePubblico.Gex(tutte, c.Prezzo, dati);
        var g0 = oggi.Count > 0 ? CboePubblico.Gex(oggi, c.Prezzo, dati) : null;

        // la conversione in prezzi di NQ: NQ all'ora dell'ultimo prezzo del sottostante
        double? nq = PrezzoNQ(c.PrezzoQuando);
        object? conversione = nq is { } n
            ? simbolo == "QQQ"
                ? new { da = "QQQ", tipo = "rapporto", valore = Math.Round(n / c.Prezzo, 4), nq = n, quando = Ms(c.PrezzoQuando),
                    nota = "NQ / QQQ all'ora dell'ultimo prezzo di QQQ" }
                : new { da = "NDX", tipo = "base", valore = Math.Round(n - c.Prezzo, 2), nq = n, quando = Ms(c.PrezzoQuando),
                    nota = "NQ - NDX all'ora dell'ultimo prezzo di NDX" }
            : null;

        var foto = new
        {
            simbolo = c.Simbolo,
            ts,
            quando = Ms(c.Quando),
            prezzo = c.Prezzo,
            prezzoQuando = Ms(c.PrezzoQuando),
            iv30 = c.Iv30,
            vol30,
            termine,
            sorriso,
            gex = new
            {
                totale = g.Totale,
                zeroGamma = g.ZeroGamma is { } z ? Math.Round(z, 2) : (double?)null,
                callWall = g.CallWall,
                putWall = g.PutWall,
                strike = g.Strike.Select(s => new { strike = s.strike, call = s.call, put = s.put, netta = Math.Round(s.call + s.put, 3) }),
            },
            gex0dte = g0 is null ? null : new
            {
                totale = g0.Totale,
                zeroGamma = g0.ZeroGamma is { } z0 ? Math.Round(z0, 2) : (double?)null,
                callWall = g0.CallWall,
                putWall = g0.PutWall,
                giorno = oggi[0].Giorno,
                strike = g0.Strike.Select(s => new { strike = s.strike, call = s.call, put = s.put, netta = Math.Round(s.call + s.put, 3) }),
            },
            conversione,
        };
        string giorno = GiornoNY(dati);
        ScriviFoto(giorno, $"cboe-{c.Simbolo}.json", foto);
        ScriviFoto(giorno, Path.Combine("cboe", $"{c.Quando:HHmm}Z-{c.Simbolo}.json"), foto);
        if (v30 is { } vv) Riga(giorno, new { ts, id = $"vol30-{nome}", v = Math.Round(vv, 3) });
        if (c.Iv30 is { } iv) Riga(giorno, new { ts, id = $"iv30-{nome}", v = Math.Round(iv, 3) });
    }

    private static long Ms(DateTime utc) => new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc)).ToUnixTimeMilliseconds();

    /// <summary>La chiusura del minuto di NQ (contratto, M1) che contiene l'istante, dal bridge.</summary>
    public double? PrezzoNQ(DateTime utc) => PrezzoDi("NQ", utc);

    /// <summary>La chiusura del minuto del contratto di una radice (NQ, ES; M1) che contiene l'istante, dal bridge.</summary>
    public double? PrezzoDi(string radice, DateTime utc)
    {
        try
        {
            var chart = _catena.Charts().FirstOrDefault(c => c.Strumento.StartsWith(radice, StringComparison.Ordinal)
                                                           && c.Strumento.Length > radice.Length && c.TimeFrame == "M1");
            if (chart is null) return null;
            string iso(DateTime t) => t.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            string url = $"{_catena.Indirizzo}/candles?chart={chart.Id}&from={iso(utc.AddMinutes(-10))}&to={iso(utc.AddMinutes(1))}";
            using var doc = JsonDocument.Parse(_http.GetStringAsync(url).GetAwaiter().GetResult());
            double? ultimo = null;
            foreach (var k in doc.RootElement.GetProperty("candles").EnumerateArray())
            {
                var t = DateTimeOffset.Parse(k.GetProperty("time").GetString()!, CultureInfo.InvariantCulture).UtcDateTime;
                if (t <= utc) ultimo = k.GetProperty("close").GetDouble();
            }
            return ultimo;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException or FormatException)
        {
            return null;
        }
    }

    /// <summary>La volatilita' realizzata di una radice (5 novembre 2026, "implicita meno realizzata" del video sul bias
    /// dalle opzioni, docs/research/metodo/il-bias-del-giorno.md). Dal chart M30 del contratto (non il continuo: niente
    /// salti di rollover): <c>Venti</c> = chiusura su chiusura delle ultime 20 sedute CME (l'ultima barra prima delle 17:00 di New York),
    /// annua con 252; <c>Cinque</c> = rendimenti a 30 minuti delle ultime 5 sedute, annua con 46 x 252. In percento.
    /// Si ricalcola al piu' ogni 10 minuti.</summary>
    public (double? Venti, double? Cinque) Realizzata(string radice)
    {
        var adesso = DateTime.UtcNow;
        if (_realizzata.TryGetValue(radice, out var c) && adesso - c.Quando < TimeSpan.FromMinutes(10)) return (c.Venti, c.Cinque);
        (double? Venti, double? Cinque) r = (null, null);
        try
        {
            var chart = _catena.Charts().FirstOrDefault(x => x.Strumento.StartsWith(radice, StringComparison.Ordinal)
                                                           && x.Strumento.Length > radice.Length && x.TimeFrame == "M30");
            if (chart is not null)
            {
                string iso(DateTime t) => t.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                string url = $"{_catena.Indirizzo}/candles?chart={chart.Id}&from={iso(adesso.AddDays(-40))}";
                using var doc = JsonDocument.Parse(_http.GetStringAsync(url).GetAwaiter().GetResult());
                var barre = new List<(DateTime t, double close)>();
                foreach (var k in doc.RootElement.GetProperty("candles").EnumerateArray())
                    barre.Add((DateTimeOffset.Parse(k.GetProperty("time").GetString()!, CultureInfo.InvariantCulture).UtcDateTime,
                               k.GetProperty("close").GetDouble()));
                if (barre.Count > 2) barre.RemoveAt(barre.Count - 1); // la barra aperta cambia ancora
                // la seduta CME di una barra: dalle 18:00 di New York del giorno prima alle 17:00 del suo giorno (le 17:00
                // e le 16:00 di Chicago); il fuso con Cassa.NewYork, che regge anche la modalita' senza ICU del processo
                DateOnly seduta(DateTime utc)
                {
                    var l = TimeZoneInfo.ConvertTimeFromUtc(utc, Cassa.NewYork);
                    return DateOnly.FromDateTime(l.Hour >= 18 ? l.AddDays(1) : l);
                }
                var chiusure = barre.GroupBy(b => seduta(b.t)).OrderBy(g => g.Key).Select(g => g.Last().close).ToList();
                if (chiusure.Count > 0) chiusure.RemoveAt(chiusure.Count - 1); // la seduta in corso non e' chiusa
                static double? Annua(List<double> p, double perAnno)
                {
                    var r = p.Zip(p.Skip(1)).Where(z => z.First > 0 && z.Second > 0).Select(z => Math.Log(z.Second / z.First)).ToList();
                    return r.Count >= 10 ? Math.Round(100 * Math.Sqrt(r.Sum(x => x * x) / r.Count * perAnno), 1) : null;
                }
                var venti = chiusure.Skip(Math.Max(0, chiusure.Count - 21)).ToList();
                var giorniUltimi = barre.Select(b => seduta(b.t)).Distinct().OrderBy(d => d).TakeLast(5).ToHashSet();
                var cinque = barre.Where(b => giorniUltimi.Contains(seduta(b.t))).Select(b => b.close).ToList();
                r = (Annua(venti, 252), Annua(cinque, 46 * 252));
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException or FormatException)
        {
            Console.Error.WriteLine($"[{DateTime.Now:HH:mm:ss}] [realizzata {radice}] {ex.GetType().Name}: {ex.Message}");
        }
        _realizzata[radice] = (adesso, r.Venti, r.Cinque);
        return r;
    }

    private readonly ConcurrentDictionary<string, (DateTime Quando, double? Venti, double? Cinque)> _realizzata = new();


    // ---------------------------------------------------------------- la GEX viva

    /// <summary>
    /// Ogni minuto a CME aperto: la GEX viva di NQ ed ES (GexViva). A CME chiuso un solo giro, se non c'e' ancora
    /// niente, con i settlement. Fino al 2 novembre 2026 qui si lanciava gex.py ogni 30 minuti; gex.py resta al
    /// giro e allo studio, che lo lanciano da se'.
    /// </summary>
    private void CicloGexViva(string radice)
    {
        var adesso = DateTime.UtcNow;
        var st = Gex.Stati[radice];
        if (!CmeAperto(adesso) && st.Calcolo is not null) return;
        Fonte($"gex-{radice.ToLowerInvariant()}", () =>
        {
            var s = Gex.Giro(radice, adesso);
            Riga(GiornoNY(adesso), GexViva.RigaArchivio(s), $"gex-{radice}.jsonl");
        });
    }

    public static string CacheGex(string strumento) =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), $".fabio-gex-{strumento}.json");

    // ---------------------------------------------------------------- l'archivio

    private void ScriviFoto(string giorno, string nome, object foto)
    {
        string path = Path.Combine(_dati, giorno, nome);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        string tmp = path + ".tmp";
        File.WriteAllText(tmp, JsonSerializer.Serialize(foto, Json), SenzaBom);
        File.Move(tmp, path, true);
    }

    private void Riga(string giorno, object riga, string file = "serie.jsonl")
    {
        string path = Path.Combine(_dati, giorno, file);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        lock (_scrivi) File.AppendAllText(path, JsonSerializer.Serialize(riga, Json) + "\n", SenzaBom);
    }

    private static readonly UTF8Encoding SenzaBom = new(false);

    public static readonly JsonSerializerOptions Json = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>Le righe di serie.jsonl di un giorno, per id (un ts ripetuto vale una volta: l'ultimo).</summary>
    public Dictionary<string, SortedDictionary<long, double>> Serie(string giorno)
    {
        var out_ = new Dictionary<string, SortedDictionary<long, double>>();
        string path = Path.Combine(_dati, giorno, "serie.jsonl");
        if (!File.Exists(path)) return out_;
        string testo;
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
        using (var r = new StreamReader(fs, Encoding.UTF8))
            testo = r.ReadToEnd();
        foreach (var riga in testo.Split('\n'))
        {
            if (riga.Length < 2) continue;
            try
            {
                var n = JsonNode.Parse(riga);
                string? id = n?["id"]?.GetValue<string>();
                if (id is null || n!["v"] is not JsonValue v || n["ts"] is not JsonValue t) continue;
                if (!out_.TryGetValue(id, out var d)) out_[id] = d = new SortedDictionary<long, double>();
                d[t.GetValue<long>()] = v.GetValue<double>();
            }
            catch (JsonException)
            {
            }
        }
        return out_;
    }

    /// <summary>L'ultima fotografia di una fonte, cercata da oggi indietro di al massimo 7 giorni.</summary>
    public JsonNode? Foto(string nome, string? giorno = null)
    {
        var d = giorno is null ? DateTime.UtcNow : DateTime.ParseExact(giorno, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddHours(20);
        for (int i = 0; i < 8; i++)
        {
            string path = Path.Combine(_dati, GiornoNY(d.AddDays(-i)), nome);
            if (!File.Exists(path)) continue;
            try
            {
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                using var r = new StreamReader(fs, Encoding.UTF8);
                return JsonNode.Parse(r.ReadToEnd());
            }
            catch (Exception ex) when (ex is IOException or JsonException)
            {
            }
            if (giorno is not null) break;
        }
        return null;
    }
}
