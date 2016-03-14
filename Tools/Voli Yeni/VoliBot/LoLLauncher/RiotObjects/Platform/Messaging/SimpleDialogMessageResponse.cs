namespace VoliBot.LoLLauncher.RiotObjects.Platform.Messaging
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class SimpleDialogMessageResponse : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public SimpleDialogMessageResponse()
        {
            this.type = "com.riotgames.platform.messaging.persistence.SimpleDialogMessageResponse";
        }

        public SimpleDialogMessageResponse(TypedObject result)
        {
            this.type = "com.riotgames.platform.messaging.persistence.SimpleDialogMessageResponse";
            base.SetFields<SimpleDialogMessageResponse>(this, result);
        }

        public SimpleDialogMessageResponse(Callback callback)
        {
            this.type = "com.riotgames.platform.messaging.persistence.SimpleDialogMessageResponse";
            this.callback = callback;
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<SimpleDialogMessageResponse>(this, result);
            this.callback(this);
        }

        [InternalName("accountId")]
        public double AccountID { get; set; }

        [InternalName("command")]
        public string Command { get; set; }

        [InternalName("msgId")]
        public double MsgID { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(SimpleDialogMessageResponse result);
    }
}

