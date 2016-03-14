namespace LoLLauncher.RiotObjects.Platform.Game.Practice
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Game;
    using System;
    using System.Runtime.CompilerServices;

    public class PracticeGameSearchResult : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PracticeGameSearchResult()
        {
            this.type = "com.riotgames.platform.game.practice.PracticeGameSearchResult";
        }

        public PracticeGameSearchResult(Callback callback)
        {
            this.type = "com.riotgames.platform.game.practice.PracticeGameSearchResult";
            this.callback = callback;
        }

        public PracticeGameSearchResult(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.practice.PracticeGameSearchResult";
            base.SetFields<PracticeGameSearchResult>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PracticeGameSearchResult>(this, result);
            this.callback(this);
        }

        [InternalName("allowSpectators")]
        public string AllowSpectators { get; set; }

        [InternalName("gameMapId")]
        public int GameMapId { get; set; }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("gameModeString")]
        public string GameModeString { get; set; }

        [InternalName("glmGameId")]
        public object GlmGameId { get; set; }

        [InternalName("glmHost")]
        public object GlmHost { get; set; }

        [InternalName("glmPort")]
        public int GlmPort { get; set; }

        [InternalName("glmSecurePort")]
        public int GlmSecurePort { get; set; }

        [InternalName("id")]
        public double Id { get; set; }

        [InternalName("maxNumPlayers")]
        public int MaxNumPlayers { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("owner")]
        public PlayerParticipant Owner { get; set; }

        [InternalName("privateGame")]
        public bool PrivateGame { get; set; }

        [InternalName("spectatorCount")]
        public int SpectatorCount { get; set; }

        [InternalName("team1Count")]
        public int Team1Count { get; set; }

        [InternalName("team2Count")]
        public int Team2Count { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PracticeGameSearchResult result);
    }
}

