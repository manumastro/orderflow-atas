# ATAS.DataFeedsCore.MarketByOrdersManager Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.html

Manager that provides access to market by order data.
 [More...](./classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md#details)

Inheritance diagram for ATAS.DataFeedsCore.MarketByOrdersManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.MarketByOrdersManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Update](./classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md#afa8497389d6b7637265635dc6e8bc321) (IReadOnlyCollection values) |
| | Update an snapshot of the current market by order data. |
| | |

| Properties | |
| --- | --- |
| IEnumerable | [MarketByOrders](./classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md#a7ffbbaafa098a8321a20c0685eddf41e)`[get]` |
| | Gets a snapshot of the current market by order data. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IMarketByOrdersManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md) | |
| IEnumerable | [MarketByOrders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md#aa16e5582e02892df31b84fc27363a81f)`[get]` |
| | Gets a snapshot of the current market by order data. |
| | |

| Events | |
| --- | --- |
| Action >? | [Changed](./classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md#a0dfa07b75608db524f8e2caf7805b452) |
| | |
| - Events inherited from [ATAS.DataFeedsCore.IMarketByOrdersManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md) | |
| Action >? | [Changed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md#ad7f415ab2ad44afe1443f9afd00cb954) |
| | Event that is raised when real-time market by order data have changed. |
| | |

## Detailed Description

Manager that provides access to market by order data.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Update()

| void ATAS.DataFeedsCore.MarketByOrdersManager.Update | ( | IReadOnlyCollection | values | ) | |
| --- | --- | --- | --- | --- | --- |

Update an snapshot of the current market by order data.

Parameters

| values | Changed items. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)MarketByOrders

| IEnumerable ATAS.DataFeedsCore.MarketByOrdersManager.MarketByOrders |
| --- |

get

Gets a snapshot of the current market by order data.

Implements [ATAS.DataFeedsCore.IMarketByOrdersManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md#aa16e5582e02892df31b84fc27363a81f).

## Event Documentation

## [◆](https://docs.atas.net/en/)Changed

| Action >? ATAS.DataFeedsCore.MarketByOrdersManager.Changed |
| --- |

The documentation for this class was generated from the following file:
- [MarketByOrdersManager.cs](../files/MarketByOrdersManager_8cs.md)
