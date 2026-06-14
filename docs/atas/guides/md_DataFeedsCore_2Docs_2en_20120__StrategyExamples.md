# Examples of strategies

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20120__StrategyExamples.html

# Sample SMA strategy

[Sources from this sample](https://atas.net/Setup/SampleStrategy.zip)

public class SmaChartStrategy : [ATAS](../api/namespaces/namespaceATAS.md).Strategies.Chart.ChartStrategy

{

 #region Private fields

 private readonly SMA _shortSma = new SMA();

 private readonly SMA _longSma = new SMA();

 private int _lastBar;

 #endregion

 #region Public properties

 [Display(

 Name = "Short period",

 Order = 10)]

 [Parameter]

 public int ShortPeriod

 {

 get => _shortSma.Period;

 set

 {

 _shortSma.Period = Math.Max(1, value);

 RecalculateValues();

 }

 }

 [Display(

 Name = "Long period",

 Order = 20)]

 [Parameter]

 public int LongPeriod

 {

 get => _longSma.Period;

 set

 {

 _longSma.Period = Math.Max(1, value);

 RecalculateValues();

 }

 }

 [Display(

 Name = "Volume",

 Order = 30)]

 [Parameter]

 public decimal [Volume](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dabd7a9717d29c5ddcab1bc175eda1e298) { get; set; }

 [Display(ResourceType = typeof(Resources),

 Name = "ClosePositionOnStopping",

 Order = 40)]

 [Parameter]

 public bool ClosePositionOnStopping { get; set; }

 #endregion

 #region ctor

 public SmaChartStrategy()

 {

 var firstSeries = (ValueDataSeries)DataSeries[0];

 firstSeries.Name = "Short";

 firstSeries.Color = Colors.Red;

 firstSeries.VisualType = [VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb).Line;

 DataSeries.Add(new ValueDataSeries("Long")

 {

 VisualType = [VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb).Line,

 Color = Colors.Green,

 });

 ShortPeriod = 21;

 LongPeriod = 75;

 [Volume](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dabd7a9717d29c5ddcab1bc175eda1e298) = 1;

 ClosePositionOnStopping = true;

 }

 #endregion

 #region Overrides of BaseIndicator

 protected override void OnCalculate(int bar, decimal value)

 {

 var shortSma = _shortSma.Calculate(bar, value);

 var longSma = _longSma.Calculate(bar, value);

 DataSeries[0][bar] = shortSma;

 DataSeries[1][bar] = longSma;

 var prevBar = _lastBar;

 _lastBar = bar;

 if (!CanProcess(bar) || prevBar == bar)

 return;

 if (_shortSma[prevBar - 1] < _longSma[prevBar - 1] && _shortSma[prevBar] >= _longSma[prevBar])

 {

 //cross up

 OpenPosition([OrderDirections](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c).Buy);

 }

 if (_shortSma[prevBar - 1] > _longSma[prevBar - 1] && _shortSma[prevBar] <= _longSma[prevBar])

 {

 //cross down

 OpenPosition([OrderDirections](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c).Sell);

 }

 }

 protected override void OnStopping()

 {

 if (CurrentPosition != 0 && ClosePositionOnStopping)

 {

 RaiseShowNotification($"Closing current position {CurrentPosition} on stopping.", level: LoggingLevel.[Warning](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a9b5d2538699d679afdd312abbba1c334a0eaadb4fcb48a0a0ed7bc9868be9fbaa));

 CloseCurrentPosition();

 }

 base.OnStopping();

 }

 #endregion

 #region Private methods

 private void OpenPosition(OrderDirections direction)

 {

 var order = new [Order](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722aa240fa27925a635b08dc28c9e4f9216d)

 {

 [Portfolio](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad4f859a96c13f551a2771b7fc3a78d38) = [Portfolio](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad4f859a96c13f551a2771b7fc3a78d38),

 [Security](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2fae32629d4ef4fc6341f1751b405e45) = [Security](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2fae32629d4ef4fc6341f1751b405e45),

 Direction = direction,

 Type = [OrderTypes](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8).Market,

 QuantityToFill = GetOrderVolume(),

 };

 OpenOrder(order);

 }

 private void CloseCurrentPosition()

 {

 var order = new [Order](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722aa240fa27925a635b08dc28c9e4f9216d)

 {

 [Portfolio](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad4f859a96c13f551a2771b7fc3a78d38) = [Portfolio](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad4f859a96c13f551a2771b7fc3a78d38),

 [Security](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2fae32629d4ef4fc6341f1751b405e45) = [Security](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2fae32629d4ef4fc6341f1751b405e45),

 Direction = CurrentPosition > 0 ? OrderDirections.Sell : [OrderDirections](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c).Buy,

 Type = [OrderTypes](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8).Market,

 QuantityToFill = Math.Abs(CurrentPosition),

 };

 OpenOrder(order);

 }

 private decimal GetOrderVolume()

 {

 if (CurrentPosition == 0)

 return [Volume](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dabd7a9717d29c5ddcab1bc175eda1e298);

 if (CurrentPosition > 0)

 return [Volume](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dabd7a9717d29c5ddcab1bc175eda1e298) + CurrentPosition;

 return [Volume](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dabd7a9717d29c5ddcab1bc175eda1e298) + Math.Abs(CurrentPosition);

 }

 #endregion

}

[ATAS.DataFeedsCore.EntityType.Security](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2fae32629d4ef4fc6341f1751b405e45)

@ Security

Represents a security entity.

[ATAS.DataFeedsCore.EntityType.Order](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722aa240fa27925a635b08dc28c9e4f9216d)

@ Order

Represents an order entity.

[ATAS.DataFeedsCore.EntityType.Portfolio](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad4f859a96c13f551a2771b7fc3a78d38)

@ Portfolio

Represents a portfolio entity.

[ATAS.DataFeedsCore.OrderTypes](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8)

OrderTypes

Specifies the type of an order.

Definition OrderTypes.cs:7

[ATAS.DataFeedsCore.OrderDirections](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c)

OrderDirections

Specifies the direction of an order.

Definition OrderDirections.cs:7

[ATAS.DataFeedsCore.MessageType.Warning](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a9b5d2538699d679afdd312abbba1c334a0eaadb4fcb48a0a0ed7bc9868be9fbaa)

@ Warning

[ATAS.Indicators.DataSeriesType.Volume](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dabd7a9717d29c5ddcab1bc175eda1e298)

@ Volume

Represents a data series containing volume data.

[ATAS.Indicators.VisualMode](../api/namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb)

VisualMode

Represents the visual modes available for displaying data series on a chart.

Definition VisualMode.cs:11

[ATAS](../api/namespaces/namespaceATAS.md)

Definition AsyncConnector.cs:7
