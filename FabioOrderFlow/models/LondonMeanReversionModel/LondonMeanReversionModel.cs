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
    internal sealed partial class LondonMeanReversionModule
    {
        private readonly BalanceZoneTracker _balanceTracker;
        private readonly Action<string, bool> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly decimal _tickSize;

        private int _currentBar;
        private readonly List<BalanceSetup> _activeSetups = new();
        private readonly List<ActivePosition> _activePositions = new();
        private readonly List<DelayedReclaimCandidate> _delayedReclaimCandidates = new();
        private readonly List<CumulativeTrade> _processedAggressionTrades = new();

        private readonly List<TradeRecord> _completedTrades = new();
        private readonly HashSet<string> _setupKeys = new();
        private readonly Dictionary<string, decimal> _liveTradeMaxVolumeByKey = new();
        private long _liveAcceptedTradeCount;
        private DateTime _lastLiveHeartbeatUtc = DateTime.MinValue;
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
            _log($"[MR_OPERATIONAL_MODE] CoreMeanReversionOnly={OperationalCoreMeanReversionOnly}, AllowedTriggers=POC_RECLAIM_AFTER_LOW_REJECTION|POC_LOSS_AFTER_HIGH_REJECTION|DELAYED_RECLAIM, UnclassifiedNormalEntries=False", false);
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
            ExpireStaleOperationalState(trade.Time);
            ProcessAggressionTrade(trade, "FootprintCumulativeTradeLive", false);
            LogLiveFlowHeartbeat(trade);
        }

    }
}
