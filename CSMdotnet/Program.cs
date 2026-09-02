using CSMdotnet;
using DotNetEnv;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class Program
{
    static async Task Main(string[] args)
    {
        Env.Load();
        while (true)
        {
            string? userInput;

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
            try
            {
                if (commandInput[0] == "quit")
                {
                    Environment.Exit(1);
                }
                if (commandInput[0] == "search")
                {
                    string? gameName = commandInput[1];
                    string? tagLine = commandInput[2];

                    await Commands.Search(gameName, tagLine);
                }
                if (commandInput[0] == "ladder")
                {
                    string? region = commandInput[1];

                    await Commands.Ladder(region);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught: {e}");
                continue;
            }
            

        }
    }
}

/*public record GetPuuid(string? Puuid,string? GameName, string? TagLine);

public class PuuidService
{

    private string gameName;
    private string tagLine;

    public PuuidService(string GameName, string TagLine)
    {
        gameName = GameName;
        tagLine = TagLine;
    }
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<GetPuuid?> GetPuuidAsync()
    {
        string fullUrl = "https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/" + gameName + "/" + tagLine + "?api_key=";

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
}*/