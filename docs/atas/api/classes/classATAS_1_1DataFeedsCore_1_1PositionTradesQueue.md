# ATAS.DataFeedsCore.PositionTradesQueue Class Reference

Source: https://docs.atas.net/en/classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.html

| Public Member Functions | |
| --- | --- |
| | [PositionTradesQueue](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a2d14ca3f2a57f45017862a851e669715) (ILoggerSource logger, [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) portfolio, [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) security, decimal openVolume) |
| | |
| | [PositionTradesQueue](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a4a55d7271a745dc32ff40f6529c1f446) (ILoggerSource logger, [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) position) |
| | |
| void | [Add](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a26651ad8f67df82a79290b7b17ce32bd) ([MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) newTrade) |
| | |
| void | [AddTrade](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a906f5d20a746c81962d3207f443cf9c3) (bool isBuy, decimal price, decimal [volume](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a417aa64e37f65206a52e6eb387023840)) |
| | |
| void | [AddTrade](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a2104e85d4cbc95d3a9ccfe5945223c52) (decimal price, decimal [volume](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a417aa64e37f65206a52e6eb387023840)) |
| | |
| bool | [CalculateAveragePrice](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a27c4310226c6bd283786670ca7050c2a) (decimal [volume](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a417aa64e37f65206a52e6eb387023840), bool checkTotalVolume, out decimal averagePrice) |
| | |
| void | [Clear](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a561b702e14b08f58249d0fff0aed5b2a) () |
| | |
| decimal decimal avgPrice | [GetPosition](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a315b0ec8e87380ac9f5b508b247c12f9) () |
| | |

| Public Attributes | |
| --- | --- |
| decimal | [volume](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#a417aa64e37f65206a52e6eb387023840) |
| | |

| Properties | |
| --- | --- |
| decimal | [Volume](./classATAS_1_1DataFeedsCore_1_1PositionTradesQueue.md#ad7be022d2b1f162ee7ada05d46ac1c14)`[get]` |
| | |

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)PositionTradesQueue() [1/2]

| ATAS.DataFeedsCore.PositionTradesQueue.PositionTradesQueue | ( | ILoggerSource | logger, |
| --- | --- | --- | --- |
| | | [Portfolio](./classATAS_1_1DataFeedsCore_1_1Portfolio.md) | portfolio, |
| | | [Security](./classATAS_1_1DataFeedsCore_1_1Security.md) | security, |
| | | decimal | openVolume |
| | ) | | |

## [◆](https://docs.atas.net/en/)PositionTradesQueue() [2/2]

| ATAS.DataFeedsCore.PositionTradesQueue.PositionTradesQueue | ( | ILoggerSource | logger, |
| --- | --- | --- | --- |
| | | [Position](./classATAS_1_1DataFeedsCore_1_1Position.md) | position |
| | ) | | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)Add()

| void ATAS.DataFeedsCore.PositionTradesQueue.Add | ( | [MyTrade](./classATAS_1_1DataFeedsCore_1_1MyTrade.md) | newTrade | ) | |
| --- | --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)AddTrade() [1/2]

| void ATAS.DataFeedsCore.PositionTradesQueue.AddTrade | ( | bool | isBuy, |
| --- | --- | --- | --- |
| | | decimal | price, |
| | | decimal | volume |
| | ) | | |

## [◆](https://docs.atas.net/en/)AddTrade() [2/2]

| void ATAS.DataFeedsCore.PositionTradesQueue.AddTrade | ( | decimal | price, |
| --- | --- | --- | --- |
| | | decimal | volume |
| | ) | | |

## [◆](https://docs.atas.net/en/)CalculateAveragePrice()

| bool ATAS.DataFeedsCore.PositionTradesQueue.CalculateAveragePrice | ( | decimal | volume, |
| --- | --- | --- | --- |
| | | bool | checkTotalVolume, |
| | | out decimal | averagePrice |
| | ) | | |

## [◆](https://docs.atas.net/en/)Clear()

| void ATAS.DataFeedsCore.PositionTradesQueue.Clear | ( | | ) | |
| --- | --- | --- | --- | --- |

## [◆](https://docs.atas.net/en/)GetPosition()

| decimal decimal avgPrice ATAS.DataFeedsCore.PositionTradesQueue.GetPosition | ( | | ) | |
| --- | --- | --- | --- | --- |

## Member Data Documentation

## [◆](https://docs.atas.net/en/)volume

| decimal ATAS.DataFeedsCore.PositionTradesQueue.volume |
| --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)Volume

| decimal ATAS.DataFeedsCore.PositionTradesQueue.Volume |
| --- |

get

The documentation for this class was generated from the following file:
- [TradesQueue.cs](../files/TradesQueue_8cs.md)
