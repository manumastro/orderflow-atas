# ATAS.Indicators.IMarketByOrdersCache Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.html

Interface for manager that provides access to market by orders cache.
 [More...](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md#details)

Inheritance diagram for ATAS.Indicators.IMarketByOrdersCache:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IMarketByOrdersCache:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Additional Inherited Members | |
| --- | --- |
| - Properties inherited from [ATAS.Indicators.ITimeMarketDataCache](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md) | |
| IEnumerable | [CachedItems](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md#a37a8c5fc5d9188118d36d418db355e22)`[get]` |
| | Returns a last items for a period of time specified by CachePeriod. |
| | |
| TimeSpan | [CachePeriod](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md#ae40d7fafc0e196636fb6faad695edf00)`[get]` |
| | Returns the time period for which items are returned by CachedItems. |
| | |
| - Properties inherited from [ATAS.Indicators.IMarketByOrdersDataProvider](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md) | |
| IEnumerable | [MarketByOrders](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md#a38bee1aaeb536bc28e0c15c2940fb777)`[get]` |
| | Gets a snapshot of the current market by order data. |
| | |

## Detailed Description

Interface for manager that provides access to market by orders cache.

The documentation for this interface was generated from the following file:
- [IOnlineDataProvider.cs](../files/IOnlineDataProvider_8cs.md)
