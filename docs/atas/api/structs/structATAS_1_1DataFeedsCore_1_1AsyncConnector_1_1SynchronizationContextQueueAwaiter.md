# ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.SynchronizationContextQueueAwaiter Struct Reference

Source: https://docs.atas.net/en/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.html

Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` This awaiter always passes the continuation to the connector's queue.
 [More...](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md#details)

Inheritance diagram for ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.SynchronizationContextQueueAwaiter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.SynchronizationContextQueueAwaiter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [SynchronizationContextQueueAwaiter](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md#a9e2f9155c3ab67d3d41409edd734dfa7) (SynchronizationContext context) |
| | |
| void | [OnCompleted](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md#af4e66326f72eb4f40b0f89e0472f9d2b) (Action continuation) |
| | |
| void | [GetResult](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md#a57d57af246a8f90135940073885bb88d) () |
| | |

| Properties | |
| --- | --- |
| bool | [IsCompleted](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md#a98bb246ca9afd477e3b832bd7a5e5d1a)`[get]` |
| | |

## Detailed Description

Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` This awaiter always passes the continuation to the connector's queue.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SynchronizationContextQueueAwaiter()

| [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextQueueAwaiter.SynchronizationContextQueueAwaiter | ( | SynchronizationContext | context | ) | |
| --- | --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetResult()

| void [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextQueueAwaiter.GetResult | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnCompleted()

| void [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextQueueAwaiter.OnCompleted | ( | Action | continuation | ) | |
| --- | --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)IsCompleted

| bool [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextQueueAwaiter.IsCompleted |
| --- |

get

The documentation for this struct was generated from the following file:
- [AsyncConnector.cs](../files/AsyncConnector_8cs.md)
