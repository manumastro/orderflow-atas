namespace XrayLive.Server;

// Le misure che la pagina disegna. Il client non calcola: riceve numeri gia' pronti, in prezzi
// di SPX, e la base ES-SPX per portarli su ES. Ogni risposta dice di quale istante e', e se lo
// spot di quell'istante e' vivo (cassa) o ricostruito da ES meno la base.

internal sealed class Misure
{
    public static readonly string[] Scadenze = { "0dte", "next_expiry", "opex" };
    public static readonly string[] Gruppi = { "firm", "broker", "procust", "nonprocust" };

    private readonly Magazzino _m;
    private readonly Ponte _ponte;
    private readonly Bordi? _bordi;

    public Misure(Magazzino m, Ponte ponte, Bordi? bordi = null)
    {
        _m = m;
        _ponte = ponte;
        _bordi = bordi;
    }

    /// <summary>I bordi del valore di ES (VAL, VAH, POC, muri del ping-pong), in prezzi di ES; vuoti senza bridge.</summary>
    public (List<Bordo> bordi, string? nota) BordiEs(string giorno) =>
        _bordi?.Di("ES", giorno) ?? (new List<Bordo>(), "bordi del valore non configurati");

    // ---------------------------------------------------------------- i giorni e gli istanti

    public List<string> Giorni() =>
        Lettura.Giorni(_m.Root, "gex__mm_0dte").Union(Lettura.Giorni(_m.Root, "observed__gamma"))
            .OrderByDescending(g => g, StringComparer.Ordinal).ToList();

    public object Istanti(string giorno)
    {
        var ts = Tempi(giorno);
        var (apertura, chiusura) = Cassa.Di(giorno);
        return new { giorno, istanti = ts, cassa = new { apertura, chiusura } };
    }

    /// <summary>Gli istanti della giornata: quelli della GEX 0DTE, o del profilo osservato se manca.</summary>
    public List<long> Tempi(string giorno)
    {
        var t = _m.Strike("gex__mm_0dte", giorno).Select(c => c.Ts).Distinct().ToList();
        if (t.Count == 0) t = _m.Piano("observed__gamma", giorno).Select(c => c.Ts).Distinct().ToList();
        t.Sort();
        return t;
    }

    // ---------------------------------------------------------------- il prezzo di riferimento

    /// <summary>
    /// Il prezzo di SPX a cui si leggono le misure: lo spot se l'istante e' in cassa; fuori, ES
    /// meno la base (lo spot resta fermo all'ultima chiusura, verificato il 30 ottobre 2026).
    /// </summary>
    public sealed record Riferimento(double Prezzo, string Fonte, bool Vivo, double SpotX, double? Es);

    public Ponte.Base Base(string giorno)
    {
        var b = _ponte.BaseDi(giorno, SpotDelGiorno(giorno));
        if (b.Valore is not null) return b;
        // prima della cassa (o in un giorno senza cassa) vale la base dell'ultima seduta che ce l'ha
        foreach (var g in Giorni().Where(g => string.CompareOrdinal(g, giorno) < 0).Take(5))
        {
            var prima = _ponte.BaseDi(g, SpotDelGiorno(g));
            if (prima.Valore is not null) return prima with { Fonte = prima.Fonte + $", dalla seduta del {g}" };
        }
        return b;
    }

    private List<(long, double)> SpotDelGiorno(string giorno) =>
        _m.Strike("gex__mm_0dte", giorno).Select(c => (c.Ts, c.Spot)).ToList();

    public Riferimento RiferimentoA(string giorno, long ts, double spotX, double? baseV)
    {
        if (Cassa.Dentro(ts) && double.IsFinite(spotX) && spotX > 0)
            return new Riferimento(spotX, "spot SPX", true, spotX, _ponte.EsAl(giorno, ts)?.prezzo);
        var es = _ponte.EsAl(giorno, ts);
        if (es is { } e && baseV is { } b)
            return new Riferimento(e.prezzo - b, "ES meno la base", false, spotX, e.prezzo);
        return new Riferimento(spotX, "spot SPX fermo (fuori dalla cassa, senza ES)", false, spotX, es?.prezzo);
    }

    // ---------------------------------------------------------------- adesso

    /// <summary>Le misure di base di un istante, tipizzate: le usano /api/adesso e le quattro domande.</summary>
    public sealed record Fotografia(
        string Giorno, long Ts, Riferimento Rif, Ponte.Base Base, ColPiano? Piano, double? Gamma,
        List<(string scadenza, ColStrike? c)> Gex, List<double> Zeri, List<Picco> Positivi, List<Picco> Negativi);

    public Fotografia FotografiaAl(string giorno, long? richiesto)
    {
        var tempi = Tempi(giorno);
        long ts = richiesto ?? (tempi.Count > 0 ? tempi[^1] : 0);
        var baseG = Base(giorno);
        var gex = Scadenze.Select(s => (s, Ultima(_m.Strike("gex__mm_" + s, giorno), ts))).ToList();
        double spotX = gex.Select(x => x.Item2?.Spot ?? double.NaN).FirstOrDefault(double.IsFinite, double.NaN);
        var rif = RiferimentoA(giorno, ts, spotX, baseG.Valore);
        var piano = Ultima(_m.Piano("observed__gamma", giorno), ts);
        double? gamma = piano is null ? null : R(Interpola(piano.Prezzi, piano.Valori, rif.Prezzo), 2);
        var (zeri, pos, neg) = LivelliAl(giorno, ts);
        return new Fotografia(giorno, ts, rif, baseG, piano, gamma, gex, zeri, pos, neg);
    }

    public object Adesso(string giorno, long? richiesto)
    {
        var f = FotografiaAl(giorno, richiesto);
        long ts = f.Ts;
        var baseG = f.Base;
        var gex = f.Gex;
        var rif = f.Rif;
        var piano = f.Piano;
        double? gamma = f.Gamma;
        var charmP = Ultima(_m.Piano("observed__charm", giorno), ts);
        double? charm = charmP is null ? null : R(Interpola(charmP.Prezzi, charmP.Valori, rif.Prezzo), 2);

        var livelli = (zeri: f.Zeri, positivi: f.Positivi, negativi: f.Negativi);
        var outlook = Outlook(giorno);

        var pr = _m.Pressione(giorno);
        var p = Ultima(pr, ts);
        var p30 = Ultima(pr, ts - 30 * 60_000);

        return new
        {
            giorno,
            ts,
            riferimento = Rif(rif),
            @base = baseG,
            // da: il primo istante da cui il segno al prezzo e' lo stesso di adesso (5 novembre 2026)
            gamma = new { valore = gamma, regime = Regime(gamma), istante = piano?.Ts, da = GammaDa(giorno, ts, gamma) },
            charm = new { valore = charm, istante = charmP?.Ts },
            flip = livelli.zeri.OrderBy(z => Math.Abs(z - rif.Prezzo)).Select(z => R(z, 2)).ToList(),
            flipVicino = FlipVicino(piano, livelli.zeri, rif.Prezzo),
            picchiPositivi = livelli.positivi,
            picchiNegativi = livelli.negativi,
            outlook,
            scala = ScalaEs(f, outlook),
            gex = gex.Select(x => Esposizione(x.scadenza, x.c, rif.Prezzo)).ToList(),
            vex = Scadenze.Select(s => Esposizione(s, Ultima(_m.Strike("vex__mm_" + s, giorno), ts), rif.Prezzo)).ToList(),
            dex = Gruppi.Select(g =>
            {
                var z = DexCumulato(giorno, $"dex__{g}_0dte", ts);
                var t = DexCumulato(giorno, $"dex__{g}_all", ts);
                return new
                {
                    gruppo = g,
                    zeroDte = z.totale,
                    zeroDteTrentaMinuti = z.trentaMinuti,
                    tutte = t.totale,
                    tutteTrentaMinuti = t.trentaMinuti,
                };
            }).ToList(),
            pressione = p is null ? null : new
            {
                delta = R(p.Delta, 2),
                trades = R(p.Trades, 2),
                trentaMinutiPrima = R(p30?.Delta ?? double.NaN, 2),
                istante = p.Ts,
                // il verso del segno di pressure__0dte non e' scritto in quant/metodo/x-ray-in-vivo.md (5 novembre
                // 2026): il server non lo inventa, e la pagina scrive "verso non dichiarato"
                verso = (string?)null,
                unita = (string?)null,
            },
        };
    }

    // ---------------------------------------------------------------- le risposte nuove di adesso (5 novembre 2026)

    /// <summary>
    /// Il primo istante da cui la gamma osservata al prezzo ha lo stesso segno di adesso: si scorrono all'indietro
    /// le colonne di observed__gamma del giorno, ciascuna letta al prezzo di riferimento del suo istante.
    /// </summary>
    private long? GammaDa(string giorno, long ts, double? adesso)
    {
        if (adesso is not { } g0 || g0 == 0) return null;
        var piano = _m.Piano("observed__gamma", giorno);
        var colonne = piano.Where(c => c.Ts <= ts).ToList();
        if (colonne.Count == 0) return null;
        var rif = SerieRiferimento(giorno, colonne.Select(c => c.Ts).ToList());
        long? da = null;
        for (int i = colonne.Count - 1; i >= 0; i--)
        {
            double v = Interpola(colonne[i].Prezzi, colonne[i].Valori, rif[i].Prezzo);
            if (!double.IsFinite(v)) continue;
            if (Math.Sign(v) != Math.Sign(g0)) break;
            da = colonne[i].Ts;
        }
        return da;
    }

    /// <summary>Il flip più vicino al prezzo, e il segno della gamma osservata un passo di griglia sopra e sotto.</summary>
    private static object? FlipVicino(ColPiano? piano, List<double> zeri, double rif)
    {
        if (zeri.Count == 0 || !double.IsFinite(rif)) return null;
        double z = zeri.MinBy(x => Math.Abs(x - rif));
        string? sopra = null, sotto = null;
        if (piano is not null && piano.Prezzi.Length > 1)
        {
            double passo = Math.Abs(piano.Prezzi[1] - piano.Prezzi[0]);
            double su = Interpola(piano.Prezzi, piano.Valori, z + passo), giu = Interpola(piano.Prezzi, piano.Valori, z - passo);
            sopra = double.IsFinite(su) ? Regime(su) : null;
            sotto = double.IsFinite(giu) ? Regime(giu) : null;
        }
        return new { prezzo = Math.Round(z, 2), distanza = Math.Round(z - rif, 2), sopra, sotto };
    }

    /// <summary>
    /// La scala dei livelli di ES in prezzi di SPX (E.3): i flip, i picchi, i muri freno e acceleratore di ogni
    /// scadenza e i bordi del valore (portati in SPX togliendo la base), più gli zeri e i picchi della prossima seduta.
    /// </summary>
    private List<RigaScala> ScalaEs(Fotografia f, OutlookLivelli? outlook)
    {
        var voci = new List<VoceScala>();
        void Voce(string cosa, string tipo, string fonte, string? scadenza, double? peso, double prezzo, string quando = "adesso", string? seduta = null) =>
            voci.Add(new VoceScala(cosa, tipo, fonte, scadenza, peso, null, prezzo, quando, seduta));
        foreach (var z in f.Zeri) Voce("flip", "flip", "xray", null, null, z);
        for (int i = 0; i < f.Positivi.Count; i++)
            Voce(i == 0 ? "freno più forte" : "freno", "freno", "xray", null, f.Positivi[i].Peso, f.Positivi[i].Strike);
        for (int i = 0; i < f.Negativi.Count; i++)
            Voce(i == 0 ? "acceleratore più forte" : "acceleratore", "acceleratore", "xray", null, f.Negativi[i].Peso, f.Negativi[i].Strike);
        foreach (var (s, c) in f.Gex)
        {
            if (c is null) continue;
            var (freno, acc) = Muri(c, f.Rif.Prezzo);
            if (freno is { } a) Voce($"muro freno {Scala.NomeScadenza(s)}", "muro-freno", "xray", s, R(c.Netto(a), 3), c.Strike[a]);
            if (acc is { } b) Voce($"muro acceleratore {Scala.NomeScadenza(s)}", "muro-acceleratore", "xray", s, R(c.Netto(b), 3), c.Strike[b]);
        }
        if (f.Base.Valore is { } baseV)
            foreach (var b in BordiEs(f.Giorno).bordi)
                Voce(b.Nome, "bordo", "atas", null, null, Math.Round(b.Prezzo - baseV, 2));
        if (outlook is not null)
        {
            foreach (var z in outlook.Zeri) Voce("flip", "flip", "xray", null, null, z, "prossima seduta", outlook.Seduta);
            for (int i = 0; i < outlook.Positivi.Count; i++)
                Voce(i == 0 ? "freno più forte" : "freno", "freno", "xray", null, outlook.Positivi[i].Peso, outlook.Positivi[i].Strike,
                    "prossima seduta", outlook.Seduta);
            for (int i = 0; i < outlook.Negativi.Count; i++)
                Voce(i == 0 ? "acceleratore più forte" : "acceleratore", "acceleratore", "xray", null, outlook.Negativi[i].Peso,
                    outlook.Negativi[i].Strike, "prossima seduta", outlook.Seduta);
        }
        return Scala.Fondi(voci, f.Rif.Prezzo, Scala.SogliaEs);
    }

    /// <summary>Il muro freno (il netto più positivo) e il muro acceleratore (il più negativo) entro il 5% dal prezzo.</summary>
    private static (int? freno, int? acceleratore) Muri(ColStrike c, double rif)
    {
        var vicini = Enumerable.Range(0, c.Strike.Length).Where(i => Math.Abs(c.Strike[i] - rif) <= rif * 0.05).ToList();
        int? piu = vicini.Where(i => c.Netto(i) > 0).OrderByDescending(c.Netto).Cast<int?>().FirstOrDefault();
        int? meno = vicini.Where(i => c.Netto(i) < 0).OrderBy(c.Netto).Cast<int?>().FirstOrDefault();
        return (piu, meno);
    }

    /// <summary>
    /// La seduta di una finestra [da, fino]: la data di New York della prima apertura di cassa (09:30 di New York, un
    /// giorno feriale) che cade dentro; se non ce n'è nessuna, la data di New York di <paramref name="fino"/>.
    /// </summary>
    public static string? SedutaDi(long da, long fino)
    {
        if (da <= 0 || fino <= 0 || fino < da) return null;
        var g = Cassa.OraNY(da).Date;
        var ultimo = Cassa.OraNY(fino).Date;
        for (; g <= ultimo; g = g.AddDays(1))
        {
            if (g.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
            string s = g.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            long apertura = Cassa.Di(s).apertura;
            if (apertura >= da && apertura <= fino) return s;
        }
        return ultimo.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    }

    private object Esposizione(string scadenza, ColStrike? c, double rif)
    {
        if (c is null) return new { scadenza, presente = false };
        int n = c.Strike.Length;
        var vicini = Enumerable.Range(0, n).Where(i => Math.Abs(c.Strike[i] - rif) <= rif * 0.05).ToList();
        int? muroPiu = vicini.Where(i => c.Netto(i) > 0).OrderByDescending(c.Netto).Cast<int?>().FirstOrDefault();
        int? muroMeno = vicini.Where(i => c.Netto(i) < 0).OrderBy(c.Netto).Cast<int?>().FirstOrDefault();
        return new
        {
            scadenza,
            presente = true,
            istante = c.Ts,
            dte = c.Giorni,
            scade = c.Scadenza,
            netto = R(c.Call.Sum() + c.Put.Sum(), 3),
            call = R(c.Call.Sum(), 3),
            put = R(c.Put.Sum(), 3),
            entroUnoPercento = R(Enumerable.Range(0, n).Where(i => Math.Abs(c.Strike[i] - rif) <= rif * 0.01).Sum(c.Netto), 3),
            muroFreno = muroPiu is { } a ? new { strike = c.Strike[a], valore = R(c.Netto(a), 3) } : null,
            muroAcceleratore = muroMeno is { } b ? new { strike = c.Strike[b], valore = R(c.Netto(b), 3) } : null,
        };
    }

    // ---------------------------------------------------------------- il profilo osservato

    /// <summary>La gamma (o la charm) osservata per prezzo all'istante: dove il modello frena e dove accelera.</summary>
    public object Profilo(string giorno, long? richiesto, string greca, double ampiezza)
    {
        var tempi = Tempi(giorno);
        long ts = richiesto ?? (tempi.Count > 0 ? tempi[^1] : 0);
        var c = Ultima(_m.Piano("observed__" + greca, giorno), ts);
        if (c is null) return new { giorno, ts, greca, presente = false };
        var rif = RiferimentoA(giorno, ts, Ultima(_m.Strike("gex__mm_0dte", giorno), ts)?.Spot ?? double.NaN, Base(giorno).Valore);
        var idx = Enumerable.Range(0, c.Prezzi.Length)
            .Where(i => Math.Abs(c.Prezzi[i] - rif.Prezzo) <= rif.Prezzo * ampiezza / 100).ToList();
        if (idx.Count == 0) idx = Enumerable.Range(0, c.Prezzi.Length).ToList();
        var (zeri, pos, neg) = LivelliAl(giorno, ts);
        return new
        {
            giorno,
            ts,
            greca,
            presente = true,
            istante = c.Ts,
            riferimento = Rif(rif),
            prezzi = idx.Select(i => c.Prezzi[i]).ToList(),
            valori = idx.Select(i => R(c.Valori[i], 2)).ToList(),
            alPrezzo = R(Interpola(c.Prezzi, c.Valori, rif.Prezzo), 2),
            zeri,
            picchiPositivi = pos,
            picchiNegativi = neg,
        };
    }

    // ---------------------------------------------------------------- per strike

    public object PerStrike(string giorno, long? richiesto, string metrica, string gruppo, string scadenza, double ampiezza)
    {
        string famiglia = $"{metrica}__{gruppo}_{scadenza}";
        var tempi = Tempi(giorno);
        long ts = richiesto ?? (tempi.Count > 0 ? tempi[^1] : 0);
        // il DEX e' un flusso al minuto: per strike si somma dall'inizio della giornata
        var c = metrica == "dex" ? DexCumulato(giorno, famiglia, ts).colonna : Ultima(_m.Strike(famiglia, giorno), ts);
        if (c is null) return new { giorno, ts, famiglia, presente = false };
        var rif = RiferimentoA(giorno, ts, c.Spot, Base(giorno).Valore);
        var idx = Enumerable.Range(0, c.Strike.Length)
            .Where(i => Math.Abs(c.Strike[i] - rif.Prezzo) <= rif.Prezzo * ampiezza / 100)
            .Where(i => c.Call[i] != 0 || c.Put[i] != 0)
            .ToList();
        return new
        {
            giorno,
            ts,
            famiglia,
            presente = true,
            istante = c.Ts,
            dte = c.Giorni,
            scade = c.Scadenza,
            riferimento = Rif(rif),
            strike = idx.Select(i => c.Strike[i]).ToList(),
            call = idx.Select(i => R(c.Call[i], 4)).ToList(),
            put = idx.Select(i => R(c.Put[i], 4)).ToList(),
            netto = idx.Select(i => R(c.Netto(i), 4)).ToList(),
            totale = new { call = R(c.Call.Sum(), 3), put = R(c.Put.Sum(), 3), netto = R(c.Call.Sum() + c.Put.Sum(), 3) },
            // 5 novembre 2026: i due muri (il netto piu' positivo e piu' negativo entro il 5%, come l'esposizione di
            // /api/adesso), la somma entro l'1% e, per la GEX, i flip dell'istante
            muri = MuriDi(c, rif.Prezzo),
            entroUnoPercento = R(Enumerable.Range(0, c.Strike.Length).Where(i => Math.Abs(c.Strike[i] - rif.Prezzo) <= rif.Prezzo * 0.01).Sum(c.Netto), 3),
            flip = metrica == "gex" ? LivelliAl(giorno, ts).zeri.Select(z => Math.Round(z, 2)).ToList() : new List<double>(),
        };
    }

    private static object MuriDi(ColStrike c, double rif)
    {
        var (piu, meno) = Muri(c, rif);
        return new
        {
            positivo = piu is { } a ? new { strike = c.Strike[a], valore = R(c.Netto(a), 3) } : null,
            negativo = meno is { } b ? new { strike = c.Strike[b], valore = R(c.Netto(b), 3) } : null,
        };
    }

    // ---------------------------------------------------------------- heat map

    public object HeatMap(string giorno, long? richiesto, string metrica, string ambito, double ampiezza)
    {
        var tempi = Tempi(giorno);
        long ts = richiesto ?? (tempi.Count > 0 ? tempi[^1] : 0);
        var colonne = metrica == "dex"
            ? Gruppi.Select(g => (chiave: g, famiglia: $"dex__{g}_{ambito}")).ToList()
            : Scadenze.Select(s => (chiave: s, famiglia: $"{metrica}__mm_{s}")).ToList();
        var dati = colonne.Select(x => (x.chiave, c: metrica == "dex"
            ? DexCumulato(giorno, x.famiglia, ts).colonna
            : Ultima(_m.Strike(x.famiglia, giorno), ts))).ToList();
        double spotX = dati.Select(d => d.c?.Spot ?? double.NaN).FirstOrDefault(double.IsFinite, double.NaN);
        var rif = RiferimentoA(giorno, ts, spotX, Base(giorno).Valore);

        var strike = dati.Where(d => d.c is not null)
            .SelectMany(d => Enumerable.Range(0, d.c!.Strike.Length)
                .Where(i => d.c.Netto(i) != 0 && Math.Abs(d.c.Strike[i] - rif.Prezzo) <= rif.Prezzo * ampiezza / 100)
                .Select(i => d.c.Strike[i]))
            .Distinct().OrderByDescending(s => s).ToList();

        var valori = strike.Select(s => dati.Select(d =>
        {
            if (d.c is null) return (double?)null;
            int i = Array.IndexOf(d.c.Strike, s);
            return i < 0 ? null : R(d.c.Netto(i), 4);
        }).ToList()).ToList();
        // l'ultima colonna e' la somma della riga
        for (int r = 0; r < valori.Count; r++)
            valori[r].Add(valori[r].Any(v => v is not null) ? R(valori[r].Sum(v => v ?? 0), 4) : null);

        // la netta di ogni colonna su TUTTI gli strike della famiglia, non solo la finestra (5 novembre 2026); l'ultima
        // e' la somma. Per GEX e VEX coincide con il netto di /api/adesso, per il DEX e' il cumulato del gruppo
        var nette = dati.Select(d => d.c is null ? (double?)null : R(d.c.Call.Sum() + d.c.Put.Sum(), 3)).ToList();
        nette.Add(nette.Any(v => v is not null) ? R(nette.Sum(v => v ?? 0), 3) : null);
        var (zeriH, posH, negH) = LivelliAl(giorno, ts);

        return new
        {
            nette,
            livelli = new
            {
                flip = zeriH.Select(z => Math.Round(z, 2)).ToList(),
                frenoPiuForte = posH.Count > 0 ? posH[0].Strike : (double?)null,
                accPiuForte = negH.Count > 0 ? negH[0].Strike : (double?)null,
            },
            giorno,
            ts,
            metrica,
            ambito,
            riferimento = Rif(rif),
            colonne = dati.Select(d => new { chiave = d.chiave, istante = d.c?.Ts, dte = d.c?.Giorni, scade = d.c?.Scadenza })
                .Append(new { chiave = "totale", istante = (long?)null, dte = (int?)null, scade = (long?)null }).ToList(),
            strike,
            valori,
        };
    }

    // ---------------------------------------------------------------- nel tempo

    /// <summary>Il profilo osservato (gamma o charm) istante per istante: tempo x prezzo.</summary>
    public object Intervalli(string giorno, string greca, int passo, double ampiezza)
    {
        var colonne = Secchi(_m.Piano("observed__" + greca, giorno), c => c.Ts, passo);
        var prezzi = new SortedSet<double>(colonne.SelectMany(c => c.Prezzi)).ToList();
        var riferimenti = SerieRiferimento(giorno, colonne.Select(c => c.Ts).ToList());
        var (basso, alto) = Finestra(riferimenti.Select(r => r.Prezzo), ampiezza);
        var tenuti = prezzi.Where(p => p >= basso && p <= alto).ToList();
        if (tenuti.Count == 0) tenuti = prezzi;
        var posizione = tenuti.Select((p, i) => (p, i)).ToDictionary(x => x.p, x => x.i);

        var valori = colonne.Select(c =>
        {
            var riga = new double?[tenuti.Count];
            for (int i = 0; i < c.Prezzi.Length; i++)
                if (posizione.TryGetValue(c.Prezzi[i], out int j)) riga[j] = R(c.Valori[i], 2);
            return riga;
        }).ToList();

        return new
        {
            giorno,
            greca,
            passo,
            tempi = colonne.Select(c => c.Ts).ToList(),
            prezzi = tenuti,
            valori,
            riferimento = riferimenti.Select(r => R(r.Prezzo, 2)).ToList(),
            vivo = riferimenti.Select(r => r.Vivo).ToList(),
            zeri = TracceZeri(giorno, passo),
        };
    }

    /// <summary>La GEX (o VEX) netta dei market maker per strike, istante per istante.</summary>
    public object IntervalliStrike(string giorno, string metrica, string scadenza, int passo, double ampiezza)
    {
        var colonne = Secchi(_m.Strike($"{metrica}__mm_{scadenza}", giorno), c => c.Ts, passo);
        var riferimenti = SerieRiferimento(giorno, colonne.Select(c => c.Ts).ToList());
        var (basso, alto) = Finestra(riferimenti.Select(r => r.Prezzo), ampiezza);
        var strike = colonne.SelectMany(c => Enumerable.Range(0, c.Strike.Length)
                .Where(i => c.Netto(i) != 0 && c.Strike[i] >= basso && c.Strike[i] <= alto).Select(i => c.Strike[i]))
            .Distinct().OrderBy(s => s).ToList();
        var posizione = strike.Select((s, i) => (s, i)).ToDictionary(x => x.s, x => x.i);
        var valori = colonne.Select(c =>
        {
            var riga = new double?[strike.Count];
            for (int i = 0; i < c.Strike.Length; i++)
                if (posizione.TryGetValue(c.Strike[i], out int j)) riga[j] = R(c.Netto(i), 4);
            return riga;
        }).ToList();
        return new
        {
            giorno,
            metrica,
            scadenza,
            passo,
            tempi = colonne.Select(c => c.Ts).ToList(),
            strike,
            valori,
            riferimento = riferimenti.Select(r => R(r.Prezzo, 2)).ToList(),
            vivo = riferimenti.Select(r => r.Vivo).ToList(),
        };
    }

    // ---------------------------------------------------------------- la deriva

    /// <summary>Minuto per minuto: prezzo, GEX netta per scadenza, delta per gruppo, pressione 0DTE.</summary>
    public object Deriva(string giorno, int passo)
    {
        var tempi = Secchi(Tempi(giorno), t => t, passo);
        var riferimenti = SerieRiferimento(giorno, tempi);

        List<double?> Serie(IReadOnlyList<ColStrike> cs) => Allinea(cs, tempi, c => c.Ts, c => c.Call.Sum() + c.Put.Sum(), 3);
        List<double?> Cumulata(IReadOnlyList<ColStrike> cs) => CumulataAl(cs, tempi);

        var pr = _m.Pressione(giorno);
        var gex = Scadenze.ToDictionary(s => s, s => Serie(_m.Strike("gex__mm_" + s, giorno)));
        var colZero = Gruppi.ToDictionary(g => g, g => _m.Strike($"dex__{g}_0dte", giorno));
        var colTutte = Gruppi.ToDictionary(g => g, g => _m.Strike($"dex__{g}_all", giorno));
        var dexZeroDte = Gruppi.ToDictionary(g => g, g => Cumulata(colZero[g]));
        var dexTutte = Gruppi.ToDictionary(g => g, g => Cumulata(colTutte[g]));
        var pressione = Allinea(pr, tempi, c => c.Ts, c => c.Delta, 2);
        var gammaAlPrezzo = GammaAlPrezzo(giorno, tempi, riferimenti);

        var anomali = new List<object>();
        foreach (var g in Gruppi)
        {
            anomali.AddRange(Anomali(dexZeroDte[g], colZero[g], tempi, $"dexZeroDte.{g}"));
            anomali.AddRange(Anomali(dexTutte[g], colTutte[g], tempi, $"dexTutte.{g}"));
        }

        return new
        {
            giorno,
            passo,
            tempi,
            riferimento = riferimenti.Select(r => R(r.Prezzo, 2)).ToList(),
            vivo = riferimenti.Select(r => r.Vivo).ToList(),
            gex,
            dexZeroDte,
            dexTutte,
            pressione,
            gammaAlPrezzo,
            adesso = DerivaAdesso(tempi, gex, dexZeroDte["nonprocust"], pressione, gammaAlPrezzo),
            anomali,
            unita = new
            {
                gex = "unità di OptionsDepth",
                dex = "delta (unità di OptionsDepth)",
                pressione = "unità di OptionsDepth, verso non dichiarato",
            },
        };
    }

    /// <summary>L'ultimo istante della Deriva in numeri: le tessere della pagina (5 novembre 2026).</summary>
    private static object? DerivaAdesso(List<long> tempi, Dictionary<string, List<double?>> gex, List<double?> retail,
        List<double?> pressione, List<double?> gamma)
    {
        if (tempi.Count == 0) return null;
        int n = tempi.Count - 1;
        long ts = tempi[n];
        double? Ultimo(List<double?> s) { for (int i = n; i >= 0; i--) if (s[i] is { } v) return v; return null; }
        double? Al(List<double?> s, long quando)
        {
            int i = Ponte.UltimoIndice(tempi, quando);
            for (; i >= 0; i--) if (s[i] is { } v) return v;
            return null;
        }
        double? g = Ultimo(gamma);
        long? da = null;
        if (g is { } g0 && g0 != 0)
            for (int i = n; i >= 0; i--)
            {
                if (gamma[i] is not { } v) continue;
                if (Math.Sign(v) != Math.Sign(g0)) break;
                da = tempi[i];
            }
        var nette = Scadenze.ToDictionary(s => s, s => Ultimo(gex[s]));
        var conTotale = new Dictionary<string, double?>(nette)
        {
            ["totale"] = nette.Values.Any(v => v is not null) ? R(nette.Values.Sum(v => v ?? 0), 3) : null,
        };
        return new
        {
            ts,
            gammaAlPrezzo = g,
            regime = Regime(g),
            regimeDa = da,
            gex = conTotale,
            retail0dte = Ultimo(retail),
            retail0dteUnOraFa = Al(retail, ts - 60 * 60_000),
            pressione = Ultimo(pressione),
            pressioneTrentaMinutiFa = Al(pressione, ts - 30 * 60_000),
        };
    }

    /// <summary>
    /// I minuti anomali di una serie cumulata del DEX (5 novembre 2026). I valori non si toccano: il server li marca, la
    /// pagina interrompe la linea e calcola la scala senza quei punti.
    ///
    /// La regola: Δi = si − si−1; m = la mediana dei |Δ| non nulli del giorno; un punto è anomalo se
    /// |Δi| > max(20·m, 0,25·(max s − min s)). Il primo punto è anomalo ("partenza") se |s0| > 20·m.
    /// Il motivo: "scadenza" se il minuto cade fra le 16:00 e le 16:02 di New York (lo 0DTE scade e esce dalla
    /// somma), "finestra" se una colonna del passo non copre i 60 secondi prima del suo istante (from ≠ ts − 60 s),
    /// "salto" altrimenti. Sul 4 novembre 2026 (dex__firm_all): 04:01 partenza (+14.100, la prima colonna del giorno),
    /// 09:59 e 10:12 salto (−14.685 e +15.430 con finestre di 60 s: un salto e il suo ritorno), 16:01 scadenza
    /// (−21.693); 09:31 ha una finestra di 7 minuti (−1.544).
    /// </summary>
    public static List<object> Anomali(List<double?> s, IReadOnlyList<ColStrike> colonne, List<long> tempi, string nome)
    {
        var out_ = new List<object>();
        var idx = Enumerable.Range(0, s.Count).Where(i => s[i] is not null).ToList();
        if (idx.Count < 2) return out_;
        var delta = new List<(int i, double d)>();
        for (int k = 1; k < idx.Count; k++) delta.Add((idx[k], s[idx[k]]!.Value - s[idx[k - 1]]!.Value));
        var assoluti = delta.Select(x => Math.Abs(x.d)).Where(x => x > 0).ToList();
        if (assoluti.Count == 0) return out_;
        double m = Ponte.Mediana(assoluti);
        double ampiezza = idx.Max(i => s[i]!.Value) - idx.Min(i => s[i]!.Value);
        double soglia = Math.Max(20 * m, 0.25 * ampiezza);
        var chiavi = colonne.Select(c => c.Ts).ToList();

        string Motivo(int i)
        {
            // il passo copre (tempi[i-1], tempi[i]]: e' "scadenza" se dentro c'e' un minuto fra le 16:00 e le 16:02 di New York
            long prima = i > 0 ? tempi[i - 1] : long.MinValue;
            string g = Cassa.OraNY(tempi[i]).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            long sedici = Cassa.Di(g).chiusura;
            if (tempi[i] >= sedici && prima < sedici + 2 * 60_000) return "scadenza";
            for (int j = Ponte.UltimoIndice(chiavi, tempi[i]); j >= 0 && chiavi[j] > prima; j--)
                if (colonne[j].From > 0 && colonne[j].From != colonne[j].Ts - 60_000) return "finestra";
            return "salto";
        }

        int i0 = idx[0];
        if (Math.Abs(s[i0]!.Value) > 20 * m)
            out_.Add(new { serie = nome, indice = i0, ts = tempi[i0], salto = R(s[i0]!.Value, 2), motivo = "partenza" });
        foreach (var (i, d) in delta)
            if (Math.Abs(d) > soglia)
                out_.Add(new { serie = nome, indice = i, ts = tempi[i], salto = R(d, 2), motivo = Motivo(i) });
        return out_;
    }

    private List<double?> GammaAlPrezzo(string giorno, List<long> tempi, List<Riferimento> riferimenti)
    {
        var piano = _m.Piano("observed__gamma", giorno);
        var out_ = new List<double?>(tempi.Count);
        for (int i = 0; i < tempi.Count; i++)
        {
            var c = Ultima(piano, tempi[i]);
            out_.Add(c is null || tempi[i] - c.Ts > 10 * 60_000 ? null : R(Interpola(c.Prezzi, c.Valori, riferimenti[i].Prezzo), 2));
        }
        return out_;
    }

    // ---------------------------------------------------------------- previsione

    /// <summary>I prezzi dove i valori di una colonna cambiano segno, interpolati fra i due punti della griglia.</summary>
    public static List<double> ZeriDi(double[] prezzi, float[] valori)
    {
        var out_ = new List<double>();
        int n = Math.Min(prezzi.Length, valori.Length);
        for (int i = 1; i < n; i++)
        {
            double a = valori[i - 1], b = valori[i];
            if (!double.IsFinite(a) || !double.IsFinite(b) || a == 0 && b == 0) continue;
            if (a == 0) { out_.Add(Math.Round(prezzi[i - 1], 2)); continue; }
            if ((a < 0) != (b < 0) && b != 0)
                out_.Add(Math.Round(prezzi[i - 1] + (prezzi[i] - prezzi[i - 1]) * (0 - a) / (b - a), 2));
        }
        return out_;
    }

    /// <summary>
    /// Gli zeri di ogni tempo uniti in tracce: uno zero continua la traccia il cui ultimo punto (al tempo prima) e' il
    /// piu' vicino entro <paramref name="tolleranza"/> punti; se no comincia una traccia nuova. Una traccia non salta
    /// tempi: se a un tempo non continua, si chiude.
    /// </summary>
    public static List<List<double[]>> Tracce(List<long> tempi, List<List<double>> zeri, double tolleranza)
    {
        var chiuse = new List<List<double[]>>();
        var aperte = new List<List<double[]>>();
        for (int t = 0; t < tempi.Count; t++)
        {
            var nuove = new List<List<double[]>>();
            var libere = new List<List<double[]>>(aperte);
            foreach (var z in zeri[t])
            {
                var vicina = libere.Where(tr => Math.Abs(tr[^1][1] - z) <= tolleranza).MinBy(tr => Math.Abs(tr[^1][1] - z));
                if (vicina is not null)
                {
                    libere.Remove(vicina);
                    vicina.Add(new double[] { tempi[t], z });
                    nuove.Add(vicina);
                }
                else nuove.Add(new List<double[]> { new double[] { tempi[t], z } });
            }
            chiuse.AddRange(libere);
            aperte = nuove;
        }
        chiuse.AddRange(aperte);
        return chiuse.OrderBy(tr => tr[0][0]).ToList();
    }

    public object Previsione(string giorno, string tipo, string greca, long? asOf)
    {
        var tutte = _m.Piano($"{tipo}__{greca}", giorno);
        var asOfs = tutte.Select(c => c.AsOf).Where(a => a > 0).Distinct().OrderBy(a => a).ToList();
        long scelto = asOf is { } a0 && asOfs.Count > 0
            ? asOfs[Math.Max(0, Ponte.UltimoIndice(asOfs, a0))]
            : asOfs.Count > 0 ? asOfs[^1] : 0;
        var colonne = tutte.Where(c => c.AsOf == scelto).OrderBy(c => c.Ts).ToList();
        var prezzi = new SortedSet<double>(colonne.SelectMany(c => c.Prezzi)).ToList();
        var posizione = prezzi.Select((p, i) => (p, i)).ToDictionary(x => x.p, x => x.i);
        var valori = colonne.Select(c =>
        {
            var riga = new double?[prezzi.Count];
            for (int i = 0; i < c.Prezzi.Length; i++)
                if (posizione.TryGetValue(c.Prezzi[i], out int j)) riga[j] = R(c.Valori[i], 2);
            return riga;
        }).ToList();
        var rif = scelto > 0
            ? RiferimentoA(giorno, scelto, Ultima(_m.Strike("gex__mm_0dte", giorno), scelto)?.Spot ?? double.NaN, Base(giorno).Valore)
            : null;

        // 5 novembre 2026: la traccia del flip previsto, il valore al prezzo del calcolo, la seduta prevista, il taglio
        // della scadenza 0DTE e la sintesi per le tessere
        var zeriPerTempo = colonne.Select(c => ZeriDi(c.Prezzi, c.Valori)).ToList();
        var alPrezzo = colonne.Select(c => rif is null ? null : R(Interpola(c.Prezzi, c.Valori, rif.Prezzo), 2)).ToList();
        string? seduta = tipo == "nextday" && colonne.Count > 0 ? SedutaDi(colonne[0].Ts, colonne[^1].Ts) : null;
        long? taglio = null;
        if (tipo == "forecast" && scelto > 0)
        {
            string g = Cassa.OraNY(scelto).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            taglio = Cassa.Di(g).chiusura - 15 * 60_000;
        }
        object? sintesi = null;
        if (colonne.Count > 0)
        {
            double? ultimo = alPrezzo.LastOrDefault(v => v is not null);
            double? primo = alPrezzo.FirstOrDefault(v => v is not null);
            long? cambia = null;
            if (primo is { } p0)
                for (int i = 0; i < alPrezzo.Count; i++)
                    if (alPrezzo[i] is { } v && Math.Sign(v) != Math.Sign(p0)) { cambia = colonne[i].Ts; break; }
            var zf = zeriPerTempo[^1];
            sintesi = new
            {
                flipFinale = zf.Count > 0 && rif is not null ? Math.Round(zf.MinBy(z => Math.Abs(z - rif.Prezzo)), 2) : zf.Count > 0 ? zf[0] : (double?)null,
                regimeFinale = Regime(ultimo),
                cambiaAlle = cambia,
            };
        }

        return new
        {
            zeri = Tracce(colonne.Select(c => c.Ts).ToList(), zeriPerTempo, colonne.Count > 0 && colonne[0].Prezzi.Length > 1
                ? 3 * Math.Abs(colonne[0].Prezzi[1] - colonne[0].Prezzi[0]) : 10),
            seduta,
            taglioScadenza = taglio,
            alPrezzo,
            sintesi,
            giorno,
            tipo,
            greca,
            asOfs,
            asOf = scelto,
            tempi = colonne.Select(c => c.Ts).ToList(),
            prezzi,
            valori,
            riferimento = rif is null ? null : Rif(rif),
        };
    }

    // ---------------------------------------------------------------- livelli

    public object Livelli(string giorno, int passo)
    {
        var tempi = Secchi(Tempi(giorno), t => t, passo);
        var riferimenti = SerieRiferimento(giorno, tempi);
        var col = _m.Livelli(giorno).Where(c => c.Superficie == "observed").OrderBy(c => c.Ts).ThenBy(c => c.Fino).LastOrDefault();
        var picchi = new List<object>();
        if (col is not null)
        {
            foreach (var t in col.Tracce.Where(t => t.Tipo is 1 or 2))
            {
                var punti = new List<object>();
                for (int k = 0; k < t.Riga.Length; k++)
                {
                    long ts = col.Ts + (t.Primo + k) * col.PassoMs;
                    if (passo > 1 && (ts / 60_000) % passo != 0 && k != t.Riga.Length - 1) continue;
                    if (t.Riga[k] is not { } r || r < 0 || r >= col.Prezzi.Length) continue;
                    punti.Add(new object?[] { ts, col.Prezzi[r], R(t.Peso.ElementAtOrDefault(k) ?? double.NaN, 1) });
                }
                if (punti.Count > 0) picchi.Add(new { tipo = t.Tipo == 1 ? "positivo" : "negativo", punti });
            }
        }
        return new
        {
            giorno,
            passo,
            tempi,
            riferimento = riferimenti.Select(r => R(r.Prezzo, 2)).ToList(),
            vivo = riferimenti.Select(r => r.Vivo).ToList(),
            zeri = TracceZeri(giorno, passo),
            picchi,
            adesso = tempi.Count > 0 && LivelliAl(giorno, tempi[^1]) is var (z, pp, nn)
                ? new { zeri = z, positivi = pp, negativi = nn }
                : null,
            outlook = Outlook(giorno),
        };
    }

    /// <summary>
    /// Le tracce degli zeri (il flip) come linee nel tempo: [[ts, prezzo], ...]. Ogni linea e' una traccia sola del
    /// file dei livelli (lo stesso track) e si interrompe (una linea nuova) quando la traccia manca per piu' di due
    /// passi: mai si uniscono punti di tracce diverse o lontani nel tempo (5 novembre 2026, lo zig-zag del 4/11).
    /// </summary>
    private List<List<double[]>> TracceZeri(string giorno, int passo)
    {
        var col = _m.Livelli(giorno).Where(c => c.Superficie == "observed").OrderBy(c => c.Ts).ThenBy(c => c.Fino).LastOrDefault();
        var out_ = new List<List<double[]>>();
        if (col is null) return out_;
        long buco = 3L * Math.Max(1, passo) * 60_000;   // due passi mancanti = un salto di tre
        foreach (var t in col.Tracce.Where(t => t.Tipo == 0))
        {
            var linea = new List<double[]>();
            long ultimo = long.MinValue;
            for (int k = 0; k < t.Prezzo.Length; k++)
            {
                long ts = col.Ts + (t.Primo + k) * col.PassoMs;
                if (passo > 1 && (ts / 60_000) % passo != 0 && k != t.Prezzo.Length - 1) continue;
                if (t.Prezzo[k] is not { } p) continue;
                if (linea.Count > 0 && ts - ultimo > buco)
                {
                    out_.Add(linea);
                    linea = new List<double[]>();
                }
                linea.Add(new[] { ts, Math.Round(p, 2) });
                ultimo = ts;
            }
            if (linea.Count > 0) out_.Add(linea);
        }
        return out_;
    }

    public sealed record Picco(double Strike, double? Peso, double? Sotto, double? Sopra);

    /// <summary>Zeri e picchi della superficie osservata all'istante ts.</summary>
    public (List<double> zeri, List<Picco> positivi, List<Picco> negativi) LivelliAl(string giorno, long ts)
    {
        var col = _m.Livelli(giorno).Where(c => c.Superficie == "observed" && c.Ts <= ts)
            .OrderBy(c => c.Ts).ThenBy(c => c.Fino).LastOrDefault();
        var zeri = new List<double>();
        var pos = new List<Picco>();
        var neg = new List<Picco>();
        if (col is null) return (zeri, pos, neg);
        long minuto = (ts - col.Ts) / col.PassoMs;
        foreach (var t in col.Tracce)
        {
            int k = (int)(minuto - t.Primo);
            int n = t.Lunghezza;
            if (k < 0) continue;
            // la traccia che finisce da poco vale ancora (il lettore e' qualche minuto indietro)
            if (k >= n) { if (k - (n - 1) > 5) continue; k = n - 1; }
            if (t.Tipo == 0)
            {
                if (t.Prezzo[k] is { } p) zeri.Add(Math.Round(p, 2));
                continue;
            }
            if (t.Riga[k] is not { } r || r < 0 || r >= col.Prezzi.Length) continue;
            var picco = new Picco(col.Prezzi[r], R(t.Peso.ElementAtOrDefault(k) ?? double.NaN, 1),
                Prezzo(col.Prezzi, t.Sotto.ElementAtOrDefault(k)), Prezzo(col.Prezzi, t.Sopra.ElementAtOrDefault(k)));
            (t.Tipo == 1 ? pos : neg).Add(picco);
        }
        zeri.Sort();
        return (zeri, pos.OrderByDescending(p => p.Peso).ToList(), neg.OrderBy(p => p.Peso).ToList());
    }

    /// <summary>La previsione dei livelli per la prossima seduta; <c>Seduta</c> e' la data di New York della seduta
    /// che la finestra [da, fino] copre (5 novembre 2026).</summary>
    public sealed record OutlookLivelli(long Istante, long Da, long Fino, List<double> Zeri, List<Picco> Positivi, List<Picco> Negativi,
        string? Seduta);

    /// <summary>La previsione dei livelli per la prossima seduta (superficie outlook, l'ultima).</summary>
    public OutlookLivelli? Outlook(string giorno)
    {
        var col = _m.Livelli(giorno).Where(c => c.Superficie == "outlook").OrderBy(c => c.Ts).LastOrDefault();
        if (col is null) return null;
        var zeri = new List<double>();
        var pos = new List<Picco>();
        var neg = new List<Picco>();
        foreach (var t in col.Tracce)
        {
            if (t.Tipo == 0)
            {
                if (t.Prezzo.LastOrDefault(p => p is not null) is { } p) zeri.Add(Math.Round(p, 2));
                continue;
            }
            int k = t.Riga.Length - 1;
            if (k < 0 || t.Riga[k] is not { } r || r < 0 || r >= col.Prezzi.Length) continue;
            (t.Tipo == 1 ? pos : neg).Add(new Picco(col.Prezzi[r], R(t.Peso.ElementAtOrDefault(k) ?? double.NaN, 1),
                Prezzo(col.Prezzi, t.Sotto.ElementAtOrDefault(k)), Prezzo(col.Prezzi, t.Sopra.ElementAtOrDefault(k))));
        }
        zeri.Sort();
        return new OutlookLivelli(col.Ts, col.Ts, col.Fino, zeri, pos.OrderByDescending(p => p.Peso).ToList(), neg.OrderBy(p => p.Peso).ToList(),
            SedutaDi(col.Ts, col.Fino));
    }

    // ---------------------------------------------------------------- il DEX e' un flusso

    /// <summary>
    /// Il DEX per gruppo di OptionsDepth non e' una posizione: ogni colonna copre una finestra
    /// [from, ts] di 60 secondi ed e' il delta che il gruppo ha scambiato in quel minuto
    /// (verificato il 2 novembre 2026: 712 colonne su 713 a 60 s, valori che oscillano attorno
    /// allo zero; GEX e VEX invece hanno from = 0 e sono curve lisce). Il numero che si legge e'
    /// quindi la somma dall'inizio della giornata: il delta accumulato dal gruppo, strike per strike.
    /// </summary>
    public (ColStrike? colonna, double? totale, double? trentaMinuti) DexCumulato(string giorno, string famiglia, long ts)
    {
        var cs = _m.Strike(famiglia, giorno);
        var somma = new SortedDictionary<double, (double call, double put)>();
        double totale = 0, prima30 = 0;
        long soglia30 = ts - 30 * 60_000;
        ColStrike? ultima = null;
        foreach (var c in cs)
        {
            if (c.Ts > ts) break;
            ultima = c;
            for (int i = 0; i < c.Strike.Length; i++)
            {
                somma.TryGetValue(c.Strike[i], out var v);
                somma[c.Strike[i]] = (v.call + c.Call[i], v.put + c.Put[i]);
                totale += c.Call[i] + c.Put[i];
                if (c.Ts <= soglia30) prima30 += c.Call[i] + c.Put[i];
            }
        }
        if (ultima is null) return (null, null, null);
        var col = new ColStrike(ultima.Ts, ultima.Spot, ultima.Scadenza, ultima.Giorni,
            somma.Keys.ToArray(), somma.Values.Select(v => v.call).ToArray(), somma.Values.Select(v => v.put).ToArray());
        return (col, R(totale, 2), R(totale - prima30, 2));
    }

    private static List<double?> CumulataAl(IReadOnlyList<ColStrike> cs, List<long> tempi)
    {
        var out_ = new List<double?>(tempi.Count);
        double somma = 0;
        int j = 0;
        foreach (var t in tempi)
        {
            while (j < cs.Count && cs[j].Ts <= t) { somma += cs[j].Call.Sum() + cs[j].Put.Sum(); j++; }
            out_.Add(j == 0 ? null : R(somma, 2));
        }
        return out_;
    }

    // ---------------------------------------------------------------- utilita'

    private List<Riferimento> SerieRiferimento(string giorno, List<long> tempi)
    {
        var spot = _m.Strike("gex__mm_0dte", giorno);
        double? b = Base(giorno).Valore;
        return tempi.Select(t => RiferimentoA(giorno, t, Ultima(spot, t)?.Spot ?? double.NaN, b)).ToList();
    }

    private static (double basso, double alto) Finestra(IEnumerable<double> prezzi, double ampiezza)
    {
        var p = prezzi.Where(x => double.IsFinite(x) && x > 0).ToList();
        if (p.Count == 0) return (double.MinValue, double.MaxValue);
        return (p.Min() * (1 - ampiezza / 100), p.Max() * (1 + ampiezza / 100));
    }

    /// <summary>Una colonna per secchio di `passo` minuti: l'ultima del secchio.</summary>
    public static List<T> Secchi<T>(IReadOnlyList<T> colonne, Func<T, long> ts, int passo)
    {
        if (passo <= 1) return colonne.ToList();
        long larghezza = passo * 60_000L;
        return colonne.GroupBy(c => ts(c) / larghezza).OrderBy(g => g.Key).Select(g => g.Last()).ToList();
    }

    private static List<double?> Allinea<T>(IReadOnlyList<T> cs, List<long> tempi, Func<T, long> ts, Func<T, double> valore, int cifre)
    {
        var chiavi = cs.Select(ts).ToList();
        return tempi.Select(t =>
        {
            int i = Ponte.UltimoIndice(chiavi, t);
            return i < 0 || t - chiavi[i] > 10 * 60_000 ? null : R(valore(cs[i]), cifre);
        }).ToList();
    }

    /// <summary>L'ultima colonna all'istante ts o prima; se non ce n'e', la prima della giornata.</summary>
    public static T? Ultima<T>(IReadOnlyList<T> cs, long ts) where T : class
    {
        if (cs.Count == 0) return null;
        var chiavi = cs.Select(c => c switch
        {
            ColStrike s => s.Ts,
            ColPiano p => p.Ts,
            ColPressione q => q.Ts,
            ColLivelli l => l.Ts,
            _ => 0L,
        }).ToList();
        int i = Ponte.UltimoIndice(chiavi, ts);
        return i < 0 ? null : cs[i];
    }

    public static double Interpola(double[] prezzi, float[] valori, double p)
    {
        int n = Math.Min(prezzi.Length, valori.Length);
        if (n == 0 || !double.IsFinite(p)) return double.NaN;
        if (p <= prezzi[0]) return p < prezzi[0] - (prezzi.Length > 1 ? prezzi[1] - prezzi[0] : 0) ? double.NaN : valori[0];
        if (p >= prezzi[n - 1]) return p > prezzi[n - 1] + (n > 1 ? prezzi[n - 1] - prezzi[n - 2] : 0) ? double.NaN : valori[n - 1];
        for (int i = 1; i < n; i++)
        {
            if (prezzi[i] < p) continue;
            double t = (p - prezzi[i - 1]) / (prezzi[i] - prezzi[i - 1]);
            return valori[i - 1] + t * (valori[i] - valori[i - 1]);
        }
        return double.NaN;
    }

    public static string? Regime(double? gamma) => gamma switch
    {
        null => null,
        > 0 => "freno",
        < 0 => "acceleratore",
        _ => "neutro",
    };

    private static object Rif(Riferimento r) => new
    {
        prezzo = R(r.Prezzo, 2),
        fonte = r.Fonte,
        vivo = r.Vivo,
        spotX = R(r.SpotX, 2),
        es = r.Es is { } e ? R(e, 2) : null,
    };

    private static double? Prezzo(double[] prezzi, int? riga) =>
        riga is { } r && r >= 0 && r < prezzi.Length ? prezzi[r] : null;

    public static double? R(double v, int cifre) => double.IsFinite(v) ? Math.Round(v, cifre) : null;
}
