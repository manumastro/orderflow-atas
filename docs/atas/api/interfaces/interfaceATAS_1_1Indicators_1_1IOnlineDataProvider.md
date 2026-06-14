# ATAS.Indicators.IOnlineDataProvider Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.html

Interface for an online data provider that provides access to real-time market data.
 [More...](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#details)

Inheritance diagram for ATAS.Indicators.IOnlineDataProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IOnlineDataProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Subscribe](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#ad6bc36c3a04ccf65c7fba9eb3b67f0d2) () |
| | Subscribes to the data provider to start receiving real-time data updates. |
| | |
| void | [Unsubscribe](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#aaf335fa8d6928bc6c15d23fdbba8c3bc) () |
| | Unsubscribes from the data provider to stop receiving real-time data updates. |
| | |
| void | [RedrawChart](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a056caee17642a325e61a8591550f1dd1) ([RedrawArg](../classes/classATAS_1_1Indicators_1_1RedrawArg.md) redrawArg) |
| | Redraws the chart based on the provided redraw argument. |
| | |
| void | [RequestCumulativeTrades](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a55c33bd55f76fc5b65bd229c7eaba271) ([CumulativeTradesRequest](../classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) request) |
| | Requests historical cumulative trade data based on the provided request. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [GetCustomStartTime](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a0ce8894ff3e0a64e8252a719650cdae3) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) time, [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) timeFrame) |
| | Gets custom candle begin time with specified timeframe and current time. |
| | |
| bool | [IsNewSession](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#ae74b6eeb3d542462110aa0c50de43fa3) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime) |
| | Checks if the provided new time indicates a new trading session compared to the previous time. |
| | |
| bool | [IsNewWeek](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#af1a5087015739d53fe2893deaf74d681) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime) |
| | Checks if the provided new time indicates a new trading week compared to the previous time. |
| | |
| bool | [IsNewMonth](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a551384d558af365bccbbaf6e55b6c9f6) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) prevTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) newTime) |
| | Checks if the provided new time indicates a new trading month compared to the previous time. |
| | |
| void | [AddAlert](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#ac423580c9e3a218f98fa7751456a2648) (string soundFile, string instrument, string message, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) background, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) foreground) |
| | Adds an alert with the specified parameters. |
| | |
| void | [AddAlert](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a2b12395f3baf7f62268f8141aeeb6e7d) (string soundFile, string instrument, string message, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) background, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) foreground, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) time) |
| | Adds an alert with the specified details to the indicator. |
| | |
| IEnumerable | [GetMarketDepthSnapshot](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a6ce439329fe9dbb318c085589d051f59) () |
| | Gets a snapshot of the current market depth data. |
| | |
| [ITradesCache](./interfaceATAS_1_1Indicators_1_1ITradesCache.md) | [GetTradesCache](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a6126683f98036b60d3c380331e5ff5aa) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period) |
| | Returns a trades cache. |
| | |
| [IMarketByOrdersCache](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) | [GetMarketByOrdersCache](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#acf186529e3d00dea6d5d1ae534e6566a) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period) |
| | Returns a market by order data cache. |
| | |
| [IMarketByOrdersWithTradesCache](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) | [GetMarketByOrdersWithTradesCache](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a7dd1020a1694720e3e6009c1a08fd9cc) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period) |
| | Returns a market by order data and trades cache. |
| | |
| Task | [SubscribeMarketByOrdersData](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#adab9212d7dd76d7a8027d94984ea8393) () |
| | Subscribes to the market by order data. |
| | |
| Task > | [GetMarketDepthSnapshotsAsync](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a85db881f7f5d92999958e34b0ff2b46a) ([MarketDepthSnapshotRequest](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md) request, CancellationToken cancellation=default) |
| | Asynchronously retrieves market depth snapshots from the server. |
| | |
| Task | [GetContractRolloversAsync](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#ad2c6cedf3427a2e7d94ca96d2d578863) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) from, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) to, [ContractRolloverType](../namespaces/namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064) type, CancellationToken cancellation=default) |
| | Retrieves historical contract rollover data for a continuous instrument. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.IKnowFixedProfiles](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md) | |
| void | [GetFixedProfile](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#aa2d5c98e8fcab98e57e75fd2fed3d0af) ([FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) request) |
| | Requests a fixed profile based on the specified FixedProfileRequest. |
| | |
| Task | [RequestFixedProfileAsync](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#a1e03876fd8174b99a1218a274cc3b7c5) ([FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) request) |
| | Asynchronously requests a fixed market profile parameterized with FixedProfileRequest. |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.IMarketTimeProvider](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md) | |
| void | [SubscribeToTimer](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a7a187460ab54c6669677fdb8a2d3153d) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback) |
| | Registers the callback and periodically calls it with specified period. |
| | |
| void | [UnsubscribeFromTimer](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a486f0f6bddd244284110a94baeecd595) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback) |
| | Unregister the callback from periodic invocations. |
| | |

| Properties | |
| --- | --- |
| decimal | [CumulativeDomAsks](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#ac75156b71052d1511951e97c364ca8e2)`[get]` |
| | Gets the cumulative DOM (Depth of Market) asks volume. |
| | |
| decimal | [CumulativeDomBids](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a426ce515e6adc4b7c2a4e23547a661cf)`[get]` |
| | Gets the cumulative DOM (Depth of Market) bids volume. |
| | |
| - Properties inherited from [ATAS.Indicators.IMarketByOrdersDataProvider](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md) | |
| IEnumerable | [MarketByOrders](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md#a38bee1aaeb536bc28e0c15c2940fb777)`[get]` |
| | Gets a snapshot of the current market by order data. |
| | |
| - Properties inherited from [ATAS.Indicators.IMarketTimeProvider](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md) | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [MarketTime](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a719ea8d551a57d0673177c0d01931ffb)`[get]` |
| | Current market time. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [UtcTime](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#afd122a8c561de01a599f88f1477615f9)`[get]` |
| | Current UTC time. |
| | |

| Events | |
| --- | --- |
| Action > | [NewTrades](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#aa89961d1c18dda39663c9d780234506f) |
| | Event that is raised when new trades are received. |
| | |
| Action | [BestBidAskChanged](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#aac96f2c392fae50cca9438747b219086) |
| | Event that is raised when the best bid or ask prices have changed. |
| | |
| Action > | [MarketDepthsChanged](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a560d485d8d955ba0aa93be92f7a202a6) |
| | Event that is raised when market depths have changed. |
| | |
| Action | [NewCumulativeTrade](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a994dc0b5b18610aa4dfa0bbcabe85926) |
| | Event that is raised when a new cumulative trade is received. |
| | |
| Action | [UpdateCumulativeTrade](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a95c082b150ed0cf977a49739c0eb820a) |
| | Event that is raised when an update to a cumulative trade is received. |
| | |
| Action > | [HistoricalCumulativeTrades](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a832cd4e7421bc260ca81529d320f00b3) |
| | Event that is raised when historical cumulative trades data is received. |
| | |
| Action > | [MarketByOrdersChanged](./interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#a7318b57f2bf3d374b35b22db6e80b7e1) |
| | Event that is raised when real-time market by order data have changed. |
| | |
| - Events inherited from [ATAS.Indicators.IKnowFixedProfiles](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md) | |
| Action | [FixedProfileReceived](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#a5384c8f8c90f9c62be76c94b31e2b13a) |
| | [Obsolete] Event that is triggered when a fixed profile is received. |
| | |
| Action | [FixedProfileBothCandlesReceived](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#a97d54cce1a248bd0bbd66f91d889414e) |
| | Event that is triggered when both the main and secondary indicator candles of a fixed profile are received. |
| | |

## Detailed Description

Interface for an online data provider that provides access to real-time market data.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)AddAlert() [1/2]

| void ATAS.Indicators.IOnlineDataProvider.AddAlert | ( | string | soundFile, |
| --- | --- | --- | --- |
| | | string | instrument, |
| | | string | message, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | background, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | foreground |
| | ) | | |

Adds an alert with the specified parameters.

Parameters

| soundFile | The path of the sound file to be played as part of the alert. |
| --- | --- |
| instrument | The name of the instrument associated with the alert. |
| message | The message text of the alert. |
| background | The background color to be used for the alert. |
| foreground | The foreground color to be used for the alert. |

## [◆](https://docs.atas.net/en/)AddAlert() [2/2]

| void ATAS.Indicators.IOnlineDataProvider.AddAlert | ( | string | soundFile, |
| --- | --- | --- | --- |
| | | string | instrument, |
| | | string | message, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | background, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | foreground, |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | time |
| | ) | | |

Adds an alert with the specified details to the indicator.

Parameters

| soundFile | The sound file to play for the alert. |
| --- | --- |
| instrument | The instrument associated with the alert. |
| message | The alert message. |
| background | The background color of the alert. |
| foreground | The foreground color of the alert. |
| time | Time when alert was triggered |

## [◆](https://docs.atas.net/en/)GetContractRolloversAsync()

| Task ATAS.Indicators.IOnlineDataProvider.GetContractRolloversAsync | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | from, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | to, |
| | | [ContractRolloverType](../namespaces/namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064) | type, |
| | | CancellationToken | cancellation = `default` |
| | ) | | |

Retrieves historical contract rollover data for a continuous instrument.

Parameters

| from | Start date (inclusive) for retrieving rollover history. |
| --- | --- |
| to | End date (inclusive) for retrieving rollover history. |
| type | Rollover calculation method to use. See ContractRolloverType for available options. |
| cancellation | Optional cancellation token to interrupt the operation. |

ReturnsA task representing the asynchronous operation. The result contains historical rollover dates and associated contracts for the instrument.

## [◆](https://docs.atas.net/en/)GetCustomStartTime()

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IOnlineDataProvider.GetCustomStartTime | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | time, |
| --- | --- | --- | --- |
| | | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | timeFrame |
| | ) | | |

Gets custom candle begin time with specified timeframe and current time.

## [◆](https://docs.atas.net/en/)GetMarketByOrdersCache()

| [IMarketByOrdersCache](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) ATAS.Indicators.IOnlineDataProvider.GetMarketByOrdersCache | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period | ) | |
| --- | --- | --- | --- | --- | --- |

Returns a market by order data cache.

Parameters

| period | Specifies the time period for which Market-by-Order updates are stored in IMarketByOrdersCache. |
| --- | --- |

ReturnsThe IMarketByOrdersCache object representing the current state and allowing to receive real-time updates.

## [◆](https://docs.atas.net/en/)GetMarketByOrdersWithTradesCache()

| [IMarketByOrdersWithTradesCache](./interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) ATAS.Indicators.IOnlineDataProvider.GetMarketByOrdersWithTradesCache | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period | ) | |
| --- | --- | --- | --- | --- | --- |

Returns a market by order data and trades cache.

Parameters

| period | Specifies the time period for which trades are stored in IMarketByOrdersWithTradesCache. |
| --- | --- |

ReturnsThe IMarketByOrdersWithTradesCache object representing the cache.

## [◆](https://docs.atas.net/en/)GetMarketDepthSnapshot()

| IEnumerable ATAS.Indicators.IOnlineDataProvider.GetMarketDepthSnapshot | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets a snapshot of the current market depth data.

ReturnsAn IEnumerable of MarketDataArg representing the current market depth data.

## [◆](https://docs.atas.net/en/)GetMarketDepthSnapshotsAsync()

| Task > ATAS.Indicators.IOnlineDataProvider.GetMarketDepthSnapshotsAsync | ( | [MarketDepthSnapshotRequest](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md) | request, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellation = `default` |
| | ) | | |

Asynchronously retrieves market depth snapshots from the server.

Parameters

| request | An object that describes parameters of the requested snapshots. |
| --- | --- |
| cancellation | Cancellation token. |

ReturnsSnapshots of market depth.

## [◆](https://docs.atas.net/en/)GetTradesCache()

| [ITradesCache](./interfaceATAS_1_1Indicators_1_1ITradesCache.md) ATAS.Indicators.IOnlineDataProvider.GetTradesCache | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period | ) | |
| --- | --- | --- | --- | --- | --- |

Returns a trades cache.

Parameters

| period | Specifies the time period for which trades are stored in ITradesCache. |
| --- | --- |

ReturnsThe ITradesCache object representing the trades cache.

## [◆](https://docs.atas.net/en/)IsNewMonth()

| bool ATAS.Indicators.IOnlineDataProvider.IsNewMonth | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks if the provided new time indicates a new trading month compared to the previous time.

## [◆](https://docs.atas.net/en/)IsNewSession()

| bool ATAS.Indicators.IOnlineDataProvider.IsNewSession | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks if the provided new time indicates a new trading session compared to the previous time.

## [◆](https://docs.atas.net/en/)IsNewWeek()

| bool ATAS.Indicators.IOnlineDataProvider.IsNewWeek | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | prevTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | newTime |
| | ) | | |

Checks if the provided new time indicates a new trading week compared to the previous time.

## [◆](https://docs.atas.net/en/)RedrawChart()

| void ATAS.Indicators.IOnlineDataProvider.RedrawChart | ( | [RedrawArg](../classes/classATAS_1_1Indicators_1_1RedrawArg.md) | redrawArg | ) | |
| --- | --- | --- | --- | --- | --- |

Redraws the chart based on the provided redraw argument.

Parameters

| redrawArg | The redraw argument specifying the region to redraw. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RequestCumulativeTrades()

| void ATAS.Indicators.IOnlineDataProvider.RequestCumulativeTrades | ( | [CumulativeTradesRequest](../classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) | request | ) | |
| --- | --- | --- | --- | --- | --- |

Requests historical cumulative trade data based on the provided request.

Parameters

| request | The cumulative trades request specifying the data range. |
| --- | --- |

## [◆](https://docs.atas.net/en/)Subscribe()

| void ATAS.Indicators.IOnlineDataProvider.Subscribe | ( | | ) | |
| --- | --- | --- | --- | --- |

Subscribes to the data provider to start receiving real-time data updates.

## [◆](https://docs.atas.net/en/)SubscribeMarketByOrdersData()

| Task ATAS.Indicators.IOnlineDataProvider.SubscribeMarketByOrdersData | ( | | ) | |
| --- | --- | --- | --- | --- |

Subscribes to the market by order data.

## [◆](https://docs.atas.net/en/)Unsubscribe()

| void ATAS.Indicators.IOnlineDataProvider.Unsubscribe | ( | | ) | |
| --- | --- | --- | --- | --- |

Unsubscribes from the data provider to stop receiving real-time data updates.

## Property Documentation

## [◆](https://docs.atas.net/en/)CumulativeDomAsks

| decimal ATAS.Indicators.IOnlineDataProvider.CumulativeDomAsks |
| --- |

get

Gets the cumulative DOM (Depth of Market) asks volume.

## [◆](https://docs.atas.net/en/)CumulativeDomBids

| decimal ATAS.Indicators.IOnlineDataProvider.CumulativeDomBids |
| --- |

get

Gets the cumulative DOM (Depth of Market) bids volume.

## Event Documentation

## [◆](https://docs.atas.net/en/)BestBidAskChanged

| Action ATAS.Indicators.IOnlineDataProvider.BestBidAskChanged |
| --- |

Event that is raised when the best bid or ask prices have changed.

Parameters

| marketDataArg | The MarketDataArg representing the new best bid or ask prices. |
| --- | --- |

## [◆](https://docs.atas.net/en/)HistoricalCumulativeTrades

| Action > ATAS.Indicators.IOnlineDataProvider.HistoricalCumulativeTrades |
| --- |

Event that is raised when historical cumulative trades data is received.

Parameters

| request | The request object for historical cumulative trades. |
| --- | --- |
| trades | The collection of historical cumulative trades data. |

## [◆](https://docs.atas.net/en/)MarketByOrdersChanged

| Action > ATAS.Indicators.IOnlineDataProvider.MarketByOrdersChanged |
| --- |

Event that is raised when real-time market by order data have changed.

Parameters

| values | An IEnumerable of MarketByOrder representing the changed market by order data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)MarketDepthsChanged

| Action > ATAS.Indicators.IOnlineDataProvider.MarketDepthsChanged |
| --- |

Event that is raised when market depths have changed.

Parameters

| marketDataArgs | The collection of MarketDataArg representing the updated market depths. |
| --- | --- |

## [◆](https://docs.atas.net/en/)NewCumulativeTrade

| Action ATAS.Indicators.IOnlineDataProvider.NewCumulativeTrade |
| --- |

Event that is raised when a new cumulative trade is received.

Parameters

| trade | The new cumulative trade object. |
| --- | --- |

## [◆](https://docs.atas.net/en/)NewTrades

| Action > ATAS.Indicators.IOnlineDataProvider.NewTrades |
| --- |

Event that is raised when new trades are received.

Parameters

| trades | An IEnumerable of MarketDataArg representing the new trades. |
| --- | --- |

## [◆](https://docs.atas.net/en/)UpdateCumulativeTrade

| Action ATAS.Indicators.IOnlineDataProvider.UpdateCumulativeTrade |
| --- |

Event that is raised when an update to a cumulative trade is received.

Parameters

| trade | The updated cumulative trade object. |
| --- | --- |

The documentation for this interface was generated from the following file:
- [IOnlineDataProvider.cs](../files/IOnlineDataProvider_8cs.md)
