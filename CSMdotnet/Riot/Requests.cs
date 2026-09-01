using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using DotNetEnv;

namespace CSMdotnet.Riot
{
    public record GetPuuid(string? Puuid, string? GameName, string? TagLine);

    public class Services
    {
        public string? apiKey = Environment.GetEnvironmentVariable("api_key");
    }
    public class PuuidService : Services
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
            string fullUrl = "https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/" + gameName + "/" + tagLine + "?api_key=" + apiKey;

            try
            {
                GetPuuid? result = await _httpClient.GetFromJsonAsync<GetPuuid>(fullUrl);
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"API Request failed: {e.Message}");
                return null;
            }
        }
    }

    //public record leagueEntries(List<Entry> Items);
    public record Entry(
        string? QueueType,
        string? Tier,
        string? Rank,
        string? Puuid,
        int? LeaguePoints,
        int? Wins,
        int? Losses,
        bool? Veteran,
        bool? Inactive,
        bool? FreshBlood,
        bool? HotStreak
        );

    public class EntriesService : Services
    {
        private string puuid;
        public EntriesService(string Puuid)
        {
            puuid = Puuid;
        }
        private static readonly HttpClient _client = new HttpClient();

        public async Task<List<Entry>?> GetLeagueEntriesAsync()
        {
            string fullUrl = "https://euw1.api.riotgames.com/lol/league/v4/entries/by-puuid/" + puuid + "?api_key=" + apiKey;
            try
            {
                List<Entry>? res = await _client.GetFromJsonAsync<List<Entry>>(fullUrl);
                return res;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"API Request failed: {e.Message}");
                return null;
            }
        }
    }
}
