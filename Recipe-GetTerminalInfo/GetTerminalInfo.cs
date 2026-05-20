using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
namespace Recipe_GetTerminal_Info;

/// <summary>
/// Get Terminal Info Recipe
/// </summary>
public class GetTerminalInfo
{
    static void Main(string[] args)
    {
        MetatraderClient mtClient = new();

        try
        {
            TerminalInfo? myTerminal = mtClient.GetTerminalInfo();

            if (mtClient.LastQueryStatus == QueryStatus.OK)
                Console.WriteLine(myTerminal);

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
