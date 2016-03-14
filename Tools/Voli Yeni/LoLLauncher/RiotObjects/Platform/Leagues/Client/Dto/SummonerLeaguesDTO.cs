namespace LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SummonerLeaguesDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerLeaguesDTO()
        {
            this.type = "com.riotgames.platform.leagues.client.dto.SummonerLeaguesDTO";
        }

        public SummonerLeaguesDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.leagues.client.dto.SummonerLeaguesDTO";
            this.callback = callback;
        }

        public SummonerLeaguesDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.leagues.client.dto.SummonerLeaguesDTO";
            base.SetFields<SummonerLeaguesDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerLeaguesDTO>(this, result);
            this.callback(this);
        }

        [InternalName("summonerLeagues")]
        public List<LeagueListDTO> SummonerLeagues { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerLeaguesDTO result);
    }
}

