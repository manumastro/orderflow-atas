# ATAS.DataFeedsCore.IDataFeedConnector Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.html

Inheritance diagram for ATAS.DataFeedsCore.IDataFeedConnector:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.IDataFeedConnector:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
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

| Public Attributes | |
| --- | --- |
| decimal? | [maxAddable](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md#ac9cd86c5d4d19df2c4c43793280a9eab) |
| | Calculates possible changes of the isolated margin of a position (only for leveraged trading) |
| | |

| Properties | |
| --- | --- |
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

| Events | |
| --- | --- |
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

## [◆](https://docs.atas.net/en/)CalcIsolatedMarginChangeRange()

| decimal?? decimal maxRemovable ATAS.DataFeedsCore.IDataFeedConnector.CalcIsolatedMarginChangeRange | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9b3913dddd50d40c7d24816d920a956b), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae69dd2ac74e719595e16568a90c30d07).

## [◆](https://docs.atas.net/en/)CalcLiquidationPrice()

| decimal? ATAS.DataFeedsCore.IDataFeedConnector.CalcLiquidationPrice | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | decimal | margin |
| | ) | | |

Allows to estimate Liquidation Price change for a margin.

Parameters

| position | Position to calculate for |
| --- | --- |
| margin | Absolute margin for calculation |

Returnsnull if calculation is not supported

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aada611a01fd0adfd6e16222b4a85248a), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aaad98d1d406974aadb449a8b178b7f48).

## [◆](https://docs.atas.net/en/)CalcMaxOrderVolume()

| decimal? ATAS.DataFeedsCore.IDataFeedConnector.CalcMaxOrderVolume | ( | [OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) | orderType, |
| --- | --- | --- | --- |
| | | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
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

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af5eb163c054fbefbb0c10f198c2c338f), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0aa514d14ba7d75bac737980c7ad8d3b).

## [◆](https://docs.atas.net/en/)CalcOrderCost()

| decimal? ATAS.DataFeedsCore.IDataFeedConnector.CalcOrderCost | ( | [OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) | orderType, |
| --- | --- | --- | --- |
| | | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
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

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a53900592509e66e0111531836ddae19c), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae00948e82ecc14777888c7226ca37edf).

## [◆](https://docs.atas.net/en/)CancelOrder()

| void ATAS.DataFeedsCore.IDataFeedConnector.CancelOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aab5ad316484fe91c65baa2fd88384f66), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2560330c5046b5e59e2bab0c65c26679).

## [◆](https://docs.atas.net/en/)CancelOrderAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.CancelOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#af92e80c366466d2ba12b6218d81c7248), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a4985667d47988d411792aa03d5f948df).

## [◆](https://docs.atas.net/en/)ChangeIsolatedMarginAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.ChangeIsolatedMarginAsync | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
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

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a2fc872d30337381563392ea37a99cfaf), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a78559883f1c9fa4451877c0e4ef426b8).

## [◆](https://docs.atas.net/en/)ChangeMarginParametersAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.ChangeMarginParametersAsync | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
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

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7f2547643bd859da8befb6ab6fb0dd93), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a032b2c214b4e682de97a77c3c24687f5).

## [◆](https://docs.atas.net/en/)ClosePositionAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.ClosePositionAsync | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac37951dbe114336a1ab6f39486e31325), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a55c069fbd36af48f6e9ee32af49447bc).

## [◆](https://docs.atas.net/en/)ClosePositionsAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.ClosePositionsAsync | ( | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a09b9311b89a7c9b06858b933475a003c), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aebaae79b39a122ed6b3054dd60dc1b1d).

## [◆](https://docs.atas.net/en/)Connect()

| void ATAS.DataFeedsCore.IDataFeedConnector.Connect | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad77290066d5ec673645100d3ad1275d0), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a9151f286c873976a287bf944c4006161).

## [◆](https://docs.atas.net/en/)ConnectAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.ConnectAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#afd95903847b4a0d21b2698ab835d29bd), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ac96c27cf201024d64ddc03d2c9e0623a).

## [◆](https://docs.atas.net/en/)ConvertCurrency()

| decimal ATAS.DataFeedsCore.IDataFeedConnector.ConvertCurrency | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
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

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae3b0d4288818b4ffe72ab216c45e8f69), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a29ed998f144b82d0db8fb5639045e68f).

## [◆](https://docs.atas.net/en/)Disconnect()

| void ATAS.DataFeedsCore.IDataFeedConnector.Disconnect | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a78a4005e7f904c3eb6113911c34a4e63), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a095c36f9b55dc5c2d071bbaa2e06d474).

## [◆](https://docs.atas.net/en/)DisconnectAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.DisconnectAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a70da19a7db18ba74d261480000725077), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aa1867fc38bea83e426f5aa178bff68da).

## [◆](https://docs.atas.net/en/)GetMyTradesAsync()

| Task > ATAS.DataFeedsCore.IDataFeedConnector.GetMyTradesAsync | ( | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
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

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa21139260925f72e5bfc6dca97dc0cdd), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae37ed8d1d0951edbc92dd78e2d4c5a66).

## [◆](https://docs.atas.net/en/)GetPortfolios()

| IEnumerable ATAS.DataFeedsCore.IDataFeedConnector.GetPortfolios | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a8d84351385f699098c586e9ff5281b04), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a3b3cd7a501b635e8e5da91c5fbdce2b9).

## [◆](https://docs.atas.net/en/)GetPosition()

| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.DataFeedsCore.IDataFeedConnector.GetPosition | ( | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | tPlusLimit |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a774ab2a321b6eb6188d9c6a2c639df71), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#acdad2f096b72c762232bb68521f9fab8).

## [◆](https://docs.atas.net/en/)GetRoutes()

| IEnumerable ATAS.DataFeedsCore.IDataFeedConnector.GetRoutes | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7aff547cff6d89b9467379f9b063c7ad), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2fa88ad7715841c9fd9d21ebf14c6cfb).

## [◆](https://docs.atas.net/en/)GetSecurityTradingOptions()

| [ISecurityTradingOptions](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md)? ATAS.DataFeedsCore.IDataFeedConnector.GetSecurityTradingOptions | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security | ) | |
| --- | --- | --- | --- | --- | --- |

Gets possible TimeInForce for order and optional flags that may be passed when order is created

 If null default behaviour is expected:

 TimeInForce = DAY, GTC, FOK

 No order flags

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad4a06233c46c588e8bafc3740efea713), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a7949e66a46250b861bff8bb04b1008c2).

## [◆](https://docs.atas.net/en/)ModifyOrder()

| void ATAS.DataFeedsCore.IDataFeedConnector.ModifyOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | neworder |
| | ) | | |

Modifies the price of an order.

Parameters

| order | The order to modify. |
| --- | --- |
| neworder | The new order with the updated parameters. |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3e36fd4856f49fe46aaae95051cda47d), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#adad25fddf54ecc056a7e84879d7b9341).

## [◆](https://docs.atas.net/en/)ModifyOrderAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.ModifyOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab9afd95c2bc06b215ccf2a086e2eb646), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a71b051434141b53951c825415e8ee552).

## [◆](https://docs.atas.net/en/)RegisterOrder()

| void ATAS.DataFeedsCore.IDataFeedConnector.RegisterOrder | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab509d691f91f1f76e19f8c77220373a4), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a322eb465522c9e08ab7b5f5c3cf7cf26).

## [◆](https://docs.atas.net/en/)RegisterOrderAsync()

| Task ATAS.DataFeedsCore.IDataFeedConnector.RegisterOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad5a4ef349b569f44c0c6896b38087223), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a6af8c995d9d63c1c4affa5fe18c06232).

## [◆](https://docs.atas.net/en/)SearchSecurities()

| void ATAS.DataFeedsCore.IDataFeedConnector.SearchSecurities | ( | [SecurityFilter](../classes/classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) | filter | ) | |
| --- | --- | --- | --- | --- | --- |

Searches for securities matching the specified filter.

Parameters

| filter | |
| --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a19ca4507436c720d4628d4afe60e509f), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a49ded93a606ae3e4daec1c33f0a8b9ce).

## [◆](https://docs.atas.net/en/)SearchSecuritiesAsync()

| Task > ATAS.DataFeedsCore.IDataFeedConnector.SearchSecuritiesAsync | ( | [SecurityFilter](../classes/classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) | filter | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ae835683cfc2e9023ef25d398ebe92865), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8bc38b96200bd305d630a4557b2c3900).

## [◆](https://docs.atas.net/en/)SubscribeToMarketData() [1/2]

| void ATAS.DataFeedsCore.IDataFeedConnector.SubscribeToMarketData | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abc8be4646760d1d79a3bb0b0400aba51), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a561a891758e90c886eb779172e00a1d6).

## [◆](https://docs.atas.net/en/)SubscribeToMarketData() [2/2]

| void ATAS.DataFeedsCore.IDataFeedConnector.SubscribeToMarketData | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Subscribes to market data for a security.

Parameters

| security | The security to subscribe to. |
| --- | --- |
| subscriptionTypes | The subscription type flags. |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a02ade369f899942dc1373bb3c84e0a18), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a83175077ec4d48c8c88993efc88d2b12).

## [◆](https://docs.atas.net/en/)TryGetOrder()

| [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) ATAS.DataFeedsCore.IDataFeedConnector.TryGetOrder | ( | long | extId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0ea1e0df22fb22d0fcf9857b38c77250), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8a6ac7d65ed7a5208066d41b6379c706).

## [◆](https://docs.atas.net/en/)UnsubscribeFromMarketData() [1/2]

| void ATAS.DataFeedsCore.IDataFeedConnector.UnsubscribeFromMarketData | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a8e3eb8741cc2675c9dad5b881b692fa6), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aeb7c59bf7fd2eea4e21618d645ca94c0).

## [◆](https://docs.atas.net/en/)UnsubscribeFromMarketData() [2/2]

| void ATAS.DataFeedsCore.IDataFeedConnector.UnsubscribeFromMarketData | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subscriptionTypes |
| | ) | | |

Unsubscribes from market data for a security.

Parameters

| security | The security to unsubscribe from. |
| --- | --- |
| subscriptionTypes | The subscription type flags. |

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a84568b2ca542e2a0f81c4ee198b8a64c), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a5a04c862efde50425fd02d8bead3d0bd).

## Member Data Documentation

## [◆](https://docs.atas.net/en/)maxAddable

| decimal? ATAS.DataFeedsCore.IDataFeedConnector.maxAddable |
| --- |

Calculates possible changes of the isolated margin of a position (only for leveraged trading)

Parameters

| position | |
| --- | --- |

Returns

## Property Documentation

## [◆](https://docs.atas.net/en/)AllowUpdatePositionsPnL

| bool ATAS.DataFeedsCore.IDataFeedConnector.AllowUpdatePositionsPnL |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ab3ab9092292113ebe2134762b4ff6e7d), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#afcf33c13f9150989728a28cc2ae6d8ba).

## [◆](https://docs.atas.net/en/)ConnectionState

| ConnectionStates ATAS.DataFeedsCore.IDataFeedConnector.ConnectionState |
| --- |

get

Current connection state of the connector.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a99c5aa250214c54c01230312312e456a), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8386f9020c67881317dd012514bd5e22).

## [◆](https://docs.atas.net/en/)DataPath

| string ATAS.DataFeedsCore.IDataFeedConnector.DataPath |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a0af66495fda4cab45a3fe1bf2b01ddc7), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a3330f7ac07ea80992ad40b7a37aea02e).

## [◆](https://docs.atas.net/en/)DefaultTimeSyncManager

| ITimeSyncManager? ATAS.DataFeedsCore.IDataFeedConnector.DefaultTimeSyncManager |
| --- |

getset

Default ITimeSyncManager to get time difference with NTP server.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a18b496cd7489ee64ac3eefa9f498ae26), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8bb0e21babccfd07b3ed9ddc20c9dcf7).

## [◆](https://docs.atas.net/en/)ExchangeInfoProvider

| [IConnectorExchangeInfoProvider](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) ATAS.DataFeedsCore.IDataFeedConnector.ExchangeInfoProvider |
| --- |

getset

Gets or sets the provider used to retrieve exchange information for the securities.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7245c1c96b5caca40ae8fe503b2bc3a2), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0cf4bb84e354cc3fae64a1e3bc9143f8).

## [◆](https://docs.atas.net/en/)Factory

| [IEntityFactory](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) ATAS.DataFeedsCore.IDataFeedConnector.Factory |
| --- |

getset

Factory used to create Security, Portfolio, and other entity objects.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a651e143caed6f4ef81560a253e83c555), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ab18a082ac345400d6106f028d03fa9ba).

## [◆](https://docs.atas.net/en/)HasPendingActions

| bool ATAS.DataFeedsCore.IDataFeedConnector.HasPendingActions |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a5d3eced3b9406f0c1f338216f02a7777), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a56abf2ee19d70f72da8fb161b13ad1fb).

## [◆](https://docs.atas.net/en/)Id

| Guid ATAS.DataFeedsCore.IDataFeedConnector.Id |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a46f1758db8613f2d5919480eafb463c4), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a48f5364a64761463fa0f5bfb351a52f7).

## [◆](https://docs.atas.net/en/)IsConnected

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsConnected |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a569810569a913dad7b693aa899d94fbf), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a11cd14332e4da78d3b6fb9dd285d5d82).

## [◆](https://docs.atas.net/en/)IsFullLicense

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsFullLicense |
| --- |

getset

License type.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac03202fba8bc165b2833308a5ca99396), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2cb6294cf432a53b2bfa1447156b291c).

## [◆](https://docs.atas.net/en/)IsSupportedAmericanFutures

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedAmericanFutures |
| --- |

get

Indicates whether the connector supports American futures.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aeaaf3c8be24431e3528e07c7dcf8b7c2), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#abd4ea16fa1e1658b417ebea328d37b82).

## [◆](https://docs.atas.net/en/)IsSupportedAmericanStocks

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedAmericanStocks |
| --- |

get

Indicates whether the connector supports American stocks.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a45d82fdbc4e09ba8a5f6ca774cd5175d), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a916060a1bad209a74b95c40d6b345513).

## [◆](https://docs.atas.net/en/)IsSupportedCfd

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedCfd |
| --- |

get

Indicates whether the connector supports CFD instruments.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a7fa9ec4593d291d4fbdc2714d04a9480), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a9fb854171b509141f02c27400a2b112f).

## [◆](https://docs.atas.net/en/)IsSupportedCrypto

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedCrypto |
| --- |

get

Indicates whether the connector supports crypto instruments.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a400c52cc5b95aaf558450ba8a4609b03), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0ef0055b413d83682aa9e510406e1778).

## [◆](https://docs.atas.net/en/)IsSupportedMaxOrderCalculation

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedMaxOrderCalculation |
| --- |

get

Does connector support max order calculation for the instrument

 This feature enables percentage slider under the volume input box

 Affects CalcMaxOrderVolume and CalcOrderCost methods.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#abacab57b817694a0d73d682472cbeb9b), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a6e9657f68a363fe99840fd78ff327af2).

## [◆](https://docs.atas.net/en/)IsSupportedRussianMarket

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedRussianMarket |
| --- |

get

Indicates whether the connector supports Russian market instruments.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#acb8fe12cc4109226b6c4cca9c2122c06), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae7737fe1c8f75fc7d47eaf36fac8377a).

## [◆](https://docs.atas.net/en/)IsSupportedServerOCO

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedServerOCO |
| --- |

get

Indicates whether the connector supports server-side OCO orders.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a23a755246b4e66aad827d36d4ec0037a), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ae8522b2df514e327791fad54582a63ae).

## [◆](https://docs.atas.net/en/)IsSupportedServerTime

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedServerTime |
| --- |

get

Does connector supports getting exchange server time.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a82dbc5923b6465489197f505c49e31be), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a4fa5380e3dc5ddeaaeda7b6c805e1295).

## [◆](https://docs.atas.net/en/)IsSupportedStopOrders

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedStopOrders |
| --- |

get

Indicates whether the connector supports stop orders.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aefc730b5c0d39e98cbf28d3c5c46726f), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a8ddeb056ff7bfc9425220bd154ed84a8).

## [◆](https://docs.atas.net/en/)IsSupportedTradingFunctions

| bool ATAS.DataFeedsCore.IDataFeedConnector.IsSupportedTradingFunctions |
| --- |

get

Indicates whether the connector supports trading functions.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a1946ad75bad8f78ea64c8f46ebe9ca1e), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a851329223fa377f4da08103b52950d21).

## [◆](https://docs.atas.net/en/)LatencyManager

| [IConnectorLatencyManager](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) ATAS.DataFeedsCore.IDataFeedConnector.LatencyManager |
| --- |

get

Latency manager.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ac8c211c97cf47e4083fe02a3c3ad246f), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#af1c1f723307e9ef55dfad9440c016c06).

## [◆](https://docs.atas.net/en/)MarketDataDelayPeriod

| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) ATAS.DataFeedsCore.IDataFeedConnector.MarketDataDelayPeriod |
| --- |

getset

Gets or sets delay period for market data.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a69ace05c12029e51e433cfc93e45ce93), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a0b69a78bb26e3a6678fcc82f6b430dc5).

## [◆](https://docs.atas.net/en/)MarketDataStreamEnabled

| bool ATAS.DataFeedsCore.IDataFeedConnector.MarketDataStreamEnabled |
| --- |

getset

Indicates whether to raise market data through BestBidAskUpdates, MarketDepthsUpdate, and NewTrades events.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a740f9048911def8c6a1ee674a209fdd2), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#aeedd3848197a9b8cc13933743730d5d5).

## [◆](https://docs.atas.net/en/)MyTrades

| IEnumerable ATAS.DataFeedsCore.IDataFeedConnector.MyTrades |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a5ab4219ad9cbb8b5bef3eb7f63b4fc6b), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2fb6386d97d6c5e3819f90571dbe0de7).

## [◆](https://docs.atas.net/en/)NeedRebatesCheck

| bool ATAS.DataFeedsCore.IDataFeedConnector.NeedRebatesCheck |
| --- |

getset

Has rebate check feature.

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a953c9a65dbd3d973e6e94b275c4dd9bb), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a3c4757bd2cadff6efb5fd4caafed55cf).

## [◆](https://docs.atas.net/en/)Orders

| IEnumerable ATAS.DataFeedsCore.IDataFeedConnector.Orders |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aac816b04a326ebb0d3088692d3906da1), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#af45afe2eba3df48e0c484f8b8e7c5de6).

## [◆](https://docs.atas.net/en/)Portfolios

| IEnumerable ATAS.DataFeedsCore.IDataFeedConnector.Portfolios |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa7ab869759933e0a876d1941f82fd241), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a9feeaf702c215c8e3119a0127932369c).

## [◆](https://docs.atas.net/en/)Positions

| IEnumerable ATAS.DataFeedsCore.IDataFeedConnector.Positions |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a2fe5d0f3982e9e67963b1d7e8b83adaa), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a54a2bae8d41a047345df30d96216c32f).

## [◆](https://docs.atas.net/en/)ReconnectOnFirstConnect

| bool ATAS.DataFeedsCore.IDataFeedConnector.ReconnectOnFirstConnect |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a3a3c020ff40dcf323339ee750c62bbe5), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a7104db4f502c1677438db553aea4c2a2).

## [◆](https://docs.atas.net/en/)RefreshSecuritiesTime

| TimeOnly? ATAS.DataFeedsCore.IDataFeedConnector.RefreshSecuritiesTime |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#aa3396be51e40753679e094b81b2ab42b), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#a2fa98b42d060c46412fa8560d9959907).

## [◆](https://docs.atas.net/en/)Securities

| IEnumerable ATAS.DataFeedsCore.IDataFeedConnector.Securities |
| --- |

get

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#ad337f78d56e41b33df81a538661e657e), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#ab32e1cbc83c9aa28364899b7eecb9e83).

## [◆](https://docs.atas.net/en/)ServerMode

| bool ATAS.DataFeedsCore.IDataFeedConnector.ServerMode |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md#a9f76a23c729a45812d359fc40f1e7ebb), and [ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md#acc279dfc89f7a9bb173f2c599d94844b).

## Event Documentation

## [◆](https://docs.atas.net/en/)BestBidAskUpdates

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.BestBidAskUpdates |
| --- |

## [◆](https://docs.atas.net/en/)Connected

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.Connected |
| --- |

## [◆](https://docs.atas.net/en/)ConnectionStateChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.ConnectionStateChanged |
| --- |

## [◆](https://docs.atas.net/en/)Disconnected

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.Disconnected |
| --- |

## [◆](https://docs.atas.net/en/)Error

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.Error |
| --- |

Raised when an error occurs.

## [◆](https://docs.atas.net/en/)MarketByOrderChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) ATAS.DataFeedsCore.IDataFeedConnector.MarketByOrderChanged |
| --- |

## [◆](https://docs.atas.net/en/)MarketDepthsUpdate

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.MarketDepthsUpdate |
| --- |

## [◆](https://docs.atas.net/en/)NewMyTrades

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.NewMyTrades |
| --- |

## [◆](https://docs.atas.net/en/)NewNews

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.NewNews |
| --- |

## [◆](https://docs.atas.net/en/)NewOrders

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.NewOrders |
| --- |

## [◆](https://docs.atas.net/en/)NewPortfolios

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.NewPortfolios |
| --- |

## [◆](https://docs.atas.net/en/)NewPositions

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.NewPositions |
| --- |

## [◆](https://docs.atas.net/en/)NewSecurities

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.NewSecurities |
| --- |

## [◆](https://docs.atas.net/en/)NewTrades

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.NewTrades |
| --- |

## [◆](https://docs.atas.net/en/)OrderChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.OrderChanged |
| --- |

## [◆](https://docs.atas.net/en/)OrderModifyFailed

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.OrderModifyFailed |
| --- |

## [◆](https://docs.atas.net/en/)OrdersCancelFailed

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.OrdersCancelFailed |
| --- |

## [◆](https://docs.atas.net/en/)OrdersRegisterFailed

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.OrdersRegisterFailed |
| --- |

## [◆](https://docs.atas.net/en/)PortfoliosChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.PortfoliosChanged |
| --- |

## [◆](https://docs.atas.net/en/)PositionsChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.PositionsChanged |
| --- |

## [◆](https://docs.atas.net/en/)RebateReceived

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.RebateReceived |
| --- |

## [◆](https://docs.atas.net/en/)RegisterSecurityResult

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.RegisterSecurityResult |
| --- |

## [◆](https://docs.atas.net/en/)RemoveSecurities

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.RemoveSecurities |
| --- |

## [◆](https://docs.atas.net/en/)SearchSecuritiesResult

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) >? ATAS.DataFeedsCore.IDataFeedConnector.SearchSecuritiesResult |
| --- |

## [◆](https://docs.atas.net/en/)SecurityChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.SecurityChanged |
| --- |

## [◆](https://docs.atas.net/en/)SecuritySummaryChanged

| [ConnectorEventHandler](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73)? ATAS.DataFeedsCore.IDataFeedConnector.SecuritySummaryChanged |
| --- |

The documentation for this interface was generated from the following file:
- [IDataFeedConnector.cs](../files/IDataFeedConnector_8cs.md)
