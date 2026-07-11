using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    /// <summary>
    /// Causal live/historical compression ledger with non-operational shadow observations.
    /// It contains no order, position, stop, target, execution, or PnL subsystem.
    /// </summary>
    internal sealed class LondonMeanReversionModule
    {
        private readonly Action<string, bool> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly decimal _tickSize;

        private const int LondonSessionStartHour = 8;
        private const int LondonSessionEndHour = 16;
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
        private static readonly int[] CompressionLedgerOutcomeHorizons = { 1, 3, 6, 12 };
        private static readonly int[] ShadowAcceptanceOutcomeHorizons = { 6, 12 };
        private const string ShadowAcceptanceModel = "ACCEPTANCE_CONTINUATION_V1";
        private const int ShadowAcceptancePathMinutes = 60;
        private const int ShadowAcceptancePathCompletionToleranceMinutes = 5;
        private const string ShadowLowFlowConfirmationModel = "LOW_ACCEPTANCE_FLOW_CONFIRMATION_V1";
        private const int ShadowLowFlowConfirmationBars = 3;
        private const int HistoricalShadowRetentionMinutes = 80;
        private const string StudyMode = "COMPRESSION_EVENT_LEDGER_NO_TRADES";
        private const string ActiveCompressionProfileSource = "ActiveCompressionProfile";

        private int _currentBar;
        private readonly List<CumulativeTrade> _historicalTrades = new();
        private readonly HashSet<string> _historicalTradeKeys = new();
        private readonly List<HistoricalLedgerTradeWindow> _historicalLedgerTradeWindows = new();
        private long _historicalTradesReceived;
        private long _historicalTradesOutsideLedgerWindows;
        private long _historicalTradesDuplicate;
        private readonly List<CompressionProfileSnapshot> _compressionStudyProfiles = new();
        private readonly List<CompressionProfileSnapshot> _liveCompressionLedgerProfiles = new();
        private readonly List<CumulativeTrade> _liveStudyTrades = new();
        private readonly HashSet<string> _liveStudyTradeKeys = new();
        private readonly HashSet<string> _loggedCompressionLedgerProfiles = new();
        private readonly HashSet<string> _loggedCompressionLedgerEvents = new();
        private readonly HashSet<string> _loggedCompressionLedgerOutcomes = new();
        private readonly HashSet<string> _loggedShadowAcceptanceEntries = new();
        private readonly HashSet<string> _loggedShadowAcceptanceOutcomes = new();
        private readonly HashSet<string> _loggedShadowAcceptancePathBars = new();
        private readonly HashSet<string> _loggedShadowLowFlowConfirmationEntries = new();
        private readonly HashSet<string> _loggedShadowLowFlowConfirmationOutcomes = new();
        private readonly HashSet<string> _loggedShadowLowFlowConfirmationPathBars = new();
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
        private bool _processingHistoricalReplay;
        private readonly string _chartTimeFrame;

        public LondonMeanReversionModule(
            Action<string, bool> log,
            Func<int, IndicatorCandle> getCandle,
            decimal tickSize,
            string chartTimeFrame)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _getCandle = getCandle ?? throw new ArgumentNullException(nameof(getCandle));
            _tickSize = tickSize > 0 ? tickSize : 1m;
            _chartTimeFrame = string.IsNullOrWhiteSpace(chartTimeFrame) ? "UNKNOWN" : chartTimeFrame;

            _log($"[MR_MODE] Model=FabioCompressionStudy, Modes=LIVE|HISTORICAL, StudyMode={StudyMode}, OperationalEntries=DISABLED, ReferenceProfiles=LOG_ONLY:PreviousDayProfile|PreviousLondonProfile, ChartVisuals=LONDON_CONTEXT_ONLY, Ledger=BOUNDARY_EVENTS_AND_OUTCOMES, LedgerQualification=NONE, LedgerOutcomeHorizons={string.Join("|", CompressionLedgerOutcomeHorizons)}, ShadowModel={ShadowAcceptanceModel}, ShadowTrigger=SECOND_CONSECUTIVE_OUTSIDE_CLOSE, ShadowOutcomeHorizons={string.Join("|", ShadowAcceptanceOutcomeHorizons)}, ShadowPath=EVERY_COMPLETED_CHART_BAR_FOR_{ShadowAcceptancePathMinutes}_MINUTES, ShadowFlowModel={ShadowLowFlowConfirmationModel}, ShadowFlowTrigger=LOW_ACCEPTANCE_PLUS_FIRST_{ShadowLowFlowConfirmationBars}_BARS_DIRECTIONAL_FLOW_POSITIVE, ChartTimeFrame={_chartTimeFrame}, ShadowOrders=DISABLED, CompressionLifecycle=SEARCHING|BUILDING|READY|RESOLVED, CompressionDetection=DYNAMIC_SCORE, CompressionBaseline=PRIOR_{LocalCompressionBaselineLookbackBars}_BARS, CompressionMinBaselineBars={LocalCompressionMinimumBaselineBars}, CompressionMinBuildingBars={LocalCompressionMinimumBuildingBars}, CompressionReadyScore={LocalCompressionMinimumReadyScore:F2}, CompressionReadyPersistence={LocalCompressionReadyPersistenceBars}, CompressionResolutionPersistence={LocalCompressionResolutionPersistenceBars}", false);
        }

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
                    EndUtc = GetCandleEventTime(_getCandle(endBar)).AddMinutes(HistoricalShadowRetentionMinutes)
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
            _liveCompressionLedgerProfiles.Clear();
            _loggedCompressionLedgerProfiles.Clear();
            _loggedCompressionLedgerEvents.Clear();
            _loggedCompressionLedgerOutcomes.Clear();
            _loggedShadowAcceptanceEntries.Clear();
            _loggedShadowAcceptanceOutcomes.Clear();
            _loggedShadowAcceptancePathBars.Clear();
            _loggedShadowLowFlowConfirmationEntries.Clear();
            _loggedShadowLowFlowConfirmationOutcomes.Clear();
            _loggedShadowLowFlowConfirmationPathBars.Clear();

            _log($"[HISTORICAL_FLOW_PROCESS_START] Model=FabioCompressionStudy, StudyMode={StudyMode}, StartBar={startBar}, EndBar={endBar}, StoredTrades={_historicalTrades.Count}, CompressionProfiles={_compressionStudyProfiles.Count}, OperationalEntries=DISABLED", false);

            try
            {
                var ledger = RunCompressionLedger(_compressionStudyProfiles, _historicalTrades, endBar, true);
                var durationMs = (DateTime.UtcNow - startedUtc).TotalMilliseconds;
                _log($"[HISTORICAL_FLOW_FINISH] Model=FabioCompressionStudy, StudyMode={StudyMode}, StartBar={startBar}, EndBar={endBar}, StoredTrades={_historicalTrades.Count}, Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0, LedgerProfiles={ledger.Profiles}, LedgerEvents={ledger.Events}, LedgerOutcomes={ledger.Outcomes}, ShadowAcceptanceEntries={ledger.ShadowAcceptanceEntries}, ShadowAcceptanceOutcomes={ledger.ShadowAcceptanceOutcomes}, ShadowAcceptancePathBars={ledger.ShadowAcceptancePathBars}, ShadowLowFlowConfirmationEntries={ledger.ShadowLowFlowConfirmationEntries}, ShadowLowFlowConfirmationOutcomes={ledger.ShadowLowFlowConfirmationOutcomes}, ShadowLowFlowConfirmationPathBars={ledger.ShadowLowFlowConfirmationPathBars}, ShadowOrders=0, ProcessDurationMs={durationMs:F0}", false);
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

                    if (TryBuildShadowLowFlowConfirmation(
                        shadowEvent,
                        observedEndBar,
                        tradesByBar,
                        tradeCoverage,
                        out var flowConfirmation,
                        out var directionalDelta,
                        out var confirmationVolume,
                        out var directionalFlowImbalance) && flowConfirmation != null)
                    {
                        var flowKey = $"{GetExecutionMode(isHistorical)}:{reference.Label}:{flowConfirmation.Bar}:LOW_FLOW";
                        var flowShadowId = $"{reference.Label}:{flowConfirmation.Bar}:LOW_FLOW";
                        if (_loggedShadowLowFlowConfirmationEntries.Add(flowKey))
                        {
                            LogShadowLowFlowConfirmationEntry(
                                shadowEvent,
                                flowConfirmation,
                                directionalDelta,
                                confirmationVolume,
                                directionalFlowImbalance,
                                tradeCoverage,
                                isHistorical);
                            result.ShadowLowFlowConfirmationEntries++;
                        }

                        foreach (var horizon in ShadowAcceptanceOutcomeHorizons)
                        {
                            if (!TryBuildCompressionLedgerOutcome(flowConfirmation, observedEndBar, horizon, out var outcome) || outcome == null)
                                continue;

                            if (_loggedShadowLowFlowConfirmationOutcomes.Add($"{flowKey}:{horizon}"))
                            {
                                LogShadowLowFlowConfirmationOutcome(outcome, shadowEvent.Bar, tradeCoverage, isHistorical);
                                result.ShadowLowFlowConfirmationOutcomes++;
                            }
                        }

                        result.ShadowLowFlowConfirmationPathBars += LogShadowPath(
                            flowConfirmation,
                            flowKey,
                            observedEndBar,
                            tradesByBar,
                            tradeCoverage,
                            isHistorical,
                            "MR_SHADOW_LOW_FLOW_CONFIRMATION_BAR",
                            ShadowLowFlowConfirmationModel,
                            flowShadowId,
                            _loggedShadowLowFlowConfirmationPathBars);
                    }
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

        private bool TryBuildShadowLowFlowConfirmation(
            CompressionLedgerEvent acceptance,
            int observedEndBar,
            IReadOnlyDictionary<int, List<CumulativeTrade>> tradesByBar,
            string tradeCoverage,
            out CompressionLedgerEvent? confirmation,
            out decimal directionalDelta,
            out decimal totalVolume,
            out decimal directionalFlowImbalance)
        {
            confirmation = null;
            directionalDelta = 0m;
            totalVolume = 0m;
            directionalFlowImbalance = 0m;
            if (acceptance.Boundary != "LOW" || tradeCoverage != "AVAILABLE")
                return false;

            var confirmationBar = acceptance.Bar + ShadowLowFlowConfirmationBars;
            if (confirmationBar > observedEndBar || confirmationBar >= _currentBar)
                return false;

            var cumulativeDelta = 0m;
            for (var bar = acceptance.Bar + 1; bar <= confirmationBar; bar++)
            {
                var stats = BuildCompressionLedgerBarStats(
                    tradesByBar.TryGetValue(bar, out var barTrades) ? barTrades : Array.Empty<CumulativeTrade>());
                cumulativeDelta += stats.Delta;
                totalVolume += stats.TotalVolume;
            }

            if (totalVolume <= 0m)
                return false;
            directionalDelta = -cumulativeDelta;
            directionalFlowImbalance = directionalDelta / totalVolume;
            if (directionalFlowImbalance <= 0m)
                return false;

            var candle = _getCandle(confirmationBar);
            confirmation = new CompressionLedgerEvent
            {
                Profile = acceptance.Profile,
                Bar = confirmationBar,
                EventTimeUtc = GetCandleEventTime(candle),
                Boundary = "LOW",
                Interaction = "FLOW_CONFIRMATION",
                EventClose = candle.Close,
                BoundaryPrice = acceptance.Profile.Reference.Low,
                CloseState = candle.Close < acceptance.Profile.Reference.Low
                    ? "OUTSIDE"
                    : candle.Close <= acceptance.Profile.Reference.High
                        ? "INSIDE"
                        : "OPPOSITE_OUTSIDE"
            };
            return true;
        }

        private void LogShadowLowFlowConfirmationEntry(
            CompressionLedgerEvent acceptance,
            CompressionLedgerEvent confirmation,
            decimal directionalDelta,
            decimal totalVolume,
            decimal directionalFlowImbalance,
            string tradeCoverage,
            bool isHistorical)
        {
            var reference = confirmation.Profile.Reference;
            var shadowId = $"{reference.Label}:{confirmation.Bar}:LOW_FLOW";
            _log($"[MR_SHADOW_LOW_FLOW_CONFIRMATION_ENTRY] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ShadowModel={ShadowLowFlowConfirmationModel}, ShadowId={shadowId}, ProfileLabel={reference.Label}, BaseAcceptanceBar={acceptance.Bar}, EntryBar={confirmation.Bar}, {FormatTime(confirmation.EventTimeUtc)}, Boundary=LOW, Direction=SHORT, ChartTimeFrame={_chartTimeFrame}, EntryPrice={confirmation.EventClose:F2}, Trigger=LOW_ACCEPTANCE_PLUS_FIRST_{ShadowLowFlowConfirmationBars}_BARS_DIRECTIONAL_FLOW_POSITIVE, FlowBars={ShadowLowFlowConfirmationBars}, DirectionalDelta={directionalDelta:F0}, TotalVolume={totalVolume:F0}, DirectionalFlowImbalance={directionalFlowImbalance:F4}, TradeCoverage={tradeCoverage}, OperationalEntry=FALSE, OrderSubmitted=FALSE", isHistorical);
        }

        private void LogShadowLowFlowConfirmationOutcome(
            CompressionLedgerOutcome outcome,
            int baseAcceptanceBar,
            string tradeCoverage,
            bool isHistorical)
        {
            var entry = outcome.Event;
            var reference = entry.Profile.Reference;
            var shadowId = $"{reference.Label}:{entry.Bar}:LOW_FLOW";
            _log($"[MR_SHADOW_LOW_FLOW_CONFIRMATION_OUTCOME] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ShadowModel={ShadowLowFlowConfirmationModel}, ShadowId={shadowId}, ProfileLabel={reference.Label}, BaseAcceptanceBar={baseAcceptanceBar}, EntryBar={entry.Bar}, Boundary=LOW, Direction=SHORT, ChartTimeFrame={_chartTimeFrame}, HorizonBars={outcome.Horizon}, ElapsedMinutes={(outcome.EndTimeUtc - entry.EventTimeUtc).TotalMinutes:F2}, EndBar={outcome.EndBar}, {FormatTime(outcome.EndTimeUtc)}, EntryPrice={entry.EventClose:F2}, EndClose={outcome.EndClose:F2}, DirectionalMoveRanges={-outcome.CloseMoveRanges:F2}, FavorableMfeRanges={outcome.DownMaeRanges:F2}, AdverseMfeRanges={outcome.UpMfeRanges:F2}, EndInsideRange={outcome.EndInsideRange}, PocTouched={outcome.PocTouched}, TradeCoverage={tradeCoverage}, OperationalEntry=FALSE, OrderSubmitted=FALSE", isHistorical);
        }

        private int LogShadowAcceptancePath(
            CompressionLedgerEvent entry,
            string shadowKey,
            int observedEndBar,
            IReadOnlyDictionary<int, List<CumulativeTrade>> tradesByBar,
            string tradeCoverage,
            bool isHistorical)
        {
            var shadowId = $"{entry.Profile.Reference.Label}:{entry.Bar}:{entry.Boundary}";
            return LogShadowPath(
                entry,
                shadowKey,
                observedEndBar,
                tradesByBar,
                tradeCoverage,
                isHistorical,
                "MR_SHADOW_ACCEPTANCE_BAR",
                ShadowAcceptanceModel,
                shadowId,
                _loggedShadowAcceptancePathBars);
        }

        private int LogShadowPath(
            CompressionLedgerEvent entry,
            string shadowKey,
            int observedEndBar,
            IReadOnlyDictionary<int, List<CumulativeTrade>> tradesByBar,
            string tradeCoverage,
            bool isHistorical,
            string marker,
            string shadowModel,
            string shadowId,
            HashSet<string> loggedPathBars)
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
                if (!loggedPathBars.Add($"{shadowKey}:{bar}"))
                    continue;

                var direction = isLong ? "LONG" : "SHORT";
                _log($"[{marker}] ExecutionMode={GetExecutionMode(isHistorical)}, StudyMode={StudyMode}, ShadowModel={shadowModel}, ShadowId={shadowId}, ProfileLabel={reference.Label}, EntryBar={entry.Bar}, Boundary={entry.Boundary}, Direction={direction}, ChartTimeFrame={_chartTimeFrame}, PathBar={bar}, PathBarOrdinal={bar - entry.Bar}, ElapsedMinutes={elapsedMinutes:F2}, {FormatTime(eventTimeUtc)}, Open={candle.Open:F2}, High={candle.High:F2}, Low={candle.Low:F2}, Close={candle.Close:F2}, CandleVolume={candle.Volume:F0}, DirectionalMoveRanges={directionalMove:F2}, FavorableMfeToDateRanges={favorableMfe:F2}, AdverseMfeToDateRanges={adverseMfe:F2}, PriceState={priceState}, PocTouchedThisBar={pocTouchedThisBar}, PocTouchedToDate={pocTouchedToDate}, TradeCount={stats.TradeCount}, TotalVolume={stats.TotalVolume:F0}, BuyVolume={stats.BuyVolume:F0}, SellVolume={stats.SellVolume:F0}, Delta={stats.Delta:F0}, MaxBuyVolume={stats.MaxBuyVolume:F0}, MaxSellVolume={stats.MaxSellVolume:F0}, TradeCoverage={tradeCoverage}, OperationalEntry=FALSE, OrderSubmitted=FALSE", isHistorical);
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

        private static bool IsInLondonSession(DateTime utcTime)
        {
            var london = MarketTimeZones.ToLondon(utcTime);
            return london.Hour >= LondonSessionStartHour && london.Hour < LondonSessionEndHour;
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
            public int ShadowLowFlowConfirmationEntries { get; set; }
            public int ShadowLowFlowConfirmationOutcomes { get; set; }
            public int ShadowLowFlowConfirmationPathBars { get; set; }
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
