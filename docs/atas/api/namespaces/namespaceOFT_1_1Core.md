# OFT.Core Namespace Reference

Source: https://docs.atas.net/en/namespaceOFT_1_1Core.html

| Classes | |
| --- | --- |
| class | [BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md) |
| | |
| class | ConnectorCategories |
| | |
| interface | [IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) |
| | |
| interface | [IConnectorSettingsAction](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsAction.md) |
| | |
| interface | [IConnectorSettingsSupportInitialization](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsSupportInitialization.md) |
| | |
| interface | [ILoginPasswordConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ILoginPasswordConnectorSettings.md) |
| | |
| interface | [ITypedConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ITypedConnectorSettings.md) |
| | |
| class | LoginPasswordConnectorSettingsExtensions |
| | |

| Enumerations | |
| --- | --- |
| enum | [ConnectorFeatures](./namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019) {
  [None](./namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019a6adf97f83acf6453d4a6a4b1070f3754) = 0x0
, [MarketData](./namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019ac2230c6e4d01b8865ebc4ad0aef9db94) = 0x1
, [Executions](./namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019a936770f2606e780ce536b4f29068542d) = 0x2
, [StopOrders](./namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019a69a6a0b658ab2f65cfa8492470f99f89) = 0x4
,
  [OcoOrders](./namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019abe6ba9cfe43f3147e2a3be0673645bf1) = 0x8

 } |
| | |
| enum | [ConnectorSettingsTypes](./namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930) { [None](./namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930a6adf97f83acf6453d4a6a4b1070f3754) = 0x00
, [Description](./namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930ab5a7adde1af5c87d7fd797b6245c2a39) = 0x01
, [Properties](./namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930a9fc2d28c05ed9eb1d75ba4465abf15a9) = 0x02
 } |
| | |
| enum | [ConnectorSettingsPostActions](./namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6) { [None](./namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6a6adf97f83acf6453d4a6a4b1070f3754) = 0x0
, [ShowMessage](./namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6a7784f68c858baf6399ab8d2c195543d6) = 0x1
, [SaveSettings](./namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6a30497d1ba1788f663cb425b24a60b7e1) = 0x2
, [UpdateTitle](./namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6ad56173887aad95a708ec5debd2db1cb8) = 0x4
 } |
| | |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)ConnectorFeatures

| enum [OFT.Core.ConnectorFeatures](./namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019) |
| --- |

| Enumerator | |
| --- | --- |
| None | |
| MarketData | |
| Executions | |
| StopOrders | |
| OcoOrders | |

## [◆](https://docs.atas.net/en/)ConnectorSettingsPostActions

| enum [OFT.Core.ConnectorSettingsPostActions](./namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6) |
| --- |

| Enumerator | |
| --- | --- |
| None | |
| ShowMessage | |
| SaveSettings | |
| UpdateTitle | |

## [◆](https://docs.atas.net/en/)ConnectorSettingsTypes

| enum [OFT.Core.ConnectorSettingsTypes](./namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930) |
| --- |

| Enumerator | |
| --- | --- |
| None | |
| Description | |
| Properties | |
