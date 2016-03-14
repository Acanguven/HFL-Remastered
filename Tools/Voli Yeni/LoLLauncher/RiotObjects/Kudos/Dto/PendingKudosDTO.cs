namespace LoLLauncher.RiotObjects.Kudos.Dto
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PendingKudosDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PendingKudosDTO()
        {
            this.type = "com.riotgames.kudos.dto.PendingKudosDTO";
        }

        public PendingKudosDTO(Callback callback)
        {
            this.type = "com.riotgames.kudos.dto.PendingKudosDTO";
            this.callback = callback;
        }

        public PendingKudosDTO(TypedObject result)
        {
            this.type = "com.riotgames.kudos.dto.PendingKudosDTO";
            base.SetFields<PendingKudosDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PendingKudosDTO>(this, result);
            this.callback(this);
        }

        [InternalName("pendingCounts")]
        public int[] PendingCounts { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PendingKudosDTO result);
    }
}

