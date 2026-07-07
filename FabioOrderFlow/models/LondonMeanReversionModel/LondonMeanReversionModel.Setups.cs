using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private bool TryCreateHighRejectionSetup(int bar, IndicatorCandle candle, out BalanceSetup? setup)
        {
            setup = null;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            var poc = _balanceTracker.LastPreviewPoc;

            if (vah == 0 || val == 0 || poc == 0)
                return false;

            if (candle.High <= vah || candle.Close >= vah)
                return false;

            var rejectionDistance = candle.High - candle.Close;
            if (rejectionDistance < RejectionThresholdTicks * _tickSize)
                return false;

            setup = new BalanceSetup
            {
                Direction = "Short",
                POC = poc,
                VAH = vah,
                VAL = val,
                BreakoutBar = bar,
                BreakoutTimeUtc = candle.Time,
                BreakoutPrice = candle.High,
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                RejectionDelta = rejectionDistance,
                StopPrice = candle.High + StopOffsetTicks * _tickSize,
                TargetPrice = poc
            };

            return true;
        }

        private bool TryCreateLowRejectionSetup(int bar, IndicatorCandle candle, out BalanceSetup? setup)
        {
            setup = null;
            var vah = _balanceTracker.LastPreviewVah;
            var val = _balanceTracker.LastPreviewVal;
            var poc = _balanceTracker.LastPreviewPoc;

            if (vah == 0 || val == 0 || poc == 0)
                return false;

            if (candle.Low >= val || candle.Close <= val)
                return false;

            var rejectionDistance = candle.Close - candle.Low;
            if (rejectionDistance < RejectionThresholdTicks * _tickSize)
                return false;

            setup = new BalanceSetup
            {
                Direction = "Long",
                POC = poc,
                VAH = vah,
                VAL = val,
                BreakoutBar = bar,
                BreakoutTimeUtc = candle.Time,
                BreakoutPrice = candle.Low,
                RejectionBar = bar,
                RejectionTimeUtc = GetCandleEventTime(candle),
                RejectionClose = candle.Close,
                RejectionHigh = candle.High,
                RejectionLow = candle.Low,
                RejectionDelta = rejectionDistance,
                StopPrice = candle.Low - StopOffsetTicks * _tickSize,
                TargetPrice = poc
            };

            return true;
        }

        private void AddSetup(BalanceSetup setup, string tag, bool prepend = false)
        {
            var key = setup.SetupSource == "HistoricalIntrabar"
                ? $"{setup.SetupSource}:{setup.Direction}:{setup.RejectionBar}:{setup.BreakoutPrice:F2}:{setup.POC:F2}:{setup.RejectionTimeUtc.Ticks}"
                : $"{setup.SetupSource}:{setup.Direction}:{setup.RejectionBar}:{setup.BreakoutPrice:F2}:{setup.POC:F2}";
            if (!_setupKeys.Add(key))
                return;

            if (prepend)
                _activeSetups.Insert(0, setup);
            else
                _activeSetups.Add(setup);

            _log($"[{tag}] SetupId={setup.SetupId}, Source={setup.SetupSource}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", IsHistoricalBar(setup.RejectionBar));
            if (setup.SetupSource == "HistoricalIntrabar" || IsHistoricalBar(setup.RejectionBar))
                StudyLog($"[DAY_STUDY_SETUP] SetupId={setup.SetupId}, Source={setup.SetupSource}, Tag={tag}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, RejectionHigh={setup.RejectionHigh:F2}, RejectionLow={setup.RejectionLow:F2}, RejectionDelta={setup.RejectionDelta:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", setup.RejectionTimeUtc);
        }

        private void LogExistingHistoricalSetups()
        {
            foreach (var setup in _activeSetups
                .Where(s => s.SetupSource != "HistoricalIntrabar" && IsHistoricalBar(s.RejectionBar))
                .Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc))
                .OrderBy(s => s.RejectionTimeUtc))
            {
                StudyLog($"[DAY_STUDY_SETUP] SetupId={setup.SetupId}, Source={setup.SetupSource}, Tag=MR_SETUP_RESTORED_AFTER_DAILY_RESET, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, BreakoutPrice={setup.BreakoutPrice:F2}, RejectionClose={setup.RejectionClose:F2}, RejectionHigh={setup.RejectionHigh:F2}, RejectionLow={setup.RejectionLow:F2}, RejectionDelta={setup.RejectionDelta:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}, AggressionConfirmed={setup.AggressionConfirmed}, Expired={setup.Expired}", setup.RejectionTimeUtc);
            }
        }

        private void AddHistoricalIntrabarSetups(List<CumulativeTrade> allTrades)
        {
            var barCloseSetups = _activeSetups
                .Where(s => s.SetupSource == "BarClose")
                .OrderBy(s => s.RejectionTimeUtc)
                .ToList();

            foreach (var setup in barCloseSetups)
            {
                var candle = _getCandle(setup.RejectionBar);
                var barEnd = GetCandleEventTime(candle);
                var trades = allTrades
                    .Where(t => t.Time >= candle.Time && t.Time <= barEnd)
                    .OrderBy(t => t.Time)
                    .ToList();

                if (trades.Count == 0)
                    continue;

                var syntheticHigh = candle.Open;
                var syntheticLow = candle.Open;

                foreach (var trade in trades)
                {
                    syntheticHigh = Math.Max(syntheticHigh, Math.Max(trade.FirstPrice, trade.Lastprice));
                    syntheticLow = Math.Min(syntheticLow, Math.Min(trade.FirstPrice, trade.Lastprice));
                    var syntheticClose = trade.Lastprice;

                    if (!TryCreateHistoricalIntrabarSetup(setup, trade.Time, syntheticHigh, syntheticLow, syntheticClose, out var intrabarSetup) || intrabarSetup == null)
                        continue;

                    AddSetup(intrabarSetup, "MR_HISTORICAL_INTRABAR_SETUP", prepend: true);
                    break;
                }
            }
        }

        private bool TryCreateHistoricalIntrabarSetup(BalanceSetup source, DateTime eventTimeUtc, decimal syntheticHigh, decimal syntheticLow, decimal syntheticClose, out BalanceSetup? setup)
        {
            setup = null;

            if (eventTimeUtc >= source.RejectionTimeUtc)
                return false;

            if (source.Direction == "Short")
            {
                if (syntheticHigh <= source.VAH || syntheticClose >= source.VAH)
                    return false;

                var rejectionDistance = syntheticHigh - syntheticClose;
                if (rejectionDistance < RejectionThresholdTicks * _tickSize)
                    return false;

                setup = new BalanceSetup
                {
                    Direction = source.Direction,
                    POC = source.POC,
                    VAH = source.VAH,
                    VAL = source.VAL,
                    BreakoutBar = source.BreakoutBar,
                    BreakoutTimeUtc = eventTimeUtc,
                    BreakoutPrice = syntheticHigh,
                    RejectionBar = source.RejectionBar,
                    RejectionTimeUtc = eventTimeUtc,
                    RejectionClose = syntheticClose,
                    RejectionHigh = syntheticHigh,
                    RejectionLow = syntheticLow,
                    RejectionDelta = rejectionDistance,
                    StopPrice = syntheticHigh + StopOffsetTicks * _tickSize,
                    TargetPrice = source.POC,
                    StudyPocTriggerConfirmed = source.StudyPocTriggerConfirmed,
                    StudyPocTriggerBar = source.StudyPocTriggerBar,
                    StudyPocTriggerTimeUtc = source.StudyPocTriggerTimeUtc,
                    StudyTriggerBar = source.StudyTriggerBar,
                    StudyTriggerTimeUtc = source.StudyTriggerTimeUtc,
                    SetupSource = "HistoricalIntrabar"
                };
                return true;
            }

            if (syntheticLow >= source.VAL || syntheticClose <= source.VAL)
                return false;

            var lowRejectionDistance = syntheticClose - syntheticLow;
            if (lowRejectionDistance < RejectionThresholdTicks * _tickSize)
                return false;

            setup = new BalanceSetup
            {
                Direction = source.Direction,
                POC = source.POC,
                VAH = source.VAH,
                VAL = source.VAL,
                BreakoutBar = source.BreakoutBar,
                BreakoutTimeUtc = eventTimeUtc,
                BreakoutPrice = syntheticLow,
                RejectionBar = source.RejectionBar,
                RejectionTimeUtc = eventTimeUtc,
                RejectionClose = syntheticClose,
                RejectionHigh = syntheticHigh,
                RejectionLow = syntheticLow,
                RejectionDelta = lowRejectionDistance,
                StopPrice = syntheticLow - StopOffsetTicks * _tickSize,
                TargetPrice = source.POC,
                StudyPocTriggerConfirmed = source.StudyPocTriggerConfirmed,
                StudyPocTriggerBar = source.StudyPocTriggerBar,
                StudyPocTriggerTimeUtc = source.StudyPocTriggerTimeUtc,
                StudyTriggerBar = source.StudyTriggerBar,
                StudyTriggerTimeUtc = source.StudyTriggerTimeUtc,
                SetupSource = "HistoricalIntrabar"
            };
            return true;
        }
    }
}
