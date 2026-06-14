# ATAS.Indicators.ExtendedIndicator Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1ExtendedIndicator.html

An extended base class for custom indicators that provide additional functionality for drawing, alerts, market data handling, etc.
 [More...](./classATAS_1_1Indicators_1_1ExtendedIndicator.md#details)

Inheritance diagram for ATAS.Indicators.ExtendedIndicator:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.ExtendedIndicator:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | | |
| --- | --- | --- |
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

An extended base class for custom indicators that provide additional functionality for drawing, alerts, market data handling, etc.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ExtendedIndicator() [1/2]

| ATAS.Indicators.ExtendedIndicator.ExtendedIndicator | ( | bool | useCandles = `false` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)ExtendedIndicator() [2/2]

| ATAS.Indicators.ExtendedIndicator.ExtendedIndicator | ( | [DataSeriesType](../namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) | seriesType | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)AddAlert() [1/3]

| void ATAS.Indicators.ExtendedIndicator.AddAlert | ( | string | soundFile, |
| --- | --- | --- | --- |
| | | string | instrument, |
| | | string | message, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | background, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | foreground |
| | ) | | |

protected

Adds an alert for a specific instrument with custom properties.

Parameters

| soundFile | The file path or name of the sound file to be played for the alert. |
| --- | --- |
| instrument | The name of the instrument or symbol associated with the alert. |
| message | The message or description of the alert. |
| background | The background color to be used for the alert visualization. |
| foreground | The foreground color to be used for the alert visualization. |

## [◆](https://docs.atas.net/en/)AddAlert() [2/3]

| void ATAS.Indicators.ExtendedIndicator.AddAlert | ( | string | soundFile, |
| --- | --- | --- | --- |
| | | string | instrument, |
| | | string | message, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | background, |
| | | [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | foreground, |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | time |
| | ) | | |

protected

Adds an alert for a specific instrument with custom properties.

Parameters

| soundFile | The file path or name of the sound file to be played for the alert. |
| --- | --- |
| instrument | The name of the instrument or symbol associated with the alert. |
| message | The message or description of the alert. |
| background | The background color to be used for the alert visualization. |
| foreground | The foreground color to be used for the alert visualization. |
| time | Exact time for alert |

## [◆](https://docs.atas.net/en/)AddAlert() [3/3]

| void ATAS.Indicators.ExtendedIndicator.AddAlert | ( | string | soundFile, |
| --- | --- | --- | --- |
| | | string | message |
| | ) | | |

protected

Adds an alert with the specified sound file and message to the chart.

Parameters

| soundFile | The file path of the sound to be played when the alert is triggered. |
| --- | --- |
| message | The message to be displayed in the alert. |

## [◆](https://docs.atas.net/en/)AddText()

| [DrawingText](./classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) ATAS.Indicators.ExtendedIndicator.AddText | ( | string | tag, |
| --- | --- | --- | --- |
| | | string | text, |
| | | bool | isAbovePrice, |
| | | int | bar, |
| | | decimal | price, |
| | | int | yOffset, |
| | | int | xOffset, |
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

| tag | A unique tag or identifier for the text element. |
| --- | --- |
| text | The actual text to be displayed. |
| isAbovePrice | A boolean flag indicating whether the text should be displayed above the price or not. |
| bar | The index of the bar where the text should be displayed. |
| price | The price at which the text should be displayed. |
| yOffset | The vertical offset of the text from its default position. |
| xOffset | The horizontal offset of the text from its default position. |
| textColor | The color of the text. |
| outlineColor | The color used for the outline of the text. |
| fillColor | The fill color of the text background. |
| fontSize | The font size of the text. |
| align | The text alignment within its bounding box. |
| autoSize | A boolean flag indicating whether the text should be auto-sized or not. |

ReturnsThe created DrawingText object.

## [◆](https://docs.atas.net/en/)ApplyDefaultColors()

| void ATAS.Indicators.ExtendedIndicator.ApplyDefaultColors | ( | | ) | |
| --- | --- | --- | --- | --- |

Applies default colors to the drawing elements.

This method triggers the OnApplyDefaultColors virtual method, which can be overridden in derived classes to customize the default color settings for the drawing elements.

## [◆](https://docs.atas.net/en/)Calculate()

| override void ATAS.Indicators.ExtendedIndicator.Calculate | ( | int | bar, |
| --- | --- | --- | --- |
| | | decimal | value |
| | ) | | |

protectedvirtual

Performs the calculation for the indicator at the specified bar and value.Parameters

| bar | The bar number for which the calculation is performed. |
| --- | --- |
| value | The input value from the data series at the specified bar. |

Reimplemented from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#a6cd6e11444382222a48db90a5672d462).

## [◆](https://docs.atas.net/en/)Dispose()

| override void ATAS.Indicators.ExtendedIndicator.Dispose | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Reimplemented from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#af616a09632eca22bb3f2ea3900ed019c).

## [◆](https://docs.atas.net/en/)Draw()

| void ATAS.Indicators.ExtendedIndicator.Draw | ( | RenderContext | context, |
| --- | --- | --- | --- |
| | | [DrawingLayouts](../namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) | layout |
| | ) | | |

## [◆](https://docs.atas.net/en/)GetCandle()

| [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) ATAS.Indicators.ExtendedIndicator.GetCandle | ( | int | bar | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Gets the IndicatorCandle object at the specified bar index.

Parameters

| bar | The index of the bar for which to retrieve the IndicatorCandle. |
| --- | --- |

ReturnsThe IndicatorCandle object at the specified bar index.

## [◆](https://docs.atas.net/en/)GetFixedProfile()

| void ATAS.Indicators.ExtendedIndicator.GetFixedProfile | ( | [FixedProfileRequest](./classATAS_1_1Indicators_1_1FixedProfileRequest.md) | request | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Requests fixed profile data from the online data provider.

Parameters

| request | The request object containing information about the fixed profile data to be fetched. |
| --- | --- |

## [◆](https://docs.atas.net/en/)GetMarketByOrdersCache()

| [IMarketByOrdersCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) ATAS.Indicators.ExtendedIndicator.GetMarketByOrdersCache | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Returns a market by order data cache.

Parameters

| period | Specifies the time period for which Market-by-Order updates are stored in IMarketByOrdersCache. |
| --- | --- |

ReturnsThe IMarketByOrdersCache object representing the current state and allowing to receive real-time updates.

## [◆](https://docs.atas.net/en/)GetMarketByOrdersWithTradesCache()

| [IMarketByOrdersWithTradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) ATAS.Indicators.ExtendedIndicator.GetMarketByOrdersWithTradesCache | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Returns a market by order data and trades cache.

Parameters

| period | Specifies the time period for which trades are stored in IMarketByOrdersWithTradesCache. |
| --- | --- |

ReturnsThe IMarketByOrdersWithTradesCache object representing the cache.

## [◆](https://docs.atas.net/en/)GetTradesCache()

| [ITradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITradesCache.md) ATAS.Indicators.ExtendedIndicator.GetTradesCache | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Returns a trades cache.

Parameters

| period | Specifies the time period for which trades are stored in ITradesCache. |
| --- | --- |

ReturnsThe ITradesCache object representing the trades cache.

## [◆](https://docs.atas.net/en/)MarketDepthChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.MarketDepthChanged | ( | [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) | depth | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles changes in a single market depth represented by a MarketDataArg object.

Parameters

| depth | The MarketDataArg representing the market depth data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)MarketDepthsChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.MarketDepthsChanged | ( | IEnumerable | depths | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles changes in multiple market depths represented by an IEnumerable<MarketDataArg>.

Parameters

| depths | The collection of MarketDataArg representing the market depth data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnApplyDefaultColors()

| virtual void ATAS.Indicators.ExtendedIndicator.OnApplyDefaultColors | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Invoked to apply default colors to the drawing elements.

This method can be overridden in derived classes to customize or set default color values for the drawing elements. When this method is called, it is an opportunity for the derived class to apply its own default color scheme to the drawing elements used in the specific implementation.

Reimplemented in [ATAS.Indicators.Indicator](./classATAS_1_1Indicators_1_1Indicator.md#a25cecfe3b240e3d3a924a57b9924f63b).

## [◆](https://docs.atas.net/en/)OnBestBidAskChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.OnBestBidAskChanged | ( | [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) | depth | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles changes in the best bid or ask prices in the market depth represented by a MarketDataArg object.

Parameters

| depth | The MarketDataArg representing the market depth data. |
| --- | --- |

Reimplemented in [ATAS.Strategies.Chart.ChartStrategy](./classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a6d7df98a414125f784cca56375a57b91).

## [◆](https://docs.atas.net/en/)OnContainerChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.OnContainerChanged | ( | [IIndicatorContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md)? | container | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

This method is called when the value of the Container property changes.

Parameters

| container | The new value of the IIndicatorContainer that the indicator is attached to. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnCumulativeTrade()

| virtual void ATAS.Indicators.ExtendedIndicator.OnCumulativeTrade | ( | [CumulativeTrade](./classATAS_1_1Indicators_1_1CumulativeTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles a cumulative trade event represented by a CumulativeTrade object.

Parameters

| trade | The CumulativeTrade representing the cumulative trade data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnCumulativeTradesResponse()

| virtual void ATAS.Indicators.ExtendedIndicator.OnCumulativeTradesResponse | ( | [CumulativeTradesRequest](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) | request, |
| --- | --- | --- | --- |
| | | IEnumerable | cumulativeTrades |
| | ) | | |

protectedvirtual

Called when a response for a cumulative trades request is received.

Parameters

| request | The CumulativeTradesRequest object representing the original request. |
| --- | --- |
| cumulativeTrades | The collection of CumulativeTrade representing cumulative trades data. |

## [◆](https://docs.atas.net/en/)OnDataProviderChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.OnDataProviderChanged | ( | [IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md)? | oldDataProvider, |
| --- | --- | --- | --- |
| | | [IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md)? | newDataProvider |
| | ) | | |

protectedvirtual

This method is called when the value of the DataProvider property changes.

Parameters

| oldDataProvider | The old IIndicatorDataProvider value. |
| --- | --- |
| newDataProvider | The new IIndicatorDataProvider value. |

## [◆](https://docs.atas.net/en/)OnFixedProfilesResponse() [1/2]

| virtual void ATAS.Indicators.ExtendedIndicator.OnFixedProfilesResponse | ( | [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) | fixedProfile, |
| --- | --- | --- | --- |
| | | [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) | period |
| | ) | | |

protectedvirtual

## [◆](https://docs.atas.net/en/)OnFixedProfilesResponse() [2/2]

| virtual void ATAS.Indicators.ExtendedIndicator.OnFixedProfilesResponse | ( | [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) | fixedProfileScaled, |
| --- | --- | --- | --- |
| | | [IndicatorCandle](./classATAS_1_1Indicators_1_1IndicatorCandle.md) | fixedProfileOriginScale, |
| | | [FixedProfilePeriods](../namespaces/namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) | period |
| | ) | | |

protectedvirtual

This method is called when the online data provider responds with fixed profile data. Override this method in derived classes to handle the received fixed profile data.

Parameters

| fixedProfileScaled | The fixed profile data scaled to a specific period. |
| --- | --- |
| fixedProfileOriginScale | The fixed profile data without scaling. |
| period | The period of the fixed profile data (e.g., daily, weekly, monthly, etc.). |

## [◆](https://docs.atas.net/en/)OnMarketByOrdersChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.OnMarketByOrdersChanged | ( | IEnumerable | values | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles a collection of market by order data represented by MarketByOrder objects.

Parameters

| values | An IEnumerable of MarketByOrder representing the market by order data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnNewMyTrade()

| virtual void ATAS.Indicators.ExtendedIndicator.OnNewMyTrade | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | myTrade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when a new trade executed by the indicator's own strategy is received.

Parameters

| myTrade | The new trade executed by the indicator's own strategy. |
| --- | --- |

Reimplemented in [ATAS.Strategies.Chart.ChartStrategy](./classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md#a89782857bd22b78d7ec490bf4ce0c1b9).

## [◆](https://docs.atas.net/en/)OnNewOrder()

| virtual void ATAS.Indicators.ExtendedIndicator.OnNewOrder | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

This method is called when a new order is received.

Parameters

| order | The new order that was received. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnNewTrade()

| virtual void ATAS.Indicators.ExtendedIndicator.OnNewTrade | ( | [MarketDataArg](./classATAS_1_1Indicators_1_1MarketDataArg.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles an individual trade represented by a MarketDataArg object.

Parameters

| trade | The MarketDataArg representing the trade data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnNewTrades()

| virtual void ATAS.Indicators.ExtendedIndicator.OnNewTrades | ( | IEnumerable | trades | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles a collection of new trade data represented by MarketDataArg objects.

Parameters

| trades | An IEnumerable of MarketDataArg representing the new trade data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnOrderCancelFailed()

| virtual void ATAS.Indicators.ExtendedIndicator.OnOrderCancelFailed | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | string | message |
| | ) | | |

protectedvirtual

Called when an attempt to cancel an existing order fails.

Parameters

| order | The order that failed to be canceled. |
| --- | --- |
| message | The error message indicating the reason for the failure. |

## [◆](https://docs.atas.net/en/)OnOrderChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.OnOrderChanged | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Called when an existing order is modified (changed).

Parameters

| order | The modified order. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnOrderModifyFailed()

| virtual void ATAS.Indicators.ExtendedIndicator.OnOrderModifyFailed | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | newOrder, |
| | | string | error |
| | ) | | |

protectedvirtual

Called when an attempt to modify an existing order fails.

Parameters

| order | The order that failed to be modified. |
| --- | --- |
| newOrder | The new order with the modifications that failed to be applied. |
| error | The error message indicating the reason for the failure. |

## [◆](https://docs.atas.net/en/)OnOrderRegisterFailed()

| virtual void ATAS.Indicators.ExtendedIndicator.OnOrderRegisterFailed | ( | [Order](./classATAS_1_1DataFeedsCore_1_1Order.md) | order, |
| --- | --- | --- | --- |
| | | string | message |
| | ) | | |

protectedvirtual

Called when an attempt to register (place) a new order fails.

Parameters

| order | The order that failed to be registered. |
| --- | --- |
| message | The error message indicating the reason for the failure. |

## [◆](https://docs.atas.net/en/)OnPortfolioChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.OnPortfolioChanged | ( | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

This method is called when the value of the Portfolio changes.

Parameters

| portfolio | The new Portfolio value. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnPositionChanged()

| virtual void ATAS.Indicators.ExtendedIndicator.OnPositionChanged | ( | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

This method is called when the value of the Position changes.

Parameters

| position | The new Position value. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnRender()

| virtual void ATAS.Indicators.ExtendedIndicator.OnRender | ( | RenderContext | context, |
| --- | --- | --- | --- |
| | | [DrawingLayouts](../namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) | layout |
| | ) | | |

protectedvirtual

Override this method for implementing your own data rendering logic.

Parameters

| context | The rendering context used to draw on the chart. |
| --- | --- |
| layout | The layout information for the drawing. |

## [◆](https://docs.atas.net/en/)OnUpdateCumulativeTrade()

| virtual void ATAS.Indicators.ExtendedIndicator.OnUpdateCumulativeTrade | ( | [CumulativeTrade](./classATAS_1_1Indicators_1_1CumulativeTrade.md) | trade | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

Handles an update event for a cumulative trade represented by a CumulativeTrade object.

Parameters

| trade | The CumulativeTrade representing the updated cumulative trade data. |
| --- | --- |

## [◆](https://docs.atas.net/en/)OnVisibleChanged()

| override void ATAS.Indicators.ExtendedIndicator.OnVisibleChanged | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Called when the [Visible](./classATAS_1_1Indicators_1_1ChartObject.md#a2ec2052076ae8008763e45c94136f432) property changes.

Reimplemented from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#a0c345165dff8b0c687383c0bc8db7755).

## [◆](https://docs.atas.net/en/)RecalculateValues()

| override void ATAS.Indicators.ExtendedIndicator.RecalculateValues | ( | | ) | |
| --- | --- | --- | --- | --- |

protectedvirtual

Recalculate the indicator values on each bar.

Reimplemented from [ATAS.Indicators.BaseIndicator](./classATAS_1_1Indicators_1_1BaseIndicator.md#a6ee783f61e21eb5ff39be738b77dbd1e).

## [◆](https://docs.atas.net/en/)RedrawChart()

| void ATAS.Indicators.ExtendedIndicator.RedrawChart | ( | [RedrawArg](./classATAS_1_1Indicators_1_1RedrawArg.md)? | redrawArg = `null` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Call to redraw chart.

## [◆](https://docs.atas.net/en/)RequestFixedProfileAsync()

| async Task ATAS.Indicators.ExtendedIndicator.RequestFixedProfileAsync | ( | [FixedProfileRequest](./classATAS_1_1Indicators_1_1FixedProfileRequest.md) | request | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Asynchronously requests a fixed market profile parameterized with FixedProfileRequest.

Parameters

| request | The request containing the fixed profile period. |
| --- | --- |

## [◆](https://docs.atas.net/en/)RequestForCumulativeTrades()

| void ATAS.Indicators.ExtendedIndicator.RequestForCumulativeTrades | ( | [CumulativeTradesRequest](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) | request | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Sends a request for cumulative trades data to the online data provider.

Parameters

| request | The CumulativeTradesRequest object representing the request to be sent. |
| --- | --- |

## [◆](https://docs.atas.net/en/)SubscribeMarketByOrderData()

| Task ATAS.Indicators.ExtendedIndicator.SubscribeMarketByOrderData | ( | | ) | |
| --- | --- | --- | --- | --- |

protected

Subscribes to the market by order data.

## [◆](https://docs.atas.net/en/)SubscribeToDrawingEvents()

| void ATAS.Indicators.ExtendedIndicator.SubscribeToDrawingEvents | ( | [DrawingLayouts](../namespaces/namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) | flags | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Method for specifying the list of layouts in which rendering will be performed.

Parameters

| flags | The drawing layout flags indicating which events to subscribe to. |
| --- | --- |

## [◆](https://docs.atas.net/en/)SubscribeToTimer()

| void ATAS.Indicators.ExtendedIndicator.SubscribeToTimer | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period, |
| --- | --- | --- | --- |
| | | Action | callback |
| | ) | | |

Registers the callback and periodically calls it with specified period.Parameters

| period | A period of market data time used to repeatedly invoke the callback. |
| --- | --- |
| callback | A method that is called each time a specified period of market data time passes. |

Implements [ATAS.Indicators.IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a7a187460ab54c6669677fdb8a2d3153d).

## [◆](https://docs.atas.net/en/)TryGetService()

| bool ATAS.Indicators.ExtendedIndicator.TryGetService | ( | [NotNullWhen(true)] out T? | service | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)UnsubscribeFromTimer()

| void ATAS.Indicators.ExtendedIndicator.UnsubscribeFromTimer | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period, |
| --- | --- | --- | --- |
| | | Action | callback |
| | ) | | |

Unregister the callback from periodic invocations.Parameters

| period | Subscribed period of invocations. |
| --- | --- |
| callback | A method to be unregistered. |

Implements [ATAS.Indicators.IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a486f0f6bddd244284110a94baeecd595).

## Property Documentation

## [◆](https://docs.atas.net/en/)AlertsEnabled

| virtual bool ATAS.Indicators.ExtendedIndicator.AlertsEnabled = false |
| --- |

getset

## [◆](https://docs.atas.net/en/)Container

| [IIndicatorContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md)? ATAS.Indicators.ExtendedIndicator.Container |
| --- |

getset

Gets or sets the container in which the indicator is hosted.

## [◆](https://docs.atas.net/en/)DataProvider

| [IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md)? ATAS.Indicators.ExtendedIndicator.DataProvider |
| --- |

getset

Gets or sets the data provider for the indicator.

## [◆](https://docs.atas.net/en/)DenyToChangePanel

| bool ATAS.Indicators.ExtendedIndicator.DenyToChangePanel |
| --- |

getset

Gets or sets a value indicating whether changing the indicator's panel is denied. For vertical indicators, it is always denied.

## [◆](https://docs.atas.net/en/)DrawAbovePrice

| bool ATAS.Indicators.ExtendedIndicator.DrawAbovePrice |
| --- |

getset

Gets or sets a value indicating whether custom painting should be performed on top of rendered bars.

## [◆](https://docs.atas.net/en/)EnableCustomDrawing

| bool ATAS.Indicators.ExtendedIndicator.EnableCustomDrawing |
| --- |

getprotected set

Gets or sets a value indicating whether custom drawing is enabled for the indicator.

## [◆](https://docs.atas.net/en/)FullScreenMode

| bool ATAS.Indicators.ExtendedIndicator.FullScreenMode |
| --- |

getset

Gets or sets full screen mode for the indicator. Could be applied only to the vertical indicators('IsVerticalIndicator = true')

## [◆](https://docs.atas.net/en/)HorizontalLinesTillTouch

| List ATAS.Indicators.ExtendedIndicator.HorizontalLinesTillTouch = new() |
| --- |

get

Gets the collection of horizontal lines used to track touch points on the chart.

## [◆](https://docs.atas.net/en/)Labels

| SyncDictionary ATAS.Indicators.ExtendedIndicator.Labels = new() |
| --- |

getset

Gets or sets the collection of named drawing text elements.

## [◆](https://docs.atas.net/en/)MarketByOrders

| IEnumerable ATAS.Indicators.ExtendedIndicator.MarketByOrders |
| --- |

getprotected

Gets a snapshot of the current market by order data.

## [◆](https://docs.atas.net/en/)MarketTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.ExtendedIndicator.MarketTime |
| --- |

get

Current market time.

Implements [ATAS.Indicators.IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a719ea8d551a57d0673177c0d01931ffb).

## [◆](https://docs.atas.net/en/)Rectangles

| List ATAS.Indicators.ExtendedIndicator.Rectangles = new() |
| --- |

get

Gets the collection of rectangles on the chart.

## [◆](https://docs.atas.net/en/)ShowDescription

| bool ATAS.Indicators.ExtendedIndicator.ShowDescription |
| --- |

getset

Gets or sets a value indicating whether the description of the indicator should be shown.

## [◆](https://docs.atas.net/en/)TrendLines

| List ATAS.Indicators.ExtendedIndicator.TrendLines = new() |
| --- |

get

Gets the collection of trend lines on the chart.

## [◆](https://docs.atas.net/en/)UtcTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.ExtendedIndicator.UtcTime |
| --- |

get

Current UTC time.

Implements [ATAS.Indicators.IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#afd122a8c561de01a599f88f1477615f9).

The documentation for this class was generated from the following file:
- [BaseIndicator.cs](../files/BaseIndicator_8cs.md)
