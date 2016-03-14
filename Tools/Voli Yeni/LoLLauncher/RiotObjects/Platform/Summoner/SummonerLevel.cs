namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SummonerLevel : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerLevel()
        {
            this.type = "com.riotgames.platform.summoner.SummonerLevel";
        }

        public SummonerLevel(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.SummonerLevel";
            this.callback = callback;
        }

        public SummonerLevel(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.SummonerLevel";
            base.SetFields<SummonerLevel>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerLevel>(this, result);
            this.callback(this);
        }

        [InternalName("expForLoss")]
        public double ExpForLoss { get; set; }

        [InternalName("expForWin")]
        public double ExpForWin { get; set; }

        [InternalName("expTierMod")]
        public double ExpTierMod { get; set; }

        [InternalName("expToNextLevel")]
        public double ExpToNextLevel { get; set; }

        [InternalName("grantRp")]
        public double GrantRp { get; set; }

        [InternalName("infTierMod")]
        public double InfTierMod { get; set; }

        [InternalName("summonerLevel")]
        public double Level { get; set; }

        [InternalName("summonerTier")]
        public double SummonerTier { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerLevel result);
    }
}

