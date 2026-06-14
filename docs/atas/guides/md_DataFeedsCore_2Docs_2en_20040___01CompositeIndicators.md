# Creating composite indicators

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20040___01CompositeIndicators.html

In order to build an indicator on the basis of another one, it is necessary to develop and calculate the indicator, on the basis of which a calculation would be carried out, after which its value will be used in its own calculations. Example of realization of the MACD indicator:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class MACD : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private readonly EMA _long = new EMA();

 private readonly EMA _short = new EMA();

 private readonly SMA _sma = new SMA();

 public int LongPeriod

 {

 get { return _long.Period; }

 set

 {

 if (value <= 0)

 return;

 _long.Period = value;

 RecalculateValues();

 }

 }

 public int ShortPeriod

 {

 get { return _short.Period; }

 set

 {

 if (value <= 0)

 return;

 _short.Period = value;

 RecalculateValues();

 }

 }

 public int SignalPeriod

 {

 get { return _sma.Period; }

 set

 {

 if (value <= 0)

 return;

 _sma.Period = value;

 RecalculateValues();

 }

 }

 public MACD()

 {

 Panel = [IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md).[NewPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8);

 (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[0]).VisualType = [VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb).Histogram;

 (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[0]).Color = Colors.CadetBlue;

 DataSeries.Add(new [ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md)("Signal")

 {

 VisualType = [VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb).Line,

 LineDashStyle = LineDashStyle.Dash

 });

 LongPeriod = 26;

 ShortPeriod = 12;

 SignalPeriod = 9;

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var macd =_short.Calculate(bar, value) - _long.Calculate(bar, value);

 var signal = _sma.Calculate(bar, macd);

 this[bar] = macd;

 DataSeries[1][bar] = signal;

 }

}

[ATAS.Indicators.IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md)

Implementation of the IIndicatorDataProvider interface that provides access to various data and servi...

Definition IndicatorDataProvider.cs:123

[ATAS.Indicators.IndicatorDataProvider.NewPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8)

const string NewPanel

Represents the name of a new panel.

Definition IndicatorDataProvider.cs:135

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

[ATAS.Indicators.ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md)

Represents a data series of decimal values, each element is a decimal.

Definition ValueDataSeries.cs:26

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2

[ATAS.Indicators.VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb)

VisualMode

Represents the visual modes available for displaying data series on a chart.

Definition VisualMode.cs:11

The Calculate methods, which calculated these indicators and returned values of each indicator for a specific bar, were called in this example for third-party indicators (EMA, SMA, etc.).

If the used indicators should be calculated at the same incoming source as the developed indicator, they could be just added as ‘internal ones’ through the Add method in the constructor.

Below is an example of the altered MACD indicator where the _long and _short indicators are added as ‘internal ones’ and their values are received just through the data request for a specific bar.

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class MACD : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private readonly EMA _long = new EMA();

 private readonly EMA _short = new EMA();

 private readonly SMA _sma = new SMA();

 public MACD()

 {

 Panel = [IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md).[NewPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8);

 (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[0]).VisualType = [VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb).Histogram;

 (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[0]).Color = Colors.CadetBlue;

 DataSeries.Add(new [ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md)("Signal")

 {

 VisualType = [VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb).Line,

 LineDashStyle = LineDashStyle.Dash

 });

 LongPeriod = 26;

 ShortPeriod = 12;

 SignalPeriod = 9;

 Add(_short);

 Add(_long);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var macd = _short[bar] - _long[bar];

 var signal = _sma.Calculate(bar, macd);

 this[bar] = macd;

 DataSeries[1][bar] = signal;

 }

}

If a used internal indicator has several [DataSeries](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21), they also could be used in the developed indicator. Example of receiving the data from the Signal [DataSeries](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21) of the MACD indicator from another indicator:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class SampleIndicator : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private readonly MACD _macd = new MACD();

 public SampleIndicator()

 {

 Add(_macd);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var macdSignalSeries = ([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))_macd.DataSeries[1];

 var macdSignal = macdSignalSeries[bar];

 this[bar] = macdSignal * 2;

 }

}

[DataSeries](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21) of the used indicators could be displayed in the developed indicator. For this, the [DataSeries](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21) from the used indicators, which should be displayed, should be added to the collection of [DataSeries](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21) of the current indicator.

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class SampleIndicator : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private readonly MACD _macd = new MACD();

 public SampleIndicator()

 {

 Add(_macd);

 DataSeries.Add(_macd.DataSeries[1]);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

}
