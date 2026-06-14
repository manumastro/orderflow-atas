# IHeatmapIndicatorRenderSource.cs File Reference

Source: https://docs.atas.net/en/IHeatmapIndicatorRenderSource_8cs.html

| Classes | |
| --- | --- |
| interface | [ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderInstance](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md) |
| | Read-only renderer view of one live indicator instance owned by the heatmap controller. The renderer (Skia / Vulkan overlay layer) consumes this surface to walk every visible instance and read its State per frame; the controller's richer `IHeatmapIndicatorInstance` is host / view-model facing and lives in a higher-level project the renderer cannot reference. Splitting the renderer-only surface here keeps the dependency direction acyclic (renderer -> indicators -> rendering primitives). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderSource](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md) |
| | Renderer-facing handle on the live indicator set. Implemented by the platform's heatmap controller (which already owns instance lifecycle and the live IHeatmapIndicatorContext) and consumed by the renderer overlay. Replaces the v1 `HeatmapIndicatorsSnapshot` pull model: the renderer enumerates Instances directly and pulls per-instance state via IHeatmapIndicatorRenderInstance.State. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |
