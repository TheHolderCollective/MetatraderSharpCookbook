using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses;
using MetatraderSharp.Extensions;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Recipe_TrackOHLC;

/// <summary>
/// Recipe which demonstrates the track prices fucntionality.
/// This works for both MT4 and MT5.
/// </summary>
public class TrackOHLCs
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

            // Define some symbol requests
            SymbolRequest symbolRequest1 = new()
            {
                Symbol = "EURUSD",
                TimeFrame = TimeframesMT4.Period_M1,
                Depth = 5
            };

            SymbolRequest symbolRequest2 = new()
            {
                Symbol = "CADJPY",
                TimeFrame = TimeframesMT4.Period_M1,
                Depth = 2
            };

            // Add symbol requests to a TrackOHLCRequest object
            TrackOHLCRequest ohlcRequest = new(symbolRequest1, symbolRequest2);

            // Track symbols
            TrackResponse ohlcResponse = await mtClient.TrackOHLCsAsync(ohlcRequest);

            Console.WriteLine("OHLC requests submitted:\n " +  ohlcRequest);
            Console.WriteLine("\nTrack OHLC repsonse:\n " + ohlcResponse + "\n");

            // Check for failures
            if (ohlcResponse.SuccessCount() == 0)
            {
                Console.WriteLine("Unable to proceed.");
                Console.WriteLine($"Tracking for all symbols failed: {ohlcResponse.FailedSymbols()}");
                Console.WriteLine($"Error: {mtClient.LastQueryMessage()}\n");
                return;
            }
            else if (ohlcResponse.FailCount() > 0 && ohlcResponse.SuccessCount() > 0)
            {
                Console.WriteLine($"An error occured tracking one or more symbols: {ohlcResponse.FailedSymbols()}");
                Console.WriteLine($"Error: {mtClient.LastQueryMessage()}");
                Console.WriteLine($"OHLC tracking will start for: {ohlcResponse.SuccessfulSymbols()}\n");
            }

            Console.WriteLine($"OHLC tracking started.");

            // Once tracking is started, a websocket connection needs to be made
            // Default url to use for the connection: ws://127.0.0.1:81
            using var webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri($"ws://127.0.0.1:81"), CancellationToken.None);

            if (webSocket.State == WebSocketState.Open)
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket successfully connected.\n");
            }
            else
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket unable to connect.\n");
                Console.WriteLine("OHLC tracking ended.");

                // Stop tracking OHLCs
                await mtClient.TrackOHLCsAsync(new TrackOHLCRequest());
                return;
            }

            // Create a buffer for storing data
            byte[] buffer = new byte[4096];

            int ohlcCount = 0;
            const int maxCount = 5;

            while (webSocket.State == WebSocketState.Open)
            {
                // Read streamed data into the buffer
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType != WebSocketMessageType.Close)
                {
                    // process data and output it to the console
                    string jsonData = Encoding.ASCII.GetString(buffer, 0, result.Count);
                    var output = (jsonData != null) ? JsonConvert.DeserializeObject<TrackOHLC>(jsonData) : null;
                    Console.WriteLine($"{ohlcCount + 1}: {output}");

                    // close websocket after max number of OHLC data items received
                    if (++ohlcCount >= maxCount)
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

            // Stop tracking OHLCs
            var response = await mtClient.TrackOHLCsAsync(new TrackOHLCRequest());
            Console.WriteLine(response);

            Console.WriteLine($"\nWebsocket state: {webSocket.State}");
            Console.WriteLine("OHLC tracking ended.");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }

    }
}
