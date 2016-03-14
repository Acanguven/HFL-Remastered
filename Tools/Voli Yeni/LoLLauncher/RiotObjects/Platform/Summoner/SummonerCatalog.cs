namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SummonerCatalog : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerCatalog()
        {
            this.type = "com.riotgames.platform.summoner.SummonerCatalog";
        }

        public SummonerCatalog(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.SummonerCatalog";
            this.callback = callback;
        }

        public SummonerCatalog(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.SummonerCatalog";
            base.SetFields<SummonerCatalog>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerCatalog>(this, result);
            this.callback(this);
        }

        [InternalName("items")]
        public object Items { get; set; }

        [InternalName("spellBookConfig")]
        public List<RuneSlot> SpellBookConfig { get; set; }

        [InternalName("talentTree")]
        public List<TalentGroup> TalentTree { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerCatalog result);
    }
}

