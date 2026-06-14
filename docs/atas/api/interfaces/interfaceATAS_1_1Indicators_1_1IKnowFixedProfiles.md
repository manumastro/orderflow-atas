# ATAS.Indicators.IKnowFixedProfiles Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.html

Represents an interface for objects that know fixed profiles and can request them.
 [More...](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#details)

Inheritance diagram for ATAS.Indicators.IKnowFixedProfiles:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [GetFixedProfile](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#aa2d5c98e8fcab98e57e75fd2fed3d0af) ([FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) request) |
| | Requests a fixed profile based on the specified FixedProfileRequest. |
| | |
| Task | [RequestFixedProfileAsync](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#a1e03876fd8174b99a1218a274cc3b7c5) ([FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) request) |
| | Asynchronously requests a fixed market profile parameterized with FixedProfileRequest. |
| | |

| Events | |
| --- | --- |
| Action | [FixedProfileReceived](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#a5384c8f8c90f9c62be76c94b31e2b13a) |
| | [Obsolete] Event that is triggered when a fixed profile is received. |
| | |
| Action | [FixedProfileBothCandlesReceived](./interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#a97d54cce1a248bd0bbd66f91d889414e) |
| | Event that is triggered when both the main and secondary indicator candles of a fixed profile are received. |
| | |

## Detailed Description

Represents an interface for objects that know fixed profiles and can request them.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetFixedProfile()

| void ATAS.Indicators.IKnowFixedProfiles.GetFixedProfile | ( | [FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) | request | ) | |
| --- | --- | --- | --- | --- | --- |

Requests a fixed profile based on the specified FixedProfileRequest.

Parameters

| request | The request containing the fixed profile period. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RequestFixedProfileAsync()

| Task ATAS.Indicators.IKnowFixedProfiles.RequestFixedProfileAsync | ( | [FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) | request | ) | |
| --- | --- | --- | --- | --- | --- |

Asynchronously requests a fixed market profile parameterized with FixedProfileRequest.

Parameters

| request | The request containing the fixed profile period. |
| --- | --- |

## Event Documentation

## [◆](https://docs.atas.net/en/)FixedProfileBothCandlesReceived

| Action ATAS.Indicators.IKnowFixedProfiles.FixedProfileBothCandlesReceived |
| --- |

Event that is triggered when both the main and secondary indicator candles of a fixed profile are received.

## [◆](https://docs.atas.net/en/)FixedProfileReceived

| Action ATAS.Indicators.IKnowFixedProfiles.FixedProfileReceived |
| --- |

[Obsolete] Event that is triggered when a fixed profile is received.

The documentation for this interface was generated from the following file:
- [PriceVolumeInfo.cs](../files/PriceVolumeInfo_8cs.md)
