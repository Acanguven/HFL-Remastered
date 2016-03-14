namespace LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SummonerLeagueItemsDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SummonerLeagueItemsDTO()
        {
            this.type = "com.riotgames.platform.leagues.client.dto.SummonerLeagueItemsDTO";
        }

        public SummonerLeagueItemsDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.leagues.client.dto.SummonerLeagueItemsDTO";
            this.callback = callback;
        }

        public SummonerLeagueItemsDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.leagues.client.dto.SummonerLeagueItemsDTO";
            base.SetFields<SummonerLeagueItemsDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SummonerLeagueItemsDTO>(this, result);
            this.callback(this);
        }

        [InternalName("summonerLeagues")]
        public List<LeagueItemDTO> SummonerLeagues { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SummonerLeagueItemsDTO result);
    }
}

