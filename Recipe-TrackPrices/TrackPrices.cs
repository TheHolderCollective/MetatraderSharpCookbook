using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
using Newtonsoft.Json;
using System.Text;
using System.Net.WebSockets;
namespace Recipe_TrackPrices;

/// <summary>
/// Recipe for live tracking of symbol prices 
/// </summary>
public class TrackPricesRecipe
{
    static async Task Main(string[] args)
    {
        MetatraderClient mtClient = new(TerminalType.MT4);

        try
        {
            if (!mtClient.StatusIsOK)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            // Up to a maximum of 5 symbols can be tracked
            // Assignment of result not necessary (only if needed)
            TrackPricesResponse ptResponse = mtClient.PriceTracker(TrackingCommand.Start, "EURUSD","CADJPY","GBPJPY");

            Console.WriteLine("Track prices response:");
            Console.WriteLine(ptResponse + "\n");

            // Check that tracking started successfully
            // This can also be checked by the ErrorID property of the TrackPricesResponse object
            // E.g. ptResponse.ErrorID == 0
            if (mtClient.LastQueryStatus == QueryStatus.Error)
            {
                // Tracking may continue if there is a symbol select error with one of the symbols, so we send a stop command to head this off
                mtClient.PriceTracker(TrackingCommand.Stop);
                Console.WriteLine($"An error occured: {mtClient.LastQueryMessage}");
                return;
            }

            Console.WriteLine("Price tracking started.");

            // Once tracking is started, a websocket connection needs to be made
            // Default url to use for the connection: ws://127.0.0.1:81
            using var webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri($"ws://127.0.0.1:{mtClient.WebSocketPort}"), CancellationToken.None);

            if (webSocket.State == WebSocketState.Open)
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket successfully connected.\n");
            }
            else
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket unable to connect.\n");
                Console.WriteLine("Price tracking ended.");

                // Stop tracking prices
                mtClient.PriceTracker(TrackingCommand.Stop);
                return;
            }

            // Create a buffer for storing data
            byte[] buffer = new byte[4096];

            int priceCount = 0;
            const int maxPricesReceived = 15;

            while (webSocket.State == WebSocketState.Open)
            {
                // Read streamed data into the buffer
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType != WebSocketMessageType.Close)
                {
                    // process data and output it to the console
                    string jsonData = Encoding.ASCII.GetString(buffer, 0, result.Count);
                    var output = JsonConvert.DeserializeObject<TrackPrices>(jsonData);
                    Console.WriteLine($"{priceCount + 1}: {output}");

                    // close websocket after maxPricesReceived items received
                    if (++priceCount >= maxPricesReceived)
                    {
                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                        Console.WriteLine(result.CloseStatusDescription);
                    }
                }
                else
                {
                    await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    Console.WriteLine(result.CloseStatusDescription);
                }
            }

            // Stop tracking prices
            mtClient.PriceTracker(TrackingCommand.Stop, "EURUSD");

            Console.WriteLine($"\nWebsocket state: {webSocket.State}");
            Console.WriteLine("Price tracking ended.");

        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }

}
