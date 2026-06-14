# ATAS.DataFeedsCore.UserGroup Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1UserGroup.html

Inheritance diagram for ATAS.DataFeedsCore.UserGroup:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.UserGroup:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a04fd9455b39aaebf4527b8fe102154cd) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#ade0488d24f3bd400b2f5a9435f73ae2a) () |
| | Returns a string that represents the current object. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a1b886ea1accf7d80b24de355d9117de5) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#aa50f4f039ec75860866eaca3c9de7885)`[get, set]` |
| | |
| string | [Code](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#ac238602a8c65685cd725704bb71e2594)`[get, set]` |
| | |
| int | [LastPortfolioNumber](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a520462c6b02bbf0db2b1dd07cc5dbf5a)`[get, set]` |
| | |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a35cd9438fa8c5a7bae98d726607576a7)`[get, set]` |
| | |
| string | [Description](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a8dea46e57bc1b35d3e76ba425955b2e5)`[get, set]` |
| | |
| bool | [AllowLogonWithWrongPassword](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a27f8813a4c710138c383321b96c02eeb)`[get, set]` |
| | |
| TimeSpan? | [DefaultExpiration](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a94e476596b5d06e80ebd0991690eaac5)`[get, set]` |
| | |
| string? | [StatisticsServerUrl](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a2f7e9041de480d8bdea88b1c90b0f653)`[get, set]` |
| | |
| long? | [ParentId](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a587d01ec1c7960af808aee5c5d2fa73b)`[get, set]` |
| | |
| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [Parent](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#aff7a089cbe825f2b5204da63e9b562a2)`[get, set]` |
| | |
| List | [Exchanges](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a8ae9e2d077d0cedfd08599df8a096777)`[get, set]` |
| | |
| long? | [TradingOptionsId](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a56f8384c4d5ff2fc06177ec173171656)`[get, set]` |
| | |
| [TradingOptions](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md) | [TradingOptions](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a68d196087b2e1e950b3ba0ae204a9511)`[get, set]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a7fba3981dcf476e4b0855e68e6d234a2)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1UserGroup.md#a6ac8e6e2ad2349128d99b68c0d27af61) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)UserGroup()

| ATAS.DataFeedsCore.UserGroup.UserGroup | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.UserGroup.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.UserGroup.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## Property Documentation

## [◆](https://docs.atas.net/en/)AllowLogonWithWrongPassword

| bool ATAS.DataFeedsCore.UserGroup.AllowLogonWithWrongPassword |
| --- |

getset

## [◆](https://docs.atas.net/en/)Code

| string ATAS.DataFeedsCore.UserGroup.Code |
| --- |

getset

## [◆](https://docs.atas.net/en/)DefaultExpiration

| TimeSpan? ATAS.DataFeedsCore.UserGroup.DefaultExpiration |
| --- |

getset

## [◆](https://docs.atas.net/en/)Description

| string ATAS.DataFeedsCore.UserGroup.Description |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.UserGroup.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)Exchanges

| List ATAS.DataFeedsCore.UserGroup.Exchanges |
| --- |

getset

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.UserGroup.Id |
| --- |

getset

## [◆](https://docs.atas.net/en/)LastPortfolioNumber

| int ATAS.DataFeedsCore.UserGroup.LastPortfolioNumber |
| --- |

getset

## [◆](https://docs.atas.net/en/)Name

| string ATAS.DataFeedsCore.UserGroup.Name |
| --- |

getset

## [◆](https://docs.atas.net/en/)Parent

| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) ATAS.DataFeedsCore.UserGroup.Parent |
| --- |

getset

## [◆](https://docs.atas.net/en/)ParentId

| long? ATAS.DataFeedsCore.UserGroup.ParentId |
| --- |

getset

## [◆](https://docs.atas.net/en/)StatisticsServerUrl

| string? ATAS.DataFeedsCore.UserGroup.StatisticsServerUrl |
| --- |

getset

## [◆](https://docs.atas.net/en/)TradingOptions

| [TradingOptions](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md) ATAS.DataFeedsCore.UserGroup.TradingOptions |
| --- |

getset

## [◆](https://docs.atas.net/en/)TradingOptionsId

| long? ATAS.DataFeedsCore.UserGroup.TradingOptionsId |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.UserGroup.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [UserGroup.cs](../files/UserGroup_8cs.md)
