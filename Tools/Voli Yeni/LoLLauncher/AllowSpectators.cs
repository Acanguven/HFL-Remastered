namespace LoLLauncher
{
    using System;

    public enum AllowSpectators
    {
        [StringValue("ALL")]
        All = 1,
        [StringValue("DROPINONLY")]
        DropInOnly = 3,
        [StringValue("LOBBYONLY")]
        LobbyOnly = 2,
        [StringValue("NONE")]
        None = 0
    }
}

