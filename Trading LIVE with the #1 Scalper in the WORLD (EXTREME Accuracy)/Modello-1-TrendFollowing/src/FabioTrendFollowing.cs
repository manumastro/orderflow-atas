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
            _balanceTracker = new BalanceZoneTracker(this, Log);
            return;
        }

        var candle = GetCandle(bar);
        _balanceTracker?.OnBarUpdate(bar, candle);

        // Phase 1: solo BalanceZoneTracker attivo
        if (_balanceTracker != null && _balanceTracker.IsOutOfBalance && bar == CurrentBar - 1)
        {
            Log($"[Bar {bar}] OUT_OF_BALANCE | Direction={_balanceTracker.Direction} | " +
                $"POC={_balanceTracker.POC} | VAH={_balanceTracker.VAH} | VAL={_balanceTracker.VAL}");
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
