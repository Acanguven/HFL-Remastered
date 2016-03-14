namespace LoLLauncher.RiotObjects.Platform.Statistics.Team
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Team;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TeamAggregatedStatsDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamAggregatedStatsDTO()
        {
            this.type = "com.riotgames.platform.statistics.team.TeamAggregatedStatsDTO";
        }

        public TeamAggregatedStatsDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.team.TeamAggregatedStatsDTO";
            this.callback = callback;
        }

        public TeamAggregatedStatsDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.team.TeamAggregatedStatsDTO";
            base.SetFields<TeamAggregatedStatsDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamAggregatedStatsDTO>(this, result);
            this.callback(this);
        }

        [InternalName("playerAggregatedStatsList")]
        public List<TeamPlayerAggregatedStatsDTO> PlayerAggregatedStatsList { get; set; }

        [InternalName("queueType")]
        public string QueueType { get; set; }

        [InternalName("serializedToJson")]
        public string SerializedToJson { get; set; }

        [InternalName("teamId")]
        public LoLLauncher.RiotObjects.Team.TeamId TeamId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TeamAggregatedStatsDTO result);
    }
}

