# ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup< T > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.html

Inheritance diagram for ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup< T >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup< T >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#a30c799961e649c3dc4742114c178d40c) () |
| | |
| void | [Process](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#a665cd894693ea9f39d9f2152fbeedc9f) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#ac4d095b767bcb6be6e661a8fc44b62e8) () |
| | |
| void | [Process](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#ac6e248a5f51aeb1f2f4aa1f3354ab168) ([HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade) |
| | |
| void | [Clear](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#afc8e20e62c4356d557139ff10470e58c) () |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#a0dcfebfc4c3df3eb03a5052cd720fc1d) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| string | [Name](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#af1317f83a5fef9a4b53ce9c6c73320fa)`[get]` |
| | |
| Type | [Type](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#ae1db88f5cb5bd46dec88b04fe6da4daf)`[get]` |
| | |
| T | [Total](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#aa6851180ac0e8fc234f1adb5b277e98b)`[get]` |
| | |
| T | [Long](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#a7cdd0b63d9ff445a1a0cc9874f8b384a)`[get]` |
| | |
| T | [Short](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#a9f1f59bb0811a65a596667fd350c3ea4)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md) | |
| string | [Name](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#ae7e7db35041b4d70e8d74bedb7f5864e)`[get]` |
| | |
| Type | [Type](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#a584adafd0bce8f60594e4b5e287811d6)`[get]` |
| | |
| [IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md) | [Total](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#ad4b97831afbc3ec3b7f46a7e89273d29)`[get]` |
| | |
| [IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md) | [Long](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#a5ba5fe1d6c8fbac27c7bc2664c169a56)`[get]` |
| | |
| [IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md) | [Short](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#a32fcb64359e45e9b840d72738647f6e0)`[get]` |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md#ab57956894a13179b64818e26def94838) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)StatisticsParameterGroup()

| [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).[StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#afc8e20e62c4356d557139ff10470e58c).

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)Process()

| void [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).Process | ( | [HistoryMyTrade](./classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#ac6e248a5f51aeb1f2f4aa1f3354ab168).

## Property Documentation

## [◆](https://docs.atas.net/en/)Long

| T [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).Long |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#a5ba5fe1d6c8fbac27c7bc2664c169a56).

## [◆](https://docs.atas.net/en/)Name

| string [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).Name |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#ae7e7db35041b4d70e8d74bedb7f5864e).

## [◆](https://docs.atas.net/en/)Short

| T [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).Short |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#a32fcb64359e45e9b840d72738647f6e0).

## [◆](https://docs.atas.net/en/)Total

| T [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).Total |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#ad4b97831afbc3ec3b7f46a7e89273d29).

## [◆](https://docs.atas.net/en/)Type

| Type [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).Type |
| --- |

get

Implements [ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md#a584adafd0bce8f60594e4b5e287811d6).

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler? [ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](./classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md).PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [StatisticsParameterGroup.cs](../files/StatisticsParameterGroup_8cs.md)
