using System;
using System.IO;
using ATAS.Indicators;

namespace FabioTrendFollowing;

public class FabioTrendFollowing : Indicator
{
    private BalanceZoneTracker? _balanceTracker;
    private readonly string _logPath;

    public FabioTrendFollowing()
    {
        Name = "Fabio Trend Following";
        
        _logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs", $"FabioTrendFollowing_{DateTime.Now:yyyy-MM-dd}.log");
        
        // Cancella log precedente all'inizializzazione
        try
        {
            if (File.Exists(_logPath))
            {
                File.Delete(_logPath);
            }
        }
        catch
        {
            // Ignore delete errors
        }
            
        Log("[INIT] FabioTrendFollowing indicator created");
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
            return;
        }

        var candle = GetCandle(bar);
        
        // Log periodico ogni 50 barre per verificare dati
        if (bar % 50 == 0)
        {
            Log($"[BAR_CHECK] Bar={bar}, Time={candle.Time:yyyy-MM-dd HH:mm:ss}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}");
        }
        
        _balanceTracker?.OnBarUpdate(bar, candle);
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
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var logMessage = $"[{timestamp}] {message}";
            File.AppendAllText(_logPath, logMessage + Environment.NewLine);
        }
        catch
        {
            // Ignore log errors
        }
    }
}
