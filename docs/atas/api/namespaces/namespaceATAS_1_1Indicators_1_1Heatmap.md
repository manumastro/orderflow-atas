# ATAS.Indicators.Heatmap Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1Indicators_1_1Heatmap.html

| Namespaces | |
| --- | --- |
| namespace | [Internal](./namespaceATAS_1_1Indicators_1_1Heatmap_1_1Internal.md) |
| | |

| Classes | |
| --- | --- |
| class | [HeatmapIndicator](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md) |
| | Author-facing entry points for the heatmap indicator API. The non-generic `HeatmapIndicator` coexists with the generic HeatmapIndicator base class — different arities disambiguate them at the type system level. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#details) |
| | |
| class | [HeatmapIndicatorAttribute](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md) |
| | Marks a class as a heatmap indicator type and supplies discovery metadata. Apply to a concrete class that derives from HeatmapIndicator (or implements IHeatmapIndicator directly). [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md#details) |
| | |
| class | HeatmapIndicatorAttributeCache |
| | Reflection cache that amortises `GetCustomAttribute` to O(1) per type. Used by the typed `Describe` helper so the attribute becomes the single source of truth for the type id. |
| | |
| class | HeatmapIndicatorColors |
| | Small colour helpers for heatmap indicator settings and visual overrides. They keep indicator assemblies from duplicating platform-internal colour parsing code. |
| | |
| class | [HeatmapIndicatorDescriptorBuilder](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) |
| | Fluent builder that produces an immutable HeatmapIndicatorDescriptor alongside the typed visual / series handles required by the state builder. Single-shot: each builder yields exactly one descriptor via Done; further mutation throws. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#details) |
| | |
| class | [HeatmapIndicatorFallbackReWarmGuard](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md) |
| | State holder for indicators whose calculation is anchored at the real data start (e.g. CVD `FromDataStart`, VWAP `FromDataStart`) and may receive a fallback-range warm-up before the host knows the real data start. Encapsulates the latch protocol so the indicator does not have to track three flags and an inline check by hand. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md#details) |
| | |
| class | [HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) |
| | Strongly typed handle for a series within a visual. Returned from HeatmapIndicatorVisualHandle.Series(string, HeatmapIndicatorSeriesRole, HeatmapIndicatorValueKind, System.Func, HeatmapIndicatorVisualStyle?, string?, string?); the constructor is internal so authors cannot fabricate one. TValue is the indicator-internal sample type — what the indicator's calculation produces. The handle also carries the projection that converts the typed sample to a renderer-facing decimal; the lease applies the projection inline on every Append so the chunked storage holds decimal samples ready for the renderer. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md#details) |
| | |
| class | [HeatmapIndicatorVisualHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) |
| | Strongly typed handle for a visual added to a descriptor via HeatmapIndicatorDescriptorBuilder. The handle captures the owning descriptor's identity so the state builder can reject handles from a different descriptor at runtime, and the constructor is internal so authors cannot fabricate handles by hand. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md#details) |
| | |
| class | [HeatmapLeaseMisuseException](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md) |
| | Thrown when an indicator misuses the visual-state lease API. Distinct from a plain InvalidOperationException so call sites can catch lease misuse separately from generic invalid-operation errors, and tests can assert on Reason instead of message text. [More...](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md#details) |
| | |
| interface | [IHeatmapDisposableIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md) |
| | Optional capability: receive a deterministic disposal signal. The supervisor invokes DisposeAsync on the instance's own consumer task — serialised against any other in-flight call, observing the per-call timeout — when the instance is removed via `HeatmapIndicatorsController.RemoveInstanceAsync` or when the controller itself is disposed. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md#details) |
| | |
| interface | [IHeatmapHistoricalDataLoadedConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md) |
| | Optional capability: receive a notification when the host's working data range expanded backward (e.g. user panned into history that triggered a load). Implement only if the indicator needs to rebuild or refill calculation state across newly-loaded historical samples. Typical reaction: take a lease, `Clear()`, refill from new historical range, dispose lease — the front stays visible across the rebuild. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md#details) |
| | |
| interface | [IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md) |
| | Non-generic runtime contract used by the platform, catalogue, and controller. Indicator authors should derive from HeatmapIndicator instead of implementing this interface directly. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#details) |
| | |
| interface | [IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) |
| | Live runtime context exposed to a heatmap indicator. Unlike the v1 `HeatmapIndicatorContext` record (snapshot at reset), this is a read-only interface whose properties reflect the host's current state at every read. Indicators query it on demand — there are no "context changed" events because the most volatile field (Viewport) updates every render frame and a cross-thread notification per change would dominate the cost. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md#details) |
| | |
| interface | [IHeatmapIndicatorRenderInstance](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md) |
| | Read-only renderer view of one live indicator instance owned by the heatmap controller. The renderer (Skia / Vulkan overlay layer) consumes this surface to walk every visible instance and read its State per frame; the controller's richer `IHeatmapIndicatorInstance` is host / view-model facing and lives in a higher-level project the renderer cannot reference. Splitting the renderer-only surface here keeps the dependency direction acyclic (renderer -> indicators -> rendering primitives). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md#details) |
| | |
| interface | [IHeatmapIndicatorRenderSource](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md) |
| | Renderer-facing handle on the live indicator set. Implemented by the platform's heatmap controller (which already owns instance lifecycle and the live IHeatmapIndicatorContext) and consumed by the renderer overlay. Replaces the v1 `HeatmapIndicatorsSnapshot` pull model: the renderer enumerates Instances directly and pulls per-instance state via IHeatmapIndicatorRenderInstance.State. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md#details) |
| | |
| interface | [IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) |
| | Runtime handle the platform passes to indicators at warm-up / reset time. Lets the indicator drive its own re-warm or full state reset from any of its async methods. The handle is rebound on every reset, so indicators MUST NOT retain a runtime reference across IHeatmapIndicator.OnStateResetNotificationAsync calls. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md#details) |
| | |
| interface | [IHeatmapPlatformResettableVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md) |
| | Platform-owned extension for clearing an indicator state without going through an author-visible update lease. Indicator authors should use IHeatmapVisualState.BeginUpdate instead. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md#details) |
| | |
| interface | [IHeatmapProfileConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapProfileConsumer.md) |
| | |
| interface | [IHeatmapSeriesLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) |
| | Per-series mutation surface inside the lease. Append, replace, clear, and trim operations are buffered to the back-stage and become visible to the renderer on lease disposal. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md#details) |
| | |
| interface | [IHeatmapSeriesStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md) |
| | Read-only handle to a series within an IHeatmapVisualStateNode. The renderer iterates committed samples through this interface; mutation goes through the lease (IHeatmapVisualLease.Series). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md#details) |
| | |
| interface | [IHeatmapTimerConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md) |
| | Optional capability: receive periodic timer ticks. Most indicators do not need this; only opt in if the indicator must do work on a wall clock rather than in response to incoming data — for example, to expire stale state at session boundaries when no trade tick has arrived to wake the indicator up. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md#details) |
| | |
| interface | [IHeatmapTradeTickConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTradeTickConsumer.md) |
| | |
| interface | [IHeatmapVisualLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) |
| | Per-visual mutation surface inside the lease. Style and presentation are mutable properties on the lease; series content is mutated through Series. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md#details) |
| | |
| interface | [IHeatmapVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) |
| | Persistent visual state owned by a single indicator instance and read by the renderer at frame rate. Created by the platform from the indicator's HeatmapIndicatorDescriptor and bound to the indicator via HeatmapIndicator.State; the same reference is valid for the entire lifetime of the runner. Resets (instrument switch, explicit `RequestStateResetAsync`) clear the content of every series but do NOT replace the state instance. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md#details) |
| | |
| interface | [IHeatmapVisualStateLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md) |
| | The exclusive write lease on an IHeatmapVisualState. Acquired via IHeatmapVisualState.BeginUpdate; disposing commits the back-stage to the front. A lease can only be used inside the calling stack frame — passing it across `await` points or to background tasks is a misuse pattern (the platform serialises every indicator callback on a single task, so all legitimate writes happen from inside one of those callbacks). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md#details) |
| | |
| interface | [IHeatmapVisualStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md) |
| | Read-only handle to a visual within an IHeatmapVisualState. Carries the descriptor metadata plus the list of series; mutation goes through the lease (IHeatmapVisualStateLease.Visual). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md#details) |
| | |
| interface | [IHeatmapWarmupIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapWarmupIndicator.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [HeatmapLeaseMisuseReason](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77) {
  [LeaseAlreadyHeld](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a68411a193efd76e1dc1868c0caa4a246)
, [WrongDescriptor](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a08ffe60f082cdfd78b4987426a9edf3a)
, [UnknownVisual](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a7cb57e8f1aae0512cd57067c6024d127)
, [WrongVisual](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a8015bf177909b7be870850862fd978f9)
,
  [UnknownSeries](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a51998a3e1dbdae3bfcb7817b7074df89)
, [MissingValueProjection](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77a0092d89ce4ac97f449278d4680e14ada)

 } |
| | Why a HeatmapLeaseMisuseException was thrown. Lets callers distinguish misuse classes without parsing the message text. [More...](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77) |
| | |

| Functions | |
| --- | --- |
| readonly record struct | [HeatmapViewport](./namespaceATAS_1_1Indicators_1_1Heatmap.md#a209281643599139f1d052d3e7be8e646) (long StartTimeNanos, long EndTimeNanos, int PixelWidth) |
| | A rectangular slice of the heatmap timeline currently visible to the user. Carried by IHeatmapIndicatorContext.Viewport. PixelWidth is the rendered horizontal extent in physical pixels (DPI-corrected) — useful for choosing decimation step. |
| | |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)HeatmapLeaseMisuseReason

| enum [ATAS.Indicators.Heatmap.HeatmapLeaseMisuseReason](./namespaceATAS_1_1Indicators_1_1Heatmap.md#ab4c1dff890f961d18eab0c2039fb3d77) |
| --- |

Why a HeatmapLeaseMisuseException was thrown. Lets callers distinguish misuse classes without parsing the message text.

| Enumerator | |
| --- | --- |
| LeaseAlreadyHeld | IHeatmapVisualState.BeginUpdate was called while a previous lease was still in flight. The platform serialises every callback on a single per-instance task, so this almost always means the indicator passed the lease across an `await` point or to a background task. |
| WrongDescriptor | A HeatmapIndicatorVisualHandle minted from one descriptor was presented to a state built from a different descriptor. |
| UnknownVisual | A handle's visual id is not present in the state — typically a stale handle that survived a descriptor rebuild. |
| WrongVisual | A HeatmapIndicatorSeriesHandle was presented to the wrong visual lease. |
| UnknownSeries | A series handle's id is not present on the visual — typically a stale handle from a previous descriptor revision. |
| MissingValueProjection | A series handle was minted via the decimal fast-path (HeatmapIndicatorVisualHandle.Series(string,OFT.Rendering.Heatmap.HeatmapIndicatorSeriesRole,OFT.Rendering.Heatmap.HeatmapIndicatorValueKind,OFT.Rendering.Heatmap.HeatmapIndicatorVisualStyle?,string?,string?)) but the runtime `TValue` is not decimal. Use the generic `Series(..., valueProjection)` overload to bind a projection at descriptor build time. |

## Function Documentation

## [◆](https://docs.atas.net/en/)HeatmapViewport()

| readonly record struct ATAS.Indicators.Heatmap.HeatmapViewport | ( | long | StartTimeNanos, |
| --- | --- | --- | --- |
| | | long | EndTimeNanos, |
| | | int | PixelWidth |
| | ) | | |

A rectangular slice of the heatmap timeline currently visible to the user. Carried by IHeatmapIndicatorContext.Viewport. PixelWidth is the rendered horizontal extent in physical pixels (DPI-corrected) — useful for choosing decimation step.

Span of the viewport in nanoseconds (>= 0).
