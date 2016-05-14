using System;

namespace HFL_3._0
{

    [Serializable]
    public class SmurfData
	{
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public LoLLauncher.Region region { get; set; }
        public double desiredLevel { get; set; }
        public double currentLevel { get; set; }
        public double currentRp { get; set; }
        public double currentIp { get; set; }
        public double expToNextLevel { get; set; }
        public double currentXp { get; set; }
        public double summonerId { get; set; }
        public string summonerName { get; set; }
        public string queue { get; set; }
    }
}
