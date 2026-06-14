# OFT.Attributes.TabAttribute Class Reference

Source: https://docs.atas.net/en/classOFT_1_1Attributes_1_1TabAttribute.html

Attribute that assigns a property or all properties of a class to a named tab in the settings UI. Use alongside System.ComponentModel.DataAnnotations.DisplayAttribute for property display metadata (Name, GroupName, Description, Order).
 [More...](./classOFT_1_1Attributes_1_1TabAttribute.md#details)

Inheritance diagram for OFT.Attributes.TabAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for OFT.Attributes.TabAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| string? | [GetLocalizedTab](./classOFT_1_1Attributes_1_1TabAttribute.md#a64a62784616a7846e618d8b2170f010a) () |
| | Gets the localized tab name. Returns `null` if TabName is not set. |
| | |
| int? | [GetTabOrder](./classOFT_1_1Attributes_1_1TabAttribute.md#a4c522ca93af4f364fd55b7b50604ea44) () |
| | Returns the explicitly assigned TabOrder, or `null` if not set. |
| | |

| Properties | |
| --- | --- |
| string? | [TabName](./classOFT_1_1Attributes_1_1TabAttribute.md#aee05a41cb8b1ba39efef6e85b76ffbb9)`[get, set]` |
| | Tab name. If ResourceType is set, this is treated as a resource key. |
| | |
| int | [TabOrder](./classOFT_1_1Attributes_1_1TabAttribute.md#a317b36ab4575dd46fb4fd9cc3b0162e3)`[get, set]` |
| | Tab display order. Lower values appear first. Tabs with equal order are sorted by the order their first property appears in the class. When not set, the tab is sorted purely by its source declaration order. |
| | |
| Type? | [ResourceType](./classOFT_1_1Attributes_1_1TabAttribute.md#a47db85998307b6793d6f9693c217db85)`[get, set]` |
| | Resource type for localization of TabName. |
| | |

## Detailed Description

Attribute that assigns a property or all properties of a class to a named tab in the settings UI. Use alongside System.ComponentModel.DataAnnotations.DisplayAttribute for property display metadata (Name, GroupName, Description, Order).

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetLocalizedTab()

| string? OFT.Attributes.TabAttribute.GetLocalizedTab | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets the localized tab name. Returns `null` if TabName is not set.

## [◆](https://docs.atas.net/en/)GetTabOrder()

| int? OFT.Attributes.TabAttribute.GetTabOrder | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns the explicitly assigned TabOrder, or `null` if not set.

## Property Documentation

## [◆](https://docs.atas.net/en/)ResourceType

| Type? OFT.Attributes.TabAttribute.ResourceType |
| --- |

getset

Resource type for localization of TabName.

## [◆](https://docs.atas.net/en/)TabName

| string? OFT.Attributes.TabAttribute.TabName |
| --- |

getset

Tab name. If ResourceType is set, this is treated as a resource key.

## [◆](https://docs.atas.net/en/)TabOrder

| int OFT.Attributes.TabAttribute.TabOrder |
| --- |

getset

Tab display order. Lower values appear first. Tabs with equal order are sorted by the order their first property appears in the class. When not set, the tab is sorted purely by its source declaration order.

The documentation for this class was generated from the following file:
- [TabAttribute.cs](../files/TabAttribute_8cs.md)
