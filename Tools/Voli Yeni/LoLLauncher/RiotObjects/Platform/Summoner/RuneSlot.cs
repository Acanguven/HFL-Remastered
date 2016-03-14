namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Catalog.Runes;
    using System;
    using System.Runtime.CompilerServices;

    public class RuneSlot : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public RuneSlot()
        {
            this.type = "com.riotgames.platform.summoner.RuneSlot";
        }

        public RuneSlot(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.RuneSlot";
            this.callback = callback;
        }

        public RuneSlot(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.RuneSlot";
            base.SetFields<RuneSlot>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<RuneSlot>(this, result);
            this.callback(this);
        }

        [InternalName("id")]
        public int Id { get; set; }

        [InternalName("minLevel")]
        public int MinLevel { get; set; }

        [InternalName("runeType")]
        public LoLLauncher.RiotObjects.Platform.Catalog.Runes.RuneType RuneType { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(RuneSlot result);
    }
}

