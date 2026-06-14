# ATAS.Indicators.Heatmap.HeatmapIndicatorFallbackReWarmGuard Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.html

State holder for indicators whose calculation is anchored at the real data start (e.g. CVD `FromDataStart`, VWAP `FromDataStart`) and may receive a fallback-range warm-up before the host knows the real data start. Encapsulates the latch protocol so the indicator does not have to track three flags and an inline check by hand.
 [More...](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md#details)

| Public Member Functions | |
| --- | --- |
| void | [Reset](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md#a2ba4286fdbbae84a69554e64ec5ae03e) () |
| | |
| void | [OnWarmedUp](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md#ae7db2da726ba13dab614a070bedb8da4) (HeatmapIndicatorWarmupRequest request) |
| | |
| bool | [ShouldRequestReWarm](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md#a3522e78292535de338f85097d459af0c) (HeatmapTickBatch ticks) |
| | |

## Detailed Description

State holder for indicators whose calculation is anchored at the real data start (e.g. CVD `FromDataStart`, VWAP `FromDataStart`) and may receive a fallback-range warm-up before the host knows the real data start. Encapsulates the latch protocol so the indicator does not have to track three flags and an inline check by hand.

Usage:

- Construct once per instance, typically as a readonly field.

- Call Reset from `ResetAsync`.

- Call OnWarmedUp from `WarmUpAsync` with the incoming request — the guard captures HeatmapIndicatorWarmupRequest.IsFallbackRange and re-arms the latch.

- Call ShouldRequestReWarm from `ProcessTicksAsync` after processing the batch; if it returns `true`, await IHeatmapIndicatorRuntime.RequestReWarmAsync.

The guard latches per warm-up cycle: ShouldRequestReWarm returns `true` at most once between two OnWarmedUp calls, so repeated tick batches do not re-trigger. A fresh warm-up re-arms the latch for the next fallback episode.

Threading: per-instance, lock-free. The platform serialises calls on a single indicator instance, which is also the only valid caller of this guard.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnWarmedUp()

| void ATAS.Indicators.Heatmap.HeatmapIndicatorFallbackReWarmGuard.OnWarmedUp | ( | HeatmapIndicatorWarmupRequest | request | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Reset()

| void ATAS.Indicators.Heatmap.HeatmapIndicatorFallbackReWarmGuard.Reset | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ShouldRequestReWarm()

| bool ATAS.Indicators.Heatmap.HeatmapIndicatorFallbackReWarmGuard.ShouldRequestReWarm | ( | HeatmapTickBatch | ticks | ) | |
| --- | --- | --- | --- | --- | --- |

The documentation for this class was generated from the following file:
- [HeatmapIndicatorFallbackReWarmGuard.cs](../files/HeatmapIndicatorFallbackReWarmGuard_8cs.md)
