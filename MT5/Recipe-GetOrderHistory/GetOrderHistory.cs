using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.MTsocketAPI.Responses.MT5;
namespace Recipe_GetOrderHistory;

/// <summary>
/// Recipe showing how to get order history from MT5 terminal
/// </summary>
public class GetOrderHistory
{
    static async Task Main(string[] args)
    {
        MT5Client mtClient = new();

        try
		{
            if (!mtClient.StatusIsOK)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            string fromDate = "2026.07.07 17:15:00";
            string toDate = "2026.08.06 22:10:00";
           
            // Change the value of the mode parameter to see the different output formats for OrderHistory
            OrderHistory orderHistory = await mtClient.GetOrderHistoryAsync(fromDate, toDate, OrderHistoryMode.ORDERS_DEALS);

            Console.WriteLine($"Order history from {fromDate} to {toDate}: ");
            Console.WriteLine(orderHistory);

        }
		catch (Exception ex)
		{
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}

