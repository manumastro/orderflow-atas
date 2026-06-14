# Heatmap Indicators — Author Guide

Source: https://docs.atas.net/en/md_Indicators_2Heatmap_2README.html

Status: v2 (lease/state model). Pre-release; the surface may still evolve before first public ship. Architecture rationale lives in `docs/HeatmapIndicatorsDesign.md`; recipes live in `docs/HeatmapIndicatorsCookbook.md`; this README is the door.

# 1. What this is

A heatmap indicator owns a single long-lived visual state that the renderer reads at frame rate. The indicator pushes new samples into the state under a short-lived exclusive lease; on lease disposal the back-stage commits to the front and the renderer picks the change up on the next frame. There is no polling and no `GetSnapshotAsync` — the indicator is the writer, the renderer is the wait-free reader.

Contrast with classic `Indicator`-derived bar / candle indicators: they pull bars on the chart thread; heatmap indicators are async, run on a per-instance consumer task supervised by the platform, and react to live trade ticks and market-profile snapshots.

# 2. Authoring an indicator

A complete indicator is the descriptor (built once in a static ctor), the settings DTO, and one or more capability interfaces. The skeleton below is a working price line driven by trade ticks:

[HeatmapIndicator(id: "myvendor.simple", DisplayName = "Simple")]

public sealed class MyIndicator

 : HeatmapIndicator<MySettings>

 , IHeatmapTradeTickConsumer

{

 private static readonly HeatmapIndicatorDescriptor _descriptor;

 private static readonly HeatmapIndicatorVisualHandle _line;

 private static readonly HeatmapIndicatorSeriesHandle<decimal> _price;

 static MyIndicator()

 {

 var build = Describe<MyIndicator>();

 _line = build.PriceLine("my.line", "Price");

 _price = _line.Series("my.price",

 HeatmapIndicatorSeriesRole.Price,

 HeatmapIndicatorValueKind.Price);

 _descriptor = build.Done();

 }

 public override HeatmapIndicatorDescriptor Descriptor => _descriptor;

 public ValueTask ProcessTicksAsync(HeatmapTickBatch ticks, CancellationToken ct)

 {

 using var lease = State.BeginUpdate();

 var s = lease.Series(_price);

 foreach (var tick in ticks)

 s.Append(tick.TimestampNanos, tick.Price);

 return ValueTask.CompletedTask;

 }

}

That's the contract: one descriptor, typed handles, a settings DTO, and mutation through `using var lease = State.BeginUpdate();`. The settings DTO is reachable as the inherited `Settings` property — the base class populates it before any data callback runs, so most indicators don't need to override `ConfigureAsync` at all. Override it only when a settings change must actively trigger work (request a reset, refresh levels). For richer shapes (typed series with projection, sub-panel pairs, profile-driven levels) see the cookbook recipes.

# 3. Capability interfaces

`IHeatmapIndicator` is the always-required core (the `HeatmapIndicator` base implements it for you). Capabilities are opt-in — implement only what you need. The supervisor inspects which capabilities you advertise and routes calls accordingly.

| Interface | Implement when… |
| --- | --- |
| `IHeatmapWarmupIndicator` | You benefit from historical ticks/profiles before going live. |
| `IHeatmapTradeTickConsumer` | You react to individual trades. Most indicators do. |
| `IHeatmapProfileConsumer` | You react to market-profile snapshots (POC, value area, OHLC). |
| `IHeatmapTimerConsumer` | You need wall-clock heartbeats independent of incoming data (rare). |
| `IHeatmapHistoricalDataLoadedConsumer` | You must rebuild when the host expands the working range backward. |
| `IHeatmapDisposableIndicator` | You hold cross-call resources that must be released deterministically. |

You can implement multiple capabilities on the same class — many indicators combine `IHeatmapWarmupIndicator` + `IHeatmapTradeTickConsumer`.

# 4. Lifecycle

The platform serialises every callback on a single per-instance consumer task — at most one of these is in flight per indicator at any moment.

ctor

 -> ConfigureAsync(initial settings) first work item on the runner task

 -> OnStateResetNotificationAsync(context, runtime)

 -> WarmUpAsync(request, sources) if IHeatmapWarmupIndicator

 -> first ProcessTicksAsync / ProcessProfileAsync takes a lease, populates state

 -> ... live callbacks continue ...

 -> DisposeAsync if IHeatmapDisposableIndicator

Notable points:

- The `State` reference is stable for the entire instance lifetime. Resets clear content; they never replace the reference.

- **`OnStateResetNotificationAsync` is informational** — the front buffer is already empty when it's called. Drop your calculation caches, capture the supplied `IHeatmapIndicatorRuntime`, and return. Don't call `State.BeginUpdate()` from inside it.

- Indicator-initiated rebuilds that should keep the front visible use `Clear()` + refill in one lease (no `RequestStateResetAsync`).

- Settings changes that invalidate everything call `runtime.RequestStateResetAsync("reason", ct)` from `ConfigureAsync` — there will be a brief blank frame before warm-up repopulates.

For the full lifecycle table and the per-callback contract see `docs/HeatmapIndicatorsDesign.md` §5.

# 5. Threading and the lease

- Per-instance serialisation. All lifecycle and capability methods on one instance run on a single dedicated task. You do not need locks to coordinate them against each other.

- Single-writer state. `State.BeginUpdate()` returns the exclusive write lease. A second `BeginUpdate()` while a previous lease is still alive throws `InvalidOperationException` synchronously — there is no queueing. Authors should keep the lease scoped to the calling stack frame (`using var lease = ...;`) and not pass it to background tasks.

- Wait-free readers. The renderer reads `State` from the renderer thread without blocking the indicator. It compares `IHeatmapVisualState.Version` per frame; a change indicates new content to upload.

- One in-flight `BeginUpdate` per instance. If you fork a CPU job with `Task.Run`, do the heavy compute into local buffers and open the lease only at the end, on the platform's task.

- Cross-instance. The platform may run different instances of the same type concurrently. Static fields shared between instances are your responsibility to protect.

For threading rationale see `docs/HeatmapIndicatorsDesign.md` §4 and §8.

# 6. Where to look next

- **`docs/HeatmapIndicatorsDesign.md`** — the architecture: front/back chunked storage, lease semantics, reset model, renderer integration, open design questions.

- **`docs/HeatmapIndicatorsCookbook.md`** — recipes by task: tick-driven decimal series, typed series with projection, sub-panel pair, profile-driven indicator, reacting to state reset, opting into the historical-data-loaded callback.

- API surface — start at [IHeatmapIndicator.cs](https://docs.atas.net/en/IHeatmapIndicator.cs) (lifecycle + capability interfaces) and [IHeatmapVisualState.cs](https://docs.atas.net/en/IHeatmapVisualState.cs) (lease + series).

- Reference indicators in `Indicators.Technical/Technical/Heatmap/` — full working v2 examples.

| If you want… | Start from |
| --- | --- |
| Simple trade-tick price overlay | `HeatmapVwapIndicator` |
| Multiple typed series on one visual | `HeatmapValueAreaIndicator` |
| Sub-panel scalar with state reset | `HeatmapCvdIndicator` |
| Paired buy/sell sub-panel | `HeatmapMarketPressureIndicator` |
| Training-period gating | `HeatmapPriceChangeIndicator` |
| Profile-driven dynamic level lines | `HeatmapOhlcLevelsIndicator` |
