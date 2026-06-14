# ATAS.DataFeedsCore.ConnectionStateEventArgs Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.html

Inheritance diagram for ATAS.DataFeedsCore.ConnectionStateEventArgs:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.ConnectionStateEventArgs:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [ConnectionStateEventArgs](./classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.md#a2bd88243db151b84b38e10ebb071432d) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector, ConnectionStates oldState, ConnectionStates newState) |
| | |

| Properties | |
| --- | --- |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [Connector](./classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.md#a8c605142cdd17503fa3b42fca41b4a2f)`[get]` |
| | |
| ConnectionStates | [OldState](./classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.md#a55317a38f549fb877c2ae629da0be8e1)`[get]` |
| | |
| ConnectionStates | [NewState](./classATAS_1_1DataFeedsCore_1_1ConnectionStateEventArgs.md#ada8046c3a9226e34febf08e3c2d83778)`[get]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ConnectionStateEventArgs()

| ATAS.DataFeedsCore.ConnectionStateEventArgs.ConnectionStateEventArgs | ( | [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector, |
| --- | --- | --- | --- |
| | | ConnectionStates | oldState, |
| | | ConnectionStates | newState |
| | ) | | |

## Property Documentation

## [◆](https://docs.atas.net/en/)Connector

| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) ATAS.DataFeedsCore.ConnectionStateEventArgs.Connector |
| --- |

get

## [◆](https://docs.atas.net/en/)NewState

| ConnectionStates ATAS.DataFeedsCore.ConnectionStateEventArgs.NewState |
| --- |

get

## [◆](https://docs.atas.net/en/)OldState

| ConnectionStates ATAS.DataFeedsCore.ConnectionStateEventArgs.OldState |
| --- |

get

The documentation for this class was generated from the following file:
- [ConnectionStateEventArgs.cs](../files/ConnectionStateEventArgs_8cs.md)
