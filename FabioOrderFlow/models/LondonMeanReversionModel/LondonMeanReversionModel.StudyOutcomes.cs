using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private ProtectedStudyOutcome EvaluateProtectedTarget2OutcomeFromTrades(string direction, decimal entry, decimal stop, decimal targetPoc, decimal target2, DateTime entryTimeUtc)
        {
            var risk = Math.Abs(entry - stop);
            if (risk <= 0)
                return new ProtectedStudyOutcome("INVALID_RISK", 0, 0, false);

            var target1Hit = false;
            var protectedStop = stop;
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var closePrice = entry;

            foreach (var trade in _lastHistoricalTrades.Where(t => t.Time > entryTimeUtc).OrderBy(t => t.Time))
            {
                var eventDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(trade.Time));
                if (eventDay != entryDay)
                    break;

                if (!IsLondonTradeAllowed(trade.Time))
                    break;

                var high = Math.Max(trade.FirstPrice, trade.Lastprice);
                var low = Math.Min(trade.FirstPrice, trade.Lastprice);
                closePrice = trade.Lastprice;

                if (direction == "Long")
                {
                    if (!target1Hit && targetPoc > entry && high >= targetPoc)
                    {
                        target1Hit = true;
                        protectedStop = Math.Max(entry, targetPoc - 2m * _tickSize);
                    }

                    if (target2 > entry && high >= target2)
                    {
                        var pnl = target2 - entry;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, target1Hit);
                    }

                    if (low <= protectedStop)
                    {
                        var pnl = protectedStop - entry;
                        return new ProtectedStudyOutcome(target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT", pnl, pnl / risk, target1Hit);
                    }
                }
                else
                {
                    if (!target1Hit && targetPoc < entry && low <= targetPoc)
                    {
                        target1Hit = true;
                        protectedStop = Math.Min(entry, targetPoc + 2m * _tickSize);
                    }

                    if (target2 < entry && low <= target2)
                    {
                        var pnl = entry - target2;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, target1Hit);
                    }

                    if (high >= protectedStop)
                    {
                        var pnl = entry - protectedStop;
                        return new ProtectedStudyOutcome(target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT", pnl, pnl / risk, target1Hit);
                    }
                }
            }

            var londonClosePnl = direction == "Long" ? closePrice - entry : entry - closePrice;
            return new ProtectedStudyOutcome("LONDON_CLOSE", londonClosePnl, londonClosePnl / risk, target1Hit);
        }

        private ProtectedStudyOutcome EvaluateProtectedTarget2Outcome(string direction, decimal entry, decimal stop, decimal targetPoc, decimal target2, DateTime entryTimeUtc, int fallbackBar)
        {
            var entryBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var activeStop = stop;
            var target1Hit = false;
            var target1HitBar = -1;
            var exitReason = "OPEN";
            var exitPrice = entry;

            for (var bar = entryBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                if (direction == "Long")
                {
                    if (!target1Hit && targetPoc > entry && candle.High >= targetPoc)
                    {
                        target1Hit = true;
                        target1HitBar = bar;
                        activeStop = Math.Max(entry, targetPoc - 2m * _tickSize);
                    }

                    if (target2 > entry && candle.High >= target2)
                    {
                        exitReason = "TARGET2_HIT";
                        exitPrice = target2;
                        break;
                    }

                    if (candle.Low <= activeStop && (!target1Hit || bar > target1HitBar))
                    {
                        exitReason = target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT";
                        exitPrice = activeStop;
                        break;
                    }
                }
                else
                {
                    if (!target1Hit && targetPoc < entry && candle.Low <= targetPoc)
                    {
                        target1Hit = true;
                        target1HitBar = bar;
                        activeStop = Math.Min(entry, targetPoc + 2m * _tickSize);
                    }

                    if (target2 < entry && candle.Low <= target2)
                    {
                        exitReason = "TARGET2_HIT";
                        exitPrice = target2;
                        break;
                    }

                    if (candle.High >= activeStop && (!target1Hit || bar > target1HitBar))
                    {
                        exitReason = target1Hit ? "PROTECTED_STOP_HIT" : "STOP_HIT";
                        exitPrice = activeStop;
                        break;
                    }
                }
            }

            if (exitReason == "OPEN")
                exitReason = "SESSION_END";

            var pnl = direction == "Long" ? exitPrice - entry : entry - exitPrice;
            var risk = Math.Abs(entry - stop);
            var rMultiple = risk > 0 ? pnl / risk : 0;
            return new ProtectedStudyOutcome(exitReason, pnl, rMultiple, target1Hit);
        }

        private DateTime? FindTargetPocHitTime(string direction, decimal entry, decimal stop, decimal targetPoc, DateTime entryTimeUtc, int fallbackBar)
        {
            var entryBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));

            for (var bar = entryBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                if (direction == "Long")
                {
                    if (candle.Low <= stop)
                        return null;
                    if (targetPoc > entry && candle.High >= targetPoc)
                        return eventTime;
                }
                else
                {
                    if (candle.High >= stop)
                        return null;
                    if (targetPoc < entry && candle.Low <= targetPoc)
                        return eventTime;
                }
            }

            return null;
        }
        private StudyOutcome EvaluateCandidateOutcome(string direction, decimal entry, decimal stop, decimal targetPoc, decimal target2, DateTime entryTimeUtc, int fallbackBar)
        {
            var entryBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var mfe = 0m;
            var mae = 0m;
            var outcomePoc = "OPEN";
            var outcomeT2 = "OPEN";
            var pnlPoc = 0m;
            var pnlT2 = 0m;

            for (var bar = entryBar; bar <= Math.Max(0, _currentBar - 1); bar++)
            {
                var candle = _getCandle(bar);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(candle.Time)) != entryDay)
                    break;

                if (!IsInLondonSession(candle.Time))
                    break;

                if (direction == "Long")
                {
                    mfe = Math.Max(mfe, candle.High - entry);
                    mae = Math.Max(mae, entry - candle.Low);
                    if (outcomePoc == "OPEN" && candle.Low <= stop)
                    {
                        outcomePoc = "STOP_HIT";
                        pnlPoc = stop - entry;
                    }
                    if (outcomePoc == "OPEN" && candle.High >= targetPoc)
                    {
                        outcomePoc = "POC_HIT";
                        pnlPoc = targetPoc - entry;
                    }
                    if (outcomeT2 == "OPEN" && candle.Low <= stop)
                    {
                        outcomeT2 = "STOP_HIT";
                        pnlT2 = stop - entry;
                    }
                    if (outcomeT2 == "OPEN" && candle.High >= target2)
                    {
                        outcomeT2 = "TARGET2_HIT";
                        pnlT2 = target2 - entry;
                    }
                }
                else
                {
                    mfe = Math.Max(mfe, entry - candle.Low);
                    mae = Math.Max(mae, candle.High - entry);
                    if (outcomePoc == "OPEN" && candle.High >= stop)
                    {
                        outcomePoc = "STOP_HIT";
                        pnlPoc = entry - stop;
                    }
                    if (outcomePoc == "OPEN" && candle.Low <= targetPoc)
                    {
                        outcomePoc = "POC_HIT";
                        pnlPoc = entry - targetPoc;
                    }
                    if (outcomeT2 == "OPEN" && candle.High >= stop)
                    {
                        outcomeT2 = "STOP_HIT";
                        pnlT2 = entry - stop;
                    }
                    if (outcomeT2 == "OPEN" && candle.Low <= target2)
                    {
                        outcomeT2 = "TARGET2_HIT";
                        pnlT2 = entry - target2;
                    }
                }
            }

            return new StudyOutcome(outcomePoc, pnlPoc, outcomeT2, pnlT2, mfe, mae);
        }

        private ProtectedStudyOutcome EvaluateHoldToTarget2OutcomeFromBars(string direction, decimal entry, decimal stop, decimal target2, DateTime entryTimeUtc, int fallbackBar)
        {
            var risk = Math.Abs(entry - stop);
            if (risk <= 0)
                return new ProtectedStudyOutcome("INVALID_RISK", 0, 0, false);

            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc));
            var startBar = FindBarByTime(entryTimeUtc, fallbackBar);
            var closePrice = entry;
            for (var bar = startBar + 1; bar < _currentBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(candle.Time))
                    continue;

                closePrice = candle.Close;
                if (direction == "Long")
                {
                    if (candle.High >= target2)
                    {
                        var pnl = target2 - entry;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, true);
                    }

                    if (candle.Low <= stop)
                    {
                        var pnl = stop - entry;
                        return new ProtectedStudyOutcome("STOP_HIT", pnl, pnl / risk, false);
                    }
                }
                else
                {
                    if (candle.Low <= target2)
                    {
                        var pnl = entry - target2;
                        return new ProtectedStudyOutcome("TARGET2_HIT", pnl, pnl / risk, true);
                    }

                    if (candle.High >= stop)
                    {
                        var pnl = entry - stop;
                        return new ProtectedStudyOutcome("STOP_HIT", pnl, pnl / risk, false);
                    }
                }
            }

            var closePnl = direction == "Long" ? closePrice - entry : entry - closePrice;
            return new ProtectedStudyOutcome("LONDON_CLOSE", closePnl, closePnl / risk, false);
        }

        private HistoricalContext GetHistoricalContextAtTime(DateTime eventUtc, int fallbackBar)
        {
            var bar = FindBarByTime(eventUtc, fallbackBar);
            if (_historicalBarSnapshots.TryGetValue(bar, out var snapshot))
                return new HistoricalContext(snapshot.PreviewPOC, snapshot.PreviewVAH, snapshot.PreviewVAL);

            return new HistoricalContext(_balanceTracker.LastPreviewPoc, _balanceTracker.LastPreviewVah, _balanceTracker.LastPreviewVal);
        }
    }
}
