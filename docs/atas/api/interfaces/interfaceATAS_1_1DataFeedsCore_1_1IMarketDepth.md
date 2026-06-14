# ATAS.DataFeedsCore.IMarketDepth Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.html

Represents a market depth entry.
 [More...](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#details)

Inheritance diagram for ATAS.DataFeedsCore.IMarketDepth:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [IMarketDepth](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) | [Clone](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a5f276674807d98c7d6645f5924e1feae) () |
| | Creates a new instance of the IMarketDepth interface that is a copy of the current instance. |
| | |

| Properties | |
| --- | --- |
| DateTime | [Time](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#ac44084dc8f7ab022bcca3759a21805c7)`[get, set]` |
| | Gets or sets the date and time of the market depth entry. |
| | |
| int | [Type](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#af8c7d0c0c2c474e471cdc3daf17da8fb)`[get, set]` |
| | Gets or sets the type of the market depth entry. This property is used to distinguish different types of orders or market depth information. |
| | |
| decimal | [Price](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a97e64088c47ab3d8fe89e2cc61c17295)`[get, set]` |
| | Gets or sets the price of the market depth entry. |
| | |
| decimal | [Volume](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a9b98defdc9bd2e5bfa8d1a144aedb0f3)`[get, set]` |
| | Gets or sets the volume of the market depth entry. |
| | |
| bool | [IsAsk](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#a8b5ef31b585ed56e9ebe463c47c26dd5)`[get]` |
| | Gets a value indicating whether the market depth entry represents an ask (sell) order. |
| | |
| bool | [IsBid](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#ae33613948b7f4516240787059dea022d)`[get]` |
| | Gets a value indicating whether the market depth entry represents a bid (buy) order. |
| | |

## Detailed Description

Represents a market depth entry.

This interface exists only to implement generic DomManager because it could be used with old and new MarketData classes.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [IMarketDepth](./interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) ATAS.DataFeedsCore.IMarketDepth.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

Creates a new instance of the IMarketDepth interface that is a copy of the current instance.

ReturnsA new instance that is a copy of the current instance.

Implemented in [ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#ae0fcc138c58731c5dba65860754c9c7b).

## Property Documentation

## [◆](https://docs.atas.net/en/)IsAsk

| bool ATAS.DataFeedsCore.IMarketDepth.IsAsk |
| --- |

get

Gets a value indicating whether the market depth entry represents an ask (sell) order.

Implemented in [ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a1a08085d2e9695fba09871950d50fbb4).

## [◆](https://docs.atas.net/en/)IsBid

| bool ATAS.DataFeedsCore.IMarketDepth.IsBid |
| --- |

get

Gets a value indicating whether the market depth entry represents a bid (buy) order.

Implemented in [ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a9a32e7ab414edbfea1e0afd24f5cba37).

## [◆](https://docs.atas.net/en/)Price

| decimal ATAS.DataFeedsCore.IMarketDepth.Price |
| --- |

getset

Gets or sets the price of the market depth entry.

Implemented in [ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a05824061fbfced7e191f3d4d5a2b2d80).

## [◆](https://docs.atas.net/en/)Time

| DateTime ATAS.DataFeedsCore.IMarketDepth.Time |
| --- |

getset

Gets or sets the date and time of the market depth entry.

Implemented in [ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a6e86513a8ac102b8571f3120506cf0d5).

## [◆](https://docs.atas.net/en/)Type

| int ATAS.DataFeedsCore.IMarketDepth.Type |
| --- |

getset

Gets or sets the type of the market depth entry. This property is used to distinguish different types of orders or market depth information.

Implemented in [ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#a0666f4d3b753ceeb5c4b4705b73c708c).

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.IMarketDepth.Volume |
| --- |

getset

Gets or sets the volume of the market depth entry.

Implemented in [ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#ad8fb43839e67edca2ecd4790965df7c2).

The documentation for this interface was generated from the following file:
- [IMarketDepth.cs](../files/IMarketDepth_8cs.md)
