# Managing orders in trading

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20110__ManagingOrders.html

The main methods for managing orders include methods such as [OpenOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#add7c48f9ede9667aa7ba8a0f97344eab), [ModifyOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a01509c1de2aadb7994a819e41a5db29e), [CancelOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a3808947989c50df10ee357f597a2171a). By calling these methods, developers are essentially providing execution instructions. However, the exchange / trading connection does not guarantee that this instruction will be followed. They can be fulfilled completely, partially or with deviations.

To obtain information about what is happening with orders, developers can override methods such as [OnOrderChanged](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16), [OnOrderRegisterFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a2ea44a2007103545463be80f85b515b5), [OnOrderModifyFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a2a206383275db0490ffadcfc6580425e), [OnOrderCancelFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a733aec75e1b68396ff38bafc9de74af6).

It is important to understand that these methods cause less response from the exchange/trading connection and may be on different threads and may sometimes use [sync methods](https://learn.microsoft.com/en-us/dotnet/standard/threading/overview-of-synchronization-primitives).

# Using synchronous methods

## Order placing

To place an order, use the [OpenOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#add7c48f9ede9667aa7ba8a0f97344eab) method, to which the filled [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) entity is passed.

An example of placing a market buy order with a volume of 1 lot:

var order = new Order

{

 Portfolio = Portfolio,

 Security = Security,

 Direction = OrderDirections.Buy,

 Type = OrderTypes.Market,

 QuantityToFill = 1,

};

OpenOrder(order);

 An example of placing a limit order to sell at a price of 100 with a volume of 1 lot:

var order = new Order

{

 Portfolio = Portfolio,

 Security = Security,

 Direction = OrderDirections.Sell,

 Type = OrderTypes.Limit,

 Price = 100,

 QuantityToFill = 1,

};

OpenOrder(order);

 An example of placing a stop order to sell at a price of 100 with a volume of 1 lot:

var order = new Order

{

 Portfolio = Portfolio,

 Security = Security,

 Direction = OrderDirections.Sell,

 Type = OrderTypes.Stop,

 TriggerPrice = 100,

 QuantityToFill = 1,

};

OpenOrder(order);

 An example of placing a stop-limit order to sell at an activation price of 100 and a maximum execution price of 95, with a volume of 1 lot:

var order = new Order

{

 Portfolio = Portfolio,

 Security = Security,

 Direction = OrderDirections.Sell,

 Type = OrderTypes.StopLimit,

 TriggerPrice = 100,

 Price = 95,

 QuantityToFill = 1,

};

OpenOrder(order);

The [OpenOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#add7c48f9ede9667aa7ba8a0f97344eab) method does not guarantee that an order will be placed, it only sends the order to the exchange. The sending result should be viewed in the [OnOrderChanged](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16) and [OnOrderRegisterFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a2ea44a2007103545463be80f85b515b5) methods.

If the exchange rejects the application for placing an order, the [OnOrderRegisterFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a2ea44a2007103545463be80f85b515b5) method will be called, which will receive a link to the original placed order and a message from the exchange.

In the process of placing an order and accepting it by the exchange, the exchange sends updates to this order. All order updates are sent to [OnOrderChanged](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16). Through [order.Status()](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b) you can get the current state of the order.

An example of placing an order and controlling its placement:

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.Strategies.Chart](../api/namespaces/namespaceATAS_1_1Strategies_1_1Chart.md);

public class SampleStrategy : [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md)

{

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _order;

 private bool _firstTime = true;

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == CurrentBar - 1 && _firstTime)

 {

 _firstTime = false;

 _order = new [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

 {

 [Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) = [Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md),

 [Security](../api/classes/classATAS_1_1DataFeedsCore_1_1Security.md) = [Security](../api/classes/classATAS_1_1DataFeedsCore_1_1Security.md),

 Direction = [OrderDirections](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c).Buy,

 Type = [OrderTypes](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8).Limit,

 Price = 100,

 QuantityToFill = 1,

 };

 OpenOrder(_order);

 }

 }

 protected override void OnOrderRegisterFailed([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, string message)

 {

 if (order == _order)

 {

 // order placement error, see message for details.

 }

 }

 protected override void OnOrderChanged([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order)

 {

 if (order == _order)

 {

 switch (order.Status())

 {

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).None:

 // The order has an undefined status (you need to wait for the next method calls).

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Placed:

 // the order is placed.

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Filled:

 // the order is filled.

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).PartlyFilled:

 // the order is partially filled.

 {

 var unfilled = order.[Unfilled](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a44c573eb702b2f38ed8b8c80e19ac5a1); // this is a unfilled volume.

 break;

 }

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Canceled:

 // the order is canceled.

 break;

 }

 }

 }

}

[ATAS.DataFeedsCore.Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

Represents an order for trading on a financial exchange.

Definition Order.cs:13

[ATAS.DataFeedsCore.Order.Unfilled](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a44c573eb702b2f38ed8b8c80e19ac5a1)

decimal Unfilled

Gets the remaining unfilled volume of the order.

Definition Order.cs:316

[ATAS.DataFeedsCore.Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md)

Represents a portfolio entity with various properties related to account balance, Profit and Loss (Pn...

Definition Portfolio.cs:25

[ATAS.DataFeedsCore.Security](../api/classes/classATAS_1_1DataFeedsCore_1_1Security.md)

Represents a security entity used in the application.

Definition Security.cs:21

[ATAS.Strategies.Chart.ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md)

Represents an abstract class for a chart strategy that extends the functionality of an Indicator and ...

Definition ChartStrategy.cs:22

[ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md)

Definition AsyncConnector.cs:7

[ATAS.DataFeedsCore.OrderTypes](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8)

OrderTypes

Specifies the type of an order.

Definition OrderTypes.cs:7

[ATAS.DataFeedsCore.OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b)

OrderStatus

Represents the possible status of an order.

Definition OrderStates.cs:33

[ATAS.DataFeedsCore.OrderDirections](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c)

OrderDirections

Specifies the direction of an order.

Definition OrderDirections.cs:7

[ATAS.Strategies.Chart](../api/namespaces/namespaceATAS_1_1Strategies_1_1Chart.md)

Definition ChartStrategy.cs:1

## Order changing

To change an placed order, you need to create a copy of this order, change the price or volume in it and call [ModifyOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a01509c1de2aadb7994a819e41a5db29e), passing the old order and the new one as parameters. We also need to remember that if we store a link to the old order, then when modifying we need to assign our field a link to the new order.

If the [OnOrderModifyFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a2a206383275db0490ffadcfc6580425e) method receives an order previously sent for modification, then the modification was impossible for some reason.

The order change status should be tracked in [OnOrderChanged](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16).

In the example of changing an order, it is assumed that the `_order` object contains an order that is active at the time of launch:

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.Strategies.Chart](../api/namespaces/namespaceATAS_1_1Strategies_1_1Chart.md);

public class SampleStrategy : [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md)

{

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _order;

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _newOrder;

 private bool _firstTime = true;

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == CurrentBar - 1 && _firstTime)

 {

 // Here we receive a signal and open an order.

 // Here the conditions under which you need to change the order price have been met.

 _newOrder = _order.[Clone](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#add3d16e72bec14afd534d7c25146f0bb)();

 _newOrder.Price = 200;

 ModifyOrder(_order, _newOrder);

 }

 }

 protected override void OnOrderModifyFailed([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, string error)

 {

 if (order == _order)

 {

 // Here it will be clear that it was not possible to change _order.

 }

 }

 protected override void OnOrderChanged([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order)

 {

 if (order == _newOrder)

 {

 switch (order.Status())

 {

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).None:

 // The order has an undefined status (you need to wait for the next method calls).

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Placed:

 // the new order has been placed, the change was successful.

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Filled:

 // the order was placed and filled.

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).PartlyFilled:

 // the order is partially filled.

 {

 var unfilled = order.[Unfilled](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a44c573eb702b2f38ed8b8c80e19ac5a1); // this is a unfilled volume.

 break;

 }

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Canceled:

 // the order is canceled.

 break;

 }

 }

 }

}

[ATAS.DataFeedsCore.Order.Clone](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#add3d16e72bec14afd534d7c25146f0bb)

Order Clone()

Creates a shallow copy of the current order.

Definition Order.cs:638

## Order canceling

[CancelOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a3808947989c50df10ee357f597a2171a) sends an instruction to the exchange to cancel the order. There is also no guarantee of successful cancellation, so you need to track the status of orders in the [OnOrderCancelFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a733aec75e1b68396ff38bafc9de74af6) and [OnOrderChanged](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16) methods.

Before sending a [CancelOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a3808947989c50df10ee357f597a2171a), it is recommended to ensure that the [State](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a5c93a3af2be3df8bdb85944b42eb4527) of the order is equal to [OrderStates.Active](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716).

In the order cancellation example, it is assumed that the `_order` object contains an order that is active at the time of launch:

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.Strategies.Chart](../api/namespaces/namespaceATAS_1_1Strategies_1_1Chart.md);

public class SampleStrategy : [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md)

{

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _order;

 private bool _firstTime = true;

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == CurrentBar - 1 && _firstTime)

 {

 // Here the conditions under which we need to cancel our order have been met.

 if (_order.[State](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a5c93a3af2be3df8bdb85944b42eb4527) == [OrderStates](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716).Active)

 {

 CancelOrder(_order);

 _firstTime = false;

 }

 }

 }

 protected override void OnOrderCancelFailed([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, string message)

 {

 if (order == _order)

 {

 // Here it will be clear that cancel _order did not work.

 }

 }

 protected override void OnOrderChanged([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order)

 {

 if (order == _order && order.Status() == [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).[Canceled](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#adf869a13c842cedd0d85c54ba69c1aca))

 {

 // Here it will be clear that the _order was successfully canceled.

 }

 }

}

[ATAS.DataFeedsCore.Order.State](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a5c93a3af2be3df8bdb85944b42eb4527)

OrderStates State

Gets or sets the current state of the order.

Definition Order.cs:502

[ATAS.DataFeedsCore.Order.Canceled](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#adf869a13c842cedd0d85c54ba69c1aca)

bool Canceled

Gets a value indicating whether the order is canceled.

Definition Order.cs:570

[ATAS.DataFeedsCore.OrderStates](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716)

OrderStates

Represents the possible states of an order.

Definition OrderStates.cs:7

# Using asynchronous methods

You can use asynchronous order management methods just like synchronous ones, and getting the results becomes more convenient.

Now there is no need to track the result in the [OnNewOrder](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#abcf82eac51543b99ff8b97d52785b9fc), [OnOrderChanged](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16), [OnOrderRegisterFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a2ea44a2007103545463be80f85b515b5), [OnOrderModifyFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a2a206383275db0490ffadcfc6580425e) and [OnOrderCancelFailed](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a733aec75e1b68396ff38bafc9de74af6) methods. Opening, canceling or modifying an order must be placed in a `try-catch` block.

It must be remembered that the result can still be both successful and unsuccessful. If the result was successful, in the code below we can find out the status of the order or perform other necessary actions. If the result is unsuccessful, an exception is thrown and we can intercept it in the `catch` block.

It should also be noted that if you have active limit or stop limit orders, the moment when their state or status changes can be found in [OnOrderChanged](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16).

## Order placing

Using [OpenOrderAsync](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a2c7f2e85bf2f50e4d49397eccbba4e0f):

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.Strategies.Chart](../api/namespaces/namespaceATAS_1_1Strategies_1_1Chart.md);

public class SampleStrategy : [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md)

{

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _order;

 private bool _firstTime = true;

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == CurrentBar - 1 && _firstTime)

 {

 _firstTime = false;

 _order = new [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

 {

 [Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) = [Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md),

 [Security](../api/classes/classATAS_1_1DataFeedsCore_1_1Security.md) = [Security](../api/classes/classATAS_1_1DataFeedsCore_1_1Security.md),

 Direction = [OrderDirections](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c).Buy,

 Type = [OrderTypes](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8).Limit,

 Price = 100,

 QuantityToFill = 1,

 };

 OpenPositionAsync(_order);

 }

 }

 private async Task OpenPositionAsync([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order)

 {

 try

 {

 await OpenOrderAsync(order);

 // If there was no error and the order was placed, below we can track the positive result of placing the order.

 switch (order.Status())

 {

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Placed:

 // the order is placed.

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).Filled:

 // the order is filled.

 break;

 case [OrderStatus](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b).PartlyFilled:

 // the order is partially filled.

 var unfilled = order.[Unfilled](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a44c573eb702b2f38ed8b8c80e19ac5a1); // this is an unfilled volume.

 break;

 }

 }

 catch (Exception ex)

 {

 // Here we find out that the order could not be opened.

 // As an option, we will output an error message to the log.

 this.LogInfo($"Open position error: {ex.Message}.");

 }

 }

}

## Order changing

Using [ModifyOrderAsync](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#abc6a4b39d9ba1383fcfda9ce45588085):

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.Strategies.Chart](../api/namespaces/namespaceATAS_1_1Strategies_1_1Chart.md);

public class SampleStrategy : [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md)

{

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _order;

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _newOrder;

 private bool _firstTime = true;

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == CurrentBar - 1 && _firstTime)

 {

 // Here we receive a signal and open an order.

 // Here the conditions under which you need to change the order price have been met.

 ChangeOrderAsync();

 }

 }

 private async Task ChangeOrderAsync()

 {

 _newOrder = _order.[Clone](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#add3d16e72bec14afd534d7c25146f0bb)();

 _newOrder.Price = 200;

 try

 {

 await ModifyOrderAsync(_order, _newOrder);

 // Here we find out that the order changes were successful.

 }

 catch (Exception ex)

 {

 // Here we find out that the order could not be changed.

 }

 }

}

## Order canceling

Using [CancelOrderAsync](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a9f15690af8db637605ba5a7aaeaa5deb):

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.Strategies.Chart](../api/namespaces/namespaceATAS_1_1Strategies_1_1Chart.md);

public class SampleStrategy : [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md)

{

 private [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) _order;

 private bool _firstTime = true;

 protected override void OnCalculate(int bar, decimal value)

 {

 if (bar == CurrentBar - 1 && _firstTime)

 {

 // Here the conditions under which we need to cancel our order have been met.

 if (_order.[State](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a5c93a3af2be3df8bdb85944b42eb4527) == [OrderStates](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716).Active)

 {

 TryCancelOrderAsync();

 _firstTime = false;

 }

 }

 }

 private async Task TryCancelOrderAsync()

 {

 try

 {

 await CancelOrderAsync(_order);

 // Here we know that the order was canceled successfully.

 }

 catch (Exception ex)

 {

 // Here we find out that the order could not be canceled.

 }

 }

}

# Points to consider

Since the API provides an asynchronous mechanism for working with trading functions, this must be taken into account during development.

Some common mistakes when working with an asynchronous approach:

- In the strategy, a trigger to cancel an order is triggered, and the algorithm calls [CancelOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a3808947989c50df10ee357f597a2171a). While the response has not arrived from the exchange, the trigger fires again and the algorithm again sends the [CancelOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a3808947989c50df10ee357f597a2171a) request to the exchange. As a result, the second request will be rejected by the exchange.

- The strategy fires a trigger to open a position, the algorithm checks [CurrentPosition](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a9bf909bec2b2dbc0eabd721c212285ff), which is equal to zero, and sends a market order. While the response has not come from the exchange and the value of the position has not been updated, the trigger fires again and another market order is sent, which ultimately leads to an increase in risks.

- In the strategy, a trigger to close a position is triggered, the algorithm checks [CurrentPosition](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a9bf909bec2b2dbc0eabd721c212285ff), creates a closing order with a volume equal to [CurrentPositions](../api/classes/classATAS_1_1Strategies_1_1Strategy.md#a9bf909bec2b2dbc0eabd721c212285ff), and sends this order. While the response has not come from the exchange and the value of the position has not been updated, the trigger fires again and another market order is sent, which ultimately leads not to the closing of the position, but to its reversal. It is important to understand that order statuses and positions arrive asynchronously as a response is received from the exchange.
