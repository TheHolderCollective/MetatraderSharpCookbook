using MetatraderSharp;
using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
namespace Recipe_GetPriceHistory;

public class GetPriceHistory
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

            PriceHistory myPriceHistory = await mtClient.GetPriceHistoryAsync("EURUSD", TimeFrameMT4.PERIOD_M5, "2025.01.22 17:10:00", "2025.01.22 20:00:00");

            if (mtClient.LastQueryStatus == QueryStatus.OK)
            {
                Console.WriteLine($"{nameof(myPriceHistory.Msg)}: {myPriceHistory.Msg}");
                Console.WriteLine($"{nameof(myPriceHistory.Symbol)}: {myPriceHistory.Symbol}");
                Console.WriteLine($"{nameof(myPriceHistory.TimeFrame)}: {myPriceHistory.TimeFrame}");
                Console.WriteLine($"Available OHLCs count: {myPriceHistory.Rates.Count}");

                List<Rate> justOHLCs = myPriceHistory.Rates;

                Console.WriteLine($"\nAvailable OHLCs:");

                foreach (var ohlc in justOHLCs)
                {
                    Console.WriteLine(ohlc);
                }
            }

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
