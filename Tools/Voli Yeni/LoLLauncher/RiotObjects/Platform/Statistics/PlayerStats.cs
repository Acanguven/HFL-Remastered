namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PlayerStats : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerStats()
        {
            this.type = "com.riotgames.platform.statistics.PlayerStats";
        }

        public PlayerStats(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.PlayerStats";
            this.callback = callback;
        }

        public PlayerStats(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.PlayerStats";
            base.SetFields<PlayerStats>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerStats>(this, result);
            this.callback(this);
        }

        [InternalName("promoGamesPlayed")]
        public int PromoGamesPlayed { get; set; }

        [InternalName("promoGamesPlayedLastUpdated")]
        public object PromoGamesPlayedLastUpdated { get; set; }

        [InternalName("timeTrackedStats")]
        public List<TimeTrackedStat> TimeTrackedStats { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PlayerStats result);
    }
}

