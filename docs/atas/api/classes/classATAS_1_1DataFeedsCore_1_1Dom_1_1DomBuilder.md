# ATAS.DataFeedsCore.Dom.DomBuilder Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.html

Builds and maintains a DOM for a connector

 Allows to obtain best prices if connector does not give them

 [More...](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#details)

Collaboration diagram for ATAS.DataFeedsCore.Dom.DomBuilder:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Classes | |
| --- | --- |
| class | [DomChangesTracker](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md) |
| | |

| Public Member Functions | |
| --- | --- |
| Dictionary? | [GetDom](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#a25e021fc9049d384ee20f3fd0b3e57fa) (string securityCode) |
| | |
| ICollection | [RecordedInstruments](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#aa0548eb0aad6c14eacd2e472c4926e73) () |
| | |
| [DomChangesTracker](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md) | [BeginChanges](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#aa80f221e899bfb7acd6056de683de872) (string securityCode) |
| | Initiates changes in the DOM by returning a special object `DomChangesTracker`
 After tracking, call `GetChanges()` to commit all changes to the parent dom builder. |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#a4109efd7284f1d4626c055a66f26c47e) () |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#a27b42fddf0ef3635e7298a299a2365f0) (string securityCode) |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)?? [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) BestAsk | [GetBestPricesFor](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#aba1b72c009f5442165af52dd46d23668) (string securityCode) |
| | |

| Public Attributes | |
| --- | --- |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)? | [BestBid](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#ae3a843f2183be6d71d956c8d78bd1e6b) |
| | Returns a tuple of best Bid and Ask prices, or null
 Double nulls are possible if no DOM were loaded yet. |
| | |

| Protected Attributes | |
| --- | --- |
| readonly Dictionary > | [_domBySecurity](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md#a03f42bf212783fbdc2b64888b25f7368) = new() |
| | |

## Detailed Description

Builds and maintains a DOM for a connector

 Allows to obtain best prices if connector does not give them

This class is intended to use in thread safe environment

## Member Function Documentation

## [◆](https://docs.atas.net/en/)BeginChanges()

| [DomChangesTracker](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md) ATAS.DataFeedsCore.Dom.DomBuilder.BeginChanges | ( | string | securityCode | ) | |
| --- | --- | --- | --- | --- | --- |

Initiates changes in the DOM by returning a special object `DomChangesTracker`

 After tracking, call `GetChanges()` to commit all changes to the parent dom builder.

Parameters

| securityCode | |
| --- | --- |

Returns

## [◆](https://docs.atas.net/en/)Clear() [1/2]

| void ATAS.DataFeedsCore.Dom.DomBuilder.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Clear() [2/2]

| void ATAS.DataFeedsCore.Dom.DomBuilder.Clear | ( | string | securityCode | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetBestPricesFor()

| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)?? [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) BestAsk ATAS.DataFeedsCore.Dom.DomBuilder.GetBestPricesFor | ( | string | securityCode | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetDom()

| Dictionary? ATAS.DataFeedsCore.Dom.DomBuilder.GetDom | ( | string | securityCode | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)RecordedInstruments()

| ICollection ATAS.DataFeedsCore.Dom.DomBuilder.RecordedInstruments | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Data Documentation

## [◆](https://docs.atas.net/en/)_domBySecurity

| readonly Dictionary > ATAS.DataFeedsCore.Dom.DomBuilder._domBySecurity = new() |
| --- |

protected

## [◆](https://docs.atas.net/en/)BestBid

| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)? ATAS.DataFeedsCore.Dom.DomBuilder.BestBid |
| --- |

Returns a tuple of best Bid and Ask prices, or null

 Double nulls are possible if no DOM were loaded yet.

Parameters

| securityCode | |
| --- | --- |

Returns

The documentation for this class was generated from the following file:
- [DomBuilder.cs](../files/DomBuilder_8cs.md)
