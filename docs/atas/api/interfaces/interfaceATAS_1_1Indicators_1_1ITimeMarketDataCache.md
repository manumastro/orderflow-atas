# ATAS.Indicators.ITimeMarketDataCache< out T > Interface Template Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.html

Cache that holds recent items based on a specified amount of time.
 [More...](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md#details)

| Properties | |
| --- | --- |
| IEnumerable | [CachedItems](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md#a37a8c5fc5d9188118d36d418db355e22)`[get]` |
| | Returns a last items for a period of time specified by CachePeriod. |
| | |
| [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | [CachePeriod](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md#ae40d7fafc0e196636fb6faad695edf00)`[get]` |
| | Returns the time period for which items are returned by CachedItems. |
| | |

## Detailed Description

Cache that holds recent items based on a specified amount of time.

## Property Documentation

## [◆](https://docs.atas.net/en/)CachedItems

| IEnumerable [ATAS.Indicators.ITimeMarketDataCache](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md).CachedItems |
| --- |

get

Returns a last items for a period of time specified by CachePeriod.

## [◆](https://docs.atas.net/en/)CachePeriod

| [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) [ATAS.Indicators.ITimeMarketDataCache](./interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md).CachePeriod |
| --- |

get

Returns the time period for which items are returned by CachedItems.

The documentation for this interface was generated from the following file:
- [IOnlineDataProvider.cs](../files/IOnlineDataProvider_8cs.md)
