# ATAS.DataFeedsCore.Security Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Security.html

Represents a security entity used in the application.
 [More...](./classATAS_1_1DataFeedsCore_1_1Security.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Security:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Security:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md#a106aab3606e346f74ce4e7bb3e660494) () |
| | Initializes a new instance of the Security class with default values. |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1Security.md#a4ee8f73c0df8054a7578ab47f589ca93) () |
| | Returns a string representation of the security, using either the instrument name or the combination of code and exchange. |
| | |

| Protected Member Functions | |
| --- | --- |
| void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1Security.md#a193fdb576167e4037228e8d244aefc94) (string name) |
| | Raises the PropertyChanged event with the specified property name. |
| | |

| Properties | |
| --- | --- |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1Security.md#a403869e98943377839c198cfcfcd12ef)`[get]` |
| | |
| string | [SecurityId](./classATAS_1_1DataFeedsCore_1_1Security.md#a1dfefc790c3dfd506068c848bdae39f3)`[get, set]` |
| | Global [ATAS](../namespaces/namespaceATAS.md) security identifier like `BCHUSDT@Bybit`. Includes exchange id. |
| | |
| string | [ConnectorId](./classATAS_1_1DataFeedsCore_1_1Security.md#a341afe3b422f27ca69b465bf9c952825)`[get, set]` |
| | Gets or sets the connector identifier associated with the security. |
| | |
| string | [Code](./classATAS_1_1DataFeedsCore_1_1Security.md#a1270123ef753defa83d8bc14cdf87bb6)`[get, set]` |
| | Id of the security
 May have any format like `CLK4` or `BCHUSDT` etc. |
| | |
| string | [Exchange](./classATAS_1_1DataFeedsCore_1_1Security.md#af0687d502e63448c669d4cd2383ffd08)`[get, set]` |
| | Gets or sets the exchange associated with the security. |
| | |
| string | [Instrument](./classATAS_1_1DataFeedsCore_1_1Security.md#ad38d0e374135778214f18264a4de8b8e)`[get, set]` |
| | Gets or sets the instrument associated with the security. |
| | |
| DateTime | [Expiration](./classATAS_1_1DataFeedsCore_1_1Security.md#ad5e40c06f1d2f07851272094c1928ad4)`[get, set]` |
| | Gets or sets the expiration date of the security. |
| | |
| [SecType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfaf) | [Type](./classATAS_1_1DataFeedsCore_1_1Security.md#a6bbd12e17607d11369542c79f7a3bdb4)`[get, set]` |
| | Gets or sets the type of security or financial instrument. |
| | |
| decimal | [TickSize](./classATAS_1_1DataFeedsCore_1_1Security.md#a8f52984e4c0015c5b5e708f94872ea00)`[get, set]` |
| | Gets or sets the minimum price increment (tick size) for the security. |
| | |
| decimal | [TickCost](./classATAS_1_1DataFeedsCore_1_1Security.md#a774834653ff6698159533ef94b9c907d)`[get, set]` |
| | Price of a single tickSize. |
| | |
| decimal | [LotSize](./classATAS_1_1DataFeedsCore_1_1Security.md#a389d11bafc0097ae1c1973bf2731ddfa)`[get, set]` |
| | Minimum volume increment. |
| | |
| decimal? | [LotMinSize](./classATAS_1_1DataFeedsCore_1_1Security.md#a59ad8e9773f36a0627e435612cbf8f49)`[get, set]` |
| | Minimum size (volume) for the order. |
| | |
| decimal? | [LotMaxSize](./classATAS_1_1DataFeedsCore_1_1Security.md#a79f11f3cefb02db3e7a39da41bfb4941)`[get, set]` |
| | Maximum size (volume) for the order. |
| | |
| int | [Digits](./classATAS_1_1DataFeedsCore_1_1Security.md#afca084690d536b82d0d0e57bf463c635)`[get]` |
| | Gets the number of decimal digits used for formatting prices. |
| | |
| decimal | [MinPrice](./classATAS_1_1DataFeedsCore_1_1Security.md#a3d9641b79bf277a449388802f43479c2)`[get, set]` |
| | Gets or sets the minimum price allowed for the security. |
| | |
| decimal | [MaxPrice](./classATAS_1_1DataFeedsCore_1_1Security.md#afd7448e504847b01b50405e38e50d93a)`[get, set]` |
| | Gets or sets the maximum price allowed for the security. |
| | |
| decimal | [MarginBuy](./classATAS_1_1DataFeedsCore_1_1Security.md#a0cf543855529313d02eb7cb7ca2e1789)`[get, set]` |
| | Gets or sets the margin buy value for the security. |
| | |
| decimal | [MarginSell](./classATAS_1_1DataFeedsCore_1_1Security.md#a2f2e946567634a1638d121679f6ebf0b)`[get, set]` |
| | Gets or sets the margin sell value for the security. |
| | |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1Security.md#a4f73381874e265f31919d95b88777b3f)`[get, set]` |
| | Gets or sets the identifier of the security. |
| | |
| long | [IsinId](./classATAS_1_1DataFeedsCore_1_1Security.md#aa5e02034dcf93545ddd56681eb0b7b5f)`[get, set]` |
| | Gets or sets the International Securities Identification Number (ISIN) identifier of the security. |
| | |
| decimal | [BestAskPrice](./classATAS_1_1DataFeedsCore_1_1Security.md#a94ca53639f29b085ddf2e8404cee2b20)`[get, set]` |
| | Gets or sets the best asking price for the security. |
| | |
| decimal | [BestAskVolume](./classATAS_1_1DataFeedsCore_1_1Security.md#a6ebadac0077cdffa04ced4436a10b057)`[get, set]` |
| | Gets or sets the volume available at the best asking price for the security. |
| | |
| decimal | [BestBidPrice](./classATAS_1_1DataFeedsCore_1_1Security.md#a51ea6565d42af03655ead6380181af95)`[get, set]` |
| | Gets or sets the best bidding price for the security. |
| | |
| decimal | [BestBidVolume](./classATAS_1_1DataFeedsCore_1_1Security.md#a1ec74f4203b1c9003bf8aa0935234c03)`[get, set]` |
| | Gets or sets the volume available at the best bidding price for the security. |
| | |
| decimal? | [LastTradePrice](./classATAS_1_1DataFeedsCore_1_1Security.md#ad4f46851141188ab79734bec72326cbd)`[get, set]` |
| | Price of the last tick
 This value may be null if no ticks were received yet. |
| | |
| decimal? | [LastTradeVolume](./classATAS_1_1DataFeedsCore_1_1Security.md#a9326dd4c031c73a1d277ff52dcf72509)`[get, set]` |
| | Volume of the last tick
 This value may be null if no ticks were received yet. |
| | |
| decimal | [PriceMultiplier](./classATAS_1_1DataFeedsCore_1_1Security.md#aada32f09130c25ac71bbbca8aa1f9be6)`[get, set]` |
| | Gets or sets the price multiplier for the security. |
| | |
| decimal | [VolumeMultiplier](./classATAS_1_1DataFeedsCore_1_1Security.md#a69c93b0f5994219c4a68337f148c5ba5)`[get, set]` |
| | Gets or sets the volume multiplier for the security. |
| | |
| decimal? | [OpenInterest](./classATAS_1_1DataFeedsCore_1_1Security.md#a25ffe586447fb7044bc5cc9d468999a4)`[get, set]` |
| | Gets or sets the open interest value for the security. |
| | |
| decimal? | [MarkPrice](./classATAS_1_1DataFeedsCore_1_1Security.md#a4f3087b60b0332ee7e60852aa0f39750)`[get, set]` |
| | A price that reflects the real-time spot price on the major exchanges. |
| | |
| decimal? | [FundingRate](./classATAS_1_1DataFeedsCore_1_1Security.md#a924f4a7c4f5f276f98bff5ad9c846d13)`[get, set]` |
| | Gets funding rate exchanged between buyers and sellers. During the funding rate cycle. |
| | |
| DateTimeOffset? | [NextFundingTime](./classATAS_1_1DataFeedsCore_1_1Security.md#a95d06141f0a1ba674deaf48b73dcc0b0)`[get, set]` |
| | Gets time of the next funding cycle. |
| | |
| string? | [BaseCurrency](./classATAS_1_1DataFeedsCore_1_1Security.md#a7372e8abae0bcd34c1ffe170108aabe5)`[get, set]` |
| | First currency of the trading pair, order volume must be sent in this currency. |
| | |
| string? | [QuoteCurrency](./classATAS_1_1DataFeedsCore_1_1Security.md#ad756ffc7500ab455031f49882ab444d5)`[get, set]` |
| | Second currency of the trading pair, if set you can use it for convert operations. |
| | |
| decimal? | [QuoteCurrencyPrecision](./classATAS_1_1DataFeedsCore_1_1Security.md#a6281b107695ca06c4f8cbb5884d3afb6)`[get, set]` |
| | Minimum step for quote currency (ex: 0.01 for USD, 0.00000001 for BTC). |
| | |
| string | [MoneyPnLFormat](./classATAS_1_1DataFeedsCore_1_1Security.md#a959e36560b044669c9a391d0ff7e61bb) = _defaultMoneyPnLFormat`[get, set]` |
| | Gets or sets money PnL format for security. |
| | |
| object? | [Parent](./classATAS_1_1DataFeedsCore_1_1Security.md#a9b77ed0a91966837ef3ccda5ede1dd3b)`[get, set]` |
| | Gets or sets connector entity the security was created from. |
| | |
| bool | [IsInverseFutures](./classATAS_1_1DataFeedsCore_1_1Security.md#a00ce234b0229229716a147062b9312f5)`[get, set]` |
| | Security is an inverse futures. |
| | |
| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md)? | [ExchangeInstance](./classATAS_1_1DataFeedsCore_1_1Security.md#a508f1771052b3df9d6958746730972c9)`[get, set]` |
| | The object that represents the exchange where the security is listed. |
| | |
| decimal? | [StrikePrice](./classATAS_1_1DataFeedsCore_1_1Security.md#aca172decbc1fc820ad3e0fd7ab92c7b9)`[get, set]` |
| | A strike price of option contract. |
| | |
| [OptionTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ae2cf0eba1a77bdf2094c56164e77bc29)? | [OptionType](./classATAS_1_1DataFeedsCore_1_1Security.md#a79858b485266ddb0c600fe8eb7e930a0)`[get, set]` |
| | Gets or sets the type of option contract. |
| | |
| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md)? | [UnderlyingSecurity](./classATAS_1_1DataFeedsCore_1_1Security.md#aa12ccdf267969edfa45dce11409e03dd)`[get, set]` |
| | An underlying security of option contract. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md) | |
| decimal | [TickSize](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a7c167afad451a5064a32e33b81c7daf7)`[get]` |
| | |
| decimal | [TickCost](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#aea3427ed2399feae0aa94ecb63dec218)`[get]` |
| | |
| int | [Digits](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#aaa7f7f5a8c11b73085c20cdea17bfbb4)`[get]` |
| | |
| string | [Code](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a6b237e5f3f1830d15e2856bbd887ff59)`[get]` |
| | |
| string | [Exchange](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a9d33afad63385cd6d32e15ce32ee22be)`[get]` |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1Security.md#ae518cce0a9bd2abe5c41210bdc6f1199) |
| | |

## Detailed Description

Represents a security entity used in the application.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)Security()

| ATAS.DataFeedsCore.Security.Security | ( | | ) | |
| --- | --- | --- | --- | --- |

Initializes a new instance of the Security class with default values.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| void ATAS.DataFeedsCore.Security.OnPropertyChanged | ( | string | name | ) | |
| --- | --- | --- | --- | --- | --- |

protected

Raises the PropertyChanged event with the specified property name.

Parameters

| name | The name of the property that changed. |
| --- | --- |

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.Security.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string representation of the security, using either the instrument name or the combination of code and exchange.

ReturnsA string representation of the security.

## Property Documentation

## [◆](https://docs.atas.net/en/)BaseCurrency

| string? ATAS.DataFeedsCore.Security.BaseCurrency |
| --- |

getset

First currency of the trading pair, order volume must be sent in this currency.

## [◆](https://docs.atas.net/en/)BestAskPrice

| decimal ATAS.DataFeedsCore.Security.BestAskPrice |
| --- |

getset

Gets or sets the best asking price for the security.

## [◆](https://docs.atas.net/en/)BestAskVolume

| decimal ATAS.DataFeedsCore.Security.BestAskVolume |
| --- |

getset

Gets or sets the volume available at the best asking price for the security.

## [◆](https://docs.atas.net/en/)BestBidPrice

| decimal ATAS.DataFeedsCore.Security.BestBidPrice |
| --- |

getset

Gets or sets the best bidding price for the security.

## [◆](https://docs.atas.net/en/)BestBidVolume

| decimal ATAS.DataFeedsCore.Security.BestBidVolume |
| --- |

getset

Gets or sets the volume available at the best bidding price for the security.

## [◆](https://docs.atas.net/en/)Code

| string ATAS.DataFeedsCore.Security.Code |
| --- |

getset

Id of the security

 May have any format like `CLK4` or `BCHUSDT` etc.

Implements [ATAS.DataFeedsCore.IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a6b237e5f3f1830d15e2856bbd887ff59).

## [◆](https://docs.atas.net/en/)ConnectorId

| string ATAS.DataFeedsCore.Security.ConnectorId |
| --- |

getset

Gets or sets the connector identifier associated with the security.

## [◆](https://docs.atas.net/en/)Digits

| int ATAS.DataFeedsCore.Security.Digits |
| --- |

get

Gets the number of decimal digits used for formatting prices.

Implements [ATAS.DataFeedsCore.IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#aaa7f7f5a8c11b73085c20cdea17bfbb4).

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.Security.EntityType |
| --- |

get

Gets the entity type, which is EntityType.Security.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)Exchange

| string ATAS.DataFeedsCore.Security.Exchange |
| --- |

getset

Gets or sets the exchange associated with the security.

Implements [ATAS.DataFeedsCore.IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a9d33afad63385cd6d32e15ce32ee22be).

## [◆](https://docs.atas.net/en/)ExchangeInstance

| [Exchange](./classATAS_1_1DataFeedsCore_1_1Exchange.md)? ATAS.DataFeedsCore.Security.ExchangeInstance |
| --- |

getset

The object that represents the exchange where the security is listed.

## [◆](https://docs.atas.net/en/)Expiration

| DateTime ATAS.DataFeedsCore.Security.Expiration |
| --- |

getset

Gets or sets the expiration date of the security.

## [◆](https://docs.atas.net/en/)FundingRate

| decimal? ATAS.DataFeedsCore.Security.FundingRate |
| --- |

getset

Gets funding rate exchanged between buyers and sellers. During the funding rate cycle.

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.Security.Id |
| --- |

getset

Gets or sets the identifier of the security.

## [◆](https://docs.atas.net/en/)Instrument

| string ATAS.DataFeedsCore.Security.Instrument |
| --- |

getset

Gets or sets the instrument associated with the security.

## [◆](https://docs.atas.net/en/)IsinId

| long ATAS.DataFeedsCore.Security.IsinId |
| --- |

getset

Gets or sets the International Securities Identification Number (ISIN) identifier of the security.

## [◆](https://docs.atas.net/en/)IsInverseFutures

| bool ATAS.DataFeedsCore.Security.IsInverseFutures |
| --- |

getset

Security is an inverse futures.

## [◆](https://docs.atas.net/en/)LastTradePrice

| decimal? ATAS.DataFeedsCore.Security.LastTradePrice |
| --- |

getset

Price of the last tick

 This value may be null if no ticks were received yet.

## [◆](https://docs.atas.net/en/)LastTradeVolume

| decimal? ATAS.DataFeedsCore.Security.LastTradeVolume |
| --- |

getset

Volume of the last tick

 This value may be null if no ticks were received yet.

## [◆](https://docs.atas.net/en/)LotMaxSize

| decimal? ATAS.DataFeedsCore.Security.LotMaxSize |
| --- |

getset

Maximum size (volume) for the order.

## [◆](https://docs.atas.net/en/)LotMinSize

| decimal? ATAS.DataFeedsCore.Security.LotMinSize |
| --- |

getset

Minimum size (volume) for the order.

## [◆](https://docs.atas.net/en/)LotSize

| decimal ATAS.DataFeedsCore.Security.LotSize |
| --- |

getset

Minimum volume increment.

## [◆](https://docs.atas.net/en/)MarginBuy

| decimal ATAS.DataFeedsCore.Security.MarginBuy |
| --- |

getset

Gets or sets the margin buy value for the security.

## [◆](https://docs.atas.net/en/)MarginSell

| decimal ATAS.DataFeedsCore.Security.MarginSell |
| --- |

getset

Gets or sets the margin sell value for the security.

## [◆](https://docs.atas.net/en/)MarkPrice

| decimal? ATAS.DataFeedsCore.Security.MarkPrice |
| --- |

getset

A price that reflects the real-time spot price on the major exchanges.

## [◆](https://docs.atas.net/en/)MaxPrice

| decimal ATAS.DataFeedsCore.Security.MaxPrice |
| --- |

getset

Gets or sets the maximum price allowed for the security.

## [◆](https://docs.atas.net/en/)MinPrice

| decimal ATAS.DataFeedsCore.Security.MinPrice |
| --- |

getset

Gets or sets the minimum price allowed for the security.

## [◆](https://docs.atas.net/en/)MoneyPnLFormat

| string ATAS.DataFeedsCore.Security.MoneyPnLFormat = _defaultMoneyPnLFormat |
| --- |

getset

Gets or sets money PnL format for security.

## [◆](https://docs.atas.net/en/)NextFundingTime

| DateTimeOffset? ATAS.DataFeedsCore.Security.NextFundingTime |
| --- |

getset

Gets time of the next funding cycle.

## [◆](https://docs.atas.net/en/)OpenInterest

| decimal? ATAS.DataFeedsCore.Security.OpenInterest |
| --- |

getset

Gets or sets the open interest value for the security.

## [◆](https://docs.atas.net/en/)OptionType

| [OptionTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ae2cf0eba1a77bdf2094c56164e77bc29)? ATAS.DataFeedsCore.Security.OptionType |
| --- |

getset

Gets or sets the type of option contract.

## [◆](https://docs.atas.net/en/)Parent

| object? ATAS.DataFeedsCore.Security.Parent |
| --- |

getset

Gets or sets connector entity the security was created from.

## [◆](https://docs.atas.net/en/)PriceMultiplier

| decimal ATAS.DataFeedsCore.Security.PriceMultiplier |
| --- |

getset

Gets or sets the price multiplier for the security.

## [◆](https://docs.atas.net/en/)QuoteCurrency

| string? ATAS.DataFeedsCore.Security.QuoteCurrency |
| --- |

getset

Second currency of the trading pair, if set you can use it for convert operations.

## [◆](https://docs.atas.net/en/)QuoteCurrencyPrecision

| decimal? ATAS.DataFeedsCore.Security.QuoteCurrencyPrecision |
| --- |

getset

Minimum step for quote currency (ex: 0.01 for USD, 0.00000001 for BTC).

## [◆](https://docs.atas.net/en/)SecurityId

| string ATAS.DataFeedsCore.Security.SecurityId |
| --- |

getset

Global [ATAS](../namespaces/namespaceATAS.md) security identifier like `BCHUSDT@Bybit`. Includes exchange id.

## [◆](https://docs.atas.net/en/)StrikePrice

| decimal? ATAS.DataFeedsCore.Security.StrikePrice |
| --- |

getset

A strike price of option contract.

## [◆](https://docs.atas.net/en/)TickCost

| decimal ATAS.DataFeedsCore.Security.TickCost |
| --- |

getset

Price of a single tickSize.

Implements [ATAS.DataFeedsCore.IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#aea3427ed2399feae0aa94ecb63dec218).

## [◆](https://docs.atas.net/en/)TickSize

| decimal ATAS.DataFeedsCore.Security.TickSize |
| --- |

getset

Gets or sets the minimum price increment (tick size) for the security.

Implements [ATAS.DataFeedsCore.IPriceFormatter](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IPriceFormatter.md#a7c167afad451a5064a32e33b81c7daf7).

## [◆](https://docs.atas.net/en/)Type

| [SecType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a185020214d9de6775f60599bf759dfaf) ATAS.DataFeedsCore.Security.Type |
| --- |

getset

Gets or sets the type of security or financial instrument.

## [◆](https://docs.atas.net/en/)UnderlyingSecurity

| [Security](./classATAS_1_1DataFeedsCore_1_1Security.md)? ATAS.DataFeedsCore.Security.UnderlyingSecurity |
| --- |

getset

An underlying security of option contract.

## [◆](https://docs.atas.net/en/)VolumeMultiplier

| decimal ATAS.DataFeedsCore.Security.VolumeMultiplier |
| --- |

getset

Gets or sets the volume multiplier for the security.

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.Security.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [Security.cs](../files/Security_8cs.md)
