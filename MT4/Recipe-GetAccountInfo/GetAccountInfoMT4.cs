using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
namespace MetatraderSharp_Examples;

/// <summary>
/// Get Account Info Recipe
/// </summary>
public class GetAccountInfoMT4
{
    static async Task Main(string[] args)
    { 
        MT4Client mtClient = new();

        try
        {
            if (mtClient.StatusIsError)
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