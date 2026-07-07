using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private void LogFabioStyleScaleInStudy(BalanceSetup setup, string trigger, List<StudyCandidate> candidates)
        {
            var valueCandidates = candidates
                .Where(c => c.CandidateType == "VALUE_REENTRY_BIG_TRADE")
                .Select(c => ToOperationalStudyCandidate(setup, c))
                .Where(c => c.Risk > 0 && c.RewardT2 / c.Risk >= MinRewardRiskToTarget2)
                .OrderBy(c => c.EntryTimeUtc)
                .ToList();

            var baseCandidate = valueCandidates
                .FirstOrDefault(c => (c.EntryTimeUtc - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds);
            if (baseCandidate == null)
                return;

            var baseOutcome = EvaluateCandidateOutcome(baseCandidate.Direction, baseCandidate.EntryPrice, baseCandidate.Stop, baseCandidate.TargetPoc, baseCandidate.Target2, baseCandidate.EntryTimeUtc, setup.RejectionBar);
            var riskFreeTime = FindTargetPocHitTime(baseCandidate.Direction, baseCandidate.EntryPrice, baseCandidate.Stop, baseCandidate.TargetPoc, baseCandidate.EntryTimeUtc, setup.RejectionBar);
            var baseReachedRiskFree = riskFreeTime.HasValue;
            var riskFreeTimeValue = riskFreeTime.GetValueOrDefault();
            var addOns = baseReachedRiskFree
                ? valueCandidates.Where(c => c.EntryTimeUtc > riskFreeTimeValue).ToList()
                : new List<StudyCandidate>();

            StudyLog($"[DAY_STUDY_SCALE_IN_SUMMARY] SetupId={setup.SetupId}, Trigger={trigger}, Direction={setup.Direction}, BaseEntryTime={FormatTime(baseCandidate.EntryTimeUtc)}, BaseEntryPrice={baseCandidate.EntryPrice:F2}, BaseVolume={baseCandidate.Volume:F0}, BaseRisk={baseCandidate.Risk:F2}, BaseRR_T2={(baseCandidate.Risk > 0 ? baseCandidate.RewardT2 / baseCandidate.Risk : 0):F2}, BaseOutcomePOC={baseOutcome.OutcomePoc}, BaseOutcomeT2={baseOutcome.OutcomeT2}, BasePnLT2={baseOutcome.PnlT2:F2}, BaseReachedRiskFree={baseReachedRiskFree}, RiskFreeTime={(baseReachedRiskFree ? FormatTime(riskFreeTimeValue) : "NA")}, AddOnCandidates={addOns.Count}, FabioRule=ADD_ONLY_AFTER_POC_RISK_FREE", setup.RejectionTimeUtc);

            var scaleInIndex = 0;
            foreach (var addOn in addOns)
            {
                scaleInIndex++;
                var outcome = EvaluateCandidateOutcome(addOn.Direction, addOn.EntryPrice, addOn.Stop, addOn.TargetPoc, addOn.Target2, addOn.EntryTimeUtc, setup.RejectionBar);
                StudyLog($"[DAY_STUDY_SCALE_IN_CANDIDATE] SetupId={setup.SetupId}, ScaleInIndex={scaleInIndex}, Trigger={trigger}, Direction={addOn.Direction}, BaseEntryTime={FormatTime(baseCandidate.EntryTimeUtc)}, RiskFreeTime={FormatTime(riskFreeTimeValue)}, EntryTime={FormatTime(addOn.EntryTimeUtc)}, EntryPrice={addOn.EntryPrice:F2}, Volume={addOn.Volume:F0}, Stop={addOn.Stop:F2}, TargetPOC={addOn.TargetPoc:F2}, Target2={addOn.Target2:F2}, Risk={addOn.Risk:F2}, RewardT2={addOn.RewardT2:F2}, RR_T2={(addOn.Risk > 0 ? addOn.RewardT2 / addOn.Risk : 0):F2}, OutcomeT2={outcome.OutcomeT2}, PnLT2={outcome.PnlT2:F2}, RMultiple={(addOn.Risk > 0 ? outcome.PnlT2 / addOn.Risk : 0):F2}R, MFE={outcome.Mfe:F2}, MAE={outcome.Mae:F2}", addOn.EntryTimeUtc);
            }

            LogScalePlanStudy(setup, trigger, baseCandidate, riskFreeTime, addOns);
        }

        private void LogScalePlanStudy(BalanceSetup setup, string trigger, StudyCandidate baseCandidate, DateTime? riskFreeTime, List<StudyCandidate> addOns)
        {
            var plans = new[]
            {
                new ScalePlan("NO_SCALE", 0, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_1", 1, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_2", 2, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_3", 3, MinAggressionVolume, null, 0m),
                new ScalePlan("SCALE_MAX_1_VOL20", 1, 20m, null, 0m),
                new ScalePlan("SCALE_MAX_2_VOL20", 2, 20m, null, 0m),
                new ScalePlan("SCALE_MAX_1_WITHIN_3MIN", 1, MinAggressionVolume, 180, 0m),
                new ScalePlan("SCALE_MAX_1_WITHIN_5MIN", 1, MinAggressionVolume, 300, 0m),
                new ScalePlan("SCALE_MAX_2_WITHIN_5MIN", 2, MinAggressionVolume, 300, 0m),
                new ScalePlan("SCALE_MAX_1_EXPAND25", 1, MinAggressionVolume, null, 0.25m),
                new ScalePlan("SCALE_MAX_1_EXPAND50", 1, MinAggressionVolume, null, 0.50m),
                new ScalePlan("SCALE_MAX_2_EXPAND25", 2, MinAggressionVolume, null, 0.25m),
                new ScalePlan("SCALE_MAX_1_WITHIN_5MIN_EXPAND25", 1, MinAggressionVolume, 300, 0.25m)
            };

            var baseProtectedOutcome = EvaluateProtectedTarget2Outcome(baseCandidate.Direction, baseCandidate.EntryPrice, baseCandidate.Stop, baseCandidate.TargetPoc, baseCandidate.Target2, baseCandidate.EntryTimeUtc, setup.RejectionBar);

            foreach (var plan in plans)
            {
                var selectedAddOns = SelectScalePlanAddOns(addOns, plan, riskFreeTime).ToList();
                var totalPnl = baseProtectedOutcome.Pnl;
                var totalR = baseProtectedOutcome.RMultiple;
                var winners = baseProtectedOutcome.Pnl > 0 ? 1 : 0;
                var losers = baseProtectedOutcome.Pnl < 0 ? 1 : 0;
                var worstLegR = baseProtectedOutcome.RMultiple;

                foreach (var addOn in selectedAddOns)
                {
                    var addOnOutcome = EvaluateProtectedTarget2Outcome(addOn.Direction, addOn.EntryPrice, addOn.Stop, addOn.TargetPoc, addOn.Target2, addOn.EntryTimeUtc, setup.RejectionBar);
                    totalPnl += addOnOutcome.Pnl;
                    totalR += addOnOutcome.RMultiple;
                    if (addOnOutcome.Pnl > 0)
                        winners++;
                    if (addOnOutcome.Pnl < 0)
                        losers++;
                    worstLegR = Math.Min(worstLegR, addOnOutcome.RMultiple);
                }

                StudyLog($"[DAY_STUDY_SCALE_PLAN] SetupId={setup.SetupId}, Plan={plan.Name}, Trigger={trigger}, Direction={setup.Direction}, BaseEntryTime={FormatTime(baseCandidate.EntryTimeUtc)}, BaseEntryPrice={baseCandidate.EntryPrice:F2}, BaseExitReason={baseProtectedOutcome.ExitReason}, BasePnL={baseProtectedOutcome.Pnl:F2}, BaseR={baseProtectedOutcome.RMultiple:F2}R, BaseTarget1Hit={baseProtectedOutcome.Target1Hit}, RiskFreeTime={(riskFreeTime.HasValue ? FormatTime(riskFreeTime.Value) : "NA")}, AddOnCount={selectedAddOns.Count}, AddOnMinVolume={plan.MinVolume:F0}, AddOnMaxSecondsAfterRiskFree={(plan.MaxSecondsAfterRiskFree.HasValue ? plan.MaxSecondsAfterRiskFree.Value.ToString() : "ANY")}, AddOnMinExpansionAfterRiskFreePct={plan.MinExpansionAfterRiskFreePct:F2}, Winners={winners}, Losers={losers}, TotalPnL={totalPnl:F2}, TotalR={totalR:F2}R, WorstLegR={worstLegR:F2}R, MaxOpenContracts={1 + selectedAddOns.Count}", setup.RejectionTimeUtc);
            }
        }

        private IEnumerable<StudyCandidate> SelectScalePlanAddOns(List<StudyCandidate> addOns, ScalePlan plan, DateTime? riskFreeTime)
        {
            if (plan.MaxAddOns <= 0 || !riskFreeTime.HasValue)
                return Enumerable.Empty<StudyCandidate>();

            var riskFreeTimeValue = riskFreeTime.Value;
            return addOns
                .Where(c => c.Volume >= plan.MinVolume)
                .Where(c => !plan.MaxSecondsAfterRiskFree.HasValue || c.EntryTimeUtc <= riskFreeTimeValue.AddSeconds(plan.MaxSecondsAfterRiskFree.Value))
                .Where(c => HasExpandedAfterRiskFree(c, riskFreeTimeValue, plan.MinExpansionAfterRiskFreePct))
                .OrderBy(c => c.EntryTimeUtc)
                .Take(plan.MaxAddOns);
        }

        private bool HasExpandedAfterRiskFree(BalanceSetup setup, CumulativeTrade trade, DateTime riskFreeTimeUtc, decimal minExpansionPct)
        {
            if (minExpansionPct <= 0)
                return true;

            var target2 = GetStudyTarget2(setup);
            var targetDistance = Math.Abs(target2 - setup.POC);
            if (targetDistance <= 0)
                return false;

            var requiredExpansion = targetDistance * minExpansionPct;
            var startBar = FindBarByTime(riskFreeTimeUtc, setup.RejectionBar);
            var endBar = FindBarByTime(trade.Time, startBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(trade.Time));
            var bestExpansion = 0m;

            for (var bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                bestExpansion = setup.Direction == "Long"
                    ? Math.Max(bestExpansion, candle.High - setup.POC)
                    : Math.Max(bestExpansion, setup.POC - candle.Low);
            }

            return bestExpansion >= requiredExpansion;
        }

        private bool HasExpandedAfterRiskFree(StudyCandidate candidate, DateTime riskFreeTimeUtc, decimal minExpansionPct)
        {
            if (minExpansionPct <= 0)
                return true;

            var targetDistance = Math.Abs(candidate.Target2 - candidate.TargetPoc);
            if (targetDistance <= 0)
                return false;

            var requiredExpansion = targetDistance * minExpansionPct;
            var startBar = FindBarByTime(riskFreeTimeUtc, 0);
            var endBar = FindBarByTime(candidate.EntryTimeUtc, startBar);
            var entryDay = DateOnly.FromDateTime(MarketTimeZones.ToItaly(candidate.EntryTimeUtc));
            var bestExpansion = 0m;

            for (var bar = startBar; bar <= endBar; bar++)
            {
                var candle = _getCandle(bar);
                var eventTime = GetCandleEventTime(candle);
                if (DateOnly.FromDateTime(MarketTimeZones.ToItaly(eventTime)) != entryDay)
                    break;

                if (!IsInLondonSession(eventTime))
                    break;

                bestExpansion = candidate.Direction == "Long"
                    ? Math.Max(bestExpansion, candle.High - candidate.TargetPoc)
                    : Math.Max(bestExpansion, candidate.TargetPoc - candle.Low);
            }

            return bestExpansion >= requiredExpansion;
        }

        private StudyCandidate BuildStudyCandidate(BalanceSetup setup, CumulativeTrade trade)
        {
            var candidateType = IsInEntryZone(setup, trade)
                ? "VALUE_REENTRY_BIG_TRADE"
                : "NOT_CANDIDATE";

            var target2 = GetStudyTarget2(setup);
            var risk = Math.Abs(trade.Lastprice - setup.StopPrice);
            var rewardPoc = Math.Abs(setup.POC - trade.Lastprice);
            var rewardT2 = Math.Abs(target2 - trade.Lastprice);

            return new StudyCandidate(setup.Direction, candidateType, trade.Time, trade.Lastprice, trade.Volume, setup.StopPrice, setup.POC, target2, risk, rewardPoc, rewardT2);
        }
    }
}
