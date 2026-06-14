# ATAS.Indicators.IDataSeries< T > Interface Template Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IDataSeries.html

Interface for data series, providing essential properties and methods.
 [More...](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#details)

Inheritance diagram for ATAS.Indicators.IDataSeries< T >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IDataSeries< T >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Clear](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#aed29e8f770f49c4bfbc821cfd9a2f089) () |
| | Clears all elements from the data series. |
| | |

| Properties | |
| --- | --- |
| string | [Id](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#aaa7a104ff978739009a04df4aad48629)`[get]` |
| | Gets the unique and constant data series ID for data serialization. |
| | |
| string | [RenderId](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a04222460e8e877393a975e84171a969f)`[get]` |
| | Unique series id for all panels and indicators. |
| | |
| [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | [Type](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#ab1a86efa296db5f1824a60727aa0c96b)`[get]` |
| | Gets the type of the data series from the enumeration. |
| | |
| string | [Name](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#af88d92b8f086f25702435468cc7ca122)`[get, set]` |
| | Gets or sets the name of the data series. |
| | |
| string | [DescriptionKey](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#aa042364274e500ec8260a7688c2657cb)`[get, set]` |
| | Get or sets the description of the data series. |
| | |
| int | [Count](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a92df2ae597a2ebafcf47c2372f2e95e2)`[get]` |
| | Gets the number of elements in the data series. |
| | |
| bool | [IsHidden](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a57177552e6d9838aa3cb2b4be4bbfccb)`[get, set]` |
| | Gets or sets a value indicating whether the data series properties should be hidden from the settings window. |
| | |
| bool | [IsVisible](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#ac31f8aaecb5f94fa787f32bede8a4c02)`[get]` |
| | Gets a value is should series drawn. |
| | |
| bool | [DrawAbovePrice](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#af2c3f111ffb83eb304cc5df9b691d0e5)`[get, set]` |
| | Gets or sets whether the data series should be drawn above candles of chart. |
| | |
| bool | [UseMinimizedModeIfEnabled](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#ae89eea1c3d1268d417eab9a27617a833)`[get, set]` |
| | Gets or sets a value indicating whether the minimized mode should be used if enabled. |
| | |
| bool | [IgnoredByAlerts](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a40967ae1aeede080dfe082ed6593132f)`[get, set]` |
| | Gets or sets a value indicating whether the data series should be ignored by alerts. |
| | |
| bool | [ResetAlertsOnNewBar](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a3ae4c1385523c7966c6c61791eb3943b)`[get, set]` |
| | Gets or sets a value indicating whether alerts should be reset on a new bar. |
| | |
| bool | [ShowTooltip](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#aa6d7cb4969c2df30701cb5f32ee6dbfc)`[get, set]` |
| | Gets or sets a value indicating whether the data series tooltip should be shown. |
| | |
| bool | [ShowNameOnMouseOver](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a44eb05b9b85c45398d147be74c1c0fd6)`[get, set]` |
| | Gets or sets a value indicating whether the name of the data series should be shown on mouseover. |
| | |
| object | [this[int index]](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a5198d6de182eb2c7a15c878c6156b06f)`[get, set]` |
| | |
| new T | [this[int index]](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a40f971fec0fc265257cc8c989822c60c)`[get, set]` |
| | Gets or sets the element at the specified index. |
| | |

| Events | |
| --- | --- |
| Action | [Changed](./interfaceATAS_1_1Indicators_1_1IDataSeries.md#a501e09f176e8cc495e27914a9d7b5b6a) |
| | Event raised when the data series is changed at a specific bar. |
| | |
| - Events inherited from [ATAS.Indicators.INotifyPanelPropertyChanged](./interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md) | |
| PropertyChangedEventHandler | [PanelPropertyChanged](./interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md#a1eb7377432737676cf47033c1fe6a965) |
| | Occurs when a panel property value changes. |
| | |

## Detailed Description

Interface for data series, providing essential properties and methods.

Interface for typed data series, providing essential properties and methods.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Clears all elements from the data series.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f).

## Property Documentation

## [◆](https://docs.atas.net/en/)Count

| int [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).Count |
| --- |

get

Gets the number of elements in the data series.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a4173d3b58d1b0d8d75b9c6b56e568646).

## [◆](https://docs.atas.net/en/)DescriptionKey

| string [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).DescriptionKey |
| --- |

getset

Get or sets the description of the data series.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a5d418dd3c036bc7ffbf39091096065ee).

## [◆](https://docs.atas.net/en/)DrawAbovePrice

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).DrawAbovePrice |
| --- |

getset

Gets or sets whether the data series should be drawn above candles of chart.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#ac9659740fca4f14dda688dc829b66888).

## [◆](https://docs.atas.net/en/)Id

| string [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).Id |
| --- |

get

Gets the unique and constant data series ID for data serialization.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a904b2ee0c338e8daedeb1be04e03bea6).

## [◆](https://docs.atas.net/en/)IgnoredByAlerts

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).IgnoredByAlerts |
| --- |

getset

Gets or sets a value indicating whether the data series should be ignored by alerts.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a9a36f03e022cb4add8fe97cfbb21169a).

## [◆](https://docs.atas.net/en/)IsHidden

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).IsHidden |
| --- |

getset

Gets or sets a value indicating whether the data series properties should be hidden from the settings window.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a8ba2d8c9df48d295738b27a2d0112c55).

## [◆](https://docs.atas.net/en/)IsVisible

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).IsVisible |
| --- |

get

Gets a value is should series drawn.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a198e6ffdd9f5a25b229889830a0869db).

## [◆](https://docs.atas.net/en/)Name

| string [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).Name |
| --- |

getset

Gets or sets the name of the data series.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a3dc63323dd14639ac81060266eadfc21).

## [◆](https://docs.atas.net/en/)RenderId

| string [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).RenderId |
| --- |

get

Unique series id for all panels and indicators.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a4a35f99d3c4cfb840b4271a46394eb95).

## [◆](https://docs.atas.net/en/)ResetAlertsOnNewBar

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).ResetAlertsOnNewBar |
| --- |

getset

Gets or sets a value indicating whether alerts should be reset on a new bar.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a4ac4b12d4d500be04116d65049fa101e).

## [◆](https://docs.atas.net/en/)ShowNameOnMouseOver

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).ShowNameOnMouseOver |
| --- |

getset

Gets or sets a value indicating whether the name of the data series should be shown on mouseover.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#ab3d8b64ba45f6a5dcc16b52055ace625).

## [◆](https://docs.atas.net/en/)ShowTooltip

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).ShowTooltip |
| --- |

getset

Gets or sets a value indicating whether the data series tooltip should be shown.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#ab600594039922d8dfe8de5c63975b5a8).

## [◆](https://docs.atas.net/en/)this[int index] [1/2]

| object [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).this[int index] |
| --- |

getset

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#ae75ee39544433dcf2373316c07c8a7bf).

## [◆](https://docs.atas.net/en/)this[int index] [2/2]

| new T [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).this[int index] |
| --- |

getset

Gets or sets the element at the specified index.

Parameters

| index | must be a non-negative integer. |
| --- | --- |

ReturnsAn element of type T at the specified index.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#ae75ee39544433dcf2373316c07c8a7bf).

## [◆](https://docs.atas.net/en/)Type

| [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).Type |
| --- |

get

Gets the type of the data series from the enumeration.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a04de8163b7f0959079937ad563e8bb69).

## [◆](https://docs.atas.net/en/)UseMinimizedModeIfEnabled

| bool [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).UseMinimizedModeIfEnabled |
| --- |

getset

Gets or sets a value indicating whether the minimized mode should be used if enabled.

Implemented in [ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#a2f4b1937267c5d22bb36ff667f7817fd).

## Event Documentation

## [◆](https://docs.atas.net/en/)Changed

| Action [ATAS.Indicators.IDataSeries](./interfaceATAS_1_1Indicators_1_1IDataSeries.md).Changed |
| --- |

Event raised when the data series is changed at a specific bar.

The documentation for this interface was generated from the following file:
- [BaseDataSeries.cs](../files/BaseDataSeries_8cs.md)
