# ATAS.DataFeedsCore.IConnectorExchangeInfoProvider Interface Reference

Source: https://docs.atas.net/en/interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.html

Defines methods to retrieve exchange information based on security or exchange codes.
 [More...](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md#details)

| Public Member Functions | |
| --- | --- |
| bool | [TryGetExchangeCode](./interfaceATAS_1_1DataFeedsCore_1_1IConnectorExchangeInfoProvider.md#acae9e0d71ae13ac9b6812f41b159f6e9) (string securityCode, out string? exchangeCode) |
| | Attempts to retrieve the exchange code associated with the specified security code. |
| | |

## Detailed Description

Defines methods to retrieve exchange information based on security or exchange codes.

Implementations of this interface provide lookup functionality for mapping security codes to exchange codes and retrieving exchange details. This is typically used in scenarios where securities are associated with specific exchanges, and such associations need to be resolved programmatically.

## Member Function Documentation

## [◆](https://docs.atas.net/en/)TryGetExchangeCode()

| bool ATAS.DataFeedsCore.IConnectorExchangeInfoProvider.TryGetExchangeCode | ( | string | securityCode, |
| --- | --- | --- | --- |
| | | out string? | exchangeCode |
| | ) | | |

Attempts to retrieve the exchange code associated with the specified security code.

Parameters

| securityCode | The security code for which to look up the corresponding exchange code. Cannot be null or empty. |
| --- | --- |
| exchangeCode | When this method returns, contains the exchange code associated with the specified security code, if found; otherwise, null. |

Returnstrue if the exchange code was found for the specified security code; otherwise, false.

The documentation for this interface was generated from the following file:
- [IConnectorExchangeInfoProvider.cs](../files/IConnectorExchangeInfoProvider_8cs.md)
