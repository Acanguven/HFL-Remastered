namespace LoLLauncher.RiotObjects.Platform.Summoner.Boost
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SummonerActiveBoostsDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerActiveBoostsDTO()
        {
            this.type = "com.riotgames.platform.summoner.boost.SummonerActiveBoostsDTO";
        }

        public SummonerActiveBoostsDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.boost.SummonerActiveBoostsDTO";
            this.callback = callback;
        }

        public SummonerActiveBoostsDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.boost.SummonerActiveBoostsDTO";
            base.SetFields<SummonerActiveBoostsDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerActiveBoostsDTO>(this, result);
            this.callback(this);
        }

        [InternalName("ipBoostEndDate")]
        public double IpBoostEndDate { get; set; }

        [InternalName("ipBoostPerWinCount")]
        public int IpBoostPerWinCount { get; set; }

        [InternalName("ipLoyaltyBoost")]
        public int IpLoyaltyBoost { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("xpBoostEndDate")]
        public double XpBoostEndDate { get; set; }

        [InternalName("xpBoostPerWinCount")]
        public int XpBoostPerWinCount { get; set; }

        [InternalName("xpLoyaltyBoost")]
        public int XpLoyaltyBoost { get; set; }

        public delegate void Callback(SummonerActiveBoostsDTO result);
    }
}

