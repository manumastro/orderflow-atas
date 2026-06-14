# OFT.Attributes.Editors.SoundComboBoxEditorAttribute Class Reference

Source: https://docs.atas.net/en/classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.html

Specialized ComboBox editor for internal application sounds. Renders non-editable list with a Play button in item template.
 [More...](./classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.md#details)

Inheritance diagram for OFT.Attributes.Editors.SoundComboBoxEditorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for OFT.Attributes.Editors.SoundComboBoxEditorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [SoundComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.md#aa9de42c8e03d1b5a6783cc3f28ec27af) () |
| | Create sound combo editor. ItemsSource will be supplied by editor behavior. |
| | |
| | [SoundComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.md#a493f06f9c12e3abbe26290ea31528062) (Type itemsSource) |
| | Create sound combo editor with custom source type. |
| | |
| - Public Member Functions inherited from [OFT.Attributes.Editors.ComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md) | |
| | [ComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a34a03b343342294dbbb44a693f3225d9) (params object[] itemsSource) |
| | Configure ComboBox editor attribute. |
| | |
| | [ComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#a12dc1e95356ec7656c291e1f751bb2fa) (Type typeSource) |
| | Configure ComboBox editor attribute. |
| | |
| IEnumerable | [GetItemsSource](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md#af959501145495b197a95fea22f2828e9) (object instance) |
| | |

| Additional Inherited Members | |
| --- | --- |
| - Properties inherited from [OFT.Attributes.Editors.ComboBoxEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md) | |
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

## Detailed Description

Specialized ComboBox editor for internal application sounds. Renders non-editable list with a Play button in item template.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)SoundComboBoxEditorAttribute() [1/2]

| OFT.Attributes.Editors.SoundComboBoxEditorAttribute.SoundComboBoxEditorAttribute | ( | | ) | |
| --- | --- | --- | --- | --- |

Create sound combo editor. ItemsSource will be supplied by editor behavior.

## [◆](https://docs.atas.net/en/)SoundComboBoxEditorAttribute() [2/2]

| OFT.Attributes.Editors.SoundComboBoxEditorAttribute.SoundComboBoxEditorAttribute | ( | Type | itemsSource | ) | |
| --- | --- | --- | --- | --- | --- |

Create sound combo editor with custom source type.

The documentation for this class was generated from the following file:
- [SoundComboBoxEditorAttribute.cs](../files/SoundComboBoxEditorAttribute_8cs.md)
