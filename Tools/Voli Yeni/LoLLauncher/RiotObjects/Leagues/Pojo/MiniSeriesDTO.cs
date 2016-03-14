namespace LoLLauncher.RiotObjects.Leagues.Pojo
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class MiniSeriesDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public MiniSeriesDTO()
        {
            this.type = "com.riotgames.leagues.pojo.MiniSeriesDTO";
        }

        public MiniSeriesDTO(Callback callback)
        {
            this.type = "com.riotgames.leagues.pojo.MiniSeriesDTO";
            this.callback = callback;
        }

        public MiniSeriesDTO(TypedObject result)
        {
            this.type = "com.riotgames.leagues.pojo.MiniSeriesDTO";
            base.SetFields<MiniSeriesDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<MiniSeriesDTO>(this, result);
            this.callback(this);
        }

        [InternalName("losses")]
        public int Losses { get; set; }

        [InternalName("progress")]
        public string Progress { get; set; }

        [InternalName("target")]
        public int Target { get; set; }

        [InternalName("timeLeftToPlayMillis")]
        public double TimeLeftToPlayMillis { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("wins")]
        public int Wins { get; set; }

        public delegate void Callback(MiniSeriesDTO result);
    }
}

