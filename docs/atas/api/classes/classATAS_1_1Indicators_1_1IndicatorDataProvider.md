# ATAS.Indicators.IndicatorDataProvider Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1IndicatorDataProvider.html

Implementation of the IIndicatorDataProvider interface that provides access to various data and services related to an indicator.
 [More...](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#details)

Inheritance diagram for ATAS.Indicators.IndicatorDataProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IndicatorDataProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | | |
| --- | --- | --- |
| | [IndicatorDataProvider](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#abcda880af8d66f92fee42964a21ec06c) ([ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) tradingStatisticsProvider, [IIndicatorServiceProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorServiceProvider.md) indicatorServiceProvider, [IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) onlineDataProvider, [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) platformSettings, [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) instrumentInfo, [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) tradingManager, [ICandleCreator](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md) candleCreator, [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) chartInfo) | |
| | Initializes a new instance of the IndicatorDataProvider class. | |
| | | |
| T | [GetService](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ae03d9d5ab2d12a6e76ec83e22ed7af4a) () | |
| | Resolves registered services. | |
| | | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [GetCustomStartTime](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#aeedb31435bff70c7fca8d6bd784c29fc) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) time, [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) timeFrame) | |
| | Gets custom candle begin time with specified timeframe and current time.Parameters

 time | |
| timeFrame | | |

Returns`true` if a new trading session has started; otherwise, `false`.

bool [IsNewSession](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a26547fdcd3e385111831639d6d41ce29) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime)
 Checks whether a new trading session has started between the specified previous time and new time.Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading session has started; otherwise, `false`.

bool [IsNewWeek](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ad159ab7aeea0f35d65ba7ae61fa67098) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime)
 Checks whether a new trading week has started between the specified previous time and new time.Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading week has started; otherwise, `false`.

bool [IsNewMonth](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ae9ddf3573b18659db6095d3e184a516c) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime)
 Checks whether a new trading month has started between the specified previous time and new time.Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading month has started; otherwise, `false`.

void [AddAlert](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a4e4f1d1cf5788977f3b7e0c21199138a) (string soundFile, string instrument, string message, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) background, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) foreground, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? time=null)
 Adds an alert with the specified details to the indicator.

void [DoActionInGuiThread](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a9bc6f2f9a1506fa0079968756fbf9b24) (Action action)
 Executes the specified action on the GUI thread.

override string [ToString](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ac1e6460d70b8bd3fcefc232e0023788f) ()
 Returns the name of the indicator data provider.

T [GetService](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aeca15defed50456fd86c2a1d8cfe86bc) ()
 Resolves registered services.

[DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) [GetCustomStartTime](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a469f420b1998ac7368481df25eac97f6) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) time, [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) timeFrame)
 Gets custom candle begin time with specified timeframe and current time.

bool [IsNewSession](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#adf0090e269b335d9082defbfe963ef13) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime)
 Checks whether a new trading session has started between the specified previous time and new time.

bool [IsNewWeek](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aa63d29061be394a137cfcf7911bc0137) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime)
 Checks whether a new trading week has started between the specified previous time and new time.

bool [IsNewMonth](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a31e1d8995b6d00313689835fc3a18631) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime)
 Checks whether a new trading month has started between the specified previous time and new time.

void [AddAlert](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a560f1b933ab985f8f177d9ab28e940dd) (string soundFile, string instrument, string message, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) background, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) foreground, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? time=null)
 Adds an alert with the specified details to the indicator.

void [DoActionInGuiThread](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a66e0fca1144fcd61b518a5b102e35bd9) (Action action)
 Executes the specified action on the GUI thread.

| Public Attributes | |
| --- | --- |
| Action? | [OnNewGuiActionRequested](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ad3f65e2fbc3b1db79fea635211032c70) |
| | Gets or sets the action to request a new GUI action. |
| | |

| Static Public Attributes | |
| --- | --- |
| const string | [NewPanel](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a043db27227677cf8ae953b5a874ddbf8) = "NewPanel" |
| | Represents the name of a new panel. |
| | |
| const string | [CandlesPanel](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a25ac59dd757bcaa818b4e5f8193d6bf4) = "Chart" |
| | Represents the name of the candles panel on the chart. |
| | |

| Properties | |
| --- | --- |
| string | [Name](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a03e1ae4a36efa91ca11a086d4ec72ac6)`[get]` |
| | Gets or sets the name of the indicator data provider. |
| | |
| [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) | [ChartInfo](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ad15db01224ee7fd65ac9e48c360f64b8)`[get]` |
| | Gets the chart information associated with the indicator. |
| | |
| [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) | [GlobalPlatformSettings](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ae3fb35301f0a2ec202c30424e17c53dc)`[get]` |
| | Gets or sets the global platform settings used by the indicator. |
| | |
| [IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | [OnlineDataProvider](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ab4dbd6acb207b696a85cda757b0efbcd)`[get]` |
| | Gets or sets the online data provider used by the indicator to fetch real-time data. |
| | |
| ObservableCollection | [CandlesDataSeries](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a4d7916207a6b0126c6e72f62094b465c)`[get]` |
| | Gets or sets the collection of candle part series used by the indicator. |
| | |
| ObservableCollection | [Panels](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a6b84f9dd2fa854b5166f2ae3d3677d15)`[get]` |
| | Gets or sets the collection of panels associated with the indicator. |
| | |
| [MarketDepthInfoProvider](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) | [MarketDepthInfoProvider](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#ac55df4a0c3b83ab6e84c6631b914e230)`[get]` |
| | Gets or sets the market depth information provider used by the indicator to access market depth data. |
| | |
| [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) | [InstrumentInfo](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a1f4a141007f68819f59e3d8ea23adc3e)`[get, set]` |
| | Gets or sets the instrument information associated with the indicator's instrument. |
| | |
| [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) | [TradingManager](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a67d84ee988cf86d93ee46f1df870eb51)`[get]` |
| | Gets the trading manager used by the indicator to manage trading-related tasks. |
| | |
| [ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) | [TradingStatisticsProvider](./classATAS_1_1Indicators_1_1IndicatorDataProvider.md#a970e4a05f7b1307c284d7e4af478d820)`[get]` |
| | Gets the trading statistics provider used by the indicator to access trading-related statistics. |
| | |
| - Properties inherited from [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md) | |
| string | [Name](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#ac8ccd66cb1785e1e0d585db51dea1d6e)`[get]` |
| | Gets the name of the indicator data provider. |
| | |
| [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) | [ChartInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a5a127e8a643aea36c7329aa131ecc5a9)`[get]` |
| | Gets the chart information associated with the indicator. |
| | |
| [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) | [GlobalPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#ad334dafef30666da32ca6f7f53380680)`[get]` |
| | Gets the global platform settings used by the indicator. |
| | |
| [IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | [OnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a61a6fd18dca8477879a8636c5bb51f69)`[get]` |
| | Gets the online data provider used by the indicator to fetch real-time data. |
| | |
| ObservableCollection | [CandlesDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aef256a05ab0077309568c1241fa4349c)`[get]` |
| | Gets the collection of candle part series used by the indicator. |
| | |
| ObservableCollection | [Panels](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a1476b4186ebddfc678dead0b512abf75)`[get]` |
| | Gets the collection of panels associated with the indicator. |
| | |
| [MarketDepthInfoProvider](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) | [MarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a59ab521388961905be353c70af216288)`[get]` |
| | Gets the market depth information provider used by the indicator to access market depth data. |
| | |
| [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) | [InstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a7a2f31983b7f35fe24d2fc363cef1f4d)`[get]` |
| | Gets the instrument information associated with the indicator's instrument. |
| | |
| [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) | [TradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#abb6b990acfab2e4e388745f6fb0da26e)`[get]` |
| | Gets the trading manager used by the indicator to manage trading-related tasks. |
| | |
| [ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) | [TradingStatisticsProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a21d0eaa16c92be103596c0af4ef7aa6a)`[get]` |
| | Gets the trading statistics provider used by the indicator to access trading-related statistics. |
| | |

## Detailed Description

Implementation of the IIndicatorDataProvider interface that provides access to various data and services related to an indicator.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)IndicatorDataProvider()

| ATAS.Indicators.IndicatorDataProvider.IndicatorDataProvider | ( | [ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) | tradingStatisticsProvider, |
| --- | --- | --- | --- |
| | | [IIndicatorServiceProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorServiceProvider.md) | indicatorServiceProvider, |
| | | [IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | onlineDataProvider, |
| | | [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) | platformSettings, |
| | | [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) | instrumentInfo, |
| | | [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) | tradingManager, |
| | | [ICandleCreator](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md) | candleCreator, |
| | | [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) | chartInfo |
| | ) | | |

Initializes a new instance of the IndicatorDataProvider class.

Parameters

| candleCreator | The candle creator instance associated with the indicator. |
| --- | --- |
| onlineDataProvider | The online data provider instance used by the indicator. |
| platformSettings | The global platform settings used by the indicator. |
| instrumentInfo | The instrument information associated with the indicator's instrument. |
| tradingManager | The trading manager used by the indicator. |
| chartInfo | The chart information associated with the indicator. |
| tradingStatisticsProvider | The trading statistics provider used by the indicator. |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)AddAlert()

| void ATAS.Indicators.IndicatorDataProvider.AddAlert | ( | string | soundFile, |
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

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a560f1b933ab985f8f177d9ab28e940dd).

## [◆](https://docs.atas.net/en/)DoActionInGuiThread()

| void ATAS.Indicators.IndicatorDataProvider.DoActionInGuiThread | ( | Action | action | ) | |
| --- | --- | --- | --- | --- | --- |

Executes the specified action on the GUI thread.

Parameters

| action | The action to execute on the GUI thread. |
| --- | --- |

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a66e0fca1144fcd61b518a5b102e35bd9).

## [◆](https://docs.atas.net/en/)GetCustomStartTime()

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IndicatorDataProvider.GetCustomStartTime | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | time, |
| --- | --- | --- | --- |
| | | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | timeFrame |
| | ) | | |

Gets custom candle begin time with specified timeframe and current time.Parameters

| time | |
| --- | --- |
| timeFrame | |

Returns`true` if a new trading session has started; otherwise, `false`.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a469f420b1998ac7368481df25eac97f6).

## [◆](https://docs.atas.net/en/)GetService()

| T ATAS.Indicators.IndicatorDataProvider.GetService | ( | | ) | |
| --- | --- | --- | --- | --- |

Resolves registered services.

Template Parameters

| T | ype of the service |
| --- | --- |

ReturnsInstance of the service

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aeca15defed50456fd86c2a1d8cfe86bc).

## [◆](https://docs.atas.net/en/)IsNewMonth()

| bool ATAS.Indicators.IndicatorDataProvider.IsNewMonth | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks whether a new trading month has started between the specified previous time and new time.Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading month has started; otherwise, `false`.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a31e1d8995b6d00313689835fc3a18631).

## [◆](https://docs.atas.net/en/)IsNewSession()

| bool ATAS.Indicators.IndicatorDataProvider.IsNewSession | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks whether a new trading session has started between the specified previous time and new time.Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading session has started; otherwise, `false`.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#adf0090e269b335d9082defbfe963ef13).

## [◆](https://docs.atas.net/en/)IsNewWeek()

| bool ATAS.Indicators.IndicatorDataProvider.IsNewWeek | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks whether a new trading week has started between the specified previous time and new time.Parameters

| prevTime | The previous time. |
| --- | --- |
| newTime | The new time. |

Returns`true` if a new trading week has started; otherwise, `false`.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aa63d29061be394a137cfcf7911bc0137).

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.Indicators.IndicatorDataProvider.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns the name of the indicator data provider.

ReturnsThe name of the indicator data provider.

## Member Data Documentation

## [◆](https://docs.atas.net/en/)CandlesPanel

| const string ATAS.Indicators.IndicatorDataProvider.CandlesPanel = "Chart" |
| --- |

static

Represents the name of the candles panel on the chart.

## [◆](https://docs.atas.net/en/)NewPanel

| const string ATAS.Indicators.IndicatorDataProvider.NewPanel = "NewPanel" |
| --- |

static

Represents the name of a new panel.

## [◆](https://docs.atas.net/en/)OnNewGuiActionRequested

| Action? ATAS.Indicators.IndicatorDataProvider.OnNewGuiActionRequested |
| --- |

Gets or sets the action to request a new GUI action.

## Property Documentation

## [◆](https://docs.atas.net/en/)CandlesDataSeries

| ObservableCollection ATAS.Indicators.IndicatorDataProvider.CandlesDataSeries |
| --- |

get

Gets or sets the collection of candle part series used by the indicator.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#aef256a05ab0077309568c1241fa4349c).

## [◆](https://docs.atas.net/en/)ChartInfo

| [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) ATAS.Indicators.IndicatorDataProvider.ChartInfo |
| --- |

get

Gets the chart information associated with the indicator.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a5a127e8a643aea36c7329aa131ecc5a9).

## [◆](https://docs.atas.net/en/)GlobalPlatformSettings

| [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) ATAS.Indicators.IndicatorDataProvider.GlobalPlatformSettings |
| --- |

get

Gets or sets the global platform settings used by the indicator.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#ad334dafef30666da32ca6f7f53380680).

## [◆](https://docs.atas.net/en/)InstrumentInfo

| [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) ATAS.Indicators.IndicatorDataProvider.InstrumentInfo |
| --- |

getset

Gets or sets the instrument information associated with the indicator's instrument.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a7a2f31983b7f35fe24d2fc363cef1f4d).

## [◆](https://docs.atas.net/en/)MarketDepthInfoProvider

| [MarketDepthInfoProvider](./classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) ATAS.Indicators.IndicatorDataProvider.MarketDepthInfoProvider |
| --- |

get

Gets or sets the market depth information provider used by the indicator to access market depth data.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a59ab521388961905be353c70af216288).

## [◆](https://docs.atas.net/en/)Name

| string ATAS.Indicators.IndicatorDataProvider.Name |
| --- |

get

Gets or sets the name of the indicator data provider.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#ac8ccd66cb1785e1e0d585db51dea1d6e).

## [◆](https://docs.atas.net/en/)OnlineDataProvider

| [IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) ATAS.Indicators.IndicatorDataProvider.OnlineDataProvider |
| --- |

get

Gets or sets the online data provider used by the indicator to fetch real-time data.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a61a6fd18dca8477879a8636c5bb51f69).

## [◆](https://docs.atas.net/en/)Panels

| ObservableCollection ATAS.Indicators.IndicatorDataProvider.Panels |
| --- |

get

Gets or sets the collection of panels associated with the indicator.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a1476b4186ebddfc678dead0b512abf75).

## [◆](https://docs.atas.net/en/)TradingManager

| [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) ATAS.Indicators.IndicatorDataProvider.TradingManager |
| --- |

get

Gets the trading manager used by the indicator to manage trading-related tasks.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#abb6b990acfab2e4e388745f6fb0da26e).

## [◆](https://docs.atas.net/en/)TradingStatisticsProvider

| [ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) ATAS.Indicators.IndicatorDataProvider.TradingStatisticsProvider |
| --- |

get

Gets the trading statistics provider used by the indicator to access trading-related statistics.

Implements [ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#a21d0eaa16c92be103596c0af4ef7aa6a).

The documentation for this class was generated from the following file:
- [IndicatorDataProvider.cs](../files/IndicatorDataProvider_8cs.md)
