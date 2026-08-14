using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Recipe_TrackOrderEvents;

/// <summary>
///  Recipe for live tracking of order events. Order event tracking functionality is only available in MT5.
/// </summary>
public class TrackingOrderEvents
{
    static async Task Main(string[] args)
    {
        MT5Client mtClient = new();

        try
        {
            if (mtClient.StatusIsError)
            {
                Console.WriteLine("Unable to connect to request URI.");
                return;
            }

            TrackOrderEventsResponse eventsResponse = await mtClient.TrackOrderEventsAsync(true);
            Console.WriteLine("Track order events response:");
            Console.WriteLine(eventsResponse);

            // Check that tracking started successfully
            if (mtClient.LastQueryFailed())
            {
                Console.WriteLine($"An error occured: {mtClient.LastQueryMessage}");
                return;
            }

            Console.WriteLine("Order event tracking started.");

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
                await mtClient.TrackOrderEventsAsync(false);
                return;
            }


            // Create a buffer for storing data
            byte[] buffer = new byte[4096];

            int eventCount = 0;
            const int maxCount = 30;

            while (webSocket.State == WebSocketState.Open)
            {
                // Read streamed data into the buffer
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType != WebSocketMessageType.Close)
                {
                    // process data and output it to the console
                    string jsonData = Encoding.ASCII.GetString(buffer, 0, result.Count);
                    var output = (jsonData != null) ? JsonConvert.DeserializeObject<TrackOrderEvents>(jsonData) : null;
                    Console.WriteLine($"{eventCount + 1}: {output}"); 

                    // close websocket after max number of events received
                    if (++eventCount >= maxCount)
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

            // Stop tracking order events
            await mtClient.TrackOrderEventsAsync(false);

            Console.WriteLine($"\nWebsocket state: {webSocket.State}");
            Console.WriteLine("Order event tracking ended.");

        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}

