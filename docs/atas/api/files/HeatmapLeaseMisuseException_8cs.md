# HeatmapLeaseMisuseException.cs File Reference

Source: https://docs.atas.net/en/HeatmapLeaseMisuseException_8cs.html

| Classes | |
| --- | --- |
| class | [ATAS.Indicators.Heatmap.HeatmapLeaseMisuseException](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md) |
| | Thrown when an indicator misuses the visual-state lease API. Distinct from a plain InvalidOperationException so call sites can catch lease misuse separately from generic invalid-operation errors, and tests can assert on Reason instead of message text. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [ATAS.Indicators.Heatmap.HeatmapLeaseMisuseReason](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77) {
  [ATAS.Indicators.Heatmap.LeaseAlreadyHeld](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a68411a193efd76e1dc1868c0caa4a246)
, [ATAS.Indicators.Heatmap.WrongDescriptor](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a08ffe60f082cdfd78b4987426a9edf3a)
, [ATAS.Indicators.Heatmap.UnknownVisual](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a7cb57e8f1aae0512cd57067c6024d127)
, [ATAS.Indicators.Heatmap.WrongVisual](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a8015bf177909b7be870850862fd978f9)
,
  [ATAS.Indicators.Heatmap.UnknownSeries](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a51998a3e1dbdae3bfcb7817b7074df89)
, [ATAS.Indicators.Heatmap.MissingValueProjection](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a0092d89ce4ac97f449278d4680e14ada)

 } |
| | Why a HeatmapLeaseMisuseException was thrown. Lets callers distinguish misuse classes without parsing the message text. [More...](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77) |
| | |
