namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class Participant : RiotGamesObject
    {
        private Callback callback;

        public Participant()
        {
        }

        public Participant(Callback callback)
        {
            this.callback = callback;
        }

        public Participant(TypedObject result)
        {
            base.SetFields<Participant>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<Participant>(this, result);
            this.callback(this);
        }

        public delegate void Callback(Participant result);
    }
}

