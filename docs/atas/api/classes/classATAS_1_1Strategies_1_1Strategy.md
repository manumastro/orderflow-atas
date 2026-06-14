# ATAS.Strategies.Strategy Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Strategies_1_1Strategy.html

Base class for implementing trading strategies.
 [More...](./classATAS_1_1Strategies_1_1Strategy.md#details)

Inheritance diagram for ATAS.Strategies.Strategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.Strategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Start](./classATAS_1_1Strategies_1_1Strategy.md#ade4c390a1851155c7f63a161dde6196e) () |
| | |
| void | [Stop](./classATAS_1_1Strategies_1_1Strategy.md#a5af17fec828c2ce5b39c84cab7512f36) () |
| | |
| async Task | [StartAsync](./classATAS_1_1Strategies_1_1Strategy.md#a2e9997a9e4ac7d773684f1a09bb0a6a4) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| async Task | [WatchAsync](./classATAS_1_1Strategies_1_1Strategy.md#ad58d918eb8415a7c32148b5b3133f7d3) () |
| | |
| async Task | [StartFromWatchAsync](./classATAS_1_1Strategies_1_1Strategy.md#a3f73ff7b92a1371c9af383339dbe8e73) () |
| | |
| async Task | [StopAsync](./classATAS_1_1Strategies_1_1Strategy.md#acf7b05b5b8377f0f22cbaf0804ea3565) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
| | |
| async void | [OpenOrder](./classATAS_1_1Strategies_1_1Strategy.md#a2c9169c7b4be83c53f1c3c7f90c6879e) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Opens an order. |
| | |
| async Task | [OpenOrderAsync](./classATAS_1_1Strategies_1_1Strategy.md#a8be8f763339e633e01c272f68b17778f) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Opens an order. |
| | |
| async void | [ModifyOrder](./classATAS_1_1Strategies_1_1Strategy.md#a7a99290532c1685f6142263ed62e6083) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) neworder, bool isAutomated=true) |
| | Modifies an order. |
| | |
| async Task | [ModifyOrderAsync](./classATAS_1_1Strategies_1_1Strategy.md#adbd0efd2d87e907763d81f961470f6cc) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) neworder, bool isAutomated=true) |
| | Modifies an order. |
| | |
| async void | [CancelOrder](./classATAS_1_1Strategies_1_1Strategy.md#af888a67fab260e57c014dc859556a915) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Cancels an order. |
| | |
| async Task | [CancelOrderAsync](./classATAS_1_1Strategies_1_1Strategy.md#a423b3fb136fb060941db865828d08dc5) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Cancels an order. |
| | |
| Task | [StartAsync](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a8b8803a4d0eff2da7088e4f0b3b4d25a) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| Task | [StopAsync](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a83d83423aa3f8a5248d681b4bb5126ad) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
| | |

| Protected Member Functions | |
| --- | --- |
| | [Strategy](./classATAS_1_1Strategies_1_1Strategy.md#abb4d47356a0d6d870711f2b8ddde84fd) () |
| | |
| void | [SetState](./classATAS_1_1Strategies_1_1Strategy.md#ac068422672291f843592b4fa5674dda8) ([StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) state) |
| | Set strategy state. |
| | |
| void | [SetErrorState](./classATAS_1_1Strategies_1_1Strategy.md#aa374a8db1a31455b708ab997ead89690) ([StrategyErrorTypes](../namespaces/namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271) type, string[] errorDescriptions) |
| | Set StrategyStates.Error state. |
| | |
| void | [ResetErrorState](./classATAS_1_1Strategies_1_1Strategy.md#a0bbe50de68938af9f856e7ac37b1dce2) () |
| | Reset error state. |
| | |
| void | [RaisePropertyChanged](./classATAS_1_1Strategies_1_1Strategy.md#ad6491f31b4fe77ec504af425a116d1fb) (string propertyName) |
| | Raises the PropertyChanged event with the specified property name. |
| | |
| void | [RaiseShowNotification](./classATAS_1_1Strategies_1_1Strategy.md#a2453e60639c8ad70cabb12c6f89557f0) (string message, string title=null, bool isError=false) |
| | Raises the ShowNotification event with the specified message, title, and error flag. |
| | |
| bool | [SetProperty](./classATAS_1_1Strategies_1_1Strategy.md#ab29c24bebb1b7fff85eaaa2fa45a210c) (ref TValue storage, TValue newValue, string propertyName, Action onChanged=null) |
| | Sets the property with the specified name to the new value and raises the PropertyChanged event if the value has changed. |
| | |
| string | [GetOCOGroup](./classATAS_1_1Strategies_1_1Strategy.md#a7d4b4a970053ef86131cdd580da050af) () |
| | Generates a unique OCO (One-Cancels-the-Other) group identifier based on the current timestamp. |
| | |
| bool | [CanProcess](./classATAS_1_1Strategies_1_1Strategy.md#a81f6b6ef027cede9cfdd7858a6d9c651) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Checks if the specified order can be processed by this strategy. |
| | |
| ICollection | [FilterMyTrades](./classATAS_1_1Strategies_1_1Strategy.md#a1294d8b1944d475615fb98e7d1f619f4) (IEnumerable trades) |
| | Filters and returns the collection of MyTrade that belong to the current portfolio and security and have occurred after the latest trade time. |
| | |
| void | [UpdateCurrentPosition](./classATAS_1_1Strategies_1_1Strategy.md#ae2f7d02455e2ad2cd233396aa1d57b5c) () |
| | Update thr CurrentPosition and AveragePrice values. |
| | |
| virtual Task | [OnStarted](./classATAS_1_1Strategies_1_1Strategy.md#a797a10bf103faf848475194494d75457) () |
| | Called when the strategy is started from StrategyStates.Stopped state. |
| | |
| virtual Task | [OnStartedFromWatch](./classATAS_1_1Strategies_1_1Strategy.md#ac78b46e4e7e0613d70ededb86afd2a6f) () |
| | Called when the strategy is started from StrategyStates.Watch state. |
| | |
| virtual Task | [OnStopping](./classATAS_1_1Strategies_1_1Strategy.md#a78ec5fd60d54a1f0c97b7ffaf53eede1) () |
| | Called when the strategy is stopping. |
| | |
| virtual Task | [OnStopped](./classATAS_1_1Strategies_1_1Strategy.md#a789356ecb84f130966e6ef84674dd21d) () |
| | Called when the strategy is stopped. |
| | |
| virtual Task | [OnOpenOrder](./classATAS_1_1Strategies_1_1Strategy.md#a02f76248cf2c12eea0793e17b2f35f83) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated) |
| | Called when a new order is opened. |
| | |
| virtual Task | [OnModifyOrder](./classATAS_1_1Strategies_1_1Strategy.md#a714f291495bd35ba61918fdc19e24f44) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool isAutomated) |
| | Called when an existing order is modified. |
| | |
| virtual Task | [OnCancelOrder](./classATAS_1_1Strategies_1_1Strategy.md#a3f1495660e455f48f120119007cc4b09) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated) |
| | Called when an order is canceled. |
| | |
| virtual void | [OnMarketDepth](./classATAS_1_1Strategies_1_1Strategy.md#ae42d28938d0d280b931f8e5f7dfc0465) (IEnumerable depths) |
| | Called when market depth data is received. |
| | |
| virtual void | [OnBestBidAsk](./classATAS_1_1Strategies_1_1Strategy.md#ae76af83f6b2f04a312bfe67d32735aa4) ([MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) depth) |
| | Called when the best bid or ask market depth data is received. |
| | |
| virtual void | [OnNewTrade](./classATAS_1_1Strategies_1_1Strategy.md#a1156a4c5fa80db189589d1fe5ed41673) ([Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) trade) |
| | Called when a new trade occurs. |
| | |
| virtual void | [OnNewPortfolio](./classATAS_1_1Strategies_1_1Strategy.md#a9bd23dc276dd94f8d782f4251e5d4f2c) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | Called when a new portfolio is added. |
| | |
| virtual void | [OnNewPosition](./classATAS_1_1Strategies_1_1Strategy.md#ab25c8865cb3c8e25d7a8d01d7edbbc2d) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | Called when a new position is added. |
| | |
| virtual void | [OnPositionChanged](./classATAS_1_1Strategies_1_1Strategy.md#af17d6ef4486c7ad54b3e682aaa61310e) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | Called when an existing position is changed. |
| | |
| virtual void | [OnPnLChanged](./classATAS_1_1Strategies_1_1Strategy.md#a31523364afbae751f14e7d9480a2de8e) (int ticks) |
| | Called when the profit and loss (PnL) changes. |
| | |
| virtual void | [OnNewOrder](./classATAS_1_1Strategies_1_1Strategy.md#abcf82eac51543b99ff8b97d52785b9fc) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when a new order is added. |
| | |
| virtual void | [OnOrderChanged](./classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when an existing order is changed. |
| | |
| virtual void | [OnOrderRegisterFailed](./classATAS_1_1Strategies_1_1Strategy.md#a2ea44a2007103545463be80f85b515b5) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string message) |
| | Called when an order registration fails. |
| | |
| virtual void | [OnOrderCancelFailed](./classATAS_1_1Strategies_1_1Strategy.md#a733aec75e1b68396ff38bafc9de74af6) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string message) |
| | Called when an order cancellation fails. |
| | |
| virtual void | [OnOrderModifyFailed](./classATAS_1_1Strategies_1_1Strategy.md#a2a206383275db0490ffadcfc6580425e) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, string message) |
| | Called when an order modification fails. |
| | |
| virtual void | [OnNewMyTrade](./classATAS_1_1Strategies_1_1Strategy.md#a648be4b1a2575e8a653b8a8a311d3971) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) myTrade) |
| | Called when a new trade is added to the collection of MyTrade. |
| | |
| virtual void | [OnCurrentPositionChanged](./classATAS_1_1Strategies_1_1Strategy.md#a4b1005797dce08e14bea18f7e1ca765d) () |
| | Called when the volume of the current position changes. |
| | |
| virtual void | [OnUpdateStrategyState](./classATAS_1_1Strategies_1_1Strategy.md#ade0b4bb984c19b8a445fda28b93726a1) () |
| | Called when the strategy state needs to be updated. |
| | |
| virtual bool | [CanProcess](./classATAS_1_1Strategies_1_1Strategy.md#a28b03647a27fced41cf5c2f07cc76dc2) () |
| | Checks if the strategy can process operations in the current state. |
| | |
| virtual bool | [CanUpdateCurrentPosition](./classATAS_1_1Strategies_1_1Strategy.md#a60d8e09ff972aefe78d4a9a64377d690) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | Checks if the current position can be updated with the specified position. |
| | |
| virtual void | [LogParameters](./classATAS_1_1Strategies_1_1Strategy.md#a794aeb1f7fd7a728e22b8a2d8e8e647e) () |
| | Log current parameters. |
| | |

| Properties | |
| --- | --- |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1Strategies_1_1Strategy.md#a8c12c1aa6a3076d09c89acc636cba81e)`[get, set]` |
| | Gets or sets the security associated with the strategy. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](./classATAS_1_1Strategies_1_1Strategy.md#a1d9f992e6103556c710ab7a5ec72c030)`[get, set]` |
| | Gets or sets the portfolio associated with the strategy. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](./classATAS_1_1Strategies_1_1Strategy.md#abc38115f61804e04c5d75060012c9c0e)`[get, set]` |
| | Gets or sets the T+ limits for the strategy. |
| | |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [Connector](./classATAS_1_1Strategies_1_1Strategy.md#a8a31979c7e9857cade90549708f2bc78)`[get, set]` |
| | Gets or sets the data feed connector for the strategy. |
| | |
| IEnumerable | [MyTrades](./classATAS_1_1Strategies_1_1Strategy.md#a896ae49426378cfde400f92c2b2210b4)`[get]` |
| | |
| IEnumerable | [Orders](./classATAS_1_1Strategies_1_1Strategy.md#a4eebe18eaa41c4fda44673f5b8e5b22a)`[get]` |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [Position](./classATAS_1_1Strategies_1_1Strategy.md#ab090a528e4c8d5d9657361931e4ab078)`[get, set]` |
| | |
| decimal | [CurrentPosition](./classATAS_1_1Strategies_1_1Strategy.md#a9bf909bec2b2dbc0eabd721c212285ff)`[get]` |
| | Gets the current position volume of the strategy. |
| | |
| decimal | [AveragePrice](./classATAS_1_1Strategies_1_1Strategy.md#a07573e17f1176d5b2adac90f5a886134)`[get]` |
| | Gets the average price of the strategy's trades. |
| | |
| int | [OpenTicksPnL](./classATAS_1_1Strategies_1_1Strategy.md#a5779518919a1fcce28069c3ceb17b18d)`[get]` |
| | |
| decimal | [OpenPnL](./classATAS_1_1Strategies_1_1Strategy.md#a3861bd96e7b0c5be873ea863b2a4122c)`[get]` |
| | Gets the open profit and loss of the strategy. |
| | |
| decimal | [ClosedPnL](./classATAS_1_1Strategies_1_1Strategy.md#aa2a9a663ead0622ef44f75aa412b579d)`[get]` |
| | Gets the closed profit and loss of the strategy. |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [BestBid](./classATAS_1_1Strategies_1_1Strategy.md#ac26d173e3f3d698c1bbee321af0e80ef)`[get]` |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [BestAsk](./classATAS_1_1Strategies_1_1Strategy.md#ab9940b9ecbf3abf39a80e8989f643118)`[get]` |
| | |
| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) | [State](./classATAS_1_1Strategies_1_1Strategy.md#a247f1529ee7c68ea9e294dce42fc68e8)`[get, protected set]` |
| | Gets the current state of the strategy. |
| | |
| [StrategyStateDescription](../namespaces/namespaceATAS_1_1Strategies.md#ac2b219bf1e9f7c99f8f9fc4c5fe39ea3) | [StateDescription](./classATAS_1_1Strategies_1_1Strategy.md#a95b1b372b56a1f381429182e4520f070)`[get]` |
| | |
| string | [Name](./classATAS_1_1Strategies_1_1Strategy.md#aa9bfbe77e7c89fa0bc933c1a0351c5fb)`[get, set]` |
| | Gets or sets the name of the strategy. |
| | |
| - Properties inherited from [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| string | [Name](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a82fa08d68c09e1ac4b699ed5901408c3)`[get, set]` |
| | Gets or sets the name of the strategy. |
| | |
| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) | [State](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a18fcdb4e7e4a8d94e285cb45d1742b5e)`[get]` |
| | Gets the current state of the strategy. |
| | |
| decimal | [CurrentPosition](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#aa3c8e8450cedf2e6dc772cee79022057)`[get]` |
| | Gets the current position volume of the strategy. |
| | |
| decimal | [AveragePrice](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a0990ad1aaa4097b121e2f9963026a3a2)`[get]` |
| | Gets the average price of the strategy's trades. |
| | |
| decimal | [OpenPnL](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a21cfcaa3598adf5c9a909fa1d87e08c1)`[get]` |
| | Gets the open profit and loss of the strategy. |
| | |
| decimal | [ClosedPnL](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a7b734a3537121bb3524c16cc94d5d8a2)`[get]` |
| | Gets the closed profit and loss of the strategy. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a1d6806a24b84f596cd7e4adf8a315edd)`[get, set]` |
| | Gets or sets the security associated with the strategy. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a2ac811e04e280f17e9a46aa376122326)`[get, set]` |
| | Gets or sets the portfolio associated with the strategy. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a1c4dd7746f51fdd6dc9033ec8c152494)`[get, set]` |
| | Gets or sets the T+ limits for the strategy. |
| | |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [Connector](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#ac42fb5c1c25cb4734acaa356371c6505)`[get, set]` |
| | Gets or sets the data feed connector for the strategy. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1Strategies_1_1Strategy.md#ae54cc0ee8ef3cfbf2efd66f4bfd42785) |
| | |
| EventHandler | [StateChanged](./classATAS_1_1Strategies_1_1Strategy.md#ab1e58113d9ed785c5d26d2cd2f560cac) |
| | |
| EventHandler | [ShowNotification](./classATAS_1_1Strategies_1_1Strategy.md#a4ac0ef8ec0383f07365ff1b2b719e301) |
| | |
| - Events inherited from [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| EventHandler | [StateChanged](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#ab97be6450a8121dafbba88aa9025ca90) |
| | Occurs when the state of the strategy changes. |
| | |
| EventHandler | [ShowNotification](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a064d9dc57f09b7862d89b5b94e47c04c) |
| | Occurs when the strategy needs to show a notification or alert. |
| | |

## Detailed Description

Base class for implementing trading strategies.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Strategy()

| ATAS.Strategies.Strategy.Strategy | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CancelOrder()

| async void ATAS.Strategies.Strategy.CancelOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated = `true` |
| | ) | | |

Cancels an order.

Parameters

| order | The order to cancel. |
| --- | --- |
| isAutomated | A flag indicating whether the order is placed automatically or by the user. |

## [◆](https://docs.atas.net/en/)CancelOrderAsync()

| async Task ATAS.Strategies.Strategy.CancelOrderAsync | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated = `true` |
| | ) | | |

Cancels an order.

Parameters

| order | The order to cancel. |
| --- | --- |
| isAutomated | A flag indicating whether the order is placed automatically or by the user. |

## [◆](https://docs.atas.net/en/)CanProcess() [1/2]

| virtual bool ATAS.Strategies.Strategy.CanProcess | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Checks if the strategy can process operations in the current state.

Returns`true` if the strategy can process; otherwise, `false`.

## [◆](https://docs.atas.net/en/)CanProcess() [2/2]

| bool ATAS.Strategies.Strategy.CanProcess | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Checks if the specified order can be processed by this strategy.

Parameters

| order | The order to be processed. |
| --- | --- |

Returns`true` if the order can be processed; otherwise, `false`.

## [◆](https://docs.atas.net/en/)CanUpdateCurrentPosition()

| virtual bool ATAS.Strategies.Strategy.CanUpdateCurrentPosition | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Checks if the current position can be updated with the specified position.

Parameters

| position | The position to be compared with the current position. |
| --- | --- |

Returns`true` if the current position can be updated; otherwise, `false`.

Reimplemented in [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a1c3fef087f9054bf719b4862faa0b17c).

## [◆](https://docs.atas.net/en/)FilterMyTrades()

| ICollection ATAS.Strategies.Strategy.FilterMyTrades | ( | IEnumerable | trades | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Filters and returns the collection of MyTrade that belong to the current portfolio and security and have occurred after the latest trade time.

Parameters

| trades | The collection of MyTrade to be filtered. |
| --- | --- |

ReturnsThe filtered collection of MyTrade.

## [◆](https://docs.atas.net/en/)GetOCOGroup()

| string ATAS.Strategies.Strategy.GetOCOGroup | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Generates a unique OCO (One-Cancels-the-Other) group identifier based on the current timestamp.

ReturnsA unique OCO group identifier.

## [◆](https://docs.atas.net/en/)LogParameters()

| virtual void ATAS.Strategies.Strategy.LogParameters | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Log current parameters.

Reimplemented in [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#af565f074a5cf9ca29ba9ef2ccdc776e1).

## [◆](https://docs.atas.net/en/)ModifyOrder()

| async void ATAS.Strategies.Strategy.ModifyOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | neworder, |
| | | bool | isAutomated = `true` |
| | ) | | |

Modifies an order.

Parameters

| order | The order to modify. |
| --- | --- |
| neworder | The order with the new parameters. |
| isAutomated | A flag indicating whether the order is placed automatically or by the user. |

## [◆](https://docs.atas.net/en/)ModifyOrderAsync()

| async Task ATAS.Strategies.Strategy.ModifyOrderAsync | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | neworder, |
| | | bool | isAutomated = `true` |
| | ) | | |

Modifies an order.

Parameters

| order | The order to modify. |
| --- | --- |
| neworder | The order with the new parameters. |
| isAutomated | A flag indicating whether the order is placed automatically or by the user. |

## [◆](https://docs.atas.net/en/)OnBestBidAsk()

| virtual void ATAS.Strategies.Strategy.OnBestBidAsk | ( | [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | depth | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when the best bid or ask market depth data is received.

Parameters

| depth | The best bid or ask market depth data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnCancelOrder()

| virtual Task ATAS.Strategies.Strategy.OnCancelOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated |
| | ) | | |

protectedvirtual

Called when an order is canceled.

Parameters

| order | The order to be canceled. |
| --- | --- |
| isAutomated | A flag indicating whether the order was canceled automatically. |

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ad239e5d34d8d75ad5c63276c19364e89).

## [◆](https://docs.atas.net/en/)OnCurrentPositionChanged()

| virtual void ATAS.Strategies.Strategy.OnCurrentPositionChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the volume of the current position changes.

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a2520687829c28c320d925427f6b25a5d).

## [◆](https://docs.atas.net/en/)OnMarketDepth()

| virtual void ATAS.Strategies.Strategy.OnMarketDepth | ( | IEnumerable | depths | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when market depth data is received.

Parameters

| depths | The collection of MarketDepth data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnModifyOrder()

| virtual Task ATAS.Strategies.Strategy.OnModifyOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder, |
| | | bool | isAutomated |
| | ) | | |

protectedvirtual

Called when an existing order is modified.

Parameters

| order | The original order. |
| --- | --- |
| newOrder | The modified order with new parameters. |
| isAutomated | A flag indicating whether the order was modified automatically. |

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a899f91073ddab1029d5a5706cb20cdaf).

## [◆](https://docs.atas.net/en/)OnNewMyTrade()

| virtual void ATAS.Strategies.Strategy.OnNewMyTrade | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | myTrade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when a new trade is added to the collection of MyTrade.

Parameters

| myTrade | The newly added MyTrade. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnNewOrder()

| virtual void ATAS.Strategies.Strategy.OnNewOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when a new order is added.

Parameters

| order | The newly added order. |
| --- | --- |

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa36961b5dcd5d13474f766ed0c953f54).

## [◆](https://docs.atas.net/en/)OnNewPortfolio()

| virtual void ATAS.Strategies.Strategy.OnNewPortfolio | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when a new portfolio is added.

Parameters

| portfolio | The newly added portfolio. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnNewPosition()

| virtual void ATAS.Strategies.Strategy.OnNewPosition | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when a new position is added.

Parameters

| position | The newly added position. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnNewTrade()

| virtual void ATAS.Strategies.Strategy.OnNewTrade | ( | [Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when a new trade occurs.

Parameters

| trade | The newly occurred trade. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnOpenOrder()

| virtual Task ATAS.Strategies.Strategy.OnOpenOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated |
| | ) | | |

protectedvirtual

Called when a new order is opened.

Parameters

| order | The newly opened order. |
| --- | --- |
| isAutomated | A flag indicating whether the order was opened automatically. |

## [◆](https://docs.atas.net/en/)OnOrderCancelFailed()

| virtual void ATAS.Strategies.Strategy.OnOrderCancelFailed | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | string | message |
| | ) | | |

protectedvirtual

Called when an order cancellation fails.

Parameters

| order | The order that failed to cancel. |
| --- | --- |
| message | The error message. |

## [◆](https://docs.atas.net/en/)OnOrderChanged()

| virtual void ATAS.Strategies.Strategy.OnOrderChanged | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when an existing order is changed.

Parameters

| order | The changed order. |
| --- | --- |

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a25c70530bf4bddf14a72b84c53ecb6bb).

## [◆](https://docs.atas.net/en/)OnOrderModifyFailed()

| virtual void ATAS.Strategies.Strategy.OnOrderModifyFailed | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder, |
| | | string | message |
| | ) | | |

protectedvirtual

Called when an order modification fails.

Parameters

| order | The order that failed to modify. |
| --- | --- |
| newOrder | The modified order with new parameters. |
| message | The error message. |

## [◆](https://docs.atas.net/en/)OnOrderRegisterFailed()

| virtual void ATAS.Strategies.Strategy.OnOrderRegisterFailed | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | string | message |
| | ) | | |

protectedvirtual

Called when an order registration fails.

Parameters

| order | The order that failed to register. |
| --- | --- |
| message | The error message. |

## [◆](https://docs.atas.net/en/)OnPnLChanged()

| virtual void ATAS.Strategies.Strategy.OnPnLChanged | ( | int | ticks | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when the profit and loss (PnL) changes.

Parameters

| ticks | The number of ticks by which the PnL changes. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnPositionChanged()

| virtual void ATAS.Strategies.Strategy.OnPositionChanged | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when an existing position is changed.

Parameters

| position | The changed position. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnStarted()

| virtual Task ATAS.Strategies.Strategy.OnStarted | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy is started from StrategyStates.Stopped state.

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a693a5e82857944e5b9bb85579e43fe00).

## [◆](https://docs.atas.net/en/)OnStartedFromWatch()

| virtual Task ATAS.Strategies.Strategy.OnStartedFromWatch | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy is started from StrategyStates.Watch state.

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adf3fb846efbee406351f0e55131939f1).

## [◆](https://docs.atas.net/en/)OnStopped()

| virtual Task ATAS.Strategies.Strategy.OnStopped | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy is stopped.

## [◆](https://docs.atas.net/en/)OnStopping()

| virtual Task ATAS.Strategies.Strategy.OnStopping | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy is stopping.

Reimplemented in [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a632892c7ccee633506067a6ff11fdaff).

## [◆](https://docs.atas.net/en/)OnUpdateStrategyState()

| virtual void ATAS.Strategies.Strategy.OnUpdateStrategyState | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy state needs to be updated.

## [◆](https://docs.atas.net/en/)OpenOrder()

| async void ATAS.Strategies.Strategy.OpenOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated = `true` |
| | ) | | |

Opens an order.

Parameters

| order | The order to open. |
| --- | --- |
| isAutomated | A flag indicating whether the order is placed automatically or by the user. |

## [◆](https://docs.atas.net/en/)OpenOrderAsync()

| async Task ATAS.Strategies.Strategy.OpenOrderAsync | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated = `true` |
| | ) | | |

Opens an order.

Parameters

| order | The order to open. |
| --- | --- |
| isAutomated | A flag indicating whether the order is placed automatically or by the user. |

## [◆](https://docs.atas.net/en/)RaisePropertyChanged()

| void ATAS.Strategies.Strategy.RaisePropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PropertyChanged event with the specified property name.

Parameters

| propertyName | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RaiseShowNotification()

| void ATAS.Strategies.Strategy.RaiseShowNotification | ( | string | message, |
| --- | --- | --- | --- |
| | | string | title = `null`, |
| | | bool | isError = `false` |
| | ) | | |

protected

Raises the ShowNotification event with the specified message, title, and error flag.

Parameters

| message | The notification message. |
| --- | --- |
| title | The notification title (optional). |
| isError | A flag indicating whether the notification is an error. |

## [◆](https://docs.atas.net/en/)ResetErrorState()

| void ATAS.Strategies.Strategy.ResetErrorState | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Reset error state.

## [◆](https://docs.atas.net/en/)SetErrorState()

| void ATAS.Strategies.Strategy.SetErrorState | ( | [StrategyErrorTypes](../namespaces/namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271) | type, |
| --- | --- | --- | --- |
| | | string[] | errorDescriptions |
| | ) | | |

protected

Set StrategyStates.Error state.

Parameters

| type | Error type. |
| --- | --- |
| errorDescriptions | Error descriptions. |

## [◆](https://docs.atas.net/en/)SetProperty()

| bool ATAS.Strategies.Strategy.SetProperty | ( | ref TValue | storage, |
| --- | --- | --- | --- |
| | | TValue | newValue, |
| | | string | propertyName, |
| | | Action | onChanged = `null` |
| | ) | | |

protected

Sets the property with the specified name to the new value and raises the PropertyChanged event if the value has changed.

Template Parameters

| TValue | The type of the property value. |
| --- | --- |

Parameters

| storage | A reference to the storage for the property value. |
| --- | --- |
| newValue | The new value of the property. |
| propertyName | The name of the property. |
| onChanged | An optional action to execute when the property value changes. |

Returns`true` if the property value has changed; otherwise, `false`.

## [◆](https://docs.atas.net/en/)SetState()

| void ATAS.Strategies.Strategy.SetState | ( | [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) | state | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Set strategy state.

Parameters

| state | New state. |
| --- | --- |

## [◆](https://docs.atas.net/en/)Start()

| void ATAS.Strategies.Strategy.Start | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)StartAsync()

| async Task ATAS.Strategies.Strategy.StartAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Starts the strategy, allowing it to execute its trading logic.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a8b8803a4d0eff2da7088e4f0b3b4d25a).

## [◆](https://docs.atas.net/en/)StartFromWatchAsync()

| async Task ATAS.Strategies.Strategy.StartFromWatchAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Stop()

| void ATAS.Strategies.Strategy.Stop | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)StopAsync()

| async Task ATAS.Strategies.Strategy.StopAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Stops the strategy, terminating its execution and releasing any resources.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a83d83423aa3f8a5248d681b4bb5126ad).

## [◆](https://docs.atas.net/en/)UpdateCurrentPosition()

| void ATAS.Strategies.Strategy.UpdateCurrentPosition | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Update thr CurrentPosition and AveragePrice values.

## [◆](https://docs.atas.net/en/)WatchAsync()

| async Task ATAS.Strategies.Strategy.WatchAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AveragePrice

| decimal ATAS.Strategies.Strategy.AveragePrice |
| --- |

get

Gets the average price of the strategy's trades.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a0990ad1aaa4097b121e2f9963026a3a2).

## [◆](https://docs.atas.net/en/)BestAsk

| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) ATAS.Strategies.Strategy.BestAsk |
| --- |

get

## [◆](https://docs.atas.net/en/)BestBid

| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) ATAS.Strategies.Strategy.BestBid |
| --- |

get

## [◆](https://docs.atas.net/en/)ClosedPnL

| decimal ATAS.Strategies.Strategy.ClosedPnL |
| --- |

get

Gets the closed profit and loss of the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a7b734a3537121bb3524c16cc94d5d8a2).

## [◆](https://docs.atas.net/en/)Connector

| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) ATAS.Strategies.Strategy.Connector |
| --- |

getset

Gets or sets the data feed connector for the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#ac42fb5c1c25cb4734acaa356371c6505).

## [◆](https://docs.atas.net/en/)CurrentPosition

| decimal ATAS.Strategies.Strategy.CurrentPosition |
| --- |

get

Gets the current position volume of the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#aa3c8e8450cedf2e6dc772cee79022057).

## [◆](https://docs.atas.net/en/)MyTrades

| IEnumerable ATAS.Strategies.Strategy.MyTrades |
| --- |

get

## [◆](https://docs.atas.net/en/)Name

| string ATAS.Strategies.Strategy.Name |
| --- |

getset

Gets or sets the name of the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a82fa08d68c09e1ac4b699ed5901408c3).

## [◆](https://docs.atas.net/en/)OpenPnL

| decimal ATAS.Strategies.Strategy.OpenPnL |
| --- |

get

Gets the open profit and loss of the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a21cfcaa3598adf5c9a909fa1d87e08c1).

## [◆](https://docs.atas.net/en/)OpenTicksPnL

| int ATAS.Strategies.Strategy.OpenTicksPnL |
| --- |

get

## [◆](https://docs.atas.net/en/)Orders

| IEnumerable ATAS.Strategies.Strategy.Orders |
| --- |

get

## [◆](https://docs.atas.net/en/)Portfolio

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.Strategies.Strategy.Portfolio |
| --- |

getset

Gets or sets the portfolio associated with the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a2ac811e04e280f17e9a46aa376122326).

## [◆](https://docs.atas.net/en/)Position

| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.Strategies.Strategy.Position |
| --- |

getset

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.Strategies.Strategy.Security |
| --- |

getset

Gets or sets the security associated with the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a1d6806a24b84f596cd7e4adf8a315edd).

## [◆](https://docs.atas.net/en/)State

| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) ATAS.Strategies.Strategy.State |
| --- |

getprotected set

Gets the current state of the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a18fcdb4e7e4a8d94e285cb45d1742b5e).

## [◆](https://docs.atas.net/en/)StateDescription

| [StrategyStateDescription](../namespaces/namespaceATAS_1_1Strategies.md#ac2b219bf1e9f7c99f8f9fc4c5fe39ea3) ATAS.Strategies.Strategy.StateDescription |
| --- |

get

## [◆](https://docs.atas.net/en/)TPlusLimit

| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? ATAS.Strategies.Strategy.TPlusLimit |
| --- |

getset

Gets or sets the T+ limits for the strategy.

Implements [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a1c4dd7746f51fdd6dc9033ec8c152494).

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.Strategies.Strategy.PropertyChanged |
| --- |

## [◆](https://docs.atas.net/en/)ShowNotification

| EventHandler ATAS.Strategies.Strategy.ShowNotification |
| --- |

## [◆](https://docs.atas.net/en/)StateChanged

| EventHandler ATAS.Strategies.Strategy.StateChanged |
| --- |

The documentation for this class was generated from the following file:
- [Strategy.cs](../files/Strategy_8cs.md)
