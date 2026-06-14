# OFT.Core.IConnectorSettings Interface Reference

Source: https://docs.atas.net/en/interfaceOFT_1_1Core_1_1IConnectorSettings.html

Inheritance diagram for OFT.Core.IConnectorSettings:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [CreateConnector](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a91ba23e01cfb95836252cd791eff242b) (string dataPath) |
| | |
| void | [ApplySettings](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a22fe45709c709ca8055ef15a6b721b7a) ([IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) |
| | |
| bool | [CheckSupported](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#ae160be2ff12e19c9d2f9483a2ac74ff7) (out string? errorMessage) |
| | Checks if connector is supported on this machine. |
| | |
| bool | [HasSameCredentials](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a078c24514424d16226bd1382d3a94a22) ([IConnectorSettings](./interfaceOFT_1_1Core_1_1IConnectorSettings.md) other) |
| | Checks if this connector has the same credentials as another connector. Also verifies that the connector types match. |
| | |

| Properties | |
| --- | --- |
| string | [Type](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a6e2b52c5b03af7a1d4ea36a175837c4b)`[get]` |
| | |
| string | [Description](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#aa387744b45b96f59d1fe2b1a89196fad)`[get]` |
| | |
| Uri | [Logo](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a00012972d5cfa12d129500fda0b86c36)`[get]` |
| | |
| Guid | [Id](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#aa629abc0db0dd2b4c22ff92dedca62e7)`[get, set]` |
| | |
| string | [Name](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a77492fd5e2e9846a999116c8778c8c74)`[get, set]` |
| | |
| bool | [IsMarketDataEnabled](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#ae5cf4eb42ddcb93d40dc5f0b9f50c0b0)`[get, set]` |
| | |
| bool | [IsAutoConnectEnabled](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a4fde71e8c06422a9784cfb04f4c0356f)`[get, set]` |
| | |
| [ConnectorFeatures](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019) | [Features](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#aea958afa6fb75b2eb87c7e18c8b32848)`[get]` |
| | |
| [ConnectorSettingsTypes](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930) | [SettingsTypes](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a1809158aa2742ace16e6fcb05b89eb6f)`[get]` |
| | |
| bool | [IsDemo](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#addb337c18a6a8e63afed2f73f586e23e)`[get]` |
| | Indicates that connector uses TestNet environment. |
| | |
| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) | [MarketDataDelayPeriod](./interfaceOFT_1_1Core_1_1IConnectorSettings.md#a01520d683c4716a505172f02a34a0fa5)`[get]` |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)ApplySettings()

| void OFT.Core.IConnectorSettings.ApplySettings | ( | [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | connector | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#ab14cde7bd5ff0a5ec3e1841f9f0b0cc8).

## [◆](https://docs.atas.net/en/)CheckSupported()

| bool OFT.Core.IConnectorSettings.CheckSupported | ( | out string? | errorMessage | ) | |
| --- | --- | --- | --- | --- | --- |

Checks if connector is supported on this machine.

Parameters

| errorMessage | Null if supported otherwise contains error text |
| --- | --- |

Returnstrue if no problems detected, false if not supported

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#a6c550e8cb2653d0c0d9cbf643d09eea7).

## [◆](https://docs.atas.net/en/)CreateConnector()

| [IDataFeedConnector](./interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) OFT.Core.IConnectorSettings.CreateConnector | ( | string | dataPath | ) | |
| --- | --- | --- | --- | --- | --- |

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#a5ebb5f4ff34be5cbdff3f7bd422b2ba5).

## [◆](https://docs.atas.net/en/)HasSameCredentials()

| bool OFT.Core.IConnectorSettings.HasSameCredentials | ( | [IConnectorSettings](./interfaceOFT_1_1Core_1_1IConnectorSettings.md) | other | ) | |
| --- | --- | --- | --- | --- | --- |

Checks if this connector has the same credentials as another connector. Also verifies that the connector types match.

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#abce12adb18c42b941570c373da3ba335).

## Property Documentation

## [◆](https://docs.atas.net/en/)Description

| string OFT.Core.IConnectorSettings.Description |
| --- |

get

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#aa7964cb1732ff9ae759a5ecb10343b9c).

## [◆](https://docs.atas.net/en/)Features

| [ConnectorFeatures](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019) OFT.Core.IConnectorSettings.Features |
| --- |

get

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#a1010d33c63b1154ac4099f24561cb864).

## [◆](https://docs.atas.net/en/)Id

| Guid OFT.Core.IConnectorSettings.Id |
| --- |

getset

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#a121eeac7e14005a2aa79ee7c7a02c0f8).

## [◆](https://docs.atas.net/en/)IsAutoConnectEnabled

| bool OFT.Core.IConnectorSettings.IsAutoConnectEnabled |
| --- |

getset

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#a330cf0085058e7a99179bd423dbe7bf1).

## [◆](https://docs.atas.net/en/)IsDemo

| bool OFT.Core.IConnectorSettings.IsDemo |
| --- |

get

Indicates that connector uses TestNet environment.

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#a406b3e929d74b6a8e44e544866d51b27).

## [◆](https://docs.atas.net/en/)IsMarketDataEnabled

| bool OFT.Core.IConnectorSettings.IsMarketDataEnabled |
| --- |

getset

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#aa7ce11ce2a70fe4413712e51c277c7fd).

## [◆](https://docs.atas.net/en/)Logo

| Uri OFT.Core.IConnectorSettings.Logo |
| --- |

get

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#ab1ff5a3a64f5c0e10e07313256a6a0fe).

## [◆](https://docs.atas.net/en/)MarketDataDelayPeriod

| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) OFT.Core.IConnectorSettings.MarketDataDelayPeriod |
| --- |

get

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#ad4ff74d836fceb547c4ecb36df0ba616).

## [◆](https://docs.atas.net/en/)Name

| string OFT.Core.IConnectorSettings.Name |
| --- |

getset

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#a21cdce64988b39256ca30275d4e7c8b7).

## [◆](https://docs.atas.net/en/)SettingsTypes

| [ConnectorSettingsTypes](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930) OFT.Core.IConnectorSettings.SettingsTypes |
| --- |

get

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#acd1c795994212a6f85412d3741355e1f).

## [◆](https://docs.atas.net/en/)Type

| string OFT.Core.IConnectorSettings.Type |
| --- |

get

Implemented in [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md#ad75d9686288735b64f1efb8a64b5adc0).

The documentation for this interface was generated from the following file:
- [IConnectorSettings.cs](../files/IConnectorSettings_8cs.md)
