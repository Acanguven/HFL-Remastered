using Newtonsoft.Json;

namespace HFL_Remastered
{
    public class User
    {
        [JsonProperty("date")]
        public long Date { get; set; }

        public string Hwid { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("forumData")]
        public forumData ForumData { get; set; }

        [JsonProperty("userData")]
        public userData UserData { get; set; } 

        User(){
            Token = "";
            Hwid = HWID.Generate();
        }
    }

    public class forumData
    {
        [JsonProperty("username")]
        public string Name { get; set; }
    }

    public class userData
    {
        [JsonProperty("trial")]
        public long Trial { get; set; }
        [JsonProperty("hwidCanChange")]
        public long HwidCanChange { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("settings")]
        public settings Settings { get; set; }
    }

    public class settings
    {
        [JsonProperty("packetSearch")]
        public bool PacketSearch { get; set; }

        [JsonProperty("buyBoost")]
        public bool BuyBoost { get; set; }

        [JsonProperty("reconnect")]
        public bool Reconnect { get; set; }

        [JsonProperty("disableGpu")]
        public bool DisableGpu { get; set; }

        [JsonProperty("manualInjection")]
        public bool ManualInjection { get; set; }
    }
}