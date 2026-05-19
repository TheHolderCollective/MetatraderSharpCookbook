using MetatraderSharp;
using MetatraderSharp.MTsocketAPI.Responses;
namespace MetatraderSharp_Examples;

internal class Program
{
    static void Main(string[] args)
    {

        MetatraderClient mtClient = new();

        try
        {
            Account? myAccount = mtClient.GetAccountInfo();

            Console.WriteLine(myAccount);
        }
        catch (Exception ex)
        {
            string exceptionName = ex.GetType().ToString();

            Console.WriteLine($"{exceptionName}: {ex.Message}");
        }
    }
}