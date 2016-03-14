namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class AggregatedStat : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public AggregatedStat()
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStat";
        }

        public AggregatedStat(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStat";
            this.callback = callback;
        }

        public AggregatedStat(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStat";
            base.SetFields<AggregatedStat>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AggregatedStat>(this, result);
            this.callback(this);
        }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("count")]
        public double Count { get; set; }

        [InternalName("statType")]
        public string StatType { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("value")]
        public double Value { get; set; }

        public delegate void Callback(AggregatedStat result);
    }
}

