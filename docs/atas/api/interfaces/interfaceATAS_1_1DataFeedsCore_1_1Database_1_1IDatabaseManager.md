# ATAS.DataFeedsCore.Database.IDatabaseManager Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.html

Inheritance diagram for ATAS.DataFeedsCore.Database.IDatabaseManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Database.IDatabaseManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| long? | [GetCommissionGroupByPortfolio](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ae6e221367eaf3f71be807a08b6a32975) (string accountId) |
| | |
| double? | [Initialize](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad6ecb1359f382e1c7f095ee7f9890d3a) () |
| | |
| int | [Delete](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a74673b34004f640ec8c9d5be35676471) (TEntity entity) |
| | |
| void | [BeginTransaction](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa114005e2efec7be0358f0504d927306) () |
| | |
| void | [CommitTransaction](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a64829dc15bfc706ec7adabbeebe39741) () |
| | |
| void | [RollbackTransaction](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#af1c9d562304bd281e2b30bcdd537a447) () |
| | |

| Properties | |
| --- | --- |
| long | [LastExtId](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a78554f7538630612631e7910822f8036)`[get]` |
| | |
| long | [LastOrderId](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa4f689db52838fe3151a2dd7f18f4788)`[get]` |
| | |
| long | [LastTradeId](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a50cc264375f878d60c736d5147b0015b)`[get]` |
| | |
| IQueryable | [Securities](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a67a0ffbf0372c0a3c8b9fb4a7c8c5cc6)`[get]` |
| | |
| IQueryable | [Portfolios](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a7f1880c509917a0123aebf9bc5e45b74)`[get]` |
| | |
| IQueryable | [Positions](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a0381e74a23b505d3f84458f16632b470)`[get]` |
| | |
| IQueryable | [Orders](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#af566500564a26ab6db8f7cb30e2fabe6)`[get]` |
| | |
| IQueryable | [MyTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a89d9da68d5632b6bb701b4d995042446)`[get]` |
| | |
| IQueryable | [Users](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad4133e6436811d4738f1a0eb4eb5e575)`[get]` |
| | |
| IQueryable | [UserRoles](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac80cbd4698bf0be7c761b7fda13030dd)`[get]` |
| | |
| IQueryable | [UserRoleRights](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a376af4ac0a394b6c77177955ec6dce7a)`[get]` |
| | |
| IQueryable | [UserGroups](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a11cb8cf39d854b1e465931697242b105)`[get]` |
| | |
| IQueryable | [GroupExchanges](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac2397bd26eff3375826a4e1764d3f757)`[get]` |
| | |
| IQueryable | [CommissionGroups](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a1d78c9959607138b49ec34d2b3c1bedf)`[get]` |
| | |
| IQueryable | [HistoryMyTrades](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a50bdae307ba17633cd6da4d5f0606193)`[get]` |
| | |
| IQueryable | [Settings](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a96b123cc78ede4735996d62622eab3e8)`[get]` |
| | |
| IQueryable | [Exchanges](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa20cdd15f1045e10488365e6616e99b7)`[get]` |
| | |
| IQueryable | [WorkingTimes](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a72b3ffb43cd53c3905cf9fabe95f74a6)`[get]` |
| | |
| IQueryable | [SecurityMargins](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a97ec44f29bc32553efb2a17b0db4c8ed)`[get]` |
| | |
| IQueryable | [News](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a634e78ed4424dac4d3443b0ee00f17ea)`[get]` |
| | |
| IQueryable | [TradingOptions](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a088db28bd3ab67a275092c3a8385ca77)`[get]` |
| | |
| IQueryable | [TradingOptionsSecurities](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a9d3bddfd02a6f0008b721a57c20c90f1)`[get]` |
| | |
| IQueryable | [CommissionGroupItems](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a7033aff96bda967fa72e9444a4ce63e7)`[get]` |
| | |
| IQueryable | [PortfolioChanges](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a4692f57c0115568f46254d13a599eb99)`[get]` |
| | |
| IQueryable | [PortfolioStates](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ad945ba967e69ec7d7ecbaf498eed1d9a)`[get]` |
| | |
| IQueryable | [PositionStates](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a5b279766f822ce3cc57b9a5757c68273)`[get]` |
| | |
| IQueryable | [SecurityRoutes](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a60e4ef3c88bcbb6a8210f3bef1e76ae8)`[get]` |
| | |
| IQueryable | [PortfolioViewers](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#aa22089b5dcd9202f4c0398a4a14d9be5)`[get]` |
| | |
| IQueryable | [PortfolioGroups](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#ac63340650493dea57fbd8ab834f05942)`[get]` |
| | |
| IQueryable | [ServerPnL](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a9e6af62fceed7304b92a6ff14fe59849)`[get]` |
| | |
| IQueryable | [InstrumentExchanges](./interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md#a151d9b9c2212246026c55bedf88df7e9)`[get]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)BeginTransaction()

| void ATAS.DataFeedsCore.Database.IDatabaseManager.BeginTransaction | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)CommitTransaction()

| void ATAS.DataFeedsCore.Database.IDatabaseManager.CommitTransaction | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Delete()

| int ATAS.DataFeedsCore.Database.IDatabaseManager.Delete | ( | TEntity | entity | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetCommissionGroupByPortfolio()

| long? ATAS.DataFeedsCore.Database.IDatabaseManager.GetCommissionGroupByPortfolio | ( | string | accountId | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#adeacc009b7ecdf4ce70511e603949808).

## [◆](https://docs.atas.net/en/)Initialize()

| double? ATAS.DataFeedsCore.Database.IDatabaseManager.Initialize | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a31a20091aa1dbae700924eead5628d66).

## [◆](https://docs.atas.net/en/)RollbackTransaction()

| void ATAS.DataFeedsCore.Database.IDatabaseManager.RollbackTransaction | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)CommissionGroupItems

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.CommissionGroupItems |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#aa4f242720bcfa10d4341a614b87049bd).

## [◆](https://docs.atas.net/en/)CommissionGroups

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.CommissionGroups |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a57b26409ef0c4b94dcaba669252c1231).

## [◆](https://docs.atas.net/en/)Exchanges

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.Exchanges |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#ab43de0ed18f3c3571532e15469378480).

## [◆](https://docs.atas.net/en/)GroupExchanges

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.GroupExchanges |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a20f9dd1b7b3fc6d344560f38974c2564).

## [◆](https://docs.atas.net/en/)HistoryMyTrades

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.HistoryMyTrades |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a293adec8bfa6f3d4c06d29b3fde86575).

## [◆](https://docs.atas.net/en/)InstrumentExchanges

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.InstrumentExchanges |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a1bbde5c79db460ee88df7b27e3fc0031).

## [◆](https://docs.atas.net/en/)LastExtId

| long ATAS.DataFeedsCore.Database.IDatabaseManager.LastExtId |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a261be598b0be0693385e68a3c2f70441).

## [◆](https://docs.atas.net/en/)LastOrderId

| long ATAS.DataFeedsCore.Database.IDatabaseManager.LastOrderId |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#aab580f8c55402043f597f281a8af3117).

## [◆](https://docs.atas.net/en/)LastTradeId

| long ATAS.DataFeedsCore.Database.IDatabaseManager.LastTradeId |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#acb8c39460f12b6deadd233b0c63cd9b9).

## [◆](https://docs.atas.net/en/)MyTrades

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.MyTrades |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5751df732e044302675bf2b72c779941).

## [◆](https://docs.atas.net/en/)News

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.News |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#ad9360dfda291e897c95eff849e95e617).

## [◆](https://docs.atas.net/en/)Orders

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.Orders |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5e38f19ddd23fb5aa23e4cb37beee9bf).

## [◆](https://docs.atas.net/en/)PortfolioChanges

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.PortfolioChanges |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a89f76c18cf4cd7f78e1514e4d93313d1).

## [◆](https://docs.atas.net/en/)PortfolioGroups

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.PortfolioGroups |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a8981630b481c8244df3264ab1284c9c8).

## [◆](https://docs.atas.net/en/)Portfolios

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.Portfolios |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a21d1fa144139d190b584d25f50de017c).

## [◆](https://docs.atas.net/en/)PortfolioStates

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.PortfolioStates |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#af5cbaffdd2640a102213095eb9aa0c3c).

## [◆](https://docs.atas.net/en/)PortfolioViewers

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.PortfolioViewers |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a678f94198cf10fca8e4e161adced7682).

## [◆](https://docs.atas.net/en/)Positions

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.Positions |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a2b62face029fd6a602f8e766f97a6257).

## [◆](https://docs.atas.net/en/)PositionStates

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.PositionStates |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a73e376ccf1c85d06e06392334dbc9ee4).

## [◆](https://docs.atas.net/en/)Securities

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.Securities |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a60e2dc352583a94186b9d2412169ec60).

## [◆](https://docs.atas.net/en/)SecurityMargins

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.SecurityMargins |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#aa792516c94f036480dfe227a367e1e3f).

## [◆](https://docs.atas.net/en/)SecurityRoutes

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.SecurityRoutes |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5647f6f035228ce85c661f21993a356d).

## [◆](https://docs.atas.net/en/)ServerPnL

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.ServerPnL |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#af626cb488998781436a825270e93af40).

## [◆](https://docs.atas.net/en/)Settings

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.Settings |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#abd864d1c87ded51330b54e4d69f93e9f).

## [◆](https://docs.atas.net/en/)TradingOptions

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.TradingOptions |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a4faa7dd295b1241bc43b74a25f20de0f).

## [◆](https://docs.atas.net/en/)TradingOptionsSecurities

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.TradingOptionsSecurities |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a5c8ba1adc6c7ecf54d51b5ba81343a28).

## [◆](https://docs.atas.net/en/)UserGroups

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.UserGroups |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#af0d28faecd4de3c62dbe32756e6494f5).

## [◆](https://docs.atas.net/en/)UserRoleRights

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.UserRoleRights |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a9fc029b2d6d6eb145f8206e347c479c8).

## [◆](https://docs.atas.net/en/)UserRoles

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.UserRoles |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a36aae166464f43c3b62fd7bd6eda6e75).

## [◆](https://docs.atas.net/en/)Users

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.Users |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#a8c558c607c9912e7be37e1e1b0a1eca0).

## [◆](https://docs.atas.net/en/)WorkingTimes

| IQueryable ATAS.DataFeedsCore.Database.IDatabaseManager.WorkingTimes |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md#ab646720d4ef983c916d8797ea3044205).

The documentation for this interface was generated from the following file:
- [IDatabaseManager.cs](../files/IDatabaseManager_8cs.md)
