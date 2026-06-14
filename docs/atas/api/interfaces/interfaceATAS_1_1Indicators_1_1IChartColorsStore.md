# ATAS.Indicators.IChartColorsStore Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IChartColorsStore.html

Interface for accessing colors and pens used in a chart's rendering.
 [More...](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#details)

| Public Member Functions | |
| --- | --- |
| void | [DrawBackground](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#ab899dcbf49d05b53311f3c8a80362330) (RenderContext context, [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) region) |
| | Draws the background of the chart within the specified region using the provided render context. |
| | |

| Properties | |
| --- | --- |
| RenderPen | [Grid](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a95bf2379bff37e838fd61e800bd081c8)`[get]` |
| | Gets the pen used for drawing the grid lines. |
| | |
| bool | [HorizontalGridIsEnabled](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#adb3621dec8457c3ab6b93d8682d7b947)`[get]` |
| | Gets a value indicating whether horizontal grid lines are enabled. |
| | |
| Color | [BaseBackgroundColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a1d69f6a6f7437fb1b3f535ce8b29fbd7)`[get]` |
| | Gets the base background color of the chart. |
| | |
| Color | [UpCandleColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a37130c7ae66e8a53f918dc09b641383f)`[get]` |
| | Gets the color used for up (bullish) candles. |
| | |
| Color | [DownCandleColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a423eabf9ec20bd04d8bc9936264defd5)`[get]` |
| | Gets the color used for down (bearish) candles. |
| | |
| Color | [AxisTextColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a5fb53d3dc47eb44297cc239e802b51b8)`[get]` |
| | Gets the color used for axis text. |
| | |
| RenderPen | [PaneSeparators](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#ae95e41ed683a8851db5067d2915008d3)`[get]` |
| | Gets the pen used for drawing pane separators. |
| | |
| RenderPen | [NewSessionPen](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#ac014f30399da1e8a5abc6f95c942266d)`[get]` |
| | Gets the pen used for drawing the start of a new session. |
| | |
| RenderPen | [UpBarPen](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#aba7a0fe2a44e189350f97fcc9c1c98a6)`[get]` |
| | Gets the pen used for drawing up bars. |
| | |
| RenderPen | [DownBarPen](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#ab67364a4a2ceacf3f8a8ed74fa5a86d9)`[get]` |
| | Gets the pen used for drawing down bars. |
| | |
| RenderPen | [DojiBarPen](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a8548e4bee1adb0f7138698ab010b1cf0)`[get]` |
| | Gets the pen used for drawing Doji bars. |
| | |
| RenderPen | [BarBorderPen](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#aab0d3c2c869bee2c72fff71c60b775fd)`[get]` |
| | Gets the pen used for drawing bar borders. |
| | |
| Color | [FootprintBidColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a9074c2ee205f0a1d21b2f12ba4c5db3f)`[get]` |
| | Gets the color used for the bid volume in a footprint chart. |
| | |
| Color | [FootprintAskColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a68c2255c2e878af1d2a9b80cc335c668)`[get]` |
| | Gets the color used for the ask volume in a footprint chart. |
| | |
| Color | [FootprintMainColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a61bb16973fb728f20e6f321799328054)`[get]` |
| | Gets the main color used in a footprint chart. |
| | |
| Color | [FootprintTextColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a05a0e764f31c23461040fe12fa584c3a)`[get]` |
| | Gets the color used for text in a footprint chart. |
| | |
| Color | [FootprintMaximumVolumeTextColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a2bcf60cae0615db0b4b3342637c50c2c)`[get]` |
| | Gets the color used for the maximum volume text in a footprint chart. |
| | |
| Color | [PositiveColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#ac015f8d29049cdaed7534f3f65599921)`[get]` |
| | Gets the color used for positive values. |
| | |
| Color | [NegativeColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a8af6ffd29ff5779ff028c677268ef25c)`[get]` |
| | Gets the color used for negative values. |
| | |
| Color | [NeutralColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#aff643818593fbaa91b62731dc7311b9b)`[get]` |
| | Gets the color used for neutral values. |
| | |
| Color | [MouseBackground](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a68b3dd83e89e5025c39887c23cfbf455)`[get]` |
| | Gets the background color for the mouse. |
| | |
| Color | [MouseTextColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a7b2222e5ba805f7d099dee0521661d00)`[get]` |
| | Gets the text color for the mouse. |
| | |
| Color | [BuyOrdersColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a0bf212dc65cdbfba89e12fbb8f4d3b77)`[get]` |
| | Gets the color used for buy orders. |
| | |
| Color | [SellOrdersColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a5a5fa52e0ab925deb97635e313f4b279)`[get]` |
| | Gets the color used for sell orders. |
| | |
| Color | [OrdersTextColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a11fe6738d0e23ee2b09884c3d07495f4)`[get]` |
| | Gets the text color for orders. |
| | |
| Color | [PositiveProfitColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a779dd38cf20626aceab2529bdddf914f)`[get]` |
| | Gets the color used for positive profit values. |
| | |
| Color | [NegativeProfitColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#ab82e45697c70060d570787b30f39c02b)`[get]` |
| | Gets the color used for negative profit values. |
| | |
| Color | [NeutralProfitColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#af07507ea5647bb43560e96eec54dfda3)`[get]` |
| | Gets the color used for neutral profit values. |
| | |
| Color | [ProfitTextColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a9428edf680a1da2d7fc2d80fa071754b)`[get]` |
| | Gets the text color for profit values. |
| | |
| Color | [DrawingObjectColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a5a5fb9f217058aae0ba361eaa1d4f5ba)`[get]` |
| | Gets the color used for drawing objects. |
| | |
| HeatmapTypes | [HeatmapType](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#a983cbacd6332de55d7050080a37f7d33)`[get]` |
| | Gets the type of heatmap used for rendering. |
| | |
| Color | [DrawingObjectsSelectionColor](./interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#ad221b132e285885fd30d1c1d10124169)`[get]` |
| | Gets the color used for drawing object selections. |
| | |

## Detailed Description

Interface for accessing colors and pens used in a chart's rendering.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)DrawBackground()

| void ATAS.Indicators.IChartColorsStore.DrawBackground | ( | RenderContext | context, |
| --- | --- | --- | --- |
| | | [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | region |
| | ) | | |

Draws the background of the chart within the specified region using the provided render context.

Parameters

| context | The render context. |
| --- | --- |
| region | The region to draw the background in. |

## Property Documentation

## [◆](https://docs.atas.net/en/)AxisTextColor

| Color ATAS.Indicators.IChartColorsStore.AxisTextColor |
| --- |

get

Gets the color used for axis text.

## [◆](https://docs.atas.net/en/)BarBorderPen

| RenderPen ATAS.Indicators.IChartColorsStore.BarBorderPen |
| --- |

get

Gets the pen used for drawing bar borders.

## [◆](https://docs.atas.net/en/)BaseBackgroundColor

| Color ATAS.Indicators.IChartColorsStore.BaseBackgroundColor |
| --- |

get

Gets the base background color of the chart.

## [◆](https://docs.atas.net/en/)BuyOrdersColor

| Color ATAS.Indicators.IChartColorsStore.BuyOrdersColor |
| --- |

get

Gets the color used for buy orders.

## [◆](https://docs.atas.net/en/)DojiBarPen

| RenderPen ATAS.Indicators.IChartColorsStore.DojiBarPen |
| --- |

get

Gets the pen used for drawing Doji bars.

## [◆](https://docs.atas.net/en/)DownBarPen

| RenderPen ATAS.Indicators.IChartColorsStore.DownBarPen |
| --- |

get

Gets the pen used for drawing down bars.

## [◆](https://docs.atas.net/en/)DownCandleColor

| Color ATAS.Indicators.IChartColorsStore.DownCandleColor |
| --- |

get

Gets the color used for down (bearish) candles.

## [◆](https://docs.atas.net/en/)DrawingObjectColor

| Color ATAS.Indicators.IChartColorsStore.DrawingObjectColor |
| --- |

get

Gets the color used for drawing objects.

## [◆](https://docs.atas.net/en/)DrawingObjectsSelectionColor

| Color ATAS.Indicators.IChartColorsStore.DrawingObjectsSelectionColor |
| --- |

get

Gets the color used for drawing object selections.

## [◆](https://docs.atas.net/en/)FootprintAskColor

| Color ATAS.Indicators.IChartColorsStore.FootprintAskColor |
| --- |

get

Gets the color used for the ask volume in a footprint chart.

## [◆](https://docs.atas.net/en/)FootprintBidColor

| Color ATAS.Indicators.IChartColorsStore.FootprintBidColor |
| --- |

get

Gets the color used for the bid volume in a footprint chart.

## [◆](https://docs.atas.net/en/)FootprintMainColor

| Color ATAS.Indicators.IChartColorsStore.FootprintMainColor |
| --- |

get

Gets the main color used in a footprint chart.

## [◆](https://docs.atas.net/en/)FootprintMaximumVolumeTextColor

| Color ATAS.Indicators.IChartColorsStore.FootprintMaximumVolumeTextColor |
| --- |

get

Gets the color used for the maximum volume text in a footprint chart.

## [◆](https://docs.atas.net/en/)FootprintTextColor

| Color ATAS.Indicators.IChartColorsStore.FootprintTextColor |
| --- |

get

Gets the color used for text in a footprint chart.

## [◆](https://docs.atas.net/en/)Grid

| RenderPen ATAS.Indicators.IChartColorsStore.Grid |
| --- |

get

Gets the pen used for drawing the grid lines.

## [◆](https://docs.atas.net/en/)HeatmapType

| HeatmapTypes ATAS.Indicators.IChartColorsStore.HeatmapType |
| --- |

get

Gets the type of heatmap used for rendering.

## [◆](https://docs.atas.net/en/)HorizontalGridIsEnabled

| bool ATAS.Indicators.IChartColorsStore.HorizontalGridIsEnabled |
| --- |

get

Gets a value indicating whether horizontal grid lines are enabled.

## [◆](https://docs.atas.net/en/)MouseBackground

| Color ATAS.Indicators.IChartColorsStore.MouseBackground |
| --- |

get

Gets the background color for the mouse.

## [◆](https://docs.atas.net/en/)MouseTextColor

| Color ATAS.Indicators.IChartColorsStore.MouseTextColor |
| --- |

get

Gets the text color for the mouse.

## [◆](https://docs.atas.net/en/)NegativeColor

| Color ATAS.Indicators.IChartColorsStore.NegativeColor |
| --- |

get

Gets the color used for negative values.

## [◆](https://docs.atas.net/en/)NegativeProfitColor

| Color ATAS.Indicators.IChartColorsStore.NegativeProfitColor |
| --- |

get

Gets the color used for negative profit values.

## [◆](https://docs.atas.net/en/)NeutralColor

| Color ATAS.Indicators.IChartColorsStore.NeutralColor |
| --- |

get

Gets the color used for neutral values.

## [◆](https://docs.atas.net/en/)NeutralProfitColor

| Color ATAS.Indicators.IChartColorsStore.NeutralProfitColor |
| --- |

get

Gets the color used for neutral profit values.

## [◆](https://docs.atas.net/en/)NewSessionPen

| RenderPen ATAS.Indicators.IChartColorsStore.NewSessionPen |
| --- |

get

Gets the pen used for drawing the start of a new session.

## [◆](https://docs.atas.net/en/)OrdersTextColor

| Color ATAS.Indicators.IChartColorsStore.OrdersTextColor |
| --- |

get

Gets the text color for orders.

## [◆](https://docs.atas.net/en/)PaneSeparators

| RenderPen ATAS.Indicators.IChartColorsStore.PaneSeparators |
| --- |

get

Gets the pen used for drawing pane separators.

## [◆](https://docs.atas.net/en/)PositiveColor

| Color ATAS.Indicators.IChartColorsStore.PositiveColor |
| --- |

get

Gets the color used for positive values.

## [◆](https://docs.atas.net/en/)PositiveProfitColor

| Color ATAS.Indicators.IChartColorsStore.PositiveProfitColor |
| --- |

get

Gets the color used for positive profit values.

## [◆](https://docs.atas.net/en/)ProfitTextColor

| Color ATAS.Indicators.IChartColorsStore.ProfitTextColor |
| --- |

get

Gets the text color for profit values.

## [◆](https://docs.atas.net/en/)SellOrdersColor

| Color ATAS.Indicators.IChartColorsStore.SellOrdersColor |
| --- |

get

Gets the color used for sell orders.

## [◆](https://docs.atas.net/en/)UpBarPen

| RenderPen ATAS.Indicators.IChartColorsStore.UpBarPen |
| --- |

get

Gets the pen used for drawing up bars.

## [◆](https://docs.atas.net/en/)UpCandleColor

| Color ATAS.Indicators.IChartColorsStore.UpCandleColor |
| --- |

get

Gets the color used for up (bullish) candles.

The documentation for this interface was generated from the following file:
- [IIndicatorContainer.cs](../files/IIndicatorContainer_8cs.md)
