# ATAS.Indicators.IMouseLocationInfo Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.html

Interface for providing information about the mouse location within the chart.
 [More...](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md#details)

| Properties | |
| --- | --- |
| decimal | [PriceBelowMouse](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md#a2c73e25ed461008d91fc0fe776b2cbd8)`[get]` |
| | Gets the price value below the mouse cursor in the chart. |
| | |
| int | [BarBelowMouse](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md#a47aabba42984bf45a7fbb969f4aa0370)`[get]` |
| | Gets the bar number below the mouse cursor in the chart. |
| | |
| Point | [LastPosition](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md#a574373436764092dedda3358273f315b)`[get]` |
| | Gets the last position of the mouse cursor within the chart. |
| | |
| bool | [IsMouseLeave](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md#ab9acf69e5bc139eddb3706064df1f12d)`[get, set]` |
| | Gets or sets a value indicating whether the mouse has left the chart area. |
| | |
| bool | [IsMovingChartUsingMouse](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md#a527838f674074ed78a618543874d231a)`[get, set]` |
| | Gets or sets a value indicating whether the chart is being moved using the mouse. |
| | |

## Detailed Description

Interface for providing information about the mouse location within the chart.

## Property Documentation

## [◆](https://docs.atas.net/en/)BarBelowMouse

| int ATAS.Indicators.IMouseLocationInfo.BarBelowMouse |
| --- |

get

Gets the bar number below the mouse cursor in the chart.

## [◆](https://docs.atas.net/en/)IsMouseLeave

| bool ATAS.Indicators.IMouseLocationInfo.IsMouseLeave |
| --- |

getset

Gets or sets a value indicating whether the mouse has left the chart area.

## [◆](https://docs.atas.net/en/)IsMovingChartUsingMouse

| bool ATAS.Indicators.IMouseLocationInfo.IsMovingChartUsingMouse |
| --- |

getset

Gets or sets a value indicating whether the chart is being moved using the mouse.

## [◆](https://docs.atas.net/en/)LastPosition

| Point ATAS.Indicators.IMouseLocationInfo.LastPosition |
| --- |

get

Gets the last position of the mouse cursor within the chart.

## [◆](https://docs.atas.net/en/)PriceBelowMouse

| decimal ATAS.Indicators.IMouseLocationInfo.PriceBelowMouse |
| --- |

get

Gets the price value below the mouse cursor in the chart.

The documentation for this interface was generated from the following file:
- [IIndicatorContainer.cs](../files/IIndicatorContainer_8cs.md)
