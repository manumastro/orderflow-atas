using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    /// <summary>
    /// London Mean Reversion Model - Implementation based on Fabio Valentino's strategy
    /// 
    /// CORE CONCEPT:
    /// During London session (08:00-16:00), market exhibits mean reverting behavior.
    /// When price breaks out of balance zone (VAH/VAL) but gets rejected, it tends to
    /// revert back to POC (Point of Control) where 70% of volume was transacted.
    /// 
    /// STRATEGY FLOW:
    /// 1. Identify Balance Zone (Volume Profile: POC, VAH, VAL)
    /// 2. Detect Breakout (price breaks VAH/VAL - out of balance)
    /// 3. Detect Rejection (breakout fails, price closes back inside value area)
    /// 4. Wait for Retracement (SECOND movement - price back inside balance)
    /// 5. Confirm Aggression (big orders 20+ contracts towards POC)
    /// 6. Enter with big players, Target = POC
    /// 
    /// KEY RULES:
    /// - Don't take FIRST movement (risky, might be real trend)
    /// - Wait for SECOND movement (retracement confirms fake breakout)
    /// - Entry only with BIG ORDERS confirmation (20+ contracts)
    /// - Target is ALWAYS POC (70% probability)
    /// - Stop loss TIGHT (1-2 ticks inside high/low to avoid slippage)
    /// - If wrong, exit IMMEDIATELY
    /// </summary>
    internal sealed class LondonMeanReversionModule
    {
        // ========================================
        // DEPENDENCIES
        // ========================================
        
        private readonly BalanceZoneTracker _balanceTracker;
        private readonly Action<string, bool> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        
        // ========================================
        // CONFIGURATION
        // ========================================
        
        private const decimal MinAggressionVolume = 20m; // Minimum contracts for big order
        private const int AggressionTimeoutSeconds = 3600; // Max 1 hour after sweep
        private const decimal RejectionThresholdTicks = 10m; // Minimum ticks inside for rejection
        
        // ========================================
        // STATE TRACKING
        // ========================================
        
        private int _currentBar;
        
        // Active setups waiting for aggression confirmation
        private readonly List<BalanceSetup> _activeSetups = new();
        
        // Confirmed entries being tracked to target
        private readonly List<ActivePosition> _activePositions = new();
        
        // Historical tracking for reporting
        private readonly List<TradeRecord> _completedTrades = new();
        
        // Deduplication
        private readonly HashSet<string> _processedKeys = new();
        
        // ========================================
        // DATA STRUCTURES
        // ========================================
        
        /// <summary>
        /// Balance setup waiting for aggression confirmation
        /// Represents: Balance Zone → Breakout → Rejection → Waiting for Retracement + Aggression
        /// </summary>
        public class BalanceSetup
        {
            public string SetupId { get; set; } = Guid.NewGuid().ToString();
            public string Direction { get; set; } = string.Empty; // "Long" or "Short"
            
            // Balance Zone at time of setup
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            
            // Breakout details
            public int BreakoutBar { get; set; }
            public DateTime BreakoutTimeUtc { get; set; }
            public decimal BreakoutPrice { get; set; } // The high/low that broke VAH/VAL
            
            // Rejection details
            public int RejectionBar { get; set; }
            public DateTime RejectionTimeUtc { get; set; }
            public decimal RejectionClose { get; set; }
            public decimal RejectionDelta { get; set; }
            
            // Status
            public bool AggressionConfirmed { get; set; }
            public bool Expired { get; set; }
            
            // Calculated levels
            public decimal StopPrice { get; set; } // 1-2 ticks inside breakout high/low
            public decimal TargetPrice { get; set; } // Always POC
        }
        
        /// <summary>
        /// Active position tracking to target (POC)
        /// </summary>
        public class ActivePosition
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            
            public decimal EntryPrice { get; set; }
            public int EntryBar { get; set; }
            public DateTime EntryTimeUtc { get; set; }
            
            public decimal StopPrice { get; set; }
            public decimal TargetPrice { get; set; } // POC
            
            // Tracking
            public decimal MaxFavorablePrice { get; set; }
            public decimal MaxAdversePrice { get; set; }
            public decimal MFE { get; set; } // Max Favorable Excursion
            public decimal MAE { get; set; } // Max Adverse Excursion
            
            // Status
            public bool Closed { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal ExitPrice { get; set; }
            public int ExitBar { get; set; }
        }
        
        /// <summary>
        /// Completed trade for historical analysis
        /// </summary>
        public class TradeRecord
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            
            // Setup
            public DateTime BreakoutTime { get; set; }
            public decimal BreakoutPrice { get; set; }
            public DateTime RejectionTime { get; set; }
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            
            // Entry
            public DateTime EntryTime { get; set; }
            public decimal EntryPrice { get; set; }
            public decimal EntryVolume { get; set; }
            
            // Exit
            public DateTime ExitTime { get; set; }
            public decimal ExitPrice { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            
            // Performance
            public decimal PnL { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public decimal RMultiple { get; set; }
        }
        
        // ========================================
        // CONSTRUCTOR
        // ========================================
        
        public LondonMeanReversionModule(
            BalanceZoneTracker balanceTracker,
            Action<string, bool> log,
            Func<int, IndicatorCandle> getCandle)
        {
            _balanceTracker = balanceTracker ?? throw new ArgumentNullException(nameof(balanceTracker));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _getCandle = getCandle ?? throw new ArgumentNullException(nameof(getCandle));
        }
        
        // ========================================
        // PUBLIC INTERFACE
        // ========================================
        
        /// <summary>
        /// Called on every bar update (both historical and real-time)
        /// </summary>
        public void OnBarUpdate(int bar, int currentBar, IndicatorCandle candle)
        {
            _currentBar = currentBar;
            
            // Update active positions tracking
            UpdateActivePositions(bar, candle);
            
            // NOTE: Do NOT expire setups during historical calculation
            // Expiry is only for real-time to prevent memory leak
            // During historical, setups will be checked when cumulative trades arrive
            // ExpireOldSetups(candle.Time);
        }
        
        /// <summary>
        /// Called when new session high is detected
        /// Phase 1: Detect BREAKOUT (high breaks VAH)
        /// Phase 2: Check for REJECTION (close back inside VAH)
        /// </summary>
        public void OnNewSessionHigh(int bar, IndicatorCandle candle, decimal previousHigh)
        {
            // Log for visibility
            _log($"[SESSION_HIGH] Bar={bar}, Time={FormatTime(candle.Time)}, High={candle.High:F2}, PrevHigh={previousHigh:F2}", false);
            
            // Check if this is a rejection setup (breakout + close back inside)
            if (IsHighRejectionSetup(candle, out var setup))
            {
                _activeSetups.Add(setup);
                
                _log($"[MR_SETUP_SHORT] SetupId={setup.SetupId}, Bar={bar}, " +
                     $"BreakoutHigh={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, " +
                     $"VAH={setup.VAH:F2}, POC={setup.POC:F2}, VAL={setup.VAL:F2}, " +
                     $"Stop={setup.StopPrice:F2}, Target={setup.TargetPrice:F2}", true);
            }
        }
        
        /// <summary>
        /// Called when new session low is detected
        /// Phase 1: Detect BREAKOUT (low breaks VAL)
        /// Phase 2: Check for REJECTION (close back inside VAL)
        /// </summary>
        public void OnNewSessionLow(int bar, IndicatorCandle candle, decimal previousLow)
        {
            // Log for visibility
            _log($"[SESSION_LOW] Bar={bar}, Time={FormatTime(candle.Time)}, Low={candle.Low:F2}, PrevLow={previousLow:F2}", false);
            
            // Check if this is a rejection setup (breakout + close back inside)
            if (IsLowRejectionSetup(candle, out var setup))
            {
                _activeSetups.Add(setup);
                
                _log($"[MR_SETUP_LONG] SetupId={setup.SetupId}, Bar={bar}, " +
                     $"BreakoutLow={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, " +
                     $"VAL={setup.VAL:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, " +
                     $"Stop={setup.StopPrice:F2}, Target={setup.TargetPrice:F2}", true);
            }
        }
        
        /// <summary>
        /// Called with historical cumulative trades after bar closes
        /// Phase 3: Detect RETRACEMENT (price back inside balance)
        /// Phase 4: Confirm AGGRESSION (big orders towards POC)
        /// </summary>
        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            var trades = cumulativeTrades.OrderBy(t => t.Time).ToList();
            
            _log($"[MR_CUM_TRADES] Received {trades.Count} cumulative trades, Active setups: {_activeSetups.Count(s => !s.AggressionConfirmed && !s.Expired)}", false);
            
            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired))
            {
                CheckForAggressionEntry(setup, trades);
            }
        }
        
        // ========================================
        // PHASE 1-2: BREAKOUT & REJECTION DETECTION
        // ========================================
        
        /// <summary>
        /// Check if new session high represents a rejection setup
        /// Criteria:
        /// - High broke VAH (breakout)
        /// - Close is back inside VAH by at least 10 ticks (rejection)
        /// - Delta confirms (negative for short)
        /// </summary>
        private bool IsHighRejectionSetup(IndicatorCandle candle, out BalanceSetup setup)
        {
            setup = null;
            
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            var poc = _balanceTracker.LastPreviewPoc;
            
            // Must have valid balance zone
            if (vah == 0 || val == 0 || poc == 0)
                return false;
            
            // High must break VAH (breakout)
            if (candle.High <= vah)
                return false;
            
            // Close must be back inside VAH by at least threshold (rejection)
            var rejectionDistance = candle.High - candle.Close;
            if (rejectionDistance < RejectionThresholdTicks)
                return false;
            
            // Close must be below VAH
            if (candle.Close >= vah)
                return false;
            
            // Delta should be negative (more selling than buying)
            // Skip delta check for now - focus on price action
            
            // Create setup
            setup = new BalanceSetup
            {
                Direction = "Short",
                POC = poc,
                VAH = vah,
                VAL = val,
                BreakoutBar = _currentBar,
                BreakoutTimeUtc = candle.Time,
                BreakoutPrice = candle.High,
                RejectionBar = _currentBar,
                RejectionTimeUtc = candle.LastTime > candle.Time ? candle.LastTime : candle.Time,
                RejectionClose = candle.Close,
                RejectionDelta = rejectionDistance,
                
                // Stop: 1-2 ticks ABOVE the high (to avoid slippage)
                StopPrice = candle.High + 2m,
                
                // Target: Always POC
                TargetPrice = poc
            };
            
            return true;
        }
        
        /// <summary>
        /// Check if new session low represents a rejection setup
        /// Criteria:
        /// - Low broke VAL (breakout)
        /// - Close is back inside VAL by at least 10 ticks (rejection)
        /// - Delta confirms (positive for long)
        /// </summary>
        private bool IsLowRejectionSetup(IndicatorCandle candle, out BalanceSetup setup)
        {
            setup = null;
            
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            var poc = _balanceTracker.LastPreviewPoc;
            
            // Must have valid balance zone
            if (vah == 0 || val == 0 || poc == 0)
                return false;
            
            // Low must break VAL (breakout)
            if (candle.Low >= val)
                return false;
            
            // Close must be back inside VAL by at least threshold (rejection)
            var rejectionDistance = candle.Close - candle.Low;
            if (rejectionDistance < RejectionThresholdTicks)
                return false;
            
            // Close must be above VAL
            if (candle.Close <= val)
                return false;
            
            // Delta should be positive (more buying than selling)
            // Skip delta check for now - focus on price action
            
            // Create setup
            setup = new BalanceSetup
            {
                Direction = "Long",
                POC = poc,
                VAH = vah,
                VAL = val,
                BreakoutBar = _currentBar,
                BreakoutTimeUtc = candle.Time,
                BreakoutPrice = candle.Low,
                RejectionBar = _currentBar,
                RejectionTimeUtc = candle.LastTime > candle.Time ? candle.LastTime : candle.Time,
                RejectionClose = candle.Close,
                RejectionDelta = rejectionDistance,
                
                // Stop: 1-2 ticks BELOW the low (to avoid slippage)
                StopPrice = candle.Low - 2m,
                
                // Target: Always POC
                TargetPrice = poc
            };
            
            return true;
        }
        
        // ========================================
        // PHASE 3-4: RETRACEMENT & AGGRESSION CONFIRMATION
        // ========================================
        
        /// <summary>
        /// Check cumulative trades for aggression entry confirmation
        /// Criteria:
        /// - Trade occurred AFTER rejection time
        /// - Trade volume >= MinAggressionVolume (big order)
        /// - Trade direction matches setup (Buy for Long, Sell for Short)
        /// - Trade price is INSIDE balance zone (retracement confirmed)
        /// - Trade direction is TOWARDS POC (mean reversion)
        /// - Within timeout window (max 1 hour after rejection)
        /// </summary>
        private void CheckForAggressionEntry(BalanceSetup setup, List<CumulativeTrade> trades)
        {
            // Find trades after rejection time
            var relevantTrades = trades
                .Where(t => t.Time > setup.RejectionTimeUtc)
                .Where(t => t.Volume >= MinAggressionVolume)
                .ToList();
            
            _log($"[MR_CHECK_AGGRESSION] SetupId={setup.SetupId}, Direction={setup.Direction}, RejectionTime={setup.RejectionTimeUtc:HH:mm:ss}, RelevantTrades={relevantTrades.Count}", false);
            
            foreach (var trade in relevantTrades)
            {
                // Check timeout (max 1 hour)
                var secondsAfterRejection = (trade.Time - setup.RejectionTimeUtc).TotalSeconds;
                if (secondsAfterRejection > AggressionTimeoutSeconds)
                    continue;
                
                // Check if trade matches setup criteria
                if (IsAggressionEntry(setup, trade))
                {
                    // Confirm entry!
                    setup.AggressionConfirmed = true;
                    
                    CreatePosition(setup, trade);
                    
                    break; // Only one entry per setup
                }
            }
        }
        
        /// <summary>
        /// Check if trade qualifies as aggression entry for setup
        /// </summary>
        private bool IsAggressionEntry(BalanceSetup setup, CumulativeTrade trade)
        {
            if (setup.Direction == "Long")
            {
                // Long setup requirements:
                // 1. Buy order (direction = Buy)
                // 2. Price is inside balance zone (above VAL, towards POC)
                // 3. Price is below POC (moving towards target)
                
                if (trade.Direction != TradeDirection.Buy)
                    return false;
                
                // Must be inside balance (above VAL)
                if (trade.Lastprice < setup.VAL)
                    return false;
                
                // Must be below POC (room to move to target)
                if (trade.Lastprice >= setup.POC)
                    return false;
                
                return true;
            }
            else // Short
            {
                // Short setup requirements:
                // 1. Sell order (direction = Sell)
                // 2. Price is inside balance zone (below VAH, towards POC)
                // 3. Price is above POC (moving towards target)
                
                if (trade.Direction != TradeDirection.Sell)
                    return false;
                
                // Must be inside balance (below VAH)
                if (trade.Lastprice > setup.VAH)
                    return false;
                
                // Must be above POC (room to move to target)
                if (trade.Lastprice <= setup.POC)
                    return false;
                
                return true;
            }
        }
        
        // ========================================
        // PHASE 5: POSITION MANAGEMENT
        // ========================================
        
        /// <summary>
        /// Create active position from confirmed setup
        /// </summary>
        private void CreatePosition(BalanceSetup setup, CumulativeTrade entryTrade)
        {
            var position = new ActivePosition
            {
                SetupId = setup.SetupId,
                Direction = setup.Direction,
                EntryPrice = entryTrade.Lastprice,
                EntryBar = _currentBar,
                EntryTimeUtc = entryTrade.Time,
                StopPrice = setup.StopPrice,
                TargetPrice = setup.TargetPrice,
                MaxFavorablePrice = entryTrade.Lastprice,
                MaxAdversePrice = entryTrade.Lastprice
            };
            
            _activePositions.Add(position);
            
            var riskPoints = setup.Direction == "Long" 
                ? position.EntryPrice - position.StopPrice 
                : position.StopPrice - position.EntryPrice;
                
            var rewardPoints = setup.Direction == "Long"
                ? position.TargetPrice - position.EntryPrice
                : position.EntryPrice - position.TargetPrice;
            
            _log($"[MR_ENTRY] SetupId={setup.SetupId}, Direction={setup.Direction}, " +
                 $"EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, " +
                 $"Stop={position.StopPrice:F2}, Target={position.TargetPrice:F2}, " +
                 $"Risk={riskPoints:F2}, Reward={rewardPoints:F2}, " +
                 $"SecondsAfterRejection={((entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds):F1}", true);
        }
        
        /// <summary>
        /// Update all active positions - check for target hit or stop hit
        /// </summary>
        private void UpdateActivePositions(int bar, IndicatorCandle candle)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                UpdatePositionTracking(position, candle);
                CheckPositionExit(position, bar, candle);
            }
        }
        
        /// <summary>
        /// Update MFE/MAE tracking
        /// </summary>
        private void UpdatePositionTracking(ActivePosition position, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                // Update max favorable (highest high since entry)
                if (candle.High > position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.High;
                    position.MFE = candle.High - position.EntryPrice;
                }
                
                // Update max adverse (lowest low since entry)
                if (candle.Low < position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.Low;
                    position.MAE = position.EntryPrice - candle.Low;
                }
            }
            else // Short
            {
                // Update max favorable (lowest low since entry)
                if (candle.Low < position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.Low;
                    position.MFE = position.EntryPrice - candle.Low;
                }
                
                // Update max adverse (highest high since entry)
                if (candle.High > position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.High;
                    position.MAE = candle.High - position.EntryPrice;
                }
            }
        }
        
        /// <summary>
        /// Check if position should exit (target hit or stop hit)
        /// </summary>
        private void CheckPositionExit(ActivePosition position, int bar, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                // Check target hit (POC reached)
                if (candle.High >= position.TargetPrice)
                {
                    ClosePosition(position, bar, candle, "TARGET_HIT", position.TargetPrice);
                    return;
                }
                
                // Check stop hit
                if (candle.Low <= position.StopPrice)
                {
                    ClosePosition(position, bar, candle, "STOP_HIT", position.StopPrice);
                    return;
                }
            }
            else // Short
            {
                // Check target hit (POC reached)
                if (candle.Low <= position.TargetPrice)
                {
                    ClosePosition(position, bar, candle, "TARGET_HIT", position.TargetPrice);
                    return;
                }
                
                // Check stop hit
                if (candle.High >= position.StopPrice)
                {
                    ClosePosition(position, bar, candle, "STOP_HIT", position.StopPrice);
                    return;
                }
            }
        }
        
        /// <summary>
        /// Close position and record trade
        /// </summary>
        private void ClosePosition(ActivePosition position, int bar, IndicatorCandle candle, string exitReason, decimal exitPrice)
        {
            position.Closed = true;
            position.ExitReason = exitReason;
            position.ExitPrice = exitPrice;
            position.ExitBar = bar;
            
            // Calculate PnL
            var pnl = position.Direction == "Long"
                ? exitPrice - position.EntryPrice
                : position.EntryPrice - exitPrice;
            
            // Calculate R-multiple
            var risk = position.Direction == "Long"
                ? position.EntryPrice - position.StopPrice
                : position.StopPrice - position.EntryPrice;
                
            var rMultiple = risk != 0 ? pnl / risk : 0;
            
            _log($"[MR_EXIT] SetupId={position.SetupId}, Direction={position.Direction}, " +
                 $"Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, " +
                 $"ExitReason={exitReason}, PnL={pnl:F2}, " +
                 $"MFE={position.MFE:F2}, MAE={position.MAE:F2}, " +
                 $"RMultiple={rMultiple:F2}R", true);
            
            // Record completed trade
            var setup = _activeSetups.FirstOrDefault(s => s.SetupId == position.SetupId);
            if (setup != null)
            {
                var trade = new TradeRecord
                {
                    SetupId = position.SetupId,
                    Direction = position.Direction,
                    BreakoutTime = setup.BreakoutTimeUtc,
                    BreakoutPrice = setup.BreakoutPrice,
                    RejectionTime = setup.RejectionTimeUtc,
                    POC = setup.POC,
                    VAH = setup.VAH,
                    VAL = setup.VAL,
                    EntryTime = position.EntryTimeUtc,
                    EntryPrice = position.EntryPrice,
                    ExitTime = candle.Time,
                    ExitPrice = exitPrice,
                    ExitReason = exitReason,
                    PnL = pnl,
                    MFE = position.MFE,
                    MAE = position.MAE,
                    RMultiple = rMultiple
                };
                
                _completedTrades.Add(trade);
            }
        }
        
        // ========================================
        // MAINTENANCE
        // ========================================
        
        /// <summary>
        /// Expire setups that are too old (no aggression found within timeout)
        /// </summary>
        private void ExpireOldSetups(DateTime currentTime)
        {
            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired))
            {
                var secondsSinceRejection = (currentTime - setup.RejectionTimeUtc).TotalSeconds;
                if (secondsSinceRejection > AggressionTimeoutSeconds)
                {
                    setup.Expired = true;
                    _log($"[MR_SETUP_EXPIRED] SetupId={setup.SetupId}, Direction={setup.Direction}, " +
                         $"SecondsElapsed={secondsSinceRejection:F0}", false);
                }
            }
        }
        
        // ========================================
        // UTILITIES
        // ========================================
        
        private string FormatTime(DateTime utc)
        {
            var italy = MarketTimeZones.ToItaly(utc);
            var london = MarketTimeZones.ToLondon(utc);
            return $"Italy={italy:yyyy-MM-dd HH:mm:ss}, London={london:yyyy-MM-dd HH:mm:ss}, UTC={utc:yyyy-MM-dd HH:mm:ss}";
        }
        
        // ========================================
        // PUBLIC ACCESSORS (for reporting/analysis)
        // ========================================
        
        public IReadOnlyList<TradeRecord> CompletedTrades => _completedTrades;
        public IReadOnlyList<ActivePosition> ActivePositions => _activePositions;
        public IReadOnlyList<BalanceSetup> ActiveSetups => _activeSetups;
    }
}
