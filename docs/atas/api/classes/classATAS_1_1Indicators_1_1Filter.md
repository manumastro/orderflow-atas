# ATAS.Indicators.Filter< TValue > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Filter.html

Generic filter class that implements the IFilterValue interface.
 [More...](./classATAS_1_1Indicators_1_1Filter.md#details)

Inheritance diagram for ATAS.Indicators.Filter< TValue >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Filter< TValue >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
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
| abstract object | [Clone](./classATAS_1_1Indicators_1_1FilterBase.md#a42f805bcaf064026a2bf891662222f47) () |
| | |
| string | [GetStringValue](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md#a96b95f72bb925b710e30d99fedb86bef) () |
| | Gets the string representation of value of the filter. |
| | |

| Static Public Member Functions | |
| --- | --- |
| static bool | [operator==](./classATAS_1_1Indicators_1_1Filter.md#a45fbef60d1aff15ddc7745ba3584466d) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static bool | [operator!=](./classATAS_1_1Indicators_1_1Filter.md#a00709417810b640918f7e15f2b5cae83) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static | [operator TValue](./classATAS_1_1Indicators_1_1Filter.md#ab04edb85fa955d48943d1bfed327250c) ([Filter](./classATAS_1_1Indicators_1_1Filter.md) other) |
| | Converts the Filter to its value of type TValue . |
| | |
| - Static Public Member Functions inherited from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| static bool | [operator==](./classATAS_1_1Indicators_1_1Filter.md#a45fbef60d1aff15ddc7745ba3584466d) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static bool | [operator!=](./classATAS_1_1Indicators_1_1Filter.md#a00709417810b640918f7e15f2b5cae83) ([Filter](./classATAS_1_1Indicators_1_1Filter.md)? left, [Filter](./classATAS_1_1Indicators_1_1Filter.md)? right) |
| | |
| static | [operator TValue](./classATAS_1_1Indicators_1_1Filter.md#ab04edb85fa955d48943d1bfed327250c) ([Filter](./classATAS_1_1Indicators_1_1Filter.md) other) |
| | Converts the Filter to its value of type TValue . |
| | |

| Protected Member Functions | |
| --- | --- |
| bool | [Equals](./classATAS_1_1Indicators_1_1Filter.md#a3db270e5ee1ed8f8a852047f4688f1db) ([Filter](./classATAS_1_1Indicators_1_1Filter.md) other) |
| | |
| virtual ? TValue | [GetRealValue](./classATAS_1_1Indicators_1_1Filter.md#a50c1b31d8e03190d38c1e9ec15f30580) (TValue? value) |
| | |
| void | [RaiseValueOnChanged](./classATAS_1_1Indicators_1_1Filter.md#a956627f1dcc6899361ced36223949cfc) () |
| | Raises the NotifyPropertyChangedBase.PropertyChanged event for the Value property and invokes the value changed action. |
| | |
| virtual TValue | [ValueOnChanging](./classATAS_1_1Indicators_1_1Filter.md#a006369aa13861d66cd0e7a7ad0f93eb3) (TValue? oldValue, TValue? newValue) |
| | Invoked when the value of the filter is changing. |
| | |
| virtual TFilter | [CreateNew](./classATAS_1_1Indicators_1_1Filter.md#af20e34e49a6b292caf76470ed1096c7b) () |
| | Creates a new instance of the derived filter type. |
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
| - Protected Member Functions inherited from [ATAS.Indicators.FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md) | |
| | [FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md#ace4c39656e88d797c3a1daa1b2bd39b3) (bool enabledVisible, bool asScalar) |
| | Initializes a new instance of the FilterBase class with the specified parameters. |
| | |
| | [FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md#ad4ee3b9baae481a6d887d441f83c8ba2) () |
| | Initializes a new instance of the FilterBase class with default parameters. |
| | |
| - Protected Member Functions inherited from [ATAS.Indicators.NotifyPropertyChangedBase](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | |
| void | [RaisePropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0cc277df1751846320a5a4d5493a7d27) ([CallerMemberName] string? propertyName=null!) |
| | Raises the PropertyChanged event for the specified property name. |
| | |
| void | [SetProperty](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#aaa4ebf490512045cb9606084bb8cae74) (ref TProperty store, TProperty value, [CallerMemberName] string? propertyName=null, Action? onChanged=null) |
| | |

| Properties | |
| --- | --- |
| TValue | [Value](./classATAS_1_1Indicators_1_1Filter.md#a47458cea011a25a1275d5ec88926fa86)`[get, set]` |
| | Gets or sets the value of the filter. |
| | |
| - Properties inherited from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md) | |
| TValue | [Value](./classATAS_1_1Indicators_1_1Filter.md#a47458cea011a25a1275d5ec88926fa86)`[get, set]` |
| | Gets or sets the value of the filter. |
| | |
| - Properties inherited from [ATAS.Indicators.FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md) | |
| bool | [Enabled](./classATAS_1_1Indicators_1_1FilterBase.md#afd2fe159ae0fdd35c11b7e8718a7cff4)`[get, set]` |
| | Gets or sets a value indicating whether the filter is enabled. |
| | |
| bool | [EnabledVisible](./classATAS_1_1Indicators_1_1FilterBase.md#a6d77b9bc8bc6264a25a5323906e0b756)`[get]` |
| | Gets a value indicating whether the visibility of the "Enabled" property is visible to users. |
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
| - Properties inherited from [ATAS.Indicators.IFilterValue](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md) | |
| object | [Value](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md#a46cc717a60c41da88c3e5d7acfdead01)`[get, set]` |
| | Gets or sets the value of the filter. The value can hold data of any type. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Protected Attributes inherited from [ATAS.Indicators.FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md) | |
| readonly bool | [_asScalar](./classATAS_1_1Indicators_1_1FilterBase.md#a00db6a0ba4c414191d6848a53f5c7736) |
| | |
| - Events inherited from [ATAS.Indicators.NotifyPropertyChangedBase](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0dc11be741c756c9a36c0756d2f5527a) |
| | Event that is raised when a property value changes. |
| | |

## Detailed Description

Generic filter class that implements the IFilterValue interface.

Represents a filter with a decimal value type. Inherits from Filter<TValue, TFilter> where TValue is set to decimal and TFilter is set to Filter. The filter's JSON serialization is handled by the custom converter FilterJsonConverter.

Represents a filter with a specific value type TValue . Inherits from Filter<TValue, TFilter> where TFilter is set to Filter<TValue>.

Template Parameters

| TValue | The type of the filter value. |
| --- | --- |
| TFilter | The type of the derived filter. |

Template Parameters

| TValue | The type of value held by the filter. |
| --- | --- |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Filter() [1/6]

| [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).[Filter](./classATAS_1_1Indicators_1_1Filter.md) | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | asScale = `false` |
| | ) | | |

Initializes a new instance of the Filter<TValue, TFilter> class with the specified parameters.

Parameters

| enabledVisible | Specifies whether the "Enabled" property is visible to users. |
| --- | --- |
| asScale | Specifies whether the filter operates in scalar mode. |

## [◆](https://docs.atas.net/en/)Filter() [2/6]

| [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).[Filter](./classATAS_1_1Indicators_1_1Filter.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the Filter<TValue, TFilter> class with default parameters.

## [◆](https://docs.atas.net/en/)Filter() [3/6]

| [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).[Filter](./classATAS_1_1Indicators_1_1Filter.md) | ( | bool | enabledVisible, |
| --- | --- | --- | --- |
| | | bool | asScalar = `false` |
| | ) | | |

Initializes a new instance of the Filter<TValue> class with the specified visibility of the Enabled property and scalar value.

Parameters

| enabledVisible | True if the Enabled property should be visible; otherwise, false. |
| --- | --- |
| asScalar | True if the filter holds a scalar value; otherwise, false. |

## [◆](https://docs.atas.net/en/)Filter() [4/6]

| [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).[Filter](./classATAS_1_1Indicators_1_1Filter.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the Filter<TValue> class with default visibility settings.

## [◆](https://docs.atas.net/en/)Filter() [5/6]

| [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).[Filter](./classATAS_1_1Indicators_1_1Filter.md) | ( | bool | enableVisible, |
| --- | --- | --- | --- |
| | | bool | asScalar = `false` |
| | ) | | |

Initializes a new instance of the Filter class with the specified visibility of the Enabled property and scalar value.

Parameters

| enableVisible | True if the Enabled property should be visible; otherwise, false. |
| --- | --- |
| asScalar | True if the filter holds a scalar value; otherwise, false. |

## [◆](https://docs.atas.net/en/)Filter() [6/6]

| [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).[Filter](./classATAS_1_1Indicators_1_1Filter.md) | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the Filter class with default visibility settings.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| override object [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Implements [ATAS.Indicators.FilterBase](./classATAS_1_1Indicators_1_1FilterBase.md#a42f805bcaf064026a2bf891662222f47).

## [◆](https://docs.atas.net/en/)CreateNew()

| virtual TFilter [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).CreateNew | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Creates a new instance of the derived filter type.

ReturnsThe new instance of the derived filter type.

Reimplemented from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md#af20e34e49a6b292caf76470ed1096c7b).

Reimplemented in [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md#af20e34e49a6b292caf76470ed1096c7b).

## [◆](https://docs.atas.net/en/)Equals() [1/2]

| bool [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).Equals | ( | [Filter](./classATAS_1_1Indicators_1_1Filter.md) > | other | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)Equals() [2/2]

| override bool [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).Equals | ( | object? | obj | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetHashCode()

| override int [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).GetHashCode | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetRealValue()

| virtual ? TValue [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).GetRealValue | ( | TValue? | value | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Reimplemented from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md#a50c1b31d8e03190d38c1e9ec15f30580).

Reimplemented in [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md#a50c1b31d8e03190d38c1e9ec15f30580).

## [◆](https://docs.atas.net/en/)GetStringValue()

| virtual string [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).GetStringValue | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md#aa04415cd8f53d6a033cf4c8e6f09d742).

Reimplemented in [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md#aa04415cd8f53d6a033cf4c8e6f09d742), and [ATAS.Indicators.FilterEnum](./classATAS_1_1Indicators_1_1FilterEnum.md#a82805fc0eb3e2cdf9a8581b69eb63d15).

## [◆](https://docs.atas.net/en/)operator TValue()

| static [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).operator TValue | ( | [Filter](./classATAS_1_1Indicators_1_1Filter.md) > | other | ) | |
| --- | --- | --- | --- | --- | --- |

explicitstatic

Converts the Filter<TValue, TFilter> to its value of type TValue .

Parameters

| other | The filter to convert. |
| --- | --- |

## [◆](https://docs.atas.net/en/)operator!=()

| static bool [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).operator!= | ( | [Filter](./classATAS_1_1Indicators_1_1Filter.md) >? | left, |
| --- | --- | --- | --- |
| | | [Filter](./classATAS_1_1Indicators_1_1Filter.md) >? | right |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)operator==()

| static bool [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).operator== | ( | [Filter](./classATAS_1_1Indicators_1_1Filter.md) >? | left, |
| --- | --- | --- | --- |
| | | [Filter](./classATAS_1_1Indicators_1_1Filter.md) >? | right |
| | ) | | |

static

## [◆](https://docs.atas.net/en/)RaiseValueOnChanged()

| void [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).RaiseValueOnChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Raises the NotifyPropertyChangedBase.PropertyChanged event for the Value property and invokes the value changed action.

## [◆](https://docs.atas.net/en/)SetValueSilently()

| bool [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).SetValueSilently | ( | TValue | value | ) | |
| --- | --- | --- | --- | --- | --- |

Sets value of Value property. Returns false when new value equals new value.

Parameters

| value | |
| --- | --- |

Returns

## [◆](https://docs.atas.net/en/)ToString()

| override string [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Converts the filter to its string representation.

ReturnsA string that represents the filter.

## [◆](https://docs.atas.net/en/)ValueOnChanged()

| TFilter [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).ValueOnChanged | ( | Action | onChanged | ) | |
| --- | --- | --- | --- | --- | --- |

Sets an action to be invoked when the value of the filter has changed.

Parameters

| onChanged | The action to be invoked. |
| --- | --- |

ReturnsThe current filter instance.

## [◆](https://docs.atas.net/en/)ValueOnChanging() [1/2]

| TFilter [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).ValueOnChanging | ( | Func, TValue > | onChanging | ) | |
| --- | --- | --- | --- | --- | --- |

Sets a function to be invoked when the value of the filter is changing.

Parameters

| onChanging | The function to be invoked. |
| --- | --- |

ReturnsThe current filter instance.

## [◆](https://docs.atas.net/en/)ValueOnChanging() [2/2]

| virtual TValue [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).ValueOnChanging | ( | TValue? | oldValue, |
| --- | --- | --- | --- |
| | | TValue? | newValue |
| | ) | | |

protectedvirtual

Invoked when the value of the filter is changing.

Parameters

| oldValue | The old value of the filter. |
| --- | --- |
| newValue | The new value of the filter. |

ReturnsThe new value to be set for the filter.

Reimplemented from [ATAS.Indicators.Filter >](./classATAS_1_1Indicators_1_1Filter.md#a006369aa13861d66cd0e7a7ad0f93eb3).

Reimplemented in [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md#a006369aa13861d66cd0e7a7ad0f93eb3).

## Property Documentation

## [◆](https://docs.atas.net/en/)Value

| TValue [ATAS.Indicators.Filter](./classATAS_1_1Indicators_1_1Filter.md).Value |
| --- |

getset

Gets or sets the value of the filter.

Implements [ATAS.Indicators.IFilterValue](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md#a46cc717a60c41da88c3e5d7acfdead01).

The documentation for this class was generated from the following file:
- [Filter.cs](../files/Filter_8cs.md)
