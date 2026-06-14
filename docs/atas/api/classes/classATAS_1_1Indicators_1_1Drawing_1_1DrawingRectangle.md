# ATAS.Indicators.Drawing.DrawingRectangle Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.html

Represents a rectangle drawn on a chart.
 [More...](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#details)

| Public Member Functions | |
| --- | --- |
| | [DrawingRectangle](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a53ac7bf9cf94d7decd88763f4466dccd) (int firstBar, decimal firstPrice, int secondBar, decimal secondPrice, [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) outlinePen, [Brush](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a5d6f5903736c8a1496da521c50778429) fillBrush) |
| | Initializes a new instance of the DrawingRectangle class with specified parameters. |
| | |
| | [DrawingRectangle](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a36d8e44c4587cef8e1f970aefff1c79d) (int firstBar, decimal firstPrice, int secondBar, decimal secondPrice, [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) outlinePen, [Brush](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a5d6f5903736c8a1496da521c50778429) fillBrush, [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) midPen) |
| | Initializes a new instance of the DrawingRectangle class with specified parameters. |
| | |

| Properties | |
| --- | --- |
| Brush | [Brush](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a5d6f5903736c8a1496da521c50778429)`[get, set]` |
| | Gets or sets the brush used to fill the rectangle. |
| | |
| [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | [Pen](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a11a38f709032d9954c0506233b2e80a3)`[get, set]` |
| | Gets or sets the pen used to draw the outline of the rectangle. |
| | |
| [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | [MiddlePen](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a25c2077773e01919d10e00cccec57d94) = new(DefaultColors.Gray)`[get, set]` |
| | Gets or sets the pen used to draw the middle horizontal line of the rectangle. |
| | |
| int | [FirstBar](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#ae57dd842458f419c69bc6989fe805c8d)`[get, set]` |
| | Gets or sets the index of the first bar. |
| | |
| int | [SecondBar](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a22d03943788636fe63d5e19666367670)`[get, set]` |
| | Gets or sets the index of the second bar. |
| | |
| decimal | [FirstPrice](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#aa63a799a67cb183fcf54e2bb04be2dd3)`[get, set]` |
| | Gets or sets the price value of the first point. |
| | |
| decimal | [SecondPrice](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#acce6f07f935602b25f7154a0817881d5)`[get, set]` |
| | Gets or sets the price value of the second point. |
| | |
| bool | [ExtendRight](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a0e0d5714a4cdbe56390515b1785a63c4)`[get, set]` |
| | Gets or sets rectangle extension to right side. |
| | |
| bool | [ExtendLeft](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a1ca0b603df2462b9ac05b0209d0d8d8c)`[get, set]` |
| | Gets or sets rectangle extension to left side. |
| | |
| bool | [MidLineEnabled](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a469312a6e7411d5d8b4cf5f024d31503)`[get, set]` |
| | Gets or sets drawing of middle horizontal line of rectangle. |
| | |

## Detailed Description

Represents a rectangle drawn on a chart.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)DrawingRectangle() [1/2]

| ATAS.Indicators.Drawing.DrawingRectangle.DrawingRectangle | ( | int | firstBar, |
| --- | --- | --- | --- |
| | | decimal | firstPrice, |
| | | int | secondBar, |
| | | decimal | secondPrice, |
| | | [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | outlinePen, |
| | | [Brush](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a5d6f5903736c8a1496da521c50778429) | fillBrush |
| | ) | | |

Initializes a new instance of the DrawingRectangle class with specified parameters.

Parameters

| firstBar | The index of the first bar. |
| --- | --- |
| firstPrice | The price value of the first point. |
| secondBar | The index of the second bar. |
| secondPrice | The price value of the second point. |
| outlinePen | The pen used to draw the outline of the rectangle. |
| fillBrush | The brush used to fill the rectangle. |

## [◆](https://docs.atas.net/en/)DrawingRectangle() [2/2]

| ATAS.Indicators.Drawing.DrawingRectangle.DrawingRectangle | ( | int | firstBar, |
| --- | --- | --- | --- |
| | | decimal | firstPrice, |
| | | int | secondBar, |
| | | decimal | secondPrice, |
| | | [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | outlinePen, |
| | | [Brush](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md#a5d6f5903736c8a1496da521c50778429) | fillBrush, |
| | | [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) | midPen |
| | ) | | |

Initializes a new instance of the DrawingRectangle class with specified parameters.

Parameters

| firstBar | The index of the first bar. |
| --- | --- |
| firstPrice | The price value of the first point. |
| secondBar | The index of the second bar. |
| secondPrice | The price value of the second point. |
| outlinePen | The pen used to draw the outline of the rectangle. |
| fillBrush | The brush used to fill the rectangle. |
| midPen | The pen used to draw the middle horizontal line of the rectangle. |

## Property Documentation

## [◆](https://docs.atas.net/en/)Brush

| Brush ATAS.Indicators.Drawing.DrawingRectangle.Brush |
| --- |

getset

Gets or sets the brush used to fill the rectangle.

## [◆](https://docs.atas.net/en/)ExtendLeft

| bool ATAS.Indicators.Drawing.DrawingRectangle.ExtendLeft |
| --- |

getset

Gets or sets rectangle extension to left side.

## [◆](https://docs.atas.net/en/)ExtendRight

| bool ATAS.Indicators.Drawing.DrawingRectangle.ExtendRight |
| --- |

getset

Gets or sets rectangle extension to right side.

## [◆](https://docs.atas.net/en/)FirstBar

| int ATAS.Indicators.Drawing.DrawingRectangle.FirstBar |
| --- |

getset

Gets or sets the index of the first bar.

## [◆](https://docs.atas.net/en/)FirstPrice

| decimal ATAS.Indicators.Drawing.DrawingRectangle.FirstPrice |
| --- |

getset

Gets or sets the price value of the first point.

## [◆](https://docs.atas.net/en/)MiddlePen

| [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) ATAS.Indicators.Drawing.DrawingRectangle.MiddlePen = new(DefaultColors.Gray) |
| --- |

getset

Gets or sets the pen used to draw the middle horizontal line of the rectangle.

## [◆](https://docs.atas.net/en/)MidLineEnabled

| bool ATAS.Indicators.Drawing.DrawingRectangle.MidLineEnabled |
| --- |

getset

Gets or sets drawing of middle horizontal line of rectangle.

## [◆](https://docs.atas.net/en/)Pen

| [CrossPen](../files/Indicators_2GlobalUsings_8cs.md#a06bc62dace1bc62524acc6edc0a729eb) ATAS.Indicators.Drawing.DrawingRectangle.Pen |
| --- |

getset

Gets or sets the pen used to draw the outline of the rectangle.

## [◆](https://docs.atas.net/en/)SecondBar

| int ATAS.Indicators.Drawing.DrawingRectangle.SecondBar |
| --- |

getset

Gets or sets the index of the second bar.

## [◆](https://docs.atas.net/en/)SecondPrice

| decimal ATAS.Indicators.Drawing.DrawingRectangle.SecondPrice |
| --- |

getset

Gets or sets the price value of the second point.

The documentation for this class was generated from the following file:
- [TrendLine.cs](../files/TrendLine_8cs.md)
