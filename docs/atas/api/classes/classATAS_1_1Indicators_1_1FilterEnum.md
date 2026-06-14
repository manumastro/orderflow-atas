# ATAS.Indicators.FilterEnum< TEnum > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1FilterEnum.html

Inheritance diagram for ATAS.Indicators.FilterEnum< TEnum >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.FilterEnum< TEnum >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md#a63a499144eabbf1f47d957c1b983a1a3) (bool enabledVisible, bool asScalar=false) |
| | |
| | [FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md#ae5450b7e47df625de9d2dfbd1f61923f) () |
| | |
| override string | [GetStringValue](./classATAS_1_1Indicators_1_1FilterEnum.md#a82805fc0eb3e2cdf9a8581b69eb63d15) () |
| | |
| - Public Member Functions inherited from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| override bool | [Equals](./classATAS_1_1Indicators_1_1Filter.md#a7c6283c6cf65d9c6893379cee4b1a654) (object? obj) |
| | |
| override int | [GetHashCode](./classATAS_1_1Indicators_1_1Filter.md#a910b7c8eb872efd50d5fed44a7df1ee8) () |
| | |
| bool | [SetValueSilently](./classATAS_1_1Indicators_1_1Filter.md#a39b743350fbe568a6b18b7efdf382dc4) (TValue value) |
| | Sets value of Value property. Returns false when new value equals new value. |
| | |
| | [Filter](./classATAS_1_1Indicators_1_1Filter.md#a28db91c1e484323c98e5f294b0208379) (bool enabledVisible, bool asScale=false) |
| | Initializes a new instance of the Filter class with the specified parameters. |
| | |
| | [Filter](./classATAS_1_1Indicators_1_1Filter.md#ad4794335b6c9d05f30a92147357787c2) () |
| | Initializes a new instance of the Filter class with default parameters. |
| | |
| | [Filter](./classATAS_1_1Indicators_1_1Filter.md#a29aaec0d1cdca51984523395051fa99a) (bool enabledVisible, bool asScalar=false) |
| | Initializes a new instance of the Filter class with the specified visibility of the Enabled property and scalar value. |
| | |
| | [Filter](./classATAS_1_1Indicators_1_1Filter.md#ad4794335b6c9d05f30a92147357787c2) () |
| | Initializes a new instance of the Filter class with default visibility settings. |
| | |
| | [Filter](./classATAS_1_1Indicators_1_1Filter.md#ad6aeabbae8b9c75d95f6c7a079069cde) (bool enableVisible, bool asScalar=false) |
| | Initializes a new instance of the Filter class with the specified visibility of the Enabled property and scalar value. |
| | |
| | [Filter](./classATAS_1_1Indicators_1_1Filter.md#ad4794335b6c9d05f30a92147357787c2) () |
| | Initializes a new instance of the Filter class with default visibility settings. |
| | |
| TFilter | [ValueOnChanging](./classATAS_1_1Indicators_1_1Filter.md#ab4ead4e01821e9320f201252019a5b06) (Func, TValue > onChanging) |
| | Sets a function to be invoked when the value of the filter is changing. |
| | |
| TFilter | [ValueOnChanged](./classATAS_1_1Indicators_1_1Filter.md#a0f1de7d467e3795fbe8b82420b5b6431) (Action onChanged) |
| | Sets an action to be invoked when the value of the filter has changed. |
| | |
| override string | [ToString](./classATAS_1_1Indicators_1_1Filter.md#aa595c8e473c5a2073a32b83436925644) () |
| | Converts the filter to its string representation. |
| | |
| virtual string | [GetStringValue](./classATAS_1_1Indicators_1_1Filter.md#aa04415cd8f53d6a033cf4c8e6f09d742) () |
| | |
| override object | [Clone](./classATAS_1_1Indicators_1_1Filter.md#ad751565da10c97facaaba8605546ddf9) () |
| | |

| Properties | |
| --- | --- |
| Type | [EnumType](./classATAS_1_1Indicators_1_1FilterEnum.md#ab5916813693ba53e941f5d8e7e79b4cc) = typeof(TEnum)`[get]` |
| | |
| - Properties inherited from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| TValue | [Value](./classATAS_1_1Indicators_1_1Filter.md#a47458cea011a25a1275d5ec88926fa86)`[get, set]` |
| | Gets or sets the value of the filter. |
| | |
| - Properties inherited from [ATAS.Indicators.IFilterEnum](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterEnum.md) | |
| Type | [EnumType](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterEnum.md#a70e41902a81a8a14ec49d22d4b4f1006)`[get]` |
| | |
| - Properties inherited from [ATAS.Indicators.IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md) | |
| bool | [Enabled](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#a40afdac09510881a97b9fb7059f09db5)`[get, set]` |
| | Gets or sets a value indicating whether the filter is enabled. |
| | |
| bool | [EnabledVisible](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#afd4286d2401b7286159d764dd05b1328)`[get]` |
| | Gets a value indicating whether the visibility of the "Enabled" property is visible to users. |
| | |
| bool | [AsScalar](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#acb501180f87b4cd563f8a428dc18328c)`[get]` |
| | Gets a value indicating whether the filter operates in scalar mode. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Static Public Member Functions inherited from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| static bool | [operator==](./classATAS_1_1Indicators_1_1Filter.md#a45fbef60d1aff15ddc7745ba3584466d) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static bool | [operator!=](./classATAS_1_1Indicators_1_1Filter.md#a00709417810b640918f7e15f2b5cae83) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static | [operator TValue](./classATAS_1_1Indicators_1_1Filter.md#ab04edb85fa955d48943d1bfed327250c) ([Filter](./classATAS_1_1Indicators_1_1Filter.md) other) |
| | Converts the Filter to its value of type TValue . |
| | |
| - Protected Member Functions inherited from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| bool | [Equals](./classATAS_1_1Indicators_1_1Filter.md#a3db270e5ee1ed8f8a852047f4688f1db) ([Filter](./classATAS_1_1Indicators_1_1Filter.md) other) |
| | |
| virtual ? TValue | [GetRealValue](./classATAS_1_1Indicators_1_1Filter.md#a50c1b31d8e03190d38c1e9ec15f30580) (TValue? value) |
| | |
| virtual TValue | [ValueOnChanging](./classATAS_1_1Indicators_1_1Filter.md#a006369aa13861d66cd0e7a7ad0f93eb3) (TValue? oldValue, TValue? newValue) |
| | Invoked when the value of the filter is changing. |
| | |
| void | [RaiseValueOnChanged](./classATAS_1_1Indicators_1_1Filter.md#a956627f1dcc6899361ced36223949cfc) () |
| | Raises the NotifyPropertyChangedBase.PropertyChanged event for the Value property and invokes the value changed action. |
| | |
| virtual TFilter | [CreateNew](./classATAS_1_1Indicators_1_1Filter.md#af20e34e49a6b292caf76470ed1096c7b) () |
| | Creates a new instance of the derived filter type. |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)FilterEnum() [1/2]

| [ATAS.Indicators.FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md).[FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md) | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | asScalar = `false` |
| | ) | | |

## [◆](https://docs.atas.net/en/)FilterEnum() [2/2]

| [ATAS.Indicators.FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md).[FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetStringValue()

| override string [ATAS.Indicators.FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md).GetStringValue | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md#aa04415cd8f53d6a033cf4c8e6f09d742).

## Property Documentation

## [◆](https://docs.atas.net/en/)EnumType

| Type [ATAS.Indicators.FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md).EnumType = typeof(TEnum) |
| --- |

get

Implements [ATAS.Indicators.IFilterEnum](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterEnum.md#a70e41902a81a8a14ec49d22d4b4f1006).

The documentation for this class was generated from the following file:
- [FilterEnum.cs](../files/FilterEnum_8cs.md)
