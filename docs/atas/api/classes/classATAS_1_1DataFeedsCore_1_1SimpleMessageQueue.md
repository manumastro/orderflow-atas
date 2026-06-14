# ATAS.DataFeedsCore.SimpleMessageQueue< TMessage > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.html

Inheritance diagram for ATAS.DataFeedsCore.SimpleMessageQueue< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.SimpleMessageQueue< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| | [override](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a40257bf2b8826b533ec10630be51c108) (TMessage, Action) [CreateItem](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#ac3292a9db0366cf2ca586e82ffdd61e2)([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector |
| | |
| [override](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a40257bf2b8826b533ec10630be51c108) void | [OnStart](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a5151998cbd92c6bb70bb37e38e277530) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| [override](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a40257bf2b8826b533ec10630be51c108) bool | [OnStop](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a22a1d7f6224542134cfa16faf8477b36) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |
| [override](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a40257bf2b8826b533ec10630be51c108) void | [OnProcess](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a2ea4d5c9e5311aa0dae2d5c19c5afdf6) ((TMessage, Action) item) |
| | |
| - Protected Member Functions inherited from [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
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

| Additional Inherited Members | |
| --- | --- |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a1f27825aa43e0c74a6f520f2427ef12c) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, TMessage message) |
| | |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aa4f5166696a4a38c83c0f7ee9f849570) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action action) |
| | |
| void | [Start](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a11783b07a2d2854d39fada82a05dac5d) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, Action handler) |
| | |
| void | [Stop](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#aece4a43baa5b763fd1c24fb386a039f9) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| TimeSpan | [HeartbeatTimeout](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#af119ae3ae76578c4385ed437b9fb9f4b)`[get, set]` |
| | |
| - Events inherited from [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| Action | [Heartbeat](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a25bc31dda3f00c2fc60d6a8c3e7f4381) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnProcess()

| [override](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a40257bf2b8826b533ec10630be51c108) void [ATAS.DataFeedsCore.SimpleMessageQueue](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md).OnProcess | ( | (TMessage, Action) | item | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)OnStart()

| [override](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a40257bf2b8826b533ec10630be51c108) void [ATAS.DataFeedsCore.SimpleMessageQueue](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md).OnStart | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | Action | handler |
| | ) | | |

protectedvirtual

Implements [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#af82daff44d813ec306cd1de5207ac744).

## [◆](https://docs.atas.net/en/)OnStop()

| [override](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md#a40257bf2b8826b533ec10630be51c108) bool [ATAS.DataFeedsCore.SimpleMessageQueue](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md).OnStop | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.DataFeedsCore.BaseMessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md#a381699e2109fc5c8d7c5a483fb680593).

## [◆](https://docs.atas.net/en/)override()

| [ATAS.DataFeedsCore.SimpleMessageQueue](./classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md).override | ( | TMessage | , |
| --- | --- | --- | --- |
| | | Action | |
| | ) | | |

protected

The documentation for this class was generated from the following file:
- [IMessageQueue.cs](../files/IMessageQueue_8cs.md)
