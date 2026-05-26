using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
namespace MetatraderSharp_Examples;

/// <summary>
/// Get Quote Recipe
/// </summary>
public class GetQuote
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

            Quote goodQuote = await mtClient.GetQuoteAsync("GBPJPY");

            Console.WriteLine("Quote result for a recognised symbol: ");
            Console.WriteLine(goodQuote);
            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}");

            Quote badQuote = await mtClient.GetQuoteAsync("GBPJPYL");

            Console.WriteLine("\nQuote result for an invalid symbol: ");
            Console.WriteLine(badQuote);
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
