namespace PVPNetConnect.RiotObjects
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class UnclassedObject : RiotGamesObject
    {
        private Callback callback;
        private string type = "";

        public UnclassedObject(Callback callback)
        {
            this.callback = callback;
        }

        public override void DoCallback(TypedObject result)
        {
            this.callback(result);
        }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TypedObject result);
    }
}

