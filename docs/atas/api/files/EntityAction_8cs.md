# EntityAction.cs File Reference

Source: https://docs.atas.net/en/EntityAction_8cs.html

| Namespaces | |
| --- | --- |
| namespace | [ATAS](../namespaces/namespaceATAS.md) |
| | |
| namespace | [ATAS.DataFeedsCore](../namespaces/namespaceATAS_1_1DataFeedsCore.md) |
| | |

| Enumerations | |
| --- | --- |
| enum | [ATAS.DataFeedsCore.EntityAction](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9e) {
  [ATAS.DataFeedsCore.Logon](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae245ad2bb801b285a62cd0accab6607f) = 0
, [ATAS.DataFeedsCore.Logout](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0323de4f66a1700e2173e9bcdce02715) = 1
, [ATAS.DataFeedsCore.UserAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea804479841aecb64f83610fc8d3be160a) = 2
, [ATAS.DataFeedsCore.UserChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea461b97fa8b21290b7974597722e59bf0) = 3
,
  [ATAS.DataFeedsCore.UserRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea9c895849be07df50ca9bd2d99a9cac81) = 4
, [ATAS.DataFeedsCore.UserLock](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eac6433bce5b7ded5abcdaf200b3fa8f9f) = 29
, [ATAS.DataFeedsCore.UserKick](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea22ce95a2359e4f1e6fec0c567fafe0ea) = 30
, [ATAS.DataFeedsCore.UserChangeExpiration](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea5a266660311d4e4a46593ff1acf40d11) = 47
,
  [ATAS.DataFeedsCore.UserGroupAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea21c245d956ba0a2919b489f2c540bf87) = 5
, [ATAS.DataFeedsCore.UserGroupChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eabb6e49f537815a22e6d2d026003d900a) = 6
, [ATAS.DataFeedsCore.UserGroupRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0d11f65fe5e0832f1036969393057d2c) = 7
, [ATAS.DataFeedsCore.UserRoleAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0c2f902daaca86298c212ec829a6cdfe) = 8
,
  [ATAS.DataFeedsCore.UserRoleChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea3cc62fe18121d71439c7c5b133275ab8) = 9
, [ATAS.DataFeedsCore.UserRoleRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea42056907ddac885469f2c41b70e8310f) = 10
, [ATAS.DataFeedsCore.ExchangeAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea4fb22f59e89bde3c37d35f7224b2629c) = 11
, [ATAS.DataFeedsCore.ExchangeChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea624b1137bc36788793e76c23c5045738) = 12
,
  [ATAS.DataFeedsCore.ExchangeRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea1db9fa73405d12ffd5743a9a05da7c96) = 13
, [ATAS.DataFeedsCore.CommissionGroupAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea1ed96ed6fa371bbb4db60a2bc6286506) = 17
, [ATAS.DataFeedsCore.CommissionGroupChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eab2843a9bb4c8c462249ea2e49e115daa) = 18
, [ATAS.DataFeedsCore.CommissionGroupRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea49c6994e99137ebc4b6d41abd73b75d1) = 19
,
  [ATAS.DataFeedsCore.SecurityMarginAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eac2b54e422146e541e916213705467389) = 20
, [ATAS.DataFeedsCore.SecurityMarginChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae842434ed6da398c6e2317f89bdd6a5f) = 21
, [ATAS.DataFeedsCore.SecurityMarginRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaf610276758d9e65c8033b9d28bc056b7) = 22
, [ATAS.DataFeedsCore.PortfolioAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea778a5e05bfc08d455366f30f8da5f4e7) = 23
,
  [ATAS.DataFeedsCore.PortfolioChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0098284eaa558f080857263d970d25e1) = 32
, [ATAS.DataFeedsCore.PortfolioValue](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaf8fdd76dac43c8fc149d58f5edd42e6b) = 24
, [ATAS.DataFeedsCore.PortfolioLock](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea2fd4f4b5559f64c4bd8a3a30bfb428a6) = 25
, [ATAS.DataFeedsCore.OrderRegister](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaadd03772c0971b5d271ffcfe4f738584) = 26
,
  [ATAS.DataFeedsCore.OrderMove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea304117295652ca874b474baa122556b9) = 27
, [ATAS.DataFeedsCore.OrderCancel](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea739040143a85ea0d9884a7f256a1ed52) = 28
, [ATAS.DataFeedsCore.NewsSend](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea9129da7bfcd0f0499be7371c50aae848) = 31
, [ATAS.DataFeedsCore.ServerSettings](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea1bdce89560b19c450acfddafdc9c6a4c) = 33
,
  [ATAS.DataFeedsCore.RequestStatistics](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea0c598ee0336c93bdf22d8047896521dd) = 34
, [ATAS.DataFeedsCore.OrderCancelled](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea74e70bbb7cc1eb86c5ce255f98781370) = 35
, [ATAS.DataFeedsCore.OrderMatched](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ead4c6a03a36f2cab7feb8e5fa7beeaa98) = 36
, [ATAS.DataFeedsCore.ServerPnL](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea56eabfb9743058f4c6c48cece19336c2) = 37
,
  [ATAS.DataFeedsCore.PortfolioSuspend](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea6f1b8a721841184b520827073445cedf) = 38
, [ATAS.DataFeedsCore.PortfolioViewerAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eade36d5466dbb3df2a65de67f3f149125) = 39
, [ATAS.DataFeedsCore.ConnectorAdd](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea6788e3a23cc017bf11bd89c7f21c084f) = 40
, [ATAS.DataFeedsCore.ConnectorChange](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eadd91916f6d667a83a49eacc0dafc8b16) = 41
,
  [ATAS.DataFeedsCore.ConnectorRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eabfbe2373eb821a4a188929045b3929c3) = 42
, [ATAS.DataFeedsCore.ConnectorEnable](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea81178120c9cf2a1dc451c6095d004682) = 43
, [ATAS.DataFeedsCore.PortfolioViewerRemove](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae5e82edc7cacb99fee496d10b4f0d33a) = 44
, [ATAS.DataFeedsCore.TradeAllowed](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea110b987608e8fc19f765f6b93a0594d2) = 45
,
  [ATAS.DataFeedsCore.PortfolioReset](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eae92fc0b7579ab1b544f89016599a66e8) = 46
, [ATAS.DataFeedsCore.GetUserChangeHistory](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea71f7800313ce0532462f93cf1fedab31) = 48
, [ATAS.DataFeedsCore.GetPortfolioChangeHistory](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea440107bc4382f5a2b35531ff4fc9bed5) = 49
, [ATAS.DataFeedsCore.UserGroupSuspendPortfolios](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9eaaca807d503aba92b094b6dd6d7935b9e) = 50
,
  [ATAS.DataFeedsCore.UserGroupReset](../namespaces/namespaceATAS_1_1DataFeedsCore.md#a700c839c3d3f27dcad2fa61a18d04c9ea2cc76c0620e8c93e10ef44901eed40f6) = 51

 } |
| | |
