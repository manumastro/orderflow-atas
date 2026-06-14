# ATAS.Indicators.Heatmap.IHeatmapHistoricalDataLoadedConsumer Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.html

Optional capability: receive a notification when the host's working data range expanded backward (e.g. user panned into history that triggered a load). Implement only if the indicator needs to rebuild or refill calculation state across newly-loaded historical samples. Typical reaction: take a lease, `Clear()`, refill from new historical range, dispose lease — the front stays visible across the rebuild.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md#details)

| Public Member Functions | |
| --- | --- |
| ValueTask | [OnHistoricalDataLoadedAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md#a42825ccc12ac02f28b31348c3d62ddda) ([HeatmapWorkingRangeUpdate](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#a2989f04048378b558492f2c8e98c2484) update, CancellationToken cancellationToken) |
| | |

## Detailed Description

Optional capability: receive a notification when the host's working data range expanded backward (e.g. user panned into history that triggered a load). Implement only if the indicator needs to rebuild or refill calculation state across newly-loaded historical samples. Typical reaction: take a lease, `Clear()`, refill from new historical range, dispose lease — the front stays visible across the rebuild.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnHistoricalDataLoadedAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapHistoricalDataLoadedConsumer.OnHistoricalDataLoadedAsync | ( | [HeatmapWorkingRangeUpdate](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#a2989f04048378b558492f2c8e98c2484) | update, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellationToken |
| | ) | | |

The documentation for this interface was generated from the following file:
- [IHeatmapIndicator.cs](../files/IHeatmapIndicator_8cs.md)
