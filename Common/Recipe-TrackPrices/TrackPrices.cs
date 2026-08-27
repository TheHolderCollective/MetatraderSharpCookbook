using MetatraderSharp;
using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.Extensions;
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
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            string[] symbols = { "EURUSD", "CADJPYs", "GBPCHFs" };

            TrackResponse ptResponse = await mtClient.TrackPricesAsync(TrackingCommand.Start, symbols);

            Console.WriteLine("Track prices response:");
            Console.WriteLine(ptResponse + "\n");

            // Check for failures
            if (ptResponse.SuccessCount() == 0)
            {
                Console.WriteLine("Unable to proceed.");
                Console.WriteLine($"Tracking for all symbols failed: {ptResponse.FailedSymbols()}");
                Console.WriteLine($"Error: {mtClient.LastQueryMessage()}\n");
                return;
            }
            else if (ptResponse.FailCount() > 0 && ptResponse.SuccessCount() > 0)
            {
                Console.WriteLine($"An error occured tracking one or more symbols: {ptResponse.FailedSymbols()}");
                Console.WriteLine($"Error: {mtClient.LastQueryMessage()}");
                Console.WriteLine($"Price tracking will start for: {ptResponse.SuccessfulSymbols()}\n");
            }

            Console.WriteLine($"Price tracking started.");

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
