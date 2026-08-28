using CSMdotnet;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            string userInput;

            Console.Write("$ ");
            try
            {
                userInput = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught {e}");
                throw;
            }

            string[] commandInput;

            commandInput = Repl.cleanInput(userInput);

            if (commandInput[0] == "quit")
            {
                Environment.Exit(1);
            }

        }
    }
}

public record GetPuuid(string Puuid,string GameName, string TagLine);

public class PuuidService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<GetPuuid?> GetPuuidAsync(int id)
    {
        string fullUrl = "https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/SUPER%20CARRY/GANK?api_key=";

        try
        {
            GetPuuid? result = await _httpClient.GetFromJsonAsync<GetPuuid>(fullUrl);
            return result;
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine($"API Request failed: {e.Message}");
            return null;
        }
    }
}