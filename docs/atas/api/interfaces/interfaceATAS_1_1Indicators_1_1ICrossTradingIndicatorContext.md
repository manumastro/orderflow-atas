# ATAS.Indicators.ICrossTradingIndicatorContext Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.html

Context for indicators that need to access cross-trading instrument data. Allows indicators to subscribe to cross-trading instrument changes and access market data for the selected cross-trading instrument without creating new subscriptions.
 [More...](./interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md#details)

| Public Member Functions | |
| --- | --- |
| Task | [GetCurrentDataProviderAsync](./interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md#afb86f0f0bc6fa000283c828719010e2f) () |
| | Attempts to get the online data provider for a specific instrument. Returns null if the instrument is not subscribed or cross-trading is not available. |
| | |

| Properties | |
| --- | --- |
| string? | [CurrentCrossTradingDisplayName](./interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md#ab3f0f8bbd4bf89a0f9f8c6f5dc175a3e)`[get]` |
| | Gets the display name of the currently selected cross-trading instrument, or null if cross-trading is not active. |
| | |
| bool | [IsCrossTradingActive](./interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md#aacd470e2c7725a9ea16c58b37fd2835e)`[get]` |
| | Gets a value indicating whether cross-trading is currently active (selected instrument differs from chart base). |
| | |

| Events | |
| --- | --- |
| EventHandler | [CrossTradingInstrumentChanged](./interfaceATAS_1_1Indicators_1_1ICrossTradingIndicatorContext.md#aff5f1941b582212ff33ceaa1c10c6301) |
| | Event raised when the cross-trading instrument selection changes. |
| | |

## Detailed Description

Context for indicators that need to access cross-trading instrument data. Allows indicators to subscribe to cross-trading instrument changes and access market data for the selected cross-trading instrument without creating new subscriptions.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetCurrentDataProviderAsync()

| Task ATAS.Indicators.ICrossTradingIndicatorContext.GetCurrentDataProviderAsync | ( | | ) | |
| --- | --- | --- | --- | --- |

Attempts to get the online data provider for a specific instrument. Returns null if the instrument is not subscribed or cross-trading is not available.

ReturnsThe data provider for the instrument, or null if not subscribed.

## Property Documentation

## [◆](https://docs.atas.net/en/)CurrentCrossTradingDisplayName

| string? ATAS.Indicators.ICrossTradingIndicatorContext.CurrentCrossTradingDisplayName |
| --- |

get

Gets the display name of the currently selected cross-trading instrument, or null if cross-trading is not active.

## [◆](https://docs.atas.net/en/)IsCrossTradingActive

| bool ATAS.Indicators.ICrossTradingIndicatorContext.IsCrossTradingActive |
| --- |

get

Gets a value indicating whether cross-trading is currently active (selected instrument differs from chart base).

## Event Documentation

## [◆](https://docs.atas.net/en/)CrossTradingInstrumentChanged

| EventHandler ATAS.Indicators.ICrossTradingIndicatorContext.CrossTradingInstrumentChanged |
| --- |

Event raised when the cross-trading instrument selection changes.

The documentation for this interface was generated from the following file:
- [ICrossTradingIndicatorContext.cs](../files/ICrossTradingIndicatorContext_8cs.md)
