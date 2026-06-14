# ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.html

Marks a class as a heatmap indicator type and supplies discovery metadata. Apply to a concrete class that derives from HeatmapIndicator<TSettings> (or implements IHeatmapIndicator directly).
 [More...](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [HeatmapIndicatorAttribute](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#ac5eb142e405c5c292190a2e5f1f52549) (string id, string? displayName=null) |
| | |

| Properties | |
| --- | --- |
| string | [Id](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#a9e02a70b91dd1801b47f92075114beb0)`[get]` |
| | Stable type identifier, e.g. "heatmap.ohlc-plus". Convention: "." using lowercase-with-dots. Must be unique within all assemblies scanned by discovery. |
| | |
| string? | [DisplayName](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#a5e1233756ad8a466d1a432bf38bc3543)`[get]` |
| | Localisable display name. If omitted, the class name is used. |
| | |
| Type? | [ResourceType](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#ac1c53b7d95c1b7b4cb8e65f9153c0126)`[get]` |
| | Localisation resource type for DisplayName / Description. |
| | |
| string? | [DisplayNameKey](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#aead2406a6450a37d59cf67b9fc0f2506)`[get]` |
| | Localisation key for DisplayName when ResourceType is set. |
| | |
| string? | [DescriptionKey](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#ae0e5357b081c4d4a00ce8f5841c185cb)`[get]` |
| | Localisation key for Description. |
| | |
| string? | [HelpLink](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#a7a814cbb8eef0714a094ce353876f671)`[get]` |
| | Optional documentation URL shown in the editor. |
| | |

## Detailed Description

Marks a class as a heatmap indicator type and supplies discovery metadata. Apply to a concrete class that derives from HeatmapIndicator<TSettings> (or implements IHeatmapIndicator directly).

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)HeatmapIndicatorAttribute()

| ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute.HeatmapIndicatorAttribute | ( | string | id, |
| --- | --- | --- | --- |
| | | string? | displayName = `null` |
| | ) | | |

## Property Documentation

## [◆](https://docs.atas.net/en/)DescriptionKey

| string? ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute.DescriptionKey |
| --- |

get

Localisation key for Description.

## [◆](https://docs.atas.net/en/)DisplayName

| string? ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute.DisplayName |
| --- |

get

Localisable display name. If omitted, the class name is used.

## [◆](https://docs.atas.net/en/)DisplayNameKey

| string? ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute.DisplayNameKey |
| --- |

get

Localisation key for DisplayName when ResourceType is set.

## [◆](https://docs.atas.net/en/)HelpLink

| string? ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute.HelpLink |
| --- |

get

Optional documentation URL shown in the editor.

## [◆](https://docs.atas.net/en/)Id

| string ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute.Id |
| --- |

get

Stable type identifier, e.g. "heatmap.ohlc-plus". Convention: "<vendor>.<indicator-name>" using lowercase-with-dots. Must be unique within all assemblies scanned by discovery.

## [◆](https://docs.atas.net/en/)ResourceType

| Type? ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute.ResourceType |
| --- |

get

Localisation resource type for DisplayName / Description.

The documentation for this class was generated from the following file:
- [HeatmapIndicatorAttribute.cs](../files/HeatmapIndicatorAttribute_8cs.md)
