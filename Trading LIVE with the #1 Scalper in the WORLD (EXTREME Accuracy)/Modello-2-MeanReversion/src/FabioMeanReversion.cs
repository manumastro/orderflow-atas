// FabioMeanReversion — Model 2
// Step 0: sessione London (TradingSessionDescriptions + IsNewSession)
// Step 1a: individuazione zona di balance (area compressione, dinamica)

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ATAS.Indicators;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    private enum BalanceZoneState
    {
        OutsideLondon,
        NoCompression,
        CompressionVisible
    }

    [Display(Name = "London Session Name", Order = 10)]
    public string LondonSessionName { get; set; } = "London";

    [Display(Name = "Enable Logging", Order = 20)]
    public bool EnableLogging { get; set; } = true;

    private PaintbarsDataSeries _londonBars = null!;
    private PaintbarsDataSeries _compressionBars = null!;
    private ValueDataSeries _compressionHigh = null!;
    private ValueDataSeries _compressionLow = null!;
    private ValueDataSeries _compressionStartMarker = null!;

    private readonly List<TradingSessionDescription> _sessions = new();
    private readonly List<int> _sessionIndexByBar = new();
    private int _londonIndex = -1;
    private long? _londonId;
    private int _lastLondonStartBar = -1;
    private int _lastCompressionStartBar = -1;
    private BalanceZoneState _lastBalanceZoneState = BalanceZoneState.OutsideLondon;

    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs", "FabioMeanReversion.log");
    private StreamWriter? _log;

    public FabioMeanReversion()
    {
        _londonBars = new PaintbarsDataSeries("London Session");
        DataSeries.Add(_londonBars);

        _compressionBars = new PaintbarsDataSeries("Compression Zone");
        DataSeries.Add(_compressionBars);

        _compressionHigh = new ValueDataSeries("Compression High")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Orange,
            ShowZeroValue = false
        };
        DataSeries.Add(_compressionHigh);

        _compressionLow = new ValueDataSeries("Compression Low")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Orange,
            ShowZeroValue = false
        };
        DataSeries.Add(_compressionLow);

        _compressionStartMarker = new ValueDataSeries("Compression Start")
        {
            VisualType = VisualMode.UpArrow,
            Color = System.Windows.Media.Colors.Gold,
            ShowZeroValue = false
        };
        DataSeries.Add(_compressionStartMarker);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            _log = new StreamWriter(LogPath, append: true) { AutoFlush = true };
        }
        catch { /* ignore */ }
    }

    protected override void OnInitialize() => ResolveSessions();

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            _sessionIndexByBar.Clear();
            _lastLondonStartBar = -1;
            _lastCompressionStartBar = -1;
            _lastBalanceZoneState = BalanceZoneState.OutsideLondon;
            ResolveSessions();
        }

        UpdateSessionIndex(bar);

        var inLondon = IsInLondonSession(bar);
        _londonBars[bar] = inLondon
            ? System.Windows.Media.Colors.DodgerBlue
            : System.Windows.Media.Colors.Transparent;

        ClearCompressionVisuals(bar);

        if (!inLondon)
        {
            UpdateBalanceZoneState(bar, BalanceZoneState.OutsideLondon);
            return;
        }

        var compressionStart = FindCompressionStart(bar);
        if (compressionStart < 0)
        {
            UpdateBalanceZoneState(bar, BalanceZoneState.NoCompression);
            return;
        }

        var compressionHigh = GetRangeHigh(compressionStart, bar);
        var compressionLow = GetRangeLow(compressionStart, bar);

        for (var i = compressionStart; i <= bar; i++)
        {
            _compressionBars[i] = System.Windows.Media.Colors.DarkGoldenrod;
            _compressionHigh[i] = compressionHigh;
            _compressionLow[i] = compressionLow;
        }

        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        _compressionStartMarker[compressionStart] = compressionLow - tick * 2;

        if (bar < CurrentBar - 1)
            return;

        if (compressionStart != _lastCompressionStartBar)
        {
            _lastCompressionStartBar = compressionStart;
            WriteLog(
                $"COMPRESSION_START bar={compressionStart} end={bar} " +
                $"time={GetCandle(compressionStart).LastTime:HH:mm:ss} " +
                $"high={compressionHigh} low={compressionLow} bars={bar - compressionStart + 1}");
        }

        UpdateBalanceZoneState(bar, BalanceZoneState.CompressionVisible);

        var start = GetLondonSessionStartBar(bar);
        if (start >= 0 && start != _lastLondonStartBar)
        {
            _lastLondonStartBar = start;
            WriteLog($"LONDON_START bar={start} time={GetCandle(start).LastTime:HH:mm:ss} id={_londonId}");
        }
    }

    #region === Step 1a: Balance zone (compression) ===

    private int FindCompressionStart(int bar)
    {
        if (bar < 3)
            return -1;

        var medianRange = MedianBarRange(bar, Math.Min(bar + 1, 30));
        if (medianRange <= 0)
            return -1;

        var compStart = bar;
        for (var i = bar; i >= 0; i--)
        {
            var range = GetCandle(i).High - GetCandle(i).Low;
            if (range > medianRange * 0.85m)
            {
                compStart = i + 1;
                break;
            }
            compStart = i;
        }

        if (compStart > bar)
            return -1;

        var impulseLookback = Math.Min(compStart, 20);
        var hadImpulse = false;
        for (var i = compStart - 1; i >= compStart - impulseLookback && i >= 0; i--)
        {
            var impulseRange = GetCandle(i).High - GetCandle(i).Low;
            if (impulseRange > medianRange * 1.35m)
            {
                hadImpulse = true;
                break;
            }
        }

        if (!hadImpulse)
            return -1;

        var compHigh = GetRangeHigh(compStart, bar);
        var compLow = GetRangeLow(compStart, bar);
        var compRange = compHigh - compLow;

        var preStart = Math.Max(0, compStart - impulseLookback);
        if (compStart > preStart)
        {
            var preHigh = GetRangeHigh(preStart, compStart - 1);
            var preLow = GetRangeLow(preStart, compStart - 1);
            var preRange = preHigh - preLow;
            if (preRange > 0 && compRange >= preRange * 0.55m)
                return -1;
        }

        if (bar - compStart < 1)
            return -1;

        return compStart;
    }

    private decimal MedianBarRange(int bar, int lookback)
    {
        var start = Math.Max(0, bar - lookback + 1);
        var ranges = new List<decimal>();
        for (var i = start; i <= bar; i++)
            ranges.Add(GetCandle(i).High - GetCandle(i).Low);

        if (ranges.Count == 0)
            return 0;

        ranges.Sort();
        return ranges[ranges.Count / 2];
    }

    private decimal GetRangeHigh(int fromBar, int toBar)
    {
        var high = decimal.MinValue;
        for (var i = fromBar; i <= toBar; i++)
            high = Math.Max(high, GetCandle(i).High);
        return high;
    }

    private decimal GetRangeLow(int fromBar, int toBar)
    {
        var low = decimal.MaxValue;
        for (var i = fromBar; i <= toBar; i++)
            low = Math.Min(low, GetCandle(i).Low);
        return low;
    }

    private void ClearCompressionVisuals(int bar)
    {
        _compressionBars[bar] = System.Windows.Media.Colors.Transparent;
        _compressionHigh[bar] = 0;
        _compressionLow[bar] = 0;
        _compressionStartMarker[bar] = 0;
    }

    private void UpdateBalanceZoneState(int bar, BalanceZoneState state)
    {
        if (bar < CurrentBar - 1 || state == _lastBalanceZoneState)
            return;

        _lastBalanceZoneState = state;
        WriteLog($"BALANCE_ZONE bar={bar} state={state}");
    }

    #endregion

    #region === Step 0: London session ===

    private void ResolveSessions()
    {
        _sessions.Clear();
        _londonIndex = -1;
        _londonId = null;

        var descriptions = ChartInfo?.TradingSessionDescriptions;
        if (descriptions == null)
        {
            WriteLog("WARN: TradingSessionDescriptions unavailable");
            return;
        }

        foreach (var s in descriptions)
            _sessions.Add(s);

        for (var i = 0; i < _sessions.Count; i++)
        {
            if ((_sessions[i].Name ?? "").Contains(LondonSessionName, StringComparison.OrdinalIgnoreCase))
            {
                _londonIndex = i;
                _londonId = _sessions[i].Id;
                break;
            }
        }

        if (_londonIndex < 0)
            WriteLog($"WARN: '{LondonSessionName}' not in [{string.Join(", ", _sessions.Select(s => s.Name))}]");
    }

    private void UpdateSessionIndex(int bar)
    {
        while (_sessionIndexByBar.Count <= bar)
            _sessionIndexByBar.Add(-1);

        if (_sessions.Count == 0)
        {
            _sessionIndexByBar[bar] = -1;
            return;
        }

        if (bar == 0)
            _sessionIndexByBar[bar] = 0;
        else if (IsNewSession(bar))
            _sessionIndexByBar[bar] = (_sessionIndexByBar[bar - 1] + 1) % _sessions.Count;
        else
            _sessionIndexByBar[bar] = _sessionIndexByBar[bar - 1];
    }

    private bool IsInLondonSession(int bar) =>
        _londonIndex >= 0
        && bar < _sessionIndexByBar.Count
        && _sessionIndexByBar[bar] == _londonIndex;

    private int GetLondonSessionStartBar(int bar)
    {
        if (!IsInLondonSession(bar))
            return -1;

        while (bar > 0 && !IsNewSession(bar))
            bar--;

        return IsInLondonSession(bar) ? bar : -1;
    }

    #endregion

    private void WriteLog(string msg)
    {
        if (!EnableLogging) return;
        try { _log?.WriteLine($"[MEAN_REV] {msg}"); } catch { /* ignore */ }
    }
}