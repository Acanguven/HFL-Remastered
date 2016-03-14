namespace LoLLauncher.RiotObjects.Platform.Catalog.Champion
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ChampionDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public ChampionDTO()
        {
            this.type = "com.riotgames.platform.catalog.champion.ChampionDTO";
        }

        public ChampionDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.catalog.champion.ChampionDTO";
            this.callback = callback;
        }

        public ChampionDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.catalog.champion.ChampionDTO";
            base.SetFields<ChampionDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<ChampionDTO>(this, result);
            this.callback(this);
        }

        [InternalName("active")]
        public bool Active { get; set; }

        [InternalName("banned")]
        public bool Banned { get; set; }

        [InternalName("botEnabled")]
        public bool BotEnabled { get; set; }

        [InternalName("championData")]
        public TypedObject ChampionData { get; set; }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("championSkins")]
        public List<ChampionSkinDTO> ChampionSkins { get; set; }

        [InternalName("description")]
        public string Description { get; set; }

        [InternalName("displayName")]
        public string DisplayName { get; set; }

        [InternalName("endDate")]
        public int EndDate { get; set; }

        [InternalName("freeToPlay")]
        public bool FreeToPlay { get; set; }

        [InternalName("freeToPlayReward")]
        public bool FreeToPlayReward { get; set; }

        [InternalName("owned")]
        public bool Owned { get; set; }

        [InternalName("ownedByEnemyTeam")]
        public bool OwnedByEnemyTeam { get; set; }

        [InternalName("ownedByYourTeam")]
        public bool OwnedByYourTeam { get; set; }

        [InternalName("purchaseDate")]
        public double PurchaseDate { get; set; }

        [InternalName("searchTags")]
        public string[] SearchTags { get; set; }

        [InternalName("skinName")]
        public string SkinName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("winCountRemaining")]
        public int WinCountRemaining { get; set; }

        public delegate void Callback(ChampionDTO result);
    }
}

