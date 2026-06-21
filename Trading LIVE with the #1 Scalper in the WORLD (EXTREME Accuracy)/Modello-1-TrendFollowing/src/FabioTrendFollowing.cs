using System;
using ATAS.Indicators;

namespace FabioTrendFollowing;

public class FabioTrendFollowing : Indicator
{
    private BalanceZoneTracker? _balanceTracker;

    public FabioTrendFollowing()
    {
        Name = "Fabio Trend Following";
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            _balanceTracker = new BalanceZoneTracker(this);
            return;
        }

        var candle = GetCandle(bar);
        _balanceTracker?.OnBarUpdate(bar, candle);

        // Phase 1: solo BalanceZoneTracker attivo
        // Debug stato per validazione
        if (_balanceTracker != null && _balanceTracker.IsOutOfBalance && bar == CurrentBar - 1)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[Bar {bar}] OUT_OF_BALANCE | Direction={_balanceTracker.Direction} | " +
                $"POC={_balanceTracker.POC} | VAH={_balanceTracker.VAH} | VAL={_balanceTracker.VAL}");
        }
    }
}
