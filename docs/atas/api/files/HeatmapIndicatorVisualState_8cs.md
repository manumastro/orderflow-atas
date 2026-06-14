# HeatmapIndicatorVisualState.cs File Reference

Source: https://docs.atas.net/en/HeatmapIndicatorVisualState_8cs.html

| Classes | |
| --- | --- |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorVisualState |
| | Concrete implementation of IHeatmapVisualState. Holds two node trees built once from the descriptor: a back tree the indicator mutates under a lease and a front tree the renderer reads. The two trees are otherwise structurally identical and share the same per-series HeatmapIndicatorChunkedSeriesStorage instances by reference, so large sample chains live in exactly one place — only small node-level fields (style, presentation) are duplicated. |
| | |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorVisualNode |
| | |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorSeriesNode |
| | |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorVisualStateLease |
| | |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorVisualLease |
| | |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorSeriesLease |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap.Internal](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap_1_1Internal.md) |
| | |
