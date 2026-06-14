# ATAS.DataFeedsCore.Exchange Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Exchange.html

Inheritance diagram for ATAS.DataFeedsCore.Exchange:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Exchange:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md#aac5ef6efdf28845417bb7260144dd5e1) () |
| | |
| bool | [IsWorkingTime](./classATAS_1_1DataFeedsCore_1_1Exchange.md#ac03d31a7056971c1548c01c996c15b6c) (DateTime time) |
| | |
| bool | [IsNewSession](./classATAS_1_1DataFeedsCore_1_1Exchange.md#aeaf2fd2447d9bf81df6bb85cb81cfc24) (DateTime prevTime, DateTime newTime) |
| | |
| bool | [IsNewWeek](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a993e53bb8a8d97648e0609f46a174da8) (DateTime prevTime, DateTime newTime) |
| | |
| bool | [IsNewMonth](./classATAS_1_1DataFeedsCore_1_1Exchange.md#ac2a5695fdc80c676480bedd9b258263c) (DateTime prevTime, DateTime newTime) |
| | |
| [WorkingTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md) | [GetWorkingTime](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a6b3a8137fa841c4f411270084500de58) (DateTime time) |
| | |
| DateTime? | [GetNextSessionOpen](./classATAS_1_1DataFeedsCore_1_1Exchange.md#aa2ccd3fbed3624e4bdbae4fe5c4d1736) (DateTime time) |
| | |
| DateTime? | [GetPreviousSessionClose](./classATAS_1_1DataFeedsCore_1_1Exchange.md#ac6324a10efc634e1528ae7eaedc020f9) (DateTime time) |
| | |
| DateTime?? DateTime MaxTradedTime | [TrimToMinTradedRange](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a3897a3c4d6a6e93c55030210a8edd498) (DateTime from, DateTime to) |
| | |
| Tuple | [GetWorkingDateTime](./classATAS_1_1DataFeedsCore_1_1Exchange.md#acf298cf79a07b7ce5f1af6c41f252b94) (DateTime time) |
| | |
| DateTime | [ToLocalTime](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a42dc89fd6e195e3e0412a77b0918fc0d) (DateTime time) |
| | |
| DateTime | [ToUtcTime](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a949cb5d02aec01f3217f6cca74232967) (DateTime time) |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1Exchange.md#aa0c6683dce0a8b74a451bb60058711dc) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a1f22507310367532ba5c7db9742278e6) () |
| | Returns a string that represents the current object. |
| | |
| bool | [IsNewSession](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md#a1cc1e421306ad674cc747c04f9a9967b) (DateTime prevTime, DateTime newTime) |
| | |
| bool | [IsNewWeek](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md#af3f6273dd8ce937b6008c31e74fccc3f) (DateTime prevTime, DateTime newTime) |
| | |
| bool | [IsNewMonth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md#acfc52e69f4b2f61846ea1d36c1ace223) (DateTime prevTime, DateTime newTime) |
| | |

| Public Attributes | |
| --- | --- |
| DateTime? | [MinTradedTime](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a4245522629559a663a0d982e489d42ce) |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Exchange.md#ae70190b50aea4e290df7706dda7a15b3) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| string | [Code](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a6a1d67a90a4cc634b356aa47f749a5fb)`[get, set]` |
| | |
| string | [ExchangeCode](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a0a722a7bc5ed4a94f629f3571dc3991a)`[get, set]` |
| | |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a369333b1027ad1e42f740109fb71103e)`[get, set]` |
| | |
| string | [Country](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a6b98fa1c9633940de3ac5fefc9032cb6)`[get, set]` |
| | |
| string | [TimeZone](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a7d72655846cd52b05f465829a37ec37b)`[get, set]` |
| | |
| TimeZoneInfo | [TimeZoneInfo](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a6097f27e0a1ace20766f3f0822e74f44)`[get, set]` |
| | |
| List | [WorkingTimes](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a7750c62f321364b60b96abca26478321)`[get, set]` |
| | |
| bool | [ConvertTradeTimeToLocal](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a225ac2c8a728f9320fe64efcdff1eb75)`[get, set]` |
| | |
| DayOfWeek | [FirstDayOfWeek](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a7b87163e2f3749e7d8fba11b3319b5d0)`[get, set]` |
| | |
| bool | [IsSystemExchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a2056e70faa4f7493f7255585109e4a35)`[get, set]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1Exchange.md#a3f7c90184bae0501d4db52390e7fbe90)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Exchange.md#ac73686bd2731cdd005bee0c7a707945c) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Exchange()

| ATAS.DataFeedsCore.Exchange.Exchange | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) ATAS.DataFeedsCore.Exchange.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetNextSessionOpen()

| DateTime? ATAS.DataFeedsCore.Exchange.GetNextSessionOpen | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetPreviousSessionClose()

| DateTime? ATAS.DataFeedsCore.Exchange.GetPreviousSessionClose | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetWorkingDateTime()

| Tuple ATAS.DataFeedsCore.Exchange.GetWorkingDateTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetWorkingTime()

| [WorkingTime](./classATAS_1_1DataFeedsCore_1_1WorkingTime.md) ATAS.DataFeedsCore.Exchange.GetWorkingTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)IsNewMonth()

| bool ATAS.DataFeedsCore.Exchange.IsNewMonth | ( | DateTime | prevTime, |
| --- | --- | --- | --- |
| | | DateTime | newTime |
| | ) | | |

Implements [ATAS.DataFeedsCore.IKnowSession](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md#acfc52e69f4b2f61846ea1d36c1ace223).

## [◆](https://docs.atas.net/en/)IsNewSession()

| bool ATAS.DataFeedsCore.Exchange.IsNewSession | ( | DateTime | prevTime, |
| --- | --- | --- | --- |
| | | DateTime | newTime |
| | ) | | |

Implements [ATAS.DataFeedsCore.IKnowSession](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md#a1cc1e421306ad674cc747c04f9a9967b).

## [◆](https://docs.atas.net/en/)IsNewWeek()

| bool ATAS.DataFeedsCore.Exchange.IsNewWeek | ( | DateTime | prevTime, |
| --- | --- | --- | --- |
| | | DateTime | newTime |
| | ) | | |

Implements [ATAS.DataFeedsCore.IKnowSession](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md#af3f6273dd8ce937b6008c31e74fccc3f).

## [◆](https://docs.atas.net/en/)IsWorkingTime()

| bool ATAS.DataFeedsCore.Exchange.IsWorkingTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.Exchange.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToLocalTime()

| DateTime ATAS.DataFeedsCore.Exchange.ToLocalTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.Exchange.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## [◆](https://docs.atas.net/en/)ToUtcTime()

| DateTime ATAS.DataFeedsCore.Exchange.ToUtcTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)TrimToMinTradedRange()

| DateTime?? DateTime MaxTradedTime ATAS.DataFeedsCore.Exchange.TrimToMinTradedRange | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to |
| | ) | | |

## Member Data Documentation

## [◆](https://docs.atas.net/en/)MinTradedTime

| DateTime? ATAS.DataFeedsCore.Exchange.MinTradedTime |
| --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Code

| string ATAS.DataFeedsCore.Exchange.Code |
| --- |

getset

## [◆](https://docs.atas.net/en/)ConvertTradeTimeToLocal

| bool ATAS.DataFeedsCore.Exchange.ConvertTradeTimeToLocal |
| --- |

getset

## [◆](https://docs.atas.net/en/)Country

| string ATAS.DataFeedsCore.Exchange.Country |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.Exchange.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)ExchangeCode

| string ATAS.DataFeedsCore.Exchange.ExchangeCode |
| --- |

getset

## [◆](https://docs.atas.net/en/)FirstDayOfWeek

| DayOfWeek ATAS.DataFeedsCore.Exchange.FirstDayOfWeek |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsSystemExchange

| bool ATAS.DataFeedsCore.Exchange.IsSystemExchange |
| --- |

getset

## [◆](https://docs.atas.net/en/)Name

| string ATAS.DataFeedsCore.Exchange.Name |
| --- |

getset

## [◆](https://docs.atas.net/en/)TimeZone

| string ATAS.DataFeedsCore.Exchange.TimeZone |
| --- |

getset

## [◆](https://docs.atas.net/en/)TimeZoneInfo

| TimeZoneInfo ATAS.DataFeedsCore.Exchange.TimeZoneInfo |
| --- |

getset

## [◆](https://docs.atas.net/en/)WorkingTimes

| List ATAS.DataFeedsCore.Exchange.WorkingTimes |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.Exchange.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [Exchange.cs](../files/Exchange_8cs.md)
