# ATAS.Indicators.Filters.TrackedPropertyBase Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.html

A base class for tracking property changes and notifying subscribers when a property value is modified.
 [More...](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#details)

Inheritance diagram for ATAS.Indicators.Filters.TrackedPropertyBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Filters.TrackedPropertyBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| void | [SetProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#af0d5001e98aaa5522f03dad9e2a2e445) (ref TProperty store, TProperty value, Action? onChanged=null, Func? onChanging=null, [CallerMemberName] string propertyName="") |
| | Sets the value of a property and notifies subscribers if the value has changed. |
| | |
| void | [SetTrackedProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#a8889a2db640b8dafb3b7cf8266f8d343) (ref TProperty store, TProperty value, Action? onChanged=null, [CallerMemberName] string propertyName="") |
| | Sets the value of a property that implements the INotifyPropertyChanged interface and notifies subscribers if the value has changed. |
| | |
| virtual void | [OnChangeProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#ac7aa2516aa4dc73893b2352a89c6d719) ([CallerMemberName] string propertyName="") |
| | Notifies subscribers when a property value changes. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#a975a4e91d70dc5e245000edd16233886) |
| | |

## Detailed Description

A base class for tracking property changes and notifying subscribers when a property value is modified.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnChangeProperty()

| virtual void ATAS.Indicators.Filters.TrackedPropertyBase.OnChangeProperty | ( | [CallerMemberName] string | propertyName = `""` | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Notifies subscribers when a property value changes.

Parameters

| propertyName | The name of the property whose value has changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)SetProperty()

| void ATAS.Indicators.Filters.TrackedPropertyBase.SetProperty | ( | ref TProperty | store, |
| --- | --- | --- | --- |
| | | TProperty | value, |
| | | Action? | onChanged = `null`, |
| | | Func? | onChanging = `null`, |
| | | [CallerMemberName] string | propertyName = `""` |
| | ) | | |

protected

Sets the value of a property and notifies subscribers if the value has changed.

Template Parameters

| TProperty | The type of the property. |
| --- | --- |

Parameters

| store | Reference to the backing field of the property. |
| --- | --- |
| value | The new value to set. |
| onChanged | Optional action to be executed after the property value has changed. |
| onChanging | Optional function to be executed before the property value changes to determine if the value can be changed. |
| propertyName | The name of the property. |

## [◆](https://docs.atas.net/en/)SetTrackedProperty()

| void ATAS.Indicators.Filters.TrackedPropertyBase.SetTrackedProperty | ( | ref TProperty | store, |
| --- | --- | --- | --- |
| | | TProperty | value, |
| | | Action? | onChanged = `null`, |
| | | [CallerMemberName] string | propertyName = `""` |
| | ) | | |

protected

Sets the value of a property that implements the INotifyPropertyChanged interface and notifies subscribers if the value has changed.

Template Parameters

| TProperty | The type of the property that implements the INotifyPropertyChanged interface. |
| --- | --- |

Parameters

| store | Reference to the backing field of the property. |
| --- | --- |
| value | The new value to set. |
| onChanged | Optional action to be executed after the property value has changed. |
| propertyName | The name of the property. |

Type Constraints

| TProperty | : | class | |
| --- | --- | --- | --- |
| TProperty | : | INotifyPropertyChanged | |

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler? ATAS.Indicators.Filters.TrackedPropertyBase.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [TrackedPropertyBase.cs](../files/TrackedPropertyBase_8cs.md)
