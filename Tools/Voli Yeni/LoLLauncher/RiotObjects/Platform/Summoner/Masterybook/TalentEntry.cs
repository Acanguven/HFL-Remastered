namespace LoLLauncher.RiotObjects.Platform.Summoner.Masterybook
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Summoner;
    using System;
    using System.Runtime.CompilerServices;

    public class TalentEntry : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TalentEntry()
        {
            this.type = "com.riotgames.platform.summoner.masterybook.TalentEntry";
        }

        public TalentEntry(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.masterybook.TalentEntry";
            this.callback = callback;
        }

        public TalentEntry(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.masterybook.TalentEntry";
            base.SetFields<TalentEntry>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TalentEntry>(this, result);
            this.callback(this);
        }

        [InternalName("rank")]
        public int Rank { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("talent")]
        public LoLLauncher.RiotObjects.Platform.Summoner.Talent Talent { get; set; }

        [InternalName("talentId")]
        public int TalentId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TalentEntry result);
    }
}

