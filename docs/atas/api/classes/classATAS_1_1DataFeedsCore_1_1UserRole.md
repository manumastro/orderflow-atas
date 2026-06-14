# ATAS.DataFeedsCore.UserRole Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1UserRole.html

Inheritance diagram for ATAS.DataFeedsCore.UserRole:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.UserRole:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a9b292bb672c4a60c6cb679513596ceaa) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a778766c88cd71c5469794fcbd7019a08) () |
| | Returns a string that represents the current object. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a9dc087da611d124cab0ebf6b5388d4c3) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a2d5dc18482d7384cfa62c3cb9f6c6d68)`[get, set]` |
| | |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1UserRole.md#aa0353ec90ceeac73ffe436367564b74d)`[get, set]` |
| | |
| string | [Description](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a4622680922f2948faf90ac273688cda2)`[get, set]` |
| | |
| HashSet | [Rights](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a707b9995bc08fb6ea9462b41f4c2c9d4)`[get, set]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a577924f34534976c1edb5674552f14d5)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1UserRole.md#a13f3303e92197d9bf78aedff2bc91922) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)UserRole()

| ATAS.DataFeedsCore.UserRole.UserRole | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.UserRole.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.UserRole.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## Property Documentation

## [◆](https://docs.atas.net/en/)Description

| string ATAS.DataFeedsCore.UserRole.Description |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.UserRole.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.UserRole.Id |
| --- |

getset

## [◆](https://docs.atas.net/en/)Name

| string ATAS.DataFeedsCore.UserRole.Name |
| --- |

getset

## [◆](https://docs.atas.net/en/)Rights

| HashSet ATAS.DataFeedsCore.UserRole.Rights |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.UserRole.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [UserRole.cs](../files/UserRole_8cs.md)
