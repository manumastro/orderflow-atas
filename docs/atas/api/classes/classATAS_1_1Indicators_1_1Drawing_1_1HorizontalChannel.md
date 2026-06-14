# ATAS.Indicators.Drawing.HorizontalChannel Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.html

Represents a horizontal line on a chart.
 [More...](./classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md#details)

| Properties | |
| --- | --- |
| required decimal | [FirstPrice](./classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md#a29a16060e1079c17f601105e23f2d827)`[get, set]` |
| | Gets or sets the top price level of the horizontal channel. |
| | |
| required decimal | [SecondPrice](./classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md#a73e810ed5f1728a589b52483f86c6d4b)`[get, set]` |
| | Gets or sets the bottom price level of the horizontal channel. |
| | |
| required RenderPen | [Pen](./classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md#a24fdda7155cd40d2cead3205ee4e4d53) = new(DefaultColors.Gray)`[get, set]` |
| | Gets or sets the style of the borderlines. |
| | |
| Color | [Background](./classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md#a4e1b70dc8b9b217b3875d760ae5a0b6e) = Color.Transparent`[get, set]` |
| | Gets or sets the color of background. |
| | |
| RenderPen | [MiddlePen](./classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md#a31a679db16e22ba617b6800fa73a343c) = new(DefaultColors.Gray)`[get, set]` |
| | Gets or sets the style of middle horizontal line. |
| | |
| bool | [EnableMiddleLine](./classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md#ae84fe94b0d2cf5c2710efebba80d0015)`[get, set]` |
| | Gets or sets drawing of middle horizontal line. |
| | |

## Detailed Description

Represents a horizontal line on a chart.

## Property Documentation

## [◆](https://docs.atas.net/en/)Background

| Color ATAS.Indicators.Drawing.HorizontalChannel.Background = Color.Transparent |
| --- |

getset

Gets or sets the color of background.

## [◆](https://docs.atas.net/en/)EnableMiddleLine

| bool ATAS.Indicators.Drawing.HorizontalChannel.EnableMiddleLine |
| --- |

getset

Gets or sets drawing of middle horizontal line.

## [◆](https://docs.atas.net/en/)FirstPrice

| required decimal ATAS.Indicators.Drawing.HorizontalChannel.FirstPrice |
| --- |

getset

Gets or sets the top price level of the horizontal channel.

## [◆](https://docs.atas.net/en/)MiddlePen

| RenderPen ATAS.Indicators.Drawing.HorizontalChannel.MiddlePen = new(DefaultColors.Gray) |
| --- |

getset

Gets or sets the style of middle horizontal line.

## [◆](https://docs.atas.net/en/)Pen

| required RenderPen ATAS.Indicators.Drawing.HorizontalChannel.Pen = new(DefaultColors.Gray) |
| --- |

getset

Gets or sets the style of the borderlines.

## [◆](https://docs.atas.net/en/)SecondPrice

| required decimal ATAS.Indicators.Drawing.HorizontalChannel.SecondPrice |
| --- |

getset

Gets or sets the bottom price level of the horizontal channel.

The documentation for this class was generated from the following file:
- [TrendLine.cs](../files/TrendLine_8cs.md)
