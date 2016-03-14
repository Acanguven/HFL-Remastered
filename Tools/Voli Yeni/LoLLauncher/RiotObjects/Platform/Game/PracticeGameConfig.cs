namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Game.Map;
    using System;
    using System.Runtime.CompilerServices;

    public class PracticeGameConfig : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PracticeGameConfig()
        {
            this.type = "com.riotgames.platform.game.PracticeGameConfig";
        }

        public PracticeGameConfig(Callback callback)
        {
            this.type = "com.riotgames.platform.game.PracticeGameConfig";
            this.callback = callback;
        }

        public PracticeGameConfig(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.PracticeGameConfig";
            base.SetFields<PracticeGameConfig>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PracticeGameConfig>(this, result);
            this.callback(this);
        }

        [InternalName("allowSpectators")]
        public string AllowSpectators { get; set; }

        [InternalName("gameMap")]
        public LoLLauncher.RiotObjects.Platform.Game.Map.GameMap GameMap { get; set; }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("gameName")]
        public string GameName { get; set; }

        [InternalName("gamePassword")]
        public string GamePassword { get; set; }

        [InternalName("gameTypeConfig")]
        public int GameTypeConfig { get; set; }

        [InternalName("maxNumPlayers")]
        public int MaxNumPlayers { get; set; }

        [InternalName("passbackDataPacket")]
        public object PassbackDataPacket { get; set; }

        [InternalName("passbackUrl")]
        public object PassbackUrl { get; set; }

        [InternalName("region")]
        public string Region { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PracticeGameConfig result);
    }
}

