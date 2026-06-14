# ATAS.DataFeedsCore.SecuritySummary Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1SecuritySummary.html

| Public Member Functions | |
| --- | --- |
| | [SecuritySummary](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a8df2ba7f490717869fbaf9395be3f0f7) () |
| | Initializes a new instance of the SecuritySummary class with default values. |
| | |
| | [SecuritySummary](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a2b4a0f2242039f83a20311b55cdf1458) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | Initializes a new instance of the SecuritySummary class with default values. |
| | |

| Properties | |
| --- | --- |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#aca3ef5e94f8c85f62a1f8f0b3f2ee9a2)`[get]` |
| | Gets or sets the security. |
| | |
| decimal? | [BestAskPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a13cfdb77a12e6b66dc682667cca01e56)`[get, set]` |
| | Gets or sets the best asking price for the security. |
| | |
| decimal? | [BestAskVolume](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#aa613a35591a8f30433e69848d3c9f73e)`[get, set]` |
| | Gets or sets the volume available at the best asking price for the security. |
| | |
| decimal? | [BestBidPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a1f53c9444dd62bd1331d7805f9e253f7)`[get, set]` |
| | Gets or sets the best bidding price for the security. |
| | |
| decimal? | [BestBidVolume](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#afe040a2d4c56a7412704fd1bd3f49728)`[get, set]` |
| | Gets or sets the volume available at the best bidding price for the security. |
| | |
| decimal? | [LastTradePrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a45f195459cebb000e2ba5ffdbd86f4a5)`[get, set]` |
| | Price of the last tick
 This value may be null if no ticks were received yet. |
| | |
| decimal? | [LastTradeVolume](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#ab09c9b0c9dc7523329df15e594242b55)`[get, set]` |
| | Volume of the last tick
 This value may be null if no ticks were received yet. |
| | |
| decimal? | [SettlementPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#aafd7ff9ac3889ebd5cf618c5caff0358)`[get, set]` |
| | Gets or sets the settlement price value for the security. |
| | |
| decimal? | [OpenInterest](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#adaee83507dde600965e91f995373e7b9)`[get, set]` |
| | Gets or sets the open interest value for the security. |
| | |
| decimal? | [CurrentDayOpenPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a1429ad634557a01e236c9c83c0f6aed2)`[get, set]` |
| | Gets or sets the current day open price value for the security. |
| | |
| decimal? | [CurrentDayHighPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a443d5c1ae9336dfa3d0abac274a6887c)`[get, set]` |
| | Gets or sets the current day high price value for the security. |
| | |
| decimal? | [CurrentDayLowPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#ab671e402b71f711a5916372331225ee9)`[get, set]` |
| | Gets or sets the current day low price value for the security. |
| | |
| decimal? | [CurrentDayTotalVolume](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a59279a1cb2b6b652785f8934e62febf3)`[get, set]` |
| | Gets or sets the today trade volume value for the security. |
| | |
| decimal? | [CurrentDayTurnover](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#abb28c3fa7e33db01a042d5dd12923a8c)`[get, set]` |
| | Gets or sets the today turnover for the security. |
| | |
| decimal? | [PrevDayClosePrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#af5f44a7acfe6bdd55a0cdab07974b47e)`[get, set]` |
| | Gets or sets the previous day close price value for the security. |
| | |
| decimal? | [PrevDayTotalVolume](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a26550948381e545780ccd285e2963e97)`[get, set]` |
| | Gets or sets the previous day trade volume value for the security. |
| | |
| decimal? | [Last24OpenPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a28e6ce29cbaa3cfc8b99e510ad9365f0)`[get, set]` |
| | Gets or sets the 24H open price value for the security. |
| | |
| decimal? | [Last24HighPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#aec013116ea863f19a034f9eb76f79d54)`[get, set]` |
| | Gets or sets the 24H high price value for the security. |
| | |
| decimal? | [Last24LowPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a14c4af01d893d60ea17e9b797458fef3)`[get, set]` |
| | Gets or sets the 24H low price value for the security. |
| | |
| decimal? | [Last24TotalVolume](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a4d351a0e0efd709f5725b8fd92bd4a75)`[get, set]` |
| | Gets or sets the 24H trade volume value for the security. |
| | |
| decimal? | [Last24Turnover](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a86c23f2075db5cabb8c2c8f353d5c01a)`[get, set]` |
| | Gets or sets the 24H turnover for the security. |
| | |
| decimal? | [MarkPrice](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a6f0efc4e3c17cbcf64ac7cefb193f5fd)`[get, set]` |
| | A price that reflects the real-time spot price on the major exchanges. |
| | |
| decimal? | [FundingRate](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#ad7e41da7711cbbfd515f97ccb4e147f7)`[get, set]` |
| | Gets funding rate exchanged between buyers and sellers. During the funding rate cycle. |
| | |
| DateTimeOffset? | [NextFundingTime](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md#a2c32868c151984d65bba5d30b6fc919c)`[get, set]` |
| | Gets time of the next funding cycle. |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SecuritySummary() [1/2]

| ATAS.DataFeedsCore.SecuritySummary.SecuritySummary | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the SecuritySummary class with default values.

## [◆](https://docs.atas.net/en/)SecuritySummary() [2/2]

| ATAS.DataFeedsCore.SecuritySummary.SecuritySummary | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the SecuritySummary class with default values.

## Property Documentation

## [◆](https://docs.atas.net/en/)BestAskPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.BestAskPrice |
| --- |

getset

Gets or sets the best asking price for the security.

## [◆](https://docs.atas.net/en/)BestAskVolume

| decimal? ATAS.DataFeedsCore.SecuritySummary.BestAskVolume |
| --- |

getset

Gets or sets the volume available at the best asking price for the security.

## [◆](https://docs.atas.net/en/)BestBidPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.BestBidPrice |
| --- |

getset

Gets or sets the best bidding price for the security.

## [◆](https://docs.atas.net/en/)BestBidVolume

| decimal? ATAS.DataFeedsCore.SecuritySummary.BestBidVolume |
| --- |

getset

Gets or sets the volume available at the best bidding price for the security.

## [◆](https://docs.atas.net/en/)CurrentDayHighPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.CurrentDayHighPrice |
| --- |

getset

Gets or sets the current day high price value for the security.

## [◆](https://docs.atas.net/en/)CurrentDayLowPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.CurrentDayLowPrice |
| --- |

getset

Gets or sets the current day low price value for the security.

## [◆](https://docs.atas.net/en/)CurrentDayOpenPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.CurrentDayOpenPrice |
| --- |

getset

Gets or sets the current day open price value for the security.

## [◆](https://docs.atas.net/en/)CurrentDayTotalVolume

| decimal? ATAS.DataFeedsCore.SecuritySummary.CurrentDayTotalVolume |
| --- |

getset

Gets or sets the today trade volume value for the security.

## [◆](https://docs.atas.net/en/)CurrentDayTurnover

| decimal? ATAS.DataFeedsCore.SecuritySummary.CurrentDayTurnover |
| --- |

getset

Gets or sets the today turnover for the security.

## [◆](https://docs.atas.net/en/)FundingRate

| decimal? ATAS.DataFeedsCore.SecuritySummary.FundingRate |
| --- |

getset

Gets funding rate exchanged between buyers and sellers. During the funding rate cycle.

## [◆](https://docs.atas.net/en/)Last24HighPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.Last24HighPrice |
| --- |

getset

Gets or sets the 24H high price value for the security.

## [◆](https://docs.atas.net/en/)Last24LowPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.Last24LowPrice |
| --- |

getset

Gets or sets the 24H low price value for the security.

## [◆](https://docs.atas.net/en/)Last24OpenPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.Last24OpenPrice |
| --- |

getset

Gets or sets the 24H open price value for the security.

## [◆](https://docs.atas.net/en/)Last24TotalVolume

| decimal? ATAS.DataFeedsCore.SecuritySummary.Last24TotalVolume |
| --- |

getset

Gets or sets the 24H trade volume value for the security.

## [◆](https://docs.atas.net/en/)Last24Turnover

| decimal? ATAS.DataFeedsCore.SecuritySummary.Last24Turnover |
| --- |

getset

Gets or sets the 24H turnover for the security.

## [◆](https://docs.atas.net/en/)LastTradePrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.LastTradePrice |
| --- |

getset

Price of the last tick

 This value may be null if no ticks were received yet.

## [◆](https://docs.atas.net/en/)LastTradeVolume

| decimal? ATAS.DataFeedsCore.SecuritySummary.LastTradeVolume |
| --- |

getset

Volume of the last tick

 This value may be null if no ticks were received yet.

## [◆](https://docs.atas.net/en/)MarkPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.MarkPrice |
| --- |

getset

A price that reflects the real-time spot price on the major exchanges.

## [◆](https://docs.atas.net/en/)NextFundingTime

| DateTimeOffset? ATAS.DataFeedsCore.SecuritySummary.NextFundingTime |
| --- |

getset

Gets time of the next funding cycle.

## [◆](https://docs.atas.net/en/)OpenInterest

| decimal? ATAS.DataFeedsCore.SecuritySummary.OpenInterest |
| --- |

getset

Gets or sets the open interest value for the security.

## [◆](https://docs.atas.net/en/)PrevDayClosePrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.PrevDayClosePrice |
| --- |

getset

Gets or sets the previous day close price value for the security.

## [◆](https://docs.atas.net/en/)PrevDayTotalVolume

| decimal? ATAS.DataFeedsCore.SecuritySummary.PrevDayTotalVolume |
| --- |

getset

Gets or sets the previous day trade volume value for the security.

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.SecuritySummary.Security |
| --- |

get

Gets or sets the security.

## [◆](https://docs.atas.net/en/)SettlementPrice

| decimal? ATAS.DataFeedsCore.SecuritySummary.SettlementPrice |
| --- |

getset

Gets or sets the settlement price value for the security.

The documentation for this class was generated from the following file:
- [Security.cs](../files/Security_8cs.md)
