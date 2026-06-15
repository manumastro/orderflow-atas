// FabioMeanReversion — Model 2 · Step 0: sessione London (ATAS session template)

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ATAS.Indicators;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    [Display(Name = "London Session Name", Order = 10)]
    public string LondonSessionName { get; set; } = "London";

    [Display(Name = "Enable Logging", Order = 20)]
    public bool EnableLogging { get; set; } = true;

    private PaintbarsDataSeries _londonBars = null!;

    private readonly List<TradingSessionDescription> _sessions = new();
    private readonly List<int> _sessionIndexByBar = new();
    private int _londonIndex = -1;
    private long? _londonId;
    private int _lastLondonStartBar = -1;

    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs", "FabioMeanReversion.log");
    private StreamWriter? _log;

    public FabioMeanReversion()
    {
        _londonBars = new PaintbarsDataSeries("London Session");
        DataSeries.Add(_londonBars);

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
            ResolveSessions();
        }

        UpdateSessionIndex(bar);
        _londonBars[bar] = IsInLondonSession(bar)
            ? System.Windows.Media.Colors.DodgerBlue
            : System.Windows.Media.Colors.Transparent;

        if (bar < CurrentBar - 1)
            return;

        var start = GetLondonSessionStartBar(bar);
        if (start >= 0 && start != _lastLondonStartBar)
        {
            _lastLondonStartBar = start;
            WriteLog($"LONDON_START bar={start} time={GetCandle(start).LastTime:HH:mm:ss} id={_londonId}");
        }
    }

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

    private void WriteLog(string msg)
    {
        if (!EnableLogging) return;
        try { _log?.WriteLine($"[MEAN_REV] {msg}"); } catch { /* ignore */ }
    }
}