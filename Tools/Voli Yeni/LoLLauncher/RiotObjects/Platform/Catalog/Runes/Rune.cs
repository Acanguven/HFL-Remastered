namespace LoLLauncher.RiotObjects.Platform.Catalog.Runes
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Rune : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public Rune()
        {
            this.type = "com.riotgames.platform.catalog.runes.Rune";
        }

        public Rune(Callback callback)
        {
            this.type = "com.riotgames.platform.catalog.runes.Rune";
            this.callback = callback;
        }

        public Rune(TypedObject result)
        {
            this.type = "com.riotgames.platform.catalog.runes.Rune";
            base.SetFields<Rune>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<Rune>(this, result);
            this.callback(this);
        }

        [InternalName("baseType")]
        public string BaseType { get; set; }

        [InternalName("description")]
        public string Description { get; set; }

        [InternalName("duration")]
        public int Duration { get; set; }

        [InternalName("gameCode")]
        public int GameCode { get; set; }

        [InternalName("imagePath")]
        public object ImagePath { get; set; }

        [InternalName("itemEffects")]
        public List<ItemEffect> ItemEffects { get; set; }

        [InternalName("itemId")]
        public int ItemId { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("runeType")]
        public LoLLauncher.RiotObjects.Platform.Catalog.Runes.RuneType RuneType { get; set; }

        [InternalName("tier")]
        public int Tier { get; set; }

        [InternalName("toolTip")]
        public object ToolTip { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("uses")]
        public object Uses { get; set; }

        public delegate void Callback(Rune result);
    }
}

