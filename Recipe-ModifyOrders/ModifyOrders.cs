using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
using System.Globalization;
namespace Recipe_ModifyOrders;

public class ModifyOrders
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();
        List<OrderSend> openedOrders = new();
        List<OrderInfo> orderInfoList = new();
        List<OrderModify> modifiedOrders = new();
        List<OrderClose> closedOrders = new();

        double orderVolume = 0.01;
        double pipValue = 0.0001;
        double stopLossPips = 30;
        double takeProfitPips = 60;

        try
        {
            // Culture needs to be en-US to prevent invalid parameter errors when using doubles 
            if (!CultureInfo.CurrentCulture.Name.Equals("en-US"))
                CultureInfo.CurrentCulture = new CultureInfo("en-US");

            if (!mtClient.StatusIsOK)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            // Place a sell order and get its info
            OrderSend openOrderResponse = await mtClient.PlaceOrderAsync("EURUSD", OrderType.ORDER_TYPE_SELL, orderVolume, comment: "test market buy");
            OrderInfo orderInfo = await mtClient.GetOrderInfoAsync(openOrderResponse.Ticket);
            openedOrders.Add(openOrderResponse);
            orderInfoList.Add(orderInfo);

            // Place a buy order and get its info
            openOrderResponse = await mtClient.PlaceOrderAsync("AUDUSD", OrderType.ORDER_TYPE_BUY, orderVolume, comment: "test market sell");
            orderInfo = await mtClient.GetOrderInfoAsync(openOrderResponse.Ticket);
            openedOrders.Add(openOrderResponse);
            orderInfoList.Add(orderInfo);

            // Get a quote for the USDCAD pair and place a sell limit order
            Quote priceQuote = await mtClient.GetQuoteAsync("USDCAD");
            double limitPrice = priceQuote.Ask + (20 * pipValue);

            Console.WriteLine("Quote: \n" + priceQuote);
            Console.WriteLine("Limit price: " + limitPrice);

            openOrderResponse = await mtClient.PlaceOrderAsync("USDCAD", OrderType.ORDER_TYPE_SELL_LIMIT, orderVolume, limitPrice);
            orderInfo = await mtClient.GetOrderInfoAsync(openOrderResponse.Ticket);
            openedOrders.Add(openOrderResponse);
            orderInfoList.Add(orderInfo);


            // Output responses 
            Console.WriteLine("Open orders:");
            PrintList<OrderSend>(openedOrders);

            Console.WriteLine("\nOrders before changes: ");
            PrintList<OrderInfo>(orderInfoList);

            // Modify orders
            foreach (var order in orderInfoList)
            {
                double stopLoss = CalculateStopLoss(order, stopLossPips, pipValue);
                double takeProfit = CalculateTakeProfit(order, takeProfitPips, pipValue);

                OrderModify modifiedOrder = await mtClient.ModifyOrderAsync(order.Trade.Ticket, stopLoss, takeProfit);
                modifiedOrders.Add(modifiedOrder);
            }

            Console.WriteLine("\nModified Orders: ");
            PrintList<OrderModify>(modifiedOrders);

            orderInfoList.Clear();
            foreach (var order in openedOrders)
            {
                orderInfo = await mtClient.GetOrderInfoAsync(order.Ticket);
                orderInfoList.Add(orderInfo);
            }

            Console.WriteLine("\nOrders after changes: ");
            PrintList<OrderInfo>(orderInfoList);

            // Close orders
            foreach (var order in openedOrders)
            {
                OrderClose orderCloseResponse = await mtClient.CloseOrderAsync(order.Ticket);
                closedOrders.Add(orderCloseResponse);
            }

            Console.WriteLine("\n Closed orders: ");
            PrintList<OrderClose>(closedOrders);
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }

    public static double CalculateStopLoss(OrderInfo orderInfo, double pips, double pipValue)
    {
        double stopLoss;

        bool isBuyOrder = orderInfo.Trade.Type.Contains("buy");

        if (isBuyOrder)
        {
            stopLoss = orderInfo.Trade.PriceOpen - (pips * pipValue);
        }
        else
        {
            stopLoss = orderInfo.Trade.PriceOpen + (pips * pipValue);
        }

        return stopLoss;
    }

    public static double CalculateTakeProfit(OrderInfo orderInfo, double pips, double pipValue)
    {
        double takeProfit;

        bool isBuyOrder = orderInfo.Trade.Type.Contains("buy");

        if (isBuyOrder)
        {
            takeProfit = orderInfo.Trade.PriceOpen + (pips * pipValue);
        }
        else
        {
            takeProfit = orderInfo.Trade.PriceOpen - (pips * pipValue);
        }

        return takeProfit;
    }

    public static void PrintList<T>(List<T> myList)
    {
        foreach (var item in myList)
        {
            Console.WriteLine(item);
        }
    }
}
