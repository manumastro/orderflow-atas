# ATAS.DataFeedsCore.MarketByOrder Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1MarketByOrder.html

Market by Order (MBO) describes an order-based data feed that provides the ability to view individual queue position, full depth of book and the size of individual orders at each price level.
 [More...](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#details)

Inheritance diagram for ATAS.DataFeedsCore.MarketByOrder:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.MarketByOrder:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a404805ec75ff577775a5815dac36eb4d) () |
| | |

| Properties | |
| --- | --- |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md)? | [Security](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#ae63739f9b55f568eb01c317b48e300ad)`[get, set]` |
| | Gets or sets the security associated with the market by order entry. |
| | |
| DateTime | [Time](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a492b23803210abaff252c6015f70f850)`[get, set]` |
| | Gets or sets the date and time of the market by order entry. |
| | |
| [MarketByOrderUpdateTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617) | [Type](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#aaa322eb2c52aa8be0e7cdc58ae6059e7)`[get, set]` |
| | Type of market by order update. |
| | |
| [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) | [Side](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#ab1706f37dd78eecb0143a726927f1dcc)`[get, set]` |
| | Side of the order. |
| | |
| long | [Priority](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a80766904fccdf9c041ada9e5e16be5fc)`[get, set]` |
| | Priority of this order in the exchange's matching engine queue. |
| | |
| long | [ExchangeOrderId](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a889d5ef746b506ef536c21e6f89f9d04)`[get, set]` |
| | Exchange order id of this order. |
| | |
| decimal | [Price](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a76c45d926f6a2b9a7177a92125903ecd)`[get, set]` |
| | Price associated with this order. |
| | |
| decimal | [Volume](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a141c57af356e8b960de5a7924f0e3025)`[get, set]` |
| | Volume associated with this order. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

## Detailed Description

Market by Order (MBO) describes an order-based data feed that provides the ability to view individual queue position, full depth of book and the size of individual orders at each price level.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.MarketByOrder.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)ExchangeOrderId

| long ATAS.DataFeedsCore.MarketByOrder.ExchangeOrderId |
| --- |

getset

Exchange order id of this order.

## [◆](https://docs.atas.net/en/)Price

| decimal ATAS.DataFeedsCore.MarketByOrder.Price |
| --- |

getset

Price associated with this order.

## [◆](https://docs.atas.net/en/)Priority

| long ATAS.DataFeedsCore.MarketByOrder.Priority |
| --- |

getset

Priority of this order in the exchange's matching engine queue.

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md)? ATAS.DataFeedsCore.MarketByOrder.Security |
| --- |

getset

Gets or sets the security associated with the market by order entry.

## [◆](https://docs.atas.net/en/)Side

| [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) ATAS.DataFeedsCore.MarketByOrder.Side |
| --- |

getset

Side of the order.

## [◆](https://docs.atas.net/en/)Time

| DateTime ATAS.DataFeedsCore.MarketByOrder.Time |
| --- |

getset

Gets or sets the date and time of the market by order entry.

## [◆](https://docs.atas.net/en/)Type

| [MarketByOrderUpdateTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617) ATAS.DataFeedsCore.MarketByOrder.Type |
| --- |

getset

Type of market by order update.

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.MarketByOrder.Volume |
| --- |

getset

Volume associated with this order.

The documentation for this class was generated from the following file:
- [MarketByOrder.cs](../files/MarketByOrder_8cs.md)
