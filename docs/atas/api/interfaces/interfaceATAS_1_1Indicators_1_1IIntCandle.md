# ATAS.Indicators.IIntCandle Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IIntCandle.html

Represents an interface for an integer-based candle.
 [More...](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#details)

| Properties | |
| --- | --- |
| int | [Open](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#adf3593ba48332d1a2de9a4027239cecb)`[get, set]` |
| | Open price of the candle. |
| | |
| int | [High](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a8662d35d4cb6a686da75b6f0b6b3a90c)`[get, set]` |
| | High price of the candle. |
| | |
| int | [Low](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#aa3655dcb44c8b5be4dc7f9497aedb0b4)`[get, set]` |
| | Low price of the candle. |
| | |
| int | [Close](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#af8c21ad4386ec8c501bd5382cf669732)`[get, set]` |
| | Close price of the candle. |
| | |
| decimal | [Ask](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a43f24f8a9ed7b2e05bb56a7f44ed8f3a)`[get, set]` |
| | Ask price of the candle. |
| | |
| decimal | [Bid](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a1dfe8a213b8f40650970a445fe895dd5)`[get, set]` |
| | Bid price of the candle. |
| | |
| decimal | [Betweens](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#ac4c341220df8a52d34b9bcd7cbad01bf)`[get, set]` |
| | Volume between bids and asks. |
| | |
| decimal | [Delta](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a651d3623feb4dcaf00c2bc02321b2138)`[get]` |
| | Delta value of the candle. |
| | |
| int | [Ticks](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a9a820e033843ec35cdfdb54eb45b7352)`[get, set]` |
| | Ticks count of the candle. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [BeginTime](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a89459de9666a4530b11abbd7c85902e7)`[get, set]` |
| | Starting time of the candle. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [LastTime](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a16ecac7b40377c67ba3bc62469118557)`[get, set]` |
| | Last time of the candle. |
| | |
| decimal | [Volume](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#afa4fb1cff0885049c99cd93890bb6c7e)`[get]` |
| | Volume of the candle. |
| | |
| decimal | [OI](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a23cd22b59e74f739baebd85aa0835964)`[get, set]` |
| | Open interest of the candle. |
| | |
| decimal | [MaxOI](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#aec71baead627ad815771703793097449)`[get, set]` |
| | Maximum open interest value of the candle. |
| | |
| decimal | [MinOI](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a02b0618355cf2bbfdf6c3eb840ed00e7)`[get, set]` |
| | Minimum open interest value of the candle. |
| | |
| decimal | [MaxDelta](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#a9cdd338e5077c4aa43938742c9c43db9)`[get, set]` |
| | Maximum delta value of the candle. |
| | |
| decimal | [MinDelta](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#ae6693a0d6935cff681d36a0195376925)`[get, set]` |
| | Minimum delta value of the candle. |
| | |
| decimal | [VWAP](./interfaceATAS_1_1Indicators_1_1IIntCandle.md#ad0e8c062e83cb51efcf10bf0e30432d7)`[get]` |
| | Volume-weighted average price of the candle. |
| | |

## Detailed Description

Represents an interface for an integer-based candle.

## Property Documentation

## [◆](https://docs.atas.net/en/)Ask

| decimal ATAS.Indicators.IIntCandle.Ask |
| --- |

getset

Ask price of the candle.

## [◆](https://docs.atas.net/en/)BeginTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IIntCandle.BeginTime |
| --- |

getset

Starting time of the candle.

## [◆](https://docs.atas.net/en/)Betweens

| decimal ATAS.Indicators.IIntCandle.Betweens |
| --- |

getset

Volume between bids and asks.

## [◆](https://docs.atas.net/en/)Bid

| decimal ATAS.Indicators.IIntCandle.Bid |
| --- |

getset

Bid price of the candle.

## [◆](https://docs.atas.net/en/)Close

| int ATAS.Indicators.IIntCandle.Close |
| --- |

getset

Close price of the candle.

## [◆](https://docs.atas.net/en/)Delta

| decimal ATAS.Indicators.IIntCandle.Delta |
| --- |

get

Delta value of the candle.

## [◆](https://docs.atas.net/en/)High

| int ATAS.Indicators.IIntCandle.High |
| --- |

getset

High price of the candle.

## [◆](https://docs.atas.net/en/)LastTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IIntCandle.LastTime |
| --- |

getset

Last time of the candle.

## [◆](https://docs.atas.net/en/)Low

| int ATAS.Indicators.IIntCandle.Low |
| --- |

getset

Low price of the candle.

## [◆](https://docs.atas.net/en/)MaxDelta

| decimal ATAS.Indicators.IIntCandle.MaxDelta |
| --- |

getset

Maximum delta value of the candle.

## [◆](https://docs.atas.net/en/)MaxOI

| decimal ATAS.Indicators.IIntCandle.MaxOI |
| --- |

getset

Maximum open interest value of the candle.

## [◆](https://docs.atas.net/en/)MinDelta

| decimal ATAS.Indicators.IIntCandle.MinDelta |
| --- |

getset

Minimum delta value of the candle.

## [◆](https://docs.atas.net/en/)MinOI

| decimal ATAS.Indicators.IIntCandle.MinOI |
| --- |

getset

Minimum open interest value of the candle.

## [◆](https://docs.atas.net/en/)OI

| decimal ATAS.Indicators.IIntCandle.OI |
| --- |

getset

Open interest of the candle.

## [◆](https://docs.atas.net/en/)Open

| int ATAS.Indicators.IIntCandle.Open |
| --- |

getset

Open price of the candle.

## [◆](https://docs.atas.net/en/)Ticks

| int ATAS.Indicators.IIntCandle.Ticks |
| --- |

getset

Ticks count of the candle.

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.Indicators.IIntCandle.Volume |
| --- |

get

Volume of the candle.

## [◆](https://docs.atas.net/en/)VWAP

| decimal ATAS.Indicators.IIntCandle.VWAP |
| --- |

get

Volume-weighted average price of the candle.

The documentation for this interface was generated from the following file:
- [PriceVolumeInfo.cs](../files/PriceVolumeInfo_8cs.md)
