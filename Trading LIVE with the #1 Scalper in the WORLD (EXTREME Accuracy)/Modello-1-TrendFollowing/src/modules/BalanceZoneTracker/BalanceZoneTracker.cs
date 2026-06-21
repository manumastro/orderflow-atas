using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;

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

        private readonly TimeZoneInfo _londonTimeZone;
        private readonly TimeZoneInfo _newYorkTimeZone;

        private const int MinSessionBars = 5;

        public BalanceZoneTracker(Indicator indicator)
        {
            _indicator = indicator;

            try
            {
                _londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                _newYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BalanceZoneTracker] Timezone not found: {ex.Message}");
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
            Log($"[BALANCE_BUILDING] London start | Bar={bar}");
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
            if (barCount < MinSessionBars || _context.CurrentZone.Profile.Count == 0)
            {
                Log($"[BALANCE_INVALID] London session too short or empty | Bars={barCount}");
                _context.State = MarketState.NoZone;
                _context.CurrentZone = null;
                return;
            }

            CalculatePOC();
            CalculateValueArea();

            _context.CurrentZone.IsReady = true;
            _context.State = MarketState.BalanceReady;

            Log($"[BALANCE_READY] London | POC={_context.CurrentZone.POC} | VAH={_context.CurrentZone.VAH} | VAL={_context.CurrentZone.VAL} | Bars={barCount} | Volume={_context.CurrentZone.TotalVolume}");
        }

        private void CalculatePOC()
        {
            if (_context.CurrentZone == null || _context.CurrentZone.Profile.Count == 0) return;

            var maxVolume = _context.CurrentZone.Profile.Values.Max();
            var pocCandidates = _context.CurrentZone.Profile.Where(kv => kv.Value == maxVolume).ToList();

            // Tie-break: prezzo più basso
            _context.CurrentZone.POC = pocCandidates.Min(kv => kv.Key);
        }

        private void CalculateValueArea()
        {
            if (_context.CurrentZone == null || _context.CurrentZone.Profile.Count == 0) return;

            var targetVolume = _context.CurrentZone.TotalVolume * 0.70m;
            var sortedLevels = _context.CurrentZone.Profile.OrderBy(kv => kv.Key).ToList();

            var pocIndex = sortedLevels.FindIndex(kv => kv.Key == _context.CurrentZone.POC);
            if (pocIndex == -1) return;

            var accumulatedVolume = sortedLevels[pocIndex].Value;
            var lowerIndex = pocIndex;
            var upperIndex = pocIndex;

            while (accumulatedVolume < targetVolume && (lowerIndex > 0 || upperIndex < sortedLevels.Count - 1))
            {
                var lowerVolume = lowerIndex > 0 ? sortedLevels[lowerIndex - 1].Value : 0;
                var upperVolume = upperIndex < sortedLevels.Count - 1 ? sortedLevels[upperIndex + 1].Value : 0;

                if (lowerVolume >= upperVolume && lowerIndex > 0)
                {
                    lowerIndex--;
                    accumulatedVolume += sortedLevels[lowerIndex].Value;
                }
                else if (upperIndex < sortedLevels.Count - 1)
                {
                    upperIndex++;
                    accumulatedVolume += sortedLevels[upperIndex].Value;
                }
                else
                {
                    break;
                }
            }

            _context.CurrentZone.VAL = sortedLevels[lowerIndex].Key;
            _context.CurrentZone.VAH = sortedLevels[upperIndex].Key;
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
                Log($"[BREAKOUT_PENDING] Bullish | Close={close} > VAH={_context.CurrentZone.VAH}");
            }
            else if (isBearishBreak)
            {
                _context.State = MarketState.BreakoutPending;
                _context.PendingDirection = BreakoutDirection.Bearish;
                _context.PendingBreakoutBar = bar;
                _context.ConsecutiveOutsideCloses = 1;
                Log($"[BREAKOUT_PENDING] Bearish | Close={close} < VAL={_context.CurrentZone.VAL}");
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

                    Log($"[OUT_OF_BALANCE] {_context.PendingDirection} | BreakoutBar={_context.PendingBreakoutBar} | TargetPOC={_context.CurrentZone.POC}");
                }
            }
            else
            {
                // False breakout, torna dentro value area
                _context.State = MarketState.BalanceReady;
                _context.PendingDirection = null;
                _context.PendingBreakoutBar = -1;
                _context.ConsecutiveOutsideCloses = 0;
                Log("[FALSE_BREAKOUT] Returned inside value area");
            }
        }

        private void ResetForNewSession(int bar)
        {
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
            // Log tramite Debug per ora, può essere esteso con callback
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
