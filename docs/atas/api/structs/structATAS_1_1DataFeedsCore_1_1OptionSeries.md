# ATAS.DataFeedsCore.OptionSeries Struct Reference

Source: https://docs.atas.net/en/structATAS_1_1DataFeedsCore_1_1OptionSeries.html

| Public Member Functions | |
| --- | --- |
| override string | [ToString](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a3b9f0218e2e7eb7541ee32a35a9d08fa) () |
| | |

| Static Public Member Functions | |
| --- | --- |
| static [OptionSeriesType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09) | [GetWeeklySeriesType](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a7cedbf6bbabb800197c307f771272246) (DateTime expiration, HashSet maxExpirationsByMonth) |
| | Determines the type of weekly option series based on expiration date. Returns EndOfMonth if the expiration is the maximum within its month, otherwise Weekly. |
| | |
| static HashSet | [GetMaxExpirationsByMonth](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a1f9446eb0bc5b2bf59fc46e6a7151d6c) (IEnumerable expirations) |
| | Builds a set of maximum expiration dates per month from a collection of expiration dates. |
| | |

| Properties | |
| --- | --- |
| required string | [Code](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a3e7901406d8bc61ecebf54b36a7c6ac1)`[get]` |
| | |
| required string | [Exchange](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a7b0d0902ac79df41caca8795d0fc690f)`[get]` |
| | |
| required string | [UnderlyingCode](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a8dc80b18fe34f1a06d3a7acc79df608d)`[get]` |
| | |
| required DateTime | [Expiration](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a901bc3f57123174197cc6506047f0168)`[get]` |
| | |
| required [OptionSeriesType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09) | [Type](./structATAS_1_1DataFeedsCore_1_1OptionSeries.md#a2dc69cb8a9a88b8163cf4846307330e6)`[get]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetMaxExpirationsByMonth()

| static HashSet ATAS.DataFeedsCore.OptionSeries.GetMaxExpirationsByMonth | ( | IEnumerable | expirations | ) | |
| --- | --- | --- | --- | --- | --- |

static

Builds a set of maximum expiration dates per month from a collection of expiration dates.

## [◆](https://docs.atas.net/en/)GetWeeklySeriesType()

| static [OptionSeriesType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09) ATAS.DataFeedsCore.OptionSeries.GetWeeklySeriesType | ( | DateTime | expiration, |
| --- | --- | --- | --- |
| | | HashSet | maxExpirationsByMonth |
| | ) | | |

static

Determines the type of weekly option series based on expiration date. Returns EndOfMonth if the expiration is the maximum within its month, otherwise Weekly.

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.OptionSeries.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Code

| required string ATAS.DataFeedsCore.OptionSeries.Code |
| --- |

get

## [◆](https://docs.atas.net/en/)Exchange

| required string ATAS.DataFeedsCore.OptionSeries.Exchange |
| --- |

get

## [◆](https://docs.atas.net/en/)Expiration

| required DateTime ATAS.DataFeedsCore.OptionSeries.Expiration |
| --- |

get

## [◆](https://docs.atas.net/en/)Type

| required [OptionSeriesType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09) ATAS.DataFeedsCore.OptionSeries.Type |
| --- |

get

## [◆](https://docs.atas.net/en/)UnderlyingCode

| required string ATAS.DataFeedsCore.OptionSeries.UnderlyingCode |
| --- |

get

The documentation for this struct was generated from the following file:
- [IOptionsDataFeed.cs](../files/IOptionsDataFeed_8cs.md)
