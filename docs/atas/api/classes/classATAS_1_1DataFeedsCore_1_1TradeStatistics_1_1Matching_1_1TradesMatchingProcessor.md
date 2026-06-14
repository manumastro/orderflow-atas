# ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.html

Inheritance diagram for ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [TradesMatchingProcessor](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a0afe0e196fe7a687745639bb5e9a18f2) ([HistoryProvider](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#aa8d369a803b8f7a457e7c0ca012096fb) getHistory, [PortfolioSecurity](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) portfolioSecurity, [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md)? connector=null, IReadOnlyCollection? unprocessedTrades=null, decimal lastKnownPos=default, bool forceRecovery=false, IProgress? progressCounter=null, TimeProvider? timeProvider=null) |
| | |
| override int | [GetHashCode](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a14e8786634b5018b22e85a77610c8837) () |
| | |
| override bool | [Equals](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#afe78528d4b6cdff8095e13fe4d942346) (object? obj) |
| | |
| void | [Start](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a9a6c4344d7b9ad12c18e09106bd6e650) () |
| | |
| void | [Process](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a6d4dcbc19bdfe5dfb97ab10011dd8996) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [Process](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#aea61525f92cce8a8836c762bf6327ed3) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| override void | [Update](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a90e9f71ea6ea32e5bd8feb92c19587cc) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| override void | [Update](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#ac3cab908c5d380599cb19b8f878464a3) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md) | |
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
| override void | [OnDispose](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#aa5822b8357e543d9bb688ace71146339) () |
| | |
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

| Properties | |
| --- | --- |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md)? | [Connector](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a2216aefde5a2010a6c13c3625745eff4)`[get, set]` |
| | |
| TimeSpan? | [HistoryReceptionCompletionPeriod](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a1c483cf09fb93ed0af56aa30a5beb1bc)`[get, set]` |
| | |
| TimeSpan | [TradesPositionsSyncTimeout](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a4bbbe0160328be232ca58caf5df50753)`[get, set]` |
| | |
| Action? | [Balanced](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a22878c4b448c747fcd5e5b53b91c969e) |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md) | |
| bool | [Disposed](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#ac996041d0e7956229e573fc7df687e09)`[get]` |
| | |
| [PortfolioSecurity](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) | [PortfolioSecurity](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a7d964c2620543c0b3978e45aa5343aee)`[get]` |
| | |
| [PortfolioSecurityKey](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a8f3459470ea3d340cdd7a8c525dfb577) | [PortfolioSecurityKey](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#adb62826fbec8f99ccfb0cd87971882de)`[get]` |
| | |

| Events | |
| --- | --- |
| Action? | [NewTrade](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#ae097cafa735a952974f1c131c2d46e41) |
| | |
| Action? | [HistoryCalculationCompleted](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#a510400599d14a4f3e00af21127d74354) |
| | |
| Action? | [PendingData](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md#af9eb4bdb2e020a28a55c21dcde803006) |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Protected Attributes inherited from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md) | |
| readonly object | [_sync](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a805cdd8284b6deb9f23ca2e8cdbb20ca) = new() |
| | |
| - Protected Attributes inherited from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingLoggerSource](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md) | |
| string? | [_prefix](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md#aa4e7622c4681a5cbdcc790304156be9b) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)TradesMatchingProcessor()

| ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.TradesMatchingProcessor | ( | [HistoryProvider](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#aa8d369a803b8f7a457e7c0ca012096fb) | getHistory, |
| --- | --- | --- | --- |
| | | [PortfolioSecurity](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md#a87019b1c2a1912959b6512589e3ce5b6) | portfolioSecurity, |
| | | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md)? | connector = `null`, |
| | | IReadOnlyCollection? | unprocessedTrades = `null`, |
| | | decimal | lastKnownPos = `default`, |
| | | bool | forceRecovery = `false`, |
| | | IProgress? | progressCounter = `null`, |
| | | TimeProvider? | timeProvider = `null` |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Equals()

| override bool ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Equals | ( | object? | obj | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetHashCode()

| override int ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.GetHashCode | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnDispose()

| override void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.OnDispose | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a51ec8cdceb7114563b5d8e080f2a1316).

## [◆](https://docs.atas.net/en/)Process() [1/2]

| void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Process | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Process() [2/2]

| void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Process | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Start()

| void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Start | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Update() [1/2]

| override void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Update | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#a2f714caf322212b8dfbeab2d1dd08db3).

## [◆](https://docs.atas.net/en/)Update() [2/2]

| override void ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Update | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](./classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md#ad4022d360398a7bb19f83de0b36917e6).

## Property Documentation

## [◆](https://docs.atas.net/en/)Balanced

| Action? ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Balanced |
| --- |

addremove

## [◆](https://docs.atas.net/en/)Connector

| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md)? ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.Connector |
| --- |

getset

## [◆](https://docs.atas.net/en/)HistoryReceptionCompletionPeriod

| TimeSpan? ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.HistoryReceptionCompletionPeriod |
| --- |

getset

## [◆](https://docs.atas.net/en/)TradesPositionsSyncTimeout

| TimeSpan ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.TradesPositionsSyncTimeout |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)HistoryCalculationCompleted

| Action? ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.HistoryCalculationCompleted |
| --- |

## [◆](https://docs.atas.net/en/)NewTrade

| Action? ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.NewTrade |
| --- |

## [◆](https://docs.atas.net/en/)PendingData

| Action? ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor.PendingData |
| --- |

The documentation for this class was generated from the following file:
- [TradesMatchingProcessor.cs](../files/TradesMatchingProcessor_8cs.md)
