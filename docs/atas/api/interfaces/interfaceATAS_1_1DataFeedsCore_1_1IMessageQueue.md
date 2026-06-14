# ATAS.DataFeedsCore.IMessageQueue< TMessage > Interface Template Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.html

Inheritance diagram for ATAS.DataFeedsCore.IMessageQueue< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.IMessageQueue< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Enqueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a700d549bbcebbf0ecf97b229a33a2d48) ([IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, TMessage message) |
| | |
| void | [Enqueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#aba9326ea042897bb29a0bdc44427b2dd) ([IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action action) |
| | |
| void | [Start](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#ad50e20850db3ea2b9391c4ea3618edc8) ([IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| void | [Stop](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a742273e3891cbbfe992012887fff45d5) ([IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |

| Properties | |
| --- | --- |
| TimeSpan | [HeartbeatTimeout](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a021e0986104e7481cd81459ce0c225f6)`[get, set]` |
| | |

| Events | |
| --- | --- |
| Action | [Heartbeat](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a301e65bd667639da3e6330a54c37d79b) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Enqueue() [1/2]

| void [ATAS.DataFeedsCore.IMessageQueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md).Enqueue | ( | [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | action |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aa4f5166696a4a38c83c0f7ee9f849570), and [ATAS.DataFeedsCore.AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a8b4ef3b8870cdefcd2e1e3aeb2ed26b3).

## [◆](https://docs.atas.net/en/)Enqueue() [2/2]

| void [ATAS.DataFeedsCore.IMessageQueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md).Enqueue | ( | [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | TMessage | message |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a1f27825aa43e0c74a6f520f2427ef12c), and [ATAS.DataFeedsCore.AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a03b99ab5cc545630d7738f6bb4e21343).

## [◆](https://docs.atas.net/en/)Start()

| void [ATAS.DataFeedsCore.IMessageQueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md).Start | ( | [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | handler |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a11783b07a2d2854d39fada82a05dac5d), and [ATAS.DataFeedsCore.AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#ab21f0b2268f34176d601516475ad2240).

## [◆](https://docs.atas.net/en/)Stop()

| void [ATAS.DataFeedsCore.IMessageQueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md).Stop | ( | [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aece4a43baa5b763fd1c24fb386a039f9), and [ATAS.DataFeedsCore.AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#ae40fd8b4ab98c619cf001fc6e126ef4d).

## Property Documentation

## [◆](https://docs.atas.net/en/)HeartbeatTimeout

| TimeSpan [ATAS.DataFeedsCore.IMessageQueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md).HeartbeatTimeout |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#af119ae3ae76578c4385ed437b9fb9f4b), and [ATAS.DataFeedsCore.AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md#a2f6d4b11673c4affe26b2c876d14cad2).

## Event Documentation

## [◆](https://docs.atas.net/en/)Heartbeat

| Action [ATAS.DataFeedsCore.IMessageQueue](./interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md).Heartbeat |
| --- |

The documentation for this interface was generated from the following file:
- [IMessageQueue.cs](../files/IMessageQueue_8cs.md)
