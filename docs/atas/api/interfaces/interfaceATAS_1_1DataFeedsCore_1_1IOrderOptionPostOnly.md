# ATAS.DataFeedsCore.IOrderOptionPostOnly Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.html

Represents an order option for placing a post-only order.
 [More...](./interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.md#details)

| Properties | |
| --- | --- |
| bool | [PostOnly](./interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.md#a984e1ff43ac2c68102760e53ce1b71f8)`[get, set]` |
| | Gets or sets a value indicating whether the order is post-only. When set to `true`, the order will only be placed if it can be added to the order book as a maker order. |
| | |

## Detailed Description

Represents an order option for placing a post-only order.

## Property Documentation

## [◆](https://docs.atas.net/en/)PostOnly

| bool ATAS.DataFeedsCore.IOrderOptionPostOnly.PostOnly |
| --- |

getset

Gets or sets a value indicating whether the order is post-only. When set to `true`, the order will only be placed if it can be added to the order book as a maker order.

The documentation for this interface was generated from the following file:
- [ConnectorTradingOptions.cs](../files/ConnectorTradingOptions_8cs.md)
