# IHeatmapIndicatorContext.cs File Reference

Source: https://docs.atas.net/en/IHeatmapIndicatorContext_8cs.html

| Classes | |
| --- | --- |
| interface | [ATAS.Indicators.Heatmap.IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) |
| | Live runtime context exposed to a heatmap indicator. Unlike the v1 `HeatmapIndicatorContext` record (snapshot at reset), this is a read-only interface whose properties reflect the host's current state at every read. Indicators query it on demand — there are no "context changed" events because the most volatile field (Viewport) updates every render frame and a cross-thread notification per change would dominate the cost. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |

| Functions | |
| --- | --- |
| readonly record struct | [ATAS.Indicators.Heatmap.HeatmapViewport](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#a209281643599139f1d052d3e7be8e646) (long StartTimeNanos, long EndTimeNanos, int PixelWidth) |
| | A rectangular slice of the heatmap timeline currently visible to the user. Carried by IHeatmapIndicatorContext.Viewport. PixelWidth is the rendered horizontal extent in physical pixels (DPI-corrected) — useful for choosing decimation step. |
| | |
