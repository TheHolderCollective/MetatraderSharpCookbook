using MetatraderSharp;
using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using Newtonsoft.Json;
using System.Text;
using System.Net.WebSockets;

namespace Recipe_TrackPrices;

/// <summary>
/// Recipe for live tracking of symbol prices. 
/// This works for both MT4 and MT5.
/// </summary>
public class TrackPricesRecipe
{
    static async Task Main(string[] args)
    {
        MT4Client mtClient = new();

        try
        {
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            TrackResponse ptResponse = await mtClient.TrackPricesAsync(TrackingCommand.Start, "EURUSD", "CADJPY", "GBPCHF");

            Console.WriteLine("Track prices response:");
            Console.WriteLine(ptResponse + "\n");

            // Check that tracking started successfully
            if (mtClient.LastQueryFailed())
            {
                // Tracking may continue if there is a symbol select error with one of the symbols, so we send a stop command to head this off
                Console.WriteLine($"An error occured: {mtClient.LastQueryMessage()}");
                await mtClient.TrackPricesAsync(TrackingCommand.Stop);
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
                await mtClient.TrackPricesAsync(TrackingCommand.Stop);
                return;
            }

            // Create a buffer for storing data
            byte[] buffer = new byte[4096];

            int priceCount = 0;
            const int maxCount = 30;

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
                    if (++priceCount >= maxCount)
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
            await mtClient.TrackPricesAsync(TrackingCommand.Stop);

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
