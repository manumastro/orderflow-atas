# ATAS.DataFeedsCore.AsyncMessageQueue< TMessage > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.html

Inheritance diagram for ATAS.DataFeedsCore.AsyncMessageQueue< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.AsyncMessageQueue< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a809d3b457fc410ee956b9ff87c8101fd) () |
| | |
| | [AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a652023184b5b92384d05b78c7f471d3a) (AsyncOneThreadProcessor processor) |
| | |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a03b99ab5cc545630d7738f6bb4e21343) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, TMessage message) |
| | |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a8b4ef3b8870cdefcd2e1e3aeb2ed26b3) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action action) |
| | |
| void | [Start](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#ab21f0b2268f34176d601516475ad2240) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| void | [Stop](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#ae40fd8b4ab98c619cf001fc6e126ef4d) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |
| void | [Enqueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a700d549bbcebbf0ecf97b229a33a2d48) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, TMessage message) |
| | |
| void | [Enqueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#aba9326ea042897bb29a0bdc44427b2dd) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action action) |
| | |
| void | [Start](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#ad50e20850db3ea2b9391c4ea3618edc8) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| void | [Stop](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a742273e3891cbbfe992012887fff45d5) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |

| Properties | |
| --- | --- |
| TimeSpan | [HeartbeatTimeout](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a2f6d4b11673c4affe26b2c876d14cad2)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) | |
| TimeSpan | [HeartbeatTimeout](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a021e0986104e7481cd81459ce0c225f6)`[get, set]` |
| | |

| Events | |
| --- | --- |
| Action? | [Heartbeat](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#aea739572553399d80fb0fdf11a53c9cb) |
| | |
| - Events inherited from [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) | |
| Action | [Heartbeat](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a301e65bd667639da3e6330a54c37d79b) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)AsyncMessageQueue() [1/2]

| [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).[AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)AsyncMessageQueue() [2/2]

| [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).[AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md) | ( | AsyncOneThreadProcessor | processor | ) | |
| --- | --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Enqueue() [1/2]

| void [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).Enqueue | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | action |
| | ) | | |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#aba9326ea042897bb29a0bdc44427b2dd).

## [◆](https://docs.atas.net/en/)Enqueue() [2/2]

| void [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).Enqueue | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | TMessage | message |
| | ) | | |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a700d549bbcebbf0ecf97b229a33a2d48).

## [◆](https://docs.atas.net/en/)Start()

| void [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).Start | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | handler |
| | ) | | |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#ad50e20850db3ea2b9391c4ea3618edc8).

## [◆](https://docs.atas.net/en/)Stop()

| void [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).Stop | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a742273e3891cbbfe992012887fff45d5).

## Property Documentation

## [◆](https://docs.atas.net/en/)HeartbeatTimeout

| TimeSpan [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).HeartbeatTimeout |
| --- |

getset

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a021e0986104e7481cd81459ce0c225f6).

## Event Documentation

## [◆](https://docs.atas.net/en/)Heartbeat

| Action? [ATAS.DataFeedsCore.AsyncMessageQueue](./classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md).Heartbeat |
| --- |

The documentation for this class was generated from the following file:
- [IMessageQueue.cs](../files/IMessageQueue_8cs.md)
