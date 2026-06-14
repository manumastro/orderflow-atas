# ATAS.Indicators.IIndicatorContainer Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.html

Interface for an indicator container that holds indicator-related information and methods.
 [More...](./interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md#details)

Inheritance diagram for ATAS.Indicators.IIndicatorContainer:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.IIndicatorContainer:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| int | [GetYByValue](./interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md#a33d0ff361b033bac0e3e6ffaa5b61c3f) (decimal value) |
| | Gets the Y-coordinate value corresponding to the specified decimal value within the indicator container. |
| | |

| Properties | |
| --- | --- |
| decimal | [Maximum](./interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md#a9a1ce92ce2ad4d8d9aadb01254782aad)`[get]` |
| | Gets the maximum value within the indicator container. |
| | |
| decimal | [Minimum](./interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md#a5ced61d12e884c605416931b19616159)`[get]` |
| | Gets the minimum value within the indicator container. |
| | |
| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | [RelativeRegion](./interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md#a0cab95d79aa0f7174011782aa73bdc12)`[get, set]` |
| | Gets or sets the relative region of the indicator container. |
| | |
| - Properties inherited from [ATAS.Indicators.IContainer](./interfaceATAS_1_1Indicators_1_1IContainer.md) | |
| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | [Region](./interfaceATAS_1_1Indicators_1_1IContainer.md#a2a127b6200f2b7e5e7ecbe090e047869)`[get]` |
| | Gets the rectangular region defined by the container. |
| | |

| Events | |
| --- | --- |
| Action | [OnChange](./interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md#a9703a3e28c5a384fd4571aad05ea6dc0) |
| | Event triggered when the indicator container's region changes. |
| | |

## Detailed Description

Interface for an indicator container that holds indicator-related information and methods.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetYByValue()

| int ATAS.Indicators.IIndicatorContainer.GetYByValue | ( | decimal | value | ) | |
| --- | --- | --- | --- | --- | --- |

Gets the Y-coordinate value corresponding to the specified decimal value within the indicator container.

Parameters

| value | The decimal value to get the Y-coordinate for. |
| --- | --- |

ReturnsThe Y-coordinate value corresponding to the specified decimal value.

## Property Documentation

## [◆](https://docs.atas.net/en/)Maximum

| decimal ATAS.Indicators.IIndicatorContainer.Maximum |
| --- |

get

Gets the maximum value within the indicator container.

## [◆](https://docs.atas.net/en/)Minimum

| decimal ATAS.Indicators.IIndicatorContainer.Minimum |
| --- |

get

Gets the minimum value within the indicator container.

## [◆](https://docs.atas.net/en/)RelativeRegion

| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) ATAS.Indicators.IIndicatorContainer.RelativeRegion |
| --- |

getset

Gets or sets the relative region of the indicator container.

## Event Documentation

## [◆](https://docs.atas.net/en/)OnChange

| Action ATAS.Indicators.IIndicatorContainer.OnChange |
| --- |

Event triggered when the indicator container's region changes.

The documentation for this interface was generated from the following file:
- [IIndicatorContainer.cs](../files/IIndicatorContainer_8cs.md)
