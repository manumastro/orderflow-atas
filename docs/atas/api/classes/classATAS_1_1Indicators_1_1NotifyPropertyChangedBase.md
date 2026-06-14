# ATAS.Indicators.NotifyPropertyChangedBase Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.html

Base class for implementing the INotifyPropertyChanged interface.
 [More...](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#details)

Inheritance diagram for ATAS.Indicators.NotifyPropertyChangedBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.NotifyPropertyChangedBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| void | [RaisePropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0cc277df1751846320a5a4d5493a7d27) ([CallerMemberName] string? propertyName=null!) |
| | Raises the PropertyChanged event for the specified property name. |
| | |
| void | [SetProperty](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#aaa4ebf490512045cb9606084bb8cae74) (ref TProperty store, TProperty value, [CallerMemberName] string? propertyName=null, Action? onChanged=null) |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0dc11be741c756c9a36c0756d2f5527a) |
| | Event that is raised when a property value changes. |
| | |

## Detailed Description

Base class for implementing the INotifyPropertyChanged interface.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)RaisePropertyChanged()

| void ATAS.Indicators.NotifyPropertyChangedBase.RaisePropertyChanged | ( | [CallerMemberName] string? | propertyName = `null!` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PropertyChanged event for the specified property name.

Parameters

| propertyName | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)SetProperty()

| void ATAS.Indicators.NotifyPropertyChangedBase.SetProperty | ( | ref TProperty | store, |
| --- | --- | --- | --- |
| | | TProperty | value, |
| | | [CallerMemberName] string? | propertyName = `null`, |
| | | Action? | onChanged = `null` |
| | ) | | |

protected

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler? ATAS.Indicators.NotifyPropertyChangedBase.PropertyChanged |
| --- |

Event that is raised when a property value changes.

The documentation for this class was generated from the following file:
- [NotifyPropertyChangedBase.cs](../files/NotifyPropertyChangedBase_8cs.md)
