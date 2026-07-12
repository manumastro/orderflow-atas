using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow;

/// <summary>
/// Causal New York impulse study. It retains completed New York bars in memory
/// for profile and flow reconstruction without creating orders, positions, or PnL.
/// </summary>
internal sealed class FabioAuctionStudyModule
{
    private const string StudyMode = "NEW_YORK_IMPULSE_STUDY_NO_TRADES";
    private const string ShadowModel = "NY_IMPULSE_CUMULATIVE_CONFIRMATION_SHADOW_V1";
    private const decimal MinimumDirectionalCumulativeTrade = 30m;
    private const double ShadowPathMinutes = 30d;
    private static readonly DateOnly ProspectiveStartSessionDate = new(2026, 7, 13);
    private readonly Action<string, bool> _log;
    private readonly string _chartTimeFrame;
    private readonly SessionProfileState _newYork = new("NEW_YORK");
    private readonly Dictionary<string, AuctionBarRecord> _pendingRecords = new();
    private readonly HashSet<string> _completedRecords = new();
    private readonly Dictionary<string, int> _sessionCounts = new();
    private readonly Dictionary<string, int> _effortResultCounts = new();
    private readonly Dictionary<string, AuctionBarRecord> _historicalRecords = new();
    private readonly List<ImpulseMarker> _historicalImpulseMarkers = new();
    private readonly NewYorkImpulseState _newYorkImpulse = new();
    private readonly HashSet<string> _processedImpulseRecords = new();
    private readonly HashSet<string> _shadowPrimaryDateDirections = new();
    private readonly Dictionary<string, ImpulseShadowObservation> _shadowsByImpulse = new();
    private readonly List<ImpulseShadowObservation> _shadowObservations = new();
    private readonly List<ImpulseShadowMarker> _historicalShadowMarkers = new();
    private List<AuctionBarRecord>? _historicalRecordsByTime;
    private long _historicalCumulativeTradesSeen;
    private long _historicalCumulativeTradesMatched;
    private int _impulseReadyCount;
    private int _impulsePullbackBarCount;
    private int _impulseResolvedCount;
    private int _shadowEntryCount;
    private int _shadowProspectiveEntryCount;
    private int _shadowPathBarCount;
    private int _shadowResolutionCount;
    private int _shadowCompletePathCount;

    public FabioAuctionStudyModule(
        Action<string, bool> log,
        string chartTimeFrame)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _chartTimeFrame = string.IsNullOrWhiteSpace(chartTimeFrame) ? "UNKNOWN" : chartTimeFrame;

        _log($"[AUCTION_STATE_MODE] StudyMode={StudyMode}, Sessions=NEW_YORK, AuctionStateBars=DISABLED, Symmetry=LONG|SHORT, Profile=NEW_YORK_SESSION_PRIOR_CAUSAL, ImpulseProfile=NEW_YORK_A_TO_B_CAUSAL, LvnRanking=RAW_CAUSAL_V1, ShadowStudy={ShadowModel}, ShadowRequiredTimeFrame=M1, ShadowEnabled={(ShadowEnabled ? "TRUE" : "FALSE")}, ShadowProspectiveStart={ProspectiveStartSessionDate:yyyy-MM-dd}, ShadowPathMinutes={ShadowPathMinutes:F0}, FlowSource=CANDLE_FOOTPRINT|CUMULATIVE_TRADES, CumulativeBigTrades=AGGREGATED_PER_BAR, OperationalEntries=DISABLED, ShadowOrders=DISABLED, ChartTimeFrame={_chartTimeFrame}", false);
    }

    public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
    {
        FlushCompletedPendingRecords(bar);

        var newYorkTime = MarketTimeZones.ToNewYork(candle.Time);
        var newYorkMinutes = newYorkTime.Hour * 60 + newYorkTime.Minute;
        if (newYorkMinutes >= 9 * 60 + 30 && newYorkMinutes < 16 * 60)
            UpdateSession(_newYork, DateOnly.FromDateTime(newYorkTime), bar, currentBar, candle);
    }

    public void AppendHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> trades)
    {
        _historicalRecordsByTime ??= _historicalRecords.Values
            .OrderBy(record => record.BeginTimeUtc)
            .ThenBy(record => record.Session)
            .ToList();

        foreach (var trade in trades)
        {
            _historicalCumulativeTradesSeen++;
            var matches = FindRecordsForTrade(_historicalRecordsByTime, trade.Time);
            if (matches.Count == 0)
                continue;
            _historicalCumulativeTradesMatched++;
            foreach (var record in matches)
                record.CumulativeFlow.AddHistorical(trade);
        }
    }

    public void OnLiveCumulativeTrade(CumulativeTrade trade)
    {
        foreach (var record in _pendingRecords.Values.Where(record => trade.Time >= record.BeginTimeUtc && trade.Time <= record.EndTimeUtc))
            record.CumulativeFlow.AddOrUpdateLive(trade);
    }

    public void LogHistoricalSummary()
    {
        BuildHistoricalShadowObservations();
        var impulseEvents = _historicalImpulseMarkers.Select(marker => new HistoricalLogEvent(
            marker.Bar,
            marker.Ordinal,
            () => LogImpulseMarker(marker)));
        var shadowEvents = _historicalShadowMarkers.Select(marker => new HistoricalLogEvent(
            marker.Bar,
            marker.Ordinal,
            () => LogShadowMarker(marker)));
        foreach (var item in impulseEvents.Concat(shadowEvents).OrderBy(item => item.Bar).ThenBy(item => item.Ordinal))
            item.Write();

        _log(
            $"[AUCTION_STATE_SUMMARY] StudyMode={StudyMode}, " +
            $"LondonBars={GetCount(_sessionCounts, "LONDON")}, " +
            $"NewYorkBars={GetCount(_sessionCounts, "NEW_YORK")}, " +
            $"BuyWithResult={GetCount(_effortResultCounts, "BUY_WITH_RESULT")}, " +
            $"BuyAbsorbed={GetCount(_effortResultCounts, "BUY_ABSORBED")}, " +
            $"SellWithResult={GetCount(_effortResultCounts, "SELL_WITH_RESULT")}, " +
            $"SellAbsorbed={GetCount(_effortResultCounts, "SELL_ABSORBED")}, " +
            $"Neutral={GetCount(_effortResultCounts, "NEUTRAL")}, " +
            $"HistoricalCumulativeTradesSeen={_historicalCumulativeTradesSeen}, " +
            $"HistoricalCumulativeTradesMatched={_historicalCumulativeTradesMatched}, " +
            $"ImpulseProfilesReady={_impulseReadyCount}, " +
            $"ImpulsePullbackBars={_impulsePullbackBarCount}, " +
            $"ImpulseProfilesResolved={_impulseResolvedCount}, " +
            $"ShadowModel={ShadowModel}, " +
            $"ShadowProspectiveStart={ProspectiveStartSessionDate:yyyy-MM-dd}, " +
            $"ShadowEnabled={(ShadowEnabled ? "TRUE" : "FALSE")}, " +
            $"ShadowEntries={_shadowEntryCount}, " +
            $"ShadowProspectiveEntries={_shadowProspectiveEntryCount}, " +
            $"ShadowPathBars={_shadowPathBarCount}, " +
            $"ShadowResolutions={_shadowResolutionCount}, " +
            $"ShadowComplete30MinutePaths={_shadowCompletePathCount}, " +
            $"OperationalEntries=0, ShadowOrders=0",
            false);
    }

    private void UpdateSession(
        SessionProfileState state,
        DateOnly sessionDate,
        int bar,
        int currentBar,
        IndicatorCandle candle)
    {
        if (state.SessionDate != sessionDate)
            state.Reset(sessionDate);

        state.RemoveContribution(bar);

        var priorProfileMetrics = CalculateProfile(state.Profile);
        if (priorProfileMetrics != null)
            priorProfileMetrics.Bars = state.Contributions.Count;

        var contribution = BarContribution.From(bar, candle);
        state.AddContribution(contribution);
        var record = BuildRecord(state, contribution, priorProfileMetrics);
        record.IsHistorical = bar < currentBar - 1;

        var key = GetRecordKey(state.Name, bar);
        _pendingRecords[key] = record;
        if (bar < currentBar - 1)
            CompleteRecord(record);
    }

    private static AuctionBarRecord BuildRecord(
        SessionProfileState state,
        BarContribution current,
        ProfileMetrics? priorProfileMetrics)
    {
        var delta = current.AskVolume - current.BidVolume;
        return new AuctionBarRecord
        {
            Session = state.Name,
            SessionDate = state.SessionDate,
            Bar = current.Bar,
            BeginTimeUtc = current.BeginTimeUtc,
            EndTimeUtc = current.EndTimeUtc,
            High = current.High,
            Low = current.Low,
            Close = current.Close,
            EffortResult = ClassifyEffortResult(delta, current.Close - current.Open),
            PriorProfile = priorProfileMetrics,
            Contribution = current
        };
    }

    private void FlushCompletedPendingRecords(int currentBar)
    {
        foreach (var record in _pendingRecords.Values.Where(record => record.Bar < currentBar).ToList())
            CompleteRecord(record);
    }

    private void CompleteRecord(AuctionBarRecord record)
    {
        var key = GetRecordKey(record.Session, record.Bar);
        _pendingRecords.Remove(key);
        if (!_completedRecords.Add(key))
            return;

        Increment(_sessionCounts, record.Session);
        Increment(_effortResultCounts, record.EffortResult);
        ProcessActiveShadowPaths(record);
        if (_processedImpulseRecords.Add(key))
            ProcessNewYorkImpulse(record);
        if (record.IsHistorical)
        {
            _historicalRecords[key] = record;
            _historicalRecordsByTime = null;
        }
    }

    private void ProcessNewYorkImpulse(AuctionBarRecord record)
    {
        var previous = _newYorkImpulse.PreviousRecord;
        if (previous != null && previous.SessionDate != record.SessionDate)
        {
            if (_newYorkImpulse.Active != null)
            {
                EnsureImpulseReady(_newYorkImpulse.Active);
                ResolveImpulse(_newYorkImpulse.Active, previous, "SESSION_END");
            }
            _newYorkImpulse.Active = null;
            previous = null;
        }

        var hadActiveImpulse = _newYorkImpulse.Active != null;
        if (_newYorkImpulse.Active != null)
            ProcessActiveImpulse(_newYorkImpulse.Active, record);
        if (!hadActiveImpulse && _newYorkImpulse.Active == null && previous != null)
            TryStartImpulse(previous, record);

        _newYorkImpulse.PreviousRecord = record;
    }

    private void TryStartImpulse(AuctionBarRecord previous, AuctionBarRecord record)
    {
        var prior = record.PriorProfile;
        if (prior == null || previous.Close < prior.Val || previous.Close > prior.Vah)
            return;

        var direction = record.Close > prior.Vah && record.EffortResult == "BUY_WITH_RESULT"
            ? "LONG"
            : record.Close < prior.Val && record.EffortResult == "SELL_WITH_RESULT"
                ? "SHORT"
                : null;
        if (direction == null)
            return;

        var impulse = new ImpulseLifecycle
        {
            Id = $"{record.SessionDate:yyyy-MM-dd}:{direction}:{record.Bar}",
            SessionDate = record.SessionDate,
            Direction = direction,
            OriginBoundary = direction == "LONG" ? prior.Vah : prior.Val,
            StartBar = record.Bar,
            LastImpulseRecord = record,
            ImpulseHigh = record.High,
            ImpulseLow = record.Low
        };
        impulse.Contributions.Add(record.Contribution);
        _newYorkImpulse.Active = impulse;
    }

    private void ProcessActiveImpulse(ImpulseLifecycle impulse, AuctionBarRecord record)
    {
        if (!impulse.IsReady)
        {
            var originReentered = impulse.Direction == "LONG"
                ? record.Close <= impulse.OriginBoundary
                : record.Close >= impulse.OriginBoundary;
            var extendsExtreme = impulse.Direction == "LONG"
                ? record.High > impulse.ImpulseHigh
                : record.Low < impulse.ImpulseLow;

            if (extendsExtreme && !originReentered)
            {
                impulse.Contributions.Add(record.Contribution);
                impulse.LastImpulseRecord = record;
                impulse.ImpulseHigh = Math.Max(impulse.ImpulseHigh, record.High);
                impulse.ImpulseLow = Math.Min(impulse.ImpulseLow, record.Low);
                return;
            }

            EnsureImpulseReady(impulse);
        }

        ProcessImpulsePullback(impulse, record);
    }

    private void EnsureImpulseReady(ImpulseLifecycle impulse)
    {
        if (impulse.IsReady)
            return;

        impulse.FrozenProfile = BuildProfile(impulse.Contributions);
        impulse.FrozenMetrics = CalculateProfile(impulse.FrozenProfile);
        impulse.FrozenLvns = FindLocalVolumeMinima(impulse.FrozenProfile, impulse);
        impulse.IsReady = true;
        EmitImpulseMarker(new ImpulseMarker
        {
            Kind = "READY",
            Ordinal = 0,
            Impulse = impulse,
            Record = impulse.LastImpulseRecord
        });
    }

    private void ProcessImpulsePullback(ImpulseLifecycle impulse, AuctionBarRecord record)
    {
        impulse.PullbackOrdinal++;
        var touchedLvns = impulse.FrozenLvns
            .Where(lvn => record.Low <= lvn.Price && lvn.Price <= record.High)
            .ToList();
        var continuation = impulse.Direction == "LONG"
            ? record.High > impulse.ImpulseHigh
            : record.Low < impulse.ImpulseLow;
        var originReentered = impulse.Direction == "LONG"
            ? record.Close <= impulse.OriginBoundary
            : record.Close >= impulse.OriginBoundary;

        EmitImpulseMarker(new ImpulseMarker
        {
            Kind = "PULLBACK_BAR",
            Ordinal = 100 + impulse.PullbackOrdinal,
            Impulse = impulse,
            Record = record,
            PullbackOrdinal = impulse.PullbackOrdinal,
            FrozenRelation = GetProfileRelation(record.Close, impulse.FrozenMetrics),
            TouchedLvns = touchedLvns
        });

        // A confirmation on the resolution bar already knows the outcome and is not causal.
        if (!continuation && !originReentered)
            TryCreateLiveShadow(impulse, record, touchedLvns);
        if (!continuation && !originReentered)
            return;

        var reason = continuation && originReentered
            ? "TWO_SIDED_RANGE"
            : continuation
                ? "CONTINUATION_NEW_EXTREME"
                : "ORIGIN_REENTRY";
        ResolveImpulse(impulse, record, reason);
    }

    private void ResolveImpulse(ImpulseLifecycle impulse, AuctionBarRecord record, string reason)
    {
        EmitImpulseMarker(new ImpulseMarker
        {
            Kind = "RESOLVED",
            Ordinal = 10000,
            Impulse = impulse,
            Record = record,
            EndReason = reason
        });
        ResolveShadow(impulse, record, reason);
        _newYorkImpulse.Active = null;
    }

    private void TryCreateLiveShadow(
        ImpulseLifecycle impulse,
        AuctionBarRecord record,
        IReadOnlyCollection<LvnMetrics> touchedLvns)
    {
        if (!ShadowEnabled || record.IsHistorical || impulse.ShadowCreated || !IsShadowConfirmation(impulse, record, touchedLvns))
            return;

        var primaryKey = GetShadowPrimaryKey(impulse.SessionDate, impulse.Direction);
        if (!_shadowPrimaryDateDirections.Add(primaryKey))
            return;

        var shadow = CreateShadowObservation(impulse, record, touchedLvns);
        impulse.ShadowCreated = true;
        _shadowObservations.Add(shadow);
        _shadowsByImpulse[impulse.Id] = shadow;
        EmitShadowMarker(ImpulseShadowMarker.Entry(shadow));
    }

    private static bool IsShadowConfirmation(
        ImpulseLifecycle impulse,
        AuctionBarRecord record,
        IReadOnlyCollection<LvnMetrics> touchedLvns)
    {
        if (record.CumulativeFlow.TradeCount == 0 || touchedLvns.Count == 0)
            return false;

        var expectedResult = impulse.Direction == "LONG" ? "BUY_WITH_RESULT" : "SELL_WITH_RESULT";
        var directionalMaximum = impulse.Direction == "LONG"
            ? record.CumulativeFlow.MaxBuy
            : record.CumulativeFlow.MaxSell;
        var oppositeMaximum = impulse.Direction == "LONG"
            ? record.CumulativeFlow.MaxSell
            : record.CumulativeFlow.MaxBuy;
        return record.EffortResult == expectedResult
            && directionalMaximum >= MinimumDirectionalCumulativeTrade
            && directionalMaximum > oppositeMaximum;
    }

    private ImpulseShadowObservation CreateShadowObservation(
        ImpulseLifecycle impulse,
        AuctionBarRecord record,
        IReadOnlyCollection<LvnMetrics> touchedLvns)
    {
        var directionalMaximum = impulse.Direction == "LONG"
            ? record.CumulativeFlow.MaxBuy
            : record.CumulativeFlow.MaxSell;
        var oppositeMaximum = impulse.Direction == "LONG"
            ? record.CumulativeFlow.MaxSell
            : record.CumulativeFlow.MaxBuy;
        return new ImpulseShadowObservation
        {
            Id = $"{impulse.Id}:SHADOW",
            Impulse = impulse,
            EntryRecord = record,
            EntryPrice = record.Close,
            EvaluationCohort = impulse.SessionDate >= ProspectiveStartSessionDate
                ? "PROSPECTIVE"
                : "HISTORICAL_REFERENCE",
            TouchedLvns = touchedLvns.ToList(),
            DirectionalCumulativeMaximum = directionalMaximum,
            OppositeCumulativeMaximum = oppositeMaximum
        };
    }

    private void ProcessActiveShadowPaths(AuctionBarRecord record)
    {
        foreach (var shadow in _shadowObservations.Where(item => !item.PathComplete).ToList())
        {
            if (shadow.Impulse.SessionDate != record.SessionDate || record.Bar <= shadow.EntryRecord.Bar)
                continue;
            AddShadowPath(shadow, record);
        }
    }

    private void AddShadowPath(ImpulseShadowObservation shadow, AuctionBarRecord record)
    {
        var elapsedMinutes = (record.EndTimeUtc - shadow.EntryRecord.EndTimeUtc).TotalMinutes;
        if (elapsedMinutes <= 0)
            return;

        var favorable = shadow.Impulse.Direction == "LONG"
            ? record.High - shadow.EntryPrice
            : shadow.EntryPrice - record.Low;
        var adverse = shadow.Impulse.Direction == "LONG"
            ? shadow.EntryPrice - record.Low
            : record.High - shadow.EntryPrice;
        shadow.FavorableMfePoints = Math.Max(shadow.FavorableMfePoints, Math.Max(0m, favorable));
        shadow.AdverseMaePoints = Math.Max(shadow.AdverseMaePoints, Math.Max(0m, adverse));
        shadow.PathBarOrdinal++;
        var completesPath = elapsedMinutes >= ShadowPathMinutes;
        if (completesPath)
            shadow.PathComplete = true;

        EmitShadowMarker(ImpulseShadowMarker.Path(
            shadow,
            record,
            elapsedMinutes,
            shadow.Impulse.Direction == "LONG"
                ? record.Close - shadow.EntryPrice
                : shadow.EntryPrice - record.Close,
            completesPath));
    }

    private void ResolveShadow(ImpulseLifecycle impulse, AuctionBarRecord record, string reason)
    {
        if (!_shadowsByImpulse.TryGetValue(impulse.Id, out var shadow) || shadow.Resolved)
            return;
        shadow.Resolved = true;
        shadow.ResolutionRecord = record;
        shadow.EndReason = reason;
        EmitShadowMarker(ImpulseShadowMarker.Resolved(shadow));
    }

    private void BuildHistoricalShadowObservations()
    {
        if (!ShadowEnabled || _historicalShadowMarkers.Count > 0)
            return;

        var grouped = _historicalImpulseMarkers
            .GroupBy(marker => marker.Impulse.Id)
            .OrderBy(group => group.First().Impulse.StartBar);
        var records = _historicalRecords.Values.OrderBy(record => record.Bar).ToList();
        foreach (var group in grouped)
        {
            var markers = group.OrderBy(marker => marker.Bar).ThenBy(marker => marker.Ordinal).ToList();
            var resolution = markers.FirstOrDefault(marker => marker.Kind == "RESOLVED");
            if (resolution == null)
                continue;

            var confirmation = markers.FirstOrDefault(marker =>
                marker.Kind == "PULLBACK_BAR"
                && marker.Record.Bar < resolution.Record.Bar
                && IsShadowConfirmation(marker.Impulse, marker.Record, marker.TouchedLvns));
            if (confirmation == null)
                continue;

            var impulse = confirmation.Impulse;
            var primaryKey = GetShadowPrimaryKey(impulse.SessionDate, impulse.Direction);
            if (!_shadowPrimaryDateDirections.Add(primaryKey))
                continue;

            var shadow = CreateShadowObservation(impulse, confirmation.Record, confirmation.TouchedLvns);
            impulse.ShadowCreated = true;
            shadow.Resolved = true;
            shadow.ResolutionRecord = resolution.Record;
            shadow.EndReason = resolution.EndReason;
            _shadowObservations.Add(shadow);
            _shadowsByImpulse[impulse.Id] = shadow;
            _historicalShadowMarkers.Add(ImpulseShadowMarker.Entry(shadow));

            foreach (var record in records.Where(record =>
                         record.SessionDate == impulse.SessionDate
                         && record.Bar > confirmation.Record.Bar))
            {
                AddShadowPath(shadow, record);
                if (shadow.PathComplete)
                    break;
            }
            _historicalShadowMarkers.Add(ImpulseShadowMarker.Resolved(shadow));
        }
    }

    private bool ShadowEnabled => string.Equals(_chartTimeFrame, "M1", StringComparison.OrdinalIgnoreCase);

    private void EmitShadowMarker(ImpulseShadowMarker marker)
    {
        if (marker.Record.IsHistorical)
            _historicalShadowMarkers.Add(marker);
        else
            LogShadowMarker(marker);
    }

    private void LogShadowMarker(ImpulseShadowMarker marker)
    {
        var shadow = marker.Shadow;
        var impulse = shadow.Impulse;
        var key = $"{shadow.Id}:{marker.Kind}:{marker.Record.Bar}:{marker.Ordinal}";
        if (!_newYorkImpulse.LoggedMarkers.Add(key))
            return;

        var fields = new List<string>
        {
            $"ShadowId={shadow.Id}",
            $"ImpulseId={impulse.Id}",
            $"SessionDate={impulse.SessionDate:yyyy-MM-dd}",
            $"Direction={impulse.Direction}",
            $"ChartTimeFrame={_chartTimeFrame}",
            $"EvaluationCohort={shadow.EvaluationCohort}"
        };
        string markerName;
        if (marker.Kind == "ENTRY")
        {
            markerName = "[AUCTION_IMPULSE_SHADOW_ENTRY]";
            _shadowEntryCount++;
            if (shadow.EvaluationCohort == "PROSPECTIVE")
                _shadowProspectiveEntryCount++;
            fields.Add($"ConfirmationBar={shadow.EntryRecord.Bar}");
            fields.Add($"ConfirmationTimeItaly={MarketTimeZones.ToItaly(shadow.EntryRecord.EndTimeUtc):yyyy-MM-dd HH:mm:ss}");
            fields.Add($"ShadowEntryPrice={F(shadow.EntryPrice)}");
            fields.Add($"PullbackOrdinal={FindPullbackOrdinal(impulse, shadow.EntryRecord.Bar)}");
            fields.Add($"TouchedLvns={FormatLvns(shadow.TouchedLvns)}");
            fields.Add($"DirectionalCumulativeMax={F(shadow.DirectionalCumulativeMaximum)}");
            fields.Add($"OppositeCumulativeMax={F(shadow.OppositeCumulativeMaximum)}");
            fields.Add($"MinimumDirectionalCumulativeTrade={F(MinimumDirectionalCumulativeTrade)}");
            fields.Add("Meaning=OBSERVATION_START_NO_ORDER");
        }
        else if (marker.Kind == "PATH")
        {
            markerName = "[AUCTION_IMPULSE_SHADOW_PATH]";
            _shadowPathBarCount++;
            if (marker.CompletesPath)
                _shadowCompletePathCount++;
            var impulseRange = impulse.ImpulseHigh - impulse.ImpulseLow;
            fields.Add($"PathBarOrdinal={marker.PathBarOrdinal}");
            fields.Add($"Bar={marker.Record.Bar}");
            fields.Add($"BarEndItaly={MarketTimeZones.ToItaly(marker.Record.EndTimeUtc):yyyy-MM-dd HH:mm:ss}");
            fields.Add($"ElapsedMinutes={marker.ElapsedMinutes:F4}");
            fields.Add($"Open={F(marker.Record.Contribution.Open)}");
            fields.Add($"High={F(marker.Record.High)}");
            fields.Add($"Low={F(marker.Record.Low)}");
            fields.Add($"Close={F(marker.Record.Close)}");
            fields.Add($"DirectionalCloseMovePoints={F(marker.DirectionalCloseMovePoints)}");
            fields.Add($"FavorableMfePoints={F(marker.FavorableMfePoints)}");
            fields.Add($"AdverseMaePoints={F(marker.AdverseMaePoints)}");
            fields.Add($"DirectionalCloseMoveRanges={F(SafeDivide(marker.DirectionalCloseMovePoints, impulseRange), 4)}");
            fields.Add($"FavorableMfeRanges={F(SafeDivide(marker.FavorableMfePoints, impulseRange), 4)}");
            fields.Add($"AdverseMaeRanges={F(SafeDivide(marker.AdverseMaePoints, impulseRange), 4)}");
            fields.Add($"CumulativeTradeCoverage={(marker.Record.CumulativeFlow.TradeCount > 0 ? "AVAILABLE" : "MISSING")}");
            fields.Add($"MaxCumulativeBuy={F(marker.Record.CumulativeFlow.MaxBuy)}");
            fields.Add($"MaxCumulativeSell={F(marker.Record.CumulativeFlow.MaxSell)}");
            fields.Add($"Completes30MinutePath={(marker.CompletesPath ? "TRUE" : "FALSE")}");
        }
        else
        {
            markerName = "[AUCTION_IMPULSE_SHADOW_RESOLVED]";
            _shadowResolutionCount++;
            fields.Add($"ResolvedBar={shadow.ResolutionRecord?.Bar}");
            fields.Add($"EndReason={shadow.EndReason}");
            fields.Add($"BarsFromConfirmationToResolution={(shadow.ResolutionRecord?.Bar ?? shadow.EntryRecord.Bar) - shadow.EntryRecord.Bar}");
        }

        fields.Add("OperationalEntry=FALSE");
        fields.Add("OrderSubmitted=FALSE");
        fields.Add("PnLComputed=FALSE");
        _log($"{markerName} {string.Join(", ", fields)}", marker.Record.IsHistorical);
    }

    private int FindPullbackOrdinal(ImpulseLifecycle impulse, int bar)
    {
        return _historicalImpulseMarkers
            .FirstOrDefault(marker => marker.Impulse.Id == impulse.Id && marker.Kind == "PULLBACK_BAR" && marker.Record.Bar == bar)
            ?.PullbackOrdinal ?? impulse.PullbackOrdinal;
    }

    private static string GetShadowPrimaryKey(DateOnly sessionDate, string direction) =>
        $"{sessionDate:yyyy-MM-dd}:{direction}";

    private static decimal SafeDivide(decimal value, decimal denominator) =>
        denominator > 0 ? value / denominator : 0m;

    private void EmitImpulseMarker(ImpulseMarker marker)
    {
        if (marker.Record.IsHistorical)
            _historicalImpulseMarkers.Add(marker);
        else
            LogImpulseMarker(marker);
    }

    private void LogImpulseMarker(ImpulseMarker marker)
    {
        var key = $"{marker.Impulse.Id}:{marker.Kind}:{marker.Record.Bar}:{marker.Ordinal}";
        if (!_newYorkImpulse.LoggedMarkers.Add(key))
            return;

        var impulse = marker.Impulse;
        var profile = impulse.FrozenMetrics;
        var fields = new List<string>
        {
            $"ImpulseId={impulse.Id}",
            $"SessionDate={impulse.SessionDate:yyyy-MM-dd}",
            $"Direction={impulse.Direction}",
            $"ChartTimeFrame={_chartTimeFrame}"
        };

        string markerName;
        if (marker.Kind == "READY")
        {
            markerName = "[AUCTION_IMPULSE_READY]";
            _impulseReadyCount++;
            fields.Add($"StartBar={impulse.StartBar}");
            fields.Add($"EndBar={impulse.LastImpulseRecord.Bar}");
            fields.Add($"ImpulseBars={impulse.Contributions.Count}");
            fields.Add($"OriginBoundary={F(impulse.OriginBoundary)}");
            fields.Add($"ImpulseHigh={F(impulse.ImpulseHigh)}");
            fields.Add($"ImpulseLow={F(impulse.ImpulseLow)}");
            fields.Add($"ImpulsePOC={F(profile?.Poc)}");
            fields.Add($"ImpulseVAH={F(profile?.Vah)}");
            fields.Add($"ImpulseVAL={F(profile?.Val)}");
            fields.Add($"ImpulseLvns={FormatLvns(impulse.FrozenLvns)}");
            fields.Add($"ImpulseLvnMetrics={FormatLvnMetrics(impulse.FrozenLvns)}");
        }
        else if (marker.Kind == "PULLBACK_BAR")
        {
            markerName = "[AUCTION_IMPULSE_PULLBACK_BAR]";
            _impulsePullbackBarCount++;
            fields.Add($"PullbackOrdinal={marker.PullbackOrdinal}");
            fields.Add($"Bar={marker.Record.Bar}");
            fields.Add($"Close={F(marker.Record.Close)}");
            fields.Add($"FrozenProfileRelation={marker.FrozenRelation}");
            fields.Add($"TouchedLvns={FormatLvns(marker.TouchedLvns)}");
            fields.Add($"TouchedLvnMetrics={FormatLvnMetrics(marker.TouchedLvns)}");
            fields.Add($"EffortResult={marker.Record.EffortResult}");
            fields.Add($"CumulativeTradeCoverage={(marker.Record.CumulativeFlow.TradeCount > 0 ? "AVAILABLE" : "MISSING")}");
            fields.Add($"MaxCumulativeBuy={F(marker.Record.CumulativeFlow.MaxBuy)}");
            fields.Add($"MaxCumulativeSell={F(marker.Record.CumulativeFlow.MaxSell)}");
        }
        else
        {
            markerName = "[AUCTION_IMPULSE_RESOLVED]";
            _impulseResolvedCount++;
            fields.Add($"ResolvedBar={marker.Record.Bar}");
            fields.Add($"EndReason={marker.EndReason}");
        }

        fields.Add("OperationalEntry=FALSE");
        fields.Add("OrderSubmitted=FALSE");
        _log($"{markerName} {string.Join(", ", fields)}", marker.Record.IsHistorical);
    }

    private static List<LvnMetrics> FindLocalVolumeMinima(
        IReadOnlyDictionary<decimal, decimal> profile,
        ImpulseLifecycle impulse)
    {
        if (profile.Count < 3)
            return new List<LvnMetrics>();

        var levels = profile.OrderBy(item => item.Key).ToList();
        var maxVolume = levels.Max(item => item.Value);
        var volumePercentiles = new Dictionary<decimal, decimal>();
        var cumulativeLevels = 0;
        foreach (var group in levels.GroupBy(item => item.Value).OrderBy(group => group.Key))
        {
            cumulativeLevels += group.Count();
            volumePercentiles[group.Key] = (decimal)cumulativeLevels / levels.Count;
        }

        var leftPeaks = new decimal[levels.Count];
        var rightPeaks = new decimal[levels.Count];
        var runningPeak = levels[0].Value;
        for (var index = 1; index < levels.Count; index++)
        {
            leftPeaks[index] = runningPeak;
            runningPeak = Math.Max(runningPeak, levels[index].Value);
        }
        runningPeak = levels[^1].Value;
        for (var index = levels.Count - 2; index >= 0; index--)
        {
            rightPeaks[index] = runningPeak;
            runningPeak = Math.Max(runningPeak, levels[index].Value);
        }

        var impulseRange = impulse.ImpulseHigh - impulse.ImpulseLow;
        var directionalRange = impulse.Direction == "LONG"
            ? impulse.ImpulseHigh - impulse.OriginBoundary
            : impulse.OriginBoundary - impulse.ImpulseLow;
        var minima = new List<LvnMetrics>();
        for (var index = 1; index < levels.Count - 1; index++)
        {
            var current = levels[index];
            if (current.Value > levels[index - 1].Value || current.Value > levels[index + 1].Value)
                continue;

            var adjacentShoulder = Math.Min(levels[index - 1].Value, levels[index + 1].Value);
            var broadShoulder = Math.Min(leftPeaks[index], rightPeaks[index]);
            var prominenceVolume = Math.Max(0m, broadShoulder - current.Value);
            var positionInRange = impulseRange > 0
                ? (current.Key - impulse.ImpulseLow) / impulseRange
                : 0m;
            var directionalProgress = directionalRange > 0
                ? impulse.Direction == "LONG"
                    ? (current.Key - impulse.OriginBoundary) / directionalRange
                    : (impulse.OriginBoundary - current.Key) / directionalRange
                : 0m;

            minima.Add(new LvnMetrics
            {
                Price = current.Key,
                VolumePercentile = volumePercentiles[current.Value],
                AdjacentDepth = RelativeDepth(current.Value, adjacentShoulder),
                ShoulderDepth = RelativeDepth(current.Value, broadShoulder),
                Prominence = maxVolume > 0 ? prominenceVolume / maxVolume : 0m,
                PositionInRange = positionInRange,
                DirectionalProgress = directionalProgress,
                DistanceToPocRanges = impulseRange > 0 && impulse.FrozenMetrics != null
                    ? Math.Abs(current.Key - impulse.FrozenMetrics.Poc) / impulseRange
                    : 0m,
                DistanceToOriginRanges = impulseRange > 0
                    ? Math.Abs(current.Key - impulse.OriginBoundary) / impulseRange
                    : 0m,
                DistanceToEdgeRanges = impulseRange > 0
                    ? Math.Min(current.Key - impulse.ImpulseLow, impulse.ImpulseHigh - current.Key) / impulseRange
                    : 0m
            });
        }

        var ranked = minima
            .OrderByDescending(lvn => lvn.Prominence)
            .ThenByDescending(lvn => lvn.ShoulderDepth)
            .ThenBy(lvn => lvn.VolumePercentile)
            .ThenBy(lvn => lvn.Price)
            .ToList();
        for (var index = 0; index < ranked.Count; index++)
        {
            ranked[index].ProminenceRank = index + 1;
            ranked[index].ProminenceRankScore = ranked.Count == 1
                ? 1m
                : 1m - (decimal)index / (ranked.Count - 1);
        }
        return minima.OrderBy(lvn => lvn.Price).ToList();
    }

    private static decimal RelativeDepth(decimal volume, decimal shoulderVolume)
    {
        return shoulderVolume > 0
            ? Math.Max(0m, shoulderVolume - volume) / shoulderVolume
            : 0m;
    }

    private static string FormatLvns(IReadOnlyCollection<LvnMetrics> lvns)
    {
        return lvns.Count == 0
            ? "NONE"
            : string.Join("|", lvns.OrderBy(lvn => lvn.Price).Select(lvn => $"{F(lvn.Price)}:{F(lvn.VolumePercentile, 4)}"));
    }

    private static string FormatLvnMetrics(IReadOnlyCollection<LvnMetrics> lvns)
    {
        return lvns.Count == 0
            ? "NONE"
            : string.Join("|", lvns.OrderBy(lvn => lvn.Price).Select(lvn => string.Join(":", new[]
            {
                F(lvn.Price),
                F(lvn.VolumePercentile, 4),
                F(lvn.AdjacentDepth, 4),
                F(lvn.ShoulderDepth, 4),
                F(lvn.Prominence, 4),
                lvn.ProminenceRank.ToString(CultureInfo.InvariantCulture),
                F(lvn.ProminenceRankScore, 4),
                F(lvn.PositionInRange, 4),
                F(lvn.DirectionalProgress, 4),
                F(lvn.DistanceToPocRanges, 4),
                F(lvn.DistanceToOriginRanges, 4),
                F(lvn.DistanceToEdgeRanges, 4)
            })));
    }

    private static List<AuctionBarRecord> FindRecordsForTrade(IReadOnlyList<AuctionBarRecord> records, DateTime tradeTimeUtc)
    {
        var low = 0;
        var high = records.Count;
        while (low < high)
        {
            var middle = low + (high - low) / 2;
            if (records[middle].BeginTimeUtc <= tradeTimeUtc)
                low = middle + 1;
            else
                high = middle;
        }

        var matches = new List<AuctionBarRecord>(2);
        for (var index = low - 1; index >= 0 && records[index].EndTimeUtc >= tradeTimeUtc; index--)
        {
            var record = records[index];
            if (tradeTimeUtc >= record.BeginTimeUtc && tradeTimeUtc <= record.EndTimeUtc)
                matches.Add(record);
        }
        return matches;
    }

    private static ProfileMetrics? CalculateProfile(IReadOnlyDictionary<decimal, decimal> profile)
    {
        var totalVolume = profile.Values.Sum();
        if (profile.Count == 0 || totalVolume <= 0)
            return null;

        var maxVolume = profile.Values.Max();
        var poc = profile.Where(item => item.Value == maxVolume).Min(item => item.Key);
        var sorted = profile.OrderBy(item => item.Key).ToList();
        var pocIndex = sorted.FindIndex(item => item.Key == poc);
        var targetVolume = totalVolume * 0.70m;
        var accumulated = sorted[pocIndex].Value;
        var lower = pocIndex;
        var upper = pocIndex;

        while (accumulated < targetVolume && (lower > 0 || upper < sorted.Count - 1))
        {
            var lowerVolume = lower > 0 ? sorted[lower - 1].Value : -1m;
            var upperVolume = upper < sorted.Count - 1 ? sorted[upper + 1].Value : -1m;
            if (lowerVolume >= upperVolume && lower > 0)
            {
                lower--;
                accumulated += sorted[lower].Value;
            }
            else if (upper < sorted.Count - 1)
            {
                upper++;
                accumulated += sorted[upper].Value;
            }
            else
            {
                break;
            }
        }

        return new ProfileMetrics
        {
            Bars = 0,
            Poc = poc,
            Vah = sorted[upper].Key,
            Val = sorted[lower].Key
        };
    }

    private static Dictionary<decimal, decimal> BuildProfile(IEnumerable<BarContribution> contributions)
    {
        var profile = new Dictionary<decimal, decimal>();
        foreach (var contribution in contributions)
        {
            foreach (var level in contribution.Levels)
                profile[level.Key] = profile.GetValueOrDefault(level.Key) + level.Value;
        }
        return profile;
    }

    private static string GetProfileRelation(decimal close, ProfileMetrics? profile)
    {
        if (profile == null)
            return "NA";
        if (close > profile.Vah)
            return "ABOVE_VAH";
        if (close < profile.Val)
            return "BELOW_VAL";
        return "INSIDE_VALUE";
    }

    private static string ClassifyEffortResult(decimal delta, decimal priceChange)
    {
        if (delta > 0)
            return priceChange > 0 ? "BUY_WITH_RESULT" : "BUY_ABSORBED";
        if (delta < 0)
            return priceChange < 0 ? "SELL_WITH_RESULT" : "SELL_ABSORBED";
        return "NEUTRAL";
    }

    private static string GetRecordKey(string session, int bar) => $"{session}:{bar}";

    private static string F(decimal? value, int decimals = 2)
    {
        return value.HasValue
            ? value.Value.ToString($"F{decimals}", CultureInfo.InvariantCulture)
            : "NA";
    }

    private static int GetCount(IReadOnlyDictionary<string, int> counts, string key)
    {
        return counts.TryGetValue(key, out var count) ? count : 0;
    }

    private static void Increment(IDictionary<string, int> counts, string key)
    {
        counts[key] = counts.TryGetValue(key, out var count) ? count + 1 : 1;
    }

    private sealed class SessionProfileState
    {
        public SessionProfileState(string name) => Name = name;

        public string Name { get; }
        public DateOnly SessionDate { get; private set; }
        public Dictionary<int, BarContribution> Contributions { get; } = new();
        public Dictionary<decimal, decimal> Profile { get; } = new();

        public void Reset(DateOnly sessionDate)
        {
            SessionDate = sessionDate;
            Contributions.Clear();
            Profile.Clear();
        }

        public void RemoveContribution(int bar)
        {
            if (!Contributions.Remove(bar, out var contribution))
                return;
            foreach (var level in contribution.Levels)
            {
                var next = Profile.GetValueOrDefault(level.Key) - level.Value;
                if (next <= 0)
                    Profile.Remove(level.Key);
                else
                    Profile[level.Key] = next;
            }
        }

        public void AddContribution(BarContribution contribution)
        {
            Contributions[contribution.Bar] = contribution;
            foreach (var level in contribution.Levels)
                Profile[level.Key] = Profile.GetValueOrDefault(level.Key) + level.Value;
        }
    }

    private sealed class BarContribution
    {
        public int Bar { get; init; }
        public DateTime BeginTimeUtc { get; init; }
        public DateTime EndTimeUtc { get; init; }
        public decimal Open { get; init; }
        public decimal High { get; init; }
        public decimal Low { get; init; }
        public decimal Close { get; init; }
        public decimal BidVolume { get; init; }
        public decimal AskVolume { get; init; }
        public Dictionary<decimal, decimal> Levels { get; init; } = new();

        public static BarContribution From(int bar, IndicatorCandle candle)
        {
            var priceLevels = candle.GetAllPriceLevels().ToList();
            return new BarContribution
            {
                Bar = bar,
                BeginTimeUtc = candle.Time,
                EndTimeUtc = candle.LastTime > candle.Time ? candle.LastTime : candle.Time,
                Open = candle.Open,
                High = candle.High,
                Low = candle.Low,
                Close = candle.Close,
                BidVolume = priceLevels.Sum(level => level.Bid),
                AskVolume = priceLevels.Sum(level => level.Ask),
                Levels = priceLevels
                    .GroupBy(level => level.Price)
                    .ToDictionary(group => group.Key, group => group.Sum(level => level.Volume))
            };
        }
    }

    private sealed class AuctionBarRecord
    {
        public string Session { get; init; } = string.Empty;
        public bool IsHistorical { get; set; }
        public DateOnly SessionDate { get; init; }
        public int Bar { get; init; }
        public DateTime BeginTimeUtc { get; init; }
        public DateTime EndTimeUtc { get; init; }
        public decimal High { get; init; }
        public decimal Low { get; init; }
        public decimal Close { get; init; }
        public string EffortResult { get; init; } = string.Empty;
        public ProfileMetrics? PriorProfile { get; init; }
        public BarContribution Contribution { get; init; } = new();
        public CumulativeFlowStats CumulativeFlow { get; } = new();
    }

    private sealed class NewYorkImpulseState
    {
        public AuctionBarRecord? PreviousRecord { get; set; }
        public ImpulseLifecycle? Active { get; set; }
        public HashSet<string> LoggedMarkers { get; } = new();
    }

    private sealed class ImpulseLifecycle
    {
        public string Id { get; init; } = string.Empty;
        public DateOnly SessionDate { get; init; }
        public string Direction { get; init; } = string.Empty;
        public decimal OriginBoundary { get; init; }
        public int StartBar { get; init; }
        public AuctionBarRecord LastImpulseRecord { get; set; } = null!;
        public decimal ImpulseHigh { get; set; }
        public decimal ImpulseLow { get; set; }
        public bool IsReady { get; set; }
        public int PullbackOrdinal { get; set; }
        public List<BarContribution> Contributions { get; } = new();
        public Dictionary<decimal, decimal> FrozenProfile { get; set; } = new();
        public ProfileMetrics? FrozenMetrics { get; set; }
        public List<LvnMetrics> FrozenLvns { get; set; } = new();
        public bool ShadowCreated { get; set; }
    }

    private sealed class ImpulseMarker
    {
        public string Kind { get; init; } = string.Empty;
        public int Ordinal { get; init; }
        public int Bar => Record.Bar;
        public ImpulseLifecycle Impulse { get; init; } = null!;
        public AuctionBarRecord Record { get; init; } = null!;
        public int PullbackOrdinal { get; init; }
        public string FrozenRelation { get; init; } = string.Empty;
        public IReadOnlyCollection<LvnMetrics> TouchedLvns { get; init; } = Array.Empty<LvnMetrics>();
        public string EndReason { get; init; } = string.Empty;
    }

    private sealed class ImpulseShadowObservation
    {
        public string Id { get; init; } = string.Empty;
        public ImpulseLifecycle Impulse { get; init; } = null!;
        public AuctionBarRecord EntryRecord { get; init; } = null!;
        public decimal EntryPrice { get; init; }
        public string EvaluationCohort { get; init; } = string.Empty;
        public List<LvnMetrics> TouchedLvns { get; init; } = new();
        public decimal DirectionalCumulativeMaximum { get; init; }
        public decimal OppositeCumulativeMaximum { get; init; }
        public int PathBarOrdinal { get; set; }
        public decimal FavorableMfePoints { get; set; }
        public decimal AdverseMaePoints { get; set; }
        public bool PathComplete { get; set; }
        public bool Resolved { get; set; }
        public AuctionBarRecord? ResolutionRecord { get; set; }
        public string EndReason { get; set; } = string.Empty;
    }

    private sealed class ImpulseShadowMarker
    {
        public string Kind { get; init; } = string.Empty;
        public int Ordinal { get; init; }
        public int Bar => Record.Bar;
        public ImpulseShadowObservation Shadow { get; init; } = null!;
        public AuctionBarRecord Record { get; init; } = null!;
        public int PathBarOrdinal { get; init; }
        public double ElapsedMinutes { get; init; }
        public decimal DirectionalCloseMovePoints { get; init; }
        public decimal FavorableMfePoints { get; init; }
        public decimal AdverseMaePoints { get; init; }
        public bool CompletesPath { get; init; }

        public static ImpulseShadowMarker Entry(ImpulseShadowObservation shadow) => new()
        {
            Kind = "ENTRY",
            Ordinal = 5000,
            Shadow = shadow,
            Record = shadow.EntryRecord
        };

        public static ImpulseShadowMarker Path(
            ImpulseShadowObservation shadow,
            AuctionBarRecord record,
            double elapsedMinutes,
            decimal directionalCloseMovePoints,
            bool completesPath) => new()
        {
            Kind = "PATH",
            Ordinal = -100,
            Shadow = shadow,
            Record = record,
            PathBarOrdinal = shadow.PathBarOrdinal,
            ElapsedMinutes = elapsedMinutes,
            DirectionalCloseMovePoints = directionalCloseMovePoints,
            FavorableMfePoints = shadow.FavorableMfePoints,
            AdverseMaePoints = shadow.AdverseMaePoints,
            CompletesPath = completesPath
        };

        public static ImpulseShadowMarker Resolved(ImpulseShadowObservation shadow) => new()
        {
            Kind = "RESOLVED",
            Ordinal = 11000,
            Shadow = shadow,
            Record = shadow.ResolutionRecord!
        };
    }

    private sealed class HistoricalLogEvent
    {
        public HistoricalLogEvent(int bar, int ordinal, Action write)
        {
            Bar = bar;
            Ordinal = ordinal;
            Write = write;
        }

        public int Bar { get; }
        public int Ordinal { get; }
        public Action Write { get; }
    }

    private sealed class CumulativeFlowStats
    {
        private readonly Dictionary<string, (TradeDirection Direction, decimal Volume)> _liveTrades = new();
        private int _historicalTradeCount;

        public int TradeCount => _historicalTradeCount + _liveTrades.Count;
        public decimal MaxBuy { get; private set; }
        public decimal MaxSell { get; private set; }

        public void AddHistorical(CumulativeTrade trade)
        {
            _historicalTradeCount++;
            AddVolume(trade.Direction, trade.Volume);
        }

        public void AddOrUpdateLive(CumulativeTrade trade)
        {
            var key = $"{trade.Time.Ticks}:{trade.Direction}:{trade.FirstPrice:F2}";
            var recalculateBuyMax = false;
            var recalculateSellMax = false;
            if (_liveTrades.TryGetValue(key, out var previous))
            {
                if (previous.Direction == TradeDirection.Buy)
                    recalculateBuyMax = previous.Volume >= MaxBuy && trade.Volume < previous.Volume;
                else if (previous.Direction == TradeDirection.Sell)
                    recalculateSellMax = previous.Volume >= MaxSell && trade.Volume < previous.Volume;
            }

            _liveTrades[key] = (trade.Direction, trade.Volume);
            AddVolume(trade.Direction, trade.Volume);
            if (recalculateBuyMax)
                MaxBuy = MaximumLiveVolume(TradeDirection.Buy);
            if (recalculateSellMax)
                MaxSell = MaximumLiveVolume(TradeDirection.Sell);
        }

        private void AddVolume(TradeDirection direction, decimal volume)
        {
            if (direction == TradeDirection.Buy)
                MaxBuy = Math.Max(MaxBuy, volume);
            else if (direction == TradeDirection.Sell)
                MaxSell = Math.Max(MaxSell, volume);
        }

        private decimal MaximumLiveVolume(TradeDirection direction)
        {
            return _liveTrades.Values
                .Where(value => value.Direction == direction)
                .Select(value => value.Volume)
                .DefaultIfEmpty(0m)
                .Max();
        }
    }

    private sealed class ProfileMetrics
    {
        public int Bars { get; set; }
        public decimal Poc { get; init; }
        public decimal Vah { get; init; }
        public decimal Val { get; init; }
    }

    private sealed class LvnMetrics
    {
        public decimal Price { get; init; }
        public decimal VolumePercentile { get; init; }
        public decimal AdjacentDepth { get; init; }
        public decimal ShoulderDepth { get; init; }
        public decimal Prominence { get; init; }
        public int ProminenceRank { get; set; }
        public decimal ProminenceRankScore { get; set; }
        public decimal PositionInRange { get; init; }
        public decimal DirectionalProgress { get; init; }
        public decimal DistanceToPocRanges { get; init; }
        public decimal DistanceToOriginRanges { get; init; }
        public decimal DistanceToEdgeRanges { get; init; }
    }
}
