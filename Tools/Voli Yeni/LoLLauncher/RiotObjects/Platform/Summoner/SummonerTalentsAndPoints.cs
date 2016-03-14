namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SummonerTalentsAndPoints : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerTalentsAndPoints()
        {
            this.type = "com.riotgames.platform.summoner.SummonerTalentsAndPoints";
        }

        public SummonerTalentsAndPoints(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.SummonerTalentsAndPoints";
            this.callback = callback;
        }

        public SummonerTalentsAndPoints(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.SummonerTalentsAndPoints";
            base.SetFields<SummonerTalentsAndPoints>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerTalentsAndPoints>(this, result);
            this.callback(this);
        }

        [InternalName("createDate")]
        public DateTime CreateDate { get; set; }

        [InternalName("modifyDate")]
        public DateTime ModifyDate { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("talentPoints")]
        public int TalentPoints { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerTalentsAndPoints result);
    }
}

