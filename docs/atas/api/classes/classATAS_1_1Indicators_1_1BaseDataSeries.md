# ATAS.Indicators.BaseDataSeries< T > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1BaseDataSeries.html

Base generic data series class providing common functionality.
 [More...](./classATAS_1_1Indicators_1_1BaseDataSeries.md#details)

Inheritance diagram for ATAS.Indicators.BaseDataSeries< T >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.BaseDataSeries< T >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| virtual void | [Clear](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f) () |
| | Clears all elements from the data series. |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9fcdd2e47bf195bc54caa66e56549c6a) () |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aed29e8f770f49c4bfbc821cfd9a2f089) () |
| | Clears all elements from the data series. |
| | |

| Protected Member Functions | |
| --- | --- |
| void | [RaiseChanged](./classATAS_1_1Indicators_1_1BaseDataSeries.md#aae4131150ded65d1ed766facbff35bfe) (int bar) |
| | |
| virtual void | [RaisePropertyChanged](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a8ac5c03b2fdc4ce94cebd52083d4f7dd) (string propertyName) |
| | |
| virtual void | [RaisePanelPropertyChanged](./classATAS_1_1Indicators_1_1BaseDataSeries.md#aeed7ce65755e918ea115a1646c159e59) (string propertyName) |
| | |
| | [BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md#aab54dc7aa127e7e4695c31bf964418d9) (string id, [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) type) |
| | |
| | [BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md#adb1761009760d73bfc544e31f5506005) (string id, string name, [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) type) |
| | |
| | [BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md#af1dc0499bd0e08bc40824980ce9e65c6) ([DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) type) |
| | |

| Properties | |
| --- | --- |
| bool | [DrawAbovePrice](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ac9659740fca4f14dda688dc829b66888)`[get, set]` |
| | Gets or sets whether the data series should be drawn above candles of chart. |
| | |
| bool | [IgnoredByAlerts](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9a36f03e022cb4add8fe97cfbb21169a)`[get, set]` |
| | Gets or sets a value indicating whether the data series should be ignored by alerts. |
| | |
| string | [Id](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a904b2ee0c338e8daedeb1be04e03bea6) = string.Empty`[get, set]` |
| | Gets or sets the unique and constant data series ID for data serialization. |
| | |
| string | [RenderId](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a4a35f99d3c4cfb840b4271a46394eb95) = string.Empty`[get]` |
| | Unique series id for all panels and indicators. |
| | |
| virtual bool | [IsVisible](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a198e6ffdd9f5a25b229889830a0869db)`[get]` |
| | Gets a value is should series drawn. |
| | |
| [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | [Type](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a04de8163b7f0959079937ad563e8bb69)`[get]` |
| | Gets the type of the data series from the enumeration. |
| | |
| string | [Name](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a3dc63323dd14639ac81060266eadfc21)`[get, set]` |
| | Gets or sets the name of the data series. |
| | |
| string | [DescriptionKey](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a5d418dd3c036bc7ffbf39091096065ee)`[get, set]` |
| | Get or sets the description of the data series. |
| | |
| abstract int | [Count](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a4173d3b58d1b0d8d75b9c6b56e568646)`[get]` |
| | Gets the number of elements in the data series. |
| | |
| abstract T | [this[int index]](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ae75ee39544433dcf2373316c07c8a7bf)`[get, set]` |
| | Gets or sets the element at the specified index. |
| | |
| bool | [IsHidden](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a8ba2d8c9df48d295738b27a2d0112c55)`[get, set]` |
| | Gets or sets a value indicating whether the data series properties should be hidden from the settings window. |
| | |
| bool | [ShowTooltip](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ab600594039922d8dfe8de5c63975b5a8) = true`[get, set]` |
| | Gets or sets a value indicating whether the data series tooltip should be shown. |
| | |
| bool | [UseMinimizedModeIfEnabled](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a2f4b1937267c5d22bb36ff667f7817fd)`[get, set]` |
| | Gets or sets a value indicating whether the minimized mode should be used if enabled. |
| | |
| bool | [ResetAlertsOnNewBar](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a4ac4b12d4d500be04116d65049fa101e)`[get, set]` |
| | Gets or sets a value indicating whether alerts should be reset on a new bar. |
| | |
| bool | [ShowNameOnMouseOver](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ab3d8b64ba45f6a5dcc16b52055ace625) = true`[get, set]` |
| | Gets or sets a value indicating whether the name of the data series should be shown on mouseover. |
| | |
| - Properties inherited from [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | |
| string | [Id](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aaa7a104ff978739009a04df4aad48629)`[get]` |
| | Gets the unique and constant data series ID for data serialization. |
| | |
| string | [RenderId](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a04222460e8e877393a975e84171a969f)`[get]` |
| | Unique series id for all panels and indicators. |
| | |
| [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | [Type](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#ab1a86efa296db5f1824a60727aa0c96b)`[get]` |
| | Gets the type of the data series from the enumeration. |
| | |
| string | [Name](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#af88d92b8f086f25702435468cc7ca122)`[get, set]` |
| | Gets or sets the name of the data series. |
| | |
| string | [DescriptionKey](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aa042364274e500ec8260a7688c2657cb)`[get, set]` |
| | Get or sets the description of the data series. |
| | |
| int | [Count](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a92df2ae597a2ebafcf47c2372f2e95e2)`[get]` |
| | Gets the number of elements in the data series. |
| | |
| bool | [IsHidden](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a57177552e6d9838aa3cb2b4be4bbfccb)`[get, set]` |
| | Gets or sets a value indicating whether the data series properties should be hidden from the settings window. |
| | |
| bool | [IsVisible](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#ac31f8aaecb5f94fa787f32bede8a4c02)`[get]` |
| | Gets a value is should series drawn. |
| | |
| bool | [DrawAbovePrice](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#af2c3f111ffb83eb304cc5df9b691d0e5)`[get, set]` |
| | Gets or sets whether the data series should be drawn above candles of chart. |
| | |
| bool | [UseMinimizedModeIfEnabled](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#ae89eea1c3d1268d417eab9a27617a833)`[get, set]` |
| | Gets or sets a value indicating whether the minimized mode should be used if enabled. |
| | |
| bool | [IgnoredByAlerts](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a40967ae1aeede080dfe082ed6593132f)`[get, set]` |
| | Gets or sets a value indicating whether the data series should be ignored by alerts. |
| | |
| bool | [ResetAlertsOnNewBar](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a3ae4c1385523c7966c6c61791eb3943b)`[get, set]` |
| | Gets or sets a value indicating whether alerts should be reset on a new bar. |
| | |
| bool | [ShowTooltip](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aa6d7cb4969c2df30701cb5f32ee6dbfc)`[get, set]` |
| | Gets or sets a value indicating whether the data series tooltip should be shown. |
| | |
| bool | [ShowNameOnMouseOver](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a44eb05b9b85c45398d147be74c1c0fd6)`[get, set]` |
| | Gets or sets a value indicating whether the name of the data series should be shown on mouseover. |
| | |
| object | [this[int index]](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a5198d6de182eb2c7a15c878c6156b06f)`[get, set]` |
| | |
| new T | [this[int index]](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a40f971fec0fc265257cc8c989822c60c)`[get, set]` |
| | Gets or sets the element at the specified index. |
| | |

| Events | |
| --- | --- |
| Action? | [Changed](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a75c341d08edf3193825203d584bd0798) |
| | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a3e8653aebae29834354b70f209b867a9) |
| | |
| PropertyChangedEventHandler? | [PanelPropertyChanged](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a2896da4e9a1cbde2af8438ca71bc667d) |
| | |
| - Events inherited from [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | |
| Action | [Changed](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a501e09f176e8cc495e27914a9d7b5b6a) |
| | Event raised when the data series is changed at a specific bar. |
| | |
| - Events inherited from [ATAS.Indicators.INotifyPanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md) | |
| PropertyChangedEventHandler | [PanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md#a1eb7377432737676cf47033c1fe6a965) |
| | Occurs when a panel property value changes. |
| | |

## Detailed Description

Base generic data series class providing common functionality.

Template Parameters

| T | Type of the data series. |
| --- | --- |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)BaseDataSeries() [1/3]

| [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).[BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | ( | string | id, |
| --- | --- | --- | --- |
| | | [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | type |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)BaseDataSeries() [2/3]

| [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).[BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | ( | string | id, |
| --- | --- | --- | --- |
| | | string | name, |
| | | [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | type |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)BaseDataSeries() [3/3]

| [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).[BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | ( | [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | type | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| virtual void [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Clears all elements from the data series.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aed29e8f770f49c4bfbc821cfd9a2f089).

Reimplemented in [ATAS.Indicators.CandleDataSeries](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a9687bc22eaa951c5dc52fa8425e722df), [ATAS.Indicators.CustomValueDataSeries](./classATAS_1_1Indicators_1_1CustomValueDataSeries.md#a5251bcb327d4ba253cf2ceab071ba477), [ATAS.Indicators.ObjectDataSeries](./classATAS_1_1Indicators_1_1ObjectDataSeries.md#a41812b12e4ef9f42dbe76c332fee2550), [ATAS.Indicators.PaintbarsDataSeries](./classATAS_1_1Indicators_1_1PaintbarsDataSeries.md#a96d1a910b5e48d41af04bae8c15d5e04), [ATAS.Indicators.PriceSelectionDataSeries](./classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md#ac4fc208dd0ce5fc09fc7d2d91771c250), [ATAS.Indicators.RangeDataSeries](./classATAS_1_1Indicators_1_1RangeDataSeries.md#af5df3bbcb7c983f94472706e2540c52f), and [ATAS.Indicators.ValueDataSeries](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a7d7d20a17b3b2f7f1b56a9c7c5c87fdd).

## [◆](https://docs.atas.net/en/)RaiseChanged()

| void [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).RaiseChanged | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)RaisePanelPropertyChanged()

| virtual void [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).RaisePanelPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)RaisePropertyChanged()

| virtual void [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).RaisePropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToString()

| override string [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Count

| abstract int [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).Count |
| --- |

get

Gets the number of elements in the data series.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a92df2ae597a2ebafcf47c2372f2e95e2).

## [◆](https://docs.atas.net/en/)DescriptionKey

| string [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).DescriptionKey |
| --- |

getset

Get or sets the description of the data series.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aa042364274e500ec8260a7688c2657cb).

## [◆](https://docs.atas.net/en/)DrawAbovePrice

| bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).DrawAbovePrice |
| --- |

getset

Gets or sets whether the data series should be drawn above candles of chart.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#af2c3f111ffb83eb304cc5df9b691d0e5).

## [◆](https://docs.atas.net/en/)Id

| string [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).Id = string.Empty |
| --- |

getset

Gets or sets the unique and constant data series ID for data serialization.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aaa7a104ff978739009a04df4aad48629).

## [◆](https://docs.atas.net/en/)IgnoredByAlerts

| bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).IgnoredByAlerts |
| --- |

getset

Gets or sets a value indicating whether the data series should be ignored by alerts.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a40967ae1aeede080dfe082ed6593132f).

## [◆](https://docs.atas.net/en/)IsHidden

| bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).IsHidden |
| --- |

getset

Gets or sets a value indicating whether the data series properties should be hidden from the settings window.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a57177552e6d9838aa3cb2b4be4bbfccb).

## [◆](https://docs.atas.net/en/)IsVisible

| virtual bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).IsVisible |
| --- |

get

Gets a value is should series drawn.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#ac31f8aaecb5f94fa787f32bede8a4c02).

## [◆](https://docs.atas.net/en/)Name

| string [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).Name |
| --- |

getset

Gets or sets the name of the data series.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#af88d92b8f086f25702435468cc7ca122).

## [◆](https://docs.atas.net/en/)RenderId

| string [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).RenderId = string.Empty |
| --- |

get

Unique series id for all panels and indicators.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a04222460e8e877393a975e84171a969f).

## [◆](https://docs.atas.net/en/)ResetAlertsOnNewBar

| bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).ResetAlertsOnNewBar |
| --- |

getset

Gets or sets a value indicating whether alerts should be reset on a new bar.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a3ae4c1385523c7966c6c61791eb3943b).

## [◆](https://docs.atas.net/en/)ShowNameOnMouseOver

| bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).ShowNameOnMouseOver = true |
| --- |

getset

Gets or sets a value indicating whether the name of the data series should be shown on mouseover.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a44eb05b9b85c45398d147be74c1c0fd6).

## [◆](https://docs.atas.net/en/)ShowTooltip

| bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).ShowTooltip = true |
| --- |

getset

Gets or sets a value indicating whether the data series tooltip should be shown.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#aa6d7cb4969c2df30701cb5f32ee6dbfc).

## [◆](https://docs.atas.net/en/)this[int index]

| abstract T [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).this[int index] |
| --- |

getset

Gets or sets the element at the specified index.

Parameters

| index | must be a non-negative integer. |
| --- | --- |

ReturnsAn element of type T at the specified index.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#a5198d6de182eb2c7a15c878c6156b06f).

## [◆](https://docs.atas.net/en/)Type

| [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).Type |
| --- |

get

Gets the type of the data series from the enumeration.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#ab1a86efa296db5f1824a60727aa0c96b).

## [◆](https://docs.atas.net/en/)UseMinimizedModeIfEnabled

| bool [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).UseMinimizedModeIfEnabled |
| --- |

getset

Gets or sets a value indicating whether the minimized mode should be used if enabled.

Implements [ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#ae89eea1c3d1268d417eab9a27617a833).

## Event Documentation

## [◆](https://docs.atas.net/en/)Changed

| Action? [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).Changed |
| --- |

## [◆](https://docs.atas.net/en/)PanelPropertyChanged

| PropertyChangedEventHandler? [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).PanelPropertyChanged |
| --- |

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler? [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md).PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [BaseDataSeries.cs](../files/BaseDataSeries_8cs.md)
