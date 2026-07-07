using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        public class BalanceSetup
        {
            public string SetupId { get; set; } = Guid.NewGuid().ToString();
            public string Direction { get; set; } = string.Empty;
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            public int BreakoutBar { get; set; }
            public DateTime BreakoutTimeUtc { get; set; }
            public decimal BreakoutPrice { get; set; }
            public int RejectionBar { get; set; }
            public DateTime RejectionTimeUtc { get; set; }
            public decimal RejectionClose { get; set; }
            public decimal RejectionHigh { get; set; }
            public decimal RejectionLow { get; set; }
            public decimal RejectionDelta { get; set; }
            public decimal StopPrice { get; set; }
            public decimal TargetPrice { get; set; }
            public bool AggressionConfirmed { get; set; }
            public bool ScaleInConfirmed { get; set; }
            public bool Expired { get; set; }
            public bool StudyPocTriggerConfirmed { get; set; }
            public int StudyPocTriggerBar { get; set; } = -1;
            public DateTime StudyPocTriggerTimeUtc { get; set; }
            public int StudyTriggerBar { get; set; } = -1;
            public DateTime StudyTriggerTimeUtc { get; set; }
            public string SetupSource { get; set; } = "BarClose";
        }

        public class ActivePosition
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string EntryModel { get; set; } = string.Empty;
            public decimal EntryPrice { get; set; }
            public int EntryBar { get; set; }
            public DateTime EntryTimeUtc { get; set; }
            public decimal InitialStopPrice { get; set; }
            public decimal StopPrice { get; set; }
            public decimal TargetPrice { get; set; }
            public decimal Target1Price { get; set; }
            public decimal Target2Price { get; set; }
            public bool UseTarget2 { get; set; }
            public bool Target1Hit { get; set; }
            public int Target1HitBar { get; set; } = -1;
            public DateTime Target1HitTimeUtc { get; set; }
            public bool StopProtectedAfterTarget1 { get; set; }
            public decimal RealizedPocPnL { get; set; }
            public decimal RealizedPocExitPct { get; set; }
            public decimal RunnerExitPct { get; set; } = 1m;
            public bool IsScaleIn { get; set; }
            public int ScaleInIndex { get; set; }
            public DateTime ExitTimeUtc { get; set; }
            public string ManagementMode { get; set; } = string.Empty;
            public string StudyTrigger { get; set; } = string.Empty;
            public decimal MaxFavorablePrice { get; set; }
            public decimal MaxAdversePrice { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public bool Closed { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal ExitPrice { get; set; }
            public int ExitBar { get; set; }
        }


        private sealed class HistoricalBarSnapshot
        {
            public int Bar { get; set; }
            public DateTime EventTimeUtc { get; set; }
            public decimal Open { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Close { get; set; }
            public decimal Volume { get; set; }
            public decimal Bid { get; set; }
            public decimal Ask { get; set; }
            public decimal Delta { get; set; }
            public decimal PreviewPOC { get; set; }
            public decimal PreviewVAH { get; set; }
            public decimal PreviewVAL { get; set; }
            public decimal PreviousSessionHigh { get; set; }
            public decimal PreviousSessionLow { get; set; }
            public string CloseLocation { get; set; } = string.Empty;
            public string TopLevels { get; set; } = string.Empty;
        }

        private sealed class DelayedReclaimCandidate
        {
            public BalanceSetup Setup { get; set; } = new();
            public int AcceptedBars { get; set; }
            public bool OperationallyReady { get; set; }
            public bool EntryConfirmed { get; set; }
            public decimal SameDirectionVolume { get; set; }
            public decimal OppositeDirectionVolume { get; set; }
            public decimal MaxSameDirectionVolume { get; set; }
            public decimal MaxOppositeDirectionVolume { get; set; }
            public DateTime LastUpdatedUtc { get; set; }
        }

        private readonly record struct DelayedReclaimNarrativeStats(
            int SameDirectionTrades,
            decimal SameDirectionVolume,
            decimal MaxSameDirectionVolume,
            int OppositeDirectionTrades,
            decimal OppositeDirectionVolume,
            decimal MaxOppositeDirectionVolume,
            string MaxBubbleSide,
            decimal MaxBubbleVolume,
            decimal NetVolume,
            decimal PressureShift);


        public class TradeRecord
        {
            public string SetupId { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string EntryModel { get; set; } = string.Empty;
            public DateTime BreakoutTime { get; set; }
            public decimal BreakoutPrice { get; set; }
            public DateTime RejectionTime { get; set; }
            public decimal POC { get; set; }
            public decimal VAH { get; set; }
            public decimal VAL { get; set; }
            public DateTime EntryTime { get; set; }
            public decimal EntryPrice { get; set; }
            public decimal EntryVolume { get; set; }
            public DateTime ExitTime { get; set; }
            public decimal ExitPrice { get; set; }
            public string ExitReason { get; set; } = string.Empty;
            public decimal PnL { get; set; }
            public decimal MFE { get; set; }
            public decimal MAE { get; set; }
            public decimal RMultiple { get; set; }
        }

        private sealed record StudyCandidate(string Direction, string CandidateType, DateTime EntryTimeUtc, decimal EntryPrice, decimal Volume, decimal Stop, decimal TargetPoc, decimal Target2, decimal Risk, decimal RewardPoc, decimal RewardT2);
        private sealed record StudyOutcome(string OutcomePoc, decimal PnlPoc, string OutcomeT2, decimal PnlT2, decimal Mfe, decimal Mae);
        private sealed record ProtectedStudyOutcome(string ExitReason, decimal Pnl, decimal RMultiple, bool Target1Hit);
        private sealed record PocManagementPressureStats(int SameTrades, decimal SameVolume, decimal MaxSameVolume, int OppositeTrades, decimal OppositeVolume, decimal MaxOppositeVolume);
        private sealed record HistoricalContext(decimal POC, decimal VAH, decimal VAL);
        private sealed record DynamicStopPlan(string Name, decimal Stop);
        private sealed record ScalePlan(string Name, int MaxAddOns, decimal MinVolume, int? MaxSecondsAfterRiskFree, decimal MinExpansionAfterRiskFreePct);
    }
}
