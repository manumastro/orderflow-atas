# ATAS.Indicators.Heatmap.HeatmapLeaseMisuseException Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.html

Thrown when an indicator misuses the visual-state lease API. Distinct from a plain InvalidOperationException so call sites can catch lease misuse separately from generic invalid-operation errors, and tests can assert on Reason instead of message text.
 [More...](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.HeatmapLeaseMisuseException:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Heatmap.HeatmapLeaseMisuseException:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| [HeatmapLeaseMisuseReason](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77) | [Reason](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md#a3cd2de772d46ef8e4c42fa11190b5baf)`[get]` |
| | The misuse class — see HeatmapLeaseMisuseReason. |
| | |

## Detailed Description

Thrown when an indicator misuses the visual-state lease API. Distinct from a plain InvalidOperationException so call sites can catch lease misuse separately from generic invalid-operation errors, and tests can assert on Reason instead of message text.

## Property Documentation

## [◆](https://docs.atas.net/en/)Reason

| [HeatmapLeaseMisuseReason](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77) ATAS.Indicators.Heatmap.HeatmapLeaseMisuseException.Reason |
| --- |

get

The misuse class — see HeatmapLeaseMisuseReason.

The documentation for this class was generated from the following file:
- [HeatmapLeaseMisuseException.cs](../files/HeatmapLeaseMisuseException_8cs.md)
