namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GameQueueConfig : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public GameQueueConfig()
        {
            this.type = "com.riotgames.platform.matchmaking.GameQueueConfig";
        }

        public GameQueueConfig(Callback callback)
        {
            this.type = "com.riotgames.platform.matchmaking.GameQueueConfig";
            this.callback = callback;
        }

        public GameQueueConfig(TypedObject result)
        {
            this.type = "com.riotgames.platform.matchmaking.GameQueueConfig";
            base.SetFields<GameQueueConfig>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<GameQueueConfig>(this, result);
            this.callback(this);
        }

        [InternalName("blockedMinutesThreshold")]
        public int BlockedMinutesThreshold { get; set; }

        [InternalName("cacheName")]
        public string CacheName { get; set; }

        [InternalName("disallowFreeChampions")]
        public bool DisallowFreeChampions { get; set; }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("gameTypeConfigId")]
        public int GameTypeConfigId { get; set; }

        [InternalName("id")]
        public double Id { get; set; }

        [InternalName("mapSelectionAlgorithm")]
        public string MapSelectionAlgorithm { get; set; }

        [InternalName("matchingThrottleConfig")]
        public LoLLauncher.RiotObjects.Platform.Matchmaking.MatchingThrottleConfig MatchingThrottleConfig { get; set; }

        [InternalName("maximumParticipantListSize")]
        public int MaximumParticipantListSize { get; set; }

        [InternalName("maxLevel")]
        public int MaxLevel { get; set; }

        [InternalName("minimumParticipantListSize")]
        public int MinimumParticipantListSize { get; set; }

        [InternalName("minimumQueueDodgeDelayTime")]
        public int MinimumQueueDodgeDelayTime { get; set; }

        [InternalName("minLevel")]
        public int MinLevel { get; set; }

        [InternalName("numPlayersPerTeam")]
        public int NumPlayersPerTeam { get; set; }

        [InternalName("pointsConfigKey")]
        public string PointsConfigKey { get; set; }

        [InternalName("queueBonusKey")]
        public string QueueBonusKey { get; set; }

        [InternalName("queueState")]
        public string QueueState { get; set; }

        [InternalName("queueStateString")]
        public string QueueStateString { get; set; }

        [InternalName("ranked")]
        public bool Ranked { get; set; }

        [InternalName("supportedMapIds")]
        public List<int> SupportedMapIds { get; set; }

        [InternalName("teamOnly")]
        public bool TeamOnly { get; set; }

        [InternalName("thresholdEnabled")]
        public bool ThresholdEnabled { get; set; }

        [InternalName("thresholdSize")]
        public double ThresholdSize { get; set; }

        [InternalName("type")]
        public string Type { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("typeString")]
        public string TypeString { get; set; }

        public delegate void Callback(GameQueueConfig result);
    }
}

