namespace LoLLauncher.RiotObjects.Platform.Summoner.Spellbook
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SlotEntry : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SlotEntry()
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SlotEntry";
        }

        public SlotEntry(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SlotEntry";
            this.callback = callback;
        }

        public SlotEntry(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SlotEntry";
            base.SetFields<SlotEntry>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SlotEntry>(this, result);
            this.callback(this);
        }

        [InternalName("runeId")]
        public int RuneId { get; set; }

        [InternalName("runeSlotId")]
        public int RuneSlotId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SlotEntry result);
    }
}

