# ATAS.Indicators.Heatmap.HeatmapIndicator< TSettings > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.html

Author-facing entry points for the heatmap indicator API. The non-generic `HeatmapIndicator` coexists with the generic HeatmapIndicator<TSettings> base class — different arities disambiguate them at the type system level.
 [More...](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#details)

Inheritance diagram for ATAS.Indicators.Heatmap.HeatmapIndicator< TSettings >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Heatmap.HeatmapIndicator< TSettings >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| virtual ValueTask | [ConfigureAsync](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a9518ec4929f208950819f3a10ad9fdac) (TSettings settings, CancellationToken cancellationToken) |
| | Apply user-edited settings. The base class has already stored settings in the inherited Settings property by the time this runs, so the default implementation does nothing. Override only when the indicator needs to react to a settings change — e.g. invalidate an accumulator, request a state reset on calc-mode change, or rebuild visual presentation under a lease. Implementations do not need to assign the supplied settings to a private field; read them via Settings. |
| | |
| ValueTask | [ConfigureAsync](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aeeb8fb1ea9c82e04bd6875fda08fdc92) (object settings, CancellationToken cancellationToken) |
| | Apply user-edited settings. The runtime type of settings must be assignable to SettingsType. |
| | |
| ValueTask | [OnStateResetNotificationAsync](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#ac594c002ea8ac22ae362d018735899f6) ([IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) context, [IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) runtime, CancellationToken cancellationToken) |
| | Notify the indicator that the platform has cleared State. Called when the active context changes (instrument, session, time zone) or when the indicator itself called IHeatmapIndicatorRuntime.RequestStateResetAsync. |
| | |

| Static Public Member Functions | |
| --- | --- |
| static [HeatmapIndicatorDescriptorBuilder](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) | [Describe](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a2b55c67258a5aab37df9cbfad7b51522) (string indicatorId, string? label=null) |
| | Begin describing a heatmap indicator. The returned builder yields visual and series handles that the author captures into `static readonly` fields; the runtime IHeatmapVisualState that backs the indicator is materialised lazily by the base class. |
| | |
| static [HeatmapIndicatorDescriptorBuilder](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) | [Describe](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a861c922130ca85f5f8a332f379335d70) (string? labelOverride=null) |
| | Typed overload that resolves the indicator id and label from the HeatmapIndicatorAttribute applied to TSelf . Equivalent to HeatmapIndicator.Describe but callable from any context (tests, factories) — author code inside the static constructor of an indicator should prefer the inherited base-class helper, which yields the shorter `Describe()` spelling. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual ValueTask | [OnStateResetCoreAsync](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ac7e4a4eb7fedec2e6bedd3d303a3861d) ([IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) context, [IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) runtime, CancellationToken cancellationToken) |
| | Override to react to a state-reset notification. The base class has already saved context and runtime into Context / Runtime by the time this runs, so most overrides only need to clear their own indicator-internal accumulators (fields, dictionaries, etc.) and don't need either argument. The arguments are still surfaced for indicators that want to drive a re-warm or capture context-derived state synchronously. |
| | |
| void | [UpdateVisual](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a6c657f5fc8e352017ad7f8e75b4483d1) ([HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) visual, Action update) |
| | Mutate one visual under a short-lived state lease. Use this for style/presentation-only updates or when a callback needs one visual lease and no cross-visual coordination. |
| | |
| void | [UpdateSeries](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ae6cdd8902b174f1347d627690b505731) ([HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) series, Action > update) |
| | Mutate one series under a short-lived state lease. Use this for simple append/clear/trim operations where the visual style and presentation do not need to change in the same transaction. |
| | |
| void | [ClearSeries](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#af701f0405034dc73621aa898c853709c) (params [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md)[] series) |
| | Clear a homogeneous set of series under a short-lived state lease. |
| | |

| Static Protected Member Functions | |
| --- | --- |
| static void | [ClearSeries](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a630acdc4da04fea6d940612bed21eea4) ([IHeatmapVisualLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) visualLease, params [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md)[] series) |
| | Clear a homogeneous set of series inside an existing visual lease. Useful for rebuilds that clear and refill multiple related series in one commit. |
| | |
| static [HeatmapIndicatorDescriptorBuilder](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) | [Describe](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a861c922130ca85f5f8a332f379335d70) (string? labelOverride=null) |
| | Begin describing the indicator using metadata from the HeatmapIndicatorAttribute applied to TSelf . Authors call this from the static constructor of the deriving type, which keeps the type id declared exactly once (in the attribute) and removes the typo-mismatch risk between discovery metadata and the descriptor. |
| | |
| static List | [OrderedTicks](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a728657d7475f9ac94c8c6a66276c3054) (HeatmapTickBatch ticks) |
| | Return eligible ticks from a pooled batch ordered by timestamp. The returned list is detached from the batch and may be kept after the callback returns. |
| | |
| static List | [OrderedTicks](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ab9bc3fcd60d29be103d72499d144aab9) (HeatmapTickBatch ticks, Predicate predicate) |
| | Return eligible ticks from a pooled batch ordered by timestamp. The returned list is detached from the batch and may be kept after the callback returns. |
| | |
| static List | [OrderedTicks](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a85836ab8a94f25e818feb4c8b6e7040c) (IEnumerable ticks) |
| | Return eligible ticks from an enumerable ordered by timestamp. |
| | |
| static List | [OrderedTicks](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ad4d612686269b9eb6c46299a7f1d38d8) (IEnumerable ticks, Predicate predicate) |
| | Return eligible ticks from an enumerable ordered by timestamp. |
| | |
| static bool | [HasValidTimestampPriceVolume](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ad63af4a6fa937ba8e46280aab5203029) (HeatmapTradeTick tick) |
| | Common filter for price/volume indicators: timestamp, price, and volume must all be positive. |
| | |
| static bool | [HasValidBuySellVolume](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a0105467bb3343eaef0b6d908adfae865) (HeatmapTradeTick tick) |
| | Common filter for buy/sell volume indicators: timestamp and volume must be positive and direction must be buy or sell. |
| | |

| Properties | |
| --- | --- |
| abstract HeatmapIndicatorDescriptor | [Descriptor](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ae079a3ebe7a405325c9a2dacbaa5e87d)`[get]` |
| | Static type-level metadata. Identifies the indicator type, declares its placement (overlay vs sub-panel), visual roles, and series shape. Must not change between calls. |
| | |
| Type | [SettingsType](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a0d2130e3535253620678f832b55ed343)`[get]` |
| | CLR type of the settings DTO this indicator accepts. Used by the catalogue to materialise persisted settings into the right shape before calling ConfigureAsync. |
| | |
| TSettings | [Settings](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#ac0472dd2a41fcd18d2526cd7a7a39133)`[get]` |
| | Most recent settings DTO applied through ConfigureAsync. The base class assigns this property before invoking the typed ConfigureAsync(TSettings, CancellationToken) override, so derived indicators can read the current settings directly without caching them into a private field. Defaults to a fresh TSettings instance until the platform calls ConfigureAsync for the first time. |
| | |
| [IHeatmapVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) | [State](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a260fc71cc91bb756db06b76dce08d78f)`[get]` |
| | Persistent visual state. Lazily created on first access from the indicator's own Descriptor; the platform may pre-create it via IHeatmapIndicator.State at instance construction time so the reference is stable from the first lifecycle callback onward. |
| | |
| [IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md)? | [Context](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a0018eac0a6100d278496312ef8cefc60)`[get]` |
| | Most recent context handed in by the platform via IHeatmapIndicator.OnStateResetNotificationAsync. Null only before the very first reset notification — the platform always emits a reset before any data callback, so by the time IHeatmapTradeTickConsumer.ProcessTicksAsync / IHeatmapWarmupIndicator.WarmUpAsync / IHeatmapProfileConsumer.ProcessProfileAsync fires this is non-null. |
| | |
| [IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md)? | [Runtime](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a677facbebb5d0bacf9ce4b69c03c7c0c)`[get]` |
| | Most recent runtime handed in by the platform via IHeatmapIndicator.OnStateResetNotificationAsync. Use to call IHeatmapIndicatorRuntime.RequestReWarmAsync / IHeatmapIndicatorRuntime.RequestStateResetAsync. The reference is rebound on every reset, so do not cache it across reset boundaries — read this property each time. |
| | |
| - Properties inherited from [ATAS.Indicators.Heatmap.IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md) | |
| HeatmapIndicatorDescriptor | [Descriptor](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aa40c3e70a852b72744531b764eedfd6c)`[get]` |
| | Static type-level metadata. Identifies the indicator type, declares its placement (overlay vs sub-panel), visual roles, and series shape. Must not change between calls. |
| | |
| Type | [SettingsType](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#af6683f22968821e5f4155dbfde0e9a48)`[get]` |
| | CLR type of the settings DTO this indicator accepts. Used by the catalogue to materialise persisted settings into the right shape before calling ConfigureAsync. |
| | |
| [IHeatmapVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) | [State](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aecb311cf43701325c593441454ed5cbe)`[get]` |
| | Persistent visual state read by the renderer. Created by the platform from Descriptor at instance construction time; the same reference is returned for the entire instance lifetime. Resets clear content but never replace the reference. |
| | |

## Detailed Description

Author-facing entry points for the heatmap indicator API. The non-generic `HeatmapIndicator` coexists with the generic HeatmapIndicator<TSettings> base class — different arities disambiguate them at the type system level.

Strongly typed base for indicator authors. Bridges to the non-generic IHeatmapIndicator with a single explicit cast, so derived classes work against typed settings.

Type Constraints

| TSettings | : | class | |
| --- | --- | --- | --- |
| TSettings | : | new() | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ClearSeries() [1/2]

| static void [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).ClearSeries | ( | [IHeatmapVisualLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) | visualLease, |
| --- | --- | --- | --- |
| | | params [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md)[] | series |
| | ) | | |

staticprotected

Clear a homogeneous set of series inside an existing visual lease. Useful for rebuilds that clear and refill multiple related series in one commit.

## [◆](https://docs.atas.net/en/)ClearSeries() [2/2]

| void [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).ClearSeries | ( | params [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md)[] | series | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Clear a homogeneous set of series under a short-lived state lease.

## [◆](https://docs.atas.net/en/)ConfigureAsync()

| virtual ValueTask [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).ConfigureAsync | ( | TSettings | settings, |
| --- | --- | --- | --- |
| | | CancellationToken | cancellationToken |
| | ) | | |

virtual

Apply user-edited settings. The base class has already stored settings in the inherited Settings property by the time this runs, so the default implementation does nothing. Override only when the indicator needs to react to a settings change — e.g. invalidate an accumulator, request a state reset on calc-mode change, or rebuild visual presentation under a lease. Implementations do not need to assign the supplied settings to a private field; read them via Settings.

## [◆](https://docs.atas.net/en/)Describe()

| static [HeatmapIndicatorDescriptorBuilder](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).Describe | ( | string | indicatorId, |
| --- | --- | --- | --- |
| | | string? | label = `null` |
| | ) | | |

static

Begin describing a heatmap indicator. The returned builder yields visual and series handles that the author captures into `static readonly` fields; the runtime IHeatmapVisualState that backs the indicator is materialised lazily by the base class.

## [◆](https://docs.atas.net/en/)Describe() [1/2]

| static [HeatmapIndicatorDescriptorBuilder](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).[Describe](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a2b55c67258a5aab37df9cbfad7b51522) | ( | string? | labelOverride = `null` | ) | |
| --- | --- | --- | --- | --- | --- |

static

Typed overload that resolves the indicator id and label from the HeatmapIndicatorAttribute applied to TSelf . Equivalent to HeatmapIndicator<TSettings>.Describe<TSelf> but callable from any context (tests, factories) — author code inside the static constructor of an indicator should prefer the inherited base-class helper, which yields the shorter `Describe()` spelling.

Type Constraints

| TSelf | : | IHeatmapIndicator | |
| --- | --- | --- | --- |
| TSelf | : | DescribeFromAttribute | |
| TSelf | : | typeof | |
| TSelf | : | TSelf | |
| TSelf | : | labelOverride | |

## [◆](https://docs.atas.net/en/)Describe() [2/2]

| static [HeatmapIndicatorDescriptorBuilder](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).[Describe](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a2b55c67258a5aab37df9cbfad7b51522) | ( | string? | labelOverride = `null` | ) | |
| --- | --- | --- | --- | --- | --- |

staticprotected

Begin describing the indicator using metadata from the HeatmapIndicatorAttribute applied to TSelf . Authors call this from the static constructor of the deriving type, which keeps the type id declared exactly once (in the attribute) and removes the typo-mismatch risk between discovery metadata and the descriptor.

Parameters

| labelOverride | When non-null, used as the descriptor label instead of the attribute's HeatmapIndicatorAttribute.DisplayName. |
| --- | --- |

Type Constraints

| TSelf | : | IHeatmapIndicator | |
| --- | --- | --- | --- |
| TSelf | : | HeatmapIndicator.DescribeFromAttribute | |
| TSelf | : | typeof | |
| TSelf | : | TSelf | |
| TSelf | : | labelOverride | |

## [◆](https://docs.atas.net/en/)HasValidBuySellVolume()

| static bool [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).HasValidBuySellVolume | ( | HeatmapTradeTick | tick | ) | |
| --- | --- | --- | --- | --- | --- |

staticprotected

Common filter for buy/sell volume indicators: timestamp and volume must be positive and direction must be buy or sell.

## [◆](https://docs.atas.net/en/)HasValidTimestampPriceVolume()

| static bool [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).HasValidTimestampPriceVolume | ( | HeatmapTradeTick | tick | ) | |
| --- | --- | --- | --- | --- | --- |

staticprotected

Common filter for price/volume indicators: timestamp, price, and volume must all be positive.

## [◆](https://docs.atas.net/en/)OnStateResetCoreAsync()

| virtual ValueTask [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).OnStateResetCoreAsync | ( | [IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) | context, |
| --- | --- | --- | --- |
| | | [IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) | runtime, |
| | | CancellationToken | cancellationToken |
| | ) | | |

protectedvirtual

Override to react to a state-reset notification. The base class has already saved context and runtime into Context / Runtime by the time this runs, so most overrides only need to clear their own indicator-internal accumulators (fields, dictionaries, etc.) and don't need either argument. The arguments are still surfaced for indicators that want to drive a re-warm or capture context-derived state synchronously.

## [◆](https://docs.atas.net/en/)OrderedTicks() [1/4]

| static List [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).OrderedTicks | ( | HeatmapTickBatch | ticks | ) | |
| --- | --- | --- | --- | --- | --- |

staticprotected

Return eligible ticks from a pooled batch ordered by timestamp. The returned list is detached from the batch and may be kept after the callback returns.

## [◆](https://docs.atas.net/en/)OrderedTicks() [2/4]

| static List [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).OrderedTicks | ( | HeatmapTickBatch | ticks, |
| --- | --- | --- | --- |
| | | Predicate | predicate |
| | ) | | |

staticprotected

Return eligible ticks from a pooled batch ordered by timestamp. The returned list is detached from the batch and may be kept after the callback returns.

## [◆](https://docs.atas.net/en/)OrderedTicks() [3/4]

| static List [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).OrderedTicks | ( | IEnumerable | ticks | ) | |
| --- | --- | --- | --- | --- | --- |

staticprotected

Return eligible ticks from an enumerable ordered by timestamp.

## [◆](https://docs.atas.net/en/)OrderedTicks() [4/4]

| static List [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).OrderedTicks | ( | IEnumerable | ticks, |
| --- | --- | --- | --- |
| | | Predicate | predicate |
| | ) | | |

staticprotected

Return eligible ticks from an enumerable ordered by timestamp.

## [◆](https://docs.atas.net/en/)UpdateSeries()

| void [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).UpdateSeries | ( | [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) | series, |
| --- | --- | --- | --- |
| | | Action > | update |
| | ) | | |

protected

Mutate one series under a short-lived state lease. Use this for simple append/clear/trim operations where the visual style and presentation do not need to change in the same transaction.

## [◆](https://docs.atas.net/en/)UpdateVisual()

| void [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).UpdateVisual | ( | [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | visual, |
| --- | --- | --- | --- |
| | | Action | update |
| | ) | | |

protected

Mutate one visual under a short-lived state lease. Use this for style/presentation-only updates or when a callback needs one visual lease and no cross-visual coordination.

## Property Documentation

## [◆](https://docs.atas.net/en/)Context

| [IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md)? [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).Context |
| --- |

getprotected

Most recent context handed in by the platform via IHeatmapIndicator.OnStateResetNotificationAsync. Null only before the very first reset notification — the platform always emits a reset before any data callback, so by the time IHeatmapTradeTickConsumer.ProcessTicksAsync / IHeatmapWarmupIndicator.WarmUpAsync / IHeatmapProfileConsumer.ProcessProfileAsync fires this is non-null.

## [◆](https://docs.atas.net/en/)Descriptor

| abstract HeatmapIndicatorDescriptor [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).Descriptor |
| --- |

get

Static type-level metadata. Identifies the indicator type, declares its placement (overlay vs sub-panel), visual roles, and series shape. Must not change between calls.

Implements [ATAS.Indicators.Heatmap.IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aa40c3e70a852b72744531b764eedfd6c).

## [◆](https://docs.atas.net/en/)Runtime

| [IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md)? [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).Runtime |
| --- |

getprotected

Most recent runtime handed in by the platform via IHeatmapIndicator.OnStateResetNotificationAsync. Use to call IHeatmapIndicatorRuntime.RequestReWarmAsync / IHeatmapIndicatorRuntime.RequestStateResetAsync. The reference is rebound on every reset, so do not cache it across reset boundaries — read this property each time.

## [◆](https://docs.atas.net/en/)Settings

| TSettings [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).Settings |
| --- |

getprotected

Most recent settings DTO applied through ConfigureAsync. The base class assigns this property before invoking the typed ConfigureAsync(TSettings, CancellationToken) override, so derived indicators can read the current settings directly without caching them into a private field. Defaults to a fresh TSettings instance until the platform calls ConfigureAsync for the first time.

## [◆](https://docs.atas.net/en/)SettingsType

| Type [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).SettingsType |
| --- |

get

CLR type of the settings DTO this indicator accepts. Used by the catalogue to materialise persisted settings into the right shape before calling ConfigureAsync.

Implements [ATAS.Indicators.Heatmap.IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#af6683f22968821e5f4155dbfde0e9a48).

## [◆](https://docs.atas.net/en/)State

| [IHeatmapVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) [ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).State |
| --- |

get

Persistent visual state. Lazily created on first access from the indicator's own Descriptor; the platform may pre-create it via IHeatmapIndicator.State at instance construction time so the reference is stable from the first lifecycle callback onward.

Implements [ATAS.Indicators.Heatmap.IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md#aecb311cf43701325c593441454ed5cbe).

The documentation for this class was generated from the following files:
- [HeatmapIndicator.cs](../files/HeatmapIndicator_8cs.md)

- [IHeatmapIndicator.cs](../files/IHeatmapIndicator_8cs.md)
