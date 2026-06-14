# ATAS.Indicators.IIndicatorDataProvider Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.html

Represents a data provider for an indicator, providing access to various data and services related to the indicator.
 [More...](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#details)

Inheritance diagram for ATAS.Indicators.IIndicatorDataProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| T | [GetService](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aeca15defed50456fd86c2a1d8cfe86bc) () |
| | Resolves registered services. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [GetCustomStartTime](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a469f420b1998ac7368481df25eac97f6) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) time, [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) timeFrame) |
| | Gets custom candle begin time with specified timeframe and current time. |
| | |
| bool | [IsNewSession](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#adf0090e269b335d9082defbfe963ef13) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime) |
| | Checks whether a new trading session has started between the specified previous time and new time. |
| | |
| bool | [IsNewWeek](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aa63d29061be394a137cfcf7911bc0137) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime) |
| | Checks whether a new trading week has started between the specified previous time and new time. |
| | |
| bool | [IsNewMonth](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a31e1d8995b6d00313689835fc3a18631) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime) |
| | Checks whether a new trading month has started between the specified previous time and new time. |
| | |
| void | [AddAlert](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a560f1b933ab985f8f177d9ab28e940dd) (string soundFile, string instrument, string message, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) background, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) foreground, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? time=null) |
| | Adds an alert with the specified details to the indicator. |
| | |
| void | [DoActionInGuiThread](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a66e0fca1144fcd61b518a5b102e35bd9) (Action action) |
| | Executes the specified action on the GUI thread. |
| | |

| Properties | |
| --- | --- |
| string | [Name](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#ac8ccd66cb1785e1e0d585db51dea1d6e)`[get]` |
| | Gets the name of the indicator data provider. |
| | |
| [IChart](./interfaceATAS_1_1Indicators_1_1IChart.md) | [ChartInfo](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a5a127e8a643aea36c7329aa131ecc5a9)`[get]` |
| | Gets the chart information associated with the indicator. |
| | |
| [IPlatformSettings](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) | [GlobalPlatformSettings](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#ad334dafef30666da32ca6f7f53380680)`[get]` |
| | Gets the global platform settings used by the indicator. |
| | |
| [IOnlineDataProvider](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | [OnlineDataProvider](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a61a6fd18dca8477879a8636c5bb51f69)`[get]` |
| | Gets the online data provider used by the indicator to fetch real-time data. |
| | |
| ObservableCollection | [CandlesDataSeries](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aef256a05ab0077309568c1241fa4349c)`[get]` |
| | Gets the collection of candle part series used by the indicator. |
| | |
| ObservableCollection | [Panels](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a1476b4186ebddfc678dead0b512abf75)`[get]` |
| | Gets the collection of panels associated with the indicator. |
| | |
| [MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) | [MarketDepthInfoProvider](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a59ab521388961905be353c70af216288)`[get]` |
| | Gets the market depth information provider used by the indicator to access market depth data. |
| | |
| [IInstrumentInfo](./interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) | [InstrumentInfo](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a7a2f31983b7f35fe24d2fc363cef1f4d)`[get]` |
| | Gets the instrument information associated with the indicator's instrument. |
| | |
| [ITradingManager](./interfaceATAS_1_1Indicators_1_1ITradingManager.md) | [TradingManager](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#abb6b990acfab2e4e388745f6fb0da26e)`[get]` |
| | Gets the trading manager used by the indicator to manage trading-related tasks. |
| | |
| [ITradingStatisticsProvider](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) | [TradingStatisticsProvider](./interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a21d0eaa16c92be103596c0af4ef7aa6a)`[get]` |
| | Gets the trading statistics provider used by the indicator to access trading-related statistics. |
| | |

## Detailed Description

Represents a data provider for an indicator, providing access to various data and services related to the indicator.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)AddAlert()

| void ATAS.Indicators.IIndicatorDataProvider.AddAlert | ( | string | soundFile, |
| --- | --- | --- | --- |
| | | string | instrument, |
| | | string | message, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | background, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | foreground, |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? | time = `null` |
| | ) | | |

Adds an alert with the specified details to the indicator.

Parameters

| soundFile | The sound file to play for the alert. |
| --- | --- |
| instrument | The instrument associated with the alert. |
| message | The alert message. |
| background | The background color of the alert. |
| foreground | The foreground color of the alert. |
| time | Exact time for alert |

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a4e4f1d1cf5788977f3b7e0c21199138a).

## [◆](https://docs.atas.net/en/)DoActionInGuiThread()

| void ATAS.Indicators.IIndicatorDataProvider.DoActionInGuiThread | ( | Action | action | ) | |
| --- | --- | --- | --- | --- | --- |

Executes the specified action on the GUI thread.

Parameters

| action | The action to execute on the GUI thread. |
| --- | --- |

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a9bc6f2f9a1506fa0079968756fbf9b24).

## [◆](https://docs.atas.net/en/)GetCustomStartTime()

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IIndicatorDataProvider.GetCustomStartTime | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | time, |
| --- | --- | --- | --- |
| | | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | timeFrame |
| | ) | | |

Gets custom candle begin time with specified timeframe and current time.

Parameters

| time | |
| --- | --- |
| timeFrame | |

Returns`true` if a new trading session has started; otherwise, `false`.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#aeedb31435bff70c7fca8d6bd784c29fc).

## [◆](https://docs.atas.net/en/)GetService()

| T ATAS.Indicators.IIndicatorDataProvider.GetService | ( | | ) | |
| --- | --- | --- | --- | --- |

Resolves registered services.

Template Parameters

| T | ype of the service |
| --- | --- |

ReturnsInstance of the service

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ae03d9d5ab2d12a6e76ec83e22ed7af4a).

## [◆](https://docs.atas.net/en/)IsNewMonth()

| bool ATAS.Indicators.IIndicatorDataProvider.IsNewMonth | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks whether a new trading month has started between the specified previous time and new time.

Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading month has started; otherwise, `false`.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ae9ddf3573b18659db6095d3e184a516c).

## [◆](https://docs.atas.net/en/)IsNewSession()

| bool ATAS.Indicators.IIndicatorDataProvider.IsNewSession | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks whether a new trading session has started between the specified previous time and new time.

Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading session has started; otherwise, `false`.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a26547fdcd3e385111831639d6d41ce29).

## [◆](https://docs.atas.net/en/)IsNewWeek()

| bool ATAS.Indicators.IIndicatorDataProvider.IsNewWeek | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks whether a new trading week has started between the specified previous time and new time.

Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading week has started; otherwise, `false`.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ad159ab7aeea0f35d65ba7ae61fa67098).

## Property Documentation

## [◆](https://docs.atas.net/en/)CandlesDataSeries

| ObservableCollection ATAS.Indicators.IIndicatorDataProvider.CandlesDataSeries |
| --- |

get

Gets the collection of candle part series used by the indicator.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a4d7916207a6b0126c6e72f62094b465c).

## [◆](https://docs.atas.net/en/)ChartInfo

| [IChart](./interfaceATAS_1_1Indicators_1_1IChart.md) ATAS.Indicators.IIndicatorDataProvider.ChartInfo |
| --- |

get

Gets the chart information associated with the indicator.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ad15db01224ee7fd65ac9e48c360f64b8).

## [◆](https://docs.atas.net/en/)GlobalPlatformSettings

| [IPlatformSettings](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) ATAS.Indicators.IIndicatorDataProvider.GlobalPlatformSettings |
| --- |

get

Gets the global platform settings used by the indicator.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ae3fb35301f0a2ec202c30424e17c53dc).

## [◆](https://docs.atas.net/en/)InstrumentInfo

| [IInstrumentInfo](./interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) ATAS.Indicators.IIndicatorDataProvider.InstrumentInfo |
| --- |

get

Gets the instrument information associated with the indicator's instrument.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a1f4a141007f68819f59e3d8ea23adc3e).

## [◆](https://docs.atas.net/en/)MarketDepthInfoProvider

| [MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) ATAS.Indicators.IIndicatorDataProvider.MarketDepthInfoProvider |
| --- |

get

Gets the market depth information provider used by the indicator to access market depth data.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ac55df4a0c3b83ab6e84c6631b914e230).

## [◆](https://docs.atas.net/en/)Name

| string ATAS.Indicators.IIndicatorDataProvider.Name |
| --- |

get

Gets the name of the indicator data provider.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a03e1ae4a36efa91ca11a086d4ec72ac6).

## [◆](https://docs.atas.net/en/)OnlineDataProvider

| [IOnlineDataProvider](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) ATAS.Indicators.IIndicatorDataProvider.OnlineDataProvider |
| --- |

get

Gets the online data provider used by the indicator to fetch real-time data.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ab4dbd6acb207b696a85cda757b0efbcd).

## [◆](https://docs.atas.net/en/)Panels

| ObservableCollection ATAS.Indicators.IIndicatorDataProvider.Panels |
| --- |

get

Gets the collection of panels associated with the indicator.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a6b84f9dd2fa854b5166f2ae3d3677d15).

## [◆](https://docs.atas.net/en/)TradingManager

| [ITradingManager](./interfaceATAS_1_1Indicators_1_1ITradingManager.md) ATAS.Indicators.IIndicatorDataProvider.TradingManager |
| --- |

get

Gets the trading manager used by the indicator to manage trading-related tasks.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a67d84ee988cf86d93ee46f1df870eb51).

## [◆](https://docs.atas.net/en/)TradingStatisticsProvider

| [ITradingStatisticsProvider](./interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) ATAS.Indicators.IIndicatorDataProvider.TradingStatisticsProvider |
| --- |

get

Gets the trading statistics provider used by the indicator to access trading-related statistics.

Implemented in [ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a970e4a05f7b1307c284d7e4af478d820).

The documentation for this interface was generated from the following file:
- [IndicatorDataProvider.cs](../files/IndicatorDataProvider_8cs.md)
