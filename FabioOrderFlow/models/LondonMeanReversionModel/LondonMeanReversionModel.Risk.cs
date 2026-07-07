using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private decimal GetPocTriggerBarStop(BalanceSetup setup)
        {
            var bar = FindBarByTime(setup.StudyPocTriggerTimeUtc, setup.RejectionBar);
            var candle = _getCandle(bar);
            return setup.Direction == "Long"
                ? candle.Low - 2m * _tickSize
                : candle.High + 2m * _tickSize;
        }

        private decimal GetRecentSwingStop(BalanceSetup setup, DateTime entryTimeUtc)
        {
            var startBar = Math.Min(FindBarByTime(setup.RejectionTimeUtc, setup.RejectionBar) + 1, Math.Max(0, _currentBar - 1));
            var endBar = FindBarByTime(entryTimeUtc, startBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            decimal? swing = null;

            for (var bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                swing = setup.Direction == "Long"
                    ? !swing.HasValue ? candle.Low : Math.Min(swing.Value, candle.Low)
                    : !swing.HasValue ? candle.High : Math.Max(swing.Value, candle.High);
            }

            if (!swing.HasValue)
                return setup.StopPrice;

            return setup.Direction == "Long"
                ? swing.Value - 2m * _tickSize
                : swing.Value + 2m * _tickSize;
        }

        private static decimal GetCappedStop(string direction, decimal entryPrice, decimal originalStop, decimal maxRisk)
        {
            var originalRisk = Math.Abs(entryPrice - originalStop);
            if (originalRisk <= maxRisk)
                return originalStop;

            return direction == "Long"
                ? entryPrice - maxRisk
                : entryPrice + maxRisk;
        }

        private static bool IsStopBehindEntry(string direction, decimal entryPrice, decimal stop)
        {
            return direction == "Long"
                ? stop < entryPrice
                : stop > entryPrice;
        }

        private static string GetRejectionAgeBucket(double seconds)
        {
            if (seconds < 120)
                return "0_2M";
            if (seconds < 300)
                return "2_5M";
            if (seconds < 600)
                return "5_10M";
            if (seconds < 1200)
                return "10_20M";
            if (seconds < 1800)
                return "20_30M";
            return "30_60M";
        }

        private StudyCandidate ToOperationalStudyCandidate(BalanceSetup setup, StudyCandidate candidate)
        {
            var stop = GetOperationalStopPrice(setup, candidate.EntryPrice);
            var risk = IsStopBehindEntry(setup.Direction, candidate.EntryPrice, stop)
                ? Math.Abs(candidate.EntryPrice - stop)
                : 0m;

            return candidate with { Stop = stop, Risk = risk };
        }
    }
}
