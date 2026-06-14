# ATAS.DataFeedsCore.Order Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Order.html

Represents an order for trading on a financial exchange.
 [More...](./classATAS_1_1DataFeedsCore_1_1Order.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Order:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Order:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1Order.md#add3d16e72bec14afd534d7c25146f0bb) () |
| | Creates a shallow copy of the current order. |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Order.md#a79d320efc536a7e8c6eed83c92277455) () |
| | Returns a string that represents the current order. |
| | |

| Protected Member Functions | |
| --- | --- |
| void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Order.md#a8e7ac97d592072dd541cc0d1db22d946) (string name) |
| | Raises the PropertyChanged event with the specified property name. |
| | |

| Properties | |
| --- | --- |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1Order.md#aa8a5a722c99cc446c0d7ab8cb5238a31)`[get]` |
| | Gets the type of the entity. |
| | |
| string? | [Id](./classATAS_1_1DataFeedsCore_1_1Order.md#ae4fb4587c80a7cc9bcfd3de7638db12c)`[get, set]` |
| | Gets or sets the ID of the order on the exchange. |
| | |
| long | [ExtId](./classATAS_1_1DataFeedsCore_1_1Order.md#a904505d6b96f5d5899a5a0125df5caf0)`[get, set]` |
| | Gets or sets the additional identifier for the order. |
| | |
| long | [UserExtId](./classATAS_1_1DataFeedsCore_1_1Order.md#a3bde42401e926b24961411aceea8f267)`[get, set]` |
| | Gets or sets the user's identifier associated with the order. |
| | |
| string? | [AccountID](./classATAS_1_1DataFeedsCore_1_1Order.md#a52439cd28485be6082c9fd38346a6eb7)`[get, set]` |
| | Gets or sets the ID of the account associated with this order. |
| | |
| string? | [RoutedAccountId](./classATAS_1_1DataFeedsCore_1_1Order.md#add916567326e9bb1c3a84c25ddcb5714)`[get, set]` |
| | Gets or sets the routed account ID for this order. |
| | |
| string? | [SecurityId](./classATAS_1_1DataFeedsCore_1_1Order.md#af320514fe0de6c6b7b6bd2894968aa1b)`[get, set]` |
| | Gets or sets the ID of the security associated with this order. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md)? | [Security](./classATAS_1_1DataFeedsCore_1_1Order.md#a572997c51abfb142fe16044de094cc80)`[get, set]` |
| | Gets or sets the security associated with this order. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md)? | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Order.md#a2db41440a3bb7d7afc07c31bc3db4049)`[get, set]` |
| | Gets or sets the portfolio associated with this order. |
| | |
| [OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) | [Type](./classATAS_1_1DataFeedsCore_1_1Order.md#a39d56f4b7f76950adb99f1299e252169)`[get, set]` |
| | Gets or sets the type of the order. |
| | |
| [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) | [Direction](./classATAS_1_1DataFeedsCore_1_1Order.md#a71ed59e08a3dbd196fa6303251be041f)`[get, set]` |
| | Gets or sets the direction of the order. |
| | |
| decimal | [TriggerPrice](./classATAS_1_1DataFeedsCore_1_1Order.md#a5276c49fb934a3f4b216543e88e87b71)`[get, set]` |
| | Gets or sets the trigger price of the order. |
| | |
| decimal | [Price](./classATAS_1_1DataFeedsCore_1_1Order.md#abf039a3839dd15c686cce7bbd5ab143a)`[get, set]` |
| | Gets or sets the price of the order. |
| | |
| decimal | [QuantityToFill](./classATAS_1_1DataFeedsCore_1_1Order.md#a7fb330bfbde308a1e448d850d370e21d)`[get, set]` |
| | Gets or sets the volume (quantity) to be filled for the order. |
| | |
| decimal | [Unfilled](./classATAS_1_1DataFeedsCore_1_1Order.md#a44c573eb702b2f38ed8b8c80e19ac5a1)`[get, set]` |
| | Gets the remaining unfilled volume of the order. |
| | |
| DateTime? | [ExpiryDate](./classATAS_1_1DataFeedsCore_1_1Order.md#a5497b5ef7f85a2480823acc5b3bd9c26)`[get, set]` |
| | Gets or sets the expiry date of the order. |
| | |
| [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) | [TimeInForce](./classATAS_1_1DataFeedsCore_1_1Order.md#a238fcf825fe5f3c5ccc596b6de6623c6)`[get, set]` |
| | Gets or sets the time in force for the order. |
| | |
| string? | [Route](./classATAS_1_1DataFeedsCore_1_1Order.md#a1d51b1b5231d0eafc143745e07ba72d0)`[get, set]` |
| | Gets or sets the routing information for the order. |
| | |
| string? | [OCOGroup](./classATAS_1_1DataFeedsCore_1_1Order.md#a1888f473511354df5e0c74a9c74b6efb)`[get, set]` |
| | Gets or sets the OCO (One-Cancels-the-Other) group for the order. |
| | |
| string? | [Comment](./classATAS_1_1DataFeedsCore_1_1Order.md#a437b5d562dcda183abd1cdc8b2e490f5)`[get, set]` |
| | Gets or sets the comment for the order. |
| | |
| DateTime | [Time](./classATAS_1_1DataFeedsCore_1_1Order.md#ab79bd40361934b5505edd346fd1f99d9)`[get, set]` |
| | Gets or sets the timestamp of when the order was created or modified. |
| | |
| [OrderStates](../namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716) | [State](./classATAS_1_1DataFeedsCore_1_1Order.md#a5c93a3af2be3df8bdb85944b42eb4527)`[get, set]` |
| | Gets or sets the current state of the order. |
| | |
| [TriggerPriceType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a40608542bbcd7fa29063f394baf3f64d) | [TriggerPriceType](./classATAS_1_1DataFeedsCore_1_1Order.md#abb29e2827080642d0b6912ddea5439ec)`[get, set]` |
| | Gets or sets the type of trigger price associated with the order. |
| | |
| object? | [Parent](./classATAS_1_1DataFeedsCore_1_1Order.md#a2bdc2b7fc8f47c0848873fdee9be6eb5)`[get, set]` |
| | Gets or sets the parent object associated with this order. |
| | |
| bool | [IsInPosition](./classATAS_1_1DataFeedsCore_1_1Order.md#a153303ccafb44a5c60050aa67b929e68)`[get, set]` |
| | Gets or sets a value indicating whether the order is in a position. |
| | |
| bool | [WasActive](./classATAS_1_1DataFeedsCore_1_1Order.md#a908232cd7d4d98db2f27655b6dd6aeb6)`[get, set]` |
| | Gets or sets a value indicating whether the order was active. |
| | |
| bool | [Canceled](./classATAS_1_1DataFeedsCore_1_1Order.md#adf869a13c842cedd0d85c54ba69c1aca)`[get]` |
| | Gets a value indicating whether the order is canceled. |
| | |
| decimal | [AmountBefore](./classATAS_1_1DataFeedsCore_1_1Order.md#a4debb19474e71a014b3f3a75b86fa0cc)`[get, set]` |
| | Gets or sets the amount before the order. |
| | |
| bool? | [IsAttached](./classATAS_1_1DataFeedsCore_1_1Order.md#a1463893b4ea337b6ff5dae6e7ae8edd8)`[get, set]` |
| | Gets or sets a value indicating whether the order is attached to another order. |
| | |
| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? | [ExtendedOptions](./classATAS_1_1DataFeedsCore_1_1Order.md#a4739d437eea15b3c0d2536a4ae1336c6)`[get, set]` |
| | Allows setting special options for the order. Use ISecurityTradingOptions.CreateMarketOrderFlagsObject and similar methods obtained by calling the IDataFeedConnector.GetSecurityTradingOptions method to get the object that you can populate and pass here. |
| | |
| int? | [ExtendedOptionsFlags](./classATAS_1_1DataFeedsCore_1_1Order.md#a363ceec3a6cc0e37d894616a68caedc7)`[get, set]` |
| | Gets or sets the flags representation of the ExtendedOptions. |
| | |
| decimal? | [QuoteVolume](./classATAS_1_1DataFeedsCore_1_1Order.md#a9beb9c617e741554cb266e19d396a427)`[get, set]` |
| | Gets or sets the quote volume associated with the order. |
| | |
| TimeSpan | [Latency](./classATAS_1_1DataFeedsCore_1_1Order.md#a7e2a4b80b50cbb235c0fb76d9f5c3c25)`[get, set]` |
| | Returns the time spent between sending the order to the server and receiving a response about it's registration or cancellation. |
| | |
| bool | [AutoCancel](./classATAS_1_1DataFeedsCore_1_1Order.md#a89a304969ea865e051ee4a40e5545be5)`[get, set]` |
| | Gets or sets a value indicating whether the order must be cancelled on position closed or reverted. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Order.md#a17be8f6a905356edde4fc01eac1b9b6d) |
| | |

## Detailed Description

Represents an order for trading on a financial exchange.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) ATAS.DataFeedsCore.Order.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

Creates a shallow copy of the current order.

ReturnsA new instance of the Order class that is a shallow copy of the current order.

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| void ATAS.DataFeedsCore.Order.OnPropertyChanged | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PropertyChanged event with the specified property name.

Parameters

| name | The name of the property that has changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.Order.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current order.

ReturnsA string representation of the order.

## Property Documentation

## [◆](https://docs.atas.net/en/)AccountID

| string? ATAS.DataFeedsCore.Order.AccountID |
| --- |

getset

Gets or sets the ID of the account associated with this order.

This property represents the ID of the account to which this order is associated.

## [◆](https://docs.atas.net/en/)AmountBefore

| decimal ATAS.DataFeedsCore.Order.AmountBefore |
| --- |

getset

Gets or sets the amount before the order.

## [◆](https://docs.atas.net/en/)AutoCancel

| bool ATAS.DataFeedsCore.Order.AutoCancel |
| --- |

getset

Gets or sets a value indicating whether the order must be cancelled on position closed or reverted.

## [◆](https://docs.atas.net/en/)Canceled

| bool ATAS.DataFeedsCore.Order.Canceled |
| --- |

get

Gets a value indicating whether the order is canceled.

## [◆](https://docs.atas.net/en/)Comment

| string? ATAS.DataFeedsCore.Order.Comment |
| --- |

getset

Gets or sets the comment for the order.

This property represents an optional comment or note associated with the order.

## [◆](https://docs.atas.net/en/)Direction

| [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) ATAS.DataFeedsCore.Order.Direction |
| --- |

getset

Gets or sets the direction of the order.

This property represents the direction of the order, such as buy or sell.

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.Order.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)ExpiryDate

| DateTime? ATAS.DataFeedsCore.Order.ExpiryDate |
| --- |

getset

Gets or sets the expiry date of the order.

If the value is null, the order will be valid until it is manually canceled. If the value is set to Today, the order will be valid for the current trading session. If the value is set to MinValue, the order will be of the FOK (Fill or Kill) type.

## [◆](https://docs.atas.net/en/)ExtendedOptions

| [OrderExtendedOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a01767107bdb7f6c885e50c22f70db191)? ATAS.DataFeedsCore.Order.ExtendedOptions |
| --- |

getset

Allows setting special options for the order. Use ISecurityTradingOptions.CreateMarketOrderFlagsObject and similar methods obtained by calling the IDataFeedConnector.GetSecurityTradingOptions method to get the object that you can populate and pass here.

## [◆](https://docs.atas.net/en/)ExtendedOptionsFlags

| int? ATAS.DataFeedsCore.Order.ExtendedOptionsFlags |
| --- |

getset

Gets or sets the flags representation of the ExtendedOptions.

## [◆](https://docs.atas.net/en/)ExtId

| long ATAS.DataFeedsCore.Order.ExtId |
| --- |

getset

Gets or sets the additional identifier for the order.

## [◆](https://docs.atas.net/en/)Id

| string? ATAS.DataFeedsCore.Order.Id |
| --- |

getset

Gets or sets the ID of the order on the exchange.

## [◆](https://docs.atas.net/en/)IsAttached

| bool? ATAS.DataFeedsCore.Order.IsAttached |
| --- |

getset

Gets or sets a value indicating whether the order is attached to another order.

## [◆](https://docs.atas.net/en/)IsInPosition

| bool ATAS.DataFeedsCore.Order.IsInPosition |
| --- |

getset

Gets or sets a value indicating whether the order is in a position.

## [◆](https://docs.atas.net/en/)Latency

| TimeSpan ATAS.DataFeedsCore.Order.Latency |
| --- |

getset

Returns the time spent between sending the order to the server and receiving a response about it's registration or cancellation.

## [◆](https://docs.atas.net/en/)OCOGroup

| string? ATAS.DataFeedsCore.Order.OCOGroup |
| --- |

getset

Gets or sets the OCO (One-Cancels-the-Other) group for the order.

This property represents the OCO group to which the order belongs, indicating that it is part of an OCO pair.

## [◆](https://docs.atas.net/en/)Parent

| object? ATAS.DataFeedsCore.Order.Parent |
| --- |

getset

Gets or sets the parent object associated with this order.

## [◆](https://docs.atas.net/en/)Portfolio

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md)? ATAS.DataFeedsCore.Order.Portfolio |
| --- |

getset

Gets or sets the portfolio associated with this order.

This property represents the portfolio to which this order belongs.

## [◆](https://docs.atas.net/en/)Price

| decimal ATAS.DataFeedsCore.Order.Price |
| --- |

getset

Gets or sets the price of the order.

This property represents the price at which the order will be executed.

## [◆](https://docs.atas.net/en/)QuantityToFill

| decimal ATAS.DataFeedsCore.Order.QuantityToFill |
| --- |

getset

Gets or sets the volume (quantity) to be filled for the order.

This property represents the quantity of the security to be bought or sold in the order.

## [◆](https://docs.atas.net/en/)QuoteVolume

| decimal? ATAS.DataFeedsCore.Order.QuoteVolume |
| --- |

getset

Gets or sets the quote volume associated with the order.

## [◆](https://docs.atas.net/en/)Route

| string? ATAS.DataFeedsCore.Order.Route |
| --- |

getset

Gets or sets the routing information for the order.

This property represents the routing information for the order, which specifies how the order will be sent to the exchange or market.

## [◆](https://docs.atas.net/en/)RoutedAccountId

| string? ATAS.DataFeedsCore.Order.RoutedAccountId |
| --- |

getset

Gets or sets the routed account ID for this order.

The routed account ID is used to track the specific account to which the order is routed.

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md)? ATAS.DataFeedsCore.Order.Security |
| --- |

getset

Gets or sets the security associated with this order.

This property represents the security (e.g., stock, bond, etc.) to which this order is related.

## [◆](https://docs.atas.net/en/)SecurityId

| string? ATAS.DataFeedsCore.Order.SecurityId |
| --- |

getset

Gets or sets the ID of the security associated with this order.

## [◆](https://docs.atas.net/en/)State

| [OrderStates](../namespaces/namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716) ATAS.DataFeedsCore.Order.State |
| --- |

getset

Gets or sets the current state of the order.

This property represents the current state of the order, such as active, done, failed, etc.

## [◆](https://docs.atas.net/en/)Time

| DateTime ATAS.DataFeedsCore.Order.Time |
| --- |

getset

Gets or sets the timestamp of when the order was created or modified.

This property represents the timestamp indicating when the order was created or last modified.

## [◆](https://docs.atas.net/en/)TimeInForce

| [TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) ATAS.DataFeedsCore.Order.TimeInForce |
| --- |

getset

Gets or sets the time in force for the order.

The time in force determines how long the order will remain active before it is executed or canceled.

## [◆](https://docs.atas.net/en/)TriggerPrice

| decimal ATAS.DataFeedsCore.Order.TriggerPrice |
| --- |

getset

Gets or sets the trigger price of the order.

This property represents the trigger price for the order, which is used in certain types of orders (e.g., stop-loss).

## [◆](https://docs.atas.net/en/)TriggerPriceType

| [TriggerPriceType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a40608542bbcd7fa29063f394baf3f64d) ATAS.DataFeedsCore.Order.TriggerPriceType |
| --- |

getset

Gets or sets the type of trigger price associated with the order.

This property represents the type of trigger price used in certain types of orders (e.g., stop-loss, stop-limit).

## [◆](https://docs.atas.net/en/)Type

| [OrderTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) ATAS.DataFeedsCore.Order.Type |
| --- |

getset

Gets or sets the type of the order.

This property represents the type of the order, such as market order, limit order, etc.

## [◆](https://docs.atas.net/en/)Unfilled

| decimal ATAS.DataFeedsCore.Order.Unfilled |
| --- |

getset

Gets the remaining unfilled volume of the order.

This property represents the remaining quantity of the order that is yet to be executed.

## [◆](https://docs.atas.net/en/)UserExtId

| long ATAS.DataFeedsCore.Order.UserExtId |
| --- |

getset

Gets or sets the user's identifier associated with the order.

## [◆](https://docs.atas.net/en/)WasActive

| bool ATAS.DataFeedsCore.Order.WasActive |
| --- |

getset

Gets or sets a value indicating whether the order was active.

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler? ATAS.DataFeedsCore.Order.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [Order.cs](../files/Order_8cs.md)
