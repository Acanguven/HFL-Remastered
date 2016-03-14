namespace LoLLauncher.RiotObjects.Platform.Summoner.Masterybook
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MasteryBookDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public MasteryBookDTO()
        {
            this.type = "com.riotgames.platform.summoner.masterybook.MasteryBookDTO";
        }

        public MasteryBookDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.masterybook.MasteryBookDTO";
            this.callback = callback;
        }

        public MasteryBookDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.masterybook.MasteryBookDTO";
            base.SetFields<MasteryBookDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<MasteryBookDTO>(this, result);
            this.callback(this);
        }

        [InternalName("bookPages")]
        public List<MasteryBookPageDTO> BookPages { get; set; }

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

        public delegate void Callback(MasteryBookDTO result);
    }
}

