# ATAS.DataFeedsCore.IEntityFactory Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.html

Inheritance diagram for ATAS.DataFeedsCore.IEntityFactory:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | [GetOrCreateSecurity](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#ac4a0adcc45a4c830907c368e8921089c) (string id, Func create) |
| | |
| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [GetOrCreatePortfolio](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#af5c98f24eaa380af205502b08c69c4ba) (string accountId, Func create) |
| | |
| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | [GetOrCreatePosition](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a5d990741b512ff237c7fc91034fa545e) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, Func create) |
| | |
| [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | [GetOrCreateOrder](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a6e2ffa6dbeaf742978239c850ae30c7c) (long extId, Func create) |
| | |
| [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [GetOrCreateMyTrade](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a443bf765ae3d3905a463711c0c5c418a) (string id, Func create) |
| | |
| [Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md) | [CreateTrade](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a1fd4bbe23a8760e52fb808612f633ddb) () |
| | |
| [MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [CreateMarketDepth](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#aaa3a4477e0590fd6c7a08fca3e0891d7) () |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CreateMarketDepth()

| [MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md) ATAS.DataFeedsCore.IEntityFactory.CreateMarketDepth | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a362ac55749ad2ee36ee05d4ad59b409a).

## [◆](https://docs.atas.net/en/)CreateTrade()

| [Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md) ATAS.DataFeedsCore.IEntityFactory.CreateTrade | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a03e2012cbd664c99ea66e11a47b01750).

## [◆](https://docs.atas.net/en/)GetOrCreateMyTrade()

| [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) ATAS.DataFeedsCore.IEntityFactory.GetOrCreateMyTrade | ( | string | id, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a34d2fb8904bda08f0a450af336161ea9).

## [◆](https://docs.atas.net/en/)GetOrCreateOrder()

| [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) ATAS.DataFeedsCore.IEntityFactory.GetOrCreateOrder | ( | long | extId, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa9cd6200ba8cb5c82b14d8813ea71510).

## [◆](https://docs.atas.net/en/)GetOrCreatePortfolio()

| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.DataFeedsCore.IEntityFactory.GetOrCreatePortfolio | ( | string | accountId, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a5a63d44c4e237759d36506dcdde1c2e4).

## [◆](https://docs.atas.net/en/)GetOrCreatePosition()

| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.DataFeedsCore.IEntityFactory.GetOrCreatePosition | ( | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | Func | create |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a62edda6513533967789f059ddd1107f2).

## [◆](https://docs.atas.net/en/)GetOrCreateSecurity()

| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.IEntityFactory.GetOrCreateSecurity | ( | string | id, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a29b704d52f60b160a635e2ddad1a3e89).

The documentation for this interface was generated from the following file:
- [IEntityFactory.cs](../files/IEntityFactory_8cs.md)
