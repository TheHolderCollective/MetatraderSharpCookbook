using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.Extensions;
namespace Recipe_GetSymbolList;

/// <summary>
/// Get Symbol List Recipe
/// </summary>
public class GetSymbolList
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();
        int symbolCount = 0;

        try
        {
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            SymbolList? symbolResponse = await mtClient.GetSymbolListAsync();

            // check to see if query failed
            if (mtClient.LastQueryFailed())
            {
                Console.WriteLine("Failed to get symbol list. An error occurred.");
                Console.WriteLine($"Error: {mtClient.LastQueryMessage()}");
            }

            // output symbol names if successful
            if (mtClient.LastQuerySuccessful())
            {
                List<string> symbolList = symbolResponse.GetSymbolNames();

                Console.WriteLine($"Available symbols count: {symbolResponse.SymbolCount()}");
                Console.WriteLine($"Available symbol names: ");

                foreach (var symbol in symbolList)
                {
                    Console.Write($"{symbol,10} ");

                    if (++symbolCount % 10 == 0)
                        Console.WriteLine();
                }
            }

            Console.WriteLine($"\n\nQueryStatus = {mtClient.LastQueryStatus()}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage()}");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();

            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}
