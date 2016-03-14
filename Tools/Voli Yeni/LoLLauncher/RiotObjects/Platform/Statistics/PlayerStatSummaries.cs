namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PlayerStatSummaries : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerStatSummaries()
        {
            this.type = "com.riotgames.platform.statistics.PlayerStatSummaries";
        }

        public PlayerStatSummaries(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.PlayerStatSummaries";
            this.callback = callback;
        }

        public PlayerStatSummaries(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.PlayerStatSummaries";
            base.SetFields<PlayerStatSummaries>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerStatSummaries>(this, result);
            this.callback(this);
        }

        [InternalName("playerStatSummarySet")]
        public List<PlayerStatSummary> PlayerStatSummarySet { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public double UserId { get; set; }

        public delegate void Callback(PlayerStatSummaries result);
    }
}

