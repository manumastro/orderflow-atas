# ATAS.DataFeedsCore.CryptoAsyncConnector< TPortfolioKey, TSecurityKey > Class Template Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.html

Inheritance diagram for ATAS.DataFeedsCore.CryptoAsyncConnector< TPortfolioKey, TSecurityKey >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.CryptoAsyncConnector< TPortfolioKey, TSecurityKey >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [CheckClientMode](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a533540077bb248a5ce10a5b51b23b7eb) () |
| | |
| abstract void | [DropMarketDataConnection](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a137aa029f55e8ef2405d0283c504b1d7) () |
| | |
| abstract void | [DropTradingConnection](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a8e0ecc80cfea7983c7d93ada1102a1d0) () |
| | |
| abstract void | [DropMarketDataConnectionDuringConnecting](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a247488c34311828f242acddab2a9ecde) () |
| | |
| abstract void | [DropTradingConnectionDuringConnecting](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a64037950181dfbfeaeb33fb00810ef00) () |
| | |
| abstract Task | [DropMarketDataConnectionCoupleTimes](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#afafabc702b255e3405d98f89141e880f) (int reboots=5) |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) | |
| [AsyncAwaiterFacade](./classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterFacade.md) | [SwitchToConnectorThreadAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#af282ed2321a52123797b6abaef3a289c) () |
| | Await it to switch to the connector queue thread, does nothing if we on the connector thread already. |
| | |
| [AsyncAwaiterQueueFacade](./classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterQueueFacade.md) | [WaitConnectorQueueAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a78ecc4ed611500e27cace4198276053f) () |
| | Schedules continuation when all other tasks are done on connector's queue (adds a task itself to the end of the queue) |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| void | [Connect](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad77290066d5ec673645100d3ad1275d0) () |
| | Asynchronously starts connection process
 `Connected` event will fire indicating successful connection. |
| | |
| async Task | [ConnectAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#afd95903847b4a0d21b2698ab835d29bd) () |
| | Asynchronously starts connection process. |
| | |
| void | [Disconnect](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a78a4005e7f904c3eb6113911c34a4e63) () |
| | |
| async Task | [DisconnectAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a70da19a7db18ba74d261480000725077) () |
| | Asynchronously starts disconnection process. |
| | |
| void | [SearchSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a19ca4507436c720d4628d4afe60e509f) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | Searches for securities matching the specified filter. |
| | |
| Task > | [SearchSecuritiesAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae835683cfc2e9023ef25d398ebe92865) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | |
| void | [SubscribeToMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abc8be4646760d1d79a3bb0b0400aba51) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [SubscribeToMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a02ade369f899942dc1373bb3c84e0a18) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Subscribes to market data for a security. |
| | |
| void | [UnsubscribeFromMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a8e3eb8741cc2675c9dad5b881b692fa6) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [UnsubscribeFromMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a84568b2ca542e2a0f81c4ee198b8a64c) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Unsubscribes from market data for a security. |
| | |
| IEnumerable | [GetRoutes](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7aff547cff6d89b9467379f9b063c7ad) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| IEnumerable | [GetPortfolios](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a8d84351385f699098c586e9ff5281b04) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [TryGetOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0ea1e0df22fb22d0fcf9857b38c77250) (long extId) |
| | |
| bool | [TryGetOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a724e7cc7e13faa8b1df42fb3525b9912) (string marketOrderId, out [Order](./classATAS_1_1DataFeedsCore_1_1Order.md)? order) |
| | |
| Task | [RegisterOrderAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad5a4ef349b569f44c0c6896b38087223) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| Task | [ModifyOrderAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab9afd95c2bc06b215ccf2a086e2eb646) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| Task | [CancelOrderAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af92e80c366466d2ba12b6218d81c7248) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [RegisterOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab509d691f91f1f76e19f8c77220373a4) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [ModifyOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3e36fd4856f49fe46aaae95051cda47d) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | Modifies the price of an order. |
| | |
| void | [CancelOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aab5ad316484fe91c65baa2fd88384f66) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [GetPosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a774ab2a321b6eb6188d9c6a2c639df71) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? tPlusLimit) |
| | |
| async Task | [ClosePositionAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac37951dbe114336a1ab6f39486e31325) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| async Task | [ClosePositionsAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a09b9311b89a7c9b06858b933475a003c) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| async Task | [ChangeMarginParametersAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7f2547643bd859da8befb6ab6fb0dd93) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, bool? isIsolated, decimal? leverage) |
| | |
| async Task | [ChangeIsolatedMarginAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a2fc872d30337381563392ea37a99cfaf) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal value) |
| | |
| virtual ? decimal | [CalcLiquidationPrice](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aada611a01fd0adfd6e16222b4a85248a) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal margin) |
| | |
| virtual ? decimal? decimal maxRemovable | [CalcIsolatedMarginChangeRange](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9b3913dddd50d40c7d24816d920a956b) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| decimal | [ConvertCurrency](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae3b0d4288818b4ffe72ab216c45e8f69) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, string currencyFrom, string currencyTo, decimal volume, decimal? limitPrice=null, bool roundToLotSize=true) |
| | Converts volume from one currency to another Used for Notional value calculation. |
| | |
| decimal? | [CalcMaxOrderVolume](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af5eb163c054fbefbb0c10f198c2c338f) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice=null) |
| | Gets max possible volume for the order
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| decimal? | [CalcOrderCost](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a53900592509e66e0111531836ddae19c) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice, decimal volume, out object? detailing, bool giveDetailing=false) |
| | Calculates total order cost including commissions and initial margin and everything else
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| [ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? | [GetSecurityTradingOptions](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad4a06233c46c588e8bafc3740efea713) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | Gets possible TimeInForce for order and optional flags that may be passed when order is created
 If null default behaviour is expected:
 TimeInForce = DAY, GTC, FOK
 No order flags |
| | |
| Task > | [GetMyTradesAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa21139260925f72e5bfc6dca97dc0cdd) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime from, DateTime to) |
| | Get a list of my trades. |
| | |
| bool | [CanSetPositionAveragePrice](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac6801eb49f95e3c168d9711912fceee9) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| void | [SetPositionAveragePrice](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae2390f1792948c45af4928105b4727f7) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal avgPrice) |
| | |
| void | [DropMarketDataConnection](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#ad1bad363fc5eae529036619dc44319d7) () |
| | |
| void | [DropTradingConnection](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#ae0962cbf93c7300293a6eee640a5e8b0) () |
| | |
| void | [DropMarketDataConnectionDuringConnecting](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a1109b08fb603f18701c76d92a385940f) () |
| | |
| void | [DropTradingConnectionDuringConnecting](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a70900272b4f5ec37dd9b23eacae5c272) () |
| | |
| Task | [DropMarketDataConnectionCoupleTimes](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#ab1f1b75f681906516a572b0c1109b600) (int reboots=5) |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
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

| Protected Member Functions | |
| --- | --- |
| string | [GetKey](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a36855f8717797244e9e7c9e81b872d5a) () |
| | |
| string | [GetSecret](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a4702c5c0df0e7c1138af087b3eae15fe) () |
| | |
| async Task | [OnTestManyReconnections](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a736f01d34cc2a9865d089a7a91ee5ae0) (IEnumerable websockets, int reboots) |
| | |
| - Protected Member Functions inherited from [ATAS.DataFeedsCore.AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) | |
| | [AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a6103fe9b5583ee6838dbb5cfda44c1c8) () |
| | |
| override Task | [OnConnectAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a840c12b23ccf6db42b723f1d1e8ebf3a) () |
| | |
| sealed override void | [OnProcess](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a0b993b5005cf6ce546940f302afac23b) ([AsyncConnectorMessage](./classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md)? message) |
| | |
| void | [ProcessPosition](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a41b50f52f7d25e44f9b57f1febe84285) (TPortfolioKey portfolioKey, TSecurityKey securityId, Func update) |
| | |
| void | [ProcessPortfolio](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#af7d4832e5967a236434043ef72ba648a) (TPortfolioKey portfolioKey, string accountId, Action update) |
| | |
| void | [ProcessMyTrade](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#af4588b633c720c4d57c4b1692efe24b6) (long extId, string tradeId, Action update) |
| | |
| void | [ProcessMyTrade](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a9f63b73b886f4f28add12bb6bfcf9760) (string orderId, string tradeId, Action update) |
| | |
| void | [ProcessOrder](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#ab6d1ff51e5926fa93961460a0945f119) (TPortfolioKey accountId, TSecurityKey securityId, long extId, Action update) |
| | |
| new void | [ProcessOrder](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#ad120ddebcae384269ba6bb62d7eac8bf) (long extId, Action update) |
| | |
| abstract Task | [OnSubscribeMarketDataAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a5ffc809eeebd3af1af4618120b2717dd) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| sealed override async void | [OnSubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a7cd7eccc3a588d7582d1a3153a4b84c5) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| abstract Task | [OnUnsubscribeMarketDataAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a12b6a5298afb28279beffe7bb9bcdba8) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| sealed override async void | [OnUnsubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a02f4fb2558e3b191ecf4153876c5ede6) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| abstract Task | [OnSearchSecuritiesAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#aeb6999b2ffc5e86dbaaae8333397387a) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | |
| sealed override async void | [OnSearchSecurities](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a944e3b8c3cbe063d67a813922dc6a351) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | |
| abstract Task | [OnRegisterOrderAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#af4812021c870bc59019d02e33f9b34ac) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) filter) |
| | |
| sealed override async void | [OnRegisterOrder](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a5894f8f5bf6215777ef72248f38d33a8) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| abstract Task | [OnCancelOrderAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#afa4defc9d9965b4f78f12a2d2c20b020) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, long extId) |
| | |
| sealed override async void | [OnCancelOrder](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a2781f66a7e7587691708c1234f350265) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, long extId) |
| | |
| virtual Task | [OnModifyOrderAsync](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#add65b0d21981fc9ff9777e3b70e21fbf) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| sealed override async void | [OnModifyOrder](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a341a60199746f492e4ed0dac59f3b074) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| - Protected Member Functions inherited from [ATAS.DataFeedsCore.BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| | [BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9e9612966422f076dbc6d72ca1ba36f7) () |
| | |
| | [BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abadbb8c0077ffc5a8b0692877847c71f) (long initialId) |
| | |
| | [BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9e9612966422f076dbc6d72ca1ba36f7) () |
| | |
| | [BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abadbb8c0077ffc5a8b0692877847c71f) (long initialId) |
| | |
| virtual Task | [OnConnectAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a638a45671b2313cb24fc77236428e8d1) () |
| | |
| virtual void | [OnConnect](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3d4eca78fedacf94203a5a83c0e77b67) () |
| | A place for connection routines
 Executed on thread-pool thread. |
| | |
| void | [OnConnected](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a14e4431e82f5011dd8a4ca4d2845164d) () |
| | Raises `Connected` event
 The event must be called when the connector have all necessary connections established and authenticated
 All security objects must be processed before this call if possible
 If there were any subscriptions, search for that securities will be performed. |
| | |
| virtual Task | [OnDisconnectAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a09ac3b7a08e61fd308b23fd3be82d6ec) () |
| | |
| virtual void | [OnDisconnect](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0ee3b95b49106c87f85d650050386848) () |
| | Called on thread-pool thread when Disconnect() method is invoked. |
| | |
| void | [OnDisconnected](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a72cc3f8ce3cb8118c4f30dc6cf74f954) () |
| | Cleans up base resources and raises `Disconnected` event. |
| | |
| void | [OnConnectionError](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a43550e5c5d32d2737ea22a5565fa2cad) (Exception error) |
| | Remember the error that will be handled when the OnDisconnected is called. |
| | |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a1cb6d43d03ed651bc6484fa047a76b43) (TMessage message) |
| | |
| void | [Enqueue](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7294abfce5aa03d2599855c5ccb1145e) (Action action) |
| | |
| abstract void | [OnProcess](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a1a801fbcef9917d2861817ba05cf7a60) (TMessage message) |
| | Handles queued messages by the MessageQueue
 In case of SimpleMessageQueue it allows to process all messages in the special connector's queue thread. |
| | |
| virtual void | [OnSendHeartbeat](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a40b84ba496b78b0df9d648aa80436c22) () |
| | |
| void | [ProcessSecurity](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aec0af78416bf70ff83654c886a03488e) (TSecurityKey id, Action action) |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md)? | [GetSecurity](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af573e103537855585c8e21736d2b877c) (TSecurityKey securityId) |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md)? | [GetPortfolio](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a71d42036b63c11d76f65edf9d7415d3e) (TPortfolioKey portfolioKey) |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [ProcessPortfolio](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a667787cd3d73a1608d4f111937904d7a) (TPortfolioKey id, string accountId, T message, Action update) |
| | |
| void | [ProcessPortfolio](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aee0c347fceae39e367ef8017b35c3618) (TPortfolioKey id, T message, Action update, bool throwIfNotFound=false) |
| | |
| void | [ProcessPosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac8311683af5411355adb7ae64855e9da) (TPortfolioKey accountId, TSecurityKey securityId, [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? tPlusLimit, T message, Func update) |
| | |
| void | [ProcessPosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a465f92e26efeeaf5ffd4c581c6d27217) (TPortfolioKey accountId, TSecurityKey securityId, T? message, Func update) |
| | |
| void | [ProcessSecurity](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aeee540c1f0bcf45963c39406790e1043) (TSecurityKey id, string securityId, T message, Action update) |
| | Converts internal security object of the connector into the ATAS-security visible to the system. |
| | |
| void | [ProcessBestBidAsk](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad250c6f78c4c47abc14ccfc4285a48c2) (TSecurityKey securityId, DateTime time, [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) type, decimal price, decimal volume, string ecn="") |
| | |
| void | [ProcessBestBidAsk](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a75fbee0cee0258a15caedb632e474e2e) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime time, [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) type, decimal price, decimal volume, string ecn="") |
| | |
| void | [ProcessBestBidAsk](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab50b1366bbd85c3955369a7c600e0b26) (TSecurityKey securityId, [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) marketDepth) |
| | |
| void | [ProcessBestBidAsk](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a464ca40beb908a8e182b4700881d0a36) ([MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) marketDepth) |
| | |
| void | [ProcessTick](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac444424cc5d66788831bdb332925ebff) (TSecurityKey securityId, long id, DateTime time, [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) direction, decimal price, decimal volume, string ecn="", decimal? openInterest=null) |
| | |
| void | [ProcessTick](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a1c03a6d5b68555ac1e35098ac3c9be25) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, long id, DateTime time, [TradeDirection](../namespaces/namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) direction, decimal price, decimal volume, string ecn="", decimal? openInterest=null) |
| | |
| void | [ProcessTick](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0b95d353b51b6ac8a8229ff0e57df267) (TSecurityKey securityId, [Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) trade) |
| | |
| void | [ProcessTick](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad2630c4e7c1b136e2c8c1dad19fc0967) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) trade) |
| | |
| void | [ProcessTick](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0893a38fecc7cc4cff49f8faee71086c) ([Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) trade) |
| | |
| void | [ProcessMarketDepthsReset](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae3039124c2628827d9e231a63ef2fbca) (TSecurityKey securityId, ICollection depth) |
| | |
| void | [ProcessMarketDepths](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9ad287b8c3789896627e832d97c95303) (TSecurityKey securityId, Func > action) |
| | |
| void | [ProcessMarketDepths](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7bf9be9be8b300d7c888a3781ebf5af8) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, Func > action) |
| | |
| void | [ProcessMarketByOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3b26339c57fe8f9ea3062b995cdc3543) ([MarketByOrder](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md) mbo) |
| | |
| void | [ProcessSummary](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a2812983d3bc92fcdbb72bfbfe270a935) (TSecurityKey id, Action action) |
| | |
| void | [ProcessOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a43c1d3bf4019d5e4eeab3e14477d4af7) (TPortfolioKey accountId, TSecurityKey securityId, long extId, T message, Action update) |
| | |
| void | [ProcessOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a6d478bde0797f3b7d54fd18e191ff528) (TPortfolioKey accountId, TSecurityKey securityId, long extId, T message, Func update) |
| | |
| void | [ProcessOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a51d30239eed584a054ecfedb0d376def) (long extId, T message, Action update) |
| | |
| void | [ProcessOrderError](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab1c73c3106e01275b15d41e53424f663) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string error) |
| | |
| void | [ProcessOrderError](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#afc6538ff81307135d501b84835c94c60) (long extId, string error) |
| | |
| void | [ProcessOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0807578862006f8bea0481b77b74f344) (long extId, Action process) |
| | |
| void | [ProcessMyTrade](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#adeda842670fbd790ee3cf8a418b45dde) (TPortfolioKey accountId, TSecurityKey securityId, long extId, string orderId, string tradeId, T message, Action update) |
| | |
| void | [ProcessMyTrade](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a29ffe3b88e33b4991ae3f0c1823a00ed) (long extId, string orderId, string tradeId, T message, Action update) |
| | |
| void | [ProcessMyTrade](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9494daeebd314931652c7cdfb794f6de) (long extId, string tradeId, T message, Action update) |
| | |
| void | [ClearDataForPortfolio](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad381f4ea1114db450176385370dc9b89) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| virtual TSecurityPositionManager | [CreatePositionManager](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3ca48ec9242b9d0da9f03b8d847fe8f5) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| virtual void | [PositionPnLChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0e12741b63fc861e3c6f9a38ed00be15) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| void | [RaiseRebateResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abc8bea28d4589ce1e927ac2a2e2512ae) ([RebateResult](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace8c78210ca5b50f90165046c04f5e7a) result) |
| | |
| void | [RaiseConnected](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9108c3be0b0c215c1056e3fc2f45069f) () |
| | |
| void | [RaiseDisconnected](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0f47e3032a93ce190ec4beac091e2b65) () |
| | |
| void | [RaisePosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a1165873817ba131b385a9dd660f140c3) (TSecurityPositionManager posManager, bool isNew) |
| | |
| void | [RaiseOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad1e280a0502e1a394eaecd11e1fcebf6) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isNew) |
| | |
| void | [RaiseOrderFailed](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a44b3dd2c334a9434817bb02b3bcedc6d) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string error) |
| | |
| void | [RaiseSearchSecuritiesResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#afdf311cb8d800a3cf2f5bc9bb4fc1c9a) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter, Exception? error, IEnumerable securities) |
| | |
| void | [RaiseRegisterSecurityResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a30c5539e8d146e031caf86e4936a477e) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionType, Exception? error) |
| | |
| void | [RaiseSubscribeMarketDataResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a50fb9c8c4571261d26103f61200d891e) (IEnumerable securities, Exception? error=null) |
| | |
| void | [RaiseUnsubscribeMarketDataResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa7be0339cb7f3c5203eebd523204a1b1) (IEnumerable securities, Exception error) |
| | |
| void | [RaiseNews](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a531a4ae4bebf8919d3eba21fe8fa75b8) ([News](./classATAS_1_1DataFeedsCore_1_1News.md) news) |
| | |
| void | [RaiseSecurityChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a771eeb1b4021fc3881fdb99bf0586ab9) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| void | [RaiseSecuritySummaryChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a2ef6848d1f559d1022c221cc5e95601c) ([SecuritySummary](./classATAS_1_1DataFeedsCore_1_1SecuritySummary.md) summary) |
| | |
| [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | [GetCurrentSubscription](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9f0effce4935f527dfe9755806085ae5) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| virtual void | [OnSearchSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af7a33eebdb07ad9ab9d7e9ffb9f765ff) (TSecurityKey securityId, TMessage message) |
| | |
| abstract void | [OnSearchSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abab9e17996170cf9850c11cc2e87e258) ([SecurityFilter](./classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | |
| abstract void | [OnSearchSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a943c3d5d0d0141b2c9fd1c2a4d25585c) (TSecurityKey securityId) |
| | |
| virtual void | [OnSubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa5c7b20e9bfeba6f74272374cc52afd0) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscription) |
| | |
| abstract void | [OnSubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa2d2aa4204670c54da61b031a9101f36) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| virtual void | [OnUnsubscribeFromMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a5166392e31e6b515f4209dd56bfc1fbb) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscription) |
| | |
| abstract void | [OnUnsubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab26951ef595d5165bd104fe7eee3146d) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| virtual IEnumerable | [OnGetRoutes](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad396d55e1b50e07b7ce7c51b625937b9) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| virtual IEnumerable | [OnGetPortfolios](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a52bd9273adda7ef1341505f59d480e08) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| abstract void | [OnRegisterOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a706a4667dc895f0d84756e0ee1f1bf80) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| virtual void | [OnModifyOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad178cb00a040d7197e4bf6fce97951b0) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| abstract void | [OnCancelOrder](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae6c4f5137dbb9a711c37db18560a6fcc) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, long extId) |
| | |
| void | [RememberPendingOrders](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a842e3f7557d9ac9594c5de548bfa4c22) () |
| | |
| virtual async Task | [OnClosePosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7e5170ab9cb76a9135e2e3d8fbb23ed1) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| virtual async Task | [OnClosePositions](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a84e9cd6fc29612c69b927a0902a7f0b8) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| virtual [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [CreateOrderToClosePosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9a8fcededdefda25d95f58156ace9917) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) pos) |
| | |
| virtual Task | [OnChangeMarginParametersAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae4ea65a63b16cd8e51f984911bcee109) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, bool? isIsolated, decimal? leverage) |
| | |
| virtual Task | [OnChangeIsolatedMarginAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a5112884f1092ad3811412f83285d0e54) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal value) |
| | |
| virtual decimal | [OnConvertCurrency](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a66cbb2c69a03bc62146961e682f3b2bf) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, string currencyFrom, string currencyTo, decimal volume, decimal? limitPrice=null, bool roundToLotSize=true) |
| | Converts volume from one currency to another Used for Notional value calculation. |
| | |
| virtual ? decimal | [OnCalcMaxOrderVolume](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a621d99fb014adabba73d55cdb0a9caee) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice=null) |
| | |
| virtual ? decimal | [OnCalcOrderCost](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a24c5c7198548eac38e90b8226e48bbee) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice, decimal volume, out object? detailing, bool giveDetailing=false) |
| | |
| virtual ? [ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md) | [OnGetSecurityTradingOptions](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa1c6cfce12e7956b4e805dd6fb6488b1) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| virtual Task > | [OnGetMyTradesAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9e2568384105c3cf2d2cde04326bc785) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime from, DateTime to) |
| | |
| void | [ValidateTradesHistoryRequestDates](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a8e4a2e87d3de98606486040f92709c6c) (ref DateTime from, DateTime to, int depthLimitMonths) |
| | |
| virtual Task | [GetApiServerTime](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a952ecb8caf88b56203d72ec103590391) (CancellationToken cancellationToken) |
| | |
| async Task | [ProcessAutoRefreshSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af329b4d4c8729332424365d729ee3478) () |
| | |
| virtual async Task | [OnRefreshSecuritiesAsync](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aab396d5ceffe5807b3780ab04ca52afe) () |
| | |
| void | [BeginProcessingDelistedSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9405c99e8ea5f7fa3ada67dce07ded21) () |
| | |
| void | [CompleteProcessingDelistedSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a17b4e341d71154fda1f0baa2f956d4bc) () |
| | |
| bool | [TryGetPosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aeb6edb6b5a9819e8156e0a8f125caa8e) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, out [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| bool | [TryGetPosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a11d66840ca55363df51b9cf3d9380c58) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, out [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| bool | [TryGetPosition](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a008ead00cd7e9a324b1f125754371ebe) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793) limits, out [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| void | [UpdatePositionsPnL](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0da51166bd5cd8232a6291486c6f0059) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) type, decimal price) |
| | |

| Properties | |
| --- | --- |
| SecureString? | [ApiKey](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a627a573de7b720827ab46a6dbf98f5b0)`[get, set]` |
| | |
| SecureString? | [Secret](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#ad9df166303b1ba0fd52efb18114b0b8c)`[get, set]` |
| | |
| bool | [MarketDataOnly](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md#a9c29cc5c7e1da2767aa51aa49004cbca)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.AsyncConnector](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) | |
| int | [ConnectorThreadId](./classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#a1bb1e593cb60294ae0d2ddf7ad567153)`[get]` |
| | Gets the ManagerThreadId of the Queue thread. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| IIdGenerator | [IdGenerator](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a4ae66837a074373c9f44c0836a656443)`[get]` |
| | |
| IEnumerable | [PositionManagers](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a21d7b14c74f7ef642cd862d1f4b695cb)`[get]` |
| | |
| ITimeSyncManager | [TimeSyncManager](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aad4eac5bc6ac78e3dea5b5365b176476)`[get]` |
| | |
| abstract bool | [IsSupportedServerOCO](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a23a755246b4e66aad827d36d4ec0037a)`[get]` |
| | Indicates whether the connector supports server-side OCO orders. |
| | |
| abstract bool | [IsSupportedStopOrders](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aefc730b5c0d39e98cbf28d3c5c46726f)`[get]` |
| | Indicates whether the connector supports stop orders. |
| | |
| abstract bool | [IsSupportedTradingFunctions](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a1946ad75bad8f78ea64c8f46ebe9ca1e)`[get]` |
| | Indicates whether the connector supports trading functions. |
| | |
| abstract bool | [IsSupportedRussianMarket](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#acb8fe12cc4109226b6c4cca9c2122c06)`[get]` |
| | Indicates whether the connector supports Russian market instruments. |
| | |
| abstract bool | [IsSupportedAmericanFutures](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aeaaf3c8be24431e3528e07c7dcf8b7c2)`[get]` |
| | Indicates whether the connector supports American futures. |
| | |
| abstract bool | [IsSupportedAmericanStocks](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a45d82fdbc4e09ba8a5f6ca774cd5175d)`[get]` |
| | Indicates whether the connector supports American stocks. |
| | |
| virtual bool | [IsSupportedCrypto](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a400c52cc5b95aaf558450ba8a4609b03)`[get]` |
| | |
| virtual bool | [IsSupportedCfd](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7fa9ec4593d291d4fbdc2714d04a9480)`[get]` |
| | |
| virtual bool | [IsSupportedMaxOrderCalculation](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abacab57b817694a0d73d682472cbeb9b)`[get]` |
| | |
| virtual bool | [IsSupportedServerTime](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a82dbc5923b6465489197f505c49e31be)`[get]` |
| | |
| bool | [MarketDataStreamEnabled](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a740f9048911def8c6a1ee674a209fdd2)`[get, set]` |
| | Indicates whether to raise market data through BestBidAskUpdates, MarketDepthsUpdate, and NewTrades events. |
| | |
| bool | [AllowUpdatePositionsPnL](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab3ab9092292113ebe2134762b4ff6e7d)`[get, set]` |
| | |
| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) | [MarketDataDelayPeriod](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a69ace05c12029e51e433cfc93e45ce93)`[get, set]` |
| | Gets or sets delay period for market data. |
| | |
| [IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) | [Factory](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a651e143caed6f4ef81560a253e83c555)`[get, set]` |
| | Factory used to create Security, Portfolio, and other entity objects. |
| | |
| ITimeSyncManager? | [DefaultTimeSyncManager](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a18b496cd7489ee64ac3eefa9f498ae26)`[get, set]` |
| | |
| [IConnectorExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) | [ExchangeInfoProvider](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7245c1c96b5caca40ae8fe503b2bc3a2)`[get, set]` |
| | |
| string | [DataPath](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0af66495fda4cab45a3fe1bf2b01ddc7)`[get, set]` |
| | |
| [IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) | [MessageQueue](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a250c8622869b82f86bdcc3662030b447)`[get, set]` |
| | |
| IEnumerable | [Securities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad337f78d56e41b33df81a538661e657e)`[get]` |
| | |
| IEnumerable | [Portfolios](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa7ab869759933e0a876d1941f82fd241)`[get]` |
| | |
| IEnumerable | [MyTrades](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a5ab4219ad9cbb8b5bef3eb7f63b4fc6b)`[get]` |
| | |
| IEnumerable | [Orders](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aac816b04a326ebb0d3088692d3906da1)`[get]` |
| | |
| IEnumerable | [Positions](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a2fe5d0f3982e9e67963b1d7e8b83adaa)`[get]` |
| | |
| bool | [IsConnected](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a569810569a913dad7b693aa899d94fbf)`[get, protected set]` |
| | Returns true when all connections are established. |
| | |
| ConnectionStates | [ConnectionState](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a99c5aa250214c54c01230312312e456a)`[get]` |
| | Current connection state of the connector. |
| | |
| ReconnectionTimer | [ReconnectionTimer](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a6c7424f64a366c7c8df275974e08ad71)`[get]` |
| | |
| bool | [ReconnectOnFirstConnect](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3a3c020ff40dcf323339ee750c62bbe5)`[get, set]` |
| | |
| bool | [ServerMode](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9f76a23c729a45812d359fc40f1e7ebb)`[get, set]` |
| | |
| TimeSpan | [HeartbeatTimeout](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a313ebc4bdd5ee80abca07425a1d4d868)`[get, set]` |
| | |
| TimeOnly? | [RefreshSecuritiesTime](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa3396be51e40753679e094b81b2ab42b)`[get, set]` |
| | |
| virtual bool | [HasPendingActions](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a5d3eced3b9406f0c1f338216f02a7777)`[get]` |
| | |
| [IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | [LatencyManager](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac8c211c97cf47e4083fe02a3c3ad246f)`[get]` |
| | Latency manager. |
| | |
| bool | [IsFullLicense](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac03202fba8bc165b2833308a5ca99396)`[get, set]` |
| | License type. |
| | |
| bool | [NeedRebatesCheck](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a953c9a65dbd3d973e6e94b275c4dd9bb)`[get, set]` |
| | Has rebate check feature. |
| | |
| Guid | [Id](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a46f1758db8613f2d5919480eafb463c4)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md) | |
| SecureString? | [ApiKey](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a444bda81564fc32836bce09c65107a05)`[get, set]` |
| | |
| SecureString? | [Secret](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a571fd98d7a860462494f65cd384e989b)`[get, set]` |
| | |
| bool | [MarketDataOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#aca16ece73eefb29c86167653792304ec)`[get, set]` |
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

| Additional Inherited Members | |
| --- | --- |
| - Public Attributes inherited from [ATAS.DataFeedsCore.BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| virtual ? decimal | [maxAddable](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#afcbef86a296758e4da7411fd496504c8) |
| | |
| - Public Attributes inherited from [ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| decimal? | [maxAddable](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac9cd86c5d4d19df2c4c43793280a9eab) |
| | Calculates possible changes of the isolated margin of a position (only for leveraged trading) |
| | |
| - Events inherited from [ATAS.DataFeedsCore.BaseConnector](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| ConnectorEventHandler? | [Connected](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abe718451aa6a97c103132456cb07b994) |
| | |
| ConnectorEventHandler? | [Disconnected](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3389521f748cca11c4d9bf03c1d68fe7) |
| | |
| ConnectorEventHandler? | [ConnectionStateChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a80e8ece7b95ec8dee476f51e1327455f) |
| | |
| ConnectorEventHandler >? | [NewMyTrades](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#acb69a2d490255e8c72629c354145b51a) |
| | |
| ConnectorEventHandler >? | [NewOrders](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae767f303bbdf05a8c30b7dfe9e2df986) |
| | |
| ConnectorEventHandler >? | [NewPositions](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a362772c239c6787d7e7b603859700179) |
| | |
| ConnectorEventHandler >? | [NewPortfolios](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9883efbd4d8eb5cfbe25c66636677628) |
| | |
| ConnectorEventHandler >? | [NewSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af59b5825b5469e087aad70b3d339f6a0) |
| | |
| ConnectorEventHandler >? | [RemoveSecurities](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a4fdb2d11ca2c2a9577b78a9d90221ef0) |
| | |
| ConnectorEventHandler? | [OrderChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae5e8c4e91a65c0e98bf48ae2f8d0e608) |
| | |
| ConnectorEventHandler? | [RebateReceived](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a12ebf1c975b7dcb237236f3feee3aef1) |
| | |
| ConnectorEventHandler? | [OrdersRegisterFailed](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7db5d047da191f2562f52a32d00c334a) |
| | |
| ConnectorEventHandler? | [OrdersCancelFailed](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a01495490d508db37d25f0155aa1858ba) |
| | |
| ConnectorEventHandler? | [OrderModifyFailed](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ace733cda4169b1b87399a6777152d746) |
| | |
| ConnectorEventHandler >? | [PortfoliosChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7f229bca182d284ecbf924eb3ce1522a) |
| | |
| ConnectorEventHandler >? | [PositionsChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a2d85612ba44ee41c2264b282e19dc933) |
| | |
| ConnectorEventHandler? | [BestBidAskUpdates](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7c841eac844ab645b981a9d9e6bc3d27) |
| | |
| ConnectorEventHandler >? | [MarketDepthsUpdate](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af4a4ee1e0f46379e4b9b886e90405ba2) |
| | |
| ConnectorEventHandler | [MarketByOrderChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0a8afadc111bf498665a1efdc335f1ea) |
| | |
| ConnectorEventHandler >? | [NewTrades](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab87ed22833a66f8904075586c615bec6) |
| | |
| ConnectorEventHandler? | [Error](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a39b6a9b35167a9b5122801e59eee72f7) |
| | |
| ConnectorEventHandler >? | [SearchSecuritiesResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae8bfc2ef8e256b49dd57b15ab0ff8ce8) |
| | |
| ConnectorEventHandler? | [RegisterSecurityResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a8dec229c96aaa7a0a655bf58382a1b43) |
| | |
| ConnectorEventHandler? | [NewNews](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a26e56a99463778bb48ef76d61160b35a) |
| | |
| ConnectorEventHandler? | [SecurityChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#afe11767de61ca9fc39b7cf149e06ca97) |
| | |
| ConnectorEventHandler? | [SecuritySummaryChanged](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a09723a49b933c960493ab98b55b87356) |
| | |
| ConnectorEventHandler, Exception?>? | [SubscribeMarketDataResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3b42421a63f9fd8989cc651719ea6955) |
| | |
| ConnectorEventHandler, Exception?>? | [UnsubscribeMarketDataResult](./classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abb4be64c080578b72707bf2466ae6210) |
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

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CheckClientMode()

| void [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).CheckClientMode | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)DropMarketDataConnection()

| abstract void [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).DropMarketDataConnection | ( | | ) | |
| --- | --- | --- | --- | --- |

pure virtual

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#ad1bad363fc5eae529036619dc44319d7).

## [◆](https://docs.atas.net/en/)DropMarketDataConnectionCoupleTimes()

| abstract Task [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).DropMarketDataConnectionCoupleTimes | ( | int | reboots = `5` | ) | |
| --- | --- | --- | --- | --- | --- |

pure virtual

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#ab1f1b75f681906516a572b0c1109b600).

## [◆](https://docs.atas.net/en/)DropMarketDataConnectionDuringConnecting()

| abstract void [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).DropMarketDataConnectionDuringConnecting | ( | | ) | |
| --- | --- | --- | --- | --- |

pure virtual

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a1109b08fb603f18701c76d92a385940f).

## [◆](https://docs.atas.net/en/)DropTradingConnection()

| abstract void [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).DropTradingConnection | ( | | ) | |
| --- | --- | --- | --- | --- |

pure virtual

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#ae0962cbf93c7300293a6eee640a5e8b0).

## [◆](https://docs.atas.net/en/)DropTradingConnectionDuringConnecting()

| abstract void [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).DropTradingConnectionDuringConnecting | ( | | ) | |
| --- | --- | --- | --- | --- |

pure virtual

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a70900272b4f5ec37dd9b23eacae5c272).

## [◆](https://docs.atas.net/en/)GetKey()

| string [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).GetKey | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)GetSecret()

| string [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).GetSecret | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)OnTestManyReconnections()

| async Task [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).OnTestManyReconnections | ( | IEnumerable | websockets, |
| --- | --- | --- | --- |
| | | int | reboots |
| | ) | | |

protected

Type Constraints

| T | : | ConnectorWebsocket.ConnectorWebsocket | |
| --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)ApiKey

| SecureString? [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).ApiKey |
| --- |

getset

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a444bda81564fc32836bce09c65107a05).

## [◆](https://docs.atas.net/en/)MarketDataOnly

| bool [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).MarketDataOnly |
| --- |

getset

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#aca16ece73eefb29c86167653792304ec).

## [◆](https://docs.atas.net/en/)Secret

| SecureString? [ATAS.DataFeedsCore.CryptoAsyncConnector](./classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md).Secret |
| --- |

getset

Implements [ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md#a571fd98d7a860462494f65cd384e989b).

The documentation for this class was generated from the following file:
- [ICryptoKeySecretConnector.cs](../files/ICryptoKeySecretConnector_8cs.md)
