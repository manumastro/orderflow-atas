# ATAS.Indicators.Heatmap.IHeatmapDisposableIndicator Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.html

Optional capability: receive a deterministic disposal signal. The supervisor invokes DisposeAsync on the instance's own consumer task — serialised against any other in-flight call, observing the per-call timeout — when the instance is removed via `HeatmapIndicatorsController.RemoveInstanceAsync` or when the controller itself is disposed.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md#details)

| Public Member Functions | |
| --- | --- |
| ValueTask | [DisposeAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md#ae3e84fbb3c3f4db20ad0f414efe69638) (CancellationToken cancellationToken) |
| | |

## Detailed Description

Optional capability: receive a deterministic disposal signal. The supervisor invokes DisposeAsync on the instance's own consumer task — serialised against any other in-flight call, observing the per-call timeout — when the instance is removed via `HeatmapIndicatorsController.RemoveInstanceAsync` or when the controller itself is disposed.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)DisposeAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapDisposableIndicator.DisposeAsync | ( | CancellationToken | cancellationToken | ) | |
| --- | --- | --- | --- | --- | --- |

The documentation for this interface was generated from the following file:
- [IHeatmapIndicator.cs](../files/IHeatmapIndicator_8cs.md)
