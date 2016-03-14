namespace LoLLauncher.RiotObjects.Platform.Harassment
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class LcdsResponseString : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public LcdsResponseString()
        {
            this.type = "com.riotgames.platform.harassment.LcdsResponseString";
        }

        public LcdsResponseString(Callback callback)
        {
            this.type = "com.riotgames.platform.harassment.LcdsResponseString";
            this.callback = callback;
        }

        public LcdsResponseString(TypedObject result)
        {
            this.type = "com.riotgames.platform.harassment.LcdsResponseString";
            base.SetFields<LcdsResponseString>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LcdsResponseString>(this, result);
            this.callback(this);
        }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("value")]
        public string Value { get; set; }

        public delegate void Callback(LcdsResponseString result);
    }
}

