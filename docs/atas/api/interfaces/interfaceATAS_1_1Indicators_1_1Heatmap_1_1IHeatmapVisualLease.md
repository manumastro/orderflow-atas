# ATAS.Indicators.Heatmap.IHeatmapVisualLease Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.html

Per-visual mutation surface inside the lease. Style and presentation are mutable properties on the lease; series content is mutated through Series<TValue>.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapVisualLease:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) | [Series](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md#a7d4e8ddc124c804c668ddec0c76424a6) ([HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) handle) |
| | Address a series for mutation. Throws if the supplied handle does not belong to this visual. |
| | |

| Properties | |
| --- | --- |
| HeatmapIndicatorVisualStyle? | [Style](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md#a3dbdd0767c8180802cb1d32799b49f61)`[get, set]` |
| | Set the active visual style. Setting `null` falls back to the descriptor default. |
| | |
| HeatmapIndicatorVisualPresentation? | [Presentation](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md#a834cb3ca6aab654a35b303250dd09b83)`[get, set]` |
| | Set the active presentation hints. Setting `null` falls back to the descriptor default. |
| | |

## Detailed Description

Per-visual mutation surface inside the lease. Style and presentation are mutable properties on the lease; series content is mutated through Series<TValue>.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Series()

| [IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) ATAS.Indicators.Heatmap.IHeatmapVisualLease.Series | ( | [HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) | handle | ) | |
| --- | --- | --- | --- | --- | --- |

Address a series for mutation. Throws if the supplied handle does not belong to this visual.

## Property Documentation

## [◆](https://docs.atas.net/en/)Presentation

| HeatmapIndicatorVisualPresentation? ATAS.Indicators.Heatmap.IHeatmapVisualLease.Presentation |
| --- |

getset

Set the active presentation hints. Setting `null` falls back to the descriptor default.

## [◆](https://docs.atas.net/en/)Style

| HeatmapIndicatorVisualStyle? ATAS.Indicators.Heatmap.IHeatmapVisualLease.Style |
| --- |

getset

Set the active visual style. Setting `null` falls back to the descriptor default.

The documentation for this interface was generated from the following file:
- [IHeatmapVisualState.cs](../files/IHeatmapVisualState_8cs.md)
