# ATAS.Indicators.IPlatformSettings Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IPlatformSettings.html

Interface for accessing platform settings.
 [More...](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md#details)

| Properties | |
| --- | --- |
| int | [ValueAreaPercent](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md#a606632e4fc7d9fd6b9f8c5139b597a49)`[get]` |
| | Gets the value representing the percentage of the value area, which is specified in the global settings of the platform. |
| | |
| int | [ValueAreaStep](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md#ab98ee7229573cb9a2d9be4b4e52030a7)`[get]` |
| | Gets the value area step size at each iteration. |
| | |
| int | [ValueAreaUpdateDelayMs](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md#aed35d741934aceb1760cf18133b6bed0)`[get]` |
| | Gets value area update frequency to reduce CPU usage on frequent candle update. In milliseconds. 0 means no cache and each update will force ValueArea recalculation. |
| | |
| string | [DataPath](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md#a6535f27f9198fff483462e6e07d4f848)`[get]` |
| | Gets the data path to the folder where the platform stores all the settings. It could be used for saving some service information by the indicator. |
| | |
| Version | [Version](./interfaceATAS_1_1Indicators_1_1IPlatformSettings.md#a657ba159281c6a02d8f12cfbd97e8fbb)`[get]` |
| | Gets the current version of platform. |
| | |

## Detailed Description

Interface for accessing platform settings.

## Property Documentation

## [◆](https://docs.atas.net/en/)DataPath

| string ATAS.Indicators.IPlatformSettings.DataPath |
| --- |

get

Gets the data path to the folder where the platform stores all the settings. It could be used for saving some service information by the indicator.

## [◆](https://docs.atas.net/en/)ValueAreaPercent

| int ATAS.Indicators.IPlatformSettings.ValueAreaPercent |
| --- |

get

Gets the value representing the percentage of the value area, which is specified in the global settings of the platform.

## [◆](https://docs.atas.net/en/)ValueAreaStep

| int ATAS.Indicators.IPlatformSettings.ValueAreaStep |
| --- |

get

Gets the value area step size at each iteration.

## [◆](https://docs.atas.net/en/)ValueAreaUpdateDelayMs

| int ATAS.Indicators.IPlatformSettings.ValueAreaUpdateDelayMs |
| --- |

get

Gets value area update frequency to reduce CPU usage on frequent candle update. In milliseconds. 0 means no cache and each update will force ValueArea recalculation.

## [◆](https://docs.atas.net/en/)Version

| Version ATAS.Indicators.IPlatformSettings.Version |
| --- |

get

Gets the current version of platform.

The documentation for this interface was generated from the following file:
- [IPlatformSettings.cs](../files/IPlatformSettings_8cs.md)
