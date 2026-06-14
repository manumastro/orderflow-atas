# StopProfit.cs File Reference

Source: https://docs.atas.net/en/StopProfit_8cs.html

| Classes | |
| --- | --- |
| class | [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Strategies](../namespaces/namespaceATAS_1_1Strategies.md) |
| | |
| namespace | [ATAS.Strategies.ATM](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md) |
| | |

| Functions | |
| --- | --- |
| record | [ATAS.Strategies.ATM.StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) (bool IsEnabled, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) [Value](../namespaces/namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fea689202409e48743b914713f96d93947c)) |
| | |
| record | [ATAS.Strategies.ATM.TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) (bool IsEnabled, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) Step, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) [Stop](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8a11a755d598c0c417f9a36758c3da7481)) |
| | |
| record | [ATAS.Strategies.ATM.BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) (bool IsEnabled, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) [Breakeven](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ad4ea3556dcac380180690530f0b0dd2d), [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) Offset) |
| | |
| | [ATAS.Strategies.ATM.SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a24b0b23c7c3c28e4ee013f9fa4a78cab) ([StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) stopLoss, [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) takeProfit, [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) breakeven, [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) trailing, [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? stopOrderId, [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? takeOrderId) |
| | |
| | [ATAS.Strategies.ATM.SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aecc408b3d4f41c9c934e248fd9d6ca09) ([StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) stopLoss, [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) takeProfit, [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) breakeven, [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) trailing) |
| | |
| | [ATAS.Strategies.ATM.SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a2a8c220f6c979ea1b5133b2fbba6c1eb) ([StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) stopLoss, [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) takeProfit, [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) breakeven, [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) trailing, [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? stopOrderId, [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? takeOrderId, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? currentStop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? currentTake, [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) timeInForce) |
| | |
| override string | [ATAS.Strategies.ATM.ToString](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abe2e6e2d35676ab00e3c4e3b5dc53d7c) () |
| | |
| void | [ATAS.Strategies.ATM.Deconstruct](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a9da0bce4ea5996058f2018755e141671) (out [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) StopLoss, out [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) [TakeProfit](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ae8145d44ef690ec05f6bd3c30cbc15be), out [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) [Breakeven](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ad4ea3556dcac380180690530f0b0dd2d), out [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) [Trailing](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a0900eaee65df94074ad6a877427fd20b), out [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? [StopOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a0c6e231b14c3305fa72e0cc604b75e46), out [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? [TakeOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a4b60886051522213e007bac0c93ad9f6), out [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? [CurrentStop](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a313129e7696ab7f51bac247f34a5010b), out [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? [CurrentTake](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aabf9463863df2dd67d3136111d8c79ac), out [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47)) |
| | |

| Variables | |
| --- | --- |
| record | [ATAS.Strategies.ATM.SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) |
| | |
| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | [ATAS.Strategies.ATM.TakeProfit](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ae8145d44ef690ec05f6bd3c30cbc15be)`[get]` |
| | |
| [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | [ATAS.Strategies.ATM.Breakeven](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ad4ea3556dcac380180690530f0b0dd2d)`[get]` |
| | |
| [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | [ATAS.Strategies.ATM.Trailing](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a0900eaee65df94074ad6a877427fd20b)`[get]` |
| | |
| [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | [ATAS.Strategies.ATM.StopOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a0c6e231b14c3305fa72e0cc604b75e46)`[get]` |
| | |
| [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | [ATAS.Strategies.ATM.TakeOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a4b60886051522213e007bac0c93ad9f6)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [ATAS.Strategies.ATM.CurrentStop](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a313129e7696ab7f51bac247f34a5010b)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [ATAS.Strategies.ATM.CurrentTake](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aabf9463863df2dd67d3136111d8c79ac)`[get]` |
| | |
