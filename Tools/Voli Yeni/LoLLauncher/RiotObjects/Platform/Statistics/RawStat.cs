namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class RawStat : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public RawStat()
        {
            this.type = "com.riotgames.platform.statistics.RawStat";
        }

        public RawStat(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.RawStat";
            this.callback = callback;
        }

        public RawStat(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.RawStat";
            base.SetFields<RawStat>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<RawStat>(this, result);
            this.callback(this);
        }

        [InternalName("statType")]
        public string StatType { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("value")]
        public double Value { get; set; }

        public delegate void Callback(RawStat result);
    }
}

