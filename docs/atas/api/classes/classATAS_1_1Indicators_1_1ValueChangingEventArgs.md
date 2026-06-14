# ATAS.Indicators.ValueChangingEventArgs< TValue > Class Template Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1ValueChangingEventArgs.html

Provides event arguments for a value changing event.
 [More...](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md#details)

| Public Member Functions | |
| --- | --- |
| | [ValueChangingEventArgs](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md#ac1ac7d153dd2e4ae52056cda2c2401c0) (TValue oldValue, TValue newValue) |
| | Initializes a new instance of the ValueChangingEventArgs class with the specified old and new values. |
| | |

| Properties | |
| --- | --- |
| TValue | [OldValue](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md#add9b6c3f864e246875911ccf53efab07)`[get]` |
| | Gets the old value before the change. |
| | |
| TValue | [NewValue](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md#abc03f9ff08aee4293d20da2792e8728e)`[get]` |
| | Gets the new value after the change. |
| | |

## Detailed Description

Provides event arguments for a value changing event.

Template Parameters

| TValue | The type of the value. |
| --- | --- |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)ValueChangingEventArgs()

| [ATAS.Indicators.ValueChangingEventArgs](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md).[ValueChangingEventArgs](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md) | ( | TValue | oldValue, |
| --- | --- | --- | --- |
| | | TValue | newValue |
| | ) | | |

Initializes a new instance of the ValueChangingEventArgs<TValue> class with the specified old and new values.

Parameters

| oldValue | The old value before the change. |
| --- | --- |
| newValue | The new value after the change. |

## Property Documentation

## [◆](https://docs.atas.net/en/)NewValue

| TValue [ATAS.Indicators.ValueChangingEventArgs](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md).NewValue |
| --- |

get

Gets the new value after the change.

## [◆](https://docs.atas.net/en/)OldValue

| TValue [ATAS.Indicators.ValueChangingEventArgs](./classATAS_1_1Indicators_1_1ValueChangingEventArgs.md).OldValue |
| --- |

get

Gets the old value before the change.

The documentation for this class was generated from the following file:
- [ValueChangingEventArgs.cs](../files/ValueChangingEventArgs_8cs.md)
