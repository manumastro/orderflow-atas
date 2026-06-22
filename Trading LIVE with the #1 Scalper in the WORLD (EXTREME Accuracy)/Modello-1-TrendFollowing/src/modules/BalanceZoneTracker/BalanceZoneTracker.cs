using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;
using ATAS.Indicators.Drawing;

namespace FabioTrendFollowing
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
        private readonly Action<string> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;

        private readonly TimeZoneInfo _londonTimeZone;
        private readonly TimeZoneInfo _newYorkTimeZone;

        private readonly List<DrawingRectangle> _rectangles;
        private readonly List<LineTillTouch> _lines;
        
        private DrawingRectangle? _currentZoneRectangle;
        private LineTillTouch? _currentPocLine;

        private const int MinSessionBars = 5;
        private const int ExpectedLondonBars = 96; // 8h * 12 bars/h on M5
        private const int MinCompleteSessionBars = 90; // Tolleranza -6 bars
        
        private bool _firstCompleteSessionFound = false;

        public BalanceZoneTracker(
            Indicator indicator, 
            Action<string> log,
            List<DrawingRectangle> rectangles,
            List<LineTillTouch> lines,
            Func<int, IndicatorCandle> getCandle)
        {
            _indicator = indicator;
            _log = log;
            _rectangles = rectangles;
            _lines = lines;
            _getCandle = getCandle;

            try
            {
                _londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                _newYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException ex)
            {
                _log($"[ERROR] Timezone not found: {ex.Message}");
                throw;
            }
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

        public void OnBarUpdate(int bar, IndicatorCandle candle)
        {
            var barTime = candle.Time;

            // Log dettagliato prima barra per verifica dati
            if (bar == 1)
            {
                _log($"[BAR_DETAIL] First bar: Time={barTime:yyyy-MM-dd HH:mm:ss}, O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}, V={candle.Volume}");
            }

            var londonTime = TimeZoneInfo.ConvertTimeFromUtc(barTime, _londonTimeZone);
            var nyTime = TimeZoneInfo.ConvertTimeFromUtc(barTime, _newYorkTimeZone);

            var isInLondonSession = IsInLondonSession(londonTime);
            var isInNewYorkSession = IsInNewYorkSession(nyTime);

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
                        CheckForBreakout(bar, candle);
                    }
                    break;

                case MarketState.BreakoutPending:
                    if (isInNewYorkSession)
                    {
                        ConfirmBreakout(bar, candle);
                    }
                    break;

                case MarketState.OutOfBalance:
                    // Out-of-balance confermato, gli altri moduli possono operare
                    // NON estendere il box della balance zone (rimane fisso sulla sessione London)
                    // Visual separato per la zona out-of-balance
                    
                    // Reset su nuova London session
                    if (isInLondonSession && !IsBarInCurrentZone(bar))
                    {
                        ResetForNewSession(bar);
                    }
                    break;
            }
        }

        private void StartLondonSession(int bar, IndicatorCandle candle)
        {
            // Verifica se è l'inizio esatto della sessione London (08:00 GMT)
            var londonTime = TimeZoneInfo.ConvertTimeFromUtc(candle.Time, _londonTimeZone);
            
            // Salta sessioni parziali fino a trovare la prima completa
            if (!_firstCompleteSessionFound && londonTime.Hour != 8)
            {
                _log($"[SKIP] Partial London session detected at {londonTime:HH:mm} | Bar={bar}");
                return;
            }
            
            _context.State = MarketState.BuildingSessionProfile;
            _context.CurrentZone = new BalanceZone
            {
                Type = BalanceType.LondonSession,
                StartBar = bar,
                IsReady = false,
                High = candle.High,
                Low = candle.Low
            };

            UpdateLondonProfile(bar, candle);
            _log($"[SESSION_START] London session started at bar {bar} (London: {londonTime:yyyy-MM-dd HH:mm}, UTC: {candle.Time:yyyy-MM-dd HH:mm:ss})");
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
            _context.CurrentZone.High = Math.Max(_context.CurrentZone.High, candle.High);
            _context.CurrentZone.Low = _context.CurrentZone.Low == 0 
                ? candle.Low 
                : Math.Min(_context.CurrentZone.Low, candle.Low);
        }

        private void FinalizeLondonSession(int bar)
        {
            if (_context.CurrentZone == null) return;

            _context.CurrentZone.EndBar = bar - 1;

            var barCount = _context.CurrentZone.EndBar - _context.CurrentZone.StartBar + 1;
            
            var endCandle = _getCandle(bar - 1);
            var londonTime = TimeZoneInfo.ConvertTimeFromUtc(endCandle.Time, _londonTimeZone);
            
            _log($"[SESSION_END] London session ended at bar {bar - 1} (London: {londonTime:yyyy-MM-dd HH:mm}, UTC: {endCandle.Time:yyyy-MM-dd HH:mm:ss}). Bars in session: {barCount}");
            _log($"[SESSION_END] Candle: O={endCandle.Open:F2}, H={endCandle.High:F2}, L={endCandle.Low:F2}, C={endCandle.Close:F2}");
            _log($"[SESSION_END] Session range so far: High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, TotalVolume={_context.CurrentZone.TotalVolume:F0}");
            
            // Verifica se è una sessione completa
            if (!_firstCompleteSessionFound)
            {
                if (barCount < MinCompleteSessionBars)
                {
                    _log($"[SESSION_SKIP] First session incomplete (only {barCount} bars), skipping...");
                    _context.State = MarketState.NoZone;
                    _context.CurrentZone = null;
                    return;
                }
                else
                {
                    _firstCompleteSessionFound = true;
                    _log($"[FIRST_COMPLETE] First complete London session found | Bars={barCount}");
                }
            }
            
            if (barCount < MinSessionBars || _context.CurrentZone.Profile.Count == 0)
            {
                _log($"[BALANCE_INVALID] London session too short or empty | Bars={barCount}");
                _context.State = MarketState.NoZone;
                _context.CurrentZone = null;
                return;
            }

            // Log range prezzi prima dei calcoli
            _log($"[PROFILE_RANGE] High={_context.CurrentZone.High:F2} | Low={_context.CurrentZone.Low:F2} | ProfileLevels={_context.CurrentZone.Profile.Count}");
            _log($"[PROFILE_DETAIL] First 10 levels: {string.Join(", ", _context.CurrentZone.Profile.OrderBy(kv => kv.Key).Take(10).Select(kv => $"{kv.Key:F2}={kv.Value:F0}"))}");
            _log($"[PROFILE_DETAIL] Last 10 levels: {string.Join(", ", _context.CurrentZone.Profile.OrderByDescending(kv => kv.Key).Take(10).Select(kv => $"{kv.Key:F2}={kv.Value:F0}"))}");

            CalculatePOC();
            CalculateValueArea();

            _context.CurrentZone.IsReady = true;
            _context.State = MarketState.BalanceReady;

            DrawBalanceZone();

            _log($"[ZONE_READY] Balance zone ready: High={_context.CurrentZone.High:F2}, Low={_context.CurrentZone.Low:F2}, POC={_context.CurrentZone.POC:F2}, VAH={_context.CurrentZone.VAH:F2}, VAL={_context.CurrentZone.VAL:F2}, TotalVolume={_context.CurrentZone.TotalVolume:F0}");
            _log($"[ZONE_READY] StartBar={_context.CurrentZone.StartBar}, EndBar={_context.CurrentZone.EndBar}, Bars={barCount}");
            
            // Verifica copertura candele nella zona - TUTTE LE CANDELE
            VerifyZoneCoverageComplete(_context.CurrentZone);
        }

        private void CalculatePOC()
        {
            if (_context.CurrentZone == null || _context.CurrentZone.Profile.Count == 0) return;

            var maxVolume = _context.CurrentZone.Profile.Values.Max();
            var pocCandidates = _context.CurrentZone.Profile.Where(kv => kv.Value == maxVolume).ToList();

            // Tie-break: prezzo più basso
            _context.CurrentZone.POC = pocCandidates.Min(kv => kv.Key);
            
            _log($"[POC_CALC] MaxVolume={maxVolume:F0}, POC={_context.CurrentZone.POC:F2}, Candidates={pocCandidates.Count}");
            if (pocCandidates.Count > 1)
            {
                _log($"[POC_CALC] Multiple POC candidates (tie-break to lowest): {string.Join(", ", pocCandidates.Select(kv => $"{kv.Key:F2}"))}");
            }
        }

        private void CalculateValueArea()
        {
            if (_context.CurrentZone == null || _context.CurrentZone.Profile.Count == 0) return;

            var targetVolume = _context.CurrentZone.TotalVolume * 0.70m;
            var sortedLevels = _context.CurrentZone.Profile.OrderBy(kv => kv.Key).ToList();

            var pocIndex = sortedLevels.FindIndex(kv => kv.Key == _context.CurrentZone.POC);
            if (pocIndex == -1) return;

            _log($"[VALUE_AREA_CALC] TotalVolume={_context.CurrentZone.TotalVolume:F0}, Target70%={targetVolume:F0}");
            _log($"[VALUE_AREA_CALC] POC at index {pocIndex} of {sortedLevels.Count} levels, Price={_context.CurrentZone.POC:F2}");

            var accumulatedVolume = sortedLevels[pocIndex].Value;
            var lowerIndex = pocIndex;
            var upperIndex = pocIndex;

            _log($"[VALUE_AREA_CALC] Starting expansion from POC, InitialVolume={accumulatedVolume:F0}");

            while (accumulatedVolume < targetVolume && (lowerIndex > 0 || upperIndex < sortedLevels.Count - 1))
            {
                var lowerVolume = lowerIndex > 0 ? sortedLevels[lowerIndex - 1].Value : 0;
                var upperVolume = upperIndex < sortedLevels.Count - 1 ? sortedLevels[upperIndex + 1].Value : 0;

                if (lowerVolume >= upperVolume && lowerIndex > 0)
                {
                    lowerIndex--;
                    accumulatedVolume += sortedLevels[lowerIndex].Value;
                    _log($"[VALUE_AREA_CALC] Expanded down: Price={sortedLevels[lowerIndex].Key:F2}, Volume={sortedLevels[lowerIndex].Value:F0}, Accumulated={accumulatedVolume:F0}");
                }
                else if (upperIndex < sortedLevels.Count - 1)
                {
                    upperIndex++;
                    accumulatedVolume += sortedLevels[upperIndex].Value;
                    _log($"[VALUE_AREA_CALC] Expanded up: Price={sortedLevels[upperIndex].Key:F2}, Volume={sortedLevels[upperIndex].Value:F0}, Accumulated={accumulatedVolume:F0}");
                }
                else
                {
                    break;
                }
            }

            _context.CurrentZone.VAL = sortedLevels[lowerIndex].Key;
            _context.CurrentZone.VAH = sortedLevels[upperIndex].Key;
            
            _log($"[VALUE_AREA_CALC] Final: VAL={_context.CurrentZone.VAL:F2} (index {lowerIndex}), VAH={_context.CurrentZone.VAH:F2} (index {upperIndex})");
            _log($"[VALUE_AREA_CALC] Final accumulated volume: {accumulatedVolume:F0} ({100.0m * accumulatedVolume / _context.CurrentZone.TotalVolume:F1}%)");
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
                _log($"[BREAKOUT_PENDING] Bullish | Bar={bar}, Time={candle.Time:yyyy-MM-dd HH:mm:ss}, Close={close:F2} > VAH={_context.CurrentZone.VAH:F2}");
            }
            else if (isBearishBreak)
            {
                _context.State = MarketState.BreakoutPending;
                _context.PendingDirection = BreakoutDirection.Bearish;
                _context.PendingBreakoutBar = bar;
                _context.ConsecutiveOutsideCloses = 1;
                _log($"[BREAKOUT_PENDING] Bearish | Bar={bar}, Time={candle.Time:yyyy-MM-dd HH:mm:ss}, Close={close:F2} < VAL={_context.CurrentZone.VAL:F2}");
            }
        }

        private void ConfirmBreakout(int bar, IndicatorCandle candle)
        {
            if (_context.CurrentZone == null) return;

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

                    _log($"[BREAKOUT_CONFIRMED] Direction: {_context.PendingDirection}, Bar: {bar}, Time: {candle.Time:yyyy-MM-dd HH:mm:ss}, Close: {close:F2}, VAH: {_context.CurrentZone.VAH:F2}, VAL: {_context.CurrentZone.VAL:F2}");
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

        private bool IsInNewYorkSession(DateTime nyTime)
        {
            var hour = nyTime.Hour;
            var minute = nyTime.Minute;
            var totalMinutes = hour * 60 + minute;

            var startMinutes = 9 * 60 + 30; // 09:30
            var endMinutes = 16 * 60;        // 16:00

            return totalMinutes >= startMinutes && totalMinutes < endMinutes;
        }

        private void Log(string message)
        {
            _log(message);
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

            _log($"[DRAW_ZONE] Rectangle=(Bar:{zone.StartBar}, High:{zone.High:F2})-(Bar:{zone.EndBar}, Low:{zone.Low:F2})");
            _log($"[DRAW_ZONE] POC_Line=(Bar:{zone.StartBar}, Price:{zone.POC:F2})-(Bar:{zone.EndBar}, Price:{zone.POC:F2})");
            _log($"[DRAW_ZONE] VAH={zone.VAH:F2}, VAL={zone.VAL:F2} (used for breakout detection only)");
            _log($"[DRAW_ZONE] Zone visual box: High={zone.High:F2}, Low={zone.Low:F2}");
            
            // Log delle prime 5 candele della zona per verifica
            _log($"[DRAW_ZONE] === First 5 candles in zone ===");
            for (int i = zone.StartBar; i < Math.Min(zone.StartBar + 5, zone.EndBar); i++)
            {
                var c = _getCandle(i);
                _log($"[DRAW_ZONE] Bar {i}: Time={c.Time:yyyy-MM-dd HH:mm:ss}, O={c.Open:F2}, H={c.High:F2}, L={c.Low:F2}, C={c.Close:F2}");
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
                    _log($"[VERIFY_COVERAGE] Bar {i}: {boxStatus} | {vaStatus} | Time={candle.Time:yyyy-MM-dd HH:mm:ss} | Candle H={candle.High:F2} L={candle.Low:F2} | Box H={zone.High:F2} L={zone.Low:F2} | VA H={zone.VAH:F2} L={zone.VAL:F2}");
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
        
    }
}
