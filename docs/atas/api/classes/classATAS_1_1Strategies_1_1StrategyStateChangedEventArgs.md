# ATAS.Strategies.StrategyStateChangedEventArgs Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.html

Provides data for the StrategyStateChanged event.
 [More...](./classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md#details)

Inheritance diagram for ATAS.Strategies.StrategyStateChangedEventArgs:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.Strategies.StrategyStateChangedEventArgs:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [StrategyStateChangedEventArgs](./classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md#a6ffe985562030bde73746af066a515c4) ([IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) strategy, [StrategyStateDescription](../namespaces/namespaceATAS_1_1Strategies.md#ac2b219bf1e9f7c99f8f9fc4c5fe39ea3) state) |
| | Initializes a new instance of the StrategyStateChangedEventArgs class with the specified strategy, old state, and new state. |
| | |

| Properties | |
| --- | --- |
| [IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | [Strategy](./classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md#a2222a68c37b4c566b2fb81b082bb04dd)`[get]` |
| | Gets the strategy associated with the state change. |
| | |
| [StrategyStateDescription](../namespaces/namespaceATAS_1_1Strategies.md#ac2b219bf1e9f7c99f8f9fc4c5fe39ea3) | [State](./classATAS_1_1Strategies_1_1StrategyStateChangedEventArgs.md#a1752f23d3e5960068c17bb89891160ae)`[get]` |
| | Gets the state of the strategy. |
| | |

## Detailed Description

Provides data for the StrategyStateChanged event.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)StrategyStateChangedEventArgs()

| ATAS.Strategies.StrategyStateChangedEventArgs.StrategyStateChangedEventArgs | ( | [IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) | strategy, |
| --- | --- | --- | --- |
| | | [StrategyStateDescription](../namespaces/namespaceATAS_1_1Strategies.md#ac2b219bf1e9f7c99f8f9fc4c5fe39ea3) | state |
| | ) | | |

Initializes a new instance of the StrategyStateChangedEventArgs class with the specified strategy, old state, and new state.

Parameters

| strategy | The strategy associated with the state change. |
| --- | --- |
| state | The state of the strategy. |

## Property Documentation

## [◆](https://docs.atas.net/en/)State

| [StrategyStateDescription](../namespaces/namespaceATAS_1_1Strategies.md#ac2b219bf1e9f7c99f8f9fc4c5fe39ea3) ATAS.Strategies.StrategyStateChangedEventArgs.State |
| --- |

get

Gets the state of the strategy.

## [◆](https://docs.atas.net/en/)Strategy

| [IStrategy](../interfaces/interfaceATAS_1_1Strategies_1_1IStrategy.md) ATAS.Strategies.StrategyStateChangedEventArgs.Strategy |
| --- |

get

Gets the strategy associated with the state change.

The documentation for this class was generated from the following file:
- [StrategyStateChangedEventArgs.cs](../files/StrategyStateChangedEventArgs_8cs.md)
