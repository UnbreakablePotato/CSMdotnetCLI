using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using DotNetEnv;

namespace CSMdotnet.Riot
{
    public record BaseInfo(string? Puuid, string? GameName, string? TagLine);

    public class Services
    {
        public string? apiKey = Environment.GetEnvironmentVariable("api_key");

        protected static readonly HttpClient SharedClient = new HttpClient();
    }
    /*
     Gets puuid from game name and tagline
     from the /riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine} endpoint
     */
    public class PuuidService : Services
    {

        private string gameName { get; set; }
        private string tagLine { get; set; }

        public PuuidService(string GameName, string TagLine)
        {
            gameName = GameName;
            tagLine = TagLine;
        }

        public async Task<BaseInfo?> GetPuuidAsync()
        {
            string fullUrl = "https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/" + gameName + "/" + tagLine;

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

                request.Headers.Add("X-Riot-Token", apiKey);

                using var response = await SharedClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<BaseInfo>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"{apiKey}");
                Console.WriteLine($"API Request failed for BaseInfo from gameName and tagLine: {e.Message}");
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

    /*
     Gets a players profile with information such as tier, rank, wins and losses etc.
     from the /lol/league/v4/entries/by-puuid/{encryptedPUUID} endpoint
    */
    public class EntriesService : Services
    {
        private string puuid { get; set; }
        public EntriesService(string Puuid)
        {
            puuid = Puuid;
        }

        public async Task<List<Entry>?> GetLeagueEntriesAsync()
        {
            string fullUrl = "https://euw1.api.riotgames.com/lol/league/v4/entries/by-puuid/" + puuid + "?api_key=" + apiKey;
            try
            {
                List<Entry>? res = await SharedClient.GetFromJsonAsync<List<Entry>>(fullUrl);
                return res;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"API Request failed for entries: {e.Message}");
                return null;
            }
        }
    }

    /*
     * Gets game name and tagline from puuid
     * from the /riot/account/v1/accounts/by-puuid/{puuid} endpoint
     */
    public class GameNameTagLineService : Services
    {
        private string puuid { get; set; }

        public GameNameTagLineService(string Puuid)
        {
            puuid = Puuid;
        }

        public async Task<BaseInfo?> GetGameNameTagAsync()
        {
            string fullUrl = "https://europe.api.riotgames.com/riot/account/v1/accounts/by-puuid/" + puuid;

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

                request.Headers.Add("X-Riot-Token", apiKey);

                using var response = await SharedClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<BaseInfo>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"API Request failed for BaseInfo from puuid: {e.Message}");
                return null;
            }
        }
    }

    public record LadderEntry(
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
    public record Ladder(
        string? Tier,
        string? Queue,
        List<LadderEntry> Entries
        );

    /* 
     * Gets the challenger ladder for a region of the users choice
     * from the /lol/league/v4/challengerleagues/by-queue/{queue} endpoint.
     */
    public class LadderService : Services
    {
        public async Task<Ladder?> GetLadderAsync(string region)
        {
            string fullUrl = "https://" + region + ".api.riotgames.com/lol/league/v4/challengerleagues/by-queue/RANKED_SOLO_5x5";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

                request.Headers.Add("X-Riot-Token", apiKey);

                using var response = await SharedClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<Ladder>();
            }
            catch(Exception e)
            {
                Console.WriteLine($"API Request failed for Ladder: {e.Message}");
                return null;
            }

        }

    }
}
