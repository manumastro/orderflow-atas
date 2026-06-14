# OFT.Attributes.Editors.TextEditorAttribute Class Reference

Source: https://docs.atas.net/en/classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.html

Represents an attribute that provides additional metadata for controlling text editors.
 [More...](./classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md#details)

Inheritance diagram for OFT.Attributes.Editors.TextEditorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for OFT.Attributes.Editors.TextEditorAttribute:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [TextEditorAttribute](./classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md#a6c62d13da3e43658f26db0a5f03198e8) (bool isMultiline, int maxHeight=50) |
| | Initializes a new instance of the TextEditorAttribute class with the specified settings. |
| | |

| Properties | |
| --- | --- |
| bool | [IsMultiline](./classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md#a01b511dac0f1021937f5d8345a348348)`[get]` |
| | Gets a value indicating whether the text editor allows multiline input. |
| | |
| int | [MaxHeight](./classOFT_1_1Attributes_1_1Editors_1_1TextEditorAttribute.md#a14be569cedc41fb04648b8be4e198bfa)`[get]` |
| | Gets the maximum height of the text editor, in lines. |
| | |

## Detailed Description

Represents an attribute that provides additional metadata for controlling text editors.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)TextEditorAttribute()

| OFT.Attributes.Editors.TextEditorAttribute.TextEditorAttribute | ( | bool | isMultiline, |
| --- | --- | --- | --- |
| | | int | maxHeight = `50` |
| | ) | | |

Initializes a new instance of the TextEditorAttribute class with the specified settings.

Parameters

| isMultiline | A value indicating whether the text editor allows multiline input. |
| --- | --- |
| maxHeight | The maximum height of the text editor, in lines. It must be at least 10 lines. |

## Property Documentation

## [◆](https://docs.atas.net/en/)IsMultiline

| bool OFT.Attributes.Editors.TextEditorAttribute.IsMultiline |
| --- |

get

Gets a value indicating whether the text editor allows multiline input.

## [◆](https://docs.atas.net/en/)MaxHeight

| int OFT.Attributes.Editors.TextEditorAttribute.MaxHeight |
| --- |

get

Gets the maximum height of the text editor, in lines.

The documentation for this class was generated from the following file:
- [TextEditorAttribute.cs](../files/TextEditorAttribute_8cs.md)
