using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.Common;
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

            // Define some valid symbol requests
            SymbolRequest symbolRequest1 = new("EURUSD", TimeframesMT4.Period_M1, 5);
            SymbolRequest symbolRequest2 = new("CADJPY", TimeframesMT4.Period_M1, 2);

            // Define an invalid symbol request
            SymbolRequest symbolRequest3 = new("CADJPPL", TimeframesMT4.Period_M1, 4);

            // Add symbol requests to a TrackOHLCRequest object
            TrackOHLCRequest ohlcRequest = new(symbolRequest1, symbolRequest2, symbolRequest3);

            // Track symbols
            TrackResponse ohlcResponse = await mtClient.TrackOHLCsAsync(ohlcRequest);

            Console.WriteLine("OHLC requests submitted:\n " + ohlcRequest);
            Console.WriteLine("\nTrack OHLC repsonse:\n " + ohlcResponse + "\n");

            // Check for failures
            if (ohlcResponse.SuccessCount() == 0 && ohlcResponse.FailCount() > 0)
            {
                Console.WriteLine("Unable to proceed.");
                Console.WriteLine($"Tracking for all symbols failed: {ohlcResponse.FailedSymbols()}");
                Console.WriteLine($"Error: {mtClient.LastQueryMessage()}\n");
                return;
            }
            else if (ohlcResponse.SuccessCount() > 0 && ohlcResponse.FailCount() > 0)
            {
                Console.WriteLine($"One or more invalid symbols requested: {ohlcResponse.FailedSymbols()}\n");
            }

            Console.WriteLine($"Press [Enter] to start tracking for: {ohlcResponse.SuccessfulSymbols()}");
            Console.ReadLine();
            Console.WriteLine($"OHLC tracking started.\nPress **Escape** to stop tracking\n");

            // Once tracking is started, a websocket connection needs to be made
            // Default url to use for the connection: ws://127.0.0.1:81
            using var webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri($"ws://127.0.0.1:81"), CancellationToken.None);

            if (webSocket.State == WebSocketState.Open)
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket successfully connected.");
                Console.WriteLine("Getting OHLC data...\n");

                ParameterizedThreadStart ts = new(GetOHLCsData);
                Thread getOHLCDataThread = new(ts);
                getOHLCDataThread.Start(webSocket);

                ConsoleKeyInfo cki = new();

                while (cki.Key != ConsoleKey.Escape)
                {
                    cki = Console.ReadKey();

                    if (cki.Key == ConsoleKey.Escape)
                    {
                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    }
                }

                getOHLCDataThread.Join();
                Console.WriteLine($"\nWebsocket state: {webSocket.State}");
            }
            else
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket unable to connect.\n");
            }

            // Stop tracking OHLCs
            var response = await mtClient.TrackOHLCsAsync(new TrackOHLCRequest());

            Console.WriteLine("Stop tracking response: \n" + response);
            Console.WriteLine("OHLC tracking ended.");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }

    public static async void GetOHLCsData(object? socket)
    {
        // Create a buffer for storing data
        byte[] buffer = new byte[4096];
        int ohlcCount = 0;

        if (socket is not null)
        {
            ClientWebSocket webSocket = (ClientWebSocket)socket;

            while (webSocket.State == WebSocketState.Open)
            {
                // Read streamed data into the buffer
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType != WebSocketMessageType.Close)
                {
                    // process data and output it to the console
                    string jsonData = Encoding.ASCII.GetString(buffer, 0, result.Count);
                    var output = JsonConvert.DeserializeObject<TrackOHLC>(jsonData);

                    Console.WriteLine($"{++ohlcCount}: {output}");
                }
            }
        }
    }
}