# IHeatmapVisualState.cs File Reference

Source: https://docs.atas.net/en/IHeatmapVisualState_8cs.html

| Classes | |
| --- | --- |
| interface | [ATAS.Indicators.Heatmap.IHeatmapVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) |
| | Persistent visual state owned by a single indicator instance and read by the renderer at frame rate. Created by the platform from the indicator's HeatmapIndicatorDescriptor and bound to the indicator via HeatmapIndicator.State; the same reference is valid for the entire lifetime of the runner. Resets (instrument switch, explicit `RequestStateResetAsync`) clear the content of every series but do NOT replace the state instance. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapPlatformResettableVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md) |
| | Platform-owned extension for clearing an indicator state without going through an author-visible update lease. Indicator authors should use IHeatmapVisualState.BeginUpdate instead. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapVisualStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md) |
| | Read-only handle to a visual within an IHeatmapVisualState. Carries the descriptor metadata plus the list of series; mutation goes through the lease (IHeatmapVisualStateLease.Visual). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md) |
| | Read-only handle to a series within an IHeatmapVisualStateNode. The renderer iterates committed samples through this interface; mutation goes through the lease (IHeatmapVisualLease.Series). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapVisualStateLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md) |
| | The exclusive write lease on an IHeatmapVisualState. Acquired via IHeatmapVisualState.BeginUpdate; disposing commits the back-stage to the front. A lease can only be used inside the calling stack frame — passing it across `await` points or to background tasks is a misuse pattern (the platform serialises every indicator callback on a single task, so all legitimate writes happen from inside one of those callbacks). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapVisualLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) |
| | Per-visual mutation surface inside the lease. Style and presentation are mutable properties on the lease; series content is mutated through Series. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapSeriesLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) |
| | Per-series mutation surface inside the lease. Append, replace, clear, and trim operations are buffered to the back-stage and become visible to the renderer on lease disposal. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |
