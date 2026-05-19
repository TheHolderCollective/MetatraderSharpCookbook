using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
namespace Recipe_GetTerminal_Info;

/// <summary>
/// Get Terminal Info Recipe
/// </summary>
public class Program
{
    static void Main(string[] args)
    {
        MetatraderClient mtClient = new();

        try
        {
            TerminalInfo? myTerminal = mtClient.GetTerminalInfo();

            Console.WriteLine(myTerminal);
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();

            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}
