# IHeatmapIndicator.cs File Reference

Source: https://docs.atas.net/en/IHeatmapIndicator_8cs.html

| Classes | |
| --- | --- |
| interface | [ATAS.Indicators.Heatmap.IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md) |
| | Non-generic runtime contract used by the platform, catalogue, and controller. Indicator authors should derive from HeatmapIndicator instead of implementing this interface directly. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#details) |
| | |
| class | [ATAS.Indicators.Heatmap.HeatmapIndicator](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md) |
| | Author-facing entry points for the heatmap indicator API. The non-generic `HeatmapIndicator` coexists with the generic HeatmapIndicator base class — different arities disambiguate them at the type system level. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) |
| | Runtime handle the platform passes to indicators at warm-up / reset time. Lets the indicator drive its own re-warm or full state reset from any of its async methods. The handle is rebound on every reset, so indicators MUST NOT retain a runtime reference across IHeatmapIndicator.OnStateResetNotificationAsync calls. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapWarmupIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapWarmupIndicator.md) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapTradeTickConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTradeTickConsumer.md) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapProfileConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapProfileConsumer.md) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapTimerConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md) |
| | Optional capability: receive periodic timer ticks. Most indicators do not need this; only opt in if the indicator must do work on a wall clock rather than in response to incoming data — for example, to expire stale state at session boundaries when no trade tick has arrived to wake the indicator up. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapHistoricalDataLoadedConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md) |
| | Optional capability: receive a notification when the host's working data range expanded backward (e.g. user panned into history that triggered a load). Implement only if the indicator needs to rebuild or refill calculation state across newly-loaded historical samples. Typical reaction: take a lease, `Clear()`, refill from new historical range, dispose lease — the front stays visible across the rebuild. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md#details) |
| | |
| interface | [ATAS.Indicators.Heatmap.IHeatmapDisposableIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md) |
| | Optional capability: receive a deterministic disposal signal. The supervisor invokes DisposeAsync on the instance's own consumer task — serialised against any other in-flight call, observing the per-call timeout — when the instance is removed via `HeatmapIndicatorsController.RemoveInstanceAsync` or when the controller itself is disposed. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.Indicators](../namespaces/namespaceATAS_1_1Indicators.md) |
| | |
| namespace | [ATAS.Indicators.Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |

| Variables | |
| --- | --- |
| sealed record | [ATAS.Indicators.Heatmap.HeatmapWorkingRangeUpdate](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#a2989f04048378b558492f2c8e98c2484) |
| | Payload for IHeatmapHistoricalDataLoadedConsumer.OnHistoricalDataLoadedAsync. The host fired the load; the indicator decides whether to react. |
| | |
