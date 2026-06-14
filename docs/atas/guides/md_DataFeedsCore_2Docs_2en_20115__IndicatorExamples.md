# Examples of indicators

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20115__IndicatorExamples.html

# Volume on the chart

The indicator, which was inherited from the standard Volume indicator, was created in this example. The [OnCalculate](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a65c50fc1fec7d6c490aa23a1c3eeebd3) method was redefined and left empty (to avoid unnecessary calculations). And outlined the rendering logic in the [OnRender](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#ad18d3b52324bbe2c61e2194187ec4f0a) method. Colours are taken from the base indicator [DataSeries](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21).

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

[DisplayName("Volume on the chart")]

public class VolumeOnChart : Volume

{

 private decimal _height = 15;

 public decimal Height

 {

 get { return _height; }

 set

 {

 if (value < 10 || value > 100)

 return;

 _height = value;

 }

 }

 public VolumeOnChart()

 {

 EnableCustomDrawing = true;

 SubscribeToDrawingEvents([DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92).LatestBar);

 Panel = [IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md).[CandlesPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a25ac59dd757bcaa818b4e5f8193d6bf4);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

 {

 var maxValue = 0m;

 var maxHeight = ChartInfo.Region.Height * _height / 100m;

 var positiveColor = (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[0]).Color.Convert(); // color from positive dataseries

 var negativeColor = (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[1]).Color.Convert(); // color from negative dataseries

 var neutralColor = (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[2]).Color.Convert(); // color from neutral dataseries

 var filterColor = (([ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md))DataSeries[3]).Color.Convert(); // color from filter dataseries

 for (int i = FirstVisibleBarNumber; i <= LastVisibleBarNumber; i++)

 {

 var candle = GetCandle(i);

 var volumeValue = Input == InputType.Volume ? candle.Volume : candle.Ticks;

 maxValue = Math.Max(volumeValue, maxValue);

 }

 for (int i = FirstVisibleBarNumber; i <= LastVisibleBarNumber; i++)

 {

 var candle = GetCandle(i);

 var volumeValue = Input == InputType.Volume ? candle.Volume : candle.Ticks;

 Color volumeColor;

 if (UseFilter && volumeValue > FilterValue)

 {

 volumeColor = filterColor;

 }

 else

 {

 if (DeltaColored)

 {

 if (candle.Delta > 0)

 {

 volumeColor = positiveColor;

 }

 else if (candle.Delta < 0)

 {

 volumeColor = negativeColor;

 }

 else

 {

 volumeColor = neutralColor;

 }

 }

 else

 {

 if (candle.Close > candle.Open)

 {

 volumeColor = positiveColor;

 }

 else if (candle.Close < candle.Open)

 {

 volumeColor = negativeColor;

 }

 else

 {

 volumeColor = neutralColor;

 }

 }

 }

 var x = ChartInfo.GetXByBar(i);

 var height = (int)(maxHeight * volumeValue / maxValue);

 var rectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(x, ChartInfo.Region.Height - height, (int)ChartInfo.PriceChartContainer.BarsWidth, height);

 context.FillRectangle(volumeColor, rectangle);

 }

 }

 public override string [ToString](../api/namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abe2e6e2d35676ab00e3c4e3b5dc53d7c)()

 {

 return "Volume on the chart";

 }

}

[ATAS.Indicators.IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md)

Implementation of the IIndicatorDataProvider interface that provides access to various data and servi...

Definition IndicatorDataProvider.cs:123

[ATAS.Indicators.IndicatorDataProvider.CandlesPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a25ac59dd757bcaa818b4e5f8193d6bf4)

const string CandlesPanel

Represents the name of the candles panel on the chart.

Definition IndicatorDataProvider.cs:140

[ATAS.Indicators.ValueDataSeries](../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md)

Represents a data series of decimal values, each element is a decimal.

Definition ValueDataSeries.cs:26

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2

[ATAS.Indicators.DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92)

DrawingLayouts

Enumerates the different drawing layouts available for chart drawings.

Definition DrawingLayouts.cs:14

[ATAS.Indicators.ObjectType.Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)

@ Rectangle

Rectangle graphic object.

[ATAS.Strategies.ATM.ToString](../api/namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abe2e6e2d35676ab00e3c4e3b5dc53d7c)

override string ToString()

Definition StopProfit.cs:94

# Current price

Example of an indicator which displays the current price and current time in the chart:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

[DisplayName("Current price")]

public class CurrentPrice : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 RenderFont _font = new RenderFont("Roboto", 14);

 private Color _background = Color.Blue;

 private Color _textColor = Color.AliceBlue;

 private RenderStringFormat _stringFormat = new RenderStringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };

 public System.Windows.Media.Color Background

 {

 get => _background.Convert();

 set => _background = value.Convert();

 }

 public System.Windows.Media.Color TextColor

 {

 get => _textColor.Convert();

 set => _textColor = value.Convert();

 }

 public float FontSize

 {

 get => _font.Size;

 set

 {

 if (value < 5)

 return;

 _font = new RenderFont("Roboto", value);

 }

 }

 public bool ShowTime { get; set; } = true;

 public string TimeFormat { get; set; } = "HH:mm:ss";

 public CurrentPrice() : base(true)

 {

 SubscribeToDrawingEvents([DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92).Final);

 EnableCustomDrawing = true;

 DataSeries[0].IsHidden = true;

 }

 #region Overrides of BaseIndicator

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 #endregion

 protected override void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

 {

 if (LastVisibleBarNumber != CurrentBar - 1)

 return;

 var candle = GetCandle(LastVisibleBarNumber);

 var priceString = candle.Close.ToString();

 var size = context.MeasureString(priceString, _font);

 var x = (int)(ChartInfo.GetXByBar(LastVisibleBarNumber) + ChartInfo.PriceChartContainer.BarsWidth);

 var y = ChartInfo.GetYByPrice(candle.Close, false);

 var rectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(x + 10, y - (int)size.Height / 2, (int)size.Width + 10, (int)size.Height);

 var points = new List<point>

 {

 new Point(x, y),

 new Point(rectangle.X, rectangle.Y),

 new Point(rectangle.X + rectangle.Width, rectangle.Y),

 new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),

 new Point(rectangle.X, rectangle.Y + rectangle.Height),

 };

 context.FillPolygon(_background, points.ToArray());

 rectangle.Y++;

 context.DrawString(priceString, _font, _textColor, rectangle, _stringFormat);

 if (ShowTime)

 {

 var time = [DateTime](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e).Now.ToString(TimeFormat);

 size = context.MeasureString(time, _font);

 context.DrawString(time, _font, _textColor, rectangle.X + rectangle.Width - size.Width, rectangle.Y - size.Height);

 }

 }

}

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

[OFT.Attributes.Editors.MaskTypes.DateTime](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)

@ DateTime

Current price

# Tables

Below is an example of the indicator, which displays the data in the table form (Cluster Statistic analog)

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class ClusterStatisticSample : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 public ClusterStatisticSample()

 {

 EnableCustomDrawing = true;

 SubscribeToDrawingEvents([DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92).LatestBar);

 Panel = [IndicatorDataProvider](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md).[NewPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8);

 DenyToChangePanel = true;

 DataSeries[0].IsHidden = true;

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

 {

 var rowHeight = Container.Region.Height / 3;

 var drawText = ChartInfo.PriceChartContainer.BarsWidth > 50;

 var font = new RenderFont("Roboto", 13);

 [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) rectangle;

 for (int bar = FirstVisibleBarNumber; bar <= LastVisibleBarNumber; bar++)

 {

 var candle = GetCandle(bar);

 var x = ChartInfo.GetXByBar(bar);

 var y = [Container](../api/classes/classATAS_1_1Indicators_1_1Container.md).[Region](../api/classes/classATAS_1_1Indicators_1_1Container.md#a233453cfd0b4536fbfb08974f6773ec9).Y;

 rectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(x, y, (int)ChartInfo.PriceChartContainer.BarsWidth, rowHeight);

 context.FillRectangle(Color.LightBlue, rectangle);

 if (drawText)

 context.DrawString(candle.Volume.ToString(), font, Color.Black, rectangle);

 rectangle.Y += rowHeight;

 context.FillRectangle(Color.IndianRed, rectangle);

 if (drawText)

 context.DrawString(candle.Bid.ToString(), font, Color.Black, rectangle);

 rectangle.Y += rowHeight;

 context.FillRectangle(Color.Lime, rectangle);

 if (drawText)

 context.DrawString(candle.Ask.ToString(), font, Color.Black, rectangle);

 }

 #region draw headers

 rectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(0, [Container](../api/classes/classATAS_1_1Indicators_1_1Container.md).[Region](../api/classes/classATAS_1_1Indicators_1_1Container.md#a233453cfd0b4536fbfb08974f6773ec9).Y, 100, rowHeight);

 context.FillRectangle(Color.Gray, rectangle);

 context.DrawString("Volume", font, Color.Black, rectangle);

 rectangle.Y += rowHeight;

 context.FillRectangle(Color.Gray, rectangle);

 context.DrawString("Bid", font, Color.Black, rectangle);

 rectangle.Y += rowHeight;

 context.FillRectangle(Color.Gray, rectangle);

 context.DrawString("Ask", font, Color.Black, rectangle);

 #endregion

 }

}

[ATAS.Indicators.Container](../api/classes/classATAS_1_1Indicators_1_1Container.md)

Represents a container with a defined region on the chart.

Definition IIndicatorContainer.cs:596

[ATAS.Indicators.Container.Region](../api/classes/classATAS_1_1Indicators_1_1Container.md#a233453cfd0b4536fbfb08974f6773ec9)

Rectangle Region

Gets the region of the container.

Definition IIndicatorContainer.cs:600

[ATAS.Indicators.IndicatorDataProvider.NewPanel](../api/classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8)

const string NewPanel

Represents the name of a new panel.

Definition IndicatorDataProvider.cs:135

Tables

# Watermark

Sample of indicator which shows watermark on the chart:

namespace ATAS.Indicators.Technical

{

 using System;

 using System.ComponentModel;

 using System.ComponentModel.DataAnnotations;

 using System.Drawing;

 using ATAS.Indicators.Editors;

 using ATAS.Indicators.Properties;

 using OFT.Rendering.Context;

 using OFT.Rendering.Tools;

 using Color = System.Windows.Media.Color;

 public class Watermark : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

 {

 #region Nested types

 public enum Location

 {

 [Display(Name = "Center")]

 Center,

 [Display(Name = "TopLeft")]

 TopLeft,

 [Display(Name = "TopRight")]

 TopRight,

 [Display(Name = "BottomLeft")]

 BottomLeft,

 [Display(Name = "BottomRight")]

 BottomRight

 }

 #endregion

 #region Properties

 [Display(Name = "Color", GroupName = "Common", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 10)]

 public Color TextColor { get; set; } = Color.FromArgb(255, 225, 225, 225);

 [Display(Name = "TextLocation", GroupName = "Common", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 20)]

 public Location TextLocation { get; set; }

 [Display(Name = "HorizontalOffset", GroupName = "Common", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 30)]

 public int HorizontalOffset { get; set; }

 [Display(Name = "VerticalOffset", GroupName = "Common", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 40)]

 public int VerticalOffset { get; set; }

 [Display(Name = "ShowInstrument", GroupName = "FirstLine", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 50)]

 public bool ShowInstrument { get; set; } = true;

 [Display(Name = "ShowPeriod", GroupName = "FirstLine", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 60)]

 public bool ShowPeriod { get; set; } = true;

 [Display(Name = "Font", GroupName = "FirstLine", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 70)]

 [Editor(typeof(FontEditor), typeof(FontEditor))]

 public FontSetting Font { get; set; } = new FontSetting { Size = 60, Bold = true };

 [Display(Name = "Text", GroupName = "SecondLine", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 80)]

 public string AdditionalText { get; set; } = "";

 [Display(Name = "Font", GroupName = "SecondLine", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 90)]

 [Editor(typeof(FontEditor), typeof(FontEditor))]

 public FontSetting AdditionalFont { get; set; } = new FontSetting { Size = 55 };

 [Display(Name = "VerticalOffset", GroupName = "SecondLine", [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) = 100)]

 public int AdditionalTextYOffset { get; set; } = -40;

 #endregion

 #region ctor

 public Watermark()

 : base(true)

 {

 Font.PropertyChanged += (a, b) => RedrawChart();

 AdditionalFont.PropertyChanged += (a, b) => RedrawChart();

 DataSeries[0].IsHidden = true;

 DenyToChangePanel = true;

 EnableCustomDrawing = true;

 SubscribeToDrawingEvents([DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92).Historical);

 DrawAbovePrice = false;

 }

 #endregion

 #region Overrides of BaseIndicator

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

 {

 var showSecondLine = !string.IsNullOrWhiteSpace(AdditionalText);

 if (!showSecondLine && !ShowInstrument && !ShowPeriod)

 return;

 var textColor = TextColor.Convert();

 var mainTextRectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)();

 var additionalTextRectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)();

 var firstLine = string.Empty;

 if (showSecondLine && !string.IsNullOrEmpty(AdditionalText))

 {

 var size = context.MeasureString(AdditionalText, AdditionalFont.Font);

 additionalTextRectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(0, 0, (int)size.Width, (int)size.Height);

 }

 if (ShowInstrument || ShowPeriod)

 {

 if (ShowInstrument)

 firstLine = [InstrumentInfo](../api/classes/classATAS_1_1Indicators_1_1InstrumentInfo.md).[Instrument](../api/classes/classATAS_1_1Indicators_1_1InstrumentInfo.md#a7464373ff790799fbb3ce1e79a43fba7);

 if (ShowPeriod)

 {

 var period = ChartInfo.ChartType == "TimeFrame" ? ChartInfo.TimeFrame : $"{ChartInfo.ChartType} {ChartInfo.TimeFrame}";

 if (ShowInstrument)

 firstLine += $", {period}";

 else

 firstLine += $"{period}";

 }

 var size = context.MeasureString(firstLine, Font.Font);

 mainTextRectangle = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(0, 0, (int)size.Width, (int)size.Height);

 }

 if (mainTextRectangle.Height > 0 && additionalTextRectangle.Height > 0)

 {

 int firstLineX;

 int secondLineX;

 var y = 0;

 var totalHeight = mainTextRectangle.Height + additionalTextRectangle.Height + AdditionalTextYOffset;

 switch (TextLocation)

 {

 case Location.Center:

 {

 firstLineX = ChartInfo.PriceChartContainer.Region.Width / 2 - mainTextRectangle.Width / 2 + HorizontalOffset;

 secondLineX = ChartInfo.PriceChartContainer.Region.Width / 2 - additionalTextRectangle.Width / 2 + HorizontalOffset;

 y = ChartInfo.PriceChartContainer.Region.Height / 2 - totalHeight / 2 + VerticalOffset;

 break;

 }

 case Location.TopLeft:

 {

 firstLineX = secondLineX = HorizontalOffset;

 break;

 }

 case Location.TopRight:

 {

 firstLineX = ChartInfo.PriceChartContainer.Region.Width - mainTextRectangle.Width + HorizontalOffset;

 secondLineX = ChartInfo.PriceChartContainer.Region.Width - additionalTextRectangle.Width + HorizontalOffset;

 break;

 }

 case Location.BottomLeft:

 {

 firstLineX = secondLineX = HorizontalOffset;

 y = ChartInfo.PriceChartContainer.Region.Height - totalHeight + VerticalOffset;

 break;

 }

 case Location.BottomRight:

 {

 firstLineX = ChartInfo.PriceChartContainer.Region.Width - mainTextRectangle.Width + HorizontalOffset;

 secondLineX = ChartInfo.PriceChartContainer.Region.Width - additionalTextRectangle.Width + HorizontalOffset;

 y = ChartInfo.PriceChartContainer.Region.Height - totalHeight + VerticalOffset;

 break;

 }

 default:

 throw new ArgumentOutOfRangeException();

 }

 context.DrawString(firstLine, Font.Font, textColor, firstLineX, y);

 context.DrawString(AdditionalText, AdditionalFont.Font, textColor, secondLineX, y + mainTextRectangle.Height + AdditionalTextYOffset);

 }

 else if (mainTextRectangle.Height > 0)

 {

 DrawString(context, firstLine, Font.Font, textColor, mainTextRectangle);

 }

 else if (additionalTextRectangle.Height > 0)

 {

 DrawString(context, AdditionalText, AdditionalFont.Font, textColor, additionalTextRectangle);

 }

 }

 private void DrawString(RenderContext context, string text, RenderFont font, System.Drawing.Color color, Rectangle rectangle)

 {

 switch (TextLocation)

 {

 case Location.Center:

 {

 context.DrawString(text, font, color, ChartInfo.PriceChartContainer.Region.Width / 2 - rectangle.Width / 2 + HorizontalOffset,

 ChartInfo.PriceChartContainer.Region.Height / 2 - rectangle.Height / 2 + VerticalOffset);

 break;

 }

 case Location.TopLeft:

 {

 context.DrawString(text, font, color, HorizontalOffset, VerticalOffset);

 break;

 }

 case Location.TopRight:

 {

 context.DrawString(text, font, color, ChartInfo.PriceChartContainer.Region.Width - rectangle.Width + HorizontalOffset, VerticalOffset);

 break;

 }

 case Location.BottomLeft:

 {

 context.DrawString(text, font, color, HorizontalOffset, ChartInfo.PriceChartContainer.Region.Height - rectangle.Height + VerticalOffset);

 break;

 }

 case Location.BottomRight:

 {

 context.DrawString(text, font, color, ChartInfo.PriceChartContainer.Region.Width - rectangle.Width + HorizontalOffset,

 ChartInfo.PriceChartContainer.Region.Height - rectangle.Height + VerticalOffset);

 break;

 }

 default:

 throw new ArgumentOutOfRangeException();

 }

 }

 #endregion

 }

}

[ATAS.DataFeedsCore.Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

Represents an order for trading on a financial exchange.

Definition Order.cs:13

[ATAS.Indicators.InstrumentInfo](../api/classes/classATAS_1_1Indicators_1_1InstrumentInfo.md)

Implementation of the IInstrumentInfo interface representing instrument information.

Definition InstrumentInfo.cs:33

[ATAS.Indicators.InstrumentInfo.Instrument](../api/classes/classATAS_1_1Indicators_1_1InstrumentInfo.md#a7464373ff790799fbb3ce1e79a43fba7)

string Instrument

Gets the name of the instrument.

Definition InstrumentInfo.cs:35

Watermark

# Maximum Levels

namespace ATAS.Indicators.Technical

{

 using System;

 using System.ComponentModel;

 using System.ComponentModel.DataAnnotations;

 using System.Drawing;

 using ATAS.Indicators.Properties;

 using OFT.Rendering.Context;

 using OFT.Rendering.Tools;

 using Utils.Common.Attributes;

 using Utils.Common.Localization;

 [DisplayName("Maximum Levels")]

 [Category("Clusters, Profiles, Levels")]

 public class MaxLevels : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

 {

 #region Nested types

 public enum MaxLevelType

 {

 Bid,

 Ask,

 PositiveDelta,

 NegativeDelta,

 Volume,

 Tick,

 Time

 };

 #endregion

 #region Private

 private RenderStringFormat _stringRightFormat = new RenderStringFormat

 {

 Alignment = StringAlignment.Far,

 LineAlignment = StringAlignment.Center,

 Trimming = StringTrimming.EllipsisCharacter,

 FormatFlags = StringFormatFlags.NoWrap

 };

 private bool _candleRequested;

 private [IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) _candle;

 private Color _lineColor = System.Drawing.Color.CornflowerBlue;

 private Color _axisTextColor = System.Drawing.Color.White;

 private Color _textColor = System.Drawing.Color.Black;

 private RenderPen _renderPen = new RenderPen(System.Drawing.Color.CornflowerBlue, 2);

 private int _width = 2;

 private [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) _period = [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).CurrentDay;

 private RenderFont _font = new RenderFont("Arial", 8);

 private string _description = "Current Day";

 #endregion

 #region Properties

 public [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) Period

 {

 get => _period;

 set

 {

 _period = value;

 _description = GetPeriodDescription(_period);

 RecalculateValues();

 }

 }

 public MaxLevelType Type { get; set; } = MaxLevelType.Volume;

 public System.Windows.Media.Color Color

 {

 get => _lineColor.Convert();

 set

 {

 _lineColor = value.Convert();

 _renderPen = new RenderPen(_lineColor, _width);

 }

 }

 public int Width

 {

 get => _width;

 set

 {

 _width = Math.Max(1, value);

 _renderPen = new RenderPen(_lineColor, _width);

 }

 }

 public System.Windows.Media.Color AxisTextColor

 {

 get => _axisTextColor.Convert();

 set => _axisTextColor = value.Convert();

 }

 public bool ShowText { get; set; } = true;

 public System.Windows.Media.Color TextColor

 {

 get => _textColor.Convert();

 set => _textColor = value.Convert();

 }

 public int FontSize

 {

 get => (int)_font.Size;

 set => _font = new RenderFont("Arial", Math.Max(7, value));

 }

 #endregion

 public MaxLevels()

 : base(true)

 {

 DataSeries[0].IsHidden = true;

 DenyToChangePanel = true;

 EnableCustomDrawing = true;

 SubscribeToDrawingEvents([DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92).LatestBar);

 DrawAbovePrice = true;

 Width = Width;

 }

 #region Overrides of BaseIndicator

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == 0)

 _candleRequested = false;

 if (!_candleRequested && bar == CurrentBar - 1)

 {

 _candleRequested = true;

 GetFixedProfile(new [FixedProfileRequest](../api/classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md)(Period));

 }

 }

 protected override void OnFixedProfilesResponse([IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) fixedProfile, [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) period)

 {

 _candle = fixedProfile;

 RedrawChart();

 }

 protected override void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

 {

 if (_candle == null)

 return;

 var priceInfo = GetPriceVolumeInfo(_candle, Type);

 if (priceInfo == null)

 return;

 var y = ChartInfo.GetYByPrice(priceInfo.Price);

 var firstX = ChartInfo.PriceChartContainer.Region.Width / 2;

 var secondX = ChartInfo.PriceChartContainer.Region.Width;

 context.DrawLine(_renderPen, firstX, y, secondX, y);

 if (ShowText)

 {

 var size = context.MeasureString(_description, _font);

 var textRect = new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(new Point(ChartInfo.PriceChartContainer.Region.Width - (int)size.Width - 20, y - (int)size.Height - Width / 2),

 new Size((int)size.Width + 20, (int)size.Height));

 context.DrawString(_description, _font, _textColor, textRect, _stringRightFormat);

 }

 this.DrawLabelOnPriceAxis(context, string.Format(ChartInfo.StringFormat, priceInfo.Price), y, _font, _lineColor, _axisTextColor);

 }

 #endregion

 private string GetPeriodDescription([FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) period)

 {

 switch (period)

 {

 case [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).CurrentDay:

 return "Current Day";

 case [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).LastDay:

 return "Last Day";

 case [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).CurrentWeek:

 return "Current Week";

 case [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).LastWeek:

 return "Last Week";

 case [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).CurrentMonth:

 return "Current Month";

 case [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).LastMonth:

 return "Last Month";

 case [FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8).Contract:

 return "Contract";

 default:

 throw new ArgumentOutOfRangeException(nameof(period), period, null);

 }

 }

 private [PriceVolumeInfo](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) GetPriceVolumeInfo([IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) candle, MaxLevelType levelType)

 {

 switch (Type)

 {

 case MaxLevelType.[Bid](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#a604834190bd71680b241ef735184762d):

 {

 return _candle.[MaxBidPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#aaa210fdfc9cd8f3ec1125bb7f5512f85);

 }

 case MaxLevelType.[Ask](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#a33bab2e20ce027e483463467dcba5012):

 {

 return _candle.[MaxAskPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3ea84ea366e936e347ea82f12bed17e5);

 }

 case MaxLevelType.PositiveDelta:

 {

 return _candle.[MaxPositiveDeltaPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3c4ef67bb640c8570f16c798a90ed6e2);

 }

 case MaxLevelType.NegativeDelta:

 {

 return _candle.[MaxNegativeDeltaPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ab322ca90826ea2437379e6101e48496e);

 }

 case MaxLevelType.[Volume](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#ac32ae8c41e6265821173bf1ee48ae58b):

 {

 return _candle.[MaxVolumePriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6a02e9c11fd7379fa28bac67ebee4c8e);

 }

 case MaxLevelType.Tick:

 {

 return _candle.[MaxTickPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6fe87b7dae24861c3b842149bf0304ad);

 }

 case MaxLevelType.[Time](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#af21750fba25d34c704aa8c7f7197451a):

 {

 return _candle.[MaxTimePriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#aa8ecffff9340501e4a29a64fbc0a64e7);

 }

 default:

 throw new ArgumentOutOfRangeException();

 }

 }

 }

}

[ATAS.Indicators.FixedProfileRequest](../api/classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md)

Represents a request for a fixed profile with a specific period.

Definition PriceVolumeInfo.cs:289

[ATAS.Indicators.IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md)

Represents an Indicator Candle.

Definition IndicatorCandle.cs:10

[ATAS.Indicators.IndicatorCandle.MaxPositiveDeltaPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3c4ef67bb640c8570f16c798a90ed6e2)

PriceVolumeInfo MaxPositiveDeltaPriceInfo

Gets the PriceVolumeInfo object with the maximum positive delta.

Definition IndicatorCandle.cs:161

[ATAS.Indicators.IndicatorCandle.MaxAskPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3ea84ea366e936e347ea82f12bed17e5)

PriceVolumeInfo MaxAskPriceInfo

Gets the PriceVolumeInfo object with the maximum ask price.

Definition IndicatorCandle.cs:151

[ATAS.Indicators.IndicatorCandle.MaxVolumePriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6a02e9c11fd7379fa28bac67ebee4c8e)

PriceVolumeInfo MaxVolumePriceInfo

Gets the PriceVolumeInfo object with the maximum volume.

Definition IndicatorCandle.cs:145

[ATAS.Indicators.IndicatorCandle.MaxTickPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6fe87b7dae24861c3b842149bf0304ad)

PriceVolumeInfo MaxTickPriceInfo

Gets the PriceVolumeInfo object with the maximum tick count.

Definition IndicatorCandle.cs:148

[ATAS.Indicators.IndicatorCandle.MaxTimePriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#aa8ecffff9340501e4a29a64fbc0a64e7)

PriceVolumeInfo MaxTimePriceInfo

Gets the PriceVolumeInfo object with the maximum time.

Definition IndicatorCandle.cs:158

[ATAS.Indicators.IndicatorCandle.MaxBidPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#aaa210fdfc9cd8f3ec1125bb7f5512f85)

PriceVolumeInfo MaxBidPriceInfo

Gets the PriceVolumeInfo object with the maximum bid price.

Definition IndicatorCandle.cs:154

[ATAS.Indicators.IndicatorCandle.MaxNegativeDeltaPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ab322ca90826ea2437379e6101e48496e)

PriceVolumeInfo MaxNegativeDeltaPriceInfo

Gets the PriceVolumeInfo object with the maximum negative delta.

Definition IndicatorCandle.cs:164

[ATAS.Indicators.PriceVolumeInfo](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md)

Represents information on volumes at a specific price.

Definition PriceVolumeInfo.cs:14

[ATAS.Indicators.PriceVolumeInfo.Ask](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#a33bab2e20ce027e483463467dcba5012)

decimal Ask

Gets or sets the ask data.

Definition PriceVolumeInfo.cs:28

[ATAS.Indicators.PriceVolumeInfo.Bid](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#a604834190bd71680b241ef735184762d)

decimal Bid

Gets or sets the bid data.

Definition PriceVolumeInfo.cs:23

[ATAS.Indicators.PriceVolumeInfo.Volume](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#ac32ae8c41e6265821173bf1ee48ae58b)

decimal Volume

Gets or sets the volume data.

Definition PriceVolumeInfo.cs:18

[ATAS.Indicators.PriceVolumeInfo.Time](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#af21750fba25d34c704aa8c7f7197451a)

int Time

Gets or sets the time data at the current level.

Definition PriceVolumeInfo.cs:43

[ATAS.Indicators.FixedProfilePeriods](../api/namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8)

FixedProfilePeriods

Enumeration representing fixed profile periods.

Definition PriceVolumeInfo.cs:241
