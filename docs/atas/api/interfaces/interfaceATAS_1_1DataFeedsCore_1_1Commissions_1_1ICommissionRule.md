# ATAS.DataFeedsCore.Commissions.ICommissionRule Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.html

Inheritance diagram for ATAS.DataFeedsCore.Commissions.ICommissionRule:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| decimal | [Process](./interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#afa9c38ca3e92d5ca177da03632e44077) ([Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| decimal | [Process](./interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#ae8a90f0942f2199e60abd30c063a506d) ([MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [Load](./interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a22b710dd91f2834c7de3e4ce7e88ffba) (string value) |
| | |
| string | [Save](./interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a0cc3560bc842e5f8be943003b86be82a) () |
| | |

| Properties | |
| --- | --- |
| bool | [NeedSave](./interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a9f20e6495d254579c2e3a18ee849438f)`[get]` |
| | |
| string | [Name](./interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#ad3c0eecb08d314f7dcbc9a01b817f169)`[get, set]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Load()

| void ATAS.DataFeedsCore.Commissions.ICommissionRule.Load | ( | string | value | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#aa273dbe5e103ed7d7c01f3a5996d6827).

## [◆](https://docs.atas.net/en/)Process() [1/2]

| decimal ATAS.DataFeedsCore.Commissions.ICommissionRule.Process | ( | [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a931606b29ca724fb059858041d0d73cd), [ATAS.DataFeedsCore.Commissions.PerContractCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerContractCommissionRule.md#ae5a8527bb77dbf42d1880db3bd56ff02), [ATAS.DataFeedsCore.Commissions.PerTradeCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerTradeCommissionRule.md#a72b8fab072b7f2ff0cc799ee1d328179), and [ATAS.DataFeedsCore.Commissions.PercentCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PercentCommissionRule.md#af54c5e5914cb038e4b2b07ec88b3ab6f).

## [◆](https://docs.atas.net/en/)Process() [2/2]

| decimal ATAS.DataFeedsCore.Commissions.ICommissionRule.Process | ( | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#ac7bbd8986558c77206cab0bebad654fd).

## [◆](https://docs.atas.net/en/)Save()

| string ATAS.DataFeedsCore.Commissions.ICommissionRule.Save | ( | | ) | |
| --- | --- | --- | --- | --- |

Implemented in [ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a6453358f4b5a1c4cb14de8cb2f8fbf8d).

## Property Documentation

## [◆](https://docs.atas.net/en/)Name

| string ATAS.DataFeedsCore.Commissions.ICommissionRule.Name |
| --- |

getset

Implemented in [ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a8ce3822f130bc228115a1df5b75a61ae).

## [◆](https://docs.atas.net/en/)NeedSave

| bool ATAS.DataFeedsCore.Commissions.ICommissionRule.NeedSave |
| --- |

get

Implemented in [ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#acbb7a20487bf6a4499d079c160a9a98c).

The documentation for this interface was generated from the following file:
- [CommissionRule.cs](../files/CommissionRule_8cs.md)
