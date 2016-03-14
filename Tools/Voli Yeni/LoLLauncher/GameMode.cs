namespace LoLLauncher
{
    using System;

    public enum GameMode
    {
        [StringValue("ODIN")]
        Dominion = 8,
        [StringValue("ARAM")]
        HowlingAbyss = 12,
        [StringValue("CLASSIC")]
        SummonersRift = 1,
        [StringValue("TUTORIAL")]
        Tutorial = 13,
        [StringValue("CLASSIC")]
        TwistedTreeline = 10
    }
}

