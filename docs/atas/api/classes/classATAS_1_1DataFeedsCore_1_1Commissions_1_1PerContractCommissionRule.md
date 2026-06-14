# ATAS.DataFeedsCore.Commissions.PerContractCommissionRule Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerContractCommissionRule.html

Inheritance diagram for ATAS.DataFeedsCore.Commissions.PerContractCommissionRule:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Commissions.PerContractCommissionRule:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| override decimal | [Process](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerContractCommissionRule.md#ae5a8527bb77dbf42d1880db3bd56ff02) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.Commissions.CommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md) | |
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

| Properties | |
| --- | --- |
| decimal | [Commission](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerContractCommissionRule.md#af35e65b21ea713bc19db516e287439a8)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Commissions.CommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md) | |
| bool | [NeedSave](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#acbb7a20487bf6a4499d079c160a9a98c)`[get, protected set]` |
| | |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a8ce3822f130bc228115a1df5b75a61ae)`[get, set]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md) | |
| bool | [NeedSave](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#a9f20e6495d254579c2e3a18ee849438f)`[get]` |
| | |
| string | [Name](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md#ad3c0eecb08d314f7dcbc9a01b817f169)`[get, set]` |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Protected Member Functions inherited from [ATAS.DataFeedsCore.Commissions.CommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md) | |
| | [CommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a23c8636b01444cbe359ffb0b6245df44) () |
| | |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#ab3b08a57acdfbbe862c5e7b23574e431) (string propertyName) |
| | |
| - Events inherited from [ATAS.DataFeedsCore.Commissions.CommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md) | |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a8b8983c3791af891b1f3382da47fe934) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Process()

| override decimal ATAS.DataFeedsCore.Commissions.PerContractCommissionRule.Process | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.DataFeedsCore.Commissions.CommissionRule](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md#a931606b29ca724fb059858041d0d73cd).

## Property Documentation

## [◆](https://docs.atas.net/en/)Commission

| decimal ATAS.DataFeedsCore.Commissions.PerContractCommissionRule.Commission |
| --- |

getset

The documentation for this class was generated from the following file:
- [CommissionRule.cs](../files/CommissionRule_8cs.md)
