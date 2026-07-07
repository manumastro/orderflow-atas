using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private void ProcessAggressionTrade(CumulativeTrade trade, string entryModel, bool isHistorical)
        {
            ExpireStaleOperationalState(trade.Time);
            TrackProcessedAggressionTrade(trade);

            if (TryProcessDelayedReclaimEntry(trade, entryModel, isHistorical))
                return;

            foreach (var setup in _activeSetups.Where(s => s.AggressionConfirmed && !s.Expired).ToList())
            {
                if (!IsScaleInEntry(setup, trade))
                    continue;

                var scaleInIndex = _activePositions.Count(p => p.SetupId == setup.SetupId && p.IsScaleIn) + 1;
                setup.ScaleInConfirmed = scaleInIndex >= MaxScaleInsPerSetup;
                CreatePosition(setup, trade, entryModel, isHistorical, isScaleIn: true, scaleInIndex: scaleInIndex);
                break;
            }

            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired).ToList())
            {
                if (!IsTradeInSetupWindow(setup, trade))
                    continue;

                if (!IsOperationalSetupEnabled(setup, trade))
                    continue;

                if (!IsAggressionEntry(setup, trade))
                    continue;

                setup.AggressionConfirmed = true;
                var resolvedEntryModel = setup.SetupSource == "HistoricalIntrabar" && entryModel == "FootprintCumulativeTradeHistorical"
                    ? "FootprintCumulativeTradeHistoricalIntrabar"
                    : entryModel;
                CreatePosition(setup, trade, resolvedEntryModel, isHistorical);
                ExpireEquivalentBaseSetups(setup);
                break;
            }
        }

        private void TrackProcessedAggressionTrade(CumulativeTrade trade)
        {
            if (trade.Volume < MinAggressionVolume || !IsLondonTradeAllowed(trade.Time))
                return;

            if (_processedAggressionTrades.Any(t => t.Time == trade.Time && t.Direction == trade.Direction && t.Lastprice == trade.Lastprice && t.Volume == trade.Volume))
                return;

            _processedAggressionTrades.Add(trade);
        }

        private List<CumulativeTrade> GetProcessedAggressionTradesForDayUntil(DateTime toUtc)
        {
            var day = DateOnly.FromDateTime(MarketTimeZones.ToItaly(toUtc));
            return _processedAggressionTrades
                .Where(t => t.Time <= toUtc)
                .Where(t => DateOnly.FromDateTime(MarketTimeZones.ToItaly(t.Time)) == day)
                .ToList();
        }

        private List<CumulativeTrade> GetProcessedAggressionTradesBetween(DateTime fromUtc, DateTime toUtc)
        {
            return _processedAggressionTrades
                .Where(t => t.Time >= fromUtc && t.Time <= toUtc)
                .ToList();
        }

        private void ExpireEquivalentBaseSetups(BalanceSetup confirmedSetup)
        {
            foreach (var setup in _activeSetups.Where(s => s.SetupId != confirmedSetup.SetupId && !s.AggressionConfirmed && !s.Expired))
            {
                if (setup.Direction != confirmedSetup.Direction)
                    continue;

                if (setup.RejectionBar != confirmedSetup.RejectionBar)
                    continue;

                if (setup.POC != confirmedSetup.POC || setup.VAH != confirmedSetup.VAH || setup.VAL != confirmedSetup.VAL)
                    continue;

                setup.Expired = true;
                StudyLog($"[DAY_STUDY_EQUIVALENT_SETUP_EXPIRED] SetupId={setup.SetupId}, KeptSetupId={confirmedSetup.SetupId}, Source={setup.SetupSource}, KeptSource={confirmedSetup.SetupSource}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}", setup.RejectionTimeUtc);
            }
        }

        private bool IsTradeInSetupWindow(BalanceSetup setup, CumulativeTrade trade)
        {
            if (trade.Time <= setup.RejectionTimeUtc)
                return false;

            if (trade.Time > setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                return false;

            return IsLondonTradeAllowed(trade.Time);
        }

        private bool IsOperationalSetupEnabled(BalanceSetup setup, CumulativeTrade trade)
        {
            if (!OperationalCoreMeanReversionOnly)
                return true;

            var trigger = GetStudyTriggerLabelAtTime(setup, trade.Time);
            return trigger == "POC_RECLAIM_AFTER_LOW_REJECTION"
                || trigger == "POC_LOSS_AFTER_HIGH_REJECTION";
        }

        private bool IsAggressionEntry(BalanceSetup setup, CumulativeTrade trade, bool enforceFreshness = true)
        {
            if (trade.Volume < MinAggressionVolume)
                return false;

            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction != expectedDirection)
                return false;

            if (!IsOperationalEntryZone(setup, trade))
                return false;

            if (enforceFreshness && (trade.Time - setup.RejectionTimeUtc).TotalSeconds > OperationalEntryTimeoutSeconds)
                return false;

            return GetRewardRiskToTarget2(setup, trade.Lastprice) >= MinRewardRiskToTarget2;
        }

        private bool IsScaleInEntry(BalanceSetup setup, CumulativeTrade trade)
        {
            if (!IsTradeInSetupWindow(setup, trade))
                return false;

            if (!IsAggressionEntry(setup, trade, enforceFreshness: true))
                return false;

            var rewardToTarget1 = setup.Direction == "Long"
                ? setup.TargetPrice - trade.Lastprice
                : trade.Lastprice - setup.TargetPrice;
            if (rewardToTarget1 < ScaleInMinRewardToTarget1Points)
                return false;

            var existingScaleIns = _activePositions.Count(p => p.SetupId == setup.SetupId && p.IsScaleIn);
            if (existingScaleIns >= MaxScaleInsPerSetup)
                return false;

            var basePosition = _activePositions
                .Where(p => p.SetupId == setup.SetupId && !p.IsScaleIn)
                .OrderBy(p => p.EntryTimeUtc)
                .FirstOrDefault();
            if (basePosition == null || !basePosition.Target1Hit)
                return false;

            var riskFreeTime = GetCandleEventTime(_getCandle(basePosition.Target1HitBar));
            if (trade.Time <= riskFreeTime)
                return false;

            if (basePosition.Closed && trade.Time > basePosition.ExitTimeUtc)
                return false;

            return HasExpandedAfterRiskFree(setup, trade, riskFreeTime, ScaleInMinExpansionAfterRiskFreePct);
        }

        private static bool IsInEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            return setup.Direction == "Long"
                ? trade.Lastprice >= setup.VAL && trade.Lastprice < setup.POC
                : trade.Lastprice <= setup.VAH && trade.Lastprice > setup.POC;
        }

        private static bool IsOperationalEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            return IsInEntryZone(setup, trade);
        }

        private decimal GetRewardRiskToTarget2(BalanceSetup setup, decimal entryPrice)
        {
            var risk = GetOperationalRisk(setup, entryPrice);
            if (risk <= 0)
                return 0;

            var reward = Math.Abs(GetStudyTarget2(setup) - entryPrice);
            return reward / risk;
        }

        private decimal GetOperationalStopPrice(BalanceSetup setup, decimal entryPrice)
        {
            var valueWidth = Math.Abs(setup.VAH - setup.VAL);
            if (valueWidth <= 0)
                return setup.StopPrice;

            return GetCappedStop(setup.Direction, entryPrice, setup.StopPrice, valueWidth * DynamicStopMaxValueAreaRiskPct);
        }

        private decimal GetOperationalRisk(BalanceSetup setup, decimal entryPrice)
        {
            var stop = GetOperationalStopPrice(setup, entryPrice);
            if (!IsStopBehindEntry(setup.Direction, entryPrice, stop))
                return 0;

            return Math.Abs(entryPrice - stop);
        }

        private string GetOperationalStopPlan(BalanceSetup setup, decimal entryPrice)
        {
            var operationalStop = GetOperationalStopPrice(setup, entryPrice);
            return operationalStop == setup.StopPrice ? "ORIGINAL_REJECTION" : "CAP_VALUE_WIDTH_50";
        }

        private void LogMissedOpportunities(List<CumulativeTrade> trades)
        {
            foreach (var setup in _activeSetups.Where(s => !s.AggressionConfirmed && !s.Expired).Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc)))
            {
                var trigger = GetStudyTriggerLabel(setup);
                if (trigger == "NONE")
                    continue;

                var windowEnd = setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds);
                var allWindowTrades = trades
                    .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= windowEnd)
                    .Where(t => IsInLondonSession(t.Time))
                    .ToList();

                var windowTrades = allWindowTrades
                    .Where(t => IsLondonTradeAllowed(t.Time))
                    .ToList();

                var operationalWindowTrades = windowTrades
                    .Where(t => t.Volume >= MinAggressionVolume)
                    .ToList();

                var sameDirectionTrades = operationalWindowTrades
                    .Where(t => setup.Direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                    .ToList();

                var insideValueTrades = sameDirectionTrades
                    .Where(t => IsInEntryZone(setup, t))
                    .ToList();

                var freshInsideValueTrades = insideValueTrades
                    .Where(t => (t.Time - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds)
                    .ToList();

                var validRrEntryTrades = freshInsideValueTrades
                    .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                    .ToList();


                var reason = operationalWindowTrades.Count == 0
                    ? "NO_BIG_TRADE_IN_WINDOW"
                    : sameDirectionTrades.Count == 0
                        ? "NO_BIG_TRADE_IN_DIRECTION"
                        : insideValueTrades.Count == 0
                            ? "NO_BIG_TRADE_IN_ENTRY_ZONE"
                            : freshInsideValueTrades.Count == 0
                                ? "ENTRY_TOO_STALE_AFTER_REJECTION"
                                : validRrEntryTrades.Count == 0
                                    ? "RR_TO_TARGET2_TOO_LOW"
                                    : "UNKNOWN_NO_ENTRY";

                var maxVolume = operationalWindowTrades.Count > 0 ? operationalWindowTrades.Max(t => t.Volume) : 0m;
                var maxSameDirectionVolume = sameDirectionTrades.Count > 0 ? sameDirectionTrades.Max(t => t.Volume) : 0m;
                var firstTradeTime = operationalWindowTrades.Count > 0 ? FormatTime(operationalWindowTrades[0].Time) : "NA";
                var firstSameDirectionTime = sameDirectionTrades.Count > 0 ? FormatTime(sameDirectionTrades[0].Time) : "NA";
                var bestSameDirectionPrice = sameDirectionTrades.Count > 0
                    ? sameDirectionTrades.OrderByDescending(t => t.Volume).First().Lastprice.ToString("F2")
                    : "NA";
                var missedMessage = $"[MR_MISSED_OPPORTUNITY] SetupId={setup.SetupId}, Direction={setup.Direction}, StudyTrigger={trigger}, Reason={reason}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, DynamicStopMaxValueAreaRiskPct={DynamicStopMaxValueAreaRiskPct:F2}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, WindowBigTrades={operationalWindowTrades.Count}, SameDirectionBigTrades={sameDirectionTrades.Count}, EntryZoneBigTrades={insideValueTrades.Count}, FreshEntryZoneBigTrades={freshInsideValueTrades.Count}, ValidRrEntryTrades={validRrEntryTrades.Count}, MaxVolume={maxVolume:F0}, MaxSameDirectionVolume={maxSameDirectionVolume:F0}, FirstBigTrade={firstTradeTime}, FirstSameDirectionBigTrade={firstSameDirectionTime}, BestSameDirectionPrice={bestSameDirectionPrice}";
                _log(missedMessage, true);
                DailyHistoricalLog(missedMessage, setup.RejectionTimeUtc);

            }
        }

        private bool CreatePosition(BalanceSetup setup, CumulativeTrade entryTrade, string entryModel, bool isHistorical, bool isScaleIn = false, int scaleInIndex = 0)
        {
            var entryBar = FindBarByTime(entryTrade.Time, setup.RejectionBar);
            var studyTrigger = GetStudyTriggerLabel(setup);
            if (!isScaleIn && TryGetOpenDuplicateBasePosition(setup, entryTrade.Time, out var duplicate) && duplicate != null)
            {
                setup.AggressionConfirmed = true;
                var skipMessage = $"[MR_ENTRY_SKIPPED] SetupId={setup.SetupId}, EntryModel={entryModel}, {GetLogContractFields(isHistorical, setup)}, Direction={setup.Direction}, Reason=DUPLICATE_BASE_POSITION, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={entryTrade.Lastprice:F2}, ExistingSetupId={duplicate.SetupId}, ExistingEntryModel={duplicate.EntryModel}, ExistingEntry={duplicate.EntryPrice:F2}, ExistingEntryTime={FormatTime(duplicate.EntryTimeUtc)}, ExistingTarget1POC={duplicate.Target1Price:F2}, ExistingTarget2={duplicate.Target2Price:F2}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}";
                _log(skipMessage, isHistorical);
                if (entryModel.Contains("Historical", StringComparison.Ordinal))
                {
                    DailyHistoricalLog(skipMessage, entryTrade.Time);
                    StudyLog(skipMessage.Replace("[MR_ENTRY_SKIPPED]", "[DAY_STUDY_ENTRY_SKIPPED]"), entryTrade.Time);
                }
                return false;
            }
            var triggerAtEntry = GetStudyTriggerLabelAtTime(setup, entryTrade.Time);
            var target2 = GetStudyTarget2(setup);
            var useTarget2 = true;
            var finalTarget = target2;
            var operationalStop = GetOperationalStopPrice(setup, entryTrade.Lastprice);
            var operationalStopPlan = GetOperationalStopPlan(setup, entryTrade.Lastprice);
            var position = new ActivePosition
            {
                SetupId = setup.SetupId,
                Direction = setup.Direction,
                EntryModel = entryModel,
                SetupSource = setup.SetupSource,
                LiveParity = GetLiveParityForSetupSource(setup.SetupSource),
                EntryPrice = entryTrade.Lastprice,
                EntryBar = entryBar,
                EntryTimeUtc = entryTrade.Time,
                InitialStopPrice = operationalStop,
                StopPrice = operationalStop,
                TargetPrice = finalTarget,
                Target1Price = setup.POC,
                Target2Price = target2,
                UseTarget2 = useTarget2,
                IsScaleIn = isScaleIn,
                ScaleInIndex = scaleInIndex,
                ManagementMode = isScaleIn ? "VALUE_REENTRY_TARGET2_SCALE_IN_EXPAND25" : "VALUE_REENTRY_TARGET2",
                StudyTrigger = studyTrigger,
                MaxFavorablePrice = entryTrade.Lastprice,
                MaxAdversePrice = entryTrade.Lastprice
            };

            _activePositions.Add(position);

            var riskPoints = setup.Direction == "Long"
                ? position.EntryPrice - position.InitialStopPrice
                : position.InitialStopPrice - position.EntryPrice;
            var rewardPoints = setup.Direction == "Long"
                ? setup.TargetPrice - position.EntryPrice
                : position.EntryPrice - setup.TargetPrice;
            var rewardToTarget2 = setup.Direction == "Long"
                ? target2 - position.EntryPrice
                : position.EntryPrice - target2;
            var rewardToFinalTarget = setup.Direction == "Long"
                ? position.TargetPrice - position.EntryPrice
                : position.EntryPrice - position.TargetPrice;

            var rewardRiskToTarget2 = riskPoints > 0 ? rewardToTarget2 / riskPoints : 0;
            var entryMessage = $"[MR_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, {GetLogContractFields(isHistorical, setup)}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, OriginalStop={setup.StopPrice:F2}, OperationalStopPlan={operationalStopPlan}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, IsScaleIn={position.IsScaleIn}, ScaleInIndex={position.ScaleInIndex}, StudyTarget2={target2:F2}, OperationalTrigger={studyTrigger}, StudyTrigger={studyTrigger}, TriggerAtEntry={triggerAtEntry}, Risk={riskPoints:F2}, Reward={rewardPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, RewardRiskToTarget2={rewardRiskToTarget2:F2}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, DynamicStopMaxValueAreaRiskPct={DynamicStopMaxValueAreaRiskPct:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}";
            _log(entryMessage, isHistorical);
            if (entryModel.Contains("Historical", StringComparison.Ordinal))
            {
                DailyHistoricalLog(entryMessage, entryTrade.Time);
                StudyLog($"[DAY_STUDY_ACTUAL_ENTRY] SetupId={setup.SetupId}, EntryModel={entryModel}, MirrorsOperational=True, {GetLogContractFields(isHistorical, setup)}, Direction={setup.Direction}, Bar={entryBar}, {FormatTime(entryTrade.Time)}, EntryPrice={position.EntryPrice:F2}, Volume={entryTrade.Volume:F0}, TradeDirection={entryTrade.Direction}, Stop={position.StopPrice:F2}, OriginalStop={setup.StopPrice:F2}, OperationalStopPlan={operationalStopPlan}, TargetPOC={setup.TargetPrice:F2}, FinalTarget={position.TargetPrice:F2}, ManagementMode={position.ManagementMode}, IsScaleIn={position.IsScaleIn}, ScaleInIndex={position.ScaleInIndex}, StudyTarget2={target2:F2}, OperationalTrigger={studyTrigger}, StudyTrigger={studyTrigger}, TriggerAtEntry={triggerAtEntry}, Risk={riskPoints:F2}, RewardToTarget2={rewardToTarget2:F2}, RewardToFinalTarget={rewardToFinalTarget:F2}, RewardRiskToTarget2={rewardRiskToTarget2:F2}, SecondsAfterRejection={(entryTrade.Time - setup.RejectionTimeUtc).TotalSeconds:F1}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}", entryTrade.Time);
            }

            if (isHistorical)
                LogHistoricalPostEntryContext(position, entryTrade);

            return true;
        }

        private bool TryGetOpenDuplicateBasePosition(BalanceSetup setup, DateTime entryTimeUtc, out ActivePosition? duplicate)
        {
            duplicate = _activePositions
                .Where(p => !p.Closed && !p.IsScaleIn && p.Direction == setup.Direction)
                .Where(p => DateOnly.FromDateTime(MarketTimeZones.ToItaly(p.EntryTimeUtc)) == DateOnly.FromDateTime(MarketTimeZones.ToItaly(entryTimeUtc)))
                .Where(p => Math.Abs(p.Target1Price - setup.POC) <= DuplicateBasePositionPocTolerancePoints)
                .Where(p => Math.Abs(p.Target2Price - GetStudyTarget2(setup)) <= DuplicateBasePositionValueEdgeTolerancePoints)
                .OrderByDescending(p => p.EntryTimeUtc)
                .FirstOrDefault();

            return duplicate != null;
        }
    }
}
