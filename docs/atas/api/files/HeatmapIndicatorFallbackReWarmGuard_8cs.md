# HeatmapIndicatorFallbackReWarmGuard.cs File Reference

Source: https://docs.atas.net/en/HeatmapIndicatorFallbackReWarmGuard_8cs.html

| Classes | |
| --- | --- |
| class | [ATAS.Indicators.Heatmap.HeatmapIndicatorFallbackReWarmGuard](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md) |
| | State holder for indicators whose calculation is anchored at the real data start (e.g. CVD `FromDataStart`, VWAP `FromDataStart`) and may receive a fallback-range warm-up before the host knows the real data start. Encapsulates the latch protocol so the indicator does not have to track three flags and an inline check by hand. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |
