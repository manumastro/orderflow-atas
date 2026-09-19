using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Control;
using OFT.Rendering.Tools;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Espone i dati che ATAS mette a disposizione di un indicatore su un endpoint HTTP locale,
/// in modo che un'analisi esterna possa richiederli quando servono invece di dipendere da una
/// cattura decisa in anticipo.
///
/// Il bridge e' osservativo: legge, converte in JSON e restituisce. Non calcola soglie, non
/// classifica e non emette segnali. Ogni risposta contiene lo strumento e il timeframe del chart
/// su cui il bridge e' caricato, perche' quel contesto e' parte del dato e non va ricostruito a
/// posteriori.
///
/// L'unica cosa che il bridge scrive sono i **livelli**: un elenco di prezzi con etichetta che
/// l'analisi esterna deposita su /levels e che l'indicatore disegna sul chart. Restano dati
/// dell'indicatore, non toccano lo stato della piattaforma, non generano ordini e non
/// influenzano nessun calcolo: servono a non dover ridisegnare a mano su ATAS quello che
/// l'analisi ha gia' individuato.
///
/// L'indicatore puo' essere caricato su un numero qualsiasi di chart: le istanze condividono
/// un solo listener di processo, che si registra sulla prima porta libera dell'intervallo
/// dichiarato, e ciascuna richiesta sceglie il chart con il parametro 'chart'. Non c'e' quindi
/// nessuna porta da configurare a mano, e nessun conflitto fra istanze.
///
/// Il listener e' legato a 127.0.0.1: non e' raggiungibile dalla rete.
/// </summary>
[DisplayName("Fabio Data Bridge")]
public sealed class DataBridge : Indicator
{
    private const string Schema = "fof-data-bridge-v1";

    /// <summary>
    /// Intervallo di porte sondate dal listener condiviso. La prima libera vince, cosi' che
    /// un ATAS gia' avviato o un altro programma sulla 8787 non impediscano l'avvio.
    /// </summary>
    private const int PortRangeStart = 8787;

    private const int PortRangeEnd = 8796;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false,
    };

    /// <summary>
    /// ATAS accetta una sola richiesta CumulativeTrades pendente alla volta: il semaforo
    /// serializza le richieste HTTP concorrenti invece di lasciarle fallire.
    /// </summary>
    private readonly SemaphoreSlim _cumulativeGate = new(1, 1);

    /// <summary>
    /// Il listener e' uno solo per processo ATAS e serve tutte le istanze dell'indicatore.
    /// Cosi' l'utente puo' caricare il bridge su quanti chart vuole senza assegnare porte:
    /// un HttpListener per istanza fallirebbe alla seconda con AddressAlreadyInUse.
    /// </summary>
    private static readonly object HubSync = new();

    private static readonly Dictionary<string, DataBridge> Instances = new(StringComparer.Ordinal);

    private static HttpListener? _hubListener;
    private static CancellationTokenSource? _hubShutdown;
    private static int _hubPort;

    /// <summary>Protegge lo stato della richiesta CumulativeTrades pendente di questa istanza.</summary>
    private readonly object _sync = new();

    private readonly CancellationTokenSource _shutdown = new();
    private readonly string _id = Guid.NewGuid().ToString("N")[..8];

    private TaskCompletionSource<List<CumulativeTrade>>? _pendingCumulative;
    private int _pendingCumulativeRequestId;

    /// <summary>
    /// I livelli depositati dall'analisi esterna. La lista viene sostituita per intero a ogni
    /// scrittura invece di essere modificata sul posto: il rendering la legge da un altro thread
    /// e una sostituzione atomica evita di doverlo sincronizzare.
    /// </summary>
    private volatile BridgeLevel[] _levels = Array.Empty<BridgeLevel>();

    private DateTime _levelsUpdatedUtc = DateTime.MinValue;

    /// <summary>
    /// Le righe del pannello: testo che non sta su un prezzo, e quindi non e' un livello. Serve
    /// a mostrare una condizione che l'analisi ricalcola in continuo - "chiusure sopra 3/3",
    /// "big trade sopra il livello: 0" - senza dover aspettare che qualcuno lo chieda a parole.
    /// Stessa idea dei livelli: l'indicatore disegna, non calcola. Chi scrive qui e' l'analisi.
    /// </summary>
    private volatile BridgeWatchLine[] _watch = Array.Empty<BridgeWatchLine>();

    private DateTime _watchUpdatedUtc = DateTime.MinValue;

    /// <summary>
    /// I livelli sopravvivono a un riavvio di ATAS e a un redeploy della DLL. Senza questo
    /// vanno persi a ogni ricarica dell'indicatore, che durante lo sviluppo succede spesso e
    /// che per chi guarda il chart e' semplicemente il lavoro che sparisce. Il file e' uno
    /// solo, con una voce per strumento: e' lo strumento a identificare i livelli, non l'id
    /// dell'istanza, che e' casuale e cambia a ogni caricamento.
    /// </summary>
    private static readonly string LevelsStorePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fabio-data-bridge-levels.json");

    private static readonly object LevelsStoreSync = new();

    /// <summary>Stessa idea dei livelli, per il pannello di testo: un file, una voce per strumento.</summary>
    private static readonly string WatchStorePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fabio-data-bridge-watch.json");

    private static readonly object WatchStoreSync = new();

    public DataBridge()
    {
        Name = "Fabio Data Bridge";
        DenyToChangePanel = true;
        EnableCustomDrawing = true;
        SubscribeToDrawingEvents(DrawingLayouts.Final);
    }

    [Display(Name = "Enabled", GroupName = "Bridge", Description = "Avvia o ferma il listener locale.")]
    public bool BridgeEnabled { get; set; } = true;

    /// <summary>
    /// Numero massimo di elementi restituiti da una singola risposta, per evitare payload
    /// ingestibili. Le richieste piu' ampie vanno spezzate dal chiamante.
    /// </summary>
    [Display(Name = "Max items", GroupName = "Bridge", Description = "Limite di elementi per risposta.")]
    [Range(100, 2_000_000)]
    public int MaxItems { get; set; } = 200_000;

    [Display(Name = "Show levels", GroupName = "Levels", Description = "Disegna i livelli depositati su /levels.")]
    public bool ShowLevels { get; set; } = true;

    [Display(Name = "Label on the right", GroupName = "Levels", Description = "Etichetta a destra invece che a sinistra.")]
    public bool LabelOnRight { get; set; } = true;

    [Display(Name = "Font size", GroupName = "Levels")]
    [Range(6, 24)]
    public int LevelFontSize { get; set; } = 11;

    /// <summary>
    /// Distanza dell'etichetta dal bordo dell'area dati. Serve anche come regolazione fine: la
    /// larghezza della scala dei prezzi cambia con il numero di cifre e con il DPI.
    /// </summary>
    [Display(Name = "Label margin", GroupName = "Levels", Description = "Distanza dal bordo, in pixel.")]
    [Range(0, 400)]
    public int LabelMargin { get; set; } = 8;

    [Display(Name = "Short labels", GroupName = "Levels",
        Description = "Mostra solo la parte prima di ' · ' (es. 'VAH Europa') e il testo intero " +
                       "solo al passaggio del mouse sulla riga. Spento, l'etichetta e' sempre intera.")]
    public bool ShortLabels { get; set; } = true;

    [Display(Name = "Line extent", GroupName = "Levels",
        Description = "Quanto si estende la riga a sinistra dal bordo destro, in pixel. Basso per " +
                       "un chart pulito con solo l'etichetta vicino al prezzo attuale; alto per " +
                       "vedere la riga sulla seduta intera.")]
    [Range(0, 4000)]
    public int LevelLineExtent { get; set; } = 160;

    [Display(Name = "Show watch panel", GroupName = "Watch", Description = "Disegna il pannello depositato su /watch.")]
    public bool ShowWatch { get; set; } = true;

    [Display(Name = "Font size", GroupName = "Watch")]
    [Range(6, 24)]
    public int WatchFontSize { get; set; } = 12;

    [Display(Name = "Margin", GroupName = "Watch", Description = "Distanza dal bordo superiore e destro, in pixel.")]
    [Range(0, 400)]
    public int WatchMargin { get; set; } = 12;

    protected override void OnCalculate(int bar, decimal value)
    {
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Register();
        RestoreLevels();
        RestoreWatch();
    }

    protected override void OnDispose()
    {
        Unregister();
        base.OnDispose();
    }

    private void Register()
    {
        if (!BridgeEnabled)
        {
            return;
        }

        lock (HubSync)
        {
            Instances[_id] = this;
            EnsureHub(this);
        }
    }

    private void Unregister()
    {
        lock (HubSync)
        {
            if (!Instances.Remove(_id))
            {
                return;
            }

            try
            {
                _shutdown.Cancel();
            }
            catch (ObjectDisposedException)
            {
            }

            if (Instances.Count == 0)
            {
                StopHub(this);
            }
        }
    }

    /// <summary>
    /// Avvia il listener condiviso se non e' gia' attivo, provando le porte dell'intervallo
    /// finche' una accetta. La porta scelta viene scritta nel file di discovery cosi' che il
    /// client non debba conoscerla ne' tentarla.
    /// </summary>
    private static void EnsureHub(DataBridge origin)
    {
        if (_hubListener is not null)
        {
            return;
        }

        for (var port = PortRangeStart; port <= PortRangeEnd; port++)
        {
            try
            {
                var listener = new HttpListener();
                listener.Prefixes.Add($"http://127.0.0.1:{port}/");
                listener.Start();

                var shutdown = new CancellationTokenSource();
                _hubListener = listener;
                _hubShutdown = shutdown;
                _hubPort = port;
                _ = Task.Run(() => AcceptLoopAsync(listener, shutdown.Token));
                WriteDiscovery(port);
                origin.LogInfo("FofDataBridge hub listening on http://127.0.0.1:{0}/ schema {1}", port, Schema);
                return;
            }
            catch (HttpListenerException)
            {
                // Porta occupata, anche da un processo estraneo: si prova la successiva.
            }
            catch (Exception exception)
            {
                origin.LogError($"FofDataBridge could not listen on port {port}.", exception);
                return;
            }
        }

        origin.LogError(
            $"FofDataBridge found no free port in {PortRangeStart}-{PortRangeEnd}.",
            new InvalidOperationException("no free port"));
    }

    private static void StopHub(DataBridge origin)
    {
        var listener = _hubListener;
        if (listener is null)
        {
            return;
        }

        try
        {
            _hubShutdown?.Cancel();
            listener.Stop();
            listener.Close();
            DeleteDiscovery();
        }
        catch (Exception exception)
        {
            origin.LogError("FofDataBridge hub failed to stop cleanly.", exception);
        }
        finally
        {
            _hubListener = null;
            _hubShutdown = null;
            _hubPort = 0;
        }
    }

    private static string DiscoveryPath() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".fabio-data-bridge.json");

    private static void WriteDiscovery(int port)
    {
        try
        {
            var payload = JsonSerializer.Serialize(
                new { schema = Schema, port, baseUrl = $"http://127.0.0.1:{port}", writtenUtc = Iso(DateTime.UtcNow) },
                JsonOptions);
            File.WriteAllText(DiscoveryPath(), payload);
        }
        catch (Exception)
        {
            // Il file e' una comodita': se il filesystem lo rifiuta, il client sonda le porte.
        }
    }

    private static void DeleteDiscovery()
    {
        try
        {
            File.Delete(DiscoveryPath());
        }
        catch (Exception)
        {
        }
    }

    private static async Task AcceptLoopAsync(HttpListener listener, CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested && listener.IsListening)
        {
            HttpListenerContext context;
            try
            {
                context = await listener.GetContextAsync().ConfigureAwait(false);
            }
            catch (Exception) when (cancellation.IsCancellationRequested || !listener.IsListening)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }

            _ = Task.Run(() => DispatchAsync(context, cancellation), CancellationToken.None);
        }
    }

    /// <summary>
    /// Sceglie l'istanza che deve rispondere. Con un solo chart registrato il parametro
    /// 'chart' e' superfluo; con piu' chart una richiesta ambigua viene rifiutata invece di
    /// essere servita da un chart arbitrario, perche' lo strumento fa parte del dato.
    /// </summary>
    private static async Task DispatchAsync(HttpListenerContext context, CancellationToken cancellation)
    {
        var path = context.Request.Url?.AbsolutePath.TrimEnd('/') ?? string.Empty;

        DataBridge[] registered;
        lock (HubSync)
        {
            registered = Instances.Values.ToArray();
        }

        if (path is "/charts")
        {
            await WriteAsync(context, 200, new
            {
                schema = Schema,
                port = _hubPort,
                count = registered.Length,
                charts = registered.Select(instance => instance.ChartDescriptor()),
            }).ConfigureAwait(false);
            return;
        }

        var selector = context.Request.QueryString["chart"];
        var matches = Select(registered, selector);

        if (matches.Length == 1)
        {
            await matches[0].HandleAsync(context, path, cancellation).ConfigureAwait(false);
            return;
        }

        var reason = matches.Length == 0
            ? registered.Length == 0
                ? "no chart has the Fabio Data Bridge indicator loaded"
                : $"no chart matches '{selector}'"
            : selector is null
                ? $"{matches.Length} charts are registered: repeat the request with ?chart=<id|instrument>"
                : $"'{selector}' matches {matches.Length} charts: use a full id from /charts";

        await WriteAsync(context, matches.Length == 0 && registered.Length == 0 ? 503 : 400, new
        {
            schema = Schema,
            error = reason,
            charts = registered.Select(instance => instance.ChartDescriptor()),
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Un selettore vuoto vale per tutti; altrimenti si prova l'id esatto, poi lo strumento
    /// esatto, poi il prefisso dello strumento. L'ordine conta: il contratto continuo 'NQ' e'
    /// prefisso di 'NQU6', quindi senza la corrispondenza esatta prima sarebbe irraggiungibile.
    /// </summary>
    private static DataBridge[] Select(DataBridge[] registered, string? selector)
    {
        if (string.IsNullOrWhiteSpace(selector))
        {
            return registered;
        }

        var wanted = selector.Trim();

        var byId = registered
            .Where(instance => string.Equals(instance._id, wanted, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (byId.Length > 0)
        {
            return byId;
        }

        var exact = registered
            .Where(instance => string.Equals(instance.InstrumentInfo?.Instrument, wanted, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (exact.Length > 0)
        {
            return exact;
        }

        return registered
            .Where(instance => (instance.InstrumentInfo?.Instrument ?? string.Empty)
                .StartsWith(wanted, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    private object ChartDescriptor() => new
    {
        id = _id,
        instrument = InstrumentInfo?.Instrument,
        exchange = InstrumentInfo?.Exchange,
        timeFrame = ChartInfo?.TimeFrame,
        chartType = ChartInfo?.ChartType,
        bars = CurrentBar,
    };

    private async Task HandleAsync(HttpListenerContext context, string path, CancellationToken cancellation)
    {
        var query = context.Request.QueryString;

        try
        {
            object payload = path switch
            {
                "" or "/health" => Health(),
                "/instrument" => InstrumentPayload(),
                "/limits" => Limits(),
                "/session" => Session(query),
                "/rollovers" => await RolloversAsync(query, cancellation).ConfigureAwait(false),
                "/profile" => await ProfileAsync(query).ConfigureAwait(false),
                "/candles" => Candles(query),
                "/cumulative" => await CumulativeAsync(query, cancellation).ConfigureAwait(false),
                "/depth" => await DepthAsync(query, cancellation).ConfigureAwait(false),
                "/levels" => await LevelsAsync(context).ConfigureAwait(false),
                "/watch" => await WatchAsync(context).ConfigureAwait(false),
            "/charts" => throw new BridgeException(500, "handled by the hub"),
                _ => throw new BridgeException(404, $"unknown endpoint '{path}'"),
            };

            await WriteAsync(context, 200, payload).ConfigureAwait(false);
        }
        catch (BridgeException bridgeException)
        {
            await WriteAsync(context, bridgeException.Status, new { schema = Schema, error = bridgeException.Message })
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            this.LogError($"FofDataBridge request '{path}' failed.", exception);
            await WriteAsync(context, 500, new { schema = Schema, error = exception.Message }).ConfigureAwait(false);
        }
    }

    private static async Task WriteAsync(HttpListenerContext context, int status, object payload)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload, JsonOptions));
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.ContentLength64 = body.Length;
        await context.Response.OutputStream.WriteAsync(body).ConfigureAwait(false);
        context.Response.Close();
    }

    // ---------------------------------------------------------------- livelli

    /// <summary>
    /// Un livello depositato dall'analisi. Solo <c>price</c> e' obbligatorio: il resto ha
    /// valori di default sensati, cosi' che depositare una lista di prezzi funzioni subito.
    /// </summary>
    private sealed record BridgeLevel
    {
        public decimal Price { get; init; }

        public string? Label { get; init; }

        /// <summary>Esadecimale <c>#RRGGBB</c> o <c>#AARRGGBB</c>; omesso usa il colore di default.</summary>
        public string? Color { get; init; }

        /// <summary>solid | dash | dot</summary>
        public string? Style { get; init; }

        public int Width { get; init; } = 1;

        /// <summary>Testo libero: non viene disegnato, torna su GET. Serve a ricordare perche' il livello c'e'.</summary>
        public string? Note { get; init; }
    }

    /// <summary>Rilegge dal file i livelli di questo strumento, se ce ne sono.</summary>
    private void RestoreLevels()
    {
        var instrument = InstrumentInfo?.Instrument;
        if (string.IsNullOrWhiteSpace(instrument))
        {
            return;
        }

        try
        {
            var store = ReadLevelsStore();
            if (store.TryGetValue(instrument, out var saved) && saved.Length > 0)
            {
                _levels = saved;
                _levelsUpdatedUtc = DateTime.UtcNow;
            }
        }
        catch (Exception exception)
        {
            // Un file corrotto non deve impedire il caricamento dell'indicatore.
            this.LogError("FofDataBridge could not restore the saved levels.", exception);
        }
    }

    private static Dictionary<string, BridgeLevel[]> ReadLevelsStore()
    {
        if (!File.Exists(LevelsStorePath))
        {
            return new Dictionary<string, BridgeLevel[]>(StringComparer.OrdinalIgnoreCase);
        }

        var json = File.ReadAllText(LevelsStorePath);
        return JsonSerializer.Deserialize<Dictionary<string, BridgeLevel[]>>(json, JsonOptions)
               ?? new Dictionary<string, BridgeLevel[]>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Scrive i livelli di questo strumento nel file condiviso, lasciando intatti quelli degli
    /// altri strumenti. Un errore qui non deve far fallire la richiesta: i livelli sono gia'
    /// applicati in memoria e disegnati.
    /// </summary>
    private void PersistLevels()
    {
        var instrument = InstrumentInfo?.Instrument;
        if (string.IsNullOrWhiteSpace(instrument))
        {
            return;
        }

        try
        {
            lock (LevelsStoreSync)
            {
                var store = ReadLevelsStore();
                if (_levels.Length == 0)
                {
                    store.Remove(instrument);
                }
                else
                {
                    store[instrument] = _levels;
                }

                File.WriteAllText(LevelsStorePath, JsonSerializer.Serialize(store, JsonOptions));
            }
        }
        catch (Exception exception)
        {
            this.LogError("FofDataBridge could not save the levels.", exception);
        }
    }

    private async Task<object> LevelsAsync(HttpListenerContext context)
    {
        var method = context.Request.HttpMethod.ToUpperInvariant();

        if (method is "DELETE")
        {
            _levels = Array.Empty<BridgeLevel>();
            _levelsUpdatedUtc = DateTime.UtcNow;
            PersistLevels();
            Repaint();
            return LevelsPayload();
        }

        if (method is "POST" or "PUT")
        {
            using var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new BridgeException(400, "empty body: send a JSON array of levels, or {\"levels\": [...]}");
            }

            BridgeLevel[]? parsed;
            try
            {
                var trimmed = body.TrimStart();
                parsed = trimmed.StartsWith('[')
                    ? JsonSerializer.Deserialize<BridgeLevel[]>(body, JsonOptions)
                    : JsonSerializer.Deserialize<LevelsRequest>(body, JsonOptions)?.Levels;
            }
            catch (JsonException exception)
            {
                throw new BridgeException(400, $"malformed JSON: {exception.Message}");
            }

            if (parsed is null)
            {
                throw new BridgeException(400, "no levels in the request");
            }

            // Un prezzo a zero e' quasi sempre un campo mancante, non un livello: meglio dirlo
            // subito che disegnare una riga sul fondo del chart.
            var bad = parsed.FirstOrDefault(level => level.Price <= 0);
            if (bad is not null)
            {
                throw new BridgeException(400, "every level needs a positive 'price'");
            }

            _levels = parsed.OrderByDescending(level => level.Price).ToArray();
            _levelsUpdatedUtc = DateTime.UtcNow;
            PersistLevels();
            Repaint();
            return LevelsPayload();
        }

        return LevelsPayload();
    }

    private sealed record LevelsRequest
    {
        public BridgeLevel[]? Levels { get; init; }
    }

    // ---------------------------------------------------------------- pannello (watch)

    /// <summary>
    /// Una riga del pannello. Nessun prezzo: sta in un angolo fisso dello schermo, non su una
    /// candela, perche' quello che deve mostrare - una condizione, un conteggio - non ha un
    /// prezzo proprio.
    /// </summary>
    private sealed record BridgeWatchLine
    {
        public string Text { get; init; } = "";

        /// <summary>Esadecimale <c>#RRGGBB</c> o <c>#AARRGGBB</c>; omesso usa il colore di default.</summary>
        public string? Color { get; init; }
    }

    private sealed record WatchRequest
    {
        public BridgeWatchLine[]? Lines { get; init; }
    }

    private void RestoreWatch()
    {
        var instrument = InstrumentInfo?.Instrument;
        if (string.IsNullOrWhiteSpace(instrument))
        {
            return;
        }

        try
        {
            var store = ReadWatchStore();
            if (store.TryGetValue(instrument, out var saved) && saved.Length > 0)
            {
                _watch = saved;
                _watchUpdatedUtc = DateTime.UtcNow;
            }
        }
        catch (Exception exception)
        {
            this.LogError("FofDataBridge could not restore the saved watch panel.", exception);
        }
    }

    private static Dictionary<string, BridgeWatchLine[]> ReadWatchStore()
    {
        if (!File.Exists(WatchStorePath))
        {
            return new Dictionary<string, BridgeWatchLine[]>(StringComparer.OrdinalIgnoreCase);
        }

        var json = File.ReadAllText(WatchStorePath);
        return JsonSerializer.Deserialize<Dictionary<string, BridgeWatchLine[]>>(json, JsonOptions)
               ?? new Dictionary<string, BridgeWatchLine[]>(StringComparer.OrdinalIgnoreCase);
    }

    private void PersistWatch()
    {
        var instrument = InstrumentInfo?.Instrument;
        if (string.IsNullOrWhiteSpace(instrument))
        {
            return;
        }

        try
        {
            lock (WatchStoreSync)
            {
                var store = ReadWatchStore();
                if (_watch.Length == 0)
                {
                    store.Remove(instrument);
                }
                else
                {
                    store[instrument] = _watch;
                }

                File.WriteAllText(WatchStorePath, JsonSerializer.Serialize(store, JsonOptions));
            }
        }
        catch (Exception exception)
        {
            this.LogError("FofDataBridge could not save the watch panel.", exception);
        }
    }

    private async Task<object> WatchAsync(HttpListenerContext context)
    {
        var method = context.Request.HttpMethod.ToUpperInvariant();

        if (method is "DELETE")
        {
            _watch = Array.Empty<BridgeWatchLine>();
            _watchUpdatedUtc = DateTime.UtcNow;
            PersistWatch();
            Repaint();
            return WatchPayload();
        }

        if (method is "POST" or "PUT")
        {
            using var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new BridgeException(400, "empty body: send a JSON array of lines, or {\"lines\": [...]}");
            }

            BridgeWatchLine[]? parsed;
            try
            {
                var trimmed = body.TrimStart();
                parsed = trimmed.StartsWith('[')
                    ? JsonSerializer.Deserialize<BridgeWatchLine[]>(body, JsonOptions)
                    : JsonSerializer.Deserialize<WatchRequest>(body, JsonOptions)?.Lines;
            }
            catch (JsonException exception)
            {
                throw new BridgeException(400, $"malformed JSON: {exception.Message}");
            }

            if (parsed is null)
            {
                throw new BridgeException(400, "no lines in the request");
            }

            _watch = parsed;
            _watchUpdatedUtc = DateTime.UtcNow;
            PersistWatch();
            Repaint();
            return WatchPayload();
        }

        return WatchPayload();
    }

    private object WatchPayload() => new
    {
        schema = Schema,
        chart = _id,
        instrument = InstrumentInfo?.Instrument,
        updatedUtc = Iso(_watchUpdatedUtc),
        count = _watch.Length,
        lines = _watch.Select(l => new { text = l.Text, color = l.Color }),
    };

    /// <summary>
    /// Ridisegna subito invece di aspettare il prossimo evento del chart: chi deposita i livelli
    /// da riga di comando si aspetta di vederli comparire, non al primo movimento del mouse.
    /// Un fallimento qui non deve far fallire la richiesta HTTP.
    /// </summary>
    private void Repaint()
    {
        try
        {
            RedrawChart(new RedrawArg(ChartArea));
        }
        catch (Exception exception)
        {
            this.LogError("FofDataBridge could not request a redraw.", exception);
        }
    }

    private object LevelsPayload() => new
    {
        schema = Schema,
        chart = _id,
        instrument = InstrumentInfo?.Instrument,
        updatedUtc = _levelsUpdatedUtc == DateTime.MinValue
            ? null
            : _levelsUpdatedUtc.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
        count = _levels.Length,
        levels = _levels,
    };

    /// <summary>
    /// Colore di default e parsing di quello richiesto. Un valore illeggibile non fa fallire la
    /// richiesta: il livello viene disegnato con il colore di default, perche' perdere un livello
    /// per un colore sbagliato sarebbe peggio.
    /// </summary>
    private static Color ParseColor(string? raw)
    {
        var fallback = Color.FromArgb(220, 255, 196, 0);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return fallback;
        }

        var text = raw.Trim().TrimStart('#');
        if (uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            return text.Length switch
            {
                6 => Color.FromArgb(255, (byte)(value >> 16), (byte)(value >> 8), (byte)value),
                8 => Color.FromArgb((byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value),
                _ => fallback,
            };
        }

        try
        {
            var named = Color.FromName(raw.Trim());
            return named.IsKnownColor ? named : fallback;
        }
        catch
        {
            return fallback;
        }
    }

    // CA1416 segnala DashStyle come solo-Windows perche' vive in System.Drawing.Common. Qui e'
    // usato come semplice enum passato a RenderPen di ATAS, che su ATAS X lo risolve con il
    // proprio renderer: non si tocca GDI+ e non c'e' dipendenza dalla piattaforma.
#pragma warning disable CA1416
    private static DashStyle DashOf(string? style) => style?.Trim().ToLowerInvariant() switch
    {
        "dash" => DashStyle.Dash,
        "dot" => DashStyle.Dot,
        "dashdot" => DashStyle.DashDot,
        _ => DashStyle.Solid,
    };
#pragma warning restore CA1416

    /// <summary>
    /// Il bordo destro dei DATI, non del pannello: <c>ChartArea</c> arriva fino a includere la
    /// scala dei prezzi, e ancorare li' un'etichetta o un pannello li fa finire sotto i numeri
    /// dell'asse (o dietro la sidebar del prezzo). L'ultima barra visibile e' dentro l'area dei
    /// dati per costruzione, quindi la sua X e' un bordo sicuro qualunque sia la larghezza
    /// della scala. Usato sia dai livelli sia dal pannello: era duplicato e i due potevano
    /// divergere - il pannello lo aveva saltato ed e' finito dietro la sidebar (19 settembre).
    /// </summary>
    private int DataAreaRight(Rectangle area)
    {
        var dataRight = area.Right;
        try
        {
            var container = ChartInfo?.PriceChartContainer;
            if (container is not null)
            {
                var lastBarX = ChartInfo!.GetXByBar(container.LastVisibleBarNumber, false);
                if (lastBarX > area.Left)
                {
                    dataRight = Math.Min(dataRight, lastBarX);
                }
            }
        }
        catch
        {
            // Se il container non e' pronto si resta sul bordo dell'area: peggio l'etichetta
            // spostata che niente disegnato.
        }
        return dataRight;
    }

    /// <summary>
    /// Il tooltip dell'etichetta corta dipende dalla posizione del mouse, che ATAS non ridisegna
    /// da sola a ogni movimento se il chart non sta cambiando: senza questo, passare il mouse
    /// su una riga non mostrerebbe niente finche' non arriva un tick nuovo.
    /// </summary>
    public override bool ProcessMouseMove(RenderControlMouseEventArgs e)
    {
        if (ShowLevels && ShortLabels)
        {
            Repaint();
        }
        return base.ProcessMouseMove(e);
    }

    protected override void OnRender(RenderContext context, DrawingLayouts layout)
    {
        if (ChartInfo is null)
        {
            return;
        }

        RenderWatchPanel(context);

        var levels = _levels;
        if (!ShowLevels || levels.Length == 0)
        {
            return;
        }

        var area = ChartArea;
        var font = new RenderFont("Arial", LevelFontSize);

        // ChartArea arriva fino al bordo del pannello, scala dei prezzi compresa: ancorare
        // l'etichetta a area.Right la fa finire sotto i numeri dell'asse. L'ultima barra
        // visibile e' dentro l'area dei dati per costruzione, quindi la sua X e' un bordo
        // destro sicuro qualunque sia la larghezza dell'asse.
        var dataRight = DataAreaRight(area);

        var mouse = MouseLocationInfo is { IsMouseLeave: false } info ? info.LastPosition : (Point?)null;
        (string Text, Color Color, Point At)? tooltip = null;

        foreach (var level in levels)
        {
            var y = ChartInfo.GetYByPrice(level.Price, false);
            if (y < area.Top || y > area.Bottom)
            {
                continue;
            }

            var color = ParseColor(level.Color);
            var pen = new RenderPen(color, Math.Clamp(level.Width, 1, 5), DashOf(level.Style));
            // La riga si ferma a LevelLineExtent pixel dal bordo destro, non attraversa tutto il
            // chart: il 19 settembre le righe intere, insieme al pannello e alle candele, erano
            // troppa roba sullo schermo. La linea corta tiene il punto di riferimento vicino al
            // prezzo attuale, dove serve, senza tagliare la seduta intera in orizzontale.
            var lineLeft = Math.Max(area.Left, dataRight - LevelLineExtent);
            context.DrawLine(pen, lineLeft, y, dataRight, y);

            var priceText = level.Price.ToString("0.##", CultureInfo.InvariantCulture);
            var fullText = string.IsNullOrWhiteSpace(level.Label) ? priceText : $"{level.Label}  {priceText}";
            // La parte prima di ' · ' e' la convenzione del metodo per il nome del livello: il
            // resto e' la misura che lo sostiene (vedi livelli-sul-chart.md). E' quella la riga
            // che serve per riconoscerlo al volo; il resto si legge passando il mouse, non prima.
            var shortText = ShortLabels && level.Label is { } label
                ? $"{label.Split(" · ", 2)[0]}  {priceText}"
                : fullText;

            var size = context.MeasureString(shortText, font);
            var x = LabelOnRight
                ? Math.Max(area.Left + LabelMargin, dataRight - size.Width - LabelMargin)
                : area.Left + LabelMargin;

            // Fondo pieno dietro l'etichetta: sopra un footprint denso il testo nudo e' illeggibile.
            var labelBox = new Rectangle(x - 3, y - size.Height - 2, size.Width + 6, size.Height + 2);
            context.FillRectangle(Color.FromArgb(190, 0, 0, 0), labelBox);
            context.DrawString(shortText, font, color, x, y - size.Height - 1);

            if (ShortLabels && shortText != fullText && mouse is { } m)
            {
                var lineBand = new Rectangle(lineLeft, y - 4, dataRight - lineLeft, 8);
                if (labelBox.Contains(m) || lineBand.Contains(m))
                {
                    tooltip = (fullText, color, m);
                }
            }
        }

        if (tooltip is { } t)
        {
            var size = context.MeasureString(t.Text, font);
            const int pad = 6;
            var boxX = Math.Min(t.At.X + 14, dataRight - size.Width - pad * 2);
            var boxY = t.At.Y - size.Height - pad * 2 - 4;
            context.FillRectangle(Color.FromArgb(235, 15, 15, 15),
                new Rectangle(boxX, boxY, size.Width + pad * 2, size.Height + pad * 2));
            context.DrawString(t.Text, font, t.Color, boxX + pad, boxY + pad);
        }
    }

    /// <summary>
    /// Disegna il pannello in un angolo fisso dello schermo, non sul prezzo: le righe restano
    /// leggibili qualunque candela sia in vista, e si aggiornano da sole a ogni chiamata di
    /// OnRender - che ATAS invoca a ogni tick - senza bisogno di un nuovo POST per ogni ridisegno.
    /// Il contenuto cambia solo quando l'analisi esterna deposita nuove righe: il refresh visivo
    /// e' gratis, il refresh del DATO dipende da chi scrive su /watch.
    /// </summary>
    private void RenderWatchPanel(RenderContext context)
    {
        var lines = _watch;
        if (!ShowWatch || lines.Length == 0)
        {
            return;
        }

        var area = ChartArea;
        var dataRight = DataAreaRight(area);
        var font = new RenderFont("Arial", WatchFontSize);

        var widest = 0;
        var lineHeight = 0;
        var sizes = new Size[lines.Length];
        for (var i = 0; i < lines.Length; i++)
        {
            sizes[i] = context.MeasureString(lines[i].Text, font);
            widest = Math.Max(widest, sizes[i].Width);
            lineHeight = Math.Max(lineHeight, sizes[i].Height);
        }

        const int padding = 8;
        const int lineGap = 3;
        var boxWidth = widest + padding * 2;
        var boxHeight = lines.Length * (lineHeight + lineGap) - lineGap + padding * 2;
        var x = Math.Max(area.Left, dataRight - WatchMargin - boxWidth);
        var y = area.Top + WatchMargin;

        context.FillRectangle(Color.FromArgb(200, 20, 20, 20),
            new Rectangle(x, y, boxWidth, boxHeight));

        var textY = y + padding;
        foreach (var (line, size) in lines.Zip(sizes))
        {
            var color = ParseColor(line.Color);
            context.DrawString(line.Text, font, color, x + padding, textY);
            textY += size.Height + lineGap;
        }
    }

    // ---------------------------------------------------------------- endpoints

    private object Health() => new
    {
        schema = Schema,
        instrument = InstrumentInfo?.Instrument,
        timeFrame = ChartInfo?.TimeFrame,
        chartType = ChartInfo?.ChartType,
        bars = CurrentBar,
        marketTimeUtc = Iso(UtcTime),
        chart = _id,
        port = _hubPort,
        charts = Instances.Count,
        endpoints = new[]
        {
            "/health", "/charts", "/instrument", "/limits", "/session", "/rollovers",
            "/profile", "/candles", "/cumulative", "/depth",
        },
    };

    private object InstrumentPayload()
    {
        var security = TradingManager?.Security;
        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            exchange = InstrumentInfo?.Exchange,
            tickSize = InstrumentInfo?.TickSize,
            timeZoneOffsetMinutes = InstrumentInfo?.TimeZoneOffset.TotalMinutes,
            timeFrame = ChartInfo?.TimeFrame,
            chartType = ChartInfo?.ChartType,
            bars = CurrentBar,
            marketTimeUtc = Iso(UtcTime),
            security = security is null
                ? null
                : new
                {
                    code = security.Code,
                    securityId = security.SecurityId,
                    type = security.Type.ToString(),
                    expiration = Iso(security.Expiration),
                    expirationMoment = security.ExpirationMoment is null ? null : Iso(security.ExpirationMoment.Value),
                    lotSize = security.LotSize,
                    tickCost = security.TickCost,
                    priceMultiplier = security.PriceMultiplier,
                    volumeMultiplier = security.VolumeMultiplier,
                    openInterest = security.OpenInterest,
                    bestBidPrice = security.BestBidPrice,
                    bestAskPrice = security.BestAskPrice,
                },
        };
    }

    /// <summary>
    /// Espone i limiti che ATAS applica alle richieste storiche, invece di lasciarli dedurre
    /// per tentativi come e' successo con le prime versioni del recorder storico.
    /// </summary>
    private object Limits()
    {
        var provider = OnlineDataProvider();
        var modes = Enum.GetValues<CumulativeTradesMode>().Select(mode => new
        {
            mode = mode.ToString(),
            maxDepthDays = Safe(() => provider.GetCumulativeTradesMaxDepth(mode).TotalDays),
            sessionLimit = Safe(() => (double?)provider.GetCumulativeTradesSessionLimit(mode)),
        });

        return new
        {
            schema = Schema,
            maxItemsPerResponse = MaxItems,
            cumulativeTrades = modes,
            fixedProfilePeriods = Enum.GetNames<FixedProfilePeriods>(),
            contractRolloverTypes = Enum.GetNames<ContractRolloverType>(),
        };
    }

    private object Session(System.Collections.Specialized.NameValueCollection query)
    {
        var at = OptionalTime(query, "at") ?? UtcTime;
        var working = OnlineDataProvider().GetSessionWorkingTime(at);
        return new
        {
            schema = Schema,
            atUtc = Iso(at),
            session = working is null ? null : new { from = Iso(working.Value.From), to = Iso(working.Value.To) },
        };
    }

    private async Task<object> RolloversAsync(
        System.Collections.Specialized.NameValueCollection query,
        CancellationToken cancellation)
    {
        var from = RequiredTime(query, "from");
        var to = RequiredTime(query, "to");
        var type = OptionalEnum<ContractRolloverType>(query, "type") ?? ContractRolloverType.VolumeBasedCurrentEnd;

        var rollovers = await OnlineDataProvider()
            .GetContractRolloversAsync(from, to, type, cancellation)
            .ConfigureAwait(false);

        return new
        {
            schema = Schema,
            type = rollovers.Type.ToString(),
            fromUtc = Iso(from),
            toUtc = Iso(to),
            rollovers = (rollovers.Rollovers ?? [])
                .Select(rollover => new { code = rollover.Code, date = Iso(rollover.Date) }),
        };
    }

    /// <summary>
    /// Profilo fisso nativo di ATAS. `session` accetta l'identificatore di sessione della
    /// piattaforma: dichiararlo esplicitamente evita l'ambiguita' fra profilo di sessione
    /// cash e profilo dell'intera giornata.
    /// </summary>
    private async Task<object> ProfileAsync(System.Collections.Specialized.NameValueCollection query)
    {
        var period = OptionalEnum<FixedProfilePeriods>(query, "period") ?? FixedProfilePeriods.CurrentDay;
        var session = OptionalLong(query, "session");
        var withLevels = OptionalBool(query, "levels") ?? true;

        // La build ATAS X espone solo (period) e (period, tradingSession): non esiste
        // l'overload con baseTime documentato per ATAS classico, quindi il profilo e'
        // sempre relativo al market time corrente.
        var response = await RequestFixedProfileAsync(new FixedProfileRequest(period, session))
            .ConfigureAwait(false);
        if (response is null)
        {
            throw new BridgeException(503, "ATAS returned no fixed profile for this request");
        }

        return new
        {
            schema = Schema,
            period = period.ToString(),
            tradingSession = session,
            instrument = InstrumentInfo?.Instrument,
            scaled = Describe(response.Value.Scaled, withLevels),
            original = Describe(response.Value.Original, withLevels),
        };
    }

    /// <summary>
    /// Candele del chart su cui il bridge e' caricato, con footprint opzionale. Il range puo'
    /// essere espresso per tempo (`from`/`to`) oppure per indice di barra (`fromBar`/`toBar`).
    /// </summary>
    private object Candles(System.Collections.Specialized.NameValueCollection query)
    {
        var withLevels = OptionalBool(query, "levels") ?? false;
        var from = OptionalTime(query, "from");
        var to = OptionalTime(query, "to");
        var first = OptionalInt(query, "fromBar") ?? 0;
        var last = OptionalInt(query, "toBar") ?? CurrentBar - 1;

        first = Math.Max(0, first);
        last = Math.Min(CurrentBar - 1, last);

        var candles = new List<object>();
        for (var bar = first; bar <= last && candles.Count < MaxItems; bar++)
        {
            var candle = GetCandle(bar);
            if (candle is null)
            {
                continue;
            }

            if (from is not null && candle.LastTime < from.Value)
            {
                continue;
            }

            if (to is not null && candle.Time > to.Value)
            {
                break;
            }

            var described = Describe(candle, withLevels, bar);
            if (described is not null)
            {
                candles.Add(described);
            }
        }

        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            timeFrame = ChartInfo?.TimeFrame,
            chartType = ChartInfo?.ChartType,
            totalBars = CurrentBar,
            truncated = candles.Count >= MaxItems,
            count = candles.Count,
            candles,
        };
    }

    /// <summary>
    /// Trade aggregati storici. `minVolume` e `maxVolume` sono il filtro nativo di ATAS: e'
    /// il modo per isolare i trade di taglia senza ricostruirli a valle.
    /// </summary>
    private async Task<object> CumulativeAsync(
        System.Collections.Specialized.NameValueCollection query,
        CancellationToken cancellation)
    {
        var from = RequiredTime(query, "from");
        var to = RequiredTime(query, "to");
        if (to <= from)
        {
            throw new BridgeException(400, "'to' must be later than 'from'");
        }

        var minVolume = OptionalInt(query, "minVolume") ?? 0;
        var maxVolume = OptionalInt(query, "maxVolume") ?? 0;
        var mode = OptionalEnum<CumulativeTradesMode>(query, "mode") ?? CumulativeTradesMode.Filter;
        var withTicks = OptionalBool(query, "ticks") ?? false;

        var request = new CumulativeTradesRequest(from, to, mode, minVolume, maxVolume);

        var trades = await RequestCumulativeAsync(request, cancellation).ConfigureAwait(false);

        // ATAS puo' restituire record fuori dalla finestra richiesta: vengono contati e scartati,
        // come gia' stabilito dalla versione v5 del recorder storico.
        var outside = 0;
        var inside = new List<CumulativeTrade>(trades.Count);
        foreach (var trade in trades)
        {
            if (trade.Time < from || trade.Time > to)
            {
                outside++;
            }
            else
            {
                inside.Add(trade);
            }
        }

        var truncated = inside.Count > MaxItems;
        var emitted = truncated ? inside.Take(MaxItems) : inside;

        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            fromUtc = Iso(from),
            toUtc = Iso(to),
            mode = mode.ToString(),
            minVolume,
            maxVolume,
            returned = trades.Count,
            outsideWindow = outside,
            count = Math.Min(inside.Count, MaxItems),
            truncated,
            trades = emitted.Select(trade => new
            {
                time = Iso(trade.Time),
                direction = trade.Direction.ToString(),
                volume = trade.Volume,
                firstPrice = trade.FirstPrice,
                lastPrice = trade.Lastprice,
                tickCount = trade.Ticks?.Count ?? 0,
                previousBid = trade.PreviousBid?.Price,
                previousAsk = trade.PreviousAsk?.Price,
                newBid = trade.NewBid?.Price,
                newAsk = trade.NewAsk?.Price,
                ticks = withTicks
                    ? trade.Ticks?.Select(tick => new
                    {
                        time = Iso(tick.Time),
                        price = tick.Price,
                        volume = tick.Volume,
                        direction = tick.Direction.ToString(),
                    })
                    : null,
            }),
        };
    }

    private async Task<List<CumulativeTrade>> RequestCumulativeAsync(
        CumulativeTradesRequest request,
        CancellationToken cancellation)
    {
        await _cumulativeGate.WaitAsync(cancellation).ConfigureAwait(false);
        try
        {
            var completion = new TaskCompletionSource<List<CumulativeTrade>>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            lock (_sync)
            {
                _pendingCumulative = completion;
                _pendingCumulativeRequestId = request.RequestId;
            }

            RequestForCumulativeTrades(request);

            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellation, _shutdown.Token);
            timeout.CancelAfter(TimeSpan.FromMinutes(5));
            using (timeout.Token.Register(() => completion.TrySetCanceled()))
            {
                try
                {
                    return await completion.Task.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw new BridgeException(504, "ATAS did not answer the cumulative trades request in time");
                }
            }
        }
        finally
        {
            lock (_sync)
            {
                _pendingCumulative = null;
            }

            _cumulativeGate.Release();
        }
    }

    protected override void OnCumulativeTradesResponse(
        CumulativeTradesRequest request,
        IEnumerable<CumulativeTrade> cumulativeTrades)
    {
        TaskCompletionSource<List<CumulativeTrade>>? completion;
        lock (_sync)
        {
            completion = _pendingCumulative;
            if (completion is not null && _pendingCumulativeRequestId != request.RequestId)
            {
                completion = null;
            }
        }

        completion?.TrySetResult(cumulativeTrades?.ToList() ?? []);
        base.OnCumulativeTradesResponse(request, cumulativeTrades ?? []);
    }

    private async Task<object> DepthAsync(
        System.Collections.Specialized.NameValueCollection query,
        CancellationToken cancellation)
    {
        var from = RequiredTime(query, "from");
        var to = RequiredTime(query, "to");
        var period = TimeSpan.FromSeconds(OptionalInt(query, "periodSeconds") ?? 60);

        var request = new MarketDepthSnapshotRequest { From = from, To = to, Period = period };
        var snapshots = await OnlineDataProvider()
            .GetMarketDepthSnapshotsAsync(request, cancellation)
            .ConfigureAwait(false);

        var list = snapshots?.Take(MaxItems).ToList() ?? [];
        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            fromUtc = Iso(from),
            toUtc = Iso(to),
            periodSeconds = period.TotalSeconds,
            count = list.Count,
            snapshots = list.Select(snapshot => new
            {
                time = Iso(snapshot.StartTime),
                bids = snapshot.Bids?.Select(level => new { price = level.Price, volume = level.Volume }),
                asks = snapshot.Asks?.Select(level => new { price = level.Price, volume = level.Volume }),
            }),
        };
    }

    // ---------------------------------------------------------------- conversione

    private static object? Describe(IndicatorCandle? candle, bool withLevels, int? bar = null)
    {
        if (candle is null)
        {
            return null;
        }

        var valueArea = candle.ValueArea;
        return new
        {
            bar,
            time = Iso(candle.Time),
            lastTime = Iso(candle.LastTime),
            open = candle.Open,
            high = candle.High,
            low = candle.Low,
            close = candle.Close,
            volume = candle.Volume,
            ticks = candle.Ticks,
            bid = candle.Bid,
            ask = candle.Ask,
            betweens = candle.Betweens,
            delta = candle.Delta,
            maxDelta = candle.MaxDelta,
            minDelta = candle.MinDelta,
            vwap = candle.VWAP,
            openInterest = candle.OI,
            maxOpenInterest = candle.MaxOI,
            minOpenInterest = candle.MinOI,
            valueAreaHigh = valueArea?.ValueAreaHigh,
            valueAreaLow = valueArea?.ValueAreaLow,
            poc = Describe(candle.MaxVolumePriceInfo),
            maxTick = Describe(candle.MaxTickPriceInfo),
            maxBid = Describe(candle.MaxBidPriceInfo),
            maxAsk = Describe(candle.MaxAskPriceInfo),
            maxPositiveDelta = Describe(candle.MaxPositiveDeltaPriceInfo),
            maxNegativeDelta = Describe(candle.MaxNegativeDeltaPriceInfo),
            levels = withLevels
                ? candle.GetAllPriceLevels().Select(Describe).ToList()
                : null,
        };
    }

    private static object? Describe(PriceVolumeInfo? level) => level is null
        ? null
        : new
        {
            price = level.Price,
            volume = level.Volume,
            bid = level.Bid,
            ask = level.Ask,
            between = level.Between,
            ticks = level.Ticks,
            time = level.Time,
        };

    // ---------------------------------------------------------------- utilita'

    private IOnlineDataProvider OnlineDataProvider()
        => DataProvider?.OnlineDataProvider
           ?? throw new BridgeException(503, "online data provider is not available on this chart");

    private static double? Safe(Func<double?> read)
    {
        try
        {
            return read();
        }
        catch
        {
            return null;
        }
    }

    private static string Iso(DateTime value)
        => DateTime.SpecifyKind(value, value.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : value.Kind)
            .ToUniversalTime()
            .ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

    private static DateTime RequiredTime(System.Collections.Specialized.NameValueCollection query, string key)
        => OptionalTime(query, key) ?? throw new BridgeException(400, $"missing required parameter '{key}'");

    private static DateTime? OptionalTime(System.Collections.Specialized.NameValueCollection query, string key)
    {
        var raw = query[key];
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        if (!DateTime.TryParse(raw, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var parsed))
        {
            throw new BridgeException(400, $"parameter '{key}' is not a valid date-time");
        }

        return DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
    }

    private static int? OptionalInt(System.Collections.Specialized.NameValueCollection query, string key)
    {
        var raw = query[key];
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        if (!int.TryParse(raw, out var parsed))
        {
            throw new BridgeException(400, $"parameter '{key}' is not an integer");
        }

        return parsed;
    }

    private static long? OptionalLong(System.Collections.Specialized.NameValueCollection query, string key)
    {
        var raw = query[key];
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        if (!long.TryParse(raw, out var parsed))
        {
            throw new BridgeException(400, $"parameter '{key}' is not an integer");
        }

        return parsed;
    }

    private static bool? OptionalBool(System.Collections.Specialized.NameValueCollection query, string key)
    {
        var raw = query[key];
        return string.IsNullOrWhiteSpace(raw) ? null : raw is "1" or "true" or "yes";
    }

    private static TEnum? OptionalEnum<TEnum>(System.Collections.Specialized.NameValueCollection query, string key)
        where TEnum : struct, Enum
    {
        var raw = query[key];
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        if (!Enum.TryParse<TEnum>(raw, ignoreCase: true, out var parsed))
        {
            throw new BridgeException(
                400,
                $"parameter '{key}' must be one of: {string.Join(", ", Enum.GetNames<TEnum>())}");
        }

        return parsed;
    }

    private sealed class BridgeException(int status, string message) : Exception(message)
    {
        public int Status { get; } = status;
    }
}
