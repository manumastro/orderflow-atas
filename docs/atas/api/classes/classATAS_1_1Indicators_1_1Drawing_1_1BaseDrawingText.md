# ATAS.Indicators.Drawing.BaseDrawingText Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.html

Represents a base class for drawing text on a chart.
 [More...](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#details)

Inheritance diagram for ATAS.Indicators.Drawing.BaseDrawingText:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| override string | [ToString](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a68188f22648c3c0c829ffd54180e3493) () |
| | Returns a string representation of the object. |
| | |

| Properties | |
| --- | --- |
| int | [XOffset](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a5832bbf3056aa242d3b283e9ade79891)`[get, set]` |
| | Gets or sets the X-axis offset of the text. |
| | |
| string | [Tag](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a49e185fd3e7f6bdec95200e9e2b63a29) = string.Empty`[get, set]` |
| | Gets or sets a tag associated with the text. |
| | |
| string | [Text](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a982b10796f09ed67e6b1870fba0f82df) = string.Empty`[get, set]` |
| | Gets or sets the text to be displayed. |
| | |
| int | [Bar](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a2f94bc144eaa455cf50bca2b467bb5f0)`[get, set]` |
| | Gets or sets the index of the bar where the text is displayed. |
| | |
| int | [YOffset](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#afa2f6bfb3c2bc7e4b9d5b86fe458c258)`[get, set]` |
| | Gets or sets the Y-axis offset of the text. |
| | |
| bool | [IsAbovePrice](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a4c46ed982913277cbdda5bfad4feeb3e)`[get, set]` |
| | Gets or sets a value indicating whether the text is displayed above the price. |
| | |
| Color | [Textcolor](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#ab58bfde2314eda9eca171cc640c17f42)`[get, set]` |
| | Gets or sets the color of the text. |
| | |
| Color | [Outlinecolor](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a3e203acf181cadc364a608f36797d861)`[get, set]` |
| | Gets or sets the color of the outline of the text. |
| | |
| Color | [FillColor](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#aa94babd557426cede75775fce1982f4c)`[get, set]` |
| | Gets or sets the fill color of the text. |
| | |
| bool | [AutoSize](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a6479fa7a62b09728c4ad037ccf2f27bf)`[get, set]` |
| | Gets or sets a value indicating whether the text size is automatically adjusted. |
| | |
| float | [FontSize](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#aee8bd4bd46654bed5d9fd8fcf8eb59d3)`[get, set]` |
| | Gets or sets the font size of the text. |
| | |
| [CrossFont](../files/Indicators_2GlobalUsings_8cs.md#ad91096c91953905dc2c2d40748c80f29) | [TextFont](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a97e493ce572a9d2cb3f6c3e985128497) = new ("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 204)`[get, set]` |
| | Gets or sets the font used for the text. |
| | |

## Detailed Description

Represents a base class for drawing text on a chart.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.Indicators.Drawing.BaseDrawingText.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string representation of the object.

ReturnsA string representation of the object.

## Property Documentation

## [◆](https://docs.atas.net/en/)AutoSize

| bool ATAS.Indicators.Drawing.BaseDrawingText.AutoSize |
| --- |

getset

Gets or sets a value indicating whether the text size is automatically adjusted.

## [◆](https://docs.atas.net/en/)Bar

| int ATAS.Indicators.Drawing.BaseDrawingText.Bar |
| --- |

getset

Gets or sets the index of the bar where the text is displayed.

## [◆](https://docs.atas.net/en/)FillColor

| Color ATAS.Indicators.Drawing.BaseDrawingText.FillColor |
| --- |

getset

Gets or sets the fill color of the text.

## [◆](https://docs.atas.net/en/)FontSize

| float ATAS.Indicators.Drawing.BaseDrawingText.FontSize |
| --- |

getset

Gets or sets the font size of the text.

## [◆](https://docs.atas.net/en/)IsAbovePrice

| bool ATAS.Indicators.Drawing.BaseDrawingText.IsAbovePrice |
| --- |

getset

Gets or sets a value indicating whether the text is displayed above the price.

## [◆](https://docs.atas.net/en/)Outlinecolor

| Color ATAS.Indicators.Drawing.BaseDrawingText.Outlinecolor |
| --- |

getset

Gets or sets the color of the outline of the text.

## [◆](https://docs.atas.net/en/)Tag

| string ATAS.Indicators.Drawing.BaseDrawingText.Tag = string.Empty |
| --- |

getset

Gets or sets a tag associated with the text.

## [◆](https://docs.atas.net/en/)Text

| string ATAS.Indicators.Drawing.BaseDrawingText.Text = string.Empty |
| --- |

getset

Gets or sets the text to be displayed.

## [◆](https://docs.atas.net/en/)Textcolor

| Color ATAS.Indicators.Drawing.BaseDrawingText.Textcolor |
| --- |

getset

Gets or sets the color of the text.

## [◆](https://docs.atas.net/en/)TextFont

| [CrossFont](../files/Indicators_2GlobalUsings_8cs.md#ad91096c91953905dc2c2d40748c80f29) ATAS.Indicators.Drawing.BaseDrawingText.TextFont = new ("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 204) |
| --- |

getset

Gets or sets the font used for the text.

## [◆](https://docs.atas.net/en/)XOffset

| int ATAS.Indicators.Drawing.BaseDrawingText.XOffset |
| --- |

getset

Gets or sets the X-axis offset of the text.

## [◆](https://docs.atas.net/en/)YOffset

| int ATAS.Indicators.Drawing.BaseDrawingText.YOffset |
| --- |

getset

Gets or sets the Y-axis offset of the text.

The documentation for this class was generated from the following file:
- [TrendLine.cs](../files/TrendLine_8cs.md)
