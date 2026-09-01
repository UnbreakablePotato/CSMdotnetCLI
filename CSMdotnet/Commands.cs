using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

public record User(string? Puuid, string? GameName, string? TagLine);

namespace CSMdotnet
{
    internal class Commands
    {

        public static async Task Search(string GameName, string TagLine)
        {

            var puuid = new CSMdotnet.Riot.PuuidService(GameName, TagLine);


            CSMdotnet.Riot.GetPuuid? req = await puuid.GetPuuidAsync();

            var entry = new CSMdotnet.Riot.EntriesService(req.Puuid);

            List<CSMdotnet.Riot.Entry>? entriesRes = await entry.GetLeagueEntriesAsync();

            Console.WriteLine($"{GameName} {TagLine}");
            Console.WriteLine($"{entriesRes[0].Rank} {entriesRes[0].Tier} {entriesRes[0].LeaguePoints} LP");
            Console.WriteLine($"{entriesRes[0].Wins} {entriesRes[0].Losses}");
            Console.WriteLine($"Winrate: {((double)entriesRes[0].Wins / (double)(entriesRes[0].Wins + (double)entriesRes[0].Losses))*100.0}%");

        }

        public static async Task ShowShallowMatch(string matchID, int amount)
        {

        }

        public static async Task ShowDeepMatch(string matchID)
        {

        }

    }


    public class SearchUserService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<User?> SearchUserAsync(int id)
        {
            string fullUrl = "";

            try
            {
                User? result = await _httpClient.GetFromJsonAsync<User>(fullUrl);
                return result;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine($"API Request failed: {e.Message}");
                return null;
            }
        }

    }
}
