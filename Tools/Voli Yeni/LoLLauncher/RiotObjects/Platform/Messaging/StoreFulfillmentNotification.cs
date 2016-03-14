namespace LoLLauncher.RiotObjects.Platform.Messaging
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
    using System;
    using System.Runtime.CompilerServices;

    internal class StoreFulfillmentNotification : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public StoreFulfillmentNotification()
        {
            this.type = "com.riotgames.platform.reroll.pojo.StoreFulfillmentNotification";
        }

        public StoreFulfillmentNotification(Callback callback)
        {
            this.type = "com.riotgames.platform.reroll.pojo.StoreFulfillmentNotification";
            this.callback = callback;
        }

        public StoreFulfillmentNotification(TypedObject result)
        {
            this.type = "com.riotgames.platform.reroll.pojo.StoreFulfillmentNotification";
            base.SetFields<StoreFulfillmentNotification>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<StoreFulfillmentNotification>(this, result);
            this.callback(this);
        }

        [InternalName("data")]
        public ChampionDTO Data { get; set; }

        [InternalName("inventoryType")]
        public string InventoryType { get; set; }

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

        public delegate void Callback(StoreFulfillmentNotification result);
    }
}

