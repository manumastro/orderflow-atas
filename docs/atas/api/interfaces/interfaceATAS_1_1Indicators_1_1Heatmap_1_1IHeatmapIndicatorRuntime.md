# ATAS.Indicators.Heatmap.IHeatmapIndicatorRuntime Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.html

Runtime handle the platform passes to indicators at warm-up / reset time. Lets the indicator drive its own re-warm or full state reset from any of its async methods. The handle is rebound on every reset, so indicators MUST NOT retain a runtime reference across IHeatmapIndicator.OnStateResetNotificationAsync calls.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md#details)

| Public Member Functions | |
| --- | --- |
| ValueTask | [RequestReWarmAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md#a9d19e0eb802909db740ee7ddb52f0b9a) (string reason, CancellationToken cancellationToken) |
| | Ask the platform to re-issue IHeatmapWarmupIndicator.WarmUpAsync for this instance. The controller pulls a fresh HeatmapIndicatorWarmupBundle from the IHeatmapIndicatorWarmupHost and queues a warm-up work item on the instance's own channel (serialised against any in-flight call; observes the supervisor's per-call timeout). |
| | |
| ValueTask | [RequestStateResetAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md#a37b685e43ef5682590db17e3575ac314) (string reason, CancellationToken cancellationToken) |
| | Ask the platform to perform a full state reset. The platform clears every series in the indicator's IHeatmapIndicator.State (front and back), then calls IHeatmapIndicator.OnStateResetNotificationAsync, then queues the next warm-up + tick callbacks. Use only when settings or ingestion state changed in a way that invalidates everything; for a background rebuild that should not blank the front, take a lease and call `Clear()` + refill in the same lease (see `HeatmapIndicatorsDesign.md` §5.2). |
| | |

## Detailed Description

Runtime handle the platform passes to indicators at warm-up / reset time. Lets the indicator drive its own re-warm or full state reset from any of its async methods. The handle is rebound on every reset, so indicators MUST NOT retain a runtime reference across IHeatmapIndicator.OnStateResetNotificationAsync calls.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)RequestReWarmAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapIndicatorRuntime.RequestReWarmAsync | ( | string | reason, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellationToken |
| | ) | | |

Ask the platform to re-issue IHeatmapWarmupIndicator.WarmUpAsync for this instance. The controller pulls a fresh HeatmapIndicatorWarmupBundle from the IHeatmapIndicatorWarmupHost and queues a warm-up work item on the instance's own channel (serialised against any in-flight call; observes the supervisor's per-call timeout).

Parameters

| reason | Free-form diagnostic string describing why the indicator wants to re-warm. Surfaced in logs when the warm-up host has nothing to produce. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RequestStateResetAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapIndicatorRuntime.RequestStateResetAsync | ( | string | reason, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellationToken |
| | ) | | |

Ask the platform to perform a full state reset. The platform clears every series in the indicator's IHeatmapIndicator.State (front and back), then calls IHeatmapIndicator.OnStateResetNotificationAsync, then queues the next warm-up + tick callbacks. Use only when settings or ingestion state changed in a way that invalidates everything; for a background rebuild that should not blank the front, take a lease and call `Clear()` + refill in the same lease (see `HeatmapIndicatorsDesign.md` §5.2).

The documentation for this interface was generated from the following file:
- [IHeatmapIndicator.cs](../files/IHeatmapIndicator_8cs.md)
