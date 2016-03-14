namespace LoLLauncher.RiotObjects.Team
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class CreatedTeam : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public CreatedTeam()
        {
            this.type = "com.riotgames.team.CreatedTeam";
        }

        public CreatedTeam(Callback callback)
        {
            this.type = "com.riotgames.team.CreatedTeam";
            this.callback = callback;
        }

        public CreatedTeam(TypedObject result)
        {
            this.type = "com.riotgames.team.CreatedTeam";
            base.SetFields<CreatedTeam>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<CreatedTeam>(this, result);
            this.callback(this);
        }

        [InternalName("teamId")]
        public LoLLauncher.RiotObjects.Team.TeamId TeamId { get; set; }

        [InternalName("timeStamp")]
        public double TimeStamp { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(CreatedTeam result);
    }
}

