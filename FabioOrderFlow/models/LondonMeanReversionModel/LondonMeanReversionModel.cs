using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;
using ATAS.Indicators.Drawing;

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
        private readonly List<DrawingRectangle> _chartRectangles;
        private readonly List<LineTillTouch> _chartLines;
        private readonly HashSet<string> _drawnCompressionProfiles = new();
        private readonly Dictionary<string, TradeChartVisual> _tradeChartVisuals = new();

        private const decimal LondonBigTradeVolume = 20m;
        private const int RejectionThresholdTicks = 10;
        private const int StopInsideFailedExtremeTicks = 2;
        private const int EntryTimeoutSeconds = 1200;
        private const decimal MinRewardRiskToPoc = 1.0m;
        private const decimal BreakEvenTriggerR = 1.0m;
        private const int BreakEvenOffsetTicks = 0;
        private const int LondonSessionStartHour = 8;
        private const int LondonSessionEndHour = 16;
        private const int NewYorkSessionEndHour = 16;
        private const int LiveHeartbeatTradeStep = 25;
        private const int LiveHeartbeatMinSeconds = 60;
        private const int LocalCompressionBaselineLookbackBars = 24;
        private const int LocalCompressionMinimumBaselineBars = 12;
        private const int LocalCompressionMinimumBuildingBars = 6;
        private const int LocalCompressionReadyPersistenceBars = 2;
        private const int LocalCompressionResolutionPersistenceBars = 2;
        private const decimal LocalCompressionStartMaximumRangePercentile = 0.50m;
        private const decimal LocalCompressionMinimumExtensionOverlap = 0.20m;
        private const decimal LocalCompressionBoundaryTolerance = 0.15m;
        private const decimal LocalCompressionMinimumReadyScore = 0.65m;
        private const decimal LocalCompressionMinimumBuildingScore = 0.40m;
        private const decimal CompressionContractionWeight = 0.20m;
        private const decimal CompressionOverlapWeight = 0.20m;
        private const decimal CompressionDirectionalWeight = 0.15m;
        private const decimal CompressionRotationWeight = 0.10m;
        private const decimal CompressionContainmentWeight = 0.10m;
        private const decimal CompressionBoundaryStabilityWeight = 0.10m;
        private const decimal CompressionPocStabilityWeight = 0.075m;
        private const decimal CompressionValueConcentrationWeight = 0.075m;
        private const int CompressionStudyConfirmationBars = 2;
        private static readonly int[] CompressionLedgerOutcomeHorizons = { 1, 3, 6, 12 };
        private static readonly int[] ShadowAcceptanceOutcomeHorizons = { 6, 12 };
        private const string ShadowAcceptanceModel = "ACCEPTANCE_CONTINUATION_V1";
        private const int ShadowAcceptancePathMinutes = 60;
        private const int ShadowAcceptancePathCompletionToleranceMinutes = 5;
        private const string StudyMode = "COMPRESSION_EVENT_LEDGER_NO_TRADES";
        private static readonly bool LogProfileDiagnosticsForSetups = false;
        private static readonly bool LogProfileDiagnosticsForEntries = true;
        private const string ActiveCompressionProfileSource = "ActiveCompressionProfile";

        private int _currentBar;
        private readonly List<BalanceSetup> _activeSetups = new();
        private readonly List<ActivePosition> _activePositions = new();
        private readonly List<CumulativeTrade> _historicalTrades = new();
        private readonly HashSet<string> _historicalTradeKeys = new();
        private readonly List<HistoricalLedgerTradeWindow> _historicalLedgerTradeWindows = new();
        private long _historicalTradesReceived;
        private long _historicalTradesOutsideLedgerWindows;
        private long _historicalTradesDuplicate;
        private readonly List<TradeRecord> _completedTrades = new();
        private readonly HashSet<string> _setupKeys = new();
        private readonly Dictionary<string, decimal> _liveTradeMaxVolumeByKey = new();
        private readonly Dictionary<string, int> _entryRejectCounts = new();
        private readonly Dictionary<string, int> _setupExpirationCounts = new();
        private readonly List<CompressionProfileSnapshot> _compressionStudyProfiles = new();
        private readonly List<CompressionProfileSnapshot> _liveCompressionLedgerProfiles = new();
        private readonly List<CumulativeTrade> _liveStudyTrades = new();
        private readonly HashSet<string> _liveStudyTradeKeys = new();
        private readonly HashSet<string> _drawnCompressionStudyCandidates = new();
        private readonly HashSet<string> _loggedCompressionLedgerProfiles = new();
        private readonly HashSet<string> _loggedCompressionLedgerEvents = new();
        private readonly HashSet<string> _loggedCompressionLedgerOutcomes = new();
        private readonly HashSet<string> _loggedShadowAcceptanceEntries = new();
        private readonly HashSet<string> _loggedShadowAcceptanceOutcomes = new();
        private readonly HashSet<string> _loggedShadowAcceptancePathBars = new();
        private readonly ProfileAccumulator _currentDayProfile = new();
        private readonly ProfileAccumulator _currentLondonProfile = new();
        private DateOnly? _currentDayItaly;
        private DateOnly? _currentLondonDay;
        private bool _currentLondonProfileActive;
        private ReferenceValueArea? _previousDayReference;
        private ReferenceValueArea? _previousLondonReference;
        private CompressionCandidateState? _buildingCompressionProfile;
        private CompressionProfileSnapshot? _activeCompressionProfile;
        private int _lastCompressionEvaluatedBar = -1;
        private int _compressionOutsideDirection;
        private int _compressionOutsideCloses;
        private long _liveAcceptedTradeCount;
        private DateTime _lastLiveHeartbeatUtc = DateTime.MinValue;
        private bool _processingHistoricalReplay;
        private readonly string _chartTimeFrame;

        public LondonMeanReversionModule(
            BalanceZoneTracker balanceTracker,
            Action<string, bool> log,
            Func<int, IndicatorCandle> getCandle,
            decimal tickSize,
            string chartTimeFrame,
            List<DrawingRectangle> chartRectangles,
            List<LineTillTouch> chartLines)
        {
            _ = balanceTracker ?? throw new ArgumentNullException(nameof(balanceTracker));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _getCandle = getCandle ?? throw new ArgumentNullException(nameof(getCandle));
            _tickSize = tickSize > 0 ? tickSize : 1m;
            _chartTimeFrame = string.IsNullOrWhiteSpace(chartTimeFrame) ? "UNKNOWN" : chartTimeFrame;
            _chartRectangles = chartRectangles ?? throw new ArgumentNullException(nameof(chartRectangles));
            _chartLines = chartLines ?? throw new ArgumentNullException(nameof(chartLines));

            _log($"[MR_MODE] Model=FabioCompressionStudy, Modes=LIVE|HISTORICAL, StudyMode={StudyMode}, OperationalEntries=DISABLED, ReferenceProfiles=LOG_ONLY:PreviousDayProfile|PreviousLondonProfile, ChartVisuals=LONDON_CONTEXT_ONLY, Ledger=BOUNDARY_EVENTS_AND_OUTCOMES, LedgerQualification=NONE, LedgerOutcomeHorizons={string.Join("|", CompressionLedgerOutcomeHorizons)}, ShadowModel={ShadowAcceptanceModel}, ShadowTrigger=SECOND_CONSECUTIVE_OUTSIDE_CLOSE, ShadowOutcomeHorizons={string.Join("|", ShadowAcceptanceOutcomeHorizons)}, ShadowPath=EVERY_COMPLETED_CHART_BAR_FOR_{ShadowAcceptancePathMinutes}_MINUTES, ChartTimeFrame={_chartTimeFrame}, ShadowOrders=DISABLED, CompressionLifecycle=SEARCHING|BUILDING|READY|RESOLVED, CompressionDetection=DYNAMIC_SCORE, CompressionBaseline=PRIOR_{LocalCompressionBaselineLookbackBars}_BARS, CompressionMinBaselineBars={LocalCompressionMinimumBaselineBars}, CompressionMinBuildingBars={LocalCompressionMinimumBuildingBars}, CompressionReadyScore={LocalCompressionMinimumReadyScore:F2}, CompressionReadyPersistence={LocalCompressionReadyPersistenceBars}, CompressionResolutionPersistence={LocalCompressionResolutionPersistenceBars}, HistoricalBigTradeVolume={LondonBigTradeVolume:F0}, HistoricalStudyConfirmationBars={CompressionStudyConfirmationBars}", false);
        }

        public IReadOnlyList<TradeRecord> CompletedTrades => _completedTrades;
        public IReadOnlyList<ActivePosition> ActivePositions => _activePositions;
        public IReadOnlyList<BalanceSetup> ActiveSetups => _activeSetups;

        public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
        {
            _currentBar = currentBar;
            UpdateReferenceProfiles(bar, candle);
            UpdateActiveCompressionProfile(bar);
            if (!IsHistoricalContext(bar))
                UpdateLiveCompressionLedgerOutcomes(bar);
        }

        public void OnNewSessionHigh(int bar, IndicatorCandle candle, decimal previousHigh)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
        }

        public void OnNewSessionLow(int bar, IndicatorCandle candle, decimal previousLow)
        {
            _currentBar = Math.Max(_currentBar, bar + 1);
        }

        public void BeginHistoricalCumulativeTrades()
        {
            _historicalTrades.Clear();
            _historicalTradeKeys.Clear();
            _historicalLedgerTradeWindows.Clear();
            _historicalTradesReceived = 0;
            _historicalTradesOutsideLedgerWindows = 0;
            _historicalTradesDuplicate = 0;

            foreach (var profile in _compressionStudyProfiles.Where(profile => profile.ResolvedBar >= profile.ReadyBar))
            {
                var beginBar = Math.Min(profile.ReadyBar + 1, Math.Max(_currentBar - 1, 0));
                var endBar = Math.Min(profile.ResolvedBar, Math.Max(_currentBar - 1, 0));
                if (endBar < beginBar)
                    continue;

                _historicalLedgerTradeWindows.Add(new HistoricalLedgerTradeWindow
                {
                    BeginUtc = _getCandle(beginBar).Time,
                    EndUtc = GetCandleEventTime(_getCandle(endBar)).AddMinutes(
                        ShadowAcceptancePathMinutes + ShadowAcceptancePathCompletionToleranceMinutes)
                });
            }

            _log($"[MR_HISTORICAL_TRADES_BEGIN] ProfileWindows={_historicalLedgerTradeWindows.Count}, CompressionProfiles={_compressionStudyProfiles.Count}", false);
        }

        public void AppendHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades, int windowIndex, int windowCount)
        {
            var received = 0;
            var retained = 0;
            var outside = 0;
            var duplicate = 0;
            foreach (var trade in cumulativeTrades)
            {
                received++;
                if (!IsHistoricalLedgerTradeTime(trade.Time))
                {
                    outside++;
                    continue;
                }

                if (!_historicalTradeKeys.Add(GetHistoricalTradeKey(trade)))
                {
                    duplicate++;
                    continue;
                }

                _historicalTrades.Add(trade);
                retained++;
            }

            _historicalTradesReceived += received;
            _historicalTradesOutsideLedgerWindows += outside;
            _historicalTradesDuplicate += duplicate;
            _log($"[MR_HISTORICAL_TRADES_WINDOW] Window={windowIndex}/{windowCount}, Received={received}, Retained={retained}, OutsideLedgerWindows={outside}, Duplicate={duplicate}, RetainedTotal={_historicalTrades.Count}", false);
        }

        public void CompleteHistoricalCumulativeTrades()
        {
            _historicalTrades.Sort((left, right) => left.Time.CompareTo(right.Time));
            _log($"[MR_HISTORICAL_TRADES] Received={_historicalTradesReceived}, Retained={_historicalTrades.Count}, OutsideLedgerWindows={_historicalTradesOutsideLedgerWindows}, Duplicate={_historicalTradesDuplicate}, BeginItaly={(_historicalTrades.Count > 0 ? MarketTimeZones.ToItaly(_historicalTrades.First().Time).ToString("yyyy-MM-dd HH:mm:ss") : "NA")}, EndItaly={(_historicalTrades.Count > 0 ? MarketTimeZones.ToItaly(_historicalTrades.Last().Time).ToString("yyyy-MM-dd HH:mm:ss") : "NA")}", false);
        }

        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            BeginHistoricalCumulativeTrades();
            AppendHistoricalCumulativeTrades(cumulativeTrades, 1, 1);
            CompleteHistoricalCumulativeTrades();
        }

        private bool IsHistoricalLedgerTradeTime(DateTime timeUtc)
        {
            return _historicalLedgerTradeWindows.Any(window => timeUtc >= window.BeginUtc && timeUtc <= window.EndUtc);
        }

        private static string GetHistoricalTradeKey(CumulativeTrade trade)
        {
            return $"{trade.Time.Ticks}:{trade.Direction}:{trade.FirstPrice:F2}:{trade.Lastprice:F2}:{trade.Volume:F4}";
        }

        public void ProcessHistoricalPositions(int startBar, int endBar)
        {
            var startedUtc = DateTime.UtcNow;
            var previousReplayState = _processingHistoricalReplay;
            _processingHistoricalReplay = true;
            _completedTrades.Clear();
            _activePositions.Clear();
            _activeSetups.Clear();
            _entryRejectCounts.Clear();
            _setupExpirationCounts.Clear();
            _liveCompressionLedgerProfiles.Clear();
            _loggedCompressionLedgerProfiles.Clear();
            _loggedCompressionLedgerEvents.Clear();
            _loggedCompressionLedgerOutcomes.Clear();
            _loggedShadowAcceptanceEntries.Clear();
            _loggedShadowAcceptanceOutcomes.Clear();
            _loggedShadowAcceptancePathBars.Clear();

            _log($"[HISTORICAL_FLOW_PROCESS_START] Model=FabioCompressionStudy, StudyMode={StudyMode}, StartBar={startBar}, EndBar={endBar}, StoredTrades={_historicalTrades.Count}, CompressionProfiles={_compressionStudyProfiles.Count}, OperationalEntries=DISABLED", false);

            try
            {
                var ledger = RunCompressionLedger(_compressionStudyProfiles, _historicalTrades, endBar, true);
                var durationMs = (DateTime.UtcNow - startedUtc).TotalMilliseconds;
                _log($"[HISTORICAL_FLOW_FINISH] Model=FabioCompressionStudy, StudyMode={StudyMode}, StartBar={startBar}, EndBar={endBar}, StoredTrades={_historicalTrades.Count}, Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0, LedgerProfiles={ledger.Profiles}, LedgerEvents={ledger.Events}, LedgerOutcomes={ledger.Outcomes}, ShadowAcceptanceEntries={ledger.ShadowAcceptanceEntries}, ShadowAcceptanceOutcomes={ledger.ShadowAcceptanceOutcomes}, ShadowAcceptancePathBars={ledger.ShadowAcceptancePathBars}, ShadowOrders=0, ProcessDurationMs={durationMs:F0}", false);
            }
            finally
            {
                _processingHistoricalReplay = previousReplayState;
            }
        }

        public void OnLiveCumulativeTrade(CumulativeTrade trade)
        {
            var key = GetLiveTradeKey(trade);
            if (_liveStudyTradeKeys.Add(key))
                _liveStudyTrades.Add(trade);
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
                    {
                        FinalizePreviousLondonReference(bar, _currentLondonDay.Value);
                        ResolveActiveCompressionProfile(bar, GetCandleEventTime(candle), candle.Close, "SESSION_ROLLOVER");
                    }

                    _currentLondonProfile.Reset();
                    ResetCompressionLifecycle();
                    _currentLondonDay = londonDay;
                    _currentLondonProfileActive = true;
                }

                _currentLondonProfile.ReplaceContribution(bar, candle);
                return;
            }

            if (_currentLondonProfileActive && _currentLondonDay.HasValue)
            {
                FinalizePreviousLondonReference(bar, _currentLondonDay.Value);
                ResolveActiveCompressionProfile(bar, GetCandleEventTime(candle), candle.Close, "SESSION_END");
                _currentLondonProfile.Reset();
                ResetCompressionLifecycle();
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

            // Setups are observations and may coexist. Position concurrency is enforced only at entry.
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

            AttachProfileDiagnostics(setup);
            _activeSetups.Add(setup);
            var isHistorical = IsHistoricalContext(setup.RejectionBar);
            _log($"[{tag}] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, Source={setup.Source}, Pattern={setup.Pattern}, ReferenceLabel={setup.ReferenceLabel}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", isHistorical);
            if (LogProfileDiagnosticsForSetups)
                LogProfileDiagnosticsContext(setup, "SETUP", setup.RejectionBar, setup.RejectionTimeUtc, isHistorical, null);
        }

        private void AttachProfileDiagnostics(BalanceSetup setup)
        {
            if (_activeCompressionProfile == null || _activeCompressionProfile.ReadyBar >= setup.RejectionBar)
                return;

            AttachActiveCompressionProfile(setup, _activeCompressionProfile);
        }

        private void AttachActiveCompressionProfile(BalanceSetup setup, CompressionProfileSnapshot snapshot)
        {
            var profile = snapshot.Reference;
            setup.HasActiveCompressionProfileContext = true;
            setup.ActiveCompressionProfileLabel = profile.Label;
            setup.ActiveCompressionProfileStartBar = profile.StartBar;
            setup.ActiveCompressionProfileEndBar = profile.EndBar;
            setup.ActiveCompressionProfileBeginTimeUtc = profile.BeginTimeUtc;
            setup.ActiveCompressionProfileEndTimeUtc = profile.EndTimeUtc;
            setup.ActiveCompressionProfileBars = profile.Bars;
            setup.ActiveCompressionPOC = profile.POC;
            setup.ActiveCompressionVAH = profile.VAH;
            setup.ActiveCompressionVAL = profile.VAL;
            setup.ActiveCompressionHigh = profile.High;
            setup.ActiveCompressionLow = profile.Low;
            setup.ActiveCompressionTotalVolume = profile.TotalVolume;
            setup.ActiveCompressionValueAreaVolume = profile.ValueAreaVolume;
            setup.ActiveCompressionMaxLevelVolume = profile.MaxLevelVolume;
            setup.ActiveCompressionReadyBar = snapshot.ReadyBar;
            setup.ActiveCompressionReadyTimeUtc = snapshot.ReadyTimeUtc;
            setup.ActiveCompressionOverlapRate = snapshot.OverlapRate;
            setup.ActiveCompressionRangeToAverageBarRange = snapshot.RangeToAverageBarRange;
            setup.ActiveCompressionDirectionalEfficiency = snapshot.DirectionalEfficiency;
            setup.ActiveCompressionCloseSpanRatio = snapshot.CloseSpanRatio;
            setup.ActiveCompressionBaselineMedianBarRange = snapshot.BaselineMedianBarRange;
            setup.ActiveCompressionRangeToBaselineMedian = snapshot.RangeToBaselineMedian;
            setup.ActiveCompressionAverageBarRangeToBaselineMedian = snapshot.AverageBarRangeToBaselineMedian;
            setup.ActiveCompressionScore = snapshot.CompressionScore;
            setup.ActiveCompressionContractionScore = snapshot.ContractionScore;
            setup.ActiveCompressionOverlapScore = snapshot.OverlapScore;
            setup.ActiveCompressionDirectionalScore = snapshot.DirectionalScore;
            setup.ActiveCompressionRotationScore = snapshot.RotationScore;
            setup.ActiveCompressionContainmentScore = snapshot.ContainmentScore;
            setup.ActiveCompressionBoundaryStabilityScore = snapshot.BoundaryStabilityScore;
            setup.ActiveCompressionPocStabilityScore = snapshot.PocStabilityScore;
            setup.ActiveCompressionValueConcentrationScore = snapshot.ValueConcentrationScore;
            setup.ActiveCompressionDirectionChanges = snapshot.DirectionChanges;
        }

        private void UpdateActiveCompressionProfile(int bar)
        {
            if (!_currentLondonProfileActive || !_currentLondonDay.HasValue)
                return;

            var completedBar = bar - 1;
            if (completedBar <= _lastCompressionEvaluatedBar)
                return;

            var bars = _currentLondonProfile.GetContributionsUpTo(completedBar);
            if (bars.Count == 0)
                return;

            var latest = bars[^1];
            _lastCompressionEvaluatedBar = latest.Bar;

            if (_activeCompressionProfile != null)
            {
                UpdateCompressionResolution(latest);
                return;
            }

            if (_buildingCompressionProfile == null)
            {
                TryStartCompressionCandidate(bars);
                return;
            }

            var selectedBeforeLatest = bars
                .Where(candidateBar => candidateBar.Bar >= _buildingCompressionProfile.StartBar && candidateBar.Bar < latest.Bar)
                .ToList();
            if (selectedBeforeLatest.Count == 0 || !CanExtendCompressionCandidate(selectedBeforeLatest, latest, _buildingCompressionProfile.BaselineMedianBarRange))
            {
                _buildingCompressionProfile = null;
                TryStartCompressionCandidate(bars);
                return;
            }

            _buildingCompressionProfile.LastBar = latest.Bar;
            var selectedBars = bars.Where(candidateBar => candidateBar.Bar >= _buildingCompressionProfile.StartBar).ToList();
            if (selectedBars.Count < LocalCompressionMinimumBuildingBars)
                return;

            if (!TryBuildDynamicCompressionSnapshot(_buildingCompressionProfile, selectedBars, out var snapshot) || snapshot == null)
            {
                _buildingCompressionProfile = null;
                TryStartCompressionCandidate(bars);
                return;
            }

            if (snapshot.CompressionScore < LocalCompressionMinimumBuildingScore)
            {
                _buildingCompressionProfile = null;
                TryStartCompressionCandidate(bars);
                return;
            }

            _buildingCompressionProfile.ReadyScorePersistence = snapshot.CompressionScore >= LocalCompressionMinimumReadyScore
                ? _buildingCompressionProfile.ReadyScorePersistence + 1
                : 0;
            if (_buildingCompressionProfile.ReadyScorePersistence < LocalCompressionReadyPersistenceBars)
                return;

            _activeCompressionProfile = snapshot;
            _compressionStudyProfiles.Add(snapshot);
            _buildingCompressionProfile = null;
            LogCompressionReady(snapshot);
        }

        private void TryStartCompressionCandidate(IReadOnlyList<ProfileAccumulator.BarContribution> bars)
        {
            var latestIndex = bars.Count - 1;
            if (!TryGetDynamicCompressionBaseline(bars, latestIndex, out var baselineRanges, out var baselineMedianBarRange))
                return;

            var latest = bars[latestIndex];
            var latestRange = latest.High - latest.Low;
            if (latestRange <= 0 || PercentileRank(baselineRanges, latestRange) > LocalCompressionStartMaximumRangePercentile)
                return;

            _buildingCompressionProfile = new CompressionCandidateState
            {
                StartBar = latest.Bar,
                LastBar = latest.Bar,
                BaselineMedianBarRange = baselineMedianBarRange,
                BaselineRanges = baselineRanges
            };
        }

        private static bool TryGetDynamicCompressionBaseline(
            IReadOnlyList<ProfileAccumulator.BarContribution> bars,
            int candidateStartIndex,
            out IReadOnlyList<decimal> baselineRanges,
            out decimal medianBarRange)
        {
            baselineRanges = Array.Empty<decimal>();
            medianBarRange = 0;
            var baselineCount = Math.Min(LocalCompressionBaselineLookbackBars, candidateStartIndex);
            if (baselineCount < LocalCompressionMinimumBaselineBars)
                return false;

            var ranges = bars
                .Skip(candidateStartIndex - baselineCount)
                .Take(baselineCount)
                .Select(candidateBar => candidateBar.High - candidateBar.Low)
                .Where(range => range > 0)
                .OrderBy(range => range)
                .ToList();
            if (ranges.Count < LocalCompressionMinimumBaselineBars)
                return false;

            baselineRanges = ranges;
            medianBarRange = Median(ranges);
            return medianBarRange > 0;
        }

        private static bool CanExtendCompressionCandidate(
            IReadOnlyList<ProfileAccumulator.BarContribution> candidateBars,
            ProfileAccumulator.BarContribution latest,
            decimal baselineMedianBarRange)
        {
            var recentBars = candidateBars.TakeLast(3).ToList();
            var candidateHigh = recentBars.Max(candidateBar => candidateBar.High);
            var candidateLow = recentBars.Min(candidateBar => candidateBar.Low);
            var latestRange = latest.High - latest.Low;
            if (latestRange <= 0 || baselineMedianBarRange <= 0)
                return false;

            var overlap = Math.Min(candidateHigh, latest.High) - Math.Max(candidateLow, latest.Low);
            var overlapRatio = Math.Max(0, overlap) / latestRange;
            var tolerance = baselineMedianBarRange * LocalCompressionBoundaryTolerance;
            var closeAccepted = latest.Close >= candidateLow - tolerance && latest.Close <= candidateHigh + tolerance;
            return closeAccepted && overlapRatio >= LocalCompressionMinimumExtensionOverlap;
        }

        private bool TryBuildDynamicCompressionSnapshot(
            CompressionCandidateState state,
            IReadOnlyList<ProfileAccumulator.BarContribution> bars,
            out CompressionProfileSnapshot? snapshot)
        {
            snapshot = null;
            if (!_currentLondonDay.HasValue || bars.Count < LocalCompressionMinimumBuildingBars)
                return false;

            var label = $"{_currentLondonDay.Value:yyyy-MM-dd}:{bars.First().Bar}-{bars.Last().Bar}:DynamicCompression";
            if (!TryBuildReferenceFromContributions(ActiveCompressionProfileSource, label, bars, out var reference) || reference?.IsValid != true)
                return false;

            var range = reference.High - reference.Low;
            var averageBarRange = bars.Average(candidateBar => candidateBar.High - candidateBar.Low);
            if (range <= 0 || averageBarRange <= 0 || state.BaselineMedianBarRange <= 0)
                return false;

            var contractionScore = 1m - PercentileRank(state.BaselineRanges, averageBarRange);
            var overlapScore = CalculateAverageOverlapScore(bars);
            var directionalEfficiency = CalculateDirectionalEfficiency(bars, out var directionChanges);
            var directionalScore = 1m - directionalEfficiency;
            var rotationScore = bars.Count > 2 ? Clamp01((decimal)directionChanges / (bars.Count - 2)) : 0;
            var closeSpanRatio = (bars.Max(candidateBar => candidateBar.Close) - bars.Min(candidateBar => candidateBar.Close)) / range;
            var containmentScore = 1m - Clamp01(closeSpanRatio);

            var previousRange = state.RangeHistory.Count > 0 ? state.RangeHistory[^1] : range;
            state.RangeHistory.Add(range);
            var boundaryExpansion = Math.Max(0, range - previousRange);
            var boundaryStabilityScore = 1m - Clamp01(boundaryExpansion / state.BaselineMedianBarRange);

            state.PocHistory.Add(reference.POC);
            var pocDrift = state.PocHistory.Count > 1 ? state.PocHistory.Max() - state.PocHistory.Min() : 0;
            var pocStabilityScore = 1m - Clamp01(pocDrift / range);
            var valueConcentrationScore = 1m - Clamp01((reference.VAH - reference.VAL) / range);
            var compressionScore =
                contractionScore * CompressionContractionWeight
                + overlapScore * CompressionOverlapWeight
                + directionalScore * CompressionDirectionalWeight
                + rotationScore * CompressionRotationWeight
                + containmentScore * CompressionContainmentWeight
                + boundaryStabilityScore * CompressionBoundaryStabilityWeight
                + pocStabilityScore * CompressionPocStabilityWeight
                + valueConcentrationScore * CompressionValueConcentrationWeight;

            snapshot = new CompressionProfileSnapshot
            {
                Reference = reference,
                ReadyBar = bars.Last().Bar,
                ReadyTimeUtc = bars.Last().EndTimeUtc,
                CompressionScore = Clamp01(compressionScore),
                ContractionScore = contractionScore,
                OverlapScore = overlapScore,
                DirectionalScore = directionalScore,
                RotationScore = rotationScore,
                ContainmentScore = containmentScore,
                BoundaryStabilityScore = boundaryStabilityScore,
                PocStabilityScore = pocStabilityScore,
                ValueConcentrationScore = valueConcentrationScore,
                OverlapRate = overlapScore,
                RangeToAverageBarRange = range / averageBarRange,
                DirectionalEfficiency = directionalEfficiency,
                CloseSpanRatio = closeSpanRatio,
                BaselineMedianBarRange = state.BaselineMedianBarRange,
                RangeToBaselineMedian = range / state.BaselineMedianBarRange,
                AverageBarRangeToBaselineMedian = averageBarRange / state.BaselineMedianBarRange,
                DirectionChanges = directionChanges
            };
            return true;
        }

        private static decimal CalculateAverageOverlapScore(IReadOnlyList<ProfileAccumulator.BarContribution> bars)
        {
            if (bars.Count < 2)
                return 0;

            var total = 0m;
            for (var i = 1; i < bars.Count; i++)
            {
                var previous = bars[i - 1];
                var current = bars[i];
                var overlap = Math.Min(previous.High, current.High) - Math.Max(previous.Low, current.Low);
                var smallerRange = Math.Min(previous.High - previous.Low, current.High - current.Low);
                if (overlap > 0 && smallerRange > 0)
                    total += Clamp01(overlap / smallerRange);
            }

            return total / (bars.Count - 1);
        }

        private static decimal CalculateDirectionalEfficiency(
            IReadOnlyList<ProfileAccumulator.BarContribution> bars,
            out int directionChanges)
        {
            directionChanges = 0;
            var path = 0m;
            var previousDirection = 0;
            for (var i = 1; i < bars.Count; i++)
            {
                var change = bars[i].Close - bars[i - 1].Close;
                path += Math.Abs(change);
                var direction = Math.Sign(change);
                if (direction == 0)
                    continue;
                if (previousDirection != 0 && direction != previousDirection)
                    directionChanges++;
                previousDirection = direction;
            }

            return path > 0 ? Clamp01(Math.Abs(bars[^1].Close - bars[0].Close) / path) : 0;
        }

        private static decimal PercentileRank(IReadOnlyList<decimal> sortedValues, decimal value)
        {
            if (sortedValues.Count == 0)
                return 1;

            var lessOrEqual = sortedValues.Count(candidate => candidate <= value);
            return Clamp01((decimal)lessOrEqual / sortedValues.Count);
        }

        private static decimal Median(IReadOnlyList<decimal> sortedValues)
        {
            if (sortedValues.Count == 0)
                return 0;

            var middle = sortedValues.Count / 2;
            return sortedValues.Count % 2 == 0
                ? (sortedValues[middle - 1] + sortedValues[middle]) / 2m
                : sortedValues[middle];
        }

        private static decimal Clamp01(decimal value)
        {
            return Math.Max(0, Math.Min(1, value));
        }

        private void UpdateCompressionResolution(ProfileAccumulator.BarContribution latest)
        {
            if (_activeCompressionProfile == null)
                return;

            var profile = _activeCompressionProfile.Reference;
            var tolerance = _activeCompressionProfile.BaselineMedianBarRange * LocalCompressionBoundaryTolerance;
            var outsideDirection = latest.Close > profile.High + tolerance
                ? 1
                : latest.Close < profile.Low - tolerance
                    ? -1
                    : 0;
            if (outsideDirection == 0)
            {
                _compressionOutsideDirection = 0;
                _compressionOutsideCloses = 0;
                return;
            }

            if (_compressionOutsideDirection == outsideDirection)
                _compressionOutsideCloses++;
            else
            {
                _compressionOutsideDirection = outsideDirection;
                _compressionOutsideCloses = 1;
            }

            if (_compressionOutsideCloses < LocalCompressionResolutionPersistenceBars)
                return;

            ResolveActiveCompressionProfile(
                latest.Bar,
                latest.EndTimeUtc,
                latest.Close,
                outsideDirection > 0 ? "ACCEPTANCE_ABOVE_RANGE" : "ACCEPTANCE_BELOW_RANGE");
        }

        private void LogCompressionReady(CompressionProfileSnapshot candidate)
        {
            var reference = candidate.Reference;
            var isHistorical = IsHistoricalContext(candidate.ReadyBar);
            _log($"[MR_LOCAL_PROFILE_READY] ExecutionMode={GetExecutionMode(isHistorical)}, State=READY, ProfileSource={ActiveCompressionProfileSource}, ProfileLabel={reference.Label}, ReadyBar={candidate.ReadyBar}, ReadyTime={FormatTime(candidate.ReadyTimeUtc)}, StartBar={reference.StartBar}, EndBar={reference.EndBar}, Begin={FormatTime(reference.BeginTimeUtc)}, End={FormatTime(reference.EndTimeUtc)}, Bars={reference.Bars}, High={reference.High:F2}, Low={reference.Low:F2}, Range={reference.High - reference.Low:F2}, POC={reference.POC:F2}, VAH={reference.VAH:F2}, VAL={reference.VAL:F2}, CompressionScore={candidate.CompressionScore:F2}, ContractionScore={candidate.ContractionScore:F2}, OverlapScore={candidate.OverlapScore:F2}, DirectionalScore={candidate.DirectionalScore:F2}, RotationScore={candidate.RotationScore:F2}, ContainmentScore={candidate.ContainmentScore:F2}, BoundaryStabilityScore={candidate.BoundaryStabilityScore:F2}, PocStabilityScore={candidate.PocStabilityScore:F2}, ValueConcentrationScore={candidate.ValueConcentrationScore:F2}, BaselineMedianBarRange={candidate.BaselineMedianBarRange:F2}, RangeToBaselineMedian={candidate.RangeToBaselineMedian:F2}, AverageBarRangeToBaselineMedian={candidate.AverageBarRangeToBaselineMedian:F2}, DirectionChanges={candidate.DirectionChanges}, ProfileUse=STUDY_INPUT_ONLY", isHistorical);
        }

        private void DrawDynamicCompressionProfile(CompressionProfileSnapshot snapshot)
        {
            var profile = snapshot.Reference;
            if (!_drawnCompressionProfiles.Add(profile.Label))
                return;

            var fillBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(28, 0, 188, 212));
            var outlinePen = new System.Drawing.Pen(System.Drawing.Color.DeepSkyBlue, 1);
            _chartRectangles.Add(new DrawingRectangle(
                profile.StartBar,
                profile.High,
                profile.EndBar,
                profile.Low,
                outlinePen,
                fillBrush));

            _chartLines.Add(CreateChartLine(profile.StartBar, profile.POC, profile.EndBar, System.Drawing.Color.Turquoise, 2, false));
            _chartLines.Add(CreateChartLine(profile.StartBar, profile.VAH, profile.EndBar, System.Drawing.Color.DarkTurquoise, 1, false, true));
            _chartLines.Add(CreateChartLine(profile.StartBar, profile.VAL, profile.EndBar, System.Drawing.Color.DarkTurquoise, 1, false, true));
        }

        private void UpdateLiveCompressionLedgerOutcomes(int currentBar)
        {
            var resolvedProfiles = _liveCompressionLedgerProfiles
                .Where(profile => profile.ResolvedBar >= profile.ReadyBar)
                .ToList();
            if (resolvedProfiles.Count == 0)
                return;

            RunCompressionLedger(resolvedProfiles, _liveStudyTrades, Math.Max(0, currentBar - 1), false);
        }

        private CompressionLedgerRunResult RunCompressionLedger(
            IReadOnlyList<CompressionProfileSnapshot> profiles,
            IReadOnlyList<CumulativeTrade> trades,
            int observedEndBar,
            bool isHistorical)
        {
            var result = new CompressionLedgerRunResult();
            if (profiles.Count == 0)
                return result;

            var tradesByBar = trades.Count > 0
                ? GroupTradesByBar(trades, observedEndBar)
                : new Dictionary<int, List<CumulativeTrade>>();

            foreach (var profile in profiles.OrderBy(snapshot => snapshot.ReadyBar))
            {
                if (profile.ResolvedBar < profile.ReadyBar)
                    continue;

                var reference = profile.Reference;
                var studyEndBar = Math.Min(profile.ResolvedBar, observedEndBar);
                if (studyEndBar <= profile.ReadyBar)
                    continue;

                var range = Math.Max(reference.High - reference.Low, _tickSize);
                var highTests = 0;
                var lowTests = 0;
                var highOutsideCloseStreak = 0;
                var lowOutsideCloseStreak = 0;
                var profileBuyVolume = 0m;
                var profileSellVolume = 0m;
                var profileCvd = 0m;
                var totalVolumeHistory = new List<decimal>();
                var absoluteDeltaHistory = new List<decimal>();
                var maxBuyHistory = new List<decimal>();
                var maxSellHistory = new List<decimal>();
                var events = new List<CompressionLedgerEvent>();

                for (var bar = profile.ReadyBar + 1; bar <= studyEndBar; bar++)
                {
                    var candle = _getCandle(bar);
                    var stats = BuildCompressionLedgerBarStats(
                        tradesByBar.TryGetValue(bar, out var barTrades) ? barTrades : Array.Empty<CumulativeTrade>());
                    profileBuyVolume += stats.BuyVolume;
                    profileSellVolume += stats.SellVolume;
                    profileCvd += stats.Delta;

                    highOutsideCloseStreak = candle.Close > reference.High ? highOutsideCloseStreak + 1 : 0;
                    lowOutsideCloseStreak = candle.Close < reference.Low ? lowOutsideCloseStreak + 1 : 0;
                    var totalVolumePercentile = GetPriorPercentile(totalVolumeHistory, stats.TotalVolume);
                    var absoluteDeltaPercentile = GetPriorPercentile(absoluteDeltaHistory, Math.Abs(stats.Delta));
                    var maxBuyPercentile = GetPriorPercentile(maxBuyHistory, stats.MaxBuyVolume);
                    var maxSellPercentile = GetPriorPercentile(maxSellHistory, stats.MaxSellVolume);

                    if (candle.High >= reference.High)
                    {
                        highTests++;
                        events.Add(CreateCompressionLedgerEvent(
                            profile,
                            bar,
                            candle,
                            stats,
                            "HIGH",
                            candle.High > reference.High ? "BREACH" : "TOUCH",
                            highTests,
                            highOutsideCloseStreak,
                            range,
                            profileCvd,
                            totalVolumePercentile,
                            absoluteDeltaPercentile,
                            maxBuyPercentile,
                            maxSellPercentile));
                    }

                    if (candle.Low <= reference.Low)
                    {
                        lowTests++;
                        events.Add(CreateCompressionLedgerEvent(
                            profile,
                            bar,
                            candle,
                            stats,
                            "LOW",
                            candle.Low < reference.Low ? "BREACH" : "TOUCH",
                            lowTests,
                            lowOutsideCloseStreak,
                            range,
                            profileCvd,
                            totalVolumePercentile,
                            absoluteDeltaPercentile,
                            maxBuyPercentile,
                            maxSellPercentile));
                    }

                    totalVolumeHistory.Add(stats.TotalVolume);
                    absoluteDeltaHistory.Add(Math.Abs(stats.Delta));
                    maxBuyHistory.Add(stats.MaxBuyVolume);
                    maxSellHistory.Add(stats.MaxSellVolume);
                }

                var profileKey = $"{GetExecutionMode(isHistorical)}:{reference.Label}";
                if (_loggedCompressionLedgerProfiles.Add(profileKey))
                {
                    _log($"[MR_COMPRESSION_LEDGER_PROFILE] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ProfileLabel={reference.Label}, ReadyBar={profile.ReadyBar}, ResolvedBar={profile.ResolvedBar}, EndReason={profile.ResolutionReason}, StudyBars={studyEndBar - profile.ReadyBar}, HighTests={highTests}, LowTests={lowTests}, BoundaryEvents={events.Count}, BuyVolume={profileBuyVolume:F0}, SellVolume={profileSellVolume:F0}, ProfileCVD={profileCvd:F0}, TradeCoverage={(profileBuyVolume + profileSellVolume > 0m ? "AVAILABLE" : "MISSING")}, High={reference.High:F2}, Low={reference.Low:F2}, POC={reference.POC:F2}, Range={range:F2}, RangeToBaselineMedian={profile.RangeToBaselineMedian:F2}, CompressionScore={profile.CompressionScore:F2}, OperationalEntry=FALSE", isHistorical);
                    result.Profiles++;
                }

                foreach (var ledgerEvent in events)
                {
                    var eventKey = $"{GetExecutionMode(isHistorical)}:{reference.Label}:{ledgerEvent.Bar}:{ledgerEvent.Boundary}";
                    if (_loggedCompressionLedgerEvents.Add(eventKey))
                    {
                        LogCompressionLedgerEvent(ledgerEvent, isHistorical);
                        result.Events++;
                    }

                    foreach (var horizon in CompressionLedgerOutcomeHorizons)
                    {
                        if (!TryBuildCompressionLedgerOutcome(ledgerEvent, observedEndBar, horizon, out var outcome) || outcome == null)
                            continue;

                        var outcomeKey = $"{eventKey}:{horizon}";
                        if (_loggedCompressionLedgerOutcomes.Add(outcomeKey))
                        {
                            LogCompressionLedgerOutcome(outcome, isHistorical);
                            result.Outcomes++;
                        }
                    }
                }

                var shadowEvent = events.FirstOrDefault(ledgerEvent =>
                    ledgerEvent.CloseState == "OUTSIDE" && ledgerEvent.OutsideCloseStreak >= 2);
                if (shadowEvent != null)
                {
                    var shadowKey = $"{GetExecutionMode(isHistorical)}:{reference.Label}:{shadowEvent.Bar}:{shadowEvent.Boundary}";
                    var tradeCoverage = profileBuyVolume + profileSellVolume > 0m ? "AVAILABLE" : "MISSING";
                    if (_loggedShadowAcceptanceEntries.Add(shadowKey))
                    {
                        LogShadowAcceptanceEntry(shadowEvent, tradeCoverage, isHistorical);
                        result.ShadowAcceptanceEntries++;
                    }

                    foreach (var horizon in ShadowAcceptanceOutcomeHorizons)
                    {
                        if (!TryBuildCompressionLedgerOutcome(shadowEvent, observedEndBar, horizon, out var outcome) || outcome == null)
                            continue;

                        if (_loggedShadowAcceptanceOutcomes.Add($"{shadowKey}:{horizon}"))
                        {
                            LogShadowAcceptanceOutcome(outcome, tradeCoverage, isHistorical);
                            result.ShadowAcceptanceOutcomes++;
                        }
                    }

                    result.ShadowAcceptancePathBars += LogShadowAcceptancePath(
                        shadowEvent,
                        shadowKey,
                        observedEndBar,
                        tradesByBar,
                        tradeCoverage,
                        isHistorical);
                }
            }

            return result;
        }

        private CompressionLedgerBarStats BuildCompressionLedgerBarStats(IReadOnlyList<CumulativeTrade> trades)
        {
            var stats = new CompressionLedgerBarStats();
            foreach (var trade in trades)
            {
                stats.TradeCount++;
                stats.TotalVolume += trade.Volume;
                if (trade.Direction == TradeDirection.Buy)
                {
                    stats.BuyVolume += trade.Volume;
                    stats.Delta += trade.Volume;
                    stats.MaxBuyVolume = Math.Max(stats.MaxBuyVolume, trade.Volume);
                }
                else if (trade.Direction == TradeDirection.Sell)
                {
                    stats.SellVolume += trade.Volume;
                    stats.Delta -= trade.Volume;
                    stats.MaxSellVolume = Math.Max(stats.MaxSellVolume, trade.Volume);
                }
            }

            return stats;
        }

        private CompressionLedgerEvent CreateCompressionLedgerEvent(
            CompressionProfileSnapshot profile,
            int bar,
            IndicatorCandle candle,
            CompressionLedgerBarStats stats,
            string boundary,
            string interaction,
            int testOrdinal,
            int outsideCloseStreak,
            decimal range,
            decimal profileCvd,
            decimal? totalVolumePercentile,
            decimal? absoluteDeltaPercentile,
            decimal? maxBuyPercentile,
            decimal? maxSellPercentile)
        {
            var reference = profile.Reference;
            var boundaryPrice = boundary == "HIGH" ? reference.High : reference.Low;
            var breachDistance = boundary == "HIGH"
                ? Math.Max(0m, candle.High - boundaryPrice)
                : Math.Max(0m, boundaryPrice - candle.Low);
            var closeDistance = boundary == "HIGH"
                ? candle.Close - boundaryPrice
                : boundaryPrice - candle.Close;
            var closeState = closeDistance > 0m ? "OUTSIDE" : candle.Close >= reference.Low && candle.Close <= reference.High ? "INSIDE" : "OPPOSITE_OUTSIDE";
            var baseline = Math.Max(profile.BaselineMedianBarRange, _tickSize);

            return new CompressionLedgerEvent
            {
                Profile = profile,
                Bar = bar,
                EventTimeUtc = GetCandleEventTime(candle),
                Boundary = boundary,
                Interaction = interaction,
                TestOrdinal = testOrdinal,
                OutsideCloseStreak = outsideCloseStreak,
                EventClose = candle.Close,
                BoundaryPrice = boundaryPrice,
                BreachDistanceRanges = breachDistance / range,
                CloseDistanceRanges = closeDistance / range,
                BarRangeToBaselineMedian = (candle.High - candle.Low) / baseline,
                CloseState = closeState,
                TradeCount = stats.TradeCount,
                TotalVolume = stats.TotalVolume,
                BuyVolume = stats.BuyVolume,
                SellVolume = stats.SellVolume,
                Delta = stats.Delta,
                ProfileCvd = profileCvd,
                MaxBuyVolume = stats.MaxBuyVolume,
                MaxSellVolume = stats.MaxSellVolume,
                TotalVolumePercentilePrior = totalVolumePercentile,
                AbsoluteDeltaPercentilePrior = absoluteDeltaPercentile,
                MaxBuyPercentilePrior = maxBuyPercentile,
                MaxSellPercentilePrior = maxSellPercentile
            };
        }

        private void LogCompressionLedgerEvent(CompressionLedgerEvent ledgerEvent, bool isHistorical)
        {
            var diagnosticState = ledgerEvent.CloseState switch
            {
                "OUTSIDE" when ledgerEvent.OutsideCloseStreak >= 2 => "OUTSIDE_ACCEPTANCE",
                "OUTSIDE" => "OUTSIDE_FIRST",
                "INSIDE" => "INSIDE_BOUNDARY_INTERACTION",
                _ => "OPPOSITE_OUTSIDE"
            };
            _log($"[MR_COMPRESSION_LEDGER_EVENT] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ProfileLabel={ledgerEvent.Profile.Reference.Label}, Bar={ledgerEvent.Bar}, {FormatTime(ledgerEvent.EventTimeUtc)}, Boundary={ledgerEvent.Boundary}, Interaction={ledgerEvent.Interaction}, DiagnosticState={diagnosticState}, TestOrdinal={ledgerEvent.TestOrdinal}, OutsideCloseStreak={ledgerEvent.OutsideCloseStreak}, BoundaryPrice={ledgerEvent.BoundaryPrice:F2}, EventClose={ledgerEvent.EventClose:F2}, CloseState={ledgerEvent.CloseState}, BreachDistanceRanges={ledgerEvent.BreachDistanceRanges:F2}, CloseDistanceRanges={ledgerEvent.CloseDistanceRanges:F2}, BarRangeToBaselineMedian={ledgerEvent.BarRangeToBaselineMedian:F2}, TradeCount={ledgerEvent.TradeCount}, TotalVolume={ledgerEvent.TotalVolume:F0}, BuyVolume={ledgerEvent.BuyVolume:F0}, SellVolume={ledgerEvent.SellVolume:F0}, Delta={ledgerEvent.Delta:F0}, ProfileCVD={ledgerEvent.ProfileCvd:F0}, MaxBuyVolume={ledgerEvent.MaxBuyVolume:F0}, MaxSellVolume={ledgerEvent.MaxSellVolume:F0}, TotalVolumePercentilePrior={FormatLedgerPercentile(ledgerEvent.TotalVolumePercentilePrior)}, AbsoluteDeltaPercentilePrior={FormatLedgerPercentile(ledgerEvent.AbsoluteDeltaPercentilePrior)}, MaxBuyPercentilePrior={FormatLedgerPercentile(ledgerEvent.MaxBuyPercentilePrior)}, MaxSellPercentilePrior={FormatLedgerPercentile(ledgerEvent.MaxSellPercentilePrior)}, OperationalEntry=FALSE", isHistorical);
        }

        private bool TryBuildCompressionLedgerOutcome(
            CompressionLedgerEvent ledgerEvent,
            int observedEndBar,
            int horizon,
            out CompressionLedgerOutcome? outcome)
        {
            outcome = null;
            var endBar = ledgerEvent.Bar + horizon;
            if (endBar > observedEndBar || endBar >= _currentBar)
                return false;

            var reference = ledgerEvent.Profile.Reference;
            var range = Math.Max(reference.High - reference.Low, _tickSize);
            var highest = ledgerEvent.EventClose;
            var lowest = ledgerEvent.EventClose;
            var pocTouched = false;
            for (var bar = ledgerEvent.Bar + 1; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                highest = Math.Max(highest, candle.High);
                lowest = Math.Min(lowest, candle.Low);
                if (candle.Low <= reference.POC && candle.High >= reference.POC)
                    pocTouched = true;
            }

            var endCandle = _getCandle(endBar);
            outcome = new CompressionLedgerOutcome
            {
                Event = ledgerEvent,
                Horizon = horizon,
                EndBar = endBar,
                EndTimeUtc = GetCandleEventTime(endCandle),
                EndClose = endCandle.Close,
                CloseMoveRanges = (endCandle.Close - ledgerEvent.EventClose) / range,
                UpMfeRanges = (highest - ledgerEvent.EventClose) / range,
                DownMaeRanges = (ledgerEvent.EventClose - lowest) / range,
                EndInsideRange = endCandle.Close >= reference.Low && endCandle.Close <= reference.High,
                PocTouched = pocTouched
            };
            return true;
        }

        private void LogCompressionLedgerOutcome(CompressionLedgerOutcome outcome, bool isHistorical)
        {
            _log($"[MR_COMPRESSION_LEDGER_OUTCOME] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ProfileLabel={outcome.Event.Profile.Reference.Label}, EventBar={outcome.Event.Bar}, Boundary={outcome.Event.Boundary}, Interaction={outcome.Event.Interaction}, HorizonBars={outcome.Horizon}, EndBar={outcome.EndBar}, {FormatTime(outcome.EndTimeUtc)}, EventClose={outcome.Event.EventClose:F2}, EndClose={outcome.EndClose:F2}, CloseMoveRanges={outcome.CloseMoveRanges:F2}, UpMfeRanges={outcome.UpMfeRanges:F2}, DownMaeRanges={outcome.DownMaeRanges:F2}, EndInsideRange={outcome.EndInsideRange}, PocTouched={outcome.PocTouched}, OperationalEntry=FALSE", isHistorical);
        }

        private int LogShadowAcceptancePath(
            CompressionLedgerEvent entry,
            string shadowKey,
            int observedEndBar,
            IReadOnlyDictionary<int, List<CumulativeTrade>> tradesByBar,
            string tradeCoverage,
            bool isHistorical)
        {
            var reference = entry.Profile.Reference;
            var range = Math.Max(reference.High - reference.Low, _tickSize);
            var isLong = entry.Boundary == "HIGH";
            var highest = entry.EventClose;
            var lowest = entry.EventClose;
            var pocTouchedToDate = false;
            var logged = 0;
            var maxBar = Math.Min(observedEndBar, _currentBar - 1);
            for (var bar = entry.Bar + 1; bar <= maxBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTimeUtc = GetCandleEventTime(candle);
                var elapsedMinutes = (eventTimeUtc - entry.EventTimeUtc).TotalMinutes;
                if (elapsedMinutes <= 0)
                    continue;
                if (elapsedMinutes > ShadowAcceptancePathMinutes + ShadowAcceptancePathCompletionToleranceMinutes)
                    break;

                highest = Math.Max(highest, candle.High);
                lowest = Math.Min(lowest, candle.Low);
                var pocTouchedThisBar = candle.Low <= reference.POC && candle.High >= reference.POC;
                pocTouchedToDate |= pocTouchedThisBar;
                var closeMove = (candle.Close - entry.EventClose) / range;
                var directionalMove = isLong ? closeMove : -closeMove;
                var favorableMfe = isLong
                    ? (highest - entry.EventClose) / range
                    : (entry.EventClose - lowest) / range;
                var adverseMfe = isLong
                    ? (entry.EventClose - lowest) / range
                    : (highest - entry.EventClose) / range;
                var priceState = candle.Close > reference.High
                    ? "ABOVE_RANGE"
                    : candle.Close < reference.Low
                        ? "BELOW_RANGE"
                        : "INSIDE_RANGE";
                var stats = BuildCompressionLedgerBarStats(
                    tradesByBar.TryGetValue(bar, out var barTrades) ? barTrades : Array.Empty<CumulativeTrade>());
                if (!_loggedShadowAcceptancePathBars.Add($"{shadowKey}:{bar}"))
                    continue;

                var direction = isLong ? "LONG" : "SHORT";
                var shadowId = $"{reference.Label}:{entry.Bar}:{entry.Boundary}";
                _log($"[MR_SHADOW_ACCEPTANCE_BAR] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ShadowModel={ShadowAcceptanceModel}, ShadowId={shadowId}, ProfileLabel={reference.Label}, EntryBar={entry.Bar}, Boundary={entry.Boundary}, Direction={direction}, ChartTimeFrame={_chartTimeFrame}, PathBar={bar}, PathBarOrdinal={bar - entry.Bar}, ElapsedMinutes={elapsedMinutes:F2}, {FormatTime(eventTimeUtc)}, Open={candle.Open:F2}, High={candle.High:F2}, Low={candle.Low:F2}, Close={candle.Close:F2}, CandleVolume={candle.Volume:F0}, DirectionalMoveRanges={directionalMove:F2}, FavorableMfeToDateRanges={favorableMfe:F2}, AdverseMfeToDateRanges={adverseMfe:F2}, PriceState={priceState}, PocTouchedThisBar={pocTouchedThisBar}, PocTouchedToDate={pocTouchedToDate}, TradeCount={stats.TradeCount}, TotalVolume={stats.TotalVolume:F0}, BuyVolume={stats.BuyVolume:F0}, SellVolume={stats.SellVolume:F0}, Delta={stats.Delta:F0}, MaxBuyVolume={stats.MaxBuyVolume:F0}, MaxSellVolume={stats.MaxSellVolume:F0}, TradeCoverage={tradeCoverage}, OperationalEntry=FALSE, OrderSubmitted=FALSE", isHistorical);
                logged++;
                if (elapsedMinutes >= ShadowAcceptancePathMinutes)
                    break;
            }

            return logged;
        }

        private void LogShadowAcceptanceEntry(CompressionLedgerEvent entry, string tradeCoverage, bool isHistorical)
        {
            var reference = entry.Profile.Reference;
            var direction = entry.Boundary == "HIGH" ? "LONG" : "SHORT";
            var shadowId = $"{reference.Label}:{entry.Bar}:{entry.Boundary}";
            _log($"[MR_SHADOW_ACCEPTANCE_ENTRY] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ShadowModel={ShadowAcceptanceModel}, ShadowId={shadowId}, ProfileLabel={reference.Label}, EntryBar={entry.Bar}, {FormatTime(entry.EventTimeUtc)}, Boundary={entry.Boundary}, Direction={direction}, ChartTimeFrame={_chartTimeFrame}, EntryPrice={entry.EventClose:F2}, Trigger=SECOND_CONSECUTIVE_OUTSIDE_CLOSE, OutsideCloseStreak={entry.OutsideCloseStreak}, High={reference.High:F2}, Low={reference.Low:F2}, POC={reference.POC:F2}, Range={Math.Max(reference.High - reference.Low, _tickSize):F2}, TradeCoverage={tradeCoverage}, TotalVolumePercentilePrior={FormatLedgerPercentile(entry.TotalVolumePercentilePrior)}, AbsoluteDeltaPercentilePrior={FormatLedgerPercentile(entry.AbsoluteDeltaPercentilePrior)}, OperationalEntry=FALSE, OrderSubmitted=FALSE", isHistorical);
        }

        private void LogShadowAcceptanceOutcome(CompressionLedgerOutcome outcome, string tradeCoverage, bool isHistorical)
        {
            var entry = outcome.Event;
            var reference = entry.Profile.Reference;
            var isLong = entry.Boundary == "HIGH";
            var direction = isLong ? "LONG" : "SHORT";
            var directionalMove = isLong ? outcome.CloseMoveRanges : -outcome.CloseMoveRanges;
            var favorableMfe = isLong ? outcome.UpMfeRanges : outcome.DownMaeRanges;
            var adverseMfe = isLong ? outcome.DownMaeRanges : outcome.UpMfeRanges;
            var shadowId = $"{reference.Label}:{entry.Bar}:{entry.Boundary}";
            _log($"[MR_SHADOW_ACCEPTANCE_OUTCOME] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ShadowModel={ShadowAcceptanceModel}, ShadowId={shadowId}, ProfileLabel={reference.Label}, EntryBar={entry.Bar}, Boundary={entry.Boundary}, Direction={direction}, ChartTimeFrame={_chartTimeFrame}, HorizonBars={outcome.Horizon}, ElapsedMinutes={(outcome.EndTimeUtc - entry.EventTimeUtc).TotalMinutes:F2}, EndBar={outcome.EndBar}, {FormatTime(outcome.EndTimeUtc)}, EntryPrice={entry.EventClose:F2}, EndClose={outcome.EndClose:F2}, DirectionalMoveRanges={directionalMove:F2}, FavorableMfeRanges={favorableMfe:F2}, AdverseMfeRanges={adverseMfe:F2}, EndInsideRange={outcome.EndInsideRange}, PocTouched={outcome.PocTouched}, TradeCoverage={tradeCoverage}, OperationalEntry=FALSE, OrderSubmitted=FALSE", isHistorical);
        }

        private static decimal? GetPriorPercentile(IReadOnlyList<decimal> history, decimal value)
        {
            if (history.Count == 0 || (value == 0m && history.All(observation => observation == 0m)))
                return null;

            return PercentileRank(history, value);
        }

        private static string FormatLedgerPercentile(decimal? value)
        {
            return value.HasValue ? value.Value.ToString("F2") : "NA";
        }

        private int RunCompressionStudy(
            IReadOnlyList<CompressionProfileSnapshot> profiles,
            IReadOnlyList<CumulativeTrade> trades,
            int fallbackEndBar,
            bool isHistorical)
        {
            if (profiles.Count == 0 || trades.Count == 0)
                return 0;

            var tradesByBar = GroupTradesByBar(trades, fallbackEndBar);
            var candidates = 0;
            foreach (var profile in profiles.OrderBy(snapshot => snapshot.ReadyBar))
                candidates += StudyCompressionProfile(profile, tradesByBar, fallbackEndBar, isHistorical);

            return candidates;
        }

        private Dictionary<int, List<CumulativeTrade>> GroupTradesByBar(IReadOnlyList<CumulativeTrade> trades, int endBar)
        {
            var result = new Dictionary<int, List<CumulativeTrade>>();
            var lastBar = Math.Min(Math.Max(endBar, 0), Math.Max(_currentBar - 1, 0));
            var bar = 0;
            foreach (var trade in trades)
            {
                while (bar < lastBar && trade.Time > GetCandleEventTime(_getCandle(bar)))
                    bar++;

                var candle = _getCandle(bar);
                if (trade.Time < candle.Time || trade.Time > GetCandleEventTime(candle))
                    continue;

                if (!result.TryGetValue(bar, out var barTrades))
                {
                    barTrades = new List<CumulativeTrade>();
                    result[bar] = barTrades;
                }

                barTrades.Add(trade);
            }

            return result;
        }

        private int StudyCompressionProfile(
            CompressionProfileSnapshot profile,
            IReadOnlyDictionary<int, List<CumulativeTrade>> tradesByBar,
            int fallbackEndBar,
            bool isHistorical)
        {
            var reference = profile.Reference;
            var studyEndBar = profile.ResolvedBar >= profile.ReadyBar
                ? profile.ResolvedBar
                : Math.Min(fallbackEndBar, _currentBar - 1);
            if (studyEndBar <= profile.ReadyBar)
                return 0;

            var tolerance = Math.Max(_tickSize * 2m, profile.BaselineMedianBarRange * LocalCompressionBoundaryTolerance);
            var highTests = 0;
            var lowTests = 0;
            var highBoundaryBuyVolume = 0m;
            var lowBoundarySellVolume = 0m;
            var profileCvd = 0m;
            var candidates = 0;
            CompressionStudyBoundaryState? pendingHighAbsorption = null;
            CompressionStudyBoundaryState? pendingLowAbsorption = null;
            var statsByBar = new Dictionary<int, CompressionStudyBarStats>();

            for (var bar = profile.ReadyBar + 1; bar <= studyEndBar; bar++)
            {
                var candle = _getCandle(bar);
                var stats = BuildCompressionStudyBarStats(
                    tradesByBar.TryGetValue(bar, out var barTrades) ? barTrades : Array.Empty<CumulativeTrade>(),
                    reference.High,
                    reference.Low,
                    tolerance);
                statsByBar[bar] = stats;
                profileCvd += stats.Delta;

                var testsHigh = candle.High >= reference.High - tolerance;
                var testsLow = candle.Low <= reference.Low + tolerance;
                if (testsHigh)
                    highTests++;
                if (testsLow)
                    lowTests++;
                highBoundaryBuyVolume += stats.HighBoundaryBuyVolume;
                lowBoundarySellVolume += stats.LowBoundarySellVolume;

                var highAbsorbed = candle.High > reference.High + tolerance
                    && stats.HighBoundaryBuyVolume >= LondonBigTradeVolume
                    && candle.Close < reference.High - tolerance;
                if (highAbsorbed)
                {
                    pendingHighAbsorption = new CompressionStudyBoundaryState
                    {
                        Bar = bar,
                        TimeUtc = GetCandleEventTime(candle),
                        AggressionVolume = stats.HighBoundaryBuyVolume
                    };
                }

                var lowAbsorbed = candle.Low < reference.Low - tolerance
                    && stats.LowBoundarySellVolume >= LondonBigTradeVolume
                    && candle.Close > reference.Low + tolerance;
                if (lowAbsorbed)
                {
                    pendingLowAbsorption = new CompressionStudyBoundaryState
                    {
                        Bar = bar,
                        TimeUtc = GetCandleEventTime(candle),
                        AggressionVolume = stats.LowBoundarySellVolume
                    };
                }

                if (pendingHighAbsorption != null)
                {
                    if (bar - pendingHighAbsorption.Bar > CompressionStudyConfirmationBars)
                    {
                        pendingHighAbsorption = null;
                    }
                    else if (stats.SellVolume >= LondonBigTradeVolume && candle.Close < reference.High - tolerance)
                    {
                        candidates += EmitCompressionStudyCandidate(
                            profile,
                            "REVERSION_SHORT",
                            "Short",
                            bar,
                            stats.LastSellTimeUtc ?? GetCandleEventTime(candle),
                            stats.LastSellPrice ?? candle.Close,
                            reference.High - _tickSize * StopInsideFailedExtremeTicks,
                            reference.POC,
                            "ABSORBED_BUY_AGGRESSION_PLUS_SELL_CONFIRMATION",
                            pendingHighAbsorption.AggressionVolume,
                            stats.Delta,
                            profileCvd,
                            isHistorical);
                        pendingHighAbsorption = null;
                    }
                }

                if (pendingLowAbsorption != null)
                {
                    if (bar - pendingLowAbsorption.Bar > CompressionStudyConfirmationBars)
                    {
                        pendingLowAbsorption = null;
                    }
                    else if (stats.BuyVolume >= LondonBigTradeVolume && candle.Close > reference.Low + tolerance)
                    {
                        candidates += EmitCompressionStudyCandidate(
                            profile,
                            "REVERSION_LONG",
                            "Long",
                            bar,
                            stats.LastBuyTimeUtc ?? GetCandleEventTime(candle),
                            stats.LastBuyPrice ?? candle.Close,
                            reference.Low + _tickSize * StopInsideFailedExtremeTicks,
                            reference.POC,
                            "ABSORBED_SELL_AGGRESSION_PLUS_BUY_CONFIRMATION",
                            pendingLowAbsorption.AggressionVolume,
                            stats.Delta,
                            profileCvd,
                            isHistorical);
                        pendingLowAbsorption = null;
                    }
                }
            }

            candidates += EmitBreakoutAcceptanceStudyCandidate(profile, statsByBar, studyEndBar, profileCvd, tolerance, isHistorical);
            var endReason = string.IsNullOrEmpty(profile.ResolutionReason) ? "REPLAY_END" : profile.ResolutionReason;
            _log($"[MR_COMPRESSION_STUDY_PROFILE] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ProfileLabel={reference.Label}, ReadyBar={profile.ReadyBar}, EndBar={studyEndBar}, EndReason={endReason}, HighTests={highTests}, LowTests={lowTests}, HighBoundaryBuyVolume={highBoundaryBuyVolume:F0}, LowBoundarySellVolume={lowBoundarySellVolume:F0}, ProfileCVD={profileCvd:F0}, Candidates={candidates}, POC={reference.POC:F2}, High={reference.High:F2}, Low={reference.Low:F2}, CompressionScore={profile.CompressionScore:F2}", isHistorical);
            return candidates;
        }

        private CompressionStudyBarStats BuildCompressionStudyBarStats(
            IReadOnlyList<CumulativeTrade> trades,
            decimal high,
            decimal low,
            decimal tolerance)
        {
            var stats = new CompressionStudyBarStats();
            foreach (var trade in trades)
            {
                if (trade.Direction == TradeDirection.Buy)
                {
                    stats.BuyVolume += trade.Volume;
                    stats.Delta += trade.Volume;
                    if (trade.Volume >= LondonBigTradeVolume)
                    {
                        stats.LastBuyPrice = trade.Lastprice;
                        stats.LastBuyTimeUtc = trade.Time;
                        if (trade.Lastprice >= high - tolerance)
                            stats.HighBoundaryBuyVolume += trade.Volume;
                    }
                }
                else if (trade.Direction == TradeDirection.Sell)
                {
                    stats.SellVolume += trade.Volume;
                    stats.Delta -= trade.Volume;
                    if (trade.Volume >= LondonBigTradeVolume)
                    {
                        stats.LastSellPrice = trade.Lastprice;
                        stats.LastSellTimeUtc = trade.Time;
                        if (trade.Lastprice <= low + tolerance)
                            stats.LowBoundarySellVolume += trade.Volume;
                    }
                }
            }

            return stats;
        }

        private int EmitBreakoutAcceptanceStudyCandidate(
            CompressionProfileSnapshot profile,
            IReadOnlyDictionary<int, CompressionStudyBarStats> statsByBar,
            int studyEndBar,
            decimal profileCvd,
            decimal tolerance,
            bool isHistorical)
        {
            if (profile.ResolvedBar < profile.ReadyBar || !profile.ResolutionReason.StartsWith("ACCEPTANCE_", StringComparison.Ordinal))
                return 0;

            var reference = profile.Reference;
            var resolutionCandle = _getCandle(profile.ResolvedBar);
            var currentStats = statsByBar.TryGetValue(profile.ResolvedBar, out var value)
                ? value
                : new CompressionStudyBarStats();
            var previousStats = statsByBar.TryGetValue(profile.ResolvedBar - 1, out var previous)
                ? previous
                : new CompressionStudyBarStats();
            var acceptanceDelta = currentStats.Delta + previousStats.Delta;
            var up = profile.ResolutionReason == "ACCEPTANCE_ABOVE_RANGE";
            var directionalBoundaryVolume = up
                ? currentStats.HighBoundaryBuyVolume + previousStats.HighBoundaryBuyVolume
                : currentStats.LowBoundarySellVolume + previousStats.LowBoundarySellVolume;
            var qualified = directionalBoundaryVolume >= LondonBigTradeVolume && (up ? acceptanceDelta > 0 : acceptanceDelta < 0);
            _log($"[MR_COMPRESSION_STUDY_CASE] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ProfileLabel={reference.Label}, Case={(up ? "BREAKOUT_ACCEPTANCE_UP" : "BREAKOUT_ACCEPTANCE_DOWN")}, Bar={profile.ResolvedBar}, {FormatTime(GetCandleEventTime(resolutionCandle))}, Qualified={qualified}, DirectionalBoundaryVolume={directionalBoundaryVolume:F0}, AcceptanceCVD={acceptanceDelta:F0}, ProfileCVD={profileCvd:F0}, High={reference.High:F2}, Low={reference.Low:F2}, Tolerance={tolerance:F2}, Retest=NOT_EVALUATED", isHistorical);
            if (!qualified)
                return 0;

            return EmitCompressionStudyCandidate(
                profile,
                up ? "BREAKOUT_LONG" : "BREAKOUT_SHORT",
                up ? "Long" : "Short",
                profile.ResolvedBar,
                up ? currentStats.LastBuyTimeUtc ?? GetCandleEventTime(resolutionCandle) : currentStats.LastSellTimeUtc ?? GetCandleEventTime(resolutionCandle),
                up ? currentStats.LastBuyPrice ?? resolutionCandle.Close : currentStats.LastSellPrice ?? resolutionCandle.Close,
                up ? reference.High - _tickSize * StopInsideFailedExtremeTicks : reference.Low + _tickSize * StopInsideFailedExtremeTicks,
                0,
                "TWO_CLOSE_ACCEPTANCE_PLUS_DIRECTIONAL_BIG_TRADE_AND_CVD",
                directionalBoundaryVolume,
                acceptanceDelta,
                profileCvd,
                isHistorical);
        }

        private int EmitCompressionStudyCandidate(
            CompressionProfileSnapshot profile,
            string candidateType,
            string direction,
            int bar,
            DateTime eventTimeUtc,
            decimal entryPrice,
            decimal stopPrice,
            decimal targetPrice,
            string evidence,
            decimal boundaryAggressionVolume,
            decimal eventCvd,
            decimal profileCvd,
            bool isHistorical)
        {
            var key = $"{profile.Reference.Label}:{candidateType}:{bar}:{entryPrice:F2}";
            if (!_drawnCompressionStudyCandidates.Add(key))
                return 0;

            var targetModel = targetPrice > 0 ? "COMPRESSION_POC" : "UNDEFINED_REQUIRES_SEPARATE_MODEL";
            _log($"[MR_COMPRESSION_STUDY_CANDIDATE] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, CandidateType={candidateType}, Direction={direction}, ProfileLabel={profile.Reference.Label}, Bar={bar}, {FormatTime(eventTimeUtc)}, EntryCandidate={entryPrice:F2}, StopCandidate={stopPrice:F2}, TargetCandidate={(targetPrice > 0 ? targetPrice.ToString("F2") : "NA")}, TargetModel={targetModel}, Evidence={evidence}, BoundaryAggressionVolume={boundaryAggressionVolume:F0}, EventCVD={eventCvd:F0}, ProfileCVD={profileCvd:F0}, OperationalEntry=FALSE", isHistorical);
            DrawCompressionStudyCandidate(candidateType, bar, entryPrice, targetPrice);
            return 1;
        }

        private void DrawCompressionStudyCandidate(string candidateType, int bar, decimal entryPrice, decimal targetPrice)
        {
            var color = candidateType switch
            {
                "REVERSION_LONG" => System.Drawing.Color.LimeGreen,
                "REVERSION_SHORT" => System.Drawing.Color.MediumVioletRed,
                "BREAKOUT_LONG" => System.Drawing.Color.DodgerBlue,
                _ => System.Drawing.Color.DarkOrange
            };
            _chartRectangles.Add(CreateTradeMarker(bar, entryPrice, color, 170));
            _chartLines.Add(CreateChartLine(bar, entryPrice, bar + 3, color, 2, false));
            if (targetPrice > 0)
                _chartLines.Add(CreateChartLine(bar, targetPrice, bar + 3, color, 1, false, true));
        }

        private void DrawTradeOpened(ActivePosition position)
        {
            if (_tradeChartVisuals.ContainsKey(position.SetupId))
                return;

            var entryColor = position.Direction == "Long" ? System.Drawing.Color.LimeGreen : System.Drawing.Color.OrangeRed;
            var liveEndBar = Math.Max(position.EntryBar + 1, _currentBar);
            var entryLine = CreateChartLine(position.EntryBar, position.EntryPrice, liveEndBar, entryColor, 2, true);
            var stopLine = CreateChartLine(position.EntryBar, position.OriginalStopPrice, liveEndBar, System.Drawing.Color.Crimson, 1, true, true);
            var targetLine = CreateChartLine(position.EntryBar, position.TargetPrice, liveEndBar, System.Drawing.Color.DodgerBlue, 1, true, true);
            _chartLines.Add(entryLine);
            _chartLines.Add(stopLine);
            _chartLines.Add(targetLine);
            _chartRectangles.Add(CreateTradeMarker(position.EntryBar, position.EntryPrice, entryColor, 110));

            _tradeChartVisuals[position.SetupId] = new TradeChartVisual
            {
                EntryLine = entryLine,
                StopLine = stopLine,
                TargetLine = targetLine
            };
        }

        private void DrawTradeClosed(ActivePosition position, decimal pnl)
        {
            var closeBar = Math.Max(position.EntryBar + 3, position.ExitBar);
            if (_tradeChartVisuals.TryGetValue(position.SetupId, out var visual))
            {
                visual.EntryLine.IsRay = false;
                visual.EntryLine.SecondBar = closeBar;
                visual.StopLine.IsRay = false;
                visual.StopLine.SecondBar = closeBar;
                visual.TargetLine.IsRay = false;
                visual.TargetLine.SecondBar = closeBar;
            }

            var exitColor = pnl > 0 ? System.Drawing.Color.LimeGreen : pnl < 0 ? System.Drawing.Color.Crimson : System.Drawing.Color.Gold;
            _chartRectangles.Add(CreateTradeMarker(position.ExitBar, position.ExitPrice, exitColor, 130));
            _chartLines.Add(CreateChartLine(position.ExitBar, position.ExitPrice, position.ExitBar + 3, exitColor, 2, false));
        }

        private DrawingRectangle CreateTradeMarker(int bar, decimal price, System.Drawing.Color color, int alpha)
        {
            var markerHalfHeight = Math.Max(_tickSize * 5m, _tickSize);
            var markerStartBar = Math.Max(0, bar - 1);
            return new DrawingRectangle(
                markerStartBar,
                price + markerHalfHeight,
                bar + 2,
                price - markerHalfHeight,
                new System.Drawing.Pen(color, 1),
                new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(alpha, color)));
        }

        private static LineTillTouch CreateChartLine(
            int startBar,
            decimal price,
            int endBar,
            System.Drawing.Color color,
            int width,
            bool isRay,
            bool dashed = false)
        {
            var pen = new System.Drawing.Pen(color, width);
            if (dashed)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            return new LineTillTouch(startBar, price, pen)
            {
                IsRay = isRay,
                SecondBar = Math.Max(startBar + 1, endBar)
            };
        }

        private void ResolveActiveCompressionProfile(int bar, DateTime eventTimeUtc, decimal close, string reason)
        {
            if (_activeCompressionProfile == null)
                return;

            var resolvedProfile = _activeCompressionProfile;
            var reference = resolvedProfile.Reference;
            var isHistorical = IsHistoricalContext(bar);
            resolvedProfile.ResolvedBar = bar;
            resolvedProfile.ResolvedTimeUtc = eventTimeUtc;
            resolvedProfile.ResolutionReason = reason;
            _log($"[MR_LOCAL_PROFILE_RESOLVED] ExecutionMode={GetExecutionMode(isHistorical)}, State=RESOLVED, ProfileSource={ActiveCompressionProfileSource}, ProfileLabel={reference.Label}, ResolvedBar={bar}, ResolvedTime={FormatTime(eventTimeUtc)}, Reason={reason}, ResolutionCloses={_compressionOutsideCloses}, Close={close:F2}, High={reference.High:F2}, Low={reference.Low:F2}, POC={reference.POC:F2}, ProfileUse=STUDY_INPUT_ONLY", isHistorical);
            if (!isHistorical)
            {
                _liveCompressionLedgerProfiles.Add(resolvedProfile);
                RunCompressionLedger(new[] { resolvedProfile }, _liveStudyTrades, bar, false);
            }

            _activeCompressionProfile = null;
            _compressionOutsideDirection = 0;
            _compressionOutsideCloses = 0;
        }

        private void ResetCompressionLifecycle()
        {
            _buildingCompressionProfile = null;
            _activeCompressionProfile = null;
            _lastCompressionEvaluatedBar = -1;
            _compressionOutsideDirection = 0;
            _compressionOutsideCloses = 0;
        }

        private bool TryBuildReferenceFromContributions(string source, string label, IReadOnlyList<ProfileAccumulator.BarContribution> bars, out ReferenceValueArea? reference)
        {
            reference = null;
            if (bars.Count == 0)
                return false;

            var profile = new Dictionary<decimal, decimal>();
            foreach (var bar in bars)
            {
                foreach (var level in bar.Levels)
                {
                    if (profile.ContainsKey(level.Key))
                        profile[level.Key] += level.Value;
                    else
                        profile[level.Key] = level.Value;
                }
            }

            var totalVolume = profile.Values.Sum();
            if (totalVolume <= 0)
                return false;

            if (!TryCalculateValueArea(profile, totalVolume, out var poc, out var vah, out var val, out var valueAreaVolume, out var maxVolume))
                return false;

            reference = new ReferenceValueArea
            {
                Source = source,
                Label = label,
                POC = poc,
                VAH = vah,
                VAL = val,
                High = bars.Max(b => b.High),
                Low = bars.Min(b => b.Low),
                TotalVolume = totalVolume,
                ValueAreaVolume = valueAreaVolume,
                MaxLevelVolume = maxVolume,
                StartBar = bars.Min(b => b.Bar),
                EndBar = bars.Max(b => b.Bar),
                BeginTimeUtc = bars.Min(b => b.BeginTimeUtc),
                EndTimeUtc = bars.Max(b => b.EndTimeUtc),
                Bars = bars.Count
            };
            return true;
        }

        private void LogProfileDiagnosticsContext(BalanceSetup setup, string context, int bar, DateTime eventTimeUtc, bool isHistorical, decimal? candidateEntry)
        {
            if (!setup.HasActiveCompressionProfileContext)
                return;

            LogProfileContextLine(
                setup,
                context,
                bar,
                eventTimeUtc,
                isHistorical,
                candidateEntry,
                ActiveCompressionProfileSource,
                setup.ActiveCompressionProfileLabel,
                setup.ActiveCompressionProfileStartBar,
                setup.ActiveCompressionProfileEndBar,
                setup.ActiveCompressionProfileBeginTimeUtc,
                setup.ActiveCompressionProfileEndTimeUtc,
                setup.ActiveCompressionProfileBars,
                setup.ActiveCompressionPOC,
                setup.ActiveCompressionVAH,
                setup.ActiveCompressionVAL,
                setup.ActiveCompressionHigh,
                setup.ActiveCompressionLow,
                setup.ActiveCompressionTotalVolume,
                setup.ActiveCompressionValueAreaVolume,
                setup.ActiveCompressionMaxLevelVolume);
        }

        private void LogProfileContextLine(
            BalanceSetup setup,
            string context,
            int bar,
            DateTime eventTimeUtc,
            bool isHistorical,
            decimal? candidateEntry,
            string profileSource,
            string profileLabel,
            int profileStartBar,
            int profileEndBar,
            DateTime profileBeginTimeUtc,
            DateTime profileEndTimeUtc,
            int profileBars,
            decimal profilePoc,
            decimal profileVah,
            decimal profileVal,
            decimal profileHigh,
            decimal profileLow,
            decimal profileTotalVolume,
            decimal profileValueAreaVolume,
            decimal profileMaxLevelVolume)
        {
            var valueWidth = profileVah - profileVal;
            var targetVsVal = setup.TargetPrice - profileVal;
            var targetVsPoc = setup.TargetPrice - profilePoc;
            var targetVsVah = setup.TargetPrice - profileVah;
            var entryPart = candidateEntry.HasValue
                ? $"CandidateEntry={candidateEntry.Value:F2}, EntryVsProfileVAL={candidateEntry.Value - profileVal:F2}, EntryVsProfilePOC={candidateEntry.Value - profilePoc:F2}, EntryVsProfileVAH={candidateEntry.Value - profileVah:F2}"
                : "CandidateEntry=NA, EntryVsProfileVAL=NA, EntryVsProfilePOC=NA, EntryVsProfileVAH=NA";

            _log($"[MR_PROFILE_CONTEXT] ExecutionMode={GetExecutionMode(isHistorical)}, Context={context}, SetupId={setup.SetupId}, Direction={setup.Direction}, Source={setup.Source}, ReferenceLabel={setup.ReferenceLabel}, Bar={bar}, {FormatTime(eventTimeUtc)}, ProfileSource={profileSource}, ProfileLabel={profileLabel}, ProfileStartBar={profileStartBar}, ProfileEndBar={profileEndBar}, ProfileBegin={FormatTime(profileBeginTimeUtc)}, ProfileEnd={FormatTime(profileEndTimeUtc)}, ProfileReadyBar={setup.ActiveCompressionReadyBar}, ProfileReadyTime={FormatTime(setup.ActiveCompressionReadyTimeUtc)}, ProfileBars={profileBars}, ProfilePOC={profilePoc:F2}, ProfileVAH={profileVah:F2}, ProfileVAL={profileVal:F2}, ProfileValueWidth={valueWidth:F2}, ProfileHigh={profileHigh:F2}, ProfileLow={profileLow:F2}, ProfileRange={profileHigh - profileLow:F2}, ProfileTotalVolume={profileTotalVolume:F0}, ProfileValueAreaVolume={profileValueAreaVolume:F0}, ProfileMaxLevelVolume={profileMaxLevelVolume:F0}, CompressionScore={setup.ActiveCompressionScore:F2}, ContractionScore={setup.ActiveCompressionContractionScore:F2}, OverlapScore={setup.ActiveCompressionOverlapScore:F2}, DirectionalScore={setup.ActiveCompressionDirectionalScore:F2}, RotationScore={setup.ActiveCompressionRotationScore:F2}, ContainmentScore={setup.ActiveCompressionContainmentScore:F2}, BoundaryStabilityScore={setup.ActiveCompressionBoundaryStabilityScore:F2}, PocStabilityScore={setup.ActiveCompressionPocStabilityScore:F2}, ValueConcentrationScore={setup.ActiveCompressionValueConcentrationScore:F2}, BaselineMedianBarRange={setup.ActiveCompressionBaselineMedianBarRange:F2}, RangeToBaselineMedian={setup.ActiveCompressionRangeToBaselineMedian:F2}, AverageBarRangeToBaselineMedian={setup.ActiveCompressionAverageBarRangeToBaselineMedian:F2}, DirectionChanges={setup.ActiveCompressionDirectionChanges}, CandidateTargetPOC={setup.TargetPrice:F2}, TargetVsProfileVAL={targetVsVal:F2}, TargetVsProfilePOC={targetVsPoc:F2}, TargetVsProfileVAH={targetVsVah:F2}, {entryPart}, ProfileUse=DIAGNOSTIC_ONLY", isHistorical);
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
            if (_activePositions.Any(position => !position.Closed))
                return;

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
                OriginalStopPrice = setup.StopPrice,
                TargetPrice = setup.TargetPrice,
                InitialRisk = risk,
                MaxFavorablePrice = trade.Lastprice,
                MaxAdversePrice = trade.Lastprice,
                LastObservedPrice = trade.Lastprice,
                LastObservedTimeUtc = trade.Time,
                SessionCloseTimeUtc = GetNewYorkSessionEndUtc(trade.Time)
            };

            _activePositions.Add(position);
            DrawTradeOpened(position);

            _log($"[MR_ENTRY] ExecutionMode={GetExecutionMode(isHistorical)}, SetupId={setup.SetupId}, EntryModel={mode}, Source={setup.Source}, Pattern={setup.Pattern}, ReferenceLabel={setup.ReferenceLabel}, Direction={setup.Direction}, Bar={position.EntryBar}, {FormatTime(trade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={trade.Volume:F0}, TradeDirection={trade.Direction}, Stop={position.StopPrice:F2}, TargetPOC={position.TargetPrice:F2}, Risk={risk:F2}, RewardToPOC={reward:F2}, RewardRiskToPOC={rr:F2}, BreakEvenTrigger={BreakEvenTriggerR:F2}R, MaxHoldUntil={FormatTime(position.SessionCloseTimeUtc)}, BigTradeVolume={LondonBigTradeVolume:F0}, SecondsAfterRejection={(trade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}", isHistorical);
            if (LogProfileDiagnosticsForEntries)
                LogProfileDiagnosticsContext(setup, "ENTRY", position.EntryBar, trade.Time, isHistorical, position.EntryPrice);
        }

        private void UpdateOpenPositionsFromBar(int bar, IndicatorCandle candle)
        {
            if (_processingHistoricalReplay)
                return;

            var eventTime = GetCandleEventTime(candle);
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                if (eventTime >= position.SessionCloseTimeUtc)
                {
                    UpdatePositionLastObserved(position, eventTime, candle.Close);
                    ClosePosition(position, bar, position.SessionCloseTimeUtc, "NEW_YORK_CLOSE", candle.Close, IsHistoricalContext(bar));
                    continue;
                }

                UpdatePositionLastObserved(position, eventTime, candle.Close);
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
                if (trade.Time >= position.SessionCloseTimeUtc)
                    continue;

                UpdatePositionLastObserved(position, trade.Time, trade.Lastprice);
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

            DrawTradeClosed(position, pnl);

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

        private void ClosePositionsPastNewYorkClose(DateTime eventTimeUtc, bool isHistorical)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed && eventTimeUtc >= p.SessionCloseTimeUtc).ToList())
            {
                var exitPrice = position.LastObservedTimeUtc >= position.EntryTimeUtc
                    ? position.LastObservedPrice
                    : position.EntryPrice;
                ClosePosition(position, FindBarByTime(position.SessionCloseTimeUtc, position.EntryBar), position.SessionCloseTimeUtc, "NEW_YORK_CLOSE", exitPrice, isHistorical);
            }
        }

        private void CloseOpenReplayPositionsWithoutPnl(DateTime replayEndUtc)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                position.Closed = true;
                position.ExitTimeUtc = replayEndUtc;
                position.ExitBar = FindBarByTime(replayEndUtc, position.EntryBar);
                position.ExitReason = "REPLAY_END_OPEN";
                position.ExitPrice = position.LastObservedTimeUtc >= position.EntryTimeUtc ? position.LastObservedPrice : position.EntryPrice;
                _log($"[MR_REPLAY_OPEN] SetupId={position.SetupId}, Direction={position.Direction}, Bar={position.ExitBar}, ReplayEnd={FormatTime(replayEndUtc)}, Entry={position.EntryPrice:F2}, LastObserved={position.ExitPrice:F2}, LastObservedTime={(position.LastObservedTimeUtc == default ? "NA" : FormatTime(position.LastObservedTimeUtc))}, TargetPOC={position.TargetPrice:F2}, Stop={position.StopPrice:F2}, MaxHoldUntil={FormatTime(position.SessionCloseTimeUtc)}, PnLNotCounted=True", true);
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
            var compression = setup.HasActiveCompressionProfileContext
                ? $"CompressionProfileSource={ActiveCompressionProfileSource}, CompressionProfileLabel={setup.ActiveCompressionProfileLabel}, CompressionProfilePOC={setup.ActiveCompressionPOC:F2}, CompressionProfileVAH={setup.ActiveCompressionVAH:F2}, CompressionProfileVAL={setup.ActiveCompressionVAL:F2}, CompressionProfileValueWidth={setup.ActiveCompressionVAH - setup.ActiveCompressionVAL:F2}, TargetVsCompressionVAL={setup.TargetPrice - setup.ActiveCompressionVAL:F2}, TargetVsCompressionPOC={setup.TargetPrice - setup.ActiveCompressionPOC:F2}, TargetVsCompressionVAH={setup.TargetPrice - setup.ActiveCompressionVAH:F2}, CompressionProfileBars={setup.ActiveCompressionProfileBars}"
                : "CompressionProfileSource=none";
            return $"{same}, {opposite}, {poc}, {compression}";
        }

        private static string FormatCounts(Dictionary<string, int> counts)
        {
            if (counts.Count == 0)
                return "none";

            return string.Join("|", counts.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).Select(kv => $"{kv.Key}:{kv.Value}"));
        }

        private static void UpdatePositionLastObserved(ActivePosition position, DateTime eventTimeUtc, decimal price)
        {
            position.LastObservedTimeUtc = eventTimeUtc;
            position.LastObservedPrice = price;
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

        private DateTime GetNewYorkSessionEndUtc(DateTime eventUtc)
        {
            var newYork = MarketTimeZones.ToNewYork(eventUtc);
            var newYorkEnd = new DateTime(newYork.Year, newYork.Month, newYork.Day, NewYorkSessionEndHour, 0, 0, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(newYorkEnd, MarketTimeZones.NewYork);
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
            public bool HasActiveCompressionProfileContext { get; set; }
            public string ActiveCompressionProfileLabel { get; set; } = string.Empty;
            public int ActiveCompressionProfileStartBar { get; set; }
            public int ActiveCompressionProfileEndBar { get; set; }
            public DateTime ActiveCompressionProfileBeginTimeUtc { get; set; }
            public DateTime ActiveCompressionProfileEndTimeUtc { get; set; }
            public int ActiveCompressionProfileBars { get; set; }
            public decimal ActiveCompressionPOC { get; set; }
            public decimal ActiveCompressionVAH { get; set; }
            public decimal ActiveCompressionVAL { get; set; }
            public decimal ActiveCompressionHigh { get; set; }
            public decimal ActiveCompressionLow { get; set; }
            public decimal ActiveCompressionTotalVolume { get; set; }
            public decimal ActiveCompressionValueAreaVolume { get; set; }
            public decimal ActiveCompressionMaxLevelVolume { get; set; }
            public int ActiveCompressionReadyBar { get; set; }
            public DateTime ActiveCompressionReadyTimeUtc { get; set; }
            public decimal ActiveCompressionOverlapRate { get; set; }
            public decimal ActiveCompressionRangeToAverageBarRange { get; set; }
            public decimal ActiveCompressionDirectionalEfficiency { get; set; }
            public decimal ActiveCompressionCloseSpanRatio { get; set; }
            public decimal ActiveCompressionBaselineMedianBarRange { get; set; }
            public decimal ActiveCompressionRangeToBaselineMedian { get; set; }
            public decimal ActiveCompressionAverageBarRangeToBaselineMedian { get; set; }
            public decimal ActiveCompressionScore { get; set; }
            public decimal ActiveCompressionContractionScore { get; set; }
            public decimal ActiveCompressionOverlapScore { get; set; }
            public decimal ActiveCompressionDirectionalScore { get; set; }
            public decimal ActiveCompressionRotationScore { get; set; }
            public decimal ActiveCompressionContainmentScore { get; set; }
            public decimal ActiveCompressionBoundaryStabilityScore { get; set; }
            public decimal ActiveCompressionPocStabilityScore { get; set; }
            public decimal ActiveCompressionValueConcentrationScore { get; set; }
            public int ActiveCompressionDirectionChanges { get; set; }
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
            public decimal LastObservedPrice { get; set; }
            public DateTime LastObservedTimeUtc { get; set; }
            public DateTime SessionCloseTimeUtc { get; set; }
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

        private sealed class HistoricalLedgerTradeWindow
        {
            public DateTime BeginUtc { get; set; }
            public DateTime EndUtc { get; set; }
        }

        private sealed class CompressionLedgerRunResult
        {
            public int Profiles { get; set; }
            public int Events { get; set; }
            public int Outcomes { get; set; }
            public int ShadowAcceptanceEntries { get; set; }
            public int ShadowAcceptanceOutcomes { get; set; }
            public int ShadowAcceptancePathBars { get; set; }
        }

        private sealed class CompressionLedgerBarStats
        {
            public int TradeCount { get; set; }
            public decimal TotalVolume { get; set; }
            public decimal BuyVolume { get; set; }
            public decimal SellVolume { get; set; }
            public decimal Delta { get; set; }
            public decimal MaxBuyVolume { get; set; }
            public decimal MaxSellVolume { get; set; }
        }

        private sealed class CompressionLedgerEvent
        {
            public CompressionProfileSnapshot Profile { get; set; } = new();
            public int Bar { get; set; }
            public DateTime EventTimeUtc { get; set; }
            public string Boundary { get; set; } = string.Empty;
            public string Interaction { get; set; } = string.Empty;
            public int TestOrdinal { get; set; }
            public int OutsideCloseStreak { get; set; }
            public decimal EventClose { get; set; }
            public decimal BoundaryPrice { get; set; }
            public decimal BreachDistanceRanges { get; set; }
            public decimal CloseDistanceRanges { get; set; }
            public decimal BarRangeToBaselineMedian { get; set; }
            public string CloseState { get; set; } = string.Empty;
            public int TradeCount { get; set; }
            public decimal TotalVolume { get; set; }
            public decimal BuyVolume { get; set; }
            public decimal SellVolume { get; set; }
            public decimal Delta { get; set; }
            public decimal ProfileCvd { get; set; }
            public decimal MaxBuyVolume { get; set; }
            public decimal MaxSellVolume { get; set; }
            public decimal? TotalVolumePercentilePrior { get; set; }
            public decimal? AbsoluteDeltaPercentilePrior { get; set; }
            public decimal? MaxBuyPercentilePrior { get; set; }
            public decimal? MaxSellPercentilePrior { get; set; }
        }

        private sealed class CompressionLedgerOutcome
        {
            public CompressionLedgerEvent Event { get; set; } = new();
            public int Horizon { get; set; }
            public int EndBar { get; set; }
            public DateTime EndTimeUtc { get; set; }
            public decimal EndClose { get; set; }
            public decimal CloseMoveRanges { get; set; }
            public decimal UpMfeRanges { get; set; }
            public decimal DownMaeRanges { get; set; }
            public bool EndInsideRange { get; set; }
            public bool PocTouched { get; set; }
        }

        private sealed class CompressionStudyBoundaryState
        {
            public int Bar { get; set; }
            public DateTime TimeUtc { get; set; }
            public decimal AggressionVolume { get; set; }
        }

        private sealed class CompressionStudyBarStats
        {
            public decimal BuyVolume { get; set; }
            public decimal SellVolume { get; set; }
            public decimal HighBoundaryBuyVolume { get; set; }
            public decimal LowBoundarySellVolume { get; set; }
            public decimal Delta { get; set; }
            public decimal? LastBuyPrice { get; set; }
            public DateTime? LastBuyTimeUtc { get; set; }
            public decimal? LastSellPrice { get; set; }
            public DateTime? LastSellTimeUtc { get; set; }
        }

        private sealed class TradeChartVisual
        {
            public LineTillTouch EntryLine { get; set; } = null!;
            public LineTillTouch StopLine { get; set; } = null!;
            public LineTillTouch TargetLine { get; set; } = null!;
        }

        private sealed class CompressionCandidateState
        {
            public int StartBar { get; set; }
            public int LastBar { get; set; }
            public decimal BaselineMedianBarRange { get; set; }
            public IReadOnlyList<decimal> BaselineRanges { get; set; } = Array.Empty<decimal>();
            public List<decimal> PocHistory { get; } = new();
            public List<decimal> RangeHistory { get; } = new();
            public int ReadyScorePersistence { get; set; }
        }

        private sealed class CompressionProfileSnapshot
        {
            public ReferenceValueArea Reference { get; set; } = new();
            public int ReadyBar { get; set; }
            public DateTime ReadyTimeUtc { get; set; }
            public decimal OverlapRate { get; set; }
            public decimal RangeToAverageBarRange { get; set; }
            public decimal DirectionalEfficiency { get; set; }
            public decimal CloseSpanRatio { get; set; }
            public decimal BaselineMedianBarRange { get; set; }
            public decimal RangeToBaselineMedian { get; set; }
            public decimal AverageBarRangeToBaselineMedian { get; set; }
            public decimal CompressionScore { get; set; }
            public decimal ContractionScore { get; set; }
            public decimal OverlapScore { get; set; }
            public decimal DirectionalScore { get; set; }
            public decimal RotationScore { get; set; }
            public decimal ContainmentScore { get; set; }
            public decimal BoundaryStabilityScore { get; set; }
            public decimal PocStabilityScore { get; set; }
            public decimal ValueConcentrationScore { get; set; }
            public int DirectionChanges { get; set; }
            public int ResolvedBar { get; set; } = -1;
            public DateTime ResolvedTimeUtc { get; set; }
            public string ResolutionReason { get; set; } = string.Empty;
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
            private readonly Dictionary<int, BarContribution> _barContributions = new();

            public Dictionary<decimal, decimal> Profile { get; } = new();
            public decimal TotalVolume { get; private set; }
            public int StartBar { get; private set; } = -1;
            public int EndBar { get; private set; } = -1;
            public DateTime BeginTimeUtc { get; private set; }
            public DateTime EndTimeUtc { get; private set; }
            public decimal High { get; private set; }
            public decimal Low { get; private set; }
            public int BarCount => _barVolumes.Count;

            public List<BarContribution> GetContributionsUpTo(int endBar)
            {
                return _barContributions.Values
                    .Where(contribution => contribution.Bar <= endBar)
                    .OrderBy(contribution => contribution.Bar)
                    .ToList();
            }

            public void Reset()
            {
                _barVolumes.Clear();
                _barContributions.Clear();
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

                var end = GetCandleEventTime(candle);
                _barVolumes[bar] = nextLevels;
                _barContributions[bar] = new BarContribution
                {
                    Bar = bar,
                    BeginTimeUtc = candle.Time,
                    EndTimeUtc = end,
                    Open = candle.Open,
                    High = candle.High,
                    Low = candle.Low,
                    Close = candle.Close,
                    Levels = nextLevels
                };
                TotalVolume = Profile.Values.Sum();
                StartBar = StartBar < 0 ? bar : Math.Min(StartBar, bar);
                EndBar = Math.Max(EndBar, bar);
                BeginTimeUtc = BeginTimeUtc == default || candle.Time < BeginTimeUtc ? candle.Time : BeginTimeUtc;
                EndTimeUtc = EndTimeUtc == default || end > EndTimeUtc ? end : EndTimeUtc;
                High = High == 0 ? candle.High : Math.Max(High, candle.High);
                Low = Low == 0 ? candle.Low : Math.Min(Low, candle.Low);
            }

            public sealed class BarContribution
            {
                public int Bar { get; set; }
                public DateTime BeginTimeUtc { get; set; }
                public DateTime EndTimeUtc { get; set; }
                public decimal Open { get; set; }
                public decimal High { get; set; }
                public decimal Low { get; set; }
                public decimal Close { get; set; }
                public IReadOnlyDictionary<decimal, decimal> Levels { get; set; } = new Dictionary<decimal, decimal>();
            }
        }
    }
}
