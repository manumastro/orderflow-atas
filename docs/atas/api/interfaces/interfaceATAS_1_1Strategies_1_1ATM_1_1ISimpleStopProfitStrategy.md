# ATAS.Strategies.ATM.ISimpleStopProfitStrategy Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.html

Inheritance diagram for ATAS.Strategies.ATM.ISimpleStopProfitStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.ATM.ISimpleStopProfitStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | [StopLoss](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a3ac92a36baf47dbacb8e01f86e2e9fd5)`[get]` |
| | |
| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) | [TakeProfit](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#adf0573e46657ed8b00537498f00292cb)`[get]` |
| | |
| [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) | [TrailingStop](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#adeeb97f671510a5149e78541b43a7503)`[get]` |
| | |
| [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) | [Breakeven](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#aa1ba3f27064a57382d5bbae65af7b116)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [CurrentStop](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a6c25d90f23cfdf8ef91b02513d5c9925)`[get]` |
| | |
| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? | [CurrentTake](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md#a121ba7d376d8cb6c78ecd02fbe4f66a8)`[get]` |
| | |
| - Properties inherited from [ATAS.Strategies.ATM.IATMStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| bool | [HasActiveOrders](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#acc870efe054da5d275b892403d2be88b)`[get]` |
| | |
| [IStrategyMarketDataProvider](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) | [MarketDataProvider](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#ae73409342feaf2a34cea866bc43f8f1a)`[get, set]` |
| | |
| - Properties inherited from [ATAS.Strategies.IStrategy](./interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| string | [Name](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a82fa08d68c09e1ac4b699ed5901408c3)`[get, set]` |
| | Gets or sets the name of the strategy. |
| | |
| [StrategyStates](../namespaces/namespaceATAS_1_1Strategies.md#a0367857bc3f059c0b5cbb9b88633cc89) | [State](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a18fcdb4e7e4a8d94e285cb45d1742b5e)`[get]` |
| | Gets the current state of the strategy. |
| | |
| decimal | [CurrentPosition](./interfaceATAS_1_1Strategies_1_1IStrategy.md#aa3c8e8450cedf2e6dc772cee79022057)`[get]` |
| | Gets the current position volume of the strategy. |
| | |
| decimal | [AveragePrice](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a0990ad1aaa4097b121e2f9963026a3a2)`[get]` |
| | Gets the average price of the strategy's trades. |
| | |
| decimal | [OpenPnL](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a21cfcaa3598adf5c9a909fa1d87e08c1)`[get]` |
| | Gets the open profit and loss of the strategy. |
| | |
| decimal | [ClosedPnL](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a7b734a3537121bb3524c16cc94d5d8a2)`[get]` |
| | Gets the closed profit and loss of the strategy. |
| | |
| [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a1d6806a24b84f596cd7e4adf8a315edd)`[get, set]` |
| | Gets or sets the security associated with the strategy. |
| | |
| [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a2ac811e04e280f17e9a46aa376122326)`[get, set]` |
| | Gets or sets the portfolio associated with the strategy. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a1c4dd7746f51fdd6dc9033ec8c152494)`[get, set]` |
| | Gets or sets the T+ limits for the strategy. |
| | |
| [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [Connector](./interfaceATAS_1_1Strategies_1_1IStrategy.md#ac42fb5c1c25cb4734acaa356371c6505)`[get, set]` |
| | Gets or sets the data feed connector for the strategy. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Public Member Functions inherited from [ATAS.Strategies.ATM.IStopProfitStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| void | [SetSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#aba182152ff29e62b10b2b57143bcd294) (TSettings settings) |
| | |
| new TSettings | [GetSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#a52a15391512ff63a67196251bddf7e01) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#ac0b1f6d0061d5289a16df3bb650e26f4) (TSettings settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.ATM.ISupportCustomStopOrTake](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md) | |
| bool | [CanSetCustomStop](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#a7ba23a96d1284f5cae81c6df2f53a9f6) () |
| | |
| bool | [CanSetCustomTake](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#aa57753f309d3d398e8617a1450e835fe) () |
| | |
| Task | [SetCustomStopOrTake](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#a5e0ad2f9ccf5d6b546e4f28d6a8fce47) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? stop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? take) |
| | |
| [IStopProfitSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [GetSettingsWithStopOrTake](./interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md#ad1bb4f174d43573224eb055a9d43f036) ([PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? stop, [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? take) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.ATM.IATMStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| Task | [WatchAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a9dc8c3245862a75b1008b570d6049cbb) () |
| | |
| Task | [StartFromWatchAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a6bff2153a446d534eca34647847a7254) () |
| | |
| Task | [RetryAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a9eb844d472ffbfa741dbb9a558cf2b02) () |
| | |
| Task | [CancelAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a8137f7e6fd51d83d38dfe7bd49509e53) () |
| | |
| Task | [ResetOrdersAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a87cfee45d0440ce36c78aca4d4114343) () |
| | |
| bool | [IsStopLoss](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#ac2f851e7a9476d63c9e9ef4bee2a9fef) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| bool | [IsTakeProfit](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a2b432249201d6e3740711800541cc2a5) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| Task | [OpenOrderAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a1c374ec5fe1701a86fa954474097f720) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | |
| Task | [ModifyOrderAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a3494b0a78fd62d7bc19433f0dfb926f5) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, bool isAutomated=true) |
| | |
| Task | [CancelOrderAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#ac46b3f21a23d9d5ebf2335585f97e491) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order, bool isAutomated=true) |
| | |
| Task | [CancelOrdersAsync](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a1a60092975f93f4b2f655bcb56e9fa1d) (IEnumerable orders) |
| | |
| [IATMStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | [Clone](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a349959c2b6b710a8d3c2123fe0e961a7) (bool cloneOrders=true) |
| | |
| void | [SetSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a37af4a4fe9af5c4760cb1ddfb13f77cf) ([IStopProfitSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings) |
| | |
| [IStopProfitSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | [GetSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#aaad791303b09343ea759213efbd8fabe) () |
| | |
| bool IEnumerable Errors | [IsValidSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#acb488db17ab5f60e367141abf559c123) ([IStopProfitSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) settings, decimal? expectedPositionVolume=null, decimal? expectedPositionPrice=null) |
| | |
| - Public Member Functions inherited from [ATAS.Strategies.IStrategy](./interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| Task | [StartAsync](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a8b8803a4d0eff2da7088e4f0b3b4d25a) () |
| | Starts the strategy, allowing it to execute its trading logic. |
| | |
| Task | [StopAsync](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a83d83423aa3f8a5248d681b4bb5126ad) () |
| | Stops the strategy, terminating its execution and releasing any resources. |
| | |
| - Public Attributes inherited from [ATAS.Strategies.ATM.IStopProfitStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| bool | [IsValid](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#a586922c33fc9e3150299d46624b2d63b) |
| | |
| - Public Attributes inherited from [ATAS.Strategies.ATM.IATMStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| bool | [IsValid](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a23bf81300da3e9bbc889674596740711) |
| | |
| - Events inherited from [ATAS.Strategies.ATM.IATMStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| EventHandler | [SettingsChanged](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a45e7b7709741ffd56e34e670e5c76731) |
| | |
| - Events inherited from [ATAS.Strategies.IStrategy](./interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| EventHandler | [StateChanged](./interfaceATAS_1_1Strategies_1_1IStrategy.md#ab97be6450a8121dafbba88aa9025ca90) |
| | Occurs when the state of the strategy changes. |
| | |
| EventHandler | [ShowNotification](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a064d9dc57f09b7862d89b5b94e47c04c) |
| | Occurs when the strategy needs to show a notification or alert. |
| | |

## Property Documentation

## [◆](https://docs.atas.net/en/)Breakeven

| [BreakevenSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a25ddc60f5ad7c9749239a908f8a59425) ATAS.Strategies.ATM.ISimpleStopProfitStrategy.Breakeven |
| --- |

get

Implemented in [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#acd27147ad61b16c1d2992767e6a6b180).

## [◆](https://docs.atas.net/en/)CurrentStop

| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? ATAS.Strategies.ATM.ISimpleStopProfitStrategy.CurrentStop |
| --- |

get

Implemented in [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#aa2983263a39f368f1862c77b5124fd8d).

## [◆](https://docs.atas.net/en/)CurrentTake

| [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md)? ATAS.Strategies.ATM.ISimpleStopProfitStrategy.CurrentTake |
| --- |

get

Implemented in [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#aa740ee89a8e0082e4331aecdad268918).

## [◆](https://docs.atas.net/en/)StopLoss

| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) ATAS.Strategies.ATM.ISimpleStopProfitStrategy.StopLoss |
| --- |

get

Implemented in [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a33c18ded70157d6a2277732915ce44cb).

## [◆](https://docs.atas.net/en/)TakeProfit

| [StopProfitSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#a72c608dc14fa0b7809e9a09a92ae061a) ATAS.Strategies.ATM.ISimpleStopProfitStrategy.TakeProfit |
| --- |

get

Implemented in [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a3dafbf345ca91c06b9869722b4cd3902).

## [◆](https://docs.atas.net/en/)TrailingStop

| [TrailingStopSettings](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md#ade19e88488040496dc139e8d370fbd29) ATAS.Strategies.ATM.ISimpleStopProfitStrategy.TrailingStop |
| --- |

get

Implemented in [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#a1a63e4fa9807ba417c36ad1cb2c94162).

The documentation for this interface was generated from the following file:
- [IATMStrategy.cs](../files/IATMStrategy_8cs.md)
