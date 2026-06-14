# ATAS.DataFeedsCore.AsyncConnectorMessage Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.html

Service message for passing continuations to the connector thread.
 [More...](./classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md#details)

| Properties | |
| --- | --- |
| SendOrPostCallback | [Callback](./classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md#af60ef77bfaee08d87be334f298b9de21)`[get, set]` |
| | |
| object | [State](./classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md#a8186ba1e2a907c65762339b53eb925cb)`[get, set]` |
| | |
| ManualResetEvent | [Sync](./classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md#a4e19dd5e2f2e2f0a78794a89f421822e)`[get, set]` |
| | |
| bool | [IsSetupMessage](./classATAS_1_1DataFeedsCore_1_1AsyncConnectorMessage.md#a510cc9d1ea951effa74767d9cb3b3cf8)`[get]` |
| | Indicates that synchronization context should be set up on the connector queue thread. |
| | |

## Detailed Description

Service message for passing continuations to the connector thread.

## Property Documentation

## [◆](https://docs.atas.net/en/)Callback

| SendOrPostCallback ATAS.DataFeedsCore.AsyncConnectorMessage.Callback |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsSetupMessage

| bool ATAS.DataFeedsCore.AsyncConnectorMessage.IsSetupMessage |
| --- |

get

Indicates that synchronization context should be set up on the connector queue thread.

## [◆](https://docs.atas.net/en/)State

| object ATAS.DataFeedsCore.AsyncConnectorMessage.State |
| --- |

getset

## [◆](https://docs.atas.net/en/)Sync

| ManualResetEvent ATAS.DataFeedsCore.AsyncConnectorMessage.Sync |
| --- |

getset

The documentation for this class was generated from the following file:
- [AsyncConnectorMessage.cs](../files/AsyncConnectorMessage_8cs.md)
