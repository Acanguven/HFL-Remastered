namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class GameTypeConfigDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public GameTypeConfigDTO()
        {
            this.type = "com.riotgames.platform.game.GameTypeConfigDTO";
        }

        public GameTypeConfigDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.game.GameTypeConfigDTO";
            this.callback = callback;
        }

        public GameTypeConfigDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.GameTypeConfigDTO";
            base.SetFields<GameTypeConfigDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<GameTypeConfigDTO>(this, result);
            this.callback(this);
        }

        [InternalName("allowTrades")]
        public bool AllowTrades { get; set; }

        [InternalName("banTimerDuration")]
        public int BanTimerDuration { get; set; }

        [InternalName("exclusivePick")]
        public bool ExclusivePick { get; set; }

        [InternalName("id")]
        public int Id { get; set; }

        [InternalName("mainPickTimerDuration")]
        public int MainPickTimerDuration { get; set; }

        [InternalName("maxAllowableBans")]
        public int MaxAllowableBans { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("pickMode")]
        public string PickMode { get; set; }

        [InternalName("postPickTimerDuration")]
        public int PostPickTimerDuration { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(GameTypeConfigDTO result);
    }
}

