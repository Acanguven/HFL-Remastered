namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Summoner.Masterybook;
    using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
    using System;
    using System.Runtime.CompilerServices;

    public class AllSummonerData : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public AllSummonerData()
        {
            this.type = "com.riotgames.platform.summoner.AllSummonerData";
        }

        public AllSummonerData(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.AllSummonerData";
            this.callback = callback;
        }

        public AllSummonerData(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.AllSummonerData";
            base.SetFields<AllSummonerData>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AllSummonerData>(this, result);
            this.callback(this);
        }

        [InternalName("masteryBook")]
        public MasteryBookDTO MasteryBook { get; set; }

        [InternalName("spellBook")]
        public SpellBookDTO SpellBook { get; set; }

        [InternalName("summoner")]
        public LoLLauncher.RiotObjects.Platform.Summoner.Summoner Summoner { get; set; }

        [InternalName("summonerDefaultSpells")]
        public LoLLauncher.RiotObjects.Platform.Summoner.SummonerDefaultSpells SummonerDefaultSpells { get; set; }

        [InternalName("summonerLevel")]
        public LoLLauncher.RiotObjects.Platform.Summoner.SummonerLevel SummonerLevel { get; set; }

        [InternalName("summonerLevelAndPoints")]
        public LoLLauncher.RiotObjects.Platform.Summoner.SummonerLevelAndPoints SummonerLevelAndPoints { get; set; }

        [InternalName("summonerTalentsAndPoints")]
        public LoLLauncher.RiotObjects.Platform.Summoner.SummonerTalentsAndPoints SummonerTalentsAndPoints { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(AllSummonerData result);
    }
}

