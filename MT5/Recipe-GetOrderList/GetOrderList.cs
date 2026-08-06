using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT5;
namespace Recipe_GetOrderList;

public class GetOrderList
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

            OrderList orderList = await mtClient.GetOrderListAsync();

            Console.WriteLine("Current order list: ");
            Console.WriteLine(orderList);

        }
		catch (Exception ex)
		{
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
       
    }
}

