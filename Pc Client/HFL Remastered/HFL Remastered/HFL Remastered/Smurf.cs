using LoLLauncher;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace HFL_Remastered
{
    public class Smurf : IDisposable
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

        public Timer liveTimer = new Timer();
        private int errorTimer = 10;
        private volatile bool _requestStop = false;

        public string regionVersion = "6.3.16_02_03_18_43";
        internal BotThread thread = new BotThread();

        public Smurf()
        {

        }

        public void updateTimer(int amount)
        {
            if(amount > errorTimer) { 
                errorTimer = amount;
            }
        }

        public async void inviteMe(double summonerId){
            thread.lobbyInviteQuery.Add(summonerId);
            thread.lobbyInviteUpdate();
        }

        public void loadSelf()
        {
            Logger.Push("Loading components...", "info", username);
            liveTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            liveTimer.Interval = 1000;
            liveTimer.Enabled = true;
            liveTimer.Start();
            thread.init(username, password, desiredLevel, region, queue, this);
        }

        public void restart()
        {
            stop();
            thread = new BotThread();
            loadSelf();
            start();
        }

        public void restartHard()
        {
            App.gameContainer.terminateUserGame(this.username);
            restart();
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
                    thread.Dispose();
                    thread = null;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void updateSelfOnRemote()
        {
            Network.updateSmurfData(this);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (!_requestStop)
            {
                errorTimer--;
                if (errorTimer <= 0)
                {
                    Smurf containsSmurf = SmurfManager.smurfs.First(pendSmurf => pendSmurf.username == username && pendSmurf.region == region);
                    if (containsSmurf != null)
                    {
                        Logger.Push("Restarting smurf because didn't recieve any message for a long time!", "warning", username);
                        errorTimer = 15;
                        restartHard();
                    }
                }
            }
        }

        public void removeTimer()
        {
            _requestStop = true;
            liveTimer.Stop();
        }

        public void Dispose()
        {
            removeTimer();
            GC.SuppressFinalize(this);
        }
    }
}
