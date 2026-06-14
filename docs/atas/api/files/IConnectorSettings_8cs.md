# IConnectorSettings.cs File Reference

Source: https://docs.atas.net/en/IConnectorSettings_8cs.html

| Classes | |
| --- | --- |
| class | OFT.Core.ConnectorCategories |
| | |
| interface | [OFT.Core.IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) |
| | |
| interface | [OFT.Core.ILoginPasswordConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ILoginPasswordConnectorSettings.md) |
| | |
| class | OFT.Core.LoginPasswordConnectorSettingsExtensions |
| | |
| interface | [OFT.Core.IConnectorSettingsSupportInitialization](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsSupportInitialization.md) |
| | |
| interface | [OFT.Core.ITypedConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1ITypedConnectorSettings.md) |
| | |
| interface | [OFT.Core.IConnectorSettingsAction](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettingsAction.md) |
| | |
| class | [OFT.Core.BaseConnectorSettings](../classes/classOFT_1_1Core_1_1BaseConnectorSettings.md) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [OFT](../namespaces/namespaceOFT.md) |
| | |
| namespace | [OFT.Core](../namespaces/namespaceOFT_1_1Core.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [OFT.Core.ConnectorFeatures](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019) {
  [OFT.Core.None](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019a6adf97f83acf6453d4a6a4b1070f3754) = 0x0
, [OFT.Core.MarketData](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019ac2230c6e4d01b8865ebc4ad0aef9db94) = 0x1
, [OFT.Core.Executions](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019a936770f2606e780ce536b4f29068542d) = 0x2
, [OFT.Core.StopOrders](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019a69a6a0b658ab2f65cfa8492470f99f89) = 0x4
,
  [OFT.Core.OcoOrders](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019abe6ba9cfe43f3147e2a3be0673645bf1) = 0x8

 } |
| | |
| enum | [OFT.Core.ConnectorSettingsTypes](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930) { [OFT.Core.None](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930a6adf97f83acf6453d4a6a4b1070f3754) = 0x00
, [OFT.Core.Description](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930ab5a7adde1af5c87d7fd797b6245c2a39) = 0x01
, [OFT.Core.Properties](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930a9fc2d28c05ed9eb1d75ba4465abf15a9) = 0x02
 } |
| | |
| enum | [OFT.Core.ConnectorSettingsPostActions](../namespaces/namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6) { [OFT.Core.None](../namespaces/namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6a6adf97f83acf6453d4a6a4b1070f3754) = 0x0
, [OFT.Core.ShowMessage](../namespaces/namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6a7784f68c858baf6399ab8d2c195543d6) = 0x1
, [OFT.Core.SaveSettings](../namespaces/namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6a30497d1ba1788f663cb425b24a60b7e1) = 0x2
, [OFT.Core.UpdateTitle](../namespaces/namespaceOFT_1_1Core.md#a1dc06d6bd9a9c32a59b8e327b37688d6ad56173887aad95a708ec5debd2db1cb8) = 0x4
 } |
| | |
