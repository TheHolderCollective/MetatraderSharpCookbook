using MetatraderSharp;
using System.Globalization;
using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
namespace Recipe_PlaceModifyCloseOrder;

public class PlaceCloseOrder
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();
        List<OrderSend> openedOrders = new();
        List<OrderClose> partiallyClosedOrders = new();
        List<OrderClose> fullyClosedOrders = new();
        List<long> openTickets = new();

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
            OrderSend openOrderResponse = await mtClient.PlaceOrderAsync("EURUSD", OrderType.ORDER_TYPE_BUY, "0.02"); // comment: "Placed by cookbook recipe");
            openedOrders.Add(openOrderResponse);

            // Place sell order
            openOrderResponse = await mtClient.PlaceOrderAsync("CADJPY", OrderType.ORDER_TYPE_SELL, "0.02"); // comment: "Placed by cookbook recipe");
            openedOrders.Add(openOrderResponse);

            // Get list of valid tickets
            openTickets = openedOrders.Where(x => x.Ticket != -1).Select(x => x.Ticket).ToList();

            Console.WriteLine("List of opened orders: ");
            PrintList<OrderSend>(openedOrders);

         
            // --> Partially close the orders
            foreach (var ticket in openTickets)
            {
                OrderClose closeOrderReponse = await mtClient.CloseOrderAsync(ticket, 0.01);
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

    public static void PrintList<T>(List<T> myList)
    {
        foreach (var item in myList)
        {
            Console.WriteLine(item);
        }
    }
}
