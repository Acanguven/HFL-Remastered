namespace LoLLauncher.RiotObjects.Team.Dto
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class TeamMemberInfoDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamMemberInfoDTO()
        {
            this.type = "com.riotgames.team.dto.TeamMemberInfoDTO";
        }

        public TeamMemberInfoDTO(Callback callback)
        {
            this.type = "com.riotgames.team.dto.TeamMemberInfoDTO";
            this.callback = callback;
        }

        public TeamMemberInfoDTO(TypedObject result)
        {
            this.type = "com.riotgames.team.dto.TeamMemberInfoDTO";
            base.SetFields<TeamMemberInfoDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamMemberInfoDTO>(this, result);
            this.callback(this);
        }

        [InternalName("inviteDate")]
        public DateTime InviteDate { get; set; }

        [InternalName("joinDate")]
        public DateTime JoinDate { get; set; }

        [InternalName("playerId")]
        public double PlayerId { get; set; }

        [InternalName("playerName")]
        public string PlayerName { get; set; }

        [InternalName("status")]
        public string Status { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TeamMemberInfoDTO result);
    }
}

