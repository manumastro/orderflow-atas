# ATAS.Indicators Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1Indicators.html

| Namespaces | |
| --- | --- |
| namespace | [Attributies](./namespaceATAS_1_1Indicators_1_1Attributies.md) |
| | |
| namespace | [Drawing](./namespaceATAS_1_1Indicators_1_1Drawing.md) |
| | |
| namespace | [Filters](./namespaceATAS_1_1Indicators_1_1Filters.md) |
| | |
| namespace | [Heatmap](./namespaceATAS_1_1Indicators_1_1Heatmap.md) |
| | |

| Classes | |
| --- | --- |
| class | [BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) |
| | Base generic data series class providing common functionality. [More...](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md#details) |
| | |
| class | [BaseIndicator](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md) |
| | Base class for custom indicators in a chart. [More...](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md#details) |
| | |
| class | [Candle](../classes/classATAS_1_1Indicators_1_1Candle.md) |
| | Represents a candle in trading, which includes open, high, low, and close prices. [More...](../classes/classATAS_1_1Indicators_1_1Candle.md#details) |
| | |
| class | [CandleDataSeries](../classes/classATAS_1_1Indicators_1_1CandleDataSeries.md) |
| | Represents a data series of candles. Each element in the series is a Candle. [More...](../classes/classATAS_1_1Indicators_1_1CandleDataSeries.md#details) |
| | |
| class | [CandlePartSeries](../classes/classATAS_1_1Indicators_1_1CandlePartSeries.md) |
| | Represents a data series of decimal values derived from specific parts of an IndicatorCandle created by an ICandleCreator. [More...](../classes/classATAS_1_1Indicators_1_1CandlePartSeries.md#details) |
| | |
| class | [ChartObject](../classes/classATAS_1_1Indicators_1_1ChartObject.md) |
| | Base class for objects in a chart. [More...](../classes/classATAS_1_1Indicators_1_1ChartObject.md#details) |
| | |
| class | [Container](../classes/classATAS_1_1Indicators_1_1Container.md) |
| | Represents a container with a defined region on the chart. [More...](../classes/classATAS_1_1Indicators_1_1Container.md#details) |
| | |
| class | [CumulativeTrade](../classes/classATAS_1_1Indicators_1_1CumulativeTrade.md) |
| | Represents a cumulative trade, which is a trade that includes multiple prints or executions. [More...](../classes/classATAS_1_1Indicators_1_1CumulativeTrade.md#details) |
| | |
| class | [CumulativeTradesRequest](../classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) |
| | Represents a request to retrieve cumulative trade data within a specified time range or for a particular date. [More...](../classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#details) |
| | |
| class | [CustomValue](../classes/classATAS_1_1Indicators_1_1CustomValue.md) |
| | Represents a custom value with associated properties. [More...](../classes/classATAS_1_1Indicators_1_1CustomValue.md#details) |
| | |
| class | [CustomValueDataSeries](../classes/classATAS_1_1Indicators_1_1CustomValueDataSeries.md) |
| | Represents a custom data series that holds CustomValue objects. [More...](../classes/classATAS_1_1Indicators_1_1CustomValueDataSeries.md#details) |
| | |
| class | [DataSeriesTypeConverter](../classes/classATAS_1_1Indicators_1_1DataSeriesTypeConverter.md) |
| | |
| class | [ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md) |
| | An extended base class for custom indicators that provide additional functionality for drawing, alerts, market data handling, etc. [More...](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#details) |
| | |
| class | Extensions |
| | Contains extension methods for various types. |
| | |
| class | [Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) |
| | Generic filter class that implements the IFilterValue interface. [More...](../classes/classATAS_1_1Indicators_1_1Filter.md#details) |
| | |
| class | [FilterBase](../classes/classATAS_1_1Indicators_1_1FilterBase.md) |
| | Base class for filters implementing the IFilter interface. [More...](../classes/classATAS_1_1Indicators_1_1FilterBase.md#details) |
| | |
| class | [FilterBool](../classes/classATAS_1_1Indicators_1_1FilterBool.md) |
| | Represents a filter with a boolean value type. Inherits from Filter where TValue is set to bool and TFilter is set to FilterBool. [More...](../classes/classATAS_1_1Indicators_1_1FilterBool.md#details) |
| | |
| class | [FilterColor](../classes/classATAS_1_1Indicators_1_1FilterColor.md) |
| | Represents a filter with a value type of CrossColor. Inherits from Filter where TValue is set to CrossColor and TFilter is set to FilterColor. [More...](../classes/classATAS_1_1Indicators_1_1FilterColor.md#details) |
| | |
| class | [FilterEnum](../classes/classATAS_1_1Indicators_1_1FilterEnum.md) |
| | |
| class | FilterExtensions |
| | Provides extension methods for working with filters. |
| | |
| class | [FilterHeatmapTypes](../classes/classATAS_1_1Indicators_1_1FilterHeatmapTypes.md) |
| | Represents a filter for heatmap types with custom JSON serialization/deserialization. [More...](../classes/classATAS_1_1Indicators_1_1FilterHeatmapTypes.md#details) |
| | |
| class | [FilterInt](../classes/classATAS_1_1Indicators_1_1FilterInt.md) |
| | Represents a filter for integer values with custom JSON serialization/deserialization. [More...](../classes/classATAS_1_1Indicators_1_1FilterInt.md#details) |
| | |
| class | [FilterKey](../classes/classATAS_1_1Indicators_1_1FilterKey.md) |
| | Represents a filter for key values with custom JSON serialization/deserialization. [More...](../classes/classATAS_1_1Indicators_1_1FilterKey.md#details) |
| | |
| class | [FilterRangeBase](../classes/classATAS_1_1Indicators_1_1FilterRangeBase.md) |
| | Represents an abstract base class for filters that represent a range of values with custom JSON serialization/deserialization. [More...](../classes/classATAS_1_1Indicators_1_1FilterRangeBase.md#details) |
| | |
| class | [FilterRangeInt](../classes/classATAS_1_1Indicators_1_1FilterRangeInt.md) |
| | Represents a filter that represents a range of integer values with custom JSON serialization/deserialization. [More...](../classes/classATAS_1_1Indicators_1_1FilterRangeInt.md#details) |
| | |
| class | [FilterRangeValue](../classes/classATAS_1_1Indicators_1_1FilterRangeValue.md) |
| | Represents a range of values of type TValue with support for property change notifications. [More...](../classes/classATAS_1_1Indicators_1_1FilterRangeValue.md#details) |
| | |
| class | [FilterRenderPen](../classes/classATAS_1_1Indicators_1_1FilterRenderPen.md) |
| | Represents a filter for PenSettings objects with support for property change notifications. [More...](../classes/classATAS_1_1Indicators_1_1FilterRenderPen.md#details) |
| | |
| class | [FilterString](../classes/classATAS_1_1Indicators_1_1FilterString.md) |
| | Represents a filter for string values with support for property change notifications. [More...](../classes/classATAS_1_1Indicators_1_1FilterString.md#details) |
| | |
| class | [FilterTimeSpan](../classes/classATAS_1_1Indicators_1_1FilterTimeSpan.md) |
| | Represents a filter for TimeSpan values with custom JSON serialization/deserialization. [More...](../classes/classATAS_1_1Indicators_1_1FilterTimeSpan.md#details) |
| | |
| class | [FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) |
| | Represents a request for a fixed profile with a specific period. [More...](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md#details) |
| | |
| interface | [ICandleCreator](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md) |
| | Represents an interface for creating and managing indicator candles. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md#details) |
| | |
| interface | [IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) |
| | Interface for a chart containing various chart-related information and methods. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#details) |
| | |
| interface | [IChartColorsStore](../interfaces/interfaceATAS_1_1Indicators_1_1IChartColorsStore.md) |
| | Interface for accessing colors and pens used in a chart's rendering. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IChartColorsStore.md#details) |
| | |
| interface | [IChartContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md) |
| | Interface for a chart container that holds chart-related information and methods. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md#details) |
| | |
| interface | [IChartCoordinatesManager](../interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md) |
| | Interface for managing chart coordinates and scaling. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md#details) |
| | |
| interface | [IContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IContainer.md) |
| | Interface for defining a container that represents a rectangular region. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IContainer.md#details) |
| | |
| interface | [ICrossTradingIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md) |
| | Context for indicators that need to access cross-trading instrument data. Allows indicators to subscribe to cross-trading instrument changes and access market data for the selected cross-trading instrument without creating new subscriptions. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md#details) |
| | |
| interface | [IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) |
| | Interface for data series, providing essential properties and methods. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md#details) |
| | |
| interface | [IDrawingObjectsListInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IDrawingObjectsListInfo.md) |
| | Interface for providing information about the list of drawing objects on the chart. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IDrawingObjectsListInfo.md#details) |
| | |
| interface | [IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md) |
| | Represents a filter with common properties and functionality. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md#details) |
| | |
| interface | [IFilterEnum](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterEnum.md) |
| | |
| interface | [IFilterValue](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md) |
| | Represents a filter with a flexible value that can hold data of any type. Inherits the properties of IFilter. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md#details) |
| | |
| interface | IIndicatorAlerts |
| | |
| interface | [IIndicatorContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md) |
| | Interface for an indicator container that holds indicator-related information and methods. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md#details) |
| | |
| interface | [IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md) |
| | Represents a data provider for an indicator, providing access to various data and services related to the indicator. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md#details) |
| | |
| interface | [IIndicatorServiceProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorServiceProvider.md) |
| | |
| interface | [IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) |
| | Interface representing instrument information. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#details) |
| | |
| interface | [IIntCandle](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md) |
| | Represents an interface for an integer-based candle. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md#details) |
| | |
| interface | [IKeyboardInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IKeyboardInfo.md) |
| | Interface for providing information about the keyboard state. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IKeyboardInfo.md#details) |
| | |
| interface | [IKnowFixedProfiles](../interfaces/interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md) |
| | Represents an interface for objects that know fixed profiles and can request them. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md#details) |
| | |
| interface | [IMarketByOrdersCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) |
| | Interface for manager that provides access to market by orders cache. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md#details) |
| | |
| interface | [IMarketByOrdersDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md) |
| | Interface for manager that provides access to market by order data. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md#details) |
| | |
| interface | [IMarketByOrdersWithTradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) |
| | Interface for manager that provides access to market by orders and trades cache. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md#details) |
| | |
| interface | [IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md) |
| | Interface for providing market depth information. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md#details) |
| | |
| interface | [IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md) |
| | |
| interface | [IMouseLocationInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md) |
| | Interface for providing information about the mouse location within the chart. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md#details) |
| | |
| class | [Indicator](../classes/classATAS_1_1Indicators_1_1Indicator.md) |
| | Base class for custom indicators. [More...](../classes/classATAS_1_1Indicators_1_1Indicator.md#details) |
| | |
| class | [IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) |
| | Represents an Indicator Candle. [More...](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#details) |
| | |
| class | IndicatorCategories |
| | |
| class | [IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md) |
| | Implementation of the IIndicatorDataProvider interface that provides access to various data and services related to an indicator. [More...](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md#details) |
| | |
| class | [IndicatorSeries](../classes/classATAS_1_1Indicators_1_1IndicatorSeries.md) |
| | Represents a custom data series for an indicator, derived from BaseDataSeries. [More...](../classes/classATAS_1_1Indicators_1_1IndicatorSeries.md#details) |
| | |
| interface | [INotifyPanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md) |
| | Notifies clients that a panel property value has changed. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md#details) |
| | |
| class | [InstrumentInfo](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md) |
| | Implementation of the IInstrumentInfo interface representing instrument information. [More...](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md#details) |
| | |
| interface | [IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) |
| | Interface for an online data provider that provides access to real-time market data. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md#details) |
| | |
| interface | [IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) |
| | Interface for accessing platform settings. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md#details) |
| | |
| interface | [IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md) |
| | |
| interface | [IPropertiesEditorOwner](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md) |
| | |
| interface | [ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md) |
| | Represents an interface for supporting price information. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md#details) |
| | |
| interface | [ITimeMarketDataCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md) |
| | Cache that holds recent items based on a specified amount of time. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md#details) |
| | |
| interface | [ITradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITradesCache.md) |
| | Interface for manager that provides access to trades cache. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ITradesCache.md#details) |
| | |
| interface | [ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) |
| | Interface representing a trading manager for handling trading-related operations. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md#details) |
| | |
| interface | [ITradingVolumeInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md) |
| | Interface for providing information about the volume selector. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#details) |
| | |
| interface | [ITransientIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1ITransientIndicator.md) |
| | Marker interface for indicators that are managed programmatically and should not be persisted in the chart template. Transient indicators are excluded from serialization and are added/removed at runtime by platform components (e.g. ChartTraderViewModel). [More...](../interfaces/interfaceATAS_1_1Indicators_1_1ITransientIndicator.md#details) |
| | |
| interface | [IVolumeSelectorItem](../interfaces/interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md) |
| | Interface representing the volume template. [More...](../interfaces/interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md#details) |
| | |
| class | [LineSeries](../classes/classATAS_1_1Indicators_1_1LineSeries.md) |
| | Represents a horizontal line with a single value. [More...](../classes/classATAS_1_1Indicators_1_1LineSeries.md#details) |
| | |
| class | [MarketDataArg](../classes/classATAS_1_1Indicators_1_1MarketDataArg.md) |
| | Represents a data point in the market. [More...](../classes/classATAS_1_1Indicators_1_1MarketDataArg.md#details) |
| | |
| class | [MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) |
| | A class that implements the IMarketDepthInfoProvider interface to provide market depth information. [More...](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#details) |
| | |
| class | [MarketDepthSnapshot](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshot.md) |
| | Represents the end state of market depth over a specified time period. [More...](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshot.md#details) |
| | |
| class | [MarketDepthSnapshotRequest](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md) |
| | Represents a request to retrieve a snapshot of the market depth for a specified time range. [More...](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md#details) |
| | |
| class | [NotifyPropertyChangedBase](../classes/classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) |
| | Base class for implementing the INotifyPropertyChanged interface. [More...](../classes/classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#details) |
| | |
| class | [ObjectDataSeries](../classes/classATAS_1_1Indicators_1_1ObjectDataSeries.md) |
| | Represents a data series of objects, allowing storing any type of data elements. [More...](../classes/classATAS_1_1Indicators_1_1ObjectDataSeries.md#details) |
| | |
| class | [PaintbarsDataSeries](../classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) |
| | Represents a data series of paintbars, each element is a nullable CrossColor value. [More...](../classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md#details) |
| | |
| class | [ParameterAttribute](../classes/classATAS_1_1Indicators_1_1ParameterAttribute.md) |
| | |
| class | [PriceSelectionDataSeries](../classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md) |
| | Represents a data series of price selection values, each element is a synchronized list of PriceSelectionValue. [More...](../classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md#details) |
| | |
| class | [PriceSelectionValue](../classes/classATAS_1_1Indicators_1_1PriceSelectionValue.md) |
| | Represents a class for defining price level selection in clusters and bars. Using in PriceSelectionDataSeries. [More...](../classes/classATAS_1_1Indicators_1_1PriceSelectionValue.md#details) |
| | |
| class | [PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) |
| | Represents information on volumes at a specific price. [More...](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md#details) |
| | |
| class | [RangeDataSeries](../classes/classATAS_1_1Indicators_1_1RangeDataSeries.md) |
| | Represents a data series of range values, each element is a RangeValue. [More...](../classes/classATAS_1_1Indicators_1_1RangeDataSeries.md#details) |
| | |
| class | [RangeValue](../classes/classATAS_1_1Indicators_1_1RangeValue.md) |
| | RangeDataSeries element. [More...](../classes/classATAS_1_1Indicators_1_1RangeValue.md#details) |
| | |
| class | [RedrawArg](../classes/classATAS_1_1Indicators_1_1RedrawArg.md) |
| | Represents the arguments for requesting a redraw of a chart. [More...](../classes/classATAS_1_1Indicators_1_1RedrawArg.md#details) |
| | |
| class | [ValueArea](../classes/classATAS_1_1Indicators_1_1ValueArea.md) |
| | Represents information on Value area high/low. [More...](../classes/classATAS_1_1Indicators_1_1ValueArea.md#details) |
| | |
| class | [ValueChangingEventArgs](../classes/classATAS_1_1Indicators_1_1ValueChangingEventArgs.md) |
| | Provides event arguments for a value changing event. [More...](../classes/classATAS_1_1Indicators_1_1ValueChangingEventArgs.md#details) |
| | |
| class | [ValueDataSeries](../classes/classATAS_1_1Indicators_1_1ValueDataSeries.md) |
| | Represents a data series of decimal values, each element is a decimal. [More...](../classes/classATAS_1_1Indicators_1_1ValueDataSeries.md#details) |
| | |

| Typedefs | |
| --- | --- |
| using | [TimerSubscriptionKey](./namespaceATAS_1_1Indicators.md#ab0e6a198c6c000e4050a9a5452709e73) = (System.TimeSpan period, System.Action callback) |
| | |
| using | [BindingFlags](./namespaceATAS_1_1Indicators.md#a87ff3146cef928fdc248d4767003e608) = System.Reflection.BindingFlags |
| | |

| Enumerations | |
| --- | --- |
| enum | [DataSeriesType](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) {
  [Bars](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da226bea4d132b81f15f1bda87c76c6706)
, [Open](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dac3bf447eabe632720a3aa1a7ce401274)
, [High](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da655d20c1ca69519ca647684edbb2db35)
, [Low](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da28d0edd045e05cf5af64e35ae0c4c6ef)
,
  [Close](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dad3d2e617335f08df83599665eef8a418)
, [Volume](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dabd7a9717d29c5ddcab1bc175eda1e298)
, [HL2](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da3e803bf857da4fda82a23b2c5adef3d9)
, [HLC3](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dac8995b9bf6d0bea0171b395c57c2baf2)
,
  [OHLC4](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da005a4ddd5923d61b15d602dcf9f62697)
, [HLCC4](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dad2d5c8dc01940d012e4eb83bd9ef5c50)
, [Indicator](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da330d9b3991e7785c21cd29a452b56219)
, [Line](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da4803e6b9e63dabf04de980788d6a13c4)
,
  [Band](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da67fd95fc1e88f15b3efb9feef0fc0dc9)
, [Value](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c)
, [Candle](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415dae966b2e6caa2b8a2f49802e3baf6fbf2)
, [PriceSelection](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da6e3e08bb5389604aefcdc508da096b8c)
,
  [PaintBars](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415daf8dc14bed22af9d86544ca7a0f8aecfb)
, [CustomValue](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da37190c6433916dff5fa5b8012829a4fc)
, [Object](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da497031794414a552435f90151ac3b54b)

 } |
| | Enum representing different types of data series available. [More...](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) |
| | |
| enum | [CandleTooltipAnchor](./namespaceATAS_1_1Indicators.md#aa512d3c6bc2e3a31829de55b275883f0) { [Close](./namespaceATAS_1_1Indicators.md#aa512d3c6bc2e3a31829de55b275883f0ad3d2e617335f08df83599665eef8a418)
, [Top](./namespaceATAS_1_1Indicators.md#aa512d3c6bc2e3a31829de55b275883f0aa4ffdcf0dc1f31b9acaf295d75b51d00)
 } |
| | Determines which point of the candle is used as the tooltip anchor. [More...](./namespaceATAS_1_1Indicators.md#aa512d3c6bc2e3a31829de55b275883f0) |
| | |
| enum | [CandleVisualMode](./namespaceATAS_1_1Indicators.md#a4a6c44527d3fdaa0633b7c838f2fba06) { [Candles](./namespaceATAS_1_1Indicators.md#a4a6c44527d3fdaa0633b7c838f2fba06ad44b2e076424ef3bfa0554f01a86ef1e) = 0
, [Bars](./namespaceATAS_1_1Indicators.md#a4a6c44527d3fdaa0633b7c838f2fba06a226bea4d132b81f15f1bda87c76c6706) = 1
 } |
| | Visualization mode in CandleDataSeries. [More...](./namespaceATAS_1_1Indicators.md#a4a6c44527d3fdaa0633b7c838f2fba06) |
| | |
| enum | [ChartVisualModes](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51) {
  [Candles](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51ad44b2e076424ef3bfa0554f01a86ef1e) = 0
, [Clusters](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51a9b99bf7d2bfd4b09363304c166832734) = 1
, [TransparentCandles](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51a27aa66692f305e11b2f6474ec8a38e30) = 2
, [Line](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51a4803e6b9e63dabf04de980788d6a13c4) = 3
,
  [Bars](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51a226bea4d132b81f15f1bda87c76c6706) = 4
, [Hidden](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51a7acdf85c69cc3c5305456a293524386e) = 5

 } |
| | Enumerates the visual modes available for displaying price chart data on a chart. [More...](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51) |
| | |
| enum | [ContractRolloverType](./namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064) { [ExpirationDate](./namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064a1150f745c38866dca5e8fe88cc0652ee)
, [VolumeBasedCurrentEnd](./namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064a447bb7cf9c8aa206f68e20cc360d0d63)
, [VolumeBasedNextStart](./namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064a31397039b30ad07254e06cdd84c0a85c)
 } |
| | Specifies the type of contract rollover calculation to use. [More...](./namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064) |
| | |
| enum | [DrawingLayouts](./namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) { [None](./namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92a6adf97f83acf6453d4a6a4b1070f3754) = 1
, [Historical](./namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92a1df940294e43cce1f43fe5cd4e103b94) = 2
, [LatestBar](./namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92a65349d7a080770fb0fff1041069f8810) = 4
, [Final](./namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92abeae421a14a34f831c113f61323d1ab3) = 8
 } |
| | Enumerates the different drawing layouts available for chart drawings. [More...](./namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) |
| | |
| enum | [FootprintColorSchemes](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104f) {
  [Delta](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fadb1f4ab5845def61a83d5df13e0c2397) = 0
, [Solid](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fae41480b6bbfbf7407974a88d3d34f4fa) = 10
, [VolumeProportion](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fa85a48fd3947dd6e4e4284922c0ec0c60) = 20
, [TradesProportion](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104faa7d9f3abff80c508d0dbba66ad55e3f5) = 30
,
  [VolumeBasedBidAsk](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fa8002ad19f8e4d2d9acbc77d027774acb) = 50
, [HeatMapVolume](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fac6f5d83636c6559743dd8997fd3808f3) = 60
, [HeatMapTrades](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fada2da73d18faf4d8ee034e87dc74e56f) = 70
, [HeatMapDelta](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fadaea78dea7b08640900a3cff90d4e1f5) = 80
,
  [None](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104fa6adf97f83acf6453d4a6a4b1070f3754) = 90

 } |
| | |
| enum | [FootprintContentModes](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945) {
  [Volume](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945abd7a9717d29c5ddcab1bc175eda1e298) = 0
, [Trades](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945a18da2603c92b826445472937933895e5) = 10
, [VolumeTrades](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945aaf860deeea0cfc98ba7015c663d3df0d) = 30
, [VolumeDelta](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945acef691a61654acf53a8b82e651b35796) = 40
,
  [Delta](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945adb1f4ab5845def61a83d5df13e0c2397) = 50
, [DeltaCentered](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945af19026e820518458bdeebed166b276b5) = 51
, [BidXAsk](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945adb5063e859d44715fbcc28d08f1d8f19) = 60
, [BidAskCentered](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945a8a809cad93555f025efee6db65023cc0) = 70
,
  [BidAsk](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945aeaed505e954797866d4c800bb6760e1b) = 80
, [None](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945a6adf97f83acf6453d4a6a4b1070f3754) = 100

 } |
| | |
| enum | [FootprintVisualModes](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71) {
  [FullRow](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71acc6cdf92d89933d8fdb6cc044d882939)
, [BidAskHistogram](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71a447da9c44b1707cb6b136aa42b9e30ef)
, [VolumeHistogram](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71a6a6d9b511126a9248aa8aa175bdf876d)
, [TradesHistogram](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71a5529b45bf48119a299758eb32d2c3c8b)
,
  [DeltaHistogram](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71a0662264efe8dd4b425be503b954619b6)
, [BidAskLadder](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71ae69d3b4d5b103eecc985d5a900f697e8)
, [PositiveNegativeDeltaProfile](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71a979da49687f26ff1524cee6a5ae9ab74)

 } |
| | |
| enum | [MarketDataType](./namespaceATAS_1_1Indicators.md#abc76096c6e1b4aeece4c2a75798ffd97) { [Bid](./namespaceATAS_1_1Indicators.md#abc76096c6e1b4aeece4c2a75798ffd97ae36ba1e187ae2b3ebcfd0a4c68367caf) = 0
, [Ask](./namespaceATAS_1_1Indicators.md#abc76096c6e1b4aeece4c2a75798ffd97aa0b271a9d8aa8e7473922164d6a1c03c) = 1
, [Trade](./namespaceATAS_1_1Indicators.md#abc76096c6e1b4aeece4c2a75798ffd97a5f390d80b20daad8f5d2f483fb0ae9d8) = 2
 } |
| | Specifies the type of market data. [More...](./namespaceATAS_1_1Indicators.md#abc76096c6e1b4aeece4c2a75798ffd97) |
| | |
| enum | [ObjectType](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86) {
  [Ellipse](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86a119518c2134c46108179369f0ce81fa2)
, [Triangle](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86a5e5500cb2b82eb72d550de644bd1b64b)
, [Rectangle](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86ace9291906a4c3b042650b70d7f3b152e)
, [Diamond](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86a8f7671185d590914ac21c7511767b699)
,
  [OnlyCluster](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86adcc80746370b5e925f289273e545988f)

 } |
| | Enumeration representing different types of graphic objects for PriceSelectionDataSeries. [More...](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86) |
| | |
| enum | [FixedProfilePeriods](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) {
  [CurrentDay](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8a4daff4cb75e848f1acb354bd0497f959) = 0
, [LastDay](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8aeeefeee31fc765eebf1b91de608ec12b) = 1
, [CurrentWeek](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8ad9f73d726b6ea5189402fbbc014f6bf0) = 2
, [LastWeek](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8a702e97487c31fee5bf11b7c5c44a65cc) = 3
,
  [CurrentMonth](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8ae98028a6370c2d4924f8916e6f248982) = 4
, [LastMonth](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8a41eeb970a31e7ca35dbe1003b3694156) = 5
, [Contract](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8af49498143b94e78415d06029763412b9) = 6

 } |
| | Enumeration representing fixed profile periods. [More...](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) |
| | |
| enum | [SelectionType](./namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1d) { [Full](./namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1dabbd47109890259c0127154db1af26c75)
, [Bid](./namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1dae36ba1e187ae2b3ebcfd0a4c68367caf)
, [Ask](./namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1daa0b271a9d8aa8e7473922164d6a1c03c)
 } |
| | Enumeration representing different types of selection for a price level. [More...](./namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1d) |
| | |
| enum | [TradeDirection](./namespaceATAS_1_1Indicators.md#a2e2d29b6c1f8a1d9d05b264b58dd936f) { [Between](./namespaceATAS_1_1Indicators.md#a2e2d29b6c1f8a1d9d05b264b58dd936fa5ccb72b3b258508dc7918070eaeb214c) = 0
, [Buy](./namespaceATAS_1_1Indicators.md#a2e2d29b6c1f8a1d9d05b264b58dd936fa831a28f1e8df07c553fcd59546465d13) = 1
, [Sell](./namespaceATAS_1_1Indicators.md#a2e2d29b6c1f8a1d9d05b264b58dd936fa3068c5a98c003498f1fec0c489212e8b) = 2
 } |
| | Represents the possible trade directions. [More...](./namespaceATAS_1_1Indicators.md#a2e2d29b6c1f8a1d9d05b264b58dd936f) |
| | |
| enum | [VisualMode](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb) {
  [Line](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bba4803e6b9e63dabf04de980788d6a13c4)
, [Histogram](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bbac00e3b8460f0592cacf813fd68988ed4)
, [Hash](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bbafae8a9257e154175da4193dbf6552ef6)
, [Block](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bbae1e4c8c9ccd9fc39c391da4bcd093fb2)
,
  [Cross](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bbae76b449b9fc8536af7557ffa6321d269)
, [Square](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bbaceb46ca115d05c51aa5a16a8867c3304)
, [Dots](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bba4c72d232d1a1406e6c26251bbf6c3b25)
, [UpArrow](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bba0b513090f0a012194da83d5515882b13)
,
  [DownArrow](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bba0a3f89b5a028cc88370ad5a49dfab276)
, [OnlyValueOnAxis](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bba3efa035ec47f7012448f5e8ef743ca3f)
, [Hide](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bba62a5e490880a92eef74f167d9dc6dca0)

 } |
| | Represents the visual modes available for displaying data series on a chart. [More...](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb) |
| | |

| Functions | |
| --- | --- |
| record | [TradingSessionDescription](./namespaceATAS_1_1Indicators.md#adb47a1a8af062b79e5a66b131d4abadd) (long? Id, string Name) |
| | Representing a trading session description. |
| | |
| record struct | [FixedProfileResponse](./namespaceATAS_1_1Indicators.md#a77a4afd41de8b48767b2066b36570332) ([IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) Scaled, [IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) Original) |
| | |

## Typedef Documentation

## [◆](https://docs.atas.net/en/)BindingFlags

| using [ATAS.Indicators.BindingFlags](./namespaceATAS_1_1Indicators.md#a87ff3146cef928fdc248d4767003e608) = typedef System.Reflection.BindingFlags |
| --- |

## [◆](https://docs.atas.net/en/)TimerSubscriptionKey

| using [ATAS.Indicators.TimerSubscriptionKey](./namespaceATAS_1_1Indicators.md#ab0e6a198c6c000e4050a9a5452709e73) = typedef (System.TimeSpan period, System.Action callback) |
| --- |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)CandleTooltipAnchor

| enum [ATAS.Indicators.CandleTooltipAnchor](./namespaceATAS_1_1Indicators.md#aa512d3c6bc2e3a31829de55b275883f0) |
| --- |

Determines which point of the candle is used as the tooltip anchor.

| Enumerator | |
| --- | --- |
| Close | Tooltip is anchored to the Close value (default behavior). |
| Top | Tooltip is anchored to the top of the candle body — max(Open, Close). |

## [◆](https://docs.atas.net/en/)CandleVisualMode

| enum [ATAS.Indicators.CandleVisualMode](./namespaceATAS_1_1Indicators.md#a4a6c44527d3fdaa0633b7c838f2fba06) |
| --- |

Visualization mode in CandleDataSeries.

| Enumerator | |
| --- | --- |
| Candles | Candle visual mode where candles are displayed. |
| Bars | Candle visual mode where bars are displayed. |

## [◆](https://docs.atas.net/en/)ChartVisualModes

| enum [ATAS.Indicators.ChartVisualModes](./namespaceATAS_1_1Indicators.md#a1f568f925605d00957606a521399cc51) |
| --- |

Enumerates the visual modes available for displaying price chart data on a chart.

| Enumerator | |
| --- | --- |
| Candles | Represents the candles visual mode where price data is displayed using candlestick charts. |
| Clusters | Represents the clusters visual mode where price data is displayed using clusters. |
| TransparentCandles | Represents the transparent candles visual mode where price data is displayed using transparent candlestick charts. |
| Line | Represents the line visual mode where price data is displayed using line charts. |
| Bars | Represents the bars visual mode where price data is displayed using bar charts. |
| Hidden | Represents the hidden visual mode where price data is hidden and not displayed on the chart. |

## [◆](https://docs.atas.net/en/)ContractRolloverType

| enum [ATAS.Indicators.ContractRolloverType](./namespaceATAS_1_1Indicators.md#a56914a53d7fe9805b25bdbafbd33c064) |
| --- |

Specifies the type of contract rollover calculation to use.

| Enumerator | |
| --- | --- |
| ExpirationDate | Rollover based on predefined contract expiration dates, following standard futures market conventions. This method uses official exchange-mandated expiration dates for contract rollovers. |
| VolumeBasedCurrentEnd | Rollover determined when the current contract's activity ends, triggered by the next contract's daily trading volume exceeding the current contract's volume. This marks the end of effective trading period for the current contract. |
| VolumeBasedNextStart | Rollover determined when the new contract's activity begins, triggered by its daily trading volume exceeding the current contract's volume. This marks the start of effective trading period for the next contract. |

## [◆](https://docs.atas.net/en/)DataSeriesType

| enum [ATAS.Indicators.DataSeriesType](./namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415d) |
| --- |

Enum representing different types of data series available.

| Enumerator | |
| --- | --- |
| Bars | Represents a data series based on bars. |
| Open | Represents a data series containing open prices. |
| High | Represents a data series containing high prices. |
| Low | Represents a data series containing low prices. |
| Close | Represents a data series containing closing prices. |
| Volume | Represents a data series containing volume data. |
| HL2 | Represents a data series containing the average of high and low prices. |
| HLC3 | Represents a data series containing the average of high, low, and closing prices. |
| OHLC4 | Represents a data series containing the open, high, low, and closing prices. |
| HLCC4 | Represents a data series containing the average of high, low and double closing prices. |
| Indicator | Represents a data series sourced from an indicator. |
| Line | Represents a line-based data series. |
| Band | Represents a band-based data series. |
| Value | Represents a value-based data series. |
| Candle | Represents a data series containing candle information. |
| PriceSelection | Represents a data series used for selecting prices. |
| PaintBars | Represents a data series used for painting bars. |
| CustomValue | Represents a custom value-based data series. |
| Object | Represents a data series containing objects. |

## [◆](https://docs.atas.net/en/)DrawingLayouts

| enum [ATAS.Indicators.DrawingLayouts](./namespaceATAS_1_1Indicators.md#a0627a7e4e0567e98e57ebbbe3d698f92) |
| --- |

Enumerates the different drawing layouts available for chart drawings.

| Enumerator | |
| --- | --- |
| None | No specific layout is applied. |
| Historical | Redrawing will be called on each new candle, when compressing, moving the chart. |
| LatestBar | Redraw is called when the most recent bar changes. As a rule, this happens on every new tick. |
| Final | The final layout that is drawn every time the chart is drawn. For example, when moving the mouse. |

## [◆](https://docs.atas.net/en/)FixedProfilePeriods

| enum [ATAS.Indicators.FixedProfilePeriods](./namespaceATAS_1_1Indicators.md#ac4558b9fb9dcffd35f0b7f3df614afc8) |
| --- |

Enumeration representing fixed profile periods.

| Enumerator | |
| --- | --- |
| CurrentDay | Current day profile. |
| LastDay | Previous day profile. |
| CurrentWeek | Current week profile. |
| LastWeek | Previous week profile. |
| CurrentMonth | Current month profile. |
| LastMonth | Previous month profile. |
| Contract | Contract profile. |

## [◆](https://docs.atas.net/en/)FootprintColorSchemes

| enum [ATAS.Indicators.FootprintColorSchemes](./namespaceATAS_1_1Indicators.md#a5764ad98efd541ef44c9cca241b6104f) |
| --- |

| Enumerator | |
| --- | --- |
| Delta | |
| Solid | |
| VolumeProportion | |
| TradesProportion | |
| VolumeBasedBidAsk | |
| HeatMapVolume | |
| HeatMapTrades | |
| HeatMapDelta | |
| None | |

## [◆](https://docs.atas.net/en/)FootprintContentModes

| enum [ATAS.Indicators.FootprintContentModes](./namespaceATAS_1_1Indicators.md#a422215f32e9130e2a677150969923945) |
| --- |

| Enumerator | |
| --- | --- |
| Volume | |
| Trades | |
| VolumeTrades | |
| VolumeDelta | |
| Delta | |
| DeltaCentered | |
| BidXAsk | |
| BidAskCentered | |
| BidAsk | |
| None | |

## [◆](https://docs.atas.net/en/)FootprintVisualModes

| enum [ATAS.Indicators.FootprintVisualModes](./namespaceATAS_1_1Indicators.md#a2b79bba385c27019e00a1e88ef574f71) |
| --- |

| Enumerator | |
| --- | --- |
| FullRow | |
| BidAskHistogram | |
| VolumeHistogram | |
| TradesHistogram | |
| DeltaHistogram | |
| BidAskLadder | |
| PositiveNegativeDeltaProfile | |

## [◆](https://docs.atas.net/en/)MarketDataType

| enum [ATAS.Indicators.MarketDataType](./namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) |
| --- |

Specifies the type of market data.

| Enumerator | |
| --- | --- |
| Bid | Represents the bid data type in the market. |
| Ask | Represents the ask data type in the market. |
| Trade | Represents the trade data type in the market. |

## [◆](https://docs.atas.net/en/)ObjectType

| enum [ATAS.Indicators.ObjectType](./namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86) |
| --- |

Enumeration representing different types of graphic objects for PriceSelectionDataSeries.

| Enumerator | |
| --- | --- |
| Ellipse | Ellipse graphic object. |
| Triangle | Triangle graphic object. |
| Rectangle | Rectangle graphic object. |
| Diamond | Diamond graphic object. |
| OnlyCluster | Graphic object used only for clustering. |

## [◆](https://docs.atas.net/en/)SelectionType

| enum [ATAS.Indicators.SelectionType](./namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1d) |
| --- |

Enumeration representing different types of selection for a price level.

| Enumerator | |
| --- | --- |
| Full | Select the full level. |
| Bid | Select the bid side. |
| Ask | Select the ask side. |

## [◆](https://docs.atas.net/en/)TradeDirection

| enum [ATAS.Indicators.TradeDirection](./namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) |
| --- |

Represents the possible trade directions.

| Enumerator | |
| --- | --- |
| Between | The volume of intra-spread transactions at the level. |
| Buy | Indicates a buy trade direction. |
| Sell | Indicates a sell trade direction. |

## [◆](https://docs.atas.net/en/)VisualMode

| enum [ATAS.Indicators.VisualMode](./namespaceATAS_1_1Indicators.md#adeb10dddb4f4107e7d5031c941fdc0bb) |
| --- |

Represents the visual modes available for displaying data series on a chart.

| Enumerator | |
| --- | --- |
| Line | Data series is displayed as a line connecting data points. |
| Histogram | Data series is displayed as a vertical column on each bar. |
| Hash | Data series is displayed as a horizontal line on each bar. |
| Block | Data series is displayed as a square block on each bar. |
| Cross | Data series is displayed as a cross on each bar. |
| Square | The data series is displayed as a 90 degree polyline connecting the data points. |
| Dots | Data series is displayed as a circle on each bar. |
| UpArrow | Data series is displayed as a up arrow on each bar. |
| DownArrow | Data series is displayed as a down arrow on each bar. |
| OnlyValueOnAxis | Data series is displayed as values only on the axis without connecting lines or shapes. |
| Hide | Data series is hidden and not displayed on the chart. |

## Function Documentation

## [◆](https://docs.atas.net/en/)FixedProfileResponse()

| record struct ATAS.Indicators.FixedProfileResponse | ( | [IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) | Scaled, |
| --- | --- | --- | --- |
| | | [IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) | Original |
| | ) | | |

## [◆](https://docs.atas.net/en/)TradingSessionDescription()

| record ATAS.Indicators.TradingSessionDescription | ( | long? | Id, |
| --- | --- | --- | --- |
| | | string | Name |
| | ) | | |

Representing a trading session description.
