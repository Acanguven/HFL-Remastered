namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class LeaverPenaltyStats : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public LeaverPenaltyStats()
        {
            this.type = "com.riotgames.platform.statistics.LeaverPenaltyStats";
        }

        public LeaverPenaltyStats(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.LeaverPenaltyStats";
            this.callback = callback;
        }

        public LeaverPenaltyStats(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.LeaverPenaltyStats";
            base.SetFields<LeaverPenaltyStats>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LeaverPenaltyStats>(this, result);
            this.callback(this);
        }

        [InternalName("lastDecay")]
        public DateTime LastDecay { get; set; }

        [InternalName("lastLevelIncrease")]
        public object LastLevelIncrease { get; set; }

        [InternalName("level")]
        public int Level { get; set; }

        [InternalName("points")]
        public int Points { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userInformed")]
        public bool UserInformed { get; set; }

        public delegate void Callback(LeaverPenaltyStats result);
    }
}

