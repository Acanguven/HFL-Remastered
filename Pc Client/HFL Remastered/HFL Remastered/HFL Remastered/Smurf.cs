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
            Logger.Log newLog = new Logger.Log("info");
            newLog.Smurf = this.username;
            newLog.Text = "Loading components...";
            Logger.Push(newLog);

            thread.init(username, password, desiredLevel, region, queue, this, regionVersion);
        }

        public void updateRegion(string newRegionInformation)
        {
            regionVersion = newRegionInformation;
            Logger.Log newLog = new Logger.Log("warning");
            newLog.Smurf = this.username;
            newLog.Text = "Region information updated, restarting...";
            Logger.Push(newLog);
            stop();
            thread = new BotThread();
            loadSelf();
            start();
        }

        public void start()
        {
            Logger.Log newLog = new Logger.Log("warning");
            newLog.Smurf = this.username;
            newLog.Text = "Trying to start smurf.";
            Logger.Push(newLog);
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
            Logger.Log newLog = new Logger.Log("warning");
            newLog.Smurf = this.username;
            newLog.Text = "Stopping smurf";
            Logger.Push(newLog);
        }

        public void restart()
        {
            loadSelf();
            start();
        }

        public void updateExpLevel(double extToNextLevel, double currentExp, double level)
        {

        }
    }
}
