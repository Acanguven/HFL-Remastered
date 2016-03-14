namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class RecentGames : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public RecentGames()
        {
            this.type = "com.riotgames.platform.statistics.RecentGames";
        }

        public RecentGames(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.RecentGames";
            this.callback = callback;
        }

        public RecentGames(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.RecentGames";
            base.SetFields<RecentGames>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<RecentGames>(this, result);
            this.callback(this);
        }

        [InternalName("gameStatistics")]
        public List<PlayerGameStats> GameStatistics { get; set; }

        [InternalName("playerGameStatsMap")]
        public TypedObject PlayerGameStatsMap { get; set; }

        [InternalName("recentGamesJson")]
        public object RecentGamesJson { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public double UserId { get; set; }

        public delegate void Callback(RecentGames result);
    }
}

