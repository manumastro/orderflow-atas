# ATAS.DataFeedsCore.Statistics.StatisticsManager Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.html

Inheritance diagram for ATAS.DataFeedsCore.Statistics.StatisticsManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Statistics.StatisticsManager:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [StatisticsManager](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a70cae1876f03bdad3343e908a85b3dd9) () |
| | |
| virtual void | [Clear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a60bce64f0592bb336522a098c46c709b) (bool clearTradesQueue=false) |
| | |
| virtual void | [Process](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a89b8ba92941aeedd6315bef541be34b9) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| virtual void | [Process](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#ac11ad5f75fcd6295aea842f8cfbafb83) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [UpdateStatistics](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a16ab537de962d42fe5c50747484f3408) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| virtual void | [Add](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a71eab6a04eefcb8b56e4d58fdf92bfda) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) |
| | |
| virtual void | [Add](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a34bd34a63761b98e8e7229c380997ab8) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| void | [AddToStatistics](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a87d059b671395add5ce328f4801af654) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade) |
| | |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a55f25f54803e38121e2e9b37e37a02be) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| ThreadSafeObservableCollection | [HistoryMyTrades](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a4008af2c1b4f5fcd0070ca2161ce2030) = new()`[get]` |
| | |
| ThreadSafeObservableCollection | [Orders](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a9be328b27b8dcc65d50221829e37ac2d) = new()`[get]` |
| | |
| ThreadSafeObservableCollection | [MyTrades](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a8cb62d7d8ccc3eda0d49c34e5ab6e23b) = new()`[get]` |
| | |
| ThreadSafeObservableCollection | [Statistics](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a296daa56aec70f18c0f08497a904b029) = new()`[get]` |
| | |
| ThreadSafeObservableCollection > | [Equity](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#ad0de2f0ce464208167f527133125f8c2) = new()`[get]` |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md#a50df7b077551f819b0155d2e3ebe2d2f) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)StatisticsManager()

| ATAS.DataFeedsCore.Statistics.StatisticsManager.StatisticsManager | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Add() [1/2]

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsManager.Add | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)Add() [2/2]

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsManager.Add | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)AddToStatistics()

| void ATAS.DataFeedsCore.Statistics.StatisticsManager.AddToStatistics | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)Clear()

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsManager.Clear | ( | bool | clearTradesQueue = `false` | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsManager.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)Process() [1/2]

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsManager.Process | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

## [◆](https://docs.atas.net/en/)Process() [2/2]

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsManager.Process | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

virtual

## [◆](https://docs.atas.net/en/)UpdateStatistics()

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsManager.UpdateStatistics | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## Property Documentation

## [◆](https://docs.atas.net/en/)Equity

| ThreadSafeObservableCollection > ATAS.DataFeedsCore.Statistics.StatisticsManager.Equity = new() |
| --- |

get

## [◆](https://docs.atas.net/en/)HistoryMyTrades

| ThreadSafeObservableCollection ATAS.DataFeedsCore.Statistics.StatisticsManager.HistoryMyTrades = new() |
| --- |

get

## [◆](https://docs.atas.net/en/)MyTrades

| ThreadSafeObservableCollection ATAS.DataFeedsCore.Statistics.StatisticsManager.MyTrades = new() |
| --- |

get

## [◆](https://docs.atas.net/en/)Orders

| ThreadSafeObservableCollection ATAS.DataFeedsCore.Statistics.StatisticsManager.Orders = new() |
| --- |

get

## [◆](https://docs.atas.net/en/)Statistics

| ThreadSafeObservableCollection ATAS.DataFeedsCore.Statistics.StatisticsManager.Statistics = new() |
| --- |

get

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.Statistics.StatisticsManager.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [StatisticsManager.cs](../files/StatisticsManager_8cs.md)
