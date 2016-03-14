namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GameDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public GameDTO()
        {
            this.type = "com.riotgames.platform.game.GameDTO";
        }

        public GameDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.game.GameDTO";
            this.callback = callback;
        }

        public GameDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.GameDTO";
            base.SetFields<GameDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<GameDTO>(this, result);
            this.callback(this);
        }

        [InternalName("bannedChampions")]
        public List<BannedChampion> BannedChampions { get; set; }

        [InternalName("banOrder")]
        public List<int> BanOrder { get; set; }

        [InternalName("expiryTime")]
        public double ExpiryTime { get; set; }

        [InternalName("gameMode")]
        public string GameMode { get; set; }

        [InternalName("gameState")]
        public string GameState { get; set; }

        [InternalName("gameStateString")]
        public string GameStateString { get; set; }

        [InternalName("gameType")]
        public string GameType { get; set; }

        [InternalName("gameTypeConfigId")]
        public int GameTypeConfigId { get; set; }

        [InternalName("glmGameId")]
        public object GlmGameId { get; set; }

        [InternalName("glmHost")]
        public object GlmHost { get; set; }

        [InternalName("glmPort")]
        public int GlmPort { get; set; }

        [InternalName("glmSecurePort")]
        public int GlmSecurePort { get; set; }

        [InternalName("id")]
        public double Id { get; set; }

        [InternalName("joinTimerDuration")]
        public int JoinTimerDuration { get; set; }

        [InternalName("mapId")]
        public int MapId { get; set; }

        [InternalName("maxNumPlayers")]
        public int MaxNumPlayers { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("observers")]
        public List<GameObserver> Observers { get; set; }

        [InternalName("optimisticLock")]
        public double OptimisticLock { get; set; }

        [InternalName("ownerSummary")]
        public PlayerParticipant OwnerSummary { get; set; }

        [InternalName("passbackDataPacket")]
        public object PassbackDataPacket { get; set; }

        [InternalName("passbackUrl")]
        public object PassbackUrl { get; set; }

        [InternalName("passwordSet")]
        public bool PasswordSet { get; set; }

        [InternalName("pickTurn")]
        public int PickTurn { get; set; }

        [InternalName("playerChampionSelections")]
        public List<PlayerChampionSelectionDTO> PlayerChampionSelections { get; set; }

        [InternalName("queuePosition")]
        public int QueuePosition { get; set; }

        [InternalName("queueTypeName")]
        public string QueueTypeName { get; set; }

        [InternalName("roomName")]
        public string RoomName { get; set; }

        [InternalName("roomPassword")]
        public string RoomPassword { get; set; }

        [InternalName("spectatorDelay")]
        public int SpectatorDelay { get; set; }

        [InternalName("spectatorsAllowed")]
        public string SpectatorsAllowed { get; set; }

        [InternalName("statusOfParticipants")]
        public string StatusOfParticipants { get; set; }

        [InternalName("teamOne")]
        public List<Participant> TeamOne { get; set; }

        [InternalName("teamTwo")]
        public List<Participant> TeamTwo { get; set; }

        [InternalName("terminatedCondition")]
        public string TerminatedCondition { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(GameDTO result);
    }
}

