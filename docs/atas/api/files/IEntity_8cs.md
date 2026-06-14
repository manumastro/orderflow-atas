# IEntity.cs File Reference

Source: https://docs.atas.net/en/IEntity_8cs.html

| Classes | |
| --- | --- |
| interface | [ATAS.DataFeedsCore.IEntity](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md) |
| | Represents an entity in the application. [More...](../interfaces/interfaceATAS_1_1DataFeedsCore_1_1IEntity.md#details) |
| | |

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.DataFeedsCore](../namespaces/namespaceATAS_1_1DataFeedsCore.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [ATAS.DataFeedsCore.EntityType](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) {
  [ATAS.DataFeedsCore.Security](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2fae32629d4ef4fc6341f1751b405e45)
, [ATAS.DataFeedsCore.SecurityMargin](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad70ef212ce9ea4e6e7337476444b2c78)
, [ATAS.DataFeedsCore.Portfolio](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ad4f859a96c13f551a2771b7fc3a78d38)
, [ATAS.DataFeedsCore.Position](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a52f5e0bc3859bc5f5e25130b6c7e8881)
,
  [ATAS.DataFeedsCore.MarketDepth](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722ae1d593b8cbd8f7efcbb2e863121a2651)
, [ATAS.DataFeedsCore.Trade](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a5f390d80b20daad8f5d2f483fb0ae9d8)
, [ATAS.DataFeedsCore.Order](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722aa240fa27925a635b08dc28c9e4f9216d)
, [ATAS.DataFeedsCore.MyTrade](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a8798a86e53e2afa85550f08bd1c0a1cf)
,
  [ATAS.DataFeedsCore.News](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722add1ba1872df91985ed1ca4cde2dfe669)
, [ATAS.DataFeedsCore.UserRole](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722afc5add8666240abdb0cc2a453f562413)
, [ATAS.DataFeedsCore.UserGroup](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a84ddb07d15cabbefecb37c79122a197c)
, [ATAS.DataFeedsCore.User](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a8f9bfe9d1345237cb3b2b205864da075)
,
  [ATAS.DataFeedsCore.UserChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a461b97fa8b21290b7974597722e59bf0)
, [ATAS.DataFeedsCore.CommissionGroup](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a2c47761b7b849cac9f8d998c5298e9c5)
, [ATAS.DataFeedsCore.WorkingTime](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a12176a25c4b08c214ef1e7003aa9cda5)
, [ATAS.DataFeedsCore.Exchange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a992374d8e2e24f17bebc50a6e57becd6)
,
  [ATAS.DataFeedsCore.PortfolioChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a0098284eaa558f080857263d970d25e1)
, [ATAS.DataFeedsCore.TradingOptions](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a6721fcd4818b6676c22055f884531eb8)
, [ATAS.DataFeedsCore.PortfolioState](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a801a1076eb00eeedc198ddb785ecbcd9)
, [ATAS.DataFeedsCore.PositionState](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a57adb23aac4872c284f332c0330f48ef)
,
  [ATAS.DataFeedsCore.Connector](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722aedf21d7ecb364e8210ddd3dfaeca6fbf)
, [ATAS.DataFeedsCore.MarketByOrder](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722a68b4b639023e46a46a05d55d86cb680d)

 } |
| | Represents the types of entities used in the application. [More...](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a0805b2edfcb1da595d3197540c64b722) |
| | |
