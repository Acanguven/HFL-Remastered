namespace LoLLauncher.RiotObjects.Platform.Reroll.Pojo
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PointSummary : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PointSummary()
        {
            this.type = "com.riotgames.platform.reroll.pojo.PointSummary";
        }

        public PointSummary(Callback callback)
        {
            this.type = "com.riotgames.platform.reroll.pojo.PointSummary";
            this.callback = callback;
        }

        public PointSummary(TypedObject result)
        {
            this.type = "com.riotgames.platform.reroll.pojo.PointSummary";
            base.SetFields<PointSummary>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PointSummary>(this, result);
            this.callback(this);
        }

        [InternalName("currentPoints")]
        public double CurrentPoints { get; set; }

        [InternalName("maxRolls")]
        public int MaxRolls { get; set; }

        [InternalName("numberOfRolls")]
        public int NumberOfRolls { get; set; }

        [InternalName("pointsCostToRoll")]
        public double PointsCostToRoll { get; set; }

        [InternalName("pointsToNextRoll")]
        public double PointsToNextRoll { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PointSummary result);
    }
}

