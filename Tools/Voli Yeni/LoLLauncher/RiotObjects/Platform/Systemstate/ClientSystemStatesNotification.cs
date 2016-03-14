namespace LoLLauncher.RiotObjects.Platform.Systemstate
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ClientSystemStatesNotification : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public ClientSystemStatesNotification()
        {
            this.type = "com.riotgames.platform.systemstate.ClientSystemStatesNotification";
        }

        public ClientSystemStatesNotification(Callback callback)
        {
            this.type = "com.riotgames.platform.systemstate.ClientSystemStatesNotification";
            this.callback = callback;
        }

        public ClientSystemStatesNotification(TypedObject result)
        {
            this.type = "com.riotgames.platform.systemstate.ClientSystemStatesNotification";
            base.SetFields<ClientSystemStatesNotification>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<ClientSystemStatesNotification>(this, result);
            this.callback(this);
        }

        [InternalName("advancedTutorialEnabled")]
        public bool AdvancedTutorialEnabled { get; set; }

        [InternalName("archivedStatsEnabled")]
        public bool ArchivedStatsEnabled { get; set; }

        [InternalName("buddyNotesEnabled")]
        public bool BuddyNotesEnabled { get; set; }

        [InternalName("championTradeThroughLCDS")]
        public bool ChampionTradeThroughLCDS { get; set; }

        [InternalName("clientHeartBeatRateSeconds")]
        public int ClientHeartBeatRateSeconds { get; set; }

        [InternalName("displayPromoGamesPlayedEnabled")]
        public bool DisplayPromoGamesPlayedEnabled { get; set; }

        [InternalName("enabledQueueIdsList")]
        public int[] EnabledQueueIdsList { get; set; }

        [InternalName("freeToPlayChampionIdList")]
        public int[] FreeToPlayChampionIdList { get; set; }

        [InternalName("gameMapEnabledDTOList")]
        public List<Dictionary<string, object>> GameMapEnabledDTOList { get; set; }

        [InternalName("inactiveAramSpellIdList")]
        public int[] InactiveAramSpellIdList { get; set; }

        [InternalName("inactiveChampionIdList")]
        public object[] InactiveChampionIdList { get; set; }

        [InternalName("inactiveClassicSpellIdList")]
        public int[] InactiveClassicSpellIdList { get; set; }

        [InternalName("inactiveOdinSpellIdList")]
        public int[] InactiveOdinSpellIdList { get; set; }

        [InternalName("inactiveSpellIdList")]
        public int[] InactiveSpellIdList { get; set; }

        [InternalName("inactiveTutorialSpellIdList")]
        public int[] InactiveTutorialSpellIdList { get; set; }

        [InternalName("knownGeographicGameServerRegions")]
        public string[] KnownGeographicGameServerRegions { get; set; }

        [InternalName("kudosEnabled")]
        public bool KudosEnabled { get; set; }

        [InternalName("leaguesDecayMessagingEnabled")]
        public bool LeaguesDecayMessagingEnabled { get; set; }

        [InternalName("leagueServiceEnabled")]
        public bool LeagueServiceEnabled { get; set; }

        [InternalName("localeSpecificChatRoomsEnabled")]
        public bool LocaleSpecificChatRoomsEnabled { get; set; }

        [InternalName("masteryPageOnServer")]
        public bool MasteryPageOnServer { get; set; }

        [InternalName("maxMasteryPagesOnServer")]
        public int MaxMasteryPagesOnServer { get; set; }

        [InternalName("minNumPlayersForPracticeGame")]
        public int MinNumPlayersForPracticeGame { get; set; }

        [InternalName("modularGameModeEnabled")]
        public bool ModularGameModeEnabled { get; set; }

        [InternalName("observableCustomGameModes")]
        public string ObservableCustomGameModes { get; set; }

        [InternalName("observableGameModes")]
        public string[] ObservableGameModes { get; set; }

        [InternalName("observerModeEnabled")]
        public bool ObserverModeEnabled { get; set; }

        [InternalName("practiceGameEnabled")]
        public bool PracticeGameEnabled { get; set; }

        [InternalName("practiceGameTypeConfigIdList")]
        public int[] PracticeGameTypeConfigIdList { get; set; }

        [InternalName("queueThrottleDTO")]
        public Dictionary<string, object> QueueThrottleDTO { get; set; }

        [InternalName("replayServiceAddress")]
        public string ReplayServiceAddress { get; set; }

        [InternalName("replaySystemStates")]
        public Dictionary<string, object> ReplaySystemStates { get; set; }

        [InternalName("riotDataServiceDataSendProbability")]
        public int RiotDataServiceDataSendProbability { get; set; }

        [InternalName("runeUniquePerSpellBook")]
        public bool RuneUniquePerSpellBook { get; set; }

        [InternalName("sendFeedbackEventsEnabled")]
        public bool SendFeedbackEventsEnabled { get; set; }

        [InternalName("socialIntegrationEnabled")]
        public bool SocialIntegrationEnabled { get; set; }

        [InternalName("spectatorSlotLimit")]
        public int SpectatorSlotLimit { get; set; }

        [InternalName("storeCustomerEnabled")]
        public bool StoreCustomerEnabled { get; set; }

        [InternalName("teamServiceEnabled")]
        public bool TeamServiceEnabled { get; set; }

        [InternalName("tournamentSendStatsEnabled")]
        public bool TournamentSendStatsEnabled { get; set; }

        [InternalName("tribunalEnabled")]
        public bool TribunalEnabled { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("unobtainableChampionSkinIDList")]
        public int[] UnobtainableChampionSkinIDList { get; set; }

        public delegate void Callback(ClientSystemStatesNotification result);
    }
}

