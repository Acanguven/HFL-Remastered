namespace LoLLauncher.RiotObjects.Team
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class TeamInfo : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamInfo()
        {
            this.type = "com.riotgames.team.TeamInfo";
        }

        public TeamInfo(Callback callback)
        {
            this.type = "com.riotgames.team.TeamInfo";
            this.callback = callback;
        }

        public TeamInfo(TypedObject result)
        {
            this.type = "com.riotgames.team.TeamInfo";
            base.SetFields<TeamInfo>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamInfo>(this, result);
            this.callback(this);
        }

        [InternalName("memberStatus")]
        public string MemberStatus { get; set; }

        [InternalName("memberStatusString")]
        public string MemberStatusString { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("secondsUntilEligibleForDeletion")]
        public double SecondsUntilEligibleForDeletion { get; set; }

        [InternalName("tag")]
        public string Tag { get; set; }

        [InternalName("teamId")]
        public LoLLauncher.RiotObjects.Team.TeamId TeamId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TeamInfo result);
    }
}

