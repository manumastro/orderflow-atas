# Dataseries

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20050__Dataseries.html

# ValueDataSeries - numerical data

The [ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md) is designed for storing the decimal data for each bar.

 It is added to each indicator by default.

 It has the following properties:

- [VisualType](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#a653b3a68b914e1a5a591f92d90e7aa3a) - visual layout. It could be, for example, a line, bar chart, arrow or something else.

- [Width](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#af7b58a3626c12088b203aa96a2d58449) - width of the line, point, bar chart and so on.

- [Color](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#afc4f2cfe3d9905c0b17f1f9f8253b879) - color.

- [LineDashStyle](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#a1a4cde304137cc6e6bc05f4088bcb16e) - line style.

- [ScaleIt](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#afa31f6a4e94d7dcef6dc0d9bed2a1910) - a flag that regulates the indicator auto-scale.

- [ShowCurrentValue](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#ae907ee964811c528eeda3c44b31080f2) - a flag that regulates display of the most recent (current) value.

- [ShowZeroValue](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#a81709dd6712b23f9bbabce016f0a0f0e) - a flag that regulates display of zero values.

- [Digits](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#a8d1a7b389cf0d90f278d5b1c94b51424) - a number of decimal places for displaying the DataSeries values in the price scale.

This DataSeries has next methods:

- [SetPointOfEndLine(int bar)](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#a36e03449209e39e11ee5c42b5b863548) - allows setting the line interruption point.

- [RemovePointOfEndLine(int bar)](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#aafa4adfd4afe5ba2eb11cf2c7b166de1) - removes the specified bar index as a point of end for a line in the value data series.

- [IsThisPointOfStartBar(int bar)](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#a338c720c7e369f069f8b2f38c7e8831e) - checks if the specified bar index is a point of start for a line in the value data series.

# RangeDataSeries - ranges

The [RangeDataSeries](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md), each element of which represents [RangeValue](../api/classes/classATAS_1_1Indicators_1_1RangeValue.md), where the minimum and maximum values are set.

Main properties:

- [RangeColor](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md#a52f41423829661aa1e252ca9b66c1841) - range area color.

- [ScaleIt](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md#afb335b3d325b420456909dc7826bf584) - a flag that regulates the indicator auto-scale.

- [Visible](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md#ac7361468c74a4a2f69cb197b5bff282a) - a flag that regulates the DataSeries visibility.

Example of the Bollinger Bands indicator that uses [RangeDataSeries](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md).

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class BollingerBands : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private readonly [RangeDataSeries](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md) _band = new [RangeDataSeries](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md)("BackGround");

 private readonly StdDev _dev = new StdDev();

 private readonly SMA _sma = new SMA();

 private decimal _width;

 public int Period

 {

 get => _sma.Period;

 set

 {

 if (value <= 0)

 return;

 _sma.Period = _dev.Period = value;

 RecalculateValues();

 }

 }

 public decimal Width

 {

 get => _width;

 set

 {

 if (value <= 0)

 return;

 _width = value;

 RecalculateValues();

 }

 }

 public BollingerBands()

 {

 (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[0]).Color = Colors.Green;

 DataSeries.Add(new [ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md)("Up")

 {

 VisualType = VisualMode.Line

 });

 DataSeries.Add(new [ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md)("Down")

 {

 VisualType = VisualMode.Line

 });

 DataSeries.Add(_band);

 Period = 10;

 Width = 1;

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var sma = _sma.Calculate(bar, value);

 var dev = _dev.Calculate(bar, value);

 this[bar] = sma;

 DataSeries[1][bar] = sma + dev * Width;

 DataSeries[2][bar] = sma - dev * Width;

 _band[bar].Upper = sma + dev * Width;

 _band[bar].Lower = sma - dev * Width;

 }

}

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

[ATAS.Indicators.RangeDataSeries](../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md)

Represents a data series of range values, each element is a RangeValue.

Definition RangeDataSeries.cs:23

[ATAS.Indicators.ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md)

Represents a data series of decimal values, each element is a decimal.

Definition ValueDataSeries.cs:26

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2

# PaintbarsDataSeries - bar colours

The [PaintbarsDataSeries](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) which allows setting colour for each bar. Each element of this DataSeries is a nullable Color.

 It has one property [Visible](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md#aefcf28d41389c9d85a5e966e737d0b78) which regulates the DataSeries visibility.

 Example of the HeikenAshi indicator, which uses this DataSeries. The [PaintbarsDataSeries](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) performs here the function of hiding standard bars with the help of setting a transparent colour.

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class HeikenAshi : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 readonly [CandleDataSeries](../api/classes/classATAS_1_1Indicators_1_1CandleDataSeries.md) _candles = new [CandleDataSeries](../api/classes/classATAS_1_1Indicators_1_1CandleDataSeries.md)("Heiken Ashi");

 readonly [PaintbarsDataSeries](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) _bars = new [PaintbarsDataSeries](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md)("Bars"){ IsHidden = true };

 public HeikenAshi() : base(true)

 {

 DenyToChangePanel = true;

 DataSeries[0] = _bars;

 DataSeries.Add(_candles);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var candle = GetCandle(bar);

 _bars[bar] = Colors.Transparent;

 if (bar == 0)

 {

 _candles[bar] = new [Candle](../api/classes/classATAS_1_1Indicators_1_1Candle.md)()

 {

 [Close](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dad3d2e617335f08df83599665eef8a418) = candle.Close,

 [High](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da655d20c1ca69519ca647684edbb2db35) = candle.High,

 [Low](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da28d0edd045e05cf5af64e35ae0c4c6ef) = candle.Low,

 [Open](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dac3bf447eabe632720a3aa1a7ce401274) = candle.Open

 };

 }

 else

 {

 var prevCandle = _candles[bar - 1];

 var close = (candle.Open + candle.Close + candle.High + candle.Low) * 0.25m;

 var open = (prevCandle.Open + prevCandle.Close) * 0.5m;

 var high = Math.Max(Math.Max(close, open), candle.High);

 var low = Math.Min(Math.Min(close, open), candle.Low);

 _candles[bar] = new [Candle](../api/classes/classATAS_1_1Indicators_1_1Candle.md)()

 {

 [Close](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dad3d2e617335f08df83599665eef8a418) = close,

 [High](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da655d20c1ca69519ca647684edbb2db35) = high,

 [Low](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da28d0edd045e05cf5af64e35ae0c4c6ef) = low,

 [Open](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dac3bf447eabe632720a3aa1a7ce401274) = open,

 };

 }

 }

}

[ATAS.Indicators.CandleDataSeries](../api/classes/classATAS_1_1Indicators_1_1CandleDataSeries.md)

Represents a data series of candles. Each element in the series is a Candle.

Definition CandleDataSeries.cs:22

[ATAS.Indicators.Candle](../api/classes/classATAS_1_1Indicators_1_1Candle.md)

Represents a candle in trading, which includes open, high, low, and close prices.

Definition Candle.cs:7

[ATAS.Indicators.PaintbarsDataSeries](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md)

Represents a data series of paintbars, each element is a nullable CrossColor value.

Definition PaintbarsDataSeries.cs:19

[ATAS.Indicators.DataSeriesType.Low](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da28d0edd045e05cf5af64e35ae0c4c6ef)

@ Low

Represents a data series containing low prices.

[ATAS.Indicators.DataSeriesType.High](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da655d20c1ca69519ca647684edbb2db35)

@ High

Represents a data series containing high prices.

[ATAS.Indicators.DataSeriesType.Open](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dac3bf447eabe632720a3aa1a7ce401274)

@ Open

Represents a data series containing open prices.

[ATAS.Indicators.DataSeriesType.Close](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dad3d2e617335f08df83599665eef8a418)

@ Close

Represents a data series containing closing prices.

Example of the Bar’s volume filter indicator which colours bars depending on their volumes.

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

[DisplayName("Bar's volume filter")]

public class BarVolumeFilter : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 #region Nested types

 public enum VolumeType

 {

 Volume,

 Ticks,

 Delta,

 Bid,

 Ask

 }

 #endregion

 private readonly [PaintbarsDataSeries](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) _paintBars = new [PaintbarsDataSeries](../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md)("Paint bars");

 private int _minFilter ;

 private int _maxFilter = 100;

 private System.Windows.Media.Color _color = System.Windows.Media.Colors.Orange;

 private VolumeType _volumeType;

 [Display(Name = "Type", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 5)]

 public VolumeType Type

 {

 get => _volumeType;

 set { _volumeType = value; RecalculateValues(); }

 }

 [Display(Name = "Minimum", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 10)]

 public int MinFilter

 {

 get => _minFilter;

 set { _minFilter = value;RecalculateValues(); }

 }

 [Display(Name = "Maximum", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 20)]

 public int MaxFilter

 {

 get => _maxFilter;

 set { _maxFilter = value; RecalculateValues(); }

 }

 [Name = "Color", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 30)]

 public System.Windows.Media.Color [FilterColor](../api/classes/classATAS_1_1Indicators_1_1FilterColor.md)

 {

 get => _color;

 set { _color = value; RecalculateValues(); }

 }

 public BarVolumeFilter() : base(true)

 {

 DataSeries[0] = _paintBars;

 _paintBars.IsHidden = true;

 DenyToChangePanel = true;

 }

 #region Overrides of BaseIndicator

 protected override void OnCalculate(int bar, decimal value)

 {

 var candle = GetCandle(bar);

 decimal volume = 0;

 switch (Type)

 {

 case VolumeType.Volume:

 {

 volume = candle.Volume;

 break;

 }

 case VolumeType.Ticks:

 {

 volume = candle.Ticks;

 break;

 }

 case VolumeType.Delta:

 {

 volume = candle.Delta;

 break;

 }

 case VolumeType.Bid:

 {

 volume = candle.Bid;

 break;

 }

 case VolumeType.Ask:

 {

 volume = candle.Ask;

 break;

 }

 default:

 throw new ArgumentOutOfRangeException();

 }

 if (volume > _minFilter && volume <= _maxFilter)

 {

 _paintBars[bar] = _color;

 }

 else

 {

 _paintBars[bar] = null;

 }

 }

 #endregion

}

[ATAS.DataFeedsCore.Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

Represents an order for trading on a financial exchange.

Definition Order.cs:13

[ATAS.Indicators.FilterColor](../api/classes/classATAS_1_1Indicators_1_1FilterColor.md)

Represents a filter with a value type of CrossColor. Inherits from Filter<TValue, TFilter> where TVal...

Definition FilterColor.cs:13

# CandleDataSeries - candles

The [CandleDataSeries](../api/classes/classATAS_1_1Indicators_1_1CandleDataSeries.md) designed for visualization of candles. Each element represents a [Candle](../api/classes/classATAS_1_1Indicators_1_1Candle.md).

Example of the indicator which displays an inverted chart:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class ReversalChart : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 readonly [CandleDataSeries](../api/classes/classATAS_1_1Indicators_1_1CandleDataSeries.md) _reversalCandles = new [CandleDataSeries](../api/classes/classATAS_1_1Indicators_1_1CandleDataSeries.md)("Candles");

 public ReversalChart()

 {

 (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[0]).VisualType = [VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb).Hide;

 DataSeries.Add(_reversalCandles);

 Panel = [IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md).[NewPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var candle = GetCandle(bar);

 _reversalCandles[bar].High = -candle.High;

 _reversalCandles[bar].Low = -candle.Low;

 _reversalCandles[bar].Open = -candle.Open;

 _reversalCandles[bar].Close = -candle.Close;

 }

}

[ATAS.Indicators.IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md)

Implementation of the IIndicatorDataProvider interface that provides access to various data and servi...

Definition IndicatorDataProvider.cs:123

[ATAS.Indicators.IndicatorDataProvider.NewPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8)

const string NewPanel

Represents the name of a new panel.

Definition IndicatorDataProvider.cs:135

[ATAS.Indicators.VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb)

VisualMode

Represents the visual modes available for displaying data series on a chart.

Definition VisualMode.cs:11

# CandlePartSeries

The [CandlePartSeries](../api/classes/classATAS_1_1Indicators_1_1CandlePartSeries.md) represents a data series of decimal values derived from specific parts of an [IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) created by an [ICandleCreator](../api/interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md).

 This DataSeries enables the creation of a data series for extracting a specific candle parameter. In other words, it allows the generation of a dataset containing values of a particular parameter (such as the [Open](../api/classes/classATAS_1_1Indicators_1_1Candle.md#af8186131eb0d1d9518ec2acf96c9c9a8)) based on a set of [IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md). Possible parameter types are represented in the [DataSeriesType](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) enum.

# IndicatorSeries

The [IndicatorSeries](../api/classes/classATAS_1_1Indicators_1_1IndicatorSeries.md) represents a custom data series for an indicator, derived from [BaseDataSeries](../api/classes/classATAS_1_1Indicators_1_1BaseDataSeries.md)<decimal>.

 This DataSeries serves as a wrapper for an indicator, allowing the retrieval of data from a specified indicator series through the [IDataSeries](../api/interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) interface. This feature is employed to use one indicator as a data source for another, facilitating the integration of indicators.

# PriceSelectionDataSeries - price selection

The [PriceSelectionDataSeries](../api/classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md), which present the [PriceSelectionValue](../api/classes/classATAS_1_1Indicators_1_1PriceSelectionValue.md) list for each bar, should be used for selection of the price levels in clusters and bars (similarly to selection of the Cluster Search indicator).

Example of use (selection of the clusters, which volume is higher than the one set by the filter):

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class SampleClusterSearch : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private int _filter = 10;

 readonly [PriceSelectionDataSeries](../api/classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md) _priceSelectionSeries = new [PriceSelectionDataSeries](../api/classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md)("Clusters Selection");

 public int [Filter](../api/classes/classATAS_1_1Indicators_1_1Filter.md)

 {

 get { return _filter; }

 set

 {

 _filter = value;

 RecalculateValues();

 }

 }

 public SampleClusterSearch()

 {

 DataSeries.Add(_priceSelectionSeries);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var candle = GetCandle(bar);

 for (decimal price = candle.High; price >= candle.Low; price -= TickSize)

 {

 var volumeinfo = candle.GetPriceVolumeInfo(price);

 if (volumeinfo == null)

 continue;

 if (volumeinfo.Volume > _filter)

 {

 var values = _priceSelectionSeries[bar];

 var priceSelection = values.FirstOrDefault(t => t.MinimumPrice == volumeinfo.Price);

 if (priceSelection == null)

 values.Add(new [PriceSelectionValue](../api/classes/classATAS_1_1Indicators_1_1PriceSelectionValue.md)(volumeinfo.Price)

 {

 VisualObject = [ObjectType](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86).Rectangle,

 Size = 10

 });

 }

 }

 }

}

[ATAS.Indicators.Filter](../api/classes/classATAS_1_1Indicators_1_1Filter.md)

Generic filter class that implements the IFilterValue interface.

Definition Filter.cs:261

[ATAS.Indicators.PriceSelectionDataSeries](../api/classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md)

Represents a data series of price selection values, each element is a synchronized list of PriceSelec...

Definition PriceSelectionDataSeries.cs:20

[ATAS.Indicators.PriceSelectionValue](../api/classes/classATAS_1_1Indicators_1_1PriceSelectionValue.md)

Represents a class for defining price level selection in clusters and bars. Using in PriceSelectionDa...

Definition PriceSelectionValue.cs:12

[ATAS.Indicators.ObjectType](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86)

ObjectType

Enumeration representing different types of graphic objects for PriceSelectionDataSeries.

Definition ObjectType.cs:13

# ObjectDataSeries - any object

The [ObjectDataSeries](../api/classes/classATAS_1_1Indicators_1_1ObjectDataSeries.md) is designed to store any object.

 It could be used for convenient linking of various objects to the bars. It could also be used for communicating complex objects between indicators.
