# ATAS.DataFeedsCore.Position Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Position.html

Represents a trading position.
 [More...](./classATAS_1_1DataFeedsCore_1_1Position.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Position:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Position:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1Position.md#af6a736eb609276f76826af23aadd8a69) () |
| | Creates a deep copy of the current position. |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Position.md#ad17aedb45be9f448df7283a717412271) () |
| | |

| Protected Member Functions | |
| --- | --- |
| void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Position.md#afbb62689dc9847e75ae2e5c39c8246c6) (string name) |
| | Raises the PropertyChanged event to notify subscribers that a property's value has changed. |
| | |

| Properties | |
| --- | --- |
| string | [AccountID](./classATAS_1_1DataFeedsCore_1_1Position.md#a76a558d86aa31d62eb09b183ccf0d7eb)`[get, set]` |
| | Gets or sets the unique identifier of the account associated with this position. |
| | |
| string | [SecurityId](./classATAS_1_1DataFeedsCore_1_1Position.md#a13b73c1fa1643bc55354c8965afadd77)`[get, set]` |
| | Gets or sets the unique identifier of the security associated with this position. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Position.md#ac4f1371c6169793ae5e8ef9c0701b614)`[get, set]` |
| | Gets or sets the portfolio associated with this position. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | [Security](./classATAS_1_1DataFeedsCore_1_1Position.md#a84e5979ffbcb79ed085a411f66c4d908)`[get, set]` |
| | Gets or sets the security associated with this position. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](./classATAS_1_1DataFeedsCore_1_1Position.md#a5071b334d9eccf729f132b2b924416b8)`[get, set]` |
| | Gets or sets the T+ limit associated with this position, if applicable. |
| | |
| [PositionAveragePriceValueTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) | [AveragePriceValueType](./classATAS_1_1DataFeedsCore_1_1Position.md#aed55fd54e695ee984cb7379cb9b811c4)`[get, set]` |
| | Gets or sets the type of value used to calculate the average price of the position. |
| | |
| decimal | [AveragePrice](./classATAS_1_1DataFeedsCore_1_1Position.md#aa47fb625eaffdcc078bd5d6789a544a8)`[get, set]` |
| | Gets or sets the average price at which the position was acquired. |
| | |
| decimal | [UnrealizedPnL](./classATAS_1_1DataFeedsCore_1_1Position.md#a3fb5573d628fc5fbf987f8d69dd88a3b)`[get, set]` |
| | Gets or sets the unrealized profit or loss associated with this position. |
| | |
| decimal | [RealizedPnL](./classATAS_1_1DataFeedsCore_1_1Position.md#a83e19226658a7209d2ed90f8ae5b686d)`[get, set]` |
| | Gets or sets the realized profit or loss associated with this position. |
| | |
| decimal | [Volume](./classATAS_1_1DataFeedsCore_1_1Position.md#a71c4e00384fae1d31a3384f9e9d13cdd)`[get, set]` |
| | Gets or sets the current volume of the position. |
| | |
| decimal | [OpenVolume](./classATAS_1_1DataFeedsCore_1_1Position.md#adf66b0f1d0eb9263df6d2676e592c5a4)`[get, set]` |
| | Gets or sets the open volume (unfilled quantity) of the financial instrument associated with this position. |
| | |
| decimal | [CurrentBuy](./classATAS_1_1DataFeedsCore_1_1Position.md#a945b2c54d3923c6a713aa44040c35385)`[get, set]` |
| | Gets or sets the current buy price for the financial instrument associated with this position. |
| | |
| decimal | [CurrentSell](./classATAS_1_1DataFeedsCore_1_1Position.md#a8d9edacc5acb6b04e0b9aeb91726cad0)`[get, set]` |
| | Gets or sets the current sell price for the financial instrument associated with this position. |
| | |
| bool | [IsInPosition](./classATAS_1_1DataFeedsCore_1_1Position.md#a21d83be3867384481f7ce2b27a225819)`[get, set]` |
| | Gets or sets whether the position is currently open (in position). |
| | |
| object | [Parent](./classATAS_1_1DataFeedsCore_1_1Position.md#a4ba6fc9d57c6fbe60527334e0edcc6e4)`[get, set]` |
| | Gets or sets the parent or container object associated with this position. |
| | |
| decimal | [Commission](./classATAS_1_1DataFeedsCore_1_1Position.md#abcbeb07441f0d934bbc7fb3dc5744602)`[get, set]` |
| | Gets or sets the commission associated with this position. |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1Position.md#a28d69a357dd4194d92db1b825c3db4b5)`[get]` |
| | |
| [PnlPercentType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87) | [PnlPercentType](./classATAS_1_1DataFeedsCore_1_1Position.md#ad17e2b468b75823bf1a69e1a745f11af) = [PnlPercentType.Portfolio](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87ad4f859a96c13f551a2771b7fc3a78d38)`[get, set]` |
| | Calculate pnl percent from portfolio balance or position margin. |
| | |
| [RiskInfo](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a5782229ea6bf214ef71162db25fbfd1b)? | [Risk](./classATAS_1_1DataFeedsCore_1_1Position.md#af307199127d6a6586a0059013b61ef66)`[get, set]` |
| | Gets or sets information about marginal trading options. Null if marginal trading is not supported. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Position.md#aee991183e9536dd8cce38088cd0cff9e) |
| | |

## Detailed Description

Represents a trading position.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) ATAS.DataFeedsCore.Position.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

Creates a deep copy of the current position.

ReturnsA new instance of the Position class that is a copy of the current position.

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| void ATAS.DataFeedsCore.Position.OnPropertyChanged | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PropertyChanged event to notify subscribers that a property's value has changed.

Parameters

| name | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.Position.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AccountID

| string ATAS.DataFeedsCore.Position.AccountID |
| --- |

getset

Gets or sets the unique identifier of the account associated with this position.

## [◆](https://docs.atas.net/en/)AveragePrice

| decimal ATAS.DataFeedsCore.Position.AveragePrice |
| --- |

getset

Gets or sets the average price at which the position was acquired.

## [◆](https://docs.atas.net/en/)AveragePriceValueType

| [PositionAveragePriceValueTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) ATAS.DataFeedsCore.Position.AveragePriceValueType |
| --- |

getset

Gets or sets the type of value used to calculate the average price of the position.

## [◆](https://docs.atas.net/en/)Commission

| decimal ATAS.DataFeedsCore.Position.Commission |
| --- |

getset

Gets or sets the commission associated with this position.

## [◆](https://docs.atas.net/en/)CurrentBuy

| decimal ATAS.DataFeedsCore.Position.CurrentBuy |
| --- |

getset

Gets or sets the current buy price for the financial instrument associated with this position.

## [◆](https://docs.atas.net/en/)CurrentSell

| decimal ATAS.DataFeedsCore.Position.CurrentSell |
| --- |

getset

Gets or sets the current sell price for the financial instrument associated with this position.

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.Position.EntityType |
| --- |

get

Gets the entity type, which is EntityType.Position.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)IsInPosition

| bool ATAS.DataFeedsCore.Position.IsInPosition |
| --- |

getset

Gets or sets whether the position is currently open (in position).

## [◆](https://docs.atas.net/en/)OpenVolume

| decimal ATAS.DataFeedsCore.Position.OpenVolume |
| --- |

getset

Gets or sets the open volume (unfilled quantity) of the financial instrument associated with this position.

## [◆](https://docs.atas.net/en/)Parent

| object ATAS.DataFeedsCore.Position.Parent |
| --- |

getset

Gets or sets the parent or container object associated with this position.

## [◆](https://docs.atas.net/en/)PnlPercentType

| [PnlPercentType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87) ATAS.DataFeedsCore.Position.PnlPercentType = [PnlPercentType.Portfolio](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87ad4f859a96c13f551a2771b7fc3a78d38) |
| --- |

getset

Calculate pnl percent from portfolio balance or position margin.

## [◆](https://docs.atas.net/en/)Portfolio

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.DataFeedsCore.Position.Portfolio |
| --- |

getset

Gets or sets the portfolio associated with this position.

## [◆](https://docs.atas.net/en/)RealizedPnL

| decimal ATAS.DataFeedsCore.Position.RealizedPnL |
| --- |

getset

Gets or sets the realized profit or loss associated with this position.

## [◆](https://docs.atas.net/en/)Risk

| [RiskInfo](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a5782229ea6bf214ef71162db25fbfd1b)? ATAS.DataFeedsCore.Position.Risk |
| --- |

getset

Gets or sets information about marginal trading options. Null if marginal trading is not supported.

## [◆](https://docs.atas.net/en/)Security

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) ATAS.DataFeedsCore.Position.Security |
| --- |

getset

Gets or sets the security associated with this position.

## [◆](https://docs.atas.net/en/)SecurityId

| string ATAS.DataFeedsCore.Position.SecurityId |
| --- |

getset

Gets or sets the unique identifier of the security associated with this position.

## [◆](https://docs.atas.net/en/)TPlusLimit

| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? ATAS.DataFeedsCore.Position.TPlusLimit |
| --- |

getset

Gets or sets the T+ limit associated with this position, if applicable.

## [◆](https://docs.atas.net/en/)UnrealizedPnL

| decimal ATAS.DataFeedsCore.Position.UnrealizedPnL |
| --- |

getset

Gets or sets the unrealized profit or loss associated with this position.

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.Position.Volume |
| --- |

getset

Gets or sets the current volume of the position.

Volume is positive for LONG positions and negative for SHORT.

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.Position.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [Position.cs](../files/Position_8cs.md)
