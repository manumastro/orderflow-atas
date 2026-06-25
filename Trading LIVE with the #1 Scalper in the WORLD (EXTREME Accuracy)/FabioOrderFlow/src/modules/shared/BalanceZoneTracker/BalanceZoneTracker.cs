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

    internal sealed class MeanReversionTriggerLog
    {
        public string Direction { get; set; } = string.Empty;
        public string Trigger { get; set; } = string.Empty;
        public int Bar { get; set; }
        public int CandidateBar { get; set; }
        public decimal POC { get; set; }
        public decimal VAH { get; set; }
        public decimal VAL { get; set; }
        public bool HistoricalAggressionLogged { get; set; }
    }

    internal sealed class MeanReversionOutcome
    {
        public string Direction { get; set; } = string.Empty;
        public string EntryModel { get; set; } = string.Empty;
        public int EntryBar { get; set; }
        public int CandidateBar { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal StopReference { get; set; }
        public decimal Target1POC { get; set; }
        public decimal Target2 { get; set; }
        public decimal MfePoints { get; set; }
        public decimal MaePoints { get; set; }
        public decimal MaxFavorablePrice { get; set; }
        public decimal MaxAdversePrice { get; set; }
        public bool Target1Hit { get; set; }
        public bool Target2Hit { get; set; }
        public bool Invalidated { get; set; }
        
        // Exit management
        public bool PositionClosed { get; set; }
        public decimal FinalPnL { get; set; }
        public string ExitReason { get; set; } = string.Empty;
        public int ExitBar { get; set; }
        public DateTime ExitTime { get; set; }
    }
    
    // Footprint-first sweep detection
    internal sealed class LiveSweepCandidate
    {
        public string Direction { get; set; } = string.Empty; // "High" or "Low"
        public DateTime SweepTimeUtc { get; set; }
        public decimal SweepPrice { get; set; }
        public decimal SweepVolume { get; set; }
        public int Bar { get; set; }
        public bool RejectionDetected { get; set; }
        public DateTime? RejectionTimeUtc { get; set; }
        public decimal RejectionPrice { get; set; }
        public decimal RejectionVolume { get; set; }
        
        // Livelli value area al momento dello sweep
        public decimal VAH { get; set; }
        public decimal VAL { get; set; }
        public decimal POC { get; set; }
    }

    internal sealed class BalanceZoneTracker
    {
        private readonly Indicator _indicator;
        private readonly MarketContext _context = new();
        private readonly Action<string> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly bool _enableLiveFootprintFirst;

        private readonly List<DrawingRectangle> _rectangles;
        private readonly List<LineTillTouch> _lines;
        
        private DrawingRectangle? _currentZoneRectangle;
        private LineTillTouch? _currentPocLine;
        private LineTillTouch? _currentVahLine;
        private LineTillTouch? _currentValLine;

        private const int MinSessionBars = 5;
        private const int ExpectedLondonBars = 96; // 8h * 12 bars/h on M5
        private const int MinCompleteSessionBars = 90; // Tolleranza -6 bars
        private const int LondonPreviewStartHour = 8;
        private static readonly bool DetailedDebugLogs = false;
        private const decimal MinAggressionTradeVolume = 20m;
        
        // ========================================
        // MEAN REVERSION STATE (Lines ~90-120)
        // To extract: Move to LondonMeanReversionModule
        // ========================================
        private readonly List<MeanReversionTriggerLog> _meanReversionTriggerLogs = new();
        private readonly List<MeanReversionOutcome> _meanReversionOutcomes = new();
        private readonly HashSet<string> _loggedAggressionCandidateKeys = new();
        private bool _firstCompleteSessionFound = false;
        private int _lastLoggedPreCloseBar = -1;
        private int _lastPreviewProfileBar = -1;
        
        // ========================================
        // MEAN REVERSION REJECTION TRACKING
        // To extract: Move to LondonMeanReversionModule
        // ========================================
        private int _lastLowRejectionCandidateBar = -1;
        private int _lastHighRejectionCandidateBar = -1;
        private decimal _lastLowRejectionHigh;
        private decimal _lastLowRejectionClose;
        private decimal _lastLowRejectionLow;
        private decimal _lastLowRejectionDelta;
        private decimal _lastHighRejectionHigh;
        private decimal _lastHighRejectionClose;
        private decimal _lastHighRejectionLow;
        private decimal _lastHighRejectionDelta;
        private bool _lowRejectionPocReclaimed;
        private bool _highRejectionPocLost;
        // ========================================
        
        private int _currentBar;
        
        // ========================================
        // MEAN REVERSION EARLY TRIGGER FLAGS
        // To extract: Move to LondonMeanReversionModule
        // ========================================
        private bool _lowRejectionEarlyTriggered;
        private bool _highRejectionEarlyTriggered;
        // ========================================
        private decimal _lastPreviewPoc;
        private decimal _lastPreviewVah;
        private decimal _lastPreviewVal;
        
        // ========================================
        // MEAN REVERSION FOOTPRINT-FIRST STATE
        // To extract: Move to LondonMeanReversionModule
        // ========================================
        private DateTime? _liveLowSweepTimeUtc;
        private DateTime? _liveHighSweepTimeUtc;
        private LiveSweepCandidate? _activeLongSweep;
        private LiveSweepCandidate? _activeShortSweep;
        private readonly HashSet<string> _footprintTriggeredKeys = new();
        // ========================================
        
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
        private int _lastLoggedNyPreCloseBar = -1;
        private int _lastNyPreviewProfileBar = -1;

        public BalanceZoneTracker(
            Indicator indicator, 
            Action<string> log,
            List<DrawingRectangle> rectangles,
            List<LineTillTouch> lines,
            Func<int, IndicatorCandle> getCandle,
            bool enableLiveFootprintFirst = false)
        {
            _indicator = indicator;
            _log = log;
            _rectangles = rectangles;
            _lines = lines;
            _getCandle = getCandle;
            _enableLiveFootprintFirst = enableLiveFootprintFirst;
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

        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            var trades = cumulativeTrades
                .OrderBy(trade => trade.Time)
                .ToList();

            // Log aggression per trigger M5 esistenti
            foreach (var triggerLog in _meanReversionTriggerLogs.Where(log => !log.HistoricalAggressionLogged))
            {
                LogHistoricalAggressionConfirmation(triggerLog, trades);
            }
        }

        public void OnLiveCumulativeTrade(CumulativeTrade trade)
        {
            if (_context.CurrentZone == null || _lastPreviewVal == 0 || _lastPreviewVah == 0 || _lastPreviewPoc == 0)
                return;

            // EXISTING LOGIC: Log aggression per trigger M5 esistenti
            TryLogLiveLongAggression(trade);
            TryLogLiveShortAggression(trade);
            
            // NEW LOGIC: Footprint-first trigger detection (solo se abilitato)
            if (!_enableLiveFootprintFirst)
                return;
            
            // 1. Detect Sweeps
            if (IsHighSweepTrade(trade))
            {
                ProcessLiveHighSweep(trade);
            }
            
            if (IsLowSweepTrade(trade))
            {
                ProcessLiveLowSweep(trade);
            }
            
            // 2. Detect Rejections
            if (_activeShortSweep != null && !_activeShortSweep.RejectionDetected)
            {
                if (IsRejectionTrade(_activeShortSweep, trade))
                {
                    ProcessLiveRejection(_activeShortSweep, trade);
                }
            }
            
            if (_activeLongSweep != null && !_activeLongSweep.RejectionDetected)
            {
                if (IsRejectionTrade(_activeLongSweep, trade))
                {
                    ProcessLiveRejection(_activeLongSweep, trade);
                }
            }
            
            // 3. Detect Entry
            if (_activeShortSweep != null && _activeShortSweep.RejectionDetected)
            {
                if (IsAggressionEntryTrade(_activeShortSweep, trade))
                {
                    ProcessFootprintEntry(_activeShortSweep, trade);
                    _activeShortSweep = null; // Reset dopo entry
                }
            }
            
            if (_activeLongSweep != null && _activeLongSweep.RejectionDetected)
            {
                if (IsAggressionEntryTrade(_activeLongSweep, trade))
                {
                    ProcessFootprintEntry(_activeLongSweep, trade);
                    _activeLongSweep = null; // Reset dopo entry
                }
            }
        }

        // ========================================
        // MEAN REVERSION METHODS (Lines ~314-1835)
        // To extract: Move to LondonMeanReversionModule
        // These methods handle:
        // - Trigger detection (rejection, POC reclaim/loss)
        // - Aggression confirmation (live + historical)
        // - Exit management (Target2/Stop)
        // - Footprint-first detection (sweep→rejection→entry)
        // ========================================

        private void TryLogLiveLongAggression(CumulativeTrade trade)
        {
            if (_lastLowRejectionCandidateBar < 0 || _liveLowSweepTimeUtc == null)
                return;

            var minVolume = GetMinAggressionTradeVolume(trade.Time);
            if (trade.Time < _liveLowSweepTimeUtc.Value || trade.Direction != TradeDirection.Buy || trade.Volume < minVolume || trade.Lastprice < _lastPreviewVal)
                return;

            var candidateKey = $"Long:{_lastLowRejectionCandidateBar}";
            if (_loggedAggressionCandidateKeys.Contains(candidateKey))
                return;

            _loggedAggressionCandidateKeys.Add(candidateKey);
            LogAggressionConfirmation(
                "Long",
                "LIVE_BUY_AGGRESSION_AFTER_LOW_REJECTION",
                "FootprintCumulativeTradeLive",
                _currentBar - 1,
                _lastLowRejectionCandidateBar,
                _getCandle(_lastLowRejectionCandidateBar),
                trade,
                _liveLowSweepTimeUtc,
                _lastPreviewPoc,
                _lastPreviewVah,
                _lastPreviewVal);
        }

        private void TryLogLiveShortAggression(CumulativeTrade trade)
        {
            if (_lastHighRejectionCandidateBar < 0 || _liveHighSweepTimeUtc == null)
                return;

            var minVolume = GetMinAggressionTradeVolume(trade.Time);
            if (trade.Time < _liveHighSweepTimeUtc.Value || trade.Direction != TradeDirection.Sell || trade.Volume < minVolume || trade.Lastprice > _lastPreviewVah)
                return;

            var candidateKey = $"Short:{_lastHighRejectionCandidateBar}";
            if (_loggedAggressionCandidateKeys.Contains(candidateKey))
                return;

            _loggedAggressionCandidateKeys.Add(candidateKey);
            LogAggressionConfirmation(
                "Short",
                "LIVE_SELL_AGGRESSION_AFTER_HIGH_REJECTION",
                "FootprintCumulativeTradeLive",
                _currentBar - 1,
                _lastHighRejectionCandidateBar,
                _getCandle(_lastHighRejectionCandidateBar),
                trade,
                _liveHighSweepTimeUtc,
                _lastPreviewPoc,
                _lastPreviewVah,
                _lastPreviewVal);
        }

        public void OnBarUpdate(int bar, IndicatorCandle candle, int currentBar)
        {
            _currentBar = currentBar;
            
            // Reset footprint-first state all'inizio di ogni nuova barra (solo se abilitato)
            if (_enableLiveFootprintFirst)
            {
                _activeLongSweep = null;
                _activeShortSweep = null;
            }
            
            var barTime = candle.Time;

            if (DetailedDebugLogs && bar == 1)
            {
                _log($"[BAR_DETAIL] First bar: Time={barTime:yyyy-MM-dd HH:mm:ss}, O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}, V={candle.Volume}");
            }

            var londonTime = MarketTimeZones.ToLondon(barTime);
            var nyTime = MarketTimeZones.ToNewYork(barTime);

            var isInLondonSession = IsInLondonSession(londonTime);
            var isInNewYorkSession = IsInNewYorkSession(nyTime);
            var isClosedBarForBreakout = IsClosedBarForBreakout(bar);
            
            if (DetailedDebugLogs && bar % 10 == 0 && (isInLondonSession || isInNewYorkSession))
            {
                _log($"[STATE] Bar={bar}, State={_context.State}, LondonSession={isInLondonSession}, NYSession={isInNewYorkSession}");
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

            UpdateMeanReversionOutcomes(bar, candle);
        }

        private void StartLondonSession(int bar, IndicatorCandle candle)
        {
            // Verifica se è l'inizio esatto della sessione London (08:00 GMT)
            var londonTime = MarketTimeZones.ToLondon(candle.Time);
            
            // Salta sessioni parziali fino a trovare la prima completa
            if (!_firstCompleteSessionFound && londonTime.Hour != 8)
            {
                var italyTime = MarketTimeZones.ToItaly(candle.Time);
                _log($"[SKIP] Partial London session detected at London={londonTime:yyyy-MM-dd HH:mm:ss}, Italy={italyTime:yyyy-MM-dd HH:mm:ss} | Bar={bar}");
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
            _lastLowRejectionCandidateBar = -1;
            _lastHighRejectionCandidateBar = -1;
            _lastLowRejectionHigh = 0;
            _lastLowRejectionClose = 0;
            _lastLowRejectionLow = 0;
            _lastLowRejectionDelta = 0;
            _lastHighRejectionHigh = 0;
            _lastHighRejectionClose = 0;
            _lastHighRejectionLow = 0;
            _lastHighRejectionDelta = 0;
            _liveLowSweepTimeUtc = null;
            _liveHighSweepTimeUtc = null;
            _lowRejectionPocReclaimed = false;
            _highRejectionPocLost = false;
            _lowRejectionEarlyTriggered = false;
            _highRejectionEarlyTriggered = false;

            UpdateLondonProfile(bar, candle);
            _log($"[SESSION_START] London session started at bar {bar} ({FormatTimes(candle.Time)})");
            _log($"[SESSION_START] Candle: O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}");
        }

        private void UpdateLondonProfile(int bar, IndicatorCandle candle)
        {
            if (_context.CurrentZone == null) return;

            var levels = candle.GetAllPriceLevels();

            foreach (var level in levels)
            {
                if (_context.CurrentZone.Profile.ContainsKey(level.Price))
                {
                    _context.CurrentZone.Profile[level.Price] += level.Volume;
                }
                else
                {
                    _context.CurrentZone.Profile[level.Price] = level.Volume;
                }
            }

            _context.CurrentZone.TotalVolume += candle.Volume;

            var previousHigh = _context.CurrentZone.High;
            var previousLow = _context.CurrentZone.Low;

            var importantEvent = false;

            if (candle.High > previousHigh)
            {
                _context.CurrentZone.High = candle.High;
                _context.CurrentZone.SessionHighBar = bar;
                _context.CurrentZone.SessionHighTimeUtc = candle.Time;
                LogSessionExtreme("NEW_SESSION_HIGH", bar, candle, previousHigh);
                if (LogPotentialRejection("HIGH_REJECTION_CANDIDATE", bar, candle, previousHigh, out var highRejectionDelta))
                {
                    _lastHighRejectionCandidateBar = bar;
                    _lastHighRejectionHigh = candle.High;
                    _lastHighRejectionClose = candle.Close;
                    _lastHighRejectionLow = candle.Low;
                    _lastHighRejectionDelta = highRejectionDelta;
                    _highRejectionPocLost = false;
                    _highRejectionEarlyTriggered = false;
                    _liveHighSweepTimeUtc = candle.LastTime > candle.Time ? candle.LastTime : candle.Time;
                    importantEvent = true;
                }
                importantEvent = true;
            }

            if (previousLow == 0 || candle.Low < previousLow)
            {
                _context.CurrentZone.Low = candle.Low;
                _context.CurrentZone.SessionLowBar = bar;
                _context.CurrentZone.SessionLowTimeUtc = candle.Time;
                LogSessionExtreme("NEW_SESSION_LOW", bar, candle, previousLow);
                if (LogPotentialRejection("LOW_REJECTION_CANDIDATE", bar, candle, previousLow, out var lowRejectionDelta))
                {
                    _lastLowRejectionCandidateBar = bar;
                    _lastLowRejectionHigh = candle.High;
                    _lastLowRejectionClose = candle.Close;
                    _lastLowRejectionLow = candle.Low;
                    _lastLowRejectionDelta = lowRejectionDelta;
                    _lowRejectionPocReclaimed = false;
                    _lowRejectionEarlyTriggered = false;
                    _liveLowSweepTimeUtc = candle.LastTime > candle.Time ? candle.LastTime : candle.Time;
                    importantEvent = true;
                }
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
                _log($"[NY_PRE_CLOSE] Bar={bar}, {FormatTimes(candle.Time)}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}, TopLevels={topLevels}");
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
            _lastLoggedNyPreCloseBar = -1;
            _lastNyPreviewProfileBar = -1;
            _nySessionHighTimeUtc = candle.Time;
            _nySessionLowTimeUtc = candle.Time;

            UpdateNewYorkSession(bar, candle);
            _log($"[NY_SESSION_START] New York session started at bar {bar} ({FormatTimes(candle.Time)})");
            _log($"[NY_SESSION_START] Candle: O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}");
        }

        private void UpdateNewYorkSession(int bar, IndicatorCandle candle)
        {
            if (!_nySessionActive)
                return;

            _nySessionEndBar = bar;

            foreach (var level in candle.GetAllPriceLevels())
            {
                if (_nySessionProfile.ContainsKey(level.Price))
                {
                    _nySessionProfile[level.Price] += level.Volume;
                }
                else
                {
                    _nySessionProfile[level.Price] = level.Volume;
                }
            }

            _nySessionTotalVolume += candle.Volume;

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

            _log($"[NY_PROFILE_PREVIEW] Bar={bar}, {FormatTimes(candle.Time)}, Reason={(force ? "event" : "live")}, Bars={nyBars}, High={_nySessionHigh:F2}, Low={_nySessionLow:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, VA_Volume={valueAreaVolume:F0}, TotalVolume={_nySessionTotalVolume:F0}, MaxLevelVolume={maxVolume:F0}, Close={candle.Close:F2}, Relation={relation}, DistToPOC={candle.Close - poc:F2}, DistToVAH={candle.Close - vah:F2}, DistToVAL={candle.Close - val:F2}, CandleBid={bid:F0}, CandleAsk={ask:F0}, CandleDelta={delta:F0}");
        }

        private void FinalizeSession(int bar, string sessionName, int minCompleteBars, int minSessionBars, bool isLondon)
        {
            if (_context.CurrentZone == null) return;

            _context.CurrentZone.EndBar = bar - 1;

            var barCount = _context.CurrentZone.EndBar - _context.CurrentZone.StartBar + 1;
            var endCandle = _getCandle(bar - 1);

            _log($"[SESSION_END] {sessionName} session ended at bar {bar - 1} ({FormatTimes(endCandle.Time)}). Bars in session: {barCount}");
            _log($"[SESSION_END] Candle: O={endCandle.Open:F2}, H={endCandle.High:F2}, L={endCandle.Low:F2}, C={endCandle.Close:F2}");

            if (isLondon)
            {
                _log($"[SESSION_END] Session range so far: High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, TotalVolume={_context.CurrentZone.TotalVolume:F0}");
                _log($"[SESSION_EXTREMES] High={_context.CurrentZone.High:F2} at Bar={_context.CurrentZone.SessionHighBar}, {FormatTimes(_context.CurrentZone.SessionHighTimeUtc)}");
                _log($"[SESSION_EXTREMES] Low={_context.CurrentZone.Low:F2} at Bar={_context.CurrentZone.SessionLowBar}, {FormatTimes(_context.CurrentZone.SessionLowTimeUtc)}");
            }

            if (barCount < minCompleteBars)
            {
                _log($"[SESSION_SKIP] {sessionName} session incomplete (only {barCount} bars), skipping...");
                _context.State = MarketState.NoZone;
                _context.CurrentZone = null;
                return;
            }

            if (barCount < minSessionBars || (isLondon && _context.CurrentZone.Profile.Count == 0))
            {
                _log($"[BALANCE_INVALID] {sessionName} session too short or empty | Bars={barCount}");
                _context.State = MarketState.NoZone;
                _context.CurrentZone = null;
                return;
            }

            if (isLondon)
            {
                if (DetailedDebugLogs)
                {
                    _log($"[PROFILE_RANGE] High={_context.CurrentZone.High:F2} | Low={_context.CurrentZone.Low:F2} | ProfileLevels={_context.CurrentZone.Profile.Count}");
                    _log($"[PROFILE_DETAIL] First 10 levels: {string.Join(", ", _context.CurrentZone.Profile.OrderBy(kv => kv.Key).Take(10).Select(kv => $"{kv.Key:F2}={kv.Value:F0}"))}");
                    _log($"[PROFILE_DETAIL] Last 10 levels: {string.Join(", ", _context.CurrentZone.Profile.OrderByDescending(kv => kv.Key).Take(10).Select(kv => $"{kv.Key:F2}={kv.Value:F0}"))}");
                }

                CalculatePOC();
                CalculateValueArea();

                _context.CurrentZone.IsReady = true;
                _context.State = MarketState.BalanceReady;

                DrawBalanceZone();

                _log($"[ZONE_READY] Balance zone ready: High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, POC={_context.CurrentZone.POC:F2}, VAH={_context.CurrentZone.VAH:F2}, VAL={_context.CurrentZone.VAL:F2}, TotalVolume={_context.CurrentZone.TotalVolume:F0}");
                _log($"[ZONE_READY] StartBar={_context.CurrentZone.StartBar}, EndBar={_context.CurrentZone.EndBar}, Bars={barCount}");

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
                _log($"[POC_CALC] MaxVolume={maxVolume:F0}, POC={_context.CurrentZone.POC:F2}, Candidates={pocCandidates.Count}");
                if (pocCandidates.Count > 1)
                {
                    _log($"[POC_CALC] Multiple POC candidates (tie-break to lowest): {string.Join(", ", pocCandidates.Select(kv => $"{kv.Key:F2}"))}");
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
                _log($"[VALUE_AREA_CALC] TotalVolume={_context.CurrentZone.TotalVolume:F0}, Target70%={targetVolume:F0}");
                _log($"[VALUE_AREA_CALC] POC at index {pocIndex} of {sortedLevels.Count} levels, Price={_context.CurrentZone.POC:F2}");
            }

            var accumulatedVolume = sortedLevels[pocIndex].Value;
            var lowerIndex = pocIndex;
            var upperIndex = pocIndex;

            if (DetailedDebugLogs)
            {
                _log($"[VALUE_AREA_CALC] Starting expansion from POC, InitialVolume={accumulatedVolume:F0}");
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
                        _log($"[VALUE_AREA_CALC] Expanded down: Price={sortedLevels[lowerIndex].Key:F2}, Volume={sortedLevels[lowerIndex].Value:F0}, Accumulated={accumulatedVolume:F0}");
                    }
                }
                else if (upperIndex < sortedLevels.Count - 1)
                {
                    upperIndex++;
                    accumulatedVolume += sortedLevels[upperIndex].Value;
                    if (DetailedDebugLogs)
                    {
                        _log($"[VALUE_AREA_CALC] Expanded up: Price={sortedLevels[upperIndex].Key:F2}, Volume={sortedLevels[upperIndex].Value:F0}, Accumulated={accumulatedVolume:F0}");
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
                _log($"[VALUE_AREA_CALC] Final: VAL={_context.CurrentZone.VAL:F2} (index {lowerIndex}), VAH={_context.CurrentZone.VAH:F2} (index {upperIndex})");
                _log($"[VALUE_AREA_CALC] Final accumulated volume: {accumulatedVolume:F0} ({100.0m * accumulatedVolume / _context.CurrentZone.TotalVolume:F1}%)");
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
                _log($"[BREAKOUT_PENDING] Bullish | Bar={bar}, {FormatTimes(candle.Time)}, Close={close:F2} > VAH={_context.CurrentZone.VAH:F2}");
            }
            else if (isBearishBreak)
            {
                _context.State = MarketState.BreakoutPending;
                _context.PendingDirection = BreakoutDirection.Bearish;
                _context.PendingBreakoutBar = bar;
                _context.ConsecutiveOutsideCloses = 1;
                _log($"[BREAKOUT_PENDING] Bearish | Bar={bar}, {FormatTimes(candle.Time)}, Close={close:F2} < VAL={_context.CurrentZone.VAL:F2}");
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

                    _log($"[BREAKOUT_CONFIRMED] Direction: {_context.PendingDirection}, Bar: {bar}, {FormatTimes(candle.Time)}, Close: {close:F2}, VAH: {_context.CurrentZone.VAH:F2}, VAL: {_context.CurrentZone.VAL:F2}");
                    _log($"[BREAKOUT_CONFIRMED] Candle: O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}");
                    _log($"[OUT_OF_BALANCE] {_context.PendingDirection} | BreakoutBar={_context.PendingBreakoutBar} | TargetPOC={_context.CurrentZone.POC}");
                }
            }
            else
            {
                // False breakout, torna dentro value area
                _context.State = MarketState.BalanceReady;
                _context.PendingDirection = null;
                _context.PendingBreakoutBar = -1;
                _context.ConsecutiveOutsideCloses = 0;
                _log("[FALSE_BREAKOUT] Returned inside value area");
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
            _log($"[{tag}] Bar={bar}, {FormatTimes(candle.Time)}, Previous={previousExtreme:F2}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}");
        }

        private bool LogPotentialRejection(string tag, int bar, IndicatorCandle candle, decimal previousExtreme, out decimal candleDelta)
        {
            candleDelta = 0;
            var range = candle.High - candle.Low;
            if (range <= 0)
                return false;

            var upperWick = candle.High - Math.Max(candle.Open, candle.Close);
            var lowerWick = Math.Min(candle.Open, candle.Close) - candle.Low;
            var closePosition = (candle.Close - candle.Low) / range;

            var highRejection = tag == "HIGH_REJECTION_CANDIDATE"
                && (candle.Close < previousExtreme || upperWick >= range * 0.40m || closePosition <= 0.50m);
            var lowRejection = tag == "LOW_REJECTION_CANDIDATE"
                && (candle.Close > previousExtreme || lowerWick >= range * 0.40m || closePosition >= 0.50m);

            if (!highRejection && !lowRejection)
                return false;

            var (bid, ask, delta, topLevels) = GetCandleVolumeDiagnostics(candle);
            candleDelta = delta;
            _log($"[{tag}] Bar={bar}, {FormatTimes(candle.Time)}, PreviousExtreme={previousExtreme:F2}, Range={range:F2}, UpperWick={upperWick:F2}, LowerWick={lowerWick:F2}, ClosePosition={closePosition:P0}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}, TopLevels={topLevels}");
            return true;
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
            _log($"[LONDON_PRE_CLOSE] Bar={bar}, {FormatTimes(candle.Time)}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}, TopLevels={topLevels}");
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
            var (bid, ask, delta, _) = GetCandleVolumeDiagnostics(candle);
            var relation = candle.Close > vah ? "ABOVE_PREVIEW_VAH" : candle.Close < val ? "BELOW_PREVIEW_VAL" : "INSIDE_PREVIEW_VA";
            var sessionBars = bar - _context.CurrentZone.StartBar + 1;

            _log($"[PROFILE_PREVIEW] Bar={bar}, {FormatTimes(candle.Time)}, Reason={(force ? "event" : "live")}, Bars={sessionBars}, High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, VA_Volume={valueAreaVolume:F0}, TotalVolume={_context.CurrentZone.TotalVolume:F0}, MaxLevelVolume={maxVolume:F0}, Close={candle.Close:F2}, Relation={relation}, DistToPOC={candle.Close - poc:F2}, DistToVAH={candle.Close - vah:F2}, DistToVAL={candle.Close - val:F2}, CandleBid={bid:F0}, CandleAsk={ask:F0}, CandleDelta={delta:F0}");
            LogMeanReversionEarlyTriggerIfNeeded(bar, candle, poc, vah, val, bid, ask, delta);
            LogMeanReversionTriggerIfNeeded(bar, candle, poc, vah, val, bid, ask, delta);
        }

        private void LogMeanReversionEarlyTriggerIfNeeded(int bar, IndicatorCandle candle, decimal poc, decimal vah, decimal val, decimal bid, decimal ask, decimal delta)
        {
            if (_context.CurrentZone == null)
                return;

            if (_lastLowRejectionCandidateBar >= 0 && !_lowRejectionEarlyTriggered && bar > _lastLowRejectionCandidateBar)
            {
                var closeAboveRejectionClose = candle.Close > _lastLowRejectionClose;
                var tradedAboveRejectionHigh = candle.High > _lastLowRejectionHigh;
                var positiveFollowThrough = delta >= 0 || candle.Close > candle.Open;
                var closeBackInsideValue = candle.Close >= val;

                if ((closeAboveRejectionClose || tradedAboveRejectionHigh) && positiveFollowThrough && closeBackInsideValue)
                {
                    _lowRejectionEarlyTriggered = true;
                    RegisterMeanReversionTrigger("Long", "LOW_REJECTION_FOLLOW_THROUGH", bar, _lastLowRejectionCandidateBar, poc, vah, val);
                    _log($"[MR_EARLY_TRIGGER] Direction=Long, Trigger=LOW_REJECTION_FOLLOW_THROUGH, BarMode={GetBarMode(bar)}, Bar={bar}, CurrentBar={_currentBar}, {FormatTimes(candle.Time)}, CandidateBar={_lastLowRejectionCandidateBar}, CandidateLow={_lastLowRejectionLow:F2}, CandidateClose={_lastLowRejectionClose:F2}, CandidateHigh={_lastLowRejectionHigh:F2}, CandidateDelta={_lastLowRejectionDelta:F0}, Close={candle.Close:F2}, High={candle.High:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, DistToPOC={candle.Close - poc:F2}, StopReference={_lastLowRejectionLow:F2}, Target1={poc:F2}, Target2={vah:F2}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}");
                }
            }

            if (_lastHighRejectionCandidateBar >= 0 && !_highRejectionEarlyTriggered && bar > _lastHighRejectionCandidateBar)
            {
                var closeBelowRejectionClose = candle.Close < _lastHighRejectionClose;
                var tradedBelowRejectionLow = candle.Low < _lastHighRejectionLow;
                var negativeFollowThrough = delta <= 0 || candle.Close < candle.Open;
                var closeBackInsideValue = candle.Close <= vah;

                if ((closeBelowRejectionClose || tradedBelowRejectionLow) && negativeFollowThrough && closeBackInsideValue)
                {
                    _highRejectionEarlyTriggered = true;
                    RegisterMeanReversionTrigger("Short", "HIGH_REJECTION_FOLLOW_THROUGH", bar, _lastHighRejectionCandidateBar, poc, vah, val);
                    _log($"[MR_EARLY_TRIGGER] Direction=Short, Trigger=HIGH_REJECTION_FOLLOW_THROUGH, BarMode={GetBarMode(bar)}, Bar={bar}, CurrentBar={_currentBar}, {FormatTimes(candle.Time)}, CandidateBar={_lastHighRejectionCandidateBar}, CandidateHigh={_lastHighRejectionHigh:F2}, CandidateClose={_lastHighRejectionClose:F2}, CandidateLow={_lastHighRejectionLow:F2}, CandidateDelta={_lastHighRejectionDelta:F0}, Close={candle.Close:F2}, Low={candle.Low:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, DistToPOC={candle.Close - poc:F2}, StopReference={_lastHighRejectionHigh:F2}, Target1={poc:F2}, Target2={val:F2}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}");
                }
            }
        }

        private void LogMeanReversionTriggerIfNeeded(int bar, IndicatorCandle candle, decimal poc, decimal vah, decimal val, decimal bid, decimal ask, decimal delta)
        {
            if (_context.CurrentZone == null)
                return;

            if (_lastLowRejectionCandidateBar >= 0 && !_lowRejectionPocReclaimed && bar > _lastLowRejectionCandidateBar && candle.Close > poc)
            {
                _lowRejectionPocReclaimed = true;
                RegisterMeanReversionTrigger("Long", "POC_RECLAIM_AFTER_LOW_REJECTION", bar, _lastLowRejectionCandidateBar, poc, vah, val);
                _log($"[MR_TRIGGER] Direction=Long, Trigger=POC_RECLAIM_AFTER_LOW_REJECTION, BarMode={GetBarMode(bar)}, Bar={bar}, CurrentBar={_currentBar}, {FormatTimes(candle.Time)}, CandidateBar={_lastLowRejectionCandidateBar}, Close={candle.Close:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, DistToPOC={candle.Close - poc:F2}, StopReference={_context.CurrentZone.Low:F2}, Target1={vah:F2}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}");
            }

            if (_lastHighRejectionCandidateBar >= 0 && !_highRejectionPocLost && bar > _lastHighRejectionCandidateBar && candle.Close < poc)
            {
                _highRejectionPocLost = true;
                RegisterMeanReversionTrigger("Short", "POC_LOSS_AFTER_HIGH_REJECTION", bar, _lastHighRejectionCandidateBar, poc, vah, val);
                _log($"[MR_TRIGGER] Direction=Short, Trigger=POC_LOSS_AFTER_HIGH_REJECTION, BarMode={GetBarMode(bar)}, Bar={bar}, CurrentBar={_currentBar}, {FormatTimes(candle.Time)}, CandidateBar={_lastHighRejectionCandidateBar}, Close={candle.Close:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, DistToPOC={candle.Close - poc:F2}, StopReference={_context.CurrentZone.High:F2}, Target1={val:F2}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}");
            }
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

        private void RegisterMeanReversionTrigger(string direction, string trigger, int bar, int candidateBar, decimal poc, decimal vah, decimal val)
        {
            if (_meanReversionTriggerLogs.Any(log => log.Direction == direction && log.Trigger == trigger && log.Bar == bar && log.CandidateBar == candidateBar))
                return;

            _meanReversionTriggerLogs.Add(new MeanReversionTriggerLog
            {
                Direction = direction,
                Trigger = trigger,
                Bar = bar,
                CandidateBar = candidateBar,
                POC = poc,
                VAH = vah,
                VAL = val
            });
        }

        private void LogAggressionConfirmation(
            string direction,
            string trigger,
            string entryModel,
            int bar,
            int candidateBar,
            IndicatorCandle candidateCandle,
            CumulativeTrade trade,
            DateTime? sweepTime,
            decimal poc,
            decimal vah,
            decimal val)
        {
            var italyTime = MarketTimeZones.ToItaly(trade.Time);
            var londonTime = MarketTimeZones.ToLondon(trade.Time);
            var entryPrice = trade.Lastprice;
            var entryAreaLow = Math.Min(trade.FirstPrice, trade.Lastprice);
            var entryAreaHigh = Math.Max(trade.FirstPrice, trade.Lastprice);
            var stopReference = direction == "Long" ? candidateCandle.Low : candidateCandle.High;
            var riskPoints = direction == "Long" ? entryPrice - stopReference : stopReference - entryPrice;
            var rewardToPoc = direction == "Long" ? poc - entryPrice : entryPrice - poc;
            var target2 = direction == "Long" ? vah : val;
            var rewardToTarget2 = direction == "Long" ? vah - entryPrice : entryPrice - val;
            var secondsAfterSweep = sweepTime.HasValue ? (trade.Time - sweepTime.Value).TotalSeconds : 0;

            var minVolume = GetMinAggressionTradeVolume(trade.Time);
            var volumeRule = GetAggressionVolumeRule(trade.Time);
            var sweepTimeItaly = sweepTime.HasValue ? MarketTimeZones.ToItaly(sweepTime.Value) : (DateTime?)null;
            var sweepTimeLondon = sweepTime.HasValue ? MarketTimeZones.ToLondon(sweepTime.Value) : (DateTime?)null;
            _log($"[MR_AGGRESSION_CONFIRM] Direction={direction}, EntryModel={entryModel}, Trigger={trigger}, Bar={bar}, CandidateBar={candidateBar}, Italy={italyTime:yyyy-MM-dd HH:mm:ss.fff}, London={londonTime:yyyy-MM-dd HH:mm:ss.fff}, UTC={trade.Time:yyyy-MM-dd HH:mm:ss.fff}, EntryPrice={entryPrice:F2}, EntryAreaLow={entryAreaLow:F2}, EntryAreaHigh={entryAreaHigh:F2}, FirstPrice={trade.FirstPrice:F2}, LastPrice={trade.Lastprice:F2}, Volume={trade.Volume:F0}, TradeDirection={trade.Direction}, SweepTimeItaly={(sweepTimeItaly.HasValue ? sweepTimeItaly.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") : "n/a")}, SweepTimeLondon={(sweepTimeLondon.HasValue ? sweepTimeLondon.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") : "n/a")}, SweepTimeUtc={(sweepTime.HasValue ? sweepTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") : "n/a")}, SecondsAfterSweep={secondsAfterSweep:F1}, StopReference={stopReference:F2}, RiskPoints={riskPoints:F2}, Target1POC={poc:F2}, Target2={target2:F2}, RewardToPOC={rewardToPoc:F2}, RewardToTarget2={rewardToTarget2:F2}, VAH={vah:F2}, VAL={val:F2}, MinVolume={minVolume:F0}, VolumeRule={volumeRule}");

            var outcome = RegisterMeanReversionOutcome(direction, entryModel, bar, candidateBar, entryPrice, stopReference, poc, target2);
            EvaluateMeanReversionOutcomeRange(outcome, bar, Math.Max(bar, _currentBar - 1));
        }

        private MeanReversionOutcome RegisterMeanReversionOutcome(string direction, string entryModel, int entryBar, int candidateBar, decimal entryPrice, decimal stopReference, decimal target1Poc, decimal target2)
        {
            var existing = _meanReversionOutcomes.FirstOrDefault(outcome => outcome.Direction == direction && outcome.CandidateBar == candidateBar);
            if (existing != null)
                return existing;

            var outcome = new MeanReversionOutcome
            {
                Direction = direction,
                EntryModel = entryModel,
                EntryBar = entryBar,
                CandidateBar = candidateBar,
                EntryPrice = entryPrice,
                StopReference = stopReference,
                Target1POC = target1Poc,
                Target2 = target2,
                MaxFavorablePrice = entryPrice,
                MaxAdversePrice = entryPrice
            };

            _meanReversionOutcomes.Add(outcome);
            return outcome;
        }

        private void UpdateMeanReversionOutcomes(int bar, IndicatorCandle candle)
        {
            foreach (var outcome in _meanReversionOutcomes.Where(outcome => bar >= outcome.EntryBar && !outcome.Invalidated))
            {
                EvaluateMeanReversionOutcomeCandle(outcome, bar, candle);
            }
        }

        private void EvaluateMeanReversionOutcomeRange(MeanReversionOutcome outcome, int startBar, int endBar)
        {
            for (var bar = startBar; bar <= endBar && bar < _currentBar; bar++)
            {
                EvaluateMeanReversionOutcomeCandle(outcome, bar, _getCandle(bar));
                if (outcome.Invalidated)
                    break;
            }
        }

        private void EvaluateMeanReversionOutcomeCandle(MeanReversionOutcome outcome, int bar, IndicatorCandle candle)
        {
            if (outcome.Direction == "Long")
            {
                EvaluateLongOutcome(outcome, bar, candle);
            }
            else
            {
                EvaluateShortOutcome(outcome, bar, candle);
            }
        }

        private void EvaluateLongOutcome(MeanReversionOutcome outcome, int bar, IndicatorCandle candle)
        {
            // Se posizione già chiusa, stop tracking
            if (outcome.PositionClosed)
                return;
            
            var favorablePoints = candle.High - outcome.EntryPrice;
            var adversePoints = outcome.EntryPrice - candle.Low;
            UpdateMfeMae(outcome, bar, candle, favorablePoints, adversePoints, candle.High, candle.Low);

            if (!outcome.Target1Hit && outcome.Target1POC > outcome.EntryPrice && candle.High >= outcome.Target1POC)
            {
                outcome.Target1Hit = true;
                LogTargetHit(outcome, bar, candle, "POC", outcome.Target1POC, outcome.Target1POC - outcome.EntryPrice);
            }

            if (!outcome.Target2Hit && outcome.Target2 > outcome.EntryPrice && candle.High >= outcome.Target2)
            {
                outcome.Target2Hit = true;
                LogTargetHit(outcome, bar, candle, "Target2", outcome.Target2, outcome.Target2 - outcome.EntryPrice);
                
                // AUTO-EXIT al Target2
                outcome.FinalPnL = outcome.Target2 - outcome.EntryPrice;
                LogPositionClosed(outcome, bar, candle, "TARGET2_HIT");
                return; // Stop tracking
            }

            if (!outcome.Invalidated && candle.Low <= outcome.StopReference)
            {
                outcome.Invalidated = true;
                outcome.FinalPnL = outcome.StopReference - outcome.EntryPrice;
                LogPositionClosed(outcome, bar, candle, "STOP_HIT");
                return; // Stop tracking
            }
        }

        private void EvaluateShortOutcome(MeanReversionOutcome outcome, int bar, IndicatorCandle candle)
        {
            // Se posizione già chiusa, stop tracking
            if (outcome.PositionClosed)
                return;
            
            var favorablePoints = outcome.EntryPrice - candle.Low;
            var adversePoints = candle.High - outcome.EntryPrice;
            UpdateMfeMae(outcome, bar, candle, favorablePoints, adversePoints, candle.Low, candle.High);

            if (!outcome.Target1Hit && outcome.Target1POC < outcome.EntryPrice && candle.Low <= outcome.Target1POC)
            {
                outcome.Target1Hit = true;
                LogTargetHit(outcome, bar, candle, "POC", outcome.Target1POC, outcome.EntryPrice - outcome.Target1POC);
            }

            if (!outcome.Target2Hit && outcome.Target2 < outcome.EntryPrice && candle.Low <= outcome.Target2)
            {
                outcome.Target2Hit = true;
                LogTargetHit(outcome, bar, candle, "Target2", outcome.Target2, outcome.EntryPrice - outcome.Target2);
                
                // AUTO-EXIT al Target2
                outcome.FinalPnL = outcome.EntryPrice - outcome.Target2;
                LogPositionClosed(outcome, bar, candle, "TARGET2_HIT");
                return; // Stop tracking
            }

            if (!outcome.Invalidated && candle.High >= outcome.StopReference)
            {
                outcome.Invalidated = true;
                outcome.FinalPnL = outcome.EntryPrice - outcome.StopReference;
                LogPositionClosed(outcome, bar, candle, "STOP_HIT");
                return; // Stop tracking
            }
        }

        private void UpdateMfeMae(MeanReversionOutcome outcome, int bar, IndicatorCandle candle, decimal favorablePoints, decimal adversePoints, decimal favorablePrice, decimal adversePrice)
        {
            if (adversePoints > outcome.MaePoints)
            {
                outcome.MaePoints = adversePoints;
                outcome.MaxAdversePrice = adversePrice;
            }

            if (favorablePoints <= outcome.MfePoints)
                return;

            outcome.MfePoints = favorablePoints;
            outcome.MaxFavorablePrice = favorablePrice;
            _log($"[MR_MFE_UPDATE] Direction={outcome.Direction}, EntryModel={outcome.EntryModel}, Bar={bar}, CandidateBar={outcome.CandidateBar}, {FormatTimes(candle.Time)}, EntryPrice={outcome.EntryPrice:F2}, MFE={outcome.MfePoints:F2}, MaxFavorablePrice={outcome.MaxFavorablePrice:F2}, MAE={outcome.MaePoints:F2}, MaxAdversePrice={outcome.MaxAdversePrice:F2}");
        }

        private void LogTargetHit(MeanReversionOutcome outcome, int bar, IndicatorCandle candle, string target, decimal targetPrice, decimal rewardPoints)
        {
            _log($"[MR_TARGET_HIT] Direction={outcome.Direction}, EntryModel={outcome.EntryModel}, Target={target}, Bar={bar}, CandidateBar={outcome.CandidateBar}, {FormatTimes(candle.Time)}, EntryPrice={outcome.EntryPrice:F2}, TargetPrice={targetPrice:F2}, RewardPoints={rewardPoints:F2}, MFE={outcome.MfePoints:F2}, MAE={outcome.MaePoints:F2}");
        }

        private void LogInvalidated(MeanReversionOutcome outcome, int bar, IndicatorCandle candle)
        {
            _log($"[MR_INVALIDATED] Direction={outcome.Direction}, EntryModel={outcome.EntryModel}, Bar={bar}, CandidateBar={outcome.CandidateBar}, {FormatTimes(candle.Time)}, EntryPrice={outcome.EntryPrice:F2}, StopReference={outcome.StopReference:F2}, MFE={outcome.MfePoints:F2}, MAE={outcome.MaePoints:F2}");
        }
        
        private void LogPositionClosed(MeanReversionOutcome outcome, int bar, IndicatorCandle candle, string exitReason)
        {
            outcome.PositionClosed = true;
            outcome.ExitReason = exitReason;
            outcome.ExitBar = bar;
            outcome.ExitTime = candle.Time;
            
            _log($"[MR_POSITION_CLOSED] Direction={outcome.Direction}, EntryModel={outcome.EntryModel}, ExitReason={exitReason}, Bar={bar}, CandidateBar={outcome.CandidateBar}, {FormatTimes(candle.Time)}, EntryPrice={outcome.EntryPrice:F2}, FinalPnL={outcome.FinalPnL:F2}, MFE={outcome.MfePoints:F2}, MAE={outcome.MaePoints:F2}, Target1Hit={outcome.Target1Hit}, Target2Hit={outcome.Target2Hit}");
        }

        private decimal GetMinAggressionTradeVolume(DateTime utcTime)
        {
            return MinAggressionTradeVolume;
        }

        private string GetAggressionVolumeRule(DateTime utcTime)
        {
            return "Hardcoded20";
        }

        private void LogHistoricalAggressionConfirmation(MeanReversionTriggerLog triggerLog, IReadOnlyCollection<CumulativeTrade> trades)
        {
            if (triggerLog.CandidateBar < 0 || triggerLog.CandidateBar >= _currentBar || triggerLog.Bar < 0 || triggerLog.Bar >= _currentBar)
                return;

            var candidateKey = $"{triggerLog.Direction}:{triggerLog.CandidateBar}";
            if (_loggedAggressionCandidateKeys.Contains(candidateKey))
            {
                triggerLog.HistoricalAggressionLogged = true;
                return;
            }

            var candidateCandle = _getCandle(triggerLog.CandidateBar);
            var triggerCandle = _getCandle(triggerLog.Bar);
            var direction = triggerLog.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var sweepTime = GetCandidateSweepTime(triggerLog.Direction, candidateCandle, trades);
            var startTime = sweepTime ?? candidateCandle.Time;
            var endTime = triggerCandle.LastTime > triggerCandle.Time ? triggerCandle.LastTime : triggerCandle.Time.AddMinutes(5);

            CumulativeTrade? matchingTrade = null;

            foreach (var trade in trades)
            {
                if (trade.Time < startTime)
                    continue;

                if (trade.Time > endTime)
                    break;

                if (trade.Direction != direction)
                    continue;

                var minVolume = GetMinAggressionTradeVolume(trade.Time);
                if (trade.Volume < minVolume)
                    continue;

                if (!IsHistoricalAggressionInsideValue(triggerLog.Direction, triggerLog, trade))
                    continue;

                matchingTrade = trade;
                break;
            }

            if (matchingTrade == null)
            {
                triggerLog.HistoricalAggressionLogged = true;
                return;
            }

            triggerLog.HistoricalAggressionLogged = true;
            _loggedAggressionCandidateKeys.Add(candidateKey);
            LogAggressionConfirmation(
                triggerLog.Direction,
                triggerLog.Trigger,
                "FootprintCumulativeTradeHistorical",
                triggerLog.Bar,
                triggerLog.CandidateBar,
                candidateCandle,
                matchingTrade,
                sweepTime,
                triggerLog.POC,
                triggerLog.VAH,
                triggerLog.VAL);
        }

        private static DateTime? GetCandidateSweepTime(string direction, IndicatorCandle candidateCandle, IReadOnlyCollection<CumulativeTrade> trades)
        {
            var candidateEndTime = candidateCandle.LastTime > candidateCandle.Time
                ? candidateCandle.LastTime
                : candidateCandle.Time.AddMinutes(5);

            return trades
                .Where(trade => trade.Time >= candidateCandle.Time && trade.Time <= candidateEndTime)
                .Where(trade => direction == "Long"
                    ? Math.Min(trade.FirstPrice, trade.Lastprice) <= candidateCandle.Low
                    : Math.Max(trade.FirstPrice, trade.Lastprice) >= candidateCandle.High)
                .OrderBy(trade => trade.Time)
                .Select(trade => (DateTime?)trade.Time)
                .FirstOrDefault();
        }

        private static bool IsHistoricalAggressionInsideValue(string direction, MeanReversionTriggerLog triggerLog, CumulativeTrade trade)
        {
            return direction == "Long"
                ? trade.Lastprice >= triggerLog.VAL
                : trade.Lastprice <= triggerLog.VAH;
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
                _log($"[DRAW_ZONE] Rectangle=(Bar:{zone.StartBar}, High:{zone.High:F2})-(Bar:{zone.EndBar}, Low:{zone.Low:F2})");
                _log($"[DRAW_ZONE] POC_Line=(Bar:{zone.StartBar}, Price:{zone.POC:F2})-(Bar:{zone.EndBar}, Price:{zone.POC:F2})");
                _log($"[DRAW_ZONE] VAH_Line=(Bar:{zone.StartBar}, Price:{zone.VAH:F2})-(Bar:{zone.EndBar}, Price:{zone.VAH:F2})");
                _log($"[DRAW_ZONE] VAL_Line=(Bar:{zone.StartBar}, Price:{zone.VAL:F2})-(Bar:{zone.EndBar}, Price:{zone.VAL:F2})");
                _log($"[DRAW_ZONE] Zone visual box: High={zone.High:F2}, Low={zone.Low:F2}");
                _log($"[DRAW_ZONE] === First 5 candles in zone ===");
                for (int i = zone.StartBar; i < Math.Min(zone.StartBar + 5, zone.EndBar); i++)
                {
                    var c = _getCandle(i);
                    _log($"[DRAW_ZONE] Bar {i}: Time={FormatTimes(c.Time)}, O={c.Open:F2}, H={c.High:F2}, L={c.Low:F2}, C={c.Close:F2}");
                }
            }
        }

        private void VerifyZoneCoverageComplete(BalanceZone zone)
        {
            _log($"[VERIFY_COVERAGE] === COMPLETE COVERAGE CHECK ===");
            _log($"[VERIFY_COVERAGE] Zone visual box: High={zone.High:F2}, Low={zone.Low:F2}");
            _log($"[VERIFY_COVERAGE] Zone VAH={zone.VAH:F2}, VAL={zone.VAL:F2} (breakout detection)");
            _log($"[VERIFY_COVERAGE] StartBar={zone.StartBar}, EndBar={zone.EndBar}, Total={zone.EndBar - zone.StartBar + 1} bars");
            
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
                    _log($"[VERIFY_COVERAGE] Bar {i}: {boxStatus} | {vaStatus} | {FormatTimes(candle.Time)} | Candle H={candle.High:F2} L={candle.Low:F2} | Box H={zone.High:F2} L={zone.Low:F2} | VA H={zone.VAH:F2} L={zone.VAL:F2}");
                }
            }
            
            _log($"[VERIFY_COVERAGE] === SUMMARY ===");
            _log($"[VERIFY_COVERAGE] BOX (High/Low): Total={totalCandles}, FullyCovered={fullyCoveredByBox} ({100.0*fullyCoveredByBox/totalCandles:F1}%), PartiallyCovered={partiallyCoveredByBox} ({100.0*partiallyCoveredByBox/totalCandles:F1}%), NotCovered={notCoveredByBox} ({100.0*notCoveredByBox/totalCandles:F1}%)");
            _log($"[VERIFY_COVERAGE] VA (VAH/VAL): Total={totalCandles}, FullyCovered={fullyCoveredByVA} ({100.0*fullyCoveredByVA/totalCandles:F1}%), PartiallyCovered={partiallyCoveredByVA} ({100.0*partiallyCoveredByVA/totalCandles:F1}%), NotCovered={notCoveredByVA} ({100.0*notCoveredByVA/totalCandles:F1}%)");
            
            if (notCoveredByBox > 0)
            {
                _log($"[VERIFY_COVERAGE] ⚠️ WARNING: {notCoveredByBox} candles not covered by box! This should NEVER happen.");
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
        
        #region Footprint-First Trigger Detection

        private bool IsHighSweepTrade(CumulativeTrade trade)
        {
            if (_context.CurrentZone == null || _lastPreviewVah == 0)
                return false;
            
            // Big sell trade che rompe sopra VAH
            return trade.Direction == TradeDirection.Sell 
                && trade.Volume >= MinAggressionTradeVolume
                && trade.Lastprice > _lastPreviewVah;
        }

        private bool IsLowSweepTrade(CumulativeTrade trade)
        {
            if (_context.CurrentZone == null || _lastPreviewVal == 0)
                return false;
            
            // Big buy trade che rompe sotto VAL
            return trade.Direction == TradeDirection.Buy 
                && trade.Volume >= MinAggressionTradeVolume
                && trade.Lastprice < _lastPreviewVal;
        }

        private bool IsRejectionTrade(LiveSweepCandidate sweep, CumulativeTrade trade)
        {
            if (sweep.Direction == "High")
            {
                // Rejection = big buy trade dopo high sweep
                return trade.Direction == TradeDirection.Buy
                    && trade.Volume >= MinAggressionTradeVolume
                    && trade.Lastprice < sweep.SweepPrice; // Trade sotto il sweep
            }
            else // "Low"
            {
                // Rejection = big sell trade dopo low sweep
                return trade.Direction == TradeDirection.Sell
                    && trade.Volume >= MinAggressionTradeVolume
                    && trade.Lastprice > sweep.SweepPrice; // Trade sopra il sweep
            }
        }

        private bool IsAggressionEntryTrade(LiveSweepCandidate sweep, CumulativeTrade trade)
        {
            if (!sweep.RejectionDetected)
                return false;
            
            if (sweep.Direction == "High")
            {
                // Entry short = big sell trade dopo rejection
                return trade.Direction == TradeDirection.Sell
                    && trade.Volume >= MinAggressionTradeVolume
                    && trade.Lastprice <= sweep.VAH; // Back inside value area del sweep
            }
            else // "Low"
            {
                // Entry long = big buy trade dopo rejection
                return trade.Direction == TradeDirection.Buy
                    && trade.Volume >= MinAggressionTradeVolume
                    && trade.Lastprice >= sweep.VAL; // Back inside value area del sweep
            }
        }

        private void ProcessLiveHighSweep(CumulativeTrade trade)
        {
            // Se già abbiamo un sweep attivo, non sovrascriverlo
            if (_activeShortSweep != null)
                return;
            
            _activeShortSweep = new LiveSweepCandidate
            {
                Direction = "High",
                SweepTimeUtc = trade.Time,
                SweepPrice = trade.Lastprice,
                SweepVolume = trade.Volume,
                Bar = _currentBar,
                VAH = _lastPreviewVah,
                VAL = _lastPreviewVal,
                POC = _lastPreviewPoc
            };
            
            _liveHighSweepTimeUtc = trade.Time;
            
            var italyTime = MarketTimeZones.ToItaly(trade.Time);
            _log($"[FOOTPRINT_HIGH_SWEEP] Bar={_currentBar}, Italy={italyTime:yyyy-MM-dd HH:mm:ss.fff}, UTC={trade.Time:yyyy-MM-dd HH:mm:ss.fff}, Price={trade.Lastprice:F2}, Volume={trade.Volume:F0}, VAH={_lastPreviewVah:F2}");
        }

        private void ProcessLiveLowSweep(CumulativeTrade trade)
        {
            // Se già abbiamo un sweep attivo, non sovrascriverlo
            if (_activeLongSweep != null)
                return;
            
            _activeLongSweep = new LiveSweepCandidate
            {
                Direction = "Low",
                SweepTimeUtc = trade.Time,
                SweepPrice = trade.Lastprice,
                SweepVolume = trade.Volume,
                Bar = _currentBar,
                VAH = _lastPreviewVah,
                VAL = _lastPreviewVal,
                POC = _lastPreviewPoc
            };
            
            _liveLowSweepTimeUtc = trade.Time;
            
            var italyTime = MarketTimeZones.ToItaly(trade.Time);
            _log($"[FOOTPRINT_LOW_SWEEP] Bar={_currentBar}, Italy={italyTime:yyyy-MM-dd HH:mm:ss.fff}, UTC={trade.Time:yyyy-MM-dd HH:mm:ss.fff}, Price={trade.Lastprice:F2}, Volume={trade.Volume:F0}, VAL={_lastPreviewVal:F2}");
        }

        private void ProcessLiveRejection(LiveSweepCandidate sweep, CumulativeTrade trade)
        {
            sweep.RejectionDetected = true;
            sweep.RejectionTimeUtc = trade.Time;
            sweep.RejectionPrice = trade.Lastprice;
            sweep.RejectionVolume = trade.Volume;
            
            var italyTime = MarketTimeZones.ToItaly(trade.Time);
            var secondsAfterSweep = (trade.Time - sweep.SweepTimeUtc).TotalSeconds;
            var direction = sweep.Direction == "High" ? "Short" : "Long";
            
            _log($"[FOOTPRINT_REJECTION] Direction={direction}, Bar={_currentBar}, Italy={italyTime:yyyy-MM-dd HH:mm:ss.fff}, UTC={trade.Time:yyyy-MM-dd HH:mm:ss.fff}, RejectionPrice={trade.Lastprice:F2}, Volume={trade.Volume:F0}, SweepPrice={sweep.SweepPrice:F2}, SecondsAfterSweep={secondsAfterSweep:F1}");
        }

        private void ProcessFootprintEntry(LiveSweepCandidate sweep, CumulativeTrade trade)
        {
            var direction = sweep.Direction == "High" ? "Short" : "Long";
            var candidateKey = $"Footprint:{direction}:{sweep.Bar}";
            
            // Evita entry duplicate per lo stesso sweep
            if (_footprintTriggeredKeys.Contains(candidateKey))
                return;
            
            _footprintTriggeredKeys.Add(candidateKey);
            
            var italyTime = MarketTimeZones.ToItaly(trade.Time);
            var sweepToEntry = (trade.Time - sweep.SweepTimeUtc).TotalSeconds;
            var rejectionToEntry = sweep.RejectionTimeUtc.HasValue 
                ? (trade.Time - sweep.RejectionTimeUtc.Value).TotalSeconds 
                : 0;
            
            var entryPrice = trade.Lastprice;
            var stopReference = direction == "Long" ? sweep.SweepPrice : sweep.SweepPrice;
            var target1Poc = sweep.POC; // Usa POC dello sweep, non quello corrente
            var target2 = direction == "Long" ? sweep.VAH : sweep.VAL; // Usa VAH/VAL dello sweep
            
            _log($"[FOOTPRINT_ENTRY] Direction={direction}, Trigger=FOOTPRINT_FIRST, Bar={_currentBar}, SweepBar={sweep.Bar}, Italy={italyTime:yyyy-MM-dd HH:mm:ss.fff}, UTC={trade.Time:yyyy-MM-dd HH:mm:ss.fff}, EntryPrice={entryPrice:F2}, Volume={trade.Volume:F0}, StopReference={stopReference:F2}, Target1POC={target1Poc:F2}, Target2={target2:F2}, SweepToEntrySeconds={sweepToEntry:F1}, RejectionToEntrySeconds={rejectionToEntry:F1}");
            
            // Registra outcome per tracking
            var outcome = RegisterMeanReversionOutcome(direction, "FootprintFirst", _currentBar, sweep.Bar, entryPrice, stopReference, target1Poc, target2);
            EvaluateMeanReversionOutcomeRange(outcome, _currentBar, Math.Max(_currentBar - 1, sweep.Bar));
        }

        #endregion
        
    }
}
