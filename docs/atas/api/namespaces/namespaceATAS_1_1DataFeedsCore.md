# ATAS.DataFeedsCore Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1DataFeedsCore.html

| Namespaces | |
| --- | --- |
| namespace | [Commissions](./namespaceATAS_1_1DataFeedsCore_1_1Commissions.md) |
| | |
| namespace | [ConnectorWebsocket](./namespaceATAS_1_1DataFeedsCore_1_1ConnectorWebsocket.md) |
| | |
| namespace | [Database](./namespaceATAS_1_1DataFeedsCore_1_1Database.md) |
| | |
| namespace | [Dom](./namespaceATAS_1_1DataFeedsCore_1_1Dom.md) |
| | |
| namespace | [Exceptions](./namespaceATAS_1_1DataFeedsCore_1_1Exceptions.md) |
| | |
| namespace | [Rebate](./namespaceATAS_1_1DataFeedsCore_1_1Rebate.md) |
| | |
| namespace | [SessionServer](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md) |
| | |
| namespace | [Statistics](./namespaceATAS_1_1DataFeedsCore_1_1Statistics.md) |
| | |
| namespace | [TradeStatistics](./namespaceATAS_1_1DataFeedsCore_1_1TradeStatistics.md) |
| | |

| Classes | |
| --- | --- |
| class | [AsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md) |
| | Connector base that allows to use `await` to switch to the connector queue thread. [More...](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnector.md#details) |
| | |
| class | [AsyncConnectorMessage](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md) |
| | Service message for passing continuations to the connector thread. [More...](../classes/classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md#details) |
| | |
| class | [AsyncMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1AsyncMessageQueue.md) |
| | |
| class | [BaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1BaseConnector.md) |
| | |
| class | [BaseMarketDataOnlyConnectorSettings](../classes/classATAS_1_1DataFeedsCore_1_1BaseMarketDataOnlyConnectorSettings.md) |
| | |
| class | [BaseMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1BaseMessageQueue.md) |
| | |
| class | [BasketConnector](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnector.md) |
| | |
| class | [BasketConnectorSettings](../classes/classATAS_1_1DataFeedsCore_1_1BasketConnectorSettings.md) |
| | |
| class | [ConnectionStateEventArgs](../classes/classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.md) |
| | |
| class | [ConnectorLatencyManager](../classes/classATAS_1_1DataFeedsCore_1_1ConnectorLatencyManager.md) |
| | |
| class | [CryptoAsyncConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoAsyncConnector.md) |
| | |
| class | [CryptoBaseConnector](../classes/classATAS_1_1DataFeedsCore_1_1CryptoBaseConnector.md) |
| | |
| class | [DailyNote](../classes/classATAS_1_1DataFeedsCore_1_1DailyNote.md) |
| | |
| class | EcnHelper |
| | |
| class | [Exchange](../classes/classATAS_1_1DataFeedsCore_1_1Exchange.md) |
| | |
| class | Extensions |
| | |
| class | [GroupExchange](../classes/classATAS_1_1DataFeedsCore_1_1GroupExchange.md) |
| | |
| class | [HistoryMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) |
| | Represents a historical trade record. [More...](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md#details) |
| | |
| class | [HistoryMyTradePlaybook](../classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTradePlaybook.md) |
| | |
| interface | [IConnectorExchangeInfoProvider](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md) |
| | Defines methods to retrieve exchange information based on security or exchange codes. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md#details) |
| | |
| interface | [IConnectorLatencyManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IConnectorLatencyManager.md) |
| | |
| interface | [ICryptoKeySecretConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ICryptoKeySecretConnector.md) |
| | |
| interface | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) |
| | |
| interface | [IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) |
| | Represents an entity in the application. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#details) |
| | |
| interface | [IEntityFactory](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntityFactory.md) |
| | |
| interface | [IKnowSession](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IKnowSession.md) |
| | |
| interface | [ILiquidationFeed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ILiquidationFeed.md) |
| | Provides liquidation orders stream for securities. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ILiquidationFeed.md#details) |
| | |
| interface | [IMarketByOrdersManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md) |
| | Interface for manager that provides access to market by order data. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md#details) |
| | |
| interface | [IMarketDataPublisher](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDataPublisher.md) |
| | |
| interface | [IMarketDepth](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md) |
| | Represents a market depth entry. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketDepth.md#details) |
| | |
| interface | [IMessageQueue](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMessageQueue.md) |
| | |
| class | [InstrumentExchange](../classes/classATAS_1_1DataFeedsCore_1_1InstrumentExchange.md) |
| | |
| interface | [IOptionsDataFeed](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOptionsDataFeed.md) |
| | |
| interface | [IOrderOptionCloseOnTrigger](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionCloseOnTrigger.md) |
| | Represents an order option for closing a position when triggered. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionCloseOnTrigger.md#details) |
| | |
| interface | [IOrderOptionPostOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.md) |
| | Represents an order option for placing a post-only order. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.md#details) |
| | |
| interface | [IOrderOptionReduceOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionReduceOnly.md) |
| | Represents an order option for reducing the position size. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionReduceOnly.md#details) |
| | |
| interface | [IPasswordChanger](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPasswordChanger.md) |
| | |
| interface | [IPortfolioExtendedInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md) |
| | Base interface for extended portfolio information. Each connector can have its own implementation with unique parameters. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md#details) |
| | |
| interface | [IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md) |
| | Exposes the subset of trading-entity properties required to format prices and derive instrument classification (US bonds, MOEX futures). Implemented by Security directly and by `OFT.Platform.Models.Instrument` via explicit interface implementation. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#details) |
| | |
| interface | [IReferralAccountConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IReferralAccountConnector.md) |
| | |
| interface | [ISecurityPositionManager](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityPositionManager.md) |
| | |
| interface | [ISecurityTradingOptions](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md) |
| | Represents an interface for providing trading options related to a security or trading connector. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISecurityTradingOptions.md#details) |
| | |
| interface | [ISupportChangePosition](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportChangePosition.md) |
| | |
| interface | [ISupportMarketDataOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportMarketDataOnly.md) |
| | |
| interface | [ISupportResetPortfolio](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportResetPortfolio.md) |
| | |
| interface | [ISupportTPlusLimits](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportTPlusLimits.md) |
| | |
| class | [MarketByOrder](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md) |
| | Market by Order (MBO) describes an order-based data feed that provides the ability to view individual queue position, full depth of book and the size of individual orders at each price level. [More...](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#details) |
| | |
| class | [MarketByOrdersManager](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md) |
| | Manager that provides access to market by order data. [More...](../classes/classATAS_1_1DataFeedsCore_1_1MarketByOrdersManager.md#details) |
| | |
| class | [MarketDepth](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md) |
| | Represents a market depth entry. [More...](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepth.md#details) |
| | |
| class | [MarketDepthComparer](../classes/classATAS_1_1DataFeedsCore_1_1MarketDepthComparer.md) |
| | |
| struct | [Money](../structs/structATAS_1_1DataFeedsCore_1_1Money.md) |
| | Represents decimal amount in some currency. [More...](../structs/structATAS_1_1DataFeedsCore_1_1Money.md#details) |
| | |
| class | [MultiConnectorMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1MultiConnectorMessageQueue.md) |
| | |
| class | [MyTrade](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) |
| | Represents a trade entity in the system. [More...](../classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md#details) |
| | |
| class | [News](../classes/classATAS_1_1DataFeedsCore_1_1News.md) |
| | |
| struct | [OptionSeries](../structs/structATAS_1_1DataFeedsCore_1_1OptionSeries.md) |
| | |
| class | [Order](../classes/classATAS_1_1DataFeedsCore_1_1Order.md) |
| | Represents an order for trading on a financial exchange. [More...](../classes/classATAS_1_1DataFeedsCore_1_1Order.md#details) |
| | |
| class | [OvernightSwapValue](../classes/classATAS_1_1DataFeedsCore_1_1OvernightSwapValue.md) |
| | |
| class | [Playbook](../classes/classATAS_1_1DataFeedsCore_1_1Playbook.md) |
| | |
| class | [PnLTradesQueue](../classes/classATAS_1_1DataFeedsCore_1_1PnLTradesQueue.md) |
| | |
| class | [Portfolio](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md) |
| | Represents a portfolio entity with various properties related to account balance, Profit and Loss (PnL), permissions, trading options, and more. [More...](../classes/classATAS_1_1DataFeedsCore_1_1Portfolio.md#details) |
| | |
| class | [PortfolioChange](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioChange.md) |
| | |
| class | [PortfolioExtendedInfoBase](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.md) |
| | Base implementation of IPortfolioExtendedInfo with common functionality. [More...](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioExtendedInfoBase.md#details) |
| | |
| class | [PortfolioGroup](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioGroup.md) |
| | Represents a portfolio group used in the application. [More...](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioGroup.md#details) |
| | |
| class | [PortfolioState](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioState.md) |
| | |
| class | [PortfolioViewer](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md) |
| | Represents a portfolio viewer used in the application. [More...](../classes/classATAS_1_1DataFeedsCore_1_1PortfolioViewer.md#details) |
| | |
| class | [Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) |
| | Represents a trading position. [More...](../classes/classATAS_1_1DataFeedsCore_1_1Position.md#details) |
| | |
| class | [PositionState](../classes/classATAS_1_1DataFeedsCore_1_1PositionState.md) |
| | |
| class | [PositionTradesQueue](../classes/classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md) |
| | |
| struct | [PriceUnit](../structs/structATAS_1_1DataFeedsCore_1_1PriceUnit.md) |
| | |
| class | [Security](../classes/classATAS_1_1DataFeedsCore_1_1Security.md) |
| | Represents a security entity used in the application. [More...](../classes/classATAS_1_1DataFeedsCore_1_1Security.md#details) |
| | |
| class | [SecurityFilter](../classes/classATAS_1_1DataFeedsCore_1_1SecurityFilter.md) |
| | |
| class | [SecurityMargin](../classes/classATAS_1_1DataFeedsCore_1_1SecurityMargin.md) |
| | |
| class | [SecurityPositionManager](../classes/classATAS_1_1DataFeedsCore_1_1SecurityPositionManager.md) |
| | |
| class | [SecurityRoute](../classes/classATAS_1_1DataFeedsCore_1_1SecurityRoute.md) |
| | |
| class | [SecurityRouteCache](../classes/classATAS_1_1DataFeedsCore_1_1SecurityRouteCache.md) |
| | |
| class | [SecuritySummary](../classes/classATAS_1_1DataFeedsCore_1_1SecuritySummary.md) |
| | |
| class | [SecurityTradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md) |
| | Each connector may have different order placement settings like TimeInForce or ReduceOnly etc. This class shows connector configuration. [More...](../classes/classATAS_1_1DataFeedsCore_1_1SecurityTradingOptions.md#details) |
| | |
| class | [ServerPnL](../classes/classATAS_1_1DataFeedsCore_1_1ServerPnL.md) |
| | |
| class | [SimpleMessageQueue](../classes/classATAS_1_1DataFeedsCore_1_1SimpleMessageQueue.md) |
| | |
| class | [SimpleMyTrade](../classes/classATAS_1_1DataFeedsCore_1_1SimpleMyTrade.md) |
| | |
| class | [SubscriptionCounter](../classes/classATAS_1_1DataFeedsCore_1_1SubscriptionCounter.md) |
| | |
| class | [SubscriptionManager](../classes/classATAS_1_1DataFeedsCore_1_1SubscriptionManager.md) |
| | |
| class | [Trade](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md) |
| | Represents an tick on a financial exchange. [More...](../classes/classATAS_1_1DataFeedsCore_1_1Trade.md#details) |
| | |
| class | [TradesTimeChecker](../classes/classATAS_1_1DataFeedsCore_1_1TradesTimeChecker.md) |
| | |
| class | [TradingOptions](../classes/classATAS_1_1DataFeedsCore_1_1TradingOptions.md) |
| | |
| class | [TradingOptionsSecurity](../classes/classATAS_1_1DataFeedsCore_1_1TradingOptionsSecurity.md) |
| | |
| class | [UniversalMarketData](../classes/classATAS_1_1DataFeedsCore_1_1UniversalMarketData.md) |
| | |
| class | [User](../classes/classATAS_1_1DataFeedsCore_1_1User.md) |
| | |
| class | [UserChange](../classes/classATAS_1_1DataFeedsCore_1_1UserChange.md) |
| | |
| class | [UserChangeHistory](../classes/classATAS_1_1DataFeedsCore_1_1UserChangeHistory.md) |
| | |
| class | [UserGroup](../classes/classATAS_1_1DataFeedsCore_1_1UserGroup.md) |
| | |
| class | [UserRole](../classes/classATAS_1_1DataFeedsCore_1_1UserRole.md) |
| | |
| class | [UserRoleRight](../classes/classATAS_1_1DataFeedsCore_1_1UserRoleRight.md) |
| | |
| struct | [VolumeUnit](../structs/structATAS_1_1DataFeedsCore_1_1VolumeUnit.md) |
| | |
| class | [WorkingTime](../classes/classATAS_1_1DataFeedsCore_1_1WorkingTime.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [RebateResult](./namespaceATAS_1_1DataFeedsCore.md#ace8c78210ca5b50f90165046c04f5e7a) { [Valid](./namespaceATAS_1_1DataFeedsCore.md#ace8c78210ca5b50f90165046c04f5e7aa3ac705f2acd51a4613f9188c05c91d0d) = 0
, [WrongRefCode](./namespaceATAS_1_1DataFeedsCore.md#ace8c78210ca5b50f90165046c04f5e7aa0a6fd2bb5774cb5633857380f7ee98ce) = 1
, [WrongBroker](./namespaceATAS_1_1DataFeedsCore.md#ace8c78210ca5b50f90165046c04f5e7aaac68a57ef931c425920dd0dfccd1d21c) = 2
, [WrongBrokerRefCode](./namespaceATAS_1_1DataFeedsCore.md#ace8c78210ca5b50f90165046c04f5e7aaf6235c988605f538ed688e3aa6b80b57) = 3
 } |
| | |
| enum | [Currencies](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9) {
  [USD](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a3518f8944d42212dd37daffe097d216e) = 0
, [EUR](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9aa055562bdb59ad8ba9cc680367308118) = 1
, [GBP](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a3add2285094fdc3186902810080c1465) = 2
, [JPY](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a0d22295dba298b8a4831a943545b0810) = 3
,
  [RUB](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a67654743cc32dfeebf2d2d154db69cd8) = 4
, [AUD](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9abfab7c49998f5863fb8d10c7f0ed8873) = 5
, [CHF](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a2fc2052eaa45739a9fd0854d4ed60178) = 6
, [BTC](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a4b9169eb3e07e0e885eb62f7bfc41a33) = 7
,
  [ETH](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9af8d2e1584059489f8ffa3663b3223df2) = 8
, [EOS](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9ad3428ee9afeb947b67aa37e634148ee5) = 9
, [XRP](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9ab963e1952393b6f6899c6978de03a6f6) = 10
, [LTC](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a521f46fd4a7aee3efec387c31967ba7f) = 11
,
  [BCH](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a43f2b0dd09b2b91146fe726dcc47381b) = 12
, [XTZ](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a9f57920a34bdd7df729f4e79ef05f7d9) = 13
, [UNI](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a94030d57154f1133d934df07c99dc09b) = 14
, [DOT](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a40679521b5da0954b705341a2859f782) = 15
,
  [ADA](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a347cbca85aed318356cd4f78b6509882) = 16
, [LINK](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a58c0a0e97e6d2f615bc264c2775fda44) = 17
, [USDT](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a6741d2f11f5e09e769e84f9a9e37631b) = 18
, [BIT](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9adb957fdc8000e1eef04a243f5199aa52) = 19
,
  [LUNA](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a7e015e3ed14067228004786f0281a295) = 20
, [MANA](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9ad07a90634be6f612ce63f418ecd3cc28) = 21
, [SOL](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a46ba9eba2f4b52116787dfa6fb4278e3) = 22
, [BUSD](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9ae729486fba723536b9d7536e284b8ef6) = 23
,
  [USDC](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9ae874c10fb3df15c8b11b57765b6802d8) = 24
, [XCH](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a6a5ef8fcb61da9e7d6603621d7e1a5b9) = 25
, [BNB](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a87243927c932713e00ea95a19985b447) = 26
, [DOGE](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a6bb4bf629a4cf5d0c2d0ad3973e9d7bf) = 27
,
  [UST](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a445eb8b348dbaef887011882b56df69e) = 28
, [AVAX](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a5fea552bc822cd3c51cdf75b3c252e0b) = 29
, [SHIB](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a812621bb2714d66b99fb768623a83a74) = 30
, [DAI](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a80142798934619bd2f59e9aec3d85943) = 31
,
  [MATIC](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a52f337ca560f3b8f5f581ce22d4924b3) = 32
, [TRX](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a364e6beb127ef9ae8d1cdf6c1d5699b7) = 33
, [BGB](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9af6dd3784eff3ba64d407875be9562e29) = 34
, [INCH](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a30a479d5d88158a7f4248944ccbf72b0) = 35
,
  [CNY](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a889ee9331f891492f89b1e578fa377e0) = 36
, [BNFCR](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a1e047568caf7c751921b01af96932ab6) = 37
, [LOT](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9a995300535f52f2af36e77bea0739e9c5) = 10000

 } |
| | Represents a list of currency values used in the application. [More...](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9) |
| | |
| enum | [EcnId](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394) {
  [NONE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ab50339a10e1de285ac99d4c3990b8693)
, [NASDAQ](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a7dfd4a400d092d56e089f4e47411a0a0)
, [NYSE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394af8de4250c89fe3200c8c7126ca364fed)
, [SIAC](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a8bbf9e8bde529fda1d7b7b2e3bd06c39)
,
  [ISI](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394adc101777b18a39292144c8423537a284)
, [ISLAND](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394abffc21a7b2365fe867f5c7ca1277ea0a)
, [ARCA](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a6720d45c5408ffedf8c96ae5c6dcbe5c)
, [ADFN](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a2ec76604369f9d43cd1da39fee053991)
,
  [BATS](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a6ab8edde7f00fe1c10415554efb8fa6e)
, [INCA](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a7ff5904ef8f2daeb64dca495d54dc7ef)
, [NYOB](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a90e596273d649e16946513a6fbc1823b)
, [MKXT](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394affbb221f840e72f56ab24835390d110c)
,
  [CME](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ac421a6cf607de61ff7c71a6ed21c7031)
, [ESSEX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394afc3d37b7f34583257ef24b81d43a0caa)
, [CHIC](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a104d92f8251d2ed56bf62cc5d03a97ce)
, [BRASS](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394af8c23b538d2b40948387f9b4c4881586)
,
  [BOS](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ae938c5bbd29de26c3a600a0330465f8b)
, [SUMO](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a6cb54e6efd1fcb8312692342cded7d38)
, [INET](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394acbeeb469aeabf16bcff81f4cde1e0b48)
, [BLZ](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394aa5562a93dc6211f36280da084c282ebb)
,
  [EDGX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a61cb5935e50d130aebe6623a60dba4ea)
, [CBOT](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ab7e8082e15bc954781923a9547a8b886)
, [IPE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a065957dff2d3bb9f6ddfb16bc7be5db8)
, [NYBOT](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ae2a7a8f79f097fd2a8d1f40ae9350b8b)
,
  [USFE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ad7b5346230f0e4c8d1ede6e190850a18)
, [EUREX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394afdcdd07063567217c7e3747919cbe18f)
, [EDGA](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a8254b49be07eed849313a48a2698ab95)
, [TSE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a778375587ff90a4c277c3a61becd768e)
,
  [AMEX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ad28e06acbeee71a5b573a392ae393ea1)
, [CBOE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a41c5ab985362ef93005f897da1aab9dc)
, [PCX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394abf370c4b5c7e048f06206adcb342906d)
, [BOX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ae657cce1913c857166b0475f18668ef5)
,
  [ISE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394af949099250e760e7001ff4acbd02488d)
, [PHX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a1375575710990aaefeb0228d672c5521)
, [CMECBT](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a4623524743dc8bc4f46f8b693bfcd5c4)
, [VSE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a26f9b84e298cbcad884ea00d552c7c5a)
,
  [PENSON](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394acf221f79ab60f20efac351bce0d48fe4)
, [OTC](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a99d34e1cefc7f86f9f4470d74e29e052)
, [NSE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394aac6d7f70650e35d39af1e736560cbfc3)
, [MI](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a9b38067e23298837802635d5172733d7)
,
  [CHX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394abbaed2e79586cbe52ba84a55266a84e9)
, [EDGE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a6563b7570ee4add31ffc4e94fa86b6fb)
, [BATX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a16af925d55cd8a4f2717ba3891c24d36)
, [BATY](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ad33e7903a0726328ed58aa6ece61838b)
,
  [EDGD](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394accbcb7b1d7e343588a1e003a44be09b5)
, [PHL](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a024d0a19e2c7138683b785b4e0412532)
, [ARCX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a407b183cdf668e72cc6d1612f7024810)
, [NTRF](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a28302424c663d0f95263f2b14512f6b2)
,
  [IEX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a5b95523b3a2c3ce3ae3e6df8f9fbe858)
, [MEMX](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a506239aefae7301159874d0996decf72)
, [PEARL](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a1d59b3c4c31db1cf1cc94c1c14455097)
, [SMALL](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a9b9c17e13f0e3dc9860a26e08b59b2a7)
,
  [LTSE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394ace877e5c47e753bb3eb3751b1a8b0092)
, [LSE](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a022c9c355248b48d98413c3d5b7870ae)
, [UNKNOWN](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394a696b031073e74bf2cb98e5ef201d4aa3) = 255

 } |
| | |
| enum | [EntityAction](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9e) {
  [Logon](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae245ad2bb801b285a62cd0accab6607f) = 0
, [Logout](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0323de4f66a1700e2173e9bcdce02715) = 1
, [UserAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea804479841aecb64f83610fc8d3be160a) = 2
, [UserChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea461b97fa8b21290b7974597722e59bf0) = 3
,
  [UserRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea9c895849be07df50ca9bd2d99a9cac81) = 4
, [UserLock](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eac6433bce5b7ded5abcdaf200b3fa8f9f) = 29
, [UserKick](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea22ce95a2359e4f1e6fec0c567fafe0ea) = 30
, [UserChangeExpiration](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea5a266660311d4e4a46593ff1acf40d11) = 47
,
  [UserGroupAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea21c245d956ba0a2919b489f2c540bf87) = 5
, [UserGroupChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eabb6e49f537815a22e6d2d026003d900a) = 6
, [UserGroupRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0d11f65fe5e0832f1036969393057d2c) = 7
, [UserRoleAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0c2f902daaca86298c212ec829a6cdfe) = 8
,
  [UserRoleChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea3cc62fe18121d71439c7c5b133275ab8) = 9
, [UserRoleRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea42056907ddac885469f2c41b70e8310f) = 10
, [ExchangeAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea4fb22f59e89bde3c37d35f7224b2629c) = 11
, [ExchangeChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea624b1137bc36788793e76c23c5045738) = 12
,
  [ExchangeRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea1db9fa73405d12ffd5743a9a05da7c96) = 13
, [CommissionGroupAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea1ed96ed6fa371bbb4db60a2bc6286506) = 17
, [CommissionGroupChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eab2843a9bb4c8c462249ea2e49e115daa) = 18
, [CommissionGroupRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea49c6994e99137ebc4b6d41abd73b75d1) = 19
,
  [SecurityMarginAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eac2b54e422146e541e916213705467389) = 20
, [SecurityMarginChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae842434ed6da398c6e2317f89bdd6a5f) = 21
, [SecurityMarginRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaf610276758d9e65c8033b9d28bc056b7) = 22
, [PortfolioAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea778a5e05bfc08d455366f30f8da5f4e7) = 23
,
  [PortfolioChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0098284eaa558f080857263d970d25e1) = 32
, [PortfolioValue](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaf8fdd76dac43c8fc149d58f5edd42e6b) = 24
, [PortfolioLock](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea2fd4f4b5559f64c4bd8a3a30bfb428a6) = 25
, [OrderRegister](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaadd03772c0971b5d271ffcfe4f738584) = 26
,
  [OrderMove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea304117295652ca874b474baa122556b9) = 27
, [OrderCancel](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea739040143a85ea0d9884a7f256a1ed52) = 28
, [NewsSend](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea9129da7bfcd0f0499be7371c50aae848) = 31
, [ServerSettings](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea1bdce89560b19c450acfddafdc9c6a4c) = 33
,
  [RequestStatistics](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0c598ee0336c93bdf22d8047896521dd) = 34
, [OrderCancelled](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea74e70bbb7cc1eb86c5ce255f98781370) = 35
, [OrderMatched](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ead4c6a03a36f2cab7feb8e5fa7beeaa98) = 36
, [ServerPnL](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea56eabfb9743058f4c6c48cece19336c2) = 37
,
  [PortfolioSuspend](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea6f1b8a721841184b520827073445cedf) = 38
, [PortfolioViewerAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eade36d5466dbb3df2a65de67f3f149125) = 39
, [ConnectorAdd](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea6788e3a23cc017bf11bd89c7f21c084f) = 40
, [ConnectorChange](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eadd91916f6d667a83a49eacc0dafc8b16) = 41
,
  [ConnectorRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eabfbe2373eb821a4a188929045b3929c3) = 42
, [ConnectorEnable](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea81178120c9cf2a1dc451c6095d004682) = 43
, [PortfolioViewerRemove](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae5e82edc7cacb99fee496d10b4f0d33a) = 44
, [TradeAllowed](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea110b987608e8fc19f765f6b93a0594d2) = 45
,
  [PortfolioReset](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae92fc0b7579ab1b544f89016599a66e8) = 46
, [GetUserChangeHistory](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea71f7800313ce0532462f93cf1fedab31) = 48
, [GetPortfolioChangeHistory](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea440107bc4382f5a2b35531ff4fc9bed5) = 49
, [UserGroupSuspendPortfolios](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaaca807d503aba92b094b6dd6d7935b9e) = 50
,
  [UserGroupReset](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea2cc76c0620e8c93e10ef44901eed40f6) = 51

 } |
| | |
| enum | [MissingDataCases](./namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9c) { [None](./namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9ca6adf97f83acf6453d4a6a4b1070f3754)
, [Opening](./namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9ca9bd99a0beea48f10663fc4a7d7a33140)
, [Closing](./namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9ca5c8de6f894682fdb1786037b2040a26e)
 } |
| | Describes what part of the HistoryMyTrade was incomplete. [More...](./namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9c) |
| | |
| enum | [EntityType](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) {
  [Security](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2fae32629d4ef4fc6341f1751b405e45)
, [SecurityMargin](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad70ef212ce9ea4e6e7337476444b2c78)
, [Portfolio](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad4f859a96c13f551a2771b7fc3a78d38)
, [Position](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a52f5e0bc3859bc5f5e25130b6c7e8881)
,
  [MarketDepth](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ae1d593b8cbd8f7efcbb2e863121a2651)
, [Trade](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a5f390d80b20daad8f5d2f483fb0ae9d8)
, [Order](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722aa240fa27925a635b08dc28c9e4f9216d)
, [MyTrade](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a8798a86e53e2afa85550f08bd1c0a1cf)
,
  [News](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722add1ba1872df91985ed1ca4cde2dfe669)
, [UserRole](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722afc5add8666240abdb0cc2a453f562413)
, [UserGroup](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a84ddb07d15cabbefecb37c79122a197c)
, [User](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a8f9bfe9d1345237cb3b2b205864da075)
,
  [UserChange](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a461b97fa8b21290b7974597722e59bf0)
, [CommissionGroup](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2c47761b7b849cac9f8d998c5298e9c5)
, [WorkingTime](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a12176a25c4b08c214ef1e7003aa9cda5)
, [Exchange](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a992374d8e2e24f17bebc50a6e57becd6)
,
  [PortfolioChange](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a0098284eaa558f080857263d970d25e1)
, [TradingOptions](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a6721fcd4818b6676c22055f884531eb8)
, [PortfolioState](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a801a1076eb00eeedc198ddb785ecbcd9)
, [PositionState](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a57adb23aac4872c284f332c0330f48ef)
,
  [Connector](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722aedf21d7ecb364e8210ddd3dfaeca6fbf)
, [MarketByOrder](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a68b4b639023e46a46a05d55d86cb680d)

 } |
| | Represents the types of entities used in the application. [More...](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) |
| | |
| enum | [OptionSeriesType](./namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09) { [Regular](./namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09ad2203cb1237cb6460cbad94564e39345)
, [Weekly](./namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09a6c25e6a6da95b3d583c6ec4c3f82ed4d)
, [EndOfMonth](./namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09a5084bfa75f8e971a5b5d12afd23d1e5c)
 } |
| | |
| enum | [MarketByOrderUpdateTypes](./namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617) { [Snapshot](./namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617ad4e2713d1b1725a1592f9268589f990d)
, [New](./namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617a03c2e7e41ffc181a4e84080b4710e81e)
, [Change](./namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617af4ec5f57bd4d31b803312d873be40da9)
, [Delete](./namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617af2a6c498fb90ee345d997f888fce3b18)
 } |
| | Type of market by order update. [More...](./namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617) |
| | |
| enum | [MarketDataDelayPeriods](./namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) { [Realtime](./namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52aaa5ff58bda67e2160b5e5d5a47a4333c3)
, [Delayed15M](./namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52aa3548d45788cb7418a91fcfe98993c10e)
 } |
| | |
| enum | [MarketDataType](./namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) { [Bid](./namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4ae36ba1e187ae2b3ebcfd0a4c68367caf) = 0
, [Ask](./namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4aa0b271a9d8aa8e7473922164d6a1c03c) = 1
, [Trade](./namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4a5f390d80b20daad8f5d2f483fb0ae9d8) = 2
 } |
| | Specifies the type of market data. [More...](./namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) |
| | |
| enum | [MessageType](./namespaceATAS_1_1DataFeedsCore.md#a9b5d2538699d679afdd312abbba1c334) { [Warning](./namespaceATAS_1_1DataFeedsCore.md#a9b5d2538699d679afdd312abbba1c334a0eaadb4fcb48a0a0ed7bc9868be9fbaa)
, [Error](./namespaceATAS_1_1DataFeedsCore.md#a9b5d2538699d679afdd312abbba1c334a902b0d55fddef6f8d651fe1035b7d4bd)
, [Information](./namespaceATAS_1_1DataFeedsCore.md#a9b5d2538699d679afdd312abbba1c334aa82be0f551b8708bc08eb33cd9ded0cf)
 } |
| | |
| enum | [NewsType](./namespaceATAS_1_1DataFeedsCore.md#a6e7968f109c5e89f8e38eec40f049070) { [Text](./namespaceATAS_1_1DataFeedsCore.md#a6e7968f109c5e89f8e38eec40f049070a9dffbf69ffba8bc38bc4e01abf4b1675)
, [Admin](./namespaceATAS_1_1DataFeedsCore.md#a6e7968f109c5e89f8e38eec40f049070ae3afed0047b08059d0fada10f400c1e5)
, [Risk](./namespaceATAS_1_1DataFeedsCore.md#a6e7968f109c5e89f8e38eec40f049070a59ca88f7b0e80cdd9af330af600a9ff6)
 } |
| | |
| enum | [OptionTypes](./namespaceATAS_1_1DataFeedsCore.md#ae2cf0eba1a77bdf2094c56164e77bc29) { [Put](./namespaceATAS_1_1DataFeedsCore.md#ae2cf0eba1a77bdf2094c56164e77bc29ad0bf1810982e9728fcf3ac444a015373)
, [Call](./namespaceATAS_1_1DataFeedsCore.md#ae2cf0eba1a77bdf2094c56164e77bc29ac3755e61202abd74da5885d2e9c9160e)
 } |
| | |
| enum | [OrderDirections](./namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) { [Buy](./namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7ca831a28f1e8df07c553fcd59546465d13) = 0
, [Sell](./namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7ca3068c5a98c003498f1fec0c489212e8b) = 1
 } |
| | Specifies the direction of an order. [More...](./namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) |
| | |
| enum | [OrderStates](./namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716) { [None](./namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716a6adf97f83acf6453d4a6a4b1070f3754) = 0
, [Active](./namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716a4d3d769b812b6faa6b76e1a8abaece2d) = 1
, [Done](./namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716af92965e2c8a7afb3c1b9a5c09a263636) = 2
, [Failed](./namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716ad7c8c85bf79bbe1b7188497c32c3b0ca) = 3
 } |
| | Represents the possible states of an order. [More...](./namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716) |
| | |
| enum | [OrderStatus](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b) {
  [None](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284ba6adf97f83acf6453d4a6a4b1070f3754) = 0
, [Placed](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284baf429e45eaf722cbbb524b40a0313aa67) = 1
, [Filled](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284bad9d586f8c792f8f661052af42536323c) = 2
, [PartlyFilled](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284badfdb144ec91170b82e3d94eb18d3cd49) = 3
,
  [Canceled](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284ba0e22fe7d45f8e5632a4abf369b24e29c) = 4

 } |
| | Represents the possible status of an order. [More...](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b) |
| | |
| enum | [OrderTypes](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) {
  [Limit](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8a80d2677cf518f4d04320042f4ea6c146) = 0
, [Market](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8a31840a66a8d6d223e5b0540138768838) = 1
, [Stop](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8a11a755d598c0c417f9a36758c3da7481) = 2
, [StopLimit](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8a9c28718cc2dea0e9e76cddfbe7e79151) = 3
,
  [Unknown](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8a88183b946cc5f0e8c96b2e66e1c74a7e)

 } |
| | Specifies the type of an order. [More...](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) |
| | |
| enum | [PortfolioChangeType](./namespaceATAS_1_1DataFeedsCore.md#a8f50690870021cddb483a2dc2d76a230) { [IncreaseBalance](./namespaceATAS_1_1DataFeedsCore.md#a8f50690870021cddb483a2dc2d76a230a3812233f22c621e02ab6d4cfed7b2a26)
, [ReduceBalance](./namespaceATAS_1_1DataFeedsCore.md#a8f50690870021cddb483a2dc2d76a230a39c7b23d8477a2cd8e84399699b88287)
, [ChangeLeverage](./namespaceATAS_1_1DataFeedsCore.md#a8f50690870021cddb483a2dc2d76a230a77b764ef0c4f748a7ef532bdae534233)
, [OvernightSwap](./namespaceATAS_1_1DataFeedsCore.md#a8f50690870021cddb483a2dc2d76a230aee4ac485f8390895b2785192bbfe6f72)
 } |
| | |
| enum | [TPlusLimits](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793) { [T0](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793aa48788bd63a0384007cd7d089af6c610) = 0
, [T1](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793ace499dea30cfce118f4fe85da0227e83) = 1
, [T2](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793a71d2c46af01feeea54a0f541243e297b) = 2
, [Tx](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793a4393102620f7750d259e3f050f32ba0b) = 365
 } |
| | Represents different T+ (settlement) periods for trading. T+ refers to the number of days it takes for a trade to settle and for the funds and securities to be exchanged between the parties involved in the transaction. [More...](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793) |
| | |
| enum | [PositionAveragePriceValueTypes](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) {
  [None](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa6adf97f83acf6453d4a6a4b1070f3754)
, [ReceivedFromServer](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa54d6083a6c427dc6aaaf683d4f59ea1d)
, [CalculatedByAllTrades](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfaec1fc7e7c94ce2d8b6d5cf4dfaf427f8)
, [CalculatedByPartTrades](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa6c82a62692256bc4700213488004ec99)
,
  [NotCalculated](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa90a0ffb1cc27df7493c875b893d923bb)

 } |
| | Represents different types of values for the average price of a position. The average price is the average cost of all the trades that make up a position. [More...](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) |
| | |
| enum | [PnlPercentType](./namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87) { [Portfolio](./namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87ad4f859a96c13f551a2771b7fc3a78d38)
, [Margin](./namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87a98369609669478919c74c916440e9978)
 } |
| | Represents the type of percentage calculation for profit and loss (PNL). [More...](./namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87) |
| | |
| enum | [PriceUnitTypes](./namespaceATAS_1_1DataFeedsCore.md#a09683f11280f15a2fea8c886380f250c) { [Price](./namespaceATAS_1_1DataFeedsCore.md#a09683f11280f15a2fea8c886380f250ca3601146c4e948c32b6424d2c0a7f0118)
, [Tick](./namespaceATAS_1_1DataFeedsCore.md#a09683f11280f15a2fea8c886380f250ca0b3516a5bbb77566f904f9d3877f4710)
, [Percent](./namespaceATAS_1_1DataFeedsCore.md#a09683f11280f15a2fea8c886380f250caadaaee4b22041c27198d410c68d952c9)
 } |
| | |
| enum | [SecType](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfaf) {
  [Future](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafaadff855173c9b92b5478129af7d39e03) = 0
, [Forex](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafa6f8f9a7d011c5002fd330949f1457a4e) = 1
, [Stock](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafa27ce7f8b5623b2e2df568d64cf051607) = 2
, [Bitcoin](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafad023ec040f79f1a9b2ac960b43785089) = 3
,
  [CryptoFutures](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafa3eabe7af0439c989e3d08c2ed2132dd5) = 4
, [Indexes](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafacd27b3ae7db40e9849a1412e04b889b0) = 5
, [Option](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafa054b4f3ea543c990f6b125f41af6ebf7) = 6
, [Cfd](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfafad37f191101013d8d9c67d9d39a7a4adc) = 7

 } |
| | Represents the type of security or financial instrument. [More...](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfaf) |
| | |
| enum | [SubscriptionType](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) {
  [None](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3a6adf97f83acf6453d4a6a4b1070f3754) = 0x0000
, [Prints](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3a1eab6627acd62e7abbc0f13a2b369a87) = 0x0001
, [Best](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3a68ef004de6166492c1d668eb8efe09bd) = 0x0002
, [Quotes](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3ac9a28e7f0dbc3ed20a161351c4f29a7b) = 0x0004
,
  [MarketByOrder](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3a68b4b639023e46a46a05d55d86cb680d) = 0x0008
, [Summary](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3a290612199861c31d1036b185b4e69b75) = 0x0010

 } |
| | |
| enum | [TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) {
  [None](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47a6adf97f83acf6453d4a6a4b1070f3754) = 0
, [GoodTillCancel](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47afcf8eb033b88c617c04f00c00cbc734e) = 1

## [◆](https://docs.atas.net/en/)Currencies

| enum [ATAS.DataFeedsCore.Currencies](./namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9) |
| --- |

Represents a list of currency values used in the application.

| Enumerator | |
| --- | --- |
| USD | United States Dollar. |
| EUR | Euro. |
| GBP | British Pound Sterling. |
| JPY | Japanese Yen. |
| RUB | Russian Ruble. |
| AUD | Australian Dollar. |
| CHF | Swiss Franc. |
| BTC | Bitcoin. |
| ETH | Ethereum. |
| EOS | EOS. |
| XRP | Ripple (XRP). |
| LTC | Litecoin. |
| BCH | Bitcoin Cash (BCH). |
| XTZ | Tezos (XTZ). |
| UNI | Uniswap (UNI). |
| DOT | Polkadot (DOT). |
| ADA | Cardano (ADA). |
| LINK | Chainlink (LINK). |
| USDT | Tether (USDT). |
| BIT | BitToken (BIT). |
| LUNA | Terra (LUNA). |
| MANA | Decentraland (MANA). |
| SOL | Solana (SOL). |
| BUSD | Binance USD (BUSD). |
| USDC | USD Coin (USDC). |
| XCH | Chia (XCH). |
| BNB | Binance Coin (BNB). |
| DOGE | Dogecoin (DOGE). |
| UST | TerraUSD (UST). |
| AVAX | Avalanche (AVAX). |
| SHIB | Shiba Inu (SHIB). |
| DAI | Dai. |
| MATIC | Polygon (MATIC). |
| TRX | TRON (TRX). |
| BGB | BAGBUX Token (BGB). |
| INCH | 1inch (INCH). |
| CNY | Chinese Yuan (CNY). |
| BNFCR | Binance Credits (BNFCR). |
| LOT | LottoCoin. |

## [◆](https://docs.atas.net/en/)EcnId

| enum [ATAS.DataFeedsCore.EcnId](./namespaceATAS_1_1DataFeedsCore.md#a9fc02d2ce83b79e5c7e770bc2ebed394) |
| --- |

| Enumerator | |
| --- | --- |
| NONE | |
| NASDAQ | |
| NYSE | |
| SIAC | |
| ISI | |
| ISLAND | |
| ARCA | |
| ADFN | |
| BATS | |
| INCA | |
| NYOB | |
| MKXT | |
| CME | |
| ESSEX | |
| CHIC | |
| BRASS | |
| BOS | |
| SUMO | |
| INET | |
| BLZ | |
| EDGX | |
| CBOT | |
| IPE | |
| NYBOT | |
| USFE | |
| EUREX | |
| EDGA | |
| TSE | |
| AMEX | |
| CBOE | |
| PCX | |
| BOX | |
| ISE | |
| PHX | |
| CMECBT | |
| VSE | |
| PENSON | |
| OTC | |
| NSE | |
| MI | |
| CHX | |
| EDGE | |
| BATX | |
| BATY | |
| EDGD | |
| PHL | |
| ARCX | |
| NTRF | |
| IEX | |
| MEMX | |
| PEARL | |
| SMALL | |
| LTSE | |
| LSE | |
| UNKNOWN | |

## [◆](https://docs.atas.net/en/)EntityAction

| enum [ATAS.DataFeedsCore.EntityAction](./namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9e) |
| --- |

| Enumerator | |
| --- | --- |
| Logon | |
| Logout | |
| UserAdd | |
| UserChange | |
| UserRemove | |
| UserLock | |
| UserKick | |
| UserChangeExpiration | |
| UserGroupAdd | |
| UserGroupChange | |
| UserGroupRemove | |
| UserRoleAdd | |
| UserRoleChange | |
| UserRoleRemove | |
| ExchangeAdd | |
| ExchangeChange | |
| ExchangeRemove | |
| CommissionGroupAdd | |
| CommissionGroupChange | |
| CommissionGroupRemove | |
| SecurityMarginAdd | |
| SecurityMarginChange | |
| SecurityMarginRemove | |
| PortfolioAdd | |
| PortfolioChange | |
| PortfolioValue | |
| PortfolioLock | |
| OrderRegister | |
| OrderMove | |
| OrderCancel | |
| NewsSend | |
| ServerSettings | |
| RequestStatistics | |
| OrderCancelled | |
| OrderMatched | |
| ServerPnL | |
| PortfolioSuspend | |
| PortfolioViewerAdd | |
| ConnectorAdd | |
| ConnectorChange | |
| ConnectorRemove | |
| ConnectorEnable | |
| PortfolioViewerRemove | |
| TradeAllowed | |
| PortfolioReset | |
| GetUserChangeHistory | |
| GetPortfolioChangeHistory | |
| UserGroupSuspendPortfolios | |
| UserGroupReset | |

## [◆](https://docs.atas.net/en/)EntityType

| enum [ATAS.DataFeedsCore.EntityType](./namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) |
| --- |

Represents the types of entities used in the application.

| Enumerator | |
| --- | --- |
| Security | Represents a security entity. |
| SecurityMargin | Represents a security margin entity. |
| Portfolio | Represents a portfolio entity. |
| Position | Represents a position entity. |
| MarketDepth | Represents a market depth entity. |
| Trade | Represents a trade entity. |
| Order | Represents an order entity. |
| MyTrade | Represents a user's trade entity. |
| News | Represents a news entity. |
| UserRole | Represents a user role entity. |
| UserGroup | Represents a user group entity. |
| User | Represents a user entity. |
| UserChange | Represents a user change entity. |
| CommissionGroup | Represents a commission group entity. |
| WorkingTime | Represents a working time entity. |
| Exchange | Represents an exchange entity. |
| PortfolioChange | Represents a portfolio change entity. |
| TradingOptions | Represents trading options entity. |
| PortfolioState | Represents the state of a portfolio entity. |
| PositionState | Represents the state of a position entity. |
| Connector | Represents a connector entity. |
| MarketByOrder | Represents a Market by Order (MBO) item. |

## [◆](https://docs.atas.net/en/)MarketByOrderUpdateTypes

| enum [ATAS.DataFeedsCore.MarketByOrderUpdateTypes](./namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617) |
| --- |

Type of market by order update.

| Enumerator | |
| --- | --- |
| Snapshot | Indicates that the MBO data is from a cache and not from the real-time data stream. |
| New | Indicates that the MBO data represents a new order added to the order book of this instrument. |
| Change | Indicates that the MBO data represents a change to an existing order. |
| Delete | Indicates that the MBO data represents a deletion of an existing order. |

## [◆](https://docs.atas.net/en/)MarketDataDelayPeriods

| enum [ATAS.DataFeedsCore.MarketDataDelayPeriods](./namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) |
| --- |

| Enumerator | |
| --- | --- |
| Realtime | |
| Delayed15M | |

## [◆](https://docs.atas.net/en/)MarketDataType

| enum [ATAS.DataFeedsCore.MarketDataType](./namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4) |
| --- |

Specifies the type of market data.

| Enumerator | |
| --- | --- |
| Bid | Represents bid (buy) market data. |
| Ask | Represents ask (sell) market data. |
| Trade | Represents trade market data. |

## [◆](https://docs.atas.net/en/)MessageType

| enum [ATAS.DataFeedsCore.MessageType](./namespaceATAS_1_1DataFeedsCore.md#a9b5d2538699d679afdd312abbba1c334) |
| --- |

| Enumerator | |
| --- | --- |
| Warning | |
| Error | |
| Information | |

## [◆](https://docs.atas.net/en/)MissingDataCases

| enum [ATAS.DataFeedsCore.MissingDataCases](./namespaceATAS_1_1DataFeedsCore.md#a128d922ae42e2b3f81059bad4bca3b9c) |
| --- |

Describes what part of the HistoryMyTrade was incomplete.

| Enumerator | |
| --- | --- |
| None | The trade has complete data. |
| Opening | Missing the opening part of the trade. |
| Closing | Missing the closing part of the trade. |

## [◆](https://docs.atas.net/en/)NewsType

| enum [ATAS.DataFeedsCore.NewsType](./namespaceATAS_1_1DataFeedsCore.md#a6e7968f109c5e89f8e38eec40f049070) |
| --- |

| Enumerator | |
| --- | --- |
| Text | |
| Admin | |
| Risk | |

## [◆](https://docs.atas.net/en/)OptionSeriesType

| enum [ATAS.DataFeedsCore.OptionSeriesType](./namespaceATAS_1_1DataFeedsCore.md#a376361e6afba9d2fac18636e52502f09) |
| --- |

| Enumerator | |
| --- | --- |
| Regular | Regular (quarterly) options. |
| Weekly | Weekly options. |
| EndOfMonth | End of month options. |

## [◆](https://docs.atas.net/en/)OptionTypes

| enum [ATAS.DataFeedsCore.OptionTypes](./namespaceATAS_1_1DataFeedsCore.md#ae2cf0eba1a77bdf2094c56164e77bc29) |
| --- |

| Enumerator | |
| --- | --- |
| Put | |
| Call | |

## [◆](https://docs.atas.net/en/)OrderDirections

| enum [ATAS.DataFeedsCore.OrderDirections](./namespaceATAS_1_1DataFeedsCore.md#a69bb1e00b8165b84d6c31a85dd8fdb7c) |
| --- |

Specifies the direction of an order.

| Enumerator | |
| --- | --- |
| Buy | Indicates a "Buy" order direction. |
| Sell | Indicates a "Sell" order direction. |

## [◆](https://docs.atas.net/en/)OrderStates

| enum [ATAS.DataFeedsCore.OrderStates](./namespaceATAS_1_1DataFeedsCore.md#af7d44e82c8115dab7b27c5d8ce102716) |
| --- |

Represents the possible states of an order.

| Enumerator | |
| --- | --- |
| None | The order is in none state. |
| Active | The order is in active state, i.e., it is currently being processed or waiting to be executed. |
| Done | The order is in done state, i.e., it has been fully executed or processed successfully. |
| Failed | The order is in failed state, i.e., it could not be processed or executed successfully. |

## [◆](https://docs.atas.net/en/)OrderStatus

| enum [ATAS.DataFeedsCore.OrderStatus](./namespaceATAS_1_1DataFeedsCore.md#a66b4e891b490d55021aebd694f57284b) |
| --- |

Represents the possible status of an order.

| Enumerator | |
| --- | --- |
| None | The order has no specific status. |
| Placed | The order has been placed successfully. |
| Filled | The order has been fully filled (executed). |
| PartlyFilled | The order has been partly filled (partially executed). |
| Canceled | The order has been canceled. |

## [◆](https://docs.atas.net/en/)OrderTypes

| enum [ATAS.DataFeedsCore.OrderTypes](./namespaceATAS_1_1DataFeedsCore.md#a33b9ecf5eb2da7746db45305a9d673f8) |
| --- |

Specifies the type of an order.

| Enumerator | |
| --- | --- |
| Limit | Indicates a "Limit" order type. |
| Market | Indicates a "Market" order type. |
| Stop | Indicates a "StopMarket" order type. |
| StopLimit | Indicates a "StopLimit" order type. |
| Unknown | Indicates an unknown or unsupported order type. |

## [◆](https://docs.atas.net/en/)PnlPercentType

| enum [ATAS.DataFeedsCore.PnlPercentType](./namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87) |
| --- |

Represents the type of percentage calculation for profit and loss (PNL).

| Enumerator | |
| --- | --- |
| Portfolio | Percentage calculation based on the portfolio. |
| Margin | Percentage calculation based on the margin. |

## [◆](https://docs.atas.net/en/)PortfolioChangeType

| enum [ATAS.DataFeedsCore.PortfolioChangeType](./namespaceATAS_1_1DataFeedsCore.md#a8f50690870021cddb483a2dc2d76a230) |
| --- |

| Enumerator | |
| --- | --- |
| IncreaseBalance | |
| ReduceBalance | |
| ChangeLeverage | |
| OvernightSwap | |

## [◆](https://docs.atas.net/en/)PositionAveragePriceValueTypes

| enum [ATAS.DataFeedsCore.PositionAveragePriceValueTypes](./namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) |
| --- |

Represents different types of values for the average price of a position. The average price is the average cost of all the trades that make up a position.

| Enumerator | |
| --- | --- |
| None | No specific average price value is set. |
| ReceivedFromServer | The average price is received from the server. |
| CalculatedByAllTrades | The average price is calculated using all trades that contribute to the position. |
| CalculatedByPartTrades | The average price is calculated using only a part of the trades that contribute to the position. |
| NotCalculated | The average price is not calculated or set for the position. |

## [◆](https://docs.atas.net/en/)PriceUnitTypes

| enum [ATAS.DataFeedsCore.PriceUnitTypes](./namespaceATAS_1_1DataFeedsCore.md#a09683f11280f15a2fea8c886380f250c) |
| --- |

| Enumerator | |
| --- | --- |
| Price | |
| Tick | |
| Percent | |

## [◆](https://docs.atas.net/en/)RebateResult

| enum [ATAS.DataFeedsCore.RebateResult](./namespaceATAS_1_1DataFeedsCore.md#ace8c78210ca5b50f90165046c04f5e7a) |
| --- |

| Enumerator | |
| --- | --- |
| Valid | |
| WrongRefCode | |
| WrongBroker | |
| WrongBrokerRefCode | |

## [◆](https://docs.atas.net/en/)SecType

| enum [ATAS.DataFeedsCore.SecType](./namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfaf) |
| --- |

Represents the type of security or financial instrument.

| Enumerator | |
| --- | --- |
| Future | Represents a future contract. |
| Forex | Represents a forex (foreign exchange) pair. |
| Stock | Represents a stock. |
| Bitcoin | Represents any crypto security, not limited to Bitcoin only. |
| CryptoFutures | Represents a crypto futures contract. |
| Indexes | Represents indexes or index-based securities. |
| Option | Represents an option contract. |
| Cfd | Represents a Contract for Difference (CFD) instrument. |

## [◆](https://docs.atas.net/en/)SubscriptionType

| enum [ATAS.DataFeedsCore.SubscriptionType](./namespaceATAS_1_1DataFeedsCore.md#ace84c9b086de28fb48ad66a822b297f3) |
| --- |

| Enumerator | |
| --- | --- |
| None | None. |
| Prints | Single ticks. |
| Best | Best bid/ask prices. |
| Quotes | Depth of Market (DOM) |
| MarketByOrder | Market by Order (MBO) |
| Summary | Summary (OHLCV and etc.) |

## [◆](https://docs.atas.net/en/)TimeInForce

| enum [ATAS.DataFeedsCore.TimeInForce](./namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) |
| --- |

Specifies the time in force options for an order.

| Enumerator | |
| --- | --- |
| None | No specific time in force specified. |
| GoodTillCancel | Order will be active until filled completely or cancelled by the user. |
| FillOrKill | Order will either fill completely or will be cancelled if not fully filled. |
| ImmediateOrCancel | Order may be filled completely or partially; if partially filled, the unfilled part will be cancelled. |
| Day | Order is valid only for the current trading session and will be cancelled at the end of the trading day. |
| Default | The default time in force options supported by connectors. |

## [◆](https://docs.atas.net/en/)TPlusLimits

| enum [ATAS.DataFeedsCore.TPlusLimits](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793) |
| --- |

Represents different T+ (settlement) periods for trading. T+ refers to the number of days it takes for a trade to settle and for the funds and securities to be exchanged between the parties involved in the transaction.

| Enumerator | |
| --- | --- |
| T0 | Same-day settlement (T0). The trade is settled on the same day it occurs. |
| T1 | One-day settlement (T1). The trade is settled one business day after the trade date. |
| T2 | Two-day settlement (T2). The trade is settled two business days after the trade date. |
| Tx | Custom settlement period (Tx). The trade is settled after a custom number of days (365 days in this case). |

## [◆](https://docs.atas.net/en/)TradeDirection

| enum [ATAS.DataFeedsCore.TradeDirection](./namespaceATAS_1_1DataFeedsCore.md#aba12e40f5e9dc50ae6d63a745405fd6b) |
| --- |

Represents the trade direction.

| Enumerator | |
| --- | --- |
| Buy | Indicates a buy trade direction. |
| Sell | Indicates a sell trade direction. |
| Between | Indicates a trade direction that is neither buy nor sell. |

## [◆](https://docs.atas.net/en/)TriggerPriceType

| enum [ATAS.DataFeedsCore.TriggerPriceType](./namespaceATAS_1_1DataFeedsCore.md#a40608542bbcd7fa29063f394baf3f64d) |
| --- |

Defines price source for conditional order trigger.

| Enumerator | |
| --- | --- |
| None | |
| Last | Order will trigger based on last tick on the exchange. |
| Index | Index price is calculated based on several exchanges price with their weight
 Implementation may vary on exchange. |
| Mark | Estimated fair value of a contract
 Implementation may vary on exchange. |

## [◆](https://docs.atas.net/en/)UserChangeTypes

| enum [ATAS.DataFeedsCore.UserChangeTypes](./namespaceATAS_1_1DataFeedsCore.md#aebead1edeaed1f3fe90cd31ec41152bc) |
| --- |

| Enumerator | |
| --- | --- |
| ExtendExpirationPeriod | |
| ReduceExpirationPeriod | |

## [◆](https://docs.atas.net/en/)VolumeUnitTypes

| enum [ATAS.DataFeedsCore.VolumeUnitTypes](./namespaceATAS_1_1DataFeedsCore.md#a78a8da757284f557e5f415d903976283) |
| --- |

| Enumerator | |
| --- | --- |
| Absolute | |
| Percent | |

## Function Documentation

## [◆](https://docs.atas.net/en/)ConnectorEventHandler()

| delegate void ATAS.DataFeedsCore.ConnectorEventHandler | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ConnectorEventHandler()

| delegate void [ATAS.DataFeedsCore.ConnectorEventHandler](./namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | T | arg |
| | ) | | |

## [◆](https://docs.atas.net/en/)ConnectorEventHandler()

| delegate void [ATAS.DataFeedsCore.ConnectorEventHandler](./namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | [T1](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793ace499dea30cfce118f4fe85da0227e83) | arg1, |
| | | [T2](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793a71d2c46af01feeea54a0f541243e297b) | arg2 |
| | ) | | |

## [◆](https://docs.atas.net/en/)ConnectorEventHandler()

| delegate void [ATAS.DataFeedsCore.ConnectorEventHandler](./namespaceATAS_1_1DataFeedsCore.md#a1ad6324b10ffddc1786d6b2850310b73) | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | [T1](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793ace499dea30cfce118f4fe85da0227e83) | arg1, |
| | | [T2](./namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793a71d2c46af01feeea54a0f541243e297b) | arg2, |
| | | T3 | arg3 |
| | ) | | |

## [◆](https://docs.atas.net/en/)CurrencyInfo()

| record struct ATAS.DataFeedsCore.CurrencyInfo | ( | string | Code, |
| --- | --- | --- | --- |
| | | bool | IsBase, |
| | | int | Precision |
| | ) | | |

Represents currency information for a decimal value.

Parameters

| Code | Currency code |
| --- | --- |
| IsBase | True- Currency is base for security |
| Precision | Number of numbers after the decimal point |

Get display format for decimal value

Get edit format for decimal value

## Variable Documentation

## [◆](https://docs.atas.net/en/)IOrderOptionPostOnly

| record [ATAS.DataFeedsCore.IOrderOptionPostOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionPostOnly.md) |
| --- |

## [◆](https://docs.atas.net/en/)IOrderOptionReduceOnly

| record [ATAS.DataFeedsCore.IOrderOptionReduceOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IOrderOptionReduceOnly.md) |
| --- |

## [◆](https://docs.atas.net/en/)OrderExtendedOptionsFlags

| record ATAS.DataFeedsCore.OrderExtendedOptionsFlags |
| --- |

Represents extended order options stored as a set of flag values for quick serialization and comparison.
