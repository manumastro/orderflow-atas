using System.Globalization;
using System.Text.Json;
using XrayLive.Server;

namespace XrayLive.Volatilita;

/// <summary>
/// La catena delle opzioni CME (ES, NQ) che ATAS riceve, dal bridge (`/opzioni` di FabioDataBridge).
///
/// IL TETTO DELLE SOTTOSCRIZIONI: ogni chart ha il suo bridge e il suo tetto di 500 opzioni quotate
/// (`LeOpzioni.cs`); una scadenza con 120 strike per lato ne prende 480. Per tenere vive due scadenze
/// senza richiuderle a ogni giro, la vicina si legge sul chart M1 del contratto e la lontana sul chart
/// Daily del continuo (ESZ6 M1 + ES Daily). L'M30 resta a `gex.py`, che ci legge tutte le scadenze.
///
/// LA CATENA ARRIVA A PEZZI: la prima lettura di NQ del 2 novembre 2026 aveva 54 settlement su 226, e il
/// risultato era sbagliato. Si rilegge finche' i prezzi smettono di crescere.
/// </summary>
internal sealed class CatenaAtas
{
    private static readonly TimeZoneInfo NY = Cassa.NewYork;
    private readonly HttpClient _http;
    public string Indirizzo { get; }

    public CatenaAtas(string indirizzo)
    {
        Indirizzo = indirizzo.TrimEnd('/');
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(200) };
    }

    public sealed record Chart(string Id, string Strumento, string TimeFrame);

    public sealed record Serie(string Giorno, string Tipo, DateTime ScadeUtc);

    public List<Chart> Charts()
    {
        using var doc = JsonDocument.Parse(_http.GetStringAsync(Indirizzo + "/charts").GetAwaiter().GetResult());
        return doc.RootElement.GetProperty("charts").EnumerateArray()
            .Select(c => new Chart(c.GetProperty("id").GetString()!, c.GetProperty("instrument").GetString()!,
                c.GetProperty("timeFrame").GetString()!))
            .ToList();
    }

    /// <summary>I due chart per una radice (ES, NQ): il contratto a 1 minuto e il continuo Daily (se c'e').</summary>
    public static (Chart? vicina, Chart? lontana) ChartPer(IReadOnlyList<Chart> charts, string radice)
    {
        bool contratto(Chart c) => c.Strumento.StartsWith(radice, StringComparison.Ordinal) && c.Strumento.Length > radice.Length;
        var m1 = charts.FirstOrDefault(c => contratto(c) && c.TimeFrame == "M1");
        var daily = charts.FirstOrDefault(c => c.Strumento == radice && c.TimeFrame == "Daily")
                    ?? charts.FirstOrDefault(c => c.Strumento == radice);
        return (m1 ?? daily, daily ?? m1);
    }

    /// <summary>L'istante in cui una serie scade: le 16:00 di New York; le trimestrali (Regular) alle 9:30, come la SOQ del CME.</summary>
    public static DateTime Scade(string giorno, string tipo)
    {
        var d = DateTime.ParseExact(giorno, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        var ora = tipo == "Regular" ? d.AddHours(9.5) : d.AddHours(16);
        return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(ora, DateTimeKind.Unspecified), NY);
    }

    public List<Serie> SerieDi(string chart)
    {
        using var doc = JsonDocument.Parse(_http.GetStringAsync($"{Indirizzo}/opzioni?chart={chart}").GetAwaiter().GetResult());
        return doc.RootElement.GetProperty("series").EnumerateArray()
            .Select(s =>
            {
                string g = s.GetProperty("expiration").GetString()![..10];
                string t = s.GetProperty("type").GetString() ?? "";
                return new Serie(g, t, Scade(g, t));
            })
            .GroupBy(s => s.Giorno).Select(g => g.First())
            .OrderBy(s => s.ScadeUtc).ToList();
    }

    /// <summary>Una scadenza, riletta finche' settlement e offerte smettono di crescere (al massimo `giri` volte).</summary>
    public Scadenza Leggi(string chart, Serie serie, int strike = 120, int giri = 12)
    {
        string url(int attesa, int quiete) =>
            $"{Indirizzo}/opzioni?chart={chart}&expiration={serie.Giorno}&strikes={strike}&wait={attesa}&quiet={quiete}";
        var (q, sottostante) = Scarica(url(20, 3000));
        var conta = Conta(q);
        for (int i = 0; i < giri; i++)
        {
            Thread.Sleep(2500);
            var (n, s) = Scarica(url(3, 1500));
            var cn = Conta(n);
            // 6 novembre 2026: una scadenza appena chiesta arriva vuota (ATAS la sottoscrive in quel momento) e si riempie
            // qualche secondo dopo: con "niente di nuovo, esco" il 4/12-11/12 di ES restava vuoto a ogni giro e il VIX
            // ricostruito era in errore da un giorno. Finche' e' vuota si aspetta tutti i giri
            bool vuota = conta.settlement == 0 && conta.bid == 0;
            if (!vuota && cn.settlement <= conta.settlement && cn.bid <= conta.bid) break;
            (q, sottostante, conta) = (n, s, cn);
        }
        return new Scadenza(serie.Giorno, serie.ScadeUtc, q, sottostante);
    }

    private static (int settlement, int bid) Conta(IReadOnlyList<Quotazione> q) =>
        (q.Count(x => x.Settlement is > 0), q.Count(x => x.Bid is > 0));

    private (List<Quotazione>, double?) Scarica(string url)
    {
        using var doc = JsonDocument.Parse(_http.GetStringAsync(url).GetAwaiter().GetResult());
        var r = doc.RootElement;
        double? sottostante = r.TryGetProperty("underlyingPrice", out var u) && u.ValueKind == JsonValueKind.Number ? u.GetDouble() : null;
        var out_ = new List<Quotazione>();
        foreach (var o in r.GetProperty("options").EnumerateArray())
        {
            if (!o.TryGetProperty("strike", out var k) || k.ValueKind != JsonValueKind.Number) continue;
            out_.Add(new Quotazione(k.GetDouble(), o.GetProperty("type").GetString() == "Call",
                Num(o, "bid"), Num(o, "ask"), Num(o, "settlement"), Num(o, "openInterest") ?? 0));
        }
        return (out_, sottostante);
    }

    private static double? Num(JsonElement e, string nome) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : null;
}
