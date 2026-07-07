using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private void ExpireStaleOperationalState(DateTime eventTimeUtc)
        {
            foreach (var setup in _activeSetups.Where(s => !s.Expired && eventTimeUtc > s.RejectionTimeUtc.AddSeconds(AggressionTimeoutSeconds)).ToList())
                setup.Expired = true;

        }

        private void LogLiveFlowHeartbeat(CumulativeTrade trade)
        {
            _liveAcceptedTradeCount++;
            var writeUtc = DateTime.UtcNow;
            var elapsedSeconds = _lastLiveHeartbeatUtc == DateTime.MinValue
                ? LiveHeartbeatMinSeconds
                : (writeUtc - _lastLiveHeartbeatUtc).TotalSeconds;
            var shouldLog = _liveAcceptedTradeCount == 1
                || _liveAcceptedTradeCount % LiveHeartbeatTradeStep == 0
                || elapsedSeconds >= LiveHeartbeatMinSeconds;
            if (!shouldLog)
                return;

            _lastLiveHeartbeatUtc = writeUtc;
            var activeSetups = _activeSetups.Count(s => !s.Expired && !s.AggressionConfirmed);
            var openPositions = _activePositions.Count(p => !p.Closed);
            var setupDiagnostics = activeSetups > 0 ? GetTradeCandidateDiagnostics(trade) : "NONE";
            var delayedDiagnostics = GetDelayedReclaimLiveDiagnostics(trade);
            _log($"[LIVE_FLOW_HEARTBEAT] EntryModel=FootprintCumulativeTradeLive, AcceptedTrades={_liveAcceptedTradeCount}, TradeTime={FormatTime(trade.Time)}, Direction={trade.Direction}, Price={trade.Lastprice:F2}, Volume={trade.Volume:F0}, ActiveSetups={activeSetups}, OpenPositions={openPositions}, DelayedCandidates={_delayedReclaimCandidates.Count(c => !c.EntryConfirmed && !c.Setup.Expired)}, SetupDiagnostics={setupDiagnostics}, DelayedDiagnostics={delayedDiagnostics}, MinAggressionVolume={MinAggressionVolume:F0}, Step={LiveHeartbeatTradeStep}, MinSeconds={LiveHeartbeatMinSeconds}", false);
        }

        private string GetDelayedReclaimLiveDiagnostics(CumulativeTrade trade)
        {
            var candidates = _delayedReclaimCandidates
                .Where(c => !c.EntryConfirmed && !c.Setup.Expired)
                .Where(c => trade.Time > c.Setup.RejectionTimeUtc && trade.Time <= c.Setup.RejectionTimeUtc.AddSeconds(OperationalEntryTimeoutSeconds))
                .OrderBy(c => Math.Abs((trade.Time - c.Setup.RejectionTimeUtc).TotalSeconds))
                .Take(3)
                .Select(c => GetDelayedReclaimCandidateDiagnostic(c, trade))
                .ToList();

            return candidates.Count == 0 ? "NONE" : string.Join("|", candidates);
        }

        private string GetDelayedReclaimCandidateDiagnostic(DelayedReclaimCandidate candidate, CumulativeTrade trade)
        {
            var setup = candidate.Setup;
            var expectedDirection = setup.Direction == "Long" ? TradeDirection.Buy : TradeDirection.Sell;
            var sameDirection = trade.Direction == expectedDirection;
            var inZone = IsDelayedReclaimEntryZone(setup, trade);
            var rr = GetRewardRiskToTarget2(setup, trade.Lastprice);
            GetDelayedReclaimPressureBeforeTime(candidate, trade, out var sameVolume, out var oppositeVolume, out var maxSameVolume, out var maxOppositeVolume);
            var acceptedBars = CountAcceptedBarsBeforeTime(setup, trade.Time, 3);
            var risk = GetOperationalRisk(setup, trade.Lastprice);
            var reason = trade.Volume < MinAggressionVolume * DelayedReclaimNarrativeMinBubbleMultiplier
                ? "VOLUME_BELOW_NARRATIVE_MIN"
                : !sameDirection
                    ? "WRONG_DIRECTION"
                    : !inZone
                        ? "OUTSIDE_DELAYED_ZONE"
                        : sameVolume <= oppositeVolume
                            ? "SAME_PRESSURE_NOT_DOMINANT"
                            : maxSameVolume < maxOppositeVolume
                                ? "MAX_BUBBLE_NOT_SAME"
                                : risk > DelayedReclaimMaxOperationalRiskPoints
                                    ? "RISK_TOO_HIGH"
                                    : rr < MinRewardRiskToTarget2
                                        ? "RR_TOO_LOW"
                                        : "CANDIDATE_READY";
            return $"{setup.SetupId}:{setup.Direction}:Reason={reason}:Age={(trade.Time - setup.RejectionTimeUtc).TotalSeconds:F0}s:InZone={inZone}:AcceptedBars={acceptedBars}:SameVol={sameVolume:F0}:OppVol={oppositeVolume:F0}:MaxSame={maxSameVolume:F0}:MaxOpp={maxOppositeVolume:F0}:RR={rr:F2}:Risk={risk:F2}";
        }
    }
}
