# ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.SynchronizationContextAwaiter Struct Reference

Source: https://docs.atas.net/en/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.html

Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext`
 [More...](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md#details)

Inheritance diagram for ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.SynchronizationContextAwaiter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.SynchronizationContextAwaiter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [SynchronizationContextAwaiter](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md#abf22c77a04931f9ddbfc94aa02aa634b) (SynchronizationContext context) |
| | |
| void | [OnCompleted](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md#a7188ae8b8047dea8428d3497e48a3812) (Action continuation) |
| | |
| void | [GetResult](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md#a5d716c9602f031c32feb1c4154f877b0) () |
| | |

| Properties | |
| --- | --- |
| bool | [IsCompleted](./structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md#a62211a4aed477d5709a105f3485dc583)`[get]` |
| | |

## Detailed Description

Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext`

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SynchronizationContextAwaiter()

| [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextAwaiter.SynchronizationContextAwaiter | ( | SynchronizationContext | context | ) | |
| --- | --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetResult()

| void [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextAwaiter.GetResult | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnCompleted()

| void [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextAwaiter.OnCompleted | ( | Action | continuation | ) | |
| --- | --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)IsCompleted

| bool [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).SynchronizationContextAwaiter.IsCompleted |
| --- |

get

The documentation for this struct was generated from the following file:
- [AsyncConnector.cs](../files/AsyncConnector_8cs.md)
