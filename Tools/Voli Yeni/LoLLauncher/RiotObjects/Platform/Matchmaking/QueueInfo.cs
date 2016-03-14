namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class QueueInfo : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public QueueInfo()
        {
            this.type = "com.riotgames.platform.matchmaking.QueueInfo";
        }

        public QueueInfo(Callback callback)
        {
            this.type = "com.riotgames.platform.matchmaking.QueueInfo";
            this.callback = callback;
        }

        public QueueInfo(TypedObject result)
        {
            this.type = "com.riotgames.platform.matchmaking.QueueInfo";
            base.SetFields<QueueInfo>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<QueueInfo>(this, result);
            this.callback(this);
        }

        [InternalName("queueId")]
        public double QueueId { get; set; }

        [InternalName("queueLength")]
        public int QueueLength { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("waitTime")]
        public double WaitTime { get; set; }

        public delegate void Callback(QueueInfo result);
    }
}

