# ATAS.Indicators.FilterRangeValue< TValue > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1FilterRangeValue.html

Represents a range of values of type TValue with support for property change notifications.
 [More...](./classATAS_1_1Indicators_1_1FilterRangeValue.md#details)

Inheritance diagram for ATAS.Indicators.FilterRangeValue< TValue >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.FilterRangeValue< TValue >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| TValue? | [Start](./classATAS_1_1Indicators_1_1FilterRangeValue.md#a6dc03d8a3baedbfc96d7e52e74bf2110)`[get, set]` |
| | Gets or sets the start value of the range. |
| | |
| TValue? | [End](./classATAS_1_1Indicators_1_1FilterRangeValue.md#ac6e652e988cd6944f961a464adea1694)`[get, set]` |
| | Gets or sets the end value of the range. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Protected Member Functions inherited from [ATAS.Indicators.NotifyPropertyChangedBase](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | |
| void | [RaisePropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0cc277df1751846320a5a4d5493a7d27) ([CallerMemberName] string? propertyName=null!) |
| | Raises the PropertyChanged event for the specified property name. |
| | |
| void | [SetProperty](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#aaa4ebf490512045cb9606084bb8cae74) (ref TProperty store, TProperty value, [CallerMemberName] string? propertyName=null, Action? onChanged=null) |
| | |
| - Events inherited from [ATAS.Indicators.NotifyPropertyChangedBase](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0dc11be741c756c9a36c0756d2f5527a) |
| | Event that is raised when a property value changes. |
| | |

## Detailed Description

Represents a range of values of type TValue with support for property change notifications.

Template Parameters

| TValue | The type of values the range can hold. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)End

| TValue? [ATAS.Indicators.FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md).End |
| --- |

getset

Gets or sets the end value of the range.

## [◆](https://docs.atas.net/en/)Start

| TValue? [ATAS.Indicators.FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md).Start |
| --- |

getset

Gets or sets the start value of the range.

The documentation for this class was generated from the following file:
- [FilterRangeValue.cs](../files/FilterRangeValue_8cs.md)
