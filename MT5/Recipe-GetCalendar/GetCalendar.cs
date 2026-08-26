using MetatraderSharp.MTsocketAPI.Responses.MT5;
using MetatraderSharp.MetatraderClient;
namespace Recipe_GetCalendar;

/// <summary>
/// Recipe showing how to get calendar info
/// This is only available in Metatrader 5
/// </summary>
public class GetCalendar
{
    static async Task Main(string[] args)
    {
        MT5Client mtClient = new();

        try
        {
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            string fromDate = "2025.01.26 21:15:00";
            string toDate = "2025.01.30 21:15:00";
            string countryCode = "US";
            string currency = "USD"; // this is optional

            Calendar usCalendar = await mtClient.GetCalendarAsync(fromDate, toDate, countryCode, currency);

            Console.WriteLine($"Calendar events for {countryCode} from {fromDate} to {toDate}:");
            Console.WriteLine(usCalendar);

        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}

