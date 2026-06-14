# ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.html

| Public Member Functions | |
| --- | --- |
| | [TaskQueue](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md#abb26d79d2c22167a94184c107558e4a5) (int requestPerPeriod, TimeSpan period, bool strictSequence=true) |
| | Task queue with execution frequency. |
| | |
| async Task | [WaitAsync](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md#a41a2ef8e0e6d317164374d1252de71e6) (bool stopOnError=false) |
| | |
| void | [Add](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md#a8c0119f39f57b679a4ef41385a7d9570) (Func action, bool highPriority=false) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md#ab457e7567647b89d9c99c7c362a9b057) () |
| | |

| Properties | |
| --- | --- |
| int | [Count](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md#a2a22a171bcc8f08b520adebea1359fdd)`[get]` |
| | |

| Events | |
| --- | --- |
| Action | [QueueError](./classATAS_1_1DataFeedsCore_1_1ConnectorWebsocket_1_1TaskQueue.md#a719f09fc14481c213c19f72c5e730c5c) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)TaskQueue()

| ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue.TaskQueue | ( | int | requestPerPeriod, |
| --- | --- | --- | --- |
| | | TimeSpan | period, |
| | | bool | strictSequence = `true` |
| | ) | | |

Task queue with execution frequency.

Parameters

| requestPerPeriod | Run execution limit per period |
| --- | --- |
| period | Period of limit |
| strictSequence | Next task is run after previous completion |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Add()

| void ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue.Add | ( | Func | action, |
| --- | --- | --- | --- |
| | | bool | highPriority = `false` |
| | ) | | |

## [◆](https://docs.atas.net/en/)Clear()

| void ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)WaitAsync()

| async Task ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue.WaitAsync | ( | bool | stopOnError = `false` | ) | |
| --- | --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Count

| int ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue.Count |
| --- |

get

## Event Documentation

## [◆](https://docs.atas.net/en/)QueueError

| Action ATAS.DataFeedsCore.ConnectorWebsocket.TaskQueue.QueueError |
| --- |

The documentation for this class was generated from the following file:
- [TaskQueue.cs](../files/TaskQueue_8cs.md)
