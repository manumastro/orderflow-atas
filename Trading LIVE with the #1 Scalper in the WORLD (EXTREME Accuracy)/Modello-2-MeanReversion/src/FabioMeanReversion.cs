// ============================================================================
// FabioMeanReversion — Balance Zone Detection (SIMPLIFIED FROM SCRATCH)
// ============================================================================
// Solo individuazione della zona di balance / compressione + profile (POC/VAH/VAL)
// Niente sessioni, niente stati complessi, niente fallback, niente follow-through, niente entry.
// 
// Visuals semplici:
//   - Linee orizzontali POC (giallo), VAH/VAL (cyan) che coprono ESATTAMENTE le barre della zona compressa
//   - Paintbars per evidenziare le barre della zona (oro)
//   - Marker (quadrato magenta) all'inizio della zona di balance
//
// Iniziamo dall'inizio: trova compressione dopo impulse, profile solo su quella area.
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

    [Display(Name = "Compression Lookback (bars)", GroupName = "Balance Zone", Order = 10)]
    public int CompressionLookback { get; set; } = 150;

    [Display(Name = "Compression Range Ratio", GroupName = "Balance Zone", Order = 11,
        Description = "Max range of compression / range of previous impulse leg")]
    public decimal CompressionRangeRatio { get; set; } = 0.45m;

    [Display(Name = "Min Compression Bars", GroupName = "Balance Zone", Order = 12)]
    public int MinCompressionBars { get; set; } = 8;

    [Display(Name = "Impulse Min Score", GroupName = "Balance Zone", Order = 13,
        Description = "Min score (0-1) to consider a bar as impulse. Lower = more impulse bars detected")]
    public decimal ImpulseMinScore { get; set; } = 0.30m;

    [Display(Name = "Value Area %", GroupName = "Profile", Order = 20)]
    public decimal ValueAreaPct { get; set; } = 70m;

    #endregion

    #region === DataSeries ===

    private ValueDataSeries _pocLine = null!;
    private ValueDataSeries _vahLine = null!;
    private ValueDataSeries _valLine = null!;
    private PaintbarsDataSeries _paintBars = null!;
    private ValueDataSeries _compressionMarker = null!;

    #endregion

    #region === State ===

    private decimal _poc;
    private decimal _vah;
    private decimal _val;

    private readonly Dictionary<decimal, decimal> _priceVolume = new();

    private decimal EffectiveTickSize => InstrumentInfo?.TickSize > 0 ? InstrumentInfo.TickSize : 0.25m;

    #endregion

    #region === Constructor ===

    public FabioMeanReversion() : base(true)
    {
        DenyToChangePanel = true;
        DataSeries[0].IsHidden = true;

        _pocLine = new ValueDataSeries("POC")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Yellow,
            Width = 2,
            ShowZeroValue = false
        };
        DataSeries.Add(_pocLine);

        _vahLine = new ValueDataSeries("VAH")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Cyan,
            Width = 1,
            ShowZeroValue = false
        };
        DataSeries.Add(_vahLine);

        _valLine = new ValueDataSeries("VAL")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Cyan,
            Width = 1,
            ShowZeroValue = false
        };
        DataSeries.Add(_valLine);

        _paintBars = new PaintbarsDataSeries("Balance Zone Bars")
        {
            IsHidden = true
        };
        DataSeries.Add(_paintBars);

        _compressionMarker = new ValueDataSeries("Balance Start")
        {
            VisualType = VisualMode.Square,
            Color = System.Windows.Media.Colors.Magenta,
            Width = 4,
            ShowZeroValue = false
        };
        DataSeries.Add(_compressionMarker);

        System.Diagnostics.Debug.WriteLine("=== FabioMeanReversion BALANCE ZONE ONLY started ===");
    }

    #endregion

    #region === Core Calculation (SOLO zona di balance) ===

    protected override void OnCalculate(int bar, decimal value)
    {
        // Puliamo questa barra
        _pocLine[bar] = 0;
        _vahLine[bar] = 0;
        _valLine[bar] = 0;
        _paintBars[bar] = System.Windows.Media.Colors.Transparent;
        _compressionMarker[bar] = 0;

        // Cerca zona di balance che arriva fino a questa barra
        int impulseEnd = FindLastImpulse(bar);
        if (impulseEnd < 0)
            return;

        int compStart = FindCompressionStart(bar, impulseEnd);
        if (compStart < 0)
            return;

        int compBars = bar - compStart + 1;
        if (compBars < MinCompressionBars)
            return;

        int impulseStart = FindImpulseStart(impulseEnd);
        decimal impulseRange = CalculateRange(impulseStart, impulseEnd);
        if (impulseRange <= 0)
            return;

        decimal compHigh = decimal.MinValue, compLow = decimal.MaxValue;
        for (int i = compStart; i <= bar; i++)
        {
            var c = GetCandle(i);
            if (c.High > compHigh) compHigh = c.High;
            if (c.Low < compLow) compLow = c.Low;
        }
        decimal compRange = compHigh - compLow;
        if (compRange <= 0 || compRange / impulseRange > CompressionRangeRatio)
            return;

        if (!BuildProfile(compStart, bar))
            return;

        // Disegna la zona di balance (solo qui, niente altro)
        for (int i = compStart; i <= bar; i++)
        {
            _pocLine[i] = _poc;
            _vahLine[i] = _vah;
            _valLine[i] = _val;
            _paintBars[i] = System.Windows.Media.Colors.Gold;
        }

        var sc = GetCandle(compStart);
        _compressionMarker[compStart] = sc.Low - (EffectiveTickSize * 3);

        // Pulizia area prima della zona corrente (solo sulla barra live per evitare ghost)
        if (bar == CurrentBar - 1)
        {
            int clearFrom = Math.Max(0, bar - CompressionLookback);
            for (int i = clearFrom; i < compStart; i++)
            {
                _pocLine[i] = 0;
                _vahLine[i] = 0;
                _valLine[i] = 0;
                _paintBars[i] = System.Windows.Media.Colors.Transparent;
                _compressionMarker[i] = 0;
            }
        }
    }

    #endregion

    #region === Helper per zona di balance (semplificati) ===

    private int FindCompressionStart(int currentBar, int impulseEnd)
    {
        int candidate = impulseEnd + 1;
        if (candidate >= currentBar)
            return -1;

        decimal high = decimal.MinValue;
        decimal low = decimal.MaxValue;
        for (int i = candidate; i <= currentBar; i++)
        {
            var c = GetCandle(i);
            if (c.High > high) high = c.High;
            if (c.Low < low) low = c.Low;
        }

        if (HasDirectionalExpansion(candidate, currentBar, high, low))
            return -1;

        return candidate;
    }

    private bool HasDirectionalExpansion(int startBar, int endBar, decimal rangeHigh, decimal rangeLow)
    {
        if (endBar - startBar + 1 < MinCompressionBars)
            return false;

        int closesAbove = 0;
        int closesBelow = 0;
        decimal band = Math.Max(EffectiveTickSize * 2, (rangeHigh - rangeLow) * 0.15m);

        int look = Math.Min(6, endBar - startBar + 1);
        for (int i = Math.Max(startBar, endBar - look + 1); i <= endBar; i++)
        {
            var c = GetCandle(i);
            if (c.Close >= rangeHigh - band) closesAbove++;
            if (c.Close <= rangeLow + band) closesBelow++;
        }
        return closesAbove >= 5 || closesBelow >= 5;
    }

    private int FindLastImpulse(int currentBar)
    {
        decimal bestScore = 0;
        int best = -1;
        int start = Math.Max(1, currentBar - CompressionLookback + 1);

        for (int i = currentBar - MinCompressionBars; i >= start; i--)
        {
            var c = GetCandle(i);
            var prev = GetCandle(i - 1);

            decimal range = c.High - c.Low;
            if (range <= 0) continue;

            decimal prevRange = Math.Max(prev.High - prev.Low, EffectiveTickSize);
            decimal body = Math.Abs(c.Close - c.Open);
            decimal bodyRatio = body / range;
            decimal closeExtreme = Math.Abs(((c.Close - c.Low) / range) - 0.5m) * 2m;
            decimal rangeExp = range / prevRange;

            decimal score = bodyRatio * 0.40m + closeExtreme * 0.35m;
            if (rangeExp > 1.2m)
                score += Math.Min(0.25m, (rangeExp - 1m) * 0.20m);
            if (Math.Abs(c.Delta) > 0)
                score += 0.05m;

            if (score > bestScore)
            {
                bestScore = score;
                best = i;
            }
        }
        return bestScore >= ImpulseMinScore ? best : -1;
    }

    private int FindImpulseStart(int impulseEnd)
    {
        if (impulseEnd <= 0) return 0;

        var c = GetCandle(impulseEnd);
        int direction = Math.Sign(c.Close - c.Open);
        if (direction == 0)
            direction = (c.Close >= c.Low + (c.High - c.Low) / 2m) ? 1 : -1;

        int start = impulseEnd;
        int minBar = Math.Max(1, impulseEnd - 8);

        for (int i = impulseEnd - 1; i >= minBar; i--)
        {
            var candle = GetCandle(i);
            decimal r = candle.High - candle.Low;
            if (r <= 0) break;

            int d = Math.Sign(candle.Close - candle.Open);
            if (d == direction || Math.Abs(candle.Close - candle.Open) >= r * 0.6m)
            {
                start = i; continue;
            }
            break;
        }
        return start;
    }

    private decimal CalculateRange(int start, int end)
    {
        if (start > end) return 0;
        decimal hi = decimal.MinValue, lo = decimal.MaxValue;
        for (int i = start; i <= end; i++)
        {
            var c = GetCandle(i);
            if (c.High > hi) hi = c.High;
            if (c.Low < lo) lo = c.Low;
        }
        return (hi == decimal.MinValue || lo == decimal.MaxValue) ? 0 : hi - lo;
    }

    private bool BuildProfile(int startBar, int endBar)
    {
        _priceVolume.Clear();
        if (startBar > endBar || startBar < 0) return false;

        for (int i = startBar; i <= endBar; i++)
        {
            var levels = GetCandle(i).GetAllPriceLevels();
            if (levels == null) continue;
            foreach (var lv in levels)
            {
                if (!_priceVolume.ContainsKey(lv.Price)) _priceVolume[lv.Price] = 0;
                _priceVolume[lv.Price] += lv.Volume;
            }
        }
        if (_priceVolume.Count == 0) return false;

        _poc = _priceVolume.OrderByDescending(kv => kv.Value).First().Key;
        return CalculateValueArea(out _vah, out _val);
    }

    private bool CalculateValueArea(out decimal vah, out decimal val)
    {
        vah = 0; val = 0;
        if (_priceVolume.Count == 0) return false;

        decimal total = _priceVolume.Values.Sum();
        decimal target = total * (ValueAreaPct / 100m);
        var prices = _priceVolume.Keys.OrderBy(p => p).ToList();
        int pocIdx = prices.IndexOf(_poc);
        if (pocIdx < 0) return false;

        decimal acc = _priceVolume[_poc];
        int lo = pocIdx, hi = pocIdx;

        while (acc < target)
        {
            decimal vB = lo > 0 ? _priceVolume[prices[lo-1]] : -1;
            decimal vA = hi < prices.Count-1 ? _priceVolume[prices[hi+1]] : -1;
            if (vB < 0 && vA < 0) break;

            if (vA >= vB)
            {
                if (hi >= prices.Count-1) break;
                hi++; acc += _priceVolume[prices[hi]];
            }
            else
            {
                if (lo <= 0) break;
                lo--; acc += _priceVolume[prices[lo]];
            }
        }
        val = prices[lo];
        vah = prices[hi];
        return true;
    }

    #endregion
}
