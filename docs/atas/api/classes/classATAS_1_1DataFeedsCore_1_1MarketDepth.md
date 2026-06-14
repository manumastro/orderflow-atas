# ATAS.DataFeedsCore.MarketDepth Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1MarketDepth.html

Represents a market depth entry.
 [More...](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#details)

Inheritance diagram for ATAS.DataFeedsCore.MarketDepth:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.MarketDepth:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a82c915dc917a8b8b880f2d36cf966803) () |
| | Initializes a new instance of the MarketDepth class. |
| | |
| [IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#ae0fcc138c58731c5dba65860754c9c7b) () |
| | Creates a new instance of the [IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) interface that is a copy of the current instance.ReturnsA new instance that is a copy of the current instance. |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a9b6d82f698e19b4fc6b8f65427e2911d) () |
| | Returns a string representation of the market depth entry. |
| | |
| [IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) | [Clone](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a5f276674807d98c7d6645f5924e1feae) () |
| | Creates a new instance of the IMarketDepth interface that is a copy of the current instance. |
| | |

| Properties | |
| --- | --- |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#adad32cb784ccc57cd04eec9134e5ccb8)`[get]` |
| | Gets the entity type, which is EntityType.MarketDepth. |
| | |
| string | [ECN](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a6a496ec99d19964954aab8424a373a3d)`[get, set]` |
| | Gets or sets the Electronic Communication Network (ECN) associated with the market depth entry. |
| | |
| decimal | [Price](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a05824061fbfced7e191f3d4d5a2b2d80)`[get, set]` |
| | Gets or sets the price of the market depth entry. |
| | |
| decimal | [Volume](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#ad8fb43839e67edca2ecd4790965df7c2)`[get, set]` |
| | Gets or sets the volume of the market depth entry. |
| | |
| int | [OrdersCount](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a72544bd61ea053d3f03f4e3125522c28)`[get, set]` |
| | Gets or sets the number of orders at the market depth entry. |
| | |
| DateTime | [Time](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a6e86513a8ac102b8571f3120506cf0d5)`[get, set]` |
| | Gets or sets the date and time of the market depth entry. |
| | |
| [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) | [Type](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a0666f4d3b753ceeb5c4b4705b73c708c)`[get, set]` |
| | Gets or sets the market data type of the market depth entry. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#aeaf5dc4e159701d65c65fd753fe543a9)`[get, set]` |
| | Gets or sets the security associated with the market depth entry. |
| | |
| bool | [IsAsk](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a1a08085d2e9695fba09871950d50fbb4)`[get]` |
| | Gets a value indicating whether the market depth entry represents an ask (sell) order. |
| | |
| bool | [IsBid](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a9a32e7ab414edbfea1e0afd24f5cba37)`[get]` |
| | Gets a value indicating whether the market depth entry represents a bid (buy) order. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) | |
| DateTime | [Time](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#ac44084dc8f7ab022bcca3759a21805c7)`[get, set]` |
| | Gets or sets the date and time of the market depth entry. |
| | |
| int | [Type](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#af8c7d0c0c2c474e471cdc3daf17da8fb)`[get, set]` |
| | Gets or sets the type of the market depth entry. This property is used to distinguish different types of orders or market depth information. |
| | |
| decimal | [Price](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a97e64088c47ab3d8fe89e2cc61c17295)`[get, set]` |
| | Gets or sets the price of the market depth entry. |
| | |
| decimal | [Volume](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a9b98defdc9bd2e5bfa8d1a144aedb0f3)`[get, set]` |
| | Gets or sets the volume of the market depth entry. |
| | |
| bool | [IsAsk](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a8b5ef31b585ed56e9ebe463c47c26dd5)`[get]` |
| | Gets a value indicating whether the market depth entry represents an ask (sell) order. |
| | |
| bool | [IsBid](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#ae33613948b7f4516240787059dea022d)`[get]` |
| | Gets a value indicating whether the market depth entry represents a bid (buy) order. |
| | |

## Detailed Description

Represents a market depth entry.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)MarketDepth()

| ATAS.DataFeedsCore.MarketDepth.MarketDepth | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the MarketDepth class.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) ATAS.DataFeedsCore.MarketDepth.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

Creates a new instance of the [IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) interface that is a copy of the current instance.ReturnsA new instance that is a copy of the current instance.

Implements [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a5f276674807d98c7d6645f5924e1feae).

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.MarketDepth.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string representation of the market depth entry.

ReturnsA string containing the date and time, market data type, price, volume, and security of the market depth entry.

## Property Documentation

## [◆](https://docs.atas.net/en/)ECN

| string ATAS.DataFeedsCore.MarketDepth.ECN |
| --- |

getset

Gets or sets the Electronic Communication Network (ECN) associated with the market depth entry.

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.MarketDepth.EntityType |
| --- |

get

Gets the entity type, which is EntityType.MarketDepth.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)IsAsk

| bool ATAS.DataFeedsCore.MarketDepth.IsAsk |
| --- |

get

Gets a value indicating whether the market depth entry represents an ask (sell) order.

Implements [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a8b5ef31b585ed56e9ebe463c47c26dd5).

## [◆](https://docs.atas.net/en/)IsBid

| bool ATAS.DataFeedsCore.MarketDepth.IsBid |
| --- |

get

Gets a value indicating whether the market depth entry represents a bid (buy) order.

Implements [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#ae33613948b7f4516240787059dea022d).

## [◆](https://docs.atas.net/en/)OrdersCount

| int ATAS.DataFeedsCore.MarketDepth.OrdersCount |
| --- |

getset

Gets or sets the number of orders at the market depth entry.

## [◆](https://docs.atas.net/en/)Price

| decimal ATAS.DataFeedsCore.MarketDepth.Price |
| --- |

getset

Gets or sets the price of the market depth entry.

Implements [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a97e64088c47ab3d8fe89e2cc61c17295).

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.MarketDepth.Security |
| --- |

getset

Gets or sets the security associated with the market depth entry.

## [◆](https://docs.atas.net/en/)Time

| DateTime ATAS.DataFeedsCore.MarketDepth.Time |
| --- |

getset

Gets or sets the date and time of the market depth entry.

Implements [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#ac44084dc8f7ab022bcca3759a21805c7).

## [◆](https://docs.atas.net/en/)Type

| [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) ATAS.DataFeedsCore.MarketDepth.Type |
| --- |

getset

Gets or sets the market data type of the market depth entry.

Implements [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#af8c7d0c0c2c474e471cdc3daf17da8fb).

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.MarketDepth.Volume |
| --- |

getset

Gets or sets the volume of the market depth entry.

Implements [ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a9b98defdc9bd2e5bfa8d1a144aedb0f3).

The documentation for this class was generated from the following file:
- [MarketDepth.cs](../files/MarketDepth_8cs.md)
