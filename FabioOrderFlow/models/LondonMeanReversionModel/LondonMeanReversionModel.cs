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
    /// 1. London is building/holding a value area.
    /// 2. Price sweeps outside VAH/VAL and closes back inside value.
    /// 3. After the failed auction, a large cumulative trade appears in the mean-reversion direction.
    /// 4. Enter toward the bulk of auction / POC.
    /// 5. Exit full position at POC, or stop quickly near the failed extreme.
    ///
    /// The same methods are used for live data and for historical replay. Historical means only
    /// "past data processed through the live rules".
    /// </summary>
    internal sealed class LondonMeanReversionModule
    {
        private readonly BalanceZoneTracker _balanceTracker;
        private readonly Action<string, bool> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly decimal _tickSize;

        private const decimal LondonBigTradeVolume = 20m;
        private const int RejectionThresholdTicks = 10;
        private const int StopInsideFailedExtremeTicks = 2;
        private const int EntryTimeoutSeconds = 1200;
        private const decimal MinRewardRiskToPoc = 1.0m;
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
        private long _liveAcceptedTradeCount;
        private DateTime _lastLiveHeartbeatUtc = DateTime.MinValue;
        private bool _processingHistoricalReplay;

        public LondonMeanReversionModule(
            BalanceZoneTracker balanceTracker,
            Action<string, bool> log,
            Func<int, IndicatorCandle> getCandle,
            decimal tickSize)
        {
            _balanceTracker = balanceTracker ?? throw new ArgumentNullException(nameof(balanceTracker));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _getCandle = getCandle ?? throw new ArgumentNullException(nameof(getCandle));
            _tickSize = tickSize > 0 ? tickSize : 1m;

            _log($"[MR_MODE] Model=FabioLondonMeanReversionCore, Modes=LIVE|HISTORICAL, BigTradeVolume={LondonBigTradeVolume:F0}, Target=POC_FULL_EXIT, Entry=FAILED_AUCTION_BACK_INSIDE_VALUE_PLUS_BIG_TRADE", false);
        }

        public IReadOnlyList<TradeRecord> CompletedTrades => _completedTrades;
        public IReadOnlyList<ActivePosition> ActivePositions => _activePositions;
        public IReadOnlyList<BalanceSetup> ActiveSetups => _activeSetups;

        public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
        {
            _currentBar = currentBar;
            ExpireSetups(GetCandleEventTime(candle));
            UpdatePocTouches(bar, candle);
            UpdateOpenPositionsFromBar(bar, candle);
        }

        public void OnNewSessionHigh(int bar, IndicatorCandle candle, decimal previousHigh)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
            if (TryCreateFailedHighSetup(bar, candle, out var setup) && setup != null)
                AddSetup(setup, "MR_SETUP_SHORT");
        }

        public void OnNewSessionLow(int bar, IndicatorCandle candle, decimal previousLow)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
            if (TryCreateFailedLowSetup(bar, candle, out var setup) && setup != null)
                AddSetup(setup, "MR_SETUP_LONG");
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
            foreach (var setup in _activeSetups)
            {
                setup.AggressionConfirmed = false;
                setup.Expired = false;
                setup.PocTouched = false;
            }

            _log($"[HISTORICAL_FLOW_PROCESS_START] Model=FabioLondonMeanReversionCore, StartBar={startBar}, EndBar={endBar}, StoredTrades={_historicalTrades.Count}, ActiveSetups={_activeSetups.Count}, ReplayStateReset=True", false);

            try
            {
                foreach (var trade in _historicalTrades)
                {
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
            if (trade.Volume < LondonBigTradeVolume)
                return;

            if (!IsLondonTradeAllowed(trade.Time))
                return;

            var key = GetLiveTradeKey(trade);
            if (_liveTradeMaxVolumeByKey.TryGetValue(key, out var seenVolume) && trade.Volume <= seenVolume)
                return;

            _liveTradeMaxVolumeByKey[key] = trade.Volume;
            ProcessAggressionTrade(trade, "Live", false);
            UpdateOpenPositionsFromTrade(trade, false);
            LogLiveHeartbeat(trade);
        }

        private bool TryCreateFailedHighSetup(int bar, IndicatorCandle candle, out BalanceSetup? setup)
        {
            setup = null;
            var poc = _balanceTracker.LastPreviewPoc;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;

            if (poc <= 0 || vah <= 0 || val <= 0)
                return false;

            if (!IsInLondonSession(candle.Time))
                return false;

            if (candle.High <= vah || candle.Close >= vah)
                return false;

            var rejection = candle.High - candle.Close;
            if (rejection < RejectionThresholdTicks * _tickSize)
                return false;

            var stop = candle.High - StopInsideFailedExtremeTicks * _tickSize;
            if (stop <= poc)
                return false;

            setup = new BalanceSetup
            {
                SetupId = Guid.NewGuid().ToString(),
                Direction = "Short",
                Source = "FailedHighBackInsideValue",
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                BreakoutPrice = candle.High,
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                POC = poc,
                VAH = vah,
                VAL = val,
                StopPrice = stop,
                TargetPrice = poc
            };

            return true;
        }

        private bool TryCreateFailedLowSetup(int bar, IndicatorCandle candle, out BalanceSetup? setup)
        {
            setup = null;
            var poc = _balanceTracker.LastPreviewPoc;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;

            if (poc <= 0 || vah <= 0 || val <= 0)
                return false;

            if (!IsInLondonSession(candle.Time))
                return false;

            if (candle.Low >= val || candle.Close <= val)
                return false;

            var rejection = candle.Close - candle.Low;
            if (rejection < RejectionThresholdTicks * _tickSize)
                return false;

            var stop = candle.Low + StopInsideFailedExtremeTicks * _tickSize;
            if (stop >= poc)
                return false;

            setup = new BalanceSetup
            {
                SetupId = Guid.NewGuid().ToString(),
                Direction = "Long",
                Source = "FailedLowBackInsideValue",
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                BreakoutPrice = candle.Low,
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                POC = poc,
                VAH = vah,
                VAL = val,
                StopPrice = stop,
                TargetPrice = poc
            };

            return true;
        }

        private void AddSetup(BalanceSetup setup, string tag)
        {
            var key = $"{setup.Direction}:{setup.RejectionBar}:{setup.BreakoutPrice:F2}:{setup.POC:F2}";
            if (!_setupKeys.Add(key))
                return;

            _activeSetups.Add(setup);
            var isHistorical = IsHistoricalContext(setup.RejectionBar);
            _log($"[{tag}] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, Source={setup.Source}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", isHistorical);
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
                .ToList())
            {
                if (!IsEntryCandidate(setup, trade, out var reason, out var risk, out var reward, out var rr))
                {
                    if (reason != "TRADE_BEFORE_REJECTION")
                        RecordEntryReject(reason);
                    continue;
                }

                setup.AggressionConfirmed = true;
                CreatePosition(setup, trade, mode, isHistorical, risk, reward, rr);
                return;
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
                TargetPrice = setup.TargetPrice,
                MaxFavorablePrice = trade.Lastprice,
                MaxAdversePrice = trade.Lastprice
            };

            _activePositions.Add(position);

            _log($"[MR_ENTRY] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, EntryModel={mode}, Source={setup.Source}, Direction={setup.Direction}, Bar={position.EntryBar}, {FormatTime(trade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={trade.Volume:F0}, TradeDirection={trade.Direction}, Stop={position.StopPrice:F2}, TargetPOC={position.TargetPrice:F2}, Risk={risk:F2}, RewardToPOC={reward:F2}, RewardRiskToPOC={rr:F2}, BigTradeVolume={LondonBigTradeVolume:F0}, SecondsAfterRejection={(trade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}", isHistorical);
        }

        private void UpdateOpenPositionsFromBar(int bar, IndicatorCandle candle)
        {
            if (_processingHistoricalReplay)
                return;

            var eventTime = GetCandleEventTime(candle);
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                UpdatePositionTracking(position, candle.High, candle.Low);
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
                CheckPositionExit(position, FindBarByTime(trade.Time, position.EntryBar), trade.Time, high, low, isHistorical);
            }
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
            var risk = position.Direction == "Long"
                ? position.EntryPrice - position.StopPrice
                : position.StopPrice - position.EntryPrice;
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
                RMultiple = rMultiple
            });

            _log($"[MR_EXIT] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={pnl:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, TargetPOC={position.TargetPrice:F2}, Stop={position.StopPrice:F2}", isHistorical);
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
                setup.Expired = true;
                RecordSetupExpiration("POC_TOUCHED_BY_TRADE");
                _log($"[MR_SETUP_EXPIRED] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, Reason=POC_TOUCHED_BEFORE_ENTRY_BY_TRADE, Direction={setup.Direction}, Bar={FindBarByTime(trade.Time, setup.RejectionBar)}, {FormatTime(trade.Time)}, POC={setup.POC:F2}, TradeFirst={trade.FirstPrice:F2}, TradeLast={trade.Lastprice:F2}, Volume={trade.Volume:F0}", isHistorical);
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

            foreach (var setup in _activeSetups.OrderBy(s => s.RejectionTimeUtc))
            {
                if (setup.AggressionConfirmed)
                    continue;

                var finalState = setup.Expired
                    ? setup.PocTouched ? "EXPIRED_POC_TOUCHED" : "EXPIRED_TIMEOUT"
                    : "NO_VALID_BIG_TRADE";
                _log($"[MR_SETUP_NO_ENTRY] SetupId={setup.SetupId}, Direction={setup.Direction}, Source={setup.Source}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, FinalState={finalState}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", true);
            }
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

        public sealed class BalanceSetup
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string Source { get; set; } = string.Empty;
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
            public decimal TargetPrice { get; set; }
            public decimal MaxFavorablePrice { get; set; }
            public decimal MaxAdversePrice { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
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
        }
    }
}
