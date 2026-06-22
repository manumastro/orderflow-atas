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
            
        Log("[INIT] FabioTrendFollowing indicator created");
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            Log($"[ONCALCULATE] Bar 0 - Initializing BalanceZoneTracker");
            Log($"[INSTRUMENT] Name: {InstrumentInfo?.Instrument}, TickSize: {InstrumentInfo?.TickSize}, Exchange: {InstrumentInfo?.Exchange}");
            _balanceTracker = new BalanceZoneTracker(this, Log, Rectangles, HorizontalLinesTillTouch, GetCandle);
            return;
        }

        var candle = GetCandle(bar);
        
        // Log periodico ogni 100 barre per verificare dati
        if (bar % 100 == 0)
        {
            Log($"[BAR_CHECK] Bar={bar}, Time={candle.Time:yyyy-MM-dd HH:mm:ss}, O={candle.Open}, H={candle.High}, L={candle.Low}, C={candle.Close}, V={candle.Volume}");
        }
        
        _balanceTracker?.OnBarUpdate(bar, candle);
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
