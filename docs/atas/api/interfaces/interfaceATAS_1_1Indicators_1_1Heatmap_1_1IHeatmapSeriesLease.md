# ATAS.Indicators.Heatmap.IHeatmapSeriesLease< TValue > Interface Template Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.html

Per-series mutation surface inside the lease. Append, replace, clear, and trim operations are buffered to the back-stage and become visible to the renderer on lease disposal.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapSeriesLease< TValue >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| bool | [Append](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#a7c7c2464f9a2e0b8153dd30ee50943fc) (long timestampNanos, TValue value) |
| | Append one sample to the back-stage. Returns `true` when stored, `false` when discarded because timestampNanos is older than the latest committed-or-staged sample. Equal-timestamp appends replace the latest staged sample (or the latest committed one if no staged samples yet). |
| | |
| bool | [Append](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#a9cbc27910d5db4312469daacfd181bed) (in HeatmapIndicatorSample sample) |
| | Append a pre-built sample by reference. Equivalent to Append(long,TValue) with the sample fields, but avoids destructuring + reconstructing the struct when the indicator already holds a HeatmapIndicatorSample (e.g. from warm-up payload or a calculation that returned the sample shape). |
| | |
| int | [AppendRange](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#a9f513dbdc60be676518612c15eca0c6b) (ReadOnlySpan > samples) |
| | Append a span of samples; same ordering rules as Append(long,TValue). Returns the number of samples accepted. |
| | |
| void | [Clear](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#a9f54c96f13b26a1ce505847d309bda38) () |
| | Drop every sample in this series. After commit, IHeatmapSeriesStateNode.CommittedCount reads as zero. Combined with subsequent appends in the same lease this is the canonical "rebuild" pattern for indicator-initiated recalculation (see design doc §5.2). |
| | |
| int | [TrimBefore](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#a771513c9e948990d36d998ebd037d4c4) (long timestampNanos) |
| | Evict every committed sample whose timestamp

## Detailed Description

Per-series mutation surface inside the lease. Append, replace, clear, and trim operations are buffered to the back-stage and become visible to the renderer on lease disposal.

Template Parameters

| TValue | Indicator-internal sample type. |
| --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Append() [1/2]

| bool [ATAS.Indicators.Heatmap.IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md).Append | ( | in HeatmapIndicatorSample | sample | ) | |
| --- | --- | --- | --- | --- | --- |

Append a pre-built sample by reference. Equivalent to Append(long,TValue) with the sample fields, but avoids destructuring + reconstructing the struct when the indicator already holds a HeatmapIndicatorSample<TValue> (e.g. from warm-up payload or a calculation that returned the sample shape).

## [◆](https://docs.atas.net/en/)Append() [2/2]

| bool [ATAS.Indicators.Heatmap.IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md).Append | ( | long | timestampNanos, |
| --- | --- | --- | --- |
| | | TValue | value |
| | ) | | |

Append one sample to the back-stage. Returns `true` when stored, `false` when discarded because timestampNanos is older than the latest committed-or-staged sample. Equal-timestamp appends replace the latest staged sample (or the latest committed one if no staged samples yet).

## [◆](https://docs.atas.net/en/)AppendRange()

| int [ATAS.Indicators.Heatmap.IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md).AppendRange | ( | ReadOnlySpan > | samples | ) | |
| --- | --- | --- | --- | --- | --- |

Append a span of samples; same ordering rules as Append(long,TValue). Returns the number of samples accepted.

## [◆](https://docs.atas.net/en/)Clear()

| void [ATAS.Indicators.Heatmap.IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md).Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

Drop every sample in this series. After commit, IHeatmapSeriesStateNode.CommittedCount reads as zero. Combined with subsequent appends in the same lease this is the canonical "rebuild" pattern for indicator-initiated recalculation (see design doc §5.2).

## [◆](https://docs.atas.net/en/)TrimBefore()

| int [ATAS.Indicators.Heatmap.IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md).TrimBefore | ( | long | timestampNanos | ) | |
| --- | --- | --- | --- | --- | --- |

Evict every committed sample whose timestamp < timestampNanos . Returns the number of samples evicted. Useful at session boundaries to bound memory.

## Property Documentation

## [◆](https://docs.atas.net/en/)Style

| HeatmapIndicatorVisualStyle? [ATAS.Indicators.Heatmap.IHeatmapSeriesLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md).Style |
| --- |

getset

Per-series style override; null falls back to the descriptor default.

The documentation for this interface was generated from the following file:
- [IHeatmapVisualState.cs](../files/IHeatmapVisualState_8cs.md)
