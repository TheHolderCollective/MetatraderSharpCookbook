using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
namespace MetatraderSharp_Examples;

/// <summary>
/// Get Quote Recipe
/// </summary>
public class GetQuote
{
    static void Main(string[] args)
    {
        MetatraderClient mtClient = new();

        try
        {
            Quote goodQuote = mtClient.GetQuote("GBPJPY");

            Console.WriteLine("Quote result for a recognised symbol: ");
            Console.WriteLine(goodQuote);
            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}");

            Quote badQuote = mtClient.GetQuote("GBPJPYL");

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
