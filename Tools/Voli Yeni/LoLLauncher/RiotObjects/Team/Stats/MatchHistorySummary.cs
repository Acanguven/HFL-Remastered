namespace LoLLauncher.RiotObjects.Team.Stats
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class MatchHistorySummary : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public MatchHistorySummary()
        {
            this.type = "com.riotgames.team.stats.MatchHistorySummary";
        }

        public MatchHistorySummary(Callback callback)
        {
            this.type = "com.riotgames.team.stats.MatchHistorySummary";
            this.callback = callback;
        }

        public MatchHistorySummary(TypedObject result)
        {
            this.type = "com.riotgames.team.stats.MatchHistorySummary";
            base.SetFields<MatchHistorySummary>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<MatchHistorySummary>(this, result);
            this.callback(this);
        }

        [InternalName("assists")]
        public int Assists { get; set; }

        [InternalName("date")]
        public double Date { get; set; }

        [InternalName("deaths")]
        public int Deaths { get; set; }

        [InternalName("gameId")]
        public double GameId { get; set; }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("invalid")]
        public bool Invalid { get; set; }

        [InternalName("kills")]
        public int Kills { get; set; }

        [InternalName("mapId")]
        public int MapId { get; set; }

        [InternalName("opposingTeamKills")]
        public int OpposingTeamKills { get; set; }

        [InternalName("opposingTeamName")]
        public string OpposingTeamName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("win")]
        public bool Win { get; set; }

        public delegate void Callback(MatchHistorySummary result);
    }
}

