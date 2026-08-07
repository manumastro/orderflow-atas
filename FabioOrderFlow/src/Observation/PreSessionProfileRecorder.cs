using System.ComponentModel;
using System.Drawing;
using System.Text.Json;
using ATAS.Indicators;
using OFT.Rendering.Context;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

[DisplayName("Fabio Pre-Session Profile Recorder")]
public sealed class PreSessionProfileRecorder : Indicator
{
    private const string Schema = "fof-pre-session-profile-v1";
    private const string SessionClockTimeZone = "America/New_York";
    private const string WindowsTimeZoneId = "Eastern Standard Time";
    private const string PrimaryWindowName = "NQ Overnight Pre-Session";
    private const string LondonWindowName = "NQ London Pre-Session";
    private const decimal ValueAreaFraction = 0.70m;
    private static readonly TimeSpan OvernightStart = new(18, 0, 0);
    private static readonly TimeSpan LondonStart = new(3, 0, 0);
    private static readonly TimeSpan CashOpen = new(9, 30, 0);
    private static readonly TimeSpan ProgressInterval = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan OpeningFiveMinutes = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan OpeningFifteenMinutes = TimeSpan.FromMinutes(15);
    private static readonly TimeZoneInfo NewYorkTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById(WindowsTimeZoneId);
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly Color PreSessionWindowColor = Color.FromArgb(24, 63, 123, 93);

    private readonly Dictionary<DateTime, CandleSnapshot> _preSessionCandles = [];
    private readonly Dictionary<DateTime, CandleSnapshot> _openingCandles = [];
    private readonly HashSet<int> _progressBuckets = [];
    private readonly ValueDataSeries _businessHighLine = CreateLineSeries(
        "pre-business-high",
        "Pre Business High",
        Color.FromArgb(220, 201, 151, 56),
        2);
    private readonly ValueDataSeries _businessLowLine = CreateLineSeries(
        "pre-business-low",
        "Pre Business Low",
        Color.FromArgb(220, 201, 151, 56),
        2);
    private readonly ValueDataSeries _pocLine = CreateLineSeries(
        "pre-poc",
        "Pre POC",
        Color.FromArgb(235, 226, 188, 79),
        2);
    private readonly ValueDataSeries _valueAreaHighLine = CreateLineSeries(
        "pre-value-high",
        "Pre VAH",
        Color.FromArgb(220, 55, 161, 153),
        1);
    private readonly ValueDataSeries _valueAreaLowLine = CreateLineSeries(
        "pre-value-low",
        "Pre VAL",
        Color.FromArgb(220, 55, 161, 153),
        1);
    private readonly ValueDataSeries _maximumPositiveDeltaLine = CreateLineSeries(
        "pre-maximum-positive-delta",
        "Pre Max +Delta",
        Color.FromArgb(215, 73, 159, 110),
        1);
    private readonly ValueDataSeries _maximumNegativeDeltaLine = CreateLineSeries(
        "pre-maximum-negative-delta",
        "Pre Max -Delta",
        Color.FromArgb(215, 193, 79, 79),
        1);
    private DateTime _targetSessionDate;
    private int? _preSessionFirstBar;
    private int? _preSessionLastBar;
    private decimal? _runningBusinessHigh;
    private decimal? _runningBusinessLow;
    private ProfileSummary? _finalPrimaryProfile;
    private bool _finalProfileComplete;
    private bool _targetInitialized;
    private bool _configurationLogged;
    private bool _finalProfileLogged;
    private bool _openingReferenceLogged;
    private bool _openingFiveLogged;
    private bool _openingFifteenLogged;
    private bool _incompleteNoticeLogged;

    public PreSessionProfileRecorder() : base(true)
    {
        Name = "Fabio Pre-Session Profile Recorder";
        Panel = IndicatorDataProvider.CandlesPanel;
        DenyToChangePanel = true;
        DrawAbovePrice = false;
        EnableCustomDrawing = true;
        SubscribeToDrawingEvents(DrawingLayouts.Historical | DrawingLayouts.LatestBar);
        DataSeries[0].IsHidden = true;
        DataSeries.Add(_businessHighLine);
        DataSeries.Add(_businessLowLine);
        DataSeries.Add(_pocLine);
        DataSeries.Add(_valueAreaHighLine);
        DataSeries.Add(_valueAreaLowLine);
        DataSeries.Add(_maximumPositiveDeltaLine);
        DataSeries.Add(_maximumNegativeDeltaLine);
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar < 0 || bar >= CurrentBar)
            return;

        EnsureTargetSession();

        var candle = GetCandle(bar);
        var beginUtc = ToUtc(candle.Time);
        var windows = GetWindows(_targetSessionDate);

        if (beginUtc >= windows.Primary.StartUtc && beginUtc < windows.Primary.EndUtc)
        {
            var snapshot = CaptureCandle(candle, beginUtc);
            _preSessionCandles[beginUtc] = snapshot;
            _preSessionFirstBar = _preSessionFirstBar is null
                ? bar
                : Math.Min(_preSessionFirstBar.Value, bar);
            _preSessionLastBar = _preSessionLastBar is null
                ? bar
                : Math.Max(_preSessionLastBar.Value, bar);
            UpdateDevelopingBusinessRange(snapshot);
            DrawDevelopingBusinessRange(bar);
            TryLogProgress(beginUtc);
            return;
        }

        if (beginUtc < windows.OpenUtc)
            return;

        if (beginUtc < windows.OpeningFifteenEndUtc)
            _openingCandles[beginUtc] = CaptureCandle(candle, beginUtc);

        TryFinalizeProfile(beginUtc);

        if (beginUtc < windows.OpeningFifteenEndUtc)
        {
            TryLogOpeningReference();
            TryLogOpeningWindow(5);
            TryLogOpeningWindow(15);
        }
    }

    protected override void OnRecalculate()
    {
        _preSessionCandles.Clear();
        _openingCandles.Clear();
        _progressBuckets.Clear();
        _preSessionFirstBar = null;
        _preSessionLastBar = null;
        _runningBusinessHigh = null;
        _runningBusinessLow = null;
        _finalPrimaryProfile = null;
        _finalProfileComplete = false;
        ClearVisuals();
        _targetInitialized = false;
        _configurationLogged = false;
        _finalProfileLogged = false;
        _openingReferenceLogged = false;
        _openingFiveLogged = false;
        _openingFifteenLogged = false;
        _incompleteNoticeLogged = false;

        base.OnRecalculate();
    }

    protected override void OnFinishRecalculate()
    {
        try
        {
            EnsureTargetSession();
            var windows = GetWindows(_targetSessionDate);

            if (_preSessionCandles.Count == 0)
            {
                TryLogIncomplete(windows, "No chart candle with footprint was observed in the target pre-session window.");
            }
            else
            {
                var latestPreSession = _preSessionCandles.Keys.Max();
                TryLogProgress(latestPreSession, force: true);

                if (GetReferenceUtc() >= windows.OpenUtc || _openingCandles.Count > 0)
                    TryFinalizeProfile(latestPreSession);
            }

            TryLogOpeningReference();
            TryLogOpeningWindow(5);
            TryLogOpeningWindow(15);
        }
        catch (Exception exception)
        {
            this.LogError("FofPreSession recalculation failed.", exception);
        }
        finally
        {
            base.OnFinishRecalculate();
        }
    }

    private void EnsureTargetSession()
    {
        if (_targetInitialized)
            return;

        var referenceUtc = GetReferenceUtc();
        var referenceNewYork = ToNewYorkTime(referenceUtc);
        var targetDate = referenceNewYork.Date;

        if (referenceNewYork.TimeOfDay >= OvernightStart || !IsWeekday(targetDate))
            targetDate = NextWeekday(targetDate);

        _targetSessionDate = targetDate;
        _targetInitialized = true;
        LogConfiguration(referenceUtc);
    }

    private void LogConfiguration(DateTime referenceUtc)
    {
        if (_configurationLogged)
            return;

        var windows = GetWindows(_targetSessionDate);
        LogObservation(new ConfigurationObservation(
            Schema,
            "configuration",
            CreateSessionId(),
            _targetSessionDate,
            SessionClockTimeZone,
            windows.Primary,
            windows.London,
            windows.OpenLocalText,
            windows.OpenUtc,
            referenceUtc,
            CaptureChart(),
            CaptureSecurity(),
            GetEffectiveTickSize(),
            "Candle timestamps are interpreted as UTC and converted to America/New_York. The recorder observes one target day only; it does not emit signals."));

        LogObservation(new RenderConfigurationObservation(
            Schema,
            "render-configuration",
            CreateSessionId(),
            _targetSessionDate,
            DateTime.UtcNow,
            Panel,
            Visible,
            DenyToChangePanel,
            DataSeries.Count,
            CaptureRenderSeries()));

        _configurationLogged = true;
    }

    private void TryLogProgress(DateTime observedUtc, bool force = false)
    {
        if (_preSessionCandles.Count == 0)
            return;

        var windows = GetWindows(_targetSessionDate);
        var elapsed = observedUtc - windows.Primary.StartUtc;
        if (elapsed < TimeSpan.Zero)
            return;

        var bucket = (int)(elapsed.Ticks / ProgressInterval.Ticks);
        if (!force && !_progressBuckets.Add(bucket))
            return;

        _progressBuckets.Add(bucket);
        LogProfileObservation(
            "profile-snapshot",
            isFinal: false,
            observedUtc,
            includeLevels: false);
    }

    private void TryFinalizeProfile(DateTime observedUtc)
    {
        if (_finalProfileLogged || _preSessionCandles.Count == 0)
            return;

        var windows = GetWindows(_targetSessionDate);
        if (GetReferenceUtc() < windows.OpenUtc && _openingCandles.Count == 0)
            return;

        var profile = BuildProfile(_preSessionCandles.Values, windows.Primary, includeLevels: false);
        var gaps = new List<string>();

        if (!profile.HasStartBoundary)
            gaps.Add("The chart did not include the 18:00 ET start boundary.");

        if (!profile.HasEndBoundary)
            gaps.Add("The chart did not include the 09:29 ET end boundary.");

        if (profile.BusinessRangeLow is null || profile.BusinessRangeHigh is null)
            gaps.Add("No nonzero footprint price levels were available in the target window.");

        _finalPrimaryProfile = profile;
        _finalProfileComplete = gaps.Count == 0;

        if (_finalProfileComplete)
            DrawFinalProfile();

        if (gaps.Count > 0)
            TryLogIncomplete(windows, string.Join(" ", gaps));

        LogProfileObservation(
            "profile-final",
            isFinal: true,
            observedUtc,
            includeLevels: true);
        _finalProfileLogged = true;
    }

    private void UpdateDevelopingBusinessRange(CandleSnapshot candle)
    {
        if (candle.Levels.Count == 0)
            return;

        var high = candle.Levels.Max(level => level.Price);
        var low = candle.Levels.Min(level => level.Price);
        _runningBusinessHigh = _runningBusinessHigh is null
            ? high
            : Math.Max(_runningBusinessHigh.Value, high);
        _runningBusinessLow = _runningBusinessLow is null
            ? low
            : Math.Min(_runningBusinessLow.Value, low);
    }

    private void DrawDevelopingBusinessRange(int bar)
    {
        if (_runningBusinessHigh is null || _runningBusinessLow is null)
            return;

        SetLevel(_businessHighLine, bar, _runningBusinessHigh);
        SetLevel(_businessLowLine, bar, _runningBusinessLow);
    }

    private void DrawFinalProfile()
    {
        if (!_finalProfileComplete || _finalPrimaryProfile is null ||
            _preSessionFirstBar is null || _preSessionLastBar is null)
            return;

        var profile = _finalPrimaryProfile;
        for (var bar = _preSessionFirstBar.Value; bar <= _preSessionLastBar.Value; bar++)
        {
            SetLevel(_businessHighLine, bar, profile.BusinessRangeHigh);
            SetLevel(_businessLowLine, bar, profile.BusinessRangeLow);
            SetLevel(_pocLine, bar, profile.ValueAreaSeedPoc);
            SetLevel(_valueAreaHighLine, bar, profile.ValueAreaHigh);
            SetLevel(_valueAreaLowLine, bar, profile.ValueAreaLow);
            SetLevel(_maximumPositiveDeltaLine, bar, GetExtremePrice(profile.MaxPositiveDelta));
            SetLevel(_maximumNegativeDeltaLine, bar, GetExtremePrice(profile.MaxNegativeDelta));
        }
    }

    protected override void OnRender(RenderContext context, DrawingLayouts layout)
    {
        if (_preSessionFirstBar is null || _preSessionLastBar is null)
            return;

        var firstVisibleBar = Math.Max(_preSessionFirstBar.Value, FirstVisibleBarNumber);
        var lastVisibleBar = Math.Min(_preSessionLastBar.Value, LastVisibleBarNumber);
        if (firstVisibleBar > lastVisibleBar)
            return;

        var container = Container;
        var chart = ChartInfo;
        if (container is null || chart is null)
            return;

        var region = container.Region;
        var left = Math.Max(region.Left, chart.GetXByBar(firstVisibleBar));
        var right = Math.Min(
            region.Right,
            (int)(chart.GetXByBar(lastVisibleBar) + chart.PriceChartContainer.BarsWidth));
        if (right <= left || region.Height <= 0)
            return;

        context.FillRectangle(
            PreSessionWindowColor,
            new Rectangle(left, region.Top, right - left, region.Height));
    }

    private static decimal? GetExtremePrice(DeltaExtremeSummary? extreme) =>
        extreme?.Levels.FirstOrDefault()?.Price;

    private static void SetLevel(ValueDataSeries series, int bar, decimal? level)
    {
        if (level is not null)
            series[bar] = level.Value;
    }

    private void ClearVisuals()
    {
        _businessHighLine.Clear();
        _businessLowLine.Clear();
        _pocLine.Clear();
        _valueAreaHighLine.Clear();
        _valueAreaLowLine.Clear();
        _maximumPositiveDeltaLine.Clear();
        _maximumNegativeDeltaLine.Clear();
    }

    private static ValueDataSeries CreateLineSeries(
        string id,
        string name,
        Color color,
        int width) =>
        new(id, name)
        {
            VisualType = VisualMode.Line,
            RenderColor = color,
            Width = width,
            ShowCurrentValue = true,
            ShowOnlyNonZeroLabels = true,
            ScaleIt = false,
            DrawAbovePrice = true,
            IgnoredByAlerts = true
        };

    private void TryLogOpeningReference()
    {
        if (_openingReferenceLogged || !_finalProfileLogged || _openingCandles.Count == 0)
            return;

        var windows = GetWindows(_targetSessionDate);
        var primary = BuildProfile(_preSessionCandles.Values, windows.Primary, includeLevels: false);
        var first = _openingCandles.Values.OrderBy(candle => candle.BeginUtc).First();
        var opening = new OpeningReferenceObservation(
            Schema,
            "opening-reference",
            CreateSessionId(),
            _targetSessionDate,
            windows.OpenLocalText,
            windows.OpenUtc,
            first,
            BuildLocation(first.Open, primary),
            DateTime.UtcNow);

        LogObservation(opening);
        _openingReferenceLogged = true;
    }

    private void TryLogOpeningWindow(int minutes)
    {
        if (!_finalProfileLogged || _openingCandles.Count == 0)
            return;

        var alreadyLogged = minutes == 5 ? _openingFiveLogged : _openingFifteenLogged;
        if (alreadyLogged)
            return;

        var windows = GetWindows(_targetSessionDate);
        var duration = TimeSpan.FromMinutes(minutes);
        var requiredLastBar = windows.OpenUtc + duration - TimeSpan.FromMinutes(1);

        if (!_openingCandles.Keys.Any(time => time >= requiredLastBar))
            return;

        var bars = _openingCandles.Values
            .Where(candle => candle.BeginUtc >= windows.OpenUtc
                && candle.BeginUtc < windows.OpenUtc + duration)
            .OrderBy(candle => candle.BeginUtc)
            .ToArray();

        if (bars.Length == 0)
            return;

        var first = bars[0];
        var last = bars[^1];
        var high = bars.Max(candle => candle.High);
        var low = bars.Min(candle => candle.Low);
        var volume = bars.Sum(candle => candle.Volume);
        var bid = bars.Sum(candle => candle.Bid);
        var ask = bars.Sum(candle => candle.Ask);
        var delta = bars.Sum(candle => candle.Delta);
        var primary = BuildProfile(_preSessionCandles.Values, windows.Primary, includeLevels: false);
        var rangeLow = primary.BusinessRangeLow;
        var rangeHigh = primary.BusinessRangeHigh;
        var valueLow = primary.ValueAreaLow;
        var valueHigh = primary.ValueAreaHigh;
        var closesInsideBusinessRange = rangeLow is not null
            && rangeHigh is not null
            && last.Close >= rangeLow.Value
            && last.Close <= rangeHigh.Value;
        var closesInsideValueArea = valueLow is not null
            && valueHigh is not null
            && last.Close >= valueLow.Value
            && last.Close <= valueHigh.Value;

        LogObservation(new OpeningWindowObservation(
            Schema,
            "opening-window",
            CreateSessionId(),
            _targetSessionDate,
            minutes,
            windows.OpenLocalText,
            first.BeginUtc,
            last.LastUtc,
            bars.Length,
            first.Open,
            high,
            low,
            last.Close,
            volume,
            bid,
            ask,
            delta,
            BuildLocation(last.Close, primary),
            rangeLow is not null && low < rangeLow.Value,
            rangeHigh is not null && high > rangeHigh.Value,
            closesInsideBusinessRange,
            closesInsideValueArea,
            DateTime.UtcNow));

        if (minutes == 5)
            _openingFiveLogged = true;
        else
            _openingFifteenLogged = true;
    }

    private void LogProfileObservation(
        string type,
        bool isFinal,
        DateTime observedUtc,
        bool includeLevels)
    {
        var windows = GetWindows(_targetSessionDate);
        var primary = BuildProfile(_preSessionCandles.Values, windows.Primary, includeLevels);
        var londonCandles = _preSessionCandles.Values.Where(candle =>
            candle.BeginUtc >= windows.London.StartUtc
            && candle.BeginUtc < windows.London.EndUtc);
        var london = BuildProfile(londonCandles, windows.London, includeLevels: false);

        LogObservation(new ProfileObservation(
            Schema,
            type,
            CreateSessionId(),
            _targetSessionDate,
            isFinal,
            observedUtc,
            DateTime.UtcNow,
            SessionClockTimeZone,
            windows.Primary,
            windows.London,
            windows.OpenLocalText,
            windows.OpenUtc,
            CaptureChart(),
            CaptureSecurity(),
            GetEffectiveTickSize(),
            primary,
            london));
    }

    private void TryLogIncomplete(SessionWindows windows, string reason)
    {
        if (_incompleteNoticeLogged)
            return;

        if (GetReferenceUtc() < windows.OpenUtc)
            return;

        LogObservation(new IncompleteObservation(
            Schema,
            "incomplete",
            CreateSessionId(),
            _targetSessionDate,
            windows.Primary,
            windows.OpenUtc,
            CaptureChart(),
            CaptureSecurity(),
            reason,
            DateTime.UtcNow));
        _incompleteNoticeLogged = true;
    }

    private ProfileSummary BuildProfile(
        IEnumerable<CandleSnapshot> source,
        SessionWindow window,
        bool includeLevels)
    {
        var candles = source
            .Where(candle => candle.BeginUtc >= window.StartUtc && candle.BeginUtc < window.EndUtc)
            .OrderBy(candle => candle.BeginUtc)
            .ToArray();
        var levels = new SortedDictionary<decimal, AggregateLevel>();

        foreach (var candle in candles)
        {
            foreach (var level in candle.Levels)
            {
                if (!levels.TryGetValue(level.Price, out var aggregate))
                {
                    aggregate = new AggregateLevel(level.Price);
                    levels.Add(level.Price, aggregate);
                }

                aggregate.Add(level);
            }
        }

        if (levels.Count == 0)
            return ProfileSummary.Empty(candles, window, includeLevels);

        var ordered = levels.Values.ToArray();
        var totalVolume = ordered.Sum(level => level.Volume);
        var totalBid = ordered.Sum(level => level.Bid);
        var totalAsk = ordered.Sum(level => level.Ask);
        var totalBetween = ordered.Sum(level => level.Between);
        var totalDelta = ordered.Sum(level => level.Delta);
        var rangeLow = ordered[0].Price;
        var rangeHigh = ordered[^1].Price;
        var maximumVolume = ordered.Max(level => level.Volume);
        var pocLevels = ordered.Where(level => level.Volume == maximumVolume).ToArray();
        var seedIndex = Array.FindIndex(ordered, level => level.Price == pocLevels[0].Price);
        var valueArea = CalculateValueArea(ordered, seedIndex, totalVolume);
        var maxPositive = GetDeltaExtreme(ordered, positive: true);
        var maxNegative = GetDeltaExtreme(ordered, positive: false);
        var distribution = CalculateDistribution(ordered, pocLevels[0].Price, rangeLow, rangeHigh, totalVolume);

        return new ProfileSummary(
            candles.Length,
            candles.Length == 0 ? null : candles[0].BeginUtc,
            candles.Length == 0 ? null : candles[^1].LastUtc,
            HasBoundary(candles, window.StartUtc, isStart: true),
            HasBoundary(candles, window.EndUtc, isStart: false),
            rangeLow,
            rangeHigh,
            (rangeLow + rangeHigh) / 2m,
            GetTickDistance(rangeHigh - rangeLow),
            totalVolume,
            totalBid,
            totalAsk,
            totalBetween,
            totalDelta,
            pocLevels.Select(level => level.Price).ToArray(),
            maximumVolume,
            valueArea.Low,
            valueArea.High,
            valueArea.CoveredFraction,
            pocLevels[0].Price,
            maxPositive,
            maxNegative,
            distribution,
            includeLevels
                ? ordered.Select(ToProfileLevelSnapshot).ToArray()
                : null,
            candles.Sum(candle => candle.Volume),
            candles.Sum(candle => candle.Bid),
            candles.Sum(candle => candle.Ask),
            candles.Sum(candle => candle.Delta));
    }

    private static ProfileLevelSnapshot ToProfileLevelSnapshot(AggregateLevel level) =>
        new(
            level.Price,
            level.Volume,
            level.Bid,
            level.Ask,
            level.Between,
            level.Delta,
            level.Ticks);

    private static ValueAreaResult CalculateValueArea(
        IReadOnlyList<AggregateLevel> levels,
        int seedIndex,
        decimal totalVolume)
    {
        if (totalVolume <= 0)
            return new ValueAreaResult(null, null, 0m);

        var low = seedIndex;
        var high = seedIndex;
        var included = levels[seedIndex].Volume;
        var target = totalVolume * ValueAreaFraction;

        while (included < target && (low > 0 || high < levels.Count - 1))
        {
            var leftVolume = low > 0 ? levels[low - 1].Volume : decimal.MinValue;
            var rightVolume = high < levels.Count - 1 ? levels[high + 1].Volume : decimal.MinValue;

            if (rightVolume > leftVolume)
            {
                high++;
                included += levels[high].Volume;
            }
            else if (leftVolume > rightVolume)
            {
                low--;
                included += levels[low].Volume;
            }
            else
            {
                if (low > 0)
                {
                    low--;
                    included += levels[low].Volume;
                }

                if (included < target && high < levels.Count - 1)
                {
                    high++;
                    included += levels[high].Volume;
                }
            }
        }

        return new ValueAreaResult(
            levels[low].Price,
            levels[high].Price,
            included / totalVolume);
    }

    private static DeltaExtremeSummary? GetDeltaExtreme(
        IReadOnlyList<AggregateLevel> levels,
        bool positive)
    {
        var candidates = levels
            .Where(level => positive ? level.Delta > 0 : level.Delta < 0)
            .ToArray();

        if (candidates.Length == 0)
            return null;

        var extreme = positive
            ? candidates.Max(level => level.Delta)
            : candidates.Min(level => level.Delta);
        var selected = candidates.Where(level => level.Delta == extreme).ToArray();

        return new DeltaExtremeSummary(
            selected.Select(ToProfileLevelSnapshot).ToArray());
    }

    private static DistributionSummary CalculateDistribution(
        IReadOnlyList<AggregateLevel> levels,
        decimal pocPrice,
        decimal rangeLow,
        decimal rangeHigh,
        decimal totalVolume)
    {
        if (totalVolume <= 0)
            return DistributionSummary.Empty;

        var below = levels.Where(level => level.Price < pocPrice).Sum(level => level.Volume);
        var at = levels.Where(level => level.Price == pocPrice).Sum(level => level.Volume);
        var above = levels.Where(level => level.Price > pocPrice).Sum(level => level.Volume);
        var width = rangeHigh - rangeLow;
        var firstCut = rangeLow + width / 3m;
        var secondCut = rangeLow + width * 2m / 3m;
        var lower = levels.Where(level => level.Price < firstCut).Sum(level => level.Volume);
        var middle = levels.Where(level => level.Price >= firstCut && level.Price < secondCut).Sum(level => level.Volume);
        var upper = levels.Where(level => level.Price >= secondCut).Sum(level => level.Volume);

        return new DistributionSummary(
            below / totalVolume,
            at / totalVolume,
            above / totalVolume,
            lower / totalVolume,
            middle / totalVolume,
            upper / totalVolume);
    }

    private OpeningLocation BuildLocation(decimal price, ProfileSummary profile)
    {
        var rangeLow = profile.BusinessRangeLow;
        var rangeHigh = profile.BusinessRangeHigh;
        var valueLow = profile.ValueAreaLow;
        var valueHigh = profile.ValueAreaHigh;
        var pocDistances = profile.PocPrices
            .Select(poc => GetTickDistance(price - poc))
            .ToArray();

        if (rangeLow is null || rangeHigh is null)
        {
            return new OpeningLocation(
                price,
                null,
                null,
                null,
                null,
                pocDistances,
                "unavailable",
                "unavailable");
        }

        return new OpeningLocation(
            price,
            GetTickDistance(price - rangeLow.Value),
            GetTickDistance(rangeHigh.Value - price),
            valueLow is null ? null : GetTickDistance(price - valueLow.Value),
            valueHigh is null ? null : GetTickDistance(valueHigh.Value - price),
            pocDistances,
            ClassifyRangePosition(price, rangeLow.Value, rangeHigh.Value),
            valueLow is null || valueHigh is null
                ? "unavailable"
                : ClassifyValuePosition(price, valueLow.Value, valueHigh.Value));
    }

    private decimal? GetTickDistance(decimal difference)
    {
        var tickSize = GetEffectiveTickSize();
        return tickSize <= 0 ? null : difference / tickSize;
    }

    private static string ClassifyRangePosition(decimal price, decimal low, decimal high) =>
        price < low ? "below" : price > high ? "above" : "inside";

    private static string ClassifyValuePosition(decimal price, decimal low, decimal high) =>
        price < low ? "below-value" : price > high ? "above-value" : "inside-value";

    private CandleSnapshot CaptureCandle(IndicatorCandle candle, DateTime beginUtc)
    {
        var levels = candle.GetAllPriceLevels()
            .Where(level => level is not null && level.Volume != 0)
            .Select(level => new ProfileLevelSnapshot(
                level!.Price,
                level.Volume,
                level.Bid,
                level.Ask,
                level.Between,
                level.Ask - level.Bid,
                level.Ticks))
            .ToArray();

        return new CandleSnapshot(
            candle.Time,
            candle.LastTime,
            beginUtc,
            ToUtc(candle.LastTime),
            candle.Open,
            candle.High,
            candle.Low,
            candle.Close,
            candle.Volume,
            candle.Bid,
            candle.Ask,
            candle.Delta,
            candle.VWAP,
            candle.Ticks,
            levels);
    }

    private static bool HasBoundary(
        IReadOnlyList<CandleSnapshot> candles,
        DateTime boundaryUtc,
        bool isStart)
    {
        if (candles.Count == 0)
            return false;

        var tolerance = TimeSpan.FromMinutes(1);
        return isStart
            ? candles.Any(candle => candle.BeginUtc >= boundaryUtc && candle.BeginUtc < boundaryUtc + tolerance)
            : candles.Any(candle => candle.BeginUtc >= boundaryUtc - tolerance && candle.BeginUtc < boundaryUtc);
    }

    private SessionWindows GetWindows(DateTime sessionDate)
    {
        var primaryStartLocal = sessionDate.Date.AddDays(-1).Add(OvernightStart);
        var londonStartLocal = sessionDate.Date.Add(LondonStart);
        var openLocal = sessionDate.Date.Add(CashOpen);
        var primaryEndLocal = openLocal;

        return new SessionWindows(
            new SessionWindow(
                PrimaryWindowName,
                ToLocalText(primaryStartLocal),
                ToLocalText(primaryEndLocal),
                ToUtcFromNewYork(primaryStartLocal),
                ToUtcFromNewYork(primaryEndLocal)),
            new SessionWindow(
                LondonWindowName,
                ToLocalText(londonStartLocal),
                ToLocalText(openLocal),
                ToUtcFromNewYork(londonStartLocal),
                ToUtcFromNewYork(openLocal)),
            ToLocalText(openLocal),
            ToUtcFromNewYork(openLocal));
    }

    private string CreateSessionId() =>
        $"{_targetSessionDate:yyyyMMdd}-NQ-pre-session";

    private ChartSnapshot CaptureChart() =>
        new(
            ChartInfo?.ChartType,
            ChartInfo?.TimeFrame,
            CurrentBar,
            "1-minute chart with footprint levels is required for exact boundary handling.");

    private SecuritySnapshot CaptureSecurity()
    {
        var security = TradingManager?.Security;
        return new SecuritySnapshot(
            security?.SecurityId,
            security?.ConnectorId,
            security?.Code,
            security?.Instrument,
            security?.Exchange,
            security?.TickSize ?? InstrumentInfo?.TickSize);
    }

    private decimal GetEffectiveTickSize()
    {
        var securityTickSize = TradingManager?.Security?.TickSize;
        if (securityTickSize is > 0)
            return securityTickSize.Value;

        var instrumentTickSize = InstrumentInfo?.TickSize;
        if (instrumentTickSize is > 0)
            return instrumentTickSize.Value;

        return 0m;
    }

    private DateTime GetReferenceUtc()
    {
        try
        {
            var providerTime = UtcTime;
            if (providerTime != default)
                return ToUtc(providerTime);
        }
        catch
        {
            // The provider may not be attached while the indicator is being constructed.
        }

        return DateTime.UtcNow;
    }

    private static DateTime ToUtc(DateTime time)
    {
        var utc = time.Kind == DateTimeKind.Utc
            ? time
            : DateTime.SpecifyKind(time, DateTimeKind.Utc);
        return utc;
    }

    private static DateTime ToNewYorkTime(DateTime utcTime) =>
        TimeZoneInfo.ConvertTimeFromUtc(ToUtc(utcTime), NewYorkTimeZone);

    private static DateTime ToUtcFromNewYork(DateTime localTime)
    {
        var unspecified = DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, NewYorkTimeZone);
    }

    private static string ToLocalText(DateTime localTime) =>
        localTime.ToString("yyyy-MM-dd HH:mm:ss");

    private static bool IsWeekday(DateTime date) =>
        date.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday;

    private static DateTime NextWeekday(DateTime date)
    {
        var next = date.Date.AddDays(1);
        while (!IsWeekday(next))
            next = next.AddDays(1);
        return next;
    }

    private IReadOnlyList<RenderSeriesSnapshot> CaptureRenderSeries() =>
    [
        CaptureLineSeries(_businessHighLine),
        CaptureLineSeries(_businessLowLine),
        CaptureLineSeries(_pocLine),
        CaptureLineSeries(_valueAreaHighLine),
        CaptureLineSeries(_valueAreaLowLine),
        CaptureLineSeries(_maximumPositiveDeltaLine),
        CaptureLineSeries(_maximumNegativeDeltaLine)
    ];

    private static RenderSeriesSnapshot CaptureLineSeries(ValueDataSeries series) =>
        new(
            series.Id,
            series.Name,
            series.IsHidden,
            series.IsVisible,
            null,
            series.ScaleIt,
            series.DrawAbovePrice);

    private void LogObservation<T>(T observation)
    {
        try
        {
            this.LogInfo("FofPreSession {0}", JsonSerializer.Serialize(observation, JsonOptions));
        }
        catch (Exception exception)
        {
            this.LogError("FofPreSession serialization failed.", exception);
        }
    }

    private sealed class AggregateLevel(decimal price)
    {
        public decimal Price { get; } = price;
        public decimal Volume { get; private set; }
        public decimal Bid { get; private set; }
        public decimal Ask { get; private set; }
        public decimal Between { get; private set; }
        public int Ticks { get; private set; }
        public decimal Delta => Ask - Bid;

        public void Add(ProfileLevelSnapshot level)
        {
            Volume += level.Volume;
            Bid += level.Bid;
            Ask += level.Ask;
            Between += level.Between;
            Ticks += level.Ticks;
        }
    }

    private sealed record SessionWindows(
        SessionWindow Primary,
        SessionWindow London,
        string OpenLocalText,
        DateTime OpenUtc)
    {
        public DateTime OpeningFifteenEndUtc => OpenUtc + OpeningFifteenMinutes;
    }

    private sealed record SessionWindow(
        string Name,
        string StartLocalText,
        string EndLocalText,
        DateTime StartUtc,
        DateTime EndUtc);

    private sealed record CandleSnapshot(
        DateTime RawBeginTime,
        DateTime RawLastTime,
        DateTime BeginUtc,
        DateTime LastUtc,
        decimal Open,
        decimal High,
        decimal Low,
        decimal Close,
        decimal Volume,
        decimal Bid,
        decimal Ask,
        decimal Delta,
        decimal VWAP,
        decimal Ticks,
        IReadOnlyList<ProfileLevelSnapshot> Levels);

    private sealed record ProfileLevelSnapshot(
        decimal Price,
        decimal Volume,
        decimal Bid,
        decimal Ask,
        decimal Between,
        decimal Delta,
        int Ticks);

    private sealed record ValueAreaResult(
        decimal? Low,
        decimal? High,
        decimal CoveredFraction);

    private sealed record ProfileSummary(
        int CandleCount,
        DateTime? FirstCandleUtc,
        DateTime? LastCandleUtc,
        bool HasStartBoundary,
        bool HasEndBoundary,
        decimal? BusinessRangeLow,
        decimal? BusinessRangeHigh,
        decimal? BusinessRangeMidpoint,
        decimal? BusinessRangeTicks,
        decimal FootprintVolume,
        decimal FootprintBid,
        decimal FootprintAsk,
        decimal FootprintBetween,
        decimal FootprintDelta,
        IReadOnlyList<decimal> PocPrices,
        decimal? PocVolume,
        decimal? ValueAreaLow,
        decimal? ValueAreaHigh,
        decimal ValueAreaCoveredFraction,
        decimal? ValueAreaSeedPoc,
        DeltaExtremeSummary? MaxPositiveDelta,
        DeltaExtremeSummary? MaxNegativeDelta,
        DistributionSummary Distribution,
        IReadOnlyList<ProfileLevelSnapshot>? Levels,
        decimal CandleVolume,
        decimal CandleBid,
        decimal CandleAsk,
        decimal CandleDelta)
    {
        public static ProfileSummary Empty(
            IReadOnlyList<CandleSnapshot> candles,
            SessionWindow window,
            bool includeLevels) =>
            new(
                candles.Count,
                candles.Count == 0 ? null : candles[0].BeginUtc,
                candles.Count == 0 ? null : candles[^1].LastUtc,
                HasBoundary(candles, window.StartUtc, isStart: true),
                HasBoundary(candles, window.EndUtc, isStart: false),
                null,
                null,
                null,
                null,
                0m,
                0m,
                0m,
                0m,
                0m,
                Array.Empty<decimal>(),
                null,
                null,
                null,
                0m,
                null,
                null,
                null,
                DistributionSummary.Empty,
                includeLevels ? Array.Empty<ProfileLevelSnapshot>() : null,
                candles.Sum(candle => candle.Volume),
                candles.Sum(candle => candle.Bid),
                candles.Sum(candle => candle.Ask),
                candles.Sum(candle => candle.Delta));
    }

    private sealed record DeltaExtremeSummary(
        IReadOnlyList<ProfileLevelSnapshot> Levels);

    private sealed record DistributionSummary(
        decimal VolumeBelowPocFraction,
        decimal VolumeAtPocFraction,
        decimal VolumeAbovePocFraction,
        decimal LowerThirdFraction,
        decimal MiddleThirdFraction,
        decimal UpperThirdFraction)
    {
        public static DistributionSummary Empty => new(0m, 0m, 0m, 0m, 0m, 0m);
    }

    private sealed record OpeningLocation(
        decimal Price,
        decimal? DistanceFromBusinessLowTicks,
        decimal? DistanceToBusinessHighTicks,
        decimal? DistanceFromValueLowTicks,
        decimal? DistanceToValueHighTicks,
        IReadOnlyList<decimal?> DistanceFromPocTicks,
        string BusinessRangePosition,
        string ValueAreaPosition);

    private sealed record ChartSnapshot(
        string? ChartType,
        string? TimeFrame,
        int LoadedBarCount,
        string Requirement);

    private sealed record SecuritySnapshot(
        string? SecurityId,
        string? ConnectorId,
        string? Code,
        string? Instrument,
        string? Exchange,
        decimal? TickSize);

    private sealed record ConfigurationObservation(
        string Schema,
        string Type,
        string SessionId,
        DateTime SessionDate,
        string ClockTimeZone,
        SessionWindow PrimaryWindow,
        SessionWindow LondonWindow,
        string OpenLocal,
        DateTime OpenUtc,
        DateTime ReferenceUtc,
        ChartSnapshot Chart,
        SecuritySnapshot Security,
        decimal TickSize,
        string Detail);

    private sealed record RenderConfigurationObservation(
        string Schema,
        string Type,
        string SessionId,
        DateTime SessionDate,
        DateTime ReceivedAtUtc,
        string Panel,
        bool IndicatorVisible,
        bool DenyToChangePanel,
        int DataSeriesCount,
        IReadOnlyList<RenderSeriesSnapshot> Series);

    private sealed record RenderSeriesSnapshot(
        string Id,
        string Name,
        bool IsHidden,
        bool IsVisible,
        bool? RangeVisible,
        bool ScaleIt,
        bool DrawAbovePrice);

    private sealed record ProfileObservation(
        string Schema,
        string Type,
        string SessionId,
        DateTime SessionDate,
        bool IsFinal,
        DateTime ObservedUtc,
        DateTime ReceivedAtUtc,
        string ClockTimeZone,
        SessionWindow PrimaryWindow,
        SessionWindow LondonWindow,
        string OpenLocal,
        DateTime OpenUtc,
        ChartSnapshot Chart,
        SecuritySnapshot Security,
        decimal TickSize,
        ProfileSummary Primary,
        ProfileSummary London);

    private sealed record OpeningReferenceObservation(
        string Schema,
        string Type,
        string SessionId,
        DateTime SessionDate,
        string OpenLocal,
        DateTime OpenUtc,
        CandleSnapshot FirstCashCandle,
        OpeningLocation Location,
        DateTime ReceivedAtUtc);

    private sealed record OpeningWindowObservation(
        string Schema,
        string Type,
        string SessionId,
        DateTime SessionDate,
        int WindowMinutes,
        string OpenLocal,
        DateTime FirstCandleUtc,
        DateTime LastCandleUtc,
        int CandleCount,
        decimal Open,
        decimal High,
        decimal Low,
        decimal Close,
        decimal Volume,
        decimal Bid,
        decimal Ask,
        decimal Delta,
        OpeningLocation CloseLocation,
        bool BrokeBusinessLow,
        bool BrokeBusinessHigh,
        bool ClosedInsideBusinessRange,
        bool ClosedInsideValueArea,
        DateTime ReceivedAtUtc);

    private sealed record IncompleteObservation(
        string Schema,
        string Type,
        string SessionId,
        DateTime SessionDate,
        SessionWindow PrimaryWindow,
        DateTime OpenUtc,
        ChartSnapshot Chart,
        SecuritySnapshot Security,
        string Reason,
        DateTime ReceivedAtUtc);
}
