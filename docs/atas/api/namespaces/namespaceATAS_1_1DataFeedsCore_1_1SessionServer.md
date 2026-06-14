# ATAS.DataFeedsCore.SessionServer Namespace Reference

Source: https://docs.atas.net/en/namespaceATAS_1_1DataFeedsCore_1_1SessionServer.html

| Classes | |
| --- | --- |
| interface | [ISessionServer](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1SessionServer_1_1ISessionServer.md) |
| | |
| class | [SessionInfo](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionInfo.md) |
| | |
| class | [SessionServer](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SessionServer.md) |
| | |
| class | [SocketSession](../classes/classATAS_1_1DataFeedsCore_1_1SessionServer_1_1SocketSession.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [SessionInfoType](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) { [Adm](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915ac88e960aededdd22976687b70dab46ce)
, [Fix](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915a2de1239f236684c5bee75bd524a38a51)
, [Sbe](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915a0be2a2a37fffca3908166b86e0bd8807)
 } |
| | |
| enum | [ServerStates](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0dde) {
  [None](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0ddea6adf97f83acf6453d4a6a4b1070f3754)
, [Stopped](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0ddeac23e2b09ebe6bf4cb5e2a9abe85c0be2)
, [Started](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0ddea8428552d86c0d262a542a528af490afa)
, [Stopping](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0ddea7b7ecb39b9e110c2a31409a1672bad23)
,
  [Starting](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0ddeac2efe4bbd13e6cb0db293e72884273c0)
, [LoggedIn](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0ddeafebcd1edf6010db4858e623d1dd2f3bc)

 } |
| | |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)ServerStates

| enum [ATAS.DataFeedsCore.SessionServer.ServerStates](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a8b1c7dd4baeba0bd439109112fef0dde) |
| --- |

| Enumerator | |
| --- | --- |
| None | |
| Stopped | |
| Started | |
| Stopping | |
| Starting | |
| LoggedIn | |

## [◆](https://docs.atas.net/en/)SessionInfoType

| enum [ATAS.DataFeedsCore.SessionServer.SessionInfoType](./namespaceATAS_1_1DataFeedsCore_1_1SessionServer.md#a079732f2a2e47d319ef60953285f0915) |
| --- |

| Enumerator | |
| --- | --- |
| Adm | |
| Fix | |
| Sbe | |
