# ATAS.Indicators.InstrumentInfo Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1InstrumentInfo.html

Implementation of the IInstrumentInfo interface representing instrument information.
 [More...](./classATAS_1_1Indicators_1_1InstrumentInfo.md#details)

Inheritance diagram for ATAS.Indicators.InstrumentInfo:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Indicators.InstrumentInfo:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [InstrumentInfo](./classATAS_1_1Indicators_1_1InstrumentInfo.md#a768fe7c563be2eab657bc0f4497ea582) (string instrument, string exchange, decimal tickSize, int timeZone) |
| | Constructor to create an InstrumentInfo object with the specified parameters. |
| | |

| Properties | |
| --- | --- |
| string | [Instrument](./classATAS_1_1Indicators_1_1InstrumentInfo.md#a7464373ff790799fbb3ce1e79a43fba7)`[get]` |
| | Gets the name of the instrument. |
| | |
| string | [Exchange](./classATAS_1_1Indicators_1_1InstrumentInfo.md#a8bf9acbdf2125eb4e48c83ccc6002e94)`[get]` |
| | Gets the name of the exchange where the instrument is traded. |
| | |
| decimal | [TickSize](./classATAS_1_1Indicators_1_1InstrumentInfo.md#a4b336c3feb765ff439124586db9ef1f2)`[get]` |
| | Gets the tick size of the instrument, which is the minimum price movement. |
| | |
| int | [TimeZone](./classATAS_1_1Indicators_1_1InstrumentInfo.md#aab089262092a0e32c62bb33ace44e9f7)`[get, set]` |
| | Gets the time zone of the instrument. |
| | |
| - Properties inherited from [ATAS.Indicators.IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md) | |
| string | [Instrument](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a534d6754af425ba189f6e615e57e288b)`[get]` |
| | Gets the name of the instrument. |
| | |
| string | [Exchange](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a45f072ac852f18be7b712feb5ef323b3)`[get]` |
| | Gets the name of the exchange where the instrument is traded. |
| | |
| decimal | [TickSize](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a3b5ac993cec2a794958de72d06e00aa7)`[get]` |
| | Gets the tick size of the instrument, which is the minimum price movement. |
| | |
| int | [TimeZone](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#ab8926123c9c2c07a7411d557643040e2)`[get]` |
| | Gets the time zone of the instrument. |
| | |

## Detailed Description

Implementation of the IInstrumentInfo interface representing instrument information.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)InstrumentInfo()

| ATAS.Indicators.InstrumentInfo.InstrumentInfo | ( | string | instrument, |
| --- | --- | --- | --- |
| | | string | exchange, |
| | | decimal | tickSize, |
| | | int | timeZone |
| | ) | | |

Constructor to create an InstrumentInfo object with the specified parameters.

Parameters

| instrument | The name of the instrument. |
| --- | --- |
| exchange | The name of the exchange where the instrument is traded. |
| tickSize | The tick size of the instrument (minimum price movement). |
| timeZone | The time zone associated with the instrument. |

## Property Documentation

## [◆](https://docs.atas.net/en/)Exchange

| string ATAS.Indicators.InstrumentInfo.Exchange |
| --- |

get

Gets the name of the exchange where the instrument is traded.

Implements [ATAS.Indicators.IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a45f072ac852f18be7b712feb5ef323b3).

## [◆](https://docs.atas.net/en/)Instrument

| string ATAS.Indicators.InstrumentInfo.Instrument |
| --- |

get

Gets the name of the instrument.

Implements [ATAS.Indicators.IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a534d6754af425ba189f6e615e57e288b).

## [◆](https://docs.atas.net/en/)TickSize

| decimal ATAS.Indicators.InstrumentInfo.TickSize |
| --- |

get

Gets the tick size of the instrument, which is the minimum price movement.

Implements [ATAS.Indicators.IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a3b5ac993cec2a794958de72d06e00aa7).

## [◆](https://docs.atas.net/en/)TimeZone

| int ATAS.Indicators.InstrumentInfo.TimeZone |
| --- |

getset

Gets the time zone of the instrument.

Implements [ATAS.Indicators.IInstrumentInfo](../interfaces/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#ab8926123c9c2c07a7411d557643040e2).

The documentation for this class was generated from the following file:
- [InstrumentInfo.cs](../files/InstrumentInfo_8cs.md)
