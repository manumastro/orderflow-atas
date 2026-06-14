# ATAS.DataFeedsCore.User Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1User.html

Inheritance diagram for ATAS.DataFeedsCore.User:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.User:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [User](./classATAS_1_1DataFeedsCore_1_1User.md#a5f802cf6bd8352c0fd402beca03cddf8) () |
| | |
| | [User](./classATAS_1_1DataFeedsCore_1_1User.md#a4cf6c09409221f77c847af533533dc4a) (string login, string password) |
| | |
| void | [SetFakePassword](./classATAS_1_1DataFeedsCore_1_1User.md#a2e7d54ee28d6d59d5b2f43abb807079b) () |
| | |
| bool | [Check](./classATAS_1_1DataFeedsCore_1_1User.md#a245568f5c11443617c00a96181c786fd) (string password) |
| | |
| void | [Update](./classATAS_1_1DataFeedsCore_1_1User.md#addb1a1e297d784228a909347f4891073) (string password) |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1User.md#a45c7f8f399ce8fe806852b8229793018) () |
| | Returns a string that represents the current object. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1User.md#a5c8777afd9b0a66caba116df31ac94f6) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1User.md#ae3ec29369750f572c2110871acfe2627)`[get, set]` |
| | |
| long | [GroupId](./classATAS_1_1DataFeedsCore_1_1User.md#a45528849f5a457090d74f449194f09b4)`[get, set]` |
| | |
| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [Group](./classATAS_1_1DataFeedsCore_1_1User.md#aba6d7e15cd517d26e5c530bdf892a078)`[get, set]` |
| | |
| long | [RoleId](./classATAS_1_1DataFeedsCore_1_1User.md#a8b7bd24c22c87b531c62b8924e3c06ce)`[get, set]` |
| | |
| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) | [Role](./classATAS_1_1DataFeedsCore_1_1User.md#a42b2867093e7678a87169f985964af74)`[get, set]` |
| | |
| string | [Login](./classATAS_1_1DataFeedsCore_1_1User.md#ab1c17f8b52174cb752ff81ea517857d2)`[get, set]` |
| | |
| string | [Password](./classATAS_1_1DataFeedsCore_1_1User.md#a86f17a0e040e94a10eda37bd6e7cdf39)`[get, set]` |
| | |
| string | [Salt](./classATAS_1_1DataFeedsCore_1_1User.md#a6c45fa876eb9811107d0a825d32a16fd)`[get, set]` |
| | |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1User.md#a50533fc3b53756319ce91dba6440fe41)`[get, set]` |
| | |
| string | [Description](./classATAS_1_1DataFeedsCore_1_1User.md#a99f08d346a44dda1a88e9f19066b4855)`[get, set]` |
| | |
| string | [Email](./classATAS_1_1DataFeedsCore_1_1User.md#a543e35c83ff4f36a369eea73066580a8)`[get, set]` |
| | |
| string | [Country](./classATAS_1_1DataFeedsCore_1_1User.md#acd175af4410ba7efb510ccd5b3c02bbf)`[get, set]` |
| | |
| string | [Zip](./classATAS_1_1DataFeedsCore_1_1User.md#ae032c9f787e2339e53604361d3095a03)`[get, set]` |
| | |
| string | [Address](./classATAS_1_1DataFeedsCore_1_1User.md#afa183746aa4dcc3786fc9b8d32352829)`[get, set]` |
| | |
| string | [Phone](./classATAS_1_1DataFeedsCore_1_1User.md#a006580b9ff272206a83c184d9ac1c4ce)`[get, set]` |
| | |
| bool | [IsOnline](./classATAS_1_1DataFeedsCore_1_1User.md#a0bacda28d126bdab69ae13070c707ea8)`[get, set]` |
| | |
| bool | [IsLocked](./classATAS_1_1DataFeedsCore_1_1User.md#a83fee43ee6ee3fd4dbe8632b4ca250df)`[get, set]` |
| | |
| DateTime? | [ExpiryDate](./classATAS_1_1DataFeedsCore_1_1User.md#a2189aa8854f6f22d96ef46274730630c)`[get, set]` |
| | |
| DateTime? | [LastLogonTime](./classATAS_1_1DataFeedsCore_1_1User.md#a60b0213c7545b1fee35e484cfd56c4fe)`[get, set]` |
| | |
| List | [Portfolios](./classATAS_1_1DataFeedsCore_1_1User.md#a0bca0cf8aef1f4e47aaf7b4e0569e973)`[get]` |
| | |
| List | [PortfolioViewers](./classATAS_1_1DataFeedsCore_1_1User.md#ae262e44e744f2acc21ae209abe0d9888)`[get]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1User.md#aef11b944fb1c8ee5d1ce633336d6ef77)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1User.md#a7b4bd0b74fae31715dc24d9837f9cc79) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)User() [1/2]

| ATAS.DataFeedsCore.User.User | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)User() [2/2]

| ATAS.DataFeedsCore.User.User | ( | string | login, |
| --- | --- | --- | --- |
| | | string | password |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Check()

| bool ATAS.DataFeedsCore.User.Check | ( | string | password | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.User.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)SetFakePassword()

| void ATAS.DataFeedsCore.User.SetFakePassword | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.User.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## [◆](https://docs.atas.net/en/)Update()

| void ATAS.DataFeedsCore.User.Update | ( | string | password | ) | |
| --- | --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Address

| string ATAS.DataFeedsCore.User.Address |
| --- |

getsetadd

## [◆](https://docs.atas.net/en/)Country

| string ATAS.DataFeedsCore.User.Country |
| --- |

getset

## [◆](https://docs.atas.net/en/)Description

| string ATAS.DataFeedsCore.User.Description |
| --- |

getset

## [◆](https://docs.atas.net/en/)Email

| string ATAS.DataFeedsCore.User.Email |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.User.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)ExpiryDate

| DateTime? ATAS.DataFeedsCore.User.ExpiryDate |
| --- |

getset

## [◆](https://docs.atas.net/en/)Group

| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) ATAS.DataFeedsCore.User.Group |
| --- |

getset

## [◆](https://docs.atas.net/en/)GroupId

| long ATAS.DataFeedsCore.User.GroupId |
| --- |

getset

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.User.Id |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsLocked

| bool ATAS.DataFeedsCore.User.IsLocked |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsOnline

| bool ATAS.DataFeedsCore.User.IsOnline |
| --- |

getset

## [◆](https://docs.atas.net/en/)LastLogonTime

| DateTime? ATAS.DataFeedsCore.User.LastLogonTime |
| --- |

getset

## [◆](https://docs.atas.net/en/)Login

| string ATAS.DataFeedsCore.User.Login |
| --- |

getset

## [◆](https://docs.atas.net/en/)Name

| string ATAS.DataFeedsCore.User.Name |
| --- |

getset

## [◆](https://docs.atas.net/en/)Password

| string ATAS.DataFeedsCore.User.Password |
| --- |

getset

## [◆](https://docs.atas.net/en/)Phone

| string ATAS.DataFeedsCore.User.Phone |
| --- |

getset

## [◆](https://docs.atas.net/en/)Portfolios

| List ATAS.DataFeedsCore.User.Portfolios |
| --- |

get

## [◆](https://docs.atas.net/en/)PortfolioViewers

| List ATAS.DataFeedsCore.User.PortfolioViewers |
| --- |

get

## [◆](https://docs.atas.net/en/)Role

| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) ATAS.DataFeedsCore.User.Role |
| --- |

getset

## [◆](https://docs.atas.net/en/)RoleId

| long ATAS.DataFeedsCore.User.RoleId |
| --- |

getset

## [◆](https://docs.atas.net/en/)Salt

| string ATAS.DataFeedsCore.User.Salt |
| --- |

getset

## [◆](https://docs.atas.net/en/)Zip

| string ATAS.DataFeedsCore.User.Zip |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.User.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [User.cs](../files/User_8cs.md)
