using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;
using XrayLive.Server;

namespace XrayLive.Volatilita;

/// <summary>
/// Le risposte delle rotte nuove (/api/fonti, /api/vol/*, /api/nq/gex, /api/es/gex-confronto): mettono
/// accanto, ognuna con la sua fonte dichiarata, X-Ray (OptionsDepth), la catena CME via ATAS, il Cboe
/// pubblico e gli indici ufficiali. Il contratto: quant/strumenti/xray/x-ray-lettore-e-dashboard.md,
/// "Le fonti a confronto".
/// </summary>
internal sealed class MisureVol
{
    private readonly Magazzino _m;
    private readonly Ponte _ponte;
    private readonly Misure _misure;
    private readonly Raccolta _r;
    private readonly Yahoo _yahoo;
    private readonly Func<(DateTime? ultimo, string? errore)> _statoXray;
    private readonly ConcurrentDictionary<string, (DateTime quando, List<(long, double)> punti)> _atmXray = new();

    private readonly Bordi? _bordi;

    public MisureVol(Magazzino m, Ponte ponte, Misure misure, Raccolta r, Yahoo yahoo, Func<(DateTime?, string?)> statoXray,
        Bordi? bordi = null)
    {
        _bordi = bordi;
        _m = m;
        _ponte = ponte;
        _misure = misure;
        _r = r;
        _yahoo = yahoo;
        _statoXray = statoXray;
    }

    private static long Ms(DateTime utc) => new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc)).ToUnixTimeMilliseconds();

    // ---------------------------------------------------------------- le fonti

    public object Fonti()
    {
        var adesso = DateTime.UtcNow;
        var out_ = new List<object>();
        var (xu, xe) = _statoXray();
        out_.Add(Riga("xray", "xray", "X-Ray (OptionsDepth, cache di ATAS)", xu, xe, TimeSpan.FromMinutes(3),
            "si aggiorna solo con gli indicatori X-Ray aperti su un chart ES"));
        foreach (var s in _r.Stato.Values.OrderBy(s => s.Id))
        {
            DateTime? ultimo = s.Ultimo;
            if (s.Fonte == "cboe")
            {
                // l'eta' e' quella del dato (l'ultimo scambio del sottostante, o 15 minuti prima del file), non dello
                // scaricamento: il lunedi' mattina il file e' scaricato da poco ma i prezzi sono della chiusura di venerdi'
                var foto = _r.Foto(s.Id == "cboe-qqq" ? "cboe-QQQ.json" : "cboe-NDX.json");
                if (foto?["ts"] is JsonValue tv && tv.TryGetValue<long>(out var t))
                    ultimo = DateTimeOffset.FromUnixTimeMilliseconds(t).UtcDateTime;
            }
            var soglia = s.Fonte == "cboe" ? TimeSpan.FromHours(4) : s.Id.StartsWith("gex-", StringComparison.Ordinal)
                ? TimeSpan.FromMinutes(5) : TimeSpan.FromMinutes(15);
            // l'errore vale se e' piu' recente dell'ultimo dato buono, o se un dato buono non c'e' mai stato
            // (prima del 5 novembre 2026 un errore senza nessun dato buono non si vedeva: "assente" e basta)
            string? errore = s.Errore is not null && (ultimo is null || s.Tentato > ultimo) ? s.Errore : null;
            string nota = s.Nota + (s.Secondi is { } sec ? $"; ultimo giro {sec:0.#} s" : "");
            if (s.InCorso) nota += s.Avanzamento?.Invoke() is { } av ? $"; giro in corso, {av}" : "; giro in corso";
            out_.Add(Riga(s.Id, s.Fonte, s.Nome, ultimo, errore, soglia, nota, s.InCorso));
        }
        // la riga "yahoo" di prima resta per le pagine di oggi (la toglie la fase 2 del progetto della pagina); la
        // sintesi conta solo le tre righe per simbolo
        out_.Add(Riga("yahoo", "yahoo", "VIX e VXN ufficiali (Yahoo)", _yahoo.UltimoOk, _yahoo.UltimoErrore, TimeSpan.FromMinutes(30),
            "si leggono quando la pagina li chiede"));
        // Yahoo per simbolo (5 novembre 2026): un indice che non arriva non si nasconde dietro l'altro
        string oggi = Raccolta.GiornoNY(adesso);
        bool dopoApertura = Cassa.Dentro(Ms(adesso)) && Cassa.OraNY(Ms(adesso)).TimeOfDay >= TimeSpan.FromMinutes(9 * 60 + 35);
        foreach (var (id, simbolo, nome) in new[]
                 {
                     ("yahoo-vix", "^VIX", "VIX ufficiale (Yahoo)"), ("yahoo-vxn", "^VXN", "VXN ufficiale (Yahoo)"),
                     ("yahoo-qqq", "QQQ", "prezzo di QQQ (Yahoo)"),
                 })
        {
            // il prezzo di QQQ lo chiede solo la pagina NQ: se nessuno l'ha ancora chiesto, lo si chiede qui (si rilegge al
            // piu' ogni 30 secondi), cosi' la riga dice lo stato vero e non "assente"
            if (simbolo == "QQQ" && !_yahoo.PerSimbolo.ContainsKey(simbolo)) _yahoo.Prezzo(simbolo);
            var st = _yahoo.PerSimbolo.GetValueOrDefault(simbolo);
            string? errore = st.errore is not null && (st.ok is null || st.tentato > st.ok) ? st.errore : null;
            string nota = simbolo == "QQQ" ? "si legge quando la pagina NQ lo chiede; anche pre e dopo mercato"
                : "si legge quando la pagina lo chiede; a 1 minuto";
            if (simbolo != "QQQ" && errore is null && dopoApertura && _yahoo.Giorno(simbolo, oggi).Count == 0)
            {
                // in cassa da almeno 5 minuti e nessun punto di oggi: il dato manca, anche se Yahoo risponde
                out_.Add(new
                {
                    id, fonte = "yahoo", nome, ultimo = st.ok is { } u0 ? Ms(u0) : (long?)null,
                    secondiFa = st.ok is { } u1 ? Math.Round((adesso - u1).TotalSeconds) : (double?)null,
                    stato = "assente", errore = (string?)null, nota = "nessun punto oggi",
                });
                continue;
            }
            out_.Add(Riga(id, "yahoo", nome, st.ok, errore, TimeSpan.FromMinutes(30), nota));
        }
        return new { adesso = Ms(adesso), fonti = out_, sintesi = Sintesi(out_, adesso) };

        object Riga(string id, string fonte, string nome, DateTime? ultimo, string? errore, TimeSpan soglia, string nota, bool inCorso = false)
        {
            double? fa = ultimo is { } u ? Math.Round((adesso - u).TotalSeconds) : null;
            string stato = errore is not null ? "errore" : ultimo is null ? (inCorso ? "in corso" : "assente")
                : adesso - ultimo > soglia ? "vecchio" : "ok";
            return new { id, fonte, nome, ultimo = ultimo is { } x ? Ms(x) : (long?)null, secondiFa = fa, stato, errore, nota };
        }
    }

    /// <summary>
    /// Una riga per fonte (xray, catena, cboe, yahoo): lo stato peggiore dei suoi flussi (errore &gt; assente &gt;
    /// vecchio &gt; in corso &gt; ok), quale flusso è il peggiore e un testo breve per la barra in alto della pagina.
    /// </summary>
    private static List<object> Sintesi(List<object> righe, DateTime adesso)
    {
        static int Gravita(string s) => s switch { "errore" => 4, "assente" => 3, "vecchio" => 2, "in corso" => 1, _ => 0 };
        var lette = righe.Select(r => System.Text.Json.JsonSerializer.SerializeToNode(r)!.AsObject())
            .Select(n => (id: n["id"]!.GetValue<string>(), fonte: n["fonte"]!.GetValue<string>(), nome: n["nome"]!.GetValue<string>(),
                stato: n["stato"]!.GetValue<string>(), fa: n["secondiFa"] is JsonValue v && v.TryGetValue<double>(out var x) ? x : (double?)null,
                errore: n["errore"] is JsonValue e && e.TryGetValue<string>(out var es) ? es : null))
            .Where(r => r.id != "yahoo")
            .ToList();
        var out_ = new List<object>();
        foreach (var fonte in new[] { "xray", "catena", "cboe", "yahoo" })
        {
            var mie = lette.Where(r => r.fonte == fonte).ToList();
            if (mie.Count == 0) continue;
            var p = mie.OrderByDescending(r => Gravita(r.stato)).ThenByDescending(r => r.fa ?? double.MaxValue).First();
            string breve = p.nome.Split(" (")[0];
            string testo = p.stato switch
            {
                "ok" => $"aggiornata {Eta(p.fa)}",
                "vecchio" => $"vecchia {Eta(p.fa)}",
                "in corso" => $"{breve}: in corso",
                "assente" => $"{breve} assente",
                _ => $"{breve}: errore",
            };
            out_.Add(new { fonte, stato = p.stato, peggiore = p.id, secondiFa = p.fa, flussi = mie.Select(r => r.id).ToList(), testo });
        }
        return out_;
    }

    /// <summary>"42 s", "7 min", "3 h", "2 giorni".</summary>
    public static string Eta(double? secondi) => secondi switch
    {
        null => "–",
        < 60 => $"{secondi:0} s",
        < 3600 => $"{Math.Floor(secondi.Value / 60):0} min",
        < 172_800 => $"{Math.Floor(secondi.Value / 3600):0} h",
        _ => $"{Math.Floor(secondi.Value / 86_400):0} giorni",
    };

    // ---------------------------------------------------------------- la volatilita' nel tempo

    private static readonly (string id, string nome, string indice, string fonte, string descrizione)[] Descrizioni =
    {
        ("vix-catena", "VIX ricostruito", "SPX", "catena", "la formula del Cboe sulle opzioni ES che ATAS riceve, a 30 giorni"),
        ("vix-ufficiale", "VIX", "SPX", "yahoo", "l'indice del Cboe sulle opzioni SPX, via Yahoo"),
        ("atm-xray", "ATM X-Ray (opex)", "SPX", "xray", "la volatilita' al prezzo della scadenza opex, dal rapporto vanna/gamma di OptionsDepth: piu' bassa del VIX, che pesa anche le ali"),
        ("vxn-catena", "VXN ricostruito", "NDX", "catena", "la formula del Cboe sulle opzioni NQ che ATAS riceve, a 30 giorni"),
        ("vxn-ufficiale", "VXN", "NDX", "yahoo", "l'indice del Cboe sul Nasdaq-100, via Yahoo"),
        ("vol30-ndx", "VXN da NDX (Cboe)", "NDX", "cboe", "la formula del Cboe sulle opzioni NDX del file pubblico del Cboe (15 minuti di ritardo)"),
        ("vol30-qqq", "Vol 30g QQQ (Cboe)", "NDX", "cboe", "la formula del Cboe sulle opzioni QQQ (15 minuti di ritardo)"),
        ("iv30-ndx", "IV30 NDX (Cboe)", "NDX", "cboe", "la volatilita' a 30 giorni che il Cboe stesso calcola per NDX"),
        ("iv30-qqq", "IV30 QQQ (Cboe)", "NDX", "cboe", "la volatilita' a 30 giorni che il Cboe stesso calcola per QQQ"),
    };

    public object Serie(string giorno)
    {
        var archivio = _r.Serie(giorno);
        var out_ = new List<object>();
        foreach (var d in Descrizioni)
        {
            IEnumerable<(long ts, double v)> punti = d.id switch
            {
                "vix-ufficiale" => _yahoo.Giorno("^VIX", giorno),
                "vxn-ufficiale" => _yahoo.Giorno("^VXN", giorno),
                "atm-xray" => AtmXray(giorno),
                _ => archivio.TryGetValue(d.id, out var s) ? s.Select(kv => (kv.Key, kv.Value)) : Enumerable.Empty<(long, double)>(),
            };
            out_.Add(new
            {
                d.id, d.nome, d.indice, d.fonte, d.descrizione,
                // stile VIX (la formula del Cboe a 30 giorni, o l'indice) o al prezzo (ATM): solo le prime si confrontano
                misura = Misure30.Values.Any(m => m.atm.Contains(d.id)) ? "atm" : "stile-vix",
                punti = punti.Select(p => new object[] { p.ts, Math.Round(p.v, 2) }).ToList(),
            });
        }
        return new { giorno, serie = out_ };
    }

    /// <summary>La volatilita' al prezzo di X-Ray (scadenza opex) ogni 5 minuti del giorno.</summary>
    private List<(long, double)> AtmXray(string giorno)
    {
        bool oggi = giorno == _misure.Giorni().FirstOrDefault();
        if (_atmXray.TryGetValue(giorno, out var c) && (!oggi || DateTime.UtcNow - c.quando < TimeSpan.FromSeconds(60))) return c.punti;
        var gex = _m.Strike("gex__mm_opex", giorno);
        var vex = _m.Strike("vex__mm_opex", giorno);
        var punti = new List<(long, double)>();
        foreach (var g in Misure.Secchi(gex, x => x.Ts, 5))
        {
            var v = Misure.Ultima(vex, g.Ts);
            if (v is null || g.Ts - v.Ts > 120_000 || v.Strike.Length != g.Strike.Length) continue;
            var r = Formula.DaVannaGamma(g.Spot, g.Strike, g.Call, g.Put, v.Call, v.Put);
            if (r is { } x && Formula.MediaVicina(x.sorriso) is { } atm) punti.Add((g.Ts, Math.Round(atm, 3)));
        }
        _atmXray[giorno] = (DateTime.UtcNow, punti);
        return punti;
    }

    // ---------------------------------------------------------------- la struttura a termine

    public object Termine()
    {
        var curve = new List<object>
        {
            TermineGex("NQ", "termine-nq", "NQ (catena CME)", "NDX"),
            TermineGex("ES", "termine-es", "ES (catena CME)", "SPX"),
            TermineXray(),
            TermineCboe("NDX", "termine-ndx", "NDX (Cboe)"),
            TermineCboe("QQQ", "termine-qqq", "QQQ (Cboe)"),
        };
        return new { quando = Ms(DateTime.UtcNow), curve = curve.Select(Sospetti).ToList(), eventi = Eventi(70) };
    }

    /// <summary>
    /// Segna i punti sporchi di una curva (5 novembre 2026): una volatilità più del doppio della mediana delle vol della
    /// stessa curva è quasi sempre un dato sbagliato (uno strike senza book), non il mercato. Il punto resta, con
    /// <c>sospetto: true</c>.
    /// </summary>
    private static object Sospetti(object curva)
    {
        var n = System.Text.Json.JsonSerializer.SerializeToNode(curva, Raccolta.Json)!.AsObject();
        if (n["punti"] is not JsonArray punti) return n;
        var vol = punti.Select(p => D(p?["vol"])).OfType<double>().Where(v => v > 0).ToList();
        if (vol.Count < 3) return n;
        double m = Ponte.Mediana(vol);
        foreach (var p in punti.OfType<JsonObject>())
            p["sospetto"] = D(p["vol"]) is { } v && v > 2 * m;
        return n;
    }

    /// <summary>Gli eventi del calendario (docs/research/calendario/eventi.json) da oggi a <paramref name="giorni"/> giorni.</summary>
    private object Eventi(int giorni)
    {
        var out_ = new List<object>();
        string oggi = Raccolta.GiornoNY(DateTime.UtcNow);
        string fino = Raccolta.GiornoNY(DateTime.UtcNow.AddDays(giorni));
        string path = Path.Combine(_r.Repo, "docs", "research", "calendario", "eventi.json");
        try
        {
            if (File.Exists(path) && JsonNode.Parse(File.ReadAllText(path, Encoding.UTF8)) is JsonArray a)
                foreach (var e in a)
                {
                    string? g = e?["data"]?.GetValue<string>();
                    string? nome = e?["evento"]?.GetValue<string>();
                    if (g is null || nome is null || string.CompareOrdinal(g, oggi) < 0 || string.CompareOrdinal(g, fino) > 0) continue;
                    out_.Add(new { giorno = g, nome });
                }
        }
        catch (Exception ex) when (ex is IOException or System.Text.Json.JsonException or InvalidOperationException)
        {
        }
        return out_;
    }

    private object TermineGex(string radice, string id, string nome, string indice)
    {
        // la GEX viva: la volatilita' al prezzo di ogni scadenza dall'ultima lettura della catena
        var st = _r.Gex.Stati[radice];
        if (st.Atm.Count > 0 && st.Calcolo is not null)
        {
            var adesso = DateTime.UtcNow;
            var vivi = st.Atm.Where(kv => st.Righe.ContainsKey(kv.Key))
                .Select(kv => (g: kv.Key, giorni: (st.Righe[kv.Key][0].ScadeUtc - adesso).TotalDays, kv.Value.vol))
                .Where(x => x.giorni > 0).OrderBy(x => x.giorni)
                .Select(x => (object)new { scadenza = x.g, giorni = Math.Round(x.giorni, 2), vol = Math.Round(x.vol * 100, 2) }).ToList();
            return new
            {
                id, nome, indice, fonte = "catena", quando = st.CatenaIntera is { } ci ? Ms(ci) : (long?)null,
                descrizione = "la volatilita' al prezzo di ogni scadenza (Black-76), dalla catena letta ogni ora; lo 0DTE ogni minuto",
                punti = vivi,
            };
        }
        var (d, quando) = LeggiGex(radice);
        var punti = new List<object>();
        if (d?["scadenze"] is JsonArray sc && quando is { } q)
            foreach (var s in sc)
            {
                string g = s!["giorno"]!.GetValue<string>();
                double? v = s["vol_atm"] is JsonValue vv && vv.TryGetValue<double>(out var x) ? x : null;
                if (v is null || v <= 0) continue;
                double giorni = (CatenaAtas.Scade(g, s["tipo"]?.GetValue<string>() ?? "") - q).TotalDays;
                if (giorni > 0) punti.Add(new { scadenza = g, giorni = Math.Round(giorni, 2), vol = Math.Round(v.Value * 100, 2) });
            }
        return new
        {
            id, nome, indice, fonte = "catena", quando = quando is { } qq ? Ms(qq) : (long?)null,
            descrizione = "la volatilita' al prezzo di ogni scadenza, dalla GEX di gex.py (Black-76)", punti,
        };
    }

    private object TermineXray()
    {
        var giorno = _misure.Giorni().FirstOrDefault();
        var punti = new List<object>();
        long? quando = null;
        if (giorno is not null)
            foreach (var sc in Misure.Scadenze)
            {
                var g = _m.Strike($"gex__mm_{sc}", giorno);
                var v = _m.Strike($"vex__mm_{sc}", giorno);
                if (g.Count == 0 || v.Count == 0) continue;
                var cg = g[^1];
                var cv = Misure.Ultima(v, cg.Ts);
                if (cv is null || cv.Strike.Length != cg.Strike.Length) continue;
                var r = Formula.DaVannaGamma(cg.Spot, cg.Strike, cg.Call, cg.Put, cv.Call, cv.Put);
                double giorni = (cg.Scadenza - cg.Ts) / 86_400_000.0;
                if (r is { } x && Formula.MediaVicina(x.sorriso) is { } atm && giorni > 0.02)
                {
                    punti.Add(new
                    {
                        scadenza = Cassa.OraNY(cg.Scadenza).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        giorni = Math.Round(giorni, 2), vol = Math.Round(atm, 2), etichetta = sc,
                    });
                    quando = cg.Ts;
                }
            }
        return new
        {
            id = "termine-xray", nome = "X-Ray (0DTE, prossima, opex)", indice = "SPX", fonte = "xray", quando,
            descrizione = "dal rapporto vanna/gamma di OptionsDepth: solo tre scadenze; lo 0DTE vicino alla chiusura non vale", punti,
        };
    }

    private object TermineCboe(string simbolo, string id, string nome)
    {
        var f = _r.Foto($"cboe-{simbolo}.json");
        return new
        {
            id, nome, indice = "NDX", fonte = "cboe", quando = f?["ts"]?.GetValue<long>(),
            descrizione = "la volatilita' al prezzo di ogni scadenza fino a 120 giorni, dal file pubblico del Cboe",
            punti = f?["termine"]?.DeepClone() ?? new JsonArray(),
        };
    }

    // ---------------------------------------------------------------- la lettura operativa (5 novembre 2026)

    /// <summary>La volatilita' al prezzo di ogni scadenza della catena (in punti percentuali), come TermineGex.</summary>
    private List<(string scadenza, double giorni, double vol)> PuntiTermine(string radice)
    {
        var adesso = DateTime.UtcNow;
        var st = _r.Gex.Stati[radice];
        // la GEX viva solo dopo il suo primo giro: durante il primo giro ha solo una parte delle scadenze
        if (st.Atm.Count > 0 && st.Calcolo is not null)
            return st.Atm.Where(kv => st.Righe.ContainsKey(kv.Key))
                .Select(kv => (kv.Key, (st.Righe[kv.Key][0].ScadeUtc - adesso).TotalDays, kv.Value.vol * 100))
                .Where(x => x.Item2 > 0).OrderBy(x => x.Item2).ToList();
        var (d, _) = LeggiGex(radice);
        var out_ = new List<(string, double, double)>();
        if (d?["scadenze"] is JsonArray sc)
            foreach (var s in sc)
            {
                string g = s!["giorno"]!.GetValue<string>();
                if (s["vol_atm"] is not JsonValue vv || !vv.TryGetValue<double>(out var v) || v <= 0) continue;
                double giorni = (CatenaAtas.Scade(g, s["tipo"]?.GetValue<string>() ?? "") - adesso).TotalDays;
                if (giorni > 0) out_.Add((g, giorni, v * 100));
            }
        return out_.OrderBy(x => x.Item2).ToList();
    }

    private static string GiornoPrima(string giorno)
    {
        var d = DateTime.ParseExact(giorno, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(-1);
        while (d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) d = d.AddDays(-1);
        return d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    /// <summary>La volatilita' di un punto del sorriso a una certa distanza dal forward (moneyness in %), interpolata.</summary>
    private static double? VolA(JsonArray? punti, double moneyness)
    {
        if (punti is null) return null;
        var p = punti.Select(x => (m: D(x?["moneyness"]), v: D(x?["vol"]))).Where(x => x.m is not null && x.v is not null)
            .Select(x => (m: x.m!.Value, v: x.v!.Value)).OrderBy(x => x.m).ToList();
        for (int i = 1; i < p.Count; i++)
            if (p[i - 1].m <= moneyness && p[i].m >= moneyness)
            {
                double w = p[i].m == p[i - 1].m ? 0 : (moneyness - p[i - 1].m) / (p[i].m - p[i - 1].m);
                return p[i - 1].v + w * (p[i].v - p[i - 1].v);
            }
        return null;
    }

    /// <summary>
    /// La volatilita' letta per chi guarda il prezzo (5 novembre 2026, richiesta dell'utente: "ci deve dare comunque
    /// un'indicazione in base a cio' che e' importante"). Per SPX (ES) e NDX (NQ), cinque cose:
    /// - il **livello** a 30 giorni (l'indice ufficiale se c'e', se no la nostra ricostruzione) e la sua **fascia**;
    ///   le soglie sono una convenzione dichiarata, non provata: VIX 15/20/30, VXN 20/26/38 (il VXN sta circa 1,3
    ///   volte sopra il VIX);
    /// - **da stamattina**: la variazione dal primo valore della giornata di New York;
    /// - il **movimento atteso**: oggi fino alla scadenza 0DTE (future x vol al prezzo x radice dei giorni / 365) e
    ///   in una giornata intera (future x vol 30 giorni / radice di 252); e' la deviazione standard, un movimento
    ///   che si supera circa una volta su tre;
    /// - la **struttura a termine**: il breve (1-8 giorni) contro il mese (il punto piu' vicino ai 30 giorni); il
    ///   breve sopra il mese di 3 punti o piu' vuol dire che si teme qualcosa di vicino;
    /// - lo **skew**: quanto costano le put al 5% sotto rispetto alle call al 5% sopra, a 30 giorni; soglie 2 e 6
    ///   punti, convenzione dichiarata.
    /// </summary>
    public RispostaInBreve InBreve()
    {
        var adesso = DateTime.UtcNow;
        string giorno = Raccolta.GiornoNY(adesso);
        var archivio = _r.Serie(giorno);
        var out_ = new List<IndiceInBreve>();
        foreach (var (indice, radice, simbolo, idCatena, soglie, nomeIndice) in new[]
                 {
                     ("SPX", "ES", "^VIX", "vix-catena", new[] { 15.0, 20, 30 }, "VIX"),
                     ("NDX", "NQ", "^VXN", "vxn-catena", new[] { 20.0, 26, 38 }, "VXN"),
                 })
        {
            // il livello: l'ufficiale (Yahoo) se ha un punto nell'ultima mezz'ora, se no la nostra ricostruzione
            var uff = _yahoo.Giorno(simbolo, giorno);
            var cat = archivio.TryGetValue(idCatena, out var sc) ? sc.Select(kv => (ts: kv.Key, v: kv.Value)).ToList() : new();
            bool usaUff = uff.Count > 0 && Ms(adesso) - uff[^1].ts < 30 * 60_000;
            var serie = usaUff ? uff.Select(p => (ts: p.ts, v: p.v)).ToList() : cat;
            string fonteLivello = usaUff ? "yahoo" : "catena";
            // nessuna delle due oggi (il VXN ufficiale fuori dalla cassa, la catena NQ in errore): l'ultimo valore
            // ufficiale, anche di ieri, con la sua ora; la frase lo dice
            if (serie.Count == 0)
            {
                var ieri = uff.Count > 0 ? uff : _yahoo.Giorno(simbolo, GiornoPrima(giorno));
                serie = ieri.Select(p => (ts: p.ts, v: p.v)).ToList();
                fonteLivello = "yahoo";
            }
            double? livello = serie.Count > 0 ? serie[^1].v : null;
            long? quandoLivello = serie.Count > 0 ? serie[^1].ts : null;
            double? apertura = serie.Count > 0 ? serie[0].v : null;
            string? fascia = livello is not { } l ? null : l < soglie[0] ? "calma" : l < soglie[1] ? "normale" : l < soglie[2] ? "tesa" : "paura";

            double? prezzo = _r.PrezzoDi(radice, adesso) ?? _r.Gex.Stati[radice].Future;
            var termine = PuntiTermine(radice);
            var zero = termine.FirstOrDefault(t => t.giorni <= 1.0 && t.scadenza == giorno);
            double? movOggi = prezzo is { } p0 && zero.scadenza is not null ? Math.Round(p0 * zero.vol / 100 * Math.Sqrt(zero.giorni / 365), 0) : null;
            double? movGiorno = prezzo is { } p1 && livello is { } l1 ? Math.Round(p1 * l1 / 100 / Math.Sqrt(252), 0) : null;

            var breve = termine.Where(t => t.giorni is >= 0.5 and <= 8).ToList();
            var mese = termine.Count > 0 ? termine.MinBy(t => Math.Abs(t.giorni - 30)) : default;
            double? volBreve = breve.Count > 0 ? breve.Average(t => t.vol) : null;
            double? volMese = mese.scadenza is not null && Math.Abs(mese.giorni - 30) <= 15 ? mese.vol : null;
            double? pendenza = volBreve is { } vb && volMese is { } vm ? Math.Round(vb - vm, 1) : null;
            string? struttura = pendenza is not { } pd ? null : pd >= 3 ? "inversa" : pd <= -3 ? "normale" : "piatta";

            var sorriso = _r.Foto(radice == "ES" ? "catena-ES.json" : "catena-NQ.json")?["sorriso"];
            var puntiSorriso = sorriso?["punti"] as JsonArray;
            double? putV = VolA(puntiSorriso, -5), callV = VolA(puntiSorriso, 5);
            double? skew = putV is { } pv && callV is { } cv ? Math.Round(pv - cv, 1) : null;
            string? skewTesto = skew is not { } sk ? null : sk >= 6 ? "put care" : sk >= 2 ? "normale" : "put a buon mercato";

            // implicita meno realizzata (5 novembre 2026, dal video sul bias dalle opzioni [V2 · 3:43-3:52]): la volatilita'
            // a 30 giorni (il livello) contro quella vera delle ultime 20 sedute del future. Sopra di 2 punti o piu'
            // l'assicurazione e' cara (le opzioni prezzano piu' movimento di quello che c'e' stato); sotto di 2 o piu',
            // il mercato si muove piu' di quanto le opzioni prezzano. Soglie 2: una convenzione, non provata
            var (real20, real5) = _r.Realizzata(radice);
            double? impMenoReal = livello is { } l2 && real20 is { } r20 ? Math.Round(l2 - r20, 1) : null;
            string? impTesto = impMenoReal is not { } im ? null : im >= 2 ? "assicurazione cara" : im <= -2 ? "si muove più del prezzato" : "in linea";

            // le frasi, nell'ordine in cui contano per il prezzo
            var frasi = new List<string>();
            // all'italiana a mano: il processo gira in modalita' "globalization invariant", senza la cultura it-IT
            string P(double? x) => x is { } v ? v.ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.') : "?";
            string V(double? x) => x is { } v ? v.ToString("0.0", CultureInfo.InvariantCulture).Replace('.', ',') : "?";
            if (livello is not null)
                frasi.Add($"{nomeIndice} a {V(livello)}: volatilit\u00e0 {fascia}" +
                          (apertura is { } ap && livello is { } lv && Math.Abs(lv - ap) >= 0.3
                              ? lv < ap ? $", in calo da stamattina ({V(lv - ap)} punti): la paura si sgonfia" : $", in salita da stamattina (+{V(lv - ap)} punti): la paura cresce"
                              : ", ferma da stamattina") + ".");
            if (movOggi is not null)
                frasi.Add($"Le opzioni di oggi prezzano circa \u00b1{P(movOggi)} punti di {radice} da qui alla chiusura delle 16:00 di New York (uno su tre lo supera).");
            else if (movGiorno is not null)
                frasi.Add($"Una giornata intera vale circa \u00b1{P(movGiorno)} punti di {radice} (dalla volatilit\u00e0 a 30 giorni).");
            if (impMenoReal is { } imr && real20 is { } rv)
                frasi.Add(imr >= 2
                    ? $"Le opzioni costano più del movimento vero: {nomeIndice} {V(livello)} contro {V(rv)} di volatilità realizzata nelle ultime 20 sedute (+{V(imr)}). L'assicurazione è cara: il mercato si aspetta più movimento di quello che c'è stato."
                    : imr <= -2
                        ? $"Il mercato si muove più di quanto le opzioni prezzano: {nomeIndice} {V(livello)} contro {V(rv)} realizzata nelle ultime 20 sedute ({V(imr)}). Succede nelle fasi agitate: le opzioni stanno rincorrendo."
                        : $"Le opzioni prezzano quanto il mercato si è mosso davvero: {nomeIndice} {V(livello)} contro {V(rv)} realizzata nelle ultime 20 sedute.");
            if (struttura == "inversa")
                frasi.Add($"Il breve costa pi\u00f9 del mese (+{V(pendenza)} punti): il mercato teme qualcosa di vicino, un evento o una giornata di movimento.");
            else if (struttura == "normale")
                frasi.Add($"Il breve costa meno del mese ({V(pendenza)} punti): nessun evento temuto a pochi giorni.");
            else if (struttura == "piatta")
                frasi.Add("Il breve e il mese costano uguale: nessuna attesa particolare a pochi giorni.");
            if (skewTesto == "put care")
                frasi.Add($"Le put costano molto pi\u00f9 delle call (+{V(skew)} punti): forte domanda di protezione dal ribasso.");
            else if (skewTesto == "put a buon mercato")
                frasi.Add($"Le put costano quasi come le call ({V(skew)} punti): poca paura del ribasso, il mercato \u00e8 tranquillo o compiacente.");
            else if (skewTesto == "normale")
                frasi.Add($"Le put costano pi\u00f9 delle call di {V(skew)} punti: la normale protezione dal ribasso.");

            out_.Add(new IndiceInBreve(
                indice, radice, nomeIndice,
                livello is { } x1 ? Math.Round(x1, 2) : null, quandoLivello, fonteLivello, fascia, soglie,
                apertura is { } x2 ? Math.Round(x2, 2) : null,
                prezzo, movOggi, zero.scadenza, zero.scadenza is null ? null : Math.Round(zero.vol, 1),
                movGiorno,
                volBreve is { } x3 ? Math.Round(x3, 1) : null, volMese is { } x4 ? Math.Round(x4, 1) : null,
                pendenza, struttura, skew, skewTesto,
                real20, real5, impMenoReal, impTesto,
                frasi,
                // 5 novembre 2026: la fascia dei prezzi del movimento atteso oggi, e il confronto fra le misure stile VIX
                prezzo is { } pf && movOggi is { } mf ? new[] { Math.Round(pf - mf, 2), Math.Round(pf + mf, 2) } : null,
                Confronto(indice, giorno, adesso)));
        }
        return new RispostaInBreve(Ms(adesso), out_);
    }

    public sealed record RispostaInBreve(long Quando, List<IndiceInBreve> Indici);

    /// <summary>Un indice di /api/vol/in-breve: i nomi sono quelli del JSON (camelCase).</summary>
    public sealed record IndiceInBreve(
        string Indice, string Radice, string NomeIndice, double? Livello, long? QuandoLivello, string FonteLivello, string? Fascia,
        double[] Soglie, double? Apertura, double? Prezzo, double? MovimentoOggi, string? ScadenzaOggi, double? VolOggi,
        double? MovimentoGiorno, double? VolBreve, double? VolMese, double? Pendenza, string? Struttura, double? Skew, string? SkewTesto,
        double? Realizzata20, double? Realizzata5, double? ImplicitaMenoRealizzata, string? ImplicitaTesto, List<string> Frasi,
        double[]? FasciaPrezzi, object Confronto);

    /// <summary>Le serie stile VIX (la formula del Cboe a 30 giorni, o l'indice ufficiale) e quelle al prezzo (ATM) di un indice.</summary>
    private static readonly Dictionary<string, (string[] stileVix, string[] atm)> Misure30 = new()
    {
        ["SPX"] = (new[] { "vix-catena", "vix-ufficiale" }, new[] { "atm-xray" }),
        ["NDX"] = (new[] { "vxn-catena", "vxn-ufficiale", "vol30-ndx", "vol30-qqq" }, new[] { "iv30-ndx", "iv30-qqq" }),
    };

    /// <summary>
    /// Il verdetto della volatilità (5 novembre 2026, E.10 del progetto della pagina): si confrontano solo le misure
    /// stile VIX con un punto negli ultimi 30 minuti; convergono se fra la più alta e la più bassa c'è al più un punto.
    /// Le misure al prezzo (ATM) si escludono: pesano meno le ali e stanno sotto il VIX per costruzione. La regola è
    /// quella di quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md, passo 1.
    /// </summary>
    private object Confronto(string indice, string giorno, DateTime adesso)
    {
        var (stile, atm) = Misure30[indice];
        var confrontate = new List<object>();
        var valori = new List<double>();
        var senza = new List<object>();
        bool cassa = Cassa.Dentro(Ms(adesso));
        foreach (var id in stile)
        {
            var d = Descrizioni.First(x => x.id == id);
            var punti = PuntiDi(id, giorno);
            if (punti.Count == 0)
            {
                senza.Add(new { id, nome = d.nome, motivo = d.fonte == "cboe" && !cassa ? "Cboe fuori dalla cassa" : "nessun punto oggi" });
                continue;
            }
            var (ts, v) = punti[^1];
            double minuti = (Ms(adesso) - ts) / 60_000.0;
            if (minuti > 30)
            {
                senza.Add(new
                {
                    id, nome = d.nome,
                    motivo = d.fonte == "cboe" && !cassa ? "Cboe fuori dalla cassa" : $"ultimo punto di {Eta(minuti * 60)} fa",
                });
                continue;
            }
            confrontate.Add(new { id, nome = d.nome, fonte = d.fonte, valore = Math.Round(v, 2), quando = ts });
            valori.Add(v);
        }
        double? ampiezza = valori.Count >= 2 ? Math.Round(valori.Max() - valori.Min(), 2) : null;
        string verdetto = ampiezza is not { } a ? "non si confronta" : a <= 1 ? "convergono" : "divergono";
        return new
        {
            verdetto, ampiezza, sogliaPunti = 1, serieConfrontate = confrontate, senzaDati = senza,
            escluse = atm.Select(id => new { id, motivo = "misura al prezzo (ATM), non stile VIX" }).ToList(),
        };
    }

    /// <summary>I punti di oggi di una serie della volatilità (senza la ATM di X-Ray, che non si confronta).</summary>
    private List<(long ts, double v)> PuntiDi(string id, string giorno) => id switch
    {
        "vix-ufficiale" => _yahoo.Giorno("^VIX", giorno),
        "vxn-ufficiale" => _yahoo.Giorno("^VXN", giorno),
        _ => _r.Serie(giorno).TryGetValue(id, out var s) ? s.Select(kv => (kv.Key, kv.Value)).ToList() : new List<(long, double)>(),
    };

    // ---------------------------------------------------------------- il sorriso

    public object Sorriso(string indice)
    {
        var curve = new List<object>();
        if (indice == "SPX")
        {
            curve.Add(SorrisoFoto("catena-ES.json", "sorriso-es", "ES", "catena"));
            curve.Add(SorrisoXray());
        }
        else
        {
            curve.Add(SorrisoFoto("catena-NQ.json", "sorriso-nq", "NQ", "catena"));
            curve.Add(SorrisoFoto("cboe-NDX.json", "sorriso-ndx", "NDX", "cboe"));
            curve.Add(SorrisoFoto("cboe-QQQ.json", "sorriso-qqq", "QQQ", "cboe"));
        }
        return new { indice, quando = Ms(DateTime.UtcNow), curve };
    }

    private object SorrisoFoto(string file, string id, string nome, string fonte)
    {
        var f = _r.Foto(file);
        var s = f?["sorriso"];
        string? scad = s?["scadenza"]?.GetValue<string>();
        return new
        {
            id, nome = scad is null ? nome : $"{nome} {scad[8..10]}/{scad[5..7]} ({(fonte == "cboe" ? "Cboe" : "catena")})", fonte,
            quando = f?["ts"]?.GetValue<long>(), scadenza = scad, giorni = s?["giorni"]?.GetValue<double>(),
            forward = s?["forward"]?.GetValue<double>(), punti = s?["punti"]?.DeepClone() ?? new JsonArray(),
        };
    }

    private object SorrisoXray()
    {
        var giorno = _misure.Giorni().FirstOrDefault();
        IReadOnlyList<ColStrike> g = giorno is null ? Array.Empty<ColStrike>() : _m.Strike("gex__mm_opex", giorno);
        IReadOnlyList<ColStrike> v = giorno is null ? Array.Empty<ColStrike>() : _m.Strike("vex__mm_opex", giorno);
        if (g.Count > 0 && v.Count > 0)
        {
            var cg = g[^1];
            var cv = Misure.Ultima(v, cg.Ts);
            if (cv is not null && cv.Strike.Length == cg.Strike.Length
                && Formula.DaVannaGamma(cg.Spot, cg.Strike, cg.Call, cg.Put, cv.Call, cv.Put) is { } r)
            {
                string scad = Cassa.OraNY(cg.Scadenza).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                return new
                {
                    id = "sorriso-xray", nome = $"X-Ray opex {scad[8..10]}/{scad[5..7]}", fonte = "xray", quando = cg.Ts, scadenza = scad,
                    giorni = Math.Round((cg.Scadenza - cg.Ts) / 86_400_000.0, 2), forward = Math.Round(r.forward, 2),
                    // +-12% come le altre fonti. Strike per strike il rapporto vanna/gamma salta di qualche punto (a denti
                    // di sega, anche sugli strike pesanti: 2 novembre 2026), quindi la curva e' la mediana mobile su 7 strike
                    descrizione = "dal rapporto vanna/gamma di OptionsDepth; mediana mobile su 7 strike, perche' strike per strike salta di qualche punto",
                    punti = Mediana(r.sorriso.Where(p => Math.Abs(p.moneyness) <= 12).ToList(), 7)
                        .Select(p => new { moneyness = p.moneyness, strike = p.strike, vol = p.vol }),
                };
            }
        }
        return new { id = "sorriso-xray", nome = "X-Ray opex", fonte = "xray", quando = (long?)null, scadenza = (string?)null,
            giorni = (double?)null, forward = (double?)null, punti = Array.Empty<object>() };
    }

    /// <summary>La mediana mobile della volatilita' su `n` strike vicini (centrata; ai bordi la finestra si accorcia).</summary>
    public static List<(double strike, double moneyness, double vol)> Mediana(List<(double strike, double moneyness, double vol)> s, int n)
    {
        var out_ = new List<(double, double, double)>(s.Count);
        for (int i = 0; i < s.Count; i++)
        {
            int a = Math.Max(0, i - n / 2), b = Math.Min(s.Count - 1, i + n / 2);
            var v = s.Skip(a).Take(b - a + 1).Select(x => x.vol).OrderBy(x => x).ToList();
            double m = v.Count % 2 == 1 ? v[v.Count / 2] : (v[v.Count / 2 - 1] + v[v.Count / 2]) / 2;
            out_.Add((s[i].strike, s[i].moneyness, Math.Round(m, 3)));
        }
        return out_;
    }

    // ---------------------------------------------------------------- la GEX di NQ da tre fonti

    public static string? UltimaCacheGex(string radice)
    {
        string casa = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Directory.GetFiles(casa, $".fabio-gex-{radice}*.json").OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault();
    }

    private static (JsonNode? dati, DateTime? quando) LeggiGex(string radice)
    {
        var f = UltimaCacheGex(radice);
        if (f is null) return (null, null);
        try
        {
            using var fs = new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var r = new StreamReader(fs, Encoding.UTF8);
            var d = JsonNode.Parse(r.ReadToEnd());
            var q = DateTime.Parse(d!["quando"]!.GetValue<string>(), CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            return (d, q);
        }
        catch (Exception ex) when (ex is IOException or System.Text.Json.JsonException or FormatException or NullReferenceException)
        {
            return (null, null);
        }
    }

    /// <summary>Il calcolo di gex.py archiviato in quant/dati/&lt;radice&gt;/gex/&lt;giorno&gt;/ piu' vicino a ts (non dopo).</summary>
    private (JsonNode? dati, DateTime? quando) StoriaGex(string radice, string giorno, long ts)
    {
        string dir = Path.GetFullPath(Path.Combine(_r.Dati, "..", radice, "gex", giorno));
        if (!Directory.Exists(dir)) return (null, null);
        foreach (var f in Directory.GetFiles(dir, "*Z-*.json").OrderByDescending(x => x))
        {
            try
            {
                var d = JsonNode.Parse(File.ReadAllText(f, Encoding.UTF8));
                var q = DateTime.Parse(d!["quando"]!.GetValue<string>(), CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                if (Ms(q) <= ts) return (d, q);
            }
            catch (Exception ex) when (ex is IOException or System.Text.Json.JsonException or FormatException or NullReferenceException)
            {
            }
        }
        return (null, null);
    }

    private static double? D(JsonNode? n) => n is JsonValue v && v.TryGetValue<double>(out var x) ? x : null;

    /// <summary>
    /// La GEX di NQ da tre fonti, in prezzi di NQ. `ambito` = "tutte" (scadenze fino a 70 giorni) o "0dte" (la
    /// scadenza di oggi: per NQ la prima della catena di gex.py, se e' di oggi; per QQQ e NDX quella di oggi
    /// del file Cboe, che fuori dalla cassa e' del giorno del file).
    /// </summary>
    public object NqGex(string ambito)
    {
        var adesso = DateTime.UtcNow;
        bool zero = ambito == "0dte";
        var (fonti, prezzoNQ) = FontiNq(zero, adesso);
        // il prezzo vero di QQQ, anche fuori dalla cassa (Yahoo), accanto a quello del file del Cboe (5 novembre 2026)
        var y = _yahoo.Prezzo("QQQ");
        var fq = _r.Foto("cboe-QQQ.json");
        object? qqq = y is null ? null : new
        {
            prezzo = y.Prezzo, quando = y.Quando, secondiFa = Math.Round((Ms(adesso) - y.Quando) / 1000.0),
            sessione = y.Sessione, chiusuraPrima = y.ChiusuraPrima,
            variazione = y.ChiusuraPrima is { } cp && cp > 0 ? Math.Round((y.Prezzo / cp - 1) * 100, 2) : (double?)null,
            rapportoNQ = prezzoNQ is { } pn && y.Prezzo > 0 ? Math.Round(pn / y.Prezzo, 4) : (double?)null,
            prezzoCboe = D(fq?["prezzo"]), quandoCboe = fq?["prezzoQuando"] is JsonValue qc && qc.TryGetValue<long>(out var qcv) ? qcv : (long?)null,
            fonte = "yahoo",
        };
        // 5 novembre 2026: un'ora fa, il movimento atteso, la finestra del profilo e la scala dei livelli (E.12)
        var punti = PuntiGexViva("NQ");
        double? mov = InBreve().Indici.FirstOrDefault(i => i.Indice == "NDX")?.MovimentoOggi;
        double[]? finestra = prezzoNQ is not { } pf ? null
            : mov is { } m ? new[] { Math.Round(pf - 2 * m, 2), Math.Round(pf + 2 * m, 2) }
            : new[] { Math.Round(pf * 0.98, 2), Math.Round(pf * 1.02, 2) };
        return new
        {
            quando = Ms(adesso), prezzoNQ, ambito = zero ? "0dte" : "tutte",
            unita = "milioni di $ per 1% di movimento, convenzione A (dealer lunghi call, corti put)",
            fonti, qqq,
            unOraFa = UnOraFa(punti),
            movimentoOggi = mov,
            finestra,
            scala = ScalaNq(prezzoNQ, adesso),
        };
    }

    /// <summary>
    /// La scala dei livelli di NQ (E.3, soglia 25 punti): zero gamma, zero 0DTE, call e put wall (tutte e 0DTE) di ogni
    /// fonte, gia' in prezzi di NQ, e i bordi del valore di NQ dal bridge.
    /// </summary>
    private List<RigaScala> ScalaNq(double? prezzo, DateTime adesso)
    {
        var voci = new List<VoceScala>();
        foreach (var zero in new[] { false, true })
        {
            foreach (var f in FontiNq(zero, adesso).fonti)
            {
                var n = System.Text.Json.JsonSerializer.SerializeToNode(f, Raccolta.Json)!.AsObject();
                string id = n["id"]!.GetValue<string>();
                string fonte = n["fonte"]!.GetValue<string>();
                string chi = id switch { "gex-qqq" => " (Cboe QQQ)", "gex-ndx" => " (Cboe NDX)", _ => "" };
                string suffisso = zero ? " 0DTE" : "";
                double? fa = D(n["secondiFa"]);
                string scad = zero ? "0dte-nq" : "tutte";
                foreach (var (k, cosa, tipo) in new[]
                         {
                             ("zeroGamma", "zero gamma", zero ? "zero-0dte" : "zero"), ("callWall", "call wall", "call-wall"),
                             ("putWall", "put wall", "put-wall"),
                         })
                    if (D(n[k]) is { } p)
                        voci.Add(new VoceScala(cosa + suffisso + chi, tipo, fonte, scad, null, fa, p, "adesso", null));
            }
        }
        if (_bordi is not null)
            foreach (var b in _bordi.Di("NQ").bordi)
                voci.Add(new VoceScala(b.Nome, "bordo", "atas", null, null, null, b.Prezzo, "adesso", null));
        return Scala.Fondi(voci, prezzo, Scala.SogliaNq);
    }

    /// <summary>I punti della GEX viva di una radice (gex-&lt;radice&gt;.jsonl) del giorno piu' recente che ne ha, fra gli
    /// ultimi quattro, in ordine di tempo: come <c>xray_query.gex_viva</c>.</summary>
    public List<JsonObject> PuntiGexViva(string radice)
    {
        var punti = new List<JsonObject>();
        if (!Directory.Exists(_r.Dati)) return punti;
        foreach (var dir in Directory.GetDirectories(_r.Dati).Select(Path.GetFileName).OfType<string>()
                     .OrderByDescending(g => g, StringComparer.Ordinal).Take(4))
        {
            string path = Path.Combine(_r.Dati, dir, $"gex-{radice}.jsonl");
            if (!File.Exists(path)) continue;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using (var r = new StreamReader(fs, Encoding.UTF8))
                foreach (var riga in r.ReadToEnd().Split('\n'))
                {
                    if (riga.Length < 2) continue;
                    try { if (JsonNode.Parse(riga) is JsonObject o && o["ts"] is JsonValue) punti.Add(o); }
                    catch (System.Text.Json.JsonException) { }
                }
            if (punti.Count > 0) break;
        }
        return punti.OrderBy(p => p["ts"]!.GetValue<long>()).ToList();
    }

    /// <summary>Il punto di un'ora prima dell'ultimo (il primo del giorno, se l'archivio e' piu' corto).</summary>
    public static JsonObject? UnOraFa(List<JsonObject> punti)
    {
        if (punti.Count == 0) return null;
        long ultimo = punti[^1]["ts"]!.GetValue<long>();
        return punti.LastOrDefault(p => p["ts"]!.GetValue<long>() <= ultimo - 60 * 60_000) ?? punti[0];
    }

    private (List<object> fonti, double? prezzoNQ) FontiNq(bool zero, DateTime adesso)
    {
        string oggiNY = Raccolta.GiornoNY(adesso);
        var fonti = new List<object>();
        var (d, quando) = LeggiGex("NQ");
        // il prezzo di adesso dal bridge; quello di gex.py solo se il bridge non risponde (puo' avere ore)
        double? prezzoNQ = _r.PrezzoNQ(adesso) ?? D(d?["prezzo_bridge"]?["prezzo"]) ?? D(d?["future"]);
        var viva = _r.Gex.Stati["NQ"];
        if (viva.Calcolo is { } calc && viva.Tutte is not null)
        {
            prezzoNQ = viva.Future;
            var prof = zero ? viva.Oggi : viva.Tutte;
            fonti.Add(new
            {
                id = "gex-nq", nome = $"{viva.Strumento} (catena CME, viva)", fonte = "catena", quando = Ms(calc),
                secondiFa = Math.Round((adesso - calc).TotalSeconds),
                conversione = (object?)null,
                scadenze = zero ? viva.GiornoZero : $"{viva.Righe.Count} scadenze fino a 70 giorni",
                nota = zero && prof is null ? $"nessuna scadenza di oggi: la piu' vicina e' del {viva.GiornoZero}"
                    : $"OI di ieri · 0DTE ogni minuto, le altre a rotazione (la piu' vecchia delle {Cassa.OraNY(Ms(viva.CatenaIntera!.Value)):HH:mm} NY)",
                totale = prof?.Totale, zeroGamma = prof?.Zero, callWall = prof?.CallWall, putWall = prof?.PutWall,
                strike = (prof?.Strike ?? new()).Select(x => new { prezzo = (double?)x.strike, gex = (double?)Math.Round(x.call + x.put, 2),
                    call = (double?)x.call, put = (double?)x.put }).ToList(),
            });
        }
        else if (d is not null)
        {
            var prof = zero ? d["prima"] : d["tutte"];
            string? giornoPrima = d["prima"]?["giorno"]?.GetValue<string>();
            var righe = zero ? d["per_strike_prima"] as JsonArray : d["per_strike"] as JsonArray;
            string? nota = zero
                ? giornoPrima != oggiNY ? $"la scadenza piu' vicina della catena e' del {giornoPrima}, non di oggi"
                : righe is null ? "per strike non ancora calcolato (gex.py dopo il 2/11 lo scrive): solo i totali" : null
                : null;
            fonti.Add(new
            {
                id = "gex-nq", nome = "NQ (catena CME, gex.py)", fonte = "catena", quando = quando is { } q ? Ms(q) : (long?)null,
                secondiFa = quando is { } q2 ? Math.Round((adesso - q2).TotalSeconds) : (double?)null,
                conversione = (object?)null, scadenze = zero ? giornoPrima : "tutte fino a 70 giorni", nota,
                totale = D(prof?["tot_a"]) is { } t ? Math.Round(t, 1) : (double?)null,
                zeroGamma = D(prof?["zero"]) is { } z ? Math.Round(z, 2) : (double?)null,
                callWall = D(prof?["call_wall"]), putWall = D(prof?["put_wall"]),
                strike = (righe ?? new JsonArray())
                    .Select(s => new { prezzo = D(s!["k"]), gex = D(s["gex_netta"]), call = D(s["gex_c"]), put = D(s["gex_p"]) })
                    .ToList(),
            });
        }
        foreach (var (simbolo, id) in new[] { ("QQQ", "gex-qqq"), ("NDX", "gex-ndx") })
        {
            var f = _r.Foto($"cboe-{simbolo}.json");
            var conv = f?["conversione"];
            if (f is null || conv is null) continue;
            bool rapporto = conv["tipo"]?.GetValue<string>() == "rapporto";
            double val = D(conv["valore"]) ?? double.NaN;
            double? inNQ(double? x) => x is { } v && double.IsFinite(val) ? Math.Round(rapporto ? v * val : v + val, 2) : null;
            var g = zero ? f["gex0dte"] : f["gex"];
            long ts = f["ts"]!.GetValue<long>();
            string? giorno = zero ? g?["giorno"]?.GetValue<string>() : null;
            fonti.Add(new
            {
                id, nome = $"{simbolo} (Cboe, in prezzi di NQ)", fonte = "cboe", quando = ts,
                secondiFa = Math.Round((Ms(adesso) - ts) / 1000.0),
                conversione = conv.DeepClone(),
                scadenze = zero ? giorno : "tutte fino a 70 giorni",
                nota = zero && g is null ? "nessuna scadenza di oggi nel file del Cboe (il file e' di un altro giorno?)"
                    : zero && giorno != oggiNY ? $"lo 0DTE del file del Cboe e' del {giorno}" : null,
                totale = D(g?["totale"]), zeroGamma = inNQ(D(g?["zeroGamma"])), callWall = inNQ(D(g?["callWall"])), putWall = inNQ(D(g?["putWall"])),
                strike = (g?["strike"] as JsonArray ?? new JsonArray())
                    .Select(s => new { prezzo = inNQ(D(s!["strike"])), gex = D(s["netta"]), call = D(s["call"]), put = D(s["put"]), strikeOriginale = D(s["strike"]) })
                    .ToList(),
            });
        }
        return (fonti, prezzoNQ);
    }

    /// <summary>La GEX viva di un giorno, minuto per minuto (gex-&lt;radice&gt;.jsonl): prezzo, totale, zero gamma e muri.</summary>
    public object GexTempo(string radice, string giorno)
    {
        string path = Path.Combine(_r.Dati, giorno, $"gex-{radice}.jsonl");
        var punti = new List<JsonNode>();
        if (File.Exists(path))
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var r = new StreamReader(fs, Encoding.UTF8);
            foreach (var riga in r.ReadToEnd().Split('\n'))
            {
                if (riga.Length < 2) continue;
                try { if (JsonNode.Parse(riga) is { } n) punti.Add(n); }
                catch (System.Text.Json.JsonException) { }
            }
        }
        return new
        {
            radice, giorno, fonte = "catena",
            descrizione = "la GEX viva della catena CME via ATAS, ricalcolata ogni minuto (convenzione A, open interest di ieri)",
            punti,
        };
    }

    // ---------------------------------------------------------------- X-Ray contro la catena ES

    public object EsConfronto(string giorno, long? richiesto)
    {
        var tempi = _m.Strike("gex__mm_0dte", giorno);
        long ts = richiesto ?? (tempi.Count > 0 ? tempi[^1].Ts : 0);
        double? baseV = _misure.Base(giorno).Valore;
        var es = _ponte.EsAl(giorno, ts)?.prezzo;

        // X-Ray: la GEX dei market maker delle tre scadenze, sommata per strike (SPX), portata in ES con la base
        var xray = new SortedDictionary<double, double>();
        long? quandoX = null;
        foreach (var sc in Misure.Scadenze)
        {
            var c = Misure.Ultima(_m.Strike($"gex__mm_{sc}", giorno), ts);
            if (c is null) continue;
            quandoX = c.Ts;
            for (int i = 0; i < c.Strike.Length; i++)
            {
                double k = c.Strike[i] + (baseV ?? 0);
                xray[k] = xray.GetValueOrDefault(k) + c.Netto(i);
            }
        }
        double centro = es ?? (xray.Count > 0 ? xray.Keys.ElementAt(xray.Count / 2) : 0);

        var catena = new SortedDictionary<double, double>();
        DateTime? quandoC;
        var vivaEs = _r.Gex.Stati["ES"];
        bool oggi = giorno == Raccolta.GiornoNY(DateTime.UtcNow);
        if (oggi && vivaEs.Tutte is not null && vivaEs.Calcolo is not null)
        {
            // la GEX viva di ES: al prezzo dell'ultimo calcolo, non a quello di ts (per un giorno passato vale gex.py)
            foreach (var x in vivaEs.Tutte.Strike) catena[x.strike] = Math.Round(x.call + x.put, 2);
            quandoC = vivaEs.Calcolo;
        }
        else
        {
            // un giorno passato (o la GEX viva non ancora pronta): l'ultimo calcolo di gex.py archiviato prima di ts
            var (d, q) = oggi ? LeggiGex("ES") : StoriaGex("ES", giorno, ts);
            quandoC = q;
            foreach (var s in d?["per_strike"] as JsonArray ?? new JsonArray())
                if (D(s!["k"]) is { } k && D(s["gex_netta"]) is { } g) catena[k] = g;
        }

        object Curva(string id, string nome, string fonte, string unita, SortedDictionary<double, double> v, long? q)
        {
            var dentro = v.Where(kv => Math.Abs(kv.Key / centro - 1) <= 0.03).ToList();
            double max = dentro.Count > 0 ? dentro.Max(kv => Math.Abs(kv.Value)) : 0;
            return new
            {
                id, nome, fonte, unita, quando = q,
                punti = dentro.Select(kv => new
                {
                    prezzo = Math.Round(kv.Key, 2), valore = Math.Round(kv.Value, 3),
                    normalizzata = max > 0 ? Math.Round(kv.Value / max, 4) : 0,
                }).ToList(),
            };
        }

        // l'accordo: sugli strike di ES (X-Ray arrotondato allo strike da 5 punti piu' vicino), dove le due
        // curve pesano almeno il 5% del loro massimo, quante volte il segno e' lo stesso
        double maxX = xray.Where(kv => Math.Abs(kv.Key / centro - 1) <= 0.03).Select(kv => Math.Abs(kv.Value)).DefaultIfEmpty(0).Max();
        double maxC = catena.Where(kv => Math.Abs(kv.Key / centro - 1) <= 0.03).Select(kv => Math.Abs(kv.Value)).DefaultIfEmpty(0).Max();
        var xr = xray.GroupBy(kv => Math.Round(kv.Key / 5) * 5).ToDictionary(g => g.Key, g => g.Sum(kv => kv.Value));
        int uguali = 0, totali = 0;
        foreach (var (k, c) in catena)
        {
            if (Math.Abs(k / centro - 1) > 0.03 || !xr.TryGetValue(k, out var x)) continue;
            if (maxX == 0 || maxC == 0 || Math.Abs(x) < 0.05 * maxX || Math.Abs(c) < 0.05 * maxC) continue;
            totali++;
            if (Math.Sign(x) == Math.Sign(c)) uguali++;
        }
        return new
        {
            giorno, ts, quando = quandoX, prezzoES = es, baseEsSpx = baseV,
            curve = new[]
            {
                Curva("xray-mm", "X-Ray, market maker (0DTE + prossima + opex)", "xray", "unita' di OptionsDepth", xray, quandoX),
                Curva("catena-es", "Catena ES, convenzione A (tutte le scadenze a 70 giorni)", "catena", "milioni di $ per 1%", catena,
                    quandoC is { } qc ? Ms(qc) : null),
            },
            accordo = new
            {
                segnoUguale = totali > 0 ? Math.Round((double)uguali / totali, 3) : (double?)null,
                strike = totali,
                nota = "quota degli strike entro il 3% dove le due curve hanno lo stesso segno (pesi sotto il 5% del massimo esclusi); " +
                       "X-Ray stima il segno dei market maker, la catena lo suppone (convenzione A)",
            },
        };
    }
}
