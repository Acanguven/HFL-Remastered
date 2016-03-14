namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PlayerStatSummary : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerStatSummary()
        {
            this.type = "com.riotgames.platform.statistics.PlayerStatSummary";
        }

        public PlayerStatSummary(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.PlayerStatSummary";
            this.callback = callback;
        }

        public PlayerStatSummary(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.PlayerStatSummary";
            base.SetFields<PlayerStatSummary>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerStatSummary>(this, result);
            this.callback(this);
        }

        [InternalName("aggregatedStats")]
        public SummaryAggStats AggregatedStats { get; set; }

        [InternalName("aggregatedStatsJson")]
        public object AggregatedStatsJson { get; set; }

        [InternalName("leaves")]
        public int Leaves { get; set; }

        [InternalName("losses")]
        public int Losses { get; set; }

        [InternalName("maxRating")]
        public int MaxRating { get; set; }

        [InternalName("modifyDate")]
        public DateTime ModifyDate { get; set; }

        [InternalName("playerStatSummaryType")]
        public string PlayerStatSummaryType { get; set; }

        [InternalName("playerStatSummaryTypeString")]
        public string PlayerStatSummaryTypeString { get; set; }

        [InternalName("rating")]
        public int Rating { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public double UserId { get; set; }

        [InternalName("wins")]
        public int Wins { get; set; }

        public delegate void Callback(PlayerStatSummary result);
    }
}

