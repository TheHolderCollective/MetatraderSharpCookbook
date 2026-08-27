using MetatraderSharp.Extensions;
using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
namespace Recipe_GetPriceHistory;

/// <summary>
/// Recipe showing how to get the price history of a symbol
/// </summary>
public class GetPriceHistory
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();
        string fromDate = "2026.07.09 12:10:00";
        string toDate = "2026.07.09 15:10:00";

        try
        {
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            PriceHistory myPriceHistory = await mtClient.GetPriceHistoryAsync("EURUSD", TimeframesMT4.Period_M1, fromDate, toDate);

            if (mtClient.LastQuerySuccessful())
            {
                Console.WriteLine($"{nameof(myPriceHistory.Msg)}: {myPriceHistory.Msg}");
                Console.WriteLine($"{nameof(myPriceHistory.Symbol)}: {myPriceHistory.Symbol}");
                Console.WriteLine($"{nameof(myPriceHistory.TimeFrame)}: {myPriceHistory.TimeFrame}");
                Console.WriteLine($"Start Date: {fromDate} \nEnd Date: {toDate}");
                Console.WriteLine($"Available OHLCs count: {myPriceHistory.RateCount()}");

                List<Rate> justOHLCs = myPriceHistory.GetRates();

                Console.WriteLine($"\nAvailable OHLCs:");

                foreach (var ohlc in justOHLCs)
                {
                    Console.WriteLine(ohlc);
                }
                Console.WriteLine();

                Rate? maxOpenRate = myPriceHistory.GetMaxOpenRate();
                Rate? minOpenRate = myPriceHistory.GetMinOpenRate();

                Console.WriteLine($"Rate with maximum open value: \n{maxOpenRate}");
                Console.WriteLine($"Rate with minimum open value: \n{minOpenRate}");

            }

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
