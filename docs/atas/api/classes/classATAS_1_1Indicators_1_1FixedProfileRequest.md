# ATAS.Indicators.FixedProfileRequest Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1FixedProfileRequest.html

Represents a request for a fixed profile with a specific period.
 [More...](./classATAS_1_1Indicators_1_1FixedProfileRequest.md#details)

| Public Member Functions | |
| --- | --- |
| | [FixedProfileRequest](./classATAS_1_1Indicators_1_1FixedProfileRequest.md#a086035a8a2fb5bd19508ec467f413239) ([FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) period) |
| | Initializes a new instance of the FixedProfileRequest class with the specified period. |
| | |
| | [FixedProfileRequest](./classATAS_1_1Indicators_1_1FixedProfileRequest.md#ae782fb200692a5c8a6c025534846936d) ([FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) period, long? tradingSession) |
| | Initializes a new instance of the FixedProfileRequest class with the specified period. |
| | |
| | [FixedProfileRequest](./classATAS_1_1Indicators_1_1FixedProfileRequest.md#ab49aa9f8b92b4ad82e52f4ae7986353c) ([FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) period, long? tradingSession, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? baseTime) |
| | Initializes a new instance of the FixedProfileRequest class with the specified period and base time. |
| | |

| Properties | |
| --- | --- |
| [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) | [Period](./classATAS_1_1Indicators_1_1FixedProfileRequest.md#a4cf798b6a5da2c39b986c5d1f6abd17f)`[get]` |
| | Gets the fixed profile period associated with this request. |
| | |
| long? | [TradingSession](./classATAS_1_1Indicators_1_1FixedProfileRequest.md#acf180781407bc6e5703525ca6f5142ad)`[get]` |
| | Gets the fixed profile trading session identifier (ETH/RTH/etc.) associated with this request. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? | [BaseTime](./classATAS_1_1Indicators_1_1FixedProfileRequest.md#a70c234a4c570cb144f4247eac502c233)`[get]` |
| | Gets the base time for period calculation. If null, the current market time is used. This is useful for Market Replay scenarios where the period should be calculated relative to a specific point in time rather than the current market time. |
| | |

## Detailed Description

Represents a request for a fixed profile with a specific period.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)FixedProfileRequest() [1/3]

| ATAS.Indicators.FixedProfileRequest.FixedProfileRequest | ( | [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) | period | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the FixedProfileRequest class with the specified period.

Parameters

| period | The fixed profile period to be used in the request. |
| --- | --- |

## [◆](https://docs.atas.net/en/)FixedProfileRequest() [2/3]

| ATAS.Indicators.FixedProfileRequest.FixedProfileRequest | ( | [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) | period, |
| --- | --- | --- | --- |
| | | long? | tradingSession |
| | ) | | |

Initializes a new instance of the FixedProfileRequest class with the specified period.

Parameters

| period | The fixed profile period to be used in the request. |
| --- | --- |
| tradingSession | The fixed profile session type to be used in the request. |

## [◆](https://docs.atas.net/en/)FixedProfileRequest() [3/3]

| ATAS.Indicators.FixedProfileRequest.FixedProfileRequest | ( | [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) | period, |
| --- | --- | --- | --- |
| | | long? | tradingSession, |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? | baseTime |
| | ) | | |

Initializes a new instance of the FixedProfileRequest class with the specified period and base time.

Parameters

| period | The fixed profile period to be used in the request. |
| --- | --- |
| tradingSession | The fixed profile session type to be used in the request. |
| baseTime | The base time for period calculation. If null, the current market time is used. |

## Property Documentation

## [◆](https://docs.atas.net/en/)BaseTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)? ATAS.Indicators.FixedProfileRequest.BaseTime |
| --- |

get

Gets the base time for period calculation. If null, the current market time is used. This is useful for Market Replay scenarios where the period should be calculated relative to a specific point in time rather than the current market time.

## [◆](https://docs.atas.net/en/)Period

| [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) ATAS.Indicators.FixedProfileRequest.Period |
| --- |

get

Gets the fixed profile period associated with this request.

## [◆](https://docs.atas.net/en/)TradingSession

| long? ATAS.Indicators.FixedProfileRequest.TradingSession |
| --- |

get

Gets the fixed profile trading session identifier (ETH/RTH/etc.) associated with this request.

The documentation for this class was generated from the following file:
- [PriceVolumeInfo.cs](../files/PriceVolumeInfo_8cs.md)
