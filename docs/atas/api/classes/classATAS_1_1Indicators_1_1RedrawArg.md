# ATAS.Indicators.RedrawArg Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1RedrawArg.html

Represents the arguments for requesting a redraw of a chart.
 [More...](./classATAS_1_1Indicators_1_1RedrawArg.md#details)

| Public Member Functions | |
| --- | --- |
| | [RedrawArg](./classATAS_1_1Indicators_1_1RedrawArg.md#af56de89f2b6c69712b14e81f487f1b6e) ([Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) redrawRegion) |
| | Initializes a new instance of the RedrawArg class with the specified redraw region. |
| | |

| Properties | |
| --- | --- |
| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | [RedrawRegion](./classATAS_1_1Indicators_1_1RedrawArg.md#aca9310eb79a94d82b4d3b2cf5beb6258)`[get, set]` |
| | Gets or sets the region to redraw on the chart. |
| | |
| bool | [ForceRedraw](./classATAS_1_1Indicators_1_1RedrawArg.md#ac8f848b7f89a7b60619e7d2ebe2099c1)`[get, set]` |
| | Gets or sets a value indicating whether the chart should be redrawn with user-interacted settings of frames per second (FPS). Should be used only if it is really needed, otherwise it could lead to performance issues. |
| | |

## Detailed Description

Represents the arguments for requesting a redraw of a chart.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)RedrawArg()

| ATAS.Indicators.RedrawArg.RedrawArg | ( | [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | redrawRegion | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the RedrawArg class with the specified redraw region.

Parameters

| redrawRegion | The region to redraw on the chart. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)ForceRedraw

| bool ATAS.Indicators.RedrawArg.ForceRedraw |
| --- |

getset

Gets or sets a value indicating whether the chart should be redrawn with user-interacted settings of frames per second (FPS). Should be used only if it is really needed, otherwise it could lead to performance issues.

## [◆](https://docs.atas.net/en/)RedrawRegion

| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) ATAS.Indicators.RedrawArg.RedrawRegion |
| --- |

getset

Gets or sets the region to redraw on the chart.

The documentation for this class was generated from the following file:
- [RedrawArg.cs](../files/RedrawArg_8cs.md)
