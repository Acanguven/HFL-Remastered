namespace LoLLauncher.RiotObjects.Platform.Trade
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class TradeContractDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public TradeContractDTO()
        {
            this.type = "com.riotgames.platform.trade.api.contract.TradeContractDTO";
        }

        public TradeContractDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.trade.api.contract.TradeContractDTO";
            this.callback = callback;
        }

        public TradeContractDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.trade.api.contract.TradeContractDTO";
            base.SetFields<TradeContractDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<TradeContractDTO>(this, result);
            this.callback(this);
        }

        [InternalName("requesterChampionId")]
        public double RequesterChampionId { get; set; }

        [InternalName("requesterInternalSummonerName")]
        public string RequesterInternalSummonerName { get; set; }

        [InternalName("responderChampionId")]
        public double ResponderChampionId { get; set; }

        [InternalName("responderInternalSummonerName")]
        public string ResponderInternalSummonerName { get; set; }

        [InternalName("state")]
        public string State { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(TradeContractDTO result);
    }
}

