// ============================================================================
// FabioMeanReversion — Model 2: Mean Reversion (rewrite)
// ============================================================================
// Step 0: London session detection via ATAS chart session template
//   - ChartInfo.TradingSessionDescriptions → resolve "London" by name
//   - IsNewSession(bar) → track session segment per bar
// NO orders sent — logging only.
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

    [Display(Name = "London Session Name", GroupName = "Session", Order = 10,
        Description = "Matched against ChartInfo.TradingSessionDescriptions (case-insensitive substring).")]
    public string LondonSessionName { get; set; } = "London";

    [Display(Name = "Enable Session Logging", GroupName = "Session", Order = 20)]
    public bool EnableSessionLogging { get; set; } = true;

    #endregion

    #region === DataSeries ===

    private PaintbarsDataSeries _sessionBars = null!;

    #endregion

    #region === Session state ===

    private readonly List<TradingSessionDescription> _chartSessions = new();
    private readonly List<int> _sessionIndexByBar = new();
    private int _londonSessionIndex = -1;
    private long? _londonSessionId;
    private bool _sessionsResolved;
    private int _lastLoggedLondonStartBar = -1;
    private int _lastLoggedBar = -1;
    private int _lastLoggedSessionIndex = -2;

    private static readonly string LogDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ATAS", "Logs");
    private static readonly string LogPath = Path.Combine(LogDir, "FabioMeanReversion.log");
    private StreamWriter? _logWriter;

    #endregion

    #region === Constructor ===

    public FabioMeanReversion()
    {
        _sessionBars = new PaintbarsDataSeries("London Session")
        {
            IsHidden = false
        };
        DataSeries.Add(_sessionBars);

        InitLog();
    }

    #endregion

    #region === Lifecycle ===

    protected override void OnInitialize()
    {
        ResolveChartSessions();
        Log($"OnInitialize | sessions={_chartSessions.Count} | londonIndex={_londonSessionIndex} | londonId={_londonSessionId}");
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            _sessionIndexByBar.Clear();
            _lastLoggedLondonStartBar = -1;
            _lastLoggedBar = -1;
            _lastLoggedSessionIndex = -2;
            ResolveChartSessions();
        }

        UpdateSessionIndex(bar);
        UpdateSessionVisual(bar);

        // Only process/log on the latest bar
        if (bar < CurrentBar - 1)
            return;

        LogLondonSessionState(bar);
    }

    #endregion

    #region === Step 0: London session (ATAS dynamic) ===

    /// <summary>
    /// Loads session definitions from the chart template.
    /// API: IChart.TradingSessionDescriptions → TradingSessionDescription(Id, Name)
    /// </summary>
    private void ResolveChartSessions()
    {
        _chartSessions.Clear();
        _londonSessionIndex = -1;
        _londonSessionId = null;
        _sessionsResolved = false;

        var descriptions = ChartInfo?.TradingSessionDescriptions;
        if (descriptions == null)
        {
            Log("WARN: ChartInfo.TradingSessionDescriptions unavailable — configure session template on chart.");
            return;
        }

        foreach (var session in descriptions)
            _chartSessions.Add(session);

        if (_chartSessions.Count == 0)
        {
            Log("WARN: TradingSessionDescriptions empty — add a session template in chart settings.");
            return;
        }

        for (var i = 0; i < _chartSessions.Count; i++)
        {
            var name = _chartSessions[i].Name ?? string.Empty;
            if (name.Contains(LondonSessionName, StringComparison.OrdinalIgnoreCase))
            {
                _londonSessionIndex = i;
                _londonSessionId = _chartSessions[i].Id;
                break;
            }
        }

        _sessionsResolved = true;

        if (_londonSessionIndex < 0)
        {
            var available = string.Join(", ", _chartSessions.Select(s => s.Name));
            Log($"WARN: Session '{LondonSessionName}' not found. Available: [{available}]");
        }
    }

    /// <summary>
    /// Tracks which chart-session segment each bar belongs to.
    /// API: IsNewSession(int bar) — boundary from chart session template.
    /// </summary>
    private void UpdateSessionIndex(int bar)
    {
        EnsureBarCapacity(bar);

        if (!_sessionsResolved || _chartSessions.Count == 0)
        {
            _sessionIndexByBar[bar] = -1;
            return;
        }

        if (bar == 0)
        {
            _sessionIndexByBar[bar] = 0;
            return;
        }

        if (IsNewSession(bar))
            _sessionIndexByBar[bar] = (_sessionIndexByBar[bar - 1] + 1) % _chartSessions.Count;
        else
            _sessionIndexByBar[bar] = _sessionIndexByBar[bar - 1];
    }

    private bool IsInLondonSession(int bar)
    {
        if (_londonSessionIndex < 0 || bar >= _sessionIndexByBar.Count)
            return false;

        return _sessionIndexByBar[bar] == _londonSessionIndex;
    }

    /// <summary>
    /// First bar of the current session segment (any session).
    /// </summary>
    private int GetCurrentSessionStartBar(int bar)
    {
        while (bar > 0 && !IsNewSession(bar))
            bar--;
        return bar;
    }

    /// <summary>
    /// First bar of the current London segment, or -1 if not in London.
    /// </summary>
    private int GetLondonSessionStartBar(int bar)
    {
        if (!IsInLondonSession(bar))
            return -1;

        var start = GetCurrentSessionStartBar(bar);
        return IsInLondonSession(start) ? start : -1;
    }

    private string GetCurrentSessionName(int bar)
    {
        if (bar >= _sessionIndexByBar.Count || _sessionIndexByBar[bar] < 0)
            return "N/A";

        var idx = _sessionIndexByBar[bar];
        return _chartSessions[idx].Name ?? $"#{idx}";
    }

    private void UpdateSessionVisual(int bar)
    {
        if (IsInLondonSession(bar))
            _sessionBars[bar] = System.Windows.Media.Colors.DodgerBlue;
        else
            _sessionBars[bar] = System.Windows.Media.Colors.Transparent;
    }

    private void LogLondonSessionState(int bar)
    {
        if (!EnableSessionLogging)
            return;

        var candle = GetCandle(bar);
        var inLondon = IsInLondonSession(bar);
        var sessionName = GetCurrentSessionName(bar);
        var londonStart = GetLondonSessionStartBar(bar);

        var sessionIdx = bar < _sessionIndexByBar.Count ? _sessionIndexByBar[bar] : -1;
        var isNewBar = bar != _lastLoggedBar;
        var sessionChanged = sessionIdx != _lastLoggedSessionIndex;

        if (inLondon && londonStart >= 0 && londonStart != _lastLoggedLondonStartBar)
        {
            _lastLoggedLondonStartBar = londonStart;
            var startCandle = GetCandle(londonStart);
            Log($"LONDON_START bar={londonStart} | time={startCandle.LastTime:HH:mm:ss} | id={_londonSessionId}");
        }

        if (isNewBar || sessionChanged)
        {
            _lastLoggedBar = bar;
            _lastLoggedSessionIndex = sessionIdx;
            Log($"SESSION bar={bar} | time={candle.LastTime:HH:mm:ss} | active={sessionName} | london={inLondon} | londonStart={londonStart}");
        }
    }

    private void EnsureBarCapacity(int bar)
    {
        while (_sessionIndexByBar.Count <= bar)
            _sessionIndexByBar.Add(-1);
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
        catch { /* ignore */ }
    }

    private void Log(string message)
    {
        if (!EnableSessionLogging)
            return;

        var line = $"[MEAN_REV] {message}";
        System.Diagnostics.Debug.WriteLine(line);
        try { _logWriter?.WriteLine(line); } catch { /* ignore */ }
    }

    #endregion
}