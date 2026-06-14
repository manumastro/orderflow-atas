# ATAS.DataFeedsCore.MyTrade Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1MyTrade.html

Represents a trade entity in the system.
 [More...](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#details)

Inheritance diagram for ATAS.DataFeedsCore.MyTrade:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.MyTrade:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#af10fb846368f0eae411f0fb6d446168b) () |
| | |
| string | [Display](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a4bd45f20954e8e7dd963f932238bbf65) () |
| | |
| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a42b14a2daafcbc93a44b0d7562e89905) () |
| | |

| Protected Member Functions | |
| --- | --- |
| void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a3be4092f42035ab816356a4f5c35ac37) (string name) |
| | Notifies subscribers that a property value has changed. |
| | |

| Properties | |
| --- | --- |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a306f8c52e513aca851b1b8c39ed0d17b)`[get]` |
| | |
| string | [Route](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a01c21c3039bb9315bec8b8ebb3a9224c)`[get, set]` |
| | Gets or sets the route associated with the trade. |
| | |
| string | [AccountID](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#adf5ad1b14a9c71700e7ae4eb6b18138d)`[get, set]` |
| | Gets or sets the account ID associated with the trade. |
| | |
| string | [SecurityId](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a16ae59143aeb5b021180d3bd7efd05b3)`[get, set]` |
| | Gets or sets the security ID associated with the trade. |
| | |
| string | [Id](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a2a1645dd626a671353ca4adc359ab697)`[get, set]` |
| | Gets or sets the unique ID of the trade. |
| | |
| string | [OrderId](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#ac54cd783a655249b56257a26f725d9c1)`[get, set]` |
| | Gets or sets the ID of the order associated with the trade. |
| | |
| [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) | [OrderDirection](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a79998b450e1aabc7435a998228f3af03)`[get, set]` |
| | Gets or sets the direction of the order (Buy or Sell) associated with the trade. |
| | |
| decimal | [Price](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a333eb95708dfe05f9facda23d9f8d40f)`[get, set]` |
| | Gets or sets the price at which the trade was executed. |
| | |
| DateTime | [Time](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#ae1038f76bacdc28cef3b5a2b9db8f869)`[get, set]` |
| | Gets or sets the timestamp of the trade. |
| | |
| decimal | [Volume](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#ac97f54584d2471e9f72153026aec55bd)`[get, set]` |
| | Gets or sets the volume of the trade. |
| | |
| decimal | [OpenVolume](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#aa7ebf2e5dc4e9be90da0d3783fd6d506)`[get, set]` |
| | Gets or sets the open volume of the trade. |
| | |
| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | [Order](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a690a1084c15075693e009303c95f9b1c)`[get, set]` |
| | Gets or sets the order associated with the trade. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a20af88ae58deca3d0550e83f0eda7595)`[get, set]` |
| | Gets or sets the security associated with the trade. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a3ffe41334bb93b523697e84b4181c10c)`[get, set]` |
| | Gets or sets the portfolio associated with the trade. |
| | |
| object | [Parent](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a3f3ecfdc0824817838ccef7cb1be74bb)`[get, set]` |
| | Gets or sets the parent object associated with the trade. |
| | |
| decimal? | [Commission](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a0300231a4231c8531fc6893413b1fa92)`[get, set]` |
| | Gets or sets the commission amount of the trade. |
| | |
| string? | [CommissionCurrency](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#abce2538b14d5f57b4af47ce4b247c75b)`[get, set]` |
| | Gets or sets the currency of the commission for the trade. |
| | |
| bool? | [IsMaker](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#ac639983f108a5d1b5d47714545f995a3)`[get, set]` |
| | Gets or sets whether the trade is a maker trade. |
| | |
| bool | [IsNew](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#a5b46f3122a808d8a6d3a257bd4ce501a)`[get, set]` |
| | Gets or sets a flag indicating whether the trade is new. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1MyTrade.md#ad8c4cf0bf5ff77399b980766ee925687) |
| | |

## Detailed Description

Represents a trade entity in the system.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) ATAS.DataFeedsCore.MyTrade.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Display()

| string ATAS.DataFeedsCore.MyTrade.Display | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| void ATAS.DataFeedsCore.MyTrade.OnPropertyChanged | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Notifies subscribers that a property value has changed.

Parameters

| name | The name of the property that changed.

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.MyTrade.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AccountID

| string ATAS.DataFeedsCore.MyTrade.AccountID |
| --- |

getset

Gets or sets the account ID associated with the trade.

## [◆](https://docs.atas.net/en/)Commission

| decimal? ATAS.DataFeedsCore.MyTrade.Commission |
| --- |

getset

Gets or sets the commission amount of the trade.

## [◆](https://docs.atas.net/en/)CommissionCurrency

| string? ATAS.DataFeedsCore.MyTrade.CommissionCurrency |
| --- |

getset

Gets or sets the currency of the commission for the trade.

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.MyTrade.EntityType |
| --- |

get

Gets the entity type, which is EntityType.MyTrade.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)Id

| string ATAS.DataFeedsCore.MyTrade.Id |
| --- |

getset

Gets or sets the unique ID of the trade.

## [◆](https://docs.atas.net/en/)IsMaker

| bool? ATAS.DataFeedsCore.MyTrade.IsMaker |
| --- |

getset

Gets or sets whether the trade is a maker trade.

## [◆](https://docs.atas.net/en/)IsNew

| bool ATAS.DataFeedsCore.MyTrade.IsNew |
| --- |

getset

Gets or sets a flag indicating whether the trade is new.

## [◆](https://docs.atas.net/en/)OpenVolume

| decimal ATAS.DataFeedsCore.MyTrade.OpenVolume |
| --- |

getset

Gets or sets the open volume of the trade.

## [◆](https://docs.atas.net/en/)Order

| [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) ATAS.DataFeedsCore.MyTrade.Order |
| --- |

getset

Gets or sets the order associated with the trade.

## [◆](https://docs.atas.net/en/)OrderDirection

| [OrderDirections](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) ATAS.DataFeedsCore.MyTrade.OrderDirection |
| --- |

getset

Gets or sets the direction of the order (Buy or Sell) associated with the trade.

## [◆](https://docs.atas.net/en/)OrderId

| string ATAS.DataFeedsCore.MyTrade.OrderId |
| --- |

getset

Gets or sets the ID of the order associated with the trade.

## [◆](https://docs.atas.net/en/)Parent

| object ATAS.DataFeedsCore.MyTrade.Parent |
| --- |

getset

Gets or sets the parent object associated with the trade.

## [◆](https://docs.atas.net/en/)Portfolio

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.DataFeedsCore.MyTrade.Portfolio |
| --- |

getset

Gets or sets the portfolio associated with the trade.

## [◆](https://docs.atas.net/en/)Price

| decimal ATAS.DataFeedsCore.MyTrade.Price |
| --- |

getset

Gets or sets the price at which the trade was executed.

## [◆](https://docs.atas.net/en/)Route

| string ATAS.DataFeedsCore.MyTrade.Route |
| --- |

getset

Gets or sets the route associated with the trade.

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.MyTrade.Security |
| --- |

getset

Gets or sets the security associated with the trade.

## [◆](https://docs.atas.net/en/)SecurityId

| string ATAS.DataFeedsCore.MyTrade.SecurityId |
| --- |

getset

Gets or sets the security ID associated with the trade.

## [◆](https://docs.atas.net/en/)Time

| DateTime ATAS.DataFeedsCore.MyTrade.Time |
| --- |

getset

Gets or sets the timestamp of the trade.

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.MyTrade.Volume |
| --- |

getset

Gets or sets the volume of the trade.

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.MyTrade.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [MyTrade.cs](../files/MyTrade_8cs.md)
