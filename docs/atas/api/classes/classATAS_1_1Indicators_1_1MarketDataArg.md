# ATAS.Indicators.MarketDataArg Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1MarketDataArg.html

Represents a data point in the market.
 [More...](./classATAS_1_1Indicators_1_1MarketDataArg.md#details)

Inheritance diagram for ATAS.Indicators.MarketDataArg:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.MarketDataArg:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md#ae8b6e66676234a1c270ebd3477a3d04d) () |
| | Initializes a new instance of the MarketDataArg class. |
| | |

| Properties | |
| --- | --- |
| decimal | [Price](./classATAS_1_1Indicators_1_1MarketDataArg.md#a1ef4c95ced4fefd7676f6bc937d918c2)`[get, set]` |
| | Price of the market data. |
| | |
| decimal | [OriginPrice](./classATAS_1_1Indicators_1_1MarketDataArg.md#a3f525d8950a036fe147515cea634d9a5)`[get, set]` |
| | Original, not scaled price of the market data. |
| | |
| decimal | [Volume](./classATAS_1_1Indicators_1_1MarketDataArg.md#af26ae9654aaf55ec8ddda0cc62770f17)`[get, set]` |
| | Volume associated with the market data. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [Time](./classATAS_1_1Indicators_1_1MarketDataArg.md#afa0a58bb9d30f50fdfeac2c9e632c808)`[get, set]` |
| | Time at which the market data occurred. |
| | |
| [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) | [Direction](./classATAS_1_1Indicators_1_1MarketDataArg.md#aa73632f63f6118a5ef7fb725776ace04)`[get, set]` |
| | Trade direction of the market data (Buy, Sell, or Between). |
| | |
| [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) | [DataType](./classATAS_1_1Indicators_1_1MarketDataArg.md#a3b289020252f5dd79b19e9bd8be7cc30)`[get, set]` |
| | Type of the market data (Bid, Ask, or Trade). |
| | |
| decimal | [OpenInterest](./classATAS_1_1Indicators_1_1MarketDataArg.md#a8447c7ce6ccc0c608062e36a13c3eb5e)`[get, set]` |
| | Open interest associated with the market data. |
| | |
| bool | [IsAsk](./classATAS_1_1Indicators_1_1MarketDataArg.md#a4fdd9497023ccc096f851bb41238deba)`[get]` |
| | Gets a value indicating whether the market data is of type Ask. |
| | |
| bool | [IsBid](./classATAS_1_1Indicators_1_1MarketDataArg.md#aff1d2ad15fcfcbe2662bf86c5eedcd64)`[get]` |
| | Gets a value indicating whether the market data is of type Bid. |
| | |
| long? | [AggressorExchangeOrderId](./classATAS_1_1Indicators_1_1MarketDataArg.md#a5454fa47800c6bfffd19e60bb7c00684)`[get, set]` |
| | Gets or sets the aggressor exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade. |
| | |
| long? | [ExchangeOrderId](./classATAS_1_1Indicators_1_1MarketDataArg.md#a12d94e38a655dc1e8366eec4e528937c)`[get, set]` |
| | Gets or sets the exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

## Detailed Description

Represents a data point in the market.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)MarketDataArg()

| ATAS.Indicators.MarketDataArg.MarketDataArg | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the MarketDataArg class.

## Property Documentation

## [◆](https://docs.atas.net/en/)AggressorExchangeOrderId

| long? ATAS.Indicators.MarketDataArg.AggressorExchangeOrderId |
| --- |

getset

Gets or sets the aggressor exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade.

## [◆](https://docs.atas.net/en/)DataType

| [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) ATAS.Indicators.MarketDataArg.DataType |
| --- |

getset

Type of the market data (Bid, Ask, or Trade).

## [◆](https://docs.atas.net/en/)Direction

| [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) ATAS.Indicators.MarketDataArg.Direction |
| --- |

getset

Trade direction of the market data (Buy, Sell, or Between).

## [◆](https://docs.atas.net/en/)ExchangeOrderId

| long? ATAS.Indicators.MarketDataArg.ExchangeOrderId |
| --- |

getset

Gets or sets the exchange order id (see the MarketByOrder.ExchangeOrderId property) associated with this trade.

## [◆](https://docs.atas.net/en/)IsAsk

| bool ATAS.Indicators.MarketDataArg.IsAsk |
| --- |

get

Gets a value indicating whether the market data is of type Ask.

## [◆](https://docs.atas.net/en/)IsBid

| bool ATAS.Indicators.MarketDataArg.IsBid |
| --- |

get

Gets a value indicating whether the market data is of type Bid.

## [◆](https://docs.atas.net/en/)OpenInterest

| decimal ATAS.Indicators.MarketDataArg.OpenInterest |
| --- |

getset

Open interest associated with the market data.

## [◆](https://docs.atas.net/en/)OriginPrice

| decimal ATAS.Indicators.MarketDataArg.OriginPrice |
| --- |

getset

Original, not scaled price of the market data.

## [◆](https://docs.atas.net/en/)Price

| decimal ATAS.Indicators.MarketDataArg.Price |
| --- |

getset

Price of the market data.

## [◆](https://docs.atas.net/en/)Time

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.MarketDataArg.Time |
| --- |

getset

Time at which the market data occurred.

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.Indicators.MarketDataArg.Volume |
| --- |

getset

Volume associated with the market data.

The documentation for this class was generated from the following file:
- [MarketDataArg.cs](../files/MarketDataArg_8cs.md)
