namespace LoLLauncher.RiotObjects.Platform.Summoner.Spellbook
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SpellBookDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SpellBookDTO()
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SpellBookDTO";
        }

        public SpellBookDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SpellBookDTO";
            this.callback = callback;
        }

        public SpellBookDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.spellbook.SpellBookDTO";
            base.SetFields<SpellBookDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SpellBookDTO>(this, result);
            this.callback(this);
        }

        [InternalName("bookPages")]
        public List<SpellBookPageDTO> BookPages { get; set; }

        [InternalName("bookPagesJson")]
        public object BookPagesJson { get; set; }

        [InternalName("dateString")]
        public string DateString { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SpellBookDTO result);
    }
}

