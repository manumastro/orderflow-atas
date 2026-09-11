using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;
using ATAS.Indicators;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Espone i dati che ATAS mette a disposizione di un indicatore su un endpoint HTTP locale,
/// in modo che un'analisi esterna possa richiederli quando servono invece di dipendere da una
/// cattura decisa in anticipo.
///
/// Il bridge e' puramente osservativo: legge, converte in JSON e restituisce. Non calcola
/// soglie, non classifica, non emette segnali e non disegna nulla sul chart. Ogni risposta
/// contiene lo strumento e il timeframe del chart su cui il bridge e' caricato, perche' quel
/// contesto e' parte del dato e non va ricostruito a posteriori.
///
/// Il listener e' legato a 127.0.0.1: non e' raggiungibile dalla rete.
/// </summary>
[DisplayName("Fabio Data Bridge")]
public sealed class DataBridge : Indicator
{
    private const string Schema = "fof-data-bridge-v1";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false,
    };

    /// <summary>
    /// ATAS accetta una sola richiesta CumulativeTrades pendente alla volta: il semaforo
    /// serializza le richieste HTTP concorrenti invece di lasciarle fallire.
    /// </summary>
    private readonly SemaphoreSlim _cumulativeGate = new(1, 1);

    private readonly object _sync = new();
    private readonly CancellationTokenSource _shutdown = new();

    private HttpListener? _listener;
    private Task? _loop;
    private TaskCompletionSource<List<CumulativeTrade>>? _pendingCumulative;
    private int _pendingCumulativeRequestId;

    public DataBridge()
    {
        Name = "Fabio Data Bridge";
        DenyToChangePanel = true;
    }

    [Display(Name = "Port", GroupName = "Bridge", Description = "Porta locale del bridge.")]
    [Range(1024, 65535)]
    public int Port { get; set; } = 8787;

    [Display(Name = "Enabled", GroupName = "Bridge", Description = "Avvia o ferma il listener locale.")]
    public bool BridgeEnabled { get; set; } = true;

    /// <summary>
    /// Numero massimo di elementi restituiti da una singola risposta, per evitare payload
    /// ingestibili. Le richieste piu' ampie vanno spezzate dal chiamante.
    /// </summary>
    [Display(Name = "Max items", GroupName = "Bridge", Description = "Limite di elementi per risposta.")]
    [Range(100, 2_000_000)]
    public int MaxItems { get; set; } = 200_000;

    protected override void OnCalculate(int bar, decimal value)
    {
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Start();
    }

    protected override void OnDispose()
    {
        Stop();
        base.OnDispose();
    }

    private void Start()
    {
        if (!BridgeEnabled)
        {
            return;
        }

        lock (_sync)
        {
            if (_listener is not null)
            {
                return;
            }

            try
            {
                var listener = new HttpListener();
                listener.Prefixes.Add($"http://127.0.0.1:{Port}/");
                listener.Start();
                _listener = listener;
                _loop = Task.Run(() => AcceptLoopAsync(listener, _shutdown.Token));
                this.LogInfo("FofDataBridge listening on http://127.0.0.1:{0}/ schema {1}", Port, Schema);
            }
            catch (Exception exception)
            {
                this.LogError($"FofDataBridge could not listen on port {Port}.", exception);
            }
        }
    }

    private void Stop()
    {
        lock (_sync)
        {
            if (_listener is null)
            {
                return;
            }

            try
            {
                _shutdown.Cancel();
                _listener.Stop();
                _listener.Close();
            }
            catch (Exception exception)
            {
                this.LogError("FofDataBridge failed to stop cleanly.", exception);
            }
            finally
            {
                _listener = null;
                _loop = null;
            }
        }
    }

    private async Task AcceptLoopAsync(HttpListener listener, CancellationToken cancellation)
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
            catch (Exception exception)
            {
                this.LogError("FofDataBridge accept failed.", exception);
                return;
            }

            _ = Task.Run(() => HandleAsync(context, cancellation), CancellationToken.None);
        }
    }

    private async Task HandleAsync(HttpListenerContext context, CancellationToken cancellation)
    {
        var path = context.Request.Url?.AbsolutePath.TrimEnd('/') ?? string.Empty;
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

    // ---------------------------------------------------------------- endpoints

    private object Health() => new
    {
        schema = Schema,
        instrument = InstrumentInfo?.Instrument,
        timeFrame = ChartInfo?.TimeFrame,
        chartType = ChartInfo?.ChartType,
        bars = CurrentBar,
        marketTimeUtc = Iso(UtcTime),
        endpoints = new[]
        {
            "/health", "/instrument", "/limits", "/session", "/rollovers",
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
        => DataProvider as IOnlineDataProvider
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
