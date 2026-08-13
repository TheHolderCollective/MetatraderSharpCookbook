using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
namespace Recipe_GetOrderInfo;

/// <summary>
/// Recipe showing how to get order information
/// </summary>
class GetOrderInfo
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();
        long ticketNumber;

        try
        {
            if (!mtClient.StatusIsOK)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            Console.Write("Please enter a valid ticket number: ");
            string? userResponse = Console.ReadLine();
            ticketNumber = Convert.ToInt64(userResponse);

            OrderInfo orderInfo = await mtClient.GetOrderInfoAsync(ticketNumber);

            if (mtClient.LastQueryStatus == QueryStatus.Ok)
            {
                Console.WriteLine($"\nOrder Info - Ticket {ticketNumber}: ");
            }

            Console.WriteLine(orderInfo);

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}\n");
        }
        catch (FormatException)
        {
            Console.WriteLine("Input is not a valid ticket number.");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}
