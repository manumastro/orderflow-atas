# ATAS.DataFeedsCore.HistoryMyTrade Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.html

Represents a historical trade record.
 [More...](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#details)

Inheritance diagram for ATAS.DataFeedsCore.HistoryMyTrade:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.HistoryMyTrade:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#ae5f8529af8f37865e0fe22f561b64d7f) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#ad9932f2b6f749aa9de74e85b432bb0a8) () |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a1281f998c2b75784e0083d2ca3d0b94c) (string propertyName) |
| | Raises the PropertyChanged event for a specific property. |
| | |

| Properties | |
| --- | --- |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a4f8ac1df5940dc4bb7dfd3b32da4e2a7)`[get, set]` |
| | Gets or sets the unique identifier of the historical trade. |
| | |
| string | [AccountID](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#ae84a5d56e2af75730ffec632e66c17a9)`[get, set]` |
| | Gets or sets the account ID associated with the historical trade. |
| | |
| string | [SecurityId](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a2b7312a0fee5b6ce480ef5c459d78e6c)`[get, set]` |
| | Gets or sets the security ID associated with the historical trade. |
| | |
| string | [SecurityCode](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#adc21aa8eee5b4d27e390e976e6db74ca)`[get, set]` |
| | Gets or sets the security code associated with the historical trade. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a21ba8e8d9eb64c07ed1a1f06d710383d)`[get, set]` |
| | Gets or sets the security object associated with the historical trade. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a646ac5856c9a597cb7fb17aef703c373)`[get, set]` |
| | Gets or sets the portfolio object associated with the historical trade. |
| | |
| DateTime | [OpenTime](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a25aecba44b20727e8d4149e22ee713f0)`[get, set]` |
| | Gets or sets the open time of the historical trade. |
| | |
| decimal | [OpenPrice](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a47875223f5a9c6abfbf49329d1662de2)`[get, set]` |
| | Gets or sets the open price of the historical trade. |
| | |
| decimal | [OpenVolume](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#af436624aee30d1ac2b0fbf077a03d0b8)`[get, set]` |
| | Gets or sets the open volume of the historical trade. |
| | |
| DateTime | [CloseTime](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a77ee0b0d929dba581c0b3ae31b5b5fad)`[get, set]` |
| | Gets or sets the close time of the historical trade. |
| | |
| decimal | [ClosePrice](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#abc6fe360bd0d114ab1ffa007b7a8f821)`[get, set]` |
| | Gets or sets the close price of the historical trade. |
| | |
| decimal | [CloseVolume](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#ab477267ce14e7760d4f1bbe4da98bbbf)`[get, set]` |
| | Gets or sets the close volume of the historical trade. |
| | |
| decimal | [PnL](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#ae3e066ace9ec3744295e7db32e37409a)`[get, set]` |
| | Gets or sets the profit and loss (PnL) of the historical trade. |
| | |
| decimal | [TicksPnL](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a5ad2377d7d72299ca628fb3f249d3e2c)`[get, set]` |
| | Gets or sets the PnL in ticks of the historical trade. |
| | |
| decimal | [PricePnL](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a184a6ec63b958845d31de6fe6ea181a5)`[get, set]` |
| | Gets or sets the PnL in price units of the historical trade. |
| | |
| decimal? | [Commission](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a0b18d755c77050adad94f9461fe33ae6)`[get, set]` |
| | Gets or sets the commission associated with the historical trade. |
| | |
| string | [Comment](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a2e6da56b8b41cf98c0b3a90585f6eb11)`[get, set]` |
| | Gets or sets the comment associated with the historical trade. |
| | |
| bool | [Reviewed](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a79082d5a56a865cd6555f5bd1f88e068)`[get, set]` |
| | |
| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [EnterTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#addefbef2bf18c552b771e8d96932f6b7)`[get, set]` |
| | Gets or sets the enter trade associated with the historical trade. |
| | |
| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | [ExitTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a2668a1a0250106a18c4e35b8d672fbdd)`[get, set]` |
| | Gets or sets the exit trade associated with the historical trade. |
| | |
| [MissingDataCases](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9c) | [MissingDataCase](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a0e26b908b484835a6fc4b23f19114807)`[get, set]` |
| | Indicates what part of the trade is missing. |
| | |
| bool | [IsComplete](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#af65455e0d4ae894f62cfc0748b1ed8d1)`[get]` |
| | |
| List | [Playbooks](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#a6bb202559c725209773ea74b5d75f538) = []`[get, set]` |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#ae1cc814c0c53a3a6bec8eca0f9cc3760) |
| | |

## Detailed Description

Represents a historical trade record.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) ATAS.DataFeedsCore.HistoryMyTrade.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.HistoryMyTrade.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Raises the PropertyChanged event for a specific property.

Parameters

| propertyName | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.HistoryMyTrade.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AccountID

| string ATAS.DataFeedsCore.HistoryMyTrade.AccountID |
| --- |

getset

Gets or sets the account ID associated with the historical trade.

## [◆](https://docs.atas.net/en/)ClosePrice

| decimal ATAS.DataFeedsCore.HistoryMyTrade.ClosePrice |
| --- |

getset

Gets or sets the close price of the historical trade.

## [◆](https://docs.atas.net/en/)CloseTime

| DateTime ATAS.DataFeedsCore.HistoryMyTrade.CloseTime |
| --- |

getset

Gets or sets the close time of the historical trade.

## [◆](https://docs.atas.net/en/)CloseVolume

| decimal ATAS.DataFeedsCore.HistoryMyTrade.CloseVolume |
| --- |

getset

Gets or sets the close volume of the historical trade.

## [◆](https://docs.atas.net/en/)Comment

| string ATAS.DataFeedsCore.HistoryMyTrade.Comment |
| --- |

getset

Gets or sets the comment associated with the historical trade.

## [◆](https://docs.atas.net/en/)Commission

| decimal? ATAS.DataFeedsCore.HistoryMyTrade.Commission |
| --- |

getset

Gets or sets the commission associated with the historical trade.

## [◆](https://docs.atas.net/en/)EnterTrade

| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) ATAS.DataFeedsCore.HistoryMyTrade.EnterTrade |
| --- |

getset

Gets or sets the enter trade associated with the historical trade.

## [◆](https://docs.atas.net/en/)ExitTrade

| [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) ATAS.DataFeedsCore.HistoryMyTrade.ExitTrade |
| --- |

getset

Gets or sets the exit trade associated with the historical trade.

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.HistoryMyTrade.Id |
| --- |

getset

Gets or sets the unique identifier of the historical trade.

## [◆](https://docs.atas.net/en/)IsComplete

| bool ATAS.DataFeedsCore.HistoryMyTrade.IsComplete |
| --- |

get

## [◆](https://docs.atas.net/en/)MissingDataCase

| [MissingDataCases](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9c) ATAS.DataFeedsCore.HistoryMyTrade.MissingDataCase |
| --- |

getset

Indicates what part of the trade is missing.

## [◆](https://docs.atas.net/en/)OpenPrice

| decimal ATAS.DataFeedsCore.HistoryMyTrade.OpenPrice |
| --- |

getset

Gets or sets the open price of the historical trade.

## [◆](https://docs.atas.net/en/)OpenTime

| DateTime ATAS.DataFeedsCore.HistoryMyTrade.OpenTime |
| --- |

getset

Gets or sets the open time of the historical trade.

## [◆](https://docs.atas.net/en/)OpenVolume

| decimal ATAS.DataFeedsCore.HistoryMyTrade.OpenVolume |
| --- |

getset

Gets or sets the open volume of the historical trade.

## [◆](https://docs.atas.net/en/)Playbooks

| List ATAS.DataFeedsCore.HistoryMyTrade.Playbooks = [] |
| --- |

getset

## [◆](https://docs.atas.net/en/)PnL

| decimal ATAS.DataFeedsCore.HistoryMyTrade.PnL |
| --- |

getset

Gets or sets the profit and loss (PnL) of the historical trade.

## [◆](https://docs.atas.net/en/)Portfolio

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.DataFeedsCore.HistoryMyTrade.Portfolio |
| --- |

getset

Gets or sets the portfolio object associated with the historical trade.

## [◆](https://docs.atas.net/en/)PricePnL

| decimal ATAS.DataFeedsCore.HistoryMyTrade.PricePnL |
| --- |

getset

Gets or sets the PnL in price units of the historical trade.

## [◆](https://docs.atas.net/en/)Reviewed

| bool ATAS.DataFeedsCore.HistoryMyTrade.Reviewed |
| --- |

getset

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.HistoryMyTrade.Security |
| --- |

getset

Gets or sets the security object associated with the historical trade.

## [◆](https://docs.atas.net/en/)SecurityCode

| string ATAS.DataFeedsCore.HistoryMyTrade.SecurityCode |
| --- |

getset

Gets or sets the security code associated with the historical trade.

## [◆](https://docs.atas.net/en/)SecurityId

| string ATAS.DataFeedsCore.HistoryMyTrade.SecurityId |
| --- |

getset

Gets or sets the security ID associated with the historical trade.

## [◆](https://docs.atas.net/en/)TicksPnL

| decimal ATAS.DataFeedsCore.HistoryMyTrade.TicksPnL |
| --- |

getset

Gets or sets the PnL in ticks of the historical trade.

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.HistoryMyTrade.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [HistoryMyTrade.cs](../files/HistoryMyTrade_8cs.md)
