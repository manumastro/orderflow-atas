using ATAS.Indicators;

namespace FabioTrendFollowing;

public class FabioTrendFollowing : Indicator
{
    public FabioTrendFollowing()
    {
        Name = "Fabio Trend Following";
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        // Intentionally empty.
        // The previous prototype has been removed so the indicator can be rebuilt
        // from the documented target logic in MODELLO-1-DOCUMENTAZIONE.md.
    }
}
