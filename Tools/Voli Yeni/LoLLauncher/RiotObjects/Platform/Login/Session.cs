namespace LoLLauncher.RiotObjects.Platform.Login
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Platform.Account;
    using System;
    using System.Runtime.CompilerServices;

    public class Session : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public Session()
        {
            this.type = "com.riotgames.platform.login.Session";
        }

        public Session(Callback callback)
        {
            this.type = "com.riotgames.platform.login.Session";
            this.callback = callback;
        }

        public Session(TypedObject result)
        {
            this.type = "com.riotgames.platform.login.Session";
            base.SetFields<Session>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<Session>(this, result);
            this.callback(this);
        }

        [InternalName("accountSummary")]
        public LoLLauncher.RiotObjects.Platform.Account.AccountSummary AccountSummary { get; set; }

        [InternalName("password")]
        public string Password { get; set; }

        [InternalName("token")]
        public string Token { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(Session result);
    }
}

