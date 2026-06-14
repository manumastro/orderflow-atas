// ============================================================================
// FabioTrendFollowing — Model 1: Trend Following (New York Session)
// ============================================================================
// Fabio Valentino's model for trending markets:
//   - Trade ONLY when market is OUT OF BALANCE (trending)
//   - Entry: Big trade aggression at Low Volume Nodes
//   - Target: POC of previous balance area
//   - Stop: Behind the aggression cluster (tight)
//   - Session: New York only (15:30 - 22:00 Italian time)
//
// Signals:
//   1. AGGRESSION       — Big trades ≥ threshold (core trigger)
//   2. LOW VOLUME NODE  — Areas with little volume = fast reaction zones
//   3. ABSORPTION       — Big orders absorbed without follow-through
//   4. CVD DIVERGENCE   — Price vs Cumulative Volume Delta mismatch
//
// All signals logged to Output Window — NO orders sent.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.IO;
using ATAS.Indicators;

namespace FabioTrendFollowing;

public class FabioTrendFollowing : Indicator
{
    #region === Parameters ===

    [Display(Name = "Min Big Trade Size (contracts)", Order = 10)]
    public int MinBigTradeSize { get; set; } = 30;

    [Display(Name = "Profile Lookback Bars", Order = 20)]
    public int ProfileLookback { get; set; } = 60;

    [Display(Name = "Low Volume Node Threshold %", Order = 25)]
    public decimal LowVolumeNodePct { get; set; } = 30m;

    [Display(Name = "Absorption Min Delta", Order = 40)]
    public decimal AbsorptionMinDelta { get; set; } = 500m;

    [Display(Name = "CVD Lookback Bars", Order = 50)]
    public int CvdLookback { get; set; } = 20;

    [Display(Name = "Enable Debug Logging", Order = 70)]
    public bool EnableLogging { get; set; } = true;

    #endregion

    #region === DataSeries ===

    private ValueDataSeries _buySignals = null!;
    private ValueDataSeries _sellSignals = null!;
    private ValueDataSeries _cvdLine = null!;
    private ValueDataSeries _deltaHistogram = null!;
    private PaintbarsDataSeries _paintBars = null!;

    #endregion

    #region === State ===

    private decimal _cumulativeDelta;
    private readonly List<decimal> _cvdHistory = new();
    private static readonly string LogDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ATAS", "Logs");
    private static readonly string LogPath = Path.Combine(LogDir, "FabioTrendFollowing.log");
    private StreamWriter? _logWriter;

    #endregion

    #region === Constructor ===

    public FabioTrendFollowing()
    {
        _buySignals = new ValueDataSeries("Buy Signal")
        {
            VisualType = VisualMode.UpArrow,
            Color = System.Windows.Media.Colors.LimeGreen,
            ShowZeroValue = false
        };
        DataSeries.Add(_buySignals);

        _sellSignals = new ValueDataSeries("Sell Signal")
        {
            VisualType = VisualMode.DownArrow,
            Color = System.Windows.Media.Colors.Red,
            ShowZeroValue = false
        };
        DataSeries.Add(_sellSignals);

        _cvdLine = new ValueDataSeries("CVD")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Cyan,
            IsHidden = true
        };
        DataSeries.Add(_cvdLine);

        _deltaHistogram = new ValueDataSeries("Delta")
        {
            VisualType = VisualMode.Histogram,
            Color = System.Windows.Media.Colors.Orange,
            IsHidden = true
        };
        DataSeries.Add(_deltaHistogram);

        _paintBars = new PaintbarsDataSeries("Signal Bars")
        {
            IsHidden = true
        };
        DataSeries.Add(_paintBars);

        InitLog();
    }

    #endregion

    #region === Core Calculation ===

    protected override void OnCalculate(int bar, decimal value)
    {
        var candle = GetCandle(bar);

        // Update CVD
        _cumulativeDelta += candle.Delta;
        _cvdLine[bar] = _cumulativeDelta;
        _deltaHistogram[bar] = candle.Delta;

        if (_cvdHistory.Count <= bar)
            _cvdHistory.Add(_cumulativeDelta);
        else
            _cvdHistory[bar] = _cumulativeDelta;

        // Only latest bar
        if (bar < CurrentBar - 1)
            return;

        // NY Session filter: 15:30 - 22:00 Italian time (14:30 - 21:00 UTC)
        if (!IsInNySession(candle))
            return;

        // Run detectors
        DetectAggression(bar, candle);
        DetectLowVolumeNode(bar, candle);
        DetectAbsorption(bar, candle);
        DetectCvdDivergence(bar, candle);
    }

    #endregion

    #region === Signal 1: Aggression (Core Trigger) ===

    private void DetectAggression(int bar, IndicatorCandle candle)
    {
        var bigBuyVolume = 0m;
        var bigSellVolume = 0m;

        var levels = candle.GetAllPriceLevels();
        if (levels == null) return;

        foreach (var level in levels)
        {
            var netDelta = level.Ask - level.Bid;
            if (netDelta > MinBigTradeSize)
                bigBuyVolume += netDelta;
            if (netDelta < -MinBigTradeSize)
                bigSellVolume += Math.Abs(netDelta);
        }

        if (bigBuyVolume >= MinBigTradeSize)
        {
            _buySignals[bar] = candle.Low - InstrumentInfo.TickSize * 2;
            _paintBars[bar] = System.Windows.Media.Colors.LimeGreen;
            if (EnableLogging)
                LogSignal(bar, "BUY_AGGRESSION",
                    $"Big buys={bigBuyVolume:F0} | Delta={candle.Delta:F0} | Vol={candle.Volume:F0}");
        }

        if (bigSellVolume >= MinBigTradeSize)
        {
            _sellSignals[bar] = candle.High + InstrumentInfo.TickSize * 2;
            _paintBars[bar] = System.Windows.Media.Colors.Red;
            if (EnableLogging)
                LogSignal(bar, "SELL_AGGRESSION",
                    $"Big sells={bigSellVolume:F0} | Delta={candle.Delta:F0} | Vol={candle.Volume:F0}");
        }
    }

    #endregion

    #region === Signal 2: Low Volume Node ===

    private void DetectLowVolumeNode(int bar, IndicatorCandle candle)
    {
        if (bar < ProfileLookback) return;

        var priceVolume = new Dictionary<decimal, decimal>();

        for (int i = bar - ProfileLookback; i <= bar; i++)
        {
            var c = GetCandle(i);
            var allLevels = c.GetAllPriceLevels();
            if (allLevels == null) continue;

            foreach (var level in allLevels)
            {
                var price = level.Price;
                if (!priceVolume.ContainsKey(price))
                    priceVolume[price] = 0;
                priceVolume[price] += level.Volume;
            }
        }

        if (priceVolume.Count == 0) return;

        var avgVolume = priceVolume.Values.Average();
        var currentPrice = candle.Close;
        var currentLevelVol = priceVolume
            .Where(kv => Math.Abs(kv.Key - currentPrice) <= InstrumentInfo.TickSize * 3)
            .Select(kv => kv.Value)
            .DefaultIfEmpty(0)
            .Average();

        var threshold = avgVolume * (LowVolumeNodePct / 100m);

        if (currentLevelVol < threshold && currentLevelVol > 0)
        {
            if (EnableLogging)
                LogSignal(bar, "LOW_VOLUME_NODE",
                    $"Vol={currentLevelVol:F0} | Avg={avgVolume:F0} | Threshold={threshold:F0}");
        }
    }

    #endregion

    #region === Signal 3: Absorption ===

    private void DetectAbsorption(int bar, IndicatorCandle candle)
    {
        if (bar < 1) return;

        var prev = GetCandle(bar - 1);

        // Big sells but price holds = buyers absorbing
        if (candle.Delta < -AbsorptionMinDelta && candle.Close >= prev.Close)
        {
            _buySignals[bar] = candle.Low - InstrumentInfo.TickSize * 2;
            if (EnableLogging)
                LogSignal(bar, "ABSORPTION_BUY",
                    $"Delta={candle.Delta:F0} (sells) but price held");
        }

        // Big buys but price rejects = sellers absorbing
        if (candle.Delta > AbsorptionMinDelta && candle.Close <= prev.Close)
        {
            _sellSignals[bar] = candle.High + InstrumentInfo.TickSize * 2;
            if (EnableLogging)
                LogSignal(bar, "ABSORPTION_SELL",
                    $"Delta={candle.Delta:F0} (buys) but price rejected");
        }
    }

    #endregion

    #region === Signal 4: CVD Divergence ===

    private void DetectCvdDivergence(int bar, IndicatorCandle candle)
    {
        if (bar < CvdLookback) return;

        var lookbackBar = bar - CvdLookback;
        var priceChange = candle.Close - GetCandle(lookbackBar).Close;
        var cvdChange = _cumulativeDelta - (_cvdHistory.Count > lookbackBar ? _cvdHistory[lookbackBar] : 0);

        if (priceChange > 0 && cvdChange < 0)
        {
            if (EnableLogging)
                LogSignal(bar, "CVD_BEAR_DIV",
                    $"Price ↑{priceChange:F2} but CVD ↓{cvdChange:F0}");
        }

        if (priceChange < 0 && cvdChange > 0)
        {
            if (EnableLogging)
                LogSignal(bar, "CVD_BULL_DIV",
                    $"Price ↓{priceChange:F2} but CVD ↑{cvdChange:F0}");
        }
    }

    #endregion

    #region === Session Filter ===

    private bool IsInNySession(IndicatorCandle candle)
    {
        var time = candle.LastTime.TimeOfDay;
        // NY: 9:30-16:00 ET = 14:30-21:00 UTC
        return time >= new TimeSpan(14, 30, 0) && time <= new TimeSpan(21, 0, 0);
    }

    #endregion

    #region === Logging ===

    private void InitLog()
    {
        try
        {
            Directory.CreateDirectory(LogDir);
            _logWriter = new StreamWriter(LogPath, append: true) { AutoFlush = true };
            _logWriter.WriteLine($"\n=== FabioTrendFollowing started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
        }
        catch { /* ignore log errors */ }
    }

    private void LogSignal(int bar, string signalType, string details)
    {
        var candle = GetCandle(bar);
        var timestamp = candle.LastTime.ToString("HH:mm:ss");
        var msg = $"[TREND] Bar={bar} | {timestamp} | {signalType} | {details}";

        System.Diagnostics.Debug.WriteLine(msg);

        try { _logWriter?.WriteLine(msg); } catch { /* ignore */ }
    }

    #endregion
}
