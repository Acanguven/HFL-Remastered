namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AggregatedStats : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public AggregatedStats()
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStats";
        }

        public AggregatedStats(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStats";
            this.callback = callback;
        }

        public AggregatedStats(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.AggregatedStats";
            base.SetFields<AggregatedStats>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AggregatedStats>(this, result);
            this.callback(this);
        }

        [InternalName("aggregatedStatsJson")]
        public string AggregatedStatsJson { get; set; }

        [InternalName("key")]
        public AggregatedStatsKey Key { get; set; }

        [InternalName("lifetimeStatistics")]
        public List<AggregatedStat> LifetimeStatistics { get; set; }

        [InternalName("modifyDate")]
        public object ModifyDate { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(AggregatedStats result);
    }
}

