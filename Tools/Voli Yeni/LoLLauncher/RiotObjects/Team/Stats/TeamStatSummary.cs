namespace LoLLauncher.RiotObjects.Team.Stats
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Team;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TeamStatSummary : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamStatSummary()
        {
            this.type = "com.riotgames.team.stats.TeamStatSummary";
        }

        public TeamStatSummary(Callback callback)
        {
            this.type = "com.riotgames.team.stats.TeamStatSummary";
            this.callback = callback;
        }

        public TeamStatSummary(TypedObject result)
        {
            this.type = "com.riotgames.team.stats.TeamStatSummary";
            base.SetFields<TeamStatSummary>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamStatSummary>(this, result);
            this.callback(this);
        }

        [InternalName("teamId")]
        public LoLLauncher.RiotObjects.Team.TeamId TeamId { get; set; }

        [InternalName("teamIdString")]
        public string TeamIdString { get; set; }

        [InternalName("teamStatDetails")]
        public List<TeamStatDetail> TeamStatDetails { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TeamStatSummary result);
    }
}

