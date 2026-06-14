# Strategies

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20100__Strategies.html

# Development of a strategy

The Strategy API allows you to create strategies based on the Indicators API to automate your trading process.

In order to develop a strategy it is necessary to:

- Develop a project of a library of classes in Visual Studio.

- Add a link to ATAS Strategies.dll, which is in the directory with the installed program, in the project.

- Create a strategy class and inherit it from [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md) class.

- Realize the logic of the strategy operation.

- Compile the library.

- Place the resulting dll file into the `APPDATA%\[ATAS](../api/namespaces/namespaceATAS.md)\Strategies` directory.

- Click on the blinking button for the changes in the list of strategies to take effect.

Updating strategy list

# Chart Strategies

Functionality of Chart Strategies allows receiving and processing the whole set of data available in indicators and performing trading operations on the basis of these data.

 The [ChartStrategy](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md) class is inherited from the [Indicator](https://docs.atas.net/en/ATAS.Indicators.Indicator) class. Thus, strategies have the full functionality of indicators.

In addition to the functionality of indicators, strategies have additional properties and methods responsible for trading functionality:

Properties:

- [Security](../api/interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a1d6806a24b84f596cd7e4adf8a315edd) - trading instrument

- [Portfolio](../api/interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a2ac811e04e280f17e9a46aa376122326) - selected portfolio

- [Connector](../api/interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#ac42fb5c1c25cb4734acaa356371c6505) - trading connection

- [MyTrades](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a8ae37c6d12ae4459dfe8582326a22677) - list of trades

- [Orders](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a128f2f53547a9ddf8131a29e4b910d33) - list of orders

- [CurrentPosition](../api/interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#aa3c8e8450cedf2e6dc772cee79022057) - volume of the strategy current position

- [AveragePrice](../api/interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a0990ad1aaa4097b121e2f9963026a3a2) - average price of the strategy current position

- [State](../api/interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a18fcdb4e7e4a8d94e285cb45d1742b5e) - strategy state (could be Stopped, Started and Suspended)

Main strategy public methods:

- [OpenOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#add7c48f9ede9667aa7ba8a0f97344eab) - method of opening a new order

- [ModifyOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a01509c1de2aadb7994a819e41a5db29e) - method of modifying an order

- [CancelOrder](../api/interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md#a3808947989c50df10ee357f597a2171a) - method of cancelling an order

- [ShrinkPrice](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#aa3bc75179281e9a10b614c5b6821ed6f) - rounding the transferred price to the trading instrument tick size

- [RaiseShowNotification](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#ae493cbafd5a0d0d6be67b0e28071ba90) - method, allowing visualization of notifications in the platform

Strategy virtual methods, which, if necessary, have to be redefined in the created strategy:

- [OnStarted](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#ae41b2659a5ed3c1f01296be3ed5c8b7c) - is called when a strategy is started

- [OnSuspended](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#afb641a7e153bab4c892d5e22b890eb01) - is called when a strategy is suspended (for example, in situations when a chart with a strategy has been closed)

- [OnStopping](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a8cd6fbcace970938210aece9ff7866b0) - is called before stopping a strategy

- [OnStopped](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#ad127f743624fff6520769ee37ed015c2) - is called when a strategy has been stopped

- [OnCurrentPositionChanged](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a70829f44f98c8210bd4feaecf6c7720e) - is called when changing the strategy current position

- [CanProcess](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#aa44be9516a3167180ba2a3e282f7e237) - returns whether it is possible to trade at this moment. By default, the method checks whether a strategy has been started and the most recent bar is processed at this moment.

Strategy positions.

Every strategy holds its position inside. This position could differ from the general position on the account.

 The account general position could be received through the [TradingManager.Position](../api/interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md#ae28cee783ef49765d986ffa308771175) property.

 The strategy internal position could be received through the [CurrentPosition](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#acfc91e9c225c7e79feb0ee32d20571bd) and [AveragePrice](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a5c12170dfa214437261503a5cab6ed97) properties.

Important! When a strategy is stopped, its internal position is reset to zero. It is extremely desirable to redefine the [OnStopping](../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a8cd6fbcace970938210aece9ff7866b0) method and realize the logic of closing internal positions and cancelling the posted orders in it.

You can find the chart strategy in the list of strategies. For this it is necessary:

- open the context menu with the right mouse button,

- click on the strategy list icon,

- in the opened window of the list of strategies, select the strategy you need,

- click the `Add` button.

- you can enable / disable the strategy by pressing the control button.

Loading Chart strategy

# Additional order options

When developing a strategy, you can set specific order options using the [TradingManager](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a860ae675c2c927b5b05c23f5e480b483) strategy property.

## Reduced only

Reduced only orders are a risk control mechanism that ensures that when executed, they will only reduce an existing position, preventing any unintended increase in exposure. This feature is particularly important in markets where precise control over positions is crucial for effective risk management.

private void TrySetReduceOnly(Order order)

 {

 var flags = TradingManager?

 .GetSecurityTradingOptions()?

 .CreateExtendedOptions(order.Type);

 if (flags is not IOrderOptionReduceOnly ro)

 return;

 ro.ReduceOnly = true;

 order.ExtendedOptions = flags;

 }

## Post only

When you place a Post only order, you are telling the exchange that you want your order to be treated as a maker order only. In other words, you want to ensure that your order doesn't execute as a taker order, which would result in you paying trading fees. If your Post only order would execute immediately as a taker order, it will be canceled instead.

private void TrySetPostOnly(Order order)

 {

 var flags = TradingManager?

 .GetSecurityTradingOptions()?

 .CreateExtendedOptions(order.Type);

 if (flags is not IOrderOptionPostOnly po)

 return;

 po.PostOnly = true;

 order.ExtendedOptions = flags;

 }

## Close on trigger

A Close on trigger order is a type of trading order that is executed when a specified trigger condition is met in the market. It's often used for risk management and to automatically close positions or execute specific actions when a certain price level is reached.

private void TrySetCloseOnTrigger(Order order)

{

 var flags = TradingManager?

 .GetSecurityTradingOptions()?

 .CreateExtendedOptions(order.Type);

 if (flags is not IOrderOptionCloseOnTrigger ct)

 return;

 ct.CloseOnTrigger = true;

 order.ExtendedOptions = flags;

}
