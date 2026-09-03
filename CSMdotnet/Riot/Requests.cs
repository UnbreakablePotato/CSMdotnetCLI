using DotNetEnv;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

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
            catch (Exception e)
            {
                Console.WriteLine($"API Request failed for Ladder: {e.Message}");
                return null;
            }

        }

    }

    /*
     * Gets a list of matches based on a puuid and an amount
     * from the /lol/match/v5/matches/by-puuid/{puuid}/ids endpoint
     */
    public record MatchIDS(List<string>? Matches);

    public class MatchIDService : Services
    {
        public async Task<MatchIDS?> GetMatchIDSAsync(string region, string puuid, int amount)
        {
            string fullUrl = "https://" + region + ".api.riotgames.com/lol/match/v5/matches/by-puuid/" + puuid + "/ids?start=0&count=" + amount;

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

                request.Headers.Add("X-Riot-Token", apiKey);

                using var response = await SharedClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<MatchIDS>();
            }
            catch(Exception e)
            {
                Console.WriteLine($"API Request failed for Ladder: {e.Message}");
                return null;
            }

        }
    }
public record RiotMatchResponse(
    Metadata Metadata,
    Info Info
);

    public record Metadata(
        string DataVersion,
        string MatchId,
        IReadOnlyList<string> Participants
    );

    public record Info(
        string EndOfGameResult,
        long GameCreation,
        long GameDuration,
        long GameEndTimestamp,
        long GameId,
        string GameMode,
        string GameName,
        long GameStartTimestamp,
        string GameType,
        string GameVersion,
        int MapId,
        IReadOnlyList<Participant> Participants,
        string PlatformId,
        int QueueId,
        IReadOnlyList<Team> Teams,
        string TournamentCode
    );

    public record Team(
        IReadOnlyList<Ban> Bans,
        Objectives Objectives,
        int TeamId,
        bool Win
    );

    public record Ban(
        int ChampionId,
        int PickTurn
    );

    public record Objectives(
        Objective Atakhan,
        Objective Baron,
        Objective Champion,
        Objective Dragon,
        Objective Horde,
        Objective Inhibitor,
        Objective RiftHerald,
        Objective Tower
    );

    public record Objective(
        bool First,
        int Kills
    );

    public record Participant(
        int Assists,
        int ChampLevel,
        int ChampionId,
        string ChampionName,
        int Deaths,
        int GoldEarned,
        int GoldSpent,
        int Item0,
        int Item1,
        int Item2,
        int Item3,
        int Item4,
        int Item5,
        int Item6,
        int Kills,
        string Lane,
        string Puuid,
        string RiotIdGameName,
        string RiotIdTagline,
        string Role,
        string SummonerName,
        int TotalDamageDealtToChampions,
        int TotalDamageTaken,
        int TotalMinionsKilled,
        int VisionScore,
        bool Win,
        Perks Perks,

        // Riot adds new fields constantly. This dictionary catches ANY properties 
        // that are in the JSON but not explicitly defined in this record.
        [property: JsonExtensionData] Dictionary<string, System.Text.Json.JsonElement>? ExtensionData = null
    );

    public record Perks(
        StatPerks StatPerks,
        IReadOnlyList<PerkStyle> Styles
    );

    public record StatPerks(
        int Defense,
        int Flex,
        int Offense
    );

    public record PerkStyle(
        string Description,
        IReadOnlyList<PerkSelection> Selections,
        [property: JsonPropertyName("style")] int StyleId
    );

    public record PerkSelection(
        int Perk,
        int Var1,
        int Var2,
        int Var3
    );

    public class MatchDataService : Services
    {
        public async Task<RiotMatchResponse?> GetRiotMatchDataAsync()
        {
            string fullUrl = "";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

                request.Headers.Add("X-Riot-Token", apiKey);

                using var response = await SharedClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<RiotMatchResponse>();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
