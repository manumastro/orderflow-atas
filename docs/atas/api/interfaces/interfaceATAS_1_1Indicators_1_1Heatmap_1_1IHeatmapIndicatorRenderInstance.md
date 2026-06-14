# ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderInstance Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.html

Read-only renderer view of one live indicator instance owned by the heatmap controller. The renderer (Skia / Vulkan overlay layer) consumes this surface to walk every visible instance and read its State per frame; the controller's richer `IHeatmapIndicatorInstance` is host / view-model facing and lives in a higher-level project the renderer cannot reference. Splitting the renderer-only surface here keeps the dependency direction acyclic (renderer -> indicators -> rendering primitives).
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md#details)

| Properties | |
| --- | --- |
| string | [InstanceId](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md#a1288c801ee46ba1efe8c18694f2ff3f2)`[get]` |
| | Stable per-instance id, matches the controller's instance id. |
| | |
| string | [TypeId](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md#a7579bb12cee44a7d454ff32ef0143f6e)`[get]` |
| | Indicator type id, matches `HeatmapIndicatorDescriptor.IndicatorId`. |
| | |
| bool | [IsVisible](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md#a82258af3ef6c86c330747c442d3d9def)`[get]` |
| | Whether the host considers this instance visible. Hidden instances are retained in the controller (so settings / health survive a toggle) but the renderer must skip them entirely — equivalent to omitting them from the v1 aggregate snapshot. |
| | |
| [IHeatmapVisualState](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) | [State](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md#ac028372877a45db2ec088dc428fe8b1f)`[get]` |
| | Persistent visual state owned by the indicator instance. The renderer reads IHeatmapVisualState.Version per frame to decide whether GPU-side caches need rebuilding; the descriptor / visuals / series tree stays referentially stable for the instance's lifetime. |
| | |

## Detailed Description

Read-only renderer view of one live indicator instance owned by the heatmap controller. The renderer (Skia / Vulkan overlay layer) consumes this surface to walk every visible instance and read its State per frame; the controller's richer `IHeatmapIndicatorInstance` is host / view-model facing and lives in a higher-level project the renderer cannot reference. Splitting the renderer-only surface here keeps the dependency direction acyclic (renderer -> indicators -> rendering primitives).

## Property Documentation

## [◆](https://docs.atas.net/en/)InstanceId

| string ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderInstance.InstanceId |
| --- |

get

Stable per-instance id, matches the controller's instance id.

## [◆](https://docs.atas.net/en/)IsVisible

| bool ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderInstance.IsVisible |
| --- |

get

Whether the host considers this instance visible. Hidden instances are retained in the controller (so settings / health survive a toggle) but the renderer must skip them entirely — equivalent to omitting them from the v1 aggregate snapshot.

## [◆](https://docs.atas.net/en/)State

| [IHeatmapVisualState](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderInstance.State |
| --- |

get

Persistent visual state owned by the indicator instance. The renderer reads IHeatmapVisualState.Version per frame to decide whether GPU-side caches need rebuilding; the descriptor / visuals / series tree stays referentially stable for the instance's lifetime.

## [◆](https://docs.atas.net/en/)TypeId

| string ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderInstance.TypeId |
| --- |

get

Indicator type id, matches `HeatmapIndicatorDescriptor.IndicatorId`.

The documentation for this interface was generated from the following file:
- [IHeatmapIndicatorRenderSource.cs](../files/IHeatmapIndicatorRenderSource_8cs.md)
