# Drawing

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20070__Graphics.html

# Basics

It is possible to redefine the [OnRender](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#ad18d3b52324bbe2c61e2194187ec4f0a) method in the indicator and realize your own logic of data rendering.

 It is necessary to set the [EnableCustomDrawing](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a8525dc77a6105f64bb51b8d7b1076fcf) flag ‘true’ for this method to start being called.

 It is also necessary to set a list of layers where rendering will be carried out.

 The list of layers is set through the [SubscribeToDrawingEvents](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a88a772682022b0a67f90099994df1a78) method, to which the [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) flags are passed over.

The [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) flags could be:

- None - nothing will be rendered in this case.

- Historical - in this case, rendering will be called at every new candle, when contracted and when the chart is moved.

- LatestBar - rendering is called when the most recent bar is changed. As a rule, it takes place at every new tick.

- Final - the final layer, which is rendered at every chart rendering. For example, when the mouse is moved.

If the [SubscribeToDrawingEvents](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a88a772682022b0a67f90099994df1a78) is not called, the indicator will be rendered only when the most recent bar changes.

 Example of calling the [SubscribeToDrawingEvents](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a88a772682022b0a67f90099994df1a78), after which the OnRender method will be called at every new tick and after the final rendering:

`SubscribeToDrawingEvents(DrawingLayouts.Final | DrawingLayouts.LatestBar);`

The following objects are passed to the [OnRender](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#ad18d3b52324bbe2c61e2194187ec4f0a) method:

- RenderContext context - the context, in which rendering would take place.

- [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout - the layout, which is rendered at the current moment.

# RenderContext

Rendering by GDI+ like principles is actually carried out with the help of this context.

Several examples:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

protected virtual void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

{

 //Draw Rectangle(width = 100; height = 200) from point(x = 5; y = 10)

 context.DrawRectangle(RenderPens.Blue,new Rectangle(5, 10, 100, 200));

 //Fill Rectangle(width = 100; height = 100) from point(x = 0; y = 0)

 context.FillRectangle(Color.DarkSalmon, new Rectangle(0, 0, 100, 100));

 //Draw line from point(x = 10; y = 20) to point(x = 50; y = 60)

 context.DrawLine(RenderPens.AliceBlue, 10 ,20, 50, 60);

 //Draw string at point(x = 50; y = 60)

 context.DrawString("Sample string", new RenderFont("Arial", 15), Color.Black, 50, 60);

 //Draw ellipse inside rectangle

 context.DrawEllipse(new RenderPen(Color.Bisque), new Rectangle(10, 10, 100, 100));

}

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2

[ATAS.Indicators.DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92)

DrawingLayouts

Enumerates the different drawing layouts available for chart drawings.

Definition DrawingLayouts.cs:14

# Coordinates system

The origin or coordinates of the chart is in the upper left corner.

 The coordinates system is shown in the picture below.

Coordinates system

 Every indicator has a container ([Container](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1753849ca89e685ad2fa9592855a3e2d) property), which contains information about the rendering area ([Region](../api/interfaces/interfaceATAS_1_1Indicators_1_1IContainer.md#a2a127b6200f2b7e5e7ecbe090e047869) property). All main properties of the chart, mouse and keyboard could be obtained through the [ChartInfo](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a52e91039466de7c2218a0248e585a259) property.

The [ChartArea](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#ac5bc6a2f7bd9fde192049f9cfc811709) property, which returns [Region](../api/interfaces/interfaceATAS_1_1Indicators_1_1IContainer.md#a2a127b6200f2b7e5e7ecbe090e047869), was added for convenience of access to the chart area.

The [MouseLocationInfo](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#ab23ac08de154b825a480f1d8fcbed590) property, which returns `ChartInfo.MouseLocationInfo`, was added for convenience of access to the mouse data.

Also, there are [ChartInfo](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a52e91039466de7c2218a0248e585a259) extensions, which allow working with coordinates:

- [GetXByBar(int bar, bool isStartOfBar)](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md#a9be0786911691b0267478f89176661e9) - the method returns the X coordinate for the passed bar number. If `isStartOfBar` = true, the bar beginning coordinate is returned, otherwise the bar middle coordinate is returned

- [GetYByPrice(decimal price, bool isStartOnPriceLevel)](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md#ae3f75c55508da9a16649cbb184976ae8) - the method returns the Y coordinate for the passed price. If `isStartOnPriceLevel` = true, the price level beginning coordinate is returned, otherwise the price level middle coordinate is returned

Example of an indicator, which draws the intersection and shows the volume of the bar, over which the mouse pointer is moved.

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class SampleRendering : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 public SampleRendering()

 {

 EnableCustomDrawing = true;

 //Subscribing only to drawing on final layout

 SubscribeToDrawingEvents([DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92).Final);

 }

 protected override void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

 {

 // creating pen, width 4px

 var pen = new RenderPen(Color.BlueViolet, 4);

 //drawing horizontal line

 context.DrawLine(pen, 0, MouseLocationInfo.LastPosition.Y, ChartArea.Width, MouseLocationInfo.LastPosition.Y);

 //drawing vertical line

 context.DrawLine(pen, MouseLocationInfo.LastPosition.X, 0, MouseLocationInfo.LastPosition.X, ChartArea.Height);

 var candle = GetCandle(MouseLocationInfo.BarBelowMouse);

 if (candle != null)

 {

 var font = new RenderFont("Arial", 14);

 var text = $"Total candle volume={candle.Volume}";

 var textSize = context.MeasureString(text, font);

 var textRectangle = new Rectangle(MouseLocationInfo.LastPosition.X + 10, MouseLocationInfo.LastPosition.Y + 10, (int)textSize.Width, (int)textSize.Height);

 context.FillRectangle(Color.CornflowerBlue, textRectangle);

 context.DrawString(text, font, Color.AliceBlue, textRectangle);

 }

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

}

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

Shows the volume of the bar

# Rendering an indicator above and below the chart

The [DrawAbovePrice](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a090c77a1502d9f34cc92bd873b181973) indicator property could be used for regulating the order of the indicator rendering.

If [DrawAbovePrice](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a090c77a1502d9f34cc92bd873b181973) = true, the indicator is rendered above the chart, otherwise - below the chart.
