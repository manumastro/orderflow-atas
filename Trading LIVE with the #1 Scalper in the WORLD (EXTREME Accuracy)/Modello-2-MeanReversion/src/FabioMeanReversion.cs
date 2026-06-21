using System.ComponentModel;
using ATAS.Indicators;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    public FabioMeanReversion() : base(true)
    {
        DenyToChangePanel = true;
        DataSeries[0].IsHidden = true;
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        // Placeholder — mean reversion approach kept for discretionary use only
    }
}
