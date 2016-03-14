namespace LoLLauncher.RiotObjects.Platform.Catalog.Champion
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class ChampionSkinDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public ChampionSkinDTO()
        {
            this.type = "com.riotgames.platform.catalog.champion.ChampionSkinDTO";
        }

        public ChampionSkinDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.catalog.champion.ChampionSkinDTO";
            this.callback = callback;
        }

        public ChampionSkinDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.catalog.champion.ChampionSkinDTO";
            base.SetFields<ChampionSkinDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<ChampionSkinDTO>(this, result);
            this.callback(this);
        }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("endDate")]
        public int EndDate { get; set; }

        [InternalName("freeToPlayReward")]
        public bool FreeToPlayReward { get; set; }

        [InternalName("lastSelected")]
        public bool LastSelected { get; set; }

        [InternalName("owned")]
        public bool Owned { get; set; }

        [InternalName("purchaseDate")]
        public int PurchaseDate { get; set; }

        [InternalName("skinId")]
        public int SkinId { get; set; }

        [InternalName("skinIndex")]
        public int SkinIndex { get; set; }

        [InternalName("stillObtainable")]
        public bool StillObtainable { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("winCountRemaining")]
        public int WinCountRemaining { get; set; }

        public delegate void Callback(ChampionSkinDTO result);
    }
}

