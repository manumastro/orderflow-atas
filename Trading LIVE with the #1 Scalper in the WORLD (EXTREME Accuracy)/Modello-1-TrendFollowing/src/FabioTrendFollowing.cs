using System;
using System.IO;
using ATAS.Indicators;

namespace FabioTrendFollowing;

public class FabioTrendFollowing : Indicator
{
    private BalanceZoneTracker? _balanceTracker;
    private readonly string _mainLogPath;
    private readonly string _verboseLogPath;

    public FabioTrendFollowing()
    {
        Name = "Fabio Trend Following";
        
        var logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ATAS", "Logs");

        _mainLogPath = Path.Combine(logDirectory, $"FabioTrendFollowing_{DateTime.Now:yyyy-MM-dd}.log");
        _verboseLogPath = Path.Combine(logDirectory, $"FabioTrendFollowing_verbose_{DateTime.Now:yyyy-MM-dd}.log");
        
        // Cancella log precedente all'inizializzazione
        try
        {
            if (File.Exists(_mainLogPath))
            {
                File.Delete(_mainLogPath);
            }

            if (File.Exists(_verboseLogPath))
            {
                File.Delete(_verboseLogPath);
            }
        }
        catch
        {
            // Ignore delete errors
        }
            
        LogMain("[INIT] FabioTrendFollowing indicator created");
        LogMain($"[LOGS] Main={_mainLogPath}");
        LogMain($"[LOGS] Verbose={_verboseLogPath}");
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            LogMain($"[ONCALCULATE] Bar 0 - Initializing BalanceZoneTracker");
            LogMain($"[INSTRUMENT] Name: {InstrumentInfo?.Instrument}, TickSize: {InstrumentInfo?.TickSize}, Exchange: {InstrumentInfo?.Exchange}, InstrumentTimeZone={InstrumentInfo?.TimeZone}");
            LogMain($"[CHART] CurrentBar={CurrentBar}, ChartType={ChartInfo?.ChartType}");
            LogChartTradingSessions();
            _balanceTracker = new BalanceZoneTracker(this, LogMain, LogVerbose, Rectangles, HorizontalLinesTillTouch, GetCandle);
            return;
        }

        var candle = GetCandle(bar);
        
        // Log periodico ogni 50 barre per verificare dati
        if (bar % 50 == 0)
        {
            LogVerbose($"[BAR_CHECK] Bar={bar}, Time={candle.Time:yyyy-MM-dd HH:mm:ss}, O={candle.Open:F2}, H={candle.High:F2}, L={candle.Low:F2}, C={candle.Close:F2}, V={candle.Volume:F0}");
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
                LogMain("[CHART_SESSIONS] TradingSessionDescriptions=null");
                return;
            }

            var count = 0;
            foreach (var session in sessions)
            {
                count++;
                LogMain($"[CHART_SESSIONS] #{count}: {session}");
            }

            if (count == 0)
            {
                LogMain("[CHART_SESSIONS] No trading session descriptions exposed by chart");
            }
        }
        catch (Exception ex)
        {
            LogMain($"[CHART_SESSIONS] Error reading trading sessions: {ex.Message}");
        }
    }

    private void LogMain(string message)
    {
        LogToFile(_mainLogPath, message);
    }

    private void LogVerbose(string message)
    {
        LogToFile(_verboseLogPath, message);
    }

    private static void LogToFile(string path, string message)
    {
        try
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var logMessage = $"[{timestamp}] {message}";
            File.AppendAllText(path, logMessage + Environment.NewLine);
        }
        catch
        {
            // Ignore log errors
        }
    }
}
