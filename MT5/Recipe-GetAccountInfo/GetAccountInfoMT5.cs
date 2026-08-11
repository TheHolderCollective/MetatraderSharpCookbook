using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT5;
namespace Recipe_GetAccountInfo;

// <summary>
/// Get Account Info Recipe
/// </summary>
public class GetAccountInfoMT5
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

            Account myAccount = await mtClient.GetAccountInfoAsync();

            Console.WriteLine($"Terminal Type: {mtClient.TerminalType}");

            if (mtClient.LastQueryStatus == QueryStatus.Ok)
                Console.WriteLine(myAccount);

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}");

        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}

