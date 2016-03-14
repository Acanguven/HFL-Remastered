namespace LoLLauncher.RiotObjects.Platform.Game.Message
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class GameNotification : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public GameNotification()
        {
            this.type = "com.riotgames.platform.game.message.GameNotification";
        }

        public GameNotification(Callback callback)
        {
            this.type = "com.riotgames.platform.game.message.GameNotification";
            this.callback = callback;
        }

        public GameNotification(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.message.GameNotification";
            base.SetFields<GameNotification>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<GameNotification>(this, result);
            this.callback(this);
        }

        [InternalName("messageArgument")]
        public object MessageArgument { get; set; }

        [InternalName("messageCode")]
        public string MessageCode { get; set; }

        [InternalName("type")]
        public string Type { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(GameNotification result);
    }
}

