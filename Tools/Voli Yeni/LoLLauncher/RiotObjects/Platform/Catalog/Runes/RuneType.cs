namespace LoLLauncher.RiotObjects.Platform.Catalog.Runes
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class RuneType : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public RuneType()
        {
            this.type = "com.riotgames.platform.catalog.runes.RuneType";
        }

        public RuneType(Callback callback)
        {
            this.type = "com.riotgames.platform.catalog.runes.RuneType";
            this.callback = callback;
        }

        public RuneType(TypedObject result)
        {
            this.type = "com.riotgames.platform.catalog.runes.RuneType";
            base.SetFields<RuneType>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<RuneType>(this, result);
            this.callback(this);
        }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("runeTypeId")]
        public int RuneTypeId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(RuneType result);
    }
}

