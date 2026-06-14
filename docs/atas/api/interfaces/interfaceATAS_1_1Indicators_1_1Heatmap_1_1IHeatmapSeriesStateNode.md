# ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.html

Read-only handle to a series within an IHeatmapVisualStateNode. The renderer iterates committed samples through this interface; mutation goes through the lease (IHeatmapVisualLease.Series<TValue>).
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| IEnumerable | [GetCommittedSamples](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#a22fff9fe7755352698bb2e8d86e6986a) () |
| | Wait-free, forward-only enumeration of the front-committed samples in arrival order. The enumeration captures the committed snapshot at the moment of the call; subsequent commits do not affect an in-flight enumerator. Safe to call from any thread (typically the renderer at frame rate). Returns an empty sequence when no samples have been committed yet. The returned enumerable does NOT allocate beyond the implementation's iterator state — implementers should avoid materialising a list and may return a struct enumerator wrapped through IEnumerable. |
| | |

| Properties | |
| --- | --- |
| string | [SeriesId](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#ab8234704239e58f72c846b81f0ed0cbc)`[get]` |
| | Stable series id, matches the descriptor entry. |
| | |
| HeatmapIndicatorSeriesDescriptor | [Descriptor](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#a2f659c45f1f3ac1381f46d25f51b9dac)`[get]` |
| | Series-level descriptor — role, value kind, metric id, unit, default style. |
| | |
| HeatmapIndicatorVisualStyle? | [Style](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#aaaf8159e0b0ab913d0e710608e2d3c7b)`[get]` |
| | Active per-series style. Defaults to HeatmapIndicatorSeriesDescriptor.DefaultStyle; when null both the descriptor default and any visual-level style apply per renderer convention. |
| | |
| int | [CommittedCount](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#a600e683963688ad6c077926adae8bb23)`[get]` |
| | Number of samples committed to the front and visible to the renderer. |
| | |
| long | [LastCommittedTimestampNanos](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#ab1122339b17fe20caf2cfb9ed91262f0)`[get]` |
| | Most recent committed sample timestamp, or 0 if empty. |
| | |

## Detailed Description

Read-only handle to a series within an IHeatmapVisualStateNode. The renderer iterates committed samples through this interface; mutation goes through the lease (IHeatmapVisualLease.Series<TValue>).

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetCommittedSamples()

| IEnumerable ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode.GetCommittedSamples | ( | | ) | |
| --- | --- | --- | --- | --- |

Wait-free, forward-only enumeration of the front-committed samples in arrival order. The enumeration captures the committed snapshot at the moment of the call; subsequent commits do not affect an in-flight enumerator. Safe to call from any thread (typically the renderer at frame rate). Returns an empty sequence when no samples have been committed yet. The returned enumerable does NOT allocate beyond the implementation's iterator state — implementers should avoid materialising a list and may return a struct enumerator wrapped through IEnumerable<T>.

## Property Documentation

## [◆](https://docs.atas.net/en/)CommittedCount

| int ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode.CommittedCount |
| --- |

get

Number of samples committed to the front and visible to the renderer.

## [◆](https://docs.atas.net/en/)Descriptor

| HeatmapIndicatorSeriesDescriptor ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode.Descriptor |
| --- |

get

Series-level descriptor — role, value kind, metric id, unit, default style.

## [◆](https://docs.atas.net/en/)LastCommittedTimestampNanos

| long ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode.LastCommittedTimestampNanos |
| --- |

get

Most recent committed sample timestamp, or 0 if empty.

## [◆](https://docs.atas.net/en/)SeriesId

| string ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode.SeriesId |
| --- |

get

Stable series id, matches the descriptor entry.

## [◆](https://docs.atas.net/en/)Style

| HeatmapIndicatorVisualStyle? ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode.Style |
| --- |

get

Active per-series style. Defaults to HeatmapIndicatorSeriesDescriptor.DefaultStyle; when null both the descriptor default and any visual-level style apply per renderer convention.

The documentation for this interface was generated from the following file:
- [IHeatmapVisualState.cs](../files/IHeatmapVisualState_8cs.md)
