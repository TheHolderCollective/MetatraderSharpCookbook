using MetatraderSharp;
using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.MTsocketAPI.Responses.MT4;
namespace Recipe_Indicators;

/// <summary>
/// Recipe demonstrating how to get indicator values
/// </summary>
public class GetIndicators
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

            AverageTrueRangeIndicator atrIndicator = await mtClient.GetATRValues(14, 0, "EURUSD", TimeframesMT4.Period_M5);

            if (mtClient.LastQueryStatus == QueryStatus.OK)
            {
                Console.WriteLine("ATR Indicator: ");
                Console.WriteLine(atrIndicator);
            }

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}\n");

            MovingAverageIndicator maIndicator = await mtClient.GetMAValues(AppliedPrice.Price_Close, MA_Method.Mode_EMA, 21, 1, "EURUSD", TimeframesMT4.Period_M5);

            if (mtClient.LastQueryStatus == QueryStatus.OK)
            {
                Console.WriteLine("MA Indicator: ");
                Console.WriteLine(maIndicator);
            }

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}\n");

            string indicatorName = "OsMa";

            CustomIndicator customIndicator = await mtClient.GetCustomIndicatorValues(indicatorName, 0, 0, "EURUSD", TimeframesMT4.Period_M5);

            if (mtClient.LastQueryStatus == QueryStatus.OK)
            {
                Console.WriteLine($"Custom Indicator ({indicatorName}): ");
                Console.WriteLine(customIndicator);
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
