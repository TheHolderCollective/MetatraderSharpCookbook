using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.MTsocketAPI.Responses.MT5;
using System.Globalization;
namespace Recipe_ModifyOrders;

public class ModifyOrders
{
    static async Task Main(string[] args)
    {
        MT5Client mtClient = new();
        List<OrderInfo> orderInfoList = new();
        List<OrderSendResponse> openedOrders = new();
        List<OrderModifyResponse> modifiedOrders = new();
        List<OrderCloseResponse> closedOrders = new();

        double orderVolume = 0.01;
        double pipValue = 0.0001;
        double stopLossPips = 30;
        double takeProfitPips = 60;

        try
        {
            // Culture needs to be en-US to prevent invalid parameter errors when using doubles 
            if (!CultureInfo.CurrentCulture.Name.Equals("en-US"))
                CultureInfo.CurrentCulture = new CultureInfo("en-US");

            if (mtClient.StatusIsError)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            // Place a sell order and get its info
            OrderSendResponse openOrderResponse = await mtClient.PlaceOrderAsync("EURUSD", OrderType.ORDER_TYPE_SELL, orderVolume, comment: "test market sell");
            OrderInfo orderInfo = await mtClient.GetOrderInfoAsync(openOrderResponse.Order);
            openedOrders.Add(openOrderResponse);

            // Place a buy order and get its info
            openOrderResponse = await mtClient.PlaceOrderAsync("AUDUSD", OrderType.ORDER_TYPE_BUY, orderVolume, comment: "test market buy");
            orderInfo = await mtClient.GetOrderInfoAsync(openOrderResponse.Order);
            openedOrders.Add(openOrderResponse);

            // Get a quote for the USDCAD pair and place a sell limit order with an expiration date of 1 day from now
            Quote priceQuote = await mtClient.GetQuoteAsync("USDCAD");
            double limitPrice = priceQuote.Bid + (20 * pipValue);

            string expirationDate = DateTime.Now.AddDays(1).ToString();

            openOrderResponse = await mtClient.PlaceOrderAsync("USDCAD", OrderType.ORDER_TYPE_SELL_LIMIT, orderVolume, false, limitPrice, expiration: expirationDate);
            orderInfo = await mtClient.GetOrderInfoAsync(openOrderResponse.Order);
            openedOrders.Add(openOrderResponse);

            // Output responses 
            Console.WriteLine("Open orders:");
            PrintList<OrderSendResponse>(openedOrders);

            Console.WriteLine("\nOrders before changes: ");
            orderInfoList = await CreateOrderInfoList(openedOrders, mtClient);
            PrintList<OrderInfo>(orderInfoList);

            // Modify orders by setting take profit and stop loss
            for (int i = 0; i < openedOrders.Count; i++)
            {
                bool isLimitOrder = IsLimitOrder(openedOrders[i]);
                long ticketNumber = isLimitOrder ? orderInfoList[i].PendingOrder[0].Ticket : orderInfoList[i].OpenedOrder[0].Ticket;

                double stopLoss = CalculateStopLoss(orderInfoList[i], stopLossPips, pipValue, isLimitOrder);
                double takeProfit = CalculateTakeProfit(orderInfoList[i], takeProfitPips, pipValue, isLimitOrder);
                
                OrderModifyResponse modifiedOrder = await mtClient.ModifyOrderAsync(ticketNumber, stopLoss, takeProfit);
                modifiedOrders.Add(modifiedOrder);
            }

            Console.WriteLine("\nModified Orders: ");
            PrintList<OrderModifyResponse>(modifiedOrders);

            Console.WriteLine("\nOrders after changes: ");
            orderInfoList = await CreateOrderInfoList(openedOrders, mtClient);
            PrintList<OrderInfo>(orderInfoList);

            // Close orders
            foreach (var order in openedOrders)
            {
                OrderCloseResponse orderCloseResponse = await mtClient.CloseOrderAsync(order.Order);
                closedOrders.Add(orderCloseResponse);
            }

            Console.WriteLine("\n Closed orders: ");
            PrintList<OrderCloseResponse>(closedOrders);
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }

    public static double CalculateStopLoss(OrderInfo orderInfo, double pips, double pipValue, bool isLimitOrder = false)
    {
        double openPrice = GetOpenPrice(orderInfo,isLimitOrder);
       
        if (IsBuyOrder(orderInfo, isLimitOrder))
        {
           return openPrice - (pips * pipValue);
        }
        else
        {
            return openPrice + (pips * pipValue);
        }
    }

    public static double CalculateTakeProfit(OrderInfo orderInfo, double pips, double pipValue, bool isLimitOrder = false)
    {
        double openPrice = GetOpenPrice(orderInfo, isLimitOrder);

        if (IsBuyOrder(orderInfo, isLimitOrder))
        {
            return openPrice + (pips * pipValue);
        }
        else
        {
            return openPrice - (pips * pipValue);
        }
    }

    public static double GetOpenPrice(OrderInfo orderInfo, bool isLimitOrder)
    {
        return isLimitOrder ? orderInfo.PendingOrder[0].PriceOpen : orderInfo.OpenedOrder[0].PriceOpen;
    }

    public static bool IsLimitOrder(OrderSendResponse orderResponse)
    {
        return orderResponse.Type.Contains("LIMIT");
    }

    public static bool IsBuyOrder(OrderInfo orderInfo, bool isLimitOrder)
    {
        return isLimitOrder ? orderInfo.PendingOrder[0].Type.Contains("BUY") : orderInfo.OpenedOrder[0].Type.Contains("BUY");
    }

    public static async Task<List<OrderInfo>> CreateOrderInfoList(List<OrderSendResponse> orderResponses, MT5Client client)
    {
        List<OrderInfo> orderInfoList = new();

        foreach (var orderResponse in orderResponses)
        {
            if (orderResponse.ErrorID == QueryStatus.Ok)
            {
                OrderInfo orderInfo = await client.GetOrderInfoAsync(orderResponse.Order);
                orderInfoList.Add(orderInfo);
            }
        }

        return orderInfoList;
    }

    public static void PrintList<T>(List<T> myList)
    {
        foreach (var item in myList)
        {
            Console.WriteLine(item);
        }
    }
}

