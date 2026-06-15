// FabioMeanReversion — Model 2
// Step 0: sessione London (TradingSessionDescriptions + IsNewSession)
// Step 1a: individuazione zona di balance (area compressione, dinamica)
// Step 1b: profile POC/VAH/VAL + stati balance sul range compressione

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ATAS.Indicators;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    private enum BalanceState
    {
        OutsideLondon,
        NoCompression,
        CompressionForming,
        BalanceReady,
        OutOfBalance
    }

    private sealed class VolumeProfile
    {
        public decimal POC { get; init; }
        public decimal VAH { get; init; }
        public decimal VAL { get; init; }
        public decimal TotalVolume { get; init; }
        public bool IsValid { get; init; }
    }

    [Display(Name = "London Session Name", Order = 10)]
    public string LondonSessionName { get; set; } = "London";

    [Display(Name = "Enable Logging", Order = 20)]
    public bool EnableLogging { get; set; } = true;

    private PaintbarsDataSeries _londonBars = null!;
    private PaintbarsDataSeries _balanceBars = null!;
    private ValueDataSeries _compressionHigh = null!;
    private ValueDataSeries _compressionLow = null!;
    private ValueDataSeries _compressionStartMarker = null!;
    private ValueDataSeries _pocLine = null!;
    private ValueDataSeries _vahLine = null!;
    private ValueDataSeries _valLine = null!;

    private readonly List<TradingSessionDescription> _sessions = new();
    private readonly List<int> _sessionIndexByBar = new();
    private int _londonIndex = -1;
    private long? _londonId;
    private int _lastLondonStartBar = -1;
    private int _lastCompressionStartBar = -1;
    private BalanceState _lastBalanceState = BalanceState.OutsideLondon;

    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs", "FabioMeanReversion.log");
    private StreamWriter? _log;

    public FabioMeanReversion()
    {
        _londonBars = new PaintbarsDataSeries("London Session");
        DataSeries.Add(_londonBars);

        _balanceBars = new PaintbarsDataSeries("Balance State");
        DataSeries.Add(_balanceBars);

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

        _pocLine = new ValueDataSeries("POC")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.White,
            Width = 2,
            ShowZeroValue = false
        };
        DataSeries.Add(_pocLine);

        _vahLine = new ValueDataSeries("VAH")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Cyan,
            ShowZeroValue = false
        };
        DataSeries.Add(_vahLine);

        _valLine = new ValueDataSeries("VAL")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Cyan,
            ShowZeroValue = false
        };
        DataSeries.Add(_valLine);

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
            _lastBalanceState = BalanceState.OutsideLondon;
            ResolveSessions();
        }

        UpdateSessionIndex(bar);

        var inLondon = IsInLondonSession(bar);
        _londonBars[bar] = inLondon
            ? System.Windows.Media.Colors.DodgerBlue
            : System.Windows.Media.Colors.Transparent;

        ClearBarVisuals(bar);

        if (!inLondon)
        {
            UpdateBalanceState(bar, BalanceState.OutsideLondon);
            return;
        }

        var compressionStart = FindCompressionStart(bar);
        if (compressionStart < 0)
        {
            UpdateBalanceState(bar, BalanceState.NoCompression);
            return;
        }

        var compressionHigh = GetRangeHigh(compressionStart, bar);
        var compressionLow = GetRangeLow(compressionStart, bar);
        var profile = BuildProfile(compressionStart, bar);
        var state = EvaluateBalanceState(bar, profile);

        for (var i = compressionStart; i <= bar; i++)
        {
            _compressionHigh[i] = compressionHigh;
            _compressionLow[i] = compressionLow;

            if (profile.IsValid)
            {
                _pocLine[i] = profile.POC;
                _vahLine[i] = profile.VAH;
                _valLine[i] = profile.VAL;
            }
        }

        _balanceBars[bar] = state switch
        {
            BalanceState.CompressionForming => System.Windows.Media.Colors.DarkGoldenrod,
            BalanceState.BalanceReady => System.Windows.Media.Colors.MediumSeaGreen,
            BalanceState.OutOfBalance => System.Windows.Media.Colors.IndianRed,
            _ => System.Windows.Media.Colors.Transparent
        };

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

        UpdateBalanceState(bar, state, profile, compressionStart);

        var londonStart = GetLondonSessionStartBar(bar);
        if (londonStart >= 0 && londonStart != _lastLondonStartBar)
        {
            _lastLondonStartBar = londonStart;
            WriteLog($"LONDON_START bar={londonStart} time={GetCandle(londonStart).LastTime:HH:mm:ss} id={_londonId}");
        }
    }

    #region === Step 1b: Profile + balance states ===

    private VolumeProfile BuildProfile(int startBar, int endBar)
    {
        var volumeByPrice = new Dictionary<decimal, decimal>();
        var barVolumes = new List<decimal>();

        for (var i = startBar; i <= endBar; i++)
        {
            var candle = GetCandle(i);
            barVolumes.Add(candle.Volume);

            var levels = candle.GetAllPriceLevels();
            if (levels == null)
                continue;

            foreach (var level in levels)
            {
                if (!volumeByPrice.ContainsKey(level.Price))
                    volumeByPrice[level.Price] = 0;
                volumeByPrice[level.Price] += level.Volume;
            }
        }

        if (volumeByPrice.Count < 2 || barVolumes.Count < 2)
            return InvalidProfile();

        var totalVolume = volumeByPrice.Values.Sum();
        if (totalVolume <= 0)
            return InvalidProfile();

        barVolumes.Sort();
        var medianBarVolume = barVolumes[barVolumes.Count / 2];
        var barCount = endBar - startBar + 1;
        if (totalVolume < medianBarVolume * barCount * 0.2m)
            return InvalidProfile();

        var poc = volumeByPrice.OrderByDescending(kv => kv.Value).First().Key;
        var sortedPrices = volumeByPrice.Keys.OrderBy(p => p).ToList();
        var pocIndex = sortedPrices.IndexOf(poc);
        var targetVolume = totalVolume * 0.70m;
        var accumulated = volumeByPrice[poc];
        var valuePrices = new List<decimal> { poc };

        var up = pocIndex + 1;
        var down = pocIndex - 1;

        while (accumulated < targetVolume && (up < sortedPrices.Count || down >= 0))
        {
            var upVolume = up < sortedPrices.Count ? volumeByPrice[sortedPrices[up]] : 0m;
            var downVolume = down >= 0 ? volumeByPrice[sortedPrices[down]] : 0m;

            if (upVolume <= 0 && downVolume <= 0)
                break;

            if (upVolume >= downVolume)
            {
                if (upVolume > 0)
                {
                    valuePrices.Add(sortedPrices[up]);
                    accumulated += upVolume;
                    up++;
                }
                else
                {
                    valuePrices.Add(sortedPrices[down]);
                    accumulated += downVolume;
                    down--;
                }
            }
            else
            {
                valuePrices.Add(sortedPrices[down]);
                accumulated += downVolume;
                down--;
            }
        }

        return new VolumeProfile
        {
            POC = poc,
            VAH = valuePrices.Max(),
            VAL = valuePrices.Min(),
            TotalVolume = totalVolume,
            IsValid = true
        };
    }

    private static VolumeProfile InvalidProfile() => new()
    {
        POC = 0,
        VAH = 0,
        VAL = 0,
        TotalVolume = 0,
        IsValid = false
    };

    private BalanceState EvaluateBalanceState(int bar, VolumeProfile profile)
    {
        if (!profile.IsValid)
            return BalanceState.CompressionForming;

        if (BrokeWithFollowThrough(bar, profile.VAH, profile.VAL))
            return BalanceState.OutOfBalance;

        if (IsProtective(bar, profile))
            return BalanceState.BalanceReady;

        return BalanceState.CompressionForming;
    }

    private bool BrokeWithFollowThrough(int bar, decimal vah, decimal val)
    {
        var candle = GetCandle(bar);
        var valueRange = vah - val;
        if (valueRange <= 0)
            return false;

        if (candle.Close > vah)
        {
            if (bar > 0 && GetCandle(bar - 1).Close > vah)
                return true;

            return candle.Close > candle.Open
                && (candle.Close - vah) >= valueRange * 0.05m;
        }

        if (candle.Close < val)
        {
            if (bar > 0 && GetCandle(bar - 1).Close < val)
                return true;

            return candle.Close < candle.Open
                && (val - candle.Close) >= valueRange * 0.05m;
        }

        return false;
    }

    private bool IsProtective(int bar, VolumeProfile profile)
    {
        var candle = GetCandle(bar);
        var valueRange = profile.VAH - profile.VAL;
        if (valueRange <= 0)
            return false;

        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        var tolerance = Math.Max(tick, valueRange * 0.02m);
        return candle.Close >= profile.VAL - tolerance
            && candle.Close <= profile.VAH + tolerance;
    }

    private void UpdateBalanceState(int bar, BalanceState state, VolumeProfile? profile = null, int compressionStart = -1)
    {
        if (bar < CurrentBar - 1 || state == _lastBalanceState)
            return;

        _lastBalanceState = state;

        if (profile?.IsValid == true)
        {
            WriteLog(
                $"BALANCE_STATE bar={bar} state={state} " +
                $"profile={compressionStart}-{bar} " +
                $"POC={profile.POC} VAH={profile.VAH} VAL={profile.VAL} vol={profile.TotalVolume:F0}");
        }
        else
        {
            WriteLog($"BALANCE_STATE bar={bar} state={state}");
        }
    }

    #endregion

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

    private void ClearBarVisuals(int bar)
    {
        _balanceBars[bar] = System.Windows.Media.Colors.Transparent;
        _compressionHigh[bar] = 0;
        _compressionLow[bar] = 0;
        _compressionStartMarker[bar] = 0;
        _pocLine[bar] = 0;
        _vahLine[bar] = 0;
        _valLine[bar] = 0;
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