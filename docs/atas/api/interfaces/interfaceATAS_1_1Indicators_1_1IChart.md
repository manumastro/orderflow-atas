# ATAS.Indicators.IChart Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IChart.html

Interface for a chart containing various chart-related information and methods.
 [More...](./interfaceATAS_1_1Indicators_1_1IChart.md#details)

Inheritance diagram for ATAS.Indicators.IChart:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IChart:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| string | [TryGetMinimizedVolumeString](./interfaceATAS_1_1Indicators_1_1IChart.md#a84fe59ac20986d91e80e202bd7ba73cb) (decimal value) |
| | Tries to get the string representation of the minimized volume for the specified value. |
| | |
| string | [TryGetMinimizedVolumeString](./interfaceATAS_1_1Indicators_1_1IChart.md#a1358c36b2f9ff7ffbc7ac665cb862713) (decimal value, decimal price) |
| | Tries to get the string representation of the minimized volume for the specified value. If instrument is in USD, price is used to convert volume to USD. |
| | |
| string | [GetPriceString](./interfaceATAS_1_1Indicators_1_1IChart.md#a204356803380ab4d94b247fad1c8ff15) (decimal value) |
| | Gets the string representation of specified price. |
| | |

| Properties | |
| --- | --- |
| [IChartColorsStore](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md) | [ColorsStore](./interfaceATAS_1_1Indicators_1_1IChart.md#ade75579aa63f898e84c8231c73ec1b90)`[get]` |
| | Gets the chart colors store providing various chart-related colors. |
| | |
| [IChartCoordinatesManager](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md) | [CoordinatesManager](./interfaceATAS_1_1Indicators_1_1IChart.md#a5c87f89e74e3e958e564a61d577c69b7)`[get]` |
| | Gets the chart coordinates manager responsible for managing chart coordinates and scaling. |
| | |
| [IMouseLocationInfo](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md) | [MouseLocationInfo](./interfaceATAS_1_1Indicators_1_1IChart.md#a2b453ffac2db8265bb5f86b00a3a134f)`[get]` |
| | Gets the information about the mouse location within the chart. |
| | |
| [IKeyboardInfo](./interfaceATAS_1_1Indicators_1_1IKeyboardInfo.md) | [KeyboardInfo](./interfaceATAS_1_1Indicators_1_1IChart.md#a0940c0b2b048afadb403e8f8771c0145)`[get]` |
| | Gets the information about the keyboard within the chart. |
| | |
| [IDrawingObjectsListInfo](./interfaceATAS_1_1Indicators_1_1IDrawingObjectsListInfo.md) | [DrawingObjectsListInfo](./interfaceATAS_1_1Indicators_1_1IChart.md#a22c9be54833347ce3a1c8f4fa78755b7)`[get]` |
| | Gets the list of drawing objects present in the chart. |
| | |
| [IChartContainer](./interfaceATAS_1_1Indicators_1_1IChartContainer.md) | [PriceChartContainer](./interfaceATAS_1_1Indicators_1_1IChart.md#ab2ce4a79734b9a560b2f665597d0a458)`[get]` |
| | Gets the container representing the area of the chart without the axis. |
| | |
| [IContainer](./interfaceATAS_1_1Indicators_1_1IContainer.md) | [ChartContainer](./interfaceATAS_1_1Indicators_1_1IChart.md#acf9503c67176bc020447a5320e8303d7)`[get]` |
| | Gets the container representing the full area of the chart. |
| | |
| RenderFont | [PriceAxisFont](./interfaceATAS_1_1Indicators_1_1IChart.md#a482bdb20843aa7deaf83e73f5780a091)`[get]` |
| | Gets the font of the chart price axis. |
| | |
| string | [StringFormat](./interfaceATAS_1_1Indicators_1_1IChart.md#a0cc9650edf1e34058720f42eeb56f393)`[get]` |
| | Gets the string format used within the chart. |
| | |
| string | [ChartType](./interfaceATAS_1_1Indicators_1_1IChart.md#aceed9146e0f22df4c6685241761fe2a7)`[get]` |
| | Gets the type of the chart. |
| | |
| string | [TimeFrame](./interfaceATAS_1_1Indicators_1_1IChart.md#a3b0d6e200543ecafcab363a1772fa2f4)`[get]` |
| | Gets the time frame of the chart. If a time-frame has several parameters, they are separated by a slash (/). |
| | |
| IEnumerable | [TradingSessionDescriptions](./interfaceATAS_1_1Indicators_1_1IChart.md#ad2078b9bea5311ee4649a657dc25ae8d)`[get]` |
| | Get the available trading sessions. |
| | |
| [ChartVisualModes](../namespaces/namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51) | [ChartVisualMode](./interfaceATAS_1_1Indicators_1_1IChart.md#a2f7abd6890435f50dd13e37364974346)`[get, set]` |
| | Gets or sets the visual mode of the chart. |
| | |
| [FootprintContentModes](../namespaces/namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945) | [FootprintContentMode](./interfaceATAS_1_1Indicators_1_1IChart.md#a83df2de5ae5b38816bf02b3ea17e7609)`[get, set]` |
| | Gets or sets footprint content mode. |
| | |
| [FootprintVisualModes](../namespaces/namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71) | [FootprintVisualMode](./interfaceATAS_1_1Indicators_1_1IChart.md#a78874ccbfdce794cc751bd5d0a1a544b)`[get, set]` |
| | Gets or sets footprint visual mode. |
| | |
| [FootprintColorSchemes](../namespaces/namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104f) | [FootprintColorScheme](./interfaceATAS_1_1Indicators_1_1IChart.md#a5ddc192b228b4b560b4f800931696d0b)`[get, set]` |
| | Gets or sets footprint color scheme. |
| | |
| bool | [HidePriceAxis](./interfaceATAS_1_1Indicators_1_1IChart.md#a590ed2dd005a3c3cd85b90a725e0c5ec)`[get, set]` |
| | Shows or hides price axis of the chart. |
| | |
| int | [StatusLineHorizontalOffset](./interfaceATAS_1_1Indicators_1_1IChart.md#a737f4d372ee697448d73979a3aec9faf)`[get, set]` |
| | Gets or sets status line horizontal offset. |
| | |
| int | [IndicatorsListVerticalOffset](./interfaceATAS_1_1Indicators_1_1IChart.md#aaab673759e729a7fde59eed139af0900)`[get, set]` |
| | Gets or sets vertical offset of indicators list. |
| | |
| int | [IndicatorsListHorizontalOffset](./interfaceATAS_1_1Indicators_1_1IChart.md#a993771507361fd7995cffabf50594c6c)`[get, set]` |
| | Gets or sets horizontal offset of indicators list. |
| | |
| decimal | [DpiY](./interfaceATAS_1_1Indicators_1_1IChart.md#ad17fb543c6ca9e76ee10bcb433d2ea31)`[get]` |
| | Gets the Y-coordinate dots per inch (DPI) value of the chart. |
| | |
| decimal | [DpiX](./interfaceATAS_1_1Indicators_1_1IChart.md#a50958777f38f6b90ce8bb6b7e432c031)`[get]` |
| | Gets the X-coordinate dots per inch (DPI) value of the chart. |
| | |
| - Properties inherited from [ATAS.Indicators.IContainer](./interfaceATAS_1_1Indicators_1_1IContainer.md) | |
| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | [Region](./interfaceATAS_1_1Indicators_1_1IContainer.md#a2a127b6200f2b7e5e7ecbe090e047869)`[get]` |
| | Gets the rectangular region defined by the container. |
| | |

## Detailed Description

Interface for a chart containing various chart-related information and methods.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetPriceString()

| string ATAS.Indicators.IChart.GetPriceString | ( | decimal | value | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the string representation of specified price.

Parameters

| value | |
| --- | --- |

Returns

## [◆](https://docs.atas.net/en/)TryGetMinimizedVolumeString() [1/2]

| string ATAS.Indicators.IChart.TryGetMinimizedVolumeString | ( | decimal | value | ) | |
| --- | --- | --- | --- | --- | --- |

Tries to get the string representation of the minimized volume for the specified value.

Parameters

| value | The volume value to get the minimized string representation for. |
| --- | --- |

ReturnsThe minimized string representation of the volume.

## [◆](https://docs.atas.net/en/)TryGetMinimizedVolumeString() [2/2]

| string ATAS.Indicators.IChart.TryGetMinimizedVolumeString | ( | decimal | value, |
| --- | --- | --- | --- |
| | | decimal | price |
| | ) | | |

Tries to get the string representation of the minimized volume for the specified value. If instrument is in USD, price is used to convert volume to USD.

Parameters

| value | The volume value to get the minimized string representation for. |
| --- | --- |
| value | Price |

ReturnsThe minimized string representation of the volume.

## Property Documentation

## [◆](https://docs.atas.net/en/)ChartContainer

| [IContainer](./interfaceATAS_1_1Indicators_1_1IContainer.md) ATAS.Indicators.IChart.ChartContainer |
| --- |

get

Gets the container representing the full area of the chart.

## [◆](https://docs.atas.net/en/)ChartType

| string ATAS.Indicators.IChart.ChartType |
| --- |

get

Gets the type of the chart.

## [◆](https://docs.atas.net/en/)ChartVisualMode

| [ChartVisualModes](../namespaces/namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51) ATAS.Indicators.IChart.ChartVisualMode |
| --- |

getset

Gets or sets the visual mode of the chart.

## [◆](https://docs.atas.net/en/)ColorsStore

| [IChartColorsStore](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md) ATAS.Indicators.IChart.ColorsStore |
| --- |

get

Gets the chart colors store providing various chart-related colors.

## [◆](https://docs.atas.net/en/)CoordinatesManager

| [IChartCoordinatesManager](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md) ATAS.Indicators.IChart.CoordinatesManager |
| --- |

get

Gets the chart coordinates manager responsible for managing chart coordinates and scaling.

## [◆](https://docs.atas.net/en/)DpiX

| decimal ATAS.Indicators.IChart.DpiX |
| --- |

get

Gets the X-coordinate dots per inch (DPI) value of the chart.

## [◆](https://docs.atas.net/en/)DpiY

| decimal ATAS.Indicators.IChart.DpiY |
| --- |

get

Gets the Y-coordinate dots per inch (DPI) value of the chart.

## [◆](https://docs.atas.net/en/)DrawingObjectsListInfo

| [IDrawingObjectsListInfo](./interfaceATAS_1_1Indicators_1_1IDrawingObjectsListInfo.md) ATAS.Indicators.IChart.DrawingObjectsListInfo |
| --- |

get

Gets the list of drawing objects present in the chart.

## [◆](https://docs.atas.net/en/)FootprintColorScheme

| [FootprintColorSchemes](../namespaces/namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104f) ATAS.Indicators.IChart.FootprintColorScheme |
| --- |

getset

Gets or sets footprint color scheme.

## [◆](https://docs.atas.net/en/)FootprintContentMode

| [FootprintContentModes](../namespaces/namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945) ATAS.Indicators.IChart.FootprintContentMode |
| --- |

getset

Gets or sets footprint content mode.

## [◆](https://docs.atas.net/en/)FootprintVisualMode

| [FootprintVisualModes](../namespaces/namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71) ATAS.Indicators.IChart.FootprintVisualMode |
| --- |

getset

Gets or sets footprint visual mode.

## [◆](https://docs.atas.net/en/)HidePriceAxis

| bool ATAS.Indicators.IChart.HidePriceAxis |
| --- |

getset

Shows or hides price axis of the chart.

## [◆](https://docs.atas.net/en/)IndicatorsListHorizontalOffset

| int ATAS.Indicators.IChart.IndicatorsListHorizontalOffset |
| --- |

getset

Gets or sets horizontal offset of indicators list.

## [◆](https://docs.atas.net/en/)IndicatorsListVerticalOffset

| int ATAS.Indicators.IChart.IndicatorsListVerticalOffset |
| --- |

getset

Gets or sets vertical offset of indicators list.

## [◆](https://docs.atas.net/en/)KeyboardInfo

| [IKeyboardInfo](./interfaceATAS_1_1Indicators_1_1IKeyboardInfo.md) ATAS.Indicators.IChart.KeyboardInfo |
| --- |

get

Gets the information about the keyboard within the chart.

## [◆](https://docs.atas.net/en/)MouseLocationInfo

| [IMouseLocationInfo](./interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md) ATAS.Indicators.IChart.MouseLocationInfo |
| --- |

get

Gets the information about the mouse location within the chart.

## [◆](https://docs.atas.net/en/)PriceAxisFont

| RenderFont ATAS.Indicators.IChart.PriceAxisFont |
| --- |

get

Gets the font of the chart price axis.

## [◆](https://docs.atas.net/en/)PriceChartContainer

| [IChartContainer](./interfaceATAS_1_1Indicators_1_1IChartContainer.md) ATAS.Indicators.IChart.PriceChartContainer |
| --- |

get

Gets the container representing the area of the chart without the axis.

## [◆](https://docs.atas.net/en/)StatusLineHorizontalOffset

| int ATAS.Indicators.IChart.StatusLineHorizontalOffset |
| --- |

getset

Gets or sets status line horizontal offset.

## [◆](https://docs.atas.net/en/)StringFormat

| string ATAS.Indicators.IChart.StringFormat |
| --- |

get

Gets the string format used within the chart.

## [◆](https://docs.atas.net/en/)TimeFrame

| string ATAS.Indicators.IChart.TimeFrame |
| --- |

get

Gets the time frame of the chart. If a time-frame has several parameters, they are separated by a slash (/).

## [◆](https://docs.atas.net/en/)TradingSessionDescriptions

| IEnumerable ATAS.Indicators.IChart.TradingSessionDescriptions |
| --- |

get

Get the available trading sessions.

The documentation for this interface was generated from the following file:
- [IIndicatorContainer.cs](../files/IIndicatorContainer_8cs.md)
