# ATAS.DataFeedsCore.Trade Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Trade.html

Represents an tick on a financial exchange.
 [More...](./classATAS_1_1DataFeedsCore_1_1Trade.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Trade:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Trade:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md#a71dbde8b14bf602fe0ff9e25979c1841) () |
| | Initializes a new instance of the Trade class. |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Trade.md#aba98b24e705afd12fd1f2177589018c9) () |
| | |

| Properties | |
| --- | --- |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1Trade.md#a9b4977a5fd6615c82db5b3de89182b27)`[get]` |
| | Gets the type of the entity. |
| | |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1Trade.md#a20cb31cd03f0b5b031c8f89aa5f6598f)`[get, set]` |
| | Gets or sets the id of the trade. |
| | |
| [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) | [OrderDirection](./classATAS_1_1DataFeedsCore_1_1Trade.md#a50b4b4af9185f603290f9a707a5f741c)`[get, set]` |
| | Gets or sets the side of the trade. |
| | |
| decimal | [Price](./classATAS_1_1DataFeedsCore_1_1Trade.md#aa219be682a6e49de02fe369ecd3aca75)`[get, set]` |
| | Gets or sets the price associated with this trade. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1DataFeedsCore_1_1Trade.md#abf151a8c473d1d48e58dbbf7b4b7282b)`[get, set]` |
| | Gets or sets the security associated with the trade entry. |
| | |
| DateTime | [Time](./classATAS_1_1DataFeedsCore_1_1Trade.md#aca8f7ebe35d7158f993f104ccd889e33)`[get, set]` |
| | Gets or sets the date and time of the trade entry. |
| | |
| decimal | [Volume](./classATAS_1_1DataFeedsCore_1_1Trade.md#adc300f74131f66d881d4c184f8b4422a)`[get, set]` |
| | Gets or sets the volume associated with this trade. |
| | |
| decimal | [OpenInterest](./classATAS_1_1DataFeedsCore_1_1Trade.md#a559d83d9b4133c9a6349f5dc2219eb3d)`[get, set]` |
| | Gets or sets the open interest associated with this trade. |
| | |
| string | [ECN](./classATAS_1_1DataFeedsCore_1_1Trade.md#ab458ff7fe39eb1397cacfcfad32ba7f8)`[get, set]` |
| | Gets or sets the Electronic Communication Network (ECN) associated with the trade entry. |
| | |
| long? | [AggressorExchangeOrderId](./classATAS_1_1DataFeedsCore_1_1Trade.md#a1f4b3df85dadad69a54ba43218060031)`[get, set]` |
| | Gets or sets the aggressor exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade. |
| | |
| long? | [ExchangeOrderId](./classATAS_1_1DataFeedsCore_1_1Trade.md#a70230ef5269958a37354252e5ca424d9)`[get, set]` |
| | Gets or sets the exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

## Detailed Description

Represents an tick on a financial exchange.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Trade()

| ATAS.DataFeedsCore.Trade.Trade | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the Trade class.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.Trade.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AggressorExchangeOrderId

| long? ATAS.DataFeedsCore.Trade.AggressorExchangeOrderId |
| --- |

getset

Gets or sets the aggressor exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade.

## [◆](https://docs.atas.net/en/)ECN

| string ATAS.DataFeedsCore.Trade.ECN |
| --- |

getset

Gets or sets the Electronic Communication Network (ECN) associated with the trade entry.

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.Trade.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)ExchangeOrderId

| long? ATAS.DataFeedsCore.Trade.ExchangeOrderId |
| --- |

getset

Gets or sets the exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade.

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.Trade.Id |
| --- |

getset

Gets or sets the id of the trade.

## [◆](https://docs.atas.net/en/)OpenInterest

| decimal ATAS.DataFeedsCore.Trade.OpenInterest |
| --- |

getset

Gets or sets the open interest associated with this trade.

## [◆](https://docs.atas.net/en/)OrderDirection

| [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) ATAS.DataFeedsCore.Trade.OrderDirection |
| --- |

getset

Gets or sets the side of the trade.

## [◆](https://docs.atas.net/en/)Price

| decimal ATAS.DataFeedsCore.Trade.Price |
| --- |

getset

Gets or sets the price associated with this trade.

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.Trade.Security |
| --- |

getset

Gets or sets the security associated with the trade entry.

## [◆](https://docs.atas.net/en/)Time

| DateTime ATAS.DataFeedsCore.Trade.Time |
| --- |

getset

Gets or sets the date and time of the trade entry.

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.Trade.Volume |
| --- |

getset

Gets or sets the volume associated with this trade.

The documentation for this class was generated from the following file:
- [Trade.cs](../files/Trade_8cs.md)
