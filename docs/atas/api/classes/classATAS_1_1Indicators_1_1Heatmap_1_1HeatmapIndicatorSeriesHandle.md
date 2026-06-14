# ATAS.Indicators.Heatmap.HeatmapIndicatorSeriesHandle< TValue > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.html

Strongly typed handle for a series within a visual. Returned from HeatmapIndicatorVisualHandle.Series<TValue>(string, HeatmapIndicatorSeriesRole, HeatmapIndicatorValueKind, System.Func<TValue, decimal>, HeatmapIndicatorVisualStyle?, string?, string?); the constructor is internal so authors cannot fabricate one. TValue is the indicator-internal sample type — what the indicator's calculation produces. The handle also carries the projection that converts the typed sample to a renderer-facing decimal; the lease applies the projection inline on every Append so the chunked storage holds decimal samples ready for the renderer.
 [More...](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md#details)

## Detailed Description

Strongly typed handle for a series within a visual. Returned from HeatmapIndicatorVisualHandle.Series<TValue>(string, HeatmapIndicatorSeriesRole, HeatmapIndicatorValueKind, System.Func<TValue, decimal>, HeatmapIndicatorVisualStyle?, string?, string?); the constructor is internal so authors cannot fabricate one. TValue is the indicator-internal sample type — what the indicator's calculation produces. The handle also carries the projection that converts the typed sample to a renderer-facing decimal; the lease applies the projection inline on every Append so the chunked storage holds decimal samples ready for the renderer.

The documentation for this class was generated from the following file:
- [HeatmapIndicatorSeriesHandle.cs](../files/HeatmapIndicatorSeriesHandle_8cs.md)
