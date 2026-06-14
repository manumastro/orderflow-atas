# ATAS.Indicators.Indicator Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1Indicator.html

Base class for custom indicators.
 [More...](./classATAS_1_1Indicators_1_1Indicator.md#details)

Inheritance diagram for ATAS.Indicators.Indicator:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.Indicator:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | | |
| --- | --- | --- |
| virtual void | [RefreshData](./classATAS_1_1Indicators_1_1Indicator.md#a487e11be117a6ab7280403ac99d170f6) () | |
| | Refreshes the data of the indicator. Override this method to update the indicator's data when necessary. | |
| | | |
| - Public Member Functions inherited from [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md) | | |
| void | [Draw](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a7548a0812fb237e8036b447dce4ceebd) (RenderContext context, [DrawingLayouts](../namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout) | |
| | | |
| override void | [Dispose](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a6552838e546335a9b1dfad976b730eb0) () | |
| | | |
| void | [ApplyDefaultColors](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ab8fcc4e51056b6883a59a5edd30b275e) () | |
| | Applies default colors to the drawing elements. | |
| | | |
| void | [SubscribeToTimer](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#aa725dd87b95c0d6347bb572458f1ad64) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback) | |
| | Registers the callback and periodically calls it with specified period.Parameters

 period | A period of market data time used to repeatedly invoke the callback. |
| callback | A method that is called each time a specified period of market data time passes. | |

void [UnsubscribeFromTimer](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a35eee088f9207adb5d5050e204e57e03) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback)
 Unregister the callback from periodic invocations.Parameters

| period | Subscribed period of invocations. |
| --- | --- |
| callback | A method to be unregistered. |

- Public Member Functions inherited from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md)
virtual void [Dispose](./classATAS_1_1Indicators_1_1BaseIndicator.md#af616a09632eca22bb3f2ea3900ed019c) ()

override string [ToString](./classATAS_1_1Indicators_1_1BaseIndicator.md#a01c654afdfeaaeafb968e990f4ca9b47) ()
 Converts the current instance of the indicator to its string representation.

- Public Member Functions inherited from [ATAS.Indicators.ChartObject](./classATAS_1_1Indicators_1_1ChartObject.md)
virtual bool [ProcessMouseClick](./classATAS_1_1Indicators_1_1ChartObject.md#a90e21830bc8fa463037483b601d7e654) (RenderControlMouseEventArgs e)
 Processes a mouse click event on the chart object.

virtual bool [ProcessMouseWheel](./classATAS_1_1Indicators_1_1ChartObject.md#ad20fdb9dfd4aba37065467b226572638) (int delta)
 Processes a mouse wheel event on the chart object.

virtual bool [ProcessMouseDown](./classATAS_1_1Indicators_1_1ChartObject.md#a77e00d6cc1c1be74712c7a75bfc3c9fc) (RenderControlMouseEventArgs e)
 Processes a mouse down event on the chart object.

virtual bool [ProcessMouseUp](./classATAS_1_1Indicators_1_1ChartObject.md#a850f79393af0ff85608968f8fb5b578e) (RenderControlMouseEventArgs e)
 Processes a mouse up event on the chart object.

virtual bool [ProcessMouseMove](./classATAS_1_1Indicators_1_1ChartObject.md#aab135d7efa994d847b9b2d1596773360) (RenderControlMouseEventArgs e)
 Processes a mouse move event on the chart object.

virtual bool [ProcessMouseDoubleClick](./classATAS_1_1Indicators_1_1ChartObject.md#ac62955c0a5bdc92c250ccb39e0b53adf) (RenderControlMouseEventArgs e)
 Processes a mouse double click event on the chart object.

virtual StdCursor [GetCursor](./classATAS_1_1Indicators_1_1ChartObject.md#ac9d5e69d8961eea481dfc3944c270aae) (RenderControlMouseEventArgs e)
 Gets the cursor to display when the mouse is over the chart object.

virtual bool [ProcessKeyDown](./classATAS_1_1Indicators_1_1ChartObject.md#a86bf534d0b898d493dffe622e6dd86b1) ([CrossKeyEventArgs](../files/Indicators_2GlobalUsings_8cs.md#a2e32a04f09342ad7cc35bdf38fd5f960) e)
 Processes a key down event on the chart object.

virtual bool [ProcessKeyUp](./classATAS_1_1Indicators_1_1ChartObject.md#a159acef22dc292a0a5e637c91d007be3) ([CrossKeyEventArgs](../files/Indicators_2GlobalUsings_8cs.md#a2e32a04f09342ad7cc35bdf38fd5f960) e)
 Processes a key up event on the chart object.

void [SubscribeToTimer](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a7a187460ab54c6669677fdb8a2d3153d) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback)
 Registers the callback and periodically calls it with specified period.

void [UnsubscribeFromTimer](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a486f0f6bddd244284110a94baeecd595) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback)
 Unregister the callback from periodic invocations.

| Protected Member Functions | | |
| --- | --- | --- |
| | [Indicator](./classATAS_1_1Indicators_1_1Indicator.md#afafb3d9a411b959bd8a78d360588b08d) (bool useCandles=false) | |
| | | |
| | [Indicator](./classATAS_1_1Indicators_1_1Indicator.md#aaaad75bcccbdcc21e2375505b41f30f4) ([DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) seriesType) | |
| | | |
| bool | [IsNewSession](./classATAS_1_1Indicators_1_1Indicator.md#a66714bf4c34e2e3741d6c19058e1d641) (int bar) | |
| | Function returns new session flag. | |
| | | |
| bool | [IsNewWeek](./classATAS_1_1Indicators_1_1Indicator.md#aeace7ec9e0967c0ec47e7085652dda88) (int bar) | |
| | Function returns new week flag. | |
| | | |
| bool | [IsNewMonth](./classATAS_1_1Indicators_1_1Indicator.md#ace949df3beb307acaed6bcf521d5bcbe) (int bar) | |
| | Function returns new month flag. | |
| | | |
| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) | [AddText](./classATAS_1_1Indicators_1_1Indicator.md#a9c08fac819d69da959f3729c237ee221) (string tag, string text, bool isAbovePrice, int bar, decimal price, Color textColor, Color outlineColor, Color fillColor, float fontSize, [DrawingText.TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) align, bool autoSize=false) | |
| | Adds a drawing text element to the chart at the specified position. | |
| | | |
| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) | [AddText](./classATAS_1_1Indicators_1_1Indicator.md#ad83e0155539d12651a588c0547dad2f1) (string tag, string text, bool isAbovePrice, int bar, decimal price, Color textColor, Color fillColor, float fontSize, [DrawingText.TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) align, bool autoSize=false) | |
| | Adds a drawing text element to the chart at the specified position. | |
| | | |
| void | [DoActionInGuiThread](./classATAS_1_1Indicators_1_1Indicator.md#aea79f472e747e3dc9451c12fcd948d33) (Action action) | |
| | Executes the specified action on the GUI thread. | |
| | | |
| IEnumerable | [GetMarketDepthSnapshot](./classATAS_1_1Indicators_1_1Indicator.md#ab1154488d7eadf340d5133a20b6d6671) () | |
| | | |
| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) | [AddText](./classATAS_1_1Indicators_1_1Indicator.md#aed0c91b2aa4ea79476b19c9f9ec8171e) (string tag, string text, bool isAbovePrice, int bar, int price, int yOffset, int xOffset, Color textcolor, Color outlinecolor, Color fillcolor, float fontSize, [DrawingText.TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) align, bool autoSize=false) | |
| | | |
| override void | [OnApplyDefaultColors](./classATAS_1_1Indicators_1_1Indicator.md#a25cecfe3b240e3d3a924a57b9924f63b) () | |
| | Invoked to apply default colors to the drawing elements.This method can be overridden in derived classes to customize or set default color values for the drawing elements. When this method is called, it is an opportunity for the derived class to apply its own default color scheme to the drawing elements used in the specific implementation. | |
| | | |
| - Protected Member Functions inherited from [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md) | | |
| | [ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#aab02840b6d69a687cf6bb0d82df030be) (bool useCandles=false) | |
| | | |
| | [ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a6c97d9e2752617cb14d5f57355b15a81) ([DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) seriesType) | |
| | | |
| virtual void | [OnContainerChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a748bbadba24dcfd5b06e55b98a1d89b6) ([IIndicatorContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md)? container) | |
| | This method is called when the value of the Container property changes. | |
| | | |
| virtual void | [OnDataProviderChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a4a2a2480006b93f2557b0e4a448a809b) ([IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md)? oldDataProvider, [IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md)? newDataProvider) | |
| | This method is called when the value of the DataProvider property changes. | |
| | | |
| bool | [TryGetService](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a64287eaa40b80f9e9f0be0b9ca586077) ([NotNullWhen(true)] out T? service) | |
| | | |
| virtual void | [OnPortfolioChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a7768bfd7fee4b30b2d7da82d9bb54359) ([Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio) | |
| | This method is called when the value of the Portfolio changes. | |
| | | |
| virtual void | [OnPositionChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a967d30114b76bb1d33e3ea91a2be1a2d) ([Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) | |
| | This method is called when the value of the Position changes. | |
| | | |
| virtual void | [OnNewOrder](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a71dbe6190be4e672674eb69bfd7cf363) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) | |
| | This method is called when a new order is received. | |
| | | |
| virtual void | [OnOrderChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a9824dc6d293046095e4c57174bea1d47) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order) | |
| | Called when an existing order is modified (changed). | |
| | | |
| virtual void | [OnNewMyTrade](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ac98aac74611a0eb14ccdebc8f17c8ad3) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) myTrade) | |
| | Called when a new trade executed by the indicator's own strategy is received. | |
| | | |
| virtual void | [OnOrderRegisterFailed](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a700f82d61eec508dc1045543e6c13d7e) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string message) | |
| | Called when an attempt to register (place) a new order fails. | |
| | | |
| virtual void | [OnOrderCancelFailed](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a36bb708f154ff763b85ea5db83b4b85a) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, string message) | |
| | Called when an attempt to cancel an existing order fails. | |
| | | |
| virtual void | [OnOrderModifyFailed](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ac2fb9aa68aa8f680bcb11bdd45a8dba0) ([Order](./classATAS_1_1DataFeedsCore_1_1Order.md) order, [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) newOrder, string error) | |
| | Called when an attempt to modify an existing order fails. | |
| | | |
| virtual void | [OnRender](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ad18d3b52324bbe2c61e2194187ec4f0a) (RenderContext context, [DrawingLayouts](../namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) layout) | |
| | Override this method for implementing your own data rendering logic. | |
| | | |
| void | [SubscribeToDrawingEvents](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a88a772682022b0a67f90099994df1a78) ([DrawingLayouts](../namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) flags) | |
| | Method for specifying the list of layouts in which rendering will be performed. | |
| | | |
| void | [RedrawChart](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#aeec530e5ae561fdee9909d6cd19bf56e) ([RedrawArg](./classATAS_1_1Indicators_1_1RedrawArg.md)? redrawArg=null) | |
| | Call to redraw chart. | |
| | | |
| void | [AddAlert](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ad9a16d5d6ed6a02cca9cbbd57dbff151) (string soundFile, string instrument, string message, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) background, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) foreground) | |
| | Adds an alert for a specific instrument with custom properties. | |
| | | |
| void | [AddAlert](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a91ab8f287b10d996380fe0c5a09f7d75) (string soundFile, string instrument, string message, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) background, [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) foreground, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) time) | |
| | Adds an alert for a specific instrument with custom properties. | |
| | | |
| void | [AddAlert](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a9bc6e3793f4bcf7cd84da7cc3e215be0) (string soundFile, string message) | |
| | Adds an alert with the specified sound file and message to the chart. | |
| | | |
| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) | [AddText](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ac0c6d0d23e4ce7fa4b4bbc95a2b4e413) (string tag, string text, bool isAbovePrice, int bar, decimal price, int yOffset, int xOffset, Color textColor, Color outlineColor, Color fillColor, float fontSize, [DrawingText.TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) align, bool autoSize=false) | |
| | Adds a drawing text element to the chart at the specified position. | |
| | | |
| override void | [RecalculateValues](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a9b335ed8cf85e1832c0ffc14013c2f2e) () | |
| | Recalculate the indicator values on each bar. | |
| | | |
| [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) | [GetCandle](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a4e22f2730354b56ffd66559a9c5f27af) (int bar) | |
| | Gets the IndicatorCandle object at the specified bar index. | |
| | | |
| override void | [Calculate](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a3795f4844cd693dd5630644d4092411d) (int bar, decimal value) | |
| | Performs the calculation for the indicator at the specified bar and value.Parameters

 bar | The bar number for which the calculation is performed. |
| value | The input value from the data series at the specified bar. | |

virtual void [OnNewTrades](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a4156041bbca69497b1ea571a937db2bb) (IEnumerable< [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) > trades)
 Handles a collection of new trade data represented by MarketDataArg objects.

virtual void [OnNewTrade](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a486ed770252ac81006f6bba11ae9c140) ([MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) trade)
 Handles an individual trade represented by a MarketDataArg object.

virtual void [OnBestBidAskChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a59989ad08bb16b9de7eb3a577b348cb3) ([MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) depth)
 Handles changes in the best bid or ask prices in the market depth represented by a MarketDataArg object.

virtual void [MarketDepthsChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ae2b152078f9ef03dc3c58883c93c1b49) (IEnumerable< [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) > depths)
 Handles changes in multiple market depths represented by an IEnumerable<MarketDataArg>.

virtual void [MarketDepthChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a532e9f8787219b852bd10f90fd682a36) ([MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) depth)
 Handles changes in a single market depth represented by a MarketDataArg object.

virtual void [OnCumulativeTrade](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ae7a7437439fea1973c7431bd45945f60) ([CumulativeTrade](./classATAS_1_1Indicators_1_1CumulativeTrade.md) trade)
 Handles a cumulative trade event represented by a CumulativeTrade object.

virtual void [OnUpdateCumulativeTrade](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#af6133d0303a699a7d8d52c4c6fa89d6e) ([CumulativeTrade](./classATAS_1_1Indicators_1_1CumulativeTrade.md) trade)
 Handles an update event for a cumulative trade represented by a CumulativeTrade object.

virtual void [OnCumulativeTradesResponse](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a979770a8f1280d7a41b26a03ea92da4b) ([CumulativeTradesRequest](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) request, IEnumerable< [CumulativeTrade](./classATAS_1_1Indicators_1_1CumulativeTrade.md) > cumulativeTrades)
 Called when a response for a cumulative trades request is received.

void [RequestForCumulativeTrades](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#add114c0bc61b7caa1ea3920d40ee0b62) ([CumulativeTradesRequest](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) request)
 Sends a request for cumulative trades data to the online data provider.

override void [OnVisibleChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1e63af84e4b7f823b5b286a3444c64eb) ()
 Called when the [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432) property changes.

virtual void [OnFixedProfilesResponse](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a8599e66ff98c9fc005b4884d6ba84db6) ([IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) fixedProfile, [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) period)

virtual void [OnFixedProfilesResponse](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a71f509f4510972758d0305c4ac22dc46) ([IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) fixedProfileScaled, [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) fixedProfileOriginScale, [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) period)
 This method is called when the online data provider responds with fixed profile data. Override this method in derived classes to handle the received fixed profile data.

void [GetFixedProfile](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a427ed52ab325b89b0728370ee571bf38) ([FixedProfileRequest](./classATAS_1_1Indicators_1_1FixedProfileRequest.md) request)
 Requests fixed profile data from the online data provider.

async Task< [FixedProfileResponse](../namespaces/namespaceATAS_1_1Indicators.md#a77a4afd41de8b48767b2066b36570332)?> [RequestFixedProfileAsync](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#acd0ed4ff03abf27cd37e30378cc6c481) ([FixedProfileRequest](./classATAS_1_1Indicators_1_1FixedProfileRequest.md) request)
 Asynchronously requests a fixed market profile parameterized with FixedProfileRequest.

Task [SubscribeMarketByOrderData](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1a25f6922ea82bc35ef188fb54083ace) ()
 Subscribes to the market by order data.

[ITradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITradesCache.md) [GetTradesCache](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a85da9de1f13440faaff4489258413b2d) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period)
 Returns a trades cache.

[IMarketByOrdersCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) [GetMarketByOrdersCache](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#acb49eb3dac65e137f93cfbe746d02d2c) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period)
 Returns a market by order data cache.

[IMarketByOrdersWithTradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) [GetMarketByOrdersWithTradesCache](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ac2c1a2feed0c5c9f141ad184cfce581d) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period)
 Returns a market by order data and trades cache.

virtual void [OnMarketByOrdersChanged](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a6025b950a67ef35f6f4ead349ac2fda6) (IEnumerable< [MarketByOrder](./classATAS_1_1DataFeedsCore_1_1MarketByOrder.md) > values)
 Handles a collection of market by order data represented by MarketByOrder objects.

virtual void [OnApplyDefaultColors](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a989beacecc3548d0bf5bc5755fca3c49) ()
 Invoked to apply default colors to the drawing elements.

- Protected Member Functions inherited from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md)
 [BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#ab626bc6b30ded7783c6e3f4a36daf118) (bool useCandles=false)

virtual void [RecalculateValues](./classATAS_1_1Indicators_1_1BaseIndicator.md#a6ee783f61e21eb5ff39be738b77dbd1e) ()
 Recalculate the indicator values on each bar.

virtual void [OnInitialize](./classATAS_1_1Indicators_1_1BaseIndicator.md#a78707b5100f92c64cea2865420e9ac83) ()
 The method is executed before the first calculation.

virtual void [OnRecalculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#aa4ef19f2c8896360932dbca55e96fb71) ()
 The method is executed before a new calculation.

virtual void [OnFinishRecalculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#aa5373860b1b58abf8e60be4700ea1217) ()
 The method is executed after the end of the calculation.

virtual void [Calculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#a6cd6e11444382222a48db90a5672d462) (int bar, decimal value)
 Performs the calculation for the indicator at the specified bar and value.

abstract void [OnCalculate](./classATAS_1_1Indicators_1_1BaseIndicator.md#a65c50fc1fec7d6c490aa23a1c3eeebd3) (int bar, decimal value)
 The main indicator calculation method is called for each bar on the history, then it is called on each tick.

void [Add](./classATAS_1_1Indicators_1_1BaseIndicator.md#a94e1ea2fe81d54e12eaab51282b3c798) ([Indicator](./classATAS_1_1Indicators_1_1Indicator.md) indicator)
 Adds an indicator to the list of used indicators by this indicator.

void [Clear](./classATAS_1_1Indicators_1_1BaseIndicator.md#ab88e03387b7c90b2752cbf5f862acfb0) ()
 Clear all data series.

virtual void [OnSourceChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#afe98089377ca3e76d9122cc7d5eef586) ()
 This method is called when the SourceDataSeries property is changed.

virtual void [OnPropertiesEditorChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#af8c8db3b926d834d3011713fec9dbc05) ([IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? oldValue, [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? newValue)
 Called when the PropertiesEditor property changes.

void [RaisePropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#afef6aaa070ba5020a82851ec203eccc3) (string propertyName)
 Raises the PropertyChanged event for the specified property name.

void [RaisePropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a460dfb3d56ebb0675f085686a355a3ba) (object? sender, PropertyChangedEventArgs e)
 Raises the PropertyChanged event with the specified event arguments.

void [RaisePanelPropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a50a0ff42e3004e36a19fd2c00202c8ef) (string name)
 Raises the PanelPropertyChanged event with the specified property name.

void [RaiseBarValueChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a18f034b71be84074920c8ff091378299) (int bar)
 Raises the BarValueChanged event with the specified bar value.

virtual void [OnDispose](./classATAS_1_1Indicators_1_1BaseIndicator.md#a9212779df0ab1c6bf2a2d76f1774931b) ()
 Called when the indicator is being disposed.

override void [OnVisibleChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a0c345165dff8b0c687383c0bc8db7755) ()
 Called when the [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432) property changes.

- Protected Member Functions inherited from [ATAS.Indicators.ChartObject](./classATAS_1_1Indicators_1_1ChartObject.md)
virtual void [OnVisibleChanged](./classATAS_1_1Indicators_1_1ChartObject.md#aebafef7ef3d98bc7b8026c6b4c0e6315) ()
 Called when the Visible property changes.

virtual void [LockedOnChanged](./classATAS_1_1Indicators_1_1ChartObject.md#af14593844c5be179a01e710ae152eda7) ()
 Called when the Locked property changes.

- Protected Member Functions inherited from [ATAS.Indicators.Filters.TrackedPropertyBase](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md)
void [SetProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#af0d5001e98aaa5522f03dad9e2a2e445) (ref TProperty store, TProperty value, Action? onChanged=null, Func< TProperty, bool >? onChanging=null, [CallerMemberName] string propertyName="")
 Sets the value of a property and notifies subscribers if the value has changed.

void [SetTrackedProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#a8889a2db640b8dafb3b7cf8266f8d343) (ref TProperty store, TProperty value, Action< string >? onChanged=null, [CallerMemberName] string propertyName="")
 Sets the value of a property that implements the INotifyPropertyChanged interface and notifies subscribers if the value has changed.

virtual void [OnChangeProperty](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#ac7aa2516aa4dc73893b2352a89c6d719) ([CallerMemberName] string propertyName="")
 Notifies subscribers when a property value changes.

| Properties | |
| --- | --- |
| string? | [Category](./classATAS_1_1Indicators_1_1Indicator.md#a1728bb87b056a777e2c7ea5e9c820907)`[get]` |
| | |
| [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md)? | [InstrumentInfo](./classATAS_1_1Indicators_1_1Indicator.md#a4bcea1b8c2d11532aa23c5d3a0806228)`[get]` |
| | Gets the instrument information associated with the data provider. |
| | |
| [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md)? | [TradingManager](./classATAS_1_1Indicators_1_1Indicator.md#a860ae675c2c927b5b05c23f5e480b483)`[get]` |
| | Gets the trading manager associated with the data provider. |
| | |
| [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md)? | [PlatformSettings](./classATAS_1_1Indicators_1_1Indicator.md#a0ae433a1e2b376b5ee05ac7b1f8b5ec4)`[get]` |
| | Gets the platform settings associated with the data provider. |
| | |
| [IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md)? | [MarketDepthInfo](./classATAS_1_1Indicators_1_1Indicator.md#a06e73f90f3cb6842174f775b2c613705)`[get]` |
| | Gets the market depth information provider associated with the data provider. |
| | |
| [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md)? | [ChartInfo](./classATAS_1_1Indicators_1_1Indicator.md#a52e91039466de7c2218a0248e585a259)`[get]` |
| | Gets the chart information associated with the data provider. |
| | |
| [ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md)? | [TradingStatisticsProvider](./classATAS_1_1Indicators_1_1Indicator.md#a8f17faea0cd2927486eb77fb218b0eda)`[get]` |
| | Gets the trading statistics provider associated with the data provider. |
| | |
| bool | [IgnoreHistoryScale](./classATAS_1_1Indicators_1_1Indicator.md#af7e7e4859c7ad504409fffcea4a9b526)`[get, set]` |
| | |
| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) | [ChartArea](./classATAS_1_1Indicators_1_1Indicator.md#ac5bc6a2f7bd9fde192049f9cfc811709)`[get]` |
| | Gets the rectangle representing the chart area on the screen. |
| | |
| [IMouseLocationInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md) | [MouseLocationInfo](./classATAS_1_1Indicators_1_1Indicator.md#ab23ac08de154b825a480f1d8fcbed590)`[get]` |
| | Gets the information about the mouse location on the chart. |
| | |
| int | [FirstVisibleBarNumber](./classATAS_1_1Indicators_1_1Indicator.md#a24c58e03f049eb938ab233a4488a3665)`[get]` |
| | Gets the bar number of the first visible bar on the chart. |
| | |
| int | [LastVisibleBarNumber](./classATAS_1_1Indicators_1_1Indicator.md#a70433fee421de1251f0459602c7ff6b5)`[get]` |
| | Gets the bar number of the last visible bar on the chart. |
| | |
| int | [VisibleBarsCount](./classATAS_1_1Indicators_1_1Indicator.md#a0eacdb117700ff3462183145350fa833)`[get]` |
| | Gets the number of visible bars on the chart. |
| | |
| decimal | [CumulativeDomAsks](./classATAS_1_1Indicators_1_1Indicator.md#a69446ed51845cb768506d3ee8ab3f81c)`[get]` |
| | |
| decimal | [CumulativeDomBids](./classATAS_1_1Indicators_1_1Indicator.md#a5cc41b167af9bafbd83b107c04b4e8cd)`[get]` |
| | |
| string? | [Instrument](./classATAS_1_1Indicators_1_1Indicator.md#a75a071aa8722e0c43f62cab5cf045af7)`[get]` |
| | Instrument name. |
| | |
| decimal | [TickSize](./classATAS_1_1Indicators_1_1Indicator.md#a7e953b1ec01332ada085944d5dcec8b0)`[get]` |
| | TickSize. |
| | |
| string | [DataPath](./classATAS_1_1Indicators_1_1Indicator.md#a0b1c2017f04fed4dd8ae18376113c44f)`[get]` |
| | Path to the working program folder. |
| | |
| int | [ValueAreaPercent](./classATAS_1_1Indicators_1_1Indicator.md#a99bfd2e090e1335e7db7e1ddffc6bc60)`[get]` |
| | |
| string? | [ChartType](./classATAS_1_1Indicators_1_1Indicator.md#acae705b4def89aa0e10a81207e5793c8)`[get]` |
| | Chart type. |
| | |
| string? | [TimeFrame](./classATAS_1_1Indicators_1_1Indicator.md#a525d40153232f16f092b586cbdb3635d)`[get]` |
| | Timeframe. |
| | |
| string | [StringFormat](./classATAS_1_1Indicators_1_1Indicator.md#a6d4769cfdf05a3e64dd1d016171db308)`[get]` |
| | Price format. |
| | |
| - Properties inherited from [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md) | |
| virtual bool | [AlertsEnabled](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a33c53ee0c30fdcaf2e666912ee2d289a) = false`[get, set]` |
| | |
| bool | [FullScreenMode](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1504042deafbbb960cdcce28a552f89e)`[get, set]` |
| | Gets or sets full screen mode for the indicator. Could be applied only to the vertical indicators('IsVerticalIndicator = true') |
| | |
| bool | [DenyToChangePanel](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a71700e9c11b8ce894e64a4e7dff0885c)`[get, set]` |
| | Gets or sets a value indicating whether changing the indicator's panel is denied. For vertical indicators, it is always denied. |
| | |
| bool | [DrawAbovePrice](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a090c77a1502d9f34cc92bd873b181973)`[get, set]` |
| | Gets or sets a value indicating whether custom painting should be performed on top of rendered bars. |
| | |
| bool | [EnableCustomDrawing](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a8525dc77a6105f64bb51b8d7b1076fcf)`[get, protected set]` |
| | Gets or sets a value indicating whether custom drawing is enabled for the indicator. |
| | |
| bool | [ShowDescription](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a0236d69dd032f46ffe4ddf3e1de433cc)`[get, set]` |
| | Gets or sets a value indicating whether the description of the indicator should be shown. |
| | |
| [IIndicatorContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md)? | [Container](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1753849ca89e685ad2fa9592855a3e2d)`[get, set]` |
| | Gets or sets the container in which the indicator is hosted. |
| | |
| [IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md)? | [DataProvider](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a39a4e28f47ffe821837de72c59b447a4)`[get, set]` |
| | Gets or sets the data provider for the indicator. |
| | |
| List | [HorizontalLinesTillTouch](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a07d8c669247a1b07aa8a7b0674f39f3e) = new()`[get]` |
| | Gets the collection of horizontal lines used to track touch points on the chart. |
| | |
| List | [TrendLines](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a885514af616ec4974504ab6a1dd3205a) = new()`[get]` |
| | Gets the collection of trend lines on the chart. |
| | |
| List | [Rectangles](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a310cc3db29528b38a8eaaf3e0341f750) = new()`[get]` |
| | Gets the collection of rectangles on the chart. |
| | |
| SyncDictionary | [Labels](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#ad2e7ed35ea74a5fb532dd3608adca3be) = new()`[get, set]` |
| | Gets or sets the collection of named drawing text elements. |
| | |
| IEnumerable | [MarketByOrders](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#aebdf63ef92c7ba3705d151ce76cab00f)`[get]` |
| | Gets a snapshot of the current market by order data. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [MarketTime](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a3cb11266584a54b0174b13d8480e3093)`[get]` |
| | Current market time. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [UtcTime](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a135df1979d1e058363d36382e8117336)`[get]` |
| | Current UTC time. |
| | |
| - Properties inherited from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md) | |
| static ? PerformanceDiagnoser | [PerformanceDiagnoser](./classATAS_1_1Indicators_1_1BaseIndicator.md#a61ec0c67898d4d0d3e045a7ad97dbd29)`[get]` |
| | Indicator performance tracker. |
| | |
| static bool | [UseProfiling](./classATAS_1_1Indicators_1_1BaseIndicator.md#abb240b488fd75dc6b9f254e739eaba22)`[get, set]` |
| | Set to `true` to measure the performance of all indicators. |
| | |
| [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? | [PropertiesEditor](./classATAS_1_1Indicators_1_1BaseIndicator.md#ac7e9c933b8fcd90a972c72999a2a5c51)`[get, set]` |
| | |
| string | [Name](./classATAS_1_1Indicators_1_1BaseIndicator.md#aab455feaa3727fcc19e50003cf0c4dca)`[get, set]` |
| | Name of the indicator. |
| | |
| bool | [IsDisposed](./classATAS_1_1Indicators_1_1BaseIndicator.md#a1464d3459efe29157642e05f9beb6de3)`[get, set]` |
| | Gets or sets a value indicating whether the indicator object has been disposed of. |
| | |
| List | [DataSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#a2935159f92499f89d36aba3c1e604d21)`[get]` |
| | List of data series used by the indicator. |
| | |
| bool | [SupportsExtendedSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#a18fa6ba1cbc6125818fa90274396f421)`[get]` |
| | Gets value indicating whether the data series can be drawn out of chart bars. |
| | |
| List | [LineSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#a95dcfd417f6a472f7391ea0792fc0b13)`[get]` |
| | List of line series used by the indicator. |
| | |
| string | [Panel](./classATAS_1_1Indicators_1_1BaseIndicator.md#a1edf20bbcde60c2dfca7ea94eaa2de89)`[get, set]` |
| | The name of the panel where the indicator is placed. |
| | |
| bool | [IsVerticalIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#a3f9791869e2f94b1b487c45f1e098c43)`[get, set]` |
| | Gets or sets a value indicating whether the indicator is intended to be displayed as a vertical indicator. |
| | |
| bool | [UseCandles](./classATAS_1_1Indicators_1_1BaseIndicator.md#a9a373643c22e84ccca84e58c2ec023ec)`[get]` |
| | Gets a value indicating whether the indicator uses candle data series. |
| | |
| int | [CurrentBar](./classATAS_1_1Indicators_1_1BaseIndicator.md#a2dad625c8cb62d89a65a46b824638b68)`[get]` |
| | Bars number. All bars and the values of the corresponding data series have a serial number. The earliest bar of the chart is assigned the number 0; the next bar is assigned the number 1, and so on. |
| | |
| [IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md)? | [SourceDataSeries](./classATAS_1_1Indicators_1_1BaseIndicator.md#af1a8638540f6a36ce4fb437988c6890a)`[get, set]` |
| | Gets or sets the data series used as the source for the indicator's calculations. |
| | |
| decimal | [this[int index]](./classATAS_1_1Indicators_1_1BaseIndicator.md#aee0c78bdc23de4c8b1a9d1aec86c8315)`[get, protected set]` |
| | Gets or sets the value of the first data series of the indicator at the specified index. |
| | |
| - Properties inherited from [ATAS.Indicators.ChartObject](./classATAS_1_1Indicators_1_1ChartObject.md) | |
| bool | [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432)`[get, set]` |
| | Gets or sets a value indicating whether the chart object is visible. |
| | |
| bool | [Locked](./classATAS_1_1Indicators_1_1ChartObject.md#adce7929b08a500d8d0af56fba421a7f6)`[get, set]` |
| | Gets or sets a value indicating whether the chart object is locked. |
| | |
| bool | [AllowedInteraction](./classATAS_1_1Indicators_1_1ChartObject.md#ad1e5ae606113f094b1179006f16db457)`[get]` |
| | Gets a value indicating whether interaction with the chart object is allowed. |
| | |
| - Properties inherited from [ATAS.Indicators.IPropertiesEditorOwner](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md) | |
| [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? | [PropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md#a06ea4c501070e8d9d05dcb8f27c8a472)`[get, set]` |
| | |
| - Properties inherited from [ATAS.Indicators.IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md) | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [MarketTime](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a719ea8d551a57d0673177c0d01931ffb)`[get]` |
| | Current market time. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [UtcTime](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#afd122a8c561de01a599f88f1477615f9)`[get]` |
| | Current UTC time. |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Static Protected Member Functions inherited from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md) | |
| static PerfCounter | [MeasurePerformance](./classATAS_1_1Indicators_1_1BaseIndicator.md#af4f20fe3c0d0c31be0e319f92bff0208) (string name) |
| | Measures the performance of a specific operation with the given name. If a performance diagnoser is available, it will be used to measure the performance; otherwise, a default performance counter will be returned. |
| | |
| - Protected Attributes inherited from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md) | |
| readonly List | [UsedIndicators](./classATAS_1_1Indicators_1_1BaseIndicator.md#aa05e2ac945fde12b8db000d0491f29bd) = new() |
| | The list of indicators that are being used by this indicator. |
| | |
| - Events inherited from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md) | |
| new? PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a13656e94a28b82a339289c05556c4b49) |
| | |
| PropertyChangedEventHandler? | [PanelPropertyChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a07137a5ec1366cdf4ac1588a50067faa) |
| | |
| Action? | [BarValueChanged](./classATAS_1_1Indicators_1_1BaseIndicator.md#a5530e08569dd5765e8f70dc48a01de3a) |
| | Event that is raised when the value of a bar in the indicator changes. |
| | |
| - Events inherited from [ATAS.Indicators.Filters.TrackedPropertyBase](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md) | |
| PropertyChangedEventHandler? | [PropertyChanged](./classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md#a975a4e91d70dc5e245000edd16233886) |
| | |
| - Events inherited from [ATAS.Indicators.INotifyPanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md) | |
| PropertyChangedEventHandler | [PanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md#a1eb7377432737676cf47033c1fe6a965) |
| | Occurs when a panel property value changes. |
| | |

## Detailed Description

Base class for custom indicators.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Indicator() [1/2]

| ATAS.Indicators.Indicator.Indicator | ( | bool | useCandles = `false` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)Indicator() [2/2]

| ATAS.Indicators.Indicator.Indicator | ( | [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | seriesType | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)AddText() [1/3]

| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) ATAS.Indicators.Indicator.AddText | ( | string | tag, |
| --- | --- | --- | --- |
| | | string | text, |
| | | bool | isAbovePrice, |
| | | int | bar, |
| | | decimal | price, |
| | | Color | textColor, |
| | | Color | fillColor, |
| | | float | fontSize, |
| | | [DrawingText::TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) | align, |
| | | bool | autoSize = `false` |
| | ) | | |

protected

Adds a drawing text element to the chart at the specified position.

Parameters

| tag | The tag associated with the text element. |
| --- | --- |
| text | The text content of the element. |
| isAbovePrice | Specifies whether the text should be placed above the price. |
| bar | The bar number where the text will be placed. |
| price | The price at which the text will be placed. |
| textColor | The color of the text. |
| fillColor | The fill color of the text. |
| fontSize | The font size of the text. |
| align | The alignment of the text. |
| autoSize | Specifies whether the text size should be automatically adjusted. |

ReturnsThe newly added DrawingText element.

## [◆](https://docs.atas.net/en/)AddText() [2/3]

| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) ATAS.Indicators.Indicator.AddText | ( | string | tag, |
| --- | --- | --- | --- |
| | | string | text, |
| | | bool | isAbovePrice, |
| | | int | bar, |
| | | decimal | price, |
| | | Color | textColor, |
| | | Color | outlineColor, |
| | | Color | fillColor, |
| | | float | fontSize, |
| | | [DrawingText::TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) | align, |
| | | bool | autoSize = `false` |
| | ) | | |

protected

Adds a drawing text element to the chart at the specified position.

Parameters

| tag | The tag associated with the text element. |
| --- | --- |
| text | The text content of the element. |
| isAbovePrice | Specifies whether the text should be placed above the price. |
| bar | The bar number where the text will be placed. |
| price | The price at which the text will be placed. |
| textColor | The color of the text. |
| outlineColor | The color of the outline of the text. |
| fillColor | The fill color of the text. |
| fontSize | The font size of the text. |
| align | The alignment of the text. |
| autoSize | Specifies whether the text size should be automatically adjusted. |

ReturnsThe newly added DrawingText element.

## [◆](https://docs.atas.net/en/)AddText() [3/3]

| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) ATAS.Indicators.Indicator.AddText | ( | string | tag, |
| --- | --- | --- | --- |
| | | string | text, |
| | | bool | isAbovePrice, |
| | | int | bar, |
| | | int | price, |
| | | int | yOffset, |
| | | int | xOffset, |
| | | Color | textcolor, |
| | | Color | outlinecolor, |
| | | Color | fillcolor, |
| | | float | fontSize, |
| | | [DrawingText::TextAlign](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md#ae311cc54d33c577806ab1202b1f4192a) | align, |
| | | bool | autoSize = `false` |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)DoActionInGuiThread()

| void ATAS.Indicators.Indicator.DoActionInGuiThread | ( | Action | action | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Executes the specified action on the GUI thread.

Parameters

| action | The action to execute on the GUI thread. |
| --- | --- |

## [◆](https://docs.atas.net/en/)GetMarketDepthSnapshot()

| IEnumerable ATAS.Indicators.Indicator.GetMarketDepthSnapshot | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)IsNewMonth()

| bool ATAS.Indicators.Indicator.IsNewMonth | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Function returns new month flag.

Parameters

| bar | Bar number |
| --- | --- |

Returns

## [◆](https://docs.atas.net/en/)IsNewSession()

| bool ATAS.Indicators.Indicator.IsNewSession | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Function returns new session flag.

Parameters

| bar | Bar number |
| --- | --- |

Returns

## [◆](https://docs.atas.net/en/)IsNewWeek()

| bool ATAS.Indicators.Indicator.IsNewWeek | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Function returns new week flag.

Parameters

| bar | Bar number |
| --- | --- |

Returns

## [◆](https://docs.atas.net/en/)OnApplyDefaultColors()

| override void ATAS.Indicators.Indicator.OnApplyDefaultColors | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Invoked to apply default colors to the drawing elements.This method can be overridden in derived classes to customize or set default color values for the drawing elements. When this method is called, it is an opportunity for the derived class to apply its own default color scheme to the drawing elements used in the specific implementation.

Reimplemented from [ATAS.Indicators.ExtendedIndicator](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#a989beacecc3548d0bf5bc5755fca3c49).

## [◆](https://docs.atas.net/en/)RefreshData()

| virtual void ATAS.Indicators.Indicator.RefreshData | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Refreshes the data of the indicator. Override this method to update the indicator's data when necessary.

## Property Documentation

## [◆](https://docs.atas.net/en/)Category

| string? ATAS.Indicators.Indicator.Category |
| --- |

get

## [◆](https://docs.atas.net/en/)ChartArea

| [Rectangle](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e) ATAS.Indicators.Indicator.ChartArea |
| --- |

getprotected

Gets the rectangle representing the chart area on the screen.

## [◆](https://docs.atas.net/en/)ChartInfo

| [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md)? ATAS.Indicators.Indicator.ChartInfo |
| --- |

get

Gets the chart information associated with the data provider.

## [◆](https://docs.atas.net/en/)ChartType

| string? ATAS.Indicators.Indicator.ChartType |
| --- |

get

Chart type.

## [◆](https://docs.atas.net/en/)CumulativeDomAsks

| decimal ATAS.Indicators.Indicator.CumulativeDomAsks |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)CumulativeDomBids

| decimal ATAS.Indicators.Indicator.CumulativeDomBids |
| --- |

getprotected

## [◆](https://docs.atas.net/en/)DataPath

| string ATAS.Indicators.Indicator.DataPath |
| --- |

get

Path to the working program folder.

## [◆](https://docs.atas.net/en/)FirstVisibleBarNumber

| int ATAS.Indicators.Indicator.FirstVisibleBarNumber |
| --- |

getprotected

Gets the bar number of the first visible bar on the chart.

## [◆](https://docs.atas.net/en/)IgnoreHistoryScale

| bool ATAS.Indicators.Indicator.IgnoreHistoryScale |
| --- |

getset

## [◆](https://docs.atas.net/en/)Instrument

| string? ATAS.Indicators.Indicator.Instrument |
| --- |

get

Instrument name.

## [◆](https://docs.atas.net/en/)InstrumentInfo

| [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md)? ATAS.Indicators.Indicator.InstrumentInfo |
| --- |

getprotected

Gets the instrument information associated with the data provider.

## [◆](https://docs.atas.net/en/)LastVisibleBarNumber

| int ATAS.Indicators.Indicator.LastVisibleBarNumber |
| --- |

getprotected

Gets the bar number of the last visible bar on the chart.

## [◆](https://docs.atas.net/en/)MarketDepthInfo

| [IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md)? ATAS.Indicators.Indicator.MarketDepthInfo |
| --- |

getprotected

Gets the market depth information provider associated with the data provider.

## [◆](https://docs.atas.net/en/)MouseLocationInfo

| [IMouseLocationInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md) ATAS.Indicators.Indicator.MouseLocationInfo |
| --- |

getprotected

Gets the information about the mouse location on the chart.

## [◆](https://docs.atas.net/en/)PlatformSettings

| [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md)? ATAS.Indicators.Indicator.PlatformSettings |
| --- |

get

Gets the platform settings associated with the data provider.

## [◆](https://docs.atas.net/en/)StringFormat

| string ATAS.Indicators.Indicator.StringFormat |
| --- |

getprotected

Price format.

## [◆](https://docs.atas.net/en/)TickSize

| decimal ATAS.Indicators.Indicator.TickSize |
| --- |

getprotected

TickSize.

## [◆](https://docs.atas.net/en/)TimeFrame

| string? ATAS.Indicators.Indicator.TimeFrame |
| --- |

get

Timeframe.

## [◆](https://docs.atas.net/en/)TradingManager

| [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md)? ATAS.Indicators.Indicator.TradingManager |
| --- |

getprotected

Gets the trading manager associated with the data provider.

## [◆](https://docs.atas.net/en/)TradingStatisticsProvider

| [ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md)? ATAS.Indicators.Indicator.TradingStatisticsProvider |
| --- |

get

Gets the trading statistics provider associated with the data provider.

## [◆](https://docs.atas.net/en/)ValueAreaPercent

| int ATAS.Indicators.Indicator.ValueAreaPercent |
| --- |

get

## [◆](https://docs.atas.net/en/)VisibleBarsCount

| int ATAS.Indicators.Indicator.VisibleBarsCount |
| --- |

getprotected

Gets the number of visible bars on the chart.

The documentation for this class was generated from the following file:
- [Indicator.cs](../files/Indicator_8cs.md)
