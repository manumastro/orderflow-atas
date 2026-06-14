# ATAS.Indicators.FilterBase Class Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1FilterBase.html

Base class for filters implementing the IFilter interface.
 [More...](./classATAS_1_1Indicators_1_1FilterBase.md#details)

Inheritance diagram for ATAS.Indicators.FilterBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.FilterBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| abstract object | [Clone](./classATAS_1_1Indicators_1_1FilterBase.md#a42f805bcaf064026a2bf891662222f47) () |
| | |

| Protected Member Functions | |
| --- | --- |
| | [FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md#ace4c39656e88d797c3a1daa1b2bd39b3) (bool enabledVisible, bool asScalar) |
| | Initializes a new instance of the FilterBase class with the specified parameters. |
| | |
| | [FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md#ad4ee3b9baae481a6d887d441f83c8ba2) () |
| | Initializes a new instance of the FilterBase class with default parameters. |
| | |
| - Protected Member Functions inherited from [ATAS.Indicators.NotifyPropertyChangedBase](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | |
| void | [RaisePropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0cc277df1751846320a5a4d5493a7d27) ([CallerMemberName] string? propertyName=null!) |
| | Raises the PropertyChanged event for the specified property name. |
| | |
| void | [SetProperty](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#aaa4ebf490512045cb9606084bb8cae74) (ref TProperty store, TProperty value, [CallerMemberName] string? propertyName=null, Action? onChanged=null) |
| | |

| Protected Attributes | |
| --- | --- |
| readonly bool | [_asScalar](./classATAS_1_1Indicators_1_1FilterBase.md#a00db6a0ba4c414191d6848a53f5c7736) |
| | |

| Properties | |
| --- | --- |
| bool | [Enabled](./classATAS_1_1Indicators_1_1FilterBase.md#afd2fe159ae0fdd35c11b7e8718a7cff4)`[get, set]` |
| | Gets or sets a value indicating whether the filter is enabled. |
| | |
| bool | [EnabledVisible](./classATAS_1_1Indicators_1_1FilterBase.md#a6d77b9bc8bc6264a25a5323906e0b756)`[get]` |
| | Gets a value indicating whether the visibility of the "Enabled" property is visible to users. |
| | |
| - Properties inherited from [ATAS.Indicators.IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md) | |
| bool | [Enabled](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#a40afdac09510881a97b9fb7059f09db5)`[get, set]` |
| | Gets or sets a value indicating whether the filter is enabled. |
| | |
| bool | [EnabledVisible](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#afd4286d2401b7286159d764dd05b1328)`[get]` |
| | Gets a value indicating whether the visibility of the "Enabled" property is visible to users. |
| | |
| bool | [AsScalar](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#acb501180f87b4cd563f8a428dc18328c)`[get]` |
| | Gets a value indicating whether the filter operates in scalar mode. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Events inherited from [ATAS.Indicators.NotifyPropertyChangedBase](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0dc11be741c756c9a36c0756d2f5527a) |
| | Event that is raised when a property value changes. |
| | |

## Detailed Description

Base class for filters implementing the IFilter interface.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)FilterBase() [1/2]

| ATAS.Indicators.FilterBase.FilterBase | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | asScalar |
| | ) | | |

protected

Initializes a new instance of the FilterBase class with the specified parameters.

Parameters

| enabledVisible | Specifies whether the "Enabled" property is visible to users. |
| --- | --- |
| asScalar | Specifies whether the filter operates in scalar mode. |

## [◆](https://docs.atas.net/en/)FilterBase() [2/2]

| ATAS.Indicators.FilterBase.FilterBase | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Initializes a new instance of the FilterBase class with default parameters.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| abstract object ATAS.Indicators.FilterBase.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

pure virtual

Implemented in [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md#ad751565da10c97facaaba8605546ddf9).

## Member Data Documentation

## [◆](https://docs.atas.net/en/)_asScalar

| readonly bool ATAS.Indicators.FilterBase._asScalar |
| --- |

protected

## Property Documentation

## [◆](https://docs.atas.net/en/)Enabled

| bool ATAS.Indicators.FilterBase.Enabled |
| --- |

getset

Gets or sets a value indicating whether the filter is enabled.

Implements [ATAS.Indicators.IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#a40afdac09510881a97b9fb7059f09db5).

## [◆](https://docs.atas.net/en/)EnabledVisible

| bool ATAS.Indicators.FilterBase.EnabledVisible |
| --- |

get

Gets a value indicating whether the visibility of the "Enabled" property is visible to users.

Implements [ATAS.Indicators.IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#afd4286d2401b7286159d764dd05b1328).

The documentation for this class was generated from the following file:
- [FilterBase.cs](../files/FilterBase_8cs.md)
