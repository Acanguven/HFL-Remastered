namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class GameObserver : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public GameObserver()
        {
            this.type = "com.riotgames.platform.game.GameObserver";
        }

        public GameObserver(Callback callback)
        {
            this.type = "com.riotgames.platform.game.GameObserver";
            this.callback = callback;
        }

        public GameObserver(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.GameObserver";
            base.SetFields<GameObserver>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<GameObserver>(this, result);
            this.callback(this);
        }

        [InternalName("accountId")]
        public double AccountId { get; set; }

        [InternalName("badges")]
        public int Badges { get; set; }

        [InternalName("botDifficulty")]
        public string BotDifficulty { get; set; }

        [InternalName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }

        [InternalName("locale")]
        public object Locale { get; set; }

        [InternalName("originalAccountId")]
        public double OriginalAccountId { get; set; }

        [InternalName("originalPlatformId")]
        public string OriginalPlatformId { get; set; }

        [InternalName("partnerId")]
        public string PartnerId { get; set; }

        [InternalName("pickMode")]
        public int PickMode { get; set; }

        [InternalName("pickTurn")]
        public int PickTurn { get; set; }

        [InternalName("profileIconId")]
        public int ProfileIconId { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        [InternalName("summonerName")]
        public string SummonerName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(GameObserver result);
    }
}

