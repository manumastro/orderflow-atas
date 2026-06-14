# ATAS.Indicators.ISupportedPriceInfo Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.html

Represents an interface for supporting price information.
 [More...](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#details)

Inheritance diagram for ATAS.Indicators.ISupportedPriceInfo:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| IEnumerable | [GetAllPriceLevels](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a3572924c4df78d35a16e95d9036cd6d6) () |
| | Gets all available price levels with associated volume information. |
| | |
| IEnumerable | [GetAllPriceLevels](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#ae7e6d627bddcd8f4b6ef666c70a004cc) ([PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) cacheItem) |
| | Gets all available price levels with associated volume information and caches the data of the last element in the specified cacheItem. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [GetPriceVolumeInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#aa89fa01ec66955096711071b523be700) (decimal price) |
| | Gets the PriceVolumeInfo object associated with the specified price. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [GetPriceVolumeInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a9e96df198214db8befec9d87e6398ffe) (decimal price, [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) cacheItem) |
| | Gets the PriceVolumeInfo object associated with the specified price. |
| | |

| Properties | |
| --- | --- |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxVolumePriceInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#af9b5188264c902e12da8d0295f964210)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum volume. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxTickPriceInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a5ccc6700623ab9e43d51500c13a6cf03)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum tick count. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxAskPriceInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#aed1135ed04d5b298b14d4da82a12a9f6)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum ask price. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxBidPriceInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a5c81153c55e81c6ebd98c97879254818)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum bid price. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxTimePriceInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a2c1cd245e9bbeed69498a8b4b22d917c)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum time. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxPositiveDeltaPriceInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a7a820f17c9d9a66bc6dd4734729d05dc)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum positive delta. |
| | |
| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxNegativeDeltaPriceInfo](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#af2f8bcd5df31ee4233f82e6b1458dd05)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum negative delta. |
| | |
| [ValueArea](../classes/classATAS_1_1Indicators_1_1ValueArea.md) | [ValueArea](./interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#abd3610eb19044c1c510c9d5bd92ff431)`[get]` |
| | Gets the ValueArea object which represents value are of candle. |
| | |

## Detailed Description

Represents an interface for supporting price information.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetAllPriceLevels() [1/2]

| IEnumerable ATAS.Indicators.ISupportedPriceInfo.GetAllPriceLevels | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets all available price levels with associated volume information.

ReturnsAn enumerable collection of PriceVolumeInfo objects representing all price levels.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3b16639dcd407b7c88866fb0d7626179).

## [◆](https://docs.atas.net/en/)GetAllPriceLevels() [2/2]

| IEnumerable ATAS.Indicators.ISupportedPriceInfo.GetAllPriceLevels | ( | [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | cacheItem | ) | |
| --- | --- | --- | --- | --- | --- |

Gets all available price levels with associated volume information and caches the data of the last element in the specified cacheItem.

Parameters

| cacheItem | A PriceVolumeInfo object to caching. |
| --- | --- |

ReturnsAn enumerable collection of PriceVolumeInfo objects representing all price levels.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a59130af8aacbb781192815a0203fc98e).

## [◆](https://docs.atas.net/en/)GetPriceVolumeInfo() [1/2]

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.GetPriceVolumeInfo | ( | decimal | price | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the PriceVolumeInfo object associated with the specified price.

Parameters

| price | The price for which the PriceVolumeInfo object is to be retrieved. |
| --- | --- |

ReturnsThe PriceVolumeInfo object representing the specified price.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ae6a64cd0a12f9ff193102b3fc8a6d3d3).

## [◆](https://docs.atas.net/en/)GetPriceVolumeInfo() [2/2]

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.GetPriceVolumeInfo | ( | decimal | price, |
| --- | --- | --- | --- |
| | | [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | cacheItem |
| | ) | | |

Gets the PriceVolumeInfo object associated with the specified price.

Parameters

| price | The price for which the PriceVolumeInfo object is to be retrieved. |
| --- | --- |
| cacheItem | A PriceVolumeInfo object to caching. |

ReturnsThe PriceVolumeInfo object representing the specified price.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ad61f9f697cf32cfadac87386814f5fdd).

## Property Documentation

## [◆](https://docs.atas.net/en/)MaxAskPriceInfo

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.MaxAskPriceInfo |
| --- |

get

Gets the PriceVolumeInfo object with the maximum ask price.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3ea84ea366e936e347ea82f12bed17e5).

## [◆](https://docs.atas.net/en/)MaxBidPriceInfo

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.MaxBidPriceInfo |
| --- |

get

Gets the PriceVolumeInfo object with the maximum bid price.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#aaa210fdfc9cd8f3ec1125bb7f5512f85).

## [◆](https://docs.atas.net/en/)MaxNegativeDeltaPriceInfo

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.MaxNegativeDeltaPriceInfo |
| --- |

get

Gets the PriceVolumeInfo object with the maximum negative delta.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ab322ca90826ea2437379e6101e48496e).

## [◆](https://docs.atas.net/en/)MaxPositiveDeltaPriceInfo

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.MaxPositiveDeltaPriceInfo |
| --- |

get

Gets the PriceVolumeInfo object with the maximum positive delta.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3c4ef67bb640c8570f16c798a90ed6e2).

## [◆](https://docs.atas.net/en/)MaxTickPriceInfo

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.MaxTickPriceInfo |
| --- |

get

Gets the PriceVolumeInfo object with the maximum tick count.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6fe87b7dae24861c3b842149bf0304ad).

## [◆](https://docs.atas.net/en/)MaxTimePriceInfo

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.MaxTimePriceInfo |
| --- |

get

Gets the PriceVolumeInfo object with the maximum time.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#aa8ecffff9340501e4a29a64fbc0a64e7).

## [◆](https://docs.atas.net/en/)MaxVolumePriceInfo

| [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.ISupportedPriceInfo.MaxVolumePriceInfo |
| --- |

get

Gets the PriceVolumeInfo object with the maximum volume.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6a02e9c11fd7379fa28bac67ebee4c8e).

## [◆](https://docs.atas.net/en/)ValueArea

| [ValueArea](../classes/classATAS_1_1Indicators_1_1ValueArea.md) ATAS.Indicators.ISupportedPriceInfo.ValueArea |
| --- |

get

Gets the ValueArea object which represents value are of candle.

Implemented in [ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a81940b0913781df7ca88ac27dc1d0802).

The documentation for this interface was generated from the following file:
- [PriceVolumeInfo.cs](../files/PriceVolumeInfo_8cs.md)
