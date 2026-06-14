# ATAS.Strategies.StrategyNotificationEventArgs Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.html

Provides data for the StrategyNotification event.
 [More...](./classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md#details)

Inheritance diagram for ATAS.Strategies.StrategyNotificationEventArgs:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.StrategyNotificationEventArgs:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [StrategyNotificationEventArgs](./classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md#adbcb8b49fec506cf7dcac10a040ab48d) ([IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) strategy, string message, string title, bool isError) |
| | Initializes a new instance of the StrategyNotificationEventArgs class with the specified strategy, message, title, and error status. |
| | |

| Properties | |
| --- | --- |
| [IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | [Strategy](./classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md#adc11d3792e8523a827e348d89dde5016)`[get]` |
| | Gets the strategy associated with the notification. |
| | |
| string | [Message](./classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md#ab693d8f75a38c40d103e38c04b28fe7b)`[get]` |
| | Gets the message of the notification. |
| | |
| string | [Title](./classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md#a27f5b66a868ef83088ace4cc23475ca0)`[get]` |
| | Gets the title of the notification. |
| | |
| bool | [IsError](./classATAS_1_1Strategies_1_1StrategyNotificationEventArgs.md#a2036af550d0dec7aedad6ac283ce347f)`[get]` |
| | Gets a value indicating whether the notification is an error. |
| | |

## Detailed Description

Provides data for the StrategyNotification event.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)StrategyNotificationEventArgs()

| ATAS.Strategies.StrategyNotificationEventArgs.StrategyNotificationEventArgs | ( | [IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | strategy, |
| --- | --- | --- | --- |
| | | string | message, |
| | | string | title, |
| | | bool | isError |
| | ) | | |

Initializes a new instance of the StrategyNotificationEventArgs class with the specified strategy, message, title, and error status.

Parameters

| strategy | The strategy associated with the notification. |
| --- | --- |
| message | The message of the notification. |
| title | The title of the notification. |
| isError | A value indicating whether the notification is an error. |

## Property Documentation

## [◆](https://docs.atas.net/en/)IsError

| bool ATAS.Strategies.StrategyNotificationEventArgs.IsError |
| --- |

get

Gets a value indicating whether the notification is an error.

## [◆](https://docs.atas.net/en/)Message

| string ATAS.Strategies.StrategyNotificationEventArgs.Message |
| --- |

get

Gets the message of the notification.

## [◆](https://docs.atas.net/en/)Strategy

| [IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) ATAS.Strategies.StrategyNotificationEventArgs.Strategy |
| --- |

get

Gets the strategy associated with the notification.

## [◆](https://docs.atas.net/en/)Title

| string ATAS.Strategies.StrategyNotificationEventArgs.Title |
| --- |

get

Gets the title of the notification.

The documentation for this class was generated from the following file:
- [StrategyNotificationEventArgs.cs](../files/StrategyNotificationEventArgs_8cs.md)
