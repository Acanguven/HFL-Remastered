namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PlayerGameStats : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerGameStats()
        {
            this.type = "com.riotgames.platform.statistics.PlayerGameStats";
        }

        public PlayerGameStats(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.PlayerGameStats";
            this.callback = callback;
        }

        public PlayerGameStats(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.PlayerGameStats";
            base.SetFields<PlayerGameStats>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerGameStats>(this, result);
            this.callback(this);
        }

        [InternalName("adjustedRating")]
        public int AdjustedRating { get; set; }

        [InternalName("afk")]
        public bool Afk { get; set; }

        [InternalName("boostIpEarned")]
        public double BoostIpEarned { get; set; }

        [InternalName("boostXpEarned")]
        public double BoostXpEarned { get; set; }

        [InternalName("championId")]
        public double ChampionId { get; set; }

        [InternalName("createDate")]
        public DateTime CreateDate { get; set; }

        [InternalName("difficulty")]
        public object Difficulty { get; set; }

        [InternalName("difficultyString")]
        public object DifficultyString { get; set; }

        [InternalName("eligibleFirstWinOfDay")]
        public bool EligibleFirstWinOfDay { get; set; }

        [InternalName("eloChange")]
        public int EloChange { get; set; }

        [InternalName("experienceEarned")]
        public double ExperienceEarned { get; set; }

        [InternalName("fellowPlayers")]
        public List<FellowPlayerInfo> FellowPlayers { get; set; }

        [InternalName("gameId")]
        public double GameId { get; set; }

        [InternalName("gameMapId")]
        public int GameMapId { get; set; }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("gameType")]
        public string GameType { get; set; }

        [InternalName("gameTypeEnum")]
        public string GameTypeEnum { get; set; }

        [InternalName("id")]
        public object Id { get; set; }

        [InternalName("invalid")]
        public bool Invalid { get; set; }

        [InternalName("ipEarned")]
        public double IpEarned { get; set; }

        [InternalName("KCoefficient")]
        public double KCoefficient { get; set; }

        [InternalName("leaver")]
        public bool Leaver { get; set; }

        [InternalName("level")]
        public double Level { get; set; }

        [InternalName("predictedWinPct")]
        public double PredictedWinPct { get; set; }

        [InternalName("premadeSize")]
        public int PremadeSize { get; set; }

        [InternalName("premadeTeam")]
        public bool PremadeTeam { get; set; }

        [InternalName("queueType")]
        public string QueueType { get; set; }

        [InternalName("ranked")]
        public bool Ranked { get; set; }

        [InternalName("rating")]
        public double Rating { get; set; }

        [InternalName("rawStatsJson")]
        public object RawStatsJson { get; set; }

        [InternalName("skinIndex")]
        public int SkinIndex { get; set; }

        [InternalName("skinName")]
        public object SkinName { get; set; }

        [InternalName("spell1")]
        public double Spell1 { get; set; }

        [InternalName("spell2")]
        public double Spell2 { get; set; }

        [InternalName("statistics")]
        public List<RawStat> Statistics { get; set; }

        [InternalName("subType")]
        public string SubType { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("teamId")]
        public double TeamId { get; set; }

        [InternalName("teamRating")]
        public int TeamRating { get; set; }

        [InternalName("timeInQueue")]
        public int TimeInQueue { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public double UserId { get; set; }

        [InternalName("userServerPing")]
        public int UserServerPing { get; set; }

        public delegate void Callback(PlayerGameStats result);
    }
}

