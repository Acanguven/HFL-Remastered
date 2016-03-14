namespace LoLLauncher
{
    using System;

    public enum CustomGameTypes
    {
        [StringValue("AllRandom")]
        AllRandom = 4,
        [StringValue("Battle Training")]
        BattleTraining = 10,
        [StringValue("Blind Draft")]
        BlindDraft = 6,
        [StringValue("Blind Duplicate")]
        BlindDuplicate = 13,
        [StringValue("Blind Pick")]
        BlindPick = 1,
        [StringValue("Blind Random")]
        BlindRandom = 12,
        [StringValue("Bugged Blind Pick")]
        BuggedBlindPick = 11,
        [StringValue("Draft")]
        Draft = 2,
        [StringValue("No Ban Draft")]
        NoBanDraft = 3,
        [StringValue("Tournament Draft")]
        TournamentDraft = 5,
        [StringValue("Tutorial")]
        Tutorial = 9,
        [StringValue("unknown")]
        Unknown1 = 0,
        [StringValue("unknown")]
        Unknown2 = 7,
        [StringValue("unknown")]
        Unknown3 = 8
    }
}

