using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        public void ProcessHistoricalPositions(int startBar, int endBar)
        {
            var processStartUtc = DateTime.UtcNow;
            var previousProcessingState = _processingHistoricalPositions;
            _processingHistoricalPositions = true;
            _log($"[HISTORICAL_FLOW_PROCESS_START] StartBar={startBar}, EndBar={endBar}, StoredTrades={_lastHistoricalTrades.Count}, ExistingSnapshots={_historicalBarSnapshots.Count}, ExistingDelayedCandidates={_delayedReclaimCandidates.Count}", false);
            _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Start, StartBar={startBar}, EndBar={endBar}, StoredTrades={_lastHistoricalTrades.Count}, ExistingSnapshots={_historicalBarSnapshots.Count}, ExistingDelayedCandidates={_delayedReclaimCandidates.Count}, StudyDebugEnabled={_historicalStudyDebugEnabled}, DebugDays={FormatHistoricalStudyDebugDays()}", false);
            try
            {
                var totalBars = Math.Max(1, endBar - startBar + 1);
                for (var bar = startBar; bar <= endBar; bar++)
                {
                    var candle = _getCandle(bar);
                    CaptureHistoricalBarSnapshot(bar, candle);
                    UpdateDelayedReclaimCandidates(bar, candle);
                    if (_historicalStudyDebugEnabled)
                        LogStudyBar(bar, candle);
                    UpdateActivePositions(bar, candle);

                    if ((bar - startBar + 1) % HistoricalStudyProgressBarStep == 0 || bar == endBar)
                    {
                        var processedBars = bar - startBar + 1;
                        _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Bars, Processed={processedBars}, Total={totalBars}, Percent={(decimal)processedBars * 100m / totalBars:F1}, CurrentBar={bar}, EventItaly={MarketTimeZones.ToItaly(GetCandleEventTime(candle)):yyyy-MM-dd HH:mm:ss}", false);
                    }
                }

                ProcessStoredHistoricalTrades();

                LogHistoricalFlowFinish(startBar, endBar, processStartUtc);

                if (_historicalStudyDebugEnabled && !_dailyHistoricalDebugLogsEnabled)
                    RunDayStudy();
            }
            finally
            {
                _processingHistoricalPositions = previousProcessingState;
            }
        }

        private void ProcessStoredHistoricalTrades()
        {
            if (_lastHistoricalTrades.Count == 0)
                return;

            var totalTrades = _lastHistoricalTrades.Count;
            for (var i = 0; i < _lastHistoricalTrades.Count; i++)
            {
                var trade = _lastHistoricalTrades[i];
                if (trade.Volume >= MinAggressionVolume)
                    ProcessAggressionTrade(trade, "FootprintCumulativeTradeHistorical", true);

                UpdateHistoricalPositionsWithTrade(trade);

                if (((i + 1) % HistoricalStudyProgressTradeStep) == 0 || i == _lastHistoricalTrades.Count - 1)
                {
                    var processedTrades = i + 1;
                    _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Trades, Processed={processedTrades}, Total={totalTrades}, Percent={(decimal)processedTrades * 100m / totalTrades:F1}, EventItaly={MarketTimeZones.ToItaly(trade.Time):yyyy-MM-dd HH:mm:ss}", false);
                }
            }

            UpdateOpenHistoricalPositionsWithCompletedBars();
            CloseOpenHistoricalPositionsAtSessionEnd();
            if (_historicalStudyDebugEnabled)
                LogMissedOpportunities(_lastHistoricalTrades);
        }

        private void LogHistoricalFlowFinish(int startBar, int endBar, DateTime processStartUtc)
        {
            var completedUtc = DateTime.UtcNow;
            var durationMs = (completedUtc - processStartUtc).TotalMilliseconds;
            var flowDurationMs = _historicalFlowStartUtc == default ? 0 : (completedUtc - _historicalFlowStartUtc).TotalMilliseconds;
            var historicalEntries = _completedTrades.Count(t => t.EntryModel.Contains("Historical", StringComparison.OrdinalIgnoreCase));
            var delayedEntries = _completedTrades.Count(t => t.EntryModel.Contains("DelayedReclaim", StringComparison.OrdinalIgnoreCase));
            var openPositions = _activePositions.Count(p => !p.Closed);
            var closedPositions = _activePositions.Count(p => p.Closed);
            var positionRecords = _activePositions.Count;
            _log($"[HISTORICAL_FLOW_FINISH] StartBar={startBar}, EndBar={endBar}, Snapshots={_historicalBarSnapshots.Count}, StoredTrades={_lastHistoricalTrades.Count}, CompletedHistoricalEntries={historicalEntries}, CompletedDelayedReclaimEntries={delayedEntries}, OpenPositions={openPositions}, ClosedPositions={closedPositions}, PositionRecords={positionRecords}, ProcessDurationMs={durationMs:F0}, TotalFlowDurationMs={flowDurationMs:F0}", false);
            _log($"[HISTORICAL_RELOAD_PROGRESS] Phase=Finish, StartBar={startBar}, EndBar={endBar}, Snapshots={_historicalBarSnapshots.Count}, StoredTrades={_lastHistoricalTrades.Count}, CompletedHistoricalEntries={historicalEntries}, CompletedDelayedReclaimEntries={delayedEntries}, OpenPositions={openPositions}, ClosedPositions={closedPositions}, PositionRecords={positionRecords}, ProcessDurationMs={durationMs:F0}, TotalFlowDurationMs={flowDurationMs:F0}", false);

            if (!_dailyHistoricalDebugLogsEnabled)
                return;

            foreach (var day in _initializedDailyLogs.OrderBy(d => d))
            {
                var path = Path.Combine(_dailyHistoricalLogDirectory, $"FabioOrderFlow-day-{day:yyyy-MM-dd}.log");
                if (!File.Exists(path))
                    continue;

                try
                {
                    var text = File.ReadAllText(path);
                    var bars = CountOccurrences(text, "[DAY_STUDY_BAR]");
                    var bigTrades = CountOccurrences(text, "[DAY_STUDY_BIG_TRADE]");
                    var entries = CountOccurrences(text, "[MR_ENTRY]");
                    var delayedEntriesForDay = CountOccurrences(text, "EntryModel=FootprintCumulativeTradeHistoricalDelayedReclaim")
                        + CountOccurrences(text, "EntryModel=FootprintCumulativeTradeLiveDelayedReclaim");
                    var exits = CountOccurrences(text, "[MR_EXIT]");
                    var accepted = CountOccurrences(text, "[DAY_STUDY_DELAYED_RECLAIM_ACCEPTED]");
                    DailyHistoricalLog($"[DAY_DEBUG_FINISH] Day={day:yyyy-MM-dd}, Bars={bars}, BigTrades={bigTrades}, Entries={entries}, DelayedReclaimEntries={delayedEntriesForDay}, Exits={exits}, DelayedReclaimAccepted={accepted}, CompletedItaly={MarketTimeZones.ToItaly(completedUtc):yyyy-MM-dd HH:mm:ss}", new DateTime(day.Year, day.Month, day.Day, 23, 59, 59, DateTimeKind.Utc));
                }
                catch
                {
                }
            }
        }

        private static int CountOccurrences(string text, string token)
        {
            var count = 0;
            var index = 0;
            while ((index = text.IndexOf(token, index, StringComparison.Ordinal)) >= 0)
            {
                count++;
                index += token.Length;
            }

            return count;
        }
    }
}
