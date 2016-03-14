namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SummaryAggStat : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummaryAggStat(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.SummaryAggStat";
            this.callback = callback;
        }

        public SummaryAggStat(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.SummaryAggStat";
            base.SetFields<SummaryAggStat>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummaryAggStat>(this, result);
            this.callback(this);
        }

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

        public delegate void Callback(SummaryAggStat result);
    }
}

