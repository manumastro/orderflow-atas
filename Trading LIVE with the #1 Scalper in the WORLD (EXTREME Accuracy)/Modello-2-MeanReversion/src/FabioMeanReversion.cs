// ============================================================================
// FabioMeanReversion — Balance Zone Detection (SIMPLIFIED FROM SCRATCH)
// ============================================================================
// Solo individuazione della zona di balance / compressione.
// Niente sessioni, niente profile, niente POC/VAH/VAL, niente linee di valore.
// 
// Visuals semplici:
//   - Paintbars per evidenziare le barre della zona di balance (colore oro)
//   - Marker (quadrato magenta) all'inizio della zona
//
// Logica base: dopo un impulse, zona dove il range resta compresso per abbastanza barre.
// 
// LOG per debug:
//   Esegui il file "tail-balance-log.bat" (doppio clic).
//   Apre PowerShell e fa il tail in tempo reale del log originale di ATAS.
//   Mostra le ultime righe + segue tutti i nuovi messaggi.
//   Per uscire: Ctrl+C nella finestra PowerShell.
// Cerca le righe con "*** BALANCE ZONE DETECTED ***".
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
        Description = "Max range of compression / range of previous impulse leg (0.45 = strict, try 0.65-0.80 for more zones)")]
    public decimal CompressionRangeRatio { get; set; } = 0.65m;

    [Display(Name = "Min Compression Bars", GroupName = "Balance Zone", Order = 12)]
    public int MinCompressionBars { get; set; } = 8;

    [Display(Name = "Impulse Min Score", GroupName = "Balance Zone", Order = 13,
        Description = "Min score (0-1) to consider a bar as impulse. Lower = more impulse bars detected")]
    public decimal ImpulseMinScore { get; set; } = 0.30m;

    #endregion

    #region === DataSeries ===

    private PaintbarsDataSeries _paintBars = null!;
    private ValueDataSeries _compressionMarker = null!;

    #endregion

    #region === State ===

    private decimal EffectiveTickSize => InstrumentInfo?.TickSize > 0 ? InstrumentInfo.TickSize : 0.25m;
    private int _lastBalanceStart = -1;
    private int _lastLoggedBar = -1;

    private static readonly string LogPath = System.IO.Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs", "FabioBalanceZone.log");

    private static void LogBal(string msg)
    {
        try
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(LogPath));
            System.IO.File.AppendAllText(LogPath, $"[{System.DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }
        catch { }
        System.Diagnostics.Debug.WriteLine(msg);
    }

    #endregion

    #region === Constructor ===

    public FabioMeanReversion() : base(true)
    {
        DenyToChangePanel = true;
        DataSeries[0].IsHidden = true;

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

        LogBal("=== FabioMeanReversion BALANCE ZONE ONLY started (check this log for detections) ===");
    }

    #endregion

    #region === Core Calculation (SOLO zona di balance) ===

    protected override void OnCalculate(int bar, decimal value)
    {
        _compressionMarker[bar] = 0;

        // Cerca zona di balance che arriva fino a questa barra
        int impulseEnd = FindLastImpulse(bar);
        if (impulseEnd < 0)
        {
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: no impulse found in lookback");
            _lastLoggedBar = bar;
            return;
        }

        int compStart = FindCompressionStart(bar, impulseEnd);
        if (compStart < 0)
        {
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: no compression start after impulse {impulseEnd}");
            _lastLoggedBar = bar;
            return;
        }

        int compBars = bar - compStart + 1;
        if (compBars < MinCompressionBars)
        {
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: compression too short ({compBars} < {MinCompressionBars}) start={compStart}");
            _lastLoggedBar = bar;
            return;
        }

        int impulseStart = FindImpulseStart(impulseEnd);
        decimal impulseRange = CalculateRange(impulseStart, impulseEnd);
        if (impulseRange <= 0)
        {
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: invalid impulse range");
            _lastLoggedBar = bar;
            return;
        }

        decimal compHigh = decimal.MinValue, compLow = decimal.MaxValue;
        for (int i = compStart; i <= bar; i++)
        {
            var c = GetCandle(i);
            if (c.High > compHigh) compHigh = c.High;
            if (c.Low < compLow) compLow = c.Low;
        }
        decimal compRange = compHigh - compLow;
        decimal ratio = compRange / impulseRange;
        if (compRange <= 0 || ratio > CompressionRangeRatio)
        {
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: ratio too high {ratio:P2} (max {CompressionRangeRatio:P2}) compStart={compStart} impulseEnd={impulseEnd}");
            _lastLoggedBar = bar;
            return;
        }

        // Valid zone! Highlight the ENTIRE zone
        LogBal($"[BAL] *** BALANCE ZONE DETECTED *** Bar={bar} compStart={compStart} compBars={compBars} ratio={ratio:P2} impulseEnd={impulseEnd}");

        // If new zone started, unpaint the previous one (so only current active zone is highlighted)
        if (_lastBalanceStart >= 0 && _lastBalanceStart != compStart)
        {
            for (int i = _lastBalanceStart; i < compStart; i++)
            {
                _paintBars[i] = null;
            }
        }

        // Paint the WHOLE zone (not just start bar)
        for (int i = compStart; i <= bar; i++)
        {
            _paintBars[i] = System.Windows.Media.Colors.Gold;  // entire balance zone highlighted
        }

        var sc = GetCandle(compStart);
        _compressionMarker[compStart] = sc.Low - (EffectiveTickSize * 3);

        _lastBalanceStart = compStart;
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
        int start = Math.Max(1, currentBar - CompressionLookback + 1);

        // Find the MOST RECENT impulse (latest bar with good score), not the best in window.
        // This prevents sticking with an old impulse from the start of a big move.
        for (int i = currentBar - 1; i >= start; i--)
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

            if (score >= ImpulseMinScore)
            {
                return i;  // most recent good impulse
            }
        }
        return -1;
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

    #endregion
}
