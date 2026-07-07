using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private const decimal MinAggressionVolume = 10m;
        private const int AggressionTimeoutSeconds = 3600;
        private const int OperationalEntryTimeoutSeconds = 1200;
        private const int RejectionThresholdTicks = 10;
        private const int StopOffsetTicks = 2;
        private const int LondonSessionEndHour = 16;
        private const int LondonSessionEndMinute = 0;
        private const decimal MinRewardRiskToTarget2 = 1.0m;
        private const decimal DynamicStopMaxValueAreaRiskPct = 0.50m;
        private const decimal PocPartialExitPct = 0.70m;
        private const decimal RunnerExitPct = 1m - PocPartialExitPct;
        private const decimal ScaleInMinExpansionAfterRiskFreePct = 0.25m;
        private const decimal ScaleInMinRewardToTarget1Points = 4m;
        private const int MaxScaleInsPerSetup = 2;
        private const decimal DelayedReclaimNarrativeMinBubbleMultiplier = 5m;
        private const int DelayedReclaimMinAcceptedBars = 2;
        private const int DelayedReclaimImmediateMaxSeconds = 120;
        private const decimal DelayedReclaimDominantBubbleMultiplier = 2m;
        private const decimal DelayedReclaimEarlyPressureVolumeRatio = 1.50m;
        private const decimal DelayedReclaimMaxOperationalRiskPoints = 120m;
        private const decimal DuplicateBasePositionPocTolerancePoints = 4m;
        private const decimal DuplicateBasePositionValueEdgeTolerancePoints = 8m;
        private static readonly bool EnableOperationalPressureGate = true;
        private const decimal PressureGateMinObservedVolume = 30m;
        private const decimal PressureGateOppositeDominanceRatio = 1.25m;
        private static readonly bool EnableOperationalSecondaryValueRejection = true;
        private static readonly bool EnableHistoricalIntrabarFromCumulativeTrades = false;
        private static readonly bool OperationalCoreMeanReversionOnly = true;
        private const int HistoricalStudyProgressBarStep = 100;
        private const int HistoricalStudyProgressTradeStep = 50000;
        private const int LiveHeartbeatTradeStep = 25;
        private const int LiveHeartbeatMinSeconds = 60;
        private static readonly DateOnly[] HistoricalStudyDebugDays =
        {
            new(2026, 6, 29),
            new(2026, 6, 30),
            new(2026, 7, 1),
            new(2026, 7, 2),
            new(2026, 7, 3),
            new(2026, 7, 6),
            new(2026, 7, 7),
        };
        private const string HistoricalStudyDebugMarkerFile = "FabioOrderFlow-enable-historical-study-debug.flag";
    }
}
