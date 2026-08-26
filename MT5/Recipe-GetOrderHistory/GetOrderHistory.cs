using MetatraderSharp.MetatraderClient;
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
        int xDays = 14;

        try
		{
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            DateTime xDaysBeforeCurrentDate = DateXDaysAgo(currentDate, xDays);

            string toDate = currentDate.ToString();
            string fromDate = xDaysBeforeCurrentDate.ToString();


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


    public static DateTime DateXDaysAgo(DateTime currentDate,int xDays)
    {
        return currentDate.AddDays(-xDays);
    }
}

