using System;
using System.IO;
using ATAS.Indicators;

namespace FabioOrderFlow;

public class FabioOrderFlow : Indicator
{
    private BalanceZoneTracker? _balanceTracker;
    private LondonMeanReversionModule? _meanReversionModule;
    private readonly Dictionary<int, CumulativeTradesRequest> _pendingCumulativeTradeRequests = new();
    private readonly List<CumulativeTrade> _historicalCumulativeTrades = new();
    private static readonly bool DetailedDebugLogs = false;
    private readonly object _logSync = new();
    private readonly string _logPath;
    
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

        Log("[INIT] FabioOrderFlow indicator created");
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

            _pendingCumulativeTradeRequests.Clear();
            _historicalCumulativeTrades.Clear();

            var requestStart = startTime;
            var requestIndex = 0;
            while (requestStart < endTime)
            {
                var requestEnd = requestStart.AddDays(7);
                if (requestEnd > endTime)
                    requestEnd = endTime;

                var request = new CumulativeTradesRequest(requestStart, requestEnd, 0, 0);
                _pendingCumulativeTradeRequests[request.RequestId] = request;
                requestIndex++;
                Log($"[CUM_TRADES_REQUEST] Index={requestIndex}, RequestId={request.RequestId}, BeginItaly={MarketTimeZones.ToItaly(requestStart):yyyy-MM-dd HH:mm:ss}, BeginUtc={requestStart:yyyy-MM-dd HH:mm:ss}, EndItaly={MarketTimeZones.ToItaly(requestEnd):yyyy-MM-dd HH:mm:ss}, EndUtc={requestEnd:yyyy-MM-dd HH:mm:ss}, CurrentBar={CurrentBar}");
                RequestForCumulativeTrades(request);

                if (requestEnd >= endTime)
                    break;

                requestStart = requestEnd.AddTicks(1);
            }

            Log($"[CUM_TRADES_REQUEST_BATCH] Requests={_pendingCumulativeTradeRequests.Count}, ChartBeginItaly={MarketTimeZones.ToItaly(startTime):yyyy-MM-dd HH:mm:ss}, ChartEndItaly={MarketTimeZones.ToItaly(endTime):yyyy-MM-dd HH:mm:ss}");
        }
        catch (Exception ex)
        {
            Log($"[CUM_TRADES_REQUEST_ERROR] {ex.Message}");
        }
    }

    protected override void OnCumulativeTradesResponse(CumulativeTradesRequest request, IEnumerable<CumulativeTrade> cumulativeTrades)
    {
        if (!_pendingCumulativeTradeRequests.Remove(request.RequestId))
            return;

        var trades = cumulativeTrades.ToList();
        _historicalCumulativeTrades.AddRange(trades);
        Log($"[CUM_TRADES_RESPONSE] Count={trades.Count}, RequestId={request.RequestId}, Pending={_pendingCumulativeTradeRequests.Count}, Accumulated={_historicalCumulativeTrades.Count}");

        if (_pendingCumulativeTradeRequests.Count > 0)
            return;

        var allTrades = _historicalCumulativeTrades
            .OrderBy(t => t.Time)
            .ToList();

        Log($"[CUM_TRADES_BATCH_COMPLETE] TotalCount={allTrades.Count}");
        _balanceTracker?.OnHistoricalCumulativeTrades(allTrades);
        
        if (_meanReversionModule != null && CurrentBar > 0)
            _meanReversionModule.ProcessHistoricalPositions(0, CurrentBar - 1);

        _historicalCumulativeTrades.Clear();
    }

    protected override void OnCumulativeTrade(CumulativeTrade trade)
    {
        _balanceTracker?.OnLiveCumulativeTrade(trade);
    }

    protected override void OnUpdateCumulativeTrade(CumulativeTrade trade)
    {
        _balanceTracker?.OnLiveCumulativeTrade(trade);
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

    private void Log(string message, bool isHistorical = false)
    {
        try
        {
            lock (_logSync)
            {
                string logMessage;
                if (isHistorical)
                {
                    // Historical logs: no processing timestamp, only event timestamp in fields
                    logMessage = message;
                }
                else
                {
                    // Live logs: include processing timestamp
                    var timestamp = MarketTimeZones.ToItaly(DateTime.UtcNow).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    logMessage = $"[Italy={timestamp}] {message}";
                }
                File.AppendAllText(_logPath, logMessage + Environment.NewLine);
            }
        }
        catch
        {
        }
    }

}
