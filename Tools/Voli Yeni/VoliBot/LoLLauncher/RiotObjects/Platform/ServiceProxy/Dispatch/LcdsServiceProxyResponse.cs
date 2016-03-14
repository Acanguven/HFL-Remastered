namespace VoliBot.LoLLauncher.RiotObjects.Platform.ServiceProxy.Dispatch
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class LcdsServiceProxyResponse : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public LcdsServiceProxyResponse()
        {
            this.type = "com.riotgames.platform.serviceproxy.dispatch.LcdsServiceProxyResponse";
        }

        public LcdsServiceProxyResponse(TypedObject result)
        {
            this.type = "com.riotgames.platform.serviceproxy.dispatch.LcdsServiceProxyResponse";
            base.SetFields<LcdsServiceProxyResponse>(this, result);
        }

        public LcdsServiceProxyResponse(Callback callback)
        {
            this.type = "com.riotgames.platform.serviceproxy.dispatch.LcdsServiceProxyResponse";
            this.callback = callback;
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LcdsServiceProxyResponse>(this, result);
            this.callback(this);
        }

        [InternalName("messageId")]
        public string messageId { get; set; }

        [InternalName("methodName")]
        public string MethodName { get; set; }

        [InternalName("payload")]
        public string Payload { get; set; }

        [InternalName("serviceName")]
        public string ServiceName { get; set; }

        [InternalName("status")]
        public string Status { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(LcdsServiceProxyResponse result);
    }
}

