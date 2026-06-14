# ATAS.DataFeedsCore.Statistics.TradingStatistics Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.html

Inheritance diagram for ATAS.DataFeedsCore.Statistics.TradingStatistics:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Statistics.TradingStatistics:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [TradingStatistics](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a37a78384fc20b40fcade263e6f7a008f) () |
| | |
| void | [ClearHistoryMyTrades](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#ab61f12b6d9867df5c8778028cae27d67) () |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a0213e2f10880bf02e2b21f38eb61a177) () |
| | |
| void | [Add](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a0d8ced9c5f0c0b13bbdea96eb34f15e8) ([DailyNote](./classATAS_1_1DataFeedsCore_1_1DailyNote.md) note) |
| | |
| void | [Remove](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a9a8e09932bb5dc303f141d2d22d3334c) ([DailyNote](./classATAS_1_1DataFeedsCore_1_1DailyNote.md) note) |
| | |
| void | [Add](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a082043ebd9b00735c4b6b512c6b84096) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [Add](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a55b56a796b1410f121d1eb15778a26e8) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [Add](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a60bfe4b7bb8e11da0f16f1e6a8bbb1ad) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) historyTrade) |
| | |
| void | [Update](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a141bd4a2a102f10366e07e84c451c2ea) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) historyTrade) |
| | |
| void | [Update](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#ab0917afa89e3ed77a5451f42fb470469) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| void | [RecalcMetrics](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a7010a434925ea982055e51ec506f33f9) () |
| | |

| Properties | |
| --- | --- |
| IMutableEnumerable | [HistoryMyTrades](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a1cbccc392b9792921309be1fd4bca4f0)`[get]` |
| | |
| IMutableEnumerable | [Orders](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#aa3840e8494668ba8db42224b328e6127)`[get]` |
| | |
| IMutableEnumerable | [MyTrades](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a4c5d9039ed7977389f92573d92d0f6b3)`[get]` |
| | |
| IMutableEnumerable | [Statistics](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#af5a6b172c4cbd1f267f0985ac2ffc335)`[get]` |
| | |
| IMutableEnumerable | [Equity](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#afd06a1112b677983d14207b09c81f4c0)`[get]` |
| | |
| IMutableEnumerable | [DailyNotes](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md#a2997571777e1dedc477c983baf068119)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) | |
| IMutableEnumerable | [Orders](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a6219d8db3ca3d5bd1dc97a1ff8b634f7)`[get]` |
| | |
| IMutableEnumerable | [MyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a93e7b9de7e4ca8ee3b465426d9e28ca3)`[get]` |
| | |
| IMutableEnumerable | [HistoryMyTrades](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a4b67cc5eeb87fcc09ceb5bd1eb59e68e)`[get]` |
| | |
| IMutableEnumerable | [DailyNotes](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#ada7e778c49e8a8cd4c22ee01d09db75b)`[get]` |
| | |
| IMutableEnumerable | [Equity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a2737aff6dc45dddf35a4b752e91bab10)`[get]` |
| | |
| IMutableEnumerable | [Statistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a9d606218cfa29406733241babbfb66f1)`[get]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)TradingStatistics()

| ATAS.DataFeedsCore.Statistics.TradingStatistics.TradingStatistics | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Add() [1/4]

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Add | ( | [DailyNote](./classATAS_1_1DataFeedsCore_1_1DailyNote.md) | note | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Add() [2/4]

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Add | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | historyTrade | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Add() [3/4]

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Add | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Add() [4/4]

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Add | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Clear()

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ClearHistoryMyTrades()

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.ClearHistoryMyTrades | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)RecalcMetrics()

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.RecalcMetrics | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Remove()

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Remove | ( | [DailyNote](./classATAS_1_1DataFeedsCore_1_1DailyNote.md) | note | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Update() [1/2]

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Update | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | historyTrade | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Update() [2/2]

| void ATAS.DataFeedsCore.Statistics.TradingStatistics.Update | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)DailyNotes

| IMutableEnumerable ATAS.DataFeedsCore.Statistics.TradingStatistics.DailyNotes |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#ada7e778c49e8a8cd4c22ee01d09db75b).

## [◆](https://docs.atas.net/en/)Equity

| IMutableEnumerable ATAS.DataFeedsCore.Statistics.TradingStatistics.Equity |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a2737aff6dc45dddf35a4b752e91bab10).

## [◆](https://docs.atas.net/en/)HistoryMyTrades

| IMutableEnumerable ATAS.DataFeedsCore.Statistics.TradingStatistics.HistoryMyTrades |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a4b67cc5eeb87fcc09ceb5bd1eb59e68e).

## [◆](https://docs.atas.net/en/)MyTrades

| IMutableEnumerable ATAS.DataFeedsCore.Statistics.TradingStatistics.MyTrades |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a93e7b9de7e4ca8ee3b465426d9e28ca3).

## [◆](https://docs.atas.net/en/)Orders

| IMutableEnumerable ATAS.DataFeedsCore.Statistics.TradingStatistics.Orders |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a6219d8db3ca3d5bd1dc97a1ff8b634f7).

## [◆](https://docs.atas.net/en/)Statistics

| IMutableEnumerable ATAS.DataFeedsCore.Statistics.TradingStatistics.Statistics |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a9d606218cfa29406733241babbfb66f1).

The documentation for this class was generated from the following file:
- [TradingStatistics.cs](../files/TradingStatistics_8cs.md)
