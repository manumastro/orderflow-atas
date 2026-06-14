# ATAS.Indicators.Heatmap.Internal Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1Indicators_1_1Heatmap_1_1Internal.html

| Classes | |
| --- | --- |
| class | HeatmapIndicatorChunkedSeriesStorage |
| | Append-only, chunked, lock-free storage backing one series in IHeatmapVisualState. Two watermarks (`_frontCommitted` visible to the renderer, `_backStaged` written by the indicator under a lease) over a single linked chain of fixed-size chunks; commit is one Volatile.Write(ref long, long) (release barrier), no copy, no allocation past the growing tail chunk. |
| | |
| class | HeatmapIndicatorContext |
| | Mutable concrete implementation of IHeatmapIndicatorContext owned by the heatmap controller. Properties reflect the host's current state; the controller updates them in place as the host signals changes. Reads are safe from any thread (Volatile.Read on every property). |
| | |
| class | HeatmapIndicatorSeriesLease |
| | |
| class | HeatmapIndicatorSeriesNode |
| | |
| class | HeatmapIndicatorVisualLease |
| | |
| class | HeatmapIndicatorVisualNode |
| | |
| class | HeatmapIndicatorVisualState |
| | Concrete implementation of IHeatmapVisualState. Holds two node trees built once from the descriptor: a back tree the indicator mutates under a lease and a front tree the renderer reads. The two trees are otherwise structurally identical and share the same per-series HeatmapIndicatorChunkedSeriesStorage instances by reference, so large sample chains live in exactly one place — only small node-level fields (style, presentation) are duplicated. |
| | |
| class | HeatmapIndicatorVisualStateLease |
| | |
