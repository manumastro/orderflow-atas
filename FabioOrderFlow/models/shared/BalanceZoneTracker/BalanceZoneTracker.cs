using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;
using ATAS.Indicators.Drawing;

namespace FabioOrderFlow
{
    internal enum MarketState
    {
        NoZone,
        BuildingSessionProfile,
        BalanceReady,
        BreakoutPending,
        OutOfBalance
    }

    internal enum BalanceType
    {
        LondonSession,
        Consolidation
    }

    internal enum BreakoutDirection
    {
        Bullish,
        Bearish
    }

    internal sealed class BalanceZone
    {
        public BalanceType Type { get; set; }
        public int StartBar { get; set; }
        public int EndBar { get; set; }

        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal POC { get; set; }
        public decimal VAH { get; set; }
        public decimal VAL { get; set; }
        public decimal TotalVolume { get; set; }

        public bool IsReady { get; set; }
        public bool IsBroken { get; set; }
        public BreakoutDirection? BreakoutDirection { get; set; }
        public int? BreakoutBar { get; set; }

        public int SessionHighBar { get; set; }
        public int SessionLowBar { get; set; }
        public DateTime SessionHighTimeUtc { get; set; }
        public DateTime SessionLowTimeUtc { get; set; }

        public Dictionary<decimal, decimal> Profile { get; } = new();
    }

    internal sealed class MarketContext
    {
        public MarketState State { get; set; } = MarketState.NoZone;
        public BalanceZone? CurrentZone { get; set; }
        public BreakoutDirection? PendingDirection { get; set; }
        public int PendingBreakoutBar { get; set; } = -1;
        public int ConsecutiveOutsideCloses { get; set; }
    }

    internal sealed class BalanceZoneTracker
    {
        private readonly Indicator _indicator;
        private readonly MarketContext _context = new();
        private readonly Action<string, bool> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;

        private readonly List<DrawingRectangle> _rectangles;
        private readonly List<LineTillTouch> _lines;
        
        // Mean Reversion module (optional)
        private LondonMeanReversionModule? _meanReversionModule;
        
        // Helper for logging events (always live)
        private void LogEvent(string message) => _log(message, false);
        
        private DrawingRectangle? _currentZoneRectangle;
        private LineTillTouch? _currentPocLine;
        private LineTillTouch? _currentVahLine;
        private LineTillTouch? _currentValLine;

        private const int MinSessionBars = 5;
        private const int ExpectedLondonBars = 96; // 8h * 12 bars/h on M5
        private const int MinCompleteSessionBars = 90; // Tolleranza -6 bars
        private const int LondonPreviewStartHour = 8;
        private static readonly bool DetailedDebugLogs = false;
        private static readonly bool DrawBalanceProfileVisuals = false;
        
        // Preview profile values (shared with MR module)
        private decimal _lastPreviewPoc;
        private decimal _lastPreviewVah;
        private decimal _lastPreviewVal;
        
        // Core state variables
        private bool _firstCompleteSessionFound = false;
        private int _lastLoggedPreCloseBar = -1;
        private int _lastPreviewProfileBar = -1;
        private int _currentBar;
        private readonly Dictionary<int, Dictionary<decimal, decimal>> _londonProfileBarVolumes = new();
        private readonly Dictionary<int, decimal> _londonProfileBarTotals = new();
        
        private bool _nySessionActive;
        private int _nySessionStartBar = -1;
        private int _nySessionEndBar = -1;
        private int _nySessionHighBar = -1;
        private int _nySessionLowBar = -1;
        private DateTime _nySessionHighTimeUtc;
        private DateTime _nySessionLowTimeUtc;
        private decimal _nySessionHigh;
        private decimal _nySessionLow;
        private decimal _nySessionTotalVolume;
        private readonly Dictionary<decimal, decimal> _nySessionProfile = new();
        private readonly Dictionary<int, Dictionary<decimal, decimal>> _nyProfileBarVolumes = new();
        private readonly Dictionary<int, decimal> _nyProfileBarTotals = new();
        private int _lastLoggedNyPreCloseBar = -1;
        private int _lastNyPreviewProfileBar = -1;

        public BalanceZoneTracker(
            Indicator indicator, 
            Action<string, bool> log,
            List<DrawingRectangle> rectangles,
            List<LineTillTouch> lines,
            Func<int, IndicatorCandle> getCandle)
        {
            _indicator = indicator;
            _log = log;
            _rectangles = rectangles;
            _lines = lines;
            _getCandle = getCandle;
        }

        // API pubblica per gli altri moduli
        public bool HasBalanceZone => _context.CurrentZone?.IsReady == true;
        public bool IsOutOfBalance => _context.State == MarketState.OutOfBalance;
        public BreakoutDirection? Direction => _context.CurrentZone?.BreakoutDirection;
        public decimal? BalancePocTarget => _context.CurrentZone?.POC;
        public int? BreakoutBar => _context.CurrentZone?.BreakoutBar;
        public decimal? VAH => _context.CurrentZone?.VAH;
        public decimal? VAL => _context.CurrentZone?.VAL;
        public decimal? POC => _context.CurrentZone?.POC;
        
        // For MR module access
        public BalanceZone? CurrentZone => _context.CurrentZone;
        
        // Preview values (exposed for MR module)
        public decimal LastPreviewPoc => _lastPreviewPoc;
        public decimal LastPreviewVah => _lastPreviewVah;
        public decimal LastPreviewVal => _lastPreviewVal;
        
        // Note: MR data structures removed - use module directly via _meanReversionModule
        // Access via: _meanReversionModule.CompletedTrades, ActivePositions, ActiveSetups
        
        // Set MR module from external (called by FabioOrderFlow)
        public void SetMeanReversionModule(LondonMeanReversionModule module)
        {
            _meanReversionModule = module;
        }

        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            // Delegate to MR module if enabled
            _meanReversionModule?.OnHistoricalCumulativeTrades(cumulativeTrades);
        }

        public void OnLiveCumulativeTrade(CumulativeTrade trade)
        {
            _meanReversionModule?.OnLiveCumulativeTrade(trade);
        }

        // ========================================
        // MEAN REVERSION METHODS
        // Moved to LondonMeanReversionModule
        // ========================================

        public void OnBarUpdate(int bar, IndicatorCandle candle, int currentBar)
        {
            _currentBar = currentBar;
            
            var barTime = candle.Time;

            if (DetailedDebugLogs && bar == 1)
            {
                LogEvent($"[BAR_DETAIL] First bar: Time={barTime:yyyy-MM-dd HH:mm:ss}, O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}, V={candle.Volume}");
            }

            var londonTime = MarketTimeZones.ToLondon(barTime);
            var nyTime = MarketTimeZones.ToNewYork(barTime);

            var isInLondonSession = IsInLondonSession(londonTime);
            var isInNewYorkSession = IsInNewYorkSession(nyTime);
            var isClosedBarForBreakout = IsClosedBarForBreakout(bar);
            
            if (DetailedDebugLogs && bar % 10 == 0 && (isInLondonSession || isInNewYorkSession))
            {
                LogEvent($"[STATE] Bar={bar}, State={_context.State}, LondonSession={isInLondonSession}, NYSession={isInNewYorkSession}");
            }

            // State machine
            switch (_context.State)
            {
                case MarketState.NoZone:
                    if (isInLondonSession)
                    {
                        StartLondonSession(bar, candle);
                    }
                    break;

                case MarketState.BuildingSessionProfile:
                    if (isInLondonSession)
                    {
                        UpdateLondonProfile(bar, candle);
                    }
                    else
                    {
                        FinalizeLondonSession(bar);
                    }
                    break;

                case MarketState.BalanceReady:
                    if (isInNewYorkSession)
                    {
                        LogNewYorkSession(bar, candle, isClosedBarForBreakout);
                    }

                    if (isInNewYorkSession && isClosedBarForBreakout)
                    {
                        CheckForBreakout(bar, candle);
                    }
                    
                    // Reset su nuova London session
                    if (isInLondonSession && !IsBarInCurrentZone(bar))
                    {
                        ResetForNewSession(bar);
                    }
                    break;

                case MarketState.BreakoutPending:
                    if (isInNewYorkSession)
                    {
                        LogNewYorkSession(bar, candle, isClosedBarForBreakout);
                    }

                    if (isInNewYorkSession && isClosedBarForBreakout)
                    {
                        ConfirmBreakout(bar, candle);
                    }
                    
                    // Reset su nuova London session
                    if (isInLondonSession && !IsBarInCurrentZone(bar))
                    {
                        ResetForNewSession(bar);
                    }
                    break;

                case MarketState.OutOfBalance:
                    // Out-of-balance confermato, gli altri moduli possono operare
                    // NON estendere il box della balance zone (rimane fisso sulla sessione London)
                    // Visual separato per la zona out-of-balance
                    if (isInNewYorkSession)
                    {
                        LogNewYorkSession(bar, candle, isClosedBarForBreakout);
                    }
                    
                    // Reset su nuova London session
                    if (isInLondonSession && !IsBarInCurrentZone(bar))
                    {
                        ResetForNewSession(bar);
                    }
                    break;
            }
            
            // Call MR module for evaluation
            _meanReversionModule?.OnBarUpdate(bar, currentBar, candle);
        }

        private void StartLondonSession(int bar, IndicatorCandle candle)
        {
            // Verifica se è l'inizio esatto della sessione London (08:00 GMT)
            var londonTime = MarketTimeZones.ToLondon(candle.Time);
            
            // Salta sessioni parziali fino a trovare la prima completa
            if (!_firstCompleteSessionFound && londonTime.Hour != 8)
            {
                var italyTime = MarketTimeZones.ToItaly(candle.Time);
                LogEvent($"[SKIP] Partial London session detected at London={londonTime:yyyy-MM-dd HH:mm:ss}, Italy={italyTime:yyyy-MM-dd HH:mm:ss} | Bar={bar}");
                return;
            }
            
            _context.State = MarketState.BuildingSessionProfile;
            _context.CurrentZone = new BalanceZone
            {
                Type = BalanceType.LondonSession,
                StartBar = bar,
                IsReady = false,
                High = candle.High,
                Low = candle.Low,
                SessionHighBar = bar,
                SessionLowBar = bar,
                SessionHighTimeUtc = candle.Time,
                SessionLowTimeUtc = candle.Time
            };

            _lastLoggedPreCloseBar = -1;
            _lastPreviewProfileBar = -1;
            _londonProfileBarVolumes.Clear();
            _londonProfileBarTotals.Clear();

            UpdateLondonProfile(bar, candle);
            LogEvent($"[SESSION_START] London session started at bar {bar} ({FormatTimes(candle.Time)})");
            LogEvent($"[SESSION_START] Candle: O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}");
        }

        private void UpdateLondonProfile(int bar, IndicatorCandle candle)
        {
            if (_context.CurrentZone == null) return;

            ReplaceProfileContribution(
                _context.CurrentZone.Profile,
                _londonProfileBarVolumes,
                _londonProfileBarTotals,
                bar,
                candle,
                total => _context.CurrentZone.TotalVolume = total);

            var previousHigh = _context.CurrentZone.High;
            var previousLow = _context.CurrentZone.Low;

            var importantEvent = false;

            if (candle.High > previousHigh)
            {
                _context.CurrentZone.High = candle.High;
                _context.CurrentZone.SessionHighBar = bar;
                _context.CurrentZone.SessionHighTimeUtc = candle.Time;
                LogSessionExtreme("NEW_SESSION_HIGH", bar, candle, previousHigh);
                _meanReversionModule?.OnNewSessionHigh(bar, candle, previousHigh);
                importantEvent = true;
            }

            if (previousLow == 0 || candle.Low < previousLow)
            {
                _context.CurrentZone.Low = candle.Low;
                _context.CurrentZone.SessionLowBar = bar;
                _context.CurrentZone.SessionLowTimeUtc = candle.Time;
                LogSessionExtreme("NEW_SESSION_LOW", bar, candle, previousLow);
                _meanReversionModule?.OnNewSessionLow(bar, candle, previousLow);
                importantEvent = true;
            }

            LogLondonPreCloseCandle(bar, candle);
            LogPreviewProfileIfNeeded(bar, candle, importantEvent);
        }

        private void FinalizeLondonSession(int bar)
        {
            FinalizeSession(bar, "London", MinCompleteSessionBars, MinSessionBars, true);
        }

        private void LogNewYorkSession(int bar, IndicatorCandle candle, bool isClosedBarForBreakout)
        {
            if (!_nySessionActive)
            {
                StartNewYorkSession(bar, candle);
            }
            else
            {
                UpdateNewYorkSession(bar, candle);
            }

            if (_nySessionActive && bar != _lastLoggedNyPreCloseBar && isClosedBarForBreakout)
            {
                _lastLoggedNyPreCloseBar = bar;
                var (bid, ask, delta, topLevels) = GetCandleVolumeDiagnostics(candle);
                // NY_PRE_CLOSE logging disabled to reduce log noise
                // LogEvent($"[NY_PRE_CLOSE] Bar={bar}, {FormatTimes(candle.Time)}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}, TopLevels={topLevels}");
            }

            if (_nySessionActive)
            {
                LogNewYorkPreviewProfileIfNeeded(bar, candle, !isClosedBarForBreakout);
            }
        }

        private void StartNewYorkSession(int bar, IndicatorCandle candle)
        {
            _nySessionActive = true;
            _nySessionStartBar = bar;
            _nySessionEndBar = bar;
            _nySessionHighBar = bar;
            _nySessionLowBar = bar;
            _nySessionHigh = candle.High;
            _nySessionLow = candle.Low;
            _nySessionTotalVolume = 0;
            _nySessionProfile.Clear();
            _nyProfileBarVolumes.Clear();
            _nyProfileBarTotals.Clear();
            _lastLoggedNyPreCloseBar = -1;
            _lastNyPreviewProfileBar = -1;
            _nySessionHighTimeUtc = candle.Time;
            _nySessionLowTimeUtc = candle.Time;

            UpdateNewYorkSession(bar, candle);
            LogEvent($"[NY_SESSION_START] New York session started at bar {bar} ({FormatTimes(candle.Time)})");
            LogEvent($"[NY_SESSION_START] Candle: O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}");
        }

        private void UpdateNewYorkSession(int bar, IndicatorCandle candle)
        {
            if (!_nySessionActive)
                return;

            _nySessionEndBar = bar;

            ReplaceProfileContribution(
                _nySessionProfile,
                _nyProfileBarVolumes,
                _nyProfileBarTotals,
                bar,
                candle,
                total => _nySessionTotalVolume = total);

            if (candle.High > _nySessionHigh)
            {
                _nySessionHigh = candle.High;
                _nySessionHighBar = bar;
                _nySessionHighTimeUtc = candle.Time;
                LogSessionExtreme("NEW_NY_SESSION_HIGH", bar, candle, _nySessionHigh);
            }

            if (_nySessionLow == 0 || candle.Low < _nySessionLow)
            {
                _nySessionLow = candle.Low;
                _nySessionLowBar = bar;
                _nySessionLowTimeUtc = candle.Time;
                LogSessionExtreme("NEW_NY_SESSION_LOW", bar, candle, _nySessionLow);
            }
        }

        private void LogNewYorkPreviewProfileIfNeeded(int bar, IndicatorCandle candle, bool force)
        {
            if (_nySessionProfile.Count == 0 || _nySessionTotalVolume <= 0)
                return;

            if (!TryCalculateProfilePreview(_nySessionProfile, _nySessionTotalVolume, out var poc, out var vah, out var val, out var valueAreaVolume, out var maxVolume))
                return;

            _lastNyPreviewProfileBar = bar;
            var (bid, ask, delta, _) = GetCandleVolumeDiagnostics(candle);
            var relation = candle.Close > vah ? "ABOVE_PREVIEW_VAH" : candle.Close < val ? "BELOW_PREVIEW_VAL" : "INSIDE_PREVIEW_VA";
            var nyBars = _nySessionEndBar - _nySessionStartBar + 1;

            // NY_PROFILE_PREVIEW logging disabled to reduce log noise
            // if (force || (hasActiveTrades && significantChange))
            // {
            //     var isHistorical = bar < _indicator.CurrentBar - 1;
            //     _log($"[NY_PROFILE_PREVIEW] Bar={bar}, {FormatTimes(candle.Time)}, Reason={(force ? "event" : "live")}, Bars={nyBars}, High={_nySessionHigh:F2}, Low={_nySessionLow:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, VA_Volume={valueAreaVolume:F0}, TotalVolume={_nySessionTotalVolume:F0}, MaxLevelVolume={maxVolume:F0}, Close={candle.Close:F2}, Relation={relation}, DistToPOC={candle.Close - poc:F2}, DistToVAH={candle.Close - vah:F2}, DistToVAL={candle.Close - val:F2}, CandleBid={bid:F0}, CandleAsk={ask:F0}, CandleDelta={delta:F0}", isHistorical);
            //     
            //     // Update last logged state
            //     _lastNyLoggedPreviewBar = bar;
            //     _lastNyLoggedPoc = poc;
            //     _lastNyLoggedVah = vah;
            //     _lastNyLoggedVal = val;
            //     _lastNyLoggedRelation = relation;
            // }
        }

        private void FinalizeSession(int bar, string sessionName, int minCompleteBars, int minSessionBars, bool isLondon)
        {
            if (_context.CurrentZone == null) return;

            _context.CurrentZone.EndBar = bar - 1;

            var barCount = _context.CurrentZone.EndBar - _context.CurrentZone.StartBar + 1;
            var endCandle = _getCandle(bar - 1);

            LogEvent($"[SESSION_END] {sessionName} session ended at bar {bar - 1} ({FormatTimes(endCandle.Time)}). Bars in session: {barCount}");
            LogEvent($"[SESSION_END] Candle: O={endCandle.Open:F2}, H={endCandle.High:F2}, L={endCandle.Low:F2}, C={endCandle.Close:F2}");

            if (isLondon)
            {
                LogEvent($"[SESSION_END] Session range so far: High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, TotalVolume={_context.CurrentZone.TotalVolume:F0}");
                LogEvent($"[SESSION_EXTREMES] High={_context.CurrentZone.High:F2} at Bar={_context.CurrentZone.SessionHighBar}, {FormatTimes(_context.CurrentZone.SessionHighTimeUtc)}");
                LogEvent($"[SESSION_EXTREMES] Low={_context.CurrentZone.Low:F2} at Bar={_context.CurrentZone.SessionLowBar}, {FormatTimes(_context.CurrentZone.SessionLowTimeUtc)}");
            }

            if (barCount < minCompleteBars)
            {
                LogEvent($"[SESSION_SKIP] {sessionName} session incomplete (only {barCount} bars), skipping...");
                _context.State = MarketState.NoZone;
                _context.CurrentZone = null;
                return;
            }

            if (barCount < minSessionBars || (isLondon && _context.CurrentZone.Profile.Count == 0))
            {
                LogEvent($"[BALANCE_INVALID] {sessionName} session too short or empty | Bars={barCount}");
                _context.State = MarketState.NoZone;
                _context.CurrentZone = null;
                return;
            }

            if (isLondon)
            {
                if (DetailedDebugLogs)
                {
                    LogEvent($"[PROFILE_RANGE] High={_context.CurrentZone.High:F2} | Low={_context.CurrentZone.Low:F2} | ProfileLevels={_context.CurrentZone.Profile.Count}");
                    LogEvent($"[PROFILE_DETAIL] First 10 levels: {string.Join(", ", _context.CurrentZone.Profile.OrderBy(kv => kv.Key).Take(10).Select(kv => $"{kv.Key:F2}={kv.Value:F0}"))}");
                    LogEvent($"[PROFILE_DETAIL] Last 10 levels: {string.Join(", ", _context.CurrentZone.Profile.OrderByDescending(kv => kv.Key).Take(10).Select(kv => $"{kv.Key:F2}={kv.Value:F0}"))}");
                }

                CalculatePOC();
                CalculateValueArea();

                _context.CurrentZone.IsReady = true;
                _context.State = MarketState.BalanceReady;

                if (DrawBalanceProfileVisuals)
                    DrawBalanceZone();

                LogEvent($"[ZONE_READY] Balance zone ready: High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, POC={_context.CurrentZone.POC:F2}, VAH={_context.CurrentZone.VAH:F2}, VAL={_context.CurrentZone.VAL:F2}, TotalVolume={_context.CurrentZone.TotalVolume:F0}");
                LogEvent($"[ZONE_READY] StartBar={_context.CurrentZone.StartBar}, EndBar={_context.CurrentZone.EndBar}, Bars={barCount}");

                if (DetailedDebugLogs)
                {
                    VerifyZoneCoverageComplete(_context.CurrentZone);
                }
            }
        }

        private void CalculatePOC()
        {
            if (_context.CurrentZone == null || _context.CurrentZone.Profile.Count == 0) return;

            var maxVolume = _context.CurrentZone.Profile.Values.Max();
            var pocCandidates = _context.CurrentZone.Profile.Where(kv => kv.Value == maxVolume).ToList();

            // Tie-break: prezzo più basso
            _context.CurrentZone.POC = pocCandidates.Min(kv => kv.Key);
            
            if (DetailedDebugLogs)
            {
                LogEvent($"[POC_CALC] MaxVolume={maxVolume:F0}, POC={_context.CurrentZone.POC:F2}, Candidates={pocCandidates.Count}");
                if (pocCandidates.Count > 1)
                {
                    LogEvent($"[POC_CALC] Multiple POC candidates (tie-break to lowest): {string.Join(", ", pocCandidates.Select(kv => $"{kv.Key:F2}"))}");
                }
            }
        }

        private void CalculateValueArea()
        {
            if (_context.CurrentZone == null || _context.CurrentZone.Profile.Count == 0) return;

            var targetVolume = _context.CurrentZone.TotalVolume * 0.70m;
            var sortedLevels = _context.CurrentZone.Profile.OrderBy(kv => kv.Key).ToList();

            var pocIndex = sortedLevels.FindIndex(kv => kv.Key == _context.CurrentZone.POC);
            if (pocIndex == -1) return;

            if (DetailedDebugLogs)
            {
                LogEvent($"[VALUE_AREA_CALC] TotalVolume={_context.CurrentZone.TotalVolume:F0}, Target70%={targetVolume:F0}");
                LogEvent($"[VALUE_AREA_CALC] POC at index {pocIndex} of {sortedLevels.Count} levels, Price={_context.CurrentZone.POC:F2}");
            }

            var accumulatedVolume = sortedLevels[pocIndex].Value;
            var lowerIndex = pocIndex;
            var upperIndex = pocIndex;

            if (DetailedDebugLogs)
            {
                LogEvent($"[VALUE_AREA_CALC] Starting expansion from POC, InitialVolume={accumulatedVolume:F0}");
            }

            while (accumulatedVolume < targetVolume && (lowerIndex > 0 || upperIndex < sortedLevels.Count - 1))
            {
                var lowerVolume = lowerIndex > 0 ? sortedLevels[lowerIndex - 1].Value : 0;
                var upperVolume = upperIndex < sortedLevels.Count - 1 ? sortedLevels[upperIndex + 1].Value : 0;

                if (lowerVolume >= upperVolume && lowerIndex > 0)
                {
                    lowerIndex--;
                    accumulatedVolume += sortedLevels[lowerIndex].Value;
                    if (DetailedDebugLogs)
                    {
                        LogEvent($"[VALUE_AREA_CALC] Expanded down: Price={sortedLevels[lowerIndex].Key:F2}, Volume={sortedLevels[lowerIndex].Value:F0}, Accumulated={accumulatedVolume:F0}");
                    }
                }
                else if (upperIndex < sortedLevels.Count - 1)
                {
                    upperIndex++;
                    accumulatedVolume += sortedLevels[upperIndex].Value;
                    if (DetailedDebugLogs)
                    {
                        LogEvent($"[VALUE_AREA_CALC] Expanded up: Price={sortedLevels[upperIndex].Key:F2}, Volume={sortedLevels[upperIndex].Value:F0}, Accumulated={accumulatedVolume:F0}");
                    }
                }
                else
                {
                    break;
                }
            }

            _context.CurrentZone.VAL = sortedLevels[lowerIndex].Key;
            _context.CurrentZone.VAH = sortedLevels[upperIndex].Key;
            
            if (DetailedDebugLogs)
            {
                LogEvent($"[VALUE_AREA_CALC] Final: VAL={_context.CurrentZone.VAL:F2} (index {lowerIndex}), VAH={_context.CurrentZone.VAH:F2} (index {upperIndex})");
                LogEvent($"[VALUE_AREA_CALC] Final accumulated volume: {accumulatedVolume:F0} ({100.0m * accumulatedVolume / _context.CurrentZone.TotalVolume:F1}%)");
            }
        }

        private void CheckForBreakout(int bar, IndicatorCandle candle)
        {
            if (_context.CurrentZone == null) return;

            var close = candle.Close;

            var isBullishBreak = close > _context.CurrentZone.VAH;
            var isBearishBreak = close < _context.CurrentZone.VAL;

            if (isBullishBreak)
            {
                _context.State = MarketState.BreakoutPending;
                _context.PendingDirection = BreakoutDirection.Bullish;
                _context.PendingBreakoutBar = bar;
                _context.ConsecutiveOutsideCloses = 1;
                LogEvent($"[BREAKOUT_PENDING] Bullish | Bar={bar}, {FormatTimes(candle.Time)}, Close={close:F2} > VAH={_context.CurrentZone.VAH:F2}");
            }
            else if (isBearishBreak)
            {
                _context.State = MarketState.BreakoutPending;
                _context.PendingDirection = BreakoutDirection.Bearish;
                _context.PendingBreakoutBar = bar;
                _context.ConsecutiveOutsideCloses = 1;
                LogEvent($"[BREAKOUT_PENDING] Bearish | Bar={bar}, {FormatTimes(candle.Time)}, Close={close:F2} < VAL={_context.CurrentZone.VAL:F2}");
            }
        }

        private void ConfirmBreakout(int bar, IndicatorCandle candle)
        {
            if (_context.CurrentZone == null) return;
            if (bar <= _context.PendingBreakoutBar) return;

            var close = candle.Close;

            var isStillOutside = _context.PendingDirection == BreakoutDirection.Bullish
                ? close > _context.CurrentZone.VAH
                : close < _context.CurrentZone.VAL;

            if (isStillOutside)
            {
                _context.ConsecutiveOutsideCloses++;

                if (_context.ConsecutiveOutsideCloses >= 2)
                {
                    _context.State = MarketState.OutOfBalance;
                    _context.CurrentZone.IsBroken = true;
                    _context.CurrentZone.BreakoutDirection = _context.PendingDirection;
                    _context.CurrentZone.BreakoutBar = _context.PendingBreakoutBar;

                    UpdateBalanceZoneColors();

                    LogEvent($"[BREAKOUT_CONFIRMED] Direction: {_context.PendingDirection}, Bar: {bar}, {FormatTimes(candle.Time)}, Close: {close:F2}, VAH: {_context.CurrentZone.VAH:F2}, VAL: {_context.CurrentZone.VAL:F2}");
                    LogEvent($"[BREAKOUT_CONFIRMED] Candle: O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}");
                    LogEvent($"[OUT_OF_BALANCE] {_context.PendingDirection} | BreakoutBar={_context.PendingBreakoutBar} | TargetPOC={_context.CurrentZone.POC}");
                }
            }
            else
            {
                // False breakout, torna dentro value area
                _context.State = MarketState.BalanceReady;
                _context.PendingDirection = null;
                _context.PendingBreakoutBar = -1;
                _context.ConsecutiveOutsideCloses = 0;
                LogEvent("[FALSE_BREAKOUT] Returned inside value area");
            }
        }

        private void ResetForNewSession(int bar)
        {
            // Reset referenze alle zone vecchie
            _currentZoneRectangle = null;
            _currentPocLine = null;
            _currentVahLine = null;
            _currentValLine = null;
            
            _context.State = MarketState.NoZone;
            _context.CurrentZone = null;
            _londonProfileBarVolumes.Clear();
            _londonProfileBarTotals.Clear();
            _context.PendingDirection = null;
            _context.PendingBreakoutBar = -1;
            _context.ConsecutiveOutsideCloses = 0;
        }

        private bool IsBarInCurrentZone(int bar)
        {
            if (_context.CurrentZone == null) return false;
            return bar >= _context.CurrentZone.StartBar && bar <= _context.CurrentZone.EndBar;
        }

        private bool IsInLondonSession(DateTime londonTime)
        {
            var hour = londonTime.Hour;
            return hour >= 8 && hour < 16;
        }

        private bool IsClosedBarForBreakout(int bar)
        {
            return bar < _currentBar - 1;
        }

        private bool IsInNewYorkSession(DateTime nyTime)
        {
            var hour = nyTime.Hour;
            var minute = nyTime.Minute;
            var totalMinutes = hour * 60 + minute;

            var startMinutes = 9 * 60 + 30; // 09:30
            var endMinutes = 16 * 60;        // 16:00

            return totalMinutes >= startMinutes && totalMinutes < endMinutes;
        }

        private string FormatTimes(DateTime utcTime)
        {
            return MarketTimeZones.FormatUtcLondonItaly(utcTime);
        }

        private string GetBarMode(int bar)
        {
            return bar >= _currentBar - 1 ? "LIVE_OR_LAST_BAR" : "HISTORICAL_CLOSED";
        }

        private void LogSessionExtreme(string tag, int bar, IndicatorCandle candle, decimal previousExtreme)
        {
            // Stub - session extreme logging can be added if needed
        }

        private void LogLondonPreCloseCandle(int bar, IndicatorCandle candle)
        {
            if (bar == _lastLoggedPreCloseBar)
                return;

            var londonTime = MarketTimeZones.ToLondon(candle.Time);
            if (londonTime.Hour != 15)
                return;

            _lastLoggedPreCloseBar = bar;
            var (bid, ask, delta, topLevels) = GetCandleVolumeDiagnostics(candle);
            // LONDON_PRE_CLOSE logging disabled to reduce log noise
            // LogEvent($"[LONDON_PRE_CLOSE] Bar={bar}, {FormatTimes(candle.Time)}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}, TopLevels={topLevels}");
        }

        private void LogPreviewProfileIfNeeded(int bar, IndicatorCandle candle, bool force)
        {
            if (_context.CurrentZone == null || _context.CurrentZone.Profile.Count == 0)
                return;

            var londonTime = MarketTimeZones.ToLondon(candle.Time);
            var isLondonPreviewWindow = londonTime.Hour >= LondonPreviewStartHour;

            if (!isLondonPreviewWindow)
                return;

            if (!TryCalculateProfilePreview(_context.CurrentZone.Profile, _context.CurrentZone.TotalVolume, out var poc, out var vah, out var val, out var valueAreaVolume, out var maxVolume))
                return;

            _lastPreviewProfileBar = bar;
            _lastPreviewPoc = poc;
            _lastPreviewVah = vah;
            _lastPreviewVal = val;
            
            var relation = candle.Close > vah ? "ABOVE_PREVIEW_VAH" : candle.Close < val ? "BELOW_PREVIEW_VAL" : "INSIDE_PREVIEW_VA";
            var sessionBars = bar - _context.CurrentZone.StartBar + 1;

            // PROFILE_PREVIEW logging disabled to reduce log noise
            // Log PROFILE_PREVIEW only if:
            // 1. force=true (event: high/low/new bar)
            // 2. There's an active MR trade AND significant change occurred
            // var hasActiveTrades = _meanReversionModule?.MeanReversionOutcomes.Any(o => !o.PositionClosed) ?? false;
            
            // Check if significant change occurred
            // var significantChange = _lastLoggedPreviewBar != bar ||
            //                        _lastLoggedPoc != poc ||
            //                        _lastLoggedVah != vah ||
            //                        _lastLoggedVal != val ||
            //                        _lastLoggedRelation != relation;
            
            // if (force || (hasActiveTrades && significantChange))
            // {
            //     var isHistorical = bar < _indicator.CurrentBar - 1;
            //     _log($"[PROFILE_PREVIEW] Bar={bar}, {FormatTimes(candle.Time)}, Reason={(force ? "event" : "live")}, Bars={sessionBars}, High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, VA_Volume={valueAreaVolume:F0}, TotalVolume={_context.CurrentZone.TotalVolume:F0}, MaxLevelVolume={maxVolume:F0}, Close={candle.Close:F2}, Relation={relation}, DistToPOC={candle.Close - poc:F2}, DistToVAH={candle.Close - vah:F2}, DistToVAL={candle.Close - val:F2}", isHistorical);
                
            //     // Update last logged state
            //     _lastLoggedPreviewBar = bar;
            //     _lastLoggedPoc = poc;
            //     _lastLoggedVah = vah;
            //     _lastLoggedVal = val;
            //     _lastLoggedRelation = relation;
            // }
            
            // Note: v3 module handles trigger detection internally via OnNewSessionHigh/Low
            // No need to call explicit trigger methods here
        }

        private void DrawBalanceZone()
        {
            if (_context.CurrentZone == null) return;

            var zone = _context.CurrentZone;
            
            // Colore grigio neutro per balance ready
            var fillBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(30, 128, 128, 128));
            var outlinePen = new System.Drawing.Pen(System.Drawing.Color.Gray, 1);
            var pocPen = new System.Drawing.Pen(System.Drawing.Color.Orange, 2);

            // HYBRID APPROACH: Box visivo usa High/Low (range completo sessione)
            // ma VAH/VAL vengono usati solo per breakout detection
            _currentZoneRectangle = new DrawingRectangle(
                zone.StartBar,
                zone.High,      // ← High della sessione (non VAH)
                zone.EndBar,
                zone.Low,       // ← Low della sessione (non VAL)
                outlinePen,
                fillBrush
            );
            _rectangles.Add(_currentZoneRectangle);

            // Linea POC (NON Ray, termina a EndBar)
            _currentPocLine = new LineTillTouch(
                zone.StartBar,
                zone.POC,
                pocPen
            )
            {
                IsRay = false,
                SecondBar = zone.EndBar
            };
            _lines.Add(_currentPocLine);
            
            // Linea VAH (Value Area High) - linea tratteggiata blu chiaro
            var vahPen = new System.Drawing.Pen(System.Drawing.Color.LightBlue, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            _currentVahLine = new LineTillTouch(
                zone.StartBar,
                zone.VAH,
                vahPen
            )
            {
                IsRay = false,
                SecondBar = zone.EndBar
            };
            _lines.Add(_currentVahLine);
            
            // Linea VAL (Value Area Low) - linea tratteggiata blu chiaro
            var valPen = new System.Drawing.Pen(System.Drawing.Color.LightBlue, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            _currentValLine = new LineTillTouch(
                zone.StartBar,
                zone.VAL,
                valPen
            )
            {
                IsRay = false,
                SecondBar = zone.EndBar
            };
            _lines.Add(_currentValLine);

            if (DetailedDebugLogs)
            {
                LogEvent($"[DRAW_ZONE] Rectangle=(Bar:{zone.StartBar}, High:{zone.High:F2})-(Bar:{zone.EndBar}, Low:{zone.Low:F2})");
                LogEvent($"[DRAW_ZONE] POC_Line=(Bar:{zone.StartBar}, Price:{zone.POC:F2})-(Bar:{zone.EndBar}, Price:{zone.POC:F2})");
                LogEvent($"[DRAW_ZONE] VAH_Line=(Bar:{zone.StartBar}, Price:{zone.VAH:F2})-(Bar:{zone.EndBar}, Price:{zone.VAH:F2})");
                LogEvent($"[DRAW_ZONE] VAL_Line=(Bar:{zone.StartBar}, Price:{zone.VAL:F2})-(Bar:{zone.EndBar}, Price:{zone.VAL:F2})");
                LogEvent($"[DRAW_ZONE] Zone visual box: High={zone.High:F2}, Low={zone.Low:F2}");
                LogEvent($"[DRAW_ZONE] === First 5 candles in zone ===");
                for (int i = zone.StartBar; i < Math.Min(zone.StartBar + 5, zone.EndBar); i++)
                {
                    var c = _getCandle(i);
                    LogEvent($"[DRAW_ZONE] Bar {i}: Time={FormatTimes(c.Time)}, O={c.Open:F2}, H={c.High:F2}, L={c.Low:F2}, C={c.Close:F2}");
                }
            }
        }

        private void VerifyZoneCoverageComplete(BalanceZone zone)
        {
            LogEvent($"[VERIFY_COVERAGE] === COMPLETE COVERAGE CHECK ===");
            LogEvent($"[VERIFY_COVERAGE] Zone visual box: High={zone.High:F2}, Low={zone.Low:F2}");
            LogEvent($"[VERIFY_COVERAGE] Zone VAH={zone.VAH:F2}, VAL={zone.VAL:F2} (breakout detection)");
            LogEvent($"[VERIFY_COVERAGE] StartBar={zone.StartBar}, EndBar={zone.EndBar}, Total={zone.EndBar - zone.StartBar + 1} bars");
            
            int totalCandles = 0;
            int fullyCoveredByBox = 0;
            int partiallyCoveredByBox = 0;
            int notCoveredByBox = 0;
            int fullyCoveredByVA = 0;
            int partiallyCoveredByVA = 0;
            int notCoveredByVA = 0;
            
            // Verifica TUTTE le candele rispetto sia al box (High/Low) che alla Value Area (VAH/VAL)
            for (int i = zone.StartBar; i <= zone.EndBar; i++)
            {
                var candle = _getCandle(i);
                totalCandles++;
                
                // Coverage rispetto al box visivo (High/Low)
                bool candleFullyCoveredByBox = candle.High <= zone.High && candle.Low >= zone.Low;
                bool candlePartiallyCoveredByBox = (candle.High >= zone.Low && candle.Low <= zone.High);
                
                // Coverage rispetto alla Value Area (VAH/VAL)
                bool candleFullyCoveredByVA = candle.High <= zone.VAH && candle.Low >= zone.VAL;
                bool candlePartiallyCoveredByVA = (candle.High >= zone.VAL && candle.Low <= zone.VAH);
                
                string boxStatus;
                if (candleFullyCoveredByBox)
                {
                    fullyCoveredByBox++;
                    boxStatus = "FULLY_IN_BOX";
                }
                else if (candlePartiallyCoveredByBox)
                {
                    partiallyCoveredByBox++;
                    boxStatus = "PARTIALLY_IN_BOX";
                }
                else
                {
                    notCoveredByBox++;
                    boxStatus = "OUT_OF_BOX";
                }
                
                string vaStatus;
                if (candleFullyCoveredByVA)
                {
                    fullyCoveredByVA++;
                    vaStatus = "FULLY_IN_VA";
                }
                else if (candlePartiallyCoveredByVA)
                {
                    partiallyCoveredByVA++;
                    vaStatus = "PARTIALLY_IN_VA";
                }
                else
                {
                    notCoveredByVA++;
                    vaStatus = "OUT_OF_VA";
                }
                
                // Log solo candele problematiche per ridurre spam
                if (!candleFullyCoveredByBox)
                {
                    LogEvent($"[VERIFY_COVERAGE] Bar {i}: {boxStatus} | {vaStatus} | {FormatTimes(candle.Time)} | Candle H={candle.High:F2} L={candle.Low:F2} | Box H={zone.High:F2} L={zone.Low:F2} | VA H={zone.VAH:F2} L={zone.VAL:F2}");
                }
            }
            
            LogEvent($"[VERIFY_COVERAGE] === SUMMARY ===");
            LogEvent($"[VERIFY_COVERAGE] BOX (High/Low): Total={totalCandles}, FullyCovered={fullyCoveredByBox} ({100.0*fullyCoveredByBox/totalCandles:F1}%), PartiallyCovered={partiallyCoveredByBox} ({100.0*partiallyCoveredByBox/totalCandles:F1}%), NotCovered={notCoveredByBox} ({100.0*notCoveredByBox/totalCandles:F1}%)");
            LogEvent($"[VERIFY_COVERAGE] VA (VAH/VAL): Total={totalCandles}, FullyCovered={fullyCoveredByVA} ({100.0*fullyCoveredByVA/totalCandles:F1}%), PartiallyCovered={partiallyCoveredByVA} ({100.0*partiallyCoveredByVA/totalCandles:F1}%), NotCovered={notCoveredByVA} ({100.0*notCoveredByVA/totalCandles:F1}%)");
            
            if (notCoveredByBox > 0)
            {
                LogEvent($"[VERIFY_COVERAGE] ⚠️ WARNING: {notCoveredByBox} candles not covered by box! This should NEVER happen.");
            }
        }

        private bool IsValidLondonSessionStart(DateTime londonTime, int sessionStartBar, int currentBar)
        {
            // Solo la prima sessione deve iniziare esattamente alle 08:00
            if (!_firstCompleteSessionFound && londonTime.Hour != 8)
            {
                return false;
            }
            return true;
        }

        private Dictionary<decimal, decimal> BuildProfile(int startBar, int endBar)
        {
            var profile = new Dictionary<decimal, decimal>();
            
            for (int bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                var levels = candle.GetAllPriceLevels();
                
                foreach (var level in levels)
                {
                    if (profile.ContainsKey(level.Price))
                    {
                        profile[level.Price] += level.Volume;
                    }
                    else
                    {
                        profile[level.Price] = level.Volume;
                    }
                }
            }
            
            return profile;
        }

        private void ReplaceProfileContribution(
            Dictionary<decimal, decimal> profile,
            Dictionary<int, Dictionary<decimal, decimal>> barVolumes,
            Dictionary<int, decimal> barTotals,
            int bar,
            IndicatorCandle candle,
            Action<decimal> setTotalVolume)
        {
            if (barVolumes.TryGetValue(bar, out var previousLevels))
            {
                foreach (var previous in previousLevels)
                {
                    if (!profile.TryGetValue(previous.Key, out var current))
                        continue;

                    var next = current - previous.Value;
                    if (next <= 0)
                        profile.Remove(previous.Key);
                    else
                        profile[previous.Key] = next;
                }
            }

            var nextLevels = candle.GetAllPriceLevels()
                .GroupBy(level => level.Price)
                .ToDictionary(group => group.Key, group => group.Sum(level => level.Volume));

            foreach (var next in nextLevels)
            {
                if (profile.ContainsKey(next.Key))
                    profile[next.Key] += next.Value;
                else
                    profile[next.Key] = next.Value;
            }

            barVolumes[bar] = nextLevels;
            barTotals[bar] = candle.Volume;
            setTotalVolume(barTotals.Values.Sum());
        }

        private bool TryCalculateProfilePreview(IReadOnlyDictionary<decimal, decimal> profile, decimal totalVolume, out decimal poc, out decimal vah, out decimal val, out decimal valueAreaVolume, out decimal maxVolume)
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
            var pocIndex = -1;
            for (var i = 0; i < sortedLevels.Count; i++)
            {
                if (sortedLevels[i].Key == poc)
                {
                    pocIndex = i;
                    break;
                }
            }

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

        private void UpdateBalanceZoneColors()
        {
            if (_context.CurrentZone == null || _currentZoneRectangle == null) return;

            var direction = _context.CurrentZone.BreakoutDirection;
            if (direction == null) return;

            if (direction == BreakoutDirection.Bullish)
            {
                // Blu trasparente per bullish
                _currentZoneRectangle.Brush = new System.Drawing.SolidBrush(
                    System.Drawing.Color.FromArgb(30, 0, 100, 200));
                _currentZoneRectangle.Pen = new System.Drawing.Pen(
                    System.Drawing.Color.DodgerBlue, 1);
            }
            else
            {
                // Rosso trasparente per bearish
                _currentZoneRectangle.Brush = new System.Drawing.SolidBrush(
                    System.Drawing.Color.FromArgb(30, 200, 50, 50));
                _currentZoneRectangle.Pen = new System.Drawing.Pen(
                    System.Drawing.Color.Red, 1);
            }
        }
    }
}
