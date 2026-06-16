// ============================================================================
// FabioMeanReversion — Model 2: Mean Reversion (London Session)
// ============================================================================
// Fabio Valentino's model for ranging markets:
//   - Trade ONLY when market is IN BALANCE (consolidating)
//   - Entry: Breakout failure → trapped side → squeeze back into value
//   - Target: POC (Point of Control) of the balance area
//   - Stop: Behind the aggression cluster (tight)
//   - Session: London primarily, also summer months
//
// Pipeline:
//   Step 0: Session filter (London)
//   Step 1: Balance detection (compression + profile)
//   Step 2: Breakout detection
//   Step 3: Fakeout detection
//   Step 4: Trapped side
//   Step 5: Trigger (second drive + big trade)
//   Step 6: Management (target POC, stop, invalidation)
//
// All signals logged to Output Window — NO orders sent.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ATAS.Indicators;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    #region === Parameters ===

    [Display(Name = "Min Big Trade Size London (contracts)", GroupName = "Big Trades", Order = 10)]
    public int MinBigTradeSizeLondon { get; set; } = 20;

    [Display(Name = "Min Big Trade Size NY (contracts)", GroupName = "Big Trades", Order = 11)]
    public int MinBigTradeSizeNY { get; set; } = 30;

    [Display(Name = "Compression Lookback (bars)", GroupName = "Compression", Order = 20)]
    public int CompressionLookback { get; set; } = 100;

    [Display(Name = "Compression Range Ratio", GroupName = "Compression", Order = 21,
        Description = "Max range/impulse ratio to qualify as compression (e.g., 0.4 = 40%)")]
    public decimal CompressionRangeRatio { get; set; } = 0.40m;

    [Display(Name = "Min Compression Bars", GroupName = "Compression", Order = 22)]
    public int MinCompressionBars { get; set; } = 10;

    [Display(Name = "Value Area % (default 70%)", GroupName = "Profile", Order = 30)]
    public decimal ValueAreaPct { get; set; } = 70m;

    [Display(Name = "Enable Logging", GroupName = "Debug", Order = 90)]
    public bool EnableLogging { get; set; } = true;

    #endregion

    #region === DataSeries ===

    private ValueDataSeries _pocLine = null!;
    private ValueDataSeries _vahLine = null!;
    private ValueDataSeries _valLine = null!;
    private PaintbarsDataSeries _paintBars = null!;
    private ValueDataSeries _stateDisplay = null!;

    #endregion

    #region === Enums ===

    private enum BalanceState
    {
        NO_COMPRESSION,
        COMPRESSION_FORMING,
        BALANCE_READY,
        FIRST_BREAKOUT_WAIT,
        FAKEOUT_WATCH,
        TRAPPED_SELLERS_LONG_WATCH,
        TRAPPED_BUYERS_SHORT_WATCH,
        TRIGGER_LONG,
        TRIGGER_SHORT,
        OUT_OF_BALANCE,
        INVALIDATED
    }

    #endregion

    #region === State ===

    private BalanceState _state = BalanceState.NO_COMPRESSION;
    private int _compressionStartBar = -1;
    private decimal _poc = 0;
    private decimal _vah = 0;
    private decimal _val = 0;
    private decimal _impulseHigh = 0;
    private decimal _impulseLow = 0;
    private decimal _impulseRange = 0;
    private int _lastBarProcessed = -1;

    // Rolling profile data
    private readonly Dictionary<decimal, decimal> _priceVolume = new();

    // Logging
    private static readonly string LogDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs");
    private static readonly string LogPath = Path.Combine(LogDir, "FabioMeanReversion.log");
    private StreamWriter? _logWriter;

    #endregion

    #region === Constructor ===

    public FabioMeanReversion()
    {
        // POC line
        _pocLine = new ValueDataSeries("POC")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Yellow,
            Width = 2,
            ShowZeroValue = false
        };
        DataSeries.Add(_pocLine);

        // VAH line
        _vahLine = new ValueDataSeries("VAH")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Cyan,
            Width = 1,
            ShowZeroValue = false
        };
        DataSeries.Add(_vahLine);

        // VAL line
        _valLine = new ValueDataSeries("VAL")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Cyan,
            Width = 1,
            ShowZeroValue = false
        };
        DataSeries.Add(_valLine);

        // Paint bars
        _paintBars = new PaintbarsDataSeries("State Bars")
        {
            IsHidden = true
        };
        DataSeries.Add(_paintBars);

        // State display (hidden, for debug)
        _stateDisplay = new ValueDataSeries("State")
        {
            IsHidden = true
        };
        DataSeries.Add(_stateDisplay);

        InitLog();
    }

    #endregion

    #region === Core Calculation ===

    protected override void OnCalculate(int bar, decimal value)
    {
        // Only process new bars
        if (bar == _lastBarProcessed) return;
        _lastBarProcessed = bar;

        var candle = GetCandle(bar);

        // Step 0: Session filter
        if (!IsInLondonSession(candle))
        {
            SetState(BalanceState.NO_COMPRESSION, bar, "Fuori sessione London");
            return;
        }

        // Step 1: Find compression
        FindCompression(bar, candle);

        // Step 2+: Pipeline (future implementation)
        // TODO: Breakout, Fakeout, Trapped, Trigger, Management
    }

    #endregion

    #region === Step 0: Session Filter ===

    private bool IsInLondonSession(IndicatorCandle candle)
    {
        var time = candle.LastTime.TimeOfDay;
        // London: 08:00 - 16:30 UTC (typical futures session)
        return time >= new TimeSpan(8, 0, 0) && time <= new TimeSpan(16, 30, 0);
    }

    private bool IsInNySession(IndicatorCandle candle)
    {
        var time = candle.LastTime.TimeOfDay;
        // NY: 14:30 - 21:00 UTC
        return time >= new TimeSpan(14, 30, 0) && time <= new TimeSpan(21, 0, 0);
    }

    private int GetMinBigTradeSize(IndicatorCandle candle)
    {
        return IsInNySession(candle) ? MinBigTradeSizeNY : MinBigTradeSizeLondon;
    }

    #endregion

    #region === Step 1: Compression Detection ===

    private void FindCompression(int bar, IndicatorCandle candle)
    {
        // Need enough bars
        if (bar < CompressionLookback)
        {
            SetState(BalanceState.NO_COMPRESSION, bar, "Bar insufficienti");
            return;
        }

        // Find the last impulse (directional move with range expansion)
        int impulseEnd = FindLastImpulse(bar);
        if (impulseEnd < 0)
        {
            SetState(BalanceState.NO_COMPRESSION, bar, "Nessun impulse trovato");
            return;
        }

        // The compression starts after the impulse
        int compressionCandidateStart = impulseEnd + 1;
        int compressionBars = bar - compressionCandidateStart + 1;

        // Need minimum bars for compression
        if (compressionBars < MinCompressionBars)
        {
            SetState(BalanceState.COMPRESSION_FORMING, bar,
                $"Compressione in formazione ({compressionBars} bar < {MinCompressionBars})");
            return;
        }

        // Calculate the impulse range for comparison
        var impulseCandle = GetCandle(impulseEnd);
        decimal impulseHigh = impulseCandle.High;
        decimal impulseLow = impulseCandle.Low;

        // Find actual impulse range by scanning the impulse leg
        for (int i = Math.Max(0, impulseEnd - 20); i <= impulseEnd; i++)
        {
            var c = GetCandle(i);
            if (c.High > impulseHigh) impulseHigh = c.High;
            if (c.Low < impulseLow) impulseLow = c.Low;
        }
        _impulseRange = impulseHigh - impulseLow;

        if (_impulseRange <= 0)
        {
            SetState(BalanceState.NO_COMPRESSION, bar, "Impulse range zero");
            return;
        }

        // Calculate compression range
        decimal compressionHigh = decimal.MinValue;
        decimal compressionLow = decimal.MaxValue;
        decimal totalVolume = 0;

        for (int i = compressionCandidateStart; i <= bar; i++)
        {
            var c = GetCandle(i);
            if (c.High > compressionHigh) compressionHigh = c.High;
            if (c.Low < compressionLow) compressionLow = c.Low;
            totalVolume += c.Volume;
        }

        decimal compressionRange = compressionHigh - compressionLow;
        decimal rangeRatio = compressionRange / _impulseRange;

        // Check if range is compressed enough
        if (rangeRatio > CompressionRangeRatio)
        {
            SetState(BalanceState.NO_COMPRESSION, bar,
                $"Range non compresso: ratio={rangeRatio:P1} > {CompressionRangeRatio:P0}");
            return;
        }

        // Build profile on compression range
        if (!BuildProfile(compressionCandidateStart, bar, compressionLow, compressionHigh))
        {
            SetState(BalanceState.COMPRESSION_FORMING, bar, "Profile non valido");
            return;
        }

        // Check if profile is protective (VAH/VAL hold)
        bool isProtective = IsProfileProtective(bar, candle);

        if (isProtective)
        {
            _compressionStartBar = compressionCandidateStart;
            _impulseHigh = impulseHigh;
            _impulseLow = impulseLow;
            SetState(BalanceState.BALANCE_READY, bar,
                $"Balance pronto | POC={_poc:F2} VAH={_vah:F2} VAL={_val:F2} | Range ratio={rangeRatio:P1}");

            // Draw levels
            DrawLevels(bar);
        }
        else
        {
            SetState(BalanceState.COMPRESSION_FORMING, bar,
                $"Profile protettivo: NO | POC={_poc:F2} VAH={_vah:F2} VAL={_val:F2}");
        }
    }

    /// <summary>
    /// Find the last impulse leg (directional move with range/volume expansion)
    /// </summary>
    private int FindLastImpulse(int currentBar)
    {
        // Scan backwards to find the last directional move
        // Look for a sequence where range expands and price moves with momentum

        decimal bestImpulseScore = 0;
        int bestImpulseEnd = -1;

        // Look at recent bars
        int lookback = Math.Min(CompressionLookback, currentBar);

        for (int i = currentBar - MinCompressionBars; i >= currentBar - lookback; i--)
        {
            if (i < 1) break;

            var candle = GetCandle(i);
            var prevCandle = GetCandle(i - 1);

            // Calculate impulse score based on:
            // 1. Range expansion
            // 2. Directional close (close near high/low)
            // 3. Volume/delta

            decimal range = candle.High - candle.Low;
            if (range <= 0) continue;

            decimal bodySize = Math.Abs(candle.Close - candle.Open);
            decimal bodyRatio = bodySize / range; // How much of the range is body (directional)

            // Close position in range (0 = low, 1 = high)
            decimal closePosition = (candle.Close - candle.Low) / range;

            // Delta significance
            decimal deltaAbs = Math.Abs(candle.Delta);

            // Score: high body ratio + extreme close position + significant delta
            decimal score = bodyRatio * 0.4m + Math.Abs(closePosition - 0.5m) * 0.4m +
                           (deltaAbs > 100 ? 0.2m : deltaAbs / 500m * 0.2m);

            // Check if this is a continuation of a move (previous bars also directional)
            if (i >= 2)
            {
                var prevRange = prevCandle.High - prevCandle.Low;
                if (prevRange > 0 && range > prevRange * 1.2m) // Range expanding
                {
                    score *= 1.2m;
                }
            }

            if (score > bestImpulseScore)
            {
                bestImpulseScore = score;
                bestImpulseEnd = i;
            }
        }

        // Require minimum score
        if (bestImpulseScore < 0.3m) return -1;

        return bestImpulseEnd;
    }

    /// <summary>
    /// Build volume profile on the compression range
    /// </summary>
    private bool BuildProfile(int startBar, int endBar, decimal rangeLow, decimal rangeHigh)
    {
        _priceVolume.Clear();

        if (startBar > endBar || startBar < 0) return false;

        // Accumulate volume per price level
        for (int i = startBar; i <= endBar; i++)
        {
            var candle = GetCandle(i);
            var levels = candle.GetAllPriceLevels();
            if (levels == null) continue;

            foreach (var level in levels)
            {
                decimal price = level.Price;
                if (!_priceVolume.ContainsKey(price))
                    _priceVolume[price] = 0;
                _priceVolume[price] += level.Volume;
            }
        }

        if (_priceVolume.Count == 0) return false;

        // Find POC (price with maximum volume)
        _poc = _priceVolume.OrderByDescending(kv => kv.Value).First().Key;

        // Calculate Value Area (70% of volume)
        if (!CalculateValueArea(out _vah, out _val))
            return false;

        return true;
    }

    /// <summary>
    /// Calculate Value Area High/Low containing ValueAreaPct% of volume
    /// </summary>
    private bool CalculateValueArea(out decimal vah, out decimal val)
    {
        vah = 0;
        val = 0;

        if (_priceVolume.Count == 0) return false;

        decimal totalVolume = _priceVolume.Values.Sum();
        decimal targetVolume = totalVolume * (ValueAreaPct / 100m);

        // Start from POC and expand up/down
        var sortedPrices = _priceVolume.Keys.OrderBy(p => p).ToList();
        int pocIndex = sortedPrices.IndexOf(_poc);

        if (pocIndex < 0) return false;

        decimal accumulatedVolume = _priceVolume[_poc];
        int lowIdx = pocIndex;
        int highIdx = pocIndex;

        while (accumulatedVolume < targetVolume)
        {
            // Check which direction has more volume
            decimal volBelow = (lowIdx > 0) ? _priceVolume[sortedPrices[lowIdx - 1]] : 0;
            decimal volAbove = (highIdx < sortedPrices.Count - 1) ? _priceVolume[sortedPrices[highIdx + 1]] : 0;

            if (volBelow == 0 && volAbove == 0) break;

            if (volAbove >= volBelow)
            {
                highIdx++;
                accumulatedVolume += volAbove;
            }
            else
            {
                lowIdx--;
                accumulatedVolume += volBelow;
            }
        }

        val = sortedPrices[lowIdx];
        vah = sortedPrices[highIdx];

        return true;
    }

    /// <summary>
    /// Check if the profile is protective (VAH/VAL hold against price)
    /// </summary>
    private bool IsProfileProtective(int currentBar, IndicatorCandle currentCandle)
    {
        if (_vah == 0 || _val == 0) return false;

        // Check recent bars: price should be oscillating within VAH/VAL
        int checkBars = Math.Min(5, currentBar);
        int breaksAbove = 0;
        int breaksBelow = 0;

        for (int i = currentBar - checkBars + 1; i <= currentBar; i++)
        {
            var c = GetCandle(i);

            // Close should be mostly within value area
            if (c.Close > _vah) breaksAbove++;
            if (c.Close < _val) breaksBelow++;
        }

        // Protective if most closes are within value area
        return breaksAbove <= 1 && breaksBelow <= 1;
    }

    #endregion

    #region === Drawing ===

    private void DrawLevels(int bar)
    {
        // Draw POC
        _pocLine[bar] = _poc;

        // Draw VAH
        _vahLine[bar] = _vah;

        // Draw VAL
        _valLine[bar] = _val;

        // Color bar based on state
        _paintBars[bar] = _state switch
        {
            BalanceState.BALANCE_READY => System.Windows.Media.Colors.LimeGreen,
            BalanceState.COMPRESSION_FORMING => System.Windows.Media.Colors.Yellow,
            _ => System.Windows.Media.Colors.Transparent
        };
    }

    #endregion

    #region === State Machine ===

    private void SetState(BalanceState newState, int bar, string reason)
    {
        if (_state != newState)
        {
            _state = newState;
            _stateDisplay[bar] = (decimal)newState;
            if (EnableLogging)
                LogSignal(bar, $"STATE → {newState}", reason);
        }
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
