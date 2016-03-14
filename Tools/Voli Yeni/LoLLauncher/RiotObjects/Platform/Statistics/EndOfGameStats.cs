namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Team;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EndOfGameStats : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public EndOfGameStats()
        {
            this.type = "com.riotgames.platform.statistics.EndOfGameStats";
        }

        public EndOfGameStats(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.EndOfGameStats";
            this.callback = callback;
        }

        public EndOfGameStats(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.EndOfGameStats";
            base.SetFields<EndOfGameStats>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<EndOfGameStats>(this, result);
            this.callback(this);
        }

        [InternalName("basePoints")]
        public int BasePoints { get; set; }

        [InternalName("boostIpEarned")]
        public double BoostIpEarned { get; set; }

        [InternalName("boostXpEarned")]
        public double BoostXpEarned { get; set; }

        [InternalName("completionBonusPoints")]
        public int CompletionBonusPoints { get; set; }

        [InternalName("coOpVsAiMinutesLeftToday")]
        public int CoOpVsAiMinutesLeftToday { get; set; }

        [InternalName("coOpVsAiMsecsUntilReset")]
        public double CoOpVsAiMsecsUntilReset { get; set; }

        [InternalName("customMinutesLeftToday")]
        public int CustomMinutesLeftToday { get; set; }

        [InternalName("customMsecsUntilReset")]
        public double CustomMsecsUntilReset { get; set; }

        [InternalName("difficulty")]
        public object Difficulty { get; set; }

        [InternalName("elo")]
        public int Elo { get; set; }

        [InternalName("eloChange")]
        public int EloChange { get; set; }

        [InternalName("experienceEarned")]
        public double ExperienceEarned { get; set; }

        [InternalName("experienceTotal")]
        public double ExperienceTotal { get; set; }

        [InternalName("firstWinBonus")]
        public double FirstWinBonus { get; set; }

        [InternalName("gameId")]
        public double GameId { get; set; }

        [InternalName("gameLength")]
        public double GameLength { get; set; }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("gameType")]
        public string GameType { get; set; }

        [InternalName("imbalancedTeamsNoPoints")]
        public bool ImbalancedTeamsNoPoints { get; set; }

        [InternalName("invalid")]
        public bool Invalid { get; set; }

        [InternalName("ipEarned")]
        public double IpEarned { get; set; }

        [InternalName("ipTotal")]
        public double IpTotal { get; set; }

        [InternalName("leveledUp")]
        public bool LeveledUp { get; set; }

        [InternalName("loyaltyBoostIpEarned")]
        public double LoyaltyBoostIpEarned { get; set; }

        [InternalName("loyaltyBoostXpEarned")]
        public double LoyaltyBoostXpEarned { get; set; }

        [InternalName("myTeamInfo")]
        public TeamInfo MyTeamInfo { get; set; }

        [InternalName("myTeamStatus")]
        public string MyTeamStatus { get; set; }

        [InternalName("newSpells")]
        public List<object> NewSpells { get; set; }

        [InternalName("odinBonusIp")]
        public int OdinBonusIp { get; set; }

        [InternalName("otherTeamInfo")]
        public TeamInfo OtherTeamInfo { get; set; }

        [InternalName("otherTeamPlayerParticipantStats")]
        public List<PlayerParticipantStatsSummary> OtherTeamPlayerParticipantStats { get; set; }

        [InternalName("pointsPenalties")]
        public List<object> PointsPenalties { get; set; }

        [InternalName("queueBonusEarned")]
        public int QueueBonusEarned { get; set; }

        [InternalName("queueType")]
        public string QueueType { get; set; }

        [InternalName("ranked")]
        public bool Ranked { get; set; }

        [InternalName("reportGameId")]
        public object ReportGameId { get; set; }

        [InternalName("roomName")]
        public object RoomName { get; set; }

        [InternalName("roomPassword")]
        public object RoomPassword { get; set; }

        [InternalName("rpEarned")]
        public double RpEarned { get; set; }

        [InternalName("sendStatsToTournamentProvider")]
        public bool SendStatsToTournamentProvider { get; set; }

        [InternalName("skinIndex")]
        public int SkinIndex { get; set; }

        [InternalName("summonerName")]
        public string SummonerName { get; set; }

        [InternalName("talentPointsGained")]
        public int TalentPointsGained { get; set; }

        [InternalName("teamPlayerParticipantStats")]
        public List<PlayerParticipantStatsSummary> TeamPlayerParticipantStats { get; set; }

        [InternalName("timeUntilNextFirstWinBonus")]
        public double TimeUntilNextFirstWinBonus { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public object UserId { get; set; }

        public delegate void Callback(EndOfGameStats result);
    }
}

