namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class FellowPlayerInfo : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public FellowPlayerInfo()
        {
            this.type = "com.riotgames.platform.statistics.FellowPlayerInfo";
        }

        public FellowPlayerInfo(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.FellowPlayerInfo";
            this.callback = callback;
        }

        public FellowPlayerInfo(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.FellowPlayerInfo";
            base.SetFields<FellowPlayerInfo>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<FellowPlayerInfo>(this, result);
            this.callback(this);
        }

        [InternalName("championId")]
        public double ChampionId { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("teamId")]
        public int TeamId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(FellowPlayerInfo result);
    }
}

