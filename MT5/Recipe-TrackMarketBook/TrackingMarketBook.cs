using MetatraderSharp.MetatraderClient;
using MetatraderSharp.MTsocketAPI.Responses.Common;
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
            if (mtClient.ClientStatusIsError())
            {
                Console.WriteLine("Unable to connect to a Metatrader terminal. Please check that an instance of Metatrader is running and that the MTsocketAPI EA is correcty loaded onto a chart.");
                return;
            }

            TrackResponse mbookResponse = await mtClient.TrackMarketBookAsync("EURUSD", "AUDCAD");
            Console.WriteLine("Track Market Book response:");
            Console.WriteLine(mbookResponse);

            // Check that tracking started successfully
            if (mtClient.LastQueryFailed())
            {
                Console.WriteLine($"An error occured: {mtClient.LastQueryMessage()}");
                return;
            }

            Console.WriteLine("\n--> Please note that market book tracking only works if the broker supplies DOM data.\n");
            Console.WriteLine($"Press [Enter] to start tracking.");
            Console.ReadLine();
            Console.WriteLine("Market Book tracking started.\nPress **Escape** to stop tracking\n");

            // Once tracking is started, a websocket connection needs to be made
            // Default url to use for the connection: ws://127.0.0.1:81
            using var webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri($"ws://127.0.0.1:{mtClient.WebSocketPort}"), CancellationToken.None);

            if (webSocket.State == WebSocketState.Open)
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket successfully connected.\n");

                ParameterizedThreadStart ts = new(GetMarketBookData);
                Thread getMarketBookDataThread = new(ts);
                getMarketBookDataThread.Start(webSocket);

                ConsoleKeyInfo cki = new();

                while (cki.Key != ConsoleKey.Escape)
                {
                    cki = Console.ReadKey();

                    if (cki.Key == ConsoleKey.Escape)
                    {
                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    }
                }

                getMarketBookDataThread.Join();
                Console.WriteLine($"Websocket state: {webSocket.State}");
            }
            else
            {
                Console.WriteLine($"Websocket state: {webSocket.State}");
                Console.WriteLine("Websocket unable to connect.\n");
            }

            mbookResponse = await mtClient.TrackMarketBookAsync("");

            Console.WriteLine("Track Market Book response::");
            Console.WriteLine(mbookResponse);
            Console.WriteLine("Market book tracking ended.");
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();
            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }

    public static async void GetMarketBookData(object? socket)
    {
        // Create a buffer for storing data
        byte[] buffer = new byte[4096];
        int depthCount = 0;

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
                    var output = JsonConvert.DeserializeObject<MarketDepth>(jsonData);

                    Console.WriteLine($"{++depthCount}: {output}");
                }
            }
        }
    }
}

