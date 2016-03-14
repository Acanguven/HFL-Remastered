namespace LoLLauncher.RiotObjects.Platform.Summoner.Masterybook
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MasteryBookPageDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public MasteryBookPageDTO()
        {
            this.type = "com.riotgames.platform.summoner.masterybook.MasteryBookPageDTO";
        }

        public MasteryBookPageDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.masterybook.MasteryBookPageDTO";
            this.callback = callback;
        }

        public MasteryBookPageDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.masterybook.MasteryBookPageDTO";
            base.SetFields<MasteryBookPageDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<MasteryBookPageDTO>(this, result);
            this.callback(this);
        }

        [InternalName("createDate")]
        public object CreateDate { get; set; }

        [InternalName("current")]
        public bool Current { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("pageId")]
        public double PageId { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("talentEntries")]
        public List<TalentEntry> TalentEntries { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(MasteryBookPageDTO result);
    }
}

