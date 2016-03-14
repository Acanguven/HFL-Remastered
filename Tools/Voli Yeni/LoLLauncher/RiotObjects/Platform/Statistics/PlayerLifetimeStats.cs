namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PlayerLifetimeStats : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerLifetimeStats()
        {
            this.type = "com.riotgames.platform.statistics.PlayerLifetimeStats";
        }

        public PlayerLifetimeStats(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.PlayerLifetimeStats";
            this.callback = callback;
        }

        public PlayerLifetimeStats(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.PlayerLifetimeStats";
            base.SetFields<PlayerLifetimeStats>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerLifetimeStats>(this, result);
            this.callback(this);
        }

        [InternalName("dodgePenaltyDate")]
        public object DodgePenaltyDate { get; set; }

        [InternalName("dodgeStreak")]
        public int DodgeStreak { get; set; }

        [InternalName("leaverPenaltyStats")]
        public LoLLauncher.RiotObjects.Platform.Statistics.LeaverPenaltyStats LeaverPenaltyStats { get; set; }

        [InternalName("playerStats")]
        public LoLLauncher.RiotObjects.Platform.Statistics.PlayerStats PlayerStats { get; set; }

        [InternalName("playerStatsJson")]
        public object PlayerStatsJson { get; set; }

        [InternalName("playerStatSummaries")]
        public LoLLauncher.RiotObjects.Platform.Statistics.PlayerStatSummaries PlayerStatSummaries { get; set; }

        [InternalName("previousFirstWinOfDay")]
        public DateTime PreviousFirstWinOfDay { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public double UserId { get; set; }

        public delegate void Callback(PlayerLifetimeStats result);
    }
}

