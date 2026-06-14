# ATAS.DataFeedsCore.Money Struct Reference

Source: https://docs.atas.net/en/structATAS_1_1DataFeedsCore_1_1Money.html

Represents decimal amount in some currency.
 [More...](./structATAS_1_1DataFeedsCore_1_1Money.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Money:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Money:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md#ab889d1f065900c5cd919bf4d75cf2148) () |
| | |
| | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md#a8a328a7338a17a67d5594bed40c584fb) (decimal value, [CurrencyInfo](./structATAS_1_1DataFeedsCore_1_1Money.md#a980c6b44e4ba0c9999f7a40069ef020d) currency) |
| | |
| override string | [ToString](./structATAS_1_1DataFeedsCore_1_1Money.md#adb9017f1111506b533a0c4d4a57713a0) () |
| | |
| string | [ToString](./structATAS_1_1DataFeedsCore_1_1Money.md#ae368d8b5dd7e2580b31b302134aaff94) (string? format, IFormatProvider? formatProvider) |
| | |

| Static Public Member Functions | |
| --- | --- |
| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | [operator-](./structATAS_1_1DataFeedsCore_1_1Money.md#ad70ee10266b6438b1e9e2ed9bdde2015) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m1) |
| | |
| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | [operator+](./structATAS_1_1DataFeedsCore_1_1Money.md#a9d4c11c871390149eb64ac39b3e8a588) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m1, [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m2) |
| | |
| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | [operator-](./structATAS_1_1DataFeedsCore_1_1Money.md#ae3e26cd33ff9eea44bbe85c548790df5) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m1, [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m2) |
| | |
| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | [operator+](./structATAS_1_1DataFeedsCore_1_1Money.md#a892921be735969fa8ba1f5c2826b2523) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m1, decimal m2) |
| | |
| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | [operator-](./structATAS_1_1DataFeedsCore_1_1Money.md#acda17b6f5e62f75cd7c5b680b5bae940) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m1, decimal m2) |
| | |
| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | [operator*](./structATAS_1_1DataFeedsCore_1_1Money.md#a54a36166e428490a90bcc30e1987e3e1) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m1, decimal m2) |
| | |
| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | [operator/](./structATAS_1_1DataFeedsCore_1_1Money.md#a8ba0e259b23de5ce89fb73e8bf7e629b) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m1, decimal m2) |
| | |
| static implicit | [operator decimal](./structATAS_1_1DataFeedsCore_1_1Money.md#a0a1067e251120902a3def354f9fca258) ([Money](./structATAS_1_1DataFeedsCore_1_1Money.md) m) |
| | |

| Properties | |
| --- | --- |
| decimal | [Value](./structATAS_1_1DataFeedsCore_1_1Money.md#a09fb96d826d8e89ba0a2132c77ba4dea)`[get]` |
| | |
| string | [Currency](./structATAS_1_1DataFeedsCore_1_1Money.md#a4839de295e0a4782b317cbd3450694ab)`[get]` |
| | |
| CurrencyInfo | [CurrencyInfo](./structATAS_1_1DataFeedsCore_1_1Money.md#a980c6b44e4ba0c9999f7a40069ef020d)`[get]` |
| | |

## Detailed Description

Represents decimal amount in some currency.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Money() [1/2]

| ATAS.DataFeedsCore.Money.Money | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Money() [2/2]

| ATAS.DataFeedsCore.Money.Money | ( | decimal | value, |
| --- | --- | --- | --- |
| | | [CurrencyInfo](./structATAS_1_1DataFeedsCore_1_1Money.md#a980c6b44e4ba0c9999f7a40069ef020d) | currency |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)operator decimal()

| static implicit ATAS.DataFeedsCore.Money.operator decimal | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m | ) | |
| --- | --- | --- | --- | --- | --- |

static

## [◆](https://docs.atas.net/en/)operator*()

| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) ATAS.DataFeedsCore.Money.operator* | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m1, |
| --- | --- | --- | --- |
| | | decimal | m2 |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)operator+() [1/2]

| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) ATAS.DataFeedsCore.Money.operator+ | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m1, |
| --- | --- | --- | --- |
| | | decimal | m2 |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)operator+() [2/2]

| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) ATAS.DataFeedsCore.Money.operator+ | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m1, |
| --- | --- | --- | --- |
| | | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m2 |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)operator-() [1/3]

| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) ATAS.DataFeedsCore.Money.operator- | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m1 | ) | |
| --- | --- | --- | --- | --- | --- |

static

## [◆](https://docs.atas.net/en/)operator-() [2/3]

| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) ATAS.DataFeedsCore.Money.operator- | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m1, |
| --- | --- | --- | --- |
| | | decimal | m2 |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)operator-() [3/3]

| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) ATAS.DataFeedsCore.Money.operator- | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m1, |
| --- | --- | --- | --- |
| | | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m2 |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)operator/()

| static [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) ATAS.DataFeedsCore.Money.operator/ | ( | [Money](./structATAS_1_1DataFeedsCore_1_1Money.md) | m1, |
| --- | --- | --- | --- |
| | | decimal | m2 |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)ToString() [1/2]

| override string ATAS.DataFeedsCore.Money.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ToString() [2/2]

| string ATAS.DataFeedsCore.Money.ToString | ( | string? | format, |
| --- | --- | --- | --- |
| | | IFormatProvider? | formatProvider |
| | ) | | |

## Property Documentation

## [◆](https://docs.atas.net/en/)Currency

| string ATAS.DataFeedsCore.Money.Currency |
| --- |

get

## [◆](https://docs.atas.net/en/)CurrencyInfo

| CurrencyInfo ATAS.DataFeedsCore.Money.CurrencyInfo |
| --- |

get

## [◆](https://docs.atas.net/en/)Value

| decimal ATAS.DataFeedsCore.Money.Value |
| --- |

get

The documentation for this struct was generated from the following file:
- [Money.cs](../files/Money_8cs.md)
