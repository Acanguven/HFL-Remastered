namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
    using System;
    using System.Runtime.CompilerServices;

    public class AllPublicSummonerDataDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public AllPublicSummonerDataDTO()
        {
            this.type = "com.riotgames.platform.summoner.AllPublicSummonerDataDTO";
        }

        public AllPublicSummonerDataDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.AllPublicSummonerDataDTO";
            this.callback = callback;
        }

        public AllPublicSummonerDataDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.AllPublicSummonerDataDTO";
            base.SetFields<AllPublicSummonerDataDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AllPublicSummonerDataDTO>(this, result);
            this.callback(this);
        }

        [InternalName("spellBook")]
        public SpellBookDTO SpellBook { get; set; }

        [InternalName("summoner")]
        public BasePublicSummonerDTO Summoner { get; set; }

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

        public delegate void Callback(AllPublicSummonerDataDTO result);
    }
}

