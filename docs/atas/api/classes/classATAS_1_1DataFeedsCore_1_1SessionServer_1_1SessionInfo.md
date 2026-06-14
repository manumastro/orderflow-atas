# ATAS.DataFeedsCore.SessionServer.SessionInfo Class Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.html

Inheritance diagram for ATAS.DataFeedsCore.SessionServer.SessionInfo:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.SessionServer.SessionInfo:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| abstract void | [Start](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a1160a452c825e97f87d6cdeadd29419c) () |
| | |
| abstract void | [Stop](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#adcd6656f87e8ad3f89d291ad8bfb0e6f) () |
| | |
| void | [SetLoggedIn](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a7ede9600e846b142815783f51cb28133) () |
| | |

| Protected Member Functions | |
| --- | --- |
| | [SessionInfo](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#abaebdcf88f0e8f0dc709d5785d893b30) ([SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) type, EndPoint address) |
| | |

| Properties | |
| --- | --- |
| [SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) | [Type](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#ac6d8363f753809d4d06b4843f4870bdb)`[get]` |
| | |
| EndPoint | [Address](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#ad6827371d9b220962e041dbe40da0895)`[get]` |
| | |
| [ServerStates](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0dde) | [State](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a4c919477b33be6c81e8d388f592834bd)`[get, protected set]` |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md) | [User](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a6b0a2c25e3703f8ef2baef93fa6a1fe5)`[get, set]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SessionInfo()

| ATAS.DataFeedsCore.SessionServer.SessionInfo.SessionInfo | ( | [SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) | type, |
| --- | --- | --- | --- |
| | | EndPoint | address |
| | ) | | |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)SetLoggedIn()

| void ATAS.DataFeedsCore.SessionServer.SessionInfo.SetLoggedIn | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Start()

| abstract void ATAS.DataFeedsCore.SessionServer.SessionInfo.Start | ( | | ) | |
| --- | --- | --- | --- | --- |

pure virtual

Implemented in [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#ab9f9f26437276b895f325597aa65e8d2).

## [◆](https://docs.atas.net/en/)Stop()

| abstract void ATAS.DataFeedsCore.SessionServer.SessionInfo.Stop | ( | | ) | |
| --- | --- | --- | --- | --- |

pure virtual

Implemented in [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#ada619202ba03dd4c2231fc7b2289d8db).

## Property Documentation

## [◆](https://docs.atas.net/en/)Address

| EndPoint ATAS.DataFeedsCore.SessionServer.SessionInfo.Address |
| --- |

get

## [◆](https://docs.atas.net/en/)State

| [ServerStates](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0dde) ATAS.DataFeedsCore.SessionServer.SessionInfo.State |
| --- |

getprotected set

## [◆](https://docs.atas.net/en/)Type

| [SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) ATAS.DataFeedsCore.SessionServer.SessionInfo.Type |
| --- |

get

## [◆](https://docs.atas.net/en/)User

| [User](./classATAS_1_1DataFeedsCore_1_1User.md) ATAS.DataFeedsCore.SessionServer.SessionInfo.User |
| --- |

getset

The documentation for this class was generated from the following file:
- [SessionInfo.cs](../files/SessionInfo_8cs.md)
