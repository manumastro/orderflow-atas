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
        private const decimal MinRewardRiskToTarget2 = 1.0m;

        private int _currentBar;
        private readonly List<BalanceSetup> _activeSetups = new();
        private readonly List<ActivePosition> _activePositions = new();

        private readonly List<TradeRecord> _completedTrades = new();
        private readonly HashSet<string> _setupKeys = new();
        private readonly Dictionary<string, decimal> _liveTradeMaxVolumeByKey = new();
        private readonly List<CumulativeTrade> _lastHistoricalTrades = new();
        private readonly HashSet<int> _studyLoggedBars = new();
        private readonly string _studyLogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs", "FabioOrderFlow-study-historical.log");
        private bool _studyLogInitialized;
        private bool _dayStudyCompleted;

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
            _dayStudyCompleted = false;
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

            if (!IsTarget2ManagementTrigger(GetStudyTriggerLabel(setup)))
                return false;

            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction != expectedDirection)
                return false;

            if (!IsInEntryZone(setup, trade))
                return false;

            return GetRewardRiskToTarget2(setup, trade.Lastprice) >= MinRewardRiskToTarget2;
        }

        private static bool IsInEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            return setup.Direction == "Long"
                ? trade.Lastprice >= setup.VAL && trade.Lastprice < setup.POC
                : trade.Lastprice <= setup.VAH && trade.Lastprice > setup.POC;
        }

        private static bool IsBeyondPocContinuationEntry(BalanceSetup setup, CumulativeTrade trade)
        {
            return setup.Direction == "Long"
                ? trade.Lastprice >= setup.POC && trade.Lastprice <= setup.VAH
                : trade.Lastprice <= setup.POC && trade.Lastprice >= setup.VAL;
        }

        private decimal GetRewardRiskToTarget2(BalanceSetup setup, decimal entryPrice)
        {
            var risk = Math.Abs(entryPrice - setup.StopPrice);
            if (risk <= 0)
                return 0;

            var reward = Math.Abs(GetStudyTarget2(setup) - entryPrice);
            return reward / risk;
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

                var operationalWindowTrades = windowTrades
                    .Where(t => t.Volume >= MinAggressionVolume)
                    .ToList();

                var sameDirectionTrades = operationalWindowTrades
                    .Where(t => setup.Direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                    .ToList();

                var insideValueTrades = sameDirectionTrades
                    .Where(t => IsInEntryZone(setup, t))
                    .ToList();

                var validRrEntryTrades = insideValueTrades
                    .Where(t => IsTarget2ManagementTrigger(trigger))
                    .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                    .ToList();

                var continuationTrades = IsTarget2ManagementTrigger(trigger)
                    ? sameDirectionTrades.Where(t => IsBeyondPocContinuationEntry(setup, t)).ToList()
                    : new List<CumulativeTrade>();

                var reason = trigger == "NONE" || !IsTarget2ManagementTrigger(trigger)
                    ? "NO_POC_RECLAIM_OR_LOSS_TRIGGER"
                    : operationalWindowTrades.Count == 0
                        ? "NO_BIG_TRADE_IN_WINDOW"
                        : sameDirectionTrades.Count == 0
                            ? "NO_BIG_TRADE_IN_DIRECTION"
                            : insideValueTrades.Count == 0
                                ? "NO_BIG_TRADE_IN_ENTRY_ZONE"
                                : validRrEntryTrades.Count == 0
                                    ? "RR_TO_TARGET2_TOO_LOW"
                                    : "UNKNOWN_NO_ENTRY";

                var maxVolume = operationalWindowTrades.Count > 0 ? operationalWindowTrades.Max(t => t.Volume) : 0m;
                var maxSameDirectionVolume = sameDirectionTrades.Count > 0 ? sameDirectionTrades.Max(t => t.Volume) : 0m;
                var firstTradeTime = operationalWindowTrades.Count > 0 ? FormatTime(operationalWindowTrades[0].Time) : "NA";
                var firstSameDirectionTime = sameDirectionTrades.Count > 0 ? FormatTime(sameDirectionTrades[0].Time) : "NA";
                var bestSameDirectionPrice = sameDirectionTrades.Count > 0
                    ? sameDirectionTrades.OrderByDescending(t => t.Volume).First().Lastprice.ToString("F2")
                    : "NA";
                _log($"[MR_MISSED_OPPORTUNITY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, Reason={reason}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, WindowBigTrades={operationalWindowTrades.Count}, SameDirectionBigTrades={sameDirectionTrades.Count}, EntryZoneBigTrades={insideValueTrades.Count}, ValidRrEntryTrades={validRrEntryTrades.Count}, ContinuationBigTrades={continuationTrades.Count}, MaxVolume={maxVolume:F0}, MaxSameDirectionVolume={maxSameDirectionVolume:F0}, FirstBigTrade={firstTradeTime}, FirstSameDirectionBigTrade={firstSameDirectionTime}, BestSameDirectionPrice={bestSameDirectionPrice}", true);

                if (insideValueTrades.Count == 0 && continuationTrades.Count > 0)
                {
                    var continuationTrade = continuationTrades[0];
                    var target2 = GetStudyTarget2(setup);
                    var risk = Math.Abs(continuationTrade.Lastprice - setup.StopPrice);
                    var rewardToTarget2 = Math.Abs(target2 - continuationTrade.Lastprice);
                    _log($"[MR_STUDY_CONTINUATION_ENTRY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, EntryTime={FormatTime(continuationTrade.Time)}, EntryPrice={continuationTrade.Lastprice:F2}, Volume={continuationTrade.Volume:F0}, TradeDirection={continuationTrade.Direction}, Stop={setup.StopPrice:F2}, Target2={target2:F2}, Risk={risk:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardRisk={(risk > 0 ? rewardToTarget2 / risk : 0):F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", true);
                }

            }
        }

        private void CreatePosition(BalanceSetup setup, CumulativeTrade entryTrade, string entryModel, bool isHistorical)
        {
            var entryBar = FindBarByTime(entryTrade.Time, setup.RejectionBar);
            var studyTrigger = GetStudyTriggerLabel(setup);
            var target2 = GetStudyTarget2(setup);
            var useTarget2 = true;
            var finalTarget = target2;
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
                ManagementMode = "VALUE_REENTRY_TARGET2",
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

            var rewardRiskToTarget2 = riskPoints > 0 ? rewardToTarget2 / riskPoints : 0;
            _log($"[MR_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, StudyTarget2={target2:F2}, StudyTrigger={studyTrigger}, Risk={riskPoints:F2}, Reward={rewardPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, RewardRiskToTarget2={rewardRiskToTarget2:F2}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}", isHistorical);
            StudyLog($"[DAY_STUDY_ACTUAL_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, StudyTarget2={target2:F2}, StudyTrigger={studyTrigger}, Risk={riskPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, RewardRiskToTarget2={rewardRiskToTarget2:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}", entryTrade.Time);

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


        private void ResetStudyLog()
        {
            _studyLoggedBars.Clear();
            _studyLogInitialized = false;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_studyLogPath)!);
                File.WriteAllText(_studyLogPath, $"[HISTORICAL_STUDY_START] MinAggressionVolume={MinAggressionVolume:F0}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, CreatedItaly={MarketTimeZones.ToItaly(DateTime.UtcNow):yyyy-MM-dd HH:mm:ss}{Environment.NewLine}");
                _studyLogInitialized = true;
            }
            catch
            {
            }
        }

        private void StudyLog(string message, DateTime eventUtc)
        {
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

            var (bid, ask, delta, topLevels) = GetCandleVolumeDiagnostics(candle);
            var relation = GetPriceLocation(candle.Close, _balanceTracker.LastPreviewPoc, _balanceTracker.LastPreviewVah, _balanceTracker.LastPreviewVal);
            StudyLog($"[DAY_STUDY_BAR] Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, Open={candle.Open:F2}, High={candle.High:F2}, Low={candle.Low:F2}, Close={candle.Close:F2}, Volume={candle.Volume:F0}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}, PreviewPOC={_balanceTracker.LastPreviewPoc:F2}, PreviewVAH={_balanceTracker.LastPreviewVah:F2}, PreviewVAL={_balanceTracker.LastPreviewVal:F2}, CloseLocation={relation}, DistToPOC={candle.Close - _balanceTracker.LastPreviewPoc:F2}, DistToVAH={candle.Close - _balanceTracker.LastPreviewVah:F2}, DistToVAL={candle.Close - _balanceTracker.LastPreviewVal:F2}, TopLevels={topLevels}", GetCandleEventTime(candle));
        }

        private void LogStudyCumulativeTrades(List<CumulativeTrade> trades)
        {
            foreach (var trade in trades.Where(t => t.Volume >= MinAggressionVolume && IsInLondonSession(t.Time)))
            {
                var nearestSetup = FindNearestStudySetup(trade);
                StudyLog($"[DAY_STUDY_BIG_TRADE] {FormatTime(trade.Time)}, Direction={trade.Direction}, Volume={trade.Volume:F0}, FirstPrice={trade.FirstPrice:F2}, LastPrice={trade.Lastprice:F2}, TickCount={trade.Ticks.Count}, Location={GetTradeLocation(trade.Lastprice)}, NearestSetupId={nearestSetup.SetupId}, NearestSetupDirection={nearestSetup.Direction}, SecondsAfterSetup={nearestSetup.SecondsAfter:F1}, SameDirectionAsSetup={nearestSetup.SameDirection}, InEntryZone={nearestSetup.InEntryZone}, ContinuationZone={nearestSetup.ContinuationZone}", trade.Time);
            }
        }

        private void RunDayStudy()
        {
            if (_dayStudyCompleted || _lastHistoricalTrades.Count == 0)
                return;

            var studySetups = _activeSetups.ToList();

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

                LogFabioStyleScaleInStudy(setup, trigger, candidates);
            }

            _dayStudyCompleted = true;
        }

        private void LogFabioStyleScaleInStudy(BalanceSetup setup, string trigger, List<StudyCandidate> candidates)
        {
            var valueCandidates = candidates
                .Where(c => c.CandidateType == "VALUE_REENTRY_BIG_TRADE")
                .Where(c => c.Risk > 0 && c.RewardT2 / c.Risk >= MinRewardRiskToTarget2)
                .OrderBy(c => c.EntryTimeUtc)
                .ToList();

            var baseCandidate = valueCandidates.FirstOrDefault();
            if (baseCandidate == null)
                return;

            var baseOutcome = EvaluateCandidateOutcome(baseCandidate.Direction, baseCandidate.EntryPrice, baseCandidate.Stop, baseCandidate.TargetPoc, baseCandidate.Target2, baseCandidate.EntryTimeUtc, setup.RejectionBar);
            var riskFreeTime = FindTargetPocHitTime(baseCandidate.Direction, baseCandidate.EntryPrice, baseCandidate.Stop, baseCandidate.TargetPoc, baseCandidate.EntryTimeUtc, setup.RejectionBar);
            var baseReachedRiskFree = riskFreeTime.HasValue;
            var riskFreeTimeValue = riskFreeTime.GetValueOrDefault();
            var addOns = baseReachedRiskFree
                ? valueCandidates.Where(c => c.EntryTimeUtc > riskFreeTimeValue).ToList()
                : new List<StudyCandidate>();

            StudyLog($"[DAY_STUDY_SCALE_IN_SUMMARY] SetupId={setup.SetupId}, Trigger={trigger}, Direction={setup.Direction}, BaseEntryTime={FormatTime(baseCandidate.EntryTimeUtc)}, BaseEntryPrice={baseCandidate.EntryPrice:F2}, BaseVolume={baseCandidate.Volume:F0}, BaseRisk={baseCandidate.Risk:F2}, BaseRR_T2={(baseCandidate.Risk > 0 ? baseCandidate.RewardT2 / baseCandidate.Risk : 0):F2}, BaseOutcomePOC={baseOutcome.OutcomePoc}, BaseOutcomeT2={baseOutcome.OutcomeT2}, BasePnLT2={baseOutcome.PnlT2:F2}, BaseReachedRiskFree={baseReachedRiskFree}, RiskFreeTime={(baseReachedRiskFree ? FormatTime(riskFreeTimeValue) : "NA")}, AddOnCandidates={addOns.Count}, FabioRule=ADD_ONLY_AFTER_POC_RISK_FREE", setup.RejectionTimeUtc);

            var scaleInIndex = 0;
            foreach (var addOn in addOns)
            {
                scaleInIndex++;
                var outcome = EvaluateCandidateOutcome(addOn.Direction, addOn.EntryPrice, addOn.Stop, addOn.TargetPoc, addOn.Target2, addOn.EntryTimeUtc, setup.RejectionBar);
                StudyLog($"[DAY_STUDY_SCALE_IN_CANDIDATE] SetupId={setup.SetupId}, ScaleInIndex={scaleInIndex}, Trigger={trigger}, Direction={addOn.Direction}, BaseEntryTime={FormatTime(baseCandidate.EntryTimeUtc)}, RiskFreeTime={FormatTime(riskFreeTimeValue)}, EntryTime={FormatTime(addOn.EntryTimeUtc)}, EntryPrice={addOn.EntryPrice:F2}, Volume={addOn.Volume:F0}, Stop={addOn.Stop:F2}, TargetPOC={addOn.TargetPoc:F2}, Target2={addOn.Target2:F2}, Risk={addOn.Risk:F2}, RewardT2={addOn.RewardT2:F2}, RR_T2={(addOn.Risk > 0 ? addOn.RewardT2 / addOn.Risk : 0):F2}, OutcomeT2={outcome.OutcomeT2}, PnLT2={outcome.PnlT2:F2}, RMultiple={(addOn.Risk > 0 ? outcome.PnlT2 / addOn.Risk : 0):F2}R, MFE={outcome.Mfe:F2}, MAE={outcome.Mae:F2}", addOn.EntryTimeUtc);
            }

            LogScalePlanStudy(setup, trigger, baseCandidate, riskFreeTime, addOns);
        }

        private void LogScalePlanStudy(BalanceSetup setup, string trigger, StudyCandidate baseCandidate, DateTime? riskFreeTime, List<StudyCandidate> addOns)
        {
            var plans = new[]
            {
                new ScalePlan("NO_SCALE", 0, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_1", 1, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_2", 2, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_3", 3, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_1_VOL20", 1, 20m, null, 0m),
                new ScalePlan("SCALE_MAX_2_VOL20", 2, 20m, null, 0m),
                new ScalePlan("SCALE_MAX_1_WITHIN_3MIN", 1, MinAggressionVolume, 180, 0m),
                new ScalePlan("SCALE_MAX_1_WITHIN_5MIN", 1, MinAggressionVolume, 300, 0m),
                new ScalePlan("SCALE_MAX_2_WITHIN_5MIN", 2, MinAggressionVolume, 300, 0m),
                new ScalePlan("SCALE_MAX_1_EXPAND25", 1, MinAggressionVolume, null, 0.25m),
                new ScalePlan("SCALE_MAX_1_EXPAND50", 1, MinAggressionVolume, null, 0.50m),
                new ScalePlan("SCALE_MAX_2_EXPAND25", 2, MinAggressionVolume, null, 0.25m),
                new ScalePlan("SCALE_MAX_1_WITHIN_5MIN_EXPAND25", 1, MinAggressionVolume, 300, 0.25m)
            };

            var baseProtectedOutcome = EvaluateProtectedTarget2Outcome(baseCandidate.Direction, baseCandidate.EntryPrice, baseCandidate.Stop, baseCandidate.TargetPoc, baseCandidate.Target2, baseCandidate.EntryTimeUtc, setup.RejectionBar);

            foreach (var plan in plans)
            {
                var selectedAddOns = SelectScalePlanAddOns(addOns, plan, riskFreeTime).ToList();
                var totalPnl = baseProtectedOutcome.Pnl;
                var totalR = baseProtectedOutcome.RMultiple;
                var winners = baseProtectedOutcome.Pnl > 0 ? 1 : 0;
                var losers = baseProtectedOutcome.Pnl < 0 ? 1 : 0;
                var worstLegR = baseProtectedOutcome.RMultiple;

                foreach (var addOn in selectedAddOns)
                {
                    var addOnOutcome = EvaluateProtectedTarget2Outcome(addOn.Direction, addOn.EntryPrice, addOn.Stop, addOn.TargetPoc, addOn.Target2, addOn.EntryTimeUtc, setup.RejectionBar);
                    totalPnl += addOnOutcome.Pnl;
                    totalR += addOnOutcome.RMultiple;
                    if (addOnOutcome.Pnl > 0)
                        winners++;
                    if (addOnOutcome.Pnl < 0)
                        losers++;
                    worstLegR = Math.Min(worstLegR, addOnOutcome.RMultiple);
                }

                StudyLog($"[DAY_STUDY_SCALE_PLAN] SetupId={setup.SetupId}, Plan={plan.Name}, Trigger={trigger}, Direction={setup.Direction}, BaseEntryTime={FormatTime(baseCandidate.EntryTimeUtc)}, BaseEntryPrice={baseCandidate.EntryPrice:F2}, BaseExitReason={baseProtectedOutcome.ExitReason}, BasePnL={baseProtectedOutcome.Pnl:F2}, BaseR={baseProtectedOutcome.RMultiple:F2}R, BaseTarget1Hit={baseProtectedOutcome.Target1Hit}, RiskFreeTime={(riskFreeTime.HasValue ? FormatTime(riskFreeTime.Value) : "NA")}, AddOnCount={selectedAddOns.Count}, AddOnMinVolume={plan.MinVolume:F0}, AddOnMaxSecondsAfterRiskFree={(plan.MaxSecondsAfterRiskFree.HasValue ? plan.MaxSecondsAfterRiskFree.Value.ToString() : "ANY")}, AddOnMinExpansionAfterRiskFreePct={plan.MinExpansionAfterRiskFreePct:F2}, Winners={winners}, Losers={losers}, TotalPnL={totalPnl:F2}, TotalR={totalR:F2}R, WorstLegR={worstLegR:F2}R, MaxOpenContracts={1 + selectedAddOns.Count}", setup.RejectionTimeUtc);
            }
        }

        private IEnumerable<StudyCandidate> SelectScalePlanAddOns(List<StudyCandidate> addOns, ScalePlan plan, DateTime? riskFreeTime)
        {
            if (plan.MaxAddOns <= 0 || !riskFreeTime.HasValue)
                return Enumerable.Empty<StudyCandidate>();

            var riskFreeTimeValue = riskFreeTime.Value;
            return addOns
                .Where(c => c.Volume >= plan.MinVolume)
                .Where(c => !plan.MaxSecondsAfterRiskFree.HasValue || c.EntryTimeUtc <= riskFreeTimeValue.AddSeconds(plan.MaxSecondsAfterRiskFree.Value))
                .Where(c => HasExpandedAfterRiskFree(c, riskFreeTimeValue, plan.MinExpansionAfterRiskFreePct))
                .OrderBy(c => c.EntryTimeUtc)
                .Take(plan.MaxAddOns);
        }

        private bool HasExpandedAfterRiskFree(StudyCandidate candidate, DateTime riskFreeTimeUtc, decimal minExpansionPct)
        {
            if (minExpansionPct <= 0)
                return true;

            var targetDistance = Math.Abs(candidate.Target2 - candidate.TargetPoc);
            if (targetDistance <= 0)
                return false;

            var requiredExpansion = targetDistance * minExpansionPct;
            var startBar = FindBarByTime(riskFreeTimeUtc, 0);
            var endBar = FindBarByTime(candidate.EntryTimeUtc, startBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(candidate.EntryTimeUtc));
            var bestExpansion = 0m;

            for (var bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                bestExpansion = candidate.Direction == "Long"
                    ? Math.Max(bestExpansion, candle.High - candidate.TargetPoc)
                    : Math.Max(bestExpansion, candidate.TargetPoc - candle.Low);
            }

            return bestExpansion >= requiredExpansion;
        }

        private StudyCandidate BuildStudyCandidate(BalanceSetup setup, CumulativeTrade trade, string trigger)
        {
            if (!IsTarget2ManagementTrigger(trigger))
                return new StudyCandidate(setup.Direction, "NOT_CANDIDATE", trade.Time, trade.Lastprice, trade.Volume, setup.StopPrice, setup.POC, GetStudyTarget2(setup), 0, 0, 0);

            var inEntryZone = IsInEntryZone(setup, trade);
            var continuation = IsBeyondPocContinuationEntry(setup, trade);
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
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var mfe = 0m;
            var mae = 0m;
            var outcomePoc = "OPEN";
            var outcomeT2 = "OPEN";
            var pnlPoc = 0m;
            var pnlT2 = 0m;

            for (var bar = entryBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(candle.Time)) != entryDay)
                    break;

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

        private ProtectedStudyOutcome EvaluateProtectedTarget2Outcome(string direction, decimal entry, decimal stop, decimal targetPoc, decimal target2, DateTime entryTimeUtc, int fallbackBar)
        {
            var entryBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var activeStop = stop;
            var target1Hit = false;
            var target1HitBar = -1;
            var exitReason = "OPEN";
            var exitPrice = entry;

            for (var bar = entryBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                if (direction == "Long")
                {
                    if (!target1Hit && targetPoc > entry && candle.High >= targetPoc)
                    {
                        target1Hit = true;
                        target1HitBar = bar;
                        activeStop = Math.Max(entry, targetPoc - 2m * _tickSize);
                    }

                    if (target2 > entry && candle.High >= target2)
                    {
                        exitReason = "TARGET2_HIT";
                        exitPrice = target2;
                        break;
                    }

                    if (candle.Low <= activeStop && (!target1Hit || bar > target1HitBar))
                    {
                        exitReason = target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT";
                        exitPrice = activeStop;
                        break;
                    }
                }
                else
                {
                    if (!target1Hit && targetPoc < entry && candle.Low <= targetPoc)
                    {
                        target1Hit = true;
                        target1HitBar = bar;
                        activeStop = Math.Min(entry, targetPoc + 2m * _tickSize);
                    }

                    if (target2 < entry && candle.Low <= target2)
                    {
                        exitReason = "TARGET2_HIT";
                        exitPrice = target2;
                        break;
                    }

                    if (candle.High >= activeStop && (!target1Hit || bar > target1HitBar))
                    {
                        exitReason = target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT";
                        exitPrice = activeStop;
                        break;
                    }
                }
            }

            if (exitReason == "OPEN")
                exitReason = "SESSION_END";

            var pnl = direction == "Long" ? exitPrice - entry : entry - exitPrice;
            var risk = Math.Abs(entry - stop);
            var rMultiple = risk > 0 ? pnl / risk : 0;
            return new ProtectedStudyOutcome(exitReason, pnl, rMultiple, target1Hit);
        }

        private DateTime? FindTargetPocHitTime(string direction, decimal entry, decimal stop, decimal targetPoc, DateTime entryTimeUtc, int fallbackBar)
        {
            var entryBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));

            for (var bar = entryBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                if (direction == "Long")
                {
                    if (candle.Low <= stop)
                        return null;
                    if (targetPoc > entry && candle.High >= targetPoc)
                        return eventTime;
                }
                else
                {
                    if (candle.High >= stop)
                        return null;
                    if (targetPoc < entry && candle.Low <= targetPoc)
                        return eventTime;
                }
            }

            return null;
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

        private (string SetupId, string Direction, double SecondsAfter, bool SameDirection, bool InEntryZone, bool ContinuationZone) FindNearestStudySetup(CumulativeTrade trade)
        {
            var setup = _activeSetups
                .Where(s => trade.Time >= s.RejectionTimeUtc && trade.Time <= s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .OrderBy(s => Math.Abs((trade.Time - s.RejectionTimeUtc).TotalSeconds))
                .FirstOrDefault();

            if (setup == null)
                return ("NA", "NA", 0, false, false, false);

            var sameDirection = setup.Direction == "Long" ? trade.Direction == TradeDirection.Buy : trade.Direction == TradeDirection.Sell;
            return (setup.SetupId, setup.Direction, (trade.Time - setup.RejectionTimeUtc).TotalSeconds, sameDirection, IsInEntryZone(setup, trade), IsBeyondPocContinuationEntry(setup, trade));
        }

        private sealed record StudyCandidate(string Direction, string CandidateType, DateTime EntryTimeUtc, decimal EntryPrice, decimal Volume, decimal Stop, decimal TargetPoc, decimal Target2, decimal Risk, decimal RewardPoc, decimal RewardT2);
        private sealed record StudyOutcome(string OutcomePoc, decimal PnlPoc, string OutcomeT2, decimal PnlT2, decimal Mfe, decimal Mae);
        private sealed record ProtectedStudyOutcome(string ExitReason, decimal Pnl, decimal RMultiple, bool Target1Hit);
        private sealed record ScalePlan(string Name, int MaxAddOns, decimal MinVolume, int? MaxSecondsAfterRiskFree, decimal MinExpansionAfterRiskFreePct);

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
