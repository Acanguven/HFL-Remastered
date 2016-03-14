namespace LoLLauncher.RiotObjects.Team.Dto
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class RosterDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public RosterDTO()
        {
            this.type = "com.riotgames.team.dto.RosterDTO";
        }

        public RosterDTO(Callback callback)
        {
            this.type = "com.riotgames.team.dto.RosterDTO";
            this.callback = callback;
        }

        public RosterDTO(TypedObject result)
        {
            this.type = "com.riotgames.team.dto.RosterDTO";
            base.SetFields<RosterDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<RosterDTO>(this, result);
            this.callback(this);
        }

        [InternalName("memberList")]
        public List<TeamMemberInfoDTO> MemberList { get; set; }

        [InternalName("ownerId")]
        public double OwnerId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(RosterDTO result);
    }
}

