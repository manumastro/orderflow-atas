# Chart managing

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20095__ChartManaging.html

Chart management is carried out both using the chart settings menu, and by entering the appropriate parameters directly into the indicator. In this case, the indicator can be provided with fields for setting graphical parameters.

 Essential chart parameters are accessible through the [ChartInfo](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a52e91039466de7c2218a0248e585a259) object.

 Let's examine some properties of this object:

- [FootprintContentMode](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#a83df2de5ae5b38816bf02b3ea17e7609) - Mode for text content within clusters.

- [FootprintVisualMode](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#a78874ccbfdce794cc751bd5d0a1a544b) - Mode for cluster visualization (histograms, "bid" and "ask" cells, and so forth).

- [FootprintColorScheme](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#a5ddc192b228b4b560b4f800931696d0b) - Color scheme for clusters.

- [ChartVisualMode](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#a2f7abd6890435f50dd13e37364974346) - This property facilitates both retrieving values and defining the chart type.

- [PriceChartContainer](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#ab2ce4a79734b9a560b2f665597d0a458):
[SetCustomBarsSpacing(decimal? value)](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md#a742e722bf069b9f7dc7648e2ba5a6e4b) - This method allows you to set a custom distance between bars.

- [SetCustomBarsWidth(decimal value, bool freezeValue)](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md#abc5f6110efe886abcedb4f40a6a60cb2) - This method enables setting a custom bar width and specifying whether users can manually adjust the width through the interface.

- [CoordinatesManager](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#a5c87f89e74e3e958e564a61d577c69b7):
[MoveChartUpAndDown(int offset)](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#a0ec28cbfa0fb72272378663a35c29d25) - This method shifts the chart up or down by the specified distance.

- [ChangeRowsHeight(decimal newHeight)](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#a57010a0e2e242ec52214000d6aaa65db) - This method alters the height of price level rows.

- [EnableAutoScale()](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#ad50f2976267956b2a99e7d5c06ad9c79) - This method enables automatic chart scaling.

- [ScrollPrice(int ticksCount)](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#aa3fe0ccf2a48e0a7c5eeb4fff10590e7) - This method scrolls the chart along the price axis by the designated number of ticks.

public class SampleChartManaging : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 private decimal _barsSpacing = 1;

 private decimal _barsWidth = 1;

 private ChartVisualModes _modes;

 private FootprintVisualModes _visualMode;

 private FootprintContentModes _contentMode;

 private FootprintColorSchemes _colorScheme;

 private int _moveChartUpAndDown;

 private decimal _changeRowsHeight = 3;

 private int _scrollPrice;

 [Range(1, 1000)]

 public decimal BarsSpacing

 {

 get => _barsSpacing;

 set

 {

 _barsSpacing = value;

 ChartInfo?.PriceChartContainer?.SetCustomBarsSpacing(_barsSpacing);

 RedrawChart();

 }

 }

 [Range(1, 1000)]

 public decimal BarsWidth

 {

 get => _barsWidth;

 set

 {

 _barsWidth = value;

 ChartInfo?.PriceChartContainer?.SetCustomBarsWidth(_barsWidth, false);

 RedrawChart();

 }

 }

 public [ChartVisualModes](../api/namespaces/namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51) ChartMode

 {

 get => _modes;

 set

 {

 _modes = value;

 if (ChartInfo != null)

 ChartInfo.ChartVisualMode = value;

 RedrawChart();

 }

 }

 public [FootprintContentModes](../api/namespaces/namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945) FootprintContentMode

 {

 get => _contentMode;

 set

 {

 _contentMode = value;

 if (ChartInfo != null)

 ChartInfo.FootprintContentMode = value;

 }

 }

 public [FootprintVisualModes](../api/namespaces/namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71) FootprintVisualMode

 {

 get => _visualMode;

 set

 {

 _visualMode = value;

 if (ChartInfo != null)

 ChartInfo.FootprintVisualMode = value;

 }

 }

 public [FootprintColorSchemes](../api/namespaces/namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104f) FootprintColorScheme

 {

 get => _colorScheme;

 set

 {

 _colorScheme = value;

 if (ChartInfo != null)

 ChartInfo.FootprintColorScheme = value;

 }

 }

 public int MoveChartUpAndDown

 {

 get => _moveChartUpAndDown;

 set

 {

 var diff = _moveChartUpAndDown - value;

 ChartInfo?.CoordinatesManager?.MoveChartUpAndDown(diff);

 _moveChartUpAndDown = value;

 }

 }

 [Range(1, 100)]

 public decimal ChangeRowsHeight

 {

 get => _changeRowsHeight;

 set

 {

 _changeRowsHeight = value;

 ChartInfo?.CoordinatesManager?.ChangeRowsHeight(value);

 }

 }

 public int ScrollPrice

 {

 get => _scrollPrice;

 set

 {

 var diff = _scrollPrice - value;

 ChartInfo?.CoordinatesManager?.ScrollPrice(diff);

 _scrollPrice = value;

 }

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnDispose()

 {

 ChartInfo?.PriceChartContainer?.SetCustomBarsSpacing(null);

 ChartInfo?.PriceChartContainer?.SetCustomBarsWidth(ChartInfo.PriceChartContainer.BarsWidth, false);

 }

}

[ATAS.Indicators.ChartVisualModes](../api/namespaces/namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51)

ChartVisualModes

Enumerates the visual modes available for displaying price chart data on a chart.

Definition ChartVisualModes.cs:7

[ATAS.Indicators.FootprintVisualModes](../api/namespaces/namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71)

FootprintVisualModes

Definition FootprintVisualModes.cs:8

[ATAS.Indicators.FootprintContentModes](../api/namespaces/namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945)

FootprintContentModes

Definition FootprintContentModes.cs:8

[ATAS.Indicators.FootprintColorSchemes](../api/namespaces/namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104f)

FootprintColorSchemes

Definition FootprintColorSchemes.cs:8

[ATAS](../api/namespaces/namespaceATAS.md)

Definition AsyncConnector.cs:7

Chart managing from indicator
