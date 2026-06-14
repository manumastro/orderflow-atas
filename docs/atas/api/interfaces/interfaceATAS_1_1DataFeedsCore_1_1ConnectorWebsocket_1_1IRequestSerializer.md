# ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.html

| Public Member Functions | |
| --- | --- |
| object[] | [SubscribeMarket](./interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md#a5a2ce5b08b9a1d13108082c130fa6487) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) types) |
| | |
| object[] | [UnsubscribeMarket](./interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md#a10496930185b38f4687964092d067f50) ([Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) security, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) types) |
| | |
| object[] | [SubscribeMarket](./interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md#aff2c079ea28332ceb6e93ddabbc1241e) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) types) |
| | |
| object[] | [UnsubscribeMarket](./interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md#a178c1e8455ac01111050b17fa0a29d1a) (IEnumerable securities, [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) types) |
| | |
| object[] | [SubscribeLiquidations](./interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md#a57ca85f5567a3c860cde326abbf20e20) (IEnumerable securities) |
| | |
| object[] | [UnsubscribeLiquidations](./interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md#aca1d0d5991ca0493d5d066a126f7b278) (IEnumerable securities) |
| | |
| void | [Reset](./interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md#afee9e7312ffacaa2af54353b3790b889) () |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Reset()

| void ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer.Reset | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)SubscribeLiquidations()

| object[] ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer.SubscribeLiquidations | ( | IEnumerable | securities | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)SubscribeMarket() [1/2]

| object[] ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer.SubscribeMarket | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | types |
| | ) | | |

## [◆](https://docs.atas.net/en/)SubscribeMarket() [2/2]

| object[] ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer.SubscribeMarket | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | types |
| | ) | | |

## [◆](https://docs.atas.net/en/)UnsubscribeLiquidations()

| object[] ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer.UnsubscribeLiquidations | ( | IEnumerable | securities | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)UnsubscribeMarket() [1/2]

| object[] ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer.UnsubscribeMarket | ( | IEnumerable | securities, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | types |
| | ) | | |

## [◆](https://docs.atas.net/en/)UnsubscribeMarket() [2/2]

| object[] ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer.UnsubscribeMarket | ( | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| --- | --- | --- | --- |
| | | [SubscriptionType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) | types |
| | ) | | |

The documentation for this interface was generated from the following file:
- [IRequestSerializer.cs](../files/IRequestSerializer_8cs.md)
