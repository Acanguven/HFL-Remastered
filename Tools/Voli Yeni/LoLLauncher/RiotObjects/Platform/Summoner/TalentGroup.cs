namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TalentGroup : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TalentGroup()
        {
            this.type = "com.riotgames.platform.summoner.TalentGroup";
        }

        public TalentGroup(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.TalentGroup";
            this.callback = callback;
        }

        public TalentGroup(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.TalentGroup";
            base.SetFields<TalentGroup>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TalentGroup>(this, result);
            this.callback(this);
        }

        [InternalName("index")]
        public int Index { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("talentRows")]
        public List<TalentRow> TalentRows { get; set; }

        [InternalName("tltGroupId")]
        public int TltGroupId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TalentGroup result);
    }
}

