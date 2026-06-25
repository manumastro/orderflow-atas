using System;
using System.IO;
using ATAS.Indicators;

namespace FabioTrendFollowing;

public class FabioTrendFollowing : Indicator
{
    private BalanceZoneTracker? _balanceTracker;
    private CumulativeTradesRequest? _cumulativeTradesRequest;
    private static readonly bool DetailedDebugLogs = false;
    private readonly object _logSync = new();
    private readonly string _logPath;
    
    // Input parameter per abilitare footprint-first su live
    public bool EnableLiveFootprintFirst { get; set; } = false;

    public FabioTrendFollowing()
    {
        Name = "Fabio Trend Following";
        
        var logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs");

        Directory.CreateDirectory(logDirectory);
        _logPath = Path.Combine(logDirectory, "FabioTrendFollowing.log");

        try
        {
            using var logFile = new FileStream(_logPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }
        catch
        {
            try
            {
                if (File.Exists(_logPath))
                    File.Delete(_logPath);

                using var logFile = File.Create(_logPath);
            }
            catch
            {
            }
        }

        Log("[INIT] FabioTrendFollowing indicator created");
        Log($"[LOGS] Path={_logPath}");
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            Log($"[ONCALCULATE] Bar 0 - Initializing BalanceZoneTracker");
            Log($"[INSTRUMENT] Name: {InstrumentInfo?.Instrument}, TickSize: {InstrumentInfo?.TickSize}, Exchange: {InstrumentInfo?.Exchange}, InstrumentTimeZone={InstrumentInfo?.TimeZone}");
            Log($"[CHART] CurrentBar={CurrentBar}, ChartType={ChartInfo?.ChartType}");
            LogChartTradingSessions();
            _balanceTracker = new BalanceZoneTracker(this, Log, Rectangles, HorizontalLinesTillTouch, GetCandle, EnableLiveFootprintFirst);
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

            var maxLookbackStart = endTime.AddDays(-7);
            if (startTime < maxLookbackStart)
                startTime = maxLookbackStart;

            _cumulativeTradesRequest = new CumulativeTradesRequest(startTime, endTime, 0, 0);
            Log($"[CUM_TRADES_REQUEST] BeginItaly={MarketTimeZones.ToItaly(startTime):yyyy-MM-dd HH:mm:ss}, BeginUtc={startTime:yyyy-MM-dd HH:mm:ss}, EndItaly={MarketTimeZones.ToItaly(endTime):yyyy-MM-dd HH:mm:ss}, EndUtc={endTime:yyyy-MM-dd HH:mm:ss}, CurrentBar={CurrentBar}");
            RequestForCumulativeTrades(_cumulativeTradesRequest);
        }
        catch (Exception ex)
        {
            Log($"[CUM_TRADES_REQUEST_ERROR] {ex.Message}");
        }
    }

    protected override void OnCumulativeTrade(CumulativeTrade trade)
    {
        _balanceTracker?.OnLiveCumulativeTrade(trade);
    }

    protected override void OnUpdateCumulativeTrade(CumulativeTrade trade)
    {
        _balanceTracker?.OnLiveCumulativeTrade(trade);
    }

    protected override void OnCumulativeTradesResponse(CumulativeTradesRequest request, IEnumerable<CumulativeTrade> cumulativeTrades)
    {
        if (_cumulativeTradesRequest == null || request != _cumulativeTradesRequest)
            return;

        var trades = cumulativeTrades.ToList();
        Log($"[CUM_TRADES_RESPONSE] Count={trades.Count}, RequestId={request.RequestId}");
        _balanceTracker?.OnHistoricalCumulativeTrades(trades);
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

    private void Log(string message)
    {
        try
        {
            lock (_logSync)
            {
                var timestamp = MarketTimeZones.ToItaly(DateTime.UtcNow).ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logMessage = $"[Italy={timestamp}] {message}";
                File.AppendAllText(_logPath, logMessage + Environment.NewLine);
            }
        }
        catch
        {
        }
    }

}
