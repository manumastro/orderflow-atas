# ATAS.Indicators.FilterRangeBase< TValue, TFilter > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1FilterRangeBase.html

Represents an abstract base class for filters that represent a range of values with custom JSON serialization/deserialization.
 [More...](./classATAS_1_1Indicators_1_1FilterRangeBase.md#details)

Inheritance diagram for ATAS.Indicators.FilterRangeBase< TValue, TFilter >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.FilterRangeBase< TValue, TFilter >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Protected Member Functions | |
| --- | --- |
| | [FilterRangeBase](./classATAS_1_1Indicators_1_1FilterRangeBase.md#ae1c27adead71fdfa3515a3adc63eaa5a) (bool enabledVisible, bool asScale=false) |
| | Initializes a new instance of the FilterRangeBase class with the specified settings. |
| | |
| | [FilterRangeBase](./classATAS_1_1Indicators_1_1FilterRangeBase.md#acd0f26643c801360ad5dd00d3daa1c6f) () |
| | Initializes a new instance of the FilterRangeBase class with default settings. |
| | |
| override [FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md) | [ValueOnChanging](./classATAS_1_1Indicators_1_1FilterRangeBase.md#ac94a372381d2583a0cc814a54b3b2f56) ([FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md)? oldValue, [FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md)? newValue) |
| | |
| - Protected Member Functions inherited from [ATAS.Indicators.Filter, TFilter >](./classATAS_1_1Indicators_1_1Filter.md) | |
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

| Additional Inherited Members | |
| --- | --- |
| - Public Member Functions inherited from [ATAS.Indicators.Filter, TFilter >](./classATAS_1_1Indicators_1_1Filter.md) | |
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
| - Static Public Member Functions inherited from [ATAS.Indicators.Filter, TFilter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| static bool | [operator==](./classATAS_1_1Indicators_1_1Filter.md#a45fbef60d1aff15ddc7745ba3584466d) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static bool | [operator!=](./classATAS_1_1Indicators_1_1Filter.md#a00709417810b640918f7e15f2b5cae83) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static | [operator TValue](./classATAS_1_1Indicators_1_1Filter.md#ab04edb85fa955d48943d1bfed327250c) ([Filter](./classATAS_1_1Indicators_1_1Filter.md) other) |
| | Converts the Filter to its value of type TValue . |
| | |
| - Properties inherited from [ATAS.Indicators.Filter, TFilter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| TValue | [Value](./classATAS_1_1Indicators_1_1Filter.md#a47458cea011a25a1275d5ec88926fa86)`[get, set]` |
| | Gets or sets the value of the filter. |
| | |

## Detailed Description

Represents an abstract base class for filters that represent a range of values with custom JSON serialization/deserialization.

Template Parameters

| TValue | The type of values in the range. |
| --- | --- |
| TFilter | The derived filter class that implements the IFilterValue interface. |

Type Constraints

| TFilter | : | [FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md) | |
| --- | --- | --- | --- |
| TFilter | : | [IFilterValue](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md) | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)FilterRangeBase() [1/2]

| [ATAS.Indicators.FilterRangeBase](./classATAS_1_1Indicators_1_1FilterRangeBase.md).[FilterRangeBase](./classATAS_1_1Indicators_1_1FilterRangeBase.md) | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | asScale = `false` |
| | ) | | |

protected

Initializes a new instance of the FilterRangeBase<TValue, TFilter> class with the specified settings.

Parameters

| enabledVisible | A value indicating whether the filter's Enabled property is visible. |
| --- | --- |
| asScale | A value indicating whether the filter represents a scalar value. |

## [◆](https://docs.atas.net/en/)FilterRangeBase() [2/2]

| [ATAS.Indicators.FilterRangeBase](./classATAS_1_1Indicators_1_1FilterRangeBase.md).[FilterRangeBase](./classATAS_1_1Indicators_1_1FilterRangeBase.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Initializes a new instance of the FilterRangeBase<TValue, TFilter> class with default settings.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ValueOnChanging()

| override [FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md) [ATAS.Indicators.FilterRangeBase](./classATAS_1_1Indicators_1_1FilterRangeBase.md).ValueOnChanging | ( | [FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md)? | oldValue, |
| --- | --- | --- | --- |
| | | [FilterRangeValue](./classATAS_1_1Indicators_1_1FilterRangeValue.md)? | newValue |
| | ) | | |

protected

The documentation for this class was generated from the following file:
- [FilterRangeBase.cs](../files/FilterRangeBase_8cs.md)
