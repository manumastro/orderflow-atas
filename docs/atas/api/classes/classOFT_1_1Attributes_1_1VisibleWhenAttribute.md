# OFT.Attributes.VisibleWhenAttribute Class Reference

Source: https://docs.atas.net/en/classOFT_1_1Attributes_1_1VisibleWhenAttribute.html

Makes the property visible only when the specified source property's value matches one of the provided values. Used for dynamic property visibility in the settings UI (e.g., showing/hiding properties based on a mode enum).
 [More...](./classOFT_1_1Attributes_1_1VisibleWhenAttribute.md#details)

Inheritance diagram for OFT.Attributes.VisibleWhenAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for OFT.Attributes.VisibleWhenAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [VisibleWhenAttribute](./classOFT_1_1Attributes_1_1VisibleWhenAttribute.md#ac28b7578a3c1c5bdc789078b4007bcdf) (string propertyName, params object[] values) |
| | |

| Properties | |
| --- | --- |
| string | [PropertyName](./classOFT_1_1Attributes_1_1VisibleWhenAttribute.md#af0d6ba8a948a2a264bd1d030745a5168)`[get]` |
| | Name of the source property whose value determines visibility. |
| | |
| object[] | [Values](./classOFT_1_1Attributes_1_1VisibleWhenAttribute.md#a937dda3cea3088b69a80f5eb398cf3f6)`[get]` |
| | The set of values for which this property is visible. If the source property's current value is in this set, the property is shown. |
| | |

## Detailed Description

Makes the property visible only when the specified source property's value matches one of the provided values. Used for dynamic property visibility in the settings UI (e.g., showing/hiding properties based on a mode enum).

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)VisibleWhenAttribute()

| OFT.Attributes.VisibleWhenAttribute.VisibleWhenAttribute | ( | string | propertyName, |
| --- | --- | --- | --- |
| | | params object[] | values |
| | ) | | |

Parameters

| propertyName | Name of the source property whose value controls visibility. |
| --- | --- |
| values | Allowed values of the source property for this property to be visible. |

## Property Documentation

## [◆](https://docs.atas.net/en/)PropertyName

| string OFT.Attributes.VisibleWhenAttribute.PropertyName |
| --- |

get

Name of the source property whose value determines visibility.

## [◆](https://docs.atas.net/en/)Values

| object [] OFT.Attributes.VisibleWhenAttribute.Values |
| --- |

get

The set of values for which this property is visible. If the source property's current value is in this set, the property is shown.

The documentation for this class was generated from the following file:
- [VisibleWhenAttribute.cs](../files/VisibleWhenAttribute_8cs.md)
