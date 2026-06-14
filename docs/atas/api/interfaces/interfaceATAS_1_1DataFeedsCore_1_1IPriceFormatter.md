# ATAS.DataFeedsCore.IPriceFormatter Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.html

Exposes the subset of trading-entity properties required to format prices and derive instrument classification (US bonds, MOEX futures). Implemented by Security directly and by `OFT.Platform.Models.Instrument` via explicit interface implementation.
 [More...](./interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#details)

Inheritance diagram for ATAS.DataFeedsCore.IPriceFormatter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| decimal | [TickSize](./interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a7c167afad451a5064a32e33b81c7daf7)`[get]` |
| | |
| decimal | [TickCost](./interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#aea3427ed2399feae0aa94ecb63dec218)`[get]` |
| | |
| int | [Digits](./interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#aaa7f7f5a8c11b73085c20cdea17bfbb4)`[get]` |
| | |
| string | [Code](./interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a6b237e5f3f1830d15e2856bbd887ff59)`[get]` |
| | |
| string | [Exchange](./interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a9d33afad63385cd6d32e15ce32ee22be)`[get]` |
| | |

## Detailed Description

Exposes the subset of trading-entity properties required to format prices and derive instrument classification (US bonds, MOEX futures). Implemented by Security directly and by `OFT.Platform.Models.Instrument` via explicit interface implementation.

## Property Documentation

## [◆](https://docs.atas.net/en/)Code

| string ATAS.DataFeedsCore.IPriceFormatter.Code |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md#a1270123ef753defa83d8bc14cdf87bb6).

## [◆](https://docs.atas.net/en/)Digits

| int ATAS.DataFeedsCore.IPriceFormatter.Digits |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md#afca084690d536b82d0d0e57bf463c635).

## [◆](https://docs.atas.net/en/)Exchange

| string ATAS.DataFeedsCore.IPriceFormatter.Exchange |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md#af0687d502e63448c669d4cd2383ffd08).

## [◆](https://docs.atas.net/en/)TickCost

| decimal ATAS.DataFeedsCore.IPriceFormatter.TickCost |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md#a774834653ff6698159533ef94b9c907d).

## [◆](https://docs.atas.net/en/)TickSize

| decimal ATAS.DataFeedsCore.IPriceFormatter.TickSize |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md#a8f52984e4c0015c5b5e708f94872ea00).

The documentation for this interface was generated from the following file:
- [IPriceFormatter.cs](../files/IPriceFormatter_8cs.md)
