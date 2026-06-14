# ATAS.DataFeedsCore.Commissions.CommissionRule Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.html

Inheritance diagram for ATAS.DataFeedsCore.Commissions.CommissionRule:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Commissions.CommissionRule:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| virtual decimal | [Process](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#ac7bbd8986558c77206cab0bebad654fd) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| virtual decimal | [Process](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a931606b29ca724fb059858041d0d73cd) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| virtual void | [Load](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#aa273dbe5e103ed7d7c01f3a5996d6827) (string value) |
| | |
| virtual string | [Save](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a6453358f4b5a1c4cb14de8cb2f8fbf8d) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a711b115dab316ed6cce49b78726aabf3) () |
| | Returns a string that represents the current object. |
| | |
| decimal | [Process](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#afa9c38ca3e92d5ca177da03632e44077) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| decimal | [Process](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#ae8a90f0942f2199e60abd30c063a506d) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [Load](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a22b710dd91f2834c7de3e4ce7e88ffba) (string value) |
| | |
| string | [Save](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a0cc3560bc842e5f8be943003b86be82a) () |
| | |

| Protected Member Functions | |
| --- | --- |
| | [CommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a23c8636b01444cbe359ffb0b6245df44) () |
| | |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#ab3b08a57acdfbbe862c5e7b23574e431) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| bool | [NeedSave](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#acbb7a20487bf6a4499d079c160a9a98c)`[get, protected set]` |
| | |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a8ce3822f130bc228115a1df5b75a61ae)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md) | |
| bool | [NeedSave](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a9f20e6495d254579c2e3a18ee849438f)`[get]` |
| | |
| string | [Name](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#ad3c0eecb08d314f7dcbc9a01b817f169)`[get, set]` |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a8b8983c3791af891b1f3382da47fe934) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)CommissionRule()

| ATAS.DataFeedsCore.Commissions.CommissionRule.CommissionRule | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Load()

| virtual void ATAS.DataFeedsCore.Commissions.CommissionRule.Load | ( | string | value | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Implements [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a22b710dd91f2834c7de3e4ce7e88ffba).

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.Commissions.CommissionRule.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)Process() [1/2]

| virtual decimal ATAS.DataFeedsCore.Commissions.CommissionRule.Process | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Implements [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#ae8a90f0942f2199e60abd30c063a506d).

Reimplemented in [ATAS.DataFeedsCore.Commissions.PerContractCommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerContractCommissionRule.md#ae5a8527bb77dbf42d1880db3bd56ff02), [ATAS.DataFeedsCore.Commissions.PerTradeCommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerTradeCommissionRule.md#a72b8fab072b7f2ff0cc799ee1d328179), and [ATAS.DataFeedsCore.Commissions.PercentCommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1PercentCommissionRule.md#af54c5e5914cb038e4b2b07ec88b3ab6f).

## [◆](https://docs.atas.net/en/)Process() [2/2]

| virtual decimal ATAS.DataFeedsCore.Commissions.CommissionRule.Process | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Implements [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#afa9c38ca3e92d5ca177da03632e44077).

## [◆](https://docs.atas.net/en/)Save()

| virtual string ATAS.DataFeedsCore.Commissions.CommissionRule.Save | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Implements [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a0cc3560bc842e5f8be943003b86be82a).

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.Commissions.CommissionRule.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## Property Documentation

## [◆](https://docs.atas.net/en/)Name

| string ATAS.DataFeedsCore.Commissions.CommissionRule.Name |
| --- |

getset

Implements [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#ad3c0eecb08d314f7dcbc9a01b817f169).

## [◆](https://docs.atas.net/en/)NeedSave

| bool ATAS.DataFeedsCore.Commissions.CommissionRule.NeedSave |
| --- |

getprotected set

Implements [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a9f20e6495d254579c2e3a18ee849438f).

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.Commissions.CommissionRule.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [CommissionRule.cs](../files/CommissionRule_8cs.md)
