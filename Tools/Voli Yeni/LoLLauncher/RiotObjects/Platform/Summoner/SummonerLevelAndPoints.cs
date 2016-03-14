namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SummonerLevelAndPoints : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerLevelAndPoints()
        {
            this.type = "com.riotgames.platform.summoner.SummonerLevelAndPoints";
        }

        public SummonerLevelAndPoints(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.SummonerLevelAndPoints";
            this.callback = callback;
        }

        public SummonerLevelAndPoints(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.SummonerLevelAndPoints";
            base.SetFields<SummonerLevelAndPoints>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerLevelAndPoints>(this, result);
            this.callback(this);
        }

        [InternalName("expPoints")]
        public double ExpPoints { get; set; }

        [InternalName("infPoints")]
        public double InfPoints { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("summonerLevel")]
        public double SummonerLevel { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerLevelAndPoints result);
    }
}

