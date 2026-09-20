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
public sealed partial class DataBridge : Indicator
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

    // Due distanze separate, non una sola: incollato all'angolo il pannello finisce sotto la
    // barra degli strumenti del chart e sopra i numeri dell'asse, ed e' proprio dove si guarda
    // di meno. Staccarlo un po' lo mette in mezzo al campo visivo, che e' lo scopo.
    [Display(Name = "Offset from top", GroupName = "Watch",
        Description = "Distanza dal bordo superiore dell'area dati, in pixel.")]
    [Range(0, 600)]
    public int WatchOffsetTop { get; set; } = 56;

    [Display(Name = "Offset from right", GroupName = "Watch",
        Description = "Distanza dal bordo destro dell'area dati, in pixel. Alzalo per spostare " +
                      "il pannello verso il centro.")]
    [Range(0, 900)]
    public int WatchOffsetRight { get; set; } = 110;

    [Display(Name = "Show value band", GroupName = "Levels",
        Description = "Dipinge la fascia fra VAL e VAH. Due righe dicono dove sono i bordi, " +
                      "non che in mezzo c'e' un dentro - ed e' il dentro che sceglie il modello.")]
    public bool ShowValueBand { get; set; } = true;

    [Display(Name = "Value band opacity", GroupName = "Levels",
        Description = "Quanto e' marcata la fascia del valore, su 255. Alta copre le candele.")]
    [Range(0, 120)]
    public int ValueBandOpacity { get; set; } = 18;

    [Display(Name = "In-play radius", GroupName = "Watch",
        Description = "Entro quanti punti dal prezzo un livello si considera IN GIOCO. " +
                      "Oltre, il pannello dice che non c'e' niente in gioco e mostra le due porte.")]
    [Range(1, 500)]
    public int InPlayRadius { get; set; } = 12;

    [Display(Name = "Lookback bars", GroupName = "Watch",
        Description = "Quante barre indietro si misura cio' che e' stato scambiato AL livello.")]
    [Range(2, 500)]
    public int PanelLookback { get; set; } = 10;

    [Display(Name = "Lookback largo", GroupName = "Watch",
        Description = "La seconda finestra del delta, in barre. Serve a dare una scala al delta " +
                      "misurato al livello: lo stesso conto su tutte le barre, non solo li'.")]
    [Range(5, 1000)]
    public int PanelLookbackLargo { get; set; } = 30;

    [Display(Name = "Apertura cash (UTC)", GroupName = "Watch",
        Description = "L'ora UTC da cui il pannello somma il delta di seduta. Su NQ e' 13:30Z " +
                      "(15:30 italiane in ora legale). Non tocca i livelli: solo il pannello.")]
    public string AperturaCash { get; set; } = "13:30Z";

    [Display(Name = "Price axis padding", GroupName = "Levels",
        Description = "Pixel in piu' oltre la larghezza misurata della scala dei prezzi. " +
                      "Alzalo se etichette o pannello finiscono ancora sopra i numeri dell'asse.")]
    [Range(0, 200)]
    public int PriceAxisPadding { get; set; } = 10;

    /// <summary>
    /// L'orologio del motore dei livelli: l'indice dell'ultima barra su cui si e' ricalcolato.
    /// </summary>
    private int _ultimaBarraVista = -1;

    protected override void OnCalculate(int bar, decimal value)
    {
        // IL RICALCOLO STA QUI, E NON IN UN PROCESSO ESTERNO. Una barra nuova e' l'unico momento
        // in cui un POC, un bordo del valore o un estremo possono essere cambiati: dentro la
        // barra in formazione le misure non si prendono affatto, perche' cambierebbero da sole
        // fra un tick e il successivo.
        //
        // Il filtro sull'ultima barra e' necessario: senza, il caricamento storico chiamerebbe
        // OnCalculate settemila volte e ricalcolerebbe settemila profili all'apertura del chart.
        if (bar < CurrentBar - 1 || bar <= _ultimaBarraVista)
        {
            return;
        }

        _ultimaBarraVista = bar;

        // LA SEMINA DEL TAPE STA QUI E NON IN OnInitialize, per una ragione sola: in
        // OnInitialize le barre non ci sono ancora, e la richiesta storica avrebbe bisogno di
        // un istante di mercato che nessuno sa ancora qual e'. Alla prima barra utile invece
        // l'orologio del chart esiste - ed e' quello del replay, non quello di casa.
        var quando = GetCandle(bar)?.Time;
        if (quando is not null)
        {
            _ = SeminaIlTapeAsync(quando.Value);
        }

        if (_rules.Length > 0)
        {
            RisolviRegole();
            Repaint();
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Register();
        RestoreLevels();
        RestoreRules();
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
                "/rules" => await RulesAsync(context).ConfigureAwait(false),
                "/regime" => Regime(),
                "/panel" => Pannello(),
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

        /// <summary>poc | vah | val | massimo | minimo | nodo_top | nodo_base | fisso.</summary>
        public string? Role { get; init; }

        /// <summary>Il nome dell'area di valore a cui il bordo appartiene: "Europa", "cash", "Asia".</summary>
        public string? Area { get; init; }

        /// <summary>Vero se il livello conta per la strategia; il contesto si disegna piu' spento.</summary>
        public bool Key { get; init; }

        /// <summary>Cosa dovrebbe essere vero perche' il livello diventi operabile.</summary>
        public BridgeCondition[]? Conditions { get; init; }

        /// <summary>A cosa servono quelle condizioni: quale setup, in che verso, verso dove.</summary>
        public BridgeScenario? Scenario { get; init; }

        /// <summary>
        /// Il nome della regola che ha prodotto il livello. Serve a due cose che dal prezzo non
        /// si deducono: risolvere un bersaglio dichiarato per nome, e dire nel pannello quale
        /// regola non ha prodotto niente.
        /// </summary>
        public string? Nome { get; init; }

        /// <summary>
        /// <c>~</c> misurato su finestra aperta, <c>=</c> misurato su finestra chiusa,
        /// <c>*</c> dichiarato a mano dall'analisi. Vedi RegoleDeiLivelli.cs.
        /// </summary>
        public string? Marcatore { get; init; }
    }

    /// <summary>
    /// Lo sviluppo a cui le condizioni di un livello servono.
    ///
    /// Senza, una lista di prerequisiti spuntati non dice **per cosa**: un livello non e' mai il
    /// fine, e' una porta, e bisogna dire a cosa serve attraversarla. Lo dichiara l'analisi nel
    /// file delle regole; la macchina lo mostra e basta, e non lo verifica.
    /// </summary>
    private sealed record BridgeScenario
    {
        /// <summary>LONG | SHORT | NIENTE.</summary>
        public string? Direzione { get; init; }

        /// <summary>Come si chiama il setup: "fade del bordo alto verso il POC".</summary>
        public string? Nome { get; init; }

        /// <summary>Dove si va se funziona. Risolto da un nome di livello, non scritto a mano.</summary>
        public decimal? Bersaglio { get; init; }

        /// <summary>Il prezzo che smonta la lettura.</summary>
        public decimal? Invalida { get; init; }

        /// <summary>
        /// Dove si porta lo stop a pareggio: il prezzo oltre il quale il lato opposto tornerebbe
        /// a vincere. Nel live Q1 la gestione e' l'edge, e questo ne e' il numero centrale —
        /// <i>"break even it's a free attempt, so you don't risk anything"</i> <c>[3 · 14:42]</c>.
        /// </summary>
        public decimal? Pareggio { get; init; }
    }

    /// <summary>
    /// Una condizione **dichiarata dall'analisi e spuntata dalla macchina**.
    ///
    /// Non e' una condizione armata: non scatta, non avvisa, non fa niente. Il vecchio impianto
    /// valutava condizioni e gridava, e per armarne una servivano sette prove; qui la macchina
    /// misura soltanto, e chi guarda vede quali prerequisiti sono gia' soddisfatti.
    ///
    /// `Testo` sono le parole dell'analisi e viene mostrato com'e': la macchina non lo interpreta.
    /// </summary>
    private sealed record BridgeCondition
    {
        /// <summary>chiusura | delta | volume | arrivo</summary>
        public string? Cosa { get; init; }

        /// <summary>Per <c>chiusura</c>: sopra | sotto. Per <c>arrivo</c>: SOPRA | SOTTO.</summary>
        public string? Verso { get; init; }

        /// <summary>Per <c>chiusura</c>: il prezzo da superare. Omesso, e' il livello stesso.</summary>
        public decimal? Prezzo { get; init; }

        /// <summary>Per <c>delta</c> e <c>volume</c>: la soglia, misurata AL LIVELLO.</summary>
        public decimal? Almeno { get; init; }

        /// <summary>Le parole dell'analisi, mostrate cosi' come sono.</summary>
        public string? Testo { get; init; }
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

            // DEPOSITARE LIVELLI A MANO SPEGNE LE REGOLE, ed e' esplicito apposta.
            //
            // Le due cose scrivono nello stesso posto: se restassero accese entrambe, alla
            // prima barra nuova il motore ricalcolerebbe e i livelli appena depositati
            // sparirebbero senza che nessuno abbia sbagliato niente. Meglio dire che il
            // controllo e' passato a mano, che vederlo tornare indietro da solo.
            if (_rules.Length > 0)
            {
                _rules = Array.Empty<BridgeRule>();
                _regoleSaltate = new[] { "regole spente: livelli depositati a mano su /levels" };
                _regoleAllaBarra = -1;
                PersistRules();
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
    /// <summary>
    /// Il bordo destro dell'area dei dati: dove finisce il disegno e comincia la scala dei prezzi.
    /// Serve a righe, etichette e pannello, e deve avere **due** proprieta' insieme, che il
    /// 20 settembre 2026 sono state sbagliate una per volta:
    ///
    ///   fisso          ancorarlo all'ultima barra visibile fa scorrere righe e pannello ogni
    ///                  volta che si trascina il grafico, e il pannello lascia il suo angolo.
    ///   dentro i dati  `ChartArea` e `ChartContainer.Region` arrivano **oltre** l'asse dei
    ///                  prezzi: ancorarsi al loro bordo mette le etichette sopra i numeri.
    ///
    /// L'API di ATAS X non espone la larghezza dell'asse da nessuna parte - verificato con
    /// reflection su C:\ATASX: ne' IChart, ne' IChartContainer, ne' IIndicatorContainer. Si
    /// misura quindi il testo del prezzo col font dell'asse, che e' esattamente cio' che l'asse
    /// disegna. Dipende dall'ordine di grandezza del prezzo, non da quante barre si vedono:
    /// resta fermo mentre si trascina, che e' la proprieta' che serve.
    /// </summary>
    private int DataAreaRight(RenderContext context, Rectangle area)
    {
        try
        {
            if (ChartInfo is { HidePriceAxis: false } chart)
            {
                // Il massimo visibile e' il prezzo piu' largo che l'asse dovra' scrivere.
                var widest = chart.PriceChartContainer?.High ?? 0m;
                var text = widest.ToString(
                    string.IsNullOrWhiteSpace(chart.StringFormat) ? "0.##" : chart.StringFormat,
                    CultureInfo.InvariantCulture);
                var axis = context.MeasureString(text, chart.PriceAxisFont).Width + PriceAxisPadding;
                var right = area.Right - axis;
                if (right > area.Left)
                {
                    return right;
                }
            }
        }
        catch
        {
            // Se il font o il container non sono pronti si resta sul bordo dell'area: peggio
            // l'etichetta spostata che niente disegnato.
        }
        return area.Right;
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

        // IL PANNELLO NON PUO' PORTARSI VIA I LIVELLI, e questa non e' prudenza generica: e'
        // successo. Una lettura fuori intervallo dentro una misura del pannello ha svuotato il
        // chart per intero, e dal di fuori era indistinguibile da un motore che aveva smesso di
        // funzionare - mentre il motore stava risolvendo dodici livelli a ogni barra.
        //
        // I livelli sono il dato su cui si decide; il pannello e' il commento. Se il commento si
        // rompe, il dato resta, e il guasto si DICHIARA sul chart invece di presentarsi come
        // assenza. Un chart vuoto non dice se manca il dato o manca il disegno.
        try
        {
            RenderWatchPanel(context);
        }
        catch (Exception errore)
        {
            this.LogError("pannello non disegnato", errore);
            try
            {
                context.DrawString(
                    $"PANNELLO IN ERRORE: {errore.GetType().Name}. I livelli sono veri, il pannello no.",
                    new RenderFont("Arial", LevelFontSize), PanelAmbra,
                    ChartArea.Left + 8, ChartArea.Top + 8);
            }
            catch (Exception)
            {
                // Se non si riesce nemmeno a scrivere l'avviso, si tace e si disegnano i livelli:
                // sono loro la cosa che serve.
            }
        }

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
        var dataRight = DataAreaRight(context, area);

        DisegnaBandaValore(context, area, levels);

        var mouse = MouseLocationInfo is { IsMouseLeave: false } info ? info.LastPosition : (Point?)null;
        (string Text, Color Color, Point At)? tooltip = null;

        foreach (var level in levels)
        {
            var y = ChartInfo.GetYByPrice(level.Price, false);
            if (y < area.Top || y > area.Bottom)
            {
                continue;
            }

            // Cio' che conta per la strategia si disegna pieno, il contesto piu' spento. Senza
            // questa distinzione dodici righe hanno tutte lo stesso peso visivo, e quella su cui
            // si decide non si trova a colpo d'occhio - che e' l'unico momento in cui serve.
            var color = ParseColor(level.Color);
            if (!level.Key)
            {
                color = Color.FromArgb(120, color.R, color.G, color.B);
            }
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
    /// Il pannello: **una cosa sola, quella su cui si decide adesso**.
    ///
    /// Non e' un riassunto della seduta. Un pannello che elenca tutto costringe a cercare, e si
    /// cerca male proprio quando il prezzo si muove. Qui c'e' il livello **in gioco** - quello
    /// entro `InPlayRadius` punti - e le tre cose che servono a giudicarlo:
    ///
    ///   il lato di arrivo   e' cio' che separa un setup dal suo sosia. Un rifiuto del bordo alto
    ///                       e una rottura dello stesso bordo dall'alto hanno gli stessi numeri:
    ///                       li distingue **solo** da che parte arriva il prezzo.
    ///   cosa e' passato LI'  non nella barra e non nella seduta: **a quel prezzo**, sommando la
    ///                       footprint delle ultime `PanelLookback` barre. Sforzo alto e risultato
    ///                       nullo e' la misura dell'assorbimento.
    ///   come ha retto       quante volte e' stato toccato, e quante volte il prezzo e' tornato
    ///                       indietro invece di passare.
    ///
    /// **Ogni blocco dichiara su cosa e' misurato, in testata.** `AL LIVELLO` e' la footprint
    /// dentro una fascia di due tick; `SU TUTTE LE BARRE` e' il delta delle barre intere, su una
    /// finestra larga e dalla cash. Sono popolazioni diverse e finche' si chiamavano tutte
    /// "delta" il pannello invitava a confrontarle: un delta di fascia si legge contro il delta
    /// di seduta, non si somma con lui.
    ///
    /// **Il delta si stampa sempre come quota del volume.** Un numero assoluto non dice se e'
    /// tanto; la percentuale si', e non richiede di sapere a memoria quanto scambia NQ.
    ///
    /// Si ridisegna a **ogni tick**, perche' e' l'indicatore a calcolarlo: non dipende da nessun
    /// processo esterno, e non puo' restare fermo a mentire mentre il mercato si muove.
    ///
    /// Le righe depositate su `/watch` restano, sotto: sono cio' che scrive l'analisi, e vanno
    /// distinte da cio' che misura la macchina.
    /// </summary>
    private void RenderWatchPanel(RenderContext context)
    {
        if (!ShowWatch)
        {
            return;
        }

        var righe = ComponiPannello();
        foreach (var l in _watch)
        {
            righe.Add(new RigaPannello(l.Text ?? string.Empty, ParseColor(l.Color)));
        }
        if (righe.Count == 0)
        {
            return;
        }

        var area = ChartArea;
        var dataRight = DataAreaRight(context, area);
        var font = new RenderFont("Arial", WatchFontSize);

        var widest = 0;
        var sizes = new Size[righe.Count];
        for (var i = 0; i < righe.Count; i++)
        {
            sizes[i] = context.MeasureString(righe[i].Testo, font);
            widest = Math.Max(widest, sizes[i].Width);
        }

        const int padding = 8;
        const int lineGap = 3;
        var boxWidth = widest + padding * 2;
        var boxHeight = sizes.Sum(z => z.Height + lineGap) - lineGap + padding * 2;
        var x = Math.Max(area.Left, dataRight - WatchOffsetRight - boxWidth);
        var y = area.Top + WatchOffsetTop;

        context.FillRectangle(Color.FromArgb(200, 20, 20, 20),
            new Rectangle(x, y, boxWidth, boxHeight));

        var textY = y + padding;
        for (var i = 0; i < righe.Count; i++)
        {
            context.DrawString(righe[i].Testo, font, righe[i].Colore, x + padding, textY);
            textY += sizes[i].Height + lineGap;
        }
    }

    /// <summary>
    /// Un'area di valore ricomposta dai bordi depositati: e' la coppia VAL/VAH che porta la
    /// stessa <c>area</c>. Si raggruppa per area e non per vicinanza, perche' i bordi di due
    /// sessioni diverse possono stare a pochi punti e non sono bordi della stessa cosa.
    /// </summary>
    private sealed class AreaValore
    {
        public string Nome = string.Empty;
        public decimal? Val;
        public decimal? Vah;
        public decimal? Poc;
        public bool Chiave;
        public bool Completa => Val is not null && Vah is not null;
    }

    private List<AreaValore> AreeDiValore(BridgeLevel[] livelli)
    {
        var mappa = new Dictionary<string, AreaValore>(StringComparer.OrdinalIgnoreCase);
        foreach (var l in livelli)
        {
            var ruolo = l.Role ?? string.Empty;
            if (ruolo is not ("val" or "vah" or "poc"))
            {
                continue;
            }
            var nome = string.IsNullOrWhiteSpace(l.Area) ? "valore" : l.Area!;
            if (!mappa.TryGetValue(nome, out var a))
            {
                a = new AreaValore { Nome = nome };
                mappa[nome] = a;
            }
            if (ruolo == "val") { a.Val = l.Price; }
            else if (ruolo == "vah") { a.Vah = l.Price; }
            else { a.Poc = l.Price; }
            a.Chiave |= l.Key;
        }
        return mappa.Values.Where(a => a.Completa).ToList();
    }

    /// <summary>
    /// La fascia fra VAL e VAH, dipinta dietro le candele.
    ///
    /// Due righe orizzontali dicono dove sono i bordi; **non** dicono che in mezzo c'e' un dentro.
    /// E dentro o fuori dal valore e' la prima cosa che sceglie il modello: dentro si fa mean
    /// reverting sui bordi verso il POC, fuori no. La banda rende quella domanda una cosa che si
    /// vede invece di una che si calcola.
    /// </summary>
    private void DisegnaBandaValore(RenderContext context, Rectangle area, BridgeLevel[] livelli)
    {
        if (!ShowValueBand)
        {
            return;
        }
        var dataRight = DataAreaRight(context, area);
        foreach (var a in AreeDiValore(livelli))
        {
            var yAlto = ChartInfo!.GetYByPrice(a.Vah!.Value, false);
            var yBasso = ChartInfo!.GetYByPrice(a.Val!.Value, false);
            if (yBasso < yAlto)
            {
                (yAlto, yBasso) = (yBasso, yAlto);
            }
            var top = Math.Max(area.Top, yAlto);
            var bottom = Math.Min(area.Bottom, yBasso);
            if (bottom <= top)
            {
                continue;
            }
            context.FillRectangle(Color.FromArgb(ValueBandOpacity, 79, 195, 247),
                new Rectangle(area.Left, top, dataRight - area.Left, bottom - top));
        }
    }

    /// <summary>L'area di valore che contiene il prezzo, o quella piu' vicina se e' fuori.</summary>
    private (AreaValore? Area, bool Dentro) ValoreCorrente(BridgeLevel[] livelli, decimal prezzo)
    {
        var aree = AreeDiValore(livelli);
        if (aree.Count == 0)
        {
            return (null, false);
        }
        // CHI VINCE FRA DUE AREE. Durante la cash ci sono almeno due valori sul chart - quello
        // della notte e quello in sviluppo - e il piu' VICINO non e' il piu' importante: il
        // valore su cui si decide e' quello dichiarato chiave dall'analisi. Il 20 settembre,
        // col prezzo sopra entrambi, la sola distanza avrebbe mostrato il valore europeo mentre
        // la cash era aperta da venti minuti.
        var dentro = aree.Where(a => a.Val!.Value <= prezzo && prezzo <= a.Vah!.Value)
                         .OrderByDescending(a => a.Chiave)
                         .FirstOrDefault();
        if (dentro is not null)
        {
            return (dentro, true);
        }
        var vicina = aree.OrderByDescending(a => a.Chiave)
                         .ThenBy(a => Math.Min(Math.Abs(prezzo - a.Val!.Value),
                                               Math.Abs(prezzo - a.Vah!.Value)))
                         .First();
        return (vicina, false);
    }

    /// <summary>
    /// Una riga del pannello.
    ///
    /// <para><b><paramref name="Peso"/> esiste perche' il colore non basta a dire il senso.</b>
    /// Il rosso sul pannello vuol dire due cose diverse: "questo e' un veto" e "questa direzione
    /// e' SHORT". A schermo si distinguono dal contesto; in un JSON no, e la prima lettura di
    /// <c>/panel</c> ha marcato <c>SERVE A SHORT</c> come un veto. Un allarme falso su una riga
    /// che descrive il setup e' peggio di nessun allarme, perche' sposta la lettura.</para>
    ///
    /// <para>Quindi il peso si <b>dichiara</b> dove conta, invece di dedurlo dal colore a valle.</para>
    /// </summary>
    private readonly record struct RigaPannello(string Testo, Color Colore, string? Peso = null);

    private static readonly Color PanelBianco = Color.FromArgb(235, 235, 235);
    private static readonly Color PanelGrigio = Color.FromArgb(158, 158, 158);
    private static readonly Color PanelVerde = Color.FromArgb(102, 187, 106);
    private static readonly Color PanelRosso = Color.FromArgb(239, 83, 80);
    private static readonly Color PanelAmbra = Color.FromArgb(255, 183, 77);
    private static readonly Color PanelAzzurro = Color.FromArgb(79, 195, 247);

    private static readonly CultureInfo Italiano = CultureInfo.GetCultureInfo("it-IT");

    /// <summary>
    /// L'ora della barra nel fuso di chi guarda, che e' l'unico orologio che conta per chi opera.
    ///
    /// `IndicatorCandle.Time` arriva con `Kind == Unspecified` ma **e' UTC**: e' la stessa
    /// convenzione che usa gia' `Iso()`. Chiamarci sopra `ToLocalTime()` direttamente non fa
    /// niente - .NET tratta `Unspecified` come locale e restituisce lo stesso valore - e il
    /// pannello finirebbe per scrivere l'ora UTC spacciandola per l'ora di casa: due ore di
    /// sfasamento che sembrano un orario giusto. Va dichiarato UTC prima di convertire.
    /// </summary>
    private static DateTime OraLocale(DateTime t)
        => DateTime.SpecifyKind(t, t.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : t.Kind)
            .ToLocalTime();

    private static string Prezzo(decimal p) => p.ToString("N2", Italiano);

    private static string Lotti(decimal v) => v.ToString("N0", Italiano);

    private static string Segnato(decimal v) =>
        (v >= 0 ? "+" : "-") + Math.Abs(v).ToString("N0", Italiano);

    private static string SegnatoPrezzo(decimal v) =>
        (v >= 0 ? "+" : "-") + Math.Abs(v).ToString("N2", Italiano);

    /// <summary>
    /// Il delta come quota del volume che l'ha prodotto. E' la sola forma in cui un delta si
    /// giudica senza conoscere a memoria quanto scambia lo strumento: <c>+1.745</c> non dice
    /// niente, <c>+4,5%</c> dice che su cento lotti quattro e mezzo sono aggressione netta.
    /// Sotto i cento lotti non si stampa: la percentuale di un campione minuscolo e' rumore
    /// travestito da misura.
    /// </summary>
    private static string Quota(decimal delta, decimal volume) =>
        volume >= 100m ? $" ({(delta >= 0 ? "+" : "-")}{Math.Abs(delta) * 100m / volume:0.0}%)" : string.Empty;

    /// <summary>
    /// Delta e volume delle barre <b>intere</b> da <paramref name="da"/> a <paramref name="a"/>,
    /// estremi inclusi. Niente fascia di prezzo: e' il conto della seduta, non quello al livello.
    /// </summary>
    private (decimal Delta, decimal Volume) DeltaDiBarre(int da, int a)
    {
        decimal delta = 0m;
        decimal volume = 0m;
        for (var bar = Math.Max(0, da); bar <= a; bar++)
        {
            var c = GetCandle(bar);
            if (c is null)
            {
                continue;
            }
            delta += c.Delta;
            volume += c.Volume;
        }

        return (delta, volume);
    }

    /// <summary>
    /// La prima barra dell'apertura di cash del giorno di mercato dell'ultima barra, o -1 se
    /// quell'ora non e' ancora arrivata o non c'e' abbastanza storico.
    ///
    /// <para>Il giorno lo si prende <b>dall'ultima barra</b>, non dall'orologio di casa: in
    /// replay sono due date diverse, ed e' esattamente il caso in cui serve.</para>
    /// </summary>
    private int BarraDellApertura(int ultimo)
    {
        DateTime apertura;
        try
        {
            apertura = Momento(AperturaCash, GetCandle(ultimo).Time);
        }
        catch (FormatException)
        {
            return -1; // ora scritta male nelle impostazioni: si tace, non si inventa una seduta
        }

        if (GetCandle(ultimo).Time < apertura)
        {
            return -1;
        }

        for (var bar = ultimo; bar >= 0; bar--)
        {
            if (GetCandle(bar).Time < apertura)
            {
                return bar + 1;
            }
        }

        return 0;
    }

    /// <summary>Il nome del livello: la parte prima di " · ", che e' la convenzione del metodo.</summary>
    private static string NomeCorto(string? label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return "livello";
        }
        var i = label!.IndexOf(" · ", StringComparison.Ordinal);
        return (i > 0 ? label[..i] : label).Trim();
    }

    /// <summary>
    /// Il pannello, come testo, per chi non guarda lo schermo.
    ///
    /// <para><b>Perche' esiste.</b> Il pannello e' l'unico posto dove vivevano il livello in
    /// gioco, il lato di arrivo, lo sforzo al livello e i veti: erano calcolati qui e soltanto
    /// disegnati. L'agente che risponde dal vivo doveva rifare quei conti dalle candele grezze —
    /// cioe' duplicare questa logica — e una copia diverge. Il rischio non e' teorico: avrei
    /// detto un numero mentre lo schermo ne mostrava un altro, ed e' il peggiore degli esiti,
    /// perche' nessuno dei due si accorge dell'altro.</para>
    ///
    /// <para><b>Si restituiscono le righe gia' composte, non i dati per ricomporle.</b> Cosi' non
    /// c'e' nessuna seconda formattazione che possa scostarsi dalla prima: quello che esce di qui
    /// <b>e'</b> quello che sta sul chart, carattere per carattere. Il colore esce insieme al
    /// testo perche' porta significato - rosso e' un veto, ambra un avvertimento - e senza si
    /// perderebbe la meta' urgente del messaggio.</para>
    /// </summary>
    private object Pannello()
    {
        if (ChartInfo is null)
        {
            throw new BridgeException(409, "il chart non ha ancora un contesto di disegno");
        }

        var righe = ComponiPannello();
        foreach (var l in _watch)
        {
            righe.Add(new RigaPannello(l.Text ?? string.Empty, ParseColor(l.Color)));
        }

        var ultimo = CurrentBar - 1;
        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            marketTimeUtc = ultimo >= 0 ? Iso(GetCandle(ultimo).Time) : null,
            // La barra in formazione E' inclusa nel pannello, e va detto: i suoi numeri cambiano
            // fra una lettura e la successiva senza che il mercato abbia fatto niente di nuovo.
            barraInFormazione = true,
            count = righe.Count,
            lines = righe.Select(r => new
            {
                text = r.Testo,
                color = $"#{r.Colore.R:X2}{r.Colore.G:X2}{r.Colore.B:X2}",
                // Il senso del colore, perche' un esadecimale non si legge a colpo d'occhio in
                // un JSON e il livello di allarme e' la cosa che si cerca per prima.
                // Il peso dichiarato vince sempre. Il colore e' un ripiego e NON puo' produrre
                // un "veto" da solo: il rosso dice anche "direzione SHORT", e due significati
                // sullo stesso segnale diventano un allarme falso appena escono dallo schermo.
                peso = r.Peso
                    ?? (r.Colore == PanelAmbra ? "attenzione"
                        : r.Colore == PanelVerde ? "a favore"
                        : r.Colore == PanelRosso ? "contro"
                        : r.Colore == PanelBianco ? "forte"
                        : "normale"),
            }),
        };
    }

    private List<RigaPannello> ComponiPannello()
    {
        var righe = new List<RigaPannello>();
        var ultimo = CurrentBar - 1;
        if (ultimo < 1 || ChartInfo is null)
        {
            return righe;
        }

        var viva = GetCandle(ultimo);
        var prezzo = viva.Close;
        var livelli = _levels;

        righe.Add(new RigaPannello(
            $"{OraLocale(viva.Time):HH:mm}   {InstrumentInfo?.Instrument}   {Prezzo(prezzo)}",
            PanelBianco));

        // --- IL REGIME, E STA IN CIMA PERCHE' DECIDE LA SIZE ---------------------------------
        // "I would start from sensitive level of the market like we are doing together, and
        // regime" [5 · 1:18:06]. I livelli li calcola il motore delle regole; il regime, fino a
        // stasera, era una parola scritta a mano nel file della giornata - e da li' restava
        // ferma. Un regime fermo e' lo stesso difetto del livello vivo fermo, su una parola
        // invece che su un prezzo.
        var regime = LeggiIlRegime(ultimo);
        righe.Add(new RigaPannello(
            $"REGIME     {regime.Nome}",
            regime.Nome switch
            {
                "DIREZIONALE" => PanelVerde,
                "CHOPPY" => PanelAmbra,
                "BALANCE" => PanelAzzurro,
                _ => PanelGrigio,
            }));
        righe.Add(new RigaPannello($"           {regime.Perche}", PanelGrigio));
        if (regime.Size.Length > 0)
        {
            righe.Add(new RigaPannello($"           {regime.Size}", PanelGrigio));
        }

        // --- LA VELOCITA', E LA PAROLA PROXY NON E' UNA CAUTELA ------------------------------
        // La speed of tape e' "how fast order are being inputs" [3 · 1:08:41]. Il bridge non ha
        // quel dato: qui c'e' il volume della barra contro la distribuzione recente, che e' un
        // altro numero. Mille lotti in dieci secondi e mille in sessanta hanno lo stesso volume
        // e velocita' opposte. Chi legge deve saperlo ogni volta, non una volta.
        var (percentile, volumeBarra, velocitaOk) = VelocitaProxy(ultimo);
        var (tapeDa, tapeA) = FinestraDelTape(ultimo);
        var (bigFinestra, bigNetto, copreFinestra) = BigTradesNellaFinestra(tapeDa, tapeA);
        var quantoVeloce = !velocitaOk ? "non misurabile"
            : percentile >= PercentileAlto ? "alta"
            : percentile <= PercentileMorto ? "morta"
            : "normale";
        righe.Add(new RigaPannello(
            $"VELOCITA'  {quantoVeloce} — {Lotti(volumeBarra)} lotti, {percentile}° percentile su "
            + $"{FinestraVelocita} barre  (PROXY, non la speed of tape)",
            !velocitaOk ? PanelGrigio
                : percentile >= PercentileAlto ? PanelVerde
                : percentile <= PercentileMorto ? PanelAmbra
                : PanelGrigio));
        righe.Add(new RigaPannello(
            !copreFinestra
                ? $"           big trade: il registro copre solo da {EtaDelTape()}. Non e' zero, e' non lo so"
                : $"           {bigFinestra} big trade da {SogliaBigTrade}+ lotti in {PanelLookback} barre, "
                  + $"netto {Segnato(bigNetto)}",
            !copreFinestra ? PanelAmbra
                : bigFinestra == 0 ? PanelAmbra
                : bigNetto > 0 ? PanelVerde : bigNetto < 0 ? PanelRosso : PanelGrigio));

        // --- L'ALLARME SULL'ETA', E SOLO QUELLO ---------------------------------------------
        // Qui stavano tre cose: quante regole, la legenda dei marcatori, e l'elenco di cio' che
        // non era stato disegnato. Tolte il 20 settembre 2026 su richiesta dell'utente: sul chart
        // erano ingombro, e nessuna delle tre serve a decidere. Vivono dove servono davvero —
        // `/rules`, `bridge.py rules` e la sezione 6bis del giro d'orizzonte — cioe' in mano
        // all'agente, non davanti agli occhi di chi opera.
        //
        // QUESTA RIGA INVECE RESTA, e si vede solo quando c'e' da vederla. Un pannello fermo e'
        // indistinguibile da uno aggiornato: era il guasto del livello vivo, e non si ripropone
        // per fare spazio. Il motore adesso non puo' morire da solo, ma un'eccezione dentro il
        // ricalcolo produrrebbe lo stesso inganno in silenzio. In condizioni normali non stampa
        // niente.
        if (_rules.Length > 0)
        {
            var vecchie = _regoleAllaBarra >= 0 && ultimo - _regoleAllaBarra > 1;
            if (vecchie)
            {
                righe.Add(new RigaPannello(
                    $"LIVELLI FERMI DA {ultimo - _regoleAllaBarra} BARRE — ultimo ricalcolo "
                    + $"{OraLocale(_regoleAllOra):HH:mm}", PanelAmbra));
            }
            else if (_regoleAllaBarra < 0)
            {
                righe.Add(new RigaPannello(
                    $"{_rules.Length} regole depositate, nessun ricalcolo ancora", PanelAmbra));
            }
        }

        if (livelli.Length == 0)
        {
            righe.Add(new RigaPannello(
                _rules.Length > 0
                    ? "nessuna regola ha prodotto un livello"
                    : "nessun livello depositato",
                PanelAmbra));
            return righe;
        }

        // --- il valore in cui si sta ---------------------------------------------------------
        // Prima riga dopo il prezzo, e non e' un ornamento: dentro o fuori dal valore sceglie il
        // modello. Dentro si fa mean reverting sui bordi verso il POC; fuori quel permesso non c'e'.
        var (zona, dentro) = ValoreCorrente(livelli, prezzo);
        if (zona is not null)
        {
            var dovePoc = zona.Poc is null ? string.Empty
                : prezzo >= zona.Poc.Value ? "  sopra il POC" : "  sotto il POC";
            var poc = zona.Poc is null ? string.Empty : $"  POC {Prezzo(zona.Poc.Value)}";
            righe.Add(new RigaPannello(
                dentro
                    ? $"VALORE     DENTRO {zona.Nome}  {Prezzo(zona.Val!.Value)}-{Prezzo(zona.Vah!.Value)}{poc}"
                    : $"VALORE     FUORI {zona.Nome}, {(prezzo > zona.Vah!.Value ? "sopra" : "sotto")}"
                      + $"  {Prezzo(zona.Val!.Value)}-{Prezzo(zona.Vah!.Value)}{poc}",
                dentro ? PanelAzzurro : PanelAmbra));
            if (dentro && dovePoc.Length > 0)
            {
                righe.Add(new RigaPannello($"          {dovePoc.Trim()}", PanelGrigio));
            }
        }

        // --- il livello in gioco -------------------------------------------------------------
        BridgeLevel? gioco = null;
        var minDist = decimal.MaxValue;
        foreach (var l in livelli)
        {
            var d = Math.Abs(l.Price - prezzo);
            // A parita' di distanza vince quello che conta per la strategia: un bordo di contesto
            // non deve rubare il posto al livello su cui si decide.
            var peso = l.Key ? d : d + 0.01m;
            if (d <= InPlayRadius && peso < minDist)
            {
                minDist = peso;
                gioco = l;
            }
        }

        if (gioco is null)
        {
            righe.Add(new RigaPannello("NIENTE IN GIOCO   le due porte:", PanelAmbra));
            var sopra = livelli.Where(l => l.Price > prezzo).OrderBy(l => l.Price).FirstOrDefault();
            var sotto = livelli.Where(l => l.Price <= prezzo).OrderByDescending(l => l.Price).FirstOrDefault();
            if (sopra is not null)
            {
                righe.Add(new RigaPannello(
                    $"   sopra  {Prezzo(sopra.Price)}  {NomeCorto(sopra.Label)}  {SegnatoPrezzo(sopra.Price - prezzo)}",
                    sopra.Key ? PanelBianco : PanelGrigio));
            }
            if (sotto is not null)
            {
                righe.Add(new RigaPannello(
                    $"   sotto  {Prezzo(sotto.Price)}  {NomeCorto(sotto.Label)}  {SegnatoPrezzo(sotto.Price - prezzo)}",
                    sotto.Key ? PanelBianco : PanelGrigio));
            }

            // I VETI VANNO DETTI ANCHE QUI, E SOPRATTUTTO QUI. Senza un livello in gioco il
            // pannello usciva subito, e il veto del mezzo range - che e' proprio quello che vale
            // quando non si sta testando niente - non poteva scattare mai. "We are in the middle.
            // This is not where I want to engage" [4 · 17:37] descrive esattamente questo stato.
            foreach (var v in Veti(ultimo, prezzo, regime, null, dentro))
            {
                righe.Add(new RigaPannello($"   ! {v}", PanelRosso, "veto"));
            }
            return righe;
        }

        var livello = gioco.Price;
        var dove = prezzo >= livello ? "sopra" : "sotto";

        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        var soglia = Math.Max(tick * 4, InPlayRadius / 4m);
        var arrivo = string.Empty;
        for (var bar = ultimo; bar >= Math.Max(0, ultimo - 240); bar--)
        {
            var c = GetCandle(bar);
            if (c.Close > livello + soglia) { arrivo = "SOPRA"; break; }
            if (c.Close < livello - soglia) { arrivo = "SOTTO"; break; }
        }

        var titolo = gioco.Key ? "IN GIOCO  " : "in gioco  ";
        righe.Add(new RigaPannello($"{titolo} {NomeCorto(gioco.Label)}   {Prezzo(livello)}",
                                   ParseColor(gioco.Color)));
        righe.Add(new RigaPannello(
            arrivo.Length > 0
                ? $"           {SegnatoPrezzo(prezzo - livello)} {dove}   arrivato da {arrivo}"
                : $"           {SegnatoPrezzo(prezzo - livello)} {dove}   lato di arrivo non deciso",
            arrivo.Length > 0 ? PanelBianco : PanelAmbra));

        // --- cosa e' stato scambiato A QUEL PREZZO --------------------------------------------
        var da = Math.Max(0, ultimo - PanelLookback + 1);
        decimal vol = 0m;
        decimal delta = 0m;
        var tocchi = 0;
        var respinti = 0;
        var passati = 0;
        var fascia = tick * 2;

        for (var bar = da; bar <= ultimo; bar++)
        {
            var c = GetCandle(bar);
            for (var pz = livello - fascia; pz <= livello + fascia; pz += tick)
            {
                var info = c.GetPriceVolumeInfo(pz);
                if (info is null)
                {
                    continue;
                }
                vol += info.Volume;
                delta += info.Ask - info.Bid;
            }

            if (c.Low - fascia <= livello && livello <= c.High + fascia)
            {
                tocchi++;
                if (bar > 0)
                {
                    var lato = GetCandle(bar - 1).Close >= livello;
                    if ((c.Close >= livello) == lato) { respinti++; } else { passati++; }
                }
            }
        }

        // --- sforzo e risultato, che e' la coppia con cui si legge l'assorbimento -------------
        // I numeri nudi non dicono niente: 3.482 lotti sono tanti o pochi a seconda di cosa hanno
        // prodotto. Il metodo legge SEMPRE la coppia - quanto e' stato speso li', e se il prezzo
        // e' passato. Sforzo alto e risultato nullo e' assorbimento; le righe lo mettono una
        // sopra l'altra invece di lasciare la sottrazione a chi guarda.
        //
        // **La testata dichiara la misura, e non e' pignoleria.** "SFORZO 3.482 lotti" non diceva
        // ne' a quale prezzo ne' su quale finestra, e il delta sotto sembrava il delta della
        // seduta mentre erano i soli lotti scambiati dentro una fascia di due tick, in dieci
        // barre. Due numeri con lo stesso nome e significati diversi e' il modo piu' rapido di
        // leggere il chart al contrario.
        var chi = delta > 0 ? "compratori aggressivi" : delta < 0 ? "venditori aggressivi" : "pari";
        righe.Add(new RigaPannello(
            $"AL LIVELLO {Prezzo(livello)} +/-{Prezzo(fascia)}, ultime {PanelLookback} barre",
            PanelGrigio));
        righe.Add(new RigaPannello(
            $"  sforzo   {Lotti(vol)} lotti scambiati a questo prezzo",
            PanelGrigio));
        righe.Add(new RigaPannello(
            $"  delta    {Segnato(delta)}{Quota(delta, vol)}  {chi}",
            delta > 0 ? PanelVerde : delta < 0 ? PanelRosso : PanelGrigio));
        righe.Add(new RigaPannello(
            $"  esito    toccato {tocchi}x, respinto {respinti}x, passato {passati}x",
            PanelGrigio));

        // I BIG TRADES SONO LA META' MANCANTE DELLO SFORZO. Mille lotti in ordini da due non
        // sono un muro; mille lotti in sei ordini da centosessanta lo sono, e il live guarda
        // sempre la seconda cosa: "look how many absorption contract you have here on this
        // horizontal level: 70, 75, 141, 33" [6 · 1:01:24]. Il volume da solo non lo distingue.
        var (bigQui, bigQuiNetto, bigQuiTot, copreQui) = BigTradesAlLivello(
            livello, fascia, GetCandle(da)?.Time ?? DateTime.MinValue, tapeA);
        righe.Add(new RigaPannello(
            !copreQui
                ? $"  big      il registro copre solo da {EtaDelTape()}: non si puo' dire"
                : bigQui == 0
                    ? $"  big      nessun ordine da {SogliaBigTrade}+ lotti a questo prezzo"
                    : $"  big      {bigQui} ordini da {SogliaBigTrade}+ lotti, {Lotti(bigQuiTot)} lotti, "
                      + $"netto {Segnato(bigQuiNetto)}",
            !copreQui || bigQui == 0 ? PanelAmbra
                : bigQuiNetto > 0 ? PanelVerde : bigQuiNetto < 0 ? PanelRosso : PanelGrigio));

        // --- lo stesso conto su finestre piu' larghe -----------------------------------------
        // Serve a dare una scala. Un delta di +120 al livello non si giudica da solo: se la
        // seduta sta a +2.300 e' un rivolo nella stessa direzione, se la seduta sta a -1.500 e'
        // una divergenza, ed e' la misura che il 16 settembre ha smentito un permesso LONG tenuto
        // in piedi per ottantaquattro minuti da un gate orario
        // (docs/research/metodo/il-permesso-si-misura-non-si-aspetta.md).
        //
        // Qui il delta e' quello delle barre INTERE, non della fascia al livello: e' una
        // popolazione diversa e la testata lo dice, altrimenti si finisce per confrontare due
        // numeri che non sono confrontabili.
        var (dLargo, vLargo) = DeltaDiBarre(Math.Max(0, ultimo - PanelLookbackLargo + 1), ultimo);
        righe.Add(new RigaPannello("SU TUTTE LE BARRE, non solo al livello:", PanelGrigio));
        righe.Add(new RigaPannello(
            $"  {PanelLookbackLargo} barre {Segnato(dLargo)}{Quota(dLargo, vLargo)}  su {Lotti(vLargo)} lotti",
            dLargo > 0 ? PanelVerde : dLargo < 0 ? PanelRosso : PanelGrigio));

        var apertura = BarraDellApertura(ultimo);
        if (apertura >= 0 && apertura < ultimo)
        {
            var (dCash, vCash) = DeltaDiBarre(apertura, ultimo);
            righe.Add(new RigaPannello(
                $"  da {AperturaCash} {Segnato(dCash)}{Quota(dCash, vCash)}  su {Lotti(vCash)} lotti",
                dCash > 0 ? PanelVerde : dCash < 0 ? PanelRosso : PanelGrigio));
        }

        // --- lo scenario, e poi le condizioni che gli servono ---------------------------------
        // Le condizioni le SPUNTA la macchina, non le inventa e non le fa scattare. Lo scenario
        // non lo verifica affatto: e' la dichiarazione dell'analisi su cosa ci si fa, qui.
        if (gioco.Scenario is { } sc)
        {
            var verso = (sc.Direzione ?? "").ToUpperInvariant();
            var colore = verso == "LONG" ? PanelVerde : verso == "SHORT" ? PanelRosso : PanelGrigio;
            righe.Add(new RigaPannello($"SERVE A     {verso}  {sc.Nome}", colore));
            var coda = new List<string>();
            if (sc.Bersaglio is { } t)
            {
                coda.Add($"bersaglio {Prezzo(t)} ({SegnatoPrezzo(t - prezzo)})");
            }
            if (sc.Invalida is { } inv)
            {
                coda.Add($"invalida {Prezzo(inv)}");
            }
            if (coda.Count > 0)
            {
                righe.Add(new RigaPannello("           " + string.Join("   ", coda), PanelGrigio));
            }
            // IL BREAK EVEN VA DETTO CON LO STOP, NON DOPO. Nel live Q1 la gestione e' l'edge e
            // questa ne e' la regola singola piu' importante: uno stop senza il suo break even
            // e' meta' istruzione. Quando l'analisi non lo ha dichiarato, il pannello lo dice
            // invece di tacere, perche' il campo vuoto e' esattamente il difetto.
            righe.Add(new RigaPannello(
                sc.Pareggio is { } pg
                    ? $"           pareggio {Prezzo(pg)} ({SegnatoPrezzo(pg - prezzo)}) — li' il lato opposto torna a vincere"
                    : "           pareggio NON DICHIARATO: metti pareggio_livello nella regola",
                sc.Pareggio is null ? PanelAmbra : PanelGrigio));
        }

        if (gioco.Conditions is { Length: > 0 } condizioni)
        {
            var fatte = 0;
            var righeCond = new List<RigaPannello>();
            foreach (var c in condizioni)
            {
                var (ok, misura) = ValutaCondizione(c, gioco, prezzo, delta, vol, arrivo);
                if (ok)
                {
                    fatte++;
                }
                righeCond.Add(new RigaPannello($"   {(ok ? "[x]" : "[ ]")} {c.Testo}{misura}",
                                               ok ? PanelVerde : PanelGrigio));
            }
            righe.Add(new RigaPannello(
                $"SERVE      {fatte} di {condizioni.Length}",
                fatte == condizioni.Length ? PanelVerde : PanelGrigio));
            righe.AddRange(righeCond);
        }

        var rng = viva.High - viva.Low;
        var pos = rng > 0 ? (viva.Close - viva.Low) / rng : 0m;
        // --- I VETI, E STANNO IN FONDO PERCHE' ANNULLANO CIO' CHE STA SOPRA -----------------
        // Non sono il complemento dei prerequisiti. I prerequisiti si contano - "2 di 3" - e
        // servono TUTTI; i veti no: ne basta UNO e il setup non si prende, quante che siano le
        // spunte verdi sopra. "We cannot force setups. Only when it's there" [4 · 43:22].
        //
        // Ce ne sono quattro misurabili con quello che il bridge ha. I due che mancano - "troppo
        // vicini al muro" e la posizione nella curva del composito - chiedono un dato che questo
        // chart non tiene, e NON si simulano: un veto inventato fa saltare setup buoni con
        // l'aria di una misura, ed e' un danno peggiore di un veto mancante.
        var veti = Veti(ultimo, prezzo, regime, gioco, dentro);
        if (veti.Count > 0)
        {
            righe.Add(new RigaPannello(
                $"VETI       {veti.Count}, e ne basta uno", PanelRosso, "veto"));
            foreach (var v in veti)
            {
                righe.Add(new RigaPannello($"   ! {v}", PanelRosso, "veto"));
            }
        }

        // La barra in formazione, e va detto che lo e': i suoi numeri cambiano fino alla
        // chiusura, e "pos 0.95" era il solo dato del pannello che nessuno sapeva leggere senza
        // che glielo avessero spiegato una volta.
        righe.Add(new RigaPannello(
            $"BARRA APERTA {OraLocale(viva.Time):HH:mm}, cambia ancora", PanelGrigio));
        righe.Add(new RigaPannello(
            $"  {Lotti(viva.Volume)} lotti   delta {Segnato(viva.Delta)}{Quota(viva.Delta, viva.Volume)}"
            + $"   chiude nel {pos * 100m:0}% alto del suo range",
            viva.Delta > 0 ? PanelVerde : viva.Delta < 0 ? PanelRosso : PanelGrigio));

        return righe;
    }

    /// <summary>
    /// Spunta una condizione contro cio' che e' gia' stato misurato. Non calcola niente di nuovo
    /// e non decide niente: se il tipo non e' riconosciuto la lascia non soddisfatta e lo dice,
    /// invece di far finta che sia vera.
    /// </summary>
    private (bool Ok, string Misura) ValutaCondizione(
        BridgeCondition c, BridgeLevel livello, decimal prezzo,
        decimal deltaAlLivello, decimal volumeAlLivello, string arrivo)
    {
        switch ((c.Cosa ?? string.Empty).ToLowerInvariant())
        {
            case "chiusura":
            {
                var soglia = c.Prezzo ?? livello.Price;
                var sopra = (c.Verso ?? "sopra").StartsWith("sop", StringComparison.OrdinalIgnoreCase);
                return (sopra ? prezzo > soglia : prezzo < soglia, string.Empty);
            }
            case "delta":
            {
                var soglia = c.Almeno ?? 0m;
                var ok = soglia >= 0 ? deltaAlLivello >= soglia : deltaAlLivello <= soglia;
                return (ok, $"   ({Segnato(deltaAlLivello)})");
            }
            case "volume":
            {
                var soglia = c.Almeno ?? 0m;
                return (volumeAlLivello >= soglia, $"   ({Lotti(volumeAlLivello)})");
            }
            case "arrivo":
                return (arrivo.Length > 0
                        && string.Equals(arrivo, c.Verso, StringComparison.OrdinalIgnoreCase),
                    arrivo.Length > 0 ? $"   ({arrivo})" : "   (non deciso)");
            default:
                return (false, "   (condizione sconosciuta)");
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
