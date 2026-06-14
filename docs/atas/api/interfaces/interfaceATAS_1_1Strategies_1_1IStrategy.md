# ATAS.Strategies.IStrategy Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Strategies_1_1IStrategy.html

Represents a trading strategy.
 [More...](./interfaceATAS_1_1Strategies_1_1IStrategy.md#details)

Inheritance diagram for ATAS.Strategies.IStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.IStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| Task | [StartAsync](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a8b8803a4d0eff2da7088e4f0b3b4d25a) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| Task | [StopAsync](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a83d83423aa3f8a5248d681b4bb5126ad) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
| | |

| Properties | |
| --- | --- |
| string | [Name](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a82fa08d68c09e1ac4b699ed5901408c3)`[get, set]` |
| | Gets or sets the name of the strategy. |
| | |
| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) | [State](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a18fcdb4e7e4a8d94e285cb45d1742b5e)`[get]` |
| | Gets the current state of the strategy. |
| | |
| decimal | [CurrentPosition](./interfaceATAS_1_1Strategies_1_1IStrategy.md#aa3c8e8450cedf2e6dc772cee79022057)`[get]` |
| | Gets the current position volume of the strategy. |
| | |
| decimal | [AveragePrice](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a0990ad1aaa4097b121e2f9963026a3a2)`[get]` |
| | Gets the average price of the strategy's trades. |
| | |
| decimal | [OpenPnL](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a21cfcaa3598adf5c9a909fa1d87e08c1)`[get]` |
| | Gets the open profit and loss of the strategy. |
| | |
| decimal | [ClosedPnL](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a7b734a3537121bb3524c16cc94d5d8a2)`[get]` |
| | Gets the closed profit and loss of the strategy. |
| | |
| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a1d6806a24b84f596cd7e4adf8a315edd)`[get, set]` |
| | Gets or sets the security associated with the strategy. |
| | |
| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a2ac811e04e280f17e9a46aa376122326)`[get, set]` |
| | Gets or sets the portfolio associated with the strategy. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a1c4dd7746f51fdd6dc9033ec8c152494)`[get, set]` |
| | Gets or sets the T+ limits for the strategy. |
| | |
| [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [Connector](./interfaceATAS_1_1Strategies_1_1IStrategy.md#ac42fb5c1c25cb4734acaa356371c6505)`[get, set]` |
| | Gets or sets the data feed connector for the strategy. |
| | |

| Events | |
| --- | --- |
| EventHandler | [StateChanged](./interfaceATAS_1_1Strategies_1_1IStrategy.md#ab97be6450a8121dafbba88aa9025ca90) |
| | Occurs when the state of the strategy changes. |
| | |
| EventHandler | [ShowNotification](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a064d9dc57f09b7862d89b5b94e47c04c) |
| | Occurs when the strategy needs to show a notification or alert. |
| | |

## Detailed Description

Represents a trading strategy.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)StartAsync()

| Task ATAS.Strategies.IStrategy.StartAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Starts the strategy, allowing it to execute its trading logic.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#ab825ea11da3e33f09b4f098af780521d), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a2e9997a9e4ac7d773684f1a09bb0a6a4).

## [◆](https://docs.atas.net/en/)StopAsync()

| Task ATAS.Strategies.IStrategy.StopAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Stops the strategy, terminating its execution and releasing any resources.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a465ca76523f902b69adc1c3d862e799b), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#acf7b05b5b8377f0f22cbaf0804ea3565).

## Property Documentation

## [◆](https://docs.atas.net/en/)AveragePrice

| decimal ATAS.Strategies.IStrategy.AveragePrice |
| --- |

get

Gets the average price of the strategy's trades.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a5c12170dfa214437261503a5cab6ed97), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a07573e17f1176d5b2adac90f5a886134).

## [◆](https://docs.atas.net/en/)ClosedPnL

| decimal ATAS.Strategies.IStrategy.ClosedPnL |
| --- |

get

Gets the closed profit and loss of the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#afc4a3cc50160389ba591fdf88e907329), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#aa2a9a663ead0622ef44f75aa412b579d).

## [◆](https://docs.atas.net/en/)Connector

| [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) ATAS.Strategies.IStrategy.Connector |
| --- |

getset

Gets or sets the data feed connector for the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#ad771faee286fa3c3a18e10ce3c17dcce), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a8a31979c7e9857cade90549708f2bc78).

## [◆](https://docs.atas.net/en/)CurrentPosition

| decimal ATAS.Strategies.IStrategy.CurrentPosition |
| --- |

get

Gets the current position volume of the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#acfc91e9c225c7e79feb0ee32d20571bd), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a9bf909bec2b2dbc0eabd721c212285ff).

## [◆](https://docs.atas.net/en/)Name

| string ATAS.Strategies.IStrategy.Name |
| --- |

getset

Gets or sets the name of the strategy.

Implemented in [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#aa9bfbe77e7c89fa0bc933c1a0351c5fb).

## [◆](https://docs.atas.net/en/)OpenPnL

| decimal ATAS.Strategies.IStrategy.OpenPnL |
| --- |

get

Gets the open profit and loss of the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a7d0c68655f8c99816239924c6926ef56), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a3861bd96e7b0c5be873ea863b2a4122c).

## [◆](https://docs.atas.net/en/)Portfolio

| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.Strategies.IStrategy.Portfolio |
| --- |

getset

Gets or sets the portfolio associated with the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a2194428e41fa00c999a1915a5ffb4638), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a1d9f992e6103556c710ab7a5ec72c030).

## [◆](https://docs.atas.net/en/)Security

| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.Strategies.IStrategy.Security |
| --- |

getset

Gets or sets the security associated with the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a4dd14b69596de3becbe4e206942e4bdd), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a8c12c1aa6a3076d09c89acc636cba81e).

## [◆](https://docs.atas.net/en/)State

| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) ATAS.Strategies.IStrategy.State |
| --- |

get

Gets the current state of the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#ae6c832175e631aafc3451cc928d0f9d6), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#a247f1529ee7c68ea9e294dce42fc68e8).

## [◆](https://docs.atas.net/en/)TPlusLimit

| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? ATAS.Strategies.IStrategy.TPlusLimit |
| --- |

getset

Gets or sets the T+ limits for the strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#afb192bc836d1badb6043f15a854281f7), and [ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md#abc38115f61804e04c5d75060012c9c0e).

## Event Documentation

## [◆](https://docs.atas.net/en/)ShowNotification

| EventHandler ATAS.Strategies.IStrategy.ShowNotification |
| --- |

Occurs when the strategy needs to show a notification or alert.

## [◆](https://docs.atas.net/en/)StateChanged

| EventHandler ATAS.Strategies.IStrategy.StateChanged |
| --- |

Occurs when the state of the strategy changes.

The documentation for this interface was generated from the following file:
- [IStrategy.cs](../files/IStrategy_8cs.md)
