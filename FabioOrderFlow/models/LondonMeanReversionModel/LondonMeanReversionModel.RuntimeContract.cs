using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private const string LogicPathOperational = "OPERATIONAL";
        private const string LogicPathStudyOnly = "STUDY_ONLY";

        private static string GetExecutionMode(bool isHistorical)
        {
            return isHistorical ? "HISTORICAL_REPLAY" : "LIVE";
        }

        private static string GetExecutionModeFromEntryModel(string entryModel)
        {
            return entryModel.Contains("Historical", StringComparison.OrdinalIgnoreCase)
                ? "HISTORICAL_REPLAY"
                : "LIVE";
        }

        private static string GetLiveParityForSetupSource(string setupSource)
        {
            return setupSource switch
            {
                "BarClose" => "LIVE_SAME_BAR_UPDATE_PATH",
                "DelayedReclaimAccepted" => "LIVE_SAME_DELAYED_RECLAIM_PATH",
                "HistoricalIntrabar" => "HISTORICAL_ONLY_DISABLED_BY_LIVE_PARITY",
                "SecondaryValueRejection" => "LIVE_SAME_BAR_UPDATE_PATH",
                "DelayedReclaimStudy" => "STUDY_ONLY_NOT_TRADED",
                "PreviewRejectionStudy" => "STUDY_ONLY_NOT_TRADED",
                _ => "UNKNOWN"
            };
        }

        private static string GetLogContractFields(bool isHistorical, BalanceSetup setup)
        {
            return $"ExecutionMode={GetExecutionMode(isHistorical)}, LogicPath={LogicPathOperational}, StudyOnly=False, SetupSource={setup.SetupSource}, LiveParity={GetLiveParityForSetupSource(setup.SetupSource)}";
        }

        private static string GetPositionContractFields(ActivePosition position)
        {
            return $"ExecutionMode={GetExecutionModeFromEntryModel(position.EntryModel)}, LogicPath={LogicPathOperational}, StudyOnly=False, SetupSource={position.SetupSource}, LiveParity={position.LiveParity}";
        }
    }
}
