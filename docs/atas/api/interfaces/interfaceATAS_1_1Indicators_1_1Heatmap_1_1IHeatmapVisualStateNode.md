# ATAS.Indicators.Heatmap.IHeatmapVisualStateNode Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.html

Read-only handle to a visual within an IHeatmapVisualState. Carries the descriptor metadata plus the list of series; mutation goes through the lease (IHeatmapVisualStateLease.Visual).
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapVisualStateNode:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| string | [VisualId](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#acd7a48af0871b5b3a16fbe8c463ac736)`[get]` |
| | Stable visual id, matches the descriptor entry. |
| | |
| HeatmapIndicatorVisualDescriptor | [Descriptor](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#a095ecdce98e78154bcb15834f31a268e)`[get]` |
| | The descriptor for this visual — kind, default style, default presentation. |
| | |
| HeatmapIndicatorVisualStyle? | [Style](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#ab5addb69e0c29ae8f0d4217e1aa906ed)`[get]` |
| | Active visual style. Defaults to HeatmapIndicatorVisualDescriptor.DefaultStyle; mutated through IHeatmapVisualLease.Style. Renderer reads with `Volatile.Read` semantics; a style-only change still commits via the lease. |
| | |
| HeatmapIndicatorVisualPresentation? | [Presentation](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#aba0e4334ad7d27def3b7fee6fc746110)`[get]` |
| | Active layout / presentation hints (panel height, scale, threshold). Defaults to HeatmapIndicatorVisualDescriptor.DefaultPresentation; mutated through IHeatmapVisualLease.Presentation. |
| | |
| IReadOnlyList | [Series](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#a04b86e46d61a84da0726931004569e6d)`[get]` |
| | The series declared by the descriptor. Schema is fixed; collection never changes shape. |
| | |

## Detailed Description

Read-only handle to a visual within an IHeatmapVisualState. Carries the descriptor metadata plus the list of series; mutation goes through the lease (IHeatmapVisualStateLease.Visual).

## Property Documentation

## [◆](https://docs.atas.net/en/)Descriptor

| HeatmapIndicatorVisualDescriptor ATAS.Indicators.Heatmap.IHeatmapVisualStateNode.Descriptor |
| --- |

get

The descriptor for this visual — kind, default style, default presentation.

## [◆](https://docs.atas.net/en/)Presentation

| HeatmapIndicatorVisualPresentation? ATAS.Indicators.Heatmap.IHeatmapVisualStateNode.Presentation |
| --- |

get

Active layout / presentation hints (panel height, scale, threshold). Defaults to HeatmapIndicatorVisualDescriptor.DefaultPresentation; mutated through IHeatmapVisualLease.Presentation.

## [◆](https://docs.atas.net/en/)Series

| IReadOnlyList ATAS.Indicators.Heatmap.IHeatmapVisualStateNode.Series |
| --- |

get

The series declared by the descriptor. Schema is fixed; collection never changes shape.

## [◆](https://docs.atas.net/en/)Style

| HeatmapIndicatorVisualStyle? ATAS.Indicators.Heatmap.IHeatmapVisualStateNode.Style |
| --- |

get

Active visual style. Defaults to HeatmapIndicatorVisualDescriptor.DefaultStyle; mutated through IHeatmapVisualLease.Style. Renderer reads with `Volatile.Read` semantics; a style-only change still commits via the lease.

## [◆](https://docs.atas.net/en/)VisualId

| string ATAS.Indicators.Heatmap.IHeatmapVisualStateNode.VisualId |
| --- |

get

Stable visual id, matches the descriptor entry.

The documentation for this interface was generated from the following file:
- [IHeatmapVisualState.cs](../files/IHeatmapVisualState_8cs.md)
