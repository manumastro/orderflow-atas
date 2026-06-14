# ATAS.Indicators.MarketDepthSnapshotRequest Class Reference

Source: https://docs.atas.net/en/classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.html

Represents a request to retrieve a snapshot of the market depth for a specified time range.
 [More...](./classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md#details)

| Properties | |
| --- | --- |
| int | [RequestId](./classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md#a4b7bcae6562557050992b86345eb2721) = DateTime.Now.GetHashCode()`[get]` |
| | Unique identifier of the request. |
| | |
| required [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [From](./classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md#a6a3f914575559e36d4242e9fb84e82dd)`[get]` |
| | Time for data to start from. |
| | |
| required [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) | [To](./classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md#af54b4a4d2bc02d8896597ec4016a50ab)`[get]` |
| | End time of the requested data. |
| | |
| required [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) | [Period](./classATAS_1_1Indicators_1_1MarketDepthSnapshotRequest.md#ac0d4570e8c794d78a83c9693ce516467)`[get]` |
| | Period for which the data is to be retrieved. |
| | |

## Detailed Description

Represents a request to retrieve a snapshot of the market depth for a specified time range.

## Property Documentation

## [◆](https://docs.atas.net/en/)From

| required [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.MarketDepthSnapshotRequest.From |
| --- |

get

Time for data to start from.

## [◆](https://docs.atas.net/en/)Period

| required [TimeSpan](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a36ccd3ba5e96096e08abdd7f1d0940fb) ATAS.Indicators.MarketDepthSnapshotRequest.Period |
| --- |

get

Period for which the data is to be retrieved.

## [◆](https://docs.atas.net/en/)RequestId

| int ATAS.Indicators.MarketDepthSnapshotRequest.RequestId = DateTime.Now.GetHashCode() |
| --- |

get

Unique identifier of the request.

## [◆](https://docs.atas.net/en/)To

| required [DateTime](../namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) ATAS.Indicators.MarketDepthSnapshotRequest.To |
| --- |

get

End time of the requested data.

The documentation for this class was generated from the following file:
- [MarketDepthSnapshotRequest.cs](../files/MarketDepthSnapshotRequest_8cs.md)
