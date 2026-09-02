using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;

//public record User(string? Puuid, string? GameName, string? TagLine);

namespace CSMdotnet
{
    internal class Commands
    {

        public static async Task Search(string GameName, string TagLine)
        {

            var puuid = new CSMdotnet.Riot.PuuidService(GameName, TagLine);


            CSMdotnet.Riot.BaseInfo? req = await puuid.GetPuuidAsync();

            var entry = new CSMdotnet.Riot.EntriesService(req.Puuid);

            List<CSMdotnet.Riot.Entry>? entriesRes = await entry.GetLeagueEntriesAsync();

            Console.WriteLine($"{GameName} {TagLine}");
            Console.WriteLine($"{entriesRes[0].Tier} {entriesRes[0].Rank} {entriesRes[0].LeaguePoints} LP");
            Console.WriteLine($"{entriesRes[0].Wins} Wins {entriesRes[0].Losses} Losses");
            if (entriesRes[0].Wins == 0)
            {
                Console.WriteLine("Winrate: 0%");
            }else
            {
                Console.WriteLine($"Winrate: {(entriesRes[0].Wins / (entriesRes[0].Wins + entriesRes[0].Losses)) * 100}%");
            }
            

        }

        public static async Task Ladder(string region)
        {
            string regionTag;

            switch (region) 
            {
                case "euw":
                    regionTag = "euw1";
                    break;
                case "eun":
                    regionTag = "eun1";
                    break;
                case "na":
                    regionTag = "na1";
                    break;
                case "kr":
                    regionTag = "kr";
                    break;
                default:
                    regionTag = "euw1";
                    break;
            }

            var reqObject = new CSMdotnet.Riot.LadderService();

            var ladderObject = await reqObject.GetLadderAsync(regionTag);

            for (var i = 0; i < ladderObject.Entries.Count; i++)
            {
                var reqBaseObject = new CSMdotnet.Riot.GameNameTagLineService(ladderObject.Entries[i].Puuid);

                var BaseInfoObject = await reqBaseObject.GetGameNameTagAsync();

                Console.WriteLine($"Ladder Rank: {i+1} {BaseInfoObject.GameName} {BaseInfoObject.TagLine}");
                Console.WriteLine($"{ladderObject.Entries[i].LeaguePoints} LP : Wins {ladderObject.Entries[i].Wins} : Losses {ladderObject.Entries[i].Losses}");
                Console.WriteLine($"Winrate {((double)ladderObject.Entries[i].Wins / (ladderObject.Entries[i].Wins + ladderObject.Entries[i].Losses)) * 100:F0}%");
                Console.WriteLine("=======================================");
                Console.WriteLine("=======================================\n");

                //Console.WriteLine($"");
            }

        }

        public static async Task ShowShallowMatch(string matchID, int amount)
        {

        }

        public static async Task ShowDeepMatch(string matchID)
        {

        }

    }
}


   /* public class SearchUserService
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
}*/
