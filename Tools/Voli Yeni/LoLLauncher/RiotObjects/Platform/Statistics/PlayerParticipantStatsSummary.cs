namespace LoLLauncher.RiotObjects.Platform.Statistics
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PlayerParticipantStatsSummary : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerParticipantStatsSummary()
        {
            this.type = "com.riotgames.platform.statistics.PlayerParticipantStatsSummary";
        }

        public PlayerParticipantStatsSummary(Callback callback)
        {
            this.type = "com.riotgames.platform.statistics.PlayerParticipantStatsSummary";
            this.callback = callback;
        }

        public PlayerParticipantStatsSummary(TypedObject result)
        {
            this.type = "com.riotgames.platform.statistics.PlayerParticipantStatsSummary";
            base.SetFields<PlayerParticipantStatsSummary>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerParticipantStatsSummary>(this, result);
            this.callback(this);
        }

        [InternalName("botPlayer")]
        public bool BotPlayer { get; set; }

        [InternalName("elo")]
        public int Elo { get; set; }

        [InternalName("eloChange")]
        public int EloChange { get; set; }

        [InternalName("gameId")]
        public double GameId { get; set; }

        [InternalName("leaver")]
        public bool Leaver { get; set; }

        [InternalName("leaves")]
        public double Leaves { get; set; }

        [InternalName("level")]
        public double Level { get; set; }

        [InternalName("losses")]
        public double Losses { get; set; }

        [InternalName("profileIconId")]
        public int ProfileIconId { get; set; }

        [InternalName("skinName")]
        public string SkinName { get; set; }

        [InternalName("spell1Id")]
        public double Spell1Id { get; set; }

        [InternalName("spell2Id")]
        public double Spell2Id { get; set; }

        [InternalName("statistics")]
        public List<RawStatDTO> Statistics { get; set; }

        [InternalName("summonerName")]
        public string SummonerName { get; set; }

        [InternalName("teamId")]
        public double TeamId { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        [InternalName("userId")]
        public double UserId { get; set; }

        [InternalName("wins")]
        public double Wins { get; set; }

        public delegate void Callback(PlayerParticipantStatsSummary result);
    }
}

