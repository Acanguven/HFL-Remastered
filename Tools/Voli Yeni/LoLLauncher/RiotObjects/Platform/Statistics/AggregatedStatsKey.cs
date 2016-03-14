namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class AggregatedStatsKey : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public AggregatedStatsKey()
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStatsKey";
        }

        public AggregatedStatsKey(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStatsKey";
            this.callback = callback;
        }

        public AggregatedStatsKey(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStatsKey";
            base.SetFields<AggregatedStatsKey>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AggregatedStatsKey>(this, result);
            this.callback(this);
        }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("gameModeString")]
        public string GameModeString { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public double UserId { get; set; }

        public delegate void Callback(AggregatedStatsKey result);
    }
}

