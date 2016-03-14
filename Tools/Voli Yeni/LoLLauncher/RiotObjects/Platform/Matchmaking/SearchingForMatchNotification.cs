namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SearchingForMatchNotification : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SearchingForMatchNotification()
        {
            this.type = "com.riotgames.platform.matchmaking.SearchingForMatchNotification";
        }

        public SearchingForMatchNotification(Callback callback)
        {
            this.type = "com.riotgames.platform.matchmaking.SearchingForMatchNotification";
            this.callback = callback;
        }

        public SearchingForMatchNotification(TypedObject result)
        {
            this.type = "com.riotgames.platform.matchmaking.SearchingForMatchNotification";
            base.SetFields<SearchingForMatchNotification>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SearchingForMatchNotification>(this, result);
            this.callback(this);
        }

        [InternalName("ghostGameSummoners")]
        public object GhostGameSummoners { get; set; }

        [InternalName("joinedQueues")]
        public List<QueueInfo> JoinedQueues { get; set; }

        [InternalName("playerJoinFailures")]
        public List<QueueDodger> PlayerJoinFailures { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SearchingForMatchNotification result);
    }
}

