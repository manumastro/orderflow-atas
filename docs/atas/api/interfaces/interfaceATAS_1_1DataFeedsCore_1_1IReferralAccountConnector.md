# ATAS.DataFeedsCore.IReferralAccountConnector Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IReferralAccountConnector.html

Inheritance diagram for ATAS.DataFeedsCore.IReferralAccountConnector:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.IReferralAccountConnector:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| Task | [IsAccountBindedWithAtas](./interfaceATAS_1_1DataFeedsCore_1_1IReferralAccountConnector.md#a2115fb2ac5a965a6f50f31121c7d765e) () |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| void | [Connect](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4a0a52aa5eea9e4fffb0013ae87669a6) () |
| | |
| void | [Disconnect](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a3c5fc1ab12e6f14d81c4536e22bfdc13) () |
| | |
| Task | [ConnectAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#adb0d00d737364d2b46b57cf6ba85b5d8) () |
| | |
| Task | [DisconnectAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae63c5fe20ea7f79b664db4e4f5ef296d) () |
| | |
| Task | [RegisterOrderAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8a2a7ec6471a430c5ca6f1a5ff4da99a) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| Task | [ModifyOrderAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a591428f4c5bf4b0ff38bdea0fe3d1440) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| Task | [CancelOrderAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a96137f3442df97f6ea0a3492383a0f9b) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [CancelOrder](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9865560841a56d8d4c9283d726d32651) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [RegisterOrder](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a19f13ee0baed1e8844817f324c315df4) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [ModifyOrder](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a367624de3add86b5c676432d800aebba) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) neworder) |
| | Modifies the price of an order. |
| | |
| [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | [TryGetOrder](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a559e4fd70f1571898b06c17817fd2e3a) (long extId) |
| | |
| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | [GetPosition](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a36f29e065945aec4fa161645c0d4217e) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? tPlusLimit) |
| | |
| Task | [ClosePositionAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a5a7c9fdaa94c404fa9f2783ba9d1abb2) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| Task | [ClosePositionsAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1c283e520d5566f1e946d0bcaab7875a) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| void | [SubscribeToMarketData](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a14e15f0373b047d97b274bb33c484466) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [SubscribeToMarketData](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a76085b20e7d1ac6fa72b01088929a88a) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Subscribes to market data for a security. |
| | |
| void | [UnsubscribeFromMarketData](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#adbd503742509a515328789dcd68924f5) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | |
| void | [UnsubscribeFromMarketData](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa5af9f893563ee262e182440d90f7223) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subscriptionTypes) |
| | Unsubscribes from market data for a security. |
| | |
| void | [SearchSecurities](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af9315f6c954ff5d935453832ca815858) ([SecurityFilter](../classes/classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | Searches for securities matching the specified filter. |
| | |
| Task > | [SearchSecuritiesAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acde2d33931ba35a3e6f309d5f9ff7eea) ([SecurityFilter](../classes/classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) filter) |
| | |
| IEnumerable | [GetRoutes](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae37580c81aa36c63a3711ff76f94f6a1) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| IEnumerable | [GetPortfolios](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a130055b35f1571b13254571d9e193f23) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | |
| Task | [ChangeMarginParametersAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8033291e306d851abc6d4d48ad4daa07) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position, bool? isolated=null, decimal? leverage=null) |
| | Switches position margin mode (isolated/cross) and/or trading leverage for it. |
| | |
| Task | [ChangeIsolatedMarginAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a6ee2cd1c376dcd72f448770dea7a8bb5) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal value) |
| | Adds or removes to isolated margin of a position. This method does not apply for crossed margin mode
 This method works only if Position.Risk property is not null. |
| | |
| decimal? | [CalcLiquidationPrice](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#afd41e76fd5571e3d8c6de4cc4b94009a) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position, decimal margin) |
| | Allows to estimate Liquidation Price change for a margin. |
| | |
| decimal?? decimal maxRemovable | [CalcIsolatedMarginChangeRange](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aebaf24ee33f44cf6fbd9a4c72eeaad5e) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| decimal | [ConvertCurrency](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4d1320f0dbc29cae7bbd8e0747114604) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, string currencyFrom, string currencyTo, decimal volume, decimal? limitPrice=null, bool roundToLotSize=true) |
| | Converts volume from one currency to another Used for Notional value calculation. |
| | |
| decimal? | [CalcMaxOrderVolume](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a2a11c5de42c99c3418bb77b7d7399252) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice=null) |
| | Gets max possible volume for the order
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| decimal? | [CalcOrderCost](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9ddc40b5df1e98db610d9407dd04844) ([OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) orderType, [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal? limitPrice, decimal volume, out object? detailing, bool giveDetailing=false) |
| | Calculates total order cost including commissions and initial margin and everything else
 This function is always return null if IsSupportedMaxOrderCalculation is false. |
| | |
| [ISecurityTradingOptions](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? | [GetSecurityTradingOptions](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9ca66fbf2eaf52f4d7ce60378ab4173e) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security) |
| | Gets possible TimeInForce for order and optional flags that may be passed when order is created
 If null default behaviour is expected:
 TimeInForce = DAY, GTC, FOK
 No order flags |
| | |
| Task > | [GetMyTradesAsync](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa648c2ceec0bb7d9198e1482102e3be9) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, DateTime from, DateTime to) |
| | Get a list of my trades. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Public Attributes inherited from [ATAS.DataFeedsCore.IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| decimal? | [maxAddable](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac9cd86c5d4d19df2c4c43793280a9eab) |
| | Calculates possible changes of the isolated margin of a position (only for leveraged trading) |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| Guid | [Id](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af690ae69693a9413c01048a4be66160d)`[get]` |
| | |
| bool | [IsSupportedServerOCO](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a065d47a51b043efc0f98ff038f41c0f7)`[get]` |
| | Indicates whether the connector supports server-side OCO orders. |
| | |
| bool | [IsSupportedStopOrders](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae14df0340e84ba634bb2f9286ea99997)`[get]` |
| | Indicates whether the connector supports stop orders. |
| | |
| bool | [IsSupportedTradingFunctions](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab6d379a91ea977907f64d7397885908a)`[get]` |
| | Indicates whether the connector supports trading functions. |
| | |
| bool | [IsConnected](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a7bb79009c97cba15552ca6608b826a41)`[get]` |
| | |
| ConnectionStates | [ConnectionState](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acc60d0101a2ad2916b840e4fb11b93e1)`[get]` |
| | Current connection state of the connector. |
| | |
| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) | [MarketDataDelayPeriod](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4b7f969cd066164ce1441bc790efcda2)`[get, set]` |
| | Gets or sets delay period for market data. |
| | |
| bool | [IsSupportedRussianMarket](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aecc8754caa33f468ed114dde5aa2a5eb)`[get]` |
| | Indicates whether the connector supports Russian market instruments. |
| | |
| bool | [IsSupportedAmericanFutures](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a83b5ae19a6279720631986344d780d79)`[get]` |
| | Indicates whether the connector supports American futures. |
| | |
| bool | [IsSupportedAmericanStocks](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9d7f85c3ef81479344acd9815f13ec0)`[get]` |
| | Indicates whether the connector supports American stocks. |
| | |
| bool | [IsSupportedCrypto](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac7585da8dd34e8e11e87b908a15bc75f)`[get]` |
| | Indicates whether the connector supports crypto instruments. |
| | |
| bool | [IsSupportedCfd](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aea6a8abf0f59bd74b101f33abfdbb2ad)`[get]` |
| | Indicates whether the connector supports CFD instruments. |
| | |
| bool | [IsSupportedMaxOrderCalculation](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a2f21b7d206667e239193611506635728)`[get]` |
| | Does connector support max order calculation for the instrument
 This feature enables percentage slider under the volume input box
 Affects CalcMaxOrderVolume and CalcOrderCost methods. |
| | |
| bool | [IsSupportedServerTime](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab17540c72cb365741341106225dd5ec0)`[get]` |
| | Does connector supports getting exchange server time. |
| | |
| bool | [MarketDataStreamEnabled](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aad0325db053fc101ebfa102ff3596a30)`[get, set]` |
| | Indicates whether to raise market data through BestBidAskUpdates, MarketDepthsUpdate, and NewTrades events. |
| | |
| bool | [AllowUpdatePositionsPnL](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1a3ce2be58309d0dea4919135e59f322)`[get, set]` |
| | |
| [IEntityFactory](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) | [Factory](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a8b148f7401ca80d932bca01613ba1cd5)`[get, set]` |
| | Factory used to create Security, Portfolio, and other entity objects. |
| | |
| ITimeSyncManager? | [DefaultTimeSyncManager](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0308d83cd28dde8844a3010ef6f2e2b4)`[get, set]` |
| | Default ITimeSyncManager to get time difference with NTP server. |
| | |
| [IConnectorExchangeInfoProvider](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) | [ExchangeInfoProvider](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0346c82f55f7364b43b46ce245ba92a7)`[get, set]` |
| | Gets or sets the provider used to retrieve exchange information for the securities. |
| | |
| string | [DataPath](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aee500f7c55bd7453290ac970e1bf2134)`[get, set]` |
| | |
| [IConnectorLatencyManager](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | [LatencyManager](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a49925c155cbad25cd934be5008859520)`[get]` |
| | Latency manager. |
| | |
| bool | [IsFullLicense](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae40d6cf32d10ac4ea546eea49448b7bc)`[get, set]` |
| | License type. |
| | |
| bool | [NeedRebatesCheck](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a02bf5da51c33e95e55e0ea44ce67da90)`[get, set]` |
| | Has rebate check feature. |
| | |
| IEnumerable | [Securities](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a455a328675ad1821a38404c2d184d0e6)`[get]` |
| | |
| IEnumerable | [Portfolios](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1e56eda4bef1e2e2e725a7d081d101bf)`[get]` |
| | |
| IEnumerable | [MyTrades](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a93c0b99a1222877d1fe70b1a0cd2d296)`[get]` |
| | |
| IEnumerable | [Orders](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac1416ecd2cf2a02aa361fe335fdb342d)`[get]` |
| | |
| IEnumerable | [Positions](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a46972cbe8a28b386c22fef5fcc0fbed3)`[get]` |
| | |
| TimeOnly? | [RefreshSecuritiesTime](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a3127b969389b75d4541d2aab7bddfd0d)`[get, set]` |
| | |
| bool | [HasPendingActions](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a002743dcb2e567e8a71897aa9e02754c)`[get]` |
| | |
| bool | [ServerMode](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a60d7936cd755fddd4fc78c83f23212db)`[get, set]` |
| | |
| bool | [ReconnectOnFirstConnect](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac33c399e843d0cdbf2d8a7e97d070d53)`[get, set]` |
| | |
| - Events inherited from [ATAS.DataFeedsCore.IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [ConnectionStateChanged](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae9107f2ec4c193fa507760c329b44e57) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Connected](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a9524e9b808b2675e4e408f3250d27bf5) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Disconnected](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a888c29e135620d4bec8dfa658b7a34b7) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewMyTrades](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a5030740d60042a4ad424259e0e361243) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewOrders](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a58bf22e2a07322a1e9e1a59c238407fb) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewPositions](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a7b2c9c1e4fc477670e4c29ecb2819dcd) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewPortfolios](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac90f7ae6aa4a85552584a841033430df) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewSecurities](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a5abf82f8241ac3756d09274b0fec297b) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [RemoveSecurities](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a7fbbed5a1770aa586a123e0e381f34f4) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrderChanged](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#aa9f15c8b6154d82095cfc3b532e20eda) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrdersRegisterFailed](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac719158631d0fe76cf53ab635199e444) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrdersCancelFailed](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a95263a71ad578f9b882342138fe08fba) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [OrderModifyFailed](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a284927aa59f4d217b7204f88475bec8f) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [PortfoliosChanged](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a1f2583219295329d26710eda515c1c78) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [PositionsChanged](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a42ddd365a0398d95ef7395f4bcf9ff67) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [BestBidAskUpdates](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#af6d2703eb1b7f40aed3a5a6c176e206e) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [MarketDepthsUpdate](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae25b1ece5a9fe0a76cfe188fbea9f6c6) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) | [MarketByOrderChanged](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ab39f4eca0f9729158f93894c80d38060) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [NewTrades](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a4c12a0ddf3f4100461ed164fe20f7fd0) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [SecurityChanged](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ae0d7de60d50692ee7898c53a10f386aa) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [SecuritySummaryChanged](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#acc7159452076320cc90d34bfa13e88b7) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [Error](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac57b629541c6e7b03eb2477ca428d467) |
| | Raised when an error occurs. |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? | [SearchSecuritiesResult](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac34381609e7bf536eed720dda9f87d46) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [RegisterSecurityResult](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a87a4126f3f11f7f4bc782004a537dbe9) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [NewNews](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a0327a30e40ec7a649a8d218c548638a8) |
| | |
| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? | [RebateReceived](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#a411fb41c5fa82c7ca49ed8c0bfc3d328) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)IsAccountBindedWithAtas()

| Task ATAS.DataFeedsCore.IReferralAccountConnector.IsAccountBindedWithAtas | ( | | ) | |
| --- | --- | --- | --- | --- |

The documentation for this interface was generated from the following file:
- [IDataFeedConnector.cs](../files/IDataFeedConnector_8cs.md)
