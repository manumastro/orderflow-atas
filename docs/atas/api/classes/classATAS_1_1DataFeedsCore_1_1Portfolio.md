# ATAS.DataFeedsCore.Portfolio Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Portfolio.html

Represents a portfolio entity with various properties related to account balance, Profit and Loss (PnL), permissions, trading options, and more.
 [More...](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Portfolio:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Portfolio:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#aa12e73b6c0bb26d5bcadae4e1115b1c6) () |
| | Initializes a new instance of the Portfolio class with default values. |
| | |
| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | [Clone](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a291c761e85e1506b596cee4122b56c35) () |
| | Creates a new instance of the Portfolio class that is a shallow copy of the current instance. |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#ac14ab04890eefbfeb86ca4be5a3a5c38) () |
| | Returns a string representation of the portfolio group, including various account-related information. |
| | |

| Protected Member Functions | |
| --- | --- |
| void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a4f0fea8a548376c9b4953925c77b86b7) (string name) |
| | Raises the PropertyChanged event with the specified property name. |
| | |

| Properties | |
| --- | --- |
| string | [AccountID](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#aa26fbb200b0bc24b2de6b2cd0ca02515)`[get, set]` |
| | |
| string | [DepoName](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a21af07985378de15b9cfd97371cb0575)`[get, set]` |
| | Gets or sets the name of the deposit for the portfolio. |
| | |
| [Currencies](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9)? | [Currency](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#af1c34d0ae8e7ced1a165ccf21d1330d8)`[get, set]` |
| | Gets or sets the currency associated with the portfolio. |
| | |
| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? | [TPlusLimit](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#ab095892ee6ca63cc9db982764c3996ed)`[get, set]` |
| | Gets or sets the T+ limits for the portfolio. |
| | |
| ConnectionStates? | [ConnectionState](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a335db0d71b90b33ae9019893dc6e82f3)`[get, set]` |
| | Gets or sets the connection state of the portfolio. |
| | |
| decimal | [Balance](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a6bdf2722c4917c8b2bd373923d2ed7b0)`[get, set]` |
| | Gets or sets the total available funds that the user can use right now to make an order. |
| | |
| decimal | [BlockedMargin](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a94b82af0bffb8439037bd37e586b27a3)`[get, set]` |
| | Gets or sets the amount of funds that are blocked by current open positions and not included in the BalanceAvailable. |
| | |
| decimal | [Leverage](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a71257c7ec920b05309154516463ece6a)`[get, set]` |
| | Gets or sets the leverage of the portfolio. |
| | |
| decimal | [BalancePower](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a60ee7477f2a5ea49799c2ea6a9e5c765)`[get, set]` |
| | Gets or sets the balance according to the leverage (on leverage x2, it will be two times bigger). |
| | |
| decimal? | [BalanceAvailable](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#af10e28666217411706b29962ca6c53d8)`[get, set]` |
| | Gets or sets the balance excluding the BlockedMargin. |
| | |
| decimal | [OpenPnL](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#aedef7dd5a8d604d50a6074bfb35c5b5e)`[get, set]` |
| | Gets or sets the opened Profit and Loss (PnL) of the portfolio. The opened PnL represents the unrealized gains or losses from currently open positions. |
| | |
| decimal | [ClosedPnL](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a3f968e10859c07511b699fafa1950eef)`[get, set]` |
| | Gets or sets the closed Profit and Loss (PnL) of the portfolio. The closed PnL represents the realized gains or losses from closed positions. |
| | |
| decimal | [TotalClosedPnL](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a3a55e059cd04d9750714820a136b72b1)`[get, set]` |
| | Gets or sets the total closed Profit and Loss (PnL) of the portfolio. The total closed PnL is the cumulative realized gains or losses from all closed positions. |
| | |
| decimal | [TotalPnL](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#abb9bcfcfbb66fb08a2ff46fc7434c322)`[get]` |
| | Gets the total Profit and Loss (PnL) of the portfolio. The total PnL is the sum of both the opened PnL and the total closed PnL. |
| | |
| decimal | [MaxEquityValue](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#aef5475320210e3438f2c4e4f2517aba1)`[get, set]` |
| | Gets or sets the maximum equity value of the portfolio. The maximum equity value represents the highest value of the portfolio's equity over time. |
| | |
| bool | [IsLocked](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#ad5536b861b37efab2a8b94c818e0d390)`[get, set]` |
| | |
| bool | [IsSuspended](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#aa0e8cddf262955ea2c45fae68d25d1fb)`[get, set]` |
| | Gets or sets a value indicating whether the trading options are suspended. |
| | |
| long? | [TradingOptionsId](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a68cedbf1f48bb1454b98f128419a2777)`[get, set]` |
| | Gets or sets the ID of the associated trading options for the portfolio. |
| | |
| [TradingOptions](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md) | [TradingOptions](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a04154dd05c5eadd41708e458d10f6801)`[get, set]` |
| | |
| [CommissionRulesGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRulesGroup.md) | [CommissionRulesGroup](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a60c507747feac70855c8557440305b6e)`[get, set]` |
| | Gets or sets the commission rules group associated with the portfolio. The commission rules group is represented by an object of the CommissionRulesGroup class. This property is not visible in the user interface (Browsable(false)). |
| | |
| string | [CommissionState](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a78686c93590de3dc3a3e5f0a995ee3d5)`[get, set]` |
| | Gets or sets the state of the commission for the portfolio. |
| | |
| decimal | [Commission](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a9273a4192ec917c7106a08e84ed40f52)`[get, set]` |
| | Gets or sets the commission value for the portfolio. |
| | |
| DateTime | [ClosedPnlDate](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#af9c6a1012ace80d928891061a9ccbf52)`[get, set]` |
| | Gets or sets the date of the closed profit and loss (PnL). |
| | |
| object | [Data](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a01722e15b6bed64bf1664e92bc2801cb)`[get, set]` |
| | Gets or sets additional data associated with the portfolio. |
| | |
| [IPortfolioExtendedInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md)? | [ExtendedInfo](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#abc6782469b35cf4a260efcdd14d7f802)`[get, set]` |
| | Gets or sets the extended portfolio information specific to the connector. Each connector can provide its own implementation with unique parameters. |
| | |
| bool | [IsAdviserPortfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a8b91ea52c22b1d72fd38a18036e68a5b)`[get, set]` |
| | Gets or sets a flag indicating whether the portfolio is associated with an adviser portfolio. |
| | |
| string | [FcmId](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#abf708b863009e7e1ee9ffcb4701e275a)`[get, set]` |
| | Gets or sets the FCM ID (Futures Commission Merchant ID) associated with the portfolio. |
| | |
| string | [IbId](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a77fb8a8f4b20c8afaa245b8be6ee506f)`[get, set]` |
| | Gets or sets the IB ID (Interactive Brokers ID) associated with the portfolio. |
| | |
| int | [ActiveOrders](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a98fafd7d0325cc13f0052da11635fdbb)`[get, set]` |
| | Gets or sets the number of active orders associated with the portfolio. |
| | |
| int | [AtasId](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a971ebeb1454177cc295a975984d9b51b)`[get, set]` |
| | Gets or sets the [ATAS](../namespaces/namespaceATAS.md) ID associated with the portfolio. |
| | |
| bool | [IsRealAccount](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a37ad178b2c36d67ae7fe9cb9548bb38e)`[get, set]` |
| | Gets or sets a flag indicating whether the portfolio is associated with a real account. |
| | |
| long | [UserId](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#ac5fd7628a8eca56acc1524871ea752b6)`[get, set]` |
| | Gets or sets the ID of the user associated with the portfolio. |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md) | [User](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#af49ecfb7dff3f91f774149cc272292ac)`[get, set]` |
| | Gets or sets the user associated with the portfolio. |
| | |
| List | [Viewers](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#aada659922ea47d89441968ccbcfbb222)`[get]` |
| | Gets the list of portfolio viewers associated with the portfolio. |
| | |
| List | [Accounts](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a74ea1920550b578b9d1d41d443dbbb57)`[get]` |
| | Gets a list of portfolio groups associated with the portfolio. |
| | |
| DateTime? | [ProcessedTradeTime](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a5cfea6c6244af4c47d64c76d0235d7d6)`[get, set]` |
| | Gets or sets the date and time when the trades for the portfolio were last processed. |
| | |
| string? | [StatisticsUrl](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a401c824486fbf964d31cbc93667a1273)`[get, set]` |
| | Url to get trading statistics for this account. |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#afe43576c4a76cea3b587f2ac19dae745)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Portfolio.md#a044f9836b40082ba40210e617cdd0868) |
| | |

## Detailed Description

Represents a portfolio entity with various properties related to account balance, Profit and Loss (PnL), permissions, trading options, and more.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Portfolio()

| ATAS.DataFeedsCore.Portfolio.Portfolio | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the Portfolio class with default values.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Clone()

| [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) ATAS.DataFeedsCore.Portfolio.Clone | ( | | ) | |
| --- | --- | --- | --- | --- |

Creates a new instance of the Portfolio class that is a shallow copy of the current instance.

ReturnsA shallow copy of the current instance.

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| void ATAS.DataFeedsCore.Portfolio.OnPropertyChanged | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PropertyChanged event with the specified property name.

Parameters

| name | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.Portfolio.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string representation of the portfolio group, including various account-related information.

ReturnsA string representation of the portfolio group.

## Property Documentation

## [◆](https://docs.atas.net/en/)AccountID

| string ATAS.DataFeedsCore.Portfolio.AccountID |
| --- |

getset

Gets or sets the account ID associated with the portfolio.

## [◆](https://docs.atas.net/en/)Accounts

| List ATAS.DataFeedsCore.Portfolio.Accounts |
| --- |

get

Gets a list of portfolio groups associated with the portfolio.

## [◆](https://docs.atas.net/en/)ActiveOrders

| int ATAS.DataFeedsCore.Portfolio.ActiveOrders |
| --- |

getset

Gets or sets the number of active orders associated with the portfolio.

## [◆](https://docs.atas.net/en/)AtasId

| int ATAS.DataFeedsCore.Portfolio.AtasId |
| --- |

getset

Gets or sets the [ATAS](../namespaces/namespaceATAS.md) ID associated with the portfolio.

## [◆](https://docs.atas.net/en/)Balance

| decimal ATAS.DataFeedsCore.Portfolio.Balance |
| --- |

getset

Gets or sets the total available funds that the user can use right now to make an order.

It includes the blocked margin. Represents the gross balance of the portfolio.

## [◆](https://docs.atas.net/en/)BalanceAvailable

| decimal? ATAS.DataFeedsCore.Portfolio.BalanceAvailable |
| --- |

getset

Gets or sets the balance excluding the BlockedMargin.

It is the amount of money that the user can use to open a new order right now.

## [◆](https://docs.atas.net/en/)BalancePower

| decimal ATAS.DataFeedsCore.Portfolio.BalancePower |
| --- |

getset

Gets or sets the balance according to the leverage (on leverage x2, it will be two times bigger).

Use 0 when leverage is not applicable to the whole portfolio.

## [◆](https://docs.atas.net/en/)BlockedMargin

| decimal ATAS.DataFeedsCore.Portfolio.BlockedMargin |
| --- |

getset

Gets or sets the amount of funds that are blocked by current open positions and not included in the BalanceAvailable.

## [◆](https://docs.atas.net/en/)ClosedPnL

| decimal ATAS.DataFeedsCore.Portfolio.ClosedPnL |
| --- |

getset

Gets or sets the closed Profit and Loss (PnL) of the portfolio. The closed PnL represents the realized gains or losses from closed positions.

## [◆](https://docs.atas.net/en/)ClosedPnlDate

| DateTime ATAS.DataFeedsCore.Portfolio.ClosedPnlDate |
| --- |

getset

Gets or sets the date of the closed profit and loss (PnL).

## [◆](https://docs.atas.net/en/)Commission

| decimal ATAS.DataFeedsCore.Portfolio.Commission |
| --- |

getset

Gets or sets the commission value for the portfolio.

## [◆](https://docs.atas.net/en/)CommissionRulesGroup

| [CommissionRulesGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionRulesGroup.md) ATAS.DataFeedsCore.Portfolio.CommissionRulesGroup |
| --- |

getset

Gets or sets the commission rules group associated with the portfolio. The commission rules group is represented by an object of the CommissionRulesGroup class. This property is not visible in the user interface (Browsable(false)).

## [◆](https://docs.atas.net/en/)CommissionState

| string ATAS.DataFeedsCore.Portfolio.CommissionState |
| --- |

getset

Gets or sets the state of the commission for the portfolio.

## [◆](https://docs.atas.net/en/)ConnectionState

| ConnectionStates? ATAS.DataFeedsCore.Portfolio.ConnectionState |
| --- |

getset

Gets or sets the connection state of the portfolio.

## [◆](https://docs.atas.net/en/)Currency

| [Currencies](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a3c64c31df3315852a6d4a5d519d2e5e9)? ATAS.DataFeedsCore.Portfolio.Currency |
| --- |

getset

Gets or sets the currency associated with the portfolio.

## [◆](https://docs.atas.net/en/)Data

| object ATAS.DataFeedsCore.Portfolio.Data |
| --- |

getset

Gets or sets additional data associated with the portfolio.

## [◆](https://docs.atas.net/en/)DepoName

| string ATAS.DataFeedsCore.Portfolio.DepoName |
| --- |

getset

Gets or sets the name of the deposit for the portfolio.

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.Portfolio.EntityType |
| --- |

get

Gets the entity type, which is EntityType.Portfolio.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)ExtendedInfo

| [IPortfolioExtendedInfo](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPortfolioExtendedInfo.md)? ATAS.DataFeedsCore.Portfolio.ExtendedInfo |
| --- |

getset

Gets or sets the extended portfolio information specific to the connector. Each connector can provide its own implementation with unique parameters.

## [◆](https://docs.atas.net/en/)FcmId

| string ATAS.DataFeedsCore.Portfolio.FcmId |
| --- |

getset

Gets or sets the FCM ID (Futures Commission Merchant ID) associated with the portfolio.

## [◆](https://docs.atas.net/en/)IbId

| string ATAS.DataFeedsCore.Portfolio.IbId |
| --- |

getset

Gets or sets the IB ID (Interactive Brokers ID) associated with the portfolio.

## [◆](https://docs.atas.net/en/)IsAdviserPortfolio

| bool ATAS.DataFeedsCore.Portfolio.IsAdviserPortfolio |
| --- |

getset

Gets or sets a flag indicating whether the portfolio is associated with an adviser portfolio.

## [◆](https://docs.atas.net/en/)IsLocked

| bool ATAS.DataFeedsCore.Portfolio.IsLocked |
| --- |

getset

Gets or sets a value indicating whether the trading options are locked.

## [◆](https://docs.atas.net/en/)IsRealAccount

| bool ATAS.DataFeedsCore.Portfolio.IsRealAccount |
| --- |

getset

Gets or sets a flag indicating whether the portfolio is associated with a real account.

## [◆](https://docs.atas.net/en/)IsSuspended

| bool ATAS.DataFeedsCore.Portfolio.IsSuspended |
| --- |

getset

Gets or sets a value indicating whether the trading options are suspended.

## [◆](https://docs.atas.net/en/)Leverage

| decimal ATAS.DataFeedsCore.Portfolio.Leverage |
| --- |

getset

Gets or sets the leverage of the portfolio.

## [◆](https://docs.atas.net/en/)MaxEquityValue

| decimal ATAS.DataFeedsCore.Portfolio.MaxEquityValue |
| --- |

getset

Gets or sets the maximum equity value of the portfolio. The maximum equity value represents the highest value of the portfolio's equity over time.

## [◆](https://docs.atas.net/en/)OpenPnL

| decimal ATAS.DataFeedsCore.Portfolio.OpenPnL |
| --- |

getset

Gets or sets the opened Profit and Loss (PnL) of the portfolio. The opened PnL represents the unrealized gains or losses from currently open positions.

## [◆](https://docs.atas.net/en/)ProcessedTradeTime

| DateTime? ATAS.DataFeedsCore.Portfolio.ProcessedTradeTime |
| --- |

getset

Gets or sets the date and time when the trades for the portfolio were last processed.

## [◆](https://docs.atas.net/en/)StatisticsUrl

| string? ATAS.DataFeedsCore.Portfolio.StatisticsUrl |
| --- |

getset

Url to get trading statistics for this account.

## [◆](https://docs.atas.net/en/)TotalClosedPnL

| decimal ATAS.DataFeedsCore.Portfolio.TotalClosedPnL |
| --- |

getset

Gets or sets the total closed Profit and Loss (PnL) of the portfolio. The total closed PnL is the cumulative realized gains or losses from all closed positions.

## [◆](https://docs.atas.net/en/)TotalPnL

| decimal ATAS.DataFeedsCore.Portfolio.TotalPnL |
| --- |

get

Gets the total Profit and Loss (PnL) of the portfolio. The total PnL is the sum of both the opened PnL and the total closed PnL.

## [◆](https://docs.atas.net/en/)TPlusLimit

| [TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793)? ATAS.DataFeedsCore.Portfolio.TPlusLimit |
| --- |

getset

Gets or sets the T+ limits for the portfolio.

## [◆](https://docs.atas.net/en/)TradingOptions

| [TradingOptions](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md) ATAS.DataFeedsCore.Portfolio.TradingOptions |
| --- |

getset

Gets or sets the trading options associated with the portfolio.

## [◆](https://docs.atas.net/en/)TradingOptionsId

| long? ATAS.DataFeedsCore.Portfolio.TradingOptionsId |
| --- |

getset

Gets or sets the ID of the associated trading options for the portfolio.

## [◆](https://docs.atas.net/en/)User

| [User](./classATAS_1_1DataFeedsCore_1_1User.md) ATAS.DataFeedsCore.Portfolio.User |
| --- |

getset

Gets or sets the user associated with the portfolio.

## [◆](https://docs.atas.net/en/)UserId

| long ATAS.DataFeedsCore.Portfolio.UserId |
| --- |

getset

Gets or sets the ID of the user associated with the portfolio.

## [◆](https://docs.atas.net/en/)Viewers

| List ATAS.DataFeedsCore.Portfolio.Viewers |
| --- |

get

Gets the list of portfolio viewers associated with the portfolio.

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.Portfolio.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [Portfolio.cs](../files/Portfolio_8cs.md)
