# ATAS.Strategies.Chart.IChartStrategy Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.html

Represents a chart strategy that extends the basic functionality of an IStrategy with additional chart-related features.
 [More...](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#details)

Inheritance diagram for ATAS.Strategies.Chart.IChartStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.Chart.IChartStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [StopWithNotification](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a370ce0bbd22d5d3d2be4d8e043e46793) (string message) |
| | Stops the strategy with a notification message. |
| | |
| void | [OpenOrder](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#add7c48f9ede9667aa7ba8a0f97344eab) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Opens an order for the strategy. |
| | |
| void | [ModifyOrder](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a01509c1de2aadb7994a819e41a5db29e) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | Modifies an existing order for the strategy. |
| | |
| void | [CancelOrder](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a3808947989c50df10ee357f597a2171a) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Cancels an existing order for the strategy. |
| | |
| Task | [OpenOrderAsync](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a2c7f2e85bf2f50e4d49397eccbba4e0f) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Opens an order for the strategy asynchronously. |
| | |
| Task | [ModifyOrderAsync](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#abc6a4b39d9ba1383fcfda9ce45588085) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | Modifies an existing order for the strategy asynchronously. |
| | |
| Task | [CancelOrderAsync](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a9f15690af8db637605ba5a7aaeaa5deb) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Cancels an existing order for the strategy asynchronously. |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.IStrategy](./interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| Task | [StartAsync](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a8b8803a4d0eff2da7088e4f0b3b4d25a) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| Task | [StopAsync](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a83d83423aa3f8a5248d681b4bb5126ad) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
| | |

| Properties | |
| --- | --- |
| IEnumerable | [Orders](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a128f2f53547a9ddf8131a29e4b910d33)`[get]` |
| | Gets the collection of orders associated with this strategy. |
| | |
| IEnumerable | [MyTrades](./interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a8ae37c6d12ae4459dfe8582326a22677)`[get]` |
| | Gets the collection of trades associated with this strategy. |
| | |
| - Properties inherited from [ATAS.Strategies.IStrategy](./interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
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

| Additional Inherited Members | |
| --- | --- |
| - Events inherited from [ATAS.Strategies.IStrategy](./interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| EventHandler | [StateChanged](./interfaceATAS_1_1Strategies_1_1IStrategy.md#ab97be6450a8121dafbba88aa9025ca90) |
| | Occurs when the state of the strategy changes. |
| | |
| EventHandler | [ShowNotification](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a064d9dc57f09b7862d89b5b94e47c04c) |
| | Occurs when the strategy needs to show a notification or alert. |
| | |

## Detailed Description

Represents a chart strategy that extends the basic functionality of an IStrategy with additional chart-related features.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CancelOrder()

| void ATAS.Strategies.Chart.IChartStrategy.CancelOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Cancels an existing order for the strategy.

Parameters

| order | The order to be canceled. |
| --- | --- |

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a60707345819d3dba2430dbbd09c6c0c1).

## [◆](https://docs.atas.net/en/)CancelOrderAsync()

| Task ATAS.Strategies.Chart.IChartStrategy.CancelOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Cancels an existing order for the strategy asynchronously.

Parameters

| order | The order to be canceled. |
| --- | --- |

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a4383cb65cabb5969133628bd25c8c50c).

## [◆](https://docs.atas.net/en/)ModifyOrder()

| void ATAS.Strategies.Chart.IChartStrategy.ModifyOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder |
| | ) | | |

Modifies an existing order for the strategy.

Parameters

| order | The order to be modified. |
| --- | --- |
| newOrder | The new order with updated parameters. |

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#aefd586572cee58f893fadddbe9133388).

## [◆](https://docs.atas.net/en/)ModifyOrderAsync()

| Task ATAS.Strategies.Chart.IChartStrategy.ModifyOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder |
| | ) | | |

Modifies an existing order for the strategy asynchronously.

Parameters

| order | The order to be modified. |
| --- | --- |
| newOrder | The new order with updated parameters. |

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a3b0105554f8d91dc53ce50a962bdf1f3).

## [◆](https://docs.atas.net/en/)OpenOrder()

| void ATAS.Strategies.Chart.IChartStrategy.OpenOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Opens an order for the strategy.

Parameters

| order | The order to be opened. |
| --- | --- |

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a8153dc643beb8694da1d609d56a20d66).

## [◆](https://docs.atas.net/en/)OpenOrderAsync()

| Task ATAS.Strategies.Chart.IChartStrategy.OpenOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Opens an order for the strategy asynchronously.

Parameters

| order | The order to be opened. |
| --- | --- |

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#af5bdbd4688edfdb370b511a704a7284b).

## [◆](https://docs.atas.net/en/)StopWithNotification()

| void ATAS.Strategies.Chart.IChartStrategy.StopWithNotification | ( | string | message | ) | |
| --- | --- | --- | --- | --- | --- |

Stops the strategy with a notification message.

Parameters

| message | The message to be displayed as a notification. |
| --- | --- |

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#abf88e5573113bc36c7b679c18870ebc1).

## Property Documentation

## [◆](https://docs.atas.net/en/)MyTrades

| IEnumerable ATAS.Strategies.Chart.IChartStrategy.MyTrades |
| --- |

get

Gets the collection of trades associated with this strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#aba061cd88a58ac0a85924255a74ae1a2).

## [◆](https://docs.atas.net/en/)Orders

| IEnumerable ATAS.Strategies.Chart.IChartStrategy.Orders |
| --- |

get

Gets the collection of orders associated with this strategy.

Implemented in [ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a8fe49c454277e91d972cd7ec4911b242).

The documentation for this interface was generated from the following file:
- [IChartStrategy.cs](../files/IChartStrategy_8cs.md)
