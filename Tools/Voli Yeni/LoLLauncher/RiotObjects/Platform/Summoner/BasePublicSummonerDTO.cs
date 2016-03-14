namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class BasePublicSummonerDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public BasePublicSummonerDTO()
        {
            this.type = "com.riotgames.platform.summoner.BasePublicSummonerDTO";
        }

        public BasePublicSummonerDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.BasePublicSummonerDTO";
            this.callback = callback;
        }

        public BasePublicSummonerDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.BasePublicSummonerDTO";
            base.SetFields<BasePublicSummonerDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<BasePublicSummonerDTO>(this, result);
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

        [InternalName("seasonOneTier")]
        public string SeasonOneTier { get; set; }

        [InternalName("seasonTwoTier")]
        public string SeasonTwoTier { get; set; }

        [InternalName("sumId")]
        public double SumId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(BasePublicSummonerDTO result);
    }
}

