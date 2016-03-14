namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class TimeTrackedStat : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TimeTrackedStat()
        {
            this.type = "com.riotgames.platform.statistics.TimeTrackedStat";
        }

        public TimeTrackedStat(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.TimeTrackedStat";
            this.callback = callback;
        }

        public TimeTrackedStat(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.TimeTrackedStat";
            base.SetFields<TimeTrackedStat>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TimeTrackedStat>(this, result);
            this.callback(this);
        }

        [InternalName("timestamp")]
        public DateTime Timestamp { get; set; }

        [InternalName("type")]
        public string Type { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TimeTrackedStat result);
    }
}

