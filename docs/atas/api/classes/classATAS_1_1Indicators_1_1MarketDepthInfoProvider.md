# ATAS.Indicators.MarketDepthInfoProvider Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.html

A class that implements the IMarketDepthInfoProvider interface to provide market depth information.
 [More...](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#details)

Inheritance diagram for ATAS.Indicators.MarketDepthInfoProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.MarketDepthInfoProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [MarketDepthInfoProvider](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#aa0d3dfb2effcc86b0d293504d4b6d695) ([IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) onlineDataProvider) |
| | Initializes a new instance of the MarketDepthInfoProvider class. |
| | |
| IEnumerable | [GetMarketDepthSnapshot](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#a76a61066eaf4c4eafdc31c38b37452fd) () |
| | Gets a snapshot of the market depth data at the moment of request.ReturnsAn enumerable collection of [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) representing the market depth. |
| | |
| IEnumerable | [GetMarketDepthSnapshot](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a9bff9bf8e37da5f524f5986ee4101d0b) () |
| | Gets a snapshot of the market depth data at the moment of request. |
| | |

| Properties | |
| --- | --- |
| decimal | [CumulativeDomAsks](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#a612f8bb390a79ec0bb01da9963871228)`[get]` |
| | Gets the cumulative sum of the ask volumes in the DOM (Depth of Market). |
| | |
| decimal | [CumulativeDomBids](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#ae1da9888dc3f9bbfada9187facabf56d)`[get]` |
| | Gets the cumulative sum of the bid volumes in the DOM (Depth of Market). |
| | |
| - Properties inherited from [ATAS.Indicators.IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md) | |
| decimal | [CumulativeDomAsks](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a8e2d9b0081d4d19def160d22a35094a5)`[get]` |
| | Gets the cumulative sum of the ask volumes in the DOM (Depth of Market). |
| | |
| decimal | [CumulativeDomBids](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a99b411ad8fa6c0a2f03a04e722f172e2)`[get]` |
| | Gets the cumulative sum of the bid volumes in the DOM (Depth of Market). |
| | |

## Detailed Description

A class that implements the IMarketDepthInfoProvider interface to provide market depth information.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)MarketDepthInfoProvider()

| ATAS.Indicators.MarketDepthInfoProvider.MarketDepthInfoProvider | ( | [IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | onlineDataProvider | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the MarketDepthInfoProvider class.

Parameters

| onlineDataProvider | An implementation of the IOnlineDataProvider interface to retrieve market depth data. |
| --- | --- |

Exceptions

| ArgumentNullException | Thrown if the onlineDataProvider parameter is null. |
| --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetMarketDepthSnapshot()

| IEnumerable ATAS.Indicators.MarketDepthInfoProvider.GetMarketDepthSnapshot | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets a snapshot of the market depth data at the moment of request.ReturnsAn enumerable collection of [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) representing the market depth.

Implements [ATAS.Indicators.IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a9bff9bf8e37da5f524f5986ee4101d0b).

## Property Documentation

## [◆](https://docs.atas.net/en/)CumulativeDomAsks

| decimal ATAS.Indicators.MarketDepthInfoProvider.CumulativeDomAsks |
| --- |

get

Gets the cumulative sum of the ask volumes in the DOM (Depth of Market).

Implements [ATAS.Indicators.IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a8e2d9b0081d4d19def160d22a35094a5).

## [◆](https://docs.atas.net/en/)CumulativeDomBids

| decimal ATAS.Indicators.MarketDepthInfoProvider.CumulativeDomBids |
| --- |

get

Gets the cumulative sum of the bid volumes in the DOM (Depth of Market).

Implements [ATAS.Indicators.IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#a99b411ad8fa6c0a2f03a04e722f172e2).

The documentation for this class was generated from the following file:
- [MarketDepthInfoProvider.cs](../files/MarketDepthInfoProvider_8cs.md)
