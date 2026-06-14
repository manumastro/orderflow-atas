# ATAS.Indicators.Heatmap.HeatmapIndicatorVisualHandle Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.html

Strongly typed handle for a visual added to a descriptor via HeatmapIndicatorDescriptorBuilder. The handle captures the owning descriptor's identity so the state builder can reject handles from a different descriptor at runtime, and the constructor is internal so authors cannot fabricate handles by hand.
 [More...](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md#details)

| Public Member Functions | |
| --- | --- |
| [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) | [Series](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md#a541f9e70a35dfd78868a712695281044) (string seriesId, HeatmapIndicatorSeriesRole role, HeatmapIndicatorValueKind valueKind, Func valueProjection, HeatmapIndicatorVisualStyle? defaultStyle=null, string? metricId=null, string? unit=null) |
| | Add a typed series to this visual. TValue is the indicator-internal sample type — the type the indicator computes (e.g. `HeatmapPriceLineSample`, `HeatmapValueAreaSample`, custom records). Each Append on the lease projects the typed value to the renderer-facing decimal via valueProjection . |
| | |
| [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) | [Series](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md#a0d010531237fb024e849b7a10f1c490a) (string seriesId, HeatmapIndicatorSeriesRole role, HeatmapIndicatorValueKind valueKind, HeatmapIndicatorVisualStyle? defaultStyle=null, string? metricId=null, string? unit=null) |
| | Decimal fast path: the series stores decimal samples and no projection is required. Equivalent to the generic overload with the identity projection, but avoids the delegate. |
| | |

## Detailed Description

Strongly typed handle for a visual added to a descriptor via HeatmapIndicatorDescriptorBuilder. The handle captures the owning descriptor's identity so the state builder can reject handles from a different descriptor at runtime, and the constructor is internal so authors cannot fabricate handles by hand.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Series()

| [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorVisualHandle.Series | ( | string | seriesId, |
| --- | --- | --- | --- |
| | | HeatmapIndicatorSeriesRole | role, |
| | | HeatmapIndicatorValueKind | valueKind, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null`, |
| | | string? | metricId = `null`, |
| | | string? | unit = `null` |
| | ) | | |

Decimal fast path: the series stores decimal samples and no projection is required. Equivalent to the generic overload with the identity projection, but avoids the delegate.

## [◆](https://docs.atas.net/en/)Series()

| [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) [ATAS.Indicators.Heatmap.HeatmapIndicatorVisualHandle.Series](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md#a0d010531237fb024e849b7a10f1c490a) | ( | string | seriesId, |
| --- | --- | --- | --- |
| | | HeatmapIndicatorSeriesRole | role, |
| | | HeatmapIndicatorValueKind | valueKind, |
| | | Func | valueProjection, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null`, |
| | | string? | metricId = `null`, |
| | | string? | unit = `null` |
| | ) | | |

Add a typed series to this visual. TValue is the indicator-internal sample type — the type the indicator computes (e.g. `HeatmapPriceLineSample`, `HeatmapValueAreaSample`, custom records). Each Append on the lease projects the typed value to the renderer-facing decimal via valueProjection .

For series whose values are already `decimal`, prefer the no-projection overload Series(string, HeatmapIndicatorSeriesRole, HeatmapIndicatorValueKind, HeatmapIndicatorVisualStyle?, string?, string?) — the projection is implicitly identity and you avoid one delegate hop per Append.

The documentation for this class was generated from the following file:
- [HeatmapIndicatorVisualHandle.cs](../files/HeatmapIndicatorVisualHandle_8cs.md)
