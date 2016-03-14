namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TalentRow : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TalentRow()
        {
            this.type = "com.riotgames.platform.summoner.TalentRow";
        }

        public TalentRow(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.TalentRow";
            this.callback = callback;
        }

        public TalentRow(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.TalentRow";
            base.SetFields<TalentRow>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TalentRow>(this, result);
            this.callback(this);
        }

        [InternalName("index")]
        public int Index { get; set; }

        [InternalName("pointsToActivate")]
        public int PointsToActivate { get; set; }

        [InternalName("talents")]
        public List<Talent> Talents { get; set; }

        [InternalName("tltGroupId")]
        public int TltGroupId { get; set; }

        [InternalName("tltRowId")]
        public int TltRowId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TalentRow result);
    }
}

