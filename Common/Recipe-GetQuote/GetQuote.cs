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
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            Quote goodQuote = await mtClient.GetQuoteAsync("GBPJPY");

            Console.WriteLine("Quote result for a recognised symbol: ");
            Console.WriteLine(goodQuote);
            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus()}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage()}");
            Console.WriteLine($"Error Code = {mtClient.LastErrorCode()}");

            Quote badQuote = await mtClient.GetQuoteAsync("GBPJPYL");

            Console.WriteLine("\nQuote result for an invalid symbol: ");
            Console.WriteLine(badQuote);
            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus()}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage()}");
            Console.WriteLine($"Error Code = {mtClient.LastErrorCode()}");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}
