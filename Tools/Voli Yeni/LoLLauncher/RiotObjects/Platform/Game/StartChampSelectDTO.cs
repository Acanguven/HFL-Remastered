namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class StartChampSelectDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public StartChampSelectDTO()
        {
            this.type = "com.riotgames.platform.game.StartChampSelectDTO";
        }

        public StartChampSelectDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.game.StartChampSelectDTO";
            this.callback = callback;
        }

        public StartChampSelectDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.StartChampSelectDTO";
            base.SetFields<StartChampSelectDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<StartChampSelectDTO>(this, result);
            this.callback(this);
        }

        [InternalName("invalidPlayers")]
        public List<object> InvalidPlayers { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(StartChampSelectDTO result);
    }
}

