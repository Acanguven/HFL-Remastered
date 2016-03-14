namespace LoLLauncher
{
    using System;

    [Flags]
    public enum GameLobbyGuiState
    {
        CHAMP_SELECT = 4,
        CHAMP_SELECT_CLIENT = 0x2000,
        DISCONNECTED = 0x20000,
        GAME_IN_PROGRESS = 0x8000,
        GAME_START_CLIENT = 0x40,
        GameClientConnectedToServer = 0x80,
        GameReconnect = 0x4000,
        IDLE = 1,
        IN_PROGRESS = 0x100,
        IN_QUEUE = 0x200,
        JOINING_CHAMP_SELECT = 0x10000,
        POST_CHAMP_SELECT = 8,
        POST_GAME = 0x400,
        PRE_CHAMP_SELECT = 0x10,
        START_REQUESTED = 0x20,
        TEAM_SELECT = 2,
        TERMINATED = 0x800,
        TERMINATED_IN_ERROR = 0x1000
    }
}

