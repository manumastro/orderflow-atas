# ATAS.Indicators.LineSeries Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1LineSeries.html

Represents a horizontal line with a single value.
 [More...](./classATAS_1_1Indicators_1_1LineSeries.md#details)

Inheritance diagram for ATAS.Indicators.LineSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.LineSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [LineSeries](./classATAS_1_1Indicators_1_1LineSeries.md#afc1d30b5a8a548bf92c4e7500eed466f) (string id, string name) |
| | Initializes a new instance of the LineSeries class with the specified unique and constant data series ID for data serialization and unique name. |
| | |
| | [LineSeries](./classATAS_1_1Indicators_1_1LineSeries.md#a56668b503630e68fbcd53529488f2deb) (string id) |
| | Initializes a new instance of the LineSeries class with the specified unique and constant data series ID for data serialization. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| virtual void | [Clear](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f) () |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9fcdd2e47bf195bc54caa66e56549c6a) () |
| | |

| Properties | |
| --- | --- |
| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | [Color](./classATAS_1_1Indicators_1_1LineSeries.md#a193c1be66f29aca8b1c672e6fd34af77)`[get, set]` |
| | Color of the line. |
| | |
| LineDashStyle | [LineDashStyle](./classATAS_1_1Indicators_1_1LineSeries.md#a4131ee1e4438aa65711f07b1692c241d)`[get, set]` |
| | Style of the line. |
| | |
| int | [Width](./classATAS_1_1Indicators_1_1LineSeries.md#adb685e2a695a204645d0da95fe76d2b2)`[get, set]` |
| | Width of the line. |
| | |
| bool | [UseScale](./classATAS_1_1Indicators_1_1LineSeries.md#ac683e972280e23d9d4b5aeb720b1735a)`[get, set]` |
| | Indicates whether to use scale. |
| | |
| decimal | [Value](./classATAS_1_1Indicators_1_1LineSeries.md#aec476f2062791e866bcb27fdfd5edd8b)`[get, set]` |
| | Value of the line. |
| | |
| string | [Text](./classATAS_1_1Indicators_1_1LineSeries.md#a28ab772416acc7a83d14cf369a77c8fa)`[get, set]` |
| | Text associated with the line. |
| | |
| override int | [Count](./classATAS_1_1Indicators_1_1LineSeries.md#a5639aa9d3a4e1a7b049d819f5ec8d2dc)`[get]` |
| | Gets the number of elements in the series (always int.MaxValue). |
| | |
| override decimal | [this[int index]](./classATAS_1_1Indicators_1_1LineSeries.md#aa217d2512c0a1251fa9337d1114daacd)`[get, set]` |
| | Gets or sets the value at the specified index (always returns the current Value and throws NotSupportedException for setting). |
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

Represents a horizontal line with a single value.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)LineSeries() [1/2]

| ATAS.Indicators.LineSeries.LineSeries | ( | string | id, |
| --- | --- | --- | --- |
| | | string | name |
| | ) | | |

Initializes a new instance of the LineSeries class with the specified unique and constant data series ID for data serialization and unique name.

Parameters

| id | The unique and constant data series ID for data serialization. |
| --- | --- |
| name | The unique data series name. |

## [◆](https://docs.atas.net/en/)LineSeries() [2/2]

| ATAS.Indicators.LineSeries.LineSeries | ( | string | id | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the LineSeries class with the specified unique and constant data series ID for data serialization.

Parameters

| id | The unique and constant data series ID for data serialization. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Color

| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) ATAS.Indicators.LineSeries.Color |
| --- |

getset

Color of the line.

## [◆](https://docs.atas.net/en/)Count

| override int ATAS.Indicators.LineSeries.Count |
| --- |

get

Gets the number of elements in the series (always int.MaxValue).

## [◆](https://docs.atas.net/en/)LineDashStyle

| LineDashStyle ATAS.Indicators.LineSeries.LineDashStyle |
| --- |

getset

Style of the line.

## [◆](https://docs.atas.net/en/)Text

| string ATAS.Indicators.LineSeries.Text |
| --- |

getset

Text associated with the line.

## [◆](https://docs.atas.net/en/)this[int index]

| override decimal ATAS.Indicators.LineSeries.this[int index] |
| --- |

getset

Gets or sets the value at the specified index (always returns the current Value and throws NotSupportedException for setting).

## [◆](https://docs.atas.net/en/)UseScale

| bool ATAS.Indicators.LineSeries.UseScale |
| --- |

getset

Indicates whether to use scale.

## [◆](https://docs.atas.net/en/)Value

| decimal ATAS.Indicators.LineSeries.Value |
| --- |

getset

Value of the line.

## [◆](https://docs.atas.net/en/)Width

| int ATAS.Indicators.LineSeries.Width |
| --- |

getset

Width of the line.

The documentation for this class was generated from the following file:
- [LineSeries.cs](../files/LineSeries_8cs.md)
