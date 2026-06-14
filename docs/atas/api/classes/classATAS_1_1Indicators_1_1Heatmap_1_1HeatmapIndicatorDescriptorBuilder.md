# ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.html

Fluent builder that produces an immutable HeatmapIndicatorDescriptor alongside the typed visual / series handles required by the state builder. Single-shot: each builder yields exactly one descriptor via Done; further mutation throws.
 [More...](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#details)

| Public Member Functions | |
| --- | --- |
| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | [Visual](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a17aea60f0010ab3aa049b7d9e2a64ad7) (string visualId, HeatmapIndicatorVisualKind kind, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null, HeatmapIndicatorVisualPresentation? defaultPresentation=null) |
| | Add a visual of any kind. The kind-specific helpers (PriceLine, SubPanelScalar, …) are usually clearer; reach for this one when the kind is computed at runtime. |
| | |
| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | [PriceLine](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#aeadd85f26d9b54f487cc479cc64f2d6d) (string visualId, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null) |
| | |
| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | [ValueArea](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a240498847a0934829b29d07256905938) (string visualId, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null) |
| | |
| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | [LevelLine](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a28de1c1497c000f76c04a1a70fe8ea67) (string visualId, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null) |
| | |
| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | [SubPanelScalar](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a3c2f9cde935878f85aca6753513e62a8) (string visualId, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null, HeatmapIndicatorVisualPresentation? defaultPresentation=null) |
| | |
| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | [SubPanelPair](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a9648187f5768742e714caa237947238e) (string visualId, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null, HeatmapIndicatorVisualPresentation? defaultPresentation=null) |
| | |
| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | [Histogram](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#ad99082b7330f7049d3278888286d0776) (string visualId, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null, HeatmapIndicatorVisualPresentation? defaultPresentation=null) |
| | |
| HeatmapIndicatorDescriptor | [Done](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a050e277f0199be3ce13b2fa8fa4e9080) () |
| | Seal the builder and produce the immutable descriptor. The visual / series handles minted by this builder remain usable as state-builder inputs after Done; what becomes invalid is mutation (no more Visual calls, no more HeatmapIndicatorVisualHandle.Series calls). Single-shot: a second Done throws. |
| | |

## Detailed Description

Fluent builder that produces an immutable HeatmapIndicatorDescriptor alongside the typed visual / series handles required by the state builder. Single-shot: each builder yields exactly one descriptor via Done; further mutation throws.

private static readonly HeatmapIndicatorDescriptor _descriptor;

private static readonly [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) _panel;

private static readonly [HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) _value;

static MyIndicator()

{

 var build = [HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md).[Describe](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a2b55c67258a5aab37df9cbfad7b51522)("vendor.my-indicator", "My Indicator");

 _panel = build.[SubPanelScalar](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a3c2f9cde935878f85aca6753513e62a8)("my.panel", "My Panel");

 _value = _panel.[Series](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md#a0d010531237fb024e849b7a10f1c490a)<long>(

 "my.value",

 HeatmapIndicatorSeriesRole.Scalar,

 HeatmapIndicatorValueKind.Integer,

 metricId: "my.value");

 _descriptor = build.Done();

}

[ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.SubPanelScalar](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md#a3c2f9cde935878f85aca6753513e62a8)

HeatmapIndicatorVisualHandle SubPanelScalar(string visualId, string? label=null, HeatmapIndicatorVisualStyle? defaultStyle=null, HeatmapIndicatorVisualPresentation? defaultPresentation=null)

[ATAS.Indicators.Heatmap.HeatmapIndicatorSeriesHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md)

Strongly typed handle for a series within a visual. Returned from HeatmapIndicatorVisualHandle....

Definition HeatmapIndicatorSeriesHandle.cs:17

[ATAS.Indicators.Heatmap.HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md)

Strongly typed handle for a visual added to a descriptor via HeatmapIndicatorDescriptorBuilder....

Definition HeatmapIndicatorVisualHandle.cs:15

[ATAS.Indicators.Heatmap.HeatmapIndicatorVisualHandle.Series](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md#a0d010531237fb024e849b7a10f1c490a)

HeatmapIndicatorSeriesHandle< decimal > Series(string seriesId, HeatmapIndicatorSeriesRole role, HeatmapIndicatorValueKind valueKind, HeatmapIndicatorVisualStyle? defaultStyle=null, string? metricId=null, string? unit=null)

Decimal fast path: the series stores decimal samples and no projection is required....

Definition HeatmapIndicatorVisualHandle.cs:118

[ATAS.Indicators.Heatmap.HeatmapIndicator](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md)

Author-facing entry points for the heatmap indicator API. The non-generic HeatmapIndicator coexists w...

Definition IHeatmapIndicator.cs:113

[ATAS.Indicators.Heatmap.HeatmapIndicator.Describe](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md#a2b55c67258a5aab37df9cbfad7b51522)

static HeatmapIndicatorDescriptorBuilder Describe(string indicatorId, string? label=null)

Begin describing a heatmap indicator. The returned builder yields visual and series handles that the ...

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Done()

| HeatmapIndicatorDescriptor ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.Done | ( | | ) | |
| --- | --- | --- | --- | --- |

Seal the builder and produce the immutable descriptor. The visual / series handles minted by this builder remain usable as state-builder inputs after Done; what becomes invalid is mutation (no more Visual calls, no more HeatmapIndicatorVisualHandle.Series<TValue> calls). Single-shot: a second Done throws.

## [◆](https://docs.atas.net/en/)Histogram()

| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.Histogram | ( | string | visualId, |
| --- | --- | --- | --- |
| | | string? | label = `null`, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null`, |
| | | HeatmapIndicatorVisualPresentation? | defaultPresentation = `null` |
| | ) | | |

## [◆](https://docs.atas.net/en/)LevelLine()

| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.LevelLine | ( | string | visualId, |
| --- | --- | --- | --- |
| | | string? | label = `null`, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null` |
| | ) | | |

## [◆](https://docs.atas.net/en/)PriceLine()

| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.PriceLine | ( | string | visualId, |
| --- | --- | --- | --- |
| | | string? | label = `null`, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null` |
| | ) | | |

## [◆](https://docs.atas.net/en/)SubPanelPair()

| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.SubPanelPair | ( | string | visualId, |
| --- | --- | --- | --- |
| | | string? | label = `null`, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null`, |
| | | HeatmapIndicatorVisualPresentation? | defaultPresentation = `null` |
| | ) | | |

## [◆](https://docs.atas.net/en/)SubPanelScalar()

| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.SubPanelScalar | ( | string | visualId, |
| --- | --- | --- | --- |
| | | string? | label = `null`, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null`, |
| | | HeatmapIndicatorVisualPresentation? | defaultPresentation = `null` |
| | ) | | |

## [◆](https://docs.atas.net/en/)ValueArea()

| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.ValueArea | ( | string | visualId, |
| --- | --- | --- | --- |
| | | string? | label = `null`, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null` |
| | ) | | |

## [◆](https://docs.atas.net/en/)Visual()

| [HeatmapIndicatorVisualHandle](./classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder.Visual | ( | string | visualId, |
| --- | --- | --- | --- |
| | | HeatmapIndicatorVisualKind | kind, |
| | | string? | label = `null`, |
| | | HeatmapIndicatorVisualStyle? | defaultStyle = `null`, |
| | | HeatmapIndicatorVisualPresentation? | defaultPresentation = `null` |
| | ) | | |

Add a visual of any kind. The kind-specific helpers (PriceLine, SubPanelScalar, …) are usually clearer; reach for this one when the kind is computed at runtime.

The documentation for this class was generated from the following file:
- [HeatmapIndicatorDescriptorBuilder.cs](../files/HeatmapIndicatorDescriptorBuilder_8cs.md)
