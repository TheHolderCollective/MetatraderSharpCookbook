# MetatraderSharp Cookbook   ![Static Badge](https://img.shields.io/badge/release-v1.1.0-blue)

A collection of small C# console apps which demonstrate how to use the MetatraderSharp wrapper library.

![Static Badge](https://img.shields.io/badge/Framework-.NET_9_-blue)  ![Static Badge](https://img.shields.io/badge/Framework-.NET_10_-blue)

## Cookbook Overview
The cookbook contains 26 recipes spread across 3 folders.

| Folder| Description |
| --- | --- |
| [Common](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/Common) | Recipes demonstrating functionality common to the MT4Client and MT5Client classes. |
| [MT4](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4) | Recipes demonstrating MT4Client only functionality. |
| [MT5](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5) | Recipes demonstrating MT5Client only functionality. |

## Cookbook Contents

**Common Folder Recipes**

| Recipe| Folder |Description |
| --- | --- |--- |
| [GetPriceHistory](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/Common/Recipe-GetPriceHistory) | Common | Demonstrates usage of `GetPriceHistoryAsync()`, `PriceHistory` and its extension methods to get and analyse price history data.|
| [GetQuote](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/Common/Recipe-GetQuote) |  Common | Demonstrates usage of `GetQuoteAsync()` and `Quote` to obtain a symbol quote.|
| [GetSymbolList](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/Common/Recipe-GetSymbolList) |  Common | Demonstrates usage of `GetSymbolAsync()`, `SymbolList` and its extenstion methods to get and manipulate available symbol data.|
| [GetTerminalInfo](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/Common/Recipe-GetTerminalInfo) |  Common | Demonstrates usage of `GetTerminalInfoAsync()` and `TerminalInfo` to get terminal info.|
| [TrackOHLC](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/Common/Recipe-TrackOHLC) |  Common |Demonstrates usage of `TrackOHLCsAsync()`, `SymbolRequest`, `TrackOHLCRequest`, and `TrackResponse` to stream OHLCs for specified symbols and timeframes. |
| [TrackPrices](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/Common/Recipe-TrackPrices) |  Common |Demonstrates usage of `TrackPricesAsync()` and  `TrackResponse` to stream live prices for specified symbols.|

---

**MT4 Folder Recipes**

| Recipe| Folder |Description |
| --- | --- |--- |
| [GetAccountInfo](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-GetAccountInfo) | MT4 | Demonstrates usage of `GetAccountInfoAsync()` and `Account` to get account information for the connected account. |
| [GetOrderHistory](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-GetOrderHistory) | MT4 | Demonstrates usage of `GetOrderHistoryAsync()` and `OrderHistory` to get order history for the connected account. |
| [GetOrderInfo](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-GetOrderInfo) | MT4 | Demonstrates usage of `GetOrderInfoAsync()` and `OrderInfo` to get detailed order information when given an existing ticket number. |
| [GetOrderList](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-GetOrderList) | MT4 | Demonstrates usage of `GetOrderListAsync()` and `OrderList` to get the list of active orders for the connected account. |
| [Indicators](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-Indicators) | MT4 | Demonstrates usage of `GetATRValuesAsync()`, `GetMAValuesAsync()`, `GetCustomIndicatorValuesAsync()`, and `Indicator` to get values from the ATR, MA, and custom indicators respectively. |
| [PlaceAndCloseOrders](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-PlaceAndCloseOrders) | MT4 | Demonstrates usage of `PlaceOrderAsync()`, `CloseOrderAsync()`, `OrderSendResponse`, and `OrderCloseResponse` to place and close orders. |
| [ModifyOrders](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-ModifyOrders) | MT4 | Demonstrates usage of `ModifyOrderAsync()`, `OrderModifyResponse` along with previously demonstrated methods and responses in `PlaceAndCloseOrders` to open, modify, and close orders.  |
| [SymbolInfo](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT4/Recipe-SymbolInfo) | MT4 | Demonstrates usage of `GetSymbolInformationAsync()` and `SymbolInformation` to get detailed information about a given symbol. |

---

**MT5 Folder Recipes**

| Recipe| Folder |Description |
| --- | --- |--- |
| [GetAccountInfo](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-GetAccountInfo) | MT5 |Demonstrates usage of `GetAccountInfoAsync()` and `Account` to get account information for the connected account.  |
| [GetCalendar](https://github.com/TheHolderCollective/MetatraderSharpCookbook/blob/master/MT5/Recipe-GetCalendar) | MT5 |Demonstrates usage of `GetCalendarAsync()` and `Calendar`  to get calendar event information.|
| [GetOrderHistory](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-GetOrderHistory) | MT5 | Demonstrates usage of `GetOrderHistoryAsync()` and `OrderHistory` to get order history for the connected account. |
| [GetOrderInfo](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-GetOrderInfo) | MT5 | Demonstrates usage of `GetOrderInfoAsync()` and `OrderInfo` to get detailed order information when given an existing ticket number. |
| [GetOrderList](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-GetOrderList) | MT5 | Demonstrates usage of `GetOrderListAsync()` and `OrderList` to get the list of active orders for the connected account. |
| [GetSymbolInfo](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-GetSymbolInfo) | MT5 | Demonstrates usage of `GetSymbolInformationAsync()` and `SymbolInformation` to get detailed information about a given symbol. |
| [GetTickHistory](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-GetTickHistory) | MT5 | Demonstrates usage of `GetTickHistoryAsync()` and `TickHistory` to get tick history data for a given symbol and time frame. |
| [Indicators](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-Indicators) | MT5 | Demonstrates usage of `GetATRValuesAsync()`, `GetMAValuesAsync()`, `GetCustomIndicatorValuesAsync()`, and `Indicator` to get values from the ATR, MA, and custom indicators respectively. |
| [PlaceAndCloseOrders](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-PlaceAndCloseOrders) | MT5 | Demonstrates usage of `PlaceOrderAsync()`, `CloseOrderAsync()`, `OrderSendResponse`, and `OrderCloseResponse` to place and close orders. |
| [ModifyOrders](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-ModifyOrders) | MT5 | Demonstrates usage of `ModifyOrderAsync()`, `OrderModifyResponse` along with previously demonstrated methods and responses in `PlaceAndCloseOrders` recipe to open, modify, and close orders. |
| [TrackMarketBook](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-TrackMarketBook) | MT5 | Demonstrates usage of `TrackMarketBookAsync`, `TrackResponse`, and `MarketDepth` to get depth of market data. This only works if the broker supplies DOM data. |
| [TrackOrderEvents](https://github.com/TheHolderCollective/MetatraderSharpCookbook/tree/master/MT5/Recipe-TrackOrderEvents) | MT5 |Demonstrates usage of `TrackOrderEventsAsync`, `TrackOrderEventsResponse`, and `TrackOrderEvents` to stream and receive order events as they occur. |


## Usage Requirements
Successful usage of the MetatraderSharp library requires:
- Metatrader 4/Metratrader 5 to be installed 
- a version of the MTsocketAPI installed in your Metatrader

## Disclaimer

I have no connection or affiliation with MTsocketAPI. This MetatraderSharp library (and by extension cookbook) is purely a labour of love which arose from my need to have an easy way to pull data from my MetaTrader terminals.
