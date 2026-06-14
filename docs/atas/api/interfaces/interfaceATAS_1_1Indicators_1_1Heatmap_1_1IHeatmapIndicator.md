# ATAS.Indicators.Heatmap.IHeatmapIndicator Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.html

Non-generic runtime contract used by the platform, catalogue, and controller. Indicator authors should derive from HeatmapIndicator<TSettings> instead of implementing this interface directly.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapIndicator:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| ValueTask | [ConfigureAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aeeb8fb1ea9c82e04bd6875fda08fdc92) (object settings, CancellationToken cancellationToken) |
| | Apply user-edited settings. The runtime type of settings must be assignable to SettingsType. |
| | |
| ValueTask | [OnStateResetNotificationAsync](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#ac594c002ea8ac22ae362d018735899f6) ([IHeatmapIndicatorContext](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) context, [IHeatmapIndicatorRuntime](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) runtime, CancellationToken cancellationToken) |
| | Notify the indicator that the platform has cleared State. Called when the active context changes (instrument, session, time zone) or when the indicator itself called IHeatmapIndicatorRuntime.RequestStateResetAsync. |
| | |

| Properties | |
| --- | --- |
| HeatmapIndicatorDescriptor | [Descriptor](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aa40c3e70a852b72744531b764eedfd6c)`[get]` |
| | Static type-level metadata. Identifies the indicator type, declares its placement (overlay vs sub-panel), visual roles, and series shape. Must not change between calls. |
| | |
| Type | [SettingsType](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#af6683f22968821e5f4155dbfde0e9a48)`[get]` |
| | CLR type of the settings DTO this indicator accepts. Used by the catalogue to materialise persisted settings into the right shape before calling ConfigureAsync. |
| | |
| [IHeatmapVisualState](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) | [State](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aecb311cf43701325c593441454ed5cbe)`[get]` |
| | Persistent visual state read by the renderer. Created by the platform from Descriptor at instance construction time; the same reference is returned for the entire instance lifetime. Resets clear content but never replace the reference. |
| | |

## Detailed Description

Non-generic runtime contract used by the platform, catalogue, and controller. Indicator authors should derive from HeatmapIndicator<TSettings> instead of implementing this interface directly.

New here? Read `[Indicators/Heatmap/README.md](../overviews/README_8md.md)` first — it contains a five-minute quickstart, the lifecycle reference, the threading contract, and pointers to working reference indicators. The full architecture is documented in `docs/HeatmapIndicatorsDesign.md`.

Per-instance threading. The platform serialises every call on a single instance — at most one of ConfigureAsync, OnStateResetNotificationAsync, the warm-up call from IHeatmapWarmupIndicator.WarmUpAsync, the consumer calls (IHeatmapTradeTickConsumer.ProcessTicksAsync, IHeatmapProfileConsumer.ProcessProfileAsync, IHeatmapTimerConsumer.ProcessTimerAsync) is in flight for a given instance. Authors do NOT need to lock for these methods to coordinate against each other. The platform may invoke methods on different instances concurrently, so static state shared between instances still needs the author's protection.

State mutation. Indicators publish their visuals by mutating State under a lease — see IHeatmapVisualState.BeginUpdate. The renderer reads `State` directly at frame rate; the platform never asks the indicator for a snapshot synchronously.

Cancellation. Each call carries a CancellationToken that fires at the platform's per-call timeout. Implementers MUST honour cancellation — hangs that ignore it are treated as fatal for the instance and cannot be recovered.

Unhandled exceptions. If any lifecycle or capability method throws an exception that is not OperationCanceledException triggered by the supplied token, the supervisor logs and transitions the runner to `HeatmapIndicatorSnapshotState.Failed`; further callbacks stop being dispatched until the host removes and re-adds the instance.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ConfigureAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapIndicator.ConfigureAsync | ( | object | settings, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellationToken |
| | ) | | |

Apply user-edited settings. The runtime type of settings must be assignable to SettingsType.

## [◆](https://docs.atas.net/en/)OnStateResetNotificationAsync()

| ValueTask ATAS.Indicators.Heatmap.IHeatmapIndicator.OnStateResetNotificationAsync | ( | [IHeatmapIndicatorContext](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) | context, |
| --- | --- | --- | --- |
| | | [IHeatmapIndicatorRuntime](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) | runtime, |
| | | CancellationToken | cancellationToken |
| | ) | | |

Notify the indicator that the platform has cleared State. Called when the active context changes (instrument, session, time zone) or when the indicator itself called IHeatmapIndicatorRuntime.RequestStateResetAsync.

The notification is informational — the front buffer is already blank when this method is invoked. The indicator does NOT mutate state from inside this callback; it simply notes the reset and repopulates state from inside the next data callback (warm-up or process-ticks).

## Property Documentation

## [◆](https://docs.atas.net/en/)Descriptor

| HeatmapIndicatorDescriptor ATAS.Indicators.Heatmap.IHeatmapIndicator.Descriptor |
| --- |

get

Static type-level metadata. Identifies the indicator type, declares its placement (overlay vs sub-panel), visual roles, and series shape. Must not change between calls.

Implemented in [ATAS.Indicators.Heatmap.HeatmapIndicator](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ae079a3ebe7a405325c9a2dacbaa5e87d).

## [◆](https://docs.atas.net/en/)SettingsType

| Type ATAS.Indicators.Heatmap.IHeatmapIndicator.SettingsType |
| --- |

get

CLR type of the settings DTO this indicator accepts. Used by the catalogue to materialise persisted settings into the right shape before calling ConfigureAsync.

Implemented in [ATAS.Indicators.Heatmap.HeatmapIndicator](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a0d2130e3535253620678f832b55ed343).

## [◆](https://docs.atas.net/en/)State

| [IHeatmapVisualState](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) ATAS.Indicators.Heatmap.IHeatmapIndicator.State |
| --- |

get

Persistent visual state read by the renderer. Created by the platform from Descriptor at instance construction time; the same reference is returned for the entire instance lifetime. Resets clear content but never replace the reference.

Implemented in [ATAS.Indicators.Heatmap.HeatmapIndicator](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a260fc71cc91bb756db06b76dce08d78f).

The documentation for this interface was generated from the following file:
- [IHeatmapIndicator.cs](../files/IHeatmapIndicator_8cs.md)
