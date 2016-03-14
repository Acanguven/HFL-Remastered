namespace LoLLauncher.RiotObjects.Platform.Account
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class AccountSummary : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public AccountSummary()
        {
            this.type = "com.riotgames.platform.account.AccountSummary";
        }

        public AccountSummary(Callback callback)
        {
            this.type = "com.riotgames.platform.account.AccountSummary";
            this.callback = callback;
        }

        public AccountSummary(TypedObject result)
        {
            this.type = "com.riotgames.platform.account.AccountSummary";
            base.SetFields<AccountSummary>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AccountSummary>(this, result);
            this.callback(this);
        }

        [InternalName("accountId")]
        public double AccountId { get; set; }

        [InternalName("admin")]
        public bool Admin { get; set; }

        [InternalName("groupCount")]
        public int GroupCount { get; set; }

        [InternalName("hasBetaAccess")]
        public bool HasBetaAccess { get; set; }

        [InternalName("needsPasswordReset")]
        public bool NeedsPasswordReset { get; set; }

        [InternalName("partnerMode")]
        public bool PartnerMode { get; set; }

        [InternalName("summonerInternalName")]
        public object SummonerInternalName { get; set; }

        [InternalName("summonerName")]
        public object SummonerName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("username")]
        public string Username { get; set; }

        public delegate void Callback(AccountSummary result);
    }
}

