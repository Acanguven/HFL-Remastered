namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SummonerDefaultSpells : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerDefaultSpells()
        {
            this.type = "com.riotgames.platform.summoner.SummonerDefaultSpells";
        }

        public SummonerDefaultSpells(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.SummonerDefaultSpells";
            this.callback = callback;
        }

        public SummonerDefaultSpells(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.SummonerDefaultSpells";
            base.SetFields<SummonerDefaultSpells>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerDefaultSpells>(this, result);
            this.callback(this);
        }

        [InternalName("summonerDefaultSpellMap")]
        public TypedObject SummonerDefaultSpellMap { get; set; }

        [InternalName("summonerDefaultSpellsJson")]
        public object SummonerDefaultSpellsJson { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerDefaultSpells result);
    }
}

