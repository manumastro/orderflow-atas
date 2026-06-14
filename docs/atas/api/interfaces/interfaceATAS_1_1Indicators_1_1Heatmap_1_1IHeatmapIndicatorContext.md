# ATAS.Indicators.Heatmap.IHeatmapIndicatorContext Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.html

Live runtime context exposed to a heatmap indicator. Unlike the v1 `HeatmapIndicatorContext` record (snapshot at reset), this is a read-only interface whose properties reflect the host's current state at every read. Indicators query it on demand — there are no "context changed" events because the most volatile field (Viewport) updates every render frame and a cross-thread notification per change would dominate the cost.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapIndicatorContext:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| string | [InstrumentId](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#ab9250381208d50b9d61ce46ced26e838)`[get]` |
| | The instrument the heatmap is bound to. |
| | |
| decimal | [TickSize](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#ac30f1ee91fda4fcd6ae774e86cf0688a)`[get]` |
| | Minimum price increment for the active instrument. |
| | |
| decimal | [LotSize](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#ae94cd624ca146ad62943c0b998bee9e8)`[get]` |
| | Multiplier converting lots into volume for the active instrument. |
| | |
| TimeZoneInfo | [TimeZone](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#a154c93e9a02c55eb9d8ce87ade1fd0fb)`[get]` |
| | Exchange / session time zone — use to format timestamps for the user. |
| | |
| string? | [TradingSessionId](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#a50053c7b16b2df5272f5fb4f02da6edf)`[get]` |
| | Optional session identifier; null when the host has no session concept. |
| | |
| long | [DataStartTimeNanos](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#abb50ccfa5044d297af67869ecf7befea)`[get]` |
| | Earliest timestamp for which heatmap data is currently loaded. Moves backward as the user pans into history and the host loads more. |
| | |
| long? | [DataEndTimeNanos](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#a4f4b767649f6dec8f75cc9adb48db786)`[get]` |
| | Latest timestamp for which heatmap data is currently loaded. `null` in live mode (= "wall clock now"); set to a finite value in playback / historical replay modes. |
| | |
| [HeatmapViewport](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#a209281643599139f1d052d3e7be8e646) | [Viewport](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#a9a71255b5e3e62d0f834e0e36b3754a5)`[get]` |
| | Last reported visible viewport. Updated by the host with sub-frame latency in live mode but with no realtime guarantee. Use for decimation hints; do not use for correctness. |
| | |

## Detailed Description

Live runtime context exposed to a heatmap indicator. Unlike the v1 `HeatmapIndicatorContext` record (snapshot at reset), this is a read-only interface whose properties reflect the host's current state at every read. Indicators query it on demand — there are no "context changed" events because the most volatile field (Viewport) updates every render frame and a cross-thread notification per change would dominate the cost.

Stable across the indicator's lifetime: InstrumentId, TickSize, LotSize, TimeZone, TradingSessionId. A change in any of these triggers a platform-initiated reset before the context reports the new value.

Updates as data flows: DataStartTimeNanos moves backward when historical data loads (a separate `OnHistoricalDataLoadedAsync` notification fires for indicators that need to react), DataEndTimeNanos tracks the latest loaded timestamp (null in live mode = wall-clock now).

Updates per frame in live mode: Viewport. Indicators MUST treat the viewport as a hint (e.g. for decimation decisions); it MUST NOT be used for correctness — there is no guarantee the viewport read inside one callback matches the one the renderer drew with.

## Property Documentation

## [◆](https://docs.atas.net/en/)DataEndTimeNanos

| long? ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.DataEndTimeNanos |
| --- |

get

Latest timestamp for which heatmap data is currently loaded. `null` in live mode (= "wall clock now"); set to a finite value in playback / historical replay modes.

## [◆](https://docs.atas.net/en/)DataStartTimeNanos

| long ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.DataStartTimeNanos |
| --- |

get

Earliest timestamp for which heatmap data is currently loaded. Moves backward as the user pans into history and the host loads more.

## [◆](https://docs.atas.net/en/)InstrumentId

| string ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.InstrumentId |
| --- |

get

The instrument the heatmap is bound to.

## [◆](https://docs.atas.net/en/)LotSize

| decimal ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.LotSize |
| --- |

get

Multiplier converting lots into volume for the active instrument.

## [◆](https://docs.atas.net/en/)TickSize

| decimal ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.TickSize |
| --- |

get

Minimum price increment for the active instrument.

## [◆](https://docs.atas.net/en/)TimeZone

| TimeZoneInfo ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.TimeZone |
| --- |

get

Exchange / session time zone — use to format timestamps for the user.

## [◆](https://docs.atas.net/en/)TradingSessionId

| string? ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.TradingSessionId |
| --- |

get

Optional session identifier; null when the host has no session concept.

## [◆](https://docs.atas.net/en/)Viewport

| [HeatmapViewport](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md#a209281643599139f1d052d3e7be8e646) ATAS.Indicators.Heatmap.IHeatmapIndicatorContext.Viewport |
| --- |

get

Last reported visible viewport. Updated by the host with sub-frame latency in live mode but with no realtime guarantee. Use for decimation hints; do not use for correctness.

The documentation for this interface was generated from the following file:
- [IHeatmapIndicatorContext.cs](../files/IHeatmapIndicatorContext_8cs.md)
