# HeatmapIndicatorChunkedSeriesStorage.cs File Reference

Source: https://docs.atas.net/en/HeatmapIndicatorChunkedSeriesStorage_8cs.html

| Classes | |
| --- | --- |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorChunkedSeriesStorage |
| | Append-only, chunked, lock-free storage backing one series in IHeatmapVisualState. Two watermarks (`_frontCommitted` visible to the renderer, `_backStaged` written by the indicator under a lease) over a single linked chain of fixed-size chunks; commit is one Volatile.Write(ref long, long) (release barrier), no copy, no allocation past the growing tail chunk. |
| | |
| class | ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorChunkedSeriesStorage.Chunk |
| | One link in the chain. Once linked into the chain, Data is treated as readable up to whatever count the visible watermark allows. |
| | |
| struct | [ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorChunkedSeriesStorage.Enumerator](../structs/structATAS_1_1Indicators_1_1Heatmap_1_1Internal_1_1HeatmapIndicatorChunkedSeriesStorage_1_1Enumerator.md) |
| | Wait-free forward enumerator over committed samples. Captures the committed count and the head chunk reference at construction time; the renderer holds it for the duration of one read pass and discards it. Subsequent commits do not affect the enumerator's visible range. [More...](../structs/structATAS_1_1Indicators_1_1Heatmap_1_1Internal_1_1HeatmapIndicatorChunkedSeriesStorage_1_1Enumerator.md#details) |
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
