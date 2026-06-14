# ATAS.DataFeedsCore.Dom.DomManager< TMarketDepth > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.html

Maintains Depth of Market state for the security.
 [More...](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Dom.DomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Dom.DomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a917715a1809b6c70e5c5ba27e3529f18) () |
| | |
| void | [Update](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a0ce041104a840e74e56a1773b47f47da) (TMarketDepth depth) |
| | |
| IReadOnlyCollection | [Update](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a101a0bd2b88f0e342177a6e559b35e11) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a6e1c2867db2a73a58041946116d88874) (TMarketDepth depth) |
| | |
| SortedDictionary | [CloneState](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#ae6a13dfd9fc2365dd5982f5942959c20) ([MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) side) |
| | |
| IEnumerable | [RemoveOverlappingQuotes](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#adb06b6f35f556022658da845a17dbc5a) () |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e) () |
| | |
| IReadOnlyCollection | [Update](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e) (TMarketDepth depth) |
| | |

| Properties | |
| --- | --- |
| SyncRoot | [SyncRoot](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#ab8bc28ec9ca0d289346a5b55524b3b3c)`[get]` |
| | |
| int | [Count](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a1f744c1be7a769829799b9cd8933dcaa)`[get]` |
| | |
| IEnumerable | [Asks](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a2e138739f59cada19f861de70ab66b65)`[get]` |
| | |
| IEnumerable | [Bids](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#ab62df4ac1448091122d4cd94c1164433)`[get]` |
| | |
| IEnumerable | [All](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a4136faf323cc9eb47a606bced8cc0695)`[get]` |
| | |
| TMarketDepth? | [BestBid](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a4a598fffc09a4575370b57748d04cfa7)`[get]` |
| | |
| TMarketDepth? | [BestAsk](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#ad18e93ad0048cc6d6676e044e13512c2)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md) | |
| SyncRoot | [SyncRoot](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5f7b5deb877fbbf678402ddbed98feac)`[get]` |
| | |
| IEnumerable | [Asks](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae7e9c178cc660965c83c22b2ed966ce6)`[get]` |
| | |
| IEnumerable | [Bids](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae8758959fcb3b724f8224a928635219d)`[get]` |
| | |
| IEnumerable | [All](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a78105922c227f2cc40b7969e7cd3b1a7)`[get]` |
| | |

## Detailed Description

Maintains Depth of Market state for the security.

Type Constraints

| TMarketDepth | : | class | |
| --- | --- | --- | --- |
| TMarketDepth | : | [IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) | |
| TMarketDepth | : | new() | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e).

## [◆](https://docs.atas.net/en/)CloneState()

| SortedDictionary [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).CloneState | ( | [MarketDataType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) | side | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)RemoveOverlappingQuotes()

| IEnumerable [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).RemoveOverlappingQuotes | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Update() [1/2]

| IReadOnlyCollection [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).Update | ( | IReadOnlyCollection | depths | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576).

## [◆](https://docs.atas.net/en/)Update() [2/2]

| void [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).Update | ( | TMarketDepth | depth | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)UpdateLevel1()

| TMarketDepth [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).UpdateLevel1 | ( | TMarketDepth | depth | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e).

## Property Documentation

## [◆](https://docs.atas.net/en/)All

| IEnumerable [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).All |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a78105922c227f2cc40b7969e7cd3b1a7).

## [◆](https://docs.atas.net/en/)Asks

| IEnumerable [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).Asks |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae7e9c178cc660965c83c22b2ed966ce6).

## [◆](https://docs.atas.net/en/)BestAsk

| TMarketDepth? [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).BestAsk |
| --- |

get

## [◆](https://docs.atas.net/en/)BestBid

| TMarketDepth? [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).BestBid |
| --- |

get

## [◆](https://docs.atas.net/en/)Bids

| IEnumerable [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).Bids |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae8758959fcb3b724f8224a928635219d).

## [◆](https://docs.atas.net/en/)Count

| int [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).Count |
| --- |

get

## [◆](https://docs.atas.net/en/)SyncRoot

| SyncRoot [ATAS.DataFeedsCore.Dom.DomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md).SyncRoot |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5f7b5deb877fbbf678402ddbed98feac).

The documentation for this class was generated from the following file:
- [DomManager.cs](../files/DomManager_8cs.md)
