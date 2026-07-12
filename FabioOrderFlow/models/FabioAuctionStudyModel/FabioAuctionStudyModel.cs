using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow;

/// <summary>
/// Broad causal ledger for the two transcript playbooks. It records completed
/// London and New York bars without creating setups, orders, positions, or PnL.
/// </summary>
internal sealed class FabioAuctionStudyModule
{
    private const string StudyMode = "DUAL_SESSION_AUCTION_STATE_LEDGER_NO_TRADES";
    private const int RollingProfileBars = 12;

    private readonly Action<string, bool> _log;
    private readonly string _chartTimeFrame;
    private readonly SessionProfileState _london = new("LONDON");
    private readonly SessionProfileState _newYork = new("NEW_YORK");
    private readonly Dictionary<string, AuctionBarRecord> _pendingRecords = new();
    private readonly HashSet<string> _loggedRecords = new();
    private readonly Dictionary<string, int> _sessionCounts = new();
    private readonly Dictionary<string, int> _effortResultCounts = new();
    private readonly Dictionary<string, AuctionBarRecord> _historicalRecords = new();
    private readonly List<ImpulseMarker> _historicalImpulseMarkers = new();
    private readonly NewYorkImpulseState _newYorkImpulse = new();
    private readonly HashSet<string> _processedImpulseRecords = new();
    private List<AuctionBarRecord>? _historicalRecordsByTime;
    private long _historicalCumulativeTradesSeen;
    private long _historicalCumulativeTradesMatched;
    private int _impulseReadyCount;
    private int _impulsePullbackBarCount;
    private int _impulseResolvedCount;

    public FabioAuctionStudyModule(
        Action<string, bool> log,
        string chartTimeFrame)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _chartTimeFrame = string.IsNullOrWhiteSpace(chartTimeFrame) ? "UNKNOWN" : chartTimeFrame;

        _log($"[AUCTION_STATE_MODE] StudyMode={StudyMode}, Sessions=LONDON|NEW_YORK, Symmetry=HIGH_LONG|LOW_SHORT|HIGH_SHORT|LOW_LONG, Profile=CURRENT_SESSION_CAUSAL, RollingProfileBars={RollingProfileBars}, ImpulseProfile=NEW_YORK_A_TO_B_CAUSAL, FlowSource=CANDLE_FOOTPRINT|CUMULATIVE_TRADES, CumulativeBigTrades=AGGREGATED_PER_BAR, OperationalEntries=DISABLED, ShadowOrders=DISABLED, ChartTimeFrame={_chartTimeFrame}", false);
    }

    public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
    {
        FlushCompletedPendingRecords(bar);

        var londonTime = MarketTimeZones.ToLondon(candle.Time);
        if (londonTime.Hour >= 8 && londonTime.Hour < 16)
            UpdateSession(_london, DateOnly.FromDateTime(londonTime), bar, currentBar, candle);

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
        foreach (var record in _historicalRecords.Values.OrderBy(record => record.Bar).ThenBy(record => record.Session))
            LogRecord(record);
        foreach (var marker in _historicalImpulseMarkers.OrderBy(marker => marker.Bar).ThenBy(marker => marker.Ordinal))
            LogImpulseMarker(marker);

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

        var priorContributions = state.Contributions.Values
            .Where(contribution => contribution.Bar < bar)
            .OrderBy(contribution => contribution.Bar)
            .ToList();
        var priorProfile = state.Profile.Count > 0
            ? new Dictionary<decimal, decimal>(state.Profile)
            : new Dictionary<decimal, decimal>();
        var priorProfileMetrics = CalculateProfile(priorProfile);
        if (priorProfileMetrics != null)
            priorProfileMetrics.Bars = priorContributions.Count;
        var rollingContributions = priorContributions.TakeLast(RollingProfileBars).ToList();
        var rollingProfile = BuildProfile(rollingContributions);
        var rollingProfileMetrics = CalculateProfile(rollingProfile);
        if (rollingProfileMetrics != null)
            rollingProfileMetrics.Bars = rollingContributions.Count;

        var contribution = BarContribution.From(bar, candle);
        state.AddContribution(contribution);
        var developingProfileMetrics = CalculateProfile(state.Profile);
        if (developingProfileMetrics != null)
            developingProfileMetrics.Bars = state.Contributions.Count;

        var record = BuildRecord(
            state,
            contribution,
            priorContributions,
            priorProfile,
            priorProfileMetrics,
            rollingContributions,
            rollingProfile,
            rollingProfileMetrics,
            developingProfileMetrics);
        record.IsHistorical = bar < currentBar - 1;

        var key = GetRecordKey(state.Name, bar);
        _pendingRecords[key] = record;
        if (bar < currentBar - 1)
            CompleteRecord(record);
    }

    private AuctionBarRecord BuildRecord(
        SessionProfileState state,
        BarContribution current,
        IReadOnlyList<BarContribution> priorContributions,
        IReadOnlyDictionary<decimal, decimal> priorProfile,
        ProfileMetrics? priorProfileMetrics,
        IReadOnlyList<BarContribution> rollingContributions,
        IReadOnlyDictionary<decimal, decimal> rollingProfile,
        ProfileMetrics? rollingProfileMetrics,
        ProfileMetrics? developingProfileMetrics)
    {
        var range = Math.Max(current.High - current.Low, 0m);
        var priceChange = current.Close - current.Open;
        var delta = current.AskVolume - current.BidVolume;
        var deltaImbalance = current.Volume > 0 ? delta / current.Volume : 0m;
        var closeLocation = range > 0 ? (current.Close - current.Low) / range : 0.5m;
        var effortResult = ClassifyEffortResult(delta, priceChange);
        var priorRelation = GetProfileRelation(current.Close, priorProfileMetrics);
        var developingRelation = GetProfileRelation(current.Close, developingProfileMetrics);
        var priorSix = priorContributions.TakeLast(6).ToList();
        var priorTwelve = priorContributions.TakeLast(12).ToList();
        var priorMedianRange = Median(priorTwelve.Select(item => item.High - item.Low));
        var rangeToPriorMedian = priorMedianRange > 0 ? range / priorMedianRange : (decimal?)null;
        var overlapWithPrevious = priorContributions.Count > 0
            ? CalculateOverlap(current, priorContributions[^1])
            : (decimal?)null;

        return new AuctionBarRecord
        {
            Session = state.Name,
            SessionDate = state.SessionDate,
            SessionBarOrdinal = state.Contributions.Count,
            Bar = current.Bar,
            BeginTimeUtc = current.BeginTimeUtc,
            EndTimeUtc = current.EndTimeUtc,
            Open = current.Open,
            High = current.High,
            Low = current.Low,
            Close = current.Close,
            Volume = current.Volume,
            BidVolume = current.BidVolume,
            AskVolume = current.AskVolume,
            Delta = delta,
            DeltaImbalance = deltaImbalance,
            MaxBidAtPrice = current.MaxBidAtPrice,
            MaxAskAtPrice = current.MaxAskAtPrice,
            PriceChange = priceChange,
            CloseLocation = closeLocation,
            EffortResult = effortResult,
            PriorProfile = priorProfileMetrics,
            DevelopingProfile = developingProfileMetrics,
            PriorRelation = priorRelation,
            DevelopingRelation = developingRelation,
            PriorSix = CalculatePathMetrics(priorSix),
            PriorTwelve = CalculatePathMetrics(priorTwelve),
            RangeToPriorMedian = rangeToPriorMedian,
            OverlapWithPrevious = overlapWithPrevious,
            PriorSessionLvnBelow = FindNearestLocalVolumeMinimum(priorProfile, current.Close, false),
            PriorSessionLvnAbove = FindNearestLocalVolumeMinimum(priorProfile, current.Close, true),
            PriorRollingLvnBelow = FindNearestLocalVolumeMinimum(rollingProfile, current.Close, false),
            PriorRollingLvnAbove = FindNearestLocalVolumeMinimum(rollingProfile, current.Close, true),
            RollingProfile = rollingProfileMetrics,
            RollingBars = rollingContributions.Count,
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
        if (record.Session == "NEW_YORK" && _processedImpulseRecords.Add(key))
            ProcessNewYorkImpulse(record);
        if (record.IsHistorical)
        {
            _historicalRecords[key] = record;
            _historicalRecordsByTime = null;
            return;
        }
        LogRecord(record);
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
        impulse.FrozenLvns = FindLocalVolumeMinima(impulse.FrozenProfile);
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

        var continuation = impulse.Direction == "LONG"
            ? record.High > impulse.ImpulseHigh
            : record.Low < impulse.ImpulseLow;
        var originReentered = impulse.Direction == "LONG"
            ? record.Close <= impulse.OriginBoundary
            : record.Close >= impulse.OriginBoundary;
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
        _newYorkImpulse.Active = null;
    }

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
        var common = new List<string>
        {
            $"ImpulseId={impulse.Id}",
            $"SessionDate={impulse.SessionDate:yyyy-MM-dd}",
            $"Direction={impulse.Direction}",
            $"ChartTimeFrame={_chartTimeFrame}",
            $"StartBar={impulse.StartBar}",
            $"EndBar={impulse.LastImpulseRecord.Bar}",
            $"ImpulseBars={impulse.Contributions.Count}",
            $"OriginBoundary={F(impulse.OriginBoundary)}",
            $"ImpulseHigh={F(impulse.ImpulseHigh)}",
            $"ImpulseLow={F(impulse.ImpulseLow)}",
            $"ImpulsePOC={F(profile?.Poc)}",
            $"ImpulseVAH={F(profile?.Vah)}",
            $"ImpulseVAL={F(profile?.Val)}",
            $"ImpulseLvns={FormatLvns(impulse.FrozenLvns)}"
        };

        string markerName;
        if (marker.Kind == "READY")
        {
            markerName = "[AUCTION_IMPULSE_READY]";
            _impulseReadyCount++;
        }
        else if (marker.Kind == "PULLBACK_BAR")
        {
            markerName = "[AUCTION_IMPULSE_PULLBACK_BAR]";
            _impulsePullbackBarCount++;
            common.Add($"PullbackOrdinal={marker.PullbackOrdinal}");
            common.Add($"Bar={marker.Record.Bar}");
            common.Add($"Close={F(marker.Record.Close)}");
            common.Add($"FrozenProfileRelation={marker.FrozenRelation}");
            common.Add($"TouchedLvns={FormatLvns(marker.TouchedLvns)}");
            common.Add($"EffortResult={marker.Record.EffortResult}");
            common.Add($"CumulativeTradeCoverage={(marker.Record.CumulativeFlow.TradeCount > 0 ? "AVAILABLE" : "MISSING")}");
            common.Add($"MaxCumulativeBuy={F(marker.Record.CumulativeFlow.MaxBuy)}");
            common.Add($"MaxCumulativeSell={F(marker.Record.CumulativeFlow.MaxSell)}");
        }
        else
        {
            markerName = "[AUCTION_IMPULSE_RESOLVED]";
            _impulseResolvedCount++;
            common.Add($"ResolvedBar={marker.Record.Bar}");
            common.Add($"EndReason={marker.EndReason}");
        }

        common.Add("OperationalEntry=FALSE");
        common.Add("OrderSubmitted=FALSE");
        _log($"{markerName} {string.Join(", ", common)}", marker.Record.IsHistorical);
    }

    private static List<LvnMetrics> FindLocalVolumeMinima(IReadOnlyDictionary<decimal, decimal> profile)
    {
        if (profile.Count < 3)
            return new List<LvnMetrics>();

        var levels = profile.OrderBy(item => item.Key).ToList();
        var minima = new List<LvnMetrics>();
        for (var index = 1; index < levels.Count - 1; index++)
        {
            var current = levels[index];
            if (current.Value > levels[index - 1].Value || current.Value > levels[index + 1].Value)
                continue;
            minima.Add(new LvnMetrics
            {
                Price = current.Key,
                VolumePercentile = (decimal)profile.Values.Count(volume => volume <= current.Value) / profile.Count
            });
        }
        return minima;
    }

    private static string FormatLvns(IReadOnlyCollection<LvnMetrics> lvns)
    {
        return lvns.Count == 0
            ? "NONE"
            : string.Join("|", lvns.OrderBy(lvn => lvn.Price).Select(lvn => $"{F(lvn.Price)}:{F(lvn.VolumePercentile, 4)}"));
    }

    private void LogRecord(AuctionBarRecord record)
    {
        var key = GetRecordKey(record.Session, record.Bar);
        if (!_loggedRecords.Add(key))
            return;

        _pendingRecords.Remove(key);
        Increment(_sessionCounts, record.Session);
        Increment(_effortResultCounts, record.EffortResult);

        var message = string.Join(", ", new[]
        {
            "[AUCTION_STATE_BAR]",
            $"StudyMode={StudyMode}",
            $"Session={record.Session}",
            $"SessionDate={record.SessionDate:yyyy-MM-dd}",
            $"SessionBarOrdinal={record.SessionBarOrdinal}",
            $"Bar={record.Bar}",
            $"ChartTimeFrame={_chartTimeFrame}",
            MarketTimeZones.FormatUtcLondonItaly(record.EndTimeUtc),
            $"NewYork={MarketTimeZones.ToNewYork(record.EndTimeUtc):yyyy-MM-dd HH:mm:ss}",
            $"Open={F(record.Open)}",
            $"High={F(record.High)}",
            $"Low={F(record.Low)}",
            $"Close={F(record.Close)}",
            $"CandleVolume={F(record.Volume)}",
            $"BidVolume={F(record.BidVolume)}",
            $"AskVolume={F(record.AskVolume)}",
            $"Delta={F(record.Delta)}",
            $"DeltaImbalance={F(record.DeltaImbalance, 6)}",
            $"MaxBidAtPrice={F(record.MaxBidAtPrice)}",
            $"MaxAskAtPrice={F(record.MaxAskAtPrice)}",
            $"CumulativeTradeCount={record.CumulativeFlow.TradeCount}",
            $"CumulativeBuyVolume={F(record.CumulativeFlow.BuyVolume)}",
            $"CumulativeSellVolume={F(record.CumulativeFlow.SellVolume)}",
            $"CumulativeDelta={F(record.CumulativeFlow.BuyVolume - record.CumulativeFlow.SellVolume)}",
            $"MaxCumulativeBuy={F(record.CumulativeFlow.MaxBuy)}",
            $"MaxCumulativeSell={F(record.CumulativeFlow.MaxSell)}",
            $"CumulativeTradeCoverage={(record.CumulativeFlow.TradeCount > 0 ? "AVAILABLE" : "MISSING")}",
            $"PriceChange={F(record.PriceChange)}",
            $"CloseLocation={F(record.CloseLocation, 4)}",
            $"EffortResult={record.EffortResult}",
            $"PriorProfileBars={record.PriorProfile?.Bars ?? 0}",
            $"PriorPOC={F(record.PriorProfile?.Poc)}",
            $"PriorVAH={F(record.PriorProfile?.Vah)}",
            $"PriorVAL={F(record.PriorProfile?.Val)}",
            $"PriorProfileRelation={record.PriorRelation}",
            $"DevelopingPOC={F(record.DevelopingProfile?.Poc)}",
            $"DevelopingVAH={F(record.DevelopingProfile?.Vah)}",
            $"DevelopingVAL={F(record.DevelopingProfile?.Val)}",
            $"DevelopingProfileRelation={record.DevelopingRelation}",
            $"Prior6Range={F(record.PriorSix?.Range)}",
            $"Prior6Efficiency={F(record.PriorSix?.DirectionalEfficiency, 4)}",
            $"Prior12Range={F(record.PriorTwelve?.Range)}",
            $"Prior12Efficiency={F(record.PriorTwelve?.DirectionalEfficiency, 4)}",
            $"RangeToPrior12Median={F(record.RangeToPriorMedian, 4)}",
            $"OverlapWithPrevious={F(record.OverlapWithPrevious, 4)}",
            $"RollingProfileBars={record.RollingBars}",
            $"RollingPOC={F(record.RollingProfile?.Poc)}",
            $"RollingVAH={F(record.RollingProfile?.Vah)}",
            $"RollingVAL={F(record.RollingProfile?.Val)}",
            $"PriorSessionLvnBelow={F(record.PriorSessionLvnBelow?.Price)}",
            $"PriorSessionLvnBelowVolumePercentile={F(record.PriorSessionLvnBelow?.VolumePercentile, 4)}",
            $"PriorSessionLvnAbove={F(record.PriorSessionLvnAbove?.Price)}",
            $"PriorSessionLvnAboveVolumePercentile={F(record.PriorSessionLvnAbove?.VolumePercentile, 4)}",
            $"PriorRollingLvnBelow={F(record.PriorRollingLvnBelow?.Price)}",
            $"PriorRollingLvnBelowVolumePercentile={F(record.PriorRollingLvnBelow?.VolumePercentile, 4)}",
            $"PriorRollingLvnAbove={F(record.PriorRollingLvnAbove?.Price)}",
            $"PriorRollingLvnAboveVolumePercentile={F(record.PriorRollingLvnAbove?.VolumePercentile, 4)}",
            "FlowSource=CANDLE_FOOTPRINT|CUMULATIVE_TRADES",
            "OperationalEntry=FALSE",
            "OrderSubmitted=FALSE"
        });

        _log(message, record.IsHistorical);
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

    private static PathMetrics? CalculatePathMetrics(IReadOnlyList<BarContribution> contributions)
    {
        if (contributions.Count == 0)
            return null;

        var high = contributions.Max(item => item.High);
        var low = contributions.Min(item => item.Low);
        var path = 0m;
        var previousClose = contributions[0].Open;
        foreach (var contribution in contributions)
        {
            path += Math.Abs(contribution.Close - previousClose);
            previousClose = contribution.Close;
        }

        return new PathMetrics
        {
            Range = high - low,
            DirectionalEfficiency = path > 0
                ? Math.Abs(contributions[^1].Close - contributions[0].Open) / path
                : 0m
        };
    }

    private static LvnMetrics? FindNearestLocalVolumeMinimum(
        IReadOnlyDictionary<decimal, decimal> profile,
        decimal referencePrice,
        bool above)
    {
        if (profile.Count < 3)
            return null;

        var levels = profile.OrderBy(item => item.Key).ToList();
        var candidates = new List<KeyValuePair<decimal, decimal>>();
        for (var i = 1; i < levels.Count - 1; i++)
        {
            var current = levels[i];
            if (current.Value > levels[i - 1].Value || current.Value > levels[i + 1].Value)
                continue;
            if (above ? current.Key <= referencePrice : current.Key >= referencePrice)
                continue;
            candidates.Add(current);
        }

        if (candidates.Count == 0)
            return null;

        var nearest = candidates
            .OrderBy(item => Math.Abs(item.Key - referencePrice))
            .ThenBy(item => item.Value)
            .First();
        var percentile = (decimal)profile.Values.Count(volume => volume <= nearest.Value) / profile.Count;
        return new LvnMetrics { Price = nearest.Key, VolumePercentile = percentile };
    }

    private static decimal? CalculateOverlap(BarContribution current, BarContribution previous)
    {
        var currentRange = current.High - current.Low;
        if (currentRange <= 0)
            return null;
        var overlap = Math.Max(0m, Math.Min(current.High, previous.High) - Math.Max(current.Low, previous.Low));
        return overlap / currentRange;
    }

    private static decimal Median(IEnumerable<decimal> values)
    {
        var sorted = values.OrderBy(value => value).ToList();
        if (sorted.Count == 0)
            return 0m;
        var middle = sorted.Count / 2;
        return sorted.Count % 2 == 0
            ? (sorted[middle - 1] + sorted[middle]) / 2m
            : sorted[middle];
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
        public decimal Volume { get; init; }
        public decimal BidVolume { get; init; }
        public decimal AskVolume { get; init; }
        public decimal MaxBidAtPrice { get; init; }
        public decimal MaxAskAtPrice { get; init; }
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
                Volume = candle.Volume,
                BidVolume = priceLevels.Sum(level => level.Bid),
                AskVolume = priceLevels.Sum(level => level.Ask),
                MaxBidAtPrice = priceLevels.Count > 0 ? priceLevels.Max(level => level.Bid) : 0m,
                MaxAskAtPrice = priceLevels.Count > 0 ? priceLevels.Max(level => level.Ask) : 0m,
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
        public int SessionBarOrdinal { get; init; }
        public int Bar { get; init; }
        public DateTime BeginTimeUtc { get; init; }
        public DateTime EndTimeUtc { get; init; }
        public decimal Open { get; init; }
        public decimal High { get; init; }
        public decimal Low { get; init; }
        public decimal Close { get; init; }
        public decimal Volume { get; init; }
        public decimal BidVolume { get; init; }
        public decimal AskVolume { get; init; }
        public decimal Delta { get; init; }
        public decimal DeltaImbalance { get; init; }
        public decimal MaxBidAtPrice { get; init; }
        public decimal MaxAskAtPrice { get; init; }
        public decimal PriceChange { get; init; }
        public decimal CloseLocation { get; init; }
        public string EffortResult { get; init; } = string.Empty;
        public ProfileMetrics? PriorProfile { get; init; }
        public ProfileMetrics? DevelopingProfile { get; init; }
        public string PriorRelation { get; init; } = string.Empty;
        public string DevelopingRelation { get; init; } = string.Empty;
        public PathMetrics? PriorSix { get; init; }
        public PathMetrics? PriorTwelve { get; init; }
        public decimal? RangeToPriorMedian { get; init; }
        public decimal? OverlapWithPrevious { get; init; }
        public ProfileMetrics? RollingProfile { get; init; }
        public int RollingBars { get; init; }
        public LvnMetrics? PriorSessionLvnBelow { get; init; }
        public LvnMetrics? PriorSessionLvnAbove { get; init; }
        public LvnMetrics? PriorRollingLvnBelow { get; init; }
        public LvnMetrics? PriorRollingLvnAbove { get; init; }
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

    private sealed class CumulativeFlowStats
    {
        private readonly Dictionary<string, (TradeDirection Direction, decimal Volume)> _liveTrades = new();
        private int _historicalTradeCount;

        public int TradeCount => _historicalTradeCount + _liveTrades.Count;
        public decimal BuyVolume { get; private set; }
        public decimal SellVolume { get; private set; }
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
                {
                    BuyVolume -= previous.Volume;
                    recalculateBuyMax = previous.Volume >= MaxBuy && trade.Volume < previous.Volume;
                }
                else if (previous.Direction == TradeDirection.Sell)
                {
                    SellVolume -= previous.Volume;
                    recalculateSellMax = previous.Volume >= MaxSell && trade.Volume < previous.Volume;
                }
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
            {
                BuyVolume += volume;
                MaxBuy = Math.Max(MaxBuy, volume);
            }
            else if (direction == TradeDirection.Sell)
            {
                SellVolume += volume;
                MaxSell = Math.Max(MaxSell, volume);
            }
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

    private sealed class PathMetrics
    {
        public decimal Range { get; init; }
        public decimal DirectionalEfficiency { get; init; }
    }

    private sealed class LvnMetrics
    {
        public decimal Price { get; init; }
        public decimal VolumePercentile { get; init; }
    }
}
