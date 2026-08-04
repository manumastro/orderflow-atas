using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using ATAS.Indicators;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

public sealed class SessionLocationPriceResponseRecorder : Indicator
{
    private const string Schema = "fof-session-observation-v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly object _sync = new();
    private readonly Dictionary<CumulativeTrade, EventState> _events =
        new(ReferenceEqualityComparer.Instance);
    private ActiveSession? _activeSession;
    private int _nextEventId;
    private long _nextRawSequence;
    private bool _configurationErrorLogged;
    private bool _configurationChangedLogged;

    public SessionLocationPriceResponseRecorder()
    {
        Name = "Fabio Session Location Recorder";
    }

    [Display(Name = "Session name", GroupName = "Session", Order = 10)]
    public string SessionName { get; set; } = "Unconfigured";

    [Display(Name = "Session clock time zone", GroupName = "Session", Order = 20)]
    public string SessionClockTimeZone { get; set; } = "Unconfigured";

    [Display(Name = "Session start (HH:mm)", GroupName = "Session", Order = 30)]
    public string SessionStart { get; set; } = "";

    [Display(Name = "Session end (HH:mm)", GroupName = "Session", Order = 40)]
    public string SessionEnd { get; set; } = "";

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
            _configurationErrorLogged = false;
            _configurationChangedLogged = false;
        }

        base.OnRecalculate();
    }

    protected override void OnNewTrade(MarketDataArg trade)
    {
        if (!string.Equals(trade.DataType.ToString(), "Trade", StringComparison.Ordinal))
            return;

        if (!TryGetActiveSession(trade.Time, out var session))
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
        if (!TryGetActiveSession(trade.Time, out var session))
            return;

        var ticks = trade.Ticks.Select(tick => new TickSnapshot(
            tick.Time,
            tick.Price,
            tick.Volume,
            tick.Direction.ToString(),
            tick.DataType.ToString())).ToArray();
        var firstTickTime = ticks.Length == 0 ? trade.Time : ticks.Min(tick => tick.Time);
        var lastTickTime = ticks.Length == 0 ? trade.Time : ticks.Max(tick => tick.Time);

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
            firstTickTime,
            lastTickTime,
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

    private bool TryGetActiveSession(DateTime eventTime, out ActiveSession session)
    {
        session = null!;

        if (!TryGetRequestedConfiguration(out var configuration, out var error))
        {
            LogConfigurationErrorOnce(error);
            return false;
        }

        if (eventTime.TimeOfDay < configuration.Start || eventTime.TimeOfDay >= configuration.End)
            return false;

        var sessionStarted = false;

        lock (_sync)
        {
            if (_activeSession is null || _activeSession.Date != eventTime.Date)
            {
                _events.Clear();
                _nextEventId = 0;
                _nextRawSequence = 0;
                _configurationChangedLogged = false;
                _activeSession = new ActiveSession(eventTime.Date, configuration, eventTime);
                sessionStarted = true;
            }
            else if (_activeSession.Configuration != configuration && !_configurationChangedLogged)
            {
                _configurationChangedLogged = true;
                LogObservation(new SessionNotice(
                    Schema,
                    "configuration-changed",
                    _activeSession.ToSnapshot(),
                    DateTime.UtcNow,
                    "The active session keeps its original configuration; reload before the next session to apply changes."));
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
                "The recorder cannot prove that it was loaded before the configured session start; verify the first observed trade time in the report."));
        }

        return true;
    }

    private bool TryGetRequestedConfiguration(out SessionConfiguration configuration, out string error)
    {
        configuration = null!;

        if (string.IsNullOrWhiteSpace(SessionName) || string.Equals(SessionName, "Unconfigured", StringComparison.OrdinalIgnoreCase))
        {
            error = "Session name must be declared before recording.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(SessionClockTimeZone) || string.Equals(SessionClockTimeZone, "Unconfigured", StringComparison.OrdinalIgnoreCase))
        {
            error = "Session clock time zone must be declared before recording.";
            return false;
        }

        if (!TimeSpan.TryParseExact(SessionStart, @"hh\:mm", CultureInfo.InvariantCulture, out var start)
            || !TimeSpan.TryParseExact(SessionEnd, @"hh\:mm", CultureInfo.InvariantCulture, out var end)
            || start >= end)
        {
            error = "Session start and end must use HH:mm and describe one same-day session.";
            return false;
        }

        configuration = new SessionConfiguration(SessionName, SessionClockTimeZone, start, end);
        error = string.Empty;
        return true;
    }

    private void LogConfigurationErrorOnce(string error)
    {
        lock (_sync)
        {
            if (_configurationErrorLogged)
                return;

            _configurationErrorLogged = true;
        }

        LogObservation(new ConfigurationNotice(Schema, "configuration-invalid", DateTime.UtcNow, error));
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

    private sealed record SessionConfiguration(
        string Name,
        string ClockTimeZone,
        TimeSpan Start,
        TimeSpan End);

    private sealed record ActiveSession(
        DateTime Date,
        SessionConfiguration Configuration,
        DateTime FirstObservedTradeTime)
    {
        public SessionSnapshot ToSnapshot() => new(
            $"{Date:yyyyMMdd}-{Configuration.Name}",
            Configuration.Name,
            Configuration.ClockTimeZone,
            Configuration.Start.ToString(@"hh\:mm", CultureInfo.InvariantCulture),
            Configuration.End.ToString(@"hh\:mm", CultureInfo.InvariantCulture),
            FirstObservedTradeTime);
    }

    private sealed record SessionSnapshot(
        string SessionId,
        string Name,
        string ClockTimeZone,
        string Start,
        string End,
        DateTime FirstObservedTradeTime);

    private sealed record ConfigurationNotice(
        string Schema,
        string Type,
        DateTime ReceivedAtUtc,
        string Detail);

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
        DateTime FirstTickTime,
        DateTime LastTickTime,
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
