# ATAS.Indicators.Drawing.TrendLine Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.html

Represents a trend line on a chart.
 [More...](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#details)

Inheritance diagram for ATAS.Indicators.Drawing.TrendLine:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [TrendLine](./classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md#a1866b30424a193554fb1fc23d53368c9) (int firstBar, decimal firstPrice, int secondBar, decimal secondPrice, [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) pen) |
| | Initializes a new instance of the TrendLine class. |
| | |

| Properties | |
| --- | --- |
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

Represents a trend line on a chart.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)TrendLine()

| ATAS.Indicators.Drawing.TrendLine.TrendLine | ( | int | firstBar, |
| --- | --- | --- | --- |
| | | decimal | firstPrice, |
| | | int | secondBar, |
| | | decimal | secondPrice, |
| | | [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | pen |
| | ) | | |

Initializes a new instance of the TrendLine class.

Parameters

| firstBar | The index of the first bar. |
| --- | --- |
| firstPrice | The price value of the first point. |
| secondBar | The index of the second bar. |
| secondPrice | The price value of the second point. |
| pen | The pen used to draw the trend line. |

## Property Documentation

## [◆](https://docs.atas.net/en/)FirstBar

| int ATAS.Indicators.Drawing.TrendLine.FirstBar |
| --- |

getset

Gets or sets the index of the first bar.

## [◆](https://docs.atas.net/en/)FirstPrice

| decimal ATAS.Indicators.Drawing.TrendLine.FirstPrice |
| --- |

getset

Gets or sets the price value of the first point.

## [◆](https://docs.atas.net/en/)IsRay

| bool ATAS.Indicators.Drawing.TrendLine.IsRay |
| --- |

getset

Gets or sets a value indicating whether the trend line is displayed as a ray.

## [◆](https://docs.atas.net/en/)Pen

| [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) ATAS.Indicators.Drawing.TrendLine.Pen |
| --- |

getset

Gets or sets the pen used to draw the trend line.

## [◆](https://docs.atas.net/en/)SecondBar

| int ATAS.Indicators.Drawing.TrendLine.SecondBar |
| --- |

getset

Gets or sets the index of the second bar.

## [◆](https://docs.atas.net/en/)SecondPrice

| decimal ATAS.Indicators.Drawing.TrendLine.SecondPrice |
| --- |

getset

Gets or sets the price value of the second point.

The documentation for this class was generated from the following file:
- [TrendLine.cs](../files/TrendLine_8cs.md)
