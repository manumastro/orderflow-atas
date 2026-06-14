# ATAS.Indicators.BaseIndicator Class Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1BaseIndicator.html

Base class for custom indicators in a chart.
 [More...](./classATAS_1_1Indicators_1_1BaseIndicator.md#details)

Inheritance diagram for ATAS.Indicators.BaseIndicator:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.BaseIndicator:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| virtual void | [Dispose](./classATAS_1_1Indicators_1_1BaseIndicator.md#af616a09632eca22bb3f2ea3900ed019c) () |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1BaseIndicator.md#a01c654afdfeaaeafb968e990f4ca9b47) () |
| | Converts the current instance of the indicator to its string representation. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.ChartObject](./classATAS_1_1Indicators_1_1ChartObject.md) | |
| virtual bool | [ProcessMouseClick](./classATAS_1_1Indicators_1_1ChartObject.md#a90e21830bc8fa463037483b601d7e654) (RenderControlMouseEventArgs e) |
| | Processes a mouse click event on the chart object. |
| | |
| virtual bool | [ProcessMouseWheel](./classATAS_1_1Indicators_1_1ChartObject.md#ad20fdb9dfd4aba37065467b226572638) (int delta) |
| | Processes a mouse wheel event on the chart object. |
| | |
| virtual bool | [ProcessMouseDown](./classATAS_1_1Indicators_1_1ChartObject.md#a77e00d6cc1c1be74712c7a75bfc3c9fc) (RenderControlMouseEventArgs e) |
| | Processes a mouse down event on the chart object. |
| | |
| virtual bool | [ProcessMouseUp](./classATAS_1_1Indicators_1_1ChartObject.md#a850f79393af0ff85608968f8fb5b578e) (RenderControlMouseEventArgs e) |
| | Processes a mouse up event on the chart object. |
| | |
| virtual bool | [ProcessMouseMove](./classATAS_1_1Indicators_1_1ChartObject.md#aab135d7efa994d847b9b2d1596773360) (RenderControlMouseEventArgs e) |
| | Processes a mouse move event on the chart object. |
| | |
| virtual bool | [ProcessMouseDoubleClick](./classATAS_1_1Indicators_1_1ChartObject.md#ac62955c0a5bdc92c250ccb39e0b53adf) (RenderControlMouseEventArgs e) |
| | Processes a mouse double click event on the chart object. |
| | |
| virtual StdCursor | [GetCursor](./classATAS_1_1Indicators_1_1ChartObject.md#ac9d5e69d8961eea481dfc3944c270aae) (RenderControlMouseEventArgs e) |
| | Gets the cursor to display when the mouse is over the chart object. |
| | |
| virtual bool | [ProcessKeyDown](./classATAS_1_1Indicators_1_1ChartObject.md#a86bf534d0b898d493dffe622e6dd86b1) ([CrossKeyEventArgs](../files/Indicators_2GlobalUsings_8cs.md#a2e32a04f09342ad7cc35bdf38fd5f960) e) |
| | Processes a key down event on the chart object. |
| | |
| virtual bool | [ProcessKeyUp](./classATAS_1_1Indicators_1_1ChartObject.md#a159acef22dc292a0a5e637c91d007be3) ([CrossKeyEventArgs](../files/Indicators_2GlobalUsings_8cs.md#a2e32a04f09342ad7cc35bdf38fd5f960) e) |
| | Processes a key up event on the chart object. |
| | |

| Protected Member Functions | |
| --- | --- |
| | [BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#ab626bc6b30ded7783c6e3f4a36daf118) (bool useCandles=false) |
| | |
| virtual void | [RecalculateValues](./classATAS_1_1Indicators_1_1BaseIndicator.md#a6ee783f61e21eb5ff39be738b77dbd1e) () |
| | Recalculate the indicator values on each bar. |
| | |
| virtual void | [OnInitialize](./classATAS_1_1Indicators_1_1BaseIndicator.md#a78707b5100f92c64cea2865420e9ac83) () |
| | The method is executed before the first calculation. |
| | |
| virtual void | [OnRecalculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#aa4ef19f2c8896360932dbca55e96fb71) () |
| | The method is executed before a new calculation. |
| | |
| virtual void | [OnFinishRecalculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#aa5373860b1b58abf8e60be4700ea1217) () |
| | The method is executed after the end of the calculation. |
| | |
| virtual void | [Calculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#a6cd6e11444382222a48db90a5672d462) (int bar, decimal value) |
| | Performs the calculation for the indicator at the specified bar and value. |
| | |
| abstract void | [OnCalculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#a65c50fc1fec7d6c490aa23a1c3eeebd3) (int bar, decimal value) |
| | The main indicator calculation method is called for each bar on the history, then it is called on each tick. |
| | |
| void | [Add](./classATAS_1_1Indicators_1_1BaseIndicator.md#a94e1ea2fe81d54e12eaab51282b3c798) ([Indicator](./classATAS_1_1Indicators_1_1Indicator.md) indicator) |
| | Adds an indicator to the list of used indicators by this indicator. |
| | |
| void | [Clear](./classATAS_1_1Indicators_1_1BaseIndicator.md#ab88e03387b7c90b2752cbf5f862acfb0) () |
| | Clear all data series. |
| | |
| virtual void | [OnSourceChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#afe98089377ca3e76d9122cc7d5eef586) () |
| | This method is called when the SourceDataSeries property is changed. |
| | |
| virtual void | [OnPropertiesEditorChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#af8c8db3b926d834d3011713fec9dbc05) ([IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? oldValue, [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? newValue) |
| | Called when the PropertiesEditor property changes. |
| | |
| void | [RaisePropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#afef6aaa070ba5020a82851ec203eccc3) (string propertyName) |
| | Raises the PropertyChanged event for the specified property name. |
| | |
| void | [RaisePropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a460dfb3d56ebb0675f085686a355a3ba) (object? sender, PropertyChangedEventArgs e) |
| | Raises the PropertyChanged event with the specified event arguments. |
| | |
| void | [RaisePanelPropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a50a0ff42e3004e36a19fd2c00202c8ef) (string name) |
| | Raises the PanelPropertyChanged event with the specified property name. |
| | |
| void | [RaiseBarValueChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a18f034b71be84074920c8ff091378299) (int bar) |
| | Raises the BarValueChanged event with the specified bar value. |
| | |
| virtual void | [OnDispose](./classATAS_1_1Indicators_1_1BaseIndicator.md#a9212779df0ab1c6bf2a2d76f1774931b) () |
| | Called when the indicator is being disposed. |
| | |
| override void | [OnVisibleChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a0c345165dff8b0c687383c0bc8db7755) () |
| | Called when the [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432) property changes. |
| | |
| - Protected Member Functions inherited from [ATAS.Indicators.ChartObject](./classATAS_1_1Indicators_1_1ChartObject.md) | |
| virtual void | [OnVisibleChanged](./classATAS_1_1Indicators_1_1ChartObject.md#aebafef7ef3d98bc7b8026c6b4c0e6315) () |
| | Called when the Visible property changes. |
| | |
| virtual void | [LockedOnChanged](./classATAS_1_1Indicators_1_1ChartObject.md#af14593844c5be179a01e710ae152eda7) () |
| | Called when the Locked property changes. |
| | |
| - Protected Member Functions inherited from [ATAS.Indicators.Filters.TrackedPropertyBase](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md) | |
| void | [SetProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#af0d5001e98aaa5522f03dad9e2a2e445) (ref TProperty store, TProperty value, Action? onChanged=null, Func? onChanging=null, [CallerMemberName] string propertyName="") |
| | Sets the value of a property and notifies subscribers if the value has changed. |
| | |
| void | [SetTrackedProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#a8889a2db640b8dafb3b7cf8266f8d343) (ref TProperty store, TProperty value, Action? onChanged=null, [CallerMemberName] string propertyName="") |
| | Sets the value of a property that implements the INotifyPropertyChanged interface and notifies subscribers if the value has changed. |
| | |
| virtual void | [OnChangeProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#ac7aa2516aa4dc73893b2352a89c6d719) ([CallerMemberName] string propertyName="") |
| | Notifies subscribers when a property value changes. |
| | |

| Static Protected Member Functions | |
| --- | --- |
| static PerfCounter | [MeasurePerformance](./classATAS_1_1Indicators_1_1BaseIndicator.md#af4f20fe3c0d0c31be0e319f92bff0208) (string name) |
| | Measures the performance of a specific operation with the given name. If a performance diagnoser is available, it will be used to measure the performance; otherwise, a default performance counter will be returned. |
| | |

| Protected Attributes | |
| --- | --- |
| readonly List | [UsedIndicators](./classATAS_1_1Indicators_1_1BaseIndicator.md#aa05e2ac945fde12b8db000d0491f29bd) = new() |
| | The list of indicators that are being used by this indicator. |
| | |

| Properties | |
| --- | --- |
| static ? PerformanceDiagnoser | [PerformanceDiagnoser](./classATAS_1_1Indicators_1_1BaseIndicator.md#a61ec0c67898d4d0d3e045a7ad97dbd29)`[get]` |
| | Indicator performance tracker. |
| | |
| static bool | [UseProfiling](./classATAS_1_1Indicators_1_1BaseIndicator.md#abb240b488fd75dc6b9f254e739eaba22)`[get, set]` |
| | Set to `true` to measure the performance of all indicators. |
| | |
| [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? | [PropertiesEditor](./classATAS_1_1Indicators_1_1BaseIndicator.md#ac7e9c933b8fcd90a972c72999a2a5c51)`[get, set]` |
| | |
| string | [Name](./classATAS_1_1Indicators_1_1BaseIndicator.md#aab455feaa3727fcc19e50003cf0c4dca)`[get, set]` |
| | Name of the indicator. |
| | |
| bool | [IsDisposed](./classATAS_1_1Indicators_1_1BaseIndicator.md#a1464d3459efe29157642e05f9beb6de3)`[get, set]` |
| | Gets or sets a value indicating whether the indicator object has been disposed of. |
| | |
| List | [DataSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21)`[get]` |
| | List of data series used by the indicator. |
| | |
| bool | [SupportsExtendedSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#a18fa6ba1cbc6125818fa90274396f421)`[get]` |
| | Gets value indicating whether the data series can be drawn out of chart bars. |
| | |
| List | [LineSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#a95dcfd417f6a472f7391ea0792fc0b13)`[get]` |
| | List of line series used by the indicator. |
| | |
| string | [Panel](./classATAS_1_1Indicators_1_1BaseIndicator.md#a1edf20bbcde60c2dfca7ea94eaa2de89)`[get, set]` |
| | The name of the panel where the indicator is placed. |
| | |
| bool | [IsVerticalIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#a3f9791869e2f94b1b487c45f1e098c43)`[get, set]` |
| | Gets or sets a value indicating whether the indicator is intended to be displayed as a vertical indicator. |
| | |
| bool | [UseCandles](./classATAS_1_1Indicators_1_1BaseIndicator.md#a9a373643c22e84ccca84e58c2ec023ec)`[get]` |
| | Gets a value indicating whether the indicator uses candle data series. |
| | |
| int | [CurrentBar](./classATAS_1_1Indicators_1_1BaseIndicator.md#a2dad625c8cb62d89a65a46b824638b68)`[get]` |
| | Bars number. All bars and the values of the corresponding data series have a serial number. The earliest bar of the chart is assigned the number 0; the next bar is assigned the number 1, and so on. |
| | |
| [IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md)? | [SourceDataSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#af1a8638540f6a36ce4fb437988c6890a)`[get, set]` |
| | Gets or sets the data series used as the source for the indicator's calculations. |
| | |
| decimal | [this[int index]](./classATAS_1_1Indicators_1_1BaseIndicator.md#aee0c78bdc23de4c8b1a9d1aec86c8315)`[get, protected set]` |
| | Gets or sets the value of the first data series of the indicator at the specified index. |
| | |
| - Properties inherited from [ATAS.Indicators.ChartObject](./classATAS_1_1Indicators_1_1ChartObject.md) | |
| bool | [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432)`[get, set]` |
| | Gets or sets a value indicating whether the chart object is visible. |
| | |
| bool | [Locked](./classATAS_1_1Indicators_1_1ChartObject.md#adce7929b08a500d8d0af56fba421a7f6)`[get, set]` |
| | Gets or sets a value indicating whether the chart object is locked. |
| | |
| bool | [AllowedInteraction](./classATAS_1_1Indicators_1_1ChartObject.md#ad1e5ae606113f094b1179006f16db457)`[get]` |
| | Gets a value indicating whether interaction with the chart object is allowed. |
| | |
| - Properties inherited from [ATAS.Indicators.IPropertiesEditorOwner](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md) | |
| [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? | [PropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md#a06ea4c501070e8d9d05dcb8f27c8a472)`[get, set]` |
| | |

| Events | |
| --- | --- |
| new? PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a13656e94a28b82a339289c05556c4b49) |
| | |
| PropertyChangedEventHandler? | [PanelPropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a07137a5ec1366cdf4ac1588a50067faa) |
| | |
| Action? | [BarValueChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a5530e08569dd5765e8f70dc48a01de3a) |
| | Event that is raised when the value of a bar in the indicator changes. |
| | |
| - Events inherited from [ATAS.Indicators.Filters.TrackedPropertyBase](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md) | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#a975a4e91d70dc5e245000edd16233886) |
| | |
| - Events inherited from [ATAS.Indicators.INotifyPanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md) | |
| PropertyChangedEventHandler | [PanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md#a1eb7377432737676cf47033c1fe6a965) |
| | Occurs when a panel property value changes. |
| | |

## Detailed Description

Base class for custom indicators in a chart.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)BaseIndicator()

| ATAS.Indicators.BaseIndicator.BaseIndicator | ( | bool | useCandles = `false` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Add()

| void ATAS.Indicators.BaseIndicator.Add | ( | [Indicator](./classATAS_1_1Indicators_1_1Indicator.md) | indicator | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Adds an indicator to the list of used indicators by this indicator.

Parameters

| indicator | The indicator to be added to the list of used indicators. |
| --- | --- |

## [◆](https://docs.atas.net/en/)Calculate()

| virtual void ATAS.Indicators.BaseIndicator.Calculate | ( | int | bar, |
| --- | --- | --- | --- |
| | | decimal | value |
| | ) | | |

protectedvirtual

Performs the calculation for the indicator at the specified bar and value.

Parameters

| bar | The bar number for which the calculation is performed. |
| --- | --- |
| value | The input value from the data series at the specified bar. |

Reimplemented in [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a3795f4844cd693dd5630644d4092411d).

## [◆](https://docs.atas.net/en/)Clear()

| void ATAS.Indicators.BaseIndicator.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Clear all data series.

## [◆](https://docs.atas.net/en/)Dispose()

| virtual void ATAS.Indicators.BaseIndicator.Dispose | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Reimplemented in [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a6552838e546335a9b1dfad976b730eb0).

## [◆](https://docs.atas.net/en/)MeasurePerformance()

| static PerfCounter ATAS.Indicators.BaseIndicator.MeasurePerformance | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

staticprotected

Measures the performance of a specific operation with the given name. If a performance diagnoser is available, it will be used to measure the performance; otherwise, a default performance counter will be returned.

Parameters

| name | The name of the operation to measure performance for. |
| --- | --- |

ReturnsA performance counter representing the measured performance. If no performance diagnoser is available, a default performance counter will be returned.

## [◆](https://docs.atas.net/en/)OnCalculate()

| abstract void ATAS.Indicators.BaseIndicator.OnCalculate | ( | int | bar, |
| --- | --- | --- | --- |
| | | decimal | value |
| | ) | | |

protectedpure virtual

The main indicator calculation method is called for each bar on the history, then it is called on each tick.

Parameters

| bar | Bar number. |
| --- | --- |
| value | Input data series value. |

Implemented in [ATAS.Strategies.Chart.SmaChartStrategy](./classATAS_1_1Strategies_1_1Chart_1_1SmaChartStrategy.md#ac253e565e57d9fa843fbe536e8862e37).

## [◆](https://docs.atas.net/en/)OnDispose()

| virtual void ATAS.Indicators.BaseIndicator.OnDispose | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the indicator is being disposed.

## [◆](https://docs.atas.net/en/)OnFinishRecalculate()

| virtual void ATAS.Indicators.BaseIndicator.OnFinishRecalculate | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

The method is executed after the end of the calculation.

## [◆](https://docs.atas.net/en/)OnInitialize()

| virtual void ATAS.Indicators.BaseIndicator.OnInitialize | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

The method is executed before the first calculation.

## [◆](https://docs.atas.net/en/)OnPropertiesEditorChanged()

| virtual void ATAS.Indicators.BaseIndicator.OnPropertiesEditorChanged | ( | [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? | oldValue, |
| --- | --- | --- | --- |
| | | [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? | newValue |
| | ) | | |

protectedvirtual

Called when the PropertiesEditor property changes.

Parameters

| oldValue | The previous properties editor instance. |
| --- | --- |
| newValue | The new properties editor instance. |

## [◆](https://docs.atas.net/en/)OnRecalculate()

| virtual void ATAS.Indicators.BaseIndicator.OnRecalculate | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

The method is executed before a new calculation.

## [◆](https://docs.atas.net/en/)OnSourceChanged()

| virtual void ATAS.Indicators.BaseIndicator.OnSourceChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

This method is called when the SourceDataSeries property is changed.

## [◆](https://docs.atas.net/en/)OnVisibleChanged()

| override void ATAS.Indicators.BaseIndicator.OnVisibleChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432) property changes.

Reimplemented from [ATAS.Indicators.ChartObject](./classATAS_1_1Indicators_1_1ChartObject.md#aebafef7ef3d98bc7b8026c6b4c0e6315).

Reimplemented in [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1e63af84e4b7f823b5b286a3444c64eb).

## [◆](https://docs.atas.net/en/)RaiseBarValueChanged()

| void ATAS.Indicators.BaseIndicator.RaiseBarValueChanged | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the BarValueChanged event with the specified bar value.

Parameters

| bar | The bar value to pass to the event handlers. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RaisePanelPropertyChanged()

| void ATAS.Indicators.BaseIndicator.RaisePanelPropertyChanged | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PanelPropertyChanged event with the specified property name.

Parameters

| name | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RaisePropertyChanged() [1/2]

| void ATAS.Indicators.BaseIndicator.RaisePropertyChanged | ( | object? | sender, |
| --- | --- | --- | --- |
| | | PropertyChangedEventArgs | e |
| | ) | | |

protected

Raises the PropertyChanged event with the specified event arguments.

Parameters

| sender | The sender of the event. |
| --- | --- |
| e | The event arguments. |

## [◆](https://docs.atas.net/en/)RaisePropertyChanged() [2/2]

| void ATAS.Indicators.BaseIndicator.RaisePropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PropertyChanged event for the specified property name.

Parameters

| propertyName | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RecalculateValues()

| virtual void ATAS.Indicators.BaseIndicator.RecalculateValues | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Recalculate the indicator values on each bar.

Reimplemented in [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a9b335ed8cf85e1832c0ffc14013c2f2e).

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.Indicators.BaseIndicator.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Converts the current instance of the indicator to its string representation.

ReturnsA string representation of the indicator.

## Member Data Documentation

## [◆](https://docs.atas.net/en/)UsedIndicators

| readonly List ATAS.Indicators.BaseIndicator.UsedIndicators = new() |
| --- |

protected

The list of indicators that are being used by this indicator.

## Property Documentation

## [◆](https://docs.atas.net/en/)CurrentBar

| int ATAS.Indicators.BaseIndicator.CurrentBar |
| --- |

get

Bars number. All bars and the values of the corresponding data series have a serial number. The earliest bar of the chart is assigned the number 0; the next bar is assigned the number 1, and so on.

## [◆](https://docs.atas.net/en/)DataSeries

| List ATAS.Indicators.BaseIndicator.DataSeries |
| --- |

get

List of data series used by the indicator.

## [◆](https://docs.atas.net/en/)IsDisposed

| bool ATAS.Indicators.BaseIndicator.IsDisposed |
| --- |

getset

Gets or sets a value indicating whether the indicator object has been disposed of.

## [◆](https://docs.atas.net/en/)IsVerticalIndicator

| bool ATAS.Indicators.BaseIndicator.IsVerticalIndicator |
| --- |

getset

Gets or sets a value indicating whether the indicator is intended to be displayed as a vertical indicator.

## [◆](https://docs.atas.net/en/)LineSeries

| List ATAS.Indicators.BaseIndicator.LineSeries |
| --- |

get

List of line series used by the indicator.

## [◆](https://docs.atas.net/en/)Name

| string ATAS.Indicators.BaseIndicator.Name |
| --- |

getset

Name of the indicator.

## [◆](https://docs.atas.net/en/)Panel

| string ATAS.Indicators.BaseIndicator.Panel |
| --- |

getset

The name of the panel where the indicator is placed.

## [◆](https://docs.atas.net/en/)PerformanceDiagnoser

| ? PerformanceDiagnoser ATAS.Indicators.BaseIndicator.PerformanceDiagnoser |
| --- |

staticgetprotected

Indicator performance tracker.

## [◆](https://docs.atas.net/en/)PropertiesEditor

| [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? ATAS.Indicators.BaseIndicator.PropertiesEditor |
| --- |

getset

Implements [ATAS.Indicators.IPropertiesEditorOwner](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md#a06ea4c501070e8d9d05dcb8f27c8a472).

## [◆](https://docs.atas.net/en/)SourceDataSeries

| [IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md)? ATAS.Indicators.BaseIndicator.SourceDataSeries |
| --- |

getset

Gets or sets the data series used as the source for the indicator's calculations.

The source data series provides the input data for the indicator's calculations. When this property is set, the indicator will automatically recalculate its values based on the new source data series.

## [◆](https://docs.atas.net/en/)SupportsExtendedSeries

| bool ATAS.Indicators.BaseIndicator.SupportsExtendedSeries |
| --- |

get

Gets value indicating whether the data series can be drawn out of chart bars.

## [◆](https://docs.atas.net/en/)this[int index]

| decimal ATAS.Indicators.BaseIndicator.this[int index] |
| --- |

getprotected set

Gets or sets the value of the first data series of the indicator at the specified index.

Parameters

| index | The index of the value to get or set. |
| --- | --- |

ReturnsThe value of the indicator at the specified index.

## [◆](https://docs.atas.net/en/)UseCandles

| bool ATAS.Indicators.BaseIndicator.UseCandles |
| --- |

get

Gets a value indicating whether the indicator uses candle data series.

## [◆](https://docs.atas.net/en/)UseProfiling

| bool ATAS.Indicators.BaseIndicator.UseProfiling |
| --- |

staticgetsetprotected

Set to `true` to measure the performance of all indicators.

## Event Documentation

## [◆](https://docs.atas.net/en/)BarValueChanged

| Action? ATAS.Indicators.BaseIndicator.BarValueChanged |
| --- |

Event that is raised when the value of a bar in the indicator changes.

## [◆](https://docs.atas.net/en/)PanelPropertyChanged

| PropertyChangedEventHandler? ATAS.Indicators.BaseIndicator.PanelPropertyChanged |
| --- |

## [◆](https://docs.atas.net/en/)PropertyChanged

| new? PropertyChangedEventHandler ATAS.Indicators.BaseIndicator.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [BaseIndicator.cs](../files/BaseIndicator_8cs.md)
