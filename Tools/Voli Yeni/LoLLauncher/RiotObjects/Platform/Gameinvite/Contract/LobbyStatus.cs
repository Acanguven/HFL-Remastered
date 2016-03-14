namespace LoLLauncher.RiotObjects.Platform.Gameinvite.Contract
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LobbyStatus : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public LobbyStatus(Callback callback)
        {
            this.type = "com.riotgames.platform.gameinvite.contract.LobbyStatus";
            this.callback = callback;
        }

        public LobbyStatus(TypedObject result)
        {
            this.type = "com.riotgames.platform.gameinvite.contract.LobbyStatus";
            base.SetFields<LobbyStatus>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LobbyStatus>(this, result);
            this.callback(this);
        }

        [InternalName("gameMetaData")]
        public Dictionary<string, object> GameMetaData { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(LobbyStatus result);
    }
}

