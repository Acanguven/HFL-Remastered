namespace LoLLauncher
{
    using System;

    public enum GameType
    {
        [StringValue("COOP_VS_AI")]
        CoopVsAi = 7,
        [StringValue("CUSTOM_GAME")]
        CustomGame = 3,
        [StringValue("NORMAL_GAME")]
        NormalGame = 2,
        [StringValue("PRACTICE_GAME")]
        PracticeGame = 5,
        [StringValue("RANKED_GAME")]
        RankedGame = 1,
        [StringValue("RANKED_GAME_PREMADE")]
        RankedGamePremade = 8,
        [StringValue("RANKED_GAME_SOLO")]
        RankedGameSolo = 6,
        [StringValue("RANKED_TEAM_GAME")]
        RankedTeamGame = 0,
        [StringValue("TUTORIAL_GAME")]
        TutorialGame = 4
    }
}

