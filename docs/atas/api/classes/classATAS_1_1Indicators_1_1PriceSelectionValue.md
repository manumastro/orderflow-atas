# ATAS.Indicators.PriceSelectionValue Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1PriceSelectionValue.html

Represents a class for defining price level selection in clusters and bars. Using in PriceSelectionDataSeries.
 [More...](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#details)

| Public Member Functions | |
| --- | --- |
| | [PriceSelectionValue](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a289fc35b822201170d8aa0e7e8cdaec7) (decimal price) |
| | Constructor for creating a price selection with a given price. |
| | |
| Color | [GetPriceSelectionColor](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a944067f420d39d3de2c98c1682e73827) () |
| | Gets the current price selection color. |
| | |
| Color | [GetObjectsColor](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a93278a25dd0c6103a2927b457e6fc452) () |
| | Gets the current graphic objects color. |
| | |
| RenderPen | [GetBorderPen](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a4948d34bb27bf76205c5f0e5ef78fcbe) () |
| | Gets the render pen for the border of the graphic objects. |
| | |

| Properties | |
| --- | --- |
| decimal | [MinimumPrice](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a05fd22f8b02dde8fe1385f381807e9b8)`[get, set]` |
| | Minimum price of the selection. |
| | |
| decimal | [MaximumPrice](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a584983857133649d9eac5e87f90cfe6e)`[get, set]` |
| | Maximum price of the selection. |
| | |
| int | [Size](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#afc9f799b0bbe38b1d75af475641c1c54)`[get, set]` |
| | Graphic objects size. |
| | |
| string | [Tooltip](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a6e36d33ee98ba806c23563bc19ca2269) = string.Empty`[get, set]` |
| | Tooltip associated with the selection. |
| | |
| bool | [DrawValue](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a0800fac9d519660738d6f8e19c585c13)`[get, set]` |
| | Draw value inside object. |
| | |
| decimal? | [RenderValue](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#af996fde2347a7f9238d013fd121772bd)`[get, set]` |
| | Render value inside object. |
| | |
| [SelectionType](../namespaces/namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1d) | [SelectionSide](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a5654aaadf1197bff7de33e41e453b818)`[get, set]` |
| | Selection type. |
| | |
| [ObjectType](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86) | [VisualObject](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a0caf5cc7ca43c4a7253d008cf377077e)`[get, set]` |
| | Visual object type. |
| | |
| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | [PriceSelectionColor](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a887d37cd87d6cac51d3955f12dade5d6)`[get, set]` |
| | Color of the price selection. Use alpha channel for transparency. |
| | |
| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) | [ObjectColor](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a3e749a19cf62c84068681ff9e4682f88)`[get, set]` |
| | Color of the graphic objects. |
| | |
| decimal | [HeightFactor](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#ae56743bd2d73cd19e425c3419fa286bc)`[get, set]` |
| | Height Factor (obsolete). |
| | |
| object | [Context](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#ae178dfe52f6a2f4a59c6a166b101127c) = null!`[get, set]` |
| | Коэффициент уменьшения высоты выделения цены. Если значение 100, выделяется вся высота, если 50, выделяется половина |
| | |
| int | [ObjectsTransparency](./classATAS_1_1Indicators_1_1PriceSelectionValue.md#a7d69d33306a09f95647283fed38f8a4a)`[get, set]` |
| | Transparency of objects filling. |
| | |

## Detailed Description

Represents a class for defining price level selection in clusters and bars. Using in PriceSelectionDataSeries.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)PriceSelectionValue()

| ATAS.Indicators.PriceSelectionValue.PriceSelectionValue | ( | decimal | price | ) | |
| --- | --- | --- | --- | --- | --- |

Constructor for creating a price selection with a given price.

Parameters

| price | The price to set as both minimum and maximum. |
| --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetBorderPen()

| RenderPen ATAS.Indicators.PriceSelectionValue.GetBorderPen | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets the render pen for the border of the graphic objects.

ReturnsThe render pen for the border of the graphic objects.

## [◆](https://docs.atas.net/en/)GetObjectsColor()

| Color ATAS.Indicators.PriceSelectionValue.GetObjectsColor | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets the current graphic objects color.

ReturnsThe current graphic objects color.

## [◆](https://docs.atas.net/en/)GetPriceSelectionColor()

| Color ATAS.Indicators.PriceSelectionValue.GetPriceSelectionColor | ( | | ) | |
| --- | --- | --- | --- | --- |

Gets the current price selection color.

ReturnsThe current price selection color.

## Property Documentation

## [◆](https://docs.atas.net/en/)Context

| object ATAS.Indicators.PriceSelectionValue.Context = null! |
| --- |

getset

Коэффициент уменьшения высоты выделения цены. Если значение 100, выделяется вся высота, если 50, выделяется половина

Context object associated with the price selection.

## [◆](https://docs.atas.net/en/)DrawValue

| bool ATAS.Indicators.PriceSelectionValue.DrawValue |
| --- |

getset

Draw value inside object.

## [◆](https://docs.atas.net/en/)HeightFactor

| decimal ATAS.Indicators.PriceSelectionValue.HeightFactor |
| --- |

getset

Height Factor (obsolete).

## [◆](https://docs.atas.net/en/)MaximumPrice

| decimal ATAS.Indicators.PriceSelectionValue.MaximumPrice |
| --- |

getset

Maximum price of the selection.

## [◆](https://docs.atas.net/en/)MinimumPrice

| decimal ATAS.Indicators.PriceSelectionValue.MinimumPrice |
| --- |

getset

Minimum price of the selection.

## [◆](https://docs.atas.net/en/)ObjectColor

| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) ATAS.Indicators.PriceSelectionValue.ObjectColor |
| --- |

getset

Color of the graphic objects.

## [◆](https://docs.atas.net/en/)ObjectsTransparency

| int ATAS.Indicators.PriceSelectionValue.ObjectsTransparency |
| --- |

getset

Transparency of objects filling.

## [◆](https://docs.atas.net/en/)PriceSelectionColor

| [CrossColor](../files/Indicators_2GlobalUsings_8cs.md#a68883159d824f2cd02503c2b277c60e3) ATAS.Indicators.PriceSelectionValue.PriceSelectionColor |
| --- |

getset

Color of the price selection. Use alpha channel for transparency.

## [◆](https://docs.atas.net/en/)RenderValue

| decimal? ATAS.Indicators.PriceSelectionValue.RenderValue |
| --- |

getset

Render value inside object.

## [◆](https://docs.atas.net/en/)SelectionSide

| [SelectionType](../namespaces/namespaceATAS_1_1Indicators.md#a05615547547e7e7c7457e17727864e1d) ATAS.Indicators.PriceSelectionValue.SelectionSide |
| --- |

getset

Selection type.

## [◆](https://docs.atas.net/en/)Size

| int ATAS.Indicators.PriceSelectionValue.Size |
| --- |

getset

Graphic objects size.

## [◆](https://docs.atas.net/en/)Tooltip

| string ATAS.Indicators.PriceSelectionValue.Tooltip = string.Empty |
| --- |

getset

Tooltip associated with the selection.

## [◆](https://docs.atas.net/en/)VisualObject

| [ObjectType](../namespaces/namespaceATAS_1_1Indicators.md#a6467b89fe21b70bc206d9528e7f30d86) ATAS.Indicators.PriceSelectionValue.VisualObject |
| --- |

getset

Visual object type.

The documentation for this class was generated from the following file:
- [PriceSelectionValue.cs](../files/PriceSelectionValue_8cs.md)
