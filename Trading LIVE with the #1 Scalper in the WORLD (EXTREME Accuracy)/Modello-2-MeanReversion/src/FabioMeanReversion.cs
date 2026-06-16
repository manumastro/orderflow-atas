// ============================================================================
// FabioMeanReversion — Model 2: Mean Reversion (London Session)
// ============================================================================
// Current scope (Step 0 + Step 1 repaired + visuals fixed):
//   Step 0: Session filter (London) via configurable times + TradingSession ready
//   Step 1: Dynamic compression zone + profile on compression (NOT IsNewSession)
//           POC/VAH/VAL + states: NO_COMPRESSION / COMPRESSION_FORMING / BALANCE_READY / OUT_OF_BALANCE
//   Visuals fixed: live updates on current bar, proper zone/marker/fallback clearing on reset, separate gray fallback lines, square marker
//
// Visuals (to make zone visible on chart):
//   - Horizontal POC (yellow), VAH/VAL (cyan) lines SPANNING the detected compression bars
//   - Separate gray Fallback POC/VAH/VAL lines for daily/prev-session macro context
//   - Paintbars coloring the compression zone bars (green=ready, gold=forming, orange=out)
//   - Compression start marker (magenta square below bar low)
//   - Full logging of why/when states change
//
// Proper clearing of previous zones/markers/fallbacks on reset/out-of-balance/no-compression.
// Fallback: optional previous-day profile when dynamic compression not yet clear (transcript "stupid simple")
// Per spec + transcript: compression start AFTER last impulse leg; profile ONLY on compressed area;
// "too early" = NO_COMPRESSION until visible; replot on new dealing range inside era.
//
// NO entries yet (future steps).
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

    [Display(Name = "London Session Start Hour", GroupName = "Session", Order = 5,
        Description = "Start hour (chart time) for London filter - e.g. 8 for Italian time")]
    public int LondonStartHour { get; set; } = 8;

    [Display(Name = "London Session Start Minute", GroupName = "Session", Order = 6)]
    public int LondonStartMinute { get; set; } = 0;

    [Display(Name = "London Session End Hour", GroupName = "Session", Order = 7,
        Description = "End hour (chart time) for London filter - e.g. 16 for Italian time")]
    public int LondonEndHour { get; set; } = 16;

    [Display(Name = "London Session End Minute", GroupName = "Session", Order = 8)]
    public int LondonEndMinute { get; set; } = 30;

    [Display(Name = "Enable London Session Filter", GroupName = "Session", Order = 9)]
    public bool EnableLondonFilter { get; set; } = true;

    [Display(Name = "Compression Lookback (bars)", GroupName = "Compression", Order = 20)]
    public int CompressionLookback { get; set; } = 150;

    [Display(Name = "Compression Range Ratio", GroupName = "Compression", Order = 21,
        Description = "Max compression range / last impulse range")]
    public decimal CompressionRangeRatio { get; set; } = 0.45m;

    [Display(Name = "Min Compression Bars", GroupName = "Compression", Order = 22)]
    public int MinCompressionBars { get; set; } = 8;

    [Display(Name = "Impulse Min Score", GroupName = "Compression", Order = 23,
        Description = "Min score (0-1) to consider a bar as impulse end. Lower = more sensitive")]
    public decimal ImpulseMinScore { get; set; } = 0.30m;

    [Display(Name = "Follow Through Bars", GroupName = "Compression", Order = 24,
        Description = "Consecutive closes outside value area required to declare OUT_OF_BALANCE")]
    public int FollowThroughBars { get; set; } = 2;

    [Display(Name = "Max Distance From Value (ticks)", GroupName = "Compression", Order = 25,
        Description = "How far price can be from VAH/VAL and still count as near value")]
    public int MaxDistanceFromValueTicks { get; set; } = 3;

    [Display(Name = "Value Area % (default 70%)", GroupName = "Profile", Order = 30)]
    public decimal ValueAreaPct { get; set; } = 70m;

    [Display(Name = "Use Previous Day Profile Fallback", GroupName = "Profile", Order = 31,
        Description = "If true and no clear compression, fall back to previous day profile for visibility (per transcript 'stupid simple')")]
    public bool UsePrevDayProfileFallback { get; set; } = true;

    [Display(Name = "Enable Logging", GroupName = "Debug", Order = 90)]
    public bool EnableLogging { get; set; } = true;

    #endregion

    #region === DataSeries ===

    private ValueDataSeries _pocLine = null!;
    private ValueDataSeries _vahLine = null!;
    private ValueDataSeries _valLine = null!;
    private ValueDataSeries _pocLineFallback = null!;
    private ValueDataSeries _vahLineFallback = null!;
    private ValueDataSeries _valLineFallback = null!;
    private PaintbarsDataSeries _paintBars = null!;
    private ValueDataSeries _stateDisplay = null!;
    private ValueDataSeries _compressionMarker = null!;

    #endregion

    #region === Enums ===

    private enum BalanceState
    {
        NO_COMPRESSION,
        COMPRESSION_FORMING,
        BALANCE_READY,
        OUT_OF_BALANCE
    }

    #endregion

    #region === State ===

    private BalanceState _state = BalanceState.NO_COMPRESSION;
    private int _compressionStartBar = -1;
    private int _compressionLevelsStart = -1;
    private int _fallbackLevelsStart = -1;
    private decimal _poc;
    private decimal _vah;
    private decimal _val;
    private decimal _impulseHigh;
    private decimal _impulseLow;
    private decimal _impulseRange;
    private string _lastProfileSnapshot = string.Empty;

    private readonly Dictionary<decimal, decimal> _priceVolume = new();

    private static readonly string LogDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs");
    private static readonly string LogPath = Path.Combine(LogDir, "FabioMeanReversion.log");
    private StreamWriter? _logWriter;
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

        _pocLineFallback = new ValueDataSeries("Fallback POC")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Gray,
            Width = 1,
            ShowZeroValue = false
        };
        DataSeries.Add(_pocLineFallback);

        _vahLineFallback = new ValueDataSeries("Fallback VAH")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Gray,
            Width = 1,
            ShowZeroValue = false
        };
        DataSeries.Add(_vahLineFallback);

        _valLineFallback = new ValueDataSeries("Fallback VAL")
        {
            VisualType = VisualMode.Line,
            Color = System.Windows.Media.Colors.Gray,
            Width = 1,
            ShowZeroValue = false
        };
        DataSeries.Add(_valLineFallback);

        _paintBars = new PaintbarsDataSeries("State Bars")
        {
            IsHidden = true
        };
        DataSeries.Add(_paintBars);

        _stateDisplay = new ValueDataSeries("State")
        {
            IsHidden = true
        };
        DataSeries.Add(_stateDisplay);

        _compressionMarker = new ValueDataSeries("Comp Start")
        {
            VisualType = VisualMode.Square,
            Color = System.Windows.Media.Colors.Magenta,
            Width = 3,
            ShowZeroValue = false
        };
        DataSeries.Add(_compressionMarker);

        InitLog();
    }

    #endregion

    #region === Core Calculation ===

    protected override void OnCalculate(int bar, decimal value)
    {
        var candle = GetCandle(bar);

        if (!IsInLondonSession(candle))
        {
            ResetActiveBalance(bar);
            SetState(BalanceState.NO_COMPRESSION, bar, "Fuori sessione London");
            return;
        }

        EvaluateBalance(bar, candle);
    }

    #endregion

    #region === Step 0: Session Filter ===

    private bool IsInLondonSession(IndicatorCandle candle)
    {
        if (!EnableLondonFilter)
            return true;

        var time = candle.LastTime.TimeOfDay;
        var start = new TimeSpan(LondonStartHour, LondonStartMinute, 0);
        var end = new TimeSpan(LondonEndHour, LondonEndMinute, 0);

        if (start <= end)
            return time >= start && time <= end;
        else
            // overnight session (rare for London)
            return time >= start || time <= end;
    }

    #endregion

    #region === Step 1: Balance Detection ===

    private void EvaluateBalance(int bar, IndicatorCandle candle)
    {
        if (bar < MinCompressionBars + 3)
        {
            ResetActiveBalance(bar);
            TryDailyFallback(bar, "Barre insufficienti per compressione");
            return;
        }

        int impulseEnd = FindLastImpulse(bar);
        if (impulseEnd < 0)
        {
            ResetActiveBalance(bar);
            TryDailyFallback(bar, "Nessun impulse leggibile - usando fallback");
            return;
        }

        int compressionStart = FindCompressionStart(bar, impulseEnd, out var compressionHigh, out var compressionLow);
        if (compressionStart < 0)
        {
            ResetActiveBalance(bar);
            TryDailyFallback(bar, "Compressione non ancora visibile (troppo presto per Fabio)");
            return;
        }

        int compressionBars = bar - compressionStart + 1;
        if (compressionBars < MinCompressionBars)
        {
            if (_compressionStartBar != compressionStart)
            {
                ResetActiveBalance(bar);
            }
            SetState(BalanceState.COMPRESSION_FORMING, bar,
                $"Compressione in formazione ({compressionBars} bar < {MinCompressionBars})");
            if (BuildProfile(compressionStart, bar))
            {
                _compressionStartBar = compressionStart;
                DrawLevels(compressionStart, bar);
            }
            else
            {
                TryDailyFallback(bar, "Forming: profile non ancora ok");
            }
            return;
        }

        var impulseStart = FindImpulseStart(impulseEnd);
        CalculateImpulseRange(impulseStart, impulseEnd);

        if (_impulseRange <= 0)
        {
            ResetActiveBalance(bar);
            TryDailyFallback(bar, "Impulse range non valido");
            return;
        }

        var compressionRange = compressionHigh - compressionLow;
        if (compressionRange <= 0)
        {
            ResetActiveBalance(bar);
            SetState(BalanceState.COMPRESSION_FORMING, bar, "Range compressione nullo");
            TryDailyFallback(bar, "Range nullo");
            return;
        }

        var rangeRatio = compressionRange / _impulseRange;
        if (rangeRatio > CompressionRangeRatio)
        {
            ResetActiveBalance(bar);
            SetState(BalanceState.NO_COMPRESSION, bar,
                $"Range non compresso: ratio={rangeRatio:P1} > {CompressionRangeRatio:P0}");
            TryDailyFallback(bar, "Non ancora compresso");
            return;
        }

        if (!BuildProfile(compressionStart, bar))
        {
            ResetActiveBalance(bar);
            TryDailyFallback(bar, "Profile compressione non calcolabile");
            return;
        }

        _compressionStartBar = compressionStart;
        DrawLevels(compressionStart, bar);

        if (BrokeWithFollowThrough(bar))
        {
            SetState(BalanceState.OUT_OF_BALANCE, bar,
                $"Follow-through oltre value | POC={_poc:F2} VAH={_vah:F2} VAL={_val:F2}");
            LogProfileSnapshot(bar, "OUT_OF_BALANCE", rangeRatio, compressionBars, compressionHigh, compressionLow);
            return;
        }

        bool profileProtective = IsProfileProtective(bar);
        bool priceNearValue = IsPriceInOrNearValue(candle.Close);

        if (profileProtective && priceNearValue)
        {
            SetState(BalanceState.BALANCE_READY, bar,
                $"Balance pronto | POC={_poc:F2} VAH={_vah:F2} VAL={_val:F2} | ratio={rangeRatio:P1}");
            LogProfileSnapshot(bar, "BALANCE_READY", rangeRatio, compressionBars, compressionHigh, compressionLow);
            return;
        }

        SetState(BalanceState.COMPRESSION_FORMING, bar,
            $"Compressione ok ma balance non confermato | Protective={profileProtective} NearValue={priceNearValue} | POC={_poc:F2} VAH={_vah:F2} VAL={_val:F2}");
        LogProfileSnapshot(bar, "COMPRESSION_FORMING", rangeRatio, compressionBars, compressionHigh, compressionLow);
    }

    /// <summary>
    /// Fallback to previous day (or current day so far) profile for basic visibility.
    /// Uses IsNewSession to find day start. Per transcript: "Puoi renderlo stupidamente semplice mettendo solo il daily profile."
    /// </summary>
    private void TryDailyFallback(int bar, string reason)
    {
        if (!UsePrevDayProfileFallback)
        {
            SetState(BalanceState.NO_COMPRESSION, bar, reason);
            return;
        }

        // Find start of "current day" using IsNewSession or scan back max 1 lookback
        int dayStart = bar;
        for (int i = bar; i >= Math.Max(0, bar - CompressionLookback); i--)
        {
            if (i == 0 || IsNewSession(i))
            {
                dayStart = i;
                break;
            }
        }

        // If we crossed a session, the previous day would be up to dayStart-1, but for simplicity use from last IsNewSession to now as "day profile"
        // To get "previous day", we'd need two IsNewSession, but for intraday visibility use the forming day profile as macro context
        int profileStart = dayStart;
        // Try to use previous full session if possible: walk to one more IsNewSession
        int prevSessionStart = -1;
        for (int i = dayStart - 1; i >= Math.Max(0, bar - CompressionLookback); i--)
        {
            if (IsNewSession(i))
            {
                prevSessionStart = i;
                break;
            }
        }
        if (prevSessionStart >= 0)
        {
            // Build profile on the *previous* session bars as "prev day"
            profileStart = prevSessionStart;
        }

        if (!BuildProfile(profileStart, bar))
        {
            SetState(BalanceState.NO_COMPRESSION, bar, reason + " | daily fallback fallito");
            return;
        }

        // Draw full from profileStart for context using GRAY fallback lines (macro daily/prev per transcript)
        for (int i = Math.Max(0, profileStart); i <= bar; i++)
        {
            _pocLine[i] = 0;
            _vahLine[i] = 0;
            _valLine[i] = 0;
            _pocLineFallback[i] = _poc;
            _vahLineFallback[i] = _vah;
            _valLineFallback[i] = _val;
        }
        _fallbackLevelsStart = Math.Max(0, profileStart);
        _paintBars[bar] = System.Windows.Media.Colors.DimGray; // mark current as fallback context

        SetState(BalanceState.COMPRESSION_FORMING, bar,
            $"[FALLBACK daily] {reason} | POC={_poc:F2} VAH={_vah:F2} VAL={_val:F2}");
        if (EnableLogging)
            LogSignal(bar, "FALLBACK_DAILY_PROFILE", $"startBar={profileStart} reason={reason}");
    }

    private int FindCompressionStart(int currentBar, int impulseEnd, out decimal compressionHigh, out decimal compressionLow)
    {
        compressionHigh = decimal.MinValue;
        compressionLow = decimal.MaxValue;

        var candidateStart = impulseEnd + 1;
        if (candidateStart >= currentBar)
            return -1;

        for (int i = candidateStart; i <= currentBar; i++)
        {
            var candle = GetCandle(i);
            if (candle.High > compressionHigh)
                compressionHigh = candle.High;
            if (candle.Low < compressionLow)
                compressionLow = candle.Low;
        }

        if (compressionHigh == decimal.MinValue || compressionLow == decimal.MaxValue)
            return -1;

        if (HasDirectionalExpansion(candidateStart, currentBar, compressionHigh, compressionLow))
            return -1;

        return candidateStart;
    }

    private bool HasDirectionalExpansion(int startBar, int endBar, decimal rangeHigh, decimal rangeLow)
    {
        if (endBar - startBar + 1 < MinCompressionBars)
            return false;

        int closesAbove = 0;
        int closesBelow = 0;
        decimal band = Math.Max(EffectiveTickSize * 2, (rangeHigh - rangeLow) * 0.15m);

        // Look at last 5-6 bars; require strong clustering at edge to call "directional"
        int look = Math.Min(6, endBar - startBar + 1);
        for (int i = Math.Max(startBar, endBar - look + 1); i <= endBar; i++)
        {
            var candle = GetCandle(i);
            if (candle.Close >= rangeHigh - band)
                closesAbove++;
            if (candle.Close <= rangeLow + band)
                closesBelow++;
        }

        // Relaxed: 5 out of ~6 near one edge to discard as directional
        return closesAbove >= 5 || closesBelow >= 5;
    }

    private int FindLastImpulse(int currentBar)
    {
        decimal bestScore = 0;
        int bestImpulseEnd = -1;
        int start = Math.Max(1, currentBar - CompressionLookback + 1);

        for (int i = currentBar - MinCompressionBars; i >= start; i--)
        {
            var candle = GetCandle(i);
            var prev = GetCandle(i - 1);

            var range = candle.High - candle.Low;
            if (range <= 0)
                continue;

            var prevRange = Math.Max(prev.High - prev.Low, EffectiveTickSize);
            var body = Math.Abs(candle.Close - candle.Open);
            var bodyRatio = body / range;
            var closeExtreme = Math.Abs(((candle.Close - candle.Low) / range) - 0.5m) * 2m;
            var rangeExpansion = range / prevRange;

            decimal score = bodyRatio * 0.40m + closeExtreme * 0.35m;
            if (rangeExpansion > 1.2m)
                score += Math.Min(0.25m, (rangeExpansion - 1m) * 0.20m);
            if (Math.Abs(candle.Delta) > 0)
                score += 0.05m;

            if (score > bestScore)
            {
                bestScore = score;
                bestImpulseEnd = i;
            }
        }

        return bestScore >= ImpulseMinScore ? bestImpulseEnd : -1;
    }

    private int FindImpulseStart(int impulseEnd)
    {
        if (impulseEnd <= 0)
            return 0;

        var impulseCandle = GetCandle(impulseEnd);
        var direction = Math.Sign(impulseCandle.Close - impulseCandle.Open);
        if (direction == 0)
            direction = impulseCandle.Close >= impulseCandle.Low + ((impulseCandle.High - impulseCandle.Low) / 2m) ? 1 : -1;

        int start = impulseEnd;
        int minBar = Math.Max(1, impulseEnd - 8);

        for (int i = impulseEnd - 1; i >= minBar; i--)
        {
            var candle = GetCandle(i);
            var candleDirection = Math.Sign(candle.Close - candle.Open);
            var candleRange = candle.High - candle.Low;

            if (candleRange <= 0)
                break;

            if (candleDirection == direction || Math.Abs(candle.Close - candle.Open) >= candleRange * 0.6m)
            {
                start = i;
                continue;
            }

            break;
        }

        return start;
    }

    private void CalculateImpulseRange(int impulseStart, int impulseEnd)
    {
        _impulseHigh = decimal.MinValue;
        _impulseLow = decimal.MaxValue;

        for (int i = impulseStart; i <= impulseEnd; i++)
        {
            var candle = GetCandle(i);
            if (candle.High > _impulseHigh)
                _impulseHigh = candle.High;
            if (candle.Low < _impulseLow)
                _impulseLow = candle.Low;
        }

        _impulseRange = (_impulseHigh == decimal.MinValue || _impulseLow == decimal.MaxValue)
            ? 0
            : _impulseHigh - _impulseLow;
    }

    private bool BuildProfile(int startBar, int endBar)
    {
        _priceVolume.Clear();

        if (startBar > endBar || startBar < 0)
            return false;

        for (int i = startBar; i <= endBar; i++)
        {
            var candle = GetCandle(i);
            var levels = candle.GetAllPriceLevels();
            if (levels == null)
                continue;

            foreach (var level in levels)
            {
                if (!_priceVolume.ContainsKey(level.Price))
                    _priceVolume[level.Price] = 0;

                _priceVolume[level.Price] += level.Volume;
            }
        }

        if (_priceVolume.Count == 0)
            return false;

        _poc = _priceVolume.OrderByDescending(kv => kv.Value).First().Key;
        return CalculateValueArea(out _vah, out _val);
    }

    private bool CalculateValueArea(out decimal vah, out decimal val)
    {
        vah = 0;
        val = 0;

        if (_priceVolume.Count == 0)
            return false;

        var totalVolume = _priceVolume.Values.Sum();
        var targetVolume = totalVolume * (ValueAreaPct / 100m);
        var sortedPrices = _priceVolume.Keys.OrderBy(p => p).ToList();
        int pocIndex = sortedPrices.IndexOf(_poc);

        if (pocIndex < 0)
            return false;

        decimal accumulatedVolume = _priceVolume[_poc];
        int lowIdx = pocIndex;
        int highIdx = pocIndex;

        while (accumulatedVolume < targetVolume)
        {
            decimal volBelow = lowIdx > 0 ? _priceVolume[sortedPrices[lowIdx - 1]] : -1;
            decimal volAbove = highIdx < sortedPrices.Count - 1 ? _priceVolume[sortedPrices[highIdx + 1]] : -1;

            if (volBelow < 0 && volAbove < 0)
                break;

            if (volAbove >= volBelow)
            {
                if (highIdx >= sortedPrices.Count - 1)
                    break;

                highIdx++;
                accumulatedVolume += _priceVolume[sortedPrices[highIdx]];
            }
            else
            {
                if (lowIdx <= 0)
                    break;

                lowIdx--;
                accumulatedVolume += _priceVolume[sortedPrices[lowIdx]];
            }
        }

        val = sortedPrices[lowIdx];
        vah = sortedPrices[highIdx];
        return true;
    }

    private bool IsProfileProtective(int currentBar)
    {
        if (_vah <= 0 || _val <= 0)
            return false;

        int barsToCheck = Math.Min(5, currentBar + 1);
        int breaksAbove = 0;
        int breaksBelow = 0;

        for (int i = currentBar - barsToCheck + 1; i <= currentBar; i++)
        {
            if (i < 0)
                continue;

            var candle = GetCandle(i);
            if (candle.Close > _vah)
                breaksAbove++;
            if (candle.Close < _val)
                breaksBelow++;
        }

        return breaksAbove <= 1 && breaksBelow <= 1;
    }

    private bool IsPriceInOrNearValue(decimal price)
    {
        var tolerance = EffectiveTickSize * MaxDistanceFromValueTicks;
        return price >= _val - tolerance && price <= _vah + tolerance;
    }

    private bool BrokeWithFollowThrough(int bar)
    {
        if (_vah <= 0 || _val <= 0 || FollowThroughBars <= 0)
            return false;

        if (bar + 1 < FollowThroughBars)
            return false;

        bool allAbove = true;
        bool allBelow = true;

        for (int i = bar - FollowThroughBars + 1; i <= bar; i++)
        {
            var candle = GetCandle(i);
            if (candle.Close <= _vah)
                allAbove = false;
            if (candle.Close >= _val)
                allBelow = false;
        }

        var extensionTicksAbove = (GetCandle(bar).Close - _vah) / EffectiveTickSize;
        var extensionTicksBelow = (_val - GetCandle(bar).Close) / EffectiveTickSize;

        return (allAbove && extensionTicksAbove >= 1)
            || (allBelow && extensionTicksBelow >= 1);
    }

    #endregion

    #region === Drawing ===

    private void DrawLevels(int startBar, int currentBar)
    {
        if (startBar < 0 || _vah <= 0 || _val <= 0)
            return;

        var zoneColor = _state switch
        {
            BalanceState.BALANCE_READY => System.Windows.Media.Colors.LimeGreen,
            BalanceState.COMPRESSION_FORMING => System.Windows.Media.Colors.Gold,
            BalanceState.OUT_OF_BALANCE => System.Windows.Media.Colors.OrangeRed,
            _ => System.Windows.Media.Colors.Transparent
        };

        _compressionLevelsStart = Math.Max(0, startBar);

        // Span the levels + paint the entire compression zone for visibility (flat lines + zone highlight)
        // Zero any fallback on this span so active zone owns the visual exclusively
        for (int i = Math.Max(0, startBar); i <= currentBar; i++)
        {
            _pocLine[i] = _poc;
            _vahLine[i] = _vah;
            _valLine[i] = _val;
            _paintBars[i] = zoneColor;
            _pocLineFallback[i] = 0;
            _vahLineFallback[i] = 0;
            _valLineFallback[i] = 0;
        }

        // Marker at compression start (square below low of start bar) for "da qui a qui" per transcript
        var startCandle = GetCandle(startBar);
        _compressionMarker[startBar] = startCandle.Low - (EffectiveTickSize * 3);
    }

    private void ResetActiveBalance(int bar)
    {
        int prevCompLevels = _compressionLevelsStart;
        int prevFallback = _fallbackLevelsStart;

        _compressionStartBar = -1;
        _compressionLevelsStart = -1;
        _fallbackLevelsStart = -1;
        _poc = 0;
        _vah = 0;
        _val = 0;
        _impulseHigh = 0;
        _impulseLow = 0;
        _impulseRange = 0;
        _lastProfileSnapshot = string.Empty;

        // Zero current bar for all level series (main + fallback)
        _pocLine[bar] = 0;
        _vahLine[bar] = 0;
        _valLine[bar] = 0;
        _pocLineFallback[bar] = 0;
        _vahLineFallback[bar] = 0;
        _valLineFallback[bar] = 0;
        _paintBars[bar] = System.Windows.Media.Colors.Transparent;
        _compressionMarker[bar] = 0;

        // Clear previous compression zone (main lines + paint + marker)
        if (prevCompLevels >= 0 && prevCompLevels < bar)
        {
            for (int i = prevCompLevels; i <= bar; i++)
            {
                _pocLine[i] = 0;
                _vahLine[i] = 0;
                _valLine[i] = 0;
                _paintBars[i] = System.Windows.Media.Colors.Transparent;
                _compressionMarker[i] = 0;
            }
        }

        // Clear previous fallback lines span
        if (prevFallback >= 0 && prevFallback < bar)
        {
            for (int i = prevFallback; i <= bar; i++)
            {
                _pocLineFallback[i] = 0;
                _vahLineFallback[i] = 0;
                _valLineFallback[i] = 0;
            }
        }
    }

    #endregion

    #region === State Machine ===

    private void SetState(BalanceState newState, int bar, string reason)
    {
        _stateDisplay[bar] = (decimal)newState;

        if (_state == newState)
            return;

        _state = newState;

        if (EnableLogging)
            LogSignal(bar, $"STATE → {newState}", reason);

        // If we have active levels (dynamic or fallback), ensure they are painted at least at this bar
        if (_vah > 0 && _compressionStartBar >= 0)
            DrawLevels(_compressionStartBar, bar);
    }

    private void LogProfileSnapshot(int bar, string phase, decimal rangeRatio, int compressionBars, decimal compressionHigh, decimal compressionLow)
    {
        if (!EnableLogging)
            return;

        var snapshot = string.Join("|",
            phase,
            _compressionStartBar,
            compressionBars,
            Math.Round(_poc, 2),
            Math.Round(_vah, 2),
            Math.Round(_val, 2),
            Math.Round(compressionHigh, 2),
            Math.Round(compressionLow, 2));

        if (_lastProfileSnapshot == snapshot)
            return;

        _lastProfileSnapshot = snapshot;

        LogSignal(bar, phase,
            $"compression={_compressionStartBar}-{bar} bars={compressionBars} | POC={_poc:F2} VAH={_vah:F2} VAL={_val:F2} | hi={compressionHigh:F2} lo={compressionLow:F2} | impulseRange={_impulseRange:F2} | ratio={rangeRatio:P1}");
    }

    #endregion

    #region === Logging ===

    private void InitLog()
    {
        try
        {
            Directory.CreateDirectory(LogDir);
            _logWriter = new StreamWriter(LogPath, append: true) { AutoFlush = true };
            _logWriter.WriteLine($"\n=== FabioMeanReversion STEP0+1 (compression zone) started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
        }
        catch
        {
        }
    }

    private void LogSignal(int bar, string signalType, string details)
    {
        var candle = GetCandle(bar);
        var timestamp = candle.LastTime.ToString("HH:mm:ss");
        var msg = $"[MEAN_REV] Bar={bar} | {timestamp} | {signalType} | {details}";

        System.Diagnostics.Debug.WriteLine(msg);

        try
        {
            _logWriter?.WriteLine(msg);
        }
        catch
        {
        }
    }

    #endregion
}
