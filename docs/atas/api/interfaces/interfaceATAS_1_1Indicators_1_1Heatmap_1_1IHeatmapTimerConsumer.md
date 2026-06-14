# ATAS.Indicators.Heatmap.IHeatmapTimerConsumer Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.html

Optional capability: receive periodic timer ticks. Most indicators do not need this; only opt in if the indicator must do work on a wall clock rather than in response to incoming data — for example, to expire stale state at session boundaries when no trade tick has arrived to wake the indicator up.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md#details)

| Public Member Functions | |
| --- | --- |
| ValueTask | [ProcessTimerAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md#aeeb00c0b0486063e4ee9f7eb250644fb) (HeatmapIndicatorTimerTick tick, CancellationToken cancellationToken) |
| | |

## Detailed Description

Optional capability: receive periodic timer ticks. Most indicators do not need this; only opt in if the indicator must do work on a wall clock rather than in response to incoming data — for example, to expire stale state at session boundaries when no trade tick has arrived to wake the indicator up.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ProcessTimerAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapTimerConsumer.ProcessTimerAsync | ( | HeatmapIndicatorTimerTick | tick, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellationToken |
| | ) | | |

The documentation for this interface was generated from the following file:
- [IHeatmapIndicator.cs](../files/IHeatmapIndicator_8cs.md)
