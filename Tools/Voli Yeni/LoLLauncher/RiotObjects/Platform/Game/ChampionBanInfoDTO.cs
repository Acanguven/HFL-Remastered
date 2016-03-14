namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class ChampionBanInfoDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public ChampionBanInfoDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.game.ChampionBanInfoDTO";
            this.callback = callback;
        }

        public ChampionBanInfoDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.ChampionBanInfoDTO";
            base.SetFields<ChampionBanInfoDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<ChampionBanInfoDTO>(this, result);
            this.callback(this);
        }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("enemyOwned")]
        public bool EnemyOwned { get; set; }

        [InternalName("owned")]
        public bool Owned { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(ChampionBanInfoDTO result);
    }
}

