# ATAS.Indicators.IMarketDepthInfoProvider Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.html

Interface for providing market depth information.
 [More...](./interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#details)

Inheritance diagram for ATAS.Indicators.IMarketDepthInfoProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| IEnumerable | [GetMarketDepthSnapshot](./interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a9bff9bf8e37da5f524f5986ee4101d0b) () |
| | Gets a snapshot of the market depth data at the moment of request. |
| | |

| Properties | |
| --- | --- |
| decimal | [CumulativeDomAsks](./interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a8e2d9b0081d4d19def160d22a35094a5)`[get]` |
| | Gets the cumulative sum of the ask volumes in the DOM (Depth of Market). |
| | |
| decimal | [CumulativeDomBids](./interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a99b411ad8fa6c0a2f03a04e722f172e2)`[get]` |
| | Gets the cumulative sum of the bid volumes in the DOM (Depth of Market). |
| | |

## Detailed Description

Interface for providing market depth information.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetMarketDepthSnapshot()

| IEnumerable ATAS.Indicators.IMarketDepthInfoProvider.GetMarketDepthSnapshot | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets a snapshot of the market depth data at the moment of request.

ReturnsAn enumerable collection of MarketDataArg representing the market depth.

Implemented in [ATAS.Indicators.MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#a76a61066eaf4c4eafdc31c38b37452fd).

## Property Documentation

## [◆](https://docs.atas.net/en/)CumulativeDomAsks

| decimal ATAS.Indicators.IMarketDepthInfoProvider.CumulativeDomAsks |
| --- |

get

Gets the cumulative sum of the ask volumes in the DOM (Depth of Market).

Implemented in [ATAS.Indicators.MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#a612f8bb390a79ec0bb01da9963871228).

## [◆](https://docs.atas.net/en/)CumulativeDomBids

| decimal ATAS.Indicators.IMarketDepthInfoProvider.CumulativeDomBids |
| --- |

get

Gets the cumulative sum of the bid volumes in the DOM (Depth of Market).

Implemented in [ATAS.Indicators.MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#ae1da9888dc3f9bbfada9187facabf56d).

The documentation for this interface was generated from the following file:
- [MarketDepthInfoProvider.cs](../files/MarketDepthInfoProvider_8cs.md)
