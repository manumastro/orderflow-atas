using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private static bool IsHistoricalCumulativeTradePosition(ActivePosition position)
        {
            return position.EntryModel.Contains("Historical", StringComparison.Ordinal);
        }

        private void UpdateHistoricalPositionsWithTrade(CumulativeTrade trade)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed && IsHistoricalCumulativeTradePosition(p)).ToList())
            {
                if (trade.Time <= position.EntryTimeUtc)
                    continue;

                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(trade.Time)) != DateOnly.FromDateTime(MarketTimeZones.ToItaly(position.EntryTimeUtc)))
                {
                    CloseHistoricalPositionAtEntryDayLondonClose(position);
                    continue;
                }

                if (!IsLondonTradeAllowed(trade.Time))
                    continue;

                var high = Math.Max(trade.FirstPrice, trade.Lastprice);
                var low = Math.Min(trade.FirstPrice, trade.Lastprice);
                UpdatePositionTracking(position, high, low, trade.Time);
                CheckHistoricalPositionExit(position, trade, high, low);
            }
        }

        private void CheckHistoricalPositionExit(ActivePosition position, CumulativeTrade trade, decimal high, decimal low)
        {
            var bar = FindBarByTime(trade.Time, position.EntryBar);
            if (position.Direction == "Long")
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price > position.EntryPrice && high >= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, trade.Time);

                    if (position.Target2Price > position.EntryPrice && high >= position.Target2Price)
                    {
                        ClosePosition(position, bar, trade.Time, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price > position.EntryPrice && high >= position.Target1Price)
                {
                    ClosePosition(position, bar, trade.Time, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (low <= position.StopPrice && CanStopTrigger(position, bar))
                    ClosePosition(position, bar, trade.Time, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
            }
            else
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price < position.EntryPrice && low <= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, trade.Time);

                    if (position.Target2Price < position.EntryPrice && low <= position.Target2Price)
                    {
                        ClosePosition(position, bar, trade.Time, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price < position.EntryPrice && low <= position.Target1Price)
                {
                    ClosePosition(position, bar, trade.Time, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (high >= position.StopPrice && CanStopTrigger(position, bar))
                    ClosePosition(position, bar, trade.Time, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
            }
        }

        private void UpdateOpenHistoricalPositionsWithCompletedBars()
        {
            foreach (var position in _activePositions.Where(p => !p.Closed && IsHistoricalCumulativeTradePosition(p)).ToList())
            {
                var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(position.EntryTimeUtc));
                for (var bar = position.EntryBar + 1; bar < _currentBar; bar++)
                {
                    if (position.Closed)
                        break;

                    var candle = _getCandle(bar);
                    var eventTime = GetCandleEventTime(candle);
                    if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                        break;

                    if (!IsInLondonSession(candle.Time))
                        continue;

                    UpdatePositionTracking(position, bar, candle);
                    CheckPositionExit(position, bar, candle);
                }
            }
        }

        private void CloseOpenHistoricalPositionsAtSessionEnd()
        {
            foreach (var position in _activePositions.Where(p => !p.Closed && IsHistoricalCumulativeTradePosition(p)).ToList())
                CloseHistoricalPositionAtEntryDayLondonClose(position);
        }

        private void CloseHistoricalPositionAtEntryDayLondonClose(ActivePosition position)
        {
            if (position.Closed)
                return;

            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(position.EntryTimeUtc));
            var closeBar = position.EntryBar;
            for (var bar = position.EntryBar; bar < _currentBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (IsInLondonSession(candle.Time))
                    closeBar = bar;
            }

            var closeCandle = _getCandle(closeBar);
            ClosePosition(position, closeBar, GetCandleEventTime(closeCandle), "LONDON_CLOSE", closeCandle.Close);
        }

        private void LogHistoricalPostEntryContext(ActivePosition position, CumulativeTrade entryTrade)
        {
            DailyHistoricalLog($"[DAY_STUDY_HISTORICAL_POSITION_MODE] SetupId={position.SetupId}, EntryModel={position.EntryModel}, {FormatTime(entryTrade.Time)}, Mode=AllCumulativeTradesPostEntry, EntryBar={position.EntryBar}, EntryPrice={position.EntryPrice:F2}, Stop={position.StopPrice:F2}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}", entryTrade.Time);
        }

        private void UpdateActivePositions(int bar, IndicatorCandle candle)
        {
            foreach (var position in _activePositions.Where(p => !p.Closed).ToList())
            {
                if (bar < position.EntryBar)
                    continue;

                if (_processingHistoricalPositions && IsHistoricalCumulativeTradePosition(position))
                    continue;

                UpdatePositionTracking(position, bar, candle);
                CheckPositionExit(position, bar, candle);
            }
        }

        private void UpdatePositionTracking(ActivePosition position, decimal high, decimal low, DateTime eventTimeUtc)
        {
            if (position.Direction == "Long")
            {
                if (high > position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = high;
                    position.MFE = high - position.EntryPrice;
                }

                if (low < position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = low;
                    position.MAE = position.EntryPrice - low;
                }
            }
            else
            {
                if (low < position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = low;
                    position.MFE = position.EntryPrice - low;
                }

                if (high > position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = high;
                    position.MAE = high - position.EntryPrice;
                }
            }
        }

        private void UpdatePositionTracking(ActivePosition position, int bar, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                if (candle.High > position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.High;
                    position.MFE = candle.High - position.EntryPrice;
                    LogMfe(position, bar, candle);
                }

                if (candle.Low < position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.Low;
                    position.MAE = position.EntryPrice - candle.Low;
                }
            }
            else
            {
                if (candle.Low < position.MaxFavorablePrice)
                {
                    position.MaxFavorablePrice = candle.Low;
                    position.MFE = position.EntryPrice - candle.Low;
                    LogMfe(position, bar, candle);
                }

                if (candle.High > position.MaxAdversePrice)
                {
                    position.MaxAdversePrice = candle.High;
                    position.MAE = candle.High - position.EntryPrice;
                }
            }
        }

        private void CheckPositionExit(ActivePosition position, int bar, IndicatorCandle candle)
        {
            if (position.Direction == "Long")
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price > position.EntryPrice && candle.High >= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, candle);

                    if (position.Target2Price > position.EntryPrice && candle.High >= position.Target2Price)
                    {
                        ClosePosition(position, bar, candle, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price > position.EntryPrice && candle.High >= position.Target1Price)
                {
                    ClosePosition(position, bar, candle, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (candle.Low <= position.StopPrice && CanStopTrigger(position, bar))
                {
                    ClosePosition(position, bar, candle, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
                    return;
                }
            }
            else
            {
                if (position.UseTarget2)
                {
                    if (!position.Target1Hit && position.Target1Price < position.EntryPrice && candle.Low <= position.Target1Price)
                        ProtectStopAfterTarget1(position, bar, candle);

                    if (position.Target2Price < position.EntryPrice && candle.Low <= position.Target2Price)
                    {
                        ClosePosition(position, bar, candle, "TARGET2_HIT", position.Target2Price);
                        return;
                    }
                }
                else if (position.Target1Price < position.EntryPrice && candle.Low <= position.Target1Price)
                {
                    ClosePosition(position, bar, candle, "TARGET_POC_HIT", position.Target1Price);
                    return;
                }

                if (candle.High >= position.StopPrice && CanStopTrigger(position, bar))
                {
                    ClosePosition(position, bar, candle, position.StopProtectedAfterTarget1 ? "PROTECTED_STOP_HIT" : "STOP_HIT", position.StopPrice);
                    return;
                }
            }

            var londonTime = MarketTimeZones.ToLondon(GetCandleEventTime(candle));
            if (londonTime.Hour >= 16)
                ClosePosition(position, bar, candle, "LONDON_CLOSE", candle.Close);
        }

        private static bool CanStopTrigger(ActivePosition position, int bar)
        {
            return !position.StopProtectedAfterTarget1 || bar > position.Target1HitBar;
        }

        private void ProtectStopAfterTarget1(ActivePosition position, int bar, IndicatorCandle candle)
        {
            ProtectStopAfterTarget1(position, bar, GetCandleEventTime(candle));
        }

        private void ProtectStopAfterTarget1(ActivePosition position, int bar, DateTime eventTimeUtc)
        {
            position.Target1Hit = true;
            position.Target1HitBar = bar;
            position.Target1HitTimeUtc = eventTimeUtc;

            var protectedStop = position.EntryPrice;

            var oldStop = position.StopPrice;
            position.StopPrice = protectedStop;
            position.StopProtectedAfterTarget1 = true;

            var reward = position.Direction == "Long"
                ? position.Target1Price - position.EntryPrice
                : position.EntryPrice - position.Target1Price;
            position.RealizedPocExitPct = PocPartialExitPct;
            position.RunnerExitPct = RunnerExitPct;
            position.RealizedPocPnL = reward * PocPartialExitPct;

            var target1Message = $"[MR_TARGET1_HIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, RewardPoints={reward:F2}, Target1ExitPct={PocPartialExitPct:F2}, RunnerPct={RunnerExitPct:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, OldStop={oldStop:F2}, ProtectedStop={position.StopPrice:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}";
            var partialExitMessage = $"[MR_PARTIAL_EXIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={position.Target1Price:F2}, ExitReason=POC_PARTIAL_EXIT, ExitPct={PocPartialExitPct:F2}, RunnerPct={RunnerExitPct:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, RunnerStop={position.StopPrice:F2}, Target2={position.Target2Price:F2}";
            _log(target1Message, IsHistoricalBar(bar));
            _log(partialExitMessage, IsHistoricalBar(bar));
            if (position.EntryModel.Contains("Historical", StringComparison.Ordinal))
            {
                DailyHistoricalLog(target1Message, eventTimeUtc);
                DailyHistoricalLog(partialExitMessage, eventTimeUtc);
            }
        }

        private void ClosePosition(ActivePosition position, int bar, IndicatorCandle candle, string exitReason, decimal exitPrice)
        {
            ClosePosition(position, bar, GetCandleEventTime(candle), exitReason, exitPrice);
        }

        private void ClosePosition(ActivePosition position, int bar, DateTime eventTimeUtc, string exitReason, decimal exitPrice)
        {
            if (position.Closed)
                return;

            position.Closed = true;
            position.ExitReason = exitReason;
            position.ExitPrice = exitPrice;
            position.ExitBar = bar;
            position.ExitTimeUtc = eventTimeUtc;

            var pnl = position.Direction == "Long"
                ? exitPrice - position.EntryPrice
                : position.EntryPrice - exitPrice;
            var runnerPnl = position.Target1Hit ? pnl * position.RunnerExitPct : pnl;
            var blendedPnl = position.Target1Hit ? position.RealizedPocPnL + runnerPnl : pnl;
            var risk = position.Direction == "Long"
                ? position.EntryPrice - position.InitialStopPrice
                : position.InitialStopPrice - position.EntryPrice;
            var rMultiple = risk != 0 ? blendedPnl / risk : 0;

            var exitMessage = $"[MR_EXIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={blendedPnl:F2}, RunnerPnL={runnerPnl:F2}, FullRunnerPnL={pnl:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, Target1ExitPct={position.RealizedPocExitPct:F2}, RunnerPct={position.RunnerExitPct:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, Target1Hit={position.Target1Hit}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, StopProtected={position.StopProtectedAfterTarget1}";
            _log(exitMessage, IsHistoricalBar(bar));

            if (position.EntryModel.Contains("Historical", StringComparison.Ordinal))
            {
                DailyHistoricalLog(exitMessage, eventTimeUtc);
                StudyLog($"[DAY_STUDY_ACTUAL_EXIT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, PnL={blendedPnl:F2}, RunnerPnL={runnerPnl:F2}, FullRunnerPnL={pnl:F2}, RealizedPocPnL={position.RealizedPocPnL:F2}, Target1ExitPct={position.RealizedPocExitPct:F2}, RunnerPct={position.RunnerExitPct:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, RMultiple={rMultiple:F2}R, Target1Hit={position.Target1Hit}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, StopProtected={position.StopProtectedAfterTarget1}", eventTimeUtc);
                LogPocManagementStudy(position, bar, eventTimeUtc, exitReason, exitPrice, blendedPnl, pnl);
            }


            var setup = _activeSetups.FirstOrDefault(s => s.SetupId == position.SetupId);
            if (setup == null)
                return;

            _completedTrades.Add(new TradeRecord
            {
                SetupId = position.SetupId,
                Direction = position.Direction,
                EntryModel = position.EntryModel,
                BreakoutTime = setup.BreakoutTimeUtc,
                BreakoutPrice = setup.BreakoutPrice,
                RejectionTime = setup.RejectionTimeUtc,
                POC = setup.POC,
                VAH = setup.VAH,
                VAL = setup.VAL,
                EntryTime = position.EntryTimeUtc,
                EntryPrice = position.EntryPrice,
                ExitTime = eventTimeUtc,
                ExitPrice = exitPrice,
                ExitReason = exitReason,
                PnL = blendedPnl,
                MFE = position.MFE,
                MAE = position.MAE,
                RMultiple = rMultiple
            });
        }

        private void LogPocManagementStudy(ActivePosition position, int bar, DateTime eventTimeUtc, string exitReason, decimal exitPrice, decimal currentPnl, decimal fullRunnerPnl)
        {
            var rewardToPoc = position.Direction == "Long"
                ? position.Target1Price - position.EntryPrice
                : position.EntryPrice - position.Target1Price;
            var pocAllOutPnl = position.Target1Hit ? rewardToPoc : currentPnl;
            var fullRunnerProtectedPnl = position.Target1Hit ? fullRunnerPnl : currentPnl;
            var pressure = position.Target1Hit
                ? GetPostPocPressureStats(position, position.Target1HitTimeUtc, eventTimeUtc)
                : new PocManagementPressureStats(0, 0, 0, 0, 0, 0);
            var postPocPressureConfirmed = pressure.SameVolume > pressure.OppositeVolume && pressure.MaxSameVolume >= pressure.MaxOppositeVolume && pressure.SameTrades > 0;
            var secondsAfterPoc = position.Target1Hit ? (eventTimeUtc - position.Target1HitTimeUtc).TotalSeconds : 0;

            StudyLog($"[DAY_STUDY_POC_MANAGEMENT] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, ManagementMode={position.ManagementMode}, StudyTrigger={position.StudyTrigger}, Bar={bar}, {FormatTime(eventTimeUtc)}, Entry={position.EntryPrice:F2}, Exit={exitPrice:F2}, ExitReason={exitReason}, Target1Hit={position.Target1Hit}, Target1Time={(position.Target1Hit ? FormatTime(position.Target1HitTimeUtc) : "NA")}, Target1POC={position.Target1Price:F2}, Target2={position.Target2Price:F2}, Current70_30PnL={currentPnl:F2}, PocAllOutPnL={pocAllOutPnl:F2}, FullRunnerProtectedPnL={fullRunnerProtectedPnl:F2}, DeltaPocAllOutVsCurrent={pocAllOutPnl - currentPnl:F2}, DeltaFullRunnerVsCurrent={fullRunnerProtectedPnl - currentPnl:F2}, SecondsAfterPoc={secondsAfterPoc:F1}, PostPocSameTrades={pressure.SameTrades}, PostPocSameVolume={pressure.SameVolume:F0}, PostPocMaxSameVolume={pressure.MaxSameVolume:F0}, PostPocOppositeTrades={pressure.OppositeTrades}, PostPocOppositeVolume={pressure.OppositeVolume:F0}, PostPocMaxOppositeVolume={pressure.MaxOppositeVolume:F0}, PostPocPressureConfirmed={postPocPressureConfirmed}, FabioStyleHypothesis=POC_PRIMARY_EXIT_THEN_REENTER_ON_NEW_CONFIRMATION", eventTimeUtc);
        }

        private List<CumulativeTrade> GetAggressionTradesForDayUntil(DateTime toUtc)
        {
            var day = DateOnly.FromDateTime(MarketTimeZones.ToItaly(toUtc));
            return _lastHistoricalTrades
                .Where(t => t.Time <= toUtc)
                .Where(t => DateOnly.FromDateTime(MarketTimeZones.ToItaly(t.Time)) == day)
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .ToList();
        }

        private List<CumulativeTrade> GetAggressionTradesBetween(DateTime fromUtc, DateTime toUtc)
        {
            return _lastHistoricalTrades
                .Where(t => t.Time >= fromUtc && t.Time <= toUtc)
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .ToList();
        }

        private HistoricalBarSnapshot? FindFirstTouch(List<HistoricalBarSnapshot> path, string direction, decimal price)
        {
            foreach (var snapshot in path)
            {
                if (direction == "Long")
                {
                    if (snapshot.High >= price)
                        return snapshot;
                }
                else if (snapshot.Low <= price)
                {
                    return snapshot;
                }
            }

            return null;
        }

        private static decimal GetDirectionalPnl(string direction, decimal entry, decimal exit)
        {
            return direction == "Long" ? exit - entry : entry - exit;
        }

        private PocManagementPressureStats GetPostPocPressureStats(ActivePosition position, DateTime fromUtc, DateTime toUtc)
        {
            var expectedDirection = position.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = position.Direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var trades = _lastHistoricalTrades
                .Where(t => t.Time > fromUtc && t.Time <= toUtc)
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .ToList();
            var same = trades.Where(t => t.Direction == expectedDirection).ToList();
            var opposite = trades.Where(t => t.Direction == oppositeDirection).ToList();

            return new PocManagementPressureStats(
                same.Count,
                same.Sum(t => t.Volume),
                same.Count > 0 ? same.Max(t => t.Volume) : 0,
                opposite.Count,
                opposite.Sum(t => t.Volume),
                opposite.Count > 0 ? opposite.Max(t => t.Volume) : 0);
        }

        private void LogMfe(ActivePosition position, int bar, IndicatorCandle candle)
        {
            _log($"[MR_MFE_UPDATE] SetupId={position.SetupId}, EntryModel={position.EntryModel}, Direction={position.Direction}, Bar={bar}, {FormatTime(GetCandleEventTime(candle))}, EntryPrice={position.EntryPrice:F2}, MFE={position.MFE:F2}, MAE={position.MAE:F2}, MaxFavorablePrice={position.MaxFavorablePrice:F2}, MaxAdversePrice={position.MaxAdversePrice:F2}", IsHistoricalBar(bar));
        }
    }
}
