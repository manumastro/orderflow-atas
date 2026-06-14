# ATAS.DataFeedsCore.Database.Cache< TConnection > Class Template Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.html

Inheritance diagram for ATAS.DataFeedsCore.Database.Cache< TConnection >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Database.Cache< TConnection >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#abeb7aac347e382f6fe9a24598637bae1) (string configurationName) |
| | |
| void | [Init](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a46d15982dc51fa926eb24ed4d0c5d6d0) (bool isServer) |
| | |
| ICollection | [GetPortfolios](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a6ecb29ed6c6ecbbfb1a5c25ddbd1212e) () |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [GetPortfolio](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a1878c54aee10f1f44193612232d9c91a) (string accountId) |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [TryGetPortfolio](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a48b3b7a900effa4fe3a6967a13e9e810) (string accountId) |
| | |
| ICollection | [GetPositions](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa5a5151177cee712a71d3e0a4ce98f7d) (string accountId) |
| | |
| ICollection | [GetPositions](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a013b68c60de555a58878a4a9268eb0be) () |
| | |
| ICollection | [GetOrders](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ae6a812963eeb16f3bbb3db719e361e35) (string accountId) |
| | |
| ICollection | [GetMyTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa715b70144cf7b7d5f548b9a4282880f) (string accountId) |
| | |
| IEnumerable | [GetPositionMyTrdades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a2959ebebb00f91cf45f431ddb2e8cfd1) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| IEnumerable | [GetOpenedMyTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a58363b757486b332cd69c596f2564350) () |
| | |
| IEnumerable | [GetOrders](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4254791589944259dfb1008518f4458a) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| IEnumerable | [GetMyTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab28bbd90a0fc4f1066492c82d3834080) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| IEnumerable | [GetMyTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#af50d79dc6118f9c8be786c7a5673902c) (string accountId, long tradeId) |
| | |
| IEnumerable | [GetHistoryTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a3c70ac8e8f3eaf837951065db0054de4) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [TryGetOrder](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad427ba77a07ab918d8cb779a1964bc5e) (string accountId, long extId, bool searchInDb) |
| | |
| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [TryGetMyTrade](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#adc48eb821abfccffff59be1200255463) (string accountId, string tradeId, bool searchInDb) |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [TryGetPosition](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a1b8400efef6ece696947d137fff7cf10) (string accountId, string securityId) |
| | |
| void | [DeletePortfolioData](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa39a25050b1462dbb8cba13c3d7ab84a) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| IEnumerable | [GetPortfolioChanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aaa683db86d6bc7110236715a223a24b9) (string accountId, DateTime from, DateTime to) |
| | |
| IEnumerable | [GetPortfolioChanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aaa1f0bc9cf9ea7e495a43bbe553c5f00) (DateTime from, DateTime to, IEnumerable accounts) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a1c5745d77c74c14a310650fb70b84cd3) ([PortfolioChange](./classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) portfolioChange, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a86feb781ae29b986ea29f89ac9cec511) ([PortfolioState](./classATAS_1_1DataFeedsCore_1_1PortfolioState.md) portfolioState, bool wait=false) |
| | |
| ICollection | [LoadAllSecurities](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0aa84c3773bd6d8026128dfc631eebc9) () |
| | |
| ICollection | [LoadAllSwapValues](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aac6357f86f459b19fbbb1747e7cc89a4) () |
| | |
| ICollection | [GetSecurities](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a865f5bc3528d411f79257d72ee98bf4b) () |
| | |
| ICollection | [GetPositionSecurities](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a08cdbecf9d3cf855c9bb69c2f8ba52d8) () |
| | |
| IEnumerable | [GetSecurities](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a337f5e298d2922da2ada30aea8cff567) (string code, string exchange) |
| | |
| IEnumerable | [GetSecuritiesByExchange](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8dadc5990d5093d5fe0cc99018ded4f9) (string exchange) |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [GetSecurity](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#afb4e02f86e865e9586283e3df92ef0c3) (string id) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa505a497c19e06fcb97677d962ff596b) (IEnumerable securities, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab0fd1640536d83e95087b621d0796f19) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, bool wait=false) |
| | |
| ICollection | [GetSecurityMargins](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a08c581b2cb3a2be9a41879717cd3a622) () |
| | |
| [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | [TryGetSecurityMargin](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4a5b19ef707d177a363b3494e5ab7571) (string securityId) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4c8238eaee2254752b67b3ce90c3cea8) ([SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) margin, bool wait=false) |
| | |
| ICollection | [GetExchanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0d26abf325cfdc2f6699234de819a752) () |
| | |
| ICollection | [GetInstrumentExchanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa2fb884165601d47a2eaac246e3cb053) () |
| | Get all InstrumentExchange need for import. |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | [GetExchange](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a598304055cca41b1add5f571c1f09ff8) (string code) |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | [TryGetExchange](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a7ea0139857d870e58c48f328af146c46) (string code) |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | [TryGetInstrumentExchange](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#abef7bc34b3817f572e6f47251e66a503) (string instrument, string code) |
| | |
| [InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | [TryGetInstrumentExchange](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a2c9574e29ca19c2545051f1d7b2c1a05) (string instrument) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0004212f7bf7750fce988db9cfcf40f2) ([Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) exchange, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a9ca47494b66be6744ab9a02c05c92282) ([InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) instrumentExchange, bool wait=false) |
| | |
| void | [Remove](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#af21755f27e44503414f147150e3dd811) ([Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) exchange) |
| | |
| void | [Remove](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#af99486c385dfdbab50a8b466edbe3e1c) ([InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) instrumentExchange) |
| | |
| ICollection | [GetUserRoles](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a3e0a7e6314e67b011c5db5b27b3cfa37) () |
| | |
| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) | [GetUserRole](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a5689a7ec79a27f05e1a30ae18477b0ee) (long id) |
| | |
| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) | [TryGetUserRole](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad7e16b48851a1260bb85685ab4ad4d03) (long id) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4f0d4cf8e55bfbe0c965e9543532b8af) ([UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) role, bool wait=false) |
| | |
| ICollection | [GetUserGroups](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a7c50be7711b13bcfb18a0e985b456d5a) () |
| | |
| ICollection | [GetUserGroups](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a9e4aec398d6c8e3bab29b4798f958a0a) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) parent) |
| | |
| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [GetUserGroup](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a06b2381dd1f66e1c4a9d497e7b3addc4) (long id) |
| | |
| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [TryGetUserGroup](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#adc19422b94dbb8fb80645223e4aac8fe) (long id) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab1ced22433ebd88eed11e7fbf4a82031) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) group, bool wait=false) |
| | |
| void | [DeleteUserGroupData](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa329c6e2f2f3868e961a2d6100ac6592) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) userGroup, [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md)? role) |
| | |
| ICollection | [GetUsers](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad1036b6ce5de5bcb2bc0d16c71d258b4) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) group) |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md)? | [GetUser](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a252b0d4d00010ea310196e3e05de6f8e) (string login) |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md) | [GetUser](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a464d958b111c017fc87280ae8cdad11d) (long id) |
| | |
| int | [GetNumberOfActiveUsers](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a054b3ca35b8fb63a5fc34aeb278482f3) () |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a4cc915c2c3092c761bc77ca65b3e7ab0) ([User](./classATAS_1_1DataFeedsCore_1_1User.md) user, bool wait=false) |
| | |
| ICollection | [GetCommissionGroups](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8765eca84ed682d222d758273a39bd7b) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) group) |
| | |
| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | [TryGetCommissionGroup](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a39b61392de7b1377d780547c63ecaa04) (long id) |
| | |
| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | [TryGetCommissionGroup](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a53861b9da1c181cb90355a6436d8817d) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a280869be0e0c4b228f546bc96b126bcb) ([CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) commissionGroup, bool wait=false) |
| | |
| string | [GetValue](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a5d15f5b42cd27dca9e4ae3c765a1521e) (string name) |
| | |
| void | [SetValue](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0e1a971ff4445abe90d1b0e47bc61753) (string name, string value, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a671cc9d3e84c786b32b9278cbd4cc9ce) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a268de22d5db04ad84dbdb865bfecaa15) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a9fee04e0700b0157bc402e8d619eaebe) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#acc3726669f7a921cb1bb5f5863631578) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, bool extended, bool wait=false) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ae2cd3e856ca132059d70607dce1ffdb5) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, bool wait=false) |
| | |
| void | [Remove](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a95153b2e9094a43f1b7b29e39fd9c9f7) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [ClearHistoryTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a22daaa76c7fdf3cb9f8052b9473c033a) () |
| | |
| void | [ClearMyTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aad7a9237a3efc21f291b34190b5bd63d) () |
| | |
| void | [ClearOrders](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ac942e3fe57251e6d47bed1c500e29f05) () |
| | |
| IEnumerable | [GetNews](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#afcfdc1f13f46ea0fe6b833b0c8b64007) (string[] accounts, DateTime from, DateTime to) |
| | |
| IEnumerable | [GetNews](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab1fb3dcb5123484e9657472fdac1c2aa) (DateTime from, DateTime to) |
| | |
| IEnumerable | [GetNews](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a666d74eea1b748fcbf77afa9ca5e5a7e) ([User](./classATAS_1_1DataFeedsCore_1_1User.md) user, bool unhandled) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a53b4625ca31809c47b28de288daf65f0) ([News](./classATAS_1_1DataFeedsCore_1_1News.md) news, bool wait=false) |
| | |
| ICollection | [GetPortfolioViewers](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a96202b7c5e2958a7be439b146e172ba8) () |
| | |
| [PortfolioViewer](./classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) | [TryGetPortfolioViewer](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a704d871644530f1089e61c087b0c602b) (long id) |
| | |
| void | [Save](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a177d2cd4e3a14c9633ddce146a919db9) ([PortfolioViewer](./classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) viewer, bool wait=false) |
| | |
| IEnumerable | [GetServerPnL](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aedc9610940edfc0e94c577c4f0e7e024) (DateTime from, DateTime to) |
| | |
| void | [Wait](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a025653697c4b90c5b2c4dae4dbb6506f) () |
| | |
| void | [WaitInitialized](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ae31e4269f6d0f2af0fcd640904e0a530) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a780c3370ea4a97b9c6265e4330622051) () |
| | Returns a string that represents the current object. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [GetOrCreateSecurity](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a29b704d52f60b160a635e2ddad1a3e89) (string id, Func create) |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [GetOrCreatePortfolio](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a5a63d44c4e237759d36506dcdde1c2e4) (string accountId, Func create) |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [GetOrCreatePosition](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a62edda6513533967789f059ddd1107f2) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, Func create) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [GetOrCreateOrder](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa9cd6200ba8cb5c82b14d8813ea71510) (long extId, Func create) |
| | |
| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [GetOrCreateMyTrade](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a34d2fb8904bda08f0a450af336161ea9) (string tradeId, Func create) |
| | |
| [Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) | [CreateTrade](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a03e2012cbd664c99ea66e11a47b01750) () |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [CreateMarketDepth](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a362ac55749ad2ee36ee05d4ad59b409a) () |
| | |
| | [Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#abeb7aac347e382f6fe9a24598637bae1) (string configurationName) |
| | |
| | [Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a2155c3dfc7c2bc700d69cf4075f43a64) (string providerName, string connectionString) |
| | |
| | [Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a656e4d1482db6677a967eaa440eadb5a) (DataProviderBase provider, string connectionString) |
| | |
| void | [Init](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3621740eaf8e559fb295e2762aee146e) (bool isServer) |
| | |
| ICollection | [GetPortfolios](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0ca1a05ed5ee6ccb597b3a19fabf8baf) () |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [GetPortfolio](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9b2590551f0c9a7556df373ca43a5a6a) (string accountId) |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [TryGetPortfolio](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#afe608c39709b0c90261b5cd1b38fee18) (string accountId) |
| | |
| ICollection | [GetPositions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0881445933d4da6d863f40f667be354b) (string accountId) |
| | |
| ICollection | [GetPositions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9558f439818d4dfc7eb2a1c2aff31099) () |
| | |
| ICollection | [GetOrders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa80ae63b7858a126c621374f411d74a9) (string accountId) |
| | |
| ICollection | [GetMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adae0091095f393cd7a82116bf2b9e1d1) (string accountId) |
| | |
| IEnumerable | [GetPositionMyTrdades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af65c5d1d8d58a2110df88da51141f952) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| IEnumerable | [GetOpenedMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a232082ea2e0a8c3be54a695bdd3cb30e) () |
| | |
| IEnumerable | [GetOrders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ab38c7dfa9121a45ad4d74794734671f5) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| IEnumerable | [GetMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a80c2f8baf6fc9b6f6da6af1fac759905) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| IEnumerable | [GetMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a884413949c8bc7dc07d6ea66eabeb066) (string accountId, long tradeId) |
| | |
| IEnumerable | [GetHistoryTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a10d1498d5f57fa5e9becde6eccf36611) (DateTime from, DateTime to, IEnumerable accounts, IEnumerable securities) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [TryGetOrder](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a5cb5a09dc7fea42ce586c63404426f80) (string accountId, long extId, bool searchInDb) |
| | |
| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [TryGetMyTrade](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a4539efbe205adb2b9fac552f08b29d14) (string accountId, string tradeId, bool searchInDb) |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [TryGetPosition](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aea2059d2d3639aa16cf0e529b7fe2760) (string accountId, string securityId) |
| | |
| IEnumerable | [GetPortfolioChanges](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a102de0e72d4192543a03544e31ec80bd) (string accountId, DateTime from, DateTime to) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a17115ee3cbfc16cf58bffe56f79e81ca) ([PortfolioChange](./classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) portfolioChange, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ada2f872842039bbf8f0f516a6d07645c) ([PortfolioState](./classATAS_1_1DataFeedsCore_1_1PortfolioState.md) portfolioState, bool wait=false) |
| | |
| ICollection | [LoadAllSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8d3cac41839c05ae3f2143cfbb91a997) () |
| | |
| ICollection | [GetSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#afd447f2fd08bfc457df68113a3a0dcf1) () |
| | |
| ICollection | [GetPositionSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7c06c5c65deba2214aaf05893000b39d) () |
| | |
| IEnumerable | [GetSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a478eb0e89de69c064f5e83094b92d1aa) (string code, string exchange) |
| | |
| IEnumerable | [GetSecuritiesByExchange](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aaca06a95853a2ccdca991544366ede38) (string exchange) |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [GetSecurity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adee048029cfb9e4aa7275f843f6e2c10) (string id) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2aecf6ccdc5c0577af2a002b9dda341e) (IEnumerable securities, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1fbbd0769f8f71b18389929df2bab361) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, bool wait=false) |
| | |
| ICollection | [GetSecurityMargins](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a12c62ac3cec0b465224bb955c454a524) () |
| | |
| [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | [TryGetSecurityMargin](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa3ce65bfa3cbba90a1aa41eefc5d9411) (string securityId) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a11ab43f11f35066b32770f482386f88e) ([SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) margin, bool wait=false) |
| | |
| ICollection | [GetExchanges](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a58177a7ff3b3d42f42c4afd1b8c63c17) () |
| | |
| ICollection | [GetInstrumentExchanges](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a92dfb269dfa42d4af30d56c3b07b453b) () |
| | Get all InstrumentExchange need for import. |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | [GetExchange](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adff4a124bc5d45012ae9b41534b402e2) (string code) |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | [TryGetExchange](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a11b0cb8584962a625ddc7c36f70582f6) (string code) |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | [TryGetInstrumentExchange](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a27889b1ee6b2aea68bfcd58450fccced) (string instrument, string code) |
| | |
| [InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | [TryGetInstrumentExchange](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0545167f603084f37498162e988ae651) (string instrument) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7746f3cad296615a2e31472d07e81374) ([Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) exchange, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8596b36dc5d1e2c1dbddef8b7b438ce0) ([InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) instrumentExchange, bool wait=false) |
| | |
| void | [Remove](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a763bc4413749b1605ceb039318c8293e) ([Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) exchange) |
| | |
| void | [Remove](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a546f29b385eff52e4bb6ac2a071d58ae) ([InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) instrumentExchange) |
| | |
| ICollection | [GetUserRoles](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3cd812bb2e453f45752f3b3cf8c5599f) () |
| | |
| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) | [GetUserRole](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7f2ee2201725c01439b4dc302a92bf4e) (long id) |
| | |
| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) | [TryGetUserRole](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9b12380274cc3ca259dfd8caf14ea574) (long id) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ad958bce8fa0df1a3b31149e91ff4a20d) ([UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) role, bool wait=false) |
| | |
| ICollection | [GetUserGroups](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aef64a101526e60dbd3b8f682e87f0225) () |
| | |
| ICollection | [GetUserGroups](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a58e43f4696b7ea9eb7fe4f97ce9897ce) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) parent) |
| | |
| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [GetUserGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af96a0f6e73908a1e9542e8b0bd0ccce9) (long id) |
| | |
| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | [TryGetUserGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a022576663874c4714e7e9f2a851be1fd) (long id) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af6663cd48d19a0cc154cf9901a5dc590) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) group, bool wait=false) |
| | |
| ICollection | [GetUsers](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2591be50ae02c6d53d769932c70c4c88) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) group) |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md)? | [GetUser](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a6bbc7d1610ca8747520105549ccf32c3) (string login) |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md)? | [GetUser](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a74052db11af8db2c9c9bb7b5491a1163) (long id) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adb2bf3aa9de9dbbd66b9655b6cd68c64) ([User](./classATAS_1_1DataFeedsCore_1_1User.md) user, bool wait=false) |
| | |
| ICollection | [GetCommissionGroups](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a77fe11f4499961a4bae70de1d74ae58f) ([UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) group) |
| | |
| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | [TryGetCommissionGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9dbf4b2d044c5ac58dfa8c4b8fb8fa40) (long id) |
| | |
| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | [TryGetCommissionGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a418b0430470f625e9a93868524d72806) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adb93637297f5d6f83818e4ce1eab3c98) ([CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) commissionGroup, bool wait=false) |
| | |
| string | [GetValue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2498d59903447eeb55914363e207606e) (string name) |
| | |
| void | [SetValue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a5bda3d3e1e17adac6dd47806bbea39a5) (string name, string value, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1d245ff73de1a198bb28319f2811c0d7) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a566eebda9fdd93126bce6b8f226a7d63) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1ed86e022309327e0b0bb29dcb749138) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ad586fdd449696e63cbaf839c68ad6604) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, bool extended, bool wait=false) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adc8add95a6dedb320ecd3bbf1e0342ed) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position, bool wait=false) |
| | |
| void | [Remove](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a44127a312d44770bc35fc1ea20f66d2c) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [ClearHistoryTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a22c3657bf6eeec237d2ae6f5733e1d1f) () |
| | |
| void | [ClearMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae985b1fa16c95bec66cc5f0947b6a006) () |
| | |
| void | [ClearOrders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae40d25831a4cd3cb9d05e7d6c662f744) () |
| | |
| IEnumerable | [GetNews](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a95631598c7817f51d6d7718d0bb500b5) (DateTime from, DateTime to) |
| | |
| IEnumerable | [GetNews](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3daa4b9366f76cb607823ce08e73ab34) ([User](./classATAS_1_1DataFeedsCore_1_1User.md) user, bool unhandled) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa6a900318aa68abdd39e1ac6fb66c9e3) ([News](./classATAS_1_1DataFeedsCore_1_1News.md) news, bool wait=false) |
| | |
| ICollection | [GetPortfolioViewers](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a17522de09b92b1bd4568f6de229c8c33) () |
| | |
| [PortfolioViewer](./classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) | [TryGetPortfolioViewer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a13f8da2dd7862b777248371061f802fe) (long id) |
| | |
| void | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a530c78fca5dc71f37e0f0f59e7eff73e) ([PortfolioViewer](./classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) viewer, bool wait=false) |
| | |
| IEnumerable | [GetServerPnL](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a6b1227ddb20257c68e10d484f35fb6c8) (DateTime from, DateTime to) |
| | |
| void | [Wait](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#abe32d05f484e610ed2ef7b408bc1abe3) () |
| | |
| void | [WaitInitialized](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae2c6264c2db3308e6d56f1112f573ec4) () |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [GetOrCreateSecurity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#ac4a0adcc45a4c830907c368e8921089c) (string id, Func create) |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [GetOrCreatePortfolio](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#af5c98f24eaa380af205502b08c69c4ba) (string accountId, Func create) |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [GetOrCreatePosition](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a5d990741b512ff237c7fc91034fa545e) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, Func create) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [GetOrCreateOrder](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a6e2ffa6dbeaf742978239c850ae30c7c) (long extId, Func create) |
| | |
| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [GetOrCreateMyTrade](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a443bf765ae3d3905a463711c0c5c418a) (string id, Func create) |
| | |
| [Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) | [CreateTrade](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a1fd4bbe23a8760e52fb808612f633ddb) () |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [CreateMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#aaa3a4477e0590fd6c7a08fca3e0891d7) () |
| | |

| Protected Member Functions | |
| --- | --- |
| | [Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a2155c3dfc7c2bc700d69cf4075f43a64) (string providerName, string connectionString) |
| | |
| | [Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a656e4d1482db6677a967eaa440eadb5a) (DataProviderBase provider, string connectionString) |
| | |
| virtual void | [OnInit](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a44aeb6bc4f873e27536aef51d49ee984) (TConnection db) |
| | |
| override void | [RaiseLoggingSettingsChanged](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#aa79178cb490d90bceeacc1b9fce6bb7a) (string propertyName="") |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md) | [GetUser](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#afe7856dc3636ed5f9325f47dbfe604d9) (TConnection db, long id) |
| | |
| abstract TConnection | [CreateDatabaseManager](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#af0b33e1aaec0c4345c6dbb0bebe2e991) () |
| | |
| T | [ProcessDb](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a11bcb3a3ef1623dd2be491e6db29002a) (Func action) |
| | |
| void | [Insert](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a95e6f6960b162f8150ffb897a23647c5) (TConnection db, T entity) |
| | |
| void | [InsertOrUpdate](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a505512f8b94a05a76437ae0a641fb6bf) (TConnection db, T entity) |
| | |
| void | [InsertWithIdentityOrUpdate](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a3439ee742ae6ddcd87f58d77ccd37100) (TConnection db, T entity, Func getId, Action setId) |
| | |
| void | [AddToQueue](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab4508add344473da38921bd4293eb2db) (Action value, bool wait) |
| | |
| void | [AddToQueue](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a3bd4648375668a8cae8db0b5fa2fd6db) (long id, Action value) |
| | |
| override [DatabaseManager](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md) | [CreateDatabaseManager](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a0dd88f1f489dddc8d4747fab75b96df5) () |
| | |

| Properties | |
| --- | --- |
| DataProviderBase | [Provider](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#acc2da5255ad7dce0056daa92a352bf04)`[get]` |
| | |
| string | [ConfigurationName](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a048f56a60365ea312a34ba400385ba93)`[get]` |
| | |
| string | [ProviderName](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a3e44086b0f9a49a332b5f9d4e74987b2)`[get]` |
| | |
| string | [ConnectionString](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ae6af3dc3c6e09fea053c6a8af30a68ae)`[get]` |
| | |
| long | [LastExtId](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ab1f0f24e6652c061fbd6561a1be11dee)`[get]` |
| | |
| long | [LastOrderId](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a34660d08e7d295bf6420a49a64ac5282)`[get]` |
| | |
| long | [LastTradeId](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8cffcf80a6ef547fc0e746366f961af4)`[get]` |
| | |
| bool | [IsInitialized](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a18c4082cc0129fa3196d59aa8476a116)`[get]` |
| | |
| TimeSpan | [ClearCachePeriod](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#ad9d5b1ad6275806916f43d4cff2aa420)`[get, set]` |
| | |
| bool | [CheckConsistency](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md#a8475aa793b1bff4093984c69a5677b96)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md) | |
| long | [LastExtId](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a92f7a13e47921d6b6a831d3b86f39c3e)`[get]` |
| | |
| long | [LastOrderId](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ab7a30e1dcf63b1050e9ae1a9565b38eb)`[get]` |
| | |
| long | [LastTradeId](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8f6ef9c563259d8787a4e5cacc8cdc4b)`[get]` |
| | |
| bool | [IsInitialized](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a87c272c47c1502ca33ae94e1907fafc1)`[get]` |
| | |
| TimeSpan | [ClearCachePeriod](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a37c4af331f1a63f1538da1d61f789e9a)`[get, set]` |
| | |
| bool | [CheckConsistency](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ac23098e9e83282020078d0c45cda2356)`[get, set]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Cache() [1/6]

| [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).[Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | ( | string | configurationName | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Cache() [2/6]

| [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).[Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | ( | string | providerName, |
| --- | --- | --- | --- |
| | | string | connectionString |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)Cache() [3/6]

| [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).[Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | ( | DataProviderBase | provider, |
| --- | --- | --- | --- |
| | | string | connectionString |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)Cache() [4/6]

| [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).[Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | ( | string | configurationName | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Cache() [5/6]

| [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).[Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | ( | string | providerName, |
| --- | --- | --- | --- |
| | | string | connectionString |
| | ) | | |

## [◆](https://docs.atas.net/en/)Cache() [6/6]

| [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).[Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | ( | DataProviderBase | provider, |
| --- | --- | --- | --- |
| | | string | connectionString |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)AddToQueue() [1/2]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).AddToQueue | ( | Action | value, |
| --- | --- | --- | --- |
| | | bool | wait |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)AddToQueue() [2/2]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).AddToQueue | ( | long | id, |
| --- | --- | --- | --- |
| | | Action | value |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)ClearHistoryTrades()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ClearHistoryTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a22c3657bf6eeec237d2ae6f5733e1d1f).

## [◆](https://docs.atas.net/en/)ClearMyTrades()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ClearMyTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae985b1fa16c95bec66cc5f0947b6a006).

## [◆](https://docs.atas.net/en/)ClearOrders()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ClearOrders | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae40d25831a4cd3cb9d05e7d6c662f744).

## [◆](https://docs.atas.net/en/)CreateDatabaseManager() [1/2]

| abstract TConnection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).CreateDatabaseManager | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedpure virtual

## [◆](https://docs.atas.net/en/)CreateDatabaseManager() [2/2]

| override [DatabaseManager](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).CreateDatabaseManager | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)CreateMarketDepth()

| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).CreateMarketDepth | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#aaa3a4477e0590fd6c7a08fca3e0891d7).

## [◆](https://docs.atas.net/en/)CreateTrade()

| [Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).CreateTrade | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a1fd4bbe23a8760e52fb808612f633ddb).

## [◆](https://docs.atas.net/en/)DeletePortfolioData()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).DeletePortfolioData | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)DeleteUserGroupData()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).DeleteUserGroupData | ( | [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | userGroup, |
| --- | --- | --- | --- |
| | | [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md)? | role |
| | ) | | |

## [◆](https://docs.atas.net/en/)GetCommissionGroups()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetCommissionGroups | ( | [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | group | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a77fe11f4499961a4bae70de1d74ae58f).

## [◆](https://docs.atas.net/en/)GetExchange()

| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetExchange | ( | string | code | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adff4a124bc5d45012ae9b41534b402e2).

## [◆](https://docs.atas.net/en/)GetExchanges()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetExchanges | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a58177a7ff3b3d42f42c4afd1b8c63c17).

## [◆](https://docs.atas.net/en/)GetHistoryTrades()

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetHistoryTrades | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | IEnumerable | accounts, |
| | | IEnumerable | securities |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a10d1498d5f57fa5e9becde6eccf36611).

## [◆](https://docs.atas.net/en/)GetInstrumentExchanges()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetInstrumentExchanges | ( | | ) | |
| --- | --- | --- | --- | --- |

Get all InstrumentExchange need for import.

Returns

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a92dfb269dfa42d4af30d56c3b07b453b).

## [◆](https://docs.atas.net/en/)GetMyTrades() [1/3]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetMyTrades | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | IEnumerable | accounts, |
| | | IEnumerable | securities |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a80c2f8baf6fc9b6f6da6af1fac759905).

## [◆](https://docs.atas.net/en/)GetMyTrades() [2/3]

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetMyTrades | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adae0091095f393cd7a82116bf2b9e1d1).

## [◆](https://docs.atas.net/en/)GetMyTrades() [3/3]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetMyTrades | ( | string | accountId, |
| --- | --- | --- | --- |
| | | long | tradeId |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a884413949c8bc7dc07d6ea66eabeb066).

## [◆](https://docs.atas.net/en/)GetNews() [1/3]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetNews | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a95631598c7817f51d6d7718d0bb500b5).

## [◆](https://docs.atas.net/en/)GetNews() [2/3]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetNews | ( | string[] | accounts, |
| --- | --- | --- | --- |
| | | DateTime | from, |
| | | DateTime | to |
| | ) | | |

## [◆](https://docs.atas.net/en/)GetNews() [3/3]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetNews | ( | [User](./classATAS_1_1DataFeedsCore_1_1User.md) | user, |
| --- | --- | --- | --- |
| | | bool | unhandled |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3daa4b9366f76cb607823ce08e73ab34).

## [◆](https://docs.atas.net/en/)GetNumberOfActiveUsers()

| int [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetNumberOfActiveUsers | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetOpenedMyTrades()

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOpenedMyTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a232082ea2e0a8c3be54a695bdd3cb30e).

## [◆](https://docs.atas.net/en/)GetOrCreateMyTrade()

| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOrCreateMyTrade | ( | string | tradeId, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implements [ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a443bf765ae3d3905a463711c0c5c418a).

## [◆](https://docs.atas.net/en/)GetOrCreateOrder()

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOrCreateOrder | ( | long | extId, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implements [ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a6e2ffa6dbeaf742978239c850ae30c7c).

## [◆](https://docs.atas.net/en/)GetOrCreatePortfolio()

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOrCreatePortfolio | ( | string | accountId, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implements [ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#af5c98f24eaa380af205502b08c69c4ba).

## [◆](https://docs.atas.net/en/)GetOrCreatePosition()

| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOrCreatePosition | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | Func | create |
| | ) | | |

Implements [ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#a5d990741b512ff237c7fc91034fa545e).

## [◆](https://docs.atas.net/en/)GetOrCreateSecurity()

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOrCreateSecurity | ( | string | id, |
| --- | --- | --- | --- |
| | | Func | create |
| | ) | | |

Implements [ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md#ac4a0adcc45a4c830907c368e8921089c).

## [◆](https://docs.atas.net/en/)GetOrders() [1/2]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOrders | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | IEnumerable | accounts, |
| | | IEnumerable | securities |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ab38c7dfa9121a45ad4d74794734671f5).

## [◆](https://docs.atas.net/en/)GetOrders() [2/2]

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetOrders | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa80ae63b7858a126c621374f411d74a9).

## [◆](https://docs.atas.net/en/)GetPortfolio()

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPortfolio | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9b2590551f0c9a7556df373ca43a5a6a).

## [◆](https://docs.atas.net/en/)GetPortfolioChanges() [1/2]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPortfolioChanges | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | IEnumerable | accounts |
| | ) | | |

## [◆](https://docs.atas.net/en/)GetPortfolioChanges() [2/2]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPortfolioChanges | ( | string | accountId, |
| --- | --- | --- | --- |
| | | DateTime | from, |
| | | DateTime | to |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a102de0e72d4192543a03544e31ec80bd).

## [◆](https://docs.atas.net/en/)GetPortfolios()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPortfolios | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0ca1a05ed5ee6ccb597b3a19fabf8baf).

## [◆](https://docs.atas.net/en/)GetPortfolioViewers()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPortfolioViewers | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a17522de09b92b1bd4568f6de229c8c33).

## [◆](https://docs.atas.net/en/)GetPositionMyTrdades()

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPositionMyTrdades | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af65c5d1d8d58a2110df88da51141f952).

## [◆](https://docs.atas.net/en/)GetPositions() [1/2]

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPositions | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9558f439818d4dfc7eb2a1c2aff31099).

## [◆](https://docs.atas.net/en/)GetPositions() [2/2]

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPositions | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0881445933d4da6d863f40f667be354b).

## [◆](https://docs.atas.net/en/)GetPositionSecurities()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetPositionSecurities | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7c06c5c65deba2214aaf05893000b39d).

## [◆](https://docs.atas.net/en/)GetSecurities() [1/2]

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetSecurities | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#afd447f2fd08bfc457df68113a3a0dcf1).

## [◆](https://docs.atas.net/en/)GetSecurities() [2/2]

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetSecurities | ( | string | code, |
| --- | --- | --- | --- |
| | | string | exchange |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a478eb0e89de69c064f5e83094b92d1aa).

## [◆](https://docs.atas.net/en/)GetSecuritiesByExchange()

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetSecuritiesByExchange | ( | string | exchange | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aaca06a95853a2ccdca991544366ede38).

## [◆](https://docs.atas.net/en/)GetSecurity()

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetSecurity | ( | string | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adee048029cfb9e4aa7275f843f6e2c10).

## [◆](https://docs.atas.net/en/)GetSecurityMargins()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetSecurityMargins | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a12c62ac3cec0b465224bb955c454a524).

## [◆](https://docs.atas.net/en/)GetServerPnL()

| IEnumerable [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetServerPnL | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a6b1227ddb20257c68e10d484f35fb6c8).

## [◆](https://docs.atas.net/en/)GetUser() [1/3]

| [User](./classATAS_1_1DataFeedsCore_1_1User.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUser | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a74052db11af8db2c9c9bb7b5491a1163).

## [◆](https://docs.atas.net/en/)GetUser() [2/3]

| [User](./classATAS_1_1DataFeedsCore_1_1User.md)? [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUser | ( | string | login | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a6bbc7d1610ca8747520105549ccf32c3).

## [◆](https://docs.atas.net/en/)GetUser() [3/3]

| [User](./classATAS_1_1DataFeedsCore_1_1User.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUser | ( | TConnection | db, |
| --- | --- | --- | --- |
| | | long | id |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)GetUserGroup()

| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUserGroup | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af96a0f6e73908a1e9542e8b0bd0ccce9).

## [◆](https://docs.atas.net/en/)GetUserGroups() [1/2]

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUserGroups | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aef64a101526e60dbd3b8f682e87f0225).

## [◆](https://docs.atas.net/en/)GetUserGroups() [2/2]

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUserGroups | ( | [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | parent | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a58e43f4696b7ea9eb7fe4f97ce9897ce).

## [◆](https://docs.atas.net/en/)GetUserRole()

| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUserRole | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7f2ee2201725c01439b4dc302a92bf4e).

## [◆](https://docs.atas.net/en/)GetUserRoles()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUserRoles | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3cd812bb2e453f45752f3b3cf8c5599f).

## [◆](https://docs.atas.net/en/)GetUsers()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetUsers | ( | [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | group | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2591be50ae02c6d53d769932c70c4c88).

## [◆](https://docs.atas.net/en/)GetValue()

| string [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).GetValue | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2498d59903447eeb55914363e207606e).

## [◆](https://docs.atas.net/en/)Init()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Init | ( | bool | isServer | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a3621740eaf8e559fb295e2762aee146e).

## [◆](https://docs.atas.net/en/)Insert()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Insert | ( | TConnection | db, |
| --- | --- | --- | --- |
| | | T | entity |
| | ) | | |

protected

Type Constraints

| T | : | class | |
| --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)InsertOrUpdate()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).InsertOrUpdate | ( | TConnection | db, |
| --- | --- | --- | --- |
| | | T | entity |
| | ) | | |

protected

Type Constraints

| T | : | class | |
| --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)InsertWithIdentityOrUpdate()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).InsertWithIdentityOrUpdate | ( | TConnection | db, |
| --- | --- | --- | --- |
| | | T | entity, |
| | | Func | getId, |
| | | Action | setId |
| | ) | | |

protected

Type Constraints

| T | : | class | |
| --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)LoadAllSecurities()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).LoadAllSecurities | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8d3cac41839c05ae3f2143cfbb91a997).

## [◆](https://docs.atas.net/en/)LoadAllSwapValues()

| ICollection [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).LoadAllSwapValues | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnInit()

| virtual void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).OnInit | ( | TConnection | db | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ProcessDb()

| T [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ProcessDb | ( | Func | action | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)RaiseLoggingSettingsChanged()

| override void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).RaiseLoggingSettingsChanged | ( | string | propertyName = `""` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)Remove() [1/3]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Remove | ( | [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | exchange | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a763bc4413749b1605ceb039318c8293e).

## [◆](https://docs.atas.net/en/)Remove() [2/3]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Remove | ( | [InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | instrumentExchange | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a546f29b385eff52e4bb6ac2a071d58ae).

## [◆](https://docs.atas.net/en/)Remove() [3/3]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Remove | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a44127a312d44770bc35fc1ea20f66d2c).

## [◆](https://docs.atas.net/en/)Save() [1/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | commissionGroup, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adb93637297f5d6f83818e4ce1eab3c98).

## [◆](https://docs.atas.net/en/)Save() [2/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) | exchange, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a7746f3cad296615a2e31472d07e81374).

## [◆](https://docs.atas.net/en/)Save() [3/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | trade, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1ed86e022309327e0b0bb29dcb749138).

## [◆](https://docs.atas.net/en/)Save() [4/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a2aecf6ccdc5c0577af2a002b9dda341e).

## [◆](https://docs.atas.net/en/)Save() [5/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | instrumentExchange, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8596b36dc5d1e2c1dbddef8b7b438ce0).

## [◆](https://docs.atas.net/en/)Save() [6/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a566eebda9fdd93126bce6b8f226a7d63).

## [◆](https://docs.atas.net/en/)Save() [7/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [News](./classATAS_1_1DataFeedsCore_1_1News.md) | news, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa6a900318aa68abdd39e1ac6fb66c9e3).

## [◆](https://docs.atas.net/en/)Save() [8/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1d245ff73de1a198bb28319f2811c0d7).

## [◆](https://docs.atas.net/en/)Save() [9/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| --- | --- | --- | --- |
| | | bool | extended, |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ad586fdd449696e63cbaf839c68ad6604).

## [◆](https://docs.atas.net/en/)Save() [10/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [PortfolioChange](./classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) | portfolioChange, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a17115ee3cbfc16cf58bffe56f79e81ca).

## [◆](https://docs.atas.net/en/)Save() [11/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [PortfolioState](./classATAS_1_1DataFeedsCore_1_1PortfolioState.md) | portfolioState, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ada2f872842039bbf8f0f516a6d07645c).

## [◆](https://docs.atas.net/en/)Save() [12/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [PortfolioViewer](./classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) | viewer, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a530c78fca5dc71f37e0f0f59e7eff73e).

## [◆](https://docs.atas.net/en/)Save() [13/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adc8add95a6dedb320ecd3bbf1e0342ed).

## [◆](https://docs.atas.net/en/)Save() [14/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a1fbbd0769f8f71b18389929df2bab361).

## [◆](https://docs.atas.net/en/)Save() [15/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | margin, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a11ab43f11f35066b32770f482386f88e).

## [◆](https://docs.atas.net/en/)Save() [16/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [User](./classATAS_1_1DataFeedsCore_1_1User.md) | user, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#adb2bf3aa9de9dbbd66b9655b6cd68c64).

## [◆](https://docs.atas.net/en/)Save() [17/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) | group, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#af6663cd48d19a0cc154cf9901a5dc590).

## [◆](https://docs.atas.net/en/)Save() [18/18]

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Save | ( | [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) | role, |
| --- | --- | --- | --- |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ad958bce8fa0df1a3b31149e91ff4a20d).

## [◆](https://docs.atas.net/en/)SetValue()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).SetValue | ( | string | name, |
| --- | --- | --- | --- |
| | | string | value, |
| | | bool | wait = `false` |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a5bda3d3e1e17adac6dd47806bbea39a5).

## [◆](https://docs.atas.net/en/)ToString()

| override string [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## [◆](https://docs.atas.net/en/)TryGetCommissionGroup() [1/2]

| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetCommissionGroup | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9dbf4b2d044c5ac58dfa8c4b8fb8fa40).

## [◆](https://docs.atas.net/en/)TryGetCommissionGroup() [2/2]

| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetCommissionGroup | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a418b0430470f625e9a93868524d72806).

## [◆](https://docs.atas.net/en/)TryGetExchange()

| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetExchange | ( | string | code | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a11b0cb8584962a625ddc7c36f70582f6).

## [◆](https://docs.atas.net/en/)TryGetInstrumentExchange() [1/2]

| [InstrumentExchange](./classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetInstrumentExchange | ( | string | instrument | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a0545167f603084f37498162e988ae651).

## [◆](https://docs.atas.net/en/)TryGetInstrumentExchange() [2/2]

| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetInstrumentExchange | ( | string | instrument, |
| --- | --- | --- | --- |
| | | string | code |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a27889b1ee6b2aea68bfcd58450fccced).

## [◆](https://docs.atas.net/en/)TryGetMyTrade()

| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetMyTrade | ( | string | accountId, |
| --- | --- | --- | --- |
| | | string | tradeId, |
| | | bool | searchInDb |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a4539efbe205adb2b9fac552f08b29d14).

## [◆](https://docs.atas.net/en/)TryGetOrder()

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetOrder | ( | string | accountId, |
| --- | --- | --- | --- |
| | | long | extId, |
| | | bool | searchInDb |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a5cb5a09dc7fea42ce586c63404426f80).

## [◆](https://docs.atas.net/en/)TryGetPortfolio()

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetPortfolio | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#afe608c39709b0c90261b5cd1b38fee18).

## [◆](https://docs.atas.net/en/)TryGetPortfolioViewer()

| [PortfolioViewer](./classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetPortfolioViewer | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a13f8da2dd7862b777248371061f802fe).

## [◆](https://docs.atas.net/en/)TryGetPosition()

| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetPosition | ( | string | accountId, |
| --- | --- | --- | --- |
| | | string | securityId |
| | ) | | |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aea2059d2d3639aa16cf0e529b7fe2760).

## [◆](https://docs.atas.net/en/)TryGetSecurityMargin()

| [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetSecurityMargin | ( | string | securityId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#aa3ce65bfa3cbba90a1aa41eefc5d9411).

## [◆](https://docs.atas.net/en/)TryGetUserGroup()

| [UserGroup](./classATAS_1_1DataFeedsCore_1_1UserGroup.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetUserGroup | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a022576663874c4714e7e9f2a851be1fd).

## [◆](https://docs.atas.net/en/)TryGetUserRole()

| [UserRole](./classATAS_1_1DataFeedsCore_1_1UserRole.md) [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).TryGetUserRole | ( | long | id | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a9b12380274cc3ca259dfd8caf14ea574).

## [◆](https://docs.atas.net/en/)Wait()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Wait | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#abe32d05f484e610ed2ef7b408bc1abe3).

## [◆](https://docs.atas.net/en/)WaitInitialized()

| void [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).WaitInitialized | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ae2c6264c2db3308e6d56f1112f573ec4).

## Property Documentation

## [◆](https://docs.atas.net/en/)CheckConsistency

| bool [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).CheckConsistency |
| --- |

getset

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ac23098e9e83282020078d0c45cda2356).

## [◆](https://docs.atas.net/en/)ClearCachePeriod

| TimeSpan [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ClearCachePeriod |
| --- |

getset

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a37c4af331f1a63f1538da1d61f789e9a).

## [◆](https://docs.atas.net/en/)ConfigurationName

| string [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ConfigurationName |
| --- |

get

## [◆](https://docs.atas.net/en/)ConnectionString

| string [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ConnectionString |
| --- |

get

## [◆](https://docs.atas.net/en/)IsInitialized

| bool [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).IsInitialized |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a87c272c47c1502ca33ae94e1907fafc1).

## [◆](https://docs.atas.net/en/)LastExtId

| long [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).LastExtId |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a92f7a13e47921d6b6a831d3b86f39c3e).

## [◆](https://docs.atas.net/en/)LastOrderId

| long [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).LastOrderId |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#ab7a30e1dcf63b1050e9ae1a9565b38eb).

## [◆](https://docs.atas.net/en/)LastTradeId

| long [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).LastTradeId |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md#a8f6ef9c563259d8787a4e5cacc8cdc4b).

## [◆](https://docs.atas.net/en/)Provider

| DataProviderBase [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).Provider |
| --- |

get

## [◆](https://docs.atas.net/en/)ProviderName

| string [ATAS.DataFeedsCore.Database.Cache](./classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md).ProviderName |
| --- |

get

The documentation for this class was generated from the following file:
- [Cache.cs](../files/Cache_8cs.md)
