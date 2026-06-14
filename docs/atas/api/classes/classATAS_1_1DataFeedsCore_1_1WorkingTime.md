# ATAS.DataFeedsCore.WorkingTime Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1WorkingTime.html

Inheritance diagram for ATAS.DataFeedsCore.WorkingTime:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.WorkingTime:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [WorkingTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#ab51df5650d32182017da60aeb11d19ac) () |
| | |
| bool | [IsWorkingTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#a8857dd41fc8e73c05c943f9989625bd7) (DateTime time) |
| | |
| [WorkingTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#a268aa60cf571e5cfb29442b5550bfcea) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#ab217f1d606fe4557aad41a547d21f9f1) () |
| | Returns a string that represents the current object. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#add25141fc2ef283588986c4997f2c1a0) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| string | [Exchange](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#abe7f08634b9f5e685deaaf5a60cd60d8)`[get, set]` |
| | |
| DayOfWeek | [StartDay](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#ab6a6a31764258ad95f18f07f781491a0)`[get, set]` |
| | |
| TimeSpan | [StartTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#ac6a5851737abcf066fac66fc4708ba98)`[get, set]` |
| | |
| DayOfWeek | [EndDay](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#a4f048f83f2857c774722be2a27bceb2e)`[get, set]` |
| | |
| TimeSpan | [EndTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#a13bf650a0d8170388d83b6e7d6df5f56)`[get, set]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#acdce1135bc222b04d045c18d4286563a)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md#adc2f3b01009b1a2b6a49135f153ae79b) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)WorkingTime()

| ATAS.DataFeedsCore.WorkingTime.WorkingTime | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [WorkingTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md) ATAS.DataFeedsCore.WorkingTime.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)IsWorkingTime()

| bool ATAS.DataFeedsCore.WorkingTime.IsWorkingTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.WorkingTime.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.WorkingTime.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## Property Documentation

## [◆](https://docs.atas.net/en/)EndDay

| DayOfWeek ATAS.DataFeedsCore.WorkingTime.EndDay |
| --- |

getset

## [◆](https://docs.atas.net/en/)EndTime

| TimeSpan ATAS.DataFeedsCore.WorkingTime.EndTime |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.WorkingTime.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)Exchange

| string ATAS.DataFeedsCore.WorkingTime.Exchange |
| --- |

getset

## [◆](https://docs.atas.net/en/)StartDay

| DayOfWeek ATAS.DataFeedsCore.WorkingTime.StartDay |
| --- |

getset

## [◆](https://docs.atas.net/en/)StartTime

| TimeSpan ATAS.DataFeedsCore.WorkingTime.StartTime |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.WorkingTime.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [Exchange.cs](../files/Exchange_8cs.md)
