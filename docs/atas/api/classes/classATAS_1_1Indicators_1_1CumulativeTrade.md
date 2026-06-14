# ATAS.Indicators.CumulativeTrade Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1CumulativeTrade.html

Represents a cumulative trade, which is a trade that includes multiple prints or executions.
 [More...](./classATAS_1_1Indicators_1_1CumulativeTrade.md#details)

| Public Member Functions | |
| --- | --- |
| override string | [ToString](./classATAS_1_1Indicators_1_1CumulativeTrade.md#ae971d37b591ff1e076f9e7a5cf314846) () |
| | Returns a string representation of the CumulativeTrade object. |
| | |

| Properties | |
| --- | --- |
| decimal | [FirstPrice](./classATAS_1_1Indicators_1_1CumulativeTrade.md#a6468955cd9ab6daf451668c0987130cd)`[get, set]` |
| | Gets or sets the first price of the trade. |
| | |
| decimal | [Lastprice](./classATAS_1_1Indicators_1_1CumulativeTrade.md#ace74987f901861d34ecf609f24af3672)`[get, set]` |
| | Gets or sets the last price of the trade. |
| | |
| decimal | [Volume](./classATAS_1_1Indicators_1_1CumulativeTrade.md#a76bf8e49fa9df89c2e783b67b6636249)`[get, set]` |
| | Gets or sets the cumulative volume of the trade. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [Time](./classATAS_1_1Indicators_1_1CumulativeTrade.md#ada8c570e449b7581f078ce518b69102c)`[get, set]` |
| | Gets or sets the time of the trade. |
| | |
| [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) | [Direction](./classATAS_1_1Indicators_1_1CumulativeTrade.md#aea6f9b1a50defc17d8e8377d400d08df)`[get, set]` |
| | Gets or sets the trade direction (Buy or Sell). |
| | |
| List | [Ticks](./classATAS_1_1Indicators_1_1CumulativeTrade.md#a9343a088eeb10f305c1fa97e6576e23c) = new()`[get, set]` |
| | Gets or sets the list of individual ticks (MarketDataArg) included in the cumulative trade. |
| | |
| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) | [PreviousAsk](./classATAS_1_1Indicators_1_1CumulativeTrade.md#ab19187b35b2fd9d9bd89a4e3c8cd2acd) = null!`[get, set]` |
| | Gets or sets the best ask before the trade. |
| | |
| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) | [PreviousBid](./classATAS_1_1Indicators_1_1CumulativeTrade.md#a80e0ea22eb1e0455e65a8de4d3a09798) = null!`[get, set]` |
| | Gets or sets the best bid before the trade. |
| | |
| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) | [NewAsk](./classATAS_1_1Indicators_1_1CumulativeTrade.md#aa9193751b8e35052bf4101971453b6f8) = null!`[get, set]` |
| | Gets or sets the best ask after the trade. |
| | |
| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) | [NewBid](./classATAS_1_1Indicators_1_1CumulativeTrade.md#a19aba7f38087ddaef5256a161f40dcfc) = null!`[get, set]` |
| | Gets or sets the best bid after the trade. |
| | |

## Detailed Description

Represents a cumulative trade, which is a trade that includes multiple prints or executions.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.Indicators.CumulativeTrade.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string representation of the CumulativeTrade object.

ReturnsA string containing the trade information.

## Property Documentation

## [◆](https://docs.atas.net/en/)Direction

| [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) ATAS.Indicators.CumulativeTrade.Direction |
| --- |

getset

Gets or sets the trade direction (Buy or Sell).

## [◆](https://docs.atas.net/en/)FirstPrice

| decimal ATAS.Indicators.CumulativeTrade.FirstPrice |
| --- |

getset

Gets or sets the first price of the trade.

## [◆](https://docs.atas.net/en/)Lastprice

| decimal ATAS.Indicators.CumulativeTrade.Lastprice |
| --- |

getset

Gets or sets the last price of the trade.

## [◆](https://docs.atas.net/en/)NewAsk

| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) ATAS.Indicators.CumulativeTrade.NewAsk = null! |
| --- |

getset

Gets or sets the best ask after the trade.

## [◆](https://docs.atas.net/en/)NewBid

| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) ATAS.Indicators.CumulativeTrade.NewBid = null! |
| --- |

getset

Gets or sets the best bid after the trade.

## [◆](https://docs.atas.net/en/)PreviousAsk

| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) ATAS.Indicators.CumulativeTrade.PreviousAsk = null! |
| --- |

getset

Gets or sets the best ask before the trade.

## [◆](https://docs.atas.net/en/)PreviousBid

| [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) ATAS.Indicators.CumulativeTrade.PreviousBid = null! |
| --- |

getset

Gets or sets the best bid before the trade.

## [◆](https://docs.atas.net/en/)Ticks

| List ATAS.Indicators.CumulativeTrade.Ticks = new() |
| --- |

getset

Gets or sets the list of individual ticks (MarketDataArg) included in the cumulative trade.

## [◆](https://docs.atas.net/en/)Time

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.CumulativeTrade.Time |
| --- |

getset

Gets or sets the time of the trade.

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.Indicators.CumulativeTrade.Volume |
| --- |

getset

Gets or sets the cumulative volume of the trade.

The documentation for this class was generated from the following file:
- [CumulativeTrade.cs](../files/CumulativeTrade_8cs.md)
