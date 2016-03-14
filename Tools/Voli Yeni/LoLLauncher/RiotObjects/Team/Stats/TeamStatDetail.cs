namespace LoLLauncher.RiotObjects.Team.Stats
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Team;
    using System;
    using System.Runtime.CompilerServices;

    public class TeamStatDetail : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamStatDetail()
        {
            this.type = "com.riotgames.team.stats.TeamStatDetail";
        }

        public TeamStatDetail(Callback callback)
        {
            this.type = "com.riotgames.team.stats.TeamStatDetail";
            this.callback = callback;
        }

        public TeamStatDetail(TypedObject result)
        {
            this.type = "com.riotgames.team.stats.TeamStatDetail";
            base.SetFields<TeamStatDetail>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamStatDetail>(this, result);
            this.callback(this);
        }

        [InternalName("averageGamesPlayed")]
        public int AverageGamesPlayed { get; set; }

        [InternalName("losses")]
        public int Losses { get; set; }

        [InternalName("maxRating")]
        public int MaxRating { get; set; }

        [InternalName("rating")]
        public int Rating { get; set; }

        [InternalName("seedRating")]
        public int SeedRating { get; set; }

        [InternalName("teamId")]
        public LoLLauncher.RiotObjects.Team.TeamId TeamId { get; set; }

        [InternalName("teamIdString")]
        public string TeamIdString { get; set; }

        [InternalName("teamStatType")]
        public string TeamStatType { get; set; }

        [InternalName("teamStatTypeString")]
        public string TeamStatTypeString { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("wins")]
        public int Wins { get; set; }

        public delegate void Callback(TeamStatDetail result);
    }
}

