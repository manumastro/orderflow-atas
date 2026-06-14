# ATAS.Indicators.Heatmap.IHeatmapVisualStateLease Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.html

The exclusive write lease on an IHeatmapVisualState. Acquired via IHeatmapVisualState.BeginUpdate; disposing commits the back-stage to the front. A lease can only be used inside the calling stack frame — passing it across `await` points or to background tasks is a misuse pattern (the platform serialises every indicator callback on a single task, so all legitimate writes happen from inside one of those callbacks).
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapVisualStateLease:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Heatmap.IHeatmapVisualStateLease:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [IHeatmapVisualLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) | [Visual](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md#a9f4e00a99eb11f04f6a4092c449cff08) ([HeatmapIndicatorVisualHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) handle) |
| | Address a visual for mutation. Throws if the supplied handle does not belong to this state's descriptor. |
| | |
| [IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) | [Series](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md#aaeaada9e428310b5705ea2bc68b15428) ([HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) handle) |
| | Direct series-access shortcut. Equivalent to `Visual(handle.Visual).Series(handle)` but skips the per-call dictionary lookup for the visual lease wrapper. Use when the indicator holds the series handle directly (typical reference pattern: one `static readonly` series handle per metric, captured at descriptor build time). Style/Presentation overrides still go through Visual(HeatmapIndicatorVisualHandle). |
| | |

## Detailed Description

The exclusive write lease on an IHeatmapVisualState. Acquired via IHeatmapVisualState.BeginUpdate; disposing commits the back-stage to the front. A lease can only be used inside the calling stack frame — passing it across `await` points or to background tasks is a misuse pattern (the platform serialises every indicator callback on a single task, so all legitimate writes happen from inside one of those callbacks).

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Series()

| [IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) ATAS.Indicators.Heatmap.IHeatmapVisualStateLease.Series | ( | [HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) | handle | ) | |
| --- | --- | --- | --- | --- | --- |

Direct series-access shortcut. Equivalent to `Visual(handle.Visual).Series(handle)` but skips the per-call dictionary lookup for the visual lease wrapper. Use when the indicator holds the series handle directly (typical reference pattern: one `static readonly` series handle per metric, captured at descriptor build time). Style/Presentation overrides still go through Visual(HeatmapIndicatorVisualHandle).

## [◆](https://docs.atas.net/en/)Visual()

| [IHeatmapVisualLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) ATAS.Indicators.Heatmap.IHeatmapVisualStateLease.Visual | ( | [HeatmapIndicatorVisualHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | handle | ) | |
| --- | --- | --- | --- | --- | --- |

Address a visual for mutation. Throws if the supplied handle does not belong to this state's descriptor.

The documentation for this interface was generated from the following file:
- [IHeatmapVisualState.cs](../files/IHeatmapVisualState_8cs.md)
