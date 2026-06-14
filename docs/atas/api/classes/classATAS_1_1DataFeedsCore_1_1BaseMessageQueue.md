# ATAS.DataFeedsCore.BaseMessageQueue< TMessage, TItem > Class Template Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.html

Inheritance diagram for ATAS.DataFeedsCore.BaseMessageQueue< TMessage, TItem >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.BaseMessageQueue< TMessage, TItem >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a1f27825aa43e0c74a6f520f2427ef12c) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, TMessage message) |
| | |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aa4f5166696a4a38c83c0f7ee9f849570) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action action) |
| | |
| void | [Start](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a11783b07a2d2854d39fada82a05dac5d) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| void | [Stop](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aece4a43baa5b763fd1c24fb386a039f9) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |
| void | [Enqueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a700d549bbcebbf0ecf97b229a33a2d48) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, TMessage message) |
| | |
| void | [Enqueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#aba9326ea042897bb29a0bdc44427b2dd) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action action) |
| | |
| void | [Start](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#ad50e20850db3ea2b9391c4ea3618edc8) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| void | [Stop](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a742273e3891cbbfe992012887fff45d5) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |

| Protected Member Functions | |
| --- | --- |
| | [BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aff48cbd24cc439ab49744d49c94ebcf9) () |
| | |
| abstract TItem | [CreateItem](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#ac3292a9db0366cf2ca586e82ffdd61e2) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, TMessage message, Action action) |
| | |
| abstract void | [OnStart](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#af82daff44d813ec306cd1de5207ac744) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| abstract bool | [OnStop](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a381699e2109fc5c8d7c5a483fb680593) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |
| abstract void | [OnProcess](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aa9dca6252b5db04f3358c5b5d3eb1f8a) (TItem item) |
| | |

| Properties | |
| --- | --- |
| TimeSpan | [HeartbeatTimeout](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#af119ae3ae76578c4385ed437b9fb9f4b)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) | |
| TimeSpan | [HeartbeatTimeout](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a021e0986104e7481cd81459ce0c225f6)`[get, set]` |
| | |

| Events | |
| --- | --- |
| Action | [Heartbeat](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a25bc31dda3f00c2fc60d6a8c3e7f4381) |
| | |
| - Events inherited from [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) | |
| Action | [Heartbeat](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a301e65bd667639da3e6330a54c37d79b) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)BaseMessageQueue()

| [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).[BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CreateItem()

| abstract TItem [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).CreateItem | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | TMessage | message, |
| | | Action | action |
| | ) | | |

protectedpure virtual

## [◆](https://docs.atas.net/en/)Enqueue() [1/2]

| void [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).Enqueue | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | action |
| | ) | | |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#aba9326ea042897bb29a0bdc44427b2dd).

## [◆](https://docs.atas.net/en/)Enqueue() [2/2]

| void [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).Enqueue | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | TMessage | message |
| | ) | | |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a700d549bbcebbf0ecf97b229a33a2d48).

## [◆](https://docs.atas.net/en/)OnProcess()

| abstract void [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).OnProcess | ( | TItem | item | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

## [◆](https://docs.atas.net/en/)OnStart()

| abstract void [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).OnStart | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | handler |
| | ) | | |

protectedpure virtual

Implemented in [ATAS.DataFeedsCore.SimpleMessageQueue](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a5151998cbd92c6bb70bb37e38e277530), and [ATAS.DataFeedsCore.MultiConnectorMessageQueue](./classATAS_1_1DataFeedsCore_1_1MultiConnectorMessageQueue.md#a95fc085bbef96c9caf68c41e2f70f977).

## [◆](https://docs.atas.net/en/)OnStop()

| abstract bool [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).OnStop | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.DataFeedsCore.SimpleMessageQueue](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a22a1d7f6224542134cfa16faf8477b36), and [ATAS.DataFeedsCore.MultiConnectorMessageQueue](./classATAS_1_1DataFeedsCore_1_1MultiConnectorMessageQueue.md#ac6d7a9bf88f3762c9d97438741f9bc22).

## [◆](https://docs.atas.net/en/)Start()

| void [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).Start | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | handler |
| | ) | | |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#ad50e20850db3ea2b9391c4ea3618edc8).

## [◆](https://docs.atas.net/en/)Stop()

| void [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).Stop | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a742273e3891cbbfe992012887fff45d5).

## Property Documentation

## [◆](https://docs.atas.net/en/)HeartbeatTimeout

| TimeSpan [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).HeartbeatTimeout |
| --- |

getset

Implements [ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md#a021e0986104e7481cd81459ce0c225f6).

## Event Documentation

## [◆](https://docs.atas.net/en/)Heartbeat

| Action [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md).Heartbeat |
| --- |

The documentation for this class was generated from the following file:
- [IMessageQueue.cs](../files/IMessageQueue_8cs.md)
