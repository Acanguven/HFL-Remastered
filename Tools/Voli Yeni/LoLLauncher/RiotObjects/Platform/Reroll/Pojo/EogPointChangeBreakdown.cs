namespace LoLLauncher.RiotObjects.Platform.Reroll.Pojo
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    internal class EogPointChangeBreakdown : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public EogPointChangeBreakdown()
        {
            this.type = "com.riotgames.platform.reroll.pojo.EogPointChangeBreakdown";
        }

        public EogPointChangeBreakdown(Callback callback)
        {
            this.type = "com.riotgames.platform.reroll.pojo.EogPointChangeBreakdown";
            this.callback = callback;
        }

        public EogPointChangeBreakdown(TypedObject result)
        {
            this.type = "com.riotgames.platform.reroll.pojo.EogPointChangeBreakdown";
            base.SetFields<EogPointChangeBreakdown>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<EogPointChangeBreakdown>(this, result);
            this.callback(this);
        }

        [InternalName("endPoints")]
        public double EndPoints { get; set; }

        [InternalName("pointChangeFromChampionsOwned")]
        public double PointChangeFromChampionsOwned { get; set; }

        [InternalName("pointChangeFromGamePlay")]
        public double PointChangeFromGamePlay { get; set; }

        [InternalName("pointsUsed")]
        public double PointsUsed { get; set; }

        [InternalName("previousPoints")]
        public double PreviousPoints { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(EogPointChangeBreakdown result);
    }
}

