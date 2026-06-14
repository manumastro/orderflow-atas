# ATAS.Indicators.CandlePartSeries Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1CandlePartSeries.html

Represents a data series of decimal values derived from specific parts of an IndicatorCandle created by an ICandleCreator.
 [More...](./classATAS_1_1Indicators_1_1CandlePartSeries.md#details)

Inheritance diagram for ATAS.Indicators.CandlePartSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.CandlePartSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [CandlePartSeries](./classATAS_1_1Indicators_1_1CandlePartSeries.md#a1633982fd758d80d53528bfb092b77da) ([ICandleCreator](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md) candleCreator, [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) type) |
| | Initializes a new instance of the CandlePartSeries class. |
| | |
| [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) | [GetCandle](./classATAS_1_1Indicators_1_1CandlePartSeries.md#aabf8d4ad3f3b7fd57216812e233895da) (int bar) |
| | Gets the IndicatorCandle at the specified bar index. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| virtual void | [Clear](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f) () |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9fcdd2e47bf195bc54caa66e56549c6a) () |
| | |

| Properties | |
| --- | --- |
| override int | [Count](./classATAS_1_1Indicators_1_1CandlePartSeries.md#a6deb30a97b0bbfafaaccbfba9fb08704)`[get]` |
| | |
| override decimal | [this[int index]](./classATAS_1_1Indicators_1_1CandlePartSeries.md#a09f38addb4a94589e0e4c18a6e96007c)`[get, set]` |
| | |
| - Properties inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| bool | [DrawAbovePrice](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ac9659740fca4f14dda688dc829b66888)`[get, set]` |
| | |
| bool | [IgnoredByAlerts](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9a36f03e022cb4add8fe97cfbb21169a)`[get, set]` |
| | |
| string | [Id](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a904b2ee0c338e8daedeb1be04e03bea6)`[get, set]` |
| | Gets or sets the unique and constant data series ID for data serialization. |
| | |
| string | [RenderId](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a4a35f99d3c4cfb840b4271a46394eb95)`[get]` |
| | Unique series id for all panels and indicators. |
| | |
| virtual bool | [IsVisible](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a198e6ffdd9f5a25b229889830a0869db)`[get]` |
| | Gets a value is should series drawn. |
| | |
| [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | [Type](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a04de8163b7f0959079937ad563e8bb69)`[get]` |
| | |
| string | [Name](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a3dc63323dd14639ac81060266eadfc21)`[get, set]` |
| | |
| string | [DescriptionKey](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a5d418dd3c036bc7ffbf39091096065ee)`[get, set]` |
| | |
| abstract int | [Count](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a4173d3b58d1b0d8d75b9c6b56e568646)`[get]` |
| | |
| abstract T | [this[int index]](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ae75ee39544433dcf2373316c07c8a7bf)`[get, set]` |
| | Gets or sets the element at the specified index. |
| | |
| bool | [IsHidden](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a8ba2d8c9df48d295738b27a2d0112c55)`[get, set]` |
| | |
| bool | [ShowTooltip](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ab600594039922d8dfe8de5c63975b5a8)`[get, set]` |
| | |
| bool | [UseMinimizedModeIfEnabled](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a2f4b1937267c5d22bb36ff667f7817fd)`[get, set]` |
| | |
| bool | [ResetAlertsOnNewBar](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a4ac4b12d4d500be04116d65049fa101e)`[get, set]` |
| | |
| bool | [ShowNameOnMouseOver](./classATAS_1_1Indicators_1_1BaseDataSeries.md#ab3d8b64ba45f6a5dcc16b52055ace625)`[get, set]` |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Protected Member Functions inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
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
| - Events inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| Action? | [Changed](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a75c341d08edf3193825203d584bd0798) |
| | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a3e8653aebae29834354b70f209b867a9) |
| | |
| PropertyChangedEventHandler? | [PanelPropertyChanged](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a2896da4e9a1cbde2af8438ca71bc667d) |
| | |

## Detailed Description

Represents a data series of decimal values derived from specific parts of an IndicatorCandle created by an ICandleCreator.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)CandlePartSeries()

| ATAS.Indicators.CandlePartSeries.CandlePartSeries | ( | [ICandleCreator](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md) | candleCreator, |
| --- | --- | --- | --- |
| | | [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | type |
| | ) | | |

Initializes a new instance of the CandlePartSeries class.

Parameters

| candleCreator | The ICandleCreator used to create IndicatorCandles. |
| --- | --- |
| type | The type of data series based on the specific part of the IndicatorCandle. |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetCandle()

| [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) ATAS.Indicators.CandlePartSeries.GetCandle | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the IndicatorCandle at the specified bar index.

Parameters

| bar | The index of the bar to get the IndicatorCandle for. |
| --- | --- |

ReturnsThe IndicatorCandle at the specified bar index.

## Property Documentation

## [◆](https://docs.atas.net/en/)Count

| override int ATAS.Indicators.CandlePartSeries.Count |
| --- |

get

## [◆](https://docs.atas.net/en/)this[int index]

| override decimal ATAS.Indicators.CandlePartSeries.this[int index] |
| --- |

getset

The documentation for this class was generated from the following file:
- [CandlePartSeries.cs](../files/CandlePartSeries_8cs.md)
