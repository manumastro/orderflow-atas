# ATAS.Indicators.ChartObject Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1ChartObject.html

Base class for objects in a chart.
 [More...](./classATAS_1_1Indicators_1_1ChartObject.md#details)

Inheritance diagram for ATAS.Indicators.ChartObject:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.ChartObject:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
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

| Properties | |
| --- | --- |
| bool | [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432)`[get, set]` |
| | Gets or sets a value indicating whether the chart object is visible. |
| | |
| bool | [Locked](./classATAS_1_1Indicators_1_1ChartObject.md#adce7929b08a500d8d0af56fba421a7f6)`[get, set]` |
| | Gets or sets a value indicating whether the chart object is locked. |
| | |
| bool | [AllowedInteraction](./classATAS_1_1Indicators_1_1ChartObject.md#ad1e5ae606113f094b1179006f16db457)`[get]` |
| | Gets a value indicating whether interaction with the chart object is allowed. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Events inherited from [ATAS.Indicators.Filters.TrackedPropertyBase](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md) | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#a975a4e91d70dc5e245000edd16233886) |
| | |

## Detailed Description

Base class for objects in a chart.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetCursor()

| virtual StdCursor ATAS.Indicators.ChartObject.GetCursor | ( | RenderControlMouseEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Gets the cursor to display when the mouse is over the chart object.

Parameters

| e | The mouse event arguments. |
| --- | --- |

ReturnsThe standard cursor to display.

## [◆](https://docs.atas.net/en/)LockedOnChanged()

| virtual void ATAS.Indicators.ChartObject.LockedOnChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the Locked property changes.

## [◆](https://docs.atas.net/en/)OnVisibleChanged()

| virtual void ATAS.Indicators.ChartObject.OnVisibleChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the Visible property changes.

Reimplemented in [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#a0c345165dff8b0c687383c0bc8db7755), and [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1e63af84e4b7f823b5b286a3444c64eb).

## [◆](https://docs.atas.net/en/)ProcessKeyDown()

| virtual bool ATAS.Indicators.ChartObject.ProcessKeyDown | ( | [CrossKeyEventArgs](../files/Indicators_2GlobalUsings_8cs.md#a2e32a04f09342ad7cc35bdf38fd5f960) | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a key down event on the chart object.

Parameters

| e | The key event arguments. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## [◆](https://docs.atas.net/en/)ProcessKeyUp()

| virtual bool ATAS.Indicators.ChartObject.ProcessKeyUp | ( | [CrossKeyEventArgs](../files/Indicators_2GlobalUsings_8cs.md#a2e32a04f09342ad7cc35bdf38fd5f960) | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a key up event on the chart object.

Parameters

| e | The key event arguments. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## [◆](https://docs.atas.net/en/)ProcessMouseClick()

| virtual bool ATAS.Indicators.ChartObject.ProcessMouseClick | ( | RenderControlMouseEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a mouse click event on the chart object.

Parameters

| e | The mouse event arguments. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## [◆](https://docs.atas.net/en/)ProcessMouseDoubleClick()

| virtual bool ATAS.Indicators.ChartObject.ProcessMouseDoubleClick | ( | RenderControlMouseEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a mouse double click event on the chart object.

Parameters

| e | The mouse event arguments. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## [◆](https://docs.atas.net/en/)ProcessMouseDown()

| virtual bool ATAS.Indicators.ChartObject.ProcessMouseDown | ( | RenderControlMouseEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a mouse down event on the chart object.

Parameters

| e | The mouse event arguments. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## [◆](https://docs.atas.net/en/)ProcessMouseMove()

| virtual bool ATAS.Indicators.ChartObject.ProcessMouseMove | ( | RenderControlMouseEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a mouse move event on the chart object.

Parameters

| e | The mouse event arguments. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## [◆](https://docs.atas.net/en/)ProcessMouseUp()

| virtual bool ATAS.Indicators.ChartObject.ProcessMouseUp | ( | RenderControlMouseEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a mouse up event on the chart object.

Parameters

| e | The mouse event arguments. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## [◆](https://docs.atas.net/en/)ProcessMouseWheel()

| virtual bool ATAS.Indicators.ChartObject.ProcessMouseWheel | ( | int | delta | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Processes a mouse wheel event on the chart object.

Parameters

| delta | The mouse wheel delta value. |
| --- | --- |

Returns`true` if the event was handled; otherwise, `false`.

## Property Documentation

## [◆](https://docs.atas.net/en/)AllowedInteraction

| bool ATAS.Indicators.ChartObject.AllowedInteraction |
| --- |

get

Gets a value indicating whether interaction with the chart object is allowed.

## [◆](https://docs.atas.net/en/)Locked

| bool ATAS.Indicators.ChartObject.Locked |
| --- |

getset

Gets or sets a value indicating whether the chart object is locked.

## [◆](https://docs.atas.net/en/)Visible

| bool ATAS.Indicators.ChartObject.Visible |
| --- |

getset

Gets or sets a value indicating whether the chart object is visible.

The documentation for this class was generated from the following file:
- [ChartObject.cs](../files/ChartObject_8cs.md)
