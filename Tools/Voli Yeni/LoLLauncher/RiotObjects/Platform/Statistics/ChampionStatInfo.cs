namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ChampionStatInfo : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public ChampionStatInfo()
        {
            this.type = "com.riotgames.platform.statistics.ChampionStatInfo";
        }

        public ChampionStatInfo(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.ChampionStatInfo";
            this.callback = callback;
        }

        public ChampionStatInfo(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.ChampionStatInfo";
            base.SetFields<ChampionStatInfo>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<ChampionStatInfo>(this, result);
            this.callback(this);
        }

        [InternalName("accountId")]
        public double AccountId { get; set; }

        [InternalName("championId")]
        public double ChampionId { get; set; }

        [InternalName("stats")]
        public List<AggregatedStat> Stats { get; set; }

        [InternalName("totalGamesPlayed")]
        public int TotalGamesPlayed { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(ChampionStatInfo result);
    }
}

