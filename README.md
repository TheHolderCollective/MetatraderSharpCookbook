## MetatraderSharp Cookbook

The MetatraderSharp cookbook is collection of small C# console apps which demonstrate how to use the MetatraderSharp wrapper library.

## Cookbook Overview
| Folder| Description |
| --- | --- |
| Common | Recipes demonstrating functionality common to both MT4/MT5 |
| MT4 | Recipes demonstrating MT4 only functionality |
| MT5 | Recipes demonstrating MT5 only functionality|

## Cookbook Contents

**Common Folder Recipes**

| Recipe| Folder |Description |
| --- | --- |--- |
| GetPriceHistory | Common | Demonstrates usage of `GetPriceHistoryAsync()`, `PriceHistory` and its extension methods to get and analyse price history data|
| GetQuote |  Common | Demonstrates usage of `GetQuoteAsync()` and `Quote` to obtain a symbol quote|
| GetSymbolList |  Common | Demonstrates usage of `GetSymbolAsync()`, `SymbolList` and its extenstion methods to get and manipulate available symbol data|
| GetTerminalInfo |  Common | Demonstrates usage of `GetTerminalInfoAsync()` and `TerminalInfo` to get terminal info|
| TrackOHLC |  Common |Demonstrates usage of `TrackOHLCsAsync()`, `SymbolRequest`, `TrackOHLCRequest`, and `TrackResponse` to stream OHLCs for specified symbols and timeframes |
| TrackPrices |  Common |Demonstrates usage of `TrackPricesAsync()` and  `TrackResponse` to stream live prices for specified symbols|

---

**MT4 Folder Recipes**

| Recipe| Folder |Description |
| --- | --- |--- |
| GetAccountInfo | MT4 | Demonstrates usage of `GetAccountInfoAsync()` and `Account` to get account information for the connected account |
| GetOrderHistory | MT4 | Demonstrates usage of `GetOrderHistoryAsync()` and `OrderHistory` to get order history for the connected account |
| GetOrderInfo | MT4 | Demonstrates usage of `GetOrderInfoAsync()` and `OrderInfo` to get detailed order information when given an existing ticket number |
| GetOrderList | MT4 | Demonstrates usage of `GetOrderListAsync()` and `OrderList` to get the list of active orders for the connected account |
| Indicators | MT4 | Demonstrates usage of `GetATRValuesAsync()`, `GetMAValuesAsync()`, `GetCustomIndicatorValuesAsync()`, and `Indicator` to get values from the ATR, MA, and custom indicators respectively. The OsMA indicator was used for the custom indicator. |
| PlaceAndCloseOrders | MT4 | Demonstrates usage of `PlaceOrderAsync()`, `CloseOrderAsync()`, `OrderSendResponse`, and `OrderCloseResponse` to place and close orders |
| ModifyOrders | MT4 | Demonstrates usage of `ModifyOrderAsync()`, `OrderModifyResponse` along with previously demonstrated methods and responses in `PlaceAndCloseOrders` to open, modify, and close orders  |
| SymbolInfo | MT4 | Demonstrates usage of `GetSymbolInformationAsync()` and `SymbolInformation` to get detailed information about a given symbol |

## Usage Requirements
Successful usage of the MetatraderSharp library requires:
- Metatrader 4/Metratrader 5 to be installed 
- a version of the MTsocketAPI installed in your Metatrader

## Disclaimer

I have no connection or affiliation with MTsocketAPI. This MetatraderSharp library (and by extension cookbook) is purely a labour of love which arose from my need to have an easy way to pull data from my MetaTrader terminals.
