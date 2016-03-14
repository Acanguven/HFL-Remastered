namespace LoLLauncher.RiotObjects.Platform.Login
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class AuthenticationCredentials : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public AuthenticationCredentials()
        {
            this.type = "com.riotgames.platform.login.AuthenticationCredentials";
        }

        public AuthenticationCredentials(Callback callback)
        {
            this.type = "com.riotgames.platform.login.AuthenticationCredentials";
            this.callback = callback;
        }

        public AuthenticationCredentials(TypedObject result)
        {
            this.type = "com.riotgames.platform.login.AuthenticationCredentials";
            base.SetFields<AuthenticationCredentials>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<AuthenticationCredentials>(this, result);
            this.callback(this);
        }

        [InternalName("authToken")]
        public string AuthToken { get; set; }

        [InternalName("clientVersion")]
        public string ClientVersion { get; set; }

        [InternalName("domain")]
        public string Domain { get; set; }

        [InternalName("ipAddress")]
        public string IpAddress { get; set; }

        [InternalName("locale")]
        public string Locale { get; set; }

        [InternalName("oldPassword")]
        public object OldPassword { get; set; }

        [InternalName("operatingSystem")]
        public string OperatingSystem { get; set; }

        [InternalName("partnerCredentials")]
        public object PartnerCredentials { get; set; }

        [InternalName("password")]
        public string Password { get; set; }

        [InternalName("securityAnswer")]
        public object SecurityAnswer { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("username")]
        public string Username { get; set; }

        public delegate void Callback(AuthenticationCredentials result);
    }
}

