namespace LoLLauncher.RiotObjects.Platform.Catalog
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class ItemEffect : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public ItemEffect()
        {
            this.type = "com.riotgames.platform.catalog.ItemEffect";
        }

        public ItemEffect(Callback callback)
        {
            this.type = "com.riotgames.platform.catalog.ItemEffect";
            this.callback = callback;
        }

        public ItemEffect(TypedObject result)
        {
            this.type = "com.riotgames.platform.catalog.ItemEffect";
            base.SetFields<ItemEffect>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<ItemEffect>(this, result);
            this.callback(this);
        }

        [InternalName("effect")]
        public LoLLauncher.RiotObjects.Platform.Catalog.Effect Effect { get; set; }

        [InternalName("effectId")]
        public int EffectId { get; set; }

        [InternalName("itemEffectId")]
        public int ItemEffectId { get; set; }

        [InternalName("itemId")]
        public int ItemId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("value")]
        public string Value { get; set; }

        public delegate void Callback(ItemEffect result);
    }
}

