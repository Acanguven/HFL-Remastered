namespace LoLLauncher.RiotObjects.Leagues.Pojo
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LeagueListDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public LeagueListDTO()
        {
            this.type = "com.riotgames.leagues.pojo.LeagueListDTO";
        }

        public LeagueListDTO(Callback callback)
        {
            this.type = "com.riotgames.leagues.pojo.LeagueListDTO";
            this.callback = callback;
        }

        public LeagueListDTO(TypedObject result)
        {
            this.type = "com.riotgames.leagues.pojo.LeagueListDTO";
            base.SetFields<LeagueListDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LeagueListDTO>(this, result);
            this.callback(this);
        }

        [InternalName("entries")]
        public List<LeagueItemDTO> Entries { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("queue")]
        public string Queue { get; set; }

        [InternalName("requestorsName")]
        public string RequestorsName { get; set; }

        [InternalName("requestorsRank")]
        public string RequestorsRank { get; set; }

        [InternalName("tier")]
        public string Tier { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(LeagueListDTO result);
    }
}

