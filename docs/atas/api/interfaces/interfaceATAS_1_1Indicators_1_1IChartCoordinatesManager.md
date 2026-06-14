# ATAS.Indicators.IChartCoordinatesManager Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.html

Interface for managing chart coordinates and scaling.
 [More...](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#details)

| Public Member Functions | |
| --- | --- |
| void | [ScrollPrice](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#aa3fe0ccf2a48e0a7c5eeb4fff10590e7) (int ticksCount) |
| | Scrolls the chart vertically by a specified number of ticks. |
| | |
| void | [ChangeRowsHeight](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#a57010a0e2e242ec52214000d6aaa65db) (decimal newHeight) |
| | Changes the height of the chart rows to a new specified height. |
| | |
| void | [StartScaling](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#a9a44faeb5fa141793c9fd176931d3549) (int mouseY) |
| | Starts a Y-axis scaling gesture and stores the initial range. |
| | |
| void | [EndScaling](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#a739ca3adaa26c3fca26df39acf3713fd) () |
| | Ends the active Y-axis scaling gesture. |
| | |
| void | [UpdateChartYScaleOnMouseMove](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#ac489dcc7a87c4f9f7035c5145e31a299) (int previousMouseY, int newMouseY) |
| | Changes the Y-axis scale of the chart on mouse move. |
| | |
| void | [UpdateChartYScaleOnMouseWheel](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#aec8a143ce4f33b5cfd98db149e775545) (int delta) |
| | Changes the Y-axis scale of the chart on mouse wheel. |
| | |
| void | [MoveChartUpAndDown](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#a0ec28cbfa0fb72272378663a35c29d25) (int offset) |
| | Moves the entire chart up or down by a specified offset. |
| | |
| void | [EnableAutoScale](./interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#ad50f2976267956b2a99e7d5c06ad9c79) () |
| | Enables auto-scaling for the chart to fit the data within the view. |
| | |

## Detailed Description

Interface for managing chart coordinates and scaling.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ChangeRowsHeight()

| void ATAS.Indicators.IChartCoordinatesManager.ChangeRowsHeight | ( | decimal | newHeight | ) | |
| --- | --- | --- | --- | --- | --- |

Changes the height of the chart rows to a new specified height.

Parameters

| newHeight | The new height to set for the chart rows. |
| --- | --- |

## [◆](https://docs.atas.net/en/)EnableAutoScale()

| void ATAS.Indicators.IChartCoordinatesManager.EnableAutoScale | ( | | ) | |
| --- | --- | --- | --- | --- |

Enables auto-scaling for the chart to fit the data within the view.

## [◆](https://docs.atas.net/en/)EndScaling()

| void ATAS.Indicators.IChartCoordinatesManager.EndScaling | ( | | ) | |
| --- | --- | --- | --- | --- |

Ends the active Y-axis scaling gesture.

## [◆](https://docs.atas.net/en/)MoveChartUpAndDown()

| void ATAS.Indicators.IChartCoordinatesManager.MoveChartUpAndDown | ( | int | offset | ) | |
| --- | --- | --- | --- | --- | --- |

Moves the entire chart up or down by a specified offset.

Parameters

| offset | The vertical offset to move the chart. Positive values move up; negative values move down. |
| --- | --- |

## [◆](https://docs.atas.net/en/)ScrollPrice()

| void ATAS.Indicators.IChartCoordinatesManager.ScrollPrice | ( | int | ticksCount | ) | |
| --- | --- | --- | --- | --- | --- |

Scrolls the chart vertically by a specified number of ticks.

Parameters

| ticksCount | The number of ticks to scroll. Positive values scroll up; negative values scroll down. |
| --- | --- |

## [◆](https://docs.atas.net/en/)StartScaling()

| void ATAS.Indicators.IChartCoordinatesManager.StartScaling | ( | int | mouseY | ) | |
| --- | --- | --- | --- | --- | --- |

Starts a Y-axis scaling gesture and stores the initial range.

Parameters

| mouseY | The initial mouse Y-coordinate. |
| --- | --- |

## [◆](https://docs.atas.net/en/)UpdateChartYScaleOnMouseMove()

| void ATAS.Indicators.IChartCoordinatesManager.UpdateChartYScaleOnMouseMove | ( | int | previousMouseY, |
| --- | --- | --- | --- |
| | | int | newMouseY |
| | ) | | |

Changes the Y-axis scale of the chart on mouse move.

Parameters

| previousMouseY | |
| --- | --- |
| newMouseY | |

## [◆](https://docs.atas.net/en/)UpdateChartYScaleOnMouseWheel()

| void ATAS.Indicators.IChartCoordinatesManager.UpdateChartYScaleOnMouseWheel | ( | int | delta | ) | |
| --- | --- | --- | --- | --- | --- |

Changes the Y-axis scale of the chart on mouse wheel.

Parameters

| delta | |
| --- | --- |

The documentation for this interface was generated from the following file:
- [IIndicatorContainer.cs](../files/IIndicatorContainer_8cs.md)
