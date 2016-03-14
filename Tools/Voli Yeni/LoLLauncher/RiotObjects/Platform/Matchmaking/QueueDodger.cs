namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Summoner;
    using System;
    using System.Runtime.CompilerServices;

    public class QueueDodger : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public QueueDodger()
        {
            this.type = "com.riotgames.platform.matchmaking.QueueDodger";
        }

        public QueueDodger(Callback callback)
        {
            this.type = "com.riotgames.platform.matchmaking.QueueDodger";
            this.callback = callback;
        }

        public QueueDodger(TypedObject result)
        {
            this.type = "com.riotgames.platform.matchmaking.QueueDodger";
            base.SetFields<QueueDodger>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<QueueDodger>(this, result);
            this.callback(this);
        }

        [InternalName("accessToken")]
        public string AccessToken { get; set; }

        [InternalName("dodgePenaltyRemainingTime")]
        public int DodgePenaltyRemainingTime { get; set; }

        [InternalName("leaverPenaltyMillisRemaining")]
        public int LeaverPenaltyMillisRemaining { get; set; }

        [InternalName("reasonFailed")]
        public string ReasonFailed { get; set; }

        [InternalName("summoner")]
        public LoLLauncher.RiotObjects.Platform.Summoner.Summoner Summoner { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(QueueDodger result);
    }
}

