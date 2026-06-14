# ATAS.DataFeedsCore.ISecurityPositionManager Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.html

Inheritance diagram for ATAS.DataFeedsCore.ISecurityPositionManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Clear](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#aa788c88f7c0fe73e08968a356104e271) () |
| | |
| bool | [GetIsChanged](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#aaf94626cdfb3f1e6c5f1d628d442aa76) () |
| | |
| bool | [GetIsNeedSubscribeLevel1](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a8adaf5e0f99b2b659240fcc9f6d3ccac) () |
| | |
| bool | [Update](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#ab46e89a6ce57b556abf41c6836247460) (decimal? volume=null, decimal? averagePrice=null, decimal? openedPnL=null, decimal? closedPnL=null, decimal? commission=null, decimal? openVolume=null, [RiskInfo](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a5782229ea6bf214ef71162db25fbfd1b)? risk=null) |
| | |
| bool | [UpdateAveragePriceByTrades](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#ad4ebdb474daeac4f5532e97c411acadd) () |
| | |
| bool | [UpdateOpenPnL](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a414f2eee075544dc614867368f671a59) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) type, decimal price) |
| | |
| bool | [Process](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a42132de7dd0cbc3165c49181d92aa4cd) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| bool | [Process](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a6102aa5022413a1019021329122692f9) ([MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [SetAveragePrice](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a1e93414fc009625f86c5d2377dda7fe1) (decimal avgPrice) |
| | |

| Properties | |
| --- | --- |
| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | [Position](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a6a41c5be952b9724a0aad0fe5ead16f9)`[get]` |
| | |
| bool | [IsPositionInitialized](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md#a26fdcd75ca24cf7341aedbcaa6c1f315)`[get]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void ATAS.DataFeedsCore.ISecurityPositionManager.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a09766360c8fd0f393da8365fd86a293a).

## [◆](https://docs.atas.net/en/)GetIsChanged()

| bool ATAS.DataFeedsCore.ISecurityPositionManager.GetIsChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a98fa8bafd5db5885437f1d3b8f40a257).

## [◆](https://docs.atas.net/en/)GetIsNeedSubscribeLevel1()

| bool ATAS.DataFeedsCore.ISecurityPositionManager.GetIsNeedSubscribeLevel1 | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a79e8d355173aba1111da77baf1dae15d).

## [◆](https://docs.atas.net/en/)Process() [1/2]

| bool ATAS.DataFeedsCore.ISecurityPositionManager.Process | ( | [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a8ecbdc3ebe5804db5a62a2d86b6ffaed).

## [◆](https://docs.atas.net/en/)Process() [2/2]

| bool ATAS.DataFeedsCore.ISecurityPositionManager.Process | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a529f8228323e2ac8ff44fb19565ab049).

## [◆](https://docs.atas.net/en/)SetAveragePrice()

| void ATAS.DataFeedsCore.ISecurityPositionManager.SetAveragePrice | ( | decimal | avgPrice | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a565f65209c79085f21bc77fccfbc5b7e).

## [◆](https://docs.atas.net/en/)Update()

| bool ATAS.DataFeedsCore.ISecurityPositionManager.Update | ( | decimal? | volume = `null`, |
| --- | --- | --- | --- |
| | | decimal? | averagePrice = `null`, |
| | | decimal? | openedPnL = `null`, |
| | | decimal? | closedPnL = `null`, |
| | | decimal? | commission = `null`, |
| | | decimal? | openVolume = `null`, |
| | | [RiskInfo](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a5782229ea6bf214ef71162db25fbfd1b)? | risk = `null` |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#aecbf50ef5506037052d181e4ad199cee).

## [◆](https://docs.atas.net/en/)UpdateAveragePriceByTrades()

| bool ATAS.DataFeedsCore.ISecurityPositionManager.UpdateAveragePriceByTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a0d88276362b9f172f3b477af1d94b5d5).

## [◆](https://docs.atas.net/en/)UpdateOpenPnL()

| bool ATAS.DataFeedsCore.ISecurityPositionManager.UpdateOpenPnL | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) | type, |
| | | decimal | price |
| | ) | | |

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a810914997353faf6ff385044926791f2).

## Property Documentation

## [◆](https://docs.atas.net/en/)IsPositionInitialized

| bool ATAS.DataFeedsCore.ISecurityPositionManager.IsPositionInitialized |
| --- |

get

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#ae3d7d1dfea4f0f87ddda013602dae342).

## [◆](https://docs.atas.net/en/)Position

| [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.DataFeedsCore.ISecurityPositionManager.Position |
| --- |

get

Implemented in [ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md#a776a6789288c51c29f5c3da3e311b8b9).

The documentation for this interface was generated from the following file:
- [SecurityPositionManager.cs](../files/SecurityPositionManager_8cs.md)
