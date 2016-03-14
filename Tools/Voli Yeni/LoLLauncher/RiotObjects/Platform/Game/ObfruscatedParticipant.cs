namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class ObfruscatedParticipant : Participant
    {
        private Callback callback;
        private string type;

        public ObfruscatedParticipant()
        {
            this.type = "com.riotgames.platform.game.ObfruscatedParticipant";
        }

        public ObfruscatedParticipant(Callback callback)
        {
            this.type = "com.riotgames.platform.game.ObfruscatedParticipant";
            this.callback = callback;
        }

        public ObfruscatedParticipant(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.ObfruscatedParticipant";
            base.SetFields<ObfruscatedParticipant>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<ObfruscatedParticipant>(this, result);
            this.callback(this);
        }

        [InternalName("badges")]
        public int Badges { get; set; }

        [InternalName("clientInSynch")]
        public bool ClientInSynch { get; set; }

        [InternalName("gameUniqueId")]
        public int GameUniqueId { get; set; }

        [InternalName("index")]
        public int Index { get; set; }

        [InternalName("pickMode")]
        public int PickMode { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(ObfruscatedParticipant result);
    }
}

