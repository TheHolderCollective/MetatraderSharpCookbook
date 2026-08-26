using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
namespace Recipe_GetOrderList;

/// <summary>
/// Recipe showing how to get list of current or pending orders
/// </summary>
public class GetOrderList
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();

        try
        {
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            OrderList orderList = await mtClient.GetOrderListAsync();

            if (mtClient. LastQuerySuccessful())
            {
                Console.WriteLine($"Order List: ");
                Console.WriteLine(orderList);
            }

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus()}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage()}\n");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }

    }
}
