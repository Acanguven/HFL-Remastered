namespace LoLLauncher.RiotObjects.Platform.Summoner.Runes
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SummonerRuneInventory : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerRuneInventory()
        {
            this.type = "com.riotgames.platform.summoner.runes.SummonerRuneInventory";
        }

        public SummonerRuneInventory(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.runes.SummonerRuneInventory";
            this.callback = callback;
        }

        public SummonerRuneInventory(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.runes.SummonerRuneInventory";
            base.SetFields<SummonerRuneInventory>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerRuneInventory>(this, result);
            this.callback(this);
        }

        [InternalName("dateString")]
        public string DateString { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("summonerRunes")]
        public List<SummonerRune> SummonerRunes { get; set; }

        [InternalName("summonerRunesJson")]
        public object SummonerRunesJson { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerRuneInventory result);
    }
}

