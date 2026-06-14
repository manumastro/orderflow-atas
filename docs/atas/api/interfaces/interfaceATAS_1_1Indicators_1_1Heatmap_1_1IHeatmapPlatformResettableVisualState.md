# ATAS.Indicators.Heatmap.IHeatmapPlatformResettableVisualState Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.html

Platform-owned extension for clearing an indicator state without going through an author-visible update lease. Indicator authors should use IHeatmapVisualState.BeginUpdate instead.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapPlatformResettableVisualState:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [ResetForPlatform](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md#a1b78d0088d541f3f7727d4543420fcea) () |
| | Clear all committed and staged samples, reset visual overrides, and invalidate renderers. |
| | |

## Detailed Description

Platform-owned extension for clearing an indicator state without going through an author-visible update lease. Indicator authors should use IHeatmapVisualState.BeginUpdate instead.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ResetForPlatform()

| void ATAS.Indicators.Heatmap.IHeatmapPlatformResettableVisualState.ResetForPlatform | ( | | ) | |
| --- | --- | --- | --- | --- |

Clear all committed and staged samples, reset visual overrides, and invalidate renderers.

The documentation for this interface was generated from the following file:
- [IHeatmapVisualState.cs](../files/IHeatmapVisualState_8cs.md)
