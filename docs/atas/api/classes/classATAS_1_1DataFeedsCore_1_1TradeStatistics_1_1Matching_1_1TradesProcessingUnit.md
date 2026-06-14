# ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.html

Inheritance diagram for ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [TradesProcessingUnit](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a4b2ad10d8ee6210c9f246b59d5d3f157) ([PortfolioSecurity](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) portfolioSecurity) |
| | |
| virtual void | [Update](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a2f714caf322212b8dfbeab2d1dd08db3) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| virtual void | [Update](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#ad4022d360398a7bb19f83de0b36917e6) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| void | [Dispose](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a1267db48137d7c0d3664afd2dd90627b) () |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnDispose](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a51ec8cdceb7114563b5d8e080f2a1316) () |
| | |
| - Protected Member Functions inherited from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingLoggerSource](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md) | |
| void | [LogDebug](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md#a938be9c4e84da90ff675572daf581b5f) (string message, params object[] args) |
| | |
| void | [LogInfo](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md#a564a0b4551a474fa56248e866e26222a) (string message, params object[] args) |
| | |
| void | [LogWarn](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md#adb0fb06c3fcdd6005a326862dade8e54) (string message, params object[] args) |
| | |
| void | [LogError](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md#a07a6b4b87c3724a2bb3f4832e6caab63) (string message, Exception e) |
| | |

| Protected Attributes | |
| --- | --- |
| readonly object | [_sync](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a805cdd8284b6deb9f23ca2e8cdbb20ca) = new() |
| | |
| - Protected Attributes inherited from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingLoggerSource](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md) | |
| string? | [_prefix](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md#aa4e7622c4681a5cbdcc790304156be9b) |
| | |

| Properties | |
| --- | --- |
| bool | [Disposed](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#ac996041d0e7956229e573fc7df687e09)`[get]` |
| | |
| [PortfolioSecurity](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) | [PortfolioSecurity](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a7d964c2620543c0b3978e45aa5343aee)`[get]` |
| | |
| [PortfolioSecurityKey](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a8f3459470ea3d340cdd7a8c525dfb577) | [PortfolioSecurityKey](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#adb62826fbec8f99ccfb0cd87971882de)`[get]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)TradesProcessingUnit()

| ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.TradesProcessingUnit | ( | [PortfolioSecurity](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) | portfolioSecurity | ) | |
| --- | --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Dispose()

| void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.Dispose | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnDispose()

| virtual void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.OnDispose | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented in [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#aa5822b8357e543d9bb688ace71146339).

## [◆](https://docs.atas.net/en/)Update() [1/2]

| virtual void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.Update | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Reimplemented in [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a90e9f71ea6ea32e5bd8feb92c19587cc).

## [◆](https://docs.atas.net/en/)Update() [2/2]

| virtual void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.Update | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Reimplemented in [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#ac3cab908c5d380599cb19b8f878464a3).

## Member Data Documentation

## [◆](https://docs.atas.net/en/)_sync

| readonly object ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit._sync = new() |
| --- |

protected

## Property Documentation

## [◆](https://docs.atas.net/en/)Disposed

| bool ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.Disposed |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)PortfolioSecurity

| [PortfolioSecurity](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.PortfolioSecurity |
| --- |

get

## [◆](https://docs.atas.net/en/)PortfolioSecurityKey

| [PortfolioSecurityKey](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a8f3459470ea3d340cdd7a8c525dfb577) ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit.PortfolioSecurityKey |
| --- |

get

The documentation for this class was generated from the following file:
- [TradesProcessingUnit.cs](../files/TradesProcessingUnit_8cs.md)
