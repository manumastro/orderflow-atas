using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    /// <summary>
    /// Fabio London mean-reversion model.
    ///
    /// One playbook only:
    /// 1. Use completed reference value areas, not the developing micro-POC of the current London session.
    /// 2. During London, wait for a sweep outside a completed reference VAH/VAL.
    /// 3. Require the candle to close back inside reference value.
    /// 4. Enter only when a large cumulative trade appears in the mean-reversion direction.
    /// 5. Target the reference POC / bulk of auction.
    /// 6. If price moves at least 1R in favor, move stop to breakeven.
    ///
    /// The same methods are used for live data and historical replay. Historical means only
    /// "past data processed through the live rules".
    /// </summary>
    internal sealed class LondonMeanReversionModule
    {
        private readonly Action<string, bool> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly decimal _tickSize;

        private const decimal LondonBigTradeVolume = 20m;
        private const int RejectionThresholdTicks = 10;
        private const int StopInsideFailedExtremeTicks = 2;
        private const int EntryTimeoutSeconds = 1200;
        private const decimal MinRewardRiskToPoc = 1.0m;
        private const decimal BreakEvenTriggerR = 1.0m;
        private const int BreakEvenOffsetTicks = 0;
        private const int LondonSessionStartHour = 8;
        private const int LondonSessionEndHour = 16;
        private const int LiveHeartbeatTradeStep = 25;
        private const int LiveHeartbeatMinSeconds = 60;

        private int _currentBar;
        private readonly List<BalanceSetup> _activeSetups = new();
        private readonly List<ActivePosition> _activePositions = new();
        private readonly List<CumulativeTrade> _historicalTrades = new();
        private readonly List<TradeRecord> _completedTrades = new();
        private readonly HashSet<string> _setupKeys = new();
        private readonly Dictionary<string, decimal> _liveTradeMaxVolumeByKey = new();
        private readonly Dictionary<string, int> _entryRejectCounts = new();
        private readonly Dictionary<string, int> _setupExpirationCounts = new();
        private readonly ProfileAccumulator _currentDayProfile = new();
        private readonly ProfileAccumulator _currentLondonProfile = new();
        private DateOnly? _currentDayItaly;
        private DateOnly? _currentLondonDay;
        private bool _currentLondonProfileActive;
        private ReferenceValueArea? _previousDayReference;
        private ReferenceValueArea? _previousLondonReference;
        private long _liveAcceptedTradeCount;
        private DateTime _lastLiveHeartbeatUtc = DateTime.MinValue;
        private bool _processingHistoricalReplay;

        public LondonMeanReversionModule(
            BalanceZoneTracker balanceTracker,
            Action<string, bool> log,
            Func<int, IndicatorCandle> getCandle,
            decimal tickSize)
        {
            _ = balanceTracker ?? throw new ArgumentNullException(nameof(balanceTracker));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _getCandle = getCandle ?? throw new ArgumentNullException(nameof(getCandle));
            _tickSize = tickSize > 0 ? tickSize : 1m;

            _log($"[MR_MODE] Model=FabioLondonMeanReversionCore, Modes=LIVE|HISTORICAL, ReferenceProfiles=PreviousDayProfile|PreviousLondonProfile, BigTradeVolume={LondonBigTradeVolume:F0}, Target=REFERENCE_POC_FULL_EXIT, Entry=FAILED_AUCTION_BACK_INSIDE_REFERENCE_VALUE_PLUS_BIG_TRADE, BreakEvenTrigger={BreakEvenTriggerR:F2}R", false);
        }

        public IReadOnlyList<TradeRecord> CompletedTrades => _completedTrades;
        public IReadOnlyList<ActivePosition> ActivePositions => _activePositions;
        public IReadOnlyList<BalanceSetup> ActiveSetups => _activeSetups;

        public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
        {
            _currentBar = currentBar;
            UpdateReferenceProfiles(bar, candle);
            ExpireSetups(GetCandleEventTime(candle));
            DetectReferenceRejectionSetups(bar, candle);
            UpdatePocTouches(bar, candle);
            UpdateOpenPositionsFromBar(bar, candle);
        }

        public void OnNewSessionHigh(int bar, IndicatorCandle candle, decimal previousHigh)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
        }

        public void OnNewSessionLow(int bar, IndicatorCandle candle, decimal previousLow)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
        }

        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            _historicalTrades.Clear();
            _historicalTrades.AddRange(cumulativeTrades.OrderBy(t => t.Time));
            _log($"[MR_HISTORICAL_TRADES] Count={_historicalTrades.Count}, BigTradeVolume={LondonBigTradeVolume:F0}, BeginItaly={(_historicalTrades.Count > 0 ? MarketTimeZones.ToItaly(_historicalTrades.First().Time).ToString("yyyy-MM-dd HH:mm:ss") : "NA")}, EndItaly={(_historicalTrades.Count > 0 ? MarketTimeZones.ToItaly(_historicalTrades.Last().Time).ToString("yyyy-MM-dd HH:mm:ss") : "NA")}", false);
        }

        public void ProcessHistoricalPositions(int startBar, int endBar)
        {
            var startedUtc = DateTime.UtcNow;
            var previousReplayState = _processingHistoricalReplay;
            _processingHistoricalReplay = true;
            _completedTrades.Clear();
            _activePositions.Clear();
            _entryRejectCounts.Clear();
            _setupExpirationCounts.Clear();

            var firstTradeTime = _historicalTrades.Count > 0 ? _historicalTrades.First().Time : DateTime.MaxValue;
            var lastTradeTime = _historicalTrades.Count > 0 ? _historicalTrades.Last().Time : DateTime.MinValue;
            foreach (var setup in _activeSetups)
                ResetSetupReplayState(setup, firstTradeTime, lastTradeTime);

            _log($"[HISTORICAL_FLOW_PROCESS_START] Model=FabioLondonMeanReversionCore, StartBar={startBar}, EndBar={endBar}, StoredTrades={_historicalTrades.Count}, ActiveSetups={_activeSetups.Count}, ReplayStateReset=True, FirstTrade={(_historicalTrades.Count > 0 ? FormatTime(firstTradeTime) : "NA")}, LastTrade={(_historicalTrades.Count > 0 ? FormatTime(lastTradeTime) : "NA")}", false);

            try
            {
                foreach (var trade in _historicalTrades)
                {
                    ExpireSetups(trade.Time);

                    if (!IsLondonTradeAllowed(trade.Time))
                        continue;

                    if (trade.Volume >= LondonBigTradeVolume)
                        ProcessAggressionTrade(trade, "Historical", true);

                    ExpireSetupsOnPocTouchFromTrade(trade, true);
                    UpdateOpenPositionsFromTrade(trade, true);
                }

                CloseOpenPositionsAtLondonClose();
                var durationMs = (DateTime.UtcNow - startedUtc).TotalMilliseconds;
                LogReplayAudit();
                _log($"[HISTORICAL_FLOW_FINISH] Model=FabioLondonMeanReversionCore, StartBar={startBar}, EndBar={endBar}, StoredTrades={_historicalTrades.Count}, Entries={_activePositions.Count}, ClosedPositions={_activePositions.Count(p => p.Closed)}, OpenPositions={_activePositions.Count(p => !p.Closed)}, CompletedTrades={_completedTrades.Count}, ProcessDurationMs={durationMs:F0}", false);
            }
            finally
            {
                _processingHistoricalReplay = previousReplayState;
            }
        }

        public void OnLiveCumulativeTrade(CumulativeTrade trade)
        {
            if (!IsLondonTradeAllowed(trade.Time))
                return;

            ExpireSetups(trade.Time);

            var acceptedBigTrade = false;
            if (trade.Volume >= LondonBigTradeVolume)
            {
                var key = GetLiveTradeKey(trade);
                if (!_liveTradeMaxVolumeByKey.TryGetValue(key, out var seenVolume) || trade.Volume > seenVolume)
                {
                    _liveTradeMaxVolumeByKey[key] = trade.Volume;
                    ProcessAggressionTrade(trade, "Live", false);
                    acceptedBigTrade = true;
                }
            }

            ExpireSetupsOnPocTouchFromTrade(trade, false);
            UpdateOpenPositionsFromTrade(trade, false);

            if (acceptedBigTrade)
                LogLiveHeartbeat(trade);
        }

        private void UpdateReferenceProfiles(int bar, IndicatorCandle candle)
        {
            UpdatePreviousDayReference(bar, candle);
            UpdatePreviousLondonReference(bar, candle);
        }

        private void UpdatePreviousDayReference(int bar, IndicatorCandle candle)
        {
            var italyDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(candle.Time));
            if (_currentDayItaly == null)
            {
                _currentDayItaly = italyDay;
                _currentDayProfile.Reset();
            }
            else if (_currentDayItaly.Value != italyDay)
            {
                FinalizePreviousDayReference(bar, _currentDayItaly.Value);
                _currentDayProfile.Reset();
                _currentDayItaly = italyDay;
            }

            _currentDayProfile.ReplaceContribution(bar, candle);
        }

        private void UpdatePreviousLondonReference(int bar, IndicatorCandle candle)
        {
            var londonTime = MarketTimeZones.ToLondon(candle.Time);
            var londonDay = DateOnly.FromDateTime(londonTime);
            var inLondon = IsInLondonSession(candle.Time);

            if (inLondon)
            {
                if (!_currentLondonProfileActive || _currentLondonDay != londonDay)
                {
                    if (_currentLondonProfileActive && _currentLondonDay.HasValue)
                        FinalizePreviousLondonReference(bar, _currentLondonDay.Value);

                    _currentLondonProfile.Reset();
                    _currentLondonDay = londonDay;
                    _currentLondonProfileActive = true;
                }

                _currentLondonProfile.ReplaceContribution(bar, candle);
                return;
            }

            if (_currentLondonProfileActive && _currentLondonDay.HasValue)
            {
                FinalizePreviousLondonReference(bar, _currentLondonDay.Value);
                _currentLondonProfile.Reset();
                _currentLondonProfileActive = false;
                _currentLondonDay = null;
            }
        }

        private void FinalizePreviousDayReference(int bar, DateOnly day)
        {
            if (!TryBuildReference("PreviousDayProfile", day.ToString("yyyy-MM-dd"), _currentDayProfile, out var reference) || reference == null)
                return;

            _previousDayReference = reference;
            LogReferenceReady(reference, bar);
        }

        private void FinalizePreviousLondonReference(int bar, DateOnly londonDay)
        {
            if (!TryBuildReference("PreviousLondonProfile", londonDay.ToString("yyyy-MM-dd"), _currentLondonProfile, out var reference) || reference == null)
                return;

            _previousLondonReference = reference;
            LogReferenceReady(reference, bar);
        }

        private bool TryBuildReference(string source, string label, ProfileAccumulator accumulator, out ReferenceValueArea? reference)
        {
            reference = null;
            if (accumulator.Profile.Count == 0 || accumulator.TotalVolume <= 0)
                return false;

            if (!TryCalculateValueArea(accumulator.Profile, accumulator.TotalVolume, out var poc, out var vah, out var val, out var valueAreaVolume, out var maxVolume))
                return false;

            reference = new ReferenceValueArea
            {
                Source = source,
                Label = label,
                POC = poc,
                VAH = vah,
                VAL = val,
                High = accumulator.High,
                Low = accumulator.Low,
                TotalVolume = accumulator.TotalVolume,
                ValueAreaVolume = valueAreaVolume,
                MaxLevelVolume = maxVolume,
                StartBar = accumulator.StartBar,
                EndBar = accumulator.EndBar,
                BeginTimeUtc = accumulator.BeginTimeUtc,
                EndTimeUtc = accumulator.EndTimeUtc,
                Bars = accumulator.BarCount
            };
            return true;
        }

        private void LogReferenceReady(ReferenceValueArea reference, int bar)
        {
            var isHistorical = IsHistoricalContext(bar);
            _log($"[MR_REFERENCE_READY] ExecutionMode={GetExecutionMode(isHistorical)}, Source={reference.Source}, Label={reference.Label}, StartBar={reference.StartBar}, EndBar={reference.EndBar}, Begin={FormatTime(reference.BeginTimeUtc)}, End={FormatTime(reference.EndTimeUtc)}, POC={reference.POC:F2}, VAH={reference.VAH:F2}, VAL={reference.VAL:F2}, High={reference.High:F2}, Low={reference.Low:F2}, Bars={reference.Bars}, TotalVolume={reference.TotalVolume:F0}, ValueAreaVolume={reference.ValueAreaVolume:F0}, MaxLevelVolume={reference.MaxLevelVolume:F0}", isHistorical);
        }

        private void DetectReferenceRejectionSetups(int bar, IndicatorCandle candle)
        {
            if (!IsInLondonSession(candle.Time))
                return;

            foreach (var reference in GetActiveReferences())
            {
                if (TryCreateFailedHighSetup(bar, candle, reference, out var shortSetup) && shortSetup != null)
                    AddSetup(shortSetup, "MR_SETUP_SHORT");

                if (TryCreateFailedLowSetup(bar, candle, reference, out var longSetup) && longSetup != null)
                    AddSetup(longSetup, "MR_SETUP_LONG");
            }
        }

        private IEnumerable<ReferenceValueArea> GetActiveReferences()
        {
            if (_previousDayReference?.IsValid == true)
                yield return _previousDayReference;

            if (_previousLondonReference?.IsValid == true)
                yield return _previousLondonReference;
        }

        private bool TryCreateFailedHighSetup(int bar, IndicatorCandle candle, ReferenceValueArea reference, out BalanceSetup? setup)
        {
            setup = null;
            if (!reference.IsValid)
                return false;

            if (candle.High <= reference.VAH || candle.Close >= reference.VAH)
                return false;

            var rejection = candle.High - candle.Close;
            if (rejection < RejectionThresholdTicks * _tickSize)
                return false;

            var stop = candle.High - StopInsideFailedExtremeTicks * _tickSize;
            if (stop <= reference.POC)
                return false;

            setup = new BalanceSetup
            {
                SetupId = Guid.NewGuid().ToString(),
                Direction = "Short",
                Source = reference.Source,
                Pattern = "FailedHighBackInsideReferenceValue",
                ReferenceLabel = reference.Label,
                ReferenceStartBar = reference.StartBar,
                ReferenceEndBar = reference.EndBar,
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                BreakoutPrice = candle.High,
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                POC = reference.POC,
                VAH = reference.VAH,
                VAL = reference.VAL,
                StopPrice = stop,
                TargetPrice = reference.POC
            };

            return true;
        }

        private bool TryCreateFailedLowSetup(int bar, IndicatorCandle candle, ReferenceValueArea reference, out BalanceSetup? setup)
        {
            setup = null;
            if (!reference.IsValid)
                return false;

            if (candle.Low >= reference.VAL || candle.Close <= reference.VAL)
                return false;

            var rejection = candle.Close - candle.Low;
            if (rejection < RejectionThresholdTicks * _tickSize)
                return false;

            var stop = candle.Low + StopInsideFailedExtremeTicks * _tickSize;
            if (stop >= reference.POC)
                return false;

            setup = new BalanceSetup
            {
                SetupId = Guid.NewGuid().ToString(),
                Direction = "Long",
                Source = reference.Source,
                Pattern = "FailedLowBackInsideReferenceValue",
                ReferenceLabel = reference.Label,
                ReferenceStartBar = reference.StartBar,
                ReferenceEndBar = reference.EndBar,
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                BreakoutPrice = candle.Low,
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                POC = reference.POC,
                VAH = reference.VAH,
                VAL = reference.VAL,
                StopPrice = stop,
                TargetPrice = reference.POC
            };

            return true;
        }

        private void AddSetup(BalanceSetup setup, string tag)
        {
            var key = $"{setup.Source}:{setup.ReferenceLabel}:{setup.Direction}:{setup.RejectionBar}:{setup.BreakoutPrice:F2}:{setup.POC:F2}";
            if (!_setupKeys.Add(key))
                return;

            _activeSetups.Add(setup);
            var isHistorical = IsHistoricalContext(setup.RejectionBar);
            _log($"[{tag}] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, Source={setup.Source}, Pattern={setup.Pattern}, ReferenceLabel={setup.ReferenceLabel}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", isHistorical);
        }

        private void UpdatePocTouches(int bar, IndicatorCandle candle)
        {
            var eventTime = GetCandleEventTime(candle);
            foreach (var setup in _activeSetups.Where(s => !s.Expired && !s.PocTouched && !s.AggressionConfirmed))
            {
                if (eventTime <= setup.RejectionTimeUtc)
                    continue;

                if (HasTouchedPoc(setup, candle.High, candle.Low))
                {
                    setup.PocTouched = true;
                    setup.FirstPocTouchTimeUtc = eventTime;
                    setup.FirstPocTouchPrice = setup.POC;
                    setup.Expired = true;
                    RecordSetupExpiration("POC_TOUCHED_BY_BAR");
                    _log($"[MR_SETUP_EXPIRED] ExecutionMode={GetExecutionMode(IsHistoricalContext(bar))}, SetupId={setup.SetupId}, Reason=POC_TOUCHED_BEFORE_ENTRY, Direction={setup.Direction}, Bar={bar}, {FormatTime(eventTime)}, POC={setup.POC:F2}", IsHistoricalContext(bar));
                }
            }
        }

        private void ProcessAggressionTrade(CumulativeTrade trade, string mode, bool isHistorical)
        {
            ExpireSetups(trade.Time);

            foreach (var setup in _activeSetups
                .Where(s => !s.Expired && !s.AggressionConfirmed)
                .OrderBy(s => s.RejectionTimeUtc)
                .ThenBy(s => s.Source)
                .ToList())
            {
                if (!IsEntryCandidate(setup, trade, out var reason, out var risk, out var reward, out var rr))
                {
                    if (reason != "TRADE_BEFORE_REJECTION")
                    {
                        RecordEntryReject(reason);
                        TrackRejectedEntryCandidate(setup, trade, reason, rr);
                    }
                    continue;
                }

                setup.AggressionConfirmed = true;
                CreatePosition(setup, trade, mode, isHistorical, risk, reward, rr);
                ExpireOverlappingSetupsOnEntry(setup);
                return;
            }
        }

        private void ExpireOverlappingSetupsOnEntry(BalanceSetup confirmedSetup)
        {
            foreach (var setup in _activeSetups.Where(s => s.SetupId != confirmedSetup.SetupId && !s.Expired && !s.AggressionConfirmed))
            {
                if (setup.Direction != confirmedSetup.Direction || setup.RejectionBar != confirmedSetup.RejectionBar)
                    continue;

                setup.Expired = true;
                RecordSetupExpiration("OVERLAPPING_REFERENCE_ENTRY");
            }
        }

        private bool IsEntryCandidate(BalanceSetup setup, CumulativeTrade trade, out string reason, out decimal risk, out decimal reward, out decimal rr)
        {
            risk = 0;
            reward = 0;
            rr = 0;
            reason = "NA";

            if (trade.Time <= setup.RejectionTimeUtc)
            {
                reason = "TRADE_BEFORE_REJECTION";
                return false;
            }

            if (trade.Time > setup.RejectionTimeUtc.AddSeconds(EntryTimeoutSeconds))
            {
                reason = "SETUP_STALE";
                setup.Expired = true;
                RecordSetupExpiration("TIMEOUT_BY_TRADE");
                return false;
            }

            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction != expectedDirection)
            {
                reason = "WRONG_AGGRESSION_DIRECTION";
                return false;
            }

            var price = trade.Lastprice;
            if (setup.Direction == "Long")
            {
                if (price <= setup.VAL || price >= setup.POC)
                {
                    reason = "OUTSIDE_LONG_ENTRY_ZONE";
                    return false;
                }

                risk = price - setup.StopPrice;
                reward = setup.POC - price;
            }
            else
            {
                if (price >= setup.VAH || price <= setup.POC)
                {
                    reason = "OUTSIDE_SHORT_ENTRY_ZONE";
                    return false;
                }

                risk = setup.StopPrice - price;
                reward = price - setup.POC;
            }

            if (risk <= 0)
            {
                reason = "STOP_NOT_BEHIND_ENTRY";
                return false;
            }

            rr = reward / risk;
            if (rr < MinRewardRiskToPoc)
            {
                reason = "RR_TO_POC_TOO_LOW";
                return false;
            }

            reason = "VALID";
            return true;
        }

        private void CreatePosition(BalanceSetup setup, CumulativeTrade trade, string mode, bool isHistorical, decimal risk, decimal reward, decimal rr)
        {
            var position = new ActivePosition
            {
                SetupId = setup.SetupId,
                Direction = setup.Direction,
                EntryModel = mode,
                EntryBar = FindBarByTime(trade.Time, setup.RejectionBar),
                EntryTimeUtc = trade.Time,
                EntryPrice = trade.Lastprice,
                StopPrice = setup.StopPrice,
                OriginalStopPrice = setup.StopPrice,
                TargetPrice = setup.TargetPrice,
                InitialRisk = risk,
                MaxFavorablePrice = trade.Lastprice,
                MaxAdversePrice = trade.Lastprice
            };

            _activePositions.Add(position);

            _log($"[MR_ENTRY] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, EntryModel={mode}, Source={setup.Source}, Pattern={setup.Pattern}, ReferenceLabel={setup.ReferenceLabel}, Direction={setup.Direction}, Bar={position.EntryBar}, {FormatTime(trade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={trade.Volume:F0}, TradeDirection={trade.Direction}, Stop={position.StopPrice:F2}, TargetPOC={position.TargetPrice:F2}, Risk={risk:F2}, RewardToPOC={reward:F2}, RewardRiskToPOC={rr:F2}, BreakEvenTrigger={BreakEvenTriggerR:F2}R, BigTradeVolume={LondonBigTradeVolume:F0}, SecondsAfterRejection={(trade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}", isHistorical);
        }

        private void UpdateOpenPositionsFromBar(int bar, IndicatorCandle candle)
        {
            if (_processingHistoricalReplay)
                return;

            var eventTime = GetCandleEventTime(candle);
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                UpdatePositionTracking(position, candle.High, candle.Low);
                TryActivateBreakEven(position, bar, eventTime, IsHistoricalContext(bar));
                CheckPositionExit(position, bar, eventTime, candle.High, candle.Low, IsHistoricalContext(bar));
            }
        }

        private void UpdateOpenPositionsFromTrade(CumulativeTrade trade, bool isHistorical)
        {
            var high = Math.Max(trade.FirstPrice, trade.Lastprice);
            var low = Math.Min(trade.FirstPrice, trade.Lastprice);
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                UpdatePositionTracking(position, high, low);
                var bar = FindBarByTime(trade.Time, position.EntryBar);
                TryActivateBreakEven(position, bar, trade.Time, isHistorical);
                CheckPositionExit(position, bar, trade.Time, high, low, isHistorical);
            }
        }

        private void TryActivateBreakEven(ActivePosition position, int bar, DateTime eventTimeUtc, bool isHistorical)
        {
            if (position.Closed || position.BreakEvenActivated || position.InitialRisk <= 0)
                return;

            var trigger = position.InitialRisk * BreakEvenTriggerR;
            if (position.MFE < trigger)
                return;

            var oldStop = position.StopPrice;
            var offset = BreakEvenOffsetTicks * _tickSize;
            position.StopPrice = position.Direction == "Long"
                ? position.EntryPrice + offset
                : position.EntryPrice - offset;
            position.BreakEvenActivated = true;
            position.BreakEvenTimeUtc = eventTimeUtc;

            _log($"[MR_BREAKEVEN] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, OldStop={oldStop:F2}, NewStop={position.StopPrice:F2}, MFE={position.MFE:F2}, InitialRisk={position.InitialRisk:F2}, TriggerR={BreakEvenTriggerR:F2}R", isHistorical);
        }

        private void CheckPositionExit(ActivePosition position, int bar, DateTime eventTimeUtc, decimal high, decimal low, bool isHistorical)
        {
            if (position.Closed)
                return;

            if (position.Direction == "Long")
            {
                if (low <= position.StopPrice)
                {
                    ClosePosition(position, bar, eventTimeUtc, "STOP_HIT", position.StopPrice, isHistorical);
                    return;
                }

                if (high >= position.TargetPrice)
                    ClosePosition(position, bar, eventTimeUtc, "POC_TARGET_HIT", position.TargetPrice, isHistorical);
            }
            else
            {
                if (high >= position.StopPrice)
                {
                    ClosePosition(position, bar, eventTimeUtc, "STOP_HIT", position.StopPrice, isHistorical);
                    return;
                }

                if (low <= position.TargetPrice)
                    ClosePosition(position, bar, eventTimeUtc, "POC_TARGET_HIT", position.TargetPrice, isHistorical);
            }
        }

        private void ClosePosition(ActivePosition position, int bar, DateTime eventTimeUtc, string exitReason, decimal exitPrice, bool isHistorical)
        {
            if (position.Closed)
                return;

            position.Closed = true;
            position.ExitBar = bar;
            position.ExitTimeUtc = eventTimeUtc;
            position.ExitReason = exitReason;
            position.ExitPrice = exitPrice;

            var pnl = GetDirectionalPnl(position.Direction, position.EntryPrice, exitPrice);
            var risk = position.InitialRisk > 0
                ? position.InitialRisk
                : position.Direction == "Long"
                    ? position.EntryPrice - position.OriginalStopPrice
                    : position.OriginalStopPrice - position.EntryPrice;
            var rMultiple = risk > 0 ? pnl / risk : 0;

            _completedTrades.Add(new TradeRecord
            {
                SetupId = position.SetupId,
                Direction = position.Direction,
                EntryModel = position.EntryModel,
                EntryTime = position.EntryTimeUtc,
                EntryPrice = position.EntryPrice,
                ExitTime = eventTimeUtc,
                ExitPrice = exitPrice,
                ExitReason = exitReason,
                PnL = pnl,
                MFE = position.MFE,
                MAE = position.MAE,
                RMultiple = rMultiple,
                BreakEvenActivated = position.BreakEvenActivated
            });

            _log($"[MR_EXIT] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={pnl:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, BreakEvenActivated={position.BreakEvenActivated}, TargetPOC={position.TargetPrice:F2}, Stop={position.StopPrice:F2}, OriginalStop={position.OriginalStopPrice:F2}", isHistorical);
        }

        private void CloseOpenPositionsAtLondonClose()
        {
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                var londonCloseUtc = GetLondonSessionEndUtc(position.EntryTimeUtc);
                var lastTrade = _historicalTrades
                    .Where(t => t.Time >= position.EntryTimeUtc && t.Time <= londonCloseUtc)
                    .OrderBy(t => t.Time)
                    .LastOrDefault();

                var exitTime = lastTrade?.Time ?? londonCloseUtc;
                var exitPrice = lastTrade?.Lastprice ?? position.EntryPrice;
                ClosePosition(position, FindBarByTime(exitTime, position.EntryBar), exitTime, "LONDON_CLOSE", exitPrice, true);
            }
        }

        private void ExpireSetups(DateTime eventTimeUtc)
        {
            foreach (var setup in _activeSetups.Where(s => !s.Expired && !s.AggressionConfirmed && eventTimeUtc > s.RejectionTimeUtc.AddSeconds(EntryTimeoutSeconds)))
            {
                setup.Expired = true;
                RecordSetupExpiration("TIMEOUT");
            }
        }

        private void ExpireSetupsOnPocTouchFromTrade(CumulativeTrade trade, bool isHistorical)
        {
            var high = Math.Max(trade.FirstPrice, trade.Lastprice);
            var low = Math.Min(trade.FirstPrice, trade.Lastprice);
            foreach (var setup in _activeSetups.Where(s => !s.Expired && !s.PocTouched && !s.AggressionConfirmed).ToList())
            {
                if (trade.Time <= setup.RejectionTimeUtc)
                    continue;

                if (!HasTouchedPoc(setup, high, low))
                    continue;

                setup.PocTouched = true;
                setup.FirstPocTouchTimeUtc = trade.Time;
                setup.FirstPocTouchPrice = trade.Lastprice;
                setup.FirstPocTouchVolume = trade.Volume;
                setup.Expired = true;
                RecordSetupExpiration("POC_TOUCHED_BY_TRADE");
                _log($"[MR_SETUP_EXPIRED] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, Reason=POC_TOUCHED_BEFORE_ENTRY_BY_TRADE, Direction={setup.Direction}, Bar={FindBarByTime(trade.Time, setup.RejectionBar)}, {FormatTime(trade.Time)}, POC={setup.POC:F2}, TradeFirst={trade.FirstPrice:F2}, TradeLast={trade.Lastprice:F2}, Volume={trade.Volume:F0}", isHistorical);
            }
        }

        private void ResetSetupReplayState(BalanceSetup setup, DateTime firstTradeTime, DateTime lastTradeTime)
        {
            setup.AggressionConfirmed = false;
            setup.Expired = false;
            setup.PocTouched = false;
            setup.ReplayExcluded = false;
            setup.FirstSameDirectionBigTradeTimeUtc = null;
            setup.FirstSameDirectionBigTradePrice = 0;
            setup.FirstSameDirectionBigTradeVolume = 0;
            setup.FirstSameDirectionRejectReason = string.Empty;
            setup.FirstSameDirectionRewardRiskToPoc = 0;
            setup.FirstOppositeDirectionBigTradeTimeUtc = null;
            setup.FirstOppositeDirectionBigTradePrice = 0;
            setup.FirstOppositeDirectionBigTradeVolume = 0;
            setup.FirstPocTouchTimeUtc = null;
            setup.FirstPocTouchPrice = 0;
            setup.FirstPocTouchVolume = 0;

            if (_historicalTrades.Count == 0 || setup.RejectionTimeUtc < firstTradeTime || setup.RejectionTimeUtc > lastTradeTime)
            {
                setup.ReplayExcluded = true;
                setup.Expired = true;
                RecordSetupExpiration("OUTSIDE_HISTORICAL_TRADE_COVERAGE");
            }
        }

        private void TrackRejectedEntryCandidate(BalanceSetup setup, CumulativeTrade trade, string reason, decimal rr)
        {
            if (!_processingHistoricalReplay)
                return;

            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction == expectedDirection)
            {
                if (setup.FirstSameDirectionBigTradeTimeUtc == null)
                {
                    setup.FirstSameDirectionBigTradeTimeUtc = trade.Time;
                    setup.FirstSameDirectionBigTradePrice = trade.Lastprice;
                    setup.FirstSameDirectionBigTradeVolume = trade.Volume;
                    setup.FirstSameDirectionRejectReason = reason;
                    setup.FirstSameDirectionRewardRiskToPoc = rr;
                }
            }
            else if (setup.FirstOppositeDirectionBigTradeTimeUtc == null)
            {
                setup.FirstOppositeDirectionBigTradeTimeUtc = trade.Time;
                setup.FirstOppositeDirectionBigTradePrice = trade.Lastprice;
                setup.FirstOppositeDirectionBigTradeVolume = trade.Volume;
            }
        }

        private void RecordEntryReject(string reason)
        {
            if (!_processingHistoricalReplay)
                return;

            _entryRejectCounts.TryGetValue(reason, out var count);
            _entryRejectCounts[reason] = count + 1;
        }

        private void RecordSetupExpiration(string reason)
        {
            if (!_processingHistoricalReplay)
                return;

            _setupExpirationCounts.TryGetValue(reason, out var count);
            _setupExpirationCounts[reason] = count + 1;
        }

        private void LogReplayAudit()
        {
            _log($"[MR_REPLAY_AUDIT] ActiveSetups={_activeSetups.Count}, Entries={_activePositions.Count}, ClosedPositions={_activePositions.Count(p => p.Closed)}, OpenPositions={_activePositions.Count(p => !p.Closed)}, EntryRejects={FormatCounts(_entryRejectCounts)}, Expirations={FormatCounts(_setupExpirationCounts)}", false);

            foreach (var setup in _activeSetups.OrderBy(s => s.RejectionTimeUtc).ThenBy(s => s.Source))
            {
                if (setup.AggressionConfirmed)
                    continue;

                var finalState = setup.ReplayExcluded
                    ? "EXCLUDED_OUTSIDE_HISTORICAL_TRADE_COVERAGE"
                    : setup.Expired
                        ? setup.PocTouched ? "EXPIRED_POC_TOUCHED" : "EXPIRED_TIMEOUT"
                        : "NO_VALID_BIG_TRADE";
                _log($"[MR_SETUP_NO_ENTRY] SetupId={setup.SetupId}, Direction={setup.Direction}, Source={setup.Source}, Pattern={setup.Pattern}, ReferenceLabel={setup.ReferenceLabel}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, FinalState={finalState}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}, {FormatSetupAudit(setup)}", true);
            }
        }

        private string FormatSetupAudit(BalanceSetup setup)
        {
            var same = setup.FirstSameDirectionBigTradeTimeUtc.HasValue
                ? $"FirstSameBigTrade={FormatTime(setup.FirstSameDirectionBigTradeTimeUtc.Value)}, SamePrice={setup.FirstSameDirectionBigTradePrice:F2}, SameVolume={setup.FirstSameDirectionBigTradeVolume:F0}, SameReject={setup.FirstSameDirectionRejectReason}, SameRR={setup.FirstSameDirectionRewardRiskToPoc:F2}"
                : "FirstSameBigTrade=none";
            var opposite = setup.FirstOppositeDirectionBigTradeTimeUtc.HasValue
                ? $"FirstOppositeBigTrade={FormatTime(setup.FirstOppositeDirectionBigTradeTimeUtc.Value)}, OppositePrice={setup.FirstOppositeDirectionBigTradePrice:F2}, OppositeVolume={setup.FirstOppositeDirectionBigTradeVolume:F0}"
                : "FirstOppositeBigTrade=none";
            var poc = setup.FirstPocTouchTimeUtc.HasValue
                ? $"FirstPocTouch={FormatTime(setup.FirstPocTouchTimeUtc.Value)}, PocTouchPrice={setup.FirstPocTouchPrice:F2}, PocTouchVolume={setup.FirstPocTouchVolume:F0}"
                : "FirstPocTouch=none";
            return $"{same}, {opposite}, {poc}";
        }

        private static string FormatCounts(Dictionary<string, int> counts)
        {
            if (counts.Count == 0)
                return "none";

            return string.Join("|", counts.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).Select(kv => $"{kv.Key}:{kv.Value}"));
        }

        private void UpdatePositionTracking(ActivePosition position, decimal high, decimal low)
        {
            if (position.Direction == "Long")
            {
                if (high > position.MaxFavorablePrice)
                    position.MaxFavorablePrice = high;
                if (low < position.MaxAdversePrice)
                    position.MaxAdversePrice = low;
                position.MFE = Math.Max(position.MFE, position.MaxFavorablePrice - position.EntryPrice);
                position.MAE = Math.Max(position.MAE, position.EntryPrice - position.MaxAdversePrice);
            }
            else
            {
                if (low < position.MaxFavorablePrice)
                    position.MaxFavorablePrice = low;
                if (high > position.MaxAdversePrice)
                    position.MaxAdversePrice = high;
                position.MFE = Math.Max(position.MFE, position.EntryPrice - position.MaxFavorablePrice);
                position.MAE = Math.Max(position.MAE, position.MaxAdversePrice - position.EntryPrice);
            }
        }

        private static bool HasTouchedPoc(BalanceSetup setup, decimal high, decimal low)
        {
            return setup.Direction == "Long"
                ? high >= setup.POC
                : low <= setup.POC;
        }

        private static decimal GetDirectionalPnl(string direction, decimal entry, decimal exit)
        {
            return direction == "Long" ? exit - entry : entry - exit;
        }

        private bool IsInLondonSession(DateTime utcTime)
        {
            var london = MarketTimeZones.ToLondon(utcTime);
            return london.Hour >= LondonSessionStartHour && london.Hour < LondonSessionEndHour;
        }

        private bool IsLondonTradeAllowed(DateTime utcTime)
        {
            return IsInLondonSession(utcTime);
        }

        private DateTime GetLondonSessionEndUtc(DateTime eventUtc)
        {
            var london = MarketTimeZones.ToLondon(eventUtc);
            var londonEnd = new DateTime(london.Year, london.Month, london.Day, LondonSessionEndHour, 0, 0, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(londonEnd, MarketTimeZones.London);
        }

        private int FindBarByTime(DateTime timeUtc, int fallbackBar)
        {
            var maxBar = Math.Max(0, _currentBar - 1);
            var start = Math.Max(0, fallbackBar);
            for (var bar = start; bar <= maxBar; bar++)
            {
                var candle = _getCandle(bar);
                var candleEnd = GetCandleEventTime(candle);
                if (timeUtc >= candle.Time && timeUtc <= candleEnd)
                    return bar;
            }

            return Math.Min(Math.Max(fallbackBar, 0), maxBar);
        }

        private static DateTime GetCandleEventTime(IndicatorCandle candle)
        {
            return candle.LastTime > candle.Time ? candle.LastTime : candle.Time;
        }

        private bool IsHistoricalContext(int bar)
        {
            return _processingHistoricalReplay || bar < _currentBar - 1;
        }

        private static string GetExecutionMode(bool isHistorical)
        {
            return isHistorical ? "HISTORICAL" : "LIVE";
        }

        private string GetLiveTradeKey(CumulativeTrade trade)
        {
            return $"{trade.Time.Ticks}:{trade.Direction}:{trade.FirstPrice:F2}";
        }

        private string FormatTime(DateTime utc)
        {
            return MarketTimeZones.FormatUtcLondonItaly(utc);
        }

        private void LogLiveHeartbeat(CumulativeTrade trade)
        {
            _liveAcceptedTradeCount++;
            var now = DateTime.UtcNow;
            var elapsedSeconds = _lastLiveHeartbeatUtc == DateTime.MinValue
                ? LiveHeartbeatMinSeconds
                : (now - _lastLiveHeartbeatUtc).TotalSeconds;
            var shouldLog = _liveAcceptedTradeCount == 1
                || _liveAcceptedTradeCount % LiveHeartbeatTradeStep == 0
                || elapsedSeconds >= LiveHeartbeatMinSeconds;
            if (!shouldLog)
                return;

            _lastLiveHeartbeatUtc = now;
            _log($"[MR_LIVE_HEARTBEAT] AcceptedBigTrades={_liveAcceptedTradeCount}, TradeTime={FormatTime(trade.Time)}, Direction={trade.Direction}, Price={trade.Lastprice:F2}, Volume={trade.Volume:F0}, OpenSetups={_activeSetups.Count(s => !s.Expired && !s.AggressionConfirmed)}, OpenPositions={_activePositions.Count(p => !p.Closed)}", false);
        }

        private static bool TryCalculateValueArea(
            IReadOnlyDictionary<decimal, decimal> profile,
            decimal totalVolume,
            out decimal poc,
            out decimal vah,
            out decimal val,
            out decimal valueAreaVolume,
            out decimal maxVolume)
        {
            poc = 0;
            vah = 0;
            val = 0;
            valueAreaVolume = 0;
            maxVolume = 0;

            if (profile.Count == 0 || totalVolume <= 0)
                return false;

            foreach (var kvp in profile)
            {
                if (kvp.Value > maxVolume || (kvp.Value == maxVolume && (poc == 0 || kvp.Key < poc)))
                {
                    maxVolume = kvp.Value;
                    poc = kvp.Key;
                }
            }

            var sortedLevels = profile.OrderBy(kv => kv.Key).ToList();
            var resolvedPoc = poc;
            var pocIndex = sortedLevels.FindIndex(kv => kv.Key == resolvedPoc);
            if (pocIndex < 0)
                return false;

            var targetVolume = totalVolume * 0.70m;
            valueAreaVolume = sortedLevels[pocIndex].Value;
            var lowerIndex = pocIndex;
            var upperIndex = pocIndex;

            while (valueAreaVolume < targetVolume && (lowerIndex > 0 || upperIndex < sortedLevels.Count - 1))
            {
                var lowerVolume = lowerIndex > 0 ? sortedLevels[lowerIndex - 1].Value : 0;
                var upperVolume = upperIndex < sortedLevels.Count - 1 ? sortedLevels[upperIndex + 1].Value : 0;

                if (lowerVolume >= upperVolume && lowerIndex > 0)
                {
                    lowerIndex--;
                    valueAreaVolume += sortedLevels[lowerIndex].Value;
                }
                else if (upperIndex < sortedLevels.Count - 1)
                {
                    upperIndex++;
                    valueAreaVolume += sortedLevels[upperIndex].Value;
                }
                else
                {
                    break;
                }
            }

            val = sortedLevels[lowerIndex].Key;
            vah = sortedLevels[upperIndex].Key;
            return true;
        }

        public sealed class BalanceSetup
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string Source { get; set; } = string.Empty;
            public string Pattern { get; set; } = string.Empty;
            public string ReferenceLabel { get; set; } = string.Empty;
            public int ReferenceStartBar { get; set; }
            public int ReferenceEndBar { get; set; }
            public int RejectionBar { get; set; }
            public DateTime RejectionTimeUtc { get; set; }
            public decimal BreakoutPrice { get; set; }
            public decimal RejectionClose { get; set; }
            public decimal RejectionHigh { get; set; }
            public decimal RejectionLow { get; set; }
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            public decimal StopPrice { get; set; }
            public decimal TargetPrice { get; set; }
            public bool AggressionConfirmed { get; set; }
            public bool Expired { get; set; }
            public bool PocTouched { get; set; }
            public bool ReplayExcluded { get; set; }
            public DateTime? FirstSameDirectionBigTradeTimeUtc { get; set; }
            public decimal FirstSameDirectionBigTradePrice { get; set; }
            public decimal FirstSameDirectionBigTradeVolume { get; set; }
            public string FirstSameDirectionRejectReason { get; set; } = string.Empty;
            public decimal FirstSameDirectionRewardRiskToPoc { get; set; }
            public DateTime? FirstOppositeDirectionBigTradeTimeUtc { get; set; }
            public decimal FirstOppositeDirectionBigTradePrice { get; set; }
            public decimal FirstOppositeDirectionBigTradeVolume { get; set; }
            public DateTime? FirstPocTouchTimeUtc { get; set; }
            public decimal FirstPocTouchPrice { get; set; }
            public decimal FirstPocTouchVolume { get; set; }
        }

        public sealed class ActivePosition
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string EntryModel { get; set; } = string.Empty;
            public int EntryBar { get; set; }
            public DateTime EntryTimeUtc { get; set; }
            public decimal EntryPrice { get; set; }
            public decimal StopPrice { get; set; }
            public decimal OriginalStopPrice { get; set; }
            public decimal TargetPrice { get; set; }
            public decimal InitialRisk { get; set; }
            public decimal MaxFavorablePrice { get; set; }
            public decimal MaxAdversePrice { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public bool BreakEvenActivated { get; set; }
            public DateTime BreakEvenTimeUtc { get; set; }
            public bool Closed { get; set; }
            public int ExitBar { get; set; }
            public DateTime ExitTimeUtc { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal ExitPrice { get; set; }
        }

        public sealed class TradeRecord
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string EntryModel { get; set; } = string.Empty;
            public DateTime EntryTime { get; set; }
            public decimal EntryPrice { get; set; }
            public DateTime ExitTime { get; set; }
            public decimal ExitPrice { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal PnL { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public decimal RMultiple { get; set; }
            public bool BreakEvenActivated { get; set; }
        }

        private sealed class ReferenceValueArea
        {
            public string Source { get; set; } = string.Empty;
            public string Label { get; set; } = string.Empty;
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal TotalVolume { get; set; }
            public decimal ValueAreaVolume { get; set; }
            public decimal MaxLevelVolume { get; set; }
            public int StartBar { get; set; }
            public int EndBar { get; set; }
            public DateTime BeginTimeUtc { get; set; }
            public DateTime EndTimeUtc { get; set; }
            public int Bars { get; set; }
            public bool IsValid => POC > 0 && VAH > 0 && VAL > 0 && VAH > VAL;
        }

        private sealed class ProfileAccumulator
        {
            private readonly Dictionary<int, Dictionary<decimal, decimal>> _barVolumes = new();

            public Dictionary<decimal, decimal> Profile { get; } = new();
            public decimal TotalVolume { get; private set; }
            public int StartBar { get; private set; } = -1;
            public int EndBar { get; private set; } = -1;
            public DateTime BeginTimeUtc { get; private set; }
            public DateTime EndTimeUtc { get; private set; }
            public decimal High { get; private set; }
            public decimal Low { get; private set; }
            public int BarCount => _barVolumes.Count;

            public void Reset()
            {
                _barVolumes.Clear();
                Profile.Clear();
                TotalVolume = 0;
                StartBar = -1;
                EndBar = -1;
                BeginTimeUtc = default;
                EndTimeUtc = default;
                High = 0;
                Low = 0;
            }

            public void ReplaceContribution(int bar, IndicatorCandle candle)
            {
                if (_barVolumes.TryGetValue(bar, out var previousLevels))
                {
                    foreach (var previous in previousLevels)
                    {
                        if (!Profile.TryGetValue(previous.Key, out var current))
                            continue;

                        var next = current - previous.Value;
                        if (next <= 0)
                            Profile.Remove(previous.Key);
                        else
                            Profile[previous.Key] = next;
                    }
                }

                var nextLevels = candle.GetAllPriceLevels()
                    .GroupBy(level => level.Price)
                    .ToDictionary(group => group.Key, group => group.Sum(level => level.Volume));

                foreach (var next in nextLevels)
                {
                    if (Profile.ContainsKey(next.Key))
                        Profile[next.Key] += next.Value;
                    else
                        Profile[next.Key] = next.Value;
                }

                _barVolumes[bar] = nextLevels;
                TotalVolume = Profile.Values.Sum();
                StartBar = StartBar < 0 ? bar : Math.Min(StartBar, bar);
                EndBar = Math.Max(EndBar, bar);
                BeginTimeUtc = BeginTimeUtc == default || candle.Time < BeginTimeUtc ? candle.Time : BeginTimeUtc;
                var end = GetCandleEventTime(candle);
                EndTimeUtc = EndTimeUtc == default || end > EndTimeUtc ? end : EndTimeUtc;
                High = High == 0 ? candle.High : Math.Max(High, candle.High);
                Low = Low == 0 ? candle.Low : Math.Min(Low, candle.Low);
            }
        }
    }
}
