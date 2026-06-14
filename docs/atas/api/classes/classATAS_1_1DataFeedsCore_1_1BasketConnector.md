# ATAS.DataFeedsCore.BasketConnector Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1BasketConnector.html

Inheritance diagram for ATAS.DataFeedsCore.BasketConnector:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.BasketConnector:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [BasketConnector](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a3aa3357835b89d75148327e14095796e) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) dataFeedConnector, [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) tradingConnector, bool marketDataOnly=false) |
| | |
| void | [Connect](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a9151f286c873976a287bf944c4006161) () |
| | |
| void | [Disconnect](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a095c36f9b55dc5c2d071bbaa2e06d474) () |
| | |
| async Task | [ConnectAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ac96c27cf201024d64ddc03d2c9e0623a) () |
| | |
| async Task | [DisconnectAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aa1867fc38bea83e426f5aa178bff68da) () |
| | |
| void | [SubscribeToMarketData](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a561a891758e90c886eb779172e00a1d6) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [SubscribeToMarketData](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a83175077ec4d48c8c88993efc88d2b12) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Subscribes to market data for a security. |
| | |
| void | [UnsubscribeFromMarketData](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aeb7c59bf7fd2eea4e21618d645ca94c0) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [UnsubscribeFromMarketData](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a5a04c862efde50425fd02d8bead3d0bd) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Unsubscribes from market data for a security. |
| | |
| void | [SearchSecurities](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a49ded93a606ae3e4daec1c33f0a8b9ce) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | Searches for securities matching the specified filter. |
| | |
| async Task > | [SearchSecuritiesAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8bc38b96200bd305d630a4557b2c3900) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | |
| async Task > | [RecoverTradesAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a494020e40907a4d2f1bbfd931ac6b9e4) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime from, DateTime to) |
| | |
| void | [SetSupportedExchanges](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aaa734d90f5ae3eb3f1949b74dbb7dc9c) (IEnumerable exchanges) |
| | |
| void | [RegisterOrder](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a322eb465522c9e08ab7b5f5c3cf7cf26) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [ModifyOrder](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#adad25fddf54ecc056a7e84879d7b9341) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) neworder) |
| | Modifies the price of an order. |
| | |
| void | [CancelOrder](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2560330c5046b5e59e2bab0c65c26679) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| Task | [RegisterOrderAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a6af8c995d9d63c1c4affa5fe18c06232) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| Task | [ModifyOrderAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a71b051434141b53951c825415e8ee552) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| Task | [CancelOrderAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a4985667d47988d411792aa03d5f948df) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [TryGetOrder](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8a6ac7d65ed7a5208066d41b6379c706) (long extId) |
| | |
| IEnumerable | [GetRoutes](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2fa88ad7715841c9fd9d21ebf14c6cfb) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| IEnumerable | [GetPortfolios](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a3b3cd7a501b635e8e5da91c5fbdce2b9) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [GetPosition](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#acdad2f096b72c762232bb68521f9fab8) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? tPlusLimit) |
| | |
| Task | [ClosePositionAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a55c069fbd36af48f6e9ee32af49447bc) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| Task | [ClosePositionsAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aebaae79b39a122ed6b3054dd60dc1b1d) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| Task | [ChangeMarginParametersAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a032b2c214b4e682de97a77c3c24687f5) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, bool? isolated=null, decimal? leverage=null) |
| | Switches position margin mode (isolated/cross) and/or trading leverage for it. |
| | |
| Task | [ChangeIsolatedMarginAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a78559883f1c9fa4451877c0e4ef426b8) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal value) |
| | Adds or removes to isolated margin of a position. This method does not apply for crossed margin mode
 This method works only if Position.Risk property is not null. |
| | |
| decimal? | [CalcLiquidationPrice](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aaad98d1d406974aadb449a8b178b7f48) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal margin) |
| | Allows to estimate Liquidation Price change for a margin. |
| | |
| decimal?? decimal maxRemovable | [CalcIsolatedMarginChangeRange](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae69dd2ac74e719595e16568a90c30d07) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| decimal | [ConvertCurrency](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a29ed998f144b82d0db8fb5639045e68f) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, string currencyFrom, string currencyTo, decimal volume, decimal? limitPrice=null, bool roundToLotSize=true) |
| | Converts volume from one currency to another Used for Notional value calculation. |
| | |
| decimal? | [CalcMaxOrderVolume](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0aa514d14ba7d75bac737980c7ad8d3b) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice=null) |
| | Gets max possible volume for the order
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| decimal? | [CalcOrderCost](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae00948e82ecc14777888c7226ca37edf) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice, decimal volume, out object? detailing, bool giveDetailing=false) |
| | Calculates total order cost including commissions and initial margin and everything else
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| [ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? | [GetSecurityTradingOptions](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a7949e66a46250b861bff8bb04b1008c2) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | Gets possible TimeInForce for order and optional flags that may be passed when order is created
 If null default behaviour is expected:
 TimeInForce = DAY, GTC, FOK
 No order flags |
| | |
| Task > | [GetMyTradesAsync](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae37ed8d1d0951edbc92dd78e2d4c5a66) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime from, DateTime to) |
| | Get a list of my trades. |
| | |
| void | [Connect](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4a0a52aa5eea9e4fffb0013ae87669a6) () |
| | |
| void | [Disconnect](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a3c5fc1ab12e6f14d81c4536e22bfdc13) () |
| | |
| Task | [ConnectAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#adb0d00d737364d2b46b57cf6ba85b5d8) () |
| | |
| Task | [DisconnectAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae63c5fe20ea7f79b664db4e4f5ef296d) () |
| | |
| Task | [RegisterOrderAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8a2a7ec6471a430c5ca6f1a5ff4da99a) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| Task | [ModifyOrderAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a591428f4c5bf4b0ff38bdea0fe3d1440) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| Task | [CancelOrderAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a96137f3442df97f6ea0a3492383a0f9b) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [CancelOrder](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9865560841a56d8d4c9283d726d32651) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [RegisterOrder](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a19f13ee0baed1e8844817f324c315df4) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [ModifyOrder](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a367624de3add86b5c676432d800aebba) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) neworder) |
| | Modifies the price of an order. |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [TryGetOrder](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a559e4fd70f1571898b06c17817fd2e3a) (long extId) |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [GetPosition](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a36f29e065945aec4fa161645c0d4217e) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? tPlusLimit) |
| | |
| Task | [ClosePositionAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a5a7c9fdaa94c404fa9f2783ba9d1abb2) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| Task | [ClosePositionsAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1c283e520d5566f1e946d0bcaab7875a) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| void | [SubscribeToMarketData](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a14e15f0373b047d97b274bb33c484466) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [SubscribeToMarketData](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a76085b20e7d1ac6fa72b01088929a88a) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Subscribes to market data for a security. |
| | |
| void | [UnsubscribeFromMarketData](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#adbd503742509a515328789dcd68924f5) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [UnsubscribeFromMarketData](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa5af9f893563ee262e182440d90f7223) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Unsubscribes from market data for a security. |
| | |
| void | [SearchSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af9315f6c954ff5d935453832ca815858) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | Searches for securities matching the specified filter. |
| | |
| Task > | [SearchSecuritiesAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acde2d33931ba35a3e6f309d5f9ff7eea) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | |
| IEnumerable | [GetRoutes](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae37580c81aa36c63a3711ff76f94f6a1) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| IEnumerable | [GetPortfolios](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a130055b35f1571b13254571d9e193f23) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| Task | [ChangeMarginParametersAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8033291e306d851abc6d4d48ad4daa07) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, bool? isolated=null, decimal? leverage=null) |
| | Switches position margin mode (isolated/cross) and/or trading leverage for it. |
| | |
| Task | [ChangeIsolatedMarginAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a6ee2cd1c376dcd72f448770dea7a8bb5) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal value) |
| | Adds or removes to isolated margin of a position. This method does not apply for crossed margin mode
 This method works only if Position.Risk property is not null. |
| | |
| decimal? | [CalcLiquidationPrice](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#afd41e76fd5571e3d8c6de4cc4b94009a) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal margin) |
| | Allows to estimate Liquidation Price change for a margin. |
| | |
| decimal?? decimal maxRemovable | [CalcIsolatedMarginChangeRange](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aebaf24ee33f44cf6fbd9a4c72eeaad5e) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| decimal | [ConvertCurrency](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4d1320f0dbc29cae7bbd8e0747114604) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, string currencyFrom, string currencyTo, decimal volume, decimal? limitPrice=null, bool roundToLotSize=true) |
| | Converts volume from one currency to another Used for Notional value calculation. |
| | |
| decimal? | [CalcMaxOrderVolume](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a2a11c5de42c99c3418bb77b7d7399252) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice=null) |
| | Gets max possible volume for the order
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| decimal? | [CalcOrderCost](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9ddc40b5df1e98db610d9407dd04844) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice, decimal volume, out object? detailing, bool giveDetailing=false) |
| | Calculates total order cost including commissions and initial margin and everything else
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| [ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? | [GetSecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9ca66fbf2eaf52f4d7ce60378ab4173e) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | Gets possible TimeInForce for order and optional flags that may be passed when order is created
 If null default behaviour is expected:
 TimeInForce = DAY, GTC, FOK
 No order flags |
| | |
| Task > | [GetMyTradesAsync](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa648c2ceec0bb7d9198e1482102e3be9) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime from, DateTime to) |
| | Get a list of my trades. |
| | |

| Public Attributes | |
| --- | --- |
| decimal? | [maxAddable](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a90c9e08dc34710df670a517b5b70c706) |
| | |
| - Public Attributes inherited from [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| decimal? | [maxAddable](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac9cd86c5d4d19df2c4c43793280a9eab) |
| | Calculates possible changes of the isolated margin of a position (only for leveraged trading) |
| | |

| Properties | |
| --- | --- |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [DataFeedConnector](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aa39f593eba0ef44a6c68d1bec38b95a5)`[get]` |
| | |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [TradingConnector](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a4164c11972bec5805c37a0c0ffe1ae9f)`[get]` |
| | |
| bool | [MarketDataOnly](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aad68678c44790c95491b265620ba75ba)`[get]` |
| | |
| Guid | [Id](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a48f5364a64761463fa0f5bfb351a52f7)`[get]` |
| | |
| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) | [MarketDataDelayPeriod](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0b69a78bb26e3a6678fcc82f6b430dc5)`[get, set]` |
| | Gets or sets delay period for market data. |
| | |
| [IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) | [Factory](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ab18a082ac345400d6106f028d03fa9ba)`[get, set]` |
| | Factory used to create Security, Portfolio, and other entity objects. |
| | |
| ITimeSyncManager? | [DefaultTimeSyncManager](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8bb0e21babccfd07b3ed9ddc20c9dcf7)`[get, set]` |
| | Default ITimeSyncManager to get time difference with NTP server. |
| | |
| [IConnectorExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) | [ExchangeInfoProvider](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0cf4bb84e354cc3fae64a1e3bc9143f8)`[get, set]` |
| | Gets or sets the provider used to retrieve exchange information for the securities. |
| | |
| string | [DataPath](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a3330f7ac07ea80992ad40b7a37aea02e)`[get, set]` |
| | |
| bool | [MarketDataStreamEnabled](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aeedd3848197a9b8cc13933743730d5d5)`[get, set]` |
| | Indicates whether to raise market data through BestBidAskUpdates, MarketDepthsUpdate, and NewTrades events. |
| | |
| bool | [AllowUpdatePositionsPnL](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#afcf33c13f9150989728a28cc2ae6d8ba)`[get, set]` |
| | |
| bool | [ServerMode](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#acc279dfc89f7a9bb173f2c599d94844b)`[get, set]` |
| | |
| bool | [ReconnectOnFirstConnect](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a7104db4f502c1677438db553aea4c2a2)`[get, set]` |
| | |
| TimeOnly? | [RefreshSecuritiesTime](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2fa98b42d060c46412fa8560d9959907)`[get, set]` |
| | |
| bool | [IsFullLicense](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2cb6294cf432a53b2bfa1447156b291c)`[get, set]` |
| | License type. |
| | |
| bool | [NeedRebatesCheck](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a3c4757bd2cadff6efb5fd4caafed55cf)`[get, set]` |
| | Has rebate check feature. |
| | |
| IEnumerable | [Securities](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ab32e1cbc83c9aa28364899b7eecb9e83)`[get]` |
| | |
| IEnumerable | [Portfolios](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a9feeaf702c215c8e3119a0127932369c)`[get]` |
| | |
| IEnumerable | [Positions](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a54a2bae8d41a047345df30d96216c32f)`[get]` |
| | |
| IEnumerable | [Orders](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#af45afe2eba3df48e0c484f8b8e7c5de6)`[get]` |
| | |
| IEnumerable | [MyTrades](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2fb6386d97d6c5e3819f90571dbe0de7)`[get]` |
| | |
| bool | [HasPendingActions](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a56abf2ee19d70f72da8fb161b13ad1fb)`[get]` |
| | |
| [IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | [LatencyManager](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#af1c1f723307e9ef55dfad9440c016c06)`[get]` |
| | Latency manager. |
| | |
| ConnectionStates | [ConnectionState](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8386f9020c67881317dd012514bd5e22)`[get]` |
| | Current connection state of the connector. |
| | |
| bool | [IsConnected](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a11cd14332e4da78d3b6fb9dd285d5d82)`[get]` |
| | |
| bool | [IsSupportedServerOCO](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae8522b2df514e327791fad54582a63ae)`[get]` |
| | Indicates whether the connector supports server-side OCO orders. |
| | |
| bool | [IsSupportedStopOrders](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8ddeb056ff7bfc9425220bd154ed84a8)`[get]` |
| | Indicates whether the connector supports stop orders. |
| | |
| bool | [IsSupportedTradingFunctions](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a851329223fa377f4da08103b52950d21)`[get]` |
| | Indicates whether the connector supports trading functions. |
| | |
| bool | [IsSupportedRussianMarket](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae7737fe1c8f75fc7d47eaf36fac8377a)`[get]` |
| | Indicates whether the connector supports Russian market instruments. |
| | |
| bool | [IsSupportedAmericanFutures](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#abd4ea16fa1e1658b417ebea328d37b82)`[get]` |
| | Indicates whether the connector supports American futures. |
| | |
| bool | [IsSupportedAmericanStocks](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a916060a1bad209a74b95c40d6b345513)`[get]` |
| | Indicates whether the connector supports American stocks. |
| | |
| bool | [IsSupportedCrypto](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0ef0055b413d83682aa9e510406e1778)`[get]` |
| | Indicates whether the connector supports crypto instruments. |
| | |
| bool | [IsSupportedCfd](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a9fb854171b509141f02c27400a2b112f)`[get]` |
| | Indicates whether the connector supports CFD instruments. |
| | |
| bool | [IsSupportedMaxOrderCalculation](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a6e9657f68a363fe99840fd78ff327af2)`[get]` |
| | Does connector support max order calculation for the instrument
 This feature enables percentage slider under the volume input box
 Affects CalcMaxOrderVolume and CalcOrderCost methods. |
| | |
| bool | [IsSupportedServerTime](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a4fa5380e3dc5ddeaaeda7b6c805e1295)`[get]` |
| | Does connector supports getting exchange server time. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| Guid | [Id](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af690ae69693a9413c01048a4be66160d)`[get]` |
| | |
| bool | [IsSupportedServerOCO](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a065d47a51b043efc0f98ff038f41c0f7)`[get]` |
| | Indicates whether the connector supports server-side OCO orders. |
| | |
| bool | [IsSupportedStopOrders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae14df0340e84ba634bb2f9286ea99997)`[get]` |
| | Indicates whether the connector supports stop orders. |
| | |
| bool | [IsSupportedTradingFunctions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab6d379a91ea977907f64d7397885908a)`[get]` |
| | Indicates whether the connector supports trading functions. |
| | |
| bool | [IsConnected](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a7bb79009c97cba15552ca6608b826a41)`[get]` |
| | |
| ConnectionStates | [ConnectionState](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acc60d0101a2ad2916b840e4fb11b93e1)`[get]` |
| | Current connection state of the connector. |
| | |
| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) | [MarketDataDelayPeriod](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4b7f969cd066164ce1441bc790efcda2)`[get, set]` |
| | Gets or sets delay period for market data. |
| | |
| bool | [IsSupportedRussianMarket](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aecc8754caa33f468ed114dde5aa2a5eb)`[get]` |
| | Indicates whether the connector supports Russian market instruments. |
| | |
| bool | [IsSupportedAmericanFutures](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a83b5ae19a6279720631986344d780d79)`[get]` |
| | Indicates whether the connector supports American futures. |
| | |
| bool | [IsSupportedAmericanStocks](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9d7f85c3ef81479344acd9815f13ec0)`[get]` |
| | Indicates whether the connector supports American stocks. |
| | |
| bool | [IsSupportedCrypto](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac7585da8dd34e8e11e87b908a15bc75f)`[get]` |
| | Indicates whether the connector supports crypto instruments. |
| | |
| bool | [IsSupportedCfd](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aea6a8abf0f59bd74b101f33abfdbb2ad)`[get]` |
| | Indicates whether the connector supports CFD instruments. |
| | |
| bool | [IsSupportedMaxOrderCalculation](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a2f21b7d206667e239193611506635728)`[get]` |
| | Does connector support max order calculation for the instrument
 This feature enables percentage slider under the volume input box
 Affects CalcMaxOrderVolume and CalcOrderCost methods. |
| | |
| bool | [IsSupportedServerTime](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab17540c72cb365741341106225dd5ec0)`[get]` |
| | Does connector supports getting exchange server time. |
| | |
| bool | [MarketDataStreamEnabled](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aad0325db053fc101ebfa102ff3596a30)`[get, set]` |
| | Indicates whether to raise market data through BestBidAskUpdates, MarketDepthsUpdate, and NewTrades events. |
| | |
| bool | [AllowUpdatePositionsPnL](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1a3ce2be58309d0dea4919135e59f322)`[get, set]` |
| | |
| [IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) | [Factory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8b148f7401ca80d932bca01613ba1cd5)`[get, set]` |
| | Factory used to create Security, Portfolio, and other entity objects. |
| | |
| ITimeSyncManager? | [DefaultTimeSyncManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0308d83cd28dde8844a3010ef6f2e2b4)`[get, set]` |
| | Default ITimeSyncManager to get time difference with NTP server. |
| | |
| [IConnectorExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) | [ExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0346c82f55f7364b43b46ce245ba92a7)`[get, set]` |
| | Gets or sets the provider used to retrieve exchange information for the securities. |
| | |
| string | [DataPath](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aee500f7c55bd7453290ac970e1bf2134)`[get, set]` |
| | |
| [IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | [LatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a49925c155cbad25cd934be5008859520)`[get]` |
| | Latency manager. |
| | |
| bool | [IsFullLicense](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae40d6cf32d10ac4ea546eea49448b7bc)`[get, set]` |
| | License type. |
| | |
| bool | [NeedRebatesCheck](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a02bf5da51c33e95e55e0ea44ce67da90)`[get, set]` |
| | Has rebate check feature. |
| | |
| IEnumerable | [Securities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a455a328675ad1821a38404c2d184d0e6)`[get]` |
| | |
| IEnumerable | [Portfolios](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1e56eda4bef1e2e2e725a7d081d101bf)`[get]` |
| | |
| IEnumerable | [MyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a93c0b99a1222877d1fe70b1a0cd2d296)`[get]` |
| | |
| IEnumerable | [Orders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac1416ecd2cf2a02aa361fe335fdb342d)`[get]` |
| | |
| IEnumerable | [Positions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a46972cbe8a28b386c22fef5fcc0fbed3)`[get]` |
| | |
| TimeOnly? | [RefreshSecuritiesTime](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a3127b969389b75d4541d2aab7bddfd0d)`[get, set]` |
| | |
| bool | [HasPendingActions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a002743dcb2e567e8a71897aa9e02754c)`[get]` |
| | |
| bool | [ServerMode](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a60d7936cd755fddd4fc78c83f23212db)`[get, set]` |
| | |
| bool | [ReconnectOnFirstConnect](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac33c399e843d0cdbf2d8a7e97d070d53)`[get, set]` |
| | |

| Events | |
| --- | --- |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [ConnectionStateChanged](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2064bc553eb969b790827e306d408574) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Connected](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ad15f8eb160d28362f1e9e5365e28e03f) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Disconnected](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a4d6e6f4956ea60bc0f039ea2f0746b37) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewMyTrades](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae577fdf7c6dc5819255a57eddca34af3) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewOrders](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#add369583be2c7fb2250553b1052dd5bb) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewPositions](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a27f8936a46ab0f0011b7d13df6b87119) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewPortfolios](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a909d0797a6b9647c10834b03464d23b4) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewSecurities](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ad712018107f46e90253eea92c4e679ba) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [RemoveSecurities](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a099fe5cde99591367527d706aaa380b2) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrderChanged](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a11d8714505145e469fa7ea6585f41eb3) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrdersRegisterFailed](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#afbc2a3bde4d9cdaaeb8f3b4cff090dcc) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrdersCancelFailed](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a7f370225da2e3db656b7ed73fba12a20) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrderModifyFailed](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a04449f86c08359fc65dac9f99264d5c4) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [PortfoliosChanged](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aeb682c645544b8a508cad7474a453adf) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [PositionsChanged](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a59c93dfe51cf0c14f3f8a4e3bd8c58d5) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [BestBidAskUpdates](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a214f6138375cf709a205044310ed3092) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [MarketDepthsUpdate](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a6161df8fd258550f46e07f27652c51f0) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [MarketByOrderChanged](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a448e360105b89f43b61540abac4b1c29) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewTrades](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ac35119683d07e10d14179efdcf80ad93) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [SecurityChanged](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2b180704afae8b38844de7270f957ac4) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [SecuritySummaryChanged](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a78cf4b7dae6eb636213843cb51ef2a9f) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Error](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a164be0a8947126fa7227ea6b5acf9088) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [SearchSecuritiesResult](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aa8435bd141c9ff3dae988c90effd6567) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [RegisterSecurityResult](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8b3ac38571abcf92ebd2f3468476b080) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [NewNews](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a7e8cf11739fdfb6972556975909bec58) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [RebateReceived](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a9aced7ac343e9ae06ce7a17a012499a4) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73), Exception?>? | [SubscribeMarketDataResult](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a69edc8ae62fe94ffb44ed7c0fa04f255) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73), Exception?>? | [UnsubscribeMarketDataResult](./classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ab119dd472d7049218ab573e6c2aa81e1) |
| | |
| - Events inherited from [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [ConnectionStateChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9107f2ec4c193fa507760c329b44e57) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Connected](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9524e9b808b2675e4e408f3250d27bf5) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Disconnected](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a888c29e135620d4bec8dfa658b7a34b7) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a5030740d60042a4ad424259e0e361243) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewOrders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a58bf22e2a07322a1e9e1a59c238407fb) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewPositions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a7b2c9c1e4fc477670e4c29ecb2819dcd) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewPortfolios](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac90f7ae6aa4a85552584a841033430df) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a5abf82f8241ac3756d09274b0fec297b) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [RemoveSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a7fbbed5a1770aa586a123e0e381f34f4) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrderChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa9f15c8b6154d82095cfc3b532e20eda) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrdersRegisterFailed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac719158631d0fe76cf53ab635199e444) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrdersCancelFailed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a95263a71ad578f9b882342138fe08fba) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrderModifyFailed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a284927aa59f4d217b7204f88475bec8f) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [PortfoliosChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1f2583219295329d26710eda515c1c78) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [PositionsChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a42ddd365a0398d95ef7395f4bcf9ff67) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [BestBidAskUpdates](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af6d2703eb1b7f40aed3a5a6c176e206e) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [MarketDepthsUpdate](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae25b1ece5a9fe0a76cfe188fbea9f6c6) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) | [MarketByOrderChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab39f4eca0f9729158f93894c80d38060) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4c12a0ddf3f4100461ed164fe20f7fd0) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [SecurityChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae0d7de60d50692ee7898c53a10f386aa) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [SecuritySummaryChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acc7159452076320cc90d34bfa13e88b7) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Error](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac57b629541c6e7b03eb2477ca428d467) |
| | Raised when an error occurs. |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [SearchSecuritiesResult](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac34381609e7bf536eed720dda9f87d46) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [RegisterSecurityResult](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a87a4126f3f11f7f4bc782004a537dbe9) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [NewNews](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0327a30e40ec7a649a8d218c548638a8) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [RebateReceived](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a411fb41c5fa82c7ca49ed8c0bfc3d328) |
| | |
| - Events inherited from [ATAS.DataFeedsCore.IMarketDataPublisher](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDataPublisher.md) | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73), Exception?>? | [SubscribeMarketDataResult](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDataPublisher.md#a1ac271007da89345271c2912cf221115) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73), Exception?>? | [UnsubscribeMarketDataResult](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDataPublisher.md#a77edb55653417c7ca1be75a52a7ce335) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)BasketConnector()

| ATAS.DataFeedsCore.BasketConnector.BasketConnector | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | dataFeedConnector, |
| --- | --- | --- | --- |
| | | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | tradingConnector, |
| | | bool | marketDataOnly = `false` |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CalcIsolatedMarginChangeRange()

| decimal?? decimal maxRemovable ATAS.DataFeedsCore.BasketConnector.CalcIsolatedMarginChangeRange | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aebaf24ee33f44cf6fbd9a4c72eeaad5e).

## [◆](https://docs.atas.net/en/)CalcLiquidationPrice()

| decimal? ATAS.DataFeedsCore.BasketConnector.CalcLiquidationPrice | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | decimal | margin |
| | ) | | |

Allows to estimate Liquidation Price change for a margin.

Parameters

| position | Position to calculate for |
| --- | --- |
| margin | Absolute margin for calculation |

Returnsnull if calculation is not supported

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#afd41e76fd5571e3d8c6de4cc4b94009a).

## [◆](https://docs.atas.net/en/)CalcMaxOrderVolume()

| decimal? ATAS.DataFeedsCore.BasketConnector.CalcMaxOrderVolume | ( | [OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) | orderType, |
| --- | --- | --- | --- |
| | | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| | | [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) | direction, |
| | | decimal? | limitPrice = `null` |
| | ) | | |

Gets max possible volume for the order

 This function is always return null if IsSupportedMaxOrderCalculation is false.

Parameters

| orderType | |
| --- | --- |
| position | Position for calculation |
| direction | |
| limitPrice | optional limit price to calculate volume at. If null market price will be used |

Returns

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a2a11c5de42c99c3418bb77b7d7399252).

## [◆](https://docs.atas.net/en/)CalcOrderCost()

| decimal? ATAS.DataFeedsCore.BasketConnector.CalcOrderCost | ( | [OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) | orderType, |
| --- | --- | --- | --- |
| | | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| | | [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) | direction, |
| | | decimal? | limitPrice, |
| | | decimal | volume, |
| | | out object? | detailing, |
| | | bool | giveDetailing = `false` |
| | ) | | |

Calculates total order cost including commissions and initial margin and everything else

 This function is always return null if IsSupportedMaxOrderCalculation is false.

Parameters

| orderType | |
| --- | --- |
| position | |
| direction | |
| limitPrice | pass null to use market price for calculation |
| volume | Desired volume of contracts to make order |
| detailing | An object explaining each item which builds up the final price |
| giveDetailing | true for request detailing |

ReturnsComplete order cost including requested volume and all commissions and margins

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9ddc40b5df1e98db610d9407dd04844).

## [◆](https://docs.atas.net/en/)CancelOrder()

| void ATAS.DataFeedsCore.BasketConnector.CancelOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9865560841a56d8d4c9283d726d32651).

## [◆](https://docs.atas.net/en/)CancelOrderAsync()

| Task ATAS.DataFeedsCore.BasketConnector.CancelOrderAsync | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a96137f3442df97f6ea0a3492383a0f9b).

## [◆](https://docs.atas.net/en/)ChangeIsolatedMarginAsync()

| Task ATAS.DataFeedsCore.BasketConnector.ChangeIsolatedMarginAsync | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | decimal | value |
| | ) | | |

Adds or removes to isolated margin of a position. This method does not apply for crossed margin mode

 This method works only if Position.Risk property is not null.

Parameters

| position | |
| --- | --- |
| value | Use positive value to add margin and negative to reduce |

Returns

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a6ee2cd1c376dcd72f448770dea7a8bb5).

## [◆](https://docs.atas.net/en/)ChangeMarginParametersAsync()

| Task ATAS.DataFeedsCore.BasketConnector.ChangeMarginParametersAsync | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | bool? | isolated = `null`, |
| | | decimal? | leverage = `null` |
| | ) | | |

Switches position margin mode (isolated/cross) and/or trading leverage for it.

Pass null if parameter should not be changed

This method works only if Position.Risk property is not null

Parameters

| position | |
| --- | --- |
| isolated | True for isolated, False for crossed mode |
| leverage | |

Returns

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8033291e306d851abc6d4d48ad4daa07).

## [◆](https://docs.atas.net/en/)ClosePositionAsync()

| Task ATAS.DataFeedsCore.BasketConnector.ClosePositionAsync | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a5a7c9fdaa94c404fa9f2783ba9d1abb2).

## [◆](https://docs.atas.net/en/)ClosePositionsAsync()

| Task ATAS.DataFeedsCore.BasketConnector.ClosePositionsAsync | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1c283e520d5566f1e946d0bcaab7875a).

## [◆](https://docs.atas.net/en/)Connect()

| void ATAS.DataFeedsCore.BasketConnector.Connect | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4a0a52aa5eea9e4fffb0013ae87669a6).

## [◆](https://docs.atas.net/en/)ConnectAsync()

| async Task ATAS.DataFeedsCore.BasketConnector.ConnectAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#adb0d00d737364d2b46b57cf6ba85b5d8).

## [◆](https://docs.atas.net/en/)ConvertCurrency()

| decimal ATAS.DataFeedsCore.BasketConnector.ConvertCurrency | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | string | currencyFrom, |
| | | string | currencyTo, |
| | | decimal | volume, |
| | | decimal? | limitPrice = `null`, |
| | | bool | roundToLotSize = `true` |
| | ) | | |

Converts volume from one currency to another Used for Notional value calculation.

Parameters

| security | |
| --- | --- |
| currencyFrom | |
| currencyTo | |
| volume | |
| limitPrice | optional value in QuoteCurrency |
| roundToLotSize | Round result to LotSize/Precision |

Returns
Exceptions

| ArgumentNullException | |
| --- | --- |
| ArgumentException | |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4d1320f0dbc29cae7bbd8e0747114604).

## [◆](https://docs.atas.net/en/)Disconnect()

| void ATAS.DataFeedsCore.BasketConnector.Disconnect | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a3c5fc1ab12e6f14d81c4536e22bfdc13).

## [◆](https://docs.atas.net/en/)DisconnectAsync()

| async Task ATAS.DataFeedsCore.BasketConnector.DisconnectAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae63c5fe20ea7f79b664db4e4f5ef296d).

## [◆](https://docs.atas.net/en/)GetMyTradesAsync()

| Task > ATAS.DataFeedsCore.BasketConnector.GetMyTradesAsync | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | DateTime | from, |
| | | DateTime | to |
| | ) | | |

Get a list of my trades.

Parameters

| portfolio | Portfolio. |
| --- | --- |
| security | Security. |
| from | From date. |
| to | To date. |

Returns

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa648c2ceec0bb7d9198e1482102e3be9).

## [◆](https://docs.atas.net/en/)GetPortfolios()

| IEnumerable ATAS.DataFeedsCore.BasketConnector.GetPortfolios | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a130055b35f1571b13254571d9e193f23).

## [◆](https://docs.atas.net/en/)GetPosition()

| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.DataFeedsCore.BasketConnector.GetPosition | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | tPlusLimit |
| | ) | | |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a36f29e065945aec4fa161645c0d4217e).

## [◆](https://docs.atas.net/en/)GetRoutes()

| IEnumerable ATAS.DataFeedsCore.BasketConnector.GetRoutes | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae37580c81aa36c63a3711ff76f94f6a1).

## [◆](https://docs.atas.net/en/)GetSecurityTradingOptions()

| [ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? ATAS.DataFeedsCore.BasketConnector.GetSecurityTradingOptions | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

Gets possible TimeInForce for order and optional flags that may be passed when order is created

 If null default behaviour is expected:

 TimeInForce = DAY, GTC, FOK

 No order flags

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9ca66fbf2eaf52f4d7ce60378ab4173e).

## [◆](https://docs.atas.net/en/)ModifyOrder()

| void ATAS.DataFeedsCore.BasketConnector.ModifyOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | neworder |
| | ) | | |

Modifies the price of an order.

Parameters

| order | The order to modify. |
| --- | --- |
| neworder | The new order with the updated parameters. |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a367624de3add86b5c676432d800aebba).

## [◆](https://docs.atas.net/en/)ModifyOrderAsync()

| Task ATAS.DataFeedsCore.BasketConnector.ModifyOrderAsync | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder |
| | ) | | |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a591428f4c5bf4b0ff38bdea0fe3d1440).

## [◆](https://docs.atas.net/en/)RecoverTradesAsync()

| async Task > ATAS.DataFeedsCore.BasketConnector.RecoverTradesAsync | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | DateTime | from, |
| | | DateTime | to |
| | ) | | |

## [◆](https://docs.atas.net/en/)RegisterOrder()

| void ATAS.DataFeedsCore.BasketConnector.RegisterOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a19f13ee0baed1e8844817f324c315df4).

## [◆](https://docs.atas.net/en/)RegisterOrderAsync()

| Task ATAS.DataFeedsCore.BasketConnector.RegisterOrderAsync | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8a2a7ec6471a430c5ca6f1a5ff4da99a).

## [◆](https://docs.atas.net/en/)SearchSecurities()

| void ATAS.DataFeedsCore.BasketConnector.SearchSecurities | ( | [SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) | filter | ) | |
| --- | --- | --- | --- | --- | --- |

Searches for securities matching the specified filter.

Parameters

| filter | |
| --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af9315f6c954ff5d935453832ca815858).

## [◆](https://docs.atas.net/en/)SearchSecuritiesAsync()

| async Task > ATAS.DataFeedsCore.BasketConnector.SearchSecuritiesAsync | ( | [SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) | filter | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acde2d33931ba35a3e6f309d5f9ff7eea).

## [◆](https://docs.atas.net/en/)SetSupportedExchanges()

| void ATAS.DataFeedsCore.BasketConnector.SetSupportedExchanges | ( | IEnumerable | exchanges | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)SubscribeToMarketData() [1/2]

| void ATAS.DataFeedsCore.BasketConnector.SubscribeToMarketData | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a14e15f0373b047d97b274bb33c484466).

## [◆](https://docs.atas.net/en/)SubscribeToMarketData() [2/2]

| void ATAS.DataFeedsCore.BasketConnector.SubscribeToMarketData | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Subscribes to market data for a security.

Parameters

| security | The security to subscribe to. |
| --- | --- |
| subscriptionTypes | The subscription type flags. |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a76085b20e7d1ac6fa72b01088929a88a).

## [◆](https://docs.atas.net/en/)TryGetOrder()

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) ATAS.DataFeedsCore.BasketConnector.TryGetOrder | ( | long | extId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a559e4fd70f1571898b06c17817fd2e3a).

## [◆](https://docs.atas.net/en/)UnsubscribeFromMarketData() [1/2]

| void ATAS.DataFeedsCore.BasketConnector.UnsubscribeFromMarketData | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#adbd503742509a515328789dcd68924f5).

## [◆](https://docs.atas.net/en/)UnsubscribeFromMarketData() [2/2]

| void ATAS.DataFeedsCore.BasketConnector.UnsubscribeFromMarketData | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Unsubscribes from market data for a security.

Parameters

| security | The security to unsubscribe from. |
| --- | --- |
| subscriptionTypes | The subscription type flags. |

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa5af9f893563ee262e182440d90f7223).

## Member Data Documentation

## [◆](https://docs.atas.net/en/)maxAddable

| decimal? ATAS.DataFeedsCore.BasketConnector.maxAddable |
| --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AllowUpdatePositionsPnL

| bool ATAS.DataFeedsCore.BasketConnector.AllowUpdatePositionsPnL |
| --- |

getset

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1a3ce2be58309d0dea4919135e59f322).

## [◆](https://docs.atas.net/en/)ConnectionState

| ConnectionStates ATAS.DataFeedsCore.BasketConnector.ConnectionState |
| --- |

get

Current connection state of the connector.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acc60d0101a2ad2916b840e4fb11b93e1).

## [◆](https://docs.atas.net/en/)DataFeedConnector

| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) ATAS.DataFeedsCore.BasketConnector.DataFeedConnector |
| --- |

get

## [◆](https://docs.atas.net/en/)DataPath

| string ATAS.DataFeedsCore.BasketConnector.DataPath |
| --- |

getset

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aee500f7c55bd7453290ac970e1bf2134).

## [◆](https://docs.atas.net/en/)DefaultTimeSyncManager

| ITimeSyncManager? ATAS.DataFeedsCore.BasketConnector.DefaultTimeSyncManager |
| --- |

getset

Default ITimeSyncManager to get time difference with NTP server.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0308d83cd28dde8844a3010ef6f2e2b4).

## [◆](https://docs.atas.net/en/)ExchangeInfoProvider

| [IConnectorExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) ATAS.DataFeedsCore.BasketConnector.ExchangeInfoProvider |
| --- |

getset

Gets or sets the provider used to retrieve exchange information for the securities.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0346c82f55f7364b43b46ce245ba92a7).

## [◆](https://docs.atas.net/en/)Factory

| [IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) ATAS.DataFeedsCore.BasketConnector.Factory |
| --- |

getset

Factory used to create Security, Portfolio, and other entity objects.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8b148f7401ca80d932bca01613ba1cd5).

## [◆](https://docs.atas.net/en/)HasPendingActions

| bool ATAS.DataFeedsCore.BasketConnector.HasPendingActions |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a002743dcb2e567e8a71897aa9e02754c).

## [◆](https://docs.atas.net/en/)Id

| Guid ATAS.DataFeedsCore.BasketConnector.Id |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af690ae69693a9413c01048a4be66160d).

## [◆](https://docs.atas.net/en/)IsConnected

| bool ATAS.DataFeedsCore.BasketConnector.IsConnected |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a7bb79009c97cba15552ca6608b826a41).

## [◆](https://docs.atas.net/en/)IsFullLicense

| bool ATAS.DataFeedsCore.BasketConnector.IsFullLicense |
| --- |

getset

License type.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae40d6cf32d10ac4ea546eea49448b7bc).

## [◆](https://docs.atas.net/en/)IsSupportedAmericanFutures

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedAmericanFutures |
| --- |

get

Indicates whether the connector supports American futures.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a83b5ae19a6279720631986344d780d79).

## [◆](https://docs.atas.net/en/)IsSupportedAmericanStocks

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedAmericanStocks |
| --- |

get

Indicates whether the connector supports American stocks.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9d7f85c3ef81479344acd9815f13ec0).

## [◆](https://docs.atas.net/en/)IsSupportedCfd

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedCfd |
| --- |

get

Indicates whether the connector supports CFD instruments.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aea6a8abf0f59bd74b101f33abfdbb2ad).

## [◆](https://docs.atas.net/en/)IsSupportedCrypto

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedCrypto |
| --- |

get

Indicates whether the connector supports crypto instruments.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac7585da8dd34e8e11e87b908a15bc75f).

## [◆](https://docs.atas.net/en/)IsSupportedMaxOrderCalculation

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedMaxOrderCalculation |
| --- |

get

Does connector support max order calculation for the instrument

 This feature enables percentage slider under the volume input box

 Affects CalcMaxOrderVolume and CalcOrderCost methods.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a2f21b7d206667e239193611506635728).

## [◆](https://docs.atas.net/en/)IsSupportedRussianMarket

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedRussianMarket |
| --- |

get

Indicates whether the connector supports Russian market instruments.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aecc8754caa33f468ed114dde5aa2a5eb).

## [◆](https://docs.atas.net/en/)IsSupportedServerOCO

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedServerOCO |
| --- |

get

Indicates whether the connector supports server-side OCO orders.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a065d47a51b043efc0f98ff038f41c0f7).

## [◆](https://docs.atas.net/en/)IsSupportedServerTime

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedServerTime |
| --- |

get

Does connector supports getting exchange server time.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab17540c72cb365741341106225dd5ec0).

## [◆](https://docs.atas.net/en/)IsSupportedStopOrders

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedStopOrders |
| --- |

get

Indicates whether the connector supports stop orders.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae14df0340e84ba634bb2f9286ea99997).

## [◆](https://docs.atas.net/en/)IsSupportedTradingFunctions

| bool ATAS.DataFeedsCore.BasketConnector.IsSupportedTradingFunctions |
| --- |

get

Indicates whether the connector supports trading functions.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab6d379a91ea977907f64d7397885908a).

## [◆](https://docs.atas.net/en/)LatencyManager

| [IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) ATAS.DataFeedsCore.BasketConnector.LatencyManager |
| --- |

get

Latency manager.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a49925c155cbad25cd934be5008859520).

## [◆](https://docs.atas.net/en/)MarketDataDelayPeriod

| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) ATAS.DataFeedsCore.BasketConnector.MarketDataDelayPeriod |
| --- |

getset

Gets or sets delay period for market data.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4b7f969cd066164ce1441bc790efcda2).

## [◆](https://docs.atas.net/en/)MarketDataOnly

| bool ATAS.DataFeedsCore.BasketConnector.MarketDataOnly |
| --- |

get

## [◆](https://docs.atas.net/en/)MarketDataStreamEnabled

| bool ATAS.DataFeedsCore.BasketConnector.MarketDataStreamEnabled |
| --- |

getset

Indicates whether to raise market data through BestBidAskUpdates, MarketDepthsUpdate, and NewTrades events.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aad0325db053fc101ebfa102ff3596a30).

## [◆](https://docs.atas.net/en/)MyTrades

| IEnumerable ATAS.DataFeedsCore.BasketConnector.MyTrades |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a93c0b99a1222877d1fe70b1a0cd2d296).

## [◆](https://docs.atas.net/en/)NeedRebatesCheck

| bool ATAS.DataFeedsCore.BasketConnector.NeedRebatesCheck |
| --- |

getset

Has rebate check feature.

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a02bf5da51c33e95e55e0ea44ce67da90).

## [◆](https://docs.atas.net/en/)Orders

| IEnumerable ATAS.DataFeedsCore.BasketConnector.Orders |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac1416ecd2cf2a02aa361fe335fdb342d).

## [◆](https://docs.atas.net/en/)Portfolios

| IEnumerable ATAS.DataFeedsCore.BasketConnector.Portfolios |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1e56eda4bef1e2e2e725a7d081d101bf).

## [◆](https://docs.atas.net/en/)Positions

| IEnumerable ATAS.DataFeedsCore.BasketConnector.Positions |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a46972cbe8a28b386c22fef5fcc0fbed3).

## [◆](https://docs.atas.net/en/)ReconnectOnFirstConnect

| bool ATAS.DataFeedsCore.BasketConnector.ReconnectOnFirstConnect |
| --- |

getset

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac33c399e843d0cdbf2d8a7e97d070d53).

## [◆](https://docs.atas.net/en/)RefreshSecuritiesTime

| TimeOnly? ATAS.DataFeedsCore.BasketConnector.RefreshSecuritiesTime |
| --- |

getset

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a3127b969389b75d4541d2aab7bddfd0d).

## [◆](https://docs.atas.net/en/)Securities

| IEnumerable ATAS.DataFeedsCore.BasketConnector.Securities |
| --- |

get

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a455a328675ad1821a38404c2d184d0e6).

## [◆](https://docs.atas.net/en/)ServerMode

| bool ATAS.DataFeedsCore.BasketConnector.ServerMode |
| --- |

getset

Implements [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a60d7936cd755fddd4fc78c83f23212db).

## [◆](https://docs.atas.net/en/)TradingConnector

| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) ATAS.DataFeedsCore.BasketConnector.TradingConnector |
| --- |

get

## Event Documentation

## [◆](https://docs.atas.net/en/)BestBidAskUpdates

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.BestBidAskUpdates |
| --- |

## [◆](https://docs.atas.net/en/)Connected

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.Connected |
| --- |

## [◆](https://docs.atas.net/en/)ConnectionStateChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.ConnectionStateChanged |
| --- |

## [◆](https://docs.atas.net/en/)Disconnected

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.Disconnected |
| --- |

## [◆](https://docs.atas.net/en/)Error

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.Error |
| --- |

## [◆](https://docs.atas.net/en/)MarketByOrderChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.MarketByOrderChanged |
| --- |

## [◆](https://docs.atas.net/en/)MarketDepthsUpdate

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.MarketDepthsUpdate |
| --- |

## [◆](https://docs.atas.net/en/)NewMyTrades

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.NewMyTrades |
| --- |

## [◆](https://docs.atas.net/en/)NewNews

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.NewNews |
| --- |

## [◆](https://docs.atas.net/en/)NewOrders

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.NewOrders |
| --- |

## [◆](https://docs.atas.net/en/)NewPortfolios

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.NewPortfolios |
| --- |

## [◆](https://docs.atas.net/en/)NewPositions

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.NewPositions |
| --- |

## [◆](https://docs.atas.net/en/)NewSecurities

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.NewSecurities |
| --- |

## [◆](https://docs.atas.net/en/)NewTrades

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.NewTrades |
| --- |

## [◆](https://docs.atas.net/en/)OrderChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.OrderChanged |
| --- |

## [◆](https://docs.atas.net/en/)OrderModifyFailed

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.OrderModifyFailed |
| --- |

## [◆](https://docs.atas.net/en/)OrdersCancelFailed

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.OrdersCancelFailed |
| --- |

## [◆](https://docs.atas.net/en/)OrdersRegisterFailed

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.OrdersRegisterFailed |
| --- |

## [◆](https://docs.atas.net/en/)PortfoliosChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.PortfoliosChanged |
| --- |

## [◆](https://docs.atas.net/en/)PositionsChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.PositionsChanged |
| --- |

## [◆](https://docs.atas.net/en/)RebateReceived

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.RebateReceived |
| --- |

## [◆](https://docs.atas.net/en/)RegisterSecurityResult

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.RegisterSecurityResult |
| --- |

## [◆](https://docs.atas.net/en/)RemoveSecurities

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.RemoveSecurities |
| --- |

## [◆](https://docs.atas.net/en/)SearchSecuritiesResult

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.BasketConnector.SearchSecuritiesResult |
| --- |

## [◆](https://docs.atas.net/en/)SecurityChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.SecurityChanged |
| --- |

## [◆](https://docs.atas.net/en/)SecuritySummaryChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.BasketConnector.SecuritySummaryChanged |
| --- |

## [◆](https://docs.atas.net/en/)SubscribeMarketDataResult

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73), Exception?>? ATAS.DataFeedsCore.BasketConnector.SubscribeMarketDataResult |
| --- |

## [◆](https://docs.atas.net/en/)UnsubscribeMarketDataResult

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73), Exception?>? ATAS.DataFeedsCore.BasketConnector.UnsubscribeMarketDataResult |
| --- |

The documentation for this class was generated from the following file:
- [BasketConnector.cs](../files/BasketConnector_8cs.md)
