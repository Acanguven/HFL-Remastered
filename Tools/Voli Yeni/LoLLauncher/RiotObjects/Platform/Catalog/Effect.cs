namespace LoLLauncher.RiotObjects.Platform.Catalog
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Catalog.Runes;
    using System;
    using System.Runtime.CompilerServices;

    public class Effect : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public Effect()
        {
            this.type = "com.riotgames.platform.catalog.Effect";
        }

        public Effect(Callback callback)
        {
            this.type = "com.riotgames.platform.catalog.Effect";
            this.callback = callback;
        }

        public Effect(TypedObject result)
        {
            this.type = "com.riotgames.platform.catalog.Effect";
            base.SetFields<Effect>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<Effect>(this, result);
            this.callback(this);
        }

        [InternalName("categoryId")]
        public object CategoryId { get; set; }

        [InternalName("effectId")]
        public int EffectId { get; set; }

        [InternalName("gameCode")]
        public string GameCode { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("runeType")]
        public LoLLauncher.RiotObjects.Platform.Catalog.Runes.RuneType RuneType { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(Effect result);
    }
}

