namespace LoLLauncher.RiotObjects.Team.Dto
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PlayerDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerDTO()
        {
            this.type = "com.riotgames.team.dto.PlayerDTO";
        }

        public PlayerDTO(Callback callback)
        {
            this.type = "com.riotgames.team.dto.PlayerDTO";
            this.callback = callback;
        }

        public PlayerDTO(TypedObject result)
        {
            this.type = "com.riotgames.team.dto.PlayerDTO";
            base.SetFields<PlayerDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerDTO>(this, result);
            this.callback(this);
        }

        [InternalName("createdTeams")]
        public List<object> CreatedTeams { get; set; }

        [InternalName("playerId")]
        public double PlayerId { get; set; }

        [InternalName("playerTeams")]
        public List<object> PlayerTeams { get; set; }

        [InternalName("teamsSummary")]
        public List<object> TeamsSummary { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PlayerDTO result);
    }
}

