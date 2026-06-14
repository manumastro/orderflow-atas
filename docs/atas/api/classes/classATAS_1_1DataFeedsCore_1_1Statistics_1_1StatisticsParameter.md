# ATAS.DataFeedsCore.Statistics.StatisticsParameter Class Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.html

Inheritance diagram for ATAS.DataFeedsCore.Statistics.StatisticsParameter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Statistics.StatisticsParameter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Process](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#aaf3720786a6714f11f944981da659c0a) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#a1a35ba3d9073190accec3427b055a003) () |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#abf4b9bebd7b9f1ce72e7bbb2fefa5ec7) () |
| | |
| void | [Process](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#a5cfbafd4b6ef153df497a6ec0bb7aaf0) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |

| Protected Member Functions | |
| --- | --- |
| abstract decimal | [OnProcess](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#adad0f6b29043f43618d2ea151a4e1616) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| virtual void | [OnClear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#ab18a24ede0757d4696353ecc16c39fa9) () |
| | |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#a75b0cba0419da911b9813383102bbb89) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#ad4de46e8e6c47df5caba8cc6531480d8)`[get]` |
| | |
| decimal | [Value](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#a5b525d444a832fe759cdfa0d486593d0)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Statistics.IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md) | |
| string | [Name](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#a3366b2554d89cadfa84e43d043914504)`[get]` |
| | |
| decimal | [Value](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#ae8cf05abca368a81db081cd3016c8810)`[get]` |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md#aab6b78253123141aaa9d9aa7fca580aa) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void ATAS.DataFeedsCore.Statistics.StatisticsParameter.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#abf4b9bebd7b9f1ce72e7bbb2fefa5ec7).

## [◆](https://docs.atas.net/en/)OnClear()

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsParameter.OnClear | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented in [ATAS.DataFeedsCore.Statistics.AvgPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgPnLParameter.md#a26045642f93335bc829ddc58908bf498), [ATAS.DataFeedsCore.Statistics.MaxDrawdownParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownParameter.md#a40adc9d5a169b9a128152457a37623ec), [ATAS.DataFeedsCore.Statistics.MaxRelativeDrawdownParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxRelativeDrawdownParameter.md#aa705dab0cd3435a217c2619ecaa28bf3), [ATAS.DataFeedsCore.Statistics.RecoveryFactorParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1RecoveryFactorParameter.md#a4e39a34a845b51694365e821f209df3a), [ATAS.DataFeedsCore.Statistics.ProfitFactorParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitFactorParameter.md#a32a1412141381ebe66596e3617524976), [ATAS.DataFeedsCore.Statistics.AvgProfitParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgProfitParameter.md#a43a4ec0ac5730cb5c0e87604f5d94e85), [ATAS.DataFeedsCore.Statistics.AvgLossParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossParameter.md#a6918aab1759b1f7050f66bb424491099), [ATAS.DataFeedsCore.Statistics.TotalDaysParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalDaysParameter.md#ad5ac71754857c82fae13d9b30df314f0), [ATAS.DataFeedsCore.Statistics.LossDaysParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossDaysParameter.md#ab8aebec5d99eb0785bf518c3e7a31329), [ATAS.DataFeedsCore.Statistics.ProfitDaysParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitDaysParameter.md#aa63df79748bb0be52703594849e43ea5), [ATAS.DataFeedsCore.Statistics.NetPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1NetPnLParameter.md#a13b1f843e456578d044e1d199c17d60f), [ATAS.DataFeedsCore.Statistics.WinRateParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinRateParameter.md#afaa2dee9b74de26e22983b9778a436aa), [ATAS.DataFeedsCore.Statistics.AvgWinParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgWinParameter.md#a17866f80b128ee9d0aac7946a7ac91fe), [ATAS.DataFeedsCore.Statistics.AvgLossInMoneyParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossInMoneyParameter.md#aab0fd97d4fdf405c86994f446a7f07e3), [ATAS.DataFeedsCore.Statistics.WinLossRatioParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinLossRatioParameter.md#a8cedfbdf37495d9cc29b97e752c69013), [ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownDateParameter.md#a242240fc2bb941d973fd5b38cc8a080e), [ATAS.DataFeedsCore.Statistics.DailyPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1DailyPnLParameter.md#ae5a72747de617056e6c1a0a216092432), [ATAS.DataFeedsCore.Statistics.WinningDaysPercentParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinningDaysPercentParameter.md#a1e8a57e9dc37fad895f09c23a50128a7), [ATAS.DataFeedsCore.Statistics.SharpeRatioParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1SharpeRatioParameter.md#a931533d66c4916ced56188d12aacfd8b), [ATAS.DataFeedsCore.Statistics.AvgTradesPerDayParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradesPerDayParameter.md#a4354111ede46d079ed5f132639724d66), [ATAS.DataFeedsCore.Statistics.AvgTradeLengthParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradeLengthParameter.md#af55017cf3b1b4cc4c3cdebc5a86aac4c), [ATAS.DataFeedsCore.Statistics.MaxConsecutiveWinsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveWinsParameter.md#a36ac60379d50868e4181af2ab215ee01), [ATAS.DataFeedsCore.Statistics.MaxConsecutiveLossesParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveLossesParameter.md#ab3d4cf7926d587187521f85cc85b3b47), [ATAS.DataFeedsCore.Statistics.AccountAgeParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AccountAgeParameter.md#ae1da79caa19d2fae9cc8dfea2cd2638b), and [ATAS.DataFeedsCore.Statistics.LastTradeDateParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1LastTradeDateParameter.md#a7e9d6cb9c741b1930b7e40060131419a).

## [◆](https://docs.atas.net/en/)OnProcess()

| abstract decimal ATAS.DataFeedsCore.Statistics.StatisticsParameter.OnProcess | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

Implemented in [ATAS.DataFeedsCore.Statistics.TotalTradesParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalTradesParameter.md#aff93018c259d1b92c0f9c52f5adbd83b), [ATAS.DataFeedsCore.Statistics.TotalPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalPnLParameter.md#a8ee68966ecd8ad458593c36ec154f3f5), [ATAS.DataFeedsCore.Statistics.AvgPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgPnLParameter.md#a92e94424c86164ad7377c48267fce54c), [ATAS.DataFeedsCore.Statistics.MaxDrawdownParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownParameter.md#a147e3f8a1035b762cce766c0a571c0e2), [ATAS.DataFeedsCore.Statistics.MaxRelativeDrawdownParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxRelativeDrawdownParameter.md#ad25c78b317627fbeacba8aafca373502), [ATAS.DataFeedsCore.Statistics.RecoveryFactorParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1RecoveryFactorParameter.md#a95cd10b95df5ac00de5a5904987d987b), [ATAS.DataFeedsCore.Statistics.ProfitFactorParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitFactorParameter.md#a31c6cffb9dda6c66f6dd95e3533ffe39), [ATAS.DataFeedsCore.Statistics.ProfitPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitPnLParameter.md#a2ad4199a9cf561bd5160e7294f3c27ee), [ATAS.DataFeedsCore.Statistics.ProfitTradesParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitTradesParameter.md#a747df08f841878db47d62d9a477b1d56), [ATAS.DataFeedsCore.Statistics.AvgProfitParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgProfitParameter.md#a12fb3bbf370a1019522ca044c20549e0), [ATAS.DataFeedsCore.Statistics.LossPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossPnLParameter.md#a7e9c59f63e725ff395c680a9c55456dc), [ATAS.DataFeedsCore.Statistics.LossTradesParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossTradesParameter.md#a431403dc50db8e8da62c6237ffc08339), [ATAS.DataFeedsCore.Statistics.AvgLossParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossParameter.md#a2734f72ba18c25d73e1a5b999b69501b), [ATAS.DataFeedsCore.Statistics.TotalDaysParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalDaysParameter.md#a62406ae38b2d3dc468d0cfaf311993b1), [ATAS.DataFeedsCore.Statistics.LossDaysParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossDaysParameter.md#ac31b81a6fc8c55861c8ac4bdfa2f002d), [ATAS.DataFeedsCore.Statistics.ProfitDaysParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitDaysParameter.md#a0b2c419cc75b256332496236f1c9c423), [ATAS.DataFeedsCore.Statistics.CommissionParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1CommissionParameter.md#a99a0547abcb2e3ba5733c82f17477d61), [ATAS.DataFeedsCore.Statistics.NetPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1NetPnLParameter.md#ab7db675d11dc892f346c5ed3176c2cbd), [ATAS.DataFeedsCore.Statistics.WinRateParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinRateParameter.md#a9e706b8a2e1703309fe85750c9e092a0), [ATAS.DataFeedsCore.Statistics.AvgWinParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgWinParameter.md#ac7962d35ce9e5c324acec3e96d4a6fc2), [ATAS.DataFeedsCore.Statistics.AvgLossInMoneyParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossInMoneyParameter.md#a59e677e0bddd0e344c363e4c38375a24), [ATAS.DataFeedsCore.Statistics.WinLossRatioParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinLossRatioParameter.md#acc0b8dee96bf25881d4e7747dcf79c91), [ATAS.DataFeedsCore.Statistics.TotalProfitParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalProfitParameter.md#af5c9eed1e12ef1d0dc0677a44aa491b1), [ATAS.DataFeedsCore.Statistics.TotalLossParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalLossParameter.md#a4db513bc716c38f8010a9d9c38467f0f), [ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownDateParameter.md#a0a840a631875b2fe33d4e9b1556f52d6), [ATAS.DataFeedsCore.Statistics.DailyPnLParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1DailyPnLParameter.md#abe9c7cfe2cc4eb4bc8fa29420b7ce299), [ATAS.DataFeedsCore.Statistics.BestTradeParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1BestTradeParameter.md#aae93c24815557c9c9f0e54a434138d50), [ATAS.DataFeedsCore.Statistics.WorstTradeParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1WorstTradeParameter.md#a822e2e2fed707ed39bd924e73255fa37), [ATAS.DataFeedsCore.Statistics.WinningDaysPercentParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinningDaysPercentParameter.md#a4ad1b0d0bf86e72ccbd7163cd8262636), [ATAS.DataFeedsCore.Statistics.SharpeRatioParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1SharpeRatioParameter.md#a5f9bf1a8d0f4c284db6958546e4637d3), [ATAS.DataFeedsCore.Statistics.AvgTradesPerDayParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradesPerDayParameter.md#a97c76c1d38ab9d50ac203718080bde48), [ATAS.DataFeedsCore.Statistics.AvgTradeLengthParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradeLengthParameter.md#a007aa695888e1153d2a7e9edcf38b05e), [ATAS.DataFeedsCore.Statistics.MaxConsecutiveWinsParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveWinsParameter.md#aaca3855fa32290aa7f5853c0a79a6aa5), [ATAS.DataFeedsCore.Statistics.MaxConsecutiveLossesParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveLossesParameter.md#a604c600fa869eec1655e152f404a07e7), [ATAS.DataFeedsCore.Statistics.AccountAgeParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1AccountAgeParameter.md#a0b3fb42fe15aced401dcae0b0ceb70f9), and [ATAS.DataFeedsCore.Statistics.LastTradeDateParameter](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1LastTradeDateParameter.md#a43e2e70c5558ab76373246f0a01d3070).

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.Statistics.StatisticsParameter.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)Process()

| void ATAS.DataFeedsCore.Statistics.StatisticsParameter.Process | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#a5cfbafd4b6ef153df497a6ec0bb7aaf0).

## Property Documentation

## [◆](https://docs.atas.net/en/)Name

| string ATAS.DataFeedsCore.Statistics.StatisticsParameter.Name |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#a3366b2554d89cadfa84e43d043914504).

## [◆](https://docs.atas.net/en/)Value

| decimal ATAS.DataFeedsCore.Statistics.StatisticsParameter.Value |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md#ae8cf05abca368a81db081cd3016c8810).

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.Statistics.StatisticsParameter.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [StatisticsParameter.cs](../files/StatisticsParameter_8cs.md)
