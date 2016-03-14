namespace LoLLauncher.RiotObjects.Platform.Messaging
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    internal class StoreAccountBalanceNotification : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public StoreAccountBalanceNotification()
        {
            this.type = "com.riotgames.platform.reroll.pojo.StoreAccountBalanceNotification";
        }

        public StoreAccountBalanceNotification(Callback callback)
        {
            this.type = "com.riotgames.platform.reroll.pojo.StoreAccountBalanceNotification";
            this.callback = callback;
        }

        public StoreAccountBalanceNotification(TypedObject result)
        {
            this.type = "com.riotgames.platform.reroll.pojo.StoreAccountBalanceNotification";
            base.SetFields<StoreAccountBalanceNotification>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<StoreAccountBalanceNotification>(this, result);
            this.callback(this);
        }

        [InternalName("ip")]
        public double Ip { get; set; }

        [InternalName("rp")]
        public double Rp { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(StoreAccountBalanceNotification result);
    }
}

