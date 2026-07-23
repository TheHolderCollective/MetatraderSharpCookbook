using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
using System.Globalization;
namespace Recipe_PlaceAndCloseOrders;

public class PlaceAndCloseOrders
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();
        List<OrderSend> openedOrders = new();
        List<OrderClose> partiallyClosedOrders = new();
        List<OrderClose> fullyClosedOrders = new();
        List<long> openTickets = new();

        double initialVolume = 0.02;
        double sellVolume = 0.01;
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

            // --> First place some orders

            // Place buy order
            OrderSend openOrderResponse = await mtClient.PlaceOrderAsync("EURUSD", OrderType.ORDER_TYPE_BUY, initialVolume);
            openedOrders.Add(openOrderResponse);

            // Place sell order
            openOrderResponse = await mtClient.PlaceOrderAsync("CADJPY", OrderType.ORDER_TYPE_SELL, initialVolume);
            openedOrders.Add(openOrderResponse);

            // Place buy order with take profit and stop loss
            // This requires estimating the stop loss and take profit based on the current price quoted for the symbol
            Quote priceQuote = await mtClient.GetQuoteAsync("USDCAD");

            double stopLoss = CalculateStopLoss(OrderType.ORDER_TYPE_BUY, priceQuote.Ask, stopLossPips, pipValue);
            double takeProfit = CalculateTakeProfit(OrderType.ORDER_TYPE_BUY, priceQuote.Ask, takeProfitPips, pipValue);

            openOrderResponse = await mtClient.PlaceOrderAsync("USDCAD", OrderType.ORDER_TYPE_BUY, initialVolume, 0, stopLoss, takeProfit);
            openedOrders.Add(openOrderResponse);

            // Get list of valid tickets
            openTickets = openedOrders.Where(x => x.Ticket != -1).Select(x => x.Ticket).ToList();

            Console.WriteLine("List of opened orders: ");
            PrintList<OrderSend>(openedOrders);

            // Get order info 
            Console.WriteLine("\nOrder info for opened orders: ");
            foreach (var ticket in openTickets)
            {
                OrderInfo info = await mtClient.GetOrderInfoAsync(ticket);
                Console.WriteLine(info);
            }

            // --> Partially close the orders
            foreach (var ticket in openTickets)
            {
                OrderClose closeOrderReponse = await mtClient.CloseOrderAsync(ticket, sellVolume);
                partiallyClosedOrders.Add(closeOrderReponse);
            }

            Console.WriteLine("\nList of partially closed orders: ");
            PrintList<OrderClose>(partiallyClosedOrders);


            // --> Completely close the orders
            foreach (var ticket in openTickets)
            {
                long newTicketNumber = await mtClient.FindNewTicketNumber(ticket);
                OrderClose closeOrderReponse = await mtClient.CloseOrderAsync(newTicketNumber);
                fullyClosedOrders.Add(closeOrderReponse);
            }

            Console.WriteLine("\nList of fully closed orders: ");
            PrintList<OrderClose>(fullyClosedOrders);
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }

    public static double CalculateStopLoss(string orderType, double price, double pips, double pipValue)
    {
        double stopLoss;

        bool isBuyOrder = orderType.ToLower().Contains("buy");

        if (isBuyOrder)
        {
            stopLoss = price - (pips * pipValue);
        }
        else
        {
            stopLoss = price + (pips * pipValue);
        }

        return stopLoss;
    }

    public static double CalculateTakeProfit(string orderType, double price, double pips, double pipValue)
    {
        double takeProfit;

        bool isBuyOrder = orderType.ToLower().Contains("buy");

        if (isBuyOrder)
        {
            takeProfit = price + (pips * pipValue);
        }
        else
        {
            takeProfit = price - (pips * pipValue);
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
