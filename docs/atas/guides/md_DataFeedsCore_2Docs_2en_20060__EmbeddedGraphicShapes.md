# Embedded collections of graphic shapes

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20060__EmbeddedGraphicShapes.html

# Adding horizontal lines

In order to add horizontal lines, it is necessary to create a [LineSeries](../api/classes/classATAS_1_1Indicators_1_1LineSeries.md) object and add [LineSeries](../api/classes/classATAS_1_1Indicators_1_1LineSeries.md) into its collection. Example of use in the RSI indicator

public class RSI : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

 {

 private readonly SMMA _negative;

 private readonly SMMA _positive;

 public int Period

 {

 get => _positive.Period;

 set

 {

 if (value <= 0)

 return;

 _positive.Period = _negative.Period = value;

 RecalculateValues();

 }

 }

 public RSI()

 {

 Panel = IndicatorDataProvider.NewPanel;

 LineSeries.Add(new LineSeries("Down")

 {

 Color = Colors.Orange,

 LineDashStyle = LineDashStyle.Dash,

 [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = 30,

 Width = 1

 });

 LineSeries.Add(new LineSeries("Up")

 {

 Color = Colors.Orange,

 LineDashStyle = LineDashStyle.Dash,

 [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = 70,

 Width = 1

 });

 _positive = new SMMA();

 _negative = new SMMA();

 Period = 10;

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == 0)

 {

 this[bar] = 0;

 }

 else

 {

 var diff = (decimal)SourceDataSeries[bar] - (decimal)SourceDataSeries[bar - 1];

 var pos = _positive.Calculate(bar, diff > 0 ? diff : 0);

 var neg = _negative.Calculate(bar, diff < 0 ? -diff : 0);

 if (neg != 0)

 {

 var div = pos / neg;

 this[bar] = div == 1

 ? 0m

 : 100m - 100m / (1m + div);

 }

 else

 {

 this[bar] = 100m;

 }

 }

 }

 }

}

[ATAS.Indicators.DataSeriesType.Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c)

@ Value

Represents a value-based data series.

[ATAS](../api/namespaces/namespaceATAS.md)

Definition AsyncConnector.cs:7

# Adding trend lines

It is possible to add trend lines to the chart with the help of the [TrendLines](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a885514af616ec4974504ab6a1dd3205a) collection.

 For this, it is necessary to create a [TrendLine](../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md) object, specifying the initial bar number, the initial point price, the final bar number and the final point price. The [IsRay](../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#aa38fc0920db3256cc3369469714e5c24) flag could also be set to convert a trend line into a half-line.

 After creating an object, it should be just added to the [TrendLines](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a885514af616ec4974504ab6a1dd3205a) collection.

# Drawing rectangular areas

Rectangular areas could be added to the chart with the help of the [Rectangles](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a310cc3db29528b38a8eaaf3e0341f750) collection.

 For this, a [DrawingRectangle](../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md) object should be created, specifying the initial bar number, the initial point price, the final bar number, the final point price, the fill and border colours.

 After creating an object, it should be just added to the [Rectangles](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a310cc3db29528b38a8eaaf3e0341f750) collection.

# Horizontal lines until the first touch

It is possible to create horizontal lines in an indicator, which would be prolonged until the first touch, with the help of the [HorizontalLinesTillTouch](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a07d8c669247a1b07aa8a7b0674f39f3e) collection.

 For this, a [LineTillTouch](../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md) object should be created and added to the collection.

[LineTillTouch](../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md) has 2 constructors:

- 1. It creates a horizontal line, which starts in a specified bar at a specified price.

public LineTillTouch(int bar, decimal price, Pen pen)

- 2. Similarly, it creates a horizontal line, for which a number of bars, at the end of which the line would be considered completed, is set additionally.

public LineTillTouch(int bar, decimal price, Pen pen, int fixedBarsCount)

# Adding text labels

Text labels can be added using the following overloads of the [AddText](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#ac0c6d0d23e4ce7fa4b4bbc95a2b4e413) method:

AddText(string tag, string text, bool isAbovePrice, int bar, decimal price,

 int yOffset, int xOffset, Color textColor,Color outlineColor, Color fillColor,

 float fontSize, DrawingText.TextAlign align, bool autoSize = false)

AddText(string tag, string text, bool isAbovePrice, int bar, decimal price,

 Color textColor,Color outlineColor, Color fillColor,

 float fontSize, DrawingText.TextAlign align, bool autoSize = false)

AddText(string tag, string text, bool isAbovePrice, int bar, decimal price,

 Color textColor, Color fillColor, float fontSize, DrawingText.TextAlign align,

 bool autoSize = false)

The passed parameters:

- tag - a unique text identifier which is linked with a text mark.

- text - the displayed text.

- isAbovePrice - whether to display text over the price or not.

- bar - the number of the bar on which the text should be displayed.

- price - the price on which the text should be displayed.

- yOffset - an offset in pixels along the Y-axis.

- xOffset - an offset in pixels along the X-axis.

- textColor - the text colour.

- outlineColor - the colour of the frame around the text.

- fillColor - the colour of the text background.

- fontSize - the font size.

- align - text alignment (left, right or center).

- autoSize - a flag which is responsible for activation of the dynamic text size depending on the chart scale.

All three methods return the [DrawingText](../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) object, in which parameters could be changed dynamically.
