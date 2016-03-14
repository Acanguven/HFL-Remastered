namespace LoLLauncher.RiotObjects.Platform.Summoner
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Summoner : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public Summoner()
        {
            this.type = "com.riotgames.platform.summoner.Summoner";
        }

        public Summoner(Callback callback)
        {
            this.type = "com.riotgames.platform.summoner.Summoner";
            this.callback = callback;
        }

        public Summoner(TypedObject result)
        {
            this.type = "com.riotgames.platform.summoner.Summoner";
            base.SetFields<LoLLauncher.RiotObjects.Platform.Summoner.Summoner>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<LoLLauncher.RiotObjects.Platform.Summoner.Summoner>(this, result);
            this.callback(this);
        }

        [InternalName("acctId")]
        public double AcctId { get; set; }

        [InternalName("advancedTutorialFlag")]
        public bool AdvancedTutorialFlag { get; set; }

        [InternalName("displayEloQuestionaire")]
        public bool DisplayEloQuestionaire { get; set; }

        [InternalName("helpFlag")]
        public bool HelpFlag { get; set; }

        [InternalName("internalName")]
        public string InternalName { get; set; }

        [InternalName("lastGameDate")]
        public DateTime LastGameDate { get; set; }

        [InternalName("name")]
        public string Name { get; set; }

        [InternalName("nameChangeFlag")]
        public bool NameChangeFlag { get; set; }

        [InternalName("profileIconId")]
        public int ProfileIconId { get; set; }

        [InternalName("revisionDate")]
        public DateTime RevisionDate { get; set; }

        [InternalName("revisionId")]
        public double RevisionId { get; set; }

        [InternalName("seasonOneTier")]
        public string SeasonOneTier { get; set; }

        [InternalName("seasonTwoTier")]
        public string SeasonTwoTier { get; set; }

        [InternalName("socialNetworkUserIds")]
        public List<object> SocialNetworkUserIds { get; set; }

        [InternalName("sumId")]
        public double SumId { get; set; }

        [InternalName("tutorialFlag")]
        public bool TutorialFlag { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(LoLLauncher.RiotObjects.Platform.Summoner.Summoner result);
    }
}

