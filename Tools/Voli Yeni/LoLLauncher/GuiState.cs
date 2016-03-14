namespace LoLLauncher
{
    using System;

    [Flags]
    public enum GuiState
    {
        CustomCreateGame = 0x20,
        CustomSearchGame = 0x10,
        GameLobby = 0x40,
        LoggedIn = 8,
        LoggedOut = 2,
        LoggingIn = 4,
        None = 1
    }
}

