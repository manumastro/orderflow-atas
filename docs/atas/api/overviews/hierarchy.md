# Class Hierarchy

Source: https://docs.atas.net/en/hierarchy.html

[Go to the graphical class hierarchy](./inherits.md)

This inheritance list is sorted roughly, but not completely, alphabetically:

[detail level 123456]

| C[ATAS.DataFeedsCore.AsyncConnector.AsyncAwaiterFacade](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterFacade.md) | Facade for `SynchronizationContextAwaiter` to allow explicit call of `SwitchToConnectorThreadAsync()` |
| --- | --- |
| C[ATAS.DataFeedsCore.AsyncConnector.AsyncAwaiterQueueFacade](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterQueueFacade.md) | Facade for `SynchronizationContextQueueAwaiter` to allow explicit call of `ForceToConnectorThreadAsync()` |
| C[ATAS.DataFeedsCore.AsyncConnectorMessage](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md) | Service message for passing continuations to the connector thread |
| ►CAttribute | |
| C[ATAS.Indicators.Heatmap.HeatmapIndicatorAttribute](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md) | Marks a class as a heatmap indicator type and supplies discovery metadata. Apply to a concrete class that derives from HeatmapIndicator (or implements IHeatmapIndicator directly) |
| C[OFT.Attributes.Editors.CheckEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1CheckEditorAttribute.md) | Check box editor for object |
| ►C[OFT.Attributes.Editors.ComboBoxEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md) | |
| C[OFT.Attributes.Editors.SoundComboBoxEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.md) | Specialized ComboBox editor for internal application sounds. Renders non-editable list with a Play button in item template |
| C[OFT.Attributes.Editors.DataSeriesEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1DataSeriesEditorAttribute.md) | |
| C[OFT.Attributes.Editors.IsExpandedAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1IsExpandedAttribute.md) | |
| C[OFT.Attributes.Editors.MaskAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1MaskAttribute.md) | |
| C[OFT.Attributes.Editors.NumericEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1NumericEditorAttribute.md) | |
| C[OFT.Attributes.Editors.PostValueModeAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1PostValueModeAttribute.md) | |
| C[OFT.Attributes.Editors.SelectDirectoryEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SelectDirectoryEditorAttribute.md) | |
| C[OFT.Attributes.Editors.SelectFileEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SelectFileEditorAttribute.md) | |
| C[OFT.Attributes.Editors.TextEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md) | Represents an attribute that provides additional metadata for controlling text editors |
| C[OFT.Attributes.IgnoreCloneAttribute](../classes/classOFT_1_1Attributes_1_1IgnoreCloneAttribute.md) | Ignore clone property or field |
| C[OFT.Attributes.LogoAttribute](../classes/classOFT_1_1Attributes_1_1LogoAttribute.md) | |
| C[OFT.Attributes.MappingAttribute](../classes/classOFT_1_1Attributes_1_1MappingAttribute.md) | |
| C[OFT.Attributes.ReferralLinkAttribute](../classes/classOFT_1_1Attributes_1_1ReferralLinkAttribute.md) | |
| C[OFT.Attributes.RegisterLinkAttribute](../classes/classOFT_1_1Attributes_1_1RegisterLinkAttribute.md) | |
| C[OFT.Attributes.SupportedExchangesAttribute](../classes/classOFT_1_1Attributes_1_1SupportedExchangesAttribute.md) | Specifies the exchange codes supported by a connector. The attribute accepts a type containing public const string fields, each representing a supported exchange code |
| C[OFT.Attributes.TabAttribute](../classes/classOFT_1_1Attributes_1_1TabAttribute.md) | Attribute that assigns a property or all properties of a class to a named tab in the settings UI. Use alongside System.ComponentModel.DataAnnotations.DisplayAttribute for property display metadata (Name, GroupName, Description, Order) |
| C[OFT.Attributes.VisibleWhenAttribute](../classes/classOFT_1_1Attributes_1_1VisibleWhenAttribute.md) | Makes the property visible only when the specified source property's value matches one of the provided values. Used for dynamic property visibility in the settings UI (e.g., showing/hiding properties based on a mode enum) |
| C[ATAS.Indicators.ValueDataSeries.BarColors](../classes/classATAS_1_1Indicators_1_1ValueDataSeries_1_1BarColors.md) | Manages colors per bar for the associated ValueDataSeries |
| ►C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| ►C[ATAS.DataFeedsCore.AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) | Connector base that allows to use `await` to switch to the connector queue thread |
| C[ATAS.DataFeedsCore.CryptoAsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md) | |
| ►C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| C[ATAS.DataFeedsCore.CryptoBaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoBaseConnector.md) | |
| ►C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| ►CBaseConnectorSettings | |
| ►C[OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md) | |
| C[ATAS.DataFeedsCore.BaseMarketDataOnlyConnectorSettings](../classes/classATAS_1_1DataFeedsCore_1_1BaseMarketDataOnlyConnectorSettings.md) | |
| C[ATAS.DataFeedsCore.BasketConnectorSettings](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnectorSettings.md) | |
| ►C[ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| C[ATAS.Indicators.CandleDataSeries](../classes/classATAS_1_1Indicators_1_1CandleDataSeries.md) | Represents a data series of candles. Each element in the series is a Candle |
| ►C[ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| C[ATAS.Indicators.PaintbarsDataSeries](../classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) | Represents a data series of paintbars, each element is a nullable CrossColor value |
| ►C[ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| C[ATAS.Indicators.CustomValueDataSeries](../classes/classATAS_1_1Indicators_1_1CustomValueDataSeries.md) | Represents a custom data series that holds CustomValue objects |
| ►C[ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| C[ATAS.Indicators.CandlePartSeries](../classes/classATAS_1_1Indicators_1_1CandlePartSeries.md) | Represents a data series of decimal values derived from specific parts of an IndicatorCandle created by an ICandleCreator |
| C[ATAS.Indicators.IndicatorSeries](../classes/classATAS_1_1Indicators_1_1IndicatorSeries.md) | Represents a custom data series for an indicator, derived from BaseDataSeries |
| C[ATAS.Indicators.LineSeries](../classes/classATAS_1_1Indicators_1_1LineSeries.md) | Represents a horizontal line with a single value |
| C[ATAS.Indicators.ValueDataSeries](../classes/classATAS_1_1Indicators_1_1ValueDataSeries.md) | Represents a data series of decimal values, each element is a decimal |
| ►C[ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| C[ATAS.Indicators.ObjectDataSeries](../classes/classATAS_1_1Indicators_1_1ObjectDataSeries.md) | Represents a data series of objects, allowing storing any type of data elements |
| ►C[ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| C[ATAS.Indicators.RangeDataSeries](../classes/classATAS_1_1Indicators_1_1RangeDataSeries.md) | Represents a data series of range values, each element is a RangeValue |
| ►C[ATAS.Indicators.BaseDataSeries >](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | |
| C[ATAS.Indicators.PriceSelectionDataSeries](../classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md) | Represents a data series of price selection values, each element is a synchronized list of PriceSelectionValue |
| ►C[ATAS.Indicators.Drawing.BaseDrawingText](../classes/classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md) | Represents a base class for drawing text on a chart |
| C[ATAS.Indicators.Drawing.DrawingText](../classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) | Represents a class for drawing text on a chart with additional alignment options |
| ►CBaseLoggerSource | |
| C[ATAS.DataFeedsCore.AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md) | |
| C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| C[ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| C[ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md) | |
| C[ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md) | |
| C[ATAS.DataFeedsCore.ConnectorWebsocket.ConnectorWebsocket](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md) | |
| C[ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | |
| C[ATAS.DataFeedsCore.Rebate.WhiteListHttpClient](../classes/classATAS_1_1DataFeedsCore_1_1Rebate_1_1WhiteListHttpClient.md) | |
| ►C[ATAS.DataFeedsCore.SessionServer.SessionInfo](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md) | |
| C[ATAS.DataFeedsCore.SessionServer.SocketSession](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md) | |
| C[ATAS.DataFeedsCore.SessionServer.SessionServer](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md) | |
| ►C[ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingLoggerSource](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md) | |
| ►C[ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md) | |
| C[ATAS.DataFeedsCore.TradeStatistics.Matching.TradesMatchingProcessor](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md) | |
| ►C[ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md) | Base class for implementing trading strategies |
| ►C[ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| C[ATAS.Strategies.ATM.BaseStopProfitStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| ►C[ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| C[ATAS.DataFeedsCore.MultiConnectorMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1MultiConnectorMessageQueue.md) | |
| ►C[ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| C[ATAS.DataFeedsCore.SimpleMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md) | |
| ►C[ATAS.Strategies.ATM.BaseStopProfitStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| C[ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) | |
| C[ATAS.Indicators.Candle](../classes/classATAS_1_1Indicators_1_1Candle.md) | Represents a candle in trading, which includes open, high, low, and close prices |
| C[ATAS.Strategies.ATM.ChangesInfo](../structs/structATAS_1_1Strategies_1_1ATM_1_1ChangesInfo.md) | |
| C[ATAS.DataFeedsCore.Commissions.CommissionRulesGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRulesGroup.md) | |
| C[ATAS.DataFeedsCore.Commissions.CommissionRuleType](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRuleType.md) | |
| C[ATAS.Indicators.CumulativeTrade](../classes/classATAS_1_1Indicators_1_1CumulativeTrade.md) | Represents a cumulative trade, which is a trade that includes multiple prints or executions |
| C[ATAS.Indicators.CumulativeTradesRequest](../classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) | Represents a request to retrieve cumulative trade data within a specified time range or for a particular date |
| C[ATAS.Indicators.CustomValue](../classes/classATAS_1_1Indicators_1_1CustomValue.md) | Represents a custom value with associated properties |
| C[ATAS.DataFeedsCore.DailyNote](../classes/classATAS_1_1DataFeedsCore_1_1DailyNote.md) | |
| ►CDataConnection | |
| C[ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md) | |
| C[ATAS.DataFeedsCore.Dom.DomBuilder](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md) | Builds and maintains a DOM for a connector
 Allows to obtain best prices if connector does not give them |
| C[ATAS.DataFeedsCore.Dom.DomBuilder.DomChangesTracker](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md) | |
| C[ATAS.Indicators.Drawing.DrawingRectangle](../classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md) | Represents a rectangle drawn on a chart |
| C[ATAS.Indicators.Heatmap.Internal.HeatmapIndicatorChunkedSeriesStorage.Enumerator](../structs/structATAS_1_1Indicators_1_1Heatmap_1_1Internal_1_1HeatmapIndicatorChunkedSeriesStorage_1_1Enumerator.md) | Wait-free forward enumerator over committed samples. Captures the committed count and the head chunk reference at construction time; the renderer holds it for the duration of one read pass and discards it. Subsequent commits do not affect the enumerator's visible range |
| ►CEventArgs | |
| C[ATAS.DataFeedsCore.ConnectionStateEventArgs](../classes/classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.md) | |
| C[ATAS.Strategies.StrategyNotificationEventArgs](../classes/classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md) | Provides data for the StrategyNotification event |
| C[ATAS.Strategies.StrategyStateChangedEventArgs](../classes/classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md) | Provides data for the StrategyStateChanged event |
| ►CException | |
| C[ATAS.DataFeedsCore.ConnectorWebsocket.WebsocketException](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1WebsocketException.md) | |
| C[ATAS.DataFeedsCore.Database.DatabaseException](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseException.md) | |
| C[ATAS.DataFeedsCore.Exceptions.ConnectorNotConnectedException](../classes/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ConnectorNotConnectedException.md) | |
| C[ATAS.DataFeedsCore.Exceptions.ExchangeException](../classes/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.md) | An exception means the Exchange sent us an logic error like violating business rules It allows us to separate network errors and logic errors |
| C[ATAS.DataFeedsCore.Exceptions.FatalConnectionErrorEcxeption](../classes/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1FatalConnectionErrorEcxeption.md) | |
| ►CExpandableObjectConverter | |
| C[ATAS.Indicators.DataSeriesTypeConverter](../classes/classATAS_1_1Indicators_1_1DataSeriesTypeConverter.md) | |
| ►CUtils.Common.Attributes.FeatureIdAttribute | |
| ►C[OFT.Attributes.FeatureIdAttribute](../classes/classOFT_1_1Attributes_1_1FeatureIdAttribute.md) | |
| C[ATAS.Indicators.Attributies.FeatureId](../classes/classATAS_1_1Indicators_1_1Attributies_1_1FeatureId.md) | |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterBool](../classes/classATAS_1_1Indicators_1_1FilterBool.md) | Represents a filter with a boolean value type. Inherits from Filter where TValue is set to bool and TFilter is set to FilterBool |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterColor](../classes/classATAS_1_1Indicators_1_1FilterColor.md) | Represents a filter with a value type of CrossColor. Inherits from Filter where TValue is set to CrossColor and TFilter is set to FilterColor |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterKey](../classes/classATAS_1_1Indicators_1_1FilterKey.md) | Represents a filter for key values with custom JSON serialization/deserialization |
| ►C[ATAS.Indicators.Filter, TFilter >](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterRangeBase](../classes/classATAS_1_1Indicators_1_1FilterRangeBase.md) | Represents an abstract base class for filters that represent a range of values with custom JSON serialization/deserialization |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterHeatmapTypes](../classes/classATAS_1_1Indicators_1_1FilterHeatmapTypes.md) | Represents a filter for heatmap types with custom JSON serialization/deserialization |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterInt](../classes/classATAS_1_1Indicators_1_1FilterInt.md) | Represents a filter for integer values with custom JSON serialization/deserialization |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterRenderPen](../classes/classATAS_1_1Indicators_1_1FilterRenderPen.md) | Represents a filter for PenSettings objects with support for property change notifications |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterString](../classes/classATAS_1_1Indicators_1_1FilterString.md) | Represents a filter for string values with support for property change notifications |
| ►C[ATAS.Indicators.Filter >](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterEnum](../classes/classATAS_1_1Indicators_1_1FilterEnum.md) | |
| ►C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.FilterTimeSpan](../classes/classATAS_1_1Indicators_1_1FilterTimeSpan.md) | Represents a filter for TimeSpan values with custom JSON serialization/deserialization |
| ►C[ATAS.Indicators.Filter >](../classes/classATAS_1_1Indicators_1_1Filter.md) | |
| C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | Generic filter class that implements the IFilterValue interface |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverter.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterBoolJsonConvert](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterBoolJsonConvert.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterColorJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterColorJsonConverter.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterHeatmapTypesJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterHeatmapTypesJsonConverter.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterIntJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterIntJsonConverter.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterKeyJsonConvert](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterKeyJsonConvert.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterRenderPenJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterStringJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterStringJsonConverter.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterTimeSpanJsonConvert](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterTimeSpanJsonConvert.md) | |
| ►C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase >](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterRangeJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRangeJsonConverterBase.md) | |
| ►C[ATAS.Indicators.FilterRangeBase](../classes/classATAS_1_1Indicators_1_1FilterRangeBase.md) | |
| C[ATAS.Indicators.FilterRangeInt](../classes/classATAS_1_1Indicators_1_1FilterRangeInt.md) | Represents a filter that represents a range of integer values with custom JSON serialization/deserialization |
| ►C[ATAS.Indicators.Filters.Converters.FilterRangeJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRangeJsonConverterBase.md) | |
| C[ATAS.Indicators.Filters.Converters.FilterRangeIntJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRangeIntJsonConverter.md) | |
| C[ATAS.Indicators.FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) | Represents a request for a fixed profile with a specific period |
| C[ATAS.DataFeedsCore.GroupExchange](../classes/classATAS_1_1DataFeedsCore_1_1GroupExchange.md) | |
| C[ATAS.Indicators.Heatmap.HeatmapIndicatorDescriptorBuilder](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) | Fluent builder that produces an immutable HeatmapIndicatorDescriptor alongside the typed visual / series handles required by the state builder. Single-shot: each builder yields exactly one descriptor via Done; further mutation throws |
| C[ATAS.Indicators.Heatmap.HeatmapIndicatorFallbackReWarmGuard](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md) | State holder for indicators whose calculation is anchored at the real data start (e.g. CVD `FromDataStart`, VWAP `FromDataStart`) and may receive a fallback-range warm-up before the host knows the real data start. Encapsulates the latch protocol so the indicator does not have to track three flags and an inline check by hand |
| C[ATAS.Indicators.Heatmap.HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) | Strongly typed handle for a series within a visual. Returned from HeatmapIndicatorVisualHandle.Series(string, HeatmapIndicatorSeriesRole, HeatmapIndicatorValueKind, System.Func, HeatmapIndicatorVisualStyle?, string?, string?); the constructor is internal so authors cannot fabricate one. TValue is the indicator-internal sample type — what the indicator's calculation produces. The handle also carries the projection that converts the typed sample to a renderer-facing decimal; the lease applies the projection inline on every Append so the chunked storage holds decimal samples ready for the renderer |
| C[ATAS.Indicators.Heatmap.HeatmapIndicatorVisualHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | Strongly typed handle for a visual added to a descriptor via HeatmapIndicatorDescriptorBuilder. The handle captures the owning descriptor's identity so the state builder can reject handles from a different descriptor at runtime, and the constructor is internal so authors cannot fabricate handles by hand |
| ►CUtils.Common.Attributes.HelpLinkAttribute | |
| C[OFT.Attributes.HelpLinkAttribute](../classes/classOFT_1_1Attributes_1_1HelpLinkAttribute.md) | |
| C[ATAS.DataFeedsCore.HistoryMyTradePlaybook](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTradePlaybook.md) | |
| C[ATAS.Indicators.Drawing.HorizontalChannel](../classes/classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md) | Represents a horizontal line on a chart |
| C[ATAS.Indicators.Drawing.HorizontalLine](../classes/classATAS_1_1Indicators_1_1Drawing_1_1HorizontalLine.md) | Represents a horizontal line on a chart |
| C[ATAS.Indicators.ICandleCreator](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md) | Represents an interface for creating and managing indicator candles |
| C[ATAS.Indicators.IChartColorsStore](../interfaces/interfaceATAS_1_1Indicators_1_1IChartColorsStore.md) | Interface for accessing colors and pens used in a chart's rendering |
| C[ATAS.Indicators.IChartCoordinatesManager](../interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md) | Interface for managing chart coordinates and scaling |
| ►CICloneable | |
| ►C[ATAS.Indicators.IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md) | Represents a filter with common properties and functionality |
| ►C[ATAS.Indicators.FilterBase](../classes/classATAS_1_1Indicators_1_1FilterBase.md) | Base class for filters implementing the IFilter interface |
| C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | Generic filter class that implements the IFilterValue interface |
| ►C[ATAS.Indicators.IFilterEnum](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterEnum.md) | |
| C[ATAS.Indicators.FilterEnum](../classes/classATAS_1_1Indicators_1_1FilterEnum.md) | |
| ►C[ATAS.Indicators.IFilterValue](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md) | Represents a filter with a flexible value that can hold data of any type. Inherits the properties of IFilter |
| C[ATAS.Indicators.Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | Generic filter class that implements the IFilterValue interface |
| ►C[ATAS.DataFeedsCore.Commissions.ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md) | |
| ►C[ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md) | |
| C[ATAS.DataFeedsCore.Commissions.PerContractCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerContractCommissionRule.md) | |
| C[ATAS.DataFeedsCore.Commissions.PerTradeCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerTradeCommissionRule.md) | |
| C[ATAS.DataFeedsCore.Commissions.PercentCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PercentCommissionRule.md) | |
| ►CIComparer | |
| C[ATAS.DataFeedsCore.Dom.PriceComparer](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1PriceComparer.md) | |
| C[ATAS.DataFeedsCore.MarketDepthComparer](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepthComparer.md) | |
| C[ATAS.DataFeedsCore.IConnectorExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) | Defines methods to retrieve exchange information based on security or exchange codes |
| ►C[ATAS.DataFeedsCore.IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | |
| C[ATAS.DataFeedsCore.ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md) | |
| ►C[OFT.Core.IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) | |
| C[OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md) | |
| C[OFT.Core.IConnectorSettingsSupportInitialization](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsSupportInitialization.md) | |
| C[OFT.Core.ILoginPasswordConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ILoginPasswordConnectorSettings.md) | |
| C[OFT.Core.ITypedConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ITypedConnectorSettings.md) | |
| C[OFT.Core.IConnectorSettingsAction](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsAction.md) | |
| ►C[ATAS.Indicators.IContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IContainer.md) | Interface for defining a container that represents a rectangular region |
| C[ATAS.Indicators.Container](../classes/classATAS_1_1Indicators_1_1Container.md) | Represents a container with a defined region on the chart |
| C[ATAS.Indicators.IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) | Interface for a chart containing various chart-related information and methods |
| C[ATAS.Indicators.IChartContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md) | Interface for a chart container that holds chart-related information and methods |
| C[ATAS.Indicators.IIndicatorContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md) | Interface for an indicator container that holds indicator-related information and methods |
| C[ATAS.Indicators.ICrossTradingIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md) | Context for indicators that need to access cross-trading instrument data. Allows indicators to subscribe to cross-trading instrument changes and access market data for the selected cross-trading instrument without creating new subscriptions |
| ►CICustomTypeDescriptor | |
| C[ATAS.DataFeedsCore.SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | |
| ►C[ATAS.Indicators.ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md) | An extended base class for custom indicators that provide additional functionality for drawing, alerts, market data handling, etc |
| ►C[ATAS.Indicators.Indicator](../classes/classATAS_1_1Indicators_1_1Indicator.md) | Base class for custom indicators |
| ►C[ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md) | Represents an abstract class for a chart strategy that extends the functionality of an Indicator and implements the IChartStrategy and ILoggerSource interfaces |
| C[ATAS.Strategies.Chart.SmaChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1SmaChartStrategy.md) | |
| C[ATAS.Indicators.FilterRenderPen](../classes/classATAS_1_1Indicators_1_1FilterRenderPen.md) | Represents a filter for PenSettings objects with support for property change notifications |
| ►CIDataSeries | |
| ►C[ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | Interface for data series, providing essential properties and methods |
| C[ATAS.Indicators.BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | Base generic data series class providing common functionality |
| ►CIDisposable | |
| ►C[ATAS.DataFeedsCore.Database.IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md) | |
| C[ATAS.DataFeedsCore.Database.DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md) | |
| C[ATAS.DataFeedsCore.TradeStatistics.Matching.TradesProcessingUnit](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md) | |
| ►C[ATAS.Indicators.BaseIndicator](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md) | Base class for custom indicators in a chart |
| C[ATAS.Indicators.ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md) | An extended base class for custom indicators that provide additional functionality for drawing, alerts, market data handling, etc |
| C[ATAS.Indicators.Heatmap.IHeatmapVisualStateLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md) | The exclusive write lease on an IHeatmapVisualState. Acquired via IHeatmapVisualState.BeginUpdate; disposing commits the back-stage to the front. A lease can only be used inside the calling stack frame — passing it across `await` points or to background tasks is a misuse pattern (the platform serialises every indicator callback on a single task, so all legitimate writes happen from inside one of those callbacks) |
| ►C[ATAS.DataFeedsCore.Dom.IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md) | |
| C[ATAS.DataFeedsCore.Dom.DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md) | Maintains Depth of Market state for the security |
| C[ATAS.DataFeedsCore.Dom.GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md) | |
| C[ATAS.DataFeedsCore.Dom.LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md) | |
| C[ATAS.DataFeedsCore.Dom.ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md) | |
| C[ATAS.Indicators.IDrawingObjectsListInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IDrawingObjectsListInfo.md) | Interface for providing information about the list of drawing objects on the chart |
| ►C[ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | Represents an entity in the application |
| C[ATAS.DataFeedsCore.Commissions.CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | |
| C[ATAS.DataFeedsCore.Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | |
| C[ATAS.DataFeedsCore.MarketByOrder](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md) | Market by Order (MBO) describes an order-based data feed that provides the ability to view individual queue position, full depth of book and the size of individual orders at each price level |
| C[ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | Represents a market depth entry |
| C[ATAS.DataFeedsCore.MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | Represents a trade entity in the system |
| C[ATAS.DataFeedsCore.News](../classes/classATAS_1_1DataFeedsCore_1_1News.md) | |
| C[ATAS.DataFeedsCore.Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | Represents an order for trading on a financial exchange |
| C[ATAS.DataFeedsCore.Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | Represents a portfolio entity with various properties related to account balance, Profit and Loss (PnL), permissions, trading options, and more |
| C[ATAS.DataFeedsCore.PortfolioChange](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) | |
| C[ATAS.DataFeedsCore.PortfolioState](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioState.md) | |
| C[ATAS.DataFeedsCore.Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | Represents a trading position |
| C[ATAS.DataFeedsCore.PositionState](../classes/classATAS_1_1DataFeedsCore_1_1PositionState.md) | |
| C[ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | Represents a security entity used in the application |
| C[ATAS.DataFeedsCore.SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | |
| C[ATAS.DataFeedsCore.Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md) | Represents an tick on a financial exchange |
| C[ATAS.DataFeedsCore.TradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1TradingOptions.md) | |
| C[ATAS.DataFeedsCore.User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) | |
| C[ATAS.DataFeedsCore.UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | |
| C[ATAS.DataFeedsCore.UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) | |
| C[ATAS.DataFeedsCore.WorkingTime](../classes/classATAS_1_1DataFeedsCore_1_1WorkingTime.md) | |
| C[ATAS.Indicators.MarketDataArg](../classes/classATAS_1_1Indicators_1_1MarketDataArg.md) | Represents a data point in the market |
| ►C[ATAS.DataFeedsCore.IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) | |
| ►C[ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md) | |
| C[ATAS.DataFeedsCore.Database.Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | |
| ►CIFormattable | |
| C[ATAS.DataFeedsCore.Money](../structs/structATAS_1_1DataFeedsCore_1_1Money.md) | Represents decimal amount in some currency |
| C[ATAS.Indicators.Heatmap.IHeatmapDisposableIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md) | Optional capability: receive a deterministic disposal signal. The supervisor invokes DisposeAsync on the instance's own consumer task — serialised against any other in-flight call, observing the per-call timeout — when the instance is removed via `HeatmapIndicatorsController.RemoveInstanceAsync` or when the controller itself is disposed |
| C[ATAS.Indicators.Heatmap.IHeatmapHistoricalDataLoadedConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md) | Optional capability: receive a notification when the host's working data range expanded backward (e.g. user panned into history that triggered a load). Implement only if the indicator needs to rebuild or refill calculation state across newly-loaded historical samples. Typical reaction: take a lease, `Clear()`, refill from new historical range, dispose lease — the front stays visible across the rebuild |
| ►C[ATAS.Indicators.Heatmap.IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md) | Non-generic runtime contract used by the platform, catalogue, and controller. Indicator authors should derive from HeatmapIndicator instead of implementing this interface directly |
| C[ATAS.Indicators.Heatmap.HeatmapIndicator](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md) | Author-facing entry points for the heatmap indicator API. The non-generic `HeatmapIndicator` coexists with the generic HeatmapIndicator base class — different arities disambiguate them at the type system level |
| C[ATAS.Indicators.Heatmap.IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) | Live runtime context exposed to a heatmap indicator. Unlike the v1 `HeatmapIndicatorContext` record (snapshot at reset), this is a read-only interface whose properties reflect the host's current state at every read. Indicators query it on demand — there are no "context changed" events because the most volatile field (Viewport) updates every render frame and a cross-thread notification per change would dominate the cost |
| C[ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderInstance](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md) | Read-only renderer view of one live indicator instance owned by the heatmap controller. The renderer (Skia / Vulkan overlay layer) consumes this surface to walk every visible instance and read its State per frame; the controller's richer `IHeatmapIndicatorInstance` is host / view-model facing and lives in a higher-level project the renderer cannot reference. Splitting the renderer-only surface here keeps the dependency direction acyclic (renderer -> indicators -> rendering primitives) |
| C[ATAS.Indicators.Heatmap.IHeatmapIndicatorRenderSource](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md) | Renderer-facing handle on the live indicator set. Implemented by the platform's heatmap controller (which already owns instance lifecycle and the live IHeatmapIndicatorContext) and consumed by the renderer overlay. Replaces the v1 `HeatmapIndicatorsSnapshot` pull model: the renderer enumerates Instances directly and pulls per-instance state via IHeatmapIndicatorRenderInstance.State |
| C[ATAS.Indicators.Heatmap.IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) | Runtime handle the platform passes to indicators at warm-up / reset time. Lets the indicator drive its own re-warm or full state reset from any of its async methods. The handle is rebound on every reset, so indicators MUST NOT retain a runtime reference across IHeatmapIndicator.OnStateResetNotificationAsync calls |
| C[ATAS.Indicators.Heatmap.IHeatmapPlatformResettableVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md) | Platform-owned extension for clearing an indicator state without going through an author-visible update lease. Indicator authors should use IHeatmapVisualState.BeginUpdate instead |
| C[ATAS.Indicators.Heatmap.IHeatmapProfileConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapProfileConsumer.md) | |
| C[ATAS.Indicators.Heatmap.IHeatmapSeriesLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) | Per-series mutation surface inside the lease. Append, replace, clear, and trim operations are buffered to the back-stage and become visible to the renderer on lease disposal |
| C[ATAS.Indicators.Heatmap.IHeatmapSeriesStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md) | Read-only handle to a series within an IHeatmapVisualStateNode. The renderer iterates committed samples through this interface; mutation goes through the lease (IHeatmapVisualLease.Series) |
| C[ATAS.Indicators.Heatmap.IHeatmapTimerConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md) | Optional capability: receive periodic timer ticks. Most indicators do not need this; only opt in if the indicator must do work on a wall clock rather than in response to incoming data — for example, to expire stale state at session boundaries when no trade tick has arrived to wake the indicator up |
| C[ATAS.Indicators.Heatmap.IHeatmapTradeTickConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTradeTickConsumer.md) | |
| C[ATAS.Indicators.Heatmap.IHeatmapVisualLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) | Per-visual mutation surface inside the lease. Style and presentation are mutable properties on the lease; series content is mutated through Series |
| C[ATAS.Indicators.Heatmap.IHeatmapVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) | Persistent visual state owned by a single indicator instance and read by the renderer at frame rate. Created by the platform from the indicator's HeatmapIndicatorDescriptor and bound to the indicator via HeatmapIndicator.State; the same reference is valid for the entire lifetime of the runner. Resets (instrument switch, explicit `RequestStateResetAsync`) clear the content of every series but do NOT replace the state instance |
| C[ATAS.Indicators.Heatmap.IHeatmapVisualStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md) | Read-only handle to a visual within an IHeatmapVisualState. Carries the descriptor metadata plus the list of series; mutation goes through the lease (IHeatmapVisualStateLease.Visual) |
| C[ATAS.Indicators.Heatmap.IHeatmapWarmupIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapWarmupIndicator.md) | |
| ►C[ATAS.Indicators.IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md) | Represents a data provider for an indicator, providing access to various data and services related to the indicator |
| C[ATAS.Indicators.IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md) | Implementation of the IIndicatorDataProvider interface that provides access to various data and services related to an indicator |
| C[ATAS.Indicators.IIndicatorServiceProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorServiceProvider.md) | |
| ►C[ATAS.Indicators.IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) | Interface representing instrument information |
| C[ATAS.Indicators.InstrumentInfo](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md) | Implementation of the IInstrumentInfo interface representing instrument information |
| C[ATAS.Indicators.IIntCandle](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md) | Represents an interface for an integer-based candle |
| C[ATAS.Indicators.IKeyboardInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IKeyboardInfo.md) | Interface for providing information about the keyboard state |
| ►C[ATAS.Indicators.IKnowFixedProfiles](../interfaces/interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md) | Represents an interface for objects that know fixed profiles and can request them |
| C[ATAS.Indicators.IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | Interface for an online data provider that provides access to real-time market data |
| ►C[ATAS.DataFeedsCore.IKnowSession](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md) | |
| C[ATAS.DataFeedsCore.Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | |
| ►CILoggerSource | |
| C[ATAS.DataFeedsCore.Database.ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md) | |
| ►C[ATAS.DataFeedsCore.IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| C[ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md) | |
| ►C[ATAS.DataFeedsCore.ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md) | |
| C[ATAS.DataFeedsCore.CryptoAsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md) | |
| C[ATAS.DataFeedsCore.CryptoBaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoBaseConnector.md) | |
| C[ATAS.DataFeedsCore.ILiquidationFeed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ILiquidationFeed.md) | Provides liquidation orders stream for securities |
| C[ATAS.DataFeedsCore.IReferralAccountConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IReferralAccountConnector.md) | |
| ►C[ATAS.DataFeedsCore.IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) | |
| C[ATAS.DataFeedsCore.AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md) | |
| C[ATAS.DataFeedsCore.BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| C[ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md) | Represents an abstract class for a chart strategy that extends the functionality of an Indicator and implements the IChartStrategy and ILoggerSource interfaces |
| ►C[ATAS.Indicators.IMarketByOrdersDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md) | Interface for manager that provides access to market by order data |
| C[ATAS.Indicators.IMarketByOrdersCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) | Interface for manager that provides access to market by orders cache |
| C[ATAS.Indicators.IMarketByOrdersWithTradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) | Interface for manager that provides access to market by orders and trades cache |
| C[ATAS.Indicators.IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | Interface for an online data provider that provides access to real-time market data |
| ►C[ATAS.DataFeedsCore.IMarketByOrdersManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md) | Interface for manager that provides access to market by order data |
| C[ATAS.DataFeedsCore.MarketByOrdersManager](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md) | Manager that provides access to market by order data |
| ►C[ATAS.DataFeedsCore.IMarketDataPublisher](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDataPublisher.md) | |
| C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| C[ATAS.DataFeedsCore.BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md) | |
| ►C[ATAS.DataFeedsCore.IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) | Represents a market depth entry |
| C[ATAS.DataFeedsCore.MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | Represents a market depth entry |
| ►C[ATAS.Indicators.IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md) | Interface for providing market depth information |
| C[ATAS.Indicators.MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) | A class that implements the IMarketDepthInfoProvider interface to provide market depth information |
| ►C[ATAS.Indicators.IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md) | |
| C[ATAS.Indicators.ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md) | An extended base class for custom indicators that provide additional functionality for drawing, alerts, market data handling, etc |
| C[ATAS.Indicators.IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | Interface for an online data provider that provides access to real-time market data |
| C[ATAS.Indicators.IMouseLocationInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md) | Interface for providing information about the mouse location within the chart |
| ►CINotifyCompletion | |
| C[ATAS.DataFeedsCore.AsyncConnector.SynchronizationContextAwaiter](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md) | Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` |
| C[ATAS.DataFeedsCore.AsyncConnector.SynchronizationContextQueueAwaiter](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md) | Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` This awaiter always passes the continuation to the connector's queue |
| ►C[ATAS.Indicators.INotifyPanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md) | Notifies clients that a panel property value has changed |
| C[ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | |
| C[ATAS.Indicators.BaseIndicator](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md) | Base class for custom indicators in a chart |
| C[ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | Interface for data series, providing essential properties and methods |
| ►CINotifyPropertyChanged | |
| C[ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | |
| C[ATAS.DataFeedsCore.Commissions.CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | |
| C[ATAS.DataFeedsCore.Commissions.CommissionGroupItem](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroupItem.md) | |
| C[ATAS.DataFeedsCore.Commissions.CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md) | |
| C[ATAS.DataFeedsCore.Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | |
| C[ATAS.DataFeedsCore.HistoryMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | Represents a historical trade record |
| C[ATAS.DataFeedsCore.InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | |
| C[ATAS.DataFeedsCore.MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | Represents a trade entity in the system |
| C[ATAS.DataFeedsCore.News](../classes/classATAS_1_1DataFeedsCore_1_1News.md) | |
| C[ATAS.DataFeedsCore.Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | Represents an order for trading on a financial exchange |
| C[ATAS.DataFeedsCore.Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | Represents a portfolio entity with various properties related to account balance, Profit and Loss (PnL), permissions, trading options, and more |
| C[ATAS.DataFeedsCore.Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | Represents a trading position |
| C[ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | Represents a security entity used in the application |
| C[ATAS.DataFeedsCore.SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | |
| C[ATAS.DataFeedsCore.SecurityRoute](../classes/classATAS_1_1DataFeedsCore_1_1SecurityRoute.md) | |
| ►C[ATAS.DataFeedsCore.Statistics.IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md) | |
| ►C[ATAS.DataFeedsCore.Statistics.StatisticsParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AccountAgeParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AccountAgeParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AvgLossInMoneyParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossInMoneyParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AvgLossParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AvgPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgPnLParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AvgProfitParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgProfitParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AvgTradeLengthParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradeLengthParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AvgTradesPerDayParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradesPerDayParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.AvgWinParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgWinParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.BestTradeParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1BestTradeParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.CommissionParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1CommissionParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.DailyPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1DailyPnLParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.LastTradeDateParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LastTradeDateParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.LossDaysParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossDaysParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.LossPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossPnLParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.LossTradesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossTradesParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.MaxConsecutiveLossesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveLossesParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.MaxConsecutiveWinsParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveWinsParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.MaxDrawdownDateParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownDateParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.MaxDrawdownParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.MaxRelativeDrawdownParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxRelativeDrawdownParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.NetPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1NetPnLParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.ProfitDaysParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitDaysParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.ProfitFactorParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitFactorParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.ProfitPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitPnLParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.ProfitTradesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitTradesParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.RecoveryFactorParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1RecoveryFactorParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.SharpeRatioParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1SharpeRatioParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.TotalDaysParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalDaysParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.TotalLossParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalLossParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.TotalPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalPnLParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.TotalProfitParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalProfitParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.TotalTradesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalTradesParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.WinLossRatioParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinLossRatioParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.WinRateParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinRateParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.WinningDaysPercentParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinningDaysPercentParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.WorstTradeParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WorstTradeParameter.md) | |
| C[ATAS.DataFeedsCore.Statistics.StatisticsManager](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md) | |
| C[ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md) | |
| C[ATAS.DataFeedsCore.TradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1TradingOptions.md) | |
| C[ATAS.DataFeedsCore.User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) | |
| C[ATAS.DataFeedsCore.UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | |
| C[ATAS.DataFeedsCore.UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) | |
| C[ATAS.DataFeedsCore.WorkingTime](../classes/classATAS_1_1DataFeedsCore_1_1WorkingTime.md) | |
| C[ATAS.Indicators.BaseIndicator](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md) | Base class for custom indicators in a chart |
| ►C[ATAS.Indicators.Filters.TrackedPropertyBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md) | A base class for tracking property changes and notifying subscribers when a property value is modified |
| ►C[ATAS.Indicators.ChartObject](../classes/classATAS_1_1Indicators_1_1ChartObject.md) | Base class for objects in a chart |
| C[ATAS.Indicators.BaseIndicator](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md) | Base class for custom indicators in a chart |
| C[ATAS.Indicators.IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | Interface for data series, providing essential properties and methods |
| C[ATAS.Indicators.IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md) | Represents a filter with common properties and functionality |
| ►C[ATAS.Indicators.NotifyPropertyChangedBase](../classes/classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | Base class for implementing the INotifyPropertyChanged interface |
| C[ATAS.Indicators.FilterBase](../classes/classATAS_1_1Indicators_1_1FilterBase.md) | Base class for filters implementing the IFilter interface |
| C[ATAS.Indicators.FilterRangeValue](../classes/classATAS_1_1Indicators_1_1FilterRangeValue.md) | Represents a range of values of type TValue with support for property change notifications |
| ►C[ATAS.Strategies.IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | Represents a trading strategy |
| ►C[ATAS.Strategies.ATM.IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| C[ATAS.Strategies.ATM.ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| ►C[ATAS.Strategies.ATM.IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| C[ATAS.Strategies.ATM.BaseStopProfitStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| ►C[ATAS.Strategies.ATM.ISupportCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md) | |
| ►C[ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md) | |
| C[ATAS.Strategies.ATM.StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) | |
| ►C[ATAS.Strategies.Chart.IChartStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md) | Represents a chart strategy that extends the basic functionality of an IStrategy with additional chart-related features |
| C[ATAS.Strategies.Chart.ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md) | Represents an abstract class for a chart strategy that extends the functionality of an Indicator and implements the IChartStrategy and ILoggerSource interfaces |
| C[ATAS.Strategies.Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md) | Base class for implementing trading strategies |
| C[OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md) | |
| ►CInvalidOperationException | |
| C[ATAS.Indicators.Heatmap.HeatmapLeaseMisuseException](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md) | Thrown when an indicator misuses the visual-state lease API. Distinct from a plain InvalidOperationException so call sites can catch lease misuse separately from generic invalid-operation errors, and tests can assert on Reason instead of message text |
| C[ATAS.DataFeedsCore.IOptionsDataFeed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOptionsDataFeed.md) | |
| C[ATAS.DataFeedsCore.IOrderOptionCloseOnTrigger](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionCloseOnTrigger.md) | Represents an order option for closing a position when triggered |
| C[ATAS.DataFeedsCore.IOrderOptionPostOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.md) | Represents an order option for placing a post-only order |
| C[ATAS.DataFeedsCore.IOrderOptionReduceOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionReduceOnly.md) | Represents an order option for reducing the position size |
| C[ATAS.DataFeedsCore.IPasswordChanger](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPasswordChanger.md) | |
| C[ATAS.Indicators.IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) | Interface for accessing platform settings |
| ►C[ATAS.DataFeedsCore.IPortfolioExtendedInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md) | Base interface for extended portfolio information. Each connector can have its own implementation with unique parameters |
| C[ATAS.DataFeedsCore.PortfolioExtendedInfoBase](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.md) | Base implementation of IPortfolioExtendedInfo with common functionality |
| ►C[ATAS.DataFeedsCore.IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md) | Exposes the subset of trading-entity properties required to format prices and derive instrument classification (US bonds, MOEX futures). Implemented by Security directly and by `OFT.Platform.Models.Instrument` via explicit interface implementation |
| C[ATAS.DataFeedsCore.Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | Represents a security entity used in the application |
| C[ATAS.Indicators.IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md) | |
| ►C[ATAS.Indicators.IPropertiesEditorOwner](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md) | |
| C[ATAS.Indicators.BaseIndicator](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md) | Base class for custom indicators in a chart |
| C[ATAS.DataFeedsCore.Rebate.IRebateInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateInfo.md) | |
| C[ATAS.DataFeedsCore.Rebate.IRebateProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateProvider.md) | Allows to obtain user affiliation status needed to calculate correct commission rebates on crypto exchanges |
| C[ATAS.DataFeedsCore.ConnectorWebsocket.IRequestSerializer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md) | |
| ►C[ATAS.DataFeedsCore.ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md) | |
| C[ATAS.DataFeedsCore.SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md) | |
| ►C[ATAS.DataFeedsCore.ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md) | Represents an interface for providing trading options related to a security or trading connector |
| C[ATAS.DataFeedsCore.SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md) | Each connector may have different order placement settings like TimeInForce or ReduceOnly etc. This class shows connector configuration |
| ►C[ATAS.DataFeedsCore.SessionServer.ISessionServer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1SessionServer_1_1ISessionServer.md) | |
| C[ATAS.DataFeedsCore.SessionServer.SessionServer](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md) | |
| ►C[ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md) | |
| C[ATAS.DataFeedsCore.Statistics.StatisticsParameterGroup](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md) | |
| C[ATAS.Strategies.ATM.IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | |
| ►C[ATAS.Strategies.ATM.IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| C[ATAS.Strategies.ATM.ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md) | |
| C[ATAS.Strategies.ATM.IStrategyMarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) | |
| ►C[ATAS.DataFeedsCore.ISupportChangePosition](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportChangePosition.md) | |
| C[ATAS.DataFeedsCore.BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| ►C[ATAS.Indicators.ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md) | Represents an interface for supporting price information |
| C[ATAS.Indicators.IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) | Represents an Indicator Candle |
| ►C[ATAS.DataFeedsCore.ISupportMarketDataOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportMarketDataOnly.md) | |
| C[ATAS.DataFeedsCore.BaseMarketDataOnlyConnectorSettings](../classes/classATAS_1_1DataFeedsCore_1_1BaseMarketDataOnlyConnectorSettings.md) | |
| C[ATAS.DataFeedsCore.ISupportResetPortfolio](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportResetPortfolio.md) | |
| C[ATAS.DataFeedsCore.ISupportTPlusLimits](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportTPlusLimits.md) | |
| C[ATAS.Indicators.ITimeMarketDataCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md) | Cache that holds recent items based on a specified amount of time |
| ►C[ATAS.Indicators.ITimeMarketDataCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md) | |
| C[ATAS.Indicators.IMarketByOrdersWithTradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) | Interface for manager that provides access to market by orders and trades cache |
| ►C[ATAS.Indicators.ITimeMarketDataCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md) | |
| C[ATAS.Indicators.IMarketByOrdersCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) | Interface for manager that provides access to market by orders cache |
| ►C[ATAS.Indicators.ITimeMarketDataCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md) | |
| C[ATAS.Indicators.ITradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITradesCache.md) | Interface for manager that provides access to trades cache |
| C[ATAS.Indicators.ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) | Interface representing a trading manager for handling trading-related operations |
| ►C[ATAS.DataFeedsCore.Statistics.ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) | |
| C[ATAS.DataFeedsCore.Statistics.TradingStatistics](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md) | |
| C[ATAS.DataFeedsCore.Statistics.ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) | |
| C[ATAS.Indicators.ITradingVolumeInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md) | Interface for providing information about the volume selector |
| C[ATAS.Indicators.ITransientIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1ITransientIndicator.md) | Marker interface for indicators that are managed programmatically and should not be persisted in the chart template. Transient indicators are excluded from serialization and are added/removed at runtime by platform components (e.g. ChartTraderViewModel) |
| C[ATAS.Indicators.IVolumeSelectorItem](../interfaces/interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md) | Interface representing the volume template |
| ►CJsonConverter | |
| C[ATAS.Indicators.Filters.Converters.FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[ATAS.Indicators.MarketDepthSnapshot](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshot.md) | Represents the end state of market depth over a specified time period |
| C[ATAS.Indicators.MarketDepthSnapshotRequest](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md) | Represents a request to retrieve a snapshot of the market depth for a specified time range |
| C[ATAS.DataFeedsCore.Statistics.MyTradeQueue](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MyTradeQueue.md) | |
| C[ATAS.DataFeedsCore.OptionSeries](../structs/structATAS_1_1DataFeedsCore_1_1OptionSeries.md) | |
| C[ATAS.DataFeedsCore.OvernightSwapValue](../classes/classATAS_1_1DataFeedsCore_1_1OvernightSwapValue.md) | |
| ►CUtils.Common.Attributes.ParameterAttribute | |
| ►C[OFT.Attributes.ParameterAttribute](../classes/classOFT_1_1Attributes_1_1ParameterAttribute.md) | |
| C[ATAS.Indicators.ParameterAttribute](../classes/classATAS_1_1Indicators_1_1ParameterAttribute.md) | |
| C[ATAS.DataFeedsCore.Playbook](../classes/classATAS_1_1DataFeedsCore_1_1Playbook.md) | |
| C[ATAS.DataFeedsCore.PnLTradesQueue](../classes/classATAS_1_1DataFeedsCore_1_1PnLTradesQueue.md) | |
| C[ATAS.DataFeedsCore.PortfolioGroup](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioGroup.md) | Represents a portfolio group used in the application |
| C[ATAS.DataFeedsCore.PortfolioViewer](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) | Represents a portfolio viewer used in the application |
| C[ATAS.DataFeedsCore.PositionTradesQueue](../classes/classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md) | |
| C[ATAS.Indicators.PriceSelectionValue](../classes/classATAS_1_1Indicators_1_1PriceSelectionValue.md) | Represents a class for defining price level selection in clusters and bars. Using in PriceSelectionDataSeries |
| C[ATAS.DataFeedsCore.PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | |
| C[ATAS.Indicators.PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | Represents information on volumes at a specific price |
| C[ATAS.Strategies.Editors.ProtectionSettingsEditor](../classes/classATAS_1_1Strategies_1_1Editors_1_1ProtectionSettingsEditor.md) | |
| C[ATAS.Indicators.RangeValue](../classes/classATAS_1_1Indicators_1_1RangeValue.md) | RangeDataSeries element |
| C[ATAS.Indicators.RedrawArg](../classes/classATAS_1_1Indicators_1_1RedrawArg.md) | Represents the arguments for requesting a redraw of a chart |
| C[ReferralData](../classes/classReferralData.md) | |
| C[ATAS.DataFeedsCore.SecurityFilter](../classes/classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) | |
| C[ATAS.DataFeedsCore.SecurityRouteCache](../classes/classATAS_1_1DataFeedsCore_1_1SecurityRouteCache.md) | |
| C[ATAS.DataFeedsCore.SecuritySummary](../classes/classATAS_1_1DataFeedsCore_1_1SecuritySummary.md) | |
| C[ATAS.DataFeedsCore.ServerPnL](../classes/classATAS_1_1DataFeedsCore_1_1ServerPnL.md) | |
| C[ATAS.DataFeedsCore.Database.SettingsItem](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1SettingsItem.md) | |
| C[ATAS.DataFeedsCore.SimpleMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1SimpleMyTrade.md) | |
| C[ATAS.DataFeedsCore.SubscriptionCounter](../classes/classATAS_1_1DataFeedsCore_1_1SubscriptionCounter.md) | |
| C[ATAS.DataFeedsCore.SubscriptionCounter](../classes/classATAS_1_1DataFeedsCore_1_1SubscriptionCounter.md) | |
| C[ATAS.DataFeedsCore.SubscriptionManager](../classes/classATAS_1_1DataFeedsCore_1_1SubscriptionManager.md) | |
| ►CSynchronizationContext | |
| C[ATAS.DataFeedsCore.AsyncConnector.ConnectorSynchronizationContext](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md) | Custom synchronization context to forward await continuations to the connector queue |
| C[ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md) | |
| C[ATAS.DataFeedsCore.TradesTimeChecker](../classes/classATAS_1_1DataFeedsCore_1_1TradesTimeChecker.md) | |
| C[ATAS.DataFeedsCore.TradingOptionsSecurity](../classes/classATAS_1_1DataFeedsCore_1_1TradingOptionsSecurity.md) | |
| ►C[ATAS.Indicators.Drawing.TrendLine](../classes/classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md) | Represents a trend line on a chart |
| C[ATAS.Indicators.Drawing.LineTillTouch](../classes/classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md) | Represents a trend line that extends until it is touched by the price |
| C[ATAS.DataFeedsCore.UniversalMarketData](../classes/classATAS_1_1DataFeedsCore_1_1UniversalMarketData.md) | |
| ►C[ATAS.DataFeedsCore.UserChange](../classes/classATAS_1_1DataFeedsCore_1_1UserChange.md) | |
| C[ATAS.DataFeedsCore.UserChangeHistory](../classes/classATAS_1_1DataFeedsCore_1_1UserChangeHistory.md) | |
| C[ATAS.DataFeedsCore.UserRoleRight](../classes/classATAS_1_1DataFeedsCore_1_1UserRoleRight.md) | |
| C[ATAS.Indicators.ValueArea](../classes/classATAS_1_1Indicators_1_1ValueArea.md) | Represents information on Value area high/low |
| C[ATAS.Indicators.ValueChangingEventArgs](../classes/classATAS_1_1Indicators_1_1ValueChangingEventArgs.md) | Provides event arguments for a value changing event |
| C[ATAS.DataFeedsCore.VolumeUnit](../structs/structATAS_1_1DataFeedsCore_1_1VolumeUnit.md) | |
