using System.ComponentModel;
using System.Text.Json;
using ATAS.Indicators;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

[DisplayName("Fabio Session Location Recorder")]
public sealed class SessionLocationPriceResponseRecorder : Indicator
{
    private const string Schema = "fof-session-observation-v2";
    private const string SessionName = "NQ US Cash";
    private const string SessionClockTimeZone = "America/New_York";
    private const string SessionStartText = "09:30";
    private const string SessionEndText = "16:00";
    private static readonly TimeSpan SessionStart = new(9, 30, 0);
    private static readonly TimeSpan SessionEnd = new(16, 0, 0);
    private static readonly TimeZoneInfo NewYorkTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly object _sync = new();
    private readonly Dictionary<CumulativeTrade, EventState> _events =
        new(ReferenceEqualityComparer.Instance);
    private ActiveSession? _activeSession;
    private int _nextEventId;
    private long _nextRawSequence;

    public SessionLocationPriceResponseRecorder()
    {
        Name = "Fabio Session Location Recorder";
    }

    protected override void OnCalculate(int bar, decimal value)
    {
    }

    protected override void OnRecalculate()
    {
        lock (_sync)
        {
            _events.Clear();
            _activeSession = null;
            _nextEventId = 0;
            _nextRawSequence = 0;
        }

        base.OnRecalculate();
    }

    protected override void OnNewTrade(MarketDataArg trade)
    {
        if (!string.Equals(trade.DataType.ToString(), "Trade", StringComparison.Ordinal))
            return;

        if (!TryGetActiveSession(trade.Time, out var session, out var sessionTime))
            return;

        long sequence;
        lock (_sync)
            sequence = ++_nextRawSequence;

        LogObservation(new RawTradeObservation(
            Schema,
            "raw-trade",
            session.ToSnapshot(),
            sequence,
            trade.Time,
            sessionTime,
            DateTime.UtcNow,
            trade.Price,
            trade.Volume,
            trade.Direction.ToString(),
            trade.DataType.ToString()));
    }

    protected override void OnCumulativeTrade(CumulativeTrade trade)
    {
        RecordCumulativeTrade(trade, "cumulative-new");
    }

    protected override void OnUpdateCumulativeTrade(CumulativeTrade trade)
    {
        RecordCumulativeTrade(trade, "cumulative-update");
    }

    private void RecordCumulativeTrade(CumulativeTrade trade, string source)
    {
        if (!TryGetActiveSession(trade.Time, out var session, out var eventSessionTime))
            return;

        var ticks = trade.Ticks.Select(tick => new TickSnapshot(
            tick.Time,
            tick.Price,
            tick.Volume,
            tick.Direction.ToString(),
            tick.DataType.ToString())).ToArray();
        var firstTickTime = ticks.Length == 0 ? trade.Time : ticks.Min(tick => tick.Time);
        var lastTickTime = ticks.Length == 0 ? trade.Time : ticks.Max(tick => tick.Time);
        var firstTickSessionTime = ToNewYorkTime(firstTickTime);
        var lastTickSessionTime = ToNewYorkTime(lastTickTime);

        EventState state;
        decimal incrementalVolume;
        int updateNumber;

        lock (_sync)
        {
            if (!_events.TryGetValue(trade, out state!))
            {
                state = new EventState(++_nextEventId);
                _events.Add(trade, state);
            }

            incrementalVolume = trade.Volume - state.ReportedVolume;
            state.ReportedVolume = trade.Volume;
            updateNumber = ++state.Updates;
        }

        var security = TradingManager?.Security;

        LogObservation(new CumulativeTradeObservation(
            Schema,
            source,
            session.ToSnapshot(),
            state.EventId,
            updateNumber,
            trade.Time,
            eventSessionTime,
            firstTickTime,
            firstTickSessionTime,
            lastTickTime,
            lastTickSessionTime,
            DateTime.UtcNow,
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
                security?.TickSize)));
    }

    private bool TryGetActiveSession(DateTime rawTime, out ActiveSession session, out DateTime sessionTime)
    {
        session = null!;
        sessionTime = ToNewYorkTime(rawTime);

        if (sessionTime.TimeOfDay < SessionStart || sessionTime.TimeOfDay >= SessionEnd)
            return false;

        var sessionStarted = false;

        lock (_sync)
        {
            if (_activeSession is null || _activeSession.Date != sessionTime.Date)
            {
                _events.Clear();
                _nextEventId = 0;
                _nextRawSequence = 0;
                _activeSession = new ActiveSession(sessionTime.Date, rawTime, sessionTime);
                sessionStarted = true;
            }

            session = _activeSession;
        }

        if (sessionStarted)
        {
            LogObservation(new SessionNotice(
                Schema,
                "session-first-trade-observed",
                session.ToSnapshot(),
                DateTime.UtcNow,
                "Raw feed time is interpreted as UTC and converted to America/New_York; verify that the first observed session time is not later than 09:30."));
        }

        return true;
    }

    private static DateTime ToNewYorkTime(DateTime rawTime)
    {
        var utcTime = rawTime.Kind == DateTimeKind.Utc
            ? rawTime
            : DateTime.SpecifyKind(rawTime, DateTimeKind.Utc);

        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, NewYorkTimeZone);
    }

    private void LogObservation<T>(T observation)
    {
        try
        {
            this.LogInfo("FofSessionObservation {0}", JsonSerializer.Serialize(observation, JsonOptions));
        }
        catch (Exception exception)
        {
            this.LogError("FofSessionObservation serialization failed.", exception);
        }
    }

    private sealed class EventState(int eventId)
    {
        public int EventId { get; } = eventId;
        public decimal ReportedVolume { get; set; }
        public int Updates { get; set; }
    }

    private sealed record ActiveSession(
        DateTime Date,
        DateTime FirstObservedRawTradeTime,
        DateTime FirstObservedSessionTime)
    {
        public SessionSnapshot ToSnapshot() => new(
            $"{Date:yyyyMMdd}-{SessionName}",
            SessionName,
            SessionClockTimeZone,
            SessionStartText,
            SessionEndText,
            FirstObservedRawTradeTime,
            FirstObservedSessionTime);
    }

    private sealed record SessionSnapshot(
        string SessionId,
        string Name,
        string ClockTimeZone,
        string Start,
        string End,
        DateTime FirstObservedRawTradeTime,
        DateTime FirstObservedSessionTime);

    private sealed record SessionNotice(
        string Schema,
        string Type,
        SessionSnapshot Session,
        DateTime ReceivedAtUtc,
        string Detail);

    private sealed record RawTradeObservation(
        string Schema,
        string Source,
        SessionSnapshot Session,
        long Sequence,
        DateTime Time,
        DateTime SessionTime,
        DateTime ReceivedAtUtc,
        decimal Price,
        decimal Volume,
        string Direction,
        string DataType);

    private sealed record CumulativeTradeObservation(
        string Schema,
        string Source,
        SessionSnapshot Session,
        int EventId,
        int UpdateNumber,
        DateTime EventTime,
        DateTime EventSessionTime,
        DateTime FirstTickTime,
        DateTime FirstTickSessionTime,
        DateTime LastTickTime,
        DateTime LastTickSessionTime,
        DateTime ReceivedAtUtc,
        string Direction,
        decimal TotalVolume,
        decimal IncrementalVolume,
        decimal FirstPrice,
        decimal LastPrice,
        IReadOnlyList<TickSnapshot> Ticks,
        SecuritySnapshot Security);

    private sealed record SecuritySnapshot(
        string? SecurityId,
        string? ConnectorId,
        string? Code,
        string? Instrument,
        string? Exchange,
        decimal? TickSize);

    private sealed record TickSnapshot(
        DateTime Time,
        decimal Price,
        decimal Volume,
        string Direction,
        string DataType);
}
