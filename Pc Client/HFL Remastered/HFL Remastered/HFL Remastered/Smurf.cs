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

        [JsonProperty("currentLevel")]
        public dynamic currentLevel { get; set; }

        [JsonProperty("currentip")]
        public dynamic currentip { get; set; }

        [JsonProperty("currentrp")]
        public dynamic currentrp { get; set; }

        public bool groupMember = false;
        public bool isHost = false;
        public List<double> inviteList = new List<double>();
        public int totalGroupLength { get; set; }
        public Smurf hostCallback { get; set; }

        public string regionVersion = "6.3.16_02_03_18_43";
        internal BotThread thread = new BotThread();

        public async void inviteMe(double summonerId){
            thread.lobbyInviteQuery.Add(summonerId);
            thread.lobbyInviteUpdate();
        }

        public void loadSelf()
        {
            Logger.Push("Loading components...", "info", username);
            thread.init(username, password, desiredLevel, region, queue, this, regionVersion);
        }

        public void updateRegion(string newRegionInformation)
        {
            regionVersion = newRegionInformation;

            Logger.Push("Region information updated, restarting...", "info", this.username);
            stop();
            thread = new BotThread();
            loadSelf();
            start();
        }

        public void start()
        {
            Logger.Push("Trying to start smurf.", "info", this.username);
            thread.start();
        }

        public void stop()
        {
            try
            {
                if (thread != null)
                {
                    if (thread.connection.IsConnected())
                    {
                        thread.connection.Disconnect();
                    }
                    thread = null;
                }
            }
            catch (Exception ex)
            {

            }
            Logger.Push("Stopping smurf", "warning", this.username);
        }

        public void restart()
        {
            loadSelf();
            start();
        }

        public void updateSelfOnRemote()
        {
            Network.updateSmurfData(this);
        }
    }
}
