namespace LoLLauncher.RiotObjects.Platform.Reroll.Pojo
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Game;
    using System;
    using System.Runtime.CompilerServices;

    public class AramPlayerParticipant : Participant
    {
        private Callback callback;
        private string type;

        public AramPlayerParticipant()
        {
            this.type = "com.riotgames.platform.reroll.pojo.AramPlayerParticipant";
        }

        public AramPlayerParticipant(Callback callback)
        {
            this.type = "com.riotgames.platform.reroll.pojo.AramPlayerParticipant";
            this.callback = callback;
        }

        public AramPlayerParticipant(TypedObject result)
        {
            this.type = "com.riotgames.platform.reroll.pojo.AramPlayerParticipant";
            base.SetFields<AramPlayerParticipant>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AramPlayerParticipant>(this, result);
            this.callback(this);
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

        [InternalName("pointSummary")]
        public LoLLauncher.RiotObjects.Platform.Reroll.Pojo.PointSummary PointSummary { get; set; }

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
        public double TeamParticipantId { get; set; }

        [InternalName("timeAddedToQueue")]
        public double TimeAddedToQueue { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(AramPlayerParticipant result);
    }
}

