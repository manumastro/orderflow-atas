# ATAS.DataFeedsCore.SessionServer.SessionServer< TSession, TMessage > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.html

Inheritance diagram for ATAS.DataFeedsCore.SessionServer.SessionServer< TSession, TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.SessionServer.SessionServer< TSession, TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md#a62fd9bee9542fc92b7f873ce0e529a86) (string name, Func, TSession > sessionFactory, Action handler) |
| | |
| void | [Start](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md#a766444fb1f8d67ee43a85c425ea35020) () |
| | |
| void | [Stop](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md#a1c263b6d7f8fefbf6706001f2fe169e6) () |
| | |
| void | [Start](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1SessionServer_1_1ISessionServer.md#a4791426a10b24144941664d1f8b78253) () |
| | |
| void | [Stop](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1SessionServer_1_1ISessionServer.md#a3898ccd22923193baa0b0daff49025d4) () |
| | |

| Properties | |
| --- | --- |
| string | [Address](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md#a18b762ce118fd2167788f151d8abda96)`[get, set]` |
| | |
| int | [Port](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md#a32d86817a92b46f9d5da0ab4bb1fa456)`[get, set]` |
| | |
| [ServerStates](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0dde) | [State](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md#a5bef38a1a2ca93a021eece83c79a84cf)`[get]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SessionServer()

| [ATAS.DataFeedsCore.SessionServer.SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md).[SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md) | ( | string | name, |
| --- | --- | --- | --- |
| | | Func, TSession > | sessionFactory, |
| | | Action | handler |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Start()

| void [ATAS.DataFeedsCore.SessionServer.SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md).Start | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.SessionServer.ISessionServer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1SessionServer_1_1ISessionServer.md#a4791426a10b24144941664d1f8b78253).

## [◆](https://docs.atas.net/en/)Stop()

| void [ATAS.DataFeedsCore.SessionServer.SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md).Stop | ( | | ) | |
| --- | --- | --- | --- | --- |

Implements [ATAS.DataFeedsCore.SessionServer.ISessionServer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1SessionServer_1_1ISessionServer.md#a3898ccd22923193baa0b0daff49025d4).

## Property Documentation

## [◆](https://docs.atas.net/en/)Address

| string [ATAS.DataFeedsCore.SessionServer.SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md).Address |
| --- |

getset

## [◆](https://docs.atas.net/en/)Port

| int [ATAS.DataFeedsCore.SessionServer.SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md).Port |
| --- |

getset

## [◆](https://docs.atas.net/en/)State

| [ServerStates](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0dde) [ATAS.DataFeedsCore.SessionServer.SessionServer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md).State |
| --- |

get

The documentation for this class was generated from the following file:
- [SessionServer.cs](../files/SessionServer_8cs.md)
