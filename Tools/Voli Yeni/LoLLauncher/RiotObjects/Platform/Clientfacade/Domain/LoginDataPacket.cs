namespace LoLLauncher.RiotObjects.Platform.Clientfacade.Domain
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Kudos.Dto;
    using LoLLauncher.RiotObjects.Platform.Broadcast;
    using LoLLauncher.RiotObjects.Platform.Game;
    using LoLLauncher.RiotObjects.Platform.Statistics;
    using LoLLauncher.RiotObjects.Platform.Summoner;
    using LoLLauncher.RiotObjects.Platform.Systemstate;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LoginDataPacket : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public LoginDataPacket()
        {
            this.type = "com.riotgames.platform.clientfacade.domain.LoginDataPacket";
        }

        public LoginDataPacket(Callback callback)
        {
            this.type = "com.riotgames.platform.clientfacade.domain.LoginDataPacket";
            this.callback = callback;
        }

        public LoginDataPacket(TypedObject result)
        {
            this.type = "com.riotgames.platform.clientfacade.domain.LoginDataPacket";
            base.SetFields<LoginDataPacket>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LoginDataPacket>(this, result);
            this.callback(this);
        }

        [InternalName("allSummonerData")]
        public LoLLauncher.RiotObjects.Platform.Summoner.AllSummonerData AllSummonerData { get; set; }

        [InternalName("bingeData")]
        public object BingeData { get; set; }

        [InternalName("bingeIsPlayerInBingePreventionWindow")]
        public bool BingeIsPlayerInBingePreventionWindow { get; set; }

        [InternalName("bingeMinutesRemaining")]
        public double BingeMinutesRemaining { get; set; }

        [InternalName("bingePreventionSystemEnabledForClient")]
        public bool BingePreventionSystemEnabledForClient { get; set; }

        [InternalName("broadcastNotification")]
        public LoLLauncher.RiotObjects.Platform.Broadcast.BroadcastNotification BroadcastNotification { get; set; }

        [InternalName("clientSystemStates")]
        public ClientSystemStatesNotification ClientSystemStates { get; set; }

        [InternalName("competitiveRegion")]
        public string CompetitiveRegion { get; set; }

        [InternalName("coOpVsAiMinutesLeftToday")]
        public int CoOpVsAiMinutesLeftToday { get; set; }

        [InternalName("coOpVsAiMsecsUntilReset")]
        public double CoOpVsAiMsecsUntilReset { get; set; }

        [InternalName("customMinutesLeftToday")]
        public int CustomMinutesLeftToday { get; set; }

        [InternalName("customMsecsUntilReset")]
        public double CustomMsecsUntilReset { get; set; }

        [InternalName("gameTypeConfigs")]
        public List<GameTypeConfigDTO> GameTypeConfigs { get; set; }

        [InternalName("inGhostGame")]
        public bool InGhostGame { get; set; }

        [InternalName("ipBalance")]
        public double IpBalance { get; set; }

        [InternalName("languages")]
        public List<string> Languages { get; set; }

        [InternalName("leaverBusterPenaltyTime")]
        public int LeaverBusterPenaltyTime { get; set; }

        [InternalName("leaverPenaltyLevel")]
        public int LeaverPenaltyLevel { get; set; }

        [InternalName("matchMakingEnabled")]
        public bool MatchMakingEnabled { get; set; }

        [InternalName("maxPracticeGameSize")]
        public int MaxPracticeGameSize { get; set; }

        [InternalName("minor")]
        public bool Minor { get; set; }

        [InternalName("minorShutdownEnforced")]
        public bool MinorShutdownEnforced { get; set; }

        [InternalName("minutesUntilMidnight")]
        public int MinutesUntilMidnight { get; set; }

        [InternalName("minutesUntilShutdown")]
        public int MinutesUntilShutdown { get; set; }

        [InternalName("minutesUntilShutdownEnabled")]
        public bool MinutesUntilShutdownEnabled { get; set; }

        [InternalName("pendingBadges")]
        public int PendingBadges { get; set; }

        [InternalName("pendingKudosDTO")]
        public LoLLauncher.RiotObjects.Kudos.Dto.PendingKudosDTO PendingKudosDTO { get; set; }

        [InternalName("platformGameLifecycleDTO")]
        public object PlatformGameLifecycleDTO { get; set; }

        [InternalName("platformId")]
        public string PlatformId { get; set; }

        [InternalName("playerStatSummaries")]
        public LoLLauncher.RiotObjects.Platform.Statistics.PlayerStatSummaries PlayerStatSummaries { get; set; }

        [InternalName("reconnectInfo")]
        public LoLLauncher.RiotObjects.Platform.Game.PlatformGameLifecycleDTO ReconnectInfo { get; set; }

        [InternalName("restrictedChatGamesRemaining")]
        public int RestrictedChatGamesRemaining { get; set; }

        [InternalName("rpBalance")]
        public double RpBalance { get; set; }

        [InternalName("simpleMessages")]
        public List<object> SimpleMessages { get; set; }

        [InternalName("summonerCatalog")]
        public LoLLauncher.RiotObjects.Platform.Summoner.SummonerCatalog SummonerCatalog { get; set; }

        [InternalName("timeUntilFirstWinOfDay")]
        public double TimeUntilFirstWinOfDay { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(LoginDataPacket result);
    }
}

