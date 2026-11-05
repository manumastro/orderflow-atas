using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace XrayLive.Server;

/// <summary>
/// Gli scalini di mese, settimana e giorno, e il ping-pong (5 novembre 2026). È l'UNICA copia della regola: la
/// pagina (bordi del valore, <see cref="Bordi"/>) la usa da qui, e il giro e lo studio la leggono da
/// <c>/api/scalini?radice=</c> attraverso FabioOrderFlow/tools/gli_scalini.py, che è solo il client. Viene dal video sul
/// bias dalla price action; come si legge: docs/research/metodo/il-bias-del-giorno.md. È CONTESTO, non una regola:
/// quant/studi/scala-dei-tre-tempi l'ha smentito come previsione (NQ 2017-2024, cinque varianti su cinque).
///
/// Le definizioni, sull'ultima candela chiusa contro quella prima (la candela aperta cambia ancora):
/// - scalino su: massimo più alto, minimo più alto E chiusura sopra il massimo di prima;
/// - scalino giù: massimo più basso, minimo più basso E chiusura sotto il minimo di prima;
/// - pausa: tutto il resto (anche un massimo più alto con la chiusura sotto il vecchio massimo).
/// Mese e settimana si costruiscono dalle barre giornaliere del chart Daily (il continuo di ATAS), per giorno di borsa
/// CME: settimana ISO e mese del giorno di borsa. Il primo mese del chart, se ha meno di 15 giornate, si scarta.
/// Il ping-pong: dopo l'ultimo scalino del giornaliero, il massimo e il minimo delle giornate chiuse da allora; un muro
/// è rotto quando una giornata chiude oltre, non quando lo tocca.
///
/// Il giorno di borsa CME di una barra giornaliera comincia alle 17:00 di Chicago del giorno prima ed è chiuso alle
/// 16:00 di Chicago del suo giorno. Il processo gira senza ICU, quindi l'ora si scrive con <see cref="Cassa.NewYork"/>:
/// Chicago è sempre un'ora dietro New York (cambiano l'ora la stessa notte), quindi le 16:00 di Chicago sono le
/// 17:00 di New York e le 12:00 di Chicago le 13:00 di New York.
///
/// Le chiavi della risposta sono quelle del dizionario Python di prima (snake_case), perché lo studio scrive
/// <c>scalini-&lt;strumento&gt;.json</c> così com'è e le skill lo leggono.
/// </summary>
internal static class Scalini
{
    public sealed record Candela(DateTime InizioUtc, double Open, double High, double Low, double Close);

    public sealed record PingPong(double? MuroAlto, double? MuroBasso, string? Dal, int Giornate, string? DopoLoScalino, string? Nota);

    /// <summary>Una candela di una scala (un giorno, una settimana, un mese), con le sue giornate.</summary>
    private sealed record Barra(DateOnly Dal, DateOnly Al, double Open, double High, double Low, double Close, int Giornate = 1);

    private static readonly (string nome, string ruolo)[] Scale =
        { ("mese", "la corrente"), ("settimana", "la marea"), ("giorno", "l'onda") };

    /// <summary>Il giorno di borsa CME della barra che comincia a <paramref name="inizioUtc"/>.</summary>
    public static DateOnly GiornoDiBorsa(DateTime inizioUtc)
    {
        var ny = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(inizioUtc, DateTimeKind.Utc), Cassa.NewYork);
        var d = DateOnly.FromDateTime(ny);
        return ny.Hour >= 13 ? d.AddDays(1) : d;   // le 12:00 di Chicago
    }

    /// <summary>Vero se il giorno di borsa è chiuso all'istante dato: dopo le 16:00 di Chicago (17:00 di New York).</summary>
    public static bool Chiusa(DateOnly giorno, DateTime adessoUtc)
    {
        var fine = TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(giorno.ToDateTime(new TimeOnly(17, 0)), DateTimeKind.Unspecified), Cassa.NewYork);
        return adessoUtc >= fine;
    }

    public static string Stato(double hPrima, double lPrima, double h, double l, double c)
    {
        bool hh = h > hPrima, hl = l > lPrima, lh = h < hPrima, ll = l < lPrima;
        if (hh && hl && c > hPrima) return "scalino su";
        if (lh && ll && c < lPrima) return "scalino giu";
        return "pausa";
    }

    private static string Stato(Barra p, Barra u) => Stato(p.High, p.Low, u.High, u.Low, u.Close);

    /// <summary>Perché una candela è in pausa, in parole (vuoto se è uno scalino).</summary>
    private static List<string> Perche(Barra p, Barra u)
    {
        var out_ = new List<string>();
        if (Stato(p, u) != "pausa") return out_;
        bool hh = u.High > p.High, hl = u.Low > p.Low, lh = u.High < p.High, ll = u.Low < p.Low;
        if (hh && u.Close <= p.High) out_.Add("massimo piu' alto ma chiude sotto il massimo di prima: visitato, non accettato");
        if (ll && u.Close >= p.Low) out_.Add("minimo piu' basso ma chiude sopra il minimo di prima: visitato, non accettato");
        if (lh && hl) out_.Add("candela dentro la precedente");
        if (hh && ll) out_.Add("candela che ingloba la precedente");
        return out_;
    }

    /// <summary>Le barre giornaliere raccolte per chiave (settimana ISO, mese), nell'ordine.</summary>
    private static List<(string chiave, Barra b)> Aggrega(List<Barra> barre, Func<DateOnly, string> chiave)
    {
        var out_ = new List<(string, Barra)>();
        foreach (var g in barre.GroupBy(b => chiave(b.Dal)))
        {
            var l = g.ToList();
            out_.Add((g.Key, new Barra(l[0].Dal, l[^1].Al, l[0].Open, l.Max(x => x.High), l.Min(x => x.Low), l[^1].Close, l.Count)));
        }
        return out_;
    }

    private static string Settimana(DateOnly d)
    {
        var t = d.ToDateTime(TimeOnly.MinValue);
        return $"{ISOWeek.GetYear(t)}-{ISOWeek.GetWeekOfYear(t)}";
    }

    private static string Mese(DateOnly d) => $"{d.Year}-{d.Month}";

    /// <summary>
    /// Tutta la misura (scale, insieme, ping-pong, realizzata) dalle candele del chart Daily (il continuo di ATAS), dalla
    /// più vecchia. Il continuo NON è aggiustato al rollover (.claude/rules/dati-e-bridge.md): <paramref name="scarto"/>
    /// si somma alle barre con giorno di borsa prima di <paramref name="finoA"/> (lo dà il ruolo della tab 1,
    /// <c>aggiustato</c>), così la candela del mese o della settimana del rollover non vede uno scalino che è solo il
    /// cambio di contratto.
    /// </summary>
    public static (Dictionary<string, object?> misura, PingPong pingPong) Calcola(
        IReadOnlyList<Candela> candele, DateTime adessoUtc, double scarto = 0, string? finoA = null)
    {
        DateOnly? lim = finoA is { Length: >= 10 } && DateOnly.TryParseExact(finoA[..10], "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var f) ? f : null;
        var barre = new List<Barra>();
        foreach (var b in candele)
        {
            var giorno = GiornoDiBorsa(b.InizioUtc);
            double k = lim is { } l && giorno < l ? scarto : 0;
            barre.Add(new Barra(giorno, giorno, b.Open + k, b.High + k, b.Low + k, b.Close + k));
        }
        var oggi = GiornoDiBorsa(adessoUtc);
        while (oggi.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) oggi = oggi.AddDays(1);   // il prossimo giorno di borsa
        var chiuseG = barre.Where(b => Chiusa(b.Dal, adessoUtc)).ToList();
        // una settimana o un mese sono chiusi quando non contengono il giorno di borsa di oggi
        var scale = new Dictionary<string, List<Barra>>
        {
            ["giorno"] = chiuseG,
            ["settimana"] = Aggrega(barre, Settimana).Where(x => x.chiave != Settimana(oggi) && Chiusa(x.b.Al, adessoUtc)).Select(x => x.b).ToList(),
            ["mese"] = Aggrega(barre, Mese).Where(x => x.chiave != Mese(oggi) && Chiusa(x.b.Al, adessoUtc)).Select(x => x.b).ToList(),
        };

        var out_ = new Dictionary<string, object?>
        {
            ["aggiustato_al_rollover"] = lim is not null && scarto != 0
                ? new Dictionary<string, object?> { ["scarto"] = scarto, ["prima_del"] = finoA } : null,
            ["convenzione"] = "ultima candela chiusa contro la precedente; scalino = massimo e minimo oltre E chiusura oltre "
                              + "l'estremo di prima; mese e settimana dalle barre giornaliere per giorno di borsa CME",
            ["fonte"] = "docs/research/metodo/il-bias-del-giorno.md, [V1 · 0:49-3:35]",
        };
        var perScala = new Dictionary<string, object?>();
        var stati = new List<string?>();
        foreach (var (nome, ruolo) in Scale)
        {
            var cs = scale[nome];
            if (nome == "mese" && cs.Count > 0 && cs[0].Giornate < 15)
                cs = cs.Skip(1).ToList();   // il primo mese del chart è quasi sempre a metà: non è una candela intera
            if (cs.Count < 2)
            {
                perScala[nome] = new Dictionary<string, object?> { ["ruolo"] = ruolo, ["stato"] = null, ["nota"] = "storia del chart troppo corta" };
                stati.Add(null);
                continue;
            }
            Barra p = cs[^2], u = cs[^1];
            // da quando: l'ultimo scalino di questa scala (per dire "pausa da tre mesi")
            Dictionary<string, object?>? ultimo = null;
            for (int i = cs.Count - 1; i > 0; i--)
            {
                string s = Stato(cs[i - 1], cs[i]);
                if (s == "pausa") continue;
                ultimo = new Dictionary<string, object?> { ["stato"] = s, ["al"] = Iso(cs[i].Al) };
                break;
            }
            string st = Stato(p, u);
            stati.Add(st);
            perScala[nome] = new Dictionary<string, object?>
            {
                ["ruolo"] = ruolo,
                ["candela"] = new Dictionary<string, object?>
                {
                    ["dal"] = Iso(u.Dal), ["al"] = Iso(u.Al), ["open"] = u.Open, ["high"] = u.High, ["low"] = u.Low, ["close"] = u.Close,
                },
                ["prima"] = new Dictionary<string, object?>
                {
                    ["dal"] = Iso(p.Dal), ["al"] = Iso(p.Al), ["high"] = p.High, ["low"] = p.Low, ["close"] = p.Close,
                },
                ["stato"] = st,
                ["perche"] = Perche(p, u),
                ["ultimo_scalino"] = ultimo,
            };
        }
        out_["scale"] = perScala;
        out_["insieme"] = stati.Contains(null) ? "incompleto"
            : stati.All(s => s == "scalino su") ? "allineati su"
            : stati.All(s => s == "scalino giu") ? "allineati giu"
            : "misti";

        // il ping-pong del giornaliero: dall'ultimo scalino, i due muri
        var g = chiuseG;
        int? j0 = null;
        for (int j = g.Count - 1; j > 0; j--)
            if (Stato(g[j - 1], g[j]) != "pausa") { j0 = j; break; }
        PingPong pp;
        if (j0 is null)
            pp = new PingPong(null, null, null, 0, null, "nessuno scalino giornaliero nella storia del chart");
        else if (j0 == g.Count - 1)
            pp = new PingPong(null, null, null, 0, null, $"l'ultima giornata chiusa ({Iso(g[j0.Value].Al)}) e' uno scalino: niente ping-pong");
        else
        {
            var dopo = g.Skip(j0.Value + 1).ToList();
            pp = new PingPong(dopo.Max(x => x.High), dopo.Min(x => x.Low), Iso(dopo[0].Al), dopo.Count, Stato(g[j0.Value - 1], g[j0.Value]), null);
        }
        out_["ping_pong"] = pp.Nota is { } nota
            ? new Dictionary<string, object?> { ["nota"] = nota }
            : new Dictionary<string, object?>
            {
                ["dal"] = pp.Dal, ["giornate"] = pp.Giornate, ["dopo_lo_scalino"] = pp.DopoLoScalino,
                ["muro_alto"] = pp.MuroAlto, ["muro_basso"] = pp.MuroBasso,
            };

        // la volatilità realizzata delle ultime 20 giornate chiuse (chiusura su chiusura, annua con 252): il termine di
        // paragone della volatilità implicita ("implicita meno realizzata", [V2 · 3:43-3:52])
        var cl = chiuseG.Skip(Math.Max(0, chiuseG.Count - 21)).Select(b => b.Close).ToList();
        var r = cl.Zip(cl.Skip(1)).Where(x => x.First > 0 && x.Second > 0).Select(x => Math.Log(x.Second / x.First)).ToList();
        if (r.Count >= 10)
            out_["realizzata_20_giornate"] = Math.Round(100 * Math.Sqrt(r.Sum(x => x * x) / r.Count * 252), 1);
        out_["ultima_chiusa"] = chiuseG.Count > 0 ? Iso(chiuseG[^1].Al) : null;
        return (out_, pp);
    }

    /// <summary>Solo il ping-pong (i bordi del valore delle quattro domande e della scala).</summary>
    public static PingPong Misura(IReadOnlyList<Candela> candele, DateTime adessoUtc, double scarto = 0, string? finoA = null) =>
        Calcola(candele, adessoUtc, scarto, finoA).pingPong;

    private static string Iso(DateOnly d) => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}

/// <summary>
/// Gli scalini di una radice (NQ, ES) letti dal bridge di ATAS: le candele del chart Daily del continuo
/// (<c>/candles?fromBar=0</c>, la tab 1) e lo scarto del rollover del suo ruolo (<c>/ruolo</c> → <c>dati.aggiustato</c>).
/// Serve <c>/api/scalini</c> e i bordi del valore (<see cref="Bordi"/>), con una lettura al minuto al più. Se il
/// bridge o il chart manca, <c>Errore</c> dice cosa manca: mai un'eccezione.
/// </summary>
internal sealed class ScaliniDalPonte
{
    public sealed record Lettura(Dictionary<string, object?>? Misura, Scalini.PingPong? PingPong, string? Errore);

    private readonly string _indirizzo;
    private readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ConcurrentDictionary<string, (DateTime quando, Lettura l)> _cache = new();

    public ScaliniDalPonte(string indirizzo) => _indirizzo = indirizzo.TrimEnd('/');

    /// <summary>Con <paramref name="giorno"/> passato: le giornate chiuse prima della sua cassa (09:30 di New York).</summary>
    public Lettura Di(string radice, string? giorno = null)
    {
        string oggi = Volatilita.Raccolta.GiornoNY(DateTime.UtcNow);
        bool passato = giorno is not null && string.CompareOrdinal(giorno, oggi) < 0;
        string chiave = radice + "|" + (passato ? giorno : "oggi");
        if (_cache.TryGetValue(chiave, out var c) && (passato || DateTime.UtcNow - c.quando < TimeSpan.FromSeconds(60)))
            return c.l;
        var l = Leggi(radice, passato ? giorno : null);
        _cache[chiave] = (DateTime.UtcNow, l);
        return l;
    }

    private Lettura Leggi(string radice, string? giorno)
    {
        string? id, strumento;
        try
        {
            using var doc = JsonDocument.Parse(_http.GetStringAsync(_indirizzo + "/charts").GetAwaiter().GetResult());
            var daily = doc.RootElement.GetProperty("charts").EnumerateArray()
                .Select(x => (id: x.GetProperty("id").GetString(), strumento: x.GetProperty("instrument").GetString() ?? "",
                    tf: x.GetProperty("timeFrame").GetString() ?? ""))
                .FirstOrDefault(x => x.strumento.ToUpperInvariant().StartsWith(radice, StringComparison.Ordinal) && x.tf == "Daily");
            (id, strumento) = (daily.id, daily.strumento);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException)
        {
            return new Lettura(null, null, $"bridge non raggiungibile su {_indirizzo}");
        }
        if (string.IsNullOrEmpty(id)) return new Lettura(null, null, $"manca il chart Daily di {radice} nel bridge");
        try
        {
            var candele = new List<Scalini.Candela>();
            using (var doc = JsonDocument.Parse(_http.GetStringAsync($"{_indirizzo}/candles?chart={id}&fromBar=0").GetAwaiter().GetResult()))
                foreach (var k in doc.RootElement.GetProperty("candles").EnumerateArray())
                    candele.Add(new Scalini.Candela(
                        DateTimeOffset.Parse(k.GetProperty("time").GetString()!, CultureInfo.InvariantCulture).UtcDateTime,
                        k.GetProperty("open").GetDouble(), k.GetProperty("high").GetDouble(),
                        k.GetProperty("low").GetDouble(), k.GetProperty("close").GetDouble()));
            double scarto = 0;
            string? finoA = null;
            using (var doc = JsonDocument.Parse(_http.GetStringAsync($"{_indirizzo}/ruolo?chart={id}").GetAwaiter().GetResult()))
                if (doc.RootElement.TryGetProperty("dati", out var d) && d.ValueKind == JsonValueKind.Object
                    && d.TryGetProperty("aggiustato", out var ag) && ag.ValueKind == JsonValueKind.Object)
                {
                    if (ag.TryGetProperty("scarto", out var s) && s.ValueKind == JsonValueKind.Number) scarto = s.GetDouble();
                    if (ag.TryGetProperty("finoA", out var fa) && fa.ValueKind == JsonValueKind.String) finoA = fa.GetString();
                }
            var adesso = giorno is null ? DateTime.UtcNow
                : DateTimeOffset.FromUnixTimeMilliseconds(Cassa.Di(giorno).apertura).UtcDateTime;
            var (m, pp) = Scalini.Calcola(candele, adesso, scarto, finoA);
            m["chart"] = $"{strumento} Daily";
            m["barre"] = candele.Count;
            return new Lettura(m, pp, null);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException or FormatException)
        {
            return new Lettura(null, null, $"candele Daily di {radice} non lette: {ex.Message}");
        }
    }
}
