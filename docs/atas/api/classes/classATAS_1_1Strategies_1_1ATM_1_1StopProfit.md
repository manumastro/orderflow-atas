# ATAS.Strategies.ATM.StopProfit Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.html

Inheritance diagram for ATAS.Strategies.ATM.StopProfit:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.ATM.StopProfit:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a5fc5897aa5471096971d0d2f7ad48323) () |
| | |
| bool | [CanSetCustomStop](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a4e88dfcea8b04f136947b6f11ee545ff) () |
| | |
| bool | [CanSetCustomTake](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#abf522ce86500db448ee000e491a8f5dc) () |
| | |
| async Task | [SetCustomStopOrTake](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a8e90a80d6dd79d21d014377542d921ef) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? stop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? take) |
| | |
| [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [GetSettingsWithStopOrTake](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a1708c737bba043a9dfa7f4555a6f5e29) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? stop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? take) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| void | [SetSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#af7d303bfb2a7d3d2f1e0fb06df781280) (TSettings settings) |
| | |
| new TSettings | [GetSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adac77a8844a40163402068af1f951bc3) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa87824f6745908767e037c80afee3c0d) (TSettings settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.ATM.IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| void | [SetSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#aba182152ff29e62b10b2b57143bcd294) (TSettings settings) |
| | |
| new TSettings | [GetSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#a52a15391512ff63a67196251bddf7e01) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#ac0b1f6d0061d5289a16df3bb650e26f4) (TSettings settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |
| bool | [CanSetCustomStop](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#a7ba23a96d1284f5cae81c6df2f53a9f6) () |
| | |
| bool | [CanSetCustomTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#aa57753f309d3d398e8617a1450e835fe) () |
| | |
| Task | [SetCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#a5e0ad2f9ccf5d6b546e4f28d6a8fce47) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? stop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? take) |
| | |
| [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [GetSettingsWithStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#ad1bb4f174d43573224eb055a9d43f036) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? stop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? take) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| Task | [WatchAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a9dc8c3245862a75b1008b570d6049cbb) () |
| | |
| Task | [StartFromWatchAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a6bff2153a446d534eca34647847a7254) () |
| | |
| Task | [RetryAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a9eb844d472ffbfa741dbb9a558cf2b02) () |
| | |
| Task | [CancelAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a8137f7e6fd51d83d38dfe7bd49509e53) () |
| | |
| Task | [ResetOrdersAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a87cfee45d0440ce36c78aca4d4114343) () |
| | |
| bool | [IsStopLoss](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#ac2f851e7a9476d63c9e9ef4bee2a9fef) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| bool | [IsTakeProfit](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a2b432249201d6e3740711800541cc2a5) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| Task | [OpenOrderAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a1c374ec5fe1701a86fa954474097f720) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | |
| Task | [ModifyOrderAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a3494b0a78fd62d7bc19433f0dfb926f5) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool isAutomated=true) |
| | |
| Task | [CancelOrderAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#ac46b3f21a23d9d5ebf2335585f97e491) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | |
| Task | [CancelOrdersAsync](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a1a60092975f93f4b2f655bcb56e9fa1d) (IEnumerable orders) |
| | |
| [IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | [Clone](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a349959c2b6b710a8d3c2123fe0e961a7) (bool cloneOrders=true) |
| | |
| void | [SetSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a37af4a4fe9af5c4760cb1ddfb13f77cf) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings) |
| | |
| [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [GetSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#aaad791303b09343ea759213efbd8fabe) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#acb488db17ab5f60e367141abf559c123) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| Task | [StartAsync](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a8b8803a4d0eff2da7088e4f0b3b4d25a) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| Task | [StopAsync](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a83d83423aa3f8a5248d681b4bb5126ad) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
| | |

| Protected Member Functions | |
| --- | --- |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) [StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) | [CreateNew](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a4a7b78c631081f047228d1e476ba1721) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [CommitChanges](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#abf1625602bef1730307417346263b3e2) ([StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) model, bool cloneOrders) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) [SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) | [OnGetSettings](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a8e0c6ee279981323d8c170067955e43b) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnSetSettings](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a3646ffdbfb3a9cb513cf92f27221df75) ([SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) settings) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) ICollection | [OnIsValidSettings](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a00b3f14f4a35e83a2e7ebabde2779f03) ([SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) settings, decimal positionVolume, decimal positionPrice, decimal currentPrice) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnResetStopTakeOrderId](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#afc83dab926c7a466bea03110ccb8b5c2) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnResetCustomStopAndTakePrices](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a1d39651f85f66e4fb0811e1918885b9c) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [DisableStopAndTake](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ac4005f2a51370fbf838ce1db36a654e1) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnAttachStopAndTakeOrdersFailed](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a05956d25133e388cc69e75971c79d0cc) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnProcessOrder](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#acb1000e0b8934efc56e586de28d87027) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnCancelAll](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ac2099b29f4cf2a322e918849f4eb9f91) (bool retryOnError) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnCancelSecondOrder](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ad125e81efe1b4f931afe79d94854a48b) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool | [SetStopOrTakeCancelledManually](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a366c31ee5a06417b16c0bdad6c40dbc1) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string type) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool | [SetStopOrTakeCustomPrice](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a7c376d50314cda06ca7232475bdd2aae) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) currentStopOrTake, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool | [IsStopOrder](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ad43a04e1140797c8149eb86d84e6d51a) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool | [IsTakeOrder](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a78d70c5d32ec2d69a1303b4804541190) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnProcess](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a5564e656c1d88ef0d6ad9ae601480258) ([ChangesInfo](../structs/structATAS_1_1Strategies_1_1ATM_1_1ChangesInfo.md) info) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnCurrentPositionChanged](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ad5c2a1004aab8390dde97ffe1024e320) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnPnLChanged](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a574fadfbcec21ce8496e1d845f4aea07) (int ticks) |
| | |
| - Protected Member Functions inherited from [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| async Task | [ProcessAfterDelay](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adcc7367e8f6cb591e2285ae17cf47ec8) (TimeSpan? delay=null) |
| | |
| void | [Process](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa411b40ab2c749b1449d02466fb4911a) (bool isPositionChanged=false, bool isOrdersChanged=false, bool isSettingsChanged=false) |
| | |
| decimal | [GetPriceValue](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a3211545827e47b6ccc69267e20ae203d) (decimal price, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) value, decimal sign) |
| | |
| bool | [IsEquals](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a6535900ee13798d776fb715cd275060b) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) first, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) second, bool isStop, bool comparePrices) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [TryAttachOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ad42988cfa3125f3ad09354c0a19f5c5b) (string type, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, StrategyOrderId orderId, string prefix) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [CreateStopOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a23ca88a926987d652829b35e19722e14) ([OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal sl, decimal volume, string prefix) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [CreateTakeOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adfe8cc6e252f082bdf7f1a26643cca03) ([OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, decimal tp, decimal volume, string prefix) |
| | |
| async Task | [TryCancelStopOrTake](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a6f793d057a22863198486a15c9a7ae0f) (string type, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string message, string prefix, bool retryOnError) |
| | |
| async Task | [TryProcessStopOrTake](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a74508820da806f953d4d7ebd976bcf75) (string type, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) oldOrder, [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) direction, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? currentValue, int positionSign, decimal volume, string prefix, bool isPositionChanged) |
| | |
| async Task | [TryCancelSecondOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a475aac2b0f35b649836c65c71ba1f3d2) (string type, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string prefix) |
| | |
| void | [ResetCustomStopAndTakePrices](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a9ad6a9cd92f62539477f107ce8c57810) () |
| | |
| void | [ResetWatchState](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#af4eda0b53e07e2d42bfcc887cd139ada) () |
| | |
| virtual void | [OnProcessOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a6766b0d5c676619c2d75e81233243643) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| abstract Task | [OnCancelAll](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a2f76531d0ebb3032543eae4176905df9) (bool retryOnError) |
| | |
| abstract Task | [OnCancelSecondOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a0f55ef8d4d43eb8a1050b3ee846e7131) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| abstract Task | [OnProcess](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a7f80de1b44d30be5cce0b69b0a9d26cf) ([ChangesInfo](../structs/structATAS_1_1Strategies_1_1ATM_1_1ChangesInfo.md) info) |
| | |
| abstract void | [OnResetCustomStopAndTakePrices](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1e137321d1783880ed1552da0a82c7c0) () |
| | |
| abstract void | [OnResetStopTakeOrderId](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa7500ad1f7fa28c83db0f8061af3f308) () |
| | |
| abstract void | [DisableStopAndTake](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ad75d314bb1018ddf9578b9f669f2bf5a) () |
| | |
| abstract void | [OnAttachStopAndTakeOrdersFailed](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ac0a22925136000c437511c5cac64f2f8) () |
| | |
| abstract bool | [SetStopOrTakeCustomPrice](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#afbb7ab3c482969129d24ed5ed9043a9a) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) currentStopOrTake, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder) |
| | |
| abstract bool | [SetStopOrTakeCancelledManually](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a0c0d60cecdeaa6044ff9b9214d71021a) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string type) |
| | |
| abstract void | [OnSetSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1d7bdf44972b38abe09656b27c517882) (TSettings settings) |
| | |
| abstract TSettings | [OnGetSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1686e830263d34353c4786ceb8721aef) () |
| | |
| abstract ICollection | [OnIsValidSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a121ce447a5240c84aacbd6b798b9cf7a) (TSettings settings, decimal positionVolume, decimal positionPrice, decimal currentPrice) |
| | |
| override async Task | [OnStarted](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a693a5e82857944e5b9bb85579e43fe00) () |
| | Called when the strategy is started from StrategyStates.Stopped state. |
| | |
| override async Task | [OnStartedFromWatch](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adf3fb846efbee406351f0e55131939f1) () |
| | Called when the strategy is started from StrategyStates.Watch state. |
| | |
| override async Task | [OnStopping](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a632892c7ccee633506067a6ff11fdaff) () |
| | Called when the strategy is stopping. |
| | |
| override Task | [OnRetry](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a4c08efaeaff121d7a46250487290b61c) () |
| | |
| override async Task | [OnCancel](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a7c07155c38d002c3c6c96b074a39fed6) () |
| | |
| override Task | [OnResetOrders](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a6ee94f5be8dfd99d1c016ed8779fedd5) () |
| | |
| override void | [OnCurrentPositionChanged](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a2520687829c28c320d925427f6b25a5d) () |
| | Called when the volume of the current position changes. |
| | |
| override async Task | [OnModifyOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a899f91073ddab1029d5a5706cb20cdaf) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool isAutomated) |
| | Called when an existing order is modified. |
| | |
| override async Task | [OnCancelOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ad239e5d34d8d75ad5c63276c19364e89) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated) |
| | Called when an order is canceled. |
| | |
| override async Task | [OnCancelOrdersAsync](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a998b27051154eb391d28fc5b84c40dca) (IEnumerable orders) |
| | |
| override void | [OnNewOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa36961b5dcd5d13474f766ed0c953f54) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when a new order is added. |
| | |
| override void | [OnOrderChanged](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a25c70530bf4bddf14a72b84c53ecb6bb) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when an existing order is changed. |
| | |
| override void | [OnSetStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a99c1bd67812932e6b706ee3b36aff855) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings) |
| | |
| override [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [OnGetStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1614a714474c1be81ba76fc19d42265f) () |
| | |
| | [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) (bool IsValid, IEnumerable Errors) OnIsValidStopProfitSettings([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings |
| | |

| Properties | |
| --- | --- |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool | [IsStopOrderAttached](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a573b6617c1a868802faaba076c66cd77)`[get]` |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool | [IsTakeOrderAttached](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a4936b160eb33cc8fe686ad960a6c5e89)`[get]` |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool | [HasActiveOrders](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ac3117428a29fa34ba4581cc51133269b)`[get]` |
| | |
| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | [TakeProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a3dafbf345ca91c06b9869722b4cd3902)`[get]` |
| | |
| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | [StopLoss](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a33c18ded70157d6a2277732915ce44cb)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [CurrentStop](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#aa2983263a39f368f1862c77b5124fd8d)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [CurrentTake](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#aa740ee89a8e0082e4331aecdad268918)`[get]` |
| | |
| [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | [StopOrderId](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#aca5fc2c3e0d166bdee29a3043d7ed118)`[get]` |
| | |
| [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? | [TakeOrderId](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a13b408842720eaf806e0f8781f5f24be)`[get]` |
| | |
| [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | [Breakeven](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#acd27147ad61b16c1d2992767e6a6b180)`[get, set]` |
| | |
| [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | [TrailingStop](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a1a63e4fa9807ba417c36ad1cb2c94162)`[get, set]` |
| | |
| - Properties inherited from [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| abstract bool | [IsStopOrderAttached](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a90b76ae5119ca29818c9e619ed3e40ed)`[get]` |
| | |
| abstract bool | [IsTakeOrderAttached](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#af1df69fa601ea3279a7f75699674cf19)`[get]` |
| | |
| TimeSpan | [ProcessTimeout](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a42b7b643753c83c3c7127955a7a20d27)`[get, set]` |
| | |
| TimeSpan | [AttachOrdersPeriod](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#abfce3ceceaa43cf42532d61365417ef5)`[get]` |
| | |
| TimeSpan | [RetryPeriodOnConnectionError](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ada5edb3d24932d09a63dbbd324dc2c9c)`[get]` |
| | |
| TimeSpan | [RetryPeriodOnOrderError](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a0d605e4a8fc002f1e456d94d78edddaf)`[get]` |
| | |
| TimeSpan | [RetryPeriod](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a945304704f124ca43f31fc27490686a7)`[get]` |
| | |
| - Properties inherited from [ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md) | |
| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | [StopLoss](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a3ac92a36baf47dbacb8e01f86e2e9fd5)`[get]` |
| | |
| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | [TakeProfit](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#adf0573e46657ed8b00537498f00292cb)`[get]` |
| | |
| [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | [TrailingStop](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#adeeb97f671510a5149e78541b43a7503)`[get]` |
| | |
| [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | [Breakeven](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#aa1ba3f27064a57382d5bbae65af7b116)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [CurrentStop](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a6c25d90f23cfdf8ef91b02513d5c9925)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [CurrentTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a121ba7d376d8cb6c78ecd02fbe4f66a8)`[get]` |
| | |
| - Properties inherited from [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| bool | [HasActiveOrders](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#acc870efe054da5d275b892403d2be88b)`[get]` |
| | |
| [IStrategyMarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) | [MarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#ae73409342feaf2a34cea866bc43f8f1a)`[get, set]` |
| | |
| - Properties inherited from [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| string | [Name](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a82fa08d68c09e1ac4b699ed5901408c3)`[get, set]` |
| | Gets or sets the name of the strategy. |
| | |
| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) | [State](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a18fcdb4e7e4a8d94e285cb45d1742b5e)`[get]` |
| | Gets the current state of the strategy. |
| | |
| decimal | [CurrentPosition](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#aa3c8e8450cedf2e6dc772cee79022057)`[get]` |
| | Gets the current position volume of the strategy. |
| | |
| decimal | [AveragePrice](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a0990ad1aaa4097b121e2f9963026a3a2)`[get]` |
| | Gets the average price of the strategy's trades. |
| | |
| decimal | [OpenPnL](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a21cfcaa3598adf5c9a909fa1d87e08c1)`[get]` |
| | Gets the open profit and loss of the strategy. |
| | |
| decimal | [ClosedPnL](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a7b734a3537121bb3524c16cc94d5d8a2)`[get]` |
| | Gets the closed profit and loss of the strategy. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a1d6806a24b84f596cd7e4adf8a315edd)`[get, set]` |
| | Gets or sets the security associated with the strategy. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a2ac811e04e280f17e9a46aa376122326)`[get, set]` |
| | Gets or sets the portfolio associated with the strategy. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a1c4dd7746f51fdd6dc9033ec8c152494)`[get, set]` |
| | Gets or sets the T+ limits for the strategy. |
| | |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [Connector](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#ac42fb5c1c25cb4734acaa356371c6505)`[get, set]` |
| | Gets or sets the data feed connector for the strategy. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Public Attributes inherited from [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| bool | [IsValid](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1bfbc0db884b8d3126f7604c05709996) |
| | |
| - Public Attributes inherited from [ATAS.Strategies.ATM.IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| bool | [IsValid](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#a586922c33fc9e3150299d46624b2d63b) |
| | |
| - Public Attributes inherited from [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| bool | [IsValid](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a23bf81300da3e9bbc889674596740711) |
| | |
| - Events inherited from [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| EventHandler | [SettingsChanged](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a45e7b7709741ffd56e34e670e5c76731) |
| | |
| - Events inherited from [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| EventHandler | [StateChanged](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#ab97be6450a8121dafbba88aa9025ca90) |
| | Occurs when the state of the strategy changes. |
| | |
| EventHandler | [ShowNotification](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a064d9dc57f09b7862d89b5b94e47c04c) |
| | Occurs when the strategy needs to show a notification or alert. |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)StopProfit()

| ATAS.Strategies.ATM.StopProfit.StopProfit | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CanSetCustomStop()

| bool ATAS.Strategies.ATM.StopProfit.CanSetCustomStop | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.Strategies.ATM.ISupportCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#a7ba23a96d1284f5cae81c6df2f53a9f6).

## [◆](https://docs.atas.net/en/)CanSetCustomTake()

| bool ATAS.Strategies.ATM.StopProfit.CanSetCustomTake | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.Strategies.ATM.ISupportCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#aa57753f309d3d398e8617a1450e835fe).

## [◆](https://docs.atas.net/en/)CommitChanges()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.CommitChanges | ( | [StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) | model, |
| --- | --- | --- | --- |
| | | bool | cloneOrders |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)CreateNew()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) [StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) ATAS.Strategies.ATM.StopProfit.CreateNew | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)DisableStopAndTake()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.DisableStopAndTake | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ad75d314bb1018ddf9578b9f669f2bf5a).

## [◆](https://docs.atas.net/en/)GetSettingsWithStopOrTake()

| [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) ATAS.Strategies.ATM.StopProfit.GetSettingsWithStopOrTake | ( | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | stop, |
| --- | --- | --- | --- |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | take |
| | ) | | |

Implements [ATAS.Strategies.ATM.ISupportCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#ad1bb4f174d43573224eb055a9d43f036).

## [◆](https://docs.atas.net/en/)IsStopOrder()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool ATAS.Strategies.ATM.StopProfit.IsStopOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)IsTakeOrder()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool ATAS.Strategies.ATM.StopProfit.IsTakeOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)OnAttachStopAndTakeOrdersFailed()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.OnAttachStopAndTakeOrdersFailed | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ac0a22925136000c437511c5cac64f2f8).

## [◆](https://docs.atas.net/en/)OnCancelAll()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task ATAS.Strategies.ATM.StopProfit.OnCancelAll | ( | bool | retryOnError | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a2f76531d0ebb3032543eae4176905df9).

## [◆](https://docs.atas.net/en/)OnCancelSecondOrder()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task ATAS.Strategies.ATM.StopProfit.OnCancelSecondOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a0f55ef8d4d43eb8a1050b3ee846e7131).

## [◆](https://docs.atas.net/en/)OnCurrentPositionChanged()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.OnCurrentPositionChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)OnGetSettings()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) [SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) ATAS.Strategies.ATM.StopProfit.OnGetSettings | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1686e830263d34353c4786ceb8721aef).

## [◆](https://docs.atas.net/en/)OnIsValidSettings()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) ICollection ATAS.Strategies.ATM.StopProfit.OnIsValidSettings | ( | [SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) | settings, |
| --- | --- | --- | --- |
| | | decimal | positionVolume, |
| | | decimal | positionPrice, |
| | | decimal | currentPrice |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)OnPnLChanged()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.OnPnLChanged | ( | int | ticks | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)OnProcess()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task ATAS.Strategies.ATM.StopProfit.OnProcess | ( | [ChangesInfo](../structs/structATAS_1_1Strategies_1_1ATM_1_1ChangesInfo.md) | info | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a7f80de1b44d30be5cce0b69b0a9d26cf).

## [◆](https://docs.atas.net/en/)OnProcessOrder()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.OnProcessOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a6766b0d5c676619c2d75e81233243643).

## [◆](https://docs.atas.net/en/)OnResetCustomStopAndTakePrices()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.OnResetCustomStopAndTakePrices | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1e137321d1783880ed1552da0a82c7c0).

## [◆](https://docs.atas.net/en/)OnResetStopTakeOrderId()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.OnResetStopTakeOrderId | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa7500ad1f7fa28c83db0f8061af3f308).

## [◆](https://docs.atas.net/en/)OnSetSettings()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void ATAS.Strategies.ATM.StopProfit.OnSetSettings | ( | [SimpleStopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#aece02d98baa9554ee0a7e700fef5507e) | settings | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)SetCustomStopOrTake()

| async Task ATAS.Strategies.ATM.StopProfit.SetCustomStopOrTake | ( | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | stop, |
| --- | --- | --- | --- |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | take |
| | ) | | |

Implements [ATAS.Strategies.ATM.ISupportCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#a5e0ad2f9ccf5d6b546e4f28d6a8fce47).

## [◆](https://docs.atas.net/en/)SetStopOrTakeCancelledManually()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool ATAS.Strategies.ATM.StopProfit.SetStopOrTakeCancelledManually | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | string | type |
| | ) | | |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a0c0d60cecdeaa6044ff9b9214d71021a).

## [◆](https://docs.atas.net/en/)SetStopOrTakeCustomPrice()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool ATAS.Strategies.ATM.StopProfit.SetStopOrTakeCustomPrice | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | currentStopOrTake, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder |
| | ) | | |

protectedvirtual

Implements [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#afbb7ab3c482969129d24ed5ed9043a9a).

## Property Documentation

## [◆](https://docs.atas.net/en/)Breakeven

| [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) ATAS.Strategies.ATM.StopProfit.Breakeven |
| --- |

getset

Implements [ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#aa1ba3f27064a57382d5bbae65af7b116).

## [◆](https://docs.atas.net/en/)CurrentStop

| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? ATAS.Strategies.ATM.StopProfit.CurrentStop |
| --- |

get

Implements [ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a6c25d90f23cfdf8ef91b02513d5c9925).

## [◆](https://docs.atas.net/en/)CurrentTake

| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? ATAS.Strategies.ATM.StopProfit.CurrentTake |
| --- |

get

Implements [ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a121ba7d376d8cb6c78ecd02fbe4f66a8).

## [◆](https://docs.atas.net/en/)HasActiveOrders

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool ATAS.Strategies.ATM.StopProfit.HasActiveOrders |
| --- |

get

Implements [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#acc870efe054da5d275b892403d2be88b).

## [◆](https://docs.atas.net/en/)IsStopOrderAttached

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool ATAS.Strategies.ATM.StopProfit.IsStopOrderAttached |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)IsTakeOrderAttached

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) bool ATAS.Strategies.ATM.StopProfit.IsTakeOrderAttached |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)StopLoss

| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) ATAS.Strategies.ATM.StopProfit.StopLoss |
| --- |

get

Implements [ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a3ac92a36baf47dbacb8e01f86e2e9fd5).

## [◆](https://docs.atas.net/en/)StopOrderId

| [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? ATAS.Strategies.ATM.StopProfit.StopOrderId |
| --- |

get

## [◆](https://docs.atas.net/en/)TakeOrderId

| [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c)? ATAS.Strategies.ATM.StopProfit.TakeOrderId |
| --- |

get

## [◆](https://docs.atas.net/en/)TakeProfit

| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) ATAS.Strategies.ATM.StopProfit.TakeProfit |
| --- |

get

Implements [ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#adf0573e46657ed8b00537498f00292cb).

## [◆](https://docs.atas.net/en/)TrailingStop

| [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) ATAS.Strategies.ATM.StopProfit.TrailingStop |
| --- |

getset

Implements [ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#adeeb97f671510a5149e78541b43a7503).

The documentation for this class was generated from the following file:
- [StopProfit.cs](../files/StopProfit_8cs.md)
