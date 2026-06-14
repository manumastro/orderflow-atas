# ATAS.DataFeedsCore.Exceptions.ExchangeException Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.html

An exception means the Exchange sent us an logic error like violating business rules It allows us to separate network errors and logic errors.
 [More...](./classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.md#details)

Inheritance diagram for ATAS.DataFeedsCore.Exceptions.ExchangeException:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.Exceptions.ExchangeException:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [ExchangeException](./classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.md#a87298b042dbbf126965b41ce99240764) (Exception innerException) |
| | |
| | [ExchangeException](./classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.md#a813f05d46354341bfe32ff92e783d71d) (string originalMessage, Exception? innerException=null) |
| | |
| | [ExchangeException](./classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.md#a43fa60eaaca91f6d6138eb53447faca5) (string originalMessage, string displayText, Exception? innerException=null) |
| | |

| Properties | |
| --- | --- |
| string | [DisplayText](./classATAS_1_1DataFeedsCore_1_1Exceptions_1_1ExchangeException.md#ae73d8ca97076ea211671cddea004bdaa)`[get, set]` |
| | |

## Detailed Description

An exception means the Exchange sent us an logic error like violating business rules It allows us to separate network errors and logic errors.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ExchangeException() [1/3]

| ATAS.DataFeedsCore.Exceptions.ExchangeException.ExchangeException | ( | Exception | innerException | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)ExchangeException() [2/3]

| ATAS.DataFeedsCore.Exceptions.ExchangeException.ExchangeException | ( | string | originalMessage, |
| --- | --- | --- | --- |
| | | Exception? | innerException = `null` |
| | ) | | |

## [◆](https://docs.atas.net/en/)ExchangeException() [3/3]

| ATAS.DataFeedsCore.Exceptions.ExchangeException.ExchangeException | ( | string | originalMessage, |
| --- | --- | --- | --- |
| | | string | displayText, |
| | | Exception? | innerException = `null` |
| | ) | | |

## Property Documentation

## [◆](https://docs.atas.net/en/)DisplayText

| string ATAS.DataFeedsCore.Exceptions.ExchangeException.DisplayText |
| --- |

getset

The documentation for this class was generated from the following file:
- [ExchangeException.cs](../files/ExchangeException_8cs.md)
