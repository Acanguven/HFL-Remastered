namespace LoLLauncher.RiotObjects.Platform.Statistics.Team
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Statistics;
    using System;
    using System.Runtime.CompilerServices;

    public class TeamPlayerAggregatedStatsDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamPlayerAggregatedStatsDTO()
        {
            this.type = "com.riotgames.platform.statistics.team.TeamPlayerAggregatedStatsDTO";
        }

        public TeamPlayerAggregatedStatsDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.team.TeamPlayerAggregatedStatsDTO";
            this.callback = callback;
        }

        public TeamPlayerAggregatedStatsDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.team.TeamPlayerAggregatedStatsDTO";
            base.SetFields<TeamPlayerAggregatedStatsDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamPlayerAggregatedStatsDTO>(this, result);
            this.callback(this);
        }

        [InternalName("aggregatedStats")]
        public LoLLauncher.RiotObjects.Platform.Statistics.AggregatedStats AggregatedStats { get; set; }

        [InternalName("playerId")]
        public double PlayerId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TeamPlayerAggregatedStatsDTO result);
    }
}

