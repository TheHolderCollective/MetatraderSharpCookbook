using MetatraderSharp;
using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
namespace Recipe_GetOrderHistory;

/// <summary>
/// Recipe showing how to get order history
/// </summary>
public class GetOrderHistory
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();

        try
        {
            if (!mtClient.StatusIsOK)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            DateTime previousTwoWeeks = new DateTime(currentDate.Year, currentDate.Month - 1, currentDate.Day + 14);

            string toDate = currentDate.ToString();
            string fromDate = previousTwoWeeks.ToString();

            OrderHistory orderHistory = await mtClient.GetOrderHistoryAsync(fromDate, toDate);

            if (mtClient.LastQueryStatus == QueryStatus.OK)
            {
                Console.WriteLine($"Order History from [{fromDate}] to [{toDate}]: ");
                Console.WriteLine(orderHistory);
            }

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}\n");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }

    }
}
