# HeatmapIndicatorSeriesHandle.cs File Reference

Source: https://docs.atas.net/en/HeatmapIndicatorSeriesHandle_8cs.html

| Classes | |
| --- | --- |
| class | [ATAS.Indicators.Heatmap.HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) |
| | Strongly typed handle for a series within a visual. Returned from HeatmapIndicatorVisualHandle.Series(string, HeatmapIndicatorSeriesRole, HeatmapIndicatorValueKind, System.Func, HeatmapIndicatorVisualStyle?, string?, string?); the constructor is internal so authors cannot fabricate one. TValue is the indicator-internal sample type — what the indicator's calculation produces. The handle also carries the projection that converts the typed sample to a renderer-facing decimal; the lease applies the projection inline on every Append so the chunked storage holds decimal samples ready for the renderer. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |
