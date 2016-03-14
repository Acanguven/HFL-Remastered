namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PlayerParticipant : Participant
    {
        private Callback callback;
        private string type;

        public PlayerParticipant()
        {
            this.type = "com.riotgames.platform.game.PlayerParticipant";
        }

        public PlayerParticipant(Callback callback)
        {
            this.type = "com.riotgames.platform.game.PlayerParticipant";
            this.callback = callback;
        }

        public PlayerParticipant(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.PlayerParticipant";
            base.SetFields<PlayerParticipant>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerParticipant>(this, result);
            this.callback(this);
        }

        public override string ToString()
        {
            return this.SummonerName;
        }

        [InternalName("accountId")]
        public double AccountId { get; set; }

        [InternalName("badges")]
        public int Badges { get; set; }

        [InternalName("botDifficulty")]
        public string BotDifficulty { get; set; }

        [InternalName("clientInSynch")]
        public bool ClientInSynch { get; set; }

        [InternalName("index")]
        public int Index { get; set; }

        [InternalName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }

        [InternalName("locale")]
        public object Locale { get; set; }

        [InternalName("minor")]
        public bool Minor { get; set; }

        [InternalName("originalAccountNumber")]
        public double OriginalAccountNumber { get; set; }

        [InternalName("originalPlatformId")]
        public string OriginalPlatformId { get; set; }

        [InternalName("partnerId")]
        public string PartnerId { get; set; }

        [InternalName("pickMode")]
        public int PickMode { get; set; }

        [InternalName("pickTurn")]
        public int PickTurn { get; set; }

        [InternalName("profileIconId")]
        public int ProfileIconId { get; set; }

        [InternalName("queueRating")]
        public int QueueRating { get; set; }

        [InternalName("summonerId")]
        public double SummonerId { get; set; }

        [InternalName("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        [InternalName("summonerName")]
        public string SummonerName { get; set; }

        [InternalName("teamOwner")]
        public bool TeamOwner { get; set; }

        [InternalName("teamParticipantId")]
        public object TeamParticipantId { get; set; }

        [InternalName("timeAddedToQueue")]
        public object TimeAddedToQueue { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PlayerParticipant result);
    }
}

