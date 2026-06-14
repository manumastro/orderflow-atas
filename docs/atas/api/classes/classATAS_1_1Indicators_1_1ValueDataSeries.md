# ATAS.Indicators.ValueDataSeries Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1ValueDataSeries.html

Represents a data series of decimal values, each element is a decimal.
 [More...](./classATAS_1_1Indicators_1_1ValueDataSeries.md#details)

Inheritance diagram for ATAS.Indicators.ValueDataSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.ValueDataSeries:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Classes | |
| --- | --- |
| class | [BarColors](./classATAS_1_1Indicators_1_1ValueDataSeries_1_1BarColors.md) |
| | Manages colors per bar for the associated ValueDataSeries. [More...](./classATAS_1_1Indicators_1_1ValueDataSeries_1_1BarColors.md#details) |
| | |

| Public Member Functions | |
| --- | --- |
| | [ValueDataSeries](./classATAS_1_1Indicators_1_1ValueDataSeries.md#ae98e178ecc4291792a28f1886c43c330) (string id, string name) |
| | Initializes a new instance of the ValueDataSeries class with the specified unique and constant data series ID for data serialization and unique name. |
| | |
| | [ValueDataSeries](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a28f8532cf77885da8d9e59df59ad19d5) (string id) |
| | Initializes a new instance of the ValueDataSeries class with the specified unique and constant data series ID for data serialization. |
| | |
| void | [SetPointOfEndLine](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a36e03449209e39e11ee5c42b5b863548) (int bar) |
| | Sets the specified bar index as a point of end for a line in the value data series. |
| | |
| void | [RemovePointOfEndLine](./classATAS_1_1Indicators_1_1ValueDataSeries.md#aafa4adfd4afe5ba2eb11cf2c7b166de1) (int bar) |
| | Removes the specified bar index as a point of end for a line in the value data series. |
| | |
| bool | [IsThisPointOfStartBar](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a338c720c7e369f069f8b2f38c7e8831e) (int bar) |
| | Checks if the specified bar index is a point of start for a line in the value data series. |
| | |
| override void | [Clear](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a7d7d20a17b3b2f7f1b56a9c7c5c87fdd) () |
| | |
| decimal | [LastNonZeroValue](./classATAS_1_1Indicators_1_1ValueDataSeries.md#aea89fb6a64e16c55223a069e7d2aedd5) (int bar) |
| | Gets the last non-zero value in the range from 0 to the specified bar. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| virtual void | [Clear](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f) () |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a9fcdd2e47bf195bc54caa66e56549c6a) () |
| | |

| Properties | |
| --- | --- |
| decimal | [ZeroValue](./classATAS_1_1Indicators_1_1ValueDataSeries.md#acf0e9ac28844b008ff3c44cf5027f26f)`[get, set]` |
| | Value of zero line for 'Histogram' mode. |
| | |
| [BarColors](./classATAS_1_1Indicators_1_1ValueDataSeries_1_1BarColors.md) | [Colors](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a0407916d780bc8fb31fe1870130a0588)`[get]` |
| | Allows to change color per bar. Color value will be used for all bars by default. |
| | |
| System.Drawing.Color | [RenderColor](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a4fb5cc9e976eaad06f260ae2d0c5c348)`[get, set]` |
| | |
| int | [Digits](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a8d1a7b389cf0d90f278d5b1c94b51424)`[get, set]` |
| | Gets or sets the number of digits after the decimal point for formatting the value data series. |
| | |
| string | [StringFormat](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a4b091acaa0a628fc8a563ac27bd1b4e7)`[get, set]` |
| | Gets or sets the price string format for formatting the value data series. |
| | |
| bool | [ShowOnlyNonZeroLabels](./classATAS_1_1Indicators_1_1ValueDataSeries.md#aef0027b4abc2f0b49c0aea74458c57a9)`[get, set]` |
| | Always draw last non-zero value on price axis. |
| | |
| [VisualMode](../namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb) | [VisualType](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a653b3a68b914e1a5a591f92d90e7aa3a)`[get, set]` |
| | Gets or sets the visual mode for drawing the value data series. |
| | |
| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | [Color](./classATAS_1_1Indicators_1_1ValueDataSeries.md#afc4f2cfe3d9905c0b17f1f9f8253b879)`[get, set]` |
| | Gets or sets the color for drawing the value data series. |
| | |
| System.Drawing.Color | [ValuesColor](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a8d5cb00d6dc8dcce1f8ac32e236cf37e)`[get, set]` |
| | Gets or sets the values text color for the value data series. |
| | |
| int | [Width](./classATAS_1_1Indicators_1_1ValueDataSeries.md#af7b58a3626c12088b203aa96a2d58449)`[get, set]` |
| | Gets or sets the width for drawing the value data series. |
| | |
| LineDashStyle | [LineDashStyle](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a1a4cde304137cc6e6bc05f4088bcb16e)`[get, set]` |
| | Gets or sets the line dash style for drawing the value data series. |
| | |
| bool | [ShowZeroValue](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a81709dd6712b23f9bbabce016f0a0f0e)`[get, set]` |
| | Gets or sets whether to show zero value on price axis for the value data series. |
| | |
| bool | [ShowCurrentValue](./classATAS_1_1Indicators_1_1ValueDataSeries.md#ae907ee964811c528eeda3c44b31080f2)`[get, set]` |
| | Gets or sets whether to show the current value on the price panel for the value data series. |
| | |
| bool | [ScaleIt](./classATAS_1_1Indicators_1_1ValueDataSeries.md#afa31f6a4e94d7dcef6dc0d9bed2a1910)`[get, set]` |
| | Gets or sets whether to use scaling for the value data series. |
| | |
| override int | [Count](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a12c93c47d7672c4fdffe1c7fb6cea96c)`[get]` |
| | |
| override bool | [IsVisible](./classATAS_1_1Indicators_1_1ValueDataSeries.md#ab22b8d8d66ed87aeb54e8ad58eb93f30)`[get]` |
| | |
| override decimal | [this[int index]](./classATAS_1_1Indicators_1_1ValueDataSeries.md#a78b410e2e05c7fcd1f89637349430dc8)`[get, set]` |
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

Represents a data series of decimal values, each element is a decimal.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ValueDataSeries() [1/2]

| ATAS.Indicators.ValueDataSeries.ValueDataSeries | ( | string | id, |
| --- | --- | --- | --- |
| | | string | name |
| | ) | | |

Initializes a new instance of the ValueDataSeries class with the specified unique and constant data series ID for data serialization and unique name.

Parameters

| id | The unique and constant data series ID for data serialization. |
| --- | --- |
| name | The unique data series name. |

## [◆](https://docs.atas.net/en/)ValueDataSeries() [2/2]

| ATAS.Indicators.ValueDataSeries.ValueDataSeries | ( | string | id | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the ValueDataSeries class with the specified unique and constant data series ID for data serialization.

Parameters

| id | The unique and constant data series ID for data serialization. |
| --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| override void ATAS.Indicators.ValueDataSeries.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.Indicators.BaseDataSeries](./classATAS_1_1Indicators_1_1BaseDataSeries.md#a37cbf8c6cd546067a84a0124871b153f).

## [◆](https://docs.atas.net/en/)IsThisPointOfStartBar()

| bool ATAS.Indicators.ValueDataSeries.IsThisPointOfStartBar | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

Checks if the specified bar index is a point of start for a line in the value data series.

Parameters

| bar | The index of the bar to check. |
| --- | --- |

ReturnsTrue if the specified bar index is a point of start for a line; otherwise, false.

## [◆](https://docs.atas.net/en/)LastNonZeroValue()

| decimal ATAS.Indicators.ValueDataSeries.LastNonZeroValue | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the last non-zero value in the range from 0 to the specified bar.

Parameters

| bar | The index of the last bar of the range. |
| --- | --- |

ReturnsThe last non-zero value within the specified range of bars. If no non-zero value is found, the value at the starting bar index is returned.

## [◆](https://docs.atas.net/en/)RemovePointOfEndLine()

| void ATAS.Indicators.ValueDataSeries.RemovePointOfEndLine | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

Removes the specified bar index as a point of end for a line in the value data series.

Parameters

| bar | The index of the bar to remove as a point of end. |
| --- | --- |

## [◆](https://docs.atas.net/en/)SetPointOfEndLine()

| void ATAS.Indicators.ValueDataSeries.SetPointOfEndLine | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

Sets the specified bar index as a point of end for a line in the value data series.

Parameters

| bar | The index of the bar to set as a point of end. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Color

| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) ATAS.Indicators.ValueDataSeries.Color |
| --- |

getset

Gets or sets the color for drawing the value data series.

## [◆](https://docs.atas.net/en/)Colors

| [BarColors](./classATAS_1_1Indicators_1_1ValueDataSeries_1_1BarColors.md) ATAS.Indicators.ValueDataSeries.Colors |
| --- |

get

Allows to change color per bar. Color value will be used for all bars by default.

## [◆](https://docs.atas.net/en/)Count

| override int ATAS.Indicators.ValueDataSeries.Count |
| --- |

get

## [◆](https://docs.atas.net/en/)Digits

| int ATAS.Indicators.ValueDataSeries.Digits |
| --- |

getset

Gets or sets the number of digits after the decimal point for formatting the value data series.

## [◆](https://docs.atas.net/en/)IsVisible

| override bool ATAS.Indicators.ValueDataSeries.IsVisible |
| --- |

get

## [◆](https://docs.atas.net/en/)LineDashStyle

| LineDashStyle ATAS.Indicators.ValueDataSeries.LineDashStyle |
| --- |

getset

Gets or sets the line dash style for drawing the value data series.

## [◆](https://docs.atas.net/en/)RenderColor

| System.Drawing.Color ATAS.Indicators.ValueDataSeries.RenderColor |
| --- |

getset

## [◆](https://docs.atas.net/en/)ScaleIt

| bool ATAS.Indicators.ValueDataSeries.ScaleIt |
| --- |

getset

Gets or sets whether to use scaling for the value data series.

## [◆](https://docs.atas.net/en/)ShowCurrentValue

| bool ATAS.Indicators.ValueDataSeries.ShowCurrentValue |
| --- |

getset

Gets or sets whether to show the current value on the price panel for the value data series.

## [◆](https://docs.atas.net/en/)ShowOnlyNonZeroLabels

| bool ATAS.Indicators.ValueDataSeries.ShowOnlyNonZeroLabels |
| --- |

getset

Always draw last non-zero value on price axis.

## [◆](https://docs.atas.net/en/)ShowZeroValue

| bool ATAS.Indicators.ValueDataSeries.ShowZeroValue |
| --- |

getset

Gets or sets whether to show zero value on price axis for the value data series.

## [◆](https://docs.atas.net/en/)StringFormat

| string ATAS.Indicators.ValueDataSeries.StringFormat |
| --- |

getset

Gets or sets the price string format for formatting the value data series.

## [◆](https://docs.atas.net/en/)this[int index]

| override decimal ATAS.Indicators.ValueDataSeries.this[int index] |
| --- |

getset

## [◆](https://docs.atas.net/en/)ValuesColor

| System.Drawing.Color ATAS.Indicators.ValueDataSeries.ValuesColor |
| --- |

getset

Gets or sets the values text color for the value data series.

## [◆](https://docs.atas.net/en/)VisualType

| [VisualMode](../namespaces/namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb) ATAS.Indicators.ValueDataSeries.VisualType |
| --- |

getset

Gets or sets the visual mode for drawing the value data series.

## [◆](https://docs.atas.net/en/)Width

| int ATAS.Indicators.ValueDataSeries.Width |
| --- |

getset

Gets or sets the width for drawing the value data series.

## [◆](https://docs.atas.net/en/)ZeroValue

| decimal ATAS.Indicators.ValueDataSeries.ZeroValue |
| --- |

getset

Value of zero line for 'Histogram' mode.

The documentation for this class was generated from the following file:
- [ValueDataSeries.cs](../files/ValueDataSeries_8cs.md)
