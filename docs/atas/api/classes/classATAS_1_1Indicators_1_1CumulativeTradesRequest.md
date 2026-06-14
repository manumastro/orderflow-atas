# ATAS.Indicators.CumulativeTradesRequest Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1CumulativeTradesRequest.html

Represents a request to retrieve cumulative trade data within a specified time range or for a particular date.
 [More...](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#details)

| Public Member Functions | |
| --- | --- |
| | [CumulativeTradesRequest](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#a1a3257f3c52dec6e5fdf8488bf424533) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) beginTime, [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) endTime, int minVolume, int maxVolume) |
| | Initializes a new instance of the CumulativeTradesRequest class with the specified time range and volume filters. |
| | |
| | [CumulativeTradesRequest](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#a5b117f9d9b2b60200f540d8e1b8ce75a) ([DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) date) |
| | Initializes a new instance of the CumulativeTradesRequest class for retrieving data for a particular date. |
| | |

| Properties | |
| --- | --- |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [BeginTime](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#a5f90b61a3ef7114553226b0f5069e2b0)`[get]` |
| | Gets the start time of the requested data. |
| | |
| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [EndTime](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#a0c5ecab89ad647e0c99dc91eba736e7d)`[get]` |
| | Gets the end time of the requested data. |
| | |
| decimal | [MinVolume](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#af8b8c925a46de2412dc924509f2ee8e2)`[get]` |
| | Gets the minimum cumulative volume filter for the requested data. |
| | |
| decimal | [MaxVolume](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#a597b72f21298ea09c2eee10d7a0b6b29)`[get]` |
| | Gets the maximum cumulative volume filter for the requested data. |
| | |
| int | [RequestId](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#afa53fbd872bafd61f03644bae017788a)`[get]` |
| | Gets the unique identifier for the request. |
| | |
| bool | [GetDataForParticularDate](./classATAS_1_1Indicators_1_1CumulativeTradesRequest.md#ae727b00b3748e1b7590811ab62337a02)`[get, set]` |
| | Gets or sets a flag indicating whether to get data for a particular date only. |
| | |

## Detailed Description

Represents a request to retrieve cumulative trade data within a specified time range or for a particular date.

It mustn't be more than 7 days.

## Constructor & Destructor Documentation

## [◆](https://docs.atas.net/en/)CumulativeTradesRequest() [1/2]

| ATAS.Indicators.CumulativeTradesRequest.CumulativeTradesRequest | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | beginTime, |
| --- | --- | --- | --- |
| | | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | endTime, |
| | | int | minVolume, |
| | | int | maxVolume |
| | ) | | |

Initializes a new instance of the CumulativeTradesRequest class with the specified time range and volume filters.

Parameters

| beginTime | The start time of the requested data. |
| --- | --- |
| endTime | The end time of the requested data. |
| minVolume | The minimum cumulative volume filter for the requested data. |
| maxVolume | The maximum cumulative volume filter for the requested data. |

## [◆](https://docs.atas.net/en/)CumulativeTradesRequest() [2/2]

| ATAS.Indicators.CumulativeTradesRequest.CumulativeTradesRequest | ( | [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | date | ) | |
| --- | --- | --- | --- | --- | --- |

Initializes a new instance of the CumulativeTradesRequest class for retrieving data for a particular date.

Parameters

| date | The date for which to retrieve the data. |
| --- | --- |

## Property Documentation

## [◆](https://docs.atas.net/en/)BeginTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.CumulativeTradesRequest.BeginTime |
| --- |

get

Gets the start time of the requested data.

## [◆](https://docs.atas.net/en/)EndTime

| [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.CumulativeTradesRequest.EndTime |
| --- |

get

Gets the end time of the requested data.

## [◆](https://docs.atas.net/en/)GetDataForParticularDate

| bool ATAS.Indicators.CumulativeTradesRequest.GetDataForParticularDate |
| --- |

getset

Gets or sets a flag indicating whether to get data for a particular date only.

## [◆](https://docs.atas.net/en/)MaxVolume

| decimal ATAS.Indicators.CumulativeTradesRequest.MaxVolume |
| --- |

get

Gets the maximum cumulative volume filter for the requested data.

## [◆](https://docs.atas.net/en/)MinVolume

| decimal ATAS.Indicators.CumulativeTradesRequest.MinVolume |
| --- |

get

Gets the minimum cumulative volume filter for the requested data.

## [◆](https://docs.atas.net/en/)RequestId

| int ATAS.Indicators.CumulativeTradesRequest.RequestId |
| --- |

get

Gets the unique identifier for the request.

The documentation for this class was generated from the following file:
- [CumulativeTradesRequest.cs](../files/CumulativeTradesRequest_8cs.md)
