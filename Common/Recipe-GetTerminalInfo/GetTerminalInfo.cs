using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
namespace Recipe_GetTerminal_Info;

/// <summary>
/// Get Terminal Info Recipe
/// </summary>
public class GetTerminalInfo
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new(); 

        try
        {
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }
            // TerminalInfo is common to both MT4 and MT5.
            TerminalInfo myTerminal = await mtClient.GetTerminalInfoAsync();

            if (mtClient. LastQuerySuccessful())
                Console.WriteLine(myTerminal);

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus()}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage()}");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}
