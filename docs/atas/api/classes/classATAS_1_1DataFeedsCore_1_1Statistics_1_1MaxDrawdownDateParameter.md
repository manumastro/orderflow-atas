# ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownDateParameter.html

Inheritance diagram for ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| override decimal | [OnProcess](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownDateParameter.md#a0a840a631875b2fe33d4e9b1556f52d6) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| override void | [OnClear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownDateParameter.md#a242240fc2bb941d973fd5b38cc8a080e) () |
| | |
| - Protected Member Functions inherited from [ATAS.DataFeedsCore.Statistics.StatisticsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md) | |
| abstract decimal | [OnProcess](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#adad0f6b29043f43618d2ea151a4e1616) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| virtual void | [OnClear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#ab18a24ede0757d4696353ecc16c39fa9) () |
| | |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#a75b0cba0419da911b9813383102bbb89) (string propertyName) |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.Statistics.StatisticsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md) | |
| void | [Process](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#aaf3720786a6714f11f944981da659c0a) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#a1a35ba3d9073190accec3427b055a003) () |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#abf4b9bebd7b9f1ce72e7bbb2fefa5ec7) () |
| | |
| void | [Process](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#a5cfbafd4b6ef153df497a6ec0bb7aaf0) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Statistics.StatisticsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md) | |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#ad4de46e8e6c47df5caba8cc6531480d8)`[get]` |
| | |
| decimal | [Value](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#a5b525d444a832fe759cdfa0d486593d0)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Statistics.IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md) | |
| string | [Name](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#a3366b2554d89cadfa84e43d043914504)`[get]` |
| | |
| decimal | [Value](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#ae8cf05abca368a81db081cd3016c8810)`[get]` |
| | |
| - Events inherited from [ATAS.DataFeedsCore.Statistics.StatisticsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md) | |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#aab6b78253123141aaa9d9aa7fca580aa) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnClear()

| override void ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter.OnClear | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.DataFeedsCore.Statistics.StatisticsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#ab18a24ede0757d4696353ecc16c39fa9).

## [◆](https://docs.atas.net/en/)OnProcess()

| override decimal ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter.OnProcess | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Implements [ATAS.DataFeedsCore.Statistics.StatisticsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#adad0f6b29043f43618d2ea151a4e1616).

The documentation for this class was generated from the following file:
- [StatisticsParameters.cs](../files/StatisticsParameters_8cs.md)
