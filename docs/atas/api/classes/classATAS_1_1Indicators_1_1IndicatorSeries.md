# ATAS.Indicators.IndicatorSeries Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1IndicatorSeries.html

Represents a custom data series for an indicator, derived from BaseDataSeries<decimal>.
 [More...](./classATAS_1_1Indicators_1_1IndicatorSeries.md#details)

Inheritance diagram for ATAS.Indicators.IndicatorSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IndicatorSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [IndicatorSeries](./classATAS_1_1Indicators_1_1IndicatorSeries.md#a6d8552fc8e869a50adc76b4cb559dfa0) ([Indicator](./classATAS_1_1Indicators_1_1Indicator.md) indicator, int seriesId) |
| | Constructor to create an IndicatorSeries object for the specified indicator and series ID. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| virtual void | [Clear](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f) () |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9fcdd2e47bf195bc54caa66e56549c6a) () |
| | |

| Properties | |
| --- | --- |
| [Indicator](./classATAS_1_1Indicators_1_1Indicator.md) | [Indicator](./classATAS_1_1Indicators_1_1IndicatorSeries.md#a2cca012679cb24e6a33b5866c8c9f53f)`[get]` |
| | Gets the associated indicator. |
| | |
| int | [SeriesId](./classATAS_1_1Indicators_1_1IndicatorSeries.md#afbcb87c361308edbb216eb9b3581089d)`[get]` |
| | Gets the ID of the series within the indicator. |
| | |
| override int | [Count](./classATAS_1_1Indicators_1_1IndicatorSeries.md#ac1385e4f6bfd317796a3c605222dcaf7)`[get]` |
| | Gets the count of data points in the series. |
| | |
| override decimal | [this[int index]](./classATAS_1_1Indicators_1_1IndicatorSeries.md#a0109483ea284f4885f80e7dc222d2e4c)`[get, set]` |
| | Gets the data value at the specified index in the IndicatorSeries. |
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

Represents a custom data series for an indicator, derived from BaseDataSeries<decimal>.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)IndicatorSeries()

| ATAS.Indicators.IndicatorSeries.IndicatorSeries | ( | [Indicator](./classATAS_1_1Indicators_1_1Indicator.md) | indicator, |
| --- | --- | --- | --- |
| | | int | seriesId |
| | ) | | |

Constructor to create an IndicatorSeries object for the specified indicator and series ID.

Parameters

| indicator | The associated indicator. |
| --- | --- |
| seriesId | The ID of the series within the indicator. |

Exceptions

| ArgumentNullException | Thrown if the 'indicator' parameter is null. An IndicatorSeries must be associated with a valid indicator. |
| --- | --- |
| IndexOutOfRangeException | Thrown if the specified 'seriesId' is not a valid index in the parent indicator's DataSeries collection. |

## Property Documentation

## [◆](https://docs.atas.net/en/)Count

| override int ATAS.Indicators.IndicatorSeries.Count |
| --- |

get

Gets the count of data points in the series.

## [◆](https://docs.atas.net/en/)Indicator

| [Indicator](./classATAS_1_1Indicators_1_1Indicator.md) ATAS.Indicators.IndicatorSeries.Indicator |
| --- |

get

Gets the associated indicator.

## [◆](https://docs.atas.net/en/)SeriesId

| int ATAS.Indicators.IndicatorSeries.SeriesId |
| --- |

get

Gets the ID of the series within the indicator.

## [◆](https://docs.atas.net/en/)this[int index]

| override decimal ATAS.Indicators.IndicatorSeries.this[int index] |
| --- |

getset

Gets the data value at the specified index in the IndicatorSeries.

Parameters

| index | The index of the data value to retrieve. |
| --- | --- |

ReturnsThe data value at the specified index.
IndicatorSeries does not support modifying data values. It is read-only.

Exceptions

| IndexOutOfRangeException | Thrown if the specified index is outside the valid range of the data series. |
| --- | --- |

The documentation for this class was generated from the following file:
- [IndicatorSeries.cs](../files/IndicatorSeries_8cs.md)
