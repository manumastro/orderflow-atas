# ATAS.DataFeedsCore.ConnectorLatencyManager Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.html

Inheritance diagram for ATAS.DataFeedsCore.ConnectorLatencyManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.ConnectorLatencyManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [ConnectorLatencyManager](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a7f813cdfa7c2d4469d83a0c7de92c7ce) () |
| | |
| void | [ProcessTrade](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a3fdaeba675f2f747d836393717fa3b48) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime time) |
| | |
| void | [ProcessBestBidAsk](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a1291e7ed18304e14b13605feb12bc294) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime time) |
| | |
| void | [ProcessMarketDepths](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#af87c3810a2408cfa3746ab3d8afd5882) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime time) |
| | |
| TimeSpan | [ProcessOrderLatency](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a51b13aa7cc7e0ec4af414bb7de53c9ba) (DateTime startTime) |
| | |
| void | [Reset](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#aa7fcdc85b020f398d330bb6c356a3a84) () |
| | |
| void | [ProcessTickTime](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a2e3132c3dc9d248a5822e1f5d9c4bb54) (DateTime time) |
| | |
| void | [ProcessMarketDepthTime](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a6161dd50edc6e15e812a3f0afde10d1e) (DateTime time) |
| | |

| Properties | |
| --- | --- |
| TimeSpan | [FeedDelay](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ad7a099a2e593fd5f4b4236cba65ff030)`[get, set]` |
| | |
| ITimeSyncManager | [TimeSyncManager](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a281ecaaae3d1bb8df8f33a507edf03ed)`[get, set]` |
| | |
| TimeSpan? | [OrdersLatency](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ad2091f737382063833f395c54d899172)`[get]` |
| | Orders processing delay time. |
| | |
| TimeSpan? | [MarketDataLatency](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ab638989c07fbb6bd9c3bfd7c068369e0)`[get]` |
| | Market data processing delay time. |
| | |
| DateTime? | [LastMarketDataReceptionTimeUtc](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ad3346362263488ed01f03fae770b259e)`[get]` |
| | Last market data update time in UTC. |
| | |
| TimeSpan? | [TimeSinceLastMarketDataReceived](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#af79bc85bc15570502af287f720e5a6ed)`[get]` |
| | Time elapsed since the last market data update. |
| | |
| Action? | [OrdersLatencyChanged](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ab51e19fc1a352ef3dc82cec770b2152d) |
| | Event raised when OrdersLatency value changes. |
| | |
| Action? | [MarketDataLatencyChanged](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a0084a8948eb5fba36d44c8dccc4ef71e) |
| | Event raised when MarketDataLatency value changes. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | |
| TimeSpan? | [OrdersLatency](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a89fbe376b0dc6c2a759bf07eeb360ecb)`[get]` |
| | Orders processing delay time. |
| | |
| TimeSpan? | [MarketDataLatency](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a9129ff8acffdb87153c17e5190f32ca6)`[get]` |
| | Market data processing delay time. |
| | |
| DateTime? | [LastMarketDataReceptionTimeUtc](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a1c01331161dcda34965d23c038d225fd)`[get]` |
| | Last market data update time in UTC. |
| | |
| TimeSpan? | [TimeSinceLastMarketDataReceived](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a4bb1798258a6a4ec7e22dbbd276f1c15)`[get]` |
| | Time elapsed since the last market data update. |
| | |

| Events | |
| --- | --- |
| Action? | [LastMarketDataReceptionTimeChanged](./classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a33ff9edaab57f4fb71ca2c62c6106527) |
| | Event raised when TimeSinceLastMarketDataReceived value changes. |
| | |
| - Events inherited from [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | |
| Action? | [OrdersLatencyChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#ad130a9b425ded2652459e94a98f7aa70) |
| | Event raised when OrdersLatency value changes. |
| | |
| Action? | [MarketDataLatencyChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a1e5caaf4bb334082dcb8f176dd6913ab) |
| | Event raised when MarketDataLatency value changes. |
| | |
| Action? | [LastMarketDataReceptionTimeChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a82481e628ecc797ce2eefa66d11c972d) |
| | Event raised when TimeSinceLastMarketDataReceived value changes. |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ConnectorLatencyManager()

| ATAS.DataFeedsCore.ConnectorLatencyManager.ConnectorLatencyManager | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ProcessBestBidAsk()

| void ATAS.DataFeedsCore.ConnectorLatencyManager.ProcessBestBidAsk | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | DateTime | time |
| | ) | | |

## [◆](https://docs.atas.net/en/)ProcessMarketDepths()

| void ATAS.DataFeedsCore.ConnectorLatencyManager.ProcessMarketDepths | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | DateTime | time |
| | ) | | |

## [◆](https://docs.atas.net/en/)ProcessMarketDepthTime()

| void ATAS.DataFeedsCore.ConnectorLatencyManager.ProcessMarketDepthTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ProcessOrderLatency()

| TimeSpan ATAS.DataFeedsCore.ConnectorLatencyManager.ProcessOrderLatency | ( | DateTime | startTime | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ProcessTickTime()

| void ATAS.DataFeedsCore.ConnectorLatencyManager.ProcessTickTime | ( | DateTime | time | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ProcessTrade()

| void ATAS.DataFeedsCore.ConnectorLatencyManager.ProcessTrade | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | DateTime | time |
| | ) | | |

## [◆](https://docs.atas.net/en/)Reset()

| void ATAS.DataFeedsCore.ConnectorLatencyManager.Reset | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)FeedDelay

| TimeSpan ATAS.DataFeedsCore.ConnectorLatencyManager.FeedDelay |
| --- |

getset

## [◆](https://docs.atas.net/en/)LastMarketDataReceptionTimeUtc

| DateTime? ATAS.DataFeedsCore.ConnectorLatencyManager.LastMarketDataReceptionTimeUtc |
| --- |

get

Last market data update time in UTC.

Implements [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a1c01331161dcda34965d23c038d225fd).

## [◆](https://docs.atas.net/en/)MarketDataLatency

| TimeSpan? ATAS.DataFeedsCore.ConnectorLatencyManager.MarketDataLatency |
| --- |

get

Market data processing delay time.

Implements [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a9129ff8acffdb87153c17e5190f32ca6).

## [◆](https://docs.atas.net/en/)MarketDataLatencyChanged

| Action? ATAS.DataFeedsCore.ConnectorLatencyManager.MarketDataLatencyChanged |
| --- |

addremove

Event raised when MarketDataLatency value changes.

Implements [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a1e5caaf4bb334082dcb8f176dd6913ab).

## [◆](https://docs.atas.net/en/)OrdersLatency

| TimeSpan? ATAS.DataFeedsCore.ConnectorLatencyManager.OrdersLatency |
| --- |

get

Orders processing delay time.

Implements [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a89fbe376b0dc6c2a759bf07eeb360ecb).

## [◆](https://docs.atas.net/en/)OrdersLatencyChanged

| Action? ATAS.DataFeedsCore.ConnectorLatencyManager.OrdersLatencyChanged |
| --- |

addremove

Event raised when OrdersLatency value changes.

Implements [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#ad130a9b425ded2652459e94a98f7aa70).

## [◆](https://docs.atas.net/en/)TimeSinceLastMarketDataReceived

| TimeSpan? ATAS.DataFeedsCore.ConnectorLatencyManager.TimeSinceLastMarketDataReceived |
| --- |

get

Time elapsed since the last market data update.

Implements [ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a4bb1798258a6a4ec7e22dbbd276f1c15).

## [◆](https://docs.atas.net/en/)TimeSyncManager

| ITimeSyncManager ATAS.DataFeedsCore.ConnectorLatencyManager.TimeSyncManager |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)LastMarketDataReceptionTimeChanged

| Action? ATAS.DataFeedsCore.ConnectorLatencyManager.LastMarketDataReceptionTimeChanged |
| --- |

Event raised when TimeSinceLastMarketDataReceived value changes.

The documentation for this class was generated from the following file:
- [ConnectorLatencyManager.cs](../files/ConnectorLatencyManager_8cs.md)
