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
        private const int OperationalEntryTimeoutSeconds = 1200;
        private const int RejectionThresholdTicks = 10;
        private const int StopOffsetTicks = 2;
        private const int LondonSessionEndHour = 16;
        private const int LondonSessionEndMinute = 0;
        private const decimal MinRewardRiskToTarget2 = 1.0m;
        private const decimal DynamicStopMaxValueAreaRiskPct = 0.50m;
        private const decimal PocPartialExitPct = 0.70m;
        private const decimal RunnerExitPct = 1m - PocPartialExitPct;
        private const decimal ScaleInMinExpansionAfterRiskFreePct = 0.25m;
        private const decimal ScaleInMinRewardToTarget1Points = 4m;
        private const int MaxScaleInsPerSetup = 2;
        private const decimal DelayedReclaimNarrativeMinBubbleMultiplier = 5m;
        private const int DelayedReclaimMinAcceptedBars = 2;
        private const int DelayedReclaimImmediateMaxSeconds = 120;
        private const decimal DelayedReclaimDominantBubbleMultiplier = 2m;
        private const decimal DelayedReclaimEarlyPressureVolumeRatio = 1.50m;
        private const decimal DelayedReclaimMaxOperationalRiskPoints = 120m;
        private const decimal DuplicateBasePositionPocTolerancePoints = 4m;
        private const decimal DuplicateBasePositionValueEdgeTolerancePoints = 8m;
        private const bool EnableHistoricalIntrabarFromCumulativeTrades = true;
        private const int HistoricalStudyProgressBarStep = 100;
        private const int HistoricalStudyProgressTradeStep = 50000;
        private static readonly bool EnableFollowThroughStudyLogs = false;
        private const int SecondLegImmediateMaxSeconds = 300;
        private const decimal SecondLegMinDecisionWindowVolumeRankPct = 0.90m;
        private const decimal SecondLegMinReentryVsDecisionMedian = 2.00m;
        private const decimal SecondLegStrongReentryVolume = 30m;
        private const int SecondLegInitialMaeBars = 3;
        private const decimal SecondLegMaxInitialMaeRiskMultiple = 0.75m;
        private static readonly DateOnly[] HistoricalStudyDebugDays = Array.Empty<DateOnly>();
        private const string HistoricalStudyDebugMarkerFile = "FabioOrderFlow-enable-historical-study-debug.flag";

        private int _currentBar;
        private readonly List<BalanceSetup> _activeSetups = new();
        private readonly List<ActivePosition> _activePositions = new();
        private readonly List<DelayedReclaimCandidate> _delayedReclaimCandidates = new();
        private readonly Dictionary<string, FollowThroughSweepContext> _followThroughSweepContexts = new();
        private readonly List<FollowThroughSecondLegContext> _pendingSecondLegAuctionContexts = new();
        private readonly List<CumulativeTrade> _processedAggressionTrades = new();

        private readonly List<TradeRecord> _completedTrades = new();
        private readonly HashSet<string> _setupKeys = new();
        private readonly Dictionary<string, decimal> _liveTradeMaxVolumeByKey = new();
        private readonly List<CumulativeTrade> _lastHistoricalTrades = new();
        private readonly HashSet<int> _studyLoggedBars = new();
        private readonly Dictionary<int, HistoricalBarSnapshot> _historicalBarSnapshots = new();
        private readonly string _historicalLogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs", "FabioOrderFlow-historical.log");
        private readonly string _dailyHistoricalLogDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs", "FabioOrderFlow-days");
        private readonly string _historicalStudyDebugMarkerPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs", HistoricalStudyDebugMarkerFile);
        private readonly bool _historicalStudyDebugEnabled;
        private readonly bool _dailyHistoricalDebugLogsEnabled;
        private readonly HashSet<DateOnly> _historicalStudyDebugDays;
        private bool _historicalLogInitialized;
        private bool _historicalStudyActive;
        private bool _processingHistoricalPositions;
        private bool _dayStudyCompleted;
        private long _historicalLogSequence;
        private DateTime _historicalFlowStartUtc;
        private readonly HashSet<DateOnly> _initializedDailyLogs = new();
        private readonly Dictionary<DateOnly, long> _dailyHistoricalLogSequences = new();

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
            public bool ScaleInConfirmed { get; set; }
            public bool Expired { get; set; }
            public bool StudyFollowThroughConfirmed { get; set; }
            public int StudyFollowThroughBar { get; set; } = -1;
            public DateTime StudyFollowThroughTimeUtc { get; set; }
            public bool StudyPocTriggerConfirmed { get; set; }
            public int StudyPocTriggerBar { get; set; } = -1;
            public DateTime StudyPocTriggerTimeUtc { get; set; }
            public int StudyTriggerBar { get; set; } = -1;
            public DateTime StudyTriggerTimeUtc { get; set; }
            public string SetupSource { get; set; } = "BarClose";
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
            public DateTime Target1HitTimeUtc { get; set; }
            public bool StopProtectedAfterTarget1 { get; set; }
            public decimal RealizedPocPnL { get; set; }
            public decimal RealizedPocExitPct { get; set; }
            public decimal RunnerExitPct { get; set; } = 1m;
            public bool IsScaleIn { get; set; }
            public int ScaleInIndex { get; set; }
            public DateTime ExitTimeUtc { get; set; }
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


        private sealed class HistoricalBarSnapshot
        {
            public int Bar { get; set; }
            public DateTime EventTimeUtc { get; set; }
            public decimal Open { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Close { get; set; }
            public decimal Volume { get; set; }
            public decimal Bid { get; set; }
            public decimal Ask { get; set; }
            public decimal Delta { get; set; }
            public decimal PreviewPOC { get; set; }
            public decimal PreviewVAH { get; set; }
            public decimal PreviewVAL { get; set; }
            public decimal PreviousSessionHigh { get; set; }
            public decimal PreviousSessionLow { get; set; }
            public string CloseLocation { get; set; } = string.Empty;
            public string TopLevels { get; set; } = string.Empty;
        }

        private sealed class DelayedReclaimCandidate
        {
            public BalanceSetup Setup { get; set; } = new();
            public int AcceptedBars { get; set; }
            public bool OperationallyReady { get; set; }
            public bool EntryConfirmed { get; set; }
            public decimal SameDirectionVolume { get; set; }
            public decimal OppositeDirectionVolume { get; set; }
            public decimal MaxSameDirectionVolume { get; set; }
            public decimal MaxOppositeDirectionVolume { get; set; }
            public DateTime LastUpdatedUtc { get; set; }
        }

        private readonly record struct DelayedReclaimNarrativeStats(
            int SameDirectionTrades,
            decimal SameDirectionVolume,
            decimal MaxSameDirectionVolume,
            int OppositeDirectionTrades,
            decimal OppositeDirectionVolume,
            decimal MaxOppositeDirectionVolume,
            string MaxBubbleSide,
            decimal MaxBubbleVolume,
            decimal NetVolume,
            decimal PressureShift);

        private record FollowThroughSweepContext(
            string Direction,
            int SweepBar,
            DateTime SweepTimeUtc,
            decimal SweepLow,
            decimal SweepHigh,
            decimal POC,
            decimal VAH,
            decimal VAL,
            decimal StopPrice);

        private record FollowThroughSecondLegContext(
            string SourceSetupId,
            string Direction,
            DateTime DecisionTimeUtc,
            decimal DecisionPrice,
            decimal OldPOC,
            decimal StopPrice,
            int DecisionBar,
            bool Consumed);

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
            _historicalStudyDebugDays = HistoricalStudyDebugDays.ToHashSet();
            _historicalStudyDebugEnabled = _historicalStudyDebugDays.Count > 0 || File.Exists(_historicalStudyDebugMarkerPath);
            _dailyHistoricalDebugLogsEnabled = _historicalStudyDebugEnabled;
            _log($"[HISTORICAL_STUDY_DEBUG] Enabled={_historicalStudyDebugEnabled}, DailyLogs={_dailyHistoricalDebugLogsEnabled}, DebugDays={FormatHistoricalStudyDebugDays()}, Marker={_historicalStudyDebugMarkerPath}", false);
        }

        public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
        {
            _currentBar = currentBar;
            CaptureHistoricalBarSnapshot(bar, candle);
            LogStudyBar(bar, candle);
            UpdateDelayedReclaimCandidates(bar, candle);
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
            _historicalFlowStartUtc = DateTime.UtcNow;
            var allTrades = cumulativeTrades.OrderBy(t => t.Time).ToList();
            _lastHistoricalTrades.Clear();
            _lastHistoricalTrades.AddRange(allTrades);
            _dayStudyCompleted = false;
            _delayedReclaimCandidates.Clear();
            _followThroughSweepContexts.Clear();
            _pendingSecondLegAuctionContexts.Clear();
            _processedAggressionTrades.Clear();
            _studyLoggedBars.Clear();
            _historicalStudyActive = false;
            ResetDailyHistoricalDebugLogs();
            if (_historicalStudyDebugEnabled)
            {
                ResetStudyLog();
                LogExistingHistoricalSetups();
            }

            var trades = allTrades
                .Where(t => t.Volume >= MinAggressionVolume)
                .ToList();

            _log($"[HISTORICAL_FLOW_TRADES_READY] AllTrades={allTrades.Count}, AggressionTrades={trades.Count}, BeginItaly={(allTrades.Count > 0 ? MarketTimeZones.ToItaly(allTrades.First().Time).ToString("yyyy-MM-dd HH:mm:ss") : "NA")}, EndItaly={(allTrades.Count > 0 ? MarketTimeZones.ToItaly(allTrades.Last().Time).ToString("yyyy-MM-dd HH:mm:ss") : "NA")}", false);
            _log($"[MR_HISTORICAL_TRADES] Count={trades.Count}, MinAggressionVolume={MinAggressionVolume:F0}, ActiveSetups={_activeSetups.Count(s => !s.AggressionConfirmed && !s.Expired)}", false);

            if (EnableHistoricalIntrabarFromCumulativeTrades)
                AddHistoricalIntrabarSetups(allTrades);

            if (_historicalStudyDebugEnabled)
            {
                LogStudyCumulativeTrades(allTrades);
                LogDailySetupCandidateSummaries(trades);
            }
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
            var processStartUtc = DateTime.UtcNow;
            var previousProcessingState = _processingHistoricalPositions;
            _processingHistoricalPositions = true;
            _log($"[HISTORICAL_FLOW_PROCESS_START] StartBar={startBar}, EndBar={endBar}, StoredTrades={_lastHistoricalTrades.Count}, ExistingSnapshots={_historicalBarSnapshots.Count}, ExistingDelayedCandidates={_delayedReclaimCandidates.Count}", false);
            _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Start, StartBar={startBar}, EndBar={endBar}, StoredTrades={_lastHistoricalTrades.Count}, ExistingSnapshots={_historicalBarSnapshots.Count}, ExistingDelayedCandidates={_delayedReclaimCandidates.Count}, StudyDebugEnabled={_historicalStudyDebugEnabled}, DebugDays={FormatHistoricalStudyDebugDays()}", false);
            try
            {
                var totalBars = Math.Max(1, endBar - startBar + 1);
                for (var bar = startBar; bar <= endBar; bar++)
                {
                    var candle = _getCandle(bar);
                    CaptureHistoricalBarSnapshot(bar, candle);
                    UpdateFollowThroughReclaimSetups(bar, candle);
                    UpdateDelayedReclaimCandidates(bar, candle);
                    if (_historicalStudyDebugEnabled)
                        LogStudyBar(bar, candle);
                    UpdateActivePositions(bar, candle);

                    if ((bar - startBar + 1) % HistoricalStudyProgressBarStep == 0 || bar == endBar)
                    {
                        var processedBars = bar - startBar + 1;
                        _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Bars, Processed={processedBars}, Total={totalBars}, Percent={(decimal)processedBars * 100m / totalBars:F1}, CurrentBar={bar}, EventItaly={MarketTimeZones.ToItaly(GetCandleEventTime(candle)):yyyy-MM-dd HH:mm:ss}", false);
                    }
                }

                ProcessStoredHistoricalTrades();

                LogHistoricalFlowFinish(startBar, endBar, processStartUtc);

                if (_historicalStudyDebugEnabled && !_dailyHistoricalDebugLogsEnabled)
                    RunDayStudy();
            }
            finally
            {
                _processingHistoricalPositions = previousProcessingState;
            }
        }

        private void ProcessStoredHistoricalTrades()
        {
            if (_lastHistoricalTrades.Count == 0)
                return;

            var totalTrades = _lastHistoricalTrades.Count;
            for (var i = 0; i < _lastHistoricalTrades.Count; i++)
            {
                var trade = _lastHistoricalTrades[i];
                if (trade.Volume >= MinAggressionVolume)
                    ProcessAggressionTrade(trade, "FootprintCumulativeTradeHistorical", true);

                UpdateHistoricalPositionsWithTrade(trade);

                if (((i + 1) % HistoricalStudyProgressTradeStep) == 0 || i == _lastHistoricalTrades.Count - 1)
                {
                    var processedTrades = i + 1;
                    _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Trades, Processed={processedTrades}, Total={totalTrades}, Percent={(decimal)processedTrades * 100m / totalTrades:F1}, EventItaly={MarketTimeZones.ToItaly(trade.Time):yyyy-MM-dd HH:mm:ss}", false);
                }
            }

            UpdateOpenHistoricalPositionsWithCompletedBars();
            CloseOpenHistoricalPositionsAtSessionEnd();
            if (_historicalStudyDebugEnabled)
                LogMissedOpportunities(_lastHistoricalTrades);
        }

        private void LogHistoricalFlowFinish(int startBar, int endBar, DateTime processStartUtc)
        {
            var completedUtc = DateTime.UtcNow;
            var durationMs = (completedUtc - processStartUtc).TotalMilliseconds;
            var flowDurationMs = _historicalFlowStartUtc == default ? 0 : (completedUtc - _historicalFlowStartUtc).TotalMilliseconds;
            var historicalEntries = _completedTrades.Count(t => t.EntryModel.Contains("Historical", StringComparison.OrdinalIgnoreCase));
            var delayedEntries = _completedTrades.Count(t => t.EntryModel.Contains("DelayedReclaim", StringComparison.OrdinalIgnoreCase));
            var openPositions = _activePositions.Count(p => !p.Closed);
            var closedPositions = _activePositions.Count(p => p.Closed);
            var positionRecords = _activePositions.Count;
            _log($"[HISTORICAL_FLOW_FINISH] StartBar={startBar}, EndBar={endBar}, Snapshots={_historicalBarSnapshots.Count}, StoredTrades={_lastHistoricalTrades.Count}, CompletedHistoricalEntries={historicalEntries}, CompletedDelayedReclaimEntries={delayedEntries}, OpenPositions={openPositions}, ClosedPositions={closedPositions}, PositionRecords={positionRecords}, ProcessDurationMs={durationMs:F0}, TotalFlowDurationMs={flowDurationMs:F0}", false);
            _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Finish, StartBar={startBar}, EndBar={endBar}, Snapshots={_historicalBarSnapshots.Count}, StoredTrades={_lastHistoricalTrades.Count}, CompletedHistoricalEntries={historicalEntries}, CompletedDelayedReclaimEntries={delayedEntries}, OpenPositions={openPositions}, ClosedPositions={closedPositions}, PositionRecords={positionRecords}, ProcessDurationMs={durationMs:F0}, TotalFlowDurationMs={flowDurationMs:F0}", false);

            if (!_dailyHistoricalDebugLogsEnabled)
                return;

            foreach (var day in _initializedDailyLogs.OrderBy(d => d))
            {
                var path = Path.Combine(_dailyHistoricalLogDirectory, $"FabioOrderFlow-day-{day:yyyy-MM-dd}.log");
                if (!File.Exists(path))
                    continue;

                try
                {
                    var text = File.ReadAllText(path);
                    var bars = CountOccurrences(text, "[DAY_STUDY_BAR]");
                    var bigTrades = CountOccurrences(text, "[DAY_STUDY_BIG_TRADE]");
                    var entries = CountOccurrences(text, "[MR_ENTRY]");
                    var delayedEntriesForDay = CountOccurrences(text, "EntryModel=FootprintCumulativeTradeHistoricalDelayedReclaim")
                        + CountOccurrences(text, "EntryModel=FootprintCumulativeTradeLiveDelayedReclaim");
                    var exits = CountOccurrences(text, "[MR_EXIT]");
                    var accepted = CountOccurrences(text, "[DAY_STUDY_DELAYED_RECLAIM_ACCEPTED]");
                    DailyHistoricalLog($"[DAY_DEBUG_FINISH] Day={day:yyyy-MM-dd}, Bars={bars}, BigTrades={bigTrades}, Entries={entries}, DelayedReclaimEntries={delayedEntriesForDay}, Exits={exits}, DelayedReclaimAccepted={accepted}, CompletedItaly={MarketTimeZones.ToItaly(completedUtc):yyyy-MM-dd HH:mm:ss}", new DateTime(day.Year, day.Month, day.Day, 23, 59, 59, DateTimeKind.Utc));
                }
                catch
                {
                }
            }
        }

        private static int CountOccurrences(string text, string token)
        {
            var count = 0;
            var index = 0;
            while ((index = text.IndexOf(token, index, StringComparison.Ordinal)) >= 0)
            {
                count++;
                index += token.Length;
            }

            return count;
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

        private void AddSetup(BalanceSetup setup, string tag, bool prepend = false)
        {
            var key = setup.SetupSource == "HistoricalIntrabar"
                ? $"{setup.SetupSource}:{setup.Direction}:{setup.RejectionBar}:{setup.BreakoutPrice:F2}:{setup.POC:F2}:{setup.RejectionTimeUtc.Ticks}"
                : $"{setup.SetupSource}:{setup.Direction}:{setup.RejectionBar}:{setup.BreakoutPrice:F2}:{setup.POC:F2}";
            if (!_setupKeys.Add(key))
                return;

            if (prepend)
                _activeSetups.Insert(0, setup);
            else
                _activeSetups.Add(setup);

            _log($"[{tag}] SetupId={setup.SetupId}, Source={setup.SetupSource}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", IsHistoricalBar(setup.RejectionBar));
            if (setup.SetupSource == "HistoricalIntrabar" || IsHistoricalBar(setup.RejectionBar))
                StudyLog($"[DAY_STUDY_SETUP] SetupId={setup.SetupId}, Source={setup.SetupSource}, Tag={tag}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, RejectionHigh={setup.RejectionHigh:F2}, RejectionLow={setup.RejectionLow:F2}, RejectionDelta={setup.RejectionDelta:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", setup.RejectionTimeUtc);
        }

        private void LogExistingHistoricalSetups()
        {
            foreach (var setup in _activeSetups
                .Where(s => s.SetupSource != "HistoricalIntrabar" && IsHistoricalBar(s.RejectionBar))
                .Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc))
                .OrderBy(s => s.RejectionTimeUtc))
            {
                StudyLog($"[DAY_STUDY_SETUP] SetupId={setup.SetupId}, Source={setup.SetupSource}, Tag=MR_SETUP_RESTORED_AFTER_DAILY_RESET, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, RejectionHigh={setup.RejectionHigh:F2}, RejectionLow={setup.RejectionLow:F2}, RejectionDelta={setup.RejectionDelta:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}, AggressionConfirmed={setup.AggressionConfirmed}, Expired={setup.Expired}", setup.RejectionTimeUtc);
            }
        }

        private void AddHistoricalIntrabarSetups(List<CumulativeTrade> allTrades)
        {
            var barCloseSetups = _activeSetups
                .Where(s => s.SetupSource == "BarClose")
                .OrderBy(s => s.RejectionTimeUtc)
                .ToList();

            foreach (var setup in barCloseSetups)
            {
                var candle = _getCandle(setup.RejectionBar);
                var barEnd = GetCandleEventTime(candle);
                var trades = allTrades
                    .Where(t => t.Time >= candle.Time && t.Time <= barEnd)
                    .OrderBy(t => t.Time)
                    .ToList();

                if (trades.Count == 0)
                    continue;

                var syntheticHigh = candle.Open;
                var syntheticLow = candle.Open;

                foreach (var trade in trades)
                {
                    syntheticHigh = Math.Max(syntheticHigh, Math.Max(trade.FirstPrice, trade.Lastprice));
                    syntheticLow = Math.Min(syntheticLow, Math.Min(trade.FirstPrice, trade.Lastprice));
                    var syntheticClose = trade.Lastprice;

                    if (!TryCreateHistoricalIntrabarSetup(setup, trade.Time, syntheticHigh, syntheticLow, syntheticClose, out var intrabarSetup) || intrabarSetup == null)
                        continue;

                    AddSetup(intrabarSetup, "MR_HISTORICAL_INTRABAR_SETUP", prepend: true);
                    break;
                }
            }
        }

        private bool TryCreateHistoricalIntrabarSetup(BalanceSetup source, DateTime eventTimeUtc, decimal syntheticHigh, decimal syntheticLow, decimal syntheticClose, out BalanceSetup? setup)
        {
            setup = null;

            if (eventTimeUtc >= source.RejectionTimeUtc)
                return false;

            if (source.Direction == "Short")
            {
                if (syntheticHigh <= source.VAH || syntheticClose >= source.VAH)
                    return false;

                var rejectionDistance = syntheticHigh - syntheticClose;
                if (rejectionDistance < RejectionThresholdTicks * _tickSize)
                    return false;

                setup = new BalanceSetup
                {
                    Direction = source.Direction,
                    POC = source.POC,
                    VAH = source.VAH,
                    VAL = source.VAL,
                    BreakoutBar = source.BreakoutBar,
                    BreakoutTimeUtc = eventTimeUtc,
                    BreakoutPrice = syntheticHigh,
                    RejectionBar = source.RejectionBar,
                    RejectionTimeUtc = eventTimeUtc,
                    RejectionClose = syntheticClose,
                    RejectionHigh = syntheticHigh,
                    RejectionLow = syntheticLow,
                    RejectionDelta = rejectionDistance,
                    StopPrice = syntheticHigh + StopOffsetTicks * _tickSize,
                    TargetPrice = source.POC,
                    StudyFollowThroughConfirmed = source.StudyFollowThroughConfirmed,
                    StudyFollowThroughBar = source.StudyFollowThroughBar,
                    StudyFollowThroughTimeUtc = source.StudyFollowThroughTimeUtc,
                    StudyPocTriggerConfirmed = source.StudyPocTriggerConfirmed,
                    StudyPocTriggerBar = source.StudyPocTriggerBar,
                    StudyPocTriggerTimeUtc = source.StudyPocTriggerTimeUtc,
                    StudyTriggerBar = source.StudyTriggerBar,
                    StudyTriggerTimeUtc = source.StudyTriggerTimeUtc,
                    SetupSource = "HistoricalIntrabar"
                };
                return true;
            }

            if (syntheticLow >= source.VAL || syntheticClose <= source.VAL)
                return false;

            var lowRejectionDistance = syntheticClose - syntheticLow;
            if (lowRejectionDistance < RejectionThresholdTicks * _tickSize)
                return false;

            setup = new BalanceSetup
            {
                Direction = source.Direction,
                POC = source.POC,
                VAH = source.VAH,
                VAL = source.VAL,
                BreakoutBar = source.BreakoutBar,
                BreakoutTimeUtc = eventTimeUtc,
                BreakoutPrice = syntheticLow,
                RejectionBar = source.RejectionBar,
                RejectionTimeUtc = eventTimeUtc,
                RejectionClose = syntheticClose,
                RejectionHigh = syntheticHigh,
                RejectionLow = syntheticLow,
                RejectionDelta = lowRejectionDistance,
                StopPrice = syntheticLow - StopOffsetTicks * _tickSize,
                TargetPrice = source.POC,
                StudyFollowThroughConfirmed = source.StudyFollowThroughConfirmed,
                StudyFollowThroughBar = source.StudyFollowThroughBar,
                StudyFollowThroughTimeUtc = source.StudyFollowThroughTimeUtc,
                StudyPocTriggerConfirmed = source.StudyPocTriggerConfirmed,
                StudyPocTriggerBar = source.StudyPocTriggerBar,
                StudyPocTriggerTimeUtc = source.StudyPocTriggerTimeUtc,
                StudyTriggerBar = source.StudyTriggerBar,
                StudyTriggerTimeUtc = source.StudyTriggerTimeUtc,
                SetupSource = "HistoricalIntrabar"
            };
            return true;
        }

        private void ProcessAggressionTrade(CumulativeTrade trade, string entryModel, bool isHistorical)
        {
            TrackProcessedAggressionTrade(trade);

            if (TryProcessDelayedReclaimEntry(trade, entryModel, isHistorical))
                return;

            if (TryProcessFollowThroughSecondLegAuctionEntry(trade, entryModel, isHistorical))
                return;

            foreach (var setup in _activeSetups.Where(s => s.AggressionConfirmed && !s.Expired).ToList())
            {
                if (!IsScaleInEntry(setup, trade))
                    continue;

                var scaleInIndex = _activePositions.Count(p => p.SetupId == setup.SetupId && p.IsScaleIn) + 1;
                setup.ScaleInConfirmed = scaleInIndex >= MaxScaleInsPerSetup;
                CreatePosition(setup, trade, entryModel, isHistorical, isScaleIn: true, scaleInIndex: scaleInIndex);
                break;
            }

            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired).ToList())
            {
                if (!IsTradeInSetupWindow(setup, trade))
                    continue;

                if (!IsAggressionEntry(setup, trade))
                    continue;

                setup.AggressionConfirmed = true;
                var resolvedEntryModel = setup.SetupSource == "HistoricalIntrabar" && entryModel == "FootprintCumulativeTradeHistorical"
                    ? "FootprintCumulativeTradeHistoricalIntrabar"
                    : entryModel;
                CreatePosition(setup, trade, resolvedEntryModel, isHistorical);
                ExpireEquivalentBaseSetups(setup);
                break;
            }
        }

        private void TrackProcessedAggressionTrade(CumulativeTrade trade)
        {
            if (trade.Volume < MinAggressionVolume || !IsLondonTradeAllowed(trade.Time))
                return;

            if (_processedAggressionTrades.Any(t => t.Time == trade.Time && t.Direction == trade.Direction && t.Lastprice == trade.Lastprice && t.Volume == trade.Volume))
                return;

            _processedAggressionTrades.Add(trade);
        }

        private bool TryProcessFollowThroughSecondLegAuctionEntry(CumulativeTrade trade, string entryModel, bool isHistorical)
        {
            foreach (var context in _pendingSecondLegAuctionContexts.Where(c => !c.Consumed).ToList())
            {
                if (!IsSecondLegAuctionTrade(context, trade))
                    continue;

                if (!HasDecisionPullbackHoldBefore(context, trade.Time))
                    continue;

                var decisionWindowTrades = GetProcessedAggressionTradesBetween(context.DecisionTimeUtc, trade.Time);
                var dayTrades = GetProcessedAggressionTradesForDayUntil(trade.Time);
                var decisionRank = GetVolumeRankPct(trade.Volume, decisionWindowTrades);
                var decisionMedianRatio = GetVolumeRatioToMedian(trade.Volume, decisionWindowTrades);
                var dayRank = GetVolumeRankPct(trade.Volume, dayTrades);
                if (decisionRank < SecondLegMinDecisionWindowVolumeRankPct || decisionMedianRatio < SecondLegMinReentryVsDecisionMedian)
                    continue;

                if (!TryCreateFollowThroughSecondLegSetup(context, trade, out var setup) || setup == null)
                    continue;

                var consumedContext = context with { Consumed = true };
                var index = _pendingSecondLegAuctionContexts.FindIndex(c => c.SourceSetupId == context.SourceSetupId && c.DecisionTimeUtc == context.DecisionTimeUtc);
                if (index >= 0)
                    _pendingSecondLegAuctionContexts[index] = consumedContext;

                AddSetup(setup, "MR_FOLLOW_THROUGH_SECOND_LEG_AUCTION");
                setup.AggressionConfirmed = true;
                CreatePosition(setup, trade, entryModel, isHistorical);
                _log($"[MR_SECOND_LEG_AUCTION_CONFIRMED] SetupId={setup.SetupId}, SourceSetupId={context.SourceSetupId}, Direction={context.Direction}, EntryTime={FormatTime(trade.Time)}, EntryPrice={trade.Lastprice:F2}, Volume={trade.Volume:F0}, DayVolumeRankPct={dayRank:F2}, DecisionWindowVolumeRankPct={decisionRank:F2}, ReentryVsDecisionMedian={decisionMedianRatio:F2}, DecisionPrice={context.DecisionPrice:F2}, OldPOC={context.OldPOC:F2}, Stop={setup.StopPrice:F2}", isHistorical);
                return true;
            }

            return false;
        }

        private bool IsSecondLegAuctionTrade(FollowThroughSecondLegContext context, CumulativeTrade trade)
        {
            if (trade.Time <= context.DecisionTimeUtc || trade.Time > context.DecisionTimeUtc.AddSeconds(SecondLegImmediateMaxSeconds))
                return false;
            if (!IsLondonTradeAllowed(trade.Time) || trade.Volume < MinAggressionVolume)
                return false;

            var expectedDirection = context.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction != expectedDirection)
                return false;

            return IsBeyondDecisionArea(context.Direction, trade.Lastprice, context.DecisionPrice);
        }

        private bool HasDecisionPullbackHoldBefore(FollowThroughSecondLegContext context, DateTime beforeUtc)
        {
            var day = DateOnly.FromDateTime(MarketTimeZones.ToItaly(context.DecisionTimeUtc));
            var startBar = FindBarByTime(context.DecisionTimeUtc, context.DecisionBar);
            for (var bar = startBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (eventTime <= context.DecisionTimeUtc)
                    continue;
                if (eventTime >= beforeUtc)
                    break;
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != day)
                    break;
                if (!IsInLondonSession(eventTime))
                    break;

                if (context.Direction == "Long")
                {
                    if (candle.Low <= context.DecisionPrice && candle.Close > context.DecisionPrice)
                        return true;
                }
                else if (candle.High >= context.DecisionPrice && candle.Close < context.DecisionPrice)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryCreateFollowThroughSecondLegSetup(FollowThroughSecondLegContext context, CumulativeTrade trade, out BalanceSetup? setup)
        {
            setup = null;
            var stop = GetSecondLegAuctionStop(context, trade.Lastprice);
            if (!IsStopBehindEntry(context.Direction, trade.Lastprice, stop))
                return false;

            setup = new BalanceSetup
            {
                Direction = context.Direction,
                POC = context.OldPOC,
                VAH = context.Direction == "Long" ? context.DecisionPrice : context.OldPOC,
                VAL = context.Direction == "Long" ? context.OldPOC : context.DecisionPrice,
                BreakoutBar = context.DecisionBar,
                BreakoutTimeUtc = context.DecisionTimeUtc,
                BreakoutPrice = context.DecisionPrice,
                RejectionBar = FindBarByTime(trade.Time, context.DecisionBar),
                RejectionTimeUtc = trade.Time,
                RejectionClose = trade.Lastprice,
                RejectionHigh = Math.Max(trade.FirstPrice, trade.Lastprice),
                RejectionLow = Math.Min(trade.FirstPrice, trade.Lastprice),
                RejectionDelta = Math.Abs(trade.Lastprice - context.DecisionPrice),
                StopPrice = stop,
                TargetPrice = context.OldPOC,
                StudyFollowThroughConfirmed = true,
                StudyFollowThroughBar = FindBarByTime(trade.Time, context.DecisionBar),
                StudyFollowThroughTimeUtc = trade.Time,
                StudyTriggerBar = FindBarByTime(trade.Time, context.DecisionBar),
                StudyTriggerTimeUtc = trade.Time,
                SetupSource = "FollowThroughSecondLegAuction"
            };
            return true;
        }

        private decimal GetSecondLegAuctionStop(FollowThroughSecondLegContext context, decimal entryPrice)
        {
            var maxRisk = Math.Abs(context.DecisionPrice - context.OldPOC) * 0.5m;
            if (maxRisk <= 0)
                return context.StopPrice;

            return context.Direction == "Long"
                ? Math.Max(context.StopPrice, entryPrice - maxRisk)
                : Math.Min(context.StopPrice, entryPrice + maxRisk);
        }

        private List<CumulativeTrade> GetProcessedAggressionTradesForDayUntil(DateTime toUtc)
        {
            var day = DateOnly.FromDateTime(MarketTimeZones.ToItaly(toUtc));
            return _processedAggressionTrades
                .Where(t => t.Time <= toUtc)
                .Where(t => DateOnly.FromDateTime(MarketTimeZones.ToItaly(t.Time)) == day)
                .ToList();
        }

        private List<CumulativeTrade> GetProcessedAggressionTradesBetween(DateTime fromUtc, DateTime toUtc)
        {
            return _processedAggressionTrades
                .Where(t => t.Time >= fromUtc && t.Time <= toUtc)
                .ToList();
        }

        private void ExpireEquivalentBaseSetups(BalanceSetup confirmedSetup)
        {
            foreach (var setup in _activeSetups.Where(s => s.SetupId != confirmedSetup.SetupId && !s.AggressionConfirmed && !s.Expired))
            {
                if (setup.Direction != confirmedSetup.Direction)
                    continue;

                if (setup.RejectionBar != confirmedSetup.RejectionBar)
                    continue;

                if (setup.POC != confirmedSetup.POC || setup.VAH != confirmedSetup.VAH || setup.VAL != confirmedSetup.VAL)
                    continue;

                setup.Expired = true;
                StudyLog($"[DAY_STUDY_EQUIVALENT_SETUP_EXPIRED] SetupId={setup.SetupId}, KeptSetupId={confirmedSetup.SetupId}, Source={setup.SetupSource}, KeptSource={confirmedSetup.SetupSource}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}", setup.RejectionTimeUtc);
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

        private bool IsAggressionEntry(BalanceSetup setup, CumulativeTrade trade, bool enforceFreshness = true)
        {
            if (trade.Volume < MinAggressionVolume)
                return false;

            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction != expectedDirection)
                return false;

            if (!IsOperationalEntryZone(setup, trade))
                return false;

            if (enforceFreshness && (trade.Time - setup.RejectionTimeUtc).TotalSeconds > OperationalEntryTimeoutSeconds)
                return false;

            return GetRewardRiskToTarget2(setup, trade.Lastprice) >= MinRewardRiskToTarget2;
        }

        private bool IsScaleInEntry(BalanceSetup setup, CumulativeTrade trade)
        {
            if (!IsTradeInSetupWindow(setup, trade))
                return false;

            if (!IsAggressionEntry(setup, trade, enforceFreshness: true))
                return false;

            var rewardToTarget1 = setup.Direction == "Long"
                ? setup.TargetPrice - trade.Lastprice
                : trade.Lastprice - setup.TargetPrice;
            if (rewardToTarget1 < ScaleInMinRewardToTarget1Points)
                return false;

            var existingScaleIns = _activePositions.Count(p => p.SetupId == setup.SetupId && p.IsScaleIn);
            if (existingScaleIns >= MaxScaleInsPerSetup)
                return false;

            var basePosition = _activePositions
                .Where(p => p.SetupId == setup.SetupId && !p.IsScaleIn)
                .OrderBy(p => p.EntryTimeUtc)
                .FirstOrDefault();
            if (basePosition == null || !basePosition.Target1Hit)
                return false;

            var riskFreeTime = GetCandleEventTime(_getCandle(basePosition.Target1HitBar));
            if (trade.Time <= riskFreeTime)
                return false;

            if (basePosition.Closed && trade.Time > basePosition.ExitTimeUtc)
                return false;

            return HasExpandedAfterRiskFree(setup, trade, riskFreeTime, ScaleInMinExpansionAfterRiskFreePct);
        }

        private static bool IsInEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            return setup.Direction == "Long"
                ? trade.Lastprice >= setup.VAL && trade.Lastprice < setup.POC
                : trade.Lastprice <= setup.VAH && trade.Lastprice > setup.POC;
        }

        private static bool IsOperationalEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            return IsFollowThroughContinuationSetup(setup)
                ? IsBeyondPocContinuationEntry(setup, trade)
                : IsInEntryZone(setup, trade);
        }

        private static bool IsFollowThroughContinuationSetup(BalanceSetup setup)
        {
            return setup.SetupSource == "FollowThroughReclaimContinuation";
        }

        private static bool IsFollowThroughSecondLegAuctionSetup(BalanceSetup setup)
        {
            return setup.SetupSource == "FollowThroughSecondLegAuction";
        }

        private static bool IsFollowThroughSecondLegAuctionPosition(ActivePosition position)
        {
            return position.ManagementMode == "FOLLOW_THROUGH_SECOND_LEG_AUCTION";
        }

        private static bool IsFollowThroughContinuationTrigger(string studyTrigger)
        {
            return studyTrigger == "FOLLOW_THROUGH_RECLAIM_CONTINUATION_LONG"
                || studyTrigger == "FOLLOW_THROUGH_RECLAIM_CONTINUATION_SHORT";
        }

        private static bool IsBeyondPocContinuationEntry(BalanceSetup setup, CumulativeTrade trade)
        {
            return setup.Direction == "Long"
                ? trade.Lastprice >= setup.POC && trade.Lastprice <= setup.VAH
                : trade.Lastprice <= setup.POC && trade.Lastprice >= setup.VAL;
        }

        private decimal GetRewardRiskToTarget2(BalanceSetup setup, decimal entryPrice)
        {
            var risk = GetOperationalRisk(setup, entryPrice);
            if (risk <= 0)
                return 0;

            var reward = Math.Abs(GetStudyTarget2(setup) - entryPrice);
            return reward / risk;
        }

        private decimal GetOperationalStopPrice(BalanceSetup setup, decimal entryPrice)
        {
            var valueWidth = Math.Abs(setup.VAH - setup.VAL);
            if (valueWidth <= 0)
                return setup.StopPrice;

            return GetCappedStop(setup.Direction, entryPrice, setup.StopPrice, valueWidth * DynamicStopMaxValueAreaRiskPct);
        }

        private decimal GetOperationalRisk(BalanceSetup setup, decimal entryPrice)
        {
            var stop = GetOperationalStopPrice(setup, entryPrice);
            if (!IsStopBehindEntry(setup.Direction, entryPrice, stop))
                return 0;

            return Math.Abs(entryPrice - stop);
        }

        private string GetOperationalStopPlan(BalanceSetup setup, decimal entryPrice)
        {
            var operationalStop = GetOperationalStopPrice(setup, entryPrice);
            return operationalStop == setup.StopPrice ? "ORIGINAL_REJECTION" : "CAP_VALUE_WIDTH_50";
        }

        private void LogMissedOpportunities(List<CumulativeTrade> trades)
        {
            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired).Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc)))
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

                var freshInsideValueTrades = insideValueTrades
                    .Where(t => (t.Time - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds)
                    .ToList();

                var validRrEntryTrades = freshInsideValueTrades
                    .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                    .ToList();

                var continuationTrades = IsTarget2ManagementTrigger(trigger)
                    ? sameDirectionTrades.Where(t => IsBeyondPocContinuationEntry(setup, t)).ToList()
                    : new List<CumulativeTrade>();

                var reason = operationalWindowTrades.Count == 0
                    ? "NO_BIG_TRADE_IN_WINDOW"
                    : sameDirectionTrades.Count == 0
                        ? "NO_BIG_TRADE_IN_DIRECTION"
                        : insideValueTrades.Count == 0
                            ? "NO_BIG_TRADE_IN_ENTRY_ZONE"
                            : freshInsideValueTrades.Count == 0
                                ? "ENTRY_TOO_STALE_AFTER_REJECTION"
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
                var missedMessage = $"[MR_MISSED_OPPORTUNITY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, Reason={reason}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, DynamicStopMaxValueAreaRiskPct={DynamicStopMaxValueAreaRiskPct:F2}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, WindowBigTrades={operationalWindowTrades.Count}, SameDirectionBigTrades={sameDirectionTrades.Count}, EntryZoneBigTrades={insideValueTrades.Count}, FreshEntryZoneBigTrades={freshInsideValueTrades.Count}, ValidRrEntryTrades={validRrEntryTrades.Count}, ContinuationBigTrades={continuationTrades.Count}, MaxVolume={maxVolume:F0}, MaxSameDirectionVolume={maxSameDirectionVolume:F0}, FirstBigTrade={firstTradeTime}, FirstSameDirectionBigTrade={firstSameDirectionTime}, BestSameDirectionPrice={bestSameDirectionPrice}";
                _log(missedMessage, true);
                DailyHistoricalLog(missedMessage, setup.RejectionTimeUtc);

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

        private bool CreatePosition(BalanceSetup setup, CumulativeTrade entryTrade, string entryModel, bool isHistorical, bool isScaleIn = false, int scaleInIndex = 0)
        {
            var entryBar = FindBarByTime(entryTrade.Time, setup.RejectionBar);
            var studyTrigger = GetStudyTriggerLabel(setup);
            if (!isScaleIn && TryGetOpenDuplicateBasePosition(setup, entryTrade.Time, out var duplicate) && duplicate != null)
            {
                setup.AggressionConfirmed = true;
                var skipMessage = $"[MR_ENTRY_SKIPPED] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Reason=DUPLICATE_BASE_POSITION, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={entryTrade.Lastprice:F2}, ExistingSetupId={duplicate.SetupId}, ExistingEntryModel={duplicate.EntryModel}, ExistingEntry={duplicate.EntryPrice:F2}, ExistingEntryTime={FormatTime(duplicate.EntryTimeUtc)}, ExistingTarget1POC={duplicate.Target1Price:F2}, ExistingTarget2={duplicate.Target2Price:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}";
                _log(skipMessage, isHistorical);
                if (entryModel.Contains("Historical", StringComparison.Ordinal))
                {
                    DailyHistoricalLog(skipMessage, entryTrade.Time);
                    StudyLog(skipMessage.Replace("[MR_ENTRY_SKIPPED]", "[DAY_STUDY_ENTRY_SKIPPED]"), entryTrade.Time);
                }
                return false;
            }
            var triggerAtEntry = GetStudyTriggerLabelAtTime(setup, entryTrade.Time);
            var target2 = GetStudyTarget2(setup);
            var isSecondLegAuction = IsFollowThroughSecondLegAuctionSetup(setup);
            var useTarget2 = !isSecondLegAuction;
            var finalTarget = isSecondLegAuction ? 0 : target2;
            var operationalStop = GetOperationalStopPrice(setup, entryTrade.Lastprice);
            var operationalStopPlan = GetOperationalStopPlan(setup, entryTrade.Lastprice);
            var position = new ActivePosition
            {
                SetupId = setup.SetupId,
                Direction = setup.Direction,
                EntryModel = entryModel,
                EntryPrice = entryTrade.Lastprice,
                EntryBar = entryBar,
                EntryTimeUtc = entryTrade.Time,
                InitialStopPrice = operationalStop,
                StopPrice = operationalStop,
                TargetPrice = finalTarget,
                Target1Price = setup.POC,
                Target2Price = target2,
                UseTarget2 = useTarget2,
                IsScaleIn = isScaleIn,
                ScaleInIndex = scaleInIndex,
                ManagementMode = isSecondLegAuction ? "FOLLOW_THROUGH_SECOND_LEG_AUCTION" : isScaleIn ? "VALUE_REENTRY_TARGET2_SCALE_IN_EXPAND25" : "VALUE_REENTRY_TARGET2",
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
            var entryMessage = $"[MR_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, OriginalStop={setup.StopPrice:F2}, OperationalStopPlan={operationalStopPlan}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, IsScaleIn={position.IsScaleIn}, ScaleInIndex={position.ScaleInIndex}, StudyTarget2={target2:F2}, StudyTrigger={studyTrigger}, TriggerAtEntry={triggerAtEntry}, Risk={riskPoints:F2}, Reward={rewardPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, RewardRiskToTarget2={rewardRiskToTarget2:F2}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, DynamicStopMaxValueAreaRiskPct={DynamicStopMaxValueAreaRiskPct:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}";
            _log(entryMessage, isHistorical);
            if (entryModel.Contains("Historical", StringComparison.Ordinal))
            {
                DailyHistoricalLog(entryMessage, entryTrade.Time);
                StudyLog($"[DAY_STUDY_ACTUAL_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, OriginalStop={setup.StopPrice:F2}, OperationalStopPlan={operationalStopPlan}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, IsScaleIn={position.IsScaleIn}, ScaleInIndex={position.ScaleInIndex}, StudyTarget2={target2:F2}, StudyTrigger={studyTrigger}, TriggerAtEntry={triggerAtEntry}, Risk={riskPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, RewardRiskToTarget2={rewardRiskToTarget2:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}", entryTrade.Time);
            }

            if (isHistorical)
                LogHistoricalPostEntryContext(position, entryTrade);

            return true;
        }

        private bool TryGetOpenDuplicateBasePosition(BalanceSetup setup, DateTime entryTimeUtc, out ActivePosition? duplicate)
        {
            duplicate = _activePositions
                .Where(p => !p.Closed && !p.IsScaleIn && p.Direction == setup.Direction)
                .Where(p => DateOnly.FromDateTime(MarketTimeZones.ToItaly(p.EntryTimeUtc)) == DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc)))
                .Where(p => Math.Abs(p.Target1Price - setup.POC) <= DuplicateBasePositionPocTolerancePoints)
                .Where(p => Math.Abs(p.Target2Price - GetStudyTarget2(setup)) <= DuplicateBasePositionValueEdgeTolerancePoints)
                .OrderByDescending(p => p.EntryTimeUtc)
                .FirstOrDefault();

            return duplicate != null;
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
                    setup.StudyFollowThroughBar = bar;
                    setup.StudyFollowThroughTimeUtc = eventTime;
                    setup.StudyTriggerBar = bar;
                    setup.StudyTriggerTimeUtc = eventTime;
                    var triggerMessage = $"[MR_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetFollowThroughLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, CandidateHigh={setup.RejectionHigh:F2}, CandidateLow={setup.RejectionLow:F2}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}";
                    _log(triggerMessage, IsHistoricalBar(bar));
                    StudyLog($"[DAY_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetFollowThroughLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, CandidateHigh={setup.RejectionHigh:F2}, CandidateLow={setup.RejectionLow:F2}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", eventTime);
                }

                if (!setup.StudyPocTriggerConfirmed && IsStudyPocTrigger(setup, candle))
                {
                    setup.StudyPocTriggerConfirmed = true;
                    setup.StudyPocTriggerBar = bar;
                    setup.StudyPocTriggerTimeUtc = eventTime;
                    setup.StudyTriggerBar = bar;
                    setup.StudyTriggerTimeUtc = eventTime;
                    var triggerMessage = $"[MR_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetPocTriggerLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}";
                    _log(triggerMessage, IsHistoricalBar(bar));
                    StudyLog($"[DAY_STUDY_TRIGGER] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={GetPocTriggerLabel(setup)}, Bar={bar}, {FormatTime(eventTime)}, CandidateBar={setup.RejectionBar}, Close={candle.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}", eventTime);
                }
            }
        }

        private static bool IsHistoricalCumulativeTradePosition(ActivePosition position)
        {
            return position.EntryModel.Contains("Historical", StringComparison.Ordinal);
        }

        private void UpdateHistoricalPositionsWithTrade(CumulativeTrade trade)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed && IsHistoricalCumulativeTradePosition(p)).ToList())
            {
                if (trade.Time <= position.EntryTimeUtc)
                    continue;

                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(trade.Time)) != DateOnly.FromDateTime(MarketTimeZones.ToItaly(position.EntryTimeUtc)))
                {
                    CloseHistoricalPositionAtEntryDayLondonClose(position);
                    continue;
                }

                if (!IsLondonTradeAllowed(trade.Time))
                    continue;

                var high = Math.Max(trade.FirstPrice, trade.Lastprice);
                var low = Math.Min(trade.FirstPrice, trade.Lastprice);
                UpdatePositionTracking(position, high, low, trade.Time);
                CheckHistoricalPositionExit(position, trade, high, low);
            }
        }

        private void CheckHistoricalPositionExit(ActivePosition position, CumulativeTrade trade, decimal high, decimal low)
        {
            var bar = FindBarByTime(trade.Time, position.EntryBar);
            if (IsFollowThroughSecondLegAuctionPosition(position))
            {
                CheckSecondLegAuctionStop(position, bar, trade.Time, high, low);
                return;
            }

            if (position.Direction == "Long")
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price > position.EntryPrice && high >= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, trade.Time);

                    if (position.Target2Price > position.EntryPrice && high >= position.Target2Price)
                    {
                        ClosePosition(position, bar, trade.Time, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price > position.EntryPrice && high >= position.Target1Price)
                {
                    ClosePosition(position, bar, trade.Time, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (low <= position.StopPrice && CanStopTrigger(position, bar))
                    ClosePosition(position, bar, trade.Time, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
            }
            else
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price < position.EntryPrice && low <= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, trade.Time);

                    if (position.Target2Price < position.EntryPrice && low <= position.Target2Price)
                    {
                        ClosePosition(position, bar, trade.Time, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price < position.EntryPrice && low <= position.Target1Price)
                {
                    ClosePosition(position, bar, trade.Time, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (high >= position.StopPrice && CanStopTrigger(position, bar))
                    ClosePosition(position, bar, trade.Time, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
            }
        }

        private void UpdateOpenHistoricalPositionsWithCompletedBars()
        {
            foreach (var position in _activePositions.Where(p => !p.Closed && IsHistoricalCumulativeTradePosition(p)).ToList())
            {
                var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(position.EntryTimeUtc));
                for (var bar = position.EntryBar + 1; bar < _currentBar; bar++)
                {
                    if (position.Closed)
                        break;

                    var candle = _getCandle(bar);
                    var eventTime = GetCandleEventTime(candle);
                    if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                        break;

                    if (!IsInLondonSession(candle.Time))
                        continue;

                    UpdatePositionTracking(position, bar, candle);
                    CheckPositionExit(position, bar, candle);
                }
            }
        }

        private void CloseOpenHistoricalPositionsAtSessionEnd()
        {
            foreach (var position in _activePositions.Where(p => !p.Closed && IsHistoricalCumulativeTradePosition(p)).ToList())
                CloseHistoricalPositionAtEntryDayLondonClose(position);
        }

        private void CloseHistoricalPositionAtEntryDayLondonClose(ActivePosition position)
        {
            if (position.Closed)
                return;

            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(position.EntryTimeUtc));
            var closeBar = position.EntryBar;
            for (var bar = position.EntryBar; bar < _currentBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (IsInLondonSession(candle.Time))
                    closeBar = bar;
            }

            var closeCandle = _getCandle(closeBar);
            ClosePosition(position, closeBar, GetCandleEventTime(closeCandle), "LONDON_CLOSE", closeCandle.Close);
        }

        private void LogHistoricalPostEntryContext(ActivePosition position, CumulativeTrade entryTrade)
        {
            DailyHistoricalLog($"[DAY_STUDY_HISTORICAL_POSITION_MODE] SetupId={position.SetupId}, EntryModel={position.EntryModel}, {FormatTime(entryTrade.Time)}, Mode=AllCumulativeTradesPostEntry, EntryBar={position.EntryBar}, EntryPrice={position.EntryPrice:F2}, Stop={position.StopPrice:F2}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}", entryTrade.Time);
        }

        private void UpdateActivePositions(int bar, IndicatorCandle candle)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                if (bar < position.EntryBar)
                    continue;

                if (_processingHistoricalPositions && IsHistoricalCumulativeTradePosition(position))
                    continue;

                UpdatePositionTracking(position, bar, candle);
                CheckPositionExit(position, bar, candle);
            }
        }

        private void UpdatePositionTracking(ActivePosition position, decimal high, decimal low, DateTime eventTimeUtc)
        {
            if (position.Direction == "Long")
            {
                if (high > position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = high;
                    position.MFE = high - position.EntryPrice;
                }

                if (low < position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = low;
                    position.MAE = position.EntryPrice - low;
                }
            }
            else
            {
                if (low < position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = low;
                    position.MFE = position.EntryPrice - low;
                }

                if (high > position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = high;
                    position.MAE = high - position.EntryPrice;
                }
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
            if (IsFollowThroughSecondLegAuctionPosition(position))
            {
                CheckSecondLegAuctionStop(position, bar, GetCandleEventTime(candle), candle.High, candle.Low);
                if (!position.Closed)
                {
                    var secondLegLondonTime = MarketTimeZones.ToLondon(GetCandleEventTime(candle));
                    if (secondLegLondonTime.Hour >= 16)
                        ClosePosition(position, bar, candle, "LONDON_CLOSE", candle.Close);
                }
                return;
            }

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

        private void CheckSecondLegAuctionStop(ActivePosition position, int bar, DateTime eventTimeUtc, decimal high, decimal low)
        {
            if (position.Direction == "Long")
            {
                if (low <= position.StopPrice && CanStopTrigger(position, bar))
                    ClosePosition(position, bar, eventTimeUtc, "SECOND_LEG_AUCTION_STOP", position.StopPrice);
            }
            else if (high >= position.StopPrice && CanStopTrigger(position, bar))
            {
                ClosePosition(position, bar, eventTimeUtc, "SECOND_LEG_AUCTION_STOP", position.StopPrice);
            }
        }

        private static bool CanStopTrigger(ActivePosition position, int bar)
        {
            return !position.StopProtectedAfterTarget1 || bar > position.Target1HitBar;
        }

        private void ProtectStopAfterTarget1(ActivePosition position, int bar, IndicatorCandle candle)
        {
            ProtectStopAfterTarget1(position, bar, GetCandleEventTime(candle));
        }

        private void ProtectStopAfterTarget1(ActivePosition position, int bar, DateTime eventTimeUtc)
        {
            position.Target1Hit = true;
            position.Target1HitBar = bar;
            position.Target1HitTimeUtc = eventTimeUtc;

            var protectedStop = position.EntryPrice;

            var oldStop = position.StopPrice;
            position.StopPrice = protectedStop;
            position.StopProtectedAfterTarget1 = true;

            var reward = position.Direction == "Long"
                ? position.Target1Price - position.EntryPrice
                : position.EntryPrice - position.Target1Price;
            position.RealizedPocExitPct = PocPartialExitPct;
            position.RunnerExitPct = RunnerExitPct;
            position.RealizedPocPnL = reward * PocPartialExitPct;

            var target1Message = $"[MR_TARGET1_HIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, RewardPoints={reward:F2}, Target1ExitPct={PocPartialExitPct:F2}, RunnerPct={RunnerExitPct:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, OldStop={oldStop:F2}, ProtectedStop={position.StopPrice:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}";
            var partialExitMessage = $"[MR_PARTIAL_EXIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={position.Target1Price:F2}, ExitReason=POC_PARTIAL_EXIT, ExitPct={PocPartialExitPct:F2}, RunnerPct={RunnerExitPct:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, RunnerStop={position.StopPrice:F2}, Target2={position.Target2Price:F2}";
            _log(target1Message, IsHistoricalBar(bar));
            _log(partialExitMessage, IsHistoricalBar(bar));
            if (position.EntryModel.Contains("Historical", StringComparison.Ordinal))
            {
                DailyHistoricalLog(target1Message, eventTimeUtc);
                DailyHistoricalLog(partialExitMessage, eventTimeUtc);
            }
        }

        private void ClosePosition(ActivePosition position, int bar, IndicatorCandle candle, string exitReason, decimal exitPrice)
        {
            ClosePosition(position, bar, GetCandleEventTime(candle), exitReason, exitPrice);
        }

        private void ClosePosition(ActivePosition position, int bar, DateTime eventTimeUtc, string exitReason, decimal exitPrice)
        {
            if (position.Closed)
                return;

            position.Closed = true;
            position.ExitReason = exitReason;
            position.ExitPrice = exitPrice;
            position.ExitBar = bar;
            position.ExitTimeUtc = eventTimeUtc;

            var pnl = position.Direction == "Long"
                ? exitPrice - position.EntryPrice
                : position.EntryPrice - exitPrice;
            var runnerPnl = position.Target1Hit ? pnl * position.RunnerExitPct : pnl;
            var blendedPnl = position.Target1Hit ? position.RealizedPocPnL + runnerPnl : pnl;
            var risk = position.Direction == "Long"
                ? position.EntryPrice - position.InitialStopPrice
                : position.InitialStopPrice - position.EntryPrice;
            var rMultiple = risk != 0 ? blendedPnl / risk : 0;

            var exitMessage = $"[MR_EXIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={blendedPnl:F2}, RunnerPnL={runnerPnl:F2}, FullRunnerPnL={pnl:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, Target1ExitPct={position.RealizedPocExitPct:F2}, RunnerPct={position.RunnerExitPct:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, Target1Hit={position.Target1Hit}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, StopProtected={position.StopProtectedAfterTarget1}";
            _log(exitMessage, IsHistoricalBar(bar));

            if (position.EntryModel.Contains("Historical", StringComparison.Ordinal))
            {
                DailyHistoricalLog(exitMessage, eventTimeUtc);
                StudyLog($"[DAY_STUDY_ACTUAL_EXIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={blendedPnl:F2}, RunnerPnL={runnerPnl:F2}, FullRunnerPnL={pnl:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, Target1ExitPct={position.RealizedPocExitPct:F2}, RunnerPct={position.RunnerExitPct:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, Target1Hit={position.Target1Hit}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, StopProtected={position.StopProtectedAfterTarget1}", eventTimeUtc);
                LogPocManagementStudy(position, bar, eventTimeUtc, exitReason, exitPrice, blendedPnl, pnl);
                if (EnableFollowThroughStudyLogs)
                    LogFollowThroughTargetDecisionStudy(position, eventTimeUtc, exitReason, exitPrice, blendedPnl);
            }

            TryAddFollowThroughSecondLegAuctionContext(position, bar, eventTimeUtc, exitReason, exitPrice);

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
                ExitTime = eventTimeUtc,
                ExitPrice = exitPrice,
                ExitReason = exitReason,
                PnL = blendedPnl,
                MFE = position.MFE,
                MAE = position.MAE,
                RMultiple = rMultiple
            });
        }

        private void LogPocManagementStudy(ActivePosition position, int bar, DateTime eventTimeUtc, string exitReason, decimal exitPrice, decimal currentPnl, decimal fullRunnerPnl)
        {
            var rewardToPoc = position.Direction == "Long"
                ? position.Target1Price - position.EntryPrice
                : position.EntryPrice - position.Target1Price;
            var pocAllOutPnl = position.Target1Hit ? rewardToPoc : currentPnl;
            var fullRunnerProtectedPnl = position.Target1Hit ? fullRunnerPnl : currentPnl;
            var pressure = position.Target1Hit
                ? GetPostPocPressureStats(position, position.Target1HitTimeUtc, eventTimeUtc)
                : new PocManagementPressureStats(0, 0, 0, 0, 0, 0);
            var continuationConfirmed = pressure.SameVolume > pressure.OppositeVolume && pressure.MaxSameVolume >= pressure.MaxOppositeVolume && pressure.SameTrades > 0;
            var secondsAfterPoc = position.Target1Hit ? (eventTimeUtc - position.Target1HitTimeUtc).TotalSeconds : 0;

            StudyLog($"[DAY_STUDY_POC_MANAGEMENT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, Target1Hit={position.Target1Hit}, Target1Time={(position.Target1Hit ? FormatTime(position.Target1HitTimeUtc) : "NA")}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, Current70_30PnL={currentPnl:F2}, PocAllOutPnL={pocAllOutPnl:F2}, FullRunnerProtectedPnL={fullRunnerProtectedPnl:F2}, DeltaPocAllOutVsCurrent={pocAllOutPnl - currentPnl:F2}, DeltaFullRunnerVsCurrent={fullRunnerProtectedPnl - currentPnl:F2}, SecondsAfterPoc={secondsAfterPoc:F1}, PostPocSameTrades={pressure.SameTrades}, PostPocSameVolume={pressure.SameVolume:F0}, PostPocMaxSameVolume={pressure.MaxSameVolume:F0}, PostPocOppositeTrades={pressure.OppositeTrades}, PostPocOppositeVolume={pressure.OppositeVolume:F0}, PostPocMaxOppositeVolume={pressure.MaxOppositeVolume:F0}, PostPocContinuationConfirmed={continuationConfirmed}, FabioStyleHypothesis=POC_PRIMARY_EXIT_THEN_REENTER_ON_NEW_CONFIRMATION", eventTimeUtc);
        }

        private void TryAddFollowThroughSecondLegAuctionContext(ActivePosition position, int bar, DateTime exitTimeUtc, string exitReason, decimal exitPrice)
        {
            if (exitReason != "TARGET2_HIT" || !IsFollowThroughContinuationTrigger(position.StudyTrigger))
                return;

            var decisionStop = position.Direction == "Long"
                ? exitPrice - 2m * _tickSize
                : exitPrice + 2m * _tickSize;
            _pendingSecondLegAuctionContexts.RemoveAll(c => c.SourceSetupId == position.SetupId);
            _pendingSecondLegAuctionContexts.Add(new FollowThroughSecondLegContext(
                position.SetupId,
                position.Direction,
                exitTimeUtc,
                exitPrice,
                position.Target1Price,
                decisionStop,
                bar,
                false));
            _log($"[MR_SECOND_LEG_AUCTION_ARMED] SourceSetupId={position.SetupId}, Direction={position.Direction}, DecisionTime={FormatTime(exitTimeUtc)}, DecisionPrice={exitPrice:F2}, OldPOC={position.Target1Price:F2}, DecisionStop={decisionStop:F2}, MaxSeconds={SecondLegImmediateMaxSeconds}", position.EntryModel.Contains("Historical", StringComparison.Ordinal));
        }

        private void LogFollowThroughTargetDecisionStudy(ActivePosition position, DateTime currentExitTimeUtc, string currentExitReason, decimal currentExitPrice, decimal currentPnl)
        {
            if (!IsFollowThroughContinuationTrigger(position.StudyTrigger) || !IsHistoricalCumulativeTradePosition(position))
                return;

            var targetTouchTime = IsDecisionAreaExit(position, currentExitReason, currentExitPrice)
                ? currentExitTimeUtc
                : FindDecisionAreaTouchTime(position.Direction, position.EntryTimeUtc, position.Target2Price, position.EntryBar);
            if (!targetTouchTime.HasValue)
                return;

            var decisionTime = targetTouchTime.Value;
            var stopLong = position.Direction == "Long" ? position.Target2Price - 2m * _tickSize : position.Target2Price + 2m * _tickSize;
            var stopPoc = position.Direction == "Long" ? position.Target1Price - 2m * _tickSize : position.Target1Price + 2m * _tickSize;
            var currentDelta = GetPostDecisionStats(position, decisionTime, null);
            var stats60 = GetPostDecisionStats(position, decisionTime, TimeSpan.FromSeconds(60));
            var stats120 = GetPostDecisionStats(position, decisionTime, TimeSpan.FromSeconds(120));
            var stats300 = GetPostDecisionStats(position, decisionTime, TimeSpan.FromSeconds(300));
            var reentry = FindFabioStyleContinuationReentry(position, decisionTime);
            var reentryOutcome = reentry == null
                ? new ProtectedStudyOutcome("NO_REENTRY", 0, 0, false)
                : EvaluateFollowThroughSecondLegOutcome(position.Direction, reentry.Lastprice, GetContinuationReentryStop(position, reentry.Lastprice), decisionTime, reentry.Time);
            var runnerStopVahOutcome = EvaluateFollowThroughSecondLegOutcome(position.Direction, position.Target2Price, stopLong, decisionTime, decisionTime);
            var runnerStopPocOutcome = EvaluateFollowThroughSecondLegOutcome(position.Direction, position.Target2Price, stopPoc, decisionTime, decisionTime);
            var reentryCandidate = reentry != null;
            var reentryText = reentry == null
                ? "NONE"
                : $"{FormatTime(reentry.Time)}:Price={reentry.Lastprice:F2}:Volume={reentry.Volume:F0}:Stop={GetContinuationReentryStop(position, reentry.Lastprice):F2}:OutcomeR={reentryOutcome.RMultiple:F2}:Outcome={reentryOutcome.ExitReason}:PnL={reentryOutcome.Pnl:F2}";
            var acceptance60 = IsDecisionAcceptanceConfirmed(stats60);
            var acceptance120 = IsDecisionAcceptanceConfirmed(stats120);
            var acceptance300 = IsDecisionAcceptanceConfirmed(stats300);
            var fabioBias = reentryCandidate ? "SECOND_LEG_CANDIDATE" : "TAKE_TARGET_NO_SECOND_LEG";

            DailyHistoricalLog($"[FOLLOWTHROUGH_TARGET_DECISION_STUDY] SetupId={position.SetupId}, Direction={position.Direction}, StudyTrigger={position.StudyTrigger}, EntryTime={FormatTime(position.EntryTimeUtc)}, DecisionTime={FormatTime(decisionTime)}, Entry={position.EntryPrice:F2}, DecisionPrice={position.Target2Price:F2}, POC={position.Target1Price:F2}, CurrentExitTime={FormatTime(currentExitTimeUtc)}, CurrentExitReason={currentExitReason}, CurrentExitPrice={currentExitPrice:F2}, CurrentPnL={currentPnl:F2}, MfeAfterDecision={currentDelta.Mfe:F2}, MaeAfterDecision={currentDelta.Mae:F2}, MaxFavorablePrice={currentDelta.MaxFavorablePrice:F2}, MaxAdversePrice={currentDelta.MaxAdversePrice:F2}, Same60={stats60.SameTrades}/{stats60.SameVolume:F0}/Max{stats60.MaxSameVolume:F0}, Opp60={stats60.OppositeTrades}/{stats60.OppositeVolume:F0}/Max{stats60.MaxOppositeVolume:F0}, CloseBeyond60={stats60.ClosesBeyondDecision}, CloseBackInside60={stats60.ClosesBackInsideDecision}, Accept60={acceptance60}, Same120={stats120.SameTrades}/{stats120.SameVolume:F0}/Max{stats120.MaxSameVolume:F0}, Opp120={stats120.OppositeTrades}/{stats120.OppositeVolume:F0}/Max{stats120.MaxOppositeVolume:F0}, CloseBeyond120={stats120.ClosesBeyondDecision}, CloseBackInside120={stats120.ClosesBackInsideDecision}, Accept120={acceptance120}, Same300={stats300.SameTrades}/{stats300.SameVolume:F0}/Max{stats300.MaxSameVolume:F0}, Opp300={stats300.OppositeTrades}/{stats300.OppositeVolume:F0}/Max{stats300.MaxOppositeVolume:F0}, CloseBeyond300={stats300.ClosesBeyondDecision}, CloseBackInside300={stats300.ClosesBackInsideDecision}, Accept300={acceptance300}, RunnerStopDecisionOutcome={runnerStopVahOutcome.ExitReason}, RunnerStopDecisionPnL={runnerStopVahOutcome.Pnl:F2}, RunnerStopDecisionR={runnerStopVahOutcome.RMultiple:F2}, RunnerStopPocOutcome={runnerStopPocOutcome.ExitReason}, RunnerStopPocPnL={runnerStopPocOutcome.Pnl:F2}, RunnerStopPocR={runnerStopPocOutcome.RMultiple:F2}, ReentryCandidate={reentryCandidate}, Reentry={reentryText}, FabioBias={fabioBias}", decisionTime);
            LogFollowThroughPostTargetOpportunityStudy(position, decisionTime, currentDelta, reentry, acceptance60, acceptance120, acceptance300);
            if (reentry != null)
                LogFollowThroughSecondLegStructureStudy(position, decisionTime, reentry, reentryOutcome);
        }

        private void LogFollowThroughPostTargetOpportunityStudy(ActivePosition position, DateTime decisionTimeUtc, PostDecisionContinuationStats currentDelta, CumulativeTrade? existingReentry, bool acceptance60, bool acceptance120, bool acceptance300)
        {
            var path = GetLondonSessionSnapshotsAfter(decisionTimeUtc, position.EntryBar);
            if (path.Count == 0)
                return;

            var firstCloseBeyond = FindFirstCloseBeyondDecision(path, position.Direction, position.Target2Price);
            var firstPullbackHold = FindFirstPullbackHold(path, position.Direction, position.Target2Price);
            var firstPocMigration = FindFirstPocMigration(path, position.Direction, position.Target2Price);
            var bestSameTrade = FindBestPostTargetSameDirectionTrade(position.Direction, decisionTimeUtc, position.Target2Price);
            var pressureToMigrationEnd = firstPocMigration?.EventTimeUtc ?? decisionTimeUtc.AddMinutes(15);
            var pressure = GetAuctionPressure(position.Direction, GetAggressionTradesBetween(decisionTimeUtc, pressureToMigrationEnd));
            var dayRank = bestSameTrade == null ? 0 : GetVolumeRankPct(bestSameTrade.Volume, GetAggressionTradesForDayUntil(bestSameTrade.Time));
            var decisionRank = bestSameTrade == null ? 0 : GetVolumeRankPct(bestSameTrade.Volume, GetAggressionTradesBetween(decisionTimeUtc, bestSameTrade.Time));
            var bestTradeText = bestSameTrade == null
                ? "NONE"
                : $"{FormatTime(bestSameTrade.Time)}:Price={bestSameTrade.Lastprice:F2}:Volume={bestSameTrade.Volume:F0}:DayRank={dayRank:F2}:DecisionRank={decisionRank:F2}";
            var firstCloseBeyondText = firstCloseBeyond == null ? "NONE" : FormatTime(firstCloseBeyond.EventTimeUtc);
            var firstPullbackHoldText = firstPullbackHold == null ? "NONE" : FormatTime(firstPullbackHold.EventTimeUtc);
            var firstPocMigrationText = firstPocMigration == null ? "NONE" : $"{FormatTime(firstPocMigration.EventTimeUtc)}:POC={firstPocMigration.PreviewPOC:F2}";
            var opportunityMode = GetPostTargetOpportunityMode(existingReentry != null, firstPullbackHold != null, firstPocMigration != null, firstCloseBeyond != null, currentDelta.Mfe, pressure);
            var opportunityReason = GetPostTargetOpportunityReason(existingReentry != null, firstPullbackHold != null, firstPocMigration != null, firstCloseBeyond != null, currentDelta.Mfe, pressure);
            var sameShare = pressure.TotalVolume == 0 ? 0 : pressure.SameVolume / pressure.TotalVolume;

            DailyHistoricalLog($"[FOLLOWTHROUGH_POST_TARGET_OPPORTUNITY_STUDY] SetupId={position.SetupId}, Direction={position.Direction}, StudyTrigger={position.StudyTrigger}, DecisionTime={FormatTime(decisionTimeUtc)}, DecisionPrice={position.Target2Price:F2}, CurrentMfeAfterDecision={currentDelta.Mfe:F2}, CurrentMaeAfterDecision={currentDelta.Mae:F2}, FirstCloseBeyondDecision={firstCloseBeyondText}, FirstPullbackHold={firstPullbackHoldText}, FirstPocMigration={firstPocMigrationText}, BestSameTrade={bestTradeText}, PressureSame={pressure.SameTrades}/{pressure.SameVolume:F0}/Max{pressure.MaxSameVolume:F0}, PressureOpp={pressure.OppositeTrades}/{pressure.OppositeVolume:F0}/Max{pressure.MaxOppositeVolume:F0}, SameShare={sameShare:F2}, Accept60={acceptance60}, Accept120={acceptance120}, Accept300={acceptance300}, ExistingReentryCandidate={existingReentry != null}, OpportunityMode={opportunityMode}, OpportunityReason={opportunityReason}", decisionTimeUtc);
            LogFollowThroughAlternativeSecondLegStudy(position, decisionTimeUtc, path, existingReentry, firstPullbackHold, firstPocMigration, bestSameTrade, pressure, opportunityMode, opportunityReason);
        }

        private void LogFollowThroughAlternativeSecondLegStudy(ActivePosition position, DateTime decisionTimeUtc, List<HistoricalBarSnapshot> path, CumulativeTrade? existingReentry, HistoricalBarSnapshot? firstPullbackHold, HistoricalBarSnapshot? firstPocMigration, CumulativeTrade? bestSameTrade, AuctionPressureStats pressure, string opportunityMode, string opportunityReason)
        {
            if (existingReentry != null || opportunityReason != "ALTERNATIVE_SECOND_LEG_FORM")
                return;

            var finalSnapshot = path[^1];
            var finalPoc = finalSnapshot.PreviewPOC;
            var pullbackOutcome = firstPullbackHold == null
                ? AlternativeSecondLegOutcome.None("NO_PULLBACK_HOLD")
                : EvaluateAlternativeSecondLegOutcome(position.Direction, firstPullbackHold.Close, firstPullbackHold.EventTimeUtc, path, finalPoc, finalSnapshot.Close);
            var pocMigrationOutcome = firstPocMigration == null
                ? AlternativeSecondLegOutcome.None("NO_POC_MIGRATION")
                : EvaluateAlternativeSecondLegOutcome(position.Direction, firstPocMigration.Close, firstPocMigration.EventTimeUtc, path, finalPoc, finalSnapshot.Close);
            var bestTradeOutcome = bestSameTrade == null
                ? AlternativeSecondLegOutcome.None("NO_BEST_SAME_TRADE")
                : EvaluateAlternativeSecondLegOutcome(position.Direction, bestSameTrade.Lastprice, bestSameTrade.Time, path, finalPoc, finalSnapshot.Close);
            var bestMode = GetBestAlternativeSecondLegMode(pullbackOutcome, pocMigrationOutcome, bestTradeOutcome);
            var sameShare = pressure.TotalVolume == 0 ? 0 : pressure.SameVolume / pressure.TotalVolume;
            var bestTradeText = bestSameTrade == null
                ? "NONE"
                : $"{FormatTime(bestSameTrade.Time)}:Price={bestSameTrade.Lastprice:F2}:Volume={bestSameTrade.Volume:F0}";

            DailyHistoricalLog($"[FOLLOWTHROUGH_ALTERNATIVE_SECOND_LEG_STUDY] SetupId={position.SetupId}, Direction={position.Direction}, StudyTrigger={position.StudyTrigger}, DecisionTime={FormatTime(decisionTimeUtc)}, DecisionPrice={position.Target2Price:F2}, OpportunityMode={opportunityMode}, OpportunityReason={opportunityReason}, PressureSame={pressure.SameTrades}/{pressure.SameVolume:F0}/Max{pressure.MaxSameVolume:F0}, PressureOpp={pressure.OppositeTrades}/{pressure.OppositeVolume:F0}/Max{pressure.MaxOppositeVolume:F0}, SameShare={sameShare:F2}, FinalStructureTime={FormatTime(finalSnapshot.EventTimeUtc)}, FinalPOC={finalPoc:F2}, PullbackEntry={FormatAlternativeOutcome(pullbackOutcome)}, PocMigrationEntry={FormatAlternativeOutcome(pocMigrationOutcome)}, BestSameTrade={bestTradeText}, BestSameTradeEntry={FormatAlternativeOutcome(bestTradeOutcome)}, BestAlternativeMode={bestMode}", decisionTimeUtc);
        }

        private AlternativeSecondLegOutcome EvaluateAlternativeSecondLegOutcome(string direction, decimal entry, DateTime entryTimeUtc, List<HistoricalBarSnapshot> path, decimal finalPoc, decimal londonClose)
        {
            var afterEntry = path.Where(s => s.EventTimeUtc >= entryTimeUtc).ToList();
            if (afterEntry.Count == 0 || finalPoc == 0)
                return AlternativeSecondLegOutcome.None("NO_PATH");

            var finalPocTouch = FindFirstTouch(afterEntry, direction, finalPoc);
            var mfe = 0m;
            var mae = 0m;
            foreach (var snapshot in afterEntry)
            {
                if (direction == "Long")
                {
                    mfe = Math.Max(mfe, snapshot.High - entry);
                    mae = Math.Max(mae, entry - snapshot.Low);
                }
                else
                {
                    mfe = Math.Max(mfe, entry - snapshot.Low);
                    mae = Math.Max(mae, snapshot.High - entry);
                }
            }

            var pnlToFinalPoc = GetDirectionalPnl(direction, entry, finalPoc);
            var londonClosePnl = GetDirectionalPnl(direction, entry, londonClose);
            return new AlternativeSecondLegOutcome("VALID", entryTimeUtc, entry, finalPocTouch != null, finalPocTouch?.EventTimeUtc, pnlToFinalPoc, londonClosePnl, mfe, mae);
        }

        private static string FormatAlternativeOutcome(AlternativeSecondLegOutcome outcome)
        {
            if (outcome.Status != "VALID")
                return outcome.Status;

            var touchTime = outcome.FinalPocTouchTime.HasValue ? MarketTimeZones.FormatUtcLondonItaly(outcome.FinalPocTouchTime.Value) : "NONE";
            return $"{MarketTimeZones.FormatUtcLondonItaly(outcome.EntryTimeUtc)}:Entry={outcome.EntryPrice:F2}:ReachedFinalPOC={outcome.ReachedFinalPoc}:FinalPOCTouch={touchTime}:PnlToFinalPOC={outcome.PnlToFinalPoc:F2}:LondonClosePnL={outcome.LondonClosePnl:F2}:MFE={outcome.Mfe:F2}:MAE={outcome.Mae:F2}";
        }

        private static string GetBestAlternativeSecondLegMode(AlternativeSecondLegOutcome pullback, AlternativeSecondLegOutcome pocMigration, AlternativeSecondLegOutcome bestTrade)
        {
            var candidates = new[]
            {
                (Mode: "PULLBACK_HOLD", Outcome: pullback),
                (Mode: "POC_MIGRATION", Outcome: pocMigration),
                (Mode: "BEST_SAME_TRADE", Outcome: bestTrade)
            }
                .Where(c => c.Outcome.Status == "VALID" && c.Outcome.ReachedFinalPoc)
                .OrderByDescending(c => c.Outcome.PnlToFinalPoc)
                .ToList();

            return candidates.Count == 0 ? "NONE" : candidates[0].Mode;
        }

        private HistoricalBarSnapshot? FindFirstCloseBeyondDecision(List<HistoricalBarSnapshot> path, string direction, decimal decisionPrice)
        {
            return path.FirstOrDefault(s => IsBeyondDecisionClose(direction, s.Close, decisionPrice));
        }

        private HistoricalBarSnapshot? FindFirstPullbackHold(List<HistoricalBarSnapshot> path, string direction, decimal decisionPrice)
        {
            return path.FirstOrDefault(s => direction == "Long"
                ? s.Low <= decisionPrice && s.Close > decisionPrice
                : s.High >= decisionPrice && s.Close < decisionPrice);
        }

        private HistoricalBarSnapshot? FindFirstPocMigration(List<HistoricalBarSnapshot> path, string direction, decimal decisionPrice)
        {
            return path.FirstOrDefault(s => direction == "Long"
                ? s.PreviewPOC > decisionPrice
                : s.PreviewPOC < decisionPrice);
        }

        private CumulativeTrade? FindBestPostTargetSameDirectionTrade(string direction, DateTime decisionTimeUtc, decimal decisionPrice)
        {
            var expectedDirection = direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var endTime = decisionTimeUtc.AddMinutes(15);
            return _lastHistoricalTrades
                .Where(t => t.Time > decisionTimeUtc && t.Time <= endTime)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Direction == expectedDirection)
                .Where(t => IsBeyondDecisionArea(direction, t.Lastprice, decisionPrice))
                .OrderByDescending(t => t.Volume)
                .ThenBy(t => t.Time)
                .FirstOrDefault();
        }

        private static string GetPostTargetOpportunityMode(bool hasExistingReentry, bool hasPullbackHold, bool hasPocMigration, bool hasCloseBeyond, decimal mfe, AuctionPressureStats pressure)
        {
            if (hasExistingReentry)
                return "IMMEDIATE_BIG_TRADE";
            if (hasPullbackHold && hasPocMigration)
                return "PULLBACK_HOLD_AND_POC_MIGRATION";
            if (hasPocMigration)
                return "POC_MIGRATION_CONFIRMATION";
            if (hasCloseBeyond)
                return "BREAKOUT_ACCEPTANCE_NO_REENTRY";
            if (mfe > 0 && pressure.SameVolume > pressure.OppositeVolume)
                return "PRESSURE_EXTENSION_NO_STRUCTURE";
            if (mfe > 0)
                return "MFE_ONLY_NO_CONFIRMATION";

            return "NO_SECOND_LEG";
        }

        private static string GetPostTargetOpportunityReason(bool hasExistingReentry, bool hasPullbackHold, bool hasPocMigration, bool hasCloseBeyond, decimal mfe, AuctionPressureStats pressure)
        {
            if (hasExistingReentry)
                return "EXISTING_REENTRY_DETECTED";
            if (!hasCloseBeyond)
                return "NO_CLOSE_BEYOND_DECISION_AREA";
            if (!hasPullbackHold && !hasPocMigration)
                return "NO_PULLBACK_HOLD_OR_POC_MIGRATION";
            if (!hasPocMigration)
                return "NO_POC_MIGRATION";
            if (pressure.SameVolume <= pressure.OppositeVolume)
                return "SAME_PRESSURE_NOT_DOMINANT";
            if (mfe <= 0)
                return "NO_FAVORABLE_EXTENSION";

            return "ALTERNATIVE_SECOND_LEG_FORM";
        }

        private void LogFollowThroughSecondLegStructureStudy(ActivePosition position, DateTime decisionTimeUtc, CumulativeTrade reentry, ProtectedStudyOutcome reentryOutcome)
        {
            var path = GetLondonSessionSnapshotsAfter(reentry.Time, position.EntryBar);
            if (path.Count == 0)
                return;

            var maxFavorablePrice = reentry.Lastprice;
            var maxAdversePrice = reentry.Lastprice;
            var mfe = 0m;
            var mae = 0m;
            var closesBeyondOldDecision = 0;
            var closesBackInsideOldDecision = 0;
            var wicksBackInsideOldDecision = 0;
            HistoricalBarSnapshot? firstMigratedPoc = null;

            foreach (var snapshot in path)
            {
                if (position.Direction == "Long")
                {
                    maxFavorablePrice = Math.Max(maxFavorablePrice, snapshot.High);
                    maxAdversePrice = Math.Min(maxAdversePrice, snapshot.Low);
                    mfe = Math.Max(mfe, snapshot.High - reentry.Lastprice);
                    mae = Math.Max(mae, reentry.Lastprice - snapshot.Low);
                    if (snapshot.Close > position.Target2Price)
                        closesBeyondOldDecision++;
                    if (snapshot.Close < position.Target2Price)
                        closesBackInsideOldDecision++;
                    if (snapshot.Low < position.Target2Price)
                        wicksBackInsideOldDecision++;
                    if (firstMigratedPoc == null && snapshot.PreviewPOC > position.Target2Price)
                        firstMigratedPoc = snapshot;
                }
                else
                {
                    maxFavorablePrice = Math.Min(maxFavorablePrice, snapshot.Low);
                    maxAdversePrice = Math.Max(maxAdversePrice, snapshot.High);
                    mfe = Math.Max(mfe, reentry.Lastprice - snapshot.Low);
                    mae = Math.Max(mae, snapshot.High - reentry.Lastprice);
                    if (snapshot.Close < position.Target2Price)
                        closesBeyondOldDecision++;
                    if (snapshot.Close > position.Target2Price)
                        closesBackInsideOldDecision++;
                    if (snapshot.High > position.Target2Price)
                        wicksBackInsideOldDecision++;
                    if (firstMigratedPoc == null && snapshot.PreviewPOC < position.Target2Price)
                        firstMigratedPoc = snapshot;
                }
            }

            var finalSnapshot = path[^1];
            var finalPoc = finalSnapshot.PreviewPOC;
            var finalVah = finalSnapshot.PreviewVAH;
            var finalVal = finalSnapshot.PreviewVAL;
            var finalPocTouch = finalPoc == 0 ? null : FindFirstTouch(path, position.Direction, finalPoc);
            var finalVahTouch = finalVah == 0 ? null : FindFirstTouch(path, position.Direction, finalVah);
            var finalValTouch = finalVal == 0 ? null : FindFirstTouch(path, position.Direction, finalVal);
            var pnlToFinalPoc = finalPoc == 0 ? 0 : GetDirectionalPnl(position.Direction, reentry.Lastprice, finalPoc);
            var pnlToFinalVah = finalVah == 0 ? 0 : GetDirectionalPnl(position.Direction, reentry.Lastprice, finalVah);
            var pnlToFinalVal = finalVal == 0 ? 0 : GetDirectionalPnl(position.Direction, reentry.Lastprice, finalVal);
            var londonClosePnl = GetDirectionalPnl(position.Direction, reentry.Lastprice, finalSnapshot.Close);
            var firstMigratedPocText = firstMigratedPoc == null
                ? "NONE"
                : $"{FormatTime(firstMigratedPoc.EventTimeUtc)}:POC={firstMigratedPoc.PreviewPOC:F2}:VAH={firstMigratedPoc.PreviewVAH:F2}:VAL={firstMigratedPoc.PreviewVAL:F2}";
            var finalPocTouchText = finalPocTouch == null ? "NONE" : FormatTime(finalPocTouch.EventTimeUtc);
            var finalVahTouchText = finalVahTouch == null ? "NONE" : FormatTime(finalVahTouch.EventTimeUtc);
            var finalValTouchText = finalValTouch == null ? "NONE" : FormatTime(finalValTouch.EventTimeUtc);
            var reentryStop = GetContinuationReentryStop(position, reentry.Lastprice);
            var reentryRisk = Math.Abs(reentry.Lastprice - reentryStop);
            var initialMae = GetSecondLegMae(position.Direction, reentry.Lastprice, path.Take(SecondLegInitialMaeBars));
            var strongReentryVolume = reentry.Volume >= SecondLegStrongReentryVolume;
            var oldDecisionHolds = closesBackInsideOldDecision == 0 && wicksBackInsideOldDecision == 0;
            var initialMaeContained = reentryRisk > 0 && initialMae <= reentryRisk * SecondLegMaxInitialMaeRiskMultiple;
            var pocMigrated = firstMigratedPoc != null;
            var structureTargetReached = finalPocTouch != null;
            var passesStructureFilter = strongReentryVolume && oldDecisionHolds && initialMaeContained && pocMigrated && structureTargetReached && pnlToFinalPoc > 0;
            var filterFailures = GetSecondLegFilterFailures(strongReentryVolume, oldDecisionHolds, initialMaeContained, pocMigrated, structureTargetReached, pnlToFinalPoc);

            DailyHistoricalLog($"[FOLLOWTHROUGH_SECOND_LEG_STRUCTURE_STUDY] SetupId={position.SetupId}, Direction={position.Direction}, StudyTrigger={position.StudyTrigger}, DecisionTime={FormatTime(decisionTimeUtc)}, ReentryTime={FormatTime(reentry.Time)}, ReentryPrice={reentry.Lastprice:F2}, ReentryVolume={reentry.Volume:F0}, ReentryOutcome={reentryOutcome.ExitReason}, ReentryOutcomePnL={reentryOutcome.Pnl:F2}, ReentryOutcomeR={reentryOutcome.RMultiple:F2}, OldDecisionPrice={position.Target2Price:F2}, OldPOC={position.Target1Price:F2}, BarsAfterReentry={path.Count}, MfeAfterReentry={mfe:F2}, MaeAfterReentry={mae:F2}, InitialMaeBars={SecondLegInitialMaeBars}, InitialMae={initialMae:F2}, ReentryStop={reentryStop:F2}, ReentryRisk={reentryRisk:F2}, MaxFavorablePrice={maxFavorablePrice:F2}, MaxAdversePrice={maxAdversePrice:F2}, ClosesBeyondOldDecision={closesBeyondOldDecision}, ClosesBackInsideOldDecision={closesBackInsideOldDecision}, WicksBackInsideOldDecision={wicksBackInsideOldDecision}, FirstMigratedPoc={firstMigratedPocText}, FinalStructureTime={FormatTime(finalSnapshot.EventTimeUtc)}, FinalPOC={finalPoc:F2}, FinalVAH={finalVah:F2}, FinalVAL={finalVal:F2}, ReachedFinalPOC={finalPocTouch != null}, FinalPOCTouchTime={finalPocTouchText}, PnlToFinalPOC={pnlToFinalPoc:F2}, ReachedFinalVAH={finalVahTouch != null}, FinalVAHTouchTime={finalVahTouchText}, PnlToFinalVAH={pnlToFinalVah:F2}, ReachedFinalVAL={finalValTouch != null}, FinalVALTouchTime={finalValTouchText}, PnlToFinalVAL={pnlToFinalVal:F2}, LondonClose={finalSnapshot.Close:F2}, LondonClosePnL={londonClosePnl:F2}", reentry.Time);
            DailyHistoricalLog($"[FOLLOWTHROUGH_SECOND_LEG_FILTER_STUDY] SetupId={position.SetupId}, Direction={position.Direction}, ReentryTime={FormatTime(reentry.Time)}, ReentryPrice={reentry.Lastprice:F2}, ReentryVolume={reentry.Volume:F0}, StrongReentryVolume={strongReentryVolume}, MinStrongReentryVolume={SecondLegStrongReentryVolume:F0}, OldDecisionHolds={oldDecisionHolds}, ClosesBackInsideOldDecision={closesBackInsideOldDecision}, WicksBackInsideOldDecision={wicksBackInsideOldDecision}, InitialMaeContained={initialMaeContained}, InitialMae={initialMae:F2}, MaxInitialMae={reentryRisk * SecondLegMaxInitialMaeRiskMultiple:F2}, PocMigrated={pocMigrated}, FirstMigratedPoc={firstMigratedPocText}, StructureTarget=FINAL_POC, StructureTargetPrice={finalPoc:F2}, StructureTargetReached={structureTargetReached}, StructureTargetTouchTime={finalPocTouchText}, StructureTargetPnL={pnlToFinalPoc:F2}, PassesStructureFilter={passesStructureFilter}, FilterFailures={filterFailures}", reentry.Time);
            LogFollowThroughSecondLegAuctionStudy(position, decisionTimeUtc, reentry, path, firstMigratedPoc, initialMae, mfe, mae, closesBeyondOldDecision, closesBackInsideOldDecision, wicksBackInsideOldDecision, finalPoc, finalPocTouch, pnlToFinalPoc, finalSnapshot);
        }

        private void LogFollowThroughSecondLegAuctionStudy(ActivePosition position, DateTime decisionTimeUtc, CumulativeTrade reentry, List<HistoricalBarSnapshot> path, HistoricalBarSnapshot? firstMigratedPoc, decimal initialMae, decimal mfe, decimal mae, int closesBeyondOldDecision, int closesBackInsideOldDecision, int wicksBackInsideOldDecision, decimal finalPoc, HistoricalBarSnapshot? finalPocTouch, decimal pnlToFinalPoc, HistoricalBarSnapshot finalSnapshot)
        {
            var direction = position.Direction;
            var impulseFromDecision = Math.Abs(reentry.Lastprice - position.Target2Price);
            var pullbackVsImpulse = impulseFromDecision > 0 ? initialMae / impulseFromDecision : 0;
            var dayTradesToReentry = GetAggressionTradesForDayUntil(reentry.Time);
            var decisionToReentryTrades = GetAggressionTradesBetween(decisionTimeUtc, reentry.Time);
            var migrationEnd = firstMigratedPoc?.EventTimeUtc ?? finalSnapshot.EventTimeUtc;
            var postReentryTrades = GetAggressionTradesBetween(reentry.Time, migrationEnd);
            var decisionPressure = GetAuctionPressure(direction, decisionToReentryTrades);
            var postPressure = GetAuctionPressure(direction, postReentryTrades);
            var dayRank = GetVolumeRankPct(reentry.Volume, dayTradesToReentry);
            var decisionRank = GetVolumeRankPct(reentry.Volume, decisionToReentryTrades);
            var reentryVsDecisionMedian = GetVolumeRatioToMedian(reentry.Volume, decisionToReentryTrades);
            var barsToPocMigration = firstMigratedPoc == null ? -1 : Math.Max(0, firstMigratedPoc.Bar - FindBarByTime(reentry.Time, position.EntryBar));
            var pocMigrationDistanceFromOldDecision = firstMigratedPoc == null ? 0 : GetDirectionalPnl(direction, position.Target2Price, firstMigratedPoc.PreviewPOC);
            var pocMigrationDistanceFromOldPoc = firstMigratedPoc == null ? 0 : GetDirectionalPnl(direction, position.Target1Price, firstMigratedPoc.PreviewPOC);
            var volumeBeyondOldDecision = path.Where(s => IsBeyondDecisionClose(direction, s.Close, position.Target2Price)).Sum(s => s.Volume);
            var volumeBackInsideOldDecision = path.Where(s => !IsBeyondDecisionClose(direction, s.Close, position.Target2Price)).Sum(s => s.Volume);
            var holdScore = path.Count == 0 ? 0 : (decimal)(path.Count - closesBackInsideOldDecision) / path.Count;
            var wickHoldScore = path.Count == 0 ? 0 : (decimal)(path.Count - wicksBackInsideOldDecision) / path.Count;
            var pullbackScore = impulseFromDecision + initialMae == 0 ? 0 : impulseFromDecision / (impulseFromDecision + initialMae);
            var samePressureShare = postPressure.TotalVolume == 0 ? 0 : postPressure.SameVolume / postPressure.TotalVolume;
            var pocMigrationScore = firstMigratedPoc == null ? 0 : 1m / (1m + Math.Max(0, barsToPocMigration));
            var auctionScore = (dayRank + decisionRank + holdScore + wickHoldScore + pullbackScore + samePressureShare + pocMigrationScore) / 7m;
            var auctionRead = GetAuctionRead(firstMigratedPoc != null, closesBeyondOldDecision, closesBackInsideOldDecision, wicksBackInsideOldDecision, postPressure, pnlToFinalPoc);
            var finalPocTouchText = finalPocTouch == null ? "NONE" : FormatTime(finalPocTouch.EventTimeUtc);

            DailyHistoricalLog($"[FOLLOWTHROUGH_SECOND_LEG_AUCTION_STUDY] SetupId={position.SetupId}, Direction={direction}, ReentryTime={FormatTime(reentry.Time)}, ReentryPrice={reentry.Lastprice:F2}, ReentryVolume={reentry.Volume:F0}, DayVolumeRankPct={dayRank:F2}, DecisionWindowVolumeRankPct={decisionRank:F2}, ReentryVsDecisionMedian={reentryVsDecisionMedian:F2}, DecisionSame={decisionPressure.SameTrades}/{decisionPressure.SameVolume:F0}/Max{decisionPressure.MaxSameVolume:F0}, DecisionOpp={decisionPressure.OppositeTrades}/{decisionPressure.OppositeVolume:F0}/Max{decisionPressure.MaxOppositeVolume:F0}, PostSame={postPressure.SameTrades}/{postPressure.SameVolume:F0}/Max{postPressure.MaxSameVolume:F0}, PostOpp={postPressure.OppositeTrades}/{postPressure.OppositeVolume:F0}/Max{postPressure.MaxOppositeVolume:F0}, PostSameShare={samePressureShare:F2}, ImpulseFromDecision={impulseFromDecision:F2}, InitialMae={initialMae:F2}, PullbackVsImpulse={pullbackVsImpulse:F2}, MfeAfterReentry={mfe:F2}, MaeAfterReentry={mae:F2}, ClosesBeyondOldDecision={closesBeyondOldDecision}, ClosesBackInsideOldDecision={closesBackInsideOldDecision}, WicksBackInsideOldDecision={wicksBackInsideOldDecision}, HoldScore={holdScore:F2}, WickHoldScore={wickHoldScore:F2}, VolumeBeyondOldDecision={volumeBeyondOldDecision:F0}, VolumeBackInsideOldDecision={volumeBackInsideOldDecision:F0}, PocMigrated={firstMigratedPoc != null}, BarsToPocMigration={barsToPocMigration}, PocMigrationDistanceFromOldDecision={pocMigrationDistanceFromOldDecision:F2}, PocMigrationDistanceFromOldPoc={pocMigrationDistanceFromOldPoc:F2}, PocMigrationScore={pocMigrationScore:F2}, StructureTarget=FINAL_POC, StructureTargetPrice={finalPoc:F2}, StructureTargetTouchTime={finalPocTouchText}, StructureTargetPnL={pnlToFinalPoc:F2}, AuctionScore={auctionScore:F2}, AuctionRead={auctionRead}", reentry.Time);
        }

        private List<CumulativeTrade> GetAggressionTradesForDayUntil(DateTime toUtc)
        {
            var day = DateOnly.FromDateTime(MarketTimeZones.ToItaly(toUtc));
            return _lastHistoricalTrades
                .Where(t => t.Time <= toUtc)
                .Where(t => DateOnly.FromDateTime(MarketTimeZones.ToItaly(t.Time)) == day)
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .ToList();
        }

        private List<CumulativeTrade> GetAggressionTradesBetween(DateTime fromUtc, DateTime toUtc)
        {
            return _lastHistoricalTrades
                .Where(t => t.Time >= fromUtc && t.Time <= toUtc)
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .ToList();
        }

        private static AuctionPressureStats GetAuctionPressure(string direction, List<CumulativeTrade> trades)
        {
            var sameDirection = direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var same = trades.Where(t => t.Direction == sameDirection).ToList();
            var opposite = trades.Where(t => t.Direction != sameDirection).ToList();
            var sameVolume = same.Sum(t => t.Volume);
            var oppositeVolume = opposite.Sum(t => t.Volume);
            return new AuctionPressureStats(
                same.Count,
                sameVolume,
                same.Count > 0 ? same.Max(t => t.Volume) : 0,
                opposite.Count,
                oppositeVolume,
                opposite.Count > 0 ? opposite.Max(t => t.Volume) : 0,
                sameVolume + oppositeVolume);
        }

        private static decimal GetVolumeRankPct(decimal volume, List<CumulativeTrade> trades)
        {
            if (trades.Count == 0)
                return 0;

            var lessOrEqual = trades.Count(t => t.Volume <= volume);
            return (decimal)lessOrEqual / trades.Count;
        }

        private static decimal GetVolumeRatioToMedian(decimal volume, List<CumulativeTrade> trades)
        {
            if (trades.Count == 0)
                return 0;

            var ordered = trades.Select(t => t.Volume).OrderBy(v => v).ToList();
            var middle = ordered.Count / 2;
            var median = ordered.Count % 2 == 0
                ? (ordered[middle - 1] + ordered[middle]) / 2m
                : ordered[middle];
            return median <= 0 ? 0 : volume / median;
        }

        private static bool IsBeyondDecisionClose(string direction, decimal close, decimal decisionPrice)
        {
            return direction == "Long" ? close > decisionPrice : close < decisionPrice;
        }

        private static string GetAuctionRead(bool pocMigrated, int closesBeyondOldDecision, int closesBackInsideOldDecision, int wicksBackInsideOldDecision, AuctionPressureStats postPressure, decimal structureTargetPnl)
        {
            if (!pocMigrated)
                return "NO_POC_MIGRATION";
            if (structureTargetPnl <= 0)
                return "OPPOSITE_AUCTION_ACCEPTED";
            if (wicksBackInsideOldDecision > closesBeyondOldDecision || closesBackInsideOldDecision > closesBeyondOldDecision)
                return "WEAK_BREAK_NO_ACCEPTANCE";
            if (postPressure.OppositeVolume > postPressure.SameVolume && postPressure.MaxOppositeVolume >= postPressure.MaxSameVolume)
                return "OPPOSITE_ABSORPTION";
            if (postPressure.SameVolume > postPressure.OppositeVolume && closesBeyondOldDecision > 0)
                return "NEW_AUCTION_ACCEPTED";

            return "MIXED_AUCTION";
        }

        private static decimal GetSecondLegMae(string direction, decimal entry, IEnumerable<HistoricalBarSnapshot> path)
        {
            var mae = 0m;
            foreach (var snapshot in path)
            {
                mae = direction == "Long"
                    ? Math.Max(mae, entry - snapshot.Low)
                    : Math.Max(mae, snapshot.High - entry);
            }

            return mae;
        }

        private static string GetSecondLegFilterFailures(bool strongReentryVolume, bool oldDecisionHolds, bool initialMaeContained, bool pocMigrated, bool structureTargetReached, decimal structureTargetPnl)
        {
            var failures = new List<string>();
            if (!strongReentryVolume)
                failures.Add("WEAK_REENTRY_VOLUME");
            if (!oldDecisionHolds)
                failures.Add("OLD_DECISION_FAILED");
            if (!initialMaeContained)
                failures.Add("INITIAL_MAE_TOO_HIGH");
            if (!pocMigrated)
                failures.Add("POC_NOT_MIGRATED");
            if (!structureTargetReached)
                failures.Add("STRUCTURE_TARGET_NOT_REACHED");
            if (structureTargetPnl <= 0)
                failures.Add("STRUCTURE_TARGET_NOT_PROFITABLE");

            return failures.Count == 0 ? "NONE" : string.Join("|", failures);
        }

        private List<HistoricalBarSnapshot> GetLondonSessionSnapshotsAfter(DateTime fromUtc, int fallbackBar)
        {
            var result = new List<HistoricalBarSnapshot>();
            var day = DateOnly.FromDateTime(MarketTimeZones.ToItaly(fromUtc));
            var startBar = FindBarByTime(fromUtc, fallbackBar);
            for (var bar = startBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (eventTime <= fromUtc)
                    continue;
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != day)
                    break;
                if (!IsInLondonSession(eventTime))
                    break;

                var snapshot = _historicalBarSnapshots.TryGetValue(bar, out var captured)
                    ? captured
                    : CreateHistoricalBarSnapshot(bar, candle);
                result.Add(snapshot);
            }

            return result;
        }

        private HistoricalBarSnapshot? FindFirstTouch(List<HistoricalBarSnapshot> path, string direction, decimal price)
        {
            foreach (var snapshot in path)
            {
                if (direction == "Long")
                {
                    if (snapshot.High >= price)
                        return snapshot;
                }
                else if (snapshot.Low <= price)
                {
                    return snapshot;
                }
            }

            return null;
        }

        private static decimal GetDirectionalPnl(string direction, decimal entry, decimal exit)
        {
            return direction == "Long" ? exit - entry : entry - exit;
        }

        private PocManagementPressureStats GetPostPocPressureStats(ActivePosition position, DateTime fromUtc, DateTime toUtc)
        {
            var expectedDirection = position.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = position.Direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var trades = _lastHistoricalTrades
                .Where(t => t.Time > fromUtc && t.Time <= toUtc)
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .ToList();
            var same = trades.Where(t => t.Direction == expectedDirection).ToList();
            var opposite = trades.Where(t => t.Direction == oppositeDirection).ToList();

            return new PocManagementPressureStats(
                same.Count,
                same.Sum(t => t.Volume),
                same.Count > 0 ? same.Max(t => t.Volume) : 0,
                opposite.Count,
                opposite.Sum(t => t.Volume),
                opposite.Count > 0 ? opposite.Max(t => t.Volume) : 0);
        }

        private void LogMfe(ActivePosition position, int bar, IndicatorCandle candle)
        {
            _log($"[MR_MFE_UPDATE] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, EntryPrice={position.EntryPrice:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, MaxFavorablePrice={position.MaxFavorablePrice:F2}, MaxAdversePrice={position.MaxAdversePrice:F2}", IsHistoricalBar(bar));
        }


        private void ResetStudyLog()
        {
            _studyLoggedBars.Clear();
            _historicalLogInitialized = false;
            _historicalStudyActive = !_dailyHistoricalDebugLogsEnabled;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_historicalLogPath)!);
                _historicalLogSequence = 0;
                var writeUtc = DateTime.UtcNow;
                var writeItaly = MarketTimeZones.ToItaly(writeUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                var mode = _dailyHistoricalDebugLogsEnabled ? "DailyHistoricalDebug" : "AggregatedHistoricalStudy";
                File.WriteAllText(_historicalLogPath, $"[Source=Historical] [Seq={++_historicalLogSequence}] [WriteItaly={writeItaly}] [WriteUtc={writeUtc:yyyy-MM-dd HH:mm:ss.fff}] [HISTORICAL_STUDY_START] Mode={mode}, AggregatedStudyEnabled={_historicalStudyActive}, DailyDebugEnabled={_dailyHistoricalDebugLogsEnabled}, HistoricalIntrabarEnabled={EnableHistoricalIntrabarFromCumulativeTrades}, MinAggressionVolume={MinAggressionVolume:F0}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, DynamicStopMaxValueAreaRiskPct={DynamicStopMaxValueAreaRiskPct:F2}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}, CreatedItaly={MarketTimeZones.ToItaly(DateTime.UtcNow):yyyy-MM-dd HH:mm:ss}{Environment.NewLine}");
                _historicalLogInitialized = true;
            }
            catch
            {
            }
        }

        private void ResetDailyHistoricalDebugLogs()
        {
            _initializedDailyLogs.Clear();
            _dailyHistoricalLogSequences.Clear();

            try
            {
                Directory.CreateDirectory(_dailyHistoricalLogDirectory);
                foreach (var path in Directory.EnumerateFiles(_dailyHistoricalLogDirectory, "FabioOrderFlow-day-*.log"))
                    File.Delete(path);
            }
            catch
            {
            }
        }

        private void StudyLog(string message, DateTime eventUtc)
        {
            if (!_historicalStudyDebugEnabled || !ShouldDebugHistoricalDay(eventUtc))
                return;

            DailyHistoricalLog(message, eventUtc);

            try
            {
                if (!_historicalStudyActive)
                    return;

                if (!_historicalLogInitialized)
                    ResetStudyLog();

                var writeUtc = DateTime.UtcNow;
                var writeItaly = MarketTimeZones.ToItaly(writeUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                var line = $"[Source=Historical] [Seq={++_historicalLogSequence}] [WriteItaly={writeItaly}] [WriteUtc={writeUtc:yyyy-MM-dd HH:mm:ss.fff}] [EventItaly={MarketTimeZones.ToItaly(eventUtc):yyyy-MM-dd HH:mm:ss}] [EventLondon={MarketTimeZones.ToLondon(eventUtc):yyyy-MM-dd HH:mm:ss}] [EventUtc={eventUtc:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(_historicalLogPath, line + Environment.NewLine);
            }
            catch
            {
            }
        }

        private void DailyHistoricalLog(string message, DateTime eventUtc)
        {
            if (!_dailyHistoricalDebugLogsEnabled)
                return;

            try
            {
                var eventItaly = MarketTimeZones.ToItaly(eventUtc);
                var day = DateOnly.FromDateTime(eventItaly);
                Directory.CreateDirectory(_dailyHistoricalLogDirectory);
                var path = Path.Combine(_dailyHistoricalLogDirectory, $"FabioOrderFlow-day-{day:yyyy-MM-dd}.log");
                if (!_initializedDailyLogs.Contains(day))
                {
                    var headerUtc = DateTime.UtcNow;
                    var headerItaly = MarketTimeZones.ToItaly(headerUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    File.WriteAllText(path, $"[Source=HistoricalDaily] [Seq=1] [WriteItaly={headerItaly}] [WriteUtc={headerUtc:yyyy-MM-dd HH:mm:ss.fff}] [DAY_DEBUG_START] Day={day:yyyy-MM-dd}, HistoricalIntrabarEnabled={EnableHistoricalIntrabarFromCumulativeTrades}, AggregatedStudyEnabled={_historicalStudyActive}, MinAggressionVolume={MinAggressionVolume:F0}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}{Environment.NewLine}");
                    _initializedDailyLogs.Add(day);
                    _dailyHistoricalLogSequences[day] = 1;
                }

                var writeUtc = DateTime.UtcNow;
                var writeItaly = MarketTimeZones.ToItaly(writeUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                var seq = _dailyHistoricalLogSequences.TryGetValue(day, out var currentSeq) ? currentSeq + 1 : 2;
                _dailyHistoricalLogSequences[day] = seq;
                var line = $"[Source=HistoricalDaily] [Seq={seq}] [WriteItaly={writeItaly}] [WriteUtc={writeUtc:yyyy-MM-dd HH:mm:ss.fff}] [EventItaly={eventItaly:yyyy-MM-dd HH:mm:ss}] [EventLondon={MarketTimeZones.ToLondon(eventUtc):yyyy-MM-dd HH:mm:ss}] [EventUtc={eventUtc:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(path, line + Environment.NewLine);
            }
            catch
            {
            }
        }

        private void LogStudyBar(int bar, IndicatorCandle candle)
        {
            if (!_processingHistoricalPositions)
                return;

            if (!_studyLoggedBars.Add(bar))
                return;

            if (!IsInLondonSession(candle.Time))
                return;

            if (!ShouldDebugHistoricalDay(GetCandleEventTime(candle)))
                return;

            var snapshot = _historicalBarSnapshots.TryGetValue(bar, out var captured)
                ? captured
                : CreateHistoricalBarSnapshot(bar, candle);
            var candidateSetupState = GetBarSetupDiagnostics(snapshot);
            var activeSetupState = GetActiveSetupDiagnostics(snapshot.EventTimeUtc);
            StudyLog($"[DAY_STUDY_BAR] Bar={bar}, {FormatTime(snapshot.EventTimeUtc)}, Open={snapshot.Open:F2}, High={snapshot.High:F2}, Low={snapshot.Low:F2}, Close={snapshot.Close:F2}, Volume={snapshot.Volume:F0}, Bid={snapshot.Bid:F0}, Ask={snapshot.Ask:F0}, Delta={snapshot.Delta:F0}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, CloseLocation={snapshot.CloseLocation}, DistToPOC={snapshot.Close - snapshot.PreviewPOC:F2}, DistToVAH={snapshot.Close - snapshot.PreviewVAH:F2}, DistToVAL={snapshot.Close - snapshot.PreviewVAL:F2}, SetupDiagnostics={candidateSetupState}, ActiveSetupDiagnostics={activeSetupState}, TopLevels={snapshot.TopLevels}", snapshot.EventTimeUtc);
            LogBlockedSetupStudy(snapshot);
            LogPotentialPreviewRejection(snapshot);
            LogFollowThroughReclaimStudy(snapshot);
            LogDelayedReclaimSetupStudy(snapshot);
        }

        private void UpdateFollowThroughReclaimSetups(int bar, IndicatorCandle candle)
        {
            var eventTimeUtc = GetCandleEventTime(candle);
            if (!IsInLondonSession(eventTimeUtc))
                return;

            if (!_historicalBarSnapshots.TryGetValue(bar, out var snapshot))
                snapshot = CreateHistoricalBarSnapshot(bar, candle);

            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return;

            var day = DateOnly.FromDateTime(MarketTimeZones.ToItaly(snapshot.EventTimeUtc));
            var longKey = $"{day:yyyy-MM-dd}:Long";
            var shortKey = $"{day:yyyy-MM-dd}:Short";

            if (snapshot.Low < snapshot.PreviewVAL && snapshot.Close <= snapshot.PreviewVAL)
            {
                _followThroughSweepContexts[longKey] = new FollowThroughSweepContext(
                    "Long",
                    snapshot.Bar,
                    snapshot.EventTimeUtc,
                    snapshot.Low,
                    snapshot.High,
                    snapshot.PreviewPOC,
                    snapshot.PreviewVAH,
                    snapshot.PreviewVAL,
                    snapshot.Low - StopOffsetTicks * _tickSize);
            }
            else if (_followThroughSweepContexts.TryGetValue(longKey, out var lowSweep)
                && snapshot.Bar > lowSweep.SweepBar
                && snapshot.Low < snapshot.PreviewVAL
                && snapshot.Close > snapshot.PreviewVAL)
            {
                if (TryCreateFollowThroughReclaimSetup(snapshot, lowSweep, out var setup) && setup != null)
                    AddSetup(setup, "MR_FOLLOW_THROUGH_RECLAIM_CONTINUATION");

                _followThroughSweepContexts.Remove(longKey);
            }

            if (snapshot.High > snapshot.PreviewVAH && snapshot.Close >= snapshot.PreviewVAH)
            {
                _followThroughSweepContexts[shortKey] = new FollowThroughSweepContext(
                    "Short",
                    snapshot.Bar,
                    snapshot.EventTimeUtc,
                    snapshot.Low,
                    snapshot.High,
                    snapshot.PreviewPOC,
                    snapshot.PreviewVAH,
                    snapshot.PreviewVAL,
                    snapshot.High + StopOffsetTicks * _tickSize);
            }
            else if (_followThroughSweepContexts.TryGetValue(shortKey, out var highSweep)
                && snapshot.Bar > highSweep.SweepBar
                && snapshot.High > snapshot.PreviewVAH
                && snapshot.Close < snapshot.PreviewVAH)
            {
                if (TryCreateFollowThroughReclaimSetup(snapshot, highSweep, out var setup) && setup != null)
                    AddSetup(setup, "MR_FOLLOW_THROUGH_RECLAIM_CONTINUATION");

                _followThroughSweepContexts.Remove(shortKey);
            }
        }

        private bool TryCreateFollowThroughReclaimSetup(HistoricalBarSnapshot reclaimSnapshot, FollowThroughSweepContext sweep, out BalanceSetup? setup)
        {
            setup = null;
            var delta = sweep.Direction == "Long"
                ? reclaimSnapshot.Close - reclaimSnapshot.Low
                : reclaimSnapshot.High - reclaimSnapshot.Close;
            if (delta < RejectionThresholdTicks * _tickSize)
                return false;

            setup = new BalanceSetup
            {
                Direction = sweep.Direction,
                POC = reclaimSnapshot.PreviewPOC,
                VAH = reclaimSnapshot.PreviewVAH,
                VAL = reclaimSnapshot.PreviewVAL,
                BreakoutBar = sweep.SweepBar,
                BreakoutTimeUtc = sweep.SweepTimeUtc,
                BreakoutPrice = sweep.Direction == "Long" ? sweep.SweepLow : sweep.SweepHigh,
                RejectionBar = reclaimSnapshot.Bar,
                RejectionTimeUtc = reclaimSnapshot.EventTimeUtc,
                RejectionClose = reclaimSnapshot.Close,
                RejectionHigh = reclaimSnapshot.High,
                RejectionLow = reclaimSnapshot.Low,
                RejectionDelta = delta,
                StopPrice = sweep.StopPrice,
                TargetPrice = reclaimSnapshot.PreviewPOC,
                StudyFollowThroughConfirmed = true,
                StudyFollowThroughBar = reclaimSnapshot.Bar,
                StudyFollowThroughTimeUtc = reclaimSnapshot.EventTimeUtc,
                SetupSource = "FollowThroughReclaimContinuation"
            };
            return true;
        }

        private void LogBlockedSetupStudy(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return;

            var longBreakVal = snapshot.Low < snapshot.PreviewVAL;
            var longCloseBackInside = snapshot.Close > snapshot.PreviewVAL;
            var longRejectionTicks = (snapshot.Close - snapshot.Low) / _tickSize;
            var longReady = longBreakVal && longCloseBackInside && longRejectionTicks >= RejectionThresholdTicks;
            var isNewSessionLow = snapshot.Low < snapshot.PreviousSessionLow;

            if (longReady && !isNewSessionLow)
            {
                StudyLog($"[DAY_STUDY_SETUP_BLOCKED] Direction=Long, Bar={snapshot.Bar}, {FormatTime(snapshot.EventTimeUtc)}, Reason=NOT_NEW_SESSION_LOW, Low={snapshot.Low:F2}, Close={snapshot.Close:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, PreviousSessionLow={snapshot.PreviousSessionLow:F2}, RejectionTicks={longRejectionTicks:F1}, CloseLocation={snapshot.CloseLocation}", snapshot.EventTimeUtc);
            }

            var shortBreakVah = snapshot.High > snapshot.PreviewVAH;
            var shortCloseBackInside = snapshot.Close < snapshot.PreviewVAH;
            var shortRejectionTicks = (snapshot.High - snapshot.Close) / _tickSize;
            var shortReady = shortBreakVah && shortCloseBackInside && shortRejectionTicks >= RejectionThresholdTicks;
            var isNewSessionHigh = snapshot.High > snapshot.PreviousSessionHigh;

            if (shortReady && !isNewSessionHigh)
            {
                StudyLog($"[DAY_STUDY_SETUP_BLOCKED] Direction=Short, Bar={snapshot.Bar}, {FormatTime(snapshot.EventTimeUtc)}, Reason=NOT_NEW_SESSION_HIGH, High={snapshot.High:F2}, Close={snapshot.Close:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviousSessionHigh={snapshot.PreviousSessionHigh:F2}, RejectionTicks={shortRejectionTicks:F1}, CloseLocation={snapshot.CloseLocation}", snapshot.EventTimeUtc);
            }
        }

        private void LogFollowThroughReclaimStudy(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return;

            var recentLowSweep = _historicalBarSnapshots.Values
                .Where(s => s.Bar < snapshot.Bar)
                .Where(s => ShouldDebugHistoricalDay(s.EventTimeUtc))
                .Where(s => IsInLondonSession(s.EventTimeUtc))
                .Where(s => snapshot.EventTimeUtc > s.EventTimeUtc)
                .Where(s => s.Low < s.PreviewVAL && s.Close <= s.PreviewVAL)
                .OrderByDescending(s => s.EventTimeUtc)
                .FirstOrDefault();

            var longReclaimNow = snapshot.Low < snapshot.PreviewVAL && snapshot.Close > snapshot.PreviewVAL;
            if (recentLowSweep != null && longReclaimNow)
            {
                LogFollowThroughReclaimStudy(snapshot, recentLowSweep, "Long");
            }

            var recentHighSweep = _historicalBarSnapshots.Values
                .Where(s => s.Bar < snapshot.Bar)
                .Where(s => ShouldDebugHistoricalDay(s.EventTimeUtc))
                .Where(s => IsInLondonSession(s.EventTimeUtc))
                .Where(s => snapshot.EventTimeUtc > s.EventTimeUtc)
                .Where(s => s.High > s.PreviewVAH && s.Close >= s.PreviewVAH)
                .OrderByDescending(s => s.EventTimeUtc)
                .FirstOrDefault();

            var shortReclaimNow = snapshot.High > snapshot.PreviewVAH && snapshot.Close < snapshot.PreviewVAH;
            if (recentHighSweep != null && shortReclaimNow)
            {
                LogFollowThroughReclaimStudy(snapshot, recentHighSweep, "Short");
            }
        }

        private void LogFollowThroughReclaimStudy(HistoricalBarSnapshot reclaimSnapshot, HistoricalBarSnapshot sweepSnapshot, string direction)
        {
            var setup = new BalanceSetup
            {
                SetupId = $"FOLLOWTHROUGH-{sweepSnapshot.Bar}-{reclaimSnapshot.Bar}-{direction}",
                Direction = direction,
                POC = reclaimSnapshot.PreviewPOC,
                VAH = reclaimSnapshot.PreviewVAH,
                VAL = reclaimSnapshot.PreviewVAL,
                BreakoutBar = sweepSnapshot.Bar,
                BreakoutTimeUtc = sweepSnapshot.EventTimeUtc,
                BreakoutPrice = direction == "Long" ? sweepSnapshot.Low : sweepSnapshot.High,
                RejectionBar = reclaimSnapshot.Bar,
                RejectionTimeUtc = reclaimSnapshot.EventTimeUtc,
                RejectionClose = reclaimSnapshot.Close,
                RejectionHigh = reclaimSnapshot.High,
                RejectionLow = reclaimSnapshot.Low,
                RejectionDelta = direction == "Long" ? reclaimSnapshot.Close - reclaimSnapshot.Low : reclaimSnapshot.High - reclaimSnapshot.Close,
                StopPrice = direction == "Long" ? sweepSnapshot.Low - StopOffsetTicks * _tickSize : sweepSnapshot.High + StopOffsetTicks * _tickSize,
                TargetPrice = reclaimSnapshot.PreviewPOC,
                SetupSource = "FollowThroughReclaimStudy"
            };

            var candidates = _lastHistoricalTrades
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                .ToList();

            var zoneCandidates = candidates.Where(t => IsInEntryZone(setup, t)).ToList();
            var continuationCandidates = candidates.Where(t => IsBeyondPocContinuationEntry(setup, t)).ToList();
            var validCandidates = zoneCandidates
                .Where(t => (t.Time - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds)
                .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                .ToList();
            var continuationValidCandidates = continuationCandidates
                .Where(t => (t.Time - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds)
                .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                .ToList();
            var firstValid = validCandidates.OrderBy(t => t.Time).FirstOrDefault();
            var firstContinuationValid = continuationValidCandidates.OrderBy(t => t.Time).FirstOrDefault();
            var firstValidText = firstValid == null
                ? "NONE"
                : $"{FormatTime(firstValid.Time)}:Price={firstValid.Lastprice:F2}:Volume={firstValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstValid.Lastprice):F2}:Age={(firstValid.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";
            var firstContinuationValidText = firstContinuationValid == null
                ? "NONE"
                : $"{FormatTime(firstContinuationValid.Time)}:Price={firstContinuationValid.Lastprice:F2}:Volume={firstContinuationValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstContinuationValid.Lastprice):F2}:Age={(firstContinuationValid.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";

            var outcomeText = "NA";
            if (firstValid != null)
            {
                var stop = GetOperationalStopPrice(setup, firstValid.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(direction, firstValid.Lastprice, stop, setup.TargetPrice, GetStudyTarget2(setup), firstValid.Time);
                outcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
            }

            var continuationOutcomeText = "NA";
            if (firstContinuationValid != null)
            {
                var stop = GetOperationalStopPrice(setup, firstContinuationValid.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(direction, firstContinuationValid.Lastprice, stop, setup.TargetPrice, GetStudyTarget2(setup), firstContinuationValid.Time);
                continuationOutcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
            }

            StudyLog($"[DAY_STUDY_FOLLOWTHROUGH_RECLAIM] Direction={direction}, SweepBar={sweepSnapshot.Bar}, ReclaimBar={reclaimSnapshot.Bar}, SweepTime={FormatTime(sweepSnapshot.EventTimeUtc)}, ReclaimTime={FormatTime(reclaimSnapshot.EventTimeUtc)}, SweepLow={sweepSnapshot.Low:F2}, SweepHigh={sweepSnapshot.High:F2}, ReclaimClose={reclaimSnapshot.Close:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, ReclaimTicks={setup.RejectionDelta / _tickSize:F1}, SameDirectionTrades={candidates.Count}, InEntryZone={zoneCandidates.Count}, Valid={validCandidates.Count}, FirstValid={firstValidText}, Outcome={outcomeText}, ContinuationZone={continuationCandidates.Count}, ContinuationValid={continuationValidCandidates.Count}, FirstContinuationValid={firstContinuationValidText}, ContinuationOutcome={continuationOutcomeText}", reclaimSnapshot.EventTimeUtc);
        }

        private void LogDelayedReclaimSetupStudy(HistoricalBarSnapshot snapshot)
        {
            foreach (var direction in GetDelayedReclaimDirections(snapshot))
                LogDelayedReclaimSetupStudy(snapshot, direction);
        }

        private void LogDelayedReclaimSetupStudy(HistoricalBarSnapshot snapshot, string direction)
        {
            var setup = CreateDelayedReclaimSetup(snapshot, direction, "DelayedReclaimStudy");
            if (setup == null)
                return;

            var excursionLow = setup.Direction == "Long" ? setup.BreakoutPrice : _historicalBarSnapshots.Values.Where(s => s.Bar >= snapshot.Bar - 6 && s.Bar <= snapshot.Bar).Max(s => s.Low);
            var excursionHigh = setup.Direction == "Short" ? setup.BreakoutPrice : _historicalBarSnapshots.Values.Where(s => s.Bar >= snapshot.Bar - 6 && s.Bar <= snapshot.Bar).Max(s => s.High);
            var stop = setup.StopPrice;

            var expectedDirection = direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var candidates = _lastHistoricalTrades
                .Where(t => t.Time > snapshot.EventTimeUtc && t.Time <= snapshot.EventTimeUtc.AddSeconds(OperationalEntryTimeoutSeconds))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Direction == expectedDirection)
                .Where(t => IsDelayedReclaimEntryZone(setup, t))
                .OrderBy(t => t.Time)
                .ToList();

            var validCandidates = candidates
                .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                .ToList();
            var firstValid = validCandidates.FirstOrDefault();
            var firstValidText = firstValid == null
                ? "NONE"
                : $"{FormatTime(firstValid.Time)}:Price={firstValid.Lastprice:F2}:Volume={firstValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstValid.Lastprice):F2}:Age={(firstValid.Time - snapshot.EventTimeUtc).TotalSeconds:F1}s";

            var outcomeText = "NA";
            if (firstValid != null)
            {
                var operationalStop = GetOperationalStopPrice(setup, firstValid.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(direction, firstValid.Lastprice, operationalStop, setup.TargetPrice, GetStudyTarget2(setup), firstValid.Time);
                outcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
            }

            var activeSetupState = GetActiveSetupDiagnostics(snapshot.EventTimeUtc);
            StudyLog($"[DAY_STUDY_DELAYED_RECLAIM_SETUP] Bar={snapshot.Bar}, Direction={direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, Close={snapshot.Close:F2}, Delta={snapshot.Delta:F0}, ExcursionLow={excursionLow:F2}, ExcursionHigh={excursionHigh:F2}, Stop={stop:F2}, ActiveSetupDiagnostics={activeSetupState}, CandidateCount={candidates.Count}, Valid={validCandidates.Count}, FirstValid={firstValidText}, Outcome={outcomeText}", snapshot.EventTimeUtc);
            LogDelayedReclaimNarrativeStudy(snapshot, setup, validCandidates);
        }

        private void LogDelayedReclaimNarrativeStudy(HistoricalBarSnapshot snapshot, BalanceSetup setup, List<CumulativeTrade> validCandidates)
        {
            var pre = GetDelayedReclaimNarrativeStats(snapshot.EventTimeUtc.AddMinutes(-15), snapshot.EventTimeUtc, setup.Direction);
            var post = GetDelayedReclaimNarrativeStats(snapshot.EventTimeUtc, snapshot.EventTimeUtc.AddMinutes(15), setup.Direction);
            var nextBarHolds = IsDelayedReclaimHeldByNextBar(snapshot, setup.Direction);
            var acceptedInsideValue = CountAcceptedBarsAfterReclaim(snapshot, setup.Direction, bars: 3);
            var pressureCandidate = FindNarrativePressureCandidate(snapshot, setup, validCandidates);
            var outcomeText = "NA";
            var candidateText = "NONE";
            if (pressureCandidate != null)
            {
                var stop = GetOperationalStopPrice(setup, pressureCandidate.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(setup.Direction, pressureCandidate.Lastprice, stop, setup.TargetPrice, GetStudyTarget2(setup), pressureCandidate.Time);
                candidateText = $"{FormatTime(pressureCandidate.Time)}:Price={pressureCandidate.Lastprice:F2}:Volume={pressureCandidate.Volume:F0}:RR={GetRewardRiskToTarget2(setup, pressureCandidate.Lastprice):F2}:Age={(pressureCandidate.Time - snapshot.EventTimeUtc).TotalSeconds:F1}s";
                outcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
            }

            var narrativeAccepted = acceptedInsideValue >= 2
                && post.NetVolume > 0
                && post.MaxBubbleSide == "SAME"
                && pressureCandidate != null;

            StudyLog($"[DAY_STUDY_DELAYED_RECLAIM_NARRATIVE] Bar={snapshot.Bar}, Direction={setup.Direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={setup.POC:F2}, PreviewVAH={setup.VAH:F2}, PreviewVAL={setup.VAL:F2}, NextBarHolds={nextBarHolds}, AcceptedBarsNext3={acceptedInsideValue}, PreSameVolume15m={pre.SameDirectionVolume:F0}, PreOppositeVolume15m={pre.OppositeDirectionVolume:F0}, PreNetVolume15m={pre.NetVolume:F0}, PostSameVolume15m={post.SameDirectionVolume:F0}, PostOppositeVolume15m={post.OppositeDirectionVolume:F0}, PostNetVolume15m={post.NetVolume:F0}, PressureShift={post.PressureShift - pre.PressureShift:F2}, PostMaxBubbleSide={post.MaxBubbleSide}, PostMaxBubbleVolume={post.MaxBubbleVolume:F0}, NarrativeAccepted={narrativeAccepted}, Valid={validCandidates.Count}, NarrativeCandidate={candidateText}, Outcome={outcomeText}", snapshot.EventTimeUtc);

            if (narrativeAccepted)
                StudyLog($"[DAY_STUDY_DELAYED_RECLAIM_ACCEPTED] Bar={snapshot.Bar}, Direction={setup.Direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={setup.POC:F2}, PreviewVAH={setup.VAH:F2}, PreviewVAL={setup.VAL:F2}, AcceptedBarsNext3={acceptedInsideValue}, PreNetVolume15m={pre.NetVolume:F0}, PostNetVolume15m={post.NetVolume:F0}, PressureShift={post.PressureShift - pre.PressureShift:F2}, PostMaxBubbleSide={post.MaxBubbleSide}, PostMaxBubbleVolume={post.MaxBubbleVolume:F0}, NarrativeCandidate={candidateText}, Outcome={outcomeText}", snapshot.EventTimeUtc);
        }

        private bool IsDelayedReclaimHeldByNextBar(HistoricalBarSnapshot snapshot, string direction)
        {
            if (!_historicalBarSnapshots.TryGetValue(snapshot.Bar + 1, out var next))
                return false;

            return direction == "Long"
                ? next.Close >= snapshot.PreviewVAL
                : next.Close <= snapshot.PreviewVAH;
        }

        private int CountAcceptedBarsAfterReclaim(HistoricalBarSnapshot snapshot, string direction, int bars)
        {
            var count = 0;
            for (var i = 1; i <= bars; i++)
            {
                if (!_historicalBarSnapshots.TryGetValue(snapshot.Bar + i, out var next))
                    break;

                var accepted = direction == "Long"
                    ? next.Close >= snapshot.PreviewVAL
                    : next.Close <= snapshot.PreviewVAH;
                if (accepted)
                    count++;
            }

            return count;
        }

        private DelayedReclaimNarrativeStats GetDelayedReclaimNarrativeStats(DateTime beginUtc, DateTime endUtc, string direction)
        {
            var expectedDirection = direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var trades = _lastHistoricalTrades
                .Where(t => t.Time > beginUtc && t.Time <= endUtc)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .ToList();
            var same = trades.Where(t => t.Direction == expectedDirection).ToList();
            var opposite = trades.Where(t => t.Direction == oppositeDirection).ToList();
            var maxSame = same.Count == 0 ? 0 : same.Max(t => t.Volume);
            var maxOpposite = opposite.Count == 0 ? 0 : opposite.Max(t => t.Volume);
            var sameVolume = same.Sum(t => t.Volume);
            var oppositeVolume = opposite.Sum(t => t.Volume);
            var maxSide = maxSame == 0 && maxOpposite == 0
                ? "NONE"
                : maxSame >= maxOpposite ? "SAME" : "OPPOSITE";
            var maxVolume = Math.Max(maxSame, maxOpposite);
            var netVolume = sameVolume - oppositeVolume;
            var pressureShift = (sameVolume + oppositeVolume) <= 0
                ? 0
                : netVolume / (sameVolume + oppositeVolume);

            return new DelayedReclaimNarrativeStats(
                same.Count,
                sameVolume,
                maxSame,
                opposite.Count,
                oppositeVolume,
                maxOpposite,
                maxSide,
                maxVolume,
                netVolume,
                pressureShift);
        }

        private CumulativeTrade? FindNarrativePressureCandidate(HistoricalBarSnapshot snapshot, BalanceSetup setup, List<CumulativeTrade> validCandidates)
        {
            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = setup.Direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var validByTime = validCandidates.OrderBy(t => t.Time).ToList();
            if (validByTime.Count == 0)
                return null;

            var sameVolume = 0m;
            var oppositeVolume = 0m;
            foreach (var trade in _lastHistoricalTrades
                .Where(t => t.Time > snapshot.EventTimeUtc && t.Time <= snapshot.EventTimeUtc.AddMinutes(15))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .OrderBy(t => t.Time))
            {
                if (trade.Direction == expectedDirection)
                    sameVolume += trade.Volume;
                else if (trade.Direction == oppositeDirection)
                    oppositeVolume += trade.Volume;

                var sameHasControl = sameVolume > 0 && sameVolume >= oppositeVolume;
                if (!sameHasControl)
                    continue;

                var candidate = validByTime.FirstOrDefault(t => t.Time >= trade.Time);
                if (candidate != null)
                    return candidate;
            }

            return null;
        }

        private IEnumerable<string> GetDelayedReclaimDirections(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                yield break;

            if (!_historicalBarSnapshots.TryGetValue(snapshot.Bar - 1, out var previous))
                yield break;

            if (previous.PreviewPOC == 0 || previous.PreviewVAH == 0 || previous.PreviewVAL == 0)
                yield break;

            if (previous.Close < previous.PreviewVAL && snapshot.Close > snapshot.PreviewVAL)
                yield return "Long";

            if (previous.Close > previous.PreviewVAH && snapshot.Close < snapshot.PreviewVAH)
                yield return "Short";
        }

        private BalanceSetup? CreateDelayedReclaimSetup(HistoricalBarSnapshot snapshot, string direction, string source)
        {
            var lookbackBars = _historicalBarSnapshots.Values
                .Where(s => s.Bar >= snapshot.Bar - 6 && s.Bar <= snapshot.Bar)
                .OrderBy(s => s.Bar)
                .ToList();
            if (lookbackBars.Count == 0)
                return null;

            var excursionLow = lookbackBars.Min(s => s.Low);
            var excursionHigh = lookbackBars.Max(s => s.High);
            var stop = direction == "Long"
                ? excursionLow - StopOffsetTicks * _tickSize
                : excursionHigh + StopOffsetTicks * _tickSize;

            return new BalanceSetup
            {
                SetupId = $"DELAYED-RECLAIM-{snapshot.Bar}-{direction}",
                Direction = direction,
                POC = snapshot.PreviewPOC,
                VAH = snapshot.PreviewVAH,
                VAL = snapshot.PreviewVAL,
                BreakoutBar = snapshot.Bar,
                BreakoutTimeUtc = snapshot.EventTimeUtc,
                BreakoutPrice = direction == "Long" ? excursionLow : excursionHigh,
                RejectionBar = snapshot.Bar,
                RejectionTimeUtc = snapshot.EventTimeUtc,
                RejectionClose = snapshot.Close,
                RejectionHigh = snapshot.High,
                RejectionLow = snapshot.Low,
                RejectionDelta = snapshot.Delta,
                StopPrice = stop,
                TargetPrice = snapshot.PreviewPOC,
                SetupSource = source
            };
        }

        private void UpdateDelayedReclaimCandidates(int bar, IndicatorCandle candle)
        {
            if (!IsInLondonSession(candle.Time))
                return;

            var snapshot = _historicalBarSnapshots.TryGetValue(bar, out var captured)
                ? captured
                : CreateHistoricalBarSnapshot(bar, candle);

            foreach (var direction in GetDelayedReclaimDirections(snapshot))
            {
                var setup = CreateDelayedReclaimSetup(snapshot, direction, "DelayedReclaimAccepted");
                if (setup == null)
                    continue;

                if (_delayedReclaimCandidates.Any(c => c.Setup.SetupId == setup.SetupId))
                    continue;

                _delayedReclaimCandidates.Add(new DelayedReclaimCandidate
                {
                    Setup = setup,
                    LastUpdatedUtc = snapshot.EventTimeUtc
                });
                _log($"[MR_DELAYED_RECLAIM_SETUP] SetupId={setup.SetupId}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", IsHistoricalBar(bar));
            }

            foreach (var candidate in _delayedReclaimCandidates.Where(c => !c.EntryConfirmed && !c.Setup.Expired).ToList())
                UpdateDelayedReclaimCandidateAcceptance(candidate, snapshot);
        }

        private void UpdateDelayedReclaimCandidateAcceptance(DelayedReclaimCandidate candidate, HistoricalBarSnapshot snapshot)
        {
            var setup = candidate.Setup;
            if (snapshot.EventTimeUtc <= setup.RejectionTimeUtc)
                return;

            if (!_processingHistoricalPositions && snapshot.EventTimeUtc > setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
            {
                setup.Expired = true;
                return;
            }

            var accepted = setup.Direction == "Long"
                ? snapshot.Close >= setup.VAL
                : snapshot.Close <= setup.VAH;
            if (accepted)
                candidate.AcceptedBars++;

            candidate.OperationallyReady = candidate.AcceptedBars >= 2;
            candidate.LastUpdatedUtc = snapshot.EventTimeUtc;
        }

        private bool TryProcessDelayedReclaimEntry(CumulativeTrade trade, string entryModel, bool isHistorical)
        {
            foreach (var candidate in _delayedReclaimCandidates.Where(c => !c.EntryConfirmed && !c.Setup.Expired).ToList())
            {
                var setup = candidate.Setup;
                if (!IsDelayedReclaimTradeCandidate(candidate, trade, out var acceptedBars, out var sameVolume, out var oppositeVolume, out var maxSameVolume, out var maxOppositeVolume, out var acceptanceMode))
                    continue;

                candidate.EntryConfirmed = true;
                setup.AggressionConfirmed = true;
                var resolvedEntryModel = isHistorical
                    ? "FootprintCumulativeTradeHistoricalDelayedReclaim"
                    : "FootprintCumulativeTradeLiveDelayedReclaim";
                if (CreatePosition(setup, trade, resolvedEntryModel, isHistorical))
                    _log($"[MR_DELAYED_RECLAIM_ENTRY] SetupId={setup.SetupId}, EntryModel={resolvedEntryModel}, Direction={setup.Direction}, {FormatTime(trade.Time)}, EntryPrice={trade.Lastprice:F2}, Volume={trade.Volume:F0}, AcceptedBars={acceptedBars}, AcceptanceMode={acceptanceMode}, SameDirectionVolume={sameVolume:F0}, OppositeDirectionVolume={oppositeVolume:F0}, MaxSameDirectionVolume={maxSameVolume:F0}, MaxOppositeDirectionVolume={maxOppositeVolume:F0}", isHistorical);
                return true;
            }

            return false;
        }

        private bool IsDelayedReclaimTradeCandidate(
            DelayedReclaimCandidate candidate,
            CumulativeTrade trade,
            out int acceptedBars,
            out decimal sameDirectionVolume,
            out decimal oppositeDirectionVolume,
            out decimal maxSameDirectionVolume,
            out decimal maxOppositeDirectionVolume,
            out string acceptanceMode)
        {
            acceptedBars = 0;
            sameDirectionVolume = 0;
            oppositeDirectionVolume = 0;
            maxSameDirectionVolume = 0;
            maxOppositeDirectionVolume = 0;
            acceptanceMode = "NONE";

            var setup = candidate.Setup;
            if (trade.Time <= setup.RejectionTimeUtc || trade.Time > setup.RejectionTimeUtc.AddSeconds(OperationalEntryTimeoutSeconds))
                return false;

            if (!IsLondonTradeAllowed(trade.Time) || trade.Volume < MinAggressionVolume)
                return false;

            acceptedBars = CountAcceptedBarsBeforeTime(setup, trade.Time, 3);

            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction != expectedDirection)
                return false;

            if (trade.Volume < MinAggressionVolume * DelayedReclaimNarrativeMinBubbleMultiplier)
                return false;

            if (!IsDelayedReclaimEntryZone(setup, trade))
                return false;

            GetDelayedReclaimPressureBeforeTime(candidate, trade, out sameDirectionVolume, out oppositeDirectionVolume, out maxSameDirectionVolume, out maxOppositeDirectionVolume);
            if (sameDirectionVolume <= oppositeDirectionVolume)
                return false;

            if (maxSameDirectionVolume < maxOppositeDirectionVolume)
                return false;

            if (GetOperationalRisk(setup, trade.Lastprice) > DelayedReclaimMaxOperationalRiskPoints)
                return false;

            if (!IsDelayedReclaimCausallyAccepted(setup, trade, acceptedBars, sameDirectionVolume, oppositeDirectionVolume, maxSameDirectionVolume, maxOppositeDirectionVolume, out acceptanceMode))
                return false;

            return GetRewardRiskToTarget2(setup, trade.Lastprice) >= MinRewardRiskToTarget2;
        }

        private bool IsDelayedReclaimCausallyAccepted(
            BalanceSetup setup,
            CumulativeTrade trade,
            int acceptedBars,
            decimal sameDirectionVolume,
            decimal oppositeDirectionVolume,
            decimal maxSameDirectionVolume,
            decimal maxOppositeDirectionVolume,
            out string acceptanceMode)
        {
            acceptanceMode = "NONE";
            var ageSeconds = (trade.Time - setup.RejectionTimeUtc).TotalSeconds;
            var dominantBubble = maxSameDirectionVolume >= maxOppositeDirectionVolume * DelayedReclaimDominantBubbleMultiplier;
            var strongEarlyPressure = oppositeDirectionVolume <= 0
                || sameDirectionVolume >= oppositeDirectionVolume * DelayedReclaimEarlyPressureVolumeRatio;

            if (acceptedBars >= DelayedReclaimMinAcceptedBars)
            {
                acceptanceMode = "CONFIRMED_ACCEPTANCE";
                return true;
            }

            if (ageSeconds <= DelayedReclaimImmediateMaxSeconds && dominantBubble)
            {
                acceptanceMode = "IMMEDIATE_DOMINANT_PRESSURE";
                return true;
            }

            if (acceptedBars >= 1 && dominantBubble && strongEarlyPressure)
            {
                acceptanceMode = "EARLY_ACCEPTED_PRESSURE";
                return true;
            }

            return false;
        }

        private int CountAcceptedBarsBeforeTime(BalanceSetup setup, DateTime tradeTimeUtc, int maxBars)
        {
            var accepted = 0;
            foreach (var snapshot in _historicalBarSnapshots.Values
                .Where(s => s.EventTimeUtc > setup.RejectionTimeUtc && s.EventTimeUtc < tradeTimeUtc)
                .OrderBy(s => s.Bar)
                .Take(maxBars))
            {
                var inside = setup.Direction == "Long"
                    ? snapshot.Close >= setup.VAL
                    : snapshot.Close <= setup.VAH;
                if (inside)
                    accepted++;
            }

            return accepted;
        }

        private void GetDelayedReclaimPressureBeforeTime(
            DelayedReclaimCandidate candidate,
            CumulativeTrade trade,
            out decimal sameDirectionVolume,
            out decimal oppositeDirectionVolume,
            out decimal maxSameDirectionVolume,
            out decimal maxOppositeDirectionVolume)
        {
            sameDirectionVolume = 0;
            oppositeDirectionVolume = 0;
            maxSameDirectionVolume = 0;
            maxOppositeDirectionVolume = 0;
            var setup = candidate.Setup;
            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = setup.Direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var source = _lastHistoricalTrades.Count > 0
                ? _lastHistoricalTrades.Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= trade.Time && t.Volume >= MinAggressionVolume)
                : new[] { trade }.Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= trade.Time && t.Volume >= MinAggressionVolume);

            foreach (var t in source)
            {
                if (t.Direction == expectedDirection)
                {
                    sameDirectionVolume += t.Volume;
                    maxSameDirectionVolume = Math.Max(maxSameDirectionVolume, t.Volume);
                }
                else if (t.Direction == oppositeDirection)
                {
                    oppositeDirectionVolume += t.Volume;
                    maxOppositeDirectionVolume = Math.Max(maxOppositeDirectionVolume, t.Volume);
                }
            }
        }

        private bool IsDelayedReclaimEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            var price = trade.Lastprice;
            return setup.Direction == "Long"
                ? price >= setup.VAL && price <= setup.POC
                : price <= setup.VAH && price >= setup.POC;
        }

        private void LogPotentialPreviewRejection(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return;

            var shortRejection = snapshot.High > snapshot.PreviewVAH && snapshot.Close < snapshot.PreviewVAH;
            var longRejection = snapshot.Low < snapshot.PreviewVAL && snapshot.Close > snapshot.PreviewVAL;
            if (!shortRejection && !longRejection)
                return;

            var shortRejectionDistance = snapshot.High - snapshot.Close;
            var longRejectionDistance = snapshot.Close - snapshot.Low;
            var matchingSetup = _activeSetups
                .Where(s => s.RejectionBar == snapshot.Bar)
                .Select(s => $"{s.SetupId[..Math.Min(8, s.SetupId.Length)]}:{s.SetupSource}:{s.Direction}")
                .ToList();

            var matchingSetupLabel = matchingSetup.Count == 0 ? "NONE" : string.Join("|", matchingSetup);
            StudyLog($"[DAY_STUDY_POTENTIAL_PREVIEW_REJECTION] Bar={snapshot.Bar}, {FormatTime(snapshot.EventTimeUtc)}, ShortRejection={shortRejection}, LongRejection={longRejection}, High={snapshot.High:F2}, Low={snapshot.Low:F2}, Close={snapshot.Close:F2}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, ShortRejectionTicks={shortRejectionDistance / _tickSize:F1}, LongRejectionTicks={longRejectionDistance / _tickSize:F1}, MatchingSetup={matchingSetupLabel}, CloseLocation={snapshot.CloseLocation}, Delta={snapshot.Delta:F0}, TopLevels={snapshot.TopLevels}", snapshot.EventTimeUtc);

            if (matchingSetup.Count == 0)
                LogPotentialPreviewRejectionOutcome(snapshot, shortRejection, longRejection);
        }

        private void LogPotentialPreviewRejectionOutcome(HistoricalBarSnapshot snapshot, bool shortRejection, bool longRejection)
        {
            if (shortRejection)
                LogPotentialPreviewRejectionOutcome(snapshot, "Short");

            if (longRejection)
                LogPotentialPreviewRejectionOutcome(snapshot, "Long");
        }

        private void LogPotentialPreviewRejectionOutcome(HistoricalBarSnapshot snapshot, string direction)
        {
            var setup = new BalanceSetup
            {
                SetupId = $"PREVIEW-{snapshot.Bar}-{direction}",
                Direction = direction,
                POC = snapshot.PreviewPOC,
                VAH = snapshot.PreviewVAH,
                VAL = snapshot.PreviewVAL,
                BreakoutBar = snapshot.Bar,
                BreakoutTimeUtc = snapshot.EventTimeUtc,
                BreakoutPrice = direction == "Long" ? snapshot.Low : snapshot.High,
                RejectionBar = snapshot.Bar,
                RejectionTimeUtc = snapshot.EventTimeUtc,
                RejectionClose = snapshot.Close,
                RejectionHigh = snapshot.High,
                RejectionLow = snapshot.Low,
                RejectionDelta = direction == "Long" ? snapshot.Close - snapshot.Low : snapshot.High - snapshot.Close,
                StopPrice = direction == "Long" ? snapshot.Low - StopOffsetTicks * _tickSize : snapshot.High + StopOffsetTicks * _tickSize,
                TargetPrice = snapshot.PreviewPOC,
                SetupSource = "PreviewRejectionStudy"
            };

            var candidates = _lastHistoricalTrades
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                .ToList();

            var zoneCandidates = candidates.Where(t => IsInEntryZone(setup, t)).ToList();
            var validCandidates = zoneCandidates
                .Where(t => (t.Time - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds)
                .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                .ToList();
            var firstValid = validCandidates.OrderBy(t => t.Time).FirstOrDefault();
            var firstValidText = firstValid == null
                ? "NONE"
                : $"{FormatTime(firstValid.Time)}:Price={firstValid.Lastprice:F2}:Volume={firstValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstValid.Lastprice):F2}:Age={(firstValid.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";

            var outcomeText = "NA";
            if (firstValid != null)
            {
                var stop = GetOperationalStopPrice(setup, firstValid.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(direction, firstValid.Lastprice, stop, setup.TargetPrice, GetStudyTarget2(setup), firstValid.Time);
                outcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
                LogPreviewRejectionContinuationStudy(snapshot, setup, firstValid, stop);
            }

            StudyLog($"[DAY_STUDY_PREVIEW_REJECTION_OUTCOME] Bar={snapshot.Bar}, Direction={direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, Stop={setup.StopPrice:F2}, RejectionDelta={setup.RejectionDelta:F2}, SameDirectionTrades={candidates.Count}, InEntryZone={zoneCandidates.Count}, Valid={validCandidates.Count}, FirstValid={firstValidText}, Outcome={outcomeText}", snapshot.EventTimeUtc);
        }

        private void LogPreviewRejectionContinuationStudy(HistoricalBarSnapshot snapshot, BalanceSetup setup, CumulativeTrade firstValid, decimal stop)
        {
            var target2 = GetStudyTarget2(setup);
            var protectedOutcome = EvaluateProtectedTarget2OutcomeFromTrades(setup.Direction, firstValid.Lastprice, stop, setup.TargetPrice, target2, firstValid.Time);
            var holdOutcome = EvaluateHoldToTarget2OutcomeFromBars(setup.Direction, firstValid.Lastprice, stop, target2, firstValid.Time, snapshot.Bar);
            var postPocContinuation = CountPostPocContinuationTrades(setup, firstValid.Time);
            StudyLog($"[DAY_STUDY_PREVIEW_CONTINUATION_STUDY] Bar={snapshot.Bar}, Direction={setup.Direction}, {FormatTime(snapshot.EventTimeUtc)}, EntryTime={FormatTime(firstValid.Time)}, EntryPrice={firstValid.Lastprice:F2}, Stop={stop:F2}, POC={setup.POC:F2}, Target2={target2:F2}, EntryVolume={firstValid.Volume:F0}, ProtectedExit={protectedOutcome.ExitReason}, ProtectedPnL={protectedOutcome.Pnl:F2}, ProtectedR={protectedOutcome.RMultiple:F2}, HoldExit={holdOutcome.ExitReason}, HoldPnL={holdOutcome.Pnl:F2}, HoldR={holdOutcome.RMultiple:F2}, PostPocSameDirectionTrades={postPocContinuation.Count}, PostPocSameDirectionVolume={postPocContinuation.Volume:F0}, FirstPostPocTrade={postPocContinuation.FirstTrade}", snapshot.EventTimeUtc);
        }

        private void LogDailySetupCandidateSummaries(List<CumulativeTrade> trades)
        {
            foreach (var setup in _activeSetups.Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc)).OrderBy(s => s.RejectionTimeUtc))
            {
                var windowTrades = trades
                    .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                    .Where(t => IsLondonTradeAllowed(t.Time))
                    .ToList();

                var sameDirection = 0;
                var wrongDirection = 0;
                var inEntryZone = 0;
                var valid = 0;
                var rrTooLow = 0;
                var stale = 0;
                CumulativeTrade? firstValid = null;
                CumulativeTrade? firstSameDirection = null;

                foreach (var trade in windowTrades)
                {
                    var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
                    if (trade.Direction != expectedDirection)
                    {
                        wrongDirection++;
                        continue;
                    }

                    sameDirection++;
                    firstSameDirection ??= trade;
                    if (!IsInEntryZone(setup, trade))
                        continue;

                    inEntryZone++;
                    var ageSeconds = (trade.Time - setup.RejectionTimeUtc).TotalSeconds;
                    if (ageSeconds > OperationalEntryTimeoutSeconds)
                    {
                        stale++;
                        continue;
                    }

                    var rr = GetRewardRiskToTarget2(setup, trade.Lastprice);
                    if (rr < MinRewardRiskToTarget2)
                    {
                        rrTooLow++;
                        continue;
                    }

                    valid++;
                    firstValid ??= trade;
                }

                var firstValidText = firstValid == null
                    ? "NONE"
                    : $"{FormatTime(firstValid.Time)}:Price={firstValid.Lastprice:F2}:Volume={firstValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstValid.Lastprice):F2}:Age={(firstValid.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";
                var firstSameDirectionText = firstSameDirection == null
                    ? "NONE"
                    : $"{FormatTime(firstSameDirection.Time)}:Price={firstSameDirection.Lastprice:F2}:Volume={firstSameDirection.Volume:F0}:InZone={IsInEntryZone(setup, firstSameDirection)}:RR={GetRewardRiskToTarget2(setup, firstSameDirection.Lastprice):F2}:Age={(firstSameDirection.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";

                StudyLog($"[DAY_STUDY_SETUP_CANDIDATE_SUMMARY] SetupId={setup.SetupId}, Source={setup.SetupSource}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, AggressionConfirmed={setup.AggressionConfirmed}, Expired={setup.Expired}, WindowTrades={windowTrades.Count}, SameDirection={sameDirection}, WrongDirection={wrongDirection}, InEntryZone={inEntryZone}, Valid={valid}, RRTooLow={rrTooLow}, Stale={stale}, FirstSameDirection={firstSameDirectionText}, FirstValid={firstValidText}", setup.RejectionTimeUtc);
            }
        }

        private void LogStudyCumulativeTrades(List<CumulativeTrade> trades)
        {
            foreach (var trade in trades.Where(t => t.Volume >= MinAggressionVolume && IsInLondonSession(t.Time) && ShouldDebugHistoricalDay(t.Time)))
            {
                var nearestSetup = FindNearestStudySetup(trade);
                var candidateDiagnostics = GetTradeCandidateDiagnostics(trade);
                StudyLog($"[DAY_STUDY_BIG_TRADE] {FormatTime(trade.Time)}, Direction={trade.Direction}, Volume={trade.Volume:F0}, FirstPrice={trade.FirstPrice:F2}, LastPrice={trade.Lastprice:F2}, TickCount={trade.Ticks.Count}, Location={GetTradeLocation(trade.Lastprice)}, NearestSetupId={nearestSetup.SetupId}, NearestSetupDirection={nearestSetup.Direction}, SecondsAfterSetup={nearestSetup.SecondsAfter:F1}, SameDirectionAsSetup={nearestSetup.SameDirection}, InEntryZone={nearestSetup.InEntryZone}, ContinuationZone={nearestSetup.ContinuationZone}, CandidateDiagnostics={candidateDiagnostics}", trade.Time);
            }
        }

        private void RunDayStudy()
        {
            if (_dayStudyCompleted || _lastHistoricalTrades.Count == 0)
                return;

            var studySetups = _activeSetups.Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc)).ToList();

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
                    var triggerAtEntry = GetStudyTriggerLabelAtTime(setup, candidate.EntryTimeUtc);
                    var outcome = EvaluateCandidateOutcome(candidate.Direction, candidate.EntryPrice, candidate.Stop, candidate.TargetPoc, candidate.Target2, candidate.EntryTimeUtc, setup.RejectionBar);
                    StudyLog($"[DAY_STUDY_CANDIDATE_ENTRY] SetupId={setup.SetupId}, CandidateType={candidate.CandidateType}, Trigger={trigger}, TriggerAtEntry={triggerAtEntry}, Direction={candidate.Direction}, EntryTime={FormatTime(candidate.EntryTimeUtc)}, EntryPrice={candidate.EntryPrice:F2}, Volume={candidate.Volume:F0}, Stop={candidate.Stop:F2}, TargetPOC={candidate.TargetPoc:F2}, Target2={candidate.Target2:F2}, Risk={candidate.Risk:F2}, RewardPOC={candidate.RewardPoc:F2}, RewardT2={candidate.RewardT2:F2}, RR_POC={(candidate.Risk > 0 ? candidate.RewardPoc / candidate.Risk : 0):F2}, RR_T2={(candidate.Risk > 0 ? candidate.RewardT2 / candidate.Risk : 0):F2}, OutcomePOC={outcome.OutcomePoc}, PnLPOC={outcome.PnlPoc:F2}, OutcomeT2={outcome.OutcomeT2}, PnLT2={outcome.PnlT2:F2}, MFE={outcome.Mfe:F2}, MAE={outcome.Mae:F2}", candidate.EntryTimeUtc);
                    LogDynamicStopStudy(setup, trigger, candidate);
                }

                LogFabioStyleScaleInStudy(setup, trigger, candidates);
            }

            _dayStudyCompleted = true;
        }

        private void LogDynamicStopStudy(BalanceSetup setup, string trigger, StudyCandidate candidate)
        {
            if (candidate.CandidateType != "VALUE_REENTRY_BIG_TRADE")
                return;

            var secondsAfterRejection = (candidate.EntryTimeUtc - setup.RejectionTimeUtc).TotalSeconds;
            var triggerAtEntry = GetStudyTriggerLabelAtTime(setup, candidate.EntryTimeUtc);
            var secondsAfterPocTrigger = setup.StudyPocTriggerConfirmed
                ? (candidate.EntryTimeUtc - setup.StudyPocTriggerTimeUtc).TotalSeconds
                : double.NaN;
            var valueWidth = Math.Abs(setup.VAH - setup.VAL);
            var plans = new List<DynamicStopPlan>
            {
                new("ORIGINAL_REJECTION", candidate.Stop),
                new("VALUE_EDGE_2T", setup.Direction == "Long" ? setup.VAL - 2m * _tickSize : setup.VAH + 2m * _tickSize),
                new("RECENT_SWING_AFTER_REJECTION_2T", GetRecentSwingStop(setup, candidate.EntryTimeUtc))
            };

            if (setup.StudyPocTriggerConfirmed && candidate.EntryTimeUtc > setup.StudyPocTriggerTimeUtc)
                plans.Add(new DynamicStopPlan("POC_TRIGGER_BAR_2T", GetPocTriggerBarStop(setup)));

            if (valueWidth > 0)
            {
                plans.Add(new DynamicStopPlan("CAP_VALUE_WIDTH_100", GetCappedStop(setup.Direction, candidate.EntryPrice, candidate.Stop, valueWidth)));
                plans.Add(new DynamicStopPlan("CAP_VALUE_WIDTH_50", GetCappedStop(setup.Direction, candidate.EntryPrice, candidate.Stop, valueWidth * 0.5m)));
            }

            foreach (var plan in plans)
            {
                var risk = Math.Abs(candidate.EntryPrice - plan.Stop);
                if (risk <= 0 || !IsStopBehindEntry(setup.Direction, candidate.EntryPrice, plan.Stop))
                    continue;

                var rewardPoc = Math.Abs(candidate.TargetPoc - candidate.EntryPrice);
                var rewardT2 = Math.Abs(candidate.Target2 - candidate.EntryPrice);
                var outcome = EvaluateProtectedTarget2Outcome(candidate.Direction, candidate.EntryPrice, plan.Stop, candidate.TargetPoc, candidate.Target2, candidate.EntryTimeUtc, setup.RejectionBar);

                StudyLog($"[DAY_STUDY_DYNAMIC_STOP_CANDIDATE] SetupId={setup.SetupId}, StopPlan={plan.Name}, CandidateType={candidate.CandidateType}, Trigger={trigger}, TriggerAtEntry={triggerAtEntry}, Direction={candidate.Direction}, EntryTime={FormatTime(candidate.EntryTimeUtc)}, EntryPrice={candidate.EntryPrice:F2}, Volume={candidate.Volume:F0}, Stop={plan.Stop:F2}, OriginalStop={candidate.Stop:F2}, TargetPOC={candidate.TargetPoc:F2}, Target2={candidate.Target2:F2}, Risk={risk:F2}, OriginalRisk={candidate.Risk:F2}, RiskReductionPct={(candidate.Risk > 0 ? 1m - risk / candidate.Risk : 0):F2}, ValueWidth={valueWidth:F2}, RiskToValueWidth={(valueWidth > 0 ? risk / valueWidth : 0):F2}, RewardPOC={rewardPoc:F2}, RewardT2={rewardT2:F2}, RR_POC={rewardPoc / risk:F2}, RR_T2={rewardT2 / risk:F2}, SecondsAfterRejection={secondsAfterRejection:F1}, RejectionAgeBucket={GetRejectionAgeBucket(secondsAfterRejection)}, SecondsAfterPocTrigger={(double.IsNaN(secondsAfterPocTrigger) ? "NA" : secondsAfterPocTrigger.ToString("F1"))}, ExitReason={outcome.ExitReason}, PnL={outcome.Pnl:F2}, RMultiple={outcome.RMultiple:F2}R, Target1Hit={outcome.Target1Hit}", candidate.EntryTimeUtc);
            }
        }

        private decimal GetPocTriggerBarStop(BalanceSetup setup)
        {
            var bar = FindBarByTime(setup.StudyPocTriggerTimeUtc, setup.RejectionBar);
            var candle = _getCandle(bar);
            return setup.Direction == "Long"
                ? candle.Low - 2m * _tickSize
                : candle.High + 2m * _tickSize;
        }

        private decimal GetRecentSwingStop(BalanceSetup setup, DateTime entryTimeUtc)
        {
            var startBar = Math.Min(FindBarByTime(setup.RejectionTimeUtc, setup.RejectionBar) + 1, Math.Max(0, _currentBar - 1));
            var endBar = FindBarByTime(entryTimeUtc, startBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            decimal? swing = null;

            for (var bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                swing = setup.Direction == "Long"
                    ? !swing.HasValue ? candle.Low : Math.Min(swing.Value, candle.Low)
                    : !swing.HasValue ? candle.High : Math.Max(swing.Value, candle.High);
            }

            if (!swing.HasValue)
                return setup.StopPrice;

            return setup.Direction == "Long"
                ? swing.Value - 2m * _tickSize
                : swing.Value + 2m * _tickSize;
        }

        private static decimal GetCappedStop(string direction, decimal entryPrice, decimal originalStop, decimal maxRisk)
        {
            var originalRisk = Math.Abs(entryPrice - originalStop);
            if (originalRisk <= maxRisk)
                return originalStop;

            return direction == "Long"
                ? entryPrice - maxRisk
                : entryPrice + maxRisk;
        }

        private static bool IsStopBehindEntry(string direction, decimal entryPrice, decimal stop)
        {
            return direction == "Long"
                ? stop < entryPrice
                : stop > entryPrice;
        }

        private static string GetRejectionAgeBucket(double seconds)
        {
            if (seconds < 120)
                return "0_2M";
            if (seconds < 300)
                return "2_5M";
            if (seconds < 600)
                return "5_10M";
            if (seconds < 1200)
                return "10_20M";
            if (seconds < 1800)
                return "20_30M";
            return "30_60M";
        }

        private StudyCandidate ToOperationalStudyCandidate(BalanceSetup setup, StudyCandidate candidate)
        {
            var stop = GetOperationalStopPrice(setup, candidate.EntryPrice);
            var risk = IsStopBehindEntry(setup.Direction, candidate.EntryPrice, stop)
                ? Math.Abs(candidate.EntryPrice - stop)
                : 0m;

            return candidate with { Stop = stop, Risk = risk };
        }

        private void LogFabioStyleScaleInStudy(BalanceSetup setup, string trigger, List<StudyCandidate> candidates)
        {
            var valueCandidates = candidates
                .Where(c => c.CandidateType == "VALUE_REENTRY_BIG_TRADE")
                .Select(c => ToOperationalStudyCandidate(setup, c))
                .Where(c => c.Risk > 0 && c.RewardT2 / c.Risk >= MinRewardRiskToTarget2)
                .OrderBy(c => c.EntryTimeUtc)
                .ToList();

            var baseCandidate = valueCandidates
                .FirstOrDefault(c => (c.EntryTimeUtc - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds);
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

        private bool HasExpandedAfterRiskFree(BalanceSetup setup, CumulativeTrade trade, DateTime riskFreeTimeUtc, decimal minExpansionPct)
        {
            if (minExpansionPct <= 0)
                return true;

            var target2 = GetStudyTarget2(setup);
            var targetDistance = Math.Abs(target2 - setup.POC);
            if (targetDistance <= 0)
                return false;

            var requiredExpansion = targetDistance * minExpansionPct;
            var startBar = FindBarByTime(riskFreeTimeUtc, setup.RejectionBar);
            var endBar = FindBarByTime(trade.Time, startBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(trade.Time));
            var bestExpansion = 0m;

            for (var bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                bestExpansion = setup.Direction == "Long"
                    ? Math.Max(bestExpansion, candle.High - setup.POC)
                    : Math.Max(bestExpansion, setup.POC - candle.Low);
            }

            return bestExpansion >= requiredExpansion;
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
            var inEntryZone = IsInEntryZone(setup, trade);
            var continuation = IsTarget2ManagementTrigger(trigger) && trade.Time > setup.StudyPocTriggerTimeUtc && IsBeyondPocContinuationEntry(setup, trade);
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

        private StudyContinuationStats CountPostPocContinuationTrades(BalanceSetup setup, DateTime entryTimeUtc)
        {
            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var firstAfterPoc = _lastHistoricalTrades
                .Where(t => t.Time > entryTimeUtc)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => setup.Direction == "Long" ? Math.Max(t.FirstPrice, t.Lastprice) >= setup.POC : Math.Min(t.FirstPrice, t.Lastprice) <= setup.POC)
                .OrderBy(t => t.Time)
                .FirstOrDefault();
            if (firstAfterPoc == null)
                return new StudyContinuationStats(0, 0, "NONE");

            var trades = _lastHistoricalTrades
                .Where(t => t.Time >= firstAfterPoc.Time && t.Time <= firstAfterPoc.Time.AddMinutes(30))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Direction == expectedDirection)
                .ToList();

            return new StudyContinuationStats(trades.Count, trades.Sum(t => t.Volume), FormatTime(firstAfterPoc.Time));
        }

        private ProtectedStudyOutcome EvaluateHoldToTarget2OutcomeFromBars(string direction, decimal entry, decimal stop, decimal target2, DateTime entryTimeUtc, int fallbackBar)
        {
            var risk = Math.Abs(entry - stop);
            if (risk <= 0)
                return new ProtectedStudyOutcome("INVALID_RISK", 0, 0, false);

            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var startBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var closePrice = entry;
            for (var bar = startBar + 1; bar < _currentBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(candle.Time))
                    continue;

                closePrice = candle.Close;
                if (direction == "Long")
                {
                    if (candle.High >= target2)
                    {
                        var pnl = target2 - entry;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, true);
                    }

                    if (candle.Low <= stop)
                    {
                        var pnl = stop - entry;
                        return new ProtectedStudyOutcome("STOP_HIT", pnl, pnl / risk, false);
                    }
                }
                else
                {
                    if (candle.Low <= target2)
                    {
                        var pnl = entry - target2;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, true);
                    }

                    if (candle.High >= stop)
                    {
                        var pnl = entry - stop;
                        return new ProtectedStudyOutcome("STOP_HIT", pnl, pnl / risk, false);
                    }
                }
            }

            var closePnl = direction == "Long" ? closePrice - entry : entry - closePrice;
            return new ProtectedStudyOutcome("LONDON_CLOSE", closePnl, closePnl / risk, false);
        }

        private HistoricalContext GetHistoricalContextAtTime(DateTime eventUtc, int fallbackBar)
        {
            var bar = FindBarByTime(eventUtc, fallbackBar);
            if (_historicalBarSnapshots.TryGetValue(bar, out var snapshot))
                return new HistoricalContext(snapshot.PreviewPOC, snapshot.PreviewVAH, snapshot.PreviewVAL);

            return new HistoricalContext(_balanceTracker.LastPreviewPoc, _balanceTracker.LastPreviewVah, _balanceTracker.LastPreviewVal);
        }

        private bool IsDecisionAreaExit(ActivePosition position, string exitReason, decimal exitPrice)
        {
            return exitReason == "TARGET2_HIT" && Math.Abs(exitPrice - position.Target2Price) <= _tickSize;
        }

        private DateTime? FindDecisionAreaTouchTime(string direction, DateTime entryTimeUtc, decimal decisionPrice, int fallbackBar)
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

                if (direction == "Long" ? candle.High >= decisionPrice : candle.Low <= decisionPrice)
                    return eventTime;
            }

            return null;
        }

        private PostDecisionContinuationStats GetPostDecisionStats(ActivePosition position, DateTime decisionTimeUtc, TimeSpan? window)
        {
            var expectedDirection = position.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = position.Direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var endTime = window.HasValue ? decisionTimeUtc.Add(window.Value) : GetLondonSessionEndUtc(decisionTimeUtc);
            var trades = _lastHistoricalTrades
                .Where(t => t.Time > decisionTimeUtc && t.Time <= endTime)
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .ToList();
            var same = trades.Where(t => t.Direction == expectedDirection).ToList();
            var opposite = trades.Where(t => t.Direction == oppositeDirection).ToList();
            var closesBeyond = 0;
            var closesBackInside = 0;
            var mfe = 0m;
            var mae = 0m;
            var maxFavorablePrice = position.Target2Price;
            var maxAdversePrice = position.Target2Price;
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(decisionTimeUtc));
            var startBar = FindBarByTime(decisionTimeUtc, position.EntryBar);

            for (var bar = startBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (eventTime <= decisionTimeUtc)
                    continue;
                if (eventTime > endTime)
                    break;
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;
                if (!IsInLondonSession(eventTime))
                    break;

                if (position.Direction == "Long")
                {
                    maxFavorablePrice = Math.Max(maxFavorablePrice, candle.High);
                    maxAdversePrice = Math.Min(maxAdversePrice, candle.Low);
                    mfe = Math.Max(mfe, candle.High - position.Target2Price);
                    mae = Math.Max(mae, position.Target2Price - candle.Low);
                    if (candle.Close > position.Target2Price)
                        closesBeyond++;
                    if (candle.Close < position.Target2Price)
                        closesBackInside++;
                }
                else
                {
                    maxFavorablePrice = Math.Min(maxFavorablePrice, candle.Low);
                    maxAdversePrice = Math.Max(maxAdversePrice, candle.High);
                    mfe = Math.Max(mfe, position.Target2Price - candle.Low);
                    mae = Math.Max(mae, candle.High - position.Target2Price);
                    if (candle.Close < position.Target2Price)
                        closesBeyond++;
                    if (candle.Close > position.Target2Price)
                        closesBackInside++;
                }
            }

            return new PostDecisionContinuationStats(
                same.Count,
                same.Sum(t => t.Volume),
                same.Count > 0 ? same.Max(t => t.Volume) : 0,
                opposite.Count,
                opposite.Sum(t => t.Volume),
                opposite.Count > 0 ? opposite.Max(t => t.Volume) : 0,
                closesBeyond,
                closesBackInside,
                mfe,
                mae,
                maxFavorablePrice,
                maxAdversePrice);
        }

        private bool IsDecisionAcceptanceConfirmed(PostDecisionContinuationStats stats)
        {
            return stats.ClosesBeyondDecision > 0
                && stats.SameTrades > 0
                && stats.SameVolume > stats.OppositeVolume
                && stats.MaxSameVolume >= stats.MaxOppositeVolume;
        }

        private CumulativeTrade? FindFabioStyleContinuationReentry(ActivePosition position, DateTime decisionTimeUtc)
        {
            var endTime = decisionTimeUtc.AddMinutes(5);
            var expectedDirection = position.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            return _lastHistoricalTrades
                .Where(t => t.Time > decisionTimeUtc && t.Time <= endTime)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Direction == expectedDirection)
                .Where(t => IsBeyondDecisionArea(position.Direction, t.Lastprice, position.Target2Price))
                .Where(t => HasDecisionAcceptanceBefore(position, decisionTimeUtc, t.Time))
                .OrderBy(t => t.Time)
                .FirstOrDefault();
        }

        private bool HasDecisionAcceptanceBefore(ActivePosition position, DateTime decisionTimeUtc, DateTime tradeTimeUtc)
        {
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(decisionTimeUtc));
            var startBar = FindBarByTime(decisionTimeUtc, position.EntryBar);
            for (var bar = startBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (eventTime <= decisionTimeUtc)
                    continue;
                if (eventTime >= tradeTimeUtc)
                    break;
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;
                if (!IsInLondonSession(eventTime))
                    break;

                if (position.Direction == "Long" ? candle.Close > position.Target2Price : candle.Close < position.Target2Price)
                    return true;
            }

            return false;
        }

        private decimal GetContinuationReentryStop(ActivePosition position, decimal reentryPrice)
        {
            var decisionStop = position.Direction == "Long"
                ? position.Target2Price - 2m * _tickSize
                : position.Target2Price + 2m * _tickSize;
            var maxRisk = Math.Abs(position.Target2Price - position.Target1Price) * 0.5m;
            if (maxRisk <= 0)
                return decisionStop;

            return position.Direction == "Long"
                ? Math.Max(decisionStop, reentryPrice - maxRisk)
                : Math.Min(decisionStop, reentryPrice + maxRisk);
        }

        private ProtectedStudyOutcome EvaluateFollowThroughSecondLegOutcome(string direction, decimal entry, decimal stop, DateTime decisionTimeUtc, DateTime entryTimeUtc)
        {
            var risk = Math.Abs(entry - stop);
            if (risk <= 0)
                return new ProtectedStudyOutcome("INVALID_RISK", 0, 0, false);

            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var startBar = FindBarByTime(entryTimeUtc, 0);
            var closePrice = entry;
            var extension50 = Math.Abs(entry - stop);
            var target = direction == "Long" ? entry + extension50 : entry - extension50;

            for (var bar = startBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (eventTime <= entryTimeUtc)
                    continue;
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;
                if (!IsInLondonSession(eventTime))
                    break;

                closePrice = candle.Close;
                if (direction == "Long")
                {
                    if (candle.High >= target)
                    {
                        var pnl = target - entry;
                        return new ProtectedStudyOutcome("EXTENSION_1R_HIT", pnl, pnl / risk, false);
                    }

                    if (candle.Low <= stop)
                    {
                        var pnl = stop - entry;
                        return new ProtectedStudyOutcome("DECISION_STOP_HIT", pnl, pnl / risk, false);
                    }
                }
                else
                {
                    if (candle.Low <= target)
                    {
                        var pnl = entry - target;
                        return new ProtectedStudyOutcome("EXTENSION_1R_HIT", pnl, pnl / risk, false);
                    }

                    if (candle.High >= stop)
                    {
                        var pnl = entry - stop;
                        return new ProtectedStudyOutcome("DECISION_STOP_HIT", pnl, pnl / risk, false);
                    }
                }
            }

            var closePnl = direction == "Long" ? closePrice - entry : entry - closePrice;
            return new ProtectedStudyOutcome("LONDON_CLOSE", closePnl, closePnl / risk, false);
        }

        private DateTime GetLondonSessionEndUtc(DateTime eventUtc)
        {
            var london = MarketTimeZones.ToLondon(eventUtc);
            var londonEnd = new DateTime(london.Year, london.Month, london.Day, LondonSessionEndHour, LondonSessionEndMinute, 0, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(londonEnd, MarketTimeZones.London);
        }

        private static bool IsBeyondDecisionArea(string direction, decimal price, decimal decisionPrice)
        {
            return direction == "Long" ? price >= decisionPrice : price <= decisionPrice;
        }

        private ProtectedStudyOutcome EvaluateProtectedTarget2OutcomeFromTrades(string direction, decimal entry, decimal stop, decimal targetPoc, decimal target2, DateTime entryTimeUtc)
        {
            var risk = Math.Abs(entry - stop);
            if (risk <= 0)
                return new ProtectedStudyOutcome("INVALID_RISK", 0, 0, false);

            var target1Hit = false;
            var protectedStop = stop;
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var closePrice = entry;

            foreach (var trade in _lastHistoricalTrades.Where(t => t.Time > entryTimeUtc).OrderBy(t => t.Time))
            {
                var eventDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(trade.Time));
                if (eventDay != entryDay)
                    break;

                if (!IsLondonTradeAllowed(trade.Time))
                    break;

                var high = Math.Max(trade.FirstPrice, trade.Lastprice);
                var low = Math.Min(trade.FirstPrice, trade.Lastprice);
                closePrice = trade.Lastprice;

                if (direction == "Long")
                {
                    if (!target1Hit && targetPoc > entry && high >= targetPoc)
                    {
                        target1Hit = true;
                        protectedStop = Math.Max(entry, targetPoc - 2m * _tickSize);
                    }

                    if (target2 > entry && high >= target2)
                    {
                        var pnl = target2 - entry;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, target1Hit);
                    }

                    if (low <= protectedStop)
                    {
                        var pnl = protectedStop - entry;
                        return new ProtectedStudyOutcome(target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT", pnl, pnl / risk, target1Hit);
                    }
                }
                else
                {
                    if (!target1Hit && targetPoc < entry && low <= targetPoc)
                    {
                        target1Hit = true;
                        protectedStop = Math.Min(entry, targetPoc + 2m * _tickSize);
                    }

                    if (target2 < entry && low <= target2)
                    {
                        var pnl = entry - target2;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, target1Hit);
                    }

                    if (high >= protectedStop)
                    {
                        var pnl = entry - protectedStop;
                        return new ProtectedStudyOutcome(target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT", pnl, pnl / risk, target1Hit);
                    }
                }
            }

            var londonClosePnl = direction == "Long" ? closePrice - entry : entry - closePrice;
            return new ProtectedStudyOutcome("LONDON_CLOSE", londonClosePnl, londonClosePnl / risk, target1Hit);
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

        private void CaptureHistoricalBarSnapshot(int bar, IndicatorCandle candle)
        {
            if (!IsInLondonSession(candle.Time))
                return;

            if (_historicalBarSnapshots.ContainsKey(bar))
                return;

            _historicalBarSnapshots[bar] = CreateHistoricalBarSnapshot(bar, candle);
        }

        private HistoricalBarSnapshot CreateHistoricalBarSnapshot(int bar, IndicatorCandle candle)
        {
            var (bid, ask, delta, topLevels) = GetCandleVolumeDiagnostics(candle);
            var eventTime = GetCandleEventTime(candle);
            var poc = _balanceTracker.LastPreviewPoc;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            return new HistoricalBarSnapshot
            {
                Bar = bar,
                EventTimeUtc = eventTime,
                Open = candle.Open,
                High = candle.High,
                Low = candle.Low,
                Close = candle.Close,
                Volume = candle.Volume,
                Bid = bid,
                Ask = ask,
                Delta = delta,
                PreviewPOC = poc,
                PreviewVAH = vah,
                PreviewVAL = val,
                PreviousSessionHigh = _balanceTracker.CurrentZone?.High ?? 0,
                PreviousSessionLow = _balanceTracker.CurrentZone?.Low ?? 0,
                CloseLocation = GetPriceLocation(candle.Close, poc, vah, val),
                TopLevels = topLevels
            };
        }

        private string GetBarSetupDiagnostics(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return "NO_PROFILE";

            var highBreak = snapshot.High > snapshot.PreviewVAH;
            var lowBreak = snapshot.Low < snapshot.PreviewVAL;
            var shortCloseBackInside = snapshot.Close < snapshot.PreviewVAH;
            var longCloseBackInside = snapshot.Close > snapshot.PreviewVAL;
            var shortRejectionTicks = (snapshot.High - snapshot.Close) / _tickSize;
            var longRejectionTicks = (snapshot.Close - snapshot.Low) / _tickSize;
            var shortReady = highBreak && shortCloseBackInside && shortRejectionTicks >= RejectionThresholdTicks;
            var longReady = lowBreak && longCloseBackInside && longRejectionTicks >= RejectionThresholdTicks;

            return $"HighBreakVAH={highBreak};LowBreakVAL={lowBreak};ShortCloseBackInside={shortCloseBackInside};LongCloseBackInside={longCloseBackInside};ShortRejectionTicks={shortRejectionTicks:F1};LongRejectionTicks={longRejectionTicks:F1};ShortSetupReady={shortReady};LongSetupReady={longReady}";
        }

        private string GetActiveSetupDiagnostics(DateTime eventUtc)
        {
            var candidates = _activeSetups
                .Where(s => !s.Expired)
                .Where(s => eventUtc >= s.RejectionTimeUtc && eventUtc <= s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .OrderBy(s => s.RejectionTimeUtc)
                .Select(s => $"{s.SetupId[..Math.Min(8, s.SetupId.Length)]}:{s.SetupSource}:{s.Direction}:Age={(eventUtc - s.RejectionTimeUtc).TotalSeconds:F0}s:Confirmed={s.AggressionConfirmed}:Trigger={GetStudyTriggerLabelAtTime(s, eventUtc)}")
                .ToList();

            return candidates.Count == 0 ? "NONE" : string.Join("|", candidates);
        }

        private bool ShouldDebugHistoricalDay(DateTime eventUtc)
        {
            if (_historicalStudyDebugDays.Count == 0)
                return true;

            return _historicalStudyDebugDays.Contains(DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventUtc)));
        }

        private string FormatHistoricalStudyDebugDays()
        {
            if (!_historicalStudyDebugEnabled)
                return "OFF";

            return _historicalStudyDebugDays.Count == 0
                ? "ALL"
                : string.Join("|", _historicalStudyDebugDays.OrderBy(d => d).Select(d => d.ToString("yyyy-MM-dd")));
        }

        private string GetTradeCandidateDiagnostics(CumulativeTrade trade)
        {
            var candidates = _activeSetups
                .Where(s => !s.Expired && !s.AggressionConfirmed)
                .Where(s => trade.Time > s.RejectionTimeUtc && trade.Time <= s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .OrderBy(s => Math.Abs((trade.Time - s.RejectionTimeUtc).TotalSeconds))
                .Take(5)
                .Select(s =>
                {
                    var sameDirection = s.Direction == "Long" ? trade.Direction == TradeDirection.Buy : trade.Direction == TradeDirection.Sell;
                    var inEntryZone = IsInEntryZone(s, trade);
                    var continuationZone = IsBeyondPocContinuationEntry(s, trade);
                    var operationalZone = IsOperationalEntryZone(s, trade);
                    var fresh = (trade.Time - s.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds;
                    var rr = GetRewardRiskToTarget2(s, trade.Lastprice);
                    var reason = !sameDirection
                        ? "WRONG_DIRECTION"
                        : !operationalZone
                            ? "OUTSIDE_ENTRY_ZONE"
                            : !fresh
                                ? "STALE"
                                : rr < MinRewardRiskToTarget2
                                    ? "RR_TOO_LOW"
                                    : "VALID";
                    return $"{s.SetupId[..Math.Min(8, s.SetupId.Length)]}:{s.SetupSource}:{s.Direction}:Reason={reason}:Age={(trade.Time - s.RejectionTimeUtc).TotalSeconds:F0}s:InZone={inEntryZone}:ContinuationZone={continuationZone}:OperationalZone={operationalZone}:RR={rr:F2}:TriggerAtTrade={GetStudyTriggerLabelAtTime(s, trade.Time)}";
                })
                .ToList();

            return candidates.Count == 0 ? "NO_ACTIVE_SETUP" : string.Join("|", candidates);
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
        private sealed record StudyContinuationStats(int Count, decimal Volume, string FirstTrade);
        private sealed record PocManagementPressureStats(int SameTrades, decimal SameVolume, decimal MaxSameVolume, int OppositeTrades, decimal OppositeVolume, decimal MaxOppositeVolume);
        private sealed record AuctionPressureStats(int SameTrades, decimal SameVolume, decimal MaxSameVolume, int OppositeTrades, decimal OppositeVolume, decimal MaxOppositeVolume, decimal TotalVolume);
        private sealed record AlternativeSecondLegOutcome(string Status, DateTime EntryTimeUtc, decimal EntryPrice, bool ReachedFinalPoc, DateTime? FinalPocTouchTime, decimal PnlToFinalPoc, decimal LondonClosePnl, decimal Mfe, decimal Mae)
        {
            public static AlternativeSecondLegOutcome None(string status) => new(status, DateTime.MinValue, 0, false, null, 0, 0, 0, 0);
        }
        private sealed record PostDecisionContinuationStats(int SameTrades, decimal SameVolume, decimal MaxSameVolume, int OppositeTrades, decimal OppositeVolume, decimal MaxOppositeVolume, int ClosesBeyondDecision, int ClosesBackInsideDecision, decimal Mfe, decimal Mae, decimal MaxFavorablePrice, decimal MaxAdversePrice);
        private sealed record HistoricalContext(decimal POC, decimal VAH, decimal VAL);
        private sealed record DynamicStopPlan(string Name, decimal Stop);
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

        private string GetStudyTriggerLabelAtTime(BalanceSetup setup, DateTime timeUtc)
        {
            if (setup.StudyPocTriggerConfirmed && setup.StudyPocTriggerTimeUtc <= timeUtc)
                return GetPocTriggerLabel(setup);

            if (setup.StudyFollowThroughConfirmed && setup.StudyFollowThroughTimeUtc <= timeUtc)
                return GetFollowThroughLabel(setup);

            return "NONE";
        }

        private static string GetFollowThroughLabel(BalanceSetup setup)
        {
            if (IsFollowThroughSecondLegAuctionSetup(setup))
            {
                return setup.Direction == "Long"
                    ? "FOLLOW_THROUGH_SECOND_LEG_AUCTION_LONG"
                    : "FOLLOW_THROUGH_SECOND_LEG_AUCTION_SHORT";
            }

            if (IsFollowThroughContinuationSetup(setup))
            {
                return setup.Direction == "Long"
                    ? "FOLLOW_THROUGH_RECLAIM_CONTINUATION_LONG"
                    : "FOLLOW_THROUGH_RECLAIM_CONTINUATION_SHORT";
            }

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
                || studyTrigger == "POC_LOSS_AFTER_HIGH_REJECTION"
                || studyTrigger == "FOLLOW_THROUGH_RECLAIM_CONTINUATION_LONG"
                || studyTrigger == "FOLLOW_THROUGH_RECLAIM_CONTINUATION_SHORT";
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
            var cutoff = LondonSessionEndHour * 60 + LondonSessionEndMinute;
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
