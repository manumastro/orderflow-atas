# ATAS.DataFeedsCore.ISecurityTradingOptions Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.html

Represents an interface for providing trading options related to a security or trading connector.
 [More...](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#details)

Inheritance diagram for ATAS.DataFeedsCore.ISecurityTradingOptions:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? | [CreateMarketOrderFlagsObject](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#ab856f9f21f7024b75d2fb2672faf864c) () |
| | Gets an object of all possible marker order options passed as Order.ExtendedOptions. |
| | |
| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? | [CreateLimitOrderFlagsObject](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#ac342a226ef21e254feaa32094b5ccc87) () |
| | Gets an object of all possible limit order options passed as Order.ExtendedOptions. |
| | |
| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? | [CreateConditionalMarketOrderFlagsObject](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#ac56789f82aa3c93b24bd749404b6ad73) () |
| | Gets an object of all possible conditional market (stop) order options passed as Order.ExtendedOptions. |
| | |
| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? | [CreateConditionalLimitOrderFlagsObject](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#a6fd62ee343d256ad0f9d07da47bb41af) () |
| | Gets an object of all possible conditional limit (stop) order options passed as Order.ExtendedOptions. |
| | |

| Properties | |
| --- | --- |
| [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) | [TimeInForce](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#af79a92c439f05a0dd80d8238f0d5eaa5)`[get]` |
| | Gets a value which contains all possible TimeInForce values supported by the connector when opening an order. |
| | |
| [TriggerPriceType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a40608542bbcd7fa29063f394baf3f64d) | [TriggerPriceTypes](./interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#aace7262a4169b07e60cb3e7daae5b74a)`[get]` |
| | Gets a value which contains all possible TriggerPriceTypes when opening a conditional order by the connector. |
| | |

## Detailed Description

Represents an interface for providing trading options related to a security or trading connector.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CreateConditionalLimitOrderFlagsObject()

| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? ATAS.DataFeedsCore.ISecurityTradingOptions.CreateConditionalLimitOrderFlagsObject | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets an object of all possible conditional limit (stop) order options passed as Order.ExtendedOptions.

Implemented in [ATAS.DataFeedsCore.SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md#af5c96205e1f818edd927d2fb938bcc4c).

## [◆](https://docs.atas.net/en/)CreateConditionalMarketOrderFlagsObject()

| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? ATAS.DataFeedsCore.ISecurityTradingOptions.CreateConditionalMarketOrderFlagsObject | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets an object of all possible conditional market (stop) order options passed as Order.ExtendedOptions.

Implemented in [ATAS.DataFeedsCore.SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md#a6effbdb582747ea9f91eab1360ec12dc).

## [◆](https://docs.atas.net/en/)CreateLimitOrderFlagsObject()

| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? ATAS.DataFeedsCore.ISecurityTradingOptions.CreateLimitOrderFlagsObject | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets an object of all possible limit order options passed as Order.ExtendedOptions.

Implemented in [ATAS.DataFeedsCore.SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md#ad12c21b19573a2be3508381e5cd4c9ce).

## [◆](https://docs.atas.net/en/)CreateMarketOrderFlagsObject()

| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? ATAS.DataFeedsCore.ISecurityTradingOptions.CreateMarketOrderFlagsObject | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets an object of all possible marker order options passed as Order.ExtendedOptions.

Implemented in [ATAS.DataFeedsCore.SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md#ad40f16a23d4d4e8f88482f851cae202f).

## Property Documentation

## [◆](https://docs.atas.net/en/)TimeInForce

| [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) ATAS.DataFeedsCore.ISecurityTradingOptions.TimeInForce |
| --- |

get

Gets a value which contains all possible TimeInForce values supported by the connector when opening an order.

Implemented in [ATAS.DataFeedsCore.SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md#ae7819b9de64e4cd31ff651d6ade3f2de).

## [◆](https://docs.atas.net/en/)TriggerPriceTypes

| [TriggerPriceType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a40608542bbcd7fa29063f394baf3f64d) ATAS.DataFeedsCore.ISecurityTradingOptions.TriggerPriceTypes |
| --- |

get

Gets a value which contains all possible TriggerPriceTypes when opening a conditional order by the connector.

Implemented in [ATAS.DataFeedsCore.SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md#ab1235308d8e4fd264070f728efdca0e9).

The documentation for this interface was generated from the following file:
- [ConnectorTradingOptions.cs](../files/ConnectorTradingOptions_8cs.md)
