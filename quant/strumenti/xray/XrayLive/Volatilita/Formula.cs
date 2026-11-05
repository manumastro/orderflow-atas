namespace XrayLive.Volatilita;

/// <summary>Una quotazione di un'opzione: strike, tipo, i prezzi che la fonte da' (quelli che mancano sono null).</summary>
internal sealed record Quotazione(double Strike, bool Call, double? Bid, double? Ask, double? Settlement,
    double OpenInterest = 0, double? Iv = null)
{
    /// <summary>Il medio fra bid e ask, se il book e' valido (bid > 0, ask >= bid).</summary>
    public double? Medio => Bid is { } b && Ask is { } a && b > 0 && a >= b ? (a + b) / 2 : null;
}

/// <summary>Una scadenza: il giorno, l'istante in cui scade (UTC) e le sue quotazioni.</summary>
internal sealed record Scadenza(string Giorno, DateTime ScadeUtc, IReadOnlyList<Quotazione> Opzioni, double? Sottostante = null);

/// <summary>Il risultato della formula del Cboe su una scadenza.</summary>
internal sealed record Varianza(string Scadenza, double Anni, double Forward, double K0, double Valore,
    int Strike, double StrikeMin, double StrikeMax)
{
    public double Vol => Math.Sqrt(Math.Max(Valore, 0)) * 100;
}

/// <summary>
/// La matematica della volatilita', senza I/O (2 novembre 2026).
///
/// LA FORMULA DEL VIX (Cboe, "VIX White Paper"): per ogni scadenza
///   sigma^2 = 2/T * sum_i (dK_i / K_i^2) * e^(RT) * Q(K_i)  -  1/T * (F/K0 - 1)^2
/// con Q il prezzo delle opzioni fuori dal denaro (put sotto K0, call sopra, la media delle due a K0),
/// F il forward dalla parita' put-call allo strike dove |C - P| e' minimo, K0 il primo strike <= F.
/// Le due scadenze attorno ai 30 giorni si interpolano a 30 giorni esatti. Vale per qualunque catena:
/// sulle opzioni ES e' il VIX, sulle NQ il VXN; su QQQ e NDX la stessa misura sul Nasdaq-100.
/// </summary>
internal static class Formula
{
    /// <summary>Il tasso annuo senza rischio usato nel fattore e^(RT): il fed funds effettivo del 29/10/2026.
    /// Su 30 giorni pesa meno di 0,05 punti di volatilita'.</summary>
    public const double Tasso = 0.0388;

    public const double AnniPerGiorno = 1.0 / 365.0;

    /// <summary>Il prezzo sotto il quale un'opzione conta come "senza offerta" (il tick minimo di ES e NQ).</summary>
    public const double PrezzoMinimo = 0.05;

    /// <summary>Quale prezzo usare di una quotazione: il medio (dal vivo) o il settlement (la chiusura).</summary>
    public enum Prezzi { Medio, Settlement }

    public static double? Prezzo(Quotazione q, Prezzi p) => p == Prezzi.Medio ? q.Medio : q.Settlement is > 0 ? q.Settlement : null;

    /// <summary>La varianza a una scadenza con la formula del Cboe; null se la catena non basta.</summary>
    public static Varianza? VarianzaCboe(Scadenza s, DateTime adesso, Prezzi p, double tasso = Tasso)
    {
        double t = (s.ScadeUtc - adesso).TotalDays * AnniPerGiorno;
        if (t <= 0) return null;
        var perStrike = new SortedDictionary<double, (double? c, double? p)>();
        foreach (var q in s.Opzioni)
        {
            var x = perStrike.GetValueOrDefault(q.Strike);
            double? v = Prezzo(q, p);
            perStrike[q.Strike] = q.Call ? (v, x.p) : (x.c, v);
        }
        // F: lo strike con entrambi i prezzi dove la call e la put si somigliano di piu'
        var entrambi = perStrike.Where(kv => kv.Value.c is > 0 && kv.Value.p is > 0).ToList();
        if (entrambi.Count < 3) return null;
        var kf = entrambi.MinBy(kv => Math.Abs(kv.Value.c!.Value - kv.Value.p!.Value));
        double crescita = Math.Exp(tasso * t);
        double f = kf.Key + crescita * (kf.Value.c!.Value - kf.Value.p!.Value);
        var sotto = entrambi.Where(kv => kv.Key <= f).ToList();
        if (sotto.Count == 0) return null;
        double k0 = sotto[^1].Key;

        // le opzioni fuori dal denaro, dal centro verso le ali; ci si ferma dopo due prezzi nulli
        // consecutivi. Uno strike senza nessun prezzo (non quotato, o non ancora arrivato) si salta.
        var scelte = new List<(double k, double q)>();
        Raccogli(perStrike.Where(kv => kv.Key < k0).Reverse().Select(kv => (kv.Key, kv.Value.p)), scelte);
        Raccogli(perStrike.Where(kv => kv.Key > k0).Select(kv => (kv.Key, kv.Value.c)), scelte);
        var a0 = perStrike[k0];
        scelte.Add((k0, (a0.c!.Value + a0.p!.Value) / 2));
        scelte.Sort((x, y) => x.k.CompareTo(y.k));
        if (scelte.Count < 5) return null;

        double somma = 0;
        for (int i = 0; i < scelte.Count; i++)
        {
            double dk = i == 0 ? scelte[1].k - scelte[0].k
                : i == scelte.Count - 1 ? scelte[i].k - scelte[i - 1].k
                : (scelte[i + 1].k - scelte[i - 1].k) / 2;
            somma += dk / (scelte[i].k * scelte[i].k) * crescita * scelte[i].q;
        }
        double var = 2 / t * somma - Math.Pow(f / k0 - 1, 2) / t;
        return new Varianza(s.Giorno, t, f, k0, var, scelte.Count, scelte[0].k, scelte[^1].k);
    }

    private static void Raccogli(IEnumerable<(double k, double? q)> lato, List<(double k, double q)> scelte)
    {
        int nulli = 0;
        foreach (var (k, q) in lato)
        {
            if (q is null) continue;
            if (q <= PrezzoMinimo)
            {
                if (++nulli >= 2) break;
                continue;
            }
            nulli = 0;
            scelte.Add((k, q.Value));
        }
    }

    /// <summary>L'interpolazione del Cboe a 30 giorni fra due varianze (in punti percentuali).</summary>
    public static double? A30Giorni(Varianza vicina, Varianza lontana, double giorni = 30)
    {
        double n = giorni * AnniPerGiorno, t1 = vicina.Anni, t2 = lontana.Anni;
        if (t2 <= t1) return null;
        double w = (t1 * vicina.Valore * (t2 - n) / (t2 - t1) + t2 * lontana.Valore * (n - t1) / (t2 - t1)) / n;
        return w > 0 ? Math.Sqrt(w) * 100 : null;
    }

    /// <summary>Le due scadenze attorno ai giorni voluti: l'ultima che scade prima e la prima che scade dopo.</summary>
    public static (T vicina, T lontana)? Attorno<T>(IEnumerable<T> scadenze, Func<T, DateTime> scade, DateTime adesso,
        double giorni = 30, double minimo = 7)
    {
        var bersaglio = adesso.AddDays(giorni);
        var ordinate = scadenze.Where(s => (scade(s) - adesso).TotalDays >= minimo).OrderBy(scade).ToList();
        int i = ordinate.FindLastIndex(s => scade(s) <= bersaglio);
        int j = ordinate.FindIndex(s => scade(s) > bersaglio);
        return i < 0 || j < 0 ? null : (ordinate[i], ordinate[j]);
    }

    // ---------------------------------------------------------------- Black-Scholes e Black-76

    public static double Phi(double x) => Math.Exp(-x * x / 2) / Math.Sqrt(2 * Math.PI);

    /// <summary>La normale cumulata (Abramowitz-Stegun 7.1.26 su erf, errore sotto 1,5e-7).</summary>
    public static double N(double x)
    {
        double z = Math.Abs(x) / Math.Sqrt(2);
        double t = 1 / (1 + 0.3275911 * z);
        double erf = 1 - (((((1.061405429 * t - 1.453152027) * t) + 1.421413741) * t - 0.284496736) * t + 0.254829592) * t * Math.Exp(-z * z);
        return x >= 0 ? 0.5 * (1 + erf) : 0.5 * (1 - erf);
    }

    /// <summary>Il prezzo non scontato di un'opzione sul forward F (Black-76 con tasso zero).</summary>
    public static double Black(double f, double k, double t, double s, bool call)
    {
        if (t <= 0 || s <= 0) return Math.Max(call ? f - k : k - f, 0);
        double v = s * Math.Sqrt(t);
        double d1 = (Math.Log(f / k) + v * v / 2) / v, d2 = d1 - v;
        return call ? f * N(d1) - k * N(d2) : k * N(-d2) - f * N(-d1);
    }

    /// <summary>La volatilita' implicita (Black-76, bisezione) dal prezzo non scontato; null se fuori dai limiti.</summary>
    public static double? Implicita(double prezzo, double f, double k, double t, bool call)
    {
        double intrinseco = Math.Max(call ? f - k : k - f, 0);
        if (t <= 0 || prezzo <= intrinseco + 1e-9 || prezzo >= (call ? f : k)) return null;
        double lo = 1e-4, hi = 5;
        for (int i = 0; i < 80; i++)
        {
            double m = (lo + hi) / 2;
            if (Black(f, k, t, m, call) > prezzo) hi = m; else lo = m;
        }
        double s = (lo + hi) / 2;
        return s is > 0.002 and < 4.9 ? s : null;
    }

    /// <summary>La gamma per unita' di sottostante (Black-76 sul forward; con S = F e' la stessa di Black-Scholes senza dividendi).</summary>
    public static double Gamma(double f, double k, double t, double s)
    {
        if (t <= 0 || s <= 0 || f <= 0) return 0;
        double v = s * Math.Sqrt(t);
        double d1 = (Math.Log(f / k) + v * v / 2) / v;
        return Phi(d1) / (f * v);
    }

    /// <summary>
    /// Il sorriso: la volatilita' di ogni strike dall'opzione fuori dal denaro (put sotto F, call sopra),
    /// con il prezzo scelto, entro +-ampiezza% da F. Con la volatilita' gia' data dalla fonte (Cboe), si usa quella.
    /// </summary>
    public static List<(double strike, double moneyness, double vol)> Sorriso(Scadenza s, DateTime adesso, double forward,
        Prezzi p, double ampiezza = 12, double tasso = Tasso)
    {
        double t = (s.ScadeUtc - adesso).TotalDays * AnniPerGiorno;
        var out_ = new List<(double, double, double)>();
        if (t <= 0) return out_;
        double sconto = Math.Exp(tasso * t);
        foreach (var q in s.Opzioni.Where(q => q.Call == q.Strike >= forward).OrderBy(q => q.Strike))
        {
            double m = (q.Strike / forward - 1) * 100;
            if (Math.Abs(m) > ampiezza) continue;
            double? v = q.Iv is > 0 ? q.Iv : Prezzo(q, p) is { } pr ? Implicita(pr * sconto, forward, q.Strike, t, q.Call) : null;
            if (v is { } vol) out_.Add((q.Strike, Math.Round(m, 3), Math.Round(vol * 100, 3)));
        }
        return out_;
    }

    /// <summary>La volatilita' al prezzo (ATM): interpolata nel sorriso al forward, in punti percentuali.</summary>
    public static double? AlPrezzo(IReadOnlyList<(double strike, double moneyness, double vol)> sorriso)
    {
        if (sorriso.Count == 0) return null;
        var sotto = sorriso.Where(x => x.moneyness <= 0).OrderBy(x => x.moneyness).LastOrDefault();
        var sopra = sorriso.Where(x => x.moneyness > 0).OrderBy(x => x.moneyness).FirstOrDefault();
        if (sotto == default) return sopra.vol;
        if (sopra == default) return sotto.vol;
        double w = (0 - sotto.moneyness) / (sopra.moneyness - sotto.moneyness);
        return sotto.vol + w * (sopra.vol - sotto.vol);
    }

    /// <summary>
    /// La volatilita' implicita di OptionsDepth da vanna e gamma della stessa opzione (x-ray-in-vivo.md,
    /// "La Volatilita' Implicita"): vanna/gamma = -S * ln(F/K) / sigma (a meno di sigma^2 T / 2), quindi
    /// sigma = S * ln(K/F) / (vex/gex), con F dove il rapporto si annulla. Restituisce F e il sorriso.
    /// </summary>
    public static (double forward, List<(double strike, double moneyness, double vol)> sorriso)? DaVannaGamma(
        double spot, IReadOnlyList<double> strike, IReadOnlyList<double> gexCall, IReadOnlyList<double> gexPut,
        IReadOnlyList<double> vexCall, IReadOnlyList<double> vexPut)
    {
        var rapporti = new List<(double k, double r)>();
        for (int i = 0; i < strike.Count; i++)
        {
            // la posizione si elide nel rapporto: basta una delle due, quella con la gamma piu' grande
            bool c = Math.Abs(gexCall[i]) >= Math.Abs(gexPut[i]);
            double g = c ? gexCall[i] : gexPut[i], v = c ? vexCall[i] : vexPut[i];
            if (Math.Abs(g) > 1e-3 && v != 0) rapporti.Add((strike[i], v / g));
        }
        rapporti.Sort((a, b) => a.k.CompareTo(b.k));
        double? f = null;
        for (int i = 1; i < rapporti.Count; i++)
        {
            var (k1, r1) = rapporti[i - 1];
            var (k2, r2) = rapporti[i];
            if (r1 < 0 && r2 >= 0) { f = k1 + (k2 - k1) * (-r1) / (r2 - r1); break; }
        }
        if (f is not { } fw) return null;
        var sorriso = new List<(double, double, double)>();
        foreach (var (k, r) in rapporti)
        {
            double x = Math.Log(k / fw);
            if (Math.Abs(x) < 0.004 || r == 0) continue;     // vicino a F il rapporto e' quasi zero: instabile
            double s = spot * x / r;
            if (s is > 0.02 and < 3) sorriso.Add((k, Math.Round((k / fw - 1) * 100, 3), Math.Round(s * 100, 3)));
        }
        return (fw, sorriso);
    }

    /// <summary>La volatilita' al prezzo di un sorriso X-Ray: la media degli strike entro l'1% da F (come nella prova del 2/11).</summary>
    public static double? MediaVicina(IReadOnlyList<(double strike, double moneyness, double vol)> sorriso, double entro = 1.2)
    {
        var v = sorriso.Where(x => Math.Abs(x.moneyness) < entro).Select(x => x.vol).ToList();
        return v.Count > 0 ? v.Average() : null;
    }
}
