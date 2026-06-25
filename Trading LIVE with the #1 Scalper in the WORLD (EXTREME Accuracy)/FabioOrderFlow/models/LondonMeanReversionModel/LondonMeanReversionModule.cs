using System;
using System.Collections.Generic;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    // Mean Reversion Data Structures (moved from BalanceZoneTracker)
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

    internal sealed class LondonMeanReversionModule
    {
        private readonly BalanceZoneTracker _balanceTracker;
        private readonly Action<string> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        private readonly bool _enableLiveFootprintFirst;
        
        // MR State variables
        private readonly List<MeanReversionTriggerLog> _meanReversionTriggerLogs = new();
        private readonly List<MeanReversionOutcome> _meanReversionOutcomes = new();
        private readonly HashSet<string> _loggedAggressionCandidateKeys = new();
        
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
        private bool _lowRejectionEarlyTriggered;
        private bool _highRejectionEarlyTriggered;
        
        private DateTime? _liveLowSweepTimeUtc;
        private DateTime? _liveHighSweepTimeUtc;
        private LiveSweepCandidate? _activeLongSweep;
        private LiveSweepCandidate? _activeShortSweep;
        private readonly HashSet<string> _footprintTriggeredKeys = new();
        
        private int _currentBar;
        private const decimal MinAggressionTradeVolume = 20m;
        
        public List<MeanReversionTriggerLog> MeanReversionTriggerLogs => _meanReversionTriggerLogs;
        public List<MeanReversionOutcome> MeanReversionOutcomes => _meanReversionOutcomes;
        
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
        
        public void OnBarUpdate(int bar, int currentBar)
        {
            _currentBar = currentBar;
        }
        
        public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> cumulativeTrades)
        {
            var trades = cumulativeTrades.OrderBy(t => t.Time).ToList();
            foreach (var triggerLog in _meanReversionTriggerLogs.Where(log => !log.HistoricalAggressionLogged))
            {
                LogHistoricalAggressionConfirmation(triggerLog, trades);
            }
        }
        
        public void OnLiveCumulativeTrade(CumulativeTrade trade)
        {
            if (!_enableLiveFootprintFirst)
                return;
                
            TryLogLiveLongAggression(trade);
            TryLogLiveShortAggression(trade);
            
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

            if (_activeShortSweep != null && _activeShortSweep.RejectionDetected)
            {
                if (IsAggressionEntryTrade(_activeShortSweep, trade))
                {
                    ProcessFootprintEntry(_activeShortSweep, trade);
                    _activeShortSweep = null;
                }
            }

            if (_activeLongSweep != null && _activeLongSweep.RejectionDetected)
            {
                if (IsAggressionEntryTrade(_activeLongSweep, trade))
                {
                    ProcessFootprintEntry(_activeLongSweep, trade);
                    _activeLongSweep = null;
                }
            }

            if (IsHighSweepTrade(trade))
                ProcessLiveHighSweep(trade);

            if (IsLowSweepTrade(trade))
                ProcessLiveLowSweep(trade);
        }
        
        // ========================================
        // MR METHODS
        // ========================================

        private void TryLogLiveLongAggression(CumulativeTrade trade)
        {
            if (_lastLowRejectionCandidateBar < 0 || _liveLowSweepTimeUtc == null)
                return;

            var minVolume = GetMinAggressionTradeVolume(trade.Time);
            if (trade.Time < _liveLowSweepTimeUtc.Value || trade.Direction != TradeDirection.Buy || trade.Volume < minVolume || trade.Lastprice < _balanceTracker.LastPreviewVal)
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
                _balanceTracker.LastPreviewPoc,
                _balanceTracker.LastPreviewVah,
                _balanceTracker.LastPreviewVal);
        }

        private void TryLogLiveShortAggression(CumulativeTrade trade)
        {
            if (_lastHighRejectionCandidateBar < 0 || _liveHighSweepTimeUtc == null)
                return;

            var minVolume = GetMinAggressionTradeVolume(trade.Time);
            if (trade.Time < _liveHighSweepTimeUtc.Value || trade.Direction != TradeDirection.Sell || trade.Volume < minVolume || trade.Lastprice > _balanceTracker.LastPreviewVah)
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
                _balanceTracker.LastPreviewPoc,
                _balanceTracker.LastPreviewVah,
                _balanceTracker.LastPreviewVal);
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

        private void LogMeanReversionEarlyTriggerIfNeeded(int bar, IndicatorCandle candle, decimal poc, decimal vah, decimal val, decimal bid, decimal ask, decimal delta)
        {
            if (_balanceTracker.CurrentZone == null)
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
            if (_balanceTracker.CurrentZone == null)
                return;

            if (_lastLowRejectionCandidateBar >= 0 && !_lowRejectionPocReclaimed && bar > _lastLowRejectionCandidateBar && candle.Close > poc)
            {
                _lowRejectionPocReclaimed = true;
                RegisterMeanReversionTrigger("Long", "POC_RECLAIM_AFTER_LOW_REJECTION", bar, _lastLowRejectionCandidateBar, poc, vah, val);
                _log($"[MR_TRIGGER] Direction=Long, Trigger=POC_RECLAIM_AFTER_LOW_REJECTION, BarMode={GetBarMode(bar)}, Bar={bar}, CurrentBar={_currentBar}, {FormatTimes(candle.Time)}, CandidateBar={_lastLowRejectionCandidateBar}, Close={candle.Close:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, DistToPOC={candle.Close - poc:F2}, StopReference={_balanceTracker.CurrentZone.Low:F2}, Target1={vah:F2}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}");
            }

            if (_lastHighRejectionCandidateBar >= 0 && !_highRejectionPocLost && bar > _lastHighRejectionCandidateBar && candle.Close < poc)
            {
                _highRejectionPocLost = true;
                RegisterMeanReversionTrigger("Short", "POC_LOSS_AFTER_HIGH_REJECTION", bar, _lastHighRejectionCandidateBar, poc, vah, val);
                _log($"[MR_TRIGGER] Direction=Short, Trigger=POC_LOSS_AFTER_HIGH_REJECTION, BarMode={GetBarMode(bar)}, Bar={bar}, CurrentBar={_currentBar}, {FormatTimes(candle.Time)}, CandidateBar={_lastHighRejectionCandidateBar}, Close={candle.Close:F2}, POC={poc:F2}, VAH={vah:F2}, VAL={val:F2}, DistToPOC={candle.Close - poc:F2}, StopReference={_balanceTracker.CurrentZone.High:F2}, Target1={val:F2}, Bid={bid:F0}, Ask={ask:F0}, Delta={delta:F0}");
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
        private bool IsHighSweepTrade(CumulativeTrade trade)
        {
            if (_balanceTracker.CurrentZone == null || _balanceTracker.LastPreviewVah == 0)
                return false;
            
            // Big sell trade che rompe sopra VAH
            return trade.Direction == TradeDirection.Sell 
                && trade.Volume >= MinAggressionTradeVolume
                && trade.Lastprice > _balanceTracker.LastPreviewVah;
        }

        private bool IsLowSweepTrade(CumulativeTrade trade)
        {
            if (_balanceTracker.CurrentZone == null || _balanceTracker.LastPreviewVal == 0)
                return false;
            
            // Big buy trade che rompe sotto VAL
            return trade.Direction == TradeDirection.Buy 
                && trade.Volume >= MinAggressionTradeVolume
                && trade.Lastprice < _balanceTracker.LastPreviewVal;
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
                VAH = _balanceTracker.LastPreviewVah,
                VAL = _balanceTracker.LastPreviewVal,
                POC = _balanceTracker.LastPreviewPoc
            };
            
            _liveHighSweepTimeUtc = trade.Time;
            
            var italyTime = MarketTimeZones.ToItaly(trade.Time);
            _log($"[FOOTPRINT_HIGH_SWEEP] Bar={_currentBar}, Italy={italyTime:yyyy-MM-dd HH:mm:ss.fff}, UTC={trade.Time:yyyy-MM-dd HH:mm:ss.fff}, Price={trade.Lastprice:F2}, Volume={trade.Volume:F0}, VAH={_balanceTracker.LastPreviewVah:F2}");
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
                VAH = _balanceTracker.LastPreviewVah,
                VAL = _balanceTracker.LastPreviewVal,
                POC = _balanceTracker.LastPreviewPoc
            };
            
            _liveLowSweepTimeUtc = trade.Time;
            
            var italyTime = MarketTimeZones.ToItaly(trade.Time);
            _log($"[FOOTPRINT_LOW_SWEEP] Bar={_currentBar}, Italy={italyTime:yyyy-MM-dd HH:mm:ss.fff}, UTC={trade.Time:yyyy-MM-dd HH:mm:ss.fff}, Price={trade.Lastprice:F2}, Volume={trade.Volume:F0}, VAL={_balanceTracker.LastPreviewVal:F2}");
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

        // Helper methods
        private string FormatTimes(DateTime utcTime)
        {
            return MarketTimeZones.FormatUtcLondonItaly(utcTime);
        }

        private string GetBarMode(int bar)
        {
            return bar >= _currentBar - 1 ? "LIVE_OR_LAST_BAR" : "HISTORICAL_CLOSED";
        }
    }
}
