# ATAS.Indicators.IFilterValue Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IFilterValue.html

Represents a filter with a flexible value that can hold data of any type. Inherits the properties of IFilter.
 [More...](./interfaceATAS_1_1Indicators_1_1IFilterValue.md#details)

Inheritance diagram for ATAS.Indicators.IFilterValue:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IFilterValue:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| string | [GetStringValue](./interfaceATAS_1_1Indicators_1_1IFilterValue.md#a96b95f72bb925b710e30d99fedb86bef) () |
| | Gets the string representation of value of the filter. |
| | |

| Properties | |
| --- | --- |
| object | [Value](./interfaceATAS_1_1Indicators_1_1IFilterValue.md#a46cc717a60c41da88c3e5d7acfdead01)`[get, set]` |
| | Gets or sets the value of the filter. The value can hold data of any type. |
| | |
| - Properties inherited from [ATAS.Indicators.IFilter](./interfaceATAS_1_1Indicators_1_1IFilter.md) | |
| bool | [Enabled](./interfaceATAS_1_1Indicators_1_1IFilter.md#a40afdac09510881a97b9fb7059f09db5)`[get, set]` |
| | Gets or sets a value indicating whether the filter is enabled. |
| | |
| bool | [EnabledVisible](./interfaceATAS_1_1Indicators_1_1IFilter.md#afd4286d2401b7286159d764dd05b1328)`[get]` |
| | Gets a value indicating whether the visibility of the "Enabled" property is visible to users. |
| | |
| bool | [AsScalar](./interfaceATAS_1_1Indicators_1_1IFilter.md#acb501180f87b4cd563f8a428dc18328c)`[get]` |
| | Gets a value indicating whether the filter operates in scalar mode. |
| | |

## Detailed Description

Represents a filter with a flexible value that can hold data of any type. Inherits the properties of IFilter.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetStringValue()

| string ATAS.Indicators.IFilterValue.GetStringValue | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets the string representation of value of the filter.

ReturnsThe string representation of the value.

Implemented in [ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md#aa04415cd8f53d6a033cf4c8e6f09d742).

## Property Documentation

## [◆](https://docs.atas.net/en/)Value

| object ATAS.Indicators.IFilterValue.Value |
| --- |

getset

Gets or sets the value of the filter. The value can hold data of any type.

Implemented in [ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md#a47458cea011a25a1275d5ec88926fa86).

The documentation for this interface was generated from the following file:
- [IFilterValue.cs](../files/IFilterValue_8cs.md)
