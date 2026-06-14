# ATAS.Indicators.Heatmap.IHeatmapTradeTickConsumer Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTradeTickConsumer.html

| Public Member Functions | |
| --- | --- |
| ValueTask | [ProcessTicksAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTradeTickConsumer.md#acad15475955e6c618601cb6040e1bd9c) (HeatmapTickBatch ticks, CancellationToken cancellationToken) |
| | Process a batch of live trade ticks. The platform batches incoming ticks into chunks (size and cadence are platform-controlled) so this method is invoked at low frequency rather than per tick. |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ProcessTicksAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapTradeTickConsumer.ProcessTicksAsync | ( | HeatmapTickBatch | ticks, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellationToken |
| | ) | | |

Process a batch of live trade ticks. The platform batches incoming ticks into chunks (size and cadence are platform-controlled) so this method is invoked at low frequency rather than per tick.

Implementers must complete promptly (target: a few milliseconds for typical batches). A hung call leaves the indicator unhealthy until the platform's per-instance timeout elapses.

The supplied HeatmapTickBatch is a zero-allocation view over a pooled buffer; see the type's documentation for the lifetime contract — do not capture it or any span / sample reference derived from it beyond the awaited completion of this call.

Await semantics. If the implementation awaits inside this method, no other lifecycle call on the same instance can have executed in the meantime — the supervisor serialises every call on a single per-instance task. Fields the indicator wrote before the await are still valid on resume; there is no need to re-validate state or re-acquire locks against other lifecycle methods. The pooled HeatmapTickBatch stays valid across the await for the duration of THIS call, but only this call.

The documentation for this interface was generated from the following file:
- [IHeatmapIndicator.cs](../files/IHeatmapIndicator_8cs.md)
