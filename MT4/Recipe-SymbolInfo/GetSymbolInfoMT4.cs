using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
namespace Recipe_GetSymbolInfo;

/// <summary>
/// Get SymbolInfo Recipe
/// </summary>
public class GetSymbolInfoMT4
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

            string validSymbol = "CADJPY";

            SymbolInformation correctSymbolInfo = await mtClient.GetSymbolInformationAsync(validSymbol);

            Console.WriteLine($"Client Type: {mtClient.ClientType}");
            Console.WriteLine($"Symbol information (valid symbol): {validSymbol} ");
            Console.WriteLine(correctSymbolInfo);
            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus()}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage()}");

            string invalidSymbol = "CADJPy";

            SymbolInformation incorrectSymbolInfo = await mtClient.GetSymbolInformationAsync(invalidSymbol);

            Console.WriteLine($"\nSymbol information (invalid symbol): {invalidSymbol} ");
            Console.WriteLine(incorrectSymbolInfo);
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
