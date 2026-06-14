# Working with trading events

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20030__IndicatorEvents.html

It is necessary to refer to the TradingInfo property for getting the data about trading entities.

This property includes:

- [Security](../api/classes/classATAS_1_1DataFeedsCore_1_1Security.md) - the selected instrument

- [Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) - the selected portfolio

- [Position](../api/classes/classATAS_1_1DataFeedsCore_1_1Position.md) - the current position

- [MyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) - the executed trades

- [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) - the posted orders

Besides, it is possible to redefine the following methods for receiving updates:

- [void OnNewOrder(Order order)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a71dbe6190be4e672674eb69bfd7cf363) - new order

- [void OnOrderChanged(Order order)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a9824dc6d293046095e4c57174bea1d47) - order change

- [void OnNewMyTrade(MyTrade myTrade)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#ac98aac74611a0eb14ccdebc8f17c8ad3) - new trade

- [void OnPortfolioChanged(Portfolio portfolio)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a7768bfd7fee4b30b2d7da82d9bb54359) - portfolio change

- [void OnPositionChanged(Position position)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a967d30114b76bb1d33e3ea91a2be1a2d) - position change

Below is an example of the indicator, which displays the portfolio, position, order and trade data. The indicator also adds entries into logs when receiving new orders and new trades when the portfolio or position changes.

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class SampleTradingInfo : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 public SampleTradingInfo()

 {

 EnableCustomDrawing = true;

 SubscribeToDrawingEvents([DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92).Final);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnNewOrder([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) orders)

 {

 this.LogWarn(order.ToString());

 }

 protected override void OnOrderChanged([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order)

 {

 this.LogWarn(order.[ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a79d320efc536a7e8c6eed83c92277455)());

 }

 protected override void OnNewMyTrade([MyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) myTrade)

 {

 this.LogWarn(myTrade.[ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md#af10fb846368f0eae411f0fb6d446168b)());

 }

 protected override void OnPortfolioChanged([Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio)

 {

 this.LogWarn(portfolio.[ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md#ac14ab04890eefbfeb86ca4be5a3a5c38)());

 }

 protected override void OnPositionChanged([Position](../api/classes/classATAS_1_1DataFeedsCore_1_1Position.md) position)

 {

 this.LogWarn(position.[ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1Position.md#ad17aedb45be9f448df7283a717412271)());

 }

 protected override void OnRender(RenderContext context, [DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout)

 {

 var label = "";

 if (TradingManager.Security != null)

 label += $"Security: {TradingManager.Security}{Environment.NewLine}";

 if (TradingManager.Portfolio != null)

 label += $"Portfolio: {TradingManager.Portfolio}{Environment.NewLine}";

 if (TradingManager.Position != null)

 label += $"Position: {TradingManager.Position}{Environment.NewLine}";

 var orders = TradingManager.Orders.Where(t => t.State == [OrderStates](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716).Active);

 if (orders.Any())

 {

 label += $"{Environment.NewLine}---------------------Active orders:------------------------{Environment.NewLine}";

 foreach (var order in orders)

 {

 label += $"{order}{Environment.NewLine}";

 }

 }

 var myTrades = TradingManager.MyTrades;

 if (myTrades.Any())

 {

 label += $"{Environment.NewLine}---------------------MyTrades:------------------------{Environment.NewLine}";

 foreach (var myTrade in myTrades)

 {

 label += $"{myTrade}{Environment.NewLine}";

 }

 }

 var font = new RenderFont("Arial", 10);

 var size = context.MeasureString(label, font);

 context.FillRectangle(Color.DarkRed, new [Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)(25, 25, (int)size.Width + 50, (int)size.Height + 50));

 context.DrawString(label, font, Color.Azure, 50, 50);

 }

}

[ATAS.DataFeedsCore.MyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md)

Represents a trade entity in the system.

Definition MyTrade.cs:19

[ATAS.DataFeedsCore.MyTrade.ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md#af10fb846368f0eae411f0fb6d446168b)

override string ToString()

Definition MyTrade.cs:228

[ATAS.DataFeedsCore.Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

Represents an order for trading on a financial exchange.

Definition Order.cs:13

[ATAS.DataFeedsCore.Order.ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md#a79d320efc536a7e8c6eed83c92277455)

override string ToString()

Returns a string that represents the current order.

Definition Order.cs:647

[ATAS.DataFeedsCore.Portfolio](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md)

Represents a portfolio entity with various properties related to account balance, Profit and Loss (Pn...

Definition Portfolio.cs:25

[ATAS.DataFeedsCore.Portfolio.ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md#ac14ab04890eefbfeb86ca4be5a3a5c38)

override string ToString()

Returns a string representation of the portfolio group, including various account-related information...

Definition Portfolio.cs:585

[ATAS.DataFeedsCore.Position](../api/classes/classATAS_1_1DataFeedsCore_1_1Position.md)

Represents a trading position.

Definition Position.cs:99

[ATAS.DataFeedsCore.Position.ToString](../api/classes/classATAS_1_1DataFeedsCore_1_1Position.md#ad17aedb45be9f448df7283a717412271)

override string ToString()

Definition Position.cs:381

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

[ATAS.DataFeedsCore.OrderStates](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716)

OrderStates

Represents the possible states of an order.

Definition OrderStates.cs:7

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2

[ATAS.Indicators.DrawingLayouts](../api/namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92)

DrawingLayouts

Enumerates the different drawing layouts available for chart drawings.

Definition DrawingLayouts.cs:14

[ATAS.Indicators.ObjectType.Rectangle](../api/namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)

@ Rectangle

Rectangle graphic object.

Displays the portfolio, position, order and trade data
