namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
    using System;
    using System.Runtime.CompilerServices;

    public class BotParticipant : Participant
    {
        private Callback callback;
        private string type;

        public BotParticipant()
        {
            this.type = "com.riotgames.platform.game.BotParticipant";
        }

        public BotParticipant(Callback callback)
        {
            this.type = "com.riotgames.platform.game.BotParticipant";
            this.callback = callback;
        }

        public BotParticipant(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.BotParticipant";
            base.SetFields<BotParticipant>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<BotParticipant>(this, result);
            this.callback(this);
        }

        [InternalName("badges")]
        public int Badges { get; set; }

        [InternalName("botSkillLevel")]
        public int BotSkillLevel { get; set; }

        [InternalName("botSkillLevelName")]
        public string BotSkillLevelName { get; set; }

        [InternalName("champion")]
        public ChampionDTO Champion { get; set; }

        [InternalName("isGameOwner")]
        public bool IsGameOwner { get; set; }

        [InternalName("isMe")]
        public bool IsMe { get; set; }

        [InternalName("pickMode")]
        public int PickMode { get; set; }

        [InternalName("pickTurn")]
        public int PickTurn { get; set; }

        [InternalName("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        [InternalName("summonerName")]
        public string SummonerName { get; set; }

        [InternalName("team")]
        public int Team { get; set; }

        [InternalName("teamId")]
        public string TeamId { get; set; }

        [InternalName("teamName")]
        public object TeamName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(BotParticipant result);
    }
}

