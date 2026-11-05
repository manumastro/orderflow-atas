using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace XrayLive.Server;

/// <summary>Un bordo del valore, nei prezzi del future (ES o NQ): VAL, VAH, POC di una cassa, o un muro del ping-pong.</summary>
internal sealed record Bordo(string Nome, string Tipo, string? Seduta, double Prezzo);

/// <summary>
/// I bordi del valore con cui si confrontano i muri delle opzioni (5 novembre 2026), dal bridge di ATAS, come
/// <c>le_quattro_tab.bordi_del_valore</c> faceva in Python:
/// - VAL, VAH e POC dell'ultima cassa chiusa ("cassa") e di quella in corso ("cassa di oggi"), dal ruolo della tab M30
///   del contratto (<c>/ruolo?chart=</c>, <c>dati[]</c>);
/// - i due muri del ping-pong del giornaliero (<see cref="Scalini"/>, letti da <see cref="ScaliniDalPonte"/>), dalle
///   candele del chart Daily del continuo, con lo scarto del rollover del suo ruolo.
/// I nomi sono quelli di Python ("VAL cassa mer 04", "muro alto del ping-pong"). Se il bridge o un chart manca, i
/// bordi restano vuoti e la nota dice cosa manca: mai un errore.
/// </summary>
internal sealed class Bordi
{
    private static readonly string[] Giorni = { "lun", "mar", "mer", "gio", "ven", "sab", "dom" };

    private readonly string _indirizzo;
    private readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ConcurrentDictionary<string, (DateTime quando, List<Bordo> bordi, string? nota)> _cache = new();

    private readonly ScaliniDalPonte _scalini;

    public Bordi(string indirizzo, ScaliniDalPonte? scalini = null)
    {
        _indirizzo = indirizzo.TrimEnd('/');
        _scalini = scalini ?? new ScaliniDalPonte(indirizzo);
    }

    /// <summary>I bordi di una radice (ES, NQ). Con <paramref name="giorno"/> passato: la cassa di quel giorno e quella
    /// prima, e il ping-pong delle giornate chiuse prima della sua cassa. Si rileggono al più ogni minuto.</summary>
    public (List<Bordo> bordi, string? nota) Di(string radice, string? giorno = null)
    {
        string oggi = Volatilita.Raccolta.GiornoNY(DateTime.UtcNow);
        bool passato = giorno is not null && string.CompareOrdinal(giorno, oggi) < 0;
        string chiave = radice + "|" + (passato ? giorno : "oggi");
        if (_cache.TryGetValue(chiave, out var c) && (passato || DateTime.UtcNow - c.quando < TimeSpan.FromSeconds(60)))
            return (c.bordi, c.nota);
        var r = Leggi(radice, passato ? giorno : null);
        _cache[chiave] = (DateTime.UtcNow, r.bordi, r.nota);
        return r;
    }

    private (List<Bordo> bordi, string? nota) Leggi(string radice, string? giorno)
    {
        var out_ = new List<Bordo>();
        var note = new List<string>();
        List<(string id, string strumento, string tf)> charts;
        try
        {
            using var doc = JsonDocument.Parse(_http.GetStringAsync(_indirizzo + "/charts").GetAwaiter().GetResult());
            charts = doc.RootElement.GetProperty("charts").EnumerateArray()
                .Select(x => (x.GetProperty("id").GetString() ?? "", x.GetProperty("instrument").GetString() ?? "",
                    x.GetProperty("timeFrame").GetString() ?? ""))
                .ToList();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                       or InvalidOperationException)
        {
            return (out_, $"bridge non raggiungibile su {_indirizzo}: niente bordi del valore");
        }

        // la tab M30 del contratto (NQZ6, ESZ6): come le_tab, il primo chart della radice a 30 minuti
        var m30 = charts.FirstOrDefault(x => x.strumento.StartsWith(radice, StringComparison.Ordinal) && x.tf == "M30");
        if (m30.id is null or "") note.Add($"manca il chart M30 di {radice} nel bridge");
        else
        {
            try
            {
                using var doc = JsonDocument.Parse(_http.GetStringAsync($"{_indirizzo}/ruolo?chart={m30.id}").GetAwaiter().GetResult());
                var dati = doc.RootElement.TryGetProperty("dati", out var d) && d.ValueKind == JsonValueKind.Array
                    ? d.EnumerateArray().Select(x => x.Clone()).ToList()
                    : new List<JsonElement>();
                string G(JsonElement x) => x.TryGetProperty("giorno", out var g) ? g.GetString() ?? "" : "";
                bool InCorso(JsonElement x) => x.TryGetProperty("inCorso", out var v) && v.ValueKind == JsonValueKind.True;
                JsonElement? cassa, diOggi;
                if (giorno is null)
                {
                    cassa = dati.Where(x => !InCorso(x)).Cast<JsonElement?>().LastOrDefault();
                    diOggi = dati.Where(InCorso).Cast<JsonElement?>().FirstOrDefault();
                }
                else
                {
                    cassa = dati.Where(x => string.CompareOrdinal(G(x), giorno) < 0).Cast<JsonElement?>().LastOrDefault();
                    diOggi = dati.Where(x => G(x) == giorno).Cast<JsonElement?>().FirstOrDefault();
                }
                // per un giorno passato "di oggi" non vale: tutte e due sono "cassa", e la data le distingue
                foreach (var (x, nome) in new[] { (cassa, "cassa"), (diOggi, giorno is null ? "cassa di oggi" : "cassa") })
                {
                    if (x is not { } e) continue;
                    foreach (var k in new[] { "val", "vah", "poc" })
                        if (e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number)
                            out_.Add(new Bordo($"{k.ToUpperInvariant()} {nome} {Breve(G(e))}", k.ToUpperInvariant(), G(e), v.GetDouble()));
                }
                if (dati.Count == 0) note.Add($"il ruolo della tab M30 di {radice} non ha casse");
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException
                                           or InvalidOperationException)
            {
                note.Add($"ruolo della tab M30 di {radice} non letto: {ex.Message}");
            }
        }

        // il ping-pong dal Daily del continuo (la tab 1): la regola e la lettura stanno in Scalini.cs, una copia sola
        var s = _scalini.Di(radice, giorno);
        if (s.Errore is { } errore) note.Add(errore.StartsWith("manca", StringComparison.Ordinal) ? errore + " (niente ping-pong)" : errore);
        else if (s.PingPong is { } pp)
        {
            if (pp.MuroAlto is { } alto) out_.Add(new Bordo("muro alto del ping-pong", "muro alto", pp.Dal, alto));
            if (pp.MuroBasso is { } basso) out_.Add(new Bordo("muro basso del ping-pong", "muro basso", pp.Dal, basso));
            if (pp.Nota is { } n) note.Add("ping-pong: " + n);
        }
        return (out_, note.Count > 0 ? string.Join("; ", note) : null);
    }

    /// <summary>"mer 04", come <c>le_quattro_tab.giorno</c>.</summary>
    public static string Breve(string iso)
    {
        if (!DateTime.TryParseExact(iso.Length >= 10 ? iso[..10] : iso, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            return iso;
        int w = ((int)d.DayOfWeek + 6) % 7;
        return $"{Giorni[w]} {d.Day:00}";
    }
}
