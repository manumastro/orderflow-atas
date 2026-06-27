using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    /// <summary>
    /// London Mean Reversion Model.
    ///
    /// Live-first implementation of Fabio Valentino's London mean reversion idea:
    /// build the London value area while London is trading, wait for a sweep/fakeout
    /// back inside value, then enter only when a cumulative big trade appears in the
    /// mean-reversion direction. Historical cumulative trades use the same entry path
    /// so a loaded chart shows how the live logic would have behaved.
    /// </summary>
    internal sealed class LondonMeanReversionModule
    {
        private readonly BalanceZoneTracker _balanceTracker;
        private readonly Action<string, bool> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly decimal _tickSize;

        private const decimal MinAggressionVolume = 10m;
        private const int AggressionTimeoutSeconds = 3600;
        private const int RejectionThresholdTicks = 10;
        private const int StopOffsetTicks = 2;
        private const int LateCutoffHour = 15;
        private const int LateCutoffMinute = 30;
        private static readonly DateOnly StudyDay = new(2026, 6, 25);

        private int _currentBar;
        private readonly List<BalanceSetup> _activeSetups = new();
        private readonly List<ActivePosition> _activePositions = new();
        private readonly List<StudyPosition> _studyPositions = new();
        private readonly List<TradeRecord> _completedTrades = new();
        private readonly HashSet<string> _setupKeys = new();
        private readonly Dictionary<string, decimal> _liveTradeMaxVolumeByKey = new();
        private readonly List<CumulativeTrade> _lastHistoricalTrades = new();
        private readonly HashSet<int> _studyLoggedBars = new();
        private readonly string _studyLogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs", $"FabioOrderFlow-study-{StudyDay:yyyy-MM-dd}.log");
        private bool _studyLogInitialized;

        public class BalanceSetup
        {
            public string SetupId { get; set; } = Guid.NewGuid().ToString();
            public string Direction { get; set; } = string.Empty;
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            public int BreakoutBar { get; set; }
            public DateTime BreakoutTimeUtc { get; set; }
            public decimal BreakoutPrice { get; set; }
            public int RejectionBar { get; set; }
            public DateTime RejectionTimeUtc { get; set; }
            public decimal RejectionClose { get; set; }
            public decimal RejectionHigh { get; set; }
            public decimal RejectionLow { get; set; }
            public decimal RejectionDelta { get; set; }
            public decimal StopPrice { get; set; }
            public decimal TargetPrice { get; set; }
            public bool AggressionConfirmed { get; set; }
            public bool Expired { get; set; }
            public bool StudyFollowThroughConfirmed { get; set; }
            public bool StudyPocTriggerConfirmed { get; set; }
            public int StudyTriggerBar { get; set; } = -1;
            public DateTime StudyTriggerTimeUtc { get; set; }
        }

        public class ActivePosition
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string EntryModel { get; set; } = string.Empty;
            public decimal EntryPrice { get; set; }
            public int EntryBar { get; set; }
            public DateTime EntryTimeUtc { get; set; }
            public decimal InitialStopPrice { get; set; }
            public decimal StopPrice { get; set; }
            public decimal TargetPrice { get; set; }
            public decimal Target1Price { get; set; }
            public decimal Target2Price { get; set; }
            public bool UseTarget2 { get; set; }
            public bool Target1Hit { get; set; }
            public int Target1HitBar { get; set; } = -1;
            public bool StopProtectedAfterTarget1 { get; set; }
            public string ManagementMode { get; set; } = string.Empty;
            public string StudyTrigger { get; set; } = string.Empty;
            public decimal MaxFavorablePrice { get; set; }
            public decimal MaxAdversePrice { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public bool Closed { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal ExitPrice { get; set; }
            public int ExitBar { get; set; }
        }

        public class StudyPosition
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string EntryModel { get; set; } = string.Empty;
            public string StudyTrigger { get; set; } = string.Empty;
            public decimal EntryPrice { get; set; }
            public int EntryBar { get; set; }
            public DateTime EntryTimeUtc { get; set; }
            public decimal StopPrice { get; set; }
            public decimal Target1Price { get; set; }
            public decimal Target2Price { get; set; }
            public bool Target1Hit { get; set; }
            public decimal MaxFavorablePrice { get; set; }
            public decimal MaxAdversePrice { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public bool Closed { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal ExitPrice { get; set; }
        }

        public class TradeRecord
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string EntryModel { get; set; } = string.Empty;
            public DateTime BreakoutTime { get; set; }
            public decimal BreakoutPrice { get; set; }
            public DateTime RejectionTime { get; set; }
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            public DateTime EntryTime { get; set; }
            public decimal EntryPrice { get; set; }
            public decimal EntryVolume { get; set; }
            public DateTime ExitTime { get; set; }
            public decimal ExitPrice { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal PnL { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public decimal RMultiple { get; set; }
        }

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
        }

        public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
        {
            _currentBar = currentBar;
            LogStudyBar(bar, candle);
            UpdateStudyTriggers(bar, candle);
            UpdateActivePositions(bar, candle);
            UpdateStudyPositions(bar, candle);
        }

        public void OnNewSessionHigh(int bar, IndicatorCandle candle, decimal previousHigh)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
            if (TryCreateHighRejectionSetup(bar, candle, out var setup) && setup != null)
                AddSetup(setup, "MR_SETUP_SHORT");
        }

        public void OnNewSessionLow(int bar, IndicatorCandle candle, decimal previousLow)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
            if (TryCreateLowRejectionSetup(bar, candle, out var setup) && setup != null)
                AddSetup(setup, "MR_SETUP_LONG");
        }

        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            var allTrades = cumulativeTrades.OrderBy(t => t.Time).ToList();
            _lastHistoricalTrades.Clear();
            _lastHistoricalTrades.AddRange(allTrades);
            ResetStudyLog();
            LogStudyCumulativeTrades(allTrades);

            var trades = allTrades
                .Where(t => t.Volume >= MinAggressionVolume)
                .ToList();

            _log($"[MR_HISTORICAL_TRADES] Count={trades.Count}, MinAggressionVolume={MinAggressionVolume:F0}, ActiveSetups={_activeSetups.Count(s => !s.AggressionConfirmed && !s.Expired)}", false);

            foreach (var trade in trades)
                ProcessAggressionTrade(trade, "FootprintCumulativeTradeHistorical", true);

            LogMissedOpportunities(allTrades);
        }

        public void OnLiveCumulativeTrade(CumulativeTrade trade)
        {
            if (trade.Volume < MinAggressionVolume)
                return;

            if (!IsLondonTradeAllowed(trade.Time))
                return;

            var key = GetLiveTradeKey(trade);
            if (_liveTradeMaxVolumeByKey.TryGetValue(key, out var seenVolume) && trade.Volume <= seenVolume)
                return;

            _liveTradeMaxVolumeByKey[key] = trade.Volume;
            ProcessAggressionTrade(trade, "FootprintCumulativeTradeLive", false);
        }

        public void ProcessHistoricalPositions(int startBar, int endBar)
        {
            for (var bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                LogStudyBar(bar, candle);
                UpdateActivePositions(bar, candle);
                UpdateStudyPositions(bar, candle);
            }

            RunDayStudy();
        }

        private bool TryCreateHighRejectionSetup(int bar, IndicatorCandle candle, out BalanceSetup? setup)
        {
            setup = null;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            var poc = _balanceTracker.LastPreviewPoc;

            if (vah == 0 || val == 0 || poc == 0)
                return false;

            if (candle.High <= vah || candle.Close >= vah)
                return false;

            var rejectionDistance = candle.High - candle.Close;
            if (rejectionDistance < RejectionThresholdTicks * _tickSize)
                return false;

            setup = new BalanceSetup
            {
                Direction = "Short",
                POC = poc,
                VAH = vah,
                VAL = val,
                BreakoutBar = bar,
                BreakoutTimeUtc = candle.Time,
                BreakoutPrice = candle.High,
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                RejectionDelta = rejectionDistance,
                StopPrice = candle.High + StopOffsetTicks * _tickSize,
                TargetPrice = poc
            };

            return true;
        }

        private bool TryCreateLowRejectionSetup(int bar, IndicatorCandle candle, out BalanceSetup? setup)
        {
            setup = null;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            var poc = _balanceTracker.LastPreviewPoc;

            if (vah == 0 || val == 0 || poc == 0)
                return false;

            if (candle.Low >= val || candle.Close <= val)
                return false;

            var rejectionDistance = candle.Close - candle.Low;
            if (rejectionDistance < RejectionThresholdTicks * _tickSize)
                return false;

            setup = new BalanceSetup
            {
                Direction = "Long",
                POC = poc,
                VAH = vah,
                VAL = val,
                BreakoutBar = bar,
                BreakoutTimeUtc = candle.Time,
                BreakoutPrice = candle.Low,
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                RejectionDelta = rejectionDistance,
                StopPrice = candle.Low - StopOffsetTicks * _tickSize,
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
            _log($"[{tag}] SetupId={setup.SetupId}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", IsHistoricalBar(setup.RejectionBar));
            StudyLog($"[DAY_STUDY_SETUP] SetupId={setup.SetupId}, Tag={tag}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, RejectionHigh={setup.RejectionHigh:F2}, RejectionLow={setup.RejectionLow:F2}, RejectionDelta={setup.RejectionDelta:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", setup.RejectionTimeUtc);
        }

        private void ProcessAggressionTrade(CumulativeTrade trade, string entryModel, bool isHistorical)
        {
            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired).ToList())
            {
                if (!IsTradeInSetupWindow(setup, trade))
                    continue;

                if (!IsAggressionEntry(setup, trade))
                    continue;

                setup.AggressionConfirmed = true;
                CreatePosition(setup, trade, entryModel, isHistorical);
                break;
            }
        }

        private bool IsTradeInSetupWindow(BalanceSetup setup, CumulativeTrade trade)
        {
            if (trade.Time <= setup.RejectionTimeUtc)
                return false;

            if (trade.Time > setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                return false;

            return IsLondonTradeAllowed(trade.Time);
        }

        private bool IsAggressionEntry(BalanceSetup setup, CumulativeTrade trade)
        {
            if (trade.Volume < MinAggressionVolume)
                return false;

            if (setup.Direction == "Long")
            {
                return trade.Direction == TradeDirection.Buy && IsInEntryZone(setup, trade);
            }

            return trade.Direction == TradeDirection.Sell && IsInEntryZone(setup, trade);
        }

        private static bool IsInEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            return setup.Direction == "Long"
                ? trade.Lastprice >= setup.VAL && trade.Lastprice < setup.POC
                : trade.Lastprice <= setup.VAH && trade.Lastprice > setup.POC;
        }

        private static bool TouchesEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            var minPrice = Math.Min(trade.FirstPrice, trade.Lastprice);
            var maxPrice = Math.Max(trade.FirstPrice, trade.Lastprice);
            var rangeTouches = setup.Direction == "Long"
                ? maxPrice >= setup.VAL && minPrice < setup.POC
                : minPrice <= setup.VAH && maxPrice > setup.POC;

            if (rangeTouches)
                return true;

            return trade.Ticks.Any(t => setup.Direction == "Long"
                ? t.Price >= setup.VAL && t.Price < setup.POC
                : t.Price <= setup.VAH && t.Price > setup.POC);
        }

        private static bool IsBeyondPocContinuationEntry(BalanceSetup setup, CumulativeTrade trade)
        {
            return setup.Direction == "Long"
                ? trade.Lastprice >= setup.POC && trade.Lastprice <= setup.VAH
                : trade.Lastprice <= setup.POC && trade.Lastprice >= setup.VAL;
        }

        private void LogMissedOpportunities(List<CumulativeTrade> trades)
        {
            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired))
            {
                var trigger = GetStudyTriggerLabel(setup);
                if (trigger == "NONE")
                    continue;

                var windowEnd = setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds);
                var allWindowTrades = trades
                    .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= windowEnd)
                    .Where(t => IsInLondonSession(t.Time))
                    .ToList();

                var windowTrades = allWindowTrades
                    .Where(t => IsLondonTradeAllowed(t.Time))
                    .ToList();

                var setupBarTrades = trades
                    .Where(t => t.Time >= setup.BreakoutTimeUtc && t.Time <= setup.RejectionTimeUtc)
                    .Where(t => IsInLondonSession(t.Time))
                    .ToList();

                var setupBarOperationalTrades = setupBarTrades
                    .Where(t => IsLondonTradeAllowed(t.Time))
                    .Where(t => t.Volume >= MinAggressionVolume)
                    .ToList();

                var setupBarSameDirectionTrades = setupBarOperationalTrades
                    .Where(t => setup.Direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                    .ToList();

                var setupBarEntryZoneTrades = setupBarSameDirectionTrades
                    .Where(t => IsInEntryZone(setup, t))
                    .ToList();

                var operationalWindowTrades = windowTrades
                    .Where(t => t.Volume >= MinAggressionVolume)
                    .ToList();

                var sameDirectionTrades = operationalWindowTrades
                    .Where(t => setup.Direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                    .ToList();

                var insideValueTrades = sameDirectionTrades
                    .Where(t => IsInEntryZone(setup, t))
                    .ToList();

                var touchedEntryZoneTrades = sameDirectionTrades
                    .Where(t => !IsInEntryZone(setup, t) && TouchesEntryZone(setup, t))
                    .ToList();

                var continuationTrades = IsTarget2ManagementTrigger(trigger)
                    ? sameDirectionTrades.Where(t => IsBeyondPocContinuationEntry(setup, t)).ToList()
                    : new List<CumulativeTrade>();

                var extendedSameDirectionTrades = allWindowTrades
                    .Where(t => setup.Direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                    .ToList();

                var extendedEntryZoneTrades = extendedSameDirectionTrades
                    .Where(t => IsInEntryZone(setup, t))
                    .ToList();

                var reason = operationalWindowTrades.Count == 0
                    ? "NO_BIG_TRADE_IN_WINDOW"
                    : sameDirectionTrades.Count == 0
                        ? "NO_BIG_TRADE_IN_DIRECTION"
                        : insideValueTrades.Count == 0
                            ? "NO_BIG_TRADE_IN_ENTRY_ZONE"
                            : "UNKNOWN_NO_ENTRY";

                var maxVolume = operationalWindowTrades.Count > 0 ? operationalWindowTrades.Max(t => t.Volume) : 0m;
                var maxSameDirectionVolume = sameDirectionTrades.Count > 0 ? sameDirectionTrades.Max(t => t.Volume) : 0m;
                var firstTradeTime = operationalWindowTrades.Count > 0 ? FormatTime(operationalWindowTrades[0].Time) : "NA";
                var firstSameDirectionTime = sameDirectionTrades.Count > 0 ? FormatTime(sameDirectionTrades[0].Time) : "NA";
                var bestSameDirectionPrice = sameDirectionTrades.Count > 0
                    ? sameDirectionTrades.OrderByDescending(t => t.Volume).First().Lastprice.ToString("F2")
                    : "NA";
                var firstExtendedEntryZoneTrade = extendedEntryZoneTrades.Count > 0 ? extendedEntryZoneTrades[0] : null;
                var extendedEntryZoneAfterCutoff = extendedEntryZoneTrades.Count(t => !IsLondonTradeAllowed(t.Time));

                _log($"[MR_MISSED_OPPORTUNITY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, Reason={reason}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, SetupBarBigTrades={setupBarOperationalTrades.Count}, SetupBarSameDirectionBigTrades={setupBarSameDirectionTrades.Count}, SetupBarEntryZoneBigTrades={setupBarEntryZoneTrades.Count}, WindowBigTrades={operationalWindowTrades.Count}, SameDirectionBigTrades={sameDirectionTrades.Count}, EntryZoneBigTrades={insideValueTrades.Count}, ContinuationBigTrades={continuationTrades.Count}, TouchedEntryZoneBigTrades={touchedEntryZoneTrades.Count}, ExtendedWindowBigTrades={allWindowTrades.Count}, ExtendedSameDirectionBigTrades={extendedSameDirectionTrades.Count}, ExtendedEntryZoneBigTrades={extendedEntryZoneTrades.Count}, ExtendedEntryZoneAfterCutoff={extendedEntryZoneAfterCutoff}, MaxVolume={maxVolume:F0}, MaxSameDirectionVolume={maxSameDirectionVolume:F0}, FirstBigTrade={firstTradeTime}, FirstSameDirectionBigTrade={firstSameDirectionTime}, BestSameDirectionPrice={bestSameDirectionPrice}", true);

                if (insideValueTrades.Count == 0 && continuationTrades.Count > 0)
                {
                    var continuationTrade = continuationTrades[0];
                    var target2 = GetStudyTarget2(setup);
                    var risk = Math.Abs(continuationTrade.Lastprice - setup.StopPrice);
                    var rewardToTarget2 = Math.Abs(target2 - continuationTrade.Lastprice);
                    _log($"[MR_STUDY_CONTINUATION_ENTRY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, EntryTime={FormatTime(continuationTrade.Time)}, EntryPrice={continuationTrade.Lastprice:F2}, Volume={continuationTrade.Volume:F0}, TradeDirection={continuationTrade.Direction}, Stop={setup.StopPrice:F2}, Target2={target2:F2}, Risk={risk:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardRisk={(risk > 0 ? rewardToTarget2 / risk : 0):F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", true);
                }

                if (insideValueTrades.Count == 0 && setupBarSameDirectionTrades.Count > 0)
                {
                    var bestSetupBarTrade = setupBarSameDirectionTrades.OrderByDescending(t => t.Volume).First();
                    _log($"[MR_STUDY_SETUP_BAR_AGGRESSION] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, Reason={reason}, RejectionBar={setup.RejectionBar}, SetupBarStart={FormatTime(setup.BreakoutTimeUtc)}, SetupBarEnd={FormatTime(setup.RejectionTimeUtc)}, SetupBarBigTrades={setupBarOperationalTrades.Count}, SetupBarSameDirectionBigTrades={setupBarSameDirectionTrades.Count}, SetupBarEntryZoneBigTrades={setupBarEntryZoneTrades.Count}, BestSetupBarTradeTime={FormatTime(bestSetupBarTrade.Time)}, BestSetupBarTradePrice={bestSetupBarTrade.Lastprice:F2}, BestSetupBarTradeVolume={bestSetupBarTrade.Volume:F0}, BestSetupBarTradeDirection={bestSetupBarTrade.Direction}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}", true);
                }

                if (insideValueTrades.Count == 0 && touchedEntryZoneTrades.Count > 0)
                {
                    var touchedTrade = touchedEntryZoneTrades[0];
                    var tickTouchCount = touchedTrade.Ticks.Count(t => setup.Direction == "Long"
                        ? t.Price >= setup.VAL && t.Price < setup.POC
                        : t.Price <= setup.VAH && t.Price > setup.POC);
                    _log($"[MR_STUDY_PRICE_TOUCH_OPPORTUNITY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, TouchTime={FormatTime(touchedTrade.Time)}, Volume={touchedTrade.Volume:F0}, TradeDirection={touchedTrade.Direction}, FirstPrice={touchedTrade.FirstPrice:F2}, LastPrice={touchedTrade.Lastprice:F2}, TickTouchCount={tickTouchCount}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}", true);
                }

                if (insideValueTrades.Count == 0 && firstExtendedEntryZoneTrade != null)
                {
                    _log($"[MR_EXTENDED_CUTOFF_OPPORTUNITY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, ExtendedEntryTime={FormatTime(firstExtendedEntryZoneTrade.Time)}, ExtendedEntryPrice={firstExtendedEntryZoneTrade.Lastprice:F2}, ExtendedEntryVolume={firstExtendedEntryZoneTrade.Volume:F0}, ExtendedEntryDirection={firstExtendedEntryZoneTrade.Direction}, OperationalCutoffLondon={LateCutoffHour:D2}:{LateCutoffMinute:D2}, SessionEndLondon=16:00, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}", true);
                }
            }
        }

        private void CreatePosition(BalanceSetup setup, CumulativeTrade entryTrade, string entryModel, bool isHistorical)
        {
            var entryBar = FindBarByTime(entryTrade.Time, setup.RejectionBar);
            var studyTrigger = GetStudyTriggerLabel(setup);
            var target2 = GetStudyTarget2(setup);
            var useTarget2 = IsTarget2ManagementTrigger(studyTrigger);
            var finalTarget = useTarget2 ? target2 : setup.TargetPrice;
            var position = new ActivePosition
            {
                SetupId = setup.SetupId,
                Direction = setup.Direction,
                EntryModel = entryModel,
                EntryPrice = entryTrade.Lastprice,
                EntryBar = entryBar,
                EntryTimeUtc = entryTrade.Time,
                InitialStopPrice = setup.StopPrice,
                StopPrice = setup.StopPrice,
                TargetPrice = finalTarget,
                Target1Price = setup.POC,
                Target2Price = target2,
                UseTarget2 = useTarget2,
                ManagementMode = useTarget2 ? "HYBRID_TARGET2_AFTER_POC" : "POC_ONLY",
                StudyTrigger = studyTrigger,
                MaxFavorablePrice = entryTrade.Lastprice,
                MaxAdversePrice = entryTrade.Lastprice
            };

            _activePositions.Add(position);

            var riskPoints = setup.Direction == "Long"
                ? position.EntryPrice - position.InitialStopPrice
                : position.InitialStopPrice - position.EntryPrice;
            var rewardPoints = setup.Direction == "Long"
                ? setup.TargetPrice - position.EntryPrice
                : position.EntryPrice - setup.TargetPrice;
            var rewardToTarget2 = setup.Direction == "Long"
                ? target2 - position.EntryPrice
                : position.EntryPrice - target2;
            var rewardToFinalTarget = setup.Direction == "Long"
                ? position.TargetPrice - position.EntryPrice
                : position.EntryPrice - position.TargetPrice;

            _log($"[MR_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, StudyTarget2={target2:F2}, StudyTrigger={studyTrigger}, Risk={riskPoints:F2}, Reward={rewardPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}", isHistorical);
            StudyLog($"[DAY_STUDY_ACTUAL_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, StudyTarget2={target2:F2}, StudyTrigger={studyTrigger}, Risk={riskPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}", entryTrade.Time);

            CreateStudyPosition(setup, entryTrade, entryModel, entryBar, studyTrigger, target2, isHistorical);

            if (isHistorical)
                ProcessHistoricalPositions(entryBar, Math.Max(entryBar, _currentBar - 1));
        }

        private void UpdateStudyTriggers(int bar, IndicatorCandle candle)
        {
            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired))
            {
                if (bar <= setup.RejectionBar)
                    continue;

                var eventTime = GetCandleEventTime(candle);
                if (eventTime > setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                    continue;

                if (!setup.StudyFollowThroughConfirmed && IsStudyFollowThrough(setup, candle))
                {
                    setup.StudyFollowThroughConfirmed = true;
                    setup.StudyTriggerBar = bar;
                    setup.StudyTriggerTimeUtc = eventTime;
                    _log($"[MR_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetFollowThroughLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, CandidateHigh={setup.RejectionHigh:F2}, CandidateLow={setup.RejectionLow:F2}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", IsHistoricalBar(bar));
                    StudyLog($"[DAY_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetFollowThroughLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, CandidateHigh={setup.RejectionHigh:F2}, CandidateLow={setup.RejectionLow:F2}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", eventTime);
                }

                if (!setup.StudyPocTriggerConfirmed && IsStudyPocTrigger(setup, candle))
                {
                    setup.StudyPocTriggerConfirmed = true;
                    setup.StudyTriggerBar = bar;
                    setup.StudyTriggerTimeUtc = eventTime;
                    _log($"[MR_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetPocTriggerLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", IsHistoricalBar(bar));
                    StudyLog($"[DAY_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetPocTriggerLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", eventTime);
                }
            }
        }

        private void UpdateActivePositions(int bar, IndicatorCandle candle)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                if (bar < position.EntryBar)
                    continue;

                UpdatePositionTracking(position, bar, candle);
                CheckPositionExit(position, bar, candle);
            }
        }

        private void UpdatePositionTracking(ActivePosition position, int bar, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                if (candle.High > position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.High;
                    position.MFE = candle.High - position.EntryPrice;
                    LogMfe(position, bar, candle);
                }

                if (candle.Low < position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.Low;
                    position.MAE = position.EntryPrice - candle.Low;
                }
            }
            else
            {
                if (candle.Low < position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.Low;
                    position.MFE = position.EntryPrice - candle.Low;
                    LogMfe(position, bar, candle);
                }

                if (candle.High > position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.High;
                    position.MAE = candle.High - position.EntryPrice;
                }
            }
        }

        private void CheckPositionExit(ActivePosition position, int bar, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price > position.EntryPrice && candle.High >= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, candle);

                    if (position.Target2Price > position.EntryPrice && candle.High >= position.Target2Price)
                    {
                        ClosePosition(position, bar, candle, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price > position.EntryPrice && candle.High >= position.Target1Price)
                {
                    ClosePosition(position, bar, candle, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (candle.Low <= position.StopPrice && CanStopTrigger(position, bar))
                {
                    ClosePosition(position, bar, candle, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
                    return;
                }
            }
            else
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price < position.EntryPrice && candle.Low <= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, candle);

                    if (position.Target2Price < position.EntryPrice && candle.Low <= position.Target2Price)
                    {
                        ClosePosition(position, bar, candle, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price < position.EntryPrice && candle.Low <= position.Target1Price)
                {
                    ClosePosition(position, bar, candle, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (candle.High >= position.StopPrice && CanStopTrigger(position, bar))
                {
                    ClosePosition(position, bar, candle, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
                    return;
                }
            }

            var londonTime = MarketTimeZones.ToLondon(GetCandleEventTime(candle));
            if (londonTime.Hour >= 16)
                ClosePosition(position, bar, candle, "LONDON_CLOSE", candle.Close);
        }

        private static bool CanStopTrigger(ActivePosition position, int bar)
        {
            return !position.StopProtectedAfterTarget1 || bar > position.Target1HitBar;
        }

        private void ProtectStopAfterTarget1(ActivePosition position, int bar, IndicatorCandle candle)
        {
            position.Target1Hit = true;
            position.Target1HitBar = bar;

            var protectedStop = position.Direction == "Long"
                ? Math.Max(position.EntryPrice, position.Target1Price - 2m * _tickSize)
                : Math.Min(position.EntryPrice, position.Target1Price + 2m * _tickSize);

            var oldStop = position.StopPrice;
            position.StopPrice = protectedStop;
            position.StopProtectedAfterTarget1 = true;

            var reward = position.Direction == "Long"
                ? position.Target1Price - position.EntryPrice
                : position.EntryPrice - position.Target1Price;

            _log($"[MR_TARGET1_HIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, Entry={position.EntryPrice:F2}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, RewardPoints={reward:F2}, OldStop={oldStop:F2}, ProtectedStop={position.StopPrice:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}", IsHistoricalBar(bar));
        }

        private void ClosePosition(ActivePosition position, int bar, IndicatorCandle candle, string exitReason, decimal exitPrice)
        {
            if (position.Closed)
                return;

            position.Closed = true;
            position.ExitReason = exitReason;
            position.ExitPrice = exitPrice;
            position.ExitBar = bar;

            var pnl = position.Direction == "Long"
                ? exitPrice - position.EntryPrice
                : position.EntryPrice - exitPrice;
            var risk = position.Direction == "Long"
                ? position.EntryPrice - position.InitialStopPrice
                : position.InitialStopPrice - position.EntryPrice;
            var rMultiple = risk != 0 ? pnl / risk : 0;

            _log($"[MR_EXIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={pnl:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, Target1Hit={position.Target1Hit}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, StopProtected={position.StopProtectedAfterTarget1}", IsHistoricalBar(bar));

            var setup = _activeSetups.FirstOrDefault(s => s.SetupId == position.SetupId);
            if (setup == null)
                return;

            _completedTrades.Add(new TradeRecord
            {
                SetupId = position.SetupId,
                Direction = position.Direction,
                EntryModel = position.EntryModel,
                BreakoutTime = setup.BreakoutTimeUtc,
                BreakoutPrice = setup.BreakoutPrice,
                RejectionTime = setup.RejectionTimeUtc,
                POC = setup.POC,
                VAH = setup.VAH,
                VAL = setup.VAL,
                EntryTime = position.EntryTimeUtc,
                EntryPrice = position.EntryPrice,
                ExitTime = GetCandleEventTime(candle),
                ExitPrice = exitPrice,
                ExitReason = exitReason,
                PnL = pnl,
                MFE = position.MFE,
                MAE = position.MAE,
                RMultiple = rMultiple
            });
        }

        private void LogMfe(ActivePosition position, int bar, IndicatorCandle candle)
        {
            _log($"[MR_MFE_UPDATE] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, EntryPrice={position.EntryPrice:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, MaxFavorablePrice={position.MaxFavorablePrice:F2}, MaxAdversePrice={position.MaxAdversePrice:F2}", IsHistoricalBar(bar));
        }

        private void CreateStudyPosition(BalanceSetup setup, CumulativeTrade entryTrade, string entryModel, int entryBar, string studyTrigger, decimal target2, bool isHistorical)
        {
            var position = new StudyPosition
            {
                SetupId = setup.SetupId,
                Direction = setup.Direction,
                EntryModel = entryModel,
                StudyTrigger = studyTrigger,
                EntryPrice = entryTrade.Lastprice,
                EntryBar = entryBar,
                EntryTimeUtc = entryTrade.Time,
                StopPrice = setup.StopPrice,
                Target1Price = setup.POC,
                Target2Price = target2,
                MaxFavorablePrice = entryTrade.Lastprice,
                MaxAdversePrice = entryTrade.Lastprice
            };

            _studyPositions.Add(position);
            _log($"[MR_STUDY_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={entryTrade.Lastprice:F2}, Stop={setup.StopPrice:F2}, Target1POC={setup.POC:F2}, Target2={target2:F2}, StudyTrigger={studyTrigger}", isHistorical);
        }

        private void UpdateStudyPositions(int bar, IndicatorCandle candle)
        {
            foreach (var position in _studyPositions.Where(p => !p.Closed).ToList())
            {
                if (bar < position.EntryBar)
                    continue;

                UpdateStudyPositionTracking(position, candle);
                CheckStudyPositionExit(position, bar, candle);
            }
        }

        private void UpdateStudyPositionTracking(StudyPosition position, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                if (candle.High > position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.High;
                    position.MFE = candle.High - position.EntryPrice;
                }

                if (candle.Low < position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.Low;
                    position.MAE = position.EntryPrice - candle.Low;
                }
            }
            else
            {
                if (candle.Low < position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.Low;
                    position.MFE = position.EntryPrice - candle.Low;
                }

                if (candle.High > position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.High;
                    position.MAE = candle.High - position.EntryPrice;
                }
            }
        }

        private void CheckStudyPositionExit(StudyPosition position, int bar, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                if (!position.Target1Hit && position.Target1Price > position.EntryPrice && candle.High >= position.Target1Price)
                    LogStudyTarget1(position, bar, candle);

                if (position.Target2Price > position.EntryPrice && candle.High >= position.Target2Price)
                {
                    CloseStudyPosition(position, bar, candle, "TARGET2_HIT", position.Target2Price);
                    return;
                }

                if (candle.Low <= position.StopPrice)
                {
                    CloseStudyPosition(position, bar, candle, "STOP_HIT", position.StopPrice);
                    return;
                }
            }
            else
            {
                if (!position.Target1Hit && position.Target1Price < position.EntryPrice && candle.Low <= position.Target1Price)
                    LogStudyTarget1(position, bar, candle);

                if (position.Target2Price < position.EntryPrice && candle.Low <= position.Target2Price)
                {
                    CloseStudyPosition(position, bar, candle, "TARGET2_HIT", position.Target2Price);
                    return;
                }

                if (candle.High >= position.StopPrice)
                {
                    CloseStudyPosition(position, bar, candle, "STOP_HIT", position.StopPrice);
                    return;
                }
            }

            var londonTime = MarketTimeZones.ToLondon(GetCandleEventTime(candle));
            if (londonTime.Hour >= 16)
                CloseStudyPosition(position, bar, candle, "LONDON_CLOSE", candle.Close);
        }

        private void LogStudyTarget1(StudyPosition position, int bar, IndicatorCandle candle)
        {
            position.Target1Hit = true;
            var reward = position.Direction == "Long"
                ? position.Target1Price - position.EntryPrice
                : position.EntryPrice - position.Target1Price;
            _log($"[MR_STUDY_TARGET1_HIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, Entry={position.EntryPrice:F2}, Target1POC={position.Target1Price:F2}, RewardPoints={reward:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}", IsHistoricalBar(bar));
        }

        private void CloseStudyPosition(StudyPosition position, int bar, IndicatorCandle candle, string exitReason, decimal exitPrice)
        {
            if (position.Closed)
                return;

            position.Closed = true;
            position.ExitReason = exitReason;
            position.ExitPrice = exitPrice;

            var pnl = position.Direction == "Long"
                ? exitPrice - position.EntryPrice
                : position.EntryPrice - exitPrice;
            var risk = position.Direction == "Long"
                ? position.EntryPrice - position.StopPrice
                : position.StopPrice - position.EntryPrice;
            var rMultiple = risk != 0 ? pnl / risk : 0;

            _log($"[MR_STUDY_CLOSE] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={pnl:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, Target1Hit={position.Target1Hit}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}", IsHistoricalBar(bar));
        }

        private void ResetStudyLog()
        {
            _studyLoggedBars.Clear();
            _studyLogInitialized = false;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_studyLogPath)!);
                File.WriteAllText(_studyLogPath, $"[DAY_STUDY_START] Day={StudyDay:yyyy-MM-dd}, MinAggressionVolume={MinAggressionVolume:F0}, CreatedItaly={MarketTimeZones.ToItaly(DateTime.UtcNow):yyyy-MM-dd HH:mm:ss}{Environment.NewLine}");
                _studyLogInitialized = true;
            }
            catch
            {
            }
        }

        private void StudyLog(string message, DateTime eventUtc)
        {
            if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventUtc)) != StudyDay)
                return;

            try
            {
                if (!_studyLogInitialized)
                    ResetStudyLog();

                File.AppendAllText(_studyLogPath, message + Environment.NewLine);
            }
            catch
            {
            }
        }

        private void LogStudyBar(int bar, IndicatorCandle candle)
        {
            if (!_studyLoggedBars.Add(bar))
                return;

            if (!IsInLondonSession(candle.Time))
                return;

            if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(candle.Time)) != StudyDay)
                return;

            var (bid, ask, delta, topLevels) = GetCandleVolumeDiagnostics(candle);
            var relation = GetPriceLocation(candle.Close, _balanceTracker.LastPreviewPoc, _balanceTracker.LastPreviewVah, _balanceTracker.LastPreviewVal);
            StudyLog($"[DAY_STUDY_BAR] Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, Open={candle.Open:F2}, High={candle.High:F2}, Low={candle.Low:F2}, Close={candle.Close:F2}, Volume={candle.Volume:F0}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}, PreviewPOC={_balanceTracker.LastPreviewPoc:F2}, PreviewVAH={_balanceTracker.LastPreviewVah:F2}, PreviewVAL={_balanceTracker.LastPreviewVal:F2}, CloseLocation={relation}, DistToPOC={candle.Close - _balanceTracker.LastPreviewPoc:F2}, DistToVAH={candle.Close - _balanceTracker.LastPreviewVah:F2}, DistToVAL={candle.Close - _balanceTracker.LastPreviewVal:F2}, TopLevels={topLevels}", GetCandleEventTime(candle));
        }

        private void LogStudyCumulativeTrades(List<CumulativeTrade> trades)
        {
            foreach (var trade in trades.Where(t => t.Volume >= MinAggressionVolume && IsInLondonSession(t.Time)))
            {
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(trade.Time)) != StudyDay)
                    continue;

                var nearestSetup = FindNearestStudySetup(trade);
                StudyLog($"[DAY_STUDY_BIG_TRADE] {FormatTime(trade.Time)}, Direction={trade.Direction}, Volume={trade.Volume:F0}, FirstPrice={trade.FirstPrice:F2}, LastPrice={trade.Lastprice:F2}, TickCount={trade.Ticks.Count}, Location={GetTradeLocation(trade.Lastprice)}, NearestSetupId={nearestSetup.SetupId}, NearestSetupDirection={nearestSetup.Direction}, SecondsAfterSetup={nearestSetup.SecondsAfter:F1}, SameDirectionAsSetup={nearestSetup.SameDirection}, InEntryZone={nearestSetup.InEntryZone}, ContinuationZone={nearestSetup.ContinuationZone}, TouchesEntryZone={nearestSetup.TouchesEntryZone}", trade.Time);
            }
        }

        private void RunDayStudy()
        {
            if (_lastHistoricalTrades.Count == 0)
                return;

            var studySetups = _activeSetups
                .Where(s => DateOnly.FromDateTime(MarketTimeZones.ToItaly(s.RejectionTimeUtc)) == StudyDay)
                .ToList();

            foreach (var setup in studySetups)
            {
                var trigger = GetStudyTriggerLabel(setup);
                var windowEnd = setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds);
                var candidates = _lastHistoricalTrades
                    .Where(t => t.Volume >= MinAggressionVolume)
                    .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= windowEnd)
                    .Where(t => IsLondonTradeAllowed(t.Time))
                    .Where(t => setup.Direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                    .Select(t => BuildStudyCandidate(setup, t, trigger))
                    .Where(c => c.CandidateType != "NOT_CANDIDATE")
                    .ToList();

                StudyLog($"[DAY_STUDY_SETUP_SUMMARY] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={trigger}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, CandidateCount={candidates.Count}, AggressionConfirmed={setup.AggressionConfirmed}", setup.RejectionTimeUtc);

                foreach (var candidate in candidates)
                {
                    var outcome = EvaluateCandidateOutcome(candidate.Direction, candidate.EntryPrice, candidate.Stop, candidate.TargetPoc, candidate.Target2, candidate.EntryTimeUtc, setup.RejectionBar);
                    StudyLog($"[DAY_STUDY_CANDIDATE_ENTRY] SetupId={setup.SetupId}, CandidateType={candidate.CandidateType}, Trigger={trigger}, Direction={candidate.Direction}, EntryTime={FormatTime(candidate.EntryTimeUtc)}, EntryPrice={candidate.EntryPrice:F2}, Volume={candidate.Volume:F0}, Stop={candidate.Stop:F2}, TargetPOC={candidate.TargetPoc:F2}, Target2={candidate.Target2:F2}, Risk={candidate.Risk:F2}, RewardPOC={candidate.RewardPoc:F2}, RewardT2={candidate.RewardT2:F2}, RR_POC={(candidate.Risk > 0 ? candidate.RewardPoc / candidate.Risk : 0):F2}, RR_T2={(candidate.Risk > 0 ? candidate.RewardT2 / candidate.Risk : 0):F2}, OutcomePOC={outcome.OutcomePoc}, PnLPOC={outcome.PnlPoc:F2}, OutcomeT2={outcome.OutcomeT2}, PnLT2={outcome.PnlT2:F2}, MFE={outcome.Mfe:F2}, MAE={outcome.Mae:F2}", candidate.EntryTimeUtc);
                }
            }
        }

        private StudyCandidate BuildStudyCandidate(BalanceSetup setup, CumulativeTrade trade, string trigger)
        {
            var inEntryZone = IsInEntryZone(setup, trade);
            var continuation = IsTarget2ManagementTrigger(trigger) && IsBeyondPocContinuationEntry(setup, trade);
            var candidateType = inEntryZone
                ? "VALUE_REENTRY_BIG_TRADE"
                : continuation
                    ? "POC_ACCEPTANCE_CONTINUATION"
                    : "NOT_CANDIDATE";

            var target2 = GetStudyTarget2(setup);
            var risk = Math.Abs(trade.Lastprice - setup.StopPrice);
            var rewardPoc = Math.Abs(setup.POC - trade.Lastprice);
            var rewardT2 = Math.Abs(target2 - trade.Lastprice);

            return new StudyCandidate(setup.Direction, candidateType, trade.Time, trade.Lastprice, trade.Volume, setup.StopPrice, setup.POC, target2, risk, rewardPoc, rewardT2);
        }

        private StudyOutcome EvaluateCandidateOutcome(string direction, decimal entry, decimal stop, decimal targetPoc, decimal target2, DateTime entryTimeUtc, int fallbackBar)
        {
            var entryBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var mfe = 0m;
            var mae = 0m;
            var outcomePoc = "OPEN";
            var outcomeT2 = "OPEN";
            var pnlPoc = 0m;
            var pnlT2 = 0m;

            for (var bar = entryBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                if (!IsInLondonSession(candle.Time))
                    break;

                if (direction == "Long")
                {
                    mfe = Math.Max(mfe, candle.High - entry);
                    mae = Math.Max(mae, entry - candle.Low);
                    if (outcomePoc == "OPEN" && candle.Low <= stop)
                    {
                        outcomePoc = "STOP_HIT";
                        pnlPoc = stop - entry;
                    }
                    if (outcomePoc == "OPEN" && candle.High >= targetPoc)
                    {
                        outcomePoc = "POC_HIT";
                        pnlPoc = targetPoc - entry;
                    }
                    if (outcomeT2 == "OPEN" && candle.Low <= stop)
                    {
                        outcomeT2 = "STOP_HIT";
                        pnlT2 = stop - entry;
                    }
                    if (outcomeT2 == "OPEN" && candle.High >= target2)
                    {
                        outcomeT2 = "TARGET2_HIT";
                        pnlT2 = target2 - entry;
                    }
                }
                else
                {
                    mfe = Math.Max(mfe, entry - candle.Low);
                    mae = Math.Max(mae, candle.High - entry);
                    if (outcomePoc == "OPEN" && candle.High >= stop)
                    {
                        outcomePoc = "STOP_HIT";
                        pnlPoc = entry - stop;
                    }
                    if (outcomePoc == "OPEN" && candle.Low <= targetPoc)
                    {
                        outcomePoc = "POC_HIT";
                        pnlPoc = entry - targetPoc;
                    }
                    if (outcomeT2 == "OPEN" && candle.High >= stop)
                    {
                        outcomeT2 = "STOP_HIT";
                        pnlT2 = entry - stop;
                    }
                    if (outcomeT2 == "OPEN" && candle.Low <= target2)
                    {
                        outcomeT2 = "TARGET2_HIT";
                        pnlT2 = entry - target2;
                    }
                }
            }

            return new StudyOutcome(outcomePoc, pnlPoc, outcomeT2, pnlT2, mfe, mae);
        }

        private (decimal Bid, decimal Ask, decimal Delta, string TopLevels) GetCandleVolumeDiagnostics(IndicatorCandle candle)
        {
            var levels = candle.GetAllPriceLevels();
            var bid = levels.Sum(level => level.Bid);
            var ask = levels.Sum(level => level.Ask);
            var delta = ask - bid;
            var topLevels = string.Join(";", levels
                .OrderByDescending(level => level.Volume)
                .Take(5)
                .Select(level => $"{level.Price:F2}:V{level.Volume:F0}/D{level.Ask - level.Bid:F0}"));

            return (bid, ask, delta, topLevels);
        }

        private string GetTradeLocation(decimal price)
        {
            return GetPriceLocation(price, _balanceTracker.LastPreviewPoc, _balanceTracker.LastPreviewVah, _balanceTracker.LastPreviewVal);
        }

        private static string GetPriceLocation(decimal price, decimal poc, decimal vah, decimal val)
        {
            if (poc == 0 || vah == 0 || val == 0)
                return "NO_PROFILE";

            if (price > vah)
                return "ABOVE_VAH";
            if (price >= poc)
                return "POC_TO_VAH";
            if (price >= val)
                return "VAL_TO_POC";
            return "BELOW_VAL";
        }

        private (string SetupId, string Direction, double SecondsAfter, bool SameDirection, bool InEntryZone, bool ContinuationZone, bool TouchesEntryZone) FindNearestStudySetup(CumulativeTrade trade)
        {
            var setup = _activeSetups
                .Where(s => DateOnly.FromDateTime(MarketTimeZones.ToItaly(s.RejectionTimeUtc)) == StudyDay)
                .Where(s => trade.Time >= s.RejectionTimeUtc && trade.Time <= s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .OrderBy(s => Math.Abs((trade.Time - s.RejectionTimeUtc).TotalSeconds))
                .FirstOrDefault();

            if (setup == null)
                return ("NA", "NA", 0, false, false, false, false);

            var sameDirection = setup.Direction == "Long" ? trade.Direction == TradeDirection.Buy : trade.Direction == TradeDirection.Sell;
            return (setup.SetupId, setup.Direction, (trade.Time - setup.RejectionTimeUtc).TotalSeconds, sameDirection, IsInEntryZone(setup, trade), IsBeyondPocContinuationEntry(setup, trade), TouchesEntryZone(setup, trade));
        }

        private sealed record StudyCandidate(string Direction, string CandidateType, DateTime EntryTimeUtc, decimal EntryPrice, decimal Volume, decimal Stop, decimal TargetPoc, decimal Target2, decimal Risk, decimal RewardPoc, decimal RewardT2);
        private sealed record StudyOutcome(string OutcomePoc, decimal PnlPoc, string OutcomeT2, decimal PnlT2, decimal Mfe, decimal Mae);

        private bool IsStudyFollowThrough(BalanceSetup setup, IndicatorCandle candle)
        {
            return setup.Direction == "Long"
                ? candle.Close > setup.RejectionHigh
                : candle.Close < setup.RejectionLow;
        }

        private bool IsStudyPocTrigger(BalanceSetup setup, IndicatorCandle candle)
        {
            return setup.Direction == "Long"
                ? candle.Close > setup.POC
                : candle.Close < setup.POC;
        }

        private string GetStudyTriggerLabel(BalanceSetup setup)
        {
            if (setup.StudyPocTriggerConfirmed)
                return GetPocTriggerLabel(setup);

            if (setup.StudyFollowThroughConfirmed)
                return GetFollowThroughLabel(setup);

            return "NONE";
        }

        private static string GetFollowThroughLabel(BalanceSetup setup)
        {
            return setup.Direction == "Long"
                ? "LOW_REJECTION_FOLLOW_THROUGH"
                : "HIGH_REJECTION_FOLLOW_THROUGH";
        }

        private static string GetPocTriggerLabel(BalanceSetup setup)
        {
            return setup.Direction == "Long"
                ? "POC_RECLAIM_AFTER_LOW_REJECTION"
                : "POC_LOSS_AFTER_HIGH_REJECTION";
        }

        private decimal GetStudyTarget2(BalanceSetup setup)
        {
            return setup.Direction == "Long" ? setup.VAH : setup.VAL;
        }

        private static bool IsTarget2ManagementTrigger(string studyTrigger)
        {
            return studyTrigger == "POC_RECLAIM_AFTER_LOW_REJECTION"
                || studyTrigger == "POC_LOSS_AFTER_HIGH_REJECTION";
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

        private bool IsLondonTradeAllowed(DateTime utcTime)
        {
            var london = MarketTimeZones.ToLondon(utcTime);
            var minutes = london.Hour * 60 + london.Minute;
            var start = 8 * 60;
            var cutoff = LateCutoffHour * 60 + LateCutoffMinute;
            return minutes >= start && minutes < cutoff;
        }

        private bool IsInLondonSession(DateTime utcTime)
        {
            var london = MarketTimeZones.ToLondon(utcTime);
            var minutes = london.Hour * 60 + london.Minute;
            return minutes >= 8 * 60 && minutes < 16 * 60;
        }

        private static DateTime GetCandleEventTime(IndicatorCandle candle)
        {
            return candle.LastTime > candle.Time ? candle.LastTime : candle.Time;
        }

        private string GetLiveTradeKey(CumulativeTrade trade)
        {
            return $"{trade.Time.Ticks}:{trade.Direction}:{trade.FirstPrice:F2}";
        }

        private string FormatTime(DateTime utc)
        {
            return MarketTimeZones.FormatUtcLondonItaly(utc);
        }

        private bool IsHistoricalBar(int bar)
        {
            return bar < _currentBar - 1;
        }

        public IReadOnlyList<TradeRecord> CompletedTrades => _completedTrades;
        public IReadOnlyList<ActivePosition> ActivePositions => _activePositions;
        public IReadOnlyList<BalanceSetup> ActiveSetups => _activeSetups;
    }
}
