# Class List

Source: https://docs.atas.net/en/annotated.html

Here are the classes, structs, unions and interfaces with brief descriptions:

[detail level 12345]

| ▼N[ATAS](../namespaces/namespaceATAS.md) | |
| --- | --- |
| ►N[DataFeedsCore](../namespaces/namespaceATAS_1_1DataFeedsCore.md) | |
| ►N[Commissions](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1Commissions.md) | |
| C[CommissionGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | |
| C[CommissionGroupItem](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroupItem.md) | |
| C[CommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRule.md) | |
| C[CommissionRulesGroup](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRulesGroup.md) | |
| C[CommissionRuleType](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRuleType.md) | |
| C[ICommissionRule](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Commissions_1_1ICommissionRule.md) | |
| C[PercentCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PercentCommissionRule.md) | |
| C[PerContractCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerContractCommissionRule.md) | |
| C[PerTradeCommissionRule](../classes/classATAS_1_1DataFeedsCore_1_1Commissions_1_1PerTradeCommissionRule.md) | |
| ►N[ConnectorWebsocket](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket.md) | |
| C[ConnectorWebsocket](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1ConnectorWebsocket.md) | |
| C[IRequestSerializer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1IRequestSerializer.md) | |
| C[TaskQueue](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md) | |
| C[WebsocketException](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1WebsocketException.md) | |
| ►N[Database](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1Database.md) | |
| C[Cache](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1Cache.md) | |
| C[DatabaseException](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseException.md) | |
| C[DatabaseManager](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1DatabaseManager.md) | |
| C[ICache](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1ICache.md) | |
| C[IDatabaseManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Database_1_1IDatabaseManager.md) | |
| C[SettingsItem](../classes/classATAS_1_1DataFeedsCore_1_1Database_1_1SettingsItem.md) | |
| ►N[Dom](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1Dom.md) | |
| ►C[DomBuilder](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder.md) | Builds and maintains a DOM for a connector
 Allows to obtain best prices if connector does not give them |
| C[DomChangesTracker](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomBuilder_1_1DomChangesTracker.md) | |
| C[DomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1DomManager.md) | Maintains Depth of Market state for the security |
| C[GroupDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1GroupDomManager.md) | |
| C[IDomManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Dom_1_1IDomManager.md) | |
| C[LimitDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1LimitDomManager.md) | |
| C[PriceComparer](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1PriceComparer.md) | |
| C[ScaleDomManager](../classes/classATAS_1_1DataFeedsCore_1_1Dom_1_1ScaleDomManager.md) | |
| ►N[Exceptions](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1Exceptions.md) | |
| C[ConnectorNotConnectedException](../classes/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ConnectorNotConnectedException.md) | |
| C[ExchangeException](../classes/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.md) | An exception means the Exchange sent us an logic error like violating business rules It allows us to separate network errors and logic errors |
| C[FatalConnectionErrorEcxeption](../classes/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1FatalConnectionErrorEcxeption.md) | |
| ►N[Rebate](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1Rebate.md) | |
| C[IRebateInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateInfo.md) | |
| C[IRebateProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Rebate_1_1IRebateProvider.md) | Allows to obtain user affiliation status needed to calculate correct commission rebates on crypto exchanges |
| C[WhiteListHttpClient](../classes/classATAS_1_1DataFeedsCore_1_1Rebate_1_1WhiteListHttpClient.md) | |
| ►N[SessionServer](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md) | |
| C[ISessionServer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1SessionServer_1_1ISessionServer.md) | |
| C[SessionInfo](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md) | |
| C[SessionServer](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md) | |
| C[SocketSession](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md) | |
| ►N[Statistics](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1Statistics.md) | |
| C[AccountAgeParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AccountAgeParameter.md) | |
| C[AvgLossInMoneyParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossInMoneyParameter.md) | |
| C[AvgLossParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgLossParameter.md) | |
| C[AvgPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgPnLParameter.md) | |
| C[AvgProfitParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgProfitParameter.md) | |
| C[AvgTradeLengthParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradeLengthParameter.md) | |
| C[AvgTradesPerDayParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgTradesPerDayParameter.md) | |
| C[AvgWinParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1AvgWinParameter.md) | |
| C[BestTradeParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1BestTradeParameter.md) | |
| C[CommissionParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1CommissionParameter.md) | |
| C[DailyPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1DailyPnLParameter.md) | |
| C[IStatisticsParameter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameter.md) | |
| C[IStatisticsParameterGroup](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md) | |
| C[ITradingStatistics](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) | |
| C[ITradingStatisticsProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md) | |
| C[LastTradeDateParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LastTradeDateParameter.md) | |
| C[LossDaysParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossDaysParameter.md) | |
| C[LossPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossPnLParameter.md) | |
| C[LossTradesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1LossTradesParameter.md) | |
| C[MaxConsecutiveLossesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveLossesParameter.md) | |
| C[MaxConsecutiveWinsParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxConsecutiveWinsParameter.md) | |
| C[MaxDrawdownDateParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownDateParameter.md) | |
| C[MaxDrawdownParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxDrawdownParameter.md) | |
| C[MaxRelativeDrawdownParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MaxRelativeDrawdownParameter.md) | |
| C[MyTradeQueue](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1MyTradeQueue.md) | |
| C[NetPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1NetPnLParameter.md) | |
| C[ProfitDaysParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitDaysParameter.md) | |
| C[ProfitFactorParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitFactorParameter.md) | |
| C[ProfitPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitPnLParameter.md) | |
| C[ProfitTradesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1ProfitTradesParameter.md) | |
| C[RecoveryFactorParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1RecoveryFactorParameter.md) | |
| C[SharpeRatioParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1SharpeRatioParameter.md) | |
| C[StatisticsManager](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsManager.md) | |
| C[StatisticsParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameter.md) | |
| C[StatisticsParameterGroup](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1StatisticsParameterGroup.md) | |
| C[TotalDaysParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalDaysParameter.md) | |
| C[TotalLossParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalLossParameter.md) | |
| C[TotalPnLParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalPnLParameter.md) | |
| C[TotalProfitParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalProfitParameter.md) | |
| C[TotalTradesParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TotalTradesParameter.md) | |
| C[TradingStatistics](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1TradingStatistics.md) | |
| C[WinLossRatioParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinLossRatioParameter.md) | |
| C[WinningDaysPercentParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinningDaysPercentParameter.md) | |
| C[WinRateParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WinRateParameter.md) | |
| C[WorstTradeParameter](../classes/classATAS_1_1DataFeedsCore_1_1Statistics_1_1WorstTradeParameter.md) | |
| ►N[TradeStatistics](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics.md) | |
| ►N[Matching](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching.md) | |
| C[TradesMatchingProcessor](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesMatchingProcessor.md) | |
| C[TradesProcessingLoggerSource](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingLoggerSource.md) | |
| C[TradesProcessingUnit](../classes/classATAS_1_1DataFeedsCore_1_1TradeStatistics_1_1Matching_1_1TradesProcessingUnit.md) | |
| ►C[AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) | Connector base that allows to use `await` to switch to the connector queue thread |
| C[AsyncAwaiterFacade](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterFacade.md) | Facade for `SynchronizationContextAwaiter` to allow explicit call of `SwitchToConnectorThreadAsync()` |
| C[AsyncAwaiterQueueFacade](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1AsyncAwaiterQueueFacade.md) | Facade for `SynchronizationContextQueueAwaiter` to allow explicit call of `ForceToConnectorThreadAsync()` |
| C[ConnectorSynchronizationContext](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1ConnectorSynchronizationContext.md) | Custom synchronization context to forward await continuations to the connector queue |
| C[SynchronizationContextAwaiter](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextAwaiter.md) | Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` |
| C[SynchronizationContextQueueAwaiter](../structs/structATAS_1_1DataFeedsCore_1_1AsyncConnector_1_1SynchronizationContextQueueAwaiter.md) | Custom awaiter to control continuation execution on the `ConnectorSynchronizationContext` This awaiter always passes the continuation to the connector's queue |
| C[AsyncConnectorMessage](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md) | Service message for passing continuations to the connector thread |
| C[AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md) | |
| C[BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) | |
| C[BaseMarketDataOnlyConnectorSettings](../classes/classATAS_1_1DataFeedsCore_1_1BaseMarketDataOnlyConnectorSettings.md) | |
| C[BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) | |
| C[BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md) | |
| C[BasketConnectorSettings](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnectorSettings.md) | |
| C[ConnectionStateEventArgs](../classes/classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.md) | |
| C[ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md) | |
| C[CryptoAsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md) | |
| C[CryptoBaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoBaseConnector.md) | |
| C[DailyNote](../classes/classATAS_1_1DataFeedsCore_1_1DailyNote.md) | |
| C[Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) | |
| C[GroupExchange](../classes/classATAS_1_1DataFeedsCore_1_1GroupExchange.md) | |
| C[HistoryMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) | Represents a historical trade record |
| C[HistoryMyTradePlaybook](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTradePlaybook.md) | |
| C[IConnectorExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) | Defines methods to retrieve exchange information based on security or exchange codes |
| C[IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) | |
| C[ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md) | |
| C[IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | |
| C[IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | Represents an entity in the application |
| C[IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) | |
| C[IKnowSession](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md) | |
| C[ILiquidationFeed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ILiquidationFeed.md) | Provides liquidation orders stream for securities |
| C[IMarketByOrdersManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md) | Interface for manager that provides access to market by order data |
| C[IMarketDataPublisher](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDataPublisher.md) | |
| C[IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) | Represents a market depth entry |
| C[IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) | |
| C[InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) | |
| C[IOptionsDataFeed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOptionsDataFeed.md) | |
| C[IOrderOptionCloseOnTrigger](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionCloseOnTrigger.md) | Represents an order option for closing a position when triggered |
| C[IOrderOptionPostOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.md) | Represents an order option for placing a post-only order |
| C[IOrderOptionReduceOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionReduceOnly.md) | Represents an order option for reducing the position size |
| C[IPasswordChanger](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPasswordChanger.md) | |
| C[IPortfolioExtendedInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md) | Base interface for extended portfolio information. Each connector can have its own implementation with unique parameters |
| C[IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md) | Exposes the subset of trading-entity properties required to format prices and derive instrument classification (US bonds, MOEX futures). Implemented by Security directly and by `OFT.Platform.Models.Instrument` via explicit interface implementation |
| C[IReferralAccountConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IReferralAccountConnector.md) | |
| C[ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md) | |
| C[ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md) | Represents an interface for providing trading options related to a security or trading connector |
| C[ISupportChangePosition](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportChangePosition.md) | |
| C[ISupportMarketDataOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportMarketDataOnly.md) | |
| C[ISupportResetPortfolio](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportResetPortfolio.md) | |
| C[ISupportTPlusLimits](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportTPlusLimits.md) | |
| C[MarketByOrder](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md) | Market by Order (MBO) describes an order-based data feed that provides the ability to view individual queue position, full depth of book and the size of individual orders at each price level |
| C[MarketByOrdersManager](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md) | Manager that provides access to market by order data |
| C[MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md) | Represents a market depth entry |
| C[MarketDepthComparer](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepthComparer.md) | |
| C[Money](../structs/structATAS_1_1DataFeedsCore_1_1Money.md) | Represents decimal amount in some currency |
| C[MultiConnectorMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1MultiConnectorMessageQueue.md) | |
| C[MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) | Represents a trade entity in the system |
| C[News](../classes/classATAS_1_1DataFeedsCore_1_1News.md) | |
| C[OptionSeries](../structs/structATAS_1_1DataFeedsCore_1_1OptionSeries.md) | |
| C[Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) | Represents an order for trading on a financial exchange |
| C[OvernightSwapValue](../classes/classATAS_1_1DataFeedsCore_1_1OvernightSwapValue.md) | |
| C[Playbook](../classes/classATAS_1_1DataFeedsCore_1_1Playbook.md) | |
| C[PnLTradesQueue](../classes/classATAS_1_1DataFeedsCore_1_1PnLTradesQueue.md) | |
| C[Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) | Represents a portfolio entity with various properties related to account balance, Profit and Loss (PnL), permissions, trading options, and more |
| C[PortfolioChange](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) | |
| C[PortfolioExtendedInfoBase](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.md) | Base implementation of IPortfolioExtendedInfo with common functionality |
| C[PortfolioGroup](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioGroup.md) | Represents a portfolio group used in the application |
| C[PortfolioState](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioState.md) | |
| C[PortfolioViewer](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) | Represents a portfolio viewer used in the application |
| C[Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) | Represents a trading position |
| C[PositionState](../classes/classATAS_1_1DataFeedsCore_1_1PositionState.md) | |
| C[PositionTradesQueue](../classes/classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md) | |
| C[PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) | |
| C[Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) | Represents a security entity used in the application |
| C[SecurityFilter](../classes/classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) | |
| C[SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) | |
| C[SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md) | |
| C[SecurityRoute](../classes/classATAS_1_1DataFeedsCore_1_1SecurityRoute.md) | |
| C[SecurityRouteCache](../classes/classATAS_1_1DataFeedsCore_1_1SecurityRouteCache.md) | |
| C[SecuritySummary](../classes/classATAS_1_1DataFeedsCore_1_1SecuritySummary.md) | |
| C[SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md) | Each connector may have different order placement settings like TimeInForce or ReduceOnly etc. This class shows connector configuration |
| C[ServerPnL](../classes/classATAS_1_1DataFeedsCore_1_1ServerPnL.md) | |
| C[SimpleMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md) | |
| C[SimpleMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1SimpleMyTrade.md) | |
| C[SubscriptionCounter](../classes/classATAS_1_1DataFeedsCore_1_1SubscriptionCounter.md) | |
| C[SubscriptionManager](../classes/classATAS_1_1DataFeedsCore_1_1SubscriptionManager.md) | |
| C[Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md) | Represents an tick on a financial exchange |
| C[TradesTimeChecker](../classes/classATAS_1_1DataFeedsCore_1_1TradesTimeChecker.md) | |
| C[TradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1TradingOptions.md) | |
| C[TradingOptionsSecurity](../classes/classATAS_1_1DataFeedsCore_1_1TradingOptionsSecurity.md) | |
| C[UniversalMarketData](../classes/classATAS_1_1DataFeedsCore_1_1UniversalMarketData.md) | |
| C[User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) | |
| C[UserChange](../classes/classATAS_1_1DataFeedsCore_1_1UserChange.md) | |
| C[UserChangeHistory](../classes/classATAS_1_1DataFeedsCore_1_1UserChangeHistory.md) | |
| C[UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) | |
| C[UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) | |
| C[UserRoleRight](../classes/classATAS_1_1DataFeedsCore_1_1UserRoleRight.md) | |
| C[VolumeUnit](../structs/structATAS_1_1DataFeedsCore_1_1VolumeUnit.md) | |
| C[WorkingTime](../classes/classATAS_1_1DataFeedsCore_1_1WorkingTime.md) | |
| ►N[Indicators](../namespaces/namespaceATAS_1_1Indicators.md) | |
| ►N[Attributies](../namespaces/namespaceATAS_1_1Indicators_1_1Attributies.md) | |
| C[FeatureId](../classes/classATAS_1_1Indicators_1_1Attributies_1_1FeatureId.md) | |
| ►N[Drawing](../namespaces/namespaceATAS_1_1Indicators_1_1Drawing.md) | |
| C[BaseDrawingText](../classes/classATAS_1_1Indicators_1_1Drawing_1_1BaseDrawingText.md) | Represents a base class for drawing text on a chart |
| C[DrawingRectangle](../classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md) | Represents a rectangle drawn on a chart |
| C[DrawingText](../classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) | Represents a class for drawing text on a chart with additional alignment options |
| C[HorizontalChannel](../classes/classATAS_1_1Indicators_1_1Drawing_1_1HorizontalChannel.md) | Represents a horizontal line on a chart |
| C[HorizontalLine](../classes/classATAS_1_1Indicators_1_1Drawing_1_1HorizontalLine.md) | Represents a horizontal line on a chart |
| C[LineTillTouch](../classes/classATAS_1_1Indicators_1_1Drawing_1_1LineTillTouch.md) | Represents a trend line that extends until it is touched by the price |
| C[TrendLine](../classes/classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md) | Represents a trend line on a chart |
| ►N[Filters](../namespaces/namespaceATAS_1_1Indicators_1_1Filters.md) | |
| ►N[Converters](../namespaces/namespaceATAS_1_1Indicators_1_1Filters_1_1Converters.md) | |
| C[FilterBoolJsonConvert](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterBoolJsonConvert.md) | |
| C[FilterColorJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterColorJsonConverter.md) | |
| C[FilterHeatmapTypesJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterHeatmapTypesJsonConverter.md) | |
| C[FilterIntJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterIntJsonConverter.md) | |
| C[FilterJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverter.md) | |
| C[FilterJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterJsonConverterBase.md) | |
| C[FilterKeyJsonConvert](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterKeyJsonConvert.md) | |
| C[FilterRangeIntJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRangeIntJsonConverter.md) | |
| C[FilterRangeJsonConverterBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRangeJsonConverterBase.md) | |
| C[FilterRenderPenJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterRenderPenJsonConverter.md) | |
| C[FilterStringJsonConverter](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterStringJsonConverter.md) | |
| C[FilterTimeSpanJsonConvert](../classes/classATAS_1_1Indicators_1_1Filters_1_1Converters_1_1FilterTimeSpanJsonConvert.md) | |
| C[TrackedPropertyBase](../classes/classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md) | A base class for tracking property changes and notifying subscribers when a property value is modified |
| ►N[Heatmap](../namespaces/namespaceATAS_1_1Indicators_1_1Heatmap.md) | |
| C[HeatmapIndicator](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicator.md) | Author-facing entry points for the heatmap indicator API. The non-generic `HeatmapIndicator` coexists with the generic HeatmapIndicator base class — different arities disambiguate them at the type system level |
| C[HeatmapIndicatorAttribute](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorAttribute.md) | Marks a class as a heatmap indicator type and supplies discovery metadata. Apply to a concrete class that derives from HeatmapIndicator (or implements IHeatmapIndicator directly) |
| C[HeatmapIndicatorDescriptorBuilder](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorDescriptorBuilder.md) | Fluent builder that produces an immutable HeatmapIndicatorDescriptor alongside the typed visual / series handles required by the state builder. Single-shot: each builder yields exactly one descriptor via Done; further mutation throws |
| C[HeatmapIndicatorFallbackReWarmGuard](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorFallbackReWarmGuard.md) | State holder for indicators whose calculation is anchored at the real data start (e.g. CVD `FromDataStart`, VWAP `FromDataStart`) and may receive a fallback-range warm-up before the host knows the real data start. Encapsulates the latch protocol so the indicator does not have to track three flags and an inline check by hand |
| C[HeatmapIndicatorSeriesHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorSeriesHandle.md) | Strongly typed handle for a series within a visual. Returned from HeatmapIndicatorVisualHandle.Series(string, HeatmapIndicatorSeriesRole, HeatmapIndicatorValueKind, System.Func, HeatmapIndicatorVisualStyle?, string?, string?); the constructor is internal so authors cannot fabricate one. TValue is the indicator-internal sample type — what the indicator's calculation produces. The handle also carries the projection that converts the typed sample to a renderer-facing decimal; the lease applies the projection inline on every Append so the chunked storage holds decimal samples ready for the renderer |
| C[HeatmapIndicatorVisualHandle](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapIndicatorVisualHandle.md) | Strongly typed handle for a visual added to a descriptor via HeatmapIndicatorDescriptorBuilder. The handle captures the owning descriptor's identity so the state builder can reject handles from a different descriptor at runtime, and the constructor is internal so authors cannot fabricate handles by hand |
| C[HeatmapLeaseMisuseException](../classes/classATAS_1_1Indicators_1_1Heatmap_1_1HeatmapLeaseMisuseException.md) | Thrown when an indicator misuses the visual-state lease API. Distinct from a plain InvalidOperationException so call sites can catch lease misuse separately from generic invalid-operation errors, and tests can assert on Reason instead of message text |
| C[IHeatmapDisposableIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapDisposableIndicator.md) | Optional capability: receive a deterministic disposal signal. The supervisor invokes DisposeAsync on the instance's own consumer task — serialised against any other in-flight call, observing the per-call timeout — when the instance is removed via `HeatmapIndicatorsController.RemoveInstanceAsync` or when the controller itself is disposed |
| C[IHeatmapHistoricalDataLoadedConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapHistoricalDataLoadedConsumer.md) | Optional capability: receive a notification when the host's working data range expanded backward (e.g. user panned into history that triggered a load). Implement only if the indicator needs to rebuild or refill calculation state across newly-loaded historical samples. Typical reaction: take a lease, `Clear()`, refill from new historical range, dispose lease — the front stays visible across the rebuild |
| C[IHeatmapIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicator.md) | Non-generic runtime contract used by the platform, catalogue, and controller. Indicator authors should derive from HeatmapIndicator instead of implementing this interface directly |
| C[IHeatmapIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorContext.md) | Live runtime context exposed to a heatmap indicator. Unlike the v1 `HeatmapIndicatorContext` record (snapshot at reset), this is a read-only interface whose properties reflect the host's current state at every read. Indicators query it on demand — there are no "context changed" events because the most volatile field (Viewport) updates every render frame and a cross-thread notification per change would dominate the cost |
| C[IHeatmapIndicatorRenderInstance](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderInstance.md) | Read-only renderer view of one live indicator instance owned by the heatmap controller. The renderer (Skia / Vulkan overlay layer) consumes this surface to walk every visible instance and read its State per frame; the controller's richer `IHeatmapIndicatorInstance` is host / view-model facing and lives in a higher-level project the renderer cannot reference. Splitting the renderer-only surface here keeps the dependency direction acyclic (renderer -> indicators -> rendering primitives) |
| C[IHeatmapIndicatorRenderSource](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRenderSource.md) | Renderer-facing handle on the live indicator set. Implemented by the platform's heatmap controller (which already owns instance lifecycle and the live IHeatmapIndicatorContext) and consumed by the renderer overlay. Replaces the v1 `HeatmapIndicatorsSnapshot` pull model: the renderer enumerates Instances directly and pulls per-instance state via IHeatmapIndicatorRenderInstance.State |
| C[IHeatmapIndicatorRuntime](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapIndicatorRuntime.md) | Runtime handle the platform passes to indicators at warm-up / reset time. Lets the indicator drive its own re-warm or full state reset from any of its async methods. The handle is rebound on every reset, so indicators MUST NOT retain a runtime reference across IHeatmapIndicator.OnStateResetNotificationAsync calls |
| C[IHeatmapPlatformResettableVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapPlatformResettableVisualState.md) | Platform-owned extension for clearing an indicator state without going through an author-visible update lease. Indicator authors should use IHeatmapVisualState.BeginUpdate instead |
| C[IHeatmapProfileConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapProfileConsumer.md) | |
| C[IHeatmapSeriesLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesLease.md) | Per-series mutation surface inside the lease. Append, replace, clear, and trim operations are buffered to the back-stage and become visible to the renderer on lease disposal |
| C[IHeatmapSeriesStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapSeriesStateNode.md) | Read-only handle to a series within an IHeatmapVisualStateNode. The renderer iterates committed samples through this interface; mutation goes through the lease (IHeatmapVisualLease.Series) |
| C[IHeatmapTimerConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTimerConsumer.md) | Optional capability: receive periodic timer ticks. Most indicators do not need this; only opt in if the indicator must do work on a wall clock rather than in response to incoming data — for example, to expire stale state at session boundaries when no trade tick has arrived to wake the indicator up |
| C[IHeatmapTradeTickConsumer](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapTradeTickConsumer.md) | |
| C[IHeatmapVisualLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualLease.md) | Per-visual mutation surface inside the lease. Style and presentation are mutable properties on the lease; series content is mutated through Series |
| C[IHeatmapVisualState](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualState.md) | Persistent visual state owned by a single indicator instance and read by the renderer at frame rate. Created by the platform from the indicator's HeatmapIndicatorDescriptor and bound to the indicator via HeatmapIndicator.State; the same reference is valid for the entire lifetime of the runner. Resets (instrument switch, explicit `RequestStateResetAsync`) clear the content of every series but do NOT replace the state instance |
| C[IHeatmapVisualStateLease](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateLease.md) | The exclusive write lease on an IHeatmapVisualState. Acquired via IHeatmapVisualState.BeginUpdate; disposing commits the back-stage to the front. A lease can only be used inside the calling stack frame — passing it across `await` points or to background tasks is a misuse pattern (the platform serialises every indicator callback on a single task, so all legitimate writes happen from inside one of those callbacks) |
| C[IHeatmapVisualStateNode](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapVisualStateNode.md) | Read-only handle to a visual within an IHeatmapVisualState. Carries the descriptor metadata plus the list of series; mutation goes through the lease (IHeatmapVisualStateLease.Visual) |
| C[IHeatmapWarmupIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1Heatmap_1_1IHeatmapWarmupIndicator.md) | |
| C[BaseDataSeries](../classes/classATAS_1_1Indicators_1_1BaseDataSeries.md) | Base generic data series class providing common functionality |
| C[BaseIndicator](../classes/classATAS_1_1Indicators_1_1BaseIndicator.md) | Base class for custom indicators in a chart |
| C[Candle](../classes/classATAS_1_1Indicators_1_1Candle.md) | Represents a candle in trading, which includes open, high, low, and close prices |
| C[CandleDataSeries](../classes/classATAS_1_1Indicators_1_1CandleDataSeries.md) | Represents a data series of candles. Each element in the series is a Candle |
| C[CandlePartSeries](../classes/classATAS_1_1Indicators_1_1CandlePartSeries.md) | Represents a data series of decimal values derived from specific parts of an IndicatorCandle created by an ICandleCreator |
| C[ChartObject](../classes/classATAS_1_1Indicators_1_1ChartObject.md) | Base class for objects in a chart |
| C[Container](../classes/classATAS_1_1Indicators_1_1Container.md) | Represents a container with a defined region on the chart |
| C[CumulativeTrade](../classes/classATAS_1_1Indicators_1_1CumulativeTrade.md) | Represents a cumulative trade, which is a trade that includes multiple prints or executions |
| C[CumulativeTradesRequest](../classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) | Represents a request to retrieve cumulative trade data within a specified time range or for a particular date |
| C[CustomValue](../classes/classATAS_1_1Indicators_1_1CustomValue.md) | Represents a custom value with associated properties |
| C[CustomValueDataSeries](../classes/classATAS_1_1Indicators_1_1CustomValueDataSeries.md) | Represents a custom data series that holds CustomValue objects |
| C[DataSeriesTypeConverter](../classes/classATAS_1_1Indicators_1_1DataSeriesTypeConverter.md) | |
| C[ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md) | An extended base class for custom indicators that provide additional functionality for drawing, alerts, market data handling, etc |
| C[Filter](../classes/classATAS_1_1Indicators_1_1Filter.md) | Generic filter class that implements the IFilterValue interface |
| C[FilterBase](../classes/classATAS_1_1Indicators_1_1FilterBase.md) | Base class for filters implementing the IFilter interface |
| C[FilterBool](../classes/classATAS_1_1Indicators_1_1FilterBool.md) | Represents a filter with a boolean value type. Inherits from Filter where TValue is set to bool and TFilter is set to FilterBool |
| C[FilterColor](../classes/classATAS_1_1Indicators_1_1FilterColor.md) | Represents a filter with a value type of CrossColor. Inherits from Filter where TValue is set to CrossColor and TFilter is set to FilterColor |
| C[FilterEnum](../classes/classATAS_1_1Indicators_1_1FilterEnum.md) | |
| C[FilterHeatmapTypes](../classes/classATAS_1_1Indicators_1_1FilterHeatmapTypes.md) | Represents a filter for heatmap types with custom JSON serialization/deserialization |
| C[FilterInt](../classes/classATAS_1_1Indicators_1_1FilterInt.md) | Represents a filter for integer values with custom JSON serialization/deserialization |
| C[FilterKey](../classes/classATAS_1_1Indicators_1_1FilterKey.md) | Represents a filter for key values with custom JSON serialization/deserialization |
| C[FilterRangeBase](../classes/classATAS_1_1Indicators_1_1FilterRangeBase.md) | Represents an abstract base class for filters that represent a range of values with custom JSON serialization/deserialization |
| C[FilterRangeInt](../classes/classATAS_1_1Indicators_1_1FilterRangeInt.md) | Represents a filter that represents a range of integer values with custom JSON serialization/deserialization |
| C[FilterRangeValue](../classes/classATAS_1_1Indicators_1_1FilterRangeValue.md) | Represents a range of values of type TValue with support for property change notifications |
| C[FilterRenderPen](../classes/classATAS_1_1Indicators_1_1FilterRenderPen.md) | Represents a filter for PenSettings objects with support for property change notifications |
| C[FilterString](../classes/classATAS_1_1Indicators_1_1FilterString.md) | Represents a filter for string values with support for property change notifications |
| C[FilterTimeSpan](../classes/classATAS_1_1Indicators_1_1FilterTimeSpan.md) | Represents a filter for TimeSpan values with custom JSON serialization/deserialization |
| C[FixedProfileRequest](../classes/classATAS_1_1Indicators_1_1FixedProfileRequest.md) | Represents a request for a fixed profile with a specific period |
| C[ICandleCreator](../interfaces/interfaceATAS_1_1Indicators_1_1ICandleCreator.md) | Represents an interface for creating and managing indicator candles |
| C[IChart](../interfaces/interfaceATAS_1_1Indicators_1_1IChart.md) | Interface for a chart containing various chart-related information and methods |
| C[IChartColorsStore](../interfaces/interfaceATAS_1_1Indicators_1_1IChartColorsStore.md) | Interface for accessing colors and pens used in a chart's rendering |
| C[IChartContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IChartContainer.md) | Interface for a chart container that holds chart-related information and methods |
| C[IChartCoordinatesManager](../interfaces/interfaceATAS_1_1Indicators_1_1IChartCoordinatesManager.md) | Interface for managing chart coordinates and scaling |
| C[IContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IContainer.md) | Interface for defining a container that represents a rectangular region |
| C[ICrossTradingIndicatorContext](../interfaces/interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md) | Context for indicators that need to access cross-trading instrument data. Allows indicators to subscribe to cross-trading instrument changes and access market data for the selected cross-trading instrument without creating new subscriptions |
| C[IDataSeries](../interfaces/interfaceATAS_1_1Indicators_1_1IDataSeries.md) | Interface for data series, providing essential properties and methods |
| C[IDrawingObjectsListInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IDrawingObjectsListInfo.md) | Interface for providing information about the list of drawing objects on the chart |
| C[IFilter](../interfaces/interfaceATAS_1_1Indicators_1_1IFilter.md) | Represents a filter with common properties and functionality |
| C[IFilterEnum](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterEnum.md) | |
| C[IFilterValue](../interfaces/interfaceATAS_1_1Indicators_1_1IFilterValue.md) | Represents a filter with a flexible value that can hold data of any type. Inherits the properties of IFilter |
| C[IIndicatorContainer](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorContainer.md) | Interface for an indicator container that holds indicator-related information and methods |
| C[IIndicatorDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorDataProvider.md) | Represents a data provider for an indicator, providing access to various data and services related to the indicator |
| C[IIndicatorServiceProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IIndicatorServiceProvider.md) | |
| C[IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) | Interface representing instrument information |
| C[IIntCandle](../interfaces/interfaceATAS_1_1Indicators_1_1IIntCandle.md) | Represents an interface for an integer-based candle |
| C[IKeyboardInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IKeyboardInfo.md) | Interface for providing information about the keyboard state |
| C[IKnowFixedProfiles](../interfaces/interfaceATAS_1_1Indicators_1_1IKnowFixedProfiles.md) | Represents an interface for objects that know fixed profiles and can request them |
| C[IMarketByOrdersCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersCache.md) | Interface for manager that provides access to market by orders cache |
| C[IMarketByOrdersDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersDataProvider.md) | Interface for manager that provides access to market by order data |
| C[IMarketByOrdersWithTradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketByOrdersWithTradesCache.md) | Interface for manager that provides access to market by orders and trades cache |
| C[IMarketDepthInfoProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketDepthInfoProvider.md) | Interface for providing market depth information |
| C[IMarketTimeProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md) | |
| C[IMouseLocationInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IMouseLocationInfo.md) | Interface for providing information about the mouse location within the chart |
| C[Indicator](../classes/classATAS_1_1Indicators_1_1Indicator.md) | Base class for custom indicators |
| C[IndicatorCandle](../classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) | Represents an Indicator Candle |
| C[IndicatorDataProvider](../classes/classATAS_1_1Indicators_1_1IndicatorDataProvider.md) | Implementation of the IIndicatorDataProvider interface that provides access to various data and services related to an indicator |
| C[IndicatorSeries](../classes/classATAS_1_1Indicators_1_1IndicatorSeries.md) | Represents a custom data series for an indicator, derived from BaseDataSeries |
| C[INotifyPanelPropertyChanged](../interfaces/interfaceATAS_1_1Indicators_1_1INotifyPanelPropertyChanged.md) | Notifies clients that a panel property value has changed |
| C[InstrumentInfo](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md) | Implementation of the IInstrumentInfo interface representing instrument information |
| C[IOnlineDataProvider](../interfaces/interfaceATAS_1_1Indicators_1_1IOnlineDataProvider.md) | Interface for an online data provider that provides access to real-time market data |
| C[IPlatformSettings](../interfaces/interfaceATAS_1_1Indicators_1_1IPlatformSettings.md) | Interface for accessing platform settings |
| C[IPropertiesEditor](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md) | |
| C[IPropertiesEditorOwner](../interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md) | |
| C[ISupportedPriceInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ISupportedPriceInfo.md) | Represents an interface for supporting price information |
| C[ITimeMarketDataCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITimeMarketDataCache.md) | Cache that holds recent items based on a specified amount of time |
| C[ITradesCache](../interfaces/interfaceATAS_1_1Indicators_1_1ITradesCache.md) | Interface for manager that provides access to trades cache |
| C[ITradingManager](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingManager.md) | Interface representing a trading manager for handling trading-related operations |
| C[ITradingVolumeInfo](../interfaces/interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md) | Interface for providing information about the volume selector |
| C[ITransientIndicator](../interfaces/interfaceATAS_1_1Indicators_1_1ITransientIndicator.md) | Marker interface for indicators that are managed programmatically and should not be persisted in the chart template. Transient indicators are excluded from serialization and are added/removed at runtime by platform components (e.g. ChartTraderViewModel) |
| C[IVolumeSelectorItem](../interfaces/interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md) | Interface representing the volume template |
| C[LineSeries](../classes/classATAS_1_1Indicators_1_1LineSeries.md) | Represents a horizontal line with a single value |
| C[MarketDataArg](../classes/classATAS_1_1Indicators_1_1MarketDataArg.md) | Represents a data point in the market |
| C[MarketDepthInfoProvider](../classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) | A class that implements the IMarketDepthInfoProvider interface to provide market depth information |
| C[MarketDepthSnapshot](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshot.md) | Represents the end state of market depth over a specified time period |
| C[MarketDepthSnapshotRequest](../classes/classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md) | Represents a request to retrieve a snapshot of the market depth for a specified time range |
| C[NotifyPropertyChangedBase](../classes/classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md) | Base class for implementing the INotifyPropertyChanged interface |
| C[ObjectDataSeries](../classes/classATAS_1_1Indicators_1_1ObjectDataSeries.md) | Represents a data series of objects, allowing storing any type of data elements |
| C[PaintbarsDataSeries](../classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) | Represents a data series of paintbars, each element is a nullable CrossColor value |
| C[ParameterAttribute](../classes/classATAS_1_1Indicators_1_1ParameterAttribute.md) | |
| C[PriceSelectionDataSeries](../classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md) | Represents a data series of price selection values, each element is a synchronized list of PriceSelectionValue |
| C[PriceSelectionValue](../classes/classATAS_1_1Indicators_1_1PriceSelectionValue.md) | Represents a class for defining price level selection in clusters and bars. Using in PriceSelectionDataSeries |
| C[PriceVolumeInfo](../classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | Represents information on volumes at a specific price |
| C[RangeDataSeries](../classes/classATAS_1_1Indicators_1_1RangeDataSeries.md) | Represents a data series of range values, each element is a RangeValue |
| C[RangeValue](../classes/classATAS_1_1Indicators_1_1RangeValue.md) | RangeDataSeries element |
| C[RedrawArg](../classes/classATAS_1_1Indicators_1_1RedrawArg.md) | Represents the arguments for requesting a redraw of a chart |
| C[ValueArea](../classes/classATAS_1_1Indicators_1_1ValueArea.md) | Represents information on Value area high/low |
| C[ValueChangingEventArgs](../classes/classATAS_1_1Indicators_1_1ValueChangingEventArgs.md) | Provides event arguments for a value changing event |
| ►C[ValueDataSeries](../classes/classATAS_1_1Indicators_1_1ValueDataSeries.md) | Represents a data series of decimal values, each element is a decimal |
| C[BarColors](../classes/classATAS_1_1Indicators_1_1ValueDataSeries_1_1BarColors.md) | Manages colors per bar for the associated ValueDataSeries |
| ►N[Strategies](../namespaces/namespaceATAS_1_1Strategies.md) | |
| ►N[ATM](../namespaces/namespaceATAS_1_1Strategies_1_1ATM.md) | |
| C[ATMStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | |
| C[BaseStopProfitStrategy](../classes/classATAS_1_1Strategies_1_1ATM_1_1BaseStopProfitStrategy.md) | |
| C[ChangesInfo](../structs/structATAS_1_1Strategies_1_1ATM_1_1ChangesInfo.md) | |
| C[IATMStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IATMStrategy.md) | |
| C[ISimpleStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISimpleStopProfitStrategy.md) | |
| C[IStopProfitSettings](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitSettings.md) | |
| C[IStopProfitStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStopProfitStrategy.md) | |
| C[IStrategyMarketDataProvider](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1IStrategyMarketDataProvider.md) | |
| C[ISupportCustomStopOrTake](../interfaces/interfaceATAS_1_1Strategies_1_1ATM_1_1ISupportCustomStopOrTake.md) | |
| C[StopProfit](../classes/classATAS_1_1Strategies_1_1ATM_1_1StopProfit.md) | |
| ►N[Chart](../namespaces/namespaceATAS_1_1Strategies_1_1Chart.md) | |
| C[ChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md) | Represents an abstract class for a chart strategy that extends the functionality of an Indicator and implements the IChartStrategy and ILoggerSource interfaces |
| C[IChartStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1Chart_1_1IChartStrategy.md) | Represents a chart strategy that extends the basic functionality of an IStrategy with additional chart-related features |
| C[SmaChartStrategy](../classes/classATAS_1_1Strategies_1_1Chart_1_1SmaChartStrategy.md) | |
| ►N[Editors](../namespaces/namespaceATAS_1_1Strategies_1_1Editors.md) | |
| C[ProtectionSettingsEditor](../classes/classATAS_1_1Strategies_1_1Editors_1_1ProtectionSettingsEditor.md) | |
| C[IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | Represents a trading strategy |
| C[Strategy](../classes/classATAS_1_1Strategies_1_1Strategy.md) | Base class for implementing trading strategies |
| C[StrategyNotificationEventArgs](../classes/classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md) | Provides data for the StrategyNotification event |
| C[StrategyStateChangedEventArgs](../classes/classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md) | Provides data for the StrategyStateChanged event |
| ▼N[OFT](../namespaces/namespaceOFT.md) | |
| ►N[Attributes](../namespaces/namespaceOFT_1_1Attributes.md) | |
| ►N[Editors](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md) | |
| C[CheckEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1CheckEditorAttribute.md) | Check box editor for object |
| C[ComboBoxEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md) | |
| C[DataSeriesEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1DataSeriesEditorAttribute.md) | |
| C[IsExpandedAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1IsExpandedAttribute.md) | |
| C[MaskAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1MaskAttribute.md) | |
| C[NumericEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1NumericEditorAttribute.md) | |
| C[PostValueModeAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1PostValueModeAttribute.md) | |
| C[SelectDirectoryEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SelectDirectoryEditorAttribute.md) | |
| C[SelectFileEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SelectFileEditorAttribute.md) | |
| C[SoundComboBoxEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.md) | Specialized ComboBox editor for internal application sounds. Renders non-editable list with a Play button in item template |
| C[TextEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md) | Represents an attribute that provides additional metadata for controlling text editors |
| C[FeatureIdAttribute](../classes/classOFT_1_1Attributes_1_1FeatureIdAttribute.md) | |
| C[HelpLinkAttribute](../classes/classOFT_1_1Attributes_1_1HelpLinkAttribute.md) | |
| C[IgnoreCloneAttribute](../classes/classOFT_1_1Attributes_1_1IgnoreCloneAttribute.md) | Ignore clone property or field |
| C[LogoAttribute](../classes/classOFT_1_1Attributes_1_1LogoAttribute.md) | |
| C[MappingAttribute](../classes/classOFT_1_1Attributes_1_1MappingAttribute.md) | |
| C[ParameterAttribute](../classes/classOFT_1_1Attributes_1_1ParameterAttribute.md) | |
| C[ReferralLinkAttribute](../classes/classOFT_1_1Attributes_1_1ReferralLinkAttribute.md) | |
| C[RegisterLinkAttribute](../classes/classOFT_1_1Attributes_1_1RegisterLinkAttribute.md) | |
| C[SupportedExchangesAttribute](../classes/classOFT_1_1Attributes_1_1SupportedExchangesAttribute.md) | Specifies the exchange codes supported by a connector. The attribute accepts a type containing public const string fields, each representing a supported exchange code |
| C[TabAttribute](../classes/classOFT_1_1Attributes_1_1TabAttribute.md) | Attribute that assigns a property or all properties of a class to a named tab in the settings UI. Use alongside System.ComponentModel.DataAnnotations.DisplayAttribute for property display metadata (Name, GroupName, Description, Order) |
| C[VisibleWhenAttribute](../classes/classOFT_1_1Attributes_1_1VisibleWhenAttribute.md) | Makes the property visible only when the specified source property's value matches one of the provided values. Used for dynamic property visibility in the settings UI (e.g., showing/hiding properties based on a mode enum) |
| ►N[Core](../namespaces/namespaceOFT_1_1Core.md) | |
| C[BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md) | |
| C[IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) | |
| C[IConnectorSettingsAction](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsAction.md) | |
| C[IConnectorSettingsSupportInitialization](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsSupportInitialization.md) | |
| C[ILoginPasswordConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ILoginPasswordConnectorSettings.md) | |
| C[ITypedConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ITypedConnectorSettings.md) | |
| C[ReferralData](../classes/classReferralData.md) | |
