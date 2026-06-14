# ATAS.DataFeedsCore.Dom.LimitDomManager< TMarketDepth > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.html

Inheritance diagram for ATAS.DataFeedsCore.Dom.LimitDomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Dom.LimitDomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#ae95ae26dcc05994dfc40921d04fd77ad) () |
| | |
| IReadOnlyCollection | [Update](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a8a234ae2aabaf812ccbc3b9e0e0a575a) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#afa1d78c8ebb72f2c1beee915c1fd2407) (TMarketDepth depth) |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e) () |
| | |
| IReadOnlyCollection | [Update](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e) (TMarketDepth depth) |
| | |

| Properties | |
| --- | --- |
| SyncRoot | [SyncRoot](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a921493e072fb7b9a4cfbed6ff13bceba)`[get]` |
| | |
| int | [DepthLevelsCount](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#afef77babcd3cc198da481890b382e784)`[get, set]` |
| | Gets or sets how many MarketDepth levels to store. |
| | |
| IEnumerable | [Asks](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a3c1c6b6c64f847d4f983b2e5407a5d93)`[get]` |
| | |
| IEnumerable | [Bids](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a9dacc3781d0899c7464a55c6b7fe3f2f)`[get]` |
| | |
| IEnumerable | [All](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#aeaae8f5d42c585c2500e8577ae3d3ef0)`[get]` |
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

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e).

## [◆](https://docs.atas.net/en/)Update()

| IReadOnlyCollection [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).Update | ( | IReadOnlyCollection | depths | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576).

## [◆](https://docs.atas.net/en/)UpdateLevel1()

| TMarketDepth [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).UpdateLevel1 | ( | TMarketDepth | depth | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e).

## Property Documentation

## [◆](https://docs.atas.net/en/)All

| IEnumerable [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).All |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a78105922c227f2cc40b7969e7cd3b1a7).

## [◆](https://docs.atas.net/en/)Asks

| IEnumerable [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).Asks |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae7e9c178cc660965c83c22b2ed966ce6).

## [◆](https://docs.atas.net/en/)Bids

| IEnumerable [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).Bids |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae8758959fcb3b724f8224a928635219d).

## [◆](https://docs.atas.net/en/)DepthLevelsCount

| int [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).DepthLevelsCount |
| --- |

getset

Gets or sets how many MarketDepth levels to store.

## [◆](https://docs.atas.net/en/)SyncRoot

| SyncRoot [ATAS.DataFeedsCore.Dom.LimitDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md).SyncRoot |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5f7b5deb877fbbf678402ddbed98feac).

The documentation for this class was generated from the following file:
- [LimitDomManager.cs](../files/LimitDomManager_8cs.md)
