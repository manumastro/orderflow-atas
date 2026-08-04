using System.Text.Json;
using ATAS.Indicators;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

public sealed class HistoricalCumulativeContextRecorder : Indicator
{
    private const string Schema = "fof-historical-cumulative-context-v1";
    private static readonly TimeSpan MaximumHistoricalRequestDuration = TimeSpan.FromDays(7);
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly object _sync = new();
    private readonly Dictionary<int, RequestState> _pendingRequests = [];
    private int _nextEventId;

    public HistoricalCumulativeContextRecorder()
    {
        Name = "Fabio Historical Cumulative Context Recorder";
    }

    protected override void OnCalculate(int bar, decimal value)
    {
    }

    protected override void OnRecalculate()
    {
        lock (_sync)
        {
            _pendingRequests.Clear();
            _nextEventId = 0;
        }

        base.OnRecalculate();
    }

    protected override void OnFinishRecalculate()
    {
        try
        {
            if (CurrentBar <= 0)
            {
                LogObservation(new HistoricalNotice(
                    Schema,
                    "historical-context-skipped",
                    null,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.UtcNow,
                    "The chart had no bars when OnFinishRecalculate ran."));
                return;
            }

            var first = GetCandle(0);
            var last = GetCandle(CurrentBar - 1);
            var beginTime = first.Time;
            var endTime = last.LastTime;
            var duration = endTime - beginTime;
            var range = new ChartRangeSnapshot(
                CreateRangeId(beginTime, endTime),
                beginTime,
                endTime,
                CurrentBar,
                duration.TotalDays,
                CaptureSecurity());

            if (duration <= TimeSpan.Zero)
            {
                LogObservation(new HistoricalNotice(
                    Schema,
                    "historical-context-skipped",
                    range,
                    beginTime,
                    endTime,
                    DateTime.UtcNow,
                    "The loaded chart range is empty or invalid."));
                return;
            }

            if (duration > MaximumHistoricalRequestDuration)
            {
                LogObservation(new HistoricalNotice(
                    Schema,
                    "historical-context-skipped",
                    range,
                    beginTime,
                    endTime,
                    DateTime.UtcNow,
                    $"Chart range is {duration.TotalDays:F2} days; ATAS limits one cumulative-trades request to seven days."));
                return;
            }

            LogObservation(new HistoricalNotice(
                Schema,
                "historical-context-started",
                range,
                beginTime,
                endTime,
                DateTime.UtcNow,
                "Logging loaded chart candles and requesting historical CumulativeTrade records with minVolume=0 and maxVolume=0."));

            for (var bar = 0; bar < CurrentBar; bar++)
                LogObservation(CaptureCandle(range, bar));

            var request = new CumulativeTradesRequest(beginTime, endTime, 0, 0);

            lock (_sync)
                _pendingRequests[request.RequestId] = new RequestState(range, request.BeginTime, request.EndTime);

            RequestForCumulativeTrades(request);
            LogObservation(new HistoricalRequestNotice(
                Schema,
                "historical-cumulative-requested",
                range,
                request.RequestId,
                request.BeginTime,
                request.EndTime,
                request.MinVolume,
                request.MaxVolume,
                DateTime.UtcNow));
        }
        catch (Exception exception)
        {
            this.LogError("FofHistoricalContext request failed.", exception);
        }
        finally
        {
            base.OnFinishRecalculate();
        }
    }

    protected override void OnCumulativeTradesResponse(
        CumulativeTradesRequest request,
        IEnumerable<CumulativeTrade> cumulativeTrades)
    {
        RequestState state;

        lock (_sync)
        {
            if (!_pendingRequests.Remove(request.RequestId, out state!))
                return;
        }

        var count = 0;

        foreach (var trade in cumulativeTrades)
        {
            count++;
            LogObservation(CaptureTrade(state.Range, request, trade));
        }

        LogObservation(new HistoricalResponseNotice(
            Schema,
            "historical-cumulative-response",
            state.Range,
            request.RequestId,
            request.BeginTime,
            request.EndTime,
            count,
            DateTime.UtcNow));
    }

    private HistoricalCandleObservation CaptureCandle(ChartRangeSnapshot range, int bar)
    {
        var candle = GetCandle(bar);
        var valueArea = candle.ValueArea;
        var priceLevels = candle.GetAllPriceLevels()
            .Select(ToPriceLevelSnapshot)
            .ToArray();

        return new HistoricalCandleObservation(
            Schema,
            "chart-candle",
            range,
            bar,
            candle.Time,
            candle.LastTime,
            candle.Open,
            candle.High,
            candle.Low,
            candle.Close,
            candle.Volume,
            candle.Bid,
            candle.Ask,
            candle.Delta,
            candle.Ticks,
            candle.VWAP,
            valueArea?.ValueAreaHigh,
            valueArea?.ValueAreaLow,
            ToPriceLevelSnapshot(candle.MaxVolumePriceInfo),
            priceLevels);
    }

    private HistoricalCumulativeTradeObservation CaptureTrade(
        ChartRangeSnapshot range,
        CumulativeTradesRequest request,
        CumulativeTrade trade)
    {
        var ticks = trade.Ticks.Select(tick => new TickSnapshot(
            tick.Time,
            tick.Price,
            tick.Volume,
            tick.Direction.ToString(),
            tick.DataType.ToString(),
            tick.AggressorExchangeOrderId,
            tick.ExchangeOrderId)).ToArray();

        int eventId;
        lock (_sync)
            eventId = ++_nextEventId;

        return new HistoricalCumulativeTradeObservation(
            Schema,
            "historical-cumulative-trade",
            range,
            request.RequestId,
            eventId,
            trade.Time,
            trade.Direction.ToString(),
            trade.Volume,
            trade.FirstPrice,
            trade.Lastprice,
            ticks,
            CaptureSecurity(),
            DateTime.UtcNow);
    }

    private SecuritySnapshot CaptureSecurity()
    {
        var security = TradingManager?.Security;

        return new SecuritySnapshot(
            security?.SecurityId,
            security?.ConnectorId,
            security?.Code,
            security?.Instrument,
            security?.Exchange,
            security?.TickSize);
    }

    private static PriceLevelSnapshot? ToPriceLevelSnapshot(PriceVolumeInfo? level)
    {
        return level is null
            ? null
            : new PriceLevelSnapshot(
                level.Price,
                level.Ask,
                level.Bid,
                level.Volume,
                level.Ask - level.Bid,
                level.Ticks,
                level.Between,
                level.Time);
    }

    private static string CreateRangeId(DateTime beginTime, DateTime endTime) =>
        $"{beginTime:yyyyMMddTHHmmss}-{endTime:yyyyMMddTHHmmss}";

    private void LogObservation<T>(T observation)
    {
        try
        {
            this.LogInfo("FofHistoricalContext {0}", JsonSerializer.Serialize(observation, JsonOptions));
        }
        catch (Exception exception)
        {
            this.LogError("FofHistoricalContext serialization failed.", exception);
        }
    }

    private sealed record RequestState(
        ChartRangeSnapshot Range,
        DateTime BeginTime,
        DateTime EndTime);

    private sealed record ChartRangeSnapshot(
        string RangeId,
        DateTime BeginTime,
        DateTime EndTime,
        int BarCount,
        double DurationDays,
        SecuritySnapshot Security);

    private sealed record HistoricalNotice(
        string Schema,
        string Type,
        ChartRangeSnapshot? Range,
        DateTime BeginTime,
        DateTime EndTime,
        DateTime ReceivedAtUtc,
        string Detail);

    private sealed record HistoricalRequestNotice(
        string Schema,
        string Type,
        ChartRangeSnapshot Range,
        int RequestId,
        DateTime BeginTime,
        DateTime EndTime,
        decimal MinVolume,
        decimal MaxVolume,
        DateTime ReceivedAtUtc);

    private sealed record HistoricalResponseNotice(
        string Schema,
        string Type,
        ChartRangeSnapshot Range,
        int RequestId,
        DateTime BeginTime,
        DateTime EndTime,
        int Records,
        DateTime ReceivedAtUtc);

    private sealed record HistoricalCandleObservation(
        string Schema,
        string Source,
        ChartRangeSnapshot Range,
        int Bar,
        DateTime BeginTime,
        DateTime EndTime,
        decimal Open,
        decimal High,
        decimal Low,
        decimal Close,
        decimal Volume,
        decimal Bid,
        decimal Ask,
        decimal Delta,
        decimal Ticks,
        decimal Vwap,
        decimal? ValueAreaHigh,
        decimal? ValueAreaLow,
        PriceLevelSnapshot? Poc,
        IReadOnlyList<PriceLevelSnapshot?> PriceLevels);

    private sealed record HistoricalCumulativeTradeObservation(
        string Schema,
        string Source,
        ChartRangeSnapshot Range,
        int RequestId,
        int EventId,
        DateTime Time,
        string Direction,
        decimal TotalVolume,
        decimal FirstPrice,
        decimal LastPrice,
        IReadOnlyList<TickSnapshot> Ticks,
        SecuritySnapshot Security,
        DateTime ReceivedAtUtc);

    private sealed record SecuritySnapshot(
        string? SecurityId,
        string? ConnectorId,
        string? Code,
        string? Instrument,
        string? Exchange,
        decimal? TickSize);

    private sealed record PriceLevelSnapshot(
        decimal Price,
        decimal Ask,
        decimal Bid,
        decimal Volume,
        decimal Delta,
        int Ticks,
        decimal Between,
        int Time);

    private sealed record TickSnapshot(
        DateTime Time,
        decimal Price,
        decimal Volume,
        string Direction,
        string DataType,
        long? AggressorExchangeOrderId,
        long? ExchangeOrderId);
}
