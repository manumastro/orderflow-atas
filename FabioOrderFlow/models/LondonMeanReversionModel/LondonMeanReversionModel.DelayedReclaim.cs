using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private void LogDelayedReclaimSetupStudy(HistoricalBarSnapshot snapshot)
        {
            foreach (var direction in GetDelayedReclaimDirections(snapshot))
                LogDelayedReclaimSetupStudy(snapshot, direction);
        }

        private void LogDelayedReclaimSetupStudy(HistoricalBarSnapshot snapshot, string direction)
        {
            var setup = CreateDelayedReclaimSetup(snapshot, direction, "DelayedReclaimStudy");
            if (setup == null)
                return;

            var excursionLow = setup.Direction == "Long" ? setup.BreakoutPrice : _historicalBarSnapshots.Values.Where(s => s.Bar >= snapshot.Bar - 6 && s.Bar <= snapshot.Bar).Max(s => s.Low);
            var excursionHigh = setup.Direction == "Short" ? setup.BreakoutPrice : _historicalBarSnapshots.Values.Where(s => s.Bar >= snapshot.Bar - 6 && s.Bar <= snapshot.Bar).Max(s => s.High);
            var stop = setup.StopPrice;

            var expectedDirection = direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var candidates = _lastHistoricalTrades
                .Where(t => t.Time > snapshot.EventTimeUtc && t.Time <= snapshot.EventTimeUtc.AddSeconds(OperationalEntryTimeoutSeconds))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .Where(t => t.Direction == expectedDirection)
                .Where(t => IsDelayedReclaimEntryZone(setup, t))
                .OrderBy(t => t.Time)
                .ToList();

            var validCandidates = candidates
                .Where(t => GetRewardRiskToTarget2(setup, t.Lastprice) >= MinRewardRiskToTarget2)
                .ToList();
            var firstValid = validCandidates.FirstOrDefault();
            var firstValidText = firstValid == null
                ? "NONE"
                : $"{FormatTime(firstValid.Time)}:Price={firstValid.Lastprice:F2}:Volume={firstValid.Volume:F0}:RR={GetRewardRiskToTarget2(setup, firstValid.Lastprice):F2}:Age={(firstValid.Time - snapshot.EventTimeUtc).TotalSeconds:F1}s";

            var outcomeText = "NA";
            if (firstValid != null)
            {
                var operationalStop = GetOperationalStopPrice(setup, firstValid.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(direction, firstValid.Lastprice, operationalStop, setup.TargetPrice, GetStudyTarget2(setup), firstValid.Time);
                outcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
            }

            var activeSetupState = GetActiveSetupDiagnostics(snapshot.EventTimeUtc);
            StudyLog($"[DAY_STUDY_DELAYED_RECLAIM_SETUP] Bar={snapshot.Bar}, Direction={direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={snapshot.PreviewPOC:F2}, PreviewVAH={snapshot.PreviewVAH:F2}, PreviewVAL={snapshot.PreviewVAL:F2}, Close={snapshot.Close:F2}, Delta={snapshot.Delta:F0}, ExcursionLow={excursionLow:F2}, ExcursionHigh={excursionHigh:F2}, Stop={stop:F2}, ActiveSetupDiagnostics={activeSetupState}, CandidateCount={candidates.Count}, Valid={validCandidates.Count}, FirstValid={firstValidText}, Outcome={outcomeText}", snapshot.EventTimeUtc);
            LogDelayedReclaimNarrativeStudy(snapshot, setup, validCandidates);
        }

        private void LogDelayedReclaimNarrativeStudy(HistoricalBarSnapshot snapshot, BalanceSetup setup, List<CumulativeTrade> validCandidates)
        {
            var pre = GetDelayedReclaimNarrativeStats(snapshot.EventTimeUtc.AddMinutes(-15), snapshot.EventTimeUtc, setup.Direction);
            var post = GetDelayedReclaimNarrativeStats(snapshot.EventTimeUtc, snapshot.EventTimeUtc.AddMinutes(15), setup.Direction);
            var nextBarHolds = IsDelayedReclaimHeldByNextBar(snapshot, setup.Direction);
            var acceptedInsideValue = CountAcceptedBarsAfterReclaim(snapshot, setup.Direction, bars: 3);
            var pressureCandidate = FindNarrativePressureCandidate(snapshot, setup, validCandidates);
            var outcomeText = "NA";
            var candidateText = "NONE";
            if (pressureCandidate != null)
            {
                var stop = GetOperationalStopPrice(setup, pressureCandidate.Lastprice);
                var outcome = EvaluateProtectedTarget2OutcomeFromTrades(setup.Direction, pressureCandidate.Lastprice, stop, setup.TargetPrice, GetStudyTarget2(setup), pressureCandidate.Time);
                candidateText = $"{FormatTime(pressureCandidate.Time)}:Price={pressureCandidate.Lastprice:F2}:Volume={pressureCandidate.Volume:F0}:RR={GetRewardRiskToTarget2(setup, pressureCandidate.Lastprice):F2}:Age={(pressureCandidate.Time - snapshot.EventTimeUtc).TotalSeconds:F1}s";
                outcomeText = $"ExitReason={outcome.ExitReason}:PnL={outcome.Pnl:F2}:R={outcome.RMultiple:F2}:Target1Hit={outcome.Target1Hit}";
            }

            var narrativeAccepted = acceptedInsideValue >= 2
                && post.NetVolume > 0
                && post.MaxBubbleSide == "SAME"
                && pressureCandidate != null;

            StudyLog($"[DAY_STUDY_DELAYED_RECLAIM_NARRATIVE] Bar={snapshot.Bar}, Direction={setup.Direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={setup.POC:F2}, PreviewVAH={setup.VAH:F2}, PreviewVAL={setup.VAL:F2}, NextBarHolds={nextBarHolds}, AcceptedBarsNext3={acceptedInsideValue}, PreSameVolume15m={pre.SameDirectionVolume:F0}, PreOppositeVolume15m={pre.OppositeDirectionVolume:F0}, PreNetVolume15m={pre.NetVolume:F0}, PostSameVolume15m={post.SameDirectionVolume:F0}, PostOppositeVolume15m={post.OppositeDirectionVolume:F0}, PostNetVolume15m={post.NetVolume:F0}, PressureShift={post.PressureShift - pre.PressureShift:F2}, PostMaxBubbleSide={post.MaxBubbleSide}, PostMaxBubbleVolume={post.MaxBubbleVolume:F0}, NarrativeAccepted={narrativeAccepted}, Valid={validCandidates.Count}, NarrativeCandidate={candidateText}, Outcome={outcomeText}", snapshot.EventTimeUtc);

            if (narrativeAccepted)
                StudyLog($"[DAY_STUDY_DELAYED_RECLAIM_ACCEPTED] Bar={snapshot.Bar}, Direction={setup.Direction}, {FormatTime(snapshot.EventTimeUtc)}, PreviewPOC={setup.POC:F2}, PreviewVAH={setup.VAH:F2}, PreviewVAL={setup.VAL:F2}, AcceptedBarsNext3={acceptedInsideValue}, PreNetVolume15m={pre.NetVolume:F0}, PostNetVolume15m={post.NetVolume:F0}, PressureShift={post.PressureShift - pre.PressureShift:F2}, PostMaxBubbleSide={post.MaxBubbleSide}, PostMaxBubbleVolume={post.MaxBubbleVolume:F0}, NarrativeCandidate={candidateText}, Outcome={outcomeText}", snapshot.EventTimeUtc);
        }

        private bool IsDelayedReclaimHeldByNextBar(HistoricalBarSnapshot snapshot, string direction)
        {
            if (!_historicalBarSnapshots.TryGetValue(snapshot.Bar + 1, out var next))
                return false;

            return direction == "Long"
                ? next.Close >= snapshot.PreviewVAL
                : next.Close <= snapshot.PreviewVAH;
        }

        private int CountAcceptedBarsAfterReclaim(HistoricalBarSnapshot snapshot, string direction, int bars)
        {
            var count = 0;
            for (var i = 1; i <= bars; i++)
            {
                if (!_historicalBarSnapshots.TryGetValue(snapshot.Bar + i, out var next))
                    break;

                var accepted = direction == "Long"
                    ? next.Close >= snapshot.PreviewVAL
                    : next.Close <= snapshot.PreviewVAH;
                if (accepted)
                    count++;
            }

            return count;
        }

        private DelayedReclaimNarrativeStats GetDelayedReclaimNarrativeStats(DateTime beginUtc, DateTime endUtc, string direction)
        {
            var expectedDirection = direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var trades = _lastHistoricalTrades
                .Where(t => t.Time > beginUtc && t.Time <= endUtc)
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .ToList();
            var same = trades.Where(t => t.Direction == expectedDirection).ToList();
            var opposite = trades.Where(t => t.Direction == oppositeDirection).ToList();
            var maxSame = same.Count == 0 ? 0 : same.Max(t => t.Volume);
            var maxOpposite = opposite.Count == 0 ? 0 : opposite.Max(t => t.Volume);
            var sameVolume = same.Sum(t => t.Volume);
            var oppositeVolume = opposite.Sum(t => t.Volume);
            var maxSide = maxSame == 0 && maxOpposite == 0
                ? "NONE"
                : maxSame >= maxOpposite ? "SAME" : "OPPOSITE";
            var maxVolume = Math.Max(maxSame, maxOpposite);
            var netVolume = sameVolume - oppositeVolume;
            var pressureShift = (sameVolume + oppositeVolume) <= 0
                ? 0
                : netVolume / (sameVolume + oppositeVolume);

            return new DelayedReclaimNarrativeStats(
                same.Count,
                sameVolume,
                maxSame,
                opposite.Count,
                oppositeVolume,
                maxOpposite,
                maxSide,
                maxVolume,
                netVolume,
                pressureShift);
        }

        private CumulativeTrade? FindNarrativePressureCandidate(HistoricalBarSnapshot snapshot, BalanceSetup setup, List<CumulativeTrade> validCandidates)
        {
            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = setup.Direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var validByTime = validCandidates.OrderBy(t => t.Time).ToList();
            if (validByTime.Count == 0)
                return null;

            var sameVolume = 0m;
            var oppositeVolume = 0m;
            foreach (var trade in _lastHistoricalTrades
                .Where(t => t.Time > snapshot.EventTimeUtc && t.Time <= snapshot.EventTimeUtc.AddMinutes(15))
                .Where(t => IsLondonTradeAllowed(t.Time))
                .Where(t => t.Volume >= MinAggressionVolume)
                .OrderBy(t => t.Time))
            {
                if (trade.Direction == expectedDirection)
                    sameVolume += trade.Volume;
                else if (trade.Direction == oppositeDirection)
                    oppositeVolume += trade.Volume;

                var sameHasControl = sameVolume > 0 && sameVolume >= oppositeVolume;
                if (!sameHasControl)
                    continue;

                var candidate = validByTime.FirstOrDefault(t => t.Time >= trade.Time);
                if (candidate != null)
                    return candidate;
            }

            return null;
        }

        private IEnumerable<string> GetDelayedReclaimDirections(HistoricalBarSnapshot snapshot)
        {
            if (snapshot.PreviewPOC == 0 || snapshot.PreviewVAH == 0 || snapshot.PreviewVAL == 0)
                yield break;

            if (!_historicalBarSnapshots.TryGetValue(snapshot.Bar - 1, out var previous))
                yield break;

            if (previous.PreviewPOC == 0 || previous.PreviewVAH == 0 || previous.PreviewVAL == 0)
                yield break;

            if (previous.Close < previous.PreviewVAL && snapshot.Close > snapshot.PreviewVAL)
                yield return "Long";

            if (previous.Close > previous.PreviewVAH && snapshot.Close < snapshot.PreviewVAH)
                yield return "Short";
        }

        private BalanceSetup? CreateDelayedReclaimSetup(HistoricalBarSnapshot snapshot, string direction, string source)
        {
            var lookbackBars = _historicalBarSnapshots.Values
                .Where(s => s.Bar >= snapshot.Bar - 6 && s.Bar <= snapshot.Bar)
                .OrderBy(s => s.Bar)
                .ToList();
            if (lookbackBars.Count == 0)
                return null;

            var excursionLow = lookbackBars.Min(s => s.Low);
            var excursionHigh = lookbackBars.Max(s => s.High);
            var stop = direction == "Long"
                ? excursionLow - StopOffsetTicks * _tickSize
                : excursionHigh + StopOffsetTicks * _tickSize;

            return new BalanceSetup
            {
                SetupId = $"DELAYED-RECLAIM-{snapshot.Bar}-{direction}",
                Direction = direction,
                POC = snapshot.PreviewPOC,
                VAH = snapshot.PreviewVAH,
                VAL = snapshot.PreviewVAL,
                BreakoutBar = snapshot.Bar,
                BreakoutTimeUtc = snapshot.EventTimeUtc,
                BreakoutPrice = direction == "Long" ? excursionLow : excursionHigh,
                RejectionBar = snapshot.Bar,
                RejectionTimeUtc = snapshot.EventTimeUtc,
                RejectionClose = snapshot.Close,
                RejectionHigh = snapshot.High,
                RejectionLow = snapshot.Low,
                RejectionDelta = snapshot.Delta,
                StopPrice = stop,
                TargetPrice = snapshot.PreviewPOC,
                SetupSource = source
            };
        }

        private void UpdateDelayedReclaimCandidates(int bar, IndicatorCandle candle)
        {
            if (!IsInLondonSession(candle.Time))
                return;

            var snapshot = _historicalBarSnapshots.TryGetValue(bar, out var captured)
                ? captured
                : CreateHistoricalBarSnapshot(bar, candle);

            foreach (var direction in GetDelayedReclaimDirections(snapshot))
            {
                var setup = CreateDelayedReclaimSetup(snapshot, direction, "DelayedReclaimAccepted");
                if (setup == null)
                    continue;

                if (_delayedReclaimCandidates.Any(c => c.Setup.SetupId == setup.SetupId))
                    continue;

                _delayedReclaimCandidates.Add(new DelayedReclaimCandidate
                {
                    Setup = setup,
                    LastUpdatedUtc = snapshot.EventTimeUtc
                });
                _log($"[MR_DELAYED_RECLAIM_SETUP] SetupId={setup.SetupId}, ExecutionMode={(IsHistoricalBar(bar) ? "HISTORICAL_REPLAY" : "LIVE")}, LogicPath={LogicPathOperational}, StudyOnly=False, SetupSource={setup.SetupSource}, LiveParity={GetLiveParityForSetupSource(setup.SetupSource)}, Direction={setup.Direction}, Bar={setup.RejectionBar}, {FormatTime(setup.RejectionTimeUtc)}, POC={setup.POC:F2}, VAH={setup.VAH:F2}, VAL={setup.VAL:F2}, Stop={setup.StopPrice:F2}, TargetPOC={setup.TargetPrice:F2}", IsHistoricalBar(bar));
            }

            foreach (var candidate in _delayedReclaimCandidates.Where(c => !c.EntryConfirmed && !c.Setup.Expired).ToList())
                UpdateDelayedReclaimCandidateAcceptance(candidate, snapshot);
        }

        private void UpdateDelayedReclaimCandidateAcceptance(DelayedReclaimCandidate candidate, HistoricalBarSnapshot snapshot)
        {
            var setup = candidate.Setup;
            if (snapshot.EventTimeUtc <= setup.RejectionTimeUtc)
                return;

            if (!_processingHistoricalPositions && snapshot.EventTimeUtc > setup.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds))
            {
                setup.Expired = true;
                return;
            }

            var accepted = setup.Direction == "Long"
                ? snapshot.Close >= setup.VAL
                : snapshot.Close <= setup.VAH;
            if (accepted)
                candidate.AcceptedBars++;

            candidate.OperationallyReady = candidate.AcceptedBars >= 2;
            candidate.LastUpdatedUtc = snapshot.EventTimeUtc;
        }

        private bool TryProcessDelayedReclaimEntry(CumulativeTrade trade, string entryModel, bool isHistorical)
        {
            foreach (var candidate in _delayedReclaimCandidates.Where(c => !c.EntryConfirmed && !c.Setup.Expired).ToList())
            {
                var setup = candidate.Setup;
                if (!IsDelayedReclaimTradeCandidate(candidate, trade, out var acceptedBars, out var sameVolume, out var oppositeVolume, out var maxSameVolume, out var maxOppositeVolume, out var acceptanceMode))
                    continue;

                candidate.EntryConfirmed = true;
                setup.AggressionConfirmed = true;
                var resolvedEntryModel = isHistorical
                    ? "FootprintCumulativeTradeHistoricalDelayedReclaim"
                    : "FootprintCumulativeTradeLiveDelayedReclaim";
                if (CreatePosition(setup, trade, resolvedEntryModel, isHistorical))
                    _log($"[MR_DELAYED_RECLAIM_ENTRY] SetupId={setup.SetupId}, EntryModel={resolvedEntryModel}, {GetLogContractFields(isHistorical, setup)}, Direction={setup.Direction}, {FormatTime(trade.Time)}, EntryPrice={trade.Lastprice:F2}, Volume={trade.Volume:F0}, AcceptedBars={acceptedBars}, AcceptanceMode={acceptanceMode}, SameDirectionVolume={sameVolume:F0}, OppositeDirectionVolume={oppositeVolume:F0}, MaxSameDirectionVolume={maxSameVolume:F0}, MaxOppositeDirectionVolume={maxOppositeVolume:F0}", isHistorical);
                return true;
            }

            return false;
        }

        private bool IsDelayedReclaimTradeCandidate(
            DelayedReclaimCandidate candidate,
            CumulativeTrade trade,
            out int acceptedBars,
            out decimal sameDirectionVolume,
            out decimal oppositeDirectionVolume,
            out decimal maxSameDirectionVolume,
            out decimal maxOppositeDirectionVolume,
            out string acceptanceMode)
        {
            acceptedBars = 0;
            sameDirectionVolume = 0;
            oppositeDirectionVolume = 0;
            maxSameDirectionVolume = 0;
            maxOppositeDirectionVolume = 0;
            acceptanceMode = "NONE";

            var setup = candidate.Setup;
            if (trade.Time <= setup.RejectionTimeUtc || trade.Time > setup.RejectionTimeUtc.AddSeconds(OperationalEntryTimeoutSeconds))
                return false;

            if (!IsLondonTradeAllowed(trade.Time) || trade.Volume < MinAggressionVolume)
                return false;

            acceptedBars = CountAcceptedBarsBeforeTime(setup, trade.Time, 3);

            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            if (trade.Direction != expectedDirection)
                return false;

            if (trade.Volume < MinAggressionVolume * DelayedReclaimNarrativeMinBubbleMultiplier)
                return false;

            if (!IsDelayedReclaimEntryZone(setup, trade))
                return false;

            GetDelayedReclaimPressureBeforeTime(candidate, trade, out sameDirectionVolume, out oppositeDirectionVolume, out maxSameDirectionVolume, out maxOppositeDirectionVolume);
            if (sameDirectionVolume <= oppositeDirectionVolume)
                return false;

            if (maxSameDirectionVolume < maxOppositeDirectionVolume)
                return false;

            if (GetOperationalRisk(setup, trade.Lastprice) > DelayedReclaimMaxOperationalRiskPoints)
                return false;

            if (!IsDelayedReclaimCausallyAccepted(setup, trade, acceptedBars, sameDirectionVolume, oppositeDirectionVolume, maxSameDirectionVolume, maxOppositeDirectionVolume, out acceptanceMode))
                return false;

            return GetRewardRiskToTarget2(setup, trade.Lastprice) >= MinRewardRiskToTarget2;
        }

        private bool IsDelayedReclaimCausallyAccepted(
            BalanceSetup setup,
            CumulativeTrade trade,
            int acceptedBars,
            decimal sameDirectionVolume,
            decimal oppositeDirectionVolume,
            decimal maxSameDirectionVolume,
            decimal maxOppositeDirectionVolume,
            out string acceptanceMode)
        {
            acceptanceMode = "NONE";
            var ageSeconds = (trade.Time - setup.RejectionTimeUtc).TotalSeconds;
            var dominantBubble = maxSameDirectionVolume >= maxOppositeDirectionVolume * DelayedReclaimDominantBubbleMultiplier;
            var strongEarlyPressure = oppositeDirectionVolume <= 0
                || sameDirectionVolume >= oppositeDirectionVolume * DelayedReclaimEarlyPressureVolumeRatio;

            if (acceptedBars >= DelayedReclaimMinAcceptedBars)
            {
                acceptanceMode = "CONFIRMED_ACCEPTANCE";
                return true;
            }

            if (ageSeconds <= DelayedReclaimImmediateMaxSeconds && dominantBubble)
            {
                acceptanceMode = "IMMEDIATE_DOMINANT_PRESSURE";
                return true;
            }

            if (acceptedBars >= 1 && dominantBubble && strongEarlyPressure)
            {
                acceptanceMode = "EARLY_ACCEPTED_PRESSURE";
                return true;
            }

            return false;
        }

        private int CountAcceptedBarsBeforeTime(BalanceSetup setup, DateTime tradeTimeUtc, int maxBars)
        {
            var accepted = 0;
            foreach (var snapshot in _historicalBarSnapshots.Values
                .Where(s => s.EventTimeUtc > setup.RejectionTimeUtc && s.EventTimeUtc < tradeTimeUtc)
                .OrderBy(s => s.Bar)
                .Take(maxBars))
            {
                var inside = setup.Direction == "Long"
                    ? snapshot.Close >= setup.VAL
                    : snapshot.Close <= setup.VAH;
                if (inside)
                    accepted++;
            }

            return accepted;
        }

        private void GetDelayedReclaimPressureBeforeTime(
            DelayedReclaimCandidate candidate,
            CumulativeTrade trade,
            out decimal sameDirectionVolume,
            out decimal oppositeDirectionVolume,
            out decimal maxSameDirectionVolume,
            out decimal maxOppositeDirectionVolume)
        {
            sameDirectionVolume = 0;
            oppositeDirectionVolume = 0;
            maxSameDirectionVolume = 0;
            maxOppositeDirectionVolume = 0;
            var setup = candidate.Setup;
            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var oppositeDirection = setup.Direction == "Long" ? TradeDirection.Sell : TradeDirection.Buy;
            var source = _processedAggressionTrades
                .Where(t => t.Time > setup.RejectionTimeUtc && t.Time <= trade.Time && t.Volume >= MinAggressionVolume)
                .ToList();
            if (source.Count == 0 && trade.Time > setup.RejectionTimeUtc && trade.Volume >= MinAggressionVolume)
                source.Add(trade);

            foreach (var t in source)
            {
                if (t.Direction == expectedDirection)
                {
                    sameDirectionVolume += t.Volume;
                    maxSameDirectionVolume = Math.Max(maxSameDirectionVolume, t.Volume);
                }
                else if (t.Direction == oppositeDirection)
                {
                    oppositeDirectionVolume += t.Volume;
                    maxOppositeDirectionVolume = Math.Max(maxOppositeDirectionVolume, t.Volume);
                }
            }
        }

        private bool IsDelayedReclaimEntryZone(BalanceSetup setup, CumulativeTrade trade)
        {
            var price = trade.Lastprice;
            return setup.Direction == "Long"
                ? price >= setup.VAL && price <= setup.POC
                : price <= setup.VAH && price >= setup.POC;
        }
    }
}
