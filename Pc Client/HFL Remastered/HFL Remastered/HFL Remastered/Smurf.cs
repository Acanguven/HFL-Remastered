using LoLLauncher;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFL_Remastered
{
    public class Smurf
    {
        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("region")]
        public Region region { get; set; }

        [JsonProperty("queue")]
        public QueueTypes queue { get; set; }

        [JsonProperty("desiredLevel")]
        public int desiredLevel { get; set; }

        public bool groupMember { get; set; }
        public BotThread thread { get; set; }

        public void loadSelf()
        {
            Logger.Log newLog = new Logger.Log("info");
            newLog.Smurf = this.username;
            newLog.Text = "Loading components...";
            Logger.Push(newLog);
        }
    }
}
