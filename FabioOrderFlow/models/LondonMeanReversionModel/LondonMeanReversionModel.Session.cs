using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private DateTime GetLondonSessionEndUtc(DateTime eventUtc)
        {
            var london = MarketTimeZones.ToLondon(eventUtc);
            var londonEnd = new DateTime(london.Year, london.Month, london.Day, LondonSessionEndHour, LondonSessionEndMinute, 0, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(londonEnd, MarketTimeZones.London);
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

        private bool IsLondonTradeAllowed(DateTime utcTime)
        {
            var london = MarketTimeZones.ToLondon(utcTime);
            var minutes = london.Hour * 60 + london.Minute;
            var start = 8 * 60;
            var cutoff = LondonSessionEndHour * 60 + LondonSessionEndMinute;
            return minutes >= start && minutes < cutoff;
        }

        private bool IsInLondonSession(DateTime utcTime)
        {
            var london = MarketTimeZones.ToLondon(utcTime);
            var minutes = london.Hour * 60 + london.Minute;
            return minutes >= 8 * 60 && minutes < 16 * 60;
        }

        private static DateTime GetCandleEventTime(IndicatorCandle candle)
        {
            return candle.LastTime > candle.Time ? candle.LastTime : candle.Time;
        }

        private string GetLiveTradeKey(CumulativeTrade trade)
        {
            return $"{trade.Time.Ticks}:{trade.Direction}:{trade.FirstPrice:F2}";
        }

        private string FormatTime(DateTime utc)
        {
            return MarketTimeZones.FormatUtcLondonItaly(utc);
        }

        private bool IsHistoricalBar(int bar)
        {
            return bar < _currentBar - 1;
        }

        public IReadOnlyList<TradeRecord> CompletedTrades => _completedTrades;
        public IReadOnlyList<ActivePosition> ActivePositions => _activePositions;
        public IReadOnlyList<BalanceSetup> ActiveSetups => _activeSetups;
    }
}
