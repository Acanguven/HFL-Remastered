namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PlatformGameLifecycleDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlatformGameLifecycleDTO()
        {
            this.type = "com.riotgames.platform.game.PlatformGameLifecycleDTO";
        }

        public PlatformGameLifecycleDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.game.PlatformGameLifecycleDTO";
            this.callback = callback;
        }

        public PlatformGameLifecycleDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.PlatformGameLifecycleDTO";
            base.SetFields<PlatformGameLifecycleDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlatformGameLifecycleDTO>(this, result);
            this.callback(this);
        }

        [InternalName("connectivityStateEnum")]
        public object ConnectivityStateEnum { get; set; }

        [InternalName("game")]
        public GameDTO Game { get; set; }

        [InternalName("gameName")]
        public string GameName { get; set; }

        [InternalName("gameSpecificLoyaltyRewards")]
        public object GameSpecificLoyaltyRewards { get; set; }

        [InternalName("lastModifiedDate")]
        public object LastModifiedDate { get; set; }

        [InternalName("playerCredentials")]
        public PlayerCredentialsDto PlayerCredentials { get; set; }

        [InternalName("reconnectDelay")]
        public int ReconnectDelay { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PlatformGameLifecycleDTO result);
    }
}

