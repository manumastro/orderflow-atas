using System;
using System.IO;
using ATAS.Indicators;

namespace FabioOrderFlow;

public class FabioOrderFlow : Indicator
{
    private BalanceZoneTracker? _balanceTracker;
    private LondonMeanReversionModule? _meanReversionModule;
    private CumulativeTradesRequest? _cumulativeTradesRequest;
    private static readonly bool DetailedDebugLogs = false;
    private readonly object _logSync = new();
    private readonly string _logPath;
    private readonly string _historicalLogPath;
    private readonly string _liveLogPath;
    private long _logSequence;
    
    // Module parameters
    public bool EnableLondonMeanReversion { get; set; } = true;
    public bool EnablePostLondonImpulse { get; set; } = false;

    public FabioOrderFlow()
    {
        Name = "Fabio Order Flow";
        
        var logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs");

        Directory.CreateDirectory(logDirectory);
        _logPath = Path.Combine(logDirectory, "FabioOrderFlow.log");
        _historicalLogPath = Path.Combine(logDirectory, "FabioOrderFlow-historical.log");
        _liveLogPath = Path.Combine(logDirectory, "FabioOrderFlow-live.log");

        ResetAtasLogs(logDirectory);
        ResetLogFile(_logPath);
        ResetLogFile(_historicalLogPath);
        ResetLogFile(_liveLogPath);

        Log("[INIT] FabioOrderFlow indicator created");
        Log($"[LOGS] General={_logPath}, Historical={_historicalLogPath}, Live={_liveLogPath}");
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            Log($"[ONCALCULATE] Bar 0 - Initializing BalanceZoneTracker");
            Log($"[INSTRUMENT] Name: {InstrumentInfo?.Instrument}, TickSize: {InstrumentInfo?.TickSize}, Exchange: {InstrumentInfo?.Exchange}, InstrumentTimeZone={InstrumentInfo?.TimeZone}");
            Log($"[CHART] CurrentBar={CurrentBar}, ChartType={ChartInfo?.ChartType}");
            LogChartTradingSessions();
            _balanceTracker = new BalanceZoneTracker(this, Log, Rectangles, HorizontalLinesTillTouch, GetCandle);
            
            // Initialize Mean Reversion module if enabled
            if (EnableLondonMeanReversion)
            {
                var tickSize = InstrumentInfo?.TickSize ?? 1m;
                _meanReversionModule = new LondonMeanReversionModule(_balanceTracker, Log, GetCandle, tickSize);
                _balanceTracker.SetMeanReversionModule(_meanReversionModule);
                Log($"[MODULE] London Mean Reversion module initialized (Live-first, TickSize={tickSize})");
            }
            
            return;
        }

        var candle = GetCandle(bar);
        
        if (DetailedDebugLogs && bar % 50 == 0)
        {
            Log($"[BAR_CHECK] Bar={bar}, Italy={MarketTimeZones.ToItaly(candle.Time):yyyy-MM-dd HH:mm:ss}, UTC={candle.Time:yyyy-MM-dd HH:mm:ss}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}");
        }
        
        _balanceTracker?.OnBarUpdate(bar, candle, CurrentBar);
    }

    protected override void OnFinishRecalculate()
    {
        if (CurrentBar <= 1)
            return;

        try
        {
            var startTime = GetCandle(0).Time;
            var endTime = GetCandle(CurrentBar - 1).LastTime;
            if (endTime <= startTime)
                endTime = GetCandle(CurrentBar - 1).Time;

            var chartStartTime = startTime;
            var maxLookbackStart = endTime.AddDays(-7);
            if (startTime < maxLookbackStart)
                startTime = maxLookbackStart;

            _cumulativeTradesRequest = new CumulativeTradesRequest(startTime, endTime, 0, 0);
            Log($"[CUM_TRADES_LOOKBACK] Mode=Last7DaysSingleRequest, ChartBeginItaly={MarketTimeZones.ToItaly(chartStartTime):yyyy-MM-dd HH:mm:ss}, EffectiveBeginItaly={MarketTimeZones.ToItaly(startTime):yyyy-MM-dd HH:mm:ss}, EndItaly={MarketTimeZones.ToItaly(endTime):yyyy-MM-dd HH:mm:ss}, CurrentBar={CurrentBar}");
            Log($"[CUM_TRADES_REQUEST] RequestId={_cumulativeTradesRequest.RequestId}, BeginItaly={MarketTimeZones.ToItaly(startTime):yyyy-MM-dd HH:mm:ss}, BeginUtc={startTime:yyyy-MM-dd HH:mm:ss}, EndItaly={MarketTimeZones.ToItaly(endTime):yyyy-MM-dd HH:mm:ss}, EndUtc={endTime:yyyy-MM-dd HH:mm:ss}, CurrentBar={CurrentBar}");
            RequestForCumulativeTrades(_cumulativeTradesRequest);
        }
        catch (Exception ex)
        {
            Log($"[CUM_TRADES_REQUEST_ERROR] {ex.Message}");
        }
    }

    protected override void OnCumulativeTradesResponse(CumulativeTradesRequest request, IEnumerable<CumulativeTrade> cumulativeTrades)
    {
        if (_cumulativeTradesRequest == null || request != _cumulativeTradesRequest)
            return;

        var trades = cumulativeTrades.ToList();
        Log($"[CUM_TRADES_RESPONSE] Count={trades.Count}, RequestId={request.RequestId}");
        _balanceTracker?.OnHistoricalCumulativeTrades(trades);
        
        if (_meanReversionModule != null && CurrentBar > 0)
            _meanReversionModule.ProcessHistoricalPositions(0, CurrentBar - 1);
    }

    protected override void OnCumulativeTrade(CumulativeTrade trade)
    {
        _balanceTracker?.OnLiveCumulativeTrade(trade);
    }

    protected override void OnUpdateCumulativeTrade(CumulativeTrade trade)
    {
        _balanceTracker?.OnLiveCumulativeTrade(trade);
    }

    protected override void OnNewTrade(MarketDataArg trade)
    {
        // Tick diagnostics are intentionally disabled in the production indicator.
        // Live behavior is debugged through operational MR logs and historical replay.
    }
    
    private void LogChartTradingSessions()
    {
        try
        {
            var sessions = ChartInfo?.TradingSessionDescriptions;
            if (sessions == null)
            {
                Log("[CHART_SESSIONS] TradingSessionDescriptions=null");
                return;
            }

            var count = 0;
            foreach (var session in sessions)
            {
                count++;
                Log($"[CHART_SESSIONS] #{count}: {session}");
            }

            if (count == 0)
            {
                Log("[CHART_SESSIONS] No trading session descriptions exposed by chart");
            }
        }
        catch (Exception ex)
        {
            Log($"[CHART_SESSIONS] Error reading trading sessions: {ex.Message}");
        }
    }

    private static void ResetAtasLogs(string logDirectory)
    {
        try
        {
            foreach (var path in Directory.EnumerateFiles(logDirectory, "*.log"))
                ResetLogFile(path);
        }
        catch
        {
        }
    }

    private static void ResetLogFile(string path)
    {
        try
        {
            using var logFile = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }
        catch
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                using var logFile = File.Create(path);
            }
            catch
            {
            }
        }
    }

    private void Log(string message, bool isHistorical = false)
    {
        try
        {
            lock (_logSync)
            {
                var source = isHistorical ? "Historical" : "General";
                var line = FormatLogLine(message, source);
                File.AppendAllText(_logPath, line + Environment.NewLine);

                if (!isHistorical && IsLiveOperationalMessage(message))
                {
                    var onlineLine = FormatLogLine(message, "Live");
                    File.AppendAllText(_liveLogPath, onlineLine + Environment.NewLine);
                }
            }
        }
        catch
        {
        }
    }

    private string FormatLogLine(string message, string source)
    {
        var writeUtc = DateTime.UtcNow;
        var writeItaly = MarketTimeZones.ToItaly(writeUtc).ToString("yyyy-MM-dd HH:mm:ss.fff");
        return $"[Source={source}] [Seq={++_logSequence}] [WriteItaly={writeItaly}] [WriteUtc={writeUtc:yyyy-MM-dd HH:mm:ss.fff}] {message}";
    }

    private static bool IsLiveOperationalMessage(string message)
    {
        if (!message.StartsWith("[MR_", StringComparison.Ordinal))
            return false;

        return message.Contains("EntryModel=FootprintCumulativeTradeLive", StringComparison.Ordinal);
    }

}
