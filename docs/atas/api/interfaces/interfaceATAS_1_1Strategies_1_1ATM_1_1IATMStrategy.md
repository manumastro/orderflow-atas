# ATAS.Strategies.ATM.IATMStrategy Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.html

Inheritance diagram for ATAS.Strategies.ATM.IATMStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.ATM.IATMStrategy:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
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

| Public Attributes | |
| --- | --- |
| bool | [IsValid](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a23bf81300da3e9bbc889674596740711) |
| | |

| Properties | |
| --- | --- |
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

| Events | |
| --- | --- |
| EventHandler | [SettingsChanged](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md#a45e7b7709741ffd56e34e670e5c76731) |
| | |
| - Events inherited from [ATAS.Strategies.IStrategy](./interfaceATAS_1_1Strategies_1_1IStrategy.md) | |
| EventHandler | [StateChanged](./interfaceATAS_1_1Strategies_1_1IStrategy.md#ab97be6450a8121dafbba88aa9025ca90) |
| | Occurs when the state of the strategy changes. |
| | |
| EventHandler | [ShowNotification](./interfaceATAS_1_1Strategies_1_1IStrategy.md#a064d9dc57f09b7862d89b5b94e47c04c) |
| | Occurs when the strategy needs to show a notification or alert. |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CancelAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.CancelAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a3c0db8acf399abef03d9763d49d281ec).

## [◆](https://docs.atas.net/en/)CancelOrderAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.CancelOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated = `true` |
| | ) | | |

## [◆](https://docs.atas.net/en/)CancelOrdersAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.CancelOrdersAsync | ( | IEnumerable | orders | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ab66c0321cfc5b4b1cba05b0fadf11ebf).

## [◆](https://docs.atas.net/en/)Clone()

| [IATMStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) ATAS.Strategies.ATM.IATMStrategy.Clone | ( | bool | cloneOrders = `true` | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a5be354c14612cc70cf9137d1d0934c4e).

## [◆](https://docs.atas.net/en/)GetSettings()

| [IStopProfitSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) ATAS.Strategies.ATM.IATMStrategy.GetSettings | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ab126b2de6acaed5fddf6f09763832cb7), [ATAS.Strategies.ATM.BaseStopProfitStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md#adac77a8844a40163402068af1f951bc3), and [ATAS.Strategies.ATM.IStopProfitStrategy](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md#a52a15391512ff63a67196251bddf7e01).

## [◆](https://docs.atas.net/en/)IsStopLoss()

| bool ATAS.Strategies.ATM.IATMStrategy.IsStopLoss | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a9cd61b463f565baaef2e0b48329e246b).

## [◆](https://docs.atas.net/en/)IsTakeProfit()

| bool ATAS.Strategies.ATM.IATMStrategy.IsTakeProfit | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ac88a85b75d9b28103a250173e8040cd7).

## [◆](https://docs.atas.net/en/)IsValidSettings()

| bool IEnumerable Errors ATAS.Strategies.ATM.IATMStrategy.IsValidSettings | ( | [IStopProfitSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | settings, |
| --- | --- | --- | --- |
| | | decimal? | expectedPositionVolume = `null`, |
| | | decimal? | expectedPositionPrice = `null` |
| | ) | | |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a33faf1a8bfe8067583a9cf037a101e69).

## [◆](https://docs.atas.net/en/)ModifyOrderAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.ModifyOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder, |
| | | bool | isAutomated = `true` |
| | ) | | |

## [◆](https://docs.atas.net/en/)OpenOrderAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.OpenOrderAsync | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | bool | isAutomated = `true` |
| | ) | | |

## [◆](https://docs.atas.net/en/)ResetOrdersAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.ResetOrdersAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#ad2514e3fb704531a88f0da5f08827991).

## [◆](https://docs.atas.net/en/)RetryAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.RetryAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a9c6b08f9ba7d1de0d2194c5b1e2125a6).

## [◆](https://docs.atas.net/en/)SetSettings()

| void ATAS.Strategies.ATM.IATMStrategy.SetSettings | ( | [IStopProfitSettings](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | settings | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#a9248d23b61fe219201ba993a0ddccad8).

## [◆](https://docs.atas.net/en/)StartFromWatchAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.StartFromWatchAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)WatchAsync()

| Task ATAS.Strategies.ATM.IATMStrategy.WatchAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Data Documentation

## [◆](https://docs.atas.net/en/)IsValid

| bool ATAS.Strategies.ATM.IATMStrategy.IsValid |
| --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)HasActiveOrders

| bool ATAS.Strategies.ATM.IATMStrategy.HasActiveOrders |
| --- |

get

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#aeea64b6cef55bd3cbc1311bfe2d04cc4), and [ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md#ac3117428a29fa34ba4581cc51133269b).

## [◆](https://docs.atas.net/en/)MarketDataProvider

| [IStrategyMarketDataProvider](./interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) ATAS.Strategies.ATM.IATMStrategy.MarketDataProvider |
| --- |

getset

Implemented in [ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md#afc1a0e9d30c1b23a4956150d021a4207).

## Event Documentation

## [◆](https://docs.atas.net/en/)SettingsChanged

| EventHandler ATAS.Strategies.ATM.IATMStrategy.SettingsChanged |
| --- |

The documentation for this interface was generated from the following file:
- [IATMStrategy.cs](../files/IATMStrategy_8cs.md)
