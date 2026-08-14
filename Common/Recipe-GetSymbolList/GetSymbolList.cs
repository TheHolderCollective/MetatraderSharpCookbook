using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
using MetatraderSharp.MTsocketAPI.Responses;
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
            if (mtClient.StatusIsError)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            SymbolList? symbolResponse = await mtClient.GetSymbolListAsync();

            Console.WriteLine("\nSymbol list response:");
            Console.WriteLine(symbolResponse);

            Console.Write("\nPress any key to continue...\n");
            Console.ReadLine();

            List<Symbol> symbolList = symbolResponse.Symbols;

            if (mtClient.LastQueryStatus == QueryStatus.Ok)
            {
                Console.WriteLine($"Available symbols count: {symbolList.Count}");
                Console.WriteLine($"Available symbol names: ");

                foreach (var symbol in symbolList)
                {
                    Console.Write($"{symbol.Name,10} ");

                    if (++symbolCount % 10 == 0)
                        Console.WriteLine();
                }

            }

            Console.WriteLine($"\n\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadLine();

        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();

            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}
