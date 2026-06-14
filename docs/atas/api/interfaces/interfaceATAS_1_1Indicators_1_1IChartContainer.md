# ATAS.Indicators.IChartContainer Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IChartContainer.html

Interface for a chart container that holds chart-related information and methods.
 [More...](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#details)

Inheritance diagram for ATAS.Indicators.IChartContainer:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IChartContainer:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| int | [GetYByPrice](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#ae3f75c55508da9a16649cbb184976ae8) (decimal price, bool isStartOfPriceLevel) |
| | Gets the Y-coordinate value corresponding to the specified price within the chart container. |
| | |
| decimal | [GetPriceByY](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a18a2606f25ba6da72d19899e79ea52ee) (int y) |
| | Gets the price value corresponding to the specified Y coordinate within the chart container. |
| | |
| int | [GetXByBarNumber](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a74df8516d086a309cb2a033a2ac1a313) (int i) |
| | Gets the X-coordinate value corresponding to the specified bar number within the chart container. |
| | |
| int | [GetXByBar](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a9be0786911691b0267478f89176661e9) (int bar, bool isStartOfBar=true) |
| | Gets the X-coordinate value corresponding to the specified bar within the chart container. |
| | |
| void | [SetCustomBarsSpacing](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a742e722bf069b9f7dc7648e2ba5a6e4b) (decimal? value) |
| | Sets custom spacing between bars. |
| | |
| void | [SetCustomBarsWidth](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#abc5f6110efe886abcedb4f40a6a60cb2) (decimal value, bool freezeValue) |
| | Sets custom bars width. |
| | |

| Properties | |
| --- | --- |
| decimal | [High](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#adadeb5c36dd6d901a3640f293e4811a3)`[get]` |
| | Gets the highest price value within the chart container. |
| | |
| decimal | [Low](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a6fd3aade2c51d1058741f37ba3a07b86)`[get]` |
| | Gets the lowest price value within the chart container. |
| | |
| decimal | [Step](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a5cf9d59bed855d7deb51a1fd1249757f)`[get]` |
| | Gets the step value for the price levels within the chart container. |
| | |
| int | [PriceLevelsCount](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a7a82ccfa3878dfb8fd61469125bb2ffc)`[get]` |
| | Gets the number of visible price levels within the chart container. |
| | |
| decimal | [PriceRowHeight](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a95f750a4bd298d892bc103cae69b9016)`[get]` |
| | Gets the height of each price row within the chart container. |
| | |
| decimal | [BarsWidth](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a73e9b65eec663572a320296c02236a44)`[get]` |
| | Gets the width of the bars within the chart container. |
| | |
| decimal | [BarSpacing](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#abe499b38a9c1763f76d8a71abc4b094c)`[get]` |
| | Gets the spacing between bars within the chart container. |
| | |
| int | [GetVisibleBarsCount](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a71990c819f6689410d948280893c9a21)`[get]` |
| | Gets the number of visible bars within the chart container. |
| | |
| int | [FirstVisibleBarNumber](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a16ceabf84b7a49642a3fa43de7c8ce00)`[get]` |
| | Gets the bar number of the first visible bar within the chart container. |
| | |
| int | [LastVisibleBarNumber](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a3a36d452801ff405218f488a9fbbdad4)`[get]` |
| | Gets the bar number of the last visible bar within the chart container. |
| | |
| int | [TotalBars](./interfaceATAS_1_1Indicators_1_1IChartContainer.md#a247820a5176b427a9108ff5370c9a34e)`[get]` |
| | Gets the total number of bars within the chart container. |
| | |
| - Properties inherited from [ATAS.Indicators.IContainer](./interfaceATAS_1_1Indicators_1_1IContainer.md) | |
| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | [Region](./interfaceATAS_1_1Indicators_1_1IContainer.md#a2a127b6200f2b7e5e7ecbe090e047869)`[get]` |
| | Gets the rectangular region defined by the container. |
| | |

## Detailed Description

Interface for a chart container that holds chart-related information and methods.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetPriceByY()

| decimal ATAS.Indicators.IChartContainer.GetPriceByY | ( | int | y | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the price value corresponding to the specified Y coordinate within the chart container.

Parameters

| y | The Y-coordinate |
| --- | --- |

Returns

## [◆](https://docs.atas.net/en/)GetXByBar()

| int ATAS.Indicators.IChartContainer.GetXByBar | ( | int | bar, |
| --- | --- | --- | --- |
| | | bool | isStartOfBar = `true` |
| | ) | | |

Gets the X-coordinate value corresponding to the specified bar within the chart container.

Parameters

| bar | The bar to get the X-coordinate for. |
| --- | --- |
| isStartOfBar | Flag indicating if the X-coordinate corresponds to the start of the bar. |

ReturnsThe X-coordinate value corresponding to the specified bar.

## [◆](https://docs.atas.net/en/)GetXByBarNumber()

| int ATAS.Indicators.IChartContainer.GetXByBarNumber | ( | int | i | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the X-coordinate value corresponding to the specified bar number within the chart container.

Parameters

| i | The bar number to get the X-coordinate for. |
| --- | --- |

ReturnsThe X-coordinate value corresponding to the specified bar number.

## [◆](https://docs.atas.net/en/)GetYByPrice()

| int ATAS.Indicators.IChartContainer.GetYByPrice | ( | decimal | price, |
| --- | --- | --- | --- |
| | | bool | isStartOfPriceLevel |
| | ) | | |

Gets the Y-coordinate value corresponding to the specified price within the chart container.

Parameters

| price | The price value to get the Y-coordinate for. |
| --- | --- |
| isStartOfPriceLevel | Flag indicating if the price level is the start of a row. |

ReturnsThe Y-coordinate value corresponding to the specified price.

## [◆](https://docs.atas.net/en/)SetCustomBarsSpacing()

| void ATAS.Indicators.IChartContainer.SetCustomBarsSpacing | ( | decimal? | value | ) | |
| --- | --- | --- | --- | --- | --- |

Sets custom spacing between bars.

Parameters

| value | Spacing between bars. Null enables automatic mode |
| --- | --- |

## [◆](https://docs.atas.net/en/)SetCustomBarsWidth()

| void ATAS.Indicators.IChartContainer.SetCustomBarsWidth | ( | decimal | value, |
| --- | --- | --- | --- |
| | | bool | freezeValue |
| | ) | | |

Sets custom bars width.

Parameters

| value | Bars width. Null enables automatic mode |
| --- | --- |
| freezeValue | If true, customer won't be able to change bars' width from the chart |

## Property Documentation

## [◆](https://docs.atas.net/en/)BarSpacing

| decimal ATAS.Indicators.IChartContainer.BarSpacing |
| --- |

get

Gets the spacing between bars within the chart container.

## [◆](https://docs.atas.net/en/)BarsWidth

| decimal ATAS.Indicators.IChartContainer.BarsWidth |
| --- |

get

Gets the width of the bars within the chart container.

## [◆](https://docs.atas.net/en/)FirstVisibleBarNumber

| int ATAS.Indicators.IChartContainer.FirstVisibleBarNumber |
| --- |

get

Gets the bar number of the first visible bar within the chart container.

## [◆](https://docs.atas.net/en/)GetVisibleBarsCount

| int ATAS.Indicators.IChartContainer.GetVisibleBarsCount |
| --- |

get

Gets the number of visible bars within the chart container.

## [◆](https://docs.atas.net/en/)High

| decimal ATAS.Indicators.IChartContainer.High |
| --- |

get

Gets the highest price value within the chart container.

## [◆](https://docs.atas.net/en/)LastVisibleBarNumber

| int ATAS.Indicators.IChartContainer.LastVisibleBarNumber |
| --- |

get

Gets the bar number of the last visible bar within the chart container.

## [◆](https://docs.atas.net/en/)Low

| decimal ATAS.Indicators.IChartContainer.Low |
| --- |

get

Gets the lowest price value within the chart container.

## [◆](https://docs.atas.net/en/)PriceLevelsCount

| int ATAS.Indicators.IChartContainer.PriceLevelsCount |
| --- |

get

Gets the number of visible price levels within the chart container.

## [◆](https://docs.atas.net/en/)PriceRowHeight

| decimal ATAS.Indicators.IChartContainer.PriceRowHeight |
| --- |

get

Gets the height of each price row within the chart container.

## [◆](https://docs.atas.net/en/)Step

| decimal ATAS.Indicators.IChartContainer.Step |
| --- |

get

Gets the step value for the price levels within the chart container.

Step could differ from the TickSize instrument if the Scale is used.

## [◆](https://docs.atas.net/en/)TotalBars

| int ATAS.Indicators.IChartContainer.TotalBars |
| --- |

get

Gets the total number of bars within the chart container.

The documentation for this interface was generated from the following file:
- [IIndicatorContainer.cs](../files/IIndicatorContainer_8cs.md)
