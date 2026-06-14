# ATAS.Strategies.ATM Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1Strategies_1_1ATM.html

| Classes | |
| --- | --- |
| class | [ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) |
| | |
| class | [BaseStopProfitStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) |
| | |
| struct | [ChangesInfo](../structs/structATAS_1_1Strategies_1_1ATM_1_1ChangesInfo.md) |
| | |
| class | Extensions |
| | |
| interface | [IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) |
| | |
| interface | [ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md) |
| | |
| interface | [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) |
| | |
| interface | [IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) |
| | |
| interface | [IStrategyMarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) |
| | |
| interface | [ISupportCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md) |
| | |
| class | [StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [StopTakeValueTypes](./namespaceATAS_1_1Strategies_1_1ATM.md#a6718d521da9fbde3a8d25d79d9aaa269) { [Ticks](./namespaceATAS_1_1Strategies_1_1ATM.md#a6718d521da9fbde3a8d25d79d9aaa269a7ad0bde3d601cd3c0eaa8eb5bf27cb1a)
, [Percent](./namespaceATAS_1_1Strategies_1_1ATM.md#a6718d521da9fbde3a8d25d79d9aaa269aadaaee4b22041c27198d410c68d952c9)
 } |
| | |

| Functions | |
| --- | --- |
| readonly record struct | [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c) (long ExtId, string Id) |
| | |
| | [BaseStopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a94d54655c7ac364b1ffa2ed8cc1adadb) ([TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) timeInForce) |
| | |
| void | [Deconstruct](./namespaceATAS_1_1Strategies_1_1ATM.md#a77cecf75378f52cd75215db241e71a18) (out [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47)) |
| | |
| record | [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) (bool IsEnabled, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) [Value](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fea689202409e48743b914713f96d93947c)) |
| | |
| record | [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) (bool IsEnabled, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) Step, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) [Stop](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8a11a755d598c0c417f9a36758c3da7481)) |
| | |
| record | [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) (bool IsEnabled, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) [Breakeven](./namespaceATAS_1_1Strategies_1_1ATM.md#ad4ea3556dcac380180690530f0b0dd2d), [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) Offset) |
| | |
| | [SimpleStopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a24b0b23c7c3c28e4ee013f9fa4a78cab) ([StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) stopLoss, [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) takeProfit, [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) breakeven, [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) trailing, [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? stopOrderId, [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? takeOrderId) |
| | |
| | [SimpleStopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#aecc408b3d4f41c9c934e248fd9d6ca09) ([StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) stopLoss, [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) takeProfit, [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) breakeven, [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) trailing) |
| | |
| | [SimpleStopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a2a8c220f6c979ea1b5133b2fbba6c1eb) ([StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) stopLoss, [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) takeProfit, [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) breakeven, [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) trailing, [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? stopOrderId, [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? takeOrderId, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? currentStop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? currentTake, [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) timeInForce) |
| | |
| override string | [ToString](./namespaceATAS_1_1Strategies_1_1ATM.md#abe2e6e2d35676ab00e3c4e3b5dc53d7c) () |
| | |
| void | [Deconstruct](./namespaceATAS_1_1Strategies_1_1ATM.md#a9da0bce4ea5996058f2018755e141671) (out [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) StopLoss, out [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) [TakeProfit](./namespaceATAS_1_1Strategies_1_1ATM.md#ae8145d44ef690ec05f6bd3c30cbc15be), out [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) [Breakeven](./namespaceATAS_1_1Strategies_1_1ATM.md#ad4ea3556dcac380180690530f0b0dd2d), out [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) [Trailing](./namespaceATAS_1_1Strategies_1_1ATM.md#a0900eaee65df94074ad6a877427fd20b), out [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? [StopOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#a0c6e231b14c3305fa72e0cc604b75e46), out [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? [TakeOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#a4b60886051522213e007bac0c93ad9f6), out [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? [CurrentStop](./namespaceATAS_1_1Strategies_1_1ATM.md#a313129e7696ab7f51bac247f34a5010b), out [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? [CurrentTake](./namespaceATAS_1_1Strategies_1_1ATM.md#aabf9463863df2dd67d3136111d8c79ac), out [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47)) |
| | |

| Variables | |
| --- | --- |
| record | [BaseStopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a608d641b715fc737c8f6d1857662bbf6) |
| | |
| | [init](./namespaceATAS_1_1Strategies_1_1ATM.md#a31894a5666eb43047918933766b7f4e4) |
| | |
| record | [SimpleStopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) |
| | |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)StopTakeValueTypes

| enum [ATAS.Strategies.ATM.StopTakeValueTypes](./namespaceATAS_1_1Strategies_1_1ATM.md#a6718d521da9fbde3a8d25d79d9aaa269) |
| --- |

| Enumerator | |
| --- | --- |
| Ticks | |
| Percent | |

## Function Documentation

## [◆](https://docs.atas.net/en/)BaseStopProfitSettings()

| ATAS.Strategies.ATM.BaseStopProfitSettings | ( | [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) | timeInForce | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)BreakevenSettings()

| record ATAS.Strategies.ATM.BreakevenSettings | ( | bool | IsEnabled, |
| --- | --- | --- | --- |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | Breakeven, |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | Offset |
| | ) | | |

## [◆](https://docs.atas.net/en/)Deconstruct() [1/2]

| void ATAS.Strategies.ATM.Deconstruct | ( | out [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | StopLoss, |
| --- | --- | --- | --- |
| | | out [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | TakeProfit, |
| | | out [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | Breakeven, |
| | | out [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | Trailing, |
| | | out [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | StopOrderId, |
| | | out [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | TakeOrderId, |
| | | out [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | CurrentStop, |
| | | out [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | CurrentTake, |
| | | out [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) | TimeInForce |
| | ) | | |

## [◆](https://docs.atas.net/en/)Deconstruct() [2/2]

| void ATAS.Strategies.ATM.Deconstruct | ( | out [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) | TimeInForce | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)SimpleStopProfitSettings() [1/3]

| ATAS.Strategies.ATM.SimpleStopProfitSettings | ( | [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | stopLoss, |
| --- | --- | --- | --- |
| | | [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | takeProfit, |
| | | [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | breakeven, |
| | | [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | trailing |
| | ) | | |

## [◆](https://docs.atas.net/en/)SimpleStopProfitSettings() [2/3]

| ATAS.Strategies.ATM.SimpleStopProfitSettings | ( | [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | stopLoss, |
| --- | --- | --- | --- |
| | | [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | takeProfit, |
| | | [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | breakeven, |
| | | [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | trailing, |
| | | [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | stopOrderId, |
| | | [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | takeOrderId |
| | ) | | |

## [◆](https://docs.atas.net/en/)SimpleStopProfitSettings() [3/3]

| ATAS.Strategies.ATM.SimpleStopProfitSettings | ( | [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | stopLoss, |
| --- | --- | --- | --- |
| | | [StopProfitSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | takeProfit, |
| | | [BreakevenSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | breakeven, |
| | | [TrailingStopSettings](./namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | trailing, |
| | | [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | stopOrderId, |
| | | [StrategyOrderId](./namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | takeOrderId, |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | currentStop, |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | currentTake, |
| | | [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) | timeInForce |
| | ) | | |

## [◆](https://docs.atas.net/en/)StopProfitSettings()

| record ATAS.Strategies.ATM.StopProfitSettings | ( | bool | IsEnabled, |
| --- | --- | --- | --- |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | Value |
| | ) | | |

## [◆](https://docs.atas.net/en/)StrategyOrderId()

| readonly record struct ATAS.Strategies.ATM.StrategyOrderId | ( | long | ExtId, |
| --- | --- | --- | --- |
| | | string | Id |
| | ) | | |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.Strategies.ATM.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)TrailingStopSettings()

| record ATAS.Strategies.ATM.TrailingStopSettings | ( | bool | IsEnabled, |
| --- | --- | --- | --- |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | Step, |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | Stop |
| | ) | | |

## Variable Documentation

## [◆](https://docs.atas.net/en/)BaseStopProfitSettings

| record ATAS.Strategies.ATM.BaseStopProfitSettings |
| --- |

## [◆](https://docs.atas.net/en/)init

| ATAS.Strategies.ATM.init |
| --- |

## [◆](https://docs.atas.net/en/)SimpleStopProfitSettings

| record ATAS.Strategies.ATM.SimpleStopProfitSettings |
| --- |
