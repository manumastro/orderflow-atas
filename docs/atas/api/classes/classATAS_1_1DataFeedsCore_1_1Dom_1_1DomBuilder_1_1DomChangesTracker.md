# ATAS.DataFeedsCore.Dom.DomBuilder.DomChangesTracker Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.html

| Public Member Functions | |
| --- | --- |
| void | [TrackChange](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md#a4c12f55d9706de9f4e54fe770fc47706) ([MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) md) |
| | |
| List | [GetChanges](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md#a881e0f38289b02b76a358803e74bc00d) () |
| | |

| Properties | |
| --- | --- |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)? | [NewBestAsk](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md#a9200889b6964c7b514e13c53dbcd5ea7)`[get]` |
| | Changed new best ASK value or null if value didn't change. |
| | |
| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)? | [NewBestBid](./classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md#a0339f1ca977e30ee4539fbf06c4a51ee)`[get]` |
| | Changed new best BID value or null if value didn't change. |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetChanges()

| List ATAS.DataFeedsCore.Dom.DomBuilder.DomChangesTracker.GetChanges | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)TrackChange()

| void ATAS.DataFeedsCore.Dom.DomBuilder.DomChangesTracker.TrackChange | ( | [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | md | ) | |
| --- | --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)NewBestAsk

| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)? ATAS.DataFeedsCore.Dom.DomBuilder.DomChangesTracker.NewBestAsk |
| --- |

get

Changed new best ASK value or null if value didn't change.

## [◆](https://docs.atas.net/en/)NewBestBid

| [MarketDepth](./classATAS_1_1DataFeedsCore_1_1MarketDepth.md)? ATAS.DataFeedsCore.Dom.DomBuilder.DomChangesTracker.NewBestBid |
| --- |

get

Changed new best BID value or null if value didn't change.

The documentation for this class was generated from the following file:
- [DomBuilder.cs](../files/DomBuilder_8cs.md)
