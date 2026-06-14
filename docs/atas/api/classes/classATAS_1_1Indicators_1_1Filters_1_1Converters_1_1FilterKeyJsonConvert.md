# ATAS.Indicators.Filters.Converters.FilterKeyJsonConvert Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterKeyJsonConvert.html

Inheritance diagram for ATAS.Indicators.Filters.Converters.FilterKeyJsonConvert:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Filters.Converters.FilterKeyJsonConvert:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| override [FilterKey](./classATAS_1_1Indicators_1_1FilterKey.md) | [Create](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterKeyJsonConvert.md#a3b247a618b894167e4ab3141a78997f5) (bool enabledVisible, bool enabled) |
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

| Additional Inherited Members | |
| --- | --- |
| - Public Member Functions inherited from [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| override void | [WriteJson](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#acfc48c6da445846adc2a7699153770cc) (JsonWriter writer, object? value, JsonSerializer serializer) |
| | |
| override? object | [ReadJson](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#adf8298c0e7c64a04630a71eb62c98aa2) (JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) |
| | |
| override bool | [CanConvert](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a40877e0cb72ffb5de9e1d7f4ed1ddb70) (Type objectType) |
| | |
| - Properties inherited from [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| string | [ValuePropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a7d9fe246a54d8f950766f3c8e226d92e)`[get]` |
| | |
| string | [EnabledPropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a131bd2ba2af0acca9b2b9f5889d2a915)`[get]` |
| | |
| string | [EnabledVisiblePropertyName](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#aac09d5331917e31e02a27af88a47eeb1)`[get]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Create()

| override [FilterKey](./classATAS_1_1Indicators_1_1FilterKey.md) ATAS.Indicators.Filters.Converters.FilterKeyJsonConvert.Create | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | enabled |
| | ) | | |

protectedvirtual

Implements [ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](./classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md#a76652affa759e392d2e788e06b096340).

The documentation for this class was generated from the following file:
- [FilterKeyJsonConvert.cs](../files/FilterKeyJsonConvert_8cs.md)
