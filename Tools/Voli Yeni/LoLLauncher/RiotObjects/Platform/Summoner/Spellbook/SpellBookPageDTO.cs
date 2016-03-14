namespace LoLLauncher.RiotObjects.Platform.Summoner.Spellbook
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SpellBookPageDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SpellBookPageDTO()
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SpellBookPageDTO";
        }

        public SpellBookPageDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SpellBookPageDTO";
            this.callback = callback;
        }

        public SpellBookPageDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SpellBookPageDTO";
            base.SetFields<SpellBookPageDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SpellBookPageDTO>(this, result);
            this.callback(this);
        }

        [InternalName("createDate")]
        public DateTime CreateDate { get; set; }

        [InternalName("current")]
        public bool Current { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("pageId")]
        public int PageId { get; set; }

        [InternalName("slotEntries")]
        public List<SlotEntry> SlotEntries { get; set; }

        [InternalName("summonerId")]
        public int SummonerId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SpellBookPageDTO result);
    }
}

