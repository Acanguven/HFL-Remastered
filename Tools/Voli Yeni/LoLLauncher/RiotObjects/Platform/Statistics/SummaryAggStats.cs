namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SummaryAggStats : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummaryAggStats()
        {
            this.type = "com.riotgames.platform.statistics.SummaryAggStats";
        }

        public SummaryAggStats(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.SummaryAggStats";
            this.callback = callback;
        }

        public SummaryAggStats(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.SummaryAggStats";
            base.SetFields<SummaryAggStats>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummaryAggStats>(this, result);
            this.callback(this);
        }

        [InternalName("stats")]
        public List<SummaryAggStat> Stats { get; set; }

        [InternalName("statsJson")]
        public object StatsJson { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummaryAggStats result);
    }
}

