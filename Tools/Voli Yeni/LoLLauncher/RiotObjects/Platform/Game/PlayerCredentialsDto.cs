namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PlayerCredentialsDto : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerCredentialsDto()
        {
            this.type = "com.riotgames.platform.game.PlayerCredentialsDto";
        }

        public PlayerCredentialsDto(Callback callback)
        {
            this.type = "com.riotgames.platform.game.PlayerCredentialsDto";
            this.callback = callback;
        }

        public PlayerCredentialsDto(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.PlayerCredentialsDto";
            base.SetFields<PlayerCredentialsDto>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerCredentialsDto>(this, result);
            this.callback(this);
        }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("encryptionKey")]
        public string EncryptionKey { get; set; }

        [InternalName("gameId")]
        public double GameId { get; set; }

        [InternalName("handshakeToken")]
        public string HandshakeToken { get; set; }

        [InternalName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }

        [InternalName("observer")]
        public bool Observer { get; set; }

        [InternalName("observerEncryptionKey")]
        public string ObserverEncryptionKey { get; set; }

        [InternalName("observerServerIp")]
        public string ObserverServerIp { get; set; }

        [InternalName("observerServerPort")]
        public int ObserverServerPort { get; set; }

        [InternalName("playerId")]
        public double PlayerId { get; set; }

        [InternalName("serverIp")]
        public string ServerIp { get; set; }

        [InternalName("serverPort")]
        public int ServerPort { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("summonerName")]
        public string SummonerName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PlayerCredentialsDto result);
    }
}

