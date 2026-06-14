# ATAS.DataFeedsCore.SecurityPositionManager Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.html

Inheritance diagram for ATAS.DataFeedsCore.SecurityPositionManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.SecurityPositionManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [SecurityPositionManager](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#ac25f092fd8cf3519373543bb7f686a3a) (ILoggerSource logger, [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a09766360c8fd0f393da8365fd86a293a) () |
| | |
| bool | [GetIsChanged](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a98fa8bafd5db5885437f1d3b8f40a257) () |
| | |
| bool | [GetIsNeedSubscribeLevel1](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a79e8d355173aba1111da77baf1dae15d) () |
| | |
| bool | [Update](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#aecbf50ef5506037052d181e4ad199cee) (decimal? volume=null, decimal? averagePrice=null, decimal? openedPnL=null, decimal? closedPnL=null, decimal? commission=null, decimal? openVolume=null, [RiskInfo](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a5782229ea6bf214ef71162db25fbfd1b)? risk=null) |
| | |
| bool | [UpdateAveragePriceByTrades](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a0d88276362b9f172f3b477af1d94b5d5) () |
| | |
| bool | [UpdateOpenPnL](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a810914997353faf6ff385044926791f2) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) type, decimal price) |
| | |
| bool | [Process](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a529f8228323e2ac8ff44fb19565ab049) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| bool | [Process](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a8ecbdc3ebe5804db5a62a2d86b6ffaed) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [SetAveragePrice](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a565f65209c79085f21bc77fccfbc5b7e) (decimal avgPrice) |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#aa788c88f7c0fe73e08968a356104e271) () |
| | |
| bool | [GetIsChanged](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#aaf94626cdfb3f1e6c5f1d628d442aa76) () |
| | |
| bool | [GetIsNeedSubscribeLevel1](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a8adaf5e0f99b2b659240fcc9f6d3ccac) () |
| | |
| bool | [Update](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#ab46e89a6ce57b556abf41c6836247460) (decimal? volume=null, decimal? averagePrice=null, decimal? openedPnL=null, decimal? closedPnL=null, decimal? commission=null, decimal? openVolume=null, [RiskInfo](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a5782229ea6bf214ef71162db25fbfd1b)? risk=null) |
| | |
| bool | [UpdateAveragePriceByTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#ad4ebdb474daeac4f5532e97c411acadd) () |
| | |
| bool | [UpdateOpenPnL](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a414f2eee075544dc614867368f671a59) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) type, decimal price) |
| | |
| bool | [Process](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a42132de7dd0cbc3165c49181d92aa4cd) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| bool | [Process](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a6102aa5022413a1019021329122692f9) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [SetAveragePrice](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a1e93414fc009625f86c5d2377dda7fe1) (decimal avgPrice) |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual bool | [UpdateUnrealizedPnl](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a392f20a4ca330b1c144e28696e542d2b) ([MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) type, decimal price) |
| | |
| virtual decimal | [GetPnlMultiplier](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a266090af8c1491f61e6db6049bab9398) () |
| | |

| Properties | |
| --- | --- |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [Position](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a776a6789288c51c29f5c3da3e311b8b9)`[get]` |
| | |
| bool | [IsPositionInitialized](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#ae3d7d1dfea4f0f87ddda013602dae342)`[get]` |
| | |
| [PositionAveragePriceValueTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) | [AveragePriceValueType](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a367cde12c209a7618745d6df5b476627)`[get, set]` |
| | |
| decimal | [AveragePrice](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a57d043b11352b1e686fb7f8eb4324299)`[get, set]` |
| | |
| decimal | [Volume](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#af34a24a380ef8dbe69c4dbb1225a4874)`[get, set]` |
| | |
| bool | [CalculateVolume](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a78ce21aa6d1018613ee6ea55b421937e)`[get, set]` |
| | |
| bool | [CalculateAveragePrice](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a25ee0ea62ce434def2ccfdef7c0bc15a)`[get, set]` |
| | |
| bool | [CalculateOpenedPnL](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a26f1e204414cfd526ccac8815997d1bd)`[get, set]` |
| | |
| bool | [CalculateClosedPnL](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a328705dcf5a930a522f65d804e9b1903)`[get, set]` |
| | |
| bool | [AllowSubscribeLevel1](./classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#aee9c16b1f1414cbbf400feb2c6501371)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md) | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [Position](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a6a41c5be952b9724a0aad0fe5ead16f9)`[get]` |
| | |
| bool | [IsPositionInitialized](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a26fdcd75ca24cf7341aedbcaa6c1f315)`[get]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SecurityPositionManager()

| ATAS.DataFeedsCore.SecurityPositionManager.SecurityPositionManager | ( | ILoggerSource | logger, |
| --- | --- | --- | --- |
| | | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void ATAS.DataFeedsCore.SecurityPositionManager.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#aa788c88f7c0fe73e08968a356104e271).

## [◆](https://docs.atas.net/en/)GetIsChanged()

| bool ATAS.DataFeedsCore.SecurityPositionManager.GetIsChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#aaf94626cdfb3f1e6c5f1d628d442aa76).

## [◆](https://docs.atas.net/en/)GetIsNeedSubscribeLevel1()

| bool ATAS.DataFeedsCore.SecurityPositionManager.GetIsNeedSubscribeLevel1 | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a8adaf5e0f99b2b659240fcc9f6d3ccac).

## [◆](https://docs.atas.net/en/)GetPnlMultiplier()

| virtual decimal ATAS.DataFeedsCore.SecurityPositionManager.GetPnlMultiplier | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)Process() [1/2]

| bool ATAS.DataFeedsCore.SecurityPositionManager.Process | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a6102aa5022413a1019021329122692f9).

## [◆](https://docs.atas.net/en/)Process() [2/2]

| bool ATAS.DataFeedsCore.SecurityPositionManager.Process | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a42132de7dd0cbc3165c49181d92aa4cd).

## [◆](https://docs.atas.net/en/)SetAveragePrice()

| void ATAS.DataFeedsCore.SecurityPositionManager.SetAveragePrice | ( | decimal | avgPrice | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a1e93414fc009625f86c5d2377dda7fe1).

## [◆](https://docs.atas.net/en/)Update()

| bool ATAS.DataFeedsCore.SecurityPositionManager.Update | ( | decimal? | volume = `null`, |
| --- | --- | --- | --- |
| | | decimal? | averagePrice = `null`, |
| | | decimal? | openedPnL = `null`, |
| | | decimal? | closedPnL = `null`, |
| | | decimal? | commission = `null`, |
| | | decimal? | openVolume = `null`, |
| | | [RiskInfo](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a5782229ea6bf214ef71162db25fbfd1b)? | risk = `null` |
| | ) | | |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#ab46e89a6ce57b556abf41c6836247460).

## [◆](https://docs.atas.net/en/)UpdateAveragePriceByTrades()

| bool ATAS.DataFeedsCore.SecurityPositionManager.UpdateAveragePriceByTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#ad4ebdb474daeac4f5532e97c411acadd).

## [◆](https://docs.atas.net/en/)UpdateOpenPnL()

| bool ATAS.DataFeedsCore.SecurityPositionManager.UpdateOpenPnL | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) | type, |
| | | decimal | price |
| | ) | | |

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a414f2eee075544dc614867368f671a59).

## [◆](https://docs.atas.net/en/)UpdateUnrealizedPnl()

| virtual bool ATAS.DataFeedsCore.SecurityPositionManager.UpdateUnrealizedPnl | ( | [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) | type, |
| --- | --- | --- | --- |
| | | decimal | price |
| | ) | | |

protectedvirtual

## Property Documentation

## [◆](https://docs.atas.net/en/)AllowSubscribeLevel1

| bool ATAS.DataFeedsCore.SecurityPositionManager.AllowSubscribeLevel1 |
| --- |

getset

## [◆](https://docs.atas.net/en/)AveragePrice

| decimal ATAS.DataFeedsCore.SecurityPositionManager.AveragePrice |
| --- |

getset

## [◆](https://docs.atas.net/en/)AveragePriceValueType

| [PositionAveragePriceValueTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) ATAS.DataFeedsCore.SecurityPositionManager.AveragePriceValueType |
| --- |

getset

## [◆](https://docs.atas.net/en/)CalculateAveragePrice

| bool ATAS.DataFeedsCore.SecurityPositionManager.CalculateAveragePrice |
| --- |

getset

## [◆](https://docs.atas.net/en/)CalculateClosedPnL

| bool ATAS.DataFeedsCore.SecurityPositionManager.CalculateClosedPnL |
| --- |

getset

## [◆](https://docs.atas.net/en/)CalculateOpenedPnL

| bool ATAS.DataFeedsCore.SecurityPositionManager.CalculateOpenedPnL |
| --- |

getset

## [◆](https://docs.atas.net/en/)CalculateVolume

| bool ATAS.DataFeedsCore.SecurityPositionManager.CalculateVolume |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsPositionInitialized

| bool ATAS.DataFeedsCore.SecurityPositionManager.IsPositionInitialized |
| --- |

get

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a26fdcd75ca24cf7341aedbcaa6c1f315).

## [◆](https://docs.atas.net/en/)Position

| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.DataFeedsCore.SecurityPositionManager.Position |
| --- |

get

Implements [ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a6a41c5be952b9724a0aad0fe5ead16f9).

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.SecurityPositionManager.Volume |
| --- |

getset

The documentation for this class was generated from the following file:
- [SecurityPositionManager.cs](../files/SecurityPositionManager_8cs.md)
