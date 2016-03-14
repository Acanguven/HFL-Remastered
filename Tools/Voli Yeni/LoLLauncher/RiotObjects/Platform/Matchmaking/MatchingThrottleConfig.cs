namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MatchingThrottleConfig : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public MatchingThrottleConfig()
        {
            this.type = "com.riotgames.platform.matchmaking.MatchingThrottleConfig";
        }

        public MatchingThrottleConfig(Callback callback)
        {
            this.type = "com.riotgames.platform.matchmaking.MatchingThrottleConfig";
            this.callback = callback;
        }

        public MatchingThrottleConfig(TypedObject result)
        {
            this.type = "com.riotgames.platform.matchmaking.MatchingThrottleConfig";
            base.SetFields<MatchingThrottleConfig>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<MatchingThrottleConfig>(this, result);
            this.callback(this);
        }

        [InternalName("cacheName")]
        public string CacheName { get; set; }

        [InternalName("limit")]
        public double Limit { get; set; }

        [InternalName("matchingThrottleProperties")]
        public List<object> MatchingThrottleProperties { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(MatchingThrottleConfig result);
    }
}

