namespace LoLLauncher.RiotObjects.Leagues.Pojo
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class LeagueItemDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public LeagueItemDTO()
        {
            this.type = "com.riotgames.leagues.pojo.LeagueItemDTO";
        }

        public LeagueItemDTO(Callback callback)
        {
            this.type = "com.riotgames.leagues.pojo.LeagueItemDTO";
            this.callback = callback;
        }

        public LeagueItemDTO(TypedObject result)
        {
            this.type = "com.riotgames.leagues.pojo.LeagueItemDTO";
            base.SetFields<LeagueItemDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LeagueItemDTO>(this, result);
            this.callback(this);
        }

        [InternalName("freshBlood")]
        public bool FreshBlood { get; set; }

        [InternalName("hotStreak")]
        public bool HotStreak { get; set; }

        [InternalName("inactive")]
        public bool Inactive { get; set; }

        [InternalName("lastPlayed")]
        public double LastPlayed { get; set; }

        [InternalName("leagueName")]
        public string LeagueName { get; set; }

        [InternalName("leaguePoints")]
        public int LeaguePoints { get; set; }

        [InternalName("losses")]
        public int Losses { get; set; }

        [InternalName("miniSeries")]
        public object MiniSeries { get; set; }

        [InternalName("playerOrTeamId")]
        public string PlayerOrTeamId { get; set; }

        [InternalName("playerOrTeamName")]
        public string PlayerOrTeamName { get; set; }

        [InternalName("previousDayLeaguePosition")]
        public int PreviousDayLeaguePosition { get; set; }

        [InternalName("queueType")]
        public string QueueType { get; set; }

        [InternalName("rank")]
        public string Rank { get; set; }

        [InternalName("tier")]
        public string Tier { get; set; }

        [InternalName("timeLastDecayMessageShown")]
        public double TimeLastDecayMessageShown { get; set; }

        [InternalName("timeUntilDecay")]
        public double TimeUntilDecay { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("veteran")]
        public bool Veteran { get; set; }

        [InternalName("wins")]
        public int Wins { get; set; }

        public delegate void Callback(LeagueItemDTO result);
    }
}

