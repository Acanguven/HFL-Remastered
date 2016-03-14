namespace LoLLauncher.RiotObjects.Team
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class TeamId : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamId()
        {
            this.type = "com.riotgames.team.TeamId";
        }

        public TeamId(Callback callback)
        {
            this.type = "com.riotgames.team.TeamId";
            this.callback = callback;
        }

        public TeamId(TypedObject result)
        {
            this.type = "com.riotgames.team.TeamId";
            base.SetFields<TeamId>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamId>(this, result);
            this.callback(this);
        }

        [InternalName("fullId")]
        public string FullId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TeamId result);
    }
}

