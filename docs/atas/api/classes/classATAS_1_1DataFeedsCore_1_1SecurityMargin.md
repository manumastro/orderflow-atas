# ATAS.DataFeedsCore.SecurityMargin Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1SecurityMargin.html

Inheritance diagram for ATAS.DataFeedsCore.SecurityMargin:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.SecurityMargin:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#acafd2cef088391511a8b0a2703898a04) () |
| | |
| | [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a223aa22230fcd9fcda31d26e633e62a7) (string securityId, decimal defaultValue) |
| | |
| [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | [GetSecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a7e4fb6c6f46b84eed77ff2b3f0d968df) (string securityId) |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a2f0f21ecb8fe66b835a5cd07ca8f8f1e) () |
| | Returns a string that represents the current object. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a2b102f43c9e50d53ca8810b1e47de2e4) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| string | [SecurityId](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a14f8fdd76350b387d4ca52dd09c16f61)`[get, set]` |
| | |
| bool | [IsContract](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#acb8f9c9d97a07f7c38ff905f16007786)`[get, set]` |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#afd5afb8c20e16736fac95fa61eb812b6)`[get, set]` |
| | |
| DateTime | [Date](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#ae52742d5c0ae4cf57e70f45c2ae3b9e9)`[get, set]` |
| | |
| decimal | [IntradayInitialMarginBuy](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a084953928a80d2652a1ab3dd38131e91)`[get, set]` |
| | |
| decimal | [IntradayInitialMarginSell](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#acd91b6adeb1ee1ee0939efca2189978f)`[get, set]` |
| | |
| decimal | [IntradayMarginBuy](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#ab82f2f96b4b5ca8467e9c2614daa2714)`[get, set]` |
| | |
| decimal | [IntradayMarginSell](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#ad045d524b5db733633dd7c26512f4009)`[get, set]` |
| | |
| decimal | [InitialMarginBuy](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a4a9add9481f298837105efd425f5a974)`[get, set]` |
| | |
| decimal | [InitialMarginSell](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a42bdae10176a7d98280c4a788ce37724)`[get, set]` |
| | |
| decimal | [MarginBuy](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#af498c5e866657c40a15391b46fc91905)`[get, set]` |
| | |
| decimal | [MarginSell](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#aa355e4d3b9c1d306e77cf63a36c0d6b5)`[get, set]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#a14833923f0c5c0172b4843bf56e32d95)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md#af2b13d24315b4a8cb07b09710a2fafc7) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SecurityMargin() [1/2]

| ATAS.DataFeedsCore.SecurityMargin.SecurityMargin | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)SecurityMargin() [2/2]

| ATAS.DataFeedsCore.SecurityMargin.SecurityMargin | ( | string | securityId, |
| --- | --- | --- | --- |
| | | decimal | defaultValue |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetSecurityMargin()

| [SecurityMargin](./classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) ATAS.DataFeedsCore.SecurityMargin.GetSecurityMargin | ( | string | securityId | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.SecurityMargin.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.SecurityMargin.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## Property Documentation

## [◆](https://docs.atas.net/en/)Date

| DateTime ATAS.DataFeedsCore.SecurityMargin.Date |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.SecurityMargin.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)InitialMarginBuy

| decimal ATAS.DataFeedsCore.SecurityMargin.InitialMarginBuy |
| --- |

getset

## [◆](https://docs.atas.net/en/)InitialMarginSell

| decimal ATAS.DataFeedsCore.SecurityMargin.InitialMarginSell |
| --- |

getset

## [◆](https://docs.atas.net/en/)IntradayInitialMarginBuy

| decimal ATAS.DataFeedsCore.SecurityMargin.IntradayInitialMarginBuy |
| --- |

getset

## [◆](https://docs.atas.net/en/)IntradayInitialMarginSell

| decimal ATAS.DataFeedsCore.SecurityMargin.IntradayInitialMarginSell |
| --- |

getset

## [◆](https://docs.atas.net/en/)IntradayMarginBuy

| decimal ATAS.DataFeedsCore.SecurityMargin.IntradayMarginBuy |
| --- |

getset

## [◆](https://docs.atas.net/en/)IntradayMarginSell

| decimal ATAS.DataFeedsCore.SecurityMargin.IntradayMarginSell |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsContract

| bool ATAS.DataFeedsCore.SecurityMargin.IsContract |
| --- |

getset

## [◆](https://docs.atas.net/en/)MarginBuy

| decimal ATAS.DataFeedsCore.SecurityMargin.MarginBuy |
| --- |

getset

## [◆](https://docs.atas.net/en/)MarginSell

| decimal ATAS.DataFeedsCore.SecurityMargin.MarginSell |
| --- |

getset

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.SecurityMargin.Security |
| --- |

getset

## [◆](https://docs.atas.net/en/)SecurityId

| string ATAS.DataFeedsCore.SecurityMargin.SecurityId |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.SecurityMargin.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [SecurityMargin.cs](../files/SecurityMargin_8cs.md)
