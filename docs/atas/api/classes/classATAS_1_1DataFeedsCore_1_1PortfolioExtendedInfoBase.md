# ATAS.DataFeedsCore.PortfolioExtendedInfoBase Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.html

Base implementation of IPortfolioExtendedInfo with common functionality.
 [More...](./classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.md#details)

Inheritance diagram for ATAS.DataFeedsCore.PortfolioExtendedInfoBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.PortfolioExtendedInfoBase:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| void | [UpdateTimestamp](./classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.md#a8107e09bfd8d8cd791532e91e70cf2af) () |
| | Updates the last update time to the current time. |
| | |

| Properties | |
| --- | --- |
| DateTime | [LastUpdateTime](./classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.md#a585ba64f42f473af1f06fe9119f67591)`[get]` |
| | Time of the last data update. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IPortfolioExtendedInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md) | |
| DateTime | [LastUpdateTime](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md#a68499b174e56a48eb9151278fff60666)`[get]` |
| | Time of the last data update. |
| | |

## Detailed Description

Base implementation of IPortfolioExtendedInfo with common functionality.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)UpdateTimestamp()

| void ATAS.DataFeedsCore.PortfolioExtendedInfoBase.UpdateTimestamp | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Updates the last update time to the current time.

## Property Documentation

## [◆](https://docs.atas.net/en/)LastUpdateTime

| DateTime ATAS.DataFeedsCore.PortfolioExtendedInfoBase.LastUpdateTime |
| --- |

get

Time of the last data update.

Implements [ATAS.DataFeedsCore.IPortfolioExtendedInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md#a68499b174e56a48eb9151278fff60666).

The documentation for this class was generated from the following file:
- [IPortfolioExtendedInfo.cs](../files/IPortfolioExtendedInfo_8cs.md)
