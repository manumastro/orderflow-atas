# OFT.Core.IConnectorSettingsSupportInitialization Interface Reference

Source: https://docs.atas.net/en/interfaceOFT_1_1Core_1_1IConnectorSettingsSupportInitialization.html

Inheritance diagram for OFT.Core.IConnectorSettingsSupportInitialization:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for OFT.Core.IConnectorSettingsSupportInitialization:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [Initialize](./interfaceOFT_1_1Core_1_1IConnectorSettingsSupportInitialization.md#a6e2841af60a1c400da400776c94bda55) (string dataPath, bool throwErrors) |
| | |
| - Public Member Functions inherited from [OFT.Core.IConnectorSettings](./interfaceOFT_1_1Core_1_1IConnectorSettings.md) | |
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

| Additional Inherited Members | |
| --- | --- |
| - Properties inherited from [OFT.Core.IConnectorSettings](./interfaceOFT_1_1Core_1_1IConnectorSettings.md) | |
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

## [◆](https://docs.atas.net/en/)Initialize()

| void OFT.Core.IConnectorSettingsSupportInitialization.Initialize | ( | string | dataPath, |
| --- | --- | --- | --- |
| | | bool | throwErrors |
| | ) | | |

The documentation for this interface was generated from the following file:
- [IConnectorSettings.cs](../files/IConnectorSettings_8cs.md)
