# OFT.Attributes.Editors Namespace Reference

Source: https://docs.atas.net/en/namespaceOFT_1_1Attributes_1_1Editors.html

| Classes | |
| --- | --- |
| class | [CheckEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1CheckEditorAttribute.md) |
| | Check box editor for object. [More...](../classes/classOFT_1_1Attributes_1_1Editors_1_1CheckEditorAttribute.md#details) |
| | |
| class | [ComboBoxEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md) |
| | |
| class | [DataSeriesEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1DataSeriesEditorAttribute.md) |
| | |
| class | [IsExpandedAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1IsExpandedAttribute.md) |
| | |
| class | [MaskAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1MaskAttribute.md) |
| | |
| class | [NumericEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1NumericEditorAttribute.md) |
| | |
| class | [PostValueModeAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1PostValueModeAttribute.md) |
| | |
| class | [SelectDirectoryEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SelectDirectoryEditorAttribute.md) |
| | |
| class | [SelectFileEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SelectFileEditorAttribute.md) |
| | |
| class | [SoundComboBoxEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.md) |
| | Specialized ComboBox editor for internal application sounds. Renders non-editable list with a Play button in item template. [More...](../classes/classOFT_1_1Attributes_1_1Editors_1_1SoundComboBoxEditorAttribute.md#details) |
| | |
| class | [TextEditorAttribute](../classes/classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md) |
| | Represents an attribute that provides additional metadata for controlling text editors. [More...](../classes/classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md#details) |
| | |

| Enumerations | |
| --- | --- |
| enum | [MaskTypes](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7) {
  [None](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a6adf97f83acf6453d4a6a4b1070f3754)
, [DateTime](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)
, [DateTimeAdvancingCaret](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a31dcaae13d8466e25df3c9afee15c11f)
, [Numeric](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a87322391cc6e8948ce9fd5d6cb84fced)
,
  [RegEx](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7aaa3c0312d71dacb7f28dd70f21d32ac0)
, [Regular](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7ad2203cb1237cb6460cbad94564e39345)
, [Simple](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a1fbb1e3943c2c6c560247ac8f9289780)
, [TimeSpan](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb)
,
  [TimeSpanAdvancingCaret](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7ab303ecbfc24d8bcfc52f6727aacf2ad1)

 } |
| | |
| enum | [NumericEditorTypes](./namespaceOFT_1_1Attributes_1_1Editors.md#a3b5976cee38b19f8c739cc72a1834617) { [Spin](./namespaceOFT_1_1Attributes_1_1Editors.md#a3b5976cee38b19f8c739cc72a1834617af5a6a925d4084ae58bd71a8a95a84ba7)
, [TrackBar](./namespaceOFT_1_1Attributes_1_1Editors.md#a3b5976cee38b19f8c739cc72a1834617aa603a787b802d9bb7b04da362fc4b84b)
 } |
| | |
| enum | [PostValueModes](./namespaceOFT_1_1Attributes_1_1Editors.md#a77a09d7fc6bc81a308341701736115a6) { [OnChanged](./namespaceOFT_1_1Attributes_1_1Editors.md#a77a09d7fc6bc81a308341701736115a6a5d4f2091f024eea690835f452eb2b817)
, [OnLostFocus](./namespaceOFT_1_1Attributes_1_1Editors.md#a77a09d7fc6bc81a308341701736115a6a6ffa285c9afa63f6cc12792dad885b2c)
, [Delayed](./namespaceOFT_1_1Attributes_1_1Editors.md#a77a09d7fc6bc81a308341701736115a6a6e04a0730cfc0bca398610196b5f8467)
 } |
| | |
| enum | [SelectAllOnModes](./namespaceOFT_1_1Attributes_1_1Editors.md#ab45710f2a120e42e370f1ce72708b5d4) { [None](./namespaceOFT_1_1Attributes_1_1Editors.md#ab45710f2a120e42e370f1ce72708b5d4a6adf97f83acf6453d4a6a4b1070f3754)
, [GotFocus](./namespaceOFT_1_1Attributes_1_1Editors.md#ab45710f2a120e42e370f1ce72708b5d4a1b5e06777fc97a0645bfe375f0fa3fe4)
, [MouseUp](./namespaceOFT_1_1Attributes_1_1Editors.md#ab45710f2a120e42e370f1ce72708b5d4a5c55840fe2a83a886590c780f0aa7031)
 } |
| | |
| enum | [SelectFileTypes](./namespaceOFT_1_1Attributes_1_1Editors.md#a53b08c385542b25f9eba77b526730789) { [Open](./namespaceOFT_1_1Attributes_1_1Editors.md#a53b08c385542b25f9eba77b526730789ac3bf447eabe632720a3aa1a7ce401274)
, [Save](./namespaceOFT_1_1Attributes_1_1Editors.md#a53b08c385542b25f9eba77b526730789ac9cc8cce247e49bae79f15173ce97354)
 } |
| | |

## Enumeration Type Documentation

## [◆](https://docs.atas.net/en/)MaskTypes

| enum [OFT.Attributes.Editors.MaskTypes](./namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7) |
| --- |

| Enumerator | |
| --- | --- |
| None | |
| DateTime | |
| DateTimeAdvancingCaret | |
| Numeric | |
| RegEx | |
| Regular | |
| Simple | |
| TimeSpan | |
| TimeSpanAdvancingCaret | |

## [◆](https://docs.atas.net/en/)NumericEditorTypes

| enum [OFT.Attributes.Editors.NumericEditorTypes](./namespaceOFT_1_1Attributes_1_1Editors.md#a3b5976cee38b19f8c739cc72a1834617) |
| --- |

| Enumerator | |
| --- | --- |
| Spin | |
| TrackBar | |

## [◆](https://docs.atas.net/en/)PostValueModes

| enum [OFT.Attributes.Editors.PostValueModes](./namespaceOFT_1_1Attributes_1_1Editors.md#a77a09d7fc6bc81a308341701736115a6) |
| --- |

| Enumerator | |
| --- | --- |
| OnChanged | |
| OnLostFocus | |
| Delayed | |

## [◆](https://docs.atas.net/en/)SelectAllOnModes

| enum [OFT.Attributes.Editors.SelectAllOnModes](./namespaceOFT_1_1Attributes_1_1Editors.md#ab45710f2a120e42e370f1ce72708b5d4) |
| --- |

| Enumerator | |
| --- | --- |
| None | |
| GotFocus | |
| MouseUp | |

## [◆](https://docs.atas.net/en/)SelectFileTypes

| enum [OFT.Attributes.Editors.SelectFileTypes](./namespaceOFT_1_1Attributes_1_1Editors.md#a53b08c385542b25f9eba77b526730789) |
| --- |

| Enumerator | |
| --- | --- |
| Open | |
| Save | |
