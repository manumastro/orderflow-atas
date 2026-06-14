# ATAS.DataFeedsCore.TradeStatistics.Matching Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.html

| Classes | |
| --- | --- |
| class | HistoryMatchingProcessor |
| | |
| class | HistoryMyTradeExtensions |
| | |
| class | HistoryTradesMatcher |
| | |
| class | RealtimeMatchingProcessor |
| | |
| class | RealtimeTradesMatcher |
| | |
| class | [TradesMatchingProcessor](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md) |
| | |
| class | TradesMatchingProduct |
| | |
| class | TradesPositionDisbalance |
| | |
| class | TradesPositionsBalancer |
| | |
| class | TradesProcessingExtensions |
| | |
| class | [TradesProcessingLoggerSource](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md) |
| | |
| class | [TradesProcessingUnit](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md) |
| | |

| Typedefs | |
| --- | --- |
| using | [OpenTradeByVolume](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a7bc05a8e3f567fee0d965e9dc6c967f2) = ([MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) [Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md), decimal OpenVolume) |
| | |
| using | [OpenTradesData](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a62841c58799f41929103d79ac841d04b) = (System.Collections.Generic.IReadOnlyCollection Trades, decimal LostOpenVolume) |
| | |
| using | [PortfolioSecurity](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) = ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md), [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md)) |
| | |
| using | [PortfolioSecurityKey](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a8f3459470ea3d340cdd7a8c525dfb577) = (string AccountId, string SecurityId) |
| | |

| Functions | |
| --- | --- |
| record struct | [PnlData](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#ac8df9ea4e174dca4efe6aec043ea8255) (decimal Pnl, decimal TicksPnl, decimal PricePnl) |
| | |
| delegate Task > | [HistoryProvider](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#aa8d369a803b8f7a457e7c0ca012096fb) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime from, DateTime to) |
| | |

## Typedef Documentation

## [◆](https://docs.atas.net/en/)OpenTradeByVolume

| using [ATAS.DataFeedsCore.TradeStatistics.Matching.OpenTradeByVolume](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a7bc05a8e3f567fee0d965e9dc6c967f2) = typedef ([MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) [Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md), decimal OpenVolume) |
| --- |

## [◆](https://docs.atas.net/en/)OpenTradesData

| using [ATAS.DataFeedsCore.TradeStatistics.Matching.OpenTradesData](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a62841c58799f41929103d79ac841d04b) = typedef (System.Collections.Generic.IReadOnlyCollection Trades, decimal LostOpenVolume) |
| --- |

## [◆](https://docs.atas.net/en/)PortfolioSecurity

| using [ATAS.DataFeedsCore.TradeStatistics.Matching.PortfolioSecurity](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) = typedef ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md), [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md)) |
| --- |

## [◆](https://docs.atas.net/en/)PortfolioSecurityKey

| using [ATAS.DataFeedsCore.TradeStatistics.Matching.PortfolioSecurityKey](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a8f3459470ea3d340cdd7a8c525dfb577) = typedef (string AccountId, string SecurityId) |
| --- |

## Function Documentation

## [◆](https://docs.atas.net/en/)HistoryProvider()

| delegate Task > ATAS.DataFeedsCore.TradeStatistics.Matching.HistoryProvider | ( | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | DateTime | from, |
| | | DateTime | to |
| | ) | | |

## [◆](https://docs.atas.net/en/)PnlData()

| record struct ATAS.DataFeedsCore.TradeStatistics.Matching.PnlData | ( | decimal | Pnl, |
| --- | --- | --- | --- |
| | | decimal | TicksPnl, |
| | | decimal | PricePnl |
| | ) | | |
