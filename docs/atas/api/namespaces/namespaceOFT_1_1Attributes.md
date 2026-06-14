# OFT.Attributes Namespace Reference

Source: https://docs.atas.net/en/namespaceOFT_1_1Attributes.html

| Namespaces | |
| --- | --- |
| namespace | [Editors](./namespaceOFT_1_1Attributes_1_1Editors.md) |
| | |
| namespace | [Properties](./namespaceOFT_1_1Attributes_1_1Properties.md) |
| | |

| Classes | |
| --- | --- |
| class | AttributeResourceHelpers |
| | |
| class | Extensions |
| | |
| class | [FeatureIdAttribute](../classes/classOFT_1_1Attributes_1_1FeatureIdAttribute.md) |
| | |
| class | [HelpLinkAttribute](../classes/classOFT_1_1Attributes_1_1HelpLinkAttribute.md) |
| | |
| class | [IgnoreCloneAttribute](../classes/classOFT_1_1Attributes_1_1IgnoreCloneAttribute.md) |
| | Ignore clone property or field. [More...](../classes/classOFT_1_1Attributes_1_1IgnoreCloneAttribute.md#details) |
| | |
| class | [LogoAttribute](../classes/classOFT_1_1Attributes_1_1LogoAttribute.md) |
| | |
| class | [MappingAttribute](../classes/classOFT_1_1Attributes_1_1MappingAttribute.md) |
| | |
| class | [ParameterAttribute](../classes/classOFT_1_1Attributes_1_1ParameterAttribute.md) |
| | |
| class | [ReferralLinkAttribute](../classes/classOFT_1_1Attributes_1_1ReferralLinkAttribute.md) |
| | |
| class | [RegisterLinkAttribute](../classes/classOFT_1_1Attributes_1_1RegisterLinkAttribute.md) |
| | |
| class | [SupportedExchangesAttribute](../classes/classOFT_1_1Attributes_1_1SupportedExchangesAttribute.md) |
| | Specifies the exchange codes supported by a connector. The attribute accepts a type containing public const string fields, each representing a supported exchange code. [More...](../classes/classOFT_1_1Attributes_1_1SupportedExchangesAttribute.md#details) |
| | |
| class | [TabAttribute](../classes/classOFT_1_1Attributes_1_1TabAttribute.md) |
| | Attribute that assigns a property or all properties of a class to a named tab in the settings UI. Use alongside System.ComponentModel.DataAnnotations.DisplayAttribute for property display metadata (Name, GroupName, Description, Order). [More...](../classes/classOFT_1_1Attributes_1_1TabAttribute.md#details) |
| | |
| class | [VisibleWhenAttribute](../classes/classOFT_1_1Attributes_1_1VisibleWhenAttribute.md) |
| | Makes the property visible only when the specified source property's value matches one of the provided values. Used for dynamic property visibility in the settings UI (e.g., showing/hiding properties based on a mode enum). [More...](../classes/classOFT_1_1Attributes_1_1VisibleWhenAttribute.md#details) |
| | |

| Enumerations | |
| --- | --- |
| enum | [DataSeriesTypes](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fe) { [Band](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fea67fd95fc1e88f15b3efb9feef0fc0dc9)
, [Line](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fea4803e6b9e63dabf04de980788d6a13c4)
, [Candle](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18feae966b2e6caa2b8a2f49802e3baf6fbf2)
, [Value](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fea689202409e48743b914713f96d93947c)
 } |
| | Enumerates the types of data series available for display on a chart. [More...](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fe) |
| | |

| Functions | |
| --- | --- |
| class | [ModuleHelpLinkAttribute](./namespaceOFT_1_1Attributes.md#a1ac2a514142d8fac54b80d8030916301) (string url, string moduleName) |
| | |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)DataSeriesTypes

| enum [OFT.Attributes.DataSeriesTypes](./namespaceOFT_1_1Attributes.md#a82893d7864c6f810b66e876c518d18fe) |
| --- |

Enumerates the types of data series available for display on a chart.

| Enumerator | |
| --- | --- |
| Band | Represents a band data series. |
| Line | Represents a line data series. |
| Candle | Represents a candle data series. |
| Value | Represents a value data series. |

## Function Documentation

## [◆](https://docs.atas.net/en/)ModuleHelpLinkAttribute()

| class OFT.Attributes.ModuleHelpLinkAttribute | ( | string | url, |
| --- | --- | --- | --- |
| | | string | moduleName |
| | ) | | |
