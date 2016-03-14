namespace LoLLauncher.RiotObjects.Team.Dto
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Team;
    using LoLLauncher.RiotObjects.Team.Stats;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TeamDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TeamDTO()
        {
            this.type = "com.riotgames.team.dto.TeamDTO";
        }

        public TeamDTO(Callback callback)
        {
            this.type = "com.riotgames.team.dto.TeamDTO";
            this.callback = callback;
        }

        public TeamDTO(TypedObject result)
        {
            this.type = "com.riotgames.team.dto.TeamDTO";
            base.SetFields<TeamDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TeamDTO>(this, result);
            this.callback(this);
        }

        [InternalName("createDate")]
        public DateTime CreateDate { get; set; }

        [InternalName("lastGameDate")]
        public object LastGameDate { get; set; }

        [InternalName("lastJoinDate")]
        public DateTime LastJoinDate { get; set; }

        [InternalName("matchHistory")]
        public List<object> MatchHistory { get; set; }

        [InternalName("messageOfDay")]
        public object MessageOfDay { get; set; }

        [InternalName("modifyDate")]
        public DateTime ModifyDate { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("roster")]
        public RosterDTO Roster { get; set; }

        [InternalName("secondLastJoinDate")]
        public DateTime SecondLastJoinDate { get; set; }

        [InternalName("secondsUntilEligibleForDeletion")]
        public double SecondsUntilEligibleForDeletion { get; set; }

        [InternalName("status")]
        public string Status { get; set; }

        [InternalName("tag")]
        public string Tag { get; set; }

        [InternalName("teamId")]
        public LoLLauncher.RiotObjects.Team.TeamId TeamId { get; set; }

        [InternalName("teamStatSummary")]
        public LoLLauncher.RiotObjects.Team.Stats.TeamStatSummary TeamStatSummary { get; set; }

        [InternalName("thirdLastJoinDate")]
        public DateTime ThirdLastJoinDate { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TeamDTO result);
    }
}

