# ATAS.DataFeedsCore.BaseMarketDataOnlyConnectorSettings< T, TSelf > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1BaseMarketDataOnlyConnectorSettings.html

Inheritance diagram for ATAS.DataFeedsCore.BaseMarketDataOnlyConnectorSettings< T, TSelf >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.BaseMarketDataOnlyConnectorSettings< T, TSelf >:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Properties | |
| --- | --- |
| bool | [MarketDataOnly](./classATAS_1_1DataFeedsCore_1_1BaseMarketDataOnlyConnectorSettings.md#aca9bfa31983c63a63369aadf3e530487)`[get, set]` |
| | |
| - Properties inherited from [OFT.Core.BaseConnectorSettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md) | |
| string | [Type](./classOFT_1_1Core_1_1BaseConnectorSettings.md#ad75d9686288735b64f1efb8a64b5adc0)`[get, set]` |
| | |
| string | [Description](./classOFT_1_1Core_1_1BaseConnectorSettings.md#aa7964cb1732ff9ae759a5ecb10343b9c)`[get]` |
| | |
| Uri | [Logo](./classOFT_1_1Core_1_1BaseConnectorSettings.md#ab1ff5a3a64f5c0e10e07313256a6a0fe)`[get]` |
| | |
| Guid | [Id](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a121eeac7e14005a2aa79ee7c7a02c0f8)`[get, set]` |
| | |
| virtual string | [DisplayName](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a719716a4982dace25b74d7a4a8d9525e)`[get, set]` |
| | |
| string | [Name](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a21cdce64988b39256ca30275d4e7c8b7)`[get, set]` |
| | |
| bool | [IsMarketDataEnabled](./classOFT_1_1Core_1_1BaseConnectorSettings.md#aa7ce11ce2a70fe4413712e51c277c7fd)`[get, set]` |
| | |
| bool | [IsAutoConnectEnabled](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a330cf0085058e7a99179bd423dbe7bf1)`[get, set]` |
| | |
| bool | [AllowUpdatePositionsPnL](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a2a4c0dce99c76ce008478e593ff6ebd5)`[get, set]` |
| | |
| TimeOnly? | [RefreshSecuritiesTime](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a5ec9545872ca4bc6a9c1a65253497b5e)`[get, set]` |
| | |
| abstract [ConnectorFeatures](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019) | [Features](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a1010d33c63b1154ac4099f24561cb864)`[get]` |
| | |
| virtual [ConnectorSettingsTypes](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930) | [SettingsTypes](./classOFT_1_1Core_1_1BaseConnectorSettings.md#acd1c795994212a6f85412d3741355e1f)`[get]` |
| | |
| virtual bool | [IsDemo](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a406b3e929d74b6a8e44e544866d51b27)`[get]` |
| | Indicates that connector uses TestNet environment. |
| | |
| virtual [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) | [MarketDataDelayPeriod](./classOFT_1_1Core_1_1BaseConnectorSettings.md#ad4ff74d836fceb547c4ecb36df0ba616)`[get]` |
| | |
| - Properties inherited from [OFT.Core.IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) | |
| string | [Type](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a6e2b52c5b03af7a1d4ea36a175837c4b)`[get]` |
| | |
| string | [Description](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#aa387744b45b96f59d1fe2b1a89196fad)`[get]` |
| | |
| Uri | [Logo](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a00012972d5cfa12d129500fda0b86c36)`[get]` |
| | |
| Guid | [Id](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#aa629abc0db0dd2b4c22ff92dedca62e7)`[get, set]` |
| | |
| string | [Name](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a77492fd5e2e9846a999116c8778c8c74)`[get, set]` |
| | |
| bool | [IsMarketDataEnabled](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#ae5cf4eb42ddcb93d40dc5f0b9f50c0b0)`[get, set]` |
| | |
| bool | [IsAutoConnectEnabled](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a4fde71e8c06422a9784cfb04f4c0356f)`[get, set]` |
| | |
| [ConnectorFeatures](../namespaces/namespaceOFT_1_1Core.md#af2025a4717ad000e29e44756c5d41019) | [Features](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#aea958afa6fb75b2eb87c7e18c8b32848)`[get]` |
| | |
| [ConnectorSettingsTypes](../namespaces/namespaceOFT_1_1Core.md#a211b16aedd0f4534285b320ed8245930) | [SettingsTypes](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a1809158aa2742ace16e6fcb05b89eb6f)`[get]` |
| | |
| bool | [IsDemo](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#addb337c18a6a8e63afed2f73f586e23e)`[get]` |
| | Indicates that connector uses TestNet environment. |
| | |
| [MarketDataDelayPeriods](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ac90a735476b8fa274f20b591558ee52a) | [MarketDataDelayPeriod](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a01520d683c4716a505172f02a34a0fa5)`[get]` |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.ISupportMarketDataOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportMarketDataOnly.md) | |
| bool | [MarketDataOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportMarketDataOnly.md#a1917b9eb4da0338e6a3ae17d914d1a39)`[get, set]` |
| | |

| Additional Inherited Members | | |
| --- | --- | --- |
| - Public Member Functions inherited from [OFT.Core.BaseConnectorSettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md) | | |
| [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) | [CreateConnector](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a5ebb5f4ff34be5cbdff3f7bd422b2ba5) (string dataPath) | |
| | | |
| void | [ApplySettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md#ab14cde7bd5ff0a5ec3e1841f9f0b0cc8) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector) | |
| | | |
| virtual bool | [CheckSupported](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a6c550e8cb2653d0c0d9cbf643d09eea7) (out string? errorMessage) | |
| | Checks if connector is supported on this machine.Parameters

 errorMessage | Null if supported otherwise contains error text |

Returnstrue if no problems detected, false if not supported

bool [HasSameCredentials](./classOFT_1_1Core_1_1BaseConnectorSettings.md#abce12adb18c42b941570c373da3ba335) ([IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) other)
 Checks if this connector has the same credentials as another connector. Also verifies that the connector types match.

override string [ToString](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a837f81dca5225f40e79bb280df68b15b) ()

[IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) [CreateConnector](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a91ba23e01cfb95836252cd791eff242b) (string dataPath)

void [ApplySettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a22fe45709c709ca8055ef15a6b721b7a) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector)

bool [CheckSupported](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#ae160be2ff12e19c9d2f9483a2ac74ff7) (out string? errorMessage)
 Checks if connector is supported on this machine.

bool [HasSameCredentials](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md#a078c24514424d16226bd1382d3a94a22) ([IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) other)
 Checks if this connector has the same credentials as another connector. Also verifies that the connector types match.

- Protected Member Functions inherited from [OFT.Core.BaseConnectorSettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md)
 [BaseConnectorSettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md#ab6c4c0b50f9dbd20867d9b7e0981f284) ()

abstract bool [CompareCredentials](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a1c112130945ecba9a1b2f15a5c173f5f) ([IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) other)
 Compares credentials with another connector of the same type. The caller (HasSameCredentials) already guarantees that other is the same Type.

abstract [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) [OnCreateConnector](./classOFT_1_1Core_1_1BaseConnectorSettings.md#afbbdc076f7e3fe58076aeccadaed956d) (string dataPath)

abstract void [OnApplySettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md#af6e636a0381112b81493c5b7815f80fc) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector)

void [RaisePropertyChanged](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a6a38202ff0f84e1458266951da1a0b4e) (string propertyName)

bool [SetProperty](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a400af9cd1d3a482f88b5b133a00588ef) (ref TValue storage, TValue newValue, string propertyName, Action< TValue, TValue > onChanged=null)

override [IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) [OnCreateConnector](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a93f8b0f6399b1c7ead04ac0d5d4725d0) (string dataPath)

override void [OnApplySettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md#ab17d90542182a7a5ef1ed69cf40afecc) ([IDataFeedConnector](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IDataFeedConnector.md) connector)

sealed override bool [CompareCredentials](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a5f4a76fc22a80be8c9dca81824575d57) ([IConnectorSettings](../interfaces/interfaceOFT_1_1Core_1_1IConnectorSettings.md) other)

abstract void [OnApplySettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md#aec41ad8ebfb580913cc99ef3b6e78440) (T connector)

abstract bool [CompareCredentials](./classOFT_1_1Core_1_1BaseConnectorSettings.md#a980e150622049ebafd642e501fd2bf47) (TSelf other)

- Events inherited from [OFT.Core.BaseConnectorSettings](./classOFT_1_1Core_1_1BaseConnectorSettings.md)
PropertyChangedEventHandler [PropertyChanged](./classOFT_1_1Core_1_1BaseConnectorSettings.md#aef89abcb4e45886f9ae191df1802cda3)

## Property Documentation

## [◆](https://docs.atas.net/en/)MarketDataOnly

| bool [ATAS.DataFeedsCore.BaseMarketDataOnlyConnectorSettings](./classATAS_1_1DataFeedsCore_1_1BaseMarketDataOnlyConnectorSettings.md).MarketDataOnly |
| --- |

getset

Implements [ATAS.DataFeedsCore.ISupportMarketDataOnly](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1ISupportMarketDataOnly.md#a1917b9eb4da0338e6a3ae17d914d1a39).

The documentation for this class was generated from the following file:
- [ISupportMarketDataOnly.cs](../files/ISupportMarketDataOnly_8cs.md)
