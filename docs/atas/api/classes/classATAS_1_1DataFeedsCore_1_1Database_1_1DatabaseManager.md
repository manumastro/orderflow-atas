# ATAS.DataFeedsCore.Database.DatabaseManager Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.html

Inheritance diagram for ATAS.DataFeedsCore.Database.DatabaseManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Database.DatabaseManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [DatabaseManager](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a9962f9dcaace1f7c3354ae94874302a7) (string configurationName) |
| | |
| | [DatabaseManager](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a9bc9f07136bfff31d8ee756969871a63) (string providerName, string connectionString) |
| | |
| | [DatabaseManager](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a83094030af983140fca02ca563d2c17e) (DataProviderBase provider, string connectionString) |
| | |
| double? | [Initialize](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a31a20091aa1dbae700924eead5628d66) () |
| | |
| long? | [GetCommissionGroupByPortfolio](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#adeacc009b7ecdf4ce70511e603949808) (string accountId) |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md) | |
| long? | [GetCommissionGroupByPortfolio](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ae6e221367eaf3f71be807a08b6a32975) (string accountId) |
| | |
| double? | [Initialize](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad6ecb1359f382e1c7f095ee7f9890d3a) () |
| | |
| int | [Delete](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a74673b34004f640ec8c9d5be35676471) (TEntity entity) |
| | |
| void | [BeginTransaction](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa114005e2efec7be0358f0504d927306) () |
| | |
| void | [CommitTransaction](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a64829dc15bfc706ec7adabbeebe39741) () |
| | |
| void | [RollbackTransaction](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#af1c9d562304bd281e2b30bcdd537a447) () |
| | |

| Properties | |
| --- | --- |
| long | [LastExtId](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a261be598b0be0693385e68a3c2f70441)`[get]` |
| | |
| long | [LastOrderId](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#aab580f8c55402043f597f281a8af3117)`[get]` |
| | |
| long | [LastTradeId](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#acb8c39460f12b6deadd233b0c63cd9b9)`[get]` |
| | |
| IQueryable | [Securities](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a60e2dc352583a94186b9d2412169ec60)`[get]` |
| | |
| IQueryable | [Portfolios](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a21d1fa144139d190b584d25f50de017c)`[get]` |
| | |
| IQueryable | [Positions](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a2b62face029fd6a602f8e766f97a6257)`[get]` |
| | |
| IQueryable | [Orders](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5e38f19ddd23fb5aa23e4cb37beee9bf)`[get]` |
| | |
| IQueryable | [MyTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5751df732e044302675bf2b72c779941)`[get]` |
| | |
| IQueryable | [Users](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a8c558c607c9912e7be37e1e1b0a1eca0)`[get]` |
| | |
| IQueryable | [UserRoles](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a36aae166464f43c3b62fd7bd6eda6e75)`[get]` |
| | |
| IQueryable | [UserRoleRights](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a9fc029b2d6d6eb145f8206e347c479c8)`[get]` |
| | |
| IQueryable | [UserGroups](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#af0d28faecd4de3c62dbe32756e6494f5)`[get]` |
| | |
| IQueryable | [GroupExchanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a20f9dd1b7b3fc6d344560f38974c2564)`[get]` |
| | |
| IQueryable | [CommissionGroups](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a57b26409ef0c4b94dcaba669252c1231)`[get]` |
| | |
| IQueryable | [HistoryMyTrades](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a293adec8bfa6f3d4c06d29b3fde86575)`[get]` |
| | |
| IQueryable | [Settings](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#abd864d1c87ded51330b54e4d69f93e9f)`[get]` |
| | |
| IQueryable | [Exchanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#ab43de0ed18f3c3571532e15469378480)`[get]` |
| | |
| IQueryable | [WorkingTimes](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#ab646720d4ef983c916d8797ea3044205)`[get]` |
| | |
| IQueryable | [SecurityMargins](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#aa792516c94f036480dfe227a367e1e3f)`[get]` |
| | |
| IQueryable | [News](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#ad9360dfda291e897c95eff849e95e617)`[get]` |
| | |
| IQueryable | [TradingOptions](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a4faa7dd295b1241bc43b74a25f20de0f)`[get]` |
| | |
| IQueryable | [TradingOptionsSecurities](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5c8ba1adc6c7ecf54d51b5ba81343a28)`[get]` |
| | |
| IQueryable | [CommissionGroupItems](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#aa4f242720bcfa10d4341a614b87049bd)`[get]` |
| | |
| IQueryable | [PortfolioChanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a89f76c18cf4cd7f78e1514e4d93313d1)`[get]` |
| | |
| IQueryable | [PortfolioStates](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#af5cbaffdd2640a102213095eb9aa0c3c)`[get]` |
| | |
| IQueryable | [PositionStates](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a73e376ccf1c85d06e06392334dbc9ee4)`[get]` |
| | |
| IQueryable | [SecurityRoutes](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5647f6f035228ce85c661f21993a356d)`[get]` |
| | |
| IQueryable | [PortfolioViewers](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a678f94198cf10fca8e4e161adced7682)`[get]` |
| | |
| IQueryable | [PortfolioGroups](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a8981630b481c8244df3264ab1284c9c8)`[get]` |
| | |
| IQueryable | [ServerPnL](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#af626cb488998781436a825270e93af40)`[get]` |
| | |
| IQueryable | [InstrumentExchanges](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a1bbde5c79db460ee88df7b27e3fc0031)`[get]` |
| | |
| IQueryable | [OvernightSwapValues](./classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#aac81764e2c5ff381ded0c6de88cf6b14)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md) | |
| long | [LastExtId](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a78554f7538630612631e7910822f8036)`[get]` |
| | |
| long | [LastOrderId](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa4f689db52838fe3151a2dd7f18f4788)`[get]` |
| | |
| long | [LastTradeId](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a50cc264375f878d60c736d5147b0015b)`[get]` |
| | |
| IQueryable | [Securities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a67a0ffbf0372c0a3c8b9fb4a7c8c5cc6)`[get]` |
| | |
| IQueryable | [Portfolios](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a7f1880c509917a0123aebf9bc5e45b74)`[get]` |
| | |
| IQueryable | [Positions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a0381e74a23b505d3f84458f16632b470)`[get]` |
| | |
| IQueryable | [Orders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#af566500564a26ab6db8f7cb30e2fabe6)`[get]` |
| | |
| IQueryable | [MyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a89d9da68d5632b6bb701b4d995042446)`[get]` |
| | |
| IQueryable | [Users](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad4133e6436811d4738f1a0eb4eb5e575)`[get]` |
| | |
| IQueryable | [UserRoles](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac80cbd4698bf0be7c761b7fda13030dd)`[get]` |
| | |
| IQueryable | [UserRoleRights](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a376af4ac0a394b6c77177955ec6dce7a)`[get]` |
| | |
| IQueryable | [UserGroups](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a11cb8cf39d854b1e465931697242b105)`[get]` |
| | |
| IQueryable | [GroupExchanges](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac2397bd26eff3375826a4e1764d3f757)`[get]` |
| | |
| IQueryable | [CommissionGroups](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a1d78c9959607138b49ec34d2b3c1bedf)`[get]` |
| | |
| IQueryable | [HistoryMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a50bdae307ba17633cd6da4d5f0606193)`[get]` |
| | |
| IQueryable | [Settings](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a96b123cc78ede4735996d62622eab3e8)`[get]` |
| | |
| IQueryable | [Exchanges](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa20cdd15f1045e10488365e6616e99b7)`[get]` |
| | |
| IQueryable | [WorkingTimes](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a72b3ffb43cd53c3905cf9fabe95f74a6)`[get]` |
| | |
| IQueryable | [SecurityMargins](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a97ec44f29bc32553efb2a17b0db4c8ed)`[get]` |
| | |
| IQueryable | [News](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a634e78ed4424dac4d3443b0ee00f17ea)`[get]` |
| | |
| IQueryable | [TradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a088db28bd3ab67a275092c3a8385ca77)`[get]` |
| | |
| IQueryable | [TradingOptionsSecurities](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a9d3bddfd02a6f0008b721a57c20c90f1)`[get]` |
| | |
| IQueryable | [CommissionGroupItems](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a7033aff96bda967fa72e9444a4ce63e7)`[get]` |
| | |
| IQueryable | [PortfolioChanges](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a4692f57c0115568f46254d13a599eb99)`[get]` |
| | |
| IQueryable | [PortfolioStates](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad945ba967e69ec7d7ecbaf498eed1d9a)`[get]` |
| | |
| IQueryable | [PositionStates](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a5b279766f822ce3cc57b9a5757c68273)`[get]` |
| | |
| IQueryable | [SecurityRoutes](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a60e4ef3c88bcbb6a8210f3bef1e76ae8)`[get]` |
| | |
| IQueryable | [PortfolioViewers](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa22089b5dcd9202f4c0398a4a14d9be5)`[get]` |
| | |
| IQueryable | [PortfolioGroups](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac63340650493dea57fbd8ab834f05942)`[get]` |
| | |
| IQueryable | [ServerPnL](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a9e6af62fceed7304b92a6ff14fe59849)`[get]` |
| | |
| IQueryable | [InstrumentExchanges](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a151d9b9c2212246026c55bedf88df7e9)`[get]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)DatabaseManager() [1/3]

| ATAS.DataFeedsCore.Database.DatabaseManager.DatabaseManager | ( | string | configurationName | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)DatabaseManager() [2/3]

| ATAS.DataFeedsCore.Database.DatabaseManager.DatabaseManager | ( | string | providerName, |
| --- | --- | --- | --- |
| | | string | connectionString |
| | ) | | |

## [◆](https://docs.atas.net/en/)DatabaseManager() [3/3]

| ATAS.DataFeedsCore.Database.DatabaseManager.DatabaseManager | ( | DataProviderBase | provider, |
| --- | --- | --- | --- |
| | | string | connectionString |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetCommissionGroupByPortfolio()

| long? ATAS.DataFeedsCore.Database.DatabaseManager.GetCommissionGroupByPortfolio | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ae6e221367eaf3f71be807a08b6a32975).

## [◆](https://docs.atas.net/en/)Initialize()

| double? ATAS.DataFeedsCore.Database.DatabaseManager.Initialize | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad6ecb1359f382e1c7f095ee7f9890d3a).

## Property Documentation

## [◆](https://docs.atas.net/en/)CommissionGroupItems

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.CommissionGroupItems |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a7033aff96bda967fa72e9444a4ce63e7).

## [◆](https://docs.atas.net/en/)CommissionGroups

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.CommissionGroups |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a1d78c9959607138b49ec34d2b3c1bedf).

## [◆](https://docs.atas.net/en/)Exchanges

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.Exchanges |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa20cdd15f1045e10488365e6616e99b7).

## [◆](https://docs.atas.net/en/)GroupExchanges

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.GroupExchanges |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac2397bd26eff3375826a4e1764d3f757).

## [◆](https://docs.atas.net/en/)HistoryMyTrades

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.HistoryMyTrades |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a50bdae307ba17633cd6da4d5f0606193).

## [◆](https://docs.atas.net/en/)InstrumentExchanges

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.InstrumentExchanges |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a151d9b9c2212246026c55bedf88df7e9).

## [◆](https://docs.atas.net/en/)LastExtId

| long ATAS.DataFeedsCore.Database.DatabaseManager.LastExtId |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a78554f7538630612631e7910822f8036).

## [◆](https://docs.atas.net/en/)LastOrderId

| long ATAS.DataFeedsCore.Database.DatabaseManager.LastOrderId |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa4f689db52838fe3151a2dd7f18f4788).

## [◆](https://docs.atas.net/en/)LastTradeId

| long ATAS.DataFeedsCore.Database.DatabaseManager.LastTradeId |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a50cc264375f878d60c736d5147b0015b).

## [◆](https://docs.atas.net/en/)MyTrades

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.MyTrades |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a89d9da68d5632b6bb701b4d995042446).

## [◆](https://docs.atas.net/en/)News

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.News |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a634e78ed4424dac4d3443b0ee00f17ea).

## [◆](https://docs.atas.net/en/)Orders

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.Orders |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#af566500564a26ab6db8f7cb30e2fabe6).

## [◆](https://docs.atas.net/en/)OvernightSwapValues

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.OvernightSwapValues |
| --- |

get

## [◆](https://docs.atas.net/en/)PortfolioChanges

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.PortfolioChanges |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a4692f57c0115568f46254d13a599eb99).

## [◆](https://docs.atas.net/en/)PortfolioGroups

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.PortfolioGroups |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac63340650493dea57fbd8ab834f05942).

## [◆](https://docs.atas.net/en/)Portfolios

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.Portfolios |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a7f1880c509917a0123aebf9bc5e45b74).

## [◆](https://docs.atas.net/en/)PortfolioStates

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.PortfolioStates |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad945ba967e69ec7d7ecbaf498eed1d9a).

## [◆](https://docs.atas.net/en/)PortfolioViewers

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.PortfolioViewers |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa22089b5dcd9202f4c0398a4a14d9be5).

## [◆](https://docs.atas.net/en/)Positions

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.Positions |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a0381e74a23b505d3f84458f16632b470).

## [◆](https://docs.atas.net/en/)PositionStates

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.PositionStates |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a5b279766f822ce3cc57b9a5757c68273).

## [◆](https://docs.atas.net/en/)Securities

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.Securities |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a67a0ffbf0372c0a3c8b9fb4a7c8c5cc6).

## [◆](https://docs.atas.net/en/)SecurityMargins

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.SecurityMargins |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a97ec44f29bc32553efb2a17b0db4c8ed).

## [◆](https://docs.atas.net/en/)SecurityRoutes

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.SecurityRoutes |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a60e4ef3c88bcbb6a8210f3bef1e76ae8).

## [◆](https://docs.atas.net/en/)ServerPnL

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.ServerPnL |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a9e6af62fceed7304b92a6ff14fe59849).

## [◆](https://docs.atas.net/en/)Settings

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.Settings |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a96b123cc78ede4735996d62622eab3e8).

## [◆](https://docs.atas.net/en/)TradingOptions

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.TradingOptions |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a088db28bd3ab67a275092c3a8385ca77).

## [◆](https://docs.atas.net/en/)TradingOptionsSecurities

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.TradingOptionsSecurities |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a9d3bddfd02a6f0008b721a57c20c90f1).

## [◆](https://docs.atas.net/en/)UserGroups

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.UserGroups |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a11cb8cf39d854b1e465931697242b105).

## [◆](https://docs.atas.net/en/)UserRoleRights

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.UserRoleRights |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a376af4ac0a394b6c77177955ec6dce7a).

## [◆](https://docs.atas.net/en/)UserRoles

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.UserRoles |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac80cbd4698bf0be7c761b7fda13030dd).

## [◆](https://docs.atas.net/en/)Users

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.Users |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad4133e6436811d4738f1a0eb4eb5e575).

## [◆](https://docs.atas.net/en/)WorkingTimes

| IQueryable ATAS.DataFeedsCore.Database.DatabaseManager.WorkingTimes |
| --- |

get

Implements [ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a72b3ffb43cd53c3905cf9fabe95f74a6).

The documentation for this class was generated from the following file:
- [DatabaseManager.cs](../files/DatabaseManager_8cs.md)
