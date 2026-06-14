# ATAS.Indicators.ICandleCreator Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1ICandleCreator.html

Represents an interface for creating and managing indicator candles.
 [More...](./interfaceATAS_1_1Indicators_1_1ICandleCreator.md#details)

| Properties | |
| --- | --- |
| string | [Name](./interfaceATAS_1_1Indicators_1_1ICandleCreator.md#a2e4fb24d1d3e03a19a360908979c2e05)`[get]` |
| | Gets the name of the candle creator. |
| | |
| List | [Candles](./interfaceATAS_1_1Indicators_1_1ICandleCreator.md#a1470ec3182b430cf51086c2e172e567e)`[get]` |
| | Gets the list of IndicatorCandle created by the candle creator. |
| | |

| Events | |
| --- | --- |
| Action | [OnNewCandle](./interfaceATAS_1_1Indicators_1_1ICandleCreator.md#a40a7dee8d72a3a003863dc51684128f4) |
| | Event raised when a new IndicatorCandle is created. |
| | |
| Action | [OnRemoveCandle](./interfaceATAS_1_1Indicators_1_1ICandleCreator.md#a72bdfc60b1b67c16f6e564bef8fb4668) |
| | Event raised when an IndicatorCandle is removed. |
| | |
| Action | [OnCandleChanged](./interfaceATAS_1_1Indicators_1_1ICandleCreator.md#a42945f29a10cacc9710f82e7a4972f8d) |
| | Event raised when an IndicatorCandle is changed. |
| | |

## Detailed Description

Represents an interface for creating and managing indicator candles.

## Property Documentation

## [◆](https://docs.atas.net/en/)Candles

| List ATAS.Indicators.ICandleCreator.Candles |
| --- |

get

Gets the list of IndicatorCandle created by the candle creator.

## [◆](https://docs.atas.net/en/)Name

| string ATAS.Indicators.ICandleCreator.Name |
| --- |

get

Gets the name of the candle creator.

## Event Documentation

## [◆](https://docs.atas.net/en/)OnCandleChanged

| Action ATAS.Indicators.ICandleCreator.OnCandleChanged |
| --- |

Event raised when an IndicatorCandle is changed.

## [◆](https://docs.atas.net/en/)OnNewCandle

| Action ATAS.Indicators.ICandleCreator.OnNewCandle |
| --- |

Event raised when a new IndicatorCandle is created.

## [◆](https://docs.atas.net/en/)OnRemoveCandle

| Action ATAS.Indicators.ICandleCreator.OnRemoveCandle |
| --- |

Event raised when an IndicatorCandle is removed.

The documentation for this interface was generated from the following file:
- [ICandleCreator.cs](../files/ICandleCreator_8cs.md)
