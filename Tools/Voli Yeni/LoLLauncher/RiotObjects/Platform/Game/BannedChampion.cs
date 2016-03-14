namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class BannedChampion : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public BannedChampion(Callback callback)
        {
            this.type = "com.riotgames.platform.game.BannedChampion";
            this.callback = callback;
        }

        public BannedChampion(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.BannedChampion";
            base.SetFields<BannedChampion>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<BannedChampion>(this, result);
            this.callback(this);
        }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("pickTurn")]
        public int PickTurn { get; set; }

        [InternalName("teamId")]
        public int TeamId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(BannedChampion result);
    }
}

