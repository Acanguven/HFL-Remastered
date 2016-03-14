namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class Talent : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public Talent()
        {
            this.type = "com.riotgames.platform.summoner.Talent";
        }

        public Talent(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.Talent";
            this.callback = callback;
        }

        public Talent(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.Talent";
            base.SetFields<Talent>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<Talent>(this, result);
            this.callback(this);
        }

        [InternalName("gameCode")]
        public int GameCode { get; set; }

        [InternalName("index")]
        public int Index { get; set; }

        [InternalName("level1Desc")]
        public string Level1Desc { get; set; }

        [InternalName("level2Desc")]
        public string Level2Desc { get; set; }

        [InternalName("level3Desc")]
        public string Level3Desc { get; set; }

        [InternalName("level4Desc")]
        public string Level4Desc { get; set; }

        [InternalName("level5Desc")]
        public string Level5Desc { get; set; }

        [InternalName("maxRank")]
        public int MaxRank { get; set; }

        [InternalName("minLevel")]
        public int MinLevel { get; set; }

        [InternalName("minTier")]
        public int MinTier { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("prereqTalentGameCode")]
        public object PrereqTalentGameCode { get; set; }

        [InternalName("talentGroupId")]
        public int TalentGroupId { get; set; }

        [InternalName("talentRowId")]
        public int TalentRowId { get; set; }

        [InternalName("tltId")]
        public int TltId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(Talent result);
    }
}

