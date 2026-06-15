// FabioMeanReversion — Model 2 · rewrite da zero

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ATAS.Indicators;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    [Display(Name = "Enable Logging", Order = 10)]
    public bool EnableLogging { get; set; } = true;

    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs", "FabioMeanReversion.log");
    private StreamWriter? _log;

    public FabioMeanReversion()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            _log = new StreamWriter(LogPath, append: true) { AutoFlush = true };
        }
        catch { /* ignore */ }
    }

    protected override void OnInitialize()
    {
        WriteLog($"=== FabioMeanReversion started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
    }

    protected override void OnCalculate(int bar, decimal value)
    {
    }

    private void WriteLog(string msg)
    {
        if (!EnableLogging) return;
        try { _log?.WriteLine($"[MEAN_REV] {msg}"); } catch { /* ignore */ }
    }
}