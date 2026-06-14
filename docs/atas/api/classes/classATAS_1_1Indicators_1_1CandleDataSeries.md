# ATAS.Indicators.CandleDataSeries Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1CandleDataSeries.html

Represents a data series of candles. Each element in the series is a Candle.
 [More...](./classATAS_1_1Indicators_1_1CandleDataSeries.md#details)

Inheritance diagram for ATAS.Indicators.CandleDataSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.CandleDataSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [CandleDataSeries](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a74e275232fd05355c05e724a46f64a90) (string id, string name) |
| | Initializes a new instance of the CandleDataSeries class with the specified unique and constant data series ID for data serialization and unique name. |
| | |
| | [CandleDataSeries](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a825d61ef32e5d5e2e2df5f40cf953ba8) (string id) |
| | Initializes a new instance of the CandleDataSeries class with the specified unique and constant data series ID for data serialization. |
| | |
| override void | [Clear](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a9687bc22eaa951c5dc52fa8425e722df) () |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| virtual void | [Clear](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f) () |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9fcdd2e47bf195bc54caa66e56549c6a) () |
| | |

| Properties | |
| --- | --- |
| int | [Digits](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a8ac9270fc6ee00e5a86acf946ebdd069)`[get, set]` |
| | Gets or sets the number of digits after the decimal point. |
| | |
| string | [StringFormat](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a9451ddd3e7d574d0320decbeb0402a1f)`[get, set]` |
| | Gets or sets the price string format used for displaying values. |
| | |
| bool | [HideZeroCandles](./classATAS_1_1Indicators_1_1CandleDataSeries.md#aab06b7ed313bc35d587a1212c5839cb3)`[get, set]` |
| | Gets or sets whether to show current values on the price panel. |
| | |
| bool | [HideOpenCloseLabels](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a2cc31ca899c51d70eeddbd0032e92374)`[get, set]` |
| | Gets or sets which point of the candle is used as the tooltip anchor. |
| | |
| [CandleTooltipAnchor](../namespaces/namespaceATAS_1_1Indicators.md#aa512d3c6bc2e3a31829de55b275883f0) | [TooltipAnchor](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a0588af81650eb733005abd76dfad6eef)`[get, set]` |
| | |
| bool | [ShowCurrentValue](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a59ee22cb04cf5b6e9cfbfcca2606f8f6)`[get, set]` |
| | |
| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | [UpCandleColor](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a7bad8acd70160cf6f59b2c8b5d88519c)`[get, set]` |
| | Gets or sets the color of the data series element on a bullish (up) candle. |
| | |
| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | [DownCandleColor](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a040c44d0db53012bdde51d77f6823cf4)`[get, set]` |
| | Gets or sets the color of the data series element on a bearish (down) candle. |
| | |
| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | [BorderColor](./classATAS_1_1Indicators_1_1CandleDataSeries.md#aae1baf0bed67427b02463c9adf6b255e)`[get, set]` |
| | Gets or sets the color of the data series element border. |
| | |
| System.Drawing.Color | [ValuesColor](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a60a169f5221a0113cf96511601457361)`[get, set]` |
| | Gets or sets the color of the values text. |
| | |
| bool | [Visible](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a53ddf4fa4cd3f9441066d075587dfad7)`[get, set]` |
| | Gets or sets whether the data series is visible on the chart. |
| | |
| bool | [ScaleIt](./classATAS_1_1Indicators_1_1CandleDataSeries.md#ac45c3d49ead3f3576cfdea1a27855d00)`[get, set]` |
| | Gets or sets whether to scale the data series on the chart. |
| | |
| [CandleVisualMode](../namespaces/namespaceATAS_1_1Indicators.md#a4a6c44527d3fdaa0633b7c838f2fba06) | [Mode](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a95b1036e7c21b0456258b55717e47d9a)`[get, set]` |
| | Gets or sets the visualization mode of the data series. |
| | |
| bool | [DrawCandleBorder](./classATAS_1_1Indicators_1_1CandleDataSeries.md#a3692082695de454151b224fc1c25c24b)`[get, set]` |
| | Gets or sets whether to draw candle border. |
| | |
| override bool | [IsVisible](./classATAS_1_1Indicators_1_1CandleDataSeries.md#ae52fbb3b30224067cf709b7920a54ba0)`[get]` |
| | |
| override int | [Count](./classATAS_1_1Indicators_1_1CandleDataSeries.md#ab891aca6dbb256a91816a34041c5d9ab)`[get]` |
| | |
| override [Candle](./classATAS_1_1Indicators_1_1Candle.md) | [this[int index]](./classATAS_1_1Indicators_1_1CandleDataSeries.md#af17117ac638360561319983120116957)`[get, set]` |
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

Represents a data series of candles. Each element in the series is a Candle.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)CandleDataSeries() [1/2]

| ATAS.Indicators.CandleDataSeries.CandleDataSeries | ( | string | id, |
| --- | --- | --- | --- |
| | | string | name |
| | ) | | |

Initializes a new instance of the CandleDataSeries class with the specified unique and constant data series ID for data serialization and unique name.

Parameters

| id | The unique and constant data series ID for data serialization. |
| --- | --- |
| name | The unique data series name. |

## [◆](https://docs.atas.net/en/)CandleDataSeries() [2/2]

| ATAS.Indicators.CandleDataSeries.CandleDataSeries | ( | string | id | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the CandleDataSeries class with the specified unique and constant data series ID for data serialization.

Parameters

| id | The unique and constant data series ID for data serialization. |
| --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| override void ATAS.Indicators.CandleDataSeries.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f).

## Property Documentation

## [◆](https://docs.atas.net/en/)BorderColor

| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) ATAS.Indicators.CandleDataSeries.BorderColor |
| --- |

getset

Gets or sets the color of the data series element border.

## [◆](https://docs.atas.net/en/)Count

| override int ATAS.Indicators.CandleDataSeries.Count |
| --- |

get

## [◆](https://docs.atas.net/en/)Digits

| int ATAS.Indicators.CandleDataSeries.Digits |
| --- |

getset

Gets or sets the number of digits after the decimal point.

## [◆](https://docs.atas.net/en/)DownCandleColor

| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) ATAS.Indicators.CandleDataSeries.DownCandleColor |
| --- |

getset

Gets or sets the color of the data series element on a bearish (down) candle.

## [◆](https://docs.atas.net/en/)DrawCandleBorder

| bool ATAS.Indicators.CandleDataSeries.DrawCandleBorder |
| --- |

getset

Gets or sets whether to draw candle border.

## [◆](https://docs.atas.net/en/)HideOpenCloseLabels

| bool ATAS.Indicators.CandleDataSeries.HideOpenCloseLabels |
| --- |

getset

Gets or sets which point of the candle is used as the tooltip anchor.

Gets or sets whether to hide "Open:"/"Close:" labels in tooltips, showing only the value.

## [◆](https://docs.atas.net/en/)HideZeroCandles

| bool ATAS.Indicators.CandleDataSeries.HideZeroCandles |
| --- |

getset

Gets or sets whether to show current values on the price panel.

Gets or sets whether to skip rendering and tooltips for candles where all OHLC values are zero.

## [◆](https://docs.atas.net/en/)IsVisible

| override bool ATAS.Indicators.CandleDataSeries.IsVisible |
| --- |

get

## [◆](https://docs.atas.net/en/)Mode

| [CandleVisualMode](../namespaces/namespaceATAS_1_1Indicators.md#a4a6c44527d3fdaa0633b7c838f2fba06) ATAS.Indicators.CandleDataSeries.Mode |
| --- |

getset

Gets or sets the visualization mode of the data series.

## [◆](https://docs.atas.net/en/)ScaleIt

| bool ATAS.Indicators.CandleDataSeries.ScaleIt |
| --- |

getset

Gets or sets whether to scale the data series on the chart.

## [◆](https://docs.atas.net/en/)ShowCurrentValue

| bool ATAS.Indicators.CandleDataSeries.ShowCurrentValue |
| --- |

getset

## [◆](https://docs.atas.net/en/)StringFormat

| string ATAS.Indicators.CandleDataSeries.StringFormat |
| --- |

getset

Gets or sets the price string format used for displaying values.

## [◆](https://docs.atas.net/en/)this[int index]

| override [Candle](./classATAS_1_1Indicators_1_1Candle.md) ATAS.Indicators.CandleDataSeries.this[int index] |
| --- |

getset

## [◆](https://docs.atas.net/en/)TooltipAnchor

| [CandleTooltipAnchor](../namespaces/namespaceATAS_1_1Indicators.md#aa512d3c6bc2e3a31829de55b275883f0) ATAS.Indicators.CandleDataSeries.TooltipAnchor |
| --- |

getset

## [◆](https://docs.atas.net/en/)UpCandleColor

| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) ATAS.Indicators.CandleDataSeries.UpCandleColor |
| --- |

getset

Gets or sets the color of the data series element on a bullish (up) candle.

## [◆](https://docs.atas.net/en/)ValuesColor

| System.Drawing.Color ATAS.Indicators.CandleDataSeries.ValuesColor |
| --- |

getset

Gets or sets the color of the values text.

## [◆](https://docs.atas.net/en/)Visible

| bool ATAS.Indicators.CandleDataSeries.Visible |
| --- |

getset

Gets or sets whether the data series is visible on the chart.

The documentation for this class was generated from the following file:
- [CandleDataSeries.cs](../files/CandleDataSeries_8cs.md)
