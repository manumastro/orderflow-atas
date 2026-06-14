# ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.html

| Public Member Functions | |
| --- | --- |
| Task | [LoadHistoryAsync](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#abe51725c715cc23e9943775485f6d6bc) (DateTime from, DateTime to, ICollection? accounts=null, ICollection? securities=null) |
| | |

| Static Public Attributes | |
| --- | --- |
| const long | [NoPlaybookId](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a556c9fa59fd20702a67199e59589193d) = -1 |
| | Special ID to filter trades without any playbook assigned. Include this ID in the Playbooks filter to show trades with no playbooks. |
| | |

| Properties | |
| --- | --- |
| DateTime | [From](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a2708c33c2be3e7da46cd61cc0d852f75)`[get, set]` |
| | |
| DateTime | [To](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#aa91c881b0dc78a6b0f26cdd19c463bfd)`[get, set]` |
| | |
| IReadOnlySet? | [Accounts](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#afca96facebd9b3482c38fe958967e0c2)`[get, set]` |
| | |
| IReadOnlySet? | [Securities](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a0a90b3b5e472738c2d54ea062baf2b8d)`[get, set]` |
| | |
| IReadOnlySet? | [Playbooks](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a04826c98ab1dccc5a92d902051e5c32d)`[get, set]` |
| | |
| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) | [FilteredStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a2a1af432d1d194bd0412d322b5a3f5e2)`[get]` |
| | |
| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) | [RawStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#aa4b2fd6b4db132376b04a6308cc2d813)`[get]` |
| | |
| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) | [Replay](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a7037e63c8fdf62358c6301cf99678de8)`[get]` |
| | |
| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) | [Realtime](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#ababb7b8655b614919db56107efbb89f7)`[get]` |
| | |

| Events | |
| --- | --- |
| Action? | [FilteredStatisticsSourceChanged](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a25cbc9be30debdea3e43376b4dff79b3) |
| | |
| Action? | [RawStatisticsSourceChanged](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#a5b645e3e1ed3f169a05a012bb01fdc5d) |
| | |
| Action? | [StatisticsRebuilt](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#aa87f2c6f263937ecb0340c27f80d3b7e) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)LoadHistoryAsync()

| Task ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.LoadHistoryAsync | ( | DateTime | from, |
| --- | --- | --- | --- |
| | | DateTime | to, |
| | | ICollection? | accounts = `null`, |
| | | ICollection? | securities = `null` |
| | ) | | |

## Member Data Documentation

## [◆](https://docs.atas.net/en/)NoPlaybookId

| const long ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.NoPlaybookId = -1 |
| --- |

static

Special ID to filter trades without any playbook assigned. Include this ID in the Playbooks filter to show trades with no playbooks.

## Property Documentation

## [◆](https://docs.atas.net/en/)Accounts

| IReadOnlySet? ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.Accounts |
| --- |

getset

## [◆](https://docs.atas.net/en/)FilteredStatistics

| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.FilteredStatistics |
| --- |

get

## [◆](https://docs.atas.net/en/)From

| DateTime ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.From |
| --- |

getset

## [◆](https://docs.atas.net/en/)Playbooks

| IReadOnlySet? ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.Playbooks |
| --- |

getset

## [◆](https://docs.atas.net/en/)RawStatistics

| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.RawStatistics |
| --- |

get

## [◆](https://docs.atas.net/en/)Realtime

| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.Realtime |
| --- |

get

## [◆](https://docs.atas.net/en/)Replay

| [ITradingStatistics](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.Replay |
| --- |

get

## [◆](https://docs.atas.net/en/)Securities

| IReadOnlySet? ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.Securities |
| --- |

getset

## [◆](https://docs.atas.net/en/)To

| DateTime ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.To |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)FilteredStatisticsSourceChanged

| Action? ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.FilteredStatisticsSourceChanged |
| --- |

## [◆](https://docs.atas.net/en/)RawStatisticsSourceChanged

| Action? ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.RawStatisticsSourceChanged |
| --- |

## [◆](https://docs.atas.net/en/)StatisticsRebuilt

| Action? ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider.StatisticsRebuilt |
| --- |

The documentation for this interface was generated from the following file:
- [ITradingStatisticsProvider.cs](../files/ITradingStatisticsProvider_8cs.md)
