namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PublicSummoner : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PublicSummoner()
        {
            this.type = "com.riotgames.platform.summoner.PublicSummoner";
        }

        public PublicSummoner(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.PublicSummoner";
            this.callback = callback;
        }

        public PublicSummoner(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.PublicSummoner";
            base.SetFields<PublicSummoner>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PublicSummoner>(this, result);
            this.callback(this);
        }

        [InternalName("acctId")]
        public double AcctId { get; set; }

        [InternalName("internalName")]
        public string InternalName { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("profileIconId")]
        public int ProfileIconId { get; set; }

        [InternalName("revisionDate")]
        public DateTime RevisionDate { get; set; }

        [InternalName("revisionId")]
        public double RevisionId { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("summonerLevel")]
        public double SummonerLevel { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PublicSummoner result);
    }
}

