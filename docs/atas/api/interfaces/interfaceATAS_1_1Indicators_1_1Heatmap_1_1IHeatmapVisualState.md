# ATAS.Indicators.Heatmap.IHeatmapVisualState Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.html

Persistent visual state owned by a single indicator instance and read by the renderer at frame rate. Created by the platform from the indicator's HeatmapIndicatorDescriptor and bound to the indicator via HeatmapIndicator<TSettings>.State; the same reference is valid for the entire lifetime of the runner. Resets (instrument switch, explicit `RequestStateResetAsync`) clear the content of every series but do NOT replace the state instance.
 [More...](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.IHeatmapVisualState:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [IHeatmapVisualStateLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md) | [BeginUpdate](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md#a7100951e24021b8bdc8723d000e9900d) () |
| | Acquire the exclusive write lease. Throws InvalidOperationException synchronously if a previous lease has not yet been disposed — there is no queueing. Authors dispose the returned lease (typically with a `using` statement); disposal commits the back-stage to the front and bumps Version if any mutation occurred. |
| | |

| Properties | |
| --- | --- |
| long | [Version](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md#a4c87dda274d20018eb826cf66ec0548a)`[get]` |
| | Monotonic counter bumped by exactly one for every commit (IHeatmapVisualStateLease.Dispose) that produced at least one mutation. The renderer compares the cached version against this property per frame; a change indicates new data to upload. Read with `Volatile.Read` semantics — safe from any thread. |
| | |
| HeatmapIndicatorDescriptor | [Descriptor](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md#ab02a8e3934f3ace129224697e4ffc9c2)`[get]` |
| | The owning descriptor — the same reference passed to the platform via IHeatmapIndicator.Descriptor. |
| | |
| IReadOnlyList | [Visuals](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md#a5aad304c1b38233258c751770eef42ff)`[get]` |
| | The visuals declared by the descriptor. Schema is fixed at construction time; this collection never gains or loses entries during the state's lifetime. Each visual exposes its series in the same fixed order as the descriptor declared them. |
| | |

## Detailed Description

Persistent visual state owned by a single indicator instance and read by the renderer at frame rate. Created by the platform from the indicator's HeatmapIndicatorDescriptor and bound to the indicator via HeatmapIndicator<TSettings>.State; the same reference is valid for the entire lifetime of the runner. Resets (instrument switch, explicit `RequestStateResetAsync`) clear the content of every series but do NOT replace the state instance.

Mutation goes through BeginUpdate; reads from the renderer are wait-free over the front-committed portion of every series. The detailed contract — including the front/back buffer model, the chunked append-only storage, lease exclusivity, and the version counter — is documented in `docs/HeatmapIndicatorsDesign.md`.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)BeginUpdate()

| [IHeatmapVisualStateLease](./interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md) ATAS.Indicators.Heatmap.IHeatmapVisualState.BeginUpdate | ( | | ) | |
| --- | --- | --- | --- | --- |

Acquire the exclusive write lease. Throws InvalidOperationException synchronously if a previous lease has not yet been disposed — there is no queueing. Authors dispose the returned lease (typically with a `using` statement); disposal commits the back-stage to the front and bumps Version if any mutation occurred.

## Property Documentation

## [◆](https://docs.atas.net/en/)Descriptor

| HeatmapIndicatorDescriptor ATAS.Indicators.Heatmap.IHeatmapVisualState.Descriptor |
| --- |

get

The owning descriptor — the same reference passed to the platform via IHeatmapIndicator.Descriptor.

## [◆](https://docs.atas.net/en/)Version

| long ATAS.Indicators.Heatmap.IHeatmapVisualState.Version |
| --- |

get

Monotonic counter bumped by exactly one for every commit (IHeatmapVisualStateLease.Dispose) that produced at least one mutation. The renderer compares the cached version against this property per frame; a change indicates new data to upload. Read with `Volatile.Read` semantics — safe from any thread.

## [◆](https://docs.atas.net/en/)Visuals

| IReadOnlyList ATAS.Indicators.Heatmap.IHeatmapVisualState.Visuals |
| --- |

get

The visuals declared by the descriptor. Schema is fixed at construction time; this collection never gains or loses entries during the state's lifetime. Each visual exposes its series in the same fixed order as the descriptor declared them.

The documentation for this interface was generated from the following file:
- [IHeatmapVisualState.cs](../files/IHeatmapVisualState_8cs.md)
