# ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.html

Inheritance diagram for ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| override [FilterRenderPen](./classATAS_1_1Indicators_1_1FilterRenderPen.md) | [Create](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#afcd79b6f5e7cb1ed5122d06a78477552) (bool enabledVisible, bool enabled) |
| | |
| override void | [WriteValue](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#a1108507cca4d0ea949c07d54e079694d) (JsonWriter writer, PenSettings value) |
| | |
| override [FilterRenderPen](./classATAS_1_1Indicators_1_1FilterRenderPen.md) | [ReadFromObject](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#a7d30da6613886d48b841d1065a6be73e) (JsonReader reader, [FilterRenderPen](./classATAS_1_1Indicators_1_1FilterRenderPen.md)? filter) |
| | |
| override? PenSettings | [ReadValue](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#ae4748f3b4c2973cd288eda40a76eb767) (JsonReader reader) |
| | |
| - Protected Member Functions inherited from [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
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
| string | [ColorPropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#a3a91564f1bbe11602811b8f0f8969d27) = nameof(PenSettings.Color).ToLower()`[get]` |
| | |
| string | [LineStylePropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#a705f0f8a7a5f874782425a0a65705f9c) = nameof(PenSettings.LineDashStyle).ToLower()`[get]` |
| | |
| string | [WidthPropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md#a868b822a9b9c9c8290af8b8b4319068d) = nameof(PenSettings.Width).ToLower()`[get]` |
| | |
| - Properties inherited from [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| string | [ValuePropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a7d9fe246a54d8f950766f3c8e226d92e)`[get]` |
| | |
| string | [EnabledPropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a131bd2ba2af0acca9b2b9f5889d2a915)`[get]` |
| | |
| string | [EnabledVisiblePropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#aac09d5331917e31e02a27af88a47eeb1)`[get]` |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Public Member Functions inherited from [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| override void | [WriteJson](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#acfc48c6da445846adc2a7699153770cc) (JsonWriter writer, object? value, JsonSerializer serializer) |
| | |
| override? object | [ReadJson](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#adf8298c0e7c64a04630a71eb62c98aa2) (JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) |
| | |
| override bool | [CanConvert](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a40877e0cb72ffb5de9e1d7f4ed1ddb70) (Type objectType) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Create()

| override [FilterRenderPen](./classATAS_1_1Indicators_1_1FilterRenderPen.md) ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter.Create | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | enabled |
| | ) | | |

protectedvirtual

Implements [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a76652affa759e392d2e788e06b096340).

## [◆](https://docs.atas.net/en/)ReadFromObject()

| override [FilterRenderPen](./classATAS_1_1Indicators_1_1FilterRenderPen.md) ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter.ReadFromObject | ( | JsonReader | reader, |
| --- | --- | --- | --- |
| | | [FilterRenderPen](./classATAS_1_1Indicators_1_1FilterRenderPen.md)? | filter |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)ReadValue()

| override? PenSettings ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter.ReadValue | ( | JsonReader | reader | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a58522298e181145feee48b7e49f7af19).

## [◆](https://docs.atas.net/en/)WriteValue()

| override void ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter.WriteValue | ( | JsonWriter | writer, |
| --- | --- | --- | --- |
| | | PenSettings | value |
| | ) | | |

protected

## Property Documentation

## [◆](https://docs.atas.net/en/)ColorPropertyName

| string ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter.ColorPropertyName = nameof(PenSettings.Color).ToLower() |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)LineStylePropertyName

| string ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter.LineStylePropertyName = nameof(PenSettings.LineDashStyle).ToLower() |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)WidthPropertyName

| string ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter.WidthPropertyName = nameof(PenSettings.Width).ToLower() |
| --- |

getprotected

The documentation for this class was generated from the following file:
- [FilterRenderPenJsonConverter.cs](../files/FilterRenderPenJsonConverter_8cs.md)
