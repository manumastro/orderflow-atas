# ATAS.Indicators.IPropertiesEditor Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.html

| Public Member Functions | |
| --- | --- |
| void | [BeginInit](./interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a3edac77a6ee218d221f107e1ba8b308d) () |
| | |
| Task | [EndInit](./interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a6e21a08b49908185288c444b1bd12f5f) () |
| | |
| void | [Refresh](./interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#ab20a9db6c45aaffd34d4fe5c91311ae3) () |
| | Refreshes the property grid to reflect changes in property visibility or values. |
| | |
| bool? | [GetIsExpandedCategory](./interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#ac1016fa47e0eccdcb93b2e102b05bb5c) (string? categoryName) |
| | |
| void | [SetIsExpandedCategory](./interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a309a0718abd12b1acd1baeec0c115e04) (string? categoryName, bool isExpanded) |
| | |
| bool? | [GetIsExpandedProperty](./interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a5a6d4982683cbe61c9a40995cdaad045) (string? propertyName) |
| | |
| void | [SetIsExpandedProperty](./interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#ac948316bbc13f15cbd2f0ece479c2d7f) (string? propertyName, bool isExpanded) |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)BeginInit()

| void ATAS.Indicators.IPropertiesEditor.BeginInit | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)EndInit()

| Task ATAS.Indicators.IPropertiesEditor.EndInit | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetIsExpandedCategory()

| bool? ATAS.Indicators.IPropertiesEditor.GetIsExpandedCategory | ( | string? | categoryName | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetIsExpandedProperty()

| bool? ATAS.Indicators.IPropertiesEditor.GetIsExpandedProperty | ( | string? | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)Refresh()

| void ATAS.Indicators.IPropertiesEditor.Refresh | ( | | ) | |
| --- | --- | --- | --- | --- |

Refreshes the property grid to reflect changes in property visibility or values.

## [◆](https://docs.atas.net/en/)SetIsExpandedCategory()

| void ATAS.Indicators.IPropertiesEditor.SetIsExpandedCategory | ( | string? | categoryName, |
| --- | --- | --- | --- |
| | | bool | isExpanded |
| | ) | | |

## [◆](https://docs.atas.net/en/)SetIsExpandedProperty()

| void ATAS.Indicators.IPropertiesEditor.SetIsExpandedProperty | ( | string? | propertyName, |
| --- | --- | --- | --- |
| | | bool | isExpanded |
| | ) | | |

The documentation for this interface was generated from the following file:
- [IPropertiesEditor.cs](../files/IPropertiesEditor_8cs.md)
