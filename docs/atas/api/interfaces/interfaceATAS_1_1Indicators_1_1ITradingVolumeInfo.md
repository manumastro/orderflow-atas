# ATAS.Indicators.ITradingVolumeInfo Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.html

Interface for providing information about the volume selector.
 [More...](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#details)

| Public Member Functions | |
| --- | --- |
| void | [SetCurrentVolumeItem](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#aa3e49c0a5e6e116afc259b0867486fa7) ([IVolumeSelectorItem](./interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md)? settings) |
| | Method that allows to set particular volume template as selected. |
| | |

| Properties | |
| --- | --- |
| string | [Currency](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#af45315269414a99ba5155aa5cff71e12)`[get]` |
| | Selected currency (mode) of trading volume. |
| | |
| decimal | [ActualVolume](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#a00b5a7f1bc295628b1cda365b811ea81)`[get]` |
| | Actual selected volume. |
| | |
| string | [VolumeFormat](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#a43f9ec92aa3973557a4a81c4b5033321)`[get]` |
| | Get format volume for display. |
| | |
| bool | [IsCustomVolumeSelected](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#a6bee5e0e36ccb47b3c85d797c34e775d)`[get]` |
| | Shows whether custom volume is entered or volume template is selected. |
| | |
| [IVolumeSelectorItem](./interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md)?[] | [VolumeItems](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#acf7e69a840003b2eb1f8fd18710da13d)`[get]` |
| | Collection of volume templates. |
| | |
| [IVolumeSelectorItem](./interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md)? | [CurrentVolumeItem](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#a2445a090b4fc882691af65ab6a6222cc)`[get]` |
| | Selected volume template. |
| | |

| Events | |
| --- | --- |
| Action | [TradingVolumeInfoChanged](./interfaceATAS_1_1Indicators_1_1ITradingVolumeInfo.md#a490b8ad61570fcd900f148d4aae1347a) |
| | Event is raised when volume settings changed. |
| | |

## Detailed Description

Interface for providing information about the volume selector.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)SetCurrentVolumeItem()

| void ATAS.Indicators.ITradingVolumeInfo.SetCurrentVolumeItem | ( | [IVolumeSelectorItem](./interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md)? | settings | ) | |
| --- | --- | --- | --- | --- | --- |

Method that allows to set particular volume template as selected.

Parameters

| settings | |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)ActualVolume

| decimal ATAS.Indicators.ITradingVolumeInfo.ActualVolume |
| --- |

get

Actual selected volume.

## [◆](https://docs.atas.net/en/)Currency

| string ATAS.Indicators.ITradingVolumeInfo.Currency |
| --- |

get

Selected currency (mode) of trading volume.

## [◆](https://docs.atas.net/en/)CurrentVolumeItem

| [IVolumeSelectorItem](./interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md)? ATAS.Indicators.ITradingVolumeInfo.CurrentVolumeItem |
| --- |

get

Selected volume template.

## [◆](https://docs.atas.net/en/)IsCustomVolumeSelected

| bool ATAS.Indicators.ITradingVolumeInfo.IsCustomVolumeSelected |
| --- |

get

Shows whether custom volume is entered or volume template is selected.

## [◆](https://docs.atas.net/en/)VolumeFormat

| string ATAS.Indicators.ITradingVolumeInfo.VolumeFormat |
| --- |

get

Get format volume for display.

## [◆](https://docs.atas.net/en/)VolumeItems

| [IVolumeSelectorItem](./interfaceATAS_1_1Indicators_1_1IVolumeSelectorItem.md)? [] ATAS.Indicators.ITradingVolumeInfo.VolumeItems |
| --- |

get

Collection of volume templates.

## Event Documentation

## [◆](https://docs.atas.net/en/)TradingVolumeInfoChanged

| Action ATAS.Indicators.ITradingVolumeInfo.TradingVolumeInfoChanged |
| --- |

Event is raised when volume settings changed.

The documentation for this interface was generated from the following file:
- [IIndicatorContainer.cs](../files/IIndicatorContainer_8cs.md)
