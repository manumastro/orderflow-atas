using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioOrderFlow
{
    internal sealed partial class LondonMeanReversionModule
    {
        private void ResetStudyLog()
        {
            _studyLoggedBars.Clear();
            _historicalLogInitialized = false;
            _historicalStudyActive = !_dailyHistoricalDebugLogsEnabled;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_historicalLogPath)!);
                _historicalLogSequence = 0;
                var writeUtc = DateTime.UtcNow;
                var writeItaly = MarketTimeZones.ToItaly(writeUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                var mode = _dailyHistoricalDebugLogsEnabled ? "DailyHistoricalDebug" : "AggregatedHistoricalStudy";
                File.WriteAllText(_historicalLogPath, $"[Source=Historical] [Seq={++_historicalLogSequence}] [WriteItaly={writeItaly}] [WriteUtc={writeUtc:yyyy-MM-dd HH:mm:ss.fff}] [HISTORICAL_STUDY_START] Mode={mode}, AggregatedStudyEnabled={_historicalStudyActive}, DailyDebugEnabled={_dailyHistoricalDebugLogsEnabled}, HistoricalIntrabarEnabled={EnableHistoricalIntrabarFromCumulativeTrades}, OperationalLogs=MR_*, StudyLogs=DAY_STUDY_*, StudyLogsDoNotTrade=True, HistoricalReplayUsesOperationalEntryPath=True, HistoricalIntrabarReplay=HISTORICAL_ONLY_DISABLED_BY_LIVE_PARITY, MinAggressionVolume={MinAggressionVolume:F0}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, DynamicStopMaxValueAreaRiskPct={DynamicStopMaxValueAreaRiskPct:F2}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}, CreatedItaly={MarketTimeZones.ToItaly(DateTime.UtcNow):yyyy-MM-dd HH:mm:ss}{Environment.NewLine}");
                _historicalLogInitialized = true;
            }
            catch
            {
            }
        }

        private void ResetDailyHistoricalDebugLogs()
        {
            _initializedDailyLogs.Clear();
            _dailyHistoricalLogSequences.Clear();

            try
            {
                Directory.CreateDirectory(_dailyHistoricalLogDirectory);
                foreach (var path in Directory.EnumerateFiles(_dailyHistoricalLogDirectory, "FabioOrderFlow-day-*.log"))
                    File.Delete(path);
            }
            catch
            {
            }
        }

        private void StudyLog(string message, DateTime eventUtc)
        {
            if (!_historicalStudyDebugEnabled || !ShouldDebugHistoricalDay(eventUtc))
                return;

            DailyHistoricalLog(message, eventUtc);

            try
            {
                if (!_historicalStudyActive)
                    return;

                if (!_historicalLogInitialized)
                    ResetStudyLog();

                var writeUtc = DateTime.UtcNow;
                var writeItaly = MarketTimeZones.ToItaly(writeUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                var line = $"[Source=Historical] [Seq={++_historicalLogSequence}] [WriteItaly={writeItaly}] [WriteUtc={writeUtc:yyyy-MM-dd HH:mm:ss.fff}] [EventItaly={MarketTimeZones.ToItaly(eventUtc):yyyy-MM-dd HH:mm:ss}] [EventLondon={MarketTimeZones.ToLondon(eventUtc):yyyy-MM-dd HH:mm:ss}] [EventUtc={eventUtc:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(_historicalLogPath, line + Environment.NewLine);
            }
            catch
            {
            }
        }

        private void DailyHistoricalLog(string message, DateTime eventUtc)
        {
            if (!_dailyHistoricalDebugLogsEnabled || !ShouldDebugHistoricalDay(eventUtc))
                return;

            try
            {
                var eventItaly = MarketTimeZones.ToItaly(eventUtc);
                var day = DateOnly.FromDateTime(eventItaly);
                Directory.CreateDirectory(_dailyHistoricalLogDirectory);
                var path = Path.Combine(_dailyHistoricalLogDirectory, $"FabioOrderFlow-day-{day:yyyy-MM-dd}.log");
                if (!_initializedDailyLogs.Contains(day))
                {
                    var headerUtc = DateTime.UtcNow;
                    var headerItaly = MarketTimeZones.ToItaly(headerUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    File.WriteAllText(path, $"[Source=HistoricalDaily] [Seq=1] [WriteItaly={headerItaly}] [WriteUtc={headerUtc:yyyy-MM-dd HH:mm:ss.fff}] [DAY_DEBUG_START] Day={day:yyyy-MM-dd}, OperationalLogs=MR_*, StudyLogs=DAY_STUDY_*, StudyLogsDoNotTrade=True, HistoricalReplayUsesOperationalEntryPath=True, HistoricalIntrabarEnabled={EnableHistoricalIntrabarFromCumulativeTrades}, HistoricalIntrabarReplay=HISTORICAL_ONLY_DISABLED_BY_LIVE_PARITY, AggregatedStudyEnabled={_historicalStudyActive}, MinAggressionVolume={MinAggressionVolume:F0}, MinRewardRiskToTarget2={MinRewardRiskToTarget2:F2}, OperationalEntryTimeoutSeconds={OperationalEntryTimeoutSeconds}{Environment.NewLine}");
                    _initializedDailyLogs.Add(day);
                    _dailyHistoricalLogSequences[day] = 1;
                }

                var writeUtc = DateTime.UtcNow;
                var writeItaly = MarketTimeZones.ToItaly(writeUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
                var seq = _dailyHistoricalLogSequences.TryGetValue(day, out var currentSeq) ? currentSeq + 1 : 2;
                _dailyHistoricalLogSequences[day] = seq;
                var line = $"[Source=HistoricalDaily] [Seq={seq}] [WriteItaly={writeItaly}] [WriteUtc={writeUtc:yyyy-MM-dd HH:mm:ss.fff}] [EventItaly={eventItaly:yyyy-MM-dd HH:mm:ss}] [EventLondon={MarketTimeZones.ToLondon(eventUtc):yyyy-MM-dd HH:mm:ss}] [EventUtc={eventUtc:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(path, line + Environment.NewLine);
            }
            catch
            {
            }
        }
    }
}
