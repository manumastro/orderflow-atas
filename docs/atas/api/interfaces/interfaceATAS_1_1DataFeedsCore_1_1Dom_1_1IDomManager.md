# ATAS.DataFeedsCore.Dom.IDomManager< TMarketDepth > Interface Template Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.html

Inheritance diagram for ATAS.DataFeedsCore.Dom.IDomManager< TMarketDepth >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Clear](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a28ef6eef0275bb8f64e0da947905a20e) () |
| | |
| IReadOnlyCollection | [Update](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#abbbd58324ceaca66a5eea43f69b10576) (IReadOnlyCollection depths) |
| | |
| TMarketDepth | [UpdateLevel1](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5d84fcd156195da03ef5b372e68e223e) (TMarketDepth depth) |
| | |

| Properties | |
| --- | --- |
| SyncRoot | [SyncRoot](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a5f7b5deb877fbbf678402ddbed98feac)`[get]` |
| | |
| IEnumerable | [Asks](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae7e9c178cc660965c83c22b2ed966ce6)`[get]` |
| | |
| IEnumerable | [Bids](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#ae8758959fcb3b724f8224a928635219d)`[get]` |
| | |
| IEnumerable | [All](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md#a78105922c227f2cc40b7969e7cd3b1a7)`[get]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.DataFeedsCore.Dom.IDomManager](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a917715a1809b6c70e5c5ba27e3529f18), [ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a279b3235e275357ccaf175f51c25bfaa), [ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#ae95ae26dcc05994dfc40921d04fd77ad), and [ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a56933f23ff19ef0394edfaf26cf5dd28).

## [◆](https://docs.atas.net/en/)Update()

| IReadOnlyCollection [ATAS.DataFeedsCore.Dom.IDomManager](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md).Update | ( | IReadOnlyCollection | depths | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a101a0bd2b88f0e342177a6e559b35e11), [ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a931665fc80f6ae976b20b06520a9c104), [ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a8a234ae2aabaf812ccbc3b9e0e0a575a), and [ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a79ad301cbfa29902076672917984efe0).

## [◆](https://docs.atas.net/en/)UpdateLevel1()

| TMarketDepth [ATAS.DataFeedsCore.Dom.IDomManager](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md).UpdateLevel1 | ( | TMarketDepth | depth | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a6e1c2867db2a73a58041946116d88874), [ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#ac86812a30eea33df98a33acf4754e80c), [ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#afa1d78c8ebb72f2c1beee915c1fd2407), and [ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#ae9366de3fa42232165d84b8dad87d789).

## Property Documentation

## [◆](https://docs.atas.net/en/)All

| IEnumerable [ATAS.DataFeedsCore.Dom.IDomManager](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md).All |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a4136faf323cc9eb47a606bced8cc0695), [ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#ab3915911d1df963a98e58e9c6204981c), [ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#aeaae8f5d42c585c2500e8577ae3d3ef0), and [ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#ac6cee9ff88eec26ca523311c0752c3f3).

## [◆](https://docs.atas.net/en/)Asks

| IEnumerable [ATAS.DataFeedsCore.Dom.IDomManager](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md).Asks |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#a2e138739f59cada19f861de70ab66b65), [ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a899bbe8fabf5789526d2149bde439165), [ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a3c1c6b6c64f847d4f983b2e5407a5d93), and [ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a8569507e09eb7ebf5488c20dfd95ee77).

## [◆](https://docs.atas.net/en/)Bids

| IEnumerable [ATAS.DataFeedsCore.Dom.IDomManager](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md).Bids |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#ab62df4ac1448091122d4cd94c1164433), [ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#a7283dc7a638ae0da5ee98253aa33f6e8), [ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a9dacc3781d0899c7464a55c6b7fe3f2f), and [ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#a1994c93dcc18fd10c8771e3c7376e991).

## [◆](https://docs.atas.net/en/)SyncRoot

| SyncRoot [ATAS.DataFeedsCore.Dom.IDomManager](./interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md).SyncRoot |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md#ab8bc28ec9ca0d289346a5b55524b3b3c), [ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md#ae0b2a9b53672a5c07c46887d2630e2f6), [ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md#a921493e072fb7b9a4cfbed6ff13bceba), and [ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md#ae45fd669d73e899b2fa336177022805b).

The documentation for this interface was generated from the following file:
- [IDomManager.cs](../files/IDomManager_8cs.md)
