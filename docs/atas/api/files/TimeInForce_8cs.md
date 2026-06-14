# TimeInForce.cs File Reference

Source: https://docs.atas.net/en/TimeInForce_8cs.html

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.DataFeedsCore](../namespaces/namespaceATAS_1_1DataFeedsCore.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [ATAS.DataFeedsCore.TimeInForce](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) {
  [ATAS.DataFeedsCore.None](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47a6adf97f83acf6453d4a6a4b1070f3754) = 0
, [ATAS.DataFeedsCore.GoodTillCancel](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47afcf8eb033b88c617c04f00c00cbc734e) = 1 << 0
, [ATAS.DataFeedsCore.FillOrKill](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47aaca8c369e3b611e1109cc77a7feaf872) = 1 << 1
, [ATAS.DataFeedsCore.ImmediateOrCancel](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47ad4a1a8e5f8ba7f72623f2ac301268c07) = 1 << 2
,
  [ATAS.DataFeedsCore.Day](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47a03727ac48595a24daed975559c944a44) = 1 << 3
, [ATAS.DataFeedsCore.Default](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47a7a1920d61156abc05a60135aefe8bc67) = GoodTillCancel | FillOrKill | Day

 } |
| | Specifies the time in force options for an order. [More...](../namespaces/namespaceATAS_1_1DataFeedsCore.md#ad78d458147081c9e728e74274b06bc47) |
| | |
