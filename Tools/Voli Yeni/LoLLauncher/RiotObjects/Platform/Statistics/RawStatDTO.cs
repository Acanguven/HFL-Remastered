namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class RawStatDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public RawStatDTO()
        {
            this.type = "com.riotgames.platform.statistics.RawStatDTO";
        }

        public RawStatDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.RawStatDTO";
            this.callback = callback;
        }

        public RawStatDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.RawStatDTO";
            base.SetFields<RawStatDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<RawStatDTO>(this, result);
            this.callback(this);
        }

        [InternalName("statTypeName")]
        public string StatTypeName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("value")]
        public double Value { get; set; }

        public delegate void Callback(RawStatDTO result);
    }
}

