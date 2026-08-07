using System.ComponentModel;
using System.Text.Json;
using ATAS.Indicators;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

[DisplayName("Fabio Cumulative Trade Recorder")]
public sealed class CumulativeTradeObservationRecorder : Indicator
{
    private const string Schema = "fof-observation-v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly object _sync = new();
    private readonly Dictionary<CumulativeTrade, ObservationState> _states =
        new(ReferenceEqualityComparer.Instance);
    private readonly HashSet<int> _pendingHistoricalRequests = [];
    private int _nextEventId;

    public CumulativeTradeObservationRecorder()
    {
        Name = "Fabio Cumulative Trade Recorder";
    }

    protected override void OnCalculate(int bar, decimal value)
    {
    }

    protected override void OnRecalculate()
    {
        lock (_sync)
        {
            _states.Clear();
            _pendingHistoricalRequests.Clear();
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
                LogObservation(new ObservationNotice(
                    Schema,
                    "historical-request-skipped",
                    DateTime.MinValue,
                    DateTime.MinValue,
                    "The chart had no bars when OnFinishRecalculate ran."));
                return;
            }

            var first = GetCandle(0);
            var last = GetCandle(CurrentBar - 1);
            var duration = last.LastTime - first.Time;

            if (duration > TimeSpan.FromDays(7))
            {
                LogObservation(new ObservationNotice(
                    Schema,
                    "historical-request-skipped",
                    first.Time,
                    last.LastTime,
                    $"Chart range is {duration.TotalDays:F2} days; ATAS limits one cumulative-trades request to seven days."));
                return;
            }

            var request = new CumulativeTradesRequest(first.Time, last.LastTime, 0, 0);

            lock (_sync)
                _pendingHistoricalRequests.Add(request.RequestId);

            RequestForCumulativeTrades(request);
            LogObservation(new ObservationNotice(
                Schema,
                "historical-requested",
                first.Time,
                last.LastTime,
                $"requestId={request.RequestId}; minVolume=0; maxVolume=0"));
        }
        catch (Exception exception)
        {
            this.LogError("FofObservation historical request failed.", exception);
        }
        finally
        {
            base.OnFinishRecalculate();
        }
    }

    protected override void OnCumulativeTrade(CumulativeTrade trade)
    {
        RecordTrade(trade, "live-new");
    }

    protected override void OnUpdateCumulativeTrade(CumulativeTrade trade)
    {
        RecordTrade(trade, "live-update");
    }

    protected override void OnCumulativeTradesResponse(
        CumulativeTradesRequest request,
        IEnumerable<CumulativeTrade> cumulativeTrades)
    {
        lock (_sync)
        {
            if (!_pendingHistoricalRequests.Remove(request.RequestId))
                return;
        }

        var count = 0;

        foreach (var trade in cumulativeTrades)
        {
            RecordTrade(trade, "historical-snapshot");
            count++;
        }

        LogObservation(new ObservationNotice(
            Schema,
            "historical-response",
            request.BeginTime,
            request.EndTime,
            $"requestId={request.RequestId}; records={count}"));
    }

    private void RecordTrade(CumulativeTrade trade, string source)
    {
        var bar = FindContainingBar(trade.Time);
        var footprint = bar >= 0 ? CaptureFootprint(bar, trade) : null;
        var ticks = trade.Ticks.Select(tick => new TickSnapshot(
            tick.Time,
            tick.Price,
            tick.Volume,
            tick.Direction.ToString(),
            tick.DataType.ToString(),
            tick.AggressorExchangeOrderId,
            tick.ExchangeOrderId)).ToArray();

        ObservationState state;
        decimal incrementalVolume;
        int updateNumber;

        lock (_sync)
        {
            if (!_states.TryGetValue(trade, out state!))
            {
                state = new ObservationState(++_nextEventId);
                _states.Add(trade, state);
            }
            else if (trade.Volume == state.ReportedVolume
                && trade.FirstPrice == state.FirstPrice
                && trade.Lastprice == state.LastPrice
                && ticks.Length == state.TickCount
                && (state.HasFootprint || footprint is null))
            {
                return;
            }

            incrementalVolume = trade.Volume - state.ReportedVolume;
            state.ReportedVolume = trade.Volume;
            state.FirstPrice = trade.FirstPrice;
            state.LastPrice = trade.Lastprice;
            state.TickCount = ticks.Length;
            state.HasFootprint |= footprint is not null;
            updateNumber = ++state.Updates;
        }

        var security = TradingManager?.Security;

        LogObservation(new TradeObservation(
            Schema,
            source,
            state.EventId,
            updateNumber,
            trade.Time,
            trade.Direction.ToString(),
            trade.Volume,
            incrementalVolume,
            trade.FirstPrice,
            trade.Lastprice,
            ticks,
            new SecuritySnapshot(
                security?.SecurityId,
                security?.ConnectorId,
                security?.Code,
                security?.Instrument,
                security?.Exchange,
                security?.TickSize),
            footprint));
    }

    private int FindContainingBar(DateTime time)
    {
        var low = 0;
        var high = CurrentBar - 1;

        while (low <= high)
        {
            var bar = low + (high - low) / 2;
            var candle = GetCandle(bar);

            if (time < candle.Time)
            {
                high = bar - 1;
                continue;
            }

            if (time > candle.LastTime)
            {
                low = bar + 1;
                continue;
            }

            return bar;
        }

        return -1;
    }

    private FootprintSnapshot CaptureFootprint(int bar, CumulativeTrade trade)
    {
        var candle = GetCandle(bar);
        var poc = candle.MaxVolumePriceInfo;

        return new FootprintSnapshot(
            bar,
            candle.Time,
            candle.LastTime,
            ToPriceLevelSnapshot(poc),
            ToPriceLevelSnapshot(candle.GetPriceVolumeInfo(trade.FirstPrice)),
            ToPriceLevelSnapshot(candle.GetPriceVolumeInfo(trade.Lastprice)));
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

    private void LogObservation<T>(T observation)
    {
        try
        {
            this.LogInfo("FofObservation {0}", JsonSerializer.Serialize(observation, JsonOptions));
        }
        catch (Exception exception)
        {
            this.LogError("FofObservation serialization failed.", exception);
        }
    }

    private sealed class ObservationState(int eventId)
    {
        public int EventId { get; } = eventId;
        public decimal ReportedVolume { get; set; }
        public decimal FirstPrice { get; set; }
        public decimal LastPrice { get; set; }
        public int TickCount { get; set; }
        public bool HasFootprint { get; set; }
        public int Updates { get; set; }
    }

    private sealed record ObservationNotice(
        string Schema,
        string Type,
        DateTime BeginTime,
        DateTime EndTime,
        string Detail);

    private sealed record TradeObservation(
        string Schema,
        string Source,
        int EventId,
        int UpdateNumber,
        DateTime Time,
        string Direction,
        decimal TotalVolume,
        decimal IncrementalVolume,
        decimal FirstPrice,
        decimal LastPrice,
        IReadOnlyList<TickSnapshot> Ticks,
        SecuritySnapshot Security,
        FootprintSnapshot? Footprint);

    private sealed record SecuritySnapshot(
        string? SecurityId,
        string? ConnectorId,
        string? Code,
        string? Instrument,
        string? Exchange,
        decimal? TickSize);

    private sealed record FootprintSnapshot(
        int Bar,
        DateTime BeginTime,
        DateTime EndTime,
        PriceLevelSnapshot? Poc,
        PriceLevelSnapshot? FirstPrice,
        PriceLevelSnapshot? LastPrice);

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
