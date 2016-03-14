namespace LoLLauncher.RiotObjects.Platform.Broadcast
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class BroadcastNotification : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public BroadcastNotification()
        {
            this.type = "com.riotgames.platform.broadcast.BroadcastNotification";
        }

        public BroadcastNotification(Callback callback)
        {
            this.type = "com.riotgames.platform.broadcast.BroadcastNotification";
            this.callback = callback;
        }

        public BroadcastNotification(TypedObject result)
        {
            this.type = "com.riotgames.platform.broadcast.BroadcastNotification";
            base.SetFields<BroadcastNotification>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<BroadcastNotification>(this, result);
            this.callback(this);
        }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(BroadcastNotification result);
    }
}

