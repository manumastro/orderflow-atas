# ATAS.Indicators.Drawing.LineTillTouch Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.html

Represents a trend line that extends until it is touched by the price.
 [More...](./classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md#details)

Inheritance diagram for ATAS.Indicators.Drawing.LineTillTouch:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Drawing.LineTillTouch:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [LineTillTouch](./classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md#a4b65bd636add92b73da496fdae041076) (int bar, decimal price, [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) pen) |
| | Initializes a new instance of the LineTillTouch class with a single point. |
| | |
| | [LineTillTouch](./classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md#ad825f36089cb10a8d095a485f872d43a) (int bar, decimal price, [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) pen, int fixedBarsCount) |
| | Initializes a new instance of the LineTillTouch class with a fixed number of bars. |
| | |
| void | [CheckIfTouched](./classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md#a473eb78fb4ae30c997de01899318fae9) (decimal high, decimal low, int bar, int lastBar) |
| | Checks if the trend line has been touched by the price within the specified high and low values. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.Drawing.TrendLine](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md) | |
| | [TrendLine](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#a1866b30424a193554fb1fc23d53368c9) (int firstBar, decimal firstPrice, int secondBar, decimal secondPrice, [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) pen) |
| | Initializes a new instance of the TrendLine class. |
| | |

| Properties | |
| --- | --- |
| bool | [Finished](./classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md#aa5ccac75d4e70919586cabb01fa70bda)`[get]` |
| | Gets a value indicating whether the trend line has been finished (touched). |
| | |
| object | [Context](./classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md#af83c62db424182e501edb197a02bbf55) = null!`[get, set]` |
| | Custom object context. |
| | |
| - Properties inherited from [ATAS.Indicators.Drawing.TrendLine](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md) | |
| [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | [Pen](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#a595ffa4c662f73c93ea39f5f9159ab47)`[get, set]` |
| | Gets or sets the pen used to draw the trend line. |
| | |
| int | [FirstBar](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#a79716241cd27f36d2def56516275eb33)`[get, set]` |
| | Gets or sets the index of the first bar. |
| | |
| int | [SecondBar](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#a965840c9740f690e0063cf9ab366e693)`[get, set]` |
| | Gets or sets the index of the second bar. |
| | |
| decimal | [FirstPrice](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#ad218abd70f6f064aa52c43e1faccef60)`[get, set]` |
| | Gets or sets the price value of the first point. |
| | |
| decimal | [SecondPrice](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#acdb3de6626d107155deb80f128814170)`[get, set]` |
| | Gets or sets the price value of the second point. |
| | |
| bool | [IsRay](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#aa38fc0920db3256cc3369469714e5c24)`[get, set]` |
| | Gets or sets a value indicating whether the trend line is displayed as a ray. |
| | |

## Detailed Description

Represents a trend line that extends until it is touched by the price.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)LineTillTouch() [1/2]

| ATAS.Indicators.Drawing.LineTillTouch.LineTillTouch | ( | int | bar, |
| --- | --- | --- | --- |
| | | decimal | price, |
| | | [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | pen |
| | ) | | |

Initializes a new instance of the LineTillTouch class with a single point.

Parameters

| bar | The index of the bar. |
| --- | --- |
| price | The price value of the point. |
| pen | The pen used to draw the trend line. |

## [◆](https://docs.atas.net/en/)LineTillTouch() [2/2]

| ATAS.Indicators.Drawing.LineTillTouch.LineTillTouch | ( | int | bar, |
| --- | --- | --- | --- |
| | | decimal | price, |
| | | [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | pen, |
| | | int | fixedBarsCount |
| | ) | | |

Initializes a new instance of the LineTillTouch class with a fixed number of bars.

Parameters

| bar | The index of the bar. |
| --- | --- |
| price | The price value of the starting point. |
| pen | The pen used to draw the trend line. |
| fixedBarsCount | The fixed number of bars for the line. |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CheckIfTouched()

| void ATAS.Indicators.Drawing.LineTillTouch.CheckIfTouched | ( | decimal | high, |
| --- | --- | --- | --- |
| | | decimal | low, |
| | | int | bar, |
| | | int | lastBar |
| | ) | | |

Checks if the trend line has been touched by the price within the specified high and low values.

Parameters

| high | The highest price within the range to check. |
| --- | --- |
| low | The lowest price within the range to check. |
| bar | The current index of the bar. |
| lastBar | The index of the last bar. |

## Property Documentation

## [◆](https://docs.atas.net/en/)Context

| object ATAS.Indicators.Drawing.LineTillTouch.Context = null! |
| --- |

getset

Custom object context.

## [◆](https://docs.atas.net/en/)Finished

| bool ATAS.Indicators.Drawing.LineTillTouch.Finished |
| --- |

get

Gets a value indicating whether the trend line has been finished (touched).

The documentation for this class was generated from the following file:
- [TrendLine.cs](../files/TrendLine_8cs.md)
