# ATAS.DataFeedsCore.Dom.GroupDomManager< TMarketDepth > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.html

Inheritance diagram for ATAS.DataFeedsCore.Dom.GroupDomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Dom.GroupDomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Add](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a62afd37d897debf26f1a1c0f825cee98) ([IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md) domManager) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a279b3235e275357ccaf175f51c25bfaa) () |
| | |
| IReadOnlyCollection | [Update](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a931665fc80f6ae976b20b06520a9c104) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#ac86812a30eea33df98a33acf4754e80c) (TMarketDepth depth) |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e) () |
| | |
| IReadOnlyCollection | [Update](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e) (TMarketDepth depth) |
| | |

| Properties | |
| --- | --- |
| SyncRoot | [SyncRoot](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#ae0b2a9b53672a5c07c46887d2630e2f6)`[get]` |
| | |
| IEnumerable | [Asks](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a899bbe8fabf5789526d2149bde439165)`[get]` |
| | |
| IEnumerable | [Bids](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a7283dc7a638ae0da5ee98253aa33f6e8)`[get]` |
| | |
| IEnumerable | [All](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#ab3915911d1df963a98e58e9c6204981c)`[get]` |
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

## [◆](https://docs.atas.net/en/)Add()

| void [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).Add | ( | [IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md) | domManager | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e).

## [◆](https://docs.atas.net/en/)Update()

| IReadOnlyCollection [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).Update | ( | IReadOnlyCollection | depths | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576).

## [◆](https://docs.atas.net/en/)UpdateLevel1()

| TMarketDepth [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).UpdateLevel1 | ( | TMarketDepth | depth | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e).

## Property Documentation

## [◆](https://docs.atas.net/en/)All

| IEnumerable [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).All |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a78105922c227f2cc40b7969e7cd3b1a7).

## [◆](https://docs.atas.net/en/)Asks

| IEnumerable [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).Asks |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae7e9c178cc660965c83c22b2ed966ce6).

## [◆](https://docs.atas.net/en/)Bids

| IEnumerable [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).Bids |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae8758959fcb3b724f8224a928635219d).

## [◆](https://docs.atas.net/en/)SyncRoot

| SyncRoot [ATAS.DataFeedsCore.Dom.GroupDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md).SyncRoot |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5f7b5deb877fbbf678402ddbed98feac).

The documentation for this class was generated from the following file:
- [GroupDomManager.cs](../files/GroupDomManager_8cs.md)
