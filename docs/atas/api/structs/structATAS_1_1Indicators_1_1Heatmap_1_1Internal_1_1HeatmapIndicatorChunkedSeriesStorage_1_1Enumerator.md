# ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorChunkedSeriesStorage.Enumerator Struct Reference

Source: https://docs.atas.net/en/structATAS_1_1Indicators_1_1Heatmap_1_1Internal_1_1HeatmapIndicatorChunkedSeriesStorage_1_1Enumerator.html

Wait-free forward enumerator over committed samples. Captures the committed count and the head chunk reference at construction time; the renderer holds it for the duration of one read pass and discards it. Subsequent commits do not affect the enumerator's visible range.
 [More...](./structATAS_1_1Indicators_1_1Heatmap_1_1Internal_1_1HeatmapIndicatorChunkedSeriesStorage_1_1Enumerator.md#details)

| Public Member Functions | |
| --- | --- |
| bool | [MoveNext](./structATAS_1_1Indicators_1_1Heatmap_1_1Internal_1_1HeatmapIndicatorChunkedSeriesStorage_1_1Enumerator.md#a3da79026342a47615c30c30a51487626) () |
| | |

| Properties | |
| --- | --- |
| HeatmapIndicatorVisualSample | [Current](./structATAS_1_1Indicators_1_1Heatmap_1_1Internal_1_1HeatmapIndicatorChunkedSeriesStorage_1_1Enumerator.md#a01979590bad19b10dd64b4132be538e4)`[get]` |
| | |

## Detailed Description

Wait-free forward enumerator over committed samples. Captures the committed count and the head chunk reference at construction time; the renderer holds it for the duration of one read pass and discards it. Subsequent commits do not affect the enumerator's visible range.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)MoveNext()

| bool ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorChunkedSeriesStorage.Enumerator.MoveNext | ( | | ) | |
| --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Current

| HeatmapIndicatorVisualSample ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorChunkedSeriesStorage.Enumerator.Current |
| --- |

get

The documentation for this struct was generated from the following file:
- [HeatmapIndicatorChunkedSeriesStorage.cs](../files/HeatmapIndicatorChunkedSeriesStorage_8cs.md)
