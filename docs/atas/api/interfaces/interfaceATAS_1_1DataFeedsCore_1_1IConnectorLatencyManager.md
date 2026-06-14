# ATAS.DataFeedsCore.IConnectorLatencyManager Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.html

Inheritance diagram for ATAS.DataFeedsCore.IConnectorLatencyManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| TimeSpan? | [OrdersLatency](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a89fbe376b0dc6c2a759bf07eeb360ecb)`[get]` |
| | Orders processing delay time. |
| | |
| TimeSpan? | [MarketDataLatency](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a9129ff8acffdb87153c17e5190f32ca6)`[get]` |
| | Market data processing delay time. |
| | |
| DateTime? | [LastMarketDataReceptionTimeUtc](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a1c01331161dcda34965d23c038d225fd)`[get]` |
| | Last market data update time in UTC. |
| | |
| TimeSpan? | [TimeSinceLastMarketDataReceived](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a4bb1798258a6a4ec7e22dbbd276f1c15)`[get]` |
| | Time elapsed since the last market data update. |
| | |

| Events | |
| --- | --- |
| Action? | [OrdersLatencyChanged](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#ad130a9b425ded2652459e94a98f7aa70) |
| | Event raised when OrdersLatency value changes. |
| | |
| Action? | [MarketDataLatencyChanged](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a1e5caaf4bb334082dcb8f176dd6913ab) |
| | Event raised when MarketDataLatency value changes. |
| | |
| Action? | [LastMarketDataReceptionTimeChanged](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md#a82481e628ecc797ce2eefa66d11c972d) |
| | Event raised when TimeSinceLastMarketDataReceived value changes. |
| | |

## Property Documentation

## [◆](https://docs.atas.net/en/)LastMarketDataReceptionTimeUtc

| DateTime? ATAS.DataFeedsCore.IConnectorLatencyManager.LastMarketDataReceptionTimeUtc |
| --- |

get

Last market data update time in UTC.

Implemented in [ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ad3346362263488ed01f03fae770b259e).

## [◆](https://docs.atas.net/en/)MarketDataLatency

| TimeSpan? ATAS.DataFeedsCore.IConnectorLatencyManager.MarketDataLatency |
| --- |

get

Market data processing delay time.

Implemented in [ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ab638989c07fbb6bd9c3bfd7c068369e0).

## [◆](https://docs.atas.net/en/)OrdersLatency

| TimeSpan? ATAS.DataFeedsCore.IConnectorLatencyManager.OrdersLatency |
| --- |

get

Orders processing delay time.

Implemented in [ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ad2091f737382063833f395c54d899172).

## [◆](https://docs.atas.net/en/)TimeSinceLastMarketDataReceived

| TimeSpan? ATAS.DataFeedsCore.IConnectorLatencyManager.TimeSinceLastMarketDataReceived |
| --- |

get

Time elapsed since the last market data update.

Implemented in [ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#af79bc85bc15570502af287f720e5a6ed).

## Event Documentation

## [◆](https://docs.atas.net/en/)LastMarketDataReceptionTimeChanged

| Action? ATAS.DataFeedsCore.IConnectorLatencyManager.LastMarketDataReceptionTimeChanged |
| --- |

Event raised when TimeSinceLastMarketDataReceived value changes.

## [◆](https://docs.atas.net/en/)MarketDataLatencyChanged

| Action? ATAS.DataFeedsCore.IConnectorLatencyManager.MarketDataLatencyChanged |
| --- |

Event raised when MarketDataLatency value changes.

Implemented in [ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#a0084a8948eb5fba36d44c8dccc4ef71e).

## [◆](https://docs.atas.net/en/)OrdersLatencyChanged

| Action? ATAS.DataFeedsCore.IConnectorLatencyManager.OrdersLatencyChanged |
| --- |

Event raised when OrdersLatency value changes.

Implemented in [ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md#ab51e19fc1a352ef3dc82cec770b2152d).

The documentation for this interface was generated from the following file:
- [ConnectorLatencyManager.cs](../files/ConnectorLatencyManager_8cs.md)
