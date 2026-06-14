# ATAS.Indicators.ITradingManager Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1ITradingManager.html

Interface representing a trading manager for handling trading-related operations.
 [More...](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#details)

| Public Member Functions | |
| --- | --- |
| void | [OpenOrder](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#abcbb69b0873d1bc631d543c91ac05383) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, bool setDefaultQuantity, bool askConfirmation=true, bool checkOrderStates=true) |
| | Opens a new order for trading. |
| | |
| Task | [OpenOrderAsync](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a0a8c1cc54ffc4a4f9899dbf3de09dd5f) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, bool setDefaultQuantity, bool askConfirmation=true, bool checkOrderStates=true) |
| | Opens a new order for trading. |
| | |
| void | [ModifyOrder](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a0314f0b79c7c1d15a4a323bf36aa7fe5) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool askConfirmation=true, bool checkOrderStates=true) |
| | Modifies an existing order. |
| | |
| Task | [ModifyOrderAsync](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a3cfbc95bdd4bc969a94f5e7c1aa8431d) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool askConfirmation=true, bool checkOrderStates=true) |
| | Modifies an existing order. |
| | |
| void | [CancelOrder](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a84f8e813a6adbc173c1c43813c303248) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, bool askConfirmation=true, bool checkOrderStates=true) |
| | Cancels an existing order. |
| | |
| Task | [CancelOrderAsync](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#aa7a0ef168fe86fb25bb6553a46fa7c3c) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, bool askConfirmation=true, bool checkOrderStates=true) |
| | Cancels an existing order. |
| | |
| bool | [ClosePosition](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a0a893a5f98eaf5e6c053bcd8d5f81a87) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position, bool askConfirmation=true, bool checkOrderStates=true) |
| | Closes the specified position. |
| | |
| Task | [ClosePositionAsync](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a267f1fa625e23fb9e504319272b9e86d) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position, bool askConfirmation=true, bool checkOrderStates=true) |
| | Closes the specified position. |
| | |
| [ISecurityTradingOptions](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? | [GetSecurityTradingOptions](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a5db611a528aecbc23148d81ae98013c4) () |
| | Gets ISecurityTradingOptions for current ITradingManager.Security. |
| | |
| Task | [SetStopLoss](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#aa58eaaffd86b3950d39faffdce13f667) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) value) |
| | Sets a new Stop-Loss value. |
| | |
| Task | [SetTakeProfit](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#aa9c8b7ba55d95e6f646fd1d36d306a3a) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) value) |
| | Sets a new Take-Profit value. |
| | |
| Task | [SetBreakeven](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#abc30943215aab48623bbf6968982247f) () |
| | Sets a Stop-Loss to breakeven. |
| | |
| bool | [IsStopLossOrder](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#ad39e02156901bffa9672a71bdaa63b5a) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Checks if Order is Stop-Loss order. |
| | |
| bool | [IsTakeProfitOrder](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#ab99bd72c7e6385d448434592c268a77a) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Checks if Order is Take-Profit order. |
| | |

| Properties | |
| --- | --- |
| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md)? | [Security](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#aba88b29d2dbac208278571eae8a07948)`[get]` |
| | The selected security. |
| | |
| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md)? | [Portfolio](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#add9ae8028b312d33e2bf9e90b97c6681)`[get]` |
| | The selected portfolio. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a2733cc34b3fce7a6803195b2ecc574b5)`[get]` |
| | The T+ limits for the selected security. |
| | |
| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md)? | [Position](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#ae28cee783ef49765d986ffa308771175)`[get]` |
| | The current position for the selected security and portfolio. |
| | |
| IEnumerable | [MyTrades](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a3911247533ce88e3c920b8a153ac1643)`[get]` |
| | Collection of MyTrade for the selected security and portfolio. |
| | |
| IEnumerable | [Orders](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a2721872e764c5341da42dc13ab0cfe9a)`[get]` |
| | Collection of orders for the selected security and portfolio. |
| | |
| [ITradingVolumeInfo](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md)? | [TradingVolumeInfo](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a220014284af7e9764922ff901abd8bca)`[get]` |
| | Gets ITradingVolumeInfo for current ITradingManager.Security. |
| | |
| bool | [IsStopLossModeActivated](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a069ccf8023d96e506f9c7eecb89e8b6b)`[get]` |
| | Indicates whether the Stop-Loss mode is activated. |
| | |
| bool | [IsTakeProfitModeActivated](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#acae9ba7baa9fbc63a59bacbe39c109af)`[get]` |
| | Indicates whether the Take-Profit mode is activated. |
| | |

| Events | |
| --- | --- |
| Action | [SecuritySelected](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#afb310591c9399eb1230038c3b0195c8c) |
| | Event that is raised when a security is selected. |
| | |
| Action | [PortfolioSelected](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#aa5e276e3c03187bb13b875423229d390) |
| | Event that is raised when a portfolio is selected. |
| | |
| Action | [PortfolioChanged](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a617ee13fbd8c3800156f459ec853580d) |
| | Event that is raised when the selected portfolio is changed. |
| | |
| Action | [PositionChanged](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a330eeba4723553e8d33d3c8194ca938e) |
| | Event that is raised when the position is changed. |
| | |
| Action | [NewOrder](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#aab6c1ca730a15750fa7229aec5d8b2c7) |
| | Event that is raised when a new order is added. |
| | |
| Action | [OrderChanged](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#aa6e933eded35f1b560319ecefbfb874f) |
| | Event that is raised when an order is modified. |
| | |
| Action | [NewMyTrade](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a36b2e015353c44931195155f4f5f7a3b) |
| | Event that is raised when a new MyTrade is added. |
| | |
| Action | [OrderRegisterFailed](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a2181c15855962e44d9874bfda53f54f3) |
| | Event that is raised when order registration fails. |
| | |
| Action | [OrderCancelFailed](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#ab2d9582e532aca12cbd6c12857ec4b5a) |
| | Event that is raised when order cancellation fails. |
| | |
| Action | [OrderModifyFailed](./interfaceATAS_1_1Indicators_1_1ITradingManager.md#a3d3d5544a246b8960e18bdd107abebcf) |
| | Event that is raised when order modification fails. |
| | |

## Detailed Description

Interface representing a trading manager for handling trading-related operations.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CancelOrder()

| void ATAS.Indicators.ITradingManager.CancelOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Cancels an existing order.

Parameters

| order | The order to cancel. |
| --- | --- |
| askConfirmation | Set to `true` to ask for confirmation before cancelling the order. |
| checkOrderStates | Set to `true` to check order states before cancelling the order. |

## [◆](https://docs.atas.net/en/)CancelOrderAsync()

| Task ATAS.Indicators.ITradingManager.CancelOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Cancels an existing order.

Parameters

| order | The order to cancel. |
| --- | --- |
| askConfirmation | Set to `true` to ask for confirmation before cancelling the order. |
| checkOrderStates | Set to `true` to check order states before cancelling the order. |

## [◆](https://docs.atas.net/en/)ClosePosition()

| bool ATAS.Indicators.ITradingManager.ClosePosition | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Closes the specified position.

Parameters

| position | The position to close. |
| --- | --- |
| askConfirmation | Set to `true` to ask for confirmation before closing the position. |
| checkOrderStates | Set to `true` to check order states before closing the position. |

## [◆](https://docs.atas.net/en/)ClosePositionAsync()

| Task ATAS.Indicators.ITradingManager.ClosePositionAsync | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Closes the specified position.

Parameters

| position | The position to close. |
| --- | --- |
| askConfirmation | Set to `true` to ask for confirmation before closing the position. |
| checkOrderStates | Set to `true` to check order states before closing the position. |

## [◆](https://docs.atas.net/en/)GetSecurityTradingOptions()

| [ISecurityTradingOptions](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? ATAS.Indicators.ITradingManager.GetSecurityTradingOptions | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets ISecurityTradingOptions for current ITradingManager.Security.

## [◆](https://docs.atas.net/en/)IsStopLossOrder()

| bool ATAS.Indicators.ITradingManager.IsStopLossOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Checks if Order is Stop-Loss order.

Parameters

| order | The Order to check. |
| --- | --- |

ReturnsReturns `true` if the order is Stop-Loss, otherwise `false`.

## [◆](https://docs.atas.net/en/)IsTakeProfitOrder()

| bool ATAS.Indicators.ITradingManager.IsTakeProfitOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Checks if Order is Take-Profit order.

Parameters

| order | The Order to check. |
| --- | --- |

ReturnsReturns `true` if the order is Take-Profit, otherwise `false`.

## [◆](https://docs.atas.net/en/)ModifyOrder()

| void ATAS.Indicators.ITradingManager.ModifyOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder, |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Modifies an existing order.

Parameters

| order | The order to modify. |
| --- | --- |
| newOrder | The modified order. |
| askConfirmation | Set to `true` to ask for confirmation before modifying the order. |
| checkOrderStates | Set to `true` to check order states before modifying the order. |

## [◆](https://docs.atas.net/en/)ModifyOrderAsync()

| Task ATAS.Indicators.ITradingManager.ModifyOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder, |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Modifies an existing order.

Parameters

| order | The order to modify. |
| --- | --- |
| newOrder | The modified order. |
| askConfirmation | Set to `true` to ask for confirmation before modifying the order. |
| checkOrderStates | Set to `true` to check order states before modifying the order. |

## [◆](https://docs.atas.net/en/)OpenOrder()

| void ATAS.Indicators.ITradingManager.OpenOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | setDefaultQuantity, |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Opens a new order for trading.

Parameters

| order | The order to open. |
| --- | --- |
| setDefaultQuantity | Set to `true` to use default quantity. |
| askConfirmation | Set to `true` to ask for confirmation before opening the order. |
| checkOrderStates | Set to `true` to check order states before opening the order. |

## [◆](https://docs.atas.net/en/)OpenOrderAsync()

| Task ATAS.Indicators.ITradingManager.OpenOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | setDefaultQuantity, |
| | | bool | askConfirmation = `true`, |
| | | bool | checkOrderStates = `true` |
| | ) | | |

Opens a new order for trading.

Parameters

| order | The order to open. |
| --- | --- |
| setDefaultQuantity | Set to `true` to use default quantity. |
| askConfirmation | Set to `true` to ask for confirmation before opening the order. |
| checkOrderStates | Set to `true` to check order states before opening the order. |

## [◆](https://docs.atas.net/en/)SetBreakeven()

| Task ATAS.Indicators.ITradingManager.SetBreakeven | ( | | ) | |
| --- | --- | --- | --- | --- |

Sets a Stop-Loss to breakeven.

## [◆](https://docs.atas.net/en/)SetStopLoss()

| Task ATAS.Indicators.ITradingManager.SetStopLoss | ( | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | value | ) | |
| --- | --- | --- | --- | --- | --- |

Sets a new Stop-Loss value.

Parameters

| value | Stop-Loss value. |
| --- | --- |

## [◆](https://docs.atas.net/en/)SetTakeProfit()

| Task ATAS.Indicators.ITradingManager.SetTakeProfit | ( | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | value | ) | |
| --- | --- | --- | --- | --- | --- |

Sets a new Take-Profit value.

Parameters

| value | Take-Profit value. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)IsStopLossModeActivated

| bool ATAS.Indicators.ITradingManager.IsStopLossModeActivated |
| --- |

get

Indicates whether the Stop-Loss mode is activated.

## [◆](https://docs.atas.net/en/)IsTakeProfitModeActivated

| bool ATAS.Indicators.ITradingManager.IsTakeProfitModeActivated |
| --- |

get

Indicates whether the Take-Profit mode is activated.

## [◆](https://docs.atas.net/en/)MyTrades

| IEnumerable ATAS.Indicators.ITradingManager.MyTrades |
| --- |

get

Collection of MyTrade for the selected security and portfolio.

## [◆](https://docs.atas.net/en/)Orders

| IEnumerable ATAS.Indicators.ITradingManager.Orders |
| --- |

get

Collection of orders for the selected security and portfolio.

## [◆](https://docs.atas.net/en/)Portfolio

| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md)? ATAS.Indicators.ITradingManager.Portfolio |
| --- |

get

The selected portfolio.

## [◆](https://docs.atas.net/en/)Position

| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md)? ATAS.Indicators.ITradingManager.Position |
| --- |

get

The current position for the selected security and portfolio.

## [◆](https://docs.atas.net/en/)Security

| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md)? ATAS.Indicators.ITradingManager.Security |
| --- |

get

The selected security.

## [◆](https://docs.atas.net/en/)TPlusLimit

| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? ATAS.Indicators.ITradingManager.TPlusLimit |
| --- |

get

The T+ limits for the selected security.

## [◆](https://docs.atas.net/en/)TradingVolumeInfo

| [ITradingVolumeInfo](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md)? ATAS.Indicators.ITradingManager.TradingVolumeInfo |
| --- |

get

Gets ITradingVolumeInfo for current ITradingManager.Security.

## Event Documentation

## [◆](https://docs.atas.net/en/)NewMyTrade

| Action ATAS.Indicators.ITradingManager.NewMyTrade |
| --- |

Event that is raised when a new MyTrade is added.

## [◆](https://docs.atas.net/en/)NewOrder

| Action ATAS.Indicators.ITradingManager.NewOrder |
| --- |

Event that is raised when a new order is added.

## [◆](https://docs.atas.net/en/)OrderCancelFailed

| Action ATAS.Indicators.ITradingManager.OrderCancelFailed |
| --- |

Event that is raised when order cancellation fails.

## [◆](https://docs.atas.net/en/)OrderChanged

| Action ATAS.Indicators.ITradingManager.OrderChanged |
| --- |

Event that is raised when an order is modified.

## [◆](https://docs.atas.net/en/)OrderModifyFailed

| Action ATAS.Indicators.ITradingManager.OrderModifyFailed |
| --- |

Event that is raised when order modification fails.

## [◆](https://docs.atas.net/en/)OrderRegisterFailed

| Action ATAS.Indicators.ITradingManager.OrderRegisterFailed |
| --- |

Event that is raised when order registration fails.

## [◆](https://docs.atas.net/en/)PortfolioChanged

| Action ATAS.Indicators.ITradingManager.PortfolioChanged |
| --- |

Event that is raised when the selected portfolio is changed.

## [◆](https://docs.atas.net/en/)PortfolioSelected

| Action ATAS.Indicators.ITradingManager.PortfolioSelected |
| --- |

Event that is raised when a portfolio is selected.

## [◆](https://docs.atas.net/en/)PositionChanged

| Action ATAS.Indicators.ITradingManager.PositionChanged |
| --- |

Event that is raised when the position is changed.

## [◆](https://docs.atas.net/en/)SecuritySelected

| Action ATAS.Indicators.ITradingManager.SecuritySelected |
| --- |

Event that is raised when a security is selected.

The documentation for this interface was generated from the following file:
- [ITradingManager.cs](../files/ITradingManager_8cs.md)
