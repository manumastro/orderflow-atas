# ATAS.DataFeedsCore.News Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1News.html

Inheritance diagram for ATAS.DataFeedsCore.News:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.News:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1News.md#a8e86f36e6f412223f84cc1e10270d593) () |
| | Returns a string that represents the current object. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1News.md#a05d3ed711c34583e87a042043b5d9360) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1News.md#a817984931ab24d216a68becf9ecef73a)`[get, set]` |
| | |
| string | [AccountID](./classATAS_1_1DataFeedsCore_1_1News.md#a0422ea1f438414e423ae058a99627bc7)`[get, set]` |
| | |
| [NewsType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a6e7968f109c5e89f8e38eec40f049070) | [Type](./classATAS_1_1DataFeedsCore_1_1News.md#af0cd5cc42bbb436243ac08909a4350aa)`[get, set]` |
| | |
| DateTime | [Time](./classATAS_1_1DataFeedsCore_1_1News.md#a186e75f04c337e91c7c30fe2b9964dd5)`[get, set]` |
| | |
| string | [Source](./classATAS_1_1DataFeedsCore_1_1News.md#aa0108099b3c275fc51e005c51aee1ceb)`[get, set]` |
| | |
| string | [Title](./classATAS_1_1DataFeedsCore_1_1News.md#a81b145aa504c309416e11a848ab7d9b1)`[get, set]` |
| | |
| string | [Text](./classATAS_1_1DataFeedsCore_1_1News.md#ae2095ae9b64b17715837f6fdf8227b43)`[get, set]` |
| | |
| bool | [IsHandled](./classATAS_1_1DataFeedsCore_1_1News.md#a6edf34b9c62ca2c230806dedbbdfec88)`[get, set]` |
| | |
| long | [UserId](./classATAS_1_1DataFeedsCore_1_1News.md#ad97de7e0827462521b77da014479d166)`[get, set]` |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md) | [User](./classATAS_1_1DataFeedsCore_1_1News.md#aa5ecddab3291a5cb3287cc49222fbeda)`[get, set]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1News.md#ad98bbe3c41ba2f18144c351af3ed3f77)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1News.md#a56a1a5b445be73c4141e425d48804d3a) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.News.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.News.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## Property Documentation

## [◆](https://docs.atas.net/en/)AccountID

| string ATAS.DataFeedsCore.News.AccountID |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.News.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.News.Id |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsHandled

| bool ATAS.DataFeedsCore.News.IsHandled |
| --- |

getset

## [◆](https://docs.atas.net/en/)Source

| string ATAS.DataFeedsCore.News.Source |
| --- |

getset

## [◆](https://docs.atas.net/en/)Text

| string ATAS.DataFeedsCore.News.Text |
| --- |

getset

## [◆](https://docs.atas.net/en/)Time

| DateTime ATAS.DataFeedsCore.News.Time |
| --- |

getset

## [◆](https://docs.atas.net/en/)Title

| string ATAS.DataFeedsCore.News.Title |
| --- |

getset

## [◆](https://docs.atas.net/en/)Type

| [NewsType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a6e7968f109c5e89f8e38eec40f049070) ATAS.DataFeedsCore.News.Type |
| --- |

getset

## [◆](https://docs.atas.net/en/)User

| [User](./classATAS_1_1DataFeedsCore_1_1User.md) ATAS.DataFeedsCore.News.User |
| --- |

getset

## [◆](https://docs.atas.net/en/)UserId

| long ATAS.DataFeedsCore.News.UserId |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.News.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [News.cs](../files/News_8cs.md)
