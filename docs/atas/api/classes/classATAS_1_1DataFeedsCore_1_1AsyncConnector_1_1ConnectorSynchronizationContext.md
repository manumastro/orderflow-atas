# ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.ConnectorSynchronizationContext Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.html

Custom synchronization context to forward await continuations to the connector queue.
 [More...](./classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md#details)

Inheritance diagram for ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.ConnectorSynchronizationContext:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.AsyncConnector< TPortfolioKey, TSecurityKey >.ConnectorSynchronizationContext:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [ConnectorSynchronizationContext](./classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md#a04aa5084ff61a0b761cb004e10c2e17b) ([AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) connector) |
| | |
| override void | [Post](./classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md#ab0f754023e67f61f0f99ab442b983b99) (SendOrPostCallback d, object state) |
| | |
| override void | [Send](./classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md#a03c46a6afa7355bec302ad92bc5a07fd) (SendOrPostCallback d, object state) |
| | |
| override SynchronizationContext | [CreateCopy](./classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md#ad52d00c47e6e1fc6c775d93b09e84e33) () |
| | |

## Detailed Description

Custom synchronization context to forward await continuations to the connector queue.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ConnectorSynchronizationContext()

| [ATAS.DataFeedsCore.AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).ConnectorSynchronizationContext.ConnectorSynchronizationContext | ( | [AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CreateCopy()

| override SynchronizationContext [ATAS.DataFeedsCore.AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).ConnectorSynchronizationContext.CreateCopy | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Post()

| override void [ATAS.DataFeedsCore.AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).ConnectorSynchronizationContext.Post | ( | SendOrPostCallback | d, |
| --- | --- | --- | --- |
| | | object | state |
| | ) | | |

## [◆](https://docs.atas.net/en/)Send()

| override void [ATAS.DataFeedsCore.AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md).ConnectorSynchronizationContext.Send | ( | SendOrPostCallback | d, |
| --- | --- | --- | --- |
| | | object | state |
| | ) | | |

The documentation for this class was generated from the following file:
- [AsyncConnector.cs](../files/AsyncConnector_8cs.md)
