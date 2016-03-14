namespace PVPNetConnect.RiotObjects
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SummonerNames : RiotGamesObject
    {
        private Callback callback;

        public SummonerNames(Callback callback)
        {
            this.callback = callback;
        }

        public override void DoCallback(TypedObject result)
        {
            this.callback(result.GetArray("array"));
        }

        public delegate void Callback(object[] result);
    }
}

