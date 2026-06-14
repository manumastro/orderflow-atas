# OFT.Attributes.Editors.ComboBoxEditorAttribute Class Reference

Source: https://docs.atas.net/en/classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.html

Inheritance diagram for OFT.Attributes.Editors.ComboBoxEditorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for OFT.Attributes.Editors.ComboBoxEditorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [ComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a34a03b343342294dbbb44a693f3225d9) (params object[] itemsSource) |
| | Configure ComboBox editor attribute. |
| | |
| | [ComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a12dc1e95356ec7656c291e1f751bb2fa) (Type typeSource) |
| | Configure ComboBox editor attribute. |
| | |
| IEnumerable | [GetItemsSource](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#af959501145495b197a95fea22f2828e9) (object instance) |
| | |

| Properties | |
| --- | --- |
| string | [DisplayMember](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a5b4e0be45dff0d54e07723b0c6fdee99)`[get, set]` |
| | Gets or sets a member name in the bound data source whose values are displayed by the editor. |
| | |
| string | [ValueMember](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a16853bcc9eeda8878f2eae7a22d1c2e9)`[get, set]` |
| | Gets or sets a member name in the bound data source, whose values are assigned to item values. |
| | |
| object[] | [ItemsSource](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a8e6a06f2b623eafb79eb21287f253215)`[get, set]` |
| | Get or set data source for selection. |
| | |
| bool | [IsTextEditable](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#af9b0252deb28733ff6ed58687d67cbaf)`[get, set]` |
| | Gets or sets whether end-users are allowed to edit the text displayed in the edit box. |
| | |
| bool | [AutoComplete](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a98123e1222792619bb1c3b0fe9ba7c34)`[get, set]` |
| | Gets or sets whether the automatic completion is enabled. |
| | |
| bool | [SelectItemWithNullValue](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a195071a6b63287989e38e58e13a335b6)`[get, set]` |
| | Gets or sets whether the editor searches for a null item in the bound data source when the editor value is null (empty). |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ComboBoxEditorAttribute() [1/2]

| OFT.Attributes.Editors.ComboBoxEditorAttribute.ComboBoxEditorAttribute | ( | params object[] | itemsSource | ) | |
| --- | --- | --- | --- | --- | --- |

Configure ComboBox editor attribute.

Parameters

| itemsSource | Elements dor selection in ComboBox. |
| --- | --- |

## [◆](https://docs.atas.net/en/)ComboBoxEditorAttribute() [2/2]

| OFT.Attributes.Editors.ComboBoxEditorAttribute.ComboBoxEditorAttribute | ( | Type | typeSource | ) | |
| --- | --- | --- | --- | --- | --- |

Configure ComboBox editor attribute.

Parameters

| typeSource | Source type must inherit from T:System.Collections.IEnumerable. |
| --- | --- |

Exceptions

| T:System.ArgumentNullException | Parameter Parameters

 typeSource | is null. |
| --- | --- | --- |

Exceptions

| T:System.ArgumentException | Parameter Parameters

 typeSource | not inherited from T:System.Collections.IEnumerable or constructor has more than one parameter. |
| --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)GetItemsSource()

| IEnumerable OFT.Attributes.Editors.ComboBoxEditorAttribute.GetItemsSource | ( | object | instance | ) | |
| --- | --- | --- | --- | --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)AutoComplete

| bool OFT.Attributes.Editors.ComboBoxEditorAttribute.AutoComplete |
| --- |

getset

Gets or sets whether the automatic completion is enabled.

## [◆](https://docs.atas.net/en/)DisplayMember

| string OFT.Attributes.Editors.ComboBoxEditorAttribute.DisplayMember |
| --- |

getset

Gets or sets a member name in the bound data source whose values are displayed by the editor.

## [◆](https://docs.atas.net/en/)IsTextEditable

| bool OFT.Attributes.Editors.ComboBoxEditorAttribute.IsTextEditable |
| --- |

getset

Gets or sets whether end-users are allowed to edit the text displayed in the edit box.

## [◆](https://docs.atas.net/en/)ItemsSource

| object [] OFT.Attributes.Editors.ComboBoxEditorAttribute.ItemsSource |
| --- |

getset

Get or set data source for selection.

## [◆](https://docs.atas.net/en/)SelectItemWithNullValue

| bool OFT.Attributes.Editors.ComboBoxEditorAttribute.SelectItemWithNullValue |
| --- |

getset

Gets or sets whether the editor searches for a null item in the bound data source when the editor value is null (empty).

## [◆](https://docs.atas.net/en/)ValueMember

| string OFT.Attributes.Editors.ComboBoxEditorAttribute.ValueMember |
| --- |

getset

Gets or sets a member name in the bound data source, whose values are assigned to item values.

The documentation for this class was generated from the following file:
- [ComboBoxEditorAttribute.cs](../files/ComboBoxEditorAttribute_8cs.md)
