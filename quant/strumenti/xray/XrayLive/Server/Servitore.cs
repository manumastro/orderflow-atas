using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XrayLive.Server;

/// <summary>
/// Il server della pagina: l'API JSON (/api/...) e i file del client (XrayWeb/dist), solo su
/// 127.0.0.1. Non scrive niente: legge l'archivio che il lettore pubblica.
/// </summary>
internal sealed class Servitore
{
    public const int PortaPredefinita = 8790;

    // quale indicatore X-Ray tenere aperto su ES perche' arrivi una famiglia
    private static readonly Dictionary<string, string> Indicatori = new()
    {
        ["levels"] = "Options Key Levels",
        ["observed"] = "Strike Profile (gamma, charm)",
        ["forecast"] = "Strike Profile: previsione",
        ["nextday"] = "Strike Profile: giorno dopo",
        ["gex"] = "GEX Heatmap / Strike Profile GEX",
        ["dex"] = "Strike Profile DEX",
        ["vex"] = "Strike Profile VEX",
        ["pressure"] = "Hedge Pressure",
    };

    private readonly string _root;
    private readonly string _pagina;
    private readonly int _porta;
    private readonly Magazzino _magazzino;
    private readonly Ponte _ponte;
    private readonly Misure _misure;
    private readonly Volatilita.Raccolta _raccolta;
    private readonly Volatilita.MisureVol _vol;
    private readonly Letture _letture;
    private readonly Bordi _bordi;
    private readonly ScaliniDalPonte _scalini;
    private readonly QuattroDomande _quattro;
    private readonly CancellationTokenSource _stop = new();
    private readonly DateTime _avviato = DateTime.UtcNow;
    private WebApplication? _app;

    public Servitore(string root, string pagina, int porta, string bridge)
    {
        _root = root;
        _pagina = pagina;
        _porta = porta;
        _magazzino = new Magazzino(root);
        _ponte = new Ponte(bridge);
        // i bordi del valore (VAL, VAH, POC, muri del ping-pong) dal bridge, per la scala e le quattro domande
        // gli scalini e il ping-pong (5 novembre 2026): la regola sta qui sola; il giro e lo studio la leggono da /api/scalini
        _scalini = new ScaliniDalPonte(bridge);
        _bordi = new Bordi(bridge, _scalini);
        _misure = new Misure(_magazzino, _ponte, _bordi);
        // le fonti nuove (2 novembre 2026): catena CME via ATAS, Cboe pubblico, indici ufficiali
        string datiVol = Path.GetFullPath(Path.Combine(root, "..", "volatilita"));
        string repo = Path.GetFullPath(Path.Combine(root, "..", "..", ".."));
        _raccolta = new Volatilita.Raccolta(datiVol, bridge, repo);
        _vol = new Volatilita.MisureVol(_magazzino, _ponte, _misure, _raccolta, new Volatilita.Yahoo(), StatoXray, _bordi);
        // le opzioni in quattro domande (5 novembre 2026): la regola sta qui, xray_query.py la legge da /api/quattro-domande
        _quattro = new QuattroDomande(_misure, _vol, _bordi);
        // le letture dal vivo dell'agente (2 novembre 2026): le scrive scripts/scrivi_lettura.py, la pagina le mostra
        // con il prezzo di adesso dello strumento dell'ultima lettura (5 novembre 2026), dal bridge
        _letture = new Letture(Path.GetFullPath(Path.Combine(root, "..", "letture")), radice => _raccolta.PrezzoDi(radice, DateTime.UtcNow));
    }

    /// <summary>Accende anche la raccolta delle fonti nuove (lo fa --serve; le prove la lasciano spenta).</summary>
    public bool Raccogli { get; init; } = true;

    public string Indirizzo => $"http://127.0.0.1:{_porta}/";

    public void Avvia()
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { ContentRootPath = AppContext.BaseDirectory });
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls($"http://127.0.0.1:{_porta}");
        builder.Services.ConfigureHttpJsonOptions(o =>
        {
            o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            o.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        });
        var app = builder.Build();

        app.Use(async (ctx, next) =>
        {
            // la pagina e' uno strumento: niente cache del browser, si vede sempre l'ultima versione
            ctx.Response.Headers.CacheControl = "no-store";
            try
            {
                await next();
            }
            catch (Exception ex) when (!ctx.Response.HasStarted)
            {
                ctx.Response.StatusCode = 500;
                await ctx.Response.WriteAsJsonAsync(new { errore = ex.GetType().Name + ": " + ex.Message });
            }
        });

        if (Directory.Exists(_pagina))
        {
            var file = new PhysicalFileProvider(_pagina);
            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = file });
            app.UseStaticFiles(new StaticFileOptions { FileProvider = file });
        }
        else
        {
            app.MapGet("/", () => Results.Text($"La pagina non c'e': manca {_pagina}", "text/plain; charset=utf-8"));
        }

        var api = app.MapGroup("/api");
        api.MapGet("/stato", () => Results.Json(Stato()));
        api.MapGet("/giorni", () => Results.Json(new { giorni = _misure.Giorni() }));
        api.MapGet("/istanti", (string? giorno) => Con(giorno, g => _misure.Istanti(g)));
        api.MapGet("/base", (string? giorno) => Con(giorno, g => _misure.Base(g)));
        api.MapGet("/adesso", (string? giorno, long? ts) => Con(giorno, g => _misure.Adesso(g, ts), ts));
        api.MapGet("/profilo", (string? giorno, long? ts, string? greca, double? ampiezza) =>
            Con(giorno, g => _misure.Profilo(g, ts, Scelta(greca, "gamma", "gamma", "charm"), Ampiezza(ampiezza, 2.5)), ts));
        api.MapGet("/strike", (string? giorno, long? ts, string? metrica, string? gruppo, string? scadenza, double? ampiezza) =>
        {
            string m = Scelta(metrica, "gex", "gex", "dex", "vex");
            string gr = m == "dex" ? Scelta(gruppo, "nonprocust", Misure.Gruppi) : "mm";
            string sc = m == "dex" ? Scelta(scadenza, "0dte", "0dte", "all") : Scelta(scadenza, "0dte", Misure.Scadenze);
            return Con(giorno, g => _misure.PerStrike(g, ts, m, gr, sc, Ampiezza(ampiezza, 3)), ts);
        });
        api.MapGet("/heatmap", (string? giorno, long? ts, string? metrica, string? ambito, double? ampiezza) =>
        {
            string m = Scelta(metrica, "gex", "gex", "dex", "vex");
            return Con(giorno, g => _misure.HeatMap(g, ts, m, Scelta(ambito, "0dte", "0dte", "all"), Ampiezza(ampiezza, 2)), ts);
        });
        api.MapGet("/intervalli", (string? giorno, string? greca, int? passo, double? ampiezza) =>
            Con(giorno, g => _misure.Intervalli(g, Scelta(greca, "gamma", "gamma", "charm"), Passo(passo), Ampiezza(ampiezza, 1.5))));
        api.MapGet("/intervalli-strike", (string? giorno, string? metrica, string? scadenza, int? passo, double? ampiezza) =>
            Con(giorno, g => _misure.IntervalliStrike(g, Scelta(metrica, "gex", "gex", "vex"), Scelta(scadenza, "0dte", Misure.Scadenze),
                Passo(passo), Ampiezza(ampiezza, 1.5))));
        api.MapGet("/deriva", (string? giorno, int? passo) => Con(giorno, g => _misure.Deriva(g, Passo(passo, 1))));
        api.MapGet("/previsione", (string? giorno, string? tipo, string? greca, long? asOf) =>
            Con(giorno, g => _misure.Previsione(g, Scelta(tipo, "forecast", "forecast", "nextday"), Scelta(greca, "gamma", "gamma", "charm"), asOf)));
        api.MapGet("/livelli", (string? giorno, int? passo) => Con(giorno, g => _misure.Livelli(g, Passo(passo, 1))));

        api.MapGet("/fonti", () => Results.Json(_vol.Fonti()));
        api.MapGet("/vol/serie", (string? giorno) =>
            Results.Json(_vol.Serie(string.IsNullOrEmpty(giorno) ? Volatilita.Raccolta.GiornoNY(DateTime.UtcNow) : giorno)));
        api.MapGet("/vol/termine", () => Results.Json(_vol.Termine()));
        api.MapGet("/vol/in-breve", () => Results.Json(_vol.InBreve()));
        api.MapGet("/vol/sorriso", (string? indice) => Results.Json(_vol.Sorriso(Scelta(indice, "SPX", "SPX", "NDX"))));
        api.MapGet("/nq/gex", (string? ambito) => Results.Json(_vol.NqGex(Scelta(ambito, "tutte", "tutte", "0dte"))));
        api.MapGet("/gex/tempo", (string? radice, string? giorno) => Results.Json(_vol.GexTempo(Scelta(radice, "NQ", "NQ", "ES"),
            string.IsNullOrEmpty(giorno) ? Volatilita.Raccolta.GiornoNY(DateTime.UtcNow) : giorno)));
        api.MapGet("/lettura", (string? giorno) => Results.Json(_letture.Del(giorno)));
        api.MapGet("/quattro-domande", (string? radice, string? giorno, long? ts) =>
        {
            string r = Scelta(radice?.ToUpperInvariant(), "ES", "ES", "NQ");
            if (r == "ES" && !string.IsNullOrEmpty(giorno) && !_misure.Giorni().Contains(giorno))
                return Results.Json(new { errore = $"giornata {giorno} non presente" }, statusCode: 404);
            if (r == "ES" && (string.IsNullOrEmpty(giorno) ? _misure.Giorni().FirstOrDefault() : giorno) is { } gq && FuoriGiornata(gq, ts) is { } fuori)
                return fuori;
            return Results.Json(_quattro.Di(r, giorno, ts));
        });
        // gli scalini di mese, settimana e giorno e il ping-pong, con le chiavi del dizionario Python di prima
        // (FabioOrderFlow/tools/gli_scalini.py e' il client); 503 con l'errore se il bridge o il chart Daily manca
        api.MapGet("/scalini", (string? radice, string? giorno) =>
        {
            string r = Scelta(radice?.ToUpperInvariant(), "NQ", "NQ", "ES");
            var s = _scalini.Di(r, string.IsNullOrEmpty(giorno) ? null : giorno);
            return s.Misura is { } m ? Results.Json(m) : Results.Json(new { errore = s.Errore }, statusCode: 503);
        });
        api.MapGet("/es/gex-confronto", (string? giorno, long? ts) => Con(giorno, g => _vol.EsConfronto(g, ts), ts));

        app.StartAsync().GetAwaiter().GetResult();
        _app = app;
        if (Raccogli) _raccolta.Avvia(_stop.Token);
    }

    public void Ferma()
    {
        _stop.Cancel();
        _app?.StopAsync().GetAwaiter().GetResult();
    }

    // ---------------------------------------------------------------- parametri

    private IResult Con(string? giorno, Func<string, object> fai, long? ts = null)
    {
        var giorni = _misure.Giorni();
        string? g = string.IsNullOrEmpty(giorno) ? giorni.FirstOrDefault() : giorno;
        if (g is null) return Results.Json(new { errore = "nessuna giornata nell'archivio" }, statusCode: 404);
        if (!giorni.Contains(g)) return Results.Json(new { errore = $"giornata {g} non presente" }, statusCode: 404);
        if (FuoriGiornata(g, ts) is { } fuori) return fuori;
        return Results.Json(fai(g));
    }

    /// <summary>Un istante prima del primo dato della giornata non ha misure: senza questo controllo i calcoli davano
    /// valori infiniti e la risposta era un 500 (5 novembre 2026). Dopo l'ultimo dato vale l'ultimo, come sempre.</summary>
    private IResult? FuoriGiornata(string giorno, long? ts)
    {
        if (ts is not { } t) return null;
        var tempi = _misure.Tempi(giorno);
        if (tempi.Count > 0 && t >= tempi[0]) return null;
        return Results.Json(new { errore = $"istante fuori dalla giornata {giorno}: il primo dato e' delle {(tempi.Count > 0 ? Cassa.OraNY(tempi[0]).ToString("HH:mm") : "-")} di New York" }, statusCode: 404);
    }

    private static string Scelta(string? valore, string predefinito, params string[] ammessi) =>
        valore is not null && ammessi.Contains(valore) ? valore : predefinito;

    private static double Ampiezza(double? v, double predefinita) => v is { } a && a > 0 && a <= 20 ? a : predefinita;

    private static int Passo(int? v, int predefinito = 5) => v is 1 or 5 or 15 or 30 ? v.Value : predefinito;

    // ---------------------------------------------------------------- stato

    /// <summary>Lo stato del lettore (status.json), l'eta' di ogni famiglia e quella del server.</summary>
    private object Stato()
    {
        var s = Leggi(Path.Combine(_root, "status.json"));
        double fa = SecondiDa(s?["lastSweepUtc"]?.GetValue<string>());
        double? chunkFa = s?["lastChunkUtc"] is JsonValue v && v.TryGetValue<string>(out var lc) ? SecondiDa(lc) : null;
        var ricevute = Ricevute();
        var famiglie = Directory.Exists(Path.Combine(_root, "series", "SPX"))
            ? Directory.GetDirectories(Path.Combine(_root, "series", "SPX")).Select(Path.GetFileName).OfType<string>().OrderBy(n => n)
                .Select(n => new
                {
                    nome = n,
                    minuti = ricevute.TryGetValue(n, out var q) ? Math.Round((DateTime.UtcNow - q).TotalMinutes, 1) : (double?)null,
                    indicatore = Indicatori.GetValueOrDefault(n.Split("__")[0], "?"),
                }).ToList<object>()
            : new List<object>();
        return new
        {
            lettore = new
            {
                presente = s is not null,
                vivo = s is not null && fa < 120,
                secondiFa = double.IsFinite(fa) ? Math.Round(fa) : (double?)null,
                ultimoChunkSecondiFa = chunkFa is { } c && double.IsFinite(c) ? Math.Round(c) : (double?)null,
                chunk = s?["chunks"]?.GetValue<long>(),
                errori = s?["errors"]?.GetValue<long>(),
                ultimoErrore = s?["lastError"] is JsonValue e && e.TryGetValue<string>(out var err) ? err : null,
                archivioMB = s?["archiveMB"]?.GetValue<double>(),
                giornate = s?["archiveDays"]?.GetValue<double>(),
            },
            famiglie,
            server = new
            {
                indirizzo = Indirizzo,
                avviato = _avviato.ToString("O", CultureInfo.InvariantCulture),
                dati = _root,
                pagina = _pagina,
                bridge = _ponte.Indirizzo,
                bridgeErrore = _ponte.UltimoErrore,
            },
        };
    }

    /// <summary>L'ultimo chunk di X-Ray e l'ultimo errore del lettore (status.json), per /api/fonti.</summary>
    private (DateTime?, string?) StatoXray()
    {
        var s = Leggi(Path.Combine(_root, "status.json"));
        double fa = s?["lastChunkUtc"] is JsonValue v && v.TryGetValue<string>(out var lc) ? SecondiDa(lc) : double.NaN;
        double giro = SecondiDa(s?["lastSweepUtc"]?.GetValue<string>());
        string? errore = s is null ? "status.json assente" : !(giro < 120) ? "il lettore non gira" : null;
        return (double.IsFinite(fa) ? DateTime.UtcNow.AddSeconds(-fa) : null, errore);
    }

    private static JsonObject? Leggi(string path)
    {
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return JsonNode.Parse(reader.ReadToEnd()) as JsonObject;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            return null;
        }
    }

    /// <summary>Quando e' arrivato l'ultimo chunk di ogni famiglia (dalla coda del registro).</summary>
    private Dictionary<string, DateTime> Ricevute()
    {
        var out_ = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        string path = Path.Combine(_root, "journal.jsonl");
        if (!File.Exists(path)) return out_;
        string text;
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            stream.Seek(Math.Max(0, stream.Length - 2_000_000), SeekOrigin.Begin);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            text = reader.ReadToEnd();
        }
        catch (IOException)
        {
            return out_;
        }
        foreach (var line in text.Split('\n'))
        {
            if (line.Length < 2 || line[0] != '{') continue;
            try
            {
                using var doc = JsonDocument.Parse(line);
                var r = doc.RootElement;
                string fam = Lettura.Str(r, "family");
                if (fam.Length == 0) continue;
                if (DateTime.TryParse(Lettura.Str(r, "seenUtc"), CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var when))
                    out_[fam] = when;
            }
            catch (JsonException)
            {
            }
        }
        return out_;
    }

    private static double SecondiDa(string? iso) =>
        iso is not null && DateTime.TryParse(iso, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var when)
            ? (DateTime.UtcNow - when).TotalSeconds
            : double.NaN;

    /// <summary>La cartella del client: quella data; se no il build di XrayWeb (XrayWeb/dist), accanto a XrayLive,
    /// risalendo dall'eseguibile. Senza build (npm run build in XrayWeb) la pagina non c'e', e "/" lo dice.</summary>
    public static string TrovaPagina(string? data)
    {
        if (!string.IsNullOrEmpty(data)) return Path.GetFullPath(data);
        foreach (var nome in new[] { Path.Combine("XrayWeb", "dist") })
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir is not null)
            {
                string p = Path.Combine(dir.FullName, nome);
                if (File.Exists(Path.Combine(p, "index.html"))) return p;
                dir = dir.Parent;
            }
        }
        return Path.Combine(AppContext.BaseDirectory, "XrayWeb", "dist");
    }
}
