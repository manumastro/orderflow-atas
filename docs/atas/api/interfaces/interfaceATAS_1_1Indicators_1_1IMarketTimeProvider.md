# ATAS.Indicators.IMarketTimeProvider Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.html

Inheritance diagram for ATAS.Indicators.IMarketTimeProvider:

This browser is not able to show SVG: try Firefox, Chrome, Safari, or Opera instead.

[[legend](../overviews/graph_legend.md)]

| Public Member Functions | |
| --- | --- |
| void | [SubscribeToTimer](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a7a187460ab54c6669677fdb8a2d3153d) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback) |
| | Registers the callback and periodically calls it with specified period. |
| | |
| void | [UnsubscribeFromTimer](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a486f0f6bddd244284110a94baeecd595) ([TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) period, Action callback) |
| | Unregister the callback from periodic invocations. |
| | |

| Properties | |
| --- | --- |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [MarketTime](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#a719ea8d551a57d0673177c0d01931ffb)`[get]` |
| | Current market time. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [UtcTime](./interfaceATAS_1_1Indicators_1_1IMarketTimeProvider.md#afd122a8c561de01a599f88f1477615f9)`[get]` |
| | Current UTC time. |
| | |

## Member Function Documentation

## [◆](https://docs.atas.net/en/)SubscribeToTimer()

| void ATAS.Indicators.IMarketTimeProvider.SubscribeToTimer | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period, |
| --- | --- | --- | --- |
| | | Action | callback |
| | ) | | |

Registers the callback and periodically calls it with specified period.

Parameters

| period | A period of market data time used to repeatedly invoke the callback. |
| --- | --- |
| callback | A method that is called each time a specified period of market data time passes. |

Implemented in [ATAS.Indicators.ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#aa725dd87b95c0d6347bb572458f1ad64).

## [◆](https://docs.atas.net/en/)UnsubscribeFromTimer()

| void ATAS.Indicators.IMarketTimeProvider.UnsubscribeFromTimer | ( | [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | period, |
| --- | --- | --- | --- |
| | | Action | callback |
| | ) | | |

Unregister the callback from periodic invocations.

Parameters

| period | Subscribed period of invocations. |
| --- | --- |
| callback | A method to be unregistered. |

Implemented in [ATAS.Indicators.ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a35eee088f9207adb5d5050e204e57e03).

## Property Documentation

## [◆](https://docs.atas.net/en/)MarketTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IMarketTimeProvider.MarketTime |
| --- |

get

Current market time.

Implemented in [ATAS.Indicators.ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a3cb11266584a54b0174b13d8480e3093).

## [◆](https://docs.atas.net/en/)UtcTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.IMarketTimeProvider.UtcTime |
| --- |

get

Current UTC time.

Implemented in [ATAS.Indicators.ExtendedIndicator](../classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a135df1979d1e058363d36382e8117336).

The documentation for this interface was generated from the following file:
- [IMarketTimeProvider.cs](../files/IMarketTimeProvider_8cs.md)
