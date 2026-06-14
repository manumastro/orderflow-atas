# ATAS.DataFeedsCore.Database.ICache Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.html

Inheritance diagram for ATAS.DataFeedsCore.Database.ICache:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Database.ICache:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Init](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3621740eaf8e559fb295e2762aee146e) (bool isServer) |
| | |
| ICollection | [GetPortfolios](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0ca1a05ed5ee6ccb597b3a19fabf8baf) () |
| | |
| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [GetPortfolio](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9b2590551f0c9a7556df373ca43a5a6a) (string accountId) |
| | |
| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [TryGetPortfolio](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#afe608c39709b0c90261b5cd1b38fee18) (string accountId) |
| | |
| ICollection | [GetPositions](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0881445933d4da6d863f40f667be354b) (string accountId) |
| | |
| ICollection | [GetPositions](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9558f439818d4dfc7eb2a1c2aff31099) () |
| | |
| ICollection | [GetOrders](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa80ae63b7858a126c621374f411d74a9) (string accountId) |
| | |
| ICollection | [GetMyTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adae0091095f393cd7a82116bf2b9e1d1) (string accountId) |
| | |
| IEnumerable | [GetPositionMyTrdades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af65c5d1d8d58a2110df88da51141f952) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| IEnumerable | [GetOpenedMyTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a232082ea2e0a8c3be54a695bdd3cb30e) () |
| | |
| IEnumerable | [GetOrders](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ab38c7dfa9121a45ad4d74794734671f5) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| IEnumerable | [GetMyTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a80c2f8baf6fc9b6f6da6af1fac759905) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| IEnumerable | [GetMyTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a884413949c8bc7dc07d6ea66eabeb066) (string accountId, long tradeId) |
| | |
| IEnumerable | [GetHistoryTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a10d1498d5f57fa5e9becde6eccf36611) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | [TryGetOrder](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a5cb5a09dc7fea42ce586c63404426f80) (string accountId, long extId, bool searchInDb) |
| | |
| [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [TryGetMyTrade](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a4539efbe205adb2b9fac552f08b29d14) (string accountId, string tradeId, bool searchInDb) |
| | |
| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | [TryGetPosition](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aea2059d2d3639aa16cf0e529b7fe2760) (string accountId, string securityId) |
| | |
| IEnumerable | [GetPortfolioChanges](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a102de0e72d4192543a03544e31ec80bd) (string accountId, DateTime from, DateTime to) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a17115ee3cbfc16cf58bffe56f79e81ca) ([PortfolioChange](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) portfolioChange, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ada2f872842039bbf8f0f516a6d07645c) ([PortfolioState](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioState.md) portfolioState, bool wait=false) |
| | |
| ICollection | [LoadAllSecurities](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8d3cac41839c05ae3f2143cfbb91a997) () |
| | |
| ICollection | [GetSecurities](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#afd447f2fd08bfc457df68113a3a0dcf1) () |
| | |
| ICollection | [GetPositionSecurities](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7c06c5c65deba2214aaf05893000b39d) () |
| | |
| IEnumerable | [GetSecurities](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a478eb0e89de69c064f5e83094b92d1aa) (string code, string exchange) |
| | |
| IEnumerable | [GetSecuritiesByExchange](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aaca06a95853a2ccdca991544366ede38) (string exchange) |
| | |
| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | [GetSecurity](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adee048029cfb9e4aa7275f843f6e2c10) (string id) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2aecf6ccdc5c0577af2a002b9dda341e) (IEnumerable securities, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1fbbd0769f8f71b18389929df2bab361) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, bool wait=false) |
| | |
| ICollection | [GetSecurityMargins](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a12c62ac3cec0b465224bb955c454a524) () |
| | |
| [SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | [TryGetSecurityMargin](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa3ce65bfa3cbba90a1aa41eefc5d9411) (string securityId) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a11ab43f11f35066b32770f482386f88e) ([SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) margin, bool wait=false) |
| | |
| ICollection | [GetExchanges](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a58177a7ff3b3d42f42c4afd1b8c63c17) () |
| | |
| ICollection | [GetInstrumentExchanges](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a92dfb269dfa42d4af30d56c3b07b453b) () |
| | Get all InstrumentExchange need for import. |
| | |
| [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | [GetExchange](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adff4a124bc5d45012ae9b41534b402e2) (string code) |
| | |
| [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | [TryGetExchange](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a11b0cb8584962a625ddc7c36f70582f6) (string code) |
| | |
| [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | [TryGetInstrumentExchange](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a27889b1ee6b2aea68bfcd58450fccced) (string instrument, string code) |
| | |
| [InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | [TryGetInstrumentExchange](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0545167f603084f37498162e988ae651) (string instrument) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7746f3cad296615a2e31472d07e81374) ([Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) exchange, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8596b36dc5d1e2c1dbddef8b7b438ce0) ([InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) instrumentExchange, bool wait=false) |
| | |
| void | [Remove](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a763bc4413749b1605ceb039318c8293e) ([Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) exchange) |
| | |
| void | [Remove](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a546f29b385eff52e4bb6ac2a071d58ae) ([InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) instrumentExchange) |
| | |
| ICollection | [GetUserRoles](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3cd812bb2e453f45752f3b3cf8c5599f) () |
| | |
| [UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) | [GetUserRole](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7f2ee2201725c01439b4dc302a92bf4e) (long id) |
| | |
| [UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) | [TryGetUserRole](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9b12380274cc3ca259dfd8caf14ea574) (long id) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ad958bce8fa0df1a3b31149e91ff4a20d) ([UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) role, bool wait=false) |
| | |
| ICollection | [GetUserGroups](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aef64a101526e60dbd3b8f682e87f0225) () |
| | |
| ICollection | [GetUserGroups](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a58e43f4696b7ea9eb7fe4f97ce9897ce) ([UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) parent) |
| | |
| [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [GetUserGroup](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af96a0f6e73908a1e9542e8b0bd0ccce9) (long id) |
| | |
| [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [TryGetUserGroup](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a022576663874c4714e7e9f2a851be1fd) (long id) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af6663cd48d19a0cc154cf9901a5dc590) ([UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) group, bool wait=false) |
| | |
| ICollection | [GetUsers](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2591be50ae02c6d53d769932c70c4c88) ([UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) group) |
| | |
| [User](../classes/classATAS_1_1DataFeedsCore_1_1User.md)? | [GetUser](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a6bbc7d1610ca8747520105549ccf32c3) (string login) |
| | |
| [User](../classes/classATAS_1_1DataFeedsCore_1_1User.md)? | [GetUser](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a74052db11af8db2c9c9bb7b5491a1163) (long id) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adb2bf3aa9de9dbbd66b9655b6cd68c64) ([User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) user, bool wait=false) |
| | |
| ICollection | [GetCommissionGroups](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a77fe11f4499961a4bae70de1d74ae58f) ([UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) group) |
| | |
| [CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | [TryGetCommissionGroup](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9dbf4b2d044c5ac58dfa8c4b8fb8fa40) (long id) |
| | |
| [CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | [TryGetCommissionGroup](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a418b0430470f625e9a93868524d72806) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adb93637297f5d6f83818e4ce1eab3c98) ([CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) commissionGroup, bool wait=false) |
| | |
| string | [GetValue](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2498d59903447eeb55914363e207606e) (string name) |
| | |
| void | [SetValue](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a5bda3d3e1e17adac6dd47806bbea39a5) (string name, string value, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1d245ff73de1a198bb28319f2811c0d7) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a566eebda9fdd93126bce6b8f226a7d63) ([MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1ed86e022309327e0b0bb29dcb749138) ([HistoryMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ad586fdd449696e63cbaf839c68ad6604) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, bool extended, bool wait=false) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adc8add95a6dedb320ecd3bbf1e0342ed) ([Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) position, bool wait=false) |
| | |
| void | [Remove](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a44127a312d44770bc35fc1ea20f66d2c) ([MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [ClearHistoryTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a22c3657bf6eeec237d2ae6f5733e1d1f) () |
| | |
| void | [ClearMyTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae985b1fa16c95bec66cc5f0947b6a006) () |
| | |
| void | [ClearOrders](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae40d25831a4cd3cb9d05e7d6c662f744) () |
| | |
| IEnumerable | [GetNews](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a95631598c7817f51d6d7718d0bb500b5) (DateTime from, DateTime to) |
| | |
| IEnumerable | [GetNews](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3daa4b9366f76cb607823ce08e73ab34) ([User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) user, bool unhandled) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa6a900318aa68abdd39e1ac6fb66c9e3) ([News](../classes/classATAS_1_1DataFeedsCore_1_1News.md) news, bool wait=false) |
| | |
| ICollection | [GetPortfolioViewers](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a17522de09b92b1bd4568f6de229c8c33) () |
| | |
| [PortfolioViewer](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) | [TryGetPortfolioViewer](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a13f8da2dd7862b777248371061f802fe) (long id) |
| | |
| void | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a530c78fca5dc71f37e0f0f59e7eff73e) ([PortfolioViewer](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) viewer, bool wait=false) |
| | |
| IEnumerable | [GetServerPnL](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a6b1227ddb20257c68e10d484f35fb6c8) (DateTime from, DateTime to) |
| | |
| void | [Wait](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#abe32d05f484e610ed2ef7b408bc1abe3) () |
| | |
| void | [WaitInitialized](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae2c6264c2db3308e6d56f1112f573ec4) () |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.IEntityFactory](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) | |
| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | [GetOrCreateSecurity](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#ac4a0adcc45a4c830907c368e8921089c) (string id, Func create) |
| | |
| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [GetOrCreatePortfolio](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#af5c98f24eaa380af205502b08c69c4ba) (string accountId, Func create) |
| | |
| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | [GetOrCreatePosition](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a5d990741b512ff237c7fc91034fa545e) ([Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, Func create) |
| | |
| [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | [GetOrCreateOrder](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a6e2ffa6dbeaf742978239c850ae30c7c) (long extId, Func create) |
| | |
| [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [GetOrCreateMyTrade](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a443bf765ae3d3905a463711c0c5c418a) (string id, Func create) |
| | |
| [Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md) | [CreateTrade](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a1fd4bbe23a8760e52fb808612f633ddb) () |
| | |
| [MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [CreateMarketDepth](./interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#aaa3a4477e0590fd6c7a08fca3e0891d7) () |
| | |

| Properties | |
| --- | --- |
| long | [LastExtId](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a92f7a13e47921d6b6a831d3b86f39c3e)`[get]` |
| | |
| long | [LastOrderId](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ab7a30e1dcf63b1050e9ae1a9565b38eb)`[get]` |
| | |
| long | [LastTradeId](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8f6ef9c563259d8787a4e5cacc8cdc4b)`[get]` |
| | |
| bool | [IsInitialized](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a87c272c47c1502ca33ae94e1907fafc1)`[get]` |
| | |
| TimeSpan | [ClearCachePeriod](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a37c4af331f1a63f1538da1d61f789e9a)`[get, set]` |
| | |
| bool | [CheckConsistency](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ac23098e9e83282020078d0c45cda2356)`[get, set]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ClearHistoryTrades()

| void ATAS.DataFeedsCore.Database.ICache.ClearHistoryTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a22daaa76c7fdf3cb9f8052b9473c033a).

## [◆](https://docs.atas.net/en/)ClearMyTrades()

| void ATAS.DataFeedsCore.Database.ICache.ClearMyTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aad7a9237a3efc21f291b34190b5bd63d).

## [◆](https://docs.atas.net/en/)ClearOrders()

| void ATAS.DataFeedsCore.Database.ICache.ClearOrders | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ac942e3fe57251e6d47bed1c500e29f05).

## [◆](https://docs.atas.net/en/)GetCommissionGroups()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetCommissionGroups | ( | [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | group | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8765eca84ed682d222d758273a39bd7b).

## [◆](https://docs.atas.net/en/)GetExchange()

| [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) ATAS.DataFeedsCore.Database.ICache.GetExchange | ( | string | code | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a598304055cca41b1add5f571c1f09ff8).

## [◆](https://docs.atas.net/en/)GetExchanges()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetExchanges | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0d26abf325cfdc2f6699234de819a752).

## [◆](https://docs.atas.net/en/)GetHistoryTrades()

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetHistoryTrades | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | IEnumerable | accounts, |
| | | IEnumerable | securities |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a3c70ac8e8f3eaf837951065db0054de4).

## [◆](https://docs.atas.net/en/)GetInstrumentExchanges()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetInstrumentExchanges | ( | | ) | |
| --- | --- | --- | --- | --- |

Get all InstrumentExchange need for import.

Returns

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa2fb884165601d47a2eaac246e3cb053).

## [◆](https://docs.atas.net/en/)GetMyTrades() [1/3]

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetMyTrades | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | IEnumerable | accounts, |
| | | IEnumerable | securities |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab28bbd90a0fc4f1066492c82d3834080).

## [◆](https://docs.atas.net/en/)GetMyTrades() [2/3]

| ICollection ATAS.DataFeedsCore.Database.ICache.GetMyTrades | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa715b70144cf7b7d5f548b9a4282880f).

## [◆](https://docs.atas.net/en/)GetMyTrades() [3/3]

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetMyTrades | ( | string | accountId, |
| --- | --- | --- | --- |
| | | long | tradeId |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#af50d79dc6118f9c8be786c7a5673902c).

## [◆](https://docs.atas.net/en/)GetNews() [1/2]

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetNews | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab1fb3dcb5123484e9657472fdac1c2aa).

## [◆](https://docs.atas.net/en/)GetNews() [2/2]

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetNews | ( | [User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) | user, |
| --- | --- | --- | --- |
| | | bool | unhandled |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a666d74eea1b748fcbf77afa9ca5e5a7e).

## [◆](https://docs.atas.net/en/)GetOpenedMyTrades()

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetOpenedMyTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a58363b757486b332cd69c596f2564350).

## [◆](https://docs.atas.net/en/)GetOrders() [1/2]

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetOrders | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | IEnumerable | accounts, |
| | | IEnumerable | securities |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4254791589944259dfb1008518f4458a).

## [◆](https://docs.atas.net/en/)GetOrders() [2/2]

| ICollection ATAS.DataFeedsCore.Database.ICache.GetOrders | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ae6a812963eeb16f3bbb3db719e361e35).

## [◆](https://docs.atas.net/en/)GetPortfolio()

| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.DataFeedsCore.Database.ICache.GetPortfolio | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a1878c54aee10f1f44193612232d9c91a).

## [◆](https://docs.atas.net/en/)GetPortfolioChanges()

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetPortfolioChanges | ( | string | accountId, |
| --- | --- | --- | --- |
| | | DateTime | from, |
| | | DateTime | to |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aaa683db86d6bc7110236715a223a24b9).

## [◆](https://docs.atas.net/en/)GetPortfolios()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetPortfolios | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a6ecb29ed6c6ecbbfb1a5c25ddbd1212e).

## [◆](https://docs.atas.net/en/)GetPortfolioViewers()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetPortfolioViewers | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a96202b7c5e2958a7be439b146e172ba8).

## [◆](https://docs.atas.net/en/)GetPositionMyTrdades()

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetPositionMyTrdades | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a2959ebebb00f91cf45f431ddb2e8cfd1).

## [◆](https://docs.atas.net/en/)GetPositions() [1/2]

| ICollection ATAS.DataFeedsCore.Database.ICache.GetPositions | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a013b68c60de555a58878a4a9268eb0be).

## [◆](https://docs.atas.net/en/)GetPositions() [2/2]

| ICollection ATAS.DataFeedsCore.Database.ICache.GetPositions | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa5a5151177cee712a71d3e0a4ce98f7d).

## [◆](https://docs.atas.net/en/)GetPositionSecurities()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetPositionSecurities | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a08cdbecf9d3cf855c9bb69c2f8ba52d8).

## [◆](https://docs.atas.net/en/)GetSecurities() [1/2]

| ICollection ATAS.DataFeedsCore.Database.ICache.GetSecurities | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a865f5bc3528d411f79257d72ee98bf4b).

## [◆](https://docs.atas.net/en/)GetSecurities() [2/2]

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetSecurities | ( | string | code, |
| --- | --- | --- | --- |
| | | string | exchange |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a337f5e298d2922da2ada30aea8cff567).

## [◆](https://docs.atas.net/en/)GetSecuritiesByExchange()

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetSecuritiesByExchange | ( | string | exchange | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8dadc5990d5093d5fe0cc99018ded4f9).

## [◆](https://docs.atas.net/en/)GetSecurity()

| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.Database.ICache.GetSecurity | ( | string | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#afb4e02f86e865e9586283e3df92ef0c3).

## [◆](https://docs.atas.net/en/)GetSecurityMargins()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetSecurityMargins | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a08c581b2cb3a2be9a41879717cd3a622).

## [◆](https://docs.atas.net/en/)GetServerPnL()

| IEnumerable ATAS.DataFeedsCore.Database.ICache.GetServerPnL | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aedc9610940edfc0e94c577c4f0e7e024).

## [◆](https://docs.atas.net/en/)GetUser() [1/2]

| [User](../classes/classATAS_1_1DataFeedsCore_1_1User.md)? ATAS.DataFeedsCore.Database.ICache.GetUser | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a464d958b111c017fc87280ae8cdad11d).

## [◆](https://docs.atas.net/en/)GetUser() [2/2]

| [User](../classes/classATAS_1_1DataFeedsCore_1_1User.md)? ATAS.DataFeedsCore.Database.ICache.GetUser | ( | string | login | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a252b0d4d00010ea310196e3e05de6f8e).

## [◆](https://docs.atas.net/en/)GetUserGroup()

| [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) ATAS.DataFeedsCore.Database.ICache.GetUserGroup | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a06b2381dd1f66e1c4a9d497e7b3addc4).

## [◆](https://docs.atas.net/en/)GetUserGroups() [1/2]

| ICollection ATAS.DataFeedsCore.Database.ICache.GetUserGroups | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a7c50be7711b13bcfb18a0e985b456d5a).

## [◆](https://docs.atas.net/en/)GetUserGroups() [2/2]

| ICollection ATAS.DataFeedsCore.Database.ICache.GetUserGroups | ( | [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | parent | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a9e4aec398d6c8e3bab29b4798f958a0a).

## [◆](https://docs.atas.net/en/)GetUserRole()

| [UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) ATAS.DataFeedsCore.Database.ICache.GetUserRole | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a5689a7ec79a27f05e1a30ae18477b0ee).

## [◆](https://docs.atas.net/en/)GetUserRoles()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetUserRoles | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a3e0a7e6314e67b011c5db5b27b3cfa37).

## [◆](https://docs.atas.net/en/)GetUsers()

| ICollection ATAS.DataFeedsCore.Database.ICache.GetUsers | ( | [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | group | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad1036b6ce5de5bcb2bc0d16c71d258b4).

## [◆](https://docs.atas.net/en/)GetValue()

| string ATAS.DataFeedsCore.Database.ICache.GetValue | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a5d15f5b42cd27dca9e4ae3c765a1521e).

## [◆](https://docs.atas.net/en/)Init()

| void ATAS.DataFeedsCore.Database.ICache.Init | ( | bool | isServer | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a46d15982dc51fa926eb24ed4d0c5d6d0).

## [◆](https://docs.atas.net/en/)LoadAllSecurities()

| ICollection ATAS.DataFeedsCore.Database.ICache.LoadAllSecurities | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0aa84c3773bd6d8026128dfc631eebc9).

## [◆](https://docs.atas.net/en/)Remove() [1/3]

| void ATAS.DataFeedsCore.Database.ICache.Remove | ( | [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | exchange | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#af21755f27e44503414f147150e3dd811).

## [◆](https://docs.atas.net/en/)Remove() [2/3]

| void ATAS.DataFeedsCore.Database.ICache.Remove | ( | [InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | instrumentExchange | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#af99486c385dfdbab50a8b466edbe3e1c).

## [◆](https://docs.atas.net/en/)Remove() [3/3]

| void ATAS.DataFeedsCore.Database.ICache.Remove | ( | [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a95153b2e9094a43f1b7b29e39fd9c9f7).

## [◆](https://docs.atas.net/en/)Save() [1/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | commissionGroup, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a280869be0e0c4b228f546bc96b126bcb).

## [◆](https://docs.atas.net/en/)Save() [2/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | exchange, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0004212f7bf7750fce988db9cfcf40f2).

## [◆](https://docs.atas.net/en/)Save() [3/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [HistoryMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | trade, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a9fee04e0700b0157bc402e8d619eaebe).

## [◆](https://docs.atas.net/en/)Save() [4/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa505a497c19e06fcb97677d962ff596b).

## [◆](https://docs.atas.net/en/)Save() [5/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | instrumentExchange, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a9ca47494b66be6744ab9a02c05c92282).

## [◆](https://docs.atas.net/en/)Save() [6/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a268de22d5db04ad84dbdb865bfecaa15).

## [◆](https://docs.atas.net/en/)Save() [7/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [News](../classes/classATAS_1_1DataFeedsCore_1_1News.md) | news, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a53b4625ca31809c47b28de288daf65f0).

## [◆](https://docs.atas.net/en/)Save() [8/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a671cc9d3e84c786b32b9278cbd4cc9ce).

## [◆](https://docs.atas.net/en/)Save() [9/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | bool | extended, |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#acc3726669f7a921cb1bb5f5863631578).

## [◆](https://docs.atas.net/en/)Save() [10/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [PortfolioChange](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) | portfolioChange, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a1c5745d77c74c14a310650fb70b84cd3).

## [◆](https://docs.atas.net/en/)Save() [11/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [PortfolioState](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioState.md) | portfolioState, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a86feb781ae29b986ea29f89ac9cec511).

## [◆](https://docs.atas.net/en/)Save() [12/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [PortfolioViewer](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) | viewer, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a177d2cd4e3a14c9633ddce146a919db9).

## [◆](https://docs.atas.net/en/)Save() [13/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ae2cd3e856ca132059d70607dce1ffdb5).

## [◆](https://docs.atas.net/en/)Save() [14/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab0fd1640536d83e95087b621d0796f19).

## [◆](https://docs.atas.net/en/)Save() [15/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | margin, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4c8238eaee2254752b67b3ce90c3cea8).

## [◆](https://docs.atas.net/en/)Save() [16/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) | user, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4cc915c2c3092c761bc77ca65b3e7ab0).

## [◆](https://docs.atas.net/en/)Save() [17/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | group, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab1ced22433ebd88eed11e7fbf4a82031).

## [◆](https://docs.atas.net/en/)Save() [18/18]

| void ATAS.DataFeedsCore.Database.ICache.Save | ( | [UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) | role, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4f0d4cf8e55bfbe0c965e9543532b8af).

## [◆](https://docs.atas.net/en/)SetValue()

| void ATAS.DataFeedsCore.Database.ICache.SetValue | ( | string | name, |
| --- | --- | --- | --- |
| | | string | value, |
| | | bool | wait = `false` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0e1a971ff4445abe90d1b0e47bc61753).

## [◆](https://docs.atas.net/en/)TryGetCommissionGroup() [1/2]

| [CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) ATAS.DataFeedsCore.Database.ICache.TryGetCommissionGroup | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a39b61392de7b1377d780547c63ecaa04).

## [◆](https://docs.atas.net/en/)TryGetCommissionGroup() [2/2]

| [CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) ATAS.DataFeedsCore.Database.ICache.TryGetCommissionGroup | ( | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a53861b9da1c181cb90355a6436d8817d).

## [◆](https://docs.atas.net/en/)TryGetExchange()

| [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) ATAS.DataFeedsCore.Database.ICache.TryGetExchange | ( | string | code | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a7ea0139857d870e58c48f328af146c46).

## [◆](https://docs.atas.net/en/)TryGetInstrumentExchange() [1/2]

| [InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) ATAS.DataFeedsCore.Database.ICache.TryGetInstrumentExchange | ( | string | instrument | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a2c9574e29ca19c2545051f1d7b2c1a05).

## [◆](https://docs.atas.net/en/)TryGetInstrumentExchange() [2/2]

| [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) ATAS.DataFeedsCore.Database.ICache.TryGetInstrumentExchange | ( | string | instrument, |
| --- | --- | --- | --- |
| | | string | code |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#abef7bc34b3817f572e6f47251e66a503).

## [◆](https://docs.atas.net/en/)TryGetMyTrade()

| [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) ATAS.DataFeedsCore.Database.ICache.TryGetMyTrade | ( | string | accountId, |
| --- | --- | --- | --- |
| | | string | tradeId, |
| | | bool | searchInDb |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#adc48eb821abfccffff59be1200255463).

## [◆](https://docs.atas.net/en/)TryGetOrder()

| [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) ATAS.DataFeedsCore.Database.ICache.TryGetOrder | ( | string | accountId, |
| --- | --- | --- | --- |
| | | long | extId, |
| | | bool | searchInDb |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad427ba77a07ab918d8cb779a1964bc5e).

## [◆](https://docs.atas.net/en/)TryGetPortfolio()

| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.DataFeedsCore.Database.ICache.TryGetPortfolio | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a48b3b7a900effa4fe3a6967a13e9e810).

## [◆](https://docs.atas.net/en/)TryGetPortfolioViewer()

| [PortfolioViewer](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) ATAS.DataFeedsCore.Database.ICache.TryGetPortfolioViewer | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a704d871644530f1089e61c087b0c602b).

## [◆](https://docs.atas.net/en/)TryGetPosition()

| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.DataFeedsCore.Database.ICache.TryGetPosition | ( | string | accountId, |
| --- | --- | --- | --- |
| | | string | securityId |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a1b8400efef6ece696947d137fff7cf10).

## [◆](https://docs.atas.net/en/)TryGetSecurityMargin()

| [SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) ATAS.DataFeedsCore.Database.ICache.TryGetSecurityMargin | ( | string | securityId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4a5b19ef707d177a363b3494e5ab7571).

## [◆](https://docs.atas.net/en/)TryGetUserGroup()

| [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) ATAS.DataFeedsCore.Database.ICache.TryGetUserGroup | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#adc19422b94dbb8fb80645223e4aac8fe).

## [◆](https://docs.atas.net/en/)TryGetUserRole()

| [UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) ATAS.DataFeedsCore.Database.ICache.TryGetUserRole | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad7e16b48851a1260bb85685ab4ad4d03).

## [◆](https://docs.atas.net/en/)Wait()

| void ATAS.DataFeedsCore.Database.ICache.Wait | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a025653697c4b90c5b2c4dae4dbb6506f).

## [◆](https://docs.atas.net/en/)WaitInitialized()

| void ATAS.DataFeedsCore.Database.ICache.WaitInitialized | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ae31e4269f6d0f2af0fcd640904e0a530).

## Property Documentation

## [◆](https://docs.atas.net/en/)CheckConsistency

| bool ATAS.DataFeedsCore.Database.ICache.CheckConsistency |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8475aa793b1bff4093984c69a5677b96).

## [◆](https://docs.atas.net/en/)ClearCachePeriod

| TimeSpan ATAS.DataFeedsCore.Database.ICache.ClearCachePeriod |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad9d5b1ad6275806916f43d4cff2aa420).

## [◆](https://docs.atas.net/en/)IsInitialized

| bool ATAS.DataFeedsCore.Database.ICache.IsInitialized |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a18c4082cc0129fa3196d59aa8476a116).

## [◆](https://docs.atas.net/en/)LastExtId

| long ATAS.DataFeedsCore.Database.ICache.LastExtId |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab1f0f24e6652c061fbd6561a1be11dee).

## [◆](https://docs.atas.net/en/)LastOrderId

| long ATAS.DataFeedsCore.Database.ICache.LastOrderId |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a34660d08e7d295bf6420a49a64ac5282).

## [◆](https://docs.atas.net/en/)LastTradeId

| long ATAS.DataFeedsCore.Database.ICache.LastTradeId |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8cffcf80a6ef547fc0e746366f961af4).

The documentation for this interface was generated from the following file:
- [ICache.cs](../files/ICache_8cs.md)
