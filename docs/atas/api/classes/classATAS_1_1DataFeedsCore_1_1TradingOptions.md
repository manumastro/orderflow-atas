# ATAS.DataFeedsCore.TradingOptions Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1TradingOptions.html

Inheritance diagram for ATAS.DataFeedsCore.TradingOptions:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

Collaboration diagram for ATAS.DataFeedsCore.TradingOptions:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| | [TradingOptions](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a1922ce5064cd5fb1182d2a3dd05d740f) () |
| | |
| override string | [ToString](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ae24d3a9df44b044c6fb7c839f8533e4a) () |
| | Returns a string that represents the current object. |
| | |

| Protected Member Functions | |
| --- | --- |
| virtual void | [OnPropertyChanged](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a6ae15dc9d4ad8c1986c9106018670205) (string propertyName) |
| | |

| Properties | |
| --- | --- |
| long | [Id](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#af6b8d4460c9f67abdb89d6d4014064d8)`[get, set]` |
| | |
| long? | [CommissionId](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a6af1c2249efaff91710f4af609327466)`[get, set]` |
| | |
| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) | [Commission](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a6d03d587a17fc5f3c5035ea62853f8db)`[get, set]` |
| | |
| TimeSpan | [ResetPnlTime](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ac699c62a9298a0b0514f214e7837b0f4)`[get, set]` |
| | |
| [SecurityRouteCache](./classATAS_1_1DataFeedsCore_1_1SecurityRouteCache.md) | [SecurityRoutes](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ac341936d129356fb71e4de86165c21ae)`[get]` |
| | |
| TimeSpan | [SessionBeginTime](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a0fc5c38e5b281d5929f77af6cbe8f3c4)`[get, set]` |
| | |
| TimeSpan | [SessionEndTime](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a820651590614c6307e859acc24bcb5d6)`[get, set]` |
| | |
| decimal | [Leverage](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a0141581d8b3f013f9268bfdaef54a1e1)`[get, set]` |
| | |
| decimal | [IntradayLeverage](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a9a6be970cf27e9b247afd177e320f5e2)`[get, set]` |
| | |
| bool | [IsIsolatedMargin](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a24f9c2bef3d4b02ef200bda1ddc228d5)`[get, set]` |
| | |
| List | [AvailableSecurities](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a5b45e4cdb39ed9abed60e082a0f65ac1)`[get, set]` |
| | |
| bool | [OvernightPositions](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ab0e932c8d739511353c7a665162c6189)`[get, set]` |
| | |
| int | [MaxPositions](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ade346125860d9171d51e8372eeb022d9)`[get, set]` |
| | |
| int | [MaxPositionSize](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ac933e9a717c7139bdea9e3c5af4e4e69)`[get, set]` |
| | |
| int | [MaxTotalPositionSize](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a92fa9617de928beb4a12788d1b11b3e4)`[get, set]` |
| | |
| int | [MaxOpenOrders](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#aab8280b42b69d65e3ff2d60429c0b221)`[get, set]` |
| | |
| int | [MaxOrderSize](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a2a10ae5af8285401b3d58f481d38a9fd)`[get, set]` |
| | |
| decimal | [BlockBalance](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a451d0f21179612d3e06c6752b31fbfc2)`[get, set]` |
| | |
| decimal | [MaxDrawdown](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a777ac7091ab7d9929e64181c78b84ce8)`[get, set]` |
| | |
| bool | [SuspendOnDrawdown](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a617273045538a1f340a83c380c2ecd24)`[get, set]` |
| | |
| decimal | [MaxUnrealizedPnL](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ab451df7a9d5ffaf244038fdca683589e)`[get, set]` |
| | |
| decimal | [TrailingDrawdown](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ab2fb31af4a393581f26688fc41026f44)`[get, set]` |
| | |
| bool | [ApplyOvernightSwap](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ad4b7fafe7c24128ed864d9b4fe3aa94a)`[get, set]` |
| | |
| bool | [StopEvaluationOnMaxTotalPositionSize](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#ac2458542be3ad254d164a4d6e537dad3)`[get, set]` |
| | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#acb4e85c549681c72c4dfbc1afefd7bd3)`[get]` |
| | Gets the type of the entity. |
| | |
| - Properties inherited from [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) | |
| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) | [EntityType](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c)`[get]` |
| | Gets the type of the entity. |
| | |

| Events | |
| --- | --- |
| PropertyChangedEventHandler | [PropertyChanged](./classATAS_1_1DataFeedsCore_1_1TradingOptions.md#a75d13214e500d445735eb4639016a4dc) |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)TradingOptions()

| ATAS.DataFeedsCore.TradingOptions.TradingOptions | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)OnPropertyChanged()

| virtual void ATAS.DataFeedsCore.TradingOptions.OnPropertyChanged | ( | string | propertyName | ) | |
| --- | --- | --- | --- | --- | --- |

protectedvirtual

## [◆](https://docs.atas.net/en/)ToString()

| override string ATAS.DataFeedsCore.TradingOptions.ToString | ( | | ) | |
| --- | --- | --- | --- | --- |

Returns a string that represents the current object.

ReturnsA string that represents the current object.

## Property Documentation

## [◆](https://docs.atas.net/en/)ApplyOvernightSwap

| bool ATAS.DataFeedsCore.TradingOptions.ApplyOvernightSwap |
| --- |

getset

## [◆](https://docs.atas.net/en/)AvailableSecurities

| List ATAS.DataFeedsCore.TradingOptions.AvailableSecurities |
| --- |

getset

## [◆](https://docs.atas.net/en/)BlockBalance

| decimal ATAS.DataFeedsCore.TradingOptions.BlockBalance |
| --- |

getset

## [◆](https://docs.atas.net/en/)Commission

| [CommissionGroup](./classATAS_1_1DataFeedsCore_1_1Commissions_1_1CommissionGroup.md) ATAS.DataFeedsCore.TradingOptions.Commission |
| --- |

getset

## [◆](https://docs.atas.net/en/)CommissionId

| long? ATAS.DataFeedsCore.TradingOptions.CommissionId |
| --- |

getset

## [◆](https://docs.atas.net/en/)EntityType

| [EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) ATAS.DataFeedsCore.TradingOptions.EntityType |
| --- |

get

Gets the type of the entity.

Implements [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#aac501c7a1be2e0616fe86ecbb1b74c5c).

## [◆](https://docs.atas.net/en/)Id

| long ATAS.DataFeedsCore.TradingOptions.Id |
| --- |

getset

## [◆](https://docs.atas.net/en/)IntradayLeverage

| decimal ATAS.DataFeedsCore.TradingOptions.IntradayLeverage |
| --- |

getset

## [◆](https://docs.atas.net/en/)IsIsolatedMargin

| bool ATAS.DataFeedsCore.TradingOptions.IsIsolatedMargin |
| --- |

getset

## [◆](https://docs.atas.net/en/)Leverage

| decimal ATAS.DataFeedsCore.TradingOptions.Leverage |
| --- |

getset

## [◆](https://docs.atas.net/en/)MaxDrawdown

| decimal ATAS.DataFeedsCore.TradingOptions.MaxDrawdown |
| --- |

getset

## [◆](https://docs.atas.net/en/)MaxOpenOrders

| int ATAS.DataFeedsCore.TradingOptions.MaxOpenOrders |
| --- |

getset

## [◆](https://docs.atas.net/en/)MaxOrderSize

| int ATAS.DataFeedsCore.TradingOptions.MaxOrderSize |
| --- |

getset

## [◆](https://docs.atas.net/en/)MaxPositions

| int ATAS.DataFeedsCore.TradingOptions.MaxPositions |
| --- |

getset

## [◆](https://docs.atas.net/en/)MaxPositionSize

| int ATAS.DataFeedsCore.TradingOptions.MaxPositionSize |
| --- |

getset

## [◆](https://docs.atas.net/en/)MaxTotalPositionSize

| int ATAS.DataFeedsCore.TradingOptions.MaxTotalPositionSize |
| --- |

getset

## [◆](https://docs.atas.net/en/)MaxUnrealizedPnL

| decimal ATAS.DataFeedsCore.TradingOptions.MaxUnrealizedPnL |
| --- |

getset

## [◆](https://docs.atas.net/en/)OvernightPositions

| bool ATAS.DataFeedsCore.TradingOptions.OvernightPositions |
| --- |

getset

## [◆](https://docs.atas.net/en/)ResetPnlTime

| TimeSpan ATAS.DataFeedsCore.TradingOptions.ResetPnlTime |
| --- |

getset

## [◆](https://docs.atas.net/en/)SecurityRoutes

| [SecurityRouteCache](./classATAS_1_1DataFeedsCore_1_1SecurityRouteCache.md) ATAS.DataFeedsCore.TradingOptions.SecurityRoutes |
| --- |

get

## [◆](https://docs.atas.net/en/)SessionBeginTime

| TimeSpan ATAS.DataFeedsCore.TradingOptions.SessionBeginTime |
| --- |

getset

## [◆](https://docs.atas.net/en/)SessionEndTime

| TimeSpan ATAS.DataFeedsCore.TradingOptions.SessionEndTime |
| --- |

getset

## [◆](https://docs.atas.net/en/)StopEvaluationOnMaxTotalPositionSize

| bool ATAS.DataFeedsCore.TradingOptions.StopEvaluationOnMaxTotalPositionSize |
| --- |

getset

## [◆](https://docs.atas.net/en/)SuspendOnDrawdown

| bool ATAS.DataFeedsCore.TradingOptions.SuspendOnDrawdown |
| --- |

getset

## [◆](https://docs.atas.net/en/)TrailingDrawdown

| decimal ATAS.DataFeedsCore.TradingOptions.TrailingDrawdown |
| --- |

getset

## Event Documentation

## [◆](https://docs.atas.net/en/)PropertyChanged

| PropertyChangedEventHandler ATAS.DataFeedsCore.TradingOptions.PropertyChanged |
| --- |

The documentation for this class was generated from the following file:
- [TradingOptions.cs](../files/TradingOptions_8cs.md)
