# ATAS.Indicators.IndicatorCandle Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1IndicatorCandle.html

Represents an Indicator Candle.
 [More...](./classATAS_1_1Indicators_1_1IndicatorCandle.md#details)

Inheritance diagram for ATAS.Indicators.IndicatorCandle:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IndicatorCandle:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | | |
| --- | --- | --- |
| | [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a631481685e09bde12e5c5021094b792f) ([ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md) dataprovider, [IIntCandle](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md) parentCandle, decimal tickSize) | |
| | Constructor for the IndicatorCandle class. | |
| | | |
| IEnumerable | [GetAllPriceLevels](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a3b16639dcd407b7c88866fb0d7626179) () | |
| | Gets all available price levels with associated volume information.ReturnsAn enumerable collection of [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) objects representing all price levels. | |
| | | |
| IEnumerable | [GetAllPriceLevels](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a59130af8aacbb781192815a0203fc98e) ([PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) cacheItem) | |
| | Gets all available price levels with associated volume information and caches the data of the last element in the specified cacheItem.Parameters

 cacheItem | A [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object to caching. |

ReturnsAn enumerable collection of [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) objects representing all price levels.

[PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) [GetPriceVolumeInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ae6a64cd0a12f9ff193102b3fc8a6d3d3) (decimal price)
 Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object associated with the specified price.Parameters

| price | The price for which the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object is to be retrieved. |
| --- | --- |

ReturnsThe [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object representing the specified price.

[PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) [GetPriceVolumeInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ad61f9f697cf32cfadac87386814f5fdd) (decimal price, [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) cacheItem)
 Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object associated with the specified price.Parameters

| price | The price for which the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object is to be retrieved. |
| --- | --- |
| cacheItem | A [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object to caching. |

ReturnsThe [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object representing the specified price.

IEnumerable< [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) > [GetAllPriceLevels](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a3572924c4df78d35a16e95d9036cd6d6) ()
 Gets all available price levels with associated volume information.

IEnumerable< [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) > [GetAllPriceLevels](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#ae7e6d627bddcd8f4b6ef666c70a004cc) ([PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) cacheItem)
 Gets all available price levels with associated volume information and caches the data of the last element in the specified cacheItem.

[PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) [GetPriceVolumeInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#aa89fa01ec66955096711071b523be700) (decimal price)
 Gets the PriceVolumeInfo object associated with the specified price.

[PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) [GetPriceVolumeInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a9e96df198214db8befec9d87e6398ffe) (decimal price, [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) cacheItem)
 Gets the PriceVolumeInfo object associated with the specified price.

| Properties | |
| --- | --- |
| decimal | [Open](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ab8a068bcce8a1e8803f764bed3bf657f)`[get]` |
| | The opening price of the candle. |
| | |
| decimal | [High](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a15b5cc326b91813a2fa783bab5cb7120)`[get]` |
| | The highest price in the candle. |
| | |
| decimal | [Low](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a11c4685e2b87b9787b59daac3f5ce5cf)`[get]` |
| | The lowest price in the candle. |
| | |
| decimal | [Close](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ae05f9449970bf106fcccb77f2af502c7)`[get]` |
| | The closing price of the candle. |
| | |
| decimal | [Volume](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a894e04adefc6d3ca2f52c7eec64ea261)`[get]` |
| | The total number of traded lots in the candle. |
| | |
| decimal | [Bid](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a475399940d195b1b8fe1aa200b8e94c9)`[get]` |
| | The number of traded lots at the best bid price in the candle. |
| | |
| decimal | [Ask](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a1bac239c185239c6d212822bb304a807)`[get]` |
| | The number of traded lots at the best offer price in the candle. |
| | |
| decimal | [Betweens](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a8a69c8370c742cdfc5800b81ca3c4704)`[get]` |
| | The number of traded lots at the price between bids and asks in the candle. |
| | |
| decimal | [Ticks](./classATAS_1_1Indicators_1_1IndicatorCandle.md#afaefd8d55ee911f356fb6a3a9340fbe4)`[get]` |
| | The number of price changes in the candle. |
| | |
| decimal | [Delta](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ac5b17ceeb43dc6f8bd159f8c9e049121)`[get]` |
| | The difference between the number of buys and the number of sales in the candle. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [Time](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a4fbc273a950cf14e998204aa6b74f185)`[get]` |
| | Candle opening time. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [LastTime](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a15b797ae49bbf7647514cb36cf66eadb)`[get]` |
| | The time when the last trade in the candle occurred. |
| | |
| decimal | [MaxDelta](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a5496a694ff63727a56de57cca5180664)`[get]` |
| | The maximum value of the delta that was during the period of the candle. |
| | |
| decimal | [MinDelta](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ab774715f9b730387684841fc6e2e01cd)`[get]` |
| | The minimum value of the delta that was during the period of the candle. |
| | |
| decimal | [MaxOI](./classATAS_1_1Indicators_1_1IndicatorCandle.md#aff03ffc123adbaa07128352297a99c0c)`[get]` |
| | The maximum value of open positions that was during the period of the candle. |
| | |
| decimal | [MinOI](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a6ad1ad8638bb6a2c62a37d45a647ce5c)`[get]` |
| | The minimum value of open positions that was during the period of the candle. |
| | |
| decimal | [OI](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a9f1084cdb69193e9e9f0d88369c965e4)`[get]` |
| | The number of open positions in the candle. |
| | |
| decimal | [VWAP](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ab65dd917729169d3278a091a0e1d581a)`[get]` |
| | Volume-weighted average price of the candle. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxVolumePriceInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a6a02e9c11fd7379fa28bac67ebee4c8e)`[get]` |
| | Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum volume. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxTickPriceInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a6fe87b7dae24861c3b842149bf0304ad)`[get]` |
| | Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum tick count. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxAskPriceInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a3ea84ea366e936e347ea82f12bed17e5)`[get]` |
| | Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum ask price. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxBidPriceInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#aaa210fdfc9cd8f3ec1125bb7f5512f85)`[get]` |
| | Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum bid price. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxTimePriceInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#aa8ecffff9340501e4a29a64fbc0a64e7)`[get]` |
| | Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum time. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxPositiveDeltaPriceInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a3c4ef67bb640c8570f16c798a90ed6e2)`[get]` |
| | Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum positive delta. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxNegativeDeltaPriceInfo](./classATAS_1_1Indicators_1_1IndicatorCandle.md#ab322ca90826ea2437379e6101e48496e)`[get]` |
| | Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum negative delta. |
| | |
| [ValueArea](./classATAS_1_1Indicators_1_1ValueArea.md) | [ValueArea](./classATAS_1_1Indicators_1_1IndicatorCandle.md#a81940b0913781df7ca88ac27dc1d0802)`[get]` |
| | Gets the ValueArea object which represents value are of candle. |
| | |
| - Properties inherited from [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md) | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxVolumePriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#af9b5188264c902e12da8d0295f964210)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum volume. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxTickPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a5ccc6700623ab9e43d51500c13a6cf03)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum tick count. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxAskPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#aed1135ed04d5b298b14d4da82a12a9f6)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum ask price. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxBidPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a5c81153c55e81c6ebd98c97879254818)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum bid price. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxTimePriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a2c1cd245e9bbeed69498a8b4b22d917c)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum time. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxPositiveDeltaPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a7a820f17c9d9a66bc6dd4734729d05dc)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum positive delta. |
| | |
| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | [MaxNegativeDeltaPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#af2f8bcd5df31ee4233f82e6b1458dd05)`[get]` |
| | Gets the PriceVolumeInfo object with the maximum negative delta. |
| | |
| [ValueArea](./classATAS_1_1Indicators_1_1ValueArea.md) | [ValueArea](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#abd3610eb19044c1c510c9d5bd92ff431)`[get]` |
| | Gets the ValueArea object which represents value are of candle. |
| | |

## Detailed Description

Represents an Indicator Candle.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)IndicatorCandle()

| ATAS.Indicators.IndicatorCandle.IndicatorCandle | ( | [ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md) | dataprovider, |
| --- | --- | --- | --- |
| | | [IIntCandle](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md) | parentCandle, |
| | | decimal | tickSize |
| | ) | | |

Constructor for the IndicatorCandle class.

Parameters

| dataprovider | An object that provides price information. |
| --- | --- |
| parentCandle | An object that provides information about candle data. |
| tickSize | Minimum minimum price step. |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetAllPriceLevels() [1/2]

| IEnumerable ATAS.Indicators.IndicatorCandle.GetAllPriceLevels | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets all available price levels with associated volume information.ReturnsAn enumerable collection of [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) objects representing all price levels.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a3572924c4df78d35a16e95d9036cd6d6).

## [◆](https://docs.atas.net/en/)GetAllPriceLevels() [2/2]

| IEnumerable ATAS.Indicators.IndicatorCandle.GetAllPriceLevels | ( | [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | cacheItem | ) | |
| --- | --- | --- | --- | --- | --- |

Gets all available price levels with associated volume information and caches the data of the last element in the specified cacheItem.Parameters

| cacheItem | A [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object to caching. |
| --- | --- |

ReturnsAn enumerable collection of [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) objects representing all price levels.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#ae7e6d627bddcd8f4b6ef666c70a004cc).

## [◆](https://docs.atas.net/en/)GetPriceVolumeInfo() [1/2]

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.GetPriceVolumeInfo | ( | decimal | price | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object associated with the specified price.Parameters

| price | The price for which the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object is to be retrieved. |
| --- | --- |

ReturnsThe [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object representing the specified price.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#aa89fa01ec66955096711071b523be700).

## [◆](https://docs.atas.net/en/)GetPriceVolumeInfo() [2/2]

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.GetPriceVolumeInfo | ( | decimal | price, |
| --- | --- | --- | --- |
| | | [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | cacheItem |
| | ) | | |

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object associated with the specified price.Parameters

| price | The price for which the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object is to be retrieved. |
| --- | --- |
| cacheItem | A [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object to caching. |

ReturnsThe [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object representing the specified price.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a9e96df198214db8befec9d87e6398ffe).

## Property Documentation

## [◆](https://docs.atas.net/en/)Ask

| decimal ATAS.Indicators.IndicatorCandle.Ask |
| --- |

get

The number of traded lots at the best offer price in the candle.

## [◆](https://docs.atas.net/en/)Betweens

| decimal ATAS.Indicators.IndicatorCandle.Betweens |
| --- |

get

The number of traded lots at the price between bids and asks in the candle.

## [◆](https://docs.atas.net/en/)Bid

| decimal ATAS.Indicators.IndicatorCandle.Bid |
| --- |

get

The number of traded lots at the best bid price in the candle.

## [◆](https://docs.atas.net/en/)Close

| decimal ATAS.Indicators.IndicatorCandle.Close |
| --- |

get

The closing price of the candle.

## [◆](https://docs.atas.net/en/)Delta

| decimal ATAS.Indicators.IndicatorCandle.Delta |
| --- |

get

The difference between the number of buys and the number of sales in the candle.

## [◆](https://docs.atas.net/en/)High

| decimal ATAS.Indicators.IndicatorCandle.High |
| --- |

get

The highest price in the candle.

## [◆](https://docs.atas.net/en/)LastTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IndicatorCandle.LastTime |
| --- |

get

The time when the last trade in the candle occurred.

## [◆](https://docs.atas.net/en/)Low

| decimal ATAS.Indicators.IndicatorCandle.Low |
| --- |

get

The lowest price in the candle.

## [◆](https://docs.atas.net/en/)MaxAskPriceInfo

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.MaxAskPriceInfo |
| --- |

get

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum ask price.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#aed1135ed04d5b298b14d4da82a12a9f6).

## [◆](https://docs.atas.net/en/)MaxBidPriceInfo

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.MaxBidPriceInfo |
| --- |

get

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum bid price.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a5c81153c55e81c6ebd98c97879254818).

## [◆](https://docs.atas.net/en/)MaxDelta

| decimal ATAS.Indicators.IndicatorCandle.MaxDelta |
| --- |

get

The maximum value of the delta that was during the period of the candle.

## [◆](https://docs.atas.net/en/)MaxNegativeDeltaPriceInfo

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.MaxNegativeDeltaPriceInfo |
| --- |

get

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum negative delta.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#af2f8bcd5df31ee4233f82e6b1458dd05).

## [◆](https://docs.atas.net/en/)MaxOI

| decimal ATAS.Indicators.IndicatorCandle.MaxOI |
| --- |

get

The maximum value of open positions that was during the period of the candle.

## [◆](https://docs.atas.net/en/)MaxPositiveDeltaPriceInfo

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.MaxPositiveDeltaPriceInfo |
| --- |

get

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum positive delta.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a7a820f17c9d9a66bc6dd4734729d05dc).

## [◆](https://docs.atas.net/en/)MaxTickPriceInfo

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.MaxTickPriceInfo |
| --- |

get

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum tick count.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a5ccc6700623ab9e43d51500c13a6cf03).

## [◆](https://docs.atas.net/en/)MaxTimePriceInfo

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.MaxTimePriceInfo |
| --- |

get

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum time.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#a2c1cd245e9bbeed69498a8b4b22d917c).

## [◆](https://docs.atas.net/en/)MaxVolumePriceInfo

| [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) ATAS.Indicators.IndicatorCandle.MaxVolumePriceInfo |
| --- |

get

Gets the [PriceVolumeInfo](./classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object with the maximum volume.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#af9b5188264c902e12da8d0295f964210).

## [◆](https://docs.atas.net/en/)MinDelta

| decimal ATAS.Indicators.IndicatorCandle.MinDelta |
| --- |

get

The minimum value of the delta that was during the period of the candle.

## [◆](https://docs.atas.net/en/)MinOI

| decimal ATAS.Indicators.IndicatorCandle.MinOI |
| --- |

get

The minimum value of open positions that was during the period of the candle.

## [◆](https://docs.atas.net/en/)OI

| decimal ATAS.Indicators.IndicatorCandle.OI |
| --- |

get

The number of open positions in the candle.

## [◆](https://docs.atas.net/en/)Open

| decimal ATAS.Indicators.IndicatorCandle.Open |
| --- |

get

The opening price of the candle.

## [◆](https://docs.atas.net/en/)Ticks

| decimal ATAS.Indicators.IndicatorCandle.Ticks |
| --- |

get

The number of price changes in the candle.

## [◆](https://docs.atas.net/en/)Time

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IndicatorCandle.Time |
| --- |

get

Candle opening time.

## [◆](https://docs.atas.net/en/)ValueArea

| [ValueArea](./classATAS_1_1Indicators_1_1ValueArea.md) ATAS.Indicators.IndicatorCandle.ValueArea |
| --- |

get

Gets the ValueArea object which represents value are of candle.

Implements [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#abd3610eb19044c1c510c9d5bd92ff431).

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.Indicators.IndicatorCandle.Volume |
| --- |

get

The total number of traded lots in the candle.

## [◆](https://docs.atas.net/en/)VWAP

| decimal ATAS.Indicators.IndicatorCandle.VWAP |
| --- |

get

Volume-weighted average price of the candle.

The documentation for this class was generated from the following file:
- [IndicatorCandle.cs](../files/IndicatorCandle_8cs.md)
