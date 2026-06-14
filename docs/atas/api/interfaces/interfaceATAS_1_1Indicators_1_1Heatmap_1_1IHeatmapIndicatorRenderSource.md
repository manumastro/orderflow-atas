# ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderSource Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.html

Renderer-facing handle on the live indicator set. Implemented by the platform's heatmap controller (which already owns instance lifecycle and the live IHeatmapIndicatorContext) and consumed by the renderer overlay. Replaces the v1 `HeatmapIndicatorsSnapshot` pull model: the renderer enumerates Instances directly and pulls per-instance state via IHeatmapIndicatorRenderInstance.State.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md#details)

| Properties | |
| --- | --- |
| IReadOnlyList | [Instances](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md#aac82896f002e491da67059c974113a5e)`[get]` |
| | Snapshot of the currently registered instances. Implementations may return a freshly-allocated list per call (cheap; the renderer caches per-instance state across frames) — callers must not assume identity stability on the list itself, but instance references inside the list are stable for the lifetime of the runner. |
| | |
| [IHeatmapIndicatorContext](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) | [Context](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md#ab89eb89f53d71b19439228a8faf6ef68)`[get]` |
| | Live read-only view of the heatmap context (instrument metadata, working range, viewport hints). The same reference is observed by every indicator hosted by the controller. The renderer typically only reads `Viewport`; mutators live on the controller. |
| | |

## Detailed Description

Renderer-facing handle on the live indicator set. Implemented by the platform's heatmap controller (which already owns instance lifecycle and the live IHeatmapIndicatorContext) and consumed by the renderer overlay. Replaces the v1 `HeatmapIndicatorsSnapshot` pull model: the renderer enumerates Instances directly and pulls per-instance state via IHeatmapIndicatorRenderInstance.State.

## Property Documentation

## [◆](https://docs.atas.net/en/)Context

| [IHeatmapIndicatorContext](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderSource.Context |
| --- |

get

Live read-only view of the heatmap context (instrument metadata, working range, viewport hints). The same reference is observed by every indicator hosted by the controller. The renderer typically only reads `Viewport`; mutators live on the controller.

## [◆](https://docs.atas.net/en/)Instances

| IReadOnlyList ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderSource.Instances |
| --- |

get

Snapshot of the currently registered instances. Implementations may return a freshly-allocated list per call (cheap; the renderer caches per-instance state across frames) — callers must not assume identity stability on the list itself, but instance references inside the list are stable for the lifetime of the runner.

The documentation for this interface was generated from the following file:
- [IHeatmapIndicatorRenderSource.cs](../files/IHeatmapIndicatorRenderSource_8cs.md)
