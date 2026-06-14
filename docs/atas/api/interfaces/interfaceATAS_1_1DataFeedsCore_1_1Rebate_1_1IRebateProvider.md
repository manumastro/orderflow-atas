# ATAS.DataFeedsCore.Rebate.IRebateProvider Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateProvider.html

Allows to obtain user affiliation status needed to calculate correct commission rebates on crypto exchanges.
 [More...](./interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateProvider.md#details)

| Public Member Functions | |
| --- | --- |
| [IRebateInfo](./interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateInfo.md)? | [GetRebateInfo](./interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateProvider.md#a19fe5c12139faa795127e7abc0ce062d) () |
| | Tries to obtain rebate information for a connector This method may work only when connected to a exchange. |
| | |

## Detailed Description

Allows to obtain user affiliation status needed to calculate correct commission rebates on crypto exchanges.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetRebateInfo()

| [IRebateInfo](./interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateInfo.md)? ATAS.DataFeedsCore.Rebate.IRebateProvider.GetRebateInfo | ( | | ) | |
| --- | --- | --- | --- | --- |

Tries to obtain rebate information for a connector This method may work only when connected to a exchange.

Returns

The documentation for this interface was generated from the following file:
- [IRebateProvider.cs](../files/IRebateProvider_8cs.md)
