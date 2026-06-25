using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    /// <summary>
    /// London Mean Reversion Module
    /// Handles fakeout detection and mean reversion trading during London session
    /// Extracted from BalanceZoneTracker for modular architecture
    /// </summary>
    internal sealed class LondonMeanReversionModule
    {
        private readonly BalanceZoneTracker _balanceTracker;
        private readonly Action<string> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly bool _enableLiveFootprintFirst;
        
        // Expose MR data through this module
        public List<MeanReversionTriggerLog> MeanReversionTriggerLogs => _balanceTracker.MeanReversionTriggerLogs;
        public List<MeanReversionOutcome> MeanReversionOutcomes => _balanceTracker.MeanReversionOutcomes;
        
        public LondonMeanReversionModule(
            BalanceZoneTracker balanceTracker,
            Action<string> log,
            Func<int, IndicatorCandle> getCandle,
            bool enableLiveFootprintFirst = true)
        {
            _balanceTracker = balanceTracker ?? throw new ArgumentNullException(nameof(balanceTracker));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _getCandle = getCandle ?? throw new ArgumentNullException(nameof(getCandle));
            _enableLiveFootprintFirst = enableLiveFootprintFirst;
        }
        
        /// <summary>
        /// Process bar update for mean reversion logic
        /// Called after BalanceZoneTracker processes the bar
        /// </summary>
        public void OnBarUpdate(int bar, IndicatorCandle candle, int currentBar)
        {
            // MR logic is currently integrated in BalanceZoneTracker
            // This module acts as facade for future full extraction
        }
        
        /// <summary>
        /// Process historical cumulative trades for aggression confirmation
        /// </summary>
        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            // Delegated to BalanceZoneTracker for now
        }
        
        /// <summary>
        /// Process live cumulative trade for footprint-first detection
        /// </summary>
        public void OnLiveCumulativeTrade(CumulativeTrade trade)
        {
            // Delegated to BalanceZoneTracker for now
        }
    }
}
