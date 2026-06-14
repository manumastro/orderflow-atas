# ATAS.Strategies.ATM.BaseStopProfitStrategy< TStrategy, TSettings > Class Template Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.html

Inheritance diagram for ATAS.Strategies.ATM.BaseStopProfitStrategy< TStrategy, TSettings >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.ATM.BaseStopProfitStrategy< TStrategy, TSettings >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [SetSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#af7d303bfb2a7d3d2f1e0fb06df781280) (TSettings settings) |
| | |
| new TSettings | [GetSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adac77a8844a40163402068af1f951bc3) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa87824f6745908767e037c80afee3c0d) (TSettings settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| Task | [RetryAsync](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a9c6b08f9ba7d1de0d2194c5b1e2125a6) () |
| | |
| Task | [CancelAsync](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a3c0db8acf399abef03d9763d49d281ec) () |
| | |
| Task | [ResetOrdersAsync](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ad2514e3fb704531a88f0da5f08827991) () |
| | |
| Task | [CancelOrdersAsync](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ab66c0321cfc5b4b1cba05b0fadf11ebf) (IEnumerable orders) |
| | |
| bool | [IsStopLoss](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a9cd61b463f565baaef2e0b48329e246b) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| bool | [IsTakeProfit](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ac88a85b75d9b28103a250173e8040cd7) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| [IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | [Clone](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a5be354c14612cc70cf9137d1d0934c4e) (bool cloneOrders=true) |
| | |
| void | [SetSettings](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a9248d23b61fe219201ba993a0ddccad8) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings) |
| | |
| [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [GetSettings](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ab126b2de6acaed5fddf6f09763832cb7) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a33faf1a8bfe8067583a9cf037a101e69) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md) | |
| void | [Start](./classATAS_1_1Strategies_1_1Strategy.md#ade4c390a1851155c7f63a161dde6196e) () |
| | |
| void | [Stop](./classATAS_1_1Strategies_1_1Strategy.md#a5af17fec828c2ce5b39c84cab7512f36) () |
| | |
| async Task | [StartAsync](./classATAS_1_1Strategies_1_1Strategy.md#a2e9997a9e4ac7d773684f1a09bb0a6a4) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| async Task | [WatchAsync](./classATAS_1_1Strategies_1_1Strategy.md#ad58d918eb8415a7c32148b5b3133f7d3) () |
| | |
| async Task | [StartFromWatchAsync](./classATAS_1_1Strategies_1_1Strategy.md#a3f73ff7b92a1371c9af383339dbe8e73) () |
| | |
| async Task | [StopAsync](./classATAS_1_1Strategies_1_1Strategy.md#acf7b05b5b8377f0f22cbaf0804ea3565) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
| | |
| async void | [OpenOrder](./classATAS_1_1Strategies_1_1Strategy.md#a2c9169c7b4be83c53f1c3c7f90c6879e) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Opens an order. |
| | |
| async Task | [OpenOrderAsync](./classATAS_1_1Strategies_1_1Strategy.md#a8be8f763339e633e01c272f68b17778f) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Opens an order. |
| | |
| async void | [ModifyOrder](./classATAS_1_1Strategies_1_1Strategy.md#a7a99290532c1685f6142263ed62e6083) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) neworder, bool isAutomated=true) |
| | Modifies an order. |
| | |
| async Task | [ModifyOrderAsync](./classATAS_1_1Strategies_1_1Strategy.md#adbd0efd2d87e907763d81f961470f6cc) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) neworder, bool isAutomated=true) |
| | Modifies an order. |
| | |
| async void | [CancelOrder](./classATAS_1_1Strategies_1_1Strategy.md#af888a67fab260e57c014dc859556a915) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Cancels an order. |
| | |
| async Task | [CancelOrderAsync](./classATAS_1_1Strategies_1_1Strategy.md#a423b3fb136fb060941db865828d08dc5) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | Cancels an order. |
| | |
| Task | [StartAsync](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a8b8803a4d0eff2da7088e4f0b3b4d25a) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| Task | [StopAsync](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a83d83423aa3f8a5248d681b4bb5126ad) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
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
| void | [SetSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#aba182152ff29e62b10b2b57143bcd294) (TSettings settings) |
| | |
| new TSettings | [GetSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#a52a15391512ff63a67196251bddf7e01) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#ac0b1f6d0061d5289a16df3bb650e26f4) (TSettings settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |

| Public Attributes | |
| --- | --- |
| bool | [IsValid](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1bfbc0db884b8d3126f7604c05709996) |
| | |
| - Public Attributes inherited from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| bool | [IsValid](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ae39e55f466e440a67f31bbe375d23ddc) |
| | |
| - Public Attributes inherited from [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| bool | [IsValid](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a23bf81300da3e9bbc889674596740711) |
| | |
| - Public Attributes inherited from [ATAS.Strategies.ATM.IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| bool | [IsValid](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#a586922c33fc9e3150299d46624b2d63b) |
| | |

| Protected Member Functions | |
| --- | --- |
| async Task | [ProcessAfterDelay](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adcc7367e8f6cb591e2285ae17cf47ec8) (TimeSpan? delay=null) |
| | |
| void | [Process](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa411b40ab2c749b1449d02466fb4911a) (bool isPositionChanged=false, bool isOrdersChanged=false, bool isSettingsChanged=false) |
| | |
| decimal | [GetPriceValue](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a3211545827e47b6ccc69267e20ae203d) (decimal price, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) value, decimal sign) |
| | |
| bool | [IsEquals](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a6535900ee13798d776fb715cd275060b) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) first, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) second, bool isStop, bool comparePrices) |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [TryAttachOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ad42988cfa3125f3ad09354c0a19f5c5b) (string type, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c) orderId, string prefix) |
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
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnStarted](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a693a5e82857944e5b9bb85579e43fe00) () |
| | Called when the strategy is started from StrategyStates.Stopped state. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnStartedFromWatch](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adf3fb846efbee406351f0e55131939f1) () |
| | Called when the strategy is started from StrategyStates.Watch state. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnStopping](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a632892c7ccee633506067a6ff11fdaff) () |
| | Called when the strategy is stopping. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) Task | [OnRetry](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a4c08efaeaff121d7a46250487290b61c) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnCancel](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a7c07155c38d002c3c6c96b074a39fed6) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) Task | [OnResetOrders](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a6ee94f5be8dfd99d1c016ed8779fedd5) () |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnCurrentPositionChanged](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a2520687829c28c320d925427f6b25a5d) () |
| | Called when the volume of the current position changes. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnModifyOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a899f91073ddab1029d5a5706cb20cdaf) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool isAutomated) |
| | Called when an existing order is modified. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnCancelOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ad239e5d34d8d75ad5c63276c19364e89) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated) |
| | Called when an order is canceled. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task | [OnCancelOrdersAsync](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a998b27051154eb391d28fc5b84c40dca) (IEnumerable orders) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnNewOrder](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#aa36961b5dcd5d13474f766ed0c953f54) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when a new order is added. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnOrderChanged](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a25c70530bf4bddf14a72b84c53ecb6bb) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when an existing order is changed. |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void | [OnSetStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a99c1bd67812932e6b706ee3b36aff855) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings) |
| | |
| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [OnGetStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1614a714474c1be81ba76fc19d42265f) () |
| | |
| | [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) (bool [IsValid](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a1bfbc0db884b8d3126f7604c05709996), IEnumerable Errors) [OnIsValidStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a1cfd9d1bcc400327f011130087b2d24e)([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings |
| | |
| - Protected Member Functions inherited from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| virtual Task | [OnRetry](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a7c70d5f5a10b033a64a782821eddbd15) () |
| | |
| virtual Task | [OnCancel](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a18cd89c13f77ce73daab4138fec33da4) () |
| | |
| virtual Task | [OnResetOrders](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#abb8a47643844a9203967d08cd26363bf) () |
| | |
| virtual TStrategy | [CreateNew](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a3eac58df3b033424ed7aae44fdc2307c) () |
| | |
| virtual void | [CommitChanges](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a86b3b76aec1e9cba99ff272c2bb6764b) (TStrategy model, bool cloneOrders) |
| | |
| abstract Task | [OnCancelOrdersAsync](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a7cf2572ccf78da9e557a84bea32b7ecd) (IEnumerable orders) |
| | |
| abstract bool | [IsStopOrder](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#aed51d1f7087601d2ffb44410f78642bd) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| abstract bool | [IsTakeOrder](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a83f970aeeccdd29d5f684030fa656623) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| abstract void | [OnSetStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#aaa93d13945b77e5e5f0114aae6f254e7) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings) |
| | |
| abstract [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [OnGetStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a859093a7bdae4db6a923aea46f506b97) () |
| | |
| abstract bool IEnumerable Errors | [OnIsValidStopProfitSettings](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a1cfd9d1bcc400327f011130087b2d24e) ([IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings, decimal? positionVolume, decimal? positionPrice) |
| | |
| void | [RaiseSettingsChanged](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#aeae54462c30ad9f4af8868cca72b61dc) () |
| | |
| void | [ThrowIfNotStarted](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a75cc6ffe499ef9e3f452b33156b9ab2c) () |
| | |
| override bool | [CanUpdateCurrentPosition](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a1c3fef087f9054bf719b4862faa0b17c) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | Checks if the current position can be updated with the specified position. |
| | |
| override void | [LogParameters](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#af565f074a5cf9ca29ba9ef2ccdc776e1) () |
| | Log current parameters. |
| | |
| - Protected Member Functions inherited from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md) | |
| | [Strategy](./classATAS_1_1Strategies_1_1Strategy.md#abb4d47356a0d6d870711f2b8ddde84fd) () |
| | |
| void | [SetState](./classATAS_1_1Strategies_1_1Strategy.md#ac068422672291f843592b4fa5674dda8) ([StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) state) |
| | Set strategy state. |
| | |
| void | [SetErrorState](./classATAS_1_1Strategies_1_1Strategy.md#aa374a8db1a31455b708ab997ead89690) ([StrategyErrorTypes](../namespaces/namespaceATAS_1_1Strategies.md#ae0d6f8e9e27e76f822654bb3f14b4271) type, string[] errorDescriptions) |
| | Set StrategyStates.Error state. |
| | |
| void | [ResetErrorState](./classATAS_1_1Strategies_1_1Strategy.md#a0bbe50de68938af9f856e7ac37b1dce2) () |
| | Reset error state. |
| | |
| void | [RaisePropertyChanged](./classATAS_1_1Strategies_1_1Strategy.md#ad6491f31b4fe77ec504af425a116d1fb) (string propertyName) |
| | Raises the PropertyChanged event with the specified property name. |
| | |
| void | [RaiseShowNotification](./classATAS_1_1Strategies_1_1Strategy.md#a2453e60639c8ad70cabb12c6f89557f0) (string message, string title=null, bool isError=false) |
| | Raises the ShowNotification event with the specified message, title, and error flag. |
| | |
| bool | [SetProperty](./classATAS_1_1Strategies_1_1Strategy.md#ab29c24bebb1b7fff85eaaa2fa45a210c) (ref TValue storage, TValue newValue, string propertyName, Action onChanged=null) |
| | Sets the property with the specified name to the new value and raises the PropertyChanged event if the value has changed. |
| | |
| string | [GetOCOGroup](./classATAS_1_1Strategies_1_1Strategy.md#a7d4b4a970053ef86131cdd580da050af) () |
| | Generates a unique OCO (One-Cancels-the-Other) group identifier based on the current timestamp. |
| | |
| bool | [CanProcess](./classATAS_1_1Strategies_1_1Strategy.md#a81f6b6ef027cede9cfdd7858a6d9c651) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Checks if the specified order can be processed by this strategy. |
| | |
| ICollection | [FilterMyTrades](./classATAS_1_1Strategies_1_1Strategy.md#a1294d8b1944d475615fb98e7d1f619f4) (IEnumerable trades) |
| | Filters and returns the collection of MyTrade that belong to the current portfolio and security and have occurred after the latest trade time. |
| | |
| void | [UpdateCurrentPosition](./classATAS_1_1Strategies_1_1Strategy.md#ae2f7d02455e2ad2cd233396aa1d57b5c) () |
| | Update thr CurrentPosition and AveragePrice values. |
| | |
| virtual Task | [OnStarted](./classATAS_1_1Strategies_1_1Strategy.md#a797a10bf103faf848475194494d75457) () |
| | Called when the strategy is started from StrategyStates.Stopped state. |
| | |
| virtual Task | [OnStartedFromWatch](./classATAS_1_1Strategies_1_1Strategy.md#ac78b46e4e7e0613d70ededb86afd2a6f) () |
| | Called when the strategy is started from StrategyStates.Watch state. |
| | |
| virtual Task | [OnStopping](./classATAS_1_1Strategies_1_1Strategy.md#a78ec5fd60d54a1f0c97b7ffaf53eede1) () |
| | Called when the strategy is stopping. |
| | |
| virtual Task | [OnStopped](./classATAS_1_1Strategies_1_1Strategy.md#a789356ecb84f130966e6ef84674dd21d) () |
| | Called when the strategy is stopped. |
| | |
| virtual Task | [OnOpenOrder](./classATAS_1_1Strategies_1_1Strategy.md#a02f76248cf2c12eea0793e17b2f35f83) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated) |
| | Called when a new order is opened. |
| | |
| virtual Task | [OnModifyOrder](./classATAS_1_1Strategies_1_1Strategy.md#a714f291495bd35ba61918fdc19e24f44) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool isAutomated) |
| | Called when an existing order is modified. |
| | |
| virtual Task | [OnCancelOrder](./classATAS_1_1Strategies_1_1Strategy.md#a3f1495660e455f48f120119007cc4b09) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated) |
| | Called when an order is canceled. |
| | |
| virtual void | [OnMarketDepth](./classATAS_1_1Strategies_1_1Strategy.md#ae42d28938d0d280b931f8e5f7dfc0465) (IEnumerable depths) |
| | Called when market depth data is received. |
| | |
| virtual void | [OnBestBidAsk](./classATAS_1_1Strategies_1_1Strategy.md#ae76af83f6b2f04a312bfe67d32735aa4) ([MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) depth) |
| | Called when the best bid or ask market depth data is received. |
| | |
| virtual void | [OnNewTrade](./classATAS_1_1Strategies_1_1Strategy.md#a1156a4c5fa80db189589d1fe5ed41673) ([Trade](./classATAS_1_1DataFeedsCore_1_1Trade.md) trade) |
| | Called when a new trade occurs. |
| | |
| virtual void | [OnNewPortfolio](./classATAS_1_1Strategies_1_1Strategy.md#a9bd23dc276dd94f8d782f4251e5d4f2c) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) |
| | Called when a new portfolio is added. |
| | |
| virtual void | [OnNewPosition](./classATAS_1_1Strategies_1_1Strategy.md#ab25c8865cb3c8e25d7a8d01d7edbbc2d) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | Called when a new position is added. |
| | |
| virtual void | [OnPositionChanged](./classATAS_1_1Strategies_1_1Strategy.md#af17d6ef4486c7ad54b3e682aaa61310e) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | Called when an existing position is changed. |
| | |
| virtual void | [OnPnLChanged](./classATAS_1_1Strategies_1_1Strategy.md#a31523364afbae751f14e7d9480a2de8e) (int ticks) |
| | Called when the profit and loss (PnL) changes. |
| | |
| virtual void | [OnNewOrder](./classATAS_1_1Strategies_1_1Strategy.md#abcf82eac51543b99ff8b97d52785b9fc) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when a new order is added. |
| | |
| virtual void | [OnOrderChanged](./classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | Called when an existing order is changed. |
| | |
| virtual void | [OnOrderRegisterFailed](./classATAS_1_1Strategies_1_1Strategy.md#a2ea44a2007103545463be80f85b515b5) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string message) |
| | Called when an order registration fails. |
| | |
| virtual void | [OnOrderCancelFailed](./classATAS_1_1Strategies_1_1Strategy.md#a733aec75e1b68396ff38bafc9de74af6) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string message) |
| | Called when an order cancellation fails. |
| | |
| virtual void | [OnOrderModifyFailed](./classATAS_1_1Strategies_1_1Strategy.md#a2a206383275db0490ffadcfc6580425e) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, string message) |
| | Called when an order modification fails. |
| | |
| virtual void | [OnNewMyTrade](./classATAS_1_1Strategies_1_1Strategy.md#a648be4b1a2575e8a653b8a8a311d3971) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) myTrade) |
| | Called when a new trade is added to the collection of MyTrade. |
| | |
| virtual void | [OnCurrentPositionChanged](./classATAS_1_1Strategies_1_1Strategy.md#a4b1005797dce08e14bea18f7e1ca765d) () |
| | Called when the volume of the current position changes. |
| | |
| virtual void | [OnUpdateStrategyState](./classATAS_1_1Strategies_1_1Strategy.md#ade0b4bb984c19b8a445fda28b93726a1) () |
| | Called when the strategy state needs to be updated. |
| | |
| virtual bool | [CanProcess](./classATAS_1_1Strategies_1_1Strategy.md#a28b03647a27fced41cf5c2f07cc76dc2) () |
| | Checks if the strategy can process operations in the current state. |
| | |
| virtual bool | [CanUpdateCurrentPosition](./classATAS_1_1Strategies_1_1Strategy.md#a60d8e09ff972aefe78d4a9a64377d690) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | Checks if the current position can be updated with the specified position. |
| | |
| virtual void | [LogParameters](./classATAS_1_1Strategies_1_1Strategy.md#a794aeb1f7fd7a728e22b8a2d8e8e647e) () |
| | Log current parameters. |
| | |

| Properties | |
| --- | --- |
| abstract bool | [IsStopOrderAttached](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a90b76ae5119ca29818c9e619ed3e40ed)`[get]` |
| | |
| abstract bool | [IsTakeOrderAttached](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#af1df69fa601ea3279a7f75699674cf19)`[get]` |
| | |
| TimeSpan | [ProcessTimeout](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a42b7b643753c83c3c7127955a7a20d27)`[get, set]` |
| | |
| TimeSpan | [AttachOrdersPeriod](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#abfce3ceceaa43cf42532d61365417ef5) = TimeSpan.FromSeconds(30)`[get]` |
| | |
| TimeSpan | [RetryPeriodOnConnectionError](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#ada5edb3d24932d09a63dbbd324dc2c9c) = TimeSpan.FromSeconds(2)`[get]` |
| | |
| TimeSpan | [RetryPeriodOnOrderError](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a0d605e4a8fc002f1e456d94d78edddaf) = TimeSpan.FromMilliseconds(500)`[get]` |
| | |
| TimeSpan | [RetryPeriod](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a945304704f124ca43f31fc27490686a7) = TimeSpan.FromMilliseconds(200)`[get]` |
| | |
| - Properties inherited from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| abstract bool | [HasActiveOrders](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#aeea64b6cef55bd3cbc1311bfe2d04cc4)`[get]` |
| | |
| [IStrategyMarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) | [MarketDataProvider](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#afc1a0e9d30c1b23a4956150d021a4207)`[get, set]` |
| | |
| [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) | [TimeInForce](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a90101a13e4cfbfc6916eb8192d9a2546)`[get, set]` |
| | |
| - Properties inherited from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md) | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1Strategies_1_1Strategy.md#a8c12c1aa6a3076d09c89acc636cba81e)`[get, set]` |
| | Gets or sets the security associated with the strategy. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](./classATAS_1_1Strategies_1_1Strategy.md#a1d9f992e6103556c710ab7a5ec72c030)`[get, set]` |
| | Gets or sets the portfolio associated with the strategy. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](./classATAS_1_1Strategies_1_1Strategy.md#abc38115f61804e04c5d75060012c9c0e)`[get, set]` |
| | Gets or sets the T+ limits for the strategy. |
| | |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [Connector](./classATAS_1_1Strategies_1_1Strategy.md#a8a31979c7e9857cade90549708f2bc78)`[get, set]` |
| | Gets or sets the data feed connector for the strategy. |
| | |
| IEnumerable | [MyTrades](./classATAS_1_1Strategies_1_1Strategy.md#a896ae49426378cfde400f92c2b2210b4)`[get]` |
| | |
| IEnumerable | [Orders](./classATAS_1_1Strategies_1_1Strategy.md#a4eebe18eaa41c4fda44673f5b8e5b22a)`[get]` |
| | |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [Position](./classATAS_1_1Strategies_1_1Strategy.md#ab090a528e4c8d5d9657361931e4ab078)`[get, set]` |
| | |
| decimal | [CurrentPosition](./classATAS_1_1Strategies_1_1Strategy.md#a9bf909bec2b2dbc0eabd721c212285ff)`[get]` |
| | Gets the current position volume of the strategy. |
| | |
| decimal | [AveragePrice](./classATAS_1_1Strategies_1_1Strategy.md#a07573e17f1176d5b2adac90f5a886134)`[get]` |
| | Gets the average price of the strategy's trades. |
| | |
| int | [OpenTicksPnL](./classATAS_1_1Strategies_1_1Strategy.md#a5779518919a1fcce28069c3ceb17b18d)`[get]` |
| | |
| decimal | [OpenPnL](./classATAS_1_1Strategies_1_1Strategy.md#a3861bd96e7b0c5be873ea863b2a4122c)`[get]` |
| | Gets the open profit and loss of the strategy. |
| | |
| decimal | [ClosedPnL](./classATAS_1_1Strategies_1_1Strategy.md#aa2a9a663ead0622ef44f75aa412b579d)`[get]` |
| | Gets the closed profit and loss of the strategy. |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [BestBid](./classATAS_1_1Strategies_1_1Strategy.md#ac26d173e3f3d698c1bbee321af0e80ef)`[get]` |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | [BestAsk](./classATAS_1_1Strategies_1_1Strategy.md#ab9940b9ecbf3abf39a80e8989f643118)`[get]` |
| | |
| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) | [State](./classATAS_1_1Strategies_1_1Strategy.md#a247f1529ee7c68ea9e294dce42fc68e8)`[get, protected set]` |
| | Gets the current state of the strategy. |
| | |
| [StrategyStateDescription](../namespaces/namespaceATAS_1_1Strategies.md#ac2b219bf1e9f7c99f8f9fc4c5fe39ea3) | [StateDescription](./classATAS_1_1Strategies_1_1Strategy.md#a95b1b372b56a1f381429182e4520f070)`[get]` |
| | |
| string | [Name](./classATAS_1_1Strategies_1_1Strategy.md#aa9bfbe77e7c89fa0bc933c1a0351c5fb)`[get, set]` |
| | Gets or sets the name of the strategy. |
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
| - Properties inherited from [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| bool | [HasActiveOrders](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#acc870efe054da5d275b892403d2be88b)`[get]` |
| | |
| [IStrategyMarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) | [MarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#ae73409342feaf2a34cea866bc43f8f1a)`[get, set]` |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Protected Attributes inherited from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| abstract bool | [IsValid](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a634f84ba0d9e72663f192a692efb903d) |
| | |
| - Events inherited from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| EventHandler | [SettingsChanged](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a909467cf3cddb159e43473be33a68ff9) |
| | |
| - Events inherited from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md) | |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1Strategies_1_1Strategy.md#ae54cc0ee8ef3cfbf2efd66f4bfd42785) |
| | |
| EventHandler | [StateChanged](./classATAS_1_1Strategies_1_1Strategy.md#ab1e58113d9ed785c5d26d2cd2f560cac) |
| | |
| EventHandler | [ShowNotification](./classATAS_1_1Strategies_1_1Strategy.md#a4ac0ef8ec0383f07365ff1b2b719e301) |
| | |
| - Events inherited from [ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| EventHandler | [StateChanged](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#ab97be6450a8121dafbba88aa9025ca90) |
| | Occurs when the state of the strategy changes. |
| | |
| EventHandler | [ShowNotification](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md#a064d9dc57f09b7862d89b5b94e47c04c) |
| | Occurs when the strategy needs to show a notification or alert. |
| | |
| - Events inherited from [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| EventHandler | [SettingsChanged](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a45e7b7709741ffd56e34e670e5c76731) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CreateStopOrder()

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).CreateStopOrder | ( | [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) | direction, |
| --- | --- | --- | --- |
| | | decimal | sl, |
| | | decimal | volume, |
| | | string | prefix |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)CreateTakeOrder()

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).CreateTakeOrder | ( | [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) | direction, |
| --- | --- | --- | --- |
| | | decimal | tp, |
| | | decimal | volume, |
| | | string | prefix |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)DisableStopAndTake()

| abstract void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).DisableStopAndTake | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ac4005f2a51370fbf838ce1db36a654e1).

## [◆](https://docs.atas.net/en/)GetPriceValue()

| decimal [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).GetPriceValue | ( | decimal | price, |
| --- | --- | --- | --- |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | value, |
| | | decimal | sign |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)GetSettings()

| new TSettings [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).GetSettings | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#aaad791303b09343ea759213efbd8fabe).

## [◆](https://docs.atas.net/en/)IsEquals()

| bool [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).IsEquals | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | first, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | second, |
| | | bool | isStop, |
| | | bool | comparePrices |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)IsValidSettings()

| bool IEnumerable Errors [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).IsValidSettings | ( | TSettings | settings, |
| --- | --- | --- | --- |
| | | decimal? | expectedPositionVolume = `null`, |
| | | decimal? | expectedPositionPrice = `null` |
| | ) | | |

Implements [ATAS.Strategies.ATM.IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#ac0b1f6d0061d5289a16df3bb650e26f4).

## [◆](https://docs.atas.net/en/)OnAttachStopAndTakeOrdersFailed()

| abstract void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnAttachStopAndTakeOrdersFailed | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a05956d25133e388cc69e75971c79d0cc).

## [◆](https://docs.atas.net/en/)OnCancel()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnCancel | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a18cd89c13f77ce73daab4138fec33da4).

## [◆](https://docs.atas.net/en/)OnCancelAll()

| abstract Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnCancelAll | ( | bool | retryOnError | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ac2099b29f4cf2a322e918849f4eb9f91).

## [◆](https://docs.atas.net/en/)OnCancelOrder()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnCancelOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated |
| | ) | | |

protectedvirtual

Called when an order is canceled.

Parameters

| order | The order to be canceled. |
| --- | --- |
| isAutomated | A flag indicating whether the order was canceled automatically. |

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#a3f1495660e455f48f120119007cc4b09).

## [◆](https://docs.atas.net/en/)OnCancelOrdersAsync()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnCancelOrdersAsync | ( | IEnumerable | orders | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a7cf2572ccf78da9e557a84bea32b7ecd).

## [◆](https://docs.atas.net/en/)OnCancelSecondOrder()

| abstract Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnCancelSecondOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ad125e81efe1b4f931afe79d94854a48b).

## [◆](https://docs.atas.net/en/)OnCurrentPositionChanged()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnCurrentPositionChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the volume of the current position changes.

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#a4b1005797dce08e14bea18f7e1ca765d).

## [◆](https://docs.atas.net/en/)OnGetSettings()

| abstract TSettings [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnGetSettings | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a8e0c6ee279981323d8c170067955e43b).

## [◆](https://docs.atas.net/en/)OnGetStopProfitSettings()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnGetStopProfitSettings | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a859093a7bdae4db6a923aea46f506b97).

## [◆](https://docs.atas.net/en/)OnIsValidSettings()

| abstract ICollection [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnIsValidSettings | ( | TSettings | settings, |
| --- | --- | --- | --- |
| | | decimal | positionVolume, |
| | | decimal | positionPrice, |
| | | decimal | currentPrice |
| | ) | | |

protectedpure virtual

## [◆](https://docs.atas.net/en/)OnModifyOrder()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnModifyOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder, |
| | | bool | isAutomated |
| | ) | | |

protectedvirtual

Called when an existing order is modified.

Parameters

| order | The original order. |
| --- | --- |
| newOrder | The modified order with new parameters. |
| isAutomated | A flag indicating whether the order was modified automatically. |

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#a714f291495bd35ba61918fdc19e24f44).

## [◆](https://docs.atas.net/en/)OnNewOrder()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnNewOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when a new order is added.

Parameters

| order | The newly added order. |
| --- | --- |

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#abcf82eac51543b99ff8b97d52785b9fc).

## [◆](https://docs.atas.net/en/)OnOrderChanged()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnOrderChanged | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when an existing order is changed.

Parameters

| order | The changed order. |
| --- | --- |

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#a995541cf220d1ae25a69286f4fc66e16).

## [◆](https://docs.atas.net/en/)OnProcess()

| abstract Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnProcess | ( | [ChangesInfo](../structs/structATAS_1_1Strategies_1_1ATM_1_1ChangesInfo.md) | info | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a5564e656c1d88ef0d6ad9ae601480258).

## [◆](https://docs.atas.net/en/)OnProcessOrder()

| virtual void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnProcessOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#acb1000e0b8934efc56e586de28d87027).

## [◆](https://docs.atas.net/en/)OnResetCustomStopAndTakePrices()

| abstract void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnResetCustomStopAndTakePrices | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a1d39651f85f66e4fb0811e1918885b9c).

## [◆](https://docs.atas.net/en/)OnResetOrders()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnResetOrders | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#abb8a47643844a9203967d08cd26363bf).

## [◆](https://docs.atas.net/en/)OnResetStopTakeOrderId()

| abstract void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnResetStopTakeOrderId | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#afc83dab926c7a466bea03110ccb8b5c2).

## [◆](https://docs.atas.net/en/)OnRetry()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnRetry | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a7c70d5f5a10b033a64a782821eddbd15).

## [◆](https://docs.atas.net/en/)OnSetSettings()

| abstract void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnSetSettings | ( | TSettings | settings | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

## [◆](https://docs.atas.net/en/)OnSetStopProfitSettings()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnSetStopProfitSettings | ( | [IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | settings | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.Strategies.ATM.ATMStrategy](./classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#aaa93d13945b77e5e5f0114aae6f254e7).

## [◆](https://docs.atas.net/en/)OnStarted()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnStarted | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy is started from StrategyStates.Stopped state.

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#a797a10bf103faf848475194494d75457).

## [◆](https://docs.atas.net/en/)OnStartedFromWatch()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnStartedFromWatch | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy is started from StrategyStates.Watch state.

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#ac78b46e4e7e0613d70ededb86afd2a6f).

## [◆](https://docs.atas.net/en/)OnStopping()

| [override](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#a45817c8182d859eac9870d953a18b4b7) async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).OnStopping | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the strategy is stopping.

Reimplemented from [ATAS.Strategies.Strategy](./classATAS_1_1Strategies_1_1Strategy.md#a78ec5fd60d54a1f0c97b7ffaf53eede1).

## [◆](https://docs.atas.net/en/)override()

| [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).override | ( | bool | IsValid, |
| --- | --- | --- | --- |
| | | IEnumerable | Errors |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)Process()

| void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).Process | ( | bool | isPositionChanged = `false`, |
| --- | --- | --- | --- |
| | | bool | isOrdersChanged = `false`, |
| | | bool | isSettingsChanged = `false` |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)ProcessAfterDelay()

| async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).ProcessAfterDelay | ( | TimeSpan? | delay = `null` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)ResetCustomStopAndTakePrices()

| void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).ResetCustomStopAndTakePrices | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)ResetWatchState()

| void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).ResetWatchState | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)SetSettings()

| void [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).SetSettings | ( | TSettings | settings | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.Strategies.ATM.IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#aba182152ff29e62b10b2b57143bcd294).

## [◆](https://docs.atas.net/en/)SetStopOrTakeCancelledManually()

| abstract bool [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).SetStopOrTakeCancelledManually | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | string | type |
| | ) | | |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a366c31ee5a06417b16c0bdad6c40dbc1).

## [◆](https://docs.atas.net/en/)SetStopOrTakeCustomPrice()

| abstract bool [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).SetStopOrTakeCustomPrice | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | currentStopOrTake, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder |
| | ) | | |

protectedpure virtual

Implemented in [ATAS.Strategies.ATM.StopProfit](./classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a7c376d50314cda06ca7232475bdd2aae).

## [◆](https://docs.atas.net/en/)TryAttachOrder()

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).TryAttachOrder | ( | string | type, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| | | [StrategyOrderId](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#abd0d1017cc849a20e83f49f55492b15c) | orderId, |
| | | string | prefix |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)TryCancelSecondOrder()

| async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).TryCancelSecondOrder | ( | string | type, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| | | string | prefix |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)TryCancelStopOrTake()

| async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).TryCancelStopOrTake | ( | string | type, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| | | string | message, |
| | | string | prefix, |
| | | bool | retryOnError |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)TryProcessStopOrTake()

| async Task [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).TryProcessStopOrTake | ( | string | type, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | oldOrder, |
| | | [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) | direction, |
| | | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | currentValue, |
| | | int | positionSign, |
| | | decimal | volume, |
| | | string | prefix, |
| | | bool | isPositionChanged |
| | ) | | |

protected

## Member Data Documentation

## [◆](https://docs.atas.net/en/)IsValid

| bool [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).IsValid |
| --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AttachOrdersPeriod

| TimeSpan [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).AttachOrdersPeriod = TimeSpan.FromSeconds(30) |
| --- |

get

## [◆](https://docs.atas.net/en/)IsStopOrderAttached

| abstract bool [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).IsStopOrderAttached |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)IsTakeOrderAttached

| abstract bool [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).IsTakeOrderAttached |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)ProcessTimeout

| TimeSpan [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).ProcessTimeout |
| --- |

getset

## [◆](https://docs.atas.net/en/)RetryPeriod

| TimeSpan [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).RetryPeriod = TimeSpan.FromMilliseconds(200) |
| --- |

get

## [◆](https://docs.atas.net/en/)RetryPeriodOnConnectionError

| TimeSpan [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).RetryPeriodOnConnectionError = TimeSpan.FromSeconds(2) |
| --- |

get

## [◆](https://docs.atas.net/en/)RetryPeriodOnOrderError

| TimeSpan [ATAS.Strategies.ATM.BaseStopProfitStrategy](./classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md).RetryPeriodOnOrderError = TimeSpan.FromMilliseconds(500) |
| --- |

get

The documentation for this class was generated from the following file:
- [BaseStopProfitStrategy.cs](../files/BaseStopProfitStrategy_8cs.md)
