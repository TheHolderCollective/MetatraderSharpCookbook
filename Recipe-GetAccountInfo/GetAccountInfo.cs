using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
namespace MetatraderSharp_Examples;

/// <summary>
/// Get Account Info Recipe
/// </summary>
public class GetAccountInfo
{
    static void Main(string[] args)
    {

        MetatraderClient mtClient = new();

        try
        {
            Account myAccount = mtClient.GetAccountInfo();

            if (mtClient.LastQueryStatus == QueryStatus.OK)
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