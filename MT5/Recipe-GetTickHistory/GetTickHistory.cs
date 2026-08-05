using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.MTsocketAPI.Responses.MT5;
namespace Recipe_GetTickHistory;

public class GetTickHistory
{
    static async Task Main(string[] args)
    {
        MT5Client mtClient = new();

        try
		{
            if (!mtClient.StatusIsOK)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            string fromDate = "2025.09.30 20:05:00";
            string toDate = "2025.09.30 20:10:00";
            string symbol = "EURUSD";

            TickHistory tickHistory = await mtClient.GetTickHistoryAsync(fromDate, toDate, symbol, TickFlag.COPY_TICKS_ALL);

            Console.WriteLine($"Tick history for {symbol} from {fromDate} to {toDate}:");
            Console.WriteLine(tickHistory);

        }
		catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}

