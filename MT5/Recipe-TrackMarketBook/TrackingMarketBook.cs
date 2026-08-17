using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Recipe_TrackMarketBook;

/// <summary>
///   Recipe for live tracking of market depth. This functionality is only available in MT5.
///   Broker must supply DOM data otherwise the recipe won't work as expected.
/// </summary>
public class TrackingMarketBook
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

            TrackResponse mbookResponse = await mtClient.TrackMarketBookAsync("EURUSD", "AUDCAD");
            Console.WriteLine("Track Market Book response:");
            Console.WriteLine(mbookResponse);

         
            // Check that tracking started successfully
            if (mtClient.LastQueryFailed())
            {
                Console.WriteLine($"An error occured: {mtClient.LastQueryMessage}");
                return;
            }

            Console.WriteLine("Market Book tracking started.");

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
                Console.WriteLine("Order event tracking ended.");

                // Stop tracking order events
                await mtClient.TrackMarketBookAsync("");
                return;
            }


            // Create a buffer for storing data
            byte[] buffer = new byte[4096];

            int mbookCount = 0;
            const int maxCount = 5;

            while (webSocket.State == WebSocketState.Open)
            {
                // Read streamed data into the buffer
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType != WebSocketMessageType.Close)
                {
                    // process data and output it to the console
                    string jsonData = Encoding.ASCII.GetString(buffer, 0, result.Count);
                    Console.WriteLine(jsonData);

                    var output = (jsonData != null) ? JsonConvert.DeserializeObject<MarketDepth>(jsonData) : null;
                    Console.WriteLine($"{mbookCount + 1}: {output}");

                    // close websocket after max number of mbook items received
                    if (++mbookCount >= maxCount)
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

            mbookResponse = await mtClient.TrackMarketBookAsync("");
            Console.WriteLine("Track Market Book response::");
            Console.WriteLine(mbookResponse);
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}

