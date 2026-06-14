# AsyncConnector.cs File Reference

Source: https://docs.atas.net/en/AsyncConnector_8cs.html

| Classes | |
| --- | --- |
| class | [ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) |
| | Connector base that allows to use `await` to switch to the connector queue thread. [More...](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#details) |
| | |
| class | [ATAS.DataFeedsCore.AsyncConnector.ConnectorSynchronizationContext](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md) |
| | Custom synchronization context to forward await continuations to the connector queue. [More...](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md#details) |
| | |
| struct | [ATAS.DataFeedsCore.AsyncConnector.SynchronizationContextAwaiter](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md) |
| | Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` [More...](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md#details) |
| | |
| struct | [ATAS.DataFeedsCore.AsyncConnector.SynchronizationContextQueueAwaiter](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md) |
| | Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` This awaiter always passes the continuation to the connector's queue. [More...](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md#details) |
| | |
| class | [ATAS.DataFeedsCore.AsyncConnector.AsyncAwaiterFacade](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterFacade.md) |
| | Facade for `SynchronizationContextAwaiter` to allow explicit call of `SwitchToConnectorThreadAsync()` [More...](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterFacade.md#details) |
| | |
| class | [ATAS.DataFeedsCore.AsyncConnector.AsyncAwaiterQueueFacade](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterQueueFacade.md) |
| | Facade for `SynchronizationContextQueueAwaiter` to allow explicit call of `ForceToConnectorThreadAsync()` [More...](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterQueueFacade.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.DataFeedsCore](../namespaces/namespaceATAS_1_1DataFeedsCore.md) |
| | |
