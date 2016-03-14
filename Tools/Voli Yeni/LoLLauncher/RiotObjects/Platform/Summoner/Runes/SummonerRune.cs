namespace LoLLauncher.RiotObjects.Platform.Summoner.Runes
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Catalog.Runes;
    using System;
    using System.Runtime.CompilerServices;

    public class SummonerRune : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerRune()
        {
            this.type = "com.riotgames.platform.summoner.runes.SummonerRune";
        }

        public SummonerRune(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.runes.SummonerRune";
            this.callback = callback;
        }

        public SummonerRune(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.runes.SummonerRune";
            base.SetFields<SummonerRune>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerRune>(this, result);
            this.callback(this);
        }

        [InternalName("purchased")]
        public DateTime Purchased { get; set; }

        [InternalName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [InternalName("quantity")]
        public int Quantity { get; set; }

        [InternalName("rune")]
        public LoLLauncher.RiotObjects.Platform.Catalog.Runes.Rune Rune { get; set; }

        [InternalName("runeId")]
        public int RuneId { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerRune result);
    }
}

