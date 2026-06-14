# ATAS.Indicators.Filters.Converters.FilterJsonConverterBase< TFilter, TValue > Class Template Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.html

Inheritance diagram for ATAS.Indicators.Filters.Converters.FilterJsonConverterBase< TFilter, TValue >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Filters.Converters.FilterJsonConverterBase< TFilter, TValue >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| override void | [WriteJson](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#acfc48c6da445846adc2a7699153770cc) (JsonWriter writer, object? value, JsonSerializer serializer) |
| | |
| override? object | [ReadJson](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#adf8298c0e7c64a04630a71eb62c98aa2) (JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) |
| | |
| override bool | [CanConvert](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a40877e0cb72ffb5de9e1d7f4ed1ddb70) (Type objectType) |
| | |

| Protected Member Functions | |
| --- | --- |
| abstract TFilter | [Create](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a76652affa759e392d2e788e06b096340) (bool enabledVisible, bool enabled) |
| | |
| TFilter | [CreateFromValue](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a7b691dfdfe4e5ec78e96f1386faab8a7) (TFilter? filter, TValue? value) |
| | |
| virtual TFilter | [ReadFromObject](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#ae12832ba7d9281e4b31565c6f86f4ed5) (JsonReader reader, TFilter? filter) |
| | |
| virtual ? TValue | [ReadValue](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a58522298e181145feee48b7e49f7af19) (JsonReader reader) |
| | |
| virtual ? object | [CreateStoredValue](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#af35c547423131fdf567a9daf5edc1593) (TValue value) |
| | |
| virtual void | [WriteValue](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#abe736247c4c771f6b8c3b5deefefe9c0) (JsonWriter writer, TValue value) |
| | |

| Properties | |
| --- | --- |
| string | [ValuePropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a7d9fe246a54d8f950766f3c8e226d92e) = nameof([Filter.Value](./classATAS_1_1Indicators_1_1Filter.md#a47458cea011a25a1275d5ec88926fa86)).ToLower()`[get]` |
| | |
| string | [EnabledPropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a131bd2ba2af0acca9b2b9f5889d2a915) = nameof([Filter.Enabled](./classATAS_1_1Indicators_1_1FilterBase.md#afd2fe159ae0fdd35c11b7e8718a7cff4)).ToLower()`[get]` |
| | |
| string | [EnabledVisiblePropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#aac09d5331917e31e02a27af88a47eeb1) = nameof([Filter.EnabledVisible](./classATAS_1_1Indicators_1_1FilterBase.md#a6d77b9bc8bc6264a25a5323906e0b756)).ToLower()`[get]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CanConvert()

| override bool [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).CanConvert | ( | Type | objectType | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Create()

| abstract TFilter [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).Create | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | enabled |
| | ) | | |

protectedpure virtual

Implemented in [ATAS.Indicators.Filters.Converters.FilterBoolJsonConvert](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterBoolJsonConvert.md#aaba609f4dfb81c86c2cfb63e6ff8368a), [ATAS.Indicators.Filters.Converters.FilterColorJsonConverter](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterColorJsonConverter.md#a6d3769c0113b7d561a311546571422df), [ATAS.Indicators.Filters.Converters.FilterHeatmapTypesJsonConverter](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterHeatmapTypesJsonConverter.md#acf614a3059ae832c81445bd8ddc11711), [ATAS.Indicators.Filters.Converters.FilterIntJsonConverter](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterIntJsonConverter.md#a7ca1ceb8c7c30d66c01d35e73b22901d), [ATAS.Indicators.Filters.Converters.FilterJsonConverter](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverter.md#adf7fec45474d7eaa09dc8a170860b440), [ATAS.Indicators.Filters.Converters.FilterKeyJsonConvert](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterKeyJsonConvert.md#a3b247a618b894167e4ab3141a78997f5), [ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#afcd79b6f5e7cb1ed5122d06a78477552), [ATAS.Indicators.Filters.Converters.FilterStringJsonConverter](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterStringJsonConverter.md#adc4c4bc33a93f5ca679d4b302a3f1d08), and [ATAS.Indicators.Filters.Converters.FilterTimeSpanJsonConvert](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterTimeSpanJsonConvert.md#ab0e0dbb820e79cf61fa35baff1d9224a).

## [◆](https://docs.atas.net/en/)CreateFromValue()

| TFilter [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).CreateFromValue | ( | TFilter? | filter, |
| --- | --- | --- | --- |
| | | TValue? | value |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)CreateStoredValue()

| virtual ? object [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).CreateStoredValue | ( | TValue | value | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ReadFromObject()

| virtual TFilter [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).ReadFromObject | ( | JsonReader | reader, |
| --- | --- | --- | --- |
| | | TFilter? | filter |
| | ) | | |

protectedvirtual

Reimplemented in [ATAS.Indicators.Filters.Converters.FilterRangeJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRangeJsonConverterBase.md#a0c884b097f20365825c7b4972c98d5e6).

## [◆](https://docs.atas.net/en/)ReadJson()

| override? object [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).ReadJson | ( | JsonReader | reader, |
| --- | --- | --- | --- |
| | | Type | objectType, |
| | | object? | existingValue, |
| | | JsonSerializer | serializer |
| | ) | | |

## [◆](https://docs.atas.net/en/)ReadValue()

| virtual ? TValue [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).ReadValue | ( | JsonReader | reader | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented in [ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#ae4748f3b4c2973cd288eda40a76eb767).

## [◆](https://docs.atas.net/en/)WriteJson()

| override void [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).WriteJson | ( | JsonWriter | writer, |
| --- | --- | --- | --- |
| | | object? | value, |
| | | JsonSerializer | serializer |
| | ) | | |

## [◆](https://docs.atas.net/en/)WriteValue()

| virtual void [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).WriteValue | ( | JsonWriter | writer, |
| --- | --- | --- | --- |
| | | TValue | value |
| | ) | | |

protectedvirtual

## Property Documentation

## [◆](https://docs.atas.net/en/)EnabledPropertyName

| string [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).EnabledPropertyName = nameof([Filter.Enabled](./classATAS_1_1Indicators_1_1FilterBase.md#afd2fe159ae0fdd35c11b7e8718a7cff4)).ToLower() |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)EnabledVisiblePropertyName

| string [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).EnabledVisiblePropertyName = nameof([Filter.EnabledVisible](./classATAS_1_1Indicators_1_1FilterBase.md#a6d77b9bc8bc6264a25a5323906e0b756)).ToLower() |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)ValuePropertyName

| string [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md).ValuePropertyName = nameof([Filter.Value](./classATAS_1_1Indicators_1_1Filter.md#a47458cea011a25a1275d5ec88926fa86)).ToLower() |
| --- |

getprotected

The documentation for this class was generated from the following file:
- [FilterJsonConverterBase.cs](../files/FilterJsonConverterBase_8cs.md)
