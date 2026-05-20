using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
namespace Recipe_GetPriceHistory;

public class GetPriceHistory
{
    static void Main(string[] args)
    {
        MetatraderClient mtClient = new();

        try
        {
            // Note: Use GetOHLCs() if no other info but the rates is required 
            // List<Rate> justOHLCs = mtClient.GetOHLCs("EURUSD", TimeFrame.PERIOD_H1, "2025.01.21 17:10:00", "2025.01.22 20:00:00");
            //

            PriceHistory myPriceHistory = mtClient.GetPriceHistoryResponse("EURUSD", TimeFrame.PERIOD_M5, "2025.01.21 17:10:00", "2025.01.22 20:00:00");

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
