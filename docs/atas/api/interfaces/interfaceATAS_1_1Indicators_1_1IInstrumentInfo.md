# ATAS.Indicators.IInstrumentInfo Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IInstrumentInfo.html

Interface representing instrument information.
 [More...](./interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#details)

Inheritance diagram for ATAS.Indicators.IInstrumentInfo:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| string | [Instrument](./interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a534d6754af425ba189f6e615e57e288b)`[get]` |
| | Gets the name of the instrument. |
| | |
| string | [Exchange](./interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a45f072ac852f18be7b712feb5ef323b3)`[get]` |
| | Gets the name of the exchange where the instrument is traded. |
| | |
| decimal | [TickSize](./interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#a3b5ac993cec2a794958de72d06e00aa7)`[get]` |
| | Gets the tick size of the instrument, which is the minimum price movement. |
| | |
| int | [TimeZone](./interfaceATAS_1_1Indicators_1_1IInstrumentInfo.md#ab8926123c9c2c07a7411d557643040e2)`[get]` |
| | Gets the time zone of the instrument. |
| | |

## Detailed Description

Interface representing instrument information.

## Property Documentation

## [◆](https://docs.atas.net/en/)Exchange

| string ATAS.Indicators.IInstrumentInfo.Exchange |
| --- |

get

Gets the name of the exchange where the instrument is traded.

Implemented in [ATAS.Indicators.InstrumentInfo](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md#a8bf9acbdf2125eb4e48c83ccc6002e94).

## [◆](https://docs.atas.net/en/)Instrument

| string ATAS.Indicators.IInstrumentInfo.Instrument |
| --- |

get

Gets the name of the instrument.

Implemented in [ATAS.Indicators.InstrumentInfo](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md#a7464373ff790799fbb3ce1e79a43fba7).

## [◆](https://docs.atas.net/en/)TickSize

| decimal ATAS.Indicators.IInstrumentInfo.TickSize |
| --- |

get

Gets the tick size of the instrument, which is the minimum price movement.

Implemented in [ATAS.Indicators.InstrumentInfo](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md#a4b336c3feb765ff439124586db9ef1f2).

## [◆](https://docs.atas.net/en/)TimeZone

| int ATAS.Indicators.IInstrumentInfo.TimeZone |
| --- |

get

Gets the time zone of the instrument.

Implemented in [ATAS.Indicators.InstrumentInfo](../classes/classATAS_1_1Indicators_1_1InstrumentInfo.md#aab089262092a0e32c62bb33ace44e9f7).

The documentation for this interface was generated from the following file:
- [InstrumentInfo.cs](../files/InstrumentInfo_8cs.md)
