namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class MatchMakerParams : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public MatchMakerParams()
        {
            this.type = "com.riotgames.platform.matchmaking.MatchMakerParams";
        }

        public MatchMakerParams(Callback callback)
        {
            this.type = "com.riotgames.platform.matchmaking.MatchMakerParams";
            this.callback = callback;
        }

        public MatchMakerParams(TypedObject result)
        {
            this.type = "com.riotgames.platform.matchmaking.MatchMakerParams";
            base.SetFields<MatchMakerParams>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<MatchMakerParams>(this, result);
            this.callback(this);
        }

        [InternalName("botDifficulty")]
        public string BotDifficulty { get; set; }

        [InternalName("invitationId")]
        public object InvitationId { get; set; }

        [InternalName("languages")]
        public object Languages { get; set; }

        [InternalName("lastMaestroMessage")]
        public object LastMaestroMessage { get; set; }

        [InternalName("queueIds")]
        public int[] QueueIds { get; set; }

        [InternalName("team")]
        public object Team { get; set; }

        [InternalName("teamId")]
        public object TeamId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(MatchMakerParams result);
    }
}

