# Position.cs File Reference

Source: https://docs.atas.net/en/Position_8cs.html

| Classes | |
| --- | --- |
| class | [ATAS.DataFeedsCore.Position](../classes/classATAS_1_1DataFeedsCore_1_1Position.md) |
| | Represents a trading position. [More...](../classes/classATAS_1_1DataFeedsCore_1_1Position.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.DataFeedsCore](../namespaces/namespaceATAS_1_1DataFeedsCore.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [ATAS.DataFeedsCore.TPlusLimits](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793) { [ATAS.DataFeedsCore.T0](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793aa48788bd63a0384007cd7d089af6c610) = 0
, [ATAS.DataFeedsCore.T1](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793ace499dea30cfce118f4fe85da0227e83) = 1
, [ATAS.DataFeedsCore.T2](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793a71d2c46af01feeea54a0f541243e297b) = 2
, [ATAS.DataFeedsCore.Tx](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793a4393102620f7750d259e3f050f32ba0b) = 365
 } |
| | Represents different T+ (settlement) periods for trading. T+ refers to the number of days it takes for a trade to settle and for the funds and securities to be exchanged between the parties involved in the transaction. [More...](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ab77911efab6cb84561e875582d07c793) |
| | |
| enum | [ATAS.DataFeedsCore.PositionAveragePriceValueTypes](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) {
  [ATAS.DataFeedsCore.None](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa6adf97f83acf6453d4a6a4b1070f3754)
, [ATAS.DataFeedsCore.ReceivedFromServer](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa54d6083a6c427dc6aaaf683d4f59ea1d)
, [ATAS.DataFeedsCore.CalculatedByAllTrades](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfaec1fc7e7c94ce2d8b6d5cf4dfaf427f8)
, [ATAS.DataFeedsCore.CalculatedByPartTrades](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa6c82a62692256bc4700213488004ec99)
,
  [ATAS.DataFeedsCore.NotCalculated](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cfa90a0ffb1cc27df7493c875b893d923bb)

 } |
| | Represents different types of values for the average price of a position. The average price is the average cost of all the trades that make up a position. [More...](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a60cda7325689c2c3850c528c78ec78cf) |
| | |
| enum | [ATAS.DataFeedsCore.PnlPercentType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87) { [ATAS.DataFeedsCore.Portfolio](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87ad4f859a96c13f551a2771b7fc3a78d38)
, [ATAS.DataFeedsCore.Margin](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87a98369609669478919c74c916440e9978)
 } |
| | Represents the type of percentage calculation for profit and loss (PNL). [More...](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a97e2063f18e832c3f61d4569cc24cd87) |
| | |
