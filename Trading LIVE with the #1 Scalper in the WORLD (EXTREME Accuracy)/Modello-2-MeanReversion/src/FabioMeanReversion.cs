// ============================================================================
// FabioMeanReversion — Model 2: Mean Reversion (London Session)
// ============================================================================
// Fabio Valentino's model for range-bound markets:
//   - Trade ONLY when market is IN BALANCE (consolidation)
//   - Entry: Wait for breakout FAIL (not first — second drive)
//   - Target: POC of the balance area
//   - Stop: Above/below the failed breakout
//   - Session: London (09:00 - 17:30 Italian time), best in summer
//
// Signals:
//   1. FAILED AUCTION   — Breakout that fails (wick rejection)
//   2. SQUEEZE SETUP    — Trapped traders forced to close
//   3. ABSORPTION       — Big orders absorbed without follow-through
//   4. CVD DIVERGENCE   — Price vs Cumulative Volume Delta mismatch
//
// All signals logged to Output Window — NO orders sent.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.IO;
using ATAS.Indicators;

namespace FabioMeanReversion;

public class FabioMeanReversion : Indicator
{
    #region === Parameters ===

    [Display(Name = "Wick Ratio for Failed Auction", Order = 10)]
    public decimal WickRatioThreshold { get; set; } = 0.6m;

    [Display(Name = "Min Big Trade Size (contracts)", Order = 20)]
    public int MinBigTradeSize { get; set; } = 20; // Lower for London

    [Display(Name = "Absorption Min Delta", Order = 30)]
    public decimal AbsorptionMinDelta { get; set; } = 300m; // Lower for London

    [Display(Name = "Squeeze Lookback Bars", Order = 40)]
    public int SqueezeLookback { get; set; } = 5;

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
    private static readonly string LogPath = Path.Combine(LogDir, "FabioMeanReversion.log");
    private StreamWriter? _logWriter;

    #endregion

    #region === Constructor ===

    public FabioMeanReversion()
    {
        _buySignals = new ValueDataSeries("Buy Signal")
        {
            VisualType = VisualMode.UpArrow,
            Color = System.Windows.Media.Colors.CornflowerBlue,
            ShowZeroValue = false
        };
        DataSeries.Add(_buySignals);

        _sellSignals = new ValueDataSeries("Sell Signal")
        {
            VisualType = VisualMode.DownArrow,
            Color = System.Windows.Media.Colors.OrangeRed,
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

        // London Session filter: 09:00 - 17:30 Italian time (08:00 - 16:30 UTC)
        if (!IsInLondonSession(candle))
            return;

        // Run detectors
        DetectFailedAuction(bar, candle);
        DetectSqueezeSetup(bar, candle);
        DetectAbsorption(bar, candle);
        DetectCvdDivergence(bar, candle);
    }

    #endregion

    #region === Signal 1: Failed Auction ===
    // Price breaks a level but closes back inside = trapped traders

    private void DetectFailedAuction(int bar, IndicatorCandle candle)
    {
        if (bar < 2) return;

        var prev = GetCandle(bar - 1);
        var fullRange = candle.High - candle.Low;
        if (fullRange == 0) return;

        // ─── Bullish Failed Auction (sellers failed to break low) ───
        var lowerWick = Math.Min(candle.Open, candle.Close) - candle.Low;
        if (lowerWick / fullRange >= WickRatioThreshold && candle.Close > candle.Open)
        {
            if (candle.Low < prev.Low) // Made new low then reversed
            {
                _buySignals[bar] = candle.Low - InstrumentInfo.TickSize * 3;
                _paintBars[bar] = System.Windows.Media.Colors.CornflowerBlue;
                if (EnableLogging)
                    LogSignal(bar, "FAILED_AUCTION_BULL",
                        $"Wick={lowerWick / fullRange:P0} | New low rejected");
            }
        }

        // ─── Bearish Failed Auction (buyers failed to break high) ───
        var upperWick = candle.High - Math.Max(candle.Open, candle.Close);
        if (upperWick / fullRange >= WickRatioThreshold && candle.Close < candle.Open)
        {
            if (candle.High > prev.High) // Made new high then reversed
            {
                _sellSignals[bar] = candle.High + InstrumentInfo.TickSize * 3;
                _paintBars[bar] = System.Windows.Media.Colors.OrangeRed;
                if (EnableLogging)
                    LogSignal(bar, "FAILED_AUCTION_BEAR",
                        $"Wick={upperWick / fullRange:P0} | New high rejected");
            }
        }
    }

    #endregion

    #region === Signal 2: Squeeze Setup ===
    // Cluster of one-sided aggression → reversal = trapped traders squeezed

    private void DetectSqueezeSetup(int bar, IndicatorCandle candle)
    {
        if (bar < SqueezeLookback) return;

        var recentDeltas = new List<decimal>();
        for (int i = Math.Max(0, bar - SqueezeLookback); i < bar; i++)
            recentDeltas.Add(GetCandle(i).Delta);

        var sumRecentDelta = recentDeltas.Sum();

        // ─── Bullish Squeeze (sellers trapped) ───
        if (sumRecentDelta < -AbsorptionMinDelta * 3 &&
            candle.Delta > 0 && candle.Close > candle.Open)
        {
            _buySignals[bar] = candle.Low - InstrumentInfo.TickSize * 2;
            _paintBars[bar] = System.Windows.Media.Colors.CornflowerBlue;
            if (EnableLogging)
                LogSignal(bar, "SQUEEZE_BUY",
                    $"Trapped sellers! Δsum={sumRecentDelta:F0} → reversal");
        }

        // ─── Bearish Squeeze (buyers trapped) ───
        if (sumRecentDelta > AbsorptionMinDelta * 3 &&
            candle.Delta < 0 && candle.Close < candle.Open)
        {
            _sellSignals[bar] = candle.High + InstrumentInfo.TickSize * 2;
            _paintBars[bar] = System.Windows.Media.Colors.OrangeRed;
            if (EnableLogging)
                LogSignal(bar, "SQUEEZE_SELL",
                    $"Trapped buyers! Δsum={sumRecentDelta:F0} → reversal");
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

    private bool IsInLondonSession(IndicatorCandle candle)
    {
        var time = candle.LastTime.TimeOfDay;
        // London: 08:00-16:30 GMT = 08:00-16:30 UTC
        return time >= new TimeSpan(8, 0, 0) && time <= new TimeSpan(16, 30, 0);
    }

    #endregion

    #region === Logging ===

    private void InitLog()
    {
        try
        {
            Directory.CreateDirectory(LogDir);
            _logWriter = new StreamWriter(LogPath, append: true) { AutoFlush = true };
            _logWriter.WriteLine($"\n=== FabioMeanReversion started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
        }
        catch { /* ignore log errors */ }
    }

    private void LogSignal(int bar, string signalType, string details)
    {
        var candle = GetCandle(bar);
        var timestamp = candle.LastTime.ToString("HH:mm:ss");
        var msg = $"[MEAN_REV] Bar={bar} | {timestamp} | {signalType} | {details}";

        System.Diagnostics.Debug.WriteLine(msg);

        try { _logWriter?.WriteLine(msg); } catch { /* ignore */ }
    }

    #endregion
}
