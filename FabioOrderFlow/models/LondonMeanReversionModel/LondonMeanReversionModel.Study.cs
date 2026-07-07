using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private void LogStudyBar(int bar, IndicatorCandle candle)
        {
            if (!_processingHistoricalPositions)
                return;

            if (!_studyLoggedBars.Add(bar))
                return;

            if (!IsInLondonSession(candle.Time))
                return;

            if (!ShouldDebugHistoricalDay(GetCandleEventTime(candle)))
                return;

            var snapshot = _historicalBarSnapshots.TryGetValue(bar, out var captured)
                ? captured
                : CreateHistoricalBarSnapshot(bar, candle);
            var candidateSetupState = GetBarSetupDiagnostics(snapshot);
            var activeSetupState = GetActiveSetupDiagnostics(snapshot.EventTimeUtc);
            StudyLog($"[DAY_STUDY_BAR] Bar={bar}, {FormatTime(snapshot.EventTimeUtc)}, Open={snapshot.Open:F2}, High={snapshot.High:F2}, Low={snapshot.Low:F2}, Close={snapshot.Close:F2}, Volume={snapshot.Volume:F0}, Bid={snapshot.Bid:F0}, Ask={snapshot.Ask:F0}, Delta={snapshot.Delta:F0}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, CloseLocation={snapshot.CloseLocation}, DistToPOC={snapshot.Close - snapshot.PreviewPOC:F2}, DistToVAH={snapshot.Close - snapshot.PreviewVAH:F2}, DistToVAL={snapshot.Close - snapshot.PreviewVAL:F2}, SetupDiagnostics={candidateSetupState}, ActiveSetupDiagnostics={activeSetupState}, TopLevels={snapshot.TopLevels}", snapshot.EventTimeUtc);
            LogBlockedSetupStudy(snapshot);
            LogPotentialPreviewRejection(snapshot);
            LogDelayedReclaimSetupStudy(snapshot);
        }

        private void LogBlockedSetupStudy(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return;

            var longBreakVal = snapshot.Low < snapshot.PreviewVAL;
            var longCloseBackInside = snapshot.Close > snapshot.PreviewVAL;
            var longRejectionTicks = (snapshot.Close - snapshot.Low) / _tickSize;
            var longReady = longBreakVal && longCloseBackInside && longRejectionTicks >= RejectionThresholdTicks;
            var isNewSessionLow = snapshot.Low < snapshot.PreviousSessionLow;

            if (longReady && !isNewSessionLow)
            {
                StudyLog($"[DAY_STUDY_SETUP_BLOCKED] Direction=Long, Bar={snapshot.Bar}, {FormatTime(snapshot.EventTimeUtc)}, Reason=NOT_NEW_SESSION_LOW, Low={snapshot.Low:F2}, Close={snapshot.Close:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, PreviousSessionLow={snapshot.PreviousSessionLow:F2}, RejectionTicks={longRejectionTicks:F1}, CloseLocation={snapshot.CloseLocation}", snapshot.EventTimeUtc);
            }

            var shortBreakVah = snapshot.High > snapshot.PreviewVAH;
            var shortCloseBackInside = snapshot.Close < snapshot.PreviewVAH;
            var shortRejectionTicks = (snapshot.High - snapshot.Close) / _tickSize;
            var shortReady = shortBreakVah && shortCloseBackInside && shortRejectionTicks >= RejectionThresholdTicks;
            var isNewSessionHigh = snapshot.High > snapshot.PreviousSessionHigh;

            if (shortReady && !isNewSessionHigh)
            {
                StudyLog($"[DAY_STUDY_SETUP_BLOCKED] Direction=Short, Bar={snapshot.Bar}, {FormatTime(snapshot.EventTimeUtc)}, Reason=NOT_NEW_SESSION_HIGH, High={snapshot.High:F2}, Close={snapshot.Close:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviousSessionHigh={snapshot.PreviousSessionHigh:F2}, RejectionTicks={shortRejectionTicks:F1}, CloseLocation={snapshot.CloseLocation}", snapshot.EventTimeUtc);
            }
        }

        private void LogPotentialPreviewRejection(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                return;

            var shortRejection = snapshot.High > snapshot.PreviewVAH && snapshot.Close < snapshot.PreviewVAH;
            var longRejection = snapshot.Low < snapshot.PreviewVAL && snapshot.Close > snapshot.PreviewVAL;
            if (!shortRejection && !longRejection)
                return;

            var shortRejectionDistance = snapshot.High - snapshot.Close;
            var longRejectionDistance = snapshot.Close - snapshot.Low;
            var matchingSetup = _activeSetups
                .Where(s => s.RejectionBar == snapshot.Bar)
                .Select(s => $"{s.SetupId[..Math.Min(8, s.SetupId.Length)]}:{s.SetupSource}:{s.Direction}")
                .ToList();

            var matchingSetupLabel = matchingSetup.Count == 0 ? "NONE" : string.Join("|", matchingSetup);
            StudyLog($"[DAY_STUDY_POTENTIAL_PREVIEW_REJECTION] Bar={snapshot.Bar}, {FormatTime(snapshot.EventTimeUtc)}, ShortRejection={shortRejection}, LongRejection={longRejection}, High={snapshot.High:F2}, Low={snapshot.Low:F2}, Close={snapshot.Close:F2}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, ShortRejectionTicks={shortRejectionDistance / _tickSize:F1}, LongRejectionTicks={longRejectionDistance / _tickSize:F1}, MatchingSetup={matchingSetupLabel}, CloseLocation={snapshot.CloseLocation}, Delta={snapshot.Delta:F0}, TopLevels={snapshot.TopLevels}", snapshot.EventTimeUtc);

            if (matchingSetup.Count == 0)
                LogPotentialPreviewRejectionOutcome(snapshot, shortRejection, longRejection);
        }

        private void LogPotentialPreviewRejectionOutcome(HistoricalBarSnapshot snapshot, bool shortRejection, bool longRejection)
        {
            if (shortRejection)
                LogPotentialPreviewRejectionOutcome(snapshot, "Short");

            if (longRejection)
                LogPotentialPreviewRejectionOutcome(snapshot, "Long");
        }

        private void LogPotentialPreviewRejectionOutcome(HistoricalBarSnapshot snapshot, string direction)
        {
            var setup = new BalanceSetup
            {
                SetupId = $"PREVIEW-{snapshot.Bar}-{direction}",
                Direction = direction,
                POC = snapshot.PreviewPOC,
                VAH = snapshot.PreviewVAH,
                VAL = snapshot.PreviewVAL,
                BreakoutBar = snapshot.Bar,
                BreakoutTimeUtc = snapshot.EventTimeUtc,
                BreakoutPrice = direction == "Long" ? snapshot.Low : snapshot.High,
                RejectionBar = snapshot.Bar,
                RejectionTimeUtc = snapshot.EventTimeUtc,
                RejectionClose = snapshot.Close,
                RejectionHigh = snapshot.High,
                RejectionLow = snapshot.Low,
                RejectionDelta = direction == "Long" ? snapshot.Close - snapshot.Low : snapshot.High - snapshot.Close,
                StopPrice = direction == "Long" ? snapshot.Low - StopOffsetTicks * _tickSize : snapshot.High + StopOffsetTicks * _tickSize,
                TargetPrice = snapshot.PreviewPOC,
                SetupSource = "PreviewRejectionStudy"
            };

            var candidates = _lastHistoricalTrades
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                .ToList();

            var zoneCandidates = candidates.Where(t => IsInEntryZone(setup, t)).ToList();
            var validCandidates = zoneCandidates
                .Where(t => (t.Time - setup.RejectionTimeUtc).TotalSeconds <= OperationalEntryTimeoutSeconds)
                .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                .ToList();
            var firstValid = validCandidates.OrderBy(t => t.Time).FirstOrDefault();
            var firstValidText = firstValid == null
                ? "NONE"
                : $"{FormatTime(firstValid.Time)}:Price={firstValid.Lastprice:F2}:Volume={firstValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstValid.Lastprice):F2}:Age={(firstValid.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";

            var outcomeText = "NA";
            if (firstValid != null)
            {
                var stop = GetOperationalStopPrice(setup, firstValid.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(direction, firstValid.Lastprice, stop, setup.TargetPrice, GetStudyTarget2(setup), firstValid.Time);
                outcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
            }

            StudyLog($"[DAY_STUDY_PREVIEW_REJECTION_OUTCOME] Bar={snapshot.Bar}, Direction={direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, Stop={setup.StopPrice:F2}, RejectionDelta={setup.RejectionDelta:F2}, SameDirectionTrades={candidates.Count}, InEntryZone={zoneCandidates.Count}, Valid={validCandidates.Count}, FirstValid={firstValidText}, Outcome={outcomeText}", snapshot.EventTimeUtc);
        }

        private void LogDailySetupCandidateSummaries(List<CumulativeTrade> trades)
        {
            foreach (var setup in _activeSetups.Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc)).OrderBy(s => s.RejectionTimeUtc))
            {
                var windowTrades = trades
                    .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
                    .Where(t => IsLondonTradeAllowed(t.Time))
                    .ToList();

                var sameDirection = 0;
                var wrongDirection = 0;
                var inEntryZone = 0;
                var valid = 0;
                var rrTooLow = 0;
                var stale = 0;
                CumulativeTrade? firstValid = null;
                CumulativeTrade? firstSameDirection = null;

                foreach (var trade in windowTrades)
                {
                    var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
                    if (trade.Direction != expectedDirection)
                    {
                        wrongDirection++;
                        continue;
                    }

                    sameDirection++;
                    firstSameDirection ??= trade;
                    if (!IsInEntryZone(setup, trade))
                        continue;

                    inEntryZone++;
                    var ageSeconds = (trade.Time - setup.RejectionTimeUtc).TotalSeconds;
                    if (ageSeconds > OperationalEntryTimeoutSeconds)
                    {
                        stale++;
                        continue;
                    }

                    var rr = GetRewardRiskToTarget2(setup, trade.Lastprice);
                    if (rr < MinRewardRiskToTarget2)
                    {
                        rrTooLow++;
                        continue;
                    }

                    valid++;
                    firstValid ??= trade;
                }

                var firstValidText = firstValid == null
                    ? "NONE"
                    : $"{FormatTime(firstValid.Time)}:Price={firstValid.Lastprice:F2}:Volume={firstValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstValid.Lastprice):F2}:Age={(firstValid.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";
                var firstSameDirectionText = firstSameDirection == null
                    ? "NONE"
                    : $"{FormatTime(firstSameDirection.Time)}:Price={firstSameDirection.Lastprice:F2}:Volume={firstSameDirection.Volume:F0}:InZone={IsInEntryZone(setup, firstSameDirection)}:RR={GetRewardRiskToTarget2(setup, firstSameDirection.Lastprice):F2}:Age={(firstSameDirection.Time - setup.RejectionTimeUtc).TotalSeconds:F1}s";

                StudyLog($"[DAY_STUDY_SETUP_CANDIDATE_SUMMARY] SetupId={setup.SetupId}, Source={setup.SetupSource}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, AggressionConfirmed={setup.AggressionConfirmed}, Expired={setup.Expired}, WindowTrades={windowTrades.Count}, SameDirection={sameDirection}, WrongDirection={wrongDirection}, InEntryZone={inEntryZone}, Valid={valid}, RRTooLow={rrTooLow}, Stale={stale}, FirstSameDirection={firstSameDirectionText}, FirstValid={firstValidText}", setup.RejectionTimeUtc);
            }
        }

        private void LogStudyCumulativeTrades(List<CumulativeTrade> trades)
        {
            foreach (var trade in trades.Where(t => t.Volume >= MinAggressionVolume && IsInLondonSession(t.Time) && ShouldDebugHistoricalDay(t.Time)))
            {
                var nearestSetup = FindNearestStudySetup(trade);
                var candidateDiagnostics = GetTradeCandidateDiagnostics(trade);
                StudyLog($"[DAY_STUDY_BIG_TRADE] {FormatTime(trade.Time)}, Direction={trade.Direction}, Volume={trade.Volume:F0}, FirstPrice={trade.FirstPrice:F2}, LastPrice={trade.Lastprice:F2}, TickCount={trade.Ticks.Count}, Location={GetTradeLocation(trade.Lastprice)}, NearestSetupId={nearestSetup.SetupId}, NearestSetupDirection={nearestSetup.Direction}, SecondsAfterSetup={nearestSetup.SecondsAfter:F1}, SameDirectionAsSetup={nearestSetup.SameDirection}, InEntryZone={nearestSetup.InEntryZone}, CandidateDiagnostics={candidateDiagnostics}", trade.Time);
            }
        }

        private void RunDayStudy()
        {
            if (_dayStudyCompleted || _lastHistoricalTrades.Count == 0)
                return;

            var studySetups = _activeSetups.Where(s => ShouldDebugHistoricalDay(s.RejectionTimeUtc)).ToList();

            foreach (var setup in studySetups)
            {
                var trigger = GetStudyTriggerLabel(setup);
                var windowEnd = setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds);
                var candidates = _lastHistoricalTrades
                    .Where(t => t.Volume >= MinAggressionVolume)
                    .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= windowEnd)
                    .Where(t => IsLondonTradeAllowed(t.Time))
                    .Where(t => setup.Direction == "Long" ? t.Direction == TradeDirection.Buy : t.Direction == TradeDirection.Sell)
                    .Select(t => BuildStudyCandidate(setup, t))
                    .Where(c => c.CandidateType != "NOT_CANDIDATE")
                    .ToList();

                StudyLog($"[DAY_STUDY_SETUP_SUMMARY] SetupId={setup.SetupId}, Direction={setup.Direction}, Trigger={trigger}, RejectionBar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, CandidateCount={candidates.Count}, AggressionConfirmed={setup.AggressionConfirmed}", setup.RejectionTimeUtc);

                foreach (var candidate in candidates)
                {
                    var triggerAtEntry = GetStudyTriggerLabelAtTime(setup, candidate.EntryTimeUtc);
                    var outcome = EvaluateCandidateOutcome(candidate.Direction, candidate.EntryPrice, candidate.Stop, candidate.TargetPoc, candidate.Target2, candidate.EntryTimeUtc, setup.RejectionBar);
                    StudyLog($"[DAY_STUDY_CANDIDATE_ENTRY] SetupId={setup.SetupId}, CandidateType={candidate.CandidateType}, Trigger={trigger}, TriggerAtEntry={triggerAtEntry}, Direction={candidate.Direction}, EntryTime={FormatTime(candidate.EntryTimeUtc)}, EntryPrice={candidate.EntryPrice:F2}, Volume={candidate.Volume:F0}, Stop={candidate.Stop:F2}, TargetPOC={candidate.TargetPoc:F2}, Target2={candidate.Target2:F2}, Risk={candidate.Risk:F2}, RewardPOC={candidate.RewardPoc:F2}, RewardT2={candidate.RewardT2:F2}, RR_POC={(candidate.Risk > 0 ? candidate.RewardPoc / candidate.Risk : 0):F2}, RR_T2={(candidate.Risk > 0 ? candidate.RewardT2 / candidate.Risk : 0):F2}, OutcomePOC={outcome.OutcomePoc}, PnLPOC={outcome.PnlPoc:F2}, OutcomeT2={outcome.OutcomeT2}, PnLT2={outcome.PnlT2:F2}, MFE={outcome.Mfe:F2}, MAE={outcome.Mae:F2}", candidate.EntryTimeUtc);
                    LogDynamicStopStudy(setup, trigger, candidate);
                }

                LogFabioStyleScaleInStudy(setup, trigger, candidates);
            }

            _dayStudyCompleted = true;
        }

        private void LogDynamicStopStudy(BalanceSetup setup, string trigger, StudyCandidate candidate)
        {
            if (candidate.CandidateType != "VALUE_REENTRY_BIG_TRADE")
                return;

            var secondsAfterRejection = (candidate.EntryTimeUtc - setup.RejectionTimeUtc).TotalSeconds;
            var triggerAtEntry = GetStudyTriggerLabelAtTime(setup, candidate.EntryTimeUtc);
            var secondsAfterPocTrigger = setup.StudyPocTriggerConfirmed
                ? (candidate.EntryTimeUtc - setup.StudyPocTriggerTimeUtc).TotalSeconds
                : double.NaN;
            var valueWidth = Math.Abs(setup.VAH - setup.VAL);
            var plans = new List<DynamicStopPlan>
            {
                new("ORIGINAL_REJECTION", candidate.Stop),
                new("VALUE_EDGE_2T", setup.Direction == "Long" ? setup.VAL - 2m * _tickSize : setup.VAH + 2m * _tickSize),
                new("RECENT_SWING_AFTER_REJECTION_2T", GetRecentSwingStop(setup, candidate.EntryTimeUtc))
            };

            if (setup.StudyPocTriggerConfirmed && candidate.EntryTimeUtc > setup.StudyPocTriggerTimeUtc)
                plans.Add(new DynamicStopPlan("POC_TRIGGER_BAR_2T", GetPocTriggerBarStop(setup)));

            if (valueWidth > 0)
            {
                plans.Add(new DynamicStopPlan("CAP_VALUE_WIDTH_100", GetCappedStop(setup.Direction, candidate.EntryPrice, candidate.Stop, valueWidth)));
                plans.Add(new DynamicStopPlan("CAP_VALUE_WIDTH_50", GetCappedStop(setup.Direction, candidate.EntryPrice, candidate.Stop, valueWidth * 0.5m)));
            }

            foreach (var plan in plans)
            {
                var risk = Math.Abs(candidate.EntryPrice - plan.Stop);
                if (risk <= 0 || !IsStopBehindEntry(setup.Direction, candidate.EntryPrice, plan.Stop))
                    continue;

                var rewardPoc = Math.Abs(candidate.TargetPoc - candidate.EntryPrice);
                var rewardT2 = Math.Abs(candidate.Target2 - candidate.EntryPrice);
                var outcome = EvaluateProtectedTarget2Outcome(candidate.Direction, candidate.EntryPrice, plan.Stop, candidate.TargetPoc, candidate.Target2, candidate.EntryTimeUtc, setup.RejectionBar);

                StudyLog($"[DAY_STUDY_DYNAMIC_STOP_CANDIDATE] SetupId={setup.SetupId}, StopPlan={plan.Name}, CandidateType={candidate.CandidateType}, Trigger={trigger}, TriggerAtEntry={triggerAtEntry}, Direction={candidate.Direction}, EntryTime={FormatTime(candidate.EntryTimeUtc)}, EntryPrice={candidate.EntryPrice:F2}, Volume={candidate.Volume:F0}, Stop={plan.Stop:F2}, OriginalStop={candidate.Stop:F2}, TargetPOC={candidate.TargetPoc:F2}, Target2={candidate.Target2:F2}, Risk={risk:F2}, OriginalRisk={candidate.Risk:F2}, RiskReductionPct={(candidate.Risk > 0 ? 1m - risk / candidate.Risk : 0):F2}, ValueWidth={valueWidth:F2}, RiskToValueWidth={(valueWidth > 0 ? risk / valueWidth : 0):F2}, RewardPOC={rewardPoc:F2}, RewardT2={rewardT2:F2}, RR_POC={rewardPoc / risk:F2}, RR_T2={rewardT2 / risk:F2}, SecondsAfterRejection={secondsAfterRejection:F1}, RejectionAgeBucket={GetRejectionAgeBucket(secondsAfterRejection)}, SecondsAfterPocTrigger={(double.IsNaN(secondsAfterPocTrigger) ? "NA" : secondsAfterPocTrigger.ToString("F1"))}, ExitReason={outcome.ExitReason}, PnL={outcome.Pnl:F2}, RMultiple={outcome.RMultiple:F2}R, Target1Hit={outcome.Target1Hit}", candidate.EntryTimeUtc);
            }
        }
    }
}
