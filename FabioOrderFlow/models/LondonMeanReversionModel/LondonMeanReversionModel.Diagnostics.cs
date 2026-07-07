using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private void CaptureHistoricalBarSnapshot(int bar, IndicatorCandle candle)
        {
            if (!IsInLondonSession(candle.Time))
                return;

            if (_historicalBarSnapshots.ContainsKey(bar))
                return;

            _historicalBarSnapshots[bar] = CreateHistoricalBarSnapshot(bar, candle);
        }

        private HistoricalBarSnapshot CreateHistoricalBarSnapshot(int bar, IndicatorCandle candle)
        {
            var (bid, ask, delta, topLevels) = GetCandleVolumeDiagnostics(candle);
            var eventTime = GetCandleEventTime(candle);
            var poc = _balanceTracker.LastPreviewPoc;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            return new HistoricalBarSnapshot
            {
                Bar = bar,
                EventTimeUtc = eventTime,
                Open = candle.Open,
                High = candle.High,
                Low = candle.Low,
                Close = candle.Close,
                Volume = candle.Volume,
                Bid = bid,
                Ask = ask,
                Delta = delta,
                PreviewPOC = poc,
                PreviewVAH = vah,
                PreviewVAL = val,
                PreviousSessionHigh = _balanceTracker.CurrentZone?.High ?? 0,
                PreviousSessionLow = _balanceTracker.CurrentZone?.Low ?? 0,
                CloseLocation = GetPriceLocation(candle.Close, poc, vah, val),
                TopLevels = topLevels
            };
        }

        private string GetBarSetupDiagnostics(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return "NO_PROFILE";

            var highBreak = snapshot.High > snapshot.PreviewVAH;
            var lowBreak = snapshot.Low < snapshot.PreviewVAL;
            var shortCloseBackInside = snapshot.Close < snapshot.PreviewVAH;
            var longCloseBackInside = snapshot.Close > snapshot.PreviewVAL;
            var shortRejectionTicks = (snapshot.High - snapshot.Close) / _tickSize;
            var longRejectionTicks = (snapshot.Close - snapshot.Low) / _tickSize;
            var shortReady = highBreak && shortCloseBackInside && shortRejectionTicks >= RejectionThresholdTicks;
            var longReady = lowBreak && longCloseBackInside && longRejectionTicks >= RejectionThresholdTicks;

            return $"HighBreakVAH={highBreak};LowBreakVAL={lowBreak};ShortCloseBackInside={shortCloseBackInside};LongCloseBackInside={longCloseBackInside};ShortRejectionTicks={shortRejectionTicks:F1};LongRejectionTicks={longRejectionTicks:F1};ShortSetupReady={shortReady};LongSetupReady={longReady}";
        }

        private string GetActiveSetupDiagnostics(DateTime eventUtc)
        {
            var candidates = _activeSetups
                .Where(s => !s.Expired)
                .Where(s => eventUtc >= s.RejectionTimeUtc && eventUtc <= s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .OrderBy(s => s.RejectionTimeUtc)
                .Select(s => $"{s.SetupId[..Math.Min(8, s.SetupId.Length)]}:{s.SetupSource}:{s.Direction}:Age={(eventUtc - s.RejectionTimeUtc).TotalSeconds:F0}s:Confirmed={s.AggressionConfirmed}:Trigger={GetStudyTriggerLabelAtTime(s, eventUtc)}")
                .ToList();

            return candidates.Count == 0 ? "NONE" : string.Join("|", candidates);
        }

        private bool ShouldDebugHistoricalDay(DateTime eventUtc)
        {
            if (_historicalStudyDebugDays.Count == 0)
                return true;

            return _historicalStudyDebugDays.Contains(DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventUtc)));
        }

        private string FormatHistoricalStudyDebugDays()
        {
            if (!_historicalStudyDebugEnabled)
                return "OFF";

            return _historicalStudyDebugDays.Count == 0
                ? "ALL"
                : string.Join("|", _historicalStudyDebugDays.OrderBy(d => d).Select(d => d.ToString("yyyy-MM-dd")));
        }

        private string GetTradeCandidateDiagnostics(CumulativeTrade trade)
        {
            var candidates = _activeSetups
                .Where(s => !s.Expired && !s.AggressionConfirmed)
                .Where(s => trade.Time > s.RejectionTimeUtc && trade.Time <= s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .OrderBy(s => Math.Abs((trade.Time - s.RejectionTimeUtc).TotalSeconds))
                .Take(5)
                .Select(s =>
                {
                    var sameDirection = s.Direction == "Long" ? trade.Direction == TradeDirection.Buy : trade.Direction == TradeDirection.Sell;
                    var inEntryZone = IsInEntryZone(s, trade);
                    var operationalZone = IsOperationalEntryZone(s, trade);
                    var fresh = (trade.Time - s.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds;
                    var rr = GetRewardRiskToTarget2(s, trade.Lastprice);
                    var reason = !sameDirection
                        ? "WRONG_DIRECTION"
                        : !operationalZone
                            ? "OUTSIDE_ENTRY_ZONE"
                            : !fresh
                                ? "STALE"
                                : rr < MinRewardRiskToTarget2
                                    ? "RR_TOO_LOW"
                                    : "VALID";
                    return $"{s.SetupId[..Math.Min(8, s.SetupId.Length)]}:{s.SetupSource}:{s.Direction}:Reason={reason}:Age={(trade.Time - s.RejectionTimeUtc).TotalSeconds:F0}s:InZone={inEntryZone}:OperationalZone={operationalZone}:RR={rr:F2}:TriggerAtTrade={GetStudyTriggerLabelAtTime(s, trade.Time)}";
                })
                .ToList();

            return candidates.Count == 0 ? "NO_ACTIVE_SETUP" : string.Join("|", candidates);
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

        private string GetTradeLocation(decimal price)
        {
            return GetPriceLocation(price, _balanceTracker.LastPreviewPoc, _balanceTracker.LastPreviewVah, _balanceTracker.LastPreviewVal);
        }

        private static string GetPriceLocation(decimal price, decimal poc, decimal vah, decimal val)
        {
            if (poc == 0 || vah == 0 || val == 0)
                return "NO_PROFILE";

            if (price > vah)
                return "ABOVE_VAH";
            if (price >= poc)
                return "POC_TO_VAH";
            if (price >= val)
                return "VAL_TO_POC";
            return "BELOW_VAL";
        }

        private (string SetupId, string Direction, double SecondsAfter, bool SameDirection, bool InEntryZone) FindNearestStudySetup(CumulativeTrade trade)
        {
            var setup = _activeSetups
                .Where(s => trade.Time >= s.RejectionTimeUtc && trade.Time <= s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .OrderBy(s => Math.Abs((trade.Time - s.RejectionTimeUtc).TotalSeconds))
                .FirstOrDefault();

            if (setup == null)
                return ("NA", "NA", 0, false, false);

            var sameDirection = setup.Direction == "Long" ? trade.Direction == TradeDirection.Buy : trade.Direction == TradeDirection.Sell;
            return (setup.SetupId, setup.Direction, (trade.Time - setup.RejectionTimeUtc).TotalSeconds, sameDirection, IsInEntryZone(setup, trade));
        }
    }
}
