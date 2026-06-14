# ATAS.DataFeedsCore.SessionServer.SocketSession< TMessage > Class Template Referenceabstract

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.html

Inheritance diagram for ATAS.DataFeedsCore.SessionServer.SocketSession< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.SessionServer.SocketSession< TMessage >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| override void | [Start](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#ab9f9f26437276b895f325597aa65e8d2) () |
| | |
| override void | [Stop](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#ada619202ba03dd4c2231fc7b2289d8db) () |
| | |
| void | [Send](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#ae7c57240c55290a09dcb6dfbc6fcff9d) (TMessage message) |
| | |
| - Public Member Functions inherited from [ATAS.DataFeedsCore.SessionServer.SessionInfo](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md) | |
| abstract void | [Start](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a1160a452c825e97f87d6cdeadd29419c) () |
| | |
| abstract void | [Stop](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#adcd6656f87e8ad3f89d291ad8bfb0e6f) () |
| | |
| void | [SetLoggedIn](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a7ede9600e846b142815783f51cb28133) () |
| | |

| Protected Member Functions | |
| --- | --- |
| | [SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a4f3c5138ea7fc9084ecd5b52fad52b62) ([SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) type, Socket socket, int maxQueueSize) |
| | |
| | [SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a428cde82524429d6b432868b9616be22) ([SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) type, Socket socket) |
| | |
| abstract void | [Process](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a7cc66e51edb2119145ea5b41835f3dab) (SocketAsyncEventArgs e) |
| | |
| abstract void | [SetSendBuffer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a9671aaf891098b67666f55534938681e) (ConcurrentQueue queue, SocketAsyncEventArgs e) |
| | |
| abstract void | [SetReceiveBuffer](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a1231f1f5aa77c205b0024374c072efed) (SocketAsyncEventArgs e) |
| | |
| abstract void | [SendLogout](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a10c060dc6bebab9ddf407ca8e9b5921a) (string reason=null) |
| | |
| void | [CloseClientSocket](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a33b180c61f302db2b6e80f4eec27186e) (string reason=null) |
| | |
| - Protected Member Functions inherited from [ATAS.DataFeedsCore.SessionServer.SessionInfo](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md) | |
| | [SessionInfo](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#abaebdcf88f0e8f0dc709d5785d893b30) ([SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) type, EndPoint address) |
| | |

| Properties | |
| --- | --- |
| int | [QueueLength](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#a95e1fb4670369606430a39b07f46d2d5)`[get]` |
| | |
| int | [MaxQueueSize](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md#abb52268dcefde9527df1fc8aeedb0e6b)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.SessionServer.SessionInfo](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md) | |
| [SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) | [Type](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#ac6d8363f753809d4d06b4843f4870bdb)`[get]` |
| | |
| EndPoint | [Address](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#ad6827371d9b220962e041dbe40da0895)`[get]` |
| | |
| [ServerStates](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0dde) | [State](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a4c919477b33be6c81e8d388f592834bd)`[get, protected set]` |
| | |
| [User](./classATAS_1_1DataFeedsCore_1_1User.md) | [User](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a6b0a2c25e3703f8ef2baef93fa6a1fe5)`[get, set]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SocketSession() [1/2]

| [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).[SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md) | ( | [SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) | type, |
| --- | --- | --- | --- |
| | | Socket | socket, |
| | | int | maxQueueSize |
| | ) | | |

protected

## [◆](https://docs.atas.net/en/)SocketSession() [2/2]

| [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).[SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md) | ( | [SessionInfoType](../namespaces/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) | type, |
| --- | --- | --- | --- |
| | | Socket | socket |
| | ) | | |

protected

## Member Function Documentation

## [◆](https://docs.atas.net/en/)CloseClientSocket()

| void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).CloseClientSocket | ( | string | reason = `null` | ) | |
| --- | --- | --- | --- | --- | --- |

protected

## [◆](https://docs.atas.net/en/)Process()

| abstract void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).Process | ( | SocketAsyncEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

## [◆](https://docs.atas.net/en/)Send()

| void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).Send | ( | TMessage | message | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)SendLogout()

| abstract void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).SendLogout | ( | string | reason = `null` | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

## [◆](https://docs.atas.net/en/)SetReceiveBuffer()

| abstract void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).SetReceiveBuffer | ( | SocketAsyncEventArgs | e | ) | |
| --- | --- | --- | --- | --- | --- |

protectedpure virtual

## [◆](https://docs.atas.net/en/)SetSendBuffer()

| abstract void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).SetSendBuffer | ( | ConcurrentQueue | queue, |
| --- | --- | --- | --- |
| | | SocketAsyncEventArgs | e |
| | ) | | |

protectedpure virtual

## [◆](https://docs.atas.net/en/)Start()

| override void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).Start | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Implements [ATAS.DataFeedsCore.SessionServer.SessionInfo](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#a1160a452c825e97f87d6cdeadd29419c).

## [◆](https://docs.atas.net/en/)Stop()

| override void [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).Stop | ( | | ) | |
| --- | --- | --- | --- | --- |

virtual

Implements [ATAS.DataFeedsCore.SessionServer.SessionInfo](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md#adcd6656f87e8ad3f89d291ad8bfb0e6f).

## Property Documentation

## [◆](https://docs.atas.net/en/)MaxQueueSize

| int [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).MaxQueueSize |
| --- |

get

## [◆](https://docs.atas.net/en/)QueueLength

| int [ATAS.DataFeedsCore.SessionServer.SocketSession](./classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md).QueueLength |
| --- |

get

The documentation for this class was generated from the following file:
- [SessionInfo.cs](../files/SessionInfo_8cs.md)
