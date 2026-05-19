using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
namespace Recipe_GetSymbolList;

internal class Program
{
    static void Main(string[] args)
    {
        MetatraderClient mtClient = new();
        int symbolCount = 0;

        try
        {
            List<Symbol> symbolList = mtClient.GetSymbolList();

            Console.WriteLine($"Available symbols count: {symbolList.Count}");
            Console.WriteLine($"Available symbol names: ");

            foreach(var symbol in symbolList)
            {
                Console.Write($"{symbol.Name,10} ");

                if (++symbolCount % 10 == 0)
                    Console.WriteLine();
            }

            Console.Write("\nPress any key to continue...");
            Console.ReadLine();

            SymbolList? symbolResponse = mtClient.GetSymbolListResponse();
            Console.WriteLine("\nComplete deserialized JSON response from get symbol list query:");
            Console.WriteLine(symbolResponse);

            Console.Write("Press any key to exit...");
            Console.ReadLine();

        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();

            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}
