# ATAS.DataFeedsCore.Dom.ScaleDomManager< TMarketDepth > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.html

Inheritance diagram for ATAS.DataFeedsCore.Dom.ScaleDomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Dom.ScaleDomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#aa3a308af82df9f24dc8a404b5b1bc984) (Func priceScaler) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a56933f23ff19ef0394edfaf26cf5dd28) () |
| | |
| IReadOnlyCollection | [Update](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a79ad301cbfa29902076672917984efe0) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#ae9366de3fa42232165d84b8dad87d789) (TMarketDepth depth) |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e) () |
| | |
| IReadOnlyCollection | [Update](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e) (TMarketDepth depth) |
| | |

| Properties | |
| --- | --- |
| SyncRoot | [SyncRoot](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#ae45fd669d73e899b2fa336177022805b)`[get]` |
| | |
| IEnumerable | [Bids](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a1994c93dcc18fd10c8771e3c7376e991)`[get]` |
| | |
| IEnumerable | [Asks](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a8569507e09eb7ebf5488c20dfd95ee77)`[get]` |
| | |
| IEnumerable | [All](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#ac6cee9ff88eec26ca523311c0752c3f3)`[get]` |
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

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ScaleDomManager()

| [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).[ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md) | ( | Func | priceScaler | ) | |
| --- | --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e).

## [◆](https://docs.atas.net/en/)Update()

| IReadOnlyCollection [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).Update | ( | IReadOnlyCollection | depths | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576).

## [◆](https://docs.atas.net/en/)UpdateLevel1()

| TMarketDepth [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).UpdateLevel1 | ( | TMarketDepth | depth | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e).

## Property Documentation

## [◆](https://docs.atas.net/en/)All

| IEnumerable [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).All |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a78105922c227f2cc40b7969e7cd3b1a7).

## [◆](https://docs.atas.net/en/)Asks

| IEnumerable [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).Asks |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae7e9c178cc660965c83c22b2ed966ce6).

## [◆](https://docs.atas.net/en/)Bids

| IEnumerable [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).Bids |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae8758959fcb3b724f8224a928635219d).

## [◆](https://docs.atas.net/en/)SyncRoot

| SyncRoot [ATAS.DataFeedsCore.Dom.ScaleDomManager](./classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md).SyncRoot |
| --- |

get

Implements [ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5f7b5deb877fbbf678402ddbed98feac).

The documentation for this class was generated from the following file:
- [ScaleDomManager.cs](../files/ScaleDomManager_8cs.md)
