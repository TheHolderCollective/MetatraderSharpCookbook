using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.MT5;
namespace Recipe_Indicators;

/// <summary>
/// Recipe demonstrating how to get indicator values
/// </summary>
public class GetIndicators
{
    static async Task Main(string[] args)
    {
        MT5Client mtClient = new();

        try
        {
            if (mtClient.ClientStatusIsError)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            Indicator atrIndicator = await mtClient.GetATRValues(14, 0, "EURUSD", TimeframesMT5.Period_M12);

            if (mtClient.LastQueryStatus == QueryStatus.Ok)
            {
                Console.WriteLine("ATR Indicator: ");
                Console.WriteLine(atrIndicator);
            }

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}\n");


            Indicator maIndicator = await mtClient.GetMAValues(AppliedPrice.Price_Close, MA_Method.Mode_EMA, 21, 5, 1, "EURUSD", TimeframesMT5.Period_M5);

            if (mtClient.LastQueryStatus == QueryStatus.Ok)
            {
                Console.WriteLine("\nMA Indicator: ");
                Console.WriteLine(maIndicator);
            }

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}\n");


            string indicatorName = "Examples\\OsMA";
           
            Indicator customIndicator = await mtClient.GetCustomIndicatorValues(indicatorName, "EURUSD", TimeframesMT5.Period_M5, 0, 10);
           
            if (mtClient.LastQueryStatus == QueryStatus.Ok)
            {
                Console.WriteLine($"\nCustom Indicator ({indicatorName}): ");
                Console.WriteLine(customIndicator);
            }

            Console.WriteLine($"\nQueryStatus = {mtClient.LastQueryStatus}");
            Console.WriteLine($"QueryMessage = {mtClient.LastQueryMessage}\n");

        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
       
    }
}

