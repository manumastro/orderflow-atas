# ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.html

Inheritance diagram for ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [ConnectorWebsocket](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a138af3bb136596779d17260e8eca7e6c) (int requestPerPeriod, TimeSpan period) |
| | Private websocket connections. |
| | |
| | [ConnectorWebsocket](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a77faced127fa82ff5898bc13e30aafc8) (int requestPerPeriod, TimeSpan period, [IRequestSerializer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md) serializer, RateLimiter? connectionLimiter=null, RateLimiter? crossMessageLimiter=null, string? connectorId=null) |
| | Public websocket connections. |
| | |
| async Task | [Start](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a4f2be148ef23f96b2f264cfd2a11136b) () |
| | |
| async Task | [StopAsync](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a243b723a6c0a2e6a0a5a65aefde26c3a) () |
| | |
| bool | [SubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a1707774f1c2255501ae16f197c39b190) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subType) |
| | Subscription request. |
| | |
| bool | [SubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#ae37beb6ced12d0897cf440288e632d84) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subType) |
| | Market data subscription bulk request. |
| | |
| void | [SubscribeLiquidations](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a766f63510d968e3d8719622f66f6dfbe) (IEnumerable securities) |
| | Liquidation subscription request. |
| | |
| void | [UnsubscribeLiquidations](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a8726cbe13c0950ccdd160b227c501240) (IEnumerable securities) |
| | Liquidation subscription request. |
| | |
| bool | [UnsubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a28e2ee22b7618681e3a2740d909df168) ([Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subType) |
| | Cancelling market data subscription request. |
| | |
| bool | [UnsubscribeMarketData](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a47c4a90b8c3c4a70bc5fa55c98494f52) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) subType) |
| | Cancelling market data subscription bulk request. |
| | |
| void | [Send](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a3f9cca3d26a5530fda1f3bfb9f0aace1) (object message) |
| | Single request. |
| | |
| void | [SendImmediate](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a0a44f9ec29d3f600ca4e55ef6ea1383f) (object message) |
| | Send request as soon as possible. |
| | |
| void | [SabotageConnection](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#ac69232986e99147786b3b235475b47c0) () |
| | For test purposes only. |
| | |
| void | [SabotageInitialization](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a9d75d535782c32ed73df2908e33b6972) () |
| | For test purposes only. |
| | |

| Properties | |
| --- | --- |
| [IRequestSerializer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md)? | [Serializer](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a2ca170004588540c5487ef319d9e2908)`[get]` |
| | |
| bool | [IsPublic](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a413f9455d6865bcf9514bb4f789fe173)`[get]` |
| | |
| ConnectionStates | [ConnectionState](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#af2c5fc9a1c78b37901fcb5371a6a6c64)`[get]` |
| | |
| TimeSpan | [ReconnectionInterval](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a3a330c51ff2e23436ddaf3d3cc735f9d) = TimeSpan.FromSeconds(5)`[get]` |
| | |
| bool | [IsConnected](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a256d5cec58f16835eb35ba1b45eb6999)`[get]` |
| | |
| string | [Url](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#ab208a6ca325d165d36116540c5451b09)`[get, set]` |
| | |
| TimeSpan | [Timeout](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#aefad98d10eff3435cf382cf0dc40de33) = TimeSpan.FromSeconds(30)`[get]` |
| | |
| TaskCompletionSource | [PrivateConnectionSource](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a2beba0488142575c764c92549b4ce546) = new()`[get, set]` |
| | Completion source for custom connection conditions. |
| | |

| Events | |
| --- | --- |
| Action? | [Connected](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a6d57761f6f82725fa7cbb9520bc57144) |
| | |
| Action? | [Error](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#a8d8117a79996fd4d41fe463d3bef57f2) |
| | |
| Action? | [Message](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md#aafe7ecb462a2993c420483ca5ac3973a) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ConnectorWebsocket() [1/2]

| ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.ConnectorWebsocket | ( | int | requestPerPeriod, |
| --- | --- | --- | --- |
| | | TimeSpan | period |
| | ) | | |

Private websocket connections.

Parameters

| requestPerPeriod | |
| --- | --- |
| period | |

## [◆](https://docs.atas.net/en/)ConnectorWebsocket() [2/2]

| ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.ConnectorWebsocket | ( | int | requestPerPeriod, |
| --- | --- | --- | --- |
| | | TimeSpan | period, |
| | | [IRequestSerializer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md) | serializer, |
| | | RateLimiter? | connectionLimiter = `null`, |
| | | RateLimiter? | crossMessageLimiter = `null`, |
| | | string? | connectorId = `null` |
| | ) | | |

Public websocket connections.

Parameters

| requestPerPeriod | |
| --- | --- |
| period | |
| serializer | |
| connectionLimiter | |
| crossMessageLimiter | |
| connectorId | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)SabotageConnection()

| void ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.SabotageConnection | ( | | ) | |
| --- | --- | --- | --- | --- |

For test purposes only.

## [◆](https://docs.atas.net/en/)SabotageInitialization()

| void ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.SabotageInitialization | ( | | ) | |
| --- | --- | --- | --- | --- |

For test purposes only.

## [◆](https://docs.atas.net/en/)Send()

| void ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Send | ( | object | message | ) | |
| --- | --- | --- | --- | --- | --- |

Single request.

Parameters

| message | |
| --- | --- |

## [◆](https://docs.atas.net/en/)SendImmediate()

| void ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.SendImmediate | ( | object | message | ) | |
| --- | --- | --- | --- | --- | --- |

Send request as soon as possible.

Parameters

| message | |
| --- | --- |

## [◆](https://docs.atas.net/en/)Start()

| async Task ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Start | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)StopAsync()

| async Task ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.StopAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)SubscribeLiquidations()

| void ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.SubscribeLiquidations | ( | IEnumerable | securities | ) | |
| --- | --- | --- | --- | --- | --- |

Liquidation subscription request.

Parameters

| securities | |
| --- | --- |

## [◆](https://docs.atas.net/en/)SubscribeMarketData() [1/2]

| bool ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.SubscribeMarketData | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subType |
| | ) | | |

Market data subscription bulk request.

Parameters

| securities | |
| --- | --- |
| subType | |

## [◆](https://docs.atas.net/en/)SubscribeMarketData() [2/2]

| bool ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.SubscribeMarketData | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subType |
| | ) | | |

Subscription request.

Parameters

| security | |
| --- | --- |
| subType | |

## [◆](https://docs.atas.net/en/)UnsubscribeLiquidations()

| void ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.UnsubscribeLiquidations | ( | IEnumerable | securities | ) | |
| --- | --- | --- | --- | --- | --- |

Liquidation subscription request.

Parameters

| securities | |
| --- | --- |

## [◆](https://docs.atas.net/en/)UnsubscribeMarketData() [1/2]

| bool ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.UnsubscribeMarketData | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subType |
| | ) | | |

Cancelling market data subscription bulk request.

Parameters

| securities | |
| --- | --- |
| subType | |

## [◆](https://docs.atas.net/en/)UnsubscribeMarketData() [2/2]

| bool ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.UnsubscribeMarketData | ( | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | subType |
| | ) | | |

Cancelling market data subscription request.

Parameters

| security | |
| --- | --- |
| subType | |

## Property Documentation

## [◆](https://docs.atas.net/en/)ConnectionState

| ConnectionStates ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.ConnectionState |
| --- |

get

## [◆](https://docs.atas.net/en/)IsConnected

| bool ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.IsConnected |
| --- |

get

## [◆](https://docs.atas.net/en/)IsPublic

| bool ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.IsPublic |
| --- |

get

## [◆](https://docs.atas.net/en/)PrivateConnectionSource

| TaskCompletionSource ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.PrivateConnectionSource = new() |
| --- |

getset

Completion source for custom connection conditions.

## [◆](https://docs.atas.net/en/)ReconnectionInterval

| TimeSpan ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.ReconnectionInterval = TimeSpan.FromSeconds(5) |
| --- |

get

## [◆](https://docs.atas.net/en/)Serializer

| [IRequestSerializer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md)? ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Serializer |
| --- |

get

## [◆](https://docs.atas.net/en/)Timeout

| TimeSpan ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Timeout = TimeSpan.FromSeconds(30) |
| --- |

get

## [◆](https://docs.atas.net/en/)Url

| string ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Url |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)Connected

| Action? ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Connected |
| --- |

## [◆](https://docs.atas.net/en/)Error

| Action? ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Error |
| --- |

## [◆](https://docs.atas.net/en/)Message

| Action? ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket.Message |
| --- |

The documentation for this class was generated from the following file:
- [ConnectorWebsocket.cs](../files/ConnectorWebsocket_8cs.md)
