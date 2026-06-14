# ATAS.Strategies Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1Strategies.html

| Namespaces | |
| --- | --- |
| namespace | [ATM](./namespaceATAS_1_1Strategies_1_1ATM.md) |
| | |
| namespace | [Chart](./namespaceATAS_1_1Strategies_1_1Chart.md) |
| | |
| namespace | [Editors](./namespaceATAS_1_1Strategies_1_1Editors.md) |
| | |

| Classes | |
| --- | --- |
| interface | [IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) |
| | Represents a trading strategy. [More...](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#details) |
| | |
| class | [Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md) |
| | Base class for implementing trading strategies. [More...](../classes/classATAS_1_1Strategies_1_1Strategy.md#details) |
| | |
| class | StrategyLoggingExtensions |
| | |
| class | [StrategyNotificationEventArgs](../classes/classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md) |
| | Provides data for the StrategyNotification event. [More...](../classes/classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md#details) |
| | |
| class | [StrategyStateChangedEventArgs](../classes/classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md) |
| | Provides data for the StrategyStateChanged event. [More...](../classes/classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md#details) |
| | |

| Typedefs | |
| --- | --- |
| using | [BaseLoggerSource](./namespaceATAS_1_1Strategies.md#aec031ca5894c0eeffb98187da8284af9) = Utils.Common.Logging.BaseLoggerSource |
| | |

| Enumerations | |
| --- | --- |
| enum | [StrategyErrorTypes](./namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271) { [MaxNumberOfErrorsExceeded](./namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271a6a10e333141821b442c71e1067bc6283)
, [MaxNumberOfConnectionErrorsExceeded](./namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271ac3122684ff7d228456400dc6ac00ddb5)
, [PreviousOrdersNotFound](./namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271abda341ada2faea77b941cb5115e9f784)
 } |
| | Strategy error types. [More...](./namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271) |
| | |
| enum | [StrategyStates](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) {
  [Stopped](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89ac23e2b09ebe6bf4cb5e2a9abe85c0be2)
, [Started](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89a8428552d86c0d262a542a528af490afa)
, [Suspended](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89a8bf906833cc7aea8084f552217ed9c1d)
, [Error](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89a902b0d55fddef6f8d651fe1035b7d4bd)
,
  [Watch](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89af20658650d987d31063b593c05980397)

 } |
| | Represents the states of a trading strategy. [More...](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) |
| | |

## Typedef Documentation

## [◆](https://docs.atas.net/en/)BaseLoggerSource

| using [ATAS.Strategies.BaseLoggerSource](./namespaceATAS_1_1Strategies.md#aec031ca5894c0eeffb98187da8284af9) = typedef Utils.Common.Logging.BaseLoggerSource |
| --- |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)StrategyErrorTypes

| enum [ATAS.Strategies.StrategyErrorTypes](./namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271) |
| --- |

Strategy error types.

| Enumerator | |
| --- | --- |
| MaxNumberOfErrorsExceeded | The allowed number of errors has been exceeded. |
| MaxNumberOfConnectionErrorsExceeded | The allowed number of [ATAS.DataFeedsCore.Exceptions.ConnectorNotConnectedException](../classes/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ConnectorNotConnectedException.md) errors has been exceeded. |
| PreviousOrdersNotFound | The previous SL/TP orders are not found. |

## [◆](https://docs.atas.net/en/)StrategyStates

| enum [ATAS.Strategies.StrategyStates](./namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) |
| --- |

Represents the states of a trading strategy.

| Enumerator | |
| --- | --- |
| Stopped | The strategy is stopped, not currently running or processing any trades. |
| Started | The strategy is started and actively processing trades. |
| Suspended | The strategy is suspended, temporarily paused or waiting for a specific event to resume. |
| Error | The strategy is in error, temporarily waiting for a settings changed event to resume. |
| Watch | The strategy is watching for current position. |
