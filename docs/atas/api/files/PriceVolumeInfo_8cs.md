# PriceVolumeInfo.cs File Reference

Source: https://docs.atas.net/en/PriceVolumeInfo_8cs.html

| Classes | |
| --- | --- |
| class | [ATAS.Indicators.PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) |
| | Represents information on volumes at a specific price. [More...](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#details) |
| | |
| class | [ATAS.Indicators.ValueArea](../classes/classATAS_1_1Indicators_1_1ValueArea.md) |
| | Represents information on Value area high/low. [More...](../classes/classATAS_1_1Indicators_1_1ValueArea.md#details) |
| | |
| interface | [ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md) |
| | Represents an interface for supporting price information. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#details) |
| | |
| interface | [ATAS.Indicators.IIntCandle](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md) |
| | Represents an interface for an integer-based candle. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md#details) |
| | |
| class | [ATAS.Indicators.FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) |
| | Represents a request for a fixed profile with a specific period. [More...](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md#details) |
| | |
| interface | [ATAS.Indicators.IKnowFixedProfiles](../interfaces/interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md) |
| | Represents an interface for objects that know fixed profiles and can request them. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [ATAS.Indicators.FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) {
  [ATAS.Indicators.CurrentDay](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8a4daff4cb75e848f1acb354bd0497f959) = 0
, [ATAS.Indicators.LastDay](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8aeeefeee31fc765eebf1b91de608ec12b) = 1
, [ATAS.Indicators.CurrentWeek](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8ad9f73d726b6ea5189402fbbc014f6bf0) = 2
, [ATAS.Indicators.LastWeek](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8a702e97487c31fee5bf11b7c5c44a65cc) = 3
,
  [ATAS.Indicators.CurrentMonth](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8ae98028a6370c2d4924f8916e6f248982) = 4
, [ATAS.Indicators.LastMonth](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8a41eeb970a31e7ca35dbe1003b3694156) = 5
, [ATAS.Indicators.Contract](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8af49498143b94e78415d06029763412b9) = 6

 } |
| | Enumeration representing fixed profile periods. [More...](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) |
| | |

| Functions | |
| --- | --- |
| record struct | [ATAS.Indicators.FixedProfileResponse](../namespaces/namespaceATAS_1_1Indicators.md#a77a4afd41de8b48767b2066b36570332) ([IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) Scaled, [IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) Original) |
| | |
