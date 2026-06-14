# ATAS.Indicators.Drawing.DrawingText Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.html

Represents a class for drawing text on a chart with additional alignment options.
 [More...](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#details)

Inheritance diagram for ATAS.Indicators.Drawing.DrawingText:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Drawing.DrawingText:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Types | |
| --- | --- |
| enum | [TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) { [Left](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192aa945d5e233cf7d6240f6b783b36a374ff)
, [Right](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192aa92b09c7c48c520c3c55e497875da437c)
, [Center](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192aa4f1f6016fc9f3f2353c0cc7c67b292bd)
 } |
| | Gets or sets the alignment of the text. [More...](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) |
| | |

| Public Member Functions | |
| --- | --- |
| | [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#af33d0208c18489aa3948d4c4c24b6506) (decimal tickSize) |
| | Initializes a new instance of the DrawingText class with the specified tick size. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.Drawing.BaseDrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md) | |
| override string | [ToString](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md#a68188f22648c3c0c829ffd54180e3493) () |
| | Returns a string representation of the object. |
| | |

| Properties | |
| --- | --- |
| [TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) | [Align](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#a713df344e9a759bec46f8fea410522e1) = [TextAlign.Center](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192aa4f1f6016fc9f3f2353c0cc7c67b292bd)`[get, set]` |
| | Gets or sets the text alignment. |
| | |
| int | [Price](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#a6707bf08ea8e5230edf95ed292fba318)`[get, set]` |
| | Gets or sets the price value associated with the text. |
| | |
| decimal | [TickSize](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#a067412cc25c31ca222dfe7074e9038ef)`[get]` |
| | Gets the tick size value used for calculations. |
| | |
| decimal | [TextPrice](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#a35ee6b2a521e8e6cdeb149a3d2787e88)`[get, set]` |
| | Gets or sets the price value associated with the text (in decimal). |
| | |
| - Properties inherited from [ATAS.Indicators.Drawing.BaseDrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md) | |
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

Represents a class for drawing text on a chart with additional alignment options.

## Member Enumeration Documentation

## [◆](https://docs.atas.net/en/)TextAlign

| enum [ATAS.Indicators.Drawing.DrawingText.TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) |
| --- |

Gets or sets the alignment of the text.

| Enumerator | |
| --- | --- |
| Left | The text is aligned to the left side. |
| Right | The text is aligned to the right side. |
| Center | The text is centered. |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)DrawingText()

| ATAS.Indicators.Drawing.DrawingText.DrawingText | ( | decimal | tickSize | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the DrawingText class with the specified tick size.

Parameters

| tickSize | The tick size used for calculations. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Align

| [TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) ATAS.Indicators.Drawing.DrawingText.Align = [TextAlign.Center](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192aa4f1f6016fc9f3f2353c0cc7c67b292bd) |
| --- |

getset

Gets or sets the text alignment.

## [◆](https://docs.atas.net/en/)Price

| int ATAS.Indicators.Drawing.DrawingText.Price |
| --- |

getset

Gets or sets the price value associated with the text.

## [◆](https://docs.atas.net/en/)TextPrice

| decimal ATAS.Indicators.Drawing.DrawingText.TextPrice |
| --- |

getset

Gets or sets the price value associated with the text (in decimal).

## [◆](https://docs.atas.net/en/)TickSize

| decimal ATAS.Indicators.Drawing.DrawingText.TickSize |
| --- |

get

Gets the tick size value used for calculations.

The documentation for this class was generated from the following file:
- [TrendLine.cs](../files/TrendLine_8cs.md)
