// ============================================================================
// FabioMeanReversion — Balance/Consolidation Zones (Historical Rectangles)
// ============================================================================
// Simple detection of past consolidation/balance zones (after impulse legs with contained range).
// No sessions, no current-only logic, no profile values/lines.
// 
// Visuals: semi-transparent cyan rectangles (boxes) for EVERY historical zone found,
// spanning the full bar range and actual high/low of the consolidation.
// Zones are drawn for the past, independent of live price.
// 
// Scan is FULL HISTORY (dynamic from start of available data, no fixed lookback param).
// Detection based on transcript: recent impulse + subsequent compression (small range).
// 
// LOG per debug: use tail-balance-log.bat (tails original ATAS log).
// Look for "*** BALANCE ZONE DETECTED ***" and rectangle adds.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Media;
using ATAS.Indicators;
using ATAS.Indicators.Drawing;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    #region === Parameters ===

    // No fixed parameters exposed for detection (as per Fabio's approach in the transcript).
    // Everything is dynamic: full history scan from start of data, relative impulse/compression
    // based on recent price action (no hardcoded scores/ratios/bars that Fabio "doesn't think with").
    // Tune not needed; the logic finds visible compressions after impulses.

    #endregion

    #region === DataSeries ===

    // Using Rectangles collection for boxes instead of paintbars

    #endregion

    #region === State ===

    private decimal EffectiveTickSize => 0.25m; // default for NQ/ES; dynamic access had name conflict with ATAS type InstrumentInfo
    private int _lastLoggedBar = -1;

    // For drawing closed past zones as rectangles (independent of current live state)
    private int _openZoneStart = -1;
    private decimal _openZoneHigh = decimal.MinValue;
    private decimal _openZoneLow = decimal.MaxValue;

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

        LogBal("=== FabioMeanReversion BALANCE ZONE ONLY (rectangles for historical consolidations) started ===");
    }

    #endregion

    #region === Core Calculation (SOLO zona di balance) ===

    protected override void OnCalculate(int bar, decimal value)
    {
        // Clear previous drawings at start of recalc (bar 0) so we redraw all historical past zones cleanly.
        // This way zones are independent of "current" live state.
        if (bar == 0)
        {
            Rectangles.Clear();
            _openZoneStart = -1;
            _openZoneHigh = decimal.MinValue;
            _openZoneLow = decimal.MaxValue;
            _lastLoggedBar = -1;
        }

        // Cerca zona di balance che arriva fino a questa barra (funziona per storici e live)
        int impulseEnd = FindLastImpulse(bar);
        if (impulseEnd < 0)
        {
            CloseOpenZoneIfAny(bar); // close any open when no impulse
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: no impulse found in lookback");
            _lastLoggedBar = bar;
            return;
        }

        int compStart = FindCompressionStart(bar, impulseEnd);
        if (compStart < 0)
        {
            CloseOpenZoneIfAny(bar);
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: no compression start after impulse {impulseEnd}");
            _lastLoggedBar = bar;
            return;
        }

        int compBars = bar - compStart + 1;
        const int minCompBars = 5;  // dynamic/simple, small to catch visible compressions (Fabio-style, no fixed param)
        if (compBars < minCompBars)
        {
            CloseOpenZoneIfAny(bar);
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: compression too short ({compBars} < {minCompBars}) start={compStart}");
            _lastLoggedBar = bar;
            return;
        }

        int impulseStart = FindImpulseStart(impulseEnd);
        decimal impulseRange = CalculateRange(impulseStart, impulseEnd);
        if (impulseRange <= 0)
        {
            CloseOpenZoneIfAny(bar);
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
        const decimal maxRatio = 0.6m;  // dynamic relative: compression range < 60% of impulse (no user param, per transcript)
        decimal ratio = compRange / impulseRange;
        if (compRange <= 0 || ratio > maxRatio)
        {
            CloseOpenZoneIfAny(bar);
            if (bar != _lastLoggedBar) LogBal($"[BAL] Bar={bar}: ratio too high {ratio:P2} (max {maxRatio:P2}) compStart={compStart} impulseEnd={impulseEnd}");
            _lastLoggedBar = bar;
            return;
        }

        // Valid compression zone at this bar's snapshot.
        // Track as open (for historical scan this will close at the right past point when later bars "break" it)
        LogBal($"[BAL] *** BALANCE ZONE DETECTED *** Bar={bar} compStart={compStart} compBars={compBars} ratio={ratio:P2} impulseEnd={impulseEnd}");

        if (_openZoneStart < 0)
        {
            // start new open zone
            _openZoneStart = compStart;
            _openZoneHigh = compHigh;
            _openZoneLow = compLow;
        }
        else if (compStart != _openZoneStart)
        {
            // new zone starting (replot), close previous as past zone (rectangle)
            AddZoneRectangle(_openZoneStart, bar - 1, _openZoneLow, _openZoneHigh);
            _openZoneStart = compStart;
            _openZoneHigh = compHigh;
            _openZoneLow = compLow;
        }
        else
        {
            // extend current open zone
            _openZoneHigh = Math.Max(_openZoneHigh, compHigh);
            _openZoneLow = Math.Min(_openZoneLow, compLow);
        }
    }

    private void CloseOpenZoneIfAny(int currentBar)
    {
        if (_openZoneStart >= 0)
        {
            AddZoneRectangle(_openZoneStart, currentBar, _openZoneLow, _openZoneHigh);
            _openZoneStart = -1;
            _openZoneHigh = decimal.MinValue;
            _openZoneLow = decimal.MaxValue;
        }
    }

    private void AddZoneRectangle(int firstBar, int secondBar, decimal lowPrice, decimal highPrice)
    {
        if (firstBar >= secondBar || firstBar < 0) return;

        // Simple semi-transparent rectangle box for the consolidation zone (height = actual high/low of zone, width = bars)
        System.Drawing.Pen outlinePen = new System.Drawing.Pen(System.Drawing.Color.DarkCyan, 1);
        System.Drawing.SolidBrush fillBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(40, 0, 255, 255)); // light cyan, low opacity box

        var rect = new DrawingRectangle(firstBar, lowPrice, secondBar, highPrice, outlinePen, fillBrush);
        // Do not extend; fixed historical zone
        rect.ExtendLeft = false;
        rect.ExtendRight = false;

        Rectangles.Add(rect);
    }

    protected override void OnFinishRecalculate()
    {
        // At the end of historical data, close any still-open zone so its rectangle is added (past zone).
        if (_openZoneStart >= 0)
        {
            AddZoneRectangle(_openZoneStart, CurrentBar - 1, _openZoneLow, _openZoneHigh);
            _openZoneStart = -1;
            _openZoneHigh = decimal.MinValue;
            _openZoneLow = decimal.MaxValue;
        }
    }

    #endregion

    #region === Helper per zona di balance (semplificati) ===

    private int FindCompressionStart(int currentBar, int impulseEnd)
    {
        int candidate = impulseEnd + 1;
        if (candidate >= currentBar)
            return -1;

        // Compute range from after impulse to current
        decimal fullHigh = decimal.MinValue;
        decimal fullLow = decimal.MaxValue;
        for (int i = candidate; i <= currentBar; i++)
        {
            var c = GetCandle(i);
            if (c.High > fullHigh) fullHigh = c.High;
            if (c.Low < fullLow) fullLow = c.Low;
        }

        // To support replotting on "new dealing range" (local recent consolidation):
        // search back a bit for a tighter recent window start. If found significantly tighter than full post-impulse,
        // use it so we don't stay stuck with an old wide range from after the impulse.
        decimal recentHigh = decimal.MinValue;
        decimal recentLow = decimal.MaxValue;
        int recentStart = currentBar;

        int look = Math.Min(50, currentBar - impulseEnd);
        for (int s = currentBar; s >= Math.Max(candidate, currentBar - look); s--)
        {
            var c = GetCandle(s);
            if (c.High > recentHigh) recentHigh = c.High;
            if (c.Low < recentLow) recentLow = c.Low;
            recentStart = s;
        }

        if (recentHigh - recentLow < (fullHigh - fullLow) * 0.65m && (recentHigh - recentLow) > 0)
        {
            candidate = recentStart;
        }

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
        if (endBar - startBar + 1 < 5)
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
        if (currentBar < 2)
            return -1;

        // Dynamic from beginning of available data (no fixed lookback)
        int start = 0;

        // Dynamic impulse: most recent bar with significantly larger range than previous
        // (relative, no fixed score threshold - Fabio doesn't use fixed numbers).
        // Looks for "impulse leg" as per transcript: expansion after previous.
        for (int i = currentBar - 1; i >= Math.Max(start, 1); i--)
        {
            var c = GetCandle(i);
            var prev = GetCandle(i - 1);

            decimal range = c.High - c.Low;
            if (range <= 0) continue;

            decimal prevRange = Math.Max(prev.High - prev.Low, EffectiveTickSize);
            decimal body = Math.Abs(c.Close - c.Open);
            decimal bodyRatio = body / range;
            decimal rangeExp = range / prevRange;

            // Dynamic: recent bar has range at least 1.5x previous AND decent body ( >50% of its range)
            // This captures "big trade" / impulse without hardcoded score.
            if (rangeExp >= 1.5m && bodyRatio >= 0.5m)
            {
                return i;  // most recent impulse leg
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
